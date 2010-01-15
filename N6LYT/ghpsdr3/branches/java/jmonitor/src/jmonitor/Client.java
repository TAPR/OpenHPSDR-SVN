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
            System.err.println("opened socket on port "+Integer.toString(port));
        } catch (UnknownHostException e) {
            System.err.println("Client: UnknownHost: "+server);
        } catch (IOException e) {
            System.err.println("Client: IOException: "+e.getMessage());
            e.printStackTrace();
        }
    }

    public void getSpectrum() {
        int bytes;
        byte[] buffer=new byte[48+SAMPLES];
        if(socket!=null) {
            try {
                outputStream.write("getSpectrum".getBytes());
                outputStream.flush();
                bytes=inputStream.read(buffer);
                //System.err.println("getSpectrum read "+Integer.toString(bytes)+" bytes");
                String frequency=new String(buffer,0,14);
                String filterLow=new String(buffer,14,6);
                String filterHigh=new String(buffer,20,6);
                String mode=new String(buffer,26,6);
                String sampleRate=new String(buffer,32,8);
                String band=new String(buffer,40,8);

                //System.err.println("frequency="+frequency);
                //System.err.println("filterLow="+filterLow);
                //System.err.println("filterHigh=" + filterHigh);
                //System.err.println("mode=" + mode);
                //System.err.println("sampleRate=" + sampleRate);
                //System.err.println("band=" + band);

                for(int i=0;i<SAMPLES;i++) {
                    samples[i]=-(buffer[i+48]&0xFF);
                }
            } catch (IOException e) {
            }
        }
    }

    public float[] getSamples() {
        return samples;
    }

    private static final int port=8000;
    private String server="81.146.61.118";
    private Socket socket;

    private InputStream inputStream;
    private OutputStream outputStream;

    private static final int SAMPLES=480;
    private float[] samples=new float[SAMPLES];
}
