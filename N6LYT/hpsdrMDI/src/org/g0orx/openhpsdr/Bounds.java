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
public class Bounds implements java.io.Serializable {
    
    private static final long serialVersionUID = 2L;
    
    public int getX() {
        return X;
    }
    
    public void setX(int x) {
        this.X=x;
    }
    
    public int getY() {
        return Y;
    }
    
    public void setY(int y) {
        this.Y=y;
    }
    
    public int getWidth() {
        return width;
    }
    
    public void setWidth(int width) {
        this.width=width;
    }
    
    public int getHeight() {
        return height;
    }
    
    public void setHeight(int height) {
        this.height=height;
    }
    
    public String toString() {
        return "X="+X+" Y="+Y+" width="+width+" height="+height;
    }
    
    private int X;
    private int Y;
    private int width;
    private int height;
}
