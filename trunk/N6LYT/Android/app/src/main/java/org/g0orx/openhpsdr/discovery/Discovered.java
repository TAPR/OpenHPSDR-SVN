package org.g0orx.openhpsdr.discovery;

import java.io.Serializable;

public class Discovered implements Serializable {

	private static final long serialVersionUID = 1L;
	
	public Discovered() {
	}
	
	public void setDevice(int device) {
		this.device=device;
	}
	
	public void setSoftwareversion(int softwareversion) {
		this.softwareversion=softwareversion;
	}
	
	public void setAddress(String address) {
		this.address=address;
	}
	
	public void setMac(String mac) {
		this.mac=mac;
	}
	
	public void setState(int state) {
		this.state=state;
	}
	
	
	public int getDevice() {
		return device;
	}
	
	public String getDeviceName() {
		String result="Unknown";
		switch(device) {
		case DEVICE_METIS:		
			result="Metis";
			break;
		case DEVICE_HERMES:
			result="Hermes";
			break;
		case DEVICE_GRIFFIN:
			result="Griffin";
			break;
		case DEVICE_ANGELIA:
			result="Angelia";
			break;
        case DEVICE_ORION:
            result="Orion";
            break;
        case DEVICE_HERMES_LITE:
            result="Hermes Lite";
            break;
		}
		return result;
	}
	
	public int getSoftwareversion() {
		return softwareversion;
	}
	
	public String getAddress() {
		return address;
	}
	
	public String getMac() {
		return mac;
	}
	
	public int getState() {
		return state;
	}
	
	public String getStateName() {
		String result="";
		switch(state) {
		case 2:
    		result="notsending";
    		break;
    	case 3:
    		result="sending";
    	    break;
		}
		return result;
	}
	
	public String toString() {
		StringBuilder result=new StringBuilder();
		result.append(getDeviceName());
		result.append(" software version:");
		result.append(String.format("%02X", softwareversion));
		result.append(" address:");
		result.append(address);
		result.append(" (");
		result.append(mac);
		result.append(") state:");
		result.append(getStateName());
		return result.toString();
	}
	
	public static final int DEVICE_METIS=0;
	public static final int DEVICE_HERMES=1;
	public static final int DEVICE_GRIFFIN=2;
	public static final int DEVICE_ANGELIA=4;
    public static final int DEVICE_ORION=5;
    public static final int DEVICE_HERMES_LITE=6;
	
	public static final int STATE_AVAILABLE=2;
	public static final int STATE_SENDING=3;
	
	
	private int device;
	private int softwareversion;
	private String address;
	private String mac;
	private int state;
	
}
