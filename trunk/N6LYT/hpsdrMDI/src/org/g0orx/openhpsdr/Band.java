package org.g0orx.openhpsdr;

import java.io.Serializable;


public class Band implements Serializable {

    private static final long serialVersionUID = 2L;

    public Band() {

    }

        public Band(int bandstacksize) {
        bandstack=new BandStack[bandstacksize];
        }

        public void setName(String name) {
                this.name=name;
        }

        public void setBandStack(int entry,BandStack bandstack) throws IllegalArgumentException {
                if(entry<0 || entry>=this.bandstack.length) {
                        throw new IllegalArgumentException();
                }
                this.bandstack[entry]=bandstack;
        }

        public void setBandEdge(BandEdge bandedge) {
                this.bandedge=bandedge;
        }

        public void setDrive(float gain) {
                drive=gain;
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
    public int getBandstackentry() { return bandstackentry; }

    public void setAGC(int agc) {
        this.agc=agc;
    }

    public int getAGC() {
        return agc;
    }

    public void setAGCGain(double gain) {
        this.agcgain=gain;
    }

    public double getAGCGain() {
        return agcgain;
    }

    public float getDrive() {
        return drive;
    }

    public byte getOCRx() { return OCRx; }
    public void setOCRx(byte b) { OCRx=b; }
    public byte getOCTx() { return OCTx; }
    public void setOCTx(byte b) { OCTx=b; }

        public BandStack next() {
                bandstackentry++;
                if(bandstackentry>=bandstack.length) {
                        bandstackentry=0;
                }
                return bandstack[bandstackentry];
        }

        public BandStack previous() {
                bandstackentry--;
                if(bandstackentry<0) {
                        bandstackentry=bandstack.length-1;
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

    public void setTransmit(boolean state) { transmit=state; }
    public boolean canTransmit() { return transmit; }

        private String name;
        private int bandstackentry=1;
        private BandStack[] bandstack;
        private BandEdge bandedge;

        private boolean squelch=false;
        private int squelchvalue=100;

    private int agc=0;
    private double agcgain=60.0;

        private float drive=0.30F;

    private byte OCRx=0x00;
    private byte OCTx=0x00;

    private boolean transmit=true;


}
