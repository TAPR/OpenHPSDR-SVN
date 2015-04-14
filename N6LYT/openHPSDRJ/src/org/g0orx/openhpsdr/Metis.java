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

import org.g0orx.openhpsdr.discovery.Discovered;
import org.g0orx.openhpsdr.modes.Modes;
import org.g0orx.openhpsdr.wdsp.WDSP;

public class Metis extends Thread {

    public Metis(int pixels) {

        Log.i("Metis", "pixels=" + pixels);
        this.pixels = pixels;

        configuration = Configuration.getInstance();

        Log.i("Metis", "using: " + configuration.discovered.toString());

        txcontrol1 = (byte) (CONFIG_BOTH | MIC_SOURCE_PENELOPE | configuration.clock10 | configuration.clock122);
        txcontrol3 = (byte) (ALEX_ATTENUATION_0DB | configuration.dither | configuration.random | configuration.preamp);
        txcontrol4 = (byte) (DUPLEX | (((receivers - 1) << 3) & 0x038));

        Log.i("Metis", "samplerate: " + configuration.samplerate);
        // set speed and output buffer size
        if (configuration.samplerate == 48000.0) {
            outsize = configuration.buffersize;
            txcontrol1 |= SPEED_48KHZ;
        } else if (configuration.samplerate == 96000.0) {
            outsize = configuration.buffersize / 2;
            txcontrol1 |= SPEED_96KHZ;
        } else if (configuration.samplerate == 192000.0) {
            outsize = configuration.buffersize / 4;
            txcontrol1 |= SPEED_192KHZ;
        } else if (configuration.samplerate == 384000.0) {
            outsize = configuration.buffersize / 8;
            txcontrol1 |= SPEED_384KHZ;
        }

        Log.i("Metis", "samplerate: " + configuration.samplerate + " buffersize=" + configuration.buffersize + " fftsize=" + configuration.fftsize + " outsize=" + outsize);

        this.wdsp = WDSP.getInstance();

        // allocate space for the input/output samples
        inlsamples = new float[configuration.buffersize];
        inrsamples = new float[configuration.buffersize];
        inmiclsamples = new float[configuration.buffersize];
        inmicrsamples = new float[configuration.buffersize];
        outlsamples = new float[outsize];
        outrsamples = new float[outsize];
        suboutlsamples = new float[outsize];
        suboutrsamples = new float[outsize];

        bslsamples = new float[BS_BUFFER_SIZE];
        bsrsamples = new float[BS_BUFFER_SIZE];
        for (int i = 0; i < BS_BUFFER_SIZE; i++) {
            bsrsamples[i] = 0.0F;
        }

        // build the toaddress InetAddress
        try {
            toaddress = InetAddress.getByName(configuration.discovered.getAddress());
        } catch (Exception e) {
            Log.i("Metis", "constructor: " + e.toString());
        }

        // create the DatagramPackets for commands and samples
        commanddatagram = new DatagramPacket(commandbuffer, commandbuffer.length, toaddress, toport);
        samplesdatagram = new DatagramPacket(sendbuffer, sendbuffer.length, toaddress, toport);

        if (configuration.audiooutput == Configuration.AUDIO_OUTPUT_LOCAL || configuration.audiooutput == Configuration.AUDIO_OUTPUT_BOTH) {
            initializeLocalAudioOutput();
        }

        /*
         try {
         audiotrack = new AudioTrack(AudioManager.STREAM_MUSIC, 48000,
         AudioFormat.CHANNEL_OUT_STEREO,
         AudioFormat.ENCODING_PCM_16BIT, 1024,
         AudioTrack.MODE_STREAM);
         } catch (IllegalArgumentException e) {
         Log.i("Metis", "new AudioTrack Error: " + e.getMessage());
         audiotrack = null;
         }
         */
        // Calculate sample values for baseband CW note^M
        double deltaf = Math.PI / 40.0;     // (2 PI f / 48k) gives an 600 Hz note at 48 ksps
        for (int i = 0; i < 240; ++i) {
            cwnotesin[i] = (float) ((Math.pow(2, 15) - 1) * Math.sin(deltaf * i) / 32767.0);
            cwnotecos[i] = (float) ((Math.pow(2, 15) - 1) * Math.cos(deltaf * i) / 32767.0);
        }

        /*
         if (audiotrack != null) {
         audiotrack.play();
         }
         */
        // rx spectrum
        wdsp.XCreateAnalyzer(Display.RX, success, 262144, 1, 1, "");
        if (success[0] != 0) {
            Log.i("Metis", "XCreateAnalyzer Display.RX failed:" + success[0]);
        }

        int flp[] = {0};
        double KEEP_TIME = 0.1;
        int spur_elimination_ffts = 1;
        int data_type = 1;
        int fft_size = 8192;
        int window_type = 4;
        double kaiser_pi = 14.0;
        int overlap = 2048;
        int clip = 0;
        int span_clip_l = 0;
        int span_clip_h = 0;
        //int pixels=1280;
        int stitches = 1;
        int avm = 0;
        double tau = 0.001 * 120.0;
        int MAX_AV_FRAMES = 60;
        int display_average = Math.max(2, (int) Math.min((double) MAX_AV_FRAMES, (double) configuration.fps * tau));
        double avb = Math.exp(-1.0 / (configuration.fps * tau));
        int calibration_data_set = 0;
        double span_min_freq = 0.0;
        double span_max_freq = 0.0;

        int max_w = fft_size + (int) Math.min(KEEP_TIME * (double) configuration.fps, KEEP_TIME * (double) fft_size * (double) configuration.fps);

        wdsp.SetAnalyzer(Display.RX,
                spur_elimination_ffts, //number of LO frequencies = number of ffts used in elimination
                data_type, //0 for real input data (I only); 1 for complex input data (I & Q)
                flp, //vector with one elt for each LO frequency, 1 if high-side LO, 0 otherwise
                fft_size, //size of the fft, i.e., number of input samples
                configuration.buffersize, //number of samples transferred for each OpenBuffer()/CloseBuffer()
                window_type, //integer specifying which window function to use
                kaiser_pi, //PiAlpha parameter for Kaiser window
                overlap, //number of samples each fft (other than the first) is to re-use from the previous
                clip, //number of fft output bins to be clipped from EACH side of each sub-span
                span_clip_l, //number of bins to clip from low end of entire span
                span_clip_h, //number of bins to clip from high end of entire span
                pixels, //number of pixel values to return.  may be either <= or > number of bins
                stitches, //number of sub-spans to concatenate to form a complete span
                avm, //averaging mode
                display_average, //number of spans to (moving) average for pixel result
                avb, //back multiplier for weighted averaging
                calibration_data_set, //identifier of which set of calibration data to use
                span_min_freq, //frequency at first pixel value8192
                span_max_freq, //frequency at last pixel value
                max_w //max samples to hold in input ring buffers
        );

        // tx spectrum
        wdsp.XCreateAnalyzer(Display.TX, success, 262144, 1, 1, "");
        if (success[0] != 0) {
            Log.i("Metis", "XCreateAnalyzer txchannel failed:" + success[0]);
        }

        // use same params as rx channel
        /*
         int flp[] = {0};
         double KEEP_TIME=0.1;
         int spur_elimination_ffts=1;
         int data_type=1;
         int fft_size=8192;
         int window_type=4;
         double kaiser_pi=14.0;
         int overlap=2048;
         int clip=0;
         int span_clip_l=0;
         int span_clip_h=0;
         int pixels=1280;
         int stitches=1;
         int avm=0;
         double tau=0.001*120.0;
         int MAX_AV_FRAMES=60;
         int display_average = Math.max(2, (int)Math.min((double)MAX_AV_FRAMES, (double)configuration.fps * tau));
         double avb = Math.exp(-1.0 / (configuration.fps * tau));
         int calibration_data_set=0;
         double span_min_freq=0.0;
         double span_max_freq=0.0;

         int max_w=fft_size + (int)Math.min(KEEP_TIME * (double)configuration.fps, KEEP_TIME * (double)fft_size * (double)configuration.fps);
         */
        wdsp.SetAnalyzer(Display.TX,
                spur_elimination_ffts, //number of LO frequencies = number of ffts used in elimination
                data_type, //0 for real input data (I only); 1 for complex input data (I & Q)
                flp, //vector with one elt for each LO frequency, 1 if high-side LO, 0 otherwise
                fft_size, //size of the fft, i.e., number of input samples
                outsize /*configuration.buffersize*/, //number of samples transferred for each OpenBuffer()/CloseBuffer()
                window_type, //integer specifying which window function to use
                kaiser_pi, //PiAlpha parameter for Kaiser window
                overlap, //number of samples each fft (other than the first) is to re-use from the previous
                clip, //number of fft output bins to be clipped from EACH side of each sub-span
                span_clip_l, //number of bins to clip from low end of entire span
                span_clip_h, //number of bins to clip from high end of entire span
                pixels, //number of pixel values to return.  may be either <= or > number of bins
                stitches, //number of sub-spans to concatenate to form a complete span
                avm, //averaging mode
                display_average, //number of spans to (moving) average for pixel result
                avb, //back multiplier for weighted averaging
                calibration_data_set, //identifier of which set of calibration data to use
                span_min_freq, //frequency at first pixel value8192
                span_max_freq, //frequency at last pixel value
                max_w //max samples to hold in input ring buffers
        );

        // bandscope spectrum
        wdsp.XCreateAnalyzer(Display.BS, success, 262144, 1, 1, "");
        if (success[0] != 0) {
            Log.i("Metis", "XCreateAnalyzer bschannel failed:" + success[0]);
        }

        wdsp.SetAnalyzer(Display.BS,
                spur_elimination_ffts, //number of LO frequencies = number of ffts used in elimination
                WDSP.REAL, //0 for real input data (I only); 1 for complex input data (I & Q)
                flp, //vector with one elt for each LO frequency, 1 if high-side LO, 0 otherwise
                BS_FFT_SIZE, //size of the fft, i.e., number of input samples
                BS_BUFFER_SIZE, //number of samples transferred for each OpenBuffer()/CloseBuffer()
                WDSP.BLACKMAN_HARRIS, //integer specifying which window function to use (Blackman-Harris)
                kaiser_pi, //PiAlpha parameter for Kaiser window
                overlap, //number of samples each fft (other than the first) is to re-use from the previous
                clip, //number of fft output bins to be clipped from EACH side of each sub-span
                span_clip_l, //number of bins to clip from low end of entire span
                span_clip_h, //number of bins to clip from high end of entire span
                pixels, //number of pixel values to return.  may be either <= or > number of bins
                stitches, //number of sub-spans to concatenate to form a complete span
                WDSP.NO_AVERAGING, //averaging mode
                display_average, //number of spans to (moving) average for pixel result
                avb, //back multiplier for weighted averaging
                calibration_data_set, //identifier of which set of calibration data to use
                span_min_freq, //frequency at first pixel value8192
                span_max_freq, //frequency at last pixel value
                max_w //max samples to hold in input ring buffers
        );

    }

