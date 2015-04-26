/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.g0orx.openhpsdr;

import java.io.Serializable;

/**
 *
 * @author john
 */
public class Receiver implements Serializable {

    private static final long serialVersionUID = 1L;
    
    public Receiver() {   
    }
    
    public Receiver(int id) {
        this.id=id;
        bands=new Bands();
    }
    
    public int getId() {
        return id;
    }
    
    public Bands getBands() {
        return bands;
    }
    
    public int getStep() {
        return step;
    }
    
    public void setStep(int step) {
        this.step=step;
    }
    
    public void setStep(String s) {
        int v=Integer.parseInt(s);
        for(int i=0;i<Step.length();i++) {
            if(Step.getStep(i)==v) {
                setStep(i);
                break;
            }
        }
    }
    
    public int getADC() {
        return adc;
    }
    
    public void setADC(int adc) {
        this.adc=adc;
    }
    
    public int getX() {
        return myX;
    }
    
    public void setX(int x) {
        this.myX=x;
    }
    
    public int getY() {
        return myY;
    }
    
    public void setY(int y) {
        this.myY=y;
    }
    
    public int getWidth() {
        return myWidth;
    }
    
    public void setWidth(int width) {
        this.myWidth=width;
    }
    
    public int getHeight() {
        return myHeight;
    }
    
    public void setHeight(int height) {
        this.myHeight=height;
    }
    
    public int getSpectrumLow() {
        return spectrumLow;
    }
    
    public void setSpectrumLow(int low) {
        this.spectrumLow=low;
    }
    
    public int getSpectrumHigh() {
        return spectrumHigh;
    }
    
    public void setSpectrumHigh(int high) {
        this.spectrumHigh=high;
    }
    
    public int getWaterfallLow() {
        return waterfallLow;
    }
    
    public void setWaterfallLow(int low) {
        this.waterfallLow=low;
    }
    
    public int getWaterfallHigh() {
        return waterfallHigh;
    }
    
    public void setWaterfallHigh(int high) {
        this.waterfallHigh=high;
    }
    
    public boolean isWaterfallGrayscale() {
        return waterfallGrayscale;
    }
    
    
    public void setWaterfallGrayscale(boolean state) {
       this.waterfallGrayscale=state;
    }
   
    public boolean isWaterfallAutomatic() {
        return waterfallAutomatic;
    }
    
    public void setWaterfallAutomatic(boolean state) {
        this.waterfallAutomatic=state;
    }
    
    public boolean displayPanadapter() {
        return displayPanadapter;
    }

    public void setDisplayPanadapter(boolean state) {
        this.displayPanadapter=state;
    }
    public boolean displayWaterfall() {
        return displayWaterfall;
    }
    
    public void setDisplayWaterfall(boolean state) {
        this.displayWaterfall=state;
    }
    
    private int id;
    
    private Bands bands;
    
    private int step=4;
    
    public static final int ADC1=0;
    public static final int ADC2=1;
    public static final int ADC3=2;
    private int adc;
    
    private int myX=0;
    private int myY=0;
    private int myWidth=0;
    private int myHeight=0;
    
    private int spectrumHigh = -60;
    private int spectrumLow = -140;
    
    private int waterfallHigh = -62;
    private int waterfallLow = -122;
    private boolean waterfallGrayscale = false;
    private boolean waterfallAutomatic = true;
    
    private boolean displayPanadapter=true;
    private boolean displayWaterfall=true;

}
