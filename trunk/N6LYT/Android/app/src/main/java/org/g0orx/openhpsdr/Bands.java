package org.g0orx.openhpsdr;

import android.util.Log;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.Iterator;

import org.g0orx.openhpsdr.modes.Modes;

public class Bands implements Serializable {
	
	private static final long serialVersionUID = 2L;

	public Bands() {
		init();
	}
	
	public Band[] getBands() {
		return bands.toArray(new Band[0]);
	}
	
	public Band get() {
        return bands.get(current);
	}

    public int get(Long frequency) {
        int result=-1;
        Iterator<Band> it=bands.iterator();
        while(it.hasNext()) {
            result++;
            Band band=it.next();
            BandEdge bandedge=band.getBandEdge();
            if(frequency>=bandedge.getLow() && frequency<=bandedge.getHigh()) {
                return result;
            }
        }
        return -1;
    }

	public int get(String name) {
        int result=-1;
		Iterator<Band> it=bands.iterator();
		while(it.hasNext()) {
            result++;
			Band band=it.next();
			if(band.getName().equals(name)) {
                return result;
			}
		}
        return -1;
	}
	
	public void set(int band) {
        current=band;
    }

	public int getCurrent() {
		return current;
	}
	
	public Band next() {
		current++;
		if(current>=bands.size()) {
			current=0;
		}
		return bands.get(current);
	}
	
	public Band previous() {
		current--;
		if(current<0) {
			current=bands.size()-1;
		}
		return bands.get(current);
	}
	
	public void add(Band band) {
		bands.add(band);
	}

	public void delete(Band band) {
		bands.remove(band);
	}
	
