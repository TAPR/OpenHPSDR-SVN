/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package jmonitor;

/**
 *
 * @author john
 */
public class MonitorUpdateThread extends Thread {

    MonitorUpdateThread(Client client,MonitorFrame monitorFrame) {
        this.client=client;
        this.monitorFrame=monitorFrame;
    }

    public void run() {
        System.err.println("MonitorUpdateThread.run");
        while(true) {
            client.getSpectrum();
            monitorFrame.updateMonitor(client.getSamples(),client.getFilterLow(),client.getFilterHigh(),client.getSampleRate());
            monitorFrame.setTitle("JMonitor: "+client.getFrequency()+" "+client.getMode());
            try {
                sleep(1000/fps);
            } catch (InterruptedException e) {
                System.err.println("MonitorUpdateThread: InterruptedException: "+e.getMessage());
            }
        }
    }

    private Client client;
    private MonitorFrame monitorFrame;
    private int fps=10;

}
