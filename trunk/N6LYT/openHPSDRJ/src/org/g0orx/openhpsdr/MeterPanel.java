/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.g0orx.openhpsdr;

import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import java.text.DecimalFormat;

import org.g0orx.openhpsdr.discovery.Discovered;
import org.g0orx.openhpsdr.modes.Modes;

/**
 *
 * @author john
 */
public class MeterPanel extends javax.swing.JPanel {

    /**
     * Creates new form MeterJPanel
     */
    public MeterPanel() {
        initComponents();
        configuration = Configuration.getInstance();
    }

    public void paintComponent(Graphics g) {

        Font font = g.getFont().deriveFont(28f);
        g.setFont(font);

        g.setColor(Color.BLACK);
        g.fillRect(0, 0, this.getWidth(), this.getHeight());

        String value1=null;
        String value2=null;
        
        if (receiving) {
            value1 = sunits;
            value2=Integer.toString(meter) + " dBm";
            g.setColor(Color.GREEN);
        } else {
            double volts = 0.0;
            double watts = 0.0;
            double fwdpower = 0.0;
            double revpower = 0.0;
            double swr = 1.0;
            boolean haveswr = false;

            double bridgevoltage = 0.09;
            double refvoltage = 3.3;
            /*
             switch(configuration.discovered.getDevice()) {
             case Discovered.DEVICE_METIS:
             break;
             case Discovered.DEVICE_HERMES:
             bridgevoltage=0.095;
             break;
             case Discovered.DEVICE_GRIFFIN:
             break;
             case Discovered.DEVICE_ANGELIA:
             break;
             case Discovered.DEVICE_ORION:
             refvoltage=5.0;
             bridgevoltage=0.083;
             break;

             }
             switch(configuration.radio) {
             case Configuration.RADIO_UNKNOWN:
             case Configuration.METIS_PENELOPE:
             case Configuration.METIS_PENNYLANE:
             case Configuration.HERMES_BOARD_ONLY:
             case Configuration.ANGELIA_BOARD_ONLY:
             case Configuration.ORION_BOARD_ONLY:
             volts=(double)penelope_forward_power/4095.0 * refvoltage;
             watts = (volts * volts) / bridgevoltage;
             fwdpower=watts;
             revpower=0.0;
             haveswr=false;
             break;
             default:
             volts = (double)alex_forward_power / 4095.0 * refvoltage;
             watts = (volts*volts) / bridgevoltage;

             fwdpower=watts;

             volts = (double) alex_reverse_power / 4095.0 * refvoltage;
             revpower = (volts * volts) / bridgevoltage;

             double rho = Math.sqrt(revpower / fwdpower);
             swr = Math.abs((1 + Math.abs(rho)) / (1 - Math.abs(rho)));
             haveswr=true;
             break;
             }
             */
            volts = (double) alex_forward_power / 4095.0 * refvoltage;
            watts = (volts * volts) / bridgevoltage;
            fwdpower = watts;

            if (fwdpower > peak_power) {
                peak_power = fwdpower;
            } else {
                peak_power_count++;
                if (peak_power_count == configuration.fps) {
                    peak_power_count = 0;
                    peak_power = fwdpower;
                }
            }

            volts = (double) alex_reverse_power / 4095.0 * refvoltage;
            revpower = (volts * volts) / bridgevoltage;

            double rho = Math.sqrt(revpower / fwdpower);
            swr = Math.abs((1 + Math.abs(rho)) / (1 - Math.abs(rho)));

            if (swr > peak_swr) {
                peak_swr = swr;
            } else {
                peak_swr_count++;
                if (peak_swr_count == configuration.fps) {
                    peak_swr_count = 0;
                    peak_swr = swr;
                }
            }

            value1 = (int) peak_power + " Watts";
            value2 = "SWR " + decimalFormat.format(peak_swr) + ":1";
            //value = "peak="+(int)peak_power+" fwd=" + (int)fwdpower + " rev=" + (int)revpower + " swr=" + (int)swr+":1";
            g.setColor(Color.RED);
        }

        g.drawString(value1, 10, this.getHeight() - 10);
        g.drawString(value2, (this.getWidth()/2)+10, this.getHeight() - 10);

    }

    public void setMeter(int meter) {
        this.meter = meter + (int) configuration.preampOffset + (int) configuration.meterCalibrationOffset;
        if (this.meter <= -121) {
            this.sunits = "S1";
        } else if (this.meter <= -115) {
            this.sunits = "S2";
        } else if (this.meter <= -109) {
            this.sunits = "S3";
        } else if (this.meter <= -103) {
            this.sunits = "S4";
        } else if (this.meter <= -97) {
            this.sunits = "S5";
        } else if (this.meter <= -91) {
            this.sunits = "S6";
        } else if (this.meter <= -85) {
            this.sunits = "S7";
        } else if (this.meter <= -79) {
            this.sunits = "S8";
        } else if (this.meter <= -73) {
            this.sunits = "S9";
        } else if (this.meter <= -63) {
            this.sunits = "S9+10";
        } else if (this.meter <= -53) {
            this.sunits = "S9+20";
        } else if (this.meter <= -43) {
            this.sunits = "S9+30";
        } else if (this.meter <= -33) {
            this.sunits = "S9+40";
        } else if (this.meter <= -23) {
            this.sunits = "S9+50";
        } else {
            this.sunits = "S9+60";
        }
        receiving = true;
        this.repaint();
    }

    public void setPower(int penelope_forward_power, int alex_forward_power, int alex_reverse_power) {
        this.penelope_forward_power = penelope_forward_power;
        this.alex_forward_power = alex_forward_power;
        this.alex_reverse_power = alex_reverse_power;
        receiving = false;
        this.repaint();
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(this);
        this.setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 337, Short.MAX_VALUE)
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 39, Short.MAX_VALUE)
        );
    }// </editor-fold>//GEN-END:initComponents

    private Configuration configuration;

    private float density;

    private int meter;
    private String sunits = "";
    private int penelope_forward_power;
    private int alex_forward_power;
    private int alex_reverse_power;

    private int peak_power_count = 0;
    private double peak_power = 0.0;
    private int peak_swr_count = 0;
    private double peak_swr = 0.0;

    private DecimalFormat decimalFormat = new DecimalFormat("0.0");
    private boolean receiving = true;

    // Variables declaration - do not modify//GEN-BEGIN:variables
    // End of variables declaration//GEN-END:variables
}
