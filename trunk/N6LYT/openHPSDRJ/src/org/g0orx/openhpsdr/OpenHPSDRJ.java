/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.g0orx.openhpsdr;

import java.io.File;
import org.g0orx.openhpsdr.wdsp.WDSP;

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
        Log.i("OpenHPSDRJ", "Current working directory: "+System.getProperty("user.dir"));
        //System.setProperty("java.library.path", System.getProperty("user.dir")+File.separator+"lib");
        //Log.i("OpenHPSDRJ", "java.library.path: "+System.getProperty("java.library.path"));
        Radio radio=new Radio();
        radio.setVisible(true);
        //WDSP wdsp=WDSP.getInstance();
    }
    
}
