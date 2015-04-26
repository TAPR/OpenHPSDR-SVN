package org.g0orx.openhpsdr;

import java.awt.Graphics;
import java.awt.Color;
import java.awt.Font;

public class VFOPanel extends javax.swing.JPanel {
    
    public VFOPanel() {
        
    }
    
    public VFOPanel(Radio radio,Receiver receiver) {
        super();
        this.radio=radio;
        this.metis=radio.getMetis();
        this.receiver=receiver;
    }
    
    public void paintComponent(Graphics g) {
        
        g.setColor(Color.BLACK);
        g.fillRect(0, 0, this.getWidth(), this.getHeight());
        if(radio==null) {
            return;
        }
        
        Font font=g.getFont().deriveFont(28f);
        g.setFont(font);
        
        
        Band band=receiver.getBands().get();
        BandStack bandstack=band.get();
        
        Filter filter = Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());

        long frequency = bandstack.getFrequency();

        if(metis!=null && metis.isActiveReceiver(receiver.getId())) {
            g.setColor(Color.GREEN);        
        } else {
            g.setColor(Color.GRAY);
        }
        if (metis!=null && metis.isTransmitting()) {
            g.setColor(Color.RED);
        }

        g.drawString(Frequency.toString(frequency) + " " + Modes.getMode(bandstack.getMode()).getName() + " " + filter.getName()+"Hz", 10, this.getHeight()-10);
        
    }
    
    private Radio radio;
    private Receiver receiver;
    private Metis metis;
    
}
