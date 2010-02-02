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

        String server="81.146.61.118";
        int receiver=0;
        int i=0;
        while(i<args.length) {

            if(args[i].equalsIgnoreCase("--receiver")) {
                i++;
                if(i<args.length) {
                    try {
                        receiver=Integer.parseInt(args[i]);
                    } catch (NumberFormatException e) {
                        usage("Invalid receiver argument");
                    }
                } else {
                    usage("Missing receiver argument");
                }
            } else if(args[i].equalsIgnoreCase("--server")) {
                i++;
                if(i<args.length) {
                    server=args[i];
                } else {
                    usage("Missing server argument");
                }
            }

            i++;
        }

        Client client=new Client(server,receiver);
        client.setFrequency(7048000);
        client.setMode(0);
        client.setFilter(-2850,-150);
        
        MonitorFrame frame=new MonitorFrame(client);
        MonitorUpdateThread monitorUpdateThread=new MonitorUpdateThread(client,frame);
        
        frame.setVisible(true);

        monitorUpdateThread.start();
        Audio audio=new Audio(server,receiver);
        audio.start();
    }

    private static void usage(String error) {
        System.err.print("Error: "+error);
        System.err.print("Usage: java -jar jmonitor.jar [--receiver RX]");
        System.exit(1);
    }
}
