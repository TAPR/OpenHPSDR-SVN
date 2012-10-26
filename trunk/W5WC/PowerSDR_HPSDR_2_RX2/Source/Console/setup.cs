//=================================================================
// setup.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2009  FlexRadio Systems
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// You may contact us via email at: sales@flex-radio.com.
// Paper mail may be sent to: 
//    FlexRadio Systems
//    8900 Marybank Dr.
//    Austin, TX 78750
//    USA
//=================================================================

using System.Collections.Generic;
using System.Linq;

using FlexCW;

namespace PowerSDR
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows.Forms;
   // using SDRSerialSupportII;
    using System.Text;
    using System.IO;
    using System.IO.Ports;
    //using TDxInput;

    public partial class Setup : Form
    {
        #region Variable Declaration

        private Console console;
        private Progress progress;
        private ArrayList KeyList;
        private int sound_card;
        private bool initializing;
        public bool alex_fw_good = false;
        private TabPage tpDSP;
        private TabPage tpDisplay;
        private NumericUpDownTS udDisplayGridStep;
        public NumericUpDownTS udDisplayGridMin;
        public NumericUpDownTS udDisplayGridMax;
        private LabelTS lblDisplayGridStep;
        private LabelTS lblDisplayGridMin;
        private LabelTS lblDisplayGridMax;
        private TabPage tpGeneral;
        private ComboBoxTS comboGeneralLPTAddr;
        private GroupBoxTS grpDisplaySpectrumGrid;
        private ButtonTS btnOK;
        private ButtonTS btnCancel;
        private ButtonTS btnApply;
        public CheckBoxTS chkGeneralSpurRed;
        private LabelTS lblGeneralLPTAddr;
        private NumericUpDownTS udGeneralLPTDelay;
        private LabelTS lblGeneralLPTDelay;
        public TabControl tcSetup;
        private TabPage tpKeyboard;
        private LabelTS lblKBTuneDown;
        private LabelTS lblKBTuneUp;
        private ComboBoxTS comboKBTuneDown1;
        private ComboBoxTS comboKBTuneDown3;
        private ComboBoxTS comboKBTuneDown2;
        private ComboBoxTS comboKBTuneUp1;
        private ComboBoxTS comboKBTuneUp2;
        private ComboBoxTS comboKBTuneUp3;
        private ComboBoxTS comboKBTuneUp4;
        private ComboBoxTS comboKBTuneDown4;
        private ComboBoxTS comboKBTuneUp5;
        private ComboBoxTS comboKBTuneDown5;
        private ComboBoxTS comboKBTuneDown6;
        private ComboBoxTS comboKBTuneUp7;
        private ComboBoxTS comboKBTuneDown7;
        private ComboBoxTS comboKBTuneUp6;
        private GroupBoxTS grpKBTune;
        private LabelTS lblKBTuneDigit;
        private LabelTS lblKBTune7;
        private LabelTS lblKBTune6;
        private LabelTS lblKBTune5;
        private LabelTS lblKBTune4;
        private LabelTS lblKBTune3;
        private LabelTS lblKBTune2;
        private LabelTS lblKBTune1;
        private GroupBoxTS grpKBBand;
        private LabelTS lblKBBandUp;
        private LabelTS lblKBBandDown;
        private GroupBoxTS grpKBFilter;
        private LabelTS lblKBFilterUp;
        private LabelTS lblKBFilterDown;
        private GroupBoxTS grpKBMode;
        private LabelTS lblKBModeUp;
        private LabelTS lblKBModeDown;
        private ComboBoxTS comboKBBandUp;
        private ComboBoxTS comboKBBandDown;
        private ComboBoxTS comboKBFilterUp;
        private ComboBoxTS comboKBFilterDown;
        private ComboBoxTS comboKBModeUp;
        private ComboBoxTS comboKBModeDown;
        private LabelTS lblDisplayFPS;
        private NumericUpDownTS udDisplayFPS;
        private GroupBoxTS grpGeneralDDS;
        private LabelTS lblPLLMult;
        private NumericUpDownTS udDDSPLLMult;
        private LabelTS lblIFFrequency;
        private NumericUpDownTS udDDSIFFreq;
        private LabelTS lblClockCorrection;
        private NumericUpDownTS udDDSCorrection;
        public TabPage tpAudio;
        private TabPage tpTransmit;
        private NumericUpDownTS udTXFilterHigh;
        private LabelTS lblTXFilterLow;
        private LabelTS lblTXFilterHigh;
        private NumericUpDownTS udTXFilterLow;
        private GroupBoxTS grpTXFilter;
        private GroupBoxTS grpDisplayPhase;
        private NumericUpDownTS udDisplayPhasePts;
        private GroupBoxTS grpDisplayAverage;
        private LabelTS lblDisplayPhasePts;
        private GroupBoxTS grpGeneralCalibration;
        private LabelTS lblGeneralCalFrequency;
        private LabelTS lblGeneralCalLevel;
        private NumericUpDownTS udGeneralCalLevel;
        private GroupBoxTS grpDisplayRefreshRates;
        private LabelTS lblDisplayMeterDelay;
        private NumericUpDownTS udDisplayMeterDelay;
        private TabPage tpAppearance;
        private LabelTS lblDisplayFilterColor;
        private LabelTS lblDisplayLineWidth;
        private NumericUpDownTS udDisplayLineWidth;
        private LabelTS lblDisplayDataLineColor;
        private LabelTS lblDisplayTextColor;
        private LabelTS lblDisplayZeroLineColor;
        private LabelTS lblDisplayGridColor;
        private LabelTS lblDisplayBackgroundColor;
        private GroupBoxTS grpAppearanceMeter;
        private LabelTS lblAppearanceMeterRight;
        private LabelTS lblAppearanceMeterLeft;
        private LabelTS lblAppearanceGenBtnSel;
        private GroupBoxTS grpGeneralOptions;
        private CheckBoxTS chkGeneralDisablePTT;
        private LabelTS lblDisplayPeakText;
        private NumericUpDownTS udDisplayPeakText;
        private NumericUpDownTS udDisplayCPUMeter;
        private LabelTS lblDisplayCPUMeter;
        private GroupBoxTS grpDisplayWaterfall;
        public NumericUpDownTS udDisplayWaterfallHighLevel;
        private LabelTS lblDisplayWaterfallHighLevel;
        private LabelTS lblDisplayWaterfallLowLevel;
        public NumericUpDownTS udDisplayWaterfallLowLevel;
        private LabelTS lblDisplayWaterfallLowColor;
        private CheckBoxTS chkGeneralPAPresent;
        private ButtonTS btnGeneralCalLevelStart;
        private ButtonTS btnGeneralCalFreqStart;
        private ButtonTS btnGeneralCalImageStart;
        private CheckBoxTS chkGeneralSoftwareGainCorr;
        private LabelTS lblBandLight;
        private LabelTS lblBandDark;
        private LabelTS lblPeakText;
        private ButtonTS btnWizard;
        private ButtonTS btnImportDB;
        private OpenFileDialog openFileDialog1;
        private TabPage tpTests;
        private LabelTS lblPAGainByBand160;
        private LabelTS lblPAGainByBand80;
        private LabelTS lblPAGainByBand60;
        private LabelTS lblPAGainByBand40;
        private LabelTS lblPAGainByBand30;
        private LabelTS lblPAGainByBand10;
        private LabelTS lblPAGainByBand12;
        private LabelTS lblPAGainByBand15;
        private LabelTS lblPAGainByBand17;
        private LabelTS lblPAGainByBand20;
        private TabPage tpPowerAmplifier;
        private ToolTip toolTip1;
        private NumericUpDownTS udPAGain10;
        private NumericUpDownTS udPAGain12;
        private NumericUpDownTS udPAGain15;
        private NumericUpDownTS udPAGain17;
        private NumericUpDownTS udPAGain20;
        private NumericUpDownTS udPAGain30;
        private NumericUpDownTS udPAGain40;
        private NumericUpDownTS udPAGain60;
        private NumericUpDownTS udPAGain80;
        private NumericUpDownTS udPAGain160;
        private GroupBoxTS grpPAGainByBand;
        private ButtonTS btnPAGainCalibration;
        private CheckBoxTS chkGeneralEnableX2;
        private LabelTS lblGeneralX2Delay;
        private NumericUpDownTS udGeneralX2Delay;
        private GroupBoxTS grpPABandOffset;
        private LabelTS lblPABandOffset30;
        private LabelTS lblPABandOffset40;
        private LabelTS lblPABandOffset60;
        private LabelTS lblPABandOffset80;
        private LabelTS lblPABandOffset160;
        private LabelTS lblPABandOffset10;
        private LabelTS lblPABandOffset12;
        private LabelTS lblPABandOffset15;
        private LabelTS lblPABandOffset17;
        private LabelTS lblPABandOffset20;
        private CheckBoxTS chkGeneralATUPresent;
        private ButtonTS btnPAGainReset;
        private ComboBoxTS comboGeneralProcessPriority;
        private GroupBoxTS grpGeneralProcessPriority;
        private GroupBoxTS grpTestTXIMD;
        private ColorButton clrbtnBtnSel;
        private ColorButton clrbtnVFODark;
        private ColorButton clrbtnVFOLight;
        private ColorButton clrbtnBandDark;
        private ColorButton clrbtnBandLight;
        private ColorButton clrbtnPeakText;
        private ColorButton clrbtnBackground;
        private ColorButton clrbtnGrid;
        private ColorButton clrbtnZeroLine;
        private ColorButton clrbtnFilter;
        public ColorButton clrbtnText;
        private ColorButton clrbtnDataLine;
        private ColorButton clrbtnMeterLeft;
        private ColorButton clrbtnMeterRight;
        private ColorButton clrbtnWaterfallLow;
        private LabelTS lblTestIMDPower;
        private NumericUpDownTS udTestIMDPower;
        private CheckBoxTS chkGeneralCustomFilter;
        private NumericUpDownTS udTestIMDFreq1;
        private NumericUpDownTS udTestIMDFreq2;
        private ButtonTS btnTestAudioBalStart;
        private CheckBoxTS chkTestX2Pin1;
        private CheckBoxTS chkTestX2Pin2;
        private CheckBoxTS chkTestX2Pin3;
        private CheckBoxTS chkTestX2Pin4;
        private CheckBoxTS chkTestX2Pin5;
        private CheckBoxTS chkTestX2Pin6;
        private NumericUpDownTS udDisplayAVGTime;
        private LabelTS lblDisplayAVGTime;
        private GroupBoxTS grpTestX2;
        private GroupBoxTS grpTestAudioBalance;
        private NumericUpDownTS udPAADC17;
        private NumericUpDownTS udPAADC15;
        private NumericUpDownTS udPAADC20;
        private NumericUpDownTS udPAADC12;
        private NumericUpDownTS udPAADC10;
        private NumericUpDownTS udPAADC160;
        private NumericUpDownTS udPAADC80;
        private NumericUpDownTS udPAADC60;
        private NumericUpDownTS udPAADC40;
        private NumericUpDownTS udPAADC30;
        private CheckBoxTS chkGeneralUSBPresent;
        private GroupBoxTS grpPATune;
        private LabelTS lblTransmitTunePower;
        private NumericUpDownTS udTXTunePower;
        private GroupBoxTS grpDisplayMultimeter;
        private LabelTS lblDisplayMultiPeakHoldTime;
        private NumericUpDownTS udDisplayMultiPeakHoldTime;
        private NumericUpDownTS udDisplayMultiTextHoldTime;
        private LabelTS lblDisplayMeterTextHoldTime;
        private GroupBoxTS grpGeneralUpdates;
        private CheckBoxTS chkGeneralUpdateRelease;
        private CheckBoxTS chkGeneralUpdateBeta;
        private CheckBoxTS chkGeneralRXOnly;
        private LabelTS lblTestX2;
        private LabelTS lblTestToneFreq2;
        private LabelTS lblTestToneFreq1;
        private TabPage tpCAT;
        private GroupBoxTS grpPTTBitBang;
        private LabelTS lblCATPTTPort;
        private CheckBoxTS chkCATPTT_RTS;
        private CheckBoxTS chkCATPTT_DTR;
        private GroupBoxTS grpCatControlBox;
        private ComboBoxTS comboCATbaud;
        private LabelTS lblCATBaud;
        private CheckBoxTS chkCATEnable;
        private LabelTS lblCATParity;
        private LabelTS lblCATData;
        private LabelTS lblCATStop;
        private ComboBoxTS comboCATparity;
        private ComboBoxTS comboCATdatabits;
        private ComboBoxTS comboCATstopbits;
        private GroupBoxTS grpKBCW;
        private LabelTS lblKBCWDot;
        private LabelTS lblKBCWDash;
        private ComboBoxTS comboKBCWDot;
        private ComboBoxTS comboKBCWDash;
        private GroupBoxTS grpKBRIT;
        private LabelTS lblKBRitUp;
        private LabelTS lblKBRITDown;
        private ComboBoxTS comboKBRITUp;
        private ComboBoxTS comboKBRITDown;
        private GroupBoxTS grpKBXIT;
        private LabelTS lblKBXITUp;
        private LabelTS lblKBXITDown;
        private ComboBoxTS comboKBXITUp;
        private ComboBoxTS comboKBXITDown;
        private CheckBoxTS chkDCBlock;
        private TabPage tpExtCtrl;
        private GroupBoxTS grpExtTX;
        private LabelTS lblExtTXX25;
        private LabelTS lblExtTXX24;
        private LabelTS lblExtTXX23;
        private LabelTS lblExtTXX22;
        private LabelTS lblExtTX2;
        private LabelTS lblExtTX6;
        private LabelTS lblExtTX10;
        private LabelTS lblExtTX12;
        private LabelTS lblExtTX15;
        private LabelTS lblExtTX17;
        private LabelTS lblExtTX20;
        private LabelTS lblExtTX30;
        private LabelTS lblExtTX40;
        private LabelTS lblExtTX60;
        private LabelTS lblExtTX80;
        private LabelTS lblExtTXX2Pins;
        private LabelTS lblExtTXBand;
        private LabelTS lblExtTX160;
        private CheckBoxTS chkExtTX1603;
        private CheckBoxTS chkExtTX1602;
        private CheckBoxTS chkExtTX1601;
        private LabelTS lblExtTXX21;
        private CheckBoxTS chkExtTX1605;
        private CheckBoxTS chkExtTX1604;
        private GroupBoxTS grpExtRX;
        private LabelTS lblExtRXX25;
        private LabelTS lblExtRXX24;
        private LabelTS lblExtRXX23;
        private LabelTS lblExtRXX22;
        private LabelTS lblExtRX2;
        private LabelTS lblExtRX6;
        private LabelTS lblExtRX10;
        private LabelTS lblExtRX12;
        private LabelTS lblExtRX15;
        private LabelTS lblExtRX17;
        private LabelTS lblExtRX20;
        private LabelTS lblExtRX30;
        private LabelTS lblExtRX40;
        private LabelTS lblExtRX60;
        private LabelTS lblExtRX80;
        private LabelTS lblExtRXX2Pins;
        private LabelTS lblExtRXBand;
        private LabelTS lblExtRX160;
        private CheckBoxTS chkExtRX1603;
        private CheckBoxTS chkExtRX1602;
        private CheckBoxTS chkExtRX1601;
        private LabelTS lblExtRXX21;
        private CheckBoxTS chkExtRX1605;
        private CheckBoxTS chkExtRX1604;
        private CheckBoxTS chkExtTX23;
        private CheckBoxTS chkExtTX22;
        private CheckBoxTS chkExtTX21;
        private CheckBoxTS chkExtTX25;
        private CheckBoxTS chkExtTX24;
        private CheckBoxTS chkExtTX63;
        private CheckBoxTS chkExtTX62;
        private CheckBoxTS chkExtTX61;
        private CheckBoxTS chkExtTX65;
        private CheckBoxTS chkExtTX64;
        private CheckBoxTS chkExtTX103;
        private CheckBoxTS chkExtTX102;
        private CheckBoxTS chkExtTX101;
        private CheckBoxTS chkExtTX105;
        private CheckBoxTS chkExtTX104;
        private CheckBoxTS chkExtTX123;
        private CheckBoxTS chkExtTX122;
        private CheckBoxTS chkExtTX121;
        private CheckBoxTS chkExtTX125;
        private CheckBoxTS chkExtTX124;
        private CheckBoxTS chkExtTX153;
        private CheckBoxTS chkExtTX152;
        private CheckBoxTS chkExtTX151;
        private CheckBoxTS chkExtTX155;
        private CheckBoxTS chkExtTX154;
        private CheckBoxTS chkExtTX173;
        private CheckBoxTS chkExtTX172;
        private CheckBoxTS chkExtTX171;
        private CheckBoxTS chkExtTX175;
        private CheckBoxTS chkExtTX174;
        private CheckBoxTS chkExtTX203;
        private CheckBoxTS chkExtTX202;
        private CheckBoxTS chkExtTX201;
        private CheckBoxTS chkExtTX205;
        private CheckBoxTS chkExtTX204;
        private CheckBoxTS chkExtTX303;
        private CheckBoxTS chkExtTX302;
        private CheckBoxTS chkExtTX301;
        private CheckBoxTS chkExtTX305;
        private CheckBoxTS chkExtTX304;
        private CheckBoxTS chkExtTX403;
        private CheckBoxTS chkExtTX402;
        private CheckBoxTS chkExtTX401;
        private CheckBoxTS chkExtTX405;
        private CheckBoxTS chkExtTX404;
        private CheckBoxTS chkExtTX603;
        private CheckBoxTS chkExtTX602;
        private CheckBoxTS chkExtTX601;
        private CheckBoxTS chkExtTX605;
        private CheckBoxTS chkExtTX604;
        private CheckBoxTS chkExtTX803;
        private CheckBoxTS chkExtTX802;
        private CheckBoxTS chkExtTX801;
        private CheckBoxTS chkExtTX805;
        private CheckBoxTS chkExtTX804;
        private CheckBoxTS chkExtRX23;
        private CheckBoxTS chkExtRX22;
        private CheckBoxTS chkExtRX21;
        private CheckBoxTS chkExtRX25;
        private CheckBoxTS chkExtRX24;
        private CheckBoxTS chkExtRX63;
        private CheckBoxTS chkExtRX62;
        private CheckBoxTS chkExtRX61;
        private CheckBoxTS chkExtRX65;
        private CheckBoxTS chkExtRX64;
        private CheckBoxTS chkExtRX103;
        private CheckBoxTS chkExtRX102;
        private CheckBoxTS chkExtRX101;
        private CheckBoxTS chkExtRX105;
        private CheckBoxTS chkExtRX104;
        private CheckBoxTS chkExtRX123;
        private CheckBoxTS chkExtRX122;
        private CheckBoxTS chkExtRX121;
        private CheckBoxTS chkExtRX125;
        private CheckBoxTS chkExtRX124;
        private CheckBoxTS chkExtRX153;
        private CheckBoxTS chkExtRX152;
        private CheckBoxTS chkExtRX151;
        private CheckBoxTS chkExtRX155;
        private CheckBoxTS chkExtRX154;
        private CheckBoxTS chkExtRX173;
        private CheckBoxTS chkExtRX172;
        private CheckBoxTS chkExtRX171;
        private CheckBoxTS chkExtRX175;
        private CheckBoxTS chkExtRX174;
        private CheckBoxTS chkExtRX203;
        private CheckBoxTS chkExtRX202;
        private CheckBoxTS chkExtRX201;
        private CheckBoxTS chkExtRX205;
        private CheckBoxTS chkExtRX204;
        private CheckBoxTS chkExtRX303;
        private CheckBoxTS chkExtRX302;
        private CheckBoxTS chkExtRX301;
        private CheckBoxTS chkExtRX305;
        private CheckBoxTS chkExtRX304;
        private CheckBoxTS chkExtRX403;
        private CheckBoxTS chkExtRX402;
        private CheckBoxTS chkExtRX401;
        private CheckBoxTS chkExtRX405;
        private CheckBoxTS chkExtRX404;
        private CheckBoxTS chkExtRX603;
        private CheckBoxTS chkExtRX602;
        private CheckBoxTS chkExtRX601;
        private CheckBoxTS chkExtRX605;
        private CheckBoxTS chkExtRX604;
        private CheckBoxTS chkExtRX803;
        private CheckBoxTS chkExtRX802;
        private CheckBoxTS chkExtRX801;
        private CheckBoxTS chkExtRX805;
        private CheckBoxTS chkExtRX804;
        private ButtonTS btnCATTest;
        private TabControl tcAudio;
        private NumericUpDownTS udAudioLineIn1;
        private ButtonTS btnAudioVoltTest1;
        private NumericUpDownTS udAudioVoltage1;
        public GroupBoxTS grpAudioDetails1;
        private ComboBoxTS comboAudioTransmit1;
        private LabelTS lblAudioMixer1;
        public LabelTS lblAudioOutput1;
        private ComboBoxTS comboAudioOutput1;
        public LabelTS lblAudioInput1;
        public LabelTS lblAudioDriver1;
        private ComboBoxTS comboAudioInput1;
        private ComboBoxTS comboAudioDriver1;
        private ComboBoxTS comboAudioMixer1;
        private LabelTS lblAudioTransmit1;
        private LabelTS lblAudioReceive1;
        private ComboBoxTS comboAudioReceive1;
        private NumericUpDownTS udAudioLatency1;
        private GroupBoxTS grpAudioCard;
        private ComboBoxTS comboAudioSoundCard;
        private ComboBoxTS comboAudioBuffer1;
        private ComboBoxTS comboAudioSampleRate1;
        private GroupBoxTS grpAudioLineInGain1;
        private GroupBoxTS grpAudioLatency1;
        private CheckBoxTS chkAudioLatencyManual1;
        private GroupBoxTS grpAudioBufferSize1;
        private GroupBoxTS grpAudioSampleRate1;
        private GroupBoxTS grpAudioDetails2;
        public LabelTS lblAudioOutput2;
        private ComboBoxTS comboAudioOutput2;
        public LabelTS lblAudioInput2;
        public LabelTS lblAudioDriver2;
        private ComboBoxTS comboAudioInput2;
        private ComboBoxTS comboAudioDriver2;
        private NumericUpDownTS udAudioLatency2;
        private ComboBoxTS comboAudioBuffer2;
        private ComboBoxTS comboAudioSampleRate2;
        private GroupBoxTS grpAudioMicInGain1;
        private NumericUpDownTS udAudioMicGain1;
        private GroupBoxTS grpAudioBuffer2;
        private GroupBoxTS grpAudioSampleRate2;
        private GroupBoxTS grpAudioLatency2;
        private CheckBoxTS chkAudioLatencyManual2;
        private GroupBoxTS grpAudioVolts1;
        private ComboBoxTS comboCATRigType;
        private ComboBoxTS comboDisplayLabelAlign;
        private LabelTS lblDisplayAlign;
        private LabelTS lblCATRigType;
        private TabPage tpAudioCard1;
        private TabPage tpDSPKeyer;
        private NumericUpDownTS udCWKeyerRamp;
        private LabelTS lblCWRamp;
        private CheckBoxTS chkCWKeyerIambic;
        private GroupBoxTS grpDSPKeyerSignalShaping;
        private NumericUpDownTS udCWKeyerWeight;
        private LabelTS lblCWWeight;
        private GroupBoxTS grpDSPKeyerOptions;
        private GroupBoxTS grpDSPKeyerSemiBreakIn;
        private GroupBoxTS grpDSPCWPitch;
        private LabelTS lblDSPCWPitchFreq;
        private NumericUpDownTS udDSPCWPitch;
        private TabControl tcDSP;
        private TabPage tpDSPImageReject;
        private GroupBoxTS grpDSPImageRejectTX;
        private LabelTS lblDSPGainValTX;
        private LabelTS lblDSPPhaseValTX;
        private NumericUpDownTS udDSPImageGainTX;
        private NumericUpDownTS udDSPImagePhaseTX;
        private LabelTS lblDSPImageGainTX;
        private TrackBarTS tbDSPImagePhaseTX;
        private LabelTS lblDSPImagePhaseTX;
        private TrackBarTS tbDSPImageGainTX;
        private TabPage tpDSPOptions;
        private GroupBoxTS grpDSPBufferSize;
        private GroupBoxTS grpDSPNB;
        private NumericUpDownTS udDSPNB;
        private LabelTS lblDSPNBThreshold;
        private GroupBoxTS grpDSPLMSNR;
        private LabelTS lblLMSNRgain;
        private NumericUpDownTS udLMSNRgain;
        private NumericUpDownTS udLMSNRdelay;
        private LabelTS lblLMSNRdelay;
        private NumericUpDownTS udLMSNRtaps;
        private LabelTS lblLMSNRtaps;
        private GroupBoxTS grpDSPLMSANF;
        private LabelTS lblLMSANFgain;
        private NumericUpDownTS udLMSANFgain;
        private LabelTS lblLMSANFdelay;
        private NumericUpDownTS udLMSANFdelay;
        private LabelTS lblLMSANFTaps;
        private NumericUpDownTS udLMSANFtaps;
        private GroupBoxTS grpDSPAGC;
        private LabelTS lblDSPAGCMaxGain;
        private NumericUpDownTS udDSPAGCMaxGaindB;
        private NumericUpDownTS udDSPAGCFixedGaindB;
        private LabelTS lblDSPAGCFixed;
        private GroupBoxTS grpDSPWindow;
        private ComboBoxTS comboDSPWindow;
        private GroupBoxTS grpDSPNB2;
        private LabelTS lblDSPNB2Threshold;
        private NumericUpDownTS udDSPNB2;
        private CheckBoxTS chkCWKeyerRevPdl;
        private CheckBoxTS chkExtEnable;
        private CheckBoxTS chkExtRX26;
        private CheckBoxTS chkExtRX66;
        private CheckBoxTS chkExtRX106;
        private CheckBoxTS chkExtRX126;
        private CheckBoxTS chkExtRX156;
        private CheckBoxTS chkExtRX176;
        private CheckBoxTS chkExtRX206;
        private CheckBoxTS chkExtRX306;
        private CheckBoxTS chkExtRX406;
        private CheckBoxTS chkExtRX606;
        private CheckBoxTS chkExtRX806;
        private CheckBoxTS chkExtRX1606;
        private LabelTS lblExtRXX26;
        private LabelTS lblExtTXX26;
        private CheckBoxTS chkExtTX26;
        private CheckBoxTS chkExtTX66;
        private CheckBoxTS chkExtTX106;
        private CheckBoxTS chkExtTX126;
        private CheckBoxTS chkExtTX156;
        private CheckBoxTS chkExtTX176;
        private CheckBoxTS chkExtTX206;
        private CheckBoxTS chkExtTX306;
        private CheckBoxTS chkExtTX406;
        private CheckBoxTS chkExtTX606;
        private CheckBoxTS chkExtTX806;
        private CheckBoxTS chkExtTX1606;
        private GroupBoxTS grpTXProfile;
        private ButtonTS btnTXProfileSave;
        private ComboBoxTS comboTXProfileName;
        private ButtonTS btnTXProfileDelete;
        private System.Windows.Forms.Timer timer_sweep;
        private LabelTS lblTestGenLow;
        private LabelTS lblTestGenHigh;
        private LabelTS lblTestGenHzSec;
        private LabelTS lblDSPAGCAttack;
        private LabelTS lblDSPAGCDecay;
        private NumericUpDownTS udDSPAGCAttack;
        private LabelTS lblDSPAGCSlope;
        private NumericUpDownTS udDSPAGCDecay;
        private NumericUpDownTS udDSPAGCSlope;
        private NumericUpDownTS udDSPALCThreshold;
        private NumericUpDownTS udDSPALCSlope;
        private NumericUpDownTS udDSPALCDecay;
        private LabelTS lblDSPALCSlope;
        private NumericUpDownTS udDSPALCAttack;
        private LabelTS lblDSPALCDecay;
        private LabelTS lblDSPALCAttack;
        private LabelTS lblDSPALCThreshold;
        private GroupBoxTS grpDSPALC;
        private GroupBoxTS grpDSPLeveler;
        private NumericUpDownTS udDSPLevelerThreshold;
        private NumericUpDownTS udDSPLevelerSlope;
        private NumericUpDownTS udDSPLevelerDecay;
        private LabelTS lblDSPLevelerSlope;
        private NumericUpDownTS udDSPLevelerAttack;
        private LabelTS lblDSPLevelerDecay;
        private LabelTS lblDSPLevelerAttack;
        private LabelTS lblDSPLevelerThreshold;
        private GroupBoxTS grpTXNoiseGate;
        private LabelTS lblTXNoiseGateThreshold;
        private NumericUpDownTS udTXNoiseGate;
        private CheckBoxTS chkTXNoiseGateEnabled;
        private TabPage tpDSPAGCALC;
        private TrackBarTS tbDSPLevelerHangThreshold;
        private LabelTS lblDSPLevelerHangThreshold;
        private LabelTS lblDSPALCHangThreshold;
        private TrackBarTS tbDSPALCHangThreshold;
        private NumericUpDownTS udDSPLevelerHangTime;
        private LabelTS lblDSPLevelerHangTime;
        private NumericUpDownTS udDSPALCHangTime;
        private LabelTS lblDSPALCHangTime;
        private GroupBoxTS grpTXVOX;
        private LabelTS lblTXVOXThreshold;
        private NumericUpDownTS udTXVOXThreshold;
        private CheckBoxTS chkTXVOXEnabled;
        private LabelTS lblTXVOXHangTime;
        private NumericUpDownTS udTXVOXHangTime;
        public TrackBarTS tbDSPAGCHangThreshold;
        private LabelTS lblDSPAGCHangThreshold;
        private LabelTS lblDSPAGCHangTime;
        private NumericUpDownTS udDSPAGCHangTime;
        private CheckBoxTS chkDSPLevelerEnabled;
        private ButtonTS btnImpulse;
        private NumericUpDownTS udImpulseNum;
        private GroupBoxTS grpTXMonitor;
        private LabelTS lblTXAF;
        private NumericUpDownTS udTXAF;
        private GroupBoxTS grpGeneralModel;
        private TabControl tcGeneral;
        private TabPage tpGeneralHardware;
        private TabPage tpGeneralOptions;
        private TabPage tpGeneralCalibration;
        private RadioButtonTS radGenModelSDR1000;
        private RadioButtonTS radGenModelSoftRock40;
        private RadioButtonTS radGenModelDemoNone;
        private NumericUpDownTS udGeneralCalFreq1;
        private NumericUpDownTS udGeneralCalFreq3;
        private NumericUpDownTS udGeneralCalFreq2;
        private NumericUpDownTS udSoftRockCenterFreq;
        private GroupBoxTS grpHWSoftRock;
        private TabPage tpVAC;
        public CheckBoxTS chkAudioEnableVAC;
        private GroupBoxTS grpAudio2Stereo;
        private GroupBoxTS grpBoxTS1;
        private TrackBarTS tkbarTestGenFreq;
        private LabelTS lblKeyerConnPrimary;
        private ComboBoxTS comboKeyerConnPrimary;
        private ComboBoxTS comboKeyerConnKeyLine;
        private ComboBoxTS comboKeyerConnSecondary;
        private LabelTS lblKeyerConnSecondary;
        private ComboBoxTS comboKeyerConnPTTLine;
        private LabelTS lblKeyerConnPTTLine;
        private LabelTS lblKeyerConnKeyLine;
        private LabelTS lblCATPort;
        private ComboBoxTS comboCATPort;
        private ComboBoxTS comboCATPTTPort;
        private CheckBoxTS chkCATPTTEnabled;
        private CheckBoxTS checkboxTXImagCal;
        private GroupBoxTS grpAudioChannels;
        private ComboBoxTS comboAudioChannels1;
        private GroupBoxTS grpAudioVACGain;
        private NumericUpDownTS udAudioVACGainRX;
        public LabelTS lblAudioVACGainRX;
        public LabelTS lblAudioVACGainTX;
        private NumericUpDownTS udAudioVACGainTX;
        private LabelTS lblTestGenScale;
        private GroupBoxTS grpGenTuningOptions;
        private GroupBoxTS grpAudioVACAutoEnable;
        private CheckBoxTS chkAudioVACAutoEnable;
        private CheckBoxTS chkSpectrumPolyphase;
        private GroupBoxTS grpImpulseTest;
        private GroupBoxTS grpDisplayScopeMode;
        private NumericUpDownTS udDisplayScopeTime;
        private LabelTS lblDisplayScopeTime;
        private NumericUpDownTS udDisplayMeterAvg;
        private LabelTS lblDisplayMeterAvg;
        private GroupBoxTS grpDisplayPolyPhase;
        private ComboBoxTS comboDisplayDriver;
        private GroupBoxTS grpDisplayDriverEngine;
        private GroupBoxTS grpGenAutoMute;
        private CheckBoxTS chkGenAutoMute;
        private PowerSDR.ColorButton clrbtnOutOfBand;
        private LabelTS lblOutOfBand;
        private LabelTS lblGenSoftRockCenterFreq;
        private LabelTS lblTestSigGenFreqCallout;
        private NumericUpDownTS udTestGenHzSec;
        private NumericUpDownTS udTestGenHigh;
        private NumericUpDownTS udTestGenLow;
        private ButtonTS btnTestGenSweep;
        private NumericUpDownTS udTestGenScale;
        private CheckBoxTS chkAudio2Stereo;
        private GroupBoxTS grpTXAM;
        private LabelTS lblTXAMCarrierLevel;
        private NumericUpDownTS udTXAMCarrierLevel;
        private GroupBoxTS grpOptQuickQSY;
        private CheckBoxTS chkOptQuickQSY;
        private CheckBoxTS chkOptAlwaysOnTop;
        private NumericUpDownTS udOptClickTuneOffsetDIGL;
        private NumericUpDownTS udOptClickTuneOffsetDIGU;
        private LabelTS lblOptClickTuneDIGL;
        private LabelTS lblOptClickTuneDIGU;
        private GroupBoxTS grpAudioMicBoost;
        private CheckBoxTS chkAudioMicBoost;
        private GroupBoxTS grpOptFilterControls;
        private LabelTS lblOptMaxFilter;
        private NumericUpDownTS udOptMaxFilterWidth;
        private LabelTS lblOptWidthSliderMode;
        private ComboBoxTS comboOptFilterWidthMode;
        private NumericUpDownTS udOptMaxFilterShift;
        private LabelTS lblOptMaxFilterShift;
        private CheckBoxTS chkOptFilterSaveChanges;
        private CheckBoxTS chkOptEnableKBShortcuts;
        private TabControl tcAppearance;
        private TabPage tpAppearanceGeneral;
        private TabPage tpAppearanceDisplay;
        private TabPage tpAppearanceMeter;
        private GroupBoxTS grpAppearanceVFO;
        private LabelTS lblVFOPowerOn;
        private LabelTS lblVFOPowerOff;
        private GroupBoxTS grpAppearanceBand;
        private TabPage tpFilters;
        private LabelTS lblDefaultLowCut;
        private NumericUpDownTS udFilterDefaultLowCut;
        private CheckBoxTS chkVFOSmallLSD;
        private PowerSDR.ColorButton clrbtnVFOSmallColor;
        private PowerSDR.ColorButton clrbtnBandBackground;
        private LabelTS lblBandBackground;
        private PowerSDR.ColorButton clrbtnVFOBackground;
        private LabelTS lblVFOBackground;
        private GroupBoxTS grpDisplayPeakCursor;
        private PowerSDR.ColorButton clrbtnPeakBackground;
        private LabelTS lblPeakBackground;
        private PowerSDR.ColorButton clrbtnMeterBackground;
        private LabelTS lblMeterBackground;
        private PowerSDR.ColorButton clrbtnTXFilter;
        private GroupBoxTS grpAppPanadapter;
        private PowerSDR.ColorButton clrbtnBandEdge;
        private LabelTS lblBandEdge;
        private CheckBoxTS chkShowFreqOffset;
        public ComboBoxTS comboMeterType;
        private PowerSDR.ColorButton clrbtnMeterEdgeBackground;
        private PowerSDR.ColorButton clrbtnMeterEdgeHigh;
        private PowerSDR.ColorButton clrbtnMeterEdgeLow;
        private GroupBoxTS grpGenCalRXImage;
        private LabelTS lblGenCalRXImageFreq;
        private GroupBoxTS grpGenCalLevel;
        private LabelTS lblGenCalLevelFreq;
        private GroupBoxTS grpKeyerConnections;
        private LabelTS lblVFOSmallColor;
        private LabelTS lblTXFilterColor;
        private LabelTS lblMeterType;
        private CheckBoxTS chekTestIMD;
        private GroupBoxTS grpMeterEdge;
        private LabelTS lblMeterEdgeBackground;
        private LabelTS lblMeterEdgeHigh;
        private LabelTS lblMeterEdgeLow;
        private PowerSDR.ColorButton clrbtnEdgeIndicator;
        private LabelTS labelTS1;
        private LabelTS lblMeterDigitalText;
        private PowerSDR.ColorButton clrbtnMeterDigText;
        private LabelTS labelTS2;
        private PowerSDR.ColorButton clrbtnMeterDigBackground;
        private PowerSDR.ColorButton clrbtnSubRXFilter;
        private LabelTS lblSubRXFilterColor;
        private PowerSDR.ColorButton clrbtnSubRXZero;
        private LabelTS lblSubRXZeroLine;
        private CheckBoxTS chkCWKeyerMode;
        private CheckBoxTS chkCWBreakInEnabled;
        private LabelTS lblCWBreakInDelay;
        private NumericUpDownTS udCWBreakInDelay;
        private GroupBoxTS grpOptMisc;
        private CheckBoxTS chkDisableToolTips;
        private RichTextBox rtxtPACalReq;
        private NumericUpDownTS udDisplayWaterfallAvgTime;
        private LabelTS lblDisplayWaterfallAverageTime;
        private NumericUpDownTS udDisplayWaterfallUpdatePeriod;
        private LabelTS lblDisplayWaterfallUpdatePeriod;
        private CheckBoxTS chkSnapClickTune;
        private RadioButtonTS radPACalAllBands;
        private CheckBoxTS chkPA160;
        private CheckBoxTS chkPA80;
        private CheckBoxTS chkPA60;
        private CheckBoxTS chkPA40;
        private CheckBoxTS chkPA30;
        private CheckBoxTS chkPA20;
        private CheckBoxTS chkPA17;
        private CheckBoxTS chkPA15;
        private CheckBoxTS chkPA12;
        private CheckBoxTS chkPA10;
        private RadioButtonTS radPACalSelBands;
        private NumericUpDownTS udPACalPower;
        private CheckBoxTS chkZeroBeatRIT;
        private CheckBoxTS chkPANewCal;
        private LabelTS lblPACalTarget;
        private CheckBoxTS chkAudioExpert;
        private ComboBoxTS cmboSigGenRXMode;
        private LabelTS lblSigGenRXMode;
        private RadioButtonTS radSigGenRXInput;
        private RadioButtonTS radSigGenRXOutput;
        private GroupBoxTS grpSigGenReceive;
        private GroupBoxTS grpSigGenTransmit;
        private LabelTS lblSigGenTXMode;
        private ComboBoxTS cmboSigGenTXMode;
        private RadioButtonTS radSigGenTXInput;
        private RadioButtonTS radSigGenTXOutput;
        private NumericUpDownTS udMeterDigitalDelay;
        private LabelTS lblMultimeterDigitalDelay;
        private RadioButtonTS radGenModelFLEX5000;
        private GroupBoxTS grpGeneralHardwareSDR1000;
        private GroupBoxTS grpGeneralHardwareFLEX5000;
        private CheckBoxTS chkPA6;
        private CheckBoxTS chkMouseTuneStep;
        private LabelTS lblModel;
        private LabelTS lblSerialNum;
        private LabelTS lblTRXRev;
        private LabelTS lblPARev;
        private LabelTS lblRFIORev;
        private LabelTS lblATURev;
        private LabelTS lblRX2Rev;
        private LabelTS lblFirmwareRev;
        private CheckBoxTS ckEnableSigGen;
        private CheckBoxTS chkGenDDSExpert;
        private CheckBoxTS chkBoxJanusOzyControl;
        private CheckBoxTS chkCalExpert;
        private CheckBoxTS chkDSPImageExpert;
        private CheckBoxTS chkGenAllModeMicPTT;
        private CheckBoxTS chkDigUIsUSB;
        private GroupBoxTS grpGenCustomTitleText;
        private TextBoxTS txtGenCustomTitle;
        private CheckBoxTS chkGenFLEX5000ExtRef;
        private NumericUpDownTS udLMSNRLeak;
        private NumericUpDownTS udLMSANFLeak;
        private LabelTS lblLMSNRLeak;
        private LabelTS lblLMSANFLeak;
        private MainMenu mainMenu1;
        private CheckBoxTS chkKWAI;
        private CheckBoxTS chkSplitOff;
        private CheckBoxTS chkEnableRFEPATR;
        private CheckBoxTS chkVACAllowBypass;
        private CheckBoxTS chkDSPTXMeterPeak;
        private CheckBoxTS chkVACCombine;
        private CheckBoxTS chkCWAutoSwitchMode;
        private CheckBoxTS chkSigGenRX2;
        private LabelTS lblGenBackground;
        private PowerSDR.ColorButton clrbtnGenBackground;
        private ComboBoxTS comboTXTUNMeter;
        private LabelTS lblTXTUNMeter;
        private ButtonTS btnResetDB;
        private GroupBoxTS grpDSPBufPhone;
        private GroupBoxTS grpDSPBufCW;
        private ComboBoxTS comboDSPPhoneTXBuf;
        private ComboBoxTS comboDSPPhoneRXBuf;
        private ComboBoxTS comboDSPCWTXBuf;
        private ComboBoxTS comboDSPCWRXBuf;
        private ComboBoxTS comboDSPDigTXBuf;
        private ComboBoxTS comboDSPDigRXBuf;
        private LabelTS lblDSPPhoneBufferTX;
        private LabelTS lblDSPPhoneBufferRX;
        private LabelTS lblDSPCWBufferRX;
        private LabelTS lblDSPCWBufferTX;
        private GroupBoxTS grpDSPBufDig;
        private LabelTS lblDSPDigBufferRX;
        private LabelTS lblDSPDigBufferTX;
        private CheckBoxTS chkDisplayMeterShowDecimal;
        private GroupBoxTS grpRTTYOffset;
        private CheckBoxTS chkRTTYOffsetEnableA;
        private CheckBoxTS chkRTTYOffsetEnableB;
        private NumericUpDownTS udRTTYL;
        private NumericUpDownTS udRTTYU;
        private LabelTS labelTS3;
        private LabelTS labelTS4;
        private TabPage tpRX2;
        private CheckBoxTS chkRX2AutoMuteTX;
        private GroupBoxTS grpDirectIQOutput;
        private CheckBoxTS chkAudioCorrectIQ;
        private CheckBoxTS chkAudioIQtoVAC;
        private CheckBoxTS chkRX2AutoMuteRX1OnVFOBTX;
        private ListBox lstTXProfileDef;
        private GroupBoxTS grpTXProfileDef;
        private CheckBoxTS chkTXExpert;
        private ButtonTS btnTXProfileDefImport;
        private NumericUpDownTS udF3KFanTempThresh;
        private LabelTS lblF3KFanTempThresh;
        private LabelTS lblGenTX1Delay;
        private CheckBoxTS chkGenTX1Delay;
        private NumericUpDownTS udGenTX1Delay;
        private CheckBoxTS chkDisplayPanFill;
        private GroupBoxTS grpAppSkins;
        private ComboBoxTS comboAppSkin;
        private ButtonTS btnSkinExport;
        private CheckBoxTS chkCWDisableUI;
        private CheckBoxTS chkAudioRX2toVAC;
        private CheckBoxTS chkGenOptionsShowATUPopup;
        private TabPage tpGeneralNavigation;
        private GroupBoxTS grpOptSpaceNav;
        private CheckBoxTS chkSpaceNavFlyPanadapter;
        private CheckBoxTS chkSpaceNavControlVFOs;
        private TrackBarTS tbRX1FilterAlpha;
        private LabelTS lblRX1FilterAlpha;
        private NumericUpDownTS udTXNoiseGateAttenuate;
        private LabelTS lblTXNoiseGateAttenuate;
        private LabelTS lblMultiRXFilterAlpha;
        private TrackBarTS tbMultiRXFilterAlpha;
        private CheckBoxTS chkWheelTuneVFOB;
        private CheckBoxTS chkCWKeyerMonoCable;
        private ButtonTS btnExportDB;
        private SaveFileDialog saveFileDialog1;
        private RadioButtonTS radGenModelHermes;
        private RadioButtonTS radGenModelHPSDR;
        private CheckBoxTS chkAlexPresent;
        private CheckBoxTS chkExcaliburPresent;
        private CheckBoxTS chkPennyPresent;
        private CheckBoxTS chkMercuryPresent;
        private CheckBoxTS chkJanusPresent;
        private GroupBoxTS grpHPSDRFreqCalDbg;
        private LabelTS labelTS10;
        private NumericUpDownTS udHPSDRFreqCorrectFactor;
        private Button btnHPSDRFreqCalReset;
        private TabPage tpPennyCtrl;
        private TabPage tpHPSDR;
        private TabPage tpAlexControl;
        private GroupBoxTS grpPennyExtCtrl;
        private LabelTS lblHFTxControl;
        private LabelTS lblHFRxControl;
        private LabelTS labelTS43;
        private LabelTS labelTS23;
        private LabelTS labelTS24;
        private LabelTS labelTS25;
        private LabelTS labelTS26;
        private LabelTS labelTS27;
        private LabelTS labelTS42;
        private LabelTS labelTS44;
        private LabelTS labelTS45;
        private LabelTS labelTS46;
        private LabelTS labelTS47;
        private LabelTS labelTS48;
        private LabelTS labelTS49;
        private LabelTS labelTS51;
        private LabelTS labelTS40;
        private CheckBoxTS chkPenOCrcv1601;
        private CheckBoxTS chkPenOCxmit1607;
        private CheckBoxTS chkPenOCxmit1606;
        private CheckBoxTS chkPenOCxmit1605;
        private CheckBoxTS chkPenOCxmit1604;
        private CheckBoxTS chkPenOCxmit1603;
        private CheckBoxTS chkPenOCxmit1602;
        private CheckBoxTS chkPenOCxmit1601;
        private CheckBoxTS chkPenOCrcv1607;
        private CheckBoxTS chkPenOCrcv1606;
        private CheckBoxTS chkPenOCrcv1605;
        private CheckBoxTS chkPenOCrcv1604;
        private CheckBoxTS chkPenOCrcv1603;
        private CheckBoxTS chkPenOCrcv1602;
        private CheckBoxTS chkPenOCxmit807;
        private CheckBoxTS chkPenOCxmit806;
        private CheckBoxTS chkPenOCxmit805;
        private CheckBoxTS chkPenOCxmit804;
        private CheckBoxTS chkPenOCxmit803;
        private CheckBoxTS chkPenOCxmit802;
        private CheckBoxTS chkPenOCxmit801;
        private CheckBoxTS chkPenOCrcv807;
        private CheckBoxTS chkPenOCrcv806;
        private CheckBoxTS chkPenOCrcv805;
        private CheckBoxTS chkPenOCrcv804;
        private CheckBoxTS chkPenOCrcv803;
        private CheckBoxTS chkPenOCrcv802;
        private CheckBoxTS chkPenOCrcv801;
        private CheckBoxTS chkPenOCxmit207;
        private CheckBoxTS chkPenOCxmit206;
        private CheckBoxTS chkPenOCxmit205;
        private CheckBoxTS chkPenOCxmit204;
        private CheckBoxTS chkPenOCxmit203;
        private CheckBoxTS chkPenOCxmit202;
        private CheckBoxTS chkPenOCxmit201;
        private CheckBoxTS chkPenOCrcv207;
        private CheckBoxTS chkPenOCrcv206;
        private CheckBoxTS chkPenOCrcv205;
        private CheckBoxTS chkPenOCrcv204;
        private CheckBoxTS chkPenOCrcv203;
        private CheckBoxTS chkPenOCrcv202;
        private CheckBoxTS chkPenOCrcv201;
        private CheckBoxTS chkPenOCxmit307;
        private CheckBoxTS chkPenOCxmit306;
        private CheckBoxTS chkPenOCxmit305;
        private CheckBoxTS chkPenOCxmit304;
        private CheckBoxTS chkPenOCxmit303;
        private CheckBoxTS chkPenOCxmit302;
        private CheckBoxTS chkPenOCxmit301;
        private CheckBoxTS chkPenOCrcv307;
        private CheckBoxTS chkPenOCrcv306;
        private CheckBoxTS chkPenOCrcv305;
        private CheckBoxTS chkPenOCrcv304;
        private CheckBoxTS chkPenOCrcv303;
        private CheckBoxTS chkPenOCrcv302;
        private CheckBoxTS chkPenOCrcv301;
        private CheckBoxTS chkPenOCxmit407;
        private CheckBoxTS chkPenOCxmit406;
        private CheckBoxTS chkPenOCxmit405;
        private CheckBoxTS chkPenOCxmit404;
        private CheckBoxTS chkPenOCxmit403;
        private CheckBoxTS chkPenOCxmit402;
        private CheckBoxTS chkPenOCxmit401;
        private CheckBoxTS chkPenOCrcv407;
        private CheckBoxTS chkPenOCrcv406;
        private CheckBoxTS chkPenOCrcv405;
        private CheckBoxTS chkPenOCrcv404;
        private CheckBoxTS chkPenOCrcv403;
        private CheckBoxTS chkPenOCrcv402;
        private CheckBoxTS chkPenOCrcv401;
        private CheckBoxTS chkPenOCxmit607;
        private CheckBoxTS chkPenOCxmit606;
        private CheckBoxTS chkPenOCxmit605;
        private CheckBoxTS chkPenOCxmit604;
        private CheckBoxTS chkPenOCxmit603;
        private CheckBoxTS chkPenOCxmit602;
        private CheckBoxTS chkPenOCxmit601;
        private CheckBoxTS chkPenOCrcv607;
        private CheckBoxTS chkPenOCrcv606;
        private CheckBoxTS chkPenOCrcv605;
        private CheckBoxTS chkPenOCrcv604;
        private CheckBoxTS chkPenOCrcv603;
        private CheckBoxTS chkPenOCrcv602;
        private CheckBoxTS chkPenOCrcv601;
        private CheckBoxTS chkPenOCxmit27;
        private CheckBoxTS chkPenOCxmit26;
        private CheckBoxTS chkPenOCxmit25;
        private CheckBoxTS chkPenOCxmit24;
        private CheckBoxTS chkPenOCxmit23;
        private CheckBoxTS chkPenOCxmit22;
        private CheckBoxTS chkPenOCxmit21;
        private CheckBoxTS chkPenOCrcv27;
        private CheckBoxTS chkPenOCrcv26;
        private CheckBoxTS chkPenOCrcv25;
        private CheckBoxTS chkPenOCrcv24;
        private CheckBoxTS chkPenOCrcv23;
        private CheckBoxTS chkPenOCrcv22;
        private CheckBoxTS chkPenOCrcv21;
        private CheckBoxTS chkPenOCxmit67;
        private CheckBoxTS chkPenOCxmit66;
        private CheckBoxTS chkPenOCxmit65;
        private CheckBoxTS chkPenOCxmit64;
        private CheckBoxTS chkPenOCxmit63;
        private CheckBoxTS chkPenOCxmit62;
        private CheckBoxTS chkPenOCxmit61;
        private CheckBoxTS chkPenOCrcv67;
        private CheckBoxTS chkPenOCrcv66;
        private CheckBoxTS chkPenOCrcv65;
        private CheckBoxTS chkPenOCrcv64;
        private CheckBoxTS chkPenOCrcv63;
        private CheckBoxTS chkPenOCrcv62;
        private CheckBoxTS chkPenOCrcv61;
        private CheckBoxTS chkPenOCxmit107;
        private CheckBoxTS chkPenOCxmit106;
        private CheckBoxTS chkPenOCxmit105;
        private CheckBoxTS chkPenOCxmit104;
        private CheckBoxTS chkPenOCxmit103;
        private CheckBoxTS chkPenOCxmit102;
        private CheckBoxTS chkPenOCxmit101;
        private CheckBoxTS chkPenOCrcv107;
        private CheckBoxTS chkPenOCrcv106;
        private CheckBoxTS chkPenOCrcv105;
        private CheckBoxTS chkPenOCrcv104;
        private CheckBoxTS chkPenOCrcv103;
        private CheckBoxTS chkPenOCrcv102;
        private CheckBoxTS chkPenOCrcv101;
        private CheckBoxTS chkPenOCxmit127;
        private CheckBoxTS chkPenOCxmit126;
        private CheckBoxTS chkPenOCxmit125;
        private CheckBoxTS chkPenOCxmit124;
        private CheckBoxTS chkPenOCxmit123;
        private CheckBoxTS chkPenOCxmit122;
        private CheckBoxTS chkPenOCxmit121;
        private CheckBoxTS chkPenOCrcv127;
        private CheckBoxTS chkPenOCrcv126;
        private CheckBoxTS chkPenOCrcv125;
        private CheckBoxTS chkPenOCrcv124;
        private CheckBoxTS chkPenOCrcv123;
        private CheckBoxTS chkPenOCrcv122;
        private CheckBoxTS chkPenOCrcv121;
        private CheckBoxTS chkPenOCxmit157;
        private CheckBoxTS chkPenOCxmit156;
        private CheckBoxTS chkPenOCxmit155;
        private CheckBoxTS chkPenOCxmit154;
        private CheckBoxTS chkPenOCxmit153;
        private CheckBoxTS chkPenOCxmit152;
        private CheckBoxTS chkPenOCxmit151;
        private CheckBoxTS chkPenOCrcv157;
        private CheckBoxTS chkPenOCrcv156;
        private CheckBoxTS chkPenOCrcv155;
        private CheckBoxTS chkPenOCrcv154;
        private CheckBoxTS chkPenOCrcv153;
        private CheckBoxTS chkPenOCrcv152;
        private CheckBoxTS chkPenOCrcv151;
        private CheckBoxTS chkPenOCxmit177;
        private CheckBoxTS chkPenOCxmit176;
        private CheckBoxTS chkPenOCxmit175;
        private CheckBoxTS chkPenOCxmit174;
        private CheckBoxTS chkPenOCxmit173;
        private CheckBoxTS chkPenOCxmit172;
        private CheckBoxTS chkPenOCxmit171;
        private CheckBoxTS chkPenOCrcv177;
        private CheckBoxTS chkPenOCrcv176;
        private CheckBoxTS chkPenOCrcv175;
        private CheckBoxTS chkPenOCrcv174;
        private CheckBoxTS chkPenOCrcv173;
        private CheckBoxTS chkPenOCrcv172;
        private CheckBoxTS chkPenOCrcv171;
        private LabelTS labelTS28;
        private LabelTS labelTS29;
        private LabelTS labelTS30;
        private LabelTS labelTS31;
        private LabelTS labelTS32;
        private LabelTS labelTS33;
        private LabelTS labelTS34;
        private LabelTS labelTS35;
        private LabelTS labelTS36;
        private LabelTS labelTS37;
        private LabelTS labelTS38;
        private LabelTS labelTS41;
        private CheckBoxTS chkPennyExtCtrl;
        private GroupBoxTS groupBox10MhzClock;
        private RadioButtonTS radPenny10MHz;
        private RadioButtonTS radMercury10MHz;
        private RadioButtonTS radAtlas10MHz;
        private GroupBoxTS groupBox122MHz;
        private RadioButtonTS radMercury12288MHz;
        private RadioButtonTS rad12288MHzPenny;
        private LabelTS label1;
        private GroupBoxTS groupBoxMicSource;
        private RadioButtonTS radPennyMic;
        private RadioButtonTS radJanusMic;
        private GroupBoxTS groupBoxRXOptions;
        private CheckBoxTS chkMercRandom;
        private CheckBoxTS chkMercDither;
        private NumericUpDownTS udMaxFreq;
        private LabelTS labelTS57;
        private GroupBoxTS grpVersion;
        private LabelTS lblPenelopeFWVer;
        private LabelTS lblMercuryFWVer;
        private LabelTS lblOzyFWVer;
        private LabelTS lblOzyFX2;
        private GroupBoxTS grpAlexAntCtrl;
        private RadioButtonTS radAlexT3_160;
        private RadioButtonTS radAlexT2_160;
        private RadioButtonTS radAlexT1_160;
        private CheckBoxTS chkAlex160XV;
        private CheckBoxTS chkAlex160R2;
        private CheckBoxTS chkAlex160R1;
        private RadioButtonTS radAlexR3_160;
        private RadioButtonTS radAlexR2_160;
        private RadioButtonTS radAlexR1_160;
        private PanelTS panel1;
        private PanelTS panel13;
        private CheckBoxTS chkAlex12XV;
        private CheckBoxTS chkAlex12R2;
        private CheckBoxTS chkAlex12R1;
        private CheckBoxTS chkAlex15XV;
        private CheckBoxTS chkAlex15R2;
        private CheckBoxTS chkAlex15R1;
        private CheckBoxTS chkAlex17XV;
        private CheckBoxTS chkAlex17R2;
        private CheckBoxTS chkAlex17R1;
        private CheckBoxTS chkAlex20XV;
        private CheckBoxTS chkAlex20R2;
        private CheckBoxTS chkAlex20R1;
        private CheckBoxTS chkAlex30XV;
        private CheckBoxTS chkAlex30R2;
        private CheckBoxTS chkAlex30R1;
        private CheckBoxTS chkAlex40XV;
        private CheckBoxTS chkAlex40R2;
        private CheckBoxTS chkAlex40R1;
        private CheckBoxTS chkAlex60XV;
        private CheckBoxTS chkAlex60R2;
        private CheckBoxTS chkAlex60R1;
        private CheckBoxTS chkAlex80XV;
        private CheckBoxTS chkAlex80R2;
        private CheckBoxTS chkAlex80R1;
        private CheckBoxTS chkAlex6XV;
        private CheckBoxTS chkAlex6R2;
        private CheckBoxTS chkAlex6R1;
        private CheckBoxTS chkAlex10XV;
        private CheckBoxTS chkAlex10R2;
        private CheckBoxTS chkAlex10R1;
        private PanelTS panel10;
        private RadioButtonTS radAlexR2_10;
        private RadioButtonTS radAlexR1_10;
        private RadioButtonTS radAlexR3_10;
        private PanelTS panel8;
        private PanelTS panel9;
        private RadioButtonTS radAlexR2_12;
        private RadioButtonTS radAlexR1_12;
        private RadioButtonTS radAlexR3_12;
        private RadioButtonTS radAlexR2_15;
        private RadioButtonTS radAlexR1_15;
        private RadioButtonTS radAlexR3_15;
        private PanelTS panel7;
        private RadioButtonTS radAlexR2_17;
        private RadioButtonTS radAlexR1_17;
        private RadioButtonTS radAlexR3_17;
        private PanelTS panel6;
        private RadioButtonTS radAlexR2_20;
        private RadioButtonTS radAlexR1_20;
        private RadioButtonTS radAlexR3_20;
        private PanelTS panel5;
        private RadioButtonTS radAlexR2_30;
        private RadioButtonTS radAlexR1_30;
        private RadioButtonTS radAlexR3_30;
        private PanelTS panel4;
        private RadioButtonTS radAlexR2_40;
        private RadioButtonTS radAlexR1_40;
        private RadioButtonTS radAlexR3_40;
        private PanelTS panel3;
        private RadioButtonTS radAlexR2_60;
        private RadioButtonTS radAlexR1_60;
        private RadioButtonTS radAlexR3_60;
        private PanelTS panel2;
        private RadioButtonTS radAlexR2_80;
        private RadioButtonTS radAlexR1_80;
        private RadioButtonTS radAlexR3_80;
        private PanelTS panel22;
        private RadioButtonTS radAlexT2_10;
        private RadioButtonTS radAlexT1_10;
        private RadioButtonTS radAlexT3_10;
        private PanelTS panel21;
        private RadioButtonTS radAlexT2_12;
        private RadioButtonTS radAlexT1_12;
        private RadioButtonTS radAlexT3_12;
        private PanelTS panel20;
        private RadioButtonTS radAlexT2_15;
        private RadioButtonTS radAlexT1_15;
        private RadioButtonTS radAlexT3_15;
        private PanelTS panel19;
        private RadioButtonTS radAlexT2_17;
        private RadioButtonTS radAlexT1_17;
        private RadioButtonTS radAlexT3_17;
        private PanelTS panel18;
        private RadioButtonTS radAlexT2_20;
        private RadioButtonTS radAlexT1_20;
        private RadioButtonTS radAlexT3_20;
        private PanelTS panel17;
        private RadioButtonTS radAlexT2_30;
        private RadioButtonTS radAlexT1_30;
        private RadioButtonTS radAlexT3_30;
        private PanelTS panel16;
        private RadioButtonTS radAlexT2_40;
        private RadioButtonTS radAlexT1_40;
        private RadioButtonTS radAlexT3_40;
        private PanelTS panel15;
        private RadioButtonTS radAlexT2_60;
        private RadioButtonTS radAlexT1_60;
        private RadioButtonTS radAlexT3_60;
        private PanelTS panel14;
        private RadioButtonTS radAlexT2_80;
        private RadioButtonTS radAlexT1_80;
        private RadioButtonTS radAlexT3_80;
        private PanelTS panel11;
        private RadioButtonTS radAlexR2_6;
        private RadioButtonTS radAlexR1_6;
        private RadioButtonTS radAlexR3_6;
        private PanelTS panel23;
        private RadioButtonTS radAlexT2_6;
        private RadioButtonTS radAlexT1_6;
        private RadioButtonTS radAlexT3_6;
        private LabelTS label12;
        private LabelTS labelTS15;
        private LabelTS labelTS5;
        private LabelTS labelTS6;
        private LabelTS labelTS7;
        private LabelTS label17;
        private LabelTS label18;
        private LabelTS label19;
        private LabelTS labelTS11;
        private LabelTS labelTS12;
        private LabelTS labelTS13;
        private LabelTS labelTS14;
        private LabelTS labelTS56;
        private LabelTS labelTS22;
        private LabelTS label4;
        private LabelTS label3;
        private LabelTS label2;
        private LabelTS label7;
        private LabelTS label6;
        private LabelTS label5;
        private LabelTS label10;
        private LabelTS label9;
        private LabelTS label8;
        private CheckBoxTS chkAlexAntCtrl;
        private GroupBoxTS groupBoxHPSDRHW;
        private LabelTS lblPAGainByBand6;
        private NumericUpDownTS udPAGain6;
        private GroupBoxTS grpSI570;
        private LabelTS labelTS8;
        private NumericUpDownTS udFXtal;
        private CheckBoxTS chkGeneralUseSi570;
        private CheckBoxTS chkHERCULES;
        private ButtonTS btnPennyCtrlReset;
        private GroupBoxTS grpFRSRegion;
        private ComboBoxTS comboFRSRegion;
        private ComboBoxTS comboGeneralXVTR;
        private CheckBoxTS chkGeneralXVTRPresent;
        private LabelTS lblPAGainByBandVHF9;
        private NumericUpDownTS udPAGainVHF9;
        private LabelTS lblPAGainByBandVHF8;
        private NumericUpDownTS udPAGainVHF8;
        private LabelTS lblPAGainByBandVHF7;
        private NumericUpDownTS udPAGainVHF7;
        private LabelTS lblPAGainByBandVHF6;
        private NumericUpDownTS udPAGainVHF6;
        private LabelTS lblPAGainByBandVHF5;
        private NumericUpDownTS udPAGainVHF5;
        private LabelTS lblPAGainByBandVHF4;
        private NumericUpDownTS udPAGainVHF4;
        private LabelTS lblPAGainByBandVHF3;
        private NumericUpDownTS udPAGainVHF3;
        private LabelTS lblPAGainByBandVHF2;
        private NumericUpDownTS udPAGainVHF2;
        private LabelTS lblPAGainByBandVHF1;
        private NumericUpDownTS udPAGainVHF1;
        private LabelTS lblPAGainByBandVHF0;
        private NumericUpDownTS udPAGainVHF0;
        private LabelTS lblPAGainByBandVHF13;
        private NumericUpDownTS udPAGainVHF13;
        private LabelTS lblPAGainByBandVHF12;
        private NumericUpDownTS udPAGainVHF12;
        private LabelTS lblPAGainByBandVHF11;
        private NumericUpDownTS udPAGainVHF11;
        private LabelTS lblPAGainByBandVHF10;
        private NumericUpDownTS udPAGainVHF10;
        private GroupBoxTS grpOzyType;
        private GroupBoxTS grpMetisAddr;
        public RadioButtonTS radMetis;
        public RadioButtonTS radOzyUSB;
        private LabelTS lblMetisIP;
        private LabelTS lblMetisMAC;
        private LabelTS labelTS9;
        private LabelTS labelTS16;
        private CheckBoxTS chkPennyLane;
        private LabelTS lblPTTOutDelay;
        private NumericUpDownTS udGenPTTOutDelay;
        private CheckBoxTS chkPTTOutDelay;
        private GroupBoxTS grpDiagInfo;
        private LabelTS lblSyncData;
        private LabelTS lblMoxDelay;
        private NumericUpDownTS udMoxDelay;
        private LabelTS lblRFDelay;
        private NumericUpDownTS udRFDelay;
        private NumericUpDownTS udVOXGain;
        private LabelTS lblVOXGain;
        private GroupBoxTS grpBoxMic;
        private NumericUpDownTS udMicGainMin;
        private NumericUpDownTS udMicGainMax;
        private LabelTS labelTS21;
        private LabelTS labelTS20;
        private CheckBoxTS chk20dbMicBoost;
        private NumericUpDownTS udTwoToneLevel;
        private LabelTS lblTwoToneLevel;
        private LabelTS labelTS52;
        private NumericUpDownTS udTXDisplayCalOffset;
        private NumericUpDownTS udPulseDuty;
        private LabelTS lblPulseDuty;
        private NumericUpDownTS udPulsePeriod;
        private LabelTS lblPulsePeriod;
        private NumericUpDownTS udTestFreq;
        private GroupBoxTS grpBoxTXDisplayCal;
        private CheckBoxTS chkDSPALCEnabled;
        private TextBox txtDisplayOffset;
        private TextBox txtMeterOffset;
        private LabelTS labelTS54;
        private LabelTS labelTS53;
        private CheckBoxTS chkSMeter;
        private GroupBoxTS grpAppGrid;
        private ColorButton clrbtnGridFine;
        private LabelTS lblGridFine;
        private TrackBarTS tbGridFineAlpha;
        private TrackBarTS tbGridCourseAlpha;
        private TrackBarTS tbBackgroundAlpha;
        private LabelTS lblGridFineAlpha;
        private LabelTS lblGridCourseAlpha;
        private LabelTS lblBackgroundAlpha;
        private TrackBarTS tbHGridColorAlpha;
        private ColorButton clrbtnHGridColor;
        private LabelTS lblHGrid;
        private CheckBoxTS chkGridControl;
        private TabPage tcAppearanceTXDisplay;
        private GroupBoxTS groupBoxTS3;
        private CheckBoxTS chkTXGridControl;
        private TrackBarTS tbTXFilterAlpha;
        private ColorButton clrbtnTXBandEdge;
        private LabelTS lblTXBandEdge;
        private LabelTS lblDispTXFilterColor;
        private ColorButton clrbtnGridTXFilter;
        private GroupBoxTS groupBoxTS2;
        private TrackBarTS tbTXHGridColorAlpha;
        private ColorButton clrbtnTXHGridColor;
        private LabelTS lblTXHGridColor;
        private TrackBarTS tbTXVGridFineAlpha;
        private TrackBarTS tbTXVGridCourseAlpha;
        private TrackBarTS tbTXBackgroundAlpha;
        private ColorButton clrbtnTXVGridFine;
        private LabelTS lblTXVGridFine;
        private LabelTS lblTXVGridColor;
        private ColorButton clrbtnTXBackground;
        private NumericUpDownTS udTXLineWidth;
        private ColorButton clrbtnTXVGrid;
        private LabelTS lblTXLineWidth;
        private LabelTS lblTXBackgroundColor;
        private ColorButton clrbtnTXDataLine;
        private ColorButton clrbtnTXZeroLine;
        private LabelTS lblTXDataLineColor;
        private LabelTS lblTXZeroLineColor;
        private ColorButton clrbtnTXText;
        private LabelTS lblTXTextColor;
        private GroupBoxTS grpTXSpectrumGrid;
        private CheckBoxTS chkTXPanFill;
        private ComboBoxTS comboTXLabelAlign;
        private LabelTS lblTXAlign;
        private NumericUpDownTS udTXGridStep;
        public NumericUpDownTS udTXGridMin;
        public NumericUpDownTS udTXGridMax;
        private LabelTS lblTXGridStep;
        private LabelTS lblTXGridMin;
        private LabelTS lblTXGridMax;
        private TrackBarTS tbTXTextAlpha;
        private TrackBarTS tbTXZeroLineAlpha;
        private LabelTS labelTS55;
        private TabPage tpAppearanceCollapsible;
        private TextBoxTS txtCollapsedHeight;
        private TextBoxTS txtCollapsedWidth;
        private GroupBox grpBoxCollapsible;
        public CheckBoxTS chkShowModeControls;
        public CheckBoxTS chkShowBandControls;
        public CheckBoxTS chkShowTopControls;
        private GroupBoxTS grpBoxSpaceBarPTT;
        private TrackBarTS tbMeterEdgeBackgroundAlpha;
        private ColorButton clrbtnInfoButtonsColor;
        private LabelTS labelTS58;
        private RadioButtonTS radSpaceBarLastBtn;
        private RadioButtonTS radSpaceBarMicMute;
        private RadioButtonTS radSpaceBarVOX;
        private RadioButtonTS radSpaceBarPTT;
        private NumericUpDownTS udLineInBoost;
        private LabelTS lblLineInBoost;
        private GroupBoxTS grpPennyExtCtrlVHF;
        private LabelTS labelTS71;
        private LabelTS labelTS72;
        private LabelTS labelTS73;
        private LabelTS labelTS74;
        private LabelTS labelTS75;
        private LabelTS labelTS76;
        private LabelTS labelTS77;
        private LabelTS labelTS78;
        private LabelTS labelTS79;
        private LabelTS labelTS80;
        private LabelTS labelTS81;
        private LabelTS labelTS82;
        private LabelTS labelTS83;
        private LabelTS labelTS84;
        private LabelTS labelTS85;
        private LabelTS lblVHFTxControl;
        private LabelTS lblVHFRxControl;
        private LabelTS labelTS59;
        private LabelTS labelTS60;
        private LabelTS labelTS61;
        private LabelTS labelTS62;
        private LabelTS labelTS63;
        private LabelTS labelTS64;
        private LabelTS labelTS65;
        private LabelTS labelTS66;
        private LabelTS labelTS67;
        private LabelTS labelTS68;
        private LabelTS labelTS69;
        private LabelTS labelTS70;
        private CheckBoxTS chkPenOCxmitVHF117;
        private CheckBoxTS chkPenOCxmitVHF116;
        private CheckBoxTS chkPenOCxmitVHF115;
        private CheckBoxTS chkPenOCxmitVHF114;
        private CheckBoxTS chkPenOCxmitVHF113;
        private CheckBoxTS chkPenOCxmitVHF112;
        private CheckBoxTS chkPenOCxmitVHF111;
        private CheckBoxTS chkPenOCrcvVHF117;
        private CheckBoxTS chkPenOCrcvVHF116;
        private CheckBoxTS chkPenOCrcvVHF115;
        private CheckBoxTS chkPenOCrcvVHF114;
        private CheckBoxTS chkPenOCrcvVHF113;
        private CheckBoxTS chkPenOCrcvVHF112;
        private CheckBoxTS chkPenOCrcvVHF111;
        private CheckBoxTS chkPenOCxmitVHF107;
        private CheckBoxTS chkPenOCxmitVHF106;
        private CheckBoxTS chkPenOCxmitVHF105;
        private CheckBoxTS chkPenOCxmitVHF104;
        private CheckBoxTS chkPenOCxmitVHF103;
        private CheckBoxTS chkPenOCxmitVHF102;
        private CheckBoxTS chkPenOCxmitVHF101;
        private CheckBoxTS chkPenOCrcvVHF107;
        private CheckBoxTS chkPenOCrcvVHF106;
        private CheckBoxTS chkPenOCrcvVHF105;
        private CheckBoxTS chkPenOCrcvVHF104;
        private CheckBoxTS chkPenOCrcvVHF103;
        private CheckBoxTS chkPenOCrcvVHF102;
        private CheckBoxTS chkPenOCrcvVHF101;
        private CheckBoxTS chkPenOCxmitVHF97;
        private CheckBoxTS chkPenOCxmitVHF96;
        private CheckBoxTS chkPenOCxmitVHF95;
        private CheckBoxTS chkPenOCxmitVHF94;
        private CheckBoxTS chkPenOCxmitVHF93;
        private CheckBoxTS chkPenOCxmitVHF92;
        private CheckBoxTS chkPenOCxmitVHF91;
        private CheckBoxTS chkPenOCrcvVHF97;
        private CheckBoxTS chkPenOCrcvVHF96;
        private CheckBoxTS chkPenOCrcvVHF95;
        private CheckBoxTS chkPenOCrcvVHF94;
        private CheckBoxTS chkPenOCrcvVHF93;
        private CheckBoxTS chkPenOCrcvVHF92;
        private CheckBoxTS chkPenOCrcvVHF91;
        private CheckBoxTS chkPenOCxmitVHF87;
        private CheckBoxTS chkPenOCxmitVHF86;
        private CheckBoxTS chkPenOCxmitVHF85;
        private CheckBoxTS chkPenOCxmitVHF84;
        private CheckBoxTS chkPenOCxmitVHF83;
        private CheckBoxTS chkPenOCxmitVHF82;
        private CheckBoxTS chkPenOCxmitVHF81;
        private CheckBoxTS chkPenOCrcvVHF87;
        private CheckBoxTS chkPenOCrcvVHF86;
        private CheckBoxTS chkPenOCrcvVHF85;
        private CheckBoxTS chkPenOCrcvVHF84;
        private CheckBoxTS chkPenOCrcvVHF83;
        private CheckBoxTS chkPenOCrcvVHF82;
        private CheckBoxTS chkPenOCrcvVHF81;
        private CheckBoxTS chkPenOCxmitVHF77;
        private CheckBoxTS chkPenOCxmitVHF76;
        private CheckBoxTS chkPenOCxmitVHF75;
        private CheckBoxTS chkPenOCxmitVHF74;
        private CheckBoxTS chkPenOCxmitVHF73;
        private CheckBoxTS chkPenOCxmitVHF72;
        private CheckBoxTS chkPenOCxmitVHF71;
        private CheckBoxTS chkPenOCrcvVHF77;
        private CheckBoxTS chkPenOCrcvVHF76;
        private CheckBoxTS chkPenOCrcvVHF75;
        private CheckBoxTS chkPenOCrcvVHF74;
        private CheckBoxTS chkPenOCrcvVHF73;
        private CheckBoxTS chkPenOCrcvVHF72;
        private CheckBoxTS chkPenOCrcvVHF71;
        private CheckBoxTS chkPenOCxmitVHF67;
        private CheckBoxTS chkPenOCxmitVHF66;
        private CheckBoxTS chkPenOCxmitVHF65;
        private CheckBoxTS chkPenOCxmitVHF64;
        private CheckBoxTS chkPenOCxmitVHF63;
        private CheckBoxTS chkPenOCxmitVHF62;
        private CheckBoxTS chkPenOCxmitVHF61;
        private CheckBoxTS chkPenOCrcvVHF67;
        private CheckBoxTS chkPenOCrcvVHF66;
        private CheckBoxTS chkPenOCrcvVHF65;
        private CheckBoxTS chkPenOCrcvVHF64;
        private CheckBoxTS chkPenOCrcvVHF63;
        private CheckBoxTS chkPenOCrcvVHF62;
        private CheckBoxTS chkPenOCrcvVHF61;
        private CheckBoxTS chkPenOCxmitVHF57;
        private CheckBoxTS chkPenOCxmitVHF56;
        private CheckBoxTS chkPenOCxmitVHF55;
        private CheckBoxTS chkPenOCxmitVHF54;
        private CheckBoxTS chkPenOCxmitVHF53;
        private CheckBoxTS chkPenOCxmitVHF52;
        private CheckBoxTS chkPenOCxmitVHF51;
        private CheckBoxTS chkPenOCrcvVHF57;
        private CheckBoxTS chkPenOCrcvVHF56;
        private CheckBoxTS chkPenOCrcvVHF55;
        private CheckBoxTS chkPenOCrcvVHF54;
        private CheckBoxTS chkPenOCrcvVHF53;
        private CheckBoxTS chkPenOCrcvVHF52;
        private CheckBoxTS chkPenOCrcvVHF51;
        private CheckBoxTS chkPenOCxmitVHF47;
        private CheckBoxTS chkPenOCxmitVHF46;
        private CheckBoxTS chkPenOCxmitVHF45;
        private CheckBoxTS chkPenOCxmitVHF44;
        private CheckBoxTS chkPenOCxmitVHF43;
        private CheckBoxTS chkPenOCxmitVHF42;
        private CheckBoxTS chkPenOCxmitVHF41;
        private CheckBoxTS chkPenOCrcvVHF47;
        private CheckBoxTS chkPenOCrcvVHF46;
        private CheckBoxTS chkPenOCrcvVHF45;
        private CheckBoxTS chkPenOCrcvVHF44;
        private CheckBoxTS chkPenOCrcvVHF43;
        private CheckBoxTS chkPenOCrcvVHF42;
        private CheckBoxTS chkPenOCrcvVHF41;
        private CheckBoxTS chkPenOCxmitVHF37;
        private CheckBoxTS chkPenOCxmitVHF36;
        private CheckBoxTS chkPenOCxmitVHF35;
        private CheckBoxTS chkPenOCxmitVHF34;
        private CheckBoxTS chkPenOCxmitVHF33;
        private CheckBoxTS chkPenOCxmitVHF32;
        private CheckBoxTS chkPenOCxmitVHF31;
        private CheckBoxTS chkPenOCrcvVHF37;
        private CheckBoxTS chkPenOCrcvVHF36;
        private CheckBoxTS chkPenOCrcvVHF35;
        private CheckBoxTS chkPenOCrcvVHF34;
        private CheckBoxTS chkPenOCrcvVHF33;
        private CheckBoxTS chkPenOCrcvVHF32;
        private CheckBoxTS chkPenOCrcvVHF31;
        private CheckBoxTS chkPenOCxmitVHF27;
        private CheckBoxTS chkPenOCxmitVHF26;
        private CheckBoxTS chkPenOCxmitVHF25;
        private CheckBoxTS chkPenOCxmitVHF24;
        private CheckBoxTS chkPenOCxmitVHF23;
        private CheckBoxTS chkPenOCxmitVHF22;
        private CheckBoxTS chkPenOCxmitVHF21;
        private CheckBoxTS chkPenOCrcvVHF27;
        private CheckBoxTS chkPenOCrcvVHF26;
        private CheckBoxTS chkPenOCrcvVHF25;
        private CheckBoxTS chkPenOCrcvVHF24;
        private CheckBoxTS chkPenOCrcvVHF23;
        private CheckBoxTS chkPenOCrcvVHF22;
        private CheckBoxTS chkPenOCrcvVHF21;
        private CheckBoxTS chkPenOCxmitVHF17;
        private CheckBoxTS chkPenOCxmitVHF16;
        private CheckBoxTS chkPenOCxmitVHF15;
        private CheckBoxTS chkPenOCxmitVHF14;
        private CheckBoxTS chkPenOCxmitVHF13;
        private CheckBoxTS chkPenOCxmitVHF12;
        private CheckBoxTS chkPenOCxmitVHF11;
        private CheckBoxTS chkPenOCrcvVHF17;
        private CheckBoxTS chkPenOCrcvVHF16;
        private CheckBoxTS chkPenOCrcvVHF15;
        private CheckBoxTS chkPenOCrcvVHF14;
        private CheckBoxTS chkPenOCrcvVHF13;
        private CheckBoxTS chkPenOCrcvVHF12;
        private CheckBoxTS chkPenOCrcvVHF11;
        private CheckBoxTS chkPenOCxmitVHF07;
        private CheckBoxTS chkPenOCxmitVHF06;
        private CheckBoxTS chkPenOCxmitVHF05;
        private CheckBoxTS chkPenOCxmitVHF04;
        private CheckBoxTS chkPenOCxmitVHF03;
        private CheckBoxTS chkPenOCxmitVHF02;
        private CheckBoxTS chkPenOCxmitVHF01;
        private CheckBoxTS chkPenOCrcvVHF07;
        private CheckBoxTS chkPenOCrcvVHF06;
        private CheckBoxTS chkPenOCrcvVHF05;
        private CheckBoxTS chkPenOCrcvVHF04;
        private CheckBoxTS chkPenOCrcvVHF03;
        private CheckBoxTS chkPenOCrcvVHF02;
        private CheckBoxTS chkPenOCrcvVHF01;
        private ButtonTS btnPennyCtrlVHFReset;
        private CheckBoxTS checkBoxTS1;
        private CheckBoxTS chkShowCTHLine;
        private CheckBoxTS chkClickTuneFilter;
        private Label label11;
        private ComboBoxTS comboColorPalette;
        private ColorButton clrbtnWaterfallMid;
        private ColorButton clrbtnWaterfallHigh;
        private LabelTS lblDisplayWaterfallMidColor;
        private LabelTS lblDisplayWaterfallHighColor;
        private LabelTS lblMetisBoardID;
        private LabelTS labelTS88;
        private LabelTS lblMetisCodeVersion;
        private LabelTS lblMetisVer;
        private TabPage tpInfo;
        public TextBoxTS txtAlexFwdPower;
        public TextBoxTS txtRX1VFO;
        public TextBoxTS txtAlexRevPower;
        public TextBoxTS txtFwdPower;
        private LabelTS labelTS92;
        private LabelTS labelTS91;
        private LabelTS labelTS90;
        private LabelTS labelTS89;
        public TextBoxTS txtTXVFO;
        public TextBoxTS txtRX2VFO;
        public TextBoxTS txtLineInGain;
        public TextBoxTS textBoxTS11;
        public TextBoxTS textBoxTS10;
        public TextBoxTS txtAlexRevADC;
        public TextBoxTS txtAlexBand;
        public TextBoxTS txtTXAnt;
        public TextBoxTS txtRXOut;
        public TextBoxTS txtRXAnt;
        public TextBoxTS txtAlexEnabled;
        public TextBoxTS textBoxTS3;
        public TextBoxTS txtAlexFwdADC;
        public TextBoxTS txtFwdADC;
        private LabelTS labelTS103;
        private LabelTS labelTS102;
        private LabelTS labelTS101;
        private LabelTS labelTS100;
        private LabelTS labelTS99;
        private LabelTS labelTS98;
        private LabelTS labelTS97;
        private LabelTS labelTS96;
        private LabelTS labelTS95;
        private LabelTS labelTS94;
        private LabelTS labelTS93;
        private LabelTS labelTS19;
        private LabelTS labelTS18;
        private LabelTS labelTS17;
        private TabControl tcAlexControl;
        private TabPage tpAlexFilterControl;
        private LabelTS labelTS104;
        private LabelTS labelTS105;
        private LabelTS labelTS106;
        private LabelTS labelTS107;
        private LabelTS labelTS108;
        private LabelTS labelTS109;
        private LabelTS labelTS110;
        private LabelTS labelTS111;
        private LabelTS labelTS112;
        private LabelTS labelTS113;
        private LabelTS labelTS114;
        private LabelTS labelTS115;
        private PanelTS panelTS1;
        public RadioButtonTS radAlexAutoCntl;
        public RadioButtonTS radAlexManualCntl;
        public NumericUpDownTS udAlex10mLPFEnd;
        public NumericUpDownTS udAlex10mLPFStart;
        public NumericUpDownTS udAlex6mLPFEnd;
        public NumericUpDownTS udAlex6mLPFStart;
        public NumericUpDownTS udAlex20mLPFStart;
        public NumericUpDownTS udAlex15mLPFStart;
        public NumericUpDownTS udAlex20mLPFEnd;
        public NumericUpDownTS udAlex15mLPFEnd;
        public NumericUpDownTS udAlex40mLPFEnd;
        public NumericUpDownTS udAlex40mLPFStart;
        public NumericUpDownTS udAlex80mLPFEnd;
        public NumericUpDownTS udAlex80mLPFStart;
        public NumericUpDownTS udAlex160mLPFEnd;
        public NumericUpDownTS udAlex160mLPFStart;
        public NumericUpDownTS udAlex13HPFStart;
        public NumericUpDownTS udAlex20HPFStart;
        public NumericUpDownTS udAlex13HPFEnd;
        public NumericUpDownTS udAlex20HPFEnd;
        public NumericUpDownTS udAlex9_5HPFEnd;
        public NumericUpDownTS udAlex9_5HPFStart;
        public NumericUpDownTS udAlex6_5HPFEnd;
        public NumericUpDownTS udAlex6_5HPFStart;
        public NumericUpDownTS udAlex1_5HPFEnd;
        public NumericUpDownTS udAlex1_5HPFStart;
        private NumericUpDownTS numericUpDownTS5;
        private NumericUpDownTS numericUpDownTS7;
        private NumericUpDownTS numericUpDownTS8;
        private PanelTS panelTS2;
        private RadioButtonTS radioButtonTS1;
        private RadioButtonTS radioButtonTS2;
        private NumericUpDownTS numericUpDownTS11;
        private NumericUpDownTS numericUpDownTS13;
        private NumericUpDownTS numericUpDownTS14;
        private PanelTS panelTS3;
        private RadioButtonTS radioButtonTS3;
        private RadioButtonTS radioButtonTS4;
        private LabelTS labelTS117;
        private LabelTS labelTS116;
        private LabelTS labelTS126;
        private LabelTS labelTS125;
        private LabelTS labelTS124;
        private LabelTS labelTS123;
        private LabelTS labelTS122;
        private LabelTS labelTS121;
        private LabelTS labelTS120;
        private LabelTS labelTS118;
        private LabelTS labelTS119;
        private NumericUpDownTS numericUpDownTS1;
        private NumericUpDownTS numericUpDownTS2;
        private NumericUpDownTS numericUpDownTS15;
        private NumericUpDownTS numericUpDownTS16;
        private NumericUpDownTS numericUpDownTS17;
        private NumericUpDownTS numericUpDownTS18;
        private NumericUpDownTS numericUpDownTS19;
        private NumericUpDownTS numericUpDownTS20;
        private NumericUpDownTS numericUpDownTS21;
        private NumericUpDownTS numericUpDownTS22;
        private NumericUpDownTS numericUpDownTS23;
        private NumericUpDownTS numericUpDownTS24;
        private NumericUpDownTS numericUpDownTS25;
        private NumericUpDownTS numericUpDownTS26;
        private NumericUpDownTS numericUpDownTS27;
        private NumericUpDownTS numericUpDownTS28;
        private NumericUpDownTS numericUpDownTS29;
        private NumericUpDownTS numericUpDownTS30;
        private NumericUpDownTS numericUpDownTS31;
        private NumericUpDownTS numericUpDownTS32;
        private NumericUpDownTS numericUpDownTS33;
        private NumericUpDownTS numericUpDownTS34;
        private NumericUpDownTS numericUpDownTS35;
        private NumericUpDownTS numericUpDownTS36;
        private PanelTS panelTS4;
        private RadioButtonTS radioButtonTS5;
        private RadioButtonTS radioButtonTS6;
        private LabelTS labelTS131;
        private LabelTS labelTS130;
        private LabelTS labelTS129;
        private LabelTS labelTS128;
        private LabelTS labelTS127;
        private CheckBoxTS chkAlexHPFBypass;
        private LabelTS labelTS132;
        public NumericUpDownTS udAlex6BPFStart;
        public NumericUpDownTS udAlex6BPFEnd;
        private PanelTS panelTS5;
        public RadioButtonTS rad1_5HPFled;
        public RadioButtonTS rad6_5HPFled;
        public RadioButtonTS rad9_5HPFled;
        public RadioButtonTS rad13HPFled;
        public RadioButtonTS rad20HPFled;
        public RadioButtonTS rad6BPFled;
        public RadioButtonTS radBPHPFled;
        private PanelTS panelTS6;
        public RadioButtonTS rad6LPFled;
        public RadioButtonTS rad80LPFled;
        public RadioButtonTS rad40LPFled;
        public RadioButtonTS rad20LPFled;
        public RadioButtonTS rad15LPFled;
        public RadioButtonTS rad10LPFled;
        public RadioButtonTS rad160LPFled;
        private LabelTS labelTS134;
        private LabelTS labelTS133;
        public CheckBoxTS chkAlex20BPHPF;
        public CheckBoxTS chkAlex6_5BPHPF;
        public CheckBoxTS chkAlex9_5BPHPF;
        public CheckBoxTS chkAlex6BPHPF;
        public CheckBoxTS chkAlex13BPHPF;
        public CheckBoxTS chkAlex1_5BPHPF;
        private NumericUpDownTS udGeneralCalRX2Level;
        private NumericUpDownTS udGeneralCalRX2Freq2;
        private LabelTS labelTS135;
        private LabelTS labelTS136;
        private ButtonTS btnCalLevel;
        private GroupBoxTS groupBoxTS1;
        private CheckBoxTS chkSwapAF;
        private LabelTS lblMercury2FWVer;
        private PanelTS panelRX2LevelCal;
        private NumericUpDownTS udDSPAGCRX2MaxGaindB;
        private NumericUpDownTS udDSPAGCRX2FixedGaindB;
        public TrackBarTS tbDSPAGCRX2HangThreshold;
        private NumericUpDownTS udDSPAGCRX2Slope;
        private NumericUpDownTS udDSPAGCRX2Attack;
        private NumericUpDownTS udDSPAGCRX2Decay;
        private NumericUpDownTS udDSPAGCRX2HangTime;
        private LabelTS labelTS137;
        private LabelTS labelTS138;
        private Label label13;
        private Label lblRX1Vol;
        private CheckBoxTS chkSpectrumLine;
        private CheckBoxTS chkShowAGC;
        private CheckBoxTS chkRX2HangSpectrumLine;
        private CheckBoxTS chkDisplayRX2HangLine;
        private CheckBoxTS chkRX2GainSpectrumLine;
        private CheckBoxTS chkDisplayRX2GainLine;
        private CheckBoxTS chkAGCHangSpectrumLine;
        private CheckBoxTS chkAGCDisplayHangLine;
        private System.ComponentModel.IContainer components;
        private CheckBoxTS chkStrictCharSpacing;
        private CheckBoxTS chkModeBStrict;
        private CheckBoxTS chkDSPKeyerSidetone;

        #endregion

        #region Constructor and Destructor

        public Setup(Console c)
        {
            FlexProfilerInstalled = false;
            InitializeComponent();
            console = c;
            openFileDialog1.InitialDirectory = console.AppDataPath;

#if(!DEBUG)
            comboGeneralProcessPriority.Items.Remove("Idle");
            comboGeneralProcessPriority.Items.Remove("Below Normal");
#endif
            initializing = true;

            InitWindowTypes();
            GetMixerDevices();
            GetHosts();
            InitAlexAntTables();

            KeyList = new ArrayList();
            SetupKeyMap();

            GetTxProfiles();
            GetTxProfileDefs();

            RefreshCOMPortLists();

            RefreshSkinList();

            comboGeneralLPTAddr.SelectedIndex = -1;
            comboGeneralXVTR.SelectedIndex = (int)XVTRTRMode.POSITIVE;
            comboGeneralProcessPriority.Text = "Normal";
            comboOptFilterWidthMode.Text = "Linear";
            comboAudioSoundCard.Text = "Unsupported Card";
            comboAudioSampleRate1.SelectedIndex = 0;
            comboAudioSampleRate2.Text = "48000";
            comboAudioSampleRate3.Text = "48000";
            comboAudioBuffer1.Text = "1024";
            comboAudioBuffer2.Text = "512";
            comboAudioBuffer3.Text = "512";
            comboAudioChannels1.Text = "6";
            Audio.IN_RX1_L = 0;
            Audio.IN_RX1_R = 1;
            Audio.IN_RX2_L = 2;
            Audio.IN_RX2_R = 3;
            Audio.IN_TX_L = 4;
            Audio.IN_TX_R = 5;
            comboDisplayLabelAlign.Text = "Auto";
            comboColorPalette.Text = "enhanced";
            comboTXLabelAlign.Text = "Auto";
            comboDisplayDriver.Text = "GDI+";
            comboDSPPhoneRXBuf.Text = "2048";
            comboDSPPhoneTXBuf.Text = "2048";
            comboDSPCWRXBuf.Text = "2048";
            comboDSPCWTXBuf.Text = "2048";
            comboDSPDigRXBuf.Text = "2048";
            comboDSPDigTXBuf.Text = "2048";
            comboDSPWindow.SelectedIndex = (int)Window.BLKHARRIS;
            comboKeyerConnKeyLine.SelectedIndex = 0;
            comboKeyerConnSecondary.SelectedIndex = 0;
            comboKeyerConnPTTLine.SelectedIndex = 0;
            comboKeyerConnPrimary.SelectedIndex = 0;
            comboTXTUNMeter.SelectedIndex = 0;
            comboMeterType.Text = "Edge";
            if (comboCATPort.Items.Count > 0) comboCATPort.SelectedIndex = 0;
            if (comboCATPTTPort.Items.Count > 0) comboCATPTTPort.SelectedIndex = 0;
            comboCATbaud.Text = "1200";
            comboCATparity.Text = "none";
            comboCATdatabits.Text = "8";
            comboCATstopbits.Text = "1";
            comboCATRigType.Text = "TS-2000";
            comboFRSRegion.Text = "United States";

            //fillMetisIPAddrCombo();  /* must happen before GetOptions is called */ 

            GetOptions();

            if (comboDSPPhoneRXBuf.SelectedIndex < 0 || comboDSPPhoneRXBuf.SelectedIndex >= comboDSPPhoneRXBuf.Items.Count)
                comboDSPPhoneRXBuf.SelectedIndex = comboDSPPhoneRXBuf.Items.Count - 1;
            if (comboDSPPhoneTXBuf.SelectedIndex < 0 || comboDSPPhoneTXBuf.SelectedIndex >= comboDSPPhoneTXBuf.Items.Count)
                comboDSPPhoneTXBuf.SelectedIndex = comboDSPPhoneTXBuf.Items.Count - 1;
            if (comboDSPCWRXBuf.SelectedIndex < 0 || comboDSPCWRXBuf.SelectedIndex >= comboDSPCWRXBuf.Items.Count)
                comboDSPCWRXBuf.SelectedIndex = comboDSPCWRXBuf.Items.Count - 1;
            if (comboDSPCWTXBuf.SelectedIndex < 0 || comboDSPCWTXBuf.SelectedIndex >= comboDSPCWTXBuf.Items.Count)
                comboDSPCWTXBuf.SelectedIndex = comboDSPCWTXBuf.Items.Count - 1;
            if (comboDSPDigRXBuf.SelectedIndex < 0 || comboDSPDigRXBuf.SelectedIndex >= comboDSPDigRXBuf.Items.Count)
                comboDSPDigRXBuf.SelectedIndex = comboDSPDigRXBuf.Items.Count - 1;
            if (comboDSPDigTXBuf.SelectedIndex < 0 || comboDSPDigTXBuf.SelectedIndex >= comboDSPDigTXBuf.Items.Count)
                comboDSPDigTXBuf.SelectedIndex = comboDSPDigTXBuf.Items.Count - 1;

            if (comboCATPort.SelectedIndex < 0)
            {
                if (comboCATPort.Items.Count > 0)
                    comboCATPort.SelectedIndex = 0;
                else
                {
                    chkCATEnable.Checked = false;
                    chkCATEnable.Enabled = false;
                }
            }

            cmboSigGenRXMode.Text = "Radio";
            cmboSigGenTXMode.Text = "Radio";

            if (comboAudioDriver1.SelectedIndex < 0 &&
                comboAudioDriver1.Items.Count > 0)
                comboAudioDriver1.SelectedIndex = 0;

            if (comboAudioDriver2.SelectedIndex < 0 &&
                comboAudioDriver2.Items.Count > 0)
                comboAudioDriver2.SelectedIndex = 0;

            if (comboAudioDriver3.SelectedIndex < 0 &&
                comboAudioDriver3.Items.Count > 0)
                comboAudioDriver3.SelectedIndex = 0;

            if (comboAudioMixer1.SelectedIndex < 0 &&
                comboAudioMixer1.Items.Count > 0)
                comboAudioMixer1.SelectedIndex = 0;

            comboAudioBuffer1_SelectedIndexChanged(this, EventArgs.Empty);

            initializing = false;
            udDisplayScopeTime_ValueChanged(this, EventArgs.Empty);

            if (comboTXProfileName.SelectedIndex < 0 &&
                comboTXProfileName.Items.Count > 0)
                comboTXProfileName.SelectedIndex = 0;
            current_profile = comboTXProfileName.Text;

            if (chkCATEnable.Checked)
            {
                chkCATEnable_CheckedChanged(this, EventArgs.Empty);
            }

            if (chkCATPTTEnabled.Checked)
            {
                chkCATPTTEnabled_CheckedChanged(this, EventArgs.Empty);
            }

            comboKeyerConnSecondary_SelectedIndexChanged(this, EventArgs.Empty);

            if (radGenModelFLEX5000.Checked && DB.GetVars("Options").Count != 0)
                radGenModelFLEX5000_CheckedChanged(this, EventArgs.Empty);

            ForceAllEvents();

            EventArgs e = EventArgs.Empty;
            comboGeneralLPTAddr_LostFocus(this, e);
            chkGeneralSpurRed_CheckedChanged(this, e);
            udDDSCorrection_ValueChanged(this, e);
            chkAudioLatencyManual1_CheckedChanged(this, e);
            udAudioLineIn1_ValueChanged(this, e);
            comboAudioReceive1_SelectedIndexChanged(this, e);
            //  udLMSANF_ValueChanged(this, e);
            // udLMSNR_ValueChanged(this, e);
            udDSPImagePhaseTX_ValueChanged(this, e);
            udDSPImageGainTX_ValueChanged(this, e);
            udDSPCWPitch_ValueChanged(this, e);
            udTXFilterHigh_ValueChanged(this, e);
            udTXFilterLow_ValueChanged(this, e);
            tbRX1FilterAlpha_Scroll(this, e);
            tbTXFilterAlpha_Scroll(this, e);
            tbBackgroundAlpha_Scroll(this, e);
            tbTXBackgroundAlpha_Scroll(this, e);
            tbGridCourseAlpha_Scroll(this, e);
            tbTXVGridCourseAlpha_Scroll(this, e);
            tbGridFineAlpha_Scroll(this, e);
            tbTXVGridFineAlpha_Scroll(this, e);
            tbHGridColorAlpha_Scroll(this, e);
            tbTXHGridColorAlpha_Scroll(this, e);
            tbMultiRXFilterAlpha_Scroll(this, e);
            tbTXZeroLineAlpha_Scroll(this, e);
            tbTXTextAlpha_Scroll(this, e);
            tbMeterEdgeBackgroundAlpha_Scroll(this, e);
            checkHPSDRDefaults(this, e);
            chkAlexPresent_CheckedChanged(this, e);
            chkApolloPresent_CheckedChanged(this, e);
            chkEClassModulation_CheckedChanged(this, e);

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    console.radio.GetDSPRX(i, j).Update = true;
            comboDSPPhoneRXBuf_SelectedIndexChanged(this, EventArgs.Empty);
            comboDSPPhoneTXBuf_SelectedIndexChanged(this, EventArgs.Empty);

            openFileDialog1.Filter = "PowerSDR Database Files (*.xml) | *.xml";

            if (chkKWAI.Checked)
                AllowFreqBroadcast = true;
            else
                AllowFreqBroadcast = false;

            tkbarTestGenFreq.Value = console.CWPitch;
        }
#if false
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
#endif
        #endregion

        #region Init Routines
        // ======================================================
        // Init Routines
        // ======================================================

        private void InitGeneralTab()
        {
            if (console.Hdw != null)
                comboGeneralLPTAddr.Text = Convert.ToString(console.Hdw.LPTAddr, 16);
            chkPTTOutDelay.Checked = console.Hdw.PTTODelayControl;
            udGeneralLPTDelay.Value = console.LatchDelay;
            chkGeneralRXOnly.Checked = console.RXOnly;
            chkGeneralUSBPresent.Checked = console.USBPresent;
            chkBoxJanusOzyControl.Checked = console.OzyControl;
            chkGeneralPAPresent.Checked = console.PAPresent;
            chkGeneralXVTRPresent.Checked = console.XVTRPresent;
            comboGeneralXVTR.SelectedItem = (int)console.CurrentXVTRTRMode;
            chkGeneralSpurRed.Checked = true;
            chkGeneralDisablePTT.Checked = console.DisablePTT;
            chkGeneralSoftwareGainCorr.Checked = console.NoHardwareOffset;
            chkGeneralEnableX2.Checked = console.X2Enabled;
            chkGeneralCustomFilter.Checked = console.EnableLPF0;
            chkGeneralUpdateRelease.Checked = console.NotifyOnRelease;
            chkGeneralUpdateBeta.Checked = console.NotifyOnBeta;
        }

        private void InitAudioTab()
        {
            // set driver type
            if (comboAudioDriver1.SelectedIndex < 0 &&
                comboAudioDriver1.Items.Count > 0)
            {
                foreach (PADeviceInfo p in comboAudioDriver1.Items)
                {
                    if (p.Name == "ASIO")
                    {
                        comboAudioDriver1.SelectedItem = p;
                        break;
                    }
                }

                if (comboAudioDriver1.Text != "ASIO")
                    comboAudioDriver1.Text = "MME";
            }

            // set Input device
            if (comboAudioInput1.Items.Count > 0)
                comboAudioInput1.SelectedIndex = 0;

            // set Output device
            if (comboAudioOutput1.Items.Count > 0)
                comboAudioOutput1.SelectedIndex = 0;

            // set sample rate
            comboAudioSampleRate1.Text = "96000";

            if (comboAudioReceive1.Enabled == true)
            {
                for (int i = 0; i < comboAudioReceive1.Items.Count; i++)
                {
                    if (((string)comboAudioReceive1.Items[i]).StartsWith("Line"))
                    {
                        comboAudioReceive1.SelectedIndex = i;
                        i = comboAudioReceive1.Items.Count;
                    }
                }
            }

            if (comboAudioTransmit1.Enabled == true)
            {
                for (int i = 0; i < comboAudioTransmit1.Items.Count; i++)
                {
                    if (((string)comboAudioTransmit1.Items[i]).StartsWith("Mic"))
                    {
                        comboAudioTransmit1.SelectedIndex = i;
                        i = comboAudioTransmit1.Items.Count;
                    }
                }
            }

            comboAudioBuffer1.Text = "1024";
            udAudioLatency1.Value = Audio.Latency1;
        }

        private void InitDisplayTab()
        {
            udDisplayGridMax.Value = Display.SpectrumGridMax;
            udDisplayGridMin.Value = Display.SpectrumGridMin;
            udDisplayGridStep.Value = Display.SpectrumGridStep;
            udDisplayFPS.Value = console.DisplayFPS;
            clrbtnWaterfallLow.Color = Display.WaterfallLowColor;
            udDisplayWaterfallLowLevel.Value = (decimal)Display.WaterfallLowThreshold;
            udDisplayWaterfallHighLevel.Value = (decimal)Display.WaterfallHighThreshold;
            udDisplayMeterDelay.Value = console.MeterDelay;
            udDisplayPeakText.Value = console.PeakTextDelay;
            udDisplayCPUMeter.Value = console.CPUMeterDelay;
            udDisplayPhasePts.Value = Display.PhaseNumPts;
            udDisplayMultiPeakHoldTime.Value = console.MultimeterPeakHoldTime;
            udDisplayMultiTextHoldTime.Value = console.MultimeterTextPeakTime;
            udTXGridMax.Value = Display.TXSpectrumGridMax;
            udTXGridMin.Value = Display.TXSpectrumGridMin;
            udTXGridStep.Value = Display.TXSpectrumGridStep;

        }

        private void InitDSPTab()
        {
            udDSPCWPitch.Value = console.CWPitch;
            comboDSPWindow.SelectedIndex = (int)console.radio.GetDSPRX(0, 0).CurrentWindow;
        }

        private void InitKeyboardTab()
        {
            // set tune keys
            comboKBTuneUp1.Text = KeyToString(console.KeyTuneUp1);
            comboKBTuneUp2.Text = KeyToString(console.KeyTuneUp2);
            comboKBTuneUp3.Text = KeyToString(console.KeyTuneUp3);
            comboKBTuneUp4.Text = KeyToString(console.KeyTuneUp4);
            comboKBTuneUp5.Text = KeyToString(console.KeyTuneUp5);
            comboKBTuneUp6.Text = KeyToString(console.KeyTuneUp6);
            comboKBTuneUp7.Text = KeyToString(console.KeyTuneUp7);
            comboKBTuneDown1.Text = KeyToString(console.KeyTuneDown1);
            comboKBTuneDown2.Text = KeyToString(console.KeyTuneDown2);
            comboKBTuneDown3.Text = KeyToString(console.KeyTuneDown3);
            comboKBTuneDown4.Text = KeyToString(console.KeyTuneDown4);
            comboKBTuneDown5.Text = KeyToString(console.KeyTuneDown5);
            comboKBTuneDown6.Text = KeyToString(console.KeyTuneDown6);
            comboKBTuneDown7.Text = KeyToString(console.KeyTuneDown7);

            // set band keys
            comboKBBandDown.Text = KeyToString(console.KeyBandDown);
            comboKBBandUp.Text = KeyToString(console.KeyBandUp);

            // set filter keys
            comboKBFilterDown.Text = KeyToString(console.KeyFilterDown);
            comboKBFilterUp.Text = KeyToString(console.KeyFilterUp);

            // set mode keys
            comboKBModeDown.Text = KeyToString(console.KeyModeDown);
            comboKBModeUp.Text = KeyToString(console.KeyModeUp);

            // set RIT keys
            comboKBRITDown.Text = KeyToString(console.KeyRITDown);
            comboKBRITUp.Text = KeyToString(console.KeyRITUp);

            // set XIT keys
            comboKBXITDown.Text = KeyToString(console.KeyXITDown);
            comboKBXITUp.Text = KeyToString(console.KeyXITUp);

            // set CW keys
            comboKBCWDot.Text = KeyToString(console.KeyCWDot);
            comboKBCWDash.Text = KeyToString(console.KeyCWDash);
        }

        private void InitAppearanceTab()
        {
            clrbtnBackground.Color = Display.DisplayBackgroundColor;
            clrbtnGrid.Color = Display.GridColor;
            clrbtnGridFine.Color = Display.GridPenDark;
            clrbtnHGridColor.Color = Display.HGridColor;
            clrbtnZeroLine.Color = Display.GridZeroColor;
            clrbtnText.Color = Display.GridTextColor;
            clrbtnDataLine.Color = Display.DataLineColor;
            clrbtnFilter.Color = Display.DisplayFilterColor;
            clrbtnMeterLeft.Color = console.MeterLeftColor;
            clrbtnMeterRight.Color = console.MeterRightColor;
            clrbtnBtnSel.Color = console.ButtonSelectedColor;
            clrbtnVFODark.Color = console.VFOTextDarkColor;
            clrbtnVFOLight.Color = console.VFOTextLightColor;
            clrbtnBandDark.Color = console.BandTextDarkColor;
            clrbtnBandLight.Color = console.BandTextLightColor;
            clrbtnPeakText.Color = console.PeakTextColor;
            clrbtnOutOfBand.Color = console.OutOfBandColor;
        }

        #endregion

        #region Misc Routines
        // ======================================================
        // Misc Routines
        // ======================================================

        private void InitDelta44()
        {
            int retval = DeltaCP.Init();
            if (retval != 0) return;
            DeltaCP.SetLevels();
            DeltaCP.Close();
        }

        private void RefreshCOMPortLists()
        {
            string[] com_ports = SerialPort.GetPortNames();

            comboKeyerConnPrimary.Items.Clear();
            switch (console.CurrentModel)
            {
                case Model.FLEX3000:
                case Model.FLEX5000:
                case Model.SDR1000:
                    comboKeyerConnPrimary.Items.Add("Radio");
                    break;
                default:
                       comboKeyerConnPrimary.Items.Add("None");
                    break;
            }

            comboKeyerConnSecondary.Items.Clear();
            comboKeyerConnSecondary.Items.Add("None");
            comboKeyerConnSecondary.Items.Add("CAT");

            comboCATPort.Items.Clear();
            comboCATPTTPort.Items.Clear();

            comboKeyerConnPrimary.Items.AddRange(com_ports);
            comboKeyerConnSecondary.Items.AddRange(com_ports);

            comboCATPort.Items.Add("None");
            comboCATPort.Items.AddRange(com_ports);

            comboCATPTTPort.Items.Add("None");
            comboCATPTTPort.Items.AddRange(com_ports);
        }

        private void RefreshSkinList()
        {
            string skin = comboAppSkin.Text;
            comboAppSkin.Items.Clear();
            string path;
            if (console.default_directory)
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    "\\FlexRadio Systems\\PowerSDR\\Skins";
            }
            else
            {
                path = Application.StartupPath + "\\Skins";
            }
            if (!Directory.Exists(path))
            {
                MessageBox.Show("The console presentation files (skins) were not found.\n" +
                    "Appearance will suffer until this is rectified.\n" +
                    "Please visit www.flex-radio.com to download the missing files.",
                    "Skins files not found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            foreach (string d in Directory.GetDirectories(path))
            {
                string s = d.Substring(d.LastIndexOf("\\") + 1);
                if (!s.StartsWith("."))
                    comboAppSkin.Items.Add(d.Substring(d.LastIndexOf("\\") + 1));
            }

            if (comboAppSkin.Items.Count == 0)
            {
                MessageBox.Show("The console presentation files (skins) were not found.\n" +
                    "Appearance will suffer until this is rectified.\n",
                    "Skins files not found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (skin == "")
            {
                if (comboAppSkin.Items.Contains("Default"))
                    comboAppSkin.Text = "Default";
                else
                    comboAppSkin.Text = "OpenHPSDR-Gray";
            }
            else if (comboAppSkin.Items.Contains(skin))
                comboAppSkin.Text = skin;
            else comboAppSkin.Text = "Default";
        }

        private void InitWindowTypes()
        {
            for (Window w = Window.FIRST + 1; w < Window.LAST; w++)
            {
                string s = w.ToString().ToLower();
                s = s.Substring(0, 1).ToUpper() + s.Substring(1, s.Length - 1);
                comboDSPWindow.Items.Add(s);
            }
        }

        private void GetHosts()
        {
            comboAudioDriver1.Items.Clear();
            comboAudioDriver2.Items.Clear();
            comboAudioDriver3.Items.Clear();
            int host_index = 0;
            foreach (string PAHostName in Audio.GetPAHosts())
            {
                if (Audio.GetPAInputDevices(host_index).Count > 0 ||
                    Audio.GetPAOutputDevices(host_index).Count > 0)
                {
                    comboAudioDriver1.Items.Add(new PADeviceInfo(PAHostName, host_index));
                    if (PAHostName != "Windows WASAPI")
                    {
                        comboAudioDriver2.Items.Add(new PADeviceInfo(PAHostName, host_index));
                        comboAudioDriver3.Items.Add(new PADeviceInfo(PAHostName, host_index));
                    }
                    // comboAudioDriver1.Items.Add(new PADeviceInfo(PAHostName, host_index));
                    // comboAudioDriver2.Items.Add(new PADeviceInfo(PAHostName, host_index));
                }
                host_index++; //Increment host index
            }
        }

        private void GetDevices1()
        {
            comboAudioInput1.Items.Clear();
            comboAudioOutput1.Items.Clear();
            int host = ((PADeviceInfo)comboAudioDriver1.SelectedItem).Index;
            ArrayList a = Audio.GetPAInputDevices(host);
            foreach (PADeviceInfo p in a)
                comboAudioInput1.Items.Add(p);

            a = Audio.GetPAOutputDevices(host);
            foreach (PADeviceInfo p in a)
                comboAudioOutput1.Items.Add(p);
        }

        private void GetDevices2()
        {
            comboAudioInput2.Items.Clear();
            comboAudioOutput2.Items.Clear();
            int host = ((PADeviceInfo)comboAudioDriver2.SelectedItem).Index;
            ArrayList a = Audio.GetPAInputDevices(host);
            foreach (PADeviceInfo p in a)
                comboAudioInput2.Items.Add(p);

            a = Audio.GetPAOutputDevices(host);
            foreach (PADeviceInfo p in a)
                comboAudioOutput2.Items.Add(p);
        }

        private void GetDevices3()
        {
            comboAudioInput3.Items.Clear();
            comboAudioOutput3.Items.Clear();
            int host = ((PADeviceInfo)comboAudioDriver3.SelectedItem).Index;
            ArrayList a = Audio.GetPAInputDevices(host);
            foreach (PADeviceInfo p in a)
                comboAudioInput3.Items.Add(p);

            a = Audio.GetPAOutputDevices(host);
            foreach (PADeviceInfo p in a)
                comboAudioOutput3.Items.Add(p);
        }

        private void ControlList(Control c, ref ArrayList a)
        {
            if (c.Controls.Count > 0)
            {
                foreach (Control c2 in c.Controls)
                    ControlList(c2, ref a);
            }

            if (c.GetType() == typeof(CheckBoxTS) || c.GetType() == typeof(CheckBox) ||
                c.GetType() == typeof(ComboBoxTS) || c.GetType() == typeof(ComboBox) ||
                c.GetType() == typeof(NumericUpDownTS) || c.GetType() == typeof(NumericUpDown) ||
                c.GetType() == typeof(RadioButtonTS) || c.GetType() == typeof(RadioButton) ||
                c.GetType() == typeof(TextBoxTS) || c.GetType() == typeof(TextBox) ||
                c.GetType() == typeof(TrackBarTS) || c.GetType() == typeof(TrackBar) ||
                c.GetType() == typeof(ColorButton))
                a.Add(c);
        }

        private static bool saving = false;

        public void SaveOptions()
        {
            // Automatically saves all control settings to the database in the tab
            // pages on this form of the following types: CheckBoxTS, ComboBoxTS,
            // NumericUpDownTS, RadioButtonTS, TextBox, and TrackBar (slider)

            saving = true;

            ArrayList a = new ArrayList();
            ArrayList temp = new ArrayList();

            ControlList(this, ref temp);

            foreach (Control c in temp)				// For each control
            {
                if (c.GetType() == typeof(CheckBoxTS))
                    a.Add(c.Name + "/" + ((CheckBoxTS)c).Checked.ToString());
                else if (c.GetType() == typeof(ComboBoxTS))
                {
                    //if(((ComboBoxTS)c).SelectedIndex >= 0)
                    a.Add(c.Name + "/" + ((ComboBoxTS)c).Text);
                }
                else if (c.GetType() == typeof(NumericUpDownTS))
                    a.Add(c.Name + "/" + ((NumericUpDownTS)c).Value.ToString());
                else if (c.GetType() == typeof(RadioButtonTS))
                    a.Add(c.Name + "/" + ((RadioButtonTS)c).Checked.ToString());
                else if (c.GetType() == typeof(TextBoxTS))
                    a.Add(c.Name + "/" + ((TextBoxTS)c).Text);
                else if (c.GetType() == typeof(TrackBarTS))
                    a.Add(c.Name + "/" + ((TrackBarTS)c).Value.ToString());
                else if (c.GetType() == typeof(ColorButton))
                {
                    Color clr = ((ColorButton)c).Color;
                    a.Add(c.Name + "/" + clr.R + "." + clr.G + "." + clr.B + "." + clr.A);
                }
#if(DEBUG)
                else if (c.GetType() == typeof(GroupBox) ||
                    c.GetType() == typeof(CheckBox) ||
                    c.GetType() == typeof(ComboBox) ||
                    c.GetType() == typeof(NumericUpDown) ||
                    c.GetType() == typeof(RadioButton) ||
                    c.GetType() == typeof(TextBox) ||
                    c.GetType() == typeof(TrackBar))
                    Debug.WriteLine(c.Name + " needs to be converted to a Thread Safe control.");
#endif
            }

            DB.SaveVars("Options", ref a);		// save the values to the DB
            saving = false;
        }

        public void GetOptions()
        {
            // Automatically restores all controls from the database in the
            // tab pages on this form of the following types: CheckBoxTS, ComboBoxTS,
            // NumericUpDownTS, RadioButtonTS, TextBox, and TrackBar (slider)

            // get list of live controls
            //ArrayList temp = new ArrayList();
            ArrayList temp = new ArrayList();		// list of all first level controls
            ControlList(this, ref temp);

            ArrayList checkbox_list = new ArrayList();
            ArrayList combobox_list = new ArrayList();
            ArrayList numericupdown_list = new ArrayList();
            ArrayList radiobutton_list = new ArrayList();
            ArrayList textbox_list = new ArrayList();
            ArrayList trackbar_list = new ArrayList();
            ArrayList colorbutton_list = new ArrayList();

            //ArrayList controls = new ArrayList();	// list of controls to restore
            foreach (Control c in temp)
            {
                if (c.GetType() == typeof(CheckBoxTS))			// the control is a CheckBoxTS
                    checkbox_list.Add(c);
                else if (c.GetType() == typeof(ComboBoxTS))		// the control is a ComboBoxTS
                    combobox_list.Add(c);
                else if (c.GetType() == typeof(NumericUpDownTS))	// the control is a NumericUpDownTS
                    numericupdown_list.Add(c);
                else if (c.GetType() == typeof(RadioButtonTS))	// the control is a RadioButtonTS
                    radiobutton_list.Add(c);
                else if (c.GetType() == typeof(TextBoxTS))		// the control is a TextBox
                    textbox_list.Add(c);
                else if (c.GetType() == typeof(TrackBarTS))		// the control is a TrackBar (slider)
                    trackbar_list.Add(c);
                else if (c.GetType() == typeof(ColorButton))
                    colorbutton_list.Add(c);
            }
            temp.Clear();	// now that we have the controls we want, delete first list 

            ArrayList a = DB.GetVars("Options");						// Get the saved list of controls
            a.Sort();
            int num_controls = checkbox_list.Count + combobox_list.Count +
                numericupdown_list.Count + radiobutton_list.Count +
                textbox_list.Count + trackbar_list.Count +
                colorbutton_list.Count;

            if (a.Count < num_controls)		// some control values are not in the database
            {								// so set all of them to the defaults
                InitGeneralTab();
                InitAudioTab();
                InitDSPTab();
                InitDisplayTab();
                InitKeyboardTab();
                InitAppearanceTab();
            }

            // restore saved values to the controls
            foreach (string s in a)				// string is in the format "name,value"
            {
                string[] vals = s.Split('/');
                if (vals.Length > 2)
                {
                    for (int i = 2; i < vals.Length; i++)
                        vals[1] += "/" + vals[i];
                }

                string name = vals[0];
                string val = vals[1];

                if (s.StartsWith("chk"))			// control is a CheckBoxTS
                {
                    for (int i = 0; i < checkbox_list.Count; i++)
                    {	// look through each control to find the matching name
                        CheckBoxTS c = (CheckBoxTS)checkbox_list[i];
                        if (c.Name.Equals(name))		// name found
                        {
                            c.Checked = bool.Parse(val);	// restore value
                            i = checkbox_list.Count + 1;
                        }
                        if (i == checkbox_list.Count)
                            MessageBox.Show("Control not found: " + name);
                    }
                }
                else if (s.StartsWith("combo"))	// control is a ComboBox
                {
                    for (int i = 0; i < combobox_list.Count; i++)
                    {	// look through each control to find the matching name
                        ComboBoxTS c = (ComboBoxTS)combobox_list[i];
                        if (c.Name.Equals(name))		// name found
                        {
                            if (c.Items.Count > 0 && c.Items[0].GetType() == typeof(string))
                            {
                                c.Text = val;
                            }
                            else
                            {
                                foreach (object o in c.Items)
                                {
                                    if (o.ToString() == val)
                                        c.Text = val;	// restore value
                                }
                            }
                            i = combobox_list.Count + 1;
                        }
                        if (i == combobox_list.Count)
                            MessageBox.Show("Control not found: " + name);
                    }
                }
                else if (s.StartsWith("ud"))
                {
                    for (int i = 0; i < numericupdown_list.Count; i++)
                    {	// look through each control to find the matching name
                        NumericUpDownTS c = (NumericUpDownTS)numericupdown_list[i];
                        if (c.Name.Equals(name))		// name found
                        {
                            decimal num = decimal.Parse(val);

                            if (num > c.Maximum) num = c.Maximum;		// check endpoints
                            else if (num < c.Minimum) num = c.Minimum;
                            c.Value = num;			// restore value
                            i = numericupdown_list.Count + 1;
                        }
                        if (i == numericupdown_list.Count)
                            MessageBox.Show("Control not found: " + name);
                    }
                }
                else if (s.StartsWith("rad"))
                {	// look through each control to find the matching name
                    for (int i = 0; i < radiobutton_list.Count; i++)
                    {
                        RadioButtonTS c = (RadioButtonTS)radiobutton_list[i];
                        if (c.Name.Equals(name))		// name found
                        {
                            c.Checked = bool.Parse(val);	// restore value
                            i = radiobutton_list.Count + 1;
                        }
                        if (i == radiobutton_list.Count)
                            MessageBox.Show("Control not found: " + name);
                    }
                }
                else if (s.StartsWith("txt"))
                {	// look through each control to find the matching name
                    for (int i = 0; i < textbox_list.Count; i++)
                    {
                        TextBoxTS c = (TextBoxTS)textbox_list[i];
                        if (c.Name.Equals(name))		// name found
                        {
                            c.Text = val;	// restore value
                            i = textbox_list.Count + 1;
                        }
                        if (i == textbox_list.Count)
                            MessageBox.Show("Control not found: " + name);
                    }
                }
                else if (s.StartsWith("tb"))
                {
                    // look through each control to find the matching name
                    for (int i = 0; i < trackbar_list.Count; i++)
                    {
                        TrackBarTS c = (TrackBarTS)trackbar_list[i];
                        if (c.Name.Equals(name))		// name found
                        {
                            c.Value = Int32.Parse(val);
                            i = trackbar_list.Count + 1;
                        }
                        if (i == trackbar_list.Count)
                            MessageBox.Show("Control not found: " + name);
                    }
                }
                else if (s.StartsWith("clrbtn"))
                {
                    string[] colors = val.Split('.');
                    if (colors.Length == 4)
                    {
                        int R, G, B, A;
                        R = Int32.Parse(colors[0]);
                        G = Int32.Parse(colors[1]);
                        B = Int32.Parse(colors[2]);
                        A = Int32.Parse(colors[3]);

                        for (int i = 0; i < colorbutton_list.Count; i++)
                        {
                            ColorButton c = (ColorButton)colorbutton_list[i];
                            if (c.Name.Equals(name))		// name found
                            {
                                c.Color = Color.FromArgb(A, R, G, B);
                                i = colorbutton_list.Count + 1;
                            }
                            if (i == colorbutton_list.Count)
                                MessageBox.Show("Control not found: " + name);
                        }
                    }
                }
            }

            foreach (ColorButton c in colorbutton_list)
                c.Automatic = "";
        }

        private string KeyToString(Keys k)
        {
            if (!k.ToString().StartsWith("Oem"))
                return k.ToString();

            string s = "";
            switch (k)
            {
                case Keys.OemOpenBrackets:
                    s = "[";
                    break;
                case Keys.OemCloseBrackets:
                    s = "]";
                    break;
                case Keys.OemQuestion:
                    s = "/";
                    break;
                case Keys.OemPeriod:
                    s = ".";
                    break;
                case Keys.OemPipe:
                    s = (k & Keys.Shift) == 0 ? "\\" : "|";
                    break;
            }
            return s;
        }

        private void SetupKeyMap()
        {
            KeyList.Add(Keys.None);
            KeyList.Add(Keys.A);
            KeyList.Add(Keys.B);
            KeyList.Add(Keys.C);
            KeyList.Add(Keys.D);
            KeyList.Add(Keys.E);
            KeyList.Add(Keys.F);
            KeyList.Add(Keys.G);
            KeyList.Add(Keys.H);
            KeyList.Add(Keys.I);
            KeyList.Add(Keys.J);
            KeyList.Add(Keys.K);
            KeyList.Add(Keys.L);
            KeyList.Add(Keys.M);
            KeyList.Add(Keys.N);
            KeyList.Add(Keys.O);
            KeyList.Add(Keys.P);
            KeyList.Add(Keys.Q);
            KeyList.Add(Keys.R);
            KeyList.Add(Keys.S);
            KeyList.Add(Keys.T);
            KeyList.Add(Keys.U);
            KeyList.Add(Keys.V);
            KeyList.Add(Keys.W);
            KeyList.Add(Keys.X);
            KeyList.Add(Keys.Y);
            KeyList.Add(Keys.Z);
            KeyList.Add(Keys.F1);
            KeyList.Add(Keys.F2);
            KeyList.Add(Keys.F3);
            KeyList.Add(Keys.F4);
            KeyList.Add(Keys.F5);
            KeyList.Add(Keys.F6);
            KeyList.Add(Keys.F7);
            KeyList.Add(Keys.F8);
            KeyList.Add(Keys.F9);
            KeyList.Add(Keys.F10);
            KeyList.Add(Keys.Insert);
            KeyList.Add(Keys.Delete);
            KeyList.Add(Keys.Home);
            KeyList.Add(Keys.End);
            KeyList.Add(Keys.PageUp);
            KeyList.Add(Keys.PageDown);
            KeyList.Add(Keys.Up);
            KeyList.Add(Keys.Down);
            KeyList.Add(Keys.Left);
            KeyList.Add(Keys.Right);
            KeyList.Add(Keys.OemOpenBrackets);
            KeyList.Add(Keys.OemCloseBrackets);
            KeyList.Add(Keys.OemPeriod);
            KeyList.Add(Keys.OemQuestion);
            //			KeyList.Add(Keys.OemSemicolon);
            //			KeyList.Add(Keys.OemQuotes);
            //			KeyList.Add(Keys.Oemcomma);
            //			KeyList.Add(Keys.OemPeriod);
            //			KeyList.Add(Keys.OemBackslash);
            //			KeyList.Add(Keys.OemQuestion);
            //          KeyList.Add(Keys.Space);

            foreach (Control c in tpKeyboard.Controls)
            {
                if (c.GetType() == typeof(GroupBoxTS))
                {
                    foreach (Control c2 in c.Controls)
                    {
                        if (c2.GetType() != typeof(ComboBoxTS)) continue;
                        ComboBoxTS combo = (ComboBoxTS)c2;
                        combo.Items.Clear();
                        foreach (Keys k in KeyList)
                        {
                            combo.Items.Add(k.ToString().StartsWith("Oem") ? KeyToString(k) : k.ToString());
                        }
                    }
                }
                else if (c.GetType() == typeof(ComboBoxTS))
                {
                    ComboBoxTS combo = (ComboBoxTS)c;
                    combo.Items.Clear();
                    foreach (Keys k in KeyList)
                        combo.Items.Add(k.ToString());
                }
            }
        }

        private void UpdateMixerControls1()
        {
            if (comboAudioMixer1.SelectedIndex < 0 || comboAudioMixer1.Items.Count <= 0) return;
            int i = -1;

            i = Mixer.GetMux(comboAudioMixer1.SelectedIndex);
            if (i < 0 || i >= Mixer.MIXERR_BASE)
            {
                comboAudioReceive1.Enabled = false;
                comboAudioReceive1.Items.Clear();
                comboAudioTransmit1.Enabled = false;
                comboAudioTransmit1.Items.Clear();
            }
            else
            {
                comboAudioReceive1.Enabled = true;
                comboAudioTransmit1.Enabled = true;
                GetMuxLineNames1();
                for (int j = 0; j < comboAudioReceive1.Items.Count; j++)
                {
                    if (((string)comboAudioReceive1.Items[j]).StartsWith("Line"))
                    {
                        comboAudioReceive1.SelectedIndex = j;
                        j = comboAudioReceive1.Items.Count;
                    }
                }

                if (comboAudioReceive1.SelectedIndex < 0)
                {
                    for (int j = 0; j < comboAudioReceive1.Items.Count; j++)
                    {
                        if (((string)comboAudioReceive1.Items[j]).StartsWith("Analog"))
                        {
                            comboAudioReceive1.SelectedIndex = j;
                            j = comboAudioReceive1.Items.Count;
                        }
                    }
                }

                for (int j = 0; j < comboAudioTransmit1.Items.Count; j++)
                {
                    if (((string)comboAudioTransmit1.Items[j]).StartsWith("Mic"))
                    {
                        comboAudioTransmit1.SelectedIndex = j;
                        j = comboAudioTransmit1.Items.Count;
                    }
                }
            }
        }

        private void GetMixerDevices()
        {
            comboAudioMixer1.Items.Clear();
            int num = Mixer.mixerGetNumDevs();
            for (int i = 0; i < num; i++)
            {
                comboAudioMixer1.Items.Add(Mixer.GetDevName(i));
            }
            comboAudioMixer1.Items.Add("None");
        }

        private void GetMuxLineNames1()
        {
            if (comboAudioMixer1.SelectedIndex >= 0 &&
                comboAudioMixer1.Items.Count > 0)
            {
                comboAudioReceive1.Items.Clear();
                comboAudioTransmit1.Items.Clear();

                ArrayList a;
                bool good = Mixer.GetMuxLineNames(comboAudioMixer1.SelectedIndex, out a);
                if (good)
                {
                    foreach (string s in a)
                    {
                        comboAudioReceive1.Items.Add(s);
                        comboAudioTransmit1.Items.Add(s);
                    }
                }
            }
        }

        public int CollapsedWidth
        {
            get { return int.Parse(txtCollapsedWidth.Text); }
            set
            {
                this.txtCollapsedWidth.Text = value.ToString();
                //				if (!saving)
                //					SaveOptions();
            }
        }

        public int CollapsedHeight
        {
            get { return int.Parse(txtCollapsedHeight.Text); }
            set
            {
                this.txtCollapsedHeight.Text = value.ToString();
                //				if (!saving)
                //					SaveOptions();
            }
        }

        private void ForceAllEvents()
        {
            EventArgs e = EventArgs.Empty;

            // General Tab
            //  radAlexR_160_Click(this, e);
            // radAlexR_160_CheckedChanged(this, e);

            chkAlexAntCtrl_CheckedChanged(this, e);
            comboGeneralLPTAddr_SelectedIndexChanged(this, e);
            udGeneralLPTDelay_ValueChanged(this, e);
            chkGeneralRXOnly_CheckedChanged(this, e);
            chkGeneralUSBPresent_CheckedChanged(this, e);
            chkGeneralPAPresent_CheckedChanged(this, e);
            chkGeneralATUPresent_CheckedChanged(this, e);
            chkXVTRPresent_CheckedChanged(this, e);
            comboGeneralXVTR_SelectedIndexChanged(this, e);
            udDDSCorrection_ValueChanged(this, e);
            udDDSPLLMult_ValueChanged(this, e);
            udDDSIFFreq_ValueChanged(this, e);
            chkGeneralSpurRed_CheckedChanged(this, e);
            chkGeneralDisablePTT_CheckedChanged(this, e);
            chkGeneralSoftwareGainCorr_CheckedChanged(this, e);
            chkGeneralEnableX2_CheckedChanged(this, e);
            udGeneralX2Delay_ValueChanged(this, e);
            chkGeneralCustomFilter_CheckedChanged(this, e);
            comboGeneralProcessPriority_SelectedIndexChanged(this, e);
            chkGeneralUpdateRelease_CheckedChanged(this, e);
            chkGeneralUpdateBeta_CheckedChanged(this, e);
            chkFullDiscovery_CheckedChanged(this, e);

            // Audio Tab
            comboAudioSoundCard_SelectedIndexChanged(this, e);
            comboAudioDriver1_SelectedIndexChanged(this, e);
            comboAudioInput1_SelectedIndexChanged(this, e);
            comboAudioOutput1_SelectedIndexChanged(this, e);
            comboAudioMixer1_SelectedIndexChanged(this, e);
            comboAudioReceive1_SelectedIndexChanged(this, e);
            comboAudioTransmit1_SelectedIndexChanged(this, e);
            //			comboAudioDriver2_SelectedIndexChanged(this, e);
            //			comboAudioInput2_SelectedIndexChanged(this, e);
            //			comboAudioOutput2_SelectedIndexChanged(this, e);
            //			comboAudioMixer2_SelectedIndexChanged(this, e);
            //			comboAudioReceive2_SelectedIndexChanged(this, e);
            //			comboAudioTransmit2_SelectedIndexChanged(this, e);
            comboAudioBuffer1_SelectedIndexChanged(this, e);
            comboAudioBuffer2_SelectedIndexChanged(this, e);
            comboAudioSampleRate1_SelectedIndexChanged(this, e);
            comboAudioSampleRate2_SelectedIndexChanged(this, e);
            udAudioLatency1_ValueChanged(this, e);
            udAudioLatency2_ValueChanged(this, e);
            udAudioLineIn1_ValueChanged(this, e);
            udAudioVoltage1_ValueChanged(this, e);
            chkAudioLatencyManual1_CheckedChanged(this, e);

            // Calibration Tab
            udTXDisplayCalOffset_ValueChanged(this, e);

            // Test Tab
            udTestFreq_ValueChanged(this, e);
            tkbarTestGenFreq_Scroll(this, e);
            updnTestGenScale_ValueChanged(this, e);
            udTwoToneLevel_ValueChanged(this, e);

            // Display Tab
            udDisplayGridMax_ValueChanged(this, e);
            udDisplayGridMin_ValueChanged(this, e);
            udDisplayGridStep_ValueChanged(this, e);
            udDisplayFPS_ValueChanged(this, e);
            udTXGridMax_ValueChanged(this, e);
            udTXGridMin_ValueChanged(this, e);
            udTXGridStep_ValueChanged(this, e);
            udDisplayMeterDelay_ValueChanged(this, e);
            udDisplayPeakText_ValueChanged(this, e);
            udDisplayCPUMeter_ValueChanged(this, e);
            udDisplayPhasePts_ValueChanged(this, e);
            udDisplayAVGTime_ValueChanged(this, e);
            udDisplayWaterfallLowLevel_ValueChanged(this, e);
            udDisplayWaterfallHighLevel_ValueChanged(this, e);
            clrbtnWaterfallLow_Changed(this, e);
            udDisplayMultiPeakHoldTime_ValueChanged(this, e);
            udDisplayMultiTextHoldTime_ValueChanged(this, e);

            // DSP Tab
            udLMSANF_ValueChanged(this, e);
            udLMSNR_ValueChanged(this, e);
            udLMSANF2_ValueChanged(this, e);
            udLMSNR2_ValueChanged(this, e);
            // udDSPImagePhaseTX_ValueChanged(this, e);
            // udDSPImageGainTX_ValueChanged(this, e);
            udDSPCWPitch_ValueChanged(this, e);
            comboDSPWindow_SelectedIndexChanged(this, e);
            udDSPNB_ValueChanged(this, e);
            udDSPNB2_ValueChanged(this, e);
            //AGC
            udDSPAGCFixedGaindB_ValueChanged(this, e);
            udDSPAGCMaxGaindB_ValueChanged(this, e);
            udDSPAGCSlope_ValueChanged(this, e);
            udDSPAGCRX2Slope_ValueChanged(this, e);
            udDSPAGCAttack_ValueChanged(this, e);
            udDSPAGCDecay_ValueChanged(this, e);
            udDSPAGCHangTime_ValueChanged(this, e);
            tbDSPAGCHangThreshold_Scroll(this, e);
            udDSPAGCRX2FixedGaindB_ValueChanged(this, e);
            udDSPAGCRX2MaxGaindB_ValueChanged(this, e);
            udDSPAGCRX2Slope_ValueChanged(this, e);
            udDSPAGCRX2Attack_ValueChanged(this, e);
            udDSPAGCRX2Decay_ValueChanged(this, e);
            udDSPAGCRX2HangTime_ValueChanged(this, e);
            tbDSPAGCRX2HangThreshold_Scroll(this, e);
            //Leveler
            chkDSPLevelerEnabled_CheckedChanged(this, e);
            udDSPLevelerThreshold_ValueChanged(this, e);
            udDSPLevelerAttack_ValueChanged(this, e);
            udDSPLevelerDecay_ValueChanged(this, e);
            udDSPLevelerHangTime_ValueChanged(this, e);
            //ALC
            chkDSPALCEnabled_CheckedChanged(this, e);
            udDSPALCThreshold_ValueChanged(this, e);
            udDSPALCAttack_ValueChanged(this, e);
            udDSPALCDecay_ValueChanged(this, e);
            udDSPALCHangTime_ValueChanged(this, e);
            // Transmit Tab
            udTXFilterHigh_ValueChanged(this, e);
            udTXFilterLow_ValueChanged(this, e);
            udTransmitTunePower_ValueChanged(this, e);
            udPAGain_ValueChanged(this, e);
            radMicIn_CheckedChanged(this, e);
            radLineIn_CheckedChanged(this, e);
            udMicGainMax_ValueChanged(this, e);
            udMicGainMin_ValueChanged(this, e);
            udLineInBoost_ValueChanged(this, e);
            // Keyboard Tab
            comboKBTuneUp1_SelectedIndexChanged(this, e);
            comboKBTuneUp2_SelectedIndexChanged(this, e);
            comboKBTuneUp3_SelectedIndexChanged(this, e);
            comboKBTuneUp4_SelectedIndexChanged(this, e);
            comboKBTuneUp5_SelectedIndexChanged(this, e);
            comboKBTuneUp6_SelectedIndexChanged(this, e);
            comboKBTuneDown1_SelectedIndexChanged(this, e);
            comboKBTuneDown2_SelectedIndexChanged(this, e);
            comboKBTuneDown3_SelectedIndexChanged(this, e);
            comboKBTuneDown4_SelectedIndexChanged(this, e);
            comboKBTuneDown5_SelectedIndexChanged(this, e);
            comboKBTuneDown6_SelectedIndexChanged(this, e);
            comboKBBandUp_SelectedIndexChanged(this, e);
            comboKBBandDown_SelectedIndexChanged(this, e);
            comboKBFilterUp_SelectedIndexChanged(this, e);
            comboKBFilterDown_SelectedIndexChanged(this, e);
            comboKBModeUp_SelectedIndexChanged(this, e);
            comboKBModeDown_SelectedIndexChanged(this, e);

            // Appearance Tab
            clrbtnBtnSel_Changed(this, e);
            clrbtnVFODark_Changed(this, e);
            clrbtnVFOLight_Changed(this, e);
            clrbtnBandDark_Changed(this, e);
            clrbtnBandLight_Changed(this, e);
            clrbtnPeakText_Changed(this, e);
            clrbtnBackground_Changed(this, e);
            clrbtnTXBackground_Changed(this, e);
            clrbtnGrid_Changed(this, e);
            clrbtnTXVGrid_Changed(this, e);
            clrbtnGridFine_Changed(this, e);
            clrbtnHGridColor_Changed(this, e);
            clrbtnTXHGridColor_Changed(this, e);
            clrbtnZeroLine_Changed(this, e);
            clrbtnTXZeroLine_Changed(this, e);
            clrbtnFilter_Changed(this, e);
            clrbtnGridTXFilter_Changed(this, e);
            clrbtnText_Changed(this, e);
            clrbtnDataLine_Changed(this, e);
            udDisplayLineWidth_ValueChanged(this, e);
            udTXLineWidth_ValueChanged(this, e);
            clrbtnTXDataLine_Changed(this, e);
            clrbtnMeterLeft_Changed(this, e);
            clrbtnMeterRight_Changed(this, e);
            chkGridControl_CheckedChanged(this, e);
            clrbtnBandEdge_Changed(this, e);
            clrbtnTXBandEdge_Changed(this, e);
        }

        public string[] GetTXProfileStrings()
        {
            string[] s = new string[comboTXProfileName.Items.Count];
            for (int i = 0; i < comboTXProfileName.Items.Count; i++)
                s[i] = (string)comboTXProfileName.Items[i];
            return s;
        }

        public string TXProfile
        {
            get
            {
                return comboTXProfileName != null ? comboTXProfileName.Text : "";
            }
            set { if (comboTXProfileName != null) comboTXProfileName.Text = value; }
        }

        public void GetTxProfiles()
        {
            comboTXProfileName.Items.Clear();
            foreach (DataRow dr in DB.ds.Tables["TxProfile"].Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (!comboTXProfileName.Items.Contains(dr["Name"]))
                        comboTXProfileName.Items.Add(dr["Name"]);
                }
            }
        }

        public void GetTxProfileDefs()
        {
            lstTXProfileDef.Items.Clear();
            foreach (DataRow dr in DB.ds.Tables["TxProfileDef"].Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (!lstTXProfileDef.Items.Contains(dr["Name"]))
                        lstTXProfileDef.Items.Add(dr["name"]);
                }
            }
        }

        private bool CheckTXProfileChanged()
        {
            DataRow[] rows = DB.ds.Tables["TxProfile"].Select(
                "'" + current_profile + "' = Name");

            if (rows.Length != 1)
                return false;

            int[] eq = console.EQForm.TXEQ;
            if (eq[0] != (int)rows[0]["TXEQPreamp"])
                return true;

            if (console.EQForm.TXEQEnabled != (bool)rows[0]["TXEQEnabled"])
                return true;

            for (int i = 1; i < eq.Length; i++)
            {
                if (eq[i] != (int)rows[0]["TXEQ" + i])
                    return true;
            }

            if (udTXFilterLow.Value != (int)rows[0]["FilterLow"] ||
                udTXFilterHigh.Value != (int)rows[0]["FilterHigh"] ||
                console.CPDR != (bool)rows[0]["CompanderOn"] ||
                console.CPDRLevel != (int)rows[0]["CompanderLevel"] ||
                console.Mic != (int)rows[0]["MicGain"])
                return true;

            return false;
        }
        public void SaveTXProfileData() //W4TME
        {
            if (profile_deleted == true)
            {
                profile_deleted = false;
                return;
            }

            string name = current_profile;

            DataRow dr = null;

            foreach (DataRow d in DB.ds.Tables["TxProfile"].Rows)
            {
                if ((string)d["Name"] == name)
                {
                    dr = d;
                    break;
                }
            }

            dr["FilterLow"] = (int)udTXFilterLow.Value;
            dr["FilterHigh"] = (int)udTXFilterHigh.Value;
            dr["TXEQEnabled"] = console.EQForm.TXEQEnabled;
            dr["TXEQNumBands"] = console.EQForm.NumBands;
            int[] eq = console.EQForm.TXEQ;
            dr["TXEQPreamp"] = eq[0];
            for (int i = 1; i < eq.Length; i++)
                dr["TXEQ" + i.ToString()] = eq[i];
            for (int i = eq.Length; i < 11; i++)
                dr["TXEQ" + i.ToString()] = 0;

            dr["DXOn"] = console.DX;
            dr["DXLevel"] = console.DXLevel;
            dr["CompanderOn"] = console.CPDR;
            dr["CompanderLevel"] = console.CPDRLevel;

            dr["MicGain"] = console.Mic;
            dr["FMMicGain"] = console.FMMic;

            dr["Lev_On"] = chkDSPLevelerEnabled.Checked;
            dr["Lev_Slope"] = (int)udDSPLevelerSlope.Value;
            dr["Lev_MaxGain"] = (int)udDSPLevelerThreshold.Value;
            dr["Lev_Attack"] = (int)udDSPLevelerAttack.Value;
            dr["Lev_Decay"] = (int)udDSPLevelerDecay.Value;
            dr["Lev_Hang"] = (int)udDSPLevelerHangTime.Value;
            dr["Lev_HangThreshold"] = tbDSPLevelerHangThreshold.Value;

            dr["ALC_Slope"] = (int)udDSPALCSlope.Value;
            dr["ALC_MaxGain"] = (int)udDSPALCThreshold.Value;
            dr["ALC_Attack"] = (int)udDSPALCAttack.Value;
            dr["ALC_Decay"] = (int)udDSPALCDecay.Value;
            dr["ALC_Hang"] = (int)udDSPALCHangTime.Value;
            dr["ALC_HangThreshold"] = tbDSPALCHangThreshold.Value;

            dr["Power"] = console.PWR;

            dr["Dexp_On"] = chkTXNoiseGateEnabled.Checked;
            dr["Dexp_Threshold"] = (int)udTXNoiseGate.Value;
            dr["Dexp_Attenuate"] = (int)udTXNoiseGateAttenuate.Value;

            dr["VOX_On"] = chkTXVOXEnabled.Checked;
            dr["VOX_Threshold"] = (int)udTXVOXThreshold.Value;
            dr["VOX_HangTime"] = (int)udTXVOXHangTime.Value;
            dr["Tune_Power"] = (int)udTXTunePower.Value;
            dr["Tune_Meter_Type"] = (string)comboTXTUNMeter.Text;

            dr["TX_Limit_Slew"] = (bool)chkTXLimitSlew.Checked;

            dr["TX_AF_Level"] = console.TXAF;

            dr["AM_Carrier_Level"] = (int)udTXAMCarrierLevel.Value;

            dr["Show_TX_Filter"] = (bool)console.ShowTXFilter;

            dr["VAC1_On"] = (bool)chkAudioEnableVAC.Checked;
            dr["VAC1_Auto_On"] = (bool)chkAudioVACAutoEnable.Checked;
            dr["VAC1_RX_GAIN"] = (int)udAudioVACGainRX.Value;
            dr["VAC1_TX_GAIN"] = (int)udAudioVACGainTX.Value;
            dr["VAC1_Stereo_On"] = (bool)chkAudio2Stereo.Checked;
            dr["VAC1_Sample_Rate"] = (string)comboAudioSampleRate2.Text;
            dr["VAC1_Buffer_Size"] = (string)comboAudioBuffer2.Text;
            dr["VAC1_IQ_Output"] = (bool)chkAudioIQtoVAC.Checked;
            dr["VAC1_IQ_Correct"] = (bool)chkAudioCorrectIQ.Checked;
            dr["VAC1_PTT_OverRide"] = (bool)chkVACAllowBypass.Checked;
            dr["VAC1_Combine_Input_Channels"] = (bool)chkVACCombine.Checked;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = (int)udAudioLatency2.Value;

            dr["VAC2_On"] = (bool)chkVAC2Enable.Checked;
            dr["VAC2_Auto_On"] = (bool)chkVAC2AutoEnable.Checked;
            dr["VAC2_RX_GAIN"] = (int)udVAC2GainRX.Value;
            dr["VAC2_TX_GAIN"] = (int)udVAC2GainTX.Value;
            dr["VAC2_Stereo_On"] = (bool)chkAudioStereo3.Checked;
            dr["VAC2_Sample_Rate"] = (string)comboAudioSampleRate3.Text;
            dr["VAC2_Buffer_Size"] = (string)comboAudioBuffer3.Text;
            dr["VAC2_IQ_Output"] = (bool)chkVAC2DirectIQ.Checked;
            dr["VAC2_IQ_Correct"] = (bool)chkVAC2DirectIQCal.Checked;
            dr["VAC2_Combine_Input_Channels"] = (bool)chkVAC2Combine.Checked;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = (int)udVAC2Latency.Value;

            dr["Phone_RX_DSP_Buffer"] = (string)comboDSPPhoneRXBuf.Text;
            dr["Phone_TX_DSP_Buffer"] = (string)comboDSPPhoneTXBuf.Text;
            dr["Digi_RX_DSP_Buffer"] = (string)comboDSPDigRXBuf.Text;
            dr["Digi_TX_DSP_Buffer"] = (string)comboDSPDigTXBuf.Text;
            dr["CW_RX_DSP_Buffer"] = (string)comboDSPCWRXBuf.Text;

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    dr["Mic_Input_On"] = (string)console.fwcMixForm.MicInputSelected;
                    dr["Mic_Input_Level"] = (int)console.fwcMixForm.MicInput;
                    dr["Line_Input_On"] = (string)console.fwcMixForm.LineInRCASelected;
                    dr["Line_Input_Level"] = (int)console.fwcMixForm.LineInRCA;
                    dr["Balanced_Line_Input_On"] = (string)console.fwcMixForm.LineInPhonoSelected;
                    dr["Balanced_Line_Input_Level"] = (int)console.fwcMixForm.LineInPhono;
                    dr["FlexWire_Input_On"] = (string)console.fwcMixForm.LineInDB9Selected;
                    dr["FlexWire_Input_Level"] = (int)console.fwcMixForm.LineInDB9;
                    break;

                case Model.FLEX3000:
                    dr["Mic_Input_On"] = (string)console.flex3000MixerForm.MicInputSelected;
                    dr["Mic_Input_Level"] = (int)console.flex3000MixerForm.MicInput;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = (string)console.flex3000MixerForm.LineInDB9Selected;
                    dr["FlexWire_Input_Level"] = (int)console.flex3000MixerForm.LineInDB9;
                    break;

                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }
        }

        #endregion

        #region Properties

        public int VACDriver
        {
            get
            {
                return comboAudioDriver2.SelectedIndex;
            }
            set
            {
                if ((comboAudioDriver2.Items.Count - 1) > value)
                {
                    comboAudioDriver2.SelectedIndex = value;
                }
            }
        }

        public int VAC2Driver
        {
            get
            {
                return comboAudioDriver3.SelectedIndex;
            }
            set
            {
                if ((comboAudioDriver3.Items.Count - 1) > value)
                {
                    comboAudioDriver3.SelectedIndex = value;
                }
            }
        }

        public int VACInputCable
        {
            get
            {
                return comboAudioInput2.SelectedIndex;
            }
            set
            {
                if ((comboAudioInput2.Items.Count - 1) > value)
                {
                    comboAudioInput2.SelectedIndex = value;
                }
            }
        }

        public int VAC2InputCable
        {
            get
            {
                return comboAudioInput3.SelectedIndex;
            }
            set
            {
                if ((comboAudioInput3.Items.Count - 1) > value)
                {
                    comboAudioInput3.SelectedIndex = value;
                }
            }
        }

        public int VACOutputCable
        {
            get
            {
                return comboAudioOutput2.SelectedIndex;
            }
            set
            {
                if ((comboAudioOutput2.Items.Count - 1) > value)
                {
                    comboAudioOutput2.SelectedIndex = value;
                }
            }
        }

        public int VAC2OutputCable
        {
            get
            {
                return comboAudioOutput3.SelectedIndex;
            }
            set
            {
                if ((comboAudioOutput3.Items.Count - 1) > value)
                {
                    comboAudioOutput3.SelectedIndex = value;
                }
            }
        }

        public bool VACUseRX2
        {
            get
            {
                if (chkAudioRX2toVAC != null && IQOutToVAC)
                {
                    return chkAudioRX2toVAC.Checked;
                }
                else return false;
            }
            set
            {
                if (chkAudioRX2toVAC != null && IQOutToVAC)
                {
                    chkAudioRX2toVAC.Checked = value;
                }
            }
        }

        public bool VAC2UseRX2
        {
            get
            {
                if (chkVAC2UseRX2 != null && IQOutToVAC)
                {
                    return chkVAC2UseRX2.Checked;
                }
                else return false;
            }
            set
            {
                if (chkVAC2UseRX2 != null && IQOutToVAC)
                {
                    chkVAC2UseRX2.Checked = value;
                }
            }
        }

        public bool CATEnabled
        {
            get
            {
                if (chkCATEnable != null) return chkCATEnable.Checked;
                else return false;
            }
            set
            {
                if (chkCATEnable != null) chkCATEnable.Checked = value;
            }
        }

        public int RXAGCAttack
        {
            get
            {
                if (udDSPAGCAttack != null) return (int)udDSPAGCAttack.Value;
                else return 0;
            }
            set
            {
                if (udDSPAGCAttack != null) udDSPAGCAttack.Value = value;
            }
        }

        public int RXAGCHang
        {
            get
            {
                if (udDSPAGCHangTime != null) return (int)udDSPAGCHangTime.Value;
                else return 0;
            }
            set
            {
                if (udDSPAGCHangTime != null) udDSPAGCHangTime.Value = value;
            }
        }

        public int RXAGCDecay
        {
            get
            {
                if (udDSPAGCDecay != null) return (int)udDSPAGCDecay.Value;
                else return 0;
            }
            set
            {
                if (udDSPAGCDecay != null) udDSPAGCDecay.Value = value;
            }
        }

        public double IFFreq
        {
            get
            {
                if (udDDSIFFreq != null) return (double)udDDSIFFreq.Value * 1e-6;
                else return 0.0;
            }
            set
            {
                if (udDDSIFFreq != null) udDDSIFFreq.Value = (int)(value * 1e6);
            }
        }

        public bool X2TR
        {
            get
            {
                if (chkGeneralEnableX2 != null) return chkGeneralEnableX2.Checked;
                else return false;
            }
            set
            {
                if (chkGeneralEnableX2 != null) chkGeneralEnableX2.Checked = value;
            }
        }

        private float display_contrast = 1.0f;
        public float DisplayContrast
        {
            get { return display_contrast; }

            set { display_contrast = value; }
        }

        public int BreakInDelay
        {
            get
            {
                if (udCWBreakInDelay != null) return (int)udCWBreakInDelay.Value;
                else return -1;
            }
            set
            {
                if (udCWBreakInDelay != null) udCWBreakInDelay.Value = value;
            }
        }

        public int CWPitch
        {
            get
            {
                if (udDSPCWPitch != null) return (int)udDSPCWPitch.Value;
                else return -1;
            }
            set
            {
                if (udDSPCWPitch != null) udDSPCWPitch.Value = value;
            }
        }

        public bool CWDisableMonitor
        {
            get
            {
                if (chkDSPKeyerSidetone != null) return chkDSPKeyerSidetone.Checked;
                else return false;
            }
            set
            {
                if (chkDSPKeyerSidetone != null) chkDSPKeyerSidetone.Checked = value;
            }
        }

        public bool CWIambic
        {
            get
            {
                if (chkCWKeyerIambic != null) return chkCWKeyerIambic.Checked;
                else return false;
            }
            set
            {
                if (chkCWKeyerIambic != null) chkCWKeyerIambic.Checked = value;
            }
        }

        public string VACSampleRate
        {
            get
            {
                if (comboAudioSampleRate2 != null) return comboAudioSampleRate2.Text;
                else return "";
            }
            set
            {
                if (comboAudioSampleRate2 != null) comboAudioSampleRate2.Text = value;
            }
        }

        public string VAC2SampleRate
        {
            get
            {
                if (comboAudioSampleRate3 != null) return comboAudioSampleRate3.Text;
                else return "";
            }
            set
            {
                if (comboAudioSampleRate3 != null) comboAudioSampleRate3.Text = value;
            }
        }

        public string VAC1BufferSize
        {
            get
            {
                if (comboAudioBuffer2 != null)
                    return comboAudioBuffer2.Text;
                else return "";
            }
            set
            {
                if (comboAudioBuffer2 != null)
                    comboAudioBuffer2.Text = value;
            }
        }

        public string VAC2BufferSize
        {
            get
            {
                if (comboAudioBuffer3 != null)
                    return comboAudioBuffer3.Text;
                else return "";
            }
            set
            {
                if (comboAudioBuffer3 != null)
                    comboAudioBuffer3.Text = value;
            }
        }

        public bool IQOutToVAC
        {
            get
            {
                if (chkAudioIQtoVAC != null) return chkAudioIQtoVAC.Checked;
                else return false;
            }
            set
            {
                if (chkAudioIQtoVAC != null) chkAudioIQtoVAC.Checked = value;
            }
        }

        public bool VAC2DirectIQ
        {
            get
            {
                if (chkVAC2DirectIQ != null) return chkVAC2DirectIQ.Checked;
                else return false;
            }
            set
            {
                if (chkVAC2DirectIQ != null) chkVAC2DirectIQ.Checked = value;
            }
        }

        public bool VAC1Calibrate
        {
            get
            {
                if (chkAudioCorrectIQ != null) return chkAudioIQtoVAC.Checked;
                else return false;
            }
            set
            {
                if (chkAudioCorrectIQ != null) chkAudioIQtoVAC.Checked = value;
            }
        }

        public bool VAC2Calibrate
        {
            get
            {
                if (chkVAC2DirectIQCal != null) return chkVAC2DirectIQ.Checked;
                else return false;
            }
            set
            {
                if (chkVAC2DirectIQCal != null) chkVAC2DirectIQ.Checked = value;
            }
        }

        public bool VACStereo
        {
            get
            {
                if (chkAudio2Stereo != null) return chkAudio2Stereo.Checked;
                else return false;
            }
            set
            {
                if (chkAudio2Stereo != null) chkAudio2Stereo.Checked = value;
            }
        }

        public bool VAC2Stereo
        {
            get
            {
                if (chkAudioStereo3 != null) return chkAudioStereo3.Checked;
                else return false;
            }
            set
            {
                if (chkAudioStereo3 != null) chkAudioStereo3.Checked = value;
            }
        }

        public bool SpurReduction
        {
            get
            {
                if (chkGeneralSpurRed != null) return chkGeneralSpurRed.Checked;
                else return true;
            }
            set
            {
                if (chkGeneralSpurRed != null) chkGeneralSpurRed.Checked = value;
            }
        }

        public int NoiseGate
        {
            get
            {
                if (udTXNoiseGate != null) return (int)udTXNoiseGate.Value;
                else return -1;
            }
            set
            {
                if (udTXNoiseGate != null) udTXNoiseGate.Value = value;
            }
        }

        public int VOXSens
        {
            get
            {
                if (udTXVOXThreshold != null) return (int)udTXVOXThreshold.Value;
                else return -1;
            }
            set
            {
                if (udTXVOXThreshold != null) udTXVOXThreshold.Value = value;
            }
        }

        public bool NoiseGateEnabled
        {
            get
            {
                if (chkTXNoiseGateEnabled != null) return chkTXNoiseGateEnabled.Checked;
                else return false;
            }
            set
            {
                if (chkTXNoiseGateEnabled != null) chkTXNoiseGateEnabled.Checked = value;
            }
        }

        public int VACRXGain
        {
            get
            {
                if (udAudioVACGainRX != null) return (int)udAudioVACGainRX.Value;
                else return -99;
            }
            set
            {
                if (udAudioVACGainRX != null) udAudioVACGainRX.Value = value;
            }
        }

        public int VAC2RXGain
        {
            get
            {
                if (udVAC2GainRX != null) return (int)udVAC2GainRX.Value;
                else return -99;
            }
            set
            {
                if (udVAC2GainRX != null) udVAC2GainRX.Value = value;
            }
        }

        public int VACTXGain
        {
            get
            {
                if (udAudioVACGainTX != null) return (int)udAudioVACGainTX.Value;
                else return -99;
            }
            set
            {
                if (udAudioVACGainTX != null) udAudioVACGainTX.Value = value;
            }
        }

        public int VAC2TXGain
        {
            get
            {
                if (udVAC2GainTX != null) return (int)udVAC2GainTX.Value;
                else return -99;
            }
            set
            {
                if (udVAC2GainTX != null) udVAC2GainTX.Value = value;
            }
        }

        public bool BreakInEnabled
        {
            get
            {
                if (chkCWBreakInEnabled != null)
                    return chkCWBreakInEnabled.Checked;
                else return false;
            }
            set
            {
                if (chkCWBreakInEnabled != null)
                    chkCWBreakInEnabled.Checked = value;
            }
        }

        private SoundCard current_sound_card = SoundCard.UNSUPPORTED_CARD;
        public SoundCard CurrentSoundCard
        {
            get { return current_sound_card; }
            set
            {
                current_sound_card = value;
                switch (value)
                {
                    case SoundCard.DELTA_44:
                        comboAudioSoundCard.Text = "M-Audio Delta 44 (PCI)";
                        break;
                    case SoundCard.FIREBOX:
                        comboAudioSoundCard.Text = "PreSonus FireBox (FireWire)";
                        break;
                    case SoundCard.EDIROL_FA_66:
                        comboAudioSoundCard.Text = "Edirol FA-66 (FireWire)";
                        break;
                    case SoundCard.AUDIGY:
                        comboAudioSoundCard.Text = "SB Audigy (PCI)";
                        break;
                    case SoundCard.AUDIGY_2:
                        comboAudioSoundCard.Text = "SB Audigy 2 (PCI)";
                        break;
                    case SoundCard.AUDIGY_2_ZS:
                        comboAudioSoundCard.Text = "SB Audigy 2 ZS (PCI)";
                        break;
                    case SoundCard.EXTIGY:
                        comboAudioSoundCard.Text = "Sound Blaster Extigy (USB)";
                        break;
                    case SoundCard.MP3_PLUS:
                        comboAudioSoundCard.Text = "Sound Blaster MP3+ (USB)";
                        break;
                    case SoundCard.SANTA_CRUZ:
                        comboAudioSoundCard.Text = "Turtle Beach Santa Cruz (PCI)";
                        break;
                    case SoundCard.UNSUPPORTED_CARD:
                        comboAudioSoundCard.Text = "Unsupported Card";
                        break;
                    case SoundCard.HPSDR:
                        comboAudioSoundCard.Text = "HPSDR";
                        break;
                }
            }
        }

        public bool VOXEnable
        {
            get
            {
                if (chkTXVOXEnabled != null) return chkTXVOXEnabled.Checked;
                else return false;
            }
            set
            {
                if (chkTXVOXEnabled != null) chkTXVOXEnabled.Checked = value;
            }
        }

        public int AGCMaxGain
        {
            get
            {
                if (udDSPAGCMaxGaindB != null) return (int)udDSPAGCMaxGaindB.Value;
                else return -1;
            }
            set
            {
                if (udDSPAGCMaxGaindB != null) udDSPAGCMaxGaindB.Value = value;
            }
        }

        public int AGCFixedGain
        {
            get
            {
                if (udDSPAGCFixedGaindB != null) return (int)udDSPAGCFixedGaindB.Value;
                else return -1;
            }
            set
            {
                if (udDSPAGCFixedGaindB != null) udDSPAGCFixedGaindB.Value = value;
            }
        }

        public int AGCRX2MaxGain
        {
            get
            {
                if (udDSPAGCRX2MaxGaindB != null) return (int)udDSPAGCRX2MaxGaindB.Value;
                else return -1;
            }
            set
            {
                if (udDSPAGCRX2MaxGaindB != null) udDSPAGCRX2MaxGaindB.Value = value;
            }
        }

        public int AGCRX2FixedGain
        {
            get
            {
                if (udDSPAGCRX2FixedGaindB != null) return (int)udDSPAGCRX2FixedGaindB.Value;
                else return -1;
            }
            set
            {
                if (udDSPAGCRX2FixedGaindB != null) udDSPAGCRX2FixedGaindB.Value = value;
            }
        }

        public int AGCHangThreshold
        {
            set
            {
                if (tbDSPAGCHangThreshold != null)
                {
                    tbDSPAGCHangThreshold.Value = value;
                    // tbDSPAGCHangThreshold_Scroll(this, EventArgs.Empty);
                }

            }
        }

        public int SetAGCRX2HangThreshold
        {
            set
            {
                if (tbDSPAGCRX2HangThreshold != null)
                {
                    //tbDSPAGCRX2HangThreshold.Value = value;
                    // tbDSPAGCRX2HangThreshold_Scroll(this, EventArgs.Empty);
                    if (value > tbDSPAGCRX2HangThreshold.Maximum) value = (int)tbDSPAGCRX2HangThreshold.Maximum;
                    if (value < tbDSPAGCRX2HangThreshold.Minimum) value = (int)tbDSPAGCRX2HangThreshold.Minimum;

                    if (tbDSPAGCRX2HangThreshold.Value == 0)
                    {
                        tbDSPAGCRX2HangThreshold.Value = value + 1;
                        tbDSPAGCRX2HangThreshold_Scroll(this, EventArgs.Empty);
                        tbDSPAGCRX2HangThreshold.Value = tbDSPAGCRX2HangThreshold.Value - 1;
                        tbDSPAGCRX2HangThreshold_Scroll(this, EventArgs.Empty);
                    }
                    else
                    {
                        tbDSPAGCRX2HangThreshold.Value = value - 1;
                        tbDSPAGCRX2HangThreshold_Scroll(this, EventArgs.Empty);
                        tbDSPAGCRX2HangThreshold.Value = tbDSPAGCRX2HangThreshold.Value + 1;
                        tbDSPAGCRX2HangThreshold_Scroll(this, EventArgs.Empty);
                    }
                }

            }
        }

        public int SetAGCHangThres
        {
            set
            {
                if (tbDSPAGCHangThreshold != null)
                {
                    if (value > tbDSPAGCHangThreshold.Maximum) value = (int)tbDSPAGCHangThreshold.Maximum;
                    if (value < tbDSPAGCHangThreshold.Minimum) value = (int)tbDSPAGCHangThreshold.Minimum;

                    if (tbDSPAGCHangThreshold.Value == 0)
                    {
                        tbDSPAGCHangThreshold.Value = value + 1;
                        tbDSPAGCHangThreshold_Scroll(this, EventArgs.Empty);
                        tbDSPAGCHangThreshold.Value = tbDSPAGCHangThreshold.Value - 1;
                        tbDSPAGCHangThreshold_Scroll(this, EventArgs.Empty);
                    }
                    else
                    {
                        tbDSPAGCHangThreshold.Value = value - 1;
                        tbDSPAGCHangThreshold_Scroll(this, EventArgs.Empty);
                        tbDSPAGCHangThreshold.Value = tbDSPAGCHangThreshold.Value + 1;
                        tbDSPAGCHangThreshold_Scroll(this, EventArgs.Empty);
                    }
                }
            }
        }

        public int TXFilterHigh
        {
            get { return (int)udTXFilterHigh.Value; }
            set
            {
                if (value > udTXFilterHigh.Maximum) value = (int)udTXFilterHigh.Maximum;
                if (value < udTXFilterHigh.Minimum) value = (int)udTXFilterHigh.Minimum;
                udTXFilterHigh.Value = value;
            }
        }

        public int TXFilterLow
        {
            get { return (int)udTXFilterLow.Value; }
            set
            {
                if (value > udTXFilterLow.Maximum) value = (int)udTXFilterLow.Maximum;
                if (value < udTXFilterLow.Minimum) value = (int)udTXFilterLow.Minimum;
                udTXFilterLow.Value = value;
            }
        }

        public int TXFilterHighSave
        {
            get { return (int)udTXFilterHighSave.Value; }
            set
            {
                if (console.TXBand != Band.B60M)
                {
                    if (value > udTXFilterHighSave.Maximum) value = (int)udTXFilterHighSave.Maximum;
                    if (value < udTXFilterHighSave.Minimum) value = (int)udTXFilterHighSave.Minimum;
                    udTXFilterHighSave.Value = value;
                }
            }
        }

        public int TXFilterLowSave
        {
            get { return (int)udTXFilterLowSave.Value; }
            set
            {
                if (console.TXBand != Band.B60M)
                {
                    if (value > udTXFilterLowSave.Maximum) value = (int)udTXFilterLowSave.Maximum;
                    if (value < udTXFilterLowSave.Minimum) value = (int)udTXFilterLowSave.Minimum;
                    udTXFilterLowSave.Value = value;
                }
            }
        }

        public bool Polyphase
        {
            get { return chkSpectrumPolyphase.Checked; }
            set { chkSpectrumPolyphase.Checked = value; }
        }

        public bool CustomRXAGCEnabled
        {
            set
            {
                udDSPAGCAttack.Enabled = value;
                udDSPAGCDecay.Enabled = value;
                udDSPAGCHangTime.Enabled = value;
                udDSPAGCRX2Attack.Enabled = value;
                udDSPAGCRX2Decay.Enabled = value;
                udDSPAGCRX2HangTime.Enabled = value;

                if (value)
                {
                    udDSPAGCAttack_ValueChanged(this, EventArgs.Empty);
                    udDSPAGCDecay_ValueChanged(this, EventArgs.Empty);
                    udDSPAGCHangTime_ValueChanged(this, EventArgs.Empty);
                    udDSPAGCRX2Attack_ValueChanged(this, EventArgs.Empty);
                    udDSPAGCRX2Decay_ValueChanged(this, EventArgs.Empty);
                    udDSPAGCRX2HangTime_ValueChanged(this, EventArgs.Empty);
                }
            }
        }

        public bool DirectX
        {
            set
            {
                if (value)
                {
                    if (!comboDisplayDriver.Items.Contains("DirectX"))
                        comboDisplayDriver.Items.Add("DirectX");
                }
                else
                {
                    if (comboDisplayDriver.Items.Contains("DirectX"))
                    {
                        comboDisplayDriver.Items.Remove("DirectX");
                        if (comboDisplayDriver.SelectedIndex < 0)
                            comboDisplayDriver.SelectedIndex = 0;
                    }
                }
            }
        }

        public bool VACEnable
        {
            get { return chkAudioEnableVAC.Checked; }
            set { chkAudioEnableVAC.Checked = value; }
        }

        public bool VAC2Enable
        {
            get { return chkVAC2Enable.Checked; }
            set { chkVAC2Enable.Checked = value; }
        }

        public int SoundCardIndex
        {
            get { return comboAudioSoundCard.SelectedIndex; }
            set { comboAudioSoundCard.SelectedIndex = value; }
        }

        private bool force_model = false;
        public Model CurrentModel
        {
            set
            {
                switch (value)
                {
                    case Model.SDR1000:
                        force_model = true;
                        radGenModelSDR1000.Checked = true;
                        break;
                    case Model.SOFTROCK40:
                        force_model = true;
                        radGenModelSoftRock40.Checked = true;
                        break;
                    case Model.DEMO:
                        force_model = true;
                        radGenModelDemoNone.Checked = true;
                        break;
                    case Model.HPSDR:
                        force_model = true;
                        radGenModelHPSDR.Checked = true;
                        break;
                    case Model.HERMES:
                        force_model = true;
                        radGenModelHermes.Checked = true;
                        break;

                }
            }
        }

        public void ResetFLEX5000()
        {
            radGenModelFLEX5000_CheckedChanged(this, EventArgs.Empty);
        }

        public bool RXOnly
        {
            get { return chkGeneralRXOnly.Checked; }
            set { chkGeneralRXOnly.Checked = value; }
        }

        private bool mox;
        public bool MOX
        {
            get { return mox; }
            set
            {
                mox = value;
                grpGeneralHardwareSDR1000.Enabled = !mox;
                if (comboAudioSoundCard.SelectedIndex == (int)SoundCard.UNSUPPORTED_CARD)
                    grpAudioDetails1.Enabled = !mox;
                grpAudioCard.Enabled = !mox;
                grpAudioBufferSize1.Enabled = !mox;
                grpAudioVolts1.Enabled = !mox;
                grpAudioLatency1.Enabled = !mox;
                chkAudioEnableVAC.Enabled = !mox;
                if (chkAudioEnableVAC.Checked)
                {
                    grpAudioDetails2.Enabled = !mox;
                    grpAudioBuffer2.Enabled = !mox;
                    grpAudioLatency2.Enabled = !mox;
                    grpAudioSampleRate2.Enabled = !mox;
                    grpAudio2Stereo.Enabled = !mox;
                }
                else
                {
                    grpAudioDetails2.Enabled = true;
                    grpAudioBuffer2.Enabled = true;
                    grpAudioLatency2.Enabled = true;
                    grpAudioSampleRate2.Enabled = true;
                    grpAudio2Stereo.Enabled = true;
                }
                grpDSPBufferSize.Enabled = !mox;
                grpTestAudioBalance.Enabled = !mox;
                if (!mox && !chekTestIMD.Checked && !chkGeneralRXOnly.Checked)
                    grpTestTXIMD.Enabled = !mox;
            }
        }

        public int TXAF
        {
            get { return (int)udTXAF.Value; }
            set { udTXAF.Value = value; }
        }

        public int AudioReceiveMux1
        {
            get { return comboAudioReceive1.SelectedIndex; }
            set
            {
                comboAudioReceive1.SelectedIndex = value;
                comboAudioReceive1_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        public bool USBPresent
        {
            get { return chkGeneralUSBPresent.Checked; }
            set { chkGeneralUSBPresent.Checked = value; }
        }

        public bool OzyControl
        {
            get { return chkBoxJanusOzyControl.Checked; }
            set { chkBoxJanusOzyControl.Checked = value; }
        }

        public bool XVTRPresent
        {
            get { return chkGeneralXVTRPresent.Checked; }
            set { chkGeneralXVTRPresent.Checked = value; }
        }

        public int XVTRSelection
        {
            get { return comboGeneralXVTR.SelectedIndex; }
            set { comboGeneralXVTR.SelectedIndex = value; }
        }

        public bool PAPresent
        {
            get { return chkGeneralPAPresent.Checked; }
            set { chkGeneralPAPresent.Checked = value; }
        }

        public bool ATUPresent
        {
            get { return chkGeneralATUPresent.Checked; }
            set { chkGeneralATUPresent.Checked = value; }
        }

        public bool SpurRedEnabled
        {
            get { return chkGeneralSpurRed.Enabled; }
            set { chkGeneralSpurRed.Enabled = value; }
        }

        public int PllMult
        {
            get { return (int)udDDSPLLMult.Value; }
            set { udDDSPLLMult.Value = value; }
        }

        public int ClockOffset
        {
            get { return (int)udDDSCorrection.Value; }
            set { udDDSCorrection.Value = value; }
        }

        public double HPSDRFreqCorrectFactor
        {
            get { return (double)udHPSDRFreqCorrectFactor.Value; }
            set { udHPSDRFreqCorrectFactor.Value = (decimal)value; }
        }


        public bool AlexPresent
        {
            get { return chkAlexPresent.Checked; }
            set { chkAlexPresent.Checked = value; }
        }

        public bool ExcaliburPresent
        {
            get { return chkExcaliburPresent.Checked; }
            set { chkExcaliburPresent.Checked = value; }
        }

        public bool MercuryPresent
        {
            get { return chkMercuryPresent.Checked; }
            set { chkMercuryPresent.Checked = value; }
        }

        public bool PenelopePresent
        {
            get { return chkPennyPresent.Checked; }
            set { chkPennyPresent.Checked = value; }
        }

        public bool PennyLanePresent
        {
            get { return chkPennyLane.Checked; }
            set { chkPennyLane.Checked = value; }
        }

        public bool PTTODelayControl
        {
            get { return chkPTTOutDelay.Checked; }
            set { chkPTTOutDelay.Checked = value; }
        }

        public float ImageGainTX
        {
            get { return (float)udDSPImageGainTX.Value; }
            set
            {
                try
                {
                    udDSPImageGainTX.Value = (decimal)value;
                }
                catch (Exception)
                {
                    MessageBox.Show("Error setting TX Image Gain (" + value.ToString("f2") + ")");
                }
            }
        }

        public float ImagePhaseTX
        {
            get { return (float)udDSPImagePhaseTX.Value; }
            set
            {
                try
                {
                    udDSPImagePhaseTX.Value = (decimal)value;
                }
                catch (Exception)
                {
                    MessageBox.Show("Error setting TX Image Phase (" + value.ToString("f2") + ")");
                }
            }
        }

        public float PAGain160
        {
            get { return (float)udPAGain160.Value; }
            set { udPAGain160.Value = (decimal)value; }
        }

        public float PAGain80
        {
            get { return (float)udPAGain80.Value; }
            set { udPAGain80.Value = (decimal)value; }
        }

        public float PAGain60
        {
            get { return (float)udPAGain60.Value; }
            set { udPAGain60.Value = (decimal)value; }
        }

        public float PAGain40
        {
            get { return (float)udPAGain40.Value; }
            set { udPAGain40.Value = (decimal)value; }
        }

        public float PAGain30
        {
            get { return (float)udPAGain30.Value; }
            set { udPAGain30.Value = (decimal)value; }
        }

        public float PAGain20
        {
            get { return (float)udPAGain20.Value; }
            set { udPAGain20.Value = (decimal)value; }
        }

        public float PAGain17
        {
            get { return (float)udPAGain17.Value; }
            set { udPAGain17.Value = (decimal)value; }
        }

        public float PAGain15
        {
            get { return (float)udPAGain15.Value; }
            set { udPAGain15.Value = (decimal)value; }
        }

        public float PAGain12
        {
            get { return (float)udPAGain12.Value; }
            set { udPAGain12.Value = (decimal)value; }
        }

        public float PAGain10
        {
            get { return (float)udPAGain10.Value; }
            set { udPAGain10.Value = (decimal)value; }
        }

        public float PAGain6
        {
            get { return (float)udPAGain6.Value; }
            set { udPAGain6.Value = (decimal)value; }
        }

        public float PAGainVHF0
        {
            get { return (float)udPAGainVHF0.Value; }
            set { udPAGainVHF0.Value = (decimal)value; }
        }

        public float PAGainVHF1
        {
            get { return (float)udPAGainVHF1.Value; }
            set { udPAGainVHF1.Value = (decimal)value; }
        }

        public float PAGainVHF2
        {
            get { return (float)udPAGainVHF2.Value; }
            set { udPAGainVHF2.Value = (decimal)value; }
        }

        public float PAGainVHF3
        {
            get { return (float)udPAGainVHF3.Value; }
            set { udPAGainVHF3.Value = (decimal)value; }
        }

        public float PAGainVHF4
        {
            get { return (float)udPAGainVHF4.Value; }
            set { udPAGainVHF4.Value = (decimal)value; }
        }

        public float PAGainVHF5
        {
            get { return (float)udPAGainVHF5.Value; }
            set { udPAGainVHF5.Value = (decimal)value; }
        }

        public float PAGainVHF6
        {
            get { return (float)udPAGainVHF6.Value; }
            set { udPAGainVHF6.Value = (decimal)value; }
        }

        public float PAGainVHF7
        {
            get { return (float)udPAGainVHF7.Value; }
            set { udPAGainVHF7.Value = (decimal)value; }
        }

        public float PAGainVHF8
        {
            get { return (float)udPAGainVHF8.Value; }
            set { udPAGainVHF8.Value = (decimal)value; }
        }

        public float PAGainVHF9
        {
            get { return (float)udPAGainVHF9.Value; }
            set { udPAGainVHF9.Value = (decimal)value; }
        }

        public float PAGainVHF10
        {
            get { return (float)udPAGainVHF10.Value; }
            set { udPAGainVHF10.Value = (decimal)value; }
        }
        public float PAGainVHF11
        {
            get { return (float)udPAGainVHF11.Value; }
            set { udPAGainVHF11.Value = (decimal)value; }
        }

        public float PAGainVHF12
        {
            get { return (float)udPAGainVHF12.Value; }
            set { udPAGainVHF12.Value = (decimal)value; }
        }

        public float PAGainVHF13
        {
            get { return (float)udPAGainVHF13.Value; }
            set { udPAGainVHF13.Value = (decimal)value; }
        }

        public float PAADC160
        {
            get { return (float)udPAADC160.Value; }
            set { udPAADC160.Value = (decimal)value; }
        }

        public float PAADC80
        {
            get { return (float)udPAADC80.Value; }
            set { udPAADC80.Value = (decimal)value; }
        }

        public float PAADC60
        {
            get { return (float)udPAADC60.Value; }
            set { udPAADC60.Value = (decimal)value; }
        }

        public float PAADC40
        {
            get { return (float)udPAADC40.Value; }
            set { udPAADC40.Value = (decimal)value; }
        }

        public float PAADC30
        {
            get { return (float)udPAADC30.Value; }
            set { udPAADC30.Value = (decimal)value; }
        }

        public float PAADC20
        {
            get { return (float)udPAADC20.Value; }
            set { udPAADC20.Value = (decimal)value; }
        }

        public float PAADC17
        {
            get { return (float)udPAADC17.Value; }
            set { udPAADC17.Value = (decimal)value; }
        }

        public float PAADC15
        {
            get { return (float)udPAADC15.Value; }
            set { udPAADC15.Value = (decimal)value; }
        }

        public float PAADC12
        {
            get { return (float)udPAADC12.Value; }
            set { udPAADC12.Value = (decimal)value; }
        }

        public float PAADC10
        {
            get { return (float)udPAADC10.Value; }
            set { udPAADC10.Value = (decimal)value; }
        }

        public int TunePower
        {
            get { return (int)udTXTunePower.Value; }
            set { udTXTunePower.Value = (decimal)value; }
        }

        public bool DigUIsUSB
        {
            get { return chkDigUIsUSB.Checked; }
        }

        public int DigU_CT_Offset
        {
            get { return (int)udOptClickTuneOffsetDIGU.Value; }
            set { udOptClickTuneOffsetDIGU.Value = value; }
        }

        public int DigL_CT_Offset
        {
            get { return (int)udOptClickTuneOffsetDIGL.Value; }
            set { udOptClickTuneOffsetDIGL.Value = value; }
        }

        // Added 06/21/05 BT for CAT commands

        public int CATNB1Threshold
        {
            get { return Convert.ToInt32(udDSPNB.Value); }
            set
            {
                value = (int)Math.Max(udDSPNB.Minimum, value);			// lower bound
                value = (int)Math.Min(udDSPNB.Maximum, value);			// upper bound
                udDSPNB.Value = value;
            }
        }

        // Added 06/21/05 BT for CAT commands
        public int CATNB2Threshold
        {
            get { return Convert.ToInt32(udDSPNB2.Value); }
            set
            {
                value = (int)Math.Max(udDSPNB2.Minimum, value);
                value = (int)Math.Min(udDSPNB2.Maximum, value);
                udDSPNB2.Value = value;
            }
        }

        // Added 06/21/05 BT for CAT commands
        /*public int CATCompThreshold
        {
            get{return Convert.ToInt32(udTXFFCompression.Value);}
            set
            {
                value = (int)Math.Max(udTXFFCompression.Minimum, value);
                value = (int)Math.Min(udTXFFCompression.Maximum, value);
                udTXFFCompression.Value = value;
            }
        }*/

        // Added 06/30/05 BT for CAT commands
        public int CATCWPitch
        {
            get { return (int)udDSPCWPitch.Value; }
            set
            {
                value = (int)Math.Max(udDSPCWPitch.Minimum, value);
                value = (int)Math.Min(udDSPCWPitch.Maximum, value);
                udDSPCWPitch.Value = value;
            }
        }

        // Added 07/07/05 BT for CAT commands
        public void CATSetRig(string rig)
        {
            comboCATRigType.Text = rig;
        }


        // Added 06/30/05 BT for CAT commands
        //		public int CATTXPreGain
        //		{
        //			get{return (int) udTXPreGain.Value;}
        //			set
        //			{
        //				value = Math.Max(-30, value);
        //				value = Math.Min(70, value);
        //				udTXPreGain.Value = value;
        //			}
        //		}

        public int DSPPhoneRXBuffer
        {
            get { return Int32.Parse(comboDSPPhoneRXBuf.Text); }
            set
            {
                string temp = value.ToString();
                if (comboDSPPhoneRXBuf.Items.Contains(temp))
                    comboDSPPhoneRXBuf.SelectedItem = temp;
            }
        }

        public int DSPPhoneTXBuffer
        {
            get { return Int32.Parse(comboDSPPhoneTXBuf.Text); }
            set
            {
                string temp = value.ToString();
                if (comboDSPPhoneTXBuf.Items.Contains(temp))
                    comboDSPPhoneTXBuf.SelectedItem = temp;
            }
        }

        public int DSPCWRXBuffer
        {
            get { return Int32.Parse(comboDSPCWRXBuf.Text); }
            set
            {
                string temp = value.ToString();
                if (comboDSPCWRXBuf.Items.Contains(temp))
                    comboDSPCWRXBuf.SelectedItem = temp;
            }
        }

        public int DSPCWTXBuffer
        {
            get { return Int32.Parse(comboDSPCWTXBuf.Text); }
            set
            {
                string temp = value.ToString();
                if (comboDSPCWTXBuf.Items.Contains(temp))
                    comboDSPCWTXBuf.SelectedItem = temp;
            }
        }

        public int DSPDigRXBuffer
        {
            get { return Int32.Parse(comboDSPDigRXBuf.Text); }
            set
            {
                string temp = value.ToString();
                if (comboDSPDigRXBuf.Items.Contains(temp))
                    comboDSPDigRXBuf.SelectedItem = temp;
            }
        }

        public int DSPDigTXBuffer
        {
            get { return Int32.Parse(comboDSPDigTXBuf.Text); }
            set
            {
                string temp = value.ToString();
                if (comboDSPDigTXBuf.Items.Contains(temp))
                    comboDSPDigTXBuf.SelectedItem = temp;
            }
        }

        public int AudioBufferSize
        {
            get { return Int32.Parse(comboAudioBuffer1.Text); }
            set
            {
                string temp = value.ToString();
                if (comboAudioBuffer1.Items.Contains(temp))
                    comboAudioBuffer1.SelectedItem = temp;
            }
        }

        public bool FlexProfilerInstalled { get; set; }

        private bool allow_freq_broadcast = false;
        public bool AllowFreqBroadcast
        {
            get { return allow_freq_broadcast; }
            set
            {
                allow_freq_broadcast = value;
                console.KWAutoInformation = value;
            }
        }

        private bool rtty_offset_enabled_a;
        public bool RttyOffsetEnabledA
        {
            get { return rtty_offset_enabled_a; }
            set { chkRTTYOffsetEnableA.Checked = value; }
        }

        private bool rtty_offset_enabled_b;
        public bool RttyOffsetEnabledB
        {
            get { return rtty_offset_enabled_b; }
            set { chkRTTYOffsetEnableB.Checked = value; }
        }

        private int rtty_offset_high = 2125;
        public int RttyOffsetHigh
        {
            get { return rtty_offset_high; }
            set
            {
                value = (int)Math.Max(udRTTYU.Minimum, value);
                value = (int)Math.Min(udRTTYU.Maximum, value);
                udRTTYU.Value = value;
            }
        }

        private int rtty_offset_low = 2125;
        public int RttyOffsetLow
        {
            get { return rtty_offset_low; }
            set
            {
                value = (int)Math.Max(udRTTYL.Minimum, value);
                value = (int)Math.Min(udRTTYL.Maximum, value);
                udRTTYL.Value = value;
            }
        }

        public float MeterOffset
        {
            get
            {
                return float.Parse(txtMeterOffset.Text);
            }
            set
            {
                txtMeterOffset.Text = value.ToString();
            }
        }

        public float DisplayOffset
        {
            get
            {
                return float.Parse(txtDisplayOffset.Text);
            }
            set
            {
                txtDisplayOffset.Text = value.ToString();
            }
        }

        private bool firmware_bypass = false;
        public bool FirmwareBypass
        {
            get
            {
                return firmware_bypass;
            }
            set
            {
                firmware_bypass = value;
            }
        }


        #endregion

        #region General Tab Event Handlers
        // ======================================================
        // General Tab Event Handlers
        // ======================================================

        private void radGenModelFLEX5000_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelFLEX5000.Checked)
            {
                if (!console.fwc_init)
                {
                    // console.fwc_init = Pal.Init();
                    if (console.fwc_init)
                    {
                        FWCEEPROM.Init();
                        console.CurrentRegion = FWCEEPROM.Region;
                        FWC.SetPalCallback();
                    }
                }
                if (console.fwc_init)
                {
                    switch (FWCEEPROM.Model)
                    {
                        case 0:
                        case 1:
                        case 2:
                            console.CurrentModel = Model.FLEX5000;
                            radGenModelFLEX5000.Text = "FLEX-5000";
                            grpGeneralHardwareFLEX5000.Text = "FLEX-5000 Config";
                            lblRFIORev.Visible = true;
                            break;
                        case 3:
                            console.CurrentModel = Model.FLEX3000;
                            radGenModelFLEX5000.Text = "FLEX-3000";
                            grpGeneralHardwareFLEX5000.Text = "FLEX-3000 Config";
                            lblRFIORev.Visible = false;
                            chkGenFLEX5000ExtRef.Visible = false;
                            udTXFilterHigh.Maximum = 3650;
                            if (comboAudioSampleRate1.Items.Contains(192000))
                                comboAudioSampleRate1.Items.Remove(192000);
                            if (comboAudioSampleRate1.SelectedIndex == -1)
                                comboAudioSampleRate1.SelectedIndex = 1;
                            lblF3KFanTempThresh.Visible = true;
                            udF3KFanTempThresh.Visible = true;
                            chkGenTX1Delay.Visible = true;
                            lblGenTX1Delay.Visible = true;
                            udGenTX1Delay.Visible = true;
                            chkSigGenRX2.Visible = false;
                            lblModel.Visible = false;
                            lblSerialNum.Left = 16;
                            chkGenOptionsShowATUPopup.Visible = true;
                            break;
                    }
                }
                comboAudioSoundCard.Text = "Unsupported Card";
                //comboAudioSampleRate1.Text = "96000";

                foreach (PADeviceInfo p in comboAudioDriver1.Items)
                {
                    if (p.Name == "ASIO")
                    {
                        comboAudioDriver1.SelectedItem = p;
                        break;
                    }
                }

                foreach (PADeviceInfo dev in comboAudioInput1.Items)
                {
                    if (dev.Name.ToLower().IndexOf("flex") >= 0)
                    {
                        comboAudioInput1.Text = dev.Name;
                        comboAudioOutput1.Text = dev.Name;
                        break;
                    }
                }

                if (comboAudioBuffer1.Items.Contains("256"))
                    comboAudioBuffer1.Items.Remove("256");

                udAudioVoltage1.Value = 1.0M;

                comboAudioMixer1.Text = "None";

                if (comboAudioInput1.Text.ToLower().IndexOf("flex") < 0)
                {
                    /*MessageBox.Show("FLEX-5000 hardware not found.  Please check " +
                        "the following:\n" +
                        "\t1. Verify that the unit has power and is running (note blue LED).\n" +
                        "\t2. Verify FireWire cable is securely plugged in on both ends.\n" +
                        "\t3. Verify that the driver is installed properly and the device shows up as FLEX 5000 in the device manager.\n" +
                        "Note that after correcting any of these issues, you must restart PowerSDR for the changes to take effect.\n" +
                        "For more support, see our website at www.flex-radio.com or email support@flex-radio.com.",
                        "Hardware Not Found",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);*/
                    console.PowerEnabled = false;
                }
                else
                {
                    bool trx_ok, pa_ok, rfio_ok, rx2_ok, atu_ok;
                    trx_ok = FWCEEPROM.TRXOK;
                    pa_ok = FWCEEPROM.PAOK;
                    rfio_ok = FWCEEPROM.RFIOOK;
                    rx2_ok = FWCEEPROM.RX2OK;

                    chkSigGenRX2.Visible = rx2_ok;

                    if (console.CurrentModel == Model.FLEX5000)
                        FWC.GetATUOK(out atu_ok);
                    else atu_ok = true;

                    switch (FWCEEPROM.Model)
                    {
                        case 0: lblModel.Text = "Model: A"; break;
                        case 1: lblModel.Text = "Model: C"; break;
                        case 2: lblModel.Text = "Model: D"; break;
                    }

                    lblSerialNum.Text = "S/N: " + FWCEEPROM.SerialToString(FWCEEPROM.SerialNumber);

                    uint val;
                    FWC.GetFirmwareRev(out val);
                    string s = "Firmware: ";
                    s += ((byte)(val >> 24)).ToString() + ".";
                    s += ((byte)(val >> 16)).ToString() + ".";
                    s += ((byte)(val >> 8)).ToString() + ".";
                    s += ((byte)(val >> 0)).ToString();
                    lblFirmwareRev.Text = s;

                    s = "TRX: " + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial);
                    val = FWCEEPROM.TRXRev;
                    s += "  (" + ((byte)(val >> 0)).ToString();
                    s += ((char)(((byte)(val >> 8)) + 65)).ToString() + ")";
                    lblTRXRev.Text = s;
                    if (!trx_ok) lblTRXRev.ForeColor = Color.Red;

                    s = "PA: " + FWCEEPROM.SerialToString(FWCEEPROM.PASerial);
                    val = FWCEEPROM.PARev;
                    s += "  (" + ((byte)(val >> 0)).ToString();
                    s += ((char)(((byte)(val >> 8)) + 65)).ToString() + ")";
                    lblPARev.Text = s;
                    if (!pa_ok) lblPARev.ForeColor = Color.Red;

                    s = "RFIO: " + FWCEEPROM.SerialToString(FWCEEPROM.RFIOSerial);
                    val = FWCEEPROM.RFIORev;
                    s += "  (" + ((byte)(val >> 0)).ToString();
                    s += ((char)(((byte)(val >> 8)) + 65)).ToString() + ")";
                    lblRFIORev.Text = s;
                    if (!rfio_ok) lblRFIORev.ForeColor = Color.Red;

                    /*s = "ATU: ";
                    FWC.GetATUSN(out val);
                    s += ((byte)(val>>0)).ToString("00");
                    s += ((byte)(val>>8)).ToString("00")+"-";
                    s += ((ushort)(val>>16)).ToString("0000");
                    FWC.GetATURev(out val);
                    s += "  ("+((byte)(val>>0)).ToString();
                    s += ((char)(((byte)(val>>8))+65)).ToString()+")";*/
                    lblATURev.Text = "ATU: Present";
                    if (!atu_ok) lblATURev.Visible = false;

                    if (console.CurrentModel == Model.FLEX5000)
                    {
                        s = "RX2: " + FWCEEPROM.SerialToString(FWCEEPROM.RX2Serial);
                        val = FWCEEPROM.RX2Rev; FWC.GetRX2Rev(out val);
                        s += "  (" + ((byte)(val >> 0)).ToString();
                        s += ((char)(((byte)(val >> 8)) + 65)).ToString() + ")";
                    }
                    else s = "RX2: ";
                    lblRX2Rev.Text = s;
                    if (!rx2_ok) lblRX2Rev.Visible = false;


                    /*if(rx2_ok) 
                    {
                        console.radio.GetDSPRX(1, 0).Active = true;
                        DttSP.SetThreadProcessingMode(2, 2);
                        RadioDSP.SetThreadNumber(3);
                        Audio.RX2Enabled = true;
                    }
                    else*/
                    RadioDSP.SetThreadNumber(2);

                    string key = comboKeyerConnPrimary.Text;
                    if (comboKeyerConnPrimary.Items.Contains("SDR"))
                        comboKeyerConnPrimary.Items.Remove("SDR");
                    if (!comboKeyerConnPrimary.Items.Contains("Radio"))
                        comboKeyerConnPrimary.Items.Insert(0, "Radio");
                    if (key == "SDR" || key == "5000") comboKeyerConnPrimary.Text = "Radio";
                    else comboKeyerConnPrimary.Text = key;
                    comboKeyerConnPrimary_SelectedIndexChanged(this, EventArgs.Empty);
                    chkPANewCal.Checked = true;
                }
            }
            else console.PowerEnabled = true;

            bool b = radGenModelFLEX5000.Checked;
            radPACalAllBands_CheckedChanged(this, EventArgs.Empty);
            grpGeneralHardwareSDR1000.Visible = !b;
            grpGeneralHardwareFLEX5000.Visible = b;
            btnWizard.Visible = !b;
            grpGenAutoMute.Visible = !b;

            grpAudioDetails1.Visible = !b;
            grpAudioCard.Visible = !b;
            grpAudioLineInGain1.Visible = !b;
            grpAudioMicInGain1.Visible = !b;
            grpAudioChannels.Visible = !b;
            grpAudioVolts1.Visible = !b;

            chkGeneralSoftwareGainCorr.Visible = false;
            chkGeneralEnableX2.Visible = !b;
            lblGeneralX2Delay.Visible = !b;
            udGeneralX2Delay.Visible = !b;
            chkGeneralCustomFilter.Visible = !b;

            chkCalExpert.Visible = b;
            chkCalExpert_CheckedChanged(this, EventArgs.Empty);
            if (!b)
            {
                grpGeneralCalibration.Visible = true;
                grpGenCalLevel.Visible = true;
                grpGenCalRXImage.Visible = true;
            }

            chkDSPImageExpert.Visible = b;
            chkDSPImageExpert_CheckedChanged(this, EventArgs.Empty);
            if (!b)
            {
                grpDSPImageRejectTX.Visible = true;
            }

            grpPAGainByBand.Visible = true;
            chkPANewCal.Visible = false;

            rtxtPACalReq.Visible = !b;

            if (b)
            {
                if (tcSetup.TabPages.Contains(tpExtCtrl))
                {
                    tcSetup.TabPages.Remove(tpExtCtrl);
                    tcSetup.SelectedIndex = 0;
                }
            }
            else
            {
                if (!tcSetup.TabPages.Contains(tpExtCtrl))
                    Common.TabControlInsert(tcSetup, tpExtCtrl, 6);
            }

            grpImpulseTest.Visible = !b;
            ckEnableSigGen.Visible = b;
            grpTestX2.Visible = !b;
        }

        private void radGenModelSDR1000_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelSDR1000.Checked)
            {
                console.CurrentModel = Model.SDR1000;
                comboGeneralLPTAddr_SelectedIndexChanged(this, EventArgs.Empty);
                chkGeneralUSBPresent_CheckedChanged(this, EventArgs.Empty);
                chkGeneralPAPresent_CheckedChanged(this, EventArgs.Empty);
                chkGeneralATUPresent_CheckedChanged(this, EventArgs.Empty);
                chkXVTRPresent_CheckedChanged(this, EventArgs.Empty);
                comboGeneralXVTR_SelectedIndexChanged(this, EventArgs.Empty);

                if (radGenModelSDR1000.Focused || force_model)
                {
                    chkGeneralRXOnly.Checked = false;
                    chkGeneralDisablePTT.Checked = false;
                    force_model = false;
                }
                chkGeneralRXOnly.Enabled = true;

                string key = comboKeyerConnPrimary.Text;
                if (comboKeyerConnPrimary.Items.Contains("Radio"))
                    comboKeyerConnPrimary.Items.Remove("Radio");
                if (!comboKeyerConnPrimary.Items.Contains("SDR"))
                    comboKeyerConnPrimary.Items.Insert(0, "SDR");
                comboKeyerConnPrimary.Text = key == "Radio" ? "SDR" : key;
                comboKeyerConnPrimary_SelectedIndexChanged(this, EventArgs.Empty);
                lblF3KFanTempThresh.Visible = false;
                udF3KFanTempThresh.Visible = false;
                chkGenTX1Delay.Visible = false;
                lblGenTX1Delay.Visible = false;
                udGenTX1Delay.Visible = false;
                grpHWSoftRock.Visible = false;
            }
            else console.XVTRPresent = false;
        }

        private void radGenModelHermes_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelHermes.Checked)
            {
                console.CurrentModel = Model.HERMES;
                chkPennyPresent.Checked = false;
                chkPennyPresent.Enabled = false;
                chkMercuryPresent.Checked = true;
                chkMercuryPresent.Enabled = false;
                chkJanusPresent.Checked = false;
                chkJanusPresent.Enabled = false;
                chkExcaliburPresent.Checked = false;
                chkExcaliburPresent.Enabled = false;
                chkPennyLane.Checked = true;
                chkPennyLane.Enabled = false;
                radPenny10MHz.Checked = true;
                rad12288MHzPenny.Checked = true;
                groupBox10MhzClock.Visible = false;
                groupBox122MHz.Visible = false;
                groupBoxMicSource.Visible = false;
                chkGeneralRXOnly.Visible = false;
                chkGeneralRXOnly.Checked = false;
                groupBoxRXOptions.Text = "Hermes Options";
                radMetis.Text = "Hermes";
                grpMetisAddr.Text = "Hermes Address";
                string key = comboKeyerConnPrimary.Text;
                if (comboKeyerConnPrimary.Items.Contains("5000"))
                    comboKeyerConnPrimary.Items.Remove("5000");
                if (comboKeyerConnPrimary.Items.Contains("SDR"))
                    comboKeyerConnPrimary.Items.Remove("SDR");
                if (!comboKeyerConnPrimary.Items.Contains("Ozy/Hermes"))
                    comboKeyerConnPrimary.Items.Insert(0, "Ozy/Hermes");
                comboKeyerConnPrimary.Text = !key.StartsWith("COM") ? "Ozy/Hermes" : key;
                comboKeyerConnPrimary_SelectedIndexChanged(this, EventArgs.Empty);

            }
            radGenModelHPSDR_or_Hermes_CheckedChanged(sender, e, true);
        }

        private void radGenModelHPSDR_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelHPSDR.Checked)
            {
                console.CurrentModel = Model.HPSDR;
                chkPennyPresent.Enabled = true;
                chkPennyLane.Enabled = true;
                chkMercuryPresent.Enabled = true;
                chkJanusPresent.Enabled = true;
                chkExcaliburPresent.Enabled = true;
                groupBox10MhzClock.Visible = true;
                groupBox122MHz.Visible = true;
                groupBoxMicSource.Visible = true;
                chkGeneralRXOnly.Visible = true;
                groupBoxRXOptions.Text = "Mercury Options";
                radMetis.Text = "Metis (Ethernet)";
                grpMetisAddr.Text = "Metis Address";
                string key = comboKeyerConnPrimary.Text;
                if (comboKeyerConnPrimary.Items.Contains("5000"))
                    comboKeyerConnPrimary.Items.Remove("5000");
                if (comboKeyerConnPrimary.Items.Contains("SDR"))
                    comboKeyerConnPrimary.Items.Remove("SDR");
                if (!comboKeyerConnPrimary.Items.Contains("Ozy/Hermes"))
                    comboKeyerConnPrimary.Items.Insert(0, "Ozy/Hermes");
                comboKeyerConnPrimary.Text = !key.StartsWith("COM") ? "Ozy/Hermes" : key;
                comboKeyerConnPrimary_SelectedIndexChanged(this, EventArgs.Empty);

            }
            radGenModelHPSDR_or_Hermes_CheckedChanged(sender, e, false);

            if (radMetis.Checked)
            {
                console.HPSDRisMetis = true;
                grpMetisAddr.Visible = true;
            }

        }

        private void radGenModelSoftRock40_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelSoftRock40.Checked)
            {
                chkGeneralDisablePTT.Checked = true;
                chkGeneralUseSi570.Visible = true;
                console.CurrentModel = Model.SOFTROCK40;
                if (radGenModelSoftRock40.Focused || force_model)
                {
                    chkGeneralRXOnly.Checked = true;
                    chkGeneralDisablePTT.Checked = true;
                    force_model = false;
                }
                chkGeneralRXOnly.Enabled = true; // modif F8CHK
                lblF3KFanTempThresh.Visible = false;
                udF3KFanTempThresh.Visible = false;
                chkGenTX1Delay.Visible = false;
                lblGenTX1Delay.Visible = false;
                udGenTX1Delay.Visible = false;

                grpHWSoftRock.Visible = !console.si570_used;  // modif F8CHK
                grpSI570.Visible = console.si570_used;
            }
            else
            {
                chkGeneralUseSi570.Visible = false; // modif F8CHK    
                grpSI570.Visible = false;
            }
        }

        private void radGenModelDemoNone_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelDemoNone.Checked)
            {
                console.CurrentModel = Model.DEMO;
                //if(radGenModelDemoNone.Focused || force_model)
                {
                    chkGeneralRXOnly.Checked = true;
                    chkGeneralDisablePTT.Checked = true;
                    MessageBox.Show("Welcome to the Demo/Test mode of the PowerSDR software.\n" +
                        "\nPlease contact us at support@flex-radio.com or call (512) 250-8595 with any questions.\n" +
                        "\nIf you did not intend to be in Demo/Test mode, please open the Setup Form and change the model\n" +
                        "to the appropriate selection (FLEX-5000, SDR-1000, etc) and then restart PowerSDR.",
                        "Welcome to Demo/Test Mode",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    force_model = false;
                    lblF3KFanTempThresh.Visible = false;
                    udF3KFanTempThresh.Visible = false;
                    chkGenTX1Delay.Visible = false;
                    lblGenTX1Delay.Visible = false;
                    udGenTX1Delay.Visible = false;
                }
                chkGeneralRXOnly.Enabled = true;
                RadioDSP.SetThreadNumber(1);
            }
        }

        private void radGenModelHPSDR_or_Hermes_CheckedChanged(object sender, System.EventArgs e, bool is_hermes)
        {
            // add or remove setup pages for HPSDR stuff 
            //
            bool b = is_hermes ? radGenModelHermes.Checked : radGenModelHPSDR.Checked;

            if (b)
            {
                AddHPSDRPages();

                // disable iq correction -- not needed with mercury and penelope 
                DttSP.SetCorrectIQEnable(0); // turn off I/Q correction
                DttSP.SetCorrectRXIQw(0, 0, 0.0f, 0.0f, 0);
                DttSP.SetCorrectRXIQw(0, 0, 0.0f, 0.0f, 1);
                udDSPImagePhaseTX.Value = 0.0M;
                udDSPImageGainTX.Value = 0.0M;
                grpDSPImageRejectTX.Enabled = false;
                // force setting of audio card 
                comboAudioSoundCard.Text = "HPSDR";
                comboAudioSoundCard.Enabled = false;
                // comboAudioSampleRate1.Text = "192000"; 
                // comboAudioSampleRate1.Enabled = false;
                udAudioVoltage1.Value = 0.80M;
                udAudioVoltage1.Enabled = false;
                btnAudioVoltTest1.Visible = false;
                // and enable the gain by band page 
                grpFRSRegion.Visible = true;
                grpPAGainByBand.Visible = true;
                grpGeneralHardwareSDR1000.Visible = false;
                grpGeneralDDS.Visible = false;
                btnWizard.Visible = true;
                //chkGeneralRXOnly.Checked = false;
                //chkGeneralRXOnly.Enabled = true;
                grpHWSoftRock.Visible = false;
                chkGeneralCustomFilter.Visible = false;
                grpGenAutoMute.Visible = false;
                grpGenCalRXImage.Visible = false;
                grpTestX2.Visible = false;
                chkGenTX1Delay.Visible = false;
                lblGenTX1Delay.Visible = false;
                udGenTX1Delay.Visible = false;
                chkGeneralEnableX2.Visible = false;
                lblGeneralX2Delay.Visible = false;
                udGeneralX2Delay.Visible = false;
                rtxtPACalReq.Visible = false;
                lblPTTOutDelay.Visible = true;
                udGenPTTOutDelay.Visible = true;
                lblMoxDelay.Visible = true;
                udMoxDelay.Visible = true;
                udMoxDelay.Enabled = true;
                chkPTTOutDelay.Visible = true;
                chkPTTOutDelay.Enabled = true;
                udRFDelay.Visible = true;
                udRFDelay.Enabled = true;
                lblRFDelay.Visible = true;
                grpImpulseTest.Visible = false;
                chkCWKeyerMonoCable.Visible = false;
                // chkGeneralPAPresent.Checked = true;

                if (is_hermes)
                {
                    groupBoxHPSDRHW.Visible = true;
                    grpOzyType.Visible = true;
                    grpOzyType.Enabled = true;
                    // make sure one of these is checked 
                    //  if (radOzyUSB.Checked == false && radMetis.Checked == false)
                    //  {
                    radMetis.Checked = true;
                    grpMetisAddr.Visible = true;
                    // }
                    radOzyUSB.Enabled = false;
                }
                else
                {
                    groupBoxHPSDRHW.Visible = true;
                    grpOzyType.Visible = true;
                    grpOzyType.Enabled = true;
                    radOzyUSB.Enabled = true;
                    if (radOzyUSB.Checked == false && radMetis.Checked == false)
                    {
                        radOzyUSB.Checked = true;
                    }
                }

                chkGeneralSpurRed.Visible = false;
                chkGeneralSpurRed.Checked = false;
                chkGeneralSpurRed.Enabled = false;
                chkAudioExpert.Checked = false;
                chkAudioExpert.Visible = false;
                // console.DisableSR();
                chkGeneralEnableX2.Checked = false;
                chkGeneralEnableX2.Enabled = true;

                // grpGeneralCalibration.Enabled = false; 
                grpGenCalRXImage.Enabled = false;
                chkCalExpert.Enabled = false;
                grpHPSDRFreqCalDbg.Visible = true;

                if (tcSetup.TabPages.Contains(tpExtCtrl))
                {
                    tcSetup.TabPages.Remove(tpExtCtrl);
                    tcSetup.SelectedIndex = 0;
                }

                string key = comboKeyerConnPrimary.Text;
                if (comboKeyerConnPrimary.Items.Contains("5000"))
                    comboKeyerConnPrimary.Items.Remove("5000");
                if (comboKeyerConnPrimary.Items.Contains("SDR"))
                    comboKeyerConnPrimary.Items.Remove("SDR");
                if (!comboKeyerConnPrimary.Items.Contains("Ozy/Hermes"))
                    comboKeyerConnPrimary.Items.Insert(0, "Ozy/Hermes");
                comboKeyerConnPrimary.Text = !key.StartsWith("COM") ? "Ozy/Hermes" : key;
                comboKeyerConnPrimary_SelectedIndexChanged(this, EventArgs.Empty);

            }
            else
            {
                RemoveHPSDRPages();
                grpDSPImageRejectTX.Enabled = true;

                grpOzyType.Visible = false;
                grpOzyType.Enabled = false;
                grpMetisAddr.Visible = false;

                comboAudioSoundCard.Enabled = true;
                comboAudioSampleRate1.Enabled = true;
                udAudioVoltage1.Enabled = true;
                btnAudioVoltTest1.Visible = true;

                grpPAGainByBand.Visible = true;
                grpGeneralHardwareSDR1000.Visible = true;
                grpGeneralDDS.Visible = true;
                grpGenCalRXImage.Visible = true;

                groupBoxHPSDRHW.Visible = false;

                chkAudioExpert.Visible = true;

                chkGeneralSpurRed.Checked = true;
                chkGeneralSpurRed.Enabled = true;

                chkGeneralSpurRed.Checked = true;
                chkGeneralSpurRed.Enabled = true;

                // console.EnableSR();

                chkGeneralEnableX2.Checked = false;
                chkGeneralEnableX2.Enabled = true;

                chkGeneralCustomFilter.Visible = true;
                grpGenAutoMute.Visible = true;

                grpGeneralCalibration.Enabled = true;
                grpGenCalRXImage.Enabled = true;
                chkCalExpert.Enabled = true;

                grpHPSDRFreqCalDbg.Visible = false;
                chkCWKeyerMonoCable.Visible = true;

                if (!tcSetup.TabPages.Contains(tpExtCtrl))
                {
                    Common.TabControlInsert(tcSetup, tpExtCtrl, 5);
                }
            }
        }

        public void UpdateDisplayMeter()
        {
            //txtMeterOffset.Text = console.PreampOffset.ToString();
            //txtDisplayOffset.Text = console.RX1DisplayCalOffset.ToString();
            MeterOffset = console.MultiMeterCalOffset;
            DisplayOffset = console.RX1DisplayCalOffset;
        }

        public void AddHPSDRPages()
        {

            /*if (tcGeneral.TabPages.Contains(tpRX2))
            {
                tcGeneral.TabPages.Remove(tpRX2);
                tcGeneral.SelectedIndex = 0;
            }*/

            /*  if (tcAudio.TabPages.Contains(tpVAC2))
               {
                   tcAudio.TabPages.Remove(tpVAC2);
                   tcAudio.SelectedIndex = 0;
               } */

              if (tcGeneral.TabPages.Contains(tpInfo))
              {
                  tcGeneral.TabPages.Remove(tpInfo);
                  tcGeneral.SelectedIndex = 0;
              } 

            if (!tcGeneral.TabPages.Contains(tpHPSDR))
            {
                Common.TabControlInsert(tcGeneral, tpHPSDR, 1);
            }
            else
            {
                if (tcGeneral.TabPages.IndexOf(tpHPSDR) != 1)
                {
                    tcGeneral.TabPages.Remove(tpHPSDR);
                    Common.TabControlInsert(tcGeneral, tpHPSDR, 1);
                }
            }

            if (!tcGeneral.TabPages.Contains(tpPennyCtrl))
            {
                Common.TabControlInsert(tcGeneral, tpPennyCtrl, 5);
            }
            else
            {
                if (tcGeneral.TabPages.IndexOf(tpPennyCtrl) != 5)
                {
                    tcGeneral.TabPages.Remove(tpPennyCtrl);
                    Common.TabControlInsert(tcGeneral, tpPennyCtrl, 5);
                }
            }


            if (!tcGeneral.TabPages.Contains(tpAlexControl))
            {
                Common.TabControlInsert(tcGeneral, tpAlexControl, 6);
            }
            else
            {
                if (tcGeneral.TabPages.IndexOf(tpAlexControl) != 6)
                {
                    tcGeneral.TabPages.Remove(tpAlexControl);
                    Common.TabControlInsert(tcGeneral, tpAlexControl, 6);
                }
            }

            if (!tcGeneral.TabPages.Contains(tpApolloControl))
            {
                Common.TabControlInsert(tcGeneral, tpApolloControl, 7);
            }
            else
            {
                if (tcGeneral.TabPages.IndexOf(tpApolloControl) != 7)
                {
                    tcGeneral.TabPages.Remove(tpApolloControl);
                    Common.TabControlInsert(tcGeneral, tpApolloControl, 7);
                }
            }

            // now make sure enablements are correct 
            if (!chkAlexPresent.Checked)
            {
                chkAlexAntCtrl.Enabled = false;
                SetAlexAntEnabled(false);
            }
            if (!(chkPennyPresent.Checked || chkPennyLane.Checked))
            {
                chkPennyExtCtrl.Enabled = false;
                grpPennyExtCtrl.Enabled = false;
            }
            console.MaxFreq = (double)udMaxFreq.Value;
        }

        public void RemoveHPSDRPages()
        {
            if (tcGeneral.TabPages.Contains(tpHPSDR))
            {
                tcGeneral.TabPages.Remove(tpHPSDR);
                tcGeneral.SelectedIndex = 0;
            }
            if (tcGeneral.TabPages.Contains(tpPennyCtrl))
            {
                tcGeneral.TabPages.Remove(tpPennyCtrl);
                tcGeneral.SelectedIndex = 0;
            }
            if (tcGeneral.TabPages.Contains(tpAlexControl))
            {
                tcGeneral.TabPages.Remove(tpAlexControl);
                tcGeneral.SelectedIndex = 0;
            }
            /*  if (!tcGeneral.TabPages.Contains(tpRX2))
              {
                  Common.TabControlInsert(tcGeneral, tpRX2, 4);
              }
              else
              {
                  if (tcGeneral.TabPages.IndexOf(tpRX2) != 4)
                  {
                      tcGeneral.TabPages.Remove(tpRX2);
                      Common.TabControlInsert(tcGeneral, tpRX2, 4);
                  }
              }*/
            console.MaxFreq = 65.0;
        }


        private void udSoftRockCenterFreq_ValueChanged(object sender, System.EventArgs e)
        {
            console.SoftRockCenterFreq = (double)udSoftRockCenterFreq.Value;
        }

        private void comboGeneralLPTAddr_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboGeneralLPTAddr.Text == "" || console.CurrentModel != Model.SDR1000)
                return;
            console.Hdw.LPTAddr = Convert.ToUInt16(comboGeneralLPTAddr.Text, 16);
        }

        private void comboGeneralLPTAddr_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (console.CurrentModel != Model.SDR1000) return;
            if (comboGeneralLPTAddr.Text == "") return;
            if (e.KeyData != Keys.Enter) return;
            if (comboGeneralLPTAddr.Text.Length > 4)
            {
                MessageBox.Show("Invalid Parallel Port Address (" + comboGeneralLPTAddr.Text + ")");
                comboGeneralLPTAddr.Text = "378";
                return;
            }

            if (comboGeneralLPTAddr.Text.Any(c => !Char.IsDigit(c) &&
                                                  Char.ToLower(c) < 'a' &&
                                                  Char.ToLower(c) > 'f'))
            {
                MessageBox.Show("Invalid Parallel Port Address (" + comboGeneralLPTAddr.Text + ")");
                comboGeneralLPTAddr.Text = "378";
                return;
            }

            console.Hdw.LPTAddr = Convert.ToUInt16(comboGeneralLPTAddr.Text, 16);
        }

        private void comboGeneralLPTAddr_LostFocus(object sender, System.EventArgs e)
        {
            comboGeneralLPTAddr_KeyDown(sender, new KeyEventArgs(Keys.Enter));
        }

        private void chkGeneralRXOnly_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkGeneralRXOnly.Focused &&
                comboAudioSoundCard.Text == "Unsupported Card" &&
                !chkGeneralRXOnly.Checked &&
                radGenModelSDR1000.Checked)
            {
                DialogResult dr = MessageBox.Show(
                    "Unchecking Receive Only while in Unsupported Card mode may \n" +
                    "cause damage to your SDR-1000 hardware.  Are you sure you want \n" +
                    "to enable transmit?",
                    "Warning: Enable Transmit?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    chkGeneralRXOnly.Checked = true;
                    return;
                }
            }
            console.RXOnly = chkGeneralRXOnly.Checked;
            tpTransmit.Enabled = !chkGeneralRXOnly.Checked;
            tpPowerAmplifier.Enabled = !chkGeneralRXOnly.Checked;
            grpTestTXIMD.Enabled = !chkGeneralRXOnly.Checked;
        }

        private void chkGeneralUSBPresent_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                console.USBPresent = chkGeneralUSBPresent.Checked;
                if (chkGeneralUSBPresent.Checked)
                {
                    if (!USB.Init(true, chkGeneralPAPresent.Checked))
                        chkGeneralUSBPresent.Checked = false;
                    else USB.Console = console;
                }
                else
                    USB.Exit();

                if (console.PowerOn)
                {
                    console.PowerOn = false;
                    Thread.Sleep(100);
                    console.PowerOn = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("A required DLL was not found (Sdr1kUsb.dll).  Please download the\n" +
                    "installer from the FlexRadio private download page and try again.",
                    "Error: Missing DLL",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                chkGeneralUSBPresent.Checked = false;
            }
        }

        private void chkGeneralPAPresent_CheckedChanged(object sender, System.EventArgs e)
        {
            console.PAPresent = false;
            chkGeneralATUPresent.Visible = chkGeneralPAPresent.Checked;
            grpPAGainByBand.Visible = true;
            rtxtPACalReq.Visible = chkGeneralPAPresent.Checked;

            if (!chkGeneralPAPresent.Checked)
                chkGeneralATUPresent.Checked = false;
            else if (console.PowerOn)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
                console.PowerOn = true;
            }

            if (chkGeneralUSBPresent.Checked)
            {
                chkGeneralUSBPresent.Checked = false;
                chkGeneralUSBPresent.Checked = true;
            }
        }

        private void chkGeneralATUPresent_CheckedChanged(object sender, System.EventArgs e)
        {
            console.ATUPresent = chkGeneralATUPresent.Checked;
        }

        private void chkXVTRPresent_CheckedChanged(object sender, System.EventArgs e)
        {
            console.XVTRPresent = chkGeneralXVTRPresent.Checked;
            comboGeneralXVTR.Visible = chkGeneralXVTRPresent.Checked;
            if (chkGeneralXVTRPresent.Checked)
            {
                if (comboGeneralXVTR.SelectedIndex == (int)XVTRTRMode.POSITIVE)
                    comboGeneralXVTR_SelectedIndexChanged(this, EventArgs.Empty);
                else
                    comboGeneralXVTR.SelectedIndex = (int)XVTRTRMode.POSITIVE;
            }
        }

        private void chkGeneralSpurRed_CheckedChanged(object sender, System.EventArgs e)
        {
            console.SpurReduction = chkGeneralSpurRed.Checked;
        }

        private void udDDSCorrection_ValueChanged(object sender, System.EventArgs e)
        {
            console.DDSClockCorrection = (double)(udDDSCorrection.Value / 1000000);
        }

        private void udDDSPLLMult_ValueChanged(object sender, System.EventArgs e)
        {
            console.Hdw.PLLMult = (int)udDDSPLLMult.Value;
        }

        private void udDDSIFFreq_ValueChanged(object sender, System.EventArgs e)
        {
            console.IFFreq = (double)udDDSIFFreq.Value * 1e-6;
        }

        private void btnGeneralCalFreqStart_Click(object sender, System.EventArgs e)
        {
            btnGeneralCalFreqStart.Enabled = false;
            Thread t = new Thread(new ThreadStart(CalibrateFreq));
            t.Name = "Freq Calibration Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.AboveNormal;
            t.Start();
        }

        private void btnGeneralCalLevelStart_Click(object sender, System.EventArgs e)
        {
            btnGeneralCalLevelStart.Enabled = false;
            progress = new Progress("Calibrate RX Level");

            Thread t = new Thread(new ThreadStart(CalibrateLevel));
            t.Name = "Level Calibration Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.AboveNormal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void btnCalLevel_Click(object sender, System.EventArgs e)
        {
            btnCalLevel.Enabled = false;
            progress = new Progress("Calibrate RX2 Level");

            Thread t = new Thread(new ThreadStart(CalibrateRX2Level));
            t.Name = "Level Calibration Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void btnGeneralCalImageStart_Click(object sender, System.EventArgs e)
        {
            btnGeneralCalImageStart.Enabled = false;
            progress = new Progress("Calibrate RX Image Rejection");

            Thread t = new Thread(new ThreadStart(CalibrateRXImage));
            t.Name = "RX Image Calibration Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.AboveNormal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void CalibrateFreq()
        {
            bool done = console.CalibrateFreq((float)udGeneralCalFreq1.Value);
            if (done) MessageBox.Show("Frequency Calibration complete.");
            btnGeneralCalFreqStart.Enabled = true;
        }

        private void CalibrateLevel()
        {
            bool done = console.CalibrateLevel(
                (float)udGeneralCalLevel.Value,
                (float)udGeneralCalFreq2.Value,
                progress,
                false);
            if (done) MessageBox.Show("Level Calibration complete.");
            btnGeneralCalLevelStart.Enabled = true;
            UpdateDisplayMeter();

        }

        private void CalibrateRX2Level()
        {
            bool done = console.CalibrateRX2Level(
                (float)udGeneralCalRX2Level.Value,
                (float)udGeneralCalRX2Freq2.Value,
                progress,
                false);
            if (done) MessageBox.Show("Level Calibration complete.");
            btnCalLevel.Enabled = true;
            UpdateDisplayMeter();

        }


        private void CalibrateRXImage()
        {
            bool done = console.CalibrateRXImage((float)udGeneralCalFreq3.Value, progress, false);
            if (done) MessageBox.Show("RX Image Rejection Calibration complete.");
            btnGeneralCalImageStart.Enabled = true;
        }

        private void chkGeneralDisablePTT_CheckedChanged(object sender, System.EventArgs e)
        {
            console.DisablePTT = chkGeneralDisablePTT.Checked;
        }

        private void comboGeneralXVTR_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (comboGeneralXVTR.SelectedIndex)
            {
                case (int)XVTRTRMode.NEGATIVE:
                    if (comboGeneralXVTR.Focused)
                    {
                        MessageBox.Show("The default TR Mode for the DEMI144-28FRS sold by FlexRadio Systems is\n" +
                            "Postive TR Logic.  Please use caution when using other TR modes.", "Warning");
                    }
                    break;
                case (int)XVTRTRMode.POSITIVE:
                case (int)XVTRTRMode.NONE:
                    break;
            }

            console.CurrentXVTRTRMode = (XVTRTRMode)comboGeneralXVTR.SelectedIndex;
        }

        private void chkGeneralSoftwareGainCorr_CheckedChanged(object sender, System.EventArgs e)
        {
            console.NoHardwareOffset = chkGeneralSoftwareGainCorr.Checked;
        }

        private void chkGeneralEnableX2_CheckedChanged(object sender, System.EventArgs e)
        {
            console.X2Enabled = chkGeneralEnableX2.Checked;
            udGeneralX2Delay.Enabled = chkGeneralEnableX2.Checked;
        }

        private void udGeneralX2Delay_ValueChanged(object sender, System.EventArgs e)
        {
            console.X2Delay = (int)udGeneralX2Delay.Value;
        }

        private void comboGeneralProcessPriority_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Process p = Process.GetCurrentProcess();

            if (comboGeneralProcessPriority.Text == "Real Time" &&
                comboGeneralProcessPriority.Focused)
            {
                DialogResult dr = MessageBox.Show(
                    "Setting the Process Priority to Realtime can cause the system to become unresponsive.\n" +
                    "This setting is not recommended.\n" +
                    "Are you sure you want to change to Realtime?",
                    "Warning: Realtime Not Recommended",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    switch (p.PriorityClass)
                    {
                        case ProcessPriorityClass.Idle:
                            comboGeneralProcessPriority.Text = "Idle";
                            break;
                        case ProcessPriorityClass.BelowNormal:
                            comboGeneralProcessPriority.Text = "Below Normal";
                            break;
                        case ProcessPriorityClass.AboveNormal:
                            comboGeneralProcessPriority.Text = "Above Normal";
                            break;
                        case ProcessPriorityClass.High:
                            comboGeneralProcessPriority.Text = "Highest";
                            break;
                        default:
                            comboGeneralProcessPriority.Text = "Normal";
                            break;
                    }
                    return;
                }
            }

            switch (comboGeneralProcessPriority.Text)
            {
                case "Idle":
                    p.PriorityClass = ProcessPriorityClass.Idle;
                    break;
                case "Below Normal":
                    p.PriorityClass = ProcessPriorityClass.BelowNormal;
                    break;
                case "Normal":
                    p.PriorityClass = ProcessPriorityClass.Normal;
                    break;
                case "Above Normal":
                    p.PriorityClass = ProcessPriorityClass.AboveNormal;
                    break;
                case "High":
                    p.PriorityClass = ProcessPriorityClass.High;
                    break;
                case "Real Time":
                    p.PriorityClass = ProcessPriorityClass.RealTime;
                    break;
            }
        }

        private void chkGeneralCustomFilter_CheckedChanged(object sender, System.EventArgs e)
        {
            console.EnableLPF0 = chkGeneralCustomFilter.Checked;
        }

        private void chkGeneralUpdateRelease_CheckedChanged(object sender, System.EventArgs e)
        {
            console.NotifyOnRelease = chkGeneralUpdateRelease.Checked;
        }

        private void chkGeneralUpdateBeta_CheckedChanged(object sender, System.EventArgs e)
        {
            console.NotifyOnBeta = chkGeneralUpdateBeta.Checked;
        }

        private void chkGenAutoMute_CheckedChanged(object sender, System.EventArgs e)
        {
            console.AutoMute = chkGenAutoMute.Checked;
        }

        private void chkOptQuickQSY_CheckedChanged(object sender, System.EventArgs e)
        {
            console.QuickQSY = chkOptQuickQSY.Checked;
        }

        private void chkOptAlwaysOnTop_CheckedChanged(object sender, System.EventArgs e)
        {
            console.AlwaysOnTop = chkOptAlwaysOnTop.Checked;
        }

        private void udOptClickTuneOffsetDIGL_ValueChanged(object sender, System.EventArgs e)
        {
            console.DIGLClickTuneOffset = (int)udOptClickTuneOffsetDIGL.Value;
        }

        private void udOptClickTuneOffsetDIGU_ValueChanged(object sender, System.EventArgs e)
        {
            console.DIGUClickTuneOffset = (int)udOptClickTuneOffsetDIGU.Value;
        }

        private void udOptMaxFilterWidth_ValueChanged(object sender, System.EventArgs e)
        {
            console.MaxFilterWidth = (int)udOptMaxFilterWidth.Value;
        }

        private void comboOptFilterWidthMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (comboOptFilterWidthMode.Text)
            {
                case "Linear":
                    console.CurrentFilterWidthMode = FilterWidthMode.Linear;
                    break;
                case "Log":
                    console.CurrentFilterWidthMode = FilterWidthMode.Log;
                    break;
                case "Log10":
                    console.CurrentFilterWidthMode = FilterWidthMode.Log10;
                    break;
            }
        }

        private void udOptMaxFilterShift_ValueChanged(object sender, System.EventArgs e)
        {
            console.MaxFilterShift = (int)udOptMaxFilterShift.Value;
        }

        private void chkOptFilterSaveChanges_CheckedChanged(object sender, System.EventArgs e)
        {
            console.SaveFilterChanges = chkOptFilterSaveChanges.Checked;
        }

        private void chkOptEnableKBShortcuts_CheckedChanged(object sender, System.EventArgs e)
        {
            console.EnableKBShortcuts = chkOptEnableKBShortcuts.Checked;
            chkOptQuickQSY.Enabled = chkOptEnableKBShortcuts.Checked;
        }

        private void udFilterDefaultLowCut_ValueChanged(object sender, System.EventArgs e)
        {
            console.DefaultLowCut = (int)udFilterDefaultLowCut.Value;
        }

        #endregion

        #region Audio Tab Event Handlers
        // ======================================================
        // Audio Tab Event Handlers
        // ======================================================

        private void comboAudioDriver1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioDriver1.SelectedIndex < 0) return;

            int old_host = Audio.Host1;
            int new_host = ((PADeviceInfo)comboAudioDriver1.SelectedItem).Index;
            bool power = console.PowerOn;

            if (power && old_host != new_host)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }
            console.AudioDriverIndex1 = new_host;
            Audio.Host1 = new_host;
            GetDevices1();
            if (comboAudioInput1.Items.Count != 0)
                comboAudioInput1.SelectedIndex = 0;
            if (comboAudioOutput1.Items.Count != 0)
                comboAudioOutput1.SelectedIndex = 0;
            if (power && old_host != new_host) console.PowerOn = true;

            if (!chkAudioLatencyManual1.Checked)
            {
                if (comboAudioDriver1.Text == "MME" || comboAudioDriver1.Text == "DirectSound")
                    Audio.Latency1 = 200;
                else Audio.Latency1 = 0;
            }
        }

        private void comboAudioInput1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioInput1.SelectedIndex < 0) return;

            int old_input = Audio.Input1;
            int new_input = ((PADeviceInfo)comboAudioInput1.SelectedItem).Index;
            bool power = console.PowerOn;

            if (power && old_input != new_input)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.AudioInputIndex1 = new_input;
            Audio.Input1 = new_input;
            if (comboAudioInput1.SelectedIndex == 0 &&
                comboAudioDriver1.SelectedIndex < 2)
            {
                comboAudioMixer1.SelectedIndex = 0;
            }
            else
            {
                foreach (string s in from string s in comboAudioMixer1.Items where s.StartsWith(comboAudioInput1.Text.Substring(0, 5)) select s)
                {
                    comboAudioMixer1.Text = s;
                }
                comboAudioMixer1.Text = comboAudioInput1.Text;
            }

            if (power && old_input != new_input) console.PowerOn = true;
        }

        private void comboAudioOutput1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioOutput1.SelectedIndex < 0) return;

            int old_output = Audio.Output1;
            int new_output = ((PADeviceInfo)comboAudioOutput1.SelectedItem).Index;
            bool power = console.PowerOn;

            if (power && new_output != old_output)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.AudioOutputIndex1 = new_output;
            Audio.Output1 = new_output;

            if (power && new_output != old_output) console.PowerOn = true;
        }

        private void comboAudioMixer1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioMixer1.SelectedIndex < 0) return;
            UpdateMixerControls1();
            console.MixerID1 = comboAudioMixer1.SelectedIndex;
        }

        private void comboAudioReceive1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioReceive1.SelectedIndex < 0) return;
            console.MixerRXMuxID1 = comboAudioReceive1.SelectedIndex;
            if (!initializing && console.PowerOn)
                Mixer.SetMux(comboAudioMixer1.SelectedIndex, comboAudioReceive1.SelectedIndex);
        }

        private void comboAudioTransmit1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioTransmit1.SelectedIndex < 0) return;
            console.MixerTXMuxID1 = comboAudioTransmit1.SelectedIndex;
        }

        private void chkAudioEnableVAC_CheckedChanged(object sender, System.EventArgs e)
        {
            bool val = chkAudioEnableVAC.Checked;
            bool old_val = console.VACEnabled;

            if (val)
            {
                if (comboAudioDriver2.SelectedIndex < 0 &&
                    comboAudioDriver2.Items.Count > 0)
                    comboAudioDriver2.SelectedIndex = 0;
            }

            bool power = console.PowerOn;
            if (power && val != old_val)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.VACEnabled = val;
            if (power && val != old_val) console.PowerOn = true;
        }

        private void chkVAC2Enable_CheckedChanged(object sender, System.EventArgs e)
        {
            bool val = chkVAC2Enable.Checked;
            bool old_val = console.VAC2Enabled;

            if (val)
            {
                if (comboAudioDriver3.SelectedIndex < 0 &&
                    comboAudioDriver3.Items.Count > 0)
                    comboAudioDriver3.SelectedIndex = 0;
            }

            bool power = console.PowerOn;
            if (power && val != old_val)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.VAC2Enabled = val;
            if (power && val != old_val) console.PowerOn = true;
        }

        private void comboAudioChannels1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioChannels1.SelectedIndex < 0) return;

            int old_chan = Audio.NumChannels;
            int new_chan = Int32.Parse(comboAudioChannels1.Text);
            bool power = console.PowerOn;

            if (power && chkAudioEnableVAC.Checked && old_chan != new_chan)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.NumChannels = new_chan;
            Audio.NumChannels = new_chan;

            //RadioDSP.SetThreadNumber((uint)new_chan/2);
            if (power && chkAudioEnableVAC.Checked && old_chan != new_chan)
                console.PowerOn = true;
        }

        private void comboAudioDriver2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioDriver2.SelectedIndex < 0) return;

            int old_driver = Audio.Host2;
            int new_driver = ((PADeviceInfo)comboAudioDriver2.SelectedItem).Index;
            bool power = console.PowerOn;

            if (power && chkAudioEnableVAC.Checked && old_driver != new_driver)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.AudioDriverIndex2 = new_driver;
            Audio.Host2 = new_driver;
            GetDevices2();
            if (comboAudioInput2.Items.Count != 0)
                comboAudioInput2.SelectedIndex = 0;
            if (comboAudioOutput2.Items.Count != 0)
                comboAudioOutput2.SelectedIndex = 0;

            if (power && chkAudioEnableVAC.Checked && old_driver != new_driver)
                console.PowerOn = true;
        }

        private void comboAudioDriver3_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioDriver3.SelectedIndex < 0) return;

            int old_driver = Audio.Host3;
            int new_driver = ((PADeviceInfo)comboAudioDriver3.SelectedItem).Index;
            bool power = console.PowerOn;

            if (power && chkVAC2Enable.Checked && old_driver != new_driver)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.AudioDriverIndex3 = new_driver;
            Audio.Host3 = new_driver;
            GetDevices3();
            if (comboAudioInput3.Items.Count != 0)
                comboAudioInput3.SelectedIndex = 0;
            if (comboAudioOutput3.Items.Count != 0)
                comboAudioOutput3.SelectedIndex = 0;

            if (power && chkVAC2Enable.Checked && old_driver != new_driver)
                console.PowerOn = true;
        }

        private void comboAudioInput2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioInput2.SelectedIndex < 0) return;

            int old_input = Audio.Input2;
            int new_input = ((PADeviceInfo)comboAudioInput2.SelectedItem).Index;
            bool power = console.PowerOn;

            if (power && chkAudioEnableVAC.Checked && old_input != new_input)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.AudioInputIndex2 = new_input;
            Audio.Input2 = new_input;

            if (power && chkAudioEnableVAC.Checked && old_input != new_input)
                console.PowerOn = true;
        }

        private void comboAudioInput3_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioInput3.SelectedIndex < 0) return;

            int old_input = Audio.Input3;
            int new_input = ((PADeviceInfo)comboAudioInput3.SelectedItem).Index;
            bool power = console.PowerOn;

            if (power && chkVAC2Enable.Checked && old_input != new_input)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.AudioInputIndex3 = new_input;
            Audio.Input3 = new_input;

            if (power && chkVAC2Enable.Checked && old_input != new_input)
                console.PowerOn = true;
        }

        private void comboAudioOutput2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioOutput2.SelectedIndex < 0) return;

            int old_output = Audio.Output2;
            int new_output = ((PADeviceInfo)comboAudioOutput2.SelectedItem).Index;
            bool power = console.PowerOn;
            if (power && chkAudioEnableVAC.Checked && old_output != new_output)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.AudioOutputIndex2 = new_output;
            Audio.Output2 = new_output;

            if (power && chkAudioEnableVAC.Checked && old_output != new_output)
                console.PowerOn = true;
        }

        private void comboAudioOutput3_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioOutput3.SelectedIndex < 0) return;

            int old_output = Audio.Output3;
            int new_output = ((PADeviceInfo)comboAudioOutput3.SelectedItem).Index;
            bool power = console.PowerOn;
            if (power && chkVAC2Enable.Checked && old_output != new_output)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.AudioOutputIndex3 = new_output;
            Audio.Output3 = new_output;

            if (power && chkVAC2Enable.Checked && old_output != new_output)
                console.PowerOn = true;
        }

        public void forceAudioSampleRate1(String rate)
        {
            comboAudioSampleRate1.Text = rate;
        }

        private void comboAudioSampleRate1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioSampleRate1.SelectedIndex < 0) return;

            int old_rate = console.SampleRate1;
            int new_rate = Int32.Parse(comboAudioSampleRate1.Text);

            if (console.CurrentModel == Model.FLEX3000 && new_rate == 192000)
            {
                comboAudioSampleRate1.Text = "96000";
                new_rate = 96000;
            }

            bool power = console.PowerOn;

            if (power && new_rate != old_rate)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.SampleRate1 = new_rate;

            Display.DrawBackground();
            console.SoftRockCenterFreq = console.SoftRockCenterFreq; // warning -- this appears to do nothing - not true, these are
            // properties and the assignment is needed due to side effects!   
            // We need the soft rock  code to recalc  its tuning limits -- 
            // setting the center freq does this as a side effect

            if (!initializing)
            {
                RadioDSP.SyncStatic();

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        RadioDSPRX dsp_rx = console.radio.GetDSPRX(i, j);
                        dsp_rx.Update = false;
                        dsp_rx.Force = true;
                        dsp_rx.Update = true;
                        dsp_rx.Force = false;
                    }
                }

                for (int i = 0; i < 1; i++)
                {
                    RadioDSPTX dsp_tx = console.radio.GetDSPTX(i);
                    dsp_tx.Update = false;
                    dsp_tx.Force = true;
                    dsp_tx.Update = true;
                    dsp_tx.Force = false;
                }
            }

            if (power && new_rate != old_rate)
            {
                if (console.CurrentModel == Model.FLEX5000)
                {
                    console.PowerOn = true;
                    Thread.Sleep(5000);
                    console.PowerOn = false;
                    console.PowerOn = true;
                }
                else console.PowerOn = true;
            }
        }

        private void comboAudioSampleRate2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioSampleRate2.SelectedIndex < 0) return;

            int old_rate = console.SampleRate2;
            int new_rate = Int32.Parse(comboAudioSampleRate2.Text);
            bool poweron = console.PowerOn;

            if (poweron && chkAudioEnableVAC.Checked && new_rate != old_rate)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.SampleRate2 = new_rate;
            console.VACSampleRate = comboAudioSampleRate2.Text;

            if (poweron && chkAudioEnableVAC.Checked && new_rate != old_rate)
                console.PowerOn = true;
        }

        private void comboAudioSampleRate3_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioSampleRate3.SelectedIndex < 0) return;

            int old_rate = console.SampleRate3;
            int new_rate = Int32.Parse(comboAudioSampleRate3.Text);
            bool poweron = console.PowerOn;

            if (poweron && chkVAC2Enable.Checked && new_rate != old_rate)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.SampleRate3 = new_rate;
            console.VAC2SampleRate = comboAudioSampleRate3.Text;

            if (poweron && chkVAC2Enable.Checked && new_rate != old_rate)
                console.PowerOn = true;
        }

        private void comboAudioBuffer1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioBuffer1.SelectedIndex < 0) return;

            int old_size = console.BlockSize1;
            int new_size = Int32.Parse(comboAudioBuffer1.Text);
            bool power = console.PowerOn;

            CWKeyer.AudioLatency = Math.Max(10.0, new_size / (double)console.SampleRate1 * 1e3);
  
            if (power && old_size != new_size)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.BlockSize1 = new_size;

            if (power && old_size != new_size) console.PowerOn = true;
        }

        private void comboAudioBuffer2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioBuffer2.SelectedIndex < 0) return;

            int old_size = console.BlockSize2;
            int new_size = Int32.Parse(comboAudioBuffer2.Text);
            bool power = console.PowerOn;

            if (power && chkAudioEnableVAC.Checked && old_size != new_size)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.BlockSize2 = new_size;

            if (power && chkAudioEnableVAC.Checked && old_size != new_size)
                console.PowerOn = true;
        }

        private void comboAudioBuffer3_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioBuffer3.SelectedIndex < 0) return;

            int old_size = console.BlockSize3;
            int new_size = Int32.Parse(comboAudioBuffer3.Text);
            bool power = console.PowerOn;

            if (power && chkVAC2Enable.Checked && old_size != new_size)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.BlockSize3 = new_size;

            if (power && chkVAC2Enable.Checked && old_size != new_size)
                console.PowerOn = true;
        }

        private void udAudioLatency1_ValueChanged(object sender, System.EventArgs e)
        {
            bool power = console.PowerOn;
            if (power)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            Audio.Latency1 = (int)udAudioLatency1.Value;

            if (power) console.PowerOn = true;
        }

        private void udAudioLatency2_ValueChanged(object sender, System.EventArgs e)
        {
            bool power = console.PowerOn;
            if (power && chkAudioEnableVAC.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            Audio.Latency2 = (int)udAudioLatency2.Value;

            if (power && chkAudioEnableVAC.Checked)
                console.PowerOn = true;
        }

        private void udVAC2Latency_ValueChanged(object sender, System.EventArgs e)
        {
            bool power = console.PowerOn;
            if (power && chkVAC2Enable.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            Audio.Latency3 = (int)udVAC2Latency.Value;

            if (power && chkVAC2Enable.Checked)
                console.PowerOn = true;
        }

        private void udAudioVoltage1_ValueChanged(object sender, System.EventArgs e)
        {
            if (udAudioVoltage1.Focused &&
                comboAudioSoundCard.SelectedIndex > 0 &&
                current_sound_card != SoundCard.UNSUPPORTED_CARD)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to change the Max RMS Voltage for this \n" +
                    "supported sound card?  The largest measured difference in supported cards \n" +
                    "was 40mV.  Note that we will only allow a 100mV difference from our measured default.",
                    "Change Voltage?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    udAudioVoltage1.Value = (decimal)console.AudioVolts1;
                    return;
                }
            }
            /*double def_volt = 0.0;
            switch(current_sound_card)
            {
                case SoundCard.SANTA_CRUZ:
                    def_volt = 1.27;
                    break;
                case SoundCard.AUDIGY:
                case SoundCard.AUDIGY_2:
                case SoundCard.AUDIGY_2_ZS:
                    def_volt = 2.23;
                    break;
                case SoundCard.EXTIGY:
                    def_volt = 1.96;
                    break;
                case SoundCard.MP3_PLUS:
                    def_volt = 0.98;
                    break;
                case SoundCard.DELTA_44:
                    def_volt = 0.98;
                    break;
                case SoundCard.FIREBOX:
                    def_volt = 6.39;
                    break;
            }

            if(current_sound_card != SoundCard.UNSUPPORTED_CARD)
            {
                if(Math.Abs(def_volt - (double)udAudioVoltage1.Value) > 0.1)
                {
                    udAudioVoltage1.Value = (decimal)def_volt;
                    return;
                }
            }*/
            console.AudioVolts1 = (double)udAudioVoltage1.Value;
        }

        private void chkAudio2Stereo_CheckedChanged(object sender, System.EventArgs e)
        {
            bool power = console.PowerOn;
            if (power && chkAudioEnableVAC.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.VACSoundCardStereo = chkAudio2Stereo.Checked;
            console.VACStereo = chkAudio2Stereo.Checked;
            chkVACCombine.Enabled = chkAudio2Stereo.Checked;

            if (power && chkAudioEnableVAC.Checked)
                console.PowerOn = true;
        }

        private void chkAudioStereo3_CheckedChanged(object sender, System.EventArgs e)
        {
            bool power = console.PowerOn;
            if (power && chkVAC2Enable.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            console.VAC2SoundCardStereo = chkAudio2Stereo.Checked;
            console.VAC2Stereo = chkAudioStereo3.Checked;
            chkVAC2Combine.Enabled = chkAudioStereo3.Checked;

            if (power && chkVAC2Enable.Checked)
                console.PowerOn = true;
        }

        private void udAudioVACGainRX_ValueChanged(object sender, System.EventArgs e)
        {
            Audio.VACRXScale = Math.Pow(10.0, (int)udAudioVACGainRX.Value / 20.0);
            console.VACRXGain = (int)udAudioVACGainRX.Value;
        }

        private void udVAC2GainRX_ValueChanged(object sender, System.EventArgs e)
        {
            Audio.VAC2RXScale = Math.Pow(10.0, (int)udVAC2GainRX.Value / 20.0);
            console.VAC2RXGain = (int)udVAC2GainRX.Value;
        }

        private void udAudioVACGainTX_ValueChanged(object sender, System.EventArgs e)
        {
            Audio.VACPreamp = Math.Pow(10.0, (int)udAudioVACGainTX.Value / 20.0);
            console.VACTXGain = (int)udAudioVACGainTX.Value;
        }

        private void udVAC2GainTX_ValueChanged(object sender, System.EventArgs e)
        {
            Audio.VAC2TXScale = Math.Pow(10.0, (int)udVAC2GainTX.Value / 20.0);
            console.VAC2TXGain = (int)udVAC2GainTX.Value;
        }

        private void chkAudioVACAutoEnable_CheckedChanged(object sender, System.EventArgs e)
        {
            console.VACAutoEnable = chkAudioVACAutoEnable.Checked;
        }

        private void chkVAC2AutoEnable_CheckedChanged(object sender, System.EventArgs e)
        {
            console.VAC2AutoEnable = chkVAC2AutoEnable.Checked;
        }

        private void udAudioLineIn1_ValueChanged(object sender, System.EventArgs e)
        {
            Mixer.SetLineInRecordVolume(comboAudioMixer1.SelectedIndex, (int)udAudioLineIn1.Value);
        }

        private void udAudioMicGain1_ValueChanged(object sender, System.EventArgs e)
        {
            Mixer.SetMicRecordVolume(comboAudioMixer1.SelectedIndex, (int)udAudioMicGain1.Value);
        }

        private void chkAudioLatencyManual1_CheckedChanged(object sender, System.EventArgs e)
        {
            bool power = console.PowerOn;
            if (power)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            udAudioLatency1.Enabled = chkAudioLatencyManual1.Checked;

            if (!chkAudioLatencyManual1.Checked)
            {
                if (comboAudioDriver1.Text == "MME" || comboAudioDriver1.Text == "DirectSound")
                    Audio.Latency1 = 50;
                else Audio.Latency1 = 0;
            }

            if (power) console.PowerOn = true;
        }

        private void chkAudioLatencyManual2_CheckedChanged(object sender, System.EventArgs e)
        {
            bool power = console.PowerOn;
            if (power && chkAudioEnableVAC.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            udAudioLatency2.Enabled = chkAudioLatencyManual2.Checked;

            if (!chkAudioLatencyManual2.Checked)
                Audio.Latency2 = 120;

            if (power && chkAudioEnableVAC.Checked)
                console.PowerOn = true;
        }

        private void chkVAC2LatencyManual_CheckedChanged(object sender, System.EventArgs e)
        {
            bool power = console.PowerOn;
            if (power && chkVAC2Enable.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            udVAC2Latency.Enabled = chkVAC2LatencyManual.Checked;

            if (!chkVAC2LatencyManual.Checked)
                Audio.Latency3 = 120;

            if (power && chkVAC2Enable.Checked)
                console.PowerOn = true;
        }

        private void chkAudioMicBoost_CheckedChanged(object sender, System.EventArgs e)
        {
            console.MicBoost = chkAudioMicBoost.Checked;
        }

        private void chk20dbMicBoost_CheckedChanged(object sender, System.EventArgs e)
        {
            console.MicBoost = chk20dbMicBoost.Checked;
            //console.SetMicGain();
        }


        private void btnAudioVoltTest1_Click(object sender, System.EventArgs e)
        {
            sound_card = 1;

            DialogResult dr = MessageBox.Show(
                "Is the Line Out Cable unplugged?  Running this test with the plug in the \n" +
                "normal position plugged into the SDR-1000 could cause damage to the device.",
                "Warning: Cable unplugged from SDR-1000?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr == DialogResult.No) return;

            progress = new Progress("Calibrate Sound Card");
            if (console.PowerOn)
                progress.Show();

            Thread t = new Thread(new ThreadStart(CalibrateSoundCard));
            t.Name = "Sound Card Calibration Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.AboveNormal;
            t.Start();
        }

        private void CalibrateSoundCard()
        {
            bool done = console.CalibrateSoundCard(progress, sound_card);
            if (done) MessageBox.Show("Sound Card Calibration complete.");
        }

        private void FireBoxMixerFix()
        {
            try
            {
                Process p = Process.Start("c:\\Program Files\\PreSonus\\1394AudioDriver_FIREBox\\FireBox Mixer.exe");
                Thread.Sleep(2000);
                p.Kill();
            }
            catch (Exception)
            {

            }
        }

        private void comboAudioSoundCard_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAudioSoundCard.SelectedIndex < 0) return;
            bool on = console.PowerOn;
            if (on)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            SoundCard card = SoundCard.FIRST;
            switch (comboAudioSoundCard.Text)
            {
                case "M-Audio Delta 44 (PCI)":
                    card = SoundCard.DELTA_44;
                    break;
                case "PreSonus FireBox (FireWire)":
                    card = SoundCard.FIREBOX;
                    break;
                case "Edirol FA-66 (FireWire)":
                    card = SoundCard.EDIROL_FA_66;
                    break;
                case "SB Audigy (PCI)":
                    card = SoundCard.AUDIGY;
                    break;
                case "SB Audigy 2 (PCI)":
                    card = SoundCard.AUDIGY_2;
                    break;
                case "SB Audigy 2 ZS (PCI)":
                    card = SoundCard.AUDIGY_2_ZS;
                    break;
                case "Sound Blaster Extigy (USB)":
                    card = SoundCard.EXTIGY;
                    break;
                case "Sound Blaster MP3+ (USB)":
                    card = SoundCard.MP3_PLUS;
                    break;
                case "Turtle Beach Santa Cruz (PCI)":
                    card = SoundCard.SANTA_CRUZ;
                    break;
                case "HPSDR":
                    card = SoundCard.HPSDR;
                    break;
                case "Unsupported Card":
                    card = SoundCard.UNSUPPORTED_CARD;
                    break;
            }

            if (card == SoundCard.FIRST) return;

            console.CurrentSoundCard = card;
            current_sound_card = card;

            switch (card)
            {
                case SoundCard.SANTA_CRUZ:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = chkAudioExpert.Checked;
                    udAudioVoltage1.Value = 1.274M;
                    if (comboAudioSampleRate1.Items.Contains(96000))
                        comboAudioSampleRate1.Items.Remove(96000);
                    if (comboAudioSampleRate1.Items.Contains(192000))
                        comboAudioSampleRate1.Items.Remove(192000);
                    comboAudioSampleRate1.Text = "48000";
                    foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "ASIO")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name == "Wuschel's ASIO4ALL")
                        {
                            comboAudioInput1.Text = "Wuschel's ASIO4ALL";
                            comboAudioOutput1.Text = "Wuschel's ASIO4ALL";
                        }
                    }
                    if (comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        foreach (PADeviceInfo dev in comboAudioInput1.Items)
                        {
                            if (dev.Name == "ASIO4ALL v2")
                            {
                                comboAudioInput1.Text = "ASIO4ALL v2";
                                comboAudioOutput1.Text = "ASIO4ALL v2";
                            }
                        }
                    }

                    comboAudioMixer1.Text = "Santa Cruz(tm)";
                    comboAudioReceive1.Text = "Line In";

                    for (int i = 0; i < comboAudioTransmit1.Items.Count; i++)
                    {
                        if (((string)comboAudioTransmit1.Items[i]).StartsWith("Mi"))
                        {
                            comboAudioTransmit1.SelectedIndex = i;
                            break;
                        }
                    }

                    if (comboAudioMixer1.SelectedIndex < 0 ||
                        comboAudioMixer1.Text != "Santa Cruz(tm)")
                    {
                        MessageBox.Show(comboAudioSoundCard.Text + " not found.\n " +
                            "Please verify that this specific sound card is installed " +
                            "and functioning and try again.  \nIf your sound card is not " +
                            "a " + comboAudioSoundCard.Text + " and your card is not in the " +
                            "list, use the Unsupported Card selection.  \nFor more support, " +
                            "email support@flex-radio.com.",
                            comboAudioSoundCard.Text + " Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (!Mixer.InitSantaCruz(console.MixerID1))
                    {
                        MessageBox.Show("The " + comboAudioSoundCard.Text + " mixer initialization " +
                            "failed.  Please install the latest drivers from www.turtlebeach.com " +
                            " and try again.  For more support, email support@flex-radio.com.",
                            comboAudioSoundCard.Text + " Mixer Initialization Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (comboAudioInput1.Text != "ASIO4ALL v2" &&
                        comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        MessageBox.Show("ASIO4ALL driver not found.  Please visit " +
                            "www.asio4all.com, download and install the driver, " +
                            "and try again.  Alternatively, you can use the Unsupported " +
                            "Card selection and setup the sound interface manually.  For " +
                            "more support, email support@flex-radio.com.",
                            "ASIO4ALL Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else
                    {
                        udAudioLineIn1.Value = 20;
                        console.PowerEnabled = true;
                        grpAudioMicInGain1.Enabled = true;
                        grpAudioLineInGain1.Enabled = true;
                        comboAudioChannels1.Text = "2";
                        comboAudioChannels1.Enabled = false;
                        Audio.IN_RX1_L = 0;
                        Audio.IN_RX1_R = 1;
                        Audio.IN_TX_L = 0;
                        Audio.IN_TX_R = 1;
                    }
                    break;
                case SoundCard.AUDIGY:
                case SoundCard.AUDIGY_2:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = chkAudioExpert.Checked;
                    udAudioVoltage1.Value = 2.23M;
                    if (comboAudioSampleRate1.Items.Contains(96000))
                        comboAudioSampleRate1.Items.Remove(96000);
                    if (comboAudioSampleRate1.Items.Contains(192000))
                        comboAudioSampleRate1.Items.Remove(192000);
                    comboAudioSampleRate1.Text = "48000";
                    foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "ASIO")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name == "Wuschel's ASIO4ALL")
                        {
                            comboAudioInput1.Text = "Wuschel's ASIO4ALL";
                            comboAudioOutput1.Text = "Wuschel's ASIO4ALL";
                        }
                    }
                    if (comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        foreach (PADeviceInfo dev in comboAudioInput1.Items)
                        {
                            if (dev.Name == "ASIO4ALL v2")
                            {
                                comboAudioInput1.Text = "ASIO4ALL v2";
                                comboAudioOutput1.Text = "ASIO4ALL v2";
                            }
                        }
                    }

                    for (int i = 0; i < comboAudioMixer1.Items.Count; i++)
                    {
                        if (((string)comboAudioMixer1.Items[i]).StartsWith("SB Audigy"))
                        {
                            comboAudioMixer1.SelectedIndex = i;
                            break;
                        }
                    }

                    for (int i = 0; i < comboAudioReceive1.Items.Count; i++)
                    {
                        if (((string)comboAudioReceive1.Items[i]).StartsWith("Analog"))
                        {
                            comboAudioReceive1.SelectedIndex = i;
                            break;
                        }
                    }

                    if (comboAudioReceive1.SelectedIndex < 0 ||
                        !comboAudioReceive1.Text.StartsWith("Analog"))
                    {
                        for (int i = 0; i < comboAudioReceive1.Items.Count; i++)
                        {
                            if (((string)comboAudioReceive1.Items[i]).StartsWith("Mix ana"))
                            {
                                comboAudioReceive1.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    for (int i = 0; i < comboAudioTransmit1.Items.Count; i++)
                    {
                        if (((string)comboAudioTransmit1.Items[i]).StartsWith("Mi"))
                        {
                            comboAudioTransmit1.SelectedIndex = i;
                            break;
                        }
                    }

                    if (comboAudioMixer1.SelectedIndex < 0 ||
                        !comboAudioMixer1.Text.StartsWith("SB Audigy"))
                    {
                        MessageBox.Show(comboAudioSoundCard.Text + " not found.\n " +
                            "Please verify that this specific sound card is installed " +
                            "and functioning and try again.  \nIf your sound card is not " +
                            "a " + comboAudioSoundCard.Text + " and your card is not in the " +
                            "list, use the Unsupported Card selection.  \nFor more support, " +
                            "email support@flex-radio.com.",
                            comboAudioSoundCard.Text + " Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (!Mixer.InitAudigy2(console.MixerID1))
                    {
                        MessageBox.Show("The " + comboAudioSoundCard.Text + " mixer initialization " +
                            "failed.  Please install the latest drivers from www.creativelabs.com " +
                            " and try again.  For more support, email support@flex-radio.com.",
                            comboAudioSoundCard.Text + " Mixer Initialization Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (comboAudioInput1.Text != "ASIO4ALL v2" &&
                        comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        MessageBox.Show("ASIO4ALL driver not found.  Please visit " +
                            "www.asio4all.com, download and install the driver, " +
                            "and try again.  Alternatively, you can use the Unsupported " +
                            "Card selection and setup the sound interface manually.  For " +
                            "more support, email support@flex-radio.com.",
                            "ASIO4ALL Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else
                    {
                        udAudioLineIn1.Value = 1;
                        console.PowerEnabled = true;
                        grpAudioMicInGain1.Enabled = true;
                        grpAudioLineInGain1.Enabled = true;
                        comboAudioChannels1.Text = "2";
                        comboAudioChannels1.Enabled = false;
                        Audio.IN_RX1_L = 0;
                        Audio.IN_RX1_R = 1;
                        Audio.IN_TX_L = 0;
                        Audio.IN_TX_R = 1;
                    }
                    break;
                case SoundCard.AUDIGY_2_ZS:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = chkAudioExpert.Checked;
                    udAudioVoltage1.Value = 2.23M;
                    if (comboAudioSampleRate1.Items.Contains(96000))
                        comboAudioSampleRate1.Items.Remove(96000);
                    if (comboAudioSampleRate1.Items.Contains(192000))
                        comboAudioSampleRate1.Items.Remove(192000);
                    comboAudioSampleRate1.Text = "48000";
                    foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "ASIO")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name == "Wuschel's ASIO4ALL")
                        {
                            comboAudioInput1.Text = "Wuschel's ASIO4ALL";
                            comboAudioOutput1.Text = "Wuschel's ASIO4ALL";
                        }
                    }
                    if (comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        foreach (PADeviceInfo dev in comboAudioInput1.Items)
                        {
                            if (dev.Name == "ASIO4ALL v2")
                            {
                                comboAudioInput1.Text = "ASIO4ALL v2";
                                comboAudioOutput1.Text = "ASIO4ALL v2";
                            }
                        }
                    }

                    for (int i = 0; i < comboAudioMixer1.Items.Count; i++)
                    {
                        if (((string)comboAudioMixer1.Items[i]).StartsWith("SB Audigy"))
                        {
                            comboAudioMixer1.SelectedIndex = i;
                            break;
                        }
                    }

                    for (int i = 0; i < comboAudioReceive1.Items.Count; i++)
                    {
                        if (((string)comboAudioReceive1.Items[i]).StartsWith("Analog"))
                        {
                            comboAudioReceive1.SelectedIndex = i;
                            break;
                        }
                    }

                    if (comboAudioReceive1.SelectedIndex < 0 ||
                        !comboAudioReceive1.Text.StartsWith("Analog"))
                    {
                        for (int i = 0; i < comboAudioReceive1.Items.Count; i++)
                        {
                            if (((string)comboAudioReceive1.Items[i]).StartsWith("Mix ana"))
                            {
                                comboAudioReceive1.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    for (int i = 0; i < comboAudioTransmit1.Items.Count; i++)
                    {
                        if (((string)comboAudioTransmit1.Items[i]).StartsWith("Mi"))
                        {
                            comboAudioTransmit1.SelectedIndex = i;
                            break;
                        }
                    }

                    if (comboAudioMixer1.SelectedIndex < 0 ||
                        !comboAudioMixer1.Text.StartsWith("SB Audigy"))
                    {
                        MessageBox.Show(comboAudioSoundCard.Text + " not found.\n " +
                            "Please verify that this specific sound card is installed " +
                            "and functioning and try again.  \nIf your sound card is not " +
                            "a " + comboAudioSoundCard.Text + " and your card is not in the " +
                            "list, use the Unsupported Card selection.  \nFor more support, " +
                            "email support@flex-radio.com.",
                            comboAudioSoundCard.Text + " Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (!Mixer.InitAudigy2ZS(console.MixerID1))
                    {
                        MessageBox.Show("The " + comboAudioSoundCard.Text + " mixer initialization " +
                            "failed.  Please install the latest drivers from www.creativelabs.com " +
                            " and try again.  For more support, email support@flex-radio.com.",
                            comboAudioSoundCard.Text + " Mixer Initialization Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (comboAudioInput1.Text != "ASIO4ALL v2" &&
                        comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        MessageBox.Show("ASIO4ALL driver not found.  Please visit " +
                            "www.asio4all.com, download and install the driver, " +
                            "and try again.  Alternatively, you can use the Unsupported " +
                            "Card selection and setup the sound interface manually.  For " +
                            "more support, email support@flex-radio.com.",
                            "ASIO4ALL Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else
                    {
                        udAudioLineIn1.Value = 1;
                        console.PowerEnabled = true;
                        grpAudioMicInGain1.Enabled = true;
                        grpAudioLineInGain1.Enabled = true;
                        comboAudioChannels1.Text = "2";
                        comboAudioChannels1.Enabled = false;
                        Audio.IN_RX1_L = 0;
                        Audio.IN_RX1_R = 1;
                        Audio.IN_TX_L = 0;
                        Audio.IN_TX_R = 1;
                    }
                    break;
                case SoundCard.EXTIGY:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = chkAudioExpert.Checked;
                    udAudioVoltage1.Value = 1.96M;
                    if (!comboAudioSampleRate1.Items.Contains(96000))
                        comboAudioSampleRate1.Items.Add(96000);
                    if (comboAudioSampleRate1.Items.Contains(192000))
                        comboAudioSampleRate1.Items.Remove(192000);
                    comboAudioSampleRate1.Text = "48000";
                    foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "ASIO")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name == "Wuschel's ASIO4ALL")
                        {
                            comboAudioInput1.Text = "Wuschel's ASIO4ALL";
                            comboAudioOutput1.Text = "Wuschel's ASIO4ALL";
                        }
                    }
                    if (comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        foreach (PADeviceInfo dev in comboAudioInput1.Items)
                        {
                            if (dev.Name == "ASIO4ALL v2")
                            {
                                comboAudioInput1.Text = "ASIO4ALL v2";
                                comboAudioOutput1.Text = "ASIO4ALL v2";
                            }
                        }
                    }

                    for (int i = 0; i < comboAudioMixer1.Items.Count; i++)
                    {
                        if (((string)comboAudioMixer1.Items[i]).StartsWith("Creative SB Extigy"))
                        {
                            comboAudioMixer1.SelectedIndex = i;
                            break;
                        }
                    }

                    comboAudioReceive1.Text = "Line In";
                    comboAudioTransmit1.Text = "Microphone";

                    if (comboAudioMixer1.SelectedIndex < 0 ||
                        comboAudioMixer1.Text != "Creative SB Extigy")
                    {
                        MessageBox.Show(comboAudioSoundCard.Text + " not found.\n " +
                            "Please verify that this specific sound card is installed " +
                            "and functioning and try again.  \nIf your sound card is not " +
                            "a " + comboAudioSoundCard.Text + " and your card is not in the " +
                            "list, use the Unsupported Card selection.  \nFor more support, " +
                            "email support@flex-radio.com.",
                            comboAudioSoundCard.Text + " Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (!Mixer.InitExtigy(console.MixerID1))
                    {
                        MessageBox.Show("The " + comboAudioSoundCard.Text + " mixer initialization " +
                            "failed.  Please install the latest drivers from www.creativelabs.com " +
                            " and try again.  For more support, email support@flex-radio.com.",
                            comboAudioSoundCard.Text + " Mixer Initialization Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (comboAudioInput1.Text != "ASIO4ALL v2" &&
                        comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        MessageBox.Show("ASIO4ALL driver not found.  Please visit " +
                            "www.asio4all.com, download and install the driver, " +
                            "and try again.  Alternatively, you can use the Unsupported " +
                            "Card selection and setup the sound interface manually.  For " +
                            "more support, email support@flex-radio.com.",
                            "ASIO4ALL Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else
                    {
                        udAudioLineIn1.Value = 20;
                        console.PowerEnabled = true;
                        grpAudioMicInGain1.Enabled = true;
                        grpAudioLineInGain1.Enabled = true;
                        comboAudioChannels1.Text = "2";
                        comboAudioChannels1.Enabled = false;
                        Audio.IN_RX1_L = 0;
                        Audio.IN_RX1_R = 1;
                        Audio.IN_TX_L = 0;
                        Audio.IN_TX_R = 1;
                    }
                    break;
                case SoundCard.MP3_PLUS:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = chkAudioExpert.Checked;
                    udAudioVoltage1.Value = 0.982M;
                    if (comboAudioSampleRate1.Items.Contains(96000))
                        comboAudioSampleRate1.Items.Remove(96000);
                    if (comboAudioSampleRate1.Items.Contains(192000))
                        comboAudioSampleRate1.Items.Remove(192000);
                    comboAudioSampleRate1.Text = "48000";
                    foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "ASIO")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    for (int i = 0; i < comboAudioMixer1.Items.Count; i++)
                    {
                        if (((string)comboAudioMixer1.Items[i]).StartsWith("Sound Blaster"))
                        {
                            comboAudioMixer1.SelectedIndex = i;
                            break;
                        }
                    }

                    if (comboAudioMixer1.SelectedIndex < 0 ||
                        (string)comboAudioMixer1.SelectedItem != "Sound Blaster")
                    {
                        for (int i = 0; i < comboAudioMixer1.Items.Count; i++)
                        {
                            if (((string)comboAudioMixer1.Items[i]).StartsWith("USB Audio"))
                            {
                                comboAudioMixer1.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name == "Wuschel's ASIO4ALL")
                        {
                            comboAudioInput1.Text = "Wuschel's ASIO4ALL";
                            comboAudioOutput1.Text = "Wuschel's ASIO4ALL";
                        }
                    }
                    if (comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        foreach (PADeviceInfo dev in comboAudioInput1.Items)
                        {
                            if (dev.Name == "ASIO4ALL v2")
                            {
                                comboAudioInput1.Text = "ASIO4ALL v2";
                                comboAudioOutput1.Text = "ASIO4ALL v2";
                            }
                        }
                    }

                    comboAudioReceive1.Text = "Line In";

                    for (int i = 0; i < comboAudioTransmit1.Items.Count; i++)
                    {
                        if (((string)comboAudioTransmit1.Items[i]).StartsWith("Mi"))
                        {
                            comboAudioTransmit1.SelectedIndex = i;
                            break;
                        }
                    }

                    if (comboAudioMixer1.SelectedIndex < 0 ||
                        (comboAudioMixer1.Text != "Sound Blaster" &&
                        comboAudioMixer1.Text != "USB Audio"))
                    {
                        MessageBox.Show(comboAudioSoundCard.Text + " not found.\n " +
                            "Please verify that this specific sound card is installed " +
                            "and functioning and try again.  \nIf your sound card is not " +
                            "a " + comboAudioSoundCard.Text + " and your card is not in the " +
                            "list, use the Unsupported Card selection.  \nFor more support, " +
                            "email support@flex-radio.com.",
                            comboAudioSoundCard.Text + " Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (!Mixer.InitMP3Plus(console.MixerID1))
                    {
                        MessageBox.Show("The " + comboAudioSoundCard.Text + " mixer initialization " +
                            "failed.  Please install the latest drivers from www.creativelabs.com " +
                            " and try again.  For more support, email support@flex-radio.com.",
                            comboAudioSoundCard.Text + " Mixer Initialization Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else if (comboAudioInput1.Text != "ASIO4ALL v2" &&
                        comboAudioInput1.Text != "Wuschel's ASIO4ALL")
                    {
                        MessageBox.Show("ASIO4ALL driver not found.  Please visit " +
                            "www.asio4all.com, download and install the driver, " +
                            "and try again.  Alternatively, you can use the Unsupported " +
                            "Card selection and setup the sound interface manually.  For " +
                            "more support, email support@flex-radio.com.",
                            "ASIO4ALL Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else
                    {
                        udAudioLineIn1.Value = 6;
                        console.PowerEnabled = true;
                        grpAudioMicInGain1.Enabled = true;
                        grpAudioLineInGain1.Enabled = true;
                        comboAudioChannels1.Text = "2";
                        comboAudioChannels1.Enabled = false;
                        Audio.IN_RX1_L = 0;
                        Audio.IN_RX1_R = 1;
                        Audio.IN_TX_L = 0;
                        Audio.IN_TX_R = 1;
                    }
                    break;
                case SoundCard.DELTA_44:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = chkAudioExpert.Checked;
                    udAudioVoltage1.Value = 0.98M;
                    if (!comboAudioSampleRate1.Items.Contains(96000))
                        comboAudioSampleRate1.Items.Add(96000);
                    if (comboAudioSampleRate1.Items.Contains(192000))
                        comboAudioSampleRate1.Items.Remove(192000);
                    if (comboAudioSoundCard.Focused || comboAudioSampleRate1.SelectedIndex < 0)
                        comboAudioSampleRate1.Text = "48000";
                    foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "ASIO")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name == "M-Audio Delta ASIO")
                        {
                            comboAudioInput1.Text = "M-Audio Delta ASIO";
                            comboAudioOutput1.Text = "M-Audio Delta ASIO";
                        }
                    }

                    comboAudioMixer1.Text = "None";

                    if (comboAudioInput1.Text != "M-Audio Delta ASIO")
                    {
                        MessageBox.Show("M-Audio Delta ASIO driver not found.  Please visit " +
                            "www.m-audio.com, download and install the latest driver, " +
                            "and try again.  For more support, email support@flex-radio.com.",
                            "Delta 44 Driver Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else
                    {
                        InitDelta44();
                        chkAudioEnableVAC.Enabled = true;
                        grpAudioMicInGain1.Enabled = false;
                        grpAudioLineInGain1.Enabled = false;
                        console.PowerEnabled = true;
                        comboAudioChannels1.Text = "4";
                        comboAudioChannels1.Enabled = false;
                        Audio.IN_RX1_L = 0;
                        Audio.IN_RX1_R = 1;
                        Audio.IN_TX_L = 2;
                        Audio.IN_TX_R = 3;
                    }
                    break;

                case SoundCard.HPSDR:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = true;
                    udAudioVoltage1.Value = 0.80M;
                    if (!comboAudioSampleRate1.Items.Contains(96000))
                        comboAudioSampleRate1.Items.Add(96000);
                    if (!comboAudioSampleRate1.Items.Contains(192000))
                        comboAudioSampleRate1.Items.Add(192000);
                    if (comboAudioSoundCard.Focused || comboAudioSampleRate1.SelectedIndex < 0)
                        comboAudioSampleRate1.Text = "48000";

                    foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "HPSDR (USB/UDP)")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name == "HPSDR (USB/UDP)")
                        {
                            comboAudioInput1.Text = "PCM";
                            comboAudioOutput1.Text = "PWM";
                        }
                    }

                    comboAudioMixer1.Text = "None";

                    chkAudioEnableVAC.Enabled = true;
                    grpAudioMicInGain1.Enabled = false;
                    grpAudioLineInGain1.Enabled = false;
                    console.PowerEnabled = true;
                    comboAudioChannels1.Text = "8";
                    comboAudioChannels1.Enabled = false;
                    Audio.IN_RX1_L = 0;
                    Audio.IN_RX1_R = 1;
                    Audio.IN_RX2_L = 2;
                    Audio.IN_RX2_R = 3;
                    Audio.IN_TX_L = 4;
                    Audio.IN_TX_R = 5;
                    break; // Janus Ozy 


                case SoundCard.FIREBOX:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = chkAudioExpert.Checked;
                    udAudioVoltage1.Value = 6.39M;
                    if (!comboAudioSampleRate1.Items.Contains(96000))
                        comboAudioSampleRate1.Items.Add(96000);
                    if (comboAudioSampleRate1.Items.Contains(192000))
                        comboAudioSampleRate1.Items.Remove(192000);
                    if (comboAudioSoundCard.Focused || comboAudioSampleRate1.SelectedIndex < 0)
                        comboAudioSampleRate1.Text = "48000";
                    foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "ASIO")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name.IndexOf("FireBox") >= 0)
                        {
                            comboAudioInput1.Text = dev.Name;
                            comboAudioOutput1.Text = dev.Name;
                        }
                    }

                    comboAudioMixer1.Text = "None";

                    if (comboAudioInput1.Text.IndexOf("FireBox") < 0)
                    {
                        MessageBox.Show("PreSonus FireBox ASIO driver not found.  Please visit " +
                            "www.presonus.com, download and install the latest driver, " +
                            "and try again.  For more support, email support@flex-radio.com.",
                            "PreSonus FireBox Driver Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else
                    {
                        chkAudioEnableVAC.Enabled = true;
                        grpAudioMicInGain1.Enabled = false;
                        grpAudioLineInGain1.Enabled = false;
                        console.PowerEnabled = true;
                        comboAudioChannels1.Text = "4";
                        comboAudioChannels1.Enabled = false;
                        Audio.IN_RX1_L = 2;
                        Audio.IN_RX1_R = 3;
                        Audio.IN_TX_L = 0;
                        Audio.IN_TX_R = 1;
                        Thread t = new Thread(new ThreadStart(FireBoxMixerFix));
                        t.Name = "FireBoxMixerFix";
                        t.Priority = ThreadPriority.Normal;
                        t.IsBackground = true;
                        t.Start();
                    }
                    break;
                case SoundCard.EDIROL_FA_66:
                    grpAudioDetails1.Enabled = false;
                    grpAudioVolts1.Visible = chkAudioExpert.Checked;
                    udAudioVoltage1.Value = 2.27M;
                    if (!comboAudioSampleRate1.Items.Contains(96000))
                        comboAudioSampleRate1.Items.Add(96000);
                    if (!comboAudioSampleRate1.Items.Contains(192000))
                        comboAudioSampleRate1.Items.Add(192000);
                    if (comboAudioSoundCard.Focused || comboAudioSampleRate1.SelectedIndex < 0)
                        comboAudioSampleRate1.Text = "192000";
                    foreach (PADeviceInfo p in comboAudioDriver1.Items)
                    {
                        if (p.Name == "ASIO")
                        {
                            comboAudioDriver1.SelectedItem = p;
                            break;
                        }
                    }

                    foreach (PADeviceInfo dev in comboAudioInput1.Items)
                    {
                        if (dev.Name == "EDIROL FA-66")
                        {
                            comboAudioInput1.Text = "EDIROL FA-66";
                            comboAudioOutput1.Text = "EDIROL FA-66";
                        }
                    }

                    comboAudioMixer1.Text = "None";

                    if (comboAudioInput1.Text != "EDIROL FA-66")
                    {
                        MessageBox.Show("Edirol FA-66 ASIO driver not found.  Please visit " +
                            "www.rolandus.com, download and install the latest driver, " +
                            "and try again.  For more support, email support@flex-radio.com.",
                            "Edirol FA-66 Driver Not Found",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        console.PowerEnabled = false;
                    }
                    else
                    {
                        chkAudioEnableVAC.Enabled = true;
                        grpAudioMicInGain1.Enabled = false;
                        grpAudioLineInGain1.Enabled = false;
                        console.PowerEnabled = true;
                        comboAudioChannels1.Text = "4";
                        comboAudioChannels1.Enabled = false;
                        Audio.IN_RX1_L = 2;
                        Audio.IN_RX1_R = 3;
                        Audio.IN_TX_L = 0;
                        Audio.IN_TX_R = 1;
                    }
                    break;
                case SoundCard.UNSUPPORTED_CARD:
                    if (comboAudioSoundCard.Focused)
                    {
                        MessageBox.Show("Proper operation of the SDR-1000 depends on the use of a sound card that is\n" +
                            "officially recommended by FlexRadio Systems.  Refer to the Specifications page on\n" +
                            "www.flex-radio.com to determine which sound cards are currently recommended.  Use only\n" +
                            "the specific model numbers stated on the website because other models within the same\n" +
                            "family may not work properly with the radio.  Officially supported sound cards may be\n" +
                            "updated on the website without notice.  If you have any question about the sound card\n" +
                            "you would like to use with the radio, please email support@flex-radio.com or call us at\n" +
                            "512-250-8595.\n\n" +

                            "NO WARRANTY IS IMPLIED WHEN THE SDR-1000 IS USED WITH ANY SOUND CARD OTHER\n" +
                            "THAN THOSE CURRENTLY RECOMMENDED AS STATED ON THE FLEXRADIO SYSTEMS WEBSITE.\n" +
                            "UNSUPPORTED SOUND CARDS MAY OR MAY NOT WORK WITH THE SDR-1000.  USE OF\n" +
                            "UNSUPPORTED SOUND CARDS IS AT THE CUSTOMERS OWN RISK.",
                            "Warning: Unsupported Card",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    grpAudioVolts1.Visible = true;
                    if (comboAudioSoundCard.Focused)
                        chkGeneralRXOnly.Checked = true;
                    if (!comboAudioSampleRate1.Items.Contains(96000))
                        comboAudioSampleRate1.Items.Add(96000);
                    if (!comboAudioSampleRate1.Items.Contains(192000))
                        comboAudioSampleRate1.Items.Add(192000);
                    if (comboAudioSoundCard.Focused || comboAudioSampleRate1.SelectedIndex < 0)
                        comboAudioSampleRate1.Text = "48000";
                    grpAudioDetails1.Enabled = true;
                    grpAudioMicInGain1.Enabled = true;
                    grpAudioLineInGain1.Enabled = true;
                    console.PowerEnabled = true;
                    comboAudioChannels1.Text = "2";
                    comboAudioChannels1.Enabled = true;
                    Audio.IN_RX1_L = 0;
                    Audio.IN_RX1_R = 1;
                    Audio.IN_TX_L = 0;
                    Audio.IN_TX_R = 1;
                    break;
            }

            console.PWR = console.PWR;
            console.AF = console.AF;
            if (on) console.PowerOn = true;
        }

        #endregion

        #region Display Tab Event Handlers
        // ======================================================
        // Display Tab Event Handlers
        // ======================================================

        private void udDisplayGridMax_LostFocus(object sender, System.EventArgs e)
        {
            Display.SpectrumGridMax = (int)udDisplayGridMax.Value;
        }

        private void udTXGridMax_LostFocus(object sender, System.EventArgs e)
        {
            Display.TXSpectrumGridMax = (int)udTXGridMax.Value;
        }

        private void udDisplayGridMax_Click(object sender, System.EventArgs e)
        {
            udDisplayGridMax_LostFocus(sender, e);
        }

        private void udDisplayGridMax_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            udDisplayGridMax_LostFocus(sender, new System.EventArgs());
        }

        private void udDisplayFPS_ValueChanged(object sender, System.EventArgs e)
        {
            console.DisplayFPS = (int)udDisplayFPS.Value;
            udDisplayAVGTime_ValueChanged(this, EventArgs.Empty);
        }

        private void udDisplayGridMax_ValueChanged(object sender, System.EventArgs e)
        {
            if (udDisplayGridMax.Value <= udDisplayGridMin.Value)
                udDisplayGridMax.Value = udDisplayGridMin.Value + 10;
            Display.SpectrumGridMax = (int)udDisplayGridMax.Value;
        }

        private void udDisplayGridMin_ValueChanged(object sender, System.EventArgs e)
        {
            if (udDisplayGridMin.Value >= udDisplayGridMax.Value)
                udDisplayGridMin.Value = udDisplayGridMax.Value - 10;
            Display.SpectrumGridMin = (int)udDisplayGridMin.Value;
        }

        private void udDisplayGridStep_ValueChanged(object sender, System.EventArgs e)
        {
            Display.SpectrumGridStep = (int)udDisplayGridStep.Value;
        }

        private void comboDisplayLabelAlign_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (comboDisplayLabelAlign.Text)
            {
                case "Left":
                    Display.DisplayLabelAlign = DisplayLabelAlignment.LEFT;
                    break;
                case "Cntr":
                    Display.DisplayLabelAlign = DisplayLabelAlignment.CENTER;
                    break;
                case "Right":
                    Display.DisplayLabelAlign = DisplayLabelAlignment.RIGHT;
                    break;
                case "Auto":
                    Display.DisplayLabelAlign = DisplayLabelAlignment.AUTO;
                    break;
                case "Off":
                    Display.DisplayLabelAlign = DisplayLabelAlignment.OFF;
                    break;
                default:
                    Display.DisplayLabelAlign = DisplayLabelAlignment.LEFT;
                    break;
            }
        }

        private void udDisplayPhasePts_ValueChanged(object sender, System.EventArgs e)
        {
            Display.PhaseNumPts = (int)udDisplayPhasePts.Value;
        }

        private void udDisplayAVGTime_ValueChanged(object sender, System.EventArgs e)
        {
            double display_time = 1 / (double)udDisplayFPS.Value;
            int buffersToAvg = (int)((float)udDisplayAVGTime.Value * 0.001 / display_time);
            Display.DisplayAvgBlocks = (int)Math.Max(2, buffersToAvg);
        }

        private void udDisplayMeterDelay_ValueChanged(object sender, System.EventArgs e)
        {
            console.MeterDelay = (int)udDisplayMeterDelay.Value;
        }

        private void udDisplayPeakText_ValueChanged(object sender, System.EventArgs e)
        {
            console.PeakTextDelay = (int)udDisplayPeakText.Value;
        }

        private void udDisplayCPUMeter_ValueChanged(object sender, System.EventArgs e)
        {
            console.CPUMeterDelay = (int)udDisplayCPUMeter.Value;
        }

        private void clrbtnWaterfallLow_Changed(object sender, System.EventArgs e)
        {
            Display.WaterfallLowColor = clrbtnWaterfallLow.Color;
        }

        private void clrbtnWaterfallHigh_Changed(object sender, System.EventArgs e)
        {
            Display.WaterfallHighColor = clrbtnWaterfallHigh.Color;
        }

        private void clrbtnWaterfallMid_Changed(object sender, System.EventArgs e)
        {
            Display.WaterfallMidColor = clrbtnWaterfallMid.Color;
        }

        private void udDisplayWaterfallLowLevel_ValueChanged(object sender, System.EventArgs e)
        {
            Display.WaterfallLowThreshold = (float)udDisplayWaterfallLowLevel.Value;
        }

        private void udDisplayWaterfallHighLevel_ValueChanged(object sender, System.EventArgs e)
        {
            Display.WaterfallHighThreshold = (float)udDisplayWaterfallHighLevel.Value;
        }

        private void udDisplayMultiPeakHoldTime_ValueChanged(object sender, System.EventArgs e)
        {
            console.MultimeterPeakHoldTime = (int)udDisplayMultiPeakHoldTime.Value;
        }

        private void udDisplayMultiTextHoldTime_ValueChanged(object sender, System.EventArgs e)
        {
            console.MultimeterTextPeakTime = (int)udDisplayMultiTextHoldTime.Value;
        }

        private void chkSpectrumPolyphase_CheckedChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPRX(0, 0).SpectrumPolyphase = chkSpectrumPolyphase.Checked;
            console.radio.GetDSPRX(1, 0).SpectrumPolyphase = chkSpectrumPolyphase.Checked;
        }

        private void udDisplayScopeTime_ValueChanged(object sender, System.EventArgs e)
        {
            //console.ScopeTime = (int)udDisplayScopeTime.Value;
            int samples = (int)((double)udDisplayScopeTime.Value * console.SampleRate1 / 1000000.0);
            //Debug.WriteLine("sample: "+samples);
            Audio.ScopeSamplesPerPixel = samples;
        }

        private void udDisplayMeterAvg_ValueChanged(object sender, System.EventArgs e)
        {
            double block_time = (double)udDisplayMeterDelay.Value * 0.001;
            int blocksToAvg = (int)((float)udDisplayMeterAvg.Value * 0.001 / block_time);
            blocksToAvg = Math.Max(2, blocksToAvg);
            console.MultiMeterAvgBlocks = blocksToAvg;
        }

        private void comboDisplayDriver_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (comboDisplayDriver.Text)
            {
                case "GDI+":
                    console.CurrentDisplayEngine = DisplayEngine.GDI_PLUS;
                    break;
                /*case "DirectX":
                    console.CurrentDisplayEngine = DisplayEngine.DIRECT_X;
                    break;*/
            }
        }

        private void udTXGridMax_ValueChanged(object sender, System.EventArgs e)
        {
            if (udTXGridMax.Value <= udTXGridMin.Value)
                udTXGridMax.Value = udTXGridMin.Value + 10;
            Display.TXSpectrumGridMax = (int)udTXGridMax.Value;
        }

        private void udTXGridMin_ValueChanged(object sender, System.EventArgs e)
        {
            if (udTXGridMin.Value >= udTXGridMax.Value)
                udTXGridMin.Value = udTXGridMax.Value - 10;
            Display.TXSpectrumGridMin = (int)udTXGridMin.Value;
        }

        private void udTXGridStep_ValueChanged(object sender, System.EventArgs e)
        {
            Display.TXSpectrumGridStep = (int)udTXGridStep.Value;
        }

        private void comboTXLabelAlign_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (comboTXLabelAlign.Text)
            {
                case "Left":
                    Display.TXDisplayLabelAlign = DisplayLabelAlignment.LEFT;
                    break;
                case "Cntr":
                    Display.TXDisplayLabelAlign = DisplayLabelAlignment.CENTER;
                    break;
                case "Right":
                    Display.TXDisplayLabelAlign = DisplayLabelAlignment.RIGHT;
                    break;
                case "Auto":
                    Display.TXDisplayLabelAlign = DisplayLabelAlignment.AUTO;
                    break;
                case "Off":
                    Display.TXDisplayLabelAlign = DisplayLabelAlignment.OFF;
                    break;
                default:
                    Display.TXDisplayLabelAlign = DisplayLabelAlignment.LEFT;
                    break;
            }
        }

        #endregion

        #region DSP Tab Event Handlers
        // ======================================================
        // DSP Tab Event Handlers
        // ======================================================

        private void udLMSNR_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPRX(0, 0).SetNRVals(
                (int)udLMSNRtaps.Value,
                (int)udLMSNRdelay.Value,
                1e-6 * (double)udLMSNRgain.Value,
                1e-3 * (double)udLMSNRLeak.Value);
            console.radio.GetDSPRX(0, 1).SetNRVals(
                (int)udLMSNRtaps.Value,
                (int)udLMSNRdelay.Value,
                1e-6 * (double)udLMSNRgain.Value,
                1e-3 * (double)udLMSNRLeak.Value);
        }

        private void udLMSNR2_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPRX(1, 0).SetNRVals(
                (int)udLMSNR2taps.Value,
                (int)udLMSNR2delay.Value,
                1e-6 * (double)udLMSNR2gain.Value,
                1e-3 * (double)udLMSNR2Leak.Value);
            console.radio.GetDSPRX(1, 1).SetNRVals(
                (int)udLMSNR2taps.Value,
                (int)udLMSNR2delay.Value,
                1e-6 * (double)udLMSNR2gain.Value,
                1e-3 * (double)udLMSNR2Leak.Value);
        }

        private void udDSPNB_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPRX(0, 0).NBThreshold = 0.165 * (double)(udDSPNB.Value);
            console.radio.GetDSPRX(0, 1).NBThreshold = 0.165 * (double)(udDSPNB.Value);
            console.radio.GetDSPRX(1, 0).NBThreshold = 0.165 * (double)(udDSPNB.Value);
            console.radio.GetDSPRX(1, 1).NBThreshold = 0.165 * (double)(udDSPNB.Value);
        }

        private void udDSPNB2_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPRX(0, 0).SDROMThreshold = 0.165 * (double)(udDSPNB2.Value);
            console.radio.GetDSPRX(0, 1).SDROMThreshold = 0.165 * (double)(udDSPNB2.Value);
            console.radio.GetDSPRX(1, 0).SDROMThreshold = 0.165 * (double)(udDSPNB2.Value);
            console.radio.GetDSPRX(1, 1).SDROMThreshold = 0.165 * (double)(udDSPNB2.Value);
        }

        private void comboDSPWindow_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPRX(0, 0).CurrentWindow = (Window)comboDSPWindow.SelectedIndex;
            console.radio.GetDSPRX(0, 1).CurrentWindow = (Window)comboDSPWindow.SelectedIndex;
            console.radio.GetDSPRX(1, 0).CurrentWindow = (Window)comboDSPWindow.SelectedIndex;
            console.radio.GetDSPRX(1, 1).CurrentWindow = (Window)comboDSPWindow.SelectedIndex;
        }

        private void comboDSPPhoneRXBuf_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.DSPBufPhoneRX = int.Parse(comboDSPPhoneRXBuf.Text);
        }

        private void comboDSPPhoneTXBuf_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.DSPBufPhoneTX = int.Parse(comboDSPPhoneTXBuf.Text);
        }

        private void comboDSPCWRXBuf_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.DSPBufCWRX = int.Parse(comboDSPCWRXBuf.Text);
        }

        private void comboDSPCWTXBuf_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.DSPBufCWTX = int.Parse(comboDSPCWTXBuf.Text);
        }

        private void comboDSPDigRXBuf_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.DSPBufDigRX = int.Parse(comboDSPDigRXBuf.Text);
        }

        private void comboDSPDigTXBuf_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.DSPBufDigTX = int.Parse(comboDSPDigTXBuf.Text);
        }


        #region Image Reject

        private void udDSPImagePhaseTX_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                console.radio.GetDSPTX(0).TXCorrectIQPhase = (double)udDSPImagePhaseTX.Value;
            }
            catch (Exception)
            {
                MessageBox.Show("Error setting TX Image Phase (" + udDSPImagePhaseTX.Value + ")");
                udDSPImagePhaseTX.Value = 0;
            }
            if (tbDSPImagePhaseTX.Value != (int)udDSPImagePhaseTX.Value)
                tbDSPImagePhaseTX.Value = (int)udDSPImagePhaseTX.Value;

        }

        private void tbDSPImagePhaseTX_Scroll(object sender, System.EventArgs e)
        {
            udDSPImagePhaseTX.Value = tbDSPImagePhaseTX.Value;
        }

        private void udDSPImageGainTX_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                console.radio.GetDSPTX(0).TXCorrectIQGain = (double)udDSPImageGainTX.Value;
            }
            catch (Exception)
            {
                MessageBox.Show("Error setting TX Image Gain (" + udDSPImageGainTX.Value + ")");
                udDSPImageGainTX.Value = 0;
            }
            if (tbDSPImageGainTX.Value != (int)udDSPImageGainTX.Value)
                tbDSPImageGainTX.Value = (int)udDSPImageGainTX.Value;
        }

        private void tbDSPImageGainTX_Scroll(object sender, System.EventArgs e)
        {
            udDSPImageGainTX.Value = tbDSPImageGainTX.Value;
        }

        private void udLMSANF_ValueChanged(object sender, System.EventArgs e)   // (NR0V) modified
        {
            console.radio.GetDSPRX(0, 0).SetANFVals(
                (int)udLMSANFtaps.Value,
                (int)udLMSANFdelay.Value,
                1e-6 * (double)udLMSANFgain.Value,
                1e-3 * (double)udLMSANFLeak.Value);
            console.radio.GetDSPRX(0, 1).SetANFVals(
                (int)udLMSANFtaps.Value,
                (int)udLMSANFdelay.Value,
                1e-6 * (double)udLMSANFgain.Value,
                1e-3 * (double)udLMSANFLeak.Value);
        }

        private void udLMSANF2_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPRX(1, 0).SetANFVals(
                (int)udLMSANF2taps.Value,
                (int)udLMSANF2delay.Value,
                1e-6 * (double)udLMSANF2gain.Value,
                1e-3 * (double)udLMSANF2Leak.Value);
            console.radio.GetDSPRX(1, 1).SetANFVals(
                (int)udLMSANF2taps.Value,
                (int)udLMSANF2delay.Value,
                1e-6 * (double)udLMSANF2gain.Value,
                1e-3 * (double)udLMSANF2Leak.Value);
        }

        private void radANFPreAGC_CheckedChanged(object sender, EventArgs e)  // (NR0V) added
        {
            int position;
            if (radANFPreAGC.Checked)
                position = 0;
            else
                position = 1;
            DttSP.SetANFposition(0, 0, position);
            DttSP.SetANFposition(0, 1, position);
            DttSP.SetNRposition(0, 0, position);
            DttSP.SetNRposition(0, 1, position);
        }

        private void radANF2PreAGC_CheckedChanged(object sender, EventArgs e)  // (NR0V) added
        {
            int position;
            if (radANF2PreAGC.Checked)
                position = 0;
            else
                position = 1;
            DttSP.SetANFposition(2, 0, position);
            DttSP.SetANFposition(2, 1, position);
            DttSP.SetNRposition(2, 0, position);
            DttSP.SetNRposition(2, 1, position);
        }

        private bool old_cpdr = false;
        private void chkTXImagCal_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkboxTXImagCal.Checked)
            {
                old_cpdr = console.CPDR;
                console.CPDR = false;

                Audio.SineFreq1 = console.CWPitch;
                Audio.TXInputSignal = Audio.SignalSource.SINE;
                Audio.SourceScale = 1.0;
            }
            else
            {
                Audio.TXInputSignal = Audio.SignalSource.RADIO;
                old_cpdr = console.CPDR;
            }
        }

        #endregion

        #region Keyer

        private void udDSPCWPitch_ValueChanged(object sender, System.EventArgs e)
        {
            console.CWPitch = (int)udDSPCWPitch.Value;
        }

        private void chkCWKeyerIambic_CheckedChanged(object sender, System.EventArgs e)
        {
           // RadioDSP.KeyerIambic = chkCWKeyerIambic.Checked;
            CWKeyer.Iambic = chkCWKeyerIambic.Checked;
            console.CWIambic = chkCWKeyerIambic.Checked;
            switch (console.CurrentModel)
            {
                case Model.FLEX3000:
                case Model.FLEX5000:
                    if (console.fwc_init)
                        FWC.SetIambic(chkCWKeyerIambic.Checked);
                    break;
            }
        }

        private void udCWKeyerWeight_ValueChanged(object sender, System.EventArgs e)
        {
          //  RadioDSP.KeyerWeight = (int)udCWKeyerWeight.Value;
            CWKeyer.Weight = (int)udCWKeyerWeight.Value;
        }

        private void udCWKeyerRamp_ValueChanged(object sender, System.EventArgs e)
        {
           // RadioDSP.KeyerRamp = (int)udCWKeyerRamp.Value;
            CWSynth.RampTime = (int)udCWKeyerRamp.Value;
        }

        private void udCWKeyerSemiBreakInDelay_ValueChanged(object sender, System.EventArgs e)
        {
            CWKeyer.BreakInDelay = (double)udCWBreakInDelay.Value;
            console.BreakInDelay = (double)udCWBreakInDelay.Value;
        }

        private void chkDSPKeyerSemiBreakInEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            CWKeyer.BreakIn = chkCWBreakInEnabled.Checked;
            console.CWSemiBreakInEnabled = chkCWBreakInEnabled.Checked;
            console.BreakInEnabled = chkCWBreakInEnabled.Checked;
            udCWBreakInDelay.Enabled = chkCWBreakInEnabled.Checked;
        }

        private void chkDSPKeyerSidetone_CheckedChanged(object sender, System.EventArgs e)
        {
            console.CWSidetone = chkDSPKeyerSidetone.Checked;
        }

        private void chkCWKeyerRevPdl_CheckedChanged(object sender, System.EventArgs e)
        {
           // RadioDSP.KeyerReversePaddle = chkCWKeyerRevPdl.Checked;
            SDRSerialPort.ReversePaddles = chkCWKeyerRevPdl.Checked;
            console.ReversePaddles = chkCWKeyerRevPdl.Checked;
        }

        private void comboKeyerConnPrimary_SelectedIndexChanged(object sender, System.EventArgs e)
        {
          //  bool running = System.Convert.ToBoolean(DttSP.KeyerRunning());
          //  if (running) DttSP.StopKeyer();
           // Thread.Sleep(40);
           
          //  console.Keyer.PrimaryConnPort = comboKeyerConnPrimary.Text;
           // if (console.Keyer.PrimaryConnPort == "SDR" && comboKeyerConnPrimary.Text != "SDR")
             //   comboKeyerConnPrimary.Text = "SDR";
          //  if (running) DttSP.StartKeyer();
 
            if (!CWInput.SetPrimaryInput(comboKeyerConnPrimary.Text))
            {
                MessageBox.Show("Error using " + comboKeyerConnPrimary.Text + " for Keyer Primary Input.\n" +
                    "The port may already be in use by another application.",
                    "Error using " + comboKeyerConnPrimary.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                comboKeyerConnPrimary.Text = CWInput.PrimaryInput;
            }
        }

        private void comboKeyerConnSecondary_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (initializing) return;

            if (comboKeyerConnSecondary.Text == "CAT")
            {
                if (!chkCATEnable.Checked)
            {
                MessageBox.Show("CAT is not Enabled.  Please enable the CAT interface before selecting this option.",
                    "CAT not enabled",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
                    comboKeyerConnSecondary.Text = CWInput.SecondaryInput;
                    return;
                }
                else
                {
                    CWInput.SetSecondaryInput("None");
                    console.Siolisten.UseForKeyPTT = true;
                }
            }
            else
            {
                if (console.Siolisten != null)
                {
                    console.Siolisten.UseForKeyPTT = false;
                }
            }

            if (!CWInput.SetSecondaryInput(comboKeyerConnSecondary.Text))
            {
                MessageBox.Show("Error using " + comboKeyerConnSecondary.Text + " for Keyer Secondary Input.\n" +
                    "The port may already be in use by another application.",
                    "Error using " + comboKeyerConnSecondary.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                comboKeyerConnSecondary.Text = CWInput.SecondaryInput;
                return;
            }

            switch (comboKeyerConnSecondary.Text)
            {
                case "None":
                    lblKeyerConnPTTLine.Visible = false;
                    comboKeyerConnPTTLine.Visible = false;
                    lblKeyerConnKeyLine.Visible = false;
                    comboKeyerConnKeyLine.Visible = false;
                    break;
                case "CAT":
                    lblKeyerConnPTTLine.Visible = true;
                    comboKeyerConnPTTLine.Visible = true;
                    lblKeyerConnKeyLine.Visible = true;
                    comboKeyerConnKeyLine.Visible = true;

                    comboKeyerConnPTTLine_SelectedIndexChanged(this, EventArgs.Empty);
                    comboKeyerConnKeyLine_SelectedIndexChanged(this, EventArgs.Empty);
                    break;
                default: // COMx
                    lblKeyerConnPTTLine.Visible = true;
                    comboKeyerConnPTTLine.Visible = true;
                    lblKeyerConnKeyLine.Visible = true;
                    comboKeyerConnKeyLine.Visible = true;

                    comboKeyerConnPTTLine_SelectedIndexChanged(this, EventArgs.Empty);
                    comboKeyerConnKeyLine_SelectedIndexChanged(this, EventArgs.Empty);
                    break;
            }
        }

        private void comboKeyerConnKeyLine_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboKeyerConnKeyLine.SelectedIndex < 0) return;

            if (comboKeyerConnSecondary.Text == "CAT")
            {
                switch ((KeyerLine)comboKeyerConnKeyLine.SelectedIndex)
                {
                    case KeyerLine.None:
                        console.Siolisten.KeyOnDTR = false;
                        console.Siolisten.KeyOnRTS = false;
                        break;
                    case KeyerLine.DTR:
                        console.Siolisten.KeyOnDTR = true;
                        console.Siolisten.KeyOnRTS = false;
                        break;
                    case KeyerLine.RTS:
                        console.Siolisten.KeyOnDTR = false;
                        console.Siolisten.KeyOnRTS = true;
                        break;
                }
            }
            else CWInput.SecondaryKeyLine = (KeyerLine)comboKeyerConnKeyLine.SelectedIndex;
        }

        private void comboKeyerConnPTTLine_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboKeyerConnPTTLine.SelectedIndex < 0) return;

            if (comboKeyerConnSecondary.Text == "CAT")
            {
                switch ((KeyerLine)comboKeyerConnPTTLine.SelectedIndex)
                {
                    case KeyerLine.None:
                        console.Siolisten.PTTOnDTR = false;
                        console.Siolisten.PTTOnRTS = false;
                        break;
                    case KeyerLine.DTR:
                        console.Siolisten.PTTOnDTR = true;
                        console.Siolisten.PTTOnRTS = false;
                        break;
                    case KeyerLine.RTS:
                        console.Siolisten.PTTOnDTR = false;
                        console.Siolisten.PTTOnRTS = true;
                        break;
                }
            }
            else CWInput.SecondaryPTTLine = (KeyerLine)comboKeyerConnPTTLine.SelectedIndex;
        }

        #endregion

        #region AGC

        private void udDSPAGCFixedGaindB_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPRX(0, 0).RXFixedAGC = (double)udDSPAGCFixedGaindB.Value;
            console.radio.GetDSPRX(0, 1).RXFixedAGC = (double)udDSPAGCFixedGaindB.Value;

            if (console.RX1AGCMode == AGCMode.FIXD)
                console.RF = (int)udDSPAGCFixedGaindB.Value;
        }

        private void udDSPAGCMaxGaindB_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPRX(0, 0).RXAGCMaxGain = (double)udDSPAGCMaxGaindB.Value;
            console.radio.GetDSPRX(0, 1).RXAGCMaxGain = (double)udDSPAGCMaxGaindB.Value;

            if (console.RX1AGCMode != AGCMode.FIXD)
                console.RF = (int)udDSPAGCMaxGaindB.Value;
        }

        private void udDSPAGCAttack_ValueChanged(object sender, System.EventArgs e)
        {
            if (udDSPAGCAttack.Enabled)
            {
                console.radio.GetDSPRX(0, 0).RXAGCAttack = (int)udDSPAGCAttack.Value;
                console.radio.GetDSPRX(0, 1).RXAGCAttack = (int)udDSPAGCAttack.Value;
            }
        }

        private void udDSPAGCRX2MaxGaindB_ValueChanged(object sender, EventArgs e)
        {
            console.radio.GetDSPRX(1, 0).RXAGCMaxGain = (double)udDSPAGCRX2MaxGaindB.Value;
            console.radio.GetDSPRX(1, 1).RXAGCMaxGain = (double)udDSPAGCRX2MaxGaindB.Value;

            if (console.RX2AGCMode != AGCMode.FIXD)
                console.RX2RF = (int)udDSPAGCRX2MaxGaindB.Value;
        }

        private void udDSPAGCRX2FixedGaindB_ValueChanged(object sender, EventArgs e)
        {
            console.radio.GetDSPRX(1, 0).RXFixedAGC = (double)udDSPAGCRX2FixedGaindB.Value;
            console.radio.GetDSPRX(1, 1).RXFixedAGC = (double)udDSPAGCRX2FixedGaindB.Value;

            if (console.RX2AGCMode == AGCMode.FIXD)
                console.RX2RF = (int)udDSPAGCRX2FixedGaindB.Value;
        }

        private void udDSPAGCRX2Attack_ValueChanged(object sender, System.EventArgs e)
        {
            if (udDSPAGCRX2Attack.Enabled)
            {
                console.radio.GetDSPRX(1, 0).RXAGCAttack = (int)udDSPAGCRX2Attack.Value;
                console.radio.GetDSPRX(1, 1).RXAGCAttack = (int)udDSPAGCRX2Attack.Value;
            }
        }

        private void udDSPAGCDecay_ValueChanged(object sender, System.EventArgs e)
        {
            if (udDSPAGCDecay.Enabled)
            {
                console.radio.GetDSPRX(0, 0).RXAGCDecay = (int)udDSPAGCDecay.Value;
                console.radio.GetDSPRX(0, 1).RXAGCDecay = (int)udDSPAGCDecay.Value;
            }
        }

        private void udDSPAGCRX2Decay_ValueChanged(object sender, System.EventArgs e)
        {
            if (udDSPAGCRX2Decay.Enabled)
            {
                console.radio.GetDSPRX(1, 0).RXAGCDecay = (int)udDSPAGCRX2Decay.Value;
                console.radio.GetDSPRX(1, 1).RXAGCDecay = (int)udDSPAGCRX2Decay.Value;
            }
        }

        private void udDSPAGCSlope_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPRX(0, 0).RXAGCSlope = 10 * (int)(udDSPAGCSlope.Value);
            console.radio.GetDSPRX(0, 1).RXAGCSlope = 10 * (int)(udDSPAGCSlope.Value);
        }

        private void udDSPAGCRX2Slope_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPRX(1, 0).RXAGCSlope = 10 * (int)(udDSPAGCRX2Slope.Value);
            console.radio.GetDSPRX(1, 1).RXAGCSlope = 10 * (int)(udDSPAGCRX2Slope.Value);
        }

        private void udDSPAGCHangTime_ValueChanged(object sender, System.EventArgs e)
        {
            if (udDSPAGCHangTime.Enabled)
            {
                console.radio.GetDSPRX(0, 0).RXAGCHang = (int)udDSPAGCHangTime.Value;
                console.radio.GetDSPRX(0, 1).RXAGCHang = (int)udDSPAGCHangTime.Value;
            }
        }

        private void udDSPAGCRX2HangTime_ValueChanged(object sender, System.EventArgs e)
        {
            if (udDSPAGCRX2HangTime.Enabled)
            {
                console.radio.GetDSPRX(1, 0).RXAGCHang = (int)udDSPAGCRX2HangTime.Value;
                console.radio.GetDSPRX(1, 1).RXAGCHang = (int)udDSPAGCRX2HangTime.Value;
            }
        }

        private void tbDSPAGCHangThreshold_Scroll(object sender, System.EventArgs e)
        {
            console.radio.GetDSPRX(0, 0).RXAGCHangThreshold = (int)tbDSPAGCHangThreshold.Value;
            console.radio.GetDSPRX(0, 1).RXAGCHangThreshold = (int)tbDSPAGCHangThreshold.Value;
        }

        private void tbDSPAGCRX2HangThreshold_Scroll(object sender, System.EventArgs e)
        {
            console.radio.GetDSPRX(1, 0).RXAGCHangThreshold = (int)tbDSPAGCRX2HangThreshold.Value;
            console.radio.GetDSPRX(1, 1).RXAGCHangThreshold = (int)tbDSPAGCRX2HangThreshold.Value;
        }

        #endregion

        #region Leveler

        private void udDSPLevelerHangTime_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPTX(0).TXLevelerHang = (int)udDSPLevelerHangTime.Value;
        }

        private void udDSPLevelerThreshold_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPTX(0).TXLevelerMaxGain = (double)udDSPLevelerThreshold.Value;
        }

        private void udDSPLevelerAttack_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPTX(0).TXLevelerAttack = (int)udDSPLevelerAttack.Value;
        }

        private void udDSPLevelerDecay_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPTX(0).TXLevelerDecay = (int)udDSPLevelerDecay.Value;
        }

        private void chkDSPLevelerEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPTX(0).TXLevelerOn = chkDSPLevelerEnabled.Checked;
        }

        #endregion

        #region ALC

        private void chkDSPALCEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPTX(0).TXALCOn = chkDSPALCEnabled.Checked;
        }

        private void udDSPALCHangTime_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPTX(0).TXALCHang = (int)udDSPALCHangTime.Value;
        }

        private void udDSPALCThreshold_ValueChanged(object sender, System.EventArgs e)
        {
            //DttSP.SetTXALCBot((double)udDSPALCThreshold.Value);
        }

        private void udDSPALCAttack_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPTX(0).TXALCAttack = (int)udDSPALCAttack.Value;
        }

        private void udDSPALCDecay_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPTX(0).TXALCDecay = (int)udDSPALCDecay.Value;
        }

        #endregion

        #endregion

        #region Transmit Tab Event Handlers

        private void udTXFilterHigh_ValueChanged(object sender, System.EventArgs e)
        {
            if (udTXFilterHigh.Value < udTXFilterLow.Value + 100)
            {
                udTXFilterHigh.Value = udTXFilterLow.Value + 100;
                return;
            }

            if (udTXFilterHigh.Focused &&
                (udTXFilterHigh.Value - udTXFilterLow.Value) > 3000 &&
                (console.TXFilterHigh - console.TXFilterLow) <= 3000)
            {
                (new Thread(new ThreadStart(TXBW))).Start();
            }

            console.TXFilterHigh = (int)udTXFilterHigh.Value;

        }

        private void TXBW()
        {
            MessageBox.Show("The transmit bandwidth is being increased beyond 3kHz.\n\n" +
                "As the control operator, you are responsible for compliance with current " +
                "rules and good operating practice.",
                "Warning: Transmit Bandwidth",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void udTXFilterLow_ValueChanged(object sender, System.EventArgs e)
        {
            if (udTXFilterLow.Value > udTXFilterHigh.Value - 100)
            {
                udTXFilterLow.Value = udTXFilterHigh.Value - 100;
                return;
            }

            if (udTXFilterLow.Focused &&
                (udTXFilterHigh.Value - udTXFilterLow.Value) > 3000 &&
                (console.TXFilterHigh - console.TXFilterLow) <= 3000)
            {
                (new Thread(new ThreadStart(TXBW))).Start();
            }

            console.TXFilterLow = (int)udTXFilterLow.Value;
        }

        private void udTransmitTunePower_ValueChanged(object sender, System.EventArgs e)
        {
            console.TunePower = (int)udTXTunePower.Value;
        }

        private string current_profile = "";
        private void comboTXProfileName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboTXProfileName.SelectedIndex < 0 || initializing)
                return;

            if (chkAutoSaveTXProfile.Checked)
            {
                SaveTXProfileData();
            }
            else
            {
                if (CheckTXProfileChanged() && comboTXProfileName.Focused)
                {
                    DialogResult result = MessageBox.Show("The current profile has changed.  " +
                        "Would you like to save the current profile?",
                        "Save Current Profile?",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        btnTXProfileSave_Click(this, EventArgs.Empty);
                        //return;
                    }
                    else if (result == DialogResult.Cancel)
                        return;
                }
            }

            console.TXProfile = comboTXProfileName.Text;
            DataRow[] rows = DB.ds.Tables["TxProfile"].Select(
                "'" + comboTXProfileName.Text + "' = Name");

            if (rows.Length != 1)
            {
                MessageBox.Show("Database error reading TxProfile Table.",
                    "Database error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            DataRow dr = rows[0];
            int[] eq = null;
            eq = new int[11];

            console.EQForm.TXEQEnabled = (bool)dr["TXEQEnabled"];
            console.EQForm.NumBands = (int)dr["TXEQNumBands"];

            eq[0] = (int)dr["TXEQPreamp"];
            for (int i = 1; i < eq.Length; i++)
                eq[i] = (int)dr["TXEQ" + i.ToString()];
            console.EQForm.TXEQ = eq;

            udTXFilterLow.Value = Math.Min(Math.Max((int)dr["FilterLow"], udTXFilterLow.Minimum), udTXFilterLow.Maximum);
            udTXFilterHigh.Value = Math.Min(Math.Max((int)dr["FilterHigh"], udTXFilterHigh.Minimum), udTXFilterHigh.Maximum);

            console.DX = (bool)dr["DXOn"];
            console.DXLevel = (int)dr["DXLevel"];

            console.CPDR = (bool)dr["CompanderOn"];
            console.CPDRLevel = (int)dr["CompanderLevel"];

            console.Mic = (int)dr["MicGain"];
            console.FMMic = (int)dr["FMMicGain"];

            chkDSPLevelerEnabled.Checked = (bool)dr["Lev_On"];
            udDSPLevelerSlope.Value = (int)dr["Lev_Slope"];
            udDSPLevelerThreshold.Value = (int)dr["Lev_MaxGain"];
            udDSPLevelerAttack.Value = (int)dr["Lev_Attack"];
            udDSPLevelerDecay.Value = (int)dr["Lev_Decay"];
            udDSPLevelerHangTime.Value = (int)dr["Lev_Hang"];
            tbDSPLevelerHangThreshold.Value = (int)dr["Lev_HangThreshold"];

            udDSPALCSlope.Value = (int)dr["ALC_Slope"];
            udDSPALCThreshold.Value = (int)dr["ALC_MaxGain"];
            udDSPALCAttack.Value = (int)dr["ALC_Attack"];
            udDSPALCDecay.Value = (int)dr["ALC_Decay"];
            udDSPALCHangTime.Value = (int)dr["ALC_Hang"];
            tbDSPALCHangThreshold.Value = (int)dr["ALC_HangThreshold"];

            console.PWR = (int)dr["Power"];
            // W4TME_
            chkTXNoiseGateEnabled.Checked = (bool)dr["Dexp_On"];
            udTXNoiseGate.Value = (int)dr["Dexp_Threshold"];
            udTXNoiseGateAttenuate.Value = (int)dr["Dexp_Attenuate"];

            chkTXVOXEnabled.Checked = (bool)dr["VOX_On"];
            udTXVOXThreshold.Value = (int)dr["VOX_Threshold"];
            udTXVOXHangTime.Value = (int)dr["VOX_HangTime"];

            udTXTunePower.Value = (int)dr["Tune_Power"];
            comboTXTUNMeter.Text = (string)dr["Tune_Meter_Type"];

            chkTXLimitSlew.Checked = (bool)dr["TX_Limit_Slew"];

            console.TXAF = (int)dr["TX_AF_Level"];

            udTXAMCarrierLevel.Value = (int)dr["AM_Carrier_Level"];

            console.ShowTXFilter = (bool)dr["Show_TX_Filter"];

            chkAudioEnableVAC.Checked = (bool)dr["VAC1_On"];
            chkAudioVACAutoEnable.Checked = (bool)dr["VAC1_Auto_On"];
            udAudioVACGainRX.Value = (int)dr["VAC1_RX_GAIN"];
            udAudioVACGainTX.Value = (int)dr["VAC1_TX_GAIN"];
            chkAudio2Stereo.Checked = (bool)dr["VAC1_Stereo_On"];
            comboAudioSampleRate2.Text = (string)dr["VAC1_Sample_Rate"];
            comboAudioBuffer2.Text = (string)dr["VAC1_Buffer_Size"];
            chkAudioIQtoVAC.Checked = (bool)dr["VAC1_IQ_Output"];
            chkAudioCorrectIQ.Checked = (bool)dr["VAC1_IQ_Correct"];
            chkVACAllowBypass.Checked = (bool)dr["VAC1_PTT_OverRide"];
            chkVACCombine.Checked = (bool)dr["VAC1_Combine_Input_Channels"];
            chkAudioLatencyManual2.Checked = true;
            udAudioLatency2.Value = (int)dr["VAC1_Latency_Duration"];

            chkVAC2Enable.Checked = (bool)dr["VAC2_On"];
            chkVAC2AutoEnable.Checked = (bool)dr["VAC2_Auto_On"];
            udVAC2GainRX.Value = (int)dr["VAC2_RX_GAIN"];
            udVAC2GainTX.Value = (int)dr["VAC2_TX_GAIN"];
            chkAudioStereo3.Checked = (bool)dr["VAC2_Stereo_On"];
            comboAudioSampleRate3.Text = (string)dr["VAC2_Sample_Rate"];
            comboAudioBuffer3.Text = (string)dr["VAC2_Buffer_Size"];
            chkVAC2DirectIQ.Checked = (bool)dr["VAC2_IQ_Output"];
            chkVAC2DirectIQCal.Checked = (bool)dr["VAC2_IQ_Correct"];
            chkVAC2Combine.Checked = (bool)dr["VAC2_Combine_Input_Channels"];
            chkVAC2LatencyManual.Checked = true;
            udVAC2Latency.Value = (int)dr["VAC2_Latency_Duration"];

            comboDSPPhoneRXBuf.Text = (string)dr["Phone_RX_DSP_Buffer"];
            comboDSPPhoneTXBuf.Text = (string)dr["Phone_TX_DSP_Buffer"];
            comboDSPDigRXBuf.Text = (string)dr["Digi_RX_DSP_Buffer"];
            comboDSPDigTXBuf.Text = (string)dr["Digi_TX_DSP_Buffer"];
            comboDSPCWRXBuf.Text = (string)dr["CW_RX_DSP_Buffer"];

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    if (console.fwcMixForm != null)
                    {
                        console.fwcMixForm.MicInputSelected = (string)dr["Mic_Input_On"];
                        console.fwcMixForm.MicInput = (int)dr["Mic_Input_Level"];
                        console.fwcMixForm.LineInRCASelected = (string)dr["Line_Input_On"];
                        console.fwcMixForm.LineInRCA = (int)dr["Line_Input_Level"];
                        console.fwcMixForm.LineInPhonoSelected = (string)dr["Balanced_Line_Input_On"];
                        console.fwcMixForm.LineInPhono = (int)dr["Balanced_Line_Input_Level"];
                        console.fwcMixForm.LineInDB9Selected = (string)dr["FlexWire_Input_On"];
                        console.fwcMixForm.LineInDB9 = (int)dr["FlexWire_Input_Level"];
                    }
                    break;

                case Model.FLEX3000:
                    if (console.flex3000MixerForm != null)
                    {
                        console.flex3000MixerForm.MicInputSelected = (string)dr["Mic_Input_On"];
                        console.flex3000MixerForm.MicInput = (int)dr["Mic_Input_Level"];
                        console.flex3000MixerForm.LineInDB9Selected = (string)dr["FlexWire_Input_On"];
                        console.flex3000MixerForm.LineInDB9 = (int)dr["FlexWire_Input_Level"];
                    }
                    break;

                 default:
                    // do nothing for other radios models
                    break;
            }

            current_profile = comboTXProfileName.Text;
        }

        private void btnTXProfileSave_Click(object sender, System.EventArgs e)
        {
            string name = InputBox.Show("Save Profile", "Please enter a profile name:",
                current_profile);

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("TX Profile Save cancelled",
                    "TX Profile",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            DataRow dr = null;
            if (comboTXProfileName.Items.Contains(name))
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to overwrite the " + name + " TX Profile?",
                    "Overwrite Profile?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    return;

                foreach (DataRow d in from DataRow d in DB.ds.Tables["TxProfile"].Rows where (string)d["Name"] == name select d)
                {
                    dr = d;
                    break;
                }
            }
            else
            {
                dr = DB.ds.Tables["TxProfile"].NewRow();
                dr["Name"] = name;
            }

            dr["FilterLow"] = (int)udTXFilterLow.Value;
            dr["FilterHigh"] = (int)udTXFilterHigh.Value;
            dr["TXEQEnabled"] = console.EQForm.TXEQEnabled;
            dr["TXEQNumBands"] = console.EQForm.NumBands;
            int[] eq = console.EQForm.TXEQ;
            dr["TXEQPreamp"] = eq[0];
            for (int i = 1; i < eq.Length; i++)
                dr["TXEQ" + i.ToString()] = eq[i];
            for (int i = eq.Length; i < 11; i++)
                dr["TXEQ" + i.ToString()] = 0;

            dr["DXOn"] = console.DX;
            dr["DXLevel"] = console.DXLevel;
            dr["CompanderOn"] = console.CPDR;
            dr["CompanderLevel"] = console.CPDRLevel;
            dr["MicGain"] = console.Mic;
            dr["FMMicGain"] = console.FMMic;

            dr["Lev_On"] = chkDSPLevelerEnabled.Checked;
            dr["Lev_Slope"] = (int)udDSPLevelerSlope.Value;
            dr["Lev_MaxGain"] = (int)udDSPLevelerThreshold.Value;
            dr["Lev_Attack"] = (int)udDSPLevelerAttack.Value;
            dr["Lev_Decay"] = (int)udDSPLevelerDecay.Value;
            dr["Lev_Hang"] = (int)udDSPLevelerHangTime.Value;
            dr["Lev_HangThreshold"] = tbDSPLevelerHangThreshold.Value;

            dr["ALC_Slope"] = (int)udDSPALCSlope.Value;
            dr["ALC_MaxGain"] = (int)udDSPALCThreshold.Value;
            dr["ALC_Attack"] = (int)udDSPALCAttack.Value;
            dr["ALC_Decay"] = (int)udDSPALCDecay.Value;
            dr["ALC_Hang"] = (int)udDSPALCHangTime.Value;
            dr["ALC_HangThreshold"] = tbDSPALCHangThreshold.Value;

            dr["Power"] = console.PWR;

            // W4TME
            dr["Dexp_On"] = chkTXNoiseGateEnabled.Checked;
            dr["Dexp_Threshold"] = (int)udTXNoiseGate.Value;
            dr["Dexp_Attenuate"] = (int)udTXNoiseGateAttenuate.Value;

            dr["VOX_On"] = chkTXVOXEnabled.Checked;
            dr["VOX_Threshold"] = (int)udTXVOXThreshold.Value;
            dr["VOX_HangTime"] = (int)udTXVOXHangTime.Value;

            dr["Tune_Power"] = (int)udTXTunePower.Value;
            dr["Tune_Meter_Type"] = (string)comboTXTUNMeter.Text;

            dr["TX_Limit_Slew"] = (bool)chkTXLimitSlew.Checked;

            dr["TX_AF_Level"] = console.TXAF;

            dr["AM_Carrier_Level"] = (int)udTXAMCarrierLevel.Value;

            dr["Show_TX_Filter"] = (bool)console.ShowTXFilter;

            dr["VAC1_On"] = (bool)chkAudioEnableVAC.Checked;
            dr["VAC1_Auto_On"] = (bool)chkAudioVACAutoEnable.Checked;
            dr["VAC1_RX_GAIN"] = (int)udAudioVACGainRX.Value;
            dr["VAC1_TX_GAIN"] = (int)udAudioVACGainTX.Value;
            dr["VAC1_Stereo_On"] = (bool)chkAudio2Stereo.Checked;
            dr["VAC1_Sample_Rate"] = (string)comboAudioSampleRate2.Text;
            dr["VAC1_Buffer_Size"] = (string)comboAudioBuffer2.Text;
            dr["VAC1_IQ_Output"] = (bool)chkAudioIQtoVAC.Checked;
            dr["VAC1_IQ_Correct"] = (bool)chkAudioCorrectIQ.Checked;
            dr["VAC1_PTT_OverRide"] = (bool)chkVACAllowBypass.Checked;
            dr["VAC1_Combine_Input_Channels"] = (bool)chkVACCombine.Checked;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = (int)udAudioLatency2.Value;

            dr["VAC2_On"] = (bool)chkVAC2Enable.Checked;
            dr["VAC2_Auto_On"] = (bool)chkVAC2AutoEnable.Checked;
            dr["VAC2_RX_GAIN"] = (int)udVAC2GainRX.Value;
            dr["VAC2_TX_GAIN"] = (int)udVAC2GainTX.Value;
            dr["VAC2_Stereo_On"] = (bool)chkAudioStereo3.Checked;
            dr["VAC2_Sample_Rate"] = (string)comboAudioSampleRate3.Text;
            dr["VAC2_Buffer_Size"] = (string)comboAudioBuffer3.Text;
            dr["VAC2_IQ_Output"] = (bool)chkVAC2DirectIQ.Checked;
            dr["VAC2_IQ_Correct"] = (bool)chkVAC2DirectIQCal.Checked;
            dr["VAC2_Combine_Input_Channels"] = (bool)chkVAC2Combine.Checked;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = (int)udVAC2Latency.Value;

            dr["Phone_RX_DSP_Buffer"] = (string)comboDSPPhoneRXBuf.Text;
            dr["Phone_TX_DSP_Buffer"] = (string)comboDSPPhoneTXBuf.Text;
            dr["Digi_RX_DSP_Buffer"] = (string)comboDSPDigRXBuf.Text;
            dr["Digi_TX_DSP_Buffer"] = (string)comboDSPDigTXBuf.Text;
            dr["CW_RX_DSP_Buffer"] = (string)comboDSPCWRXBuf.Text;

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    dr["Mic_Input_On"] = (string)console.fwcMixForm.MicInputSelected;
                    dr["Mic_Input_Level"] = (int)console.fwcMixForm.MicInput;
                    dr["Line_Input_On"] = (string)console.fwcMixForm.LineInRCASelected;
                    dr["Line_Input_Level"] = (int)console.fwcMixForm.LineInRCA;
                    dr["Balanced_Line_Input_On"] = (string)console.fwcMixForm.LineInPhonoSelected;
                    dr["Balanced_Line_Input_Level"] = (int)console.fwcMixForm.LineInPhono;
                    dr["FlexWire_Input_On"] = (string)console.fwcMixForm.LineInDB9Selected;
                    dr["FlexWire_Input_Level"] = (int)console.fwcMixForm.LineInDB9;
                    break;

                case Model.FLEX3000:
                    dr["Mic_Input_On"] = (string)console.flex3000MixerForm.MicInputSelected;
                    dr["Mic_Input_Level"] = (int)console.flex3000MixerForm.MicInput;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = (string)console.flex3000MixerForm.LineInDB9Selected;
                    dr["FlexWire_Input_Level"] = (int)console.flex3000MixerForm.LineInDB9;
                    break;

                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }
            
            if (!comboTXProfileName.Items.Contains(name))
            {
                DB.ds.Tables["TxProfile"].Rows.Add(dr);
                comboTXProfileName.Items.Add(name);
                comboTXProfileName.Text = name;
            }

            console.UpdateTXProfile(name);
        }

        private bool profile_deleted = false;
        private void btnTXProfileDelete_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "Are you sure you want to delete the " + comboTXProfileName.Text + " TX Profile?",
                "Delete Profile?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr == DialogResult.No)
                return;

            profile_deleted = true;

            DataRow[] rows = DB.ds.Tables["TxProfile"].Select(
                "'" + comboTXProfileName.Text + "' = Name");

            if (rows.Length == 1)
                rows[0].Delete();

            int index = comboTXProfileName.SelectedIndex;
            comboTXProfileName.Items.Remove(comboTXProfileName.Text);
            if (comboTXProfileName.Items.Count > 0)
            {
                if (index > comboTXProfileName.Items.Count - 1)
                    index = comboTXProfileName.Items.Count - 1;
                comboTXProfileName.SelectedIndex = index;
            }

            console.UpdateTXProfile(comboTXProfileName.Text);
        }

        private void chkDCBlock_CheckedChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPTX(0).DCBlock = chkDCBlock.Checked;
        }

        private void chkTXVOXEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            Audio.VOXEnabled = chkTXVOXEnabled.Checked;
            console.VOXEnable = chkTXVOXEnabled.Checked;
        }

        private void udTXVOXThreshold_ValueChanged(object sender, System.EventArgs e)
        {
            Audio.VOXThreshold = (float)udTXVOXThreshold.Value / 10000.0f;
            console.VOXSens = (int)udTXVOXThreshold.Value;
        }

        private void udVOXGain_ValueChanged(object sender, System.EventArgs e)
        {
            Audio.VOXGain = (float)udVOXGain.Value / 10000.0f;
        }

        private void udTXVOXHangTime_ValueChanged(object sender, System.EventArgs e)
        {
            console.VOXHangTime = (int)udTXVOXHangTime.Value;
        }

        private void udTXNoiseGate_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPTX(0).TXSquelchThreshold = (float)udTXNoiseGate.Value;
            console.NoiseGate = (int)udTXNoiseGate.Value;
        }

        private void chkTXNoiseGateEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPTX(0).TXSquelchOn = chkTXNoiseGateEnabled.Checked;
            console.NoiseGateEnabled = chkTXNoiseGateEnabled.Checked;
        }

        private void udTXAF_ValueChanged(object sender, System.EventArgs e)
        {
            console.TXAF = (int)udTXAF.Value;
        }

        private void udTXAMCarrierLevel_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPTX(0).TXAMCarrierLevel = Math.Sqrt((0.01 * (double)udTXAMCarrierLevel.Value)/2);
        }

        private void chkSaveTXProfileOnExit_CheckedChanged(object sender, EventArgs e)
        {
            console.SaveTXProfileOnExit = chkSaveTXProfileOnExit.Checked;
        }

        private void udMicGainMin_ValueChanged(object sender, System.EventArgs e)
        {
            console.MicGainMin = (int)udMicGainMin.Value;
        }

        private void udMicGainMax_ValueChanged(object sender, System.EventArgs e)
        {
            console.MicGainMax = (int)udMicGainMax.Value;
        }

        private void udLineInBoost_ValueChanged(object sender, System.EventArgs e)
        {
            console.LineInBoost = (double)udLineInBoost.Value;
        }

        private void chkShowTopControls_CheckedChanged(object sender, EventArgs e)
        {
            this.console.ShowTopControls = chkShowTopControls.Checked;
            this.console.topControlsToolStripMenuItem.Checked = chkShowTopControls.Checked;
            this.console.bandToolStripMenuItem.Visible = !chkShowBandControls.Checked;

            if (this.console.CollapsedDisplay)
                this.console.CollapseDisplay();
        }

        private void chkShowBandControls_CheckedChanged(object sender, EventArgs e)
        {
            this.console.ShowBandControls = chkShowBandControls.Checked;
            this.console.bandControlsToolStripMenuItem.Checked = chkShowBandControls.Checked;
            this.console.modeToolStripMenuItem.Visible = !chkShowModeControls.Checked;

            if (this.console.CollapsedDisplay)
                this.console.CollapseDisplay();
        }

        private void chkModeControls_CheckedChanged(object sender, EventArgs e)
        {
            this.console.ShowModeControls = chkShowModeControls.Checked;
            this.console.modeControlsToolStripMenuItem.Checked = chkShowModeControls.Checked;

            if (this.console.CollapsedDisplay)
                this.console.CollapseDisplay();
        }


        #endregion

        #region PA Settings Tab Event Handlers

        private void btnPAGainCalibration_Click(object sender, System.EventArgs e)
        {
            string s = "NOTE: this routine works well with Penelope. At present this calibration\n" +
                       "routine is NOT recommended if you are using PennyLane, Hermes or \n" +
                       "Angelia as it produces a large overshoot during the calibraion \n" +
                       "process when used with those boards! \n\n" +
                "Is a 50 Ohm dummy load connected to the amplifier?\n" +
                "\n This function is valid only with an external amplifier and Alex (or equivalent) present." +
                "\n\nFailure to use a dummy load with this routine could cause damage to the amplifier.";
            if (radGenModelFLEX5000.Checked)
            {
                s = "Is a 50 Ohm dummy load connected to the correct antenna port (";
                switch (FWCAnt.ANT1)
                {
                    case FWCAnt.ANT1: s += "ANT 1"; break;
                    /*case FWCAnt.ANT2: s += "ANT 2"; break;
                    case FWCAnt.ANT3: s += "ANT 3"; break;*/
                }
                s += ")?\nFailure to connect a dummy load properly could cause damage to the radio.";
            }
            DialogResult dr = MessageBox.Show(s,
                "Warning: Is dummy load properly connected?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr == DialogResult.No)
                return;

            btnPAGainCalibration.Enabled = false;
            progress = new Progress("Calibrate PA Gain");

            Thread t = new Thread(CalibratePAGain)
            {
                Name = "PA Gain Calibration Thread",
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal
            };
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void CalibratePAGain()
        {
            bool[] run = new bool[11];

            if (radPACalAllBands.Checked)
            {
                for (int i = 0; i < 11; i++) run[i] = true;
            }
            else
            {
                run[0] = chkPA160.Checked;
                run[1] = chkPA80.Checked;
                run[2] = chkPA60.Checked;
                run[3] = chkPA40.Checked;
                run[4] = chkPA30.Checked;
                run[5] = chkPA20.Checked;
                run[6] = chkPA17.Checked;
                run[7] = chkPA15.Checked;
                run[8] = chkPA12.Checked;
                run[9] = chkPA10.Checked;
                run[10] = chkPA6.Checked;
            }
            bool done = false;
            done = chkPANewCal.Checked ? console.CalibratePAGain2(progress, run, false) : console.CalibratePAGain(progress, run, (int)udPACalPower.Value);
            if (done) MessageBox.Show("PA Gain Calibration complete.");
            btnPAGainCalibration.Enabled = true;
        }

        private void udPAGain_ValueChanged(object sender, System.EventArgs e)
        {
            console.PWR = console.PWR;
        }

        private void btnPAGainReset_Click(object sender, System.EventArgs e)
        {
            udPAGain160.Value = 41.0M;
            udPAGain80.Value = 41.2M;
            udPAGain60.Value = 41.3M;
            udPAGain40.Value = 41.3M;
            udPAGain30.Value = 41.0M;
            udPAGain20.Value = 40.5M;
            udPAGain17.Value = 39.9M;
            udPAGain15.Value = 38.8M;
            udPAGain12.Value = 38.8M;
            udPAGain10.Value = 38.8M;
            udPAGain6.Value = 38.8M;
            udPAGainVHF0.Value = 38.8M;
            udPAGainVHF1.Value = 38.8M;
            udPAGainVHF2.Value = 38.8M;
            udPAGainVHF3.Value = 38.8M;
            udPAGainVHF4.Value = 38.8M;
            udPAGainVHF5.Value = 38.8M;
            udPAGainVHF6.Value = 38.8M;
            udPAGainVHF7.Value = 38.8M;
            udPAGainVHF8.Value = 38.8M;
            udPAGainVHF9.Value = 38.8M;
            udPAGainVHF10.Value = 38.8M;
            udPAGainVHF11.Value = 38.8M;
            udPAGainVHF12.Value = 38.8M;
            udPAGainVHF13.Value = 38.8M;
        }

        #endregion

        #region Appearance Tab Event Handlers

        private void clrbtnBackground_Changed(object sender, System.EventArgs e)
        {
            Display.DisplayBackgroundColor = Color.FromArgb(tbBackgroundAlpha.Value, clrbtnBackground.Color);
            //Display.DisplayBackgroundColor = clrbtnBackground.Color;
        }

        private void clrbtnTXBackground_Changed(object sender, System.EventArgs e)
        {
            Display.TXDisplayBackgroundColor = Color.FromArgb(tbTXBackgroundAlpha.Value, clrbtnTXBackground.Color);
        }

        private void clrbtnGrid_Changed(object sender, System.EventArgs e)
        {
            Display.GridColor = Color.FromArgb(tbGridCourseAlpha.Value, clrbtnGrid.Color);
            //Display.GridColor = clrbtnGrid.Color;
        }

        private void clrbtnTXVGrid_Changed(object sender, System.EventArgs e)
        {
            Display.TXVGridColor = Color.FromArgb(tbTXVGridCourseAlpha.Value, clrbtnTXVGrid.Color);
        }

        private void clrbtnZeroLine_Changed(object sender, System.EventArgs e)
        {
            Display.GridZeroColor = clrbtnZeroLine.Color;
        }

        private void clrbtnTXZeroLine_Changed(object sender, System.EventArgs e)
        {
            Display.TXGridZeroColor = Color.FromArgb(tbTXZeroLineAlpha.Value, clrbtnTXZeroLine.Color);
        }

        private void clrbtnText_Changed(object sender, System.EventArgs e)
        {
            Display.GridTextColor = clrbtnText.Color;
        }

        private void clrbtnTXText_Changed(object sender, System.EventArgs e)
        {
            Display.GridTXTextColor = Color.FromArgb(tbTXTextAlpha.Value, clrbtnTXText.Color);
        }

        private void clrbtnDataLine_Changed(object sender, System.EventArgs e)
        {
            Display.DataLineColor = clrbtnDataLine.Color;
        }

        private void clrbtnTXDataLine_Changed(object sender, System.EventArgs e)
        {
            Display.TXDataLineColor = clrbtnTXDataLine.Color;
        }

        private void clrbtnFilter_Changed(object sender, System.EventArgs e)
        {
            Display.DisplayFilterColor = Color.FromArgb(tbRX1FilterAlpha.Value, clrbtnFilter.Color);
        }

        private void clrbtnGridTXFilter_Changed(object sender, System.EventArgs e)
        {
            Display.TXFilterColor = Color.FromArgb(tbTXFilterAlpha.Value, clrbtnGridTXFilter.Color);
        }

        private void udDisplayLineWidth_ValueChanged(object sender, System.EventArgs e)
        {
            Display.DisplayLineWidth = (float)udDisplayLineWidth.Value;
        }

        private void udTXLineWidth_ValueChanged(object sender, System.EventArgs e)
        {
            Display.TXDisplayLineWidth = (float)udTXLineWidth.Value;
        }

        private void clrbtnMeterLeft_Changed(object sender, System.EventArgs e)
        {
            console.MeterLeftColor = clrbtnMeterLeft.Color;
        }

        private void clrbtnMeterRight_Changed(object sender, System.EventArgs e)
        {
            console.MeterRightColor = clrbtnMeterRight.Color;
        }

        private void clrbtnBtnSel_Changed(object sender, System.EventArgs e)
        {
            console.ButtonSelectedColor = clrbtnBtnSel.Color;
        }

        private void clrbtnVFODark_Changed(object sender, System.EventArgs e)
        {
            console.VFOTextDarkColor = clrbtnVFODark.Color;
        }

        private void clrbtnVFOLight_Changed(object sender, System.EventArgs e)
        {
            console.VFOTextLightColor = clrbtnVFOLight.Color;
        }

        private void clrbtnBandDark_Changed(object sender, System.EventArgs e)
        {
            console.BandTextDarkColor = clrbtnBandDark.Color;
        }

        private void clrbtnBandLight_Changed(object sender, System.EventArgs e)
        {
            console.BandTextLightColor = clrbtnBandLight.Color;
        }

        private void clrbtnPeakText_Changed(object sender, System.EventArgs e)
        {
            console.PeakTextColor = clrbtnPeakText.Color;
        }

        private void clrbtnOutOfBand_Changed(object sender, System.EventArgs e)
        {
            console.OutOfBandColor = clrbtnOutOfBand.Color;
        }

        private void chkVFOSmallLSD_CheckedChanged(object sender, System.EventArgs e)
        {
            console.SmallLSD = chkVFOSmallLSD.Checked;
        }

        private void clrbtnVFOSmallColor_Changed(object sender, System.EventArgs e)
        {
            console.SmallVFOColor = clrbtnVFOSmallColor.Color;
        }

        private void clrbtnInfoButtonsColor_Changed(object sender, System.EventArgs e)
        {
            console.InfoButtonsColor = clrbtnInfoButtonsColor.Color;
        }

        private void clrbtnPeakBackground_Changed(object sender, System.EventArgs e)
        {
            console.PeakBackgroundColor = clrbtnPeakBackground.Color;
        }

        private void clrbtnMeterBackground_Changed(object sender, System.EventArgs e)
        {
            console.MeterBackgroundColor = clrbtnMeterBackground.Color;
        }

        private void clrbtnBandBackground_Changed(object sender, System.EventArgs e)
        {
            console.BandBackgroundColor = clrbtnBandBackground.Color;
        }

        private void clrbtnVFOBackground_Changed(object sender, System.EventArgs e)
        {
            console.VFOBackgroundColor = clrbtnVFOBackground.Color;
        }

        #endregion

        #region Keyboard Tab Event Handlers

        private void comboKBTuneUp1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneUp1 = (Keys)KeyList[comboKBTuneUp1.SelectedIndex];
        }

        private void comboKBTuneDown1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneDown1 = (Keys)KeyList[comboKBTuneDown1.SelectedIndex];
        }

        private void comboKBTuneUp2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneUp2 = (Keys)KeyList[comboKBTuneUp2.SelectedIndex];
        }

        private void comboKBTuneDown2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneDown2 = (Keys)KeyList[comboKBTuneDown2.SelectedIndex];
        }

        private void comboKBTuneUp3_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneUp3 = (Keys)KeyList[comboKBTuneUp3.SelectedIndex];
        }

        private void comboKBTuneDown3_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneDown3 = (Keys)KeyList[comboKBTuneDown3.SelectedIndex];
        }

        private void comboKBTuneUp4_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneUp4 = (Keys)KeyList[comboKBTuneUp4.SelectedIndex];
        }

        private void comboKBTuneDown4_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneDown4 = (Keys)KeyList[comboKBTuneDown4.SelectedIndex];
        }

        private void comboKBTuneUp5_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneUp5 = (Keys)KeyList[comboKBTuneUp5.SelectedIndex];
        }

        private void comboKBTuneDown5_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneDown5 = (Keys)KeyList[comboKBTuneDown5.SelectedIndex];
        }

        private void comboKBTuneUp6_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneUp6 = (Keys)KeyList[comboKBTuneUp6.SelectedIndex];
        }

        private void comboKBTuneDown6_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneDown6 = (Keys)KeyList[comboKBTuneDown6.SelectedIndex];
        }

        private void comboKBTuneUp7_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneUp7 = (Keys)KeyList[comboKBTuneUp7.SelectedIndex];
        }

        private void comboKBTuneDown7_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyTuneDown7 = (Keys)KeyList[comboKBTuneDown7.SelectedIndex];
        }

        private void comboKBBandUp_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyBandUp = (Keys)KeyList[comboKBBandUp.SelectedIndex];
        }

        private void comboKBBandDown_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyBandDown = (Keys)KeyList[comboKBBandDown.SelectedIndex];
        }

        private void comboKBFilterUp_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyFilterUp = (Keys)KeyList[comboKBFilterUp.SelectedIndex];
        }

        private void comboKBFilterDown_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyFilterDown = (Keys)KeyList[comboKBFilterDown.SelectedIndex];
        }

        private void comboKBModeUp_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyModeUp = (Keys)KeyList[comboKBModeUp.SelectedIndex];
        }

        private void comboKBModeDown_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyModeDown = (Keys)KeyList[comboKBModeDown.SelectedIndex];
        }

        private void comboKBCWDot_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyCWDot = (Keys)KeyList[comboKBCWDot.SelectedIndex];
        }

        private void comboKBCWDash_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyCWDash = (Keys)KeyList[comboKBCWDash.SelectedIndex];
        }

        private void comboKBRITUp_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyRITUp = (Keys)KeyList[comboKBRITUp.SelectedIndex];
        }

        private void comboKBRITDown_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyRITDown = (Keys)KeyList[comboKBRITDown.SelectedIndex];
        }

        private void comboKBXITUp_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyXITUp = (Keys)KeyList[comboKBXITUp.SelectedIndex];
        }

        private void comboKBXITDown_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.KeyXITDown = (Keys)KeyList[comboKBXITDown.SelectedIndex];
        }

        #endregion

        #region Ext Ctrl Tab Event Handlers

        private void chkExtRX160_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX1601.Checked) val += 1 << 0;
            if (chkExtRX1602.Checked) val += 1 << 1;
            if (chkExtRX1603.Checked) val += 1 << 2;
            if (chkExtRX1604.Checked) val += 1 << 3;
            if (chkExtRX1605.Checked) val += 1 << 4;
            if (chkExtRX1606.Checked) val += 1 << 5;

            console.X2160RX = (byte)val;
        }

        private void chkExtTX160_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX1601.Checked) val += 1 << 0;
            if (chkExtTX1602.Checked) val += 1 << 1;
            if (chkExtTX1603.Checked) val += 1 << 2;
            if (chkExtTX1604.Checked) val += 1 << 3;
            if (chkExtTX1605.Checked) val += 1 << 4;
            if (chkExtTX1606.Checked) val += 1 << 5;

            console.X2160TX = (byte)val;
        }

        private void chkExtRX80_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX801.Checked) val += 1 << 0;
            if (chkExtRX802.Checked) val += 1 << 1;
            if (chkExtRX803.Checked) val += 1 << 2;
            if (chkExtRX804.Checked) val += 1 << 3;
            if (chkExtRX805.Checked) val += 1 << 4;
            if (chkExtRX806.Checked) val += 1 << 5;

            console.X280RX = (byte)val;
        }

        private void chkExtTX80_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX801.Checked) val += 1 << 0;
            if (chkExtTX802.Checked) val += 1 << 1;
            if (chkExtTX803.Checked) val += 1 << 2;
            if (chkExtTX804.Checked) val += 1 << 3;
            if (chkExtTX805.Checked) val += 1 << 4;
            if (chkExtTX806.Checked) val += 1 << 5;

            console.X280TX = (byte)val;
        }

        private void chkExtRX60_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX601.Checked) val += 1 << 0;
            if (chkExtRX602.Checked) val += 1 << 1;
            if (chkExtRX603.Checked) val += 1 << 2;
            if (chkExtRX604.Checked) val += 1 << 3;
            if (chkExtRX605.Checked) val += 1 << 4;
            if (chkExtRX606.Checked) val += 1 << 5;

            console.X260RX = (byte)val;
        }

        private void chkExtTX60_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX601.Checked) val += 1 << 0;
            if (chkExtTX602.Checked) val += 1 << 1;
            if (chkExtTX603.Checked) val += 1 << 2;
            if (chkExtTX604.Checked) val += 1 << 3;
            if (chkExtTX605.Checked) val += 1 << 4;
            if (chkExtTX606.Checked) val += 1 << 5;

            console.X260TX = (byte)val;
        }

        private void chkExtRX40_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX401.Checked) val += 1 << 0;
            if (chkExtRX402.Checked) val += 1 << 1;
            if (chkExtRX403.Checked) val += 1 << 2;
            if (chkExtRX404.Checked) val += 1 << 3;
            if (chkExtRX405.Checked) val += 1 << 4;
            if (chkExtRX406.Checked) val += 1 << 5;

            console.X240RX = (byte)val;
        }

        private void chkExtTX40_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX401.Checked) val += 1 << 0;
            if (chkExtTX402.Checked) val += 1 << 1;
            if (chkExtTX403.Checked) val += 1 << 2;
            if (chkExtTX404.Checked) val += 1 << 3;
            if (chkExtTX405.Checked) val += 1 << 4;
            if (chkExtTX406.Checked) val += 1 << 5;

            console.X240TX = (byte)val;
        }

        private void chkExtRX30_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX301.Checked) val += 1 << 0;
            if (chkExtRX302.Checked) val += 1 << 1;
            if (chkExtRX303.Checked) val += 1 << 2;
            if (chkExtRX304.Checked) val += 1 << 3;
            if (chkExtRX305.Checked) val += 1 << 4;
            if (chkExtRX306.Checked) val += 1 << 5;

            console.X230RX = (byte)val;
        }

        private void chkExtTX30_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX301.Checked) val += 1 << 0;
            if (chkExtTX302.Checked) val += 1 << 1;
            if (chkExtTX303.Checked) val += 1 << 2;
            if (chkExtTX304.Checked) val += 1 << 3;
            if (chkExtTX305.Checked) val += 1 << 4;
            if (chkExtTX306.Checked) val += 1 << 5;

            console.X230TX = (byte)val;
        }

        private void chkExtRX20_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX201.Checked) val += 1 << 0;
            if (chkExtRX202.Checked) val += 1 << 1;
            if (chkExtRX203.Checked) val += 1 << 2;
            if (chkExtRX204.Checked) val += 1 << 3;
            if (chkExtRX205.Checked) val += 1 << 4;
            if (chkExtRX306.Checked) val += 1 << 5;

            console.X220RX = (byte)val;
        }

        private void chkExtTX20_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX201.Checked) val += 1 << 0;
            if (chkExtTX202.Checked) val += 1 << 1;
            if (chkExtTX203.Checked) val += 1 << 2;
            if (chkExtTX204.Checked) val += 1 << 3;
            if (chkExtTX205.Checked) val += 1 << 4;
            if (chkExtTX306.Checked) val += 1 << 5;

            console.X220TX = (byte)val;
        }

        private void chkExtRX17_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX171.Checked) val += 1 << 0;
            if (chkExtRX172.Checked) val += 1 << 1;
            if (chkExtRX173.Checked) val += 1 << 2;
            if (chkExtRX174.Checked) val += 1 << 3;
            if (chkExtRX175.Checked) val += 1 << 4;
            if (chkExtRX176.Checked) val += 1 << 5;

            console.X217RX = (byte)val;
        }

        private void chkExtTX17_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX171.Checked) val += 1 << 0;
            if (chkExtTX172.Checked) val += 1 << 1;
            if (chkExtTX173.Checked) val += 1 << 2;
            if (chkExtTX174.Checked) val += 1 << 3;
            if (chkExtTX175.Checked) val += 1 << 4;
            if (chkExtTX176.Checked) val += 1 << 5;

            console.X217TX = (byte)val;
        }

        private void chkExtRX15_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX151.Checked) val += 1 << 0;
            if (chkExtRX152.Checked) val += 1 << 1;
            if (chkExtRX153.Checked) val += 1 << 2;
            if (chkExtRX154.Checked) val += 1 << 3;
            if (chkExtRX155.Checked) val += 1 << 4;
            if (chkExtRX156.Checked) val += 1 << 5;

            console.X215RX = (byte)val;
        }

        private void chkExtTX15_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX151.Checked) val += 1 << 0;
            if (chkExtTX152.Checked) val += 1 << 1;
            if (chkExtTX153.Checked) val += 1 << 2;
            if (chkExtTX154.Checked) val += 1 << 3;
            if (chkExtTX155.Checked) val += 1 << 4;
            if (chkExtTX156.Checked) val += 1 << 5;

            console.X215TX = (byte)val;
        }

        private void chkExtRX12_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX121.Checked) val += 1 << 0;
            if (chkExtRX122.Checked) val += 1 << 1;
            if (chkExtRX123.Checked) val += 1 << 2;
            if (chkExtRX124.Checked) val += 1 << 3;
            if (chkExtRX125.Checked) val += 1 << 4;
            if (chkExtRX126.Checked) val += 1 << 5;

            console.X212RX = (byte)val;
        }

        private void chkExtTX12_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX121.Checked) val += 1 << 0;
            if (chkExtTX122.Checked) val += 1 << 1;
            if (chkExtTX123.Checked) val += 1 << 2;
            if (chkExtTX124.Checked) val += 1 << 3;
            if (chkExtTX125.Checked) val += 1 << 4;
            if (chkExtTX126.Checked) val += 1 << 5;

            console.X212TX = (byte)val;
        }

        private void chkExtRX10_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX101.Checked) val += 1 << 0;
            if (chkExtRX102.Checked) val += 1 << 1;
            if (chkExtRX103.Checked) val += 1 << 2;
            if (chkExtRX104.Checked) val += 1 << 3;
            if (chkExtRX105.Checked) val += 1 << 4;
            if (chkExtRX106.Checked) val += 1 << 5;

            console.X210RX = (byte)val;
        }

        private void chkExtTX10_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX101.Checked) val += 1 << 0;
            if (chkExtTX102.Checked) val += 1 << 1;
            if (chkExtTX103.Checked) val += 1 << 2;
            if (chkExtTX104.Checked) val += 1 << 3;
            if (chkExtTX105.Checked) val += 1 << 4;
            if (chkExtTX106.Checked) val += 1 << 5;

            console.X210TX = (byte)val;
        }

        private void chkExtRX6_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX61.Checked) val += 1 << 0;
            if (chkExtRX62.Checked) val += 1 << 1;
            if (chkExtRX63.Checked) val += 1 << 2;
            if (chkExtRX64.Checked) val += 1 << 3;
            if (chkExtRX65.Checked) val += 1 << 4;
            if (chkExtRX66.Checked) val += 1 << 5;

            console.X26RX = (byte)val;
        }

        private void chkExtTX6_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX61.Checked) val += 1 << 0;
            if (chkExtTX62.Checked) val += 1 << 1;
            if (chkExtTX63.Checked) val += 1 << 2;
            if (chkExtTX64.Checked) val += 1 << 3;
            if (chkExtTX65.Checked) val += 1 << 4;
            if (chkExtTX66.Checked) val += 1 << 5;

            console.X26TX = (byte)val;
        }

        private void chkExtRX2_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtRX21.Checked) val += 1 << 0;
            if (chkExtRX22.Checked) val += 1 << 1;
            if (chkExtRX23.Checked) val += 1 << 2;
            if (chkExtRX24.Checked) val += 1 << 3;
            if (chkExtRX25.Checked) val += 1 << 4;
            if (chkExtRX26.Checked) val += 1 << 5;

            console.X22RX = (byte)val;
        }

        private void chkExtTX2_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkExtTX21.Checked) val += 1 << 0;
            if (chkExtTX22.Checked) val += 1 << 1;
            if (chkExtTX23.Checked) val += 1 << 2;
            if (chkExtTX24.Checked) val += 1 << 3;
            if (chkExtTX25.Checked) val += 1 << 4;
            if (chkExtTX26.Checked) val += 1 << 5;

            console.X22TX = (byte)val;
        }

        private void chkExtEnable_CheckedChanged(object sender, System.EventArgs e)
        {
            grpExtRX.Enabled = chkExtEnable.Checked;
            grpExtTX.Enabled = chkExtEnable.Checked;
            console.ExtCtrlEnabled = chkExtEnable.Checked;
        }

        #endregion

        #region CAT Setup event handlers

        public void initCATandPTTprops()
        {
            console.CATEnabled = chkCATEnable.Checked;
            if (comboCATPort.Text.StartsWith("COM"))
                console.CATPort = Int32.Parse(comboCATPort.Text.Substring(3));
            console.CATPTTRTS = chkCATPTT_RTS.Checked;
            console.CATPTTDTR = chkCATPTT_DTR.Checked;
            //console.PTTBitBangEnabled = chkCATPTTEnabled.Checked; 
            if (comboCATPTTPort.Text.StartsWith("COM"))
                console.CATPTTBitBangPort = Int32.Parse(comboCATPTTPort.Text.Substring(3));
            console.CATParity = SDRSerialPort.StringToParity((string)comboCATparity.SelectedItem);
            console.CATDataBits = int.Parse((string)comboCATdatabits.SelectedItem);
            console.CATStopBits = SDRSerialPort.StringToStopBits((string)comboCATstopbits.SelectedItem);
            console.CATEnabled = chkCATEnable.Checked;

            // make sure the enabled state of bitbang ptt is correct 
            if (chkCATPTT_RTS.Checked || chkCATPTT_DTR.Checked)
            {
                chkCATPTTEnabled.Enabled = true;
            }
            else
            {
                chkCATPTTEnabled.Enabled = false;
                chkCATPTTEnabled.Checked = false;
            }
        }

        // called in error cases to set the dialiog vars from 
        // the console properties -- sort of ugly, we should only have 1 copy 
        // of this stuff 
        public void copyCATPropsToDialogVars()
        {
            chkCATEnable.Checked = console.CATEnabled;
            string port = "COM" + console.CATPort.ToString();
            if (comboCATPort.Items.Contains(port))
                comboCATPort.Text = port;
            chkCATPTT_RTS.Checked = console.CATPTTRTS;
            chkCATPTT_DTR.Checked = console.CATPTTDTR;
            chkCATPTTEnabled.Checked = console.PTTBitBangEnabled;
            port = "COM" + console.CATPTTBitBangPort.ToString();
            if (comboCATPTTPort.Items.Contains(port))
                comboCATPTTPort.Text = port;

            // wjt fixme -- need to hand baudrate, parity, data, stop -- see initCATandPTTprops 
        }


        private void chkCATEnable_CheckedChanged(object sender, System.EventArgs e)
        {
            if (initializing) return;

            if (comboCATPort.Text == "" || !comboCATPort.Text.StartsWith("COM"))
            {
                if (chkCATEnable.Focused)
                {
                    if (chkCATEnable.Focused && chkCATEnable.Checked)
                    {
                        MessageBox.Show("The CAT port \"" + comboCATPort.Text + "\" is not a valid port.\n" +
                            "Please select another port.");
                    chkCATEnable.Checked = false;
                }
                }
                return;
            }

            // make sure we're not using the same comm port as the bit banger 
            if (chkCATEnable.Checked && console.PTTBitBangEnabled &&
                (comboCATPort.Text == comboCATPTTPort.Text))
            {
                MessageBox.Show("CAT port cannot be the same as Bit Bang Port", "Port Selection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                chkCATEnable.Checked = false;
            }

            // if enabled, disable changing of serial port 
            bool enable_sub_fields = !chkCATEnable.Checked;
            comboCATPort.Enabled = enable_sub_fields;

            enableCAT_HardwareFields(enable_sub_fields);

            if (chkCATEnable.Checked)
            {
                try
                {
                    console.CATEnabled = true;
                }
                catch (Exception ex)
                {
                    console.CATEnabled = false;
                    chkCATEnable.Checked = false;
                    MessageBox.Show("Could not initialize CAT control.  Exception was:\n\n " + ex.Message +
                        "\n\nCAT control has been disabled.", "Error Initializing CAT control",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (comboKeyerConnSecondary.Text == "CAT" && chkCATEnable.Focused)
                {
                    MessageBox.Show("The Secondary Keyer option has been changed to None since CAT has been disabled.",
                        "CAT Disabled",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    comboKeyerConnSecondary.Text = "None";
                }
                console.CATEnabled = false;
            }
        }

        private void enableCAT_HardwareFields(bool enable)
        {
            comboCATbaud.Enabled = enable;
            comboCATparity.Enabled = enable;
            comboCATdatabits.Enabled = enable;
            comboCATstopbits.Enabled = enable;
        }

        private void doEnablementOnBitBangEnable()
        {
            if (comboCATPTTPort.Text != "None" && (chkCATPTT_RTS.Checked || chkCATPTT_DTR.Checked))  // if RTS or DTR & port is not None, enable 
            {
                chkCATPTTEnabled.Enabled = true;
            }
            else
            {
                chkCATPTTEnabled.Enabled = false;
                chkCATPTTEnabled.Checked = false; // make sure it is not checked 
            }
        }

        private void chkCATPTT_RTS_CheckedChanged(object sender, System.EventArgs e)
        {
            console.CATPTTRTS = chkCATPTT_RTS.Checked;
            doEnablementOnBitBangEnable();
        }

        private void chkCATPTT_DTR_CheckedChanged(object sender, System.EventArgs e)
        {
            console.CATPTTDTR = chkCATPTT_DTR.Checked;
            doEnablementOnBitBangEnable();
        }

        private void chkCATPTTEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            if (initializing) return;

            bool enable_sub_fields;

            if (comboCATPTTPort.Text == "" || !comboCATPTTPort.Text.StartsWith("COM"))
            {
                if (chkCATPTTEnabled.Focused && chkCATPTTEnabled.Checked)
                {
                    MessageBox.Show("The PTT port \"" + comboCATPTTPort.Text + "\" is not a valid port.  Please select another port.");
                }
                    chkCATPTTEnabled.Checked = false;
                return;
            }

            if (chkCATPTTEnabled.Checked && console.CATEnabled &&
                comboCATPort.Text == comboCATPTTPort.Text)
            {
                if (chkCATPTTEnabled.Focused)
                {
                    MessageBox.Show("CAT port cannot be the same as Bit Bang Port", "Port Selection Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    chkCATPTTEnabled.Checked = false;
                }
                return;
            }

            console.PTTBitBangEnabled = chkCATPTTEnabled.Checked;
            if (chkCATPTTEnabled.Checked) // if it's enabled don't allow changing settings on port 
            {
                enable_sub_fields = false;
            }
            else
            {
                enable_sub_fields = true;
            }
            chkCATPTT_RTS.Enabled = enable_sub_fields;
            chkCATPTT_DTR.Enabled = enable_sub_fields;
            comboCATPTTPort.Enabled = enable_sub_fields;
        }

        private void comboCATparity_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string selection = comboCATparity.SelectedText;
            if (selection == null) return;

            console.CATParity = SDRSerialPort.StringToParity(selection);
        }

        private void comboCATPort_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboCATPort.Text == "None")
            {
                if (chkCATEnable.Checked)
                {
                    if (comboCATPort.Focused)
                        chkCATEnable.Checked = false;
                }

                chkCATEnable.Enabled = false;
            }
            else chkCATEnable.Enabled = true;

            if (comboCATPort.Text.StartsWith("COM"))
                console.CATPort = Int32.Parse(comboCATPort.Text.Substring(3));
        }

        private void comboCATPTTPort_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboCATPTTPort.Text == "None")
            {
                if (chkCATPTTEnabled.Checked)
                {
                    if (comboCATPTTPort.Focused)
                        chkCATPTTEnabled.Checked = false;
                }

                //chkCATEnable.Enabled = false;
                doEnablementOnBitBangEnable();
            }
            else
            {
                if (chkCATPTT_RTS.Checked || chkCATPTT_DTR.Checked)
                    //chkCATEnable.Enabled = true;
                    doEnablementOnBitBangEnable();
            }

            if (comboCATPTTPort.Text.StartsWith("COM"))
                console.CATPTTBitBangPort = Int32.Parse(comboCATPTTPort.Text.Substring(3));
            if (!comboCATPTTPort.Focused)
                chkCATPTTEnabled_CheckedChanged(sender, e);
        }

        private void comboCATbaud_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboCATbaud.SelectedIndex >= 0)
                console.CATBaudRate = Int32.Parse(comboCATbaud.Text);
        }

        private void comboCATdatabits_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboCATdatabits.SelectedIndex >= 0)
                console.CATDataBits = int.Parse(comboCATdatabits.Text);
        }

        private void comboCATstopbits_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboCATstopbits.SelectedIndex >= 0)
                console.CATStopBits = SDRSerialPort.StringToStopBits(comboCATstopbits.Text);
        }

        private void btnCATTest_Click(object sender, System.EventArgs e)
        {
            CATTester cat = new CATTester(console);
            //this.Close();
            cat.Show();
            cat.Focus();
        }

        //Modified 10/12/08 BT to change "SDR-1000" to "PowerSDR"
        private void comboCATRigType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (comboCATRigType.Text)
            {
                case "PowerSDR":
                    console.CATRigType = 900;
                    break;
                case "TS-2000":
                    console.CATRigType = 19;
                    break;
                case "TS-50S":
                    console.CATRigType = 13;
                    break;
                case "TS-440":
                    console.CATRigType = 20;
                    break;
                default:
                    console.CATRigType = 19;
                    break;
            }
        }

        #endregion

        #region Test Tab Event Handlers

        private void chkTestIMD_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chekTestIMD.Checked)
            {
                if (!console.PowerOn)
                {
                    MessageBox.Show("Power must be on to run this test.",
                        "Power is off",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Hand);
                    chekTestIMD.Checked = false;
                    return;
                }
                console.PreviousPWR = console.PWR;
                console.PWR = (int)udTestIMDPower.Value;
                console.MOX = true;

                if (!console.MOX)
                {
                    chekTestIMD.Checked = false;
                    return;
                }

                Audio.MOX = true;
                chekTestIMD.BackColor = console.ButtonSelectedColor;
                Audio.SineFreq1 = (double)udTestIMDFreq1.Value;
                Audio.SineFreq2 = (double)udTestIMDFreq2.Value;
                Audio.two_tone = true;
                Audio.TXInputSignal = Audio.SignalSource.SINE_TWO_TONE;
                Audio.SourceScale = Math.Pow(10.0, (double)udTwoToneLevel.Value / 20.0);
            }
            else
            {
                udTestIMDPower.Value = console.PWR;
                console.PWR = console.PreviousPWR;
                Audio.TXInputSignal = Audio.SignalSource.RADIO;
                Audio.MOX = false;
                console.MOX = false;
                Audio.SineFreq1 = (double)udDSPCWPitch.Value;
                Audio.two_tone = false;
                chekTestIMD.BackColor = SystemColors.Control;
            }
        }

        private void chkTestX2_CheckedChanged(object sender, System.EventArgs e)
        {
            byte val = 0;
            if (chkTestX2Pin1.Checked) val |= 1 << 0;
            if (chkTestX2Pin2.Checked) val |= 1 << 1;
            if (chkTestX2Pin3.Checked) val |= 1 << 2;
            if (chkTestX2Pin4.Checked) val |= 1 << 3;
            if (chkTestX2Pin5.Checked) val |= 1 << 4;
            if (chkTestX2Pin6.Checked) val |= 1 << 5;

            console.Hdw.X2 = val;
        }

        private void btnTestAudioBalStart_Click(object sender, System.EventArgs e)
        {
            if (!console.PowerOn)
            {
                MessageBox.Show("Power must be on to run this test.",
                    "Power is off",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
                return;
            }

            DialogResult dr = DialogResult.No;
            Audio.two_tone = false;
            Audio.SineFreq1 = 600.0;

            do
            {
                Audio.RX1OutputSignal = Audio.SignalSource.SINE_LEFT_ONLY;
                dr = MessageBox.Show("Do you hear a tone in the left channel?",
                    "Tone in left channel?",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                Audio.RX1OutputSignal = Audio.SignalSource.RADIO;

                if (dr == DialogResult.No)
                {
                    DialogResult dr2 = MessageBox.Show("Please double check cable and speaker connections.\n" +
                        "Click OK to try again (cancel to abort).",
                        "Check connections",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Asterisk);
                    if (dr2 == DialogResult.Cancel)
                    {
                        MessageBox.Show("Test Failed",
                            "Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
                        btnTestAudioBalStart.BackColor = Color.Red;
                        return;
                    }
                }
                else if (dr == DialogResult.Cancel)
                {
                    MessageBox.Show("Test Failed",
                        "Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    btnTestAudioBalStart.BackColor = Color.Red;
                    return;
                }
            } while (dr != DialogResult.Yes);

            do
            {
                Audio.RX1OutputSignal = Audio.SignalSource.SINE_RIGHT_ONLY;
                dr = MessageBox.Show("Do you hear a tone in the right channel?",
                    "Tone in right channel?",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                Audio.RX1OutputSignal = Audio.SignalSource.RADIO;

                switch (dr)
                {
                    case DialogResult.No:
                        {
                            DialogResult dr2 = MessageBox.Show("Please double check cable and speaker connections.\n" +
                                                               "Click OK to try again (cancel to abort).",
                                                               "Check connections",
                                                               MessageBoxButtons.OKCancel,
                                                               MessageBoxIcon.Asterisk);
                            if (dr2 == DialogResult.Cancel)
                            {
                                MessageBox.Show("Test Failed",
                                                "Failed",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Stop);
                                btnTestAudioBalStart.BackColor = Color.Red;
                                return;
                            }
                        }
                        break;
                    case DialogResult.Cancel:
                        MessageBox.Show("Test Failed",
                                        "Failed",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Stop);
                        btnTestAudioBalStart.BackColor = Color.Red;
                        return;
                }
            } while (dr != DialogResult.Yes);

            MessageBox.Show("Test was successful.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            btnTestAudioBalStart.BackColor = Color.Green;
        }

        private void timer_sweep_Tick(object sender, System.EventArgs e)
        {
            if (tkbarTestGenFreq.Value >= udTestGenHigh.Value)
            {
                timer_sweep.Enabled = false;
                btnTestGenSweep.BackColor = SystemColors.Control;
            }
            else
            {
                tkbarTestGenFreq.Value += (int)(udTestGenHzSec.Value / 10);
                tkbarTestGenFreq_Scroll(this, EventArgs.Empty);
            }
        }

        private void buttonTestGenSweep_Click(object sender, System.EventArgs e)
        {
            if (timer_sweep.Enabled)
            {
                timer_sweep.Enabled = false;
                btnTestGenSweep.BackColor = SystemColors.Control;
            }
            else
            {
                btnTestGenSweep.BackColor = console.ButtonSelectedColor;
                tkbarTestGenFreq.Value = (int)udTestGenLow.Value;
                timer_sweep.Enabled = true;
            }
        }

        private void tkbarTestGenFreq_Scroll(object sender, System.EventArgs e)
        {
            udTestFreq.Value = tkbarTestGenFreq.Value;
            //Audio.SineFreq1 = tkbarTestGenFreq.Value;
        }

        private void udTestFreq_ValueChanged(object sender, EventArgs e)
        {
            Audio.SineFreq1 = (double)udTestFreq.Value;
            if (tkbarTestGenFreq != null && tkbarTestGenFreq.Value != (int)udTestFreq.Value)
                tkbarTestGenFreq.Value = (int)udTestFreq.Value;

        }

        private void cmboSigGenRXMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cmboSigGenRXMode.SelectedIndex < 0) return;

            Audio.SignalSource source = Audio.SignalSource.RADIO;

            switch (cmboSigGenRXMode.Text)
            {
                case "Radio":
                    source = Audio.SignalSource.RADIO;
                    break;
                case "Tone":
                    source = Audio.SignalSource.SINE;
                    break;
                case "Noise":
                    source = Audio.SignalSource.NOISE;
                    break;
                case "Triangle":
                    source = Audio.SignalSource.TRIANGLE;
                    break;
                case "Sawtooth":
                    source = Audio.SignalSource.SAWTOOTH;
                    break;
                case "Pulse":
                    source = Audio.SignalSource.PULSE;
                    break;
                case "Silence":
                    source = Audio.SignalSource.SILENCE;
                    break;
            }

            if (chkSigGenRX2.Checked)
            {
                if (radSigGenRXInput.Checked)
                    Audio.RX2InputSignal = source;
                else Audio.RX2OutputSignal = source;
            }
            else
            {
                if (radSigGenRXInput.Checked)
                    Audio.RX1InputSignal = source;
                else Audio.RX1OutputSignal = source;
            }

            UpdateSigGenScaleVisible();
            UpdateSigGenPulseVisible();
        }

        private void radSigGenRXInput_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radSigGenRXInput.Checked)
            {
                Audio.RX1OutputSignal = Audio.SignalSource.RADIO;
                Audio.RX2OutputSignal = Audio.SignalSource.RADIO;
                cmboSigGenRXMode_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        private void radSigGenRXOutput_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radSigGenRXOutput.Checked)
            {
                Audio.RX1InputSignal = Audio.SignalSource.RADIO;
                Audio.RX2InputSignal = Audio.SignalSource.RADIO;
                cmboSigGenRXMode_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        private void chkSigGenRX2_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkSigGenRX2.Checked) Audio.RX1InputSignal = Audio.RX1OutputSignal = Audio.SignalSource.RADIO;
            else Audio.RX2InputSignal = Audio.RX2OutputSignal = Audio.SignalSource.RADIO;
            cmboSigGenRXMode_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void UpdateSigGenScaleVisible()
        {
            bool b = false;
            switch (cmboSigGenRXMode.Text)
            {
                case "Tone": b = true; break;
                case "Pulse": b = true; break;
            }

            switch (cmboSigGenTXMode.Text)
            {
                case "Tone": b = true; break;
                case "Pulse": b = true; break;
            }

            lblTestGenScale.Visible = b;
            udTestGenScale.Visible = b;
            Audio.SineFreq1 = (double)udTestFreq.Value;
        }

        private void UpdateSigGenPulseVisible()
        {
            bool b = false;
            switch (cmboSigGenRXMode.Text)
            {
                case "Pulse": b = true; break;
            }

            switch (cmboSigGenTXMode.Text)
            {
                case "Pulse": b = true; break;
            }

            lblPulseDuty.Visible = b;
            udPulseDuty.Visible = b;
            lblPulsePeriod.Visible = b;
            udPulsePeriod.Visible = b;
        }



        private void cmboSigGenTXMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cmboSigGenTXMode.SelectedIndex < 0) return;

            Audio.SignalSource source = Audio.SignalSource.RADIO;

            switch (cmboSigGenTXMode.Text)
            {
                case "Radio":
                    source = Audio.SignalSource.RADIO;
                    break;
                case "Tone":
                    source = Audio.SignalSource.SINE;
                    break;
                case "Noise":
                    source = Audio.SignalSource.NOISE;
                    break;
                case "Triangle":
                    source = Audio.SignalSource.TRIANGLE;
                    break;
                case "Sawtooth":
                    source = Audio.SignalSource.SAWTOOTH;
                    break;
                case "Pulse":
                    source = Audio.SignalSource.PULSE;
                    break;
                case "Silence":
                    source = Audio.SignalSource.SILENCE;
                    break;
            }

            if (radSigGenTXInput.Checked)
                Audio.TXInputSignal = source;
            else Audio.TXOutputSignal = source;

            UpdateSigGenScaleVisible();
            UpdateSigGenPulseVisible();
        }

        private void radSigGenTXInput_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radSigGenTXInput.Checked)
            {
                Audio.TXOutputSignal = Audio.SignalSource.RADIO;
                cmboSigGenTXMode_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        private void radSigGenTXOutput_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radSigGenTXOutput.Checked)
            {
                Audio.TXInputSignal = Audio.SignalSource.RADIO;
                cmboSigGenTXMode_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        private void updnTestGenScale_ValueChanged(object sender, System.EventArgs e)
        {
            Audio.SourceScale = Math.Pow(10.0, (double)udTestGenScale.Value / 20.0);
        }

        private void btnImpulse_Click(object sender, System.EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(ImpulseFunction));
            t.Name = "Impulse";
            t.Priority = ThreadPriority.Highest;
            t.IsBackground = true;
            t.Start();
        }

        private void ImpulseFunction()
        {
            console.Hdw.ImpulseEnable = true;
            Thread.Sleep(500);
            for (int i = 0; i < (int)udImpulseNum.Value; i++)
            {
                console.Hdw.Impulse();
                Thread.Sleep(45);
            }
            Thread.Sleep(500);
            console.Hdw.ImpulseEnable = false;
        }

        #endregion

        #region Other Event Handlers
        // ======================================================
        // Display Tab Event Handlers
        // ======================================================

        private void btnWizard_Click(object sender, System.EventArgs e)
        {
            SetupWizard w = new SetupWizard(console, comboAudioSoundCard.SelectedIndex);
            w.Show();
            w.Focus();
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            if (saving)
            {
                this.Hide();
            }//w5wc
            else
            {
                Thread t = new Thread(new ThreadStart(SaveOptions));
                t.Name = "Save Options Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Lowest;
                t.Start();
                this.Hide();
            }

        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(GetOptions));
            t.Name = "Save Options Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Lowest;
            t.Start();
            this.Hide();
        }

        private void btnApply_Click(object sender, System.EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(ApplyOptions));
            t.Name = "Save Options Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private void ApplyOptions()
        {
            if (saving) return;
            SaveOptions();
            DB.Update();
        }

        private void udGeneralLPTDelay_ValueChanged(object sender, System.EventArgs e)
        {
            console.LatchDelay = (int)udGeneralLPTDelay.Value;
        }

        private void Setup_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void btnImportDB_Click(object sender, System.EventArgs e)
        {
            string path = console.AppDataPath;
            path = path.Substring(0, path.LastIndexOf("\\"));
            openFileDialog1.InitialDirectory = path;
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CompleteImport();
        }

        private void CompleteImport()
        {
            if (DB.ImportDatabase(openFileDialog1.FileName))
                MessageBox.Show("Database Imported Successfully");

            GetTxProfiles();
            console.UpdateTXProfile(TXProfile);

            GetOptions();					// load all database values
            console.GetState();
            if (console.EQForm != null) Common.RestoreForm(console.EQForm, "EQForm", false);
            if (console.XVTRForm != null) Common.RestoreForm(console.XVTRForm, "XVTR", false);
            // if (console.ProdTestForm != null) Common.RestoreForm(console.ProdTestForm, "ProdTest", false);

            SaveOptions();					// save all database values
            console.SaveState();
            if (console.EQForm != null) Common.SaveForm(console.EQForm, "EQForm");
            if (console.XVTRForm != null) Common.SaveForm(console.XVTRForm, "XVTR");
            // if (console.ProdTestForm != null) Common.SaveForm(console.ProdTestForm, "ProdTest");

            udTransmitTunePower_ValueChanged(this, EventArgs.Empty);
            //console.ResetMemForm();
        }

        #endregion

        private bool shift_key = false;
        private bool ctrl_key = false;
        private bool alt_key = false;
        private bool windows_key = false;
        private bool menu_key = false;

        private void txtKB_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Debug.WriteLine("KeyCode: " + e.KeyCode + " KeyData: " + e.KeyData + " KeyValue: " + e.KeyValue);
            shift_key = e.Shift;
            ctrl_key = e.Control;
            alt_key = e.Alt;

            if (e.KeyCode == Keys.LWin ||
                e.KeyCode == Keys.RWin)
                windows_key = true;

            if (e.KeyCode == Keys.Apps)
                menu_key = true;

            TextBoxTS txtbox = (TextBoxTS)sender;

            string s = "";

            if (ctrl_key) s += "Ctrl+";
            if (alt_key) s += "Alt+";
            if (shift_key) s += "Shift+";
            if (windows_key)
                s += "Win+";
            if (menu_key)
                s += "Menu+";

            if (e.KeyCode != Keys.ShiftKey &&
                e.KeyCode != Keys.ControlKey &&
                e.KeyCode != Keys.Menu &&
                e.KeyCode != Keys.RMenu &&
                e.KeyCode != Keys.LWin &&
                e.KeyCode != Keys.RWin &&
                e.KeyCode != Keys.Apps)
                s += KeyToString(e.KeyCode);

            txtbox.Text = s;
            e.Handled = true;
        }

        private void txtKB_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtKB_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //Debug.WriteLine("KeyUp: "+e.KeyCode.ToString());
            shift_key = e.Shift;
            ctrl_key = e.Control;
            alt_key = e.Alt;

            if (e.KeyCode == Keys.LWin ||
                e.KeyCode == Keys.RWin)
                windows_key = false;

            if (e.KeyCode == Keys.Apps)
                menu_key = false;


            TextBoxTS txtbox = (TextBoxTS)sender;

            if (txtbox.Text.EndsWith("+"))
            {
                if (shift_key || ctrl_key || alt_key ||
                    windows_key || menu_key)
                {
                    string s = "";

                    if (ctrl_key) s += "Ctrl+";
                    if (alt_key) s += "Alt+";
                    if (shift_key) s += "Shift+";
                    if (windows_key)
                        s += "Win+";
                    if (menu_key)
                        s += "Menu+";

                    txtbox.Text = s;
                }
                else
                    txtbox.Text = "Not Assigned";
            }
        }

        private void clrbtnTXFilter_Changed(object sender, System.EventArgs e)
        {
            Display.DisplayFilterTXColor = clrbtnTXFilter.Color;
        }

        #region Lost Focus Event Handlers

        private void udGeneralCalFreq1_LostFocus(object sender, EventArgs e)
        {
            udGeneralCalFreq1.Value = udGeneralCalFreq1.Value;
        }

        private void udSoftRockCenterFreq_LostFocus(object sender, EventArgs e)
        {
            udSoftRockCenterFreq.Value = udSoftRockCenterFreq.Value;
        }

        private void udDDSCorrection_LostFocus(object sender, EventArgs e)
        {
            udDDSCorrection.Value = udDDSCorrection.Value;
        }

        private void udDDSIFFreq_LostFocus(object sender, EventArgs e)
        {
            udDDSIFFreq.Value = udDDSIFFreq.Value;
        }

        private void udDDSPLLMult_LostFocus(object sender, EventArgs e)
        {
            udDDSPLLMult.Value = udDDSPLLMult.Value;
        }

        private void udGeneralLPTDelay_LostFocus(object sender, EventArgs e)
        {
            udGeneralLPTDelay.Value = udGeneralLPTDelay.Value;
        }

        private void udOptClickTuneOffsetDIGL_LostFocus(object sender, EventArgs e)
        {
            udOptClickTuneOffsetDIGL.Value = udOptClickTuneOffsetDIGL.Value;
        }

        private void udOptClickTuneOffsetDIGU_LostFocus(object sender, EventArgs e)
        {
            udOptClickTuneOffsetDIGU.Value = udOptClickTuneOffsetDIGU.Value;
        }

        private void udGeneralX2Delay_LostFocus(object sender, EventArgs e)
        {
            udGeneralX2Delay.Value = udGeneralX2Delay.Value;
        }

        private void udGeneralCalFreq3_LostFocus(object sender, EventArgs e)
        {
            udGeneralCalFreq3.Value = udGeneralCalFreq3.Value;
        }

        private void udGeneralCalLevel_LostFocus(object sender, EventArgs e)
        {
            udGeneralCalLevel.Value = udGeneralCalLevel.Value;
        }

        private void udGeneralCalFreq2_LostFocus(object sender, EventArgs e)
        {
            udGeneralCalFreq2.Value = udGeneralCalFreq2.Value;
        }

        private void udFilterDefaultLowCut_LostFocus(object sender, EventArgs e)
        {
            udFilterDefaultLowCut.Value = udFilterDefaultLowCut.Value;
        }

        private void udOptMaxFilterShift_LostFocus(object sender, EventArgs e)
        {
            udOptMaxFilterShift.Value = udOptMaxFilterShift.Value;
        }

        private void udOptMaxFilterWidth_LostFocus(object sender, EventArgs e)
        {
            udOptMaxFilterWidth.Value = udOptMaxFilterWidth.Value;
        }

        private void udAudioMicGain1_LostFocus(object sender, EventArgs e)
        {
            udAudioMicGain1.Value = udAudioMicGain1.Value;
        }

        private void udAudioLineIn1_LostFocus(object sender, EventArgs e)
        {
            udAudioLineIn1.Value = udAudioLineIn1.Value;
        }

        private void udAudioVoltage1_LostFocus(object sender, EventArgs e)
        {
            udAudioVoltage1.Value = udAudioVoltage1.Value;
        }

        private void udAudioLatency1_LostFocus(object sender, EventArgs e)
        {
            udAudioLatency1.Value = udAudioLatency1.Value;
        }

        private void udAudioVACGainTX_LostFocus(object sender, EventArgs e)
        {
            udAudioVACGainTX.Value = udAudioVACGainTX.Value;
        }

        private void udAudioVACGainRX_LostFocus(object sender, EventArgs e)
        {
            udAudioVACGainRX.Value = udAudioVACGainRX.Value;
        }

        private void udAudioLatency2_LostFocus(object sender, EventArgs e)
        {
            udAudioLatency2.Value = udAudioLatency2.Value;
        }

        private void udDisplayScopeTime_LostFocus(object sender, EventArgs e)
        {
            udDisplayScopeTime.Value = udDisplayScopeTime.Value;
        }

        private void udDisplayMeterAvg_LostFocus(object sender, EventArgs e)
        {
            udDisplayMeterAvg.Value = udDisplayMeterAvg.Value;
        }

        private void udDisplayMultiTextHoldTime_LostFocus(object sender, EventArgs e)
        {
            udDisplayMultiTextHoldTime.Value = udDisplayMultiTextHoldTime.Value;
        }

        private void udDisplayMultiPeakHoldTime_LostFocus(object sender, EventArgs e)
        {
            udDisplayMultiPeakHoldTime.Value = udDisplayMultiPeakHoldTime.Value;
        }

        private void udDisplayWaterfallLowLevel_LostFocus(object sender, EventArgs e)
        {
            udDisplayWaterfallLowLevel.Value = udDisplayWaterfallLowLevel.Value;
        }

        private void udDisplayWaterfallHighLevel_LostFocus(object sender, EventArgs e)
        {
            udDisplayWaterfallHighLevel.Value = udDisplayWaterfallHighLevel.Value;
        }

        private void udDisplayCPUMeter_LostFocus(object sender, EventArgs e)
        {
            udDisplayCPUMeter.Value = udDisplayCPUMeter.Value;
        }

        private void udDisplayPeakText_LostFocus(object sender, EventArgs e)
        {
            udDisplayPeakText.Value = udDisplayPeakText.Value;
        }

        private void udDisplayMeterDelay_LostFocus(object sender, EventArgs e)
        {
            udDisplayMeterDelay.Value = udDisplayMeterDelay.Value;
        }

        private void udDisplayFPS_LostFocus(object sender, EventArgs e)
        {
            udDisplayFPS.Value = udDisplayFPS.Value;
        }

        private void udDisplayAVGTime_LostFocus(object sender, EventArgs e)
        {
            udDisplayAVGTime.Value = udDisplayAVGTime.Value;
        }

        private void udDisplayPhasePts_LostFocus(object sender, EventArgs e)
        {
            udDisplayPhasePts.Value = udDisplayPhasePts.Value;
        }

        private void udDisplayGridStep_LostFocus(object sender, EventArgs e)
        {
            udDisplayGridStep.Value = udDisplayGridStep.Value;
        }

        private void udTXGridMin_LostFocus(object sender, EventArgs e)
        {
            udTXGridMin.Value = udTXGridMin.Value;
        }

        private void udTXGridStep_LostFocus(object sender, EventArgs e)
        {
            udTXGridStep.Value = udTXGridStep.Value;
        }

        private void udDisplayGridMin_LostFocus(object sender, EventArgs e)
        {
            udDisplayGridMin.Value = udDisplayGridMin.Value;
        }

        private void udDSPNB_LostFocus(object sender, EventArgs e)
        {
            udDSPNB.Value = udDSPNB.Value;
        }

        private void udLMSNRgain_LostFocus(object sender, EventArgs e)
        {
            udLMSNRgain.Value = udLMSNRgain.Value;
        }

        private void udLMSNRdelay_LostFocus(object sender, EventArgs e)
        {
            udLMSNRdelay.Value = udLMSNRdelay.Value;
        }

        private void udLMSNRtaps_LostFocus(object sender, EventArgs e)
        {
            udLMSNRtaps.Value = udLMSNRtaps.Value;
        }

        private void udLMSANFgain_LostFocus(object sender, EventArgs e)
        {
            udLMSANFgain.Value = udLMSANFgain.Value;
        }

        private void udLMSANFdelay_LostFocus(object sender, EventArgs e)
        {
            udLMSANFdelay.Value = udLMSANFdelay.Value;
        }

        private void udLMSANFtaps_LostFocus(object sender, EventArgs e)
        {
            udLMSANFtaps.Value = udLMSANFtaps.Value;
        }

        private void udDSPNB2_LostFocus(object sender, EventArgs e)
        {
            udDSPNB2.Value = udDSPNB2.Value;
        }

        private void udDSPImageGainTX_LostFocus(object sender, EventArgs e)
        {
            udDSPImageGainTX.Value = udDSPImageGainTX.Value;
        }

        private void udDSPImagePhaseTX_LostFocus(object sender, EventArgs e)
        {
            udDSPImagePhaseTX.Value = udDSPImagePhaseTX.Value;
        }

        private void udDSPCWPitch_LostFocus(object sender, EventArgs e)
        {
            udDSPCWPitch.Value = udDSPCWPitch.Value;
        }

        private void udCWKeyerWeight_LostFocus(object sender, EventArgs e)
        {
            udCWKeyerWeight.Value = udCWKeyerWeight.Value;
        }

        private void udCWKeyerRamp_LostFocus(object sender, EventArgs e)
        {
            udCWKeyerRamp.Value = udCWKeyerRamp.Value;
        }

        private void udCWBreakInDelay_LostFocus(object sender, EventArgs e)
        {
            udCWBreakInDelay.Value = udCWBreakInDelay.Value;
        }

        private void udDSPLevelerHangTime_LostFocus(object sender, EventArgs e)
        {
            udDSPLevelerHangTime.Value = udDSPLevelerHangTime.Value;
        }

        private void udDSPLevelerThreshold_LostFocus(object sender, EventArgs e)
        {
            udDSPLevelerThreshold.Value = udDSPLevelerThreshold.Value;
        }

        private void udDSPLevelerSlope_LostFocus(object sender, EventArgs e)
        {
            udDSPLevelerSlope.Value = udDSPLevelerSlope.Value;
        }

        private void udDSPLevelerDecay_LostFocus(object sender, EventArgs e)
        {
            udDSPLevelerDecay.Value = udDSPLevelerDecay.Value;
        }

        private void udDSPLevelerAttack_LostFocus(object sender, EventArgs e)
        {
            udDSPLevelerAttack.Value = udDSPLevelerAttack.Value;
        }

        private void udDSPALCHangTime_LostFocus(object sender, EventArgs e)
        {
            udDSPALCHangTime.Value = udDSPALCHangTime.Value;
        }

        private void udDSPALCThreshold_LostFocus(object sender, EventArgs e)
        {
            udDSPALCThreshold.Value = udDSPALCThreshold.Value;
        }

        private void udDSPALCSlope_LostFocus(object sender, EventArgs e)
        {
            udDSPALCSlope.Value = udDSPALCSlope.Value;
        }

        private void udDSPALCDecay_LostFocus(object sender, EventArgs e)
        {
            udDSPALCDecay.Value = udDSPALCDecay.Value;
        }

        private void udDSPALCAttack_LostFocus(object sender, EventArgs e)
        {
            udDSPALCAttack.Value = udDSPALCAttack.Value;
        }

        private void udDSPAGCHangTime_LostFocus(object sender, EventArgs e)
        {
            udDSPAGCHangTime.Value = udDSPAGCHangTime.Value;
        }

        private void udDSPAGCMaxGaindB_LostFocus(object sender, EventArgs e)
        {
            udDSPAGCMaxGaindB.Value = udDSPAGCMaxGaindB.Value;
        }

        private void udDSPAGCSlope_LostFocus(object sender, EventArgs e)
        {
            udDSPAGCSlope.Value = udDSPAGCSlope.Value;
        }

        private void udDSPAGCDecay_LostFocus(object sender, EventArgs e)
        {
            udDSPAGCDecay.Value = udDSPAGCDecay.Value;
        }

        private void udDSPAGCAttack_LostFocus(object sender, EventArgs e)
        {
            udDSPAGCAttack.Value = udDSPAGCAttack.Value;
        }

        private void udDSPAGCFixedGaindB_LostFocus(object sender, EventArgs e)
        {
            udDSPAGCFixedGaindB.Value = udDSPAGCFixedGaindB.Value;
        }

        private void udTXAMCarrierLevel_LostFocus(object sender, EventArgs e)
        {
            udTXAMCarrierLevel.Value = udTXAMCarrierLevel.Value;
        }

        private void udTXAF_LostFocus(object sender, EventArgs e)
        {
            udTXAF.Value = udTXAF.Value;
        }

        private void udTXVOXHangTime_LostFocus(object sender, EventArgs e)
        {
            udTXVOXHangTime.Value = udTXVOXHangTime.Value;
        }

        private void udTXVOXThreshold_LostFocus(object sender, EventArgs e)
        {
            udTXVOXThreshold.Value = udTXVOXThreshold.Value;
        }

        private void udTXNoiseGate_LostFocus(object sender, EventArgs e)
        {
            udTXNoiseGate.Value = udTXNoiseGate.Value;
        }

        private void udTXTunePower_LostFocus(object sender, EventArgs e)
        {
            udTXTunePower.Value = udTXTunePower.Value;
        }

        private void udTXFilterLow_LostFocus(object sender, EventArgs e)
        {
            udTXFilterLow.Value = udTXFilterLow.Value;
        }

        private void udTXFilterHigh_LostFocus(object sender, EventArgs e)
        {
            udTXFilterHigh.Value = udTXFilterHigh.Value;
        }

        private void udMicGainMax_LostFocus(object sender, EventArgs e)
        {
            udMicGainMax.Value = udMicGainMax.Value;
        }

        private void udMicGainMin_LostFocus(object sender, EventArgs e)
        {
            udMicGainMin.Value = udMicGainMin.Value;
        }

        private void udPAADC17_LostFocus(object sender, EventArgs e)
        {
            udPAADC17.Value = udPAADC17.Value;
        }

        private void udPAADC15_LostFocus(object sender, EventArgs e)
        {
            udPAADC15.Value = udPAADC15.Value;
        }

        private void udPAADC20_LostFocus(object sender, EventArgs e)
        {
            udPAADC20.Value = udPAADC20.Value;
        }

        private void udPAADC12_LostFocus(object sender, EventArgs e)
        {
            udPAADC12.Value = udPAADC12.Value;
        }

        private void udPAADC10_LostFocus(object sender, EventArgs e)
        {
            udPAADC10.Value = udPAADC10.Value;
        }

        private void udPAADC160_LostFocus(object sender, EventArgs e)
        {
            udPAADC160.Value = udPAADC160.Value;
        }

        private void udPAADC80_LostFocus(object sender, EventArgs e)
        {
            udPAADC80.Value = udPAADC80.Value;
        }

        private void udPAADC60_LostFocus(object sender, EventArgs e)
        {
            udPAADC60.Value = udPAADC60.Value;
        }

        private void udPAADC40_LostFocus(object sender, EventArgs e)
        {
            udPAADC40.Value = udPAADC40.Value;
        }

        private void udPAADC30_LostFocus(object sender, EventArgs e)
        {
            udPAADC30.Value = udPAADC30.Value;
        }

        private void udPAGain10_LostFocus(object sender, EventArgs e)
        {
            udPAGain10.Value = udPAGain10.Value;
        }

        private void udPAGain12_LostFocus(object sender, EventArgs e)
        {
            udPAGain12.Value = udPAGain12.Value;
        }

        private void udPAGain15_LostFocus(object sender, EventArgs e)
        {
            udPAGain15.Value = udPAGain15.Value;
        }

        private void udPAGain17_LostFocus(object sender, EventArgs e)
        {
            udPAGain17.Value = udPAGain17.Value;
        }

        private void udPAGain20_LostFocus(object sender, EventArgs e)
        {
            udPAGain20.Value = udPAGain20.Value;
        }

        private void udPAGain30_LostFocus(object sender, EventArgs e)
        {
            udPAGain30.Value = udPAGain30.Value;
        }

        private void udPAGain40_LostFocus(object sender, EventArgs e)
        {
            udPAGain40.Value = udPAGain40.Value;
        }

        private void udPAGain60_LostFocus(object sender, EventArgs e)
        {
            udPAGain60.Value = udPAGain60.Value;
        }

        private void udPAGain80_LostFocus(object sender, EventArgs e)
        {
            udPAGain80.Value = udPAGain80.Value;
        }

        private void udPAGain160_LostFocus(object sender, EventArgs e)
        {
            udPAGain160.Value = udPAGain160.Value;
        }

        private void udPACalPower_LostFocus(object sender, EventArgs e)
        {
            udPACalPower.Value = udPACalPower.Value;
        }

        private void udDisplayLineWidth_LostFocus(object sender, EventArgs e)
        {
            udDisplayLineWidth.Value = udDisplayLineWidth.Value;
        }

        private void udTXLineWidth_LostFocus(object sender, EventArgs e)
        {
            udTXLineWidth.Value = udTXLineWidth.Value;
        }

        private void udTestGenScale_LostFocus(object sender, EventArgs e)
        {
            udTestGenScale.Value = udTestGenScale.Value;
        }

        private void udTestGenHzSec_LostFocus(object sender, EventArgs e)
        {
            udTestGenHzSec.Value = udTestGenHzSec.Value;
        }

        private void udTestGenHigh_LostFocus(object sender, EventArgs e)
        {
            udTestGenHigh.Value = udTestGenHigh.Value;
        }

        private void udTestGenLow_LostFocus(object sender, EventArgs e)
        {
            udTestGenLow.Value = udTestGenLow.Value;
        }

        private void udTestIMDFreq2_LostFocus(object sender, EventArgs e)
        {
            udTestIMDFreq2.Value = udTestIMDFreq2.Value;
        }

        private void udTestIMDPower_LostFocus(object sender, EventArgs e)
        {
            udTestIMDPower.Value = udTestIMDPower.Value;
        }

        private void udTestIMDFreq1_LostFocus(object sender, EventArgs e)
        {
            udTestIMDFreq1.Value = udTestIMDFreq1.Value;
        }

        private void udImpulseNum_LostFocus(object sender, EventArgs e)
        {
            udImpulseNum.Value = udImpulseNum.Value;
        }

        #endregion

        private void chkShowFreqOffset_CheckedChanged(object sender, System.EventArgs e)
        {
            Display.ShowFreqOffset = chkShowFreqOffset.Checked;
        }

        private void clrbtnBandEdge_Changed(object sender, System.EventArgs e)
        {
            Display.BandEdgeColor = clrbtnBandEdge.Color;
        }

        private void clrbtnTXBandEdge_Changed(object sender, System.EventArgs e)
        {
            Display.TXBandEdgeColor = clrbtnTXBandEdge.Color;
        }

        private void comboMeterType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboMeterType.Text == "") return;
            switch (comboMeterType.Text)
            {
                case "Original":
                    console.CurrentMeterDisplayMode = MultiMeterDisplayMode.Original;
                    break;
                case "Edge":
                    console.CurrentMeterDisplayMode = MultiMeterDisplayMode.Edge;
                    break;
                case "Analog":
                    console.CurrentMeterDisplayMode = MultiMeterDisplayMode.Analog;
                    break;
            }
        }

        private void clrbtnMeterEdgeLow_Changed(object sender, System.EventArgs e)
        {
            console.EdgeLowColor = clrbtnMeterEdgeLow.Color;
        }

        private void clrbtnMeterEdgeHigh_Changed(object sender, System.EventArgs e)
        {
            console.EdgeHighColor = clrbtnMeterEdgeHigh.Color;
        }

        private void clrbtnMeterEdgeBackground_Changed(object sender, System.EventArgs e)
        {
            console.EdgeMeterBackgroundColor = Color.FromArgb(tbMeterEdgeBackgroundAlpha.Value, clrbtnMeterEdgeBackground.Color);
            // console.EdgeMeterBackgroundColor = clrbtnMeterEdgeBackground.Color;
        }

        private void clrbtnEdgeIndicator_Changed(object sender, System.EventArgs e)
        {
            console.EdgeAVGColor = clrbtnEdgeIndicator.Color;
        }

        private void clrbtnMeterDigText_Changed(object sender, System.EventArgs e)
        {
            console.MeterDigitalTextColor = clrbtnMeterDigText.Color;
        }

        private void clrbtnMeterDigBackground_Changed(object sender, System.EventArgs e)
        {
            console.MeterDigitalBackgroundColor = clrbtnMeterDigBackground.Color;
        }

        private void clrbtnSubRXFilter_Changed(object sender, System.EventArgs e)
        {
            Display.SubRXFilterColor = Color.FromArgb(tbMultiRXFilterAlpha.Value, clrbtnSubRXFilter.Color);
        }

        private void clrbtnSubRXZero_Changed(object sender, System.EventArgs e)
        {
            Display.SubRXZeroLine = clrbtnSubRXZero.Color;
        }

        private void chkCWKeyerMode_CheckedChanged(object sender, System.EventArgs e)
        {
           // console.Keyer.KeyerMode = chkCWKeyerMode.Checked ? 1 : 0;
            if (chkCWKeyerMode.Checked)
            {
                if (chkModeBStrict.Checked)
                    CWKeyer.CurrentIambicMode = CWKeyer.IambicMode.ModeBStrict;
                else
                    CWKeyer.CurrentIambicMode = CWKeyer.IambicMode.ModeB;
            }
            else CWKeyer.CurrentIambicMode = CWKeyer.IambicMode.ModeA;
        }

        private void chkDisableToolTips_CheckedChanged(object sender, System.EventArgs e)
        {
            toolTip1.Active = !chkDisableToolTips.Checked;
            console.DisableToolTips = chkDisableToolTips.Checked;
        }

        private void comboColorPalette_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboColorPalette.Text == "original")
            {
                console.color_sheme = ColorSheme.original;
                clrbtnWaterfallLow.Visible = true;
                clrbtnWaterfallHigh.Visible = true;
                clrbtnWaterfallMid.Visible = true;
                lblDisplayWaterfallHighColor.Visible = true;
                lblDisplayWaterfallLowColor.Visible = true;
                lblDisplayWaterfallMidColor.Visible = true;
            }
            if (comboColorPalette.Text == "enhanced")
            {
                console.color_sheme = ColorSheme.enhanced;
                clrbtnWaterfallLow.Visible = true;
                clrbtnWaterfallHigh.Visible = false;
                clrbtnWaterfallMid.Visible = false;
                lblDisplayWaterfallHighColor.Visible = false;
                lblDisplayWaterfallLowColor.Visible = true;
                lblDisplayWaterfallMidColor.Visible = false;
            }
            if (comboColorPalette.Text == "Spectran")
            {
                clrbtnWaterfallLow.Visible = false;
                console.color_sheme = ColorSheme.SPECTRAN;
                clrbtnWaterfallHigh.Visible = false;
                clrbtnWaterfallMid.Visible = false;
                lblDisplayWaterfallHighColor.Visible = false;
                lblDisplayWaterfallLowColor.Visible = false;
                lblDisplayWaterfallMidColor.Visible = false;
            }
            if (comboColorPalette.Text == "BlackWhite")
            {
                console.color_sheme = ColorSheme.BLACKWHITE;
                clrbtnWaterfallLow.Visible = false;
                clrbtnWaterfallHigh.Visible = false;
                clrbtnWaterfallMid.Visible = false;
                lblDisplayWaterfallHighColor.Visible = false;
                lblDisplayWaterfallLowColor.Visible = false;
                lblDisplayWaterfallMidColor.Visible = false;
            }

        }

        private void udDisplayWaterfallAvgTime_ValueChanged(object sender, System.EventArgs e)
        {
            double buffer_time = double.Parse(comboAudioBuffer1.Text) / (double)console.SampleRate1;
            int buffersToAvg = (int)((float)udDisplayWaterfallAvgTime.Value * 0.001 / buffer_time);
            buffersToAvg = Math.Max(2, buffersToAvg);
            Display.WaterfallAvgBlocks = buffersToAvg;
        }

        private void udDisplayWaterfallUpdatePeriod_ValueChanged(object sender, System.EventArgs e)
        {
            Display.WaterfallUpdatePeriod = (int)udDisplayWaterfallUpdatePeriod.Value;
        }

        private void chkSnapClickTune_CheckedChanged(object sender, System.EventArgs e)
        {
            console.SnapToClickTuning = chkSnapClickTune.Checked;
        }

        private void chkClickTuneFilter_CheckedChanged(object sender, System.EventArgs e)
        {
            console.ClickTuneFilter = chkClickTuneFilter.Checked;
            Display.ClickTuneFilter = chkClickTuneFilter.Checked;
        }

        private void chkShowCTHLine_CheckedChanged(object sender, System.EventArgs e)
        {
            Display.ShowCTHLine = chkShowCTHLine.Checked;
        }

        private void radPACalAllBands_CheckedChanged(object sender, System.EventArgs e)
        {
            foreach (Control c in grpPAGainByBand.Controls)
            {
                if (c.Name.StartsWith("chkPA"))
                {
                    c.Visible = !radPACalAllBands.Checked;
                }
            }
            /*if(radGenModelFLEX5000.Checked && !radPACalAllBands.Checked) //w5wc
                chkPA6.Visible = true;
            else chkPA6.Visible = false;*/
        }

        private void chkZeroBeatRIT_CheckedChanged(object sender, System.EventArgs e)
        {
            console.ZeroBeatRIT = chkZeroBeatRIT.Checked;
        }

        private FWCAnt old_ant = FWCAnt.ANT1;
        private void ckEnableSigGen_CheckedChanged(object sender, System.EventArgs e)
        {
            if (console.fwc_init)
            {
                if (ckEnableSigGen.Checked)
                {
                    old_ant = console.RX1Ant;
                    console.RX1Ant = FWCAnt.SIG_GEN;
                }
                else console.RX1Ant = old_ant;
                FWC.SetTest(ckEnableSigGen.Checked);
                FWC.SetGen(ckEnableSigGen.Checked);
                FWC.SetSig(ckEnableSigGen.Checked);
            }

            if (!console.FullDuplex)
                FWC.SetFullDuplex(ckEnableSigGen.Checked);
        }

        private void chkPANewCal_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkPANewCal.Checked;

            console.NewPowerCal = b;

            lblPAGainByBand160.Visible = !b;
            lblPAGainByBand80.Visible = !b;
            lblPAGainByBand60.Visible = !b;
            lblPAGainByBand40.Visible = !b;
            lblPAGainByBand30.Visible = !b;
            lblPAGainByBand20.Visible = !b;
            lblPAGainByBand17.Visible = !b;
            lblPAGainByBand15.Visible = !b;
            lblPAGainByBand12.Visible = !b;
            lblPAGainByBand10.Visible = !b;

            udPAGain160.Visible = !b;
            udPAGain80.Visible = !b;
            udPAGain60.Visible = !b;
            udPAGain40.Visible = !b;
            udPAGain30.Visible = !b;
            udPAGain20.Visible = !b;
            udPAGain17.Visible = !b;
            udPAGain15.Visible = !b;
            udPAGain12.Visible = !b;
            udPAGain10.Visible = !b;
            udPAGain6.Visible = !b;

            if (!radGenModelFLEX5000.Checked)
            {
                lblPACalTarget.Visible = !b;
                udPACalPower.Visible = !b;
                btnPAGainReset.Visible = !b;
            }
        }

        private void chkAudioExpert_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkAudioExpert.Checked && chkAudioExpert.Focused)
            {
                DialogResult dr = MessageBox.Show("The Expert mode allows the user to control advanced controls that only \n" +
                    "experienced PowerSDR users should use.  These controls may allow the user\n" +
                    "to cause damage to the radio.  Are you sure you want to enable Expert mode?",
                    "Warning: Enable Expert Mode?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    chkAudioExpert.Checked = false;
                    return;
                }
            }

            bool b = chkAudioExpert.Checked;
            grpAudioLatency1.Visible = b;
            grpAudioVolts1.Visible = ((b && !radGenModelFLEX5000.Checked) || (comboAudioSoundCard.Text == "Unsupported Card" && !radGenModelFLEX5000.Checked));
        }

        private void udMeterDigitalDelay_ValueChanged(object sender, System.EventArgs e)
        {
            console.MeterDigDelay = (int)udMeterDigitalDelay.Value;
        }

        private void chkMouseTuneStep_CheckedChanged(object sender, System.EventArgs e)
        {
            console.MouseTuneStep = chkMouseTuneStep.Checked;
        }

        private void chkBoxJanusOzyControl_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                console.OzyControl = chkBoxJanusOzyControl.Checked;
                if (chkBoxJanusOzyControl.Checked)
                {
                    chkBoxJanusOzyControl.Checked = false;
                    MessageBox.Show("Error initializing Ozy; Ozy Control has been disabled. ", "Ozy Initialization Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                }
                else
                {
                    OzySDR1kControl.Close();
                }

                if (console.PowerOn)
                {
                    console.PowerOn = false;
                    Thread.Sleep(100);
                    console.PowerOn = true;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("OzyInitException: " + ex.ToString());
                MessageBox.Show("Exception initializing Ozy: " + ex.Message, "Ozy Init Error: ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                chkBoxJanusOzyControl.Checked = false;
            }
        }

        private void chkGenDDSExpert_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkGenDDSExpert.Checked && chkGenDDSExpert.Focused)
            {
                DialogResult dr = MessageBox.Show("The Expert mode allows the user to control advanced controls that only \n" +
                    "experienced PowerSDR users should use.  These controls may allow the user\n" +
                    "to change important calibration parameters.\n" +
                    "Are you sure you want to enable Expert mode?",
                    "Warning: Enable Expert Mode?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    chkGenDDSExpert.Checked = false;
                    return;
                }
            }

            bool b = chkGenDDSExpert.Checked;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    lblClockCorrection.Visible = b;
                    udDDSCorrection.Visible = b;
                    lblIFFrequency.Visible = b;
                    udDDSIFFreq.Visible = b;
                    break;
                default:
                    lblClockCorrection.Visible = b;
                    udDDSCorrection.Visible = b;
                    lblPLLMult.Visible = b;
                    udDDSPLLMult.Visible = b;
                    lblIFFrequency.Visible = b;
                    udDDSIFFreq.Visible = b;
                    break;
            }
        }

        private void chkCalExpert_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelFLEX5000.Checked)
            {
                if (chkCalExpert.Checked && chkCalExpert.Focused)
                {
                    DialogResult dr = MessageBox.Show("The Expert mode allows the user to control advanced controls that only \n" +
                        "experienced PowerSDR users should use.  These controls may allow the user\n" +
                        "to change important calibration parameters.\n" +
                        "Are you sure you want to enable Expert mode?",
                        "Warning: Enable Expert Mode?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    if (dr == DialogResult.No)
                    {
                        chkCalExpert.Checked = false;
                        return;
                    }
                }

                bool b = chkCalExpert.Checked;
                switch (console.CurrentModel)
                {
                    case Model.FLEX5000:
                    case Model.FLEX3000:
                        grpGeneralCalibration.Visible = b;
                        grpGenCalLevel.Visible = false;
                        grpGenCalRXImage.Visible = false;
                        break;
                    default:
                        grpGeneralCalibration.Visible = b;
                        grpGenCalLevel.Visible = b;
                        grpGenCalRXImage.Visible = b;
                        break;
                }
            }
        }

        private void chkDSPImageExpert_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelFLEX5000.Checked)
            {
                if (chkDSPImageExpert.Checked && chkDSPImageExpert.Focused)
                {
                    DialogResult dr = MessageBox.Show("The Expert mode allows the user to control advanced controls that only \n" +
                        "experienced PowerSDR users should use.  These controls may allow the user\n" +
                        "to cause damage to the radio or change important calibration parameters.\n" +
                        "Are you sure you want to enable Expert mode?",
                        "Warning: Enable Expert Mode?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    if (dr == DialogResult.No)
                    {
                        chkDSPImageExpert.Checked = false;
                        return;
                    }
                }

                bool b = chkDSPImageExpert.Checked;
                grpDSPImageRejectTX.Visible = b;
            }
        }

        private void txtGenCustomTitle_TextChanged(object sender, System.EventArgs e)
        {
            string title = console.Text;
            int index = title.IndexOf("   --   ");
            if (index >= 0) title = title.Substring(0, index);
            if (!string.IsNullOrEmpty(txtGenCustomTitle.Text))
                title += "   --   " + txtGenCustomTitle.Text;
            console.Text = title;
        }

        private void chkGenFLEX5000ExtRef_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelFLEX5000.Checked)
                FWC.SetXREF(chkGenFLEX5000ExtRef.Checked);
        }

        private void chkGenAllModeMicPTT_CheckedChanged(object sender, System.EventArgs e)
        {
            console.AllModeMicPTT = chkGenAllModeMicPTT.Checked;
        }

        private void chkKWAI_CheckedChanged(object sender, System.EventArgs e)
        {
            AllowFreqBroadcast = chkKWAI.Checked;
        }

        private void chkSplitOff_CheckedChanged(object sender, System.EventArgs e)
        {
            console.DisableSplitOnBandchange = chkSplitOff.Checked;
        }

        public bool RFE_PA_TR
        {
            get { return chkEnableRFEPATR.Checked; }
            set { chkEnableRFEPATR.Checked = value; }
        }

        private void chkEnableRFEPATR_CheckedChanged(object sender, System.EventArgs e)
        {
            console.RFE_PA_TR_enable = chkEnableRFEPATR.Checked;
        }

        private void chkVACAllowBypass_CheckedChanged(object sender, System.EventArgs e)
        {
            console.AllowVACBypass = chkVACAllowBypass.Checked;
        }

        private void chkDSPTXMeterPeak_CheckedChanged(object sender, System.EventArgs e)
        {
            console.PeakTXMeter = chkDSPTXMeterPeak.Checked;
        }

        private void chkVACCombine_CheckedChanged(object sender, System.EventArgs e)
        {
            Audio.VACCombineInput = chkVACCombine.Checked;
        }

        private void chkCWAutoSwitchMode_CheckedChanged(object sender, System.EventArgs e)
        {
           // console.Keyer.AutoSwitchMode = chkCWAutoSwitchMode.Checked;
            console.CWAutoModeSwitch = chkCWAutoSwitchMode.Checked;
        }

        private void clrbtnGenBackground_Changed(object sender, System.EventArgs e)//k6jca 1/13/08
        {
            //console.GenBackgroundColor = clrbtnGenBackground.Color;
        }

        public MeterTXMode TuneMeterTXMode
        {
            set
            {
                switch (value)
                {
                    case MeterTXMode.FORWARD_POWER:
                        comboTXTUNMeter.Text = "Fwd Pwr";
                        break;
                    case MeterTXMode.REVERSE_POWER:
                        comboTXTUNMeter.Text = "Ref Pwr";
                        break;
                    case MeterTXMode.SWR:
                        comboTXTUNMeter.Text = "SWR";
                        break;
                    case MeterTXMode.OFF:
                        comboTXTUNMeter.Text = "Off";
                        break;
                }
            }
        }

        private void comboTXTUNMeter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (comboTXTUNMeter.Text)
            {
                case "Fwd Pwr":
                    console.TuneTXMeterMode = MeterTXMode.FORWARD_POWER;
                    break;
                case "Ref Pwr":
                    console.TuneTXMeterMode = MeterTXMode.REVERSE_POWER;
                    break;
                case "SWR":
                    console.TuneTXMeterMode = MeterTXMode.SWR;
                    break;
                case "Off":
                    console.TuneTXMeterMode = MeterTXMode.OFF;
                    break;
            }
        }

        private void btnResetDB_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = MessageBox.Show("This will close the program, make a copy of the current\n" +
                "database to your desktop, and reset the active database\n" +
                "the next time PowerSDR is launched.\n\n" +
                "Are you sure you want to reset the database?",
                "Reset Database?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr == DialogResult.No) return;

            console.reset_db = true;
            console.Close();
        }

        private void chkDisplayMeterShowDecimal_CheckedChanged(object sender, System.EventArgs e)
        {
            console.MeterDetail = chkDisplayMeterShowDecimal.Checked;
        }

        private void chkRTTYOffsetEnableA_CheckedChanged(object sender, System.EventArgs e)
        {
            rtty_offset_enabled_a = chkRTTYOffsetEnableA.Checked;
        }

        private void chkRTTYOffsetEnableB_CheckedChanged(object sender, System.EventArgs e)
        {
            rtty_offset_enabled_b = chkRTTYOffsetEnableB.Checked;
        }

        private void udRTTYL_ValueChanged(object sender, System.EventArgs e)
        {
            rtty_offset_low = (int)udRTTYL.Value;
        }

        private void udRTTYU_ValueChanged(object sender, System.EventArgs e)
        {
            rtty_offset_high = (int)udRTTYU.Value;
        }

        private void chkRX2AutoMuteTX_CheckedChanged(object sender, System.EventArgs e)
        {
            Audio.RX2AutoMuteTX = chkRX2AutoMuteTX.Checked;
        }

        private void chkAudioIQtoVAC_CheckedChanged(object sender, System.EventArgs e)
        {
            bool power = console.PowerOn;
            if (power && chkAudioEnableVAC.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            Audio.VACOutputIQ = chkAudioIQtoVAC.Checked;

            if (power && chkAudioEnableVAC.Checked)
                console.PowerOn = true;

            chkAudioCorrectIQ.Enabled = chkAudioIQtoVAC.Checked;
            chkAudioRX2toVAC.Enabled = chkAudioIQtoVAC.Checked;
        }

        private void chkVAC2DirectIQ_CheckedChanged(object sender, System.EventArgs e)
        {
            bool power = console.PowerOn;
            if (power && chkVAC2Enable.Checked)
            {
                console.PowerOn = false;
                Thread.Sleep(100);
            }

            Audio.VAC2OutputIQ = chkVAC2DirectIQ.Checked;

            if (power && chkVAC2Enable.Checked)
                console.PowerOn = true;

            chkVAC2DirectIQCal.Enabled = chkVAC2DirectIQ.Checked;
        }

        private void chkAudioCorrectIQ_CheckChanged(object sender, System.EventArgs e)
        {
            Audio.VACCorrectIQ = chkAudioCorrectIQ.Checked;
        }

        private void chkVAC2IQCal_CheckChanged(object sender, System.EventArgs e)
        {
            Audio.VAC2CorrectIQ = chkVAC2DirectIQCal.Checked;
        }

        private void chkRX2AutoMuteRX1OnVFOBTX_CheckedChanged(object sender, System.EventArgs e)
        {
            console.MuteRX1OnVFOBTX = chkRX2AutoMuteRX1OnVFOBTX.Checked;
        }

        private void chkRX2BlankDisplayOnVFOATX_CheckedChanged(object sender, System.EventArgs e)
        {
            console.BlankRX2OnVFOATX = chkRX2BlankDisplayOnVFOATX.Checked;
        }
        
        private void chkTXExpert_CheckedChanged(object sender, System.EventArgs e)
        {
            grpTXProfileDef.Visible = chkTXExpert.Checked;
        }

        private void btnTXProfileDefImport_Click(object sender, System.EventArgs e)
        {
            if (lstTXProfileDef.SelectedIndex < 0) return;

            DialogResult result = MessageBox.Show("Import profile from defaults?",
                "Import?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;

            string name = lstTXProfileDef.Text;
            DataRow[] rows = DB.ds.Tables["TxProfileDef"].Select(
                "'" + name + "' = Name");

            if (rows.Length != 1)
            {
                MessageBox.Show("Database error reading TXProfileDef Table.",
                    "Database error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            DataRow dr = null;
            if (comboTXProfileName.Items.Contains(name))
            {
                result = MessageBox.Show(
                    "Are you sure you want to overwrite the " + name + " TX Profile?",
                    "Overwrite Profile?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    return;

                foreach (DataRow d in DB.ds.Tables["TxProfile"].Rows)
                {
                    if ((string)d["Name"] == name)
                    {
                        dr = d;
                        break;
                    }
                }
            }
            else
            {
                dr = DB.ds.Tables["TxProfile"].NewRow();
                dr["Name"] = name;
            }

            for (int i = 0; i < dr.ItemArray.Length; i++)
                dr[i] = rows[0][i];

            if (!comboTXProfileName.Items.Contains(name))
            {
                DB.ds.Tables["TxProfile"].Rows.Add(dr);
                comboTXProfileName.Items.Add(name);
                comboTXProfileName.Text = name;
            }

            console.UpdateTXProfile(name);
        }

        private void Setup_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Control == true && e.Alt == true)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        chkPANewCal.Visible = true;
                        grpPAGainByBand.Visible = true;
                        break;
                    case Keys.O:
                        radSigGenTXOutput.Visible = true;
                        break;
                }
            }
        }

        private void chkDisplayPanFill_CheckedChanged(object sender, System.EventArgs e)
        {
            Display.PanFill = chkDisplayPanFill.Checked;
        }

        private void chkTXPanFill_CheckedChanged(object sender, System.EventArgs e)
        {
            Display.TXPanFill = chkTXPanFill.Checked;
        }

        private void udF3KFanTempThresh_ValueChanged(object sender, System.EventArgs e)
        {
            console.F3KTempThresh = (float)udF3KFanTempThresh.Value;
        }

        private void chkGenTX1Delay_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!console.fwc_init || console.CurrentModel != Model.FLEX3000) return;
            FWC.SetAmpTX1DelayEnable(chkGenTX1Delay.Checked);
            udGenTX1Delay.Enabled = chkGenTX1Delay.Checked;
        }

        private void udGenTX1Delay_ValueChanged(object sender, System.EventArgs e)
        {
            if (!console.fwc_init || console.CurrentModel != Model.FLEX3000) return;
            FWC.SetAmpTX1Delay((uint)udGenTX1Delay.Value);
        }

        private void comboAppSkin_SelectedIndexChanged(object sender, EventArgs e)
        {
            string path;
            if (console.default_directory)
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    "\\FlexRadio Systems\\PowerSDR\\Skins\\";
            }
            else
            {
                path = Application.StartupPath + "\\Skins\\";
            }

            if (Directory.Exists(path + comboAppSkin.Text))
                Skin.Restore(comboAppSkin.Text, path, console);
        }

        private void btnSkinExport_Click(object sender, EventArgs e)
        {
            string path;
            if (console.default_directory)
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    "\\FlexRadio Systems\\PowerSDR\\Skins\\";
            }
            else
            {
                path = Application.StartupPath + "\\Skins\\";
            }
            if (Directory.Exists(path + comboAppSkin.Text))
                Skin.Save(comboAppSkin.Text, path, console);
        }

        private void chkCWDisableUI_CheckedChanged(object sender, EventArgs e)
        {
            console.DisableUIMOXChanges = chkCWDisableUI.Checked;
        }

        private void chkAudioRX2toVAC_CheckedChanged(object sender, EventArgs e)
        {
            Audio.VACOutputRX2 = chkAudioRX2toVAC.Checked;
        }

        private void chkVAC2UseRX2_CheckedChanged(object sender, EventArgs e)
        {
            console.VAC2RX2 = chkVAC2UseRX2.Checked;
        }

        private void chkGenOptionsShowATUPopup_CheckedChanged(object sender, System.EventArgs e)
        {
            if (console.flex3000ATUForm != null && !console.flex3000ATUForm.IsDisposed)
                console.flex3000ATUForm.ShowFeedbackPopup = chkGenOptionsShowATUPopup.Checked;
        }

        private void chkSpaceNavControlVFOs_CheckChanged(object sender, System.EventArgs e)
        {
            console.SpaceNavControlVFOs = (bool)chkSpaceNavControlVFOs.Checked;
        }

        private void chkSpaceNAvFlyPanadapter_CheckChanged(object sender, System.EventArgs e)
        {
            console.SpaceNavFlyPanadapter = (bool)chkSpaceNavFlyPanadapter.Checked;
        }

        private void tbRX1FilterAlpha_Scroll(object sender, EventArgs e)
        {
            clrbtnFilter_Changed(this, EventArgs.Empty);
            toolTip1.SetToolTip(tbRX1FilterAlpha, tbRX1FilterAlpha.Value.ToString());
        }

        private void tbTXFilterAlpha_Scroll(object sender, EventArgs e)
        {
            clrbtnGridTXFilter_Changed(this, EventArgs.Empty);
            toolTip1.SetToolTip(tbTXFilterAlpha, tbTXFilterAlpha.Value.ToString());
        }

        private void udTXNoiseGateAttenuate_ValueChanged(object sender, System.EventArgs e)
        {
            console.radio.GetDSPTX(0).TXSquelchAttenuate = (float)udTXNoiseGateAttenuate.Value;
        }

        private void tbMultiRXFilterAlpha_Scroll(object sender, EventArgs e)
        {
            clrbtnSubRXFilter_Changed(this, EventArgs.Empty);
            toolTip1.SetToolTip(tbMultiRXFilterAlpha, tbMultiRXFilterAlpha.Value.ToString());
        }

        private void chkWheelTuneVFOB_CheckedChanged(object sender, EventArgs e)
        {
            console.WheelTunesVFOB = chkWheelTuneVFOB.Checked;
        }

        private void chkCWKeyerMonoCable_CheckedChanged(object sender, EventArgs e)
        {
            if (console.fwc_init && (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX3000))
            {
                FWC.ignore_dash = chkCWKeyerMonoCable.Checked;
                //  if (chkCWKeyerMonoCable.Checked && console.Keyer != null)
                //    console.Keyer.FWCDash = false;
            }

                if (chkCWKeyerMonoCable.Checked)
                    CWKeyer.SensorEnqueue(new CWSensorItem(CWSensorItem.InputType.Dash, false));
           
        }

        private void btnExportDB_Click(object sender, EventArgs e)
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string datetime = DateTime.Now.ToShortDateString().Replace("/", "-") + "_" +
                    DateTime.Now.ToShortTimeString().Replace(":", ".");
            saveFileDialog1.FileName = desktop + "\\PowerSDR_database_export_" + datetime + ".xml";
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            DB.ds.WriteXml(saveFileDialog1.FileName, XmlWriteMode.WriteSchema);
        }

        private void udPulseDuty_ValueChanged(object sender, EventArgs e)
        {
            Audio.PulseDuty = (double)udPulseDuty.Value / 100.0;
        }

        private void udPulsePeriod_ValueChanged(object sender, EventArgs e)
        {
            Audio.PulsePeriod = (double)udPulsePeriod.Value;
        }

        private void chkPennyLane_CheckedChanged(object sender, System.EventArgs e)
        {
            int bits = JanusAudio.GetC1Bits();
            if (!chkPennyLane.Checked)
            {
                bits &= 0xdf;  // 11011111
                //console.PennyLanePresent = false;
                radPenny10MHz.Checked = false;
                radPenny10MHz.Enabled = false;
                radPennyMic.Checked = false;
                radPennyMic.Enabled = false;
                rad12288MHzPenny.Checked = false;
                rad12288MHzPenny.Enabled = false;
                grpPennyExtCtrl.Enabled = false;
                chkPennyExtCtrl.Checked = false;
                chkPennyExtCtrl.Enabled = false;
                JanusAudio.EnableHermesPower(0);
            }
            else
            {
                bits |= 0x20;   // 00100000

                chkPennyPresent.Checked = false;
                radPenny10MHz.Enabled = true;
                radPennyMic.Enabled = true;
                rad12288MHzPenny.Enabled = true;

                chkPennyExtCtrl.Enabled = true;
                if (chkPennyExtCtrl.Checked)
                {
                    grpPennyExtCtrl.Enabled = true;
                }
                console.PennyPresent = false;
                //console.PennyLanePresent = true;
                chkGeneralRXOnly.Enabled = true;
                // chkGeneralRXOnly.Enabled = false;

                JanusAudio.EnableHermesPower(1);
            }
            console.PennyLanePresent = chkPennyLane.Checked;
            JanusAudio.SetC1Bits(bits);
            checkHPSDRDefaults(sender, e);
            JanusAudio.fwVersionsChecked = false;
        }

        private void chkPennyPresent_CheckedChanged(object sender, System.EventArgs e)
        {
            int bits = JanusAudio.GetC1Bits();
            if (!chkPennyPresent.Checked)
            {
                bits &= 0xdf;  // 11011111
                //console.PennyPresent = false;
                radPenny10MHz.Checked = false;
                radPenny10MHz.Enabled = false;
                radPennyMic.Checked = false;
                radPennyMic.Enabled = false;
                rad12288MHzPenny.Checked = false;
                rad12288MHzPenny.Enabled = false;
                grpPennyExtCtrl.Enabled = false;
                chkPennyExtCtrl.Checked = false;
                chkPennyExtCtrl.Enabled = false;
            }
            else
            {
                chkPennyLane.Checked = false;
                bits |= 0x20;   // 00100000

                radPenny10MHz.Enabled = true;
                radPennyMic.Enabled = true;
                rad12288MHzPenny.Enabled = true;

                chkPennyExtCtrl.Enabled = true;
                if (chkPennyExtCtrl.Checked)
                {
                    grpPennyExtCtrl.Enabled = true;
                }
                //console.PennyPresent = true;
                // chkGeneralRXOnly.Enabled = true;  
                console.PennyLanePresent = false;

                JanusAudio.EnableHermesPower(0);
            }
            console.PennyPresent = chkPennyPresent.Checked;
            JanusAudio.SetC1Bits(bits);
            checkHPSDRDefaults(sender, e);
            JanusAudio.fwVersionsChecked = false;
        }

        private void checkHPSDRDefaults(object sender, System.EventArgs e)
        {
            if (chkJanusPresent.Checked && !chkPennyPresent.Checked && !chkPennyLane.Checked)  // only janus - default mic to Janus 
            {
                radJanusMic.Checked = true;
                radJanusMic_CheckedChanged(sender, e);
            }
            else if ((chkPennyPresent.Checked || chkPennyLane.Checked) && !chkJanusPresent.Checked)
            {
                radPennyMic.Checked = true;
                radPennyMic_CheckedChanged(sender, e);
            }

            if ((chkPennyPresent.Checked || chkPennyLane.Checked) && !chkMercuryPresent.Checked)
            {
                rad12288MHzPenny.Checked = true;
                rad12288MHzPenny_CheckedChanged(sender, e);
            }
            else if (chkMercuryPresent.Checked && !chkPennyPresent.Checked && !chkPennyLane.Checked)
            {
                radMercury12288MHz.Checked = true;
                radMercury12288MHz_CheckedChanged(sender, e);
            }

            if (chkMercuryPresent.Checked && !radMercury10MHz.Checked && !radAtlas10MHz.Checked &&
                !radPenny10MHz.Checked)
            {
                radMercury10MHz.Checked = true;
                radMercury10MHz_CheckedChanged(sender, e);
                radMercury12288MHz.Checked = true;
                radMercury12288MHz_CheckedChanged(sender, e);
            }

            if (!chkPennyPresent.Checked && !chkPennyLane.Checked)
            {
                chkGeneralRXOnly.Checked = true;
                // chkGeneralRXOnly.Enabled = false;
            }
            else
            {
                // chkGeneralRXOnly.Enabled = true;
                chkGeneralRXOnly.Checked = false;
            }
            return;
        }

        private void chkMercuryPresent_CheckedChanged(object sender, System.EventArgs e)
        {
            int bits = JanusAudio.GetC1Bits();
            if (!chkMercuryPresent.Checked)
            {
                radMercury10MHz.Checked = false;
                radMercury12288MHz.Checked = false;
                radMercury10MHz.Enabled = false;
                radMercury12288MHz.Enabled = false;

                bits &= 0xbf;  // 1011 1111
            }
            else
            {
                radMercury10MHz.Enabled = true;
                radMercury12288MHz.Enabled = true;

                bits |= 0x40;   // 0100 0000
            }
            JanusAudio.SetC1Bits(bits);
            checkHPSDRDefaults(sender, e);
            console.MercuryPresent = chkMercuryPresent.Checked;
            JanusAudio.fwVersionsChecked = false;
        }

        private void chkAlexPresent_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkAlexPresent.Checked)
            {
                chkAlexAntCtrl.Enabled = true;
                chkAlexAntCtrl.Checked = true;
                radAlexManualCntl.Enabled = true;
                //  if (!chkAlexAntCtrl.Checked)
                //  {
                //    grpAlexAntCtrl.Enabled = false;
                // }
                console.chkSR.Enabled = true;
                if (console.rx2_preamp_present) console.chkRX2Preamp.Visible = false;
                if (chkApolloPresent.Checked) chkApolloPresent.Checked = false;
                if (radGenModelHermes.Checked) JanusAudio.SetHermesFilter(0);
            }
            else
            {
                chkAlexAntCtrl.Checked = false;
                chkAlexAntCtrl.Enabled = false;
                // grpAlexAntCtrl.Enabled = false;
                radAlexAutoCntl.Checked = true;
                radAlexManualCntl.Enabled = false;
                console.chkSR.Enabled = false;
                if (console.rx2_preamp_present) console.chkRX2Preamp.Visible = true;
            }
            console.AlexPresent = chkAlexPresent.Checked;
            console.SetComboPreampForHPSDR();
        }


        private void chkExcaliburPresent_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkExcaliburPresent.Checked)
            {
                radAtlas10MHz.Checked = true;
                groupBox10MhzClock.Enabled = false;
            }
            else
            {
                groupBox10MhzClock.Enabled = true;
            }

        }

        private void radAtlas10MHz_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radAtlas10MHz.Checked)
            {
                int bits = JanusAudio.GetC1Bits();
                radMercury10MHz.Checked = false;
                radPenny10MHz.Checked = false;
                bits &= 0xf3;  // 1111 0011
                JanusAudio.SetC1Bits(bits);
            }
        }

        private void radMercury10MHz_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radMercury10MHz.Checked)
            {
                int bits = JanusAudio.GetC1Bits();
                radAtlas10MHz.Checked = false;
                radPenny10MHz.Checked = false;
                bits &= 0xf3;  // 1111 0011
                bits |= 0x8;  // 0000 1000
                JanusAudio.SetC1Bits(bits);
            }
        }

        private void radPenny10MHz_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radPenny10MHz.Checked)
            {
                int bits = JanusAudio.GetC1Bits();
                radAtlas10MHz.Checked = false;
                radMercury10MHz.Checked = false;
                bits &= 0xf3;  // 1111 0011
                bits |= 0x4;  // 0000 0100
                JanusAudio.SetC1Bits(bits);
            }
        }

        private void rad12288MHzPenny_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rad12288MHzPenny.Checked)
            {
                int bits = JanusAudio.GetC1Bits();
                radMercury12288MHz.Checked = false;

                bits &= 0xef;  // 1110 1111				
                JanusAudio.SetC1Bits(bits);
            }
        }

        private void radMercury12288MHz_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radMercury12288MHz.Checked)
            {
                int bits = JanusAudio.GetC1Bits();
                rad12288MHzPenny.Checked = false;

                bits |= 0x10;  // 0001 0000				
                JanusAudio.SetC1Bits(bits);
            }
        }

        private void radPennyMic_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radPennyMic.Checked)
            {
                int bits = JanusAudio.GetC1Bits();
                radJanusMic.Checked = false;

                bits |= 0x80;  // 1000 0000				
                JanusAudio.SetC1Bits(bits);
            }
        }

        private void radJanusMic_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radJanusMic.Checked)
            {
                int bits = JanusAudio.GetC1Bits();
                radPennyMic.Checked = false;

                bits &= 0x7f;  // 0111 1111
                JanusAudio.SetC1Bits(bits);
            }
        }

        private void chkJanusPresent_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkJanusPresent.Checked)
            {
                radJanusMic.Enabled = true;
            }
            else
            {
                radJanusMic.Checked = false;
                radJanusMic.Enabled = false;
            }
            checkHPSDRDefaults(sender, e);
            console.JanusPresent = chkJanusPresent.Checked;
        }

        private void chkPenOCrcv160_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkPenOCrcv1601.Checked) val += 1 << 0;
            if (chkPenOCrcv1602.Checked) val += 1 << 1;
            if (chkPenOCrcv1603.Checked) val += 1 << 2;
            if (chkPenOCrcv1604.Checked) val += 1 << 3;
            if (chkPenOCrcv1605.Checked) val += 1 << 4;
            if (chkPenOCrcv1606.Checked) val += 1 << 5;
            if (chkPenOCrcv1607.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B160M, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCxmit160_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkPenOCxmit1601.Checked) val += 1 << 0;
            if (chkPenOCxmit1602.Checked) val += 1 << 1;
            if (chkPenOCxmit1603.Checked) val += 1 << 2;
            if (chkPenOCxmit1604.Checked) val += 1 << 3;
            if (chkPenOCxmit1605.Checked) val += 1 << 4;
            if (chkPenOCxmit1606.Checked) val += 1 << 5;
            if (chkPenOCxmit1607.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B160M, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcv80_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkPenOCrcv801.Checked) val += 1 << 0;
            if (chkPenOCrcv802.Checked) val += 1 << 1;
            if (chkPenOCrcv803.Checked) val += 1 << 2;
            if (chkPenOCrcv804.Checked) val += 1 << 3;
            if (chkPenOCrcv805.Checked) val += 1 << 4;
            if (chkPenOCrcv806.Checked) val += 1 << 5;
            if (chkPenOCrcv807.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B80M, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCxmit80_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCxmit801.Checked) val += 1 << 0;
            if (chkPenOCxmit802.Checked) val += 1 << 1;
            if (chkPenOCxmit803.Checked) val += 1 << 2;
            if (chkPenOCxmit804.Checked) val += 1 << 3;
            if (chkPenOCxmit805.Checked) val += 1 << 4;
            if (chkPenOCxmit806.Checked) val += 1 << 5;
            if (chkPenOCxmit807.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B80M, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcv60_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCrcv601.Checked) val += 1 << 0;
            if (chkPenOCrcv602.Checked) val += 1 << 1;
            if (chkPenOCrcv603.Checked) val += 1 << 2;
            if (chkPenOCrcv604.Checked) val += 1 << 3;
            if (chkPenOCrcv605.Checked) val += 1 << 4;
            if (chkPenOCrcv606.Checked) val += 1 << 5;
            if (chkPenOCrcv607.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B60M, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCxmit60_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCxmit601.Checked) val += 1 << 0;
            if (chkPenOCxmit602.Checked) val += 1 << 1;
            if (chkPenOCxmit603.Checked) val += 1 << 2;
            if (chkPenOCxmit604.Checked) val += 1 << 3;
            if (chkPenOCxmit605.Checked) val += 1 << 4;
            if (chkPenOCxmit606.Checked) val += 1 << 5;
            if (chkPenOCxmit607.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B60M, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcv40_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCrcv401.Checked) val += 1 << 0;
            if (chkPenOCrcv402.Checked) val += 1 << 1;
            if (chkPenOCrcv403.Checked) val += 1 << 2;
            if (chkPenOCrcv404.Checked) val += 1 << 3;
            if (chkPenOCrcv405.Checked) val += 1 << 4;
            if (chkPenOCrcv406.Checked) val += 1 << 5;
            if (chkPenOCrcv407.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B40M, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCxmit40_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCxmit401.Checked) val += 1 << 0;
            if (chkPenOCxmit402.Checked) val += 1 << 1;
            if (chkPenOCxmit403.Checked) val += 1 << 2;
            if (chkPenOCxmit404.Checked) val += 1 << 3;
            if (chkPenOCxmit405.Checked) val += 1 << 4;
            if (chkPenOCxmit406.Checked) val += 1 << 5;
            if (chkPenOCxmit407.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B40M, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcv30_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCrcv301.Checked) val += 1 << 0;
            if (chkPenOCrcv302.Checked) val += 1 << 1;
            if (chkPenOCrcv303.Checked) val += 1 << 2;
            if (chkPenOCrcv304.Checked) val += 1 << 3;
            if (chkPenOCrcv305.Checked) val += 1 << 4;
            if (chkPenOCrcv306.Checked) val += 1 << 5;
            if (chkPenOCrcv307.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B30M, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCxmit30_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCxmit301.Checked) val += 1 << 0;
            if (chkPenOCxmit302.Checked) val += 1 << 1;
            if (chkPenOCxmit303.Checked) val += 1 << 2;
            if (chkPenOCxmit304.Checked) val += 1 << 3;
            if (chkPenOCxmit305.Checked) val += 1 << 4;
            if (chkPenOCxmit306.Checked) val += 1 << 5;
            if (chkPenOCxmit307.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B30M, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcv20_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCrcv201.Checked) val += 1 << 0;
            if (chkPenOCrcv202.Checked) val += 1 << 1;
            if (chkPenOCrcv203.Checked) val += 1 << 2;
            if (chkPenOCrcv204.Checked) val += 1 << 3;
            if (chkPenOCrcv205.Checked) val += 1 << 4;
            if (chkPenOCrcv206.Checked) val += 1 << 5;
            if (chkPenOCrcv207.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B20M, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCxmit20_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCxmit201.Checked) val += 1 << 0;
            if (chkPenOCxmit202.Checked) val += 1 << 1;
            if (chkPenOCxmit203.Checked) val += 1 << 2;
            if (chkPenOCxmit204.Checked) val += 1 << 3;
            if (chkPenOCxmit205.Checked) val += 1 << 4;
            if (chkPenOCxmit206.Checked) val += 1 << 5;
            if (chkPenOCxmit207.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B20M, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcv17_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCrcv171.Checked) val += 1 << 0;
            if (chkPenOCrcv172.Checked) val += 1 << 1;
            if (chkPenOCrcv173.Checked) val += 1 << 2;
            if (chkPenOCrcv174.Checked) val += 1 << 3;
            if (chkPenOCrcv175.Checked) val += 1 << 4;
            if (chkPenOCrcv176.Checked) val += 1 << 5;
            if (chkPenOCrcv177.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B17M, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCxmit17_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCxmit171.Checked) val += 1 << 0;
            if (chkPenOCxmit172.Checked) val += 1 << 1;
            if (chkPenOCxmit173.Checked) val += 1 << 2;
            if (chkPenOCxmit174.Checked) val += 1 << 3;
            if (chkPenOCxmit175.Checked) val += 1 << 4;
            if (chkPenOCxmit176.Checked) val += 1 << 5;
            if (chkPenOCxmit177.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B17M, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcv15_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCrcv151.Checked) val += 1 << 0;
            if (chkPenOCrcv152.Checked) val += 1 << 1;
            if (chkPenOCrcv153.Checked) val += 1 << 2;
            if (chkPenOCrcv154.Checked) val += 1 << 3;
            if (chkPenOCrcv155.Checked) val += 1 << 4;
            if (chkPenOCrcv156.Checked) val += 1 << 5;
            if (chkPenOCrcv157.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B15M, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCxmit15_CheckedChanged(object sender, System.EventArgs e)
        {
            int val = 0;
            if (chkPenOCxmit151.Checked) val += 1 << 0;
            if (chkPenOCxmit152.Checked) val += 1 << 1;
            if (chkPenOCxmit153.Checked) val += 1 << 2;
            if (chkPenOCxmit154.Checked) val += 1 << 3;
            if (chkPenOCxmit155.Checked) val += 1 << 4;
            if (chkPenOCxmit156.Checked) val += 1 << 5;
            if (chkPenOCxmit157.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B15M, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 

        }


        private void chkPenOCrcv12_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCrcv121.Checked) val += 1 << 0;
            if (chkPenOCrcv122.Checked) val += 1 << 1;
            if (chkPenOCrcv123.Checked) val += 1 << 2;
            if (chkPenOCrcv124.Checked) val += 1 << 3;
            if (chkPenOCrcv125.Checked) val += 1 << 4;
            if (chkPenOCrcv126.Checked) val += 1 << 5;
            if (chkPenOCrcv127.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B12M, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCxmit12_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCxmit121.Checked) val += 1 << 0;
            if (chkPenOCxmit122.Checked) val += 1 << 1;
            if (chkPenOCxmit123.Checked) val += 1 << 2;
            if (chkPenOCxmit124.Checked) val += 1 << 3;
            if (chkPenOCxmit125.Checked) val += 1 << 4;
            if (chkPenOCxmit126.Checked) val += 1 << 5;
            if (chkPenOCxmit127.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B12M, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcv10_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCrcv101.Checked) val += 1 << 0;
            if (chkPenOCrcv102.Checked) val += 1 << 1;
            if (chkPenOCrcv103.Checked) val += 1 << 2;
            if (chkPenOCrcv104.Checked) val += 1 << 3;
            if (chkPenOCrcv105.Checked) val += 1 << 4;
            if (chkPenOCrcv106.Checked) val += 1 << 5;
            if (chkPenOCrcv107.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B10M, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCxmit10_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCxmit101.Checked) val += 1 << 0;
            if (chkPenOCxmit102.Checked) val += 1 << 1;
            if (chkPenOCxmit103.Checked) val += 1 << 2;
            if (chkPenOCxmit104.Checked) val += 1 << 3;
            if (chkPenOCxmit105.Checked) val += 1 << 4;
            if (chkPenOCxmit106.Checked) val += 1 << 5;
            if (chkPenOCxmit107.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B10M, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }


        private void chkPenOCrcv6_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCrcv61.Checked) val += 1 << 0;
            if (chkPenOCrcv62.Checked) val += 1 << 1;
            if (chkPenOCrcv63.Checked) val += 1 << 2;
            if (chkPenOCrcv64.Checked) val += 1 << 3;
            if (chkPenOCrcv65.Checked) val += 1 << 4;
            if (chkPenOCrcv66.Checked) val += 1 << 5;
            if (chkPenOCrcv67.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B6M, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCxmit6_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCxmit61.Checked) val += 1 << 0;
            if (chkPenOCxmit62.Checked) val += 1 << 1;
            if (chkPenOCxmit63.Checked) val += 1 << 2;
            if (chkPenOCxmit64.Checked) val += 1 << 3;
            if (chkPenOCxmit65.Checked) val += 1 << 4;
            if (chkPenOCxmit66.Checked) val += 1 << 5;
            if (chkPenOCxmit67.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B6M, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcv2_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCrcv21.Checked) val += 1 << 0;
            if (chkPenOCrcv22.Checked) val += 1 << 1;
            if (chkPenOCrcv23.Checked) val += 1 << 2;
            if (chkPenOCrcv24.Checked) val += 1 << 3;
            if (chkPenOCrcv25.Checked) val += 1 << 4;
            if (chkPenOCrcv26.Checked) val += 1 << 5;
            if (chkPenOCrcv27.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B2M, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCxmit2_CheckedChanged(object sender, System.EventArgs e)
        {

            int val = 0;
            if (chkPenOCxmit21.Checked) val += 1 << 0;
            if (chkPenOCxmit22.Checked) val += 1 << 1;
            if (chkPenOCxmit23.Checked) val += 1 << 2;
            if (chkPenOCxmit24.Checked) val += 1 << 3;
            if (chkPenOCxmit25.Checked) val += 1 << 4;
            if (chkPenOCxmit26.Checked) val += 1 << 5;
            if (chkPenOCxmit27.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.B2M, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPennyExtCtrl_CheckedChanged(object sender, System.EventArgs e)
        {
            grpPennyExtCtrl.Enabled = chkPennyExtCtrl.Checked;
            grpPennyExtCtrlVHF.Enabled = chkPennyExtCtrl.Checked;
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;

#if false 		
		{
			grpExtRX.Enabled = chkExtEnable.Checked;
			grpExtTX.Enabled = chkExtEnable.Checked;
			console.ExtCtrlEnabled = chkExtEnable.Checked;
		}
#endif

        }

        private void chkAlexAntCtrl_CheckedChanged(object sender, System.EventArgs e)
        {
            grpAlexAntCtrl.Enabled = chkAlexAntCtrl.Checked;
            console.AlexAntCtrlEnabled = chkAlexAntCtrl.Checked;
        }

        private void chkMercDither_CheckedChanged(object sender, System.EventArgs e)
        {
            int v = chkMercDither.Checked ? 1 : 0;
            JanusAudio.SetMercDither(v);
        }

        private void chkMercRandom_CheckedChanged(object sender, System.EventArgs e)
        {
            int v = chkMercRandom.Checked ? 1 : 0;
            JanusAudio.SetMercRandom(v);
        }


        private RadioButtonTS[][] AlexRxAntButtons = null;
        private RadioButtonTS[][] AlexTxAntButtons = null;
        private CheckBoxTS[][] AlexRxOnlyAntCheckBoxes = null;

        private void InitAlexAntTables()
        {

            AlexRxOnlyAntCheckBoxes = new[]
                                          { 
														 new[] { chkAlex160R1, chkAlex160R2, chkAlex160XV }, 
													   	 new[] { chkAlex80R1, chkAlex80R2, chkAlex80XV }, 
													   	 new[] { chkAlex60R1, chkAlex60R2, chkAlex60XV }, 
													   	 new[] { chkAlex40R1, chkAlex40R2, chkAlex40XV }, 
													   	 new[] { chkAlex30R1, chkAlex30R2, chkAlex30XV }, 
													   	 new[] { chkAlex20R1, chkAlex20R2, chkAlex20XV }, 
													   	 new[] { chkAlex17R1, chkAlex17R2, chkAlex17XV }, 
													   	 new[] { chkAlex15R1, chkAlex15R2, chkAlex15XV }, 
													   	 new[] { chkAlex12R1, chkAlex12R2, chkAlex12XV }, 
													   	 new[] { chkAlex10R1, chkAlex10R2, chkAlex10XV }, 
													   	 new[] { chkAlex6R1, chkAlex6R2, chkAlex6XV } 
													 };


            AlexRxAntButtons = new[] { new RadioButtonTS[] { radAlexR1_160,  radAlexR2_160, radAlexR3_160 }, 
														 new[] { radAlexR1_80,  radAlexR2_80, radAlexR3_80 },
														 new[] { radAlexR1_60,  radAlexR2_60, radAlexR3_60 },
														 new[] { radAlexR1_40,  radAlexR2_40, radAlexR3_40 },
														 new[] { radAlexR1_30,  radAlexR2_30, radAlexR3_30 },
														 new[] { radAlexR1_20,  radAlexR2_20, radAlexR3_20 },
														 new[] { radAlexR1_17,  radAlexR2_17, radAlexR3_17 },
														 new[] { radAlexR1_15,  radAlexR2_15, radAlexR3_15 },
														 new[] { radAlexR1_12,  radAlexR2_12, radAlexR3_12 },
														 new[] { radAlexR1_10,  radAlexR2_10, radAlexR3_10 },
														 new[] { radAlexR1_6,  radAlexR2_6, radAlexR3_6 }
													 };

            AlexTxAntButtons = new[] { new RadioButtonTS[] { radAlexT1_160, radAlexT2_160, radAlexT3_160 }, 
														 new[] { radAlexT1_80,  radAlexT2_80, radAlexT3_80 },
														 new[] { radAlexT1_60,  radAlexT2_60, radAlexT3_60 },
														 new[] { radAlexT1_40,  radAlexT2_40, radAlexT3_40 },
														 new[] { radAlexT1_30,  radAlexT2_30, radAlexT3_30 },
														 new[] { radAlexT1_20,  radAlexT2_20, radAlexT3_20 },
														 new[] { radAlexT1_17,  radAlexT2_17, radAlexT3_17 },
														 new[] { radAlexT1_15,  radAlexT2_15, radAlexT3_15 },
														 new[] { radAlexT1_12,  radAlexT2_12, radAlexT3_12 },
														 new[] { radAlexT1_10,  radAlexT2_10, radAlexT3_10 },
														 new[] { radAlexT1_6,  radAlexT2_6, radAlexT3_6 }
													 };
        }

        public bool SetAlexAntEnabled(bool state)
        {
            bool orig_state = grpAlexAntCtrl.Enabled;
            grpAlexAntCtrl.Enabled = state;
            return orig_state;
        }

        private void radAlexR_160_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B160M, false);
        }

        private void radAlexR_80_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B80M, false);
        }

        private void radAlexR_60_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B60M, false);
        }

        private void radAlexR_40_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B40M, false);
        }

        private void radAlexR_30_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B30M, false);
        }

        private void radAlexR_20_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B20M, false);
        }

        private void radAlexR_17_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B17M, false);
        }

        private void radAlexR_15_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B15M, false);
        }

        private void radAlexR_12_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B12M, false);
        }

        private void radAlexR_10_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B10M, false);
        }

        private void radAlexR_6_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B6M, false);
        }

        private void radAlexT_160_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B160M, true);
        }

        private void radAlexT_80_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B80M, true);
        }

        private void radAlexT_60_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B60M, true);
        }

        private void radAlexT_40_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B40M, true);
        }

        private void radAlexT_30_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B30M, true);
        }

        private void radAlexT_20_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B20M, true);
        }

        private void radAlexT_17_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B17M, true);
        }

        private void radAlexT_15_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B15M, true);
        }

        private void radAlexT_12_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B12M, true);
        }

        private void radAlexT_10_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B10M, true);
        }

        private void radAlexT_6_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntRadioButton(sender, Band.B6M, true);
        }

        private void chkAlex160R_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntCheckBox(sender, Band.B160M);
        }

        private void chkAlex80R_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntCheckBox(sender, Band.B80M);
        }

        private void chkAlex60R_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntCheckBox(sender, Band.B60M);
        }

        private void chkAlex40R_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntCheckBox(sender, Band.B40M);
        }

        private void chkAlex30R_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntCheckBox(sender, Band.B30M);
        }

        private void chkAlex20R_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntCheckBox(sender, Band.B20M);
        }

        private void chkAlex17R_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntCheckBox(sender, Band.B17M);
        }

        private void chkAlex15R_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntCheckBox(sender, Band.B15M);
        }

        private void chkAlex12R_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntCheckBox(sender, Band.B12M);
        }

        private void chkAlex10R_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntCheckBox(sender, Band.B10M);
        }

        private void chkAlex6R_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntCheckBox(sender, Band.B6M);
        }

        private void chkAlex2R_CheckedChanged(object sender, System.EventArgs e)
        {
            ProcessAlexAntCheckBox(sender, Band.B2M);
        }

        private void ProcessAlexAntCheckBox(object sender, Band band)
        {

            if (sender == null) return;
            if (sender.GetType() != typeof(CheckBoxTS)) return;

            int idx = (int)band - (int)Band.B160M;

            CheckBoxTS[] cboxes = AlexRxOnlyAntCheckBoxes[idx];


            int ant = 0;
            int i;

            // find which button was changed 
            for (i = 0; i < 3; i++)
            {
                if (sender.Equals(cboxes[i]))
                {
                    if (cboxes[i].Checked)
                    {
                        ant = i + 1;
                    }
                    break;
                }
            }
            if (i == 3)
            {
                System.Console.WriteLine("internal error - did not find sender");
                return;
            }

            if (ant != 0)
            {
                // turn off unselected antenna 
                for (i = 1; i <= 3; i++)
                {
                    if (ant != i)
                    {
                        cboxes[i - 1].Checked = false;
                    }
                }
            }
            Alex.getAlex().setRxOnlyAnt(band, (byte)ant);
            console.AlexAntCtrlEnabled = true; // need side effect of prop set to push data down to C code 
            return;
        }


        private void ProcessAlexAntRadioButton(object sender, Band band, bool is_xmit)
        {
            if (sender == null) return;
            if (sender.GetType() != typeof(RadioButtonTS)) return;
            RadioButtonTS radBtnTS = (RadioButtonTS)sender;
            if (!radBtnTS.Checked) return;

            int idx = (int)band - (int)Band.B160M;

            RadioButtonTS[] buttons = is_xmit ? AlexTxAntButtons[idx] : AlexRxAntButtons[idx];

            int ant = 0;

            // find which button was changed 
            for (int i = 0; i < 3; i++)
            {
                if (!sender.Equals(buttons[i])) continue;
                ant = i + 1;
                break;
            }


            if (ant == 0)
            {
                int i = 0;
                foreach (RadioButtonTS b in buttons)
                {
                    if (b.Checked)
                    {
                        ant = i + 1;
                        break;
                    }
                    i++;
                }


                // System.Console.WriteLine("internal error - did not find sender");
                //  return;
            }

            if (is_xmit)
            {
                Alex.getAlex().setTxAnt(band, (byte)ant);
            }
            else
            {
                Alex.getAlex().setRxAnt(band, (byte)ant);
            }

            console.AlexAntCtrlEnabled = true; // need side effect of prop set to push data down to C code 

            return;
        }

        private void btnHPSDRFreqCalReset_Click(object sender, System.EventArgs e)
        {
            HPSDRFreqCorrectFactor = 1.0;
            if (console != null)
            {
                if (console.PowerOn)
                {
                    console.PowerOn = false;
                    Thread.Sleep(100);
                    console.PowerOn = true;
                }
            }
        }

        private void tpHPSDR_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (console.PowerOn && !radGenModelHermes.Checked)
            {
                lblMercury2FWVer.Visible = console.rx2_preamp_present;

                try // this will take an exception in the conversion for Metis .. 
                {
                    lblOzyFX2.Text = Convert.ToUInt32(JanusAudio.getFX2FirmwareVersionString()).ToString("Ozy FX2: 0000 00 00");
                }
                catch (Exception)
                {
                    lblOzyFX2.Text = "Ozy FX2: n/a";
                }
                if (console.HPSDRisMetis)
                {
                    lblOzyFWVer.Text = JanusAudio.getOzyFWVersion().ToString("Metis: 0\\.0");
                    lblOzyFX2.Text = "";
                }
                else
                {
                    lblOzyFWVer.Text = JanusAudio.getOzyFWVersion().ToString("Ozy: 0\\.0");
                }
                lblMercuryFWVer.Text = console.MercuryPresent ? JanusAudio.getMercuryFWVersion().ToString("Mercury: 0\\.0") : "Mercury: n/a";
                lblMercury2FWVer.Text = console.MercuryPresent ? JanusAudio.getMercury2FWVersion().ToString("Mercury2: 0\\.0") : "Mercury2: n/a";
                if (console.PennyPresent || console.PennyLanePresent)
                    lblPenelopeFWVer.Text = JanusAudio.getPenelopeFWVersion().ToString("Penny[Lane]: 0\\.0");
                else lblPenelopeFWVer.Text = "Penny[Lane]: n/a";

                /*               int rc = JanusAudio.GetHaveSync();
                               if (rc == 1)
                                   lblSyncData.Text = "FrameSync: Yes";
                               else
                                   lblSyncData.Text = "FrameSync: No"; 
               */
            }
            else
            {
                byte[] ver_bytes = new byte[1];
                JanusAudio.GetMetisCodeVersion(ver_bytes);
                lblOzyFX2.Text = ver_bytes[0].ToString("Hermes: 0\\.0");
                lblOzyFWVer.Text = "";
                lblMercuryFWVer.Text = "";
                lblMercury2FWVer.Text = "";
                lblPenelopeFWVer.Text = "";
            }
        }

        private void tpGeneralHardware_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int metis_ip_addr = JanusAudio.GetMetisIPAddr();
            lblMetisIP.Text = IPStringFromInt(metis_ip_addr);
            byte[] mac_bytes = new byte[6];
            byte[] ver_bytes = new byte[1];
            byte[] id_bytes = new byte[1];
            JanusAudio.GetMetisMACAddr(mac_bytes);
            lblMetisMAC.Text = BitConverter.ToString(mac_bytes).Replace("-", ":").ToLower();
            JanusAudio.GetMetisCodeVersion(ver_bytes);
            // lblMetisCodeVersion.Text = BitConverter.ToString(ver_bytes);
            lblMetisCodeVersion.Text = ver_bytes[0].ToString("0\\.0");
            JanusAudio.GetMetisBoardID(id_bytes);
            lblMetisBoardID.Text = BitConverter.ToString(id_bytes);
            return;
        }

        private void udMaxFreq_ValueChanged(object sender, System.EventArgs e)
        {
            console.MaxFreq = (double)udMaxFreq.Value;
        }

        private void udHPSDRFreqCorrectFactor_ValueChanged(object sender, System.EventArgs e)
        {
            JanusAudio.freqCorrectionChanged();
        }

        private void udFXtal_ValueChanged_1(object sender, System.EventArgs e) // modif F8CHK
        {
            console.SI570FXtal = (double)udFXtal.Value;
        }

        private void chkGeneralUseSi570_CheckedChanged(object sender, EventArgs e) // modif F8CHK
        {
            console.si570_used = chkGeneralUseSi570.Checked;
            grpHWSoftRock.Visible = !chkGeneralUseSi570.Checked;
            grpSI570.Visible = chkGeneralUseSi570.Checked;
        }

        private void chkHERCULES_CheckedChanged(object sender, EventArgs e)
        {
            switch (chkHERCULES.Checked)
            {
                case true:
                    chkPenOCrcv1601.Checked = true;
                    chkPenOCxmit1601.Checked = true;
                    chkPenOCrcv802.Checked = true;
                    chkPenOCxmit802.Checked = true;
                    chkPenOCrcv601.Checked = true;
                    chkPenOCxmit601.Checked = true;
                    chkPenOCrcv602.Checked = true;
                    chkPenOCxmit602.Checked = true;
                    chkPenOCrcv403.Checked = true;
                    chkPenOCxmit403.Checked = true;
                    chkPenOCrcv301.Checked = true;
                    chkPenOCxmit301.Checked = true;
                    chkPenOCrcv303.Checked = true;
                    chkPenOCxmit303.Checked = true;
                    chkPenOCrcv202.Checked = true;
                    chkPenOCxmit202.Checked = true;
                    chkPenOCrcv203.Checked = true;
                    chkPenOCxmit203.Checked = true;
                    chkPenOCrcv171.Checked = true;
                    chkPenOCxmit171.Checked = true;
                    chkPenOCrcv172.Checked = true;
                    chkPenOCxmit172.Checked = true;
                    chkPenOCrcv173.Checked = true;
                    chkPenOCxmit173.Checked = true;
                    chkPenOCrcv154.Checked = true;
                    chkPenOCxmit154.Checked = true;
                    chkPenOCrcv121.Checked = true;
                    chkPenOCxmit121.Checked = true;
                    chkPenOCrcv124.Checked = true;
                    chkPenOCxmit124.Checked = true;
                    chkPenOCrcv102.Checked = true;
                    chkPenOCxmit102.Checked = true;
                    chkPenOCrcv104.Checked = true;
                    chkPenOCxmit104.Checked = true;
                    chkPenOCrcv61.Checked = true;
                    chkPenOCxmit61.Checked = true;
                    chkPenOCrcv62.Checked = true;
                    chkPenOCxmit62.Checked = true;
                    chkPenOCrcv64.Checked = true;
                    chkPenOCxmit64.Checked = true;
                    chkPenOCrcv66.Checked = true;
                    break;
                case false:
                    chkPenOCrcv1601.Checked = false;
                    chkPenOCxmit1601.Checked = false;
                    chkPenOCrcv802.Checked = false;
                    chkPenOCxmit802.Checked = false;
                    chkPenOCrcv601.Checked = false;
                    chkPenOCxmit601.Checked = false;
                    chkPenOCrcv602.Checked = false;
                    chkPenOCxmit602.Checked = false;
                    chkPenOCrcv403.Checked = false;
                    chkPenOCxmit403.Checked = false;
                    chkPenOCrcv301.Checked = false;
                    chkPenOCxmit301.Checked = false;
                    chkPenOCrcv303.Checked = false;
                    chkPenOCxmit303.Checked = false;
                    chkPenOCrcv202.Checked = false;
                    chkPenOCxmit202.Checked = false;
                    chkPenOCrcv203.Checked = false;
                    chkPenOCxmit203.Checked = false;
                    chkPenOCrcv171.Checked = false;
                    chkPenOCxmit171.Checked = false;
                    chkPenOCrcv172.Checked = false;
                    chkPenOCxmit172.Checked = false;
                    chkPenOCrcv173.Checked = false;
                    chkPenOCxmit173.Checked = false;
                    chkPenOCrcv154.Checked = false;
                    chkPenOCxmit154.Checked = false;
                    chkPenOCrcv121.Checked = false;
                    chkPenOCxmit121.Checked = false;
                    chkPenOCrcv124.Checked = false;
                    chkPenOCxmit124.Checked = false;
                    chkPenOCrcv102.Checked = false;
                    chkPenOCxmit102.Checked = false;
                    chkPenOCrcv104.Checked = false;
                    chkPenOCxmit104.Checked = false;
                    chkPenOCrcv61.Checked = false;
                    chkPenOCxmit61.Checked = false;
                    chkPenOCrcv62.Checked = false;
                    chkPenOCxmit62.Checked = false;
                    chkPenOCrcv64.Checked = false;
                    chkPenOCxmit64.Checked = false;
                    chkPenOCrcv66.Checked = false;
                    break;
            }
        }

        private void btnPennyCtrlReset_Click(object sender, EventArgs e)
        {
            switch (chkHERCULES.Checked)
            {
                case false:
                    chkPenOCrcv1601.Checked = false;
                    chkPenOCxmit1601.Checked = false;
                    chkPenOCrcv1602.Checked = false;
                    chkPenOCxmit1602.Checked = false;
                    chkPenOCrcv1603.Checked = false;
                    chkPenOCxmit1603.Checked = false;
                    chkPenOCrcv1604.Checked = false;
                    chkPenOCxmit1604.Checked = false;
                    chkPenOCrcv1605.Checked = false;
                    chkPenOCxmit1605.Checked = false;
                    chkPenOCrcv1606.Checked = false;
                    chkPenOCxmit1606.Checked = false;
                    chkPenOCrcv1607.Checked = false;
                    chkPenOCxmit1607.Checked = false;
                    chkPenOCrcv801.Checked = false;
                    chkPenOCxmit801.Checked = false;
                    chkPenOCrcv802.Checked = false;
                    chkPenOCxmit802.Checked = false;
                    chkPenOCrcv803.Checked = false;
                    chkPenOCxmit803.Checked = false;
                    chkPenOCrcv804.Checked = false;
                    chkPenOCxmit804.Checked = false;
                    chkPenOCrcv805.Checked = false;
                    chkPenOCxmit805.Checked = false;
                    chkPenOCrcv806.Checked = false;
                    chkPenOCxmit806.Checked = false;
                    chkPenOCrcv807.Checked = false;
                    chkPenOCxmit807.Checked = false;
                    chkPenOCrcv601.Checked = false;
                    chkPenOCxmit601.Checked = false;
                    chkPenOCrcv602.Checked = false;
                    chkPenOCxmit602.Checked = false;
                    chkPenOCrcv603.Checked = false;
                    chkPenOCxmit603.Checked = false;
                    chkPenOCrcv604.Checked = false;
                    chkPenOCxmit604.Checked = false;
                    chkPenOCrcv605.Checked = false;
                    chkPenOCxmit605.Checked = false;
                    chkPenOCrcv606.Checked = false;
                    chkPenOCxmit606.Checked = false;
                    chkPenOCrcv607.Checked = false;
                    chkPenOCxmit607.Checked = false;
                    chkPenOCrcv401.Checked = false;
                    chkPenOCxmit401.Checked = false;
                    chkPenOCrcv402.Checked = false;
                    chkPenOCxmit402.Checked = false;
                    chkPenOCrcv403.Checked = false;
                    chkPenOCxmit403.Checked = false;
                    chkPenOCrcv404.Checked = false;
                    chkPenOCxmit404.Checked = false;
                    chkPenOCrcv405.Checked = false;
                    chkPenOCxmit405.Checked = false;
                    chkPenOCrcv406.Checked = false;
                    chkPenOCxmit406.Checked = false;
                    chkPenOCrcv407.Checked = false;
                    chkPenOCxmit407.Checked = false;
                    chkPenOCrcv301.Checked = false;
                    chkPenOCxmit301.Checked = false;
                    chkPenOCrcv302.Checked = false;
                    chkPenOCxmit302.Checked = false;
                    chkPenOCrcv303.Checked = false;
                    chkPenOCxmit303.Checked = false;
                    chkPenOCrcv304.Checked = false;
                    chkPenOCxmit304.Checked = false;
                    chkPenOCrcv305.Checked = false;
                    chkPenOCxmit305.Checked = false;
                    chkPenOCrcv306.Checked = false;
                    chkPenOCxmit306.Checked = false;
                    chkPenOCrcv307.Checked = false;
                    chkPenOCxmit307.Checked = false;
                    chkPenOCrcv201.Checked = false;
                    chkPenOCxmit201.Checked = false;
                    chkPenOCrcv202.Checked = false;
                    chkPenOCxmit202.Checked = false;
                    chkPenOCrcv203.Checked = false;
                    chkPenOCxmit203.Checked = false;
                    chkPenOCrcv204.Checked = false;
                    chkPenOCxmit204.Checked = false;
                    chkPenOCrcv205.Checked = false;
                    chkPenOCxmit205.Checked = false;
                    chkPenOCrcv206.Checked = false;
                    chkPenOCxmit206.Checked = false;
                    chkPenOCrcv207.Checked = false;
                    chkPenOCxmit207.Checked = false;
                    chkPenOCrcv171.Checked = false;
                    chkPenOCxmit171.Checked = false;
                    chkPenOCrcv172.Checked = false;
                    chkPenOCxmit172.Checked = false;
                    chkPenOCrcv173.Checked = false;
                    chkPenOCxmit173.Checked = false;
                    chkPenOCrcv174.Checked = false;
                    chkPenOCxmit174.Checked = false;
                    chkPenOCrcv175.Checked = false;
                    chkPenOCxmit175.Checked = false;
                    chkPenOCrcv176.Checked = false;
                    chkPenOCxmit176.Checked = false;
                    chkPenOCrcv177.Checked = false;
                    chkPenOCxmit177.Checked = false;
                    chkPenOCrcv151.Checked = false;
                    chkPenOCxmit151.Checked = false;
                    chkPenOCrcv152.Checked = false;
                    chkPenOCxmit152.Checked = false;
                    chkPenOCrcv153.Checked = false;
                    chkPenOCxmit153.Checked = false;
                    chkPenOCrcv154.Checked = false;
                    chkPenOCxmit154.Checked = false;
                    chkPenOCrcv155.Checked = false;
                    chkPenOCxmit155.Checked = false;
                    chkPenOCrcv156.Checked = false;
                    chkPenOCxmit156.Checked = false;
                    chkPenOCrcv157.Checked = false;
                    chkPenOCxmit157.Checked = false;
                    chkPenOCrcv121.Checked = false;
                    chkPenOCxmit121.Checked = false;
                    chkPenOCrcv122.Checked = false;
                    chkPenOCxmit122.Checked = false;
                    chkPenOCrcv123.Checked = false;
                    chkPenOCxmit123.Checked = false;
                    chkPenOCrcv124.Checked = false;
                    chkPenOCxmit124.Checked = false;
                    chkPenOCrcv125.Checked = false;
                    chkPenOCxmit125.Checked = false;
                    chkPenOCrcv126.Checked = false;
                    chkPenOCxmit126.Checked = false;
                    chkPenOCrcv127.Checked = false;
                    chkPenOCxmit127.Checked = false;
                    chkPenOCrcv101.Checked = false;
                    chkPenOCxmit101.Checked = false;
                    chkPenOCrcv102.Checked = false;
                    chkPenOCxmit102.Checked = false;
                    chkPenOCrcv103.Checked = false;
                    chkPenOCxmit103.Checked = false;
                    chkPenOCrcv104.Checked = false;
                    chkPenOCxmit104.Checked = false;
                    chkPenOCrcv105.Checked = false;
                    chkPenOCxmit105.Checked = false;
                    chkPenOCrcv106.Checked = false;
                    chkPenOCxmit106.Checked = false;
                    chkPenOCrcv107.Checked = false;
                    chkPenOCxmit107.Checked = false;
                    chkPenOCrcv61.Checked = false;
                    chkPenOCxmit61.Checked = false;
                    chkPenOCrcv62.Checked = false;
                    chkPenOCxmit62.Checked = false;
                    chkPenOCrcv63.Checked = false;
                    chkPenOCxmit63.Checked = false;
                    chkPenOCrcv64.Checked = false;
                    chkPenOCxmit64.Checked = false;
                    chkPenOCrcv65.Checked = false;
                    chkPenOCxmit65.Checked = false;
                    chkPenOCrcv66.Checked = false;
                    chkPenOCxmit66.Checked = false;
                    chkPenOCrcv67.Checked = false;
                    chkPenOCxmit67.Checked = false;
                    chkPenOCrcv21.Checked = false;
                    chkPenOCxmit21.Checked = false;
                    chkPenOCrcv22.Checked = false;
                    chkPenOCxmit22.Checked = false;
                    chkPenOCrcv23.Checked = false;
                    chkPenOCxmit23.Checked = false;
                    chkPenOCrcv24.Checked = false;
                    chkPenOCxmit24.Checked = false;
                    chkPenOCrcv25.Checked = false;
                    chkPenOCxmit25.Checked = false;
                    chkPenOCrcv26.Checked = false;
                    chkPenOCxmit26.Checked = false;
                    chkPenOCrcv27.Checked = false;
                    chkPenOCxmit27.Checked = false;
                    break;
            }
        }

        private void btnPennyCtrlVHFReset_Click(object sender, EventArgs e)
        {
            chkPenOCrcvVHF01.Checked = false;
            chkPenOCxmitVHF01.Checked = false;
            chkPenOCrcvVHF02.Checked = false;
            chkPenOCxmitVHF02.Checked = false;
            chkPenOCrcvVHF03.Checked = false;
            chkPenOCxmitVHF03.Checked = false;
            chkPenOCrcvVHF04.Checked = false;
            chkPenOCxmitVHF04.Checked = false;
            chkPenOCrcvVHF05.Checked = false;
            chkPenOCxmitVHF05.Checked = false;
            chkPenOCrcvVHF06.Checked = false;
            chkPenOCxmitVHF06.Checked = false;
            chkPenOCrcvVHF07.Checked = false;
            chkPenOCxmitVHF07.Checked = false;

            chkPenOCrcvVHF11.Checked = false;
            chkPenOCxmitVHF11.Checked = false;
            chkPenOCrcvVHF12.Checked = false;
            chkPenOCxmitVHF12.Checked = false;
            chkPenOCrcvVHF13.Checked = false;
            chkPenOCxmitVHF13.Checked = false;
            chkPenOCrcvVHF14.Checked = false;
            chkPenOCxmitVHF14.Checked = false;
            chkPenOCrcvVHF15.Checked = false;
            chkPenOCxmitVHF15.Checked = false;
            chkPenOCrcvVHF16.Checked = false;
            chkPenOCxmitVHF16.Checked = false;
            chkPenOCrcvVHF17.Checked = false;
            chkPenOCxmitVHF17.Checked = false;

            chkPenOCrcvVHF21.Checked = false;
            chkPenOCxmitVHF21.Checked = false;
            chkPenOCrcvVHF22.Checked = false;
            chkPenOCxmitVHF22.Checked = false;
            chkPenOCrcvVHF23.Checked = false;
            chkPenOCxmitVHF23.Checked = false;
            chkPenOCrcvVHF24.Checked = false;
            chkPenOCxmitVHF24.Checked = false;
            chkPenOCrcvVHF25.Checked = false;
            chkPenOCxmitVHF25.Checked = false;
            chkPenOCrcvVHF26.Checked = false;
            chkPenOCxmitVHF26.Checked = false;
            chkPenOCrcvVHF27.Checked = false;
            chkPenOCxmitVHF27.Checked = false;

            chkPenOCrcvVHF31.Checked = false;
            chkPenOCxmitVHF31.Checked = false;
            chkPenOCrcvVHF32.Checked = false;
            chkPenOCxmitVHF32.Checked = false;
            chkPenOCrcvVHF33.Checked = false;
            chkPenOCxmitVHF33.Checked = false;
            chkPenOCrcvVHF34.Checked = false;
            chkPenOCxmitVHF34.Checked = false;
            chkPenOCrcvVHF35.Checked = false;
            chkPenOCxmitVHF35.Checked = false;
            chkPenOCrcvVHF36.Checked = false;
            chkPenOCxmitVHF36.Checked = false;
            chkPenOCrcvVHF37.Checked = false;
            chkPenOCxmitVHF37.Checked = false;

            chkPenOCrcvVHF41.Checked = false;
            chkPenOCxmitVHF41.Checked = false;
            chkPenOCrcvVHF42.Checked = false;
            chkPenOCxmitVHF42.Checked = false;
            chkPenOCrcvVHF43.Checked = false;
            chkPenOCxmitVHF43.Checked = false;
            chkPenOCrcvVHF44.Checked = false;
            chkPenOCxmitVHF44.Checked = false;
            chkPenOCrcvVHF45.Checked = false;
            chkPenOCxmitVHF45.Checked = false;
            chkPenOCrcvVHF46.Checked = false;
            chkPenOCxmitVHF46.Checked = false;
            chkPenOCrcvVHF47.Checked = false;
            chkPenOCxmitVHF47.Checked = false;

            chkPenOCrcvVHF51.Checked = false;
            chkPenOCxmitVHF51.Checked = false;
            chkPenOCrcvVHF52.Checked = false;
            chkPenOCxmitVHF52.Checked = false;
            chkPenOCrcvVHF53.Checked = false;
            chkPenOCxmitVHF53.Checked = false;
            chkPenOCrcvVHF54.Checked = false;
            chkPenOCxmitVHF54.Checked = false;
            chkPenOCrcvVHF55.Checked = false;
            chkPenOCxmitVHF55.Checked = false;
            chkPenOCrcvVHF56.Checked = false;
            chkPenOCxmitVHF56.Checked = false;
            chkPenOCrcvVHF57.Checked = false;
            chkPenOCxmitVHF57.Checked = false;

            chkPenOCrcvVHF61.Checked = false;
            chkPenOCxmitVHF61.Checked = false;
            chkPenOCrcvVHF62.Checked = false;
            chkPenOCxmitVHF62.Checked = false;
            chkPenOCrcvVHF63.Checked = false;
            chkPenOCxmitVHF63.Checked = false;
            chkPenOCrcvVHF64.Checked = false;
            chkPenOCxmitVHF64.Checked = false;
            chkPenOCrcvVHF65.Checked = false;
            chkPenOCxmitVHF65.Checked = false;
            chkPenOCrcvVHF66.Checked = false;
            chkPenOCxmitVHF66.Checked = false;
            chkPenOCrcvVHF67.Checked = false;
            chkPenOCxmitVHF67.Checked = false;

            chkPenOCrcvVHF71.Checked = false;
            chkPenOCxmitVHF71.Checked = false;
            chkPenOCrcvVHF72.Checked = false;
            chkPenOCxmitVHF72.Checked = false;
            chkPenOCrcvVHF73.Checked = false;
            chkPenOCxmitVHF73.Checked = false;
            chkPenOCrcvVHF74.Checked = false;
            chkPenOCxmitVHF74.Checked = false;
            chkPenOCrcvVHF75.Checked = false;
            chkPenOCxmitVHF75.Checked = false;
            chkPenOCrcvVHF76.Checked = false;
            chkPenOCxmitVHF76.Checked = false;
            chkPenOCrcvVHF77.Checked = false;
            chkPenOCxmitVHF77.Checked = false;

            chkPenOCrcvVHF81.Checked = false;
            chkPenOCxmitVHF81.Checked = false;
            chkPenOCrcvVHF82.Checked = false;
            chkPenOCxmitVHF82.Checked = false;
            chkPenOCrcvVHF83.Checked = false;
            chkPenOCxmitVHF83.Checked = false;
            chkPenOCrcvVHF84.Checked = false;
            chkPenOCxmitVHF84.Checked = false;
            chkPenOCrcvVHF85.Checked = false;
            chkPenOCxmitVHF85.Checked = false;
            chkPenOCrcvVHF86.Checked = false;
            chkPenOCxmitVHF86.Checked = false;
            chkPenOCrcvVHF87.Checked = false;
            chkPenOCxmitVHF87.Checked = false;

            chkPenOCrcvVHF91.Checked = false;
            chkPenOCxmitVHF91.Checked = false;
            chkPenOCrcvVHF92.Checked = false;
            chkPenOCxmitVHF92.Checked = false;
            chkPenOCrcvVHF93.Checked = false;
            chkPenOCxmitVHF93.Checked = false;
            chkPenOCrcvVHF94.Checked = false;
            chkPenOCxmitVHF94.Checked = false;
            chkPenOCrcvVHF95.Checked = false;
            chkPenOCxmitVHF95.Checked = false;
            chkPenOCrcvVHF96.Checked = false;
            chkPenOCxmitVHF96.Checked = false;
            chkPenOCrcvVHF97.Checked = false;
            chkPenOCxmitVHF97.Checked = false;

            chkPenOCrcvVHF101.Checked = false;
            chkPenOCxmitVHF101.Checked = false;
            chkPenOCrcvVHF102.Checked = false;
            chkPenOCxmitVHF102.Checked = false;
            chkPenOCrcvVHF103.Checked = false;
            chkPenOCxmitVHF103.Checked = false;
            chkPenOCrcvVHF104.Checked = false;
            chkPenOCxmitVHF104.Checked = false;
            chkPenOCrcvVHF105.Checked = false;
            chkPenOCxmitVHF105.Checked = false;
            chkPenOCrcvVHF106.Checked = false;
            chkPenOCxmitVHF106.Checked = false;
            chkPenOCrcvVHF107.Checked = false;
            chkPenOCxmitVHF107.Checked = false;

            chkPenOCrcvVHF111.Checked = false;
            chkPenOCxmitVHF111.Checked = false;
            chkPenOCrcvVHF112.Checked = false;
            chkPenOCxmitVHF112.Checked = false;
            chkPenOCrcvVHF113.Checked = false;
            chkPenOCxmitVHF113.Checked = false;
            chkPenOCrcvVHF114.Checked = false;
            chkPenOCxmitVHF114.Checked = false;
            chkPenOCrcvVHF115.Checked = false;
            chkPenOCxmitVHF115.Checked = false;
            chkPenOCrcvVHF116.Checked = false;
            chkPenOCxmitVHF116.Checked = false;
            chkPenOCrcvVHF117.Checked = false;
            chkPenOCxmitVHF117.Checked = false;
        }

        private void comboFRSRegion_SelectedIndexChanged(object sender, EventArgs e) //w5wc
        {
            FRSRegion CurrentRegion = FRSRegion.US;
            if (comboFRSRegion.Text == "") return;
            switch (comboFRSRegion.Text)
            {
                case "Australia":
                    CurrentRegion = FRSRegion.Australia;
                    console.Extended = false;
                    break;
                case "Europe":
                    CurrentRegion = FRSRegion.Europe;
                    console.Extended = false;
                    break;
                case "Italy":
                    CurrentRegion = FRSRegion.Italy_Plus;
                    console.Extended = false;
                    break;
                case "Spain":
                    CurrentRegion = FRSRegion.Spain;
                    console.Extended = false;
                    break;
                case "Japan":
                    CurrentRegion = FRSRegion.Japan;
                    console.Extended = false;
                    break;
                case "United Kingdom":
                    CurrentRegion = FRSRegion.UK;
                    console.Extended = false;
                    break;
                case "United States":
                    CurrentRegion = FRSRegion.US;
                    console.Extended = false;
                    break;
                case "Norway":
                    CurrentRegion = FRSRegion.Norway;
                    console.Extended = false;
                    break;
                case "Denmark":
                    CurrentRegion = FRSRegion.Denmark;
                    console.Extended = false;
                    break;
                case "Latvia":
                    CurrentRegion = FRSRegion.Latvia;
                    console.Extended = false;
                    break;
                case "Slovakia":
                    CurrentRegion = FRSRegion.Slovakia;
                    console.Extended = false;
                    break;
                case "Bulgaria":
                    CurrentRegion = FRSRegion.Bulgaria;
                    console.Extended = false;
                    break;
                case "Greece":
                    CurrentRegion = FRSRegion.Greece;
                    console.Extended = false;
                    break;
                case "Hungary":
                    CurrentRegion = FRSRegion.Hungary;
                    console.Extended = false;
                    break;
                case "Netherlands":
                    CurrentRegion = FRSRegion.Netherlands;
                    console.Extended = false;
                    break;
                case "France":
                    CurrentRegion = FRSRegion.France;
                    console.Extended = false;
                    break;
                case "Extended":
                    console.Extended = true;
                    break;
            }
            console.CurrentRegion = CurrentRegion;
            console.Init60mChannels();
        }

        private void radMicIn_CheckedChanged(object sender, EventArgs e)
        {
            if (!radMicIn.Checked) return;
            console.LineIn = false;
            radLineIn.Checked = false;
            chk20dbMicBoost.Visible = true;
            chk20dbMicBoost.Enabled = true;
            lblLineInBoost.Visible = false;
            udLineInBoost.Visible = false;
            //console.SetMicGain();
        }

        private void radLineIn_CheckedChanged(object sender, EventArgs e)
        {
            if (!radLineIn.Checked) return;
            console.LineIn = true;
            radMicIn.Checked = false;
            chk20dbMicBoost.Visible = false;
           // chk20dbMicBoost.Checked = false;
            chk20dbMicBoost.Enabled = false;
            //           if (!console.HPSDRisMetis)
            //            {
            lblLineInBoost.Visible = true;
            udLineInBoost.Visible = true;
            //            }
            //            else
            //            {
            //                lblLineInBoost.Visible = false;
            //                udLineInBoost.Visible = false;
            //           }
            //console.SetMicGain();
        }

        private void radOzyUSB_CheckedChanged(object sender, EventArgs e)
        {
            if (radOzyUSB.Checked)
            {
                radMetis.Checked = false;
                console.HPSDRisMetis = false;
                grpMetisAddr.Visible = false;
                radMicIn_CheckedChanged(this, EventArgs.Empty);
                radLineIn_CheckedChanged(this, EventArgs.Empty);

            }
            else
            {
                radMetis.Checked = true;
                console.HPSDRisMetis = true;
                grpMetisAddr.Visible = true;
            }
        }

        private void radMetis_CheckedChanged(object sender, EventArgs e)
        {
            if (radMetis.Checked)
            {
                radOzyUSB.Checked = false;
                console.HPSDRisMetis = true;
                grpMetisAddr.Visible = true;
                radMicIn_CheckedChanged(this, EventArgs.Empty);
                radLineIn_CheckedChanged(this, EventArgs.Empty);
                //  lblLineInBoost.Visible = false;
                //  udLineInBoost.Visible = false;
                //  chk20dbMicBoost.Visible = true;

            }
            else
            {
                radOzyUSB.Checked = true;
                console.HPSDRisMetis = false;
                grpMetisAddr.Visible = false;
            }
        }


        private string IPStringFromInt(Int32 addr, StringBuilder sb)
        {
            sb.Length = 0;
            for (int i = 0; i < 4; i++)
            {
                int j = addr & 0xff;
                addr = addr >> 8;
                sb.Append(j);
                if (i != 3)
                {
                    sb.Append(".");
                }
            }
            return sb.ToString();
        }

        private string IPStringFromInt(Int32 addr)
        {
            return IPStringFromInt(addr, new StringBuilder());
        }

        private void udGenPTTOutDelay_ValueChanged(object sender, EventArgs e)
        {
            HW.PTTOutDelay = (int)udGenPTTOutDelay.Value;
        }

        private void chkPTTOutDelay_CheckedChanged(object sender, EventArgs e)
        {
            udGenPTTOutDelay.Enabled = chkPTTOutDelay.Checked;
            console.Hdw.PTTODelayControl = chkPTTOutDelay.Checked;
        }

        private void udMoxDelay_ValueChanged(object sender, EventArgs e)
        {
            console.MoxDelay = (int)udMoxDelay.Value;
        }

        private void udRFDelay_ValueChanged(object sender, EventArgs e)
        {
            console.RFDelay = (int)udRFDelay.Value;
        }

        private void udTXDisplayCalOffset_ValueChanged(object sender, EventArgs e)
        {
            Display.TXDisplayCalOffset = (float)udTXDisplayCalOffset.Value;
            //console.TXDisplayCalOffset = (float)udTXDisplayCalOffset.Value;
        }

        private void udTXDisplayCalOffset_LostFocus(object sender, EventArgs e)
        {
            udTXDisplayCalOffset.Value = udTXDisplayCalOffset.Value;
        }

        private void udTwoToneLevel_ValueChanged(object sender, EventArgs e)
        {
            if (chekTestIMD.Checked)
            {
                Audio.SourceScale = Math.Pow(10.0, (double)udTwoToneLevel.Value / 20.0);
            }
        }

        private void chkSMeter_CheckedChanged(object sender, EventArgs e)
        {
            console.SMeter = chkSMeter.Checked;
        }

        private void clrbtnGridFine_Changed(object sender, EventArgs e)
        {
            Display.GridPenDark = Color.FromArgb(tbGridFineAlpha.Value, clrbtnGridFine.Color);
            //Display.GridPenDark = clrbtnGridFine.Color;
        }

        private void clrbtnTXVGridFine_Changed(object sender, EventArgs e)
        {
            Display.TXVGridPenFine = Color.FromArgb(tbTXVGridFineAlpha.Value, clrbtnTXVGridFine.Color);
            //Display.GridPenDark = clrbtnGridFine.Color;
        }

        private void tbBackgroundAlpha_Scroll(object sender, EventArgs e)
        {
            clrbtnBackground_Changed(this, EventArgs.Empty);
            toolTip1.SetToolTip(tbBackgroundAlpha, tbBackgroundAlpha.Value.ToString());
        }

        private void tbTXBackgroundAlpha_Scroll(object sender, EventArgs e)
        {
            clrbtnTXBackground_Changed(this, EventArgs.Empty);
            toolTip1.SetToolTip(tbTXBackgroundAlpha, tbTXBackgroundAlpha.Value.ToString());
        }

        private void tbGridCourseAlpha_Scroll(object sender, EventArgs e)
        {
            clrbtnGrid_Changed(this, EventArgs.Empty);
            toolTip1.SetToolTip(tbGridCourseAlpha, tbGridCourseAlpha.Value.ToString());
        }

        private void tbTXVGridCourseAlpha_Scroll(object sender, EventArgs e)
        {
            clrbtnTXVGrid_Changed(this, EventArgs.Empty);
            toolTip1.SetToolTip(tbTXVGridCourseAlpha, tbTXVGridCourseAlpha.Value.ToString());
        }

        private void tbGridFineAlpha_Scroll(object sender, EventArgs e)
        {
            clrbtnGridFine_Changed(this, EventArgs.Empty);
            toolTip1.SetToolTip(tbGridFineAlpha, tbGridFineAlpha.Value.ToString());
        }

        private void tbTXVGridFineAlpha_Scroll(object sender, EventArgs e)
        {
            clrbtnTXVGridFine_Changed(this, EventArgs.Empty);
            toolTip1.SetToolTip(tbTXVGridFineAlpha, tbTXVGridFineAlpha.Value.ToString());
        }

        private void clrbtnHGridColor_Changed(object sender, EventArgs e)
        {
            Display.HGridColor = Color.FromArgb(tbHGridColorAlpha.Value, clrbtnHGridColor.Color);
        }

        private void clrbtnTXHGridColor_Changed(object sender, EventArgs e)
        {
            Display.TXHGridColor = Color.FromArgb(tbTXHGridColorAlpha.Value, clrbtnTXHGridColor.Color);
        }

        private void tbHGridColorAlpha_Scroll(object sender, EventArgs e)
        {
            clrbtnHGridColor_Changed(this, EventArgs.Empty);
            toolTip1.SetToolTip(tbHGridColorAlpha, tbHGridColorAlpha.Value.ToString());
        }

        private void tbMeterEdgeBackgroundAlpha_Scroll(object sender, EventArgs e)
        {
            clrbtnMeterEdgeBackground_Changed(this, EventArgs.Empty);
            toolTip1.SetToolTip(tbMeterEdgeBackgroundAlpha, tbMeterEdgeBackgroundAlpha.Value.ToString());
        }

        private void tbTXHGridColorAlpha_Scroll(object sender, EventArgs e)
        {
            clrbtnTXHGridColor_Changed(this, EventArgs.Empty);
            toolTip1.SetToolTip(tbTXHGridColorAlpha, tbTXHGridColorAlpha.Value.ToString());
        }

        private void tbTXZeroLineAlpha_Scroll(object sender, EventArgs e)
        {
            clrbtnTXZeroLine_Changed(this, EventArgs.Empty);
            toolTip1.SetToolTip(tbTXZeroLineAlpha, tbTXZeroLineAlpha.Value.ToString());
        }

        private void tbTXTextAlpha_Scroll(object sender, EventArgs e)
        {
            clrbtnTXText_Changed(this, EventArgs.Empty);
            toolTip1.SetToolTip(tbTXTextAlpha, tbTXTextAlpha.Value.ToString());
        }

        /* private void chkTXCal_CheckedChanged(object sender, EventArgs e)
         {
             udTXDisplayCalOffset.Enabled = chkTXCal.Checked;
             Display.TXDisplayCalControl = chkTXCal.Checked;
             //udTXDisplayCalOffset_ValueChanged(this, EventArgs.Empty);
         }*/

        private void chkGridControl_CheckedChanged(object sender, EventArgs e)
        {
            Display.GridControl = chkGridControl.Checked;
        }

        private void chkTXGridControl_CheckedChanged(object sender, EventArgs e)
        {
            Display.TXGridControl = chkTXGridControl.Checked;
        }

        private void radSpaceBarLastBtn_CheckedChanged(object sender, EventArgs e)
        {
            this.console.SpaceBarLastBtn = radSpaceBarLastBtn.Checked;
        }

        private void radSpaceBarPTT_CheckedChanged(object sender, EventArgs e)
        {
            this.console.SpaceBarPTT = radSpaceBarPTT.Checked;
        }

        private void radSpaceBarVOX_CheckedChanged(object sender, EventArgs e)
        {
            this.console.SpaceBarVOX = radSpaceBarVOX.Checked;
        }

        private void radSpaceBarMicMute_CheckedChanged(object sender, EventArgs e)
        {
            this.console.SpaceBarMicMute = radSpaceBarMicMute.Checked;
        }

        private void chkPenOCrcvVHF0_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCrcvVHF01.Checked) val += 1 << 0;
            if (chkPenOCrcvVHF02.Checked) val += 1 << 1;
            if (chkPenOCrcvVHF03.Checked) val += 1 << 2;
            if (chkPenOCrcvVHF04.Checked) val += 1 << 3;
            if (chkPenOCrcvVHF05.Checked) val += 1 << 4;
            if (chkPenOCrcvVHF06.Checked) val += 1 << 5;
            if (chkPenOCrcvVHF07.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF0, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcvVHF1_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCrcvVHF11.Checked) val += 1 << 0;
            if (chkPenOCrcvVHF12.Checked) val += 1 << 1;
            if (chkPenOCrcvVHF13.Checked) val += 1 << 2;
            if (chkPenOCrcvVHF14.Checked) val += 1 << 3;
            if (chkPenOCrcvVHF15.Checked) val += 1 << 4;
            if (chkPenOCrcvVHF16.Checked) val += 1 << 5;
            if (chkPenOCrcvVHF17.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF1, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcvVHF2_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCrcvVHF21.Checked) val += 1 << 0;
            if (chkPenOCrcvVHF22.Checked) val += 1 << 1;
            if (chkPenOCrcvVHF23.Checked) val += 1 << 2;
            if (chkPenOCrcvVHF24.Checked) val += 1 << 3;
            if (chkPenOCrcvVHF25.Checked) val += 1 << 4;
            if (chkPenOCrcvVHF26.Checked) val += 1 << 5;
            if (chkPenOCrcvVHF27.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF2, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcvVHF3_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCrcvVHF31.Checked) val += 1 << 0;
            if (chkPenOCrcvVHF32.Checked) val += 1 << 1;
            if (chkPenOCrcvVHF33.Checked) val += 1 << 2;
            if (chkPenOCrcvVHF34.Checked) val += 1 << 3;
            if (chkPenOCrcvVHF35.Checked) val += 1 << 4;
            if (chkPenOCrcvVHF36.Checked) val += 1 << 5;
            if (chkPenOCrcvVHF37.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF3, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcvVHF4_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCrcvVHF41.Checked) val += 1 << 0;
            if (chkPenOCrcvVHF42.Checked) val += 1 << 1;
            if (chkPenOCrcvVHF43.Checked) val += 1 << 2;
            if (chkPenOCrcvVHF44.Checked) val += 1 << 3;
            if (chkPenOCrcvVHF45.Checked) val += 1 << 4;
            if (chkPenOCrcvVHF46.Checked) val += 1 << 5;
            if (chkPenOCrcvVHF47.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF4, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcvVHF5_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCrcvVHF51.Checked) val += 1 << 0;
            if (chkPenOCrcvVHF52.Checked) val += 1 << 1;
            if (chkPenOCrcvVHF53.Checked) val += 1 << 2;
            if (chkPenOCrcvVHF54.Checked) val += 1 << 3;
            if (chkPenOCrcvVHF55.Checked) val += 1 << 4;
            if (chkPenOCrcvVHF56.Checked) val += 1 << 5;
            if (chkPenOCrcvVHF57.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF5, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcvVHF6_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCrcvVHF61.Checked) val += 1 << 0;
            if (chkPenOCrcvVHF62.Checked) val += 1 << 1;
            if (chkPenOCrcvVHF63.Checked) val += 1 << 2;
            if (chkPenOCrcvVHF64.Checked) val += 1 << 3;
            if (chkPenOCrcvVHF65.Checked) val += 1 << 4;
            if (chkPenOCrcvVHF66.Checked) val += 1 << 5;
            if (chkPenOCrcvVHF67.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF6, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcvVHF7_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCrcvVHF71.Checked) val += 1 << 0;
            if (chkPenOCrcvVHF72.Checked) val += 1 << 1;
            if (chkPenOCrcvVHF73.Checked) val += 1 << 2;
            if (chkPenOCrcvVHF74.Checked) val += 1 << 3;
            if (chkPenOCrcvVHF75.Checked) val += 1 << 4;
            if (chkPenOCrcvVHF76.Checked) val += 1 << 5;
            if (chkPenOCrcvVHF77.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF7, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCrcvVHF8_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCrcvVHF81.Checked) val += 1 << 0;
            if (chkPenOCrcvVHF82.Checked) val += 1 << 1;
            if (chkPenOCrcvVHF83.Checked) val += 1 << 2;
            if (chkPenOCrcvVHF84.Checked) val += 1 << 3;
            if (chkPenOCrcvVHF85.Checked) val += 1 << 4;
            if (chkPenOCrcvVHF86.Checked) val += 1 << 5;
            if (chkPenOCrcvVHF87.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF8, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code  
        }

        private void chkPenOCrcvVHF9_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCrcvVHF91.Checked) val += 1 << 0;
            if (chkPenOCrcvVHF92.Checked) val += 1 << 1;
            if (chkPenOCrcvVHF93.Checked) val += 1 << 2;
            if (chkPenOCrcvVHF94.Checked) val += 1 << 3;
            if (chkPenOCrcvVHF95.Checked) val += 1 << 4;
            if (chkPenOCrcvVHF96.Checked) val += 1 << 5;
            if (chkPenOCrcvVHF97.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF9, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code  
        }

        private void chkPenOCrcvVHF10_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCrcvVHF101.Checked) val += 1 << 0;
            if (chkPenOCrcvVHF102.Checked) val += 1 << 1;
            if (chkPenOCrcvVHF103.Checked) val += 1 << 2;
            if (chkPenOCrcvVHF104.Checked) val += 1 << 3;
            if (chkPenOCrcvVHF105.Checked) val += 1 << 4;
            if (chkPenOCrcvVHF106.Checked) val += 1 << 5;
            if (chkPenOCrcvVHF107.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF10, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code  
        }

        private void chkPenOCrcvVHF11_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCrcvVHF111.Checked) val += 1 << 0;
            if (chkPenOCrcvVHF112.Checked) val += 1 << 1;
            if (chkPenOCrcvVHF113.Checked) val += 1 << 2;
            if (chkPenOCrcvVHF114.Checked) val += 1 << 3;
            if (chkPenOCrcvVHF115.Checked) val += 1 << 4;
            if (chkPenOCrcvVHF116.Checked) val += 1 << 5;
            if (chkPenOCrcvVHF117.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF11, (byte)val, false);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code 
        }

        private void chkPenOCxmitVHF0_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCxmitVHF01.Checked) val += 1 << 0;
            if (chkPenOCxmitVHF02.Checked) val += 1 << 1;
            if (chkPenOCxmitVHF03.Checked) val += 1 << 2;
            if (chkPenOCxmitVHF04.Checked) val += 1 << 3;
            if (chkPenOCxmitVHF05.Checked) val += 1 << 4;
            if (chkPenOCxmitVHF06.Checked) val += 1 << 5;
            if (chkPenOCxmitVHF07.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF0, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code
        }

        private void chkPenOCxmitVHF1_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCxmitVHF11.Checked) val += 1 << 0;
            if (chkPenOCxmitVHF12.Checked) val += 1 << 1;
            if (chkPenOCxmitVHF13.Checked) val += 1 << 2;
            if (chkPenOCxmitVHF14.Checked) val += 1 << 3;
            if (chkPenOCxmitVHF15.Checked) val += 1 << 4;
            if (chkPenOCxmitVHF16.Checked) val += 1 << 5;
            if (chkPenOCxmitVHF17.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF1, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code
        }

        private void chkPenOCxmitVHF2_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCxmitVHF21.Checked) val += 1 << 0;
            if (chkPenOCxmitVHF22.Checked) val += 1 << 1;
            if (chkPenOCxmitVHF23.Checked) val += 1 << 2;
            if (chkPenOCxmitVHF24.Checked) val += 1 << 3;
            if (chkPenOCxmitVHF25.Checked) val += 1 << 4;
            if (chkPenOCxmitVHF26.Checked) val += 1 << 5;
            if (chkPenOCxmitVHF27.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF2, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code
        }

        private void chkPenOCxmitVHF3_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCxmitVHF31.Checked) val += 1 << 0;
            if (chkPenOCxmitVHF32.Checked) val += 1 << 1;
            if (chkPenOCxmitVHF33.Checked) val += 1 << 2;
            if (chkPenOCxmitVHF34.Checked) val += 1 << 3;
            if (chkPenOCxmitVHF35.Checked) val += 1 << 4;
            if (chkPenOCxmitVHF36.Checked) val += 1 << 5;
            if (chkPenOCxmitVHF37.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF3, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code
        }

        private void chkPenOCxmitVHF4_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCxmitVHF41.Checked) val += 1 << 0;
            if (chkPenOCxmitVHF42.Checked) val += 1 << 1;
            if (chkPenOCxmitVHF43.Checked) val += 1 << 2;
            if (chkPenOCxmitVHF44.Checked) val += 1 << 3;
            if (chkPenOCxmitVHF45.Checked) val += 1 << 4;
            if (chkPenOCxmitVHF46.Checked) val += 1 << 5;
            if (chkPenOCxmitVHF47.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF4, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code
        }

        private void chkPenOCxmitVHF5_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCxmitVHF51.Checked) val += 1 << 0;
            if (chkPenOCxmitVHF52.Checked) val += 1 << 1;
            if (chkPenOCxmitVHF53.Checked) val += 1 << 2;
            if (chkPenOCxmitVHF54.Checked) val += 1 << 3;
            if (chkPenOCxmitVHF55.Checked) val += 1 << 4;
            if (chkPenOCxmitVHF56.Checked) val += 1 << 5;
            if (chkPenOCxmitVHF57.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF5, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code
        }

        private void chkPenOCxmitVHF6_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCxmitVHF61.Checked) val += 1 << 0;
            if (chkPenOCxmitVHF62.Checked) val += 1 << 1;
            if (chkPenOCxmitVHF63.Checked) val += 1 << 2;
            if (chkPenOCxmitVHF64.Checked) val += 1 << 3;
            if (chkPenOCxmitVHF65.Checked) val += 1 << 4;
            if (chkPenOCxmitVHF66.Checked) val += 1 << 5;
            if (chkPenOCxmitVHF67.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF6, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code
        }

        private void chkPenOCxmitVHF7_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCxmitVHF71.Checked) val += 1 << 0;
            if (chkPenOCxmitVHF72.Checked) val += 1 << 1;
            if (chkPenOCxmitVHF73.Checked) val += 1 << 2;
            if (chkPenOCxmitVHF74.Checked) val += 1 << 3;
            if (chkPenOCxmitVHF75.Checked) val += 1 << 4;
            if (chkPenOCxmitVHF76.Checked) val += 1 << 5;
            if (chkPenOCxmitVHF77.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF7, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code
        }

        private void chkPenOCxmitVHF8_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCxmitVHF81.Checked) val += 1 << 0;
            if (chkPenOCxmitVHF82.Checked) val += 1 << 1;
            if (chkPenOCxmitVHF83.Checked) val += 1 << 2;
            if (chkPenOCxmitVHF84.Checked) val += 1 << 3;
            if (chkPenOCxmitVHF85.Checked) val += 1 << 4;
            if (chkPenOCxmitVHF86.Checked) val += 1 << 5;
            if (chkPenOCxmitVHF87.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF8, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code
        }

        private void chkPenOCxmitVHF9_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCxmitVHF91.Checked) val += 1 << 0;
            if (chkPenOCxmitVHF92.Checked) val += 1 << 1;
            if (chkPenOCxmitVHF93.Checked) val += 1 << 2;
            if (chkPenOCxmitVHF94.Checked) val += 1 << 3;
            if (chkPenOCxmitVHF95.Checked) val += 1 << 4;
            if (chkPenOCxmitVHF96.Checked) val += 1 << 5;
            if (chkPenOCxmitVHF97.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF9, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code
        }

        private void chkPenOCxmitVHF10_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCxmitVHF101.Checked) val += 1 << 0;
            if (chkPenOCxmitVHF102.Checked) val += 1 << 1;
            if (chkPenOCxmitVHF103.Checked) val += 1 << 2;
            if (chkPenOCxmitVHF104.Checked) val += 1 << 3;
            if (chkPenOCxmitVHF105.Checked) val += 1 << 4;
            if (chkPenOCxmitVHF106.Checked) val += 1 << 5;
            if (chkPenOCxmitVHF107.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF10, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code
        }

        private void chkPenOCxmitVHF11_CheckedChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (chkPenOCxmitVHF111.Checked) val += 1 << 0;
            if (chkPenOCxmitVHF112.Checked) val += 1 << 1;
            if (chkPenOCxmitVHF113.Checked) val += 1 << 2;
            if (chkPenOCxmitVHF114.Checked) val += 1 << 3;
            if (chkPenOCxmitVHF115.Checked) val += 1 << 4;
            if (chkPenOCxmitVHF116.Checked) val += 1 << 5;
            if (chkPenOCxmitVHF117.Checked) val += 1 << 6;

            Penny.getPenny().setBandBitMask(Band.VHF11, (byte)val, true);
            console.PennyExtCtrlEnabled = chkPennyExtCtrl.Checked;  // need side effect of this to push change to native code
        }

        private void radAlexManualControl_CheckedChanged(object sender, EventArgs e)
        {
            // int val = 0;
            /*    if (radAlexAutoCntl.Checked)
                {
                    JanusAudio.SetAlexManEnable(0x00);
                  //  val = 0;
                    console.AlexPresent = chkAlexPresent.Checked;
                  //  JanusAudio.SetAlexHPFBits(0x20); // Bypass HPF
                  //  radBPHPFled.Checked = true;
                    console.chkSR.Checked = true; //ALEX console
                } */

            if (radAlexManualCntl.Checked)
            {
                JanusAudio.SetAlexManEnable(0x01);
                //  val = 1;
                console.AlexPresent = chkAlexPresent.Checked;
                //  console.AlexPresent = chkAlexPresent.Checked;
                console.chkSR.Checked = false;
            }

            // JanusAudio.SetAlexManEnable(val & 0x01);

        }

        private void radAlexAutoControl_CheckedChanged(object sender, EventArgs e)
        {
            // int val = 0;
            if (radAlexAutoCntl.Checked)
            {
                JanusAudio.SetAlexManEnable(0x00);
                //  val = 0;
                console.AlexPresent = chkAlexPresent.Checked;
                //  JanusAudio.SetAlexHPFBits(0x20); // Bypass HPF
                //  radBPHPFled.Checked = true;
                console.chkSR.Checked = true; //ALEX console
            }

            /*  if (radAlexManualCntl.Checked)
              {
                  JanusAudio.SetAlexManEnable(0x01);
                  //  val = 1;
                  console.AlexPresent = chkAlexPresent.Checked;
                  //  console.AlexPresent = chkAlexPresent.Checked;
                  console.chkSR.Checked = false;
              } */

            // JanusAudio.SetAlexManEnable(val & 0x01);

        }

        private void chkAlexHPFBypass_CheckedChanged(object sender, EventArgs e)
        {
            console.AlexHPFBypass = chkAlexHPFBypass.Checked;
        }

        private void chkAlex2HPFBypass_CheckedChanged(object sender, EventArgs e)
        {
            console.Alex2HPFBypass = chkAlex2HPFBypass.Checked;
        }

        private void chkSwapAF_CheckedChanged(object sender, EventArgs e)
        {
            console.SwapAF = chkSwapAF.Checked;
        }

        private void tpGeneralCalibration_Paint(object sender, PaintEventArgs e)
        {
            panelRX2LevelCal.Visible = false; // console.rx2_preamp_present;
        }

        private void chkShowAGC_CheckedChanged(object sender, EventArgs e)
        {
            console.ShowAGC = chkShowAGC.Checked;
        }

        private void chkSpectrumLine_CheckedChanged(object sender, EventArgs e)
        {
            Display.SpectrumLine = chkSpectrumLine.Checked;
        }

        private void chkAGCDisplayHangLine_CheckedChanged(object sender, EventArgs e)
        {
            console.DisplayAGCHangLine = chkAGCDisplayHangLine.Checked;
        }

        private void chkAGCHangSpectrumLine_CheckedChanged(object sender, EventArgs e)
        {
            Display.RX1HangSpectrumLine = chkAGCHangSpectrumLine.Checked;
        }

        private void chkDisplayRX2GainLine_CheckedChanged(object sender, EventArgs e)
        {
            console.DisplayRX2GainLine = chkDisplayRX2GainLine.Checked;
        }

        private void chkRX2GainSpectrumLine_CheckedChanged(object sender, EventArgs e)
        {
            Display.RX2GainSpectrumLine = chkRX2GainSpectrumLine.Checked;
        }

        private void chkDisplayRX2HangLine_CheckedChanged(object sender, EventArgs e)
        {
            console.DisplayRX2HangLine = chkDisplayRX2HangLine.Checked;
        }

        private void chkRX2HangSpectrumLine_CheckedChanged(object sender, EventArgs e)
        {
            Display.RX2HangSpectrumLine = chkRX2HangSpectrumLine.Checked;
        }

        private void chkFirmwareByp_CheckedChanged(object sender, EventArgs e)
        {
            firmware_bypass = chkFirmwareByp.Checked;
        }

        private void chkFullDiscovery_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFullDiscovery.Checked)
                JanusAudio.SetDiscoveryMode(1);
            else
                JanusAudio.SetDiscoveryMode(0);
        }

      private void chkStrictCharSpacing_CheckedChanged(object sender, EventArgs e)
        {
            CWKeyer.AutoCharSpace = chkStrictCharSpacing.Checked;
        }

        private void chkModeBStrict_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCWKeyerMode.Checked)
            {
                if (chkModeBStrict.Checked)
                    CWKeyer.CurrentIambicMode = CWKeyer.IambicMode.ModeBStrict;
                else
                    CWKeyer.CurrentIambicMode = CWKeyer.IambicMode.ModeB;
            }
        }

       private void chkRxOutOnTx_CheckedChanged(object sender, EventArgs e)
        {
            Alex.RxOutOnTx = chkRxOutOnTx.Checked;
        }

        private void chkSWRProtection_CheckedChanged(object sender, EventArgs e)
        {
            console.SWRProtection = chkSWRProtection.Checked;
        }

        private void chkATTOnTX_CheckedChanged(object sender, EventArgs e)
        {
            console.ATTOnTX = chkATTOnTX.Checked;
        }

        private void chkTXLimitSlew_CheckedChanged(object sender, EventArgs e)
        {
            console.LimitSlew = chkTXLimitSlew.Checked;
        }

        private void chkAlex1_5BPHPF_CheckedChanged(object sender, EventArgs e)
        {
            console.Alex1_5BPHPFBypass = chkAlex1_5BPHPF.Checked;
        }

        private void chkAlex6_5BPHPF_CheckedChanged(object sender, EventArgs e)
        {
            console.Alex6_5BPHPFBypass = chkAlex6_5BPHPF.Checked;
        }

        private void chkAlex9_5BPHPF_CheckedChanged(object sender, EventArgs e)
        {
            console.Alex9_5BPHPFBypass = chkAlex9_5BPHPF.Checked;
        }

        private void chkAlex13BPHPF_CheckedChanged(object sender, EventArgs e)
        {
            console.Alex13BPHPFBypass = chkAlex13BPHPF.Checked;
        }

        private void chkAlex20BPHPF_CheckedChanged(object sender, EventArgs e)
        {
            console.Alex20BPHPFBypass = chkAlex20BPHPF.Checked;
        }

        private void chkAlex6BPHPF_CheckedChanged(object sender, EventArgs e)
        {
            console.Alex6BPHPFBypass = chkAlex6BPHPF.Checked;
        }

        private void chkAlex21_5BPHPF_CheckedChanged(object sender, EventArgs e)
        {
            console.Alex21_5BPHPFBypass = chkAlex21_5BPHPF.Checked;
        }

        private void chkAlex26_5BPHPF_CheckedChanged(object sender, EventArgs e)
        {
            console.Alex26_5BPHPFBypass = chkAlex26_5BPHPF.Checked;
        }

        private void chkAlex29_5BPHPF_CheckedChanged(object sender, EventArgs e)
        {
            console.Alex29_5BPHPFBypass = chkAlex29_5BPHPF.Checked;
        }

        private void chkAlex213BPHPF_CheckedChanged(object sender, EventArgs e)
        {
            console.Alex213BPHPFBypass = chkAlex213BPHPF.Checked;
        }

        private void chkAlex220BPHPF_CheckedChanged(object sender, EventArgs e)
        {
            console.Alex220BPHPFBypass = chkAlex220BPHPF.Checked;
        }

        private void chkAlex26BPHPF_CheckedChanged(object sender, EventArgs e)
        {
            console.Alex26BPHPFBypass = chkAlex26BPHPF.Checked;
        }

        private void tpPennyCtrl_Paint(object sender, PaintEventArgs e)
        {
            switch (console.CurrentModel)
            {
                case Model.HPSDR:
                    lblHFRxControl.Text = "J6 Receive Pins";
                    lblHFTxControl.Text = "J6 Transmit Pins";
                    lblVHFRxControl.Text = "J6 Receive Pins";
                    lblVHFTxControl.Text = "J6 Transmit Pins";
                    break;
                case Model.HERMES:
                    lblHFRxControl.Text = "J16 Receive Pins";
                    lblHFTxControl.Text = "J16 Transmit Pins";
                    lblVHFRxControl.Text = "J16 Receive Pins";
                    lblVHFTxControl.Text = "J16 Transmit Pins";
                    break;
                default:
                    lblHFRxControl.Text = "J6 Receive Pins";
                    lblHFTxControl.Text = "J6 Transmit Pins";
                    lblVHFRxControl.Text = "J6 Receive Pins";
                    lblVHFTxControl.Text = "J6 Transmit Pins";
                    break;
            }
        }

        private void chkApolloPresent_CheckedChanged(object sender, EventArgs e)
        {
            if (chkApolloPresent.Checked)
            {
                if (chkAlexPresent.Checked) chkAlexPresent.Checked = false;
 
                JanusAudio.SetHermesFilter(1);
                chkApolloFilter_CheckedChanged(this, EventArgs.Empty);
                chkApolloTuner_CheckedChanged(this, EventArgs.Empty);
            }
            else JanusAudio.SetHermesFilter(0);
        }

        private void chkApolloFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (chkApolloFilter.Checked) JanusAudio.EnableApolloFilter(1);
            else JanusAudio.EnableApolloFilter(0);
        }

        private void chkApolloTuner_CheckedChanged(object sender, EventArgs e)
        {
            if (chkApolloTuner.Checked) JanusAudio.EnableApolloTuner(1);
            else JanusAudio.EnableApolloTuner(0);
        }

        private void chkEClassModulation_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEClassModulation.Checked) JanusAudio.EnableEClassModulation(1);
            else JanusAudio.EnableEClassModulation(0);
        }
    }

    #region PADeviceInfo Helper Class

    public class PADeviceInfo
    {
        private string _Name;
        private int _Index;

        public string Name
        {
            get { return _Name; }
        }

        public int Index
        {
            get { return _Index; }
        }

        public PADeviceInfo(String argName, int argIndex)
        {
            _Name = argName;
            _Index = argIndex;
        }

        public override string ToString()
        {
            return _Name;
        }
    }

    #endregion

    #region 60m Channels Class

    public class Channel60m
    {
        private double freq;
        public Channel60m(double f)
            : base()
        {
            freq = f;
        }

        public double Freq
        {
            get
            {
                return freq;
            }
            set
            {
                freq = value;
            }
        }
    }

    #endregion

}
