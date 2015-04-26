/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.g0orx.openhpsdr;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import java.awt.image.BufferedImage;
import javax.swing.JPanel;

import org.g0orx.openhpsdr.debug.Log;

/**
 *
 * @author john
 */
public class WaterfallPanel extends JPanel {

     public WaterfallPanel() {
    }
    
    public WaterfallPanel(Radio radio,Receiver rx) {
        super();
        this.radio=radio;
        this.receiver=rx;
    }
    
    public void paintComponent(Graphics g) {
        if (radio==null || this.getWidth() == 0 || this.getHeight() == 0) {
            g.setColor(Color.BLUE);
            g.fillRect(0, 0, this.getWidth(), this.getHeight());
            return;
        }
        Graphics2D g2d = (Graphics2D) g.create();
        if(image==null) {
            g2d.setColor(Color.BLACK);
            g2d.fillRect(0, 0, this.getWidth(), this.getHeight());
        } else {
            g2d.drawImage(image, 0, 0, this);
        }
    }

    public void update(float[] samples) {
        if (samples != null && this.getWidth()!=0 && this.getHeight()!=0) {
            if (samples.length == this.getWidth()) {

                if(image==null || image.getWidth()!=samples.length || image.getHeight()!=this.getHeight()) {
                    image=new BufferedImage(this.getWidth(),this.getHeight(),BufferedImage.TYPE_INT_RGB);
                    pixelswidth=samples.length;
                    pixelsheight=this.getHeight();
                    pixels = new int[pixelswidth * pixelsheight];
                    for (int i = 0; i < pixels.length; i++) {
                        pixels[i] = 0x000000;
                    }
                } else {
                    // move the rows down 1
                    System.arraycopy(pixels, 0, pixels, this.getWidth(), this.getWidth() * (this.getHeight() - 1));
                }

                int p = 0;
                float sample;
                float previous = 0.0F;

                float max = -400.0F;

                int average = 0;
                for (int i = 0; i < samples.length; i++) {
                    float s = samples[i] + radio.displayCalibrationOffset + radio.preampOffset;
                    average += (int) s;
                    pixels[i] = calculatePixel(s);
                }
                if (receiver.isWaterfallAutomatic()) {
                    average = average / samples.length;
                    receiver.setWaterfallLow(average - 5);
                    receiver.setWaterfallHigh(average + 55);
                }
                
                if(this.getWidth()==pixelswidth && this.getHeight()==pixelsheight) {
                    image.setRGB(0, 0, this.getWidth(), this.getHeight(), pixels, 0, this.getWidth());
                }
                
                this.repaint();
            } else {
                Log.i("WaterfallPanel","update: samples:"+samples.length+" width:"+this.getWidth());
            }
        }
    }

    private int calculatePixel(float sample) {
        //Log.i("WaterfallView","calculatePixel:"+sample);

        int R, G, B;

        if (sample < receiver.getWaterfallLow()) {
            R = colorLowR;
            G = colorLowG;
            B = colorLowB;
        } else if (sample > receiver.getWaterfallHigh()) {
            R = colorHighR;
            G = colorHighG;
            B = colorHighB;
        } else {
            float range = receiver.getWaterfallHigh() - receiver.getWaterfallLow();
            float offset = sample - receiver.getWaterfallLow();
            float percent = offset / range;
            if (percent < (2.0f / 9.0f)) {
                float local_percent = percent / (2.0f / 9.0f);
                R = (int) ((1.0f - local_percent) * colorLowR);
                G = (int) ((1.0f - local_percent) * colorLowG);
                B = (int) (colorLowB + local_percent * (255 - colorLowB));
            } else if (percent < (3.0f / 9.0f)) {
                float local_percent = (percent - 2.0f / 9.0f) / (1.0f / 9.0f);
                R = 0;
                G = (int) (local_percent * 255);
                B = 255;
            } else if (percent < (4.0f / 9.0f)) {
                float local_percent = (percent - 3.0f / 9.0f) / (1.0f / 9.0f);
                R = 0;
                G = 255;
                B = (int) ((1.0f - local_percent) * 255);
            } else if (percent < (5.0f / 9.0f)) {
                float local_percent = (percent - 4.0f / 9.0f) / (1.0f / 9.0f);
                R = (int) (local_percent * 255);
                G = 255;
                B = 0;
            } else if (percent < (7.0f / 9.0f)) {
                float local_percent = (percent - 5.0f / 9.0f) / (2.0f / 9.0f);
                R = 255;
                G = (int) ((1.0f - local_percent) * 255);
                B = 0;
            } else if (percent < (8.0f / 9.0f)) {
                float local_percent = (percent - 7.0f / 9.0f) / (1.0f / 9.0f);
                R = 255;
                G = 0;
                B = (int) (local_percent * 255);
            } else {
                float local_percent = (percent - 8.0f / 9.0f) / (1.0f / 9.0f);
                R = (int) ((0.75f + 0.25f * (1.0f - local_percent)) * 255.0f);
                G = (int) (local_percent * 255.0f * 0.5f);
                B = 255;
            }
        }

        if (receiver.isWaterfallGrayscale()) {
            final float GS_RED = 0.299F;
            final float GS_GREEN = 0.587F;
            final float GS_BLUE = 0.114F;
            R = G = B = (int) (GS_RED * R + GS_GREEN * G + GS_BLUE * B);
        }

        int pixel = (255 << 24) + (R << 16) + (G << 8) + B;

        return pixel;
    }

    private Radio radio;
    private Receiver receiver;

    BufferedImage image;
    
    private int pixelswidth=0;
    private int pixelsheight=0;
    int[] pixels;

    private int colorLowR = 0;
    private int colorLowG = 0;
    private int colorLowB = 0;
    private int colorMidR = 255;
    private int colorMidG = 0;
    private int colorMidB = 0;
    private int colorHighR = 255;
    private int colorHighG = 255;
    private int colorHighB = 0;

}
