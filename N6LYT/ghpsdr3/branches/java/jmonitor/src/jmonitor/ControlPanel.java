/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

/*
 * ControlPanel.java
 *
 * Created on 01-Feb-2010, 12:47:52
 */

package jmonitor;

import java.awt.Insets;
import java.text.DecimalFormat;
import javax.swing.JPopupMenu;

/**
 *
 * @author john
 */
public class ControlPanel extends javax.swing.JPanel {

    /** Creates new form ControlPanel */
    public ControlPanel() {
        initComponents();
       

        createLSBFilterMenu();
        
        bandMenu = new JPopupMenu("Band");
        bandMenu.add("160").addActionListener (new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFrequency(1850000);
                client.setFilter(-3440, -150);
                client.setMode(Client.modeLSB);
            }
        });
        bandMenu.add("80").addActionListener (new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFrequency(3890000);
                client.setFilter(-3440, -150);
                client.setMode(Client.modeLSB);
            }
        });
        bandMenu.add("60").addActionListener (new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFrequency(5371500);
                client.setFilter(-3440, -150);
                client.setMode(Client.modeLSB);
            }
        });
        bandMenu.add("40").addActionListener (new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFrequency(7048000);
                client.setFilter(-3440, -150);
                client.setMode(Client.modeLSB);
            }
        });
        bandMenu.add("30").addActionListener (new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFrequency(10135600);
                client.setFilter(150,3440);
                client.setMode(Client.modeUSB);
            }
        });
        bandMenu.add("20").addActionListener (new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFrequency(14200000);
                client.setFilter(150,3440);
                client.setMode(Client.modeUSB);
            }
        });
        bandMenu.add("17").addActionListener (new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFrequency(18118900);
                client.setFilter(150,3440);
                client.setMode(Client.modeUSB);
            }
        });
        bandMenu.add("15").addActionListener (new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFrequency(21200000);
                client.setFilter(150,3440);
                client.setMode(Client.modeUSB);
            }
        });
        bandMenu.add("12").addActionListener (new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFrequency(24910000);
                client.setFilter(150,3440);
                client.setMode(Client.modeUSB);
            }
        });
        bandMenu.add("10").addActionListener (new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFrequency(28500000);
                client.setFilter(150,3440);
                client.setMode(Client.modeUSB);
            }
        });
        bandMenu.add("6").addActionListener (new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFrequency(50200000);
                client.setFilter(150,3440);
                client.setMode(Client.modeUSB);
            }
        });
        bandMenu.add("GEN").addActionListener (new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFrequency(909000);
                client.setFilter(-4000,4000);
                client.setMode(Client.modeAM);
            }

        });
        bandMenu.add("WWV").addActionListener (new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFrequency(5000000);
                client.setFilter(-4000,4000);
                client.setMode(Client.modeSAM);
            }
        });


        modeMenu = new JPopupMenu("Mode");
        modeMenu.add("LSB").addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setMode(0);
                client.setFilter(-2850, -150);
                createLSBFilterMenu();
            }
        });
        modeMenu.add("USB").addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setMode(1);
                client.setFilter(150, 2850);
                createUSBFilterMenu();
            }
        });
        modeMenu.add("DSB").addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setMode(2);
                client.setFilter(-2600, 2600);
                createAMFilterMenu();
            }
        });
        modeMenu.add("CWL").addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setMode(3);
                client.setFilter(-800, -400);
                createCWFilterMenu();
            }
        });
        modeMenu.add("CWU").addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setMode(4);
                client.setFilter(400, 800);
                createCWFilterMenu();
            }
        });
        modeMenu.add("SPEC").addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setMode(8);
                client.setFilter(-6000, -6000);
            }
        });
        modeMenu.add("DIGL").addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setMode(9);
                client.setFilter(-3450, -150);
                createDIGLFilterMenu();
            }
        });
        modeMenu.add("DIGU").addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setMode(7);
                client.setFilter(150, 3450);
                createDIGUFilterMenu();
            }
        });
        modeMenu.add("DRM").addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setMode(11);
                client.setFilter(-6000, 6000);
                filterMenu=null;
            }
        });
        modeMenu.add("AM").addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setMode(6);
                client.setFilter(-4000,4000);
                createAMFilterMenu();
            }
        });
        modeMenu.add("SAM").addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setMode(10);
                client.setFilter(-4000,4000);
                createAMFilterMenu();
            }
        });
        modeMenu.add("FMN").addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setMode(5);
                client.setFilter(-2600, 2600);
                createAMFilterMenu();
            }
        });

    }

    public void setClient(Client client) {
        this.client=client;
    }

    public void update() {
        long f=client.getFrequency();
        String fString=mhzFormat.format(f/1000000)+"."+khzFormat.format(f%1000000);
        
        vfo.setText(fString);
        mode.setText(client.getStringMode());
        digitalMeter.update(client.getMeter());
        
    }

    /** This method is called from within the constructor to
     * initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is
     * always regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        bandButton = new javax.swing.JButton();
        modeButton = new javax.swing.JButton();
        vfo = new javax.swing.JTextField();
        mode = new javax.swing.JTextField();
        digitalMeter = new jmonitor.DigitalMeter();
        filterButton = new javax.swing.JButton();

        setMaximumSize(new java.awt.Dimension(480, 27));
        setMinimumSize(new java.awt.Dimension(480, 27));
        setPreferredSize(new java.awt.Dimension(480, 27));
        setLayout(new org.netbeans.lib.awtextra.AbsoluteLayout());

        bandButton.setFont(new java.awt.Font("DejaVu Sans", 0, 12)); // NOI18N
        bandButton.setText("Band");
        bandButton.setAlignmentY(0.0F);
        bandButton.setMaximumSize(new java.awt.Dimension(100, 27));
        bandButton.setMinimumSize(new java.awt.Dimension(100, 27));
        bandButton.setPreferredSize(new java.awt.Dimension(100, 27));
        bandButton.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                bandButtonActionPerformed(evt);
            }
        });
        add(bandButton, new org.netbeans.lib.awtextra.AbsoluteConstraints(0, 0, -1, -1));

        modeButton.setFont(new java.awt.Font("DejaVu Sans", 0, 12)); // NOI18N
        modeButton.setText("Mode");
        modeButton.setAlignmentY(0.0F);
        modeButton.setMaximumSize(new java.awt.Dimension(100, 27));
        modeButton.setMinimumSize(new java.awt.Dimension(100, 27));
        modeButton.setPreferredSize(new java.awt.Dimension(100, 27));
        modeButton.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                modeButtonActionPerformed(evt);
            }
        });
        add(modeButton, new org.netbeans.lib.awtextra.AbsoluteConstraints(0, 30, -1, -1));

        vfo.setBackground(new java.awt.Color(0, 0, 0));
        vfo.setEditable(false);
        vfo.setFont(new java.awt.Font("FreeMono", 1, 24)); // NOI18N
        vfo.setForeground(new java.awt.Color(0, 255, 0));
        vfo.setHorizontalAlignment(javax.swing.JTextField.RIGHT);
        vfo.setText("2400.000.000");
        vfo.setBorder(javax.swing.BorderFactory.createEmptyBorder(1, 1, 1, 1));
        vfo.setMaximumSize(new java.awt.Dimension(178, 26));
        vfo.setMinimumSize(new java.awt.Dimension(178, 26));
        vfo.setPreferredSize(new java.awt.Dimension(178, 26));
        add(vfo, new org.netbeans.lib.awtextra.AbsoluteConstraints(250, 30, 180, 30));

        mode.setBackground(new java.awt.Color(0, 0, 0));
        mode.setEditable(false);
        mode.setFont(new java.awt.Font("DejaVu Sans", 1, 14)); // NOI18N
        mode.setForeground(new java.awt.Color(0, 255, 0));
        mode.setHorizontalAlignment(javax.swing.JTextField.RIGHT);
        mode.setText("LSB");
        mode.setBorder(javax.swing.BorderFactory.createEmptyBorder(1, 1, 1, 1));
        add(mode, new org.netbeans.lib.awtextra.AbsoluteConstraints(430, 30, 50, 30));

        digitalMeter.setPreferredSize(new java.awt.Dimension(120, 27));

        javax.swing.GroupLayout digitalMeterLayout = new javax.swing.GroupLayout(digitalMeter);
        digitalMeter.setLayout(digitalMeterLayout);
        digitalMeterLayout.setHorizontalGroup(
            digitalMeterLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 140, Short.MAX_VALUE)
        );
        digitalMeterLayout.setVerticalGroup(
            digitalMeterLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 20, Short.MAX_VALUE)
        );

        add(digitalMeter, new org.netbeans.lib.awtextra.AbsoluteConstraints(340, 0, 140, 20));

        filterButton.setText("Filter");
        filterButton.setMaximumSize(new java.awt.Dimension(100, 27));
        filterButton.setMinimumSize(new java.awt.Dimension(100, 27));
        filterButton.setPreferredSize(new java.awt.Dimension(100, 27));
        filterButton.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                filterButtonActionPerformed(evt);
            }
        });
        add(filterButton, new org.netbeans.lib.awtextra.AbsoluteConstraints(100, 0, 90, -1));
    }// </editor-fold>//GEN-END:initComponents

    private void bandButtonActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_bandButtonActionPerformed
        bandMenu.show(bandButton, 0, 0);
    }//GEN-LAST:event_bandButtonActionPerformed

    private void modeButtonActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_modeButtonActionPerformed
        modeMenu.show(modeButton, 0, 0);
    }//GEN-LAST:event_modeButtonActionPerformed

    private void filterButtonActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_filterButtonActionPerformed
        if(filterMenu!=null) {
            filterMenu.show(filterButton, 0, 0);
        }
    }//GEN-LAST:event_filterButtonActionPerformed


    private void createLSBFilterMenu() {
        filterMenu = new JPopupMenu("Filter");
        filterMenu.add("5.0k").addActionListener (new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-5150,-150);
            }
        });
        filterMenu.add("4.4k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-4550, -150);
            }
        });
        filterMenu.add("3.8k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-3950, -150);
            }
        });
        filterMenu.add("3.3k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-3440, -150);
            }
        });
        filterMenu.add("2.9k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-3050, -150);
            }
        });
        filterMenu.add("2.7k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-2850, -150);
            }
        });
        filterMenu.add("2.4k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-2550, -150);
            }
        });
        filterMenu.add("2.1k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-2250, -150);
            }
        });
        filterMenu.add("1.8k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-1950, -150);
            }
        });
        filterMenu.add("1.0k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-1150, -150);
            }
        });
    }

    private void createUSBFilterMenu() {
        filterMenu = new JPopupMenu("Filter");
        filterMenu.add("5.0k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150,5150);
            }
        });
        filterMenu.add("4.4k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150,4550);
            }
        });
        filterMenu.add("3.8k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150,3950);
            }
        });
        filterMenu.add("3.3k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150,3440);
            }
        });
        filterMenu.add("2.9k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150,3050);
            }
        });
        filterMenu.add("2.7k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150,2850);
            }
        });
        filterMenu.add("2.4k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150,2550);
            }
        });
        filterMenu.add("2.1k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150,2250);
            }
        });
        filterMenu.add("1.8k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150,1950);
            }
        });
        filterMenu.add("1.0k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150,1150);
            }
        });
    }

    private void createCWFilterMenu() {
        filterMenu = new JPopupMenu("Filter");
        filterMenu.add("1.0k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                if(client.getMode()==Client.modeCWL) {
                    client.setFilter(-client.getCWPitch()-500,-client.getCWPitch()+500);
                } else {
                    client.setFilter(client.getCWPitch()-500,client.getCWPitch()+500);
                }
            }
        });
        filterMenu.add("800").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                if(client.getMode()==Client.modeCWL) {
                    client.setFilter(-client.getCWPitch()-400,-client.getCWPitch()+400);
                } else {
                    client.setFilter(client.getCWPitch()-400,client.getCWPitch()+400);
                }
            }
        });
        filterMenu.add("750").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                if(client.getMode()==Client.modeCWL) {
                    client.setFilter(-client.getCWPitch()-375,-client.getCWPitch()+375);
                } else {
                    client.setFilter(client.getCWPitch()-375,client.getCWPitch()+375);
                }
            }
        });
        filterMenu.add("600").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                if(client.getMode()==Client.modeCWL) {
                    client.setFilter(-client.getCWPitch()-300,-client.getCWPitch()+300);
                } else {
                    client.setFilter(client.getCWPitch()-300,client.getCWPitch()+300);
                }
            }
        });
        filterMenu.add("500").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                if(client.getMode()==Client.modeCWL) {
                    client.setFilter(-client.getCWPitch()-250,-client.getCWPitch()+250);
                } else {
                    client.setFilter(client.getCWPitch()-250,client.getCWPitch()+250);
                }
            }
        });
        filterMenu.add("400").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                if(client.getMode()==Client.modeCWL) {
                    client.setFilter(-client.getCWPitch()-200,-client.getCWPitch()+200);
                } else {
                    client.setFilter(client.getCWPitch()-200,client.getCWPitch()+200);
                }
            }
        });
        filterMenu.add("250").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                if(client.getMode()==Client.modeCWL) {
                    client.setFilter(-client.getCWPitch()-125,-client.getCWPitch()+125);
                } else {
                    client.setFilter(client.getCWPitch()-125,client.getCWPitch()+125);
                }
            }
        });
        filterMenu.add("100").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                if(client.getMode()==Client.modeCWL) {
                    client.setFilter(-client.getCWPitch()-50,-client.getCWPitch()+50);
                } else {
                    client.setFilter(client.getCWPitch()-50,client.getCWPitch()+50);
                }
            }
        });
        filterMenu.add("50").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                if(client.getMode()==Client.modeCWL) {
                    client.setFilter(-client.getCWPitch()-25,-client.getCWPitch()+25);
                } else {
                    client.setFilter(client.getCWPitch()-25,client.getCWPitch()+25);
                }
            }
        });
        filterMenu.add("25").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                if(client.getMode()==Client.modeCWL) {
                    client.setFilter(-client.getCWPitch()-13,-client.getCWPitch()+13);
                } else {
                    client.setFilter(client.getCWPitch()-13,client.getCWPitch()+13);
                }
            }
        });
    }


    private void createDIGLFilterMenu() {
        filterMenu = new JPopupMenu("Filter");
        filterMenu.add("5.0k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-5150, -150);
            }
        });
        filterMenu.add("4.4k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-4550, -150);
            }
        });
        filterMenu.add("3.8k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-3950, -150);
            }
        });
        filterMenu.add("3.3k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-3440, -150);
            }
        });
        filterMenu.add("2.9k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-3050, -150);
            }
        });
        filterMenu.add("2.7k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-2850, -150);
            }
        });
        filterMenu.add("2.4k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-2550, -150);
            }
        });
        filterMenu.add("2.1k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-2250, -150);
            }
        });
        filterMenu.add("1.8k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-1950, -150);
            }
        });
        filterMenu.add("1.0k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-1150, -150);
            }
        });
    }

    private void createDIGUFilterMenu() {
        filterMenu = new JPopupMenu("Filter");
        filterMenu.add("5.0k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150, 5150);
            }
        });
        filterMenu.add("4.4k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150, 4550);
            }
        });
        filterMenu.add("3.8k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150, 3950);
            }
        });
        filterMenu.add("3.3k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150, 3440);
            }
        });
        filterMenu.add("2.9k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150, 3050);
            }
        });
        filterMenu.add("2.7k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150, 2850);
            }
        });
        filterMenu.add("2.4k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150, 2550);
            }
        });
        filterMenu.add("2.1k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150, 2250);
            }
        });
        filterMenu.add("1.8k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150, 1950);
            }
        });
        filterMenu.add("1.0k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(150, 1150);
            }
        });
    }

    private void createAMFilterMenu() {
        filterMenu = new JPopupMenu("Filter");
        filterMenu.add("16.0k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-8000,8000);
            }
        });
        filterMenu.add("12.0k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-6000,6000);
            }
        });
        filterMenu.add("10.0k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-5000,5000);
            }
        });
        filterMenu.add("8.0k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-4000,4000);
            }
        });
        filterMenu.add("6.6k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-3300,3300);
            }
        });
        filterMenu.add("5.2k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-2600,2600);
            }
        });
        filterMenu.add("4.0k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-2000,2000);
            }
        });
        filterMenu.add("3.1k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-1550,1550);
            }
        });
        filterMenu.add("2.9k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-1450,1450);
            }
        });
        filterMenu.add("2.4k").addActionListener(new java.awt.event.ActionListener() {

            public void actionPerformed(java.awt.event.ActionEvent evt) {
                client.setFilter(-1200,1200);
            }
        });
    }



    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton bandButton;
    private jmonitor.DigitalMeter digitalMeter;
    private javax.swing.JButton filterButton;
    private javax.swing.JTextField mode;
    private javax.swing.JButton modeButton;
    private javax.swing.JTextField vfo;
    // End of variables declaration//GEN-END:variables

    private Client client;

    private JPopupMenu bandMenu;
    private JPopupMenu modeMenu;
    private JPopupMenu filterMenu;

    private DecimalFormat mhzFormat=new DecimalFormat("####0");
    private DecimalFormat khzFormat=new DecimalFormat("000000");

}
