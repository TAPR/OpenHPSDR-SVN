package org.g0orx.openhpsdr;

import java.io.Serializable;

import org.g0orx.openhpsdr.discovery.Discovered;

public class Configuration implements Serializable {

    static final long serialVersionUID = 8L;

    public static Configuration getInstance() {
        if (instance == null) {
            instance = new Configuration();
        }
        return instance;
    }

    protected Configuration() {
        bands = new Bands();
    }

    static Configuration instance;

    public static void setInstance(Configuration i) {
        instance = i;
    }

    public long getVersion() {
        return serialVersionUID;
    }

    public Discovered discovered;

    public double samplerate = 48000.0;
    public double dsprate = 48000.0;
    public int buffersize = 1024;
    public int fftsize = 4096;
    public int receivers = 1;
    public int receiver = 0;
    public int fps = 10;

    public Bands bands;

    public byte speed = Metis.SPEED_48KHZ;

    public byte dither = Metis.LT2208_DITHER_ON;
    public byte random = Metis.LT2208_RANDOM_ON;
    public byte preamp = Metis.LT2208_GAIN_OFF;

    public byte clock10 = Metis.MERCURY_10MHZ_SOURCE;
    public byte clock122 = Metis.MERCURY_122_88MHZ_SOURCE;

    public float drive = 0.5F;
    public float tunegain = 0.1F;

    public float afgain = 1.0F;

    public float micgain = 1.0F;

    public boolean subrx = false;

    public static final int AUDIO_OUTPUT_RADIO = 0;
    public static final int AUDIO_OUTPUT_LOCAL = 1;
    public static final int AUDIO_OUTPUT_BOTH = 2;
    public int audiooutput = AUDIO_OUTPUT_BOTH;

    public static final int MIC_SOURCE_RADIO = 0;
    public static final int MIC_SOURCE_LOCAL = 1;
    public int micsource = MIC_SOURCE_LOCAL;

    public boolean panadapter = true;
    public boolean waterfall = true;

    public boolean micboost = false;

    public int squelchlevel = 150;

    public int spectrumHigh = -40;
    public int spectrumLow = -140;

    public float meterCalibrationOffset = 17.0f;
    public float displayCalibrationOffset = 17.0f;
    public float preampOffset = 0.0F;

    public boolean opengl = false;

    public int waterfallHigh = -62;
    public int waterfallLow = -122;
    public boolean waterfallGrayscale = false;
    public boolean waterfallAutomatic = true;

    public boolean allowoutofband = false;

    public int attenuation = 20;

    public int step = 4; // 1000

    public boolean tunaknob = false;

    public static final int SMETER_PNG = 0;
    public static final int METER_PNG = 1;
    public int meter = SMETER_PNG;

    public static final int RADIO_UNKNOWN = 0;

    public static final int METIS_PENELOPE = 10;
    public static final int METIS_PENNYLANE = 11;
    public static final int METIS_PENELOPE_ALEX = 12;
    public static final int METIS_PENNYLANE_ALEX = 13;

    public static final int HERMES_BOARD_ONLY = 20;
    public static final int HERMES_ALEX = 21;
    public static final int HERMES_APOLLO = 22;
    public static final int HERMES_ANAN10 = 23;
    public static final int HERMES_ANAN100 = 24;

    public static final int ANGELIA_BOARD_ONLY = 30;
    public static final int ANGELIA_ANAN100D = 31;

    public static final int ORION_BOARD_ONLY = 40;
    public static final int ORION_ANAN200D = 41;

    public static final int HERMES_LITE_ONLY = 50;

    public int radio = RADIO_UNKNOWN;

    public int cwkeysreversed = 0; // 0=disabled 1=enabled

    public int cwkeyerspeed = 12; // 1-60 WPM

    public static final int KEYER_STRAIGHT = 0;
    public static final int KEYER_MODE_A = 1;
    public static final int KEYER_MODE_B = 2;

    public int cwkeyermode = KEYER_STRAIGHT;

    public int cwkeyerweight = 30; //0-100
    public int cwkeyerspacing = 0; // 0=on 1=off

    public int cwinternal = 1;  // 0=external, 1=internal
    public int cwsidetonevolume = 127; // 0-127
    public int cwpttdelay = 20; // 0-255ms

    public int cwhangtime = 10; // ms
    public int cwsidetonefrequency = 400; // Hz

    static final int MIC_PTT_TO_RING_BIAS_TO_TIP = 0;
    static final int MIC_PTT_TO_TIP_BIAS_TO_RING = 1;
    public int oriontipring = MIC_PTT_TO_RING_BIAS_TO_TIP;

    static final int MIC_BIAS_DISABLE = 0;
    static final int MIC_BIAS_ENABLE = 1;
    public int orionmicbias = MIC_BIAS_ENABLE;

    static final int MIC_PTT_ENABLE = 0;
    static final int MIC_PTT_DISABLE = 1;
    public int orionmicptt = MIC_PTT_ENABLE;

    public boolean NB2 = false;
    public boolean NB2_AE = true;
    public int NB2_GAIN = 1; // 0:Linear 1:Log
    public int NB2_NPE = 0; // 0:OSMS 1:MMSE
    public int NB2_POSITION = 1; // 0:PRE-AGC 1:POST-AGC
    
    public boolean NR = false;
    public boolean NB = false;
    public boolean ANF = false;
    
    public int myX=0;
    public int myY=0;
    public int myWidth=0;
    public int myHeight=0;
    
    public int bandscopeX=0;
    public int bandscopeY=0;
    public int bandscopeWidth=0;
    public int bandscopeHeight=0;
    
    
}
