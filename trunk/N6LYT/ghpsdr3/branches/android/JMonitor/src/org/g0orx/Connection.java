package org.g0orx;

import java.io.InputStream;
import java.io.OutputStream;
import java.io.IOException;
import java.net.Socket;

import android.media.AudioFormat;
import android.media.AudioManager;
import android.media.AudioTrack;
import android.util.Log;

public class Connection extends Thread {

	public Connection(String server,int port) {
		//Log.i("Connection",server+":"+port);
		
		this.server=server;
		this.port=port;
		try {
		    socket=new Socket(server,port);
		    inputStream=socket.getInputStream();
		    outputStream=socket.getOutputStream();
		} catch (Exception e) {
			Log.e("Connection","Error creating socket for "+server+":"+port+"'"+e.getMessage()+"'");
		}
		//audioTrack=new AudioTrack(AudioManager.STREAM_MUSIC,8000,AudioFormat.CHANNEL_OUT_MONO,AudioFormat.ENCODING_PCM_16BIT,BUFFER_SIZE*2,AudioTrack.MODE_STREAM);
		audioTrack=new AudioTrack(AudioManager.STREAM_MUSIC,8000,AudioFormat.CHANNEL_CONFIGURATION_MONO,AudioFormat.ENCODING_PCM_16BIT,BUFFER_SIZE*2,AudioTrack.MODE_STREAM);
	}
	
	public void close() {
		running=false;
		if(socket!=null) {
			try {
				socket.close();
			} catch (Exception e) {
				
			}
		}
	}
	public void run() {
		int bytes;
		byte[] buffer=new byte[HEADER_SIZE+BUFFER_SIZE];
		if(socket!=null) {
		    while(running) {
			    try {
			    	bytes=inputStream.read(buffer);
			    	if(bytes<0) {
			    		Log.e("Connection.run","read returned "+bytes+" bytes");
			    		close();
			    		break;
			    	}
			    	
			    	if(!connected) {
			    		sendCommand("startAudioStream");
			    		connected=true;
			    	}
			    	
			    	if(bytes==buffer.length){
			    		switch(buffer[0]) {
			    		    case SPECTRUM_BUFFER:
			    		    	processSpectrumBuffer(buffer);
			    			    break;
			    		    case AUDIO_BUFFER:
			    		    	processAudioBuffer(buffer);
			    		    	break;
			    		}
			    	} else {
			    		System.err.println("Connection.run: socket read "+bytes+" bytes!");
			    	}
			    	
			    } catch (Exception e) {
			    	System.err.println("Connection.run: Exception reading socket: "+e.getMessage());
			    }
	   	    }
		}
	}
	
	private void processSpectrumBuffer(byte[] buffer) {
		int j;
		String s;
		
		j=0;
		while(buffer[j+32]!='\0') {
			j++;
		}
		s=new String(buffer,32,j);
		sampleRate=Integer.parseInt(s);
		
		j=0;
		while(buffer[j+40]!='\0') {
			j++;
		}
		s=new String(buffer,40,j);
		meter=Integer.parseInt(s);
		
		for(int i=0;i<BUFFER_SIZE;i++) {
			samples[i]=-(buffer[i+48]&0xFF)-30;
		}
		
		if(spectrumView!=null) {
			spectrumView.plotSpectrum(samples,filterLow,filterHigh,sampleRate);
		}
	}
	
	private void processAudioBuffer(byte[] buffer) {
		
		// decode 8 bit aLaw to 16 bit linear
		byte[] decodedBuffer=new byte[BUFFER_SIZE*2];
		int i;
        short v;
        for (int inIx=0, outIx=0; inIx < BUFFER_SIZE; inIx++) {
            i=buffer[inIx+HEADER_SIZE]&0xFF;
            v=aLawDecode[i];
            // assumes BIGENDIAN
            decodedBuffer[outIx++]=(byte)((v>>8)&0xFF);
            decodedBuffer[outIx++]=(byte)(v&0xFF);
        }
		
	}
	
	public synchronized void sendCommand(String command) {
		
		byte[] buffer=new byte[32];
		byte[] commandBytes=command.getBytes();
		for(int i=0;i<32;i++) {
			if(i<commandBytes.length) {
				buffer[i]=commandBytes[i];
			} else {
				buffer[i]=0;
			}
		}
		
		if(socket!=null) {
			try {
				outputStream.write(buffer);
				outputStream.flush();
			} catch (IOException e) {
				System.err.println("Connection.sendCommand: IOException: "+e.getMessage());
			}
		}
	}
	
	public void setFrequency(long frequency) {
        this.frequency=frequency;
        sendCommand("setFrequency "+frequency);
    }

	public long getFrequency() {
		return frequency;
	}
	
    public void setFilter(int filterLow,int filterHigh) {
        this.filterLow=filterLow;
        this.filterHigh=filterHigh;
        sendCommand("setFilter "+filterLow+" "+filterHigh);
        //System.err.println("setFilter "+filterLow+" "+filterHigh);
    }

    public void setMode(int mode) {
        this.mode=mode;
        sendCommand("setMode "+mode);
        //System.err.println("setMode " + mode);
    }
    
    public int getMode() {
    	return mode;
    }
    
    public String getStringMode() {
    	return modes[mode];
    }
    
    public void setAGC(int agc) {
        this.agc=agc;
        sendCommand("setRXAGC  "+agc);
    }

    public void setNR(boolean state) {
        sendCommand("setNR "+state);
    }
    
    public void setANF(boolean state) {
        sendCommand("setANF "+state);
    }
    
    public void setNB(boolean state) {
        sendCommand("setNB "+state);
    }
    
    public void setGain(int gain) {
        sendCommand("SetRXOutputGain "+gain);
    }
    
    public void getSpectrum() {
    	sendCommand("getSpectrum "+BUFFER_SIZE);
    }

    public int[] getSamples() {
    	return samples;
    }
    
    public int getMeter() {
    	return meter;
    }
    
    public int getSampleRate() {
    	return sampleRate;
    }
    
    public boolean isConnected() {
    	return connected;
    }
    
	public void setSpectrumView(SpectrumView spectrumView) {
		this.spectrumView=spectrumView;
	}
	
	private SpectrumView spectrumView;
	
	private static final int HEADER_SIZE=48;
	private static final int BUFFER_SIZE=480;
	
	private static final int SPECTRUM_BUFFER=0;
	private static final int AUDIO_BUFFER=1;
	
	private String server;
	private int port;
	private Socket socket;
	private InputStream inputStream;
	private OutputStream outputStream;
	private boolean running=true;
	private boolean connected=false;
	
	private long frequency;
    private int filterLow;
    private int filterHigh;
    private int mode;
    private int sampleRate;
    private int band;
    private int meter;
    private int agc;
    
    private int cwPitch=600;
    
    int[] samples=new int[BUFFER_SIZE];

    
	private AudioTrack audioTrack;
	
	public static final int modeLSB=0;
    public static final int modeUSB=1;
    public static final int modeDSB=2;
    public static final int modeCWL=3;
    public static final int modeCWU=4;
    public static final int modeFMN=5;
    public static final int modeAM=6;
    public static final int modeDIGU=7;
    public static final int modeSPEC=8;
    public static final int modeDIGL=9;
    public static final int modeSAM=10;
    public static final int modeDRM=11;

	private String[] modes={"LSB","USB","DSB","CWL","CWU","FMN","AM","DIGU","SPEC","DIGL","SAM","DRM"};
	
    private static short[] aLawDecode=new short[256];
	
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
            aLawDecode[i] = (short) value;
        }

    }
	
	
}
