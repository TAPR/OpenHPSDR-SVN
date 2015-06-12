package org.g0orx.openhpsdr;

import org.g0orx.openhpsdr.wdsp.WDSP;

import android.media.AudioFormat;
import android.media.AudioRecord;
import android.media.MediaRecorder.AudioSource;
import android.util.Log;

public class Microphone extends Thread {


    public Microphone(Metis metis, int channel) {

        Log.i("Microphone", "channel:" + channel);

        this.metis = metis;
        this.txchannel = channel;

        configuration = Configuration.getInstance();
        wdsp = WDSP.getInstance();

        mBufferSize = AudioRecord.getMinBufferSize(SAMPLE_RATE, AudioFormat.CHANNEL_IN_MONO,
                AudioFormat.ENCODING_PCM_16BIT);

        Log.i("Microphone", "min buffersize:" + mBufferSize);

        mAudioBuffer = new short[mBufferSize / 2];
        inlsamples = new float[configuration.fftsize];
        inrsamples = new float[configuration.fftsize];
        outlsamples = new float[configuration.fftsize];
        outrsamples = new float[configuration.fftsize];
        index = 0;
    }

    public void run() {

        android.os.Process.setThreadPriority(android.os.Process.THREAD_PRIORITY_AUDIO);

        AudioRecord microphone = new AudioRecord(AudioSource.MIC, SAMPLE_RATE,
                AudioFormat.CHANNEL_IN_MONO, AudioFormat.ENCODING_PCM_16BIT, mBufferSize);

        if (microphone.getState() == AudioRecord.STATE_UNINITIALIZED) {
            Log.e("Microphone", "run: cannot initialzie microphone audio");
            return;
        }

        microphone.startRecording();

        time = System.currentTimeMillis();

        while (metis.isTransmitting() && !metis.isTuning()) {
            int read = microphone.read(mAudioBuffer, 0, mAudioBuffer.length);

            if (read != mAudioBuffer.length) {
                Log.i("Microphone", "expected:" + mAudioBuffer.length + " got:" + read);
            }

            for (int i = 0; i < read; i++) {
                float sample = (float) mAudioBuffer[i];
                inlsamples[index] = sample / 32767.0F * configuration.micgain; // convert 16 bit samples to -1.0 .. +1.0
                inrsamples[index++] = 0.0F; /*sample/32767.0F*configuration.micgain;*/ // convert 16 bit samples to -1.0 .. +1.0
                if (index == configuration.fftsize) {
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
        microphone.stop();
        Log.i("Microphone", "thread ended");
    }

    public static final int SAMPLE_RATE = 48000;

    public float[] getLeftSamples() {
        return outlsamples;
    }

    public float[] getRightSamples() {
        return outrsamples;
    }

    private int mBufferSize;
    private short[] mAudioBuffer;

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

}