    public void initializeLocalAudioOutput() {

        try {
            audioformat = new AudioFormat(AudioFormat.Encoding.PCM_SIGNED, 48000F, 16, 2, 4, 48000F, true);
            audioline = AudioSystem.getSourceDataLine(audioformat);
            audioline.open(audioformat, 48000);
            audioline.start();
        } catch (Exception e) {
            Log.i("Metis", "initializeLocalAudioOutput: " + e.toString());
        }

    }
    
    public void stopLocalAudioOutput() {
        if(audioline!=null) {
            audioline.flush();
            audioline.close();
            audioline=null;
        }
    }

    public void setPTTListener(PTTListener listener) {
        this.pttListener = listener;
    }

    public synchronized void setPixels(int pixels) {
        int flp[] = {0};
        double KEEP_TIME = 0.1;
        int spur_elimination_ffts = 1;
        int data_type = 1;
        int fft_size = 8192;
        int window_type = 4;
        double kaiser_pi = 14.0;
        int overlap = 2048;
        int clip = 0;
        int span_clip_l = 0;
        int span_clip_h = 0;
        //int pixels=1280;
        int stitches = 1;
        int avm = 0;
        double tau = 0.001 * 120.0;
        int MAX_AV_FRAMES = 60;
        int display_average = Math.max(2, (int) Math.min((double) MAX_AV_FRAMES, (double) configuration.fps * tau));
        double avb = Math.exp(-1.0 / (configuration.fps * tau));
        int calibration_data_set = 0;
        double span_min_freq = 0.0;
        double span_max_freq = 0.0;

        this.pixels = pixels;

        int max_w = fft_size + (int) Math.min(KEEP_TIME * (double) configuration.fps, KEEP_TIME * (double) fft_size * (double) configuration.fps);

        wdsp.SetAnalyzer(Display.RX,
                spur_elimination_ffts, //number of LO frequencies = number of ffts used in elimination
                data_type, //0 for real input data (I only); 1 for complex input data (I & Q)
                flp, //vector with one elt for each LO frequency, 1 if high-side LO, 0 otherwise
                fft_size, //size of the fft, i.e., number of input samples
                configuration.buffersize, //number of samples transferred for each OpenBuffer()/CloseBuffer()
                window_type, //integer specifying which window function to use
                kaiser_pi, //PiAlpha parameter for Kaiser window
                overlap, //number of samples each fft (other than the first) is to re-use from the previous
                clip, //number of fft output bins to be clipped from EACH side of each sub-span
                span_clip_l, //number of bins to clip from low end of entire span
                span_clip_h, //number of bins to clip from high end of entire span
                pixels, //number of pixel values to return.  may be either <= or > number of bins
                stitches, //number of sub-spans to concatenate to form a complete span
                avm, //averaging mode
                display_average, //number of spans to (moving) average for pixel result
                avb, //back multiplier for weighted averaging
                calibration_data_set, //identifier of which set of calibration data to use
                span_min_freq, //frequency at first pixel value8192
                span_max_freq, //frequency at last pixel value
                max_w //max samples to hold in input ring buffers
        );

        wdsp.SetAnalyzer(Display.BS,
                spur_elimination_ffts, //number of LO frequencies = number of ffts used in elimination
                WDSP.REAL, //0 for real input data (I only); 1 for complex input data (I & Q)
                flp, //vector with one elt for each LO frequency, 1 if high-side LO, 0 otherwise
                BS_FFT_SIZE, //size of the fft, i.e., number of input samples
                BS_BUFFER_SIZE, //number of samples transferred for each OpenBuffer()/CloseBuffer()
                WDSP.BLACKMAN_HARRIS, //integer specifying which window function to use (Blackman-Harris)
                kaiser_pi, //PiAlpha parameter for Kaiser window
                overlap, //number of samples each fft (other than the first) is to re-use from the previous
                clip, //number of fft output bins to be clipped from EACH side of each sub-span
                span_clip_l, //number of bins to clip from low end of entire span
                span_clip_h, //number of bins to clip from high end of entire span
                pixels, //number of pixel values to return.  may be either <= or > number of bins
                stitches, //number of sub-spans to concatenate to form a complete span
                WDSP.NO_AVERAGING, //averaging mode
                display_average, //number of spans to (moving) average for pixel result
                avb, //back multiplier for weighted averaging
                calibration_data_set, //identifier of which set of calibration data to use
                span_min_freq, //frequency at first pixel value8192
                span_max_freq, //frequency at last pixel value
                max_w //max samples to hold in input ring buffers
        );

    }

