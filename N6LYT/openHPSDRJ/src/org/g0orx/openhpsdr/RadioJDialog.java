/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.g0orx.openhpsdr;

import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;
import javax.swing.JRadioButton;

import org.g0orx.openhpsdr.wdsp.WDSP;
import org.g0orx.openhpsdr.discovery.Discovered;

/**
 *
 * @author john
 */
public class RadioJDialog extends javax.swing.JDialog {

    /**
     * Creates new form RadioJDialog
     */
    public RadioJDialog(java.awt.Frame parent, boolean modal) {
        super(parent, modal);
        initComponents();
        configuration = Configuration.getInstance();
        wdsp = WDSP.getInstance();
        this.jCheckBoxAEFilter.setSelected(configuration.NB2_AE);
        this.jRadioButtonLinear.setSelected(configuration.NB2_GAIN == 0);
        this.jRadioButtonLog.setSelected(configuration.NB2_GAIN == 1);
        this.jRadioButtonOSMS.setSelected(configuration.NB2_NPE == 0);
        this.jRadioButtonMMSE.setSelected(configuration.NB2_NPE == 1);
        this.jRadioButtonPreAGC.setSelected(configuration.NB2_POSITION == 0);
        this.jRadioButtonPostAGC.setSelected(configuration.NB2_POSITION == 1);
            
        Band band = configuration.bands.get();
        JRadioButton rb;
        if (configuration.radio != Configuration.METIS_PENELOPE
                && configuration.radio != Configuration.METIS_PENNYLANE) {
            switch (band.get().getTxAntenna()) {
                case 0:
                    rb = (JRadioButton) this.jRadioButtonAnt1;
                    rb.setSelected(true);
                    break;
                case 1:
                    rb = (JRadioButton) this.jRadioButtonAnt2;
                    rb.setSelected(true);
                    break;
                case 2:
                    if (configuration.radio != Configuration.METIS_PENELOPE
                            && configuration.radio != Configuration.METIS_PENNYLANE) {
                        switch (band.get().getTxAntenna()) {
                            case 0:
                                rb = (JRadioButton) this.jRadioButtonAnt1;
                                rb.setSelected(true);
                                break;
                            case 1:
                                rb = (JRadioButton) this.jRadioButtonAnt2;
                                rb.setSelected(true);
                                break;
                            case 2:
                                rb = (JRadioButton) this.jRadioButtonAnt3;
                                rb.setSelected(true);
                                break;
                        }
                    } else {
                        jRadioButtonAnt1.setEnabled(false);
                        jRadioButtonAnt2.setEnabled(false);
                        jRadioButtonAnt3.setEnabled(false);
                    }
                    rb = (JRadioButton) this.jRadioButtonAnt3;
                    rb.setSelected(true);
                    break;
            }
        } else {
            jRadioButtonAnt1.setEnabled(false);
            jRadioButtonAnt2.setEnabled(false);
            jRadioButtonAnt3.setEnabled(false);
        }

        if (configuration.radio != Configuration.METIS_PENELOPE
                && configuration.radio != Configuration.METIS_PENNYLANE) {
            if (configuration.radio == Configuration.ANGELIA_ANAN100D || configuration.radio == Configuration.ORION_ANAN200D) {
                rb = (JRadioButton) this.jRadioButtonRX1;
                rb.setText("EXT 2");
                rb = (JRadioButton) this.jRadioButtonRX2;
                rb.setText("EXT 1");
            }

            switch (band.get().getRxAntenna()) {
                case 0:
                    rb = (JRadioButton) this.jRadioButtonNone;
                    rb.setSelected(true);
                    break;
                case 1:
                    rb = (JRadioButton) this.jRadioButtonRX1;
                    rb.setSelected(true);
                    break;
                case 2:
                    rb = (JRadioButton) this.jRadioButtonRX2;
                    rb.setSelected(true);
                    break;
                case 3:
                    rb = (JRadioButton) this.jRadioButtonRXXV;
                    rb.setSelected(true);
                    break;
            }

        } else {
            jRadioButtonNone.setEnabled(false);
            jRadioButtonRX1.setEnabled(false);
            jRadioButtonRX2.setEnabled(false);
            jRadioButtonRXXV.setEnabled(false);
        }

        this.jLabelPASettings.setText("Band: "+band.getName());
        this.jSliderPASettings.setValue((int)(band.getDrive()*100.0));
        
        this.jLabelOCBand.setText("Band: "+band.getName());
        this.jCheckBoxOCRx1.setSelected((band.getOCRx()&0x01)==0x01);
        this.jCheckBoxOCRx2.setSelected((band.getOCRx()&0x02)==0x02);
        this.jCheckBoxOCRx3.setSelected((band.getOCRx()&0x04)==0x04);
        this.jCheckBoxOCRx4.setSelected((band.getOCRx()&0x08)==0x08);
        this.jCheckBoxOCRx5.setSelected((band.getOCRx()&0x10)==0x10);
        this.jCheckBoxOCRx6.setSelected((band.getOCRx()&0x20)==0x20);
        this.jCheckBoxOCRx7.setSelected((band.getOCRx()&0x40)==0x40);
        this.jCheckBoxOCTx1.setSelected((band.getOCTx()&0x01)==0x01);
        this.jCheckBoxOCTx2.setSelected((band.getOCTx()&0x02)==0x02);
        this.jCheckBoxOCTx3.setSelected((band.getOCTx()&0x04)==0x04);
        this.jCheckBoxOCTx4.setSelected((band.getOCTx()&0x08)==0x08);
        this.jCheckBoxOCTx5.setSelected((band.getOCTx()&0x10)==0x10);
        this.jCheckBoxOCTx6.setSelected((band.getOCTx()&0x20)==0x20);
        this.jCheckBoxOCTx7.setSelected((band.getOCTx()&0x40)==0x40);

        this.jCheckBox20dBMicBoost.setSelected(configuration.micboost);
    }
    
    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        buttonGroupGain = new javax.swing.ButtonGroup();
        buttonGroupNPEMethod = new javax.swing.ButtonGroup();
        buttonGroupPosition = new javax.swing.ButtonGroup();
        buttonGroupTXAnt = new javax.swing.ButtonGroup();
        buttonGroupRXAnt = new javax.swing.ButtonGroup();
        jTabbedPane1 = new javax.swing.JTabbedPane();
        jPanel3 = new javax.swing.JPanel();
        jRadioButtonAnt1 = new javax.swing.JRadioButton();
        jRadioButtonAnt2 = new javax.swing.JRadioButton();
        jRadioButtonAnt3 = new javax.swing.JRadioButton();
        jRadioButtonNone = new javax.swing.JRadioButton();
        jRadioButtonRX1 = new javax.swing.JRadioButton();
        jRadioButtonRX2 = new javax.swing.JRadioButton();
        jRadioButtonRXXV = new javax.swing.JRadioButton();
        jLabel4 = new javax.swing.JLabel();
        jLabel5 = new javax.swing.JLabel();
        jPanel1 = new javax.swing.JPanel();
        jLabel1 = new javax.swing.JLabel();
        jRadioButtonLinear = new javax.swing.JRadioButton();
        jRadioButtonLog = new javax.swing.JRadioButton();
        jLabel2 = new javax.swing.JLabel();
        jRadioButtonOSMS = new javax.swing.JRadioButton();
        jRadioButtonMMSE = new javax.swing.JRadioButton();
        jLabel3 = new javax.swing.JLabel();
        jRadioButtonPreAGC = new javax.swing.JRadioButton();
        jRadioButtonPostAGC = new javax.swing.JRadioButton();
        jCheckBoxAEFilter = new javax.swing.JCheckBox();
        jPanel2 = new javax.swing.JPanel();
        jLabelPASettings = new javax.swing.JLabel();
        jSliderPASettings = new javax.swing.JSlider();
        jPanel4 = new javax.swing.JPanel();
        jLabelOCBand = new javax.swing.JLabel();
        jLabel7 = new javax.swing.JLabel();
        jLabel8 = new javax.swing.JLabel();
        jCheckBoxOCRx1 = new javax.swing.JCheckBox();
        jCheckBoxOCRx2 = new javax.swing.JCheckBox();
        jCheckBoxOCRx3 = new javax.swing.JCheckBox();
        jCheckBoxOCRx4 = new javax.swing.JCheckBox();
        jCheckBoxOCRx5 = new javax.swing.JCheckBox();
        jCheckBoxOCRx6 = new javax.swing.JCheckBox();
        jCheckBoxOCRx7 = new javax.swing.JCheckBox();
        jCheckBoxOCTx1 = new javax.swing.JCheckBox();
        jCheckBoxOCTx2 = new javax.swing.JCheckBox();
        jCheckBoxOCTx3 = new javax.swing.JCheckBox();
        jCheckBoxOCTx4 = new javax.swing.JCheckBox();
        jCheckBoxOCTx5 = new javax.swing.JCheckBox();
        jCheckBoxOCTx6 = new javax.swing.JCheckBox();
        jCheckBoxOCTx7 = new javax.swing.JCheckBox();
        jPanel5 = new javax.swing.JPanel();
        jCheckBox20dBMicBoost = new javax.swing.JCheckBox();
        jButtonClose = new javax.swing.JButton();

        setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);

        buttonGroupTXAnt.add(jRadioButtonAnt1);
        jRadioButtonAnt1.setText("Ant 1");
        jRadioButtonAnt1.addChangeListener(new javax.swing.event.ChangeListener() {
            public void stateChanged(javax.swing.event.ChangeEvent evt) {
                jRadioButtonAnt1StateChanged(evt);
            }
        });
        jRadioButtonAnt1.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonAnt1ActionPerformed(evt);
            }
        });

        buttonGroupTXAnt.add(jRadioButtonAnt2);
        jRadioButtonAnt2.setText("Ant 2");
        jRadioButtonAnt2.addChangeListener(new javax.swing.event.ChangeListener() {
            public void stateChanged(javax.swing.event.ChangeEvent evt) {
                jRadioButtonAnt2StateChanged(evt);
            }
        });
        jRadioButtonAnt2.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonAnt2ActionPerformed(evt);
            }
        });

        buttonGroupTXAnt.add(jRadioButtonAnt3);
        jRadioButtonAnt3.setText("Ant 3");
        jRadioButtonAnt3.addChangeListener(new javax.swing.event.ChangeListener() {
            public void stateChanged(javax.swing.event.ChangeEvent evt) {
                jRadioButtonAnt3StateChanged(evt);
            }
        });
        jRadioButtonAnt3.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonAnt3ActionPerformed(evt);
            }
        });

        buttonGroupRXAnt.add(jRadioButtonNone);
        jRadioButtonNone.setText("None (Use Tx)");
        jRadioButtonNone.addChangeListener(new javax.swing.event.ChangeListener() {
            public void stateChanged(javax.swing.event.ChangeEvent evt) {
                jRadioButtonNoneStateChanged(evt);
            }
        });
        jRadioButtonNone.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonNoneActionPerformed(evt);
            }
        });

        buttonGroupRXAnt.add(jRadioButtonRX1);
        jRadioButtonRX1.setText("RX 1");
        jRadioButtonRX1.addChangeListener(new javax.swing.event.ChangeListener() {
            public void stateChanged(javax.swing.event.ChangeEvent evt) {
                jRadioButtonRX1StateChanged(evt);
            }
        });
        jRadioButtonRX1.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonRX1ActionPerformed(evt);
            }
        });

        buttonGroupRXAnt.add(jRadioButtonRX2);
        jRadioButtonRX2.setText("RX 2");
        jRadioButtonRX2.addChangeListener(new javax.swing.event.ChangeListener() {
            public void stateChanged(javax.swing.event.ChangeEvent evt) {
                jRadioButtonRX2StateChanged(evt);
            }
        });
        jRadioButtonRX2.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonRX2ActionPerformed(evt);
            }
        });

        buttonGroupRXAnt.add(jRadioButtonRXXV);
        jRadioButtonRXXV.setText("RX  XV");
        jRadioButtonRXXV.addChangeListener(new javax.swing.event.ChangeListener() {
            public void stateChanged(javax.swing.event.ChangeEvent evt) {
                jRadioButtonRXXVStateChanged(evt);
            }
        });
        jRadioButtonRXXV.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonRXXVActionPerformed(evt);
            }
        });

        jLabel4.setText("RX Ant:");

        jLabel5.setText("TX Ant:");

        javax.swing.GroupLayout jPanel3Layout = new javax.swing.GroupLayout(jPanel3);
        jPanel3.setLayout(jPanel3Layout);
        jPanel3Layout.setHorizontalGroup(
            jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel3Layout.createSequentialGroup()
                .addGap(29, 29, 29)
                .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING, false)
                    .addGroup(jPanel3Layout.createSequentialGroup()
                        .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jRadioButtonAnt2)
                            .addComponent(jRadioButtonAnt3))
                        .addGap(101, 101, 101)
                        .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jRadioButtonRX2)
                            .addComponent(jRadioButtonRX1)
                            .addComponent(jRadioButtonRXXV))
                        .addGap(0, 0, Short.MAX_VALUE))
                    .addGroup(jPanel3Layout.createSequentialGroup()
                        .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING, false)
                            .addGroup(jPanel3Layout.createSequentialGroup()
                                .addComponent(jLabel5)
                                .addGap(114, 114, 114))
                            .addGroup(javax.swing.GroupLayout.Alignment.LEADING, jPanel3Layout.createSequentialGroup()
                                .addComponent(jRadioButtonAnt1)
                                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)))
                        .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jLabel4)
                            .addComponent(jRadioButtonNone))))
                .addContainerGap(138, Short.MAX_VALUE))
        );
        jPanel3Layout.setVerticalGroup(
            jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel3Layout.createSequentialGroup()
                .addGap(26, 26, 26)
                .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jLabel4)
                    .addComponent(jLabel5))
                .addGap(18, 18, 18)
                .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jRadioButtonAnt1)
                    .addComponent(jRadioButtonNone))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jRadioButtonAnt2)
                    .addComponent(jRadioButtonRX1))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addGroup(jPanel3Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jRadioButtonAnt3)
                    .addComponent(jRadioButtonRX2))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addComponent(jRadioButtonRXXV)
                .addContainerGap(64, Short.MAX_VALUE))
        );

        jTabbedPane1.addTab("Antenna", jPanel3);

        jLabel1.setText("Gain Method:");

        buttonGroupGain.add(jRadioButtonLinear);
        jRadioButtonLinear.setText("Linear");
        jRadioButtonLinear.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonLinearActionPerformed(evt);
            }
        });

        buttonGroupGain.add(jRadioButtonLog);
        jRadioButtonLog.setText("Log");
        jRadioButtonLog.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonLogActionPerformed(evt);
            }
        });

        jLabel2.setText("NPE Method:");

        buttonGroupNPEMethod.add(jRadioButtonOSMS);
        jRadioButtonOSMS.setText("OSMS");
        jRadioButtonOSMS.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonOSMSActionPerformed(evt);
            }
        });

        buttonGroupNPEMethod.add(jRadioButtonMMSE);
        jRadioButtonMMSE.setText("MMSE");
        jRadioButtonMMSE.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonMMSEActionPerformed(evt);
            }
        });

        jLabel3.setText("Position:");

        buttonGroupPosition.add(jRadioButtonPreAGC);
        jRadioButtonPreAGC.setText("Pre AGC");
        jRadioButtonPreAGC.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonPreAGCActionPerformed(evt);
            }
        });

        buttonGroupPosition.add(jRadioButtonPostAGC);
        jRadioButtonPostAGC.setText("Post AGC");
        jRadioButtonPostAGC.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonPostAGCActionPerformed(evt);
            }
        });

        jCheckBoxAEFilter.setText("AE Filter");
        jCheckBoxAEFilter.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxAEFilterActionPerformed(evt);
            }
        });

        javax.swing.GroupLayout jPanel1Layout = new javax.swing.GroupLayout(jPanel1);
        jPanel1.setLayout(jPanel1Layout);
        jPanel1Layout.setHorizontalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addGap(24, 24, 24)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(jPanel1Layout.createSequentialGroup()
                        .addComponent(jCheckBoxAEFilter)
                        .addGap(0, 0, Short.MAX_VALUE))
                    .addGroup(jPanel1Layout.createSequentialGroup()
                        .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addGroup(jPanel1Layout.createSequentialGroup()
                                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                                    .addComponent(jRadioButtonLinear)
                                    .addComponent(jRadioButtonLog))
                                .addGap(53, 53, 53)
                                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                                    .addComponent(jRadioButtonMMSE)
                                    .addGroup(jPanel1Layout.createSequentialGroup()
                                        .addComponent(jRadioButtonOSMS)
                                        .addGap(165, 165, 165))))
                            .addGroup(jPanel1Layout.createSequentialGroup()
                                .addComponent(jLabel1)
                                .addGap(30, 30, 30)
                                .addComponent(jLabel2)
                                .addGap(18, 18, 18)
                                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                                    .addComponent(jRadioButtonPreAGC)
                                    .addComponent(jLabel3)
                                    .addComponent(jRadioButtonPostAGC))))
                        .addContainerGap(74, Short.MAX_VALUE))))
        );
        jPanel1Layout.setVerticalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addGap(28, 28, 28)
                .addComponent(jCheckBoxAEFilter)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jLabel1, javax.swing.GroupLayout.PREFERRED_SIZE, 17, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                        .addComponent(jLabel3)
                        .addComponent(jLabel2)))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jRadioButtonOSMS)
                    .addComponent(jRadioButtonPreAGC)
                    .addComponent(jRadioButtonLinear))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jRadioButtonMMSE)
                    .addComponent(jRadioButtonPostAGC)
                    .addComponent(jRadioButtonLog))
                .addContainerGap(122, Short.MAX_VALUE))
        );

        jTabbedPane1.addTab("NB2", jPanel1);

        jLabelPASettings.setText("Band:");

        jSliderPASettings.addChangeListener(new javax.swing.event.ChangeListener() {
            public void stateChanged(javax.swing.event.ChangeEvent evt) {
                jSliderPASettingsStateChanged(evt);
            }
        });

        javax.swing.GroupLayout jPanel2Layout = new javax.swing.GroupLayout(jPanel2);
        jPanel2.setLayout(jPanel2Layout);
        jPanel2Layout.setHorizontalGroup(
            jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel2Layout.createSequentialGroup()
                .addContainerGap()
                .addComponent(jLabelPASettings, javax.swing.GroupLayout.PREFERRED_SIZE, 78, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(40, 40, 40)
                .addComponent(jSliderPASettings, javax.swing.GroupLayout.DEFAULT_SIZE, 310, Short.MAX_VALUE)
                .addContainerGap())
        );
        jPanel2Layout.setVerticalGroup(
            jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel2Layout.createSequentialGroup()
                .addGap(30, 30, 30)
                .addGroup(jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jSliderPASettings, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jLabelPASettings))
                .addContainerGap(173, Short.MAX_VALUE))
        );

        jTabbedPane1.addTab("PA Settings", jPanel2);

        jLabelOCBand.setText("Band:");

        jLabel7.setText("RX Pins");

        jLabel8.setText("TX Pins");

        jCheckBoxOCRx1.setText("1");
        jCheckBoxOCRx1.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxOCRx1ActionPerformed(evt);
            }
        });

        jCheckBoxOCRx2.setText("2");
        jCheckBoxOCRx2.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxOCRx2ActionPerformed(evt);
            }
        });

        jCheckBoxOCRx3.setText("3");
        jCheckBoxOCRx3.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxOCRx3ActionPerformed(evt);
            }
        });

        jCheckBoxOCRx4.setText("4");
        jCheckBoxOCRx4.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxOCRx4ActionPerformed(evt);
            }
        });

        jCheckBoxOCRx5.setText("5");
        jCheckBoxOCRx5.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxOCRx5ActionPerformed(evt);
            }
        });

        jCheckBoxOCRx6.setText("6");
        jCheckBoxOCRx6.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxOCRx6ActionPerformed(evt);
            }
        });

        jCheckBoxOCRx7.setText("7");
        jCheckBoxOCRx7.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxOCRx7ActionPerformed(evt);
            }
        });

        jCheckBoxOCTx1.setText("1");
        jCheckBoxOCTx1.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxOCTx1ActionPerformed(evt);
            }
        });

        jCheckBoxOCTx2.setText("2");
        jCheckBoxOCTx2.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxOCTx2ActionPerformed(evt);
            }
        });

        jCheckBoxOCTx3.setText("3");
        jCheckBoxOCTx3.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxOCTx3ActionPerformed(evt);
            }
        });

        jCheckBoxOCTx4.setText("4");
        jCheckBoxOCTx4.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxOCTx4ActionPerformed(evt);
            }
        });

        jCheckBoxOCTx5.setText("5");
        jCheckBoxOCTx5.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxOCTx5ActionPerformed(evt);
            }
        });

        jCheckBoxOCTx6.setText("6");
        jCheckBoxOCTx6.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxOCTx6ActionPerformed(evt);
            }
        });

        jCheckBoxOCTx7.setText("7");
        jCheckBoxOCTx7.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxOCTx7ActionPerformed(evt);
            }
        });

        javax.swing.GroupLayout jPanel4Layout = new javax.swing.GroupLayout(jPanel4);
        jPanel4.setLayout(jPanel4Layout);
        jPanel4Layout.setHorizontalGroup(
            jPanel4Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel4Layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(jPanel4Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING)
                    .addComponent(jLabel7)
                    .addGroup(jPanel4Layout.createSequentialGroup()
                        .addComponent(jLabelOCBand, javax.swing.GroupLayout.PREFERRED_SIZE, 67, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addGap(36, 36, 36)
                        .addGroup(jPanel4Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jCheckBoxOCRx2)
                            .addComponent(jCheckBoxOCRx1)
                            .addComponent(jCheckBoxOCRx3)
                            .addComponent(jCheckBoxOCRx4)
                            .addComponent(jCheckBoxOCRx5)
                            .addComponent(jCheckBoxOCRx6)
                            .addComponent(jCheckBoxOCRx7))))
                .addGap(121, 121, 121)
                .addGroup(jPanel4Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jLabel8)
                    .addGroup(jPanel4Layout.createSequentialGroup()
                        .addGap(12, 12, 12)
                        .addGroup(jPanel4Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jCheckBoxOCTx2)
                            .addComponent(jCheckBoxOCTx1)
                            .addComponent(jCheckBoxOCTx3)
                            .addComponent(jCheckBoxOCTx4)
                            .addComponent(jCheckBoxOCTx5)
                            .addComponent(jCheckBoxOCTx6)
                            .addComponent(jCheckBoxOCTx7))))
                .addContainerGap(134, Short.MAX_VALUE))
        );
        jPanel4Layout.setVerticalGroup(
            jPanel4Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel4Layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(jPanel4Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jLabel7)
                    .addComponent(jLabel8))
                .addGroup(jPanel4Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(jPanel4Layout.createSequentialGroup()
                        .addGap(1, 1, 1)
                        .addComponent(jLabelOCBand))
                    .addGroup(jPanel4Layout.createSequentialGroup()
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addGroup(jPanel4Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                            .addComponent(jCheckBoxOCRx1)
                            .addComponent(jCheckBoxOCTx1))))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel4Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jCheckBoxOCRx2)
                    .addComponent(jCheckBoxOCTx2, javax.swing.GroupLayout.Alignment.TRAILING))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addGroup(jPanel4Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jCheckBoxOCRx3)
                    .addComponent(jCheckBoxOCTx3))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel4Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jCheckBoxOCRx4)
                    .addComponent(jCheckBoxOCTx4))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel4Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jCheckBoxOCRx5)
                    .addComponent(jCheckBoxOCTx5))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel4Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jCheckBoxOCRx6)
                    .addComponent(jCheckBoxOCTx6))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel4Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jCheckBoxOCRx7)
                    .addComponent(jCheckBoxOCTx7))
                .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );

        jTabbedPane1.addTab("OC Outputs", jPanel4);

        jCheckBox20dBMicBoost.setText("20dB Mic Boost");
        jCheckBox20dBMicBoost.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBox20dBMicBoostActionPerformed(evt);
            }
        });

        javax.swing.GroupLayout jPanel5Layout = new javax.swing.GroupLayout(jPanel5);
        jPanel5.setLayout(jPanel5Layout);
        jPanel5Layout.setHorizontalGroup(
            jPanel5Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel5Layout.createSequentialGroup()
                .addContainerGap()
                .addComponent(jCheckBox20dBMicBoost)
                .addContainerGap(308, Short.MAX_VALUE))
        );
        jPanel5Layout.setVerticalGroup(
            jPanel5Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel5Layout.createSequentialGroup()
                .addGap(21, 21, 21)
                .addComponent(jCheckBox20dBMicBoost)
                .addContainerGap(212, Short.MAX_VALUE))
        );

        jTabbedPane1.addTab("Audio", jPanel5);

        jButtonClose.setText("Close");
        jButtonClose.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonCloseActionPerformed(evt);
            }
        });

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jTabbedPane1)
            .addGroup(layout.createSequentialGroup()
                .addGap(174, 174, 174)
                .addComponent(jButtonClose)
                .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addComponent(jTabbedPane1, javax.swing.GroupLayout.PREFERRED_SIZE, 292, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(18, 18, 18)
                .addComponent(jButtonClose)
                .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void jButtonCloseActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonCloseActionPerformed
        this.dispose();
    }//GEN-LAST:event_jButtonCloseActionPerformed

    private void jRadioButtonOSMSActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonOSMSActionPerformed
        configuration.NB2_NPE = 0;
        wdsp.SetRXAEMNRnpeMethod(Channel.RX, 0);
    }//GEN-LAST:event_jRadioButtonOSMSActionPerformed

    private void jCheckBoxAEFilterActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxAEFilterActionPerformed
        configuration.NB2_AE = this.jCheckBoxAEFilter.isSelected();
        wdsp.SetRXAEMNRaeRun(Channel.RX, configuration.NB2_AE ? 1 : 0);
    }//GEN-LAST:event_jCheckBoxAEFilterActionPerformed

    private void jRadioButtonLinearActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonLinearActionPerformed
        configuration.NB2_GAIN = 0;
        wdsp.SetRXAEMNRgainMethod(Channel.RX, 0);
    }//GEN-LAST:event_jRadioButtonLinearActionPerformed

    private void jRadioButtonLogActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonLogActionPerformed
        configuration.NB2_GAIN = 1;
        wdsp.SetRXAEMNRgainMethod(Channel.RX, 1);
    }//GEN-LAST:event_jRadioButtonLogActionPerformed

    private void jRadioButtonMMSEActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonMMSEActionPerformed
        configuration.NB2_NPE = 1;
        wdsp.SetRXAEMNRnpeMethod(Channel.RX, 1);
    }//GEN-LAST:event_jRadioButtonMMSEActionPerformed

    private void jRadioButtonPreAGCActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonPreAGCActionPerformed
        configuration.NB2_POSITION = 0;
        wdsp.SetRXAEMNRPosition(Channel.RX, 0);
    }//GEN-LAST:event_jRadioButtonPreAGCActionPerformed

    private void jRadioButtonPostAGCActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonPostAGCActionPerformed
        configuration.NB2_POSITION = 1;
        wdsp.SetRXAEMNRPosition(Channel.RX, 1);
    }//GEN-LAST:event_jRadioButtonPostAGCActionPerformed

    private void jRadioButtonAnt2ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonAnt2ActionPerformed
        configuration.bands.get().get().setTxAntenna(1);
    }//GEN-LAST:event_jRadioButtonAnt2ActionPerformed

    private void jRadioButtonAnt1StateChanged(javax.swing.event.ChangeEvent evt) {//GEN-FIRST:event_jRadioButtonAnt1StateChanged
        // TODO add your handling code here:
    }//GEN-LAST:event_jRadioButtonAnt1StateChanged

    private void jRadioButtonAnt2StateChanged(javax.swing.event.ChangeEvent evt) {//GEN-FIRST:event_jRadioButtonAnt2StateChanged
        // TODO add your handling code here:
    }//GEN-LAST:event_jRadioButtonAnt2StateChanged

    private void jRadioButtonAnt3StateChanged(javax.swing.event.ChangeEvent evt) {//GEN-FIRST:event_jRadioButtonAnt3StateChanged
        // TODO add your handling code here:
    }//GEN-LAST:event_jRadioButtonAnt3StateChanged

    private void jRadioButtonNoneStateChanged(javax.swing.event.ChangeEvent evt) {//GEN-FIRST:event_jRadioButtonNoneStateChanged
        // TODO add your handling code here:
    }//GEN-LAST:event_jRadioButtonNoneStateChanged

    private void jRadioButtonRX1StateChanged(javax.swing.event.ChangeEvent evt) {//GEN-FIRST:event_jRadioButtonRX1StateChanged
        // TODO add your handling code here:
    }//GEN-LAST:event_jRadioButtonRX1StateChanged

    private void jRadioButtonRX2StateChanged(javax.swing.event.ChangeEvent evt) {//GEN-FIRST:event_jRadioButtonRX2StateChanged
        // TODO add your handling code here:
    }//GEN-LAST:event_jRadioButtonRX2StateChanged

    private void jRadioButtonRXXVStateChanged(javax.swing.event.ChangeEvent evt) {//GEN-FIRST:event_jRadioButtonRXXVStateChanged
        // TODO add your handling code here:
    }//GEN-LAST:event_jRadioButtonRXXVStateChanged

    private void jRadioButtonAnt1ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonAnt1ActionPerformed
        configuration.bands.get().get().setTxAntenna(0);
    }//GEN-LAST:event_jRadioButtonAnt1ActionPerformed

    private void jRadioButtonAnt3ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonAnt3ActionPerformed
        configuration.bands.get().get().setTxAntenna(1);
    }//GEN-LAST:event_jRadioButtonAnt3ActionPerformed

    private void jRadioButtonNoneActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonNoneActionPerformed
        // TODO add your handling code here:
    }//GEN-LAST:event_jRadioButtonNoneActionPerformed

    private void jRadioButtonRX1ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonRX1ActionPerformed
        // TODO add your handling code here:
    }//GEN-LAST:event_jRadioButtonRX1ActionPerformed

    private void jRadioButtonRX2ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonRX2ActionPerformed
        // TODO add your handling code here:
    }//GEN-LAST:event_jRadioButtonRX2ActionPerformed

    private void jRadioButtonRXXVActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonRXXVActionPerformed
        // TODO add your handling code here:
    }//GEN-LAST:event_jRadioButtonRXXVActionPerformed

    private void jSliderPASettingsStateChanged(javax.swing.event.ChangeEvent evt) {//GEN-FIRST:event_jSliderPASettingsStateChanged
        int value=this.jSliderPASettings.getValue();
        configuration.bands.get().setDrive((float)value/100.0F);
    }//GEN-LAST:event_jSliderPASettingsStateChanged

    private void jCheckBoxOCRx1ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCRx1ActionPerformed
        Band band=configuration.bands.get();
        if(jCheckBoxOCRx1.isSelected()) {
            band.setOCRx((byte)(band.getOCRx()|0x01));
        } else {
            band.setOCRx((byte)(band.getOCRx()&0xFE));
        }
    }//GEN-LAST:event_jCheckBoxOCRx1ActionPerformed

    private void jCheckBoxOCRx2ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCRx2ActionPerformed
        Band band=configuration.bands.get();
        if(jCheckBoxOCRx2.isSelected()) {
            band.setOCRx((byte)(band.getOCRx()|0x02));
        } else {
            band.setOCRx((byte)(band.getOCRx()&0xFD));
        }
    }//GEN-LAST:event_jCheckBoxOCRx2ActionPerformed

    private void jCheckBoxOCRx3ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCRx3ActionPerformed
        Band band=configuration.bands.get();
        if(jCheckBoxOCRx3.isSelected()) {
            band.setOCRx((byte)(band.getOCRx()|0x04));
        } else {
            band.setOCRx((byte)(band.getOCRx()&0xFB));
        }
    }//GEN-LAST:event_jCheckBoxOCRx3ActionPerformed

    private void jCheckBoxOCRx4ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCRx4ActionPerformed
        Band band=configuration.bands.get();
        if(jCheckBoxOCRx4.isSelected()) {
            band.setOCRx((byte)(band.getOCRx()|0x08));
        } else {
            band.setOCRx((byte)(band.getOCRx()&0xF7));
        }
    }//GEN-LAST:event_jCheckBoxOCRx4ActionPerformed

    private void jCheckBoxOCRx5ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCRx5ActionPerformed
        Band band=configuration.bands.get();
        if(jCheckBoxOCRx5.isSelected()) {
            band.setOCRx((byte)(band.getOCRx()|0x10));
        } else {
            band.setOCRx((byte)(band.getOCRx()&0xEF));
        }
    }//GEN-LAST:event_jCheckBoxOCRx5ActionPerformed

    private void jCheckBoxOCRx6ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCRx6ActionPerformed
        Band band=configuration.bands.get();
        if(jCheckBoxOCRx6.isSelected()) {
            band.setOCRx((byte)(band.getOCRx()|0x20));
        } else {
            band.setOCRx((byte)(band.getOCRx()&0xDF));
        }
    }//GEN-LAST:event_jCheckBoxOCRx6ActionPerformed

    private void jCheckBoxOCRx7ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCRx7ActionPerformed
        Band band=configuration.bands.get();
        if(jCheckBoxOCRx7.isSelected()) {
            band.setOCRx((byte)(band.getOCRx()|0x40));
        } else {
            band.setOCRx((byte)(band.getOCRx()&0xBF));
        }
    }//GEN-LAST:event_jCheckBoxOCRx7ActionPerformed

    private void jCheckBoxOCTx1ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCTx1ActionPerformed
        Band band=configuration.bands.get();
        if(jCheckBoxOCTx1.isSelected()) {
            band.setOCTx((byte)(band.getOCTx()|0x01));
        } else {
            band.setOCTx((byte)(band.getOCTx()&0xFE));
        }
    }//GEN-LAST:event_jCheckBoxOCTx1ActionPerformed

    private void jCheckBoxOCTx2ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCTx2ActionPerformed
        Band band=configuration.bands.get();
        if(jCheckBoxOCTx2.isSelected()) {
            band.setOCTx((byte)(band.getOCTx()|0x02));
        } else {
            band.setOCTx((byte)(band.getOCTx()&0xFD));
        }
    }//GEN-LAST:event_jCheckBoxOCTx2ActionPerformed

    private void jCheckBoxOCTx3ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCTx3ActionPerformed
        Band band=configuration.bands.get();
        if(jCheckBoxOCTx3.isSelected()) {
            band.setOCTx((byte)(band.getOCTx()|0x04));
        } else {
            band.setOCTx((byte)(band.getOCTx()&0xFB));
        }
    }//GEN-LAST:event_jCheckBoxOCTx3ActionPerformed

    private void jCheckBoxOCTx4ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCTx4ActionPerformed
        Band band=configuration.bands.get();
        if(jCheckBoxOCTx4.isSelected()) {
            band.setOCTx((byte)(band.getOCTx()|0x08));
        } else {
            band.setOCTx((byte)(band.getOCTx()&0xF7));
        }
    }//GEN-LAST:event_jCheckBoxOCTx4ActionPerformed

    private void jCheckBoxOCTx5ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCTx5ActionPerformed
        Band band=configuration.bands.get();
        if(jCheckBoxOCTx5.isSelected()) {
            band.setOCTx((byte)(band.getOCTx()|0x10));
        } else {
            band.setOCTx((byte)(band.getOCTx()&0xEF));
        }
    }//GEN-LAST:event_jCheckBoxOCTx5ActionPerformed

    private void jCheckBoxOCTx6ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCTx6ActionPerformed
        Band band=configuration.bands.get();
        if(jCheckBoxOCTx6.isSelected()) {
            band.setOCTx((byte)(band.getOCTx()|0x20));
        } else {
            band.setOCTx((byte)(band.getOCTx()&0xDF));
        }
    }//GEN-LAST:event_jCheckBoxOCTx6ActionPerformed

    private void jCheckBoxOCTx7ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCTx7ActionPerformed
        Band band=configuration.bands.get();
        if(jCheckBoxOCTx7.isSelected()) {
            band.setOCTx((byte)(band.getOCTx()|0x40));
        } else {
            band.setOCTx((byte)(band.getOCTx()&0xBF));
        }
    }//GEN-LAST:event_jCheckBoxOCTx7ActionPerformed

    private void jCheckBox20dBMicBoostActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBox20dBMicBoostActionPerformed
        configuration.micboost=this.jCheckBox20dBMicBoost.isSelected();
    }//GEN-LAST:event_jCheckBox20dBMicBoostActionPerformed

    private Configuration configuration;
    private WDSP wdsp;

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.ButtonGroup buttonGroupGain;
    private javax.swing.ButtonGroup buttonGroupNPEMethod;
    private javax.swing.ButtonGroup buttonGroupPosition;
    private javax.swing.ButtonGroup buttonGroupRXAnt;
    private javax.swing.ButtonGroup buttonGroupTXAnt;
    private javax.swing.JButton jButtonClose;
    private javax.swing.JCheckBox jCheckBox20dBMicBoost;
    private javax.swing.JCheckBox jCheckBoxAEFilter;
    private javax.swing.JCheckBox jCheckBoxOCRx1;
    private javax.swing.JCheckBox jCheckBoxOCRx2;
    private javax.swing.JCheckBox jCheckBoxOCRx3;
    private javax.swing.JCheckBox jCheckBoxOCRx4;
    private javax.swing.JCheckBox jCheckBoxOCRx5;
    private javax.swing.JCheckBox jCheckBoxOCRx6;
    private javax.swing.JCheckBox jCheckBoxOCRx7;
    private javax.swing.JCheckBox jCheckBoxOCTx1;
    private javax.swing.JCheckBox jCheckBoxOCTx2;
    private javax.swing.JCheckBox jCheckBoxOCTx3;
    private javax.swing.JCheckBox jCheckBoxOCTx4;
    private javax.swing.JCheckBox jCheckBoxOCTx5;
    private javax.swing.JCheckBox jCheckBoxOCTx6;
    private javax.swing.JCheckBox jCheckBoxOCTx7;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel2;
    private javax.swing.JLabel jLabel3;
    private javax.swing.JLabel jLabel4;
    private javax.swing.JLabel jLabel5;
    private javax.swing.JLabel jLabel7;
    private javax.swing.JLabel jLabel8;
    private javax.swing.JLabel jLabelOCBand;
    private javax.swing.JLabel jLabelPASettings;
    private javax.swing.JPanel jPanel1;
    private javax.swing.JPanel jPanel2;
    private javax.swing.JPanel jPanel3;
    private javax.swing.JPanel jPanel4;
    private javax.swing.JPanel jPanel5;
    private javax.swing.JRadioButton jRadioButtonAnt1;
    private javax.swing.JRadioButton jRadioButtonAnt2;
    private javax.swing.JRadioButton jRadioButtonAnt3;
    private javax.swing.JRadioButton jRadioButtonLinear;
    private javax.swing.JRadioButton jRadioButtonLog;
    private javax.swing.JRadioButton jRadioButtonMMSE;
    private javax.swing.JRadioButton jRadioButtonNone;
    private javax.swing.JRadioButton jRadioButtonOSMS;
    private javax.swing.JRadioButton jRadioButtonPostAGC;
    private javax.swing.JRadioButton jRadioButtonPreAGC;
    private javax.swing.JRadioButton jRadioButtonRX1;
    private javax.swing.JRadioButton jRadioButtonRX2;
    private javax.swing.JRadioButton jRadioButtonRXXV;
    private javax.swing.JSlider jSliderPASettings;
    private javax.swing.JTabbedPane jTabbedPane1;
    // End of variables declaration//GEN-END:variables
}
