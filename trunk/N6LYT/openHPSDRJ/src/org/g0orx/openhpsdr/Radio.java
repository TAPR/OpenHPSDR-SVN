/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package org.g0orx.openhpsdr;

import com.sun.glass.ui.Cursor;
import java.awt.Color;
import java.awt.Event;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.awt.event.MouseWheelEvent;
import java.awt.event.MouseWheelListener;
import java.beans.XMLDecoder;
import java.beans.XMLEncoder;
import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.ArrayList;

import org.g0orx.openhpsdr.discovery.Discover;
import org.g0orx.openhpsdr.discovery.Discovered;
import org.g0orx.openhpsdr.discovery.Discovery;
import org.g0orx.openhpsdr.modes.Mode;
import org.g0orx.openhpsdr.modes.Modes;
import org.g0orx.openhpsdr.wdsp.WDSP;

/**
 *
 * @author john
 */
public class Radio extends javax.swing.JFrame implements Discover, BandChanged, ModeChanged, FilterChanged, MouseListener, MouseMotionListener, MouseWheelListener, PTTListener {

    /**
     * Creates new form Radio
     */
    public Radio() {
        initComponents();
        this.setTitle("openHPSDR");
        this.addWindowListener(new java.awt.event.WindowAdapter() {
            @Override
            public void windowClosing(java.awt.event.WindowEvent windowEvent) {
                if (metis != null) {
                    stop();
                }
            }
        });
        configuration = Configuration.getInstance();
        discovered.clear();
        this.bandJPanel.addListener(this);
        this.modeJPanel.addListener(this);
        this.filterJPanel.addListener(this);

        this.bandJPanel.setEnabled(false);
        this.modeJPanel.setEnabled(false);
        this.filterJPanel.setEnabled(false);

        this.jButtonDiscover.setBackground(Color.WHITE);
        this.jButtonConfigure.setBackground(Color.WHITE);
        this.jButtonStart.setBackground(Color.WHITE);
        this.jButtonMOX.setBackground(Color.WHITE);
        this.jButtonTune.setBackground(Color.WHITE);
        this.jButtonDiscover.setEnabled(false);
        this.jButtonConfigure.setEnabled(false);
        this.jButtonStart.setEnabled(false);
        this.jButtonMOX.setEnabled(false);
        this.jButtonTune.setEnabled(false);
        this.jSliderAFGain.setEnabled(false);
        this.jSliderAGCGain.setEnabled(false);
        this.jSliderAttenuation.setEnabled(false);
        this.jSliderDrive.setEnabled(false);
        this.jSliderMicGain.setEnabled(false);
        this.jComboBoxAGC.setEnabled(false);
        this.jCheckBoxNB2.setEnabled(false);
        this.jCheckBoxANF.setEnabled(false);
        this.jCheckBoxNR.setEnabled(false);

        panadapterPanel.addMouseListener(this);
        panadapterPanel.addMouseMotionListener(this);
        panadapterPanel.addMouseWheelListener(this);

        frequencyPanel.addMouseListener(this);
        frequencyPanel.addMouseMotionListener(this);
        frequencyPanel.addMouseWheelListener(this);

        waterfallPanel.addMouseListener(this);
        waterfallPanel.addMouseMotionListener(this);
        waterfallPanel.addMouseWheelListener(this);

        this.setVisible(true);

        this.discoverDialog = new DiscoverJDialog(this, true);

        Discovery discovery = new Discovery(this);
        discovery.start();

        this.discoverDialog.setVisible(true);

    }

    public void discovered(Discovered d) {
        Log.i("Radio", "Discovered: " + d.toString());
        discovered.add(d);
    }

    public void endDiscovery() {
        Log.i("Radio", "End Discovery");
        this.discoverDialog.dispose();
        this.jButtonDiscover.setEnabled(true);

        if (discovered.size() == 0) {
            this.setTitle("openHPSDR: No devices found");
            return;
        } else if (discovered.size() == 1) {
            configuration.discovered = this.discovered.get(0);
        } else {
            Log.i("Radio", "DeviceJDialog");
            DeviceJDialog dialog = new DeviceJDialog(this, true, discovered);
            dialog.setVisible(true);
        }

        if (configuration == null || configuration.discovered == null) {
            this.setTitle("openHPSDR: No device selected");
            return;
        }
        this.jButtonStart.setEnabled(true);
        selected = configuration.discovered;
        Log.i("Radio", "selected: " + selected.toString());
        this.setTitle("openHPSDR: " + selected.getDeviceName() + " " + selected.getAddress() + " (" + selected.getMac() + ")");

        if (configuration.radio == Configuration.RADIO_UNKNOWN) {
            switch (configuration.discovered.getDevice()) {
                case Discovered.DEVICE_METIS:
                    configuration.radio = Configuration.METIS_PENELOPE_ALEX;
                    break;
                case Discovered.DEVICE_HERMES:
                    configuration.radio = Configuration.HERMES_ANAN100;
                    break;
                case Discovered.DEVICE_ANGELIA:
                    configuration.radio = Configuration.ANGELIA_ANAN100D;
                    break;
                case Discovered.DEVICE_ORION:
                    configuration.radio = Configuration.ORION_ANAN200D;
                    break;
                case Discovered.DEVICE_HERMES_LITE:
                    configuration.radio = Configuration.HERMES_LITE_ONLY;
                    break;
            }
        }

        if (configuration.discovered.getDevice() == Discovered.DEVICE_HERMES_LITE) {
            this.jLabelAttenuation.setText("LNA Gain: ");
        } else {
            this.jLabelAttenuation.setText("Attenuation: ");
        }

        switch (configuration.discovered.getDevice()) {
            case Discovered.DEVICE_HERMES_LITE:
                this.jSliderAttenuation.setMaximum(60); // -12 .. 48
                this.jSliderAttenuation.setValue(configuration.attenuation + 12);
                break;
            case Discovered.DEVICE_HERMES:
            case Discovered.DEVICE_ANGELIA:
            case Discovered.DEVICE_ORION:
                if (configuration.radio == Configuration.HERMES_ALEX
                        || configuration.radio == Configuration.ANGELIA_ANAN100D
                        || configuration.radio == Configuration.ORION_ANAN200D) {
                    this.jSliderAttenuation.setMaximum(61); // 0 .. 61
                } else {
                    this.jSliderAttenuation.setMaximum(31); // 0 .. 31
                }
                this.jSliderAttenuation.setValue(configuration.attenuation);
                break;
            case Discovered.DEVICE_METIS:
                this.jSliderAttenuation.setMaximum(30); // 0 .. 30
                this.jSliderAttenuation.setValue(configuration.attenuation / 10);
                break;
            default:
                break;
        }

        this.jButtonConfigure.setEnabled(true);

        filename = selected.getMac() + ".conf";
        filename = filename.replace(":", "-");
        try {
            FileInputStream fis = new FileInputStream(filename);
            ObjectInputStream ois = new ObjectInputStream(fis);
            Configuration.setInstance((Configuration) ois.readObject());
            ois.close();
            fis.close();
            configuration = Configuration.getInstance();
            configuration.discovered = selected;
        } catch (Exception e) {
            Log.i("Radio", "Read configuration: " + e.toString());
            configuration = Configuration.getInstance();
            configuration.discovered = selected;
        }

        this.bandJPanel.init();
        this.modeJPanel.init();
        this.filterJPanel.init();

        this.vfoPanel.repaint();
        this.panadapterPanel.repaint();
        this.frequencyPanel.repaint();

        this.jSliderAFGain.setValue((int) (configuration.afgain * 100.0));
        this.jLabelAFGain.setText("AF Gain: " + (int) (configuration.afgain * 100.0) + "%");
        this.jSliderAGCGain.setValue((int) configuration.bands.get().getAGCGain());
        this.jLabelAGCGain.setText("AGC Gain: " + (int) configuration.bands.get().getAGCGain());
        this.jComboBoxAGC.setSelectedIndex(configuration.bands.get().getAGC());
        this.jSliderAttenuation.setValue(configuration.attenuation);
        this.jLabelAttenuation.setText("Attenuation: " + configuration.attenuation + " dBm");
        this.jSliderDrive.setValue((int) (configuration.drive * 255.0));
        this.jLabelDrive.setText("Drive: " + (int) (configuration.drive * 100.0) + "%");
        this.jSliderMicGain.setValue((int)(configuration.micgain*100.0F));
        this.jLabelMicGain.setText("Mic Gain: "+(int)(configuration.micgain*100.0F)+"%");
        
        this.jCheckBoxNB2.setSelected(configuration.NB2);
        this.jCheckBoxNR.setSelected(configuration.NR);
        this.jCheckBoxANF.setSelected(configuration.ANF);
    }

