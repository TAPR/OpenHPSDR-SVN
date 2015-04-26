package org.g0orx.openhpsdr;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.NetworkInterface;
import java.net.SocketException;
import java.net.UnknownHostException;
import java.util.ArrayList;
import java.util.Enumeration;

import javax.sound.sampled.AudioFormat;
import javax.sound.sampled.AudioSystem;
import javax.sound.sampled.DataLine;
import javax.sound.sampled.Line;
import javax.sound.sampled.LineUnavailableException;
import javax.sound.sampled.Mixer;
import javax.sound.sampled.SourceDataLine;

import org.g0orx.openhpsdr.debug.Log;
import org.g0orx.openhpsdr.discovery.Discovered;
import org.g0orx.openhpsdr.wdsp.WDSP;

public class Metis extends Thread {

    public Metis(Radio radio, boolean bandscope) {

        this.radio = radio;
        this.receivers = radio.getReceivers();
        this.discovered = radio.getDiscovered();
        this.id = radio.getId();
        this.receiverbase = id * 4; // 2 per radio
        this.bandscope = bandscope;

        Log.i(this,"bandscope="+bandscope);
        this.frame=(receivers*6)+2;
        this.frames=(512-8)/frame;
        this.data=8+(frames*frame);

        myport = myport + id + 1;

        Log.i(this, "using: " + this.discovered.toString() + " receivers=" + this.receivers + " myport=" + myport);

        txcontrol1 = (byte) (CONFIG_BOTH | MIC_SOURCE_PENELOPE | radio.clock10 | radio.clock122);
        txcontrol3 = (byte) (ALEX_ATTENUATION_0DB | radio.dither | radio.random | radio.preamp);
        txcontrol4 = (byte) (DUPLEX | (((receivers - 1) << 3) & 0x038));

        Log.i(this, "txcontrol4=" + Integer.toHexString(txcontrol4 & 0xFF));
        // set speed and output buffer size
        if (radio.samplerate == 48000.0) {
            outsize = radio.buffersize;
            txcontrol1 |= SPEED_48KHZ;
        } else if (radio.samplerate == 96000.0) {
            outsize = radio.buffersize / 2;
            txcontrol1 |= SPEED_96KHZ;
        } else if (radio.samplerate == 192000.0) {
            outsize = radio.buffersize / 4;
            txcontrol1 |= SPEED_192KHZ;
        } else if (radio.samplerate == 384000.0) {
            outsize = radio.buffersize / 8;
            txcontrol1 |= SPEED_384KHZ;
        }

        Log.i(this, "samplerate: " + radio.samplerate + " buffersize=" + radio.buffersize + " fftsize=" + radio.fftsize + " outsize=" + outsize);

        this.wdsp = WDSP.getInstance();

        // allocate space for the input/output samples
        inlsamples = new float[this.receivers][radio.buffersize];
        inrsamples = new float[this.receivers][radio.buffersize];
        outlsamples = new float[this.receivers][outsize];
        outrsamples = new float[this.receivers][outsize];
        inmiclsamples = new float[radio.buffersize];
        inmicrsamples = new float[radio.buffersize];
        outmiclsamples = new float[outsize];
        outmicrsamples = new float[outsize];

        if (bandscope) {
            bslsamples = new float[BS_BUFFER_SIZE];
            bsrsamples = new float[BS_BUFFER_SIZE];
            for (int i = 0; i < BS_BUFFER_SIZE; i++) {
                bsrsamples[i] = 0.0F;
            }
        }

        // build the toaddress InetAddress
        try {
            toaddress = InetAddress.getByName(this.discovered.getAddress());
        } catch (Exception e) {
            Log.i(this, "constructor: " + e.toString());
        }

        // create the DatagramPackets for commands and samples
        commanddatagram = new DatagramPacket(commandbuffer, commandbuffer.length, toaddress, toport);
        samplesdatagram = new DatagramPacket(sendbuffer, sendbuffer.length, toaddress, toport);

        if (radio.audiooutput == Radio.AUDIO_OUTPUT_LOCAL || radio.audiooutput == Radio.AUDIO_OUTPUT_BOTH) {
            initializeLocalAudioOutput();
        }

        // Calculate sample values for baseband CW note^M
        double deltaf = Math.PI / 40.0;     // (2 PI f / 48k) gives an 600 Hz note at 48 ksps
        for (int i = 0; i < 240; ++i) {
            cwnotesin[i] = (float) ((Math.pow(2, 15) - 1) * Math.sin(deltaf * i) / 32767.0);
            cwnotecos[i] = (float) ((Math.pow(2, 15) - 1) * Math.cos(deltaf * i) / 32767.0);
        }

    }

    public void setActiveReceiver(int r) {
        Log.i(this, "setActiveReceiver: " + r);
        this.activeReceiver = r;
    }

    public void initializeLocalAudioOutput() {

        Log.i(this, "initializeLocalAudioOutput");
        try {
            audioformat = new AudioFormat(AudioFormat.Encoding.PCM_SIGNED, 48000F, 16, 2, 4, 48000F, true);
            audioline = AudioSystem.getSourceDataLine(audioformat);
            audioline.open(audioformat);
            audioline.start();
        } catch (Exception e) {
            Log.i(this, "initializeLocalAudioOutput: " + e.toString());
        }

    }

    public void stopLocalAudioOutput() {
        Log.i(this, "stopLocalAudioOutput");
        if (audioline != null) {
            audioline.flush();
            audioline.close();
            audioline = null;
        }
    }

    public void setPTTListener(PTTListener listener) {
        this.pttListener = listener;
    }

