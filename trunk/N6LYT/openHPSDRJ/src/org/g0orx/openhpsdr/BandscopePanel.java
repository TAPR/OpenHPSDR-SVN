/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.g0orx.openhpsdr;

import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Stroke;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;

import org.g0orx.openhpsdr.discovery.Discovered;

/**
 *
 * @author john
 */
public class BandscopePanel extends javax.swing.JPanel implements ComponentListener {

    public BandscopePanel() {
        super();
        this.configuration = Configuration.getInstance();
        setVisible(true);
    }

    public void setMetis(Metis metis) {
        this.metis = metis;
    }

    public void componentShown(ComponentEvent e) {
    }

    public void componentHidden(ComponentEvent e) {
    }

    public void componentResized(ComponentEvent e) {
        if (metis != null) {
            metis.setBandscopePixels(this.getWidth());
        }
    }
    
    public void componentMoved(ComponentEvent e) {
    }
    
    public void paintComponent(Graphics g) {
        if (this.getWidth() == 0 || this.getHeight() == 0) {
            return;
        }

        final Graphics2D g2d = (Graphics2D) g.create();
        try {

            // fill in the background
            //g2d.setTextSize(16.0F);
            g2d.setColor(Color.BLUE);
            g2d.fillRect(0, 0, this.getWidth(), this.getHeight());
            
            

            float hzPerPixel = 62440000F / (float) this.getWidth(); // 124.88 MHz xtal
            if (configuration.discovered!=null && configuration.discovered.getDevice() == Discovered.DEVICE_HERMES_LITE) {
                hzPerPixel = 30720000F / (float) this.getWidth(); // 61.44 MHz xtal
            }

            Band[] bands = configuration.bands.getBands();
            for (int i = 0; i < bands.length; i++) {
                if (bands[i] instanceof Band && bands[i].canTransmit()) {
                    Band band = bands[i];
                    BandEdge edges = band.getBandEdge();
                    int x1 = (int) ((float) edges.getLow() / hzPerPixel);
                    int x2 = (int) ((float) edges.getHigh() / hzPerPixel);
                    if (x1 != x2) {
                        g2d.setColor(Color.DARK_GRAY);
                        g2d.fillRect(x1, 0, x2-x1, this.getHeight() - 30);
                        g2d.setColor(Color.WHITE);
                        g2d.drawString(band.getName(), x1, 20);
                    }
                }
            }

            // plot the spectrum levels
            int V = HIGH - LOW;
            int numSteps = V / 20;
            //this.setTextSize(20.0F);
            Stroke stroke = g2d.getStroke();
            g2d.setStroke(dotted);
            for (int i = 1; i < numSteps; i++) {
                int num = HIGH - i * 20;
                int y = (int) Math.floor((HIGH - num) * this.getHeight() / V);

                g2d.setColor(Color.YELLOW);
                g2d.drawLine(0, y, this.getWidth(), y);

                g2d.setColor(Color.WHITE);
                g2d.drawString(Integer.toString(num) + " dBm", 3, y - 2);
            }

            // plot the vertical frequency markers
            float f = 0;
            String fs;

            int maxfrequency = 65;
            if (configuration.discovered!=null && configuration.discovered.getDevice() == Discovered.DEVICE_HERMES_LITE) {
                maxfrequency = 32;
            }

            for (int i = 2; i < 65; i += 2) {
                f = (float) i * 1000000.0F;
                int x = (int) (f / hzPerPixel);
                g2d.setColor(Color.LIGHT_GRAY);
                g2d.drawLine(x, 0, x, this.getHeight() - 30);
                fs = String.format("%d", i);
                g2d.setColor(Color.WHITE);
                g2d.drawString(fs, x - 10, this.getHeight() - 5);
            }

            g2d.setColor(Color.YELLOW);
            g2d.setStroke(stroke);
            if (xpoints != null) {
                g2d.drawPolyline(xpoints, ypoints, xpoints.length);
            }
        } finally {
            g2d.dispose();
        }

    }

    public void update(float[] samples) {
        //Log.i("BandscopeView","update");

        if (this.getWidth() == samples.length) {

            BandStack bandstack = configuration.bands.get().get();

            if (xpoints == null || xpoints.length != samples.length) {
                xpoints = new int[samples.length];
                ypoints = new int[samples.length];
            }

            int p = 0;
            float sample;
            float previous = 0.0F;

            for (int i = 0; i < this.getWidth(); i++) {

                sample = samples[i];

                sample = sample + (-20.0F);

                sample = (float) Math
                        .floor(((float) HIGH - sample)
                                * (float) this.getHeight()
                                / (float) (HIGH - LOW));
                
                xpoints[i] = i;
                ypoints[i] = (int) sample;
            
            }

            this.repaint();
        }

    }

    private Configuration configuration;
    private Metis metis;

    private static final int HIGH = -40;
    private static final int LOW = -160;

    private int[] xpoints;
    private int[] ypoints;
    
    private Stroke dotted = new BasicStroke(1, BasicStroke.CAP_BUTT, BasicStroke.JOIN_BEVEL, 0, new float[]{1, 4}, 0);

}