    public void start() {

        Log.i("Radio", "start: " + selected.toString());
        this.setCursor(java.awt.Cursor.getPredefinedCursor(java.awt.Cursor.WAIT_CURSOR));
        String title = this.getTitle();
        this.setTitle(title + ": Starting - Please wait ...");

        this.jButtonDiscover.setEnabled(false);
        this.jButtonMOX.setEnabled(true);
        this.jButtonTune.setEnabled(true);

        this.jButtonStart.setText("Stop");
        this.jButtonStart.setEnabled(true);

        this.jSliderAFGain.setEnabled(true);
        this.jSliderAGCGain.setEnabled(true);
        this.jSliderAttenuation.setEnabled(true);
        this.jSliderDrive.setEnabled(true);
        this.jSliderMicGain.setEnabled(true);
        this.jComboBoxAGC.setEnabled(true);
        this.jCheckBoxNB2.setEnabled(true);
        this.jCheckBoxANF.setEnabled(true);
        this.jCheckBoxNR.setEnabled(true);

        this.bandJPanel.setEnabled(true);
        this.modeJPanel.setEnabled(true);
        this.filterJPanel.setEnabled(true);

        this.metis = new Metis(this.panadapterPanel.getWidth(), configuration.bandscope);

        this.vfoPanel.setMetis(metis);
        this.panadapterPanel.setMetis(metis);
        this.frequencyPanel.setMetis(metis);

        this.setTitle(title + ": Initializing WDSP - Please wait ...");
        Log.i("Radio", "load WDSP");
        wdsp = WDSP.getInstance();
        Log.i("Radio", "WDSP loaded");

        Band band = configuration.bands.get();
        BandStack bandstack = band.get();
        Filter filter = Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());
        int low = filter.getLow();
        int high = filter.getHigh();
        if (bandstack.getMode() == Modes.CWL) {
            low = -configuration.cwsidetonefrequency - low;
            high = -configuration.cwsidetonefrequency + high;
        } else if (bandstack.getMode() == Modes.CWU) {
            low = configuration.cwsidetonefrequency - low;
            high = configuration.cwsidetonefrequency + high;
        }

        Log.i("Radio", "OpenChannel (RX): buffersize=" + configuration.buffersize + " fftsize=" + configuration.fftsize + " samperate=" + configuration.samplerate + " dsprate=" + configuration.dsprate);
        // setup receiver
        wdsp.OpenChannel(Channel.RX,
                configuration.buffersize,
                configuration.fftsize,
                (int) configuration.samplerate,
                (int) configuration.dsprate,
                48000,
                0/*rx*/, 1/*RUNNING*/,
                0.010, 0.025, 0.0, 0.010, 0);

        wdsp.SetRXAMode(Channel.RX, bandstack.getMode());
        wdsp.SetRXABandpassFreqs(Channel.RX, low, high);
        wdsp.SetRXABandpassRun(Channel.RX, 1);
        wdsp.SetRXAAGCTop(Channel.RX, band.getAGCGain());
        wdsp.SetRXAAMDSBMode(Channel.RX, 0);
        //wdsp.SetRXAShiftRun(Channel.RX, 0);
        wdsp.SetRXAEMNRRun(Channel.RX, configuration.NB2 ? 1 : 0);
        wdsp.SetRXAEMNRgainMethod(Channel.RX, configuration.NB2_GAIN);
        wdsp.SetRXAEMNRnpeMethod(Channel.RX, configuration.NB2_NPE);
        wdsp.SetRXAEMNRaeRun(Channel.RX, configuration.NB2_AE ? 1 : 0);
        wdsp.SetRXAEMNRPosition(Channel.RX, configuration.NB2_POSITION);

        wdsp.SetRXAANRRun(Channel.RX, configuration.NR ? 1 : 0);
        wdsp.SetRXAANFRun(Channel.RX, configuration.ANF ? 1 : 0);

        // setup transmitter
        Log.i("Radio", "OpenChannel (TX): buffersize=" + configuration.buffersize + " fftsize=" + configuration.fftsize + " samperate=" + configuration.samplerate + " dsprate=" + configuration.dsprate);
        wdsp.OpenChannel(Channel.TX,
                configuration.buffersize,
                configuration.fftsize,
                (int) configuration.samplerate,
                (int) configuration.dsprate,
                48000,
                1/*tx*/, 0/*NOT RUNNING*/,
                0.010, 0.025, 0.0, 0.010, 0);
        wdsp.SetTXAMode(Channel.TX, bandstack.getMode());
        wdsp.SetTXABandpassFreqs(Channel.TX, low, high);
        wdsp.SetTXABandpassRun(Channel.TX, 1);

