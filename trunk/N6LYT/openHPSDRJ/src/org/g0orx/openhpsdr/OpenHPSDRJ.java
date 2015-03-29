/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.g0orx.openhpsdr;

/**
 *
 * @author john
 */
public class OpenHPSDRJ {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        Log.i("OpenHPSDRJ", "Running on: "+System.getProperty("os.name"));
        Radio radio=new Radio();
        radio.setVisible(true);
    }
    
}
