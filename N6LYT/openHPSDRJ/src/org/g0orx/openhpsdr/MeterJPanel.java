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

/**
 *
 * @author john
 */
public class MeterJPanel extends javax.swing.JPanel {

    public MeterJPanel() {
        super();
    }

    public void paintComponent(Graphics g) {

        //Font font = g.getFont().deriveFont(28f);
        //g.setFont(font);
        g.setColor(Color.WHITE);
        g.fillRect(0, 0, this.getWidth(), this.getHeight());

        if (receiving) {
            double radius = (double) getHeight() / 2.0;
            double centerX = (double) getWidth() / 2.0;
            double centerY = (double) getHeight();

            double startAngle = 213.0;
            // draw the meter
            Font font = g.getFont().deriveFont(10f);
            g.setFont(font);
            g.setColor(Color.BLACK);
            for (int i = -127; i < -73; i += 6) {
                double angle = (double) (i + 127.0) + startAngle;
                double pX1 = centerX + (radius + 2) * Math.cos(Math.toRadians(angle));
                double pY1 = centerY + (radius + 2) * Math.sin(Math.toRadians(angle));
                double pX2 = centerX + (radius + 10) * Math.cos(Math.toRadians(angle));
                double pY2 = centerY + (radius + 10) * Math.sin(Math.toRadians(angle));
                if (i == -121) {
                    // S1
                    double pX = centerX + (radius + 12) * Math.cos(Math.toRadians(angle));
                    double pY = centerY + (radius + 12) * Math.sin(Math.toRadians(angle));
                    g.drawLine((int) pX1, (int) pY1, (int) pX2, (int) pY2);
                    g.drawString("1", (int) pX - 2, (int) pY);
                } else if (i == -109) {
                    // S3
                    double pX = centerX + (radius + 12) * Math.cos(Math.toRadians(angle));
                    double pY = centerY + (radius + 12) * Math.sin(Math.toRadians(angle));
                    g.drawLine((int) pX1, (int) pY1, (int) pX2, (int) pY2);
                    g.drawString("3", (int) pX - 2, (int) pY);
                } else if (i == -97) {
                    // S5
                    double pX = centerX + (radius + 12) * Math.cos(Math.toRadians(angle));
                    double pY = centerY + (radius + 12) * Math.sin(Math.toRadians(angle));
                    g.drawLine((int) pX1, (int) pY1, (int) pX2, (int) pY2);
                    g.drawString("5", (int) pX - 2, (int) pY);
                } else if (i == -85) {
                    // S7
                    double pX = centerX + (radius + 12) * Math.cos(Math.toRadians(angle));
                    double pY = centerY + (radius + 12) * Math.sin(Math.toRadians(angle));
                    g.drawLine((int) pX1, (int) pY1, (int) pX2, (int) pY2);
                    g.drawString("7", (int) pX - 2, (int) pY);
                } else {
                    pX1 = centerX + (radius + 2) * Math.cos(Math.toRadians(angle));
                    pY1 = centerY + (radius + 2) * Math.sin(Math.toRadians(angle));
                    pX2 = centerX + (radius + 6) * Math.cos(Math.toRadians(angle));
                    pY2 = centerY + (radius + 6) * Math.sin(Math.toRadians(angle));
                    g.drawLine((int) pX1, (int) pY1, (int) pX2, (int) pY2);
                }
            }
            g.setColor(Color.RED);
            for (int i = -73; i <= -13; i += 10) {
                double angle = (double) (i + 127.0) + startAngle;
                double pX1 = centerX + (radius + 2) * Math.cos(Math.toRadians(angle));
                double pY1 = centerY + (radius + 2) * Math.sin(Math.toRadians(angle));
                double pX2 = centerX + (radius + 10) * Math.cos(Math.toRadians(angle));
                double pY2 = centerY + (radius + 10) * Math.sin(Math.toRadians(angle));
                if (i == -73) {
                    // S9
                    double pX = centerX + (radius + 12) * Math.cos(Math.toRadians(angle));
                    double pY = centerY + (radius + 12) * Math.sin(Math.toRadians(angle));
                    g.drawString("9", (int) pX - 2, (int) pY);
                    g.drawLine((int) pX1, (int) pY1, (int) pX2, (int) pY2);
                } else if (i == -53) {
                    // S9+20
                    double pX = centerX + (radius + 12) * Math.cos(Math.toRadians(angle));
                    double pY = centerY + (radius + 12) * Math.sin(Math.toRadians(angle));
                    g.drawString("+20", (int) pX - 2, (int) pY);
                    g.drawLine((int) pX1, (int) pY1, (int) pX2, (int) pY2);
                } else if (i == -33) {
                    // S9+40
                    double pX = centerX + (radius + 12) * Math.cos(Math.toRadians(angle));
                    double pY = centerY + (radius + 12) * Math.sin(Math.toRadians(angle));
                    g.drawString("+40", (int) pX - 2, (int) pY);
                    g.drawLine((int) pX1, (int) pY1, (int) pX2, (int) pY2);
                } else if (i == -13) {
                    // S9+40
                    double pX = centerX + (radius + 12) * Math.cos(Math.toRadians(angle));
                    double pY = centerY + (radius + 12) * Math.sin(Math.toRadians(angle));
                    g.drawString("+60", (int) pX - 2, (int) pY);
                    g.drawLine((int) pX1, (int) pY1, (int) pX2, (int) pY2);
                } else {
                    pX1 = centerX + (radius + 2) * Math.cos(Math.toRadians(angle));
                    pY1 = centerY + (radius + 2) * Math.sin(Math.toRadians(angle));
                    pX2 = centerX + (radius + 6) * Math.cos(Math.toRadians(angle));
                    pY2 = centerY + (radius + 6) * Math.sin(Math.toRadians(angle));
                    g.drawLine((int) pX1, (int) pY1, (int) pX2, (int) pY2);
                }
            }

            g.setColor(Color.BLACK);
            font = g.getFont().deriveFont(18f);
            g.setFont(font);
            g.drawString(Integer.toString(meter) + " dBm", 10, 20);

            double angle = (double) (meter + 127.0) + startAngle;
            double pX = centerX + radius * Math.cos(Math.toRadians(angle));
            double pY = centerY + radius * Math.sin(Math.toRadians(angle));

            g.setColor(Color.BLACK);
            g.drawLine(this.getWidth() / 2, this.getHeight(), (int) pX, (int) pY);
        } else {
            Font font = g.getFont().deriveFont(18f);
            g.setFont(font);
            g.setColor(Color.BLACK);

            double volts = 0.0;
            double watts = 0.0;
            double fwdpower = 0.0;
            double revpower = 0.0;
            double swr = 1.0;
            boolean haveswr = false;

            double bridgevoltage = 0.09;
            double refvoltage = 3.3;
            switch (configuration.discovered.getDevice()) {
                case Discovered.DEVICE_METIS:
                    break;
                case Discovered.DEVICE_HERMES:
                    bridgevoltage = 0.095;
                    break;
                case Discovered.DEVICE_GRIFFIN:
                    break;
                case Discovered.DEVICE_ANGELIA:
                    break;
                case Discovered.DEVICE_ORION:
                    refvoltage = 5.0;
                    bridgevoltage = 0.083;
                    break;

            }
            switch (configuration.radio) {
                case Configuration.RADIO_UNKNOWN:
                case Configuration.METIS_PENELOPE:
                case Configuration.METIS_PENNYLANE:
                case Configuration.HERMES_BOARD_ONLY:
                case Configuration.ANGELIA_BOARD_ONLY:
                case Configuration.ORION_BOARD_ONLY:
                    volts = (double) penelope_forward_power / 4095.0 * refvoltage;
                    watts = (volts * volts) / bridgevoltage;
                    fwdpower = watts;
                    revpower = 0.0;
                    haveswr = false;
                    break;
                default:
                    volts = (double) alex_forward_power / 4095.0 * refvoltage;
                    watts = (volts * volts) / bridgevoltage;

                    fwdpower = watts;

                    volts = (double) alex_reverse_power / 4095.0 * refvoltage;
                    revpower = (volts * volts) / bridgevoltage;

                    double rho = Math.sqrt(revpower / fwdpower);
                    swr = Math.abs((1 + Math.abs(rho)) / (1 - Math.abs(rho)));
                    haveswr = true;
                    break;
            }

            if (fwdpower > peak_power) {
                peak_power = fwdpower;
            } else {
                peak_power_count++;
                if (peak_power_count == configuration.fps) {
                    peak_power_count = 0;
                    peak_power = fwdpower;
                }
            }

            if (swr > peak_swr) {
                peak_swr = swr;
            } else {
                peak_swr_count++;
                if (peak_swr_count == configuration.fps) {
                    peak_swr_count = 0;
                    peak_swr = swr;
                }
            }

            if(peak_power<1.0) {
                g.drawString(decimalFormat.format(peak_power)+" Watts", 10, 20);
            } else {
                g.drawString((int)peak_power+" Watts", 10, 20);
            }
            if(haveswr) {
                g.drawString("SWR "+decimalFormat.format(peak_swr)+":1", 10, 40);
            }

        }
    }

    public void setMeter(int meter) {
        if(configuration==null) {
            configuration = Configuration.getInstance();
        }
        this.meter = meter + (int) configuration.preampOffset + (int) configuration.meterCalibrationOffset;
        receiving = true;
        this.repaint();
    }

    public void setPower(int penelope_forward_power, int alex_forward_power, int alex_reverse_power) {
        if(configuration==null) {
            configuration = Configuration.getInstance();
        }
        this.penelope_forward_power = penelope_forward_power;
        this.alex_forward_power = alex_forward_power;
        this.alex_reverse_power = alex_reverse_power;
        receiving = false;
        this.repaint();
    }

    private Configuration configuration;

    private float density;

    private int meter = -127;
    private int penelope_forward_power;
    private int alex_forward_power;
    private int alex_reverse_power;

    private int peak_power_count = 0;
    private double peak_power = 0.0;
    private int peak_swr_count = 0;
    private double peak_swr = 0.0;

    private DecimalFormat decimalFormat = new DecimalFormat("0.0");
    private boolean receiving = true;
}
