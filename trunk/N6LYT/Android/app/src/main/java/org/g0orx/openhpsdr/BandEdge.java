package org.g0orx.openhpsdr;

import java.io.Serializable;

public class BandEdge implements Serializable {
	
	private static final long serialVersionUID = 1L;
	
	public BandEdge() {
		
	}
	
	public void setLow(long low) {
		this.low=low;
	}
	
	public void setHigh(long high) {
		this.high=high;
	}
	
	public long getLow() {
		return low;
	}
	
	public long getHigh() {
		return high;
	}
	
	private long low;
	private long high;

}
