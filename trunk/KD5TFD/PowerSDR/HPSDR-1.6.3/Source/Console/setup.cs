//=================================================================
// setup.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004, 2005, 2006  FlexRadio Systems
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
//    12100 Technology Blvd.
//    Austin, TX 78727
//    USA
//=================================================================

using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using SDRSerialSupportII; 

namespace PowerSDR
{
	public class Setup : System.Windows.Forms.Form
	{
		#region Variable Declaration
		
		private Console console;
		private Progress progress;
		private ArrayList KeyList;
		private int sound_card;
		private bool initializing;

		private System.Windows.Forms.TabPage tpDSP;
		private System.Windows.Forms.TabPage tpDisplay;
		private System.Windows.Forms.NumericUpDownTS udDisplayGridStep;
		private System.Windows.Forms.NumericUpDownTS udDisplayGridMin;
		private System.Windows.Forms.NumericUpDownTS udDisplayGridMax;
		private System.Windows.Forms.LabelTS lblDisplayGridStep;
		private System.Windows.Forms.LabelTS lblDisplayGridMin;
		private System.Windows.Forms.LabelTS lblDisplayGridMax;
		private System.Windows.Forms.TabPage tpGeneral;
		private System.Windows.Forms.ComboBoxTS comboGeneralLPTAddr;
		private System.Windows.Forms.GroupBoxTS grpDisplaySpectrumGrid;
		private System.Windows.Forms.ButtonTS btnOK;
		private System.Windows.Forms.ButtonTS btnCancel;
		private System.Windows.Forms.ButtonTS btnApply;
		private System.Windows.Forms.CheckBoxTS chkGeneralRFEPresent;
		public System.Windows.Forms.CheckBoxTS chkGeneralSpurRed;
		private System.Windows.Forms.GroupBoxTS grpGeneralHardwareSetup;
		private System.Windows.Forms.LabelTS lblGeneralLPTAddr;
		private System.Windows.Forms.NumericUpDownTS udGeneralLPTDelay;
		private System.Windows.Forms.LabelTS lblGeneralLPTDelay;
		public System.Windows.Forms.TabControl tcSetup;
		private System.Windows.Forms.TabPage tpKeyboard;
		private System.Windows.Forms.LabelTS lblKBTuneDown;
		private System.Windows.Forms.LabelTS lblKBTuneUp;
		private System.Windows.Forms.ComboBoxTS comboKBTuneDown1;
		private System.Windows.Forms.ComboBoxTS comboKBTuneDown3;
		private System.Windows.Forms.ComboBoxTS comboKBTuneDown2;
		private System.Windows.Forms.ComboBoxTS comboKBTuneUp1;
		private System.Windows.Forms.ComboBoxTS comboKBTuneUp2;
		private System.Windows.Forms.ComboBoxTS comboKBTuneUp3;
		private System.Windows.Forms.ComboBoxTS comboKBTuneUp4;
		private System.Windows.Forms.ComboBoxTS comboKBTuneDown4;
		private System.Windows.Forms.ComboBoxTS comboKBTuneUp5;
		private System.Windows.Forms.ComboBoxTS comboKBTuneDown5;
		private System.Windows.Forms.ComboBoxTS comboKBTuneDown6;
		private System.Windows.Forms.ComboBoxTS comboKBTuneUp7;
		private System.Windows.Forms.ComboBoxTS comboKBTuneDown7;
		private System.Windows.Forms.ComboBoxTS comboKBTuneUp6;
		private System.Windows.Forms.GroupBoxTS grpKBTune;
		private System.Windows.Forms.LabelTS lblKBTuneDigit;
		private System.Windows.Forms.LabelTS lblKBTune7;
		private System.Windows.Forms.LabelTS lblKBTune6;
		private System.Windows.Forms.LabelTS lblKBTune5;
		private System.Windows.Forms.LabelTS lblKBTune4;
		private System.Windows.Forms.LabelTS lblKBTune3;
		private System.Windows.Forms.LabelTS lblKBTune2;
		private System.Windows.Forms.LabelTS lblKBTune1;
		private System.Windows.Forms.GroupBoxTS grpKBBand;
		private System.Windows.Forms.LabelTS lblKBBandUp;
		private System.Windows.Forms.LabelTS lblKBBandDown;
		private System.Windows.Forms.GroupBoxTS grpKBFilter;
		private System.Windows.Forms.LabelTS lblKBFilterUp;
		private System.Windows.Forms.LabelTS lblKBFilterDown;
		private System.Windows.Forms.GroupBoxTS grpKBMode;
		private System.Windows.Forms.LabelTS lblKBModeUp;
		private System.Windows.Forms.LabelTS lblKBModeDown;
		private System.Windows.Forms.ComboBoxTS comboKBBandUp;
		private System.Windows.Forms.ComboBoxTS comboKBBandDown;
		private System.Windows.Forms.ComboBoxTS comboKBFilterUp;
		private System.Windows.Forms.ComboBoxTS comboKBFilterDown;
		private System.Windows.Forms.ComboBoxTS comboKBModeUp;
		private System.Windows.Forms.ComboBoxTS comboKBModeDown;
		private System.Windows.Forms.LabelTS lblDisplayFPS;
		private System.Windows.Forms.NumericUpDownTS udDisplayFPS;
		private System.Windows.Forms.CheckBoxTS chkGeneralXVTRPresent;
		private System.Windows.Forms.GroupBoxTS grpGeneralDDS;
		private System.Windows.Forms.LabelTS lblPLLMult;
		private System.Windows.Forms.NumericUpDownTS udDDSPLLMult;
		private System.Windows.Forms.LabelTS lblIFFrequency;
		private System.Windows.Forms.NumericUpDownTS udDDSIFFreq;
		private System.Windows.Forms.LabelTS lblClockCorrection;
		private System.Windows.Forms.NumericUpDownTS udDDSCorrection;
		public System.Windows.Forms.TabPage tpAudio;
		private System.Windows.Forms.TabPage tpTransmit;
		private System.Windows.Forms.NumericUpDownTS udTXFilterHigh;
		private System.Windows.Forms.LabelTS lblTXFilterLow;
		private System.Windows.Forms.LabelTS lblTXFilterHigh;
		private System.Windows.Forms.NumericUpDownTS udTXFilterLow;
		private System.Windows.Forms.GroupBoxTS grpTXFilter;
		private System.Windows.Forms.GroupBoxTS grpTXCompression;
		private System.Windows.Forms.GroupBoxTS grpDisplayPhase;
		private System.Windows.Forms.NumericUpDownTS udDisplayPhasePts;
		private System.Windows.Forms.GroupBoxTS grpDisplayAverage;
		private System.Windows.Forms.LabelTS lblDisplayPhasePts;
		private System.Windows.Forms.LabelTS lblTransmitFeedForward;
		private System.Windows.Forms.GroupBoxTS grpGeneralCalibration;
		private System.Windows.Forms.LabelTS lblGeneralCalFrequency;
		private System.Windows.Forms.LabelTS lblGeneralCalLevel;
		private System.Windows.Forms.NumericUpDownTS udGeneralCalLevel;
		private System.Windows.Forms.GroupBoxTS grpDisplayRefreshRates;
		private System.Windows.Forms.LabelTS lblDisplayMeterDelay;
		private System.Windows.Forms.NumericUpDownTS udDisplayMeterDelay;
		private System.Windows.Forms.TabPage tpAppearance;
		private System.Windows.Forms.LabelTS lblDisplayFilterColor;
		private System.Windows.Forms.LabelTS lblDisplayLineWidth;
		private System.Windows.Forms.NumericUpDownTS udDisplayLineWidth;
		private System.Windows.Forms.LabelTS lblDisplayDataLineColor;
		private System.Windows.Forms.LabelTS lblDisplayTextColor;
		private System.Windows.Forms.LabelTS lblDisplayZeroLineColor;
		private System.Windows.Forms.LabelTS lblDisplayGridColor;
		private System.Windows.Forms.LabelTS lblDisplayBackgroundColor;
		private System.Windows.Forms.GroupBoxTS grpAppearanceMeter;
		private System.Windows.Forms.LabelTS lblAppearanceMeterRight;
		private System.Windows.Forms.LabelTS lblAppearanceMeterLeft;
		private System.Windows.Forms.LabelTS lblAppearanceGenBtnSel;
		private System.Windows.Forms.GroupBoxTS grpGeneralOptions;
		private System.Windows.Forms.CheckBoxTS chkGeneralDisablePTT;
		private System.Windows.Forms.LabelTS lblDisplayPeakText;
		private System.Windows.Forms.NumericUpDownTS udDisplayPeakText;
		private System.Windows.Forms.NumericUpDownTS udDisplayCPUMeter;
		private System.Windows.Forms.LabelTS lblDisplayCPUMeter;
		private System.Windows.Forms.TrackBarTS tbTXFFCompression;
		private System.Windows.Forms.NumericUpDownTS udTXFFCompression;
		private System.Windows.Forms.ComboBoxTS comboGeneralXVTR;
		private System.Windows.Forms.GroupBoxTS grpDisplayWaterfall;
		private System.Windows.Forms.NumericUpDownTS udDisplayWaterfallHighLevel;
		private System.Windows.Forms.LabelTS lblDisplayWaterfallHighLevel;
		private System.Windows.Forms.LabelTS lblDisplayWaterfallLowLevel;
		private System.Windows.Forms.NumericUpDownTS udDisplayWaterfallLowLevel;
		private System.Windows.Forms.LabelTS lblDisplayWaterfallLowColor;
		private System.Windows.Forms.LabelTS lblDisplayWaterfallHighColor;
		private System.Windows.Forms.LabelTS lblDisplayWaterfallMidColor;
		private System.Windows.Forms.CheckBoxTS chkGeneralPAPresent;
		private System.Windows.Forms.ButtonTS btnGeneralCalLevelStart;
		private System.Windows.Forms.ButtonTS btnGeneralCalFreqStart;
		private System.Windows.Forms.ButtonTS btnGeneralCalImageStart;
		private System.Windows.Forms.CheckBoxTS chkGeneralSoftwareGainCorr;
		private System.Windows.Forms.LabelTS lblBandLight;
		private System.Windows.Forms.LabelTS lblBandDark;
		private System.Windows.Forms.LabelTS lblPeakText;
		private System.Windows.Forms.ButtonTS btnWizard;
		private System.Windows.Forms.ButtonTS btnImportDB;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.TabPage tpTests;
		private System.Windows.Forms.LabelTS lblPAGainByBand160;
		private System.Windows.Forms.LabelTS lblPAGainByBand80;
		private System.Windows.Forms.LabelTS lblPAGainByBand60;
		private System.Windows.Forms.LabelTS lblPAGainByBand40;
		private System.Windows.Forms.LabelTS lblPAGainByBand30;
		private System.Windows.Forms.LabelTS lblPAGainByBand10;
		private System.Windows.Forms.LabelTS lblPAGainByBand12;
		private System.Windows.Forms.LabelTS lblPAGainByBand15;
		private System.Windows.Forms.LabelTS lblPAGainByBand17;
		private System.Windows.Forms.LabelTS lblPAGainByBand20;
		private System.Windows.Forms.TabPage tpPowerAmplifier;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.NumericUpDownTS udPAGain10;
		private System.Windows.Forms.NumericUpDownTS udPAGain12;
		private System.Windows.Forms.NumericUpDownTS udPAGain15;
		private System.Windows.Forms.NumericUpDownTS udPAGain17;
		private System.Windows.Forms.NumericUpDownTS udPAGain20;
		private System.Windows.Forms.NumericUpDownTS udPAGain30;
		private System.Windows.Forms.NumericUpDownTS udPAGain40;
		private System.Windows.Forms.NumericUpDownTS udPAGain60;
		private System.Windows.Forms.NumericUpDownTS udPAGain80;
		private System.Windows.Forms.NumericUpDownTS udPAGain160;
		private System.Windows.Forms.GroupBoxTS grpPAGainByBand;
		private System.Windows.Forms.ButtonTS btnPAGainCalibration;
		private System.Windows.Forms.CheckBoxTS chkGeneralEnableX2;
		private System.Windows.Forms.LabelTS lblGeneralX2Delay;
		private System.Windows.Forms.NumericUpDownTS udGeneralX2Delay;
		private System.Windows.Forms.GroupBoxTS grpPABandOffset;
		private System.Windows.Forms.LabelTS lblPABandOffset30;
		private System.Windows.Forms.LabelTS lblPABandOffset40;
		private System.Windows.Forms.LabelTS lblPABandOffset60;
		private System.Windows.Forms.LabelTS lblPABandOffset80;
		private System.Windows.Forms.LabelTS lblPABandOffset160;
		private System.Windows.Forms.LabelTS lblPABandOffset10;
		private System.Windows.Forms.LabelTS lblPABandOffset12;
		private System.Windows.Forms.LabelTS lblPABandOffset15;
		private System.Windows.Forms.LabelTS lblPABandOffset17;
		private System.Windows.Forms.LabelTS lblPABandOffset20;
		private System.Windows.Forms.CheckBoxTS chkGeneralATUPresent;
		private System.Windows.Forms.ButtonTS btnPAGainReset;
		private System.Windows.Forms.ComboBoxTS comboGeneralProcessPriority;
		private System.Windows.Forms.GroupBoxTS grpGeneralProcessPriority;
		private System.Windows.Forms.GroupBoxTS grpTestTXIMD;
		private System.Windows.Forms.LabelTS lblTXFFVal;
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
		private ColorButton clrbtnText;
		private ColorButton clrbtnDataLine;
		private ColorButton clrbtnMeterLeft;
		private ColorButton clrbtnMeterRight;
		private ColorButton clrbtnWaterfallLow;
		private ColorButton clrbtnWaterfallHigh;
		private ColorButton clrbtnWaterfallMid;
		private System.Windows.Forms.LabelTS lblTestIMDPower;
		private System.Windows.Forms.NumericUpDownTS udTestIMDPower;
		private System.Windows.Forms.ButtonTS btnResetAll;
		private System.Windows.Forms.CheckBoxTS chkGeneralCustomFilter;
		private System.Windows.Forms.NumericUpDownTS udTestIMDFreq1;
		private System.Windows.Forms.NumericUpDownTS udTestIMDFreq2;
		private System.Windows.Forms.ButtonTS btnTestAudioBalStart;
		private System.Windows.Forms.CheckBoxTS chkTestX2Pin1;
		private System.Windows.Forms.CheckBoxTS chkTestX2Pin2;
		private System.Windows.Forms.CheckBoxTS chkTestX2Pin3;
		private System.Windows.Forms.CheckBoxTS chkTestX2Pin4;
		private System.Windows.Forms.CheckBoxTS chkTestX2Pin5;
		private System.Windows.Forms.CheckBoxTS chkTestX2Pin6;
		private System.Windows.Forms.NumericUpDownTS udDisplayAVGTime;
		private System.Windows.Forms.LabelTS lblDisplayAVGTime;
		private System.Windows.Forms.GroupBoxTS grpTestX2;
		private System.Windows.Forms.GroupBoxTS grpTestAudioBalance;
		private System.Windows.Forms.NumericUpDownTS udPAADC17;
		private System.Windows.Forms.NumericUpDownTS udPAADC15;
		private System.Windows.Forms.NumericUpDownTS udPAADC20;
		private System.Windows.Forms.NumericUpDownTS udPAADC12;
		private System.Windows.Forms.NumericUpDownTS udPAADC10;
		private System.Windows.Forms.NumericUpDownTS udPAADC160;
		private System.Windows.Forms.NumericUpDownTS udPAADC80;
		private System.Windows.Forms.NumericUpDownTS udPAADC60;
		private System.Windows.Forms.NumericUpDownTS udPAADC40;
		private System.Windows.Forms.NumericUpDownTS udPAADC30;
		private System.Windows.Forms.CheckBoxTS chkGeneralUSBPresent;
		private System.Windows.Forms.GroupBoxTS grpPATune;
		private System.Windows.Forms.LabelTS lblTransmitTunePower;
		private System.Windows.Forms.NumericUpDownTS udTXTunePower;
		private System.Windows.Forms.LabelTS lblPACalPower;
		private System.Windows.Forms.NumericUpDownTS udPACalPower;
		private System.Windows.Forms.GroupBoxTS grpDisplayMultimeter;
		private System.Windows.Forms.LabelTS lblDisplayMultiPeakHoldTime;
		private System.Windows.Forms.NumericUpDownTS udDisplayMultiPeakHoldTime;
		private System.Windows.Forms.NumericUpDownTS udDisplayMultiTextHoldTime;
		private System.Windows.Forms.LabelTS lblDisplayMeterTextHoldTime;
		private System.Windows.Forms.GroupBoxTS grpGeneralUpdates;
		private System.Windows.Forms.CheckBoxTS chkGeneralUpdateRelease;
		private System.Windows.Forms.CheckBoxTS chkGeneralUpdateBeta;
		private System.Windows.Forms.CheckBoxTS chkGeneralRXOnly;
		private System.Windows.Forms.LabelTS lblTestX2;
		private System.Windows.Forms.LabelTS lblTestToneFreq2;
		private System.Windows.Forms.LabelTS lblTestToneFreq1;
		private System.Windows.Forms.TabPage tpCAT;
		private System.Windows.Forms.GroupBoxTS grpPTTBitBang;
		private System.Windows.Forms.LabelTS lblCATPTTPort;
		private System.Windows.Forms.CheckBoxTS chkCATPTT_RTS;
		private System.Windows.Forms.CheckBoxTS chkCATPTT_DTR;
		private System.Windows.Forms.GroupBoxTS grpCatControlBox;
		private System.Windows.Forms.ComboBoxTS comboCATbaud;
		private System.Windows.Forms.LabelTS lblCATBaud;
		private System.Windows.Forms.CheckBoxTS chkCATEnable;
		private System.Windows.Forms.LabelTS lblCATParity;
		private System.Windows.Forms.LabelTS lblCATData;
		private System.Windows.Forms.LabelTS lblCATStop;
		private System.Windows.Forms.ComboBoxTS comboCATparity;
		private System.Windows.Forms.ComboBoxTS comboCATdatabits;
		private System.Windows.Forms.ComboBoxTS comboCATstopbits;
		private System.Windows.Forms.GroupBoxTS grpKBCW;
		private System.Windows.Forms.LabelTS lblKBCWDot;
		private System.Windows.Forms.LabelTS lblKBCWDash;
		private System.Windows.Forms.ComboBoxTS comboKBCWDot;
		private System.Windows.Forms.ComboBoxTS comboKBCWDash;
		private System.Windows.Forms.GroupBoxTS grpKBRIT;
		private System.Windows.Forms.LabelTS lblKBRitUp;
		private System.Windows.Forms.LabelTS lblKBRITDown;
		private System.Windows.Forms.ComboBoxTS comboKBRITUp;
		private System.Windows.Forms.ComboBoxTS comboKBRITDown;
		private System.Windows.Forms.GroupBoxTS grpKBXIT;
		private System.Windows.Forms.LabelTS lblKBXITUp;
		private System.Windows.Forms.LabelTS lblKBXITDown;
		private System.Windows.Forms.ComboBoxTS comboKBXITUp;
		private System.Windows.Forms.ComboBoxTS comboKBXITDown;
		private System.Windows.Forms.CheckBoxTS chkDCBlock;
		private System.Windows.Forms.TabPage tpExtCtrl;
		private System.Windows.Forms.GroupBoxTS grpExtTX;
		private System.Windows.Forms.LabelTS lblExtTXX25;
		private System.Windows.Forms.LabelTS lblExtTXX24;
		private System.Windows.Forms.LabelTS lblExtTXX23;
		private System.Windows.Forms.LabelTS lblExtTXX22;
		private System.Windows.Forms.LabelTS lblExtTX2;
		private System.Windows.Forms.LabelTS lblExtTX6;
		private System.Windows.Forms.LabelTS lblExtTX10;
		private System.Windows.Forms.LabelTS lblExtTX12;
		private System.Windows.Forms.LabelTS lblExtTX15;
		private System.Windows.Forms.LabelTS lblExtTX17;
		private System.Windows.Forms.LabelTS lblExtTX20;
		private System.Windows.Forms.LabelTS lblExtTX30;
		private System.Windows.Forms.LabelTS lblExtTX40;
		private System.Windows.Forms.LabelTS lblExtTX60;
		private System.Windows.Forms.LabelTS lblExtTX80;
		private System.Windows.Forms.LabelTS lblExtTXX2Pins;
		private System.Windows.Forms.LabelTS lblExtTXBand;
		private System.Windows.Forms.LabelTS lblExtTX160;
		private System.Windows.Forms.CheckBoxTS chkExtTX1603;
		private System.Windows.Forms.CheckBoxTS chkExtTX1602;
		private System.Windows.Forms.CheckBoxTS chkExtTX1601;
		private System.Windows.Forms.LabelTS lblExtTXX21;
		private System.Windows.Forms.CheckBoxTS chkExtTX1605;
		private System.Windows.Forms.CheckBoxTS chkExtTX1604;
		private System.Windows.Forms.GroupBoxTS grpExtRX;
		private System.Windows.Forms.LabelTS lblExtRXX25;
		private System.Windows.Forms.LabelTS lblExtRXX24;
		private System.Windows.Forms.LabelTS lblExtRXX23;
		private System.Windows.Forms.LabelTS lblExtRXX22;
		private System.Windows.Forms.LabelTS lblExtRX2;
		private System.Windows.Forms.LabelTS lblExtRX6;
		private System.Windows.Forms.LabelTS lblExtRX10;
		private System.Windows.Forms.LabelTS lblExtRX12;
		private System.Windows.Forms.LabelTS lblExtRX15;
		private System.Windows.Forms.LabelTS lblExtRX17;
		private System.Windows.Forms.LabelTS lblExtRX20;
		private System.Windows.Forms.LabelTS lblExtRX30;
		private System.Windows.Forms.LabelTS lblExtRX40;
		private System.Windows.Forms.LabelTS lblExtRX60;
		private System.Windows.Forms.LabelTS lblExtRX80;
		private System.Windows.Forms.LabelTS lblExtRXX2Pins;
		private System.Windows.Forms.LabelTS lblExtRXBand;
		private System.Windows.Forms.LabelTS lblExtRX160;
		private System.Windows.Forms.CheckBoxTS chkExtRX1603;
		private System.Windows.Forms.CheckBoxTS chkExtRX1602;
		private System.Windows.Forms.CheckBoxTS chkExtRX1601;
		private System.Windows.Forms.LabelTS lblExtRXX21;
		private System.Windows.Forms.CheckBoxTS chkExtRX1605;
		private System.Windows.Forms.CheckBoxTS chkExtRX1604;
		private System.Windows.Forms.CheckBoxTS chkExtTX23;
		private System.Windows.Forms.CheckBoxTS chkExtTX22;
		private System.Windows.Forms.CheckBoxTS chkExtTX21;
		private System.Windows.Forms.CheckBoxTS chkExtTX25;
		private System.Windows.Forms.CheckBoxTS chkExtTX24;
		private System.Windows.Forms.CheckBoxTS chkExtTX63;
		private System.Windows.Forms.CheckBoxTS chkExtTX62;
		private System.Windows.Forms.CheckBoxTS chkExtTX61;
		private System.Windows.Forms.CheckBoxTS chkExtTX65;
		private System.Windows.Forms.CheckBoxTS chkExtTX64;
		private System.Windows.Forms.CheckBoxTS chkExtTX103;
		private System.Windows.Forms.CheckBoxTS chkExtTX102;
		private System.Windows.Forms.CheckBoxTS chkExtTX101;
		private System.Windows.Forms.CheckBoxTS chkExtTX105;
		private System.Windows.Forms.CheckBoxTS chkExtTX104;
		private System.Windows.Forms.CheckBoxTS chkExtTX123;
		private System.Windows.Forms.CheckBoxTS chkExtTX122;
		private System.Windows.Forms.CheckBoxTS chkExtTX121;
		private System.Windows.Forms.CheckBoxTS chkExtTX125;
		private System.Windows.Forms.CheckBoxTS chkExtTX124;
		private System.Windows.Forms.CheckBoxTS chkExtTX153;
		private System.Windows.Forms.CheckBoxTS chkExtTX152;
		private System.Windows.Forms.CheckBoxTS chkExtTX151;
		private System.Windows.Forms.CheckBoxTS chkExtTX155;
		private System.Windows.Forms.CheckBoxTS chkExtTX154;
		private System.Windows.Forms.CheckBoxTS chkExtTX173;
		private System.Windows.Forms.CheckBoxTS chkExtTX172;
		private System.Windows.Forms.CheckBoxTS chkExtTX171;
		private System.Windows.Forms.CheckBoxTS chkExtTX175;
		private System.Windows.Forms.CheckBoxTS chkExtTX174;
		private System.Windows.Forms.CheckBoxTS chkExtTX203;
		private System.Windows.Forms.CheckBoxTS chkExtTX202;
		private System.Windows.Forms.CheckBoxTS chkExtTX201;
		private System.Windows.Forms.CheckBoxTS chkExtTX205;
		private System.Windows.Forms.CheckBoxTS chkExtTX204;
		private System.Windows.Forms.CheckBoxTS chkExtTX303;
		private System.Windows.Forms.CheckBoxTS chkExtTX302;
		private System.Windows.Forms.CheckBoxTS chkExtTX301;
		private System.Windows.Forms.CheckBoxTS chkExtTX305;
		private System.Windows.Forms.CheckBoxTS chkExtTX304;
		private System.Windows.Forms.CheckBoxTS chkExtTX403;
		private System.Windows.Forms.CheckBoxTS chkExtTX402;
		private System.Windows.Forms.CheckBoxTS chkExtTX401;
		private System.Windows.Forms.CheckBoxTS chkExtTX405;
		private System.Windows.Forms.CheckBoxTS chkExtTX404;
		private System.Windows.Forms.CheckBoxTS chkExtTX603;
		private System.Windows.Forms.CheckBoxTS chkExtTX602;
		private System.Windows.Forms.CheckBoxTS chkExtTX601;
		private System.Windows.Forms.CheckBoxTS chkExtTX605;
		private System.Windows.Forms.CheckBoxTS chkExtTX604;
		private System.Windows.Forms.CheckBoxTS chkExtTX803;
		private System.Windows.Forms.CheckBoxTS chkExtTX802;
		private System.Windows.Forms.CheckBoxTS chkExtTX801;
		private System.Windows.Forms.CheckBoxTS chkExtTX805;
		private System.Windows.Forms.CheckBoxTS chkExtTX804;
		private System.Windows.Forms.CheckBoxTS chkExtRX23;
		private System.Windows.Forms.CheckBoxTS chkExtRX22;
		private System.Windows.Forms.CheckBoxTS chkExtRX21;
		private System.Windows.Forms.CheckBoxTS chkExtRX25;
		private System.Windows.Forms.CheckBoxTS chkExtRX24;
		private System.Windows.Forms.CheckBoxTS chkExtRX63;
		private System.Windows.Forms.CheckBoxTS chkExtRX62;
		private System.Windows.Forms.CheckBoxTS chkExtRX61;
		private System.Windows.Forms.CheckBoxTS chkExtRX65;
		private System.Windows.Forms.CheckBoxTS chkExtRX64;
		private System.Windows.Forms.CheckBoxTS chkExtRX103;
		private System.Windows.Forms.CheckBoxTS chkExtRX102;
		private System.Windows.Forms.CheckBoxTS chkExtRX101;
		private System.Windows.Forms.CheckBoxTS chkExtRX105;
		private System.Windows.Forms.CheckBoxTS chkExtRX104;
		private System.Windows.Forms.CheckBoxTS chkExtRX123;
		private System.Windows.Forms.CheckBoxTS chkExtRX122;
		private System.Windows.Forms.CheckBoxTS chkExtRX121;
		private System.Windows.Forms.CheckBoxTS chkExtRX125;
		private System.Windows.Forms.CheckBoxTS chkExtRX124;
		private System.Windows.Forms.CheckBoxTS chkExtRX153;
		private System.Windows.Forms.CheckBoxTS chkExtRX152;
		private System.Windows.Forms.CheckBoxTS chkExtRX151;
		private System.Windows.Forms.CheckBoxTS chkExtRX155;
		private System.Windows.Forms.CheckBoxTS chkExtRX154;
		private System.Windows.Forms.CheckBoxTS chkExtRX173;
		private System.Windows.Forms.CheckBoxTS chkExtRX172;
		private System.Windows.Forms.CheckBoxTS chkExtRX171;
		private System.Windows.Forms.CheckBoxTS chkExtRX175;
		private System.Windows.Forms.CheckBoxTS chkExtRX174;
		private System.Windows.Forms.CheckBoxTS chkExtRX203;
		private System.Windows.Forms.CheckBoxTS chkExtRX202;
		private System.Windows.Forms.CheckBoxTS chkExtRX201;
		private System.Windows.Forms.CheckBoxTS chkExtRX205;
		private System.Windows.Forms.CheckBoxTS chkExtRX204;
		private System.Windows.Forms.CheckBoxTS chkExtRX303;
		private System.Windows.Forms.CheckBoxTS chkExtRX302;
		private System.Windows.Forms.CheckBoxTS chkExtRX301;
		private System.Windows.Forms.CheckBoxTS chkExtRX305;
		private System.Windows.Forms.CheckBoxTS chkExtRX304;
		private System.Windows.Forms.CheckBoxTS chkExtRX403;
		private System.Windows.Forms.CheckBoxTS chkExtRX402;
		private System.Windows.Forms.CheckBoxTS chkExtRX401;
		private System.Windows.Forms.CheckBoxTS chkExtRX405;
		private System.Windows.Forms.CheckBoxTS chkExtRX404;
		private System.Windows.Forms.CheckBoxTS chkExtRX603;
		private System.Windows.Forms.CheckBoxTS chkExtRX602;
		private System.Windows.Forms.CheckBoxTS chkExtRX601;
		private System.Windows.Forms.CheckBoxTS chkExtRX605;
		private System.Windows.Forms.CheckBoxTS chkExtRX604;
		private System.Windows.Forms.CheckBoxTS chkExtRX803;
		private System.Windows.Forms.CheckBoxTS chkExtRX802;
		private System.Windows.Forms.CheckBoxTS chkExtRX801;
		private System.Windows.Forms.CheckBoxTS chkExtRX805;
		private System.Windows.Forms.CheckBoxTS chkExtRX804;
		private System.Windows.Forms.LabelTS lblTXCompander;
		private System.Windows.Forms.TrackBarTS tbTXCompander;
		private System.Windows.Forms.NumericUpDownTS udTXCompander;
		private System.Windows.Forms.ButtonTS btnCATTest;
		private System.Windows.Forms.TabControl tcAudio;
		private System.Windows.Forms.NumericUpDownTS udAudioLineIn1;
		private System.Windows.Forms.ButtonTS btnAudioVoltTest1;
		private System.Windows.Forms.NumericUpDownTS udAudioVoltage1;
		public System.Windows.Forms.GroupBoxTS grpAudioDetails1;
		private System.Windows.Forms.ComboBoxTS comboAudioTransmit1;
		private System.Windows.Forms.LabelTS lblAudioMixer1;
		public System.Windows.Forms.LabelTS lblAudioOutput1;
		private System.Windows.Forms.ComboBoxTS comboAudioOutput1;
		public System.Windows.Forms.LabelTS lblAudioInput1;
		public System.Windows.Forms.LabelTS lblAudioDriver1;
		private System.Windows.Forms.ComboBoxTS comboAudioInput1;
		private System.Windows.Forms.ComboBoxTS comboAudioDriver1;
		private System.Windows.Forms.ComboBoxTS comboAudioMixer1;
		private System.Windows.Forms.LabelTS lblAudioTransmit1;
		private System.Windows.Forms.LabelTS lblAudioReceive1;
		private System.Windows.Forms.ComboBoxTS comboAudioReceive1;
		private System.Windows.Forms.NumericUpDownTS udAudioLatency1;
		private System.Windows.Forms.GroupBoxTS grpAudioCard;
		private System.Windows.Forms.ComboBoxTS comboAudioSoundCard;
		private System.Windows.Forms.ComboBoxTS comboAudioBuffer1;
		private System.Windows.Forms.ComboBoxTS comboAudioSampleRate1;
		private System.Windows.Forms.GroupBoxTS grpAudioLineInGain1;
		private System.Windows.Forms.GroupBoxTS grpAudioLatency1;
		private System.Windows.Forms.CheckBoxTS chkAudioLatencyManual1;
		private System.Windows.Forms.GroupBoxTS grpAudioBufferSize1;
		private System.Windows.Forms.GroupBoxTS grpAudioSampleRate1;
		private System.Windows.Forms.GroupBoxTS grpAudioDetails2;
		public System.Windows.Forms.LabelTS lblAudioOutput2;
		private System.Windows.Forms.ComboBoxTS comboAudioOutput2;
		public System.Windows.Forms.LabelTS lblAudioInput2;
		public System.Windows.Forms.LabelTS lblAudioDriver2;
		private System.Windows.Forms.ComboBoxTS comboAudioInput2;
		private System.Windows.Forms.ComboBoxTS comboAudioDriver2;
		private System.Windows.Forms.NumericUpDownTS udAudioLatency2;
		private System.Windows.Forms.ComboBoxTS comboAudioBuffer2;
		private System.Windows.Forms.ComboBoxTS comboAudioSampleRate2;
		private System.Windows.Forms.GroupBoxTS grpAudioMicInGain1;
		private System.Windows.Forms.NumericUpDownTS udAudioMicGain1;
		private System.Windows.Forms.GroupBoxTS grpAudioBuffer2;
		private System.Windows.Forms.GroupBoxTS grpAudioSampleRate2;
		private System.Windows.Forms.GroupBoxTS grpAudioLatency2;
		private System.Windows.Forms.CheckBoxTS chkAudioLatencyManual2;
		private System.Windows.Forms.GroupBoxTS grpAudioVolts1;
		private System.Windows.Forms.ComboBoxTS comboCATRigType;
		private System.Windows.Forms.ComboBoxTS comboDisplayLabelAlign;
		private System.Windows.Forms.LabelTS lblDisplayAlign;
		private System.Windows.Forms.LabelTS lblCATRigType;
		private System.Windows.Forms.TabPage tpAudioCard1;
		private System.Windows.Forms.TabPage tpDSPKeyer;
		private System.Windows.Forms.LabelTS lblCWKeyerBreakIn;
		private System.Windows.Forms.NumericUpDownTS udCWKeyerRamp;
		private System.Windows.Forms.LabelTS lblCWRamp;
		private System.Windows.Forms.CheckBoxTS chkCWKeyerIambic;
		private System.Windows.Forms.GroupBoxTS grpDSPKeyerSignalShaping;
		private System.Windows.Forms.NumericUpDownTS udCWKeyerWeight;
		private System.Windows.Forms.LabelTS lblCWWeight;
		private System.Windows.Forms.GroupBoxTS grpDSPKeyerOptions;
		private System.Windows.Forms.GroupBoxTS grpDSPKeyerSemiBreakIn;
		private System.Windows.Forms.CheckBoxTS chkDSPKeyerSemiBreakInEnabled;
		private System.Windows.Forms.NumericUpDownTS udCWKeyerSemiBreakInDelay;
		private System.Windows.Forms.GroupBoxTS grpDSPCWPitch;
		private System.Windows.Forms.LabelTS lblDSPCWPitchFreq;
		private System.Windows.Forms.NumericUpDownTS udDSPCWPitch;
		private System.Windows.Forms.CheckBoxTS chkDSPKeyerDisableMonitor;
		private System.Windows.Forms.TabControl tcDSP;
		private System.Windows.Forms.TabPage tpDSPImageReject;
		private System.Windows.Forms.GroupBoxTS grpDSPImageRejectTX;
		private System.Windows.Forms.LabelTS lblDSPGainValTX;
		private System.Windows.Forms.LabelTS lblDSPPhaseValTX;
		private System.Windows.Forms.NumericUpDownTS udDSPImageGainTX;
		private System.Windows.Forms.NumericUpDownTS udDSPImagePhaseTX;
		private System.Windows.Forms.LabelTS lblDSPImageGainTX;
		private System.Windows.Forms.TrackBarTS tbDSPImagePhaseTX;
		private System.Windows.Forms.LabelTS lblDSPImagePhaseTX;
		private System.Windows.Forms.TrackBarTS tbDSPImageGainTX;
		private System.Windows.Forms.GroupBoxTS grpDSPImageRejectRX;
		private System.Windows.Forms.LabelTS lblDSPGainValRX;
		private System.Windows.Forms.LabelTS lblDSPPhaseValRX;
		private System.Windows.Forms.NumericUpDownTS udDSPImageGainRX;
		private System.Windows.Forms.NumericUpDownTS udDSPImagePhaseRX;
		private System.Windows.Forms.LabelTS lblDSPImageGainRX;
		private System.Windows.Forms.TrackBarTS tbDSPImagePhaseRX;
		private System.Windows.Forms.LabelTS lblDSPImagePhaseRX;
		private System.Windows.Forms.TrackBarTS tbDSPImageGainRX;
		private System.Windows.Forms.TabPage tpDSPOptions;
		private System.Windows.Forms.GroupBoxTS grpDSPBufferSize;
		private System.Windows.Forms.ComboBoxTS comboDSPBufSize;
		private System.Windows.Forms.GroupBoxTS grpDSPNB;
		private System.Windows.Forms.NumericUpDownTS udDSPNB;
		private System.Windows.Forms.LabelTS lblDSPNBThreshold;
		private System.Windows.Forms.GroupBoxTS grpDSPLMSNR;
		private System.Windows.Forms.LabelTS lblLMSNRgain;
		private System.Windows.Forms.NumericUpDownTS udLMSNRgain;
		private System.Windows.Forms.NumericUpDownTS udLMSNRdelay;
		private System.Windows.Forms.LabelTS lblLMSNRdelay;
		private System.Windows.Forms.NumericUpDownTS udLMSNRtaps;
		private System.Windows.Forms.LabelTS lblLMSNRtaps;
		private System.Windows.Forms.GroupBoxTS grpDSPLMSANF;
		private System.Windows.Forms.LabelTS lblLMSANFgain;
		private System.Windows.Forms.NumericUpDownTS udLMSANFgain;
		private System.Windows.Forms.LabelTS lblLMSANFdelay;
		private System.Windows.Forms.NumericUpDownTS udLMSANFdelay;
		private System.Windows.Forms.LabelTS lblLMSANFTaps;
		private System.Windows.Forms.NumericUpDownTS udLMSANFtaps;
		private System.Windows.Forms.GroupBoxTS grpDSPAGC;
		private System.Windows.Forms.LabelTS lblDSPAGCMaxGain;
		private System.Windows.Forms.NumericUpDownTS udDSPAGCMaxGaindB;
		private System.Windows.Forms.NumericUpDownTS udDSPAGCFixedGaindB;
		private System.Windows.Forms.LabelTS lblDSPAGCFixed;
		private System.Windows.Forms.GroupBoxTS grpDSPWindow;
		private System.Windows.Forms.ComboBoxTS comboDSPWindow;
		private System.Windows.Forms.GroupBoxTS grpDSPNB2;
		private System.Windows.Forms.LabelTS lblDSPNB2Threshold;
		private System.Windows.Forms.NumericUpDownTS udDSPNB2;
		private System.Windows.Forms.LabelTS lblKeyerDeBounce;
		private System.Windows.Forms.NumericUpDownTS udCWKeyerDeBounce;
		private System.Windows.Forms.CheckBoxTS chkCWKeyerRevPdl;
		private System.Windows.Forms.CheckBoxTS chkHiPerfKeyer;
		private System.Windows.Forms.CheckBoxTS chkExtEnable;
		private System.Windows.Forms.CheckBoxTS chkExtRX26;
		private System.Windows.Forms.CheckBoxTS chkExtRX66;
		private System.Windows.Forms.CheckBoxTS chkExtRX106;
		private System.Windows.Forms.CheckBoxTS chkExtRX126;
		private System.Windows.Forms.CheckBoxTS chkExtRX156;
		private System.Windows.Forms.CheckBoxTS chkExtRX176;
		private System.Windows.Forms.CheckBoxTS chkExtRX206;
		private System.Windows.Forms.CheckBoxTS chkExtRX306;
		private System.Windows.Forms.CheckBoxTS chkExtRX406;
		private System.Windows.Forms.CheckBoxTS chkExtRX606;
		private System.Windows.Forms.CheckBoxTS chkExtRX806;
		private System.Windows.Forms.CheckBoxTS chkExtRX1606;
		private System.Windows.Forms.LabelTS lblExtRXX26;
		private System.Windows.Forms.LabelTS lblExtTXX26;
		private System.Windows.Forms.CheckBoxTS chkExtTX26;
		private System.Windows.Forms.CheckBoxTS chkExtTX66;
		private System.Windows.Forms.CheckBoxTS chkExtTX106;
		private System.Windows.Forms.CheckBoxTS chkExtTX126;
		private System.Windows.Forms.CheckBoxTS chkExtTX156;
		private System.Windows.Forms.CheckBoxTS chkExtTX176;
		private System.Windows.Forms.CheckBoxTS chkExtTX206;
		private System.Windows.Forms.CheckBoxTS chkExtTX306;
		private System.Windows.Forms.CheckBoxTS chkExtTX406;
		private System.Windows.Forms.CheckBoxTS chkExtTX606;
		private System.Windows.Forms.CheckBoxTS chkExtTX806;
		private System.Windows.Forms.CheckBoxTS chkExtTX1606;
		private System.Windows.Forms.GroupBoxTS grpTXProfile;
		private System.Windows.Forms.Button btnTXProfileSave;
		private System.Windows.Forms.ComboBoxTS comboTXProfileName;
		private System.Windows.Forms.Button btnTXProfileDelete;
		private System.Windows.Forms.Timer timer_sweep;
		private System.Windows.Forms.LabelTS lblTestGenLow;
		private System.Windows.Forms.LabelTS lblTestGenHigh;
		private System.Windows.Forms.LabelTS lblTestGenHzSec;
		private System.Windows.Forms.LabelTS lblDSPAGCAttack;
		private System.Windows.Forms.LabelTS lblDSPAGCDecay;
		private System.Windows.Forms.NumericUpDownTS udDSPAGCAttack;
		private System.Windows.Forms.LabelTS lblDSPAGCSlope;
		private System.Windows.Forms.NumericUpDownTS udDSPAGCDecay;
		private System.Windows.Forms.NumericUpDownTS udDSPAGCSlope;
		private System.Windows.Forms.NumericUpDownTS udDSPALCThreshold;
		private System.Windows.Forms.NumericUpDownTS udDSPALCSlope;
		private System.Windows.Forms.NumericUpDownTS udDSPALCDecay;
		private System.Windows.Forms.LabelTS lblDSPALCSlope;
		private System.Windows.Forms.NumericUpDownTS udDSPALCAttack;
		private System.Windows.Forms.LabelTS lblDSPALCDecay;
		private System.Windows.Forms.LabelTS lblDSPALCAttack;
		private System.Windows.Forms.LabelTS lblDSPALCThreshold;
		private System.Windows.Forms.GroupBoxTS grpDSPALC;
		private System.Windows.Forms.GroupBoxTS grpDSPLeveler;
		private System.Windows.Forms.NumericUpDownTS udDSPLevelerThreshold;
		private System.Windows.Forms.NumericUpDownTS udDSPLevelerSlope;
		private System.Windows.Forms.NumericUpDownTS udDSPLevelerDecay;
		private System.Windows.Forms.LabelTS lblDSPLevelerSlope;
		private System.Windows.Forms.NumericUpDownTS udDSPLevelerAttack;
		private System.Windows.Forms.LabelTS lblDSPLevelerDecay;
		private System.Windows.Forms.LabelTS lblDSPLevelerAttack;
		private System.Windows.Forms.LabelTS lblDSPLevelerThreshold;
		private System.Windows.Forms.GroupBoxTS grpTXNoiseGate;
		private System.Windows.Forms.LabelTS lblTXNoiseGateThreshold;
		private System.Windows.Forms.NumericUpDownTS udTXNoiseGate;
		private System.Windows.Forms.CheckBoxTS chkTXNoiseGateEnabled;
		private System.Windows.Forms.TabPage tpDSPAGCALC;
		private System.Windows.Forms.TrackBarTS tbDSPLevelerHangThreshold;
		private System.Windows.Forms.LabelTS lblDSPLevelerHangThreshold;
		private System.Windows.Forms.LabelTS lblDSPALCHangThreshold;
		private System.Windows.Forms.TrackBarTS tbDSPALCHangThreshold;
		private System.Windows.Forms.NumericUpDownTS udDSPLevelerHangTime;
		private System.Windows.Forms.LabelTS lblDSPLevelerHangTime;
		private System.Windows.Forms.NumericUpDownTS udDSPALCHangTime;
		private System.Windows.Forms.LabelTS lblDSPALCHangTime;
		private System.Windows.Forms.GroupBoxTS grpTXVOX;
		private System.Windows.Forms.LabelTS lblTXVOXThreshold;
		private System.Windows.Forms.NumericUpDownTS udTXVOXThreshold;
		private System.Windows.Forms.CheckBoxTS chkTXVOXEnabled;
		private System.Windows.Forms.LabelTS lblTXVOXHangTime;
		private System.Windows.Forms.NumericUpDownTS udTXVOXHangTime;
		private System.Windows.Forms.TrackBarTS tbDSPAGCHangThreshold;
		private System.Windows.Forms.LabelTS lblDSPAGCHangThreshold;
		private System.Windows.Forms.LabelTS lblDSPAGCHangTime;
		private System.Windows.Forms.NumericUpDownTS udDSPAGCHangTime;
		private System.Windows.Forms.CheckBoxTS chkDSPLevelerEnabled;
		private System.Windows.Forms.Button btnImpulse;
		private System.Windows.Forms.NumericUpDownTS udImpulseNum;
		private System.Windows.Forms.GroupBoxTS grpTXMonitor;
		private System.Windows.Forms.LabelTS lblTXAF;
		private System.Windows.Forms.NumericUpDownTS udTXAF;
		private System.Windows.Forms.GroupBoxTS grpGeneralModel;
		private System.Windows.Forms.TabControl tcGeneral;
		private System.Windows.Forms.TabPage tpGeneralHardware;
		private System.Windows.Forms.TabPage tpGeneralOptions;
		private System.Windows.Forms.TabPage tpGeneralCalibration;
		private System.Windows.Forms.RadioButtonTS radGenModelSDR1000;
		private System.Windows.Forms.RadioButtonTS radGenModelSoftRock40;
		private System.Windows.Forms.RadioButtonTS radGenModelDemoNone;
		private System.Windows.Forms.NumericUpDownTS udGeneralCalFreq1;
		private System.Windows.Forms.NumericUpDownTS udGeneralCalFreq3;
		private System.Windows.Forms.NumericUpDownTS udGeneralCalFreq2;
		private System.Windows.Forms.NumericUpDownTS udSoftRockCenterFreq;
		private System.Windows.Forms.GroupBoxTS grpHWSoftRock;
		private System.Windows.Forms.TabPage tpVAC;
		public System.Windows.Forms.CheckBoxTS chkAudioEnableVAC;
		private System.Windows.Forms.GroupBoxTS grpAudio2Stereo;
		private System.Windows.Forms.GroupBoxTS grpBoxTS1;
		private System.Windows.Forms.TrackBarTS tkbarTestGenFreq;
		private System.Windows.Forms.LabelTS lblKeyerConnPrimary;
		private System.Windows.Forms.ComboBoxTS comboKeyerConnPrimary;
		private System.Windows.Forms.ComboBoxTS comboKeyerConnKeyLine;
		private System.Windows.Forms.ComboBoxTS comboKeyerConnSecondary;
		private System.Windows.Forms.LabelTS lblKeyerConnSecondary;
		private System.Windows.Forms.ComboBoxTS comboKeyerConnPTTLine;
		private System.Windows.Forms.LabelTS lblKeyerConnPTTLine;
		private System.Windows.Forms.LabelTS lblKeyerConnKeyLine;
		private System.Windows.Forms.LabelTS lblCATPort;
		private System.Windows.Forms.ComboBoxTS comboCATPort;
		private System.Windows.Forms.ComboBoxTS comboCATPTTPort;
		private System.Windows.Forms.CheckBoxTS chkCATPTTEnabled;
		private System.Windows.Forms.CheckBoxTS checkboxTXImagCal;
		private System.Windows.Forms.GroupBoxTS grpAudioChannels;
		private System.Windows.Forms.ComboBoxTS comboAudioChannels1;
		private System.Windows.Forms.GroupBoxTS grpAudioVACGain;
		private System.Windows.Forms.NumericUpDownTS udAudioVACGainRX;
		public System.Windows.Forms.LabelTS lblAudioVACGainRX;
		public System.Windows.Forms.LabelTS lblAudioVACGainTX;
		private System.Windows.Forms.NumericUpDownTS udAudioVACGainTX;
		private System.Windows.Forms.LabelTS lblTestGenScale;
		private System.Windows.Forms.GroupBoxTS grpGenTuningOptions;
		private System.Windows.Forms.GroupBoxTS grpAudioVACAutoEnable;
		private System.Windows.Forms.CheckBoxTS chkAudioVACAutoEnable;
		private System.Windows.Forms.CheckBoxTS chkSpectrumPolyphase;
		private System.Windows.Forms.GroupBoxTS grpImpulseTest;
		private System.Windows.Forms.GroupBoxTS grpDisplayScopeMode;
		private System.Windows.Forms.NumericUpDownTS udDisplayScopeTime;
		private System.Windows.Forms.LabelTS lblDisplayScopeTime;
		private System.Windows.Forms.NumericUpDownTS udDisplayMeterAvg;
		private System.Windows.Forms.LabelTS lblDisplayMeterAvg;
		private System.Windows.Forms.GroupBoxTS grpDisplayPolyPhase;
		private System.Windows.Forms.ComboBoxTS comboDisplayDriver;
		private System.Windows.Forms.GroupBoxTS grpDisplayDriverEngine;
		private System.Windows.Forms.GroupBoxTS grpGenAutoMute;
		private System.Windows.Forms.CheckBoxTS chkGenAutoMute;
		private PowerSDR.ColorButton clrbtnOutOfBand;
		private System.Windows.Forms.LabelTS lblOutOfBand;
		private System.Windows.Forms.Label lblGenSoftRockCenterFreq;
		private System.Windows.Forms.LabelTS lblTestSigGenFreqCallout;
		private System.Windows.Forms.NumericUpDownTS udTestGenHzSec;
		private System.Windows.Forms.NumericUpDownTS udTestGenHigh;
		private System.Windows.Forms.NumericUpDownTS udTestGenLow;
		private System.Windows.Forms.ButtonTS btnTestGenSweep;
		private System.Windows.Forms.NumericUpDownTS udTestGenScale;
		private System.Windows.Forms.RadioButtonTS radTestGenOutput;
		private System.Windows.Forms.RadioButtonTS radTestGenInput;
		private System.Windows.Forms.ComboBoxTS cmboTestGenMode;
		private System.Windows.Forms.CheckBoxTS chkBlkLMSNR;
		private System.Windows.Forms.CheckBoxTS chkBlkLMSANF;
		private System.Windows.Forms.CheckBoxTS chkAudio2Stereo;
		private System.Windows.Forms.GroupBoxTS grpTXAM;
		private System.Windows.Forms.LabelTS lblTXAMCarrierLevel;
		private System.Windows.Forms.NumericUpDownTS udTXAMCarrierLevel;
		private System.Windows.Forms.GroupBoxTS grpOptQuickQSY;
		private System.Windows.Forms.CheckBoxTS chkOptQuickQSY;
		private System.Windows.Forms.GroupBoxTS grpOptMainConsole;
		private System.Windows.Forms.CheckBoxTS chkOptAlwaysOnTop;
		private System.Windows.Forms.NumericUpDownTS udOptClickTuneOffsetDIGL;
		private System.Windows.Forms.NumericUpDownTS udOptClickTuneOffsetDIGU;
		private System.Windows.Forms.LabelTS lblOptClickTuneDIGL;
		private System.Windows.Forms.LabelTS lblOptClickTuneDIGU;
		private System.Windows.Forms.GroupBoxTS grpAudioMicBoost;
		private System.Windows.Forms.CheckBoxTS chkAudioMicBoost;
		private System.Windows.Forms.GroupBoxTS grpOptFilterControls;
		private System.Windows.Forms.LabelTS lblOptMaxFilter;
		private System.Windows.Forms.NumericUpDownTS udOptMaxFilterWidth;
		private System.Windows.Forms.LabelTS lblOptWidthSliderMode;
		private System.Windows.Forms.ComboBoxTS comboOptFilterWidthMode;
		private System.Windows.Forms.NumericUpDownTS udOptMaxFilterShift;
		private System.Windows.Forms.LabelTS lblOptMaxFilterShift;
		private System.Windows.Forms.CheckBoxTS chkOptFilterSaveChanges;
		private System.Windows.Forms.CheckBoxTS chkOptEnableKBShortcuts;
		private System.Windows.Forms.TabControl tcAppearance;
		private System.Windows.Forms.TabPage tpAppearanceGeneral;
		private System.Windows.Forms.TabPage tpAppearanceDisplay;
		private System.Windows.Forms.TabPage tpAppearanceMeter;
		private System.Windows.Forms.GroupBoxTS grpAppearanceVFO;
		private System.Windows.Forms.LabelTS lblVFOPowerOn;
		private System.Windows.Forms.LabelTS lblVFOPowerOff;
		private System.Windows.Forms.GroupBoxTS grpAppearanceBand;
		private System.Windows.Forms.TabPage tpFilters;
		private System.Windows.Forms.LabelTS lblDefaultLowCut;
		private System.Windows.Forms.NumericUpDownTS udFilterDefaultLowCut;
		private System.Windows.Forms.CheckBoxTS chkVFOSmallLSD;
		private PowerSDR.ColorButton clrbtnVFOSmallColor;
		private PowerSDR.ColorButton clrbtnBandBackground;
		private System.Windows.Forms.LabelTS lblBandBackground;
		private PowerSDR.ColorButton clrbtnVFOBackground;
		private System.Windows.Forms.LabelTS lblVFOBackground;
		private System.Windows.Forms.GroupBoxTS grpDisplayPeakCursor;
		private PowerSDR.ColorButton clrbtnPeakBackground;
		private System.Windows.Forms.LabelTS lblPeakBackground;
		private PowerSDR.ColorButton clrbtnMeterBackground;
		private System.Windows.Forms.LabelTS lblMeterBackground;
		private PowerSDR.ColorButton clrbtnTXFilter;
		private System.Windows.Forms.GroupBoxTS grpAppPanadapter;
		private PowerSDR.ColorButton clrbtnBandEdge;
		private System.Windows.Forms.LabelTS lblBandEdge;
		private System.Windows.Forms.CheckBoxTS chkShowFreqOffset;
		private System.Windows.Forms.ComboBoxTS comboMeterType;
		private PowerSDR.ColorButton clrbtnMeterEdgeBackground;
		private PowerSDR.ColorButton clrbtnMeterEdgeHigh;
		private PowerSDR.ColorButton clrbtnMeterEdgeLow;
		private System.Windows.Forms.GroupBoxTS grpGenCalRXImage;
		private System.Windows.Forms.LabelTS lblGenCalRXImageFreq;
		private System.Windows.Forms.GroupBoxTS grpGenCalLevel;
		private System.Windows.Forms.LabelTS lblGenCalLevelFreq;
		private System.Windows.Forms.GroupBoxTS grpKeyerConnections;
		private System.Windows.Forms.LabelTS lblCompand;
		private System.Windows.Forms.LabelTS lblVFOSmallColor;
		private System.Windows.Forms.LabelTS lblTXFilterColor;
		private System.Windows.Forms.LabelTS lblMeterType;
		private System.Windows.Forms.CheckBoxTS chekTestIMD;
		private System.Windows.Forms.GroupBoxTS grpMeterEdge;
		private System.Windows.Forms.LabelTS lblMeterEdgeBackground;
		private System.Windows.Forms.LabelTS lblMeterEdgeHigh;
		private System.Windows.Forms.LabelTS lblMeterEdgeLow;
		private PowerSDR.ColorButton clrbtnEdgeIndicator;
		private System.Windows.Forms.LabelTS labelTS1;
		private System.Windows.Forms.LabelTS lblMeterDigitalText;
		private PowerSDR.ColorButton clrbtnMeterDigText;
		private System.Windows.Forms.LabelTS labelTS2;
		private PowerSDR.ColorButton clrbtnMeterDigBackground;
		private PowerSDR.ColorButton clrbtnSubRXFilter;
		private System.Windows.Forms.LabelTS lblSubRXFilterColor;
		private PowerSDR.ColorButton clrbtnSubRXZero;
		private System.Windows.Forms.LabelTS lblSubRXZeroLine;
		private System.Windows.Forms.CheckBoxTS chkCWKeyerMode;
		private System.Windows.Forms.CheckBoxTS chkBoxJanusOzyControl;
		private System.ComponentModel.IContainer components;

		#endregion

		#region Constructor and Destructor

		public Setup(Console c)
		{
			InitializeComponent();
			console = c;

#if(!DEBUG)
			comboGeneralProcessPriority.Items.Remove("Idle");
			comboGeneralProcessPriority.Items.Remove("Below Normal");
#endif
			initializing = true;

			InitWindowTypes();
			GetMixerDevices();
			GetHosts();			
			
			KeyList = new ArrayList();
			SetupKeyMap();

			GetTxProfiles();

			RefreshCOMPortLists();

			comboGeneralLPTAddr.SelectedIndex = -1;
			comboGeneralXVTR.SelectedIndex = (int)XVTRTRMode.POSITIVE;
			comboGeneralProcessPriority.Text = "Normal";
			comboOptFilterWidthMode.Text = "Linear";
			comboAudioSoundCard.Text = "Unsupported Card";
			comboAudioSampleRate1.SelectedIndex = 0;
			comboAudioSampleRate2.Text = "11025";
			comboAudioBuffer1.Text = "2048";
			comboAudioBuffer2.Text = "512";
			comboAudioChannels1.Text = "2";
			comboDisplayLabelAlign.Text = "Auto";
			comboDisplayDriver.Text = "GDI+";
			comboDSPBufSize.Text = "2048";
			comboDSPWindow.SelectedIndex = (int)DttSP.Window.HANNING;
			comboKeyerConnKeyLine.SelectedIndex = 0;
			comboKeyerConnSecondary.SelectedIndex = 0;
			comboKeyerConnPTTLine.SelectedIndex = 0;
			comboKeyerConnPrimary.SelectedIndex = 0;
			comboMeterType.Text = "Edge";
			if(comboCATPort.Items.Count > 0) comboCATPort.SelectedIndex = 0;
			if(comboCATPTTPort.Items.Count > 0) comboCATPTTPort.SelectedIndex = 0;
			comboCATbaud.Text = "1200";
			comboCATparity.Text = "none";
			comboCATdatabits.Text = "8";
			comboCATstopbits.Text = "1";
			comboCATRigType.Text = "TS-2000";

			GetOptions();

			cmboTestGenMode.Text = "Off";

			if(comboAudioDriver1.SelectedIndex < 0 &&
				comboAudioDriver1.Items.Count > 0)
				comboAudioDriver1.SelectedIndex = 0;

			if(comboAudioDriver2.SelectedIndex < 0 &&
				comboAudioDriver2.Items.Count > 0)
				comboAudioDriver2.SelectedIndex = 0;

			if(comboAudioMixer1.SelectedIndex < 0 &&
				comboAudioMixer1.Items.Count > 0)
				comboAudioMixer1.SelectedIndex = 0;

			initializing = false;

			if(comboTXProfileName.SelectedIndex < 0 &&
				comboTXProfileName.Items.Count > 0)
				comboTXProfileName.SelectedIndex = 0;

			if(chkCATEnable.Checked)
			{
				chkCATEnable_CheckedChanged(this, EventArgs.Empty);
			}

			if(chkCATPTTEnabled.Checked)
			{
				chkCATPTTEnabled_CheckedChanged(this, EventArgs.Empty);
			}

			comboKeyerConnSecondary_SelectedIndexChanged(this, EventArgs.Empty);

			//ForceAllEvents();
			EventArgs e = EventArgs.Empty;
			comboGeneralLPTAddr_LostFocus(this, e);
			chkGeneralSpurRed_CheckedChanged(this, e);
			chkGeneralRFEPresent_CheckedChanged(this, e);
			udDDSCorrection_ValueChanged(this, e);
			chkAudioLatencyManual1_CheckedChanged(this, e);
			udAudioLineIn1_ValueChanged(this, e);
			comboAudioReceive1_SelectedIndexChanged(this, e);
			udLMSANF_ValueChanged(this, e);
			udLMSNR_ValueChanged(this, e);			
			udDSPImagePhaseRX_ValueChanged(this, e);
			udDSPImageGainRX_ValueChanged(this, e);
			udDSPImagePhaseTX_ValueChanged(this, e);
			udDSPImageGainTX_ValueChanged(this, e);
			udDSPCWPitch_ValueChanged(this, e);
			tbDSPAGCHangThreshold_Scroll(this, e);
			udTXFilterHigh_ValueChanged(this, e);
			udTXFilterLow_ValueChanged(this, e);
			
			openFileDialog1.Filter = "PowerSDR Database Files (*.mdb) | *.mdb";
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Setup));
			this.tcSetup = new System.Windows.Forms.TabControl();
			this.tpGeneral = new System.Windows.Forms.TabPage();
			this.tcGeneral = new System.Windows.Forms.TabControl();
			this.tpGeneralHardware = new System.Windows.Forms.TabPage();
			this.grpHWSoftRock = new System.Windows.Forms.GroupBoxTS();
			this.lblGenSoftRockCenterFreq = new System.Windows.Forms.Label();
			this.udSoftRockCenterFreq = new System.Windows.Forms.NumericUpDownTS();
			this.grpGeneralDDS = new System.Windows.Forms.GroupBoxTS();
			this.udDDSCorrection = new System.Windows.Forms.NumericUpDownTS();
			this.lblClockCorrection = new System.Windows.Forms.LabelTS();
			this.udDDSIFFreq = new System.Windows.Forms.NumericUpDownTS();
			this.lblIFFrequency = new System.Windows.Forms.LabelTS();
			this.udDDSPLLMult = new System.Windows.Forms.NumericUpDownTS();
			this.lblPLLMult = new System.Windows.Forms.LabelTS();
			this.grpGeneralModel = new System.Windows.Forms.GroupBoxTS();
			this.radGenModelDemoNone = new System.Windows.Forms.RadioButtonTS();
			this.radGenModelSoftRock40 = new System.Windows.Forms.RadioButtonTS();
			this.radGenModelSDR1000 = new System.Windows.Forms.RadioButtonTS();
			this.grpGeneralHardwareSetup = new System.Windows.Forms.GroupBoxTS();
			this.chkBoxJanusOzyControl = new System.Windows.Forms.CheckBoxTS();
			this.chkGeneralRXOnly = new System.Windows.Forms.CheckBoxTS();
			this.chkGeneralUSBPresent = new System.Windows.Forms.CheckBoxTS();
			this.chkGeneralATUPresent = new System.Windows.Forms.CheckBoxTS();
			this.chkGeneralPAPresent = new System.Windows.Forms.CheckBoxTS();
			this.chkGeneralXVTRPresent = new System.Windows.Forms.CheckBoxTS();
			this.lblGeneralLPTDelay = new System.Windows.Forms.LabelTS();
			this.udGeneralLPTDelay = new System.Windows.Forms.NumericUpDownTS();
			this.lblGeneralLPTAddr = new System.Windows.Forms.LabelTS();
			this.comboGeneralLPTAddr = new System.Windows.Forms.ComboBoxTS();
			this.chkGeneralRFEPresent = new System.Windows.Forms.CheckBoxTS();
			this.comboGeneralXVTR = new System.Windows.Forms.ComboBoxTS();
			this.btnWizard = new System.Windows.Forms.ButtonTS();
			this.tpGeneralOptions = new System.Windows.Forms.TabPage();
			this.grpOptMainConsole = new System.Windows.Forms.GroupBoxTS();
			this.chkOptAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
			this.grpOptQuickQSY = new System.Windows.Forms.GroupBoxTS();
			this.chkOptEnableKBShortcuts = new System.Windows.Forms.CheckBoxTS();
			this.chkOptQuickQSY = new System.Windows.Forms.CheckBoxTS();
			this.grpGenAutoMute = new System.Windows.Forms.GroupBoxTS();
			this.chkGenAutoMute = new System.Windows.Forms.CheckBoxTS();
			this.grpGenTuningOptions = new System.Windows.Forms.GroupBoxTS();
			this.lblOptClickTuneDIGL = new System.Windows.Forms.LabelTS();
			this.udOptClickTuneOffsetDIGL = new System.Windows.Forms.NumericUpDownTS();
			this.lblOptClickTuneDIGU = new System.Windows.Forms.LabelTS();
			this.udOptClickTuneOffsetDIGU = new System.Windows.Forms.NumericUpDownTS();
			this.grpGeneralOptions = new System.Windows.Forms.GroupBoxTS();
			this.chkGeneralCustomFilter = new System.Windows.Forms.CheckBoxTS();
			this.lblGeneralX2Delay = new System.Windows.Forms.LabelTS();
			this.udGeneralX2Delay = new System.Windows.Forms.NumericUpDownTS();
			this.chkGeneralEnableX2 = new System.Windows.Forms.CheckBoxTS();
			this.chkGeneralSoftwareGainCorr = new System.Windows.Forms.CheckBoxTS();
			this.chkGeneralDisablePTT = new System.Windows.Forms.CheckBoxTS();
			this.chkGeneralSpurRed = new System.Windows.Forms.CheckBoxTS();
			this.grpGeneralProcessPriority = new System.Windows.Forms.GroupBoxTS();
			this.comboGeneralProcessPriority = new System.Windows.Forms.ComboBoxTS();
			this.grpGeneralUpdates = new System.Windows.Forms.GroupBoxTS();
			this.chkGeneralUpdateBeta = new System.Windows.Forms.CheckBoxTS();
			this.chkGeneralUpdateRelease = new System.Windows.Forms.CheckBoxTS();
			this.tpGeneralCalibration = new System.Windows.Forms.TabPage();
			this.grpGenCalRXImage = new System.Windows.Forms.GroupBoxTS();
			this.udGeneralCalFreq3 = new System.Windows.Forms.NumericUpDownTS();
			this.lblGenCalRXImageFreq = new System.Windows.Forms.LabelTS();
			this.btnGeneralCalImageStart = new System.Windows.Forms.ButtonTS();
			this.grpGenCalLevel = new System.Windows.Forms.GroupBoxTS();
			this.udGeneralCalLevel = new System.Windows.Forms.NumericUpDownTS();
			this.udGeneralCalFreq2 = new System.Windows.Forms.NumericUpDownTS();
			this.lblGenCalLevelFreq = new System.Windows.Forms.LabelTS();
			this.lblGeneralCalLevel = new System.Windows.Forms.LabelTS();
			this.btnGeneralCalLevelStart = new System.Windows.Forms.ButtonTS();
			this.grpGeneralCalibration = new System.Windows.Forms.GroupBoxTS();
			this.btnGeneralCalFreqStart = new System.Windows.Forms.ButtonTS();
			this.udGeneralCalFreq1 = new System.Windows.Forms.NumericUpDownTS();
			this.lblGeneralCalFrequency = new System.Windows.Forms.LabelTS();
			this.tpFilters = new System.Windows.Forms.TabPage();
			this.grpOptFilterControls = new System.Windows.Forms.GroupBoxTS();
			this.udFilterDefaultLowCut = new System.Windows.Forms.NumericUpDownTS();
			this.lblDefaultLowCut = new System.Windows.Forms.LabelTS();
			this.udOptMaxFilterShift = new System.Windows.Forms.NumericUpDownTS();
			this.lblOptMaxFilterShift = new System.Windows.Forms.LabelTS();
			this.comboOptFilterWidthMode = new System.Windows.Forms.ComboBoxTS();
			this.lblOptWidthSliderMode = new System.Windows.Forms.LabelTS();
			this.udOptMaxFilterWidth = new System.Windows.Forms.NumericUpDownTS();
			this.lblOptMaxFilter = new System.Windows.Forms.LabelTS();
			this.chkOptFilterSaveChanges = new System.Windows.Forms.CheckBoxTS();
			this.tpAudio = new System.Windows.Forms.TabPage();
			this.tcAudio = new System.Windows.Forms.TabControl();
			this.tpAudioCard1 = new System.Windows.Forms.TabPage();
			this.grpAudioMicBoost = new System.Windows.Forms.GroupBoxTS();
			this.chkAudioMicBoost = new System.Windows.Forms.CheckBoxTS();
			this.grpAudioChannels = new System.Windows.Forms.GroupBoxTS();
			this.comboAudioChannels1 = new System.Windows.Forms.ComboBoxTS();
			this.grpAudioMicInGain1 = new System.Windows.Forms.GroupBoxTS();
			this.udAudioMicGain1 = new System.Windows.Forms.NumericUpDownTS();
			this.grpAudioLineInGain1 = new System.Windows.Forms.GroupBoxTS();
			this.udAudioLineIn1 = new System.Windows.Forms.NumericUpDownTS();
			this.grpAudioVolts1 = new System.Windows.Forms.GroupBoxTS();
			this.btnAudioVoltTest1 = new System.Windows.Forms.ButtonTS();
			this.udAudioVoltage1 = new System.Windows.Forms.NumericUpDownTS();
			this.grpAudioDetails1 = new System.Windows.Forms.GroupBoxTS();
			this.comboAudioTransmit1 = new System.Windows.Forms.ComboBoxTS();
			this.lblAudioMixer1 = new System.Windows.Forms.LabelTS();
			this.lblAudioOutput1 = new System.Windows.Forms.LabelTS();
			this.comboAudioOutput1 = new System.Windows.Forms.ComboBoxTS();
			this.lblAudioInput1 = new System.Windows.Forms.LabelTS();
			this.lblAudioDriver1 = new System.Windows.Forms.LabelTS();
			this.comboAudioInput1 = new System.Windows.Forms.ComboBoxTS();
			this.comboAudioDriver1 = new System.Windows.Forms.ComboBoxTS();
			this.comboAudioMixer1 = new System.Windows.Forms.ComboBoxTS();
			this.lblAudioTransmit1 = new System.Windows.Forms.LabelTS();
			this.lblAudioReceive1 = new System.Windows.Forms.LabelTS();
			this.comboAudioReceive1 = new System.Windows.Forms.ComboBoxTS();
			this.grpAudioLatency1 = new System.Windows.Forms.GroupBoxTS();
			this.chkAudioLatencyManual1 = new System.Windows.Forms.CheckBoxTS();
			this.udAudioLatency1 = new System.Windows.Forms.NumericUpDownTS();
			this.grpAudioCard = new System.Windows.Forms.GroupBoxTS();
			this.comboAudioSoundCard = new System.Windows.Forms.ComboBoxTS();
			this.grpAudioBufferSize1 = new System.Windows.Forms.GroupBoxTS();
			this.comboAudioBuffer1 = new System.Windows.Forms.ComboBoxTS();
			this.grpAudioSampleRate1 = new System.Windows.Forms.GroupBoxTS();
			this.comboAudioSampleRate1 = new System.Windows.Forms.ComboBoxTS();
			this.tpVAC = new System.Windows.Forms.TabPage();
			this.grpAudioVACAutoEnable = new System.Windows.Forms.GroupBoxTS();
			this.chkAudioVACAutoEnable = new System.Windows.Forms.CheckBoxTS();
			this.grpAudioVACGain = new System.Windows.Forms.GroupBoxTS();
			this.lblAudioVACGainTX = new System.Windows.Forms.LabelTS();
			this.udAudioVACGainTX = new System.Windows.Forms.NumericUpDownTS();
			this.lblAudioVACGainRX = new System.Windows.Forms.LabelTS();
			this.udAudioVACGainRX = new System.Windows.Forms.NumericUpDownTS();
			this.grpAudio2Stereo = new System.Windows.Forms.GroupBoxTS();
			this.chkAudio2Stereo = new System.Windows.Forms.CheckBoxTS();
			this.grpAudioLatency2 = new System.Windows.Forms.GroupBoxTS();
			this.chkAudioLatencyManual2 = new System.Windows.Forms.CheckBoxTS();
			this.udAudioLatency2 = new System.Windows.Forms.NumericUpDownTS();
			this.grpAudioSampleRate2 = new System.Windows.Forms.GroupBoxTS();
			this.comboAudioSampleRate2 = new System.Windows.Forms.ComboBoxTS();
			this.grpAudioBuffer2 = new System.Windows.Forms.GroupBoxTS();
			this.comboAudioBuffer2 = new System.Windows.Forms.ComboBoxTS();
			this.grpAudioDetails2 = new System.Windows.Forms.GroupBoxTS();
			this.lblAudioOutput2 = new System.Windows.Forms.LabelTS();
			this.comboAudioOutput2 = new System.Windows.Forms.ComboBoxTS();
			this.lblAudioInput2 = new System.Windows.Forms.LabelTS();
			this.lblAudioDriver2 = new System.Windows.Forms.LabelTS();
			this.comboAudioInput2 = new System.Windows.Forms.ComboBoxTS();
			this.comboAudioDriver2 = new System.Windows.Forms.ComboBoxTS();
			this.chkAudioEnableVAC = new System.Windows.Forms.CheckBoxTS();
			this.tpDisplay = new System.Windows.Forms.TabPage();
			this.grpDisplayDriverEngine = new System.Windows.Forms.GroupBoxTS();
			this.comboDisplayDriver = new System.Windows.Forms.ComboBoxTS();
			this.grpDisplayPolyPhase = new System.Windows.Forms.GroupBoxTS();
			this.chkSpectrumPolyphase = new System.Windows.Forms.CheckBoxTS();
			this.grpDisplayScopeMode = new System.Windows.Forms.GroupBoxTS();
			this.udDisplayScopeTime = new System.Windows.Forms.NumericUpDownTS();
			this.lblDisplayScopeTime = new System.Windows.Forms.LabelTS();
			this.grpDisplayMultimeter = new System.Windows.Forms.GroupBoxTS();
			this.udDisplayMeterAvg = new System.Windows.Forms.NumericUpDownTS();
			this.lblDisplayMeterAvg = new System.Windows.Forms.LabelTS();
			this.udDisplayMultiTextHoldTime = new System.Windows.Forms.NumericUpDownTS();
			this.lblDisplayMeterTextHoldTime = new System.Windows.Forms.LabelTS();
			this.udDisplayMultiPeakHoldTime = new System.Windows.Forms.NumericUpDownTS();
			this.lblDisplayMultiPeakHoldTime = new System.Windows.Forms.LabelTS();
			this.grpDisplayWaterfall = new System.Windows.Forms.GroupBoxTS();
			this.clrbtnWaterfallMid = new PowerSDR.ColorButton();
			this.clrbtnWaterfallHigh = new PowerSDR.ColorButton();
			this.clrbtnWaterfallLow = new PowerSDR.ColorButton();
			this.lblDisplayWaterfallMidColor = new System.Windows.Forms.LabelTS();
			this.lblDisplayWaterfallHighColor = new System.Windows.Forms.LabelTS();
			this.lblDisplayWaterfallLowColor = new System.Windows.Forms.LabelTS();
			this.lblDisplayWaterfallLowLevel = new System.Windows.Forms.LabelTS();
			this.udDisplayWaterfallLowLevel = new System.Windows.Forms.NumericUpDownTS();
			this.lblDisplayWaterfallHighLevel = new System.Windows.Forms.LabelTS();
			this.udDisplayWaterfallHighLevel = new System.Windows.Forms.NumericUpDownTS();
			this.grpDisplayRefreshRates = new System.Windows.Forms.GroupBoxTS();
			this.udDisplayCPUMeter = new System.Windows.Forms.NumericUpDownTS();
			this.lblDisplayCPUMeter = new System.Windows.Forms.LabelTS();
			this.udDisplayPeakText = new System.Windows.Forms.NumericUpDownTS();
			this.lblDisplayPeakText = new System.Windows.Forms.LabelTS();
			this.udDisplayMeterDelay = new System.Windows.Forms.NumericUpDownTS();
			this.lblDisplayMeterDelay = new System.Windows.Forms.LabelTS();
			this.udDisplayFPS = new System.Windows.Forms.NumericUpDownTS();
			this.lblDisplayFPS = new System.Windows.Forms.LabelTS();
			this.grpDisplayAverage = new System.Windows.Forms.GroupBoxTS();
			this.udDisplayAVGTime = new System.Windows.Forms.NumericUpDownTS();
			this.lblDisplayAVGTime = new System.Windows.Forms.LabelTS();
			this.grpDisplayPhase = new System.Windows.Forms.GroupBoxTS();
			this.lblDisplayPhasePts = new System.Windows.Forms.LabelTS();
			this.udDisplayPhasePts = new System.Windows.Forms.NumericUpDownTS();
			this.grpDisplaySpectrumGrid = new System.Windows.Forms.GroupBoxTS();
			this.comboDisplayLabelAlign = new System.Windows.Forms.ComboBoxTS();
			this.lblDisplayAlign = new System.Windows.Forms.LabelTS();
			this.udDisplayGridStep = new System.Windows.Forms.NumericUpDownTS();
			this.udDisplayGridMin = new System.Windows.Forms.NumericUpDownTS();
			this.udDisplayGridMax = new System.Windows.Forms.NumericUpDownTS();
			this.lblDisplayGridStep = new System.Windows.Forms.LabelTS();
			this.lblDisplayGridMin = new System.Windows.Forms.LabelTS();
			this.lblDisplayGridMax = new System.Windows.Forms.LabelTS();
			this.tpDSP = new System.Windows.Forms.TabPage();
			this.tcDSP = new System.Windows.Forms.TabControl();
			this.tpDSPOptions = new System.Windows.Forms.TabPage();
			this.grpDSPBufferSize = new System.Windows.Forms.GroupBoxTS();
			this.comboDSPBufSize = new System.Windows.Forms.ComboBoxTS();
			this.grpDSPNB = new System.Windows.Forms.GroupBoxTS();
			this.udDSPNB = new System.Windows.Forms.NumericUpDownTS();
			this.lblDSPNBThreshold = new System.Windows.Forms.LabelTS();
			this.grpDSPLMSNR = new System.Windows.Forms.GroupBoxTS();
			this.chkBlkLMSNR = new System.Windows.Forms.CheckBoxTS();
			this.lblLMSNRgain = new System.Windows.Forms.LabelTS();
			this.udLMSNRgain = new System.Windows.Forms.NumericUpDownTS();
			this.udLMSNRdelay = new System.Windows.Forms.NumericUpDownTS();
			this.lblLMSNRdelay = new System.Windows.Forms.LabelTS();
			this.udLMSNRtaps = new System.Windows.Forms.NumericUpDownTS();
			this.lblLMSNRtaps = new System.Windows.Forms.LabelTS();
			this.grpDSPLMSANF = new System.Windows.Forms.GroupBoxTS();
			this.chkBlkLMSANF = new System.Windows.Forms.CheckBoxTS();
			this.lblLMSANFgain = new System.Windows.Forms.LabelTS();
			this.udLMSANFgain = new System.Windows.Forms.NumericUpDownTS();
			this.lblLMSANFdelay = new System.Windows.Forms.LabelTS();
			this.udLMSANFdelay = new System.Windows.Forms.NumericUpDownTS();
			this.lblLMSANFTaps = new System.Windows.Forms.LabelTS();
			this.udLMSANFtaps = new System.Windows.Forms.NumericUpDownTS();
			this.grpDSPWindow = new System.Windows.Forms.GroupBoxTS();
			this.comboDSPWindow = new System.Windows.Forms.ComboBoxTS();
			this.grpDSPNB2 = new System.Windows.Forms.GroupBoxTS();
			this.udDSPNB2 = new System.Windows.Forms.NumericUpDownTS();
			this.lblDSPNB2Threshold = new System.Windows.Forms.LabelTS();
			this.tpDSPImageReject = new System.Windows.Forms.TabPage();
			this.grpDSPImageRejectRX = new System.Windows.Forms.GroupBoxTS();
			this.lblDSPGainValRX = new System.Windows.Forms.LabelTS();
			this.lblDSPPhaseValRX = new System.Windows.Forms.LabelTS();
			this.udDSPImageGainRX = new System.Windows.Forms.NumericUpDownTS();
			this.udDSPImagePhaseRX = new System.Windows.Forms.NumericUpDownTS();
			this.lblDSPImageGainRX = new System.Windows.Forms.LabelTS();
			this.tbDSPImagePhaseRX = new System.Windows.Forms.TrackBarTS();
			this.lblDSPImagePhaseRX = new System.Windows.Forms.LabelTS();
			this.tbDSPImageGainRX = new System.Windows.Forms.TrackBarTS();
			this.grpDSPImageRejectTX = new System.Windows.Forms.GroupBoxTS();
			this.checkboxTXImagCal = new System.Windows.Forms.CheckBoxTS();
			this.lblDSPGainValTX = new System.Windows.Forms.LabelTS();
			this.lblDSPPhaseValTX = new System.Windows.Forms.LabelTS();
			this.udDSPImageGainTX = new System.Windows.Forms.NumericUpDownTS();
			this.udDSPImagePhaseTX = new System.Windows.Forms.NumericUpDownTS();
			this.lblDSPImageGainTX = new System.Windows.Forms.LabelTS();
			this.tbDSPImagePhaseTX = new System.Windows.Forms.TrackBarTS();
			this.lblDSPImagePhaseTX = new System.Windows.Forms.LabelTS();
			this.tbDSPImageGainTX = new System.Windows.Forms.TrackBarTS();
			this.tpDSPKeyer = new System.Windows.Forms.TabPage();
			this.grpKeyerConnections = new System.Windows.Forms.GroupBoxTS();
			this.comboKeyerConnKeyLine = new System.Windows.Forms.ComboBoxTS();
			this.comboKeyerConnSecondary = new System.Windows.Forms.ComboBoxTS();
			this.lblKeyerConnSecondary = new System.Windows.Forms.LabelTS();
			this.lblKeyerConnKeyLine = new System.Windows.Forms.LabelTS();
			this.comboKeyerConnPTTLine = new System.Windows.Forms.ComboBoxTS();
			this.lblKeyerConnPrimary = new System.Windows.Forms.LabelTS();
			this.lblKeyerConnPTTLine = new System.Windows.Forms.LabelTS();
			this.comboKeyerConnPrimary = new System.Windows.Forms.ComboBoxTS();
			this.grpDSPCWPitch = new System.Windows.Forms.GroupBoxTS();
			this.lblDSPCWPitchFreq = new System.Windows.Forms.LabelTS();
			this.udDSPCWPitch = new System.Windows.Forms.NumericUpDownTS();
			this.grpDSPKeyerOptions = new System.Windows.Forms.GroupBoxTS();
			this.chkCWKeyerMode = new System.Windows.Forms.CheckBoxTS();
			this.chkHiPerfKeyer = new System.Windows.Forms.CheckBoxTS();
			this.chkCWKeyerRevPdl = new System.Windows.Forms.CheckBoxTS();
			this.chkDSPKeyerDisableMonitor = new System.Windows.Forms.CheckBoxTS();
			this.chkCWKeyerIambic = new System.Windows.Forms.CheckBoxTS();
			this.grpDSPKeyerSignalShaping = new System.Windows.Forms.GroupBoxTS();
			this.udCWKeyerDeBounce = new System.Windows.Forms.NumericUpDownTS();
			this.lblKeyerDeBounce = new System.Windows.Forms.LabelTS();
			this.udCWKeyerWeight = new System.Windows.Forms.NumericUpDownTS();
			this.lblCWWeight = new System.Windows.Forms.LabelTS();
			this.udCWKeyerRamp = new System.Windows.Forms.NumericUpDownTS();
			this.lblCWRamp = new System.Windows.Forms.LabelTS();
			this.grpDSPKeyerSemiBreakIn = new System.Windows.Forms.GroupBoxTS();
			this.chkDSPKeyerSemiBreakInEnabled = new System.Windows.Forms.CheckBoxTS();
			this.lblCWKeyerBreakIn = new System.Windows.Forms.LabelTS();
			this.udCWKeyerSemiBreakInDelay = new System.Windows.Forms.NumericUpDownTS();
			this.tpDSPAGCALC = new System.Windows.Forms.TabPage();
			this.grpDSPLeveler = new System.Windows.Forms.GroupBoxTS();
			this.chkDSPLevelerEnabled = new System.Windows.Forms.CheckBoxTS();
			this.lblDSPLevelerHangThreshold = new System.Windows.Forms.LabelTS();
			this.udDSPLevelerHangTime = new System.Windows.Forms.NumericUpDownTS();
			this.lblDSPLevelerHangTime = new System.Windows.Forms.LabelTS();
			this.udDSPLevelerThreshold = new System.Windows.Forms.NumericUpDownTS();
			this.udDSPLevelerSlope = new System.Windows.Forms.NumericUpDownTS();
			this.udDSPLevelerDecay = new System.Windows.Forms.NumericUpDownTS();
			this.lblDSPLevelerSlope = new System.Windows.Forms.LabelTS();
			this.udDSPLevelerAttack = new System.Windows.Forms.NumericUpDownTS();
			this.lblDSPLevelerDecay = new System.Windows.Forms.LabelTS();
			this.lblDSPLevelerAttack = new System.Windows.Forms.LabelTS();
			this.lblDSPLevelerThreshold = new System.Windows.Forms.LabelTS();
			this.tbDSPLevelerHangThreshold = new System.Windows.Forms.TrackBarTS();
			this.grpDSPALC = new System.Windows.Forms.GroupBoxTS();
			this.lblDSPALCHangThreshold = new System.Windows.Forms.LabelTS();
			this.tbDSPALCHangThreshold = new System.Windows.Forms.TrackBarTS();
			this.udDSPALCHangTime = new System.Windows.Forms.NumericUpDownTS();
			this.lblDSPALCHangTime = new System.Windows.Forms.LabelTS();
			this.udDSPALCThreshold = new System.Windows.Forms.NumericUpDownTS();
			this.udDSPALCSlope = new System.Windows.Forms.NumericUpDownTS();
			this.udDSPALCDecay = new System.Windows.Forms.NumericUpDownTS();
			this.lblDSPALCSlope = new System.Windows.Forms.LabelTS();
			this.udDSPALCAttack = new System.Windows.Forms.NumericUpDownTS();
			this.lblDSPALCDecay = new System.Windows.Forms.LabelTS();
			this.lblDSPALCAttack = new System.Windows.Forms.LabelTS();
			this.lblDSPALCThreshold = new System.Windows.Forms.LabelTS();
			this.grpDSPAGC = new System.Windows.Forms.GroupBoxTS();
			this.tbDSPAGCHangThreshold = new System.Windows.Forms.TrackBarTS();
			this.lblDSPAGCHangThreshold = new System.Windows.Forms.LabelTS();
			this.lblDSPAGCHangTime = new System.Windows.Forms.LabelTS();
			this.udDSPAGCHangTime = new System.Windows.Forms.NumericUpDownTS();
			this.udDSPAGCMaxGaindB = new System.Windows.Forms.NumericUpDownTS();
			this.udDSPAGCSlope = new System.Windows.Forms.NumericUpDownTS();
			this.udDSPAGCDecay = new System.Windows.Forms.NumericUpDownTS();
			this.lblDSPAGCSlope = new System.Windows.Forms.LabelTS();
			this.udDSPAGCAttack = new System.Windows.Forms.NumericUpDownTS();
			this.lblDSPAGCDecay = new System.Windows.Forms.LabelTS();
			this.lblDSPAGCAttack = new System.Windows.Forms.LabelTS();
			this.lblDSPAGCMaxGain = new System.Windows.Forms.LabelTS();
			this.udDSPAGCFixedGaindB = new System.Windows.Forms.NumericUpDownTS();
			this.lblDSPAGCFixed = new System.Windows.Forms.LabelTS();
			this.tpTransmit = new System.Windows.Forms.TabPage();
			this.grpTXAM = new System.Windows.Forms.GroupBoxTS();
			this.lblTXAMCarrierLevel = new System.Windows.Forms.LabelTS();
			this.udTXAMCarrierLevel = new System.Windows.Forms.NumericUpDownTS();
			this.grpTXMonitor = new System.Windows.Forms.GroupBoxTS();
			this.lblTXAF = new System.Windows.Forms.LabelTS();
			this.udTXAF = new System.Windows.Forms.NumericUpDownTS();
			this.grpTXVOX = new System.Windows.Forms.GroupBoxTS();
			this.lblTXVOXHangTime = new System.Windows.Forms.LabelTS();
			this.udTXVOXHangTime = new System.Windows.Forms.NumericUpDownTS();
			this.chkTXVOXEnabled = new System.Windows.Forms.CheckBoxTS();
			this.lblTXVOXThreshold = new System.Windows.Forms.LabelTS();
			this.udTXVOXThreshold = new System.Windows.Forms.NumericUpDownTS();
			this.grpTXNoiseGate = new System.Windows.Forms.GroupBoxTS();
			this.chkTXNoiseGateEnabled = new System.Windows.Forms.CheckBoxTS();
			this.udTXNoiseGate = new System.Windows.Forms.NumericUpDownTS();
			this.lblTXNoiseGateThreshold = new System.Windows.Forms.LabelTS();
			this.grpTXProfile = new System.Windows.Forms.GroupBoxTS();
			this.btnTXProfileDelete = new System.Windows.Forms.Button();
			this.btnTXProfileSave = new System.Windows.Forms.Button();
			this.comboTXProfileName = new System.Windows.Forms.ComboBoxTS();
			this.grpPATune = new System.Windows.Forms.GroupBoxTS();
			this.lblTransmitTunePower = new System.Windows.Forms.LabelTS();
			this.udTXTunePower = new System.Windows.Forms.NumericUpDownTS();
			this.grpTXCompression = new System.Windows.Forms.GroupBoxTS();
			this.lblCompand = new System.Windows.Forms.LabelTS();
			this.tbTXCompander = new System.Windows.Forms.TrackBarTS();
			this.udTXCompander = new System.Windows.Forms.NumericUpDownTS();
			this.lblTXCompander = new System.Windows.Forms.LabelTS();
			this.udTXFFCompression = new System.Windows.Forms.NumericUpDownTS();
			this.lblTXFFVal = new System.Windows.Forms.LabelTS();
			this.lblTransmitFeedForward = new System.Windows.Forms.LabelTS();
			this.tbTXFFCompression = new System.Windows.Forms.TrackBarTS();
			this.grpTXFilter = new System.Windows.Forms.GroupBoxTS();
			this.lblTXFilterHigh = new System.Windows.Forms.LabelTS();
			this.udTXFilterLow = new System.Windows.Forms.NumericUpDownTS();
			this.lblTXFilterLow = new System.Windows.Forms.LabelTS();
			this.udTXFilterHigh = new System.Windows.Forms.NumericUpDownTS();
			this.chkDCBlock = new System.Windows.Forms.CheckBoxTS();
			this.tpPowerAmplifier = new System.Windows.Forms.TabPage();
			this.grpPABandOffset = new System.Windows.Forms.GroupBoxTS();
			this.lblPABandOffset10 = new System.Windows.Forms.LabelTS();
			this.lblPABandOffset12 = new System.Windows.Forms.LabelTS();
			this.lblPABandOffset15 = new System.Windows.Forms.LabelTS();
			this.lblPABandOffset17 = new System.Windows.Forms.LabelTS();
			this.lblPABandOffset20 = new System.Windows.Forms.LabelTS();
			this.lblPABandOffset30 = new System.Windows.Forms.LabelTS();
			this.lblPABandOffset40 = new System.Windows.Forms.LabelTS();
			this.lblPABandOffset60 = new System.Windows.Forms.LabelTS();
			this.lblPABandOffset80 = new System.Windows.Forms.LabelTS();
			this.lblPABandOffset160 = new System.Windows.Forms.LabelTS();
			this.udPAADC17 = new System.Windows.Forms.NumericUpDownTS();
			this.udPAADC15 = new System.Windows.Forms.NumericUpDownTS();
			this.udPAADC20 = new System.Windows.Forms.NumericUpDownTS();
			this.udPAADC12 = new System.Windows.Forms.NumericUpDownTS();
			this.udPAADC10 = new System.Windows.Forms.NumericUpDownTS();
			this.udPAADC160 = new System.Windows.Forms.NumericUpDownTS();
			this.udPAADC80 = new System.Windows.Forms.NumericUpDownTS();
			this.udPAADC60 = new System.Windows.Forms.NumericUpDownTS();
			this.udPAADC40 = new System.Windows.Forms.NumericUpDownTS();
			this.udPAADC30 = new System.Windows.Forms.NumericUpDownTS();
			this.grpPAGainByBand = new System.Windows.Forms.GroupBoxTS();
			this.btnPAGainReset = new System.Windows.Forms.ButtonTS();
			this.btnPAGainCalibration = new System.Windows.Forms.ButtonTS();
			this.lblPAGainByBand10 = new System.Windows.Forms.LabelTS();
			this.udPAGain10 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand12 = new System.Windows.Forms.LabelTS();
			this.udPAGain12 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand15 = new System.Windows.Forms.LabelTS();
			this.udPAGain15 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand17 = new System.Windows.Forms.LabelTS();
			this.udPAGain17 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand20 = new System.Windows.Forms.LabelTS();
			this.udPAGain20 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand30 = new System.Windows.Forms.LabelTS();
			this.udPAGain30 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand40 = new System.Windows.Forms.LabelTS();
			this.udPAGain40 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand60 = new System.Windows.Forms.LabelTS();
			this.udPAGain60 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand80 = new System.Windows.Forms.LabelTS();
			this.udPAGain80 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand160 = new System.Windows.Forms.LabelTS();
			this.udPAGain160 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPACalPower = new System.Windows.Forms.LabelTS();
			this.udPACalPower = new System.Windows.Forms.NumericUpDownTS();
			this.tpExtCtrl = new System.Windows.Forms.TabPage();
			this.chkExtEnable = new System.Windows.Forms.CheckBoxTS();
			this.grpExtTX = new System.Windows.Forms.GroupBoxTS();
			this.lblExtTXX26 = new System.Windows.Forms.LabelTS();
			this.chkExtTX26 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX66 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX106 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX126 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX156 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX176 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX206 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX306 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX406 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX606 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX806 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX1606 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtTXX25 = new System.Windows.Forms.LabelTS();
			this.lblExtTXX24 = new System.Windows.Forms.LabelTS();
			this.lblExtTXX23 = new System.Windows.Forms.LabelTS();
			this.lblExtTXX22 = new System.Windows.Forms.LabelTS();
			this.lblExtTX2 = new System.Windows.Forms.LabelTS();
			this.chkExtTX23 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX22 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX21 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX25 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX24 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtTX6 = new System.Windows.Forms.LabelTS();
			this.chkExtTX63 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX62 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX61 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX65 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX64 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtTX10 = new System.Windows.Forms.LabelTS();
			this.chkExtTX103 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX102 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX101 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX105 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX104 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtTX12 = new System.Windows.Forms.LabelTS();
			this.chkExtTX123 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX122 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX121 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX125 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX124 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtTX15 = new System.Windows.Forms.LabelTS();
			this.chkExtTX153 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX152 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX151 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX155 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX154 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtTX17 = new System.Windows.Forms.LabelTS();
			this.chkExtTX173 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX172 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX171 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX175 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX174 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtTX20 = new System.Windows.Forms.LabelTS();
			this.chkExtTX203 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX202 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX201 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX205 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX204 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtTX30 = new System.Windows.Forms.LabelTS();
			this.chkExtTX303 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX302 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX301 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX305 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX304 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtTX40 = new System.Windows.Forms.LabelTS();
			this.chkExtTX403 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX402 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX401 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX405 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX404 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtTX60 = new System.Windows.Forms.LabelTS();
			this.chkExtTX603 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX602 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX601 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX605 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX604 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtTX80 = new System.Windows.Forms.LabelTS();
			this.chkExtTX803 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX802 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX801 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX805 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX804 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtTXX2Pins = new System.Windows.Forms.LabelTS();
			this.lblExtTXBand = new System.Windows.Forms.LabelTS();
			this.lblExtTX160 = new System.Windows.Forms.LabelTS();
			this.chkExtTX1603 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX1602 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX1601 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtTXX21 = new System.Windows.Forms.LabelTS();
			this.chkExtTX1605 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtTX1604 = new System.Windows.Forms.CheckBoxTS();
			this.grpExtRX = new System.Windows.Forms.GroupBoxTS();
			this.lblExtRXX26 = new System.Windows.Forms.LabelTS();
			this.chkExtRX26 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX66 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX106 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX126 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX156 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX176 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX206 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX306 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX406 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX606 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX806 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX1606 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtRXX25 = new System.Windows.Forms.LabelTS();
			this.lblExtRXX24 = new System.Windows.Forms.LabelTS();
			this.lblExtRXX23 = new System.Windows.Forms.LabelTS();
			this.lblExtRXX22 = new System.Windows.Forms.LabelTS();
			this.lblExtRX2 = new System.Windows.Forms.LabelTS();
			this.chkExtRX23 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX22 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX21 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX25 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX24 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtRX6 = new System.Windows.Forms.LabelTS();
			this.chkExtRX63 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX62 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX61 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX65 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX64 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtRX10 = new System.Windows.Forms.LabelTS();
			this.chkExtRX103 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX102 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX101 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX105 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX104 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtRX12 = new System.Windows.Forms.LabelTS();
			this.chkExtRX123 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX122 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX121 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX125 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX124 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtRX15 = new System.Windows.Forms.LabelTS();
			this.chkExtRX153 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX152 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX151 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX155 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX154 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtRX17 = new System.Windows.Forms.LabelTS();
			this.chkExtRX173 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX172 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX171 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX175 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX174 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtRX20 = new System.Windows.Forms.LabelTS();
			this.chkExtRX203 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX202 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX201 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX205 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX204 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtRX30 = new System.Windows.Forms.LabelTS();
			this.chkExtRX303 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX302 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX301 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX305 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX304 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtRX40 = new System.Windows.Forms.LabelTS();
			this.chkExtRX403 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX402 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX401 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX405 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX404 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtRX60 = new System.Windows.Forms.LabelTS();
			this.chkExtRX603 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX602 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX601 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX605 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX604 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtRX80 = new System.Windows.Forms.LabelTS();
			this.chkExtRX803 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX802 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX801 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX805 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX804 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtRXX2Pins = new System.Windows.Forms.LabelTS();
			this.lblExtRXBand = new System.Windows.Forms.LabelTS();
			this.lblExtRX160 = new System.Windows.Forms.LabelTS();
			this.chkExtRX1603 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX1602 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX1601 = new System.Windows.Forms.CheckBoxTS();
			this.lblExtRXX21 = new System.Windows.Forms.LabelTS();
			this.chkExtRX1605 = new System.Windows.Forms.CheckBoxTS();
			this.chkExtRX1604 = new System.Windows.Forms.CheckBoxTS();
			this.tpAppearance = new System.Windows.Forms.TabPage();
			this.tcAppearance = new System.Windows.Forms.TabControl();
			this.tpAppearanceDisplay = new System.Windows.Forms.TabPage();
			this.grpAppPanadapter = new System.Windows.Forms.GroupBoxTS();
			this.clrbtnSubRXZero = new PowerSDR.ColorButton();
			this.lblSubRXZeroLine = new System.Windows.Forms.LabelTS();
			this.clrbtnSubRXFilter = new PowerSDR.ColorButton();
			this.lblSubRXFilterColor = new System.Windows.Forms.LabelTS();
			this.chkShowFreqOffset = new System.Windows.Forms.CheckBoxTS();
			this.clrbtnBandEdge = new PowerSDR.ColorButton();
			this.lblBandEdge = new System.Windows.Forms.LabelTS();
			this.clrbtnFilter = new PowerSDR.ColorButton();
			this.clrbtnTXFilter = new PowerSDR.ColorButton();
			this.lblTXFilterColor = new System.Windows.Forms.LabelTS();
			this.lblDisplayFilterColor = new System.Windows.Forms.LabelTS();
			this.grpDisplayPeakCursor = new System.Windows.Forms.GroupBoxTS();
			this.clrbtnPeakBackground = new PowerSDR.ColorButton();
			this.lblPeakBackground = new System.Windows.Forms.LabelTS();
			this.clrbtnPeakText = new PowerSDR.ColorButton();
			this.lblPeakText = new System.Windows.Forms.LabelTS();
			this.lblDisplayDataLineColor = new System.Windows.Forms.LabelTS();
			this.lblDisplayTextColor = new System.Windows.Forms.LabelTS();
			this.lblDisplayZeroLineColor = new System.Windows.Forms.LabelTS();
			this.lblDisplayGridColor = new System.Windows.Forms.LabelTS();
			this.lblDisplayBackgroundColor = new System.Windows.Forms.LabelTS();
			this.clrbtnDataLine = new PowerSDR.ColorButton();
			this.clrbtnText = new PowerSDR.ColorButton();
			this.clrbtnZeroLine = new PowerSDR.ColorButton();
			this.clrbtnGrid = new PowerSDR.ColorButton();
			this.clrbtnBackground = new PowerSDR.ColorButton();
			this.lblDisplayLineWidth = new System.Windows.Forms.LabelTS();
			this.udDisplayLineWidth = new System.Windows.Forms.NumericUpDownTS();
			this.tpAppearanceGeneral = new System.Windows.Forms.TabPage();
			this.grpAppearanceBand = new System.Windows.Forms.GroupBoxTS();
			this.clrbtnBandBackground = new PowerSDR.ColorButton();
			this.lblBandBackground = new System.Windows.Forms.LabelTS();
			this.clrbtnBandLight = new PowerSDR.ColorButton();
			this.clrbtnBandDark = new PowerSDR.ColorButton();
			this.lblBandLight = new System.Windows.Forms.LabelTS();
			this.lblBandDark = new System.Windows.Forms.LabelTS();
			this.clrbtnOutOfBand = new PowerSDR.ColorButton();
			this.lblOutOfBand = new System.Windows.Forms.LabelTS();
			this.grpAppearanceVFO = new System.Windows.Forms.GroupBoxTS();
			this.clrbtnVFOBackground = new PowerSDR.ColorButton();
			this.lblVFOBackground = new System.Windows.Forms.LabelTS();
			this.clrbtnVFOSmallColor = new PowerSDR.ColorButton();
			this.lblVFOSmallColor = new System.Windows.Forms.LabelTS();
			this.chkVFOSmallLSD = new System.Windows.Forms.CheckBoxTS();
			this.clrbtnVFOLight = new PowerSDR.ColorButton();
			this.clrbtnVFODark = new PowerSDR.ColorButton();
			this.lblVFOPowerOn = new System.Windows.Forms.LabelTS();
			this.lblVFOPowerOff = new System.Windows.Forms.LabelTS();
			this.clrbtnBtnSel = new PowerSDR.ColorButton();
			this.lblAppearanceGenBtnSel = new System.Windows.Forms.LabelTS();
			this.tpAppearanceMeter = new System.Windows.Forms.TabPage();
			this.labelTS2 = new System.Windows.Forms.LabelTS();
			this.clrbtnMeterDigBackground = new PowerSDR.ColorButton();
			this.lblMeterDigitalText = new System.Windows.Forms.LabelTS();
			this.clrbtnMeterDigText = new PowerSDR.ColorButton();
			this.grpMeterEdge = new System.Windows.Forms.GroupBoxTS();
			this.clrbtnEdgeIndicator = new PowerSDR.ColorButton();
			this.labelTS1 = new System.Windows.Forms.LabelTS();
			this.clrbtnMeterEdgeBackground = new PowerSDR.ColorButton();
			this.lblMeterEdgeBackground = new System.Windows.Forms.LabelTS();
			this.clrbtnMeterEdgeHigh = new PowerSDR.ColorButton();
			this.lblMeterEdgeHigh = new System.Windows.Forms.LabelTS();
			this.lblMeterEdgeLow = new System.Windows.Forms.LabelTS();
			this.clrbtnMeterEdgeLow = new PowerSDR.ColorButton();
			this.grpAppearanceMeter = new System.Windows.Forms.GroupBoxTS();
			this.clrbtnMeterBackground = new PowerSDR.ColorButton();
			this.lblMeterBackground = new System.Windows.Forms.LabelTS();
			this.clrbtnMeterRight = new PowerSDR.ColorButton();
			this.lblAppearanceMeterRight = new System.Windows.Forms.LabelTS();
			this.lblAppearanceMeterLeft = new System.Windows.Forms.LabelTS();
			this.clrbtnMeterLeft = new PowerSDR.ColorButton();
			this.lblMeterType = new System.Windows.Forms.LabelTS();
			this.comboMeterType = new System.Windows.Forms.ComboBoxTS();
			this.tpKeyboard = new System.Windows.Forms.TabPage();
			this.grpKBXIT = new System.Windows.Forms.GroupBoxTS();
			this.lblKBXITUp = new System.Windows.Forms.LabelTS();
			this.lblKBXITDown = new System.Windows.Forms.LabelTS();
			this.comboKBXITUp = new System.Windows.Forms.ComboBoxTS();
			this.comboKBXITDown = new System.Windows.Forms.ComboBoxTS();
			this.grpKBRIT = new System.Windows.Forms.GroupBoxTS();
			this.lblKBRitUp = new System.Windows.Forms.LabelTS();
			this.lblKBRITDown = new System.Windows.Forms.LabelTS();
			this.comboKBRITUp = new System.Windows.Forms.ComboBoxTS();
			this.comboKBRITDown = new System.Windows.Forms.ComboBoxTS();
			this.grpKBMode = new System.Windows.Forms.GroupBoxTS();
			this.lblKBModeUp = new System.Windows.Forms.LabelTS();
			this.lblKBModeDown = new System.Windows.Forms.LabelTS();
			this.comboKBModeUp = new System.Windows.Forms.ComboBoxTS();
			this.comboKBModeDown = new System.Windows.Forms.ComboBoxTS();
			this.grpKBBand = new System.Windows.Forms.GroupBoxTS();
			this.lblKBBandUp = new System.Windows.Forms.LabelTS();
			this.lblKBBandDown = new System.Windows.Forms.LabelTS();
			this.comboKBBandUp = new System.Windows.Forms.ComboBoxTS();
			this.comboKBBandDown = new System.Windows.Forms.ComboBoxTS();
			this.grpKBTune = new System.Windows.Forms.GroupBoxTS();
			this.lblKBTuneDigit = new System.Windows.Forms.LabelTS();
			this.lblKBTune7 = new System.Windows.Forms.LabelTS();
			this.lblKBTune6 = new System.Windows.Forms.LabelTS();
			this.lblKBTune5 = new System.Windows.Forms.LabelTS();
			this.lblKBTune4 = new System.Windows.Forms.LabelTS();
			this.lblKBTune3 = new System.Windows.Forms.LabelTS();
			this.lblKBTune2 = new System.Windows.Forms.LabelTS();
			this.comboKBTuneUp7 = new System.Windows.Forms.ComboBoxTS();
			this.comboKBTuneDown7 = new System.Windows.Forms.ComboBoxTS();
			this.comboKBTuneUp6 = new System.Windows.Forms.ComboBoxTS();
			this.comboKBTuneDown6 = new System.Windows.Forms.ComboBoxTS();
			this.comboKBTuneUp5 = new System.Windows.Forms.ComboBoxTS();
			this.comboKBTuneDown5 = new System.Windows.Forms.ComboBoxTS();
			this.comboKBTuneUp4 = new System.Windows.Forms.ComboBoxTS();
			this.comboKBTuneDown4 = new System.Windows.Forms.ComboBoxTS();
			this.lblKBTune1 = new System.Windows.Forms.LabelTS();
			this.lblKBTuneUp = new System.Windows.Forms.LabelTS();
			this.lblKBTuneDown = new System.Windows.Forms.LabelTS();
			this.comboKBTuneUp3 = new System.Windows.Forms.ComboBoxTS();
			this.comboKBTuneDown3 = new System.Windows.Forms.ComboBoxTS();
			this.comboKBTuneUp1 = new System.Windows.Forms.ComboBoxTS();
			this.comboKBTuneUp2 = new System.Windows.Forms.ComboBoxTS();
			this.comboKBTuneDown1 = new System.Windows.Forms.ComboBoxTS();
			this.comboKBTuneDown2 = new System.Windows.Forms.ComboBoxTS();
			this.grpKBFilter = new System.Windows.Forms.GroupBoxTS();
			this.lblKBFilterUp = new System.Windows.Forms.LabelTS();
			this.lblKBFilterDown = new System.Windows.Forms.LabelTS();
			this.comboKBFilterUp = new System.Windows.Forms.ComboBoxTS();
			this.comboKBFilterDown = new System.Windows.Forms.ComboBoxTS();
			this.grpKBCW = new System.Windows.Forms.GroupBoxTS();
			this.lblKBCWDot = new System.Windows.Forms.LabelTS();
			this.lblKBCWDash = new System.Windows.Forms.LabelTS();
			this.comboKBCWDot = new System.Windows.Forms.ComboBoxTS();
			this.comboKBCWDash = new System.Windows.Forms.ComboBoxTS();
			this.tpCAT = new System.Windows.Forms.TabPage();
			this.lblCATRigType = new System.Windows.Forms.LabelTS();
			this.comboCATRigType = new System.Windows.Forms.ComboBoxTS();
			this.btnCATTest = new System.Windows.Forms.ButtonTS();
			this.grpPTTBitBang = new System.Windows.Forms.GroupBoxTS();
			this.comboCATPTTPort = new System.Windows.Forms.ComboBoxTS();
			this.lblCATPTTPort = new System.Windows.Forms.LabelTS();
			this.chkCATPTT_RTS = new System.Windows.Forms.CheckBoxTS();
			this.chkCATPTT_DTR = new System.Windows.Forms.CheckBoxTS();
			this.chkCATPTTEnabled = new System.Windows.Forms.CheckBoxTS();
			this.grpCatControlBox = new System.Windows.Forms.GroupBoxTS();
			this.comboCATPort = new System.Windows.Forms.ComboBoxTS();
			this.comboCATbaud = new System.Windows.Forms.ComboBoxTS();
			this.lblCATBaud = new System.Windows.Forms.LabelTS();
			this.lblCATPort = new System.Windows.Forms.LabelTS();
			this.chkCATEnable = new System.Windows.Forms.CheckBoxTS();
			this.lblCATParity = new System.Windows.Forms.LabelTS();
			this.lblCATData = new System.Windows.Forms.LabelTS();
			this.lblCATStop = new System.Windows.Forms.LabelTS();
			this.comboCATparity = new System.Windows.Forms.ComboBoxTS();
			this.comboCATdatabits = new System.Windows.Forms.ComboBoxTS();
			this.comboCATstopbits = new System.Windows.Forms.ComboBoxTS();
			this.tpTests = new System.Windows.Forms.TabPage();
			this.grpBoxTS1 = new System.Windows.Forms.GroupBoxTS();
			this.lblTestGenScale = new System.Windows.Forms.LabelTS();
			this.udTestGenScale = new System.Windows.Forms.NumericUpDownTS();
			this.radTestGenOutput = new System.Windows.Forms.RadioButtonTS();
			this.radTestGenInput = new System.Windows.Forms.RadioButtonTS();
			this.cmboTestGenMode = new System.Windows.Forms.ComboBoxTS();
			this.lblTestSigGenFreqCallout = new System.Windows.Forms.LabelTS();
			this.tkbarTestGenFreq = new System.Windows.Forms.TrackBarTS();
			this.lblTestGenHzSec = new System.Windows.Forms.LabelTS();
			this.udTestGenHzSec = new System.Windows.Forms.NumericUpDownTS();
			this.lblTestGenHigh = new System.Windows.Forms.LabelTS();
			this.udTestGenHigh = new System.Windows.Forms.NumericUpDownTS();
			this.lblTestGenLow = new System.Windows.Forms.LabelTS();
			this.udTestGenLow = new System.Windows.Forms.NumericUpDownTS();
			this.btnTestGenSweep = new System.Windows.Forms.ButtonTS();
			this.grpTestX2 = new System.Windows.Forms.GroupBoxTS();
			this.lblTestX2 = new System.Windows.Forms.LabelTS();
			this.chkTestX2Pin6 = new System.Windows.Forms.CheckBoxTS();
			this.chkTestX2Pin5 = new System.Windows.Forms.CheckBoxTS();
			this.chkTestX2Pin4 = new System.Windows.Forms.CheckBoxTS();
			this.chkTestX2Pin3 = new System.Windows.Forms.CheckBoxTS();
			this.chkTestX2Pin2 = new System.Windows.Forms.CheckBoxTS();
			this.chkTestX2Pin1 = new System.Windows.Forms.CheckBoxTS();
			this.grpTestAudioBalance = new System.Windows.Forms.GroupBoxTS();
			this.btnTestAudioBalStart = new System.Windows.Forms.ButtonTS();
			this.grpTestTXIMD = new System.Windows.Forms.GroupBoxTS();
			this.lblTestToneFreq2 = new System.Windows.Forms.LabelTS();
			this.udTestIMDFreq2 = new System.Windows.Forms.NumericUpDownTS();
			this.lblTestIMDPower = new System.Windows.Forms.LabelTS();
			this.udTestIMDPower = new System.Windows.Forms.NumericUpDownTS();
			this.chekTestIMD = new System.Windows.Forms.CheckBoxTS();
			this.lblTestToneFreq1 = new System.Windows.Forms.LabelTS();
			this.udTestIMDFreq1 = new System.Windows.Forms.NumericUpDownTS();
			this.grpImpulseTest = new System.Windows.Forms.GroupBoxTS();
			this.udImpulseNum = new System.Windows.Forms.NumericUpDownTS();
			this.btnImpulse = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.ButtonTS();
			this.btnCancel = new System.Windows.Forms.ButtonTS();
			this.btnApply = new System.Windows.Forms.ButtonTS();
			this.btnResetAll = new System.Windows.Forms.ButtonTS();
			this.btnImportDB = new System.Windows.Forms.ButtonTS();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.timer_sweep = new System.Windows.Forms.Timer(this.components);
			this.tcSetup.SuspendLayout();
			this.tpGeneral.SuspendLayout();
			this.tcGeneral.SuspendLayout();
			this.tpGeneralHardware.SuspendLayout();
			this.grpHWSoftRock.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udSoftRockCenterFreq)).BeginInit();
			this.grpGeneralDDS.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDDSCorrection)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDDSIFFreq)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDDSPLLMult)).BeginInit();
			this.grpGeneralModel.SuspendLayout();
			this.grpGeneralHardwareSetup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udGeneralLPTDelay)).BeginInit();
			this.tpGeneralOptions.SuspendLayout();
			this.grpOptMainConsole.SuspendLayout();
			this.grpOptQuickQSY.SuspendLayout();
			this.grpGenAutoMute.SuspendLayout();
			this.grpGenTuningOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udOptClickTuneOffsetDIGL)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udOptClickTuneOffsetDIGU)).BeginInit();
			this.grpGeneralOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udGeneralX2Delay)).BeginInit();
			this.grpGeneralProcessPriority.SuspendLayout();
			this.grpGeneralUpdates.SuspendLayout();
			this.tpGeneralCalibration.SuspendLayout();
			this.grpGenCalRXImage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udGeneralCalFreq3)).BeginInit();
			this.grpGenCalLevel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udGeneralCalLevel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udGeneralCalFreq2)).BeginInit();
			this.grpGeneralCalibration.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udGeneralCalFreq1)).BeginInit();
			this.tpFilters.SuspendLayout();
			this.grpOptFilterControls.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udFilterDefaultLowCut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udOptMaxFilterShift)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udOptMaxFilterWidth)).BeginInit();
			this.tpAudio.SuspendLayout();
			this.tcAudio.SuspendLayout();
			this.tpAudioCard1.SuspendLayout();
			this.grpAudioMicBoost.SuspendLayout();
			this.grpAudioChannels.SuspendLayout();
			this.grpAudioMicInGain1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udAudioMicGain1)).BeginInit();
			this.grpAudioLineInGain1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udAudioLineIn1)).BeginInit();
			this.grpAudioVolts1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udAudioVoltage1)).BeginInit();
			this.grpAudioDetails1.SuspendLayout();
			this.grpAudioLatency1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udAudioLatency1)).BeginInit();
			this.grpAudioCard.SuspendLayout();
			this.grpAudioBufferSize1.SuspendLayout();
			this.grpAudioSampleRate1.SuspendLayout();
			this.tpVAC.SuspendLayout();
			this.grpAudioVACAutoEnable.SuspendLayout();
			this.grpAudioVACGain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udAudioVACGainTX)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udAudioVACGainRX)).BeginInit();
			this.grpAudio2Stereo.SuspendLayout();
			this.grpAudioLatency2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udAudioLatency2)).BeginInit();
			this.grpAudioSampleRate2.SuspendLayout();
			this.grpAudioBuffer2.SuspendLayout();
			this.grpAudioDetails2.SuspendLayout();
			this.tpDisplay.SuspendLayout();
			this.grpDisplayDriverEngine.SuspendLayout();
			this.grpDisplayPolyPhase.SuspendLayout();
			this.grpDisplayScopeMode.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayScopeTime)).BeginInit();
			this.grpDisplayMultimeter.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayMeterAvg)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayMultiTextHoldTime)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayMultiPeakHoldTime)).BeginInit();
			this.grpDisplayWaterfall.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayWaterfallLowLevel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayWaterfallHighLevel)).BeginInit();
			this.grpDisplayRefreshRates.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayCPUMeter)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayPeakText)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayMeterDelay)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayFPS)).BeginInit();
			this.grpDisplayAverage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayAVGTime)).BeginInit();
			this.grpDisplayPhase.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayPhasePts)).BeginInit();
			this.grpDisplaySpectrumGrid.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayGridStep)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayGridMin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayGridMax)).BeginInit();
			this.tpDSP.SuspendLayout();
			this.tcDSP.SuspendLayout();
			this.tpDSPOptions.SuspendLayout();
			this.grpDSPBufferSize.SuspendLayout();
			this.grpDSPNB.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDSPNB)).BeginInit();
			this.grpDSPLMSNR.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udLMSNRgain)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udLMSNRdelay)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udLMSNRtaps)).BeginInit();
			this.grpDSPLMSANF.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udLMSANFgain)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udLMSANFdelay)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udLMSANFtaps)).BeginInit();
			this.grpDSPWindow.SuspendLayout();
			this.grpDSPNB2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDSPNB2)).BeginInit();
			this.tpDSPImageReject.SuspendLayout();
			this.grpDSPImageRejectRX.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDSPImageGainRX)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPImagePhaseRX)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbDSPImagePhaseRX)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbDSPImageGainRX)).BeginInit();
			this.grpDSPImageRejectTX.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDSPImageGainTX)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPImagePhaseTX)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbDSPImagePhaseTX)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbDSPImageGainTX)).BeginInit();
			this.tpDSPKeyer.SuspendLayout();
			this.grpKeyerConnections.SuspendLayout();
			this.grpDSPCWPitch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDSPCWPitch)).BeginInit();
			this.grpDSPKeyerOptions.SuspendLayout();
			this.grpDSPKeyerSignalShaping.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udCWKeyerDeBounce)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udCWKeyerWeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udCWKeyerRamp)).BeginInit();
			this.grpDSPKeyerSemiBreakIn.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udCWKeyerSemiBreakInDelay)).BeginInit();
			this.tpDSPAGCALC.SuspendLayout();
			this.grpDSPLeveler.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDSPLevelerHangTime)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPLevelerThreshold)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPLevelerSlope)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPLevelerDecay)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPLevelerAttack)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbDSPLevelerHangThreshold)).BeginInit();
			this.grpDSPALC.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbDSPALCHangThreshold)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPALCHangTime)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPALCThreshold)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPALCSlope)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPALCDecay)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPALCAttack)).BeginInit();
			this.grpDSPAGC.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbDSPAGCHangThreshold)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPAGCHangTime)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPAGCMaxGaindB)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPAGCSlope)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPAGCDecay)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPAGCAttack)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPAGCFixedGaindB)).BeginInit();
			this.tpTransmit.SuspendLayout();
			this.grpTXAM.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udTXAMCarrierLevel)).BeginInit();
			this.grpTXMonitor.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udTXAF)).BeginInit();
			this.grpTXVOX.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udTXVOXHangTime)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udTXVOXThreshold)).BeginInit();
			this.grpTXNoiseGate.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udTXNoiseGate)).BeginInit();
			this.grpTXProfile.SuspendLayout();
			this.grpPATune.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udTXTunePower)).BeginInit();
			this.grpTXCompression.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbTXCompander)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udTXCompander)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udTXFFCompression)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXFFCompression)).BeginInit();
			this.grpTXFilter.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udTXFilterLow)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udTXFilterHigh)).BeginInit();
			this.tpPowerAmplifier.SuspendLayout();
			this.grpPABandOffset.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC17)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC15)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC20)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC12)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC10)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC160)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC80)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC60)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC40)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC30)).BeginInit();
			this.grpPAGainByBand.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain10)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain12)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain15)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain17)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain20)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain30)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain40)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain60)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain80)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain160)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPACalPower)).BeginInit();
			this.tpExtCtrl.SuspendLayout();
			this.grpExtTX.SuspendLayout();
			this.grpExtRX.SuspendLayout();
			this.tpAppearance.SuspendLayout();
			this.tcAppearance.SuspendLayout();
			this.tpAppearanceDisplay.SuspendLayout();
			this.grpAppPanadapter.SuspendLayout();
			this.grpDisplayPeakCursor.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayLineWidth)).BeginInit();
			this.tpAppearanceGeneral.SuspendLayout();
			this.grpAppearanceBand.SuspendLayout();
			this.grpAppearanceVFO.SuspendLayout();
			this.tpAppearanceMeter.SuspendLayout();
			this.grpMeterEdge.SuspendLayout();
			this.grpAppearanceMeter.SuspendLayout();
			this.tpKeyboard.SuspendLayout();
			this.grpKBXIT.SuspendLayout();
			this.grpKBRIT.SuspendLayout();
			this.grpKBMode.SuspendLayout();
			this.grpKBBand.SuspendLayout();
			this.grpKBTune.SuspendLayout();
			this.grpKBFilter.SuspendLayout();
			this.grpKBCW.SuspendLayout();
			this.tpCAT.SuspendLayout();
			this.grpPTTBitBang.SuspendLayout();
			this.grpCatControlBox.SuspendLayout();
			this.tpTests.SuspendLayout();
			this.grpBoxTS1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udTestGenScale)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tkbarTestGenFreq)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udTestGenHzSec)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udTestGenHigh)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udTestGenLow)).BeginInit();
			this.grpTestX2.SuspendLayout();
			this.grpTestAudioBalance.SuspendLayout();
			this.grpTestTXIMD.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udTestIMDFreq2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udTestIMDPower)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udTestIMDFreq1)).BeginInit();
			this.grpImpulseTest.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udImpulseNum)).BeginInit();
			this.SuspendLayout();
			// 
			// tcSetup
			// 
			this.tcSetup.Controls.Add(this.tpGeneral);
			this.tcSetup.Controls.Add(this.tpAudio);
			this.tcSetup.Controls.Add(this.tpDisplay);
			this.tcSetup.Controls.Add(this.tpDSP);
			this.tcSetup.Controls.Add(this.tpTransmit);
			this.tcSetup.Controls.Add(this.tpPowerAmplifier);
			this.tcSetup.Controls.Add(this.tpExtCtrl);
			this.tcSetup.Controls.Add(this.tpAppearance);
			this.tcSetup.Controls.Add(this.tpKeyboard);
			this.tcSetup.Controls.Add(this.tpCAT);
			this.tcSetup.Controls.Add(this.tpTests);
			this.tcSetup.Location = new System.Drawing.Point(8, 8);
			this.tcSetup.Name = "tcSetup";
			this.tcSetup.SelectedIndex = 0;
			this.tcSetup.Size = new System.Drawing.Size(592, 312);
			this.tcSetup.TabIndex = 16;
			// 
			// tpGeneral
			// 
			this.tpGeneral.Controls.Add(this.tcGeneral);
			this.tpGeneral.Location = new System.Drawing.Point(4, 22);
			this.tpGeneral.Name = "tpGeneral";
			this.tpGeneral.Size = new System.Drawing.Size(584, 286);
			this.tpGeneral.TabIndex = 3;
			this.tpGeneral.Text = "General";
			// 
			// tcGeneral
			// 
			this.tcGeneral.Controls.Add(this.tpGeneralHardware);
			this.tcGeneral.Controls.Add(this.tpGeneralOptions);
			this.tcGeneral.Controls.Add(this.tpGeneralCalibration);
			this.tcGeneral.Controls.Add(this.tpFilters);
			this.tcGeneral.Location = new System.Drawing.Point(0, 0);
			this.tcGeneral.Name = "tcGeneral";
			this.tcGeneral.SelectedIndex = 0;
			this.tcGeneral.Size = new System.Drawing.Size(600, 344);
			this.tcGeneral.TabIndex = 26;
			// 
			// tpGeneralHardware
			// 
			this.tpGeneralHardware.Controls.Add(this.grpHWSoftRock);
			this.tpGeneralHardware.Controls.Add(this.grpGeneralDDS);
			this.tpGeneralHardware.Controls.Add(this.grpGeneralModel);
			this.tpGeneralHardware.Controls.Add(this.grpGeneralHardwareSetup);
			this.tpGeneralHardware.Controls.Add(this.btnWizard);
			this.tpGeneralHardware.Location = new System.Drawing.Point(4, 22);
			this.tpGeneralHardware.Name = "tpGeneralHardware";
			this.tpGeneralHardware.Size = new System.Drawing.Size(592, 318);
			this.tpGeneralHardware.TabIndex = 0;
			this.tpGeneralHardware.Text = "Hardware Config";
			// 
			// grpHWSoftRock
			// 
			this.grpHWSoftRock.Controls.Add(this.lblGenSoftRockCenterFreq);
			this.grpHWSoftRock.Controls.Add(this.udSoftRockCenterFreq);
			this.grpHWSoftRock.Location = new System.Drawing.Point(8, 112);
			this.grpHWSoftRock.Name = "grpHWSoftRock";
			this.grpHWSoftRock.Size = new System.Drawing.Size(144, 101);
			this.grpHWSoftRock.TabIndex = 26;
			this.grpHWSoftRock.TabStop = false;
			this.grpHWSoftRock.Text = "SoftRock Options";
			this.grpHWSoftRock.Visible = false;
			// 
			// lblGenSoftRockCenterFreq
			// 
			this.lblGenSoftRockCenterFreq.Location = new System.Drawing.Point(16, 24);
			this.lblGenSoftRockCenterFreq.Name = "lblGenSoftRockCenterFreq";
			this.lblGenSoftRockCenterFreq.Size = new System.Drawing.Size(104, 16);
			this.lblGenSoftRockCenterFreq.TabIndex = 1;
			this.lblGenSoftRockCenterFreq.Text = "Center Freq (MHz):";
			// 
			// udSoftRockCenterFreq
			// 
			this.udSoftRockCenterFreq.DecimalPlaces = 6;
			this.udSoftRockCenterFreq.Increment = new System.Decimal(new int[] {
																				   1,
																				   0,
																				   0,
																				   196608});
			this.udSoftRockCenterFreq.Location = new System.Drawing.Point(16, 40);
			this.udSoftRockCenterFreq.Maximum = new System.Decimal(new int[] {
																				 65,
																				 0,
																				 0,
																				 0});
			this.udSoftRockCenterFreq.Minimum = new System.Decimal(new int[] {
																				 0,
																				 0,
																				 0,
																				 0});
			this.udSoftRockCenterFreq.Name = "udSoftRockCenterFreq";
			this.udSoftRockCenterFreq.Size = new System.Drawing.Size(80, 20);
			this.udSoftRockCenterFreq.TabIndex = 0;
			this.toolTip1.SetToolTip(this.udSoftRockCenterFreq, "Sets the center frequency for the SoftRock 40.");
			this.udSoftRockCenterFreq.Value = new System.Decimal(new int[] {
																			   7056,
																			   0,
																			   0,
																			   196608});
			this.udSoftRockCenterFreq.LostFocus += new System.EventHandler(this.udSoftRockCenterFreq_LostFocus);
			this.udSoftRockCenterFreq.ValueChanged += new System.EventHandler(this.udSoftRockCenterFreq_ValueChanged);
			// 
			// grpGeneralDDS
			// 
			this.grpGeneralDDS.Controls.Add(this.udDDSCorrection);
			this.grpGeneralDDS.Controls.Add(this.lblClockCorrection);
			this.grpGeneralDDS.Controls.Add(this.udDDSIFFreq);
			this.grpGeneralDDS.Controls.Add(this.lblIFFrequency);
			this.grpGeneralDDS.Controls.Add(this.udDDSPLLMult);
			this.grpGeneralDDS.Controls.Add(this.lblPLLMult);
			this.grpGeneralDDS.Location = new System.Drawing.Point(384, 8);
			this.grpGeneralDDS.Name = "grpGeneralDDS";
			this.grpGeneralDDS.Size = new System.Drawing.Size(176, 112);
			this.grpGeneralDDS.TabIndex = 4;
			this.grpGeneralDDS.TabStop = false;
			this.grpGeneralDDS.Text = "DDS";
			// 
			// udDDSCorrection
			// 
			this.udDDSCorrection.Increment = new System.Decimal(new int[] {
																			  10,
																			  0,
																			  0,
																			  0});
			this.udDDSCorrection.Location = new System.Drawing.Point(104, 24);
			this.udDDSCorrection.Maximum = new System.Decimal(new int[] {
																			1000000,
																			0,
																			0,
																			0});
			this.udDDSCorrection.Minimum = new System.Decimal(new int[] {
																			1000000,
																			0,
																			0,
																			-2147483648});
			this.udDDSCorrection.Name = "udDDSCorrection";
			this.udDDSCorrection.Size = new System.Drawing.Size(64, 20);
			this.udDDSCorrection.TabIndex = 7;
			this.toolTip1.SetToolTip(this.udDDSCorrection, "Correction for DDS frequency");
			this.udDDSCorrection.Value = new System.Decimal(new int[] {
																		  0,
																		  0,
																		  0,
																		  0});
			this.udDDSCorrection.LostFocus += new System.EventHandler(this.udDDSCorrection_LostFocus);
			this.udDDSCorrection.ValueChanged += new System.EventHandler(this.udDDSCorrection_ValueChanged);
			// 
			// lblClockCorrection
			// 
			this.lblClockCorrection.Image = null;
			this.lblClockCorrection.Location = new System.Drawing.Point(16, 24);
			this.lblClockCorrection.Name = "lblClockCorrection";
			this.lblClockCorrection.Size = new System.Drawing.Size(88, 23);
			this.lblClockCorrection.TabIndex = 6;
			this.lblClockCorrection.Text = "Clock Offset:";
			// 
			// udDDSIFFreq
			// 
			this.udDDSIFFreq.Enabled = false;
			this.udDDSIFFreq.Increment = new System.Decimal(new int[] {
																		  1,
																		  0,
																		  0,
																		  0});
			this.udDDSIFFreq.Location = new System.Drawing.Point(112, 72);
			this.udDDSIFFreq.Maximum = new System.Decimal(new int[] {
																		20000,
																		0,
																		0,
																		0});
			this.udDDSIFFreq.Minimum = new System.Decimal(new int[] {
																		0,
																		0,
																		0,
																		0});
			this.udDDSIFFreq.Name = "udDDSIFFreq";
			this.udDDSIFFreq.Size = new System.Drawing.Size(56, 20);
			this.udDDSIFFreq.TabIndex = 5;
			this.toolTip1.SetToolTip(this.udDDSIFFreq, "Intermediate Frequency");
			this.udDDSIFFreq.Value = new System.Decimal(new int[] {
																	  11025,
																	  0,
																	  0,
																	  0});
			this.udDDSIFFreq.LostFocus += new System.EventHandler(this.udDDSIFFreq_LostFocus);
			this.udDDSIFFreq.ValueChanged += new System.EventHandler(this.udDDSIFFreq_ValueChanged);
			// 
			// lblIFFrequency
			// 
			this.lblIFFrequency.Image = null;
			this.lblIFFrequency.Location = new System.Drawing.Point(16, 72);
			this.lblIFFrequency.Name = "lblIFFrequency";
			this.lblIFFrequency.Size = new System.Drawing.Size(98, 23);
			this.lblIFFrequency.TabIndex = 4;
			this.lblIFFrequency.Text = "IF (Hz):";
			// 
			// udDDSPLLMult
			// 
			this.udDDSPLLMult.Increment = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   0});
			this.udDDSPLLMult.Location = new System.Drawing.Point(120, 48);
			this.udDDSPLLMult.Maximum = new System.Decimal(new int[] {
																		 20,
																		 0,
																		 0,
																		 0});
			this.udDDSPLLMult.Minimum = new System.Decimal(new int[] {
																		 0,
																		 0,
																		 0,
																		 0});
			this.udDDSPLLMult.Name = "udDDSPLLMult";
			this.udDDSPLLMult.Size = new System.Drawing.Size(48, 20);
			this.udDDSPLLMult.TabIndex = 3;
			this.toolTip1.SetToolTip(this.udDDSPLLMult, "Multiplier for external clock (1 if using internal clock)");
			this.udDDSPLLMult.Value = new System.Decimal(new int[] {
																	   1,
																	   0,
																	   0,
																	   0});
			this.udDDSPLLMult.LostFocus += new System.EventHandler(this.udDDSPLLMult_LostFocus);
			this.udDDSPLLMult.ValueChanged += new System.EventHandler(this.udDDSPLLMult_ValueChanged);
			// 
			// lblPLLMult
			// 
			this.lblPLLMult.Image = null;
			this.lblPLLMult.Location = new System.Drawing.Point(16, 48);
			this.lblPLLMult.Name = "lblPLLMult";
			this.lblPLLMult.Size = new System.Drawing.Size(80, 23);
			this.lblPLLMult.TabIndex = 2;
			this.lblPLLMult.Text = "PLL Multiplier:";
			// 
			// grpGeneralModel
			// 
			this.grpGeneralModel.Controls.Add(this.radGenModelDemoNone);
			this.grpGeneralModel.Controls.Add(this.radGenModelSoftRock40);
			this.grpGeneralModel.Controls.Add(this.radGenModelSDR1000);
			this.grpGeneralModel.Location = new System.Drawing.Point(8, 8);
			this.grpGeneralModel.Name = "grpGeneralModel";
			this.grpGeneralModel.Size = new System.Drawing.Size(144, 101);
			this.grpGeneralModel.TabIndex = 25;
			this.grpGeneralModel.TabStop = false;
			this.grpGeneralModel.Text = "Radio Model";
			// 
			// radGenModelDemoNone
			// 
			this.radGenModelDemoNone.Image = null;
			this.radGenModelDemoNone.Location = new System.Drawing.Point(16, 72);
			this.radGenModelDemoNone.Name = "radGenModelDemoNone";
			this.radGenModelDemoNone.Size = new System.Drawing.Size(88, 24);
			this.radGenModelDemoNone.TabIndex = 2;
			this.radGenModelDemoNone.Text = "Demo/None";
			this.toolTip1.SetToolTip(this.radGenModelDemoNone, "Select if using without any SDR hardware.");
			this.radGenModelDemoNone.CheckedChanged += new System.EventHandler(this.radGenModelDemoNone_CheckedChanged);
			// 
			// radGenModelSoftRock40
			// 
			this.radGenModelSoftRock40.Image = null;
			this.radGenModelSoftRock40.Location = new System.Drawing.Point(16, 48);
			this.radGenModelSoftRock40.Name = "radGenModelSoftRock40";
			this.radGenModelSoftRock40.Size = new System.Drawing.Size(88, 24);
			this.radGenModelSoftRock40.TabIndex = 1;
			this.radGenModelSoftRock40.Text = "Soft Rock 40";
			this.toolTip1.SetToolTip(this.radGenModelSoftRock40, "Select if using the SoftRock 40");
			this.radGenModelSoftRock40.CheckedChanged += new System.EventHandler(this.radGenModelSoftRock40_CheckedChanged);
			// 
			// radGenModelSDR1000
			// 
			this.radGenModelSDR1000.Checked = true;
			this.radGenModelSDR1000.Image = null;
			this.radGenModelSDR1000.Location = new System.Drawing.Point(16, 24);
			this.radGenModelSDR1000.Name = "radGenModelSDR1000";
			this.radGenModelSDR1000.Size = new System.Drawing.Size(88, 24);
			this.radGenModelSDR1000.TabIndex = 0;
			this.radGenModelSDR1000.TabStop = true;
			this.radGenModelSDR1000.Text = "SDR-1000";
			this.toolTip1.SetToolTip(this.radGenModelSDR1000, "Select if using the SDR-1000 Hardware");
			this.radGenModelSDR1000.CheckedChanged += new System.EventHandler(this.radGenModelSDR1000_CheckedChanged);
			// 
			// grpGeneralHardwareSetup
			// 
			this.grpGeneralHardwareSetup.Controls.Add(this.chkBoxJanusOzyControl);
			this.grpGeneralHardwareSetup.Controls.Add(this.chkGeneralRXOnly);
			this.grpGeneralHardwareSetup.Controls.Add(this.chkGeneralUSBPresent);
			this.grpGeneralHardwareSetup.Controls.Add(this.chkGeneralATUPresent);
			this.grpGeneralHardwareSetup.Controls.Add(this.chkGeneralPAPresent);
			this.grpGeneralHardwareSetup.Controls.Add(this.chkGeneralXVTRPresent);
			this.grpGeneralHardwareSetup.Controls.Add(this.lblGeneralLPTDelay);
			this.grpGeneralHardwareSetup.Controls.Add(this.udGeneralLPTDelay);
			this.grpGeneralHardwareSetup.Controls.Add(this.lblGeneralLPTAddr);
			this.grpGeneralHardwareSetup.Controls.Add(this.comboGeneralLPTAddr);
			this.grpGeneralHardwareSetup.Controls.Add(this.chkGeneralRFEPresent);
			this.grpGeneralHardwareSetup.Controls.Add(this.comboGeneralXVTR);
			this.grpGeneralHardwareSetup.Location = new System.Drawing.Point(160, 8);
			this.grpGeneralHardwareSetup.Name = "grpGeneralHardwareSetup";
			this.grpGeneralHardwareSetup.Size = new System.Drawing.Size(216, 248);
			this.grpGeneralHardwareSetup.TabIndex = 1;
			this.grpGeneralHardwareSetup.TabStop = false;
			this.grpGeneralHardwareSetup.Text = "Hardware Setup";
			// 
			// chkBoxJanusOzyControl
			// 
			this.chkBoxJanusOzyControl.Image = null;
			this.chkBoxJanusOzyControl.Location = new System.Drawing.Point(16, 224);
			this.chkBoxJanusOzyControl.Name = "chkBoxJanusOzyControl";
			this.chkBoxJanusOzyControl.Size = new System.Drawing.Size(168, 16);
			this.chkBoxJanusOzyControl.TabIndex = 13;
			this.chkBoxJanusOzyControl.Text = "Janus/Ozy Control";
			this.toolTip1.SetToolTip(this.chkBoxJanusOzyControl, "Check if the USB adapter is being used.");
			this.chkBoxJanusOzyControl.CheckedChanged += new System.EventHandler(this.chkBoxJanusOzyControl_CheckedChanged);
			// 
			// chkGeneralRXOnly
			// 
			this.chkGeneralRXOnly.Image = null;
			this.chkGeneralRXOnly.Location = new System.Drawing.Point(16, 88);
			this.chkGeneralRXOnly.Name = "chkGeneralRXOnly";
			this.chkGeneralRXOnly.Size = new System.Drawing.Size(96, 16);
			this.chkGeneralRXOnly.TabIndex = 11;
			this.chkGeneralRXOnly.Text = "Receive Only";
			this.toolTip1.SetToolTip(this.chkGeneralRXOnly, "Check to disable transmit functionality.");
			this.chkGeneralRXOnly.CheckedChanged += new System.EventHandler(this.chkGeneralRXOnly_CheckedChanged);
			// 
			// chkGeneralUSBPresent
			// 
			this.chkGeneralUSBPresent.Image = null;
			this.chkGeneralUSBPresent.Location = new System.Drawing.Point(16, 104);
			this.chkGeneralUSBPresent.Name = "chkGeneralUSBPresent";
			this.chkGeneralUSBPresent.Size = new System.Drawing.Size(96, 16);
			this.chkGeneralUSBPresent.TabIndex = 10;
			this.chkGeneralUSBPresent.Text = "USB Adapter";
			this.toolTip1.SetToolTip(this.chkGeneralUSBPresent, "Check if the USB adapter is being used.");
			this.chkGeneralUSBPresent.CheckedChanged += new System.EventHandler(this.chkGeneralUSBPresent_CheckedChanged);
			// 
			// chkGeneralATUPresent
			// 
			this.chkGeneralATUPresent.Image = null;
			this.chkGeneralATUPresent.Location = new System.Drawing.Point(16, 152);
			this.chkGeneralATUPresent.Name = "chkGeneralATUPresent";
			this.chkGeneralATUPresent.Size = new System.Drawing.Size(88, 16);
			this.chkGeneralATUPresent.TabIndex = 9;
			this.chkGeneralATUPresent.Text = "ATU Present";
			this.toolTip1.SetToolTip(this.chkGeneralATUPresent, "Check if integrated LDG Z-100 is installed.");
			this.chkGeneralATUPresent.Visible = false;
			this.chkGeneralATUPresent.CheckedChanged += new System.EventHandler(this.chkGeneralATUPresent_CheckedChanged);
			// 
			// chkGeneralPAPresent
			// 
			this.chkGeneralPAPresent.Image = null;
			this.chkGeneralPAPresent.Location = new System.Drawing.Point(16, 136);
			this.chkGeneralPAPresent.Name = "chkGeneralPAPresent";
			this.chkGeneralPAPresent.Size = new System.Drawing.Size(88, 16);
			this.chkGeneralPAPresent.TabIndex = 8;
			this.chkGeneralPAPresent.Text = "PA Present";
			this.toolTip1.SetToolTip(this.chkGeneralPAPresent, "Check if FlexRadio Systems 100W PA is installed.");
			this.chkGeneralPAPresent.Visible = false;
			this.chkGeneralPAPresent.CheckedChanged += new System.EventHandler(this.chkGeneralPAPresent_CheckedChanged);
			// 
			// chkGeneralXVTRPresent
			// 
			this.chkGeneralXVTRPresent.Image = null;
			this.chkGeneralXVTRPresent.Location = new System.Drawing.Point(16, 168);
			this.chkGeneralXVTRPresent.Name = "chkGeneralXVTRPresent";
			this.chkGeneralXVTRPresent.Size = new System.Drawing.Size(104, 16);
			this.chkGeneralXVTRPresent.TabIndex = 7;
			this.chkGeneralXVTRPresent.Text = "XVTR Present";
			this.toolTip1.SetToolTip(this.chkGeneralXVTRPresent, "Check if DEMI XVTR is installed.");
			this.chkGeneralXVTRPresent.Visible = false;
			this.chkGeneralXVTRPresent.CheckedChanged += new System.EventHandler(this.chkXVTRPresent_CheckedChanged);
			// 
			// lblGeneralLPTDelay
			// 
			this.lblGeneralLPTDelay.Image = null;
			this.lblGeneralLPTDelay.Location = new System.Drawing.Point(16, 56);
			this.lblGeneralLPTDelay.Name = "lblGeneralLPTDelay";
			this.lblGeneralLPTDelay.Size = new System.Drawing.Size(80, 16);
			this.lblGeneralLPTDelay.TabIndex = 6;
			this.lblGeneralLPTDelay.Text = "LPT Delay:";
			// 
			// udGeneralLPTDelay
			// 
			this.udGeneralLPTDelay.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			this.udGeneralLPTDelay.Location = new System.Drawing.Point(96, 56);
			this.udGeneralLPTDelay.Maximum = new System.Decimal(new int[] {
																			  100,
																			  0,
																			  0,
																			  0});
			this.udGeneralLPTDelay.Minimum = new System.Decimal(new int[] {
																			  0,
																			  0,
																			  0,
																			  0});
			this.udGeneralLPTDelay.Name = "udGeneralLPTDelay";
			this.udGeneralLPTDelay.Size = new System.Drawing.Size(56, 20);
			this.udGeneralLPTDelay.TabIndex = 5;
			this.toolTip1.SetToolTip(this.udGeneralLPTDelay, "Delay to compensate for longer Parallel cables.");
			this.udGeneralLPTDelay.Value = new System.Decimal(new int[] {
																			0,
																			0,
																			0,
																			0});
			this.udGeneralLPTDelay.LostFocus += new System.EventHandler(this.udGeneralLPTDelay_LostFocus);
			this.udGeneralLPTDelay.ValueChanged += new System.EventHandler(this.udGeneralLPTDelay_ValueChanged);
			// 
			// lblGeneralLPTAddr
			// 
			this.lblGeneralLPTAddr.Image = null;
			this.lblGeneralLPTAddr.Location = new System.Drawing.Point(16, 24);
			this.lblGeneralLPTAddr.Name = "lblGeneralLPTAddr";
			this.lblGeneralLPTAddr.Size = new System.Drawing.Size(80, 16);
			this.lblGeneralLPTAddr.TabIndex = 3;
			this.lblGeneralLPTAddr.Text = "LPT Address:";
			// 
			// comboGeneralLPTAddr
			// 
			this.comboGeneralLPTAddr.DropDownWidth = 56;
			this.comboGeneralLPTAddr.Items.AddRange(new object[] {
																	 "278",
																	 "378",
																	 "3BC",
																	 "B800",
																	 "BC00"});
			this.comboGeneralLPTAddr.Location = new System.Drawing.Point(96, 24);
			this.comboGeneralLPTAddr.Name = "comboGeneralLPTAddr";
			this.comboGeneralLPTAddr.Size = new System.Drawing.Size(56, 21);
			this.comboGeneralLPTAddr.TabIndex = 0;
			this.comboGeneralLPTAddr.Text = "378";
			this.toolTip1.SetToolTip(this.comboGeneralLPTAddr, "Parallel Port Address");
			this.comboGeneralLPTAddr.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboGeneralLPTAddr_KeyDown);
			this.comboGeneralLPTAddr.LostFocus += new System.EventHandler(this.comboGeneralLPTAddr_LostFocus);
			this.comboGeneralLPTAddr.SelectedIndexChanged += new System.EventHandler(this.comboGeneralLPTAddr_SelectedIndexChanged);
			// 
			// chkGeneralRFEPresent
			// 
			this.chkGeneralRFEPresent.Image = null;
			this.chkGeneralRFEPresent.Location = new System.Drawing.Point(16, 120);
			this.chkGeneralRFEPresent.Name = "chkGeneralRFEPresent";
			this.chkGeneralRFEPresent.Size = new System.Drawing.Size(88, 16);
			this.chkGeneralRFEPresent.TabIndex = 2;
			this.chkGeneralRFEPresent.Text = "RFE Present";
			this.toolTip1.SetToolTip(this.chkGeneralRFEPresent, "Check if the RFE board is installed.");
			this.chkGeneralRFEPresent.CheckedChanged += new System.EventHandler(this.chkGeneralRFEPresent_CheckedChanged);
			// 
			// comboGeneralXVTR
			// 
			this.comboGeneralXVTR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGeneralXVTR.DropDownWidth = 136;
			this.comboGeneralXVTR.Items.AddRange(new object[] {
																  "Negative TR Logic",
																  "Positive TR Logic",
																  "No TR Logic"});
			this.comboGeneralXVTR.Location = new System.Drawing.Point(16, 192);
			this.comboGeneralXVTR.Name = "comboGeneralXVTR";
			this.comboGeneralXVTR.Size = new System.Drawing.Size(136, 21);
			this.comboGeneralXVTR.TabIndex = 5;
			this.toolTip1.SetToolTip(this.comboGeneralXVTR, "XVTR TR Logic Selection -- Negative for XVTR FlexRadio Systems provides.  Positiv" +
				"e for 25W version.  No TR logic for other XVTRs.");
			this.comboGeneralXVTR.Visible = false;
			this.comboGeneralXVTR.SelectedIndexChanged += new System.EventHandler(this.comboGeneralXVTR_SelectedIndexChanged);
			// 
			// btnWizard
			// 
			this.btnWizard.Image = null;
			this.btnWizard.Location = new System.Drawing.Point(24, 224);
			this.btnWizard.Name = "btnWizard";
			this.btnWizard.TabIndex = 22;
			this.btnWizard.Text = "Wizard...";
			this.toolTip1.SetToolTip(this.btnWizard, "Run the Startup Wizard.");
			this.btnWizard.Click += new System.EventHandler(this.btnWizard_Click);
			// 
			// tpGeneralOptions
			// 
			this.tpGeneralOptions.Controls.Add(this.grpOptMainConsole);
			this.tpGeneralOptions.Controls.Add(this.grpOptQuickQSY);
			this.tpGeneralOptions.Controls.Add(this.grpGenAutoMute);
			this.tpGeneralOptions.Controls.Add(this.grpGenTuningOptions);
			this.tpGeneralOptions.Controls.Add(this.grpGeneralOptions);
			this.tpGeneralOptions.Controls.Add(this.grpGeneralProcessPriority);
			this.tpGeneralOptions.Controls.Add(this.grpGeneralUpdates);
			this.tpGeneralOptions.Location = new System.Drawing.Point(4, 22);
			this.tpGeneralOptions.Name = "tpGeneralOptions";
			this.tpGeneralOptions.Size = new System.Drawing.Size(592, 318);
			this.tpGeneralOptions.TabIndex = 1;
			this.tpGeneralOptions.Text = "Options";
			// 
			// grpOptMainConsole
			// 
			this.grpOptMainConsole.Controls.Add(this.chkOptAlwaysOnTop);
			this.grpOptMainConsole.Location = new System.Drawing.Point(416, 96);
			this.grpOptMainConsole.Name = "grpOptMainConsole";
			this.grpOptMainConsole.Size = new System.Drawing.Size(128, 56);
			this.grpOptMainConsole.TabIndex = 28;
			this.grpOptMainConsole.TabStop = false;
			this.grpOptMainConsole.Text = "Main Console";
			// 
			// chkOptAlwaysOnTop
			// 
			this.chkOptAlwaysOnTop.Image = null;
			this.chkOptAlwaysOnTop.Location = new System.Drawing.Point(16, 24);
			this.chkOptAlwaysOnTop.Name = "chkOptAlwaysOnTop";
			this.chkOptAlwaysOnTop.Size = new System.Drawing.Size(104, 16);
			this.chkOptAlwaysOnTop.TabIndex = 0;
			this.chkOptAlwaysOnTop.Text = "Always On Top";
			this.toolTip1.SetToolTip(this.chkOptAlwaysOnTop, "Check this box to set the main console to always be on top (visible).");
			this.chkOptAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkOptAlwaysOnTop_CheckedChanged);
			// 
			// grpOptQuickQSY
			// 
			this.grpOptQuickQSY.Controls.Add(this.chkOptEnableKBShortcuts);
			this.grpOptQuickQSY.Controls.Add(this.chkOptQuickQSY);
			this.grpOptQuickQSY.Location = new System.Drawing.Point(416, 160);
			this.grpOptQuickQSY.Name = "grpOptQuickQSY";
			this.grpOptQuickQSY.Size = new System.Drawing.Size(128, 72);
			this.grpOptQuickQSY.TabIndex = 27;
			this.grpOptQuickQSY.TabStop = false;
			this.grpOptQuickQSY.Text = "Keyboard";
			// 
			// chkOptEnableKBShortcuts
			// 
			this.chkOptEnableKBShortcuts.Checked = true;
			this.chkOptEnableKBShortcuts.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkOptEnableKBShortcuts.Image = null;
			this.chkOptEnableKBShortcuts.Location = new System.Drawing.Point(16, 24);
			this.chkOptEnableKBShortcuts.Name = "chkOptEnableKBShortcuts";
			this.chkOptEnableKBShortcuts.Size = new System.Drawing.Size(109, 16);
			this.chkOptEnableKBShortcuts.TabIndex = 1;
			this.chkOptEnableKBShortcuts.Text = "Enable Shortcuts";
			this.toolTip1.SetToolTip(this.chkOptEnableKBShortcuts, "Enable keyboard shortcuts.  If this box is not checked, none of the keyboard shor" +
				"tcuts other than those that are built into windows will function.");
			this.chkOptEnableKBShortcuts.CheckedChanged += new System.EventHandler(this.chkOptEnableKBShortcuts_CheckedChanged);
			// 
			// chkOptQuickQSY
			// 
			this.chkOptQuickQSY.Checked = true;
			this.chkOptQuickQSY.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkOptQuickQSY.Image = null;
			this.chkOptQuickQSY.Location = new System.Drawing.Point(16, 48);
			this.chkOptQuickQSY.Name = "chkOptQuickQSY";
			this.chkOptQuickQSY.Size = new System.Drawing.Size(80, 16);
			this.chkOptQuickQSY.TabIndex = 0;
			this.chkOptQuickQSY.Text = "Quick QSY";
			this.toolTip1.SetToolTip(this.chkOptQuickQSY, "Enabled the Quick QSY feature -- directly enter the frequency in MHz while the ma" +
				"in form has the focus and hit enter.");
			this.chkOptQuickQSY.CheckedChanged += new System.EventHandler(this.chkOptQuickQSY_CheckedChanged);
			// 
			// grpGenAutoMute
			// 
			this.grpGenAutoMute.Controls.Add(this.chkGenAutoMute);
			this.grpGenAutoMute.Location = new System.Drawing.Point(264, 72);
			this.grpGenAutoMute.Name = "grpGenAutoMute";
			this.grpGenAutoMute.Size = new System.Drawing.Size(96, 56);
			this.grpGenAutoMute.TabIndex = 26;
			this.grpGenAutoMute.TabStop = false;
			this.grpGenAutoMute.Text = "Auto Mute";
			// 
			// chkGenAutoMute
			// 
			this.chkGenAutoMute.Image = null;
			this.chkGenAutoMute.Location = new System.Drawing.Point(16, 24);
			this.chkGenAutoMute.Name = "chkGenAutoMute";
			this.chkGenAutoMute.Size = new System.Drawing.Size(72, 16);
			this.chkGenAutoMute.TabIndex = 0;
			this.chkGenAutoMute.Text = "Enabled";
			this.toolTip1.SetToolTip(this.chkGenAutoMute, "Check this box to enable the software to poll Pin X2-12 to look for a signal to m" +
				"ute the radio.");
			this.chkGenAutoMute.CheckedChanged += new System.EventHandler(this.chkGenAutoMute_CheckedChanged);
			// 
			// grpGenTuningOptions
			// 
			this.grpGenTuningOptions.Controls.Add(this.lblOptClickTuneDIGL);
			this.grpGenTuningOptions.Controls.Add(this.udOptClickTuneOffsetDIGL);
			this.grpGenTuningOptions.Controls.Add(this.lblOptClickTuneDIGU);
			this.grpGenTuningOptions.Controls.Add(this.udOptClickTuneOffsetDIGU);
			this.grpGenTuningOptions.Location = new System.Drawing.Point(8, 136);
			this.grpGenTuningOptions.Name = "grpGenTuningOptions";
			this.grpGenTuningOptions.Size = new System.Drawing.Size(144, 80);
			this.grpGenTuningOptions.TabIndex = 25;
			this.grpGenTuningOptions.TabStop = false;
			this.grpGenTuningOptions.Text = "Click Tune Offsets (Hz)";
			// 
			// lblOptClickTuneDIGL
			// 
			this.lblOptClickTuneDIGL.Image = null;
			this.lblOptClickTuneDIGL.Location = new System.Drawing.Point(16, 48);
			this.lblOptClickTuneDIGL.Name = "lblOptClickTuneDIGL";
			this.lblOptClickTuneDIGL.Size = new System.Drawing.Size(40, 23);
			this.lblOptClickTuneDIGL.TabIndex = 12;
			this.lblOptClickTuneDIGL.Text = "DIGL:";
			// 
			// udOptClickTuneOffsetDIGL
			// 
			this.udOptClickTuneOffsetDIGL.Increment = new System.Decimal(new int[] {
																					   1,
																					   0,
																					   0,
																					   0});
			this.udOptClickTuneOffsetDIGL.Location = new System.Drawing.Point(56, 48);
			this.udOptClickTuneOffsetDIGL.Maximum = new System.Decimal(new int[] {
																					 9999,
																					 0,
																					 0,
																					 0});
			this.udOptClickTuneOffsetDIGL.Minimum = new System.Decimal(new int[] {
																					 0,
																					 0,
																					 0,
																					 0});
			this.udOptClickTuneOffsetDIGL.Name = "udOptClickTuneOffsetDIGL";
			this.udOptClickTuneOffsetDIGL.Size = new System.Drawing.Size(56, 20);
			this.udOptClickTuneOffsetDIGL.TabIndex = 11;
			this.udOptClickTuneOffsetDIGL.Value = new System.Decimal(new int[] {
																				   2210,
																				   0,
																				   0,
																				   0});
			this.udOptClickTuneOffsetDIGL.LostFocus += new System.EventHandler(this.udOptClickTuneOffsetDIGL_LostFocus);
			this.udOptClickTuneOffsetDIGL.ValueChanged += new System.EventHandler(this.udOptClickTuneOffsetDIGL_ValueChanged);
			// 
			// lblOptClickTuneDIGU
			// 
			this.lblOptClickTuneDIGU.Image = null;
			this.lblOptClickTuneDIGU.Location = new System.Drawing.Point(16, 24);
			this.lblOptClickTuneDIGU.Name = "lblOptClickTuneDIGU";
			this.lblOptClickTuneDIGU.Size = new System.Drawing.Size(40, 23);
			this.lblOptClickTuneDIGU.TabIndex = 10;
			this.lblOptClickTuneDIGU.Text = "DIGU:";
			// 
			// udOptClickTuneOffsetDIGU
			// 
			this.udOptClickTuneOffsetDIGU.Increment = new System.Decimal(new int[] {
																					   1,
																					   0,
																					   0,
																					   0});
			this.udOptClickTuneOffsetDIGU.Location = new System.Drawing.Point(56, 24);
			this.udOptClickTuneOffsetDIGU.Maximum = new System.Decimal(new int[] {
																					 9999,
																					 0,
																					 0,
																					 0});
			this.udOptClickTuneOffsetDIGU.Minimum = new System.Decimal(new int[] {
																					 0,
																					 0,
																					 0,
																					 0});
			this.udOptClickTuneOffsetDIGU.Name = "udOptClickTuneOffsetDIGU";
			this.udOptClickTuneOffsetDIGU.Size = new System.Drawing.Size(56, 20);
			this.udOptClickTuneOffsetDIGU.TabIndex = 0;
			this.udOptClickTuneOffsetDIGU.Value = new System.Decimal(new int[] {
																				   1200,
																				   0,
																				   0,
																				   0});
			this.udOptClickTuneOffsetDIGU.LostFocus += new System.EventHandler(this.udOptClickTuneOffsetDIGU_LostFocus);
			this.udOptClickTuneOffsetDIGU.ValueChanged += new System.EventHandler(this.udOptClickTuneOffsetDIGU_ValueChanged);
			// 
			// grpGeneralOptions
			// 
			this.grpGeneralOptions.Controls.Add(this.chkGeneralCustomFilter);
			this.grpGeneralOptions.Controls.Add(this.lblGeneralX2Delay);
			this.grpGeneralOptions.Controls.Add(this.udGeneralX2Delay);
			this.grpGeneralOptions.Controls.Add(this.chkGeneralEnableX2);
			this.grpGeneralOptions.Controls.Add(this.chkGeneralSoftwareGainCorr);
			this.grpGeneralOptions.Controls.Add(this.chkGeneralDisablePTT);
			this.grpGeneralOptions.Controls.Add(this.chkGeneralSpurRed);
			this.grpGeneralOptions.Location = new System.Drawing.Point(8, 8);
			this.grpGeneralOptions.Name = "grpGeneralOptions";
			this.grpGeneralOptions.Size = new System.Drawing.Size(248, 120);
			this.grpGeneralOptions.TabIndex = 6;
			this.grpGeneralOptions.TabStop = false;
			this.grpGeneralOptions.Text = "Options";
			// 
			// chkGeneralCustomFilter
			// 
			this.chkGeneralCustomFilter.Image = null;
			this.chkGeneralCustomFilter.Location = new System.Drawing.Point(128, 80);
			this.chkGeneralCustomFilter.Name = "chkGeneralCustomFilter";
			this.chkGeneralCustomFilter.Size = new System.Drawing.Size(104, 32);
			this.chkGeneralCustomFilter.TabIndex = 10;
			this.chkGeneralCustomFilter.Text = "Enable 300kHz Filter";
			this.toolTip1.SetToolTip(this.chkGeneralCustomFilter, "If the custom filter bank on the RFE is configured for 300kHz LPF, use this setti" +
				"ng.");
			this.chkGeneralCustomFilter.CheckedChanged += new System.EventHandler(this.chkGeneralCustomFilter_CheckedChanged);
			// 
			// lblGeneralX2Delay
			// 
			this.lblGeneralX2Delay.Image = null;
			this.lblGeneralX2Delay.Location = new System.Drawing.Point(128, 56);
			this.lblGeneralX2Delay.Name = "lblGeneralX2Delay";
			this.lblGeneralX2Delay.Size = new System.Drawing.Size(56, 23);
			this.lblGeneralX2Delay.TabIndex = 9;
			this.lblGeneralX2Delay.Text = "X2 Delay:";
			// 
			// udGeneralX2Delay
			// 
			this.udGeneralX2Delay.Enabled = false;
			this.udGeneralX2Delay.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   0});
			this.udGeneralX2Delay.Location = new System.Drawing.Point(184, 56);
			this.udGeneralX2Delay.Maximum = new System.Decimal(new int[] {
																			 1000,
																			 0,
																			 0,
																			 0});
			this.udGeneralX2Delay.Minimum = new System.Decimal(new int[] {
																			 0,
																			 0,
																			 0,
																			 0});
			this.udGeneralX2Delay.Name = "udGeneralX2Delay";
			this.udGeneralX2Delay.Size = new System.Drawing.Size(48, 20);
			this.udGeneralX2Delay.TabIndex = 8;
			this.toolTip1.SetToolTip(this.udGeneralX2Delay, "Sets the Delay on TR switching when the sequencing above is enabled.");
			this.udGeneralX2Delay.Value = new System.Decimal(new int[] {
																		   100,
																		   0,
																		   0,
																		   0});
			this.udGeneralX2Delay.LostFocus += new System.EventHandler(this.udGeneralX2Delay_LostFocus);
			this.udGeneralX2Delay.ValueChanged += new System.EventHandler(this.udGeneralX2Delay_ValueChanged);
			// 
			// chkGeneralEnableX2
			// 
			this.chkGeneralEnableX2.Image = null;
			this.chkGeneralEnableX2.Location = new System.Drawing.Point(128, 20);
			this.chkGeneralEnableX2.Name = "chkGeneralEnableX2";
			this.chkGeneralEnableX2.Size = new System.Drawing.Size(96, 32);
			this.chkGeneralEnableX2.TabIndex = 7;
			this.chkGeneralEnableX2.Text = "Enable X2 TR Sequencing";
			this.toolTip1.SetToolTip(this.chkGeneralEnableX2, "Check this box to enable X2-7 TR sequencing using the delay set below.");
			this.chkGeneralEnableX2.CheckedChanged += new System.EventHandler(this.chkGeneralEnableX2_CheckedChanged);
			// 
			// chkGeneralSoftwareGainCorr
			// 
			this.chkGeneralSoftwareGainCorr.Image = null;
			this.chkGeneralSoftwareGainCorr.Location = new System.Drawing.Point(16, 72);
			this.chkGeneralSoftwareGainCorr.Name = "chkGeneralSoftwareGainCorr";
			this.chkGeneralSoftwareGainCorr.Size = new System.Drawing.Size(112, 32);
			this.chkGeneralSoftwareGainCorr.TabIndex = 6;
			this.chkGeneralSoftwareGainCorr.Text = "Disable Software Gain Correction";
			this.toolTip1.SetToolTip(this.chkGeneralSoftwareGainCorr, "Don\'t compensate in software for hardware gain or attenuation.");
			this.chkGeneralSoftwareGainCorr.CheckedChanged += new System.EventHandler(this.chkGeneralSoftwareGainCorr_CheckedChanged);
			// 
			// chkGeneralDisablePTT
			// 
			this.chkGeneralDisablePTT.Image = null;
			this.chkGeneralDisablePTT.Location = new System.Drawing.Point(16, 48);
			this.chkGeneralDisablePTT.Name = "chkGeneralDisablePTT";
			this.chkGeneralDisablePTT.TabIndex = 4;
			this.chkGeneralDisablePTT.Text = "Disable PTT";
			this.toolTip1.SetToolTip(this.chkGeneralDisablePTT, "Disable Push To Talk detection.");
			this.chkGeneralDisablePTT.CheckedChanged += new System.EventHandler(this.chkGeneralDisablePTT_CheckedChanged);
			// 
			// chkGeneralSpurRed
			// 
			this.chkGeneralSpurRed.Checked = true;
			this.chkGeneralSpurRed.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkGeneralSpurRed.Image = null;
			this.chkGeneralSpurRed.Location = new System.Drawing.Point(16, 24);
			this.chkGeneralSpurRed.Name = "chkGeneralSpurRed";
			this.chkGeneralSpurRed.TabIndex = 3;
			this.chkGeneralSpurRed.Text = "Spur Reduction";
			this.toolTip1.SetToolTip(this.chkGeneralSpurRed, "Enable Spur Reduction/Avoidance Routine");
			this.chkGeneralSpurRed.CheckedChanged += new System.EventHandler(this.chkGeneralSpurRed_CheckedChanged);
			// 
			// grpGeneralProcessPriority
			// 
			this.grpGeneralProcessPriority.Controls.Add(this.comboGeneralProcessPriority);
			this.grpGeneralProcessPriority.Location = new System.Drawing.Point(264, 8);
			this.grpGeneralProcessPriority.Name = "grpGeneralProcessPriority";
			this.grpGeneralProcessPriority.Size = new System.Drawing.Size(144, 56);
			this.grpGeneralProcessPriority.TabIndex = 23;
			this.grpGeneralProcessPriority.TabStop = false;
			this.grpGeneralProcessPriority.Text = "Process Priority";
			// 
			// comboGeneralProcessPriority
			// 
			this.comboGeneralProcessPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGeneralProcessPriority.DropDownWidth = 112;
			this.comboGeneralProcessPriority.Items.AddRange(new object[] {
																			 "Idle",
																			 "Below Normal",
																			 "Normal",
																			 "Above Normal",
																			 "High",
																			 "Real Time"});
			this.comboGeneralProcessPriority.Location = new System.Drawing.Point(16, 24);
			this.comboGeneralProcessPriority.Name = "comboGeneralProcessPriority";
			this.comboGeneralProcessPriority.Size = new System.Drawing.Size(112, 21);
			this.comboGeneralProcessPriority.TabIndex = 0;
			this.toolTip1.SetToolTip(this.comboGeneralProcessPriority, "Sets the process priority of the PowerSDR software.");
			this.comboGeneralProcessPriority.SelectedIndexChanged += new System.EventHandler(this.comboGeneralProcessPriority_SelectedIndexChanged);
			// 
			// grpGeneralUpdates
			// 
			this.grpGeneralUpdates.Controls.Add(this.chkGeneralUpdateBeta);
			this.grpGeneralUpdates.Controls.Add(this.chkGeneralUpdateRelease);
			this.grpGeneralUpdates.Location = new System.Drawing.Point(416, 8);
			this.grpGeneralUpdates.Name = "grpGeneralUpdates";
			this.grpGeneralUpdates.Size = new System.Drawing.Size(120, 80);
			this.grpGeneralUpdates.TabIndex = 24;
			this.grpGeneralUpdates.TabStop = false;
			this.grpGeneralUpdates.Text = "Update Notification";
			// 
			// chkGeneralUpdateBeta
			// 
			this.chkGeneralUpdateBeta.Checked = true;
			this.chkGeneralUpdateBeta.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkGeneralUpdateBeta.Image = null;
			this.chkGeneralUpdateBeta.Location = new System.Drawing.Point(16, 48);
			this.chkGeneralUpdateBeta.Name = "chkGeneralUpdateBeta";
			this.chkGeneralUpdateBeta.Size = new System.Drawing.Size(56, 16);
			this.chkGeneralUpdateBeta.TabIndex = 1;
			this.chkGeneralUpdateBeta.Text = "Beta";
			this.toolTip1.SetToolTip(this.chkGeneralUpdateBeta, "Check this box to enable the PowerSDR software to look at the FlexRadio website a" +
				"nd see if there are any new Beta Releases");
			this.chkGeneralUpdateBeta.CheckedChanged += new System.EventHandler(this.chkGeneralUpdateBeta_CheckedChanged);
			// 
			// chkGeneralUpdateRelease
			// 
			this.chkGeneralUpdateRelease.Checked = true;
			this.chkGeneralUpdateRelease.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkGeneralUpdateRelease.Image = null;
			this.chkGeneralUpdateRelease.Location = new System.Drawing.Point(16, 24);
			this.chkGeneralUpdateRelease.Name = "chkGeneralUpdateRelease";
			this.chkGeneralUpdateRelease.Size = new System.Drawing.Size(72, 16);
			this.chkGeneralUpdateRelease.TabIndex = 0;
			this.chkGeneralUpdateRelease.Text = "Release";
			this.toolTip1.SetToolTip(this.chkGeneralUpdateRelease, "Check this box to enable the PowerSDR software to look at the FlexRadio website a" +
				"nd see if there are any new Official Releases");
			this.chkGeneralUpdateRelease.CheckedChanged += new System.EventHandler(this.chkGeneralUpdateRelease_CheckedChanged);
			// 
			// tpGeneralCalibration
			// 
			this.tpGeneralCalibration.Controls.Add(this.grpGenCalRXImage);
			this.tpGeneralCalibration.Controls.Add(this.grpGenCalLevel);
			this.tpGeneralCalibration.Controls.Add(this.grpGeneralCalibration);
			this.tpGeneralCalibration.Location = new System.Drawing.Point(4, 22);
			this.tpGeneralCalibration.Name = "tpGeneralCalibration";
			this.tpGeneralCalibration.Size = new System.Drawing.Size(592, 318);
			this.tpGeneralCalibration.TabIndex = 2;
			this.tpGeneralCalibration.Text = "Calibration";
			// 
			// grpGenCalRXImage
			// 
			this.grpGenCalRXImage.Controls.Add(this.udGeneralCalFreq3);
			this.grpGenCalRXImage.Controls.Add(this.lblGenCalRXImageFreq);
			this.grpGenCalRXImage.Controls.Add(this.btnGeneralCalImageStart);
			this.grpGenCalRXImage.Location = new System.Drawing.Point(360, 8);
			this.grpGenCalRXImage.Name = "grpGenCalRXImage";
			this.grpGenCalRXImage.Size = new System.Drawing.Size(168, 112);
			this.grpGenCalRXImage.TabIndex = 9;
			this.grpGenCalRXImage.TabStop = false;
			this.grpGenCalRXImage.Text = "RX Image Reject Cal";
			// 
			// udGeneralCalFreq3
			// 
			this.udGeneralCalFreq3.DecimalPlaces = 6;
			this.udGeneralCalFreq3.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			this.udGeneralCalFreq3.Location = new System.Drawing.Point(80, 24);
			this.udGeneralCalFreq3.Maximum = new System.Decimal(new int[] {
																			  65,
																			  0,
																			  0,
																			  0});
			this.udGeneralCalFreq3.Minimum = new System.Decimal(new int[] {
																			  0,
																			  0,
																			  0,
																			  0});
			this.udGeneralCalFreq3.Name = "udGeneralCalFreq3";
			this.udGeneralCalFreq3.Size = new System.Drawing.Size(72, 20);
			this.udGeneralCalFreq3.TabIndex = 1;
			this.toolTip1.SetToolTip(this.udGeneralCalFreq3, "RX Image calibration reference frequency");
			this.udGeneralCalFreq3.Value = new System.Decimal(new int[] {
																			10,
																			0,
																			0,
																			0});
			this.udGeneralCalFreq3.LostFocus += new System.EventHandler(this.udGeneralCalFreq3_LostFocus);
			// 
			// lblGenCalRXImageFreq
			// 
			this.lblGenCalRXImageFreq.Image = null;
			this.lblGenCalRXImageFreq.Location = new System.Drawing.Point(16, 24);
			this.lblGenCalRXImageFreq.Name = "lblGenCalRXImageFreq";
			this.lblGenCalRXImageFreq.Size = new System.Drawing.Size(64, 23);
			this.lblGenCalRXImageFreq.TabIndex = 0;
			this.lblGenCalRXImageFreq.Text = "Frequency:";
			// 
			// btnGeneralCalImageStart
			// 
			this.btnGeneralCalImageStart.Image = null;
			this.btnGeneralCalImageStart.Location = new System.Drawing.Point(48, 80);
			this.btnGeneralCalImageStart.Name = "btnGeneralCalImageStart";
			this.btnGeneralCalImageStart.TabIndex = 7;
			this.btnGeneralCalImageStart.Text = "Start";
			this.toolTip1.SetToolTip(this.btnGeneralCalImageStart, "Click to start the RX Image rejection calibration using the above frequency refer" +
				"ence.");
			this.btnGeneralCalImageStart.Click += new System.EventHandler(this.btnGeneralCalImageStart_Click);
			// 
			// grpGenCalLevel
			// 
			this.grpGenCalLevel.Controls.Add(this.udGeneralCalLevel);
			this.grpGenCalLevel.Controls.Add(this.udGeneralCalFreq2);
			this.grpGenCalLevel.Controls.Add(this.lblGenCalLevelFreq);
			this.grpGenCalLevel.Controls.Add(this.lblGeneralCalLevel);
			this.grpGenCalLevel.Controls.Add(this.btnGeneralCalLevelStart);
			this.grpGenCalLevel.Location = new System.Drawing.Point(184, 8);
			this.grpGenCalLevel.Name = "grpGenCalLevel";
			this.grpGenCalLevel.Size = new System.Drawing.Size(168, 112);
			this.grpGenCalLevel.TabIndex = 8;
			this.grpGenCalLevel.TabStop = false;
			this.grpGenCalLevel.Text = "Level Cal";
			// 
			// udGeneralCalLevel
			// 
			this.udGeneralCalLevel.Increment = new System.Decimal(new int[] {
																				10,
																				0,
																				0,
																				0});
			this.udGeneralCalLevel.Location = new System.Drawing.Point(80, 48);
			this.udGeneralCalLevel.Maximum = new System.Decimal(new int[] {
																			  0,
																			  0,
																			  0,
																			  0});
			this.udGeneralCalLevel.Minimum = new System.Decimal(new int[] {
																			  150,
																			  0,
																			  0,
																			  -2147483648});
			this.udGeneralCalLevel.Name = "udGeneralCalLevel";
			this.udGeneralCalLevel.Size = new System.Drawing.Size(72, 20);
			this.udGeneralCalLevel.TabIndex = 3;
			this.toolTip1.SetToolTip(this.udGeneralCalLevel, "Level calibration reference level");
			this.udGeneralCalLevel.Value = new System.Decimal(new int[] {
																			70,
																			0,
																			0,
																			-2147483648});
			this.udGeneralCalLevel.LostFocus += new System.EventHandler(this.udGeneralCalLevel_LostFocus);
			// 
			// udGeneralCalFreq2
			// 
			this.udGeneralCalFreq2.DecimalPlaces = 6;
			this.udGeneralCalFreq2.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			this.udGeneralCalFreq2.Location = new System.Drawing.Point(80, 24);
			this.udGeneralCalFreq2.Maximum = new System.Decimal(new int[] {
																			  65,
																			  0,
																			  0,
																			  0});
			this.udGeneralCalFreq2.Minimum = new System.Decimal(new int[] {
																			  0,
																			  0,
																			  0,
																			  0});
			this.udGeneralCalFreq2.Name = "udGeneralCalFreq2";
			this.udGeneralCalFreq2.Size = new System.Drawing.Size(72, 20);
			this.udGeneralCalFreq2.TabIndex = 1;
			this.toolTip1.SetToolTip(this.udGeneralCalFreq2, "Level calibration reference frequency");
			this.udGeneralCalFreq2.Value = new System.Decimal(new int[] {
																			10,
																			0,
																			0,
																			0});
			this.udGeneralCalFreq2.LostFocus += new System.EventHandler(this.udGeneralCalFreq2_LostFocus);
			// 
			// lblGenCalLevelFreq
			// 
			this.lblGenCalLevelFreq.Image = null;
			this.lblGenCalLevelFreq.Location = new System.Drawing.Point(16, 24);
			this.lblGenCalLevelFreq.Name = "lblGenCalLevelFreq";
			this.lblGenCalLevelFreq.Size = new System.Drawing.Size(64, 23);
			this.lblGenCalLevelFreq.TabIndex = 0;
			this.lblGenCalLevelFreq.Text = "Frequency:";
			// 
			// lblGeneralCalLevel
			// 
			this.lblGeneralCalLevel.Image = null;
			this.lblGeneralCalLevel.Location = new System.Drawing.Point(16, 48);
			this.lblGeneralCalLevel.Name = "lblGeneralCalLevel";
			this.lblGeneralCalLevel.Size = new System.Drawing.Size(68, 23);
			this.lblGeneralCalLevel.TabIndex = 2;
			this.lblGeneralCalLevel.Text = "Level (dBm):";
			// 
			// btnGeneralCalLevelStart
			// 
			this.btnGeneralCalLevelStart.Image = null;
			this.btnGeneralCalLevelStart.Location = new System.Drawing.Point(48, 80);
			this.btnGeneralCalLevelStart.Name = "btnGeneralCalLevelStart";
			this.btnGeneralCalLevelStart.TabIndex = 4;
			this.btnGeneralCalLevelStart.Text = "Start";
			this.toolTip1.SetToolTip(this.btnGeneralCalLevelStart, "Click to start the level calibration using the frequency and level references abo" +
				"ve.");
			this.btnGeneralCalLevelStart.Click += new System.EventHandler(this.btnGeneralCalLevelStart_Click);
			// 
			// grpGeneralCalibration
			// 
			this.grpGeneralCalibration.Controls.Add(this.btnGeneralCalFreqStart);
			this.grpGeneralCalibration.Controls.Add(this.udGeneralCalFreq1);
			this.grpGeneralCalibration.Controls.Add(this.lblGeneralCalFrequency);
			this.grpGeneralCalibration.Location = new System.Drawing.Point(8, 8);
			this.grpGeneralCalibration.Name = "grpGeneralCalibration";
			this.grpGeneralCalibration.Size = new System.Drawing.Size(168, 112);
			this.grpGeneralCalibration.TabIndex = 5;
			this.grpGeneralCalibration.TabStop = false;
			this.grpGeneralCalibration.Text = "Freq Cal";
			// 
			// btnGeneralCalFreqStart
			// 
			this.btnGeneralCalFreqStart.Image = null;
			this.btnGeneralCalFreqStart.Location = new System.Drawing.Point(48, 80);
			this.btnGeneralCalFreqStart.Name = "btnGeneralCalFreqStart";
			this.btnGeneralCalFreqStart.TabIndex = 5;
			this.btnGeneralCalFreqStart.Text = "Start";
			this.toolTip1.SetToolTip(this.btnGeneralCalFreqStart, "Click to start the frequency calibration using the reference frequency above.");
			this.btnGeneralCalFreqStart.Click += new System.EventHandler(this.btnGeneralCalFreqStart_Click);
			// 
			// udGeneralCalFreq1
			// 
			this.udGeneralCalFreq1.DecimalPlaces = 6;
			this.udGeneralCalFreq1.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			this.udGeneralCalFreq1.Location = new System.Drawing.Point(80, 24);
			this.udGeneralCalFreq1.Maximum = new System.Decimal(new int[] {
																			  65,
																			  0,
																			  0,
																			  0});
			this.udGeneralCalFreq1.Minimum = new System.Decimal(new int[] {
																			  0,
																			  0,
																			  0,
																			  0});
			this.udGeneralCalFreq1.Name = "udGeneralCalFreq1";
			this.udGeneralCalFreq1.Size = new System.Drawing.Size(72, 20);
			this.udGeneralCalFreq1.TabIndex = 1;
			this.toolTip1.SetToolTip(this.udGeneralCalFreq1, "Frequency calibration reference frequency");
			this.udGeneralCalFreq1.Value = new System.Decimal(new int[] {
																			10,
																			0,
																			0,
																			0});
			this.udGeneralCalFreq1.LostFocus += new System.EventHandler(this.udGeneralCalFreq1_LostFocus);
			// 
			// lblGeneralCalFrequency
			// 
			this.lblGeneralCalFrequency.Image = null;
			this.lblGeneralCalFrequency.Location = new System.Drawing.Point(16, 24);
			this.lblGeneralCalFrequency.Name = "lblGeneralCalFrequency";
			this.lblGeneralCalFrequency.Size = new System.Drawing.Size(64, 23);
			this.lblGeneralCalFrequency.TabIndex = 0;
			this.lblGeneralCalFrequency.Text = "Frequency:";
			// 
			// tpFilters
			// 
			this.tpFilters.Controls.Add(this.grpOptFilterControls);
			this.tpFilters.Location = new System.Drawing.Point(4, 22);
			this.tpFilters.Name = "tpFilters";
			this.tpFilters.Size = new System.Drawing.Size(592, 318);
			this.tpFilters.TabIndex = 3;
			this.tpFilters.Text = "Filters";
			// 
			// grpOptFilterControls
			// 
			this.grpOptFilterControls.Controls.Add(this.udFilterDefaultLowCut);
			this.grpOptFilterControls.Controls.Add(this.lblDefaultLowCut);
			this.grpOptFilterControls.Controls.Add(this.udOptMaxFilterShift);
			this.grpOptFilterControls.Controls.Add(this.lblOptMaxFilterShift);
			this.grpOptFilterControls.Controls.Add(this.comboOptFilterWidthMode);
			this.grpOptFilterControls.Controls.Add(this.lblOptWidthSliderMode);
			this.grpOptFilterControls.Controls.Add(this.udOptMaxFilterWidth);
			this.grpOptFilterControls.Controls.Add(this.lblOptMaxFilter);
			this.grpOptFilterControls.Controls.Add(this.chkOptFilterSaveChanges);
			this.grpOptFilterControls.Location = new System.Drawing.Point(8, 8);
			this.grpOptFilterControls.Name = "grpOptFilterControls";
			this.grpOptFilterControls.Size = new System.Drawing.Size(200, 152);
			this.grpOptFilterControls.TabIndex = 29;
			this.grpOptFilterControls.TabStop = false;
			this.grpOptFilterControls.Text = "Filter Controls";
			// 
			// udFilterDefaultLowCut
			// 
			this.udFilterDefaultLowCut.Increment = new System.Decimal(new int[] {
																					1,
																					0,
																					0,
																					0});
			this.udFilterDefaultLowCut.Location = new System.Drawing.Point(128, 120);
			this.udFilterDefaultLowCut.Maximum = new System.Decimal(new int[] {
																				  500,
																				  0,
																				  0,
																				  0});
			this.udFilterDefaultLowCut.Minimum = new System.Decimal(new int[] {
																				  0,
																				  0,
																				  0,
																				  0});
			this.udFilterDefaultLowCut.Name = "udFilterDefaultLowCut";
			this.udFilterDefaultLowCut.Size = new System.Drawing.Size(48, 20);
			this.udFilterDefaultLowCut.TabIndex = 17;
			this.toolTip1.SetToolTip(this.udFilterDefaultLowCut, "Sets the default low cut filter for filter changes");
			this.udFilterDefaultLowCut.Value = new System.Decimal(new int[] {
																				150,
																				0,
																				0,
																				0});
			this.udFilterDefaultLowCut.LostFocus += new System.EventHandler(this.udFilterDefaultLowCut_LostFocus);
			this.udFilterDefaultLowCut.ValueChanged += new System.EventHandler(this.udFilterDefaultLowCut_ValueChanged);
			// 
			// lblDefaultLowCut
			// 
			this.lblDefaultLowCut.Image = null;
			this.lblDefaultLowCut.Location = new System.Drawing.Point(16, 120);
			this.lblDefaultLowCut.Name = "lblDefaultLowCut";
			this.lblDefaultLowCut.Size = new System.Drawing.Size(120, 23);
			this.lblDefaultLowCut.TabIndex = 16;
			this.lblDefaultLowCut.Text = "Default Low Cut (Hz):";
			// 
			// udOptMaxFilterShift
			// 
			this.udOptMaxFilterShift.Increment = new System.Decimal(new int[] {
																				  1,
																				  0,
																				  0,
																				  0});
			this.udOptMaxFilterShift.Location = new System.Drawing.Point(128, 72);
			this.udOptMaxFilterShift.Maximum = new System.Decimal(new int[] {
																				9999,
																				0,
																				0,
																				0});
			this.udOptMaxFilterShift.Minimum = new System.Decimal(new int[] {
																				0,
																				0,
																				0,
																				0});
			this.udOptMaxFilterShift.Name = "udOptMaxFilterShift";
			this.udOptMaxFilterShift.Size = new System.Drawing.Size(48, 20);
			this.udOptMaxFilterShift.TabIndex = 13;
			this.toolTip1.SetToolTip(this.udOptMaxFilterShift, "Sets the maximum amount for the Shift control.  Set lower for finer resolution co" +
				"ntrol");
			this.udOptMaxFilterShift.Value = new System.Decimal(new int[] {
																			  9999,
																			  0,
																			  0,
																			  0});
			this.udOptMaxFilterShift.LostFocus += new System.EventHandler(this.udOptMaxFilterShift_LostFocus);
			this.udOptMaxFilterShift.ValueChanged += new System.EventHandler(this.udOptMaxFilterShift_ValueChanged);
			// 
			// lblOptMaxFilterShift
			// 
			this.lblOptMaxFilterShift.Image = null;
			this.lblOptMaxFilterShift.Location = new System.Drawing.Point(16, 72);
			this.lblOptMaxFilterShift.Name = "lblOptMaxFilterShift";
			this.lblOptMaxFilterShift.Size = new System.Drawing.Size(120, 23);
			this.lblOptMaxFilterShift.TabIndex = 14;
			this.lblOptMaxFilterShift.Text = "Max Filter Shift (Hz):";
			// 
			// comboOptFilterWidthMode
			// 
			this.comboOptFilterWidthMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboOptFilterWidthMode.DropDownWidth = 112;
			this.comboOptFilterWidthMode.Items.AddRange(new object[] {
																		 "Linear",
																		 "Log",
																		 "Log10"});
			this.comboOptFilterWidthMode.Location = new System.Drawing.Point(120, 48);
			this.comboOptFilterWidthMode.Name = "comboOptFilterWidthMode";
			this.comboOptFilterWidthMode.Size = new System.Drawing.Size(56, 21);
			this.comboOptFilterWidthMode.TabIndex = 12;
			this.toolTip1.SetToolTip(this.comboOptFilterWidthMode, "Sets the mapping for the filter width slider.");
			this.comboOptFilterWidthMode.SelectedIndexChanged += new System.EventHandler(this.comboOptFilterWidthMode_SelectedIndexChanged);
			// 
			// lblOptWidthSliderMode
			// 
			this.lblOptWidthSliderMode.Image = null;
			this.lblOptWidthSliderMode.Location = new System.Drawing.Point(16, 48);
			this.lblOptWidthSliderMode.Name = "lblOptWidthSliderMode";
			this.lblOptWidthSliderMode.Size = new System.Drawing.Size(104, 16);
			this.lblOptWidthSliderMode.TabIndex = 11;
			this.lblOptWidthSliderMode.Text = "Width Slider Mode:";
			// 
			// udOptMaxFilterWidth
			// 
			this.udOptMaxFilterWidth.Increment = new System.Decimal(new int[] {
																				  1,
																				  0,
																				  0,
																				  0});
			this.udOptMaxFilterWidth.Location = new System.Drawing.Point(128, 24);
			this.udOptMaxFilterWidth.Maximum = new System.Decimal(new int[] {
																				9999,
																				0,
																				0,
																				0});
			this.udOptMaxFilterWidth.Minimum = new System.Decimal(new int[] {
																				0,
																				0,
																				0,
																				0});
			this.udOptMaxFilterWidth.Name = "udOptMaxFilterWidth";
			this.udOptMaxFilterWidth.Size = new System.Drawing.Size(48, 20);
			this.udOptMaxFilterWidth.TabIndex = 0;
			this.toolTip1.SetToolTip(this.udOptMaxFilterWidth, "Wets the maximum filter bandwidth");
			this.udOptMaxFilterWidth.Value = new System.Decimal(new int[] {
																			  9999,
																			  0,
																			  0,
																			  0});
			this.udOptMaxFilterWidth.LostFocus += new System.EventHandler(this.udOptMaxFilterWidth_LostFocus);
			this.udOptMaxFilterWidth.ValueChanged += new System.EventHandler(this.udOptMaxFilterWidth_ValueChanged);
			// 
			// lblOptMaxFilter
			// 
			this.lblOptMaxFilter.Image = null;
			this.lblOptMaxFilter.Location = new System.Drawing.Point(16, 24);
			this.lblOptMaxFilter.Name = "lblOptMaxFilter";
			this.lblOptMaxFilter.Size = new System.Drawing.Size(120, 23);
			this.lblOptMaxFilter.TabIndex = 10;
			this.lblOptMaxFilter.Text = "Max Filter Width (Hz):";
			// 
			// chkOptFilterSaveChanges
			// 
			this.chkOptFilterSaveChanges.Image = null;
			this.chkOptFilterSaveChanges.Location = new System.Drawing.Point(16, 96);
			this.chkOptFilterSaveChanges.Name = "chkOptFilterSaveChanges";
			this.chkOptFilterSaveChanges.Size = new System.Drawing.Size(176, 16);
			this.chkOptFilterSaveChanges.TabIndex = 15;
			this.chkOptFilterSaveChanges.Text = "Save Slider/Display Changes";
			this.toolTip1.SetToolTip(this.chkOptFilterSaveChanges, "If checked, changes made to the filters via the display or sliders will be saved " +
				"in the Variable filter.");
			this.chkOptFilterSaveChanges.CheckedChanged += new System.EventHandler(this.chkOptFilterSaveChanges_CheckedChanged);
			// 
			// tpAudio
			// 
			this.tpAudio.Controls.Add(this.tcAudio);
			this.tpAudio.Location = new System.Drawing.Point(4, 22);
			this.tpAudio.Name = "tpAudio";
			this.tpAudio.Size = new System.Drawing.Size(584, 286);
			this.tpAudio.TabIndex = 0;
			this.tpAudio.Text = "Audio";
			// 
			// tcAudio
			// 
			this.tcAudio.Controls.Add(this.tpAudioCard1);
			this.tcAudio.Controls.Add(this.tpVAC);
			this.tcAudio.Location = new System.Drawing.Point(0, 0);
			this.tcAudio.Name = "tcAudio";
			this.tcAudio.SelectedIndex = 0;
			this.tcAudio.Size = new System.Drawing.Size(600, 344);
			this.tcAudio.TabIndex = 35;
			// 
			// tpAudioCard1
			// 
			this.tpAudioCard1.Controls.Add(this.grpAudioMicBoost);
			this.tpAudioCard1.Controls.Add(this.grpAudioChannels);
			this.tpAudioCard1.Controls.Add(this.grpAudioMicInGain1);
			this.tpAudioCard1.Controls.Add(this.grpAudioLineInGain1);
			this.tpAudioCard1.Controls.Add(this.grpAudioVolts1);
			this.tpAudioCard1.Controls.Add(this.grpAudioDetails1);
			this.tpAudioCard1.Controls.Add(this.grpAudioLatency1);
			this.tpAudioCard1.Controls.Add(this.grpAudioCard);
			this.tpAudioCard1.Controls.Add(this.grpAudioBufferSize1);
			this.tpAudioCard1.Controls.Add(this.grpAudioSampleRate1);
			this.tpAudioCard1.Location = new System.Drawing.Point(4, 22);
			this.tpAudioCard1.Name = "tpAudioCard1";
			this.tpAudioCard1.Size = new System.Drawing.Size(592, 318);
			this.tpAudioCard1.TabIndex = 0;
			this.tpAudioCard1.Text = "Sound Card";
			// 
			// grpAudioMicBoost
			// 
			this.grpAudioMicBoost.Controls.Add(this.chkAudioMicBoost);
			this.grpAudioMicBoost.Location = new System.Drawing.Point(368, 200);
			this.grpAudioMicBoost.Name = "grpAudioMicBoost";
			this.grpAudioMicBoost.Size = new System.Drawing.Size(72, 56);
			this.grpAudioMicBoost.TabIndex = 43;
			this.grpAudioMicBoost.TabStop = false;
			this.grpAudioMicBoost.Text = "Mic Boost";
			// 
			// chkAudioMicBoost
			// 
			this.chkAudioMicBoost.Image = null;
			this.chkAudioMicBoost.Location = new System.Drawing.Point(16, 20);
			this.chkAudioMicBoost.Name = "chkAudioMicBoost";
			this.chkAudioMicBoost.Size = new System.Drawing.Size(40, 16);
			this.chkAudioMicBoost.TabIndex = 6;
			this.chkAudioMicBoost.Text = "On";
			this.chkAudioMicBoost.CheckedChanged += new System.EventHandler(this.chkAudioMicBoost_CheckedChanged);
			// 
			// grpAudioChannels
			// 
			this.grpAudioChannels.Controls.Add(this.comboAudioChannels1);
			this.grpAudioChannels.Location = new System.Drawing.Point(440, 72);
			this.grpAudioChannels.Name = "grpAudioChannels";
			this.grpAudioChannels.Size = new System.Drawing.Size(96, 56);
			this.grpAudioChannels.TabIndex = 42;
			this.grpAudioChannels.TabStop = false;
			this.grpAudioChannels.Text = "Channels";
			// 
			// comboAudioChannels1
			// 
			this.comboAudioChannels1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAudioChannels1.DropDownWidth = 56;
			this.comboAudioChannels1.Items.AddRange(new object[] {
																	 "2",
																	 "4"});
			this.comboAudioChannels1.Location = new System.Drawing.Point(16, 24);
			this.comboAudioChannels1.Name = "comboAudioChannels1";
			this.comboAudioChannels1.Size = new System.Drawing.Size(56, 21);
			this.comboAudioChannels1.TabIndex = 0;
			this.toolTip1.SetToolTip(this.comboAudioChannels1, "Number of channels to open");
			this.comboAudioChannels1.SelectedIndexChanged += new System.EventHandler(this.comboAudioChannels1_SelectedIndexChanged);
			// 
			// grpAudioMicInGain1
			// 
			this.grpAudioMicInGain1.Controls.Add(this.udAudioMicGain1);
			this.grpAudioMicInGain1.Location = new System.Drawing.Point(344, 136);
			this.grpAudioMicInGain1.Name = "grpAudioMicInGain1";
			this.grpAudioMicInGain1.Size = new System.Drawing.Size(88, 56);
			this.grpAudioMicInGain1.TabIndex = 41;
			this.grpAudioMicInGain1.TabStop = false;
			this.grpAudioMicInGain1.Text = "Mic In Gain";
			// 
			// udAudioMicGain1
			// 
			this.udAudioMicGain1.Increment = new System.Decimal(new int[] {
																			  1,
																			  0,
																			  0,
																			  0});
			this.udAudioMicGain1.Location = new System.Drawing.Point(16, 24);
			this.udAudioMicGain1.Maximum = new System.Decimal(new int[] {
																			100,
																			0,
																			0,
																			0});
			this.udAudioMicGain1.Minimum = new System.Decimal(new int[] {
																			0,
																			0,
																			0,
																			0});
			this.udAudioMicGain1.Name = "udAudioMicGain1";
			this.udAudioMicGain1.Size = new System.Drawing.Size(40, 20);
			this.udAudioMicGain1.TabIndex = 51;
			this.toolTip1.SetToolTip(this.udAudioMicGain1, "MIC Gain - Input Volume");
			this.udAudioMicGain1.Value = new System.Decimal(new int[] {
																		  50,
																		  0,
																		  0,
																		  0});
			this.udAudioMicGain1.LostFocus += new System.EventHandler(this.udAudioMicGain1_LostFocus);
			this.udAudioMicGain1.ValueChanged += new System.EventHandler(this.udAudioMicGain1_ValueChanged);
			// 
			// grpAudioLineInGain1
			// 
			this.grpAudioLineInGain1.Controls.Add(this.udAudioLineIn1);
			this.grpAudioLineInGain1.Location = new System.Drawing.Point(344, 72);
			this.grpAudioLineInGain1.Name = "grpAudioLineInGain1";
			this.grpAudioLineInGain1.Size = new System.Drawing.Size(88, 56);
			this.grpAudioLineInGain1.TabIndex = 40;
			this.grpAudioLineInGain1.TabStop = false;
			this.grpAudioLineInGain1.Text = "Line In Gain";
			// 
			// udAudioLineIn1
			// 
			this.udAudioLineIn1.Increment = new System.Decimal(new int[] {
																			 1,
																			 0,
																			 0,
																			 0});
			this.udAudioLineIn1.Location = new System.Drawing.Point(16, 24);
			this.udAudioLineIn1.Maximum = new System.Decimal(new int[] {
																		   100,
																		   0,
																		   0,
																		   0});
			this.udAudioLineIn1.Minimum = new System.Decimal(new int[] {
																		   0,
																		   0,
																		   0,
																		   0});
			this.udAudioLineIn1.Name = "udAudioLineIn1";
			this.udAudioLineIn1.Size = new System.Drawing.Size(40, 20);
			this.udAudioLineIn1.TabIndex = 51;
			this.toolTip1.SetToolTip(this.udAudioLineIn1, "IF Gain - Input Volume");
			this.udAudioLineIn1.Value = new System.Decimal(new int[] {
																		 20,
																		 0,
																		 0,
																		 0});
			this.udAudioLineIn1.LostFocus += new System.EventHandler(this.udAudioLineIn1_LostFocus);
			this.udAudioLineIn1.ValueChanged += new System.EventHandler(this.udAudioLineIn1_ValueChanged);
			// 
			// grpAudioVolts1
			// 
			this.grpAudioVolts1.Controls.Add(this.btnAudioVoltTest1);
			this.grpAudioVolts1.Controls.Add(this.udAudioVoltage1);
			this.grpAudioVolts1.Location = new System.Drawing.Point(240, 200);
			this.grpAudioVolts1.Name = "grpAudioVolts1";
			this.grpAudioVolts1.Size = new System.Drawing.Size(128, 56);
			this.grpAudioVolts1.TabIndex = 39;
			this.grpAudioVolts1.TabStop = false;
			this.grpAudioVolts1.Text = "Output Voltage";
			// 
			// btnAudioVoltTest1
			// 
			this.btnAudioVoltTest1.Image = null;
			this.btnAudioVoltTest1.Location = new System.Drawing.Point(72, 24);
			this.btnAudioVoltTest1.Name = "btnAudioVoltTest1";
			this.btnAudioVoltTest1.Size = new System.Drawing.Size(40, 23);
			this.btnAudioVoltTest1.TabIndex = 2;
			this.btnAudioVoltTest1.Text = "Test";
			this.toolTip1.SetToolTip(this.btnAudioVoltTest1, "Outputs a full scale sinewave at the CW pitch for determining the RMS Voltage of " +
				"the sound card.");
			this.btnAudioVoltTest1.Click += new System.EventHandler(this.btnAudioVoltTest1_Click);
			// 
			// udAudioVoltage1
			// 
			this.udAudioVoltage1.DecimalPlaces = 2;
			this.udAudioVoltage1.Increment = new System.Decimal(new int[] {
																			  1,
																			  0,
																			  0,
																			  131072});
			this.udAudioVoltage1.Location = new System.Drawing.Point(16, 24);
			this.udAudioVoltage1.Maximum = new System.Decimal(new int[] {
																			100,
																			0,
																			0,
																			0});
			this.udAudioVoltage1.Minimum = new System.Decimal(new int[] {
																			0,
																			0,
																			0,
																			0});
			this.udAudioVoltage1.Name = "udAudioVoltage1";
			this.udAudioVoltage1.Size = new System.Drawing.Size(48, 20);
			this.udAudioVoltage1.TabIndex = 1;
			this.toolTip1.SetToolTip(this.udAudioVoltage1, "The measured VRMS on the sound card output when outputting a full range tone.");
			this.udAudioVoltage1.Value = new System.Decimal(new int[] {
																		  223,
																		  0,
																		  0,
																		  131072});
			this.udAudioVoltage1.LostFocus += new System.EventHandler(this.udAudioVoltage1_LostFocus);
			this.udAudioVoltage1.ValueChanged += new System.EventHandler(this.udAudioVoltage1_ValueChanged);
			// 
			// grpAudioDetails1
			// 
			this.grpAudioDetails1.Controls.Add(this.comboAudioTransmit1);
			this.grpAudioDetails1.Controls.Add(this.lblAudioMixer1);
			this.grpAudioDetails1.Controls.Add(this.lblAudioOutput1);
			this.grpAudioDetails1.Controls.Add(this.comboAudioOutput1);
			this.grpAudioDetails1.Controls.Add(this.lblAudioInput1);
			this.grpAudioDetails1.Controls.Add(this.lblAudioDriver1);
			this.grpAudioDetails1.Controls.Add(this.comboAudioInput1);
			this.grpAudioDetails1.Controls.Add(this.comboAudioDriver1);
			this.grpAudioDetails1.Controls.Add(this.comboAudioMixer1);
			this.grpAudioDetails1.Controls.Add(this.lblAudioTransmit1);
			this.grpAudioDetails1.Controls.Add(this.lblAudioReceive1);
			this.grpAudioDetails1.Controls.Add(this.comboAudioReceive1);
			this.grpAudioDetails1.Location = new System.Drawing.Point(8, 8);
			this.grpAudioDetails1.Name = "grpAudioDetails1";
			this.grpAudioDetails1.Size = new System.Drawing.Size(224, 216);
			this.grpAudioDetails1.TabIndex = 34;
			this.grpAudioDetails1.TabStop = false;
			this.grpAudioDetails1.Text = "Primary Sound Card Setup Details";
			// 
			// comboAudioTransmit1
			// 
			this.comboAudioTransmit1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAudioTransmit1.DropDownWidth = 160;
			this.comboAudioTransmit1.ItemHeight = 13;
			this.comboAudioTransmit1.Location = new System.Drawing.Point(56, 184);
			this.comboAudioTransmit1.Name = "comboAudioTransmit1";
			this.comboAudioTransmit1.Size = new System.Drawing.Size(160, 21);
			this.comboAudioTransmit1.TabIndex = 2;
			this.toolTip1.SetToolTip(this.comboAudioTransmit1, "Transmit mode mixer MUX setting.");
			this.comboAudioTransmit1.SelectedIndexChanged += new System.EventHandler(this.comboAudioTransmit1_SelectedIndexChanged);
			// 
			// lblAudioMixer1
			// 
			this.lblAudioMixer1.Image = null;
			this.lblAudioMixer1.Location = new System.Drawing.Point(8, 120);
			this.lblAudioMixer1.Name = "lblAudioMixer1";
			this.lblAudioMixer1.Size = new System.Drawing.Size(48, 23);
			this.lblAudioMixer1.TabIndex = 22;
			this.lblAudioMixer1.Text = "Mixer:";
			// 
			// lblAudioOutput1
			// 
			this.lblAudioOutput1.Image = null;
			this.lblAudioOutput1.Location = new System.Drawing.Point(8, 88);
			this.lblAudioOutput1.Name = "lblAudioOutput1";
			this.lblAudioOutput1.Size = new System.Drawing.Size(48, 16);
			this.lblAudioOutput1.TabIndex = 6;
			this.lblAudioOutput1.Text = "Output:";
			// 
			// comboAudioOutput1
			// 
			this.comboAudioOutput1.DisplayMember = "sdfg";
			this.comboAudioOutput1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAudioOutput1.DropDownWidth = 160;
			this.comboAudioOutput1.ItemHeight = 13;
			this.comboAudioOutput1.Location = new System.Drawing.Point(56, 88);
			this.comboAudioOutput1.Name = "comboAudioOutput1";
			this.comboAudioOutput1.Size = new System.Drawing.Size(160, 21);
			this.comboAudioOutput1.TabIndex = 5;
			this.toolTip1.SetToolTip(this.comboAudioOutput1, "Output Audio Device");
			this.comboAudioOutput1.SelectedIndexChanged += new System.EventHandler(this.comboAudioOutput1_SelectedIndexChanged);
			// 
			// lblAudioInput1
			// 
			this.lblAudioInput1.Image = null;
			this.lblAudioInput1.Location = new System.Drawing.Point(8, 56);
			this.lblAudioInput1.Name = "lblAudioInput1";
			this.lblAudioInput1.Size = new System.Drawing.Size(48, 16);
			this.lblAudioInput1.TabIndex = 4;
			this.lblAudioInput1.Text = "Input:";
			// 
			// lblAudioDriver1
			// 
			this.lblAudioDriver1.Image = null;
			this.lblAudioDriver1.Location = new System.Drawing.Point(8, 24);
			this.lblAudioDriver1.Name = "lblAudioDriver1";
			this.lblAudioDriver1.Size = new System.Drawing.Size(48, 16);
			this.lblAudioDriver1.TabIndex = 3;
			this.lblAudioDriver1.Text = "Driver:";
			// 
			// comboAudioInput1
			// 
			this.comboAudioInput1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAudioInput1.DropDownWidth = 160;
			this.comboAudioInput1.ItemHeight = 13;
			this.comboAudioInput1.Location = new System.Drawing.Point(56, 56);
			this.comboAudioInput1.Name = "comboAudioInput1";
			this.comboAudioInput1.Size = new System.Drawing.Size(160, 21);
			this.comboAudioInput1.TabIndex = 1;
			this.toolTip1.SetToolTip(this.comboAudioInput1, "Input Audio Device");
			this.comboAudioInput1.SelectedIndexChanged += new System.EventHandler(this.comboAudioInput1_SelectedIndexChanged);
			// 
			// comboAudioDriver1
			// 
			this.comboAudioDriver1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAudioDriver1.DropDownWidth = 160;
			this.comboAudioDriver1.ItemHeight = 13;
			this.comboAudioDriver1.Location = new System.Drawing.Point(56, 24);
			this.comboAudioDriver1.Name = "comboAudioDriver1";
			this.comboAudioDriver1.Size = new System.Drawing.Size(160, 21);
			this.comboAudioDriver1.TabIndex = 0;
			this.toolTip1.SetToolTip(this.comboAudioDriver1, "Sound Card Driver Selection");
			this.comboAudioDriver1.SelectedIndexChanged += new System.EventHandler(this.comboAudioDriver1_SelectedIndexChanged);
			// 
			// comboAudioMixer1
			// 
			this.comboAudioMixer1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAudioMixer1.DropDownWidth = 160;
			this.comboAudioMixer1.ItemHeight = 13;
			this.comboAudioMixer1.Location = new System.Drawing.Point(56, 120);
			this.comboAudioMixer1.Name = "comboAudioMixer1";
			this.comboAudioMixer1.Size = new System.Drawing.Size(160, 21);
			this.comboAudioMixer1.TabIndex = 21;
			this.toolTip1.SetToolTip(this.comboAudioMixer1, "Audio Mixer Device ");
			this.comboAudioMixer1.SelectedIndexChanged += new System.EventHandler(this.comboAudioMixer1_SelectedIndexChanged);
			// 
			// lblAudioTransmit1
			// 
			this.lblAudioTransmit1.Image = null;
			this.lblAudioTransmit1.Location = new System.Drawing.Point(8, 184);
			this.lblAudioTransmit1.Name = "lblAudioTransmit1";
			this.lblAudioTransmit1.Size = new System.Drawing.Size(56, 16);
			this.lblAudioTransmit1.TabIndex = 3;
			this.lblAudioTransmit1.Text = "Transmit:";
			// 
			// lblAudioReceive1
			// 
			this.lblAudioReceive1.Image = null;
			this.lblAudioReceive1.Location = new System.Drawing.Point(8, 152);
			this.lblAudioReceive1.Name = "lblAudioReceive1";
			this.lblAudioReceive1.Size = new System.Drawing.Size(48, 16);
			this.lblAudioReceive1.TabIndex = 1;
			this.lblAudioReceive1.Text = "Receive:";
			// 
			// comboAudioReceive1
			// 
			this.comboAudioReceive1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAudioReceive1.DropDownWidth = 160;
			this.comboAudioReceive1.ItemHeight = 13;
			this.comboAudioReceive1.Location = new System.Drawing.Point(56, 152);
			this.comboAudioReceive1.Name = "comboAudioReceive1";
			this.comboAudioReceive1.Size = new System.Drawing.Size(160, 21);
			this.comboAudioReceive1.TabIndex = 0;
			this.toolTip1.SetToolTip(this.comboAudioReceive1, "Receive mode Mixer MUX setting");
			this.comboAudioReceive1.SelectedIndexChanged += new System.EventHandler(this.comboAudioReceive1_SelectedIndexChanged);
			// 
			// grpAudioLatency1
			// 
			this.grpAudioLatency1.Controls.Add(this.chkAudioLatencyManual1);
			this.grpAudioLatency1.Controls.Add(this.udAudioLatency1);
			this.grpAudioLatency1.Location = new System.Drawing.Point(440, 136);
			this.grpAudioLatency1.Name = "grpAudioLatency1";
			this.grpAudioLatency1.Size = new System.Drawing.Size(96, 80);
			this.grpAudioLatency1.TabIndex = 38;
			this.grpAudioLatency1.TabStop = false;
			this.grpAudioLatency1.Text = "Latency (ms)";
			// 
			// chkAudioLatencyManual1
			// 
			this.chkAudioLatencyManual1.Image = null;
			this.chkAudioLatencyManual1.Location = new System.Drawing.Point(16, 24);
			this.chkAudioLatencyManual1.Name = "chkAudioLatencyManual1";
			this.chkAudioLatencyManual1.Size = new System.Drawing.Size(64, 16);
			this.chkAudioLatencyManual1.TabIndex = 5;
			this.chkAudioLatencyManual1.Text = "Manual";
			this.chkAudioLatencyManual1.CheckedChanged += new System.EventHandler(this.chkAudioLatencyManual1_CheckedChanged);
			// 
			// udAudioLatency1
			// 
			this.udAudioLatency1.Enabled = false;
			this.udAudioLatency1.Increment = new System.Decimal(new int[] {
																			  1,
																			  0,
																			  0,
																			  0});
			this.udAudioLatency1.Location = new System.Drawing.Point(16, 48);
			this.udAudioLatency1.Maximum = new System.Decimal(new int[] {
																			240,
																			0,
																			0,
																			0});
			this.udAudioLatency1.Minimum = new System.Decimal(new int[] {
																			0,
																			0,
																			0,
																			0});
			this.udAudioLatency1.Name = "udAudioLatency1";
			this.udAudioLatency1.Size = new System.Drawing.Size(48, 20);
			this.udAudioLatency1.TabIndex = 0;
			this.toolTip1.SetToolTip(this.udAudioLatency1, "Adds latency/stability to the audio subsystem.  Not needed when using ASIO driver" +
				".  Mainly for compatibility.  The Manual setting should only be used for unsuppo" +
				"rted cards.");
			this.udAudioLatency1.Value = new System.Decimal(new int[] {
																		  120,
																		  0,
																		  0,
																		  0});
			this.udAudioLatency1.LostFocus += new System.EventHandler(this.udAudioLatency1_LostFocus);
			this.udAudioLatency1.ValueChanged += new System.EventHandler(this.udAudioLatency1_ValueChanged);
			// 
			// grpAudioCard
			// 
			this.grpAudioCard.Controls.Add(this.comboAudioSoundCard);
			this.grpAudioCard.Location = new System.Drawing.Point(240, 8);
			this.grpAudioCard.Name = "grpAudioCard";
			this.grpAudioCard.Size = new System.Drawing.Size(224, 56);
			this.grpAudioCard.TabIndex = 37;
			this.grpAudioCard.TabStop = false;
			this.grpAudioCard.Text = "Sound Card Selection";
			this.grpAudioCard.Enter += new System.EventHandler(this.grpAudioCard_Enter);
			// 
			// comboAudioSoundCard
			// 
			this.comboAudioSoundCard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAudioSoundCard.DropDownWidth = 184;
			this.comboAudioSoundCard.Items.AddRange(new object[] {
																	 "M-Audio Delta 44 (PCI)",
																	 "PreSonus FireBox (FireWire)",
																	 "Edirol FA-66 (FireWire)",
																	 "SB Audigy (PCI)",
																	 "SB Audigy 2 (PCI)",
																	 "SB Audigy 2 ZS (PCI)",
																	 "HPSDR Janus/Ozy (USB2)",
																	 "Sound Blaster Extigy (USB)",
																	 "Sound Blaster MP3+ (USB)",
																	 "Turtle Beach Santa Cruz (PCI)",
																	 "Unsupported Card"});
			this.comboAudioSoundCard.Location = new System.Drawing.Point(24, 24);
			this.comboAudioSoundCard.MaxDropDownItems = 10;
			this.comboAudioSoundCard.Name = "comboAudioSoundCard";
			this.comboAudioSoundCard.Size = new System.Drawing.Size(184, 21);
			this.comboAudioSoundCard.TabIndex = 0;
			this.toolTip1.SetToolTip(this.comboAudioSoundCard, "Sound Card Selection (use Unsupported Card if your card isn\'t in the list -- this" +
				" will require manual setup of the below controls).");
			this.comboAudioSoundCard.SelectedIndexChanged += new System.EventHandler(this.comboAudioSoundCard_SelectedIndexChanged);
			// 
			// grpAudioBufferSize1
			// 
			this.grpAudioBufferSize1.Controls.Add(this.comboAudioBuffer1);
			this.grpAudioBufferSize1.Location = new System.Drawing.Point(240, 72);
			this.grpAudioBufferSize1.Name = "grpAudioBufferSize1";
			this.grpAudioBufferSize1.Size = new System.Drawing.Size(96, 56);
			this.grpAudioBufferSize1.TabIndex = 36;
			this.grpAudioBufferSize1.TabStop = false;
			this.grpAudioBufferSize1.Text = "Buffer Size";
			// 
			// comboAudioBuffer1
			// 
			this.comboAudioBuffer1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAudioBuffer1.DropDownWidth = 56;
			this.comboAudioBuffer1.Items.AddRange(new object[] {
																   "256",
																   "512",
																   "1024",
																   "2048"});
			this.comboAudioBuffer1.Location = new System.Drawing.Point(16, 24);
			this.comboAudioBuffer1.Name = "comboAudioBuffer1";
			this.comboAudioBuffer1.Size = new System.Drawing.Size(56, 21);
			this.comboAudioBuffer1.TabIndex = 0;
			this.toolTip1.SetToolTip(this.comboAudioBuffer1, "Samples per audio buffer.  Smaller settings give less latency, more CPU load.");
			this.comboAudioBuffer1.SelectedIndexChanged += new System.EventHandler(this.comboAudioBuffer1_SelectedIndexChanged);
			// 
			// grpAudioSampleRate1
			// 
			this.grpAudioSampleRate1.Controls.Add(this.comboAudioSampleRate1);
			this.grpAudioSampleRate1.Location = new System.Drawing.Point(240, 136);
			this.grpAudioSampleRate1.Name = "grpAudioSampleRate1";
			this.grpAudioSampleRate1.Size = new System.Drawing.Size(96, 56);
			this.grpAudioSampleRate1.TabIndex = 35;
			this.grpAudioSampleRate1.TabStop = false;
			this.grpAudioSampleRate1.Text = "Sample Rate";
			// 
			// comboAudioSampleRate1
			// 
			this.comboAudioSampleRate1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAudioSampleRate1.DropDownWidth = 64;
			this.comboAudioSampleRate1.Items.AddRange(new object[] {
																	   "48000"});
			this.comboAudioSampleRate1.Location = new System.Drawing.Point(16, 24);
			this.comboAudioSampleRate1.Name = "comboAudioSampleRate1";
			this.comboAudioSampleRate1.Size = new System.Drawing.Size(64, 21);
			this.comboAudioSampleRate1.TabIndex = 4;
			this.toolTip1.SetToolTip(this.comboAudioSampleRate1, "Sample Rate -- Range is dependent on selected sound card! ");
			this.comboAudioSampleRate1.SelectedIndexChanged += new System.EventHandler(this.comboAudioSampleRate1_SelectedIndexChanged);
			// 
			// tpVAC
			// 
			this.tpVAC.Controls.Add(this.grpAudioVACAutoEnable);
			this.tpVAC.Controls.Add(this.grpAudioVACGain);
			this.tpVAC.Controls.Add(this.grpAudio2Stereo);
			this.tpVAC.Controls.Add(this.grpAudioLatency2);
			this.tpVAC.Controls.Add(this.grpAudioSampleRate2);
			this.tpVAC.Controls.Add(this.grpAudioBuffer2);
			this.tpVAC.Controls.Add(this.grpAudioDetails2);
			this.tpVAC.Controls.Add(this.chkAudioEnableVAC);
			this.tpVAC.Location = new System.Drawing.Point(4, 22);
			this.tpVAC.Name = "tpVAC";
			this.tpVAC.Size = new System.Drawing.Size(592, 318);
			this.tpVAC.TabIndex = 1;
			this.tpVAC.Text = "VAC";
			// 
			// grpAudioVACAutoEnable
			// 
			this.grpAudioVACAutoEnable.Controls.Add(this.chkAudioVACAutoEnable);
			this.grpAudioVACAutoEnable.Location = new System.Drawing.Point(8, 168);
			this.grpAudioVACAutoEnable.Name = "grpAudioVACAutoEnable";
			this.grpAudioVACAutoEnable.Size = new System.Drawing.Size(224, 64);
			this.grpAudioVACAutoEnable.TabIndex = 74;
			this.grpAudioVACAutoEnable.TabStop = false;
			this.grpAudioVACAutoEnable.Text = "Auto Enable";
			// 
			// chkAudioVACAutoEnable
			// 
			this.chkAudioVACAutoEnable.Image = null;
			this.chkAudioVACAutoEnable.Location = new System.Drawing.Point(16, 24);
			this.chkAudioVACAutoEnable.Name = "chkAudioVACAutoEnable";
			this.chkAudioVACAutoEnable.Size = new System.Drawing.Size(200, 32);
			this.chkAudioVACAutoEnable.TabIndex = 0;
			this.chkAudioVACAutoEnable.Text = "Enable for Digital modes, Disable for all others";
			this.toolTip1.SetToolTip(this.chkAudioVACAutoEnable, "Click this button to automatically enable VAC when in Digital modes (DIGL, DIGU, " +
				"DRM)");
			this.chkAudioVACAutoEnable.CheckedChanged += new System.EventHandler(this.chkAudioVACAutoEnable_CheckedChanged);
			// 
			// grpAudioVACGain
			// 
			this.grpAudioVACGain.Controls.Add(this.lblAudioVACGainTX);
			this.grpAudioVACGain.Controls.Add(this.udAudioVACGainTX);
			this.grpAudioVACGain.Controls.Add(this.lblAudioVACGainRX);
			this.grpAudioVACGain.Controls.Add(this.udAudioVACGainRX);
			this.grpAudioVACGain.Location = new System.Drawing.Point(344, 8);
			this.grpAudioVACGain.Name = "grpAudioVACGain";
			this.grpAudioVACGain.Size = new System.Drawing.Size(96, 80);
			this.grpAudioVACGain.TabIndex = 72;
			this.grpAudioVACGain.TabStop = false;
			this.grpAudioVACGain.Text = "Gain (dB)";
			// 
			// lblAudioVACGainTX
			// 
			this.lblAudioVACGainTX.Image = null;
			this.lblAudioVACGainTX.Location = new System.Drawing.Point(16, 48);
			this.lblAudioVACGainTX.Name = "lblAudioVACGainTX";
			this.lblAudioVACGainTX.Size = new System.Drawing.Size(32, 16);
			this.lblAudioVACGainTX.TabIndex = 39;
			this.lblAudioVACGainTX.Text = "TX:";
			// 
			// udAudioVACGainTX
			// 
			this.udAudioVACGainTX.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   0});
			this.udAudioVACGainTX.Location = new System.Drawing.Point(48, 48);
			this.udAudioVACGainTX.Maximum = new System.Decimal(new int[] {
																			 20,
																			 0,
																			 0,
																			 0});
			this.udAudioVACGainTX.Minimum = new System.Decimal(new int[] {
																			 40,
																			 0,
																			 0,
																			 -2147483648});
			this.udAudioVACGainTX.Name = "udAudioVACGainTX";
			this.udAudioVACGainTX.Size = new System.Drawing.Size(40, 20);
			this.udAudioVACGainTX.TabIndex = 38;
			this.toolTip1.SetToolTip(this.udAudioVACGainTX, "Controls the gain on the audio coming from third party applications.");
			this.udAudioVACGainTX.Value = new System.Decimal(new int[] {
																		   0,
																		   0,
																		   0,
																		   0});
			this.udAudioVACGainTX.LostFocus += new System.EventHandler(this.udAudioVACGainTX_LostFocus);
			this.udAudioVACGainTX.ValueChanged += new System.EventHandler(this.udAudioVACGainTX_ValueChanged);
			// 
			// lblAudioVACGainRX
			// 
			this.lblAudioVACGainRX.Image = null;
			this.lblAudioVACGainRX.Location = new System.Drawing.Point(16, 24);
			this.lblAudioVACGainRX.Name = "lblAudioVACGainRX";
			this.lblAudioVACGainRX.Size = new System.Drawing.Size(24, 16);
			this.lblAudioVACGainRX.TabIndex = 37;
			this.lblAudioVACGainRX.Text = "RX:";
			// 
			// udAudioVACGainRX
			// 
			this.udAudioVACGainRX.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   0});
			this.udAudioVACGainRX.Location = new System.Drawing.Point(48, 24);
			this.udAudioVACGainRX.Maximum = new System.Decimal(new int[] {
																			 20,
																			 0,
																			 0,
																			 0});
			this.udAudioVACGainRX.Minimum = new System.Decimal(new int[] {
																			 40,
																			 0,
																			 0,
																			 -2147483648});
			this.udAudioVACGainRX.Name = "udAudioVACGainRX";
			this.udAudioVACGainRX.Size = new System.Drawing.Size(40, 20);
			this.udAudioVACGainRX.TabIndex = 36;
			this.toolTip1.SetToolTip(this.udAudioVACGainRX, "Controls the gain applied to the RX audio before it is sent to the third party ap" +
				"plication.");
			this.udAudioVACGainRX.Value = new System.Decimal(new int[] {
																		   0,
																		   0,
																		   0,
																		   0});
			this.udAudioVACGainRX.LostFocus += new System.EventHandler(this.udAudioVACGainRX_LostFocus);
			this.udAudioVACGainRX.ValueChanged += new System.EventHandler(this.udAudioVACGainRX_ValueChanged);
			// 
			// grpAudio2Stereo
			// 
			this.grpAudio2Stereo.Controls.Add(this.chkAudio2Stereo);
			this.grpAudio2Stereo.Location = new System.Drawing.Point(240, 136);
			this.grpAudio2Stereo.Name = "grpAudio2Stereo";
			this.grpAudio2Stereo.Size = new System.Drawing.Size(96, 56);
			this.grpAudio2Stereo.TabIndex = 71;
			this.grpAudio2Stereo.TabStop = false;
			this.grpAudio2Stereo.Text = "Mono/Stereo";
			// 
			// chkAudio2Stereo
			// 
			this.chkAudio2Stereo.Image = null;
			this.chkAudio2Stereo.Location = new System.Drawing.Point(16, 24);
			this.chkAudio2Stereo.Name = "chkAudio2Stereo";
			this.chkAudio2Stereo.Size = new System.Drawing.Size(64, 16);
			this.chkAudio2Stereo.TabIndex = 0;
			this.chkAudio2Stereo.Text = "Stereo";
			this.toolTip1.SetToolTip(this.chkAudio2Stereo, "Click this button if the third party software will open the Virtual Audio Cable i" +
				"n 2 channel (stereo) mode.");
			this.chkAudio2Stereo.CheckedChanged += new System.EventHandler(this.chkAudio2Stereo_CheckedChanged);
			// 
			// grpAudioLatency2
			// 
			this.grpAudioLatency2.Controls.Add(this.chkAudioLatencyManual2);
			this.grpAudioLatency2.Controls.Add(this.udAudioLatency2);
			this.grpAudioLatency2.Location = new System.Drawing.Point(344, 112);
			this.grpAudioLatency2.Name = "grpAudioLatency2";
			this.grpAudioLatency2.Size = new System.Drawing.Size(96, 80);
			this.grpAudioLatency2.TabIndex = 67;
			this.grpAudioLatency2.TabStop = false;
			this.grpAudioLatency2.Text = "Latency (ms)";
			this.grpAudioLatency2.Visible = false;
			// 
			// chkAudioLatencyManual2
			// 
			this.chkAudioLatencyManual2.Image = null;
			this.chkAudioLatencyManual2.Location = new System.Drawing.Point(16, 24);
			this.chkAudioLatencyManual2.Name = "chkAudioLatencyManual2";
			this.chkAudioLatencyManual2.Size = new System.Drawing.Size(64, 16);
			this.chkAudioLatencyManual2.TabIndex = 5;
			this.chkAudioLatencyManual2.Text = "Manual";
			this.chkAudioLatencyManual2.CheckedChanged += new System.EventHandler(this.chkAudioLatencyManual2_CheckedChanged);
			// 
			// udAudioLatency2
			// 
			this.udAudioLatency2.Increment = new System.Decimal(new int[] {
																			  1,
																			  0,
																			  0,
																			  0});
			this.udAudioLatency2.Location = new System.Drawing.Point(16, 48);
			this.udAudioLatency2.Maximum = new System.Decimal(new int[] {
																			240,
																			0,
																			0,
																			0});
			this.udAudioLatency2.Minimum = new System.Decimal(new int[] {
																			0,
																			0,
																			0,
																			0});
			this.udAudioLatency2.Name = "udAudioLatency2";
			this.udAudioLatency2.Size = new System.Drawing.Size(48, 20);
			this.udAudioLatency2.TabIndex = 36;
			this.udAudioLatency2.Value = new System.Decimal(new int[] {
																		  120,
																		  0,
																		  0,
																		  0});
			this.udAudioLatency2.LostFocus += new System.EventHandler(this.udAudioLatency2_LostFocus);
			this.udAudioLatency2.ValueChanged += new System.EventHandler(this.udAudioLatency2_ValueChanged);
			// 
			// grpAudioSampleRate2
			// 
			this.grpAudioSampleRate2.Controls.Add(this.comboAudioSampleRate2);
			this.grpAudioSampleRate2.Location = new System.Drawing.Point(240, 72);
			this.grpAudioSampleRate2.Name = "grpAudioSampleRate2";
			this.grpAudioSampleRate2.Size = new System.Drawing.Size(96, 56);
			this.grpAudioSampleRate2.TabIndex = 66;
			this.grpAudioSampleRate2.TabStop = false;
			this.grpAudioSampleRate2.Text = "Sample Rate";
			// 
			// comboAudioSampleRate2
			// 
			this.comboAudioSampleRate2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAudioSampleRate2.DropDownWidth = 64;
			this.comboAudioSampleRate2.Items.AddRange(new object[] {
																	   "6000",
																	   "8000",
																	   "11025",
																	   "12000",
																	   "24000",
																	   "22050",
																	   "44100",
																	   "48000"});
			this.comboAudioSampleRate2.Location = new System.Drawing.Point(16, 24);
			this.comboAudioSampleRate2.Name = "comboAudioSampleRate2";
			this.comboAudioSampleRate2.Size = new System.Drawing.Size(64, 21);
			this.comboAudioSampleRate2.TabIndex = 60;
			this.toolTip1.SetToolTip(this.comboAudioSampleRate2, "Samples per second.  Set to match the third party software program.");
			this.comboAudioSampleRate2.SelectedIndexChanged += new System.EventHandler(this.comboAudioSampleRate2_SelectedIndexChanged);
			// 
			// grpAudioBuffer2
			// 
			this.grpAudioBuffer2.Controls.Add(this.comboAudioBuffer2);
			this.grpAudioBuffer2.Location = new System.Drawing.Point(240, 8);
			this.grpAudioBuffer2.Name = "grpAudioBuffer2";
			this.grpAudioBuffer2.Size = new System.Drawing.Size(96, 56);
			this.grpAudioBuffer2.TabIndex = 65;
			this.grpAudioBuffer2.TabStop = false;
			this.grpAudioBuffer2.Text = "Buffer Size";
			// 
			// comboAudioBuffer2
			// 
			this.comboAudioBuffer2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAudioBuffer2.DropDownWidth = 56;
			this.comboAudioBuffer2.Items.AddRange(new object[] {
																   "512",
																   "1024",
																   "2048"});
			this.comboAudioBuffer2.Location = new System.Drawing.Point(16, 24);
			this.comboAudioBuffer2.Name = "comboAudioBuffer2";
			this.comboAudioBuffer2.Size = new System.Drawing.Size(56, 21);
			this.comboAudioBuffer2.TabIndex = 58;
			this.toolTip1.SetToolTip(this.comboAudioBuffer2, "Samples per buffer.");
			this.comboAudioBuffer2.SelectedIndexChanged += new System.EventHandler(this.comboAudioBuffer2_SelectedIndexChanged);
			// 
			// grpAudioDetails2
			// 
			this.grpAudioDetails2.Controls.Add(this.lblAudioOutput2);
			this.grpAudioDetails2.Controls.Add(this.comboAudioOutput2);
			this.grpAudioDetails2.Controls.Add(this.lblAudioInput2);
			this.grpAudioDetails2.Controls.Add(this.lblAudioDriver2);
			this.grpAudioDetails2.Controls.Add(this.comboAudioInput2);
			this.grpAudioDetails2.Controls.Add(this.comboAudioDriver2);
			this.grpAudioDetails2.Location = new System.Drawing.Point(8, 40);
			this.grpAudioDetails2.Name = "grpAudioDetails2";
			this.grpAudioDetails2.Size = new System.Drawing.Size(224, 120);
			this.grpAudioDetails2.TabIndex = 35;
			this.grpAudioDetails2.TabStop = false;
			this.grpAudioDetails2.Text = "Virtual Audio Cable Setup";
			// 
			// lblAudioOutput2
			// 
			this.lblAudioOutput2.Image = null;
			this.lblAudioOutput2.Location = new System.Drawing.Point(8, 88);
			this.lblAudioOutput2.Name = "lblAudioOutput2";
			this.lblAudioOutput2.Size = new System.Drawing.Size(48, 16);
			this.lblAudioOutput2.TabIndex = 35;
			this.lblAudioOutput2.Text = "Output:";
			// 
			// comboAudioOutput2
			// 
			this.comboAudioOutput2.DisplayMember = "sdfg";
			this.comboAudioOutput2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAudioOutput2.DropDownWidth = 160;
			this.comboAudioOutput2.ItemHeight = 13;
			this.comboAudioOutput2.Location = new System.Drawing.Point(56, 88);
			this.comboAudioOutput2.Name = "comboAudioOutput2";
			this.comboAudioOutput2.Size = new System.Drawing.Size(160, 21);
			this.comboAudioOutput2.TabIndex = 34;
			this.toolTip1.SetToolTip(this.comboAudioOutput2, "Output Audio Device");
			this.comboAudioOutput2.SelectedIndexChanged += new System.EventHandler(this.comboAudioOutput2_SelectedIndexChanged);
			// 
			// lblAudioInput2
			// 
			this.lblAudioInput2.Image = null;
			this.lblAudioInput2.Location = new System.Drawing.Point(8, 56);
			this.lblAudioInput2.Name = "lblAudioInput2";
			this.lblAudioInput2.Size = new System.Drawing.Size(40, 16);
			this.lblAudioInput2.TabIndex = 33;
			this.lblAudioInput2.Text = "Input:";
			// 
			// lblAudioDriver2
			// 
			this.lblAudioDriver2.Image = null;
			this.lblAudioDriver2.Location = new System.Drawing.Point(8, 24);
			this.lblAudioDriver2.Name = "lblAudioDriver2";
			this.lblAudioDriver2.Size = new System.Drawing.Size(40, 16);
			this.lblAudioDriver2.TabIndex = 32;
			this.lblAudioDriver2.Text = "Driver:";
			// 
			// comboAudioInput2
			// 
			this.comboAudioInput2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAudioInput2.DropDownWidth = 160;
			this.comboAudioInput2.ItemHeight = 13;
			this.comboAudioInput2.Location = new System.Drawing.Point(56, 56);
			this.comboAudioInput2.Name = "comboAudioInput2";
			this.comboAudioInput2.Size = new System.Drawing.Size(160, 21);
			this.comboAudioInput2.TabIndex = 28;
			this.toolTip1.SetToolTip(this.comboAudioInput2, "Input Audio Device");
			this.comboAudioInput2.SelectedIndexChanged += new System.EventHandler(this.comboAudioInput2_SelectedIndexChanged);
			// 
			// comboAudioDriver2
			// 
			this.comboAudioDriver2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAudioDriver2.DropDownWidth = 160;
			this.comboAudioDriver2.ItemHeight = 13;
			this.comboAudioDriver2.Location = new System.Drawing.Point(56, 24);
			this.comboAudioDriver2.Name = "comboAudioDriver2";
			this.comboAudioDriver2.Size = new System.Drawing.Size(160, 21);
			this.comboAudioDriver2.TabIndex = 26;
			this.toolTip1.SetToolTip(this.comboAudioDriver2, "Sound Card Driver Selection");
			this.comboAudioDriver2.SelectedIndexChanged += new System.EventHandler(this.comboAudioDriver2_SelectedIndexChanged);
			// 
			// chkAudioEnableVAC
			// 
			this.chkAudioEnableVAC.Image = null;
			this.chkAudioEnableVAC.Location = new System.Drawing.Point(16, 8);
			this.chkAudioEnableVAC.Name = "chkAudioEnableVAC";
			this.chkAudioEnableVAC.Size = new System.Drawing.Size(88, 24);
			this.chkAudioEnableVAC.TabIndex = 25;
			this.chkAudioEnableVAC.Text = "Enable VAC";
			this.toolTip1.SetToolTip(this.chkAudioEnableVAC, "Enable Virtual Audio Cable Support using the settings on this form.");
			this.chkAudioEnableVAC.CheckedChanged += new System.EventHandler(this.chkAudioEnableVAC_CheckedChanged);
			// 
			// tpDisplay
			// 
			this.tpDisplay.Controls.Add(this.grpDisplayDriverEngine);
			this.tpDisplay.Controls.Add(this.grpDisplayPolyPhase);
			this.tpDisplay.Controls.Add(this.grpDisplayScopeMode);
			this.tpDisplay.Controls.Add(this.grpDisplayMultimeter);
			this.tpDisplay.Controls.Add(this.grpDisplayWaterfall);
			this.tpDisplay.Controls.Add(this.grpDisplayRefreshRates);
			this.tpDisplay.Controls.Add(this.grpDisplayAverage);
			this.tpDisplay.Controls.Add(this.grpDisplayPhase);
			this.tpDisplay.Controls.Add(this.grpDisplaySpectrumGrid);
			this.tpDisplay.Location = new System.Drawing.Point(4, 22);
			this.tpDisplay.Name = "tpDisplay";
			this.tpDisplay.Size = new System.Drawing.Size(584, 286);
			this.tpDisplay.TabIndex = 2;
			this.tpDisplay.Text = "Display";
			// 
			// grpDisplayDriverEngine
			// 
			this.grpDisplayDriverEngine.Controls.Add(this.comboDisplayDriver);
			this.grpDisplayDriverEngine.Location = new System.Drawing.Point(480, 144);
			this.grpDisplayDriverEngine.Name = "grpDisplayDriverEngine";
			this.grpDisplayDriverEngine.Size = new System.Drawing.Size(96, 56);
			this.grpDisplayDriverEngine.TabIndex = 46;
			this.grpDisplayDriverEngine.TabStop = false;
			this.grpDisplayDriverEngine.Text = "Driver Engine";
			this.grpDisplayDriverEngine.Visible = false;
			// 
			// comboDisplayDriver
			// 
			this.comboDisplayDriver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDisplayDriver.DropDownWidth = 48;
			this.comboDisplayDriver.Items.AddRange(new object[] {
																	"GDI+"});
			this.comboDisplayDriver.Location = new System.Drawing.Point(8, 24);
			this.comboDisplayDriver.Name = "comboDisplayDriver";
			this.comboDisplayDriver.Size = new System.Drawing.Size(80, 21);
			this.comboDisplayDriver.TabIndex = 45;
			this.toolTip1.SetToolTip(this.comboDisplayDriver, "Sets the driver to be used for the display.");
			this.comboDisplayDriver.SelectedIndexChanged += new System.EventHandler(this.comboDisplayDriver_SelectedIndexChanged);
			// 
			// grpDisplayPolyPhase
			// 
			this.grpDisplayPolyPhase.Controls.Add(this.chkSpectrumPolyphase);
			this.grpDisplayPolyPhase.Location = new System.Drawing.Point(440, 72);
			this.grpDisplayPolyPhase.Name = "grpDisplayPolyPhase";
			this.grpDisplayPolyPhase.Size = new System.Drawing.Size(120, 56);
			this.grpDisplayPolyPhase.TabIndex = 44;
			this.grpDisplayPolyPhase.TabStop = false;
			this.grpDisplayPolyPhase.Text = "Polyphase FFT";
			// 
			// chkSpectrumPolyphase
			// 
			this.chkSpectrumPolyphase.Image = null;
			this.chkSpectrumPolyphase.Location = new System.Drawing.Point(16, 24);
			this.chkSpectrumPolyphase.Name = "chkSpectrumPolyphase";
			this.chkSpectrumPolyphase.Size = new System.Drawing.Size(64, 16);
			this.chkSpectrumPolyphase.TabIndex = 39;
			this.chkSpectrumPolyphase.Text = "Enable";
			this.toolTip1.SetToolTip(this.chkSpectrumPolyphase, "Check to enable polyphase spectrum display mode.  While adding latency, this adds" +
				" resolution to the display.");
			this.chkSpectrumPolyphase.CheckedChanged += new System.EventHandler(this.chkSpectrumPolyphase_CheckedChanged);
			// 
			// grpDisplayScopeMode
			// 
			this.grpDisplayScopeMode.Controls.Add(this.udDisplayScopeTime);
			this.grpDisplayScopeMode.Controls.Add(this.lblDisplayScopeTime);
			this.grpDisplayScopeMode.Location = new System.Drawing.Point(440, 8);
			this.grpDisplayScopeMode.Name = "grpDisplayScopeMode";
			this.grpDisplayScopeMode.Size = new System.Drawing.Size(120, 56);
			this.grpDisplayScopeMode.TabIndex = 43;
			this.grpDisplayScopeMode.TabStop = false;
			this.grpDisplayScopeMode.Text = "Scope Mode";
			// 
			// udDisplayScopeTime
			// 
			this.udDisplayScopeTime.Increment = new System.Decimal(new int[] {
																				 1,
																				 0,
																				 0,
																				 0});
			this.udDisplayScopeTime.Location = new System.Drawing.Point(64, 24);
			this.udDisplayScopeTime.Maximum = new System.Decimal(new int[] {
																			   85,
																			   0,
																			   0,
																			   0});
			this.udDisplayScopeTime.Minimum = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   0});
			this.udDisplayScopeTime.Name = "udDisplayScopeTime";
			this.udDisplayScopeTime.Size = new System.Drawing.Size(48, 20);
			this.udDisplayScopeTime.TabIndex = 0;
			this.toolTip1.SetToolTip(this.udDisplayScopeTime, "Amount of time to display across the width of the scope display window.");
			this.udDisplayScopeTime.Value = new System.Decimal(new int[] {
																			 50,
																			 0,
																			 0,
																			 0});
			this.udDisplayScopeTime.LostFocus += new System.EventHandler(this.udDisplayScopeTime_LostFocus);
			this.udDisplayScopeTime.ValueChanged += new System.EventHandler(this.udDisplayScopeTime_ValueChanged);
			// 
			// lblDisplayScopeTime
			// 
			this.lblDisplayScopeTime.Image = null;
			this.lblDisplayScopeTime.Location = new System.Drawing.Point(8, 24);
			this.lblDisplayScopeTime.Name = "lblDisplayScopeTime";
			this.lblDisplayScopeTime.Size = new System.Drawing.Size(64, 23);
			this.lblDisplayScopeTime.TabIndex = 1;
			this.lblDisplayScopeTime.Text = "Time (ms):";
			// 
			// grpDisplayMultimeter
			// 
			this.grpDisplayMultimeter.Controls.Add(this.udDisplayMeterAvg);
			this.grpDisplayMultimeter.Controls.Add(this.lblDisplayMeterAvg);
			this.grpDisplayMultimeter.Controls.Add(this.udDisplayMultiTextHoldTime);
			this.grpDisplayMultimeter.Controls.Add(this.lblDisplayMeterTextHoldTime);
			this.grpDisplayMultimeter.Controls.Add(this.udDisplayMultiPeakHoldTime);
			this.grpDisplayMultimeter.Controls.Add(this.lblDisplayMultiPeakHoldTime);
			this.grpDisplayMultimeter.Location = new System.Drawing.Point(272, 144);
			this.grpDisplayMultimeter.Name = "grpDisplayMultimeter";
			this.grpDisplayMultimeter.Size = new System.Drawing.Size(200, 136);
			this.grpDisplayMultimeter.TabIndex = 41;
			this.grpDisplayMultimeter.TabStop = false;
			this.grpDisplayMultimeter.Text = "Multimeter";
			// 
			// udDisplayMeterAvg
			// 
			this.udDisplayMeterAvg.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			this.udDisplayMeterAvg.Location = new System.Drawing.Point(136, 72);
			this.udDisplayMeterAvg.Maximum = new System.Decimal(new int[] {
																			  99999,
																			  0,
																			  0,
																			  0});
			this.udDisplayMeterAvg.Minimum = new System.Decimal(new int[] {
																			  1,
																			  0,
																			  0,
																			  0});
			this.udDisplayMeterAvg.Name = "udDisplayMeterAvg";
			this.udDisplayMeterAvg.Size = new System.Drawing.Size(56, 20);
			this.udDisplayMeterAvg.TabIndex = 8;
			this.toolTip1.SetToolTip(this.udDisplayMeterAvg, "Controls the length of time to average for the meter.");
			this.udDisplayMeterAvg.Value = new System.Decimal(new int[] {
																			1000,
																			0,
																			0,
																			0});
			this.udDisplayMeterAvg.LostFocus += new System.EventHandler(this.udDisplayMeterAvg_LostFocus);
			this.udDisplayMeterAvg.ValueChanged += new System.EventHandler(this.udDisplayMeterAvg_ValueChanged);
			// 
			// lblDisplayMeterAvg
			// 
			this.lblDisplayMeterAvg.Image = null;
			this.lblDisplayMeterAvg.Location = new System.Drawing.Point(16, 72);
			this.lblDisplayMeterAvg.Name = "lblDisplayMeterAvg";
			this.lblDisplayMeterAvg.Size = new System.Drawing.Size(112, 16);
			this.lblDisplayMeterAvg.TabIndex = 7;
			this.lblDisplayMeterAvg.Text = "Average Time (ms):";
			// 
			// udDisplayMultiTextHoldTime
			// 
			this.udDisplayMultiTextHoldTime.Increment = new System.Decimal(new int[] {
																						 1,
																						 0,
																						 0,
																						 0});
			this.udDisplayMultiTextHoldTime.Location = new System.Drawing.Point(136, 48);
			this.udDisplayMultiTextHoldTime.Maximum = new System.Decimal(new int[] {
																					   99999,
																					   0,
																					   0,
																					   0});
			this.udDisplayMultiTextHoldTime.Minimum = new System.Decimal(new int[] {
																					   1,
																					   0,
																					   0,
																					   0});
			this.udDisplayMultiTextHoldTime.Name = "udDisplayMultiTextHoldTime";
			this.udDisplayMultiTextHoldTime.Size = new System.Drawing.Size(56, 20);
			this.udDisplayMultiTextHoldTime.TabIndex = 4;
			this.toolTip1.SetToolTip(this.udDisplayMultiTextHoldTime, "Controls how long the meter will hold the digital peak value when in the Peak Pow" +
				"er mode.");
			this.udDisplayMultiTextHoldTime.Value = new System.Decimal(new int[] {
																					 500,
																					 0,
																					 0,
																					 0});
			this.udDisplayMultiTextHoldTime.LostFocus += new System.EventHandler(this.udDisplayMultiTextHoldTime_LostFocus);
			this.udDisplayMultiTextHoldTime.ValueChanged += new System.EventHandler(this.udDisplayMultiTextHoldTime_ValueChanged);
			// 
			// lblDisplayMeterTextHoldTime
			// 
			this.lblDisplayMeterTextHoldTime.Image = null;
			this.lblDisplayMeterTextHoldTime.Location = new System.Drawing.Point(16, 48);
			this.lblDisplayMeterTextHoldTime.Name = "lblDisplayMeterTextHoldTime";
			this.lblDisplayMeterTextHoldTime.Size = new System.Drawing.Size(120, 16);
			this.lblDisplayMeterTextHoldTime.TabIndex = 3;
			this.lblDisplayMeterTextHoldTime.Text = "Digital Peak Hold (ms):";
			// 
			// udDisplayMultiPeakHoldTime
			// 
			this.udDisplayMultiPeakHoldTime.Increment = new System.Decimal(new int[] {
																						 1,
																						 0,
																						 0,
																						 0});
			this.udDisplayMultiPeakHoldTime.Location = new System.Drawing.Point(136, 24);
			this.udDisplayMultiPeakHoldTime.Maximum = new System.Decimal(new int[] {
																					   99999,
																					   0,
																					   0,
																					   0});
			this.udDisplayMultiPeakHoldTime.Minimum = new System.Decimal(new int[] {
																					   0,
																					   0,
																					   0,
																					   0});
			this.udDisplayMultiPeakHoldTime.Name = "udDisplayMultiPeakHoldTime";
			this.udDisplayMultiPeakHoldTime.Size = new System.Drawing.Size(56, 20);
			this.udDisplayMultiPeakHoldTime.TabIndex = 1;
			this.toolTip1.SetToolTip(this.udDisplayMultiPeakHoldTime, "Controls how long the analog peak red line will be held on the multimeter.");
			this.udDisplayMultiPeakHoldTime.Value = new System.Decimal(new int[] {
																					 1000,
																					 0,
																					 0,
																					 0});
			this.udDisplayMultiPeakHoldTime.LostFocus += new System.EventHandler(this.udDisplayMultiPeakHoldTime_LostFocus);
			this.udDisplayMultiPeakHoldTime.ValueChanged += new System.EventHandler(this.udDisplayMultiPeakHoldTime_ValueChanged);
			// 
			// lblDisplayMultiPeakHoldTime
			// 
			this.lblDisplayMultiPeakHoldTime.Image = null;
			this.lblDisplayMultiPeakHoldTime.Location = new System.Drawing.Point(16, 24);
			this.lblDisplayMultiPeakHoldTime.Name = "lblDisplayMultiPeakHoldTime";
			this.lblDisplayMultiPeakHoldTime.Size = new System.Drawing.Size(128, 16);
			this.lblDisplayMultiPeakHoldTime.TabIndex = 0;
			this.lblDisplayMultiPeakHoldTime.Text = "Analog Peak Hold (ms):";
			// 
			// grpDisplayWaterfall
			// 
			this.grpDisplayWaterfall.Controls.Add(this.clrbtnWaterfallMid);
			this.grpDisplayWaterfall.Controls.Add(this.clrbtnWaterfallHigh);
			this.grpDisplayWaterfall.Controls.Add(this.clrbtnWaterfallLow);
			this.grpDisplayWaterfall.Controls.Add(this.lblDisplayWaterfallMidColor);
			this.grpDisplayWaterfall.Controls.Add(this.lblDisplayWaterfallHighColor);
			this.grpDisplayWaterfall.Controls.Add(this.lblDisplayWaterfallLowColor);
			this.grpDisplayWaterfall.Controls.Add(this.lblDisplayWaterfallLowLevel);
			this.grpDisplayWaterfall.Controls.Add(this.udDisplayWaterfallLowLevel);
			this.grpDisplayWaterfall.Controls.Add(this.lblDisplayWaterfallHighLevel);
			this.grpDisplayWaterfall.Controls.Add(this.udDisplayWaterfallHighLevel);
			this.grpDisplayWaterfall.Location = new System.Drawing.Point(8, 144);
			this.grpDisplayWaterfall.Name = "grpDisplayWaterfall";
			this.grpDisplayWaterfall.Size = new System.Drawing.Size(256, 136);
			this.grpDisplayWaterfall.TabIndex = 40;
			this.grpDisplayWaterfall.TabStop = false;
			this.grpDisplayWaterfall.Text = "Waterfall";
			// 
			// clrbtnWaterfallMid
			// 
			this.clrbtnWaterfallMid.Automatic = "Automatic";
			this.clrbtnWaterfallMid.Color = System.Drawing.Color.Transparent;
			this.clrbtnWaterfallMid.Image = null;
			this.clrbtnWaterfallMid.Location = new System.Drawing.Point(72, 88);
			this.clrbtnWaterfallMid.MoreColors = "More Colors...";
			this.clrbtnWaterfallMid.Name = "clrbtnWaterfallMid";
			this.clrbtnWaterfallMid.Size = new System.Drawing.Size(40, 23);
			this.clrbtnWaterfallMid.TabIndex = 70;
			this.toolTip1.SetToolTip(this.clrbtnWaterfallMid, "The mid color used when the signal level is exactly between the low and high leve" +
				"ls shown above.  A gradient is used if it is not eactly in the middle.");
			this.clrbtnWaterfallMid.Changed += new System.EventHandler(this.clrbtnWaterfallMid_Changed);
			// 
			// clrbtnWaterfallHigh
			// 
			this.clrbtnWaterfallHigh.Automatic = "Automatic";
			this.clrbtnWaterfallHigh.Color = System.Drawing.Color.Transparent;
			this.clrbtnWaterfallHigh.Image = null;
			this.clrbtnWaterfallHigh.Location = new System.Drawing.Point(192, 56);
			this.clrbtnWaterfallHigh.MoreColors = "More Colors...";
			this.clrbtnWaterfallHigh.Name = "clrbtnWaterfallHigh";
			this.clrbtnWaterfallHigh.Size = new System.Drawing.Size(40, 23);
			this.clrbtnWaterfallHigh.TabIndex = 69;
			this.toolTip1.SetToolTip(this.clrbtnWaterfallHigh, "The color to display when the signal level is at or above the high level shown ab" +
				"ove.");
			this.clrbtnWaterfallHigh.Changed += new System.EventHandler(this.clrbtnWaterfallHigh_Changed);
			// 
			// clrbtnWaterfallLow
			// 
			this.clrbtnWaterfallLow.Automatic = "Automatic";
			this.clrbtnWaterfallLow.Color = System.Drawing.Color.Transparent;
			this.clrbtnWaterfallLow.Image = null;
			this.clrbtnWaterfallLow.Location = new System.Drawing.Point(72, 56);
			this.clrbtnWaterfallLow.MoreColors = "More Colors...";
			this.clrbtnWaterfallLow.Name = "clrbtnWaterfallLow";
			this.clrbtnWaterfallLow.Size = new System.Drawing.Size(40, 23);
			this.clrbtnWaterfallLow.TabIndex = 68;
			this.toolTip1.SetToolTip(this.clrbtnWaterfallLow, "The Color to use when the signal level is at or below the low level set above.");
			this.clrbtnWaterfallLow.Changed += new System.EventHandler(this.clrbtnWaterfallLow_Changed);
			// 
			// lblDisplayWaterfallMidColor
			// 
			this.lblDisplayWaterfallMidColor.Image = null;
			this.lblDisplayWaterfallMidColor.Location = new System.Drawing.Point(8, 88);
			this.lblDisplayWaterfallMidColor.Name = "lblDisplayWaterfallMidColor";
			this.lblDisplayWaterfallMidColor.Size = new System.Drawing.Size(64, 16);
			this.lblDisplayWaterfallMidColor.TabIndex = 61;
			this.lblDisplayWaterfallMidColor.Text = "Mid Color:";
			// 
			// lblDisplayWaterfallHighColor
			// 
			this.lblDisplayWaterfallHighColor.Image = null;
			this.lblDisplayWaterfallHighColor.Location = new System.Drawing.Point(128, 56);
			this.lblDisplayWaterfallHighColor.Name = "lblDisplayWaterfallHighColor";
			this.lblDisplayWaterfallHighColor.Size = new System.Drawing.Size(64, 16);
			this.lblDisplayWaterfallHighColor.TabIndex = 59;
			this.lblDisplayWaterfallHighColor.Text = "High Color:";
			// 
			// lblDisplayWaterfallLowColor
			// 
			this.lblDisplayWaterfallLowColor.Image = null;
			this.lblDisplayWaterfallLowColor.Location = new System.Drawing.Point(8, 56);
			this.lblDisplayWaterfallLowColor.Name = "lblDisplayWaterfallLowColor";
			this.lblDisplayWaterfallLowColor.Size = new System.Drawing.Size(64, 16);
			this.lblDisplayWaterfallLowColor.TabIndex = 57;
			this.lblDisplayWaterfallLowColor.Text = "Low Color:";
			// 
			// lblDisplayWaterfallLowLevel
			// 
			this.lblDisplayWaterfallLowLevel.Image = null;
			this.lblDisplayWaterfallLowLevel.Location = new System.Drawing.Point(8, 24);
			this.lblDisplayWaterfallLowLevel.Name = "lblDisplayWaterfallLowLevel";
			this.lblDisplayWaterfallLowLevel.Size = new System.Drawing.Size(64, 23);
			this.lblDisplayWaterfallLowLevel.TabIndex = 3;
			this.lblDisplayWaterfallLowLevel.Text = "Low Level";
			// 
			// udDisplayWaterfallLowLevel
			// 
			this.udDisplayWaterfallLowLevel.Increment = new System.Decimal(new int[] {
																						 10,
																						 0,
																						 0,
																						 0});
			this.udDisplayWaterfallLowLevel.Location = new System.Drawing.Point(72, 24);
			this.udDisplayWaterfallLowLevel.Maximum = new System.Decimal(new int[] {
																					   200,
																					   0,
																					   0,
																					   0});
			this.udDisplayWaterfallLowLevel.Minimum = new System.Decimal(new int[] {
																					   200,
																					   0,
																					   0,
																					   -2147483648});
			this.udDisplayWaterfallLowLevel.Name = "udDisplayWaterfallLowLevel";
			this.udDisplayWaterfallLowLevel.Size = new System.Drawing.Size(48, 20);
			this.udDisplayWaterfallLowLevel.TabIndex = 2;
			this.toolTip1.SetToolTip(this.udDisplayWaterfallLowLevel, "Waterfall Low Signal - Show Low Color below this value (gradient in between).");
			this.udDisplayWaterfallLowLevel.Value = new System.Decimal(new int[] {
																					 110,
																					 0,
																					 0,
																					 -2147483648});
			this.udDisplayWaterfallLowLevel.LostFocus += new System.EventHandler(this.udDisplayWaterfallLowLevel_LostFocus);
			this.udDisplayWaterfallLowLevel.ValueChanged += new System.EventHandler(this.udDisplayWaterfallLowLevel_ValueChanged);
			// 
			// lblDisplayWaterfallHighLevel
			// 
			this.lblDisplayWaterfallHighLevel.Image = null;
			this.lblDisplayWaterfallHighLevel.Location = new System.Drawing.Point(128, 24);
			this.lblDisplayWaterfallHighLevel.Name = "lblDisplayWaterfallHighLevel";
			this.lblDisplayWaterfallHighLevel.Size = new System.Drawing.Size(64, 23);
			this.lblDisplayWaterfallHighLevel.TabIndex = 1;
			this.lblDisplayWaterfallHighLevel.Text = "High Level";
			// 
			// udDisplayWaterfallHighLevel
			// 
			this.udDisplayWaterfallHighLevel.Increment = new System.Decimal(new int[] {
																						  10,
																						  0,
																						  0,
																						  0});
			this.udDisplayWaterfallHighLevel.Location = new System.Drawing.Point(192, 24);
			this.udDisplayWaterfallHighLevel.Maximum = new System.Decimal(new int[] {
																						200,
																						0,
																						0,
																						0});
			this.udDisplayWaterfallHighLevel.Minimum = new System.Decimal(new int[] {
																						200,
																						0,
																						0,
																						-2147483648});
			this.udDisplayWaterfallHighLevel.Name = "udDisplayWaterfallHighLevel";
			this.udDisplayWaterfallHighLevel.Size = new System.Drawing.Size(48, 20);
			this.udDisplayWaterfallHighLevel.TabIndex = 0;
			this.toolTip1.SetToolTip(this.udDisplayWaterfallHighLevel, "Waterfall High Signal - Show High Color above this value (gradient in between).");
			this.udDisplayWaterfallHighLevel.Value = new System.Decimal(new int[] {
																					  70,
																					  0,
																					  0,
																					  -2147483648});
			this.udDisplayWaterfallHighLevel.LostFocus += new System.EventHandler(this.udDisplayWaterfallHighLevel_LostFocus);
			this.udDisplayWaterfallHighLevel.ValueChanged += new System.EventHandler(this.udDisplayWaterfallHighLevel_ValueChanged);
			// 
			// grpDisplayRefreshRates
			// 
			this.grpDisplayRefreshRates.Controls.Add(this.udDisplayCPUMeter);
			this.grpDisplayRefreshRates.Controls.Add(this.lblDisplayCPUMeter);
			this.grpDisplayRefreshRates.Controls.Add(this.udDisplayPeakText);
			this.grpDisplayRefreshRates.Controls.Add(this.lblDisplayPeakText);
			this.grpDisplayRefreshRates.Controls.Add(this.udDisplayMeterDelay);
			this.grpDisplayRefreshRates.Controls.Add(this.lblDisplayMeterDelay);
			this.grpDisplayRefreshRates.Controls.Add(this.udDisplayFPS);
			this.grpDisplayRefreshRates.Controls.Add(this.lblDisplayFPS);
			this.grpDisplayRefreshRates.Location = new System.Drawing.Point(128, 8);
			this.grpDisplayRefreshRates.Name = "grpDisplayRefreshRates";
			this.grpDisplayRefreshRates.Size = new System.Drawing.Size(176, 128);
			this.grpDisplayRefreshRates.TabIndex = 39;
			this.grpDisplayRefreshRates.TabStop = false;
			this.grpDisplayRefreshRates.Text = "Refresh Rates";
			// 
			// udDisplayCPUMeter
			// 
			this.udDisplayCPUMeter.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			this.udDisplayCPUMeter.Location = new System.Drawing.Point(120, 96);
			this.udDisplayCPUMeter.Maximum = new System.Decimal(new int[] {
																			  9999,
																			  0,
																			  0,
																			  0});
			this.udDisplayCPUMeter.Minimum = new System.Decimal(new int[] {
																			  100,
																			  0,
																			  0,
																			  0});
			this.udDisplayCPUMeter.Name = "udDisplayCPUMeter";
			this.udDisplayCPUMeter.Size = new System.Drawing.Size(48, 20);
			this.udDisplayCPUMeter.TabIndex = 38;
			this.toolTip1.SetToolTip(this.udDisplayCPUMeter, "CPU Meter Refresh Rate.");
			this.udDisplayCPUMeter.Value = new System.Decimal(new int[] {
																			1000,
																			0,
																			0,
																			0});
			this.udDisplayCPUMeter.LostFocus += new System.EventHandler(this.udDisplayCPUMeter_LostFocus);
			this.udDisplayCPUMeter.ValueChanged += new System.EventHandler(this.udDisplayCPUMeter_ValueChanged);
			// 
			// lblDisplayCPUMeter
			// 
			this.lblDisplayCPUMeter.Image = null;
			this.lblDisplayCPUMeter.Location = new System.Drawing.Point(16, 96);
			this.lblDisplayCPUMeter.Name = "lblDisplayCPUMeter";
			this.lblDisplayCPUMeter.TabIndex = 37;
			this.lblDisplayCPUMeter.Text = "CPU Meter (ms)";
			// 
			// udDisplayPeakText
			// 
			this.udDisplayPeakText.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			this.udDisplayPeakText.Location = new System.Drawing.Point(120, 72);
			this.udDisplayPeakText.Maximum = new System.Decimal(new int[] {
																			  9999,
																			  0,
																			  0,
																			  0});
			this.udDisplayPeakText.Minimum = new System.Decimal(new int[] {
																			  100,
																			  0,
																			  0,
																			  0});
			this.udDisplayPeakText.Name = "udDisplayPeakText";
			this.udDisplayPeakText.Size = new System.Drawing.Size(48, 20);
			this.udDisplayPeakText.TabIndex = 36;
			this.toolTip1.SetToolTip(this.udDisplayPeakText, "Peak Text Refresh Rate.");
			this.udDisplayPeakText.Value = new System.Decimal(new int[] {
																			500,
																			0,
																			0,
																			0});
			this.udDisplayPeakText.LostFocus += new System.EventHandler(this.udDisplayPeakText_LostFocus);
			this.udDisplayPeakText.ValueChanged += new System.EventHandler(this.udDisplayPeakText_ValueChanged);
			// 
			// lblDisplayPeakText
			// 
			this.lblDisplayPeakText.Image = null;
			this.lblDisplayPeakText.Location = new System.Drawing.Point(16, 72);
			this.lblDisplayPeakText.Name = "lblDisplayPeakText";
			this.lblDisplayPeakText.TabIndex = 35;
			this.lblDisplayPeakText.Text = "Peak Text (ms)";
			// 
			// udDisplayMeterDelay
			// 
			this.udDisplayMeterDelay.Increment = new System.Decimal(new int[] {
																				  1,
																				  0,
																				  0,
																				  0});
			this.udDisplayMeterDelay.Location = new System.Drawing.Point(120, 48);
			this.udDisplayMeterDelay.Maximum = new System.Decimal(new int[] {
																				5000,
																				0,
																				0,
																				0});
			this.udDisplayMeterDelay.Minimum = new System.Decimal(new int[] {
																				50,
																				0,
																				0,
																				0});
			this.udDisplayMeterDelay.Name = "udDisplayMeterDelay";
			this.udDisplayMeterDelay.Size = new System.Drawing.Size(48, 20);
			this.udDisplayMeterDelay.TabIndex = 34;
			this.toolTip1.SetToolTip(this.udDisplayMeterDelay, "Multimeter Refresh Rate.");
			this.udDisplayMeterDelay.Value = new System.Decimal(new int[] {
																			  100,
																			  0,
																			  0,
																			  0});
			this.udDisplayMeterDelay.LostFocus += new System.EventHandler(this.udDisplayMeterDelay_LostFocus);
			this.udDisplayMeterDelay.ValueChanged += new System.EventHandler(this.udDisplayMeterDelay_ValueChanged);
			// 
			// lblDisplayMeterDelay
			// 
			this.lblDisplayMeterDelay.Image = null;
			this.lblDisplayMeterDelay.Location = new System.Drawing.Point(16, 48);
			this.lblDisplayMeterDelay.Name = "lblDisplayMeterDelay";
			this.lblDisplayMeterDelay.Size = new System.Drawing.Size(96, 23);
			this.lblDisplayMeterDelay.TabIndex = 33;
			this.lblDisplayMeterDelay.Text = "Meter Delay (ms):";
			// 
			// udDisplayFPS
			// 
			this.udDisplayFPS.Increment = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   0});
			this.udDisplayFPS.Location = new System.Drawing.Point(120, 24);
			this.udDisplayFPS.Maximum = new System.Decimal(new int[] {
																		 50,
																		 0,
																		 0,
																		 0});
			this.udDisplayFPS.Minimum = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 0});
			this.udDisplayFPS.Name = "udDisplayFPS";
			this.udDisplayFPS.Size = new System.Drawing.Size(48, 20);
			this.udDisplayFPS.TabIndex = 32;
			this.toolTip1.SetToolTip(this.udDisplayFPS, "Frames Per Second (approximate)");
			this.udDisplayFPS.Value = new System.Decimal(new int[] {
																	   15,
																	   0,
																	   0,
																	   0});
			this.udDisplayFPS.LostFocus += new System.EventHandler(this.udDisplayFPS_LostFocus);
			this.udDisplayFPS.ValueChanged += new System.EventHandler(this.udDisplayFPS_ValueChanged);
			// 
			// lblDisplayFPS
			// 
			this.lblDisplayFPS.Image = null;
			this.lblDisplayFPS.Location = new System.Drawing.Point(16, 24);
			this.lblDisplayFPS.Name = "lblDisplayFPS";
			this.lblDisplayFPS.Size = new System.Drawing.Size(104, 16);
			this.lblDisplayFPS.TabIndex = 31;
			this.lblDisplayFPS.Text = "Main Display FPS:";
			// 
			// grpDisplayAverage
			// 
			this.grpDisplayAverage.Controls.Add(this.udDisplayAVGTime);
			this.grpDisplayAverage.Controls.Add(this.lblDisplayAVGTime);
			this.grpDisplayAverage.Location = new System.Drawing.Point(312, 72);
			this.grpDisplayAverage.Name = "grpDisplayAverage";
			this.grpDisplayAverage.Size = new System.Drawing.Size(120, 56);
			this.grpDisplayAverage.TabIndex = 38;
			this.grpDisplayAverage.TabStop = false;
			this.grpDisplayAverage.Text = "Averaging";
			// 
			// udDisplayAVGTime
			// 
			this.udDisplayAVGTime.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   0});
			this.udDisplayAVGTime.Location = new System.Drawing.Point(64, 24);
			this.udDisplayAVGTime.Maximum = new System.Decimal(new int[] {
																			 9999,
																			 0,
																			 0,
																			 0});
			this.udDisplayAVGTime.Minimum = new System.Decimal(new int[] {
																			 1,
																			 0,
																			 0,
																			 0});
			this.udDisplayAVGTime.Name = "udDisplayAVGTime";
			this.udDisplayAVGTime.Size = new System.Drawing.Size(48, 20);
			this.udDisplayAVGTime.TabIndex = 2;
			this.toolTip1.SetToolTip(this.udDisplayAVGTime, "When averaging, use this number of buffers to calculate the average.");
			this.udDisplayAVGTime.Value = new System.Decimal(new int[] {
																		   350,
																		   0,
																		   0,
																		   0});
			this.udDisplayAVGTime.LostFocus += new System.EventHandler(this.udDisplayAVGTime_LostFocus);
			this.udDisplayAVGTime.ValueChanged += new System.EventHandler(this.udDisplayAVGTime_ValueChanged);
			// 
			// lblDisplayAVGTime
			// 
			this.lblDisplayAVGTime.Image = null;
			this.lblDisplayAVGTime.Location = new System.Drawing.Point(8, 24);
			this.lblDisplayAVGTime.Name = "lblDisplayAVGTime";
			this.lblDisplayAVGTime.Size = new System.Drawing.Size(64, 23);
			this.lblDisplayAVGTime.TabIndex = 3;
			this.lblDisplayAVGTime.Text = "Time (ms):";
			// 
			// grpDisplayPhase
			// 
			this.grpDisplayPhase.Controls.Add(this.lblDisplayPhasePts);
			this.grpDisplayPhase.Controls.Add(this.udDisplayPhasePts);
			this.grpDisplayPhase.Location = new System.Drawing.Point(312, 8);
			this.grpDisplayPhase.Name = "grpDisplayPhase";
			this.grpDisplayPhase.Size = new System.Drawing.Size(120, 56);
			this.grpDisplayPhase.TabIndex = 37;
			this.grpDisplayPhase.TabStop = false;
			this.grpDisplayPhase.Text = "Phase Mode";
			// 
			// lblDisplayPhasePts
			// 
			this.lblDisplayPhasePts.Image = null;
			this.lblDisplayPhasePts.Location = new System.Drawing.Point(8, 24);
			this.lblDisplayPhasePts.Name = "lblDisplayPhasePts";
			this.lblDisplayPhasePts.Size = new System.Drawing.Size(56, 23);
			this.lblDisplayPhasePts.TabIndex = 1;
			this.lblDisplayPhasePts.Text = "Num Pts:";
			// 
			// udDisplayPhasePts
			// 
			this.udDisplayPhasePts.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			this.udDisplayPhasePts.Location = new System.Drawing.Point(64, 24);
			this.udDisplayPhasePts.Maximum = new System.Decimal(new int[] {
																			  500,
																			  0,
																			  0,
																			  0});
			this.udDisplayPhasePts.Minimum = new System.Decimal(new int[] {
																			  25,
																			  0,
																			  0,
																			  0});
			this.udDisplayPhasePts.Name = "udDisplayPhasePts";
			this.udDisplayPhasePts.Size = new System.Drawing.Size(48, 20);
			this.udDisplayPhasePts.TabIndex = 0;
			this.toolTip1.SetToolTip(this.udDisplayPhasePts, "Number of points to display in Phase Mode.");
			this.udDisplayPhasePts.Value = new System.Decimal(new int[] {
																			100,
																			0,
																			0,
																			0});
			this.udDisplayPhasePts.LostFocus += new System.EventHandler(this.udDisplayPhasePts_LostFocus);
			this.udDisplayPhasePts.ValueChanged += new System.EventHandler(this.udDisplayPhasePts_ValueChanged);
			// 
			// grpDisplaySpectrumGrid
			// 
			this.grpDisplaySpectrumGrid.Controls.Add(this.comboDisplayLabelAlign);
			this.grpDisplaySpectrumGrid.Controls.Add(this.lblDisplayAlign);
			this.grpDisplaySpectrumGrid.Controls.Add(this.udDisplayGridStep);
			this.grpDisplaySpectrumGrid.Controls.Add(this.udDisplayGridMin);
			this.grpDisplaySpectrumGrid.Controls.Add(this.udDisplayGridMax);
			this.grpDisplaySpectrumGrid.Controls.Add(this.lblDisplayGridStep);
			this.grpDisplaySpectrumGrid.Controls.Add(this.lblDisplayGridMin);
			this.grpDisplaySpectrumGrid.Controls.Add(this.lblDisplayGridMax);
			this.grpDisplaySpectrumGrid.Location = new System.Drawing.Point(8, 8);
			this.grpDisplaySpectrumGrid.Name = "grpDisplaySpectrumGrid";
			this.grpDisplaySpectrumGrid.Size = new System.Drawing.Size(112, 128);
			this.grpDisplaySpectrumGrid.TabIndex = 29;
			this.grpDisplaySpectrumGrid.TabStop = false;
			this.grpDisplaySpectrumGrid.Text = "Spectrum Grid";
			// 
			// comboDisplayLabelAlign
			// 
			this.comboDisplayLabelAlign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDisplayLabelAlign.DropDownWidth = 48;
			this.comboDisplayLabelAlign.Items.AddRange(new object[] {
																		"Left",
																		"Cntr",
																		"Right",
																		"Auto",
																		"Off"});
			this.comboDisplayLabelAlign.Location = new System.Drawing.Point(48, 96);
			this.comboDisplayLabelAlign.Name = "comboDisplayLabelAlign";
			this.comboDisplayLabelAlign.Size = new System.Drawing.Size(56, 21);
			this.comboDisplayLabelAlign.TabIndex = 30;
			this.toolTip1.SetToolTip(this.comboDisplayLabelAlign, "Sets the alignement of the grid callouts on the display.");
			this.comboDisplayLabelAlign.SelectedIndexChanged += new System.EventHandler(this.comboDisplayLabelAlign_SelectedIndexChanged);
			// 
			// lblDisplayAlign
			// 
			this.lblDisplayAlign.Image = null;
			this.lblDisplayAlign.Location = new System.Drawing.Point(8, 96);
			this.lblDisplayAlign.Name = "lblDisplayAlign";
			this.lblDisplayAlign.Size = new System.Drawing.Size(40, 16);
			this.lblDisplayAlign.TabIndex = 29;
			this.lblDisplayAlign.Text = "Align:";
			// 
			// udDisplayGridStep
			// 
			this.udDisplayGridStep.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			this.udDisplayGridStep.Location = new System.Drawing.Point(48, 72);
			this.udDisplayGridStep.Maximum = new System.Decimal(new int[] {
																			  40,
																			  0,
																			  0,
																			  0});
			this.udDisplayGridStep.Minimum = new System.Decimal(new int[] {
																			  1,
																			  0,
																			  0,
																			  0});
			this.udDisplayGridStep.Name = "udDisplayGridStep";
			this.udDisplayGridStep.Size = new System.Drawing.Size(56, 20);
			this.udDisplayGridStep.TabIndex = 25;
			this.toolTip1.SetToolTip(this.udDisplayGridStep, "Horizontal Grid Step Size in dB.");
			this.udDisplayGridStep.Value = new System.Decimal(new int[] {
																			10,
																			0,
																			0,
																			0});
			this.udDisplayGridStep.LostFocus += new System.EventHandler(this.udDisplayGridStep_LostFocus);
			this.udDisplayGridStep.ValueChanged += new System.EventHandler(this.udDisplayGridStep_ValueChanged);
			// 
			// udDisplayGridMin
			// 
			this.udDisplayGridMin.Increment = new System.Decimal(new int[] {
																			   5,
																			   0,
																			   0,
																			   0});
			this.udDisplayGridMin.Location = new System.Drawing.Point(48, 48);
			this.udDisplayGridMin.Maximum = new System.Decimal(new int[] {
																			 200,
																			 0,
																			 0,
																			 0});
			this.udDisplayGridMin.Minimum = new System.Decimal(new int[] {
																			 200,
																			 0,
																			 0,
																			 -2147483648});
			this.udDisplayGridMin.Name = "udDisplayGridMin";
			this.udDisplayGridMin.Size = new System.Drawing.Size(56, 20);
			this.udDisplayGridMin.TabIndex = 24;
			this.toolTip1.SetToolTip(this.udDisplayGridMin, "Signal Level at bottom of display in dB.");
			this.udDisplayGridMin.Value = new System.Decimal(new int[] {
																		   150,
																		   0,
																		   0,
																		   -2147483648});
			this.udDisplayGridMin.LostFocus += new System.EventHandler(this.udDisplayGridMin_LostFocus);
			this.udDisplayGridMin.ValueChanged += new System.EventHandler(this.udDisplayGridMin_ValueChanged);
			// 
			// udDisplayGridMax
			// 
			this.udDisplayGridMax.Increment = new System.Decimal(new int[] {
																			   5,
																			   0,
																			   0,
																			   0});
			this.udDisplayGridMax.Location = new System.Drawing.Point(48, 24);
			this.udDisplayGridMax.Maximum = new System.Decimal(new int[] {
																			 200,
																			 0,
																			 0,
																			 0});
			this.udDisplayGridMax.Minimum = new System.Decimal(new int[] {
																			 200,
																			 0,
																			 0,
																			 -2147483648});
			this.udDisplayGridMax.Name = "udDisplayGridMax";
			this.udDisplayGridMax.Size = new System.Drawing.Size(56, 20);
			this.udDisplayGridMax.TabIndex = 23;
			this.toolTip1.SetToolTip(this.udDisplayGridMax, "Signal level at top of display in dB.");
			this.udDisplayGridMax.Value = new System.Decimal(new int[] {
																		   0,
																		   0,
																		   0,
																		   0});
			this.udDisplayGridMax.LostFocus += new System.EventHandler(this.udDisplayGridMax_LostFocus);
			this.udDisplayGridMax.ValueChanged += new System.EventHandler(this.udDisplayGridMax_ValueChanged);
			// 
			// lblDisplayGridStep
			// 
			this.lblDisplayGridStep.Image = null;
			this.lblDisplayGridStep.Location = new System.Drawing.Point(8, 72);
			this.lblDisplayGridStep.Name = "lblDisplayGridStep";
			this.lblDisplayGridStep.Size = new System.Drawing.Size(32, 16);
			this.lblDisplayGridStep.TabIndex = 28;
			this.lblDisplayGridStep.Text = "Step:";
			// 
			// lblDisplayGridMin
			// 
			this.lblDisplayGridMin.Image = null;
			this.lblDisplayGridMin.Location = new System.Drawing.Point(8, 48);
			this.lblDisplayGridMin.Name = "lblDisplayGridMin";
			this.lblDisplayGridMin.Size = new System.Drawing.Size(32, 16);
			this.lblDisplayGridMin.TabIndex = 27;
			this.lblDisplayGridMin.Text = "Min:";
			// 
			// lblDisplayGridMax
			// 
			this.lblDisplayGridMax.Image = null;
			this.lblDisplayGridMax.Location = new System.Drawing.Point(8, 24);
			this.lblDisplayGridMax.Name = "lblDisplayGridMax";
			this.lblDisplayGridMax.Size = new System.Drawing.Size(32, 16);
			this.lblDisplayGridMax.TabIndex = 26;
			this.lblDisplayGridMax.Text = "Max:";
			// 
			// tpDSP
			// 
			this.tpDSP.Controls.Add(this.tcDSP);
			this.tpDSP.Location = new System.Drawing.Point(4, 22);
			this.tpDSP.Name = "tpDSP";
			this.tpDSP.Size = new System.Drawing.Size(584, 286);
			this.tpDSP.TabIndex = 1;
			this.tpDSP.Text = "DSP";
			// 
			// tcDSP
			// 
			this.tcDSP.Controls.Add(this.tpDSPOptions);
			this.tcDSP.Controls.Add(this.tpDSPImageReject);
			this.tcDSP.Controls.Add(this.tpDSPKeyer);
			this.tcDSP.Controls.Add(this.tpDSPAGCALC);
			this.tcDSP.Location = new System.Drawing.Point(0, 0);
			this.tcDSP.Name = "tcDSP";
			this.tcDSP.SelectedIndex = 0;
			this.tcDSP.Size = new System.Drawing.Size(600, 344);
			this.tcDSP.TabIndex = 0;
			// 
			// tpDSPOptions
			// 
			this.tpDSPOptions.Controls.Add(this.grpDSPBufferSize);
			this.tpDSPOptions.Controls.Add(this.grpDSPNB);
			this.tpDSPOptions.Controls.Add(this.grpDSPLMSNR);
			this.tpDSPOptions.Controls.Add(this.grpDSPLMSANF);
			this.tpDSPOptions.Controls.Add(this.grpDSPWindow);
			this.tpDSPOptions.Controls.Add(this.grpDSPNB2);
			this.tpDSPOptions.Location = new System.Drawing.Point(4, 22);
			this.tpDSPOptions.Name = "tpDSPOptions";
			this.tpDSPOptions.Size = new System.Drawing.Size(592, 318);
			this.tpDSPOptions.TabIndex = 2;
			this.tpDSPOptions.Text = "Options";
			// 
			// grpDSPBufferSize
			// 
			this.grpDSPBufferSize.Controls.Add(this.comboDSPBufSize);
			this.grpDSPBufferSize.Location = new System.Drawing.Point(256, 8);
			this.grpDSPBufferSize.Name = "grpDSPBufferSize";
			this.grpDSPBufferSize.Size = new System.Drawing.Size(104, 48);
			this.grpDSPBufferSize.TabIndex = 37;
			this.grpDSPBufferSize.TabStop = false;
			this.grpDSPBufferSize.Text = "Buffer Size";
			// 
			// comboDSPBufSize
			// 
			this.comboDSPBufSize.DisplayMember = "2048";
			this.comboDSPBufSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDSPBufSize.DropDownWidth = 64;
			this.comboDSPBufSize.Items.AddRange(new object[] {
																 "256",
																 "512",
																 "1024",
																 "2048",
																 "4096"});
			this.comboDSPBufSize.Location = new System.Drawing.Point(16, 16);
			this.comboDSPBufSize.Name = "comboDSPBufSize";
			this.comboDSPBufSize.Size = new System.Drawing.Size(64, 21);
			this.comboDSPBufSize.TabIndex = 18;
			this.toolTip1.SetToolTip(this.comboDSPBufSize, "Sets DSP internal Buffer Size -- larger yields sharper filters, more latency");
			this.comboDSPBufSize.ValueMember = "1024";
			this.comboDSPBufSize.SelectedIndexChanged += new System.EventHandler(this.comboDSPBufSize_SelectedIndexChanged);
			// 
			// grpDSPNB
			// 
			this.grpDSPNB.Controls.Add(this.udDSPNB);
			this.grpDSPNB.Controls.Add(this.lblDSPNBThreshold);
			this.grpDSPNB.Location = new System.Drawing.Point(384, 8);
			this.grpDSPNB.Name = "grpDSPNB";
			this.grpDSPNB.Size = new System.Drawing.Size(120, 56);
			this.grpDSPNB.TabIndex = 35;
			this.grpDSPNB.TabStop = false;
			this.grpDSPNB.Text = "Noise Blanker";
			// 
			// udDSPNB
			// 
			this.udDSPNB.Increment = new System.Decimal(new int[] {
																	  1,
																	  0,
																	  0,
																	  0});
			this.udDSPNB.Location = new System.Drawing.Point(64, 24);
			this.udDSPNB.Maximum = new System.Decimal(new int[] {
																	200,
																	0,
																	0,
																	0});
			this.udDSPNB.Minimum = new System.Decimal(new int[] {
																	1,
																	0,
																	0,
																	0});
			this.udDSPNB.Name = "udDSPNB";
			this.udDSPNB.Size = new System.Drawing.Size(40, 20);
			this.udDSPNB.TabIndex = 0;
			this.toolTip1.SetToolTip(this.udDSPNB, "Controls the detection threshold for impulse noise.  ");
			this.udDSPNB.Value = new System.Decimal(new int[] {
																  20,
																  0,
																  0,
																  0});
			this.udDSPNB.LostFocus += new System.EventHandler(this.udDSPNB_LostFocus);
			this.udDSPNB.ValueChanged += new System.EventHandler(this.udDSPNB_ValueChanged);
			// 
			// lblDSPNBThreshold
			// 
			this.lblDSPNBThreshold.Image = null;
			this.lblDSPNBThreshold.Location = new System.Drawing.Point(8, 24);
			this.lblDSPNBThreshold.Name = "lblDSPNBThreshold";
			this.lblDSPNBThreshold.Size = new System.Drawing.Size(64, 16);
			this.lblDSPNBThreshold.TabIndex = 9;
			this.lblDSPNBThreshold.Text = "Threshold:";
			// 
			// grpDSPLMSNR
			// 
			this.grpDSPLMSNR.Controls.Add(this.chkBlkLMSNR);
			this.grpDSPLMSNR.Controls.Add(this.lblLMSNRgain);
			this.grpDSPLMSNR.Controls.Add(this.udLMSNRgain);
			this.grpDSPLMSNR.Controls.Add(this.udLMSNRdelay);
			this.grpDSPLMSNR.Controls.Add(this.lblLMSNRdelay);
			this.grpDSPLMSNR.Controls.Add(this.udLMSNRtaps);
			this.grpDSPLMSNR.Controls.Add(this.lblLMSNRtaps);
			this.grpDSPLMSNR.Location = new System.Drawing.Point(8, 8);
			this.grpDSPLMSNR.Name = "grpDSPLMSNR";
			this.grpDSPLMSNR.Size = new System.Drawing.Size(112, 144);
			this.grpDSPLMSNR.TabIndex = 33;
			this.grpDSPLMSNR.TabStop = false;
			this.grpDSPLMSNR.Text = "NR";
			// 
			// chkBlkLMSNR
			// 
			this.chkBlkLMSNR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkBlkLMSNR.Image = null;
			this.chkBlkLMSNR.Location = new System.Drawing.Point(16, 112);
			this.chkBlkLMSNR.Name = "chkBlkLMSNR";
			this.chkBlkLMSNR.Size = new System.Drawing.Size(80, 16);
			this.chkBlkLMSNR.TabIndex = 10;
			this.chkBlkLMSNR.Text = "Block LMS";
			this.toolTip1.SetToolTip(this.chkBlkLMSNR, "Enables the Fast Block LMS NR.");
			this.chkBlkLMSNR.CheckedChanged += new System.EventHandler(this.chkBlkLMSNR_CheckedChanged);
			// 
			// lblLMSNRgain
			// 
			this.lblLMSNRgain.Image = null;
			this.lblLMSNRgain.Location = new System.Drawing.Point(8, 72);
			this.lblLMSNRgain.Name = "lblLMSNRgain";
			this.lblLMSNRgain.Size = new System.Drawing.Size(40, 16);
			this.lblLMSNRgain.TabIndex = 9;
			this.lblLMSNRgain.Text = "Gain:";
			// 
			// udLMSNRgain
			// 
			this.udLMSNRgain.Increment = new System.Decimal(new int[] {
																		  1,
																		  0,
																		  0,
																		  0});
			this.udLMSNRgain.Location = new System.Drawing.Point(56, 72);
			this.udLMSNRgain.Maximum = new System.Decimal(new int[] {
																		9999,
																		0,
																		0,
																		0});
			this.udLMSNRgain.Minimum = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		0});
			this.udLMSNRgain.Name = "udLMSNRgain";
			this.udLMSNRgain.Size = new System.Drawing.Size(48, 20);
			this.udLMSNRgain.TabIndex = 7;
			this.toolTip1.SetToolTip(this.udLMSNRgain, "Determines the adaptation rate of the filter.");
			this.udLMSNRgain.Value = new System.Decimal(new int[] {
																	  50,
																	  0,
																	  0,
																	  0});
			this.udLMSNRgain.LostFocus += new System.EventHandler(this.udLMSNRgain_LostFocus);
			this.udLMSNRgain.ValueChanged += new System.EventHandler(this.udLMSNR_ValueChanged);
			// 
			// udLMSNRdelay
			// 
			this.udLMSNRdelay.Increment = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   0});
			this.udLMSNRdelay.Location = new System.Drawing.Point(56, 48);
			this.udLMSNRdelay.Maximum = new System.Decimal(new int[] {
																		 127,
																		 0,
																		 0,
																		 0});
			this.udLMSNRdelay.Minimum = new System.Decimal(new int[] {
																		 16,
																		 0,
																		 0,
																		 0});
			this.udLMSNRdelay.Name = "udLMSNRdelay";
			this.udLMSNRdelay.Size = new System.Drawing.Size(48, 20);
			this.udLMSNRdelay.TabIndex = 6;
			this.toolTip1.SetToolTip(this.udLMSNRdelay, "Determines how far back you look in the signal before you begin to compute a cohe" +
				"rent signal enhancement filter.  ");
			this.udLMSNRdelay.Value = new System.Decimal(new int[] {
																	   50,
																	   0,
																	   0,
																	   0});
			this.udLMSNRdelay.LostFocus += new System.EventHandler(this.udLMSNRdelay_LostFocus);
			this.udLMSNRdelay.ValueChanged += new System.EventHandler(this.udLMSNR_ValueChanged);
			// 
			// lblLMSNRdelay
			// 
			this.lblLMSNRdelay.Image = null;
			this.lblLMSNRdelay.Location = new System.Drawing.Point(8, 48);
			this.lblLMSNRdelay.Name = "lblLMSNRdelay";
			this.lblLMSNRdelay.Size = new System.Drawing.Size(40, 16);
			this.lblLMSNRdelay.TabIndex = 5;
			this.lblLMSNRdelay.Text = "Delay:";
			// 
			// udLMSNRtaps
			// 
			this.udLMSNRtaps.Increment = new System.Decimal(new int[] {
																		  1,
																		  0,
																		  0,
																		  0});
			this.udLMSNRtaps.Location = new System.Drawing.Point(56, 24);
			this.udLMSNRtaps.Maximum = new System.Decimal(new int[] {
																		127,
																		0,
																		0,
																		0});
			this.udLMSNRtaps.Minimum = new System.Decimal(new int[] {
																		31,
																		0,
																		0,
																		0});
			this.udLMSNRtaps.Name = "udLMSNRtaps";
			this.udLMSNRtaps.Size = new System.Drawing.Size(48, 20);
			this.udLMSNRtaps.TabIndex = 5;
			this.toolTip1.SetToolTip(this.udLMSNRtaps, "Determines the length of the NR computed filter.  ");
			this.udLMSNRtaps.Value = new System.Decimal(new int[] {
																	  65,
																	  0,
																	  0,
																	  0});
			this.udLMSNRtaps.LostFocus += new System.EventHandler(this.udLMSNRtaps_LostFocus);
			this.udLMSNRtaps.ValueChanged += new System.EventHandler(this.udLMSNR_ValueChanged);
			// 
			// lblLMSNRtaps
			// 
			this.lblLMSNRtaps.Image = null;
			this.lblLMSNRtaps.Location = new System.Drawing.Point(8, 24);
			this.lblLMSNRtaps.Name = "lblLMSNRtaps";
			this.lblLMSNRtaps.Size = new System.Drawing.Size(40, 16);
			this.lblLMSNRtaps.TabIndex = 3;
			this.lblLMSNRtaps.Text = "Taps:";
			// 
			// grpDSPLMSANF
			// 
			this.grpDSPLMSANF.Controls.Add(this.chkBlkLMSANF);
			this.grpDSPLMSANF.Controls.Add(this.lblLMSANFgain);
			this.grpDSPLMSANF.Controls.Add(this.udLMSANFgain);
			this.grpDSPLMSANF.Controls.Add(this.lblLMSANFdelay);
			this.grpDSPLMSANF.Controls.Add(this.udLMSANFdelay);
			this.grpDSPLMSANF.Controls.Add(this.lblLMSANFTaps);
			this.grpDSPLMSANF.Controls.Add(this.udLMSANFtaps);
			this.grpDSPLMSANF.Location = new System.Drawing.Point(128, 8);
			this.grpDSPLMSANF.Name = "grpDSPLMSANF";
			this.grpDSPLMSANF.Size = new System.Drawing.Size(120, 144);
			this.grpDSPLMSANF.TabIndex = 32;
			this.grpDSPLMSANF.TabStop = false;
			this.grpDSPLMSANF.Text = "ANF";
			// 
			// chkBlkLMSANF
			// 
			this.chkBlkLMSANF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkBlkLMSANF.Image = null;
			this.chkBlkLMSANF.Location = new System.Drawing.Point(20, 112);
			this.chkBlkLMSANF.Name = "chkBlkLMSANF";
			this.chkBlkLMSANF.Size = new System.Drawing.Size(80, 16);
			this.chkBlkLMSANF.TabIndex = 11;
			this.chkBlkLMSANF.Text = "Block LMS";
			this.toolTip1.SetToolTip(this.chkBlkLMSANF, "Enables the Fast Block LMS ANF.");
			this.chkBlkLMSANF.CheckedChanged += new System.EventHandler(this.chkBlkLMSANF_CheckedChanged);
			// 
			// lblLMSANFgain
			// 
			this.lblLMSANFgain.Image = null;
			this.lblLMSANFgain.Location = new System.Drawing.Point(8, 72);
			this.lblLMSANFgain.Name = "lblLMSANFgain";
			this.lblLMSANFgain.Size = new System.Drawing.Size(40, 16);
			this.lblLMSANFgain.TabIndex = 6;
			this.lblLMSANFgain.Text = "Gain:";
			// 
			// udLMSANFgain
			// 
			this.udLMSANFgain.Increment = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   0});
			this.udLMSANFgain.Location = new System.Drawing.Point(56, 72);
			this.udLMSANFgain.Maximum = new System.Decimal(new int[] {
																		 9999,
																		 0,
																		 0,
																		 0});
			this.udLMSANFgain.Minimum = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 0});
			this.udLMSANFgain.Name = "udLMSANFgain";
			this.udLMSANFgain.Size = new System.Drawing.Size(48, 20);
			this.udLMSANFgain.TabIndex = 3;
			this.toolTip1.SetToolTip(this.udLMSANFgain, "Determines the adaptation rate of the filter.");
			this.udLMSANFgain.Value = new System.Decimal(new int[] {
																	   50,
																	   0,
																	   0,
																	   0});
			this.udLMSANFgain.LostFocus += new System.EventHandler(this.udLMSANFgain_LostFocus);
			this.udLMSANFgain.ValueChanged += new System.EventHandler(this.udLMSANF_ValueChanged);
			// 
			// lblLMSANFdelay
			// 
			this.lblLMSANFdelay.Image = null;
			this.lblLMSANFdelay.Location = new System.Drawing.Point(8, 48);
			this.lblLMSANFdelay.Name = "lblLMSANFdelay";
			this.lblLMSANFdelay.Size = new System.Drawing.Size(40, 16);
			this.lblLMSANFdelay.TabIndex = 4;
			this.lblLMSANFdelay.Text = "Delay:";
			// 
			// udLMSANFdelay
			// 
			this.udLMSANFdelay.Increment = new System.Decimal(new int[] {
																			1,
																			0,
																			0,
																			0});
			this.udLMSANFdelay.Location = new System.Drawing.Point(56, 48);
			this.udLMSANFdelay.Maximum = new System.Decimal(new int[] {
																		  127,
																		  0,
																		  0,
																		  0});
			this.udLMSANFdelay.Minimum = new System.Decimal(new int[] {
																		  16,
																		  0,
																		  0,
																		  0});
			this.udLMSANFdelay.Name = "udLMSANFdelay";
			this.udLMSANFdelay.Size = new System.Drawing.Size(48, 20);
			this.udLMSANFdelay.TabIndex = 2;
			this.toolTip1.SetToolTip(this.udLMSANFdelay, "Determines how far back you look in the signal before you begin to compute a canc" +
				"ellation filter");
			this.udLMSANFdelay.Value = new System.Decimal(new int[] {
																		50,
																		0,
																		0,
																		0});
			this.udLMSANFdelay.LostFocus += new System.EventHandler(this.udLMSANFdelay_LostFocus);
			this.udLMSANFdelay.ValueChanged += new System.EventHandler(this.udLMSANF_ValueChanged);
			// 
			// lblLMSANFTaps
			// 
			this.lblLMSANFTaps.Image = null;
			this.lblLMSANFTaps.Location = new System.Drawing.Point(8, 24);
			this.lblLMSANFTaps.Name = "lblLMSANFTaps";
			this.lblLMSANFTaps.Size = new System.Drawing.Size(40, 16);
			this.lblLMSANFTaps.TabIndex = 2;
			this.lblLMSANFTaps.Text = "Taps:";
			// 
			// udLMSANFtaps
			// 
			this.udLMSANFtaps.Increment = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   0});
			this.udLMSANFtaps.Location = new System.Drawing.Point(56, 24);
			this.udLMSANFtaps.Maximum = new System.Decimal(new int[] {
																		 127,
																		 0,
																		 0,
																		 0});
			this.udLMSANFtaps.Minimum = new System.Decimal(new int[] {
																		 31,
																		 0,
																		 0,
																		 0});
			this.udLMSANFtaps.Name = "udLMSANFtaps";
			this.udLMSANFtaps.Size = new System.Drawing.Size(48, 20);
			this.udLMSANFtaps.TabIndex = 1;
			this.toolTip1.SetToolTip(this.udLMSANFtaps, "Determines the length of the computed notch filter.");
			this.udLMSANFtaps.Value = new System.Decimal(new int[] {
																	   65,
																	   0,
																	   0,
																	   0});
			this.udLMSANFtaps.LostFocus += new System.EventHandler(this.udLMSANFtaps_LostFocus);
			this.udLMSANFtaps.ValueChanged += new System.EventHandler(this.udLMSANF_ValueChanged);
			// 
			// grpDSPWindow
			// 
			this.grpDSPWindow.Controls.Add(this.comboDSPWindow);
			this.grpDSPWindow.Location = new System.Drawing.Point(256, 64);
			this.grpDSPWindow.Name = "grpDSPWindow";
			this.grpDSPWindow.Size = new System.Drawing.Size(120, 56);
			this.grpDSPWindow.TabIndex = 36;
			this.grpDSPWindow.TabStop = false;
			this.grpDSPWindow.Text = "Window";
			// 
			// comboDSPWindow
			// 
			this.comboDSPWindow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDSPWindow.DropDownWidth = 88;
			this.comboDSPWindow.Location = new System.Drawing.Point(16, 24);
			this.comboDSPWindow.Name = "comboDSPWindow";
			this.comboDSPWindow.Size = new System.Drawing.Size(88, 21);
			this.comboDSPWindow.TabIndex = 0;
			this.toolTip1.SetToolTip(this.comboDSPWindow, "Selects the DSP windowing function that will be applied to the power spectrum in " +
				"the main display when in Spectrum, Panadapter, and Waterfall modes.   ");
			this.comboDSPWindow.SelectedIndexChanged += new System.EventHandler(this.comboDSPWindow_SelectedIndexChanged);
			// 
			// grpDSPNB2
			// 
			this.grpDSPNB2.Controls.Add(this.udDSPNB2);
			this.grpDSPNB2.Controls.Add(this.lblDSPNB2Threshold);
			this.grpDSPNB2.Location = new System.Drawing.Point(384, 72);
			this.grpDSPNB2.Name = "grpDSPNB2";
			this.grpDSPNB2.Size = new System.Drawing.Size(120, 56);
			this.grpDSPNB2.TabIndex = 34;
			this.grpDSPNB2.TabStop = false;
			this.grpDSPNB2.Text = "Noise Blanker 2";
			// 
			// udDSPNB2
			// 
			this.udDSPNB2.Increment = new System.Decimal(new int[] {
																	   1,
																	   0,
																	   0,
																	   0});
			this.udDSPNB2.Location = new System.Drawing.Point(64, 24);
			this.udDSPNB2.Maximum = new System.Decimal(new int[] {
																	 1000,
																	 0,
																	 0,
																	 0});
			this.udDSPNB2.Minimum = new System.Decimal(new int[] {
																	 1,
																	 0,
																	 0,
																	 0});
			this.udDSPNB2.Name = "udDSPNB2";
			this.udDSPNB2.Size = new System.Drawing.Size(40, 20);
			this.udDSPNB2.TabIndex = 7;
			this.toolTip1.SetToolTip(this.udDSPNB2, "Controls the detection threshold for a pulse.  ");
			this.udDSPNB2.Value = new System.Decimal(new int[] {
																   15,
																   0,
																   0,
																   0});
			this.udDSPNB2.LostFocus += new System.EventHandler(this.udDSPNB2_LostFocus);
			this.udDSPNB2.ValueChanged += new System.EventHandler(this.udDSPNB2_ValueChanged);
			// 
			// lblDSPNB2Threshold
			// 
			this.lblDSPNB2Threshold.Image = null;
			this.lblDSPNB2Threshold.Location = new System.Drawing.Point(8, 24);
			this.lblDSPNB2Threshold.Name = "lblDSPNB2Threshold";
			this.lblDSPNB2Threshold.Size = new System.Drawing.Size(64, 16);
			this.lblDSPNB2Threshold.TabIndex = 10;
			this.lblDSPNB2Threshold.Text = "Threshold:";
			// 
			// tpDSPImageReject
			// 
			this.tpDSPImageReject.Controls.Add(this.grpDSPImageRejectRX);
			this.tpDSPImageReject.Controls.Add(this.grpDSPImageRejectTX);
			this.tpDSPImageReject.Location = new System.Drawing.Point(4, 22);
			this.tpDSPImageReject.Name = "tpDSPImageReject";
			this.tpDSPImageReject.Size = new System.Drawing.Size(592, 318);
			this.tpDSPImageReject.TabIndex = 1;
			this.tpDSPImageReject.Text = "Image Reject";
			// 
			// grpDSPImageRejectRX
			// 
			this.grpDSPImageRejectRX.Controls.Add(this.lblDSPGainValRX);
			this.grpDSPImageRejectRX.Controls.Add(this.lblDSPPhaseValRX);
			this.grpDSPImageRejectRX.Controls.Add(this.udDSPImageGainRX);
			this.grpDSPImageRejectRX.Controls.Add(this.udDSPImagePhaseRX);
			this.grpDSPImageRejectRX.Controls.Add(this.lblDSPImageGainRX);
			this.grpDSPImageRejectRX.Controls.Add(this.tbDSPImagePhaseRX);
			this.grpDSPImageRejectRX.Controls.Add(this.lblDSPImagePhaseRX);
			this.grpDSPImageRejectRX.Controls.Add(this.tbDSPImageGainRX);
			this.grpDSPImageRejectRX.Location = new System.Drawing.Point(8, 8);
			this.grpDSPImageRejectRX.Name = "grpDSPImageRejectRX";
			this.grpDSPImageRejectRX.Size = new System.Drawing.Size(240, 128);
			this.grpDSPImageRejectRX.TabIndex = 34;
			this.grpDSPImageRejectRX.TabStop = false;
			this.grpDSPImageRejectRX.Text = "Receive Rejection";
			// 
			// lblDSPGainValRX
			// 
			this.lblDSPGainValRX.Image = null;
			this.lblDSPGainValRX.Location = new System.Drawing.Point(72, 104);
			this.lblDSPGainValRX.Name = "lblDSPGainValRX";
			this.lblDSPGainValRX.Size = new System.Drawing.Size(163, 16);
			this.lblDSPGainValRX.TabIndex = 15;
			this.lblDSPGainValRX.Text = "-500    -250       0       250     500";
			// 
			// lblDSPPhaseValRX
			// 
			this.lblDSPPhaseValRX.Image = null;
			this.lblDSPPhaseValRX.Location = new System.Drawing.Point(72, 56);
			this.lblDSPPhaseValRX.Name = "lblDSPPhaseValRX";
			this.lblDSPPhaseValRX.Size = new System.Drawing.Size(163, 16);
			this.lblDSPPhaseValRX.TabIndex = 14;
			this.lblDSPPhaseValRX.Text = "-400    -200       0       200     400";
			// 
			// udDSPImageGainRX
			// 
			this.udDSPImageGainRX.DecimalPlaces = 2;
			this.udDSPImageGainRX.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   131072});
			this.udDSPImageGainRX.Location = new System.Drawing.Point(16, 88);
			this.udDSPImageGainRX.Maximum = new System.Decimal(new int[] {
																			 500,
																			 0,
																			 0,
																			 0});
			this.udDSPImageGainRX.Minimum = new System.Decimal(new int[] {
																			 500,
																			 0,
																			 0,
																			 -2147483648});
			this.udDSPImageGainRX.Name = "udDSPImageGainRX";
			this.udDSPImageGainRX.Size = new System.Drawing.Size(56, 20);
			this.udDSPImageGainRX.TabIndex = 8;
			this.toolTip1.SetToolTip(this.udDSPImageGainRX, "Sets the amplitude/gain offset between the I and Q channels.  ");
			this.udDSPImageGainRX.Value = new System.Decimal(new int[] {
																		   0,
																		   0,
																		   0,
																		   0});
			this.udDSPImageGainRX.LostFocus += new System.EventHandler(this.udDSPImageGainRX_LostFocus);
			this.udDSPImageGainRX.ValueChanged += new System.EventHandler(this.udDSPImageGainRX_ValueChanged);
			// 
			// udDSPImagePhaseRX
			// 
			this.udDSPImagePhaseRX.DecimalPlaces = 2;
			this.udDSPImagePhaseRX.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				131072});
			this.udDSPImagePhaseRX.Location = new System.Drawing.Point(16, 40);
			this.udDSPImagePhaseRX.Maximum = new System.Decimal(new int[] {
																			  400,
																			  0,
																			  0,
																			  0});
			this.udDSPImagePhaseRX.Minimum = new System.Decimal(new int[] {
																			  400,
																			  0,
																			  0,
																			  -2147483648});
			this.udDSPImagePhaseRX.Name = "udDSPImagePhaseRX";
			this.udDSPImagePhaseRX.Size = new System.Drawing.Size(56, 20);
			this.udDSPImagePhaseRX.TabIndex = 7;
			this.toolTip1.SetToolTip(this.udDSPImagePhaseRX, "Sets the phase offset between the I and Q channels.  ");
			this.udDSPImagePhaseRX.Value = new System.Decimal(new int[] {
																			0,
																			0,
																			0,
																			0});
			this.udDSPImagePhaseRX.LostFocus += new System.EventHandler(this.udDSPImagePhaseRX_LostFocus);
			this.udDSPImagePhaseRX.ValueChanged += new System.EventHandler(this.udDSPImagePhaseRX_ValueChanged);
			// 
			// lblDSPImageGainRX
			// 
			this.lblDSPImageGainRX.Image = null;
			this.lblDSPImageGainRX.Location = new System.Drawing.Point(16, 72);
			this.lblDSPImageGainRX.Name = "lblDSPImageGainRX";
			this.lblDSPImageGainRX.Size = new System.Drawing.Size(48, 16);
			this.lblDSPImageGainRX.TabIndex = 6;
			this.lblDSPImageGainRX.Text = "Gain:";
			// 
			// tbDSPImagePhaseRX
			// 
			this.tbDSPImagePhaseRX.LargeChange = 1;
			this.tbDSPImagePhaseRX.Location = new System.Drawing.Point(72, 24);
			this.tbDSPImagePhaseRX.Maximum = 400;
			this.tbDSPImagePhaseRX.Minimum = -400;
			this.tbDSPImagePhaseRX.Name = "tbDSPImagePhaseRX";
			this.tbDSPImagePhaseRX.Size = new System.Drawing.Size(160, 50);
			this.tbDSPImagePhaseRX.TabIndex = 3;
			this.tbDSPImagePhaseRX.TickFrequency = 50;
			this.toolTip1.SetToolTip(this.tbDSPImagePhaseRX, "Sets the phase offset between the I and Q channels.  ");
			this.tbDSPImagePhaseRX.Scroll += new System.EventHandler(this.tbDSPImagePhaseRX_Scroll);
			// 
			// lblDSPImagePhaseRX
			// 
			this.lblDSPImagePhaseRX.Image = null;
			this.lblDSPImagePhaseRX.Location = new System.Drawing.Point(16, 24);
			this.lblDSPImagePhaseRX.Name = "lblDSPImagePhaseRX";
			this.lblDSPImagePhaseRX.Size = new System.Drawing.Size(48, 16);
			this.lblDSPImagePhaseRX.TabIndex = 5;
			this.lblDSPImagePhaseRX.Text = "Phase:";
			// 
			// tbDSPImageGainRX
			// 
			this.tbDSPImageGainRX.LargeChange = 1;
			this.tbDSPImageGainRX.Location = new System.Drawing.Point(72, 72);
			this.tbDSPImageGainRX.Maximum = 500;
			this.tbDSPImageGainRX.Minimum = -500;
			this.tbDSPImageGainRX.Name = "tbDSPImageGainRX";
			this.tbDSPImageGainRX.Size = new System.Drawing.Size(160, 50);
			this.tbDSPImageGainRX.TabIndex = 4;
			this.tbDSPImageGainRX.TickFrequency = 50;
			this.toolTip1.SetToolTip(this.tbDSPImageGainRX, "Sets the amplitude/gain offset between the I and Q channels.  ");
			this.tbDSPImageGainRX.Scroll += new System.EventHandler(this.tbDSPImageGainRX_Scroll);
			// 
			// grpDSPImageRejectTX
			// 
			this.grpDSPImageRejectTX.Controls.Add(this.checkboxTXImagCal);
			this.grpDSPImageRejectTX.Controls.Add(this.lblDSPGainValTX);
			this.grpDSPImageRejectTX.Controls.Add(this.lblDSPPhaseValTX);
			this.grpDSPImageRejectTX.Controls.Add(this.udDSPImageGainTX);
			this.grpDSPImageRejectTX.Controls.Add(this.udDSPImagePhaseTX);
			this.grpDSPImageRejectTX.Controls.Add(this.lblDSPImageGainTX);
			this.grpDSPImageRejectTX.Controls.Add(this.tbDSPImagePhaseTX);
			this.grpDSPImageRejectTX.Controls.Add(this.lblDSPImagePhaseTX);
			this.grpDSPImageRejectTX.Controls.Add(this.tbDSPImageGainTX);
			this.grpDSPImageRejectTX.Location = new System.Drawing.Point(264, 8);
			this.grpDSPImageRejectTX.Name = "grpDSPImageRejectTX";
			this.grpDSPImageRejectTX.Size = new System.Drawing.Size(240, 184);
			this.grpDSPImageRejectTX.TabIndex = 33;
			this.grpDSPImageRejectTX.TabStop = false;
			this.grpDSPImageRejectTX.Text = "Transmit Rejection";
			// 
			// checkboxTXImagCal
			// 
			this.checkboxTXImagCal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.checkboxTXImagCal.Image = null;
			this.checkboxTXImagCal.Location = new System.Drawing.Point(48, 144);
			this.checkboxTXImagCal.Name = "checkboxTXImagCal";
			this.checkboxTXImagCal.Size = new System.Drawing.Size(136, 16);
			this.checkboxTXImagCal.TabIndex = 37;
			this.checkboxTXImagCal.Text = "Enable TX Imag Tone";
			this.toolTip1.SetToolTip(this.checkboxTXImagCal, "Check this box while in MOX on USB to calibrate the Transmit Rejection using the " +
				"controls above.");
			this.checkboxTXImagCal.CheckedChanged += new System.EventHandler(this.chkTXImagCal_CheckedChanged);
			// 
			// lblDSPGainValTX
			// 
			this.lblDSPGainValTX.Image = null;
			this.lblDSPGainValTX.Location = new System.Drawing.Point(72, 104);
			this.lblDSPGainValTX.Name = "lblDSPGainValTX";
			this.lblDSPGainValTX.Size = new System.Drawing.Size(163, 16);
			this.lblDSPGainValTX.TabIndex = 15;
			this.lblDSPGainValTX.Text = "-500    -250       0       250     500";
			// 
			// lblDSPPhaseValTX
			// 
			this.lblDSPPhaseValTX.Image = null;
			this.lblDSPPhaseValTX.Location = new System.Drawing.Point(72, 56);
			this.lblDSPPhaseValTX.Name = "lblDSPPhaseValTX";
			this.lblDSPPhaseValTX.Size = new System.Drawing.Size(163, 16);
			this.lblDSPPhaseValTX.TabIndex = 14;
			this.lblDSPPhaseValTX.Text = "-400    -200       0       200     400";
			// 
			// udDSPImageGainTX
			// 
			this.udDSPImageGainTX.DecimalPlaces = 2;
			this.udDSPImageGainTX.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   131072});
			this.udDSPImageGainTX.Location = new System.Drawing.Point(16, 88);
			this.udDSPImageGainTX.Maximum = new System.Decimal(new int[] {
																			 500,
																			 0,
																			 0,
																			 0});
			this.udDSPImageGainTX.Minimum = new System.Decimal(new int[] {
																			 500,
																			 0,
																			 0,
																			 -2147483648});
			this.udDSPImageGainTX.Name = "udDSPImageGainTX";
			this.udDSPImageGainTX.Size = new System.Drawing.Size(56, 20);
			this.udDSPImageGainTX.TabIndex = 8;
			this.toolTip1.SetToolTip(this.udDSPImageGainTX, "Sets the amplitude/gain offset between the I and Q channels.  ");
			this.udDSPImageGainTX.Value = new System.Decimal(new int[] {
																		   0,
																		   0,
																		   0,
																		   0});
			this.udDSPImageGainTX.LostFocus += new System.EventHandler(this.udDSPImageGainTX_LostFocus);
			this.udDSPImageGainTX.ValueChanged += new System.EventHandler(this.udDSPImageGainTX_ValueChanged);
			// 
			// udDSPImagePhaseTX
			// 
			this.udDSPImagePhaseTX.DecimalPlaces = 2;
			this.udDSPImagePhaseTX.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				131072});
			this.udDSPImagePhaseTX.Location = new System.Drawing.Point(16, 40);
			this.udDSPImagePhaseTX.Maximum = new System.Decimal(new int[] {
																			  400,
																			  0,
																			  0,
																			  0});
			this.udDSPImagePhaseTX.Minimum = new System.Decimal(new int[] {
																			  400,
																			  0,
																			  0,
																			  -2147483648});
			this.udDSPImagePhaseTX.Name = "udDSPImagePhaseTX";
			this.udDSPImagePhaseTX.Size = new System.Drawing.Size(56, 20);
			this.udDSPImagePhaseTX.TabIndex = 7;
			this.toolTip1.SetToolTip(this.udDSPImagePhaseTX, "Sets the phase offset between the I and Q channels.  ");
			this.udDSPImagePhaseTX.Value = new System.Decimal(new int[] {
																			0,
																			0,
																			0,
																			0});
			this.udDSPImagePhaseTX.LostFocus += new System.EventHandler(this.udDSPImagePhaseTX_LostFocus);
			this.udDSPImagePhaseTX.ValueChanged += new System.EventHandler(this.udDSPImagePhaseTX_ValueChanged);
			// 
			// lblDSPImageGainTX
			// 
			this.lblDSPImageGainTX.Image = null;
			this.lblDSPImageGainTX.Location = new System.Drawing.Point(16, 72);
			this.lblDSPImageGainTX.Name = "lblDSPImageGainTX";
			this.lblDSPImageGainTX.Size = new System.Drawing.Size(48, 16);
			this.lblDSPImageGainTX.TabIndex = 6;
			this.lblDSPImageGainTX.Text = "Gain:";
			// 
			// tbDSPImagePhaseTX
			// 
			this.tbDSPImagePhaseTX.LargeChange = 1;
			this.tbDSPImagePhaseTX.Location = new System.Drawing.Point(72, 24);
			this.tbDSPImagePhaseTX.Maximum = 400;
			this.tbDSPImagePhaseTX.Minimum = -400;
			this.tbDSPImagePhaseTX.Name = "tbDSPImagePhaseTX";
			this.tbDSPImagePhaseTX.Size = new System.Drawing.Size(160, 50);
			this.tbDSPImagePhaseTX.TabIndex = 3;
			this.tbDSPImagePhaseTX.TickFrequency = 50;
			this.toolTip1.SetToolTip(this.tbDSPImagePhaseTX, "Sets the phase offset between the I and Q channels.  ");
			this.tbDSPImagePhaseTX.Scroll += new System.EventHandler(this.tbDSPImagePhaseTX_Scroll);
			// 
			// lblDSPImagePhaseTX
			// 
			this.lblDSPImagePhaseTX.Image = null;
			this.lblDSPImagePhaseTX.Location = new System.Drawing.Point(16, 24);
			this.lblDSPImagePhaseTX.Name = "lblDSPImagePhaseTX";
			this.lblDSPImagePhaseTX.Size = new System.Drawing.Size(48, 16);
			this.lblDSPImagePhaseTX.TabIndex = 5;
			this.lblDSPImagePhaseTX.Text = "Phase:";
			// 
			// tbDSPImageGainTX
			// 
			this.tbDSPImageGainTX.LargeChange = 1;
			this.tbDSPImageGainTX.Location = new System.Drawing.Point(72, 72);
			this.tbDSPImageGainTX.Maximum = 500;
			this.tbDSPImageGainTX.Minimum = -500;
			this.tbDSPImageGainTX.Name = "tbDSPImageGainTX";
			this.tbDSPImageGainTX.Size = new System.Drawing.Size(160, 50);
			this.tbDSPImageGainTX.TabIndex = 4;
			this.tbDSPImageGainTX.TickFrequency = 50;
			this.toolTip1.SetToolTip(this.tbDSPImageGainTX, "Sets the amplitude/gain offset between the I and Q channels.  ");
			this.tbDSPImageGainTX.Scroll += new System.EventHandler(this.tbDSPImageGainTX_Scroll);
			// 
			// tpDSPKeyer
			// 
			this.tpDSPKeyer.Controls.Add(this.grpKeyerConnections);
			this.tpDSPKeyer.Controls.Add(this.grpDSPCWPitch);
			this.tpDSPKeyer.Controls.Add(this.grpDSPKeyerOptions);
			this.tpDSPKeyer.Controls.Add(this.grpDSPKeyerSignalShaping);
			this.tpDSPKeyer.Controls.Add(this.grpDSPKeyerSemiBreakIn);
			this.tpDSPKeyer.Location = new System.Drawing.Point(4, 22);
			this.tpDSPKeyer.Name = "tpDSPKeyer";
			this.tpDSPKeyer.Size = new System.Drawing.Size(592, 318);
			this.tpDSPKeyer.TabIndex = 0;
			this.tpDSPKeyer.Text = "Keyer";
			// 
			// grpKeyerConnections
			// 
			this.grpKeyerConnections.Controls.Add(this.comboKeyerConnKeyLine);
			this.grpKeyerConnections.Controls.Add(this.comboKeyerConnSecondary);
			this.grpKeyerConnections.Controls.Add(this.lblKeyerConnSecondary);
			this.grpKeyerConnections.Controls.Add(this.lblKeyerConnKeyLine);
			this.grpKeyerConnections.Controls.Add(this.comboKeyerConnPTTLine);
			this.grpKeyerConnections.Controls.Add(this.lblKeyerConnPrimary);
			this.grpKeyerConnections.Controls.Add(this.lblKeyerConnPTTLine);
			this.grpKeyerConnections.Controls.Add(this.comboKeyerConnPrimary);
			this.grpKeyerConnections.Location = new System.Drawing.Point(112, 8);
			this.grpKeyerConnections.Name = "grpKeyerConnections";
			this.grpKeyerConnections.Size = new System.Drawing.Size(176, 128);
			this.grpKeyerConnections.TabIndex = 40;
			this.grpKeyerConnections.TabStop = false;
			this.grpKeyerConnections.Text = "Connections";
			// 
			// comboKeyerConnKeyLine
			// 
			this.comboKeyerConnKeyLine.DisplayMember = "None";
			this.comboKeyerConnKeyLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKeyerConnKeyLine.DropDownWidth = 64;
			this.comboKeyerConnKeyLine.Items.AddRange(new object[] {
																	   "None",
																	   "DTR",
																	   "RTS"});
			this.comboKeyerConnKeyLine.Location = new System.Drawing.Point(104, 96);
			this.comboKeyerConnKeyLine.Name = "comboKeyerConnKeyLine";
			this.comboKeyerConnKeyLine.Size = new System.Drawing.Size(64, 21);
			this.comboKeyerConnKeyLine.TabIndex = 51;
			this.toolTip1.SetToolTip(this.comboKeyerConnKeyLine, "Sets the COM port line that triggers the tone on the Keyer Port selected above.");
			this.comboKeyerConnKeyLine.ValueMember = "None";
			this.comboKeyerConnKeyLine.Visible = false;
			this.comboKeyerConnKeyLine.SelectedIndexChanged += new System.EventHandler(this.comboKeyerConnKeyLine_SelectedIndexChanged);
			// 
			// comboKeyerConnSecondary
			// 
			this.comboKeyerConnSecondary.DisplayMember = "None";
			this.comboKeyerConnSecondary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKeyerConnSecondary.DropDownWidth = 64;
			this.comboKeyerConnSecondary.Items.AddRange(new object[] {
																		 "None"});
			this.comboKeyerConnSecondary.Location = new System.Drawing.Point(104, 48);
			this.comboKeyerConnSecondary.Name = "comboKeyerConnSecondary";
			this.comboKeyerConnSecondary.Size = new System.Drawing.Size(64, 21);
			this.comboKeyerConnSecondary.TabIndex = 53;
			this.toolTip1.SetToolTip(this.comboKeyerConnSecondary, "Sets Keyer Input COM port.  This can be an external keyer or a virtual COM port b" +
				"eing driven by a third party program.");
			this.comboKeyerConnSecondary.ValueMember = "None";
			this.comboKeyerConnSecondary.SelectedIndexChanged += new System.EventHandler(this.comboKeyerConnSecondary_SelectedIndexChanged);
			// 
			// lblKeyerConnSecondary
			// 
			this.lblKeyerConnSecondary.Image = null;
			this.lblKeyerConnSecondary.Location = new System.Drawing.Point(16, 48);
			this.lblKeyerConnSecondary.Name = "lblKeyerConnSecondary";
			this.lblKeyerConnSecondary.Size = new System.Drawing.Size(68, 16);
			this.lblKeyerConnSecondary.TabIndex = 52;
			this.lblKeyerConnSecondary.Text = "Secondary:";
			// 
			// lblKeyerConnKeyLine
			// 
			this.lblKeyerConnKeyLine.Image = null;
			this.lblKeyerConnKeyLine.Location = new System.Drawing.Point(16, 96);
			this.lblKeyerConnKeyLine.Name = "lblKeyerConnKeyLine";
			this.lblKeyerConnKeyLine.Size = new System.Drawing.Size(68, 16);
			this.lblKeyerConnKeyLine.TabIndex = 50;
			this.lblKeyerConnKeyLine.Text = "Key Line:";
			this.lblKeyerConnKeyLine.Visible = false;
			// 
			// comboKeyerConnPTTLine
			// 
			this.comboKeyerConnPTTLine.DisplayMember = "None";
			this.comboKeyerConnPTTLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKeyerConnPTTLine.DropDownWidth = 64;
			this.comboKeyerConnPTTLine.Items.AddRange(new object[] {
																	   "None",
																	   "DTR",
																	   "RTS"});
			this.comboKeyerConnPTTLine.Location = new System.Drawing.Point(104, 72);
			this.comboKeyerConnPTTLine.Name = "comboKeyerConnPTTLine";
			this.comboKeyerConnPTTLine.Size = new System.Drawing.Size(64, 21);
			this.comboKeyerConnPTTLine.TabIndex = 49;
			this.toolTip1.SetToolTip(this.comboKeyerConnPTTLine, "Sets the line on the Keyer Port above that triggers PTT.");
			this.comboKeyerConnPTTLine.ValueMember = "None";
			this.comboKeyerConnPTTLine.Visible = false;
			this.comboKeyerConnPTTLine.SelectedIndexChanged += new System.EventHandler(this.comboKeyerConnPTTLine_SelectedIndexChanged);
			// 
			// lblKeyerConnPrimary
			// 
			this.lblKeyerConnPrimary.Image = null;
			this.lblKeyerConnPrimary.Location = new System.Drawing.Point(16, 24);
			this.lblKeyerConnPrimary.Name = "lblKeyerConnPrimary";
			this.lblKeyerConnPrimary.Size = new System.Drawing.Size(88, 16);
			this.lblKeyerConnPrimary.TabIndex = 41;
			this.lblKeyerConnPrimary.Text = "Primary:";
			// 
			// lblKeyerConnPTTLine
			// 
			this.lblKeyerConnPTTLine.Image = null;
			this.lblKeyerConnPTTLine.Location = new System.Drawing.Point(16, 72);
			this.lblKeyerConnPTTLine.Name = "lblKeyerConnPTTLine";
			this.lblKeyerConnPTTLine.Size = new System.Drawing.Size(68, 16);
			this.lblKeyerConnPTTLine.TabIndex = 48;
			this.lblKeyerConnPTTLine.Text = "PTT Line:";
			this.lblKeyerConnPTTLine.Visible = false;
			// 
			// comboKeyerConnPrimary
			// 
			this.comboKeyerConnPrimary.DisplayMember = "LPT";
			this.comboKeyerConnPrimary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKeyerConnPrimary.DropDownWidth = 64;
			this.comboKeyerConnPrimary.Items.AddRange(new object[] {
																	   "SDR",
																	   "Ozy"});
			this.comboKeyerConnPrimary.Location = new System.Drawing.Point(104, 24);
			this.comboKeyerConnPrimary.Name = "comboKeyerConnPrimary";
			this.comboKeyerConnPrimary.Size = new System.Drawing.Size(64, 21);
			this.comboKeyerConnPrimary.TabIndex = 40;
			this.toolTip1.SetToolTip(this.comboKeyerConnPrimary, "Sets Key Paddle Input port");
			this.comboKeyerConnPrimary.ValueMember = "LPT";
			this.comboKeyerConnPrimary.SelectedIndexChanged += new System.EventHandler(this.comboKeyerConnPrimary_SelectedIndexChanged);
			// 
			// grpDSPCWPitch
			// 
			this.grpDSPCWPitch.Controls.Add(this.lblDSPCWPitchFreq);
			this.grpDSPCWPitch.Controls.Add(this.udDSPCWPitch);
			this.grpDSPCWPitch.Location = new System.Drawing.Point(8, 8);
			this.grpDSPCWPitch.Name = "grpDSPCWPitch";
			this.grpDSPCWPitch.Size = new System.Drawing.Size(96, 57);
			this.grpDSPCWPitch.TabIndex = 39;
			this.grpDSPCWPitch.TabStop = false;
			this.grpDSPCWPitch.Text = "CW Pitch (Hz)";
			// 
			// lblDSPCWPitchFreq
			// 
			this.lblDSPCWPitchFreq.Image = null;
			this.lblDSPCWPitchFreq.Location = new System.Drawing.Point(8, 24);
			this.lblDSPCWPitchFreq.Name = "lblDSPCWPitchFreq";
			this.lblDSPCWPitchFreq.Size = new System.Drawing.Size(32, 16);
			this.lblDSPCWPitchFreq.TabIndex = 8;
			this.lblDSPCWPitchFreq.Text = "Freq:";
			// 
			// udDSPCWPitch
			// 
			this.udDSPCWPitch.Increment = new System.Decimal(new int[] {
																		   10,
																		   0,
																		   0,
																		   0});
			this.udDSPCWPitch.Location = new System.Drawing.Point(40, 24);
			this.udDSPCWPitch.Maximum = new System.Decimal(new int[] {
																		 2250,
																		 0,
																		 0,
																		 0});
			this.udDSPCWPitch.Minimum = new System.Decimal(new int[] {
																		 200,
																		 0,
																		 0,
																		 0});
			this.udDSPCWPitch.Name = "udDSPCWPitch";
			this.udDSPCWPitch.Size = new System.Drawing.Size(48, 20);
			this.udDSPCWPitch.TabIndex = 7;
			this.toolTip1.SetToolTip(this.udDSPCWPitch, "Selects the preferred CW tone frequency.");
			this.udDSPCWPitch.Value = new System.Decimal(new int[] {
																	   600,
																	   0,
																	   0,
																	   0});
			this.udDSPCWPitch.LostFocus += new System.EventHandler(this.udDSPCWPitch_LostFocus);
			this.udDSPCWPitch.ValueChanged += new System.EventHandler(this.udDSPCWPitch_ValueChanged);
			// 
			// grpDSPKeyerOptions
			// 
			this.grpDSPKeyerOptions.Controls.Add(this.chkCWKeyerMode);
			this.grpDSPKeyerOptions.Controls.Add(this.chkHiPerfKeyer);
			this.grpDSPKeyerOptions.Controls.Add(this.chkCWKeyerRevPdl);
			this.grpDSPKeyerOptions.Controls.Add(this.chkDSPKeyerDisableMonitor);
			this.grpDSPKeyerOptions.Controls.Add(this.chkCWKeyerIambic);
			this.grpDSPKeyerOptions.Location = new System.Drawing.Point(296, 8);
			this.grpDSPKeyerOptions.Name = "grpDSPKeyerOptions";
			this.grpDSPKeyerOptions.Size = new System.Drawing.Size(128, 152);
			this.grpDSPKeyerOptions.TabIndex = 37;
			this.grpDSPKeyerOptions.TabStop = false;
			this.grpDSPKeyerOptions.Text = "Options";
			// 
			// chkCWKeyerMode
			// 
			this.chkCWKeyerMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkCWKeyerMode.Image = null;
			this.chkCWKeyerMode.Location = new System.Drawing.Point(16, 120);
			this.chkCWKeyerMode.Name = "chkCWKeyerMode";
			this.chkCWKeyerMode.Size = new System.Drawing.Size(96, 16);
			this.chkCWKeyerMode.TabIndex = 40;
			this.chkCWKeyerMode.Text = "Mode B";
			this.toolTip1.SetToolTip(this.chkCWKeyerMode, "Set Keyer Mode");
			this.chkCWKeyerMode.CheckedChanged += new System.EventHandler(this.chkCWKeyerMode_CheckedChanged);
			// 
			// chkHiPerfKeyer
			// 
			this.chkHiPerfKeyer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkHiPerfKeyer.Image = null;
			this.chkHiPerfKeyer.Location = new System.Drawing.Point(16, 96);
			this.chkHiPerfKeyer.Name = "chkHiPerfKeyer";
			this.chkHiPerfKeyer.Size = new System.Drawing.Size(96, 16);
			this.chkHiPerfKeyer.TabIndex = 39;
			this.chkHiPerfKeyer.Text = "High Res.";
			this.toolTip1.SetToolTip(this.chkHiPerfKeyer, "Sets High Res CW keyer clock -- only use with P4.");
			this.chkHiPerfKeyer.CheckedChanged += new System.EventHandler(this.chkHiPerfKeyer_CheckedChanged);
			// 
			// chkCWKeyerRevPdl
			// 
			this.chkCWKeyerRevPdl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkCWKeyerRevPdl.Image = null;
			this.chkCWKeyerRevPdl.Location = new System.Drawing.Point(16, 72);
			this.chkCWKeyerRevPdl.Name = "chkCWKeyerRevPdl";
			this.chkCWKeyerRevPdl.Size = new System.Drawing.Size(88, 16);
			this.chkCWKeyerRevPdl.TabIndex = 38;
			this.chkCWKeyerRevPdl.Text = "Rev. Paddle";
			this.toolTip1.SetToolTip(this.chkCWKeyerRevPdl, "Reverses the input paddle -- Dot becomes Dash and vice versa.");
			this.chkCWKeyerRevPdl.CheckedChanged += new System.EventHandler(this.chkCWKeyerRevPdl_CheckedChanged);
			// 
			// chkDSPKeyerDisableMonitor
			// 
			this.chkDSPKeyerDisableMonitor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkDSPKeyerDisableMonitor.Image = null;
			this.chkDSPKeyerDisableMonitor.Location = new System.Drawing.Point(16, 48);
			this.chkDSPKeyerDisableMonitor.Name = "chkDSPKeyerDisableMonitor";
			this.chkDSPKeyerDisableMonitor.Size = new System.Drawing.Size(104, 16);
			this.chkDSPKeyerDisableMonitor.TabIndex = 37;
			this.chkDSPKeyerDisableMonitor.Text = "Disable Monitor";
			this.toolTip1.SetToolTip(this.chkDSPKeyerDisableMonitor, "Disable the monitor output for CW Keyer");
			this.chkDSPKeyerDisableMonitor.CheckedChanged += new System.EventHandler(this.chkDSPKeyerDisableMonitor_CheckedChanged);
			// 
			// chkCWKeyerIambic
			// 
			this.chkCWKeyerIambic.Checked = true;
			this.chkCWKeyerIambic.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCWKeyerIambic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkCWKeyerIambic.Image = null;
			this.chkCWKeyerIambic.Location = new System.Drawing.Point(16, 24);
			this.chkCWKeyerIambic.Name = "chkCWKeyerIambic";
			this.chkCWKeyerIambic.Size = new System.Drawing.Size(64, 16);
			this.chkCWKeyerIambic.TabIndex = 36;
			this.chkCWKeyerIambic.Text = "Iambic";
			this.toolTip1.SetToolTip(this.chkCWKeyerIambic, "Iambic or Straight Key?");
			this.chkCWKeyerIambic.CheckedChanged += new System.EventHandler(this.chkCWKeyerIambic_CheckedChanged);
			// 
			// grpDSPKeyerSignalShaping
			// 
			this.grpDSPKeyerSignalShaping.Controls.Add(this.udCWKeyerDeBounce);
			this.grpDSPKeyerSignalShaping.Controls.Add(this.lblKeyerDeBounce);
			this.grpDSPKeyerSignalShaping.Controls.Add(this.udCWKeyerWeight);
			this.grpDSPKeyerSignalShaping.Controls.Add(this.lblCWWeight);
			this.grpDSPKeyerSignalShaping.Controls.Add(this.udCWKeyerRamp);
			this.grpDSPKeyerSignalShaping.Controls.Add(this.lblCWRamp);
			this.grpDSPKeyerSignalShaping.Location = new System.Drawing.Point(432, 8);
			this.grpDSPKeyerSignalShaping.Name = "grpDSPKeyerSignalShaping";
			this.grpDSPKeyerSignalShaping.Size = new System.Drawing.Size(136, 128);
			this.grpDSPKeyerSignalShaping.TabIndex = 34;
			this.grpDSPKeyerSignalShaping.TabStop = false;
			this.grpDSPKeyerSignalShaping.Text = "Signal Shaping";
			// 
			// udCWKeyerDeBounce
			// 
			this.udCWKeyerDeBounce.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			this.udCWKeyerDeBounce.Location = new System.Drawing.Point(80, 72);
			this.udCWKeyerDeBounce.Maximum = new System.Decimal(new int[] {
																			  15,
																			  0,
																			  0,
																			  0});
			this.udCWKeyerDeBounce.Minimum = new System.Decimal(new int[] {
																			  1,
																			  0,
																			  0,
																			  0});
			this.udCWKeyerDeBounce.Name = "udCWKeyerDeBounce";
			this.udCWKeyerDeBounce.Size = new System.Drawing.Size(40, 20);
			this.udCWKeyerDeBounce.TabIndex = 42;
			this.udCWKeyerDeBounce.Value = new System.Decimal(new int[] {
																			1,
																			0,
																			0,
																			0});
			this.udCWKeyerDeBounce.Visible = false;
			this.udCWKeyerDeBounce.LostFocus += new System.EventHandler(this.udCWKeyerDeBounce_LostFocus);
			this.udCWKeyerDeBounce.ValueChanged += new System.EventHandler(this.udCWKeyerDeBounce_ValueChanged);
			// 
			// lblKeyerDeBounce
			// 
			this.lblKeyerDeBounce.Image = null;
			this.lblKeyerDeBounce.Location = new System.Drawing.Point(16, 72);
			this.lblKeyerDeBounce.Name = "lblKeyerDeBounce";
			this.lblKeyerDeBounce.Size = new System.Drawing.Size(64, 16);
			this.lblKeyerDeBounce.TabIndex = 41;
			this.lblKeyerDeBounce.Text = "Debounce:";
			this.lblKeyerDeBounce.Visible = false;
			// 
			// udCWKeyerWeight
			// 
			this.udCWKeyerWeight.Increment = new System.Decimal(new int[] {
																			  1,
																			  0,
																			  0,
																			  0});
			this.udCWKeyerWeight.Location = new System.Drawing.Point(80, 24);
			this.udCWKeyerWeight.Maximum = new System.Decimal(new int[] {
																			100,
																			0,
																			0,
																			0});
			this.udCWKeyerWeight.Minimum = new System.Decimal(new int[] {
																			0,
																			0,
																			0,
																			0});
			this.udCWKeyerWeight.Name = "udCWKeyerWeight";
			this.udCWKeyerWeight.Size = new System.Drawing.Size(40, 20);
			this.udCWKeyerWeight.TabIndex = 40;
			this.toolTip1.SetToolTip(this.udCWKeyerWeight, "Sets the weight of the tones when sending Iambic.");
			this.udCWKeyerWeight.Value = new System.Decimal(new int[] {
																		  50,
																		  0,
																		  0,
																		  0});
			this.udCWKeyerWeight.LostFocus += new System.EventHandler(this.udCWKeyerWeight_LostFocus);
			this.udCWKeyerWeight.ValueChanged += new System.EventHandler(this.udCWKeyerWeight_ValueChanged);
			// 
			// lblCWWeight
			// 
			this.lblCWWeight.Image = null;
			this.lblCWWeight.Location = new System.Drawing.Point(16, 24);
			this.lblCWWeight.Name = "lblCWWeight";
			this.lblCWWeight.Size = new System.Drawing.Size(48, 16);
			this.lblCWWeight.TabIndex = 39;
			this.lblCWWeight.Text = "Weight:";
			// 
			// udCWKeyerRamp
			// 
			this.udCWKeyerRamp.Increment = new System.Decimal(new int[] {
																			1,
																			0,
																			0,
																			0});
			this.udCWKeyerRamp.Location = new System.Drawing.Point(80, 48);
			this.udCWKeyerRamp.Maximum = new System.Decimal(new int[] {
																		  25,
																		  0,
																		  0,
																		  0});
			this.udCWKeyerRamp.Minimum = new System.Decimal(new int[] {
																		  0,
																		  0,
																		  0,
																		  0});
			this.udCWKeyerRamp.Name = "udCWKeyerRamp";
			this.udCWKeyerRamp.Size = new System.Drawing.Size(40, 20);
			this.udCWKeyerRamp.TabIndex = 40;
			this.toolTip1.SetToolTip(this.udCWKeyerRamp, "The width of the ramp on the leading and trailing edge of the tone.");
			this.udCWKeyerRamp.Value = new System.Decimal(new int[] {
																		5,
																		0,
																		0,
																		0});
			this.udCWKeyerRamp.LostFocus += new System.EventHandler(this.udCWKeyerRamp_LostFocus);
			this.udCWKeyerRamp.ValueChanged += new System.EventHandler(this.udCWKeyerRamp_ValueChanged);
			// 
			// lblCWRamp
			// 
			this.lblCWRamp.Image = null;
			this.lblCWRamp.Location = new System.Drawing.Point(16, 48);
			this.lblCWRamp.Name = "lblCWRamp";
			this.lblCWRamp.Size = new System.Drawing.Size(64, 16);
			this.lblCWRamp.TabIndex = 39;
			this.lblCWRamp.Text = "Ramp (ms):";
			// 
			// grpDSPKeyerSemiBreakIn
			// 
			this.grpDSPKeyerSemiBreakIn.Controls.Add(this.chkDSPKeyerSemiBreakInEnabled);
			this.grpDSPKeyerSemiBreakIn.Controls.Add(this.lblCWKeyerBreakIn);
			this.grpDSPKeyerSemiBreakIn.Controls.Add(this.udCWKeyerSemiBreakInDelay);
			this.grpDSPKeyerSemiBreakIn.Location = new System.Drawing.Point(8, 144);
			this.grpDSPKeyerSemiBreakIn.Name = "grpDSPKeyerSemiBreakIn";
			this.grpDSPKeyerSemiBreakIn.Size = new System.Drawing.Size(136, 88);
			this.grpDSPKeyerSemiBreakIn.TabIndex = 38;
			this.grpDSPKeyerSemiBreakIn.TabStop = false;
			this.grpDSPKeyerSemiBreakIn.Text = "Semi Break In";
			// 
			// chkDSPKeyerSemiBreakInEnabled
			// 
			this.chkDSPKeyerSemiBreakInEnabled.Checked = true;
			this.chkDSPKeyerSemiBreakInEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDSPKeyerSemiBreakInEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkDSPKeyerSemiBreakInEnabled.Image = null;
			this.chkDSPKeyerSemiBreakInEnabled.Location = new System.Drawing.Point(16, 24);
			this.chkDSPKeyerSemiBreakInEnabled.Name = "chkDSPKeyerSemiBreakInEnabled";
			this.chkDSPKeyerSemiBreakInEnabled.Size = new System.Drawing.Size(80, 16);
			this.chkDSPKeyerSemiBreakInEnabled.TabIndex = 36;
			this.chkDSPKeyerSemiBreakInEnabled.Text = "Enabled";
			this.toolTip1.SetToolTip(this.chkDSPKeyerSemiBreakInEnabled, "Enables Semi Break In operation.");
			this.chkDSPKeyerSemiBreakInEnabled.CheckedChanged += new System.EventHandler(this.chkDSPKeyerSemiBreakInEnabled_CheckedChanged);
			// 
			// lblCWKeyerBreakIn
			// 
			this.lblCWKeyerBreakIn.Image = null;
			this.lblCWKeyerBreakIn.Location = new System.Drawing.Point(8, 48);
			this.lblCWKeyerBreakIn.Name = "lblCWKeyerBreakIn";
			this.lblCWKeyerBreakIn.Size = new System.Drawing.Size(64, 16);
			this.lblCWKeyerBreakIn.TabIndex = 34;
			this.lblCWKeyerBreakIn.Text = "Delay (ms):";
			// 
			// udCWKeyerSemiBreakInDelay
			// 
			this.udCWKeyerSemiBreakInDelay.Increment = new System.Decimal(new int[] {
																						1,
																						0,
																						0,
																						0});
			this.udCWKeyerSemiBreakInDelay.Location = new System.Drawing.Point(72, 48);
			this.udCWKeyerSemiBreakInDelay.Maximum = new System.Decimal(new int[] {
																					  5000,
																					  0,
																					  0,
																					  0});
			this.udCWKeyerSemiBreakInDelay.Minimum = new System.Decimal(new int[] {
																					  150,
																					  0,
																					  0,
																					  0});
			this.udCWKeyerSemiBreakInDelay.Name = "udCWKeyerSemiBreakInDelay";
			this.udCWKeyerSemiBreakInDelay.Size = new System.Drawing.Size(48, 20);
			this.udCWKeyerSemiBreakInDelay.TabIndex = 35;
			this.toolTip1.SetToolTip(this.udCWKeyerSemiBreakInDelay, "Amount of time to stay in TX after the last detected CW signal.");
			this.udCWKeyerSemiBreakInDelay.Value = new System.Decimal(new int[] {
																					400,
																					0,
																					0,
																					0});
			this.udCWKeyerSemiBreakInDelay.LostFocus += new System.EventHandler(this.udCWKeyerSemiBreakInDelay_LostFocus);
			this.udCWKeyerSemiBreakInDelay.ValueChanged += new System.EventHandler(this.udCWKeyerSemiBreakInDelay_ValueChanged);
			// 
			// tpDSPAGCALC
			// 
			this.tpDSPAGCALC.Controls.Add(this.grpDSPLeveler);
			this.tpDSPAGCALC.Controls.Add(this.grpDSPALC);
			this.tpDSPAGCALC.Controls.Add(this.grpDSPAGC);
			this.tpDSPAGCALC.Location = new System.Drawing.Point(4, 22);
			this.tpDSPAGCALC.Name = "tpDSPAGCALC";
			this.tpDSPAGCALC.Size = new System.Drawing.Size(592, 318);
			this.tpDSPAGCALC.TabIndex = 3;
			this.tpDSPAGCALC.Text = "AGC/ALC";
			// 
			// grpDSPLeveler
			// 
			this.grpDSPLeveler.Controls.Add(this.chkDSPLevelerEnabled);
			this.grpDSPLeveler.Controls.Add(this.lblDSPLevelerHangThreshold);
			this.grpDSPLeveler.Controls.Add(this.udDSPLevelerHangTime);
			this.grpDSPLeveler.Controls.Add(this.lblDSPLevelerHangTime);
			this.grpDSPLeveler.Controls.Add(this.udDSPLevelerThreshold);
			this.grpDSPLeveler.Controls.Add(this.udDSPLevelerSlope);
			this.grpDSPLeveler.Controls.Add(this.udDSPLevelerDecay);
			this.grpDSPLeveler.Controls.Add(this.lblDSPLevelerSlope);
			this.grpDSPLeveler.Controls.Add(this.udDSPLevelerAttack);
			this.grpDSPLeveler.Controls.Add(this.lblDSPLevelerDecay);
			this.grpDSPLeveler.Controls.Add(this.lblDSPLevelerAttack);
			this.grpDSPLeveler.Controls.Add(this.lblDSPLevelerThreshold);
			this.grpDSPLeveler.Controls.Add(this.tbDSPLevelerHangThreshold);
			this.grpDSPLeveler.Location = new System.Drawing.Point(264, 8);
			this.grpDSPLeveler.Name = "grpDSPLeveler";
			this.grpDSPLeveler.Size = new System.Drawing.Size(144, 216);
			this.grpDSPLeveler.TabIndex = 39;
			this.grpDSPLeveler.TabStop = false;
			this.grpDSPLeveler.Text = "Leveler";
			// 
			// chkDSPLevelerEnabled
			// 
			this.chkDSPLevelerEnabled.Checked = true;
			this.chkDSPLevelerEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDSPLevelerEnabled.Image = null;
			this.chkDSPLevelerEnabled.Location = new System.Drawing.Point(16, 24);
			this.chkDSPLevelerEnabled.Name = "chkDSPLevelerEnabled";
			this.chkDSPLevelerEnabled.Size = new System.Drawing.Size(104, 16);
			this.chkDSPLevelerEnabled.TabIndex = 42;
			this.chkDSPLevelerEnabled.Text = "Enabled";
			this.toolTip1.SetToolTip(this.chkDSPLevelerEnabled, "Check this box to Enabled (activate) the leveler feature.");
			this.chkDSPLevelerEnabled.CheckedChanged += new System.EventHandler(this.chkDSPLevelerEnabled_CheckedChanged);
			// 
			// lblDSPLevelerHangThreshold
			// 
			this.lblDSPLevelerHangThreshold.Image = null;
			this.lblDSPLevelerHangThreshold.Location = new System.Drawing.Point(8, 168);
			this.lblDSPLevelerHangThreshold.Name = "lblDSPLevelerHangThreshold";
			this.lblDSPLevelerHangThreshold.Size = new System.Drawing.Size(88, 16);
			this.lblDSPLevelerHangThreshold.TabIndex = 41;
			this.lblDSPLevelerHangThreshold.Text = "Hang Threshold:";
			this.lblDSPLevelerHangThreshold.Visible = false;
			// 
			// udDSPLevelerHangTime
			// 
			this.udDSPLevelerHangTime.Increment = new System.Decimal(new int[] {
																				   1,
																				   0,
																				   0,
																				   0});
			this.udDSPLevelerHangTime.Location = new System.Drawing.Point(88, 144);
			this.udDSPLevelerHangTime.Maximum = new System.Decimal(new int[] {
																				 5000,
																				 0,
																				 0,
																				 0});
			this.udDSPLevelerHangTime.Minimum = new System.Decimal(new int[] {
																				 10,
																				 0,
																				 0,
																				 0});
			this.udDSPLevelerHangTime.Name = "udDSPLevelerHangTime";
			this.udDSPLevelerHangTime.Size = new System.Drawing.Size(48, 20);
			this.udDSPLevelerHangTime.TabIndex = 15;
			this.udDSPLevelerHangTime.Value = new System.Decimal(new int[] {
																			   500,
																			   0,
																			   0,
																			   0});
			this.udDSPLevelerHangTime.LostFocus += new System.EventHandler(this.udDSPLevelerHangTime_LostFocus);
			this.udDSPLevelerHangTime.ValueChanged += new System.EventHandler(this.udDSPLevelerHangTime_ValueChanged);
			// 
			// lblDSPLevelerHangTime
			// 
			this.lblDSPLevelerHangTime.Image = null;
			this.lblDSPLevelerHangTime.Location = new System.Drawing.Point(8, 144);
			this.lblDSPLevelerHangTime.Name = "lblDSPLevelerHangTime";
			this.lblDSPLevelerHangTime.Size = new System.Drawing.Size(72, 16);
			this.lblDSPLevelerHangTime.TabIndex = 14;
			this.lblDSPLevelerHangTime.Text = "Hang (ms):";
			// 
			// udDSPLevelerThreshold
			// 
			this.udDSPLevelerThreshold.Increment = new System.Decimal(new int[] {
																					1,
																					0,
																					0,
																					0});
			this.udDSPLevelerThreshold.Location = new System.Drawing.Point(88, 72);
			this.udDSPLevelerThreshold.Maximum = new System.Decimal(new int[] {
																				  20,
																				  0,
																				  0,
																				  0});
			this.udDSPLevelerThreshold.Minimum = new System.Decimal(new int[] {
																				  0,
																				  0,
																				  0,
																				  0});
			this.udDSPLevelerThreshold.Name = "udDSPLevelerThreshold";
			this.udDSPLevelerThreshold.Size = new System.Drawing.Size(40, 20);
			this.udDSPLevelerThreshold.TabIndex = 6;
			this.toolTip1.SetToolTip(this.udDSPLevelerThreshold, "This provides for a “threshold” AGC.  Irrespective of how weak a signal is, no ga" +
				"in over this Max Gain is applied.");
			this.udDSPLevelerThreshold.Value = new System.Decimal(new int[] {
																				15,
																				0,
																				0,
																				0});
			this.udDSPLevelerThreshold.LostFocus += new System.EventHandler(this.udDSPLevelerThreshold_LostFocus);
			this.udDSPLevelerThreshold.ValueChanged += new System.EventHandler(this.udDSPLevelerThreshold_ValueChanged);
			// 
			// udDSPLevelerSlope
			// 
			this.udDSPLevelerSlope.Enabled = false;
			this.udDSPLevelerSlope.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			this.udDSPLevelerSlope.Location = new System.Drawing.Point(88, 48);
			this.udDSPLevelerSlope.Maximum = new System.Decimal(new int[] {
																			  100,
																			  0,
																			  0,
																			  0});
			this.udDSPLevelerSlope.Minimum = new System.Decimal(new int[] {
																			  0,
																			  0,
																			  0,
																			  0});
			this.udDSPLevelerSlope.Name = "udDSPLevelerSlope";
			this.udDSPLevelerSlope.Size = new System.Drawing.Size(40, 20);
			this.udDSPLevelerSlope.TabIndex = 13;
			this.udDSPLevelerSlope.Value = new System.Decimal(new int[] {
																			0,
																			0,
																			0,
																			0});
			this.udDSPLevelerSlope.Visible = false;
			this.udDSPLevelerSlope.LostFocus += new System.EventHandler(this.udDSPLevelerSlope_LostFocus);
			this.udDSPLevelerSlope.ValueChanged += new System.EventHandler(this.udDSPLevelerSlope_ValueChanged);
			// 
			// udDSPLevelerDecay
			// 
			this.udDSPLevelerDecay.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			this.udDSPLevelerDecay.Location = new System.Drawing.Point(88, 120);
			this.udDSPLevelerDecay.Maximum = new System.Decimal(new int[] {
																			  5000,
																			  0,
																			  0,
																			  0});
			this.udDSPLevelerDecay.Minimum = new System.Decimal(new int[] {
																			  10,
																			  0,
																			  0,
																			  0});
			this.udDSPLevelerDecay.Name = "udDSPLevelerDecay";
			this.udDSPLevelerDecay.Size = new System.Drawing.Size(48, 20);
			this.udDSPLevelerDecay.TabIndex = 12;
			this.udDSPLevelerDecay.Value = new System.Decimal(new int[] {
																			500,
																			0,
																			0,
																			0});
			this.udDSPLevelerDecay.LostFocus += new System.EventHandler(this.udDSPLevelerDecay_LostFocus);
			this.udDSPLevelerDecay.ValueChanged += new System.EventHandler(this.udDSPLevelerDecay_ValueChanged);
			// 
			// lblDSPLevelerSlope
			// 
			this.lblDSPLevelerSlope.Enabled = false;
			this.lblDSPLevelerSlope.Image = null;
			this.lblDSPLevelerSlope.Location = new System.Drawing.Point(8, 48);
			this.lblDSPLevelerSlope.Name = "lblDSPLevelerSlope";
			this.lblDSPLevelerSlope.Size = new System.Drawing.Size(64, 16);
			this.lblDSPLevelerSlope.TabIndex = 11;
			this.lblDSPLevelerSlope.Text = "Slope (dB):";
			this.lblDSPLevelerSlope.Visible = false;
			// 
			// udDSPLevelerAttack
			// 
			this.udDSPLevelerAttack.Increment = new System.Decimal(new int[] {
																				 1,
																				 0,
																				 0,
																				 0});
			this.udDSPLevelerAttack.Location = new System.Drawing.Point(88, 96);
			this.udDSPLevelerAttack.Maximum = new System.Decimal(new int[] {
																			   10,
																			   0,
																			   0,
																			   0});
			this.udDSPLevelerAttack.Minimum = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   0});
			this.udDSPLevelerAttack.Name = "udDSPLevelerAttack";
			this.udDSPLevelerAttack.Size = new System.Drawing.Size(40, 20);
			this.udDSPLevelerAttack.TabIndex = 10;
			this.udDSPLevelerAttack.Value = new System.Decimal(new int[] {
																			 2,
																			 0,
																			 0,
																			 0});
			this.udDSPLevelerAttack.LostFocus += new System.EventHandler(this.udDSPLevelerAttack_LostFocus);
			this.udDSPLevelerAttack.ValueChanged += new System.EventHandler(this.udDSPLevelerAttack_ValueChanged);
			// 
			// lblDSPLevelerDecay
			// 
			this.lblDSPLevelerDecay.Image = null;
			this.lblDSPLevelerDecay.Location = new System.Drawing.Point(8, 120);
			this.lblDSPLevelerDecay.Name = "lblDSPLevelerDecay";
			this.lblDSPLevelerDecay.Size = new System.Drawing.Size(72, 16);
			this.lblDSPLevelerDecay.TabIndex = 9;
			this.lblDSPLevelerDecay.Text = "Decay (ms):";
			// 
			// lblDSPLevelerAttack
			// 
			this.lblDSPLevelerAttack.Image = null;
			this.lblDSPLevelerAttack.Location = new System.Drawing.Point(8, 96);
			this.lblDSPLevelerAttack.Name = "lblDSPLevelerAttack";
			this.lblDSPLevelerAttack.Size = new System.Drawing.Size(64, 16);
			this.lblDSPLevelerAttack.TabIndex = 8;
			this.lblDSPLevelerAttack.Text = "Attack (ms):";
			// 
			// lblDSPLevelerThreshold
			// 
			this.lblDSPLevelerThreshold.Image = null;
			this.lblDSPLevelerThreshold.Location = new System.Drawing.Point(8, 72);
			this.lblDSPLevelerThreshold.Name = "lblDSPLevelerThreshold";
			this.lblDSPLevelerThreshold.Size = new System.Drawing.Size(88, 24);
			this.lblDSPLevelerThreshold.TabIndex = 7;
			this.lblDSPLevelerThreshold.Text = "Max.Gain (dB):";
			// 
			// tbDSPLevelerHangThreshold
			// 
			this.tbDSPLevelerHangThreshold.AutoSize = false;
			this.tbDSPLevelerHangThreshold.Enabled = false;
			this.tbDSPLevelerHangThreshold.LargeChange = 1;
			this.tbDSPLevelerHangThreshold.Location = new System.Drawing.Point(8, 184);
			this.tbDSPLevelerHangThreshold.Maximum = 100;
			this.tbDSPLevelerHangThreshold.Name = "tbDSPLevelerHangThreshold";
			this.tbDSPLevelerHangThreshold.Size = new System.Drawing.Size(128, 16);
			this.tbDSPLevelerHangThreshold.TabIndex = 40;
			this.tbDSPLevelerHangThreshold.TickFrequency = 10;
			this.tbDSPLevelerHangThreshold.Visible = false;
			this.tbDSPLevelerHangThreshold.Scroll += new System.EventHandler(this.tbDSPLevelerHangThreshold_Scroll);
			// 
			// grpDSPALC
			// 
			this.grpDSPALC.Controls.Add(this.lblDSPALCHangThreshold);
			this.grpDSPALC.Controls.Add(this.tbDSPALCHangThreshold);
			this.grpDSPALC.Controls.Add(this.udDSPALCHangTime);
			this.grpDSPALC.Controls.Add(this.lblDSPALCHangTime);
			this.grpDSPALC.Controls.Add(this.udDSPALCThreshold);
			this.grpDSPALC.Controls.Add(this.udDSPALCSlope);
			this.grpDSPALC.Controls.Add(this.udDSPALCDecay);
			this.grpDSPALC.Controls.Add(this.lblDSPALCSlope);
			this.grpDSPALC.Controls.Add(this.udDSPALCAttack);
			this.grpDSPALC.Controls.Add(this.lblDSPALCDecay);
			this.grpDSPALC.Controls.Add(this.lblDSPALCAttack);
			this.grpDSPALC.Controls.Add(this.lblDSPALCThreshold);
			this.grpDSPALC.Location = new System.Drawing.Point(416, 8);
			this.grpDSPALC.Name = "grpDSPALC";
			this.grpDSPALC.Size = new System.Drawing.Size(144, 192);
			this.grpDSPALC.TabIndex = 38;
			this.grpDSPALC.TabStop = false;
			this.grpDSPALC.Text = "ALC";
			// 
			// lblDSPALCHangThreshold
			// 
			this.lblDSPALCHangThreshold.Image = null;
			this.lblDSPALCHangThreshold.Location = new System.Drawing.Point(8, 144);
			this.lblDSPALCHangThreshold.Name = "lblDSPALCHangThreshold";
			this.lblDSPALCHangThreshold.Size = new System.Drawing.Size(88, 16);
			this.lblDSPALCHangThreshold.TabIndex = 43;
			this.lblDSPALCHangThreshold.Text = "Hang Threshold:";
			this.lblDSPALCHangThreshold.Visible = false;
			// 
			// tbDSPALCHangThreshold
			// 
			this.tbDSPALCHangThreshold.AutoSize = false;
			this.tbDSPALCHangThreshold.Enabled = false;
			this.tbDSPALCHangThreshold.LargeChange = 1;
			this.tbDSPALCHangThreshold.Location = new System.Drawing.Point(8, 160);
			this.tbDSPALCHangThreshold.Maximum = 100;
			this.tbDSPALCHangThreshold.Name = "tbDSPALCHangThreshold";
			this.tbDSPALCHangThreshold.Size = new System.Drawing.Size(128, 16);
			this.tbDSPALCHangThreshold.TabIndex = 42;
			this.tbDSPALCHangThreshold.TickFrequency = 10;
			this.tbDSPALCHangThreshold.Visible = false;
			this.tbDSPALCHangThreshold.Scroll += new System.EventHandler(this.tbDSPALCHangThreshold_Scroll);
			// 
			// udDSPALCHangTime
			// 
			this.udDSPALCHangTime.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   0});
			this.udDSPALCHangTime.Location = new System.Drawing.Point(88, 120);
			this.udDSPALCHangTime.Maximum = new System.Decimal(new int[] {
																			 5000,
																			 0,
																			 0,
																			 0});
			this.udDSPALCHangTime.Minimum = new System.Decimal(new int[] {
																			 10,
																			 0,
																			 0,
																			 0});
			this.udDSPALCHangTime.Name = "udDSPALCHangTime";
			this.udDSPALCHangTime.Size = new System.Drawing.Size(48, 20);
			this.udDSPALCHangTime.TabIndex = 17;
			this.udDSPALCHangTime.Value = new System.Decimal(new int[] {
																		   500,
																		   0,
																		   0,
																		   0});
			this.udDSPALCHangTime.LostFocus += new System.EventHandler(this.udDSPALCHangTime_LostFocus);
			this.udDSPALCHangTime.ValueChanged += new System.EventHandler(this.udDSPALCHangTime_ValueChanged);
			// 
			// lblDSPALCHangTime
			// 
			this.lblDSPALCHangTime.Image = null;
			this.lblDSPALCHangTime.Location = new System.Drawing.Point(8, 120);
			this.lblDSPALCHangTime.Name = "lblDSPALCHangTime";
			this.lblDSPALCHangTime.Size = new System.Drawing.Size(72, 16);
			this.lblDSPALCHangTime.TabIndex = 16;
			this.lblDSPALCHangTime.Text = "Hang (ms):";
			// 
			// udDSPALCThreshold
			// 
			this.udDSPALCThreshold.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			this.udDSPALCThreshold.Location = new System.Drawing.Point(88, 48);
			this.udDSPALCThreshold.Maximum = new System.Decimal(new int[] {
																			  0,
																			  0,
																			  0,
																			  0});
			this.udDSPALCThreshold.Minimum = new System.Decimal(new int[] {
																			  120,
																			  0,
																			  0,
																			  -2147483648});
			this.udDSPALCThreshold.Name = "udDSPALCThreshold";
			this.udDSPALCThreshold.Size = new System.Drawing.Size(48, 20);
			this.udDSPALCThreshold.TabIndex = 6;
			this.toolTip1.SetToolTip(this.udDSPALCThreshold, "This provides for a “threshold” AGC.  Irrespective of how weak a signal is, no ga" +
				"in over this Max Gain is applied.");
			this.udDSPALCThreshold.Value = new System.Decimal(new int[] {
																			120,
																			0,
																			0,
																			-2147483648});
			this.udDSPALCThreshold.Visible = false;
			this.udDSPALCThreshold.LostFocus += new System.EventHandler(this.udDSPALCThreshold_LostFocus);
			this.udDSPALCThreshold.ValueChanged += new System.EventHandler(this.udDSPALCThreshold_ValueChanged);
			// 
			// udDSPALCSlope
			// 
			this.udDSPALCSlope.Enabled = false;
			this.udDSPALCSlope.Increment = new System.Decimal(new int[] {
																			1,
																			0,
																			0,
																			0});
			this.udDSPALCSlope.Location = new System.Drawing.Point(88, 24);
			this.udDSPALCSlope.Maximum = new System.Decimal(new int[] {
																		  100,
																		  0,
																		  0,
																		  0});
			this.udDSPALCSlope.Minimum = new System.Decimal(new int[] {
																		  0,
																		  0,
																		  0,
																		  0});
			this.udDSPALCSlope.Name = "udDSPALCSlope";
			this.udDSPALCSlope.Size = new System.Drawing.Size(40, 20);
			this.udDSPALCSlope.TabIndex = 13;
			this.udDSPALCSlope.Value = new System.Decimal(new int[] {
																		0,
																		0,
																		0,
																		0});
			this.udDSPALCSlope.Visible = false;
			this.udDSPALCSlope.LostFocus += new System.EventHandler(this.udDSPALCSlope_LostFocus);
			this.udDSPALCSlope.ValueChanged += new System.EventHandler(this.udDSPALCSlope_ValueChanged);
			// 
			// udDSPALCDecay
			// 
			this.udDSPALCDecay.Increment = new System.Decimal(new int[] {
																			1,
																			0,
																			0,
																			0});
			this.udDSPALCDecay.Location = new System.Drawing.Point(88, 96);
			this.udDSPALCDecay.Maximum = new System.Decimal(new int[] {
																		  50,
																		  0,
																		  0,
																		  0});
			this.udDSPALCDecay.Minimum = new System.Decimal(new int[] {
																		  1,
																		  0,
																		  0,
																		  0});
			this.udDSPALCDecay.Name = "udDSPALCDecay";
			this.udDSPALCDecay.Size = new System.Drawing.Size(48, 20);
			this.udDSPALCDecay.TabIndex = 12;
			this.udDSPALCDecay.Value = new System.Decimal(new int[] {
																		10,
																		0,
																		0,
																		0});
			this.udDSPALCDecay.LostFocus += new System.EventHandler(this.udDSPALCDecay_LostFocus);
			this.udDSPALCDecay.ValueChanged += new System.EventHandler(this.udDSPALCDecay_ValueChanged);
			// 
			// lblDSPALCSlope
			// 
			this.lblDSPALCSlope.Image = null;
			this.lblDSPALCSlope.Location = new System.Drawing.Point(8, 24);
			this.lblDSPALCSlope.Name = "lblDSPALCSlope";
			this.lblDSPALCSlope.Size = new System.Drawing.Size(64, 16);
			this.lblDSPALCSlope.TabIndex = 11;
			this.lblDSPALCSlope.Text = "Slope (dB):";
			this.lblDSPALCSlope.Visible = false;
			// 
			// udDSPALCAttack
			// 
			this.udDSPALCAttack.Increment = new System.Decimal(new int[] {
																			 1,
																			 0,
																			 0,
																			 0});
			this.udDSPALCAttack.Location = new System.Drawing.Point(88, 72);
			this.udDSPALCAttack.Maximum = new System.Decimal(new int[] {
																		   10,
																		   0,
																		   0,
																		   0});
			this.udDSPALCAttack.Minimum = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   0});
			this.udDSPALCAttack.Name = "udDSPALCAttack";
			this.udDSPALCAttack.Size = new System.Drawing.Size(40, 20);
			this.udDSPALCAttack.TabIndex = 10;
			this.udDSPALCAttack.Value = new System.Decimal(new int[] {
																		 2,
																		 0,
																		 0,
																		 0});
			this.udDSPALCAttack.LostFocus += new System.EventHandler(this.udDSPALCAttack_LostFocus);
			this.udDSPALCAttack.ValueChanged += new System.EventHandler(this.udDSPALCAttack_ValueChanged);
			// 
			// lblDSPALCDecay
			// 
			this.lblDSPALCDecay.Image = null;
			this.lblDSPALCDecay.Location = new System.Drawing.Point(8, 96);
			this.lblDSPALCDecay.Name = "lblDSPALCDecay";
			this.lblDSPALCDecay.Size = new System.Drawing.Size(72, 16);
			this.lblDSPALCDecay.TabIndex = 9;
			this.lblDSPALCDecay.Text = "Decay (ms):";
			// 
			// lblDSPALCAttack
			// 
			this.lblDSPALCAttack.Image = null;
			this.lblDSPALCAttack.Location = new System.Drawing.Point(8, 72);
			this.lblDSPALCAttack.Name = "lblDSPALCAttack";
			this.lblDSPALCAttack.Size = new System.Drawing.Size(64, 16);
			this.lblDSPALCAttack.TabIndex = 8;
			this.lblDSPALCAttack.Text = "Attack (ms):";
			// 
			// lblDSPALCThreshold
			// 
			this.lblDSPALCThreshold.Image = null;
			this.lblDSPALCThreshold.Location = new System.Drawing.Point(8, 48);
			this.lblDSPALCThreshold.Name = "lblDSPALCThreshold";
			this.lblDSPALCThreshold.Size = new System.Drawing.Size(88, 24);
			this.lblDSPALCThreshold.TabIndex = 7;
			this.lblDSPALCThreshold.Text = "Neg. Gain (dB):";
			this.lblDSPALCThreshold.Visible = false;
			// 
			// grpDSPAGC
			// 
			this.grpDSPAGC.Controls.Add(this.tbDSPAGCHangThreshold);
			this.grpDSPAGC.Controls.Add(this.lblDSPAGCHangThreshold);
			this.grpDSPAGC.Controls.Add(this.lblDSPAGCHangTime);
			this.grpDSPAGC.Controls.Add(this.udDSPAGCHangTime);
			this.grpDSPAGC.Controls.Add(this.udDSPAGCMaxGaindB);
			this.grpDSPAGC.Controls.Add(this.udDSPAGCSlope);
			this.grpDSPAGC.Controls.Add(this.udDSPAGCDecay);
			this.grpDSPAGC.Controls.Add(this.lblDSPAGCSlope);
			this.grpDSPAGC.Controls.Add(this.udDSPAGCAttack);
			this.grpDSPAGC.Controls.Add(this.lblDSPAGCDecay);
			this.grpDSPAGC.Controls.Add(this.lblDSPAGCAttack);
			this.grpDSPAGC.Controls.Add(this.lblDSPAGCMaxGain);
			this.grpDSPAGC.Controls.Add(this.udDSPAGCFixedGaindB);
			this.grpDSPAGC.Controls.Add(this.lblDSPAGCFixed);
			this.grpDSPAGC.Location = new System.Drawing.Point(8, 8);
			this.grpDSPAGC.Name = "grpDSPAGC";
			this.grpDSPAGC.Size = new System.Drawing.Size(168, 232);
			this.grpDSPAGC.TabIndex = 31;
			this.grpDSPAGC.TabStop = false;
			this.grpDSPAGC.Text = "AGC";
			// 
			// tbDSPAGCHangThreshold
			// 
			this.tbDSPAGCHangThreshold.AutoSize = false;
			this.tbDSPAGCHangThreshold.LargeChange = 1;
			this.tbDSPAGCHangThreshold.Location = new System.Drawing.Point(8, 168);
			this.tbDSPAGCHangThreshold.Maximum = 100;
			this.tbDSPAGCHangThreshold.Name = "tbDSPAGCHangThreshold";
			this.tbDSPAGCHangThreshold.Size = new System.Drawing.Size(144, 16);
			this.tbDSPAGCHangThreshold.TabIndex = 47;
			this.tbDSPAGCHangThreshold.TickFrequency = 10;
			this.tbDSPAGCHangThreshold.Scroll += new System.EventHandler(this.tbDSPAGCHangThreshold_Scroll);
			// 
			// lblDSPAGCHangThreshold
			// 
			this.lblDSPAGCHangThreshold.Image = null;
			this.lblDSPAGCHangThreshold.Location = new System.Drawing.Point(8, 144);
			this.lblDSPAGCHangThreshold.Name = "lblDSPAGCHangThreshold";
			this.lblDSPAGCHangThreshold.Size = new System.Drawing.Size(88, 16);
			this.lblDSPAGCHangThreshold.TabIndex = 46;
			this.lblDSPAGCHangThreshold.Text = "Hang Threshold:";
			// 
			// lblDSPAGCHangTime
			// 
			this.lblDSPAGCHangTime.Image = null;
			this.lblDSPAGCHangTime.Location = new System.Drawing.Point(8, 120);
			this.lblDSPAGCHangTime.Name = "lblDSPAGCHangTime";
			this.lblDSPAGCHangTime.Size = new System.Drawing.Size(72, 16);
			this.lblDSPAGCHangTime.TabIndex = 45;
			this.lblDSPAGCHangTime.Text = "Hang (ms):";
			// 
			// udDSPAGCHangTime
			// 
			this.udDSPAGCHangTime.Enabled = false;
			this.udDSPAGCHangTime.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   0});
			this.udDSPAGCHangTime.Location = new System.Drawing.Point(104, 120);
			this.udDSPAGCHangTime.Maximum = new System.Decimal(new int[] {
																			 5000,
																			 0,
																			 0,
																			 0});
			this.udDSPAGCHangTime.Minimum = new System.Decimal(new int[] {
																			 10,
																			 0,
																			 0,
																			 0});
			this.udDSPAGCHangTime.Name = "udDSPAGCHangTime";
			this.udDSPAGCHangTime.Size = new System.Drawing.Size(48, 20);
			this.udDSPAGCHangTime.TabIndex = 44;
			this.udDSPAGCHangTime.Value = new System.Decimal(new int[] {
																		   500,
																		   0,
																		   0,
																		   0});
			this.udDSPAGCHangTime.LostFocus += new System.EventHandler(this.udDSPAGCHangTime_LostFocus);
			this.udDSPAGCHangTime.ValueChanged += new System.EventHandler(this.udDSPAGCHangTime_ValueChanged);
			// 
			// udDSPAGCMaxGaindB
			// 
			this.udDSPAGCMaxGaindB.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			this.udDSPAGCMaxGaindB.Location = new System.Drawing.Point(104, 48);
			this.udDSPAGCMaxGaindB.Maximum = new System.Decimal(new int[] {
																			  120,
																			  0,
																			  0,
																			  0});
			this.udDSPAGCMaxGaindB.Minimum = new System.Decimal(new int[] {
																			  20,
																			  0,
																			  0,
																			  -2147483648});
			this.udDSPAGCMaxGaindB.Name = "udDSPAGCMaxGaindB";
			this.udDSPAGCMaxGaindB.Size = new System.Drawing.Size(40, 20);
			this.udDSPAGCMaxGaindB.TabIndex = 6;
			this.toolTip1.SetToolTip(this.udDSPAGCMaxGaindB, "This provides for a “threshold” AGC.  Irrespective of how weak a signal is, no ga" +
				"in over this Max Gain is applied.");
			this.udDSPAGCMaxGaindB.Value = new System.Decimal(new int[] {
																			90,
																			0,
																			0,
																			0});
			this.udDSPAGCMaxGaindB.LostFocus += new System.EventHandler(this.udDSPAGCMaxGaindB_LostFocus);
			this.udDSPAGCMaxGaindB.ValueChanged += new System.EventHandler(this.udDSPAGCMaxGaindB_ValueChanged);
			// 
			// udDSPAGCSlope
			// 
			this.udDSPAGCSlope.Increment = new System.Decimal(new int[] {
																			1,
																			0,
																			0,
																			0});
			this.udDSPAGCSlope.Location = new System.Drawing.Point(104, 24);
			this.udDSPAGCSlope.Maximum = new System.Decimal(new int[] {
																		  10,
																		  0,
																		  0,
																		  0});
			this.udDSPAGCSlope.Minimum = new System.Decimal(new int[] {
																		  0,
																		  0,
																		  0,
																		  0});
			this.udDSPAGCSlope.Name = "udDSPAGCSlope";
			this.udDSPAGCSlope.Size = new System.Drawing.Size(40, 20);
			this.udDSPAGCSlope.TabIndex = 13;
			this.udDSPAGCSlope.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.udDSPAGCSlope.Value = new System.Decimal(new int[] {
																		0,
																		0,
																		0,
																		0});
			this.udDSPAGCSlope.LostFocus += new System.EventHandler(this.udDSPAGCSlope_LostFocus);
			this.udDSPAGCSlope.ValueChanged += new System.EventHandler(this.udDSPAGCSlope_ValueChanged);
			// 
			// udDSPAGCDecay
			// 
			this.udDSPAGCDecay.Enabled = false;
			this.udDSPAGCDecay.Increment = new System.Decimal(new int[] {
																			1,
																			0,
																			0,
																			0});
			this.udDSPAGCDecay.Location = new System.Drawing.Point(104, 96);
			this.udDSPAGCDecay.Maximum = new System.Decimal(new int[] {
																		  5000,
																		  0,
																		  0,
																		  0});
			this.udDSPAGCDecay.Minimum = new System.Decimal(new int[] {
																		  10,
																		  0,
																		  0,
																		  0});
			this.udDSPAGCDecay.Name = "udDSPAGCDecay";
			this.udDSPAGCDecay.Size = new System.Drawing.Size(48, 20);
			this.udDSPAGCDecay.TabIndex = 12;
			this.udDSPAGCDecay.Value = new System.Decimal(new int[] {
																		500,
																		0,
																		0,
																		0});
			this.udDSPAGCDecay.LostFocus += new System.EventHandler(this.udDSPAGCDecay_LostFocus);
			this.udDSPAGCDecay.ValueChanged += new System.EventHandler(this.udDSPAGCDecay_ValueChanged);
			// 
			// lblDSPAGCSlope
			// 
			this.lblDSPAGCSlope.Image = null;
			this.lblDSPAGCSlope.Location = new System.Drawing.Point(8, 24);
			this.lblDSPAGCSlope.Name = "lblDSPAGCSlope";
			this.lblDSPAGCSlope.Size = new System.Drawing.Size(80, 16);
			this.lblDSPAGCSlope.TabIndex = 11;
			this.lblDSPAGCSlope.Text = "Slope (dB):";
			// 
			// udDSPAGCAttack
			// 
			this.udDSPAGCAttack.Enabled = false;
			this.udDSPAGCAttack.Increment = new System.Decimal(new int[] {
																			 1,
																			 0,
																			 0,
																			 0});
			this.udDSPAGCAttack.Location = new System.Drawing.Point(104, 72);
			this.udDSPAGCAttack.Maximum = new System.Decimal(new int[] {
																		   10,
																		   0,
																		   0,
																		   0});
			this.udDSPAGCAttack.Minimum = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   0});
			this.udDSPAGCAttack.Name = "udDSPAGCAttack";
			this.udDSPAGCAttack.Size = new System.Drawing.Size(40, 20);
			this.udDSPAGCAttack.TabIndex = 10;
			this.udDSPAGCAttack.Value = new System.Decimal(new int[] {
																		 2,
																		 0,
																		 0,
																		 0});
			this.udDSPAGCAttack.LostFocus += new System.EventHandler(this.udDSPAGCAttack_LostFocus);
			this.udDSPAGCAttack.ValueChanged += new System.EventHandler(this.udDSPAGCAttack_ValueChanged);
			// 
			// lblDSPAGCDecay
			// 
			this.lblDSPAGCDecay.Image = null;
			this.lblDSPAGCDecay.Location = new System.Drawing.Point(8, 96);
			this.lblDSPAGCDecay.Name = "lblDSPAGCDecay";
			this.lblDSPAGCDecay.Size = new System.Drawing.Size(72, 16);
			this.lblDSPAGCDecay.TabIndex = 9;
			this.lblDSPAGCDecay.Text = "Decay (ms):";
			// 
			// lblDSPAGCAttack
			// 
			this.lblDSPAGCAttack.Image = null;
			this.lblDSPAGCAttack.Location = new System.Drawing.Point(8, 72);
			this.lblDSPAGCAttack.Name = "lblDSPAGCAttack";
			this.lblDSPAGCAttack.Size = new System.Drawing.Size(64, 16);
			this.lblDSPAGCAttack.TabIndex = 8;
			this.lblDSPAGCAttack.Text = "Attack (ms):";
			// 
			// lblDSPAGCMaxGain
			// 
			this.lblDSPAGCMaxGain.Image = null;
			this.lblDSPAGCMaxGain.Location = new System.Drawing.Point(8, 48);
			this.lblDSPAGCMaxGain.Name = "lblDSPAGCMaxGain";
			this.lblDSPAGCMaxGain.Size = new System.Drawing.Size(88, 24);
			this.lblDSPAGCMaxGain.TabIndex = 7;
			this.lblDSPAGCMaxGain.Text = "Max Gain (dB):";
			// 
			// udDSPAGCFixedGaindB
			// 
			this.udDSPAGCFixedGaindB.Increment = new System.Decimal(new int[] {
																				  1,
																				  0,
																				  0,
																				  0});
			this.udDSPAGCFixedGaindB.Location = new System.Drawing.Point(104, 200);
			this.udDSPAGCFixedGaindB.Maximum = new System.Decimal(new int[] {
																				120,
																				0,
																				0,
																				0});
			this.udDSPAGCFixedGaindB.Minimum = new System.Decimal(new int[] {
																				20,
																				0,
																				0,
																				-2147483648});
			this.udDSPAGCFixedGaindB.Name = "udDSPAGCFixedGaindB";
			this.udDSPAGCFixedGaindB.Size = new System.Drawing.Size(40, 20);
			this.udDSPAGCFixedGaindB.TabIndex = 4;
			this.toolTip1.SetToolTip(this.udDSPAGCFixedGaindB, "When you choose Fixed AGC on the front panel, this number is used to multiply the" +
				" signal.");
			this.udDSPAGCFixedGaindB.Value = new System.Decimal(new int[] {
																			  20,
																			  0,
																			  0,
																			  0});
			this.udDSPAGCFixedGaindB.LostFocus += new System.EventHandler(this.udDSPAGCFixedGaindB_LostFocus);
			this.udDSPAGCFixedGaindB.ValueChanged += new System.EventHandler(this.udDSPAGCFixedGaindB_ValueChanged);
			// 
			// lblDSPAGCFixed
			// 
			this.lblDSPAGCFixed.Image = null;
			this.lblDSPAGCFixed.Location = new System.Drawing.Point(8, 200);
			this.lblDSPAGCFixed.Name = "lblDSPAGCFixed";
			this.lblDSPAGCFixed.Size = new System.Drawing.Size(88, 16);
			this.lblDSPAGCFixed.TabIndex = 5;
			this.lblDSPAGCFixed.Text = "Fixed Gain (dB):";
			// 
			// tpTransmit
			// 
			this.tpTransmit.Controls.Add(this.grpTXAM);
			this.tpTransmit.Controls.Add(this.grpTXMonitor);
			this.tpTransmit.Controls.Add(this.grpTXVOX);
			this.tpTransmit.Controls.Add(this.grpTXNoiseGate);
			this.tpTransmit.Controls.Add(this.grpTXProfile);
			this.tpTransmit.Controls.Add(this.grpPATune);
			this.tpTransmit.Controls.Add(this.grpTXCompression);
			this.tpTransmit.Controls.Add(this.grpTXFilter);
			this.tpTransmit.Controls.Add(this.chkDCBlock);
			this.tpTransmit.Location = new System.Drawing.Point(4, 22);
			this.tpTransmit.Name = "tpTransmit";
			this.tpTransmit.Size = new System.Drawing.Size(584, 286);
			this.tpTransmit.TabIndex = 5;
			this.tpTransmit.Text = "Transmit";
			// 
			// grpTXAM
			// 
			this.grpTXAM.Controls.Add(this.lblTXAMCarrierLevel);
			this.grpTXAM.Controls.Add(this.udTXAMCarrierLevel);
			this.grpTXAM.Location = new System.Drawing.Point(368, 200);
			this.grpTXAM.Name = "grpTXAM";
			this.grpTXAM.Size = new System.Drawing.Size(144, 56);
			this.grpTXAM.TabIndex = 52;
			this.grpTXAM.TabStop = false;
			this.grpTXAM.Text = "AM";
			// 
			// lblTXAMCarrierLevel
			// 
			this.lblTXAMCarrierLevel.Image = null;
			this.lblTXAMCarrierLevel.Location = new System.Drawing.Point(8, 24);
			this.lblTXAMCarrierLevel.Name = "lblTXAMCarrierLevel";
			this.lblTXAMCarrierLevel.Size = new System.Drawing.Size(72, 16);
			this.lblTXAMCarrierLevel.TabIndex = 5;
			this.lblTXAMCarrierLevel.Text = "Carrier Level:";
			// 
			// udTXAMCarrierLevel
			// 
			this.udTXAMCarrierLevel.DecimalPlaces = 1;
			this.udTXAMCarrierLevel.Increment = new System.Decimal(new int[] {
																				 1,
																				 0,
																				 0,
																				 65536});
			this.udTXAMCarrierLevel.Location = new System.Drawing.Point(80, 24);
			this.udTXAMCarrierLevel.Maximum = new System.Decimal(new int[] {
																			   100,
																			   0,
																			   0,
																			   0});
			this.udTXAMCarrierLevel.Minimum = new System.Decimal(new int[] {
																			   0,
																			   0,
																			   0,
																			   0});
			this.udTXAMCarrierLevel.Name = "udTXAMCarrierLevel";
			this.udTXAMCarrierLevel.Size = new System.Drawing.Size(56, 20);
			this.udTXAMCarrierLevel.TabIndex = 4;
			this.toolTip1.SetToolTip(this.udTXAMCarrierLevel, "Adjusts the carrier level on AM (pecentage of full 1/4 carrier) .");
			this.udTXAMCarrierLevel.Value = new System.Decimal(new int[] {
																			 100,
																			 0,
																			 0,
																			 0});
			this.udTXAMCarrierLevel.LostFocus += new System.EventHandler(this.udTXAMCarrierLevel_LostFocus);
			this.udTXAMCarrierLevel.ValueChanged += new System.EventHandler(this.udTXAMCarrierLevel_ValueChanged);
			// 
			// grpTXMonitor
			// 
			this.grpTXMonitor.Controls.Add(this.lblTXAF);
			this.grpTXMonitor.Controls.Add(this.udTXAF);
			this.grpTXMonitor.Location = new System.Drawing.Point(152, 184);
			this.grpTXMonitor.Name = "grpTXMonitor";
			this.grpTXMonitor.Size = new System.Drawing.Size(120, 56);
			this.grpTXMonitor.TabIndex = 51;
			this.grpTXMonitor.TabStop = false;
			this.grpTXMonitor.Text = "Monitor";
			// 
			// lblTXAF
			// 
			this.lblTXAF.Image = null;
			this.lblTXAF.Location = new System.Drawing.Point(8, 24);
			this.lblTXAF.Name = "lblTXAF";
			this.lblTXAF.Size = new System.Drawing.Size(40, 16);
			this.lblTXAF.TabIndex = 5;
			this.lblTXAF.Text = "TX AF:";
			// 
			// udTXAF
			// 
			this.udTXAF.Increment = new System.Decimal(new int[] {
																	 1,
																	 0,
																	 0,
																	 0});
			this.udTXAF.Location = new System.Drawing.Point(56, 24);
			this.udTXAF.Maximum = new System.Decimal(new int[] {
																   100,
																   0,
																   0,
																   0});
			this.udTXAF.Minimum = new System.Decimal(new int[] {
																   0,
																   0,
																   0,
																   0});
			this.udTXAF.Name = "udTXAF";
			this.udTXAF.Size = new System.Drawing.Size(48, 20);
			this.udTXAF.TabIndex = 4;
			this.toolTip1.SetToolTip(this.udTXAF, "AF value to use when in TX mode (with the Delta 44 only).");
			this.udTXAF.Value = new System.Decimal(new int[] {
																 50,
																 0,
																 0,
																 0});
			this.udTXAF.LostFocus += new System.EventHandler(this.udTXAF_LostFocus);
			this.udTXAF.ValueChanged += new System.EventHandler(this.udTXAF_ValueChanged);
			// 
			// grpTXVOX
			// 
			this.grpTXVOX.Controls.Add(this.lblTXVOXHangTime);
			this.grpTXVOX.Controls.Add(this.udTXVOXHangTime);
			this.grpTXVOX.Controls.Add(this.chkTXVOXEnabled);
			this.grpTXVOX.Controls.Add(this.lblTXVOXThreshold);
			this.grpTXVOX.Controls.Add(this.udTXVOXThreshold);
			this.grpTXVOX.Location = new System.Drawing.Point(8, 184);
			this.grpTXVOX.Name = "grpTXVOX";
			this.grpTXVOX.Size = new System.Drawing.Size(136, 96);
			this.grpTXVOX.TabIndex = 50;
			this.grpTXVOX.TabStop = false;
			this.grpTXVOX.Text = "VOX";
			// 
			// lblTXVOXHangTime
			// 
			this.lblTXVOXHangTime.Image = null;
			this.lblTXVOXHangTime.Location = new System.Drawing.Point(8, 72);
			this.lblTXVOXHangTime.Name = "lblTXVOXHangTime";
			this.lblTXVOXHangTime.Size = new System.Drawing.Size(64, 16);
			this.lblTXVOXHangTime.TabIndex = 52;
			this.lblTXVOXHangTime.Text = "Delay (ms):";
			// 
			// udTXVOXHangTime
			// 
			this.udTXVOXHangTime.Increment = new System.Decimal(new int[] {
																			  1,
																			  0,
																			  0,
																			  0});
			this.udTXVOXHangTime.Location = new System.Drawing.Point(72, 72);
			this.udTXVOXHangTime.Maximum = new System.Decimal(new int[] {
																			10000,
																			0,
																			0,
																			0});
			this.udTXVOXHangTime.Minimum = new System.Decimal(new int[] {
																			0,
																			0,
																			0,
																			0});
			this.udTXVOXHangTime.Name = "udTXVOXHangTime";
			this.udTXVOXHangTime.Size = new System.Drawing.Size(56, 20);
			this.udTXVOXHangTime.TabIndex = 51;
			this.toolTip1.SetToolTip(this.udTXVOXHangTime, "The amount of time in ms to stay in TX mode after the last signal above the thres" +
				"hold.");
			this.udTXVOXHangTime.Value = new System.Decimal(new int[] {
																		  250,
																		  0,
																		  0,
																		  0});
			this.udTXVOXHangTime.LostFocus += new System.EventHandler(this.udTXVOXHangTime_LostFocus);
			this.udTXVOXHangTime.ValueChanged += new System.EventHandler(this.udTXVOXHangTime_ValueChanged);
			// 
			// chkTXVOXEnabled
			// 
			this.chkTXVOXEnabled.Image = null;
			this.chkTXVOXEnabled.Location = new System.Drawing.Point(16, 24);
			this.chkTXVOXEnabled.Name = "chkTXVOXEnabled";
			this.chkTXVOXEnabled.Size = new System.Drawing.Size(72, 16);
			this.chkTXVOXEnabled.TabIndex = 50;
			this.chkTXVOXEnabled.Text = "Enabled";
			this.toolTip1.SetToolTip(this.chkTXVOXEnabled, "Enables VOX operation using the parameters below.");
			this.chkTXVOXEnabled.CheckedChanged += new System.EventHandler(this.chkTXVOXEnabled_CheckedChanged);
			// 
			// lblTXVOXThreshold
			// 
			this.lblTXVOXThreshold.Image = null;
			this.lblTXVOXThreshold.Location = new System.Drawing.Point(8, 48);
			this.lblTXVOXThreshold.Name = "lblTXVOXThreshold";
			this.lblTXVOXThreshold.Size = new System.Drawing.Size(64, 16);
			this.lblTXVOXThreshold.TabIndex = 5;
			this.lblTXVOXThreshold.Text = "Sensitivity:";
			// 
			// udTXVOXThreshold
			// 
			this.udTXVOXThreshold.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   0});
			this.udTXVOXThreshold.Location = new System.Drawing.Point(72, 48);
			this.udTXVOXThreshold.Maximum = new System.Decimal(new int[] {
																			 1000,
																			 0,
																			 0,
																			 0});
			this.udTXVOXThreshold.Minimum = new System.Decimal(new int[] {
																			 0,
																			 0,
																			 0,
																			 0});
			this.udTXVOXThreshold.Name = "udTXVOXThreshold";
			this.udTXVOXThreshold.Size = new System.Drawing.Size(48, 20);
			this.udTXVOXThreshold.TabIndex = 4;
			this.toolTip1.SetToolTip(this.udTXVOXThreshold, "Numeric sample value above which triggers the VOX circuit.");
			this.udTXVOXThreshold.Value = new System.Decimal(new int[] {
																		   90,
																		   0,
																		   0,
																		   0});
			this.udTXVOXThreshold.LostFocus += new System.EventHandler(this.udTXVOXThreshold_LostFocus);
			this.udTXVOXThreshold.ValueChanged += new System.EventHandler(this.udTXVOXThreshold_ValueChanged);
			// 
			// grpTXNoiseGate
			// 
			this.grpTXNoiseGate.Controls.Add(this.chkTXNoiseGateEnabled);
			this.grpTXNoiseGate.Controls.Add(this.udTXNoiseGate);
			this.grpTXNoiseGate.Controls.Add(this.lblTXNoiseGateThreshold);
			this.grpTXNoiseGate.Location = new System.Drawing.Point(152, 96);
			this.grpTXNoiseGate.Name = "grpTXNoiseGate";
			this.grpTXNoiseGate.Size = new System.Drawing.Size(144, 80);
			this.grpTXNoiseGate.TabIndex = 49;
			this.grpTXNoiseGate.TabStop = false;
			this.grpTXNoiseGate.Text = "Noise Gate";
			// 
			// chkTXNoiseGateEnabled
			// 
			this.chkTXNoiseGateEnabled.Image = null;
			this.chkTXNoiseGateEnabled.Location = new System.Drawing.Point(16, 24);
			this.chkTXNoiseGateEnabled.Name = "chkTXNoiseGateEnabled";
			this.chkTXNoiseGateEnabled.Size = new System.Drawing.Size(72, 16);
			this.chkTXNoiseGateEnabled.TabIndex = 49;
			this.chkTXNoiseGateEnabled.Text = "Enabled";
			this.toolTip1.SetToolTip(this.chkTXNoiseGateEnabled, "Enables the noise gate to operate using the threshold set below.");
			this.chkTXNoiseGateEnabled.CheckedChanged += new System.EventHandler(this.chkTXNoiseGateEnabled_CheckedChanged);
			// 
			// udTXNoiseGate
			// 
			this.udTXNoiseGate.Increment = new System.Decimal(new int[] {
																			1,
																			0,
																			0,
																			0});
			this.udTXNoiseGate.Location = new System.Drawing.Point(88, 48);
			this.udTXNoiseGate.Maximum = new System.Decimal(new int[] {
																		  0,
																		  0,
																		  0,
																		  0});
			this.udTXNoiseGate.Minimum = new System.Decimal(new int[] {
																		  160,
																		  0,
																		  0,
																		  -2147483648});
			this.udTXNoiseGate.Name = "udTXNoiseGate";
			this.udTXNoiseGate.Size = new System.Drawing.Size(48, 20);
			this.udTXNoiseGate.TabIndex = 4;
			this.toolTip1.SetToolTip(this.udTXNoiseGate, "Signal level in dB above which to transmit audio.");
			this.udTXNoiseGate.Value = new System.Decimal(new int[] {
																		40,
																		0,
																		0,
																		-2147483648});
			this.udTXNoiseGate.LostFocus += new System.EventHandler(this.udTXNoiseGate_LostFocus);
			this.udTXNoiseGate.ValueChanged += new System.EventHandler(this.udTXNoiseGate_ValueChanged);
			// 
			// lblTXNoiseGateThreshold
			// 
			this.lblTXNoiseGateThreshold.Image = null;
			this.lblTXNoiseGateThreshold.Location = new System.Drawing.Point(8, 48);
			this.lblTXNoiseGateThreshold.Name = "lblTXNoiseGateThreshold";
			this.lblTXNoiseGateThreshold.Size = new System.Drawing.Size(82, 23);
			this.lblTXNoiseGateThreshold.TabIndex = 5;
			this.lblTXNoiseGateThreshold.Text = "Threshold (dB):";
			// 
			// grpTXProfile
			// 
			this.grpTXProfile.Controls.Add(this.btnTXProfileDelete);
			this.grpTXProfile.Controls.Add(this.btnTXProfileSave);
			this.grpTXProfile.Controls.Add(this.comboTXProfileName);
			this.grpTXProfile.Location = new System.Drawing.Point(8, 8);
			this.grpTXProfile.Name = "grpTXProfile";
			this.grpTXProfile.Size = new System.Drawing.Size(136, 80);
			this.grpTXProfile.TabIndex = 23;
			this.grpTXProfile.TabStop = false;
			this.grpTXProfile.Text = "Profiles";
			// 
			// btnTXProfileDelete
			// 
			this.btnTXProfileDelete.Location = new System.Drawing.Point(72, 48);
			this.btnTXProfileDelete.Name = "btnTXProfileDelete";
			this.btnTXProfileDelete.Size = new System.Drawing.Size(48, 21);
			this.btnTXProfileDelete.TabIndex = 2;
			this.btnTXProfileDelete.Text = "Delete";
			this.toolTip1.SetToolTip(this.btnTXProfileDelete, "Click to delete the currently selected TX Profile.");
			this.btnTXProfileDelete.Click += new System.EventHandler(this.btnTXProfileDelete_Click);
			// 
			// btnTXProfileSave
			// 
			this.btnTXProfileSave.Location = new System.Drawing.Point(16, 48);
			this.btnTXProfileSave.Name = "btnTXProfileSave";
			this.btnTXProfileSave.Size = new System.Drawing.Size(48, 21);
			this.btnTXProfileSave.TabIndex = 1;
			this.btnTXProfileSave.Text = "Save";
			this.toolTip1.SetToolTip(this.btnTXProfileSave, "Click to save the current settings to a TX Profile.");
			this.btnTXProfileSave.Click += new System.EventHandler(this.btnTXProfileSave_Click);
			// 
			// comboTXProfileName
			// 
			this.comboTXProfileName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTXProfileName.DropDownWidth = 104;
			this.comboTXProfileName.Location = new System.Drawing.Point(16, 24);
			this.comboTXProfileName.Name = "comboTXProfileName";
			this.comboTXProfileName.Size = new System.Drawing.Size(104, 21);
			this.comboTXProfileName.TabIndex = 0;
			this.toolTip1.SetToolTip(this.comboTXProfileName, "Sets the current Transmit Profile to be used.");
			this.comboTXProfileName.SelectedIndexChanged += new System.EventHandler(this.comboTXProfileName_SelectedIndexChanged);
			// 
			// grpPATune
			// 
			this.grpPATune.Controls.Add(this.lblTransmitTunePower);
			this.grpPATune.Controls.Add(this.udTXTunePower);
			this.grpPATune.Location = new System.Drawing.Point(8, 96);
			this.grpPATune.Name = "grpPATune";
			this.grpPATune.Size = new System.Drawing.Size(136, 56);
			this.grpPATune.TabIndex = 22;
			this.grpPATune.TabStop = false;
			this.grpPATune.Text = "Tune";
			// 
			// lblTransmitTunePower
			// 
			this.lblTransmitTunePower.Image = null;
			this.lblTransmitTunePower.Location = new System.Drawing.Point(8, 24);
			this.lblTransmitTunePower.Name = "lblTransmitTunePower";
			this.lblTransmitTunePower.Size = new System.Drawing.Size(64, 16);
			this.lblTransmitTunePower.TabIndex = 5;
			this.lblTransmitTunePower.Text = "Power (W):";
			// 
			// udTXTunePower
			// 
			this.udTXTunePower.Increment = new System.Decimal(new int[] {
																			1,
																			0,
																			0,
																			0});
			this.udTXTunePower.Location = new System.Drawing.Point(72, 24);
			this.udTXTunePower.Maximum = new System.Decimal(new int[] {
																		  100,
																		  0,
																		  0,
																		  0});
			this.udTXTunePower.Minimum = new System.Decimal(new int[] {
																		  0,
																		  0,
																		  0,
																		  0});
			this.udTXTunePower.Name = "udTXTunePower";
			this.udTXTunePower.Size = new System.Drawing.Size(48, 20);
			this.udTXTunePower.TabIndex = 4;
			this.toolTip1.SetToolTip(this.udTXTunePower, "Power used when using the TUN button on the front panel.");
			this.udTXTunePower.Value = new System.Decimal(new int[] {
																		10,
																		0,
																		0,
																		0});
			this.udTXTunePower.LostFocus += new System.EventHandler(this.udTXTunePower_LostFocus);
			this.udTXTunePower.ValueChanged += new System.EventHandler(this.udTransmitTunePower_ValueChanged);
			// 
			// grpTXCompression
			// 
			this.grpTXCompression.Controls.Add(this.lblCompand);
			this.grpTXCompression.Controls.Add(this.tbTXCompander);
			this.grpTXCompression.Controls.Add(this.udTXCompander);
			this.grpTXCompression.Controls.Add(this.lblTXCompander);
			this.grpTXCompression.Controls.Add(this.udTXFFCompression);
			this.grpTXCompression.Controls.Add(this.lblTXFFVal);
			this.grpTXCompression.Controls.Add(this.lblTransmitFeedForward);
			this.grpTXCompression.Controls.Add(this.tbTXFFCompression);
			this.grpTXCompression.Location = new System.Drawing.Point(368, 8);
			this.grpTXCompression.Name = "grpTXCompression";
			this.grpTXCompression.Size = new System.Drawing.Size(192, 184);
			this.grpTXCompression.TabIndex = 21;
			this.grpTXCompression.TabStop = false;
			this.grpTXCompression.Text = "Transmit Compression";
			// 
			// lblCompand
			// 
			this.lblCompand.Image = null;
			this.lblCompand.Location = new System.Drawing.Point(25, 160);
			this.lblCompand.Name = "lblCompand";
			this.lblCompand.Size = new System.Drawing.Size(152, 16);
			this.lblCompand.TabIndex = 35;
			this.lblCompand.Text = " 0                   5                  10";
			// 
			// tbTXCompander
			// 
			this.tbTXCompander.Location = new System.Drawing.Point(20, 128);
			this.tbTXCompander.Name = "tbTXCompander";
			this.tbTXCompander.Size = new System.Drawing.Size(152, 50);
			this.tbTXCompander.TabIndex = 34;
			this.toolTip1.SetToolTip(this.tbTXCompander, "Sets the compander value.");
			this.tbTXCompander.Value = 3;
			this.tbTXCompander.Scroll += new System.EventHandler(this.tbTXCmpd_Scroll);
			// 
			// udTXCompander
			// 
			this.udTXCompander.Increment = new System.Decimal(new int[] {
																			1,
																			0,
																			0,
																			0});
			this.udTXCompander.Location = new System.Drawing.Point(104, 104);
			this.udTXCompander.Maximum = new System.Decimal(new int[] {
																		  10,
																		  0,
																		  0,
																		  0});
			this.udTXCompander.Minimum = new System.Decimal(new int[] {
																		  0,
																		  0,
																		  0,
																		  0});
			this.udTXCompander.Name = "udTXCompander";
			this.udTXCompander.Size = new System.Drawing.Size(48, 20);
			this.udTXCompander.TabIndex = 33;
			this.toolTip1.SetToolTip(this.udTXCompander, "Sets the compander value.");
			this.udTXCompander.Value = new System.Decimal(new int[] {
																		3,
																		0,
																		0,
																		0});
			this.udTXCompander.LostFocus += new System.EventHandler(this.udTXCompander_LostFocus);
			this.udTXCompander.ValueChanged += new System.EventHandler(this.udTXCmpd_ValueChanged);
			// 
			// lblTXCompander
			// 
			this.lblTXCompander.Image = null;
			this.lblTXCompander.Location = new System.Drawing.Point(16, 104);
			this.lblTXCompander.Name = "lblTXCompander";
			this.lblTXCompander.Size = new System.Drawing.Size(72, 23);
			this.lblTXCompander.TabIndex = 32;
			this.lblTXCompander.Text = "Compand";
			// 
			// udTXFFCompression
			// 
			this.udTXFFCompression.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			this.udTXFFCompression.Location = new System.Drawing.Point(104, 24);
			this.udTXFFCompression.Maximum = new System.Decimal(new int[] {
																			  20,
																			  0,
																			  0,
																			  0});
			this.udTXFFCompression.Minimum = new System.Decimal(new int[] {
																			  0,
																			  0,
																			  0,
																			  0});
			this.udTXFFCompression.Name = "udTXFFCompression";
			this.udTXFFCompression.Size = new System.Drawing.Size(48, 20);
			this.udTXFFCompression.TabIndex = 31;
			this.toolTip1.SetToolTip(this.udTXFFCompression, "Sets the compression value allowing more average power without raising the peaks." +
				"");
			this.udTXFFCompression.Value = new System.Decimal(new int[] {
																			3,
																			0,
																			0,
																			0});
			this.udTXFFCompression.LostFocus += new System.EventHandler(this.udTXFFCompression_LostFocus);
			this.udTXFFCompression.ValueChanged += new System.EventHandler(this.udTXFFCompression_ValueChanged);
			// 
			// lblTXFFVal
			// 
			this.lblTXFFVal.Image = null;
			this.lblTXFFVal.Location = new System.Drawing.Point(32, 80);
			this.lblTXFFVal.Name = "lblTXFFVal";
			this.lblTXFFVal.Size = new System.Drawing.Size(152, 16);
			this.lblTXFFVal.TabIndex = 29;
			this.lblTXFFVal.Text = "0dB             10dB            20dB";
			// 
			// lblTransmitFeedForward
			// 
			this.lblTransmitFeedForward.Image = null;
			this.lblTransmitFeedForward.Location = new System.Drawing.Point(16, 24);
			this.lblTransmitFeedForward.Name = "lblTransmitFeedForward";
			this.lblTransmitFeedForward.Size = new System.Drawing.Size(72, 23);
			this.lblTransmitFeedForward.TabIndex = 23;
			this.lblTransmitFeedForward.Text = "FeedForward";
			// 
			// tbTXFFCompression
			// 
			this.tbTXFFCompression.Location = new System.Drawing.Point(24, 48);
			this.tbTXFFCompression.Maximum = 20;
			this.tbTXFFCompression.Name = "tbTXFFCompression";
			this.tbTXFFCompression.Size = new System.Drawing.Size(152, 50);
			this.tbTXFFCompression.TabIndex = 21;
			this.tbTXFFCompression.TickFrequency = 2;
			this.toolTip1.SetToolTip(this.tbTXFFCompression, "Sets the compression value allowing more average power without raising the peaks." +
				"");
			this.tbTXFFCompression.Value = 3;
			this.tbTXFFCompression.Scroll += new System.EventHandler(this.tbTXFFCompression_Scroll);
			// 
			// grpTXFilter
			// 
			this.grpTXFilter.Controls.Add(this.lblTXFilterHigh);
			this.grpTXFilter.Controls.Add(this.udTXFilterLow);
			this.grpTXFilter.Controls.Add(this.lblTXFilterLow);
			this.grpTXFilter.Controls.Add(this.udTXFilterHigh);
			this.grpTXFilter.Location = new System.Drawing.Point(152, 8);
			this.grpTXFilter.Name = "grpTXFilter";
			this.grpTXFilter.Size = new System.Drawing.Size(128, 80);
			this.grpTXFilter.TabIndex = 19;
			this.grpTXFilter.TabStop = false;
			this.grpTXFilter.Text = "Transmit Filter";
			// 
			// lblTXFilterHigh
			// 
			this.lblTXFilterHigh.Image = null;
			this.lblTXFilterHigh.Location = new System.Drawing.Point(16, 24);
			this.lblTXFilterHigh.Name = "lblTXFilterHigh";
			this.lblTXFilterHigh.Size = new System.Drawing.Size(40, 23);
			this.lblTXFilterHigh.TabIndex = 3;
			this.lblTXFilterHigh.Text = "High:";
			// 
			// udTXFilterLow
			// 
			this.udTXFilterLow.Increment = new System.Decimal(new int[] {
																			1,
																			0,
																			0,
																			0});
			this.udTXFilterLow.Location = new System.Drawing.Point(56, 48);
			this.udTXFilterLow.Maximum = new System.Decimal(new int[] {
																		  20000,
																		  0,
																		  0,
																		  0});
			this.udTXFilterLow.Minimum = new System.Decimal(new int[] {
																		  0,
																		  0,
																		  0,
																		  0});
			this.udTXFilterLow.Name = "udTXFilterLow";
			this.udTXFilterLow.Size = new System.Drawing.Size(56, 20);
			this.udTXFilterLow.TabIndex = 2;
			this.toolTip1.SetToolTip(this.udTXFilterLow, "Low Frequency TX Filter Cutoff");
			this.udTXFilterLow.Value = new System.Decimal(new int[] {
																		200,
																		0,
																		0,
																		0});
			this.udTXFilterLow.LostFocus += new System.EventHandler(this.udTXFilterLow_LostFocus);
			this.udTXFilterLow.ValueChanged += new System.EventHandler(this.udTXFilterLow_ValueChanged);
			// 
			// lblTXFilterLow
			// 
			this.lblTXFilterLow.Image = null;
			this.lblTXFilterLow.Location = new System.Drawing.Point(16, 48);
			this.lblTXFilterLow.Name = "lblTXFilterLow";
			this.lblTXFilterLow.Size = new System.Drawing.Size(40, 23);
			this.lblTXFilterLow.TabIndex = 1;
			this.lblTXFilterLow.Text = "Low:";
			// 
			// udTXFilterHigh
			// 
			this.udTXFilterHigh.Increment = new System.Decimal(new int[] {
																			 1,
																			 0,
																			 0,
																			 0});
			this.udTXFilterHigh.Location = new System.Drawing.Point(56, 24);
			this.udTXFilterHigh.Maximum = new System.Decimal(new int[] {
																		   20000,
																		   0,
																		   0,
																		   0});
			this.udTXFilterHigh.Minimum = new System.Decimal(new int[] {
																		   0,
																		   0,
																		   0,
																		   0});
			this.udTXFilterHigh.Name = "udTXFilterHigh";
			this.udTXFilterHigh.Size = new System.Drawing.Size(56, 20);
			this.udTXFilterHigh.TabIndex = 0;
			this.toolTip1.SetToolTip(this.udTXFilterHigh, "High Frequency TX Filter Cutoff");
			this.udTXFilterHigh.Value = new System.Decimal(new int[] {
																		 2900,
																		 0,
																		 0,
																		 0});
			this.udTXFilterHigh.LostFocus += new System.EventHandler(this.udTXFilterHigh_LostFocus);
			this.udTXFilterHigh.ValueChanged += new System.EventHandler(this.udTXFilterHigh_ValueChanged);
			// 
			// chkDCBlock
			// 
			this.chkDCBlock.Image = null;
			this.chkDCBlock.Location = new System.Drawing.Point(288, 16);
			this.chkDCBlock.Name = "chkDCBlock";
			this.chkDCBlock.Size = new System.Drawing.Size(72, 16);
			this.chkDCBlock.TabIndex = 48;
			this.chkDCBlock.Text = "DC Block";
			this.toolTip1.SetToolTip(this.chkDCBlock, "Enable this to engage a digital LPF to help eliminate DC garbage caused by some s" +
				"ound cards.");
			this.chkDCBlock.CheckedChanged += new System.EventHandler(this.chkDCBlock_CheckedChanged);
			// 
			// tpPowerAmplifier
			// 
			this.tpPowerAmplifier.Controls.Add(this.grpPABandOffset);
			this.tpPowerAmplifier.Controls.Add(this.grpPAGainByBand);
			this.tpPowerAmplifier.Controls.Add(this.lblPACalPower);
			this.tpPowerAmplifier.Controls.Add(this.udPACalPower);
			this.tpPowerAmplifier.Location = new System.Drawing.Point(4, 22);
			this.tpPowerAmplifier.Name = "tpPowerAmplifier";
			this.tpPowerAmplifier.Size = new System.Drawing.Size(584, 286);
			this.tpPowerAmplifier.TabIndex = 8;
			this.tpPowerAmplifier.Text = "PA Settings";
			// 
			// grpPABandOffset
			// 
			this.grpPABandOffset.Controls.Add(this.lblPABandOffset10);
			this.grpPABandOffset.Controls.Add(this.lblPABandOffset12);
			this.grpPABandOffset.Controls.Add(this.lblPABandOffset15);
			this.grpPABandOffset.Controls.Add(this.lblPABandOffset17);
			this.grpPABandOffset.Controls.Add(this.lblPABandOffset20);
			this.grpPABandOffset.Controls.Add(this.lblPABandOffset30);
			this.grpPABandOffset.Controls.Add(this.lblPABandOffset40);
			this.grpPABandOffset.Controls.Add(this.lblPABandOffset60);
			this.grpPABandOffset.Controls.Add(this.lblPABandOffset80);
			this.grpPABandOffset.Controls.Add(this.lblPABandOffset160);
			this.grpPABandOffset.Controls.Add(this.udPAADC17);
			this.grpPABandOffset.Controls.Add(this.udPAADC15);
			this.grpPABandOffset.Controls.Add(this.udPAADC20);
			this.grpPABandOffset.Controls.Add(this.udPAADC12);
			this.grpPABandOffset.Controls.Add(this.udPAADC10);
			this.grpPABandOffset.Controls.Add(this.udPAADC160);
			this.grpPABandOffset.Controls.Add(this.udPAADC80);
			this.grpPABandOffset.Controls.Add(this.udPAADC60);
			this.grpPABandOffset.Controls.Add(this.udPAADC40);
			this.grpPABandOffset.Controls.Add(this.udPAADC30);
			this.grpPABandOffset.Location = new System.Drawing.Point(232, 8);
			this.grpPABandOffset.Name = "grpPABandOffset";
			this.grpPABandOffset.Size = new System.Drawing.Size(208, 152);
			this.grpPABandOffset.TabIndex = 81;
			this.grpPABandOffset.TabStop = false;
			this.grpPABandOffset.Text = "ADC Offset (ADC bits)";
			this.grpPABandOffset.Visible = false;
			// 
			// lblPABandOffset10
			// 
			this.lblPABandOffset10.Image = null;
			this.lblPABandOffset10.Location = new System.Drawing.Point(104, 120);
			this.lblPABandOffset10.Name = "lblPABandOffset10";
			this.lblPABandOffset10.Size = new System.Drawing.Size(40, 16);
			this.lblPABandOffset10.TabIndex = 90;
			this.lblPABandOffset10.Text = "10m:";
			// 
			// lblPABandOffset12
			// 
			this.lblPABandOffset12.Image = null;
			this.lblPABandOffset12.Location = new System.Drawing.Point(104, 96);
			this.lblPABandOffset12.Name = "lblPABandOffset12";
			this.lblPABandOffset12.Size = new System.Drawing.Size(40, 16);
			this.lblPABandOffset12.TabIndex = 89;
			this.lblPABandOffset12.Text = "12m:";
			// 
			// lblPABandOffset15
			// 
			this.lblPABandOffset15.Image = null;
			this.lblPABandOffset15.Location = new System.Drawing.Point(104, 72);
			this.lblPABandOffset15.Name = "lblPABandOffset15";
			this.lblPABandOffset15.Size = new System.Drawing.Size(40, 16);
			this.lblPABandOffset15.TabIndex = 88;
			this.lblPABandOffset15.Text = "15m:";
			// 
			// lblPABandOffset17
			// 
			this.lblPABandOffset17.Image = null;
			this.lblPABandOffset17.Location = new System.Drawing.Point(104, 48);
			this.lblPABandOffset17.Name = "lblPABandOffset17";
			this.lblPABandOffset17.Size = new System.Drawing.Size(40, 16);
			this.lblPABandOffset17.TabIndex = 87;
			this.lblPABandOffset17.Text = "17m:";
			// 
			// lblPABandOffset20
			// 
			this.lblPABandOffset20.Image = null;
			this.lblPABandOffset20.Location = new System.Drawing.Point(104, 24);
			this.lblPABandOffset20.Name = "lblPABandOffset20";
			this.lblPABandOffset20.Size = new System.Drawing.Size(40, 16);
			this.lblPABandOffset20.TabIndex = 86;
			this.lblPABandOffset20.Text = "20m:";
			// 
			// lblPABandOffset30
			// 
			this.lblPABandOffset30.Image = null;
			this.lblPABandOffset30.Location = new System.Drawing.Point(16, 120);
			this.lblPABandOffset30.Name = "lblPABandOffset30";
			this.lblPABandOffset30.Size = new System.Drawing.Size(40, 16);
			this.lblPABandOffset30.TabIndex = 85;
			this.lblPABandOffset30.Text = "30m:";
			// 
			// lblPABandOffset40
			// 
			this.lblPABandOffset40.Image = null;
			this.lblPABandOffset40.Location = new System.Drawing.Point(16, 96);
			this.lblPABandOffset40.Name = "lblPABandOffset40";
			this.lblPABandOffset40.Size = new System.Drawing.Size(40, 16);
			this.lblPABandOffset40.TabIndex = 84;
			this.lblPABandOffset40.Text = "40m:";
			// 
			// lblPABandOffset60
			// 
			this.lblPABandOffset60.Image = null;
			this.lblPABandOffset60.Location = new System.Drawing.Point(16, 72);
			this.lblPABandOffset60.Name = "lblPABandOffset60";
			this.lblPABandOffset60.Size = new System.Drawing.Size(40, 16);
			this.lblPABandOffset60.TabIndex = 83;
			this.lblPABandOffset60.Text = "60m:";
			// 
			// lblPABandOffset80
			// 
			this.lblPABandOffset80.Image = null;
			this.lblPABandOffset80.Location = new System.Drawing.Point(16, 48);
			this.lblPABandOffset80.Name = "lblPABandOffset80";
			this.lblPABandOffset80.Size = new System.Drawing.Size(40, 16);
			this.lblPABandOffset80.TabIndex = 82;
			this.lblPABandOffset80.Text = "80m:";
			// 
			// lblPABandOffset160
			// 
			this.lblPABandOffset160.Image = null;
			this.lblPABandOffset160.Location = new System.Drawing.Point(16, 24);
			this.lblPABandOffset160.Name = "lblPABandOffset160";
			this.lblPABandOffset160.Size = new System.Drawing.Size(40, 16);
			this.lblPABandOffset160.TabIndex = 81;
			this.lblPABandOffset160.Text = "160m:";
			// 
			// udPAADC17
			// 
			this.udPAADC17.Increment = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		0});
			this.udPAADC17.Location = new System.Drawing.Point(144, 48);
			this.udPAADC17.Maximum = new System.Decimal(new int[] {
																	  255,
																	  0,
																	  0,
																	  0});
			this.udPAADC17.Minimum = new System.Decimal(new int[] {
																	  0,
																	  0,
																	  0,
																	  0});
			this.udPAADC17.Name = "udPAADC17";
			this.udPAADC17.Size = new System.Drawing.Size(48, 20);
			this.udPAADC17.TabIndex = 77;
			this.udPAADC17.Value = new System.Decimal(new int[] {
																	108,
																	0,
																	0,
																	0});
			this.udPAADC17.LostFocus += new System.EventHandler(this.udPAADC17_LostFocus);
			// 
			// udPAADC15
			// 
			this.udPAADC15.Increment = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		0});
			this.udPAADC15.Location = new System.Drawing.Point(144, 72);
			this.udPAADC15.Maximum = new System.Decimal(new int[] {
																	  255,
																	  0,
																	  0,
																	  0});
			this.udPAADC15.Minimum = new System.Decimal(new int[] {
																	  0,
																	  0,
																	  0,
																	  0});
			this.udPAADC15.Name = "udPAADC15";
			this.udPAADC15.Size = new System.Drawing.Size(48, 20);
			this.udPAADC15.TabIndex = 78;
			this.udPAADC15.Value = new System.Decimal(new int[] {
																	108,
																	0,
																	0,
																	0});
			this.udPAADC15.LostFocus += new System.EventHandler(this.udPAADC15_LostFocus);
			// 
			// udPAADC20
			// 
			this.udPAADC20.Increment = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		0});
			this.udPAADC20.Location = new System.Drawing.Point(144, 24);
			this.udPAADC20.Maximum = new System.Decimal(new int[] {
																	  255,
																	  0,
																	  0,
																	  0});
			this.udPAADC20.Minimum = new System.Decimal(new int[] {
																	  0,
																	  0,
																	  0,
																	  0});
			this.udPAADC20.Name = "udPAADC20";
			this.udPAADC20.Size = new System.Drawing.Size(48, 20);
			this.udPAADC20.TabIndex = 76;
			this.udPAADC20.Value = new System.Decimal(new int[] {
																	108,
																	0,
																	0,
																	0});
			this.udPAADC20.LostFocus += new System.EventHandler(this.udPAADC20_LostFocus);
			// 
			// udPAADC12
			// 
			this.udPAADC12.Increment = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		0});
			this.udPAADC12.Location = new System.Drawing.Point(144, 96);
			this.udPAADC12.Maximum = new System.Decimal(new int[] {
																	  255,
																	  0,
																	  0,
																	  0});
			this.udPAADC12.Minimum = new System.Decimal(new int[] {
																	  0,
																	  0,
																	  0,
																	  0});
			this.udPAADC12.Name = "udPAADC12";
			this.udPAADC12.Size = new System.Drawing.Size(48, 20);
			this.udPAADC12.TabIndex = 79;
			this.udPAADC12.Value = new System.Decimal(new int[] {
																	110,
																	0,
																	0,
																	0});
			this.udPAADC12.LostFocus += new System.EventHandler(this.udPAADC12_LostFocus);
			// 
			// udPAADC10
			// 
			this.udPAADC10.Increment = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		0});
			this.udPAADC10.Location = new System.Drawing.Point(144, 120);
			this.udPAADC10.Maximum = new System.Decimal(new int[] {
																	  255,
																	  0,
																	  0,
																	  0});
			this.udPAADC10.Minimum = new System.Decimal(new int[] {
																	  0,
																	  0,
																	  0,
																	  0});
			this.udPAADC10.Name = "udPAADC10";
			this.udPAADC10.Size = new System.Drawing.Size(48, 20);
			this.udPAADC10.TabIndex = 80;
			this.udPAADC10.Value = new System.Decimal(new int[] {
																	111,
																	0,
																	0,
																	0});
			this.udPAADC10.LostFocus += new System.EventHandler(this.udPAADC10_LostFocus);
			// 
			// udPAADC160
			// 
			this.udPAADC160.Increment = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 0});
			this.udPAADC160.Location = new System.Drawing.Point(56, 24);
			this.udPAADC160.Maximum = new System.Decimal(new int[] {
																	   255,
																	   0,
																	   0,
																	   0});
			this.udPAADC160.Minimum = new System.Decimal(new int[] {
																	   0,
																	   0,
																	   0,
																	   0});
			this.udPAADC160.Name = "udPAADC160";
			this.udPAADC160.Size = new System.Drawing.Size(48, 20);
			this.udPAADC160.TabIndex = 71;
			this.udPAADC160.Value = new System.Decimal(new int[] {
																	 107,
																	 0,
																	 0,
																	 0});
			this.udPAADC160.LostFocus += new System.EventHandler(this.udPAADC160_LostFocus);
			// 
			// udPAADC80
			// 
			this.udPAADC80.Increment = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		0});
			this.udPAADC80.Location = new System.Drawing.Point(56, 48);
			this.udPAADC80.Maximum = new System.Decimal(new int[] {
																	  255,
																	  0,
																	  0,
																	  0});
			this.udPAADC80.Minimum = new System.Decimal(new int[] {
																	  0,
																	  0,
																	  0,
																	  0});
			this.udPAADC80.Name = "udPAADC80";
			this.udPAADC80.Size = new System.Drawing.Size(48, 20);
			this.udPAADC80.TabIndex = 72;
			this.udPAADC80.Value = new System.Decimal(new int[] {
																	107,
																	0,
																	0,
																	0});
			this.udPAADC80.LostFocus += new System.EventHandler(this.udPAADC80_LostFocus);
			// 
			// udPAADC60
			// 
			this.udPAADC60.Increment = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		0});
			this.udPAADC60.Location = new System.Drawing.Point(56, 72);
			this.udPAADC60.Maximum = new System.Decimal(new int[] {
																	  255,
																	  0,
																	  0,
																	  0});
			this.udPAADC60.Minimum = new System.Decimal(new int[] {
																	  0,
																	  0,
																	  0,
																	  0});
			this.udPAADC60.Name = "udPAADC60";
			this.udPAADC60.Size = new System.Drawing.Size(48, 20);
			this.udPAADC60.TabIndex = 73;
			this.udPAADC60.Value = new System.Decimal(new int[] {
																	107,
																	0,
																	0,
																	0});
			this.udPAADC60.LostFocus += new System.EventHandler(this.udPAADC60_LostFocus);
			// 
			// udPAADC40
			// 
			this.udPAADC40.Increment = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		0});
			this.udPAADC40.Location = new System.Drawing.Point(56, 96);
			this.udPAADC40.Maximum = new System.Decimal(new int[] {
																	  255,
																	  0,
																	  0,
																	  0});
			this.udPAADC40.Minimum = new System.Decimal(new int[] {
																	  0,
																	  0,
																	  0,
																	  0});
			this.udPAADC40.Name = "udPAADC40";
			this.udPAADC40.Size = new System.Drawing.Size(48, 20);
			this.udPAADC40.TabIndex = 74;
			this.udPAADC40.Value = new System.Decimal(new int[] {
																	106,
																	0,
																	0,
																	0});
			this.udPAADC40.LostFocus += new System.EventHandler(this.udPAADC40_LostFocus);
			// 
			// udPAADC30
			// 
			this.udPAADC30.Increment = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		0});
			this.udPAADC30.Location = new System.Drawing.Point(56, 120);
			this.udPAADC30.Maximum = new System.Decimal(new int[] {
																	  255,
																	  0,
																	  0,
																	  0});
			this.udPAADC30.Minimum = new System.Decimal(new int[] {
																	  0,
																	  0,
																	  0,
																	  0});
			this.udPAADC30.Name = "udPAADC30";
			this.udPAADC30.Size = new System.Drawing.Size(48, 20);
			this.udPAADC30.TabIndex = 75;
			this.udPAADC30.Value = new System.Decimal(new int[] {
																	108,
																	0,
																	0,
																	0});
			this.udPAADC30.LostFocus += new System.EventHandler(this.udPAADC30_LostFocus);
			// 
			// grpPAGainByBand
			// 
			this.grpPAGainByBand.Controls.Add(this.btnPAGainReset);
			this.grpPAGainByBand.Controls.Add(this.btnPAGainCalibration);
			this.grpPAGainByBand.Controls.Add(this.lblPAGainByBand10);
			this.grpPAGainByBand.Controls.Add(this.udPAGain10);
			this.grpPAGainByBand.Controls.Add(this.lblPAGainByBand12);
			this.grpPAGainByBand.Controls.Add(this.udPAGain12);
			this.grpPAGainByBand.Controls.Add(this.lblPAGainByBand15);
			this.grpPAGainByBand.Controls.Add(this.udPAGain15);
			this.grpPAGainByBand.Controls.Add(this.lblPAGainByBand17);
			this.grpPAGainByBand.Controls.Add(this.udPAGain17);
			this.grpPAGainByBand.Controls.Add(this.lblPAGainByBand20);
			this.grpPAGainByBand.Controls.Add(this.udPAGain20);
			this.grpPAGainByBand.Controls.Add(this.lblPAGainByBand30);
			this.grpPAGainByBand.Controls.Add(this.udPAGain30);
			this.grpPAGainByBand.Controls.Add(this.lblPAGainByBand40);
			this.grpPAGainByBand.Controls.Add(this.udPAGain40);
			this.grpPAGainByBand.Controls.Add(this.lblPAGainByBand60);
			this.grpPAGainByBand.Controls.Add(this.udPAGain60);
			this.grpPAGainByBand.Controls.Add(this.lblPAGainByBand80);
			this.grpPAGainByBand.Controls.Add(this.udPAGain80);
			this.grpPAGainByBand.Controls.Add(this.lblPAGainByBand160);
			this.grpPAGainByBand.Controls.Add(this.udPAGain160);
			this.grpPAGainByBand.Location = new System.Drawing.Point(8, 8);
			this.grpPAGainByBand.Name = "grpPAGainByBand";
			this.grpPAGainByBand.Size = new System.Drawing.Size(216, 184);
			this.grpPAGainByBand.TabIndex = 1;
			this.grpPAGainByBand.TabStop = false;
			this.grpPAGainByBand.Text = "Gain By Band (dB)";
			this.grpPAGainByBand.Visible = false;
			// 
			// btnPAGainReset
			// 
			this.btnPAGainReset.Image = null;
			this.btnPAGainReset.Location = new System.Drawing.Point(120, 152);
			this.btnPAGainReset.Name = "btnPAGainReset";
			this.btnPAGainReset.TabIndex = 21;
			this.btnPAGainReset.Text = "Reset";
			this.toolTip1.SetToolTip(this.btnPAGainReset, "Reset all Gain values to the default 48.0dB");
			this.btnPAGainReset.Click += new System.EventHandler(this.btnPAGainReset_Click);
			// 
			// btnPAGainCalibration
			// 
			this.btnPAGainCalibration.Image = null;
			this.btnPAGainCalibration.Location = new System.Drawing.Point(24, 152);
			this.btnPAGainCalibration.Name = "btnPAGainCalibration";
			this.btnPAGainCalibration.TabIndex = 20;
			this.btnPAGainCalibration.Text = "Calibrate";
			this.btnPAGainCalibration.Click += new System.EventHandler(this.btnPAGainCalibration_Click);
			// 
			// lblPAGainByBand10
			// 
			this.lblPAGainByBand10.Image = null;
			this.lblPAGainByBand10.Location = new System.Drawing.Point(112, 120);
			this.lblPAGainByBand10.Name = "lblPAGainByBand10";
			this.lblPAGainByBand10.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand10.TabIndex = 19;
			this.lblPAGainByBand10.Text = "10m:";
			// 
			// udPAGain10
			// 
			this.udPAGain10.DecimalPlaces = 1;
			this.udPAGain10.Increment = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 65536});
			this.udPAGain10.Location = new System.Drawing.Point(152, 120);
			this.udPAGain10.Maximum = new System.Decimal(new int[] {
																	   100,
																	   0,
																	   0,
																	   0});
			this.udPAGain10.Minimum = new System.Decimal(new int[] {
																	   38,
																	   0,
																	   0,
																	   0});
			this.udPAGain10.Name = "udPAGain10";
			this.udPAGain10.Size = new System.Drawing.Size(48, 20);
			this.udPAGain10.TabIndex = 18;
			this.udPAGain10.Value = new System.Decimal(new int[] {
																	 430,
																	 0,
																	 0,
																	 65536});
			this.udPAGain10.LostFocus += new System.EventHandler(this.udPAGain10_LostFocus);
			this.udPAGain10.ValueChanged += new System.EventHandler(this.udPAGain_ValueChanged);
			// 
			// lblPAGainByBand12
			// 
			this.lblPAGainByBand12.Image = null;
			this.lblPAGainByBand12.Location = new System.Drawing.Point(112, 96);
			this.lblPAGainByBand12.Name = "lblPAGainByBand12";
			this.lblPAGainByBand12.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand12.TabIndex = 17;
			this.lblPAGainByBand12.Text = "12m:";
			// 
			// udPAGain12
			// 
			this.udPAGain12.DecimalPlaces = 1;
			this.udPAGain12.Increment = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 65536});
			this.udPAGain12.Location = new System.Drawing.Point(152, 96);
			this.udPAGain12.Maximum = new System.Decimal(new int[] {
																	   100,
																	   0,
																	   0,
																	   0});
			this.udPAGain12.Minimum = new System.Decimal(new int[] {
																	   38,
																	   0,
																	   0,
																	   0});
			this.udPAGain12.Name = "udPAGain12";
			this.udPAGain12.Size = new System.Drawing.Size(48, 20);
			this.udPAGain12.TabIndex = 16;
			this.udPAGain12.Value = new System.Decimal(new int[] {
																	 474,
																	 0,
																	 0,
																	 65536});
			this.udPAGain12.LostFocus += new System.EventHandler(this.udPAGain12_LostFocus);
			this.udPAGain12.ValueChanged += new System.EventHandler(this.udPAGain_ValueChanged);
			// 
			// lblPAGainByBand15
			// 
			this.lblPAGainByBand15.Image = null;
			this.lblPAGainByBand15.Location = new System.Drawing.Point(112, 72);
			this.lblPAGainByBand15.Name = "lblPAGainByBand15";
			this.lblPAGainByBand15.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand15.TabIndex = 15;
			this.lblPAGainByBand15.Text = "15m:";
			// 
			// udPAGain15
			// 
			this.udPAGain15.DecimalPlaces = 1;
			this.udPAGain15.Increment = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 65536});
			this.udPAGain15.Location = new System.Drawing.Point(152, 72);
			this.udPAGain15.Maximum = new System.Decimal(new int[] {
																	   100,
																	   0,
																	   0,
																	   0});
			this.udPAGain15.Minimum = new System.Decimal(new int[] {
																	   38,
																	   0,
																	   0,
																	   0});
			this.udPAGain15.Name = "udPAGain15";
			this.udPAGain15.Size = new System.Drawing.Size(48, 20);
			this.udPAGain15.TabIndex = 14;
			this.udPAGain15.Value = new System.Decimal(new int[] {
																	 481,
																	 0,
																	 0,
																	 65536});
			this.udPAGain15.LostFocus += new System.EventHandler(this.udPAGain15_LostFocus);
			this.udPAGain15.ValueChanged += new System.EventHandler(this.udPAGain_ValueChanged);
			// 
			// lblPAGainByBand17
			// 
			this.lblPAGainByBand17.Image = null;
			this.lblPAGainByBand17.Location = new System.Drawing.Point(112, 48);
			this.lblPAGainByBand17.Name = "lblPAGainByBand17";
			this.lblPAGainByBand17.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand17.TabIndex = 13;
			this.lblPAGainByBand17.Text = "17m:";
			// 
			// udPAGain17
			// 
			this.udPAGain17.DecimalPlaces = 1;
			this.udPAGain17.Increment = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 65536});
			this.udPAGain17.Location = new System.Drawing.Point(152, 48);
			this.udPAGain17.Maximum = new System.Decimal(new int[] {
																	   100,
																	   0,
																	   0,
																	   0});
			this.udPAGain17.Minimum = new System.Decimal(new int[] {
																	   38,
																	   0,
																	   0,
																	   0});
			this.udPAGain17.Name = "udPAGain17";
			this.udPAGain17.Size = new System.Drawing.Size(48, 20);
			this.udPAGain17.TabIndex = 12;
			this.udPAGain17.Value = new System.Decimal(new int[] {
																	 493,
																	 0,
																	 0,
																	 65536});
			this.udPAGain17.LostFocus += new System.EventHandler(this.udPAGain17_LostFocus);
			this.udPAGain17.ValueChanged += new System.EventHandler(this.udPAGain_ValueChanged);
			// 
			// lblPAGainByBand20
			// 
			this.lblPAGainByBand20.Image = null;
			this.lblPAGainByBand20.Location = new System.Drawing.Point(112, 24);
			this.lblPAGainByBand20.Name = "lblPAGainByBand20";
			this.lblPAGainByBand20.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand20.TabIndex = 11;
			this.lblPAGainByBand20.Text = "20m:";
			// 
			// udPAGain20
			// 
			this.udPAGain20.DecimalPlaces = 1;
			this.udPAGain20.Increment = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 65536});
			this.udPAGain20.Location = new System.Drawing.Point(152, 24);
			this.udPAGain20.Maximum = new System.Decimal(new int[] {
																	   100,
																	   0,
																	   0,
																	   0});
			this.udPAGain20.Minimum = new System.Decimal(new int[] {
																	   38,
																	   0,
																	   0,
																	   0});
			this.udPAGain20.Name = "udPAGain20";
			this.udPAGain20.Size = new System.Drawing.Size(48, 20);
			this.udPAGain20.TabIndex = 10;
			this.udPAGain20.Value = new System.Decimal(new int[] {
																	 483,
																	 0,
																	 0,
																	 65536});
			this.udPAGain20.LostFocus += new System.EventHandler(this.udPAGain20_LostFocus);
			this.udPAGain20.ValueChanged += new System.EventHandler(this.udPAGain_ValueChanged);
			// 
			// lblPAGainByBand30
			// 
			this.lblPAGainByBand30.Image = null;
			this.lblPAGainByBand30.Location = new System.Drawing.Point(16, 120);
			this.lblPAGainByBand30.Name = "lblPAGainByBand30";
			this.lblPAGainByBand30.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand30.TabIndex = 9;
			this.lblPAGainByBand30.Text = "30m:";
			// 
			// udPAGain30
			// 
			this.udPAGain30.DecimalPlaces = 1;
			this.udPAGain30.Increment = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 65536});
			this.udPAGain30.Location = new System.Drawing.Point(56, 120);
			this.udPAGain30.Maximum = new System.Decimal(new int[] {
																	   100,
																	   0,
																	   0,
																	   0});
			this.udPAGain30.Minimum = new System.Decimal(new int[] {
																	   38,
																	   0,
																	   0,
																	   0});
			this.udPAGain30.Name = "udPAGain30";
			this.udPAGain30.Size = new System.Drawing.Size(48, 20);
			this.udPAGain30.TabIndex = 8;
			this.udPAGain30.Value = new System.Decimal(new int[] {
																	 489,
																	 0,
																	 0,
																	 65536});
			this.udPAGain30.LostFocus += new System.EventHandler(this.udPAGain30_LostFocus);
			this.udPAGain30.ValueChanged += new System.EventHandler(this.udPAGain_ValueChanged);
			// 
			// lblPAGainByBand40
			// 
			this.lblPAGainByBand40.Image = null;
			this.lblPAGainByBand40.Location = new System.Drawing.Point(16, 96);
			this.lblPAGainByBand40.Name = "lblPAGainByBand40";
			this.lblPAGainByBand40.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand40.TabIndex = 7;
			this.lblPAGainByBand40.Text = "40m:";
			// 
			// udPAGain40
			// 
			this.udPAGain40.DecimalPlaces = 1;
			this.udPAGain40.Increment = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 65536});
			this.udPAGain40.Location = new System.Drawing.Point(56, 96);
			this.udPAGain40.Maximum = new System.Decimal(new int[] {
																	   100,
																	   0,
																	   0,
																	   0});
			this.udPAGain40.Minimum = new System.Decimal(new int[] {
																	   38,
																	   0,
																	   0,
																	   0});
			this.udPAGain40.Name = "udPAGain40";
			this.udPAGain40.Size = new System.Drawing.Size(48, 20);
			this.udPAGain40.TabIndex = 6;
			this.udPAGain40.Value = new System.Decimal(new int[] {
																	 469,
																	 0,
																	 0,
																	 65536});
			this.udPAGain40.LostFocus += new System.EventHandler(this.udPAGain40_LostFocus);
			this.udPAGain40.ValueChanged += new System.EventHandler(this.udPAGain_ValueChanged);
			// 
			// lblPAGainByBand60
			// 
			this.lblPAGainByBand60.Image = null;
			this.lblPAGainByBand60.Location = new System.Drawing.Point(16, 72);
			this.lblPAGainByBand60.Name = "lblPAGainByBand60";
			this.lblPAGainByBand60.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand60.TabIndex = 5;
			this.lblPAGainByBand60.Text = "60m:";
			// 
			// udPAGain60
			// 
			this.udPAGain60.DecimalPlaces = 1;
			this.udPAGain60.Increment = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 65536});
			this.udPAGain60.Location = new System.Drawing.Point(56, 72);
			this.udPAGain60.Maximum = new System.Decimal(new int[] {
																	   100,
																	   0,
																	   0,
																	   0});
			this.udPAGain60.Minimum = new System.Decimal(new int[] {
																	   38,
																	   0,
																	   0,
																	   0});
			this.udPAGain60.Name = "udPAGain60";
			this.udPAGain60.Size = new System.Drawing.Size(48, 20);
			this.udPAGain60.TabIndex = 4;
			this.udPAGain60.Value = new System.Decimal(new int[] {
																	 474,
																	 0,
																	 0,
																	 65536});
			this.udPAGain60.LostFocus += new System.EventHandler(this.udPAGain60_LostFocus);
			this.udPAGain60.ValueChanged += new System.EventHandler(this.udPAGain_ValueChanged);
			// 
			// lblPAGainByBand80
			// 
			this.lblPAGainByBand80.Image = null;
			this.lblPAGainByBand80.Location = new System.Drawing.Point(16, 48);
			this.lblPAGainByBand80.Name = "lblPAGainByBand80";
			this.lblPAGainByBand80.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand80.TabIndex = 3;
			this.lblPAGainByBand80.Text = "80m:";
			// 
			// udPAGain80
			// 
			this.udPAGain80.DecimalPlaces = 1;
			this.udPAGain80.Increment = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 65536});
			this.udPAGain80.Location = new System.Drawing.Point(56, 48);
			this.udPAGain80.Maximum = new System.Decimal(new int[] {
																	   100,
																	   0,
																	   0,
																	   0});
			this.udPAGain80.Minimum = new System.Decimal(new int[] {
																	   38,
																	   0,
																	   0,
																	   0});
			this.udPAGain80.Name = "udPAGain80";
			this.udPAGain80.Size = new System.Drawing.Size(48, 20);
			this.udPAGain80.TabIndex = 2;
			this.udPAGain80.Value = new System.Decimal(new int[] {
																	 480,
																	 0,
																	 0,
																	 65536});
			this.udPAGain80.LostFocus += new System.EventHandler(this.udPAGain80_LostFocus);
			this.udPAGain80.ValueChanged += new System.EventHandler(this.udPAGain_ValueChanged);
			// 
			// lblPAGainByBand160
			// 
			this.lblPAGainByBand160.Image = null;
			this.lblPAGainByBand160.Location = new System.Drawing.Point(16, 24);
			this.lblPAGainByBand160.Name = "lblPAGainByBand160";
			this.lblPAGainByBand160.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand160.TabIndex = 1;
			this.lblPAGainByBand160.Text = "160m:";
			// 
			// udPAGain160
			// 
			this.udPAGain160.DecimalPlaces = 1;
			this.udPAGain160.Increment = new System.Decimal(new int[] {
																		  1,
																		  0,
																		  0,
																		  65536});
			this.udPAGain160.Location = new System.Drawing.Point(56, 24);
			this.udPAGain160.Maximum = new System.Decimal(new int[] {
																		100,
																		0,
																		0,
																		0});
			this.udPAGain160.Minimum = new System.Decimal(new int[] {
																		38,
																		0,
																		0,
																		0});
			this.udPAGain160.Name = "udPAGain160";
			this.udPAGain160.Size = new System.Drawing.Size(48, 20);
			this.udPAGain160.TabIndex = 0;
			this.udPAGain160.Value = new System.Decimal(new int[] {
																	  490,
																	  0,
																	  0,
																	  65536});
			this.udPAGain160.LostFocus += new System.EventHandler(this.udPAGain160_LostFocus);
			this.udPAGain160.ValueChanged += new System.EventHandler(this.udPAGain_ValueChanged);
			// 
			// lblPACalPower
			// 
			this.lblPACalPower.Image = null;
			this.lblPACalPower.Location = new System.Drawing.Point(32, 224);
			this.lblPACalPower.Name = "lblPACalPower";
			this.lblPACalPower.Size = new System.Drawing.Size(64, 32);
			this.lblPACalPower.TabIndex = 23;
			this.lblPACalPower.Text = "Target Power (W):";
			this.lblPACalPower.Visible = false;
			// 
			// udPACalPower
			// 
			this.udPACalPower.Increment = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   0});
			this.udPACalPower.Location = new System.Drawing.Point(104, 224);
			this.udPACalPower.Maximum = new System.Decimal(new int[] {
																		 100,
																		 0,
																		 0,
																		 0});
			this.udPACalPower.Minimum = new System.Decimal(new int[] {
																		 5,
																		 0,
																		 0,
																		 0});
			this.udPACalPower.Name = "udPACalPower";
			this.udPACalPower.Size = new System.Drawing.Size(40, 20);
			this.udPACalPower.TabIndex = 22;
			this.toolTip1.SetToolTip(this.udPACalPower, "The target power used for the PA Calibration.");
			this.udPACalPower.Value = new System.Decimal(new int[] {
																	   100,
																	   0,
																	   0,
																	   0});
			this.udPACalPower.Visible = false;
			this.udPACalPower.LostFocus += new System.EventHandler(this.udPACalPower_LostFocus);
			// 
			// tpExtCtrl
			// 
			this.tpExtCtrl.Controls.Add(this.chkExtEnable);
			this.tpExtCtrl.Controls.Add(this.grpExtTX);
			this.tpExtCtrl.Controls.Add(this.grpExtRX);
			this.tpExtCtrl.Location = new System.Drawing.Point(4, 22);
			this.tpExtCtrl.Name = "tpExtCtrl";
			this.tpExtCtrl.Size = new System.Drawing.Size(584, 286);
			this.tpExtCtrl.TabIndex = 11;
			this.tpExtCtrl.Text = "Ext. Ctrl";
			// 
			// chkExtEnable
			// 
			this.chkExtEnable.Image = null;
			this.chkExtEnable.Location = new System.Drawing.Point(360, 16);
			this.chkExtEnable.Name = "chkExtEnable";
			this.chkExtEnable.Size = new System.Drawing.Size(64, 24);
			this.chkExtEnable.TabIndex = 9;
			this.chkExtEnable.Text = "Enable";
			this.toolTip1.SetToolTip(this.chkExtEnable, "Check this box to enable the matrix to the left to control the X2 pins.");
			this.chkExtEnable.CheckedChanged += new System.EventHandler(this.chkExtEnable_CheckedChanged);
			// 
			// grpExtTX
			// 
			this.grpExtTX.Controls.Add(this.lblExtTXX26);
			this.grpExtTX.Controls.Add(this.chkExtTX26);
			this.grpExtTX.Controls.Add(this.chkExtTX66);
			this.grpExtTX.Controls.Add(this.chkExtTX106);
			this.grpExtTX.Controls.Add(this.chkExtTX126);
			this.grpExtTX.Controls.Add(this.chkExtTX156);
			this.grpExtTX.Controls.Add(this.chkExtTX176);
			this.grpExtTX.Controls.Add(this.chkExtTX206);
			this.grpExtTX.Controls.Add(this.chkExtTX306);
			this.grpExtTX.Controls.Add(this.chkExtTX406);
			this.grpExtTX.Controls.Add(this.chkExtTX606);
			this.grpExtTX.Controls.Add(this.chkExtTX806);
			this.grpExtTX.Controls.Add(this.chkExtTX1606);
			this.grpExtTX.Controls.Add(this.lblExtTXX25);
			this.grpExtTX.Controls.Add(this.lblExtTXX24);
			this.grpExtTX.Controls.Add(this.lblExtTXX23);
			this.grpExtTX.Controls.Add(this.lblExtTXX22);
			this.grpExtTX.Controls.Add(this.lblExtTX2);
			this.grpExtTX.Controls.Add(this.chkExtTX23);
			this.grpExtTX.Controls.Add(this.chkExtTX22);
			this.grpExtTX.Controls.Add(this.chkExtTX21);
			this.grpExtTX.Controls.Add(this.chkExtTX25);
			this.grpExtTX.Controls.Add(this.chkExtTX24);
			this.grpExtTX.Controls.Add(this.lblExtTX6);
			this.grpExtTX.Controls.Add(this.chkExtTX63);
			this.grpExtTX.Controls.Add(this.chkExtTX62);
			this.grpExtTX.Controls.Add(this.chkExtTX61);
			this.grpExtTX.Controls.Add(this.chkExtTX65);
			this.grpExtTX.Controls.Add(this.chkExtTX64);
			this.grpExtTX.Controls.Add(this.lblExtTX10);
			this.grpExtTX.Controls.Add(this.chkExtTX103);
			this.grpExtTX.Controls.Add(this.chkExtTX102);
			this.grpExtTX.Controls.Add(this.chkExtTX101);
			this.grpExtTX.Controls.Add(this.chkExtTX105);
			this.grpExtTX.Controls.Add(this.chkExtTX104);
			this.grpExtTX.Controls.Add(this.lblExtTX12);
			this.grpExtTX.Controls.Add(this.chkExtTX123);
			this.grpExtTX.Controls.Add(this.chkExtTX122);
			this.grpExtTX.Controls.Add(this.chkExtTX121);
			this.grpExtTX.Controls.Add(this.chkExtTX125);
			this.grpExtTX.Controls.Add(this.chkExtTX124);
			this.grpExtTX.Controls.Add(this.lblExtTX15);
			this.grpExtTX.Controls.Add(this.chkExtTX153);
			this.grpExtTX.Controls.Add(this.chkExtTX152);
			this.grpExtTX.Controls.Add(this.chkExtTX151);
			this.grpExtTX.Controls.Add(this.chkExtTX155);
			this.grpExtTX.Controls.Add(this.chkExtTX154);
			this.grpExtTX.Controls.Add(this.lblExtTX17);
			this.grpExtTX.Controls.Add(this.chkExtTX173);
			this.grpExtTX.Controls.Add(this.chkExtTX172);
			this.grpExtTX.Controls.Add(this.chkExtTX171);
			this.grpExtTX.Controls.Add(this.chkExtTX175);
			this.grpExtTX.Controls.Add(this.chkExtTX174);
			this.grpExtTX.Controls.Add(this.lblExtTX20);
			this.grpExtTX.Controls.Add(this.chkExtTX203);
			this.grpExtTX.Controls.Add(this.chkExtTX202);
			this.grpExtTX.Controls.Add(this.chkExtTX201);
			this.grpExtTX.Controls.Add(this.chkExtTX205);
			this.grpExtTX.Controls.Add(this.chkExtTX204);
			this.grpExtTX.Controls.Add(this.lblExtTX30);
			this.grpExtTX.Controls.Add(this.chkExtTX303);
			this.grpExtTX.Controls.Add(this.chkExtTX302);
			this.grpExtTX.Controls.Add(this.chkExtTX301);
			this.grpExtTX.Controls.Add(this.chkExtTX305);
			this.grpExtTX.Controls.Add(this.chkExtTX304);
			this.grpExtTX.Controls.Add(this.lblExtTX40);
			this.grpExtTX.Controls.Add(this.chkExtTX403);
			this.grpExtTX.Controls.Add(this.chkExtTX402);
			this.grpExtTX.Controls.Add(this.chkExtTX401);
			this.grpExtTX.Controls.Add(this.chkExtTX405);
			this.grpExtTX.Controls.Add(this.chkExtTX404);
			this.grpExtTX.Controls.Add(this.lblExtTX60);
			this.grpExtTX.Controls.Add(this.chkExtTX603);
			this.grpExtTX.Controls.Add(this.chkExtTX602);
			this.grpExtTX.Controls.Add(this.chkExtTX601);
			this.grpExtTX.Controls.Add(this.chkExtTX605);
			this.grpExtTX.Controls.Add(this.chkExtTX604);
			this.grpExtTX.Controls.Add(this.lblExtTX80);
			this.grpExtTX.Controls.Add(this.chkExtTX803);
			this.grpExtTX.Controls.Add(this.chkExtTX802);
			this.grpExtTX.Controls.Add(this.chkExtTX801);
			this.grpExtTX.Controls.Add(this.chkExtTX805);
			this.grpExtTX.Controls.Add(this.chkExtTX804);
			this.grpExtTX.Controls.Add(this.lblExtTXX2Pins);
			this.grpExtTX.Controls.Add(this.lblExtTXBand);
			this.grpExtTX.Controls.Add(this.lblExtTX160);
			this.grpExtTX.Controls.Add(this.chkExtTX1603);
			this.grpExtTX.Controls.Add(this.chkExtTX1602);
			this.grpExtTX.Controls.Add(this.chkExtTX1601);
			this.grpExtTX.Controls.Add(this.lblExtTXX21);
			this.grpExtTX.Controls.Add(this.chkExtTX1605);
			this.grpExtTX.Controls.Add(this.chkExtTX1604);
			this.grpExtTX.Enabled = false;
			this.grpExtTX.Location = new System.Drawing.Point(184, 8);
			this.grpExtTX.Name = "grpExtTX";
			this.grpExtTX.Size = new System.Drawing.Size(168, 264);
			this.grpExtTX.TabIndex = 8;
			this.grpExtTX.TabStop = false;
			this.grpExtTX.Text = "Transmit";
			// 
			// lblExtTXX26
			// 
			this.lblExtTXX26.Image = null;
			this.lblExtTXX26.Location = new System.Drawing.Point(144, 40);
			this.lblExtTXX26.Name = "lblExtTXX26";
			this.lblExtTXX26.Size = new System.Drawing.Size(16, 16);
			this.lblExtTXX26.TabIndex = 171;
			this.lblExtTXX26.Text = "6";
			this.lblExtTXX26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chkExtTX26
			// 
			this.chkExtTX26.Image = null;
			this.chkExtTX26.Location = new System.Drawing.Point(144, 240);
			this.chkExtTX26.Name = "chkExtTX26";
			this.chkExtTX26.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX26.TabIndex = 170;
			this.chkExtTX26.Text = "checkBox5";
			this.chkExtTX26.CheckedChanged += new System.EventHandler(this.chkExtTX2_CheckedChanged);
			// 
			// chkExtTX66
			// 
			this.chkExtTX66.Image = null;
			this.chkExtTX66.Location = new System.Drawing.Point(144, 224);
			this.chkExtTX66.Name = "chkExtTX66";
			this.chkExtTX66.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX66.TabIndex = 169;
			this.chkExtTX66.Text = "checkBox5";
			this.chkExtTX66.CheckedChanged += new System.EventHandler(this.chkExtTX6_CheckedChanged);
			// 
			// chkExtTX106
			// 
			this.chkExtTX106.Image = null;
			this.chkExtTX106.Location = new System.Drawing.Point(144, 208);
			this.chkExtTX106.Name = "chkExtTX106";
			this.chkExtTX106.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX106.TabIndex = 168;
			this.chkExtTX106.Text = "checkBox5";
			this.chkExtTX106.CheckedChanged += new System.EventHandler(this.chkExtTX10_CheckedChanged);
			// 
			// chkExtTX126
			// 
			this.chkExtTX126.Image = null;
			this.chkExtTX126.Location = new System.Drawing.Point(144, 192);
			this.chkExtTX126.Name = "chkExtTX126";
			this.chkExtTX126.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX126.TabIndex = 167;
			this.chkExtTX126.Text = "checkBox5";
			this.chkExtTX126.CheckedChanged += new System.EventHandler(this.chkExtTX12_CheckedChanged);
			// 
			// chkExtTX156
			// 
			this.chkExtTX156.Image = null;
			this.chkExtTX156.Location = new System.Drawing.Point(144, 176);
			this.chkExtTX156.Name = "chkExtTX156";
			this.chkExtTX156.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX156.TabIndex = 166;
			this.chkExtTX156.Text = "checkBox5";
			this.chkExtTX156.CheckedChanged += new System.EventHandler(this.chkExtTX15_CheckedChanged);
			// 
			// chkExtTX176
			// 
			this.chkExtTX176.Image = null;
			this.chkExtTX176.Location = new System.Drawing.Point(144, 160);
			this.chkExtTX176.Name = "chkExtTX176";
			this.chkExtTX176.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX176.TabIndex = 165;
			this.chkExtTX176.Text = "checkBox5";
			this.chkExtTX176.CheckedChanged += new System.EventHandler(this.chkExtTX17_CheckedChanged);
			// 
			// chkExtTX206
			// 
			this.chkExtTX206.Image = null;
			this.chkExtTX206.Location = new System.Drawing.Point(144, 144);
			this.chkExtTX206.Name = "chkExtTX206";
			this.chkExtTX206.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX206.TabIndex = 164;
			this.chkExtTX206.Text = "checkBox5";
			this.chkExtTX206.CheckedChanged += new System.EventHandler(this.chkExtTX20_CheckedChanged);
			// 
			// chkExtTX306
			// 
			this.chkExtTX306.Image = null;
			this.chkExtTX306.Location = new System.Drawing.Point(144, 128);
			this.chkExtTX306.Name = "chkExtTX306";
			this.chkExtTX306.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX306.TabIndex = 163;
			this.chkExtTX306.Text = "checkBox5";
			this.chkExtTX306.CheckedChanged += new System.EventHandler(this.chkExtTX30_CheckedChanged);
			// 
			// chkExtTX406
			// 
			this.chkExtTX406.Image = null;
			this.chkExtTX406.Location = new System.Drawing.Point(144, 112);
			this.chkExtTX406.Name = "chkExtTX406";
			this.chkExtTX406.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX406.TabIndex = 162;
			this.chkExtTX406.Text = "checkBox5";
			this.chkExtTX406.CheckedChanged += new System.EventHandler(this.chkExtTX40_CheckedChanged);
			// 
			// chkExtTX606
			// 
			this.chkExtTX606.Image = null;
			this.chkExtTX606.Location = new System.Drawing.Point(144, 96);
			this.chkExtTX606.Name = "chkExtTX606";
			this.chkExtTX606.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX606.TabIndex = 161;
			this.chkExtTX606.Text = "checkBox5";
			this.chkExtTX606.CheckedChanged += new System.EventHandler(this.chkExtTX60_CheckedChanged);
			// 
			// chkExtTX806
			// 
			this.chkExtTX806.Image = null;
			this.chkExtTX806.Location = new System.Drawing.Point(144, 80);
			this.chkExtTX806.Name = "chkExtTX806";
			this.chkExtTX806.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX806.TabIndex = 160;
			this.chkExtTX806.Text = "checkBox5";
			this.chkExtTX806.CheckedChanged += new System.EventHandler(this.chkExtTX80_CheckedChanged);
			// 
			// chkExtTX1606
			// 
			this.chkExtTX1606.Image = null;
			this.chkExtTX1606.Location = new System.Drawing.Point(144, 64);
			this.chkExtTX1606.Name = "chkExtTX1606";
			this.chkExtTX1606.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX1606.TabIndex = 159;
			this.chkExtTX1606.Text = "checkBox5";
			this.chkExtTX1606.CheckedChanged += new System.EventHandler(this.chkExtTX160_CheckedChanged);
			// 
			// lblExtTXX25
			// 
			this.lblExtTXX25.Image = null;
			this.lblExtTXX25.Location = new System.Drawing.Point(128, 40);
			this.lblExtTXX25.Name = "lblExtTXX25";
			this.lblExtTXX25.Size = new System.Drawing.Size(16, 16);
			this.lblExtTXX25.TabIndex = 158;
			this.lblExtTXX25.Text = "5";
			this.lblExtTXX25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExtTXX24
			// 
			this.lblExtTXX24.Image = null;
			this.lblExtTXX24.Location = new System.Drawing.Point(112, 40);
			this.lblExtTXX24.Name = "lblExtTXX24";
			this.lblExtTXX24.Size = new System.Drawing.Size(16, 16);
			this.lblExtTXX24.TabIndex = 157;
			this.lblExtTXX24.Text = "4";
			this.lblExtTXX24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExtTXX23
			// 
			this.lblExtTXX23.Image = null;
			this.lblExtTXX23.Location = new System.Drawing.Point(96, 40);
			this.lblExtTXX23.Name = "lblExtTXX23";
			this.lblExtTXX23.Size = new System.Drawing.Size(16, 16);
			this.lblExtTXX23.TabIndex = 156;
			this.lblExtTXX23.Text = "3";
			this.lblExtTXX23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExtTXX22
			// 
			this.lblExtTXX22.Image = null;
			this.lblExtTXX22.Location = new System.Drawing.Point(80, 40);
			this.lblExtTXX22.Name = "lblExtTXX22";
			this.lblExtTXX22.Size = new System.Drawing.Size(16, 16);
			this.lblExtTXX22.TabIndex = 155;
			this.lblExtTXX22.Text = "2";
			this.lblExtTXX22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExtTX2
			// 
			this.lblExtTX2.Image = null;
			this.lblExtTX2.Location = new System.Drawing.Point(12, 240);
			this.lblExtTX2.Name = "lblExtTX2";
			this.lblExtTX2.Size = new System.Drawing.Size(40, 16);
			this.lblExtTX2.TabIndex = 154;
			this.lblExtTX2.Text = "2m";
			// 
			// chkExtTX23
			// 
			this.chkExtTX23.Image = null;
			this.chkExtTX23.Location = new System.Drawing.Point(96, 240);
			this.chkExtTX23.Name = "chkExtTX23";
			this.chkExtTX23.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX23.TabIndex = 151;
			this.chkExtTX23.Text = "checkBox3";
			this.chkExtTX23.CheckedChanged += new System.EventHandler(this.chkExtTX2_CheckedChanged);
			// 
			// chkExtTX22
			// 
			this.chkExtTX22.Image = null;
			this.chkExtTX22.Location = new System.Drawing.Point(80, 240);
			this.chkExtTX22.Name = "chkExtTX22";
			this.chkExtTX22.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX22.TabIndex = 150;
			this.chkExtTX22.Text = "checkBox2";
			this.chkExtTX22.CheckedChanged += new System.EventHandler(this.chkExtTX2_CheckedChanged);
			// 
			// chkExtTX21
			// 
			this.chkExtTX21.Image = null;
			this.chkExtTX21.Location = new System.Drawing.Point(64, 240);
			this.chkExtTX21.Name = "chkExtTX21";
			this.chkExtTX21.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX21.TabIndex = 149;
			this.chkExtTX21.CheckedChanged += new System.EventHandler(this.chkExtTX2_CheckedChanged);
			// 
			// chkExtTX25
			// 
			this.chkExtTX25.Image = null;
			this.chkExtTX25.Location = new System.Drawing.Point(128, 240);
			this.chkExtTX25.Name = "chkExtTX25";
			this.chkExtTX25.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX25.TabIndex = 153;
			this.chkExtTX25.Text = "checkBox5";
			this.chkExtTX25.CheckedChanged += new System.EventHandler(this.chkExtTX2_CheckedChanged);
			// 
			// chkExtTX24
			// 
			this.chkExtTX24.Image = null;
			this.chkExtTX24.Location = new System.Drawing.Point(112, 240);
			this.chkExtTX24.Name = "chkExtTX24";
			this.chkExtTX24.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX24.TabIndex = 152;
			this.chkExtTX24.Text = "checkBox4";
			this.chkExtTX24.CheckedChanged += new System.EventHandler(this.chkExtTX2_CheckedChanged);
			// 
			// lblExtTX6
			// 
			this.lblExtTX6.Image = null;
			this.lblExtTX6.Location = new System.Drawing.Point(12, 224);
			this.lblExtTX6.Name = "lblExtTX6";
			this.lblExtTX6.Size = new System.Drawing.Size(40, 16);
			this.lblExtTX6.TabIndex = 148;
			this.lblExtTX6.Text = "6m";
			// 
			// chkExtTX63
			// 
			this.chkExtTX63.Image = null;
			this.chkExtTX63.Location = new System.Drawing.Point(96, 224);
			this.chkExtTX63.Name = "chkExtTX63";
			this.chkExtTX63.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX63.TabIndex = 145;
			this.chkExtTX63.Text = "checkBox3";
			this.chkExtTX63.CheckedChanged += new System.EventHandler(this.chkExtTX6_CheckedChanged);
			// 
			// chkExtTX62
			// 
			this.chkExtTX62.Image = null;
			this.chkExtTX62.Location = new System.Drawing.Point(80, 224);
			this.chkExtTX62.Name = "chkExtTX62";
			this.chkExtTX62.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX62.TabIndex = 144;
			this.chkExtTX62.Text = "checkBox2";
			this.chkExtTX62.CheckedChanged += new System.EventHandler(this.chkExtTX6_CheckedChanged);
			// 
			// chkExtTX61
			// 
			this.chkExtTX61.Image = null;
			this.chkExtTX61.Location = new System.Drawing.Point(64, 224);
			this.chkExtTX61.Name = "chkExtTX61";
			this.chkExtTX61.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX61.TabIndex = 143;
			this.chkExtTX61.CheckedChanged += new System.EventHandler(this.chkExtTX6_CheckedChanged);
			// 
			// chkExtTX65
			// 
			this.chkExtTX65.Image = null;
			this.chkExtTX65.Location = new System.Drawing.Point(128, 224);
			this.chkExtTX65.Name = "chkExtTX65";
			this.chkExtTX65.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX65.TabIndex = 147;
			this.chkExtTX65.Text = "checkBox5";
			this.chkExtTX65.CheckedChanged += new System.EventHandler(this.chkExtTX6_CheckedChanged);
			// 
			// chkExtTX64
			// 
			this.chkExtTX64.Image = null;
			this.chkExtTX64.Location = new System.Drawing.Point(112, 224);
			this.chkExtTX64.Name = "chkExtTX64";
			this.chkExtTX64.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX64.TabIndex = 146;
			this.chkExtTX64.Text = "checkBox4";
			this.chkExtTX64.CheckedChanged += new System.EventHandler(this.chkExtTX6_CheckedChanged);
			// 
			// lblExtTX10
			// 
			this.lblExtTX10.Image = null;
			this.lblExtTX10.Location = new System.Drawing.Point(12, 208);
			this.lblExtTX10.Name = "lblExtTX10";
			this.lblExtTX10.Size = new System.Drawing.Size(40, 16);
			this.lblExtTX10.TabIndex = 142;
			this.lblExtTX10.Text = "10m";
			// 
			// chkExtTX103
			// 
			this.chkExtTX103.Image = null;
			this.chkExtTX103.Location = new System.Drawing.Point(96, 208);
			this.chkExtTX103.Name = "chkExtTX103";
			this.chkExtTX103.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX103.TabIndex = 139;
			this.chkExtTX103.Text = "checkBox3";
			this.chkExtTX103.CheckedChanged += new System.EventHandler(this.chkExtTX10_CheckedChanged);
			// 
			// chkExtTX102
			// 
			this.chkExtTX102.Image = null;
			this.chkExtTX102.Location = new System.Drawing.Point(80, 208);
			this.chkExtTX102.Name = "chkExtTX102";
			this.chkExtTX102.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX102.TabIndex = 138;
			this.chkExtTX102.Text = "checkBox2";
			this.chkExtTX102.CheckedChanged += new System.EventHandler(this.chkExtTX10_CheckedChanged);
			// 
			// chkExtTX101
			// 
			this.chkExtTX101.Image = null;
			this.chkExtTX101.Location = new System.Drawing.Point(64, 208);
			this.chkExtTX101.Name = "chkExtTX101";
			this.chkExtTX101.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX101.TabIndex = 137;
			this.chkExtTX101.CheckedChanged += new System.EventHandler(this.chkExtTX10_CheckedChanged);
			// 
			// chkExtTX105
			// 
			this.chkExtTX105.Image = null;
			this.chkExtTX105.Location = new System.Drawing.Point(128, 208);
			this.chkExtTX105.Name = "chkExtTX105";
			this.chkExtTX105.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX105.TabIndex = 141;
			this.chkExtTX105.Text = "checkBox5";
			this.chkExtTX105.CheckedChanged += new System.EventHandler(this.chkExtTX10_CheckedChanged);
			// 
			// chkExtTX104
			// 
			this.chkExtTX104.Image = null;
			this.chkExtTX104.Location = new System.Drawing.Point(112, 208);
			this.chkExtTX104.Name = "chkExtTX104";
			this.chkExtTX104.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX104.TabIndex = 140;
			this.chkExtTX104.Text = "checkBox4";
			this.chkExtTX104.CheckedChanged += new System.EventHandler(this.chkExtTX10_CheckedChanged);
			// 
			// lblExtTX12
			// 
			this.lblExtTX12.Image = null;
			this.lblExtTX12.Location = new System.Drawing.Point(12, 192);
			this.lblExtTX12.Name = "lblExtTX12";
			this.lblExtTX12.Size = new System.Drawing.Size(40, 16);
			this.lblExtTX12.TabIndex = 136;
			this.lblExtTX12.Text = "12m";
			// 
			// chkExtTX123
			// 
			this.chkExtTX123.Image = null;
			this.chkExtTX123.Location = new System.Drawing.Point(96, 192);
			this.chkExtTX123.Name = "chkExtTX123";
			this.chkExtTX123.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX123.TabIndex = 133;
			this.chkExtTX123.Text = "checkBox3";
			this.chkExtTX123.CheckedChanged += new System.EventHandler(this.chkExtTX12_CheckedChanged);
			// 
			// chkExtTX122
			// 
			this.chkExtTX122.Image = null;
			this.chkExtTX122.Location = new System.Drawing.Point(80, 192);
			this.chkExtTX122.Name = "chkExtTX122";
			this.chkExtTX122.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX122.TabIndex = 132;
			this.chkExtTX122.Text = "checkBox2";
			this.chkExtTX122.CheckedChanged += new System.EventHandler(this.chkExtTX12_CheckedChanged);
			// 
			// chkExtTX121
			// 
			this.chkExtTX121.Image = null;
			this.chkExtTX121.Location = new System.Drawing.Point(64, 192);
			this.chkExtTX121.Name = "chkExtTX121";
			this.chkExtTX121.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX121.TabIndex = 131;
			this.chkExtTX121.CheckedChanged += new System.EventHandler(this.chkExtTX12_CheckedChanged);
			// 
			// chkExtTX125
			// 
			this.chkExtTX125.Image = null;
			this.chkExtTX125.Location = new System.Drawing.Point(128, 192);
			this.chkExtTX125.Name = "chkExtTX125";
			this.chkExtTX125.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX125.TabIndex = 135;
			this.chkExtTX125.Text = "checkBox5";
			this.chkExtTX125.CheckedChanged += new System.EventHandler(this.chkExtTX12_CheckedChanged);
			// 
			// chkExtTX124
			// 
			this.chkExtTX124.Image = null;
			this.chkExtTX124.Location = new System.Drawing.Point(112, 192);
			this.chkExtTX124.Name = "chkExtTX124";
			this.chkExtTX124.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX124.TabIndex = 134;
			this.chkExtTX124.Text = "checkBox4";
			this.chkExtTX124.CheckedChanged += new System.EventHandler(this.chkExtTX12_CheckedChanged);
			// 
			// lblExtTX15
			// 
			this.lblExtTX15.Image = null;
			this.lblExtTX15.Location = new System.Drawing.Point(12, 176);
			this.lblExtTX15.Name = "lblExtTX15";
			this.lblExtTX15.Size = new System.Drawing.Size(40, 16);
			this.lblExtTX15.TabIndex = 130;
			this.lblExtTX15.Text = "15m";
			// 
			// chkExtTX153
			// 
			this.chkExtTX153.Image = null;
			this.chkExtTX153.Location = new System.Drawing.Point(96, 176);
			this.chkExtTX153.Name = "chkExtTX153";
			this.chkExtTX153.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX153.TabIndex = 127;
			this.chkExtTX153.Text = "checkBox3";
			this.chkExtTX153.CheckedChanged += new System.EventHandler(this.chkExtTX15_CheckedChanged);
			// 
			// chkExtTX152
			// 
			this.chkExtTX152.Image = null;
			this.chkExtTX152.Location = new System.Drawing.Point(80, 176);
			this.chkExtTX152.Name = "chkExtTX152";
			this.chkExtTX152.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX152.TabIndex = 126;
			this.chkExtTX152.Text = "checkBox2";
			this.chkExtTX152.CheckedChanged += new System.EventHandler(this.chkExtTX15_CheckedChanged);
			// 
			// chkExtTX151
			// 
			this.chkExtTX151.Image = null;
			this.chkExtTX151.Location = new System.Drawing.Point(64, 176);
			this.chkExtTX151.Name = "chkExtTX151";
			this.chkExtTX151.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX151.TabIndex = 125;
			this.chkExtTX151.CheckedChanged += new System.EventHandler(this.chkExtTX15_CheckedChanged);
			// 
			// chkExtTX155
			// 
			this.chkExtTX155.Image = null;
			this.chkExtTX155.Location = new System.Drawing.Point(128, 176);
			this.chkExtTX155.Name = "chkExtTX155";
			this.chkExtTX155.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX155.TabIndex = 129;
			this.chkExtTX155.Text = "checkBox5";
			this.chkExtTX155.CheckedChanged += new System.EventHandler(this.chkExtTX15_CheckedChanged);
			// 
			// chkExtTX154
			// 
			this.chkExtTX154.Image = null;
			this.chkExtTX154.Location = new System.Drawing.Point(112, 176);
			this.chkExtTX154.Name = "chkExtTX154";
			this.chkExtTX154.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX154.TabIndex = 128;
			this.chkExtTX154.Text = "checkBox4";
			this.chkExtTX154.CheckedChanged += new System.EventHandler(this.chkExtTX15_CheckedChanged);
			// 
			// lblExtTX17
			// 
			this.lblExtTX17.Image = null;
			this.lblExtTX17.Location = new System.Drawing.Point(12, 160);
			this.lblExtTX17.Name = "lblExtTX17";
			this.lblExtTX17.Size = new System.Drawing.Size(40, 16);
			this.lblExtTX17.TabIndex = 124;
			this.lblExtTX17.Text = "17m";
			// 
			// chkExtTX173
			// 
			this.chkExtTX173.Image = null;
			this.chkExtTX173.Location = new System.Drawing.Point(96, 160);
			this.chkExtTX173.Name = "chkExtTX173";
			this.chkExtTX173.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX173.TabIndex = 121;
			this.chkExtTX173.Text = "checkBox3";
			this.chkExtTX173.CheckedChanged += new System.EventHandler(this.chkExtTX17_CheckedChanged);
			// 
			// chkExtTX172
			// 
			this.chkExtTX172.Image = null;
			this.chkExtTX172.Location = new System.Drawing.Point(80, 160);
			this.chkExtTX172.Name = "chkExtTX172";
			this.chkExtTX172.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX172.TabIndex = 120;
			this.chkExtTX172.Text = "checkBox2";
			this.chkExtTX172.CheckedChanged += new System.EventHandler(this.chkExtTX17_CheckedChanged);
			// 
			// chkExtTX171
			// 
			this.chkExtTX171.Image = null;
			this.chkExtTX171.Location = new System.Drawing.Point(64, 160);
			this.chkExtTX171.Name = "chkExtTX171";
			this.chkExtTX171.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX171.TabIndex = 119;
			this.chkExtTX171.CheckedChanged += new System.EventHandler(this.chkExtTX17_CheckedChanged);
			// 
			// chkExtTX175
			// 
			this.chkExtTX175.Image = null;
			this.chkExtTX175.Location = new System.Drawing.Point(128, 160);
			this.chkExtTX175.Name = "chkExtTX175";
			this.chkExtTX175.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX175.TabIndex = 123;
			this.chkExtTX175.Text = "checkBox5";
			this.chkExtTX175.CheckedChanged += new System.EventHandler(this.chkExtTX17_CheckedChanged);
			// 
			// chkExtTX174
			// 
			this.chkExtTX174.Image = null;
			this.chkExtTX174.Location = new System.Drawing.Point(112, 160);
			this.chkExtTX174.Name = "chkExtTX174";
			this.chkExtTX174.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX174.TabIndex = 122;
			this.chkExtTX174.Text = "checkBox4";
			this.chkExtTX174.CheckedChanged += new System.EventHandler(this.chkExtTX17_CheckedChanged);
			// 
			// lblExtTX20
			// 
			this.lblExtTX20.Image = null;
			this.lblExtTX20.Location = new System.Drawing.Point(12, 144);
			this.lblExtTX20.Name = "lblExtTX20";
			this.lblExtTX20.Size = new System.Drawing.Size(40, 16);
			this.lblExtTX20.TabIndex = 118;
			this.lblExtTX20.Text = "20m";
			// 
			// chkExtTX203
			// 
			this.chkExtTX203.Image = null;
			this.chkExtTX203.Location = new System.Drawing.Point(96, 144);
			this.chkExtTX203.Name = "chkExtTX203";
			this.chkExtTX203.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX203.TabIndex = 115;
			this.chkExtTX203.Text = "checkBox3";
			this.chkExtTX203.CheckedChanged += new System.EventHandler(this.chkExtTX20_CheckedChanged);
			// 
			// chkExtTX202
			// 
			this.chkExtTX202.Image = null;
			this.chkExtTX202.Location = new System.Drawing.Point(80, 144);
			this.chkExtTX202.Name = "chkExtTX202";
			this.chkExtTX202.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX202.TabIndex = 114;
			this.chkExtTX202.Text = "checkBox2";
			this.chkExtTX202.CheckedChanged += new System.EventHandler(this.chkExtTX20_CheckedChanged);
			// 
			// chkExtTX201
			// 
			this.chkExtTX201.Image = null;
			this.chkExtTX201.Location = new System.Drawing.Point(64, 144);
			this.chkExtTX201.Name = "chkExtTX201";
			this.chkExtTX201.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX201.TabIndex = 113;
			this.chkExtTX201.CheckedChanged += new System.EventHandler(this.chkExtTX20_CheckedChanged);
			// 
			// chkExtTX205
			// 
			this.chkExtTX205.Image = null;
			this.chkExtTX205.Location = new System.Drawing.Point(128, 144);
			this.chkExtTX205.Name = "chkExtTX205";
			this.chkExtTX205.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX205.TabIndex = 117;
			this.chkExtTX205.Text = "checkBox5";
			this.chkExtTX205.CheckedChanged += new System.EventHandler(this.chkExtTX20_CheckedChanged);
			// 
			// chkExtTX204
			// 
			this.chkExtTX204.Image = null;
			this.chkExtTX204.Location = new System.Drawing.Point(112, 144);
			this.chkExtTX204.Name = "chkExtTX204";
			this.chkExtTX204.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX204.TabIndex = 116;
			this.chkExtTX204.Text = "checkBox4";
			this.chkExtTX204.CheckedChanged += new System.EventHandler(this.chkExtTX20_CheckedChanged);
			// 
			// lblExtTX30
			// 
			this.lblExtTX30.Image = null;
			this.lblExtTX30.Location = new System.Drawing.Point(12, 128);
			this.lblExtTX30.Name = "lblExtTX30";
			this.lblExtTX30.Size = new System.Drawing.Size(40, 16);
			this.lblExtTX30.TabIndex = 112;
			this.lblExtTX30.Text = "30m";
			// 
			// chkExtTX303
			// 
			this.chkExtTX303.Image = null;
			this.chkExtTX303.Location = new System.Drawing.Point(96, 128);
			this.chkExtTX303.Name = "chkExtTX303";
			this.chkExtTX303.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX303.TabIndex = 109;
			this.chkExtTX303.Text = "checkBox3";
			this.chkExtTX303.CheckedChanged += new System.EventHandler(this.chkExtTX30_CheckedChanged);
			// 
			// chkExtTX302
			// 
			this.chkExtTX302.Image = null;
			this.chkExtTX302.Location = new System.Drawing.Point(80, 128);
			this.chkExtTX302.Name = "chkExtTX302";
			this.chkExtTX302.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX302.TabIndex = 108;
			this.chkExtTX302.Text = "checkBox2";
			this.chkExtTX302.CheckedChanged += new System.EventHandler(this.chkExtTX30_CheckedChanged);
			// 
			// chkExtTX301
			// 
			this.chkExtTX301.Image = null;
			this.chkExtTX301.Location = new System.Drawing.Point(64, 128);
			this.chkExtTX301.Name = "chkExtTX301";
			this.chkExtTX301.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX301.TabIndex = 107;
			this.chkExtTX301.CheckedChanged += new System.EventHandler(this.chkExtTX30_CheckedChanged);
			// 
			// chkExtTX305
			// 
			this.chkExtTX305.Image = null;
			this.chkExtTX305.Location = new System.Drawing.Point(128, 128);
			this.chkExtTX305.Name = "chkExtTX305";
			this.chkExtTX305.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX305.TabIndex = 111;
			this.chkExtTX305.Text = "checkBox5";
			this.chkExtTX305.CheckedChanged += new System.EventHandler(this.chkExtTX30_CheckedChanged);
			// 
			// chkExtTX304
			// 
			this.chkExtTX304.Image = null;
			this.chkExtTX304.Location = new System.Drawing.Point(112, 128);
			this.chkExtTX304.Name = "chkExtTX304";
			this.chkExtTX304.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX304.TabIndex = 110;
			this.chkExtTX304.Text = "checkBox4";
			this.chkExtTX304.CheckedChanged += new System.EventHandler(this.chkExtTX30_CheckedChanged);
			// 
			// lblExtTX40
			// 
			this.lblExtTX40.Image = null;
			this.lblExtTX40.Location = new System.Drawing.Point(12, 112);
			this.lblExtTX40.Name = "lblExtTX40";
			this.lblExtTX40.Size = new System.Drawing.Size(40, 16);
			this.lblExtTX40.TabIndex = 106;
			this.lblExtTX40.Text = "40m";
			// 
			// chkExtTX403
			// 
			this.chkExtTX403.Image = null;
			this.chkExtTX403.Location = new System.Drawing.Point(96, 112);
			this.chkExtTX403.Name = "chkExtTX403";
			this.chkExtTX403.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX403.TabIndex = 103;
			this.chkExtTX403.Text = "checkBox3";
			this.chkExtTX403.CheckedChanged += new System.EventHandler(this.chkExtTX40_CheckedChanged);
			// 
			// chkExtTX402
			// 
			this.chkExtTX402.Image = null;
			this.chkExtTX402.Location = new System.Drawing.Point(80, 112);
			this.chkExtTX402.Name = "chkExtTX402";
			this.chkExtTX402.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX402.TabIndex = 102;
			this.chkExtTX402.Text = "checkBox2";
			this.chkExtTX402.CheckedChanged += new System.EventHandler(this.chkExtTX40_CheckedChanged);
			// 
			// chkExtTX401
			// 
			this.chkExtTX401.Image = null;
			this.chkExtTX401.Location = new System.Drawing.Point(64, 112);
			this.chkExtTX401.Name = "chkExtTX401";
			this.chkExtTX401.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX401.TabIndex = 101;
			this.chkExtTX401.CheckedChanged += new System.EventHandler(this.chkExtTX40_CheckedChanged);
			// 
			// chkExtTX405
			// 
			this.chkExtTX405.Image = null;
			this.chkExtTX405.Location = new System.Drawing.Point(128, 112);
			this.chkExtTX405.Name = "chkExtTX405";
			this.chkExtTX405.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX405.TabIndex = 105;
			this.chkExtTX405.Text = "checkBox5";
			this.chkExtTX405.CheckedChanged += new System.EventHandler(this.chkExtTX40_CheckedChanged);
			// 
			// chkExtTX404
			// 
			this.chkExtTX404.Image = null;
			this.chkExtTX404.Location = new System.Drawing.Point(112, 112);
			this.chkExtTX404.Name = "chkExtTX404";
			this.chkExtTX404.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX404.TabIndex = 104;
			this.chkExtTX404.Text = "checkBox4";
			this.chkExtTX404.CheckedChanged += new System.EventHandler(this.chkExtTX40_CheckedChanged);
			// 
			// lblExtTX60
			// 
			this.lblExtTX60.Image = null;
			this.lblExtTX60.Location = new System.Drawing.Point(12, 96);
			this.lblExtTX60.Name = "lblExtTX60";
			this.lblExtTX60.Size = new System.Drawing.Size(40, 16);
			this.lblExtTX60.TabIndex = 100;
			this.lblExtTX60.Text = "60m";
			// 
			// chkExtTX603
			// 
			this.chkExtTX603.Image = null;
			this.chkExtTX603.Location = new System.Drawing.Point(96, 96);
			this.chkExtTX603.Name = "chkExtTX603";
			this.chkExtTX603.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX603.TabIndex = 97;
			this.chkExtTX603.Text = "checkBox3";
			this.chkExtTX603.CheckedChanged += new System.EventHandler(this.chkExtTX60_CheckedChanged);
			// 
			// chkExtTX602
			// 
			this.chkExtTX602.Image = null;
			this.chkExtTX602.Location = new System.Drawing.Point(80, 96);
			this.chkExtTX602.Name = "chkExtTX602";
			this.chkExtTX602.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX602.TabIndex = 96;
			this.chkExtTX602.Text = "checkBox2";
			this.chkExtTX602.CheckedChanged += new System.EventHandler(this.chkExtTX60_CheckedChanged);
			// 
			// chkExtTX601
			// 
			this.chkExtTX601.Image = null;
			this.chkExtTX601.Location = new System.Drawing.Point(64, 96);
			this.chkExtTX601.Name = "chkExtTX601";
			this.chkExtTX601.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX601.TabIndex = 95;
			this.chkExtTX601.CheckedChanged += new System.EventHandler(this.chkExtTX60_CheckedChanged);
			// 
			// chkExtTX605
			// 
			this.chkExtTX605.Image = null;
			this.chkExtTX605.Location = new System.Drawing.Point(128, 96);
			this.chkExtTX605.Name = "chkExtTX605";
			this.chkExtTX605.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX605.TabIndex = 99;
			this.chkExtTX605.Text = "checkBox5";
			this.chkExtTX605.CheckedChanged += new System.EventHandler(this.chkExtTX60_CheckedChanged);
			// 
			// chkExtTX604
			// 
			this.chkExtTX604.Image = null;
			this.chkExtTX604.Location = new System.Drawing.Point(112, 96);
			this.chkExtTX604.Name = "chkExtTX604";
			this.chkExtTX604.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX604.TabIndex = 98;
			this.chkExtTX604.Text = "checkBox4";
			this.chkExtTX604.CheckedChanged += new System.EventHandler(this.chkExtTX60_CheckedChanged);
			// 
			// lblExtTX80
			// 
			this.lblExtTX80.Image = null;
			this.lblExtTX80.Location = new System.Drawing.Point(12, 80);
			this.lblExtTX80.Name = "lblExtTX80";
			this.lblExtTX80.Size = new System.Drawing.Size(40, 16);
			this.lblExtTX80.TabIndex = 94;
			this.lblExtTX80.Text = "80m";
			// 
			// chkExtTX803
			// 
			this.chkExtTX803.Image = null;
			this.chkExtTX803.Location = new System.Drawing.Point(96, 80);
			this.chkExtTX803.Name = "chkExtTX803";
			this.chkExtTX803.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX803.TabIndex = 91;
			this.chkExtTX803.Text = "checkBox3";
			this.chkExtTX803.CheckedChanged += new System.EventHandler(this.chkExtTX80_CheckedChanged);
			// 
			// chkExtTX802
			// 
			this.chkExtTX802.Image = null;
			this.chkExtTX802.Location = new System.Drawing.Point(80, 80);
			this.chkExtTX802.Name = "chkExtTX802";
			this.chkExtTX802.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX802.TabIndex = 90;
			this.chkExtTX802.Text = "checkBox107";
			this.chkExtTX802.CheckedChanged += new System.EventHandler(this.chkExtTX80_CheckedChanged);
			// 
			// chkExtTX801
			// 
			this.chkExtTX801.Image = null;
			this.chkExtTX801.Location = new System.Drawing.Point(64, 80);
			this.chkExtTX801.Name = "chkExtTX801";
			this.chkExtTX801.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX801.TabIndex = 89;
			this.chkExtTX801.CheckedChanged += new System.EventHandler(this.chkExtTX80_CheckedChanged);
			// 
			// chkExtTX805
			// 
			this.chkExtTX805.Image = null;
			this.chkExtTX805.Location = new System.Drawing.Point(128, 80);
			this.chkExtTX805.Name = "chkExtTX805";
			this.chkExtTX805.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX805.TabIndex = 93;
			this.chkExtTX805.Text = "checkBox5";
			this.chkExtTX805.CheckedChanged += new System.EventHandler(this.chkExtTX80_CheckedChanged);
			// 
			// chkExtTX804
			// 
			this.chkExtTX804.Image = null;
			this.chkExtTX804.Location = new System.Drawing.Point(112, 80);
			this.chkExtTX804.Name = "chkExtTX804";
			this.chkExtTX804.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX804.TabIndex = 92;
			this.chkExtTX804.Text = "checkBox4";
			this.chkExtTX804.CheckedChanged += new System.EventHandler(this.chkExtTX80_CheckedChanged);
			// 
			// lblExtTXX2Pins
			// 
			this.lblExtTXX2Pins.Image = null;
			this.lblExtTXX2Pins.Location = new System.Drawing.Point(60, 24);
			this.lblExtTXX2Pins.Name = "lblExtTXX2Pins";
			this.lblExtTXX2Pins.Size = new System.Drawing.Size(100, 16);
			this.lblExtTXX2Pins.TabIndex = 88;
			this.lblExtTXX2Pins.Text = "X2 Pins";
			this.lblExtTXX2Pins.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExtTXBand
			// 
			this.lblExtTXBand.Image = null;
			this.lblExtTXBand.Location = new System.Drawing.Point(12, 24);
			this.lblExtTXBand.Name = "lblExtTXBand";
			this.lblExtTXBand.Size = new System.Drawing.Size(32, 16);
			this.lblExtTXBand.TabIndex = 87;
			this.lblExtTXBand.Text = "Band";
			// 
			// lblExtTX160
			// 
			this.lblExtTX160.Image = null;
			this.lblExtTX160.Location = new System.Drawing.Point(12, 64);
			this.lblExtTX160.Name = "lblExtTX160";
			this.lblExtTX160.Size = new System.Drawing.Size(40, 16);
			this.lblExtTX160.TabIndex = 86;
			this.lblExtTX160.Text = "160m";
			// 
			// chkExtTX1603
			// 
			this.chkExtTX1603.Image = null;
			this.chkExtTX1603.Location = new System.Drawing.Point(96, 64);
			this.chkExtTX1603.Name = "chkExtTX1603";
			this.chkExtTX1603.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX1603.TabIndex = 82;
			this.chkExtTX1603.Text = "checkBox3";
			this.chkExtTX1603.CheckedChanged += new System.EventHandler(this.chkExtTX160_CheckedChanged);
			// 
			// chkExtTX1602
			// 
			this.chkExtTX1602.Image = null;
			this.chkExtTX1602.Location = new System.Drawing.Point(80, 64);
			this.chkExtTX1602.Name = "chkExtTX1602";
			this.chkExtTX1602.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX1602.TabIndex = 81;
			this.chkExtTX1602.Text = "checkBox2";
			this.chkExtTX1602.CheckedChanged += new System.EventHandler(this.chkExtTX160_CheckedChanged);
			// 
			// chkExtTX1601
			// 
			this.chkExtTX1601.Image = null;
			this.chkExtTX1601.Location = new System.Drawing.Point(64, 64);
			this.chkExtTX1601.Name = "chkExtTX1601";
			this.chkExtTX1601.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX1601.TabIndex = 80;
			this.chkExtTX1601.CheckedChanged += new System.EventHandler(this.chkExtTX160_CheckedChanged);
			// 
			// lblExtTXX21
			// 
			this.lblExtTXX21.Image = null;
			this.lblExtTXX21.Location = new System.Drawing.Point(64, 40);
			this.lblExtTXX21.Name = "lblExtTXX21";
			this.lblExtTXX21.Size = new System.Drawing.Size(16, 16);
			this.lblExtTXX21.TabIndex = 85;
			this.lblExtTXX21.Text = "1";
			this.lblExtTXX21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chkExtTX1605
			// 
			this.chkExtTX1605.Image = null;
			this.chkExtTX1605.Location = new System.Drawing.Point(128, 64);
			this.chkExtTX1605.Name = "chkExtTX1605";
			this.chkExtTX1605.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX1605.TabIndex = 84;
			this.chkExtTX1605.Text = "checkBox5";
			this.chkExtTX1605.CheckedChanged += new System.EventHandler(this.chkExtTX160_CheckedChanged);
			// 
			// chkExtTX1604
			// 
			this.chkExtTX1604.Image = null;
			this.chkExtTX1604.Location = new System.Drawing.Point(112, 64);
			this.chkExtTX1604.Name = "chkExtTX1604";
			this.chkExtTX1604.Size = new System.Drawing.Size(16, 16);
			this.chkExtTX1604.TabIndex = 83;
			this.chkExtTX1604.Text = "checkBox4";
			this.chkExtTX1604.CheckedChanged += new System.EventHandler(this.chkExtTX160_CheckedChanged);
			// 
			// grpExtRX
			// 
			this.grpExtRX.Controls.Add(this.lblExtRXX26);
			this.grpExtRX.Controls.Add(this.chkExtRX26);
			this.grpExtRX.Controls.Add(this.chkExtRX66);
			this.grpExtRX.Controls.Add(this.chkExtRX106);
			this.grpExtRX.Controls.Add(this.chkExtRX126);
			this.grpExtRX.Controls.Add(this.chkExtRX156);
			this.grpExtRX.Controls.Add(this.chkExtRX176);
			this.grpExtRX.Controls.Add(this.chkExtRX206);
			this.grpExtRX.Controls.Add(this.chkExtRX306);
			this.grpExtRX.Controls.Add(this.chkExtRX406);
			this.grpExtRX.Controls.Add(this.chkExtRX606);
			this.grpExtRX.Controls.Add(this.chkExtRX806);
			this.grpExtRX.Controls.Add(this.chkExtRX1606);
			this.grpExtRX.Controls.Add(this.lblExtRXX25);
			this.grpExtRX.Controls.Add(this.lblExtRXX24);
			this.grpExtRX.Controls.Add(this.lblExtRXX23);
			this.grpExtRX.Controls.Add(this.lblExtRXX22);
			this.grpExtRX.Controls.Add(this.lblExtRX2);
			this.grpExtRX.Controls.Add(this.chkExtRX23);
			this.grpExtRX.Controls.Add(this.chkExtRX22);
			this.grpExtRX.Controls.Add(this.chkExtRX21);
			this.grpExtRX.Controls.Add(this.chkExtRX25);
			this.grpExtRX.Controls.Add(this.chkExtRX24);
			this.grpExtRX.Controls.Add(this.lblExtRX6);
			this.grpExtRX.Controls.Add(this.chkExtRX63);
			this.grpExtRX.Controls.Add(this.chkExtRX62);
			this.grpExtRX.Controls.Add(this.chkExtRX61);
			this.grpExtRX.Controls.Add(this.chkExtRX65);
			this.grpExtRX.Controls.Add(this.chkExtRX64);
			this.grpExtRX.Controls.Add(this.lblExtRX10);
			this.grpExtRX.Controls.Add(this.chkExtRX103);
			this.grpExtRX.Controls.Add(this.chkExtRX102);
			this.grpExtRX.Controls.Add(this.chkExtRX101);
			this.grpExtRX.Controls.Add(this.chkExtRX105);
			this.grpExtRX.Controls.Add(this.chkExtRX104);
			this.grpExtRX.Controls.Add(this.lblExtRX12);
			this.grpExtRX.Controls.Add(this.chkExtRX123);
			this.grpExtRX.Controls.Add(this.chkExtRX122);
			this.grpExtRX.Controls.Add(this.chkExtRX121);
			this.grpExtRX.Controls.Add(this.chkExtRX125);
			this.grpExtRX.Controls.Add(this.chkExtRX124);
			this.grpExtRX.Controls.Add(this.lblExtRX15);
			this.grpExtRX.Controls.Add(this.chkExtRX153);
			this.grpExtRX.Controls.Add(this.chkExtRX152);
			this.grpExtRX.Controls.Add(this.chkExtRX151);
			this.grpExtRX.Controls.Add(this.chkExtRX155);
			this.grpExtRX.Controls.Add(this.chkExtRX154);
			this.grpExtRX.Controls.Add(this.lblExtRX17);
			this.grpExtRX.Controls.Add(this.chkExtRX173);
			this.grpExtRX.Controls.Add(this.chkExtRX172);
			this.grpExtRX.Controls.Add(this.chkExtRX171);
			this.grpExtRX.Controls.Add(this.chkExtRX175);
			this.grpExtRX.Controls.Add(this.chkExtRX174);
			this.grpExtRX.Controls.Add(this.lblExtRX20);
			this.grpExtRX.Controls.Add(this.chkExtRX203);
			this.grpExtRX.Controls.Add(this.chkExtRX202);
			this.grpExtRX.Controls.Add(this.chkExtRX201);
			this.grpExtRX.Controls.Add(this.chkExtRX205);
			this.grpExtRX.Controls.Add(this.chkExtRX204);
			this.grpExtRX.Controls.Add(this.lblExtRX30);
			this.grpExtRX.Controls.Add(this.chkExtRX303);
			this.grpExtRX.Controls.Add(this.chkExtRX302);
			this.grpExtRX.Controls.Add(this.chkExtRX301);
			this.grpExtRX.Controls.Add(this.chkExtRX305);
			this.grpExtRX.Controls.Add(this.chkExtRX304);
			this.grpExtRX.Controls.Add(this.lblExtRX40);
			this.grpExtRX.Controls.Add(this.chkExtRX403);
			this.grpExtRX.Controls.Add(this.chkExtRX402);
			this.grpExtRX.Controls.Add(this.chkExtRX401);
			this.grpExtRX.Controls.Add(this.chkExtRX405);
			this.grpExtRX.Controls.Add(this.chkExtRX404);
			this.grpExtRX.Controls.Add(this.lblExtRX60);
			this.grpExtRX.Controls.Add(this.chkExtRX603);
			this.grpExtRX.Controls.Add(this.chkExtRX602);
			this.grpExtRX.Controls.Add(this.chkExtRX601);
			this.grpExtRX.Controls.Add(this.chkExtRX605);
			this.grpExtRX.Controls.Add(this.chkExtRX604);
			this.grpExtRX.Controls.Add(this.lblExtRX80);
			this.grpExtRX.Controls.Add(this.chkExtRX803);
			this.grpExtRX.Controls.Add(this.chkExtRX802);
			this.grpExtRX.Controls.Add(this.chkExtRX801);
			this.grpExtRX.Controls.Add(this.chkExtRX805);
			this.grpExtRX.Controls.Add(this.chkExtRX804);
			this.grpExtRX.Controls.Add(this.lblExtRXX2Pins);
			this.grpExtRX.Controls.Add(this.lblExtRXBand);
			this.grpExtRX.Controls.Add(this.lblExtRX160);
			this.grpExtRX.Controls.Add(this.chkExtRX1603);
			this.grpExtRX.Controls.Add(this.chkExtRX1602);
			this.grpExtRX.Controls.Add(this.chkExtRX1601);
			this.grpExtRX.Controls.Add(this.lblExtRXX21);
			this.grpExtRX.Controls.Add(this.chkExtRX1605);
			this.grpExtRX.Controls.Add(this.chkExtRX1604);
			this.grpExtRX.Enabled = false;
			this.grpExtRX.Location = new System.Drawing.Point(8, 8);
			this.grpExtRX.Name = "grpExtRX";
			this.grpExtRX.Size = new System.Drawing.Size(168, 264);
			this.grpExtRX.TabIndex = 7;
			this.grpExtRX.TabStop = false;
			this.grpExtRX.Text = "Receive";
			// 
			// lblExtRXX26
			// 
			this.lblExtRXX26.Image = null;
			this.lblExtRXX26.Location = new System.Drawing.Point(144, 40);
			this.lblExtRXX26.Name = "lblExtRXX26";
			this.lblExtRXX26.Size = new System.Drawing.Size(16, 16);
			this.lblExtRXX26.TabIndex = 92;
			this.lblExtRXX26.Text = "6";
			this.lblExtRXX26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chkExtRX26
			// 
			this.chkExtRX26.Image = null;
			this.chkExtRX26.Location = new System.Drawing.Point(144, 240);
			this.chkExtRX26.Name = "chkExtRX26";
			this.chkExtRX26.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX26.TabIndex = 91;
			this.chkExtRX26.Text = "checkBox5";
			this.chkExtRX26.CheckedChanged += new System.EventHandler(this.chkExtRX2_CheckedChanged);
			// 
			// chkExtRX66
			// 
			this.chkExtRX66.Image = null;
			this.chkExtRX66.Location = new System.Drawing.Point(144, 224);
			this.chkExtRX66.Name = "chkExtRX66";
			this.chkExtRX66.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX66.TabIndex = 90;
			this.chkExtRX66.Text = "checkBox5";
			this.chkExtRX66.CheckedChanged += new System.EventHandler(this.chkExtRX6_CheckedChanged);
			// 
			// chkExtRX106
			// 
			this.chkExtRX106.Image = null;
			this.chkExtRX106.Location = new System.Drawing.Point(144, 208);
			this.chkExtRX106.Name = "chkExtRX106";
			this.chkExtRX106.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX106.TabIndex = 89;
			this.chkExtRX106.Text = "checkBox5";
			this.chkExtRX106.CheckedChanged += new System.EventHandler(this.chkExtRX10_CheckedChanged);
			// 
			// chkExtRX126
			// 
			this.chkExtRX126.Image = null;
			this.chkExtRX126.Location = new System.Drawing.Point(144, 192);
			this.chkExtRX126.Name = "chkExtRX126";
			this.chkExtRX126.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX126.TabIndex = 88;
			this.chkExtRX126.Text = "checkBox5";
			this.chkExtRX126.CheckedChanged += new System.EventHandler(this.chkExtRX12_CheckedChanged);
			// 
			// chkExtRX156
			// 
			this.chkExtRX156.Image = null;
			this.chkExtRX156.Location = new System.Drawing.Point(144, 176);
			this.chkExtRX156.Name = "chkExtRX156";
			this.chkExtRX156.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX156.TabIndex = 87;
			this.chkExtRX156.Text = "checkBox5";
			this.chkExtRX156.CheckedChanged += new System.EventHandler(this.chkExtRX15_CheckedChanged);
			// 
			// chkExtRX176
			// 
			this.chkExtRX176.Image = null;
			this.chkExtRX176.Location = new System.Drawing.Point(144, 160);
			this.chkExtRX176.Name = "chkExtRX176";
			this.chkExtRX176.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX176.TabIndex = 86;
			this.chkExtRX176.Text = "checkBox5";
			this.chkExtRX176.CheckedChanged += new System.EventHandler(this.chkExtRX17_CheckedChanged);
			// 
			// chkExtRX206
			// 
			this.chkExtRX206.Image = null;
			this.chkExtRX206.Location = new System.Drawing.Point(144, 144);
			this.chkExtRX206.Name = "chkExtRX206";
			this.chkExtRX206.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX206.TabIndex = 85;
			this.chkExtRX206.Text = "checkBox5";
			this.chkExtRX206.CheckedChanged += new System.EventHandler(this.chkExtRX20_CheckedChanged);
			// 
			// chkExtRX306
			// 
			this.chkExtRX306.Image = null;
			this.chkExtRX306.Location = new System.Drawing.Point(144, 128);
			this.chkExtRX306.Name = "chkExtRX306";
			this.chkExtRX306.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX306.TabIndex = 84;
			this.chkExtRX306.Text = "checkBox5";
			this.chkExtRX306.CheckedChanged += new System.EventHandler(this.chkExtRX30_CheckedChanged);
			// 
			// chkExtRX406
			// 
			this.chkExtRX406.Image = null;
			this.chkExtRX406.Location = new System.Drawing.Point(144, 112);
			this.chkExtRX406.Name = "chkExtRX406";
			this.chkExtRX406.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX406.TabIndex = 83;
			this.chkExtRX406.Text = "checkBox5";
			this.chkExtRX406.CheckedChanged += new System.EventHandler(this.chkExtRX40_CheckedChanged);
			// 
			// chkExtRX606
			// 
			this.chkExtRX606.Image = null;
			this.chkExtRX606.Location = new System.Drawing.Point(144, 96);
			this.chkExtRX606.Name = "chkExtRX606";
			this.chkExtRX606.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX606.TabIndex = 82;
			this.chkExtRX606.Text = "checkBox5";
			this.chkExtRX606.CheckedChanged += new System.EventHandler(this.chkExtRX60_CheckedChanged);
			// 
			// chkExtRX806
			// 
			this.chkExtRX806.Image = null;
			this.chkExtRX806.Location = new System.Drawing.Point(144, 80);
			this.chkExtRX806.Name = "chkExtRX806";
			this.chkExtRX806.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX806.TabIndex = 81;
			this.chkExtRX806.Text = "checkBox5";
			this.chkExtRX806.CheckedChanged += new System.EventHandler(this.chkExtRX80_CheckedChanged);
			// 
			// chkExtRX1606
			// 
			this.chkExtRX1606.Image = null;
			this.chkExtRX1606.Location = new System.Drawing.Point(144, 64);
			this.chkExtRX1606.Name = "chkExtRX1606";
			this.chkExtRX1606.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX1606.TabIndex = 80;
			this.chkExtRX1606.Text = "checkBox5";
			this.chkExtRX1606.CheckedChanged += new System.EventHandler(this.chkExtRX160_CheckedChanged);
			// 
			// lblExtRXX25
			// 
			this.lblExtRXX25.Image = null;
			this.lblExtRXX25.Location = new System.Drawing.Point(128, 40);
			this.lblExtRXX25.Name = "lblExtRXX25";
			this.lblExtRXX25.Size = new System.Drawing.Size(16, 16);
			this.lblExtRXX25.TabIndex = 79;
			this.lblExtRXX25.Text = "5";
			this.lblExtRXX25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExtRXX24
			// 
			this.lblExtRXX24.Image = null;
			this.lblExtRXX24.Location = new System.Drawing.Point(112, 40);
			this.lblExtRXX24.Name = "lblExtRXX24";
			this.lblExtRXX24.Size = new System.Drawing.Size(16, 16);
			this.lblExtRXX24.TabIndex = 78;
			this.lblExtRXX24.Text = "4";
			this.lblExtRXX24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExtRXX23
			// 
			this.lblExtRXX23.Image = null;
			this.lblExtRXX23.Location = new System.Drawing.Point(96, 40);
			this.lblExtRXX23.Name = "lblExtRXX23";
			this.lblExtRXX23.Size = new System.Drawing.Size(16, 16);
			this.lblExtRXX23.TabIndex = 77;
			this.lblExtRXX23.Text = "3";
			this.lblExtRXX23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExtRXX22
			// 
			this.lblExtRXX22.Image = null;
			this.lblExtRXX22.Location = new System.Drawing.Point(80, 40);
			this.lblExtRXX22.Name = "lblExtRXX22";
			this.lblExtRXX22.Size = new System.Drawing.Size(16, 16);
			this.lblExtRXX22.TabIndex = 76;
			this.lblExtRXX22.Text = "2";
			this.lblExtRXX22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExtRX2
			// 
			this.lblExtRX2.Image = null;
			this.lblExtRX2.Location = new System.Drawing.Point(16, 240);
			this.lblExtRX2.Name = "lblExtRX2";
			this.lblExtRX2.Size = new System.Drawing.Size(40, 16);
			this.lblExtRX2.TabIndex = 75;
			this.lblExtRX2.Text = "2m";
			// 
			// chkExtRX23
			// 
			this.chkExtRX23.Image = null;
			this.chkExtRX23.Location = new System.Drawing.Point(96, 240);
			this.chkExtRX23.Name = "chkExtRX23";
			this.chkExtRX23.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX23.TabIndex = 72;
			this.chkExtRX23.Text = "checkBox3";
			this.chkExtRX23.CheckedChanged += new System.EventHandler(this.chkExtRX2_CheckedChanged);
			// 
			// chkExtRX22
			// 
			this.chkExtRX22.Image = null;
			this.chkExtRX22.Location = new System.Drawing.Point(80, 240);
			this.chkExtRX22.Name = "chkExtRX22";
			this.chkExtRX22.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX22.TabIndex = 71;
			this.chkExtRX22.Text = "checkBox2";
			this.chkExtRX22.CheckedChanged += new System.EventHandler(this.chkExtRX2_CheckedChanged);
			// 
			// chkExtRX21
			// 
			this.chkExtRX21.Image = null;
			this.chkExtRX21.Location = new System.Drawing.Point(64, 240);
			this.chkExtRX21.Name = "chkExtRX21";
			this.chkExtRX21.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX21.TabIndex = 70;
			this.chkExtRX21.CheckedChanged += new System.EventHandler(this.chkExtRX2_CheckedChanged);
			// 
			// chkExtRX25
			// 
			this.chkExtRX25.Image = null;
			this.chkExtRX25.Location = new System.Drawing.Point(128, 240);
			this.chkExtRX25.Name = "chkExtRX25";
			this.chkExtRX25.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX25.TabIndex = 74;
			this.chkExtRX25.Text = "checkBox5";
			this.chkExtRX25.CheckedChanged += new System.EventHandler(this.chkExtRX2_CheckedChanged);
			// 
			// chkExtRX24
			// 
			this.chkExtRX24.Image = null;
			this.chkExtRX24.Location = new System.Drawing.Point(112, 240);
			this.chkExtRX24.Name = "chkExtRX24";
			this.chkExtRX24.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX24.TabIndex = 73;
			this.chkExtRX24.Text = "checkBox4";
			this.chkExtRX24.CheckedChanged += new System.EventHandler(this.chkExtRX2_CheckedChanged);
			// 
			// lblExtRX6
			// 
			this.lblExtRX6.Image = null;
			this.lblExtRX6.Location = new System.Drawing.Point(16, 224);
			this.lblExtRX6.Name = "lblExtRX6";
			this.lblExtRX6.Size = new System.Drawing.Size(40, 16);
			this.lblExtRX6.TabIndex = 69;
			this.lblExtRX6.Text = "6m";
			// 
			// chkExtRX63
			// 
			this.chkExtRX63.Image = null;
			this.chkExtRX63.Location = new System.Drawing.Point(96, 224);
			this.chkExtRX63.Name = "chkExtRX63";
			this.chkExtRX63.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX63.TabIndex = 66;
			this.chkExtRX63.Text = "checkBox3";
			this.chkExtRX63.CheckedChanged += new System.EventHandler(this.chkExtRX6_CheckedChanged);
			// 
			// chkExtRX62
			// 
			this.chkExtRX62.Image = null;
			this.chkExtRX62.Location = new System.Drawing.Point(80, 224);
			this.chkExtRX62.Name = "chkExtRX62";
			this.chkExtRX62.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX62.TabIndex = 65;
			this.chkExtRX62.Text = "checkBox2";
			this.chkExtRX62.CheckedChanged += new System.EventHandler(this.chkExtRX6_CheckedChanged);
			// 
			// chkExtRX61
			// 
			this.chkExtRX61.Image = null;
			this.chkExtRX61.Location = new System.Drawing.Point(64, 224);
			this.chkExtRX61.Name = "chkExtRX61";
			this.chkExtRX61.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX61.TabIndex = 64;
			this.chkExtRX61.CheckedChanged += new System.EventHandler(this.chkExtRX6_CheckedChanged);
			// 
			// chkExtRX65
			// 
			this.chkExtRX65.Image = null;
			this.chkExtRX65.Location = new System.Drawing.Point(128, 224);
			this.chkExtRX65.Name = "chkExtRX65";
			this.chkExtRX65.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX65.TabIndex = 68;
			this.chkExtRX65.Text = "checkBox5";
			this.chkExtRX65.CheckedChanged += new System.EventHandler(this.chkExtRX6_CheckedChanged);
			// 
			// chkExtRX64
			// 
			this.chkExtRX64.Image = null;
			this.chkExtRX64.Location = new System.Drawing.Point(112, 224);
			this.chkExtRX64.Name = "chkExtRX64";
			this.chkExtRX64.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX64.TabIndex = 67;
			this.chkExtRX64.Text = "checkBox4";
			this.chkExtRX64.CheckedChanged += new System.EventHandler(this.chkExtRX6_CheckedChanged);
			// 
			// lblExtRX10
			// 
			this.lblExtRX10.Image = null;
			this.lblExtRX10.Location = new System.Drawing.Point(16, 208);
			this.lblExtRX10.Name = "lblExtRX10";
			this.lblExtRX10.Size = new System.Drawing.Size(40, 16);
			this.lblExtRX10.TabIndex = 63;
			this.lblExtRX10.Text = "10m";
			// 
			// chkExtRX103
			// 
			this.chkExtRX103.Image = null;
			this.chkExtRX103.Location = new System.Drawing.Point(96, 208);
			this.chkExtRX103.Name = "chkExtRX103";
			this.chkExtRX103.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX103.TabIndex = 60;
			this.chkExtRX103.Text = "checkBox3";
			this.chkExtRX103.CheckedChanged += new System.EventHandler(this.chkExtRX10_CheckedChanged);
			// 
			// chkExtRX102
			// 
			this.chkExtRX102.Image = null;
			this.chkExtRX102.Location = new System.Drawing.Point(80, 208);
			this.chkExtRX102.Name = "chkExtRX102";
			this.chkExtRX102.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX102.TabIndex = 59;
			this.chkExtRX102.Text = "checkBox2";
			this.chkExtRX102.CheckedChanged += new System.EventHandler(this.chkExtRX10_CheckedChanged);
			// 
			// chkExtRX101
			// 
			this.chkExtRX101.Image = null;
			this.chkExtRX101.Location = new System.Drawing.Point(64, 208);
			this.chkExtRX101.Name = "chkExtRX101";
			this.chkExtRX101.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX101.TabIndex = 58;
			this.chkExtRX101.CheckedChanged += new System.EventHandler(this.chkExtRX10_CheckedChanged);
			// 
			// chkExtRX105
			// 
			this.chkExtRX105.Image = null;
			this.chkExtRX105.Location = new System.Drawing.Point(128, 208);
			this.chkExtRX105.Name = "chkExtRX105";
			this.chkExtRX105.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX105.TabIndex = 62;
			this.chkExtRX105.Text = "checkBox5";
			this.chkExtRX105.CheckedChanged += new System.EventHandler(this.chkExtRX10_CheckedChanged);
			// 
			// chkExtRX104
			// 
			this.chkExtRX104.Image = null;
			this.chkExtRX104.Location = new System.Drawing.Point(112, 208);
			this.chkExtRX104.Name = "chkExtRX104";
			this.chkExtRX104.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX104.TabIndex = 61;
			this.chkExtRX104.Text = "checkBox4";
			this.chkExtRX104.CheckedChanged += new System.EventHandler(this.chkExtRX10_CheckedChanged);
			// 
			// lblExtRX12
			// 
			this.lblExtRX12.Image = null;
			this.lblExtRX12.Location = new System.Drawing.Point(16, 192);
			this.lblExtRX12.Name = "lblExtRX12";
			this.lblExtRX12.Size = new System.Drawing.Size(40, 16);
			this.lblExtRX12.TabIndex = 57;
			this.lblExtRX12.Text = "12m";
			// 
			// chkExtRX123
			// 
			this.chkExtRX123.Image = null;
			this.chkExtRX123.Location = new System.Drawing.Point(96, 192);
			this.chkExtRX123.Name = "chkExtRX123";
			this.chkExtRX123.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX123.TabIndex = 54;
			this.chkExtRX123.Text = "checkBox3";
			this.chkExtRX123.CheckedChanged += new System.EventHandler(this.chkExtRX12_CheckedChanged);
			// 
			// chkExtRX122
			// 
			this.chkExtRX122.Image = null;
			this.chkExtRX122.Location = new System.Drawing.Point(80, 192);
			this.chkExtRX122.Name = "chkExtRX122";
			this.chkExtRX122.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX122.TabIndex = 53;
			this.chkExtRX122.Text = "checkBox2";
			this.chkExtRX122.CheckedChanged += new System.EventHandler(this.chkExtRX12_CheckedChanged);
			// 
			// chkExtRX121
			// 
			this.chkExtRX121.Image = null;
			this.chkExtRX121.Location = new System.Drawing.Point(64, 192);
			this.chkExtRX121.Name = "chkExtRX121";
			this.chkExtRX121.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX121.TabIndex = 52;
			this.chkExtRX121.CheckedChanged += new System.EventHandler(this.chkExtRX12_CheckedChanged);
			// 
			// chkExtRX125
			// 
			this.chkExtRX125.Image = null;
			this.chkExtRX125.Location = new System.Drawing.Point(128, 192);
			this.chkExtRX125.Name = "chkExtRX125";
			this.chkExtRX125.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX125.TabIndex = 56;
			this.chkExtRX125.Text = "checkBox5";
			this.chkExtRX125.CheckedChanged += new System.EventHandler(this.chkExtRX12_CheckedChanged);
			// 
			// chkExtRX124
			// 
			this.chkExtRX124.Image = null;
			this.chkExtRX124.Location = new System.Drawing.Point(112, 192);
			this.chkExtRX124.Name = "chkExtRX124";
			this.chkExtRX124.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX124.TabIndex = 55;
			this.chkExtRX124.Text = "checkBox4";
			this.chkExtRX124.CheckedChanged += new System.EventHandler(this.chkExtRX12_CheckedChanged);
			// 
			// lblExtRX15
			// 
			this.lblExtRX15.Image = null;
			this.lblExtRX15.Location = new System.Drawing.Point(16, 176);
			this.lblExtRX15.Name = "lblExtRX15";
			this.lblExtRX15.Size = new System.Drawing.Size(40, 16);
			this.lblExtRX15.TabIndex = 51;
			this.lblExtRX15.Text = "15m";
			// 
			// chkExtRX153
			// 
			this.chkExtRX153.Image = null;
			this.chkExtRX153.Location = new System.Drawing.Point(96, 176);
			this.chkExtRX153.Name = "chkExtRX153";
			this.chkExtRX153.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX153.TabIndex = 48;
			this.chkExtRX153.Text = "checkBox3";
			this.chkExtRX153.CheckedChanged += new System.EventHandler(this.chkExtRX15_CheckedChanged);
			// 
			// chkExtRX152
			// 
			this.chkExtRX152.Image = null;
			this.chkExtRX152.Location = new System.Drawing.Point(80, 176);
			this.chkExtRX152.Name = "chkExtRX152";
			this.chkExtRX152.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX152.TabIndex = 47;
			this.chkExtRX152.Text = "checkBox2";
			this.chkExtRX152.CheckedChanged += new System.EventHandler(this.chkExtRX15_CheckedChanged);
			// 
			// chkExtRX151
			// 
			this.chkExtRX151.Image = null;
			this.chkExtRX151.Location = new System.Drawing.Point(64, 176);
			this.chkExtRX151.Name = "chkExtRX151";
			this.chkExtRX151.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX151.TabIndex = 46;
			this.chkExtRX151.CheckedChanged += new System.EventHandler(this.chkExtRX15_CheckedChanged);
			// 
			// chkExtRX155
			// 
			this.chkExtRX155.Image = null;
			this.chkExtRX155.Location = new System.Drawing.Point(128, 176);
			this.chkExtRX155.Name = "chkExtRX155";
			this.chkExtRX155.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX155.TabIndex = 50;
			this.chkExtRX155.Text = "checkBox5";
			this.chkExtRX155.CheckedChanged += new System.EventHandler(this.chkExtRX15_CheckedChanged);
			// 
			// chkExtRX154
			// 
			this.chkExtRX154.Image = null;
			this.chkExtRX154.Location = new System.Drawing.Point(112, 176);
			this.chkExtRX154.Name = "chkExtRX154";
			this.chkExtRX154.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX154.TabIndex = 49;
			this.chkExtRX154.Text = "checkBox4";
			this.chkExtRX154.CheckedChanged += new System.EventHandler(this.chkExtRX15_CheckedChanged);
			// 
			// lblExtRX17
			// 
			this.lblExtRX17.Image = null;
			this.lblExtRX17.Location = new System.Drawing.Point(16, 160);
			this.lblExtRX17.Name = "lblExtRX17";
			this.lblExtRX17.Size = new System.Drawing.Size(40, 16);
			this.lblExtRX17.TabIndex = 45;
			this.lblExtRX17.Text = "17m";
			// 
			// chkExtRX173
			// 
			this.chkExtRX173.Image = null;
			this.chkExtRX173.Location = new System.Drawing.Point(96, 160);
			this.chkExtRX173.Name = "chkExtRX173";
			this.chkExtRX173.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX173.TabIndex = 42;
			this.chkExtRX173.Text = "checkBox3";
			this.chkExtRX173.CheckedChanged += new System.EventHandler(this.chkExtRX17_CheckedChanged);
			// 
			// chkExtRX172
			// 
			this.chkExtRX172.Image = null;
			this.chkExtRX172.Location = new System.Drawing.Point(80, 160);
			this.chkExtRX172.Name = "chkExtRX172";
			this.chkExtRX172.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX172.TabIndex = 41;
			this.chkExtRX172.Text = "checkBox2";
			this.chkExtRX172.CheckedChanged += new System.EventHandler(this.chkExtRX17_CheckedChanged);
			// 
			// chkExtRX171
			// 
			this.chkExtRX171.Image = null;
			this.chkExtRX171.Location = new System.Drawing.Point(64, 160);
			this.chkExtRX171.Name = "chkExtRX171";
			this.chkExtRX171.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX171.TabIndex = 40;
			this.chkExtRX171.CheckedChanged += new System.EventHandler(this.chkExtRX17_CheckedChanged);
			// 
			// chkExtRX175
			// 
			this.chkExtRX175.Image = null;
			this.chkExtRX175.Location = new System.Drawing.Point(128, 160);
			this.chkExtRX175.Name = "chkExtRX175";
			this.chkExtRX175.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX175.TabIndex = 44;
			this.chkExtRX175.Text = "checkBox5";
			this.chkExtRX175.CheckedChanged += new System.EventHandler(this.chkExtRX17_CheckedChanged);
			// 
			// chkExtRX174
			// 
			this.chkExtRX174.Image = null;
			this.chkExtRX174.Location = new System.Drawing.Point(112, 160);
			this.chkExtRX174.Name = "chkExtRX174";
			this.chkExtRX174.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX174.TabIndex = 43;
			this.chkExtRX174.Text = "checkBox4";
			this.chkExtRX174.CheckedChanged += new System.EventHandler(this.chkExtRX17_CheckedChanged);
			// 
			// lblExtRX20
			// 
			this.lblExtRX20.Image = null;
			this.lblExtRX20.Location = new System.Drawing.Point(16, 144);
			this.lblExtRX20.Name = "lblExtRX20";
			this.lblExtRX20.Size = new System.Drawing.Size(40, 16);
			this.lblExtRX20.TabIndex = 39;
			this.lblExtRX20.Text = "20m";
			// 
			// chkExtRX203
			// 
			this.chkExtRX203.Image = null;
			this.chkExtRX203.Location = new System.Drawing.Point(96, 144);
			this.chkExtRX203.Name = "chkExtRX203";
			this.chkExtRX203.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX203.TabIndex = 36;
			this.chkExtRX203.Text = "checkBox3";
			this.chkExtRX203.CheckedChanged += new System.EventHandler(this.chkExtRX20_CheckedChanged);
			// 
			// chkExtRX202
			// 
			this.chkExtRX202.Image = null;
			this.chkExtRX202.Location = new System.Drawing.Point(80, 144);
			this.chkExtRX202.Name = "chkExtRX202";
			this.chkExtRX202.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX202.TabIndex = 35;
			this.chkExtRX202.Text = "checkBox2";
			this.chkExtRX202.CheckedChanged += new System.EventHandler(this.chkExtRX20_CheckedChanged);
			// 
			// chkExtRX201
			// 
			this.chkExtRX201.Image = null;
			this.chkExtRX201.Location = new System.Drawing.Point(64, 144);
			this.chkExtRX201.Name = "chkExtRX201";
			this.chkExtRX201.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX201.TabIndex = 34;
			this.chkExtRX201.CheckedChanged += new System.EventHandler(this.chkExtRX20_CheckedChanged);
			// 
			// chkExtRX205
			// 
			this.chkExtRX205.Image = null;
			this.chkExtRX205.Location = new System.Drawing.Point(128, 144);
			this.chkExtRX205.Name = "chkExtRX205";
			this.chkExtRX205.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX205.TabIndex = 38;
			this.chkExtRX205.Text = "checkBox5";
			this.chkExtRX205.CheckedChanged += new System.EventHandler(this.chkExtRX20_CheckedChanged);
			// 
			// chkExtRX204
			// 
			this.chkExtRX204.Image = null;
			this.chkExtRX204.Location = new System.Drawing.Point(112, 144);
			this.chkExtRX204.Name = "chkExtRX204";
			this.chkExtRX204.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX204.TabIndex = 37;
			this.chkExtRX204.Text = "checkBox4";
			this.chkExtRX204.CheckedChanged += new System.EventHandler(this.chkExtRX20_CheckedChanged);
			// 
			// lblExtRX30
			// 
			this.lblExtRX30.Image = null;
			this.lblExtRX30.Location = new System.Drawing.Point(16, 128);
			this.lblExtRX30.Name = "lblExtRX30";
			this.lblExtRX30.Size = new System.Drawing.Size(40, 16);
			this.lblExtRX30.TabIndex = 33;
			this.lblExtRX30.Text = "30m";
			// 
			// chkExtRX303
			// 
			this.chkExtRX303.Image = null;
			this.chkExtRX303.Location = new System.Drawing.Point(96, 128);
			this.chkExtRX303.Name = "chkExtRX303";
			this.chkExtRX303.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX303.TabIndex = 30;
			this.chkExtRX303.Text = "checkBox3";
			this.chkExtRX303.CheckedChanged += new System.EventHandler(this.chkExtRX30_CheckedChanged);
			// 
			// chkExtRX302
			// 
			this.chkExtRX302.Image = null;
			this.chkExtRX302.Location = new System.Drawing.Point(80, 128);
			this.chkExtRX302.Name = "chkExtRX302";
			this.chkExtRX302.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX302.TabIndex = 29;
			this.chkExtRX302.Text = "checkBox2";
			this.chkExtRX302.CheckedChanged += new System.EventHandler(this.chkExtRX30_CheckedChanged);
			// 
			// chkExtRX301
			// 
			this.chkExtRX301.Image = null;
			this.chkExtRX301.Location = new System.Drawing.Point(64, 128);
			this.chkExtRX301.Name = "chkExtRX301";
			this.chkExtRX301.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX301.TabIndex = 28;
			this.chkExtRX301.CheckedChanged += new System.EventHandler(this.chkExtRX30_CheckedChanged);
			// 
			// chkExtRX305
			// 
			this.chkExtRX305.Image = null;
			this.chkExtRX305.Location = new System.Drawing.Point(128, 128);
			this.chkExtRX305.Name = "chkExtRX305";
			this.chkExtRX305.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX305.TabIndex = 32;
			this.chkExtRX305.Text = "checkBox5";
			this.chkExtRX305.CheckedChanged += new System.EventHandler(this.chkExtRX30_CheckedChanged);
			// 
			// chkExtRX304
			// 
			this.chkExtRX304.Image = null;
			this.chkExtRX304.Location = new System.Drawing.Point(112, 128);
			this.chkExtRX304.Name = "chkExtRX304";
			this.chkExtRX304.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX304.TabIndex = 31;
			this.chkExtRX304.Text = "checkBox4";
			this.chkExtRX304.CheckedChanged += new System.EventHandler(this.chkExtRX30_CheckedChanged);
			// 
			// lblExtRX40
			// 
			this.lblExtRX40.Image = null;
			this.lblExtRX40.Location = new System.Drawing.Point(16, 112);
			this.lblExtRX40.Name = "lblExtRX40";
			this.lblExtRX40.Size = new System.Drawing.Size(40, 16);
			this.lblExtRX40.TabIndex = 27;
			this.lblExtRX40.Text = "40m";
			// 
			// chkExtRX403
			// 
			this.chkExtRX403.Image = null;
			this.chkExtRX403.Location = new System.Drawing.Point(96, 112);
			this.chkExtRX403.Name = "chkExtRX403";
			this.chkExtRX403.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX403.TabIndex = 24;
			this.chkExtRX403.Text = "checkBox3";
			this.chkExtRX403.CheckedChanged += new System.EventHandler(this.chkExtRX40_CheckedChanged);
			// 
			// chkExtRX402
			// 
			this.chkExtRX402.Image = null;
			this.chkExtRX402.Location = new System.Drawing.Point(80, 112);
			this.chkExtRX402.Name = "chkExtRX402";
			this.chkExtRX402.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX402.TabIndex = 23;
			this.chkExtRX402.Text = "checkBox2";
			this.chkExtRX402.CheckedChanged += new System.EventHandler(this.chkExtRX40_CheckedChanged);
			// 
			// chkExtRX401
			// 
			this.chkExtRX401.Image = null;
			this.chkExtRX401.Location = new System.Drawing.Point(64, 112);
			this.chkExtRX401.Name = "chkExtRX401";
			this.chkExtRX401.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX401.TabIndex = 22;
			this.chkExtRX401.CheckedChanged += new System.EventHandler(this.chkExtRX40_CheckedChanged);
			// 
			// chkExtRX405
			// 
			this.chkExtRX405.Image = null;
			this.chkExtRX405.Location = new System.Drawing.Point(128, 112);
			this.chkExtRX405.Name = "chkExtRX405";
			this.chkExtRX405.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX405.TabIndex = 26;
			this.chkExtRX405.Text = "checkBox5";
			this.chkExtRX405.CheckedChanged += new System.EventHandler(this.chkExtRX40_CheckedChanged);
			// 
			// chkExtRX404
			// 
			this.chkExtRX404.Image = null;
			this.chkExtRX404.Location = new System.Drawing.Point(112, 112);
			this.chkExtRX404.Name = "chkExtRX404";
			this.chkExtRX404.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX404.TabIndex = 25;
			this.chkExtRX404.Text = "checkBox4";
			this.chkExtRX404.CheckedChanged += new System.EventHandler(this.chkExtRX40_CheckedChanged);
			// 
			// lblExtRX60
			// 
			this.lblExtRX60.Image = null;
			this.lblExtRX60.Location = new System.Drawing.Point(16, 96);
			this.lblExtRX60.Name = "lblExtRX60";
			this.lblExtRX60.Size = new System.Drawing.Size(40, 16);
			this.lblExtRX60.TabIndex = 21;
			this.lblExtRX60.Text = "60m";
			// 
			// chkExtRX603
			// 
			this.chkExtRX603.Image = null;
			this.chkExtRX603.Location = new System.Drawing.Point(96, 96);
			this.chkExtRX603.Name = "chkExtRX603";
			this.chkExtRX603.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX603.TabIndex = 18;
			this.chkExtRX603.Text = "checkBox3";
			this.chkExtRX603.CheckedChanged += new System.EventHandler(this.chkExtRX60_CheckedChanged);
			// 
			// chkExtRX602
			// 
			this.chkExtRX602.Image = null;
			this.chkExtRX602.Location = new System.Drawing.Point(80, 96);
			this.chkExtRX602.Name = "chkExtRX602";
			this.chkExtRX602.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX602.TabIndex = 17;
			this.chkExtRX602.Text = "checkBox2";
			this.chkExtRX602.CheckedChanged += new System.EventHandler(this.chkExtRX60_CheckedChanged);
			// 
			// chkExtRX601
			// 
			this.chkExtRX601.Image = null;
			this.chkExtRX601.Location = new System.Drawing.Point(64, 96);
			this.chkExtRX601.Name = "chkExtRX601";
			this.chkExtRX601.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX601.TabIndex = 16;
			this.chkExtRX601.CheckedChanged += new System.EventHandler(this.chkExtRX60_CheckedChanged);
			// 
			// chkExtRX605
			// 
			this.chkExtRX605.Image = null;
			this.chkExtRX605.Location = new System.Drawing.Point(128, 96);
			this.chkExtRX605.Name = "chkExtRX605";
			this.chkExtRX605.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX605.TabIndex = 20;
			this.chkExtRX605.Text = "checkBox5";
			this.chkExtRX605.CheckedChanged += new System.EventHandler(this.chkExtRX60_CheckedChanged);
			// 
			// chkExtRX604
			// 
			this.chkExtRX604.Image = null;
			this.chkExtRX604.Location = new System.Drawing.Point(112, 96);
			this.chkExtRX604.Name = "chkExtRX604";
			this.chkExtRX604.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX604.TabIndex = 19;
			this.chkExtRX604.Text = "checkBox4";
			this.chkExtRX604.CheckedChanged += new System.EventHandler(this.chkExtRX60_CheckedChanged);
			// 
			// lblExtRX80
			// 
			this.lblExtRX80.Image = null;
			this.lblExtRX80.Location = new System.Drawing.Point(16, 80);
			this.lblExtRX80.Name = "lblExtRX80";
			this.lblExtRX80.Size = new System.Drawing.Size(40, 16);
			this.lblExtRX80.TabIndex = 15;
			this.lblExtRX80.Text = "80m";
			// 
			// chkExtRX803
			// 
			this.chkExtRX803.Image = null;
			this.chkExtRX803.Location = new System.Drawing.Point(96, 80);
			this.chkExtRX803.Name = "chkExtRX803";
			this.chkExtRX803.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX803.TabIndex = 12;
			this.chkExtRX803.Text = "checkBox3";
			this.chkExtRX803.CheckedChanged += new System.EventHandler(this.chkExtRX80_CheckedChanged);
			// 
			// chkExtRX802
			// 
			this.chkExtRX802.Image = null;
			this.chkExtRX802.Location = new System.Drawing.Point(80, 80);
			this.chkExtRX802.Name = "chkExtRX802";
			this.chkExtRX802.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX802.TabIndex = 11;
			this.chkExtRX802.Text = "checkBox2";
			this.chkExtRX802.CheckedChanged += new System.EventHandler(this.chkExtRX80_CheckedChanged);
			// 
			// chkExtRX801
			// 
			this.chkExtRX801.Image = null;
			this.chkExtRX801.Location = new System.Drawing.Point(64, 80);
			this.chkExtRX801.Name = "chkExtRX801";
			this.chkExtRX801.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX801.TabIndex = 10;
			this.chkExtRX801.CheckedChanged += new System.EventHandler(this.chkExtRX80_CheckedChanged);
			// 
			// chkExtRX805
			// 
			this.chkExtRX805.Image = null;
			this.chkExtRX805.Location = new System.Drawing.Point(128, 80);
			this.chkExtRX805.Name = "chkExtRX805";
			this.chkExtRX805.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX805.TabIndex = 14;
			this.chkExtRX805.Text = "checkBox5";
			this.chkExtRX805.CheckedChanged += new System.EventHandler(this.chkExtRX80_CheckedChanged);
			// 
			// chkExtRX804
			// 
			this.chkExtRX804.Image = null;
			this.chkExtRX804.Location = new System.Drawing.Point(112, 80);
			this.chkExtRX804.Name = "chkExtRX804";
			this.chkExtRX804.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX804.TabIndex = 13;
			this.chkExtRX804.Text = "checkBox4";
			this.chkExtRX804.CheckedChanged += new System.EventHandler(this.chkExtRX80_CheckedChanged);
			// 
			// lblExtRXX2Pins
			// 
			this.lblExtRXX2Pins.Image = null;
			this.lblExtRXX2Pins.Location = new System.Drawing.Point(64, 24);
			this.lblExtRXX2Pins.Name = "lblExtRXX2Pins";
			this.lblExtRXX2Pins.Size = new System.Drawing.Size(96, 16);
			this.lblExtRXX2Pins.TabIndex = 9;
			this.lblExtRXX2Pins.Text = "X2 Pins";
			this.lblExtRXX2Pins.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExtRXBand
			// 
			this.lblExtRXBand.Image = null;
			this.lblExtRXBand.Location = new System.Drawing.Point(16, 24);
			this.lblExtRXBand.Name = "lblExtRXBand";
			this.lblExtRXBand.Size = new System.Drawing.Size(32, 16);
			this.lblExtRXBand.TabIndex = 8;
			this.lblExtRXBand.Text = "Band";
			// 
			// lblExtRX160
			// 
			this.lblExtRX160.Image = null;
			this.lblExtRX160.Location = new System.Drawing.Point(16, 64);
			this.lblExtRX160.Name = "lblExtRX160";
			this.lblExtRX160.Size = new System.Drawing.Size(40, 16);
			this.lblExtRX160.TabIndex = 7;
			this.lblExtRX160.Text = "160m";
			// 
			// chkExtRX1603
			// 
			this.chkExtRX1603.Image = null;
			this.chkExtRX1603.Location = new System.Drawing.Point(96, 64);
			this.chkExtRX1603.Name = "chkExtRX1603";
			this.chkExtRX1603.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX1603.TabIndex = 3;
			this.chkExtRX1603.Text = "checkBox3";
			this.chkExtRX1603.CheckedChanged += new System.EventHandler(this.chkExtRX160_CheckedChanged);
			// 
			// chkExtRX1602
			// 
			this.chkExtRX1602.Image = null;
			this.chkExtRX1602.Location = new System.Drawing.Point(80, 64);
			this.chkExtRX1602.Name = "chkExtRX1602";
			this.chkExtRX1602.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX1602.TabIndex = 2;
			this.chkExtRX1602.Text = "checkBox2";
			this.chkExtRX1602.CheckedChanged += new System.EventHandler(this.chkExtRX160_CheckedChanged);
			// 
			// chkExtRX1601
			// 
			this.chkExtRX1601.Image = null;
			this.chkExtRX1601.Location = new System.Drawing.Point(64, 64);
			this.chkExtRX1601.Name = "chkExtRX1601";
			this.chkExtRX1601.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX1601.TabIndex = 1;
			this.chkExtRX1601.CheckedChanged += new System.EventHandler(this.chkExtRX160_CheckedChanged);
			// 
			// lblExtRXX21
			// 
			this.lblExtRXX21.Image = null;
			this.lblExtRXX21.Location = new System.Drawing.Point(64, 40);
			this.lblExtRXX21.Name = "lblExtRXX21";
			this.lblExtRXX21.Size = new System.Drawing.Size(16, 16);
			this.lblExtRXX21.TabIndex = 6;
			this.lblExtRXX21.Text = "1";
			this.lblExtRXX21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chkExtRX1605
			// 
			this.chkExtRX1605.Image = null;
			this.chkExtRX1605.Location = new System.Drawing.Point(128, 64);
			this.chkExtRX1605.Name = "chkExtRX1605";
			this.chkExtRX1605.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX1605.TabIndex = 5;
			this.chkExtRX1605.Text = "checkBox5";
			this.chkExtRX1605.CheckedChanged += new System.EventHandler(this.chkExtRX160_CheckedChanged);
			// 
			// chkExtRX1604
			// 
			this.chkExtRX1604.Image = null;
			this.chkExtRX1604.Location = new System.Drawing.Point(112, 64);
			this.chkExtRX1604.Name = "chkExtRX1604";
			this.chkExtRX1604.Size = new System.Drawing.Size(16, 16);
			this.chkExtRX1604.TabIndex = 4;
			this.chkExtRX1604.Text = "checkBox4";
			this.chkExtRX1604.CheckedChanged += new System.EventHandler(this.chkExtRX160_CheckedChanged);
			// 
			// tpAppearance
			// 
			this.tpAppearance.Controls.Add(this.tcAppearance);
			this.tpAppearance.Location = new System.Drawing.Point(4, 22);
			this.tpAppearance.Name = "tpAppearance";
			this.tpAppearance.Size = new System.Drawing.Size(584, 286);
			this.tpAppearance.TabIndex = 6;
			this.tpAppearance.Text = "Appearance";
			// 
			// tcAppearance
			// 
			this.tcAppearance.Controls.Add(this.tpAppearanceDisplay);
			this.tcAppearance.Controls.Add(this.tpAppearanceGeneral);
			this.tcAppearance.Controls.Add(this.tpAppearanceMeter);
			this.tcAppearance.Location = new System.Drawing.Point(0, 0);
			this.tcAppearance.Name = "tcAppearance";
			this.tcAppearance.SelectedIndex = 0;
			this.tcAppearance.Size = new System.Drawing.Size(600, 344);
			this.tcAppearance.TabIndex = 40;
			// 
			// tpAppearanceDisplay
			// 
			this.tpAppearanceDisplay.Controls.Add(this.grpAppPanadapter);
			this.tpAppearanceDisplay.Controls.Add(this.grpDisplayPeakCursor);
			this.tpAppearanceDisplay.Controls.Add(this.lblDisplayDataLineColor);
			this.tpAppearanceDisplay.Controls.Add(this.lblDisplayTextColor);
			this.tpAppearanceDisplay.Controls.Add(this.lblDisplayZeroLineColor);
			this.tpAppearanceDisplay.Controls.Add(this.lblDisplayGridColor);
			this.tpAppearanceDisplay.Controls.Add(this.lblDisplayBackgroundColor);
			this.tpAppearanceDisplay.Controls.Add(this.clrbtnDataLine);
			this.tpAppearanceDisplay.Controls.Add(this.clrbtnText);
			this.tpAppearanceDisplay.Controls.Add(this.clrbtnZeroLine);
			this.tpAppearanceDisplay.Controls.Add(this.clrbtnGrid);
			this.tpAppearanceDisplay.Controls.Add(this.clrbtnBackground);
			this.tpAppearanceDisplay.Controls.Add(this.lblDisplayLineWidth);
			this.tpAppearanceDisplay.Controls.Add(this.udDisplayLineWidth);
			this.tpAppearanceDisplay.Location = new System.Drawing.Point(4, 22);
			this.tpAppearanceDisplay.Name = "tpAppearanceDisplay";
			this.tpAppearanceDisplay.Size = new System.Drawing.Size(592, 318);
			this.tpAppearanceDisplay.TabIndex = 1;
			this.tpAppearanceDisplay.Text = "Display";
			// 
			// grpAppPanadapter
			// 
			this.grpAppPanadapter.Controls.Add(this.clrbtnSubRXZero);
			this.grpAppPanadapter.Controls.Add(this.lblSubRXZeroLine);
			this.grpAppPanadapter.Controls.Add(this.clrbtnSubRXFilter);
			this.grpAppPanadapter.Controls.Add(this.lblSubRXFilterColor);
			this.grpAppPanadapter.Controls.Add(this.chkShowFreqOffset);
			this.grpAppPanadapter.Controls.Add(this.clrbtnBandEdge);
			this.grpAppPanadapter.Controls.Add(this.lblBandEdge);
			this.grpAppPanadapter.Controls.Add(this.clrbtnFilter);
			this.grpAppPanadapter.Controls.Add(this.clrbtnTXFilter);
			this.grpAppPanadapter.Controls.Add(this.lblTXFilterColor);
			this.grpAppPanadapter.Controls.Add(this.lblDisplayFilterColor);
			this.grpAppPanadapter.Location = new System.Drawing.Point(8, 112);
			this.grpAppPanadapter.Name = "grpAppPanadapter";
			this.grpAppPanadapter.Size = new System.Drawing.Size(264, 152);
			this.grpAppPanadapter.TabIndex = 77;
			this.grpAppPanadapter.TabStop = false;
			this.grpAppPanadapter.Text = "Panadapter";
			// 
			// clrbtnSubRXZero
			// 
			this.clrbtnSubRXZero.Automatic = "Automatic";
			this.clrbtnSubRXZero.Color = System.Drawing.Color.LightSkyBlue;
			this.clrbtnSubRXZero.Image = null;
			this.clrbtnSubRXZero.Location = new System.Drawing.Point(208, 56);
			this.clrbtnSubRXZero.MoreColors = "More Colors...";
			this.clrbtnSubRXZero.Name = "clrbtnSubRXZero";
			this.clrbtnSubRXZero.Size = new System.Drawing.Size(40, 23);
			this.clrbtnSubRXZero.TabIndex = 81;
			this.clrbtnSubRXZero.Changed += new System.EventHandler(this.clrbtnSubRXZero_Changed);
			// 
			// lblSubRXZeroLine
			// 
			this.lblSubRXZeroLine.Image = null;
			this.lblSubRXZeroLine.Location = new System.Drawing.Point(136, 56);
			this.lblSubRXZeroLine.Name = "lblSubRXZeroLine";
			this.lblSubRXZeroLine.Size = new System.Drawing.Size(64, 24);
			this.lblSubRXZeroLine.TabIndex = 80;
			this.lblSubRXZeroLine.Text = "Sub RX Zero Line:";
			// 
			// clrbtnSubRXFilter
			// 
			this.clrbtnSubRXFilter.Automatic = "Automatic";
			this.clrbtnSubRXFilter.Color = System.Drawing.Color.Blue;
			this.clrbtnSubRXFilter.Image = null;
			this.clrbtnSubRXFilter.Location = new System.Drawing.Point(208, 24);
			this.clrbtnSubRXFilter.MoreColors = "More Colors...";
			this.clrbtnSubRXFilter.Name = "clrbtnSubRXFilter";
			this.clrbtnSubRXFilter.Size = new System.Drawing.Size(40, 23);
			this.clrbtnSubRXFilter.TabIndex = 79;
			this.clrbtnSubRXFilter.Changed += new System.EventHandler(this.clrbtnSubRXFilter_Changed);
			// 
			// lblSubRXFilterColor
			// 
			this.lblSubRXFilterColor.Image = null;
			this.lblSubRXFilterColor.Location = new System.Drawing.Point(136, 24);
			this.lblSubRXFilterColor.Name = "lblSubRXFilterColor";
			this.lblSubRXFilterColor.Size = new System.Drawing.Size(64, 24);
			this.lblSubRXFilterColor.TabIndex = 78;
			this.lblSubRXFilterColor.Text = "Sub RX Filter Color:";
			// 
			// chkShowFreqOffset
			// 
			this.chkShowFreqOffset.Image = null;
			this.chkShowFreqOffset.Location = new System.Drawing.Point(16, 112);
			this.chkShowFreqOffset.Name = "chkShowFreqOffset";
			this.chkShowFreqOffset.Size = new System.Drawing.Size(104, 32);
			this.chkShowFreqOffset.TabIndex = 77;
			this.chkShowFreqOffset.Text = "Show Freq Offset";
			this.toolTip1.SetToolTip(this.chkShowFreqOffset, "Show the frequency offset from the VFO rather than the actual frequency in MHz on" +
				" the display.");
			this.chkShowFreqOffset.CheckedChanged += new System.EventHandler(this.chkShowFreqOffset_CheckedChanged);
			// 
			// clrbtnBandEdge
			// 
			this.clrbtnBandEdge.Automatic = "Automatic";
			this.clrbtnBandEdge.Color = System.Drawing.Color.Red;
			this.clrbtnBandEdge.Image = null;
			this.clrbtnBandEdge.Location = new System.Drawing.Point(80, 88);
			this.clrbtnBandEdge.MoreColors = "More Colors...";
			this.clrbtnBandEdge.Name = "clrbtnBandEdge";
			this.clrbtnBandEdge.Size = new System.Drawing.Size(40, 23);
			this.clrbtnBandEdge.TabIndex = 71;
			this.clrbtnBandEdge.Changed += new System.EventHandler(this.clrbtnBandEdge_Changed);
			// 
			// lblBandEdge
			// 
			this.lblBandEdge.Image = null;
			this.lblBandEdge.Location = new System.Drawing.Point(8, 88);
			this.lblBandEdge.Name = "lblBandEdge";
			this.lblBandEdge.Size = new System.Drawing.Size(64, 24);
			this.lblBandEdge.TabIndex = 65;
			this.lblBandEdge.Text = "Band Edge:";
			// 
			// clrbtnFilter
			// 
			this.clrbtnFilter.Automatic = "Automatic";
			this.clrbtnFilter.Color = System.Drawing.Color.Green;
			this.clrbtnFilter.Image = null;
			this.clrbtnFilter.Location = new System.Drawing.Point(80, 24);
			this.clrbtnFilter.MoreColors = "More Colors...";
			this.clrbtnFilter.Name = "clrbtnFilter";
			this.clrbtnFilter.Size = new System.Drawing.Size(40, 23);
			this.clrbtnFilter.TabIndex = 71;
			this.clrbtnFilter.Changed += new System.EventHandler(this.clrbtnFilter_Changed);
			// 
			// clrbtnTXFilter
			// 
			this.clrbtnTXFilter.Automatic = "Automatic";
			this.clrbtnTXFilter.Color = System.Drawing.Color.Yellow;
			this.clrbtnTXFilter.Image = null;
			this.clrbtnTXFilter.Location = new System.Drawing.Point(80, 56);
			this.clrbtnTXFilter.MoreColors = "More Colors...";
			this.clrbtnTXFilter.Name = "clrbtnTXFilter";
			this.clrbtnTXFilter.Size = new System.Drawing.Size(40, 23);
			this.clrbtnTXFilter.TabIndex = 76;
			this.clrbtnTXFilter.Changed += new System.EventHandler(this.clrbtnTXFilter_Changed);
			// 
			// lblTXFilterColor
			// 
			this.lblTXFilterColor.Image = null;
			this.lblTXFilterColor.Location = new System.Drawing.Point(8, 56);
			this.lblTXFilterColor.Name = "lblTXFilterColor";
			this.lblTXFilterColor.Size = new System.Drawing.Size(64, 24);
			this.lblTXFilterColor.TabIndex = 75;
			this.lblTXFilterColor.Text = "TX Filter Color:";
			// 
			// lblDisplayFilterColor
			// 
			this.lblDisplayFilterColor.Image = null;
			this.lblDisplayFilterColor.Location = new System.Drawing.Point(8, 24);
			this.lblDisplayFilterColor.Name = "lblDisplayFilterColor";
			this.lblDisplayFilterColor.Size = new System.Drawing.Size(64, 24);
			this.lblDisplayFilterColor.TabIndex = 45;
			this.lblDisplayFilterColor.Text = "Main RX Filter Color:";
			// 
			// grpDisplayPeakCursor
			// 
			this.grpDisplayPeakCursor.Controls.Add(this.clrbtnPeakBackground);
			this.grpDisplayPeakCursor.Controls.Add(this.lblPeakBackground);
			this.grpDisplayPeakCursor.Controls.Add(this.clrbtnPeakText);
			this.grpDisplayPeakCursor.Controls.Add(this.lblPeakText);
			this.grpDisplayPeakCursor.Location = new System.Drawing.Point(264, 8);
			this.grpDisplayPeakCursor.Name = "grpDisplayPeakCursor";
			this.grpDisplayPeakCursor.Size = new System.Drawing.Size(136, 100);
			this.grpDisplayPeakCursor.TabIndex = 74;
			this.grpDisplayPeakCursor.TabStop = false;
			this.grpDisplayPeakCursor.Text = "Cursor/Peak Readout";
			// 
			// clrbtnPeakBackground
			// 
			this.clrbtnPeakBackground.Automatic = "Automatic";
			this.clrbtnPeakBackground.Color = System.Drawing.Color.Black;
			this.clrbtnPeakBackground.Image = null;
			this.clrbtnPeakBackground.Location = new System.Drawing.Point(80, 56);
			this.clrbtnPeakBackground.MoreColors = "More Colors...";
			this.clrbtnPeakBackground.Name = "clrbtnPeakBackground";
			this.clrbtnPeakBackground.Size = new System.Drawing.Size(40, 23);
			this.clrbtnPeakBackground.TabIndex = 73;
			this.clrbtnPeakBackground.Changed += new System.EventHandler(this.clrbtnPeakBackground_Changed);
			// 
			// lblPeakBackground
			// 
			this.lblPeakBackground.Image = null;
			this.lblPeakBackground.Location = new System.Drawing.Point(8, 56);
			this.lblPeakBackground.Name = "lblPeakBackground";
			this.lblPeakBackground.Size = new System.Drawing.Size(72, 24);
			this.lblPeakBackground.TabIndex = 72;
			this.lblPeakBackground.Text = "Background:";
			// 
			// clrbtnPeakText
			// 
			this.clrbtnPeakText.Automatic = "Automatic";
			this.clrbtnPeakText.Color = System.Drawing.Color.DodgerBlue;
			this.clrbtnPeakText.Image = null;
			this.clrbtnPeakText.Location = new System.Drawing.Point(80, 24);
			this.clrbtnPeakText.MoreColors = "More Colors...";
			this.clrbtnPeakText.Name = "clrbtnPeakText";
			this.clrbtnPeakText.Size = new System.Drawing.Size(40, 23);
			this.clrbtnPeakText.TabIndex = 71;
			this.clrbtnPeakText.Changed += new System.EventHandler(this.clrbtnPeakText_Changed);
			// 
			// lblPeakText
			// 
			this.lblPeakText.Image = null;
			this.lblPeakText.Location = new System.Drawing.Point(8, 24);
			this.lblPeakText.Name = "lblPeakText";
			this.lblPeakText.Size = new System.Drawing.Size(64, 24);
			this.lblPeakText.TabIndex = 65;
			this.lblPeakText.Text = "Peak Text:";
			// 
			// lblDisplayDataLineColor
			// 
			this.lblDisplayDataLineColor.Image = null;
			this.lblDisplayDataLineColor.Location = new System.Drawing.Point(136, 48);
			this.lblDisplayDataLineColor.Name = "lblDisplayDataLineColor";
			this.lblDisplayDataLineColor.Size = new System.Drawing.Size(64, 24);
			this.lblDisplayDataLineColor.TabIndex = 41;
			this.lblDisplayDataLineColor.Text = "Data Line:";
			// 
			// lblDisplayTextColor
			// 
			this.lblDisplayTextColor.Image = null;
			this.lblDisplayTextColor.Location = new System.Drawing.Point(136, 16);
			this.lblDisplayTextColor.Name = "lblDisplayTextColor";
			this.lblDisplayTextColor.Size = new System.Drawing.Size(64, 24);
			this.lblDisplayTextColor.TabIndex = 39;
			this.lblDisplayTextColor.Text = "Text:";
			// 
			// lblDisplayZeroLineColor
			// 
			this.lblDisplayZeroLineColor.Image = null;
			this.lblDisplayZeroLineColor.Location = new System.Drawing.Point(16, 80);
			this.lblDisplayZeroLineColor.Name = "lblDisplayZeroLineColor";
			this.lblDisplayZeroLineColor.Size = new System.Drawing.Size(72, 24);
			this.lblDisplayZeroLineColor.TabIndex = 36;
			this.lblDisplayZeroLineColor.Text = "Zero Line:";
			// 
			// lblDisplayGridColor
			// 
			this.lblDisplayGridColor.Image = null;
			this.lblDisplayGridColor.Location = new System.Drawing.Point(16, 48);
			this.lblDisplayGridColor.Name = "lblDisplayGridColor";
			this.lblDisplayGridColor.Size = new System.Drawing.Size(72, 24);
			this.lblDisplayGridColor.TabIndex = 35;
			this.lblDisplayGridColor.Text = "Grid:";
			// 
			// lblDisplayBackgroundColor
			// 
			this.lblDisplayBackgroundColor.Image = null;
			this.lblDisplayBackgroundColor.Location = new System.Drawing.Point(16, 16);
			this.lblDisplayBackgroundColor.Name = "lblDisplayBackgroundColor";
			this.lblDisplayBackgroundColor.Size = new System.Drawing.Size(72, 24);
			this.lblDisplayBackgroundColor.TabIndex = 34;
			this.lblDisplayBackgroundColor.Text = "Background:";
			// 
			// clrbtnDataLine
			// 
			this.clrbtnDataLine.Automatic = "Automatic";
			this.clrbtnDataLine.Color = System.Drawing.Color.LightGreen;
			this.clrbtnDataLine.Image = null;
			this.clrbtnDataLine.Location = new System.Drawing.Point(208, 48);
			this.clrbtnDataLine.MoreColors = "More Colors...";
			this.clrbtnDataLine.Name = "clrbtnDataLine";
			this.clrbtnDataLine.Size = new System.Drawing.Size(40, 23);
			this.clrbtnDataLine.TabIndex = 73;
			this.clrbtnDataLine.Changed += new System.EventHandler(this.clrbtnDataLine_Changed);
			// 
			// clrbtnText
			// 
			this.clrbtnText.Automatic = "Automatic";
			this.clrbtnText.Color = System.Drawing.Color.Yellow;
			this.clrbtnText.Image = null;
			this.clrbtnText.Location = new System.Drawing.Point(208, 16);
			this.clrbtnText.MoreColors = "More Colors...";
			this.clrbtnText.Name = "clrbtnText";
			this.clrbtnText.Size = new System.Drawing.Size(40, 23);
			this.clrbtnText.TabIndex = 72;
			this.clrbtnText.Changed += new System.EventHandler(this.clrbtnText_Changed);
			// 
			// clrbtnZeroLine
			// 
			this.clrbtnZeroLine.Automatic = "Automatic";
			this.clrbtnZeroLine.Color = System.Drawing.Color.Red;
			this.clrbtnZeroLine.Image = null;
			this.clrbtnZeroLine.Location = new System.Drawing.Point(88, 80);
			this.clrbtnZeroLine.MoreColors = "More Colors...";
			this.clrbtnZeroLine.Name = "clrbtnZeroLine";
			this.clrbtnZeroLine.Size = new System.Drawing.Size(40, 23);
			this.clrbtnZeroLine.TabIndex = 70;
			this.clrbtnZeroLine.Changed += new System.EventHandler(this.clrbtnZeroLine_Changed);
			// 
			// clrbtnGrid
			// 
			this.clrbtnGrid.Automatic = "Automatic";
			this.clrbtnGrid.Color = System.Drawing.Color.Purple;
			this.clrbtnGrid.Image = null;
			this.clrbtnGrid.Location = new System.Drawing.Point(88, 48);
			this.clrbtnGrid.MoreColors = "More Colors...";
			this.clrbtnGrid.Name = "clrbtnGrid";
			this.clrbtnGrid.Size = new System.Drawing.Size(40, 23);
			this.clrbtnGrid.TabIndex = 69;
			this.clrbtnGrid.Changed += new System.EventHandler(this.clrbtnGrid_Changed);
			// 
			// clrbtnBackground
			// 
			this.clrbtnBackground.Automatic = "Automatic";
			this.clrbtnBackground.Color = System.Drawing.Color.Black;
			this.clrbtnBackground.Image = null;
			this.clrbtnBackground.Location = new System.Drawing.Point(88, 16);
			this.clrbtnBackground.MoreColors = "More Colors...";
			this.clrbtnBackground.Name = "clrbtnBackground";
			this.clrbtnBackground.Size = new System.Drawing.Size(40, 23);
			this.clrbtnBackground.TabIndex = 68;
			this.clrbtnBackground.Changed += new System.EventHandler(this.clrbtnBackground_Changed);
			// 
			// lblDisplayLineWidth
			// 
			this.lblDisplayLineWidth.Image = null;
			this.lblDisplayLineWidth.Location = new System.Drawing.Point(136, 80);
			this.lblDisplayLineWidth.Name = "lblDisplayLineWidth";
			this.lblDisplayLineWidth.Size = new System.Drawing.Size(64, 24);
			this.lblDisplayLineWidth.TabIndex = 43;
			this.lblDisplayLineWidth.Text = "Line Width:";
			// 
			// udDisplayLineWidth
			// 
			this.udDisplayLineWidth.DecimalPlaces = 1;
			this.udDisplayLineWidth.Increment = new System.Decimal(new int[] {
																				 1,
																				 0,
																				 0,
																				 65536});
			this.udDisplayLineWidth.Location = new System.Drawing.Point(208, 80);
			this.udDisplayLineWidth.Maximum = new System.Decimal(new int[] {
																			   50,
																			   0,
																			   0,
																			   65536});
			this.udDisplayLineWidth.Minimum = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   65536});
			this.udDisplayLineWidth.Name = "udDisplayLineWidth";
			this.udDisplayLineWidth.Size = new System.Drawing.Size(40, 20);
			this.udDisplayLineWidth.TabIndex = 42;
			this.udDisplayLineWidth.Value = new System.Decimal(new int[] {
																			 10,
																			 0,
																			 0,
																			 65536});
			this.udDisplayLineWidth.LostFocus += new System.EventHandler(this.udDisplayLineWidth_LostFocus);
			this.udDisplayLineWidth.ValueChanged += new System.EventHandler(this.udDisplayLineWidth_ValueChanged);
			// 
			// tpAppearanceGeneral
			// 
			this.tpAppearanceGeneral.Controls.Add(this.grpAppearanceBand);
			this.tpAppearanceGeneral.Controls.Add(this.grpAppearanceVFO);
			this.tpAppearanceGeneral.Controls.Add(this.clrbtnBtnSel);
			this.tpAppearanceGeneral.Controls.Add(this.lblAppearanceGenBtnSel);
			this.tpAppearanceGeneral.Location = new System.Drawing.Point(4, 22);
			this.tpAppearanceGeneral.Name = "tpAppearanceGeneral";
			this.tpAppearanceGeneral.Size = new System.Drawing.Size(592, 318);
			this.tpAppearanceGeneral.TabIndex = 0;
			this.tpAppearanceGeneral.Text = "General";
			// 
			// grpAppearanceBand
			// 
			this.grpAppearanceBand.Controls.Add(this.clrbtnBandBackground);
			this.grpAppearanceBand.Controls.Add(this.lblBandBackground);
			this.grpAppearanceBand.Controls.Add(this.clrbtnBandLight);
			this.grpAppearanceBand.Controls.Add(this.clrbtnBandDark);
			this.grpAppearanceBand.Controls.Add(this.lblBandLight);
			this.grpAppearanceBand.Controls.Add(this.lblBandDark);
			this.grpAppearanceBand.Controls.Add(this.clrbtnOutOfBand);
			this.grpAppearanceBand.Controls.Add(this.lblOutOfBand);
			this.grpAppearanceBand.Location = new System.Drawing.Point(296, 8);
			this.grpAppearanceBand.Name = "grpAppearanceBand";
			this.grpAppearanceBand.Size = new System.Drawing.Size(144, 152);
			this.grpAppearanceBand.TabIndex = 74;
			this.grpAppearanceBand.TabStop = false;
			this.grpAppearanceBand.Text = "Band Data";
			// 
			// clrbtnBandBackground
			// 
			this.clrbtnBandBackground.Automatic = "Automatic";
			this.clrbtnBandBackground.Color = System.Drawing.Color.Black;
			this.clrbtnBandBackground.Image = null;
			this.clrbtnBandBackground.Location = new System.Drawing.Point(88, 120);
			this.clrbtnBandBackground.MoreColors = "More Colors...";
			this.clrbtnBandBackground.Name = "clrbtnBandBackground";
			this.clrbtnBandBackground.Size = new System.Drawing.Size(40, 23);
			this.clrbtnBandBackground.TabIndex = 75;
			this.clrbtnBandBackground.Changed += new System.EventHandler(this.clrbtnBandBackground_Changed);
			// 
			// lblBandBackground
			// 
			this.lblBandBackground.Image = null;
			this.lblBandBackground.Location = new System.Drawing.Point(16, 120);
			this.lblBandBackground.Name = "lblBandBackground";
			this.lblBandBackground.Size = new System.Drawing.Size(72, 24);
			this.lblBandBackground.TabIndex = 74;
			this.lblBandBackground.Text = "Background:";
			// 
			// clrbtnBandLight
			// 
			this.clrbtnBandLight.Automatic = "Automatic";
			this.clrbtnBandLight.Color = System.Drawing.Color.Lime;
			this.clrbtnBandLight.Image = null;
			this.clrbtnBandLight.Location = new System.Drawing.Point(88, 56);
			this.clrbtnBandLight.MoreColors = "More Colors...";
			this.clrbtnBandLight.Name = "clrbtnBandLight";
			this.clrbtnBandLight.Size = new System.Drawing.Size(40, 23);
			this.clrbtnBandLight.TabIndex = 70;
			this.clrbtnBandLight.Changed += new System.EventHandler(this.clrbtnBandLight_Changed);
			// 
			// clrbtnBandDark
			// 
			this.clrbtnBandDark.Automatic = "Automatic";
			this.clrbtnBandDark.Color = System.Drawing.Color.Green;
			this.clrbtnBandDark.Image = null;
			this.clrbtnBandDark.Location = new System.Drawing.Point(88, 24);
			this.clrbtnBandDark.MoreColors = "More Colors...";
			this.clrbtnBandDark.Name = "clrbtnBandDark";
			this.clrbtnBandDark.Size = new System.Drawing.Size(40, 23);
			this.clrbtnBandDark.TabIndex = 69;
			this.clrbtnBandDark.Changed += new System.EventHandler(this.clrbtnBandDark_Changed);
			// 
			// lblBandLight
			// 
			this.lblBandLight.Image = null;
			this.lblBandLight.Location = new System.Drawing.Point(16, 56);
			this.lblBandLight.Name = "lblBandLight";
			this.lblBandLight.Size = new System.Drawing.Size(64, 24);
			this.lblBandLight.TabIndex = 63;
			this.lblBandLight.Text = "Active:";
			// 
			// lblBandDark
			// 
			this.lblBandDark.Image = null;
			this.lblBandDark.Location = new System.Drawing.Point(16, 24);
			this.lblBandDark.Name = "lblBandDark";
			this.lblBandDark.Size = new System.Drawing.Size(64, 24);
			this.lblBandDark.TabIndex = 61;
			this.lblBandDark.Text = "Inactive:";
			// 
			// clrbtnOutOfBand
			// 
			this.clrbtnOutOfBand.Automatic = "Automatic";
			this.clrbtnOutOfBand.Color = System.Drawing.Color.DimGray;
			this.clrbtnOutOfBand.Image = null;
			this.clrbtnOutOfBand.Location = new System.Drawing.Point(88, 88);
			this.clrbtnOutOfBand.MoreColors = "More Colors...";
			this.clrbtnOutOfBand.Name = "clrbtnOutOfBand";
			this.clrbtnOutOfBand.Size = new System.Drawing.Size(40, 23);
			this.clrbtnOutOfBand.TabIndex = 73;
			this.clrbtnOutOfBand.Changed += new System.EventHandler(this.clrbtnOutOfBand_Changed);
			// 
			// lblOutOfBand
			// 
			this.lblOutOfBand.Image = null;
			this.lblOutOfBand.Location = new System.Drawing.Point(16, 88);
			this.lblOutOfBand.Name = "lblOutOfBand";
			this.lblOutOfBand.Size = new System.Drawing.Size(72, 24);
			this.lblOutOfBand.TabIndex = 72;
			this.lblOutOfBand.Text = "Out Of Band:";
			// 
			// grpAppearanceVFO
			// 
			this.grpAppearanceVFO.Controls.Add(this.clrbtnVFOBackground);
			this.grpAppearanceVFO.Controls.Add(this.lblVFOBackground);
			this.grpAppearanceVFO.Controls.Add(this.clrbtnVFOSmallColor);
			this.grpAppearanceVFO.Controls.Add(this.lblVFOSmallColor);
			this.grpAppearanceVFO.Controls.Add(this.chkVFOSmallLSD);
			this.grpAppearanceVFO.Controls.Add(this.clrbtnVFOLight);
			this.grpAppearanceVFO.Controls.Add(this.clrbtnVFODark);
			this.grpAppearanceVFO.Controls.Add(this.lblVFOPowerOn);
			this.grpAppearanceVFO.Controls.Add(this.lblVFOPowerOff);
			this.grpAppearanceVFO.Location = new System.Drawing.Point(144, 8);
			this.grpAppearanceVFO.Name = "grpAppearanceVFO";
			this.grpAppearanceVFO.Size = new System.Drawing.Size(144, 184);
			this.grpAppearanceVFO.TabIndex = 39;
			this.grpAppearanceVFO.TabStop = false;
			this.grpAppearanceVFO.Text = "VFO";
			// 
			// clrbtnVFOBackground
			// 
			this.clrbtnVFOBackground.Automatic = "Automatic";
			this.clrbtnVFOBackground.Color = System.Drawing.Color.Black;
			this.clrbtnVFOBackground.Image = null;
			this.clrbtnVFOBackground.Location = new System.Drawing.Point(88, 88);
			this.clrbtnVFOBackground.MoreColors = "More Colors...";
			this.clrbtnVFOBackground.Name = "clrbtnVFOBackground";
			this.clrbtnVFOBackground.Size = new System.Drawing.Size(40, 23);
			this.clrbtnVFOBackground.TabIndex = 73;
			this.clrbtnVFOBackground.Changed += new System.EventHandler(this.clrbtnVFOBackground_Changed);
			// 
			// lblVFOBackground
			// 
			this.lblVFOBackground.Image = null;
			this.lblVFOBackground.Location = new System.Drawing.Point(16, 88);
			this.lblVFOBackground.Name = "lblVFOBackground";
			this.lblVFOBackground.Size = new System.Drawing.Size(72, 24);
			this.lblVFOBackground.TabIndex = 72;
			this.lblVFOBackground.Text = "Background:";
			// 
			// clrbtnVFOSmallColor
			// 
			this.clrbtnVFOSmallColor.Automatic = "Automatic";
			this.clrbtnVFOSmallColor.Color = System.Drawing.Color.OrangeRed;
			this.clrbtnVFOSmallColor.Image = null;
			this.clrbtnVFOSmallColor.Location = new System.Drawing.Point(88, 152);
			this.clrbtnVFOSmallColor.MoreColors = "More Colors...";
			this.clrbtnVFOSmallColor.Name = "clrbtnVFOSmallColor";
			this.clrbtnVFOSmallColor.Size = new System.Drawing.Size(40, 23);
			this.clrbtnVFOSmallColor.TabIndex = 71;
			this.clrbtnVFOSmallColor.Changed += new System.EventHandler(this.clrbtnVFOSmallColor_Changed);
			// 
			// lblVFOSmallColor
			// 
			this.lblVFOSmallColor.Image = null;
			this.lblVFOSmallColor.Location = new System.Drawing.Point(16, 152);
			this.lblVFOSmallColor.Name = "lblVFOSmallColor";
			this.lblVFOSmallColor.Size = new System.Drawing.Size(72, 24);
			this.lblVFOSmallColor.TabIndex = 70;
			this.lblVFOSmallColor.Text = "Small Color:";
			// 
			// chkVFOSmallLSD
			// 
			this.chkVFOSmallLSD.Checked = true;
			this.chkVFOSmallLSD.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVFOSmallLSD.Image = null;
			this.chkVFOSmallLSD.Location = new System.Drawing.Point(16, 120);
			this.chkVFOSmallLSD.Name = "chkVFOSmallLSD";
			this.chkVFOSmallLSD.TabIndex = 69;
			this.chkVFOSmallLSD.Text = "Small 3 Digits";
			this.chkVFOSmallLSD.CheckedChanged += new System.EventHandler(this.chkVFOSmallLSD_CheckedChanged);
			// 
			// clrbtnVFOLight
			// 
			this.clrbtnVFOLight.Automatic = "Automatic";
			this.clrbtnVFOLight.Color = System.Drawing.Color.Yellow;
			this.clrbtnVFOLight.Image = null;
			this.clrbtnVFOLight.Location = new System.Drawing.Point(88, 56);
			this.clrbtnVFOLight.MoreColors = "More Colors...";
			this.clrbtnVFOLight.Name = "clrbtnVFOLight";
			this.clrbtnVFOLight.Size = new System.Drawing.Size(40, 23);
			this.clrbtnVFOLight.TabIndex = 68;
			this.clrbtnVFOLight.Changed += new System.EventHandler(this.clrbtnVFOLight_Changed);
			// 
			// clrbtnVFODark
			// 
			this.clrbtnVFODark.Automatic = "Automatic";
			this.clrbtnVFODark.Color = System.Drawing.Color.Olive;
			this.clrbtnVFODark.Image = null;
			this.clrbtnVFODark.Location = new System.Drawing.Point(88, 24);
			this.clrbtnVFODark.MoreColors = "More Colors...";
			this.clrbtnVFODark.Name = "clrbtnVFODark";
			this.clrbtnVFODark.Size = new System.Drawing.Size(40, 23);
			this.clrbtnVFODark.TabIndex = 67;
			this.clrbtnVFODark.Changed += new System.EventHandler(this.clrbtnVFODark_Changed);
			// 
			// lblVFOPowerOn
			// 
			this.lblVFOPowerOn.Image = null;
			this.lblVFOPowerOn.Location = new System.Drawing.Point(16, 56);
			this.lblVFOPowerOn.Name = "lblVFOPowerOn";
			this.lblVFOPowerOn.Size = new System.Drawing.Size(64, 24);
			this.lblVFOPowerOn.TabIndex = 59;
			this.lblVFOPowerOn.Text = "Active:";
			// 
			// lblVFOPowerOff
			// 
			this.lblVFOPowerOff.Image = null;
			this.lblVFOPowerOff.Location = new System.Drawing.Point(16, 24);
			this.lblVFOPowerOff.Name = "lblVFOPowerOff";
			this.lblVFOPowerOff.Size = new System.Drawing.Size(64, 24);
			this.lblVFOPowerOff.TabIndex = 57;
			this.lblVFOPowerOff.Text = "Inactive:";
			// 
			// clrbtnBtnSel
			// 
			this.clrbtnBtnSel.Automatic = "Automatic";
			this.clrbtnBtnSel.Color = System.Drawing.Color.Yellow;
			this.clrbtnBtnSel.Image = null;
			this.clrbtnBtnSel.Location = new System.Drawing.Point(88, 16);
			this.clrbtnBtnSel.MoreColors = "More Colors...";
			this.clrbtnBtnSel.Name = "clrbtnBtnSel";
			this.clrbtnBtnSel.Size = new System.Drawing.Size(40, 23);
			this.clrbtnBtnSel.TabIndex = 66;
			this.clrbtnBtnSel.Changed += new System.EventHandler(this.clrbtnBtnSel_Changed);
			// 
			// lblAppearanceGenBtnSel
			// 
			this.lblAppearanceGenBtnSel.Image = null;
			this.lblAppearanceGenBtnSel.Location = new System.Drawing.Point(16, 16);
			this.lblAppearanceGenBtnSel.Name = "lblAppearanceGenBtnSel";
			this.lblAppearanceGenBtnSel.Size = new System.Drawing.Size(64, 32);
			this.lblAppearanceGenBtnSel.TabIndex = 55;
			this.lblAppearanceGenBtnSel.Text = "Button Selected:";
			// 
			// tpAppearanceMeter
			// 
			this.tpAppearanceMeter.Controls.Add(this.labelTS2);
			this.tpAppearanceMeter.Controls.Add(this.clrbtnMeterDigBackground);
			this.tpAppearanceMeter.Controls.Add(this.lblMeterDigitalText);
			this.tpAppearanceMeter.Controls.Add(this.clrbtnMeterDigText);
			this.tpAppearanceMeter.Controls.Add(this.grpMeterEdge);
			this.tpAppearanceMeter.Controls.Add(this.grpAppearanceMeter);
			this.tpAppearanceMeter.Controls.Add(this.lblMeterType);
			this.tpAppearanceMeter.Controls.Add(this.comboMeterType);
			this.tpAppearanceMeter.Location = new System.Drawing.Point(4, 22);
			this.tpAppearanceMeter.Name = "tpAppearanceMeter";
			this.tpAppearanceMeter.Size = new System.Drawing.Size(592, 318);
			this.tpAppearanceMeter.TabIndex = 2;
			this.tpAppearanceMeter.Text = "Meter";
			// 
			// labelTS2
			// 
			this.labelTS2.Image = null;
			this.labelTS2.Location = new System.Drawing.Point(24, 80);
			this.labelTS2.Name = "labelTS2";
			this.labelTS2.Size = new System.Drawing.Size(72, 32);
			this.labelTS2.TabIndex = 83;
			this.labelTS2.Text = "Digital Background:";
			// 
			// clrbtnMeterDigBackground
			// 
			this.clrbtnMeterDigBackground.Automatic = "Automatic";
			this.clrbtnMeterDigBackground.Color = System.Drawing.Color.Black;
			this.clrbtnMeterDigBackground.Image = null;
			this.clrbtnMeterDigBackground.Location = new System.Drawing.Point(96, 80);
			this.clrbtnMeterDigBackground.MoreColors = "More Colors...";
			this.clrbtnMeterDigBackground.Name = "clrbtnMeterDigBackground";
			this.clrbtnMeterDigBackground.Size = new System.Drawing.Size(40, 23);
			this.clrbtnMeterDigBackground.TabIndex = 84;
			this.clrbtnMeterDigBackground.Changed += new System.EventHandler(this.clrbtnMeterDigBackground_Changed);
			// 
			// lblMeterDigitalText
			// 
			this.lblMeterDigitalText.Image = null;
			this.lblMeterDigitalText.Location = new System.Drawing.Point(24, 48);
			this.lblMeterDigitalText.Name = "lblMeterDigitalText";
			this.lblMeterDigitalText.Size = new System.Drawing.Size(72, 24);
			this.lblMeterDigitalText.TabIndex = 81;
			this.lblMeterDigitalText.Text = "Digital Text:";
			// 
			// clrbtnMeterDigText
			// 
			this.clrbtnMeterDigText.Automatic = "Automatic";
			this.clrbtnMeterDigText.Color = System.Drawing.Color.Yellow;
			this.clrbtnMeterDigText.Image = null;
			this.clrbtnMeterDigText.Location = new System.Drawing.Point(96, 48);
			this.clrbtnMeterDigText.MoreColors = "More Colors...";
			this.clrbtnMeterDigText.Name = "clrbtnMeterDigText";
			this.clrbtnMeterDigText.Size = new System.Drawing.Size(40, 23);
			this.clrbtnMeterDigText.TabIndex = 82;
			this.clrbtnMeterDigText.Changed += new System.EventHandler(this.clrbtnMeterDigText_Changed);
			// 
			// grpMeterEdge
			// 
			this.grpMeterEdge.Controls.Add(this.clrbtnEdgeIndicator);
			this.grpMeterEdge.Controls.Add(this.labelTS1);
			this.grpMeterEdge.Controls.Add(this.clrbtnMeterEdgeBackground);
			this.grpMeterEdge.Controls.Add(this.lblMeterEdgeBackground);
			this.grpMeterEdge.Controls.Add(this.clrbtnMeterEdgeHigh);
			this.grpMeterEdge.Controls.Add(this.lblMeterEdgeHigh);
			this.grpMeterEdge.Controls.Add(this.lblMeterEdgeLow);
			this.grpMeterEdge.Controls.Add(this.clrbtnMeterEdgeLow);
			this.grpMeterEdge.Location = new System.Drawing.Point(312, 8);
			this.grpMeterEdge.Name = "grpMeterEdge";
			this.grpMeterEdge.Size = new System.Drawing.Size(136, 160);
			this.grpMeterEdge.TabIndex = 80;
			this.grpMeterEdge.TabStop = false;
			this.grpMeterEdge.Text = "Edge Style";
			// 
			// clrbtnEdgeIndicator
			// 
			this.clrbtnEdgeIndicator.Automatic = "Automatic";
			this.clrbtnEdgeIndicator.Color = System.Drawing.Color.Yellow;
			this.clrbtnEdgeIndicator.ForeColor = System.Drawing.Color.Black;
			this.clrbtnEdgeIndicator.Image = null;
			this.clrbtnEdgeIndicator.Location = new System.Drawing.Point(80, 120);
			this.clrbtnEdgeIndicator.MoreColors = "More Colors...";
			this.clrbtnEdgeIndicator.Name = "clrbtnEdgeIndicator";
			this.clrbtnEdgeIndicator.Size = new System.Drawing.Size(40, 23);
			this.clrbtnEdgeIndicator.TabIndex = 79;
			this.clrbtnEdgeIndicator.Changed += new System.EventHandler(this.clrbtnEdgeIndicator_Changed);
			// 
			// labelTS1
			// 
			this.labelTS1.Image = null;
			this.labelTS1.Location = new System.Drawing.Point(8, 120);
			this.labelTS1.Name = "labelTS1";
			this.labelTS1.Size = new System.Drawing.Size(56, 24);
			this.labelTS1.TabIndex = 78;
			this.labelTS1.Text = "Indicator:";
			// 
			// clrbtnMeterEdgeBackground
			// 
			this.clrbtnMeterEdgeBackground.Automatic = "Automatic";
			this.clrbtnMeterEdgeBackground.Color = System.Drawing.Color.Black;
			this.clrbtnMeterEdgeBackground.ForeColor = System.Drawing.Color.Black;
			this.clrbtnMeterEdgeBackground.Image = null;
			this.clrbtnMeterEdgeBackground.Location = new System.Drawing.Point(80, 88);
			this.clrbtnMeterEdgeBackground.MoreColors = "More Colors...";
			this.clrbtnMeterEdgeBackground.Name = "clrbtnMeterEdgeBackground";
			this.clrbtnMeterEdgeBackground.Size = new System.Drawing.Size(40, 23);
			this.clrbtnMeterEdgeBackground.TabIndex = 77;
			this.clrbtnMeterEdgeBackground.Changed += new System.EventHandler(this.clrbtnMeterEdgeBackground_Changed);
			// 
			// lblMeterEdgeBackground
			// 
			this.lblMeterEdgeBackground.Image = null;
			this.lblMeterEdgeBackground.Location = new System.Drawing.Point(8, 88);
			this.lblMeterEdgeBackground.Name = "lblMeterEdgeBackground";
			this.lblMeterEdgeBackground.Size = new System.Drawing.Size(72, 24);
			this.lblMeterEdgeBackground.TabIndex = 76;
			this.lblMeterEdgeBackground.Text = "Background:";
			// 
			// clrbtnMeterEdgeHigh
			// 
			this.clrbtnMeterEdgeHigh.Automatic = "Automatic";
			this.clrbtnMeterEdgeHigh.Color = System.Drawing.Color.Red;
			this.clrbtnMeterEdgeHigh.Image = null;
			this.clrbtnMeterEdgeHigh.Location = new System.Drawing.Point(80, 56);
			this.clrbtnMeterEdgeHigh.MoreColors = "More Colors...";
			this.clrbtnMeterEdgeHigh.Name = "clrbtnMeterEdgeHigh";
			this.clrbtnMeterEdgeHigh.Size = new System.Drawing.Size(40, 23);
			this.clrbtnMeterEdgeHigh.TabIndex = 75;
			this.clrbtnMeterEdgeHigh.Changed += new System.EventHandler(this.clrbtnMeterEdgeHigh_Changed);
			// 
			// lblMeterEdgeHigh
			// 
			this.lblMeterEdgeHigh.Image = null;
			this.lblMeterEdgeHigh.Location = new System.Drawing.Point(8, 56);
			this.lblMeterEdgeHigh.Name = "lblMeterEdgeHigh";
			this.lblMeterEdgeHigh.Size = new System.Drawing.Size(72, 24);
			this.lblMeterEdgeHigh.TabIndex = 53;
			this.lblMeterEdgeHigh.Text = "High Color:";
			// 
			// lblMeterEdgeLow
			// 
			this.lblMeterEdgeLow.Image = null;
			this.lblMeterEdgeLow.Location = new System.Drawing.Point(8, 24);
			this.lblMeterEdgeLow.Name = "lblMeterEdgeLow";
			this.lblMeterEdgeLow.Size = new System.Drawing.Size(72, 24);
			this.lblMeterEdgeLow.TabIndex = 51;
			this.lblMeterEdgeLow.Text = "Low Color:";
			// 
			// clrbtnMeterEdgeLow
			// 
			this.clrbtnMeterEdgeLow.Automatic = "Automatic";
			this.clrbtnMeterEdgeLow.Color = System.Drawing.Color.White;
			this.clrbtnMeterEdgeLow.Image = null;
			this.clrbtnMeterEdgeLow.Location = new System.Drawing.Point(80, 24);
			this.clrbtnMeterEdgeLow.MoreColors = "More Colors...";
			this.clrbtnMeterEdgeLow.Name = "clrbtnMeterEdgeLow";
			this.clrbtnMeterEdgeLow.Size = new System.Drawing.Size(40, 23);
			this.clrbtnMeterEdgeLow.TabIndex = 74;
			this.clrbtnMeterEdgeLow.Changed += new System.EventHandler(this.clrbtnMeterEdgeLow_Changed);
			// 
			// grpAppearanceMeter
			// 
			this.grpAppearanceMeter.Controls.Add(this.clrbtnMeterBackground);
			this.grpAppearanceMeter.Controls.Add(this.lblMeterBackground);
			this.grpAppearanceMeter.Controls.Add(this.clrbtnMeterRight);
			this.grpAppearanceMeter.Controls.Add(this.lblAppearanceMeterRight);
			this.grpAppearanceMeter.Controls.Add(this.lblAppearanceMeterLeft);
			this.grpAppearanceMeter.Controls.Add(this.clrbtnMeterLeft);
			this.grpAppearanceMeter.Location = new System.Drawing.Point(168, 8);
			this.grpAppearanceMeter.Name = "grpAppearanceMeter";
			this.grpAppearanceMeter.Size = new System.Drawing.Size(136, 120);
			this.grpAppearanceMeter.TabIndex = 38;
			this.grpAppearanceMeter.TabStop = false;
			this.grpAppearanceMeter.Text = "Original Style";
			// 
			// clrbtnMeterBackground
			// 
			this.clrbtnMeterBackground.Automatic = "Automatic";
			this.clrbtnMeterBackground.Color = System.Drawing.Color.Black;
			this.clrbtnMeterBackground.Image = null;
			this.clrbtnMeterBackground.Location = new System.Drawing.Point(80, 88);
			this.clrbtnMeterBackground.MoreColors = "More Colors...";
			this.clrbtnMeterBackground.Name = "clrbtnMeterBackground";
			this.clrbtnMeterBackground.Size = new System.Drawing.Size(40, 23);
			this.clrbtnMeterBackground.TabIndex = 77;
			this.clrbtnMeterBackground.Changed += new System.EventHandler(this.clrbtnMeterBackground_Changed);
			// 
			// lblMeterBackground
			// 
			this.lblMeterBackground.Image = null;
			this.lblMeterBackground.Location = new System.Drawing.Point(8, 88);
			this.lblMeterBackground.Name = "lblMeterBackground";
			this.lblMeterBackground.Size = new System.Drawing.Size(72, 24);
			this.lblMeterBackground.TabIndex = 76;
			this.lblMeterBackground.Text = "Background:";
			// 
			// clrbtnMeterRight
			// 
			this.clrbtnMeterRight.Automatic = "Automatic";
			this.clrbtnMeterRight.Color = System.Drawing.Color.Lime;
			this.clrbtnMeterRight.Image = null;
			this.clrbtnMeterRight.Location = new System.Drawing.Point(80, 56);
			this.clrbtnMeterRight.MoreColors = "More Colors...";
			this.clrbtnMeterRight.Name = "clrbtnMeterRight";
			this.clrbtnMeterRight.Size = new System.Drawing.Size(40, 23);
			this.clrbtnMeterRight.TabIndex = 75;
			this.clrbtnMeterRight.Changed += new System.EventHandler(this.clrbtnMeterRight_Changed);
			// 
			// lblAppearanceMeterRight
			// 
			this.lblAppearanceMeterRight.Image = null;
			this.lblAppearanceMeterRight.Location = new System.Drawing.Point(8, 56);
			this.lblAppearanceMeterRight.Name = "lblAppearanceMeterRight";
			this.lblAppearanceMeterRight.Size = new System.Drawing.Size(72, 24);
			this.lblAppearanceMeterRight.TabIndex = 53;
			this.lblAppearanceMeterRight.Text = "Right Color:";
			// 
			// lblAppearanceMeterLeft
			// 
			this.lblAppearanceMeterLeft.Image = null;
			this.lblAppearanceMeterLeft.Location = new System.Drawing.Point(8, 24);
			this.lblAppearanceMeterLeft.Name = "lblAppearanceMeterLeft";
			this.lblAppearanceMeterLeft.Size = new System.Drawing.Size(72, 24);
			this.lblAppearanceMeterLeft.TabIndex = 51;
			this.lblAppearanceMeterLeft.Text = "Left Color:";
			// 
			// clrbtnMeterLeft
			// 
			this.clrbtnMeterLeft.Automatic = "Automatic";
			this.clrbtnMeterLeft.Color = System.Drawing.Color.Green;
			this.clrbtnMeterLeft.Image = null;
			this.clrbtnMeterLeft.Location = new System.Drawing.Point(80, 24);
			this.clrbtnMeterLeft.MoreColors = "More Colors...";
			this.clrbtnMeterLeft.Name = "clrbtnMeterLeft";
			this.clrbtnMeterLeft.Size = new System.Drawing.Size(40, 23);
			this.clrbtnMeterLeft.TabIndex = 74;
			this.clrbtnMeterLeft.Changed += new System.EventHandler(this.clrbtnMeterLeft_Changed);
			// 
			// lblMeterType
			// 
			this.lblMeterType.Image = null;
			this.lblMeterType.Location = new System.Drawing.Point(16, 16);
			this.lblMeterType.Name = "lblMeterType";
			this.lblMeterType.Size = new System.Drawing.Size(64, 24);
			this.lblMeterType.TabIndex = 79;
			this.lblMeterType.Text = "Meter Type:";
			// 
			// comboMeterType
			// 
			this.comboMeterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboMeterType.DropDownWidth = 80;
			this.comboMeterType.Items.AddRange(new object[] {
																"Original",
																"Edge"});
			this.comboMeterType.Location = new System.Drawing.Point(80, 16);
			this.comboMeterType.Name = "comboMeterType";
			this.comboMeterType.Size = new System.Drawing.Size(80, 21);
			this.comboMeterType.TabIndex = 78;
			this.toolTip1.SetToolTip(this.comboMeterType, "Changes the appearance of the Multimeter on the front panel.");
			this.comboMeterType.SelectedIndexChanged += new System.EventHandler(this.comboMeterType_SelectedIndexChanged);
			// 
			// tpKeyboard
			// 
			this.tpKeyboard.Controls.Add(this.grpKBXIT);
			this.tpKeyboard.Controls.Add(this.grpKBRIT);
			this.tpKeyboard.Controls.Add(this.grpKBMode);
			this.tpKeyboard.Controls.Add(this.grpKBBand);
			this.tpKeyboard.Controls.Add(this.grpKBTune);
			this.tpKeyboard.Controls.Add(this.grpKBFilter);
			this.tpKeyboard.Controls.Add(this.grpKBCW);
			this.tpKeyboard.Location = new System.Drawing.Point(4, 22);
			this.tpKeyboard.Name = "tpKeyboard";
			this.tpKeyboard.Size = new System.Drawing.Size(584, 286);
			this.tpKeyboard.TabIndex = 4;
			this.tpKeyboard.Text = "Keyboard";
			// 
			// grpKBXIT
			// 
			this.grpKBXIT.Controls.Add(this.lblKBXITUp);
			this.grpKBXIT.Controls.Add(this.lblKBXITDown);
			this.grpKBXIT.Controls.Add(this.comboKBXITUp);
			this.grpKBXIT.Controls.Add(this.comboKBXITDown);
			this.grpKBXIT.Location = new System.Drawing.Point(136, 192);
			this.grpKBXIT.Name = "grpKBXIT";
			this.grpKBXIT.Size = new System.Drawing.Size(112, 72);
			this.grpKBXIT.TabIndex = 16;
			this.grpKBXIT.TabStop = false;
			this.grpKBXIT.Text = "XIT";
			// 
			// lblKBXITUp
			// 
			this.lblKBXITUp.Image = null;
			this.lblKBXITUp.Location = new System.Drawing.Point(8, 16);
			this.lblKBXITUp.Name = "lblKBXITUp";
			this.lblKBXITUp.Size = new System.Drawing.Size(40, 16);
			this.lblKBXITUp.TabIndex = 10;
			this.lblKBXITUp.Text = "Up:";
			this.lblKBXITUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblKBXITDown
			// 
			this.lblKBXITDown.Image = null;
			this.lblKBXITDown.Location = new System.Drawing.Point(8, 40);
			this.lblKBXITDown.Name = "lblKBXITDown";
			this.lblKBXITDown.Size = new System.Drawing.Size(40, 16);
			this.lblKBXITDown.TabIndex = 9;
			this.lblKBXITDown.Text = "Down:";
			this.lblKBXITDown.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// comboKBXITUp
			// 
			this.comboKBXITUp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBXITUp.DropDownWidth = 56;
			this.comboKBXITUp.Location = new System.Drawing.Point(48, 16);
			this.comboKBXITUp.Name = "comboKBXITUp";
			this.comboKBXITUp.Size = new System.Drawing.Size(56, 21);
			this.comboKBXITUp.TabIndex = 6;
			this.toolTip1.SetToolTip(this.comboKBXITUp, "Adjust XIT control up 1kHz.");
			this.comboKBXITUp.SelectedIndexChanged += new System.EventHandler(this.comboKBXITUp_SelectedIndexChanged);
			// 
			// comboKBXITDown
			// 
			this.comboKBXITDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBXITDown.DropDownWidth = 56;
			this.comboKBXITDown.Location = new System.Drawing.Point(48, 40);
			this.comboKBXITDown.Name = "comboKBXITDown";
			this.comboKBXITDown.Size = new System.Drawing.Size(56, 21);
			this.comboKBXITDown.TabIndex = 5;
			this.toolTip1.SetToolTip(this.comboKBXITDown, "Adjust the XIT control down 1kHz.");
			this.comboKBXITDown.SelectedIndexChanged += new System.EventHandler(this.comboKBXITDown_SelectedIndexChanged);
			// 
			// grpKBRIT
			// 
			this.grpKBRIT.Controls.Add(this.lblKBRitUp);
			this.grpKBRIT.Controls.Add(this.lblKBRITDown);
			this.grpKBRIT.Controls.Add(this.comboKBRITUp);
			this.grpKBRIT.Controls.Add(this.comboKBRITDown);
			this.grpKBRIT.Location = new System.Drawing.Point(8, 192);
			this.grpKBRIT.Name = "grpKBRIT";
			this.grpKBRIT.Size = new System.Drawing.Size(112, 72);
			this.grpKBRIT.TabIndex = 15;
			this.grpKBRIT.TabStop = false;
			this.grpKBRIT.Text = "RIT";
			// 
			// lblKBRitUp
			// 
			this.lblKBRitUp.Image = null;
			this.lblKBRitUp.Location = new System.Drawing.Point(8, 16);
			this.lblKBRitUp.Name = "lblKBRitUp";
			this.lblKBRitUp.Size = new System.Drawing.Size(40, 16);
			this.lblKBRitUp.TabIndex = 10;
			this.lblKBRitUp.Text = "Up:";
			this.lblKBRitUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblKBRITDown
			// 
			this.lblKBRITDown.Image = null;
			this.lblKBRITDown.Location = new System.Drawing.Point(8, 40);
			this.lblKBRITDown.Name = "lblKBRITDown";
			this.lblKBRITDown.Size = new System.Drawing.Size(40, 16);
			this.lblKBRITDown.TabIndex = 9;
			this.lblKBRITDown.Text = "Down:";
			this.lblKBRITDown.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// comboKBRITUp
			// 
			this.comboKBRITUp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBRITUp.DropDownWidth = 56;
			this.comboKBRITUp.Location = new System.Drawing.Point(48, 16);
			this.comboKBRITUp.Name = "comboKBRITUp";
			this.comboKBRITUp.Size = new System.Drawing.Size(56, 21);
			this.comboKBRITUp.TabIndex = 6;
			this.toolTip1.SetToolTip(this.comboKBRITUp, "Adjust RIT control up 1kHz.");
			this.comboKBRITUp.SelectedIndexChanged += new System.EventHandler(this.comboKBRITUp_SelectedIndexChanged);
			// 
			// comboKBRITDown
			// 
			this.comboKBRITDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBRITDown.DropDownWidth = 56;
			this.comboKBRITDown.Location = new System.Drawing.Point(48, 40);
			this.comboKBRITDown.Name = "comboKBRITDown";
			this.comboKBRITDown.Size = new System.Drawing.Size(56, 21);
			this.comboKBRITDown.TabIndex = 5;
			this.toolTip1.SetToolTip(this.comboKBRITDown, "Adjust RIT control down 1kHz.");
			this.comboKBRITDown.SelectedIndexChanged += new System.EventHandler(this.comboKBRITDown_SelectedIndexChanged);
			// 
			// grpKBMode
			// 
			this.grpKBMode.Controls.Add(this.lblKBModeUp);
			this.grpKBMode.Controls.Add(this.lblKBModeDown);
			this.grpKBMode.Controls.Add(this.comboKBModeUp);
			this.grpKBMode.Controls.Add(this.comboKBModeDown);
			this.grpKBMode.Location = new System.Drawing.Point(264, 112);
			this.grpKBMode.Name = "grpKBMode";
			this.grpKBMode.Size = new System.Drawing.Size(112, 72);
			this.grpKBMode.TabIndex = 14;
			this.grpKBMode.TabStop = false;
			this.grpKBMode.Text = "Mode";
			// 
			// lblKBModeUp
			// 
			this.lblKBModeUp.Image = null;
			this.lblKBModeUp.Location = new System.Drawing.Point(8, 16);
			this.lblKBModeUp.Name = "lblKBModeUp";
			this.lblKBModeUp.Size = new System.Drawing.Size(40, 16);
			this.lblKBModeUp.TabIndex = 10;
			this.lblKBModeUp.Text = "Up:";
			this.lblKBModeUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblKBModeDown
			// 
			this.lblKBModeDown.Image = null;
			this.lblKBModeDown.Location = new System.Drawing.Point(8, 40);
			this.lblKBModeDown.Name = "lblKBModeDown";
			this.lblKBModeDown.Size = new System.Drawing.Size(40, 16);
			this.lblKBModeDown.TabIndex = 9;
			this.lblKBModeDown.Text = "Down:";
			this.lblKBModeDown.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// comboKBModeUp
			// 
			this.comboKBModeUp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBModeUp.DropDownWidth = 56;
			this.comboKBModeUp.Location = new System.Drawing.Point(48, 16);
			this.comboKBModeUp.Name = "comboKBModeUp";
			this.comboKBModeUp.Size = new System.Drawing.Size(56, 21);
			this.comboKBModeUp.TabIndex = 6;
			this.toolTip1.SetToolTip(this.comboKBModeUp, "Select the Next mode.");
			this.comboKBModeUp.SelectedIndexChanged += new System.EventHandler(this.comboKBModeUp_SelectedIndexChanged);
			// 
			// comboKBModeDown
			// 
			this.comboKBModeDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBModeDown.DropDownWidth = 56;
			this.comboKBModeDown.Location = new System.Drawing.Point(48, 40);
			this.comboKBModeDown.Name = "comboKBModeDown";
			this.comboKBModeDown.Size = new System.Drawing.Size(56, 21);
			this.comboKBModeDown.TabIndex = 5;
			this.toolTip1.SetToolTip(this.comboKBModeDown, "Select the Previous mode.");
			this.comboKBModeDown.SelectedIndexChanged += new System.EventHandler(this.comboKBModeDown_SelectedIndexChanged);
			// 
			// grpKBBand
			// 
			this.grpKBBand.Controls.Add(this.lblKBBandUp);
			this.grpKBBand.Controls.Add(this.lblKBBandDown);
			this.grpKBBand.Controls.Add(this.comboKBBandUp);
			this.grpKBBand.Controls.Add(this.comboKBBandDown);
			this.grpKBBand.Location = new System.Drawing.Point(8, 112);
			this.grpKBBand.Name = "grpKBBand";
			this.grpKBBand.Size = new System.Drawing.Size(112, 72);
			this.grpKBBand.TabIndex = 12;
			this.grpKBBand.TabStop = false;
			this.grpKBBand.Text = "Band";
			// 
			// lblKBBandUp
			// 
			this.lblKBBandUp.Image = null;
			this.lblKBBandUp.Location = new System.Drawing.Point(8, 16);
			this.lblKBBandUp.Name = "lblKBBandUp";
			this.lblKBBandUp.Size = new System.Drawing.Size(40, 16);
			this.lblKBBandUp.TabIndex = 10;
			this.lblKBBandUp.Text = "Up:";
			this.lblKBBandUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblKBBandDown
			// 
			this.lblKBBandDown.Image = null;
			this.lblKBBandDown.Location = new System.Drawing.Point(8, 40);
			this.lblKBBandDown.Name = "lblKBBandDown";
			this.lblKBBandDown.Size = new System.Drawing.Size(40, 16);
			this.lblKBBandDown.TabIndex = 9;
			this.lblKBBandDown.Text = "Down:";
			this.lblKBBandDown.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// comboKBBandUp
			// 
			this.comboKBBandUp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBBandUp.DropDownWidth = 56;
			this.comboKBBandUp.Location = new System.Drawing.Point(48, 16);
			this.comboKBBandUp.Name = "comboKBBandUp";
			this.comboKBBandUp.Size = new System.Drawing.Size(56, 21);
			this.comboKBBandUp.TabIndex = 6;
			this.toolTip1.SetToolTip(this.comboKBBandUp, "Jump to the next band stack memory.");
			this.comboKBBandUp.SelectedIndexChanged += new System.EventHandler(this.comboKBBandUp_SelectedIndexChanged);
			// 
			// comboKBBandDown
			// 
			this.comboKBBandDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBBandDown.DropDownWidth = 56;
			this.comboKBBandDown.Location = new System.Drawing.Point(48, 40);
			this.comboKBBandDown.Name = "comboKBBandDown";
			this.comboKBBandDown.Size = new System.Drawing.Size(56, 21);
			this.comboKBBandDown.TabIndex = 5;
			this.toolTip1.SetToolTip(this.comboKBBandDown, "Jump to the previous band stack memory.");
			this.comboKBBandDown.SelectedIndexChanged += new System.EventHandler(this.comboKBBandDown_SelectedIndexChanged);
			// 
			// grpKBTune
			// 
			this.grpKBTune.Controls.Add(this.lblKBTuneDigit);
			this.grpKBTune.Controls.Add(this.lblKBTune7);
			this.grpKBTune.Controls.Add(this.lblKBTune6);
			this.grpKBTune.Controls.Add(this.lblKBTune5);
			this.grpKBTune.Controls.Add(this.lblKBTune4);
			this.grpKBTune.Controls.Add(this.lblKBTune3);
			this.grpKBTune.Controls.Add(this.lblKBTune2);
			this.grpKBTune.Controls.Add(this.comboKBTuneUp7);
			this.grpKBTune.Controls.Add(this.comboKBTuneDown7);
			this.grpKBTune.Controls.Add(this.comboKBTuneUp6);
			this.grpKBTune.Controls.Add(this.comboKBTuneDown6);
			this.grpKBTune.Controls.Add(this.comboKBTuneUp5);
			this.grpKBTune.Controls.Add(this.comboKBTuneDown5);
			this.grpKBTune.Controls.Add(this.comboKBTuneUp4);
			this.grpKBTune.Controls.Add(this.comboKBTuneDown4);
			this.grpKBTune.Controls.Add(this.lblKBTune1);
			this.grpKBTune.Controls.Add(this.lblKBTuneUp);
			this.grpKBTune.Controls.Add(this.lblKBTuneDown);
			this.grpKBTune.Controls.Add(this.comboKBTuneUp3);
			this.grpKBTune.Controls.Add(this.comboKBTuneDown3);
			this.grpKBTune.Controls.Add(this.comboKBTuneUp1);
			this.grpKBTune.Controls.Add(this.comboKBTuneUp2);
			this.grpKBTune.Controls.Add(this.comboKBTuneDown1);
			this.grpKBTune.Controls.Add(this.comboKBTuneDown2);
			this.grpKBTune.Location = new System.Drawing.Point(8, 8);
			this.grpKBTune.Name = "grpKBTune";
			this.grpKBTune.Size = new System.Drawing.Size(456, 96);
			this.grpKBTune.TabIndex = 11;
			this.grpKBTune.TabStop = false;
			this.grpKBTune.Text = "Tune";
			// 
			// lblKBTuneDigit
			// 
			this.lblKBTuneDigit.Image = null;
			this.lblKBTuneDigit.Location = new System.Drawing.Point(16, 16);
			this.lblKBTuneDigit.Name = "lblKBTuneDigit";
			this.lblKBTuneDigit.Size = new System.Drawing.Size(32, 16);
			this.lblKBTuneDigit.TabIndex = 26;
			this.lblKBTuneDigit.Text = "Digit";
			// 
			// lblKBTune7
			// 
			this.lblKBTune7.Image = null;
			this.lblKBTune7.Location = new System.Drawing.Point(392, 16);
			this.lblKBTune7.Name = "lblKBTune7";
			this.lblKBTune7.Size = new System.Drawing.Size(56, 16);
			this.lblKBTune7.TabIndex = 25;
			this.lblKBTune7.Text = "0.00000x";
			this.lblKBTune7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblKBTune6
			// 
			this.lblKBTune6.Image = null;
			this.lblKBTune6.Location = new System.Drawing.Point(336, 16);
			this.lblKBTune6.Name = "lblKBTune6";
			this.lblKBTune6.Size = new System.Drawing.Size(56, 16);
			this.lblKBTune6.TabIndex = 24;
			this.lblKBTune6.Text = "0.0000x0";
			this.lblKBTune6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblKBTune5
			// 
			this.lblKBTune5.Image = null;
			this.lblKBTune5.Location = new System.Drawing.Point(280, 16);
			this.lblKBTune5.Name = "lblKBTune5";
			this.lblKBTune5.Size = new System.Drawing.Size(56, 16);
			this.lblKBTune5.TabIndex = 23;
			this.lblKBTune5.Text = "0.000x00";
			this.lblKBTune5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblKBTune4
			// 
			this.lblKBTune4.Image = null;
			this.lblKBTune4.Location = new System.Drawing.Point(224, 16);
			this.lblKBTune4.Name = "lblKBTune4";
			this.lblKBTune4.Size = new System.Drawing.Size(56, 16);
			this.lblKBTune4.TabIndex = 22;
			this.lblKBTune4.Text = "0.00x000";
			this.lblKBTune4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblKBTune3
			// 
			this.lblKBTune3.Image = null;
			this.lblKBTune3.Location = new System.Drawing.Point(168, 16);
			this.lblKBTune3.Name = "lblKBTune3";
			this.lblKBTune3.Size = new System.Drawing.Size(56, 16);
			this.lblKBTune3.TabIndex = 21;
			this.lblKBTune3.Text = "0.0x0000";
			this.lblKBTune3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblKBTune2
			// 
			this.lblKBTune2.Image = null;
			this.lblKBTune2.Location = new System.Drawing.Point(112, 16);
			this.lblKBTune2.Name = "lblKBTune2";
			this.lblKBTune2.Size = new System.Drawing.Size(56, 16);
			this.lblKBTune2.TabIndex = 20;
			this.lblKBTune2.Text = "0.x00000";
			this.lblKBTune2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// comboKBTuneUp7
			// 
			this.comboKBTuneUp7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBTuneUp7.DropDownWidth = 56;
			this.comboKBTuneUp7.Location = new System.Drawing.Point(392, 40);
			this.comboKBTuneUp7.Name = "comboKBTuneUp7";
			this.comboKBTuneUp7.Size = new System.Drawing.Size(56, 21);
			this.comboKBTuneUp7.TabIndex = 19;
			this.toolTip1.SetToolTip(this.comboKBTuneUp7, "Tune Up 1Hz");
			this.comboKBTuneUp7.SelectedIndexChanged += new System.EventHandler(this.comboKBTuneUp7_SelectedIndexChanged);
			// 
			// comboKBTuneDown7
			// 
			this.comboKBTuneDown7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBTuneDown7.DropDownWidth = 56;
			this.comboKBTuneDown7.Location = new System.Drawing.Point(392, 64);
			this.comboKBTuneDown7.Name = "comboKBTuneDown7";
			this.comboKBTuneDown7.Size = new System.Drawing.Size(56, 21);
			this.comboKBTuneDown7.TabIndex = 18;
			this.toolTip1.SetToolTip(this.comboKBTuneDown7, "Tune Down 1Hz");
			this.comboKBTuneDown7.SelectedIndexChanged += new System.EventHandler(this.comboKBTuneDown7_SelectedIndexChanged);
			// 
			// comboKBTuneUp6
			// 
			this.comboKBTuneUp6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBTuneUp6.DropDownWidth = 56;
			this.comboKBTuneUp6.Location = new System.Drawing.Point(336, 40);
			this.comboKBTuneUp6.Name = "comboKBTuneUp6";
			this.comboKBTuneUp6.Size = new System.Drawing.Size(56, 21);
			this.comboKBTuneUp6.TabIndex = 17;
			this.toolTip1.SetToolTip(this.comboKBTuneUp6, "Tune Up 10Hz");
			this.comboKBTuneUp6.SelectedIndexChanged += new System.EventHandler(this.comboKBTuneUp6_SelectedIndexChanged);
			// 
			// comboKBTuneDown6
			// 
			this.comboKBTuneDown6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBTuneDown6.DropDownWidth = 56;
			this.comboKBTuneDown6.Location = new System.Drawing.Point(336, 64);
			this.comboKBTuneDown6.Name = "comboKBTuneDown6";
			this.comboKBTuneDown6.Size = new System.Drawing.Size(56, 21);
			this.comboKBTuneDown6.TabIndex = 16;
			this.toolTip1.SetToolTip(this.comboKBTuneDown6, "Tune Down 10Hz");
			this.comboKBTuneDown6.SelectedIndexChanged += new System.EventHandler(this.comboKBTuneDown6_SelectedIndexChanged);
			// 
			// comboKBTuneUp5
			// 
			this.comboKBTuneUp5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBTuneUp5.DropDownWidth = 56;
			this.comboKBTuneUp5.Location = new System.Drawing.Point(280, 40);
			this.comboKBTuneUp5.Name = "comboKBTuneUp5";
			this.comboKBTuneUp5.Size = new System.Drawing.Size(56, 21);
			this.comboKBTuneUp5.TabIndex = 15;
			this.toolTip1.SetToolTip(this.comboKBTuneUp5, "Tune Up 100Hz");
			this.comboKBTuneUp5.SelectedIndexChanged += new System.EventHandler(this.comboKBTuneUp5_SelectedIndexChanged);
			// 
			// comboKBTuneDown5
			// 
			this.comboKBTuneDown5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBTuneDown5.DropDownWidth = 56;
			this.comboKBTuneDown5.Location = new System.Drawing.Point(280, 64);
			this.comboKBTuneDown5.Name = "comboKBTuneDown5";
			this.comboKBTuneDown5.Size = new System.Drawing.Size(56, 21);
			this.comboKBTuneDown5.TabIndex = 14;
			this.toolTip1.SetToolTip(this.comboKBTuneDown5, "Tune Down 100Hz");
			this.comboKBTuneDown5.SelectedIndexChanged += new System.EventHandler(this.comboKBTuneDown5_SelectedIndexChanged);
			// 
			// comboKBTuneUp4
			// 
			this.comboKBTuneUp4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBTuneUp4.DropDownWidth = 56;
			this.comboKBTuneUp4.Location = new System.Drawing.Point(224, 40);
			this.comboKBTuneUp4.Name = "comboKBTuneUp4";
			this.comboKBTuneUp4.Size = new System.Drawing.Size(56, 21);
			this.comboKBTuneUp4.TabIndex = 13;
			this.toolTip1.SetToolTip(this.comboKBTuneUp4, "Tune Up 1kHz");
			this.comboKBTuneUp4.SelectedIndexChanged += new System.EventHandler(this.comboKBTuneUp4_SelectedIndexChanged);
			// 
			// comboKBTuneDown4
			// 
			this.comboKBTuneDown4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBTuneDown4.DropDownWidth = 56;
			this.comboKBTuneDown4.Location = new System.Drawing.Point(224, 64);
			this.comboKBTuneDown4.Name = "comboKBTuneDown4";
			this.comboKBTuneDown4.Size = new System.Drawing.Size(56, 21);
			this.comboKBTuneDown4.TabIndex = 12;
			this.toolTip1.SetToolTip(this.comboKBTuneDown4, "Tune Down 1kHz");
			this.comboKBTuneDown4.SelectedIndexChanged += new System.EventHandler(this.comboKBTuneDown4_SelectedIndexChanged);
			// 
			// lblKBTune1
			// 
			this.lblKBTune1.Image = null;
			this.lblKBTune1.Location = new System.Drawing.Point(48, 16);
			this.lblKBTune1.Name = "lblKBTune1";
			this.lblKBTune1.Size = new System.Drawing.Size(56, 16);
			this.lblKBTune1.TabIndex = 11;
			this.lblKBTune1.Text = "x.000000";
			this.lblKBTune1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblKBTuneUp
			// 
			this.lblKBTuneUp.Image = null;
			this.lblKBTuneUp.Location = new System.Drawing.Point(8, 40);
			this.lblKBTuneUp.Name = "lblKBTuneUp";
			this.lblKBTuneUp.Size = new System.Drawing.Size(40, 16);
			this.lblKBTuneUp.TabIndex = 8;
			this.lblKBTuneUp.Text = "Up:";
			this.lblKBTuneUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblKBTuneDown
			// 
			this.lblKBTuneDown.Image = null;
			this.lblKBTuneDown.Location = new System.Drawing.Point(8, 64);
			this.lblKBTuneDown.Name = "lblKBTuneDown";
			this.lblKBTuneDown.Size = new System.Drawing.Size(40, 16);
			this.lblKBTuneDown.TabIndex = 7;
			this.lblKBTuneDown.Text = "Down:";
			this.lblKBTuneDown.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// comboKBTuneUp3
			// 
			this.comboKBTuneUp3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBTuneUp3.DropDownWidth = 56;
			this.comboKBTuneUp3.Location = new System.Drawing.Point(168, 40);
			this.comboKBTuneUp3.Name = "comboKBTuneUp3";
			this.comboKBTuneUp3.Size = new System.Drawing.Size(56, 21);
			this.comboKBTuneUp3.TabIndex = 6;
			this.toolTip1.SetToolTip(this.comboKBTuneUp3, "Tune Up 10kHz");
			this.comboKBTuneUp3.SelectedIndexChanged += new System.EventHandler(this.comboKBTuneUp3_SelectedIndexChanged);
			// 
			// comboKBTuneDown3
			// 
			this.comboKBTuneDown3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBTuneDown3.DropDownWidth = 56;
			this.comboKBTuneDown3.Location = new System.Drawing.Point(168, 64);
			this.comboKBTuneDown3.Name = "comboKBTuneDown3";
			this.comboKBTuneDown3.Size = new System.Drawing.Size(56, 21);
			this.comboKBTuneDown3.TabIndex = 1;
			this.toolTip1.SetToolTip(this.comboKBTuneDown3, "Tune Down 10kHz");
			this.comboKBTuneDown3.SelectedIndexChanged += new System.EventHandler(this.comboKBTuneDown3_SelectedIndexChanged);
			// 
			// comboKBTuneUp1
			// 
			this.comboKBTuneUp1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBTuneUp1.DropDownWidth = 56;
			this.comboKBTuneUp1.Location = new System.Drawing.Point(48, 40);
			this.comboKBTuneUp1.Name = "comboKBTuneUp1";
			this.comboKBTuneUp1.Size = new System.Drawing.Size(56, 21);
			this.comboKBTuneUp1.TabIndex = 4;
			this.toolTip1.SetToolTip(this.comboKBTuneUp1, "Tune Up 1MHz");
			this.comboKBTuneUp1.SelectedIndexChanged += new System.EventHandler(this.comboKBTuneUp1_SelectedIndexChanged);
			// 
			// comboKBTuneUp2
			// 
			this.comboKBTuneUp2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBTuneUp2.DropDownWidth = 56;
			this.comboKBTuneUp2.Location = new System.Drawing.Point(112, 40);
			this.comboKBTuneUp2.Name = "comboKBTuneUp2";
			this.comboKBTuneUp2.Size = new System.Drawing.Size(56, 21);
			this.comboKBTuneUp2.TabIndex = 5;
			this.toolTip1.SetToolTip(this.comboKBTuneUp2, "Tune Up 100kHz");
			this.comboKBTuneUp2.SelectedIndexChanged += new System.EventHandler(this.comboKBTuneUp2_SelectedIndexChanged);
			// 
			// comboKBTuneDown1
			// 
			this.comboKBTuneDown1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBTuneDown1.DropDownWidth = 56;
			this.comboKBTuneDown1.Location = new System.Drawing.Point(48, 64);
			this.comboKBTuneDown1.Name = "comboKBTuneDown1";
			this.comboKBTuneDown1.Size = new System.Drawing.Size(56, 21);
			this.comboKBTuneDown1.TabIndex = 0;
			this.toolTip1.SetToolTip(this.comboKBTuneDown1, "Tune Down 1MHz");
			this.comboKBTuneDown1.SelectedIndexChanged += new System.EventHandler(this.comboKBTuneDown1_SelectedIndexChanged);
			// 
			// comboKBTuneDown2
			// 
			this.comboKBTuneDown2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBTuneDown2.DropDownWidth = 56;
			this.comboKBTuneDown2.Location = new System.Drawing.Point(112, 64);
			this.comboKBTuneDown2.Name = "comboKBTuneDown2";
			this.comboKBTuneDown2.Size = new System.Drawing.Size(56, 21);
			this.comboKBTuneDown2.TabIndex = 2;
			this.toolTip1.SetToolTip(this.comboKBTuneDown2, "Tune Down 100kHz");
			this.comboKBTuneDown2.SelectedIndexChanged += new System.EventHandler(this.comboKBTuneDown2_SelectedIndexChanged);
			// 
			// grpKBFilter
			// 
			this.grpKBFilter.Controls.Add(this.lblKBFilterUp);
			this.grpKBFilter.Controls.Add(this.lblKBFilterDown);
			this.grpKBFilter.Controls.Add(this.comboKBFilterUp);
			this.grpKBFilter.Controls.Add(this.comboKBFilterDown);
			this.grpKBFilter.Location = new System.Drawing.Point(136, 112);
			this.grpKBFilter.Name = "grpKBFilter";
			this.grpKBFilter.Size = new System.Drawing.Size(112, 72);
			this.grpKBFilter.TabIndex = 13;
			this.grpKBFilter.TabStop = false;
			this.grpKBFilter.Text = "Filter";
			// 
			// lblKBFilterUp
			// 
			this.lblKBFilterUp.Image = null;
			this.lblKBFilterUp.Location = new System.Drawing.Point(8, 16);
			this.lblKBFilterUp.Name = "lblKBFilterUp";
			this.lblKBFilterUp.Size = new System.Drawing.Size(40, 16);
			this.lblKBFilterUp.TabIndex = 10;
			this.lblKBFilterUp.Text = "Up:";
			this.lblKBFilterUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblKBFilterDown
			// 
			this.lblKBFilterDown.Image = null;
			this.lblKBFilterDown.Location = new System.Drawing.Point(8, 40);
			this.lblKBFilterDown.Name = "lblKBFilterDown";
			this.lblKBFilterDown.Size = new System.Drawing.Size(40, 16);
			this.lblKBFilterDown.TabIndex = 9;
			this.lblKBFilterDown.Text = "Down:";
			this.lblKBFilterDown.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// comboKBFilterUp
			// 
			this.comboKBFilterUp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBFilterUp.DropDownWidth = 56;
			this.comboKBFilterUp.Location = new System.Drawing.Point(48, 16);
			this.comboKBFilterUp.Name = "comboKBFilterUp";
			this.comboKBFilterUp.Size = new System.Drawing.Size(56, 21);
			this.comboKBFilterUp.TabIndex = 6;
			this.toolTip1.SetToolTip(this.comboKBFilterUp, "Select the Next filter.");
			this.comboKBFilterUp.SelectedIndexChanged += new System.EventHandler(this.comboKBFilterUp_SelectedIndexChanged);
			// 
			// comboKBFilterDown
			// 
			this.comboKBFilterDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBFilterDown.DropDownWidth = 56;
			this.comboKBFilterDown.Location = new System.Drawing.Point(48, 40);
			this.comboKBFilterDown.Name = "comboKBFilterDown";
			this.comboKBFilterDown.Size = new System.Drawing.Size(56, 21);
			this.comboKBFilterDown.TabIndex = 5;
			this.toolTip1.SetToolTip(this.comboKBFilterDown, "Select the Previous filter.");
			this.comboKBFilterDown.SelectedIndexChanged += new System.EventHandler(this.comboKBFilterDown_SelectedIndexChanged);
			// 
			// grpKBCW
			// 
			this.grpKBCW.Controls.Add(this.lblKBCWDot);
			this.grpKBCW.Controls.Add(this.lblKBCWDash);
			this.grpKBCW.Controls.Add(this.comboKBCWDot);
			this.grpKBCW.Controls.Add(this.comboKBCWDash);
			this.grpKBCW.Location = new System.Drawing.Point(264, 192);
			this.grpKBCW.Name = "grpKBCW";
			this.grpKBCW.Size = new System.Drawing.Size(112, 72);
			this.grpKBCW.TabIndex = 13;
			this.grpKBCW.TabStop = false;
			this.grpKBCW.Text = "CW";
			this.grpKBCW.Visible = false;
			// 
			// lblKBCWDot
			// 
			this.lblKBCWDot.Image = null;
			this.lblKBCWDot.Location = new System.Drawing.Point(8, 16);
			this.lblKBCWDot.Name = "lblKBCWDot";
			this.lblKBCWDot.Size = new System.Drawing.Size(40, 16);
			this.lblKBCWDot.TabIndex = 10;
			this.lblKBCWDot.Text = "Dot:";
			this.lblKBCWDot.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblKBCWDash
			// 
			this.lblKBCWDash.Image = null;
			this.lblKBCWDash.Location = new System.Drawing.Point(8, 40);
			this.lblKBCWDash.Name = "lblKBCWDash";
			this.lblKBCWDash.Size = new System.Drawing.Size(40, 16);
			this.lblKBCWDash.TabIndex = 9;
			this.lblKBCWDash.Text = "Dash:";
			this.lblKBCWDash.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// comboKBCWDot
			// 
			this.comboKBCWDot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBCWDot.DropDownWidth = 56;
			this.comboKBCWDot.Location = new System.Drawing.Point(48, 16);
			this.comboKBCWDot.Name = "comboKBCWDot";
			this.comboKBCWDot.Size = new System.Drawing.Size(56, 21);
			this.comboKBCWDot.TabIndex = 6;
			this.toolTip1.SetToolTip(this.comboKBCWDot, "Note: Only works with old keyer.");
			this.comboKBCWDot.SelectedIndexChanged += new System.EventHandler(this.comboKBCWDot_SelectedIndexChanged);
			// 
			// comboKBCWDash
			// 
			this.comboKBCWDash.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboKBCWDash.DropDownWidth = 56;
			this.comboKBCWDash.Location = new System.Drawing.Point(48, 40);
			this.comboKBCWDash.Name = "comboKBCWDash";
			this.comboKBCWDash.Size = new System.Drawing.Size(56, 21);
			this.comboKBCWDash.TabIndex = 5;
			this.toolTip1.SetToolTip(this.comboKBCWDash, "Note: Only works with old keyer.");
			this.comboKBCWDash.SelectedIndexChanged += new System.EventHandler(this.comboKBCWDash_SelectedIndexChanged);
			// 
			// tpCAT
			// 
			this.tpCAT.Controls.Add(this.lblCATRigType);
			this.tpCAT.Controls.Add(this.comboCATRigType);
			this.tpCAT.Controls.Add(this.btnCATTest);
			this.tpCAT.Controls.Add(this.grpPTTBitBang);
			this.tpCAT.Controls.Add(this.grpCatControlBox);
			this.tpCAT.Location = new System.Drawing.Point(4, 22);
			this.tpCAT.Name = "tpCAT";
			this.tpCAT.Size = new System.Drawing.Size(584, 286);
			this.tpCAT.TabIndex = 10;
			this.tpCAT.Text = "CAT Control";
			// 
			// lblCATRigType
			// 
			this.lblCATRigType.Image = null;
			this.lblCATRigType.Location = new System.Drawing.Point(440, 24);
			this.lblCATRigType.Name = "lblCATRigType";
			this.lblCATRigType.Size = new System.Drawing.Size(40, 23);
			this.lblCATRigType.TabIndex = 95;
			this.lblCATRigType.Text = "ID as:";
			// 
			// comboCATRigType
			// 
			this.comboCATRigType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCATRigType.DropDownWidth = 56;
			this.comboCATRigType.Items.AddRange(new object[] {
																 "SDR-1000",
																 "TS-2000",
																 "TS-50S",
																 "TS-480"});
			this.comboCATRigType.Location = new System.Drawing.Point(480, 24);
			this.comboCATRigType.Name = "comboCATRigType";
			this.comboCATRigType.Size = new System.Drawing.Size(88, 21);
			this.comboCATRigType.TabIndex = 94;
			this.toolTip1.SetToolTip(this.comboCATRigType, "Sets the CAT protocol for programs that do not have SDR-1000 specific setups.");
			this.comboCATRigType.SelectedIndexChanged += new System.EventHandler(this.comboCATRigType_SelectedIndexChanged);
			// 
			// btnCATTest
			// 
			this.btnCATTest.Image = null;
			this.btnCATTest.Location = new System.Drawing.Point(344, 24);
			this.btnCATTest.Name = "btnCATTest";
			this.btnCATTest.Size = new System.Drawing.Size(40, 20);
			this.btnCATTest.TabIndex = 92;
			this.btnCATTest.Text = "Test";
			this.btnCATTest.Click += new System.EventHandler(this.btnCATTest_Click);
			// 
			// grpPTTBitBang
			// 
			this.grpPTTBitBang.Controls.Add(this.comboCATPTTPort);
			this.grpPTTBitBang.Controls.Add(this.lblCATPTTPort);
			this.grpPTTBitBang.Controls.Add(this.chkCATPTT_RTS);
			this.grpPTTBitBang.Controls.Add(this.chkCATPTT_DTR);
			this.grpPTTBitBang.Controls.Add(this.chkCATPTTEnabled);
			this.grpPTTBitBang.Location = new System.Drawing.Point(192, 16);
			this.grpPTTBitBang.Name = "grpPTTBitBang";
			this.grpPTTBitBang.Size = new System.Drawing.Size(128, 152);
			this.grpPTTBitBang.TabIndex = 91;
			this.grpPTTBitBang.TabStop = false;
			this.grpPTTBitBang.Text = "PTT Control";
			// 
			// comboCATPTTPort
			// 
			this.comboCATPTTPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCATPTTPort.DropDownWidth = 56;
			this.comboCATPTTPort.Location = new System.Drawing.Point(40, 56);
			this.comboCATPTTPort.Name = "comboCATPTTPort";
			this.comboCATPTTPort.Size = new System.Drawing.Size(80, 21);
			this.comboCATPTTPort.TabIndex = 96;
			this.toolTip1.SetToolTip(this.comboCATPTTPort, "Selects the COM port for use with PTT control");
			this.comboCATPTTPort.SelectedIndexChanged += new System.EventHandler(this.comboCATPTTPort_SelectedIndexChanged);
			// 
			// lblCATPTTPort
			// 
			this.lblCATPTTPort.Image = null;
			this.lblCATPTTPort.Location = new System.Drawing.Point(8, 56);
			this.lblCATPTTPort.Name = "lblCATPTTPort";
			this.lblCATPTTPort.Size = new System.Drawing.Size(40, 23);
			this.lblCATPTTPort.TabIndex = 6;
			this.lblCATPTTPort.Text = "Port:";
			// 
			// chkCATPTT_RTS
			// 
			this.chkCATPTT_RTS.Image = null;
			this.chkCATPTT_RTS.Location = new System.Drawing.Point(40, 88);
			this.chkCATPTT_RTS.Name = "chkCATPTT_RTS";
			this.chkCATPTT_RTS.Size = new System.Drawing.Size(48, 24);
			this.chkCATPTT_RTS.TabIndex = 0;
			this.chkCATPTT_RTS.Text = "RTS";
			this.chkCATPTT_RTS.CheckedChanged += new System.EventHandler(this.chkCATPTT_RTS_CheckedChanged);
			// 
			// chkCATPTT_DTR
			// 
			this.chkCATPTT_DTR.Image = null;
			this.chkCATPTT_DTR.Location = new System.Drawing.Point(40, 120);
			this.chkCATPTT_DTR.Name = "chkCATPTT_DTR";
			this.chkCATPTT_DTR.Size = new System.Drawing.Size(48, 16);
			this.chkCATPTT_DTR.TabIndex = 1;
			this.chkCATPTT_DTR.Text = "DTR";
			this.chkCATPTT_DTR.CheckedChanged += new System.EventHandler(this.chkCATPTT_DTR_CheckedChanged);
			// 
			// chkCATPTTEnabled
			// 
			this.chkCATPTTEnabled.Image = null;
			this.chkCATPTTEnabled.Location = new System.Drawing.Point(8, 16);
			this.chkCATPTTEnabled.Name = "chkCATPTTEnabled";
			this.chkCATPTTEnabled.TabIndex = 4;
			this.chkCATPTTEnabled.Text = "Enable PTT";
			this.chkCATPTTEnabled.CheckedChanged += new System.EventHandler(this.chkCATPTTEnabled_CheckedChanged);
			// 
			// grpCatControlBox
			// 
			this.grpCatControlBox.Controls.Add(this.comboCATPort);
			this.grpCatControlBox.Controls.Add(this.comboCATbaud);
			this.grpCatControlBox.Controls.Add(this.lblCATBaud);
			this.grpCatControlBox.Controls.Add(this.lblCATPort);
			this.grpCatControlBox.Controls.Add(this.chkCATEnable);
			this.grpCatControlBox.Controls.Add(this.lblCATParity);
			this.grpCatControlBox.Controls.Add(this.lblCATData);
			this.grpCatControlBox.Controls.Add(this.lblCATStop);
			this.grpCatControlBox.Controls.Add(this.comboCATparity);
			this.grpCatControlBox.Controls.Add(this.comboCATdatabits);
			this.grpCatControlBox.Controls.Add(this.comboCATstopbits);
			this.grpCatControlBox.Location = new System.Drawing.Point(16, 16);
			this.grpCatControlBox.Name = "grpCatControlBox";
			this.grpCatControlBox.Size = new System.Drawing.Size(160, 216);
			this.grpCatControlBox.TabIndex = 90;
			this.grpCatControlBox.TabStop = false;
			this.grpCatControlBox.Text = "CAT Control";
			// 
			// comboCATPort
			// 
			this.comboCATPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCATPort.DropDownWidth = 56;
			this.comboCATPort.Location = new System.Drawing.Point(72, 48);
			this.comboCATPort.Name = "comboCATPort";
			this.comboCATPort.Size = new System.Drawing.Size(72, 21);
			this.comboCATPort.TabIndex = 95;
			this.toolTip1.SetToolTip(this.comboCATPort, "Sets the COM port to be used for the CAT interface.");
			this.comboCATPort.SelectedIndexChanged += new System.EventHandler(this.comboCATPort_SelectedIndexChanged);
			// 
			// comboCATbaud
			// 
			this.comboCATbaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCATbaud.DropDownWidth = 56;
			this.comboCATbaud.Items.AddRange(new object[] {
															  "300",
															  "1200",
															  "2400",
															  "4800",
															  "9600",
															  "19200",
															  "38400",
															  "57600"});
			this.comboCATbaud.Location = new System.Drawing.Point(72, 88);
			this.comboCATbaud.Name = "comboCATbaud";
			this.comboCATbaud.Size = new System.Drawing.Size(72, 21);
			this.comboCATbaud.TabIndex = 93;
			this.comboCATbaud.SelectedIndexChanged += new System.EventHandler(this.comboCATbaud_SelectedIndexChanged);
			// 
			// lblCATBaud
			// 
			this.lblCATBaud.Image = null;
			this.lblCATBaud.Location = new System.Drawing.Point(24, 88);
			this.lblCATBaud.Name = "lblCATBaud";
			this.lblCATBaud.Size = new System.Drawing.Size(40, 23);
			this.lblCATBaud.TabIndex = 5;
			this.lblCATBaud.Text = "Baud";
			// 
			// lblCATPort
			// 
			this.lblCATPort.Image = null;
			this.lblCATPort.Location = new System.Drawing.Point(24, 48);
			this.lblCATPort.Name = "lblCATPort";
			this.lblCATPort.Size = new System.Drawing.Size(40, 23);
			this.lblCATPort.TabIndex = 3;
			this.lblCATPort.Text = "Port:";
			// 
			// chkCATEnable
			// 
			this.chkCATEnable.Image = null;
			this.chkCATEnable.Location = new System.Drawing.Point(16, 24);
			this.chkCATEnable.Name = "chkCATEnable";
			this.chkCATEnable.TabIndex = 0;
			this.chkCATEnable.Text = "Enable CAT";
			this.chkCATEnable.CheckedChanged += new System.EventHandler(this.chkCATEnable_CheckedChanged);
			// 
			// lblCATParity
			// 
			this.lblCATParity.Image = null;
			this.lblCATParity.Location = new System.Drawing.Point(24, 120);
			this.lblCATParity.Name = "lblCATParity";
			this.lblCATParity.Size = new System.Drawing.Size(48, 23);
			this.lblCATParity.TabIndex = 92;
			this.lblCATParity.Text = "Parity";
			// 
			// lblCATData
			// 
			this.lblCATData.Image = null;
			this.lblCATData.Location = new System.Drawing.Point(24, 152);
			this.lblCATData.Name = "lblCATData";
			this.lblCATData.Size = new System.Drawing.Size(40, 23);
			this.lblCATData.TabIndex = 92;
			this.lblCATData.Text = "Data";
			// 
			// lblCATStop
			// 
			this.lblCATStop.Image = null;
			this.lblCATStop.Location = new System.Drawing.Point(24, 184);
			this.lblCATStop.Name = "lblCATStop";
			this.lblCATStop.Size = new System.Drawing.Size(40, 23);
			this.lblCATStop.TabIndex = 92;
			this.lblCATStop.Text = "Stop";
			// 
			// comboCATparity
			// 
			this.comboCATparity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCATparity.DropDownWidth = 56;
			this.comboCATparity.Items.AddRange(new object[] {
																"none",
																"odd ",
																"even",
																"mark",
																"space"});
			this.comboCATparity.Location = new System.Drawing.Point(72, 120);
			this.comboCATparity.Name = "comboCATparity";
			this.comboCATparity.Size = new System.Drawing.Size(72, 21);
			this.comboCATparity.TabIndex = 92;
			this.comboCATparity.SelectedIndexChanged += new System.EventHandler(this.comboCATparity_SelectedIndexChanged);
			// 
			// comboCATdatabits
			// 
			this.comboCATdatabits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCATdatabits.DropDownWidth = 56;
			this.comboCATdatabits.Items.AddRange(new object[] {
																  "8",
																  "7",
																  "6"});
			this.comboCATdatabits.Location = new System.Drawing.Point(72, 152);
			this.comboCATdatabits.Name = "comboCATdatabits";
			this.comboCATdatabits.Size = new System.Drawing.Size(72, 21);
			this.comboCATdatabits.TabIndex = 93;
			this.comboCATdatabits.SelectedIndexChanged += new System.EventHandler(this.comboCATdatabits_SelectedIndexChanged);
			// 
			// comboCATstopbits
			// 
			this.comboCATstopbits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCATstopbits.DropDownWidth = 56;
			this.comboCATstopbits.Items.AddRange(new object[] {
																  "1",
																  "1.5",
																  "2"});
			this.comboCATstopbits.Location = new System.Drawing.Point(72, 184);
			this.comboCATstopbits.Name = "comboCATstopbits";
			this.comboCATstopbits.Size = new System.Drawing.Size(72, 21);
			this.comboCATstopbits.TabIndex = 94;
			this.comboCATstopbits.SelectedIndexChanged += new System.EventHandler(this.comboCATstopbits_SelectedIndexChanged);
			// 
			// tpTests
			// 
			this.tpTests.Controls.Add(this.grpBoxTS1);
			this.tpTests.Controls.Add(this.grpTestX2);
			this.tpTests.Controls.Add(this.grpTestAudioBalance);
			this.tpTests.Controls.Add(this.grpTestTXIMD);
			this.tpTests.Controls.Add(this.grpImpulseTest);
			this.tpTests.Location = new System.Drawing.Point(4, 22);
			this.tpTests.Name = "tpTests";
			this.tpTests.Size = new System.Drawing.Size(584, 286);
			this.tpTests.TabIndex = 7;
			this.tpTests.Text = "Tests";
			// 
			// grpBoxTS1
			// 
			this.grpBoxTS1.Controls.Add(this.lblTestGenScale);
			this.grpBoxTS1.Controls.Add(this.udTestGenScale);
			this.grpBoxTS1.Controls.Add(this.radTestGenOutput);
			this.grpBoxTS1.Controls.Add(this.radTestGenInput);
			this.grpBoxTS1.Controls.Add(this.cmboTestGenMode);
			this.grpBoxTS1.Controls.Add(this.lblTestSigGenFreqCallout);
			this.grpBoxTS1.Controls.Add(this.tkbarTestGenFreq);
			this.grpBoxTS1.Controls.Add(this.lblTestGenHzSec);
			this.grpBoxTS1.Controls.Add(this.udTestGenHzSec);
			this.grpBoxTS1.Controls.Add(this.lblTestGenHigh);
			this.grpBoxTS1.Controls.Add(this.udTestGenHigh);
			this.grpBoxTS1.Controls.Add(this.lblTestGenLow);
			this.grpBoxTS1.Controls.Add(this.udTestGenLow);
			this.grpBoxTS1.Controls.Add(this.btnTestGenSweep);
			this.grpBoxTS1.Location = new System.Drawing.Point(176, 120);
			this.grpBoxTS1.Name = "grpBoxTS1";
			this.grpBoxTS1.Size = new System.Drawing.Size(400, 152);
			this.grpBoxTS1.TabIndex = 88;
			this.grpBoxTS1.TabStop = false;
			this.grpBoxTS1.Text = "Signal Generator";
			// 
			// lblTestGenScale
			// 
			this.lblTestGenScale.Image = null;
			this.lblTestGenScale.Location = new System.Drawing.Point(280, 24);
			this.lblTestGenScale.Name = "lblTestGenScale";
			this.lblTestGenScale.Size = new System.Drawing.Size(40, 16);
			this.lblTestGenScale.TabIndex = 95;
			this.lblTestGenScale.Text = "Scale:";
			this.lblTestGenScale.Visible = false;
			// 
			// udTestGenScale
			// 
			this.udTestGenScale.DecimalPlaces = 6;
			this.udTestGenScale.Increment = new System.Decimal(new int[] {
																			 1,
																			 0,
																			 0,
																			 196608});
			this.udTestGenScale.Location = new System.Drawing.Point(320, 24);
			this.udTestGenScale.Maximum = new System.Decimal(new int[] {
																		   20,
																		   0,
																		   0,
																		   0});
			this.udTestGenScale.Minimum = new System.Decimal(new int[] {
																		   0,
																		   0,
																		   0,
																		   0});
			this.udTestGenScale.Name = "udTestGenScale";
			this.udTestGenScale.Size = new System.Drawing.Size(72, 20);
			this.udTestGenScale.TabIndex = 94;
			this.toolTip1.SetToolTip(this.udTestGenScale, "Sets the amplitude of the signal (typically between 0 and 1.0)");
			this.udTestGenScale.Value = new System.Decimal(new int[] {
																		 10,
																		 0,
																		 0,
																		 65536});
			this.udTestGenScale.Visible = false;
			this.udTestGenScale.LostFocus += new System.EventHandler(this.udTestGenScale_LostFocus);
			this.udTestGenScale.ValueChanged += new System.EventHandler(this.updnTestGenScale_ValueChanged);
			// 
			// radTestGenOutput
			// 
			this.radTestGenOutput.Image = null;
			this.radTestGenOutput.Location = new System.Drawing.Point(208, 24);
			this.radTestGenOutput.Name = "radTestGenOutput";
			this.radTestGenOutput.Size = new System.Drawing.Size(56, 24);
			this.radTestGenOutput.TabIndex = 93;
			this.radTestGenOutput.Text = "Output";
			this.toolTip1.SetToolTip(this.radTestGenOutput, "Select this button if the signal is to be an output to the soundcard.");
			this.radTestGenOutput.CheckedChanged += new System.EventHandler(this.rbTestGenOutput_CheckedChanged);
			// 
			// radTestGenInput
			// 
			this.radTestGenInput.Checked = true;
			this.radTestGenInput.Image = null;
			this.radTestGenInput.Location = new System.Drawing.Point(152, 24);
			this.radTestGenInput.Name = "radTestGenInput";
			this.radTestGenInput.Size = new System.Drawing.Size(56, 24);
			this.radTestGenInput.TabIndex = 92;
			this.radTestGenInput.TabStop = true;
			this.radTestGenInput.Text = "Input";
			this.toolTip1.SetToolTip(this.radTestGenInput, "Select this button if the signal is to be an input to the DSP.");
			this.radTestGenInput.CheckedChanged += new System.EventHandler(this.rbTestGenInput_CheckedChanged);
			// 
			// cmboTestGenMode
			// 
			this.cmboTestGenMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmboTestGenMode.DropDownWidth = 121;
			this.cmboTestGenMode.Items.AddRange(new object[] {
																 "Off",
																 "Tone",
																 "Noise",
																 "Triangle",
																 "Sawtooth"});
			this.cmboTestGenMode.Location = new System.Drawing.Point(16, 24);
			this.cmboTestGenMode.Name = "cmboTestGenMode";
			this.cmboTestGenMode.Size = new System.Drawing.Size(121, 21);
			this.cmboTestGenMode.TabIndex = 91;
			this.toolTip1.SetToolTip(this.cmboTestGenMode, "Select the signal type.");
			this.cmboTestGenMode.SelectedIndexChanged += new System.EventHandler(this.cmboTestGenMode_SelectedIndexChanged);
			// 
			// lblTestSigGenFreqCallout
			// 
			this.lblTestSigGenFreqCallout.Image = null;
			this.lblTestSigGenFreqCallout.Location = new System.Drawing.Point(24, 88);
			this.lblTestSigGenFreqCallout.Name = "lblTestSigGenFreqCallout";
			this.lblTestSigGenFreqCallout.Size = new System.Drawing.Size(336, 16);
			this.lblTestSigGenFreqCallout.TabIndex = 90;
			this.lblTestSigGenFreqCallout.Text = "0                                                 10k                            " +
				"                   20k";
			// 
			// tkbarTestGenFreq
			// 
			this.tkbarTestGenFreq.Location = new System.Drawing.Point(16, 56);
			this.tkbarTestGenFreq.Maximum = 20000;
			this.tkbarTestGenFreq.Name = "tkbarTestGenFreq";
			this.tkbarTestGenFreq.Size = new System.Drawing.Size(344, 50);
			this.tkbarTestGenFreq.TabIndex = 1;
			this.tkbarTestGenFreq.TickFrequency = 1000;
			this.toolTip1.SetToolTip(this.tkbarTestGenFreq, "Sets the frequency of the signal.");
			this.tkbarTestGenFreq.Value = 10000;
			this.tkbarTestGenFreq.Scroll += new System.EventHandler(this.tkbarTestGenFreq_Scroll);
			// 
			// lblTestGenHzSec
			// 
			this.lblTestGenHzSec.Image = null;
			this.lblTestGenHzSec.Location = new System.Drawing.Point(200, 112);
			this.lblTestGenHzSec.Name = "lblTestGenHzSec";
			this.lblTestGenHzSec.Size = new System.Drawing.Size(48, 16);
			this.lblTestGenHzSec.TabIndex = 88;
			this.lblTestGenHzSec.Text = "Hz/Sec:";
			// 
			// udTestGenHzSec
			// 
			this.udTestGenHzSec.Increment = new System.Decimal(new int[] {
																			 1,
																			 0,
																			 0,
																			 0});
			this.udTestGenHzSec.Location = new System.Drawing.Point(248, 112);
			this.udTestGenHzSec.Maximum = new System.Decimal(new int[] {
																		   20000,
																		   0,
																		   0,
																		   0});
			this.udTestGenHzSec.Minimum = new System.Decimal(new int[] {
																		   0,
																		   0,
																		   0,
																		   0});
			this.udTestGenHzSec.Name = "udTestGenHzSec";
			this.udTestGenHzSec.Size = new System.Drawing.Size(56, 20);
			this.udTestGenHzSec.TabIndex = 87;
			this.toolTip1.SetToolTip(this.udTestGenHzSec, "See the Sweep Button to the right.");
			this.udTestGenHzSec.Value = new System.Decimal(new int[] {
																		 100,
																		 0,
																		 0,
																		 0});
			this.udTestGenHzSec.LostFocus += new System.EventHandler(this.udTestGenHzSec_LostFocus);
			// 
			// lblTestGenHigh
			// 
			this.lblTestGenHigh.Image = null;
			this.lblTestGenHigh.Location = new System.Drawing.Point(104, 112);
			this.lblTestGenHigh.Name = "lblTestGenHigh";
			this.lblTestGenHigh.Size = new System.Drawing.Size(32, 16);
			this.lblTestGenHigh.TabIndex = 86;
			this.lblTestGenHigh.Text = "High:";
			// 
			// udTestGenHigh
			// 
			this.udTestGenHigh.Increment = new System.Decimal(new int[] {
																			1,
																			0,
																			0,
																			0});
			this.udTestGenHigh.Location = new System.Drawing.Point(136, 112);
			this.udTestGenHigh.Maximum = new System.Decimal(new int[] {
																		  20000,
																		  0,
																		  0,
																		  0});
			this.udTestGenHigh.Minimum = new System.Decimal(new int[] {
																		  0,
																		  0,
																		  0,
																		  0});
			this.udTestGenHigh.Name = "udTestGenHigh";
			this.udTestGenHigh.Size = new System.Drawing.Size(56, 20);
			this.udTestGenHigh.TabIndex = 85;
			this.toolTip1.SetToolTip(this.udTestGenHigh, "See the Sweep Button to the right.");
			this.udTestGenHigh.Value = new System.Decimal(new int[] {
																		4000,
																		0,
																		0,
																		0});
			this.udTestGenHigh.LostFocus += new System.EventHandler(this.udTestGenHigh_LostFocus);
			// 
			// lblTestGenLow
			// 
			this.lblTestGenLow.Image = null;
			this.lblTestGenLow.Location = new System.Drawing.Point(8, 112);
			this.lblTestGenLow.Name = "lblTestGenLow";
			this.lblTestGenLow.Size = new System.Drawing.Size(32, 16);
			this.lblTestGenLow.TabIndex = 84;
			this.lblTestGenLow.Text = "Low:";
			// 
			// udTestGenLow
			// 
			this.udTestGenLow.Increment = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   0});
			this.udTestGenLow.Location = new System.Drawing.Point(40, 112);
			this.udTestGenLow.Maximum = new System.Decimal(new int[] {
																		 20000,
																		 0,
																		 0,
																		 0});
			this.udTestGenLow.Minimum = new System.Decimal(new int[] {
																		 0,
																		 0,
																		 0,
																		 0});
			this.udTestGenLow.Name = "udTestGenLow";
			this.udTestGenLow.Size = new System.Drawing.Size(56, 20);
			this.udTestGenLow.TabIndex = 83;
			this.toolTip1.SetToolTip(this.udTestGenLow, "See the Sweep Button to the right.");
			this.udTestGenLow.Value = new System.Decimal(new int[] {
																	   0,
																	   0,
																	   0,
																	   0});
			this.udTestGenLow.LostFocus += new System.EventHandler(this.udTestGenLow_LostFocus);
			// 
			// btnTestGenSweep
			// 
			this.btnTestGenSweep.Image = null;
			this.btnTestGenSweep.Location = new System.Drawing.Point(336, 112);
			this.btnTestGenSweep.Name = "btnTestGenSweep";
			this.btnTestGenSweep.Size = new System.Drawing.Size(48, 23);
			this.btnTestGenSweep.TabIndex = 0;
			this.btnTestGenSweep.Text = "Sweep";
			this.toolTip1.SetToolTip(this.btnTestGenSweep, "Click this button to sweep from the Low setting to the High setting using the Hz/" +
				"Sec setting.");
			this.btnTestGenSweep.Click += new System.EventHandler(this.buttonTestGenSweep_Click);
			// 
			// grpTestX2
			// 
			this.grpTestX2.Controls.Add(this.lblTestX2);
			this.grpTestX2.Controls.Add(this.chkTestX2Pin6);
			this.grpTestX2.Controls.Add(this.chkTestX2Pin5);
			this.grpTestX2.Controls.Add(this.chkTestX2Pin4);
			this.grpTestX2.Controls.Add(this.chkTestX2Pin3);
			this.grpTestX2.Controls.Add(this.chkTestX2Pin2);
			this.grpTestX2.Controls.Add(this.chkTestX2Pin1);
			this.grpTestX2.Location = new System.Drawing.Point(8, 160);
			this.grpTestX2.Name = "grpTestX2";
			this.grpTestX2.Size = new System.Drawing.Size(160, 72);
			this.grpTestX2.TabIndex = 87;
			this.grpTestX2.TabStop = false;
			this.grpTestX2.Text = "X2";
			// 
			// lblTestX2
			// 
			this.lblTestX2.Image = null;
			this.lblTestX2.Location = new System.Drawing.Point(16, 48);
			this.lblTestX2.Name = "lblTestX2";
			this.lblTestX2.Size = new System.Drawing.Size(136, 16);
			this.lblTestX2.TabIndex = 6;
			this.lblTestX2.Text = "1      2      3      4      5      6";
			// 
			// chkTestX2Pin6
			// 
			this.chkTestX2Pin6.Image = null;
			this.chkTestX2Pin6.Location = new System.Drawing.Point(136, 24);
			this.chkTestX2Pin6.Name = "chkTestX2Pin6";
			this.chkTestX2Pin6.Size = new System.Drawing.Size(16, 24);
			this.chkTestX2Pin6.TabIndex = 5;
			this.chkTestX2Pin6.Text = "checkBox6";
			this.chkTestX2Pin6.CheckedChanged += new System.EventHandler(this.chkTestX2_CheckedChanged);
			// 
			// chkTestX2Pin5
			// 
			this.chkTestX2Pin5.Image = null;
			this.chkTestX2Pin5.Location = new System.Drawing.Point(112, 24);
			this.chkTestX2Pin5.Name = "chkTestX2Pin5";
			this.chkTestX2Pin5.Size = new System.Drawing.Size(16, 24);
			this.chkTestX2Pin5.TabIndex = 4;
			this.chkTestX2Pin5.Text = "checkBox5";
			this.chkTestX2Pin5.CheckedChanged += new System.EventHandler(this.chkTestX2_CheckedChanged);
			// 
			// chkTestX2Pin4
			// 
			this.chkTestX2Pin4.Image = null;
			this.chkTestX2Pin4.Location = new System.Drawing.Point(88, 24);
			this.chkTestX2Pin4.Name = "chkTestX2Pin4";
			this.chkTestX2Pin4.Size = new System.Drawing.Size(16, 24);
			this.chkTestX2Pin4.TabIndex = 3;
			this.chkTestX2Pin4.Text = "checkBox4";
			this.chkTestX2Pin4.CheckedChanged += new System.EventHandler(this.chkTestX2_CheckedChanged);
			// 
			// chkTestX2Pin3
			// 
			this.chkTestX2Pin3.Image = null;
			this.chkTestX2Pin3.Location = new System.Drawing.Point(64, 24);
			this.chkTestX2Pin3.Name = "chkTestX2Pin3";
			this.chkTestX2Pin3.Size = new System.Drawing.Size(16, 24);
			this.chkTestX2Pin3.TabIndex = 2;
			this.chkTestX2Pin3.Text = "checkBox3";
			this.chkTestX2Pin3.CheckedChanged += new System.EventHandler(this.chkTestX2_CheckedChanged);
			// 
			// chkTestX2Pin2
			// 
			this.chkTestX2Pin2.Image = null;
			this.chkTestX2Pin2.Location = new System.Drawing.Point(40, 24);
			this.chkTestX2Pin2.Name = "chkTestX2Pin2";
			this.chkTestX2Pin2.Size = new System.Drawing.Size(16, 24);
			this.chkTestX2Pin2.TabIndex = 1;
			this.chkTestX2Pin2.Text = "checkBox2";
			this.chkTestX2Pin2.CheckedChanged += new System.EventHandler(this.chkTestX2_CheckedChanged);
			// 
			// chkTestX2Pin1
			// 
			this.chkTestX2Pin1.Image = null;
			this.chkTestX2Pin1.Location = new System.Drawing.Point(16, 24);
			this.chkTestX2Pin1.Name = "chkTestX2Pin1";
			this.chkTestX2Pin1.Size = new System.Drawing.Size(16, 24);
			this.chkTestX2Pin1.TabIndex = 0;
			this.chkTestX2Pin1.Text = "checkBox1";
			this.chkTestX2Pin1.CheckedChanged += new System.EventHandler(this.chkTestX2_CheckedChanged);
			// 
			// grpTestAudioBalance
			// 
			this.grpTestAudioBalance.Controls.Add(this.btnTestAudioBalStart);
			this.grpTestAudioBalance.Location = new System.Drawing.Point(344, 8);
			this.grpTestAudioBalance.Name = "grpTestAudioBalance";
			this.grpTestAudioBalance.Size = new System.Drawing.Size(120, 64);
			this.grpTestAudioBalance.TabIndex = 86;
			this.grpTestAudioBalance.TabStop = false;
			this.grpTestAudioBalance.Text = "Audio Balance Test";
			// 
			// btnTestAudioBalStart
			// 
			this.btnTestAudioBalStart.Image = null;
			this.btnTestAudioBalStart.Location = new System.Drawing.Point(24, 24);
			this.btnTestAudioBalStart.Name = "btnTestAudioBalStart";
			this.btnTestAudioBalStart.TabIndex = 0;
			this.btnTestAudioBalStart.Text = "Start";
			this.btnTestAudioBalStart.Click += new System.EventHandler(this.btnTestAudioBalStart_Click);
			// 
			// grpTestTXIMD
			// 
			this.grpTestTXIMD.Controls.Add(this.lblTestToneFreq2);
			this.grpTestTXIMD.Controls.Add(this.udTestIMDFreq2);
			this.grpTestTXIMD.Controls.Add(this.lblTestIMDPower);
			this.grpTestTXIMD.Controls.Add(this.udTestIMDPower);
			this.grpTestTXIMD.Controls.Add(this.chekTestIMD);
			this.grpTestTXIMD.Controls.Add(this.lblTestToneFreq1);
			this.grpTestTXIMD.Controls.Add(this.udTestIMDFreq1);
			this.grpTestTXIMD.Location = new System.Drawing.Point(8, 8);
			this.grpTestTXIMD.Name = "grpTestTXIMD";
			this.grpTestTXIMD.Size = new System.Drawing.Size(152, 144);
			this.grpTestTXIMD.TabIndex = 83;
			this.grpTestTXIMD.TabStop = false;
			this.grpTestTXIMD.Text = "TX IMD Test";
			// 
			// lblTestToneFreq2
			// 
			this.lblTestToneFreq2.Image = null;
			this.lblTestToneFreq2.Location = new System.Drawing.Point(16, 48);
			this.lblTestToneFreq2.Name = "lblTestToneFreq2";
			this.lblTestToneFreq2.Size = new System.Drawing.Size(64, 16);
			this.lblTestToneFreq2.TabIndex = 88;
			this.lblTestToneFreq2.Text = "Freq #2:";
			// 
			// udTestIMDFreq2
			// 
			this.udTestIMDFreq2.Increment = new System.Decimal(new int[] {
																			 1,
																			 0,
																			 0,
																			 0});
			this.udTestIMDFreq2.Location = new System.Drawing.Point(80, 48);
			this.udTestIMDFreq2.Maximum = new System.Decimal(new int[] {
																		   20000,
																		   0,
																		   0,
																		   0});
			this.udTestIMDFreq2.Minimum = new System.Decimal(new int[] {
																		   0,
																		   0,
																		   0,
																		   0});
			this.udTestIMDFreq2.Name = "udTestIMDFreq2";
			this.udTestIMDFreq2.Size = new System.Drawing.Size(56, 20);
			this.udTestIMDFreq2.TabIndex = 87;
			this.udTestIMDFreq2.Value = new System.Decimal(new int[] {
																		 19000,
																		 0,
																		 0,
																		 65536});
			this.udTestIMDFreq2.LostFocus += new System.EventHandler(this.udTestIMDFreq2_LostFocus);
			// 
			// lblTestIMDPower
			// 
			this.lblTestIMDPower.Image = null;
			this.lblTestIMDPower.Location = new System.Drawing.Point(16, 72);
			this.lblTestIMDPower.Name = "lblTestIMDPower";
			this.lblTestIMDPower.Size = new System.Drawing.Size(64, 16);
			this.lblTestIMDPower.TabIndex = 86;
			this.lblTestIMDPower.Text = "Power:";
			// 
			// udTestIMDPower
			// 
			this.udTestIMDPower.Increment = new System.Decimal(new int[] {
																			 1,
																			 0,
																			 0,
																			 0});
			this.udTestIMDPower.Location = new System.Drawing.Point(80, 72);
			this.udTestIMDPower.Maximum = new System.Decimal(new int[] {
																		   100,
																		   0,
																		   0,
																		   0});
			this.udTestIMDPower.Minimum = new System.Decimal(new int[] {
																		   0,
																		   0,
																		   0,
																		   0});
			this.udTestIMDPower.Name = "udTestIMDPower";
			this.udTestIMDPower.Size = new System.Drawing.Size(56, 20);
			this.udTestIMDPower.TabIndex = 85;
			this.udTestIMDPower.Value = new System.Decimal(new int[] {
																		 50,
																		 0,
																		 0,
																		 0});
			this.udTestIMDPower.LostFocus += new System.EventHandler(this.udTestIMDPower_LostFocus);
			// 
			// chekTestIMD
			// 
			this.chekTestIMD.Appearance = System.Windows.Forms.Appearance.Button;
			this.chekTestIMD.Image = null;
			this.chekTestIMD.Location = new System.Drawing.Point(48, 104);
			this.chekTestIMD.Name = "chekTestIMD";
			this.chekTestIMD.Size = new System.Drawing.Size(64, 24);
			this.chekTestIMD.TabIndex = 84;
			this.chekTestIMD.Text = "Start";
			this.chekTestIMD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chekTestIMD.CheckedChanged += new System.EventHandler(this.chkTestIMD_CheckedChanged);
			// 
			// lblTestToneFreq1
			// 
			this.lblTestToneFreq1.Image = null;
			this.lblTestToneFreq1.Location = new System.Drawing.Point(16, 24);
			this.lblTestToneFreq1.Name = "lblTestToneFreq1";
			this.lblTestToneFreq1.Size = new System.Drawing.Size(64, 16);
			this.lblTestToneFreq1.TabIndex = 83;
			this.lblTestToneFreq1.Text = "Freq #1:";
			// 
			// udTestIMDFreq1
			// 
			this.udTestIMDFreq1.Increment = new System.Decimal(new int[] {
																			 1,
																			 0,
																			 0,
																			 0});
			this.udTestIMDFreq1.Location = new System.Drawing.Point(80, 24);
			this.udTestIMDFreq1.Maximum = new System.Decimal(new int[] {
																		   20000,
																		   0,
																		   0,
																		   0});
			this.udTestIMDFreq1.Minimum = new System.Decimal(new int[] {
																		   0,
																		   0,
																		   0,
																		   0});
			this.udTestIMDFreq1.Name = "udTestIMDFreq1";
			this.udTestIMDFreq1.Size = new System.Drawing.Size(56, 20);
			this.udTestIMDFreq1.TabIndex = 82;
			this.udTestIMDFreq1.Value = new System.Decimal(new int[] {
																		 7000,
																		 0,
																		 0,
																		 65536});
			this.udTestIMDFreq1.LostFocus += new System.EventHandler(this.udTestIMDFreq1_LostFocus);
			// 
			// grpImpulseTest
			// 
			this.grpImpulseTest.Controls.Add(this.udImpulseNum);
			this.grpImpulseTest.Controls.Add(this.btnImpulse);
			this.grpImpulseTest.Location = new System.Drawing.Point(168, 8);
			this.grpImpulseTest.Name = "grpImpulseTest";
			this.grpImpulseTest.Size = new System.Drawing.Size(160, 64);
			this.grpImpulseTest.TabIndex = 91;
			this.grpImpulseTest.TabStop = false;
			this.grpImpulseTest.Text = "Impulse Test";
			// 
			// udImpulseNum
			// 
			this.udImpulseNum.Increment = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   0});
			this.udImpulseNum.Location = new System.Drawing.Point(104, 24);
			this.udImpulseNum.Maximum = new System.Decimal(new int[] {
																		 20,
																		 0,
																		 0,
																		 0});
			this.udImpulseNum.Minimum = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 0});
			this.udImpulseNum.Name = "udImpulseNum";
			this.udImpulseNum.Size = new System.Drawing.Size(40, 20);
			this.udImpulseNum.TabIndex = 92;
			this.udImpulseNum.Value = new System.Decimal(new int[] {
																	   20,
																	   0,
																	   0,
																	   0});
			this.udImpulseNum.LostFocus += new System.EventHandler(this.udImpulseNum_LostFocus);
			// 
			// btnImpulse
			// 
			this.btnImpulse.Location = new System.Drawing.Point(16, 24);
			this.btnImpulse.Name = "btnImpulse";
			this.btnImpulse.TabIndex = 90;
			this.btnImpulse.Text = "Impulse";
			this.btnImpulse.Click += new System.EventHandler(this.btnImpulse_Click);
			// 
			// btnOK
			// 
			this.btnOK.Image = null;
			this.btnOK.Location = new System.Drawing.Point(328, 328);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 17;
			this.btnOK.Text = "OK";
			this.toolTip1.SetToolTip(this.btnOK, "Keep current settings and close form.");
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Image = null;
			this.btnCancel.Location = new System.Drawing.Point(416, 328);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 18;
			this.btnCancel.Text = "Cancel";
			this.toolTip1.SetToolTip(this.btnCancel, "Load settings from database and close form.");
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnApply
			// 
			this.btnApply.Image = null;
			this.btnApply.Location = new System.Drawing.Point(504, 328);
			this.btnApply.Name = "btnApply";
			this.btnApply.TabIndex = 19;
			this.btnApply.Text = "Apply";
			this.toolTip1.SetToolTip(this.btnApply, "Save current settings to the database.");
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// btnResetAll
			// 
			this.btnResetAll.Image = null;
			this.btnResetAll.Location = new System.Drawing.Point(24, 328);
			this.btnResetAll.Name = "btnResetAll";
			this.btnResetAll.TabIndex = 20;
			this.btnResetAll.Text = "Reset All";
			this.toolTip1.SetToolTip(this.btnResetAll, "Reset values to defaults.");
			this.btnResetAll.Click += new System.EventHandler(this.btnResetAll_Click);
			// 
			// btnImportDB
			// 
			this.btnImportDB.Image = null;
			this.btnImportDB.Location = new System.Drawing.Point(136, 328);
			this.btnImportDB.Name = "btnImportDB";
			this.btnImportDB.TabIndex = 21;
			this.btnImportDB.Text = "Import DB...";
			this.toolTip1.SetToolTip(this.btnImportDB, "Import a saved PowerSDR Database file.");
			this.btnImportDB.Click += new System.EventHandler(this.btnImportDB_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.InitialDirectory = "Application.StartupPath";
			this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
			// 
			// timer_sweep
			// 
			this.timer_sweep.Tick += new System.EventHandler(this.timer_sweep_Tick);
			// 
			// Setup
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(616, 357);
			this.Controls.Add(this.btnImportDB);
			this.Controls.Add(this.btnResetAll);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.tcSetup);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Setup";
			this.Text = "PowerSDR Setup";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Setup_Closing);
			this.tcSetup.ResumeLayout(false);
			this.tpGeneral.ResumeLayout(false);
			this.tcGeneral.ResumeLayout(false);
			this.tpGeneralHardware.ResumeLayout(false);
			this.grpHWSoftRock.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udSoftRockCenterFreq)).EndInit();
			this.grpGeneralDDS.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udDDSCorrection)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDDSIFFreq)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDDSPLLMult)).EndInit();
			this.grpGeneralModel.ResumeLayout(false);
			this.grpGeneralHardwareSetup.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udGeneralLPTDelay)).EndInit();
			this.tpGeneralOptions.ResumeLayout(false);
			this.grpOptMainConsole.ResumeLayout(false);
			this.grpOptQuickQSY.ResumeLayout(false);
			this.grpGenAutoMute.ResumeLayout(false);
			this.grpGenTuningOptions.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udOptClickTuneOffsetDIGL)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udOptClickTuneOffsetDIGU)).EndInit();
			this.grpGeneralOptions.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udGeneralX2Delay)).EndInit();
			this.grpGeneralProcessPriority.ResumeLayout(false);
			this.grpGeneralUpdates.ResumeLayout(false);
			this.tpGeneralCalibration.ResumeLayout(false);
			this.grpGenCalRXImage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udGeneralCalFreq3)).EndInit();
			this.grpGenCalLevel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udGeneralCalLevel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udGeneralCalFreq2)).EndInit();
			this.grpGeneralCalibration.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udGeneralCalFreq1)).EndInit();
			this.tpFilters.ResumeLayout(false);
			this.grpOptFilterControls.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udFilterDefaultLowCut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udOptMaxFilterShift)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udOptMaxFilterWidth)).EndInit();
			this.tpAudio.ResumeLayout(false);
			this.tcAudio.ResumeLayout(false);
			this.tpAudioCard1.ResumeLayout(false);
			this.grpAudioMicBoost.ResumeLayout(false);
			this.grpAudioChannels.ResumeLayout(false);
			this.grpAudioMicInGain1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udAudioMicGain1)).EndInit();
			this.grpAudioLineInGain1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udAudioLineIn1)).EndInit();
			this.grpAudioVolts1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udAudioVoltage1)).EndInit();
			this.grpAudioDetails1.ResumeLayout(false);
			this.grpAudioLatency1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udAudioLatency1)).EndInit();
			this.grpAudioCard.ResumeLayout(false);
			this.grpAudioBufferSize1.ResumeLayout(false);
			this.grpAudioSampleRate1.ResumeLayout(false);
			this.tpVAC.ResumeLayout(false);
			this.grpAudioVACAutoEnable.ResumeLayout(false);
			this.grpAudioVACGain.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udAudioVACGainTX)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udAudioVACGainRX)).EndInit();
			this.grpAudio2Stereo.ResumeLayout(false);
			this.grpAudioLatency2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udAudioLatency2)).EndInit();
			this.grpAudioSampleRate2.ResumeLayout(false);
			this.grpAudioBuffer2.ResumeLayout(false);
			this.grpAudioDetails2.ResumeLayout(false);
			this.tpDisplay.ResumeLayout(false);
			this.grpDisplayDriverEngine.ResumeLayout(false);
			this.grpDisplayPolyPhase.ResumeLayout(false);
			this.grpDisplayScopeMode.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udDisplayScopeTime)).EndInit();
			this.grpDisplayMultimeter.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udDisplayMeterAvg)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayMultiTextHoldTime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayMultiPeakHoldTime)).EndInit();
			this.grpDisplayWaterfall.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udDisplayWaterfallLowLevel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayWaterfallHighLevel)).EndInit();
			this.grpDisplayRefreshRates.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udDisplayCPUMeter)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayPeakText)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayMeterDelay)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayFPS)).EndInit();
			this.grpDisplayAverage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udDisplayAVGTime)).EndInit();
			this.grpDisplayPhase.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udDisplayPhasePts)).EndInit();
			this.grpDisplaySpectrumGrid.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udDisplayGridStep)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayGridMin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDisplayGridMax)).EndInit();
			this.tpDSP.ResumeLayout(false);
			this.tcDSP.ResumeLayout(false);
			this.tpDSPOptions.ResumeLayout(false);
			this.grpDSPBufferSize.ResumeLayout(false);
			this.grpDSPNB.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udDSPNB)).EndInit();
			this.grpDSPLMSNR.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udLMSNRgain)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udLMSNRdelay)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udLMSNRtaps)).EndInit();
			this.grpDSPLMSANF.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udLMSANFgain)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udLMSANFdelay)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udLMSANFtaps)).EndInit();
			this.grpDSPWindow.ResumeLayout(false);
			this.grpDSPNB2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udDSPNB2)).EndInit();
			this.tpDSPImageReject.ResumeLayout(false);
			this.grpDSPImageRejectRX.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udDSPImageGainRX)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPImagePhaseRX)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbDSPImagePhaseRX)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbDSPImageGainRX)).EndInit();
			this.grpDSPImageRejectTX.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udDSPImageGainTX)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPImagePhaseTX)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbDSPImagePhaseTX)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbDSPImageGainTX)).EndInit();
			this.tpDSPKeyer.ResumeLayout(false);
			this.grpKeyerConnections.ResumeLayout(false);
			this.grpDSPCWPitch.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udDSPCWPitch)).EndInit();
			this.grpDSPKeyerOptions.ResumeLayout(false);
			this.grpDSPKeyerSignalShaping.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udCWKeyerDeBounce)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udCWKeyerWeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udCWKeyerRamp)).EndInit();
			this.grpDSPKeyerSemiBreakIn.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udCWKeyerSemiBreakInDelay)).EndInit();
			this.tpDSPAGCALC.ResumeLayout(false);
			this.grpDSPLeveler.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udDSPLevelerHangTime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPLevelerThreshold)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPLevelerSlope)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPLevelerDecay)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPLevelerAttack)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbDSPLevelerHangThreshold)).EndInit();
			this.grpDSPALC.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbDSPALCHangThreshold)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPALCHangTime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPALCThreshold)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPALCSlope)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPALCDecay)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPALCAttack)).EndInit();
			this.grpDSPAGC.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbDSPAGCHangThreshold)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPAGCHangTime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPAGCMaxGaindB)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPAGCSlope)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPAGCDecay)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPAGCAttack)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDSPAGCFixedGaindB)).EndInit();
			this.tpTransmit.ResumeLayout(false);
			this.grpTXAM.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udTXAMCarrierLevel)).EndInit();
			this.grpTXMonitor.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udTXAF)).EndInit();
			this.grpTXVOX.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udTXVOXHangTime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udTXVOXThreshold)).EndInit();
			this.grpTXNoiseGate.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udTXNoiseGate)).EndInit();
			this.grpTXProfile.ResumeLayout(false);
			this.grpPATune.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udTXTunePower)).EndInit();
			this.grpTXCompression.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbTXCompander)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udTXCompander)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udTXFFCompression)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXFFCompression)).EndInit();
			this.grpTXFilter.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udTXFilterLow)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udTXFilterHigh)).EndInit();
			this.tpPowerAmplifier.ResumeLayout(false);
			this.grpPABandOffset.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udPAADC17)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC15)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC20)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC12)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC10)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC160)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC80)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC60)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC40)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAADC30)).EndInit();
			this.grpPAGainByBand.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udPAGain10)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain12)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain15)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain17)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain20)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain30)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain40)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain60)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain80)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGain160)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPACalPower)).EndInit();
			this.tpExtCtrl.ResumeLayout(false);
			this.grpExtTX.ResumeLayout(false);
			this.grpExtRX.ResumeLayout(false);
			this.tpAppearance.ResumeLayout(false);
			this.tcAppearance.ResumeLayout(false);
			this.tpAppearanceDisplay.ResumeLayout(false);
			this.grpAppPanadapter.ResumeLayout(false);
			this.grpDisplayPeakCursor.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udDisplayLineWidth)).EndInit();
			this.tpAppearanceGeneral.ResumeLayout(false);
			this.grpAppearanceBand.ResumeLayout(false);
			this.grpAppearanceVFO.ResumeLayout(false);
			this.tpAppearanceMeter.ResumeLayout(false);
			this.grpMeterEdge.ResumeLayout(false);
			this.grpAppearanceMeter.ResumeLayout(false);
			this.tpKeyboard.ResumeLayout(false);
			this.grpKBXIT.ResumeLayout(false);
			this.grpKBRIT.ResumeLayout(false);
			this.grpKBMode.ResumeLayout(false);
			this.grpKBBand.ResumeLayout(false);
			this.grpKBTune.ResumeLayout(false);
			this.grpKBFilter.ResumeLayout(false);
			this.grpKBCW.ResumeLayout(false);
			this.tpCAT.ResumeLayout(false);
			this.grpPTTBitBang.ResumeLayout(false);
			this.grpCatControlBox.ResumeLayout(false);
			this.tpTests.ResumeLayout(false);
			this.grpBoxTS1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udTestGenScale)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tkbarTestGenFreq)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udTestGenHzSec)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udTestGenHigh)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udTestGenLow)).EndInit();
			this.grpTestX2.ResumeLayout(false);
			this.grpTestAudioBalance.ResumeLayout(false);
			this.grpTestTXIMD.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udTestIMDFreq2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udTestIMDPower)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udTestIMDFreq1)).EndInit();
			this.grpImpulseTest.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udImpulseNum)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Init Routines
		// ======================================================
		// Init Routines
		// ======================================================

		private void InitGeneralTab()
		{
			comboGeneralLPTAddr.Text = Convert.ToString(console.Hdw.LPTAddr, 16);
			udGeneralLPTDelay.Value = console.LatchDelay;
			chkGeneralRXOnly.Checked = console.RXOnly;
			chkGeneralUSBPresent.Checked = console.USBPresent;
			chkBoxJanusOzyControl.Checked = console.OzyControl;
			chkGeneralRFEPresent.Checked = console.RFEPresent;
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
			if(comboAudioDriver1.SelectedIndex < 0 &&
				comboAudioDriver1.Items.Count > 0)
			{
				if(comboAudioDriver1.Items.Count == 3)
					comboAudioDriver1.SelectedIndex = 2;
				else if(comboAudioDriver1.Items.Count == 2)
					comboAudioDriver1.SelectedIndex = 1;
				else comboAudioDriver1.SelectedIndex = 0;
			}

			// set Input device
			if(comboAudioInput1.Items.Count > 0)
			{
				comboAudioInput1.SelectedIndex = console.AudioInputIndex1;

				// set Output device
				if(comboAudioDriver1.SelectedIndex < 2)
					comboAudioOutput1.SelectedIndex = console.AudioOutputIndex1 - comboAudioInput1.Items.Count;
				else
					comboAudioOutput1.SelectedIndex = console.AudioOutputIndex1;
			}
			
			// set sample rate
			// setup sample rate here

			if(comboAudioReceive1.Enabled == true)
			{
				for(int i=0; i<comboAudioReceive1.Items.Count; i++)
				{
					if(((string)comboAudioReceive1.Items[i]).StartsWith("Line"))
					{
						comboAudioReceive1.SelectedIndex = i;
						i = comboAudioReceive1.Items.Count;
					}
				}
			}

			if(comboAudioTransmit1.Enabled == true)
			{
				for(int i=0; i<comboAudioTransmit1.Items.Count; i++)
				{
					if(((string)comboAudioTransmit1.Items[i]).StartsWith("Mic"))
					{
						comboAudioTransmit1.SelectedIndex = i;
						i = comboAudioTransmit1.Items.Count;
					}
				}
			}

			comboAudioBuffer1.Text = "2048";
			udAudioLatency1.Value = console.AudioLatency1;
		}

		private void InitDisplayTab()
		{
			udDisplayGridMax.Value = Display.SpectrumGridMax;
			udDisplayGridMin.Value = Display.SpectrumGridMin;
			udDisplayGridStep.Value = Display.SpectrumGridStep;
			udDisplayFPS.Value = console.DisplayFPS;
			clrbtnWaterfallLow.Color = Display.WaterfallLowColor;
			clrbtnWaterfallMid.Color = Display.WaterfallMidColor;
			clrbtnWaterfallHigh.Color = Display.WaterfallHighColor;
			udDisplayWaterfallLowLevel.Value = (decimal)Display.WaterfallLowThreshold;
			udDisplayWaterfallHighLevel.Value = (decimal)Display.WaterfallHighThreshold;
			udDisplayMeterDelay.Value = console.MeterDelay;
			udDisplayPeakText.Value = console.PeakTextDelay;
			udDisplayCPUMeter.Value = console.CPUMeterDelay;
			udDisplayPhasePts.Value = Display.PhaseNumPts;
			udDisplayMultiPeakHoldTime.Value = console.MultimeterPeakHoldTime;
			udDisplayMultiTextHoldTime.Value = console.MultimeterTextPeakTime;
		}

		private void InitDSPTab()
		{
			udDSPCWPitch.Value = console.CWPitch;
			comboDSPWindow.SelectedIndex = (int)DttSP.CurrentWindow;
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
			if(retval != 0) return;
			DeltaCP.SetLevels();
			DeltaCP.Close();
		}

		private void RefreshCOMPortLists()
		{
			ArrayList com_ports = GetAvailCOMPorts();

			comboKeyerConnPrimary.Items.Clear();
			comboKeyerConnPrimary.Items.Add("SDR");
			comboKeyerConnPrimary.Items.Add("Ozy");

			comboKeyerConnSecondary.Items.Clear();
			comboKeyerConnSecondary.Items.Add("None");
			comboKeyerConnSecondary.Items.Add("CAT");

			comboCATPort.Items.Clear();
			comboCATPTTPort.Items.Clear();

			foreach(string s in com_ports)
			{
				comboKeyerConnPrimary.Items.Add(s);
				comboKeyerConnSecondary.Items.Add(s);
				comboCATPort.Items.Add(s);
				comboCATPTTPort.Items.Add(s);
			}
		}

		private ArrayList GetAvailCOMPorts()
		{
			ArrayList a = new ArrayList();
			for (int i=1;i<24;i++)
			{
				SerialPorts.SerialPort sp = new SerialPorts.SerialPort();
				sp.PortName = "COM"+i.ToString();
				try 
				{
					sp.Open();
					if(sp.IsOpen)
					{
						a.Add(sp.PortName);
						sp.Close();
					}
				}
				catch(Exception) {};
			}
			return a;
		}

		private void InitWindowTypes()
		{
			for(DttSP.Window w = DttSP.Window.FIRST+1; w<DttSP.Window.LAST; w++)
			{
				string s = w.ToString().ToLower();
				s = s.Substring(0, 1).ToUpper() + s.Substring(1, s.Length-1);
				comboDSPWindow.Items.Add(s);
			}
		}

		private void GetHosts()
		{
			comboAudioDriver1.Items.Clear();
			comboAudioDriver2.Items.Clear();
			int host_index = 0;
			foreach(string PAHostName in Audio.GetPAHosts())
			{
				if(Audio.GetPAInputDevices(host_index).Count > 0 ||
					Audio.GetPAOutputDevices(host_index).Count > 0)
				{
					comboAudioDriver1.Items.Add(new PADeviceInfo(PAHostName, host_index));
					comboAudioDriver2.Items.Add(new PADeviceInfo(PAHostName, host_index));
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
			foreach(PADeviceInfo p in a)
				comboAudioInput1.Items.Add(p);

			a = Audio.GetPAOutputDevices(host);
			foreach(PADeviceInfo p in a)
				comboAudioOutput1.Items.Add(p);
		}

		private void GetDevices2()
		{
			comboAudioInput2.Items.Clear();
			comboAudioOutput2.Items.Clear();
			int host = ((PADeviceInfo)comboAudioDriver2.SelectedItem).Index;
			ArrayList a = Audio.GetPAInputDevices(host);
			foreach(PADeviceInfo p in a)
				comboAudioInput2.Items.Add(p);

			a = Audio.GetPAOutputDevices(host);
			foreach(PADeviceInfo p in a)
				comboAudioOutput2.Items.Add(p);
		}

		private void ControlList(Control c, ref ArrayList a)
		{
			if(c.Controls.Count > 0)
			{
				foreach(Control c2 in c.Controls)
					ControlList(c2, ref a);
			}

			if(c.GetType() == typeof(CheckBoxTS) || c.GetType() == typeof(CheckBoxTS) ||
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
			// pages on this form of the following types: CheckBoxTS, ComboBox,
			// NumericUpDown, RadioButton, TextBox, and TrackBar (slider)

			saving = true;

			ArrayList a = new ArrayList();
			ArrayList temp = new ArrayList();

			ControlList(this, ref temp);

			foreach(Control c in temp)				// For each control
			{
				// System.Console.WriteLine("processing: " + c.Name + " " + c.GetType()); 
				if(c.GetType() == typeof(CheckBoxTS))
					a.Add(c.Name+"/"+((CheckBoxTS)c).Checked.ToString());
				else if(c.GetType() == typeof(ComboBoxTS))
				{
					//if(((ComboBox)c).SelectedIndex >= 0)
					a.Add(c.Name+"/"+((ComboBoxTS)c).Text);
				}
				else if(c.GetType() == typeof(NumericUpDownTS))
					a.Add(c.Name+"/"+((NumericUpDownTS)c).Value.ToString());
				else if(c.GetType() == typeof(RadioButtonTS))
					a.Add(c.Name+"/"+((RadioButtonTS)c).Checked.ToString());
				else if(c.GetType() == typeof(TextBoxTS))
					a.Add(c.Name+"/"+((TextBoxTS)c).Text);
				else if(c.GetType() == typeof(TrackBarTS))
					a.Add(c.Name+"/"+((TrackBarTS)c).Value.ToString());
				else if(c.GetType() == typeof(ColorButton))
				{
					Color clr = ((ColorButton)c).Color;
					a.Add(c.Name+"/"+clr.R+"."+clr.G+"."+clr.B+"."+clr.A);
				}
				// else 
				// { 
				// 	System.Console.WriteLine("not saved: " + c.Name + " " + c.GetType()); 
				// } 
#if(DEBUG)
				else if(c.GetType() == typeof(GroupBox) ||
					c.GetType() == typeof(CheckBoxTS) ||
					c.GetType() == typeof(ComboBox) ||
					c.GetType() == typeof(NumericUpDown) ||
					c.GetType() == typeof(RadioButton) ||
					c.GetType() == typeof(TextBox) ||
					c.GetType() == typeof(TrackBar))
					Debug.WriteLine(c.Name+" needs to be converted to a Thread Safe control.");
#endif
			}

			DB.SaveOptions(ref a);		// save the values to the DB
			saving = false;
		}

		public void GetOptions()
		{
			// Automatically restores all controls from the database in the
			// tab pages on this form of the following types: CheckBoxTS, ComboBox,
			// NumericUpDown, RadioButton, TextBox, and TrackBar (slider)

			// get list of live controls
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
			foreach(Control c in temp)
			{
				if(c.GetType() == typeof(CheckBoxTS))			// the control is a CheckBoxTS
					checkbox_list.Add(c);
				else if(c.GetType() == typeof(ComboBoxTS))		// the control is a ComboBox
					combobox_list.Add(c);
				else if(c.GetType() == typeof(NumericUpDownTS))	// the control is a NumericUpDown
					numericupdown_list.Add(c);
				else if(c.GetType() == typeof(RadioButtonTS))	// the control is a RadioButton
					radiobutton_list.Add(c);
				else if(c.GetType() == typeof(TextBoxTS))		// the control is a TextBox
					textbox_list.Add(c);
				else if(c.GetType() == typeof(TrackBarTS))		// the control is a TrackBar (slider)
					trackbar_list.Add(c);
				else if(c.GetType() == typeof(ColorButton))
					colorbutton_list.Add(c);
			}
			temp.Clear();	// now that we have the controls we want, delete first list 

			ArrayList a = DB.GetOptions();						// Get the saved list of controls
			a.Sort();
			int num_controls = checkbox_list.Count + combobox_list.Count +
				numericupdown_list.Count + radiobutton_list.Count +
				textbox_list.Count + trackbar_list.Count +
				colorbutton_list.Count;

			if(a.Count < num_controls)		// some control values are not in the database
			{								// so set all of them to the defaults
				InitGeneralTab();
				InitAudioTab();
				InitDSPTab();
				InitDisplayTab();
				InitKeyboardTab();
				InitAppearanceTab();
			}
			
			// restore saved values to the controls
			foreach(string s in a)				// string is in the format "name,value"
			{
				string[] vals = s.Split('/');
				if(vals.Length > 2)
				{
					for(int i=2; i<vals.Length; i++)
						vals[1] += "/"+vals[i];
				}

				string name = vals[0];
				string val = vals[1];

				if(s.StartsWith("chk"))			// control is a CheckBoxTS
				{
					for(int i=0; i<checkbox_list.Count; i++)
					{	// look through each control to find the matching name
						CheckBoxTS c = (CheckBoxTS)checkbox_list[i];
						if(c.Name.Equals(name))		// name found
						{
							c.Checked = bool.Parse(val);	// restore value
							i = checkbox_list.Count+1;
						}
						if(i == checkbox_list.Count)
							MessageBox.Show("Control not found: "+name);
					}
				}
				else if(s.StartsWith("combo"))	// control is a ComboBox
				{
					for(int i=0; i<combobox_list.Count; i++)
					{	// look through each control to find the matching name
						ComboBoxTS c = (ComboBoxTS)combobox_list[i];
						if(c.Name.Equals(name))		// name found
						{
							if(c.Items.Count > 0 && c.Items[0].GetType() == typeof(string))
							{
								c.Text = val;
							}
							else
							{
								foreach(object o in c.Items)
								{
									if(o.ToString() == val)
										c.Text = val;	// restore value
								}
							}
							i = combobox_list.Count+1;
						}
						if(i == combobox_list.Count)
							MessageBox.Show("Control not found: "+name);
					}
				}
				else if(s.StartsWith("ud"))
				{
					for(int i=0; i<numericupdown_list.Count; i++)
					{	// look through each control to find the matching name
						NumericUpDownTS c = (NumericUpDownTS)numericupdown_list[i];
						if(c.Name.Equals(name))		// name found
						{
							decimal num = decimal.Parse(val);

							if(num > c.Maximum) num = c.Maximum;		// check endpoints
							else if(num < c.Minimum) num = c.Minimum;
							c.Value = num;			// restore value
							i = numericupdown_list.Count+1;
						}
						if(i == numericupdown_list.Count)
							MessageBox.Show("Control not found: "+name);	
					}
				}
				else if(s.StartsWith("rad"))
				{	// look through each control to find the matching name
					for(int i=0; i<radiobutton_list.Count; i++)
					{
						RadioButtonTS c = (RadioButtonTS)radiobutton_list[i];
						if(c.Name.Equals(name))		// name found
						{
							c.Checked = bool.Parse(val);	// restore value
							i = radiobutton_list.Count+1;
						}
						if(i == radiobutton_list.Count)
							MessageBox.Show("Control not found: "+name);
					}
				}
				else if(s.StartsWith("txt"))
				{	// look through each control to find the matching name
					for(int i=0; i<textbox_list.Count; i++)
					{
						TextBoxTS c = (TextBoxTS)textbox_list[i];
						if(c.Name.Equals(name))		// name found
						{
							c.Text = val;	// restore value
							i = textbox_list.Count+1;
						}
						if(i == textbox_list.Count)
							MessageBox.Show("Control not found: "+name);
					}
				}
				else if(s.StartsWith("tb"))
				{
					// look through each control to find the matching name
					for(int i=0; i<trackbar_list.Count; i++)
					{
						TrackBarTS c = (TrackBarTS)trackbar_list[i];
						if(c.Name.Equals(name))		// name found
						{
							c.Value = Int32.Parse(val);
							i = trackbar_list.Count+1;
						}
						if(i == trackbar_list.Count)
							MessageBox.Show("Control not found: "+name);
					}
				}
				else if(s.StartsWith("clrbtn"))
				{
					string[] colors = val.Split('.');
					if(colors.Length == 4)
					{
						int R,G,B,A;
						R = Int32.Parse(colors[0]);
						G = Int32.Parse(colors[1]);
						B = Int32.Parse(colors[2]);
						A = Int32.Parse(colors[3]);

						for(int i=0; i<colorbutton_list.Count; i++)
						{
							ColorButton c = (ColorButton)colorbutton_list[i];
							if(c.Name.Equals(name))		// name found
							{
								c.Color = Color.FromArgb(A, R, G, B);
								i = colorbutton_list.Count+1;
							}
							if(i == colorbutton_list.Count)
								MessageBox.Show("Control not found: "+name);
						}
					}
				}
			}

			foreach(ColorButton c in colorbutton_list)
				c.Automatic = "";
		}

		private string KeyToString(Keys k)
		{
			if(!k.ToString().StartsWith("Oem"))
				return k.ToString();

			string s = "";
			switch(k)
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
					if((k & Keys.Shift) == 0)
						s = "\\";
					else s = "|";
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

			foreach(Control c in tpKeyboard.Controls)
			{
				if(c.GetType() == typeof(GroupBoxTS))
				{
					foreach(Control c2 in c.Controls)
					{
						if(c2.GetType() == typeof(ComboBoxTS))
						{
							ComboBoxTS combo = (ComboBoxTS)c2;
							combo.Items.Clear();
							foreach(Keys k in KeyList)
							{
								if(k.ToString().StartsWith("Oem"))
									combo.Items.Add(KeyToString(k));
								else
									combo.Items.Add(k.ToString());
							}
						}
					}
				}
				else if(c.GetType() == typeof(ComboBoxTS))
				{
					ComboBoxTS combo = (ComboBoxTS)c;
					combo.Items.Clear();
					foreach(Keys k in KeyList)
						combo.Items.Add(k.ToString());
				}
			}
		}

		private void UpdateMixerControls1()
		{
			if(comboAudioMixer1.SelectedIndex >= 0 &&
				comboAudioMixer1.Items.Count > 0)
			{
				int i = -1;

				i = Mixer.GetMux(comboAudioMixer1.SelectedIndex);
				if(i < 0 || i >= Mixer.MIXERR_BASE)
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
					for(int j=0; j<comboAudioReceive1.Items.Count; j++)
					{
						if(((string)comboAudioReceive1.Items[j]).StartsWith("Line"))
						{
							comboAudioReceive1.SelectedIndex = j;
							j = comboAudioReceive1.Items.Count;
						}							
					}

					if(comboAudioReceive1.SelectedIndex < 0)
					{
						for(int j=0; j<comboAudioReceive1.Items.Count; j++)
						{
							if(((string)comboAudioReceive1.Items[j]).StartsWith("Analog"))
							{
								comboAudioReceive1.SelectedIndex = j;
								j = comboAudioReceive1.Items.Count;
							}							
						}
					}

					for(int j=0; j<comboAudioTransmit1.Items.Count; j++)
					{
						if(((string)comboAudioTransmit1.Items[j]).StartsWith("Mic"))
						{
							comboAudioTransmit1.SelectedIndex = j;
							j = comboAudioTransmit1.Items.Count;
						}
					}
				}				
			}
		}

		private void GetMixerDevices()
		{
			comboAudioMixer1.Items.Clear();
			int num = Mixer.mixerGetNumDevs();
			for(int i=0; i<num; i++)
			{
				comboAudioMixer1.Items.Add(Mixer.GetDevName(i));
			}
			comboAudioMixer1.Items.Add("None");
		}

		private void GetMuxLineNames1()
		{
			if(comboAudioMixer1.SelectedIndex >= 0 &&
				comboAudioMixer1.Items.Count > 0)
			{
				comboAudioReceive1.Items.Clear();
				comboAudioTransmit1.Items.Clear();

				ArrayList a;
				bool good = Mixer.GetMuxLineNames(comboAudioMixer1.SelectedIndex, out a);
				if(good)
				{					
					foreach(string s in a)
					{
						comboAudioReceive1.Items.Add(s);
						comboAudioTransmit1.Items.Add(s);
					}
				}
			}
		}

		private void ForceAllEvents()
		{
			EventArgs e = EventArgs.Empty;

			// General Tab
			comboGeneralLPTAddr_SelectedIndexChanged(this, e);
			udGeneralLPTDelay_ValueChanged(this, e);
			chkGeneralRXOnly_CheckedChanged(this, e);
			chkGeneralUSBPresent_CheckedChanged(this, e);
			chkGeneralRFEPresent_CheckedChanged(this, e);
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

			// Display Tab
			udDisplayGridMax_ValueChanged(this, e);
			udDisplayGridMin_ValueChanged(this, e);
			udDisplayGridStep_ValueChanged(this, e);
			udDisplayFPS_ValueChanged(this, e);
			udDisplayMeterDelay_ValueChanged(this, e);
			udDisplayPeakText_ValueChanged(this, e);
			udDisplayCPUMeter_ValueChanged(this, e);
			udDisplayPhasePts_ValueChanged(this, e);
			udDisplayAVGTime_ValueChanged(this, e);
			udDisplayWaterfallLowLevel_ValueChanged(this, e);
			udDisplayWaterfallHighLevel_ValueChanged(this, e);
			clrbtnWaterfallLow_Changed(this, e);
			clrbtnWaterfallMid_Changed(this, e);
			clrbtnWaterfallHigh_Changed(this, e);
			udDisplayMultiPeakHoldTime_ValueChanged(this, e);
			udDisplayMultiTextHoldTime_ValueChanged(this, e);

			// DSP Tab
			udLMSANF_ValueChanged(this, e);
			udLMSNR_ValueChanged(this, e);
			udDSPImagePhaseRX_ValueChanged(this, e);
			udDSPImageGainRX_ValueChanged(this, e);
			udDSPImagePhaseTX_ValueChanged(this, e);
			udDSPImageGainTX_ValueChanged(this, e);
			udDSPAGCFixedGaindB_ValueChanged(this, e);
			udDSPAGCMaxGaindB_ValueChanged(this, e);
			udDSPCWPitch_ValueChanged(this, e);
			comboDSPWindow_SelectedIndexChanged(this, e);
			udDSPNB_ValueChanged(this, e);
			udDSPNB2_ValueChanged(this, e);

			// Transmit Tab
			udTXFFCompression_ValueChanged(this, e);
			udTXFilterHigh_ValueChanged(this, e);
			udTXFilterLow_ValueChanged(this, e);
			udTransmitTunePower_ValueChanged(this, e);
			udPAGain_ValueChanged(this, e);

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
			clrbtnGrid_Changed(this, e);
			clrbtnZeroLine_Changed(this, e);
			clrbtnFilter_Changed(this, e);
			clrbtnText_Changed(this, e);
			clrbtnDataLine_Changed(this, e);
			udDisplayLineWidth_ValueChanged(this, e);
			clrbtnMeterLeft_Changed(this, e);
			clrbtnMeterRight_Changed(this, e);
		}

		public string[] GetTXProfileStrings()
		{
			string[] s = new string[comboTXProfileName.Items.Count];
			for(int i=0; i<comboTXProfileName.Items.Count; i++)
				s[i] = (string)comboTXProfileName.Items[i];
            return s;
		}

		public string TXProfile
		{
			get 
			{
				if(comboTXProfileName != null) return comboTXProfileName.Text;
				else return "";
			}
			set { if(comboTXProfileName != null) comboTXProfileName.Text = value; }
		}

		public void GetTxProfiles()
		{
			comboTXProfileName.Items.Clear();
			foreach(DataRow dr in DB.dsTxProfile.Tables["TxProfile"].Rows)
			{
				if(dr.RowState != DataRowState.Deleted)
				{
					if(!comboTXProfileName.Items.Contains(dr["Name"]))
						comboTXProfileName.Items.Add(dr["Name"]);
				}
			}
		}

		private bool CheckTXProfileChanged()
		{
			DataRow[] rows = DB.dsTxProfile.Tables["TxProfile"].Select(
				"'"+current_profile+"' = Name");

			if(rows.Length != 1)
				return false;

			int[] eq = console.EQForm.TXEQ;
			if(eq[0] != (int)rows[0]["TXEQPreamp"])
				return true;

			if(console.EQForm.TXEQEnabled != (bool)rows[0]["TXEQEnabled"])
				return true;
				
			for(int i=1; i<eq.Length; i++)
			{
				if(eq[i] != (int)rows[0]["TXEQ"+i.ToString()])
					return true;
			}
			
			if(udTXFilterLow.Value != (int)rows[0]["FilterLow"] ||
				udTXFilterHigh.Value != (int)rows[0]["FilterHigh"] ||
				console.COMP != (bool)rows[0]["CompressorOn"] ||
				udTXFFCompression.Value != (int)rows[0]["CompressorLevel"] ||
				console.CPDR != (bool)rows[0]["CompanderOn"] ||
				udTXCompander.Value != (int)rows[0]["CompanderLevel"] ||
				console.Mic != (int)rows[0]["MicGain"])
				return true;

			return false;
		}

		#endregion

		#region Properties

		public bool X2TR
		{
			get 
			{
				if(chkGeneralEnableX2 != null) return chkGeneralEnableX2.Checked;
				else return false;
			}
			set
			{
				if(chkGeneralEnableX2 != null) chkGeneralEnableX2.Checked = value;
			}
		}

		public int CWPitch
		{
			get
			{
				if(udDSPCWPitch != null) return (int)udDSPCWPitch.Value;
				else return -1;
			}
			set
			{
				if(udDSPCWPitch != null) udDSPCWPitch.Value = value;
			}
		}

		public bool CWDisableMonitor
		{
			get 
			{
				if(chkDSPKeyerDisableMonitor != null) return chkDSPKeyerDisableMonitor.Checked;
				else return false;
			}
			set
			{
				if(chkDSPKeyerDisableMonitor != null) chkDSPKeyerDisableMonitor.Checked = value;
			}
		}

		public bool CWIambic
		{
			get 
			{
				if(chkCWKeyerIambic != null) return chkCWKeyerIambic.Checked;
				else return false;
			}
			set
			{
				if(chkCWKeyerIambic != null) chkCWKeyerIambic.Checked = value;
			}
		}

		public string VACSampleRate
		{
			get
			{
				if(comboAudioSampleRate2 != null) return comboAudioSampleRate2.Text;
				else return "";
			}
			set
			{
				if(comboAudioSampleRate2 != null) comboAudioSampleRate2.Text = value;
			}
		}

		public bool VACStereo
		{
			get 
			{
				if(chkAudio2Stereo != null) return chkAudio2Stereo.Checked;
				else return false;
			}
			set
			{
				if(chkAudio2Stereo != null) chkAudio2Stereo.Checked = value;
			}
		}

		public bool SpurReduction
		{
			get
			{
				if(chkGeneralSpurRed != null) return chkGeneralSpurRed.Checked;
				else return true;
			}
			set
			{
				if(chkGeneralSpurRed != null) chkGeneralSpurRed.Checked = value;
			}
		}

		public int COMPVal
		{
			get
			{
				if(udTXFFCompression != null) return (int)udTXFFCompression.Value;
				else return -1;
			}
			set
			{
				if(udTXFFCompression != null) udTXFFCompression.Value = value;
			}
		}

		public int CPDRVal
		{
			get
			{
				if(udTXCompander != null) return (int)udTXCompander.Value;
				else return -1;
			}
			set
			{
				if(udTXCompander != null) udTXCompander.Value = value;
			}
		}

		public int NoiseGate
		{
			get
			{
				if(udTXNoiseGate != null) return (int)udTXNoiseGate.Value;
				else return -1;
			}
			set
			{
				if(udTXNoiseGate != null) udTXNoiseGate.Value = value;
			}
		}

		public int VOXSens
		{
			get 
			{
				if(udTXVOXThreshold != null) return (int)udTXVOXThreshold.Value;
				else return -1;
			}
			set
			{
				if(udTXVOXThreshold != null) udTXVOXThreshold.Value = value; 
			}
		}

		public bool NoiseGateEnabled
		{
			get 
			{
				if(chkTXNoiseGateEnabled != null) return chkTXNoiseGateEnabled.Checked;
				else return false;
			}
			set
			{
				if(chkTXNoiseGateEnabled != null) chkTXNoiseGateEnabled.Checked = value;
			}
		}

		public int VACRXGain
		{
			get
			{
				if(udAudioVACGainRX != null) return (int)udAudioVACGainRX.Value;
				else return -99;
			}
			set
			{
				if(udAudioVACGainRX != null) udAudioVACGainRX.Value = value;
			}
		}

		public int VACTXGain
		{
			get
			{
				if(udAudioVACGainTX != null) return (int)udAudioVACGainTX.Value;
				else return -99;
			}
			set
			{
				if(udAudioVACGainTX != null) udAudioVACGainTX.Value = value;
			}
		}

		public bool BreakInEnabled
		{
			get 
			{
				if(chkDSPKeyerSemiBreakInEnabled != null)
					return chkDSPKeyerSemiBreakInEnabled.Checked;
				else return false;
			}
			set
			{
				if(chkDSPKeyerSemiBreakInEnabled != null)
					chkDSPKeyerSemiBreakInEnabled.Checked = value;
			}
		}

		private SoundCard current_sound_card = SoundCard.UNSUPPORTED_CARD;
		public SoundCard CurrentSoundCard
		{
			get { return current_sound_card; }
			set
			{
				current_sound_card = value;
				switch(value)
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
				}
			}
		}

		public bool VOXEnable
		{
			get
			{
				if(chkTXVOXEnabled != null) return chkTXVOXEnabled.Checked;
				else return false;
			}
			set
			{
				if(chkTXVOXEnabled != null) chkTXVOXEnabled.Checked = value;
			}
		}

		public int AGCMaxGain
		{
			get
			{
				if(udDSPAGCMaxGaindB != null) return (int)udDSPAGCMaxGaindB.Value;
				else return -1;
			}
			set
			{
				if(udDSPAGCMaxGaindB != null) udDSPAGCMaxGaindB.Value = value;
			}
		}

		public int AGCFixedGain
		{
			get
			{
				if(udDSPAGCFixedGaindB != null) return (int)udDSPAGCFixedGaindB.Value;
				else return -1;
			}
			set
			{
				if(udDSPAGCFixedGaindB != null) udDSPAGCFixedGaindB.Value = value;
			}
		}

		public int TXFilterHigh
		{
			get { return (int)udTXFilterHigh.Value; }
			set { udTXFilterHigh.Value = value; }
		}

		public int TXFilterLow
		{
			get { return (int)udTXFilterLow.Value; }
			set { udTXFilterLow.Value = value; }
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

				if(value)
				{
					udDSPAGCAttack_ValueChanged(this, EventArgs.Empty);
					udDSPAGCDecay_ValueChanged(this, EventArgs.Empty);
					udDSPAGCHangTime_ValueChanged(this, EventArgs.Empty);
				}
			}
		}

		public bool DirectX
		{
			set
			{
				if(value)
				{
					if(!comboDisplayDriver.Items.Contains("DirectX"))
						comboDisplayDriver.Items.Add("DirectX");
				}
				else
				{
					if(comboDisplayDriver.Items.Contains("DirectX"))
					{
						comboDisplayDriver.Items.Remove("DirectX");
						if(comboDisplayDriver.SelectedIndex < 0)
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
				switch(value)
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
				}
			}
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
				grpGeneralHardwareSetup.Enabled = !mox;
				if(comboAudioSoundCard.SelectedIndex == (int)SoundCard.UNSUPPORTED_CARD)
					grpAudioDetails1.Enabled = !mox;
				grpAudioCard.Enabled = !mox;
				grpAudioBufferSize1.Enabled = !mox;
				grpAudioVolts1.Enabled = !mox;
				grpAudioLatency1.Enabled = !mox;
				chkAudioEnableVAC.Enabled = !mox;
				if(chkAudioEnableVAC.Checked)
				{
					grpAudioDetails2.Enabled = !mox;
					grpAudioBuffer2.Enabled = !mox;
					grpAudioLatency2.Enabled = !mox;
					grpAudioSampleRate2.Enabled = !mox;
					grpAudio2Stereo.Enabled = !mox;
				}
				grpDSPBufferSize.Enabled = !mox;
				grpTestAudioBalance.Enabled = !mox;
				if(!mox && !chekTestIMD.Checked && !chkGeneralRXOnly.Checked)
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

		public bool RFEPresent
		{
			get { return chkGeneralRFEPresent.Checked; }
			set	{ chkGeneralRFEPresent.Checked = value;	}
		}

		public bool USBPresent
		{
			get { return chkGeneralUSBPresent.Checked; }
			set	{ chkGeneralUSBPresent.Checked = value; }
		}

		public bool OzyControl  
		{
			get { return chkBoxJanusOzyControl.Checked; }
			set	{ chkBoxJanusOzyControl.Checked = value; }
		}

		public bool XVTRPresent
		{
			get { return chkGeneralXVTRPresent.Checked; }
			set	{ chkGeneralXVTRPresent.Checked = value; }
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
			set	{ chkGeneralATUPresent.Checked = value; }
		}

		public bool SpurRedEnabled
		{
			get { return chkGeneralSpurRed.Enabled; }
			set { chkGeneralSpurRed.Enabled = value; }
		}

		public int PllMult
		{
			get { return (int)udDDSPLLMult.Value; }
			set	{ udDDSPLLMult.Value = value; }
		}

		public int ClockOffset
		{
			get { return (int)udDDSCorrection.Value; }
			set { udDDSCorrection.Value = value; }
		}

		public double ImageGainRX
		{
			get { return (double)udDSPImageGainRX.Value; }
			set	{ udDSPImageGainRX.Value = (decimal)value; }
		}

		public double ImagePhaseRX
		{
			get { return (double)udDSPImagePhaseRX.Value; }
			set	{ udDSPImagePhaseRX.Value = (decimal)value;	}
		}



		public double ImageGainTX
		{
			get { return (double)udDSPImageGainTX.Value; }
			set	{ udDSPImageGainTX.Value = (decimal)value; }
		}

		public double ImagePhaseTX
		{
			get { return (double)udDSPImagePhaseTX.Value; }
			set	{ udDSPImagePhaseTX.Value = (decimal)value;	}
		}

		public float PAGain160
		{
			get { return (float)udPAGain160.Value; }
			set	{ udPAGain160.Value = (decimal)value; }
		}

		public float PAGain80
		{
			get { return (float)udPAGain80.Value; }
			set	{ udPAGain80.Value = (decimal)value; }
		}

		public float PAGain60
		{
			get { return (float)udPAGain60.Value; }
			set	{ udPAGain60.Value = (decimal)value; }
		}

		public float PAGain40
		{
			get { return (float)udPAGain40.Value; }
			set	{ udPAGain40.Value = (decimal)value; }
		}

		public float PAGain30
		{
			get { return (float)udPAGain30.Value; }
			set	{ udPAGain30.Value = (decimal)value; }
		}

		public float PAGain20
		{
			get { return (float)udPAGain20.Value; }
			set { udPAGain20.Value = (decimal)value; }
		}

		public float PAGain17
		{
			get { return (float)udPAGain17.Value; }
			set	{ udPAGain17.Value = (decimal)value; }
		}

		public float PAGain15
		{
			get { return (float)udPAGain15.Value; }
			set	{ udPAGain15.Value = (decimal)value; }
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

		public float PAADC160
		{
			get { return (float)udPAADC160.Value; }
			set { udPAADC160.Value = (decimal)value; }
		}

		public float PAADC80
		{
			get { return (float)udPAADC80.Value; }
			set	{ udPAADC80.Value = (decimal)value; }
		}

		public float PAADC60
		{
			get { return (float)udPAADC60.Value; }
			set { udPAADC60.Value = (decimal)value;	}
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

		// Added 06/21/05 BT for CAT commands

		public int CATNB1Threshold
		{
			get{ return Convert.ToInt32(udDSPNB.Value); }
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
			get{return Convert.ToInt32(udDSPNB2.Value);}
			set
			{
				value = (int)Math.Max(udDSPNB2.Minimum, value);
				value = (int)Math.Min(udDSPNB2.Maximum, value);
				udDSPNB2.Value = value;
			}
		}

		// Added 06/21/05 BT for CAT commands
		public int CATCompThreshold
		{
			get{return Convert.ToInt32(udTXFFCompression.Value);}
			set
			{
				value = (int)Math.Max(udTXFFCompression.Minimum, value);
				value = (int)Math.Min(udTXFFCompression.Maximum, value);
				udTXFFCompression.Value = value;
			}
		}

		// Added 06/30/05 BT for CAT commands
		public int CATCWPitch
		{
			get{return (int) udDSPCWPitch.Value;}
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

		public int DSPBufferSize
		{
			get { return Int32.Parse(comboDSPBufSize.Text); }
			set
			{
				string temp = value.ToString();
				if(comboDSPBufSize.Items.Contains(temp))
					comboDSPBufSize.SelectedItem = temp;
			}
		}

		#endregion

		#region General Tab Event Handlers
		// ======================================================
		// General Tab Event Handlers
		// ======================================================

		private void radGenModelSDR1000_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radGenModelSDR1000.Checked)
			{
				console.CurrentModel = Model.SDR1000;
				if(radGenModelSDR1000.Focused || force_model)
				{
					chkGeneralRXOnly.Checked = false;
					chkGeneralDisablePTT.Checked = false;
					force_model = false;
				}
				chkGeneralRXOnly.Enabled = true;
			}
		}

		private void radGenModelSoftRock40_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radGenModelSoftRock40.Checked)
			{
				chkGeneralDisablePTT.Checked = true;
				console.CurrentModel = Model.SOFTROCK40;
				if(radGenModelSoftRock40.Focused || force_model)
				{
					chkGeneralRXOnly.Checked = true;
					chkGeneralDisablePTT.Checked = true;
					force_model = false;
				}
				chkGeneralRXOnly.Enabled = false;
			}
			grpHWSoftRock.Visible = radGenModelSoftRock40.Checked;
		}

		private void radGenModelDemoNone_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radGenModelDemoNone.Checked)
			{
				console.CurrentModel = Model.DEMO;
				if(radGenModelDemoNone.Focused || force_model)
				{
					chkGeneralRXOnly.Checked = true;
					chkGeneralDisablePTT.Checked = true;
					MessageBox.Show("Welcome to the Demo/Test Drive mode of the PowerSDR software.\n"+
						"You can download wave files from our website at http://www.flex-radio.com/downloads.htm#SDR-1000_wave_files.\n"+
						"For information about how to use the Wave file player, please see page 125 in our Operating Manual\n"+
						"at http://www.flex-radio.com/downloads/SDR-1000_Operating_Manual.pdf.\n"+
						"\nPlease contact us at support@flex-radio.com or call (512) 250-8595 with any questions.",
						"Demo/Test Drive Mode Welcome",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information);
					force_model = false;
				}
				chkGeneralRXOnly.Enabled = true;
			}
		}

		private void udSoftRockCenterFreq_ValueChanged(object sender, System.EventArgs e)
		{
			console.SoftRockCenterFreq = (double)udSoftRockCenterFreq.Value;
		}

		private void comboGeneralLPTAddr_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(comboGeneralLPTAddr.Text == "")
				return;
			console.Hdw.LPTAddr = Convert.ToUInt16(comboGeneralLPTAddr.Text, 16);
		}

		private void comboGeneralLPTAddr_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyData == Keys.Enter)
			{
				if(comboGeneralLPTAddr.Text.Length > 4)
				{
					MessageBox.Show("Invalid Parallel Port Address ("+comboGeneralLPTAddr.Text+")");
					comboGeneralLPTAddr.Text = "378";
					return;
				}

				foreach(Char c in comboGeneralLPTAddr.Text)
				{
					if(!Char.IsDigit(c) &&
						Char.ToLower(c) < 'a' &&
						Char.ToLower(c) > 'f')
					{
						MessageBox.Show("Invalid Parallel Port Address ("+comboGeneralLPTAddr.Text+")");
						comboGeneralLPTAddr.Text = "378";
						return;
					}
				}

				console.Hdw.LPTAddr = Convert.ToUInt16(comboGeneralLPTAddr.Text, 16);
			}
					
		}

		private void comboGeneralLPTAddr_LostFocus(object sender, System.EventArgs e)
		{
			comboGeneralLPTAddr_KeyDown(sender, new KeyEventArgs(Keys.Enter));
		}
		
		private void chkGeneralRXOnly_CheckedChanged(object sender, System.EventArgs e)
		{
			if(comboAudioSoundCard.Text == "Unsupported Card" && !chkGeneralRXOnly.Checked)
			{
				DialogResult dr = MessageBox.Show(
					"Unchecking Receive Only while in Unsupported Card mode may \n"+
					"cause damage to your SDR-1000 hardware.  Are you sure you want \n"+
					"to enable transmit?",
					"Warning: Enable Transmit?",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning);
				if(dr == DialogResult.No)
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
				if(chkGeneralUSBPresent.Checked)
				{
					if(!USB.Init(chkGeneralRFEPresent.Checked, chkGeneralPAPresent.Checked))
						chkGeneralUSBPresent.Checked = false;
					else USB.Console = console;
				}
				else
					USB.Exit();
				
				if(console.PowerOn)
				{
					console.PowerOn = false;
					Thread.Sleep(100);
					console.PowerOn = true;
				}				
			}
			catch(Exception)
			{
				MessageBox.Show("A required DLL was not found (Sdr1kUsb.dll).  Please download the\n"+
					"installer from the FlexRadio private download page and try again.",
					"Error: Missing DLL",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				chkGeneralUSBPresent.Checked = false;
			}
		}

		private void chkGeneralRFEPresent_CheckedChanged(object sender, System.EventArgs e)
		{
			chkGeneralXVTRPresent.Visible = chkGeneralRFEPresent.Checked;
			chkGeneralPAPresent.Visible = chkGeneralRFEPresent.Checked;
			console.RFEPresent = chkGeneralRFEPresent.Checked;
			if(chkGeneralRFEPresent.Checked == false)
			{
				chkGeneralXVTRPresent.Checked = false;
				chkGeneralPAPresent.Checked = false;
			}

			if(chkGeneralUSBPresent.Checked)
			{
				chkGeneralUSBPresent.Checked = false;
				chkGeneralUSBPresent.Checked = true;
			}
		}

		private void chkGeneralPAPresent_CheckedChanged(object sender, System.EventArgs e)
		{
			console.PAPresent = chkGeneralPAPresent.Checked;
			chkGeneralATUPresent.Visible = chkGeneralPAPresent.Checked;
			grpPAGainByBand.Visible = chkGeneralPAPresent.Checked;

			if(!chkGeneralPAPresent.Checked)
				chkGeneralATUPresent.Checked = false;
			else if(console.PowerOn)
			{
				console.PowerOn = false;
				Thread.Sleep(100);
				console.PowerOn = true;
			}

			if(chkGeneralUSBPresent.Checked)
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
			if(chkGeneralXVTRPresent.Checked)
			{
				if(comboGeneralXVTR.SelectedIndex == (int)XVTRTRMode.POSITIVE)
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
			console.IFFreq = (double)udDDSIFFreq.Value / 1000000;
		}

		private void btnGeneralCalFreqStart_Click(object sender, System.EventArgs e)
		{
			Thread t = new Thread(new ThreadStart(CalibrateFreq));
			t.Name = "Freq Calibration Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.AboveNormal;
			t.Start();
		}

		private void btnGeneralCalLevelStart_Click(object sender, System.EventArgs e)
		{
			progress = new Progress("Calibrate RX Level");

			Thread t = new Thread(new ThreadStart(CalibrateLevel));
			t.Name = "Level Calibration Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.AboveNormal;
			t.Start();

			if(console.PowerOn)
				progress.Show();
		}

		private void btnGeneralCalImageStart_Click(object sender, System.EventArgs e)
		{
			progress = new Progress("Calibrate Image Rejection");

			Thread t = new Thread(new ThreadStart(CalibrateImage));
			t.Name = "Image Calibration Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.AboveNormal;
			t.Start();

			if(console.PowerOn)
				progress.Show();
		}

		private void CalibrateFreq()
		{
			bool done = console.CalibrateFreq((float)udGeneralCalFreq1.Value);
			if(done) MessageBox.Show("Frequency Calibration complete.");
		}

		private void CalibrateLevel()
		{
			bool done = console.CalibrateLevel(
				(float)udGeneralCalLevel.Value,
				(float)udGeneralCalFreq2.Value,
				progress);
			if(done) MessageBox.Show("Level Calibration complete.");
		}

		private void CalibrateImage()
		{
			bool done = console.CalibrateImage((float)udGeneralCalFreq3.Value, progress);
			if(done) MessageBox.Show("Image Rejection Calibration complete.");
		}

		private void chkGeneralDisablePTT_CheckedChanged(object sender, System.EventArgs e)
		{
			console.DisablePTT = chkGeneralDisablePTT.Checked;
		}

		private void comboGeneralXVTR_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch(comboGeneralXVTR.SelectedIndex)
			{
				case (int)XVTRTRMode.NEGATIVE:
					if(comboGeneralXVTR.Focused)
					{
						MessageBox.Show("The default TR Mode for the DEMI144-28FRS sold by FlexRadio Systems is\n"+
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

			if(comboGeneralProcessPriority.Text == "Real Time" &&
				comboGeneralProcessPriority.Focused)
			{
				DialogResult dr = MessageBox.Show(
					"Setting the Process Priority to Realtime can cause the system to become unresponsive.\n"+
					"This setting is not recommended.\n"+
					"Are you sure you want to change to Realtime?",
					"Warning: Realtime Not Recommended",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning);
				if(dr == DialogResult.No)
				{
					switch(p.PriorityClass)
					{
						case ProcessPriorityClass.Idle:
							comboGeneralProcessPriority.Text = "Idle";
							break;
						case ProcessPriorityClass.BelowNormal:
							comboGeneralProcessPriority.Text = "Below Normal";
							break;
						case ProcessPriorityClass.Normal:
							comboGeneralProcessPriority.Text = "Normal";
							break;
						case ProcessPriorityClass.AboveNormal:
							comboGeneralProcessPriority.Text = "Above Normal";
							break;
						case ProcessPriorityClass.High:
							comboGeneralProcessPriority.Text = "Highest";
							break;
					}
					return;
				}
			}
			
			switch(comboGeneralProcessPriority.Text)
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
			switch(comboOptFilterWidthMode.Text)
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
			bool power = console.PowerOn;
			if(power)
			{
				console.PowerOn = false;
				Thread.Sleep(100);
			}
			console.AudioDriverIndex1 = ((PADeviceInfo)comboAudioDriver1.SelectedItem).Index;
			Audio.Host1 = ((PADeviceInfo)comboAudioDriver1.SelectedItem).Index;
			GetDevices1();
			if(comboAudioInput1.Items.Count != 0)
				comboAudioInput1.SelectedIndex = 0;
			if(comboAudioOutput1.Items.Count != 0)
				comboAudioOutput1.SelectedIndex = 0;
			if(power) console.PowerOn = true;
		}

		private void comboAudioInput1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			bool power = console.PowerOn;
			if(power)
			{
				console.PowerOn = false;
				Thread.Sleep(100);
			}

			int index = ((PADeviceInfo)comboAudioInput1.SelectedItem).Index;
			console.AudioInputIndex1 = index;
			Audio.Input1 = index;
			if(comboAudioInput1.SelectedIndex == 0 &&
				comboAudioDriver1.SelectedIndex < 2)
			{
				comboAudioMixer1.SelectedIndex = 0;
			}
			else
			{
				for(int i=0; i<comboAudioMixer1.Items.Count; i++)
				{
					string s = (string)comboAudioMixer1.Items[i];
					if(s.StartsWith(comboAudioInput1.Text.Substring(0, 5)))
						comboAudioMixer1.Text = s;
				}
				comboAudioMixer1.Text = comboAudioInput1.Text;
			}

			if(power) console.PowerOn = true;
		}

		private void comboAudioOutput1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			bool power = console.PowerOn;
			if(power)
			{
				console.PowerOn = false;
				Thread.Sleep(100);
			}

			int index = ((PADeviceInfo)comboAudioOutput1.SelectedItem).Index;
			console.AudioOutputIndex1 = index;
			Audio.Output1 = index;

			if(power) console.PowerOn = true;
		}

		private void comboAudioMixer1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			UpdateMixerControls1();
			console.MixerID1 = comboAudioMixer1.SelectedIndex;
		}	

		private void comboAudioReceive1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			console.MixerRXMuxID1 = comboAudioReceive1.SelectedIndex;
			if(!initializing && console.PowerOn)
				Mixer.SetMux(comboAudioMixer1.SelectedIndex, comboAudioReceive1.SelectedIndex);
		}

		private void comboAudioTransmit1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			console.MixerTXMuxID1 = comboAudioTransmit1.SelectedIndex;
		}

		private void chkAudioEnableVAC_CheckedChanged(object sender, System.EventArgs e)
		{
			bool val = chkAudioEnableVAC.Checked;

			if(val)
			{
				if(comboAudioDriver2.SelectedIndex < 0 && 
					comboAudioDriver2.Items.Count > 0)
					comboAudioDriver2.SelectedIndex = 0;
			}

			bool power = console.PowerOn;
			if(power)
			{
				console.PowerOn = false;
				Thread.Sleep(100);
			}
				
			console.VACEnabled = val;
			if(power)console.PowerOn = true;
		}

		private void comboAudioChannels1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			bool power = console.PowerOn;
			if(chkAudioEnableVAC.Checked && power) 
			{
				console.PowerOn = false;
				Thread.Sleep(100);
			}
			if(comboAudioChannels1.SelectedIndex < 0) return;
			console.NumChannels = Int32.Parse(comboAudioChannels1.Text);
			Audio.NumChannels = Int32.Parse(comboAudioChannels1.Text);
			if(chkAudioEnableVAC.Checked && power)
				console.PowerOn = true;
		}

		private void comboAudioDriver2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			bool power = console.PowerOn;
			if(chkAudioEnableVAC.Checked && power) 
			{
				console.PowerOn = false;
				Thread.Sleep(100);
			}
			console.AudioDriverIndex2 = ((PADeviceInfo)comboAudioDriver2.SelectedItem).Index;
			Audio.Host2 = ((PADeviceInfo)comboAudioDriver2.SelectedItem).Index;
			GetDevices2();
			if(comboAudioInput2.Items.Count != 0)
				comboAudioInput2.SelectedIndex = 0;
			if(comboAudioOutput2.Items.Count != 0)
				comboAudioOutput2.SelectedIndex = 0;
			if(chkAudioEnableVAC.Checked && power) 
				console.PowerOn = true;
		}

		private void comboAudioInput2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			bool power = console.PowerOn;
			if(chkAudioEnableVAC.Checked && power) 
			{
				console.PowerOn = false;
				Thread.Sleep(100);
			}
			int index = ((PADeviceInfo)comboAudioInput2.SelectedItem).Index;
			console.AudioInputIndex2 = index;
			Audio.Input2 = index;
			if(chkAudioEnableVAC.Checked && power) 
				console.PowerOn = true;
		}

		private void comboAudioOutput2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			bool power = console.PowerOn;
			if(chkAudioEnableVAC.Checked && power) 
			{
				console.PowerOn = false;
				Thread.Sleep(100);
			}
			int index = ((PADeviceInfo)comboAudioOutput2.SelectedItem).Index;
			console.AudioOutputIndex2 = index;
			Audio.Output2 = index;

			if(chkAudioEnableVAC.Checked && power) 
				console.PowerOn = true;
		}

		private void comboAudioSampleRate1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int old_rate = console.SampleRate1;
			int new_rate = Int32.Parse(comboAudioSampleRate1.Text);
			bool power = console.PowerOn;
			if(power && new_rate != old_rate)
			{
				console.PowerOn = false;
				Thread.Sleep(100);
			}
			DttSP.SampleRate = new_rate;
			console.SampleRate1 = new_rate;			
			Display.DrawBackground();
			DttSP.SetKeyerSampleRate(new_rate);
			DttSP.SetKeyerFreq((float)console.CWPitch);
			console.SoftRockCenterFreq  = console.SoftRockCenterFreq; // warning -- this appears to do nothing - not true, these are
			                                                          // properties and the assignment is needed due to side effects!   
			                                                          // We need the soft rock  code to recalc  its tuning limits -- 
			                                                          // setting the center freq does this as a side effect
			                                                          
			if(power && new_rate != old_rate)
				console.PowerOn = true;			
		}

		private void comboAudioSampleRate2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int old_rate = console.SampleRate2;
			int new_rate = Int32.Parse(comboAudioSampleRate2.Text);
			bool poweron = console.PowerOn;
			if(console.SetupForm != null && new_rate != old_rate && poweron)
				console.PowerOn = false;
			Thread.Sleep(100);
			console.SampleRate2 = new_rate;
			console.VACSampleRate = comboAudioSampleRate2.Text;

			if(console.SetupForm != null && new_rate != old_rate && poweron)
				console.PowerOn = true;	
		}
	
		private void comboAudioBuffer1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			bool power = console.PowerOn;
			if(power)
			{
				console.PowerOn = false;
				Thread.Sleep(100);
			}

			console.BlockSize1 = Int32.Parse(comboAudioBuffer1.Text);
			DttSP.SetAudioSize(console.BlockSize1);
			DttSP.SetKeyerResetSize(3*console.BlockSize1/2);
			
			if(power) console.PowerOn = true;
		}

		private void comboAudioBuffer2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			bool power = console.PowerOn;
			if(power && chkAudioEnableVAC.Checked)
			{
				console.PowerOn = false;
				Thread.Sleep(100);
			}

			console.BlockSize2 = Int32.Parse(comboAudioBuffer2.Text);
			
			if(power && chkAudioEnableVAC.Checked)
				console.PowerOn = true;
		}

		private void udAudioLatency1_ValueChanged(object sender, System.EventArgs e)
		{
			bool power = console.PowerOn;
			if(power)
			{
				console.PowerOn = false;
				Thread.Sleep(100);
			}

			console.AudioLatency1 = (int)udAudioLatency1.Value;
			Audio.Latency1 = (int)udAudioLatency1.Value;
			
			if(power) console.PowerOn = true;
		}

		private void udAudioLatency2_ValueChanged(object sender, System.EventArgs e)
		{
			bool power = console.PowerOn;
			if(power && chkAudioEnableVAC.Checked)
			{
				console.PowerOn = false;
				Thread.Sleep(100);
			}

			console.AudioLatency2 = (int)udAudioLatency2.Value;
			Audio.Latency2 = (int)udAudioLatency2.Value;

			if(power && chkAudioEnableVAC.Checked)
				console.PowerOn = true;
		}

		private void udAudioVoltage1_ValueChanged(object sender, System.EventArgs e)
		{
			if(udAudioVoltage1.Focused &&
				comboAudioSoundCard.SelectedIndex > 0 &&
				current_sound_card != SoundCard.UNSUPPORTED_CARD)
			{
				DialogResult dr = MessageBox.Show("Are you sure you want to change the Max RMS Voltage for this \n"+
					"supported sound card?  The largest measured difference in supported cards \n"+
					"was 40mV.  Note that we will only allow a 100mV difference from our measured default.",
					"Change Voltage?",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning);
				if(dr == DialogResult.No)
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
			if(power && chkAudioEnableVAC.Checked)
			{
				console.PowerOn = false;
				Thread.Sleep(100);
			}

			console.SecondSoundCardStereo = chkAudio2Stereo.Checked;
			console.VACStereo = chkAudio2Stereo.Checked;

			if(power && chkAudioEnableVAC.Checked)
				console.PowerOn = true;
		}

		private void udAudioVACGainRX_ValueChanged(object sender, System.EventArgs e)
		{
			Audio.VACRXScale = Math.Pow(10.0, (int)udAudioVACGainRX.Value/20.0);
			console.VACRXGain = (int)udAudioVACGainRX.Value;
		}

		private void udAudioVACGainTX_ValueChanged(object sender, System.EventArgs e)
		{
			Audio.VACPreamp = Math.Pow(10.0, (int)udAudioVACGainTX.Value/20.0);
			console.VACTXGain = (int)udAudioVACGainTX.Value;
		}

		private void chkAudioVACAutoEnable_CheckedChanged(object sender, System.EventArgs e)
		{
			console.VACAutoEnable = chkAudioVACAutoEnable.Checked;
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
			if(power)
			{
				console.PowerOn = false;
				Thread.Sleep(100);
			}

			udAudioLatency1.Enabled = chkAudioLatencyManual1.Checked;

			if(!chkAudioLatencyManual1.Checked)
				console.AudioLatency1 = 0;

			if(power) console.PowerOn = true;
		}

		private void chkAudioLatencyManual2_CheckedChanged(object sender, System.EventArgs e)
		{
			bool power = console.PowerOn;
			if(power && chkAudioEnableVAC.Checked)
			{
				console.PowerOn = false;
				Thread.Sleep(100);
			}

			udAudioLatency2.Enabled = chkAudioLatencyManual2.Checked;

			if(!chkAudioLatencyManual2.Checked)
				console.AudioLatency2 = 0;

			if(power && chkAudioEnableVAC.Checked)
				console.PowerOn = true;
		}

		private void chkAudioMicBoost_CheckedChanged(object sender, System.EventArgs e)
		{
			console.MicBoost = chkAudioMicBoost.Checked;
		}

		private void btnAudioVoltTest1_Click(object sender, System.EventArgs e)
		{
			sound_card = 1;

			progress = new Progress("Calibrate Sound Card");
			if(console.PowerOn)
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
			if(done) MessageBox.Show("Sound Card Calibration complete.");
		}

		private void FireBoxMixerFix()
		{
			try
			{
				Process p = Process.Start("c:\\Program Files\\PreSonus\\1394AudioDriver_FIREBox\\FireBox Mixer.exe");
				Thread.Sleep(2000);
				p.Kill();
			}
			catch(Exception)
			{

			}
		}

		private void comboAudioSoundCard_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			bool on = console.PowerOn;
			if(on)
			{
				console.PowerOn = false;
				Thread.Sleep(50);
			}

			SoundCard card = SoundCard.FIRST;
			switch(comboAudioSoundCard.Text)
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
				case "HPSDR Janus/Ozy (USB2)":
					card = SoundCard.JANUS_OZY; 
					break; 
				case "Unsupported Card":
					card = SoundCard.UNSUPPORTED_CARD;
					break;
			}

			if(card == SoundCard.FIRST) return;
			
			console.CurrentSoundCard = card;
			current_sound_card = card;

			switch(card)
			{
				case SoundCard.SANTA_CRUZ:
					grpAudioDetails1.Enabled = false;
					grpAudioVolts1.Visible = false;
					udAudioVoltage1.Value = 1.274M;					
					if(comboAudioSampleRate1.Items.Contains(96000))
						comboAudioSampleRate1.Items.Remove(96000);
					if(comboAudioSampleRate1.Items.Contains(192000))
						comboAudioSampleRate1.Items.Remove(192000);
					comboAudioSampleRate1.Text = "48000";
					foreach(PADeviceInfo p in comboAudioDriver1.Items)
					{
						if(p.Name == "ASIO")
						{
							comboAudioDriver1.SelectedItem = p;
							break;
						}
					}

					foreach(PADeviceInfo dev in comboAudioInput1.Items)
					{
						if(dev.Name == "Wuschel's ASIO4ALL")
						{
							comboAudioInput1.Text = "Wuschel's ASIO4ALL";
							comboAudioOutput1.Text = "Wuschel's ASIO4ALL";
						}
					}
					if(comboAudioInput1.Text != "Wuschel's ASIO4ALL")
					{
						foreach(PADeviceInfo dev in comboAudioInput1.Items)
						{
							if(dev.Name == "ASIO4ALL v2")
							{
								comboAudioInput1.Text = "ASIO4ALL v2";
								comboAudioOutput1.Text = "ASIO4ALL v2";
							}
						}
					}

					comboAudioMixer1.Text = "Santa Cruz(tm)";
					comboAudioReceive1.Text = "Line In";
					
					for(int i=0; i<comboAudioTransmit1.Items.Count; i++)
					{
						if(((string)comboAudioTransmit1.Items[i]).StartsWith("Mi"))
						{
							comboAudioTransmit1.SelectedIndex = i;
							break;
						}
					}

					if(comboAudioMixer1.SelectedIndex < 0 || 
						comboAudioMixer1.Text != "Santa Cruz(tm)")
					{
						MessageBox.Show(comboAudioSoundCard.Text+" not found.\n "+
							"Please verify that this specific sound card is installed " +
							"and functioning and try again.  \nIf your sound card is not " +
							"a "+comboAudioSoundCard.Text+" and your card is not in the "+
							"list, use the Unsupported Card selection.  \nFor more support, "+
							"email support@flex-radio.com.",
							comboAudioSoundCard.Text+" Not Found",
							MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation);
						console.PowerEnabled = false;
					}
					else if(!Mixer.InitSantaCruz(console.MixerID1))
					{
						MessageBox.Show("The "+comboAudioSoundCard.Text+" mixer initialization "+
							"failed.  Please install the latest drivers from www.turtlebeach.com " +
							" and try again.  For more support, email support@flex-radio.com.",
							comboAudioSoundCard.Text+" Mixer Initialization Failed",
							MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation);
						console.PowerEnabled = false;
					}
					else if(comboAudioInput1.Text != "ASIO4ALL v2" &&
						comboAudioInput1.Text != "Wuschel's ASIO4ALL")
					{
						MessageBox.Show("ASIO4ALL driver not found.  Please visit " +
							"www.asio4all.com, download and install the driver, "+
							"and try again.  Alternatively, you can use the Unsupported "+
							"Card selection and setup the sound interface manually.  For "+
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
					}
					break;
				case SoundCard.AUDIGY:
				case SoundCard.AUDIGY_2:
					grpAudioDetails1.Enabled = false;
					grpAudioVolts1.Visible = false;
					udAudioVoltage1.Value = 2.23M;
					if(comboAudioSampleRate1.Items.Contains(96000))
						comboAudioSampleRate1.Items.Remove(96000);
					if(comboAudioSampleRate1.Items.Contains(192000))
						comboAudioSampleRate1.Items.Remove(192000);
					comboAudioSampleRate1.Text = "48000";
					foreach(PADeviceInfo p in comboAudioDriver1.Items)
					{
						if(p.Name == "ASIO")
						{
							comboAudioDriver1.SelectedItem = p;
							break;
						}
					}
					
					foreach(PADeviceInfo dev in comboAudioInput1.Items)
					{
						if(dev.Name == "Wuschel's ASIO4ALL")
						{
							comboAudioInput1.Text = "Wuschel's ASIO4ALL";
							comboAudioOutput1.Text = "Wuschel's ASIO4ALL";
						}
					}
					if(comboAudioInput1.Text != "Wuschel's ASIO4ALL")
					{
						foreach(PADeviceInfo dev in comboAudioInput1.Items)
						{
							if(dev.Name == "ASIO4ALL v2")
							{
								comboAudioInput1.Text = "ASIO4ALL v2";
								comboAudioOutput1.Text = "ASIO4ALL v2";
							}
						}
					}

					for(int i=0; i<comboAudioMixer1.Items.Count; i++)
					{
						if(((string)comboAudioMixer1.Items[i]).StartsWith("SB Audigy"))
						{
							comboAudioMixer1.SelectedIndex = i;
							break;
						}
					}

					for(int i=0; i<comboAudioReceive1.Items.Count; i++)
					{
						if(((string)comboAudioReceive1.Items[i]).StartsWith("Analog"))
						{
							comboAudioReceive1.SelectedIndex = i;
							break;
						}
					}

					if(comboAudioReceive1.SelectedIndex < 0 ||
						!comboAudioReceive1.Text.StartsWith("Analog"))
					{
						for(int i=0; i<comboAudioReceive1.Items.Count; i++)
						{
							if(((string)comboAudioReceive1.Items[i]).StartsWith("Mix ana"))
							{
								comboAudioReceive1.SelectedIndex = i;
								break;
							}
						}
					}

					for(int i=0; i<comboAudioTransmit1.Items.Count; i++)
					{
						if(((string)comboAudioTransmit1.Items[i]).StartsWith("Mi"))
						{
							comboAudioTransmit1.SelectedIndex = i;
							break;
						}
					}

					if(comboAudioMixer1.SelectedIndex < 0 ||
						!comboAudioMixer1.Text.StartsWith("SB Audigy"))
					{
						MessageBox.Show(comboAudioSoundCard.Text+" not found.\n "+
							"Please verify that this specific sound card is installed " +
							"and functioning and try again.  \nIf your sound card is not " +
							"a "+comboAudioSoundCard.Text+" and your card is not in the "+
							"list, use the Unsupported Card selection.  \nFor more support, "+
							"email support@flex-radio.com.",
							comboAudioSoundCard.Text+" Not Found",
							MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation);
						console.PowerEnabled = false;
					}
					else if(!Mixer.InitAudigy2(console.MixerID1))
					{
						MessageBox.Show("The "+comboAudioSoundCard.Text+" mixer initialization "+
							"failed.  Please install the latest drivers from www.creativelabs.com " +
							" and try again.  For more support, email support@flex-radio.com.",
							comboAudioSoundCard.Text+" Mixer Initialization Failed",
							MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation);
						console.PowerEnabled = false;
					}
					else if(comboAudioInput1.Text != "ASIO4ALL v2" &&
						comboAudioInput1.Text != "Wuschel's ASIO4ALL")
					{
						MessageBox.Show("ASIO4ALL driver not found.  Please visit " +
							"www.asio4all.com, download and install the driver, "+
							"and try again.  Alternatively, you can use the Unsupported "+
							"Card selection and setup the sound interface manually.  For "+
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
					}
					break;
				case SoundCard.AUDIGY_2_ZS:
					grpAudioDetails1.Enabled = false;
					grpAudioVolts1.Visible = false;
					udAudioVoltage1.Value = 2.23M;
					if(comboAudioSampleRate1.Items.Contains(96000))
						comboAudioSampleRate1.Items.Remove(96000);
					if(comboAudioSampleRate1.Items.Contains(192000))
						comboAudioSampleRate1.Items.Remove(192000);
					comboAudioSampleRate1.Text = "48000";
					foreach(PADeviceInfo p in comboAudioDriver1.Items)
					{
						if(p.Name == "ASIO")
						{
							comboAudioDriver1.SelectedItem = p;
							break;
						}
					}
					
					foreach(PADeviceInfo dev in comboAudioInput1.Items)
					{
						if(dev.Name == "Wuschel's ASIO4ALL")
						{
							comboAudioInput1.Text = "Wuschel's ASIO4ALL";
							comboAudioOutput1.Text = "Wuschel's ASIO4ALL";
						}
					}
					if(comboAudioInput1.Text != "Wuschel's ASIO4ALL")
					{
						foreach(PADeviceInfo dev in comboAudioInput1.Items)
						{
							if(dev.Name == "ASIO4ALL v2")
							{
								comboAudioInput1.Text = "ASIO4ALL v2";
								comboAudioOutput1.Text = "ASIO4ALL v2";
							}
						}
					}

					for(int i=0; i<comboAudioMixer1.Items.Count; i++)
					{
						if(((string)comboAudioMixer1.Items[i]).StartsWith("SB Audigy"))
						{
							comboAudioMixer1.SelectedIndex = i;
							break;
						}
					}

					for(int i=0; i<comboAudioReceive1.Items.Count; i++)
					{
						if(((string)comboAudioReceive1.Items[i]).StartsWith("Analog"))
						{
							comboAudioReceive1.SelectedIndex = i;
							break;
						}
					}

					if(comboAudioReceive1.SelectedIndex < 0 ||
						!comboAudioReceive1.Text.StartsWith("Analog"))
					{
						for(int i=0; i<comboAudioReceive1.Items.Count; i++)
						{
							if(((string)comboAudioReceive1.Items[i]).StartsWith("Mix ana"))
							{
								comboAudioReceive1.SelectedIndex = i;
								break;
							}
						}
					}

					for(int i=0; i<comboAudioTransmit1.Items.Count; i++)
					{
						if(((string)comboAudioTransmit1.Items[i]).StartsWith("Mi"))
						{
							comboAudioTransmit1.SelectedIndex = i;
							break;
						}
					}

					if(comboAudioMixer1.SelectedIndex < 0 || 
						!comboAudioMixer1.Text.StartsWith("SB Audigy"))
					{
						MessageBox.Show(comboAudioSoundCard.Text+" not found.\n "+
							"Please verify that this specific sound card is installed " +
							"and functioning and try again.  \nIf your sound card is not " +
							"a "+comboAudioSoundCard.Text+" and your card is not in the "+
							"list, use the Unsupported Card selection.  \nFor more support, "+
							"email support@flex-radio.com.",
							comboAudioSoundCard.Text+" Not Found",
							MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation);
						console.PowerEnabled = false;
					}
					else if(!Mixer.InitAudigy2ZS(console.MixerID1))
					{
						MessageBox.Show("The "+comboAudioSoundCard.Text+" mixer initialization "+
							"failed.  Please install the latest drivers from www.creativelabs.com " +
							" and try again.  For more support, email support@flex-radio.com.",
							comboAudioSoundCard.Text+" Mixer Initialization Failed",
							MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation);
						console.PowerEnabled = false;
					}
					else if(comboAudioInput1.Text != "ASIO4ALL v2" &&
						comboAudioInput1.Text != "Wuschel's ASIO4ALL")
					{
						MessageBox.Show("ASIO4ALL driver not found.  Please visit " +
							"www.asio4all.com, download and install the driver, "+
							"and try again.  Alternatively, you can use the Unsupported "+
							"Card selection and setup the sound interface manually.  For "+
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
					}
					break;
				case SoundCard.EXTIGY:
					grpAudioDetails1.Enabled = false;
					grpAudioVolts1.Visible = false;
					udAudioVoltage1.Value = 1.96M;
					if(comboAudioSampleRate1.Items.Contains(96000))
						comboAudioSampleRate1.Items.Remove(96000);
					if(comboAudioSampleRate1.Items.Contains(192000))
						comboAudioSampleRate1.Items.Remove(192000);
					comboAudioSampleRate1.Text = "48000";
					foreach(PADeviceInfo p in comboAudioDriver1.Items)
					{
						if(p.Name == "ASIO")
						{
							comboAudioDriver1.SelectedItem = p;
							break;
						}
					}
					
					foreach(PADeviceInfo dev in comboAudioInput1.Items)
					{
						if(dev.Name == "Wuschel's ASIO4ALL")
						{
							comboAudioInput1.Text = "Wuschel's ASIO4ALL";
							comboAudioOutput1.Text = "Wuschel's ASIO4ALL";
						}
					}
					if(comboAudioInput1.Text != "Wuschel's ASIO4ALL")
					{
						foreach(PADeviceInfo dev in comboAudioInput1.Items)
						{
							if(dev.Name == "ASIO4ALL v2")
							{
								comboAudioInput1.Text = "ASIO4ALL v2";
								comboAudioOutput1.Text = "ASIO4ALL v2";
							}
						}
					}

					for(int i=0; i<comboAudioMixer1.Items.Count; i++)
					{
						if(((string)comboAudioMixer1.Items[i]).StartsWith("Creative SB Extigy"))
						{
							comboAudioMixer1.SelectedIndex = i;
							break;
						}
					}

					comboAudioReceive1.Text = "Line In";
					comboAudioTransmit1.Text = "Microphone";

					if(comboAudioMixer1.SelectedIndex < 0 ||
						comboAudioMixer1.Text != "Creative SB Extigy")
					{
						MessageBox.Show(comboAudioSoundCard.Text+" not found.\n "+
							"Please verify that this specific sound card is installed " +
							"and functioning and try again.  \nIf your sound card is not " +
							"a "+comboAudioSoundCard.Text+" and your card is not in the "+
							"list, use the Unsupported Card selection.  \nFor more support, "+
							"email support@flex-radio.com.",
							comboAudioSoundCard.Text+" Not Found",
							MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation);
						console.PowerEnabled = false;
					}
					else if(!Mixer.InitExtigy(console.MixerID1))
					{
						MessageBox.Show("The "+comboAudioSoundCard.Text+" mixer initialization "+
							"failed.  Please install the latest drivers from www.creativelabs.com " +
							" and try again.  For more support, email support@flex-radio.com.",
							comboAudioSoundCard.Text+" Mixer Initialization Failed",
							MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation);
						console.PowerEnabled = false;
					}
					else if(comboAudioInput1.Text != "ASIO4ALL v2" &&
						comboAudioInput1.Text != "Wuschel's ASIO4ALL")
					{
						MessageBox.Show("ASIO4ALL driver not found.  Please visit " +
							"www.asio4all.com, download and install the driver, "+
							"and try again.  Alternatively, you can use the Unsupported "+
							"Card selection and setup the sound interface manually.  For "+
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
					}
					break;
				case SoundCard.MP3_PLUS:
					grpAudioDetails1.Enabled = false;
					grpAudioVolts1.Visible = false;
					udAudioVoltage1.Value = 0.982M;
					if(comboAudioSampleRate1.Items.Contains(96000))
						comboAudioSampleRate1.Items.Remove(96000);
					if(comboAudioSampleRate1.Items.Contains(192000))
						comboAudioSampleRate1.Items.Remove(192000);
					comboAudioSampleRate1.Text = "48000";
					foreach(PADeviceInfo p in comboAudioDriver1.Items)
					{
						if(p.Name == "ASIO")
						{
							comboAudioDriver1.SelectedItem = p;
							break;
						}
					}

					for(int i=0; i<comboAudioMixer1.Items.Count; i++)
					{
						if(((string)comboAudioMixer1.Items[i]).StartsWith("Sound Blaster"))
						{
							comboAudioMixer1.SelectedIndex = i;
							break;
						}
					}

					if(comboAudioMixer1.SelectedIndex < 0 ||
						(string)comboAudioMixer1.SelectedItem != "Sound Blaster")
					{
						for(int i=0; i<comboAudioMixer1.Items.Count; i++)
						{
							if(((string)comboAudioMixer1.Items[i]).StartsWith("USB Audio"))
							{
								comboAudioMixer1.SelectedIndex = i;
								break;
							}
						}
					}

					foreach(PADeviceInfo dev in comboAudioInput1.Items)
					{
						if(dev.Name == "Wuschel's ASIO4ALL")
						{
							comboAudioInput1.Text = "Wuschel's ASIO4ALL";
							comboAudioOutput1.Text = "Wuschel's ASIO4ALL";
						}
					}
					if(comboAudioInput1.Text != "Wuschel's ASIO4ALL")
					{
						foreach(PADeviceInfo dev in comboAudioInput1.Items)
						{
							if(dev.Name == "ASIO4ALL v2")
							{
								comboAudioInput1.Text = "ASIO4ALL v2";
								comboAudioOutput1.Text = "ASIO4ALL v2";
							}
						}
					}

					comboAudioReceive1.Text = "Line In";
					
					for(int i=0; i<comboAudioTransmit1.Items.Count; i++)
					{
						if(((string)comboAudioTransmit1.Items[i]).StartsWith("Mi"))
						{
							comboAudioTransmit1.SelectedIndex = i;
							break;
						}
					}

					if(comboAudioMixer1.SelectedIndex < 0 ||
						(comboAudioMixer1.Text != "Sound Blaster" &&
						comboAudioMixer1.Text != "USB Audio"))
					{
						MessageBox.Show(comboAudioSoundCard.Text+" not found.\n "+
							"Please verify that this specific sound card is installed " +
							"and functioning and try again.  \nIf your sound card is not " +
							"a "+comboAudioSoundCard.Text+" and your card is not in the "+
							"list, use the Unsupported Card selection.  \nFor more support, "+
							"email support@flex-radio.com.",
							comboAudioSoundCard.Text+" Not Found",
							MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation);
						console.PowerEnabled = false;
					}
					else if(!Mixer.InitMP3Plus(console.MixerID1))
					{
						MessageBox.Show("The "+comboAudioSoundCard.Text+" mixer initialization "+
							"failed.  Please install the latest drivers from www.creativelabs.com " +
							" and try again.  For more support, email support@flex-radio.com.",
							comboAudioSoundCard.Text+" Mixer Initialization Failed",
							MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation);
						console.PowerEnabled = false;
					}
					else if(comboAudioInput1.Text != "ASIO4ALL v2" &&
						comboAudioInput1.Text != "Wuschel's ASIO4ALL")
					{
						MessageBox.Show("ASIO4ALL driver not found.  Please visit " +
							"www.asio4all.com, download and install the driver, "+
							"and try again.  Alternatively, you can use the Unsupported "+
							"Card selection and setup the sound interface manually.  For "+
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
					}
					break;
				case SoundCard.DELTA_44:
					grpAudioDetails1.Enabled = false;
					grpAudioVolts1.Visible = false;
					udAudioVoltage1.Value = 0.98M;
					if(!comboAudioSampleRate1.Items.Contains(96000))
						comboAudioSampleRate1.Items.Add(96000);
					if(comboAudioSampleRate1.Items.Contains(192000))
						comboAudioSampleRate1.Items.Remove(192000);
					if(comboAudioSoundCard.Focused || comboAudioSampleRate1.SelectedIndex < 0)
						comboAudioSampleRate1.Text = "48000";
					foreach(PADeviceInfo p in comboAudioDriver1.Items)
					{
						if(p.Name == "ASIO")
						{
							comboAudioDriver1.SelectedItem = p;
							break;
						}
					}

					foreach(PADeviceInfo dev in comboAudioInput1.Items)
					{
						if(dev.Name == "M-Audio Delta ASIO")
						{
							comboAudioInput1.Text = "M-Audio Delta ASIO";
							comboAudioOutput1.Text = "M-Audio Delta ASIO";
						}
					}
					
					comboAudioMixer1.Text = "None";

					if(comboAudioInput1.Text != "M-Audio Delta ASIO")
					{
						MessageBox.Show("M-Audio Delta ASIO driver not found.  Please visit " +
							"www.m-audio.com, download and install the latest driver, "+
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
					}
					break;

				case SoundCard.JANUS_OZY: 
					grpAudioDetails1.Enabled = false;
					grpAudioVolts1.Visible = true;
					udAudioVoltage1.Value = 0.98M;
					if(!comboAudioSampleRate1.Items.Contains(96000))
						comboAudioSampleRate1.Items.Add(96000);
					if(!comboAudioSampleRate1.Items.Contains(192000))
						comboAudioSampleRate1.Items.Add(192000);					
					if(comboAudioSoundCard.Focused || comboAudioSampleRate1.SelectedIndex < 0)
						comboAudioSampleRate1.Text = "48000";

					foreach(PADeviceInfo p in comboAudioDriver1.Items)
					{
						if(p.Name == "Janus/Ozy")						
						{
							comboAudioDriver1.SelectedItem = p;
							break;
						}
					}

					foreach(PADeviceInfo dev in comboAudioInput1.Items)
					{
						if(dev.Name == "Janus/Ozy")
						{
							comboAudioInput1.Text = "AK5394A";
							comboAudioOutput1.Text = "PWM";
						}
					}
					
					comboAudioMixer1.Text = "None";
						
					chkAudioEnableVAC.Enabled = true;
					grpAudioMicInGain1.Enabled = false;
					grpAudioLineInGain1.Enabled = false;
					console.PowerEnabled = true;
					comboAudioChannels1.Text = "4";
					comboAudioChannels1.Enabled = false;
					break; // Janus Ozy 


				case SoundCard.FIREBOX:
					grpAudioDetails1.Enabled = false;
					grpAudioVolts1.Visible = false;
					udAudioVoltage1.Value = 6.39M;
					if(!comboAudioSampleRate1.Items.Contains(96000))
						comboAudioSampleRate1.Items.Add(96000);
					if(comboAudioSampleRate1.Items.Contains(192000))
						comboAudioSampleRate1.Items.Remove(192000);
					if(comboAudioSoundCard.Focused || comboAudioSampleRate1.SelectedIndex < 0)
						comboAudioSampleRate1.Text = "48000";
					foreach(PADeviceInfo p in comboAudioDriver1.Items)
					{
						if(p.Name == "ASIO")
						{
							comboAudioDriver1.SelectedItem = p;
							break;
						}
					}

					foreach(PADeviceInfo dev in comboAudioInput1.Items)
					{
						if(dev.Name == "PreSonus ASIO Driver (FireBox)")
						{
							comboAudioInput1.Text = "PreSonus ASIO Driver (FireBox)";
							comboAudioOutput1.Text = "PreSonus ASIO Driver (FireBox)";
						}
					}
					
					comboAudioMixer1.Text = "None";

					if(comboAudioInput1.Text != "PreSonus ASIO Driver (FireBox)")
					{
						MessageBox.Show("PreSonus FireBox ASIO driver not found.  Please visit " +
							"www.presonus.com, download and install the latest driver, "+
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
						Thread t = new Thread(new ThreadStart(FireBoxMixerFix));
						t.Name = "FireBoxMixerFix";
						t.Priority = ThreadPriority.Normal;
						t.IsBackground = true;
						t.Start();
					}
					break;
				case SoundCard.EDIROL_FA_66:
					grpAudioDetails1.Enabled = false;
					grpAudioVolts1.Visible = false;
					udAudioVoltage1.Value = 2.327M;
					if(!comboAudioSampleRate1.Items.Contains(96000))
						comboAudioSampleRate1.Items.Add(96000);
					if(!comboAudioSampleRate1.Items.Contains(192000))
						comboAudioSampleRate1.Items.Add(192000);
					if(comboAudioSoundCard.Focused || comboAudioSampleRate1.SelectedIndex < 0)
						comboAudioSampleRate1.Text = "192000";
					foreach(PADeviceInfo p in comboAudioDriver1.Items)
					{
						if(p.Name == "ASIO")
						{
							comboAudioDriver1.SelectedItem = p;
							break;
						}
					}

					foreach(PADeviceInfo dev in comboAudioInput1.Items)
					{
						if(dev.Name == "EDIROL FA-66")
						{
							comboAudioInput1.Text = "EDIROL FA-66";
							comboAudioOutput1.Text = "EDIROL FA-66";
						}
					}
					
					comboAudioMixer1.Text = "None";

					if(comboAudioInput1.Text != "EDIROL FA-66")
					{
						MessageBox.Show("Edirol FA-66 ASIO driver not found.  Please visit " +
							"www.rolandus.com, download and install the latest driver, "+
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
					}
					break;
				case SoundCard.UNSUPPORTED_CARD:
					if(comboAudioSoundCard.Focused)
					{
						MessageBox.Show("Proper operation of the SDR-1000 depends on the use of a sound card that is\n"+
							"officially recommended by FlexRadio Systems.  Refer to the Specifications page on\n"+
							"www.flex-radio.com to determine which sound cards are currently recommended.  Use only\n"+
							"the specific model numbers stated on the website because other models within the same\n"+
							"family may not work properly with the radio.  Officially supported sound cards may be\n"+
							"updated on the website without notice.  If you have any question about the sound card\n"+
							"you would like to use with the radio, please email support@flex-radio.com or call us at\n"+
							"512-250-8595.\n\n"+

							"NO WARRANTY IS IMPLIED WHEN THE SDR-1000 IS USED WITH ANY SOUND CARD OTHER\n"+
							"THAN THOSE CURRENTLY RECOMMENDED AS STATED ON THE FLEXRADIO SYSTEMS WEBSITE.\n"+
							"UNSUPPORTED SOUND CARDS MAY OR MAY NOT WORK WITH THE SDR-1000.  USE OF\n"+
							"UNSUPPORTED SOUND CARDS IS AT THE CUSTOMERS OWN RISK.",
							"Warning: Unsupported Card",
							MessageBoxButtons.OK,
							MessageBoxIcon.Warning);
					}
					grpAudioVolts1.Visible = true;
					if(comboAudioSoundCard.Focused)
						chkGeneralRXOnly.Checked = true;
					if(!comboAudioSampleRate1.Items.Contains(96000))
						comboAudioSampleRate1.Items.Add(96000);
					if(!comboAudioSampleRate1.Items.Contains(192000))
						comboAudioSampleRate1.Items.Add(192000);
					if(comboAudioSoundCard.Focused || comboAudioSampleRate1.SelectedIndex < 0)
						comboAudioSampleRate1.Text = "48000";
					grpAudioDetails1.Enabled = true;
					grpAudioMicInGain1.Enabled = true;
					grpAudioLineInGain1.Enabled = true;
					console.PowerEnabled = true;
					comboAudioChannels1.Text = "2";
					comboAudioChannels1.Enabled = true;
					break;				
			}

			console.PWR = console.PWR;
			console.AF = console.AF;
			if(on) console.PowerOn = true;
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
		}

		private void udDisplayGridMax_ValueChanged(object sender, System.EventArgs e)
		{
			if(udDisplayGridMax.Value <= udDisplayGridMin.Value)
				udDisplayGridMax.Value = udDisplayGridMin.Value + 10;
			Display.SpectrumGridMax = (int)udDisplayGridMax.Value;
		}

		private void udDisplayGridMin_ValueChanged(object sender, System.EventArgs e)
		{
			if(udDisplayGridMin.Value >= udDisplayGridMax.Value)
				udDisplayGridMin.Value = udDisplayGridMax.Value - 10;
			Display.SpectrumGridMin = (int)udDisplayGridMin.Value;
		}

		private void udDisplayGridStep_ValueChanged(object sender, System.EventArgs e)
		{
			Display.SpectrumGridStep = (int)udDisplayGridStep.Value;
		}

		private void comboDisplayLabelAlign_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch(comboDisplayLabelAlign.Text)
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
			double buffer_time = double.Parse(comboAudioBuffer1.Text) / (double)console.SampleRate1;
			int buffersToAvg = (int)((float)udDisplayAVGTime.Value * 0.001 / buffer_time);
			buffersToAvg = Math.Max(2, buffersToAvg);
			Display.DisplayAvgBlocks = buffersToAvg;
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
			if (chkSpectrumPolyphase.Checked) DttSP.SetSpectrumPolyphase(1);
			else DttSP.SetSpectrumPolyphase(0);
		}

		private void udDisplayScopeTime_ValueChanged(object sender, System.EventArgs e)
		{
			console.ScopeTime = (int)udDisplayScopeTime.Value;
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
			switch(comboDisplayDriver.Text)
			{
				case "GDI+":
					console.CurrentDisplayEngine = DisplayEngine.GDI_PLUS;
					break;
				/*case "DirectX":
					console.CurrentDisplayEngine = DisplayEngine.DIRECT_X;
					break;*/
			}
		}

		#endregion

		#region DSP Tab Event Handlers
		// ======================================================
		// DSP Tab Event Handlers
		// ======================================================

		private void udLMSNR_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetNRvals((int)udLMSNRtaps.Value,(int)udLMSNRdelay.Value,
				0.00001*(double)udLMSNRgain.Value,0.00005);
		}

		private void udDSPNB_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetNBvals(0.165*(double)(udDSPNB.Value));
		}

		private void udDSPNB2_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetSDROMvals(0.165*(double)(udDSPNB2.Value));
		}

		private void comboDSPWindow_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DttSP.CurrentWindow = (DttSP.Window)comboDSPWindow.SelectedIndex;
		}

		private void comboDSPBufSize_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int size = Int32.Parse(comboDSPBufSize.Text);

			int i = (int)Math.Log(size, 2);
			console.FilterSizeCalOffset = 3*(11-i);

			if(console.PowerOn)
			{
				console.PowerOn = false;
				Thread.Sleep(100);
				DttSP.ResizeSDR(size);
				console.PowerOn = true;
			} 
			else DttSP.ResizeSDR(Int32.Parse(comboDSPBufSize.Text));
		}

		private void chkBlkLMSNR_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkBlkLMSNR.Checked) DttSP.SetBlkNR(1);
			else DttSP.SetBlkNR(0);
		}

		private void chkBlkLMSANF_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkBlkLMSANF.Checked) DttSP.SetBlkANF(1);
			else DttSP.SetBlkANF(0);
		}

		#region Image Reject

		private void tbDSPImagePhaseRX_Scroll(object sender, System.EventArgs e)
		{
			udDSPImagePhaseRX.Value = tbDSPImagePhaseRX.Value;
		}

		private void udDSPImagePhaseRX_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetCorrectIQPhase((double)udDSPImagePhaseRX.Value);
			if(tbDSPImagePhaseRX.Value != (int)udDSPImagePhaseRX.Value)
				tbDSPImagePhaseRX.Value = (int)udDSPImagePhaseRX.Value;
		}

		private void tbDSPImageGainRX_Scroll(object sender, System.EventArgs e)
		{
			udDSPImageGainRX.Value = tbDSPImageGainRX.Value;
		}

		private void udDSPImageGainRX_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetCorrectIQGain((double)udDSPImageGainRX.Value);
			if(tbDSPImageGainRX.Value != (int)udDSPImageGainRX.Value)
				tbDSPImageGainRX.Value = (int)udDSPImageGainRX.Value;
		}

		private void udDSPImagePhaseTX_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetTXIQPhase((double)udDSPImagePhaseTX.Value);
			if(tbDSPImagePhaseTX.Value != (int)udDSPImagePhaseTX.Value)
				tbDSPImagePhaseTX.Value = (int)udDSPImagePhaseTX.Value;
		
		}

		private void tbDSPImagePhaseTX_Scroll(object sender, System.EventArgs e)
		{
			udDSPImagePhaseTX.Value = tbDSPImagePhaseTX.Value;		
		}

		private void udDSPImageGainTX_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetTXIQGain((double)udDSPImageGainTX.Value);
			if(tbDSPImageGainTX.Value != (int)udDSPImageGainTX.Value)
				tbDSPImageGainTX.Value = (int)udDSPImageGainTX.Value;
		
		}

		private void tbDSPImageGainTX_Scroll(object sender, System.EventArgs e)
		{
			udDSPImageGainTX.Value = tbDSPImageGainTX.Value;
		}
		
		private void udLMSANF_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetANFvals((int)udLMSANFtaps.Value,(int)udLMSANFdelay.Value,
				0.00001*(double)udLMSANFgain.Value,0.00005);
		}

		private void chkTXImagCal_CheckedChanged(object sender, System.EventArgs e)
		{
			if(checkboxTXImagCal.Checked)
				Audio.CurrentInputSignal = Audio.SignalSource.SINE;
			else Audio.CurrentInputSignal = Audio.SignalSource.SOUNDCARD;
		}

		#endregion

		#region Keyer

		private void udDSPCWPitch_ValueChanged(object sender, System.EventArgs e)
		{
			console.CWPitch = (int)udDSPCWPitch.Value;
			DttSP.SetKeyerFreq((float)udDSPCWPitch.Value);
		}

		private void chkCWKeyerIambic_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkCWKeyerIambic.Checked) DttSP.SetKeyerIambic(1);
			else DttSP.SetKeyerIambic(0);
			console.CWIambic = chkCWKeyerIambic.Checked;
		}

		private void udCWKeyerWeight_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetKeyerWeight((int)udCWKeyerWeight.Value);
		}

		private void udCWKeyerRamp_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetKeyerRamp((int)udCWKeyerRamp.Value);
		}

		private void udCWKeyerSemiBreakInDelay_ValueChanged(object sender, System.EventArgs e)
		{
			console.BreakInDelay = (double)udCWKeyerSemiBreakInDelay.Value;
		}

		private void chkDSPKeyerSemiBreakInEnabled_CheckedChanged(object sender, System.EventArgs e)
		{
			console.CWSemiBreakInEnabled = chkDSPKeyerSemiBreakInEnabled.Checked;
			console.BreakInEnabled = chkDSPKeyerSemiBreakInEnabled.Checked;
			udCWKeyerSemiBreakInDelay.Enabled = chkDSPKeyerSemiBreakInEnabled.Checked;
		}

		private void chkDSPKeyerDisableMonitor_CheckedChanged(object sender, System.EventArgs e)
		{
			console.CWDisableMonitor = chkDSPKeyerDisableMonitor.Checked;
		}

		private void udCWKeyerDeBounce_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetKeyerDeBounce((int)udCWKeyerDeBounce.Value);
		}

		private void chkCWKeyerRevPdl_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkCWKeyerRevPdl.Checked) DttSP.SetKeyerRevPdl(1);
			else DttSP.SetKeyerRevPdl(0);
		}

		private void chkHiPerfKeyer_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkHiPerfKeyer.Checked) DttSP.SetKeyerPerf(1);
			else DttSP.SetKeyerPerf(0);
		}

		private void comboKeyerConnPrimary_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			bool running = System.Convert.ToBoolean(DttSP.KeyerRunning());
			if (running) DttSP.StopKeyer();
			Thread.Sleep(10);
			console.Keyer.PrimaryConnPort = comboKeyerConnPrimary.Text;
			if(console.Keyer.PrimaryConnPort == "SDR" && comboKeyerConnPrimary.Text != "SDR")
				comboKeyerConnPrimary.Text = "SDR";
			if (running) DttSP.StartKeyer();
		}

		private void comboKeyerConnSecondary_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(initializing) return;
			bool running = System.Convert.ToBoolean(DttSP.KeyerRunning());
			if (running) DttSP.StopKeyer();
			Thread.Sleep(10);
			if(comboKeyerConnSecondary.Text == "CAT" && !chkCATEnable.Checked)
			{
				MessageBox.Show("CAT is not Enabled.  Please enable the CAT interface before selecting this option.",
					"CAT not enabled",
					MessageBoxButtons.OK,
					MessageBoxIcon.Hand);
				comboKeyerConnSecondary.Text = "None";
				return;
			}

			console.Keyer.SecondaryConnPort = comboKeyerConnSecondary.Text;
			switch(comboKeyerConnSecondary.Text)
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
					break;
				default: // COMx
					lblKeyerConnPTTLine.Visible = true;
					comboKeyerConnPTTLine.Visible = true;
					lblKeyerConnKeyLine.Visible = true;
					comboKeyerConnKeyLine.Visible = true;
					break;
			}
			if(console.Keyer.SecondaryConnPort == "None" && comboKeyerConnSecondary.Text != "None")
				comboKeyerConnSecondary.Text = "None";
			if (running) DttSP.StartKeyer();
		}

		private void comboKeyerConnKeyLine_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(comboKeyerConnKeyLine.SelectedIndex < 0) return;
			console.Keyer.SecondaryKeyLine = (KeyerLine)comboKeyerConnKeyLine.SelectedIndex;
		}

		private void comboKeyerConnPTTLine_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(comboKeyerConnPTTLine.SelectedIndex < 0) return;
			console.Keyer.SecondaryPTTLine = (KeyerLine)comboKeyerConnPTTLine.SelectedIndex;
		}

		#endregion

		#region AGC

		private void udDSPAGCFixedGaindB_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetFixedAGC((double)udDSPAGCFixedGaindB.Value);
			if(console.CurrentAGCMode == AGCMode.FIXD)
				console.RF = (int)udDSPAGCFixedGaindB.Value;
		}

		private void udDSPAGCMaxGaindB_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetRXAGCMaxGain((double)udDSPAGCMaxGaindB.Value);
			if(console.CurrentAGCMode != AGCMode.FIXD)
				console.RF = (int)udDSPAGCMaxGaindB.Value;
		}

		private void udDSPAGCAttack_ValueChanged(object sender, System.EventArgs e)
		{
			if(udDSPAGCAttack.Enabled)
				DttSP.SetRXAGCAttack((int)udDSPAGCAttack.Value);
		}

		private void udDSPAGCDecay_ValueChanged(object sender, System.EventArgs e)
		{
			if(udDSPAGCDecay.Enabled)
				DttSP.SetRXAGCDecay((int)udDSPAGCDecay.Value);
		}

		private void udDSPAGCSlope_ValueChanged(object sender, System.EventArgs e)
		{
				DttSP.SetRXAGCSlope(10*(int)(udDSPAGCSlope.Value));
		}

		private void udDSPAGCHangTime_ValueChanged(object sender, System.EventArgs e)
		{
			if(udDSPAGCHangTime.Enabled)
				DttSP.SetRXAGCHang((int)udDSPAGCHangTime.Value);
		}
		private void tbDSPAGCHangThreshold_Scroll(object sender, System.EventArgs e)
		{
				DttSP.SetRXAGCHangThreshold((int)tbDSPAGCHangThreshold.Value);
		}

		#endregion

		#region Leveler

		private void udDSPLevelerHangTime_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetTXLevelerHang((int)udDSPLevelerHangTime.Value);
		}

		private void tbDSPLevelerHangThreshold_Scroll(object sender, System.EventArgs e)
		{
		
		}

		private void udDSPLevelerSlope_ValueChanged(object sender, System.EventArgs e)
		{
		
		}

		private void udDSPLevelerThreshold_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetTXLevelerMaxGain((double)udDSPLevelerThreshold.Value);
		}

		private void udDSPLevelerAttack_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetTXLevelerAttack((int)udDSPLevelerAttack.Value);
		}

		private void udDSPLevelerDecay_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetTXLevelerDecay((int)udDSPLevelerDecay.Value);
		}

		private void chkDSPLevelerEnabled_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkDSPLevelerEnabled.Checked)
				DttSP.SetTXLevelerSt(1);
			else
				DttSP.SetTXLevelerSt(0);
		}

		#endregion

		#region ALC

		private void udDSPALCHangTime_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetTXALCHang((int)udDSPALCHangTime.Value);
		}

		private void tbDSPALCHangThreshold_Scroll(object sender, System.EventArgs e)
		{
		
		}

		private void udDSPALCThreshold_ValueChanged(object sender, System.EventArgs e)
		{
			//DttSP.SetTXALCBot((double)udDSPALCThreshold.Value);
		}

		private void udDSPALCAttack_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetTXALCAttack((int)udDSPALCAttack.Value);
		}

		private void udDSPALCDecay_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetTXALCDecay((int)udDSPALCDecay.Value);
		}

		private void udDSPALCSlope_ValueChanged(object sender, System.EventArgs e)
		{
		
		}

		#endregion

		#endregion

		#region Transmit Tab Event Handlers

		private void udTXFilterHigh_ValueChanged(object sender, System.EventArgs e)
		{
			if(udTXFilterHigh.Value < udTXFilterLow.Value + 100)
			{
				udTXFilterHigh.Value = udTXFilterLow.Value + 100;
				return;
			}

			if(udTXFilterHigh.Focused &&
				(udTXFilterHigh.Value - udTXFilterLow.Value) > 3000 &&
				(console.TXFilterHigh - console.TXFilterLow) <= 3000)
			{
				(new Thread(new ThreadStart(TXBW))).Start();
			}

			console.TXFilterHigh = (int)udTXFilterHigh.Value;
			
		}

		private void TXBW()
		{
			MessageBox.Show("The transmit bandwidth is being increased beyond 3kHz.\n\n"+
				"As the control operator, you are responsible for compliance with current "+
				"rules and good operating practice.",
				"Warning: Transmit Bandwidth",
				MessageBoxButtons.OK,
				MessageBoxIcon.Warning);
		}

		private void udTXFilterLow_ValueChanged(object sender, System.EventArgs e)
		{
			if(udTXFilterLow.Value > udTXFilterHigh.Value - 100)
			{
				udTXFilterLow.Value = udTXFilterHigh.Value - 100;
				return;
			}

			if(udTXFilterLow.Focused &&
				(udTXFilterHigh.Value - udTXFilterLow.Value) > 3000 &&
				(console.TXFilterHigh - console.TXFilterLow) <= 3000)
			{
				(new Thread(new ThreadStart(TXBW))).Start();
			}

			console.TXFilterLow = (int)udTXFilterLow.Value;
		}

		private void tbTXFFCompression_Scroll(object sender, System.EventArgs e)
		{
			DttSP.SetTXAGCFFCompression((double)tbTXFFCompression.Value);
			udTXFFCompression.Value = tbTXFFCompression.Value;
		}

		private void udTXFFCompression_ValueChanged(object sender, System.EventArgs e)
		{
			tbTXFFCompression.Value = (int)udTXFFCompression.Value;
			tbTXFFCompression_Scroll(this, EventArgs.Empty);
			console.COMPVal = (int)tbTXFFCompression.Value;
		}

		private void udTransmitTunePower_ValueChanged(object sender, System.EventArgs e)
		{
			console.TunePower = (int)udTXTunePower.Value;
		}

		private string current_profile = "";
		private void comboTXProfileName_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(comboTXProfileName.SelectedIndex < 0)
				return;

			if(CheckTXProfileChanged() && comboTXProfileName.Focused)
			{
				DialogResult result = MessageBox.Show("The current profile has changed.  "+
					"Would you like to save the current profile?",
					"Save Current Profile?",
					MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Question);
				
				if(result == DialogResult.Yes)
				{
					btnTXProfileSave_Click(this, EventArgs.Empty);
					//return;
				}
				else if(result == DialogResult.Cancel)
					return;
			}

			console.TXProfile = comboTXProfileName.Text;
			DataRow[] rows = DB.dsTxProfile.Tables["TxProfile"].Select(
				"'"+comboTXProfileName.Text+"' = Name");

			if(rows.Length != 1)
			{
				MessageBox.Show("Database error reading TxProfile Table.",
					"Database error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}
			
			DataRow dr = rows[0];
			int[] eq = null;
			eq = new int[4];

			console.EQForm.TXEQEnabled = (bool)dr["TXEQEnabled"];
			
			eq[0] = (int)dr["TXEQPreamp"];
			for(int i=1; i<eq.Length; i++)
				eq[i] = (int)dr["TXEQ"+i.ToString()];
			console.EQForm.TXEQ = eq;

			udTXFilterLow.Value = (int)dr["FilterLow"];
			udTXFilterHigh.Value = (int)dr["FilterHigh"];
			
			console.COMP = (bool)dr["CompressorOn"];
			udTXFFCompression.Value = (int)dr["CompressorLevel"];
			
			console.CPDR = (bool)dr["CompanderOn"];			
			udTXCompander.Value = (int)dr["CompanderLevel"];
			
			console.Mic = (int)dr["MicGain"];

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

			current_profile = comboTXProfileName.Text;
		}

		private void btnTXProfileSave_Click(object sender, System.EventArgs e)
		{
			string name = InputBox.Show("Save Profile", "Please enter a profile name:",
				current_profile);

			if(name == "")
				return;

			DataRow dr = null;
			if(comboTXProfileName.Items.Contains(name))
			{
				DialogResult result = MessageBox.Show(
					"Are you sure you want to overwrite the "+name+" TX Profile?",
					"Overwrite Profile?",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question);
				
				if(result == DialogResult.No)
					return;

				foreach(DataRow d in DB.dsTxProfile.Tables["TxProfile"].Rows)
				{
					if((string)d["Name"] == name) 
					{
						dr = d;
						break;
					}
				}
			}
			else
			{
				dr = DB.dsTxProfile.Tables["TxProfile"].NewRow();
				dr["Name"] = name;
			}
			
			dr["FilterLow"] = (int)udTXFilterLow.Value;
			dr["FilterHigh"] = (int)udTXFilterHigh.Value;
			dr["TXEQEnabled"] = console.EQForm.TXEQEnabled;
			dr["TXEQNumBands"] = 10;
			int[] eq = console.EQForm.TXEQ;
			dr["TXEQPreamp"] = eq[0];
			for(int i=1; i<4; i++)
				dr["TXEQ"+i.ToString()] = eq[i];
			for(int i=4; i<32; i++)
				dr["TXEQ"+i.ToString()] = 0;

			dr["CompressorOn"] = console.COMP;
			dr["CompressorLevel"] = (int)udTXFFCompression.Value;
			dr["CompanderOn"] = console.CPDR;
			dr["CompanderLevel"] = (int)udTXCompander.Value;
			dr["MicGain"] = console.Mic;

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

			if(!comboTXProfileName.Items.Contains(name))
			{
				DB.dsTxProfile.Tables["TxProfile"].Rows.Add(dr);
				comboTXProfileName.Items.Add(name);
			}

			console.UpdateTXProfile();
		}

		private void btnTXProfileDelete_Click(object sender, System.EventArgs e)
		{
			DialogResult dr = MessageBox.Show(
				"Are you sure you want to delete the "+comboTXProfileName.Text+" TX Profile?",
				"Delete Profile?",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning);

			if(dr == DialogResult.No)
				return;

			DataRow[] rows = DB.dsTxProfile.Tables["TxProfile"].Select(
				"'"+comboTXProfileName.Text+"' = Name");

			if(rows.Length == 1)
				rows[0].Delete();

			int index = comboTXProfileName.SelectedIndex;
			comboTXProfileName.Items.Remove(comboTXProfileName.Text);
			if(comboTXProfileName.Items.Count > 0)
			{
				if(index > comboTXProfileName.Items.Count-1)
					index = comboTXProfileName.Items.Count-1;
				comboTXProfileName.SelectedIndex = index;
			}

			console.UpdateTXProfile();
		}

		private void chkDCBlock_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkDCBlock.Checked)
				DttSP.SetDCBlock(1);
			else DttSP.SetDCBlock(0);
		}

		private void tbTXCmpd_Scroll(object sender, System.EventArgs e)
		{
			DttSP.SetTXCompand((double)tbTXCompander.Value*0.3);
			udTXCompander.Value = tbTXCompander.Value;
		}

		private void udTXCmpd_ValueChanged(object sender, System.EventArgs e)
		{
			tbTXCompander.Value = (int)udTXCompander.Value;
			tbTXCmpd_Scroll(this, EventArgs.Empty);
			console.CPDRVal = (int)udTXCompander.Value;
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

		private void udTXVOXHangTime_ValueChanged(object sender, System.EventArgs e)
		{
			console.VOXHangTime = (int)udTXVOXHangTime.Value;
		}

		private void udTXNoiseGate_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetTXSquelchVal((int)udTXNoiseGate.Value);
			console.NoiseGate = (int)udTXNoiseGate.Value;
		}

		private void chkTXNoiseGateEnabled_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkTXNoiseGateEnabled.Checked)
				DttSP.SetTXSquelchState(1);
			else DttSP.SetTXSquelchState(0);

			console.NoiseGateEnabled = chkTXNoiseGateEnabled.Checked;
		}

		private void udTXAF_ValueChanged(object sender, System.EventArgs e)
		{
			console.TXAF = (int)udTXAF.Value;
		}

		private void udTXAMCarrierLevel_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetTXCarrierLevel((double)udTXAMCarrierLevel.Value * 0.005);
		}

		#endregion

		#region PA Settings Tab Event Handlers

		private void btnPAGainCalibration_Click(object sender, System.EventArgs e)
		{
			DialogResult dr = MessageBox.Show("Is a 50 Ohm dummy load connected to the amplifier?\n" +
				"Failure to use a dummy load with this routine could cause damage to the amplifier.",
				"Warning: Is dummy load connected?",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning);

			if(dr == DialogResult.No)
				return;

			progress = new Progress("Calibrate PA Gain");

			Thread t = new Thread(new ThreadStart(CalibratePAGain));
			t.Name = "PA Gain Calibration Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.AboveNormal;
			t.Start();

			if(console.PowerOn)
				progress.Show();
		}

		private void CalibratePAGain()
		{
			bool[] run = new bool[10];
			for(int i=0; i<10; i++) run[i] = true;
			bool done = console.CalibratePAGain(progress, run, (int)udPACalPower.Value);
			if(done) MessageBox.Show("PA Gain Calibration complete.");
		}

		private void udPAGain_ValueChanged(object sender, System.EventArgs e)
		{
			console.PWR = console.PWR;
		}

		private void btnPAGainReset_Click(object sender, System.EventArgs e)
		{
			udPAGain160.Value = 48.0M;
			udPAGain80.Value = 48.0M;
			udPAGain60.Value = 48.0M;
			udPAGain40.Value = 48.0M;
			udPAGain30.Value = 48.0M;
			udPAGain20.Value = 48.0M;
			udPAGain17.Value = 48.0M;
			udPAGain15.Value = 48.0M;
			udPAGain12.Value = 48.0M;
			udPAGain10.Value = 48.0M;
		}

		#endregion

		#region Appearance Tab Event Handlers

		private void clrbtnBackground_Changed(object sender, System.EventArgs e)
		{
			Display.DisplayBackgroundColor = clrbtnBackground.Color;
		}

		private void clrbtnGrid_Changed(object sender, System.EventArgs e)
		{
			Display.GridColor = clrbtnGrid.Color;
		}

		private void clrbtnZeroLine_Changed(object sender, System.EventArgs e)
		{
			Display.GridZeroColor = clrbtnZeroLine.Color;
		}

		private void clrbtnText_Changed(object sender, System.EventArgs e)
		{
			Display.GridTextColor = clrbtnText.Color;
		}

		private void clrbtnDataLine_Changed(object sender, System.EventArgs e)
		{
			Display.DataLineColor = clrbtnDataLine.Color;
		}

		private void clrbtnFilter_Changed(object sender, System.EventArgs e)
		{
			Display.DisplayFilterColor = clrbtnFilter.Color;
		}

		private void udDisplayLineWidth_ValueChanged(object sender, System.EventArgs e)
		{
			Display.DisplayLineWidth = (float)udDisplayLineWidth.Value;
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
			if(chkExtRX1601.Checked) val += 1<<0;
			if(chkExtRX1602.Checked) val += 1<<1;
			if(chkExtRX1603.Checked) val += 1<<2;
			if(chkExtRX1604.Checked) val += 1<<3;
			if(chkExtRX1605.Checked) val += 1<<4;
			if(chkExtRX1606.Checked) val += 1<<5;

			console.X2160RX = (byte)val;
		}

		private void chkExtTX160_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtTX1601.Checked) val += 1<<0;
			if(chkExtTX1602.Checked) val += 1<<1;
			if(chkExtTX1603.Checked) val += 1<<2;
			if(chkExtTX1604.Checked) val += 1<<3;
			if(chkExtTX1605.Checked) val += 1<<4;
			if(chkExtTX1606.Checked) val += 1<<5;

			console.X2160TX = (byte)val;
		}

		private void chkExtRX80_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtRX801.Checked) val += 1<<0;
			if(chkExtRX802.Checked) val += 1<<1;
			if(chkExtRX803.Checked) val += 1<<2;
			if(chkExtRX804.Checked) val += 1<<3;
			if(chkExtRX805.Checked) val += 1<<4;
			if(chkExtRX806.Checked) val += 1<<5;

			console.X280RX = (byte)val;
		}

		private void chkExtTX80_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtTX801.Checked) val += 1<<0;
			if(chkExtTX802.Checked) val += 1<<1;
			if(chkExtTX803.Checked) val += 1<<2;
			if(chkExtTX804.Checked) val += 1<<3;
			if(chkExtTX805.Checked) val += 1<<4;
			if(chkExtTX806.Checked) val += 1<<5;

			console.X280TX = (byte)val;
		}

		private void chkExtRX60_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtRX601.Checked) val += 1<<0;
			if(chkExtRX602.Checked) val += 1<<1;
			if(chkExtRX603.Checked) val += 1<<2;
			if(chkExtRX604.Checked) val += 1<<3;
			if(chkExtRX605.Checked) val += 1<<4;
			if(chkExtRX606.Checked) val += 1<<5;

			console.X260RX = (byte)val;
		}

		private void chkExtTX60_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtTX601.Checked) val += 1<<0;
			if(chkExtTX602.Checked) val += 1<<1;
			if(chkExtTX603.Checked) val += 1<<2;
			if(chkExtTX604.Checked) val += 1<<3;
			if(chkExtTX605.Checked) val += 1<<4;
			if(chkExtTX606.Checked) val += 1<<5;

			console.X260TX = (byte)val;
		}

		private void chkExtRX40_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtRX401.Checked) val += 1<<0;
			if(chkExtRX402.Checked) val += 1<<1;
			if(chkExtRX403.Checked) val += 1<<2;
			if(chkExtRX404.Checked) val += 1<<3;
			if(chkExtRX405.Checked) val += 1<<4;
			if(chkExtRX406.Checked) val += 1<<5;

			console.X240RX = (byte)val;
		}

		private void chkExtTX40_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtTX401.Checked) val += 1<<0;
			if(chkExtTX402.Checked) val += 1<<1;
			if(chkExtTX403.Checked) val += 1<<2;
			if(chkExtTX404.Checked) val += 1<<3;
			if(chkExtTX405.Checked) val += 1<<4;
			if(chkExtTX406.Checked) val += 1<<5;

			console.X240TX = (byte)val;
		}

		private void chkExtRX30_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtRX301.Checked) val += 1<<0;
			if(chkExtRX302.Checked) val += 1<<1;
			if(chkExtRX303.Checked) val += 1<<2;
			if(chkExtRX304.Checked) val += 1<<3;
			if(chkExtRX305.Checked) val += 1<<4;
			if(chkExtRX306.Checked) val += 1<<5;

			console.X230RX = (byte)val;
		}

		private void chkExtTX30_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtTX301.Checked) val += 1<<0;
			if(chkExtTX302.Checked) val += 1<<1;
			if(chkExtTX303.Checked) val += 1<<2;
			if(chkExtTX304.Checked) val += 1<<3;
			if(chkExtTX305.Checked) val += 1<<4;
			if(chkExtTX306.Checked) val += 1<<5;

			console.X230TX = (byte)val;
		}

		private void chkExtRX20_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtRX201.Checked) val += 1<<0;
			if(chkExtRX202.Checked) val += 1<<1;
			if(chkExtRX203.Checked) val += 1<<2;
			if(chkExtRX204.Checked) val += 1<<3;
			if(chkExtRX205.Checked) val += 1<<4;
			if(chkExtTX306.Checked) val += 1<<5;

			console.X220RX = (byte)val;
		}

		private void chkExtTX20_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtTX201.Checked) val += 1<<0;
			if(chkExtTX202.Checked) val += 1<<1;
			if(chkExtTX203.Checked) val += 1<<2;
			if(chkExtTX204.Checked) val += 1<<3;
			if(chkExtTX205.Checked) val += 1<<4;
			if(chkExtTX306.Checked) val += 1<<5;

			console.X220TX = (byte)val;
		}

		private void chkExtRX17_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtRX171.Checked) val += 1<<0;
			if(chkExtRX172.Checked) val += 1<<1;
			if(chkExtRX173.Checked) val += 1<<2;
			if(chkExtRX174.Checked) val += 1<<3;
			if(chkExtRX175.Checked) val += 1<<4;
			if(chkExtRX176.Checked) val += 1<<5;

			console.X217RX = (byte)val;
		}

		private void chkExtTX17_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtTX171.Checked) val += 1<<0;
			if(chkExtTX172.Checked) val += 1<<1;
			if(chkExtTX173.Checked) val += 1<<2;
			if(chkExtTX174.Checked) val += 1<<3;
			if(chkExtTX175.Checked) val += 1<<4;
			if(chkExtTX176.Checked) val += 1<<5;

			console.X217TX = (byte)val;
		}

		private void chkExtRX15_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtRX151.Checked) val += 1<<0;
			if(chkExtRX152.Checked) val += 1<<1;
			if(chkExtRX153.Checked) val += 1<<2;
			if(chkExtRX154.Checked) val += 1<<3;
			if(chkExtRX155.Checked) val += 1<<4;
			if(chkExtRX156.Checked) val += 1<<5;

			console.X215RX = (byte)val;
		}

		private void chkExtTX15_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtTX151.Checked) val += 1<<0;
			if(chkExtTX152.Checked) val += 1<<1;
			if(chkExtTX153.Checked) val += 1<<2;
			if(chkExtTX154.Checked) val += 1<<3;
			if(chkExtTX155.Checked) val += 1<<4;
			if(chkExtTX156.Checked) val += 1<<5;

			console.X215TX = (byte)val;
		}

		private void chkExtRX12_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtRX121.Checked) val += 1<<0;
			if(chkExtRX122.Checked) val += 1<<1;
			if(chkExtRX123.Checked) val += 1<<2;
			if(chkExtRX124.Checked) val += 1<<3;
			if(chkExtRX125.Checked) val += 1<<4;
			if(chkExtRX126.Checked) val += 1<<5;

			console.X212RX = (byte)val;
		}

		private void chkExtTX12_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtTX121.Checked) val += 1<<0;
			if(chkExtTX122.Checked) val += 1<<1;
			if(chkExtTX123.Checked) val += 1<<2;
			if(chkExtTX124.Checked) val += 1<<3;
			if(chkExtTX125.Checked) val += 1<<4;
			if(chkExtTX126.Checked) val += 1<<5;

			console.X212TX = (byte)val;
		}

		private void chkExtRX10_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtRX101.Checked) val += 1<<0;
			if(chkExtRX102.Checked) val += 1<<1;
			if(chkExtRX103.Checked) val += 1<<2;
			if(chkExtRX104.Checked) val += 1<<3;
			if(chkExtRX105.Checked) val += 1<<4;
			if(chkExtRX106.Checked) val += 1<<5;

			console.X210RX = (byte)val;
		}

		private void chkExtTX10_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtTX101.Checked) val += 1<<0;
			if(chkExtTX102.Checked) val += 1<<1;
			if(chkExtTX103.Checked) val += 1<<2;
			if(chkExtTX104.Checked) val += 1<<3;
			if(chkExtTX105.Checked) val += 1<<4;
			if(chkExtTX106.Checked) val += 1<<5;

			console.X210TX = (byte)val;
		}

		private void chkExtRX6_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtRX61.Checked) val += 1<<0;
			if(chkExtRX62.Checked) val += 1<<1;
			if(chkExtRX63.Checked) val += 1<<2;
			if(chkExtRX64.Checked) val += 1<<3;
			if(chkExtRX65.Checked) val += 1<<4;
			if(chkExtRX66.Checked) val += 1<<5;

			console.X26RX = (byte)val;
		}

		private void chkExtTX6_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtTX61.Checked) val += 1<<0;
			if(chkExtTX62.Checked) val += 1<<1;
			if(chkExtTX63.Checked) val += 1<<2;
			if(chkExtTX64.Checked) val += 1<<3;
			if(chkExtTX65.Checked) val += 1<<4;
			if(chkExtTX66.Checked) val += 1<<5;

			console.X26TX = (byte)val;
		}

		private void chkExtRX2_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtRX21.Checked) val += 1<<0;
			if(chkExtRX22.Checked) val += 1<<1;
			if(chkExtRX23.Checked) val += 1<<2;
			if(chkExtRX24.Checked) val += 1<<3;
			if(chkExtRX25.Checked) val += 1<<4;
			if(chkExtRX26.Checked) val += 1<<5;

			console.X22RX = (byte)val;
		}

		private void chkExtTX2_CheckedChanged(object sender, System.EventArgs e)
		{
			int val = 0;
			if(chkExtTX21.Checked) val += 1<<0;
			if(chkExtTX22.Checked) val += 1<<1;
			if(chkExtTX23.Checked) val += 1<<2;
			if(chkExtTX24.Checked) val += 1<<3;
			if(chkExtTX25.Checked) val += 1<<4;
			if(chkExtTX26.Checked) val += 1<<5;

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
			if(comboCATPort.Text.StartsWith("COM"))
				console.CATPort = Int32.Parse(comboCATPort.Text.Substring(3));
			console.CATPTTRTS = chkCATPTT_RTS.Checked; 
			console.CATPTTDTR = chkCATPTT_DTR.Checked;
			//console.PTTBitBangEnabled = chkCATPTTEnabled.Checked; 
			if(comboCATPTTPort.Text.StartsWith("COM"))
				console.CATPTTBitBangPort = Int32.Parse(comboCATPTTPort.Text.Substring(3)); 
			console.CATBaudRate = Convert.ToInt32((string)comboCATbaud.SelectedItem, 10); 
			console.CATParity = SDRSerialPort.stringToParity((string)comboCATparity.SelectedItem);
			console.CATDataBits = SDRSerialPort.stringToDataBits((string)comboCATdatabits.SelectedItem); 
			console.CATStopBits = SDRSerialPort.stringToStopBits((string)comboCATstopbits.SelectedItem); 

			// make sure the enabled state of bitbang ptt is correct 
			if ( chkCATPTT_RTS.Checked || chkCATPTT_DTR.Checked ) 
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
			string port = "COM"+console.CATPort.ToString();
			if(comboCATPort.Items.Contains(port))
				comboCATPort.Text = port; 
			chkCATPTT_RTS.Checked = console.CATPTTRTS;
			chkCATPTT_DTR.Checked = console.CATPTTDTR; 
			chkCATPTTEnabled.Checked = console.PTTBitBangEnabled; 
			port = "COM"+console.CATPTTBitBangPort.ToString();
			if(comboCATPTTPort.Items.Contains(port))
				comboCATPTTPort.Text = port; 

			// wjt fixme -- need to hand baudrate, parity, data, stop -- see initCATandPTTprops 
		}


		private void chkCATEnable_CheckedChanged(object sender, System.EventArgs e) 
		{
			if(initializing) return;

			if(comboCATPort.Text == "" || !comboCATPort.Text.StartsWith("COM"))
			{
				if(chkCATEnable.Focused)
				{
					MessageBox.Show("The CAT port \""+comboCATPort.Text+"\" is not a valid port.  Please select another port.");
					chkCATEnable.Checked = false;
				}
				return;
			}

			// make sure we're not using the same comm port as the bit banger 
			if ( chkCATEnable.Checked && console.PTTBitBangEnabled && 
				( comboCATPort.Text == comboCATPTTPort.Text ) )
			{
				MessageBox.Show("CAT port cannot be the same as Bit Bang Port", "Port Selection Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				chkCATEnable.Checked = false; 
			}
			
			// if enabled, disable changing of serial port 
			bool enable_sub_fields = !chkCATEnable.Checked; 
			comboCATPort.Enabled = enable_sub_fields; 

			enableCAT_HardwareFields(enable_sub_fields); 
			
			if ( chkCATEnable.Checked ) 
			{ 
				try
				{
					console.CATEnabled = true; 
				}
				catch(Exception ex)
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
				if(comboKeyerConnSecondary.Text == "CAT" && chkCATEnable.Focused)
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
			if ( console.CATPTTRTS || console.CATPTTDTR )  // if RTS or DTR is selectment, enable is ok 
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
			if(initializing) return;

			bool enable_sub_fields; 

			if(comboCATPTTPort.Text == "" || !comboCATPTTPort.Text.StartsWith("COM"))
			{
				if(chkCATPTTEnabled.Focused)
				{
					MessageBox.Show("The PTT port \""+comboCATPTTPort.Text+"\" is not a valid port.  Please select another port.");
					chkCATPTTEnabled.Checked = false;
				}				
				return;
			}

			if ( chkCATPTTEnabled.Checked && console.CATEnabled && 
				comboCATPort.Text == comboCATPTTPort.Text )  
			{
				if(chkCATPTTEnabled.Focused)
				{
					MessageBox.Show("CAT port cannot be the same as Bit Bang Port", "Port Selection Error",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					chkCATPTTEnabled.Checked = false; 
				}
				return;
			}

			console.PTTBitBangEnabled = chkCATPTTEnabled.Checked; 	
			if ( chkCATPTTEnabled.Checked ) // if it's enabled don't allow changing settings on port 
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
			if ( selection != null ) 
			{ 
				SDRSerialPort.Parity p = SDRSerialPort.stringToParity(selection); 
				console.CATParity = p; 
			}			
		}

		private void comboCATPort_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(comboCATPort.Text.StartsWith("COM"))
				console.CATPort = Int32.Parse(comboCATPort.Text.Substring(3));
		}

		private void comboCATPTTPort_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(comboCATPTTPort.Text.StartsWith("COM"))
				console.CATPTTBitBangPort = Int32.Parse(comboCATPTTPort.Text.Substring(3));
		}

		private void comboCATbaud_SelectedIndexChanged(object sender, System.EventArgs e)
		{			
			if ( comboCATbaud.SelectedIndex >= 0 ) 
				console.CATBaudRate =  Int32.Parse(comboCATbaud.Text); 			
		}

		private void comboCATdatabits_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ( comboCATdatabits.SelectedIndex >= 0 )
				console.CATDataBits = SDRSerialPort.stringToDataBits(comboCATdatabits.Text); 
		}

		private void comboCATstopbits_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ( comboCATstopbits.SelectedIndex >= 0 ) 
				console.CATStopBits = SDRSerialPort.stringToStopBits(comboCATstopbits.Text); 
		}

		private void btnCATTest_Click(object sender, System.EventArgs e)
		{
			CATTester cat = new CATTester(console);
			//this.Close();
			cat.Show();
			cat.Focus();
		}

		private void comboCATRigType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch(comboCATRigType.Text)
			{
				case "SDR-1000":
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
			if(chekTestIMD.Checked)
			{
				if(!console.PowerOn)
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
				
				if(!console.MOX)
				{
					chekTestIMD.Checked = false;
					return;
				}

				Audio.MOX = true;
				chekTestIMD.BackColor = console.ButtonSelectedColor;
				Audio.SineFreq1 = (double)udTestIMDFreq1.Value;
				Audio.SineFreq2 = (double)udTestIMDFreq2.Value;
				Audio.two_tone = true;
				Audio.CurrentAudioState1 = Audio.AudioState.SINL_COSR;
			}
			else
			{
				udTestIMDPower.Value = console.PWR;
				console.PWR = console.PreviousPWR;
				Audio.CurrentAudioState1 = Audio.AudioState.DTTSP;
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
			if(chkTestX2Pin1.Checked) val |= 1<<0;
			if(chkTestX2Pin2.Checked) val |= 1<<1;
			if(chkTestX2Pin3.Checked) val |= 1<<2;
			if(chkTestX2Pin4.Checked) val |= 1<<3;
			if(chkTestX2Pin5.Checked) val |= 1<<4;
			if(chkTestX2Pin6.Checked) val |= 1<<5;

			console.Hdw.X2 = val;
		}

		private void btnTestAudioBalStart_Click(object sender, System.EventArgs e)
		{
			if(!console.PowerOn)
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
				Audio.CurrentAudioState1 = Audio.AudioState.SINL_NOR;
				dr = MessageBox.Show("Do you hear a tone in the left channel?",
					"Tone in left channel?",
					MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Question);

				Audio.CurrentAudioState1 = Audio.AudioState.DTTSP;

				if(dr == DialogResult.No)
				{
					DialogResult dr2 = MessageBox.Show("Please double check cable and speaker connections.\n"+
						"Click OK to try again (cancel to abort).",
						"Check connections",
						MessageBoxButtons.OKCancel,
						MessageBoxIcon.Asterisk);
					if(dr2 == DialogResult.Cancel)
					{
						MessageBox.Show("Test Failed",
							"Failed",
							MessageBoxButtons.OK,
							MessageBoxIcon.Stop);
						btnTestAudioBalStart.BackColor = Color.Red;
						return;
					}
				}
				else if(dr == DialogResult.Cancel)
				{
					MessageBox.Show("Test Failed",
						"Failed",
						MessageBoxButtons.OK,
						MessageBoxIcon.Stop);
					btnTestAudioBalStart.BackColor = Color.Red;
					return;
				}
			} while(dr != DialogResult.Yes);

			do
			{
				Audio.CurrentAudioState1 = Audio.AudioState.NOL_SINR;
				dr = MessageBox.Show("Do you hear a tone in the right channel?",
					"Tone in right channel?",
					MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Question);

				Audio.CurrentAudioState1 = Audio.AudioState.DTTSP;

				if(dr == DialogResult.No)
				{
					DialogResult dr2 = MessageBox.Show("Please double check cable and speaker connections.\n"+
						"Click OK to try again (cancel to abort).",
						"Check connections",
						MessageBoxButtons.OKCancel,
						MessageBoxIcon.Asterisk);
					if(dr2 == DialogResult.Cancel)
					{
						MessageBox.Show("Test Failed",
							"Failed",
							MessageBoxButtons.OK,
							MessageBoxIcon.Stop);
						btnTestAudioBalStart.BackColor = Color.Red;
						return;
					}
				}
				else if(dr == DialogResult.Cancel)
				{
					MessageBox.Show("Test Failed",
						"Failed",
						MessageBoxButtons.OK,
						MessageBoxIcon.Stop);
					btnTestAudioBalStart.BackColor = Color.Red;
					return;
				}
			} while(dr != DialogResult.Yes);

			MessageBox.Show("Test was successful.",
				"Success",
				MessageBoxButtons.OK,
				MessageBoxIcon.Information);

			btnTestAudioBalStart.BackColor = Color.Green;
		}

		private void timer_sweep_Tick(object sender, System.EventArgs e)
		{
			if(tkbarTestGenFreq.Value >= udTestGenHigh.Value)
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
			if(timer_sweep.Enabled)
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
			Audio.SineFreq1 = tkbarTestGenFreq.Value;
		}

		private void cmboTestGenMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(cmboTestGenMode.SelectedIndex < 0) return;
			if(radTestGenInput.Checked)
			{
				switch(cmboTestGenMode.Text)
				{
					case "Off":
						Audio.CurrentInputSignal = Audio.SignalSource.SOUNDCARD;
						lblTestGenScale.Visible = false;
						udTestGenScale.Visible = false;
						break;
					case "Tone":
						Audio.CurrentInputSignal = Audio.SignalSource.SINE;
						lblTestGenScale.Visible = true;
						udTestGenScale.Visible = true;
						break;
					case "Noise":
						Audio.CurrentInputSignal = Audio.SignalSource.NOISE;
						lblTestGenScale.Visible = false;
						udTestGenScale.Visible = false;
						break;
					case "Triangle":
						Audio.CurrentInputSignal = Audio.SignalSource.TRIANGLE;
						lblTestGenScale.Visible = false;
						udTestGenScale.Visible = false;
						break;
					case "Sawtooth":
						Audio.CurrentInputSignal = Audio.SignalSource.SAWTOOTH;
						lblTestGenScale.Visible = false;
						udTestGenScale.Visible = false;
						break;
				}
			}
			else
			{
				switch(cmboTestGenMode.Text)
				{
					case "Off":
						Audio.CurrentOutputSignal = Audio.SignalSource.SOUNDCARD;
						break;
					case "Tone":
						Audio.CurrentOutputSignal = Audio.SignalSource.SINE;
						break;
					case "Noise":
						Audio.CurrentOutputSignal = Audio.SignalSource.NOISE;
						break;
					case "Triangle":
						Audio.CurrentOutputSignal = Audio.SignalSource.TRIANGLE;
						break;
					case "Sawtooth":
						Audio.CurrentOutputSignal = Audio.SignalSource.SAWTOOTH;
						break;
				}
			}
		}

		private void rbTestGenInput_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radTestGenInput.Checked)
			{
				Audio.CurrentOutputSignal = Audio.SignalSource.SOUNDCARD;
				cmboTestGenMode_SelectedIndexChanged(this, EventArgs.Empty);
			}
		}

		private void rbTestGenOutput_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radTestGenOutput.Checked)
			{
				Audio.CurrentInputSignal = Audio.SignalSource.SOUNDCARD;
				cmboTestGenMode_SelectedIndexChanged(this, EventArgs.Empty);
			}
		}

		private void updnTestGenScale_ValueChanged(object sender, System.EventArgs e)
		{
			Audio.InputSourceScale = (double)udTestGenScale.Value;
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
			for(int i=0; i<(int)udImpulseNum.Value; i++)
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

		private void btnResetAll_Click(object sender, System.EventArgs e)
		{
			DialogResult dr = MessageBox.Show("This will reset all the settings in the Setup form to their \n" +
				"default values.  Are you sure you want to do this?",
				"Warning: Complete Setup Reset?",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning);
			if(dr == DialogResult.No)
				return;

			if(console.PowerOn)
				console.PowerOn = false;

			// general tab
			//comboGeneralLPTAddr.Text = "378";
			udGeneralLPTDelay.Value = 0;
			//chkGeneralRXOnly.Checked = false;
			//chkGeneralUSBPresent.Checked = false;
			//chkGeneralRFEPresent.Checked = true;
			//chkGeneralXVTRPresent.Checked = false;
			//chkGeneralPAPresent.Checked = false;
			//chkGeneralATUPresent.Checked = false;
			chkGeneralSpurRed.Checked = true;
			chkGeneralDisablePTT.Checked = false;
			udDDSCorrection.Value = 0;
			udDDSPLLMult.Value = 1;
			udDDSIFFreq.Value = 11025;
			udGeneralCalFreq1.Value = 7;
			udGeneralCalFreq2.Value = 7;
			udGeneralCalFreq3.Value = 7;
			udGeneralCalLevel.Value = -70;
			chkGeneralSoftwareGainCorr.Checked = false;
			chkGeneralEnableX2.Checked = false;
			udGeneralX2Delay.Value = 500;
			comboGeneralProcessPriority.Text = "Normal";
			chkGeneralUpdateRelease.Checked = true;
			chkGeneralUpdateBeta.Checked = true;
            
			// audio tab
			comboAudioSoundCard.SelectedIndex = 0;
			//			if(comboAudioDriver.Items.Count > 2)
			//				comboAudioDriver.SelectedIndex = 2;
			//			else comboAudioDriver.SelectedIndex = 0;
			//			comboAudioInput.SelectedIndex = 0;
			//			comboAudioOutput.SelectedIndex = 0;
			//			comboAudioBufferSamples.SelectedIndex = 2;
			udAudioLatency1.Value = 120;
			chkAudioLatencyManual1.Checked = false;

			// display tab
			udDisplayGridMax.Value		= 0;
			udDisplayGridMin.Value		= -150;
			udDisplayGridStep.Value		= 10;
			udDisplayFPS.Value			= 15;
			udDisplayMeterDelay.Value	= 100;
			udDisplayPeakText.Value		= 500;
			udDisplayCPUMeter.Value		= 1000;
			udDisplayPhasePts.Value		= 100;
			udDisplayAVGTime.Value		= 350;	
			udDisplayWaterfallLowLevel.Value = -130;
			udDisplayWaterfallHighLevel.Value = -80;
			clrbtnWaterfallLow.Color = Color.Blue;
			clrbtnWaterfallMid.Color = Color.Red;
			clrbtnWaterfallHigh.Color = Color.Yellow;
			udDisplayMultiPeakHoldTime.Value = 1000;
			udDisplayMultiTextHoldTime.Value = 500;

			// dsp tab
			udLMSANFtaps.Value = 65;
			udLMSANFdelay.Value = 50;
			udLMSANFgain.Value = 50;
			udLMSNRtaps.Value = 65;
			udLMSNRdelay.Value = 50;
			udLMSNRgain.Value = 50;
			udDSPAGCFixedGaindB.Value = 20;
			udDSPAGCMaxGaindB.Value = 68;
			udDSPNB.Value = 20;
			udDSPNB2.Value = 15;
			udDSPCWPitch.Value = 600;
			udDSPImagePhaseRX.Value = 0;
			udDSPImageGainRX.Value = 0;
			udDSPImagePhaseTX.Value = 0;
			udDSPImageGainTX.Value = 0;
			comboDSPWindow.SelectedIndex = (int)DttSP.Window.HANNING;

			// transmit tab
			udTXFilterHigh.Value = 3000;
			udTXFilterLow.Value = 300;
			udTXFFCompression.Value = 3;

			// pa settings tab
			udPAGain160.Value = 49.0M;
			udPAGain80.Value = 48.0M;
			udPAGain60.Value = 47.4M;
			udPAGain40.Value = 46.9M;
			udPAGain30.Value = 48.9M;
			udPAGain20.Value = 48.3M;
			udPAGain17.Value = 49.3M;
			udPAGain15.Value = 48.1M;
			udPAGain12.Value = 47.4M;
			udPAGain10.Value = 43.0M;
			udAudioVoltage1.Value = 2.23M;
			udPAADC160.Value = 107;
			udPAADC80.Value = 107;
			udPAADC60.Value = 107;
			udPAADC40.Value = 106;
			udPAADC30.Value = 108;
			udPAADC20.Value = 108;
			udPAADC17.Value = 108;
			udPAADC15.Value = 108;
			udPAADC12.Value = 110;
			udPAADC10.Value = 111;

			// keyboard tab
			comboKBTuneUp1.Text		= "q";
			comboKBTuneUp2.Text		= "w";
			comboKBTuneUp3.Text		= "e";
			comboKBTuneUp4.Text		= "r";
			comboKBTuneUp5.Text		= "t";
			comboKBTuneUp6.Text		= "y";
			comboKBTuneUp7.Text		= "u";
			comboKBTuneDown1.Text	= "a";
			comboKBTuneDown2.Text	= "s";
			comboKBTuneDown3.Text	= "d";
			comboKBTuneDown4.Text	= "f";
			comboKBTuneDown5.Text	= "g";
			comboKBTuneDown6.Text	= "h";
			comboKBTuneDown7.Text	= "j";
			comboKBBandUp.Text		= "m";
			comboKBBandDown.Text	= "n";
			comboKBFilterUp.Text	= "b";
			comboKBFilterDown.Text	= "v";
			comboKBModeUp.Text		= "x";
			comboKBModeDown.Text	= "z";
			comboKBRITUp.Text		= "o";
			comboKBRITDown.Text		= "i";
			comboKBXITUp.Text		= "[";
			comboKBXITDown.Text		= "p";
			comboKBCWDot.Text		= ".";
			comboKBCWDash.Text		= "/";

			// appearance tab
			clrbtnBtnSel.Color			= Color.Yellow;
			clrbtnVFODark.Color			= Color.Olive;
			clrbtnVFOLight.Color		= Color.Yellow;
			clrbtnBandDark.Color		= Color.Green;
			clrbtnBandLight.Color		= Color.Lime;
			clrbtnPeakText.Color		= Color.DodgerBlue;
			clrbtnBackground.Color		= Color.Black;
			clrbtnGrid.Color			= Color.Purple;
			clrbtnZeroLine.Color		= Color.Red;
			clrbtnText.Color			= Color.Yellow;
			clrbtnDataLine.Color		= Color.LightGreen;
			clrbtnFilter.Color			= Color.Green;
			udDisplayLineWidth.Value	= 1;
			clrbtnMeterLeft.Color		= Color.Green;
			clrbtnMeterRight.Color		= Color.Lime;
			clrbtnVFODark.Color			= Color.Olive;
			clrbtnVFOLight.Color		= Color.Yellow;
			clrbtnBandDark.Color		= Color.Green;
			clrbtnBandLight.Color		= Color.Lime;
			clrbtnPeakText.Color		= Color.DodgerBlue;

			// tests tab
			udTestIMDFreq1.Value = 700;
			udTestIMDFreq2.Value = 1900;
			udTestIMDPower.Value = 50;
		}

		private void btnWizard_Click(object sender, System.EventArgs e)
		{
			SetupWizard w = new SetupWizard(console, comboAudioSoundCard.SelectedIndex);
			w.Show();
			w.Focus();
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			Thread t = new Thread(new ThreadStart(SaveOptions));
			t.Name = "Save Options Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Lowest;
			t.Start();
			this.Hide();
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
			t.Priority = ThreadPriority.Lowest;
			t.Start();
		}

		private void ApplyOptions()
		{
			if(saving) return;
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
			string path = Application.StartupPath;
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
			if(DB.ImportDatabase(openFileDialog1.FileName))
				MessageBox.Show("Database Imported Successfully");

			GetTxProfiles();

			GetOptions();					// load all database values
			console.GetState();				
			if(console.EQForm != null) console.EQForm.RestoreSettings();
			if(console.XVTRForm != null) console.XVTRForm.RestoreSettings();
			if(console.ProdTestForm != null) console.ProdTestForm.RestoreSettings();

			SaveOptions();					// save all database values
			console.SaveState();
			if(console.EQForm != null) console.EQForm.SaveSettings();
			if(console.XVTRForm != null) console.XVTRForm.SaveSettings();
			if(console.ProdTestForm != null) console.ProdTestForm.SaveSettings();

			udTransmitTunePower_ValueChanged(this, EventArgs.Empty);
			console.ResetMemForm();
		}

		#endregion				

		private bool shift_key = false;
		private bool ctrl_key = false;
		private bool alt_key = false;		
		private bool windows_key = false;
		private bool menu_key = false;

		private void txtKB_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			Debug.WriteLine("KeyCode: "+e.KeyCode+" KeyData: "+e.KeyData+" KeyValue: "+e.KeyValue);
			shift_key = e.Shift;
			ctrl_key = e.Control;
			alt_key = e.Alt;

			if(e.KeyCode == Keys.LWin ||
				e.KeyCode == Keys.RWin)
				windows_key = true;

			if(e.KeyCode == Keys.Apps)
				menu_key = true;

			TextBoxTS txtbox = (TextBoxTS)sender;

			string s = "";
			
			if(ctrl_key) s+="Ctrl+";
			if(alt_key) s+="Alt+";	
			if(shift_key) s+="Shift+";
			if(windows_key)
				s+="Win+";
			if(menu_key)
				s+="Menu+";

			if(e.KeyCode != Keys.ShiftKey &&
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

			if(e.KeyCode == Keys.LWin ||
				e.KeyCode == Keys.RWin)
				windows_key = false;

			if(e.KeyCode == Keys.Apps)
				menu_key = false;


			TextBoxTS txtbox = (TextBoxTS)sender;

			if(txtbox.Text.EndsWith("+"))
			{
				if(shift_key || ctrl_key || alt_key ||
					windows_key || menu_key)
				{
					string s = "";

					if(ctrl_key) s+="Ctrl+";
					if(alt_key) s+="Alt+";
					if(shift_key) s+="Shift+";
					if(windows_key)
						s+="Win+";
					if(menu_key)
						s+="Menu+";

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

		private void udDSPImageGainRX_LostFocus(object sender, EventArgs e)
		{
			udDSPImageGainRX.Value = udDSPImageGainRX.Value;
		}

		private void udDSPImagePhaseRX_LostFocus(object sender, EventArgs e)
		{
			udDSPImagePhaseRX.Value = udDSPImagePhaseRX.Value;
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

		private void udCWKeyerDeBounce_LostFocus(object sender, EventArgs e)
		{
			udCWKeyerDeBounce.Value = udCWKeyerDeBounce.Value;
		}

		private void udCWKeyerWeight_LostFocus(object sender, EventArgs e)
		{
			udCWKeyerWeight.Value = udCWKeyerWeight.Value;
		}

		private void udCWKeyerRamp_LostFocus(object sender, EventArgs e)
		{
			udCWKeyerRamp.Value = udCWKeyerRamp.Value;
		}

		private void udCWKeyerSemiBreakInDelay_LostFocus(object sender, EventArgs e)
		{
			udCWKeyerSemiBreakInDelay.Value = udCWKeyerSemiBreakInDelay.Value;
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

		private void udTXCompander_LostFocus(object sender, EventArgs e)
		{
			udTXCompander.Value = udTXCompander.Value;
		}

		private void udTXFFCompression_LostFocus(object sender, EventArgs e)
		{
			udTXFFCompression.Value = udTXFFCompression.Value;
		}

		private void udTXFilterLow_LostFocus(object sender, EventArgs e)
		{
			udTXFilterLow.Value = udTXFilterLow.Value;
		}

		private void udTXFilterHigh_LostFocus(object sender, EventArgs e)
		{
			udTXFilterHigh.Value = udTXFilterHigh.Value;
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

		private void comboMeterType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(comboMeterType.Text == "") return;
			switch(comboMeterType.Text)
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
			console.EdgeMeterBackgroundColor = clrbtnMeterEdgeBackground.Color;
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
			Display.SubRXFilterColor = clrbtnSubRXFilter.Color;
		}

		private void clrbtnSubRXZero_Changed(object sender, System.EventArgs e)
		{
			Display.SubRXZeroLine = clrbtnSubRXZero.Color;
		}

		private void chkCWKeyerMode_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkCWKeyerMode.Checked) console.Keyer.KeyerMode = 1;
			else console.Keyer.KeyerMode = 0;
		}

		private void chkBoxJanusOzyControl_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				console.OzyControl = chkBoxJanusOzyControl.Checked;
				if(chkBoxJanusOzyControl.Checked)
				{
					if(!OzySDR1kControl.Init(chkGeneralRFEPresent.Checked, chkGeneralPAPresent.Checked))
						chkBoxJanusOzyControl.Checked = false;					
				}
				else
					OzySDR1kControl.Close(); 					
				
				if(console.PowerOn)
				{
					console.PowerOn = false;
					Thread.Sleep(100);
					console.PowerOn = true;
				}				
			}
			catch(Exception ex)
			{
				System.Console.WriteLine("OzyInitException: " + ex.ToString()); 
				MessageBox.Show("Error initializing Ozy: " + ex.Message , "Ozy Init Error: ", 			
									MessageBoxButtons.OK,	
									MessageBoxIcon.Error);
				chkBoxJanusOzyControl.Checked = false;
			}
		}

		private void grpAudioCard_Enter(object sender, System.EventArgs e)
		{
		
		}
	}

	#region PADeviceInfo Helper Class

	public class PADeviceInfo
	{
		private string 	_Name;
		private int		_Index;

		public string Name
		{
			get { return _Name;	}
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
}