	private void init() {
		
		// initialize the bands
		
		// setup defaults
		BandEdge bandedge=new BandEdge();
		bandedge.setLow(135700);
		bandedge.setHigh(137800);
		Band band=new Band(3);
		band.setName("2200");
		band.setBandEdge(bandedge);
		band.setBandStack(0, makeBandStack(135800,Modes.CWL,7));
		band.setBandStack(1, makeBandStack(136500,Modes.CWL,7));
		band.setBandStack(2, makeBandStack(137600,Modes.CWL,7));
		bands.add(band);

        bandedge=new BandEdge();
        bandedge.setLow(472000);
        bandedge.setHigh(479000);
        band=new Band(3);
        band.setName("630");
        band.setBandEdge(bandedge);
        band.setBandStack(0, makeBandStack(473000,Modes.CWL,7));
        band.setBandStack(1, makeBandStack(475000,Modes.CWL,7));
        band.setBandStack(2, makeBandStack(478000,Modes.LSB,3));
        bands.add(band);

        bandedge=new BandEdge();
        bandedge.setLow(1800000);
        bandedge.setHigh(2000000);
        band=new Band(3);
        band.setName("160");
        band.setBandEdge(bandedge);
        band.setBandStack(0, makeBandStack(1801000,Modes.CWL,7));
        band.setBandStack(1, makeBandStack(1835000,Modes.LSB,3));
        band.setBandStack(2, makeBandStack(1840000,Modes.LSB,3));
        bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(3500000);
		bandedge.setHigh(4000000);
		band=new Band(3);
		band.setName("80");
		band.setBandEdge(bandedge);
		band.setBandStack(0, makeBandStack(3501000,Modes.CWL,7));
		band.setBandStack(1, makeBandStack(3751000,Modes.LSB,3));
		band.setBandStack(2, makeBandStack(3850000,Modes.LSB,3));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(5330500);
		bandedge.setHigh(5403500);
		band=new Band(3);
		band.setName("60");
		band.setBandEdge(bandedge);
		band.setBandStack(0, makeBandStack(5330500,Modes.CWL,7));
		band.setBandStack(1, makeBandStack(5346500,Modes.CWL,7));
		band.setBandStack(2, makeBandStack(5366500,Modes.CWL,7));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(7000000);
		bandedge.setHigh(7300000);
		band=new Band(3);
		band.setName("40");
		band.setBandEdge(bandedge);
		band.setBandStack(0, makeBandStack(7001000,Modes.CWL,7));
		band.setBandStack(1, makeBandStack(7100000,Modes.LSB,3));
		band.setBandStack(2, makeBandStack(7200000,Modes.LSB,3));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(10100000);
		bandedge.setHigh(10150000);
		band=new Band(3);
		band.setName("30");
		band.setBandEdge(bandedge);
		band.setBandStack(0, makeBandStack(10120000,Modes.CWU,7));
		band.setBandStack(1, makeBandStack(10130000,Modes.USB,3));
		band.setBandStack(2, makeBandStack(10140000,Modes.USB,3));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(14000000);
		bandedge.setHigh(14350000);
		band=new Band(3);
		band.setName("20");
		band.setBandEdge(bandedge);
		band.setBandStack(0, makeBandStack(14010000,Modes.CWU,7));
		band.setBandStack(1, makeBandStack(14230000,Modes.USB,3));
		band.setBandStack(2, makeBandStack(14330000,Modes.USB,3));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(18068000);
		bandedge.setHigh(18168000);
		band=new Band(3);
		band.setName("17");
		band.setBandEdge(bandedge);
		band.setBandStack(0, makeBandStack(18068600,Modes.CWU,7));
		band.setBandStack(1, makeBandStack(18125000,Modes.USB,3));
		band.setBandStack(2, makeBandStack(18140000,Modes.USB,3));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(21000000);
		bandedge.setHigh(21450000);
		band=new Band(3);
		band.setName("15");
		band.setBandEdge(bandedge);
		band.setBandStack(0, makeBandStack(21001000,Modes.CWU,7));
		band.setBandStack(1, makeBandStack(21255000,Modes.USB,3));
		band.setBandStack(2, makeBandStack(21300000,Modes.USB,3));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(24890000);
		bandedge.setHigh(24990000);
		band=new Band(3);
		band.setName("12");
		band.setBandEdge(bandedge);
		band.setBandStack(0, makeBandStack(24895000,Modes.CWU,7));
		band.setBandStack(1, makeBandStack(24900000,Modes.USB,3));
		band.setBandStack(2, makeBandStack(24910000,Modes.USB,3));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(28000000);
		bandedge.setHigh(29700000);
		band=new Band(3);
		band.setName("10");
		band.setBandEdge(bandedge);
		band.setBandStack(0, makeBandStack(28001000,Modes.CWU,7));
		band.setBandStack(1, makeBandStack(28300000,Modes.USB,3));
		band.setBandStack(2, makeBandStack(28500000,Modes.USB,3));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(50000000);
		bandedge.setHigh(54000000);
		band=new Band(3);
		band.setName("6");
		band.setBandEdge(bandedge);
		band.setBandStack(0, makeBandStack(50001000,Modes.CWU,7));
		band.setBandStack(1, makeBandStack(50125000,Modes.USB,3));
		band.setBandStack(2, makeBandStack(50200000,Modes.USB,3));
		bands.add(band);
		

		
		bandedge=new BandEdge();
		bandedge.setLow(526000);
		bandedge.setHigh(1705000);
		band=new Band(3);
		band.setName("MW");
		band.setBandEdge(bandedge);
		band.setBandStack(0, makeBandStack(897000,Modes.AM,3));
		band.setBandStack(1, makeBandStack(909000,Modes.AM,3));
		band.setBandStack(2, makeBandStack(1215000,Modes.AM,3));
		bands.add(band);

        bandedge=new BandEdge();
        bandedge.setLow(00000000);
        bandedge.setHigh(61440000);
        band=new Band(3);
        band.setName("Gen");
        band.setBandEdge(bandedge);
        band.setBandStack(0, makeBandStack(5975000,Modes.AM,6));
        band.setBandStack(1, makeBandStack(11710000,Modes.AM,6));
        band.setBandStack(2, makeBandStack(13845000,Modes.AM,6));
        bands.add(band);

        bandedge=new BandEdge();
        bandedge.setLow(0);
        bandedge.setHigh(0);
        band=new Band(5);
        band.setName("WWV");
        band.setBandEdge(bandedge);
        band.setBandStack(0, makeBandStack(2500000,Modes.SAM,6));
        band.setBandStack(1, makeBandStack(5000000,Modes.SAM,6));
        band.setBandStack(2, makeBandStack(10000000,Modes.SAM,6));
        band.setBandStack(3, makeBandStack(15000000,Modes.SAM,6));
        band.setBandStack(4, makeBandStack(20000000,Modes.SAM,6));
        bands.add(band);

        // default to 20 Mtrs
        current=get("20");
        if(current==-1) {
            current=0;
        }
	}
	
	private BandStack makeBandStack(long frequency,int mode,int filter) {
		BandStack bandstack=new BandStack();
		bandstack.setFrequency(frequency);
		bandstack.setMode(mode);
		bandstack.setFilter(filter);
		return bandstack;
	}
	
	private ArrayList<Band> bands=new ArrayList<Band>();
	
	private int current=0;
	

}
