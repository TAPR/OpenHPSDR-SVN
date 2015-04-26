package org.g0orx.openhpsdr;

/**
 * Created by john on 28/01/15.
 */
public class Filter {

    public Filter() {
    }

    public Filter(int low,int high,String name) {
        this.low=low;
        this.high=high;
        this.name=name;
    }

    public void setLow(int low) {
        this.low=low;
    }

    public void setHigh(int high) {
        this.high=high;
    }

    public void setName(String name) {
        this.name=name;
    }

    public int getLow() {
        return low;
    }

    public int getHigh() {
        return high;
    }

    public String getName() {
        return name;
    }

    private int low;
    private int high;
    private String name;
}

