package org.g0orx.openhpsdr;

import java.io.Serializable;


public class Band implements Serializable {
	
	private static final long serialVersionUID = 1L;
	
	public Band() {
	}
	
	public void setName(String name) {
		this.name=name;
	}
	
	public void setBandStack(int entry,BandStack bandstack) throws IllegalArgumentException {
		if(entry<0 || entry>=STACK_SIZE) {
			throw new IllegalArgumentException();
		}
		this.bandstack[entry]=bandstack;
	}
	
	public void setBandEdge(BandEdge bandedge) {
		this.bandedge=bandedge;
	}
	
	public void setTxGain(float gain) {
		txgain=gain;
	}
	
	public void setSquelch(boolean state) {
		squelch=state;
	}
	
	public void setSquelchValue(int level) {
		squelchvalue=level;
	}
	
	public String getName() {
		return name;
	}
	
	public BandStack get() {
		return bandstack[bandstackentry];
	}
	
	public float getTxGain() {
		return txgain;
	}

    public byte getOCRx() { return OCRx; }
    public void setOCRx(byte b) { OCRx=b; }
    public byte getOCTx() { return OCTx; }
    public void setOCTx(byte b) { OCTx=b; }
	
	public BandStack next() {
		bandstackentry++;
		if(bandstackentry>=STACK_SIZE) {
			bandstackentry=0;
		}
		return bandstack[bandstackentry];
	}
	
	public BandStack previous() {
		bandstackentry--;
		if(bandstackentry<0) {
			bandstackentry=STACK_SIZE-1;
		}
		return bandstack[bandstackentry];
	}
	
	public BandEdge getBandEdge() {
		return bandedge;
	}
	
	public boolean getSquelch() {
		return squelch;
	}
	
	public int getSquelchValue() {
		return squelchvalue;
	}
	
	public static final int STACK_SIZE=1;

	private String name;
	private int bandstackentry=0;
	private BandStack[] bandstack=new BandStack[STACK_SIZE];
	private BandEdge bandedge;
	
	private boolean squelch=false;
	private int squelchvalue=100;
	
	private float txgain=0.30F;

    private byte OCRx=0x00;
    private byte OCTx=0x00;


}
