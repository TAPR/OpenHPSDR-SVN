package org.hpsdr;

import java.io.InputStream;
import java.io.OutputStream;
import java.io.IOException;
import java.net.Socket;

import android.util.Log;

public class Connection extends Thread {
	public Connection(String server, int port, int width) {
		Log.i("Connection",server+":"+port);
		
		SPECTRUM_BUFFER_SIZE=width;
		samples = new int[SPECTRUM_BUFFER_SIZE];

		this.server = server;
		this.port = port;
		
		System.gc();
	}

	public void setServer(String server) {
		Log.i("Connection","setServer: "+server);
		this.server=server;
	}
	
	public void connect() {
		Log.i("Connection","connect: "+server+":"+port);
		try {
			socket = new Socket(server, port);
			inputStream = socket.getInputStream();
			outputStream = socket.getOutputStream();

			connected=true;

		    sendCommand("client aHPSDR(v1.0)");
	
			audio=new Audio(port,10900);
			audio.start();
		    
		    sendCommand("connect 10900 160 8000 1 G711a");
		    
		} catch (Exception e) {
			Log.e("Connection", "Error creating socket for " + server + ":"
					+ port + "'" + e.getMessage() + "'");
			status=e.toString();
		}
	}
	
	public void close() {
        Log.i("Connection","close");
        running=false;
        if(socket!=null) {
                try {
                        socket.close();
                } catch (Exception e) {

                }
                socket=null;
        }
        
        if(audio!=null) {
        	audio.close();
        }
        
    }

	public boolean isRunning() {
		return running;
	}
	
	public void run() {
		int bytes=0;
		int bytes_read=0;
		int length=0;
		byte[] spectrumHeader = new byte[SPECTRUM_HEADER_SIZE];
		byte[] spectrumBuffer = new byte[SPECTRUM_BUFFER_SIZE];
        Log.i("Connection","run");
		if (socket != null) {
			running=true;
			while (running) {
				try {

					bytes=0;
					while (bytes != SPECTRUM_HEADER_SIZE) {
						bytes_read = inputStream.read(spectrumHeader, bytes, SPECTRUM_HEADER_SIZE-bytes);
						if(bytes_read==-1) break;
						bytes+=bytes_read;
					}
					

					if(bytes_read==-1) {
						if(socket!=null) {
							socket.close();
							socket=null;
						}
						status="remote connection terminated";
						connected=false;
						break;
					}

					if(spectrumHeader[0]==SPECTRUM_BUFFER) {
						length=(int)getShort(spectrumHeader,3);
						if(length==SPECTRUM_BUFFER_SIZE) {
							bytes = 0;
							while (bytes != length) {
								bytes += inputStream.read(spectrumBuffer, bytes, length- bytes);
							}
						    processSpectrumBuffer(spectrumHeader, spectrumBuffer);
						} else {
							// discard as wrong length for current display
						    byte[] discard = new byte[length];
						    bytes = 0;
							while (bytes != length) {
								bytes += inputStream.read(discard, bytes, length - bytes);
							}
						}
					} else {
						// invalid
						Log.i("Connection","invalid length expected "+SPECTRUM_BUFFER_SIZE+" got "+length);
					}

				} catch (Exception e) {
					Log.e("Connection","run: Exception reading socket: "
									+ e.toString());
					e.printStackTrace();
					status=e.toString();
					running=false;
					connected=false;
				}
			}
			
			running=false;
		}
        Log.i("Connection","run: exit");
	}

	private short getShort(byte[] buffer,int offset) {
		short result;
		result=(short)(((buffer[offset]&0xFF)<<8)+(buffer[offset+1]&0xFF));
		return result;
	}
	
	private int getInt(byte[] buffer,int offset) {
		int result;
		result=((buffer[offset]&0xFF)<<24)|((buffer[offset+1]&0xFF)<<16)|((buffer[offset+2]&0xFF)<<8)|(buffer[offset+3]&0xFF);
		return result;
	}
	
