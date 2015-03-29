package org.g0orx.openhpsdr;

import java.awt.Graphics;
import java.awt.Color;

import org.g0orx.openhpsdr.modes.Modes;

public class VFOPanel extends javax.swing.JPanel {
    
    public void paintComponent(Graphics g) {
        Configuration configuration=Configuration.getInstance();
        Band band=configuration.bands.get();
        BandStack bandstack=band.get();
        
        Filter filter = Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());

        g.setColor(Color.BLACK);
        g.fillRect(0, 0, this.getWidth(), this.getHeight());

        long frequency = bandstack.getFrequency();
        /*
        if (metis!=null && metis.isTransmitting()) {
            paint.setColor(Color.RED);
        } else {
            paint.setColor(Color.GREEN);
        }
        */
        g.setColor(Color.GREEN);
        g.drawString(Frequency.toString(frequency) + " " + Modes.getMode(bandstack.getMode()).getName() + " " + filter.getName()+"Hz", 10, this.getHeight()-10);
        
    }
    
}