    public void run() {
        int status;
        long sequence;
        int endpoint;
        Log.i(this, "run: " + this.getName());

        myaddress = this.discovered.getInterface();
        Log.i(this, "run: myAddress:" + myaddress + " (" + this.discovered.getInterfaceName() + ")");

        running = true;

        time = System.currentTimeMillis();

        try {
            InetSocketAddress socketaddress = new InetSocketAddress(myaddress, myport);

            //Log.i(this,"run: socketaddress: "+socketaddress.toString());
            socket = new DatagramSocket(null);
            socket.setReuseAddress(true);
            socket.setBroadcast(true);
            socket.setSoTimeout(0);
            socket.bind(socketaddress);
            
            
            InetAddress address = InetAddress.getByName(this.discovered.getAddress());
            rxdatagram = new DatagramPacket(rxbuffer, rxbuffer.length, address, toport);

            prime();

            while (running) {
                socket.receive(rxdatagram);
                if (rxdatagram.getLength() == 1032 && (rxbuffer[0] & 0xFF) == 0xEF && (rxbuffer[1] & 0xFF) == 0xFE) {
                    status = rxbuffer[2] & 0xFF;
                    if (status == 1) {
                        endpoint = rxbuffer[3] & 0xFF;
                        if (endpoint == 6) {
                            sequence = ((rxbuffer[4] & 0xFF) << 24)
                                    | ((rxbuffer[5] & 0xFF) << 16)
                                    | ((rxbuffer[6] & 0xFF) << 8)
                                    | ((rxbuffer[7] & 0xFF));
                            packetsreceived++;
                            ep6sequence++;
                            if (sequence != ep6sequence) {
                                packetsmissed += (sequence - ep6sequence);
                                //Log.i(this,"run: EP6 sequence error: expected:"+ep6sequence+" got:"+sequence);
                                ep6sequence = sequence;
                            }
                            demuxBuffer(rxbuffer, 8);
                            demuxBuffer(rxbuffer, 520);
                        } else if (endpoint == 4) {
                            if(bandscope) {
                                processBandscope(rxbuffer, 8);
                                processBandscope(rxbuffer, 520);
                            }
                        }
                    } else {
                        Log.i(this, "run: received unknown status: " + status);
                    }
                } else {
                    Log.i(this, "run: received unknown packet: length:" + rxdatagram.getLength());
                    for (int i = 0; i < rxdatagram.getLength(); i++) {
                        Log.i(this, Integer.toString(i) + ":" + Integer.toHexString(rxbuffer[i] & 0xFF));
                    }
                }
            }

            // send stop command
            commandbuffer[0] = (byte) 0xEF;
            commandbuffer[1] = (byte) 0xFE;
            commandbuffer[2] = (byte) 0x04;
            commandbuffer[3] = (byte) 0x00;
            for (int i = 4; i < 64; i++) {
                commandbuffer[i] = (byte) 0x00;
            }
            sendCommand();

        } catch (SocketException se) {
            Log.i(this, "run: " + se.toString());
            se.printStackTrace();
        } catch (UnknownHostException uhe) {
            Log.i(this, "run: " + uhe.toString());
        } catch (IOException ioe) {
            Log.i(this, "run: " + ioe.toString());
        }

        socket.close();

        //wdsp.DestroyAnalyzer(Display.RX0);
        //wdsp.DestroyAnalyzer(Display.TX);
        //wdsp.DestroyAnalyzer(Display.BS);
        stopLocalAudioOutput();

    }

    public void terminate() {
        running = false;
    }

    public boolean isRunning() {
        return running;
    }

    public void setTransmit(boolean transmit, boolean tuning) {
        //Log.i(this, "setTransmit: transmit=" + transmit + " tuning=" + tuning);
        this.transmit = transmit;
        this.tuning = tuning;
    }

    public boolean isTransmitting() {
        return this.transmit;
    }

    public boolean isTuning() {
        return this.tuning;
    }

    private void processBandscope(byte[] bytes, int offset) {
        if (running) {
            short sample;
            for (int i = offset; i < offset + 512; i += 2) {
                sample = (short) ((bytes[i + 1] << 8) + (bytes[i] & 0xFF));
                bslsamples[bsoffset++] = (float) sample / 32767.0F;
                if (bsoffset == bslsamples.length) {
                    wdsp.Spectrum(Display.BS, 0, 0, bslsamples, bsrsamples);
                    bsoffset = 0;
                }
            }
        }
    }

