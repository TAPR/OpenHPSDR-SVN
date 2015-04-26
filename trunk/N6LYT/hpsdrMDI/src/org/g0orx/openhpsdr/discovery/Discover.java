/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.g0orx.openhpsdr.discovery;

/**
 *
 * @author john
 */
public interface Discover {
    
    public void discovered(Discovered d);
    public void endDiscovery();
    
}
