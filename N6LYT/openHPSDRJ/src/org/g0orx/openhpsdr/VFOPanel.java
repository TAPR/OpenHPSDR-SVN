package org.g0orx.openhpsdr;

import java.awt.Graphics;
import java.awt.Color;
import java.awt.Font;

import org.g0orx.openhpsdr.modes.Modes;

public class VFOPanel extends javax.swing.JPanel {
    
    public void setMetis(Metis metis) {
        this.metis=metis;
    }
    
    public void paintComponent(Graphics g) {
        
        Font font=g.getFont().deriveFont(28f);
        g.setFont(font);
        
        Configuration configuration=Configuration.getInstance();
        Band band=configuration.bands.get();
        BandStack bandstack=band.get();
        
        Filter filter = Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());

        g.setColor(Color.BLACK);
        g.fillRect(0, 0, this.getWidth(), this.getHeight());

        long frequency = bandstack.getFrequency();

        g.setColor(Color.GREEN);        
        if (metis!=null && metis.isTransmitting()) {
            g.setColor(Color.RED);
        }

        g.drawString(Frequency.toString(frequency) + " " + Modes.getMode(bandstack.getMode()).getName() + " " + filter.getName()+"Hz", 10, this.getHeight()-10);
        
    }
    
    private Metis metis;
    
}