        // setup sub receiver
        Log.i("Radio", "OpenChannel (SUBRX): buffersize=" + configuration.buffersize + " fftsize=" + configuration.fftsize + " samperate=" + configuration.samplerate + " dsprate=" + configuration.dsprate);
        wdsp.OpenChannel(Channel.SUBRX,
                configuration.buffersize,
                configuration.fftsize,
                (int) configuration.samplerate,
                (int) configuration.dsprate,
                48000,
                0/*rx*/, 0/*NOT RUNNING*/,
                0.010, 0.025, 0.0, 0.010, 0);

        wdsp.SetRXAMode(Channel.SUBRX, bandstack.getMode());
        wdsp.SetRXABandpassFreqs(Channel.SUBRX, low, high);
        wdsp.SetRXABandpassRun(Channel.SUBRX, 1);
        setAGC(Channel.SUBRX, band.getAGC());
        wdsp.SetRXAAGCTop(Channel.SUBRX, band.getAGCGain());
        wdsp.SetRXAAMDSBMode(Channel.SUBRX, 0);
        wdsp.SetRXAShiftRun(Channel.SUBRX, 0);
        wdsp.SetRXAShiftFreq(Channel.SUBRX, bandstack.getSubRxFrequency());
        wdsp.SetRXAEMNRRun(Channel.SUBRX, configuration.NB2 ? 1 : 0);
        wdsp.SetRXAEMNRgainMethod(Channel.SUBRX, configuration.NB2_GAIN);
        wdsp.SetRXAEMNRnpeMethod(Channel.SUBRX, configuration.NB2_NPE);
        wdsp.SetRXAEMNRaeRun(Channel.SUBRX, configuration.NB2_AE ? 1 : 0);
        wdsp.SetRXAEMNRPosition(Channel.SUBRX, configuration.NB2_POSITION);

        wdsp.SetRXAANRRun(Channel.SUBRX, configuration.NR ? 1 : 0);
        wdsp.SetRXAANFRun(Channel.SUBRX, configuration.ANF ? 1 : 0);

        bandChanged(band);

        metis.setPTTListener(this);
        metis.start();

