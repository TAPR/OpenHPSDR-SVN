package org.g0orx.openhpsdr;

import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Event;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Stroke;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;

import org.g0orx.openhpsdr.discovery.Discovered;
import org.g0orx.openhpsdr.modes.Modes;

public class PanadapterPanel extends javax.swing.JPanel {

    public void setMetis(Metis metis) {
        this.metis = metis;
    }

    public void paintComponent(Graphics g) {
        if (this.getWidth() == 0 || this.getHeight() == 0) {
            return;
        }

        final Graphics2D g2d = (Graphics2D) g.create();
        try {

            Configuration configuration = Configuration.getInstance();

            int samplerate = (int) configuration.samplerate;
            if (metis != null && metis.isTransmitting()) {
                samplerate = 48000;
            }

            int spectrumhigh = configuration.spectrumHigh;
            int spectrumlow = configuration.spectrumLow;
            if (metis != null && metis.isTransmitting()) {
                spectrumhigh = 30;
                spectrumlow = -100;
            }

            Band band = configuration.bands.get();
            BandStack bandstack = band.get();
            Filter filter = Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());
            int low = filter.getLow();
            int high = filter.getHigh();
            if (bandstack.getMode() == Modes.CWL) {
                low = -configuration.cwsidetonefrequency - low;
                high = -configuration.cwsidetonefrequency + high;
            } else if (bandstack.getMode() == Modes.CWU) {
                low = configuration.cwsidetonefrequency - low;
                high = configuration.cwsidetonefrequency + high;
            }
            filterLeft = ((int) low - (-samplerate / 2)) * this.getWidth() / samplerate;
            filterRight = ((int) high - (-samplerate / 2)) * this.getWidth() / samplerate;
            if (filterLeft == filterRight) {
                filterRight++;
            }

            long frequency = bandstack.getFrequency();

            // fill in the background
            g2d.setColor(Color.BLUE);
            g2d.fillRect(0, 0, this.getWidth(), this.getHeight());

            // draw the filter
            g2d.setColor(Color.DARK_GRAY);
            g2d.fillRect(filterLeft, 0, filterRight - filterLeft, this.getHeight());

            if (configuration.subrx) {
                // draw the subrx filter
                int offset = (int) ((bandstack.getSubRxFrequency() - bandstack.getFrequency()) / (configuration.samplerate / this.getWidth()));
                g2d.setColor(Color.DARK_GRAY);
                g2d.fillRect(filterLeft + offset, 0, filterRight - filterLeft + offset, this.getHeight());
            }

            // plot the spectrum levels
            int V = spectrumhigh - spectrumlow;
            int numSteps = V / 20;

            Stroke stroke = g2d.getStroke();
            g2d.setStroke(dotted);
            //paint.setTextSize(20.0F);
            for (int i = 1; i < numSteps; i++) {
                int num = spectrumhigh - i * 20;
                int y = (int) Math.floor((spectrumhigh - num) * this.getHeight() / V);

                g2d.setColor(Color.YELLOW);
                g2d.drawLine(0, y, this.getWidth(), y);

                g2d.setColor(Color.WHITE);
                g2d.drawString(Integer.toString(num) + " dBm", 3, y - 2);
            }

            // plot the vertical frequency markers
            float hzPerPixel = samplerate / (float) this.getWidth();
            long f = frequency - ((long) samplerate / 2);
            String fs;
            for (int i = 0; i < this.getWidth(); i++) {
                f = frequency - ((long) samplerate / 2) + (long) (hzPerPixel * i);
                if (f > 0) {
                    if ((f % 20000) < (long) hzPerPixel) {
                        g2d.setColor(Color.YELLOW);

                        //g.setPathEffect(dashPath);
                        //g.setStrokeWidth(1);
                        g2d.drawLine(i, 0, i, this.getHeight());

                        g2d.setColor(Color.WHITE);
                        //g.setPathEffect(null);
                    /*
                         fs = String.format("%d.%02d", f / 1000000, (f % 1000000) / 10000);
                         canvas.drawText(fs, i - 20, this.getHeight() - 5, paint);
                         */
                    }
                }
            }

            g2d.setStroke(stroke);

            // plot the cursor
            g2d.setColor(Color.RED);
            g2d.drawLine((this.getWidth() / 2), 0, (this.getWidth() / 2), this.getHeight());

            /*
             // paint the frequency
             if (metis!=null && metis.isTransmitting()) {
             paint.setColor(Color.RED);
             } else {
             paint.setColor(Color.GREEN);
             }
             paint.setTextSize(paint.getTextSize() * 2.0F);
             canvas.drawText(Frequency.toString(frequency) + " " + Modes.getMode(bandstack.getMode()).getName() + " " + filter.getName()+"Hz", this.getWidth() / 8, 40, paint);

             if (configuration.subrx) {
             paint.setColor(Color.GRAY);
             canvas.drawText(Frequency.toString(bandstack.getSubRxFrequency()), this.getWidth() - (this.getWidth() / 4), 40, paint);
             }
             */
            // plot the band edge
            BandEdge bandedge = band.getBandEdge();
            if (bandedge.getLow() != 0) {
                long minfrequency = frequency - ((long) samplerate / 2);
                long maxfrequency = frequency + ((long) samplerate / 2);
                double hzperpixel = samplerate / (double) this.getWidth();
                if (bandedge.getLow() > minfrequency && bandedge.getLow() < maxfrequency) {
                    // show lower band edge
                    float x = (float) (bandedge.getLow() - minfrequency) / (float) hzperpixel;
                    g2d.drawLine((int) x, 0, (int) x, this.getHeight());
                    g2d.drawLine((int) x + 1, 0, (int) x + 1, this.getHeight());
                }

                if (bandedge.getHigh() > minfrequency && bandedge.getHigh() < maxfrequency) {
                    // show upper band edge
                    float x = (float) (bandedge.getHigh() - minfrequency) / (float) hzperpixel;
                    g2d.drawLine((int) x, 0, (int) x, this.getHeight());
                    g2d.drawLine((int) x + 1, 0, (int) x + 1, this.getHeight());
                }
            }

            // plot the spectrum
            g2d.setColor(Color.WHITE);
            if (xpoints != null) {
                g2d.drawPolyline(xpoints, ypoints, xpoints.length);
            }

            // paint the radio details
            //float sz=paint.getTextSize();
            //paint.setTextSize(sz*2.0F);
        /*
             g.setColor(Color.GRAY);
             Discovered d=configuration.discovered;
             if(d!=null){
             g.drawString(d.getDeviceName() + " " + d.getAddress() + " (" + d.getMac() + ")", 0, 20);
             } else {
             g.drawString("Discovered is null", 0, 20);
             }
             //paint.setTextSize(sz);
             */
            if (metis != null && metis.isADC1Overflow()) {
                g2d.setColor(Color.RED);
                g2d.drawString("ADC Overflow", this.getWidth() - (this.getWidth() / 4), 20);
            }

        } finally {
            g2d.dispose();
        }

    }

    public void plotSpectrum(float[] samples) {

        Configuration configuration = Configuration.getInstance();
        BandStack bandstack = configuration.bands.get().get();

        if (xpoints == null || xpoints.length != samples.length) {
            xpoints = new int[samples.length];
            ypoints = new int[samples.length];
        }

        int spectrumhigh = configuration.spectrumHigh;
        int spectrumlow = configuration.spectrumLow;
        if (metis!=null && metis.isTransmitting()) {
            spectrumhigh = 30;
            spectrumlow = -100;
        }

        float sample;

        float max = -400.0F;

        for (int i = 0; i < samples.length; i++) {

            sample = samples[i];

            sample = sample + configuration.displayCalibrationOffset + configuration.preampOffset;

            if (sample > max) {
                max = sample;
            }

            sample = (float) Math
                    .floor(((float) spectrumhigh - sample)
                            * (float) this.getHeight()
                            / (float) (spectrumhigh - spectrumlow));
            xpoints[i] = i;
            ypoints[i] = (int) sample;
        }

        this.repaint();

    }

    private Stroke dotted = new BasicStroke(1, BasicStroke.CAP_BUTT, BasicStroke.JOIN_BEVEL, 0, new float[]{1, 4}, 0);
    private Metis metis;

    private int[] xpoints;
    private int[] ypoints;

    private int filterLeft;
    private int filterRight;

    private boolean vfoLocked = false;

    private float startX;
    private float startY;
    private boolean moved;
    private boolean scroll;
    private boolean jog;

    //private Timer timer;
    private long jogAmount;

    //private DashPathEffect dashPath = new DashPathEffect(new float[]{1, 4}, 1);
    private long lastfrequency = 0L;
    private int lastfilterleft = 0;
    private int lastfilterright = 0;

}
