package org.g0orx.openhpsdr;

import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Event;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Stroke;
import java.awt.image.BufferedImage;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import org.g0orx.openhpsdr.debug.Log;

import org.g0orx.openhpsdr.discovery.Discovered;

public class PanadapterPanel extends javax.swing.JPanel {

    public PanadapterPanel() {
    }
    
    public PanadapterPanel(Radio radio,Receiver rx) {
        super();
        this.radio=radio;
        this.metis=radio.getMetis();
        this.receiver=rx;
    }

    public void paintComponent(Graphics g) {
        //Log.i(this,"paintComponent");
        if (radio==null || this.getWidth() == 0 || this.getHeight() == 0) {
            g.setColor(Color.BLUE);
            g.fillRect(0, 0, this.getWidth(), this.getHeight());
            return;
        }

        int samplerate = (int) radio.samplerate;
        if (metis != null && metis.isTransmitting()) {
            samplerate = 48000;
        }

        int high = receiver.getSpectrumHigh();
        int low = receiver.getSpectrumLow();
        if (metis != null && metis.isTransmitting()) {
            high = 30;
            low = -100;
        }

        Band band = receiver.getBands().get();
        BandStack bandstack = band.get();
        Filter filter = Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());
        int filterlow = filter.getLow();
        int filterhigh = filter.getHigh();

        if (image == null || (spectrumhigh != high) || (spectrumlow != low)
                || (this.filterhigh!=filterhigh) || (this.filterlow!=filterlow)) {
            
            image = new BufferedImage(this.getWidth(), this.getHeight(), BufferedImage.TYPE_INT_ARGB);
            Graphics2D g2d = image.createGraphics();

            // fill in the background
            g2d.setColor(Color.BLUE);
            g2d.fillRect(0, 0, this.getWidth(), this.getHeight());

            if (bandstack.getMode() == Modes.CWL) {
                filterlow = -radio.cwsidetonefrequency - filterlow;
                filterhigh = -radio.cwsidetonefrequency + filterhigh;
            } else if (bandstack.getMode() == Modes.CWU) {
                filterlow = radio.cwsidetonefrequency - filterlow;
                filterhigh = radio.cwsidetonefrequency + filterhigh;
            }
            filterLeft = ((int) filterlow - (-samplerate / 2)) * this.getWidth() / samplerate;
            filterRight = ((int) filterhigh - (-samplerate / 2)) * this.getWidth() / samplerate;
            if (filterLeft == filterRight) {
                filterRight++;
            }
            
            this.filterlow=filterlow;
            this.filterhigh=filterhigh;

            // draw the filter
            g2d.setColor(Color.DARK_GRAY);
            g2d.fillRect(filterLeft, 0, filterRight - filterLeft, this.getHeight());

            // plot the spectrum levels
            int V = high - low;
            int numSteps = V / 20;

            g2d.setStroke(dotted);
            //paint.setTextSize(20.0F);
            for (int i = 1; i < numSteps; i++) {
                int num = high - i * 20;
                int y = (int) Math.floor((high - num) * this.getHeight() / V);

                g2d.setColor(Color.YELLOW);
                g2d.drawLine(0, y, this.getWidth(), y);

                g2d.setColor(Color.WHITE);
                g2d.drawString(Integer.toString(num) + " dBm", 3, y - 2);
            }

            g2d.dispose();

        }

        Graphics2D g2d = (Graphics2D) g.create();
        Stroke stroke = g2d.getStroke();

        g2d.drawImage(image, 0, 0, this);

        long frequency = bandstack.getFrequency();

        if (radio.subrx) {
            // draw the subrx filter
            int offset = (int) ((bandstack.getSubRxFrequency() - bandstack.getFrequency()) / (radio.samplerate / this.getWidth()));
            g2d.setColor(Color.DARK_GRAY);
            g2d.fillRect(filterLeft + offset, 0, filterRight - filterLeft + offset, this.getHeight());
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
                    g2d.drawLine(i, 0, i, this.getHeight());
                }
            }
        }

        g2d.setStroke(stroke);

        // plot the cursor
        g2d.setColor(Color.RED);
        g2d.drawLine((this.getWidth() / 2), 0, (this.getWidth() / 2), this.getHeight());

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
        } else {
            Log.i(this, "xpoints is null");
        }

        if (metis != null && metis.isADC1Overflow()) {
            g2d.setColor(Color.RED);
            g2d.drawString("ADC Overflow", this.getWidth() - (this.getWidth() / 4), 20);
        }

        g2d.dispose();

    }

    public void plotSpectrum(float[] samples) {

        BandStack bandstack = receiver.getBands().get().get();

        if (xpoints == null || xpoints.length != samples.length) {
            Log.i(this, "plotSpectrum: new points");
            xpoints = new int[samples.length];
            ypoints = new int[samples.length];
        }

        int spectrumhigh = receiver.getSpectrumHigh();
        int spectrumlow = receiver.getSpectrumLow();
        if (metis != null && metis.isTransmitting()) {
            spectrumhigh = 30;
            spectrumlow = -100;
        }

        float sample;

        float max = -400.0F;

        for (int i = 0; i < samples.length; i++) {

            sample = samples[i];

            sample = sample + radio.displayCalibrationOffset + radio.preampOffset;

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
    
    private Radio radio;
    private Receiver receiver;
    private Metis metis;

    private BufferedImage image;
    int spectrumhigh;
    int spectrumlow;

    private int[] xpoints;
    private int[] ypoints;

    private int filterlow;
    private int filterhigh;
    
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
