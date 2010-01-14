/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package jmonitor;

/**
 *
 * @author john
 */
public class Main {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        // TODO code application logic here
        Client client=new Client(null);
        MonitorFrame frame=new MonitorFrame(); 
        MonitorUpdateThread monitorUpdateThread=new MonitorUpdateThread(client,frame);
        monitorUpdateThread.start();
        frame.setVisible(true);
    }

}
