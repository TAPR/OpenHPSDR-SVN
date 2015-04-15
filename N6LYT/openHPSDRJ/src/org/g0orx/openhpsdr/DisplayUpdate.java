package org.g0orx.openhpsdr;

import org.g0orx.openhpsdr.wdsp.WDSP;

import java.util.Timer;
import java.util.TimerTask;

public class DisplayUpdate {

    public DisplayUpdate(VFOPanel vfoView, PanadapterPanel panadapterView, WaterfallPanel waterfallView, MeterJPanel meterView, /*FrequencyView frequencyView,*/ BandscopePanel bandscopeView, Metis metis) {
        this.vfoView = vfoView;
        this.panadapterView = panadapterView;
        this.waterfallView = waterfallView;
        this.meterView = meterView;
        /*
         this.frequencyView = frequencyView;
         */
        this.bandscopeView = bandscopeView;
        this.metis = metis;
        configuration = Configuration.getInstance();

        wdsp = WDSP.getInstance();

        samples = new float[panadapterView.getWidth()];
        bandscopesamples = new float[bandscopeView.getWidth()];
        Log.i("DisplayUpdate", "fps=" + configuration.fps);
    }

    public void startTimer() {
        running = true;
        final Timer timer = new Timer();
        time = System.currentTimeMillis();
        timer.scheduleAtFixedRate(new TimerTask() {
            public void run() {
                if (running) {
                    update();
                } else {
                    timer.cancel();
                    Log.i("DisplayUpdate", "Cancel timer");
                }
            }
        }, 0, 1000 / configuration.fps);
    }

    
    private void update() {
        if (samples.length != panadapterView.getWidth()) {
            samples = new float[panadapterView.getWidth()];
            metis.setPixels(panadapterView.getWidth());
        }
        
        if(bandscopesamples.length!=bandscopeView.getWidth()) {
            bandscopesamples = new float[bandscopeView.getWidth()];
            metis.setBandscopePixels(bandscopeView.getWidth());
        }
        
        if (metis.isTransmitting()) {
            if (metis.getDisplaySamples(Display.TX, samples)) {
                if (panadapterView != null) {
                    panadapterView.plotSpectrum(samples);
                }
                meterView.setPower(metis.getPenelopeForwardPower(), metis.getAlexForwardPower(), metis.getAlexReversePower());
            }
        } else {
            if (metis.getDisplaySamples(Display.RX, samples)) {
                if (panadapterView != null) {
                    panadapterView.plotSpectrum(samples);
                }
                if (waterfallView != null) {
                    waterfallView.update(samples);
                }
                int meter = (int) wdsp.GetRXAMeter(configuration.subrx ? Channel.SUBRX : Channel.RX, WDSP.S_AV);
                meterView.setMeter(meter);
            }

            if (bandscopeView != null) {
                if (metis.getDisplaySamples(Display.BS, bandscopesamples)) {
                    bandscopeView.update(bandscopesamples);
                }
            }

        }

    }

    public boolean isRunning() {
        return running;
    }

    public void terminate() {
        running = false;
    }

    private Configuration configuration;

    private boolean running;
    private float[] samples;
    private int meter = 0;

    private float[] bandscopesamples;

    private VFOPanel vfoView;
    private PanadapterPanel panadapterView;
    private WaterfallPanel waterfallView;
    private MeterJPanel meterView;
    /*
     private FrequencyView frequencyView;
     */
    private BandscopePanel bandscopeView;
    private Metis metis;
    private WDSP wdsp;

    private int frames = 0;

    private long time;

}
