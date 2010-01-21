/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package jmonitor;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.net.UnknownHostException;

/**
 *
 * @author john
 */
public class Client {

    public Client(String server) {
        if(server!=null) {
            this.server=server;
        }
        try {
            socket=new Socket(this.server,port);
            inputStream=socket.getInputStream();
            outputStream=socket.getOutputStream();

            System.err.println("opened client socket on port "+Integer.toString(port));
        } catch (UnknownHostException e) {
            System.err.println("Client: UnknownHost: "+server);
            System.exit(1);
        } catch (IOException e) {
            System.err.println("Client: IOException: "+e.getMessage());
            System.exit(1);
        }
    }

    public synchronized void sendCommand(String command) {
        byte[] buffer=new byte[32];
        byte[] commandBytes=command.getBytes();

        //System.err.println(command);
        
        for(int i=0;i<32;i++) {
            if(i<commandBytes.length) {
                buffer[i]=commandBytes[i];
            } else {
                buffer[i]=0;
            }
        }

        try {
            outputStream.write(buffer);
            outputStream.flush();
        } catch (IOException e) {
            System.err.println("sendCommand: IOException: "+e.getMessage());
            System.exit(1);
        }
    }

    public void getSpectrum() {
        int bytes;
        byte[] buffer=new byte[48+SAMPLES];
        int j;
        if(socket!=null) {
            try {
                sendCommand("getSpectrum");
                bytes=inputStream.read(buffer);
                if(bytes==buffer.length) {
                    //System.err.println("getSpectrum: read "+Integer.toString(bytes)+" bytes");
                    // Strings sent with NULL terminator
                    j = 0;
                    while (buffer[j] != '\0')j++;
                    frequency=new String(buffer,0,j);
                    j=0;
                    while(buffer[j+14]!='\0') j++;
                    filterLow=new String(buffer,14,j);
                    j=0;
                    while(buffer[j+20]!='\0') j++;
                    filterHigh=new String(buffer,20,j);
                    j = 0;
                    while (buffer[j+26] != '\0') j++;
                    mode=new String(buffer,26,j);
                    j=0;
                    while(buffer[j+32]!='\0') j++;
                    sampleRate=new String(buffer,32,j);
                    j=0;
                    while(buffer[j+40]!='\0') j++;
                    band=new String(buffer,40,j);

                    for(int i=0;i<SAMPLES;i++) {
                        samples[i]=-(buffer[i+48]&0xFF);
                    }
                } else {
                    System.err.println("read spectrum read "+Integer.toString(bytes)+" bytes");
                }
            } catch (IOException e) {
            }
        }
    }

    public float[] getSamples() {
        return samples;
    }

    public String getFrequency() {
        return frequency;
    }

    public String getMode() {
        return mode;
    }

    public int getFilterLow() {
        return Integer.parseInt(filterLow);
    }

    public int getFilterHigh() {
        return Integer.parseInt(filterHigh);
    }

    public int getSampleRate() {
        return Integer.parseInt(sampleRate);
    }

    private static final int port=8000;
    private String server="81.146.61.118";
    private Socket socket;

    private InputStream inputStream;
    private OutputStream outputStream;

    private static final int SAMPLES=480;
    private float[] samples=new float[SAMPLES];

    String frequency;
    String filterLow;
    String filterHigh;
    String mode;
    String sampleRate;
    String band;
}
