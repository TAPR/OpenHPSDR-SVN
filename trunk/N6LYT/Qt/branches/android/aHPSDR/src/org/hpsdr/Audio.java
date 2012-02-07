package org.hpsdr;

import java.net.DatagramPacket;
import java.net.DatagramSocket;

import android.media.AudioFormat;
import android.media.AudioManager;
import android.media.AudioTrack;
import android.util.Log;

public class Audio extends Thread {

	Audio(int remotePort,int localPort) {
		Log.i("Audio","remotePort="+remotePort+" localPort="+localPort);
		try {
		    socket=new DatagramSocket(localPort);
		    
			audioTrack = new AudioTrack(AudioManager.STREAM_MUSIC, 8000,
					AudioFormat.CHANNEL_CONFIGURATION_MONO,
					AudioFormat.ENCODING_PCM_16BIT, 1600,
					AudioTrack.MODE_STREAM);
			
			Log.i("Audio","state:"+audioTrack.getState());
			
			audioTrack.play();
			
			Log.i("Audio","state:"+audioTrack.getState());
			
		} catch (Exception e) {
			Log.i("Audio","Error:"+e.toString());
		}
	}
	
	public void run() {
		if(socket!=null) {
			Log.i("Audio","run");
			byte[] buffer=new byte[AUDIO_BUFFER_SIZE];
			DatagramPacket packet=new DatagramPacket(buffer,buffer.length);
			while(running) {
				try {
					socket.receive(packet);
					processAudioBuffer(packet.getData());
				} catch(Exception e) {
					Log.i("Audion.run","Error:"+e.toString());
				}
			}
			socket.close();
		    socket=null;
		}
	}
	
	public void close() {
		Log.i("Audio","close");
		running=false;
		if(audioTrack!=null) {
			audioTrack.stop();
		}
	}
	
	private void processAudioBuffer(byte[] buffer) {

		short[] decodedBuffer=new short[buffer.length];
		
		// decode 8 bit aLaw to 16 bit linear
		for (int i = 0; i < buffer.length; i++) {
			decodedBuffer[i] = aLawDecode[buffer[i] & 0xFF];
		}
		
		// drop frame if more than 0.25 seconds behind
		//String txt=String.valueOf(frames-audioTrack.getPlaybackHeadPosition());
		//Log.i("processAudioBuffer",txt);
		if((frames-audioTrack.getPlaybackHeadPosition())<2000) {
		    audioTrack.write(decodedBuffer, 0, decodedBuffer.length);
		    frames+=buffer.length;
		} else {
			String err=String.valueOf(audioTrack.getPlaybackHeadPosition())+":"+String.valueOf(frames)+":"+String.valueOf(buffer.length);
			Log.i("processAudioBuffer: dropped buffer",err);
		}
	}
	
	private DatagramSocket socket;
	private boolean running=true;
	
	static final int AUDIO_BUFFER_SIZE = 160;
	private AudioTrack audioTrack;
	
	private int frames=0;
	
	private static short[] aLawDecode = new short[256];

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
