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
public class AGC {
    
    public static final int AGCOff = 0;
    public static final int AGCLong = 1;
    public static final int AGCSlow = 2;
    public static final int AGCMed = 3;
    public static final int AGCFast = 4;
    
    public String[] agclabels = {"Off", "Long", "Slow", "Medium", "Fast"};
}
