/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.g0orx.openhpsdr;

import javax.swing.DefaultListModel;
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
    public RadioJDialog(java.awt.Frame parent, boolean modal, WDSP wdsp) {
        super(parent, modal);
        initComponents();
        configuration = Configuration.getInstance();
        this.wdsp = wdsp;
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

        this.jLabelPASettings.setText("Band: " + band.getName());
        this.jSliderPASettings.setValue((int) (band.getDrive() * 100.0));

        this.jLabelOCBand.setText("Band: " + band.getName());
        this.jCheckBoxOCRx1.setSelected((band.getOCRx() & 0x01) == 0x01);
        this.jCheckBoxOCRx2.setSelected((band.getOCRx() & 0x02) == 0x02);
        this.jCheckBoxOCRx3.setSelected((band.getOCRx() & 0x04) == 0x04);
        this.jCheckBoxOCRx4.setSelected((band.getOCRx() & 0x08) == 0x08);
        this.jCheckBoxOCRx5.setSelected((band.getOCRx() & 0x10) == 0x10);
        this.jCheckBoxOCRx6.setSelected((band.getOCRx() & 0x20) == 0x20);
        this.jCheckBoxOCRx7.setSelected((band.getOCRx() & 0x40) == 0x40);
        this.jCheckBoxOCTx1.setSelected((band.getOCTx() & 0x01) == 0x01);
        this.jCheckBoxOCTx2.setSelected((band.getOCTx() & 0x02) == 0x02);
        this.jCheckBoxOCTx3.setSelected((band.getOCTx() & 0x04) == 0x04);
        this.jCheckBoxOCTx4.setSelected((band.getOCTx() & 0x08) == 0x08);
        this.jCheckBoxOCTx5.setSelected((band.getOCTx() & 0x10) == 0x10);
        this.jCheckBoxOCTx6.setSelected((band.getOCTx() & 0x20) == 0x20);
        this.jCheckBoxOCTx7.setSelected((band.getOCTx() & 0x40) == 0x40);

        this.jCheckBox20dBMicBoost.setSelected(configuration.micboost);

        if (configuration.discovered.getDevice() == Discovered.DEVICE_ORION) {

            this.jCheckBoxPTTEnabled.setSelected(configuration.orionmicptt == Configuration.MIC_PTT_ENABLE);
            this.jCheckBoxBiasEnabled.setSelected(configuration.orionmicbias == Configuration.MIC_BIAS_ENABLE);

            if (this.configuration.oriontipring == Configuration.MIC_PTT_TO_RING_BIAS_TO_TIP) {
                this.jRadioButtonPTTtoRing.setSelected(true);
            }
            if (this.configuration.oriontipring == Configuration.MIC_PTT_TO_TIP_BIAS_TO_RING) {
                this.jRadioButtonPTTtoTip.setSelected(true);
            }

        } else {
            this.jCheckBoxPTTEnabled.setEnabled(false);
            this.jCheckBoxBiasEnabled.setEnabled(false);
            this.jRadioButtonPTTtoRing.setEnabled(false);
            this.jRadioButtonPTTtoTip.setEnabled(false);
        }

        switch ((int) configuration.samplerate) {
            case 48000:
                this.jRadioButtonRate48K.setSelected(true);
                break;
            case 96000:
                this.jRadioButtonRate96K.setSelected(true);
                break;
            case 192000:
                this.jRadioButtonRate192K.setSelected(true);
                break;
            case 384000:
                this.jRadioButtonRate384K.setSelected(true);
                break;
        }
        switch ((int) configuration.buffersize) {
            case 64:
                this.jRadioButtonBuffer64.setSelected(true);
                break;
            case 128:
                this.jRadioButtonBuffer128.setSelected(true);
                break;
            case 256:
                this.jRadioButtonBuffer256.setSelected(true);
                break;
            case 512:
                this.jRadioButtonBuffer512.setSelected(true);
                break;
            case 1024:
                this.jRadioButtonBuffer1024.setSelected(true);
                break;
            case 2048:
                this.jRadioButtonBuffer2048.setSelected(true);
                break;
        }
        switch (configuration.fftsize) {
            case 256:
                this.jRadioButton256.setSelected(true);
                break;
            case 512:
                this.jRadioButton512.setSelected(true);
                break;
            case 1024:
                this.jRadioButton1024.setSelected(true);
                break;
            case 2048:
                this.jRadioButton2048.setSelected(true);
                break;
            case 4096:
                this.jRadioButton4096.setSelected(true);
                break;
            case 8192:
                this.jRadioButton8192.setSelected(true);
                break;
            case 16384:
                this.jRadioButton16384.setSelected(true);
                break;
        }

        DefaultListModel listModel = new DefaultListModel();
        jListRadio.setModel(listModel);
        switch (configuration.discovered.getDevice()) {
            case Discovered.DEVICE_METIS:
                listModel.addElement("Metis, Mercury, Penelope");
                listModel.addElement("Metis, Mercury, Pennylane");
                listModel.addElement("Metis, Mercury, Penelope, Alex");
                listModel.addElement("Metis, Mercury, Pennylane, Alex");
                switch (configuration.radio) {
                    case Configuration.RADIO_UNKNOWN:
                        configuration.radio = Configuration.METIS_PENELOPE;
                        jListRadio.setSelectedIndex(0);
                        break;
                    case Configuration.METIS_PENELOPE:
                        jListRadio.setSelectedIndex(0);
                        break;
                    case Configuration.METIS_PENNYLANE:
                        jListRadio.setSelectedIndex(1);
                        break;
                    case Configuration.METIS_PENELOPE_ALEX:
                        jListRadio.setSelectedIndex(2);
                        break;
                    case Configuration.METIS_PENNYLANE_ALEX:
                        jListRadio.setSelectedIndex(3);
                        break;
                }
                break;
            case Discovered.DEVICE_HERMES:
                listModel.addElement("Hermes");
                listModel.addElement("Hermes, Alex");
                listModel.addElement("Hermes, Apollo");
                listModel.addElement("ANAN-10");
                listModel.addElement("ANAN-100");
                switch (configuration.radio) {
                    case Configuration.RADIO_UNKNOWN:
                        configuration.radio = Configuration.HERMES_ANAN100;
                        jListRadio.setSelectedIndex(4);
                        break;
                    case Configuration.HERMES_BOARD_ONLY:
                        jListRadio.setSelectedIndex(0);
                        break;
                    case Configuration.HERMES_ALEX:
                        jListRadio.setSelectedIndex(1);
                        break;
                    case Configuration.HERMES_APOLLO:
                        jListRadio.setSelectedIndex(2);
                        break;
                    case Configuration.HERMES_ANAN10:
                        jListRadio.setSelectedIndex(3);
                        break;
                    case Configuration.HERMES_ANAN100:
                        jListRadio.setSelectedIndex(4);
                        break;
                }
                break;
            case Discovered.DEVICE_ANGELIA:
                listModel.addElement("Angelia");
                listModel.addElement("ANAN-100D");
                switch (configuration.radio) {
                    case Configuration.RADIO_UNKNOWN:
                        configuration.radio = Configuration.ANGELIA_ANAN100D;
                        jListRadio.setSelectedIndex(1);
                        break;
                    case Configuration.ANGELIA_BOARD_ONLY:
                        jListRadio.setSelectedIndex(0);
                        break;
                    case Configuration.ANGELIA_ANAN100D:
                        jListRadio.setSelectedIndex(1);
                        break;
                }
                break;
            case Discovered.DEVICE_ORION:
                listModel.addElement("Orion");
                listModel.addElement("ANAN-200D");
                switch (configuration.radio) {
                    case Configuration.RADIO_UNKNOWN:
                        configuration.radio = Configuration.ORION_ANAN200D;
                        jListRadio.setSelectedIndex(1);
                        break;
                    case Configuration.ORION_BOARD_ONLY:
                        jListRadio.setSelectedIndex(0);
                        break;
                    case Configuration.ORION_ANAN200D:
                        jListRadio.setSelectedIndex(1);
                        break;
                }
                break;
            case Discovered.DEVICE_HERMES_LITE:
                listModel.addElement("Hermes Lite");
                configuration.radio = Configuration.HERMES_LITE_ONLY;
                jListRadio.setSelectedIndex(0);
                break;
        }
        if (wdsp != null) {
            this.jTabbedPane1.setEnabledAt(0, false);
            this.jTabbedPane1.setEnabledAt(1, false);
            this.jTabbedPane1.setSelectedIndex(2);
        }

        switch (configuration.audiooutput) {
            case Configuration.AUDIO_OUTPUT_RADIO:
                this.jRadioButtonAudioOutputRadio.setSelected(true);
                break;
            case Configuration.AUDIO_OUTPUT_LOCAL:
                this.jRadioButtonAudioOutputLocal.setSelected(true);
                break;
            case Configuration.AUDIO_OUTPUT_BOTH:
                this.jRadioButtonAudioOutputBoth.setSelected(true);
                break;
        }

        switch (configuration.micsource) {
            case Configuration.MIC_SOURCE_RADIO:
                this.jRadioButtonMicInputRadio.setSelected(true);
                break;
            case Configuration.MIC_SOURCE_LOCAL:
                this.jRadioButtonMicInputLocal.setSelected(true);
                break;
        }
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
        buttonGroupTipRing = new javax.swing.ButtonGroup();
        buttonGroupAudioOutput = new javax.swing.ButtonGroup();
        buttonGroupMicSource = new javax.swing.ButtonGroup();
        jTabbedPane1 = new javax.swing.JTabbedPane();
        jPanel7 = new javax.swing.JPanel();
        jScrollPane1 = new javax.swing.JScrollPane();
        jListRadio = new javax.swing.JList();
        jPanel6 = new javax.swing.JPanel();
        jLabel6 = new javax.swing.JLabel();
        jRadioButtonRate48K = new javax.swing.JRadioButton();
        jRadioButtonRate96K = new javax.swing.JRadioButton();
        jRadioButtonRate192K = new javax.swing.JRadioButton();
        jRadioButtonRate384K = new javax.swing.JRadioButton();
        jLabel9 = new javax.swing.JLabel();
        jRadioButton256 = new javax.swing.JRadioButton();
        jRadioButton512 = new javax.swing.JRadioButton();
        jRadioButton1024 = new javax.swing.JRadioButton();
        jRadioButton2048 = new javax.swing.JRadioButton();
        jRadioButton4096 = new javax.swing.JRadioButton();
        jRadioButton8192 = new javax.swing.JRadioButton();
        jRadioButton16384 = new javax.swing.JRadioButton();
        jLabel10 = new javax.swing.JLabel();
        jRadioButtonBuffer64 = new javax.swing.JRadioButton();
        jRadioButtonBuffer128 = new javax.swing.JRadioButton();
        jRadioButtonBuffer256 = new javax.swing.JRadioButton();
        jRadioButtonBuffer512 = new javax.swing.JRadioButton();
        jRadioButtonBuffer1024 = new javax.swing.JRadioButton();
        jRadioButtonBuffer2048 = new javax.swing.JRadioButton();
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
        jRadioButtonPTTtoRing = new javax.swing.JRadioButton();
        jRadioButtonPTTtoTip = new javax.swing.JRadioButton();
        jCheckBoxPTTEnabled = new javax.swing.JCheckBox();
        jCheckBoxBiasEnabled = new javax.swing.JCheckBox();
        jRadioButtonAudioOutputRadio = new javax.swing.JRadioButton();
        jRadioButtonAudioOutputLocal = new javax.swing.JRadioButton();
        jRadioButtonAudioOutputBoth = new javax.swing.JRadioButton();
        jRadioButtonMicInputRadio = new javax.swing.JRadioButton();
        jRadioButtonMicInputLocal = new javax.swing.JRadioButton();
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
        jButtonClose = new javax.swing.JButton();

        setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);

        jListRadio.addListSelectionListener(new javax.swing.event.ListSelectionListener() {
            public void valueChanged(javax.swing.event.ListSelectionEvent evt) {
                jListRadioValueChanged(evt);
            }
        });
        jScrollPane1.setViewportView(jListRadio);

        javax.swing.GroupLayout jPanel7Layout = new javax.swing.GroupLayout(jPanel7);
        jPanel7.setLayout(jPanel7Layout);
        jPanel7Layout.setHorizontalGroup(
            jPanel7Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel7Layout.createSequentialGroup()
                .addGap(128, 128, 128)
                .addComponent(jScrollPane1, javax.swing.GroupLayout.PREFERRED_SIZE, 248, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addContainerGap(167, Short.MAX_VALUE))
        );
        jPanel7Layout.setVerticalGroup(
            jPanel7Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel7Layout.createSequentialGroup()
                .addGap(22, 22, 22)
                .addComponent(jScrollPane1)
                .addGap(23, 23, 23))
        );

        jTabbedPane1.addTab("Radio", jPanel7);

        jLabel6.setText("Sample Rate:");

        jRadioButtonRate48K.setText("48K");
        jRadioButtonRate48K.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonRate48KActionPerformed(evt);
            }
        });

        jRadioButtonRate96K.setText("96K");
        jRadioButtonRate96K.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonRate96KActionPerformed(evt);
            }
        });

        jRadioButtonRate192K.setText("192K");
        jRadioButtonRate192K.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonRate192KActionPerformed(evt);
            }
        });

        jRadioButtonRate384K.setText("384K");
        jRadioButtonRate384K.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonRate384KActionPerformed(evt);
            }
        });

        jLabel9.setText("FFT Size:");

        jRadioButton256.setText("256");
        jRadioButton256.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButton256ActionPerformed(evt);
            }
        });

        jRadioButton512.setText("512");
        jRadioButton512.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButton512ActionPerformed(evt);
            }
        });

        jRadioButton1024.setText("1024");
        jRadioButton1024.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButton1024ActionPerformed(evt);
            }
        });

        jRadioButton2048.setText("2048");
        jRadioButton2048.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButton2048ActionPerformed(evt);
            }
        });

        jRadioButton4096.setText("4096");
        jRadioButton4096.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButton4096ActionPerformed(evt);
            }
        });

        jRadioButton8192.setText("8192");
        jRadioButton8192.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButton8192ActionPerformed(evt);
            }
        });

        jRadioButton16384.setText("16384");
        jRadioButton16384.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButton16384ActionPerformed(evt);
            }
        });

        jLabel10.setText("Buffer Size:");

        jRadioButtonBuffer64.setText("64");
        jRadioButtonBuffer64.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonBuffer64ActionPerformed(evt);
            }
        });

        jRadioButtonBuffer128.setText("128");
        jRadioButtonBuffer128.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonBuffer128ActionPerformed(evt);
            }
        });

        jRadioButtonBuffer256.setText("256");
        jRadioButtonBuffer256.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonBuffer256ActionPerformed(evt);
            }
        });

        jRadioButtonBuffer512.setText("512");
        jRadioButtonBuffer512.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonBuffer512ActionPerformed(evt);
            }
        });

        jRadioButtonBuffer1024.setText("1024");
        jRadioButtonBuffer1024.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonBuffer1024ActionPerformed(evt);
            }
        });

        jRadioButtonBuffer2048.setText("2048");
        jRadioButtonBuffer2048.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonBuffer2048ActionPerformed(evt);
            }
        });

        javax.swing.GroupLayout jPanel6Layout = new javax.swing.GroupLayout(jPanel6);
        jPanel6.setLayout(jPanel6Layout);
        jPanel6Layout.setHorizontalGroup(
            jPanel6Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel6Layout.createSequentialGroup()
                .addGap(58, 58, 58)
                .addGroup(jPanel6Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jLabel6)
                    .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, jPanel6Layout.createSequentialGroup()
                        .addGroup(jPanel6Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jRadioButtonRate192K)
                            .addComponent(jRadioButtonRate96K)
                            .addComponent(jRadioButtonRate48K)
                            .addComponent(jRadioButtonRate384K))
                        .addGap(19, 19, 19)))
                .addGap(33, 33, 33)
                .addGroup(jPanel6Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jLabel10)
                    .addGroup(jPanel6Layout.createSequentialGroup()
                        .addGap(8, 8, 8)
                        .addGroup(jPanel6Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jRadioButtonBuffer128)
                            .addComponent(jRadioButtonBuffer64)
                            .addComponent(jRadioButtonBuffer256)
                            .addComponent(jRadioButtonBuffer512)
                            .addComponent(jRadioButtonBuffer1024)
                            .addComponent(jRadioButtonBuffer2048))))
                .addGap(40, 40, 40)
                .addGroup(jPanel6Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jRadioButton16384)
                    .addComponent(jLabel9)
                    .addGroup(jPanel6Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING)
                        .addGroup(jPanel6Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jRadioButton512)
                            .addComponent(jRadioButton256)
                            .addComponent(jRadioButton1024))
                        .addComponent(jRadioButton2048)
                        .addComponent(jRadioButton4096)
                        .addComponent(jRadioButton8192)))
                .addContainerGap(177, Short.MAX_VALUE))
        );
        jPanel6Layout.setVerticalGroup(
            jPanel6Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, jPanel6Layout.createSequentialGroup()
                .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                .addGroup(jPanel6Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jLabel10)
                    .addComponent(jLabel6)
                    .addComponent(jLabel9, javax.swing.GroupLayout.PREFERRED_SIZE, 28, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addGroup(jPanel6Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(jPanel6Layout.createSequentialGroup()
                        .addGroup(jPanel6Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                            .addComponent(jRadioButtonBuffer64)
                            .addComponent(jRadioButtonRate48K))
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addGroup(jPanel6Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                            .addComponent(jRadioButtonBuffer128)
                            .addComponent(jRadioButtonRate96K))
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addGroup(jPanel6Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                            .addComponent(jRadioButtonBuffer256)
                            .addComponent(jRadioButtonRate192K))
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addGroup(jPanel6Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                            .addComponent(jRadioButtonBuffer512)
                            .addComponent(jRadioButtonRate384K))
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(jRadioButtonBuffer1024)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(jRadioButtonBuffer2048))
                    .addGroup(jPanel6Layout.createSequentialGroup()
                        .addComponent(jRadioButton256)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(jRadioButton512)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(jRadioButton1024)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(jRadioButton2048)
                        .addGap(7, 7, 7)
                        .addComponent(jRadioButton4096, javax.swing.GroupLayout.PREFERRED_SIZE, 18, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(jRadioButton8192)))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(jRadioButton16384)
                .addContainerGap())
        );

        jTabbedPane1.addTab("DSP", jPanel6);

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
                .addContainerGap(229, Short.MAX_VALUE))
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
                .addContainerGap(62, Short.MAX_VALUE))
        );

        jTabbedPane1.addTab("Antenna", jPanel3);

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
                .addComponent(jSliderPASettings, javax.swing.GroupLayout.DEFAULT_SIZE, 401, Short.MAX_VALUE)
                .addContainerGap())
        );
        jPanel2Layout.setVerticalGroup(
            jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel2Layout.createSequentialGroup()
                .addGap(30, 30, 30)
                .addGroup(jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jSliderPASettings, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jLabelPASettings))
                .addContainerGap(171, Short.MAX_VALUE))
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
                .addContainerGap(225, Short.MAX_VALUE))
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

        buttonGroupTipRing.add(jRadioButtonPTTtoRing);
        jRadioButtonPTTtoRing.setText("Orion PTT to ring, Mic/Bias to tip");
        jRadioButtonPTTtoRing.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonPTTtoRingActionPerformed(evt);
            }
        });

        buttonGroupTipRing.add(jRadioButtonPTTtoTip);
        jRadioButtonPTTtoTip.setText("Orion PTT to tip, Mic/Bias to ring");
        jRadioButtonPTTtoTip.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonPTTtoTipActionPerformed(evt);
            }
        });

        jCheckBoxPTTEnabled.setText("Orion PTT Enabled");
        jCheckBoxPTTEnabled.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxPTTEnabledActionPerformed(evt);
            }
        });

        jCheckBoxBiasEnabled.setText("Orion Bias Enabled");
        jCheckBoxBiasEnabled.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxBiasEnabledActionPerformed(evt);
            }
        });

        buttonGroupAudioOutput.add(jRadioButtonAudioOutputRadio);
        jRadioButtonAudioOutputRadio.setText("Audio Output Radio");
        jRadioButtonAudioOutputRadio.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonAudioOutputRadioActionPerformed(evt);
            }
        });

        buttonGroupAudioOutput.add(jRadioButtonAudioOutputLocal);
        jRadioButtonAudioOutputLocal.setText("Audio Output Local");
        jRadioButtonAudioOutputLocal.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonAudioOutputLocalActionPerformed(evt);
            }
        });

        buttonGroupAudioOutput.add(jRadioButtonAudioOutputBoth);
        jRadioButtonAudioOutputBoth.setText("Audio Output Both");
        jRadioButtonAudioOutputBoth.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonAudioOutputBothActionPerformed(evt);
            }
        });

        buttonGroupMicSource.add(jRadioButtonMicInputRadio);
        jRadioButtonMicInputRadio.setText("Mic Input Radio");
        jRadioButtonMicInputRadio.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonMicInputRadioActionPerformed(evt);
            }
        });

        buttonGroupMicSource.add(jRadioButtonMicInputLocal);
        jRadioButtonMicInputLocal.setText("Mic Input Local");
        jRadioButtonMicInputLocal.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jRadioButtonMicInputLocalActionPerformed(evt);
            }
        });

        javax.swing.GroupLayout jPanel5Layout = new javax.swing.GroupLayout(jPanel5);
        jPanel5.setLayout(jPanel5Layout);
        jPanel5Layout.setHorizontalGroup(
            jPanel5Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel5Layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(jPanel5Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jRadioButtonPTTtoRing)
                    .addComponent(jRadioButtonPTTtoTip)
                    .addComponent(jRadioButtonAudioOutputBoth)
                    .addGroup(jPanel5Layout.createSequentialGroup()
                        .addGroup(jPanel5Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jRadioButtonAudioOutputRadio)
                            .addComponent(jRadioButtonAudioOutputLocal)
                            .addComponent(jCheckBoxPTTEnabled))
                        .addGap(67, 67, 67)
                        .addGroup(jPanel5Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jRadioButtonMicInputLocal)
                            .addGroup(jPanel5Layout.createSequentialGroup()
                                .addComponent(jRadioButtonMicInputRadio)
                                .addGap(18, 18, 18)
                                .addComponent(jCheckBox20dBMicBoost))))
                    .addComponent(jCheckBoxBiasEnabled))
                .addContainerGap(26, Short.MAX_VALUE))
        );
        jPanel5Layout.setVerticalGroup(
            jPanel5Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel5Layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(jPanel5Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jRadioButtonAudioOutputRadio)
                    .addComponent(jRadioButtonMicInputRadio)
                    .addComponent(jCheckBox20dBMicBoost))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel5Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jRadioButtonAudioOutputLocal)
                    .addComponent(jRadioButtonMicInputLocal))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(jRadioButtonAudioOutputBoth)
                .addGap(18, 18, 18)
                .addComponent(jCheckBoxPTTEnabled)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                .addComponent(jCheckBoxBiasEnabled)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(jRadioButtonPTTtoRing)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(jRadioButtonPTTtoTip)
                .addGap(73, 73, 73))
        );

        jTabbedPane1.addTab("Audio", jPanel5);

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
                                    .addComponent(jRadioButtonOSMS)))
                            .addGroup(jPanel1Layout.createSequentialGroup()
                                .addComponent(jLabel1)
                                .addGap(30, 30, 30)
                                .addComponent(jLabel2)
                                .addGap(18, 18, 18)
                                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                                    .addComponent(jRadioButtonPreAGC)
                                    .addComponent(jLabel3)
                                    .addComponent(jRadioButtonPostAGC))))
                        .addContainerGap(199, Short.MAX_VALUE))))
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
                .addContainerGap(120, Short.MAX_VALUE))
        );

        jTabbedPane1.addTab("NB2", jPanel1);

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
            .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createSequentialGroup()
                .addComponent(jTabbedPane1, javax.swing.GroupLayout.PREFERRED_SIZE, 551, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(0, 0, Short.MAX_VALUE))
            .addGroup(layout.createSequentialGroup()
                .addGap(249, 249, 249)
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
        if (wdsp != null) {
            wdsp.SetRXAEMNRnpeMethod(Channel.RX, 0);
        }
    }//GEN-LAST:event_jRadioButtonOSMSActionPerformed

    private void jCheckBoxAEFilterActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxAEFilterActionPerformed
        configuration.NB2_AE = this.jCheckBoxAEFilter.isSelected();
        if (wdsp != null) {
            wdsp.SetRXAEMNRaeRun(Channel.RX, configuration.NB2_AE ? 1 : 0);
        }
    }//GEN-LAST:event_jCheckBoxAEFilterActionPerformed

    private void jRadioButtonLinearActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonLinearActionPerformed
        configuration.NB2_GAIN = 0;
        if (wdsp != null) {
            wdsp.SetRXAEMNRgainMethod(Channel.RX, 0);
        }
    }//GEN-LAST:event_jRadioButtonLinearActionPerformed

    private void jRadioButtonLogActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonLogActionPerformed
        configuration.NB2_GAIN = 1;
        if (wdsp != null) {
            wdsp.SetRXAEMNRgainMethod(Channel.RX, 1);
        }
    }//GEN-LAST:event_jRadioButtonLogActionPerformed

    private void jRadioButtonMMSEActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonMMSEActionPerformed
        configuration.NB2_NPE = 1;
        if (wdsp != null) {
            wdsp.SetRXAEMNRnpeMethod(Channel.RX, 1);
        }
    }//GEN-LAST:event_jRadioButtonMMSEActionPerformed

    private void jRadioButtonPreAGCActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonPreAGCActionPerformed
        configuration.NB2_POSITION = 0;
        if (wdsp != null) {
            wdsp.SetRXAEMNRPosition(Channel.RX, 0);
        }
    }//GEN-LAST:event_jRadioButtonPreAGCActionPerformed

    private void jRadioButtonPostAGCActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonPostAGCActionPerformed
        configuration.NB2_POSITION = 1;
        if (wdsp != null) {
            wdsp.SetRXAEMNRPosition(Channel.RX, 1);
        }
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
        int value = this.jSliderPASettings.getValue();
        configuration.bands.get().setDrive((float) value / 100.0F);
    }//GEN-LAST:event_jSliderPASettingsStateChanged

    private void jCheckBoxOCRx1ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCRx1ActionPerformed
        Band band = configuration.bands.get();
        if (jCheckBoxOCRx1.isSelected()) {
            band.setOCRx((byte) (band.getOCRx() | 0x01));
        } else {
            band.setOCRx((byte) (band.getOCRx() & 0xFE));
        }
    }//GEN-LAST:event_jCheckBoxOCRx1ActionPerformed

    private void jCheckBoxOCRx2ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCRx2ActionPerformed
        Band band = configuration.bands.get();
        if (jCheckBoxOCRx2.isSelected()) {
            band.setOCRx((byte) (band.getOCRx() | 0x02));
        } else {
            band.setOCRx((byte) (band.getOCRx() & 0xFD));
        }
    }//GEN-LAST:event_jCheckBoxOCRx2ActionPerformed

    private void jCheckBoxOCRx3ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCRx3ActionPerformed
        Band band = configuration.bands.get();
        if (jCheckBoxOCRx3.isSelected()) {
            band.setOCRx((byte) (band.getOCRx() | 0x04));
        } else {
            band.setOCRx((byte) (band.getOCRx() & 0xFB));
        }
    }//GEN-LAST:event_jCheckBoxOCRx3ActionPerformed

    private void jCheckBoxOCRx4ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCRx4ActionPerformed
        Band band = configuration.bands.get();
        if (jCheckBoxOCRx4.isSelected()) {
            band.setOCRx((byte) (band.getOCRx() | 0x08));
        } else {
            band.setOCRx((byte) (band.getOCRx() & 0xF7));
        }
    }//GEN-LAST:event_jCheckBoxOCRx4ActionPerformed

    private void jCheckBoxOCRx5ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCRx5ActionPerformed
        Band band = configuration.bands.get();
        if (jCheckBoxOCRx5.isSelected()) {
            band.setOCRx((byte) (band.getOCRx() | 0x10));
        } else {
            band.setOCRx((byte) (band.getOCRx() & 0xEF));
        }
    }//GEN-LAST:event_jCheckBoxOCRx5ActionPerformed

    private void jCheckBoxOCRx6ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCRx6ActionPerformed
        Band band = configuration.bands.get();
        if (jCheckBoxOCRx6.isSelected()) {
            band.setOCRx((byte) (band.getOCRx() | 0x20));
        } else {
            band.setOCRx((byte) (band.getOCRx() & 0xDF));
        }
    }//GEN-LAST:event_jCheckBoxOCRx6ActionPerformed

    private void jCheckBoxOCRx7ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCRx7ActionPerformed
        Band band = configuration.bands.get();
        if (jCheckBoxOCRx7.isSelected()) {
            band.setOCRx((byte) (band.getOCRx() | 0x40));
        } else {
            band.setOCRx((byte) (band.getOCRx() & 0xBF));
        }
    }//GEN-LAST:event_jCheckBoxOCRx7ActionPerformed

    private void jCheckBoxOCTx1ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCTx1ActionPerformed
        Band band = configuration.bands.get();
        if (jCheckBoxOCTx1.isSelected()) {
            band.setOCTx((byte) (band.getOCTx() | 0x01));
        } else {
            band.setOCTx((byte) (band.getOCTx() & 0xFE));
        }
    }//GEN-LAST:event_jCheckBoxOCTx1ActionPerformed

    private void jCheckBoxOCTx2ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCTx2ActionPerformed
        Band band = configuration.bands.get();
        if (jCheckBoxOCTx2.isSelected()) {
            band.setOCTx((byte) (band.getOCTx() | 0x02));
        } else {
            band.setOCTx((byte) (band.getOCTx() & 0xFD));
        }
    }//GEN-LAST:event_jCheckBoxOCTx2ActionPerformed

    private void jCheckBoxOCTx3ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCTx3ActionPerformed
        Band band = configuration.bands.get();
        if (jCheckBoxOCTx3.isSelected()) {
            band.setOCTx((byte) (band.getOCTx() | 0x04));
        } else {
            band.setOCTx((byte) (band.getOCTx() & 0xFB));
        }
    }//GEN-LAST:event_jCheckBoxOCTx3ActionPerformed

    private void jCheckBoxOCTx4ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCTx4ActionPerformed
        Band band = configuration.bands.get();
        if (jCheckBoxOCTx4.isSelected()) {
            band.setOCTx((byte) (band.getOCTx() | 0x08));
        } else {
            band.setOCTx((byte) (band.getOCTx() & 0xF7));
        }
    }//GEN-LAST:event_jCheckBoxOCTx4ActionPerformed

    private void jCheckBoxOCTx5ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCTx5ActionPerformed
        Band band = configuration.bands.get();
        if (jCheckBoxOCTx5.isSelected()) {
            band.setOCTx((byte) (band.getOCTx() | 0x10));
        } else {
            band.setOCTx((byte) (band.getOCTx() & 0xEF));
        }
    }//GEN-LAST:event_jCheckBoxOCTx5ActionPerformed

    private void jCheckBoxOCTx6ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCTx6ActionPerformed
        Band band = configuration.bands.get();
        if (jCheckBoxOCTx6.isSelected()) {
            band.setOCTx((byte) (band.getOCTx() | 0x20));
        } else {
            band.setOCTx((byte) (band.getOCTx() & 0xDF));
        }
    }//GEN-LAST:event_jCheckBoxOCTx6ActionPerformed

    private void jCheckBoxOCTx7ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxOCTx7ActionPerformed
        Band band = configuration.bands.get();
        if (jCheckBoxOCTx7.isSelected()) {
            band.setOCTx((byte) (band.getOCTx() | 0x40));
        } else {
            band.setOCTx((byte) (band.getOCTx() & 0xBF));
        }
    }//GEN-LAST:event_jCheckBoxOCTx7ActionPerformed

    private void jCheckBox20dBMicBoostActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBox20dBMicBoostActionPerformed
        configuration.micboost = this.jCheckBox20dBMicBoost.isSelected();
    }//GEN-LAST:event_jCheckBox20dBMicBoostActionPerformed

    private void jRadioButtonPTTtoTipActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonPTTtoTipActionPerformed
        configuration.oriontipring = Configuration.MIC_PTT_TO_TIP_BIAS_TO_RING;
    }//GEN-LAST:event_jRadioButtonPTTtoTipActionPerformed

    private void jCheckBoxPTTEnabledActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxPTTEnabledActionPerformed
        configuration.orionmicptt = this.jCheckBoxPTTEnabled.isSelected() ? Configuration.MIC_PTT_ENABLE : Configuration.MIC_PTT_DISABLE;
    }//GEN-LAST:event_jCheckBoxPTTEnabledActionPerformed

    private void jCheckBoxBiasEnabledActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxBiasEnabledActionPerformed
        configuration.orionmicbias = this.jCheckBoxBiasEnabled.isSelected() ? Configuration.MIC_BIAS_ENABLE : Configuration.MIC_BIAS_DISABLE;
    }//GEN-LAST:event_jCheckBoxBiasEnabledActionPerformed

    private void jRadioButtonPTTtoRingActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonPTTtoRingActionPerformed
        configuration.oriontipring = Configuration.MIC_PTT_TO_RING_BIAS_TO_TIP;
    }//GEN-LAST:event_jRadioButtonPTTtoRingActionPerformed

    private void jRadioButtonRate48KActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonRate48KActionPerformed
        configuration.samplerate = 48000;
    }//GEN-LAST:event_jRadioButtonRate48KActionPerformed

    private void jRadioButtonRate96KActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonRate96KActionPerformed
        configuration.samplerate = 96000;
    }//GEN-LAST:event_jRadioButtonRate96KActionPerformed

    private void jRadioButtonRate192KActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonRate192KActionPerformed
        configuration.samplerate = 192000;
    }//GEN-LAST:event_jRadioButtonRate192KActionPerformed

    private void jRadioButtonRate384KActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonRate384KActionPerformed
        configuration.samplerate = 384000;
    }//GEN-LAST:event_jRadioButtonRate384KActionPerformed

    private void jRadioButton256ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButton256ActionPerformed
        configuration.fftsize = 256;
    }//GEN-LAST:event_jRadioButton256ActionPerformed

    private void jRadioButton512ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButton512ActionPerformed
        configuration.fftsize = 512;
    }//GEN-LAST:event_jRadioButton512ActionPerformed

    private void jRadioButton1024ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButton1024ActionPerformed
        configuration.fftsize = 1024;
    }//GEN-LAST:event_jRadioButton1024ActionPerformed

    private void jRadioButton2048ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButton2048ActionPerformed
        configuration.fftsize = 2048;
    }//GEN-LAST:event_jRadioButton2048ActionPerformed

    private void jRadioButton4096ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButton4096ActionPerformed
        configuration.fftsize = 4096;
    }//GEN-LAST:event_jRadioButton4096ActionPerformed

    private void jRadioButton8192ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButton8192ActionPerformed
        configuration.fftsize = 8192;
    }//GEN-LAST:event_jRadioButton8192ActionPerformed

    private void jRadioButtonBuffer64ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonBuffer64ActionPerformed
        configuration.buffersize = 64;
    }//GEN-LAST:event_jRadioButtonBuffer64ActionPerformed

    private void jRadioButtonBuffer128ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonBuffer128ActionPerformed
        configuration.buffersize = 128;
    }//GEN-LAST:event_jRadioButtonBuffer128ActionPerformed

    private void jRadioButtonBuffer256ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonBuffer256ActionPerformed
        configuration.buffersize = 256;
    }//GEN-LAST:event_jRadioButtonBuffer256ActionPerformed

    private void jRadioButtonBuffer512ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonBuffer512ActionPerformed
        configuration.buffersize = 512;
    }//GEN-LAST:event_jRadioButtonBuffer512ActionPerformed

    private void jRadioButtonBuffer1024ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonBuffer1024ActionPerformed
        configuration.buffersize = 1024;
    }//GEN-LAST:event_jRadioButtonBuffer1024ActionPerformed

    private void jRadioButton16384ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButton16384ActionPerformed
        configuration.fftsize = 16384;
    }//GEN-LAST:event_jRadioButton16384ActionPerformed

    private void jRadioButtonBuffer2048ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonBuffer2048ActionPerformed
        configuration.buffersize = 2048;
    }//GEN-LAST:event_jRadioButtonBuffer2048ActionPerformed

    private void jListRadioValueChanged(javax.swing.event.ListSelectionEvent evt) {//GEN-FIRST:event_jListRadioValueChanged
        switch (jListRadio.getSelectedIndex()) {
            case 0:
                if (configuration.discovered.getDevice() == Discovered.DEVICE_METIS) {
                    configuration.radio = Configuration.METIS_PENELOPE;
                } else if (configuration.discovered.getDevice() == Discovered.DEVICE_HERMES) {
                    configuration.radio = Configuration.HERMES_BOARD_ONLY;
                } else if (configuration.discovered.getDevice() == Discovered.DEVICE_ANGELIA) {
                    configuration.radio = Configuration.ANGELIA_BOARD_ONLY;
                } else if (configuration.discovered.getDevice() == Discovered.DEVICE_ORION) {
                    configuration.radio = Configuration.ORION_BOARD_ONLY;
                }
                break;
            case 1:
                if (configuration.discovered.getDevice() == Discovered.DEVICE_METIS) {
                    configuration.radio = Configuration.METIS_PENNYLANE;
                } else if (configuration.discovered.getDevice() == Discovered.DEVICE_HERMES) {
                    configuration.radio = Configuration.HERMES_ALEX;
                } else if (configuration.discovered.getDevice() == Discovered.DEVICE_ANGELIA) {
                    configuration.radio = Configuration.ANGELIA_ANAN100D;
                } else if (configuration.discovered.getDevice() == Discovered.DEVICE_ORION) {
                    configuration.radio = Configuration.ORION_ANAN200D;
                }
                break;
            case 2:
                if (configuration.discovered.getDevice() == Discovered.DEVICE_METIS) {
                    configuration.radio = Configuration.METIS_PENELOPE_ALEX;
                } else if (configuration.discovered.getDevice() == Discovered.DEVICE_HERMES) {
                    configuration.radio = Configuration.HERMES_APOLLO;
                }
                break;
            case 3:
                if (configuration.discovered.getDevice() == Discovered.DEVICE_METIS) {
                    configuration.radio = Configuration.METIS_PENNYLANE_ALEX;
                } else if (configuration.discovered.getDevice() == Discovered.DEVICE_HERMES) {
                    configuration.radio = Configuration.HERMES_ANAN10;
                }
                break;
            case 4:
                if (configuration.discovered.getDevice() == Discovered.DEVICE_HERMES) {
                    configuration.radio = Configuration.HERMES_ANAN100;
                }
                break;
        }
    }//GEN-LAST:event_jListRadioValueChanged

    private void jRadioButtonAudioOutputRadioActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonAudioOutputRadioActionPerformed
        configuration.audiooutput = Configuration.AUDIO_OUTPUT_RADIO;
    }//GEN-LAST:event_jRadioButtonAudioOutputRadioActionPerformed

    private void jRadioButtonAudioOutputLocalActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonAudioOutputLocalActionPerformed
        configuration.audiooutput = Configuration.AUDIO_OUTPUT_LOCAL;
    }//GEN-LAST:event_jRadioButtonAudioOutputLocalActionPerformed

    private void jRadioButtonAudioOutputBothActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonAudioOutputBothActionPerformed
        configuration.audiooutput = Configuration.AUDIO_OUTPUT_BOTH;
    }//GEN-LAST:event_jRadioButtonAudioOutputBothActionPerformed

    private void jRadioButtonMicInputRadioActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonMicInputRadioActionPerformed
        configuration.micsource = Configuration.MIC_SOURCE_RADIO;
    }//GEN-LAST:event_jRadioButtonMicInputRadioActionPerformed

    private void jRadioButtonMicInputLocalActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jRadioButtonMicInputLocalActionPerformed
        configuration.micsource = Configuration.MIC_SOURCE_LOCAL;
    }//GEN-LAST:event_jRadioButtonMicInputLocalActionPerformed

    private Configuration configuration;
    private WDSP wdsp;

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.ButtonGroup buttonGroupAudioOutput;
    private javax.swing.ButtonGroup buttonGroupGain;
    private javax.swing.ButtonGroup buttonGroupMicSource;
    private javax.swing.ButtonGroup buttonGroupNPEMethod;
    private javax.swing.ButtonGroup buttonGroupPosition;
    private javax.swing.ButtonGroup buttonGroupRXAnt;
    private javax.swing.ButtonGroup buttonGroupTXAnt;
    private javax.swing.ButtonGroup buttonGroupTipRing;
    private javax.swing.JButton jButtonClose;
    private javax.swing.JCheckBox jCheckBox20dBMicBoost;
    private javax.swing.JCheckBox jCheckBoxAEFilter;
    private javax.swing.JCheckBox jCheckBoxBiasEnabled;
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
    private javax.swing.JCheckBox jCheckBoxPTTEnabled;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel10;
    private javax.swing.JLabel jLabel2;
    private javax.swing.JLabel jLabel3;
    private javax.swing.JLabel jLabel4;
    private javax.swing.JLabel jLabel5;
    private javax.swing.JLabel jLabel6;
    private javax.swing.JLabel jLabel7;
    private javax.swing.JLabel jLabel8;
    private javax.swing.JLabel jLabel9;
    private javax.swing.JLabel jLabelOCBand;
    private javax.swing.JLabel jLabelPASettings;
    private javax.swing.JList jListRadio;
    private javax.swing.JPanel jPanel1;
    private javax.swing.JPanel jPanel2;
    private javax.swing.JPanel jPanel3;
    private javax.swing.JPanel jPanel4;
    private javax.swing.JPanel jPanel5;
    private javax.swing.JPanel jPanel6;
    private javax.swing.JPanel jPanel7;
    private javax.swing.JRadioButton jRadioButton1024;
    private javax.swing.JRadioButton jRadioButton16384;
    private javax.swing.JRadioButton jRadioButton2048;
    private javax.swing.JRadioButton jRadioButton256;
    private javax.swing.JRadioButton jRadioButton4096;
    private javax.swing.JRadioButton jRadioButton512;
    private javax.swing.JRadioButton jRadioButton8192;
    private javax.swing.JRadioButton jRadioButtonAnt1;
    private javax.swing.JRadioButton jRadioButtonAnt2;
    private javax.swing.JRadioButton jRadioButtonAnt3;
    private javax.swing.JRadioButton jRadioButtonAudioOutputBoth;
    private javax.swing.JRadioButton jRadioButtonAudioOutputLocal;
    private javax.swing.JRadioButton jRadioButtonAudioOutputRadio;
    private javax.swing.JRadioButton jRadioButtonBuffer1024;
    private javax.swing.JRadioButton jRadioButtonBuffer128;
    private javax.swing.JRadioButton jRadioButtonBuffer2048;
    private javax.swing.JRadioButton jRadioButtonBuffer256;
    private javax.swing.JRadioButton jRadioButtonBuffer512;
    private javax.swing.JRadioButton jRadioButtonBuffer64;
    private javax.swing.JRadioButton jRadioButtonLinear;
    private javax.swing.JRadioButton jRadioButtonLog;
    private javax.swing.JRadioButton jRadioButtonMMSE;
    private javax.swing.JRadioButton jRadioButtonMicInputLocal;
    private javax.swing.JRadioButton jRadioButtonMicInputRadio;
    private javax.swing.JRadioButton jRadioButtonNone;
    private javax.swing.JRadioButton jRadioButtonOSMS;
    private javax.swing.JRadioButton jRadioButtonPTTtoRing;
    private javax.swing.JRadioButton jRadioButtonPTTtoTip;
    private javax.swing.JRadioButton jRadioButtonPostAGC;
    private javax.swing.JRadioButton jRadioButtonPreAGC;
    private javax.swing.JRadioButton jRadioButtonRX1;
    private javax.swing.JRadioButton jRadioButtonRX2;
    private javax.swing.JRadioButton jRadioButtonRXXV;
    private javax.swing.JRadioButton jRadioButtonRate192K;
    private javax.swing.JRadioButton jRadioButtonRate384K;
    private javax.swing.JRadioButton jRadioButtonRate48K;
    private javax.swing.JRadioButton jRadioButtonRate96K;
    private javax.swing.JScrollPane jScrollPane1;
    private javax.swing.JSlider jSliderPASettings;
    private javax.swing.JTabbedPane jTabbedPane1;
    // End of variables declaration//GEN-END:variables
}
