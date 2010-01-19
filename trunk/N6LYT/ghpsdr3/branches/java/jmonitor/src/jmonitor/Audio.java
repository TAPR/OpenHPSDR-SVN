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

        af = new AudioFormat((float) sampleRate, 16, 1, true, true);
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

        byte[] buffer=new byte[AUDIO_BUFFER_SIZE];
        byte[] decodedBuffer=new byte[AUDIO_BUFFER_SIZE*2];

        while(true) {
            try {
                bytes=0;
                while(bytes<AUDIO_BUFFER_SIZE) {
                    bytes+=inputStream.read(buffer,bytes,AUDIO_BUFFER_SIZE-bytes);
                    aLawDecode(buffer,decodedBuffer);
                    source.write(decodedBuffer,0,bytes);
                }
            } catch (IOException e) {
                System.err.println("Audio.run: IOException: "+e.getMessage());
            }
        }

    }

    private void aLawDecode(byte[] buffer,byte[] decodedBuffer) {
        int i;
        short v;
        for (int inIx=0, outIx=0; inIx < buffer.length; inIx++) {
            i=buffer[inIx]&0xFF;
            v=decodetable[i];
            // assumes BIGENDIAN
            decodedBuffer[outIx++]=(byte)((v>>8)&0xFF);
            decodedBuffer[outIx++]=(byte)(v&0xFF);
        }
    }

    private String server = "81.146.61.118";
    private static final int port=8002;
    private Socket socket;
    private InputStream inputStream;

    private static final int AUDIO_BUFFER_SIZE=256;

    private static final int sampleRate=8000;

    private AudioFormat af;
    private DataLine.Info info;
    private SourceDataLine source;

    private static short[] decodetable=new short[256];

    static {

        for (int i = 0; i < 256; i++) {
            int input = i ^ 85;
            int mantissa = (input & 15) << 4;
            int segment = (input & 112) >> 4;
            int value = mantissa + 8;
            if (segment >= 1) {
                value += 256;
            }
            if (segment > 1) {
                value <<= (segment - 1);
            }
            if ((input & 128) == 0) {
                value = -value;
            }
            decodetable[i] = (short) value;
        }

    }

}
