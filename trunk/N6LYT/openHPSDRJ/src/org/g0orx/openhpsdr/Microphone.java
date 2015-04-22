/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.g0orx.openhpsdr;

import javax.sound.sampled.AudioFormat;
import javax.sound.sampled.AudioSystem;
import javax.sound.sampled.DataLine;
import javax.sound.sampled.Line;
import javax.sound.sampled.LineUnavailableException;
import javax.sound.sampled.Mixer;
import javax.sound.sampled.SourceDataLine;
import javax.sound.sampled.TargetDataLine;
import static org.g0orx.openhpsdr.Metis.SPEED_192KHZ;
import static org.g0orx.openhpsdr.Metis.SPEED_384KHZ;
import static org.g0orx.openhpsdr.Metis.SPEED_48KHZ;
import static org.g0orx.openhpsdr.Metis.SPEED_96KHZ;

import org.g0orx.openhpsdr.wdsp.WDSP;

/**
 *
 * @author john
 */
public class Microphone extends Thread {
    
    
    public Microphone(Metis metis, int channel) {

        Log.i("Microphone", "channel:" + channel);

        this.metis = metis;
        this.txchannel = channel;

        configuration = Configuration.getInstance();
        wdsp = WDSP.getInstance();
        
        audiobuffer=new byte[configuration.buffersize*2];

        inlsamples = new float[configuration.buffersize];
        inrsamples = new float[configuration.buffersize];
        outlsamples = new float[configuration.buffersize];
        outrsamples = new float[configuration.buffersize];
        index = 0;
    }

    public void run() {

        wdsp.SetAllRates(txchannel,SAMPLE_RATE,SAMPLE_RATE,SAMPLE_RATE);
        
        try {
            //audioformat = new AudioFormat(AudioFormat.Encoding.PCM_SIGNED, 48000F, 16, 1, 2, 48000F, true);
            audioformat = new AudioFormat((float)SAMPLE_RATE, 16, 1, true, true);
            microphoneline = AudioSystem.getTargetDataLine(audioformat);
            microphoneline.open(audioformat, configuration.buffersize*2);
            microphoneline.start();
        } catch (Exception e) {
            Log.i("Microphone", e.toString());
            microphoneline=null;
            return;
        }
                
        time = System.currentTimeMillis();

        while (metis.isTransmitting() && !metis.isTuning()) {
            int bytes = microphoneline.read(audiobuffer, 0, audiobuffer.length);

            if (bytes != audiobuffer.length) {
                Log.i("Microphone", "expected:" + audiobuffer.length + " got:" + bytes);
            }

            for (int i = 0; i < bytes; i+=2) {
                short sample=(short)(((audiobuffer[i]<<8)&0xFF00)| (audiobuffer[i+1]&0xFF));
                inlsamples[index] = (float)sample / 32767.0F * configuration.micgain; // convert 16 bit samples to -1.0 .. +1.0
                inrsamples[index++] = 0.0F; /*sample/32767.0F*configuration.micgain;*/ // convert 16 bit samples to -1.0 .. +1.0
                if (index == inlsamples.length) {
                    // process the microphone buffer
                    int[] error = new int[1];
                    wdsp.fexchange2(txchannel, inlsamples, inrsamples, outlsamples, outrsamples, error);
                    if (error[0] != 0) {
                        Log.i("Microphone", "fechange2 returned " + error[0]);
                    }
                    // send to Metis
                    metis.sendSamples(outlsamples, outrsamples);
                    wdsp.Spectrum(txchannel, 0, 0, outrsamples, outlsamples);
                    index = 0;
                }
            }
        }
        microphoneline.flush();
        microphoneline.close();
        microphoneline=null;
        
        wdsp.SetAllRates(txchannel,(int)configuration.samplerate,(int)configuration.dsprate,SAMPLE_RATE);
        Log.i("Microphone", "thread ended");
    }

    
    public static final int SAMPLE_RATE = 48000;

    private byte[] audiobuffer;

    private float[] inlsamples;
    private float[] inrsamples;
    private int index;

    private float[] outlsamples;
    private float[] outrsamples;

    private WDSP wdsp;

    private Metis metis;

    private int increment;

    private Configuration configuration;

    private long time;
    private int txchannel;

    private AudioFormat audioformat;
    private TargetDataLine microphoneline;
    
}
