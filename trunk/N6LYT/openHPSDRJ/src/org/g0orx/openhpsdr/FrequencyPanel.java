package org.g0orx.openhpsdr;

import java.awt.Color;
import java.awt.Graphics;
import javax.swing.JPanel;

import org.g0orx.openhpsdr.modes.Modes;

public class FrequencyPanel extends JPanel {
    
    public void paintComponent(Graphics g) {
        if (this.getWidth() == 0 || this.getHeight() == 0) {
            return;
        }

        Configuration configuration=Configuration.getInstance();
        
        Band band = configuration.bands.get();
        BandStack bandstack = band.get();
        Filter filter = Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());
        int low=filter.getLow();
        int high=filter.getHigh();
        if(bandstack.getMode()==Modes.CWL) {
            low=-configuration.cwsidetonefrequency-low;
            high=-configuration.cwsidetonefrequency+high;
        } else if(bandstack.getMode()==Modes.CWU) {
            low=configuration.cwsidetonefrequency-low;
            high=configuration.cwsidetonefrequency+high;
        }
        int filterLeft = ((int) low - (-(int) configuration.samplerate / 2)) * this.getWidth() / (int) configuration.samplerate;
        int filterRight = ((int) high - (-(int) configuration.samplerate / 2)) * this.getWidth() / (int) configuration.samplerate;
        if (filterLeft == filterRight) {
            filterRight++;
        }

        long frequency = bandstack.getFrequency();

        // fill in the background
        //paint.setTextSize(20.0F);
        g.setColor(Color.BLACK);
        g.fillRect(0, 0, this.getWidth(), this.getHeight());

        // draw the filter
        g.setColor(Color.DARK_GRAY);
        g.fillRect(filterLeft, 0, filterRight-filterLeft, this.getHeight());

        g.setColor(Color.WHITE);
        //g.setStrokeWidth(1);


        // plot the vertical frequency markers
        float hzPerPixel = (float) configuration.samplerate / (float) this.getWidth();
        long f = frequency - ((long) configuration.samplerate / 2);
        String fs;
        for (int i = 0; i < this.getWidth(); i++) {
            f = frequency - ((long) configuration.samplerate / 2) + (long) (hzPerPixel * i);
            if (f > 0) {
                if ((f % 20000) < (long) hzPerPixel) {
                    g.drawLine(i, 0, i, this.getHeight() - 20);
                    fs = String.format("%d.%02d", f / 1000000, (f % 1000000) / 10000);
                    g.drawString(fs, i - 20, this.getHeight() - 5);
                } else if((f%10000)<(long)hzPerPixel) {
                    g.drawLine(i, 0, i, this.getHeight() - 20);
                }
            }
        }

        // plot the cursor
        g.setColor(Color.RED);
        g.drawLine((this.getWidth() / 2), 0, (this.getWidth() / 2), this.getHeight());

    }
    
}
