/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

/*
 * MonitorApplet.java
 *
 * Created on 01-Feb-2010, 11:05:21
 */

package jmonitor;

import javax.swing.JFrame;

/**
 *
 * @author john
 */
public class MonitorApplet extends javax.swing.JApplet {

    /** Initializes the applet MonitorApplet */
    public void init() {
        try {
            java.awt.EventQueue.invokeAndWait(new Runnable() {
                public void run() {
                    initComponents();
                }
            });
        } catch (Exception ex) {
            ex.printStackTrace();
        }
    }

    /** This method is called from within the init() method to
     * initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is
     * always regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        monitorPanel = new jmonitor.MonitorPanel();

        getContentPane().setLayout(null);
        getContentPane().add(monitorPanel);
        monitorPanel.setBounds(0, 0, 482, 230);
    }// </editor-fold>//GEN-END:initComponents


    public void start() {

        // get the host we came from
        String s;

        server=getCodeBase().getHost();

        s=this.getParameter("server");
        if(s!=null) {
            server=s;
        }
        System.err.println("server: "+server);

        s=this.getParameter("receiver");
        if(s!=null) {
            receiver=Integer.parseInt(s);
        }

        audio = new Audio(server, receiver);
        
        client = new Client(server, receiver,audio);
        client.start();
        client.setFrequency(7048000);
        client.setMode(0);
        client.setFilter(-2850, -150);
        client.setAGC(1);
        client.setGain(30);

        monitorPanel.setClient(client);
        add(monitorPanel);

        MonitorUpdateThread monitorUpdateThread = new MonitorUpdateThread(client, monitorPanel);
        monitorUpdateThread.start();

    }

    public void stop() {

    }

    public void destroy() {
        audio.close();
        client.close();
    }

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private jmonitor.MonitorPanel monitorPanel;
    // End of variables declaration//GEN-END:variables

    String server = "81.146.61.118";
    int receiver = 0;

    Client client;
    Audio audio;
}
