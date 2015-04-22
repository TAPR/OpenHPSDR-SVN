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
public class Log {
    
    public static void i(Object obj,String message) {
        Log.i(obj.getClass().getName(),message);
    }
    
    public static void i(String app,String message) {
        System.err.println(app+": "+message);
    }
    
}
