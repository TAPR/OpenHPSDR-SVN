/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package jmonitor;

import java.io.IOException;
import java.io.InputStream;
import java.net.Socket;
import java.net.UnknownHostException;
import javax.sound.sampled.AudioFormat;
import javax.sound.sampled.AudioSystem;
import javax.sound.sampled.DataLine;
import javax.sound.sampled.SourceDataLine;

/**
 *
 * @author john
 */
public class Audio extends Thread {

    public Audio(String server) {

        if (server != null) {
            this.server = server;
        }
        try {
            socket = new Socket(this.server, port);
            inputStream = socket.getInputStream();

            System.err.println("opened audio socket on port " + Integer.toString(port));
        } catch (UnknownHostException e) {
            System.err.println("Client: UnknownHost: " + server);
            System.exit(1);
        } catch (IOException e) {
            System.err.println("Client: IOException: " + e.getMessage());
            System.exit(1);
        }

        af = new AudioFormat((float) sampleRate, 8, 1, true, true);
        info = new DataLine.Info(SourceDataLine.class, af);
        try {
            source=(SourceDataLine) AudioSystem.getLine(info);
            source.open(af);
            source.start();
        } catch (Exception e) {
            System.err.println("Audio: "+e.toString());
        }
    }

    public void run() {
        int bytes;

        byte[] buffer=new byte[1024];

        while(true) {
            try {
                bytes = inputStream.read(buffer);
                source.write(buffer, 0, bytes);
            } catch (IOException e) {
                System.err.println("Audio.run: IOException: "+e.getMessage());
            }
        }

    }

    private String server = "81.146.61.118";
    private static final int port=8002;
    private Socket socket;
    private InputStream inputStream;


    private static final int sampleRate=8000;

    private AudioFormat af;
    private DataLine.Info info;
    private SourceDataLine source;

}