        update = new DisplayUpdate(vfoPanel, panadapterPanel, waterfallPanel, meterJPanel, metis);
        update.startTimer();
        Log.i("Radio", "Start completed");
        this.setTitle(title);
        
                
        this.setCursor(java.awt.Cursor.getPredefinedCursor(java.awt.Cursor.DEFAULT_CURSOR));
    }

    private void stop() {
        if(metis.isTransmitting()) {
            processPTT(false);
        }
        
        update.terminate();
        while (update.isRunning()) {
            try {
                Thread.sleep(10);
            } catch (Exception e) {
            }
        }
        metis.terminate();
        while (metis.isRunning()) {
            try {
                Thread.sleep(10);
            } catch (Exception e) {
            }
        }
        wdsp.CloseChannel(Channel.SUBRX);
        wdsp.CloseChannel(Channel.TX);
        wdsp.CloseChannel(Channel.RX);

        this.jButtonStart.setText("Start");
        this.jButtonDiscover.setEnabled(true);
        this.jButtonMOX.setEnabled(false);
        this.jButtonTune.setEnabled(false);
        this.jSliderAFGain.setEnabled(false);
        this.jSliderAGCGain.setEnabled(false);
        this.jSliderAttenuation.setEnabled(false);
        this.jSliderDrive.setEnabled(false);
        this.jSliderMicGain.setEnabled(false);
        this.jComboBoxAGC.setEnabled(false);
        this.jCheckBoxNB2.setEnabled(false);
        this.jCheckBoxANF.setEnabled(false);
        this.jCheckBoxNR.setEnabled(false);

        this.bandJPanel.setEnabled(false);
        this.modeJPanel.setEnabled(false);
        this.filterJPanel.setEnabled(false);

        this.vfoPanel.setMetis(null);
        this.panadapterPanel.setMetis(null);
        this.frequencyPanel.setMetis(null);

        wdsp = null;
        metis = null;
        update = null;

        try {
            FileOutputStream fos = new FileOutputStream(filename);
            ObjectOutputStream oos = new ObjectOutputStream(fos);
            oos.writeObject(Configuration.getInstance());
            oos.close();
            fos.close();
        } catch (Exception e) {
            Log.i("Radio", "stop: " + e.toString());
        }
        this.vfoPanel.repaint();
        this.panadapterPanel.repaint();
        this.frequencyPanel.repaint();
        this.waterfallPanel.repaint();
        this.frequencyPanel.repaint();
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        vfoPanel = new org.g0orx.openhpsdr.VFOPanel();
        panadapterPanel = new org.g0orx.openhpsdr.PanadapterPanel();
        frequencyPanel = new org.g0orx.openhpsdr.FrequencyPanel();
        waterfallPanel = new org.g0orx.openhpsdr.WaterfallPanel();
        bandJPanel = new org.g0orx.openhpsdr.BandJPanel();
        modeJPanel = new org.g0orx.openhpsdr.ModeJPanel();
        filterJPanel = new org.g0orx.openhpsdr.FilterJPanel();
        jButtonDiscover = new javax.swing.JButton();
        jButtonConfigure = new javax.swing.JButton();
        jButtonStart = new javax.swing.JButton();
        jButtonMOX = new javax.swing.JButton();
        jButtonTune = new javax.swing.JButton();
        jCheckBoxNB2 = new javax.swing.JCheckBox();
        jCheckBoxNR = new javax.swing.JCheckBox();
        jCheckBoxANF = new javax.swing.JCheckBox();
        jPanel1 = new javax.swing.JPanel();
        jLabelAFGain = new javax.swing.JLabel();
        jLabelAGCGain = new javax.swing.JLabel();
        jLabel1 = new javax.swing.JLabel();
        jLabelAttenuation = new javax.swing.JLabel();
        jLabelDrive = new javax.swing.JLabel();
        jSliderAFGain = new javax.swing.JSlider();
        jSliderAGCGain = new javax.swing.JSlider();
        jComboBoxAGC = new javax.swing.JComboBox();
        jSliderAttenuation = new javax.swing.JSlider();
        jSliderDrive = new javax.swing.JSlider();
        jLabelMicGain = new javax.swing.JLabel();
        jSliderMicGain = new javax.swing.JSlider();
        meterJPanel = new org.g0orx.openhpsdr.MeterJPanel();

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
        setBounds(new java.awt.Rectangle(0, 0, 1024, 400));
        setMinimumSize(new java.awt.Dimension(1024, 400));

        javax.swing.GroupLayout frequencyPanelLayout = new javax.swing.GroupLayout(frequencyPanel);
        frequencyPanel.setLayout(frequencyPanelLayout);
        frequencyPanelLayout.setHorizontalGroup(
            frequencyPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 0, Short.MAX_VALUE)
        );
        frequencyPanelLayout.setVerticalGroup(
            frequencyPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 21, Short.MAX_VALUE)
        );

        javax.swing.GroupLayout waterfallPanelLayout = new javax.swing.GroupLayout(waterfallPanel);
        waterfallPanel.setLayout(waterfallPanelLayout);
        waterfallPanelLayout.setHorizontalGroup(
            waterfallPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 0, Short.MAX_VALUE)
        );
        waterfallPanelLayout.setVerticalGroup(
            waterfallPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 104, Short.MAX_VALUE)
        );

        jButtonDiscover.setFont(new java.awt.Font("Arial", 1, 15)); // NOI18N
        jButtonDiscover.setText("Discover");
        jButtonDiscover.setFocusable(false);
        jButtonDiscover.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        jButtonDiscover.setVerticalTextPosition(javax.swing.SwingConstants.BOTTOM);
        jButtonDiscover.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonDiscoverActionPerformed(evt);
            }
        });

        jButtonConfigure.setFont(new java.awt.Font("Arial", 1, 15)); // NOI18N
        jButtonConfigure.setText("Configure");
        jButtonConfigure.setFocusable(false);
        jButtonConfigure.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        jButtonConfigure.setVerticalTextPosition(javax.swing.SwingConstants.BOTTOM);
        jButtonConfigure.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonConfigureActionPerformed(evt);
            }
        });

        jButtonStart.setFont(new java.awt.Font("Arial", 1, 15)); // NOI18N
        jButtonStart.setText("Start");
        jButtonStart.setFocusable(false);
        jButtonStart.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        jButtonStart.setVerticalTextPosition(javax.swing.SwingConstants.BOTTOM);
        jButtonStart.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonStartActionPerformed(evt);
            }
        });

        jButtonMOX.setFont(new java.awt.Font("Arial", 1, 15)); // NOI18N
        jButtonMOX.setText("MOX");
        jButtonMOX.setFocusable(false);
        jButtonMOX.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        jButtonMOX.setVerticalTextPosition(javax.swing.SwingConstants.BOTTOM);
        jButtonMOX.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonMOXActionPerformed(evt);
            }
        });

        jButtonTune.setFont(new java.awt.Font("Arial", 1, 15)); // NOI18N
        jButtonTune.setText("Tune");
        jButtonTune.setFocusable(false);
        jButtonTune.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);
        jButtonTune.setVerticalTextPosition(javax.swing.SwingConstants.BOTTOM);
        jButtonTune.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonTuneActionPerformed(evt);
            }
        });

        jCheckBoxNB2.setText("NB2");
        jCheckBoxNB2.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxNB2ActionPerformed(evt);
            }
        });

        jCheckBoxNR.setText("NR");
        jCheckBoxNR.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxNRActionPerformed(evt);
            }
        });

        jCheckBoxANF.setText("ANF");
        jCheckBoxANF.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jCheckBoxANFActionPerformed(evt);
            }
        });

        jLabelAFGain.setText("AF Gain:");

        jLabelAGCGain.setText("AGC Gain:");

        jLabel1.setText("AGC:");

        jLabelAttenuation.setText("Attenuation:");

        jLabelDrive.setText("Drive:");

        jSliderAFGain.setMaximum(200);
        jSliderAFGain.addChangeListener(new javax.swing.event.ChangeListener() {
            public void stateChanged(javax.swing.event.ChangeEvent evt) {
                jSliderAFGainStateChanged(evt);
            }
        });
        jSliderAFGain.addVetoableChangeListener(new java.beans.VetoableChangeListener() {
            public void vetoableChange(java.beans.PropertyChangeEvent evt)throws java.beans.PropertyVetoException {
                jSliderAFGainVetoableChange(evt);
            }
        });

        jSliderAGCGain.setMaximum(120);
        jSliderAGCGain.addChangeListener(new javax.swing.event.ChangeListener() {
            public void stateChanged(javax.swing.event.ChangeEvent evt) {
                jSliderAGCGainStateChanged(evt);
            }
        });

        jComboBoxAGC.setModel(new javax.swing.DefaultComboBoxModel(new String[] { "Off", "Long", "Slow", "Medium", "Fast" }));
        jComboBoxAGC.addItemListener(new java.awt.event.ItemListener() {
            public void itemStateChanged(java.awt.event.ItemEvent evt) {
                jComboBoxAGCItemStateChanged(evt);
            }
        });

        jSliderAttenuation.setMajorTickSpacing(10);
        jSliderAttenuation.setMaximum(61);
        jSliderAttenuation.addChangeListener(new javax.swing.event.ChangeListener() {
            public void stateChanged(javax.swing.event.ChangeEvent evt) {
                jSliderAttenuationStateChanged(evt);
            }
        });

        jSliderDrive.setMaximum(255);
        jSliderDrive.addChangeListener(new javax.swing.event.ChangeListener() {
            public void stateChanged(javax.swing.event.ChangeEvent evt) {
                jSliderDriveStateChanged(evt);
            }
        });

        jLabelMicGain.setText("Mic Gain:");

        jSliderMicGain.setMaximum(200);
        jSliderMicGain.addChangeListener(new javax.swing.event.ChangeListener() {
            public void stateChanged(javax.swing.event.ChangeEvent evt) {
                jSliderMicGainStateChanged(evt);
            }
        });

        javax.swing.GroupLayout jPanel1Layout = new javax.swing.GroupLayout(jPanel1);
        jPanel1.setLayout(jPanel1Layout);
        jPanel1Layout.setHorizontalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jLabelAFGain)
                    .addComponent(jSliderAFGain, javax.swing.GroupLayout.PREFERRED_SIZE, 150, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jSliderAGCGain, javax.swing.GroupLayout.PREFERRED_SIZE, 150, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jLabelAGCGain))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jLabel1, javax.swing.GroupLayout.PREFERRED_SIZE, 86, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jComboBoxAGC, javax.swing.GroupLayout.PREFERRED_SIZE, 114, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jLabelAttenuation)
                    .addComponent(jSliderAttenuation, javax.swing.GroupLayout.PREFERRED_SIZE, 150, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jSliderDrive, javax.swing.GroupLayout.PREFERRED_SIZE, 150, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jLabelDrive))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jLabelMicGain)
                    .addComponent(jSliderMicGain, javax.swing.GroupLayout.PREFERRED_SIZE, 150, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addContainerGap())
        );
        jPanel1Layout.setVerticalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, jPanel1Layout.createSequentialGroup()
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jLabelAFGain)
                    .addComponent(jLabelAGCGain)
                    .addComponent(jLabel1, javax.swing.GroupLayout.PREFERRED_SIZE, 28, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jLabelAttenuation)
                    .addComponent(jLabelDrive)
                    .addComponent(jLabelMicGain))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(jPanel1Layout.createSequentialGroup()
                        .addGap(0, 0, Short.MAX_VALUE)
                        .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                            .addComponent(jSliderDrive, javax.swing.GroupLayout.Alignment.TRAILING, javax.swing.GroupLayout.PREFERRED_SIZE, 0, Short.MAX_VALUE)
                            .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING)
                                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                                    .addComponent(jSliderAGCGain, javax.swing.GroupLayout.PREFERRED_SIZE, 42, javax.swing.GroupLayout.PREFERRED_SIZE)
                                    .addComponent(jSliderAFGain, javax.swing.GroupLayout.PREFERRED_SIZE, 42, javax.swing.GroupLayout.PREFERRED_SIZE))
                                .addComponent(jSliderAttenuation, javax.swing.GroupLayout.PREFERRED_SIZE, 42, javax.swing.GroupLayout.PREFERRED_SIZE)
                                .addComponent(jComboBoxAGC, javax.swing.GroupLayout.Alignment.LEADING, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))))
                    .addGroup(jPanel1Layout.createSequentialGroup()
                        .addComponent(jSliderMicGain, javax.swing.GroupLayout.PREFERRED_SIZE, 42, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addGap(0, 0, Short.MAX_VALUE))))
        );

        javax.swing.GroupLayout meterJPanelLayout = new javax.swing.GroupLayout(meterJPanel);
        meterJPanel.setLayout(meterJPanelLayout);
        meterJPanelLayout.setHorizontalGroup(
            meterJPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 156, Short.MAX_VALUE)
        );
        meterJPanelLayout.setVerticalGroup(
            meterJPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 102, Short.MAX_VALUE)
        );

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(frequencyPanel, javax.swing.GroupLayout.Alignment.TRAILING, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
            .addComponent(panadapterPanel, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
            .addComponent(waterfallPanel, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
            .addComponent(vfoPanel, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
            .addGroup(layout.createSequentialGroup()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createSequentialGroup()
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jButtonDiscover)
                            .addComponent(jButtonMOX, javax.swing.GroupLayout.PREFERRED_SIZE, 76, javax.swing.GroupLayout.PREFERRED_SIZE))
                        .addGap(2, 2, 2)
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jButtonTune, javax.swing.GroupLayout.PREFERRED_SIZE, 82, javax.swing.GroupLayout.PREFERRED_SIZE)
                            .addComponent(jButtonConfigure))
                        .addGap(15, 15, 15)
                        .addComponent(jButtonStart))
                    .addGroup(layout.createSequentialGroup()
                        .addComponent(jCheckBoxNB2)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                        .addComponent(jCheckBoxNR)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                        .addComponent(jCheckBoxANF)))
                .addGap(18, 18, 18)
                .addComponent(bandJPanel, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(modeJPanel, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(filterJPanel, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addComponent(meterJPanel, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
            .addGroup(layout.createSequentialGroup()
                .addComponent(jPanel1, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(0, 0, Short.MAX_VALUE))
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createSequentialGroup()
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jButtonStart)
                            .addComponent(jButtonDiscover)
                            .addComponent(jButtonConfigure))
                        .addGap(6, 6, 6)
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                            .addComponent(jButtonMOX, javax.swing.GroupLayout.PREFERRED_SIZE, 52, javax.swing.GroupLayout.PREFERRED_SIZE)
                            .addComponent(jButtonTune, javax.swing.GroupLayout.PREFERRED_SIZE, 52, javax.swing.GroupLayout.PREFERRED_SIZE))
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                            .addComponent(jCheckBoxNB2)
                            .addComponent(jCheckBoxNR)
                            .addComponent(jCheckBoxANF)))
                    .addComponent(bandJPanel, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(modeJPanel, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(filterJPanel, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(meterJPanel, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addComponent(jPanel1, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(0, 0, 0)
                .addComponent(vfoPanel, javax.swing.GroupLayout.PREFERRED_SIZE, 37, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(0, 0, 0)
                .addComponent(panadapterPanel, javax.swing.GroupLayout.DEFAULT_SIZE, 107, Short.MAX_VALUE)
                .addGap(0, 0, 0)
                .addComponent(frequencyPanel, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(0, 0, 0)
                .addComponent(waterfallPanel, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void jButtonStartActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonStartActionPerformed
        if (stopped) {
            this.start();
            stopped = false;
        } else {
            // Stop
            this.stop();
            stopped = true;
        }
    }//GEN-LAST:event_jButtonStartActionPerformed

    private void jButtonDiscoverActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonDiscoverActionPerformed
        discovered.clear();
        this.jButtonStart.setEnabled(false);
        this.jButtonConfigure.setEnabled(false);
        this.discoverDialog = new DiscoverJDialog(this, true);
        Discovery discovery = new Discovery(this);
        discovery.start();
        this.discoverDialog.setVisible(true);
    }//GEN-LAST:event_jButtonDiscoverActionPerformed

    private void jButtonConfigureActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonConfigureActionPerformed
        // different configuration if active
        if (wdsp == null) {
            SystemJDialog dialog = new SystemJDialog(this, true);
            dialog.setVisible(true);
        } else {
            RadioJDialog dialog = new RadioJDialog(this, true);
            dialog.setVisible(true);
        }

        this.vfoPanel.repaint();
        this.panadapterPanel.repaint();
        this.frequencyPanel.repaint();
        this.waterfallPanel.repaint();

    }//GEN-LAST:event_jButtonConfigureActionPerformed

    private void jButtonMOXActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonMOXActionPerformed
        processPTT(!metis.isTransmitting());
    }//GEN-LAST:event_jButtonMOXActionPerformed

    private void jButtonTuneActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonTuneActionPerformed
        cantransmit = false;
        switch (configuration.discovered.getDevice()) {
            case Discovered.DEVICE_METIS:
                if (metis.penelope_software_version > 0) {
                    cantransmit = true;
                }
                break;
            case Discovered.DEVICE_HERMES:
            case Discovered.DEVICE_GRIFFIN:
            case Discovered.DEVICE_ANGELIA:
            case Discovered.DEVICE_ORION:
                cantransmit = true;
                break;
        }
        if (!cantransmit) {
            Log.i("Radio", "Cannot Transmit: No Penelope or Pennylane");
        } else if (outofband && !configuration.allowoutofband) {
            Log.i("Radio", "Out of Band");
        } else {
            if (metis.isTransmitting() && metis.isTuning()) {
                            //Log.i("RadioActivity","Tune button: stop tuning");

                //Log.i("RadioActivity","TUN: SetChannelState(1,0,1)");
                wdsp.SetChannelState(Channel.TX, 0, 1);
                //Log.i("RadioActivity","TUN: SetChannelState(0,1,0)");
                wdsp.SetChannelState(Channel.RX, 1, 0);
                if (configuration.subrx) {
                    wdsp.SetChannelState(Channel.SUBRX, 1, 0);
                }

                metis.setTransmit(false, false);
                this.jButtonTune.setBackground(Color.white);
            } else {
                if (metis.isTransmitting() && !metis.isTuning()) {
                    //Log.i("RadioActivity","Tune button: stop transmitting");
                    this.jButtonMOX.setBackground(Color.white);
                    this.jButtonTune.setBackground(Color.white);
                }

                // receive to transmit
                //Log.i("RadioActivity","TUN: SetChannelState(0,0,1)");
                wdsp.SetChannelState(Channel.RX, 0, 1);
                if (configuration.subrx) {
                    wdsp.SetChannelState(Channel.SUBRX, 0, 1);
                }
                //Log.i("RadioActivity","TUN: SetChannelState(1,1,0)");
                wdsp.SetChannelState(Channel.TX, 1, 0);

                //configuration.transmit=true;
                //configuration.tuning=true;
                metis.setTransmit(true, true);
                this.jButtonTune.setBackground(Color.red);
            }
        }
        if (metis.isTuning()) {
            this.jSliderDrive.setValue((int) (configuration.tunegain * 255.0));
        } else {
            this.jSliderDrive.setValue((int) (configuration.drive * 255.0));
        }
        this.vfoPanel.repaint();
        this.frequencyPanel.repaint();

    }//GEN-LAST:event_jButtonTuneActionPerformed

    private void jSliderAFGainVetoableChange(java.beans.PropertyChangeEvent evt)throws java.beans.PropertyVetoException {//GEN-FIRST:event_jSliderAFGainVetoableChange

    }//GEN-LAST:event_jSliderAFGainVetoableChange

    private void jSliderAFGainStateChanged(javax.swing.event.ChangeEvent evt) {//GEN-FIRST:event_jSliderAFGainStateChanged
        configuration.afgain = (float) this.jSliderAFGain.getValue() / 100.0F;
        this.jLabelAFGain.setText("AF Gain: " + (int) (configuration.afgain * 100.0) + "%");
    }//GEN-LAST:event_jSliderAFGainStateChanged

    private void jSliderAGCGainStateChanged(javax.swing.event.ChangeEvent evt) {//GEN-FIRST:event_jSliderAGCGainStateChanged
        double gain = (double) this.jSliderAGCGain.getValue();
        Band band = configuration.bands.get();
        band.setAGCGain(gain);
        if (wdsp != null) {
            wdsp.SetRXAAGCTop(Channel.RX, gain);
            wdsp.SetRXAAGCTop(Channel.SUBRX, gain);
        }
        this.jLabelAGCGain.setText("AGC Gain: " + (int) configuration.bands.get().getAGCGain());
    }//GEN-LAST:event_jSliderAGCGainStateChanged

    private void jComboBoxAGCItemStateChanged(java.awt.event.ItemEvent evt) {//GEN-FIRST:event_jComboBoxAGCItemStateChanged
        Band band = configuration.bands.get();
        int agc = this.jComboBoxAGC.getSelectedIndex();
        band.setAGC(agc);
        setAGC(Channel.RX, agc);
        setAGC(Channel.SUBRX, agc);

    }//GEN-LAST:event_jComboBoxAGCItemStateChanged

    private void jSliderDriveStateChanged(javax.swing.event.ChangeEvent evt) {//GEN-FIRST:event_jSliderDriveStateChanged
        float gain = (float) this.jSliderDrive.getValue() / 255.0F;
        if (metis != null && metis.isTuning()) {
            configuration.tunegain = gain;
        } else {
            configuration.drive = gain;
        }
        this.jLabelDrive.setText("Drive: " + (int) (gain * 100.0) + "%");
    }//GEN-LAST:event_jSliderDriveStateChanged

    private void jSliderAttenuationStateChanged(javax.swing.event.ChangeEvent evt) {//GEN-FIRST:event_jSliderAttenuationStateChanged
        int attenuation = this.jSliderAttenuation.getValue();
        switch (configuration.discovered.getDevice()) {
            case Discovered.DEVICE_HERMES_LITE:
                configuration.attenuation = attenuation - 12;
                break;
            case Discovered.DEVICE_HERMES:
            case Discovered.DEVICE_ANGELIA:
            case Discovered.DEVICE_ORION:
                configuration.attenuation = attenuation;
                break;
            case Discovered.DEVICE_METIS:
                configuration.attenuation = attenuation * 10;
                break;
            default:
                break;
        }
        this.jLabelAttenuation.setText("Attenuation: " + configuration.attenuation + " dBm");
    }//GEN-LAST:event_jSliderAttenuationStateChanged

    private void jCheckBoxNB2ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxNB2ActionPerformed
        configuration.NB2 = this.jCheckBoxNB2.isSelected();
        wdsp.SetRXAEMNRRun(Channel.RX, configuration.NB2 ? 1 : 0);
    }//GEN-LAST:event_jCheckBoxNB2ActionPerformed

    private void jCheckBoxNRActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxNRActionPerformed
        configuration.NR = this.jCheckBoxNR.isSelected();
        wdsp.SetRXAANRRun(Channel.RX, configuration.NR ? 1 : 0);
    }//GEN-LAST:event_jCheckBoxNRActionPerformed

    private void jCheckBoxANFActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jCheckBoxANFActionPerformed
        configuration.ANF = this.jCheckBoxANF.isSelected();
        wdsp.SetRXAANFRun(Channel.RX, configuration.ANF ? 1 : 0);
    }//GEN-LAST:event_jCheckBoxANFActionPerformed

    private void jSliderMicGainStateChanged(javax.swing.event.ChangeEvent evt) {//GEN-FIRST:event_jSliderMicGainStateChanged
        configuration.micgain=(float)this.jSliderMicGain.getValue()/100.0F;
        this.jLabelMicGain.setText("Mic Gain: "+(int)(configuration.micgain*100.0F)+"%");
    }//GEN-LAST:event_jSliderMicGainStateChanged

    public void bandChanged(Band band) {

        Log.i("Radio", "bandChanged: " + band.getName());

        // update mode and filter
        this.modeJPanel.init();
        this.filterJPanel.init();

        if (this.stopped) {
            return;
        }

        BandStack bandstack = band.get();
        Filter filter = Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());

        if (bandstack != null && !locked) {
            int low = filter.getLow();
            int high = filter.getHigh();
            if (bandstack.getMode() == Modes.CWL) {
                low = -configuration.cwsidetonefrequency - low;
                high = -configuration.cwsidetonefrequency + high;
            } else if (bandstack.getMode() == Modes.CWU) {
                low = configuration.cwsidetonefrequency - low;
                high = configuration.cwsidetonefrequency + high;
            }

            setFilter(low, high);
            setAGC(Channel.RX, band.getAGC());

            if (configuration.subrx) {
                configuration.subrx = false;
                wdsp.SetChannelState(Channel.SUBRX, 0, 0);
                wdsp.SetRXAShiftRun(Channel.SUBRX, 0);
                setAGC(Channel.SUBRX, band.getAGC());
            }

            setMode(bandstack.getMode());
            setFrequency(bandstack.getFrequency());

            /*
             Button squelch = (Button) findViewById(R.id.buttonSquelch);
             squelch.getBackground().setColorFilter(band.getSquelch() ? Color.RED : Color.LTGRAY, Mode.MULTIPLY);
             */
            if (band.get().getMode() == Modes.FMN) {
                double v1 = -2.0 * (double) band.getSquelchValue() / 100.0;
                double v2 = Math.pow(10.0, v1);
                Log.i("RadioActivity", "FM squelch set to " + band.getSquelchValue() + " " + v1 + " " + v2);
                wdsp.SetRXAFMSQThreshold(Channel.RX, v2);
                wdsp.SetRXAFMSQRun(Channel.RX, band.getSquelch() ? 1 : 0);
            } else {
                wdsp.SetRXAAMSQThreshold(Channel.RX, (double) -band.getSquelchValue());
                wdsp.SetRXAAMSQRun(Channel.RX, band.getSquelch() ? 1 : 0);
            }

            this.jSliderAGCGain.setValue((int) band.getAGCGain());
            this.jComboBoxAGC.setSelectedIndex(configuration.bands.get().getAGC());
            //this.setTitle("Band " + band.getName() + "(" + band.getBandstackentry() + ")" + " " + Modes.getMode(bandstack.getMode()).getName() + " " + Frequency.toString(bandstack.getFrequency()));
        }

    }

    public void modeChanged(int mode) {
        // update filter
        this.filterJPanel.init();

        if (this.stopped) {
            return;
        }
        setMode(mode);
    }

    public void filterChanged(int f) {
        if (this.stopped) {
            return;
        }
        Band band = configuration.bands.get();
        BandStack bandstack = band.get();
        Filter filter = Modes.getMode(bandstack.getMode()).getFilter(f);
        int low = filter.getLow();
        int high = filter.getHigh();
        if (bandstack.getMode() == Modes.CWL) {
            low = -configuration.cwsidetonefrequency - low;
            high = -configuration.cwsidetonefrequency + high;
        } else if (bandstack.getMode() == Modes.CWU) {
            low = configuration.cwsidetonefrequency - low;
            high = configuration.cwsidetonefrequency + high;
        }

        setFilter(low, high);
    }

    public void PTTChanged(boolean ptt) {
        Log.i("Radio", "PTTChanged:" + ptt);
        if (!this.stopped) {
            java.awt.event.ActionEvent event = new java.awt.event.ActionEvent(this.jButtonMOX, java.awt.event.ActionEvent.ACTION_PERFORMED, "MOX", System.currentTimeMillis(), java.awt.event.InputEvent.BUTTON1_MASK);
            Log.i("Radio", "postEvent:" + event.toString());
            java.awt.Toolkit.getDefaultToolkit().getSystemEventQueue().postEvent(event);
            //processPTT(ptt);
        }
    }

    private void processPTT(boolean state) {

        Log.i("Radio", "processPTT:" + state);

        if (state) {
            // start transmitting
            cantransmit = false;
            switch (configuration.discovered.getDevice()) {
                case Discovered.DEVICE_METIS:
                    if (metis.penelope_software_version > 0) {
                        cantransmit = true;
                    }
                    break;
                case Discovered.DEVICE_HERMES:
                case Discovered.DEVICE_GRIFFIN:
                case Discovered.DEVICE_ANGELIA:
                case Discovered.DEVICE_ORION:
                    cantransmit = true;
                    break;
            }
            if (!cantransmit) {
                Log.i("Radio", "Cannot transmit");
            } else if (outofband && !configuration.allowoutofband) {
                Log.i("Radio", "Out of Band");
            } else {
                if (metis.isTuning()) {
                    metis.setTransmit(false, false);
                }
                BandStack bandstack = configuration.bands.get().get();

                wdsp.SetChannelState(Channel.RX, 0, 1);
                if (configuration.subrx) {
                    wdsp.SetChannelState(Channel.SUBRX, 0, 1);
                }
                wdsp.SetChannelState(Channel.TX, 1, 0);

                metis.setTransmit(true, false);
                /*
                 if (configuration.micsource == Configuration.MIC_SOURCE_LOCAL) {
                 microphone = new Microphone(metis, Channel.TX);
                 microphone.start();
                 }
                 */
                this.jButtonMOX.setBackground(Color.red);
            }
        } else {
            // stop transmitting
            wdsp.SetChannelState(Channel.TX, 0, 1);
            wdsp.SetChannelState(Channel.RX, 1, 0);
            if (configuration.subrx) {
                wdsp.SetChannelState(Channel.SUBRX, 1, 0);
            }
            metis.setTransmit(false, false);
            this.jButtonMOX.setBackground(Color.white);
            this.jButtonTune.setBackground(Color.white);
        }

        this.vfoPanel.repaint();
        this.frequencyPanel.repaint();
    }

    private void setFrequency(long f) {
        String fs = String.format("%d.%03d.%03d",
                f / 1000000,
                (f % 1000000) / 1000,
                (f % 1000));
        /*
         frequency.setText(fs);
         */
        BandEdge bandedge = configuration.bands.get().getBandEdge();
        if (f >= bandedge.getLow() && f < bandedge.getHigh()) {
            outofband = false;
        } else {
            outofband = true;
        }

        frequencyPanel.repaint();
        vfoPanel.repaint();
    }

    private void setMode(int mode) {
        if (wdsp != null) {
            wdsp.SetRXAMode(Channel.RX, mode);
            wdsp.SetRXAMode(Channel.SUBRX, mode);
            wdsp.SetTXAMode(Channel.TX, mode);
        }
        frequencyPanel.repaint();
        vfoPanel.repaint();
    }

    private void setFilter(int low, int high) {
        if (wdsp != null) {
            wdsp.SetRXABandpassFreqs(Channel.RX, low, high);
            wdsp.SetRXABandpassFreqs(Channel.SUBRX, low, high);
            wdsp.SetTXABandpassFreqs(Channel.TX, low, high);
        }
        frequencyPanel.repaint();
        vfoPanel.repaint();
    }

    private void setAGC(int channel, int agc) {
        if (wdsp != null) {
            wdsp.SetRXAAGCMode(channel, agc);
            switch (agc) {
                case AGCLong:
                    wdsp.SetRXAAGCAttack(channel, 2);
                    wdsp.SetRXAAGCHang(channel, 2000);
                    wdsp.SetRXAAGCDecay(channel, 2000);
                    break;
                case AGCSlow:
                    wdsp.SetRXAAGCAttack(channel, 2);
                    wdsp.SetRXAAGCHang(channel, 1000);
                    wdsp.SetRXAAGCDecay(channel, 500);
                    break;
                case AGCMed:
                    wdsp.SetRXAAGCAttack(channel, 2);
                    wdsp.SetRXAAGCHang(channel, 0);
                    wdsp.SetRXAAGCDecay(channel, 250);
                    break;
                case AGCFast:
                    wdsp.SetRXAAGCAttack(channel, 2);
                    wdsp.SetRXAAGCHang(channel, 0);
                    wdsp.SetRXAAGCDecay(channel, 50);
                    break;
            }
        }
    }

    @Override
    public void mouseClicked(MouseEvent e) {
        if (!stopped) {
            double hzperpixel = configuration.samplerate / (double) this.getWidth();
            int step = Step.getStep(configuration.step);
            Band band = configuration.bands.get();
            BandStack bandstack = band.get();
            long start = bandstack.getFrequency() - ((long) configuration.samplerate / 2);
            long f = start + (long) (e.getX() * hzperpixel);

            // make it step resolution
            f = f / (long) step * (long) step;

            bandstack.setFrequency(f);
            vfoPanel.repaint();
            frequencyPanel.repaint();
        }
    }

    @Override
    public void mouseEntered(MouseEvent e) {
        // TODO Auto-generated method stub

    }

    @Override
    public void mouseExited(MouseEvent e) {
        // TODO Auto-generated method stub

    }

    @Override
    public void mousePressed(MouseEvent e) {
        // TODO Auto-generated method stub
        //Log.i("Radio","mousePressed: "+e.getX());

        if (!stopped) {
            mouseX = e.getX();
        }
    }

    @Override
    public void mouseReleased(MouseEvent e) {
        // align to step frequency
        if (!stopped) {
            Band band = configuration.bands.get();
            BandStack bandstack = band.get();
            int step = Step.getStep(configuration.step);
            bandstack.setFrequency(bandstack.getFrequency() / step * step);
            vfoPanel.repaint();
            frequencyPanel.repaint();
        }
    }

    @Override
    public void mouseMoved(MouseEvent e) {
        // TODO Auto-generated method stub

    }

    @Override
    public void mouseDragged(MouseEvent e) {
        if (!stopped) {
            int increment = mouseX - e.getX();
            mouseX = e.getX();
            Band band = configuration.bands.get();
            BandStack bandstack = band.get();
            //int step = Step.getStep(configuration.step);
            int step = (int) configuration.samplerate / this.getWidth();
            bandstack.setFrequency(bandstack.getFrequency() + (step * increment));
            vfoPanel.repaint();
            frequencyPanel.repaint();
        }
    }

    @Override
    public void mouseWheelMoved(MouseWheelEvent e) {
        if (!stopped) {
            Band band = configuration.bands.get();
            BandStack bandstack = band.get();
            int step = Step.getStep(configuration.step);
            bandstack.setFrequency(bandstack.getFrequency() + (step * e.getWheelRotation()));
            vfoPanel.repaint();
            frequencyPanel.repaint();
        }
    }

    private static final int AGCOff = 0;
    private static final int AGCLong = 1;
    private static final int AGCSlow = 2;
    private static final int AGCMed = 3;
    private static final int AGCFast = 4;

    private String filename;
    private Configuration configuration;
    private WDSP wdsp;
    private Metis metis;
    private DisplayUpdate update;

    private boolean locked = false;
    private boolean outofband = false;

    private ArrayList<Discovered> discovered = new ArrayList<Discovered>();

    private Discovered selected;

    private int mouseX;

    private boolean cantransmit = false;

    private DiscoverJDialog discoverDialog;

    private boolean stopped = true;

    private String[] agclabels = {"Off", "Long", "Slow", "Medium", "Fast"};

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private org.g0orx.openhpsdr.BandJPanel bandJPanel;
    private org.g0orx.openhpsdr.FilterJPanel filterJPanel;
    private org.g0orx.openhpsdr.FrequencyPanel frequencyPanel;
    private javax.swing.JButton jButtonConfigure;
    private javax.swing.JButton jButtonDiscover;
    private javax.swing.JButton jButtonMOX;
    private javax.swing.JButton jButtonStart;
    private javax.swing.JButton jButtonTune;
    private javax.swing.JCheckBox jCheckBoxANF;
    private javax.swing.JCheckBox jCheckBoxNB2;
    private javax.swing.JCheckBox jCheckBoxNR;
    private javax.swing.JComboBox jComboBoxAGC;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabelAFGain;
    private javax.swing.JLabel jLabelAGCGain;
    private javax.swing.JLabel jLabelAttenuation;
    private javax.swing.JLabel jLabelDrive;
    private javax.swing.JLabel jLabelMicGain;
    private javax.swing.JPanel jPanel1;
    private javax.swing.JSlider jSliderAFGain;
    private javax.swing.JSlider jSliderAGCGain;
    private javax.swing.JSlider jSliderAttenuation;
    private javax.swing.JSlider jSliderDrive;
    private javax.swing.JSlider jSliderMicGain;
    private org.g0orx.openhpsdr.MeterJPanel meterJPanel;
    private org.g0orx.openhpsdr.ModeJPanel modeJPanel;
    private org.g0orx.openhpsdr.PanadapterPanel panadapterPanel;
    private org.g0orx.openhpsdr.VFOPanel vfoPanel;
    private org.g0orx.openhpsdr.WaterfallPanel waterfallPanel;
    // End of variables declaration//GEN-END:variables
}
