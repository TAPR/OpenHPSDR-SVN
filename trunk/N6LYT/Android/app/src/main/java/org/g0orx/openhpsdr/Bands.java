package org.g0orx.openhpsdr;

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
		//Band result=null;
        int result=-1;
		Iterator<Band> it=bands.iterator();
		while(it.hasNext()) {
            result++;
			Band band=it.next();
			if(band.getName().equals(name)) {
				//result=band;
				//break;
                return result;
			}
		}
		//return result;
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
		Band band=new Band();
		band.setName("2200");
		band.setBandEdge(bandedge);
		//band.setBandStack(0, makeBandStack(1801000,DttSP.CWL,-500,-100));
		band.setBandStack(0, makeBandStack(136500,Modes.CWL,-500,-100));
		//band.setBandStack(2, makeBandStack(1840000,DttSP.LSB,-2700,-300));
		bands.add(band);

        bandedge=new BandEdge();
        bandedge.setLow(472000);
        bandedge.setHigh(479000);
        band=new Band();
        band.setName("630");
        band.setBandEdge(bandedge);
        //band.setBandStack(0, makeBandStack(1801000,DttSP.CWL,-500,-100));
        band.setBandStack(0, makeBandStack(475000,Modes.CWL,-500,-100));
        //band.setBandStack(2, makeBandStack(1840000,DttSP.LSB,-2700,-300));
        bands.add(band);

        bandedge=new BandEdge();
        bandedge.setLow(1800000);
        bandedge.setHigh(2000000);
        band=new Band();
        band.setName("160");
        band.setBandEdge(bandedge);
        //band.setBandStack(0, makeBandStack(1801000,DttSP.CWL,-500,-100));
        band.setBandStack(0, makeBandStack(1835000,Modes.LSB,-2700,-300));
        //band.setBandStack(2, makeBandStack(1840000,DttSP.LSB,-2700,-300));
        bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(3500000);
		bandedge.setHigh(4000000);
		band=new Band();
		band.setName("80");
		band.setBandEdge(bandedge);
		//band.setBandStack(0, makeBandStack(3501000,DttSP.CWL,-500,-100));
		band.setBandStack(0, makeBandStack(3751000,Modes.LSB,-2700,-300));
		//band.setBandStack(2, makeBandStack(3850000,DttSP.LSB,-2700,-300));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(5330500);
		bandedge.setHigh(5403500);
		band=new Band();
		band.setName("60");
		band.setBandEdge(bandedge);
		//band.setBandStack(0, makeBandStack(5330500,DttSP.CWL,-500,-100));
		band.setBandStack(0, makeBandStack(5346500,Modes.CWL,-500,-100));
		//band.setBandStack(2, makeBandStack(5366500,DttSP.CWL,-500,-100));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(7000000);
		bandedge.setHigh(7300000);
		band=new Band();
		band.setName("40");
		band.setBandEdge(bandedge);
		//band.setBandStack(0, makeBandStack(7001000,DttSP.CWL,-500,-100));
		band.setBandStack(0, makeBandStack(7100000,Modes.LSB,-2700,-300));
		//band.setBandStack(2, makeBandStack(7200000,DttSP.LSB,-2700,-300));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(10100000);
		bandedge.setHigh(10150000);
		band=new Band();
		band.setName("30");
		band.setBandEdge(bandedge);
		//band.setBandStack(0, makeBandStack(10120000,DttSP.CWU,100,500));
		band.setBandStack(0, makeBandStack(10130000,Modes.USB,300,2700));
		//band.setBandStack(2, makeBandStack(10140000,DttSP.USB,300,2700));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(14000000);
		bandedge.setHigh(14350000);
		band=new Band();
		band.setName("20");
		band.setBandEdge(bandedge);
		//band.setBandStack(0, makeBandStack(14010000,DttSP.CWU,100,500));
		band.setBandStack(0, makeBandStack(14230000,Modes.USB,300,2700));
		//band.setBandStack(2, makeBandStack(14330000,DttSP.USB,300,2700));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(18068000);
		bandedge.setHigh(18168000);
		band=new Band();
		band.setName("17");
		band.setBandEdge(bandedge);
		//band.setBandStack(0, makeBandStack(18068600,DttSP.CWU,100,500));
		band.setBandStack(0, makeBandStack(18125000,Modes.USB,300,2700));
		//band.setBandStack(2, makeBandStack(18140000,DttSP.USB,300,2700));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(21000000);
		bandedge.setHigh(21450000);
		band=new Band();
		band.setName("15");
		band.setBandEdge(bandedge);
		//band.setBandStack(0, makeBandStack(21001000,DttSP.CWU,100,500));
		band.setBandStack(0, makeBandStack(21255000,Modes.USB,300,2700));
		//band.setBandStack(2, makeBandStack(21300000,DttSP.USB,300,2700));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(24890000);
		bandedge.setHigh(24990000);
		band=new Band();
		band.setName("12");
		band.setBandEdge(bandedge);
		//band.setBandStack(0, makeBandStack(24895000,DttSP.CWU,100,500));
		band.setBandStack(0, makeBandStack(24900000,Modes.USB,300,2700));
		//band.setBandStack(2, makeBandStack(24910000,DttSP.USB,300,2700));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(28000000);
		bandedge.setHigh(29700000);
		band=new Band();
		band.setName("10");
		band.setBandEdge(bandedge);
		//band.setBandStack(0, makeBandStack(28001000,DttSP.CWU,100,500));
		band.setBandStack(0, makeBandStack(28300000,Modes.USB,300,2700));
		//band.setBandStack(2, makeBandStack(28500000,DttSP.USB,300,2700));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(50000000);
		bandedge.setHigh(54000000);
		band=new Band();
		band.setName("6");
		band.setBandEdge(bandedge);
		//band.setBandStack(0, makeBandStack(50001000,DttSP.CWU,100,500));
		band.setBandStack(0, makeBandStack(50125000,Modes.USB,300,2700));
		//band.setBandStack(2, makeBandStack(50200000,DttSP.USB,300,2700));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(00000000);
		bandedge.setHigh(0000000);
		band=new Band();
		band.setName("WWV");
		band.setBandEdge(bandedge);
		//band.setBandStack(0, makeBandStack(5000000,DttSP.SAM,-2000,2000));
		band.setBandStack(0, makeBandStack(10000000,Modes.SAM,-2000,2000));
		//band.setBandStack(2, makeBandStack(15000000,DttSP.SAM,-2000,2000));
		bands.add(band);
		
		bandedge=new BandEdge();
		bandedge.setLow(00000000);
		bandedge.setHigh(0000000);
		band=new Band();
		band.setName("AM");
		band.setBandEdge(bandedge);
		band.setBandStack(0, makeBandStack(909000,Modes.AM,-4000,4000));
		//band.setBandStack(1, makeBandStack(5975000,DttSP.AM,-4000,4000));
		//band.setBandStack(2, makeBandStack(13845000,DttSP.AM,-4000,4000));
		bands.add(band);

        bandedge=new BandEdge();
        bandedge.setLow(00000000);
        bandedge.setHigh(61440000);
        band=new Band();
        band.setName("Gen");
        band.setBandEdge(bandedge);
        band.setBandStack(0, makeBandStack(11710000,Modes.AM,-4000,4000));
        //band.setBandStack(1, makeBandStack(5975000,DttSP.AM,-4000,4000));
        //band.setBandStack(2, makeBandStack(13845000,DttSP.AM,-4000,4000));
        bands.add(band);

	}
	
	private BandStack makeBandStack(long frequency,int mode,int filterlow,int filterhigh) {
		BandStack bandstack=new BandStack();
		bandstack.setFrequency(frequency);
		bandstack.setMode(mode);
		bandstack.setFilterLow(filterlow);
		bandstack.setFilterHigh(filterhigh);
		return bandstack;
	}
	
	private ArrayList<Band> bands=new ArrayList<Band>();
	
	private int current=0;
	

}