    private void demuxBuffer(byte[] bytes, int offset) {

        if (running) {
            Band band = radio.getReceiver(0).getBands().get(); //RX0 always used for transmit
            BandStack bandstack = band.get();
            // 512 byte buffer (startig at offset)
            //    0: SYNC
            //    1: SYNC
            //    2: SYNC
            //    3: Control 0
            //    4: Control 1
            //    5: Control 2
            //    6: Control 3
            //    7: Control 4

            //    8,9,10:   I   for 1 receiver  |
            //    11,12,13: Q   for 1 receiver  |  repeated 63 times from 1 receiver
            //    14,15:    MIC                 |
            boolean mydebug = dbg;
            dbg = false;
            state = STATE_SYNC0;
            int limit=offset+data;
            for (int i = offset; i < limit; i++) {
                switch (state) {
                    case STATE_SYNC0:
                        if (bytes[i] != SYNC) {
                            Log.i(this, "SYNC error: offset:" + offset + ":" + String.format("%02X", bytes[i]));
                            state = STATE_SYNC_ERROR;
                        } else {
                            state++;
                        }
                        break;
                    case STATE_SYNC1:
                        if (bytes[i] != SYNC) {
                            Log.i(this, "SYNC error: offset:" + offset + ":" + String.format("%02X", bytes[i]));
                            state = STATE_SYNC_ERROR;
                        } else {
                            state++;
                        }
                        break;
                    case STATE_SYNC2:
                        if (bytes[i] != SYNC) {
                            Log.i(this, "SYNC error: offset:" + offset + ":" + String.format("%02X", bytes[i]));
                            state = STATE_SYNC_ERROR;
                        } else {
                            state++;
                        }
                        break;
                    case STATE_CONTROL0:
                        rxcontrol0 = (byte) (bytes[i] & 0xFF);
                        state++;
                        break;
                    case STATE_CONTROL1:
                        rxcontrol1 = (byte) (bytes[i] & 0xFF);

                        state++;
                        break;
                    case STATE_CONTROL2:
                        rxcontrol2 = (byte) (bytes[i] & 0xFF);
                        state++;
                        break;
                    case STATE_CONTROL3:
                        rxcontrol3 = (byte) (bytes[i] & 0xFF);
                        state++;
                        break;
                    case STATE_CONTROL4:
                        rxcontrol4 = (byte) (bytes[i] & 0xFF);
                        ptt = (rxcontrol0 & 0x01) == 0x01;
                        dash = (rxcontrol0 & 0x02) == 0x02;
                        dot = (rxcontrol0 & 0x04) == 0x04;

                        if (ptt != last_ptt) {
                            if (bandstack.getMode() != Modes.CWL && bandstack.getMode() != Modes.CWU) {
                                if (pttListener != null) {
                                    pttListener.PTTChanged(ptt);
                                }
                            }
                            Log.i(this, "ptt changed=" + ptt);
                            last_ptt = ptt;
                        }
                        if (dash != last_dash) {
                            Log.i(this, "dash=" + dash);
                            last_dash = dash;
                        }
                        if (dot != last_dot) {
                            Log.i(this, "dot=" + dot);
                            last_dot = dot;
                        }

                        lt2208_overflow = (rxcontrol1 & 0x01) == 0x01;

                        byte address = (byte) ((rxcontrol0 >> 3) & 0x1F);
                        switch (address) {
                            case 0:
                                if (mercury_software_version != rxcontrol2) {
                                    mercury_software_version = rxcontrol2;
                                    //Log.i(this,"Mercury: "+rxcontrol2);
                                }
                                if (penelope_software_version != rxcontrol3) {
                                    penelope_software_version = rxcontrol3;
                                    //Log.i(this,"Penelope: "+rxcontrol3);
                                }
                                if (metis_software_version != rxcontrol4) {
                                    metis_software_version = rxcontrol4;
                                    //Log.i(this,"Metis: "+rxcontrol4);
                                }
                                break;
                            case 1:
                                // forward power
                                avg_penelope_forward_power += ((rxcontrol1 << 8) & 0xFF00) | (rxcontrol2 & 0xFF);
                                avg_alex_forward_power += ((rxcontrol3 << 8) & 0xFF00) | (rxcontrol4 & 0xFF);
                                break;
                            case 2:
                                // reverse power
                                avg_alex_reverse_power += ((rxcontrol1 << 8) & 0xFF00) | (rxcontrol2 & 0xFF);
                                ain3 = (rxcontrol3 << 8) + (rxcontrol4 & 0xFF);
                                power_samples++;
                                if (power_samples == 4) {
                                    penelope_forward_power = avg_penelope_forward_power / 4;
                                    alex_forward_power = avg_alex_forward_power / 4;
                                    alex_reverse_power = avg_alex_reverse_power / 4;

                                    avg_penelope_forward_power = 0;
                                    avg_alex_forward_power = 0;
                                    avg_alex_reverse_power = 0;
                                    power_samples = 0;
                                }
                                break;
                            case 3:
                                ain4 = (rxcontrol1 << 8) + (rxcontrol2 & 0xFF);
                                ain6 = (rxcontrol3 << 8) + (rxcontrol4 & 0xFF);
                                break;
                            case 4:
                                adc1overflow = (rxcontrol1 & 0x01) == 0x01;
                                adc2overflow = (rxcontrol2 & 0x01) == 0x01;
                                adc3overflow = (rxcontrol3 & 0x01) == 0x01;
                                adc4overflow = (rxcontrol4 & 0x01) == 0x01;
                                break;
                        }
                        state++;
                        break;
                    case STATE_I0:
                        isample = bytes[i] << 16; // keep sign
                        state++;
                        break;
                    case STATE_I1:
                        isample |= (bytes[i] & 0xFF) << 8;
                        state++;
                        break;
                    case STATE_I2:
                        isample |= bytes[i] & 0xFF;
                        state++;
                        break;
                    case STATE_Q0:
                        qsample = bytes[i] << 16; // keep sign
                        state++;
                        break;
                    case STATE_Q1:
                        qsample |= (bytes[i] & 0xFF) << 8;
                        state++;
                        break;
                    case STATE_Q2:
                        qsample |= bytes[i] & 0xFF;
                        inlsamples[receiver][inoffset] = (float) isample / 8388607.0F; // 24 bit sample convert to -1..+1
                        inrsamples[receiver][inoffset] = (float) qsample / 8388607.0F; // 24 bit sample convert to -1..+1
                        receiver++;
                        if (receiver == receivers) {
                            state++;
                            receiver = 0;
                        } else {
                            state = STATE_I0;
                        }
                        break;
                    case STATE_M0:
                        msample = bytes[i] << 8; // keep sign ????
                        state++;
                        break;
                    case STATE_M1:
                        msample |= bytes[i] & 0xFF;

                        // we now have an I, Q and Microphone sample
                        inmiclsamples[inoffset] = (float) msample / 32767.0F * radio.micgain; // 16 bit sample convert to -1..+1
                        inmicrsamples[inoffset] = 0.0F;

                        inoffset++;

                        if (inoffset == radio.buffersize) {

                            if (transmit) {
                                if (tuning) {
                                    Filter filter = Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());
                                    tunefrequency = filter.getLow() + ((filter.getHigh() - filter.getLow()) / 2);
                                    phase = sineWave(inmiclsamples, inmiclsamples.length, phase, tunefrequency);

                                    wdsp.fexchange2(Channel.TX, inmiclsamples, inmicrsamples, outmiclsamples, outmicrsamples, error);
                                    if (error[0] != 0) {
                                        //Log.i(this, "fexchange2 returned " + error[0]);
                                    }

                                } else if (radio.micsource == Radio.MIC_SOURCE_RADIO) {

                                    wdsp.fexchange2(Channel.TX, inmiclsamples, inmicrsamples, outmiclsamples, outmicrsamples, error);
                                    if (error[0] != 0) {
                                        //Log.i(this, "fexchange2 returned " + error[0]);
                                    }

                                }

                                wdsp.Spectrum(Display.TX, 0, 0, outmicrsamples, outmiclsamples);

                            } else {
                                for (int r = 0; r < receivers; r++) {
                                    wdsp.fexchange2(r + receiverbase, inlsamples[r], inrsamples[r], outlsamples[r], outrsamples[r], error);
                                    if (error[0] != 0) {
                                        //Log.i(this, "fexchange2 returned " + error[0]);
                                    }
                                    //Log.i(this,"calling Spectrum with "+inoffset+" samples");
                                    wdsp.Spectrum(r + receiverbase, 0, 0, inrsamples[r], inlsamples[r]);
                                }
                            }

                            if (transmit) {
                                if (tuning || radio.micsource == Radio.MIC_SOURCE_RADIO) {
                                    sendSamples(outmiclsamples, outmicrsamples);
                                }
                            } else {
                                if (radio.audiooutput == Radio.AUDIO_OUTPUT_RADIO || radio.audiooutput == Radio.AUDIO_OUTPUT_BOTH) {
                                    sendSamples(outlsamples[activeReceiver], outrsamples[activeReceiver]);
                                }
                                if (radio.audiooutput == Radio.AUDIO_OUTPUT_LOCAL || radio.audiooutput == Radio.AUDIO_OUTPUT_BOTH) {
                                    for (int j = 0; j < outlsamples[activeReceiver].length; j++) {
                                        short lsample = (short) (outlsamples[activeReceiver][j] * 32767.0F * radio.afgain);
                                        short rsample = (short) (outrsamples[activeReceiver][j] * 32767.0F * radio.afgain);
                                        audiooutput[audiooutputindex++] = (byte) ((lsample >> 8) & 0xFF);
                                        audiooutput[audiooutputindex++] = (byte) (lsample & 0xFF);
                                        audiooutput[audiooutputindex++] = (byte) ((rsample >> 8) & 0xFF);
                                        audiooutput[audiooutputindex++] = (byte) (rsample & 0xFF);
                                        //Log.i(this,audiooutputindex+":"+lsample+","+rsample);
                                        if (audiooutputindex == audiooutput.length) {

                                            if (audioline != null) {
                                                //Log.i(this, "write audio: " + audiooutput.length+" active="+audioline.isActive()+" "+audioline.available());
                                                int sent = audioline.write(audiooutput, 0, audiooutput.length);
                                                if (sent != audiooutput.length) {
                                                    Log.i(this, "write audio returned " + sent + " when sending " + audiooutput.length);
                                                }
                                            }
                                            audiooutputindex = 0;
                                        }
                                    }
                                }
                                if (radio.audiooutput == Radio.AUDIO_OUTPUT_LOCAL) {
                                    for (int j = 0; j < outlsamples[activeReceiver].length; j++) {
                                        outlsamples[activeReceiver][j] = outrsamples[activeReceiver][j] = 0;
                                    }
                                    sendSamples(outlsamples[activeReceiver], outrsamples[activeReceiver]);
                                }
                            }

                            inoffset = 0;
                        }

                        state = STATE_I0;
                        break;

                    case STATE_SYNC_ERROR:
                        // discard
                        return;
                }
            }
        }
    }

    public synchronized void sendSamples(float[] outlsamples, float[] outrsamples) {
        //Log.i(this,"sendSamples: "+outlsamples.length);
        Band band = radio.getReceiver(0).getBands().get();
        BandStack bandstack = band.get();
        float rfgain = 1.0F;  // for PENNYLANE
        if (radio.radio == Radio.METIS_PENELOPE || radio.radio == Radio.METIS_PENELOPE_ALEX) {
            rfgain = radio.drive;
            if (tuning) {
                rfgain = radio.tunegain;
            }
            rfgain = rfgain * band.getDrive();
        }

        // send data back to Metis
        for (int j = 0; j < outlsamples.length; j++) {

            if (transmit) {
                sendbuffer[txoffset++] = (byte) 0; // rx
                sendbuffer[txoffset++] = (byte) 0; // rx
                sendbuffer[txoffset++] = (byte) 0; // rx
                sendbuffer[txoffset++] = (byte) 0; // rx

                short l = (short) (outlsamples[j] * 32767.0F * rfgain);
                short r = (short) (outrsamples[j] * 32767.0F * rfgain);
                sendbuffer[txoffset++] = (byte) ((l >> 8) & 0xFF);
                sendbuffer[txoffset++] = (byte) (l & 0xFF);
                sendbuffer[txoffset++] = (byte) ((r >> 8) & 0xFF);
                sendbuffer[txoffset++] = (byte) (r & 0xFF);
            } else {
                short l = (short) (outlsamples[j] * 32767.0F * radio.afgain);
                short r = (short) (outrsamples[j] * 32767.0F * radio.afgain);
                sendbuffer[txoffset++] = (byte) ((l >> 8) & 0xFF);
                sendbuffer[txoffset++] = (byte) (l & 0xFF);
                sendbuffer[txoffset++] = (byte) ((r >> 8) & 0xFF);
                sendbuffer[txoffset++] = (byte) (r & 0xFF);

                sendbuffer[txoffset++] = (byte) 0; // tx
                sendbuffer[txoffset++] = (byte) 0; // tx
                sendbuffer[txoffset++] = (byte) 0; // tx
                sendbuffer[txoffset++] = (byte) 0; // tx
            }

            if (txoffset == 520) {
                // first OZY buffer filled
                txoffset = 528;
            } else if (txoffset == 1032) {
                // second OZY buffer filled

                // put in the header
                sendbuffer[0] = (byte) 0xEF;
                sendbuffer[1] = (byte) 0xFE;
                sendbuffer[2] = (byte) 0x01;
                sendbuffer[3] = (byte) 0x02;  // to EP2

                // put in the tx sequence number
                sendbuffer[4] = (byte) ((txsequence >> 24) & 0xFF);
                sendbuffer[5] = (byte) ((txsequence >> 16) & 0xFF);
                sendbuffer[6] = (byte) ((txsequence >> 8) & 0xFF);
                sendbuffer[7] = (byte) (txsequence & 0xFF);

                // put in the OZY control bytes
                sendbuffer[8] = SYNC;
                sendbuffer[9] = SYNC;
                sendbuffer[10] = SYNC;

                byte mox = transmit ? MOX_ENABLED : MOX_DISABLED;

                switch (command) {
                    case 0: {
                        sendbuffer[11] = txcontrol0;
                        sendbuffer[12] = txcontrol1;
                        sendbuffer[13] = (byte) (txcontrol2 | ((transmit ? band.getOCTx() : band.getOCRx()) << 1));
                        byte tx3 = 0x00;
                        switch (this.discovered.getDevice()) {
                            case Discovered.DEVICE_HERMES_LITE:
                                tx3 |= (byte) (ALEX_ATTENUATION_0DB | radio.random);
                                if (radio.random == LT2208_RANDOM_OFF) { // AGC off
                                    if (lna_dither[radio.attenuation + 12]) {
                                        tx3 |= LT2208_DITHER_ON;
                                    }
                                }
                                break;
                            case Discovered.DEVICE_HERMES:
                            case Discovered.DEVICE_ANGELIA:
                            case Discovered.DEVICE_ORION:
                                tx3 |= (byte) (radio.dither | radio.random | radio.preamp);
                                if (radio.attenuation > 31) {
                                    tx3 |= (byte) ALEX_ATTENUATION_30DB;
                                }
                                break;
                            case Discovered.DEVICE_METIS:
                                tx3 |= (byte) (radio.dither | radio.random | radio.preamp);
                                switch (radio.attenuation) {
                                    case 0:
                                        tx3 |= (byte) ALEX_ATTENUATION_0DB;
                                        break;
                                    case 10:
                                        tx3 |= (byte) ALEX_ATTENUATION_10DB;
                                        break;
                                    case 20:
                                        tx3 |= (byte) ALEX_ATTENUATION_20DB;
                                        break;
                                    case 30:
                                        tx3 |= (byte) ALEX_ATTENUATION_30DB;
                                        break;
                                }
                                break;
                        }
                        if (radio.radio != Radio.METIS_PENELOPE
                                && radio.radio != Radio.METIS_PENNYLANE) {

                            switch (bandstack.getRxAntenna()) {
                                case BandStack.NONE:
                                    //tx3|=0x00;
                                    break;
                                case BandStack.RX1:
                                    tx3 |= 0xA0;
                                    break;
                                case BandStack.RX2:
                                    tx3 |= 0xC0;
                                    break;
                                case BandStack.RXXV:
                                    tx3 |= 0xE0;
                                    break;
                            }
                        }
                        sendbuffer[14] = tx3;
                        byte tx4 = txcontrol4;
                        if (radio.radio != Radio.METIS_PENELOPE
                                && radio.radio != Radio.METIS_PENNYLANE) {
                            switch (bandstack.getTxAntenna()) {
                                case BandStack.ANT1:
                                    tx4 |= 0x00;
                                    break;
                                case BandStack.ANT2:
                                    tx4 |= 0x01;
                                    break;
                                case BandStack.ANT3:
                                    tx4 |= 0x02;
                                    break;
                            }
                        }
                        sendbuffer[15] = tx4;
                        //Log.i(this, "c4="+Integer.toHexString(sendbuffer[15]&0xFF));
                        command++;

                        break;
                    }
                    case 1: {
                        sendbuffer[11] = 0x12;
                        sendbuffer[12] = 0x00;
                        if (tuning) {
                            sendbuffer[12] = (byte) (255.0F * band.getDrive() * radio.tunegain);
                        } else if (transmit) {
                            sendbuffer[12] = (byte) (255.0F * band.getDrive() * radio.drive);
                        }
                        byte c2 = 0x00;
                        if (radio.micboost) {
                            c2 |= 0x01;
                        }
                        if (this.discovered.getDevice() == Discovered.DEVICE_HERMES) {
                            if (radio.radio == Radio.HERMES_APOLLO) {
                                c2 = (byte) (APOLLO_BOARD | APOLLO_FILTER | APOLLO_TUNER);
                                if (tuning) {
                                    c2 |= APOLLO_TUNE;
                                }
                            }
                        }
                        sendbuffer[13] = c2;
                        sendbuffer[14] = (byte) 0;
                        sendbuffer[15] = (byte) 0;
                        command++;

                        break;
                    }
                    case 2: {
                        sendbuffer[11] = 0x14;
                        byte c1 = 0x00;
                        /*
                         if(receivers.length>0) {
                         if(receivers[0].getPreamp()) {
                         c1|=0x01;
                         }
                         }
                         if(receivers.length>1) {
                         if(receivers[1].getPreamp()) {
                         c1|=0x02;
                         }
                         }
                         */
                        int mode = bandstack.getMode();
                        if (mode != Modes.CWU && mode != Modes.CWL) {
                            c1 = (byte) ((radio.oriontipring << 4) | (radio.orionmicbias << 5) | (radio.orionmicptt << 6));
                        }
                        sendbuffer[12] = c1;
                        sendbuffer[13] = 0;
                        sendbuffer[14] = 0;

                        byte c4 = 0x00;
                        switch (this.discovered.getDevice()) {
                            case Discovered.DEVICE_HERMES_LITE:
                                if (radio.random == LT2208_RANDOM_OFF) {
                                    c4 = lna_att[radio.attenuation + 12];
                                    //c4 |= (byte) 0x20; // enable
                                }
                                break;
                            case Discovered.DEVICE_HERMES:
                            case Discovered.DEVICE_ANGELIA:
                            case Discovered.DEVICE_ORION:

                                if (radio.attenuation > 31) {
                                    c4 = (byte) (radio.attenuation - 30);
                                } else {
                                    c4 = (byte) (radio.attenuation);
                                }
                                c4 |= (byte) 0x20;
                                break;
                        }
                        sendbuffer[15] = c4;
                        command++;

                        break;
                    }
                    case 3: {
                        sendbuffer[11] = 0x16;
                        byte c1=0x00;
                        c1=(byte)(radio.attenuation);
                        c1 |= 0x20;
                        sendbuffer[12] = c1;
                        byte c2=0x00;
                        c2=(byte)(radio.attenuation);
                        c2 |= 0x20;
                        if (radio.cwkeysreversed != 0) {
                            c2 |= 0x40;
                        }
                        sendbuffer[13] = c2;
                        sendbuffer[14] = (byte) (radio.cwkeyerspeed | (radio.cwkeyermode << 6));
                        sendbuffer[15] = (byte) (radio.cwkeyerweight | (radio.cwkeyerspacing << 7));
                        command++;
                        break;
                    }
                    case 4: {
                        sendbuffer[11] = 0x1C;
                        byte c1 = 0x00;
                        for (int i = 0; i < 4; i++) {
                            if(i<radio.MAX_RECEIVERS) {
                                c1 |= (byte) (radio.getReceiver(i).getADC() << (i * 2));
                            }
                        }
                        sendbuffer[12] = c1;
                        byte c2 = 0x00;
                        for (int i = 4; i < 7; i++) {
                            if(i<radio.MAX_RECEIVERS) {
                                c2 |= (byte) (radio.getReceiver(i).getADC() << ((i - 4) * 2));
                            }
                        }
                        sendbuffer[13] = c2;
                        sendbuffer[14] = (byte) radio.txinputattenuation;
                        sendbuffer[15] = 0x00;
                        command++;
                        break;
                    }
                    case 5: {
                        sendbuffer[11] = 0x1E;
                        int mode = bandstack.getMode();
                        sendbuffer[12] = (byte) radio.cwinternal;
                        if ((mode != Modes.CWU && mode != Modes.CWL) || tuning || transmit) {
                            sendbuffer[12] = (byte) 0;
                        }
                        sendbuffer[13] = (byte) radio.cwsidetonevolume;
                        sendbuffer[14] = (byte) radio.cwpttdelay;
                        sendbuffer[15] = 0x00;
                        command++;
                        break;
                    }
                    case 6: {
                        sendbuffer[11] = 0x20;
                        sendbuffer[12] = (byte) radio.cwhangtime;
                        sendbuffer[13] = (byte) (radio.cwhangtime >> 8);
                        sendbuffer[14] = (byte) radio.cwsidetonefrequency;
                        sendbuffer[15] = (byte) (radio.cwsidetonefrequency >> 8);
                        command = 0;
                        break;
                    }
                }

                // turn on transmit if needed
                sendbuffer[11] |= mox;

                // put in the frequency
                long frequency;

                sendbuffer[520] = SYNC;
                sendbuffer[521] = SYNC;
                sendbuffer[522] = SYNC;

                switch (freqcommand) {
                    case 0: { // rx frequency
                        Band rxband = radio.getReceiver(rxfrequency).getBands().get();
                        frequency = rxband.get().getFrequency();
                        if (rxband instanceof Transverter) {
                            Transverter xvtr = (Transverter) rxband;
                            frequency = frequency - xvtr.getIfFrequency();
                        }
                        sendbuffer[523] = (byte) (txcontrol0 | ((rxfrequency + 2) << 1) | mox);
                        sendbuffer[524] = (byte) ((frequency >> 24) & 0xFF);
                        sendbuffer[525] = (byte) ((frequency >> 16) & 0xFF);
                        sendbuffer[526] = (byte) ((frequency >> 8) & 0xFF);
                        sendbuffer[527] = (byte) (frequency & 0xFF);

                        //Log.i(this, "rx="+rxfrequency+" f="+frequency+" c0="+Integer.toHexString(sendbuffer[523]&0xFF));
                        rxfrequency++;
                        if (rxfrequency == receivers) {
                            rxfrequency = 0;
                            freqcommand++;
                        }
                        break;
                    }
                    case 1: { // tx frequency
                        Band txband = radio.getReceiver(0).getBands().get();
                        frequency = txband.get().getFrequency();
                        if (txband instanceof Transverter) {
                            Transverter xvtr = (Transverter) txband;
                            frequency = frequency - xvtr.getIfFrequency();
                        }
                        if (txband.get().getMode() == Modes.CWL) {
                            frequency = frequency - radio.cwsidetonefrequency;
                        } else if (txband.get().getMode() == Modes.CWU) {
                            frequency = frequency + radio.cwsidetonefrequency;
                        }
                        sendbuffer[523] = (byte) (txcontrol0 | 0x02 | mox);
                        sendbuffer[524] = (byte) ((frequency >> 24) & 0xFF);
                        sendbuffer[525] = (byte) ((frequency >> 16) & 0xFF);
                        sendbuffer[526] = (byte) ((frequency >> 8) & 0xFF);
                        sendbuffer[527] = (byte) (frequency & 0xFF);
                        //Log.i(this, "tx=0 f="+frequency+" c0="+Integer.toHexString(sendbuffer[523]&0xFF));
                        freqcommand = 0;
                        break;
                    }

                }

                send();

                txoffset = 16;
            }
        }

    }

    public synchronized void send() {
        try {
            socket.send(samplesdatagram);
        } catch (SocketException se) {
            Log.i(this, "send: " + se.toString());
        } catch (UnknownHostException uhe) {
            Log.i(this, "send: " + uhe.toString());
        } catch (IOException ioe) {
            Log.i(this, "send: " + ioe.toString());
        }
        txsequence++;
    }

    public synchronized void sendCommand() {
        try {
            socket.send(commanddatagram);
        } catch (SocketException se) {
            Log.i(this, "sendCommand: " + se.toString());
        } catch (UnknownHostException uhe) {
            Log.i(this, "sendCommand: " + uhe.toString());
        } catch (IOException ioe) {
            Log.i(this, "sendCommand: " + ioe.toString());
        }
    }

    private synchronized void prime() {
        for (int i = 0; i < outlsamples[0].length; i++) {
            outlsamples[0][i] = 0.0F;
            outrsamples[0][i] = 0.0F;
        }
        do {
            sendSamples(outlsamples[0], outrsamples[0]);
        } while (command != 0);

        // send start command
        commandbuffer[0] = (byte) 0xEF;
        commandbuffer[1] = (byte) 0xFE;
        commandbuffer[2] = (byte) 0x04;
        if (bandscope) {
            commandbuffer[3] = (byte) 0x03;
        } else {
            commandbuffer[3] = (byte) 0x01;
        }
        for (int i = 4; i < 64; i++) {
            commandbuffer[i] = (byte) 0x00;
        }

        sendCommand();
    }

    double sineWave(float[] buf, int samples, double phase, double freq) {
        double phase_step = 2 * Math.PI * freq / radio.samplerate;
        int i;

        for (i = 0; i < samples; i++) {
            buf[i] = (float) Math.sin(phase);
            phase += phase_step;
        }
        return phase;
    }

    public int getMercurySoftwareVersion() {
        return mercury_software_version;
    }

    public int getPenelopeSoftwareVersion() {
        return penelope_software_version;
    }

    public int getMetisSoftwareVersion() {
        return metis_software_version;
    }

    public int getPenelopeForwardPower() {
        return penelope_forward_power;
    }

    public int getAlexForwardPower() {
        return alex_forward_power;
    }

    public int getAlexReversePower() {
        return alex_reverse_power;
    }

    public void debug() {
        this.dbg = true;
    }

    /*
     public synchronized boolean getDisplaySamples(int channel, float[] samples) {
     int[] result = new int[1];

     if (samples.length != pixels) {
     Log.i(this, "Process_Panadapter: channel=" + channel + " samples=" + samples.length + " pixels=" + pixels);
     return false;
     }

     wdsp.GetPixels(channel, samples, result);

     return (result[0] == 1);
     }
     */
    public boolean isADC1Overflow() {
        return adc1overflow;
    }

    public boolean isADC2Overflow() {
        return adc2overflow;
    }

    public boolean isADC3Overflow() {
        return adc3overflow;
    }

    public boolean isADC4Overflow() {
        return adc4overflow;
    }

    public boolean isActiveReceiver(int receiver) {
        return activeReceiver == receiver;
    }

    private Radio radio;

    private int receivers = 1;
    private int receiver = 0;
    private int rxfrequency = 0;
    private int activeReceiver = 0;
    private Discovered discovered;
    private int id;
    private int receiverbase;

    private int frame;
    private int frames;
    private int data;
            
    private boolean bandscope;

    private int bandscopepixels;

    private PTTListener pttListener;

    private boolean dbg = false;

    private double phase = 0.0;
    private double tunefrequency = 600;

    private int toport = 1024;

    private String myaddress = "127.0.0.1";
    private int myport = 1024;
    private DatagramSocket socket;

    private byte rxbuffer[] = new byte[1032];
    private DatagramPacket rxdatagram;

    private long ep6sequence = -1;
    private boolean running;

    private WDSP wdsp;

    private byte SYNC = 0x7F;

    private byte rxcontrol0;
    private byte rxcontrol1;
    private byte rxcontrol2;
    private byte rxcontrol3;
    private byte rxcontrol4;

    // control 0
    public static byte MOX_DISABLED = (byte) 0x00;
    public static byte MOX_ENABLED = (byte) 0x01;

    // control 1
    public static byte MIC_SOURCE_JANUS = (byte) 0x00;
    public static byte MIC_SOURCE_PENELOPE = (byte) 0x80;
    public static byte CONFIG_NONE = (byte) 0x00;
    public static byte CONFIG_PENELOPE = (byte) 0x20;
    public static byte CONFIG_MERCURY = (byte) 0x40;
    public static byte CONFIG_BOTH = (byte) 0x60;
    public static byte PENELOPE_122_88MHZ_SOURCE = (byte) 0x00;
    public static byte MERCURY_122_88MHZ_SOURCE = (byte) 0x10;
    public static byte ATLAS_10MHZ_SOURCE = (byte) 0x00;
    public static byte PENELOPE_10MHZ_SOURCE = (byte) 0x04;
    public static byte MERCURY_10MHZ_SOURCE = (byte) 0x08;
    public static byte SPEED_48KHZ = (byte) 0x00;
    public static byte SPEED_96KHZ = (byte) 0x01;
    public static byte SPEED_192KHZ = (byte) 0x02;
    public static byte SPEED_384KHZ = (byte) 0x03;

    // control 2
    public static byte MODE_CLASS_E = (byte) 0x01;
    public static byte MODE_OTHERS = (byte) 0x00;

    public static byte APOLLO_FILTER = (byte) 0x04;
    public static byte APOLLO_TUNER = (byte) 0x08;
    public static byte APOLLO_TUNE = (byte) 0x10;
    public static byte APOLLO_BOARD = (byte) 0x20;

    // control 3
    public static byte ALEX_ATTENUATION_0DB = (byte) 0x00;
    public static byte ALEX_ATTENUATION_10DB = (byte) 0x01;
    public static byte ALEX_ATTENUATION_20DB = (byte) 0x02;
    public static byte ALEX_ATTENUATION_30DB = (byte) 0x03;
    public static byte LT2208_GAIN_OFF = (byte) 0x00;
    public static byte LT2208_GAIN_ON = (byte) 0x04;
    public static byte LT2208_DITHER_OFF = (byte) 0x00;
    public static byte LT2208_DITHER_ON = (byte) 0x08;
    public static byte LT2208_RANDOM_OFF = (byte) 0x00;
    public static byte LT2208_RANDOM_ON = (byte) 0x10;

    // control 4
    private static byte DUPLEX = (byte) 0x04;

    // default tx control bytes
    private byte txcontrol0 = (byte) (MOX_DISABLED);
    private byte txcontrol1 = (byte) (CONFIG_BOTH /*| MERCURY_122_88MHZ_SOURCE | MERCURY_10MHZ_SOURCE*/ | MIC_SOURCE_PENELOPE | SPEED_48KHZ);
    private byte txcontrol2 = (byte) (MODE_OTHERS);
    private byte txcontrol3 = (byte) (ALEX_ATTENUATION_0DB /* |LT2208_GAIN_OFF  | LT2208_DITHER_ON | LT2208_RANDOM_ON */);
    private byte txcontrol4 = (byte) (DUPLEX);

    private long txsequence = 0L;

    private static final int STATE_SYNC0 = 0;
    private static final int STATE_SYNC1 = 1;
    private static final int STATE_SYNC2 = 2;
    private static final int STATE_CONTROL0 = 3;
    private static final int STATE_CONTROL1 = 4;
    private static final int STATE_CONTROL2 = 5;
    private static final int STATE_CONTROL3 = 6;
    private static final int STATE_CONTROL4 = 7;
    private static final int STATE_I0 = 8;
    private static final int STATE_I1 = 9;
    private static final int STATE_I2 = 10;
    private static final int STATE_Q0 = 11;
    private static final int STATE_Q1 = 12;
    private static final int STATE_Q2 = 13;
    private static final int STATE_M0 = 14;
    private static final int STATE_M1 = 15;
    private static final int STATE_SYNC_ERROR = 16;

    private int state = STATE_SYNC0;

    private int isample;
    private int qsample;
    private int msample;

    private float[][] inlsamples;
    private float[][] inrsamples;
    private float[][] outlsamples;
    private float[][] outrsamples;
    private float[] inmiclsamples;
    private float[] inmicrsamples;
    private float[] outmiclsamples;
    private float[] outmicrsamples;
    private int inoffset = 0;

    private static final int BS_FFT_SIZE = 8192;
    private static final int BS_BUFFER_SIZE = 2048;
    private float[] bslsamples;
    private float[] bsrsamples;
    private int bsoffset = 0;

    private byte[] sendbuffer = new byte[1032];
    private int txoffset = 16;

    private byte[] commandbuffer = new byte[84];

    private InetAddress toaddress;
    private DatagramPacket commanddatagram;
    private DatagramPacket samplesdatagram;

    // local audio
    /*
     private AudioTrack audiotrack;
     */
    private int outsize;
    //private short[] audiooutput = new short[1024 * 2];
    private byte[] audiooutput = new byte[1024 * 4]; // 2 channels of shorts
    private int audiooutputindex = 0;

    private long time;

    private int cwnoteindex = 0;
    private float cwnotesin[] = new float[240];
    private float cwnotecos[] = new float[240];

    private double packetsreceived = 0.0;
    private double packetsmissed = 0.0;

    public int metis_software_version = 0;
    public int mercury_software_version = 0;
    public int penelope_software_version = 0;

    private int power_samples = 0;
    private int avg_penelope_forward_power = 0;
    private int avg_alex_forward_power = 0;
    private int avg_alex_reverse_power = 0;

    private int penelope_forward_power = 0;
    private int alex_forward_power = 0;
    private int alex_reverse_power = 0;

    private boolean ptt = false;
    private boolean dash = false;
    private boolean dot = false;

    private boolean lt2208_overflow = false;
    private boolean adc1overflow = false;
    private boolean adc2overflow = false;
    private boolean adc3overflow = false;
    private boolean adc4overflow = false;

    private int ain3 = 0;
    private int ain4 = 0;
    private int ain6 = 0;

    public double getPacketLoss() {
        double loss = (packetsmissed / packetsreceived) * 100.0;
        packetsmissed = packetsreceived = 0.0;
        return loss;
    }

    private int[] error = new int[1];

    private int[] success = new int[1];

    private int command = 0;
    private int freqcommand = 0;

    private boolean transmit = false;
    private boolean tuning = false;

    boolean last_ptt = false;
    boolean last_dot = false;
    boolean last_dash = false;

    boolean lna_dither[] = {true, true, true, true, true, true, true, true,
        true, true, true, true, true, true, true, true,
        true, true, true, true, true, true, true, true,
        true, true, true, true, true, true, true, true,
        false, false, false, false, false, false, false, false,
        false, false, false, false, false, false, false, false,
        false, false, false, false, false, false, false, false,
        false, false, false, false, false, false, false, false};
    byte lna_att[] = {31, 30, 29, 28, 27, 26, 25, 24,
        23, 22, 21, 20, 19, 18, 17, 16,
        15, 14, 13, 12, 11, 10, 9, 8,
        7, 6, 5, 4, 3, 2, 1, 0,
        31, 30, 29, 28, 27, 26, 25, 24,
        23, 22, 21, 20, 19, 18, 17, 16,
        15, 14, 13, 12, 11, 10, 9, 8,
        7, 6, 5, 4, 3, 2, 1, 0};

    private AudioFormat audioformat;
    private SourceDataLine audioline;
}