	private void processSpectrumBuffer(byte[] header, byte[] buffer) {
		
		meter=getShort(header,5);
        sampleRate=getInt(header,9); 

        //Log.i("processSpectrumBuffer","sampeRate="+sampleRate+" meter="+meter+" buffer="+buffer.length);

        for(int i=0;i<SPECTRUM_BUFFER_SIZE;i++) {
        	samples[i] = -(buffer[i] & 0xFF);
        }

		if (spectrumView != null) {
			spectrumView.plotSpectrum(samples, filterLow, filterHigh,
					sampleRate);
		} else {
			Log.i("Connection","Error: spectrumView is null!");
		}
	}

	

	public synchronized void sendCommand(String command) {

		// Log.i("sendCommand",command);
		// append a newline
		command=command+"\n";
		if (socket != null) {
			try {
				outputStream.write(command.getBytes());
				outputStream.flush();
			} catch (IOException e) {
				Log.e("Connection","sendCommand: IOException: "
						+ e.getMessage());
				status=e.toString();
				connected=false;
			}
		}
	}

	public void setFrequency(long frequency) {
		this.frequency = frequency;
		sendCommand("frequency " + frequency);
	}

	public long getFrequency() {
		return frequency;
	}

	public void setFilter(int filterLow, int filterHigh) {
		this.filterLow = filterLow;
		this.filterHigh = filterHigh;
		sendCommand("filter " + filterLow + " " + filterHigh);
	}

	public int getFilterLow() {
		return filterLow;
	}
	
	public int getFilterHigh() {
		return filterHigh;
	}
	
	public void setMode(int mode) {
		this.mode = mode;
		sendCommand("mode " + mode);
	}

	public int getMode() {
		return mode;
	}

	public String getStringMode() {
		return modes[mode];
	}

	public void setAGC(int agc) {
		this.agc = agc;
		sendCommand("agc " + agc);
	}

	public void setNR(boolean state) {
		sendCommand("nr " + state);
	}

	public void setANF(boolean state) {
		sendCommand("anf " + state);
	}

	public void setNB(boolean state) {
		sendCommand("nb " + state);
	}

	public void setGain(int gain) {
		sendCommand("rxoutputgain " + gain);
	}

	public void getSpectrum() {
		//Log.i("Connection","getSpectrum "+SPECTRUM_BUFFER_SIZE);
		sendCommand("getspectrum " + SPECTRUM_BUFFER_SIZE);
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
		this.spectrumView = spectrumView;
	}

	public void setStatus(String message) {
		status = message;
	}

	public String getStatus() {
		return status;
	}
	
	private SpectrumView spectrumView;
	
	private Audio audio;

	private static final int BUFFER_TYPE_SIZE = 1;
	private static final int BUFFER_VERSION_SIZE = 2;
	private static final int AUDIO_HEADER_SIZE = 2;
	private static final int SPECTRUM_HEADER_SIZE = 13;
	private int SPECTRUM_BUFFER_SIZE = 480;
	

	private static final int SPECTRUM_BUFFER = 0;
	private static final int AUDIO_BUFFER = 1;

	private String server;
	private int port;
	private Socket socket;
	private InputStream inputStream;
	private OutputStream outputStream;
	private boolean running = false;
	private boolean connected = false;

	
	private long frequency;
	private int filterLow;
	private int filterHigh;
	private int mode;
	private int sampleRate;
	private int band;
	private short meter;
	private int agc;

	private int cwPitch = 600;


	private int[] samples;

	
	private String status = "";

	public static final int modeLSB = 0;
	public static final int modeUSB = 1;
	public static final int modeDSB = 2;
	public static final int modeCWL = 3;
	public static final int modeCWU = 4;
	public static final int modeFMN = 5;
	public static final int modeAM = 6;
	public static final int modeDIGU = 7;
	public static final int modeSPEC = 8;
	public static final int modeDIGL = 9;
	public static final int modeSAM = 10;
	public static final int modeDRM = 11;

	private static final String[] modes = { "LSB", "USB", "DSB", "CWL", "CWU", "FMN", "AM",
			"DIGU", "SPEC", "DIGL", "SAM", "DRM" };

	

}