    // run the thread to read/write samples from/to Metis
    public void run() {
        int status;
        long sequence;
        int endpoint;
        Log.i("Metis", "run: " + this.getName());

        myaddress = getLocalIpAddress();
        Log.i("Metis", "run: myAddress:" + myaddress);

        running = true;

        time = System.currentTimeMillis();

        try {
            InetSocketAddress socketaddress = new InetSocketAddress(myaddress, myport);

            //Log.i("Metis","run: socketaddress: "+socketaddress.toString());
            socket = new DatagramSocket(socketaddress);
            socket.setReuseAddress(true);
            socket.setBroadcast(true);
            socket.setSoTimeout(0);
            InetAddress address = InetAddress.getByName(configuration.discovered.getAddress());
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
                                //Log.i("Metis","run: EP6 sequence error: expected:"+ep6sequence+" got:"+sequence);
                                ep6sequence = sequence;
                            }
                            demuxBuffer(rxbuffer, 8);
                            demuxBuffer(rxbuffer, 520);
                        } else if (endpoint == 4) {
                            processBandscope(rxbuffer, 8);
                            processBandscope(rxbuffer, 520);
                        }
                    } else {
                        Log.i("Metis", "run: received unknown status: " + status);
                    }
                } else {
                    Log.i("Metis", "run: received unknown packet: length:" + rxdatagram.getLength());
                    for (int i = 0; i < rxdatagram.getLength(); i++) {
                        Log.i("Metis", Integer.toString(i) + ":" + Integer.toHexString(rxbuffer[i] & 0xFF));
                    }
                }
            }

        } catch (SocketException se) {
            Log.i("Metis", "run: " + se.toString());
        } catch (UnknownHostException uhe) {
            Log.i("Metis", "run: " + uhe.toString());
        } catch (IOException ioe) {
            Log.i("Metis", "run: " + ioe.toString());
        }

        //Log.i("Metis","run ending: "+this.getName());

        /*
         if (audiotrack != null) {
         audiotrack.stop();
         audiotrack.release();
         }
         */
        // send stop command
        commandbuffer[0] = (byte) 0xEF;
        commandbuffer[1] = (byte) 0xFE;
        commandbuffer[2] = (byte) 0x04;
        commandbuffer[3] = (byte) 0x00;
        for (int i = 4; i < 64; i++) {
            commandbuffer[i] = (byte) 0x00;
        }
        sendCommand();

        socket.close();

        wdsp.DestroyAnalyzer(Display.RX);
        wdsp.DestroyAnalyzer(Display.TX);
        wdsp.DestroyAnalyzer(Display.BS);
        
        stopLocalAudioOutput();

    }

    public void terminate() {
        running = false;
    }

    public boolean isRunning() {
        return running;
    }

    public void setTransmit(boolean transmit, boolean tuning) {
        //Log.i("Metis", "setTransmit: transmit=" + transmit + " tuning=" + tuning);
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
            Band band = configuration.bands.get();
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
            for (int i = offset; i < offset + 512; i++) {
                switch (state) {
                    case STATE_SYNC0:
                        if (bytes[i] != SYNC) {
                            Log.i("Metis", "SYNC error: offset:" + offset + ":" + String.format("%02X", bytes[i]));
                            state = STATE_SYNC_ERROR;
                        } else {
                            state++;
                        }
                        break;
                    case STATE_SYNC1:
                        if (bytes[i] != SYNC) {
                            Log.i("Metis", "SYNC error: offset:" + offset + ":" + String.format("%02X", bytes[i]));
                            state = STATE_SYNC_ERROR;
                        } else {
                            state++;
                        }
                        break;
                    case STATE_SYNC2:
                        if (bytes[i] != SYNC) {
                            Log.i("Metis", "SYNC error: offset:" + offset + ":" + String.format("%02X", bytes[i]));
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
                            Log.i("Metis", "ptt changed=" + ptt);
                            last_ptt = ptt;
                        }
                        if (dash != last_dash) {
                            Log.i("Metis", "dash=" + dash);
                            last_dash = dash;
                        }
                        if (dot != last_dot) {
                            Log.i("Metis", "dot=" + dot);
                            last_dot = dot;
                        }

                        lt2208_overflow = (rxcontrol1 & 0x01) == 0x01;

                        byte address = (byte) ((rxcontrol0 >> 3) & 0x1F);
                        switch (address) {
                            case 0:
                                if (mercury_software_version != rxcontrol2) {
                                    mercury_software_version = rxcontrol2;
                                    //Log.i("Metis","Mercury: "+rxcontrol2);
                                }
                                if (penelope_software_version != rxcontrol3) {
                                    penelope_software_version = rxcontrol3;
                                    //Log.i("Metis","Penelope: "+rxcontrol3);
                                }
                                if (metis_software_version != rxcontrol4) {
                                    metis_software_version = rxcontrol4;
                                    //Log.i("Metis","Metis: "+rxcontrol4);
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
                        state++;
                        break;
                    case STATE_M0:
                        msample = bytes[i] << 8; // keep sign ????
                        state++;
                        break;
                    case STATE_M1:
                        msample |= bytes[i] & 0xFF;

                        // we now have an I, Q and Microphone sample
                        inlsamples[inoffset] = (float) isample / 8388607.0F; // 24 bit sample convert to -1..+1
                        inrsamples[inoffset] = (float) qsample / 8388607.0F; // 24 bit sample convert to -1..+1
                        inmiclsamples[inoffset] = (float) msample / 32767.0F * configuration.micgain; // 16 bit sample convert to -1..+1
                        inmicrsamples[inoffset] = 0.0F;

                        inoffset++;

                        if (inoffset == configuration.buffersize) {

                            if (transmit) {
                                if (tuning) {
                                    Filter filter = Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());
                                    tunefrequency = filter.getLow() + ((filter.getHigh() - filter.getLow()) / 2);
                                    phase = sineWave(inmiclsamples, inmiclsamples.length, phase, tunefrequency);

                                    wdsp.fexchange2(Channel.TX, inmiclsamples, inmicrsamples, outlsamples, outrsamples, error);
                                    if (error[0] != 0) {
                                        //Log.i("Metis", "fexchange2 returned " + error[0]);
                                    }

                                } else if (configuration.micsource == Configuration.MIC_SOURCE_RADIO) {

                                    wdsp.fexchange2(Channel.TX, inmiclsamples, inmicrsamples, outlsamples, outrsamples, error);
                                    if (error[0] != 0) {
                                        //Log.i("Metis", "fexchange2 returned " + error[0]);
                                    }

                                }

                                wdsp.Spectrum(Display.TX, 0, 0, outrsamples, outlsamples);

                            } else {
                                wdsp.fexchange2(Channel.RX, inlsamples, inrsamples, outlsamples, outrsamples, error);
                                if (error[0] != 0) {
                                    //Log.i("Metis", "fexchange2 returned " + error[0]);
                                }
                                if (configuration.subrx) {
                                    wdsp.fexchange2(Channel.SUBRX, inlsamples, inrsamples, suboutlsamples, suboutrsamples, error);
                                    if (error[0] != 0) {
                                        //Log.i("Metis", "Channel.SUBRX fexchange2 returned " + error[0]);
                                    }
                                }
                                //Log.i("Metis","calling Spectrum with "+inoffset+" samples");
                                wdsp.Spectrum(Display.RX, 0, 0, inrsamples, inlsamples);
                            }

                            if (transmit) {
                                if (tuning || configuration.micsource == Configuration.MIC_SOURCE_RADIO) {
                                    sendSamples(outlsamples, outrsamples);
                                }
                            } else {
                                if (configuration.audiooutput == Configuration.AUDIO_OUTPUT_RADIO || configuration.audiooutput == Configuration.AUDIO_OUTPUT_BOTH) {
                                    if (configuration.subrx) {
                                        sendSamples(outlsamples, suboutrsamples);
                                    } else {
                                        sendSamples(outlsamples, outrsamples);
                                    }
                                }
                                if (configuration.audiooutput == Configuration.AUDIO_OUTPUT_LOCAL || configuration.audiooutput == Configuration.AUDIO_OUTPUT_BOTH) {
                                    for (int j = 0; j < outlsamples.length; j++) {
                                        short lsample;
                                        short rsample;
                                        if (configuration.subrx) {
                                            lsample = (short) (outlsamples[j] * 32767.0F * configuration.afgain);
                                            rsample = (short) (suboutrsamples[j] * 32767.0F * configuration.afgain);
                                            //audiooutput[audiooutputindex++] = (short) (outlsamples[j] * 32767.0F * configuration.afgain);
                                            //audiooutput[audiooutputindex++] = (short) (suboutrsamples[j] * 32767.0F * configuration.afgain);
                                        } else {
                                            lsample = (short) (outlsamples[j] * 32767.0F * configuration.afgain);
                                            rsample = (short) (outrsamples[j] * 32767.0F * configuration.afgain);
                                            //audiooutput[audiooutputindex++] = (short) (outlsamples[j] * 32767.0F * configuration.afgain);
                                            //audiooutput[audiooutputindex++] = (short) (outrsamples[j] * 32767.0F * configuration.afgain);
                                        }
                                        audiooutput[audiooutputindex++] = (byte) ((lsample >> 8) & 0xFF);
                                        audiooutput[audiooutputindex++] = (byte) (lsample & 0xFF);
                                        audiooutput[audiooutputindex++] = (byte) ((rsample >> 8) & 0xFF);
                                        audiooutput[audiooutputindex++] = (byte) (rsample & 0xFF);
                                        if (audiooutputindex == audiooutput.length) {

                                            if (audioline != null) {
                                                int sent = audioline.write(audiooutput, 0, audiooutput.length);
                                                if (sent != audiooutput.length) {
                                                    Log.i("Metis", "write audio returned " + sent + " when sending " + audiooutput.length);
                                                }
                                            }
                                            audiooutputindex = 0;
                                        }
                                    }
                                }
                                if (configuration.audiooutput == Configuration.AUDIO_OUTPUT_LOCAL) {
                                    for (int j = 0; j < outlsamples.length; j++) {
                                        outlsamples[j] = outrsamples[j] = 0;
                                    }
                                    sendSamples(outlsamples, outrsamples);
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
        //Log.i("Metis","sendSamples: "+outlsamples.length);
        Band band = configuration.bands.get();
        BandStack bandstack = band.get();
        float rfgain = 1.0F;  // for PENNYLANE
        if (configuration.radio == Configuration.METIS_PENELOPE || configuration.radio == Configuration.METIS_PENELOPE_ALEX) {
            rfgain = configuration.drive;
            if (tuning) {
                rfgain = configuration.tunegain;
            }
            rfgain = rfgain * configuration.bands.get().getDrive();
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
                short l = (short) (outlsamples[j] * 32767.0F * configuration.afgain);
                short r = (short) (outrsamples[j] * 32767.0F * configuration.afgain);
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
                        sendbuffer[13] = (byte) (txcontrol2 | ((transmit ? configuration.bands.get().getOCTx() : configuration.bands.get().getOCRx()) << 1));
                        byte tx3 = 0x00;
                        switch (configuration.discovered.getDevice()) {
                            case Discovered.DEVICE_HERMES_LITE:
                                tx3 |= (byte) (ALEX_ATTENUATION_0DB | configuration.random);
                                if (configuration.random == LT2208_RANDOM_OFF) { // AGC off
                                    if (lna_dither[configuration.attenuation + 12]) {
                                        tx3 |= LT2208_DITHER_ON;
                                    }
                                }
                                break;
                            case Discovered.DEVICE_HERMES:
                            case Discovered.DEVICE_ANGELIA:
                            case Discovered.DEVICE_ORION:
                                tx3 |= (byte) (configuration.dither | configuration.random | configuration.preamp);
                                if (configuration.attenuation > 31) {
                                    tx3 |= (byte) ALEX_ATTENUATION_30DB;
                                }
                                break;
                            case Discovered.DEVICE_METIS:
                                tx3 |= (byte) (configuration.dither | configuration.random | configuration.preamp);
                                switch (configuration.attenuation) {
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
                        if (configuration.radio != Configuration.METIS_PENELOPE
                                && configuration.radio != Configuration.METIS_PENNYLANE) {
                            BandStack bs = configuration.bands.get().get();
                            switch (bs.getRxAntenna()) {
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
                        if (configuration.radio != Configuration.METIS_PENELOPE
                                && configuration.radio != Configuration.METIS_PENNYLANE) {
                            BandStack bs = configuration.bands.get().get();
                            switch (bs.getTxAntenna()) {
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
                        command++;

                        break;
                    }
                    case 1: {
                        sendbuffer[11] = 0x12;
                        sendbuffer[12] = 0x00;
                        if (tuning) {
                            sendbuffer[12] = (byte) (255.0F * configuration.bands.get().getDrive() * configuration.tunegain);
                        } else if (transmit) {
                            sendbuffer[12] = (byte) (255.0F * configuration.bands.get().getDrive() * configuration.drive);
                        }
                        byte c2 = 0x00;
                        if (configuration.micboost) {
                            c2 |= 0x01;
                        }
                        if (configuration.discovered.getDevice() == Discovered.DEVICE_HERMES) {
                            if (configuration.radio == Configuration.HERMES_APOLLO) {
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
                            c1 = (byte) ((configuration.oriontipring << 4) | (configuration.orionmicbias << 5) | (configuration.orionmicptt << 6));
                        }
                        sendbuffer[12] = c1;
                        sendbuffer[13] = 0;
                        sendbuffer[14] = 0;

                        byte c4 = 0x00;
                        switch (configuration.discovered.getDevice()) {
                            case Discovered.DEVICE_HERMES_LITE:
                                if (configuration.random == LT2208_RANDOM_OFF) {
                                    c4 = lna_att[configuration.attenuation + 12];
                                    //c4 |= (byte) 0x20; // enable
                                }
                                break;
                            case Discovered.DEVICE_HERMES:
                            case Discovered.DEVICE_ANGELIA:
                            case Discovered.DEVICE_ORION:

                                if (configuration.attenuation > 31) {
                                    c4 = (byte) (configuration.attenuation - 30);
                                } else {
                                    c4 = (byte) (configuration.attenuation);
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
                        sendbuffer[12] = 0x00;
                        sendbuffer[13] = 0x00;
                        if (configuration.cwkeysreversed != 0) {
                            sendbuffer[13] |= 0x40;
                        }
                        sendbuffer[14] = (byte) (configuration.cwkeyerspeed | (configuration.cwkeyermode << 6));
                        sendbuffer[15] = (byte) (configuration.cwkeyerweight | (configuration.cwkeyerspacing << 7));
                        command++;
                        break;
                    }
                    case 4: {
                        sendbuffer[11] = 0x1C;
                        sendbuffer[12] = (byte) (0x00 & (rx1adc & 0x03));
                        sendbuffer[13] = 0x00;
                        sendbuffer[14] = 0x00;
                        sendbuffer[15] = 0x00;
                        command++;
                        break;
                    }
                    case 5: {
                        sendbuffer[11] = 0x1E;
                        int mode = bandstack.getMode();
                        sendbuffer[12] = (byte) configuration.cwinternal;
                        if ((mode != Modes.CWU && mode != Modes.CWL) || tuning || transmit) {
                            sendbuffer[12] = (byte) 0;
                        }
                        sendbuffer[13] = (byte) configuration.cwsidetonevolume;
                        sendbuffer[14] = (byte) configuration.cwpttdelay;
                        sendbuffer[15] = 0x00;
                        command++;
                        break;
                    }
                    case 6: {
                        sendbuffer[11] = 0x20;
                        sendbuffer[12] = (byte) configuration.cwhangtime;
                        sendbuffer[13] = (byte) (configuration.cwhangtime >> 8);
                        sendbuffer[14] = (byte) configuration.cwsidetonefrequency;
                        sendbuffer[15] = (byte) (configuration.cwsidetonefrequency >> 8);
                        command = 0;
                        break;
                    }
                }

                // turn on transmit if needed
                sendbuffer[11] |= mox;

                // put in the frequency
                long frequency = bandstack.getFrequency();
                if (band instanceof Transverter) {
                    Transverter xvtr = (Transverter) band;
                    frequency = frequency - xvtr.getIfFrequency();
                }

                sendbuffer[520] = SYNC;
                sendbuffer[521] = SYNC;
                sendbuffer[522] = SYNC;

                switch (freqcommand) {
                    case 0: { // rx frequency
                        sendbuffer[523] = (byte) (txcontrol0 | ((configuration.receiver + 2) << 1) | mox);
                        sendbuffer[524] = (byte) ((frequency >> 24) & 0xFF);
                        sendbuffer[525] = (byte) ((frequency >> 16) & 0xFF);
                        sendbuffer[526] = (byte) ((frequency >> 8) & 0xFF);
                        sendbuffer[527] = (byte) (frequency & 0xFF);
                        freqcommand++;
                        break;
                    }
                    case 1: { // tx frequency
                        if (bandstack.getMode() == Modes.CWL) {
                            frequency = frequency - configuration.cwsidetonefrequency;
                        } else if (bandstack.getMode() == Modes.CWU) {
                            frequency = frequency + configuration.cwsidetonefrequency;
                        }
                        sendbuffer[523] = (byte) (txcontrol0 | 0x02 | mox);
                        sendbuffer[524] = (byte) ((frequency >> 24) & 0xFF);
                        sendbuffer[525] = (byte) ((frequency >> 16) & 0xFF);
                        sendbuffer[526] = (byte) ((frequency >> 8) & 0xFF);
                        sendbuffer[527] = (byte) (frequency & 0xFF);
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
            Log.i("Metis", "send: " + se.toString());
        } catch (UnknownHostException uhe) {
            Log.i("Metis", "send: " + uhe.toString());
        } catch (IOException ioe) {
            Log.i("Metis", "send: " + ioe.toString());
        }
        txsequence++;
    }

    public synchronized void sendCommand() {
        //Log.i("Metis","sendCommand: "+Integer.toHexString(commandbuffer[0]&0xFF)+":"+
        //              Integer.toHexString(commandbuffer[1]&0xFF)+":"+
        //              Integer.toHexString(commandbuffer[2]&0xFF)+":"+
        //              Integer.toHexString(commandbuffer[3]&0xFF));
        try {
            socket.send(commanddatagram);
        } catch (SocketException se) {
            Log.i("Metis", "sendCommand: " + se.toString());
        } catch (UnknownHostException uhe) {
            Log.i("Metis", "sendCommand: " + uhe.toString());
        } catch (IOException ioe) {
            Log.i("Metis", "sendCommand: " + ioe.toString());
        }
    }

    private synchronized void prime() {
        // prime the device so it starts sending samples to us
        sendbuffer[0] = (byte) 0xEF;
        sendbuffer[1] = (byte) 0xFE;
        sendbuffer[2] = (byte) 0x01;
        sendbuffer[3] = (byte) 0x02;  // to EP2

        sendbuffer[4] = (byte) ((txsequence >> 24) & 0xF);
        sendbuffer[5] = (byte) ((txsequence >> 16) & 0xF);
        sendbuffer[6] = (byte) ((txsequence >> 8) & 0xF);
        sendbuffer[7] = (byte) (txsequence & 0xF);

        for (int i = 0; i < 2; i++) {
            sendbuffer[(i * 512) + 8] = SYNC;
            sendbuffer[(i * 512) + 9] = SYNC;
            sendbuffer[(i * 512) + 10] = SYNC;
            sendbuffer[(i * 512) + 11] = txcontrol0;
            sendbuffer[(i * 512) + 12] = txcontrol1;
            sendbuffer[(i * 512) + 13] = txcontrol2;
            sendbuffer[(i * 512) + 14] = txcontrol3;
            sendbuffer[(i * 512) + 15] = txcontrol4;
            for (int j = 0; j < 504; j++) {
                sendbuffer[(i * 512) + 16 + j] = 0;
            }
        }

        send();

        sendbuffer[4] = (byte) ((txsequence >> 24) & 0xF);
        sendbuffer[5] = (byte) ((txsequence >> 16) & 0xF);
        sendbuffer[6] = (byte) ((txsequence >> 8) & 0xF);
        sendbuffer[7] = (byte) (txsequence & 0xF);

        send();

        // send start command
        commandbuffer[0] = (byte) 0xEF;
        commandbuffer[1] = (byte) 0xFE;
        commandbuffer[2] = (byte) 0x04;
        //if (bandscope) {
        commandbuffer[3] = (byte) 0x03;
        //} else {
        //    commandbuffer[3] = (byte) 0x01;
        //}
        for (int i = 4; i < 64; i++) {
            commandbuffer[i] = (byte) 0x00;
        }

        sendCommand();

    }

    private String getLocalIpAddress() {
        String result = "";
        try {
            for (Enumeration<NetworkInterface> en = NetworkInterface.getNetworkInterfaces(); en.hasMoreElements();) {
                NetworkInterface intf = en.nextElement();
                for (Enumeration<InetAddress> enumIpAddr = intf.getInetAddresses(); enumIpAddr.hasMoreElements();) {
                    InetAddress inetAddress = enumIpAddr.nextElement();
                    if (!inetAddress.isLoopbackAddress()) {
                        result = inetAddress.getHostAddress().toString();
                        //Log.i("Metis","getLocalIpAddress: "+result);
                        if (result.contains(".")) {
                            return result;
                        }
                    }
                }
            }
        } catch (Exception ex) {
            Log.i("Metis", "getLocalIpAddress:" + ex.toString());
        }
        return result;
    }

    double sineWave(float[] buf, int samples, double phase, double freq) {
        double phase_step = 2 * Math.PI * freq / configuration.samplerate;
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

    public synchronized boolean Process_Panadapter(int channel, float[] samples) {
        int[] result = new int[1];

        if (samples.length != pixels) {
            Log.i("Metis", "Process_Panadapter: channel=" + channel + " samples=" + samples.length + " pixels=" + pixels);
            return false;
        }

        wdsp.GetPixels(channel, samples, result);

        return (result[0] == 1);
    }

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

    private int pixels;
    private int bandscopepixels;

    private PTTListener pttListener;

    private boolean dbg = false;

    private double phase = 0.0;
    private double tunefrequency = 600;

    private Configuration configuration;

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

    private int receivers = 1;
    private int receiver = 0;

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

    private float[] inlsamples;
    private float[] inrsamples;
    private float[] inmiclsamples;
    private float[] inmicrsamples;
    private float[] outlsamples;
    private float[] outrsamples;
    private float[] suboutlsamples;
    private float[] suboutrsamples;
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

    private int rx1adc = 0x00; // ADC2

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
