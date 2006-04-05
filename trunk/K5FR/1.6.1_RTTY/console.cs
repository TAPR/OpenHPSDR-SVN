//=================================================================
// console.cs
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
//    FlexRadio Systems, 8900 Marybank Dr., Austin, TX  78750, USA.
//=================================================================

//#define INTERLEAVED
//#define SPLIT_INTERLEAVED

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using SDRSerialSupportII;
using Microsoft.DirectX;

namespace PowerSDR
{
	#region Enums

	public enum DisplayEngine
	{
		GDI_PLUS = 0,
		DIRECT_X,
	}

	public enum Model
	{
		SDR1000 = 0,
		SOFTROCK40,
		DEMO,
	}

	public enum BPFBand
	{
		NONE = -1,
		B160,
		B60,
		B20,
		B40,		
		B10,
		B6,
	}

	public enum RFELPFBand
	{
		NONE = -1,
		AUX,
		B6,
		B60,
		B20,
		B30,
		B40,
		B1210,
		B80,
		B1715,
		B160,
	}

	public enum PAFBand
	{
		NONE = 0,
		B1210,
		B1715,
		B3020,
		B6040,
		B80,
		B160,
	}

	public enum XVTRTRMode
	{
		NEGATIVE = 0,	// DEMI144-28FRS
		POSITIVE,		// 25W DEMI
		NONE,			// For Elecraft or similar XVTR
	}

	public enum SoundCard
	{
		FIRST = -1,
		SANTA_CRUZ,
		AUDIGY,
		AUDIGY_2,
		AUDIGY_2_ZS,
		EXTIGY,
		MP3_PLUS,
		DELTA_44,
		FIREBOX,
		UNSUPPORTED_CARD,
		LAST,
	}

	public enum DisplayMode
	{
		FIRST = -1,
		SPECTRUM,
		PANADAPTER,
		SCOPE,
		PHASE,
		PHASE2,
		WATERFALL,
		HISTOGRAM,
		OFF,
		LAST,
	}

	public enum AGCMode
	{
		FIRST = -1,
		FIXD,
		LONG,
		SLOW,
		MED,
		FAST,
		CUSTOM,
		LAST,
	}

	public enum MeterRXMode
	{
		FIRST = -1,
		SIGNAL_STRENGTH,
		SIGNAL_AVERAGE,
		ADC_L,
		ADC_R,
		OFF,
		LAST,
	}

	public enum MeterTXMode
	{
		FIRST = -1,
		FORWARD_POWER,
		REVERSE_POWER,
		MIC,
		EQ,
		LEVELER,
		LVL_G,
		COMP,
		CPDR,
		ALC,
		ALC_G,
		SWR,
		OFF,
		LAST,
	}

	public enum KeyerLine
	{
		NONE = 0,
		DTR,
		RTS,
	}

	public enum DateTimeMode
	{
		OFF = 0,
		LOCAL,
		UTC,
		LAST,
	}

	public enum BandPlan
	{
		IARU1 = 1,
		IARU2 = 2,
		IARU3 = 3,
	}

	public enum PreampMode
	{
		FIRST = -1,
		OFF,
		LOW,
		MED,
		HIGH,
		LAST,
	}

	public enum StatusPin
	{
		PIN_12	= 0x08,		//S3
		Dash	= 0x10,		//S4
		Dot		= 0x20,		//S5
		PA_DATA	= 0x40,		//S6
		PIN_11	= 0x80,		//S7\
	}

	public enum ATUTuneMode
	{
		FIRST = -1,
		BYPASS,
		MEMORY,
		FULL,
		LAST,
	}

	public enum DSPMode
	{
		FIRST = -1,
		LSB,
		USB,
		DSB,
		CWL,
		CWU,
		FMN,
		AM,
		DIGU,
		SPEC,
		DIGL,
		SAM,
		DRM,
		LAST,
	}

	public enum Band
	{
		FIRST = -1,
		GEN,
		B160M,
		B80M,
		B60M,
		B40M,
		B30M,
		B20M,
		B17M,
		B15M,
		B12M,
		B10M,
		B6M,
		B2M,
		WWV,
		VHF0,
		VHF1,
		VHF2,
		VHF3,
		VHF4,
		VHF5,
		VHF6,
		VHF7,
		VHF8,
		VHF9,
		VHF10,
		VHF11,
		VHF12,
		VHF13,
		LAST,
	}

	public enum Filter
	{
		FIRST = -1,
		F6000,
		F4000,
		F2600,
		F2100,
		F1000,
		F500,
		F250,
		F100,
		F50,
		F25,
		VAR1,
		VAR2,
		NONE,
		LAST,
	}

	public enum PTTMode
	{
		FIRST = -1,
		NONE,
		MANUAL,
		MIC,
		CW,
		X2,
		CAT,
		VOX,
		LAST,
	}

	public enum DisplayLabelAlignment
	{
		FIRST = -1,
		LEFT,
		CENTER,
		RIGHT,
		AUTO,
		OFF,
		LAST,
	}

	#endregion

	unsafe public class Console : System.Windows.Forms.Form
	{
		#region Variable Declarations
		// ======================================================
		// Variable Declarations
		// ======================================================

		private int main_volume;
		private int wave_volume;

		private PA19.PaStreamCallback callback1;			// audio callback for regular RX/TX
		private PA19.PaStreamCallback callbackVAC;			// audio callback for Virtual Audio Cable
		private PA19.PaStreamCallback callback4port;		// audio callback for 4port cards

		// BT 04/2005 Added serial IO event
		//public event SIOEventHandler NotifySIO;
		private SIOListenerII siolisten = null; 

		// SN 04/06 Added Digital Vars for RTTY
		private bool bFSK = false;							// is RTTY FSK checked on setup form 
		private const int FSKfreq = 2210;					// filter center freq for FSK RTTY
		private int FilterFreq;								// Filter Freq Setting

		private Thread audio_process_thread;				// fields calls to DttSP functions
		private Thread draw_display_thread;					// draws the main display 
		private Thread multimeter_thread;					// draws/updates the multimeter
		private Thread poll_ptt_thread;						// polls the PTT line on the parallel port
		private Thread poll_pa_pwr_thread;					// polls the FWD and REV power if the PA is installed

		private Mutex pa_power_mutex;
		private Mutex high_swr_mutex;

		public Setup SetupForm;
		public CW CWForm;
		public CWX CWXForm;
		public UCBForm UCBForm;
		public XVTRForm XVTRForm;
		public EQForm EQForm;

		private bool new_keyer = true;

		public Memory MemForm;
		private HW hw;										// will eventually be an array of rigs to support multiple radios

		public WaveControl WaveForm;
		public PAQualify PAQualForm;
		public ProductionTest ProdTestForm;

		private bool run_setup_wizard;						// Used to run the wizard the first time the software comes up

		private int band_160m_index;						// These band indexes are used to keep track of which
		private int band_80m_index;							// location in the bandstack was last saved/recalled
		private int band_60m_index;
		private int band_40m_index;
		private int band_30m_index;
		private int band_20m_index;
		private int band_17m_index;
		private int band_15m_index;
		private int band_12m_index;
		private int band_10m_index;
		private int band_6m_index;
		private int band_2m_index;
		private int band_wwv_index;
		private int band_gen_index;
		private int band_vhf0_index;
		private int band_vhf1_index;
		private int band_vhf2_index;
		private int band_vhf3_index;
		private int band_vhf4_index;
		private int band_vhf5_index;
		private int band_vhf6_index;
		private int band_vhf7_index;
		private int band_vhf8_index;
		private int band_vhf9_index;
		private int band_vhf10_index;
		private int band_vhf11_index;
		private int band_vhf12_index;
		private int band_vhf13_index;

		private int band_160m_register;						// These integers are the number of band stack registers
		private int band_80m_register;						// found in the database for each band
		private int band_60m_register;
		private int band_40m_register;
		private int band_30m_register;
		private int band_20m_register;
		private int band_17m_register;
		private int band_15m_register;
		private int band_12m_register;
		private int band_10m_register;
		private int band_6m_register;
		private int band_2m_register;
		private int band_wwv_register;
		private int band_gen_register;
		private int band_vhf0_register;
		private int band_vhf1_register;
		private int band_vhf2_register;
		private int band_vhf3_register;
		private int band_vhf4_register;
		private int band_vhf5_register;
		private int band_vhf6_register;
		private int band_vhf7_register;
		private int band_vhf8_register;
		private int band_vhf9_register;
		private int band_vhf10_register;
		private int band_vhf11_register;
		private int band_vhf12_register;
		private int band_vhf13_register;

		private double[] wheel_tune_list;					// A list of available tuning steps
		private int wheel_tune_index;						// An index into the above array

		private Button[] vhf_text;
		private bool was_panadapter = false;				// used to restore panadater when switching to spectrum DSP mode

		private float[] preamp_offset;						// offset values for each preamp mode in dB
		public float multimeter_cal_offset;					// multimeter calibration offset per volume setting in dB
		public float filter_size_cal_offset;				// filter offset based on DSP filter size
		public float xvtr_gain_offset;						// gain offset as entered on the xvtr form
		private int current_xvtr_index = -1;				// index of current xvtr in use

		private bool meter_data_ready;						// used to synchronize the new DSP data with the multimeter
		private float new_meter_data;						// new data for the multimeter from the DSP
		private float current_meter_data;					// current data for the multimeter
		private int meter_peak_count;						// Counter for peak hold on multimeter
		private int meter_peak_value;						// Value for peak hold on multimeter
		private float[] meter_text_history;					// Array used to output the peak power over a period of time
		private int meter_text_history_index;				// index used with above variable to do peak power

		private int pa_fwd_power;							// forward power as read by the ADC on the PA
		private int pa_rev_power;							// reverse power as read by the ADC on the PA
		private bool atu_tuning;							// true while the atu is tuning
		private Band tuned_band;							// last band that the atu was tuned on
	
		private bool shift_down;							// used to modify tuning rate
		private bool calibrating;							// true if running a calibration routine
		private bool manual_mox;							// True if the MOX button was clicked on (not PTT)		

		private DSPMode vfob_dsp_mode;						// Saves control pointer for last mode used on VFO B 
		private Filter vfob_filter;							// Saves control pointer for last filter used on VFO B
		private int vfo_char_width;							// Used to calibrate mousewheel tuning
		private int vfo_char_space;							// Used to calibrate mousewheel tuning
		private int vfo_decimal_width;						// Used to calibrate mousewheel tuning
		private int vfo_decimal_space;						// Used to calibrate mousewheel tuning
		private int vfo_pixel_offset;						// Used to calibrate mousewheel tuning
		private int vfoa_hover_digit;						// Digit for hover display
		private int vfob_hover_digit;						// Digit for hover display
		private string last_band;							// Used in bandstacking algorithm

		private DSPMode quick_save_mode;					// Quick Save Mode
		private Filter quick_save_filter;					// Quick Save Filter

		private Filter[] last_filter;						// Last filter used indexed by mode
		private int[] var1_low;								// Saved variable filter 1 low cut indexed by mode
		private int[] var1_high;							// Saved variable filter 1 high cut indexed by mode
		private int[] var2_low;								// Saved variable filter 2 low cut indexed by mode
		private int[] var2_high;							// Saved variable filter 2 high cut indexed by mode

		private string separator;							// contains the locations specific decimal separator

		private int last_filter_shift;						// VK6APH
		private int last_var1_shift;						// VK6APH 
		private int last_var2_shift;						// VK6APH
		private int pre_shift_filter_low;
		private int pre_shift_filter_high;

		private int[] sc2_cal;								// array for calibrating second sound card output
		public string[] CmdLineArgs;

		private double avg_last_ddsfreq = 0;				// Used to move the display average when tuning
		private double avg_last_dttsp_osc = 0;
		private double last_bin_shift_leftover = 0; 

		public CWKeyer2 Keyer;
		private HiPerfTimer break_in_timer;
		public double avg_vox_pwr = 0.0;

//		private bool display_running = false;
//		private bool multimeter_running = false;
//		private bool poll_ptt_running = false;
//		private bool closing = false;

		#endregion

		#region Windows Form Generated Code

		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.ButtonTS btnHidden;
		private System.Windows.Forms.TextBoxTS txtVFOAFreq;
		private System.Windows.Forms.TextBoxTS txtVFOABand;
		private System.Windows.Forms.TextBoxTS txtVFOBFreq;
		private System.Windows.Forms.PictureBox picDisplay;
		private System.Windows.Forms.GroupBoxTS grpVFOA;
		private System.Windows.Forms.GroupBoxTS grpVFOB;
		private System.Windows.Forms.TextBoxTS txtVFOBBand;
		private System.Windows.Forms.GroupBoxTS grpMode;
		private System.Windows.Forms.GroupBoxTS grpDisplay;
		private System.Windows.Forms.ButtonTS btnBandGEN;
		private System.Windows.Forms.ButtonTS btnBandWWV;
		private System.Windows.Forms.ButtonTS btnBand2;
		private System.Windows.Forms.ButtonTS btnBand6;
		private System.Windows.Forms.ButtonTS btnBand10;
		private System.Windows.Forms.ButtonTS btnBand12;
		private System.Windows.Forms.ButtonTS btnBand15;
		private System.Windows.Forms.ButtonTS btnBand17;
		private System.Windows.Forms.ButtonTS btnBand20;
		private System.Windows.Forms.ButtonTS btnBand40;
		private System.Windows.Forms.ButtonTS btnBand60;
		private System.Windows.Forms.ButtonTS btnBand80;
		private System.Windows.Forms.ButtonTS btnBand160;
		private System.Windows.Forms.CheckBoxTS chkPower;
		private System.Windows.Forms.TextBoxTS txtDisplayCurPos;
		private System.Windows.Forms.ButtonTS btnBand30;
		private System.Windows.Forms.LabelTS lblCPUMeter;
		private System.Windows.Forms.ComboBoxTS comboDisplayMode;
		private System.Windows.Forms.NumericUpDownTS udFilterLow;
		private System.Windows.Forms.NumericUpDownTS udFilterHigh;
		private System.Windows.Forms.RadioButtonTS radFilter6000;
		private System.Windows.Forms.RadioButtonTS radFilter4000;
		private System.Windows.Forms.RadioButtonTS radFilter2600;
		private System.Windows.Forms.RadioButtonTS radFilter2100;
		private System.Windows.Forms.RadioButtonTS radFilter1000;
		private System.Windows.Forms.RadioButtonTS radFilter500;
		private System.Windows.Forms.RadioButtonTS radFilter250;
		private System.Windows.Forms.RadioButtonTS radFilter100;
		private System.Windows.Forms.RadioButtonTS radFilter50;
		private System.Windows.Forms.RadioButtonTS radFilter25;
		private System.Windows.Forms.RadioButtonTS radFilterVar1;
		private System.Windows.Forms.RadioButtonTS radFilterVar2;
		private System.Windows.Forms.RadioButtonTS radModeSPEC;
		private System.Windows.Forms.RadioButtonTS radModeLSB;
		private System.Windows.Forms.RadioButtonTS radModeDIGL;
		private System.Windows.Forms.RadioButtonTS radModeCWU;
		private System.Windows.Forms.RadioButtonTS radModeDSB;
		private System.Windows.Forms.RadioButtonTS radModeSAM;
		private System.Windows.Forms.RadioButtonTS radModeAM;
		private System.Windows.Forms.RadioButtonTS radModeCWL;
		private System.Windows.Forms.RadioButtonTS radModeUSB;
		private System.Windows.Forms.RadioButtonTS radModeFMN;
		private System.Windows.Forms.RadioButtonTS radModeDRM;
		private System.Windows.Forms.GroupBoxTS grpDSP;
		private System.Windows.Forms.LabelTS lblAGC;
		private System.Windows.Forms.ComboBoxTS comboAGC;
		private System.Windows.Forms.CheckBoxTS chkNB;
		private System.Windows.Forms.CheckBoxTS chkANF;
		private System.Windows.Forms.CheckBoxTS chkNR;
		private System.Windows.Forms.CheckBoxTS chkMON;
		private System.Windows.Forms.CheckBoxTS chkTUN;
		private System.Windows.Forms.CheckBoxTS chkMOX;
		private System.Windows.Forms.NumericUpDownTS udXIT;
		private System.Windows.Forms.NumericUpDownTS udRIT;
		private System.Windows.Forms.CheckBoxTS chkMUT;
		private System.Windows.Forms.CheckBoxTS chkXIT;
		private System.Windows.Forms.CheckBoxTS chkRIT;
		private System.Windows.Forms.LabelTS lblPWR;
		private System.Windows.Forms.NumericUpDownTS udPWR;
		private System.Windows.Forms.LabelTS lblAF;
		private System.Windows.Forms.NumericUpDownTS udAF;
		private System.Windows.Forms.LabelTS lblMIC;
		private System.Windows.Forms.NumericUpDownTS udMIC;
		private System.Windows.Forms.TextBoxTS txtWheelTune;
		private System.Windows.Forms.CheckBoxTS chkBIN;
		private System.Windows.Forms.CheckBoxTS chkPipe;
		private System.Windows.Forms.GroupBoxTS grpMultimeter;
		private System.Windows.Forms.ButtonTS btnVFOSwap;
		private System.Windows.Forms.ButtonTS btnVFOBtoA;
		private System.Windows.Forms.ButtonTS btnVFOAtoB;
		private System.Windows.Forms.GroupBoxTS grpVFO;
		private System.Windows.Forms.CheckBoxTS chkVFOSplit;
		private System.Windows.Forms.GroupBoxTS grpDisplay2;
		private System.Windows.Forms.CheckBoxTS chkDisplayAVG;
		private System.Windows.Forms.GroupBoxTS grpMemory;
		private System.Windows.Forms.TextBoxTS txtMultiText;
		private System.Windows.Forms.Timer timer_cpu_meter;
		private System.Windows.Forms.ButtonTS btnMemoryRecall;
		private System.Windows.Forms.ButtonTS btnSave;
		private System.Windows.Forms.LabelTS lblFilterHigh;
		private System.Windows.Forms.LabelTS lblFilterLow;
		private System.Windows.Forms.CheckBoxTS chkMemoryScanner;
		private System.Windows.Forms.NumericUpDownTS udScannerLow;
		private System.Windows.Forms.NumericUpDownTS udScannerHigh;
		private System.Windows.Forms.NumericUpDownTS udScannerStep;
		private System.Windows.Forms.Timer timer_scanner;
		private System.Windows.Forms.LabelTS labelScannerDelay;
		private System.Windows.Forms.NumericUpDownTS udScannerDelay;
		private System.Windows.Forms.RadioButtonTS radScannerFreq;
		private System.Windows.Forms.RadioButtonTS radScannerChannel;
		private System.Windows.Forms.LabelTS lblMultiSMeter;
		private System.Windows.Forms.PictureBox picMultimeterAnalog;
		private System.Windows.Forms.PictureBox picMultiMeterDigital;
		private System.Windows.Forms.NumericUpDownTS udSquelch;
		private System.Windows.Forms.CheckBoxTS chkSquelch;
		private System.Windows.Forms.Timer timer_peak_text;
		private System.Windows.Forms.TextBoxTS txtMemoryQuick;
		private System.Windows.Forms.ButtonTS btnMemoryQuickSave;
		private System.Windows.Forms.ButtonTS btnMemoryQuickRestore;
		private System.Windows.Forms.LabelTS lblMemoryStep;
		private System.Windows.Forms.LabelTS lblMemoryHigh;
		private System.Windows.Forms.LabelTS lblMemoryLow;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.TrackBarTS tbFilterShift;
		private System.Windows.Forms.LabelTS lblFilterShift;
		private System.Windows.Forms.ButtonTS btnFilterShiftReset;
		private System.Windows.Forms.Timer timer_clock;
		private System.Windows.Forms.TextBoxTS txtDate;
		private System.Windows.Forms.Panel panelVFOAHover;
		private System.Windows.Forms.Panel panelVFOBHover;
		private System.Windows.Forms.ComboBoxTS comboMeterRXMode;
		private System.Windows.Forms.ComboBoxTS comboMeterTXMode;
		private System.Windows.Forms.GroupBoxTS grpSoundControls;
		private System.Windows.Forms.GroupBoxTS grpOptions;
		private System.Windows.Forms.GroupBoxTS grpDateTime;
		private System.Windows.Forms.ButtonTS btnXITReset;
		private System.Windows.Forms.ButtonTS btnRITReset;
		private System.Windows.Forms.ComboBoxTS comboTuneMode;
		private System.Windows.Forms.ComboBoxTS comboPreamp;
		private System.Windows.Forms.LabelTS lblPreamp;
		private System.Windows.Forms.CheckBoxTS chkDSPComp;
		private System.Windows.Forms.CheckBoxTS chkDSPNB2;
		private System.Windows.Forms.TextBoxTS txtDisplayPeak;
		private System.Windows.Forms.CheckBoxTS chkVFOLock;
		private System.Windows.Forms.TrackBarTS tbFilterWidth;
		private System.Windows.Forms.LabelTS lblFilterWidth;
		private System.Windows.Forms.GroupBoxTS grpBandHF;
		private System.Windows.Forms.ButtonTS btnBandHF;
		private System.Windows.Forms.ButtonTS btnBandVHF;
		private System.Windows.Forms.GroupBoxTS grpBandVHF;
		private System.Windows.Forms.Label lblCWSpeed;
		private System.Windows.Forms.NumericUpDownTS udCWSpeed;
		private System.Windows.Forms.ButtonTS btnIFtoVFO;
		private System.Windows.Forms.ButtonTS btnZeroBeat;
		private System.Windows.Forms.CheckBoxTS chkDSPCompander;
		private System.Windows.Forms.MenuItem mnuSetup;
		private System.Windows.Forms.MenuItem mnuCW;
		private System.Windows.Forms.MenuItem mnuWave;
		private System.Windows.Forms.MenuItem mnuUCB;
		private System.Windows.Forms.CheckBoxTS chkDisplayPeak;
		private System.Windows.Forms.MenuItem mnuEQ;
		private System.Windows.Forms.ButtonTS btnBandVHF13;
		private System.Windows.Forms.ButtonTS btnBandVHF12;
		private System.Windows.Forms.ButtonTS btnBandVHF11;
		private System.Windows.Forms.ButtonTS btnBandVHF10;
		private System.Windows.Forms.ButtonTS btnBandVHF9;
		private System.Windows.Forms.ButtonTS btnBandVHF8;
		private System.Windows.Forms.ButtonTS btnBandVHF7;
		private System.Windows.Forms.ButtonTS btnBandVHF6;
		private System.Windows.Forms.ButtonTS btnBandVHF5;
		private System.Windows.Forms.ButtonTS btnBandVHF4;
		private System.Windows.Forms.ButtonTS btnBandVHF3;
		private System.Windows.Forms.ButtonTS btnBandVHF2;
		private System.Windows.Forms.ButtonTS btnBandVHF1;
		private System.Windows.Forms.ButtonTS btnBandVHF0;
		private System.Windows.Forms.GroupBoxTS grpFilter;
		private System.Windows.Forms.MenuItem mnuXVTR;
		private System.Windows.Forms.TextBoxTS txtTime;
		private System.Windows.Forms.RadioButtonTS radModeDIGU;
		private System.Windows.Forms.MenuItem mnuCWX;
		private System.ComponentModel.IContainer components;

		#endregion

		#region Constructor and Destructor
		// ======================================================
		// Constructor and Destructor
		// ======================================================

		public Console(string[] args)
		{
//			HiPerfTimer t1 = new HiPerfTimer();
//			Debug.WriteLine("timer_freq: "+t1.GetFreq());

			CmdLineArgs = args; 

#if(INTERLEAVED)
#if(SPLIT_INTERLEAVED)
			callback1 = new PA19.PaStreamCallback(Audio.Callback1ILDI);	// Init callbacks to prevent GC
			callbackVAC = new PA19.PaStreamCallback(Audio.CallbackVACILDI);
			callback4port = new PA19.PaStreamCallback(Audio.Callback4PortILDI);
#else
			callback1 = new PA19.PaStreamCallback(Audio.Callback1IL);	// Init callbacks to prevent GC
			callbackVAC = new PA19.PaStreamCallback(Audio.CallbackVACIL);
			callback4port = new PA19.PaStreamCallback(Audio.Callback4PortIL);
#endif
#else
			callback1 = new PA19.PaStreamCallback(Audio.Callback1);	// Init callbacks to prevent GC
			callbackVAC = new PA19.PaStreamCallback(Audio.CallbackVAC);
			callback4port = new PA19.PaStreamCallback(Audio.Callback4Port);
#endif

			Splash.ShowSplashScreen();							// Start splash screen
			
			Splash.SetStatus("Initializing Components");		// Set progress point
			InitializeComponent();								// Windows Forms Generated Code

			Splash.SetStatus("Saving Mixer Values");			// Set progress point
			main_volume = Mixer.GetMainVolume(0);
			wave_volume = Mixer.GetWaveOutVolume(0);
			
			Splash.SetStatus("Initializing Database");			// Set progress point
			DB.Init();											// Initialize the database

			Splash.SetStatus("Initializing DSP");				// Set progress point
			DttSP.Init();										// Initialize the DSP processor

			//Splash.SetStatus("Initializing Parallel Port");	// Set progress point
			//Parallel.InitPortTalk();							// Initialize parallel port

			Splash.SetStatus("Initializing PortAudio");			// Set progress point
			PA19.PA_Initialize();								// Initialize the audio interface

			Splash.SetStatus("Loading Main Form");				// Set progress point
			Splash.SplashForm.Owner = this;						// So that main form will show when splash disappears
			break_in_timer = new HiPerfTimer();

			InitConsole();										// Initialize all forms and main variables
			
			Splash.SetStatus("Finished");						// Set progress point
			// Activates double buffering
			SetStyle(ControlStyles.DoubleBuffer, true);

			Splash.CloseForm();									// End splash screen

			if(File.Exists("extended.edf"))						// Check for extended capabilities file
			{
				ArrayList a = DB.GetVars("State");				
				a.Sort();
				ArrayList names = new ArrayList();
				ArrayList vals = new ArrayList();

				foreach(string s in a)
				{
					string[] str = s.Split('/');
					if(str.Length > 2)
					{
						for(int j=2; j<str.Length; j++)
							str[1] += "/"+str[j];
					}
					names.Add(str[0]);
					vals.Add(str[1]);
				}

				int i = names.BinarySearch("extended");
				if(i < 0) // If not found, prompt for logon info
				{
					LogOn LogOnForm = new LogOn(this);
					LogOnForm.ShowDialog();
				}
				else  // If found, check for existing logon info
				{
					string text = (string)vals[i];
					
					StreamReader sr = File.OpenText("extended.edf");
					string data = sr.ReadLine();
					sr.Close();

					if(text == data)
						extended = true;
					else	// Logon information found, but doesn't match
					{
						MessageBox.Show("Error reading logon information.", "Logon Error",
							MessageBoxButtons.OK, MessageBoxIcon.Error);
						LogOn LogOnForm = new LogOn(this);
						LogOnForm.ShowDialog();
					}
				}
			}

			if(run_setup_wizard)
			{
				SetupWizard w = new SetupWizard(this, (int)SoundCard.DELTA_44);
				w.ShowDialog();
				w.Focus();
			}
			
			if(multimeter_cal_offset == 0.0f)
			{
				switch(current_soundcard)
				{
					case SoundCard.SANTA_CRUZ:
						multimeter_cal_offset = -26.39952f;
						break;
					case SoundCard.AUDIGY_2_ZS:
						multimeter_cal_offset = 1.024933f;
						break;
					case SoundCard.MP3_PLUS:
						multimeter_cal_offset = -33.40224f;
						break;
					case SoundCard.EXTIGY:
						multimeter_cal_offset = -29.30501f;
						break;
					case SoundCard.DELTA_44:
						multimeter_cal_offset = -25.13887f;
						break;
					case SoundCard.FIREBOX:
						multimeter_cal_offset = -20.94611f;
						break;
					case SoundCard.UNSUPPORTED_CARD:
						multimeter_cal_offset = -52.43533f;
						break;
				}
			}

			if(display_cal_offset == 0.0f)
			{
				switch(current_soundcard)
				{
					case SoundCard.SANTA_CRUZ:
						display_cal_offset = -56.56675f;
						break;
					case SoundCard.AUDIGY_2_ZS:
						display_cal_offset = -29.20928f;
						break;
					case SoundCard.MP3_PLUS:
						display_cal_offset = -62.84578f;
						break;
					case SoundCard.EXTIGY:
						display_cal_offset = -62.099f;
						break;
					case SoundCard.DELTA_44:
						display_cal_offset = -57.467f;
						break;
					case SoundCard.FIREBOX:
						display_cal_offset = -54.019f;
						break;
					case SoundCard.UNSUPPORTED_CARD:
						display_cal_offset = -82.62103f;
						break;
				}
			}

			if(notify_on_beta || notify_on_release)
			{
				Thread t = new Thread(new ThreadStart(CheckForUpdates));
				t.IsBackground = true;
				t.Priority = ThreadPriority.Lowest;
				t.Name = "Update Check Thread";
				t.Start();
			}

			// BT 04/2005
			// instantiates the listener and raises event to initialize commander
			// if it has been opened before the console.

			//siolisten = new SIOListenerII(this);	// moved up higher to prevent problems with CW
			//                      EventArgs sioe = EventArgs.Empty;
			//                      OnNotifySIO(sioe);
		}

		protected override void Dispose( bool disposing )
		{
			USB.Exit();
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
			ExitConsole();
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Console));
			this.txtVFOAFreq = new System.Windows.Forms.TextBoxTS();
			this.grpVFOA = new System.Windows.Forms.GroupBoxTS();
			this.panelVFOAHover = new System.Windows.Forms.Panel();
			this.txtWheelTune = new System.Windows.Forms.TextBoxTS();
			this.txtVFOABand = new System.Windows.Forms.TextBoxTS();
			this.btnHidden = new System.Windows.Forms.ButtonTS();
			this.grpVFOB = new System.Windows.Forms.GroupBoxTS();
			this.panelVFOBHover = new System.Windows.Forms.Panel();
			this.txtVFOBBand = new System.Windows.Forms.TextBoxTS();
			this.txtVFOBFreq = new System.Windows.Forms.TextBoxTS();
			this.grpDisplay = new System.Windows.Forms.GroupBoxTS();
			this.txtDisplayCurPos = new System.Windows.Forms.TextBoxTS();
			this.txtDisplayPeak = new System.Windows.Forms.TextBoxTS();
			this.picDisplay = new System.Windows.Forms.PictureBox();
			this.grpBandVHF = new System.Windows.Forms.GroupBoxTS();
			this.btnBandVHF13 = new System.Windows.Forms.ButtonTS();
			this.btnBandVHF12 = new System.Windows.Forms.ButtonTS();
			this.btnBandHF = new System.Windows.Forms.ButtonTS();
			this.btnBandVHF11 = new System.Windows.Forms.ButtonTS();
			this.btnBandVHF10 = new System.Windows.Forms.ButtonTS();
			this.btnBandVHF9 = new System.Windows.Forms.ButtonTS();
			this.btnBandVHF8 = new System.Windows.Forms.ButtonTS();
			this.btnBandVHF7 = new System.Windows.Forms.ButtonTS();
			this.btnBandVHF6 = new System.Windows.Forms.ButtonTS();
			this.btnBandVHF5 = new System.Windows.Forms.ButtonTS();
			this.btnBandVHF4 = new System.Windows.Forms.ButtonTS();
			this.btnBandVHF3 = new System.Windows.Forms.ButtonTS();
			this.btnBandVHF2 = new System.Windows.Forms.ButtonTS();
			this.btnBandVHF1 = new System.Windows.Forms.ButtonTS();
			this.btnBandVHF0 = new System.Windows.Forms.ButtonTS();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.mnuSetup = new System.Windows.Forms.MenuItem();
			this.mnuCW = new System.Windows.Forms.MenuItem();
			this.mnuWave = new System.Windows.Forms.MenuItem();
			this.mnuEQ = new System.Windows.Forms.MenuItem();
			this.mnuUCB = new System.Windows.Forms.MenuItem();
			this.mnuXVTR = new System.Windows.Forms.MenuItem();
			this.mnuCWX = new System.Windows.Forms.MenuItem();
			this.grpMode = new System.Windows.Forms.GroupBoxTS();
			this.radModeAM = new System.Windows.Forms.RadioButtonTS();
			this.radModeSAM = new System.Windows.Forms.RadioButtonTS();
			this.radModeDSB = new System.Windows.Forms.RadioButtonTS();
			this.radModeCWU = new System.Windows.Forms.RadioButtonTS();
			this.radModeDIGU = new System.Windows.Forms.RadioButtonTS();
			this.radModeDIGL = new System.Windows.Forms.RadioButtonTS();
			this.radModeLSB = new System.Windows.Forms.RadioButtonTS();
			this.radModeSPEC = new System.Windows.Forms.RadioButtonTS();
			this.radModeDRM = new System.Windows.Forms.RadioButtonTS();
			this.radModeFMN = new System.Windows.Forms.RadioButtonTS();
			this.radModeUSB = new System.Windows.Forms.RadioButtonTS();
			this.radModeCWL = new System.Windows.Forms.RadioButtonTS();
			this.grpBandHF = new System.Windows.Forms.GroupBoxTS();
			this.btnBandGEN = new System.Windows.Forms.ButtonTS();
			this.btnBandWWV = new System.Windows.Forms.ButtonTS();
			this.btnBandVHF = new System.Windows.Forms.ButtonTS();
			this.btnBand2 = new System.Windows.Forms.ButtonTS();
			this.btnBand6 = new System.Windows.Forms.ButtonTS();
			this.btnBand10 = new System.Windows.Forms.ButtonTS();
			this.btnBand12 = new System.Windows.Forms.ButtonTS();
			this.btnBand15 = new System.Windows.Forms.ButtonTS();
			this.btnBand17 = new System.Windows.Forms.ButtonTS();
			this.btnBand20 = new System.Windows.Forms.ButtonTS();
			this.btnBand30 = new System.Windows.Forms.ButtonTS();
			this.btnBand40 = new System.Windows.Forms.ButtonTS();
			this.btnBand60 = new System.Windows.Forms.ButtonTS();
			this.btnBand80 = new System.Windows.Forms.ButtonTS();
			this.btnBand160 = new System.Windows.Forms.ButtonTS();
			this.grpFilter = new System.Windows.Forms.GroupBoxTS();
			this.tbFilterWidth = new System.Windows.Forms.TrackBarTS();
			this.lblFilterWidth = new System.Windows.Forms.LabelTS();
			this.btnFilterShiftReset = new System.Windows.Forms.ButtonTS();
			this.tbFilterShift = new System.Windows.Forms.TrackBarTS();
			this.lblFilterShift = new System.Windows.Forms.LabelTS();
			this.radFilter6000 = new System.Windows.Forms.RadioButtonTS();
			this.radFilter4000 = new System.Windows.Forms.RadioButtonTS();
			this.radFilter2600 = new System.Windows.Forms.RadioButtonTS();
			this.radFilter2100 = new System.Windows.Forms.RadioButtonTS();
			this.radFilter1000 = new System.Windows.Forms.RadioButtonTS();
			this.radFilter500 = new System.Windows.Forms.RadioButtonTS();
			this.radFilter250 = new System.Windows.Forms.RadioButtonTS();
			this.radFilter100 = new System.Windows.Forms.RadioButtonTS();
			this.radFilter50 = new System.Windows.Forms.RadioButtonTS();
			this.radFilter25 = new System.Windows.Forms.RadioButtonTS();
			this.radFilterVar1 = new System.Windows.Forms.RadioButtonTS();
			this.radFilterVar2 = new System.Windows.Forms.RadioButtonTS();
			this.lblFilterHigh = new System.Windows.Forms.LabelTS();
			this.lblFilterLow = new System.Windows.Forms.LabelTS();
			this.udFilterHigh = new System.Windows.Forms.NumericUpDownTS();
			this.udFilterLow = new System.Windows.Forms.NumericUpDownTS();
			this.chkPower = new System.Windows.Forms.CheckBoxTS();
			this.comboDisplayMode = new System.Windows.Forms.ComboBoxTS();
			this.lblCPUMeter = new System.Windows.Forms.LabelTS();
			this.timer_cpu_meter = new System.Windows.Forms.Timer(this.components);
			this.grpDSP = new System.Windows.Forms.GroupBoxTS();
			this.chkDSPCompander = new System.Windows.Forms.CheckBoxTS();
			this.chkDSPNB2 = new System.Windows.Forms.CheckBoxTS();
			this.chkDSPComp = new System.Windows.Forms.CheckBoxTS();
			this.chkNB = new System.Windows.Forms.CheckBoxTS();
			this.chkANF = new System.Windows.Forms.CheckBoxTS();
			this.chkNR = new System.Windows.Forms.CheckBoxTS();
			this.lblAGC = new System.Windows.Forms.LabelTS();
			this.comboAGC = new System.Windows.Forms.ComboBoxTS();
			this.chkBIN = new System.Windows.Forms.CheckBoxTS();
			this.comboPreamp = new System.Windows.Forms.ComboBoxTS();
			this.lblPreamp = new System.Windows.Forms.LabelTS();
			this.udXIT = new System.Windows.Forms.NumericUpDownTS();
			this.chkMON = new System.Windows.Forms.CheckBoxTS();
			this.chkTUN = new System.Windows.Forms.CheckBoxTS();
			this.chkMOX = new System.Windows.Forms.CheckBoxTS();
			this.udRIT = new System.Windows.Forms.NumericUpDownTS();
			this.chkMUT = new System.Windows.Forms.CheckBoxTS();
			this.chkXIT = new System.Windows.Forms.CheckBoxTS();
			this.chkRIT = new System.Windows.Forms.CheckBoxTS();
			this.lblPWR = new System.Windows.Forms.LabelTS();
			this.udPWR = new System.Windows.Forms.NumericUpDownTS();
			this.lblAF = new System.Windows.Forms.LabelTS();
			this.udAF = new System.Windows.Forms.NumericUpDownTS();
			this.lblMIC = new System.Windows.Forms.LabelTS();
			this.udMIC = new System.Windows.Forms.NumericUpDownTS();
			this.chkPipe = new System.Windows.Forms.CheckBoxTS();
			this.grpMultimeter = new System.Windows.Forms.GroupBoxTS();
			this.comboMeterTXMode = new System.Windows.Forms.ComboBoxTS();
			this.picMultiMeterDigital = new System.Windows.Forms.PictureBox();
			this.lblMultiSMeter = new System.Windows.Forms.LabelTS();
			this.comboMeterRXMode = new System.Windows.Forms.ComboBoxTS();
			this.txtMultiText = new System.Windows.Forms.TextBoxTS();
			this.picMultimeterAnalog = new System.Windows.Forms.PictureBox();
			this.grpVFO = new System.Windows.Forms.GroupBoxTS();
			this.btnZeroBeat = new System.Windows.Forms.ButtonTS();
			this.btnRITReset = new System.Windows.Forms.ButtonTS();
			this.btnXITReset = new System.Windows.Forms.ButtonTS();
			this.btnIFtoVFO = new System.Windows.Forms.ButtonTS();
			this.btnVFOSwap = new System.Windows.Forms.ButtonTS();
			this.btnVFOBtoA = new System.Windows.Forms.ButtonTS();
			this.btnVFOAtoB = new System.Windows.Forms.ButtonTS();
			this.chkVFOSplit = new System.Windows.Forms.CheckBoxTS();
			this.grpSoundControls = new System.Windows.Forms.GroupBoxTS();
			this.udSquelch = new System.Windows.Forms.NumericUpDownTS();
			this.chkSquelch = new System.Windows.Forms.CheckBoxTS();
			this.grpOptions = new System.Windows.Forms.GroupBoxTS();
			this.comboTuneMode = new System.Windows.Forms.ComboBoxTS();
			this.grpDisplay2 = new System.Windows.Forms.GroupBoxTS();
			this.chkDisplayPeak = new System.Windows.Forms.CheckBoxTS();
			this.chkDisplayAVG = new System.Windows.Forms.CheckBoxTS();
			this.grpMemory = new System.Windows.Forms.GroupBoxTS();
			this.btnMemoryQuickRestore = new System.Windows.Forms.ButtonTS();
			this.btnMemoryQuickSave = new System.Windows.Forms.ButtonTS();
			this.txtMemoryQuick = new System.Windows.Forms.TextBoxTS();
			this.labelScannerDelay = new System.Windows.Forms.LabelTS();
			this.udScannerDelay = new System.Windows.Forms.NumericUpDownTS();
			this.lblMemoryStep = new System.Windows.Forms.LabelTS();
			this.lblMemoryHigh = new System.Windows.Forms.LabelTS();
			this.lblMemoryLow = new System.Windows.Forms.LabelTS();
			this.udScannerStep = new System.Windows.Forms.NumericUpDownTS();
			this.udScannerHigh = new System.Windows.Forms.NumericUpDownTS();
			this.udScannerLow = new System.Windows.Forms.NumericUpDownTS();
			this.chkMemoryScanner = new System.Windows.Forms.CheckBoxTS();
			this.radScannerFreq = new System.Windows.Forms.RadioButtonTS();
			this.radScannerChannel = new System.Windows.Forms.RadioButtonTS();
			this.btnSave = new System.Windows.Forms.ButtonTS();
			this.btnMemoryRecall = new System.Windows.Forms.ButtonTS();
			this.timer_scanner = new System.Windows.Forms.Timer(this.components);
			this.timer_peak_text = new System.Windows.Forms.Timer(this.components);
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.grpDateTime = new System.Windows.Forms.GroupBoxTS();
			this.txtTime = new System.Windows.Forms.TextBoxTS();
			this.txtDate = new System.Windows.Forms.TextBoxTS();
			this.chkVFOLock = new System.Windows.Forms.CheckBoxTS();
			this.lblCWSpeed = new System.Windows.Forms.Label();
			this.udCWSpeed = new System.Windows.Forms.NumericUpDownTS();
			this.timer_clock = new System.Windows.Forms.Timer(this.components);
			this.grpVFOA.SuspendLayout();
			this.grpVFOB.SuspendLayout();
			this.grpDisplay.SuspendLayout();
			this.grpBandVHF.SuspendLayout();
			this.grpMode.SuspendLayout();
			this.grpBandHF.SuspendLayout();
			this.grpFilter.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbFilterWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbFilterShift)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udFilterHigh)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udFilterLow)).BeginInit();
			this.grpDSP.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udXIT)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udRIT)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPWR)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udAF)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udMIC)).BeginInit();
			this.grpMultimeter.SuspendLayout();
			this.grpVFO.SuspendLayout();
			this.grpSoundControls.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udSquelch)).BeginInit();
			this.grpOptions.SuspendLayout();
			this.grpDisplay2.SuspendLayout();
			this.grpMemory.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udScannerDelay)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udScannerStep)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udScannerHigh)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udScannerLow)).BeginInit();
			this.grpDateTime.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udCWSpeed)).BeginInit();
			this.SuspendLayout();
			// 
			// txtVFOAFreq
			// 
			this.txtVFOAFreq.AccessibleDescription = resources.GetString("txtVFOAFreq.AccessibleDescription");
			this.txtVFOAFreq.AccessibleName = resources.GetString("txtVFOAFreq.AccessibleName");
			this.txtVFOAFreq.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtVFOAFreq.Anchor")));
			this.txtVFOAFreq.AutoSize = ((bool)(resources.GetObject("txtVFOAFreq.AutoSize")));
			this.txtVFOAFreq.BackColor = System.Drawing.Color.Black;
			this.txtVFOAFreq.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtVFOAFreq.BackgroundImage")));
			this.txtVFOAFreq.Cursor = System.Windows.Forms.Cursors.Default;
			this.txtVFOAFreq.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtVFOAFreq.Dock")));
			this.txtVFOAFreq.Enabled = ((bool)(resources.GetObject("txtVFOAFreq.Enabled")));
			this.txtVFOAFreq.Font = ((System.Drawing.Font)(resources.GetObject("txtVFOAFreq.Font")));
			this.txtVFOAFreq.ForeColor = System.Drawing.Color.Olive;
			this.txtVFOAFreq.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtVFOAFreq.ImeMode")));
			this.txtVFOAFreq.Location = ((System.Drawing.Point)(resources.GetObject("txtVFOAFreq.Location")));
			this.txtVFOAFreq.MaxLength = ((int)(resources.GetObject("txtVFOAFreq.MaxLength")));
			this.txtVFOAFreq.Multiline = ((bool)(resources.GetObject("txtVFOAFreq.Multiline")));
			this.txtVFOAFreq.Name = "txtVFOAFreq";
			this.txtVFOAFreq.PasswordChar = ((char)(resources.GetObject("txtVFOAFreq.PasswordChar")));
			this.txtVFOAFreq.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtVFOAFreq.RightToLeft")));
			this.txtVFOAFreq.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtVFOAFreq.ScrollBars")));
			this.txtVFOAFreq.Size = ((System.Drawing.Size)(resources.GetObject("txtVFOAFreq.Size")));
			this.txtVFOAFreq.TabIndex = ((int)(resources.GetObject("txtVFOAFreq.TabIndex")));
			this.txtVFOAFreq.Text = resources.GetString("txtVFOAFreq.Text");
			this.txtVFOAFreq.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtVFOAFreq.TextAlign")));
			this.toolTip1.SetToolTip(this.txtVFOAFreq, resources.GetString("txtVFOAFreq.ToolTip"));
			this.txtVFOAFreq.Visible = ((bool)(resources.GetObject("txtVFOAFreq.Visible")));
			this.txtVFOAFreq.WordWrap = ((bool)(resources.GetObject("txtVFOAFreq.WordWrap")));
			this.txtVFOAFreq.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WheelTune_MouseDown);
			this.txtVFOAFreq.LostFocus += new System.EventHandler(this.txtVFOAFreq_LostFocus);
			this.txtVFOAFreq.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVFOAFreq_KeyPress);
			this.txtVFOAFreq.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtVFOAFreq_MouseMove);
			this.txtVFOAFreq.MouseLeave += new System.EventHandler(this.txtVFOAFreq_MouseLeave);
			// 
			// grpVFOA
			// 
			this.grpVFOA.AccessibleDescription = resources.GetString("grpVFOA.AccessibleDescription");
			this.grpVFOA.AccessibleName = resources.GetString("grpVFOA.AccessibleName");
			this.grpVFOA.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpVFOA.Anchor")));
			this.grpVFOA.BackColor = System.Drawing.SystemColors.Control;
			this.grpVFOA.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpVFOA.BackgroundImage")));
			this.grpVFOA.Controls.Add(this.panelVFOAHover);
			this.grpVFOA.Controls.Add(this.txtWheelTune);
			this.grpVFOA.Controls.Add(this.txtVFOABand);
			this.grpVFOA.Controls.Add(this.txtVFOAFreq);
			this.grpVFOA.Controls.Add(this.btnHidden);
			this.grpVFOA.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpVFOA.Dock")));
			this.grpVFOA.Enabled = ((bool)(resources.GetObject("grpVFOA.Enabled")));
			this.grpVFOA.Font = ((System.Drawing.Font)(resources.GetObject("grpVFOA.Font")));
			this.grpVFOA.ForeColor = System.Drawing.Color.Black;
			this.grpVFOA.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpVFOA.ImeMode")));
			this.grpVFOA.Location = ((System.Drawing.Point)(resources.GetObject("grpVFOA.Location")));
			this.grpVFOA.Name = "grpVFOA";
			this.grpVFOA.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpVFOA.RightToLeft")));
			this.grpVFOA.Size = ((System.Drawing.Size)(resources.GetObject("grpVFOA.Size")));
			this.grpVFOA.TabIndex = ((int)(resources.GetObject("grpVFOA.TabIndex")));
			this.grpVFOA.TabStop = false;
			this.grpVFOA.Text = resources.GetString("grpVFOA.Text");
			this.toolTip1.SetToolTip(this.grpVFOA, resources.GetString("grpVFOA.ToolTip"));
			this.grpVFOA.Visible = ((bool)(resources.GetObject("grpVFOA.Visible")));
			// 
			// panelVFOAHover
			// 
			this.panelVFOAHover.AccessibleDescription = resources.GetString("panelVFOAHover.AccessibleDescription");
			this.panelVFOAHover.AccessibleName = resources.GetString("panelVFOAHover.AccessibleName");
			this.panelVFOAHover.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("panelVFOAHover.Anchor")));
			this.panelVFOAHover.AutoScroll = ((bool)(resources.GetObject("panelVFOAHover.AutoScroll")));
			this.panelVFOAHover.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("panelVFOAHover.AutoScrollMargin")));
			this.panelVFOAHover.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("panelVFOAHover.AutoScrollMinSize")));
			this.panelVFOAHover.BackColor = System.Drawing.Color.Black;
			this.panelVFOAHover.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelVFOAHover.BackgroundImage")));
			this.panelVFOAHover.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("panelVFOAHover.Dock")));
			this.panelVFOAHover.Enabled = ((bool)(resources.GetObject("panelVFOAHover.Enabled")));
			this.panelVFOAHover.Font = ((System.Drawing.Font)(resources.GetObject("panelVFOAHover.Font")));
			this.panelVFOAHover.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("panelVFOAHover.ImeMode")));
			this.panelVFOAHover.Location = ((System.Drawing.Point)(resources.GetObject("panelVFOAHover.Location")));
			this.panelVFOAHover.Name = "panelVFOAHover";
			this.panelVFOAHover.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("panelVFOAHover.RightToLeft")));
			this.panelVFOAHover.Size = ((System.Drawing.Size)(resources.GetObject("panelVFOAHover.Size")));
			this.panelVFOAHover.TabIndex = ((int)(resources.GetObject("panelVFOAHover.TabIndex")));
			this.panelVFOAHover.Text = resources.GetString("panelVFOAHover.Text");
			this.toolTip1.SetToolTip(this.panelVFOAHover, resources.GetString("panelVFOAHover.ToolTip"));
			this.panelVFOAHover.Visible = ((bool)(resources.GetObject("panelVFOAHover.Visible")));
			this.panelVFOAHover.Paint += new System.Windows.Forms.PaintEventHandler(this.panelVFOAHover_Paint);
			this.panelVFOAHover.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelVFOAHover_MouseMove);
			// 
			// txtWheelTune
			// 
			this.txtWheelTune.AccessibleDescription = resources.GetString("txtWheelTune.AccessibleDescription");
			this.txtWheelTune.AccessibleName = resources.GetString("txtWheelTune.AccessibleName");
			this.txtWheelTune.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtWheelTune.Anchor")));
			this.txtWheelTune.AutoSize = ((bool)(resources.GetObject("txtWheelTune.AutoSize")));
			this.txtWheelTune.BackColor = System.Drawing.Color.Black;
			this.txtWheelTune.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtWheelTune.BackgroundImage")));
			this.txtWheelTune.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtWheelTune.Dock")));
			this.txtWheelTune.Enabled = ((bool)(resources.GetObject("txtWheelTune.Enabled")));
			this.txtWheelTune.Font = ((System.Drawing.Font)(resources.GetObject("txtWheelTune.Font")));
			this.txtWheelTune.ForeColor = System.Drawing.Color.White;
			this.txtWheelTune.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtWheelTune.ImeMode")));
			this.txtWheelTune.Location = ((System.Drawing.Point)(resources.GetObject("txtWheelTune.Location")));
			this.txtWheelTune.MaxLength = ((int)(resources.GetObject("txtWheelTune.MaxLength")));
			this.txtWheelTune.Multiline = ((bool)(resources.GetObject("txtWheelTune.Multiline")));
			this.txtWheelTune.Name = "txtWheelTune";
			this.txtWheelTune.PasswordChar = ((char)(resources.GetObject("txtWheelTune.PasswordChar")));
			this.txtWheelTune.ReadOnly = true;
			this.txtWheelTune.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtWheelTune.RightToLeft")));
			this.txtWheelTune.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtWheelTune.ScrollBars")));
			this.txtWheelTune.Size = ((System.Drawing.Size)(resources.GetObject("txtWheelTune.Size")));
			this.txtWheelTune.TabIndex = ((int)(resources.GetObject("txtWheelTune.TabIndex")));
			this.txtWheelTune.Text = resources.GetString("txtWheelTune.Text");
			this.txtWheelTune.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtWheelTune.TextAlign")));
			this.toolTip1.SetToolTip(this.txtWheelTune, resources.GetString("txtWheelTune.ToolTip"));
			this.txtWheelTune.Visible = ((bool)(resources.GetObject("txtWheelTune.Visible")));
			this.txtWheelTune.WordWrap = ((bool)(resources.GetObject("txtWheelTune.WordWrap")));
			this.txtWheelTune.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WheelTune_MouseDown);
			this.txtWheelTune.GotFocus += new System.EventHandler(this.HideFocus);
			// 
			// txtVFOABand
			// 
			this.txtVFOABand.AccessibleDescription = resources.GetString("txtVFOABand.AccessibleDescription");
			this.txtVFOABand.AccessibleName = resources.GetString("txtVFOABand.AccessibleName");
			this.txtVFOABand.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtVFOABand.Anchor")));
			this.txtVFOABand.AutoSize = ((bool)(resources.GetObject("txtVFOABand.AutoSize")));
			this.txtVFOABand.BackColor = System.Drawing.Color.Black;
			this.txtVFOABand.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtVFOABand.BackgroundImage")));
			this.txtVFOABand.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtVFOABand.Dock")));
			this.txtVFOABand.Enabled = ((bool)(resources.GetObject("txtVFOABand.Enabled")));
			this.txtVFOABand.Font = ((System.Drawing.Font)(resources.GetObject("txtVFOABand.Font")));
			this.txtVFOABand.ForeColor = System.Drawing.Color.Green;
			this.txtVFOABand.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtVFOABand.ImeMode")));
			this.txtVFOABand.Location = ((System.Drawing.Point)(resources.GetObject("txtVFOABand.Location")));
			this.txtVFOABand.MaxLength = ((int)(resources.GetObject("txtVFOABand.MaxLength")));
			this.txtVFOABand.Multiline = ((bool)(resources.GetObject("txtVFOABand.Multiline")));
			this.txtVFOABand.Name = "txtVFOABand";
			this.txtVFOABand.PasswordChar = ((char)(resources.GetObject("txtVFOABand.PasswordChar")));
			this.txtVFOABand.ReadOnly = true;
			this.txtVFOABand.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtVFOABand.RightToLeft")));
			this.txtVFOABand.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtVFOABand.ScrollBars")));
			this.txtVFOABand.Size = ((System.Drawing.Size)(resources.GetObject("txtVFOABand.Size")));
			this.txtVFOABand.TabIndex = ((int)(resources.GetObject("txtVFOABand.TabIndex")));
			this.txtVFOABand.Text = resources.GetString("txtVFOABand.Text");
			this.txtVFOABand.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtVFOABand.TextAlign")));
			this.toolTip1.SetToolTip(this.txtVFOABand, resources.GetString("txtVFOABand.ToolTip"));
			this.txtVFOABand.Visible = ((bool)(resources.GetObject("txtVFOABand.Visible")));
			this.txtVFOABand.WordWrap = ((bool)(resources.GetObject("txtVFOABand.WordWrap")));
			this.txtVFOABand.GotFocus += new System.EventHandler(this.HideFocus);
			// 
			// btnHidden
			// 
			this.btnHidden.AccessibleDescription = resources.GetString("btnHidden.AccessibleDescription");
			this.btnHidden.AccessibleName = resources.GetString("btnHidden.AccessibleName");
			this.btnHidden.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnHidden.Anchor")));
			this.btnHidden.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHidden.BackgroundImage")));
			this.btnHidden.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnHidden.Dock")));
			this.btnHidden.Enabled = ((bool)(resources.GetObject("btnHidden.Enabled")));
			this.btnHidden.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnHidden.FlatStyle")));
			this.btnHidden.Font = ((System.Drawing.Font)(resources.GetObject("btnHidden.Font")));
			this.btnHidden.Image = ((System.Drawing.Image)(resources.GetObject("btnHidden.Image")));
			this.btnHidden.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnHidden.ImageAlign")));
			this.btnHidden.ImageIndex = ((int)(resources.GetObject("btnHidden.ImageIndex")));
			this.btnHidden.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnHidden.ImeMode")));
			this.btnHidden.Location = ((System.Drawing.Point)(resources.GetObject("btnHidden.Location")));
			this.btnHidden.Name = "btnHidden";
			this.btnHidden.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnHidden.RightToLeft")));
			this.btnHidden.Size = ((System.Drawing.Size)(resources.GetObject("btnHidden.Size")));
			this.btnHidden.TabIndex = ((int)(resources.GetObject("btnHidden.TabIndex")));
			this.btnHidden.Text = resources.GetString("btnHidden.Text");
			this.btnHidden.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnHidden.TextAlign")));
			this.toolTip1.SetToolTip(this.btnHidden, resources.GetString("btnHidden.ToolTip"));
			this.btnHidden.Visible = ((bool)(resources.GetObject("btnHidden.Visible")));
			// 
			// grpVFOB
			// 
			this.grpVFOB.AccessibleDescription = resources.GetString("grpVFOB.AccessibleDescription");
			this.grpVFOB.AccessibleName = resources.GetString("grpVFOB.AccessibleName");
			this.grpVFOB.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpVFOB.Anchor")));
			this.grpVFOB.BackColor = System.Drawing.SystemColors.Control;
			this.grpVFOB.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpVFOB.BackgroundImage")));
			this.grpVFOB.Controls.Add(this.panelVFOBHover);
			this.grpVFOB.Controls.Add(this.txtVFOBBand);
			this.grpVFOB.Controls.Add(this.txtVFOBFreq);
			this.grpVFOB.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpVFOB.Dock")));
			this.grpVFOB.Enabled = ((bool)(resources.GetObject("grpVFOB.Enabled")));
			this.grpVFOB.Font = ((System.Drawing.Font)(resources.GetObject("grpVFOB.Font")));
			this.grpVFOB.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpVFOB.ImeMode")));
			this.grpVFOB.Location = ((System.Drawing.Point)(resources.GetObject("grpVFOB.Location")));
			this.grpVFOB.Name = "grpVFOB";
			this.grpVFOB.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpVFOB.RightToLeft")));
			this.grpVFOB.Size = ((System.Drawing.Size)(resources.GetObject("grpVFOB.Size")));
			this.grpVFOB.TabIndex = ((int)(resources.GetObject("grpVFOB.TabIndex")));
			this.grpVFOB.TabStop = false;
			this.grpVFOB.Text = resources.GetString("grpVFOB.Text");
			this.toolTip1.SetToolTip(this.grpVFOB, resources.GetString("grpVFOB.ToolTip"));
			this.grpVFOB.Visible = ((bool)(resources.GetObject("grpVFOB.Visible")));
			// 
			// panelVFOBHover
			// 
			this.panelVFOBHover.AccessibleDescription = resources.GetString("panelVFOBHover.AccessibleDescription");
			this.panelVFOBHover.AccessibleName = resources.GetString("panelVFOBHover.AccessibleName");
			this.panelVFOBHover.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("panelVFOBHover.Anchor")));
			this.panelVFOBHover.AutoScroll = ((bool)(resources.GetObject("panelVFOBHover.AutoScroll")));
			this.panelVFOBHover.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("panelVFOBHover.AutoScrollMargin")));
			this.panelVFOBHover.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("panelVFOBHover.AutoScrollMinSize")));
			this.panelVFOBHover.BackColor = System.Drawing.Color.Black;
			this.panelVFOBHover.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelVFOBHover.BackgroundImage")));
			this.panelVFOBHover.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("panelVFOBHover.Dock")));
			this.panelVFOBHover.Enabled = ((bool)(resources.GetObject("panelVFOBHover.Enabled")));
			this.panelVFOBHover.Font = ((System.Drawing.Font)(resources.GetObject("panelVFOBHover.Font")));
			this.panelVFOBHover.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("panelVFOBHover.ImeMode")));
			this.panelVFOBHover.Location = ((System.Drawing.Point)(resources.GetObject("panelVFOBHover.Location")));
			this.panelVFOBHover.Name = "panelVFOBHover";
			this.panelVFOBHover.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("panelVFOBHover.RightToLeft")));
			this.panelVFOBHover.Size = ((System.Drawing.Size)(resources.GetObject("panelVFOBHover.Size")));
			this.panelVFOBHover.TabIndex = ((int)(resources.GetObject("panelVFOBHover.TabIndex")));
			this.panelVFOBHover.Text = resources.GetString("panelVFOBHover.Text");
			this.toolTip1.SetToolTip(this.panelVFOBHover, resources.GetString("panelVFOBHover.ToolTip"));
			this.panelVFOBHover.Visible = ((bool)(resources.GetObject("panelVFOBHover.Visible")));
			this.panelVFOBHover.Paint += new System.Windows.Forms.PaintEventHandler(this.panelVFOBHover_Paint);
			this.panelVFOBHover.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelVFOBHover_MouseMove);
			// 
			// txtVFOBBand
			// 
			this.txtVFOBBand.AccessibleDescription = resources.GetString("txtVFOBBand.AccessibleDescription");
			this.txtVFOBBand.AccessibleName = resources.GetString("txtVFOBBand.AccessibleName");
			this.txtVFOBBand.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtVFOBBand.Anchor")));
			this.txtVFOBBand.AutoSize = ((bool)(resources.GetObject("txtVFOBBand.AutoSize")));
			this.txtVFOBBand.BackColor = System.Drawing.Color.Black;
			this.txtVFOBBand.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtVFOBBand.BackgroundImage")));
			this.txtVFOBBand.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtVFOBBand.Dock")));
			this.txtVFOBBand.Enabled = ((bool)(resources.GetObject("txtVFOBBand.Enabled")));
			this.txtVFOBBand.Font = ((System.Drawing.Font)(resources.GetObject("txtVFOBBand.Font")));
			this.txtVFOBBand.ForeColor = System.Drawing.Color.Green;
			this.txtVFOBBand.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtVFOBBand.ImeMode")));
			this.txtVFOBBand.Location = ((System.Drawing.Point)(resources.GetObject("txtVFOBBand.Location")));
			this.txtVFOBBand.MaxLength = ((int)(resources.GetObject("txtVFOBBand.MaxLength")));
			this.txtVFOBBand.Multiline = ((bool)(resources.GetObject("txtVFOBBand.Multiline")));
			this.txtVFOBBand.Name = "txtVFOBBand";
			this.txtVFOBBand.PasswordChar = ((char)(resources.GetObject("txtVFOBBand.PasswordChar")));
			this.txtVFOBBand.ReadOnly = true;
			this.txtVFOBBand.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtVFOBBand.RightToLeft")));
			this.txtVFOBBand.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtVFOBBand.ScrollBars")));
			this.txtVFOBBand.Size = ((System.Drawing.Size)(resources.GetObject("txtVFOBBand.Size")));
			this.txtVFOBBand.TabIndex = ((int)(resources.GetObject("txtVFOBBand.TabIndex")));
			this.txtVFOBBand.Text = resources.GetString("txtVFOBBand.Text");
			this.txtVFOBBand.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtVFOBBand.TextAlign")));
			this.toolTip1.SetToolTip(this.txtVFOBBand, resources.GetString("txtVFOBBand.ToolTip"));
			this.txtVFOBBand.Visible = ((bool)(resources.GetObject("txtVFOBBand.Visible")));
			this.txtVFOBBand.WordWrap = ((bool)(resources.GetObject("txtVFOBBand.WordWrap")));
			this.txtVFOBBand.GotFocus += new System.EventHandler(this.HideFocus);
			// 
			// txtVFOBFreq
			// 
			this.txtVFOBFreq.AccessibleDescription = resources.GetString("txtVFOBFreq.AccessibleDescription");
			this.txtVFOBFreq.AccessibleName = resources.GetString("txtVFOBFreq.AccessibleName");
			this.txtVFOBFreq.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtVFOBFreq.Anchor")));
			this.txtVFOBFreq.AutoSize = ((bool)(resources.GetObject("txtVFOBFreq.AutoSize")));
			this.txtVFOBFreq.BackColor = System.Drawing.Color.Black;
			this.txtVFOBFreq.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtVFOBFreq.BackgroundImage")));
			this.txtVFOBFreq.Cursor = System.Windows.Forms.Cursors.Default;
			this.txtVFOBFreq.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtVFOBFreq.Dock")));
			this.txtVFOBFreq.Enabled = ((bool)(resources.GetObject("txtVFOBFreq.Enabled")));
			this.txtVFOBFreq.Font = ((System.Drawing.Font)(resources.GetObject("txtVFOBFreq.Font")));
			this.txtVFOBFreq.ForeColor = System.Drawing.Color.Olive;
			this.txtVFOBFreq.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtVFOBFreq.ImeMode")));
			this.txtVFOBFreq.Location = ((System.Drawing.Point)(resources.GetObject("txtVFOBFreq.Location")));
			this.txtVFOBFreq.MaxLength = ((int)(resources.GetObject("txtVFOBFreq.MaxLength")));
			this.txtVFOBFreq.Multiline = ((bool)(resources.GetObject("txtVFOBFreq.Multiline")));
			this.txtVFOBFreq.Name = "txtVFOBFreq";
			this.txtVFOBFreq.PasswordChar = ((char)(resources.GetObject("txtVFOBFreq.PasswordChar")));
			this.txtVFOBFreq.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtVFOBFreq.RightToLeft")));
			this.txtVFOBFreq.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtVFOBFreq.ScrollBars")));
			this.txtVFOBFreq.Size = ((System.Drawing.Size)(resources.GetObject("txtVFOBFreq.Size")));
			this.txtVFOBFreq.TabIndex = ((int)(resources.GetObject("txtVFOBFreq.TabIndex")));
			this.txtVFOBFreq.Text = resources.GetString("txtVFOBFreq.Text");
			this.txtVFOBFreq.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtVFOBFreq.TextAlign")));
			this.toolTip1.SetToolTip(this.txtVFOBFreq, resources.GetString("txtVFOBFreq.ToolTip"));
			this.txtVFOBFreq.Visible = ((bool)(resources.GetObject("txtVFOBFreq.Visible")));
			this.txtVFOBFreq.WordWrap = ((bool)(resources.GetObject("txtVFOBFreq.WordWrap")));
			this.txtVFOBFreq.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WheelTune_MouseDown);
			this.txtVFOBFreq.LostFocus += new System.EventHandler(this.txtVFOBFreq_LostFocus);
			this.txtVFOBFreq.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVFOBFreq_KeyPress);
			this.txtVFOBFreq.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtVFOBFreq_MouseMove);
			this.txtVFOBFreq.MouseLeave += new System.EventHandler(this.txtVFOBFreq_MouseLeave);
			// 
			// grpDisplay
			// 
			this.grpDisplay.AccessibleDescription = resources.GetString("grpDisplay.AccessibleDescription");
			this.grpDisplay.AccessibleName = resources.GetString("grpDisplay.AccessibleName");
			this.grpDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpDisplay.Anchor")));
			this.grpDisplay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpDisplay.BackgroundImage")));
			this.grpDisplay.Controls.Add(this.txtDisplayCurPos);
			this.grpDisplay.Controls.Add(this.txtDisplayPeak);
			this.grpDisplay.Controls.Add(this.picDisplay);
			this.grpDisplay.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpDisplay.Dock")));
			this.grpDisplay.Enabled = ((bool)(resources.GetObject("grpDisplay.Enabled")));
			this.grpDisplay.Font = ((System.Drawing.Font)(resources.GetObject("grpDisplay.Font")));
			this.grpDisplay.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpDisplay.ImeMode")));
			this.grpDisplay.Location = ((System.Drawing.Point)(resources.GetObject("grpDisplay.Location")));
			this.grpDisplay.Name = "grpDisplay";
			this.grpDisplay.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpDisplay.RightToLeft")));
			this.grpDisplay.Size = ((System.Drawing.Size)(resources.GetObject("grpDisplay.Size")));
			this.grpDisplay.TabIndex = ((int)(resources.GetObject("grpDisplay.TabIndex")));
			this.grpDisplay.TabStop = false;
			this.grpDisplay.Text = resources.GetString("grpDisplay.Text");
			this.toolTip1.SetToolTip(this.grpDisplay, resources.GetString("grpDisplay.ToolTip"));
			this.grpDisplay.Visible = ((bool)(resources.GetObject("grpDisplay.Visible")));
			// 
			// txtDisplayCurPos
			// 
			this.txtDisplayCurPos.AccessibleDescription = resources.GetString("txtDisplayCurPos.AccessibleDescription");
			this.txtDisplayCurPos.AccessibleName = resources.GetString("txtDisplayCurPos.AccessibleName");
			this.txtDisplayCurPos.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtDisplayCurPos.Anchor")));
			this.txtDisplayCurPos.AutoSize = ((bool)(resources.GetObject("txtDisplayCurPos.AutoSize")));
			this.txtDisplayCurPos.BackColor = System.Drawing.Color.Black;
			this.txtDisplayCurPos.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtDisplayCurPos.BackgroundImage")));
			this.txtDisplayCurPos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtDisplayCurPos.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtDisplayCurPos.Dock")));
			this.txtDisplayCurPos.Enabled = ((bool)(resources.GetObject("txtDisplayCurPos.Enabled")));
			this.txtDisplayCurPos.Font = ((System.Drawing.Font)(resources.GetObject("txtDisplayCurPos.Font")));
			this.txtDisplayCurPos.ForeColor = System.Drawing.Color.DodgerBlue;
			this.txtDisplayCurPos.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtDisplayCurPos.ImeMode")));
			this.txtDisplayCurPos.Location = ((System.Drawing.Point)(resources.GetObject("txtDisplayCurPos.Location")));
			this.txtDisplayCurPos.MaxLength = ((int)(resources.GetObject("txtDisplayCurPos.MaxLength")));
			this.txtDisplayCurPos.Multiline = ((bool)(resources.GetObject("txtDisplayCurPos.Multiline")));
			this.txtDisplayCurPos.Name = "txtDisplayCurPos";
			this.txtDisplayCurPos.PasswordChar = ((char)(resources.GetObject("txtDisplayCurPos.PasswordChar")));
			this.txtDisplayCurPos.ReadOnly = true;
			this.txtDisplayCurPos.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtDisplayCurPos.RightToLeft")));
			this.txtDisplayCurPos.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtDisplayCurPos.ScrollBars")));
			this.txtDisplayCurPos.Size = ((System.Drawing.Size)(resources.GetObject("txtDisplayCurPos.Size")));
			this.txtDisplayCurPos.TabIndex = ((int)(resources.GetObject("txtDisplayCurPos.TabIndex")));
			this.txtDisplayCurPos.Text = resources.GetString("txtDisplayCurPos.Text");
			this.txtDisplayCurPos.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtDisplayCurPos.TextAlign")));
			this.toolTip1.SetToolTip(this.txtDisplayCurPos, resources.GetString("txtDisplayCurPos.ToolTip"));
			this.txtDisplayCurPos.Visible = ((bool)(resources.GetObject("txtDisplayCurPos.Visible")));
			this.txtDisplayCurPos.WordWrap = ((bool)(resources.GetObject("txtDisplayCurPos.WordWrap")));
			this.txtDisplayCurPos.GotFocus += new System.EventHandler(this.HideFocus);
			// 
			// txtDisplayPeak
			// 
			this.txtDisplayPeak.AccessibleDescription = resources.GetString("txtDisplayPeak.AccessibleDescription");
			this.txtDisplayPeak.AccessibleName = resources.GetString("txtDisplayPeak.AccessibleName");
			this.txtDisplayPeak.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtDisplayPeak.Anchor")));
			this.txtDisplayPeak.AutoSize = ((bool)(resources.GetObject("txtDisplayPeak.AutoSize")));
			this.txtDisplayPeak.BackColor = System.Drawing.Color.Black;
			this.txtDisplayPeak.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtDisplayPeak.BackgroundImage")));
			this.txtDisplayPeak.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtDisplayPeak.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtDisplayPeak.Dock")));
			this.txtDisplayPeak.Enabled = ((bool)(resources.GetObject("txtDisplayPeak.Enabled")));
			this.txtDisplayPeak.Font = ((System.Drawing.Font)(resources.GetObject("txtDisplayPeak.Font")));
			this.txtDisplayPeak.ForeColor = System.Drawing.Color.DodgerBlue;
			this.txtDisplayPeak.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtDisplayPeak.ImeMode")));
			this.txtDisplayPeak.Location = ((System.Drawing.Point)(resources.GetObject("txtDisplayPeak.Location")));
			this.txtDisplayPeak.MaxLength = ((int)(resources.GetObject("txtDisplayPeak.MaxLength")));
			this.txtDisplayPeak.Multiline = ((bool)(resources.GetObject("txtDisplayPeak.Multiline")));
			this.txtDisplayPeak.Name = "txtDisplayPeak";
			this.txtDisplayPeak.PasswordChar = ((char)(resources.GetObject("txtDisplayPeak.PasswordChar")));
			this.txtDisplayPeak.ReadOnly = true;
			this.txtDisplayPeak.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtDisplayPeak.RightToLeft")));
			this.txtDisplayPeak.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtDisplayPeak.ScrollBars")));
			this.txtDisplayPeak.Size = ((System.Drawing.Size)(resources.GetObject("txtDisplayPeak.Size")));
			this.txtDisplayPeak.TabIndex = ((int)(resources.GetObject("txtDisplayPeak.TabIndex")));
			this.txtDisplayPeak.Text = resources.GetString("txtDisplayPeak.Text");
			this.txtDisplayPeak.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtDisplayPeak.TextAlign")));
			this.toolTip1.SetToolTip(this.txtDisplayPeak, resources.GetString("txtDisplayPeak.ToolTip"));
			this.txtDisplayPeak.Visible = ((bool)(resources.GetObject("txtDisplayPeak.Visible")));
			this.txtDisplayPeak.WordWrap = ((bool)(resources.GetObject("txtDisplayPeak.WordWrap")));
			this.txtDisplayPeak.GotFocus += new System.EventHandler(this.HideFocus);
			// 
			// picDisplay
			// 
			this.picDisplay.AccessibleDescription = resources.GetString("picDisplay.AccessibleDescription");
			this.picDisplay.AccessibleName = resources.GetString("picDisplay.AccessibleName");
			this.picDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("picDisplay.Anchor")));
			this.picDisplay.BackColor = System.Drawing.Color.Black;
			this.picDisplay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picDisplay.BackgroundImage")));
			this.picDisplay.Cursor = System.Windows.Forms.Cursors.Cross;
			this.picDisplay.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("picDisplay.Dock")));
			this.picDisplay.Enabled = ((bool)(resources.GetObject("picDisplay.Enabled")));
			this.picDisplay.Font = ((System.Drawing.Font)(resources.GetObject("picDisplay.Font")));
			this.picDisplay.Image = ((System.Drawing.Image)(resources.GetObject("picDisplay.Image")));
			this.picDisplay.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("picDisplay.ImeMode")));
			this.picDisplay.Location = ((System.Drawing.Point)(resources.GetObject("picDisplay.Location")));
			this.picDisplay.Name = "picDisplay";
			this.picDisplay.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("picDisplay.RightToLeft")));
			this.picDisplay.Size = ((System.Drawing.Size)(resources.GetObject("picDisplay.Size")));
			this.picDisplay.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("picDisplay.SizeMode")));
			this.picDisplay.TabIndex = ((int)(resources.GetObject("picDisplay.TabIndex")));
			this.picDisplay.TabStop = false;
			this.picDisplay.Text = resources.GetString("picDisplay.Text");
			this.toolTip1.SetToolTip(this.picDisplay, resources.GetString("picDisplay.ToolTip"));
			this.picDisplay.Visible = ((bool)(resources.GetObject("picDisplay.Visible")));
			this.picDisplay.Resize += new System.EventHandler(this.picDisplay_Resize);
			this.picDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.picDisplay_Paint);
			this.picDisplay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picDisplay_MouseMove);
			this.picDisplay.MouseLeave += new System.EventHandler(this.picDisplay_MouseLeave);
			this.picDisplay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picDisplay_MouseDown);
			// 
			// grpBandVHF
			// 
			this.grpBandVHF.AccessibleDescription = resources.GetString("grpBandVHF.AccessibleDescription");
			this.grpBandVHF.AccessibleName = resources.GetString("grpBandVHF.AccessibleName");
			this.grpBandVHF.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpBandVHF.Anchor")));
			this.grpBandVHF.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpBandVHF.BackgroundImage")));
			this.grpBandVHF.Controls.Add(this.btnBandVHF13);
			this.grpBandVHF.Controls.Add(this.btnBandVHF12);
			this.grpBandVHF.Controls.Add(this.btnBandHF);
			this.grpBandVHF.Controls.Add(this.btnBandVHF11);
			this.grpBandVHF.Controls.Add(this.btnBandVHF10);
			this.grpBandVHF.Controls.Add(this.btnBandVHF9);
			this.grpBandVHF.Controls.Add(this.btnBandVHF8);
			this.grpBandVHF.Controls.Add(this.btnBandVHF7);
			this.grpBandVHF.Controls.Add(this.btnBandVHF6);
			this.grpBandVHF.Controls.Add(this.btnBandVHF5);
			this.grpBandVHF.Controls.Add(this.btnBandVHF4);
			this.grpBandVHF.Controls.Add(this.btnBandVHF3);
			this.grpBandVHF.Controls.Add(this.btnBandVHF2);
			this.grpBandVHF.Controls.Add(this.btnBandVHF1);
			this.grpBandVHF.Controls.Add(this.btnBandVHF0);
			this.grpBandVHF.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpBandVHF.Dock")));
			this.grpBandVHF.Enabled = ((bool)(resources.GetObject("grpBandVHF.Enabled")));
			this.grpBandVHF.Font = ((System.Drawing.Font)(resources.GetObject("grpBandVHF.Font")));
			this.grpBandVHF.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpBandVHF.ImeMode")));
			this.grpBandVHF.Location = ((System.Drawing.Point)(resources.GetObject("grpBandVHF.Location")));
			this.grpBandVHF.Name = "grpBandVHF";
			this.grpBandVHF.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpBandVHF.RightToLeft")));
			this.grpBandVHF.Size = ((System.Drawing.Size)(resources.GetObject("grpBandVHF.Size")));
			this.grpBandVHF.TabIndex = ((int)(resources.GetObject("grpBandVHF.TabIndex")));
			this.grpBandVHF.TabStop = false;
			this.grpBandVHF.Text = resources.GetString("grpBandVHF.Text");
			this.toolTip1.SetToolTip(this.grpBandVHF, resources.GetString("grpBandVHF.ToolTip"));
			this.grpBandVHF.Visible = ((bool)(resources.GetObject("grpBandVHF.Visible")));
			// 
			// btnBandVHF13
			// 
			this.btnBandVHF13.AccessibleDescription = resources.GetString("btnBandVHF13.AccessibleDescription");
			this.btnBandVHF13.AccessibleName = resources.GetString("btnBandVHF13.AccessibleName");
			this.btnBandVHF13.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandVHF13.Anchor")));
			this.btnBandVHF13.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandVHF13.BackgroundImage")));
			this.btnBandVHF13.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandVHF13.Dock")));
			this.btnBandVHF13.Enabled = ((bool)(resources.GetObject("btnBandVHF13.Enabled")));
			this.btnBandVHF13.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandVHF13.FlatStyle")));
			this.btnBandVHF13.Font = ((System.Drawing.Font)(resources.GetObject("btnBandVHF13.Font")));
			this.btnBandVHF13.Image = ((System.Drawing.Image)(resources.GetObject("btnBandVHF13.Image")));
			this.btnBandVHF13.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF13.ImageAlign")));
			this.btnBandVHF13.ImageIndex = ((int)(resources.GetObject("btnBandVHF13.ImageIndex")));
			this.btnBandVHF13.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandVHF13.ImeMode")));
			this.btnBandVHF13.Location = ((System.Drawing.Point)(resources.GetObject("btnBandVHF13.Location")));
			this.btnBandVHF13.Name = "btnBandVHF13";
			this.btnBandVHF13.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandVHF13.RightToLeft")));
			this.btnBandVHF13.Size = ((System.Drawing.Size)(resources.GetObject("btnBandVHF13.Size")));
			this.btnBandVHF13.TabIndex = ((int)(resources.GetObject("btnBandVHF13.TabIndex")));
			this.btnBandVHF13.Text = resources.GetString("btnBandVHF13.Text");
			this.btnBandVHF13.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF13.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandVHF13, resources.GetString("btnBandVHF13.ToolTip"));
			this.btnBandVHF13.Visible = ((bool)(resources.GetObject("btnBandVHF13.Visible")));
			this.btnBandVHF13.Click += new System.EventHandler(this.btnBandVHF0_Click);
			// 
			// btnBandVHF12
			// 
			this.btnBandVHF12.AccessibleDescription = resources.GetString("btnBandVHF12.AccessibleDescription");
			this.btnBandVHF12.AccessibleName = resources.GetString("btnBandVHF12.AccessibleName");
			this.btnBandVHF12.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandVHF12.Anchor")));
			this.btnBandVHF12.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandVHF12.BackgroundImage")));
			this.btnBandVHF12.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandVHF12.Dock")));
			this.btnBandVHF12.Enabled = ((bool)(resources.GetObject("btnBandVHF12.Enabled")));
			this.btnBandVHF12.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandVHF12.FlatStyle")));
			this.btnBandVHF12.Font = ((System.Drawing.Font)(resources.GetObject("btnBandVHF12.Font")));
			this.btnBandVHF12.Image = ((System.Drawing.Image)(resources.GetObject("btnBandVHF12.Image")));
			this.btnBandVHF12.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF12.ImageAlign")));
			this.btnBandVHF12.ImageIndex = ((int)(resources.GetObject("btnBandVHF12.ImageIndex")));
			this.btnBandVHF12.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandVHF12.ImeMode")));
			this.btnBandVHF12.Location = ((System.Drawing.Point)(resources.GetObject("btnBandVHF12.Location")));
			this.btnBandVHF12.Name = "btnBandVHF12";
			this.btnBandVHF12.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandVHF12.RightToLeft")));
			this.btnBandVHF12.Size = ((System.Drawing.Size)(resources.GetObject("btnBandVHF12.Size")));
			this.btnBandVHF12.TabIndex = ((int)(resources.GetObject("btnBandVHF12.TabIndex")));
			this.btnBandVHF12.Text = resources.GetString("btnBandVHF12.Text");
			this.btnBandVHF12.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF12.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandVHF12, resources.GetString("btnBandVHF12.ToolTip"));
			this.btnBandVHF12.Visible = ((bool)(resources.GetObject("btnBandVHF12.Visible")));
			this.btnBandVHF12.Click += new System.EventHandler(this.btnBandVHF0_Click);
			// 
			// btnBandHF
			// 
			this.btnBandHF.AccessibleDescription = resources.GetString("btnBandHF.AccessibleDescription");
			this.btnBandHF.AccessibleName = resources.GetString("btnBandHF.AccessibleName");
			this.btnBandHF.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandHF.Anchor")));
			this.btnBandHF.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandHF.BackgroundImage")));
			this.btnBandHF.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandHF.Dock")));
			this.btnBandHF.Enabled = ((bool)(resources.GetObject("btnBandHF.Enabled")));
			this.btnBandHF.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandHF.FlatStyle")));
			this.btnBandHF.Font = ((System.Drawing.Font)(resources.GetObject("btnBandHF.Font")));
			this.btnBandHF.Image = ((System.Drawing.Image)(resources.GetObject("btnBandHF.Image")));
			this.btnBandHF.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandHF.ImageAlign")));
			this.btnBandHF.ImageIndex = ((int)(resources.GetObject("btnBandHF.ImageIndex")));
			this.btnBandHF.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandHF.ImeMode")));
			this.btnBandHF.Location = ((System.Drawing.Point)(resources.GetObject("btnBandHF.Location")));
			this.btnBandHF.Name = "btnBandHF";
			this.btnBandHF.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandHF.RightToLeft")));
			this.btnBandHF.Size = ((System.Drawing.Size)(resources.GetObject("btnBandHF.Size")));
			this.btnBandHF.TabIndex = ((int)(resources.GetObject("btnBandHF.TabIndex")));
			this.btnBandHF.Text = resources.GetString("btnBandHF.Text");
			this.btnBandHF.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandHF.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandHF, resources.GetString("btnBandHF.ToolTip"));
			this.btnBandHF.Visible = ((bool)(resources.GetObject("btnBandHF.Visible")));
			this.btnBandHF.Click += new System.EventHandler(this.btnBandHF_Click);
			// 
			// btnBandVHF11
			// 
			this.btnBandVHF11.AccessibleDescription = resources.GetString("btnBandVHF11.AccessibleDescription");
			this.btnBandVHF11.AccessibleName = resources.GetString("btnBandVHF11.AccessibleName");
			this.btnBandVHF11.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandVHF11.Anchor")));
			this.btnBandVHF11.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandVHF11.BackgroundImage")));
			this.btnBandVHF11.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandVHF11.Dock")));
			this.btnBandVHF11.Enabled = ((bool)(resources.GetObject("btnBandVHF11.Enabled")));
			this.btnBandVHF11.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandVHF11.FlatStyle")));
			this.btnBandVHF11.Font = ((System.Drawing.Font)(resources.GetObject("btnBandVHF11.Font")));
			this.btnBandVHF11.Image = ((System.Drawing.Image)(resources.GetObject("btnBandVHF11.Image")));
			this.btnBandVHF11.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF11.ImageAlign")));
			this.btnBandVHF11.ImageIndex = ((int)(resources.GetObject("btnBandVHF11.ImageIndex")));
			this.btnBandVHF11.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandVHF11.ImeMode")));
			this.btnBandVHF11.Location = ((System.Drawing.Point)(resources.GetObject("btnBandVHF11.Location")));
			this.btnBandVHF11.Name = "btnBandVHF11";
			this.btnBandVHF11.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandVHF11.RightToLeft")));
			this.btnBandVHF11.Size = ((System.Drawing.Size)(resources.GetObject("btnBandVHF11.Size")));
			this.btnBandVHF11.TabIndex = ((int)(resources.GetObject("btnBandVHF11.TabIndex")));
			this.btnBandVHF11.Text = resources.GetString("btnBandVHF11.Text");
			this.btnBandVHF11.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF11.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandVHF11, resources.GetString("btnBandVHF11.ToolTip"));
			this.btnBandVHF11.Visible = ((bool)(resources.GetObject("btnBandVHF11.Visible")));
			this.btnBandVHF11.Click += new System.EventHandler(this.btnBandVHF0_Click);
			// 
			// btnBandVHF10
			// 
			this.btnBandVHF10.AccessibleDescription = resources.GetString("btnBandVHF10.AccessibleDescription");
			this.btnBandVHF10.AccessibleName = resources.GetString("btnBandVHF10.AccessibleName");
			this.btnBandVHF10.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandVHF10.Anchor")));
			this.btnBandVHF10.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandVHF10.BackgroundImage")));
			this.btnBandVHF10.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandVHF10.Dock")));
			this.btnBandVHF10.Enabled = ((bool)(resources.GetObject("btnBandVHF10.Enabled")));
			this.btnBandVHF10.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandVHF10.FlatStyle")));
			this.btnBandVHF10.Font = ((System.Drawing.Font)(resources.GetObject("btnBandVHF10.Font")));
			this.btnBandVHF10.Image = ((System.Drawing.Image)(resources.GetObject("btnBandVHF10.Image")));
			this.btnBandVHF10.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF10.ImageAlign")));
			this.btnBandVHF10.ImageIndex = ((int)(resources.GetObject("btnBandVHF10.ImageIndex")));
			this.btnBandVHF10.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandVHF10.ImeMode")));
			this.btnBandVHF10.Location = ((System.Drawing.Point)(resources.GetObject("btnBandVHF10.Location")));
			this.btnBandVHF10.Name = "btnBandVHF10";
			this.btnBandVHF10.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandVHF10.RightToLeft")));
			this.btnBandVHF10.Size = ((System.Drawing.Size)(resources.GetObject("btnBandVHF10.Size")));
			this.btnBandVHF10.TabIndex = ((int)(resources.GetObject("btnBandVHF10.TabIndex")));
			this.btnBandVHF10.Text = resources.GetString("btnBandVHF10.Text");
			this.btnBandVHF10.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF10.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandVHF10, resources.GetString("btnBandVHF10.ToolTip"));
			this.btnBandVHF10.Visible = ((bool)(resources.GetObject("btnBandVHF10.Visible")));
			this.btnBandVHF10.Click += new System.EventHandler(this.btnBandVHF0_Click);
			// 
			// btnBandVHF9
			// 
			this.btnBandVHF9.AccessibleDescription = resources.GetString("btnBandVHF9.AccessibleDescription");
			this.btnBandVHF9.AccessibleName = resources.GetString("btnBandVHF9.AccessibleName");
			this.btnBandVHF9.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandVHF9.Anchor")));
			this.btnBandVHF9.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandVHF9.BackgroundImage")));
			this.btnBandVHF9.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandVHF9.Dock")));
			this.btnBandVHF9.Enabled = ((bool)(resources.GetObject("btnBandVHF9.Enabled")));
			this.btnBandVHF9.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandVHF9.FlatStyle")));
			this.btnBandVHF9.Font = ((System.Drawing.Font)(resources.GetObject("btnBandVHF9.Font")));
			this.btnBandVHF9.Image = ((System.Drawing.Image)(resources.GetObject("btnBandVHF9.Image")));
			this.btnBandVHF9.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF9.ImageAlign")));
			this.btnBandVHF9.ImageIndex = ((int)(resources.GetObject("btnBandVHF9.ImageIndex")));
			this.btnBandVHF9.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandVHF9.ImeMode")));
			this.btnBandVHF9.Location = ((System.Drawing.Point)(resources.GetObject("btnBandVHF9.Location")));
			this.btnBandVHF9.Name = "btnBandVHF9";
			this.btnBandVHF9.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandVHF9.RightToLeft")));
			this.btnBandVHF9.Size = ((System.Drawing.Size)(resources.GetObject("btnBandVHF9.Size")));
			this.btnBandVHF9.TabIndex = ((int)(resources.GetObject("btnBandVHF9.TabIndex")));
			this.btnBandVHF9.Text = resources.GetString("btnBandVHF9.Text");
			this.btnBandVHF9.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF9.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandVHF9, resources.GetString("btnBandVHF9.ToolTip"));
			this.btnBandVHF9.Visible = ((bool)(resources.GetObject("btnBandVHF9.Visible")));
			this.btnBandVHF9.Click += new System.EventHandler(this.btnBandVHF0_Click);
			// 
			// btnBandVHF8
			// 
			this.btnBandVHF8.AccessibleDescription = resources.GetString("btnBandVHF8.AccessibleDescription");
			this.btnBandVHF8.AccessibleName = resources.GetString("btnBandVHF8.AccessibleName");
			this.btnBandVHF8.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandVHF8.Anchor")));
			this.btnBandVHF8.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandVHF8.BackgroundImage")));
			this.btnBandVHF8.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandVHF8.Dock")));
			this.btnBandVHF8.Enabled = ((bool)(resources.GetObject("btnBandVHF8.Enabled")));
			this.btnBandVHF8.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandVHF8.FlatStyle")));
			this.btnBandVHF8.Font = ((System.Drawing.Font)(resources.GetObject("btnBandVHF8.Font")));
			this.btnBandVHF8.Image = ((System.Drawing.Image)(resources.GetObject("btnBandVHF8.Image")));
			this.btnBandVHF8.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF8.ImageAlign")));
			this.btnBandVHF8.ImageIndex = ((int)(resources.GetObject("btnBandVHF8.ImageIndex")));
			this.btnBandVHF8.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandVHF8.ImeMode")));
			this.btnBandVHF8.Location = ((System.Drawing.Point)(resources.GetObject("btnBandVHF8.Location")));
			this.btnBandVHF8.Name = "btnBandVHF8";
			this.btnBandVHF8.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandVHF8.RightToLeft")));
			this.btnBandVHF8.Size = ((System.Drawing.Size)(resources.GetObject("btnBandVHF8.Size")));
			this.btnBandVHF8.TabIndex = ((int)(resources.GetObject("btnBandVHF8.TabIndex")));
			this.btnBandVHF8.Text = resources.GetString("btnBandVHF8.Text");
			this.btnBandVHF8.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF8.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandVHF8, resources.GetString("btnBandVHF8.ToolTip"));
			this.btnBandVHF8.Visible = ((bool)(resources.GetObject("btnBandVHF8.Visible")));
			this.btnBandVHF8.Click += new System.EventHandler(this.btnBandVHF0_Click);
			// 
			// btnBandVHF7
			// 
			this.btnBandVHF7.AccessibleDescription = resources.GetString("btnBandVHF7.AccessibleDescription");
			this.btnBandVHF7.AccessibleName = resources.GetString("btnBandVHF7.AccessibleName");
			this.btnBandVHF7.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandVHF7.Anchor")));
			this.btnBandVHF7.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandVHF7.BackgroundImage")));
			this.btnBandVHF7.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandVHF7.Dock")));
			this.btnBandVHF7.Enabled = ((bool)(resources.GetObject("btnBandVHF7.Enabled")));
			this.btnBandVHF7.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandVHF7.FlatStyle")));
			this.btnBandVHF7.Font = ((System.Drawing.Font)(resources.GetObject("btnBandVHF7.Font")));
			this.btnBandVHF7.Image = ((System.Drawing.Image)(resources.GetObject("btnBandVHF7.Image")));
			this.btnBandVHF7.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF7.ImageAlign")));
			this.btnBandVHF7.ImageIndex = ((int)(resources.GetObject("btnBandVHF7.ImageIndex")));
			this.btnBandVHF7.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandVHF7.ImeMode")));
			this.btnBandVHF7.Location = ((System.Drawing.Point)(resources.GetObject("btnBandVHF7.Location")));
			this.btnBandVHF7.Name = "btnBandVHF7";
			this.btnBandVHF7.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandVHF7.RightToLeft")));
			this.btnBandVHF7.Size = ((System.Drawing.Size)(resources.GetObject("btnBandVHF7.Size")));
			this.btnBandVHF7.TabIndex = ((int)(resources.GetObject("btnBandVHF7.TabIndex")));
			this.btnBandVHF7.Text = resources.GetString("btnBandVHF7.Text");
			this.btnBandVHF7.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF7.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandVHF7, resources.GetString("btnBandVHF7.ToolTip"));
			this.btnBandVHF7.Visible = ((bool)(resources.GetObject("btnBandVHF7.Visible")));
			this.btnBandVHF7.Click += new System.EventHandler(this.btnBandVHF0_Click);
			// 
			// btnBandVHF6
			// 
			this.btnBandVHF6.AccessibleDescription = resources.GetString("btnBandVHF6.AccessibleDescription");
			this.btnBandVHF6.AccessibleName = resources.GetString("btnBandVHF6.AccessibleName");
			this.btnBandVHF6.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandVHF6.Anchor")));
			this.btnBandVHF6.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandVHF6.BackgroundImage")));
			this.btnBandVHF6.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandVHF6.Dock")));
			this.btnBandVHF6.Enabled = ((bool)(resources.GetObject("btnBandVHF6.Enabled")));
			this.btnBandVHF6.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandVHF6.FlatStyle")));
			this.btnBandVHF6.Font = ((System.Drawing.Font)(resources.GetObject("btnBandVHF6.Font")));
			this.btnBandVHF6.Image = ((System.Drawing.Image)(resources.GetObject("btnBandVHF6.Image")));
			this.btnBandVHF6.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF6.ImageAlign")));
			this.btnBandVHF6.ImageIndex = ((int)(resources.GetObject("btnBandVHF6.ImageIndex")));
			this.btnBandVHF6.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandVHF6.ImeMode")));
			this.btnBandVHF6.Location = ((System.Drawing.Point)(resources.GetObject("btnBandVHF6.Location")));
			this.btnBandVHF6.Name = "btnBandVHF6";
			this.btnBandVHF6.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandVHF6.RightToLeft")));
			this.btnBandVHF6.Size = ((System.Drawing.Size)(resources.GetObject("btnBandVHF6.Size")));
			this.btnBandVHF6.TabIndex = ((int)(resources.GetObject("btnBandVHF6.TabIndex")));
			this.btnBandVHF6.Text = resources.GetString("btnBandVHF6.Text");
			this.btnBandVHF6.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF6.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandVHF6, resources.GetString("btnBandVHF6.ToolTip"));
			this.btnBandVHF6.Visible = ((bool)(resources.GetObject("btnBandVHF6.Visible")));
			this.btnBandVHF6.Click += new System.EventHandler(this.btnBandVHF0_Click);
			// 
			// btnBandVHF5
			// 
			this.btnBandVHF5.AccessibleDescription = resources.GetString("btnBandVHF5.AccessibleDescription");
			this.btnBandVHF5.AccessibleName = resources.GetString("btnBandVHF5.AccessibleName");
			this.btnBandVHF5.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandVHF5.Anchor")));
			this.btnBandVHF5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandVHF5.BackgroundImage")));
			this.btnBandVHF5.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandVHF5.Dock")));
			this.btnBandVHF5.Enabled = ((bool)(resources.GetObject("btnBandVHF5.Enabled")));
			this.btnBandVHF5.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandVHF5.FlatStyle")));
			this.btnBandVHF5.Font = ((System.Drawing.Font)(resources.GetObject("btnBandVHF5.Font")));
			this.btnBandVHF5.Image = ((System.Drawing.Image)(resources.GetObject("btnBandVHF5.Image")));
			this.btnBandVHF5.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF5.ImageAlign")));
			this.btnBandVHF5.ImageIndex = ((int)(resources.GetObject("btnBandVHF5.ImageIndex")));
			this.btnBandVHF5.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandVHF5.ImeMode")));
			this.btnBandVHF5.Location = ((System.Drawing.Point)(resources.GetObject("btnBandVHF5.Location")));
			this.btnBandVHF5.Name = "btnBandVHF5";
			this.btnBandVHF5.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandVHF5.RightToLeft")));
			this.btnBandVHF5.Size = ((System.Drawing.Size)(resources.GetObject("btnBandVHF5.Size")));
			this.btnBandVHF5.TabIndex = ((int)(resources.GetObject("btnBandVHF5.TabIndex")));
			this.btnBandVHF5.Text = resources.GetString("btnBandVHF5.Text");
			this.btnBandVHF5.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF5.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandVHF5, resources.GetString("btnBandVHF5.ToolTip"));
			this.btnBandVHF5.Visible = ((bool)(resources.GetObject("btnBandVHF5.Visible")));
			this.btnBandVHF5.Click += new System.EventHandler(this.btnBandVHF0_Click);
			// 
			// btnBandVHF4
			// 
			this.btnBandVHF4.AccessibleDescription = resources.GetString("btnBandVHF4.AccessibleDescription");
			this.btnBandVHF4.AccessibleName = resources.GetString("btnBandVHF4.AccessibleName");
			this.btnBandVHF4.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandVHF4.Anchor")));
			this.btnBandVHF4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandVHF4.BackgroundImage")));
			this.btnBandVHF4.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandVHF4.Dock")));
			this.btnBandVHF4.Enabled = ((bool)(resources.GetObject("btnBandVHF4.Enabled")));
			this.btnBandVHF4.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandVHF4.FlatStyle")));
			this.btnBandVHF4.Font = ((System.Drawing.Font)(resources.GetObject("btnBandVHF4.Font")));
			this.btnBandVHF4.Image = ((System.Drawing.Image)(resources.GetObject("btnBandVHF4.Image")));
			this.btnBandVHF4.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF4.ImageAlign")));
			this.btnBandVHF4.ImageIndex = ((int)(resources.GetObject("btnBandVHF4.ImageIndex")));
			this.btnBandVHF4.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandVHF4.ImeMode")));
			this.btnBandVHF4.Location = ((System.Drawing.Point)(resources.GetObject("btnBandVHF4.Location")));
			this.btnBandVHF4.Name = "btnBandVHF4";
			this.btnBandVHF4.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandVHF4.RightToLeft")));
			this.btnBandVHF4.Size = ((System.Drawing.Size)(resources.GetObject("btnBandVHF4.Size")));
			this.btnBandVHF4.TabIndex = ((int)(resources.GetObject("btnBandVHF4.TabIndex")));
			this.btnBandVHF4.Text = resources.GetString("btnBandVHF4.Text");
			this.btnBandVHF4.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF4.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandVHF4, resources.GetString("btnBandVHF4.ToolTip"));
			this.btnBandVHF4.Visible = ((bool)(resources.GetObject("btnBandVHF4.Visible")));
			this.btnBandVHF4.Click += new System.EventHandler(this.btnBandVHF0_Click);
			// 
			// btnBandVHF3
			// 
			this.btnBandVHF3.AccessibleDescription = resources.GetString("btnBandVHF3.AccessibleDescription");
			this.btnBandVHF3.AccessibleName = resources.GetString("btnBandVHF3.AccessibleName");
			this.btnBandVHF3.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandVHF3.Anchor")));
			this.btnBandVHF3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandVHF3.BackgroundImage")));
			this.btnBandVHF3.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandVHF3.Dock")));
			this.btnBandVHF3.Enabled = ((bool)(resources.GetObject("btnBandVHF3.Enabled")));
			this.btnBandVHF3.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandVHF3.FlatStyle")));
			this.btnBandVHF3.Font = ((System.Drawing.Font)(resources.GetObject("btnBandVHF3.Font")));
			this.btnBandVHF3.Image = ((System.Drawing.Image)(resources.GetObject("btnBandVHF3.Image")));
			this.btnBandVHF3.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF3.ImageAlign")));
			this.btnBandVHF3.ImageIndex = ((int)(resources.GetObject("btnBandVHF3.ImageIndex")));
			this.btnBandVHF3.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandVHF3.ImeMode")));
			this.btnBandVHF3.Location = ((System.Drawing.Point)(resources.GetObject("btnBandVHF3.Location")));
			this.btnBandVHF3.Name = "btnBandVHF3";
			this.btnBandVHF3.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandVHF3.RightToLeft")));
			this.btnBandVHF3.Size = ((System.Drawing.Size)(resources.GetObject("btnBandVHF3.Size")));
			this.btnBandVHF3.TabIndex = ((int)(resources.GetObject("btnBandVHF3.TabIndex")));
			this.btnBandVHF3.Text = resources.GetString("btnBandVHF3.Text");
			this.btnBandVHF3.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF3.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandVHF3, resources.GetString("btnBandVHF3.ToolTip"));
			this.btnBandVHF3.Visible = ((bool)(resources.GetObject("btnBandVHF3.Visible")));
			this.btnBandVHF3.Click += new System.EventHandler(this.btnBandVHF0_Click);
			// 
			// btnBandVHF2
			// 
			this.btnBandVHF2.AccessibleDescription = resources.GetString("btnBandVHF2.AccessibleDescription");
			this.btnBandVHF2.AccessibleName = resources.GetString("btnBandVHF2.AccessibleName");
			this.btnBandVHF2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandVHF2.Anchor")));
			this.btnBandVHF2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandVHF2.BackgroundImage")));
			this.btnBandVHF2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandVHF2.Dock")));
			this.btnBandVHF2.Enabled = ((bool)(resources.GetObject("btnBandVHF2.Enabled")));
			this.btnBandVHF2.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandVHF2.FlatStyle")));
			this.btnBandVHF2.Font = ((System.Drawing.Font)(resources.GetObject("btnBandVHF2.Font")));
			this.btnBandVHF2.Image = ((System.Drawing.Image)(resources.GetObject("btnBandVHF2.Image")));
			this.btnBandVHF2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF2.ImageAlign")));
			this.btnBandVHF2.ImageIndex = ((int)(resources.GetObject("btnBandVHF2.ImageIndex")));
			this.btnBandVHF2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandVHF2.ImeMode")));
			this.btnBandVHF2.Location = ((System.Drawing.Point)(resources.GetObject("btnBandVHF2.Location")));
			this.btnBandVHF2.Name = "btnBandVHF2";
			this.btnBandVHF2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandVHF2.RightToLeft")));
			this.btnBandVHF2.Size = ((System.Drawing.Size)(resources.GetObject("btnBandVHF2.Size")));
			this.btnBandVHF2.TabIndex = ((int)(resources.GetObject("btnBandVHF2.TabIndex")));
			this.btnBandVHF2.Text = resources.GetString("btnBandVHF2.Text");
			this.btnBandVHF2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF2.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandVHF2, resources.GetString("btnBandVHF2.ToolTip"));
			this.btnBandVHF2.Visible = ((bool)(resources.GetObject("btnBandVHF2.Visible")));
			this.btnBandVHF2.Click += new System.EventHandler(this.btnBandVHF0_Click);
			// 
			// btnBandVHF1
			// 
			this.btnBandVHF1.AccessibleDescription = resources.GetString("btnBandVHF1.AccessibleDescription");
			this.btnBandVHF1.AccessibleName = resources.GetString("btnBandVHF1.AccessibleName");
			this.btnBandVHF1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandVHF1.Anchor")));
			this.btnBandVHF1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandVHF1.BackgroundImage")));
			this.btnBandVHF1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandVHF1.Dock")));
			this.btnBandVHF1.Enabled = ((bool)(resources.GetObject("btnBandVHF1.Enabled")));
			this.btnBandVHF1.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandVHF1.FlatStyle")));
			this.btnBandVHF1.Font = ((System.Drawing.Font)(resources.GetObject("btnBandVHF1.Font")));
			this.btnBandVHF1.Image = ((System.Drawing.Image)(resources.GetObject("btnBandVHF1.Image")));
			this.btnBandVHF1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF1.ImageAlign")));
			this.btnBandVHF1.ImageIndex = ((int)(resources.GetObject("btnBandVHF1.ImageIndex")));
			this.btnBandVHF1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandVHF1.ImeMode")));
			this.btnBandVHF1.Location = ((System.Drawing.Point)(resources.GetObject("btnBandVHF1.Location")));
			this.btnBandVHF1.Name = "btnBandVHF1";
			this.btnBandVHF1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandVHF1.RightToLeft")));
			this.btnBandVHF1.Size = ((System.Drawing.Size)(resources.GetObject("btnBandVHF1.Size")));
			this.btnBandVHF1.TabIndex = ((int)(resources.GetObject("btnBandVHF1.TabIndex")));
			this.btnBandVHF1.Text = resources.GetString("btnBandVHF1.Text");
			this.btnBandVHF1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF1.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandVHF1, resources.GetString("btnBandVHF1.ToolTip"));
			this.btnBandVHF1.Visible = ((bool)(resources.GetObject("btnBandVHF1.Visible")));
			this.btnBandVHF1.Click += new System.EventHandler(this.btnBandVHF0_Click);
			// 
			// btnBandVHF0
			// 
			this.btnBandVHF0.AccessibleDescription = resources.GetString("btnBandVHF0.AccessibleDescription");
			this.btnBandVHF0.AccessibleName = resources.GetString("btnBandVHF0.AccessibleName");
			this.btnBandVHF0.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandVHF0.Anchor")));
			this.btnBandVHF0.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandVHF0.BackgroundImage")));
			this.btnBandVHF0.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandVHF0.Dock")));
			this.btnBandVHF0.Enabled = ((bool)(resources.GetObject("btnBandVHF0.Enabled")));
			this.btnBandVHF0.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandVHF0.FlatStyle")));
			this.btnBandVHF0.Font = ((System.Drawing.Font)(resources.GetObject("btnBandVHF0.Font")));
			this.btnBandVHF0.Image = ((System.Drawing.Image)(resources.GetObject("btnBandVHF0.Image")));
			this.btnBandVHF0.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF0.ImageAlign")));
			this.btnBandVHF0.ImageIndex = ((int)(resources.GetObject("btnBandVHF0.ImageIndex")));
			this.btnBandVHF0.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandVHF0.ImeMode")));
			this.btnBandVHF0.Location = ((System.Drawing.Point)(resources.GetObject("btnBandVHF0.Location")));
			this.btnBandVHF0.Name = "btnBandVHF0";
			this.btnBandVHF0.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandVHF0.RightToLeft")));
			this.btnBandVHF0.Size = ((System.Drawing.Size)(resources.GetObject("btnBandVHF0.Size")));
			this.btnBandVHF0.TabIndex = ((int)(resources.GetObject("btnBandVHF0.TabIndex")));
			this.btnBandVHF0.Text = resources.GetString("btnBandVHF0.Text");
			this.btnBandVHF0.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF0.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandVHF0, resources.GetString("btnBandVHF0.ToolTip"));
			this.btnBandVHF0.Visible = ((bool)(resources.GetObject("btnBandVHF0.Visible")));
			this.btnBandVHF0.Click += new System.EventHandler(this.btnBandVHF0_Click);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuSetup,
																					  this.mnuCW,
																					  this.mnuWave,
																					  this.mnuEQ,
																					  this.mnuUCB,
																					  this.mnuXVTR,
																					  this.mnuCWX});
			this.mainMenu1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mainMenu1.RightToLeft")));
			// 
			// mnuSetup
			// 
			this.mnuSetup.Enabled = ((bool)(resources.GetObject("mnuSetup.Enabled")));
			this.mnuSetup.Index = 0;
			this.mnuSetup.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuSetup.Shortcut")));
			this.mnuSetup.ShowShortcut = ((bool)(resources.GetObject("mnuSetup.ShowShortcut")));
			this.mnuSetup.Text = resources.GetString("mnuSetup.Text");
			this.mnuSetup.Visible = ((bool)(resources.GetObject("mnuSetup.Visible")));
			this.mnuSetup.Click += new System.EventHandler(this.menu_setup_Click);
			// 
			// mnuCW
			// 
			this.mnuCW.Enabled = ((bool)(resources.GetObject("mnuCW.Enabled")));
			this.mnuCW.Index = 1;
			this.mnuCW.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuCW.Shortcut")));
			this.mnuCW.ShowShortcut = ((bool)(resources.GetObject("mnuCW.ShowShortcut")));
			this.mnuCW.Text = resources.GetString("mnuCW.Text");
			this.mnuCW.Visible = ((bool)(resources.GetObject("mnuCW.Visible")));
			this.mnuCW.Click += new System.EventHandler(this.menu_cw_Click);
			// 
			// mnuWave
			// 
			this.mnuWave.Enabled = ((bool)(resources.GetObject("mnuWave.Enabled")));
			this.mnuWave.Index = 2;
			this.mnuWave.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuWave.Shortcut")));
			this.mnuWave.ShowShortcut = ((bool)(resources.GetObject("mnuWave.ShowShortcut")));
			this.mnuWave.Text = resources.GetString("mnuWave.Text");
			this.mnuWave.Visible = ((bool)(resources.GetObject("mnuWave.Visible")));
			this.mnuWave.Click += new System.EventHandler(this.menu_wave_Click);
			// 
			// mnuEQ
			// 
			this.mnuEQ.Enabled = ((bool)(resources.GetObject("mnuEQ.Enabled")));
			this.mnuEQ.Index = 3;
			this.mnuEQ.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuEQ.Shortcut")));
			this.mnuEQ.ShowShortcut = ((bool)(resources.GetObject("mnuEQ.ShowShortcut")));
			this.mnuEQ.Text = resources.GetString("mnuEQ.Text");
			this.mnuEQ.Visible = ((bool)(resources.GetObject("mnuEQ.Visible")));
			this.mnuEQ.Click += new System.EventHandler(this.mnuEQ_Click);
			// 
			// mnuUCB
			// 
			this.mnuUCB.Enabled = ((bool)(resources.GetObject("mnuUCB.Enabled")));
			this.mnuUCB.Index = 4;
			this.mnuUCB.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuUCB.Shortcut")));
			this.mnuUCB.ShowShortcut = ((bool)(resources.GetObject("mnuUCB.ShowShortcut")));
			this.mnuUCB.Text = resources.GetString("mnuUCB.Text");
			this.mnuUCB.Visible = ((bool)(resources.GetObject("mnuUCB.Visible")));
			this.mnuUCB.Click += new System.EventHandler(this.mnuUCB_Click);
			// 
			// mnuXVTR
			// 
			this.mnuXVTR.Enabled = ((bool)(resources.GetObject("mnuXVTR.Enabled")));
			this.mnuXVTR.Index = 5;
			this.mnuXVTR.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuXVTR.Shortcut")));
			this.mnuXVTR.ShowShortcut = ((bool)(resources.GetObject("mnuXVTR.ShowShortcut")));
			this.mnuXVTR.Text = resources.GetString("mnuXVTR.Text");
			this.mnuXVTR.Visible = ((bool)(resources.GetObject("mnuXVTR.Visible")));
			this.mnuXVTR.Click += new System.EventHandler(this.mnuXVTR_Click);
			// 
			// mnuCWX
			// 
			this.mnuCWX.Enabled = ((bool)(resources.GetObject("mnuCWX.Enabled")));
			this.mnuCWX.Index = 6;
			this.mnuCWX.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuCWX.Shortcut")));
			this.mnuCWX.ShowShortcut = ((bool)(resources.GetObject("mnuCWX.ShowShortcut")));
			this.mnuCWX.Text = resources.GetString("mnuCWX.Text");
			this.mnuCWX.Visible = ((bool)(resources.GetObject("mnuCWX.Visible")));
			this.mnuCWX.Click += new System.EventHandler(this.mnuCWX_Click);
			// 
			// grpMode
			// 
			this.grpMode.AccessibleDescription = resources.GetString("grpMode.AccessibleDescription");
			this.grpMode.AccessibleName = resources.GetString("grpMode.AccessibleName");
			this.grpMode.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpMode.Anchor")));
			this.grpMode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpMode.BackgroundImage")));
			this.grpMode.Controls.Add(this.radModeAM);
			this.grpMode.Controls.Add(this.radModeSAM);
			this.grpMode.Controls.Add(this.radModeDSB);
			this.grpMode.Controls.Add(this.radModeCWU);
			this.grpMode.Controls.Add(this.radModeDIGU);
			this.grpMode.Controls.Add(this.radModeDIGL);
			this.grpMode.Controls.Add(this.radModeLSB);
			this.grpMode.Controls.Add(this.radModeSPEC);
			this.grpMode.Controls.Add(this.radModeDRM);
			this.grpMode.Controls.Add(this.radModeFMN);
			this.grpMode.Controls.Add(this.radModeUSB);
			this.grpMode.Controls.Add(this.radModeCWL);
			this.grpMode.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpMode.Dock")));
			this.grpMode.Enabled = ((bool)(resources.GetObject("grpMode.Enabled")));
			this.grpMode.Font = ((System.Drawing.Font)(resources.GetObject("grpMode.Font")));
			this.grpMode.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpMode.ImeMode")));
			this.grpMode.Location = ((System.Drawing.Point)(resources.GetObject("grpMode.Location")));
			this.grpMode.Name = "grpMode";
			this.grpMode.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpMode.RightToLeft")));
			this.grpMode.Size = ((System.Drawing.Size)(resources.GetObject("grpMode.Size")));
			this.grpMode.TabIndex = ((int)(resources.GetObject("grpMode.TabIndex")));
			this.grpMode.TabStop = false;
			this.grpMode.Text = resources.GetString("grpMode.Text");
			this.toolTip1.SetToolTip(this.grpMode, resources.GetString("grpMode.ToolTip"));
			this.grpMode.Visible = ((bool)(resources.GetObject("grpMode.Visible")));
			// 
			// radModeAM
			// 
			this.radModeAM.AccessibleDescription = resources.GetString("radModeAM.AccessibleDescription");
			this.radModeAM.AccessibleName = resources.GetString("radModeAM.AccessibleName");
			this.radModeAM.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radModeAM.Anchor")));
			this.radModeAM.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radModeAM.Appearance")));
			this.radModeAM.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radModeAM.BackgroundImage")));
			this.radModeAM.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeAM.CheckAlign")));
			this.radModeAM.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radModeAM.Dock")));
			this.radModeAM.Enabled = ((bool)(resources.GetObject("radModeAM.Enabled")));
			this.radModeAM.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radModeAM.FlatStyle")));
			this.radModeAM.Font = ((System.Drawing.Font)(resources.GetObject("radModeAM.Font")));
			this.radModeAM.Image = ((System.Drawing.Image)(resources.GetObject("radModeAM.Image")));
			this.radModeAM.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeAM.ImageAlign")));
			this.radModeAM.ImageIndex = ((int)(resources.GetObject("radModeAM.ImageIndex")));
			this.radModeAM.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radModeAM.ImeMode")));
			this.radModeAM.Location = ((System.Drawing.Point)(resources.GetObject("radModeAM.Location")));
			this.radModeAM.Name = "radModeAM";
			this.radModeAM.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radModeAM.RightToLeft")));
			this.radModeAM.Size = ((System.Drawing.Size)(resources.GetObject("radModeAM.Size")));
			this.radModeAM.TabIndex = ((int)(resources.GetObject("radModeAM.TabIndex")));
			this.radModeAM.Text = resources.GetString("radModeAM.Text");
			this.radModeAM.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeAM.TextAlign")));
			this.toolTip1.SetToolTip(this.radModeAM, resources.GetString("radModeAM.ToolTip"));
			this.radModeAM.Visible = ((bool)(resources.GetObject("radModeAM.Visible")));
			this.radModeAM.CheckedChanged += new System.EventHandler(this.radModeAM_CheckedChanged);
			// 
			// radModeSAM
			// 
			this.radModeSAM.AccessibleDescription = resources.GetString("radModeSAM.AccessibleDescription");
			this.radModeSAM.AccessibleName = resources.GetString("radModeSAM.AccessibleName");
			this.radModeSAM.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radModeSAM.Anchor")));
			this.radModeSAM.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radModeSAM.Appearance")));
			this.radModeSAM.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radModeSAM.BackgroundImage")));
			this.radModeSAM.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeSAM.CheckAlign")));
			this.radModeSAM.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radModeSAM.Dock")));
			this.radModeSAM.Enabled = ((bool)(resources.GetObject("radModeSAM.Enabled")));
			this.radModeSAM.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radModeSAM.FlatStyle")));
			this.radModeSAM.Font = ((System.Drawing.Font)(resources.GetObject("radModeSAM.Font")));
			this.radModeSAM.Image = ((System.Drawing.Image)(resources.GetObject("radModeSAM.Image")));
			this.radModeSAM.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeSAM.ImageAlign")));
			this.radModeSAM.ImageIndex = ((int)(resources.GetObject("radModeSAM.ImageIndex")));
			this.radModeSAM.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radModeSAM.ImeMode")));
			this.radModeSAM.Location = ((System.Drawing.Point)(resources.GetObject("radModeSAM.Location")));
			this.radModeSAM.Name = "radModeSAM";
			this.radModeSAM.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radModeSAM.RightToLeft")));
			this.radModeSAM.Size = ((System.Drawing.Size)(resources.GetObject("radModeSAM.Size")));
			this.radModeSAM.TabIndex = ((int)(resources.GetObject("radModeSAM.TabIndex")));
			this.radModeSAM.Text = resources.GetString("radModeSAM.Text");
			this.radModeSAM.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeSAM.TextAlign")));
			this.toolTip1.SetToolTip(this.radModeSAM, resources.GetString("radModeSAM.ToolTip"));
			this.radModeSAM.Visible = ((bool)(resources.GetObject("radModeSAM.Visible")));
			this.radModeSAM.CheckedChanged += new System.EventHandler(this.radModeSAM_CheckedChanged);
			// 
			// radModeDSB
			// 
			this.radModeDSB.AccessibleDescription = resources.GetString("radModeDSB.AccessibleDescription");
			this.radModeDSB.AccessibleName = resources.GetString("radModeDSB.AccessibleName");
			this.radModeDSB.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radModeDSB.Anchor")));
			this.radModeDSB.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radModeDSB.Appearance")));
			this.radModeDSB.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radModeDSB.BackgroundImage")));
			this.radModeDSB.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeDSB.CheckAlign")));
			this.radModeDSB.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radModeDSB.Dock")));
			this.radModeDSB.Enabled = ((bool)(resources.GetObject("radModeDSB.Enabled")));
			this.radModeDSB.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radModeDSB.FlatStyle")));
			this.radModeDSB.Font = ((System.Drawing.Font)(resources.GetObject("radModeDSB.Font")));
			this.radModeDSB.Image = ((System.Drawing.Image)(resources.GetObject("radModeDSB.Image")));
			this.radModeDSB.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeDSB.ImageAlign")));
			this.radModeDSB.ImageIndex = ((int)(resources.GetObject("radModeDSB.ImageIndex")));
			this.radModeDSB.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radModeDSB.ImeMode")));
			this.radModeDSB.Location = ((System.Drawing.Point)(resources.GetObject("radModeDSB.Location")));
			this.radModeDSB.Name = "radModeDSB";
			this.radModeDSB.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radModeDSB.RightToLeft")));
			this.radModeDSB.Size = ((System.Drawing.Size)(resources.GetObject("radModeDSB.Size")));
			this.radModeDSB.TabIndex = ((int)(resources.GetObject("radModeDSB.TabIndex")));
			this.radModeDSB.Text = resources.GetString("radModeDSB.Text");
			this.radModeDSB.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeDSB.TextAlign")));
			this.toolTip1.SetToolTip(this.radModeDSB, resources.GetString("radModeDSB.ToolTip"));
			this.radModeDSB.Visible = ((bool)(resources.GetObject("radModeDSB.Visible")));
			this.radModeDSB.CheckedChanged += new System.EventHandler(this.radModeDSB_CheckedChanged);
			// 
			// radModeCWU
			// 
			this.radModeCWU.AccessibleDescription = resources.GetString("radModeCWU.AccessibleDescription");
			this.radModeCWU.AccessibleName = resources.GetString("radModeCWU.AccessibleName");
			this.radModeCWU.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radModeCWU.Anchor")));
			this.radModeCWU.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radModeCWU.Appearance")));
			this.radModeCWU.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radModeCWU.BackgroundImage")));
			this.radModeCWU.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeCWU.CheckAlign")));
			this.radModeCWU.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radModeCWU.Dock")));
			this.radModeCWU.Enabled = ((bool)(resources.GetObject("radModeCWU.Enabled")));
			this.radModeCWU.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radModeCWU.FlatStyle")));
			this.radModeCWU.Font = ((System.Drawing.Font)(resources.GetObject("radModeCWU.Font")));
			this.radModeCWU.Image = ((System.Drawing.Image)(resources.GetObject("radModeCWU.Image")));
			this.radModeCWU.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeCWU.ImageAlign")));
			this.radModeCWU.ImageIndex = ((int)(resources.GetObject("radModeCWU.ImageIndex")));
			this.radModeCWU.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radModeCWU.ImeMode")));
			this.radModeCWU.Location = ((System.Drawing.Point)(resources.GetObject("radModeCWU.Location")));
			this.radModeCWU.Name = "radModeCWU";
			this.radModeCWU.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radModeCWU.RightToLeft")));
			this.radModeCWU.Size = ((System.Drawing.Size)(resources.GetObject("radModeCWU.Size")));
			this.radModeCWU.TabIndex = ((int)(resources.GetObject("radModeCWU.TabIndex")));
			this.radModeCWU.Text = resources.GetString("radModeCWU.Text");
			this.radModeCWU.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeCWU.TextAlign")));
			this.toolTip1.SetToolTip(this.radModeCWU, resources.GetString("radModeCWU.ToolTip"));
			this.radModeCWU.Visible = ((bool)(resources.GetObject("radModeCWU.Visible")));
			this.radModeCWU.CheckedChanged += new System.EventHandler(this.radModeCWU_CheckedChanged);
			// 
			// radModeDIGU
			// 
			this.radModeDIGU.AccessibleDescription = resources.GetString("radModeDIGU.AccessibleDescription");
			this.radModeDIGU.AccessibleName = resources.GetString("radModeDIGU.AccessibleName");
			this.radModeDIGU.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radModeDIGU.Anchor")));
			this.radModeDIGU.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radModeDIGU.Appearance")));
			this.radModeDIGU.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radModeDIGU.BackgroundImage")));
			this.radModeDIGU.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeDIGU.CheckAlign")));
			this.radModeDIGU.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radModeDIGU.Dock")));
			this.radModeDIGU.Enabled = ((bool)(resources.GetObject("radModeDIGU.Enabled")));
			this.radModeDIGU.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radModeDIGU.FlatStyle")));
			this.radModeDIGU.Font = ((System.Drawing.Font)(resources.GetObject("radModeDIGU.Font")));
			this.radModeDIGU.Image = ((System.Drawing.Image)(resources.GetObject("radModeDIGU.Image")));
			this.radModeDIGU.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeDIGU.ImageAlign")));
			this.radModeDIGU.ImageIndex = ((int)(resources.GetObject("radModeDIGU.ImageIndex")));
			this.radModeDIGU.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radModeDIGU.ImeMode")));
			this.radModeDIGU.Location = ((System.Drawing.Point)(resources.GetObject("radModeDIGU.Location")));
			this.radModeDIGU.Name = "radModeDIGU";
			this.radModeDIGU.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radModeDIGU.RightToLeft")));
			this.radModeDIGU.Size = ((System.Drawing.Size)(resources.GetObject("radModeDIGU.Size")));
			this.radModeDIGU.TabIndex = ((int)(resources.GetObject("radModeDIGU.TabIndex")));
			this.radModeDIGU.Text = resources.GetString("radModeDIGU.Text");
			this.radModeDIGU.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeDIGU.TextAlign")));
			this.toolTip1.SetToolTip(this.radModeDIGU, resources.GetString("radModeDIGU.ToolTip"));
			this.radModeDIGU.Visible = ((bool)(resources.GetObject("radModeDIGU.Visible")));
			this.radModeDIGU.CheckedChanged += new System.EventHandler(this.radModeDIGU_CheckedChanged);
			// 
			// radModeDIGL
			// 
			this.radModeDIGL.AccessibleDescription = resources.GetString("radModeDIGL.AccessibleDescription");
			this.radModeDIGL.AccessibleName = resources.GetString("radModeDIGL.AccessibleName");
			this.radModeDIGL.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radModeDIGL.Anchor")));
			this.radModeDIGL.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radModeDIGL.Appearance")));
			this.radModeDIGL.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radModeDIGL.BackgroundImage")));
			this.radModeDIGL.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeDIGL.CheckAlign")));
			this.radModeDIGL.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radModeDIGL.Dock")));
			this.radModeDIGL.Enabled = ((bool)(resources.GetObject("radModeDIGL.Enabled")));
			this.radModeDIGL.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radModeDIGL.FlatStyle")));
			this.radModeDIGL.Font = ((System.Drawing.Font)(resources.GetObject("radModeDIGL.Font")));
			this.radModeDIGL.Image = ((System.Drawing.Image)(resources.GetObject("radModeDIGL.Image")));
			this.radModeDIGL.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeDIGL.ImageAlign")));
			this.radModeDIGL.ImageIndex = ((int)(resources.GetObject("radModeDIGL.ImageIndex")));
			this.radModeDIGL.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radModeDIGL.ImeMode")));
			this.radModeDIGL.Location = ((System.Drawing.Point)(resources.GetObject("radModeDIGL.Location")));
			this.radModeDIGL.Name = "radModeDIGL";
			this.radModeDIGL.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radModeDIGL.RightToLeft")));
			this.radModeDIGL.Size = ((System.Drawing.Size)(resources.GetObject("radModeDIGL.Size")));
			this.radModeDIGL.TabIndex = ((int)(resources.GetObject("radModeDIGL.TabIndex")));
			this.radModeDIGL.Text = resources.GetString("radModeDIGL.Text");
			this.radModeDIGL.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeDIGL.TextAlign")));
			this.toolTip1.SetToolTip(this.radModeDIGL, resources.GetString("radModeDIGL.ToolTip"));
			this.radModeDIGL.Visible = ((bool)(resources.GetObject("radModeDIGL.Visible")));
			this.radModeDIGL.CheckedChanged += new System.EventHandler(this.radModeDIGL_CheckedChanged);
			// 
			// radModeLSB
			// 
			this.radModeLSB.AccessibleDescription = resources.GetString("radModeLSB.AccessibleDescription");
			this.radModeLSB.AccessibleName = resources.GetString("radModeLSB.AccessibleName");
			this.radModeLSB.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radModeLSB.Anchor")));
			this.radModeLSB.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radModeLSB.Appearance")));
			this.radModeLSB.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radModeLSB.BackgroundImage")));
			this.radModeLSB.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeLSB.CheckAlign")));
			this.radModeLSB.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radModeLSB.Dock")));
			this.radModeLSB.Enabled = ((bool)(resources.GetObject("radModeLSB.Enabled")));
			this.radModeLSB.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radModeLSB.FlatStyle")));
			this.radModeLSB.Font = ((System.Drawing.Font)(resources.GetObject("radModeLSB.Font")));
			this.radModeLSB.Image = ((System.Drawing.Image)(resources.GetObject("radModeLSB.Image")));
			this.radModeLSB.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeLSB.ImageAlign")));
			this.radModeLSB.ImageIndex = ((int)(resources.GetObject("radModeLSB.ImageIndex")));
			this.radModeLSB.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radModeLSB.ImeMode")));
			this.radModeLSB.Location = ((System.Drawing.Point)(resources.GetObject("radModeLSB.Location")));
			this.radModeLSB.Name = "radModeLSB";
			this.radModeLSB.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radModeLSB.RightToLeft")));
			this.radModeLSB.Size = ((System.Drawing.Size)(resources.GetObject("radModeLSB.Size")));
			this.radModeLSB.TabIndex = ((int)(resources.GetObject("radModeLSB.TabIndex")));
			this.radModeLSB.Text = resources.GetString("radModeLSB.Text");
			this.radModeLSB.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeLSB.TextAlign")));
			this.toolTip1.SetToolTip(this.radModeLSB, resources.GetString("radModeLSB.ToolTip"));
			this.radModeLSB.Visible = ((bool)(resources.GetObject("radModeLSB.Visible")));
			this.radModeLSB.CheckedChanged += new System.EventHandler(this.radModeLSB_CheckedChanged);
			// 
			// radModeSPEC
			// 
			this.radModeSPEC.AccessibleDescription = resources.GetString("radModeSPEC.AccessibleDescription");
			this.radModeSPEC.AccessibleName = resources.GetString("radModeSPEC.AccessibleName");
			this.radModeSPEC.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radModeSPEC.Anchor")));
			this.radModeSPEC.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radModeSPEC.Appearance")));
			this.radModeSPEC.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radModeSPEC.BackgroundImage")));
			this.radModeSPEC.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeSPEC.CheckAlign")));
			this.radModeSPEC.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radModeSPEC.Dock")));
			this.radModeSPEC.Enabled = ((bool)(resources.GetObject("radModeSPEC.Enabled")));
			this.radModeSPEC.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radModeSPEC.FlatStyle")));
			this.radModeSPEC.Font = ((System.Drawing.Font)(resources.GetObject("radModeSPEC.Font")));
			this.radModeSPEC.Image = ((System.Drawing.Image)(resources.GetObject("radModeSPEC.Image")));
			this.radModeSPEC.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeSPEC.ImageAlign")));
			this.radModeSPEC.ImageIndex = ((int)(resources.GetObject("radModeSPEC.ImageIndex")));
			this.radModeSPEC.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radModeSPEC.ImeMode")));
			this.radModeSPEC.Location = ((System.Drawing.Point)(resources.GetObject("radModeSPEC.Location")));
			this.radModeSPEC.Name = "radModeSPEC";
			this.radModeSPEC.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radModeSPEC.RightToLeft")));
			this.radModeSPEC.Size = ((System.Drawing.Size)(resources.GetObject("radModeSPEC.Size")));
			this.radModeSPEC.TabIndex = ((int)(resources.GetObject("radModeSPEC.TabIndex")));
			this.radModeSPEC.Text = resources.GetString("radModeSPEC.Text");
			this.radModeSPEC.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeSPEC.TextAlign")));
			this.toolTip1.SetToolTip(this.radModeSPEC, resources.GetString("radModeSPEC.ToolTip"));
			this.radModeSPEC.Visible = ((bool)(resources.GetObject("radModeSPEC.Visible")));
			this.radModeSPEC.CheckedChanged += new System.EventHandler(this.radModeSPEC_CheckedChanged);
			// 
			// radModeDRM
			// 
			this.radModeDRM.AccessibleDescription = resources.GetString("radModeDRM.AccessibleDescription");
			this.radModeDRM.AccessibleName = resources.GetString("radModeDRM.AccessibleName");
			this.radModeDRM.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radModeDRM.Anchor")));
			this.radModeDRM.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radModeDRM.Appearance")));
			this.radModeDRM.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radModeDRM.BackgroundImage")));
			this.radModeDRM.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeDRM.CheckAlign")));
			this.radModeDRM.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radModeDRM.Dock")));
			this.radModeDRM.Enabled = ((bool)(resources.GetObject("radModeDRM.Enabled")));
			this.radModeDRM.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radModeDRM.FlatStyle")));
			this.radModeDRM.Font = ((System.Drawing.Font)(resources.GetObject("radModeDRM.Font")));
			this.radModeDRM.Image = ((System.Drawing.Image)(resources.GetObject("radModeDRM.Image")));
			this.radModeDRM.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeDRM.ImageAlign")));
			this.radModeDRM.ImageIndex = ((int)(resources.GetObject("radModeDRM.ImageIndex")));
			this.radModeDRM.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radModeDRM.ImeMode")));
			this.radModeDRM.Location = ((System.Drawing.Point)(resources.GetObject("radModeDRM.Location")));
			this.radModeDRM.Name = "radModeDRM";
			this.radModeDRM.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radModeDRM.RightToLeft")));
			this.radModeDRM.Size = ((System.Drawing.Size)(resources.GetObject("radModeDRM.Size")));
			this.radModeDRM.TabIndex = ((int)(resources.GetObject("radModeDRM.TabIndex")));
			this.radModeDRM.Text = resources.GetString("radModeDRM.Text");
			this.radModeDRM.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeDRM.TextAlign")));
			this.toolTip1.SetToolTip(this.radModeDRM, resources.GetString("radModeDRM.ToolTip"));
			this.radModeDRM.Visible = ((bool)(resources.GetObject("radModeDRM.Visible")));
			this.radModeDRM.CheckedChanged += new System.EventHandler(this.radModeDRM_CheckedChanged);
			// 
			// radModeFMN
			// 
			this.radModeFMN.AccessibleDescription = resources.GetString("radModeFMN.AccessibleDescription");
			this.radModeFMN.AccessibleName = resources.GetString("radModeFMN.AccessibleName");
			this.radModeFMN.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radModeFMN.Anchor")));
			this.radModeFMN.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radModeFMN.Appearance")));
			this.radModeFMN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radModeFMN.BackgroundImage")));
			this.radModeFMN.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeFMN.CheckAlign")));
			this.radModeFMN.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radModeFMN.Dock")));
			this.radModeFMN.Enabled = ((bool)(resources.GetObject("radModeFMN.Enabled")));
			this.radModeFMN.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radModeFMN.FlatStyle")));
			this.radModeFMN.Font = ((System.Drawing.Font)(resources.GetObject("radModeFMN.Font")));
			this.radModeFMN.Image = ((System.Drawing.Image)(resources.GetObject("radModeFMN.Image")));
			this.radModeFMN.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeFMN.ImageAlign")));
			this.radModeFMN.ImageIndex = ((int)(resources.GetObject("radModeFMN.ImageIndex")));
			this.radModeFMN.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radModeFMN.ImeMode")));
			this.radModeFMN.Location = ((System.Drawing.Point)(resources.GetObject("radModeFMN.Location")));
			this.radModeFMN.Name = "radModeFMN";
			this.radModeFMN.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radModeFMN.RightToLeft")));
			this.radModeFMN.Size = ((System.Drawing.Size)(resources.GetObject("radModeFMN.Size")));
			this.radModeFMN.TabIndex = ((int)(resources.GetObject("radModeFMN.TabIndex")));
			this.radModeFMN.Text = resources.GetString("radModeFMN.Text");
			this.radModeFMN.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeFMN.TextAlign")));
			this.toolTip1.SetToolTip(this.radModeFMN, resources.GetString("radModeFMN.ToolTip"));
			this.radModeFMN.Visible = ((bool)(resources.GetObject("radModeFMN.Visible")));
			this.radModeFMN.CheckedChanged += new System.EventHandler(this.radModeFMN_CheckedChanged);
			// 
			// radModeUSB
			// 
			this.radModeUSB.AccessibleDescription = resources.GetString("radModeUSB.AccessibleDescription");
			this.radModeUSB.AccessibleName = resources.GetString("radModeUSB.AccessibleName");
			this.radModeUSB.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radModeUSB.Anchor")));
			this.radModeUSB.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radModeUSB.Appearance")));
			this.radModeUSB.BackColor = System.Drawing.SystemColors.Control;
			this.radModeUSB.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radModeUSB.BackgroundImage")));
			this.radModeUSB.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeUSB.CheckAlign")));
			this.radModeUSB.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radModeUSB.Dock")));
			this.radModeUSB.Enabled = ((bool)(resources.GetObject("radModeUSB.Enabled")));
			this.radModeUSB.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radModeUSB.FlatStyle")));
			this.radModeUSB.Font = ((System.Drawing.Font)(resources.GetObject("radModeUSB.Font")));
			this.radModeUSB.Image = ((System.Drawing.Image)(resources.GetObject("radModeUSB.Image")));
			this.radModeUSB.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeUSB.ImageAlign")));
			this.radModeUSB.ImageIndex = ((int)(resources.GetObject("radModeUSB.ImageIndex")));
			this.radModeUSB.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radModeUSB.ImeMode")));
			this.radModeUSB.Location = ((System.Drawing.Point)(resources.GetObject("radModeUSB.Location")));
			this.radModeUSB.Name = "radModeUSB";
			this.radModeUSB.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radModeUSB.RightToLeft")));
			this.radModeUSB.Size = ((System.Drawing.Size)(resources.GetObject("radModeUSB.Size")));
			this.radModeUSB.TabIndex = ((int)(resources.GetObject("radModeUSB.TabIndex")));
			this.radModeUSB.Text = resources.GetString("radModeUSB.Text");
			this.radModeUSB.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeUSB.TextAlign")));
			this.toolTip1.SetToolTip(this.radModeUSB, resources.GetString("radModeUSB.ToolTip"));
			this.radModeUSB.Visible = ((bool)(resources.GetObject("radModeUSB.Visible")));
			this.radModeUSB.CheckedChanged += new System.EventHandler(this.radModeUSB_CheckedChanged);
			// 
			// radModeCWL
			// 
			this.radModeCWL.AccessibleDescription = resources.GetString("radModeCWL.AccessibleDescription");
			this.radModeCWL.AccessibleName = resources.GetString("radModeCWL.AccessibleName");
			this.radModeCWL.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radModeCWL.Anchor")));
			this.radModeCWL.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radModeCWL.Appearance")));
			this.radModeCWL.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radModeCWL.BackgroundImage")));
			this.radModeCWL.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeCWL.CheckAlign")));
			this.radModeCWL.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radModeCWL.Dock")));
			this.radModeCWL.Enabled = ((bool)(resources.GetObject("radModeCWL.Enabled")));
			this.radModeCWL.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radModeCWL.FlatStyle")));
			this.radModeCWL.Font = ((System.Drawing.Font)(resources.GetObject("radModeCWL.Font")));
			this.radModeCWL.Image = ((System.Drawing.Image)(resources.GetObject("radModeCWL.Image")));
			this.radModeCWL.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeCWL.ImageAlign")));
			this.radModeCWL.ImageIndex = ((int)(resources.GetObject("radModeCWL.ImageIndex")));
			this.radModeCWL.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radModeCWL.ImeMode")));
			this.radModeCWL.Location = ((System.Drawing.Point)(resources.GetObject("radModeCWL.Location")));
			this.radModeCWL.Name = "radModeCWL";
			this.radModeCWL.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radModeCWL.RightToLeft")));
			this.radModeCWL.Size = ((System.Drawing.Size)(resources.GetObject("radModeCWL.Size")));
			this.radModeCWL.TabIndex = ((int)(resources.GetObject("radModeCWL.TabIndex")));
			this.radModeCWL.Text = resources.GetString("radModeCWL.Text");
			this.radModeCWL.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radModeCWL.TextAlign")));
			this.toolTip1.SetToolTip(this.radModeCWL, resources.GetString("radModeCWL.ToolTip"));
			this.radModeCWL.Visible = ((bool)(resources.GetObject("radModeCWL.Visible")));
			this.radModeCWL.CheckedChanged += new System.EventHandler(this.radModeCWL_CheckedChanged);
			// 
			// grpBandHF
			// 
			this.grpBandHF.AccessibleDescription = resources.GetString("grpBandHF.AccessibleDescription");
			this.grpBandHF.AccessibleName = resources.GetString("grpBandHF.AccessibleName");
			this.grpBandHF.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpBandHF.Anchor")));
			this.grpBandHF.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpBandHF.BackgroundImage")));
			this.grpBandHF.Controls.Add(this.btnBandGEN);
			this.grpBandHF.Controls.Add(this.btnBandWWV);
			this.grpBandHF.Controls.Add(this.btnBandVHF);
			this.grpBandHF.Controls.Add(this.btnBand2);
			this.grpBandHF.Controls.Add(this.btnBand6);
			this.grpBandHF.Controls.Add(this.btnBand10);
			this.grpBandHF.Controls.Add(this.btnBand12);
			this.grpBandHF.Controls.Add(this.btnBand15);
			this.grpBandHF.Controls.Add(this.btnBand17);
			this.grpBandHF.Controls.Add(this.btnBand20);
			this.grpBandHF.Controls.Add(this.btnBand30);
			this.grpBandHF.Controls.Add(this.btnBand40);
			this.grpBandHF.Controls.Add(this.btnBand60);
			this.grpBandHF.Controls.Add(this.btnBand80);
			this.grpBandHF.Controls.Add(this.btnBand160);
			this.grpBandHF.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpBandHF.Dock")));
			this.grpBandHF.Enabled = ((bool)(resources.GetObject("grpBandHF.Enabled")));
			this.grpBandHF.Font = ((System.Drawing.Font)(resources.GetObject("grpBandHF.Font")));
			this.grpBandHF.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpBandHF.ImeMode")));
			this.grpBandHF.Location = ((System.Drawing.Point)(resources.GetObject("grpBandHF.Location")));
			this.grpBandHF.Name = "grpBandHF";
			this.grpBandHF.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpBandHF.RightToLeft")));
			this.grpBandHF.Size = ((System.Drawing.Size)(resources.GetObject("grpBandHF.Size")));
			this.grpBandHF.TabIndex = ((int)(resources.GetObject("grpBandHF.TabIndex")));
			this.grpBandHF.TabStop = false;
			this.grpBandHF.Text = resources.GetString("grpBandHF.Text");
			this.toolTip1.SetToolTip(this.grpBandHF, resources.GetString("grpBandHF.ToolTip"));
			this.grpBandHF.Visible = ((bool)(resources.GetObject("grpBandHF.Visible")));
			// 
			// btnBandGEN
			// 
			this.btnBandGEN.AccessibleDescription = resources.GetString("btnBandGEN.AccessibleDescription");
			this.btnBandGEN.AccessibleName = resources.GetString("btnBandGEN.AccessibleName");
			this.btnBandGEN.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandGEN.Anchor")));
			this.btnBandGEN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandGEN.BackgroundImage")));
			this.btnBandGEN.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandGEN.Dock")));
			this.btnBandGEN.Enabled = ((bool)(resources.GetObject("btnBandGEN.Enabled")));
			this.btnBandGEN.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandGEN.FlatStyle")));
			this.btnBandGEN.Font = ((System.Drawing.Font)(resources.GetObject("btnBandGEN.Font")));
			this.btnBandGEN.Image = ((System.Drawing.Image)(resources.GetObject("btnBandGEN.Image")));
			this.btnBandGEN.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandGEN.ImageAlign")));
			this.btnBandGEN.ImageIndex = ((int)(resources.GetObject("btnBandGEN.ImageIndex")));
			this.btnBandGEN.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandGEN.ImeMode")));
			this.btnBandGEN.Location = ((System.Drawing.Point)(resources.GetObject("btnBandGEN.Location")));
			this.btnBandGEN.Name = "btnBandGEN";
			this.btnBandGEN.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandGEN.RightToLeft")));
			this.btnBandGEN.Size = ((System.Drawing.Size)(resources.GetObject("btnBandGEN.Size")));
			this.btnBandGEN.TabIndex = ((int)(resources.GetObject("btnBandGEN.TabIndex")));
			this.btnBandGEN.Text = resources.GetString("btnBandGEN.Text");
			this.btnBandGEN.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandGEN.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandGEN, resources.GetString("btnBandGEN.ToolTip"));
			this.btnBandGEN.Visible = ((bool)(resources.GetObject("btnBandGEN.Visible")));
			this.btnBandGEN.Click += new System.EventHandler(this.btnBandGEN_Click);
			// 
			// btnBandWWV
			// 
			this.btnBandWWV.AccessibleDescription = resources.GetString("btnBandWWV.AccessibleDescription");
			this.btnBandWWV.AccessibleName = resources.GetString("btnBandWWV.AccessibleName");
			this.btnBandWWV.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandWWV.Anchor")));
			this.btnBandWWV.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandWWV.BackgroundImage")));
			this.btnBandWWV.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandWWV.Dock")));
			this.btnBandWWV.Enabled = ((bool)(resources.GetObject("btnBandWWV.Enabled")));
			this.btnBandWWV.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandWWV.FlatStyle")));
			this.btnBandWWV.Font = ((System.Drawing.Font)(resources.GetObject("btnBandWWV.Font")));
			this.btnBandWWV.Image = ((System.Drawing.Image)(resources.GetObject("btnBandWWV.Image")));
			this.btnBandWWV.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandWWV.ImageAlign")));
			this.btnBandWWV.ImageIndex = ((int)(resources.GetObject("btnBandWWV.ImageIndex")));
			this.btnBandWWV.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandWWV.ImeMode")));
			this.btnBandWWV.Location = ((System.Drawing.Point)(resources.GetObject("btnBandWWV.Location")));
			this.btnBandWWV.Name = "btnBandWWV";
			this.btnBandWWV.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandWWV.RightToLeft")));
			this.btnBandWWV.Size = ((System.Drawing.Size)(resources.GetObject("btnBandWWV.Size")));
			this.btnBandWWV.TabIndex = ((int)(resources.GetObject("btnBandWWV.TabIndex")));
			this.btnBandWWV.Text = resources.GetString("btnBandWWV.Text");
			this.btnBandWWV.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandWWV.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandWWV, resources.GetString("btnBandWWV.ToolTip"));
			this.btnBandWWV.Visible = ((bool)(resources.GetObject("btnBandWWV.Visible")));
			this.btnBandWWV.Click += new System.EventHandler(this.btnBandWWV_Click);
			// 
			// btnBandVHF
			// 
			this.btnBandVHF.AccessibleDescription = resources.GetString("btnBandVHF.AccessibleDescription");
			this.btnBandVHF.AccessibleName = resources.GetString("btnBandVHF.AccessibleName");
			this.btnBandVHF.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBandVHF.Anchor")));
			this.btnBandVHF.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBandVHF.BackgroundImage")));
			this.btnBandVHF.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBandVHF.Dock")));
			this.btnBandVHF.Enabled = ((bool)(resources.GetObject("btnBandVHF.Enabled")));
			this.btnBandVHF.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBandVHF.FlatStyle")));
			this.btnBandVHF.Font = ((System.Drawing.Font)(resources.GetObject("btnBandVHF.Font")));
			this.btnBandVHF.Image = ((System.Drawing.Image)(resources.GetObject("btnBandVHF.Image")));
			this.btnBandVHF.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF.ImageAlign")));
			this.btnBandVHF.ImageIndex = ((int)(resources.GetObject("btnBandVHF.ImageIndex")));
			this.btnBandVHF.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBandVHF.ImeMode")));
			this.btnBandVHF.Location = ((System.Drawing.Point)(resources.GetObject("btnBandVHF.Location")));
			this.btnBandVHF.Name = "btnBandVHF";
			this.btnBandVHF.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBandVHF.RightToLeft")));
			this.btnBandVHF.Size = ((System.Drawing.Size)(resources.GetObject("btnBandVHF.Size")));
			this.btnBandVHF.TabIndex = ((int)(resources.GetObject("btnBandVHF.TabIndex")));
			this.btnBandVHF.Text = resources.GetString("btnBandVHF.Text");
			this.btnBandVHF.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBandVHF.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBandVHF, resources.GetString("btnBandVHF.ToolTip"));
			this.btnBandVHF.Visible = ((bool)(resources.GetObject("btnBandVHF.Visible")));
			this.btnBandVHF.Click += new System.EventHandler(this.btnBandVHF_Click);
			// 
			// btnBand2
			// 
			this.btnBand2.AccessibleDescription = resources.GetString("btnBand2.AccessibleDescription");
			this.btnBand2.AccessibleName = resources.GetString("btnBand2.AccessibleName");
			this.btnBand2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBand2.Anchor")));
			this.btnBand2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBand2.BackgroundImage")));
			this.btnBand2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBand2.Dock")));
			this.btnBand2.Enabled = ((bool)(resources.GetObject("btnBand2.Enabled")));
			this.btnBand2.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBand2.FlatStyle")));
			this.btnBand2.Font = ((System.Drawing.Font)(resources.GetObject("btnBand2.Font")));
			this.btnBand2.Image = ((System.Drawing.Image)(resources.GetObject("btnBand2.Image")));
			this.btnBand2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand2.ImageAlign")));
			this.btnBand2.ImageIndex = ((int)(resources.GetObject("btnBand2.ImageIndex")));
			this.btnBand2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBand2.ImeMode")));
			this.btnBand2.Location = ((System.Drawing.Point)(resources.GetObject("btnBand2.Location")));
			this.btnBand2.Name = "btnBand2";
			this.btnBand2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBand2.RightToLeft")));
			this.btnBand2.Size = ((System.Drawing.Size)(resources.GetObject("btnBand2.Size")));
			this.btnBand2.TabIndex = ((int)(resources.GetObject("btnBand2.TabIndex")));
			this.btnBand2.Text = resources.GetString("btnBand2.Text");
			this.btnBand2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand2.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBand2, resources.GetString("btnBand2.ToolTip"));
			this.btnBand2.Visible = ((bool)(resources.GetObject("btnBand2.Visible")));
			this.btnBand2.Click += new System.EventHandler(this.btnBand2_Click);
			// 
			// btnBand6
			// 
			this.btnBand6.AccessibleDescription = resources.GetString("btnBand6.AccessibleDescription");
			this.btnBand6.AccessibleName = resources.GetString("btnBand6.AccessibleName");
			this.btnBand6.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBand6.Anchor")));
			this.btnBand6.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBand6.BackgroundImage")));
			this.btnBand6.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBand6.Dock")));
			this.btnBand6.Enabled = ((bool)(resources.GetObject("btnBand6.Enabled")));
			this.btnBand6.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBand6.FlatStyle")));
			this.btnBand6.Font = ((System.Drawing.Font)(resources.GetObject("btnBand6.Font")));
			this.btnBand6.Image = ((System.Drawing.Image)(resources.GetObject("btnBand6.Image")));
			this.btnBand6.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand6.ImageAlign")));
			this.btnBand6.ImageIndex = ((int)(resources.GetObject("btnBand6.ImageIndex")));
			this.btnBand6.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBand6.ImeMode")));
			this.btnBand6.Location = ((System.Drawing.Point)(resources.GetObject("btnBand6.Location")));
			this.btnBand6.Name = "btnBand6";
			this.btnBand6.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBand6.RightToLeft")));
			this.btnBand6.Size = ((System.Drawing.Size)(resources.GetObject("btnBand6.Size")));
			this.btnBand6.TabIndex = ((int)(resources.GetObject("btnBand6.TabIndex")));
			this.btnBand6.Text = resources.GetString("btnBand6.Text");
			this.btnBand6.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand6.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBand6, resources.GetString("btnBand6.ToolTip"));
			this.btnBand6.Visible = ((bool)(resources.GetObject("btnBand6.Visible")));
			this.btnBand6.Click += new System.EventHandler(this.btnBand6_Click);
			// 
			// btnBand10
			// 
			this.btnBand10.AccessibleDescription = resources.GetString("btnBand10.AccessibleDescription");
			this.btnBand10.AccessibleName = resources.GetString("btnBand10.AccessibleName");
			this.btnBand10.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBand10.Anchor")));
			this.btnBand10.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBand10.BackgroundImage")));
			this.btnBand10.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBand10.Dock")));
			this.btnBand10.Enabled = ((bool)(resources.GetObject("btnBand10.Enabled")));
			this.btnBand10.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBand10.FlatStyle")));
			this.btnBand10.Font = ((System.Drawing.Font)(resources.GetObject("btnBand10.Font")));
			this.btnBand10.Image = ((System.Drawing.Image)(resources.GetObject("btnBand10.Image")));
			this.btnBand10.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand10.ImageAlign")));
			this.btnBand10.ImageIndex = ((int)(resources.GetObject("btnBand10.ImageIndex")));
			this.btnBand10.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBand10.ImeMode")));
			this.btnBand10.Location = ((System.Drawing.Point)(resources.GetObject("btnBand10.Location")));
			this.btnBand10.Name = "btnBand10";
			this.btnBand10.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBand10.RightToLeft")));
			this.btnBand10.Size = ((System.Drawing.Size)(resources.GetObject("btnBand10.Size")));
			this.btnBand10.TabIndex = ((int)(resources.GetObject("btnBand10.TabIndex")));
			this.btnBand10.Text = resources.GetString("btnBand10.Text");
			this.btnBand10.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand10.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBand10, resources.GetString("btnBand10.ToolTip"));
			this.btnBand10.Visible = ((bool)(resources.GetObject("btnBand10.Visible")));
			this.btnBand10.Click += new System.EventHandler(this.btnBand10_Click);
			// 
			// btnBand12
			// 
			this.btnBand12.AccessibleDescription = resources.GetString("btnBand12.AccessibleDescription");
			this.btnBand12.AccessibleName = resources.GetString("btnBand12.AccessibleName");
			this.btnBand12.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBand12.Anchor")));
			this.btnBand12.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBand12.BackgroundImage")));
			this.btnBand12.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBand12.Dock")));
			this.btnBand12.Enabled = ((bool)(resources.GetObject("btnBand12.Enabled")));
			this.btnBand12.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBand12.FlatStyle")));
			this.btnBand12.Font = ((System.Drawing.Font)(resources.GetObject("btnBand12.Font")));
			this.btnBand12.Image = ((System.Drawing.Image)(resources.GetObject("btnBand12.Image")));
			this.btnBand12.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand12.ImageAlign")));
			this.btnBand12.ImageIndex = ((int)(resources.GetObject("btnBand12.ImageIndex")));
			this.btnBand12.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBand12.ImeMode")));
			this.btnBand12.Location = ((System.Drawing.Point)(resources.GetObject("btnBand12.Location")));
			this.btnBand12.Name = "btnBand12";
			this.btnBand12.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBand12.RightToLeft")));
			this.btnBand12.Size = ((System.Drawing.Size)(resources.GetObject("btnBand12.Size")));
			this.btnBand12.TabIndex = ((int)(resources.GetObject("btnBand12.TabIndex")));
			this.btnBand12.Text = resources.GetString("btnBand12.Text");
			this.btnBand12.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand12.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBand12, resources.GetString("btnBand12.ToolTip"));
			this.btnBand12.Visible = ((bool)(resources.GetObject("btnBand12.Visible")));
			this.btnBand12.Click += new System.EventHandler(this.btnBand12_Click);
			// 
			// btnBand15
			// 
			this.btnBand15.AccessibleDescription = resources.GetString("btnBand15.AccessibleDescription");
			this.btnBand15.AccessibleName = resources.GetString("btnBand15.AccessibleName");
			this.btnBand15.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBand15.Anchor")));
			this.btnBand15.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBand15.BackgroundImage")));
			this.btnBand15.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBand15.Dock")));
			this.btnBand15.Enabled = ((bool)(resources.GetObject("btnBand15.Enabled")));
			this.btnBand15.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBand15.FlatStyle")));
			this.btnBand15.Font = ((System.Drawing.Font)(resources.GetObject("btnBand15.Font")));
			this.btnBand15.Image = ((System.Drawing.Image)(resources.GetObject("btnBand15.Image")));
			this.btnBand15.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand15.ImageAlign")));
			this.btnBand15.ImageIndex = ((int)(resources.GetObject("btnBand15.ImageIndex")));
			this.btnBand15.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBand15.ImeMode")));
			this.btnBand15.Location = ((System.Drawing.Point)(resources.GetObject("btnBand15.Location")));
			this.btnBand15.Name = "btnBand15";
			this.btnBand15.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBand15.RightToLeft")));
			this.btnBand15.Size = ((System.Drawing.Size)(resources.GetObject("btnBand15.Size")));
			this.btnBand15.TabIndex = ((int)(resources.GetObject("btnBand15.TabIndex")));
			this.btnBand15.Text = resources.GetString("btnBand15.Text");
			this.btnBand15.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand15.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBand15, resources.GetString("btnBand15.ToolTip"));
			this.btnBand15.Visible = ((bool)(resources.GetObject("btnBand15.Visible")));
			this.btnBand15.Click += new System.EventHandler(this.btnBand15_Click);
			// 
			// btnBand17
			// 
			this.btnBand17.AccessibleDescription = resources.GetString("btnBand17.AccessibleDescription");
			this.btnBand17.AccessibleName = resources.GetString("btnBand17.AccessibleName");
			this.btnBand17.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBand17.Anchor")));
			this.btnBand17.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBand17.BackgroundImage")));
			this.btnBand17.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBand17.Dock")));
			this.btnBand17.Enabled = ((bool)(resources.GetObject("btnBand17.Enabled")));
			this.btnBand17.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBand17.FlatStyle")));
			this.btnBand17.Font = ((System.Drawing.Font)(resources.GetObject("btnBand17.Font")));
			this.btnBand17.Image = ((System.Drawing.Image)(resources.GetObject("btnBand17.Image")));
			this.btnBand17.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand17.ImageAlign")));
			this.btnBand17.ImageIndex = ((int)(resources.GetObject("btnBand17.ImageIndex")));
			this.btnBand17.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBand17.ImeMode")));
			this.btnBand17.Location = ((System.Drawing.Point)(resources.GetObject("btnBand17.Location")));
			this.btnBand17.Name = "btnBand17";
			this.btnBand17.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBand17.RightToLeft")));
			this.btnBand17.Size = ((System.Drawing.Size)(resources.GetObject("btnBand17.Size")));
			this.btnBand17.TabIndex = ((int)(resources.GetObject("btnBand17.TabIndex")));
			this.btnBand17.Text = resources.GetString("btnBand17.Text");
			this.btnBand17.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand17.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBand17, resources.GetString("btnBand17.ToolTip"));
			this.btnBand17.Visible = ((bool)(resources.GetObject("btnBand17.Visible")));
			this.btnBand17.Click += new System.EventHandler(this.btnBand17_Click);
			// 
			// btnBand20
			// 
			this.btnBand20.AccessibleDescription = resources.GetString("btnBand20.AccessibleDescription");
			this.btnBand20.AccessibleName = resources.GetString("btnBand20.AccessibleName");
			this.btnBand20.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBand20.Anchor")));
			this.btnBand20.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBand20.BackgroundImage")));
			this.btnBand20.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBand20.Dock")));
			this.btnBand20.Enabled = ((bool)(resources.GetObject("btnBand20.Enabled")));
			this.btnBand20.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBand20.FlatStyle")));
			this.btnBand20.Font = ((System.Drawing.Font)(resources.GetObject("btnBand20.Font")));
			this.btnBand20.Image = ((System.Drawing.Image)(resources.GetObject("btnBand20.Image")));
			this.btnBand20.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand20.ImageAlign")));
			this.btnBand20.ImageIndex = ((int)(resources.GetObject("btnBand20.ImageIndex")));
			this.btnBand20.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBand20.ImeMode")));
			this.btnBand20.Location = ((System.Drawing.Point)(resources.GetObject("btnBand20.Location")));
			this.btnBand20.Name = "btnBand20";
			this.btnBand20.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBand20.RightToLeft")));
			this.btnBand20.Size = ((System.Drawing.Size)(resources.GetObject("btnBand20.Size")));
			this.btnBand20.TabIndex = ((int)(resources.GetObject("btnBand20.TabIndex")));
			this.btnBand20.Text = resources.GetString("btnBand20.Text");
			this.btnBand20.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand20.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBand20, resources.GetString("btnBand20.ToolTip"));
			this.btnBand20.Visible = ((bool)(resources.GetObject("btnBand20.Visible")));
			this.btnBand20.Click += new System.EventHandler(this.btnBand20_Click);
			// 
			// btnBand30
			// 
			this.btnBand30.AccessibleDescription = resources.GetString("btnBand30.AccessibleDescription");
			this.btnBand30.AccessibleName = resources.GetString("btnBand30.AccessibleName");
			this.btnBand30.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBand30.Anchor")));
			this.btnBand30.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBand30.BackgroundImage")));
			this.btnBand30.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBand30.Dock")));
			this.btnBand30.Enabled = ((bool)(resources.GetObject("btnBand30.Enabled")));
			this.btnBand30.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBand30.FlatStyle")));
			this.btnBand30.Font = ((System.Drawing.Font)(resources.GetObject("btnBand30.Font")));
			this.btnBand30.Image = ((System.Drawing.Image)(resources.GetObject("btnBand30.Image")));
			this.btnBand30.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand30.ImageAlign")));
			this.btnBand30.ImageIndex = ((int)(resources.GetObject("btnBand30.ImageIndex")));
			this.btnBand30.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBand30.ImeMode")));
			this.btnBand30.Location = ((System.Drawing.Point)(resources.GetObject("btnBand30.Location")));
			this.btnBand30.Name = "btnBand30";
			this.btnBand30.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBand30.RightToLeft")));
			this.btnBand30.Size = ((System.Drawing.Size)(resources.GetObject("btnBand30.Size")));
			this.btnBand30.TabIndex = ((int)(resources.GetObject("btnBand30.TabIndex")));
			this.btnBand30.Text = resources.GetString("btnBand30.Text");
			this.btnBand30.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand30.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBand30, resources.GetString("btnBand30.ToolTip"));
			this.btnBand30.Visible = ((bool)(resources.GetObject("btnBand30.Visible")));
			this.btnBand30.Click += new System.EventHandler(this.btnBand30_Click);
			// 
			// btnBand40
			// 
			this.btnBand40.AccessibleDescription = resources.GetString("btnBand40.AccessibleDescription");
			this.btnBand40.AccessibleName = resources.GetString("btnBand40.AccessibleName");
			this.btnBand40.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBand40.Anchor")));
			this.btnBand40.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBand40.BackgroundImage")));
			this.btnBand40.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBand40.Dock")));
			this.btnBand40.Enabled = ((bool)(resources.GetObject("btnBand40.Enabled")));
			this.btnBand40.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBand40.FlatStyle")));
			this.btnBand40.Font = ((System.Drawing.Font)(resources.GetObject("btnBand40.Font")));
			this.btnBand40.Image = ((System.Drawing.Image)(resources.GetObject("btnBand40.Image")));
			this.btnBand40.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand40.ImageAlign")));
			this.btnBand40.ImageIndex = ((int)(resources.GetObject("btnBand40.ImageIndex")));
			this.btnBand40.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBand40.ImeMode")));
			this.btnBand40.Location = ((System.Drawing.Point)(resources.GetObject("btnBand40.Location")));
			this.btnBand40.Name = "btnBand40";
			this.btnBand40.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBand40.RightToLeft")));
			this.btnBand40.Size = ((System.Drawing.Size)(resources.GetObject("btnBand40.Size")));
			this.btnBand40.TabIndex = ((int)(resources.GetObject("btnBand40.TabIndex")));
			this.btnBand40.Text = resources.GetString("btnBand40.Text");
			this.btnBand40.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand40.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBand40, resources.GetString("btnBand40.ToolTip"));
			this.btnBand40.Visible = ((bool)(resources.GetObject("btnBand40.Visible")));
			this.btnBand40.Click += new System.EventHandler(this.btnBand40_Click);
			// 
			// btnBand60
			// 
			this.btnBand60.AccessibleDescription = resources.GetString("btnBand60.AccessibleDescription");
			this.btnBand60.AccessibleName = resources.GetString("btnBand60.AccessibleName");
			this.btnBand60.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBand60.Anchor")));
			this.btnBand60.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBand60.BackgroundImage")));
			this.btnBand60.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBand60.Dock")));
			this.btnBand60.Enabled = ((bool)(resources.GetObject("btnBand60.Enabled")));
			this.btnBand60.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBand60.FlatStyle")));
			this.btnBand60.Font = ((System.Drawing.Font)(resources.GetObject("btnBand60.Font")));
			this.btnBand60.Image = ((System.Drawing.Image)(resources.GetObject("btnBand60.Image")));
			this.btnBand60.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand60.ImageAlign")));
			this.btnBand60.ImageIndex = ((int)(resources.GetObject("btnBand60.ImageIndex")));
			this.btnBand60.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBand60.ImeMode")));
			this.btnBand60.Location = ((System.Drawing.Point)(resources.GetObject("btnBand60.Location")));
			this.btnBand60.Name = "btnBand60";
			this.btnBand60.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBand60.RightToLeft")));
			this.btnBand60.Size = ((System.Drawing.Size)(resources.GetObject("btnBand60.Size")));
			this.btnBand60.TabIndex = ((int)(resources.GetObject("btnBand60.TabIndex")));
			this.btnBand60.Text = resources.GetString("btnBand60.Text");
			this.btnBand60.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand60.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBand60, resources.GetString("btnBand60.ToolTip"));
			this.btnBand60.Visible = ((bool)(resources.GetObject("btnBand60.Visible")));
			this.btnBand60.Click += new System.EventHandler(this.btnBand60_Click);
			// 
			// btnBand80
			// 
			this.btnBand80.AccessibleDescription = resources.GetString("btnBand80.AccessibleDescription");
			this.btnBand80.AccessibleName = resources.GetString("btnBand80.AccessibleName");
			this.btnBand80.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBand80.Anchor")));
			this.btnBand80.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBand80.BackgroundImage")));
			this.btnBand80.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBand80.Dock")));
			this.btnBand80.Enabled = ((bool)(resources.GetObject("btnBand80.Enabled")));
			this.btnBand80.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBand80.FlatStyle")));
			this.btnBand80.Font = ((System.Drawing.Font)(resources.GetObject("btnBand80.Font")));
			this.btnBand80.Image = ((System.Drawing.Image)(resources.GetObject("btnBand80.Image")));
			this.btnBand80.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand80.ImageAlign")));
			this.btnBand80.ImageIndex = ((int)(resources.GetObject("btnBand80.ImageIndex")));
			this.btnBand80.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBand80.ImeMode")));
			this.btnBand80.Location = ((System.Drawing.Point)(resources.GetObject("btnBand80.Location")));
			this.btnBand80.Name = "btnBand80";
			this.btnBand80.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBand80.RightToLeft")));
			this.btnBand80.Size = ((System.Drawing.Size)(resources.GetObject("btnBand80.Size")));
			this.btnBand80.TabIndex = ((int)(resources.GetObject("btnBand80.TabIndex")));
			this.btnBand80.Text = resources.GetString("btnBand80.Text");
			this.btnBand80.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand80.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBand80, resources.GetString("btnBand80.ToolTip"));
			this.btnBand80.Visible = ((bool)(resources.GetObject("btnBand80.Visible")));
			this.btnBand80.Click += new System.EventHandler(this.btnBand80_Click);
			// 
			// btnBand160
			// 
			this.btnBand160.AccessibleDescription = resources.GetString("btnBand160.AccessibleDescription");
			this.btnBand160.AccessibleName = resources.GetString("btnBand160.AccessibleName");
			this.btnBand160.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBand160.Anchor")));
			this.btnBand160.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBand160.BackgroundImage")));
			this.btnBand160.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBand160.Dock")));
			this.btnBand160.Enabled = ((bool)(resources.GetObject("btnBand160.Enabled")));
			this.btnBand160.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBand160.FlatStyle")));
			this.btnBand160.Font = ((System.Drawing.Font)(resources.GetObject("btnBand160.Font")));
			this.btnBand160.Image = ((System.Drawing.Image)(resources.GetObject("btnBand160.Image")));
			this.btnBand160.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand160.ImageAlign")));
			this.btnBand160.ImageIndex = ((int)(resources.GetObject("btnBand160.ImageIndex")));
			this.btnBand160.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBand160.ImeMode")));
			this.btnBand160.Location = ((System.Drawing.Point)(resources.GetObject("btnBand160.Location")));
			this.btnBand160.Name = "btnBand160";
			this.btnBand160.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBand160.RightToLeft")));
			this.btnBand160.Size = ((System.Drawing.Size)(resources.GetObject("btnBand160.Size")));
			this.btnBand160.TabIndex = ((int)(resources.GetObject("btnBand160.TabIndex")));
			this.btnBand160.Text = resources.GetString("btnBand160.Text");
			this.btnBand160.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBand160.TextAlign")));
			this.toolTip1.SetToolTip(this.btnBand160, resources.GetString("btnBand160.ToolTip"));
			this.btnBand160.Visible = ((bool)(resources.GetObject("btnBand160.Visible")));
			this.btnBand160.Click += new System.EventHandler(this.btnBand160_Click);
			// 
			// grpFilter
			// 
			this.grpFilter.AccessibleDescription = resources.GetString("grpFilter.AccessibleDescription");
			this.grpFilter.AccessibleName = resources.GetString("grpFilter.AccessibleName");
			this.grpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpFilter.Anchor")));
			this.grpFilter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpFilter.BackgroundImage")));
			this.grpFilter.Controls.Add(this.tbFilterWidth);
			this.grpFilter.Controls.Add(this.lblFilterWidth);
			this.grpFilter.Controls.Add(this.btnFilterShiftReset);
			this.grpFilter.Controls.Add(this.tbFilterShift);
			this.grpFilter.Controls.Add(this.lblFilterShift);
			this.grpFilter.Controls.Add(this.radFilter6000);
			this.grpFilter.Controls.Add(this.radFilter4000);
			this.grpFilter.Controls.Add(this.radFilter2600);
			this.grpFilter.Controls.Add(this.radFilter2100);
			this.grpFilter.Controls.Add(this.radFilter1000);
			this.grpFilter.Controls.Add(this.radFilter500);
			this.grpFilter.Controls.Add(this.radFilter250);
			this.grpFilter.Controls.Add(this.radFilter100);
			this.grpFilter.Controls.Add(this.radFilter50);
			this.grpFilter.Controls.Add(this.radFilter25);
			this.grpFilter.Controls.Add(this.radFilterVar1);
			this.grpFilter.Controls.Add(this.radFilterVar2);
			this.grpFilter.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpFilter.Dock")));
			this.grpFilter.Enabled = ((bool)(resources.GetObject("grpFilter.Enabled")));
			this.grpFilter.Font = ((System.Drawing.Font)(resources.GetObject("grpFilter.Font")));
			this.grpFilter.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpFilter.ImeMode")));
			this.grpFilter.Location = ((System.Drawing.Point)(resources.GetObject("grpFilter.Location")));
			this.grpFilter.Name = "grpFilter";
			this.grpFilter.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpFilter.RightToLeft")));
			this.grpFilter.Size = ((System.Drawing.Size)(resources.GetObject("grpFilter.Size")));
			this.grpFilter.TabIndex = ((int)(resources.GetObject("grpFilter.TabIndex")));
			this.grpFilter.TabStop = false;
			this.grpFilter.Text = resources.GetString("grpFilter.Text");
			this.toolTip1.SetToolTip(this.grpFilter, resources.GetString("grpFilter.ToolTip"));
			this.grpFilter.Visible = ((bool)(resources.GetObject("grpFilter.Visible")));
			// 
			// tbFilterWidth
			// 
			this.tbFilterWidth.AccessibleDescription = resources.GetString("tbFilterWidth.AccessibleDescription");
			this.tbFilterWidth.AccessibleName = resources.GetString("tbFilterWidth.AccessibleName");
			this.tbFilterWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tbFilterWidth.Anchor")));
			this.tbFilterWidth.AutoSize = false;
			this.tbFilterWidth.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tbFilterWidth.BackgroundImage")));
			this.tbFilterWidth.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tbFilterWidth.Dock")));
			this.tbFilterWidth.Enabled = ((bool)(resources.GetObject("tbFilterWidth.Enabled")));
			this.tbFilterWidth.Font = ((System.Drawing.Font)(resources.GetObject("tbFilterWidth.Font")));
			this.tbFilterWidth.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tbFilterWidth.ImeMode")));
			this.tbFilterWidth.Location = ((System.Drawing.Point)(resources.GetObject("tbFilterWidth.Location")));
			this.tbFilterWidth.Maximum = 10000;
			this.tbFilterWidth.Minimum = 1;
			this.tbFilterWidth.Name = "tbFilterWidth";
			this.tbFilterWidth.Orientation = ((System.Windows.Forms.Orientation)(resources.GetObject("tbFilterWidth.Orientation")));
			this.tbFilterWidth.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tbFilterWidth.RightToLeft")));
			this.tbFilterWidth.Size = ((System.Drawing.Size)(resources.GetObject("tbFilterWidth.Size")));
			this.tbFilterWidth.TabIndex = ((int)(resources.GetObject("tbFilterWidth.TabIndex")));
			this.tbFilterWidth.Text = resources.GetString("tbFilterWidth.Text");
			this.tbFilterWidth.TickStyle = System.Windows.Forms.TickStyle.None;
			this.toolTip1.SetToolTip(this.tbFilterWidth, resources.GetString("tbFilterWidth.ToolTip"));
			this.tbFilterWidth.Value = 1;
			this.tbFilterWidth.Visible = ((bool)(resources.GetObject("tbFilterWidth.Visible")));
			this.tbFilterWidth.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Console_MouseWheel);
			this.tbFilterWidth.Scroll += new System.EventHandler(this.tbFilterWidth_Scroll);
			// 
			// lblFilterWidth
			// 
			this.lblFilterWidth.AccessibleDescription = resources.GetString("lblFilterWidth.AccessibleDescription");
			this.lblFilterWidth.AccessibleName = resources.GetString("lblFilterWidth.AccessibleName");
			this.lblFilterWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblFilterWidth.Anchor")));
			this.lblFilterWidth.AutoSize = ((bool)(resources.GetObject("lblFilterWidth.AutoSize")));
			this.lblFilterWidth.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblFilterWidth.Dock")));
			this.lblFilterWidth.Enabled = ((bool)(resources.GetObject("lblFilterWidth.Enabled")));
			this.lblFilterWidth.Font = ((System.Drawing.Font)(resources.GetObject("lblFilterWidth.Font")));
			this.lblFilterWidth.Image = ((System.Drawing.Image)(resources.GetObject("lblFilterWidth.Image")));
			this.lblFilterWidth.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFilterWidth.ImageAlign")));
			this.lblFilterWidth.ImageIndex = ((int)(resources.GetObject("lblFilterWidth.ImageIndex")));
			this.lblFilterWidth.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblFilterWidth.ImeMode")));
			this.lblFilterWidth.Location = ((System.Drawing.Point)(resources.GetObject("lblFilterWidth.Location")));
			this.lblFilterWidth.Name = "lblFilterWidth";
			this.lblFilterWidth.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblFilterWidth.RightToLeft")));
			this.lblFilterWidth.Size = ((System.Drawing.Size)(resources.GetObject("lblFilterWidth.Size")));
			this.lblFilterWidth.TabIndex = ((int)(resources.GetObject("lblFilterWidth.TabIndex")));
			this.lblFilterWidth.Text = resources.GetString("lblFilterWidth.Text");
			this.lblFilterWidth.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFilterWidth.TextAlign")));
			this.toolTip1.SetToolTip(this.lblFilterWidth, resources.GetString("lblFilterWidth.ToolTip"));
			this.lblFilterWidth.Visible = ((bool)(resources.GetObject("lblFilterWidth.Visible")));
			// 
			// btnFilterShiftReset
			// 
			this.btnFilterShiftReset.AccessibleDescription = resources.GetString("btnFilterShiftReset.AccessibleDescription");
			this.btnFilterShiftReset.AccessibleName = resources.GetString("btnFilterShiftReset.AccessibleName");
			this.btnFilterShiftReset.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnFilterShiftReset.Anchor")));
			this.btnFilterShiftReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFilterShiftReset.BackgroundImage")));
			this.btnFilterShiftReset.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnFilterShiftReset.Dock")));
			this.btnFilterShiftReset.Enabled = ((bool)(resources.GetObject("btnFilterShiftReset.Enabled")));
			this.btnFilterShiftReset.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnFilterShiftReset.FlatStyle")));
			this.btnFilterShiftReset.Font = ((System.Drawing.Font)(resources.GetObject("btnFilterShiftReset.Font")));
			this.btnFilterShiftReset.Image = ((System.Drawing.Image)(resources.GetObject("btnFilterShiftReset.Image")));
			this.btnFilterShiftReset.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnFilterShiftReset.ImageAlign")));
			this.btnFilterShiftReset.ImageIndex = ((int)(resources.GetObject("btnFilterShiftReset.ImageIndex")));
			this.btnFilterShiftReset.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnFilterShiftReset.ImeMode")));
			this.btnFilterShiftReset.Location = ((System.Drawing.Point)(resources.GetObject("btnFilterShiftReset.Location")));
			this.btnFilterShiftReset.Name = "btnFilterShiftReset";
			this.btnFilterShiftReset.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnFilterShiftReset.RightToLeft")));
			this.btnFilterShiftReset.Size = ((System.Drawing.Size)(resources.GetObject("btnFilterShiftReset.Size")));
			this.btnFilterShiftReset.TabIndex = ((int)(resources.GetObject("btnFilterShiftReset.TabIndex")));
			this.btnFilterShiftReset.Tag = "Reset Filter Shift";
			this.btnFilterShiftReset.Text = resources.GetString("btnFilterShiftReset.Text");
			this.btnFilterShiftReset.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnFilterShiftReset.TextAlign")));
			this.toolTip1.SetToolTip(this.btnFilterShiftReset, resources.GetString("btnFilterShiftReset.ToolTip"));
			this.btnFilterShiftReset.Visible = ((bool)(resources.GetObject("btnFilterShiftReset.Visible")));
			this.btnFilterShiftReset.Click += new System.EventHandler(this.btnFilterShiftReset_Click);
			// 
			// tbFilterShift
			// 
			this.tbFilterShift.AccessibleDescription = resources.GetString("tbFilterShift.AccessibleDescription");
			this.tbFilterShift.AccessibleName = resources.GetString("tbFilterShift.AccessibleName");
			this.tbFilterShift.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tbFilterShift.Anchor")));
			this.tbFilterShift.AutoSize = false;
			this.tbFilterShift.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tbFilterShift.BackgroundImage")));
			this.tbFilterShift.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tbFilterShift.Dock")));
			this.tbFilterShift.Enabled = ((bool)(resources.GetObject("tbFilterShift.Enabled")));
			this.tbFilterShift.Font = ((System.Drawing.Font)(resources.GetObject("tbFilterShift.Font")));
			this.tbFilterShift.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tbFilterShift.ImeMode")));
			this.tbFilterShift.Location = ((System.Drawing.Point)(resources.GetObject("tbFilterShift.Location")));
			this.tbFilterShift.Maximum = 1000;
			this.tbFilterShift.Minimum = -1000;
			this.tbFilterShift.Name = "tbFilterShift";
			this.tbFilterShift.Orientation = ((System.Windows.Forms.Orientation)(resources.GetObject("tbFilterShift.Orientation")));
			this.tbFilterShift.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tbFilterShift.RightToLeft")));
			this.tbFilterShift.Size = ((System.Drawing.Size)(resources.GetObject("tbFilterShift.Size")));
			this.tbFilterShift.TabIndex = ((int)(resources.GetObject("tbFilterShift.TabIndex")));
			this.tbFilterShift.Text = resources.GetString("tbFilterShift.Text");
			this.tbFilterShift.TickStyle = System.Windows.Forms.TickStyle.None;
			this.toolTip1.SetToolTip(this.tbFilterShift, resources.GetString("tbFilterShift.ToolTip"));
			this.tbFilterShift.Visible = ((bool)(resources.GetObject("tbFilterShift.Visible")));
			this.tbFilterShift.Scroll += new System.EventHandler(this.tbFilterShift_Scroll);
			// 
			// lblFilterShift
			// 
			this.lblFilterShift.AccessibleDescription = resources.GetString("lblFilterShift.AccessibleDescription");
			this.lblFilterShift.AccessibleName = resources.GetString("lblFilterShift.AccessibleName");
			this.lblFilterShift.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblFilterShift.Anchor")));
			this.lblFilterShift.AutoSize = ((bool)(resources.GetObject("lblFilterShift.AutoSize")));
			this.lblFilterShift.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblFilterShift.Dock")));
			this.lblFilterShift.Enabled = ((bool)(resources.GetObject("lblFilterShift.Enabled")));
			this.lblFilterShift.Font = ((System.Drawing.Font)(resources.GetObject("lblFilterShift.Font")));
			this.lblFilterShift.Image = ((System.Drawing.Image)(resources.GetObject("lblFilterShift.Image")));
			this.lblFilterShift.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFilterShift.ImageAlign")));
			this.lblFilterShift.ImageIndex = ((int)(resources.GetObject("lblFilterShift.ImageIndex")));
			this.lblFilterShift.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblFilterShift.ImeMode")));
			this.lblFilterShift.Location = ((System.Drawing.Point)(resources.GetObject("lblFilterShift.Location")));
			this.lblFilterShift.Name = "lblFilterShift";
			this.lblFilterShift.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblFilterShift.RightToLeft")));
			this.lblFilterShift.Size = ((System.Drawing.Size)(resources.GetObject("lblFilterShift.Size")));
			this.lblFilterShift.TabIndex = ((int)(resources.GetObject("lblFilterShift.TabIndex")));
			this.lblFilterShift.Text = resources.GetString("lblFilterShift.Text");
			this.lblFilterShift.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFilterShift.TextAlign")));
			this.toolTip1.SetToolTip(this.lblFilterShift, resources.GetString("lblFilterShift.ToolTip"));
			this.lblFilterShift.Visible = ((bool)(resources.GetObject("lblFilterShift.Visible")));
			// 
			// radFilter6000
			// 
			this.radFilter6000.AccessibleDescription = resources.GetString("radFilter6000.AccessibleDescription");
			this.radFilter6000.AccessibleName = resources.GetString("radFilter6000.AccessibleName");
			this.radFilter6000.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radFilter6000.Anchor")));
			this.radFilter6000.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radFilter6000.Appearance")));
			this.radFilter6000.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radFilter6000.BackgroundImage")));
			this.radFilter6000.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter6000.CheckAlign")));
			this.radFilter6000.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radFilter6000.Dock")));
			this.radFilter6000.Enabled = ((bool)(resources.GetObject("radFilter6000.Enabled")));
			this.radFilter6000.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radFilter6000.FlatStyle")));
			this.radFilter6000.Font = ((System.Drawing.Font)(resources.GetObject("radFilter6000.Font")));
			this.radFilter6000.Image = ((System.Drawing.Image)(resources.GetObject("radFilter6000.Image")));
			this.radFilter6000.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter6000.ImageAlign")));
			this.radFilter6000.ImageIndex = ((int)(resources.GetObject("radFilter6000.ImageIndex")));
			this.radFilter6000.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radFilter6000.ImeMode")));
			this.radFilter6000.Location = ((System.Drawing.Point)(resources.GetObject("radFilter6000.Location")));
			this.radFilter6000.Name = "radFilter6000";
			this.radFilter6000.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radFilter6000.RightToLeft")));
			this.radFilter6000.Size = ((System.Drawing.Size)(resources.GetObject("radFilter6000.Size")));
			this.radFilter6000.TabIndex = ((int)(resources.GetObject("radFilter6000.TabIndex")));
			this.radFilter6000.Text = resources.GetString("radFilter6000.Text");
			this.radFilter6000.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter6000.TextAlign")));
			this.toolTip1.SetToolTip(this.radFilter6000, resources.GetString("radFilter6000.ToolTip"));
			this.radFilter6000.Visible = ((bool)(resources.GetObject("radFilter6000.Visible")));
			this.radFilter6000.CheckedChanged += new System.EventHandler(this.radFilter6000_CheckedChanged);
			// 
			// radFilter4000
			// 
			this.radFilter4000.AccessibleDescription = resources.GetString("radFilter4000.AccessibleDescription");
			this.radFilter4000.AccessibleName = resources.GetString("radFilter4000.AccessibleName");
			this.radFilter4000.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radFilter4000.Anchor")));
			this.radFilter4000.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radFilter4000.Appearance")));
			this.radFilter4000.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radFilter4000.BackgroundImage")));
			this.radFilter4000.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter4000.CheckAlign")));
			this.radFilter4000.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radFilter4000.Dock")));
			this.radFilter4000.Enabled = ((bool)(resources.GetObject("radFilter4000.Enabled")));
			this.radFilter4000.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radFilter4000.FlatStyle")));
			this.radFilter4000.Font = ((System.Drawing.Font)(resources.GetObject("radFilter4000.Font")));
			this.radFilter4000.Image = ((System.Drawing.Image)(resources.GetObject("radFilter4000.Image")));
			this.radFilter4000.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter4000.ImageAlign")));
			this.radFilter4000.ImageIndex = ((int)(resources.GetObject("radFilter4000.ImageIndex")));
			this.radFilter4000.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radFilter4000.ImeMode")));
			this.radFilter4000.Location = ((System.Drawing.Point)(resources.GetObject("radFilter4000.Location")));
			this.radFilter4000.Name = "radFilter4000";
			this.radFilter4000.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radFilter4000.RightToLeft")));
			this.radFilter4000.Size = ((System.Drawing.Size)(resources.GetObject("radFilter4000.Size")));
			this.radFilter4000.TabIndex = ((int)(resources.GetObject("radFilter4000.TabIndex")));
			this.radFilter4000.Text = resources.GetString("radFilter4000.Text");
			this.radFilter4000.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter4000.TextAlign")));
			this.toolTip1.SetToolTip(this.radFilter4000, resources.GetString("radFilter4000.ToolTip"));
			this.radFilter4000.Visible = ((bool)(resources.GetObject("radFilter4000.Visible")));
			this.radFilter4000.CheckedChanged += new System.EventHandler(this.radFilter4000_CheckedChanged);
			// 
			// radFilter2600
			// 
			this.radFilter2600.AccessibleDescription = resources.GetString("radFilter2600.AccessibleDescription");
			this.radFilter2600.AccessibleName = resources.GetString("radFilter2600.AccessibleName");
			this.radFilter2600.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radFilter2600.Anchor")));
			this.radFilter2600.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radFilter2600.Appearance")));
			this.radFilter2600.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radFilter2600.BackgroundImage")));
			this.radFilter2600.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter2600.CheckAlign")));
			this.radFilter2600.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radFilter2600.Dock")));
			this.radFilter2600.Enabled = ((bool)(resources.GetObject("radFilter2600.Enabled")));
			this.radFilter2600.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radFilter2600.FlatStyle")));
			this.radFilter2600.Font = ((System.Drawing.Font)(resources.GetObject("radFilter2600.Font")));
			this.radFilter2600.Image = ((System.Drawing.Image)(resources.GetObject("radFilter2600.Image")));
			this.radFilter2600.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter2600.ImageAlign")));
			this.radFilter2600.ImageIndex = ((int)(resources.GetObject("radFilter2600.ImageIndex")));
			this.radFilter2600.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radFilter2600.ImeMode")));
			this.radFilter2600.Location = ((System.Drawing.Point)(resources.GetObject("radFilter2600.Location")));
			this.radFilter2600.Name = "radFilter2600";
			this.radFilter2600.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radFilter2600.RightToLeft")));
			this.radFilter2600.Size = ((System.Drawing.Size)(resources.GetObject("radFilter2600.Size")));
			this.radFilter2600.TabIndex = ((int)(resources.GetObject("radFilter2600.TabIndex")));
			this.radFilter2600.Text = resources.GetString("radFilter2600.Text");
			this.radFilter2600.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter2600.TextAlign")));
			this.toolTip1.SetToolTip(this.radFilter2600, resources.GetString("radFilter2600.ToolTip"));
			this.radFilter2600.Visible = ((bool)(resources.GetObject("radFilter2600.Visible")));
			this.radFilter2600.CheckedChanged += new System.EventHandler(this.radFilter2600_CheckedChanged);
			// 
			// radFilter2100
			// 
			this.radFilter2100.AccessibleDescription = resources.GetString("radFilter2100.AccessibleDescription");
			this.radFilter2100.AccessibleName = resources.GetString("radFilter2100.AccessibleName");
			this.radFilter2100.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radFilter2100.Anchor")));
			this.radFilter2100.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radFilter2100.Appearance")));
			this.radFilter2100.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radFilter2100.BackgroundImage")));
			this.radFilter2100.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter2100.CheckAlign")));
			this.radFilter2100.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radFilter2100.Dock")));
			this.radFilter2100.Enabled = ((bool)(resources.GetObject("radFilter2100.Enabled")));
			this.radFilter2100.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radFilter2100.FlatStyle")));
			this.radFilter2100.Font = ((System.Drawing.Font)(resources.GetObject("radFilter2100.Font")));
			this.radFilter2100.Image = ((System.Drawing.Image)(resources.GetObject("radFilter2100.Image")));
			this.radFilter2100.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter2100.ImageAlign")));
			this.radFilter2100.ImageIndex = ((int)(resources.GetObject("radFilter2100.ImageIndex")));
			this.radFilter2100.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radFilter2100.ImeMode")));
			this.radFilter2100.Location = ((System.Drawing.Point)(resources.GetObject("radFilter2100.Location")));
			this.radFilter2100.Name = "radFilter2100";
			this.radFilter2100.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radFilter2100.RightToLeft")));
			this.radFilter2100.Size = ((System.Drawing.Size)(resources.GetObject("radFilter2100.Size")));
			this.radFilter2100.TabIndex = ((int)(resources.GetObject("radFilter2100.TabIndex")));
			this.radFilter2100.Text = resources.GetString("radFilter2100.Text");
			this.radFilter2100.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter2100.TextAlign")));
			this.toolTip1.SetToolTip(this.radFilter2100, resources.GetString("radFilter2100.ToolTip"));
			this.radFilter2100.Visible = ((bool)(resources.GetObject("radFilter2100.Visible")));
			this.radFilter2100.CheckedChanged += new System.EventHandler(this.radFilter2100_CheckedChanged);
			// 
			// radFilter1000
			// 
			this.radFilter1000.AccessibleDescription = resources.GetString("radFilter1000.AccessibleDescription");
			this.radFilter1000.AccessibleName = resources.GetString("radFilter1000.AccessibleName");
			this.radFilter1000.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radFilter1000.Anchor")));
			this.radFilter1000.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radFilter1000.Appearance")));
			this.radFilter1000.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radFilter1000.BackgroundImage")));
			this.radFilter1000.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter1000.CheckAlign")));
			this.radFilter1000.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radFilter1000.Dock")));
			this.radFilter1000.Enabled = ((bool)(resources.GetObject("radFilter1000.Enabled")));
			this.radFilter1000.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radFilter1000.FlatStyle")));
			this.radFilter1000.Font = ((System.Drawing.Font)(resources.GetObject("radFilter1000.Font")));
			this.radFilter1000.Image = ((System.Drawing.Image)(resources.GetObject("radFilter1000.Image")));
			this.radFilter1000.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter1000.ImageAlign")));
			this.radFilter1000.ImageIndex = ((int)(resources.GetObject("radFilter1000.ImageIndex")));
			this.radFilter1000.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radFilter1000.ImeMode")));
			this.radFilter1000.Location = ((System.Drawing.Point)(resources.GetObject("radFilter1000.Location")));
			this.radFilter1000.Name = "radFilter1000";
			this.radFilter1000.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radFilter1000.RightToLeft")));
			this.radFilter1000.Size = ((System.Drawing.Size)(resources.GetObject("radFilter1000.Size")));
			this.radFilter1000.TabIndex = ((int)(resources.GetObject("radFilter1000.TabIndex")));
			this.radFilter1000.Text = resources.GetString("radFilter1000.Text");
			this.radFilter1000.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter1000.TextAlign")));
			this.toolTip1.SetToolTip(this.radFilter1000, resources.GetString("radFilter1000.ToolTip"));
			this.radFilter1000.Visible = ((bool)(resources.GetObject("radFilter1000.Visible")));
			this.radFilter1000.CheckedChanged += new System.EventHandler(this.radFilter1000_CheckedChanged);
			// 
			// radFilter500
			// 
			this.radFilter500.AccessibleDescription = resources.GetString("radFilter500.AccessibleDescription");
			this.radFilter500.AccessibleName = resources.GetString("radFilter500.AccessibleName");
			this.radFilter500.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radFilter500.Anchor")));
			this.radFilter500.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radFilter500.Appearance")));
			this.radFilter500.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radFilter500.BackgroundImage")));
			this.radFilter500.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter500.CheckAlign")));
			this.radFilter500.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radFilter500.Dock")));
			this.radFilter500.Enabled = ((bool)(resources.GetObject("radFilter500.Enabled")));
			this.radFilter500.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radFilter500.FlatStyle")));
			this.radFilter500.Font = ((System.Drawing.Font)(resources.GetObject("radFilter500.Font")));
			this.radFilter500.Image = ((System.Drawing.Image)(resources.GetObject("radFilter500.Image")));
			this.radFilter500.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter500.ImageAlign")));
			this.radFilter500.ImageIndex = ((int)(resources.GetObject("radFilter500.ImageIndex")));
			this.radFilter500.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radFilter500.ImeMode")));
			this.radFilter500.Location = ((System.Drawing.Point)(resources.GetObject("radFilter500.Location")));
			this.radFilter500.Name = "radFilter500";
			this.radFilter500.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radFilter500.RightToLeft")));
			this.radFilter500.Size = ((System.Drawing.Size)(resources.GetObject("radFilter500.Size")));
			this.radFilter500.TabIndex = ((int)(resources.GetObject("radFilter500.TabIndex")));
			this.radFilter500.Text = resources.GetString("radFilter500.Text");
			this.radFilter500.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter500.TextAlign")));
			this.toolTip1.SetToolTip(this.radFilter500, resources.GetString("radFilter500.ToolTip"));
			this.radFilter500.Visible = ((bool)(resources.GetObject("radFilter500.Visible")));
			this.radFilter500.CheckedChanged += new System.EventHandler(this.radFilter500_CheckedChanged);
			// 
			// radFilter250
			// 
			this.radFilter250.AccessibleDescription = resources.GetString("radFilter250.AccessibleDescription");
			this.radFilter250.AccessibleName = resources.GetString("radFilter250.AccessibleName");
			this.radFilter250.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radFilter250.Anchor")));
			this.radFilter250.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radFilter250.Appearance")));
			this.radFilter250.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radFilter250.BackgroundImage")));
			this.radFilter250.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter250.CheckAlign")));
			this.radFilter250.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radFilter250.Dock")));
			this.radFilter250.Enabled = ((bool)(resources.GetObject("radFilter250.Enabled")));
			this.radFilter250.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radFilter250.FlatStyle")));
			this.radFilter250.Font = ((System.Drawing.Font)(resources.GetObject("radFilter250.Font")));
			this.radFilter250.Image = ((System.Drawing.Image)(resources.GetObject("radFilter250.Image")));
			this.radFilter250.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter250.ImageAlign")));
			this.radFilter250.ImageIndex = ((int)(resources.GetObject("radFilter250.ImageIndex")));
			this.radFilter250.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radFilter250.ImeMode")));
			this.radFilter250.Location = ((System.Drawing.Point)(resources.GetObject("radFilter250.Location")));
			this.radFilter250.Name = "radFilter250";
			this.radFilter250.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radFilter250.RightToLeft")));
			this.radFilter250.Size = ((System.Drawing.Size)(resources.GetObject("radFilter250.Size")));
			this.radFilter250.TabIndex = ((int)(resources.GetObject("radFilter250.TabIndex")));
			this.radFilter250.Text = resources.GetString("radFilter250.Text");
			this.radFilter250.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter250.TextAlign")));
			this.toolTip1.SetToolTip(this.radFilter250, resources.GetString("radFilter250.ToolTip"));
			this.radFilter250.Visible = ((bool)(resources.GetObject("radFilter250.Visible")));
			this.radFilter250.CheckedChanged += new System.EventHandler(this.radFilter250_CheckedChanged);
			// 
			// radFilter100
			// 
			this.radFilter100.AccessibleDescription = resources.GetString("radFilter100.AccessibleDescription");
			this.radFilter100.AccessibleName = resources.GetString("radFilter100.AccessibleName");
			this.radFilter100.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radFilter100.Anchor")));
			this.radFilter100.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radFilter100.Appearance")));
			this.radFilter100.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radFilter100.BackgroundImage")));
			this.radFilter100.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter100.CheckAlign")));
			this.radFilter100.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radFilter100.Dock")));
			this.radFilter100.Enabled = ((bool)(resources.GetObject("radFilter100.Enabled")));
			this.radFilter100.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radFilter100.FlatStyle")));
			this.radFilter100.Font = ((System.Drawing.Font)(resources.GetObject("radFilter100.Font")));
			this.radFilter100.Image = ((System.Drawing.Image)(resources.GetObject("radFilter100.Image")));
			this.radFilter100.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter100.ImageAlign")));
			this.radFilter100.ImageIndex = ((int)(resources.GetObject("radFilter100.ImageIndex")));
			this.radFilter100.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radFilter100.ImeMode")));
			this.radFilter100.Location = ((System.Drawing.Point)(resources.GetObject("radFilter100.Location")));
			this.radFilter100.Name = "radFilter100";
			this.radFilter100.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radFilter100.RightToLeft")));
			this.radFilter100.Size = ((System.Drawing.Size)(resources.GetObject("radFilter100.Size")));
			this.radFilter100.TabIndex = ((int)(resources.GetObject("radFilter100.TabIndex")));
			this.radFilter100.Text = resources.GetString("radFilter100.Text");
			this.radFilter100.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter100.TextAlign")));
			this.toolTip1.SetToolTip(this.radFilter100, resources.GetString("radFilter100.ToolTip"));
			this.radFilter100.Visible = ((bool)(resources.GetObject("radFilter100.Visible")));
			this.radFilter100.CheckedChanged += new System.EventHandler(this.radFilter100_CheckedChanged);
			// 
			// radFilter50
			// 
			this.radFilter50.AccessibleDescription = resources.GetString("radFilter50.AccessibleDescription");
			this.radFilter50.AccessibleName = resources.GetString("radFilter50.AccessibleName");
			this.radFilter50.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radFilter50.Anchor")));
			this.radFilter50.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radFilter50.Appearance")));
			this.radFilter50.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radFilter50.BackgroundImage")));
			this.radFilter50.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter50.CheckAlign")));
			this.radFilter50.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radFilter50.Dock")));
			this.radFilter50.Enabled = ((bool)(resources.GetObject("radFilter50.Enabled")));
			this.radFilter50.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radFilter50.FlatStyle")));
			this.radFilter50.Font = ((System.Drawing.Font)(resources.GetObject("radFilter50.Font")));
			this.radFilter50.Image = ((System.Drawing.Image)(resources.GetObject("radFilter50.Image")));
			this.radFilter50.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter50.ImageAlign")));
			this.radFilter50.ImageIndex = ((int)(resources.GetObject("radFilter50.ImageIndex")));
			this.radFilter50.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radFilter50.ImeMode")));
			this.radFilter50.Location = ((System.Drawing.Point)(resources.GetObject("radFilter50.Location")));
			this.radFilter50.Name = "radFilter50";
			this.radFilter50.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radFilter50.RightToLeft")));
			this.radFilter50.Size = ((System.Drawing.Size)(resources.GetObject("radFilter50.Size")));
			this.radFilter50.TabIndex = ((int)(resources.GetObject("radFilter50.TabIndex")));
			this.radFilter50.Text = resources.GetString("radFilter50.Text");
			this.radFilter50.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter50.TextAlign")));
			this.toolTip1.SetToolTip(this.radFilter50, resources.GetString("radFilter50.ToolTip"));
			this.radFilter50.Visible = ((bool)(resources.GetObject("radFilter50.Visible")));
			this.radFilter50.CheckedChanged += new System.EventHandler(this.radFilter50_CheckedChanged);
			// 
			// radFilter25
			// 
			this.radFilter25.AccessibleDescription = resources.GetString("radFilter25.AccessibleDescription");
			this.radFilter25.AccessibleName = resources.GetString("radFilter25.AccessibleName");
			this.radFilter25.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radFilter25.Anchor")));
			this.radFilter25.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radFilter25.Appearance")));
			this.radFilter25.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radFilter25.BackgroundImage")));
			this.radFilter25.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter25.CheckAlign")));
			this.radFilter25.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radFilter25.Dock")));
			this.radFilter25.Enabled = ((bool)(resources.GetObject("radFilter25.Enabled")));
			this.radFilter25.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radFilter25.FlatStyle")));
			this.radFilter25.Font = ((System.Drawing.Font)(resources.GetObject("radFilter25.Font")));
			this.radFilter25.Image = ((System.Drawing.Image)(resources.GetObject("radFilter25.Image")));
			this.radFilter25.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter25.ImageAlign")));
			this.radFilter25.ImageIndex = ((int)(resources.GetObject("radFilter25.ImageIndex")));
			this.radFilter25.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radFilter25.ImeMode")));
			this.radFilter25.Location = ((System.Drawing.Point)(resources.GetObject("radFilter25.Location")));
			this.radFilter25.Name = "radFilter25";
			this.radFilter25.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radFilter25.RightToLeft")));
			this.radFilter25.Size = ((System.Drawing.Size)(resources.GetObject("radFilter25.Size")));
			this.radFilter25.TabIndex = ((int)(resources.GetObject("radFilter25.TabIndex")));
			this.radFilter25.Text = resources.GetString("radFilter25.Text");
			this.radFilter25.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilter25.TextAlign")));
			this.toolTip1.SetToolTip(this.radFilter25, resources.GetString("radFilter25.ToolTip"));
			this.radFilter25.Visible = ((bool)(resources.GetObject("radFilter25.Visible")));
			this.radFilter25.CheckedChanged += new System.EventHandler(this.radFilter25_CheckedChanged);
			// 
			// radFilterVar1
			// 
			this.radFilterVar1.AccessibleDescription = resources.GetString("radFilterVar1.AccessibleDescription");
			this.radFilterVar1.AccessibleName = resources.GetString("radFilterVar1.AccessibleName");
			this.radFilterVar1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radFilterVar1.Anchor")));
			this.radFilterVar1.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radFilterVar1.Appearance")));
			this.radFilterVar1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radFilterVar1.BackgroundImage")));
			this.radFilterVar1.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilterVar1.CheckAlign")));
			this.radFilterVar1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radFilterVar1.Dock")));
			this.radFilterVar1.Enabled = ((bool)(resources.GetObject("radFilterVar1.Enabled")));
			this.radFilterVar1.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radFilterVar1.FlatStyle")));
			this.radFilterVar1.Font = ((System.Drawing.Font)(resources.GetObject("radFilterVar1.Font")));
			this.radFilterVar1.Image = ((System.Drawing.Image)(resources.GetObject("radFilterVar1.Image")));
			this.radFilterVar1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilterVar1.ImageAlign")));
			this.radFilterVar1.ImageIndex = ((int)(resources.GetObject("radFilterVar1.ImageIndex")));
			this.radFilterVar1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radFilterVar1.ImeMode")));
			this.radFilterVar1.Location = ((System.Drawing.Point)(resources.GetObject("radFilterVar1.Location")));
			this.radFilterVar1.Name = "radFilterVar1";
			this.radFilterVar1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radFilterVar1.RightToLeft")));
			this.radFilterVar1.Size = ((System.Drawing.Size)(resources.GetObject("radFilterVar1.Size")));
			this.radFilterVar1.TabIndex = ((int)(resources.GetObject("radFilterVar1.TabIndex")));
			this.radFilterVar1.Text = resources.GetString("radFilterVar1.Text");
			this.radFilterVar1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilterVar1.TextAlign")));
			this.toolTip1.SetToolTip(this.radFilterVar1, resources.GetString("radFilterVar1.ToolTip"));
			this.radFilterVar1.Visible = ((bool)(resources.GetObject("radFilterVar1.Visible")));
			this.radFilterVar1.CheckedChanged += new System.EventHandler(this.radFilterVar1_CheckedChanged);
			// 
			// radFilterVar2
			// 
			this.radFilterVar2.AccessibleDescription = resources.GetString("radFilterVar2.AccessibleDescription");
			this.radFilterVar2.AccessibleName = resources.GetString("radFilterVar2.AccessibleName");
			this.radFilterVar2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radFilterVar2.Anchor")));
			this.radFilterVar2.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radFilterVar2.Appearance")));
			this.radFilterVar2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radFilterVar2.BackgroundImage")));
			this.radFilterVar2.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilterVar2.CheckAlign")));
			this.radFilterVar2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radFilterVar2.Dock")));
			this.radFilterVar2.Enabled = ((bool)(resources.GetObject("radFilterVar2.Enabled")));
			this.radFilterVar2.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radFilterVar2.FlatStyle")));
			this.radFilterVar2.Font = ((System.Drawing.Font)(resources.GetObject("radFilterVar2.Font")));
			this.radFilterVar2.Image = ((System.Drawing.Image)(resources.GetObject("radFilterVar2.Image")));
			this.radFilterVar2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilterVar2.ImageAlign")));
			this.radFilterVar2.ImageIndex = ((int)(resources.GetObject("radFilterVar2.ImageIndex")));
			this.radFilterVar2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radFilterVar2.ImeMode")));
			this.radFilterVar2.Location = ((System.Drawing.Point)(resources.GetObject("radFilterVar2.Location")));
			this.radFilterVar2.Name = "radFilterVar2";
			this.radFilterVar2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radFilterVar2.RightToLeft")));
			this.radFilterVar2.Size = ((System.Drawing.Size)(resources.GetObject("radFilterVar2.Size")));
			this.radFilterVar2.TabIndex = ((int)(resources.GetObject("radFilterVar2.TabIndex")));
			this.radFilterVar2.Text = resources.GetString("radFilterVar2.Text");
			this.radFilterVar2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radFilterVar2.TextAlign")));
			this.toolTip1.SetToolTip(this.radFilterVar2, resources.GetString("radFilterVar2.ToolTip"));
			this.radFilterVar2.Visible = ((bool)(resources.GetObject("radFilterVar2.Visible")));
			this.radFilterVar2.CheckedChanged += new System.EventHandler(this.radFilterVar2_CheckedChanged);
			// 
			// lblFilterHigh
			// 
			this.lblFilterHigh.AccessibleDescription = resources.GetString("lblFilterHigh.AccessibleDescription");
			this.lblFilterHigh.AccessibleName = resources.GetString("lblFilterHigh.AccessibleName");
			this.lblFilterHigh.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblFilterHigh.Anchor")));
			this.lblFilterHigh.AutoSize = ((bool)(resources.GetObject("lblFilterHigh.AutoSize")));
			this.lblFilterHigh.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblFilterHigh.Dock")));
			this.lblFilterHigh.Enabled = ((bool)(resources.GetObject("lblFilterHigh.Enabled")));
			this.lblFilterHigh.Font = ((System.Drawing.Font)(resources.GetObject("lblFilterHigh.Font")));
			this.lblFilterHigh.Image = ((System.Drawing.Image)(resources.GetObject("lblFilterHigh.Image")));
			this.lblFilterHigh.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFilterHigh.ImageAlign")));
			this.lblFilterHigh.ImageIndex = ((int)(resources.GetObject("lblFilterHigh.ImageIndex")));
			this.lblFilterHigh.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblFilterHigh.ImeMode")));
			this.lblFilterHigh.Location = ((System.Drawing.Point)(resources.GetObject("lblFilterHigh.Location")));
			this.lblFilterHigh.Name = "lblFilterHigh";
			this.lblFilterHigh.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblFilterHigh.RightToLeft")));
			this.lblFilterHigh.Size = ((System.Drawing.Size)(resources.GetObject("lblFilterHigh.Size")));
			this.lblFilterHigh.TabIndex = ((int)(resources.GetObject("lblFilterHigh.TabIndex")));
			this.lblFilterHigh.Text = resources.GetString("lblFilterHigh.Text");
			this.lblFilterHigh.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFilterHigh.TextAlign")));
			this.toolTip1.SetToolTip(this.lblFilterHigh, resources.GetString("lblFilterHigh.ToolTip"));
			this.lblFilterHigh.Visible = ((bool)(resources.GetObject("lblFilterHigh.Visible")));
			// 
			// lblFilterLow
			// 
			this.lblFilterLow.AccessibleDescription = resources.GetString("lblFilterLow.AccessibleDescription");
			this.lblFilterLow.AccessibleName = resources.GetString("lblFilterLow.AccessibleName");
			this.lblFilterLow.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblFilterLow.Anchor")));
			this.lblFilterLow.AutoSize = ((bool)(resources.GetObject("lblFilterLow.AutoSize")));
			this.lblFilterLow.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblFilterLow.Dock")));
			this.lblFilterLow.Enabled = ((bool)(resources.GetObject("lblFilterLow.Enabled")));
			this.lblFilterLow.Font = ((System.Drawing.Font)(resources.GetObject("lblFilterLow.Font")));
			this.lblFilterLow.Image = ((System.Drawing.Image)(resources.GetObject("lblFilterLow.Image")));
			this.lblFilterLow.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFilterLow.ImageAlign")));
			this.lblFilterLow.ImageIndex = ((int)(resources.GetObject("lblFilterLow.ImageIndex")));
			this.lblFilterLow.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblFilterLow.ImeMode")));
			this.lblFilterLow.Location = ((System.Drawing.Point)(resources.GetObject("lblFilterLow.Location")));
			this.lblFilterLow.Name = "lblFilterLow";
			this.lblFilterLow.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblFilterLow.RightToLeft")));
			this.lblFilterLow.Size = ((System.Drawing.Size)(resources.GetObject("lblFilterLow.Size")));
			this.lblFilterLow.TabIndex = ((int)(resources.GetObject("lblFilterLow.TabIndex")));
			this.lblFilterLow.Text = resources.GetString("lblFilterLow.Text");
			this.lblFilterLow.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFilterLow.TextAlign")));
			this.toolTip1.SetToolTip(this.lblFilterLow, resources.GetString("lblFilterLow.ToolTip"));
			this.lblFilterLow.Visible = ((bool)(resources.GetObject("lblFilterLow.Visible")));
			// 
			// udFilterHigh
			// 
			this.udFilterHigh.AccessibleDescription = resources.GetString("udFilterHigh.AccessibleDescription");
			this.udFilterHigh.AccessibleName = resources.GetString("udFilterHigh.AccessibleName");
			this.udFilterHigh.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("udFilterHigh.Anchor")));
			this.udFilterHigh.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("udFilterHigh.Dock")));
			this.udFilterHigh.Enabled = ((bool)(resources.GetObject("udFilterHigh.Enabled")));
			this.udFilterHigh.Font = ((System.Drawing.Font)(resources.GetObject("udFilterHigh.Font")));
			this.udFilterHigh.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("udFilterHigh.ImeMode")));
			this.udFilterHigh.Increment = new System.Decimal(new int[] {
																		   10,
																		   0,
																		   0,
																		   0});
			this.udFilterHigh.Location = ((System.Drawing.Point)(resources.GetObject("udFilterHigh.Location")));
			this.udFilterHigh.Maximum = new System.Decimal(new int[] {
																		 9999,
																		 0,
																		 0,
																		 0});
			this.udFilterHigh.Minimum = new System.Decimal(new int[] {
																		 9999,
																		 0,
																		 0,
																		 -2147483648});
			this.udFilterHigh.Name = "udFilterHigh";
			this.udFilterHigh.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("udFilterHigh.RightToLeft")));
			this.udFilterHigh.Size = ((System.Drawing.Size)(resources.GetObject("udFilterHigh.Size")));
			this.udFilterHigh.TabIndex = ((int)(resources.GetObject("udFilterHigh.TabIndex")));
			this.udFilterHigh.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("udFilterHigh.TextAlign")));
			this.udFilterHigh.ThousandsSeparator = ((bool)(resources.GetObject("udFilterHigh.ThousandsSeparator")));
			this.toolTip1.SetToolTip(this.udFilterHigh, resources.GetString("udFilterHigh.ToolTip"));
			this.udFilterHigh.UpDownAlign = ((System.Windows.Forms.LeftRightAlignment)(resources.GetObject("udFilterHigh.UpDownAlign")));
			this.udFilterHigh.Value = new System.Decimal(new int[] {
																	   6000,
																	   0,
																	   0,
																	   0});
			this.udFilterHigh.Visible = ((bool)(resources.GetObject("udFilterHigh.Visible")));
			this.udFilterHigh.LostFocus += new System.EventHandler(this.udFilterHigh_LostFocus);
			this.udFilterHigh.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
			this.udFilterHigh.ValueChanged += new System.EventHandler(this.udFilterHigh_ValueChanged);
			// 
			// udFilterLow
			// 
			this.udFilterLow.AccessibleDescription = resources.GetString("udFilterLow.AccessibleDescription");
			this.udFilterLow.AccessibleName = resources.GetString("udFilterLow.AccessibleName");
			this.udFilterLow.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("udFilterLow.Anchor")));
			this.udFilterLow.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("udFilterLow.Dock")));
			this.udFilterLow.Enabled = ((bool)(resources.GetObject("udFilterLow.Enabled")));
			this.udFilterLow.Font = ((System.Drawing.Font)(resources.GetObject("udFilterLow.Font")));
			this.udFilterLow.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("udFilterLow.ImeMode")));
			this.udFilterLow.Increment = new System.Decimal(new int[] {
																		  10,
																		  0,
																		  0,
																		  0});
			this.udFilterLow.Location = ((System.Drawing.Point)(resources.GetObject("udFilterLow.Location")));
			this.udFilterLow.Maximum = new System.Decimal(new int[] {
																		9999,
																		0,
																		0,
																		0});
			this.udFilterLow.Minimum = new System.Decimal(new int[] {
																		9999,
																		0,
																		0,
																		-2147483648});
			this.udFilterLow.Name = "udFilterLow";
			this.udFilterLow.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("udFilterLow.RightToLeft")));
			this.udFilterLow.Size = ((System.Drawing.Size)(resources.GetObject("udFilterLow.Size")));
			this.udFilterLow.TabIndex = ((int)(resources.GetObject("udFilterLow.TabIndex")));
			this.udFilterLow.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("udFilterLow.TextAlign")));
			this.udFilterLow.ThousandsSeparator = ((bool)(resources.GetObject("udFilterLow.ThousandsSeparator")));
			this.toolTip1.SetToolTip(this.udFilterLow, resources.GetString("udFilterLow.ToolTip"));
			this.udFilterLow.UpDownAlign = ((System.Windows.Forms.LeftRightAlignment)(resources.GetObject("udFilterLow.UpDownAlign")));
			this.udFilterLow.Value = new System.Decimal(new int[] {
																	  0,
																	  0,
																	  0,
																	  0});
			this.udFilterLow.Visible = ((bool)(resources.GetObject("udFilterLow.Visible")));
			this.udFilterLow.LostFocus += new System.EventHandler(this.udFilterLow_LostFocus);
			this.udFilterLow.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
			this.udFilterLow.ValueChanged += new System.EventHandler(this.udFilterLow_ValueChanged);
			// 
			// chkPower
			// 
			this.chkPower.AccessibleDescription = resources.GetString("chkPower.AccessibleDescription");
			this.chkPower.AccessibleName = resources.GetString("chkPower.AccessibleName");
			this.chkPower.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkPower.Anchor")));
			this.chkPower.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkPower.Appearance")));
			this.chkPower.BackColor = System.Drawing.SystemColors.Control;
			this.chkPower.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkPower.BackgroundImage")));
			this.chkPower.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkPower.CheckAlign")));
			this.chkPower.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkPower.Dock")));
			this.chkPower.Enabled = ((bool)(resources.GetObject("chkPower.Enabled")));
			this.chkPower.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkPower.FlatStyle")));
			this.chkPower.Font = ((System.Drawing.Font)(resources.GetObject("chkPower.Font")));
			this.chkPower.Image = ((System.Drawing.Image)(resources.GetObject("chkPower.Image")));
			this.chkPower.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkPower.ImageAlign")));
			this.chkPower.ImageIndex = ((int)(resources.GetObject("chkPower.ImageIndex")));
			this.chkPower.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkPower.ImeMode")));
			this.chkPower.Location = ((System.Drawing.Point)(resources.GetObject("chkPower.Location")));
			this.chkPower.Name = "chkPower";
			this.chkPower.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkPower.RightToLeft")));
			this.chkPower.Size = ((System.Drawing.Size)(resources.GetObject("chkPower.Size")));
			this.chkPower.TabIndex = ((int)(resources.GetObject("chkPower.TabIndex")));
			this.chkPower.Text = resources.GetString("chkPower.Text");
			this.chkPower.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkPower.TextAlign")));
			this.toolTip1.SetToolTip(this.chkPower, resources.GetString("chkPower.ToolTip"));
			this.chkPower.Visible = ((bool)(resources.GetObject("chkPower.Visible")));
			this.chkPower.CheckedChanged += new System.EventHandler(this.chkPower_CheckedChanged);
			// 
			// comboDisplayMode
			// 
			this.comboDisplayMode.AccessibleDescription = resources.GetString("comboDisplayMode.AccessibleDescription");
			this.comboDisplayMode.AccessibleName = resources.GetString("comboDisplayMode.AccessibleName");
			this.comboDisplayMode.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("comboDisplayMode.Anchor")));
			this.comboDisplayMode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("comboDisplayMode.BackgroundImage")));
			this.comboDisplayMode.DisplayMember = "0";
			this.comboDisplayMode.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("comboDisplayMode.Dock")));
			this.comboDisplayMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDisplayMode.DropDownWidth = 88;
			this.comboDisplayMode.Enabled = ((bool)(resources.GetObject("comboDisplayMode.Enabled")));
			this.comboDisplayMode.Font = ((System.Drawing.Font)(resources.GetObject("comboDisplayMode.Font")));
			this.comboDisplayMode.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("comboDisplayMode.ImeMode")));
			this.comboDisplayMode.IntegralHeight = ((bool)(resources.GetObject("comboDisplayMode.IntegralHeight")));
			this.comboDisplayMode.ItemHeight = ((int)(resources.GetObject("comboDisplayMode.ItemHeight")));
			this.comboDisplayMode.Location = ((System.Drawing.Point)(resources.GetObject("comboDisplayMode.Location")));
			this.comboDisplayMode.MaxDropDownItems = ((int)(resources.GetObject("comboDisplayMode.MaxDropDownItems")));
			this.comboDisplayMode.MaxLength = ((int)(resources.GetObject("comboDisplayMode.MaxLength")));
			this.comboDisplayMode.Name = "comboDisplayMode";
			this.comboDisplayMode.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("comboDisplayMode.RightToLeft")));
			this.comboDisplayMode.Size = ((System.Drawing.Size)(resources.GetObject("comboDisplayMode.Size")));
			this.comboDisplayMode.TabIndex = ((int)(resources.GetObject("comboDisplayMode.TabIndex")));
			this.comboDisplayMode.Text = resources.GetString("comboDisplayMode.Text");
			this.toolTip1.SetToolTip(this.comboDisplayMode, resources.GetString("comboDisplayMode.ToolTip"));
			this.comboDisplayMode.Visible = ((bool)(resources.GetObject("comboDisplayMode.Visible")));
			this.comboDisplayMode.SelectedIndexChanged += new System.EventHandler(this.comboDisplayMode_SelectedIndexChanged);
			// 
			// lblCPUMeter
			// 
			this.lblCPUMeter.AccessibleDescription = resources.GetString("lblCPUMeter.AccessibleDescription");
			this.lblCPUMeter.AccessibleName = resources.GetString("lblCPUMeter.AccessibleName");
			this.lblCPUMeter.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblCPUMeter.Anchor")));
			this.lblCPUMeter.AutoSize = ((bool)(resources.GetObject("lblCPUMeter.AutoSize")));
			this.lblCPUMeter.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblCPUMeter.Dock")));
			this.lblCPUMeter.Enabled = ((bool)(resources.GetObject("lblCPUMeter.Enabled")));
			this.lblCPUMeter.Font = ((System.Drawing.Font)(resources.GetObject("lblCPUMeter.Font")));
			this.lblCPUMeter.Image = ((System.Drawing.Image)(resources.GetObject("lblCPUMeter.Image")));
			this.lblCPUMeter.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblCPUMeter.ImageAlign")));
			this.lblCPUMeter.ImageIndex = ((int)(resources.GetObject("lblCPUMeter.ImageIndex")));
			this.lblCPUMeter.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblCPUMeter.ImeMode")));
			this.lblCPUMeter.Location = ((System.Drawing.Point)(resources.GetObject("lblCPUMeter.Location")));
			this.lblCPUMeter.Name = "lblCPUMeter";
			this.lblCPUMeter.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblCPUMeter.RightToLeft")));
			this.lblCPUMeter.Size = ((System.Drawing.Size)(resources.GetObject("lblCPUMeter.Size")));
			this.lblCPUMeter.TabIndex = ((int)(resources.GetObject("lblCPUMeter.TabIndex")));
			this.lblCPUMeter.Text = resources.GetString("lblCPUMeter.Text");
			this.lblCPUMeter.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblCPUMeter.TextAlign")));
			this.toolTip1.SetToolTip(this.lblCPUMeter, resources.GetString("lblCPUMeter.ToolTip"));
			this.lblCPUMeter.Visible = ((bool)(resources.GetObject("lblCPUMeter.Visible")));
			// 
			// timer_cpu_meter
			// 
			this.timer_cpu_meter.Enabled = true;
			this.timer_cpu_meter.Interval = 1000;
			this.timer_cpu_meter.Tick += new System.EventHandler(this.timer_cpu_meter_Tick);
			// 
			// grpDSP
			// 
			this.grpDSP.AccessibleDescription = resources.GetString("grpDSP.AccessibleDescription");
			this.grpDSP.AccessibleName = resources.GetString("grpDSP.AccessibleName");
			this.grpDSP.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpDSP.Anchor")));
			this.grpDSP.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpDSP.BackgroundImage")));
			this.grpDSP.Controls.Add(this.chkDSPCompander);
			this.grpDSP.Controls.Add(this.chkDSPNB2);
			this.grpDSP.Controls.Add(this.chkDSPComp);
			this.grpDSP.Controls.Add(this.chkNB);
			this.grpDSP.Controls.Add(this.chkANF);
			this.grpDSP.Controls.Add(this.chkNR);
			this.grpDSP.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpDSP.Dock")));
			this.grpDSP.Enabled = ((bool)(resources.GetObject("grpDSP.Enabled")));
			this.grpDSP.Font = ((System.Drawing.Font)(resources.GetObject("grpDSP.Font")));
			this.grpDSP.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpDSP.ImeMode")));
			this.grpDSP.Location = ((System.Drawing.Point)(resources.GetObject("grpDSP.Location")));
			this.grpDSP.Name = "grpDSP";
			this.grpDSP.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpDSP.RightToLeft")));
			this.grpDSP.Size = ((System.Drawing.Size)(resources.GetObject("grpDSP.Size")));
			this.grpDSP.TabIndex = ((int)(resources.GetObject("grpDSP.TabIndex")));
			this.grpDSP.TabStop = false;
			this.grpDSP.Text = resources.GetString("grpDSP.Text");
			this.toolTip1.SetToolTip(this.grpDSP, resources.GetString("grpDSP.ToolTip"));
			this.grpDSP.Visible = ((bool)(resources.GetObject("grpDSP.Visible")));
			// 
			// chkDSPCompander
			// 
			this.chkDSPCompander.AccessibleDescription = resources.GetString("chkDSPCompander.AccessibleDescription");
			this.chkDSPCompander.AccessibleName = resources.GetString("chkDSPCompander.AccessibleName");
			this.chkDSPCompander.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkDSPCompander.Anchor")));
			this.chkDSPCompander.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkDSPCompander.Appearance")));
			this.chkDSPCompander.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkDSPCompander.BackgroundImage")));
			this.chkDSPCompander.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDSPCompander.CheckAlign")));
			this.chkDSPCompander.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkDSPCompander.Dock")));
			this.chkDSPCompander.Enabled = ((bool)(resources.GetObject("chkDSPCompander.Enabled")));
			this.chkDSPCompander.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkDSPCompander.FlatStyle")));
			this.chkDSPCompander.Font = ((System.Drawing.Font)(resources.GetObject("chkDSPCompander.Font")));
			this.chkDSPCompander.Image = ((System.Drawing.Image)(resources.GetObject("chkDSPCompander.Image")));
			this.chkDSPCompander.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDSPCompander.ImageAlign")));
			this.chkDSPCompander.ImageIndex = ((int)(resources.GetObject("chkDSPCompander.ImageIndex")));
			this.chkDSPCompander.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkDSPCompander.ImeMode")));
			this.chkDSPCompander.Location = ((System.Drawing.Point)(resources.GetObject("chkDSPCompander.Location")));
			this.chkDSPCompander.Name = "chkDSPCompander";
			this.chkDSPCompander.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkDSPCompander.RightToLeft")));
			this.chkDSPCompander.Size = ((System.Drawing.Size)(resources.GetObject("chkDSPCompander.Size")));
			this.chkDSPCompander.TabIndex = ((int)(resources.GetObject("chkDSPCompander.TabIndex")));
			this.chkDSPCompander.Text = resources.GetString("chkDSPCompander.Text");
			this.chkDSPCompander.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDSPCompander.TextAlign")));
			this.toolTip1.SetToolTip(this.chkDSPCompander, resources.GetString("chkDSPCompander.ToolTip"));
			this.chkDSPCompander.Visible = ((bool)(resources.GetObject("chkDSPCompander.Visible")));
			this.chkDSPCompander.CheckedChanged += new System.EventHandler(this.chkDSPCompander_CheckedChanged);
			// 
			// chkDSPNB2
			// 
			this.chkDSPNB2.AccessibleDescription = resources.GetString("chkDSPNB2.AccessibleDescription");
			this.chkDSPNB2.AccessibleName = resources.GetString("chkDSPNB2.AccessibleName");
			this.chkDSPNB2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkDSPNB2.Anchor")));
			this.chkDSPNB2.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkDSPNB2.Appearance")));
			this.chkDSPNB2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkDSPNB2.BackgroundImage")));
			this.chkDSPNB2.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDSPNB2.CheckAlign")));
			this.chkDSPNB2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkDSPNB2.Dock")));
			this.chkDSPNB2.Enabled = ((bool)(resources.GetObject("chkDSPNB2.Enabled")));
			this.chkDSPNB2.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkDSPNB2.FlatStyle")));
			this.chkDSPNB2.Font = ((System.Drawing.Font)(resources.GetObject("chkDSPNB2.Font")));
			this.chkDSPNB2.Image = ((System.Drawing.Image)(resources.GetObject("chkDSPNB2.Image")));
			this.chkDSPNB2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDSPNB2.ImageAlign")));
			this.chkDSPNB2.ImageIndex = ((int)(resources.GetObject("chkDSPNB2.ImageIndex")));
			this.chkDSPNB2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkDSPNB2.ImeMode")));
			this.chkDSPNB2.Location = ((System.Drawing.Point)(resources.GetObject("chkDSPNB2.Location")));
			this.chkDSPNB2.Name = "chkDSPNB2";
			this.chkDSPNB2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkDSPNB2.RightToLeft")));
			this.chkDSPNB2.Size = ((System.Drawing.Size)(resources.GetObject("chkDSPNB2.Size")));
			this.chkDSPNB2.TabIndex = ((int)(resources.GetObject("chkDSPNB2.TabIndex")));
			this.chkDSPNB2.Text = resources.GetString("chkDSPNB2.Text");
			this.chkDSPNB2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDSPNB2.TextAlign")));
			this.toolTip1.SetToolTip(this.chkDSPNB2, resources.GetString("chkDSPNB2.ToolTip"));
			this.chkDSPNB2.Visible = ((bool)(resources.GetObject("chkDSPNB2.Visible")));
			this.chkDSPNB2.CheckedChanged += new System.EventHandler(this.chkDSPNB2_CheckedChanged);
			// 
			// chkDSPComp
			// 
			this.chkDSPComp.AccessibleDescription = resources.GetString("chkDSPComp.AccessibleDescription");
			this.chkDSPComp.AccessibleName = resources.GetString("chkDSPComp.AccessibleName");
			this.chkDSPComp.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkDSPComp.Anchor")));
			this.chkDSPComp.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkDSPComp.Appearance")));
			this.chkDSPComp.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkDSPComp.BackgroundImage")));
			this.chkDSPComp.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDSPComp.CheckAlign")));
			this.chkDSPComp.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkDSPComp.Dock")));
			this.chkDSPComp.Enabled = ((bool)(resources.GetObject("chkDSPComp.Enabled")));
			this.chkDSPComp.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkDSPComp.FlatStyle")));
			this.chkDSPComp.Font = ((System.Drawing.Font)(resources.GetObject("chkDSPComp.Font")));
			this.chkDSPComp.Image = ((System.Drawing.Image)(resources.GetObject("chkDSPComp.Image")));
			this.chkDSPComp.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDSPComp.ImageAlign")));
			this.chkDSPComp.ImageIndex = ((int)(resources.GetObject("chkDSPComp.ImageIndex")));
			this.chkDSPComp.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkDSPComp.ImeMode")));
			this.chkDSPComp.Location = ((System.Drawing.Point)(resources.GetObject("chkDSPComp.Location")));
			this.chkDSPComp.Name = "chkDSPComp";
			this.chkDSPComp.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkDSPComp.RightToLeft")));
			this.chkDSPComp.Size = ((System.Drawing.Size)(resources.GetObject("chkDSPComp.Size")));
			this.chkDSPComp.TabIndex = ((int)(resources.GetObject("chkDSPComp.TabIndex")));
			this.chkDSPComp.Text = resources.GetString("chkDSPComp.Text");
			this.chkDSPComp.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDSPComp.TextAlign")));
			this.toolTip1.SetToolTip(this.chkDSPComp, resources.GetString("chkDSPComp.ToolTip"));
			this.chkDSPComp.Visible = ((bool)(resources.GetObject("chkDSPComp.Visible")));
			this.chkDSPComp.CheckedChanged += new System.EventHandler(this.chkDSPComp_CheckedChanged);
			// 
			// chkNB
			// 
			this.chkNB.AccessibleDescription = resources.GetString("chkNB.AccessibleDescription");
			this.chkNB.AccessibleName = resources.GetString("chkNB.AccessibleName");
			this.chkNB.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkNB.Anchor")));
			this.chkNB.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkNB.Appearance")));
			this.chkNB.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkNB.BackgroundImage")));
			this.chkNB.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkNB.CheckAlign")));
			this.chkNB.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkNB.Dock")));
			this.chkNB.Enabled = ((bool)(resources.GetObject("chkNB.Enabled")));
			this.chkNB.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkNB.FlatStyle")));
			this.chkNB.Font = ((System.Drawing.Font)(resources.GetObject("chkNB.Font")));
			this.chkNB.Image = ((System.Drawing.Image)(resources.GetObject("chkNB.Image")));
			this.chkNB.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkNB.ImageAlign")));
			this.chkNB.ImageIndex = ((int)(resources.GetObject("chkNB.ImageIndex")));
			this.chkNB.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkNB.ImeMode")));
			this.chkNB.Location = ((System.Drawing.Point)(resources.GetObject("chkNB.Location")));
			this.chkNB.Name = "chkNB";
			this.chkNB.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkNB.RightToLeft")));
			this.chkNB.Size = ((System.Drawing.Size)(resources.GetObject("chkNB.Size")));
			this.chkNB.TabIndex = ((int)(resources.GetObject("chkNB.TabIndex")));
			this.chkNB.Text = resources.GetString("chkNB.Text");
			this.chkNB.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkNB.TextAlign")));
			this.toolTip1.SetToolTip(this.chkNB, resources.GetString("chkNB.ToolTip"));
			this.chkNB.Visible = ((bool)(resources.GetObject("chkNB.Visible")));
			this.chkNB.CheckedChanged += new System.EventHandler(this.chkNB_CheckedChanged);
			// 
			// chkANF
			// 
			this.chkANF.AccessibleDescription = resources.GetString("chkANF.AccessibleDescription");
			this.chkANF.AccessibleName = resources.GetString("chkANF.AccessibleName");
			this.chkANF.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkANF.Anchor")));
			this.chkANF.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkANF.Appearance")));
			this.chkANF.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkANF.BackgroundImage")));
			this.chkANF.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkANF.CheckAlign")));
			this.chkANF.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkANF.Dock")));
			this.chkANF.Enabled = ((bool)(resources.GetObject("chkANF.Enabled")));
			this.chkANF.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkANF.FlatStyle")));
			this.chkANF.Font = ((System.Drawing.Font)(resources.GetObject("chkANF.Font")));
			this.chkANF.Image = ((System.Drawing.Image)(resources.GetObject("chkANF.Image")));
			this.chkANF.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkANF.ImageAlign")));
			this.chkANF.ImageIndex = ((int)(resources.GetObject("chkANF.ImageIndex")));
			this.chkANF.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkANF.ImeMode")));
			this.chkANF.Location = ((System.Drawing.Point)(resources.GetObject("chkANF.Location")));
			this.chkANF.Name = "chkANF";
			this.chkANF.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkANF.RightToLeft")));
			this.chkANF.Size = ((System.Drawing.Size)(resources.GetObject("chkANF.Size")));
			this.chkANF.TabIndex = ((int)(resources.GetObject("chkANF.TabIndex")));
			this.chkANF.Text = resources.GetString("chkANF.Text");
			this.chkANF.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkANF.TextAlign")));
			this.toolTip1.SetToolTip(this.chkANF, resources.GetString("chkANF.ToolTip"));
			this.chkANF.Visible = ((bool)(resources.GetObject("chkANF.Visible")));
			this.chkANF.CheckedChanged += new System.EventHandler(this.chkANF_CheckedChanged);
			// 
			// chkNR
			// 
			this.chkNR.AccessibleDescription = resources.GetString("chkNR.AccessibleDescription");
			this.chkNR.AccessibleName = resources.GetString("chkNR.AccessibleName");
			this.chkNR.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkNR.Anchor")));
			this.chkNR.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkNR.Appearance")));
			this.chkNR.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkNR.BackgroundImage")));
			this.chkNR.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkNR.CheckAlign")));
			this.chkNR.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkNR.Dock")));
			this.chkNR.Enabled = ((bool)(resources.GetObject("chkNR.Enabled")));
			this.chkNR.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkNR.FlatStyle")));
			this.chkNR.Font = ((System.Drawing.Font)(resources.GetObject("chkNR.Font")));
			this.chkNR.Image = ((System.Drawing.Image)(resources.GetObject("chkNR.Image")));
			this.chkNR.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkNR.ImageAlign")));
			this.chkNR.ImageIndex = ((int)(resources.GetObject("chkNR.ImageIndex")));
			this.chkNR.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkNR.ImeMode")));
			this.chkNR.Location = ((System.Drawing.Point)(resources.GetObject("chkNR.Location")));
			this.chkNR.Name = "chkNR";
			this.chkNR.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkNR.RightToLeft")));
			this.chkNR.Size = ((System.Drawing.Size)(resources.GetObject("chkNR.Size")));
			this.chkNR.TabIndex = ((int)(resources.GetObject("chkNR.TabIndex")));
			this.chkNR.Text = resources.GetString("chkNR.Text");
			this.chkNR.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkNR.TextAlign")));
			this.toolTip1.SetToolTip(this.chkNR, resources.GetString("chkNR.ToolTip"));
			this.chkNR.Visible = ((bool)(resources.GetObject("chkNR.Visible")));
			this.chkNR.CheckedChanged += new System.EventHandler(this.chkNR_CheckedChanged);
			// 
			// lblAGC
			// 
			this.lblAGC.AccessibleDescription = resources.GetString("lblAGC.AccessibleDescription");
			this.lblAGC.AccessibleName = resources.GetString("lblAGC.AccessibleName");
			this.lblAGC.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblAGC.Anchor")));
			this.lblAGC.AutoSize = ((bool)(resources.GetObject("lblAGC.AutoSize")));
			this.lblAGC.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblAGC.Dock")));
			this.lblAGC.Enabled = ((bool)(resources.GetObject("lblAGC.Enabled")));
			this.lblAGC.Font = ((System.Drawing.Font)(resources.GetObject("lblAGC.Font")));
			this.lblAGC.Image = ((System.Drawing.Image)(resources.GetObject("lblAGC.Image")));
			this.lblAGC.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblAGC.ImageAlign")));
			this.lblAGC.ImageIndex = ((int)(resources.GetObject("lblAGC.ImageIndex")));
			this.lblAGC.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblAGC.ImeMode")));
			this.lblAGC.Location = ((System.Drawing.Point)(resources.GetObject("lblAGC.Location")));
			this.lblAGC.Name = "lblAGC";
			this.lblAGC.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblAGC.RightToLeft")));
			this.lblAGC.Size = ((System.Drawing.Size)(resources.GetObject("lblAGC.Size")));
			this.lblAGC.TabIndex = ((int)(resources.GetObject("lblAGC.TabIndex")));
			this.lblAGC.Text = resources.GetString("lblAGC.Text");
			this.lblAGC.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblAGC.TextAlign")));
			this.toolTip1.SetToolTip(this.lblAGC, resources.GetString("lblAGC.ToolTip"));
			this.lblAGC.Visible = ((bool)(resources.GetObject("lblAGC.Visible")));
			// 
			// comboAGC
			// 
			this.comboAGC.AccessibleDescription = resources.GetString("comboAGC.AccessibleDescription");
			this.comboAGC.AccessibleName = resources.GetString("comboAGC.AccessibleName");
			this.comboAGC.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("comboAGC.Anchor")));
			this.comboAGC.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("comboAGC.BackgroundImage")));
			this.comboAGC.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("comboAGC.Dock")));
			this.comboAGC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAGC.DropDownWidth = 48;
			this.comboAGC.Enabled = ((bool)(resources.GetObject("comboAGC.Enabled")));
			this.comboAGC.Font = ((System.Drawing.Font)(resources.GetObject("comboAGC.Font")));
			this.comboAGC.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("comboAGC.ImeMode")));
			this.comboAGC.IntegralHeight = ((bool)(resources.GetObject("comboAGC.IntegralHeight")));
			this.comboAGC.ItemHeight = ((int)(resources.GetObject("comboAGC.ItemHeight")));
			this.comboAGC.Location = ((System.Drawing.Point)(resources.GetObject("comboAGC.Location")));
			this.comboAGC.MaxDropDownItems = ((int)(resources.GetObject("comboAGC.MaxDropDownItems")));
			this.comboAGC.MaxLength = ((int)(resources.GetObject("comboAGC.MaxLength")));
			this.comboAGC.Name = "comboAGC";
			this.comboAGC.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("comboAGC.RightToLeft")));
			this.comboAGC.Size = ((System.Drawing.Size)(resources.GetObject("comboAGC.Size")));
			this.comboAGC.TabIndex = ((int)(resources.GetObject("comboAGC.TabIndex")));
			this.comboAGC.Text = resources.GetString("comboAGC.Text");
			this.toolTip1.SetToolTip(this.comboAGC, resources.GetString("comboAGC.ToolTip"));
			this.comboAGC.Visible = ((bool)(resources.GetObject("comboAGC.Visible")));
			this.comboAGC.SelectedIndexChanged += new System.EventHandler(this.comboAGC_SelectedIndexChanged);
			// 
			// chkBIN
			// 
			this.chkBIN.AccessibleDescription = resources.GetString("chkBIN.AccessibleDescription");
			this.chkBIN.AccessibleName = resources.GetString("chkBIN.AccessibleName");
			this.chkBIN.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkBIN.Anchor")));
			this.chkBIN.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkBIN.Appearance")));
			this.chkBIN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkBIN.BackgroundImage")));
			this.chkBIN.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkBIN.CheckAlign")));
			this.chkBIN.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkBIN.Dock")));
			this.chkBIN.Enabled = ((bool)(resources.GetObject("chkBIN.Enabled")));
			this.chkBIN.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkBIN.FlatStyle")));
			this.chkBIN.Font = ((System.Drawing.Font)(resources.GetObject("chkBIN.Font")));
			this.chkBIN.Image = ((System.Drawing.Image)(resources.GetObject("chkBIN.Image")));
			this.chkBIN.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkBIN.ImageAlign")));
			this.chkBIN.ImageIndex = ((int)(resources.GetObject("chkBIN.ImageIndex")));
			this.chkBIN.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkBIN.ImeMode")));
			this.chkBIN.Location = ((System.Drawing.Point)(resources.GetObject("chkBIN.Location")));
			this.chkBIN.Name = "chkBIN";
			this.chkBIN.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkBIN.RightToLeft")));
			this.chkBIN.Size = ((System.Drawing.Size)(resources.GetObject("chkBIN.Size")));
			this.chkBIN.TabIndex = ((int)(resources.GetObject("chkBIN.TabIndex")));
			this.chkBIN.Text = resources.GetString("chkBIN.Text");
			this.chkBIN.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkBIN.TextAlign")));
			this.toolTip1.SetToolTip(this.chkBIN, resources.GetString("chkBIN.ToolTip"));
			this.chkBIN.Visible = ((bool)(resources.GetObject("chkBIN.Visible")));
			this.chkBIN.CheckedChanged += new System.EventHandler(this.chkBIN_CheckedChanged);
			// 
			// comboPreamp
			// 
			this.comboPreamp.AccessibleDescription = resources.GetString("comboPreamp.AccessibleDescription");
			this.comboPreamp.AccessibleName = resources.GetString("comboPreamp.AccessibleName");
			this.comboPreamp.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("comboPreamp.Anchor")));
			this.comboPreamp.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("comboPreamp.BackgroundImage")));
			this.comboPreamp.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("comboPreamp.Dock")));
			this.comboPreamp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPreamp.DropDownWidth = 48;
			this.comboPreamp.Enabled = ((bool)(resources.GetObject("comboPreamp.Enabled")));
			this.comboPreamp.Font = ((System.Drawing.Font)(resources.GetObject("comboPreamp.Font")));
			this.comboPreamp.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("comboPreamp.ImeMode")));
			this.comboPreamp.IntegralHeight = ((bool)(resources.GetObject("comboPreamp.IntegralHeight")));
			this.comboPreamp.ItemHeight = ((int)(resources.GetObject("comboPreamp.ItemHeight")));
			this.comboPreamp.Items.AddRange(new object[] {
															 resources.GetString("comboPreamp.Items"),
															 resources.GetString("comboPreamp.Items1")});
			this.comboPreamp.Location = ((System.Drawing.Point)(resources.GetObject("comboPreamp.Location")));
			this.comboPreamp.MaxDropDownItems = ((int)(resources.GetObject("comboPreamp.MaxDropDownItems")));
			this.comboPreamp.MaxLength = ((int)(resources.GetObject("comboPreamp.MaxLength")));
			this.comboPreamp.Name = "comboPreamp";
			this.comboPreamp.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("comboPreamp.RightToLeft")));
			this.comboPreamp.Size = ((System.Drawing.Size)(resources.GetObject("comboPreamp.Size")));
			this.comboPreamp.TabIndex = ((int)(resources.GetObject("comboPreamp.TabIndex")));
			this.comboPreamp.Text = resources.GetString("comboPreamp.Text");
			this.toolTip1.SetToolTip(this.comboPreamp, resources.GetString("comboPreamp.ToolTip"));
			this.comboPreamp.Visible = ((bool)(resources.GetObject("comboPreamp.Visible")));
			this.comboPreamp.SelectedIndexChanged += new System.EventHandler(this.comboPreamp_SelectedIndexChanged);
			// 
			// lblPreamp
			// 
			this.lblPreamp.AccessibleDescription = resources.GetString("lblPreamp.AccessibleDescription");
			this.lblPreamp.AccessibleName = resources.GetString("lblPreamp.AccessibleName");
			this.lblPreamp.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblPreamp.Anchor")));
			this.lblPreamp.AutoSize = ((bool)(resources.GetObject("lblPreamp.AutoSize")));
			this.lblPreamp.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblPreamp.Dock")));
			this.lblPreamp.Enabled = ((bool)(resources.GetObject("lblPreamp.Enabled")));
			this.lblPreamp.Font = ((System.Drawing.Font)(resources.GetObject("lblPreamp.Font")));
			this.lblPreamp.Image = ((System.Drawing.Image)(resources.GetObject("lblPreamp.Image")));
			this.lblPreamp.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblPreamp.ImageAlign")));
			this.lblPreamp.ImageIndex = ((int)(resources.GetObject("lblPreamp.ImageIndex")));
			this.lblPreamp.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblPreamp.ImeMode")));
			this.lblPreamp.Location = ((System.Drawing.Point)(resources.GetObject("lblPreamp.Location")));
			this.lblPreamp.Name = "lblPreamp";
			this.lblPreamp.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblPreamp.RightToLeft")));
			this.lblPreamp.Size = ((System.Drawing.Size)(resources.GetObject("lblPreamp.Size")));
			this.lblPreamp.TabIndex = ((int)(resources.GetObject("lblPreamp.TabIndex")));
			this.lblPreamp.Text = resources.GetString("lblPreamp.Text");
			this.lblPreamp.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblPreamp.TextAlign")));
			this.toolTip1.SetToolTip(this.lblPreamp, resources.GetString("lblPreamp.ToolTip"));
			this.lblPreamp.Visible = ((bool)(resources.GetObject("lblPreamp.Visible")));
			// 
			// udXIT
			// 
			this.udXIT.AccessibleDescription = resources.GetString("udXIT.AccessibleDescription");
			this.udXIT.AccessibleName = resources.GetString("udXIT.AccessibleName");
			this.udXIT.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("udXIT.Anchor")));
			this.udXIT.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("udXIT.Dock")));
			this.udXIT.Enabled = ((bool)(resources.GetObject("udXIT.Enabled")));
			this.udXIT.Font = ((System.Drawing.Font)(resources.GetObject("udXIT.Font")));
			this.udXIT.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("udXIT.ImeMode")));
			this.udXIT.Increment = new System.Decimal(new int[] {
																	1,
																	0,
																	0,
																	0});
			this.udXIT.Location = ((System.Drawing.Point)(resources.GetObject("udXIT.Location")));
			this.udXIT.Maximum = new System.Decimal(new int[] {
																  9999,
																  0,
																  0,
																  0});
			this.udXIT.Minimum = new System.Decimal(new int[] {
																  9999,
																  0,
																  0,
																  -2147483648});
			this.udXIT.Name = "udXIT";
			this.udXIT.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("udXIT.RightToLeft")));
			this.udXIT.Size = ((System.Drawing.Size)(resources.GetObject("udXIT.Size")));
			this.udXIT.TabIndex = ((int)(resources.GetObject("udXIT.TabIndex")));
			this.udXIT.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("udXIT.TextAlign")));
			this.udXIT.ThousandsSeparator = ((bool)(resources.GetObject("udXIT.ThousandsSeparator")));
			this.toolTip1.SetToolTip(this.udXIT, resources.GetString("udXIT.ToolTip"));
			this.udXIT.UpDownAlign = ((System.Windows.Forms.LeftRightAlignment)(resources.GetObject("udXIT.UpDownAlign")));
			this.udXIT.Value = new System.Decimal(new int[] {
																0,
																0,
																0,
																0});
			this.udXIT.Visible = ((bool)(resources.GetObject("udXIT.Visible")));
			this.udXIT.LostFocus += new System.EventHandler(this.udXIT_LostFocus);
			this.udXIT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
			this.udXIT.ValueChanged += new System.EventHandler(this.udXIT_ValueChanged);
			// 
			// chkMON
			// 
			this.chkMON.AccessibleDescription = resources.GetString("chkMON.AccessibleDescription");
			this.chkMON.AccessibleName = resources.GetString("chkMON.AccessibleName");
			this.chkMON.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkMON.Anchor")));
			this.chkMON.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkMON.Appearance")));
			this.chkMON.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkMON.BackgroundImage")));
			this.chkMON.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkMON.CheckAlign")));
			this.chkMON.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkMON.Dock")));
			this.chkMON.Enabled = ((bool)(resources.GetObject("chkMON.Enabled")));
			this.chkMON.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkMON.FlatStyle")));
			this.chkMON.Font = ((System.Drawing.Font)(resources.GetObject("chkMON.Font")));
			this.chkMON.Image = ((System.Drawing.Image)(resources.GetObject("chkMON.Image")));
			this.chkMON.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkMON.ImageAlign")));
			this.chkMON.ImageIndex = ((int)(resources.GetObject("chkMON.ImageIndex")));
			this.chkMON.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkMON.ImeMode")));
			this.chkMON.Location = ((System.Drawing.Point)(resources.GetObject("chkMON.Location")));
			this.chkMON.Name = "chkMON";
			this.chkMON.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkMON.RightToLeft")));
			this.chkMON.Size = ((System.Drawing.Size)(resources.GetObject("chkMON.Size")));
			this.chkMON.TabIndex = ((int)(resources.GetObject("chkMON.TabIndex")));
			this.chkMON.Text = resources.GetString("chkMON.Text");
			this.chkMON.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkMON.TextAlign")));
			this.toolTip1.SetToolTip(this.chkMON, resources.GetString("chkMON.ToolTip"));
			this.chkMON.Visible = ((bool)(resources.GetObject("chkMON.Visible")));
			this.chkMON.CheckedChanged += new System.EventHandler(this.chkMON_CheckedChanged);
			// 
			// chkTUN
			// 
			this.chkTUN.AccessibleDescription = resources.GetString("chkTUN.AccessibleDescription");
			this.chkTUN.AccessibleName = resources.GetString("chkTUN.AccessibleName");
			this.chkTUN.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkTUN.Anchor")));
			this.chkTUN.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkTUN.Appearance")));
			this.chkTUN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkTUN.BackgroundImage")));
			this.chkTUN.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkTUN.CheckAlign")));
			this.chkTUN.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkTUN.Dock")));
			this.chkTUN.Enabled = ((bool)(resources.GetObject("chkTUN.Enabled")));
			this.chkTUN.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkTUN.FlatStyle")));
			this.chkTUN.Font = ((System.Drawing.Font)(resources.GetObject("chkTUN.Font")));
			this.chkTUN.Image = ((System.Drawing.Image)(resources.GetObject("chkTUN.Image")));
			this.chkTUN.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkTUN.ImageAlign")));
			this.chkTUN.ImageIndex = ((int)(resources.GetObject("chkTUN.ImageIndex")));
			this.chkTUN.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkTUN.ImeMode")));
			this.chkTUN.Location = ((System.Drawing.Point)(resources.GetObject("chkTUN.Location")));
			this.chkTUN.Name = "chkTUN";
			this.chkTUN.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkTUN.RightToLeft")));
			this.chkTUN.Size = ((System.Drawing.Size)(resources.GetObject("chkTUN.Size")));
			this.chkTUN.TabIndex = ((int)(resources.GetObject("chkTUN.TabIndex")));
			this.chkTUN.Text = resources.GetString("chkTUN.Text");
			this.chkTUN.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkTUN.TextAlign")));
			this.toolTip1.SetToolTip(this.chkTUN, resources.GetString("chkTUN.ToolTip"));
			this.chkTUN.Visible = ((bool)(resources.GetObject("chkTUN.Visible")));
			this.chkTUN.CheckedChanged += new System.EventHandler(this.chkTUN_CheckedChanged);
			// 
			// chkMOX
			// 
			this.chkMOX.AccessibleDescription = resources.GetString("chkMOX.AccessibleDescription");
			this.chkMOX.AccessibleName = resources.GetString("chkMOX.AccessibleName");
			this.chkMOX.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkMOX.Anchor")));
			this.chkMOX.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkMOX.Appearance")));
			this.chkMOX.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkMOX.BackgroundImage")));
			this.chkMOX.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkMOX.CheckAlign")));
			this.chkMOX.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkMOX.Dock")));
			this.chkMOX.Enabled = ((bool)(resources.GetObject("chkMOX.Enabled")));
			this.chkMOX.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkMOX.FlatStyle")));
			this.chkMOX.Font = ((System.Drawing.Font)(resources.GetObject("chkMOX.Font")));
			this.chkMOX.Image = ((System.Drawing.Image)(resources.GetObject("chkMOX.Image")));
			this.chkMOX.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkMOX.ImageAlign")));
			this.chkMOX.ImageIndex = ((int)(resources.GetObject("chkMOX.ImageIndex")));
			this.chkMOX.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkMOX.ImeMode")));
			this.chkMOX.Location = ((System.Drawing.Point)(resources.GetObject("chkMOX.Location")));
			this.chkMOX.Name = "chkMOX";
			this.chkMOX.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkMOX.RightToLeft")));
			this.chkMOX.Size = ((System.Drawing.Size)(resources.GetObject("chkMOX.Size")));
			this.chkMOX.TabIndex = ((int)(resources.GetObject("chkMOX.TabIndex")));
			this.chkMOX.Text = resources.GetString("chkMOX.Text");
			this.chkMOX.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkMOX.TextAlign")));
			this.toolTip1.SetToolTip(this.chkMOX, resources.GetString("chkMOX.ToolTip"));
			this.chkMOX.Visible = ((bool)(resources.GetObject("chkMOX.Visible")));
			this.chkMOX.Click += new System.EventHandler(this.chkMOX_Click);
			this.chkMOX.CheckedChanged += new System.EventHandler(this.chkMOX_CheckedChanged);
			// 
			// udRIT
			// 
			this.udRIT.AccessibleDescription = resources.GetString("udRIT.AccessibleDescription");
			this.udRIT.AccessibleName = resources.GetString("udRIT.AccessibleName");
			this.udRIT.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("udRIT.Anchor")));
			this.udRIT.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("udRIT.Dock")));
			this.udRIT.Enabled = ((bool)(resources.GetObject("udRIT.Enabled")));
			this.udRIT.Font = ((System.Drawing.Font)(resources.GetObject("udRIT.Font")));
			this.udRIT.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("udRIT.ImeMode")));
			this.udRIT.Increment = new System.Decimal(new int[] {
																	1,
																	0,
																	0,
																	0});
			this.udRIT.Location = ((System.Drawing.Point)(resources.GetObject("udRIT.Location")));
			this.udRIT.Maximum = new System.Decimal(new int[] {
																  9999,
																  0,
																  0,
																  0});
			this.udRIT.Minimum = new System.Decimal(new int[] {
																  9999,
																  0,
																  0,
																  -2147483648});
			this.udRIT.Name = "udRIT";
			this.udRIT.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("udRIT.RightToLeft")));
			this.udRIT.Size = ((System.Drawing.Size)(resources.GetObject("udRIT.Size")));
			this.udRIT.TabIndex = ((int)(resources.GetObject("udRIT.TabIndex")));
			this.udRIT.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("udRIT.TextAlign")));
			this.udRIT.ThousandsSeparator = ((bool)(resources.GetObject("udRIT.ThousandsSeparator")));
			this.toolTip1.SetToolTip(this.udRIT, resources.GetString("udRIT.ToolTip"));
			this.udRIT.UpDownAlign = ((System.Windows.Forms.LeftRightAlignment)(resources.GetObject("udRIT.UpDownAlign")));
			this.udRIT.Value = new System.Decimal(new int[] {
																0,
																0,
																0,
																0});
			this.udRIT.Visible = ((bool)(resources.GetObject("udRIT.Visible")));
			this.udRIT.LostFocus += new System.EventHandler(this.udRIT_LostFocus);
			this.udRIT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
			this.udRIT.ValueChanged += new System.EventHandler(this.udRIT_ValueChanged);
			// 
			// chkMUT
			// 
			this.chkMUT.AccessibleDescription = resources.GetString("chkMUT.AccessibleDescription");
			this.chkMUT.AccessibleName = resources.GetString("chkMUT.AccessibleName");
			this.chkMUT.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkMUT.Anchor")));
			this.chkMUT.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkMUT.Appearance")));
			this.chkMUT.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkMUT.BackgroundImage")));
			this.chkMUT.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkMUT.CheckAlign")));
			this.chkMUT.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkMUT.Dock")));
			this.chkMUT.Enabled = ((bool)(resources.GetObject("chkMUT.Enabled")));
			this.chkMUT.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkMUT.FlatStyle")));
			this.chkMUT.Font = ((System.Drawing.Font)(resources.GetObject("chkMUT.Font")));
			this.chkMUT.Image = ((System.Drawing.Image)(resources.GetObject("chkMUT.Image")));
			this.chkMUT.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkMUT.ImageAlign")));
			this.chkMUT.ImageIndex = ((int)(resources.GetObject("chkMUT.ImageIndex")));
			this.chkMUT.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkMUT.ImeMode")));
			this.chkMUT.Location = ((System.Drawing.Point)(resources.GetObject("chkMUT.Location")));
			this.chkMUT.Name = "chkMUT";
			this.chkMUT.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkMUT.RightToLeft")));
			this.chkMUT.Size = ((System.Drawing.Size)(resources.GetObject("chkMUT.Size")));
			this.chkMUT.TabIndex = ((int)(resources.GetObject("chkMUT.TabIndex")));
			this.chkMUT.Text = resources.GetString("chkMUT.Text");
			this.chkMUT.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkMUT.TextAlign")));
			this.toolTip1.SetToolTip(this.chkMUT, resources.GetString("chkMUT.ToolTip"));
			this.chkMUT.Visible = ((bool)(resources.GetObject("chkMUT.Visible")));
			this.chkMUT.CheckedChanged += new System.EventHandler(this.chkMUT_CheckedChanged);
			// 
			// chkXIT
			// 
			this.chkXIT.AccessibleDescription = resources.GetString("chkXIT.AccessibleDescription");
			this.chkXIT.AccessibleName = resources.GetString("chkXIT.AccessibleName");
			this.chkXIT.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkXIT.Anchor")));
			this.chkXIT.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkXIT.Appearance")));
			this.chkXIT.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkXIT.BackgroundImage")));
			this.chkXIT.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkXIT.CheckAlign")));
			this.chkXIT.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkXIT.Dock")));
			this.chkXIT.Enabled = ((bool)(resources.GetObject("chkXIT.Enabled")));
			this.chkXIT.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkXIT.FlatStyle")));
			this.chkXIT.Font = ((System.Drawing.Font)(resources.GetObject("chkXIT.Font")));
			this.chkXIT.Image = ((System.Drawing.Image)(resources.GetObject("chkXIT.Image")));
			this.chkXIT.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkXIT.ImageAlign")));
			this.chkXIT.ImageIndex = ((int)(resources.GetObject("chkXIT.ImageIndex")));
			this.chkXIT.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkXIT.ImeMode")));
			this.chkXIT.Location = ((System.Drawing.Point)(resources.GetObject("chkXIT.Location")));
			this.chkXIT.Name = "chkXIT";
			this.chkXIT.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkXIT.RightToLeft")));
			this.chkXIT.Size = ((System.Drawing.Size)(resources.GetObject("chkXIT.Size")));
			this.chkXIT.TabIndex = ((int)(resources.GetObject("chkXIT.TabIndex")));
			this.chkXIT.Text = resources.GetString("chkXIT.Text");
			this.chkXIT.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkXIT.TextAlign")));
			this.toolTip1.SetToolTip(this.chkXIT, resources.GetString("chkXIT.ToolTip"));
			this.chkXIT.Visible = ((bool)(resources.GetObject("chkXIT.Visible")));
			this.chkXIT.CheckedChanged += new System.EventHandler(this.chkXIT_CheckedChanged);
			// 
			// chkRIT
			// 
			this.chkRIT.AccessibleDescription = resources.GetString("chkRIT.AccessibleDescription");
			this.chkRIT.AccessibleName = resources.GetString("chkRIT.AccessibleName");
			this.chkRIT.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkRIT.Anchor")));
			this.chkRIT.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkRIT.Appearance")));
			this.chkRIT.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkRIT.BackgroundImage")));
			this.chkRIT.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkRIT.CheckAlign")));
			this.chkRIT.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkRIT.Dock")));
			this.chkRIT.Enabled = ((bool)(resources.GetObject("chkRIT.Enabled")));
			this.chkRIT.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkRIT.FlatStyle")));
			this.chkRIT.Font = ((System.Drawing.Font)(resources.GetObject("chkRIT.Font")));
			this.chkRIT.Image = ((System.Drawing.Image)(resources.GetObject("chkRIT.Image")));
			this.chkRIT.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkRIT.ImageAlign")));
			this.chkRIT.ImageIndex = ((int)(resources.GetObject("chkRIT.ImageIndex")));
			this.chkRIT.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkRIT.ImeMode")));
			this.chkRIT.Location = ((System.Drawing.Point)(resources.GetObject("chkRIT.Location")));
			this.chkRIT.Name = "chkRIT";
			this.chkRIT.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkRIT.RightToLeft")));
			this.chkRIT.Size = ((System.Drawing.Size)(resources.GetObject("chkRIT.Size")));
			this.chkRIT.TabIndex = ((int)(resources.GetObject("chkRIT.TabIndex")));
			this.chkRIT.Text = resources.GetString("chkRIT.Text");
			this.chkRIT.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkRIT.TextAlign")));
			this.toolTip1.SetToolTip(this.chkRIT, resources.GetString("chkRIT.ToolTip"));
			this.chkRIT.Visible = ((bool)(resources.GetObject("chkRIT.Visible")));
			this.chkRIT.CheckedChanged += new System.EventHandler(this.chkRIT_CheckedChanged);
			// 
			// lblPWR
			// 
			this.lblPWR.AccessibleDescription = resources.GetString("lblPWR.AccessibleDescription");
			this.lblPWR.AccessibleName = resources.GetString("lblPWR.AccessibleName");
			this.lblPWR.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblPWR.Anchor")));
			this.lblPWR.AutoSize = ((bool)(resources.GetObject("lblPWR.AutoSize")));
			this.lblPWR.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblPWR.Dock")));
			this.lblPWR.Enabled = ((bool)(resources.GetObject("lblPWR.Enabled")));
			this.lblPWR.Font = ((System.Drawing.Font)(resources.GetObject("lblPWR.Font")));
			this.lblPWR.Image = ((System.Drawing.Image)(resources.GetObject("lblPWR.Image")));
			this.lblPWR.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblPWR.ImageAlign")));
			this.lblPWR.ImageIndex = ((int)(resources.GetObject("lblPWR.ImageIndex")));
			this.lblPWR.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblPWR.ImeMode")));
			this.lblPWR.Location = ((System.Drawing.Point)(resources.GetObject("lblPWR.Location")));
			this.lblPWR.Name = "lblPWR";
			this.lblPWR.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblPWR.RightToLeft")));
			this.lblPWR.Size = ((System.Drawing.Size)(resources.GetObject("lblPWR.Size")));
			this.lblPWR.TabIndex = ((int)(resources.GetObject("lblPWR.TabIndex")));
			this.lblPWR.Text = resources.GetString("lblPWR.Text");
			this.lblPWR.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblPWR.TextAlign")));
			this.toolTip1.SetToolTip(this.lblPWR, resources.GetString("lblPWR.ToolTip"));
			this.lblPWR.Visible = ((bool)(resources.GetObject("lblPWR.Visible")));
			// 
			// udPWR
			// 
			this.udPWR.AccessibleDescription = resources.GetString("udPWR.AccessibleDescription");
			this.udPWR.AccessibleName = resources.GetString("udPWR.AccessibleName");
			this.udPWR.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("udPWR.Anchor")));
			this.udPWR.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("udPWR.Dock")));
			this.udPWR.Enabled = ((bool)(resources.GetObject("udPWR.Enabled")));
			this.udPWR.Font = ((System.Drawing.Font)(resources.GetObject("udPWR.Font")));
			this.udPWR.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("udPWR.ImeMode")));
			this.udPWR.Increment = new System.Decimal(new int[] {
																	1,
																	0,
																	0,
																	0});
			this.udPWR.Location = ((System.Drawing.Point)(resources.GetObject("udPWR.Location")));
			this.udPWR.Maximum = new System.Decimal(new int[] {
																  100,
																  0,
																  0,
																  0});
			this.udPWR.Minimum = new System.Decimal(new int[] {
																  0,
																  0,
																  0,
																  0});
			this.udPWR.Name = "udPWR";
			this.udPWR.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("udPWR.RightToLeft")));
			this.udPWR.Size = ((System.Drawing.Size)(resources.GetObject("udPWR.Size")));
			this.udPWR.TabIndex = ((int)(resources.GetObject("udPWR.TabIndex")));
			this.udPWR.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("udPWR.TextAlign")));
			this.udPWR.ThousandsSeparator = ((bool)(resources.GetObject("udPWR.ThousandsSeparator")));
			this.toolTip1.SetToolTip(this.udPWR, resources.GetString("udPWR.ToolTip"));
			this.udPWR.UpDownAlign = ((System.Windows.Forms.LeftRightAlignment)(resources.GetObject("udPWR.UpDownAlign")));
			this.udPWR.Value = new System.Decimal(new int[] {
																50,
																0,
																0,
																0});
			this.udPWR.Visible = ((bool)(resources.GetObject("udPWR.Visible")));
			this.udPWR.LostFocus += new System.EventHandler(this.udPWR_LostFocus);
			this.udPWR.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
			this.udPWR.ValueChanged += new System.EventHandler(this.udPWR_ValueChanged);
			// 
			// lblAF
			// 
			this.lblAF.AccessibleDescription = resources.GetString("lblAF.AccessibleDescription");
			this.lblAF.AccessibleName = resources.GetString("lblAF.AccessibleName");
			this.lblAF.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblAF.Anchor")));
			this.lblAF.AutoSize = ((bool)(resources.GetObject("lblAF.AutoSize")));
			this.lblAF.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblAF.Dock")));
			this.lblAF.Enabled = ((bool)(resources.GetObject("lblAF.Enabled")));
			this.lblAF.Font = ((System.Drawing.Font)(resources.GetObject("lblAF.Font")));
			this.lblAF.Image = ((System.Drawing.Image)(resources.GetObject("lblAF.Image")));
			this.lblAF.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblAF.ImageAlign")));
			this.lblAF.ImageIndex = ((int)(resources.GetObject("lblAF.ImageIndex")));
			this.lblAF.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblAF.ImeMode")));
			this.lblAF.Location = ((System.Drawing.Point)(resources.GetObject("lblAF.Location")));
			this.lblAF.Name = "lblAF";
			this.lblAF.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblAF.RightToLeft")));
			this.lblAF.Size = ((System.Drawing.Size)(resources.GetObject("lblAF.Size")));
			this.lblAF.TabIndex = ((int)(resources.GetObject("lblAF.TabIndex")));
			this.lblAF.Text = resources.GetString("lblAF.Text");
			this.lblAF.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblAF.TextAlign")));
			this.toolTip1.SetToolTip(this.lblAF, resources.GetString("lblAF.ToolTip"));
			this.lblAF.Visible = ((bool)(resources.GetObject("lblAF.Visible")));
			// 
			// udAF
			// 
			this.udAF.AccessibleDescription = resources.GetString("udAF.AccessibleDescription");
			this.udAF.AccessibleName = resources.GetString("udAF.AccessibleName");
			this.udAF.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("udAF.Anchor")));
			this.udAF.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("udAF.Dock")));
			this.udAF.Enabled = ((bool)(resources.GetObject("udAF.Enabled")));
			this.udAF.Font = ((System.Drawing.Font)(resources.GetObject("udAF.Font")));
			this.udAF.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("udAF.ImeMode")));
			this.udAF.Increment = new System.Decimal(new int[] {
																   1,
																   0,
																   0,
																   0});
			this.udAF.Location = ((System.Drawing.Point)(resources.GetObject("udAF.Location")));
			this.udAF.Maximum = new System.Decimal(new int[] {
																 100,
																 0,
																 0,
																 0});
			this.udAF.Minimum = new System.Decimal(new int[] {
																 0,
																 0,
																 0,
																 0});
			this.udAF.Name = "udAF";
			this.udAF.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("udAF.RightToLeft")));
			this.udAF.Size = ((System.Drawing.Size)(resources.GetObject("udAF.Size")));
			this.udAF.TabIndex = ((int)(resources.GetObject("udAF.TabIndex")));
			this.udAF.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("udAF.TextAlign")));
			this.udAF.ThousandsSeparator = ((bool)(resources.GetObject("udAF.ThousandsSeparator")));
			this.toolTip1.SetToolTip(this.udAF, resources.GetString("udAF.ToolTip"));
			this.udAF.UpDownAlign = ((System.Windows.Forms.LeftRightAlignment)(resources.GetObject("udAF.UpDownAlign")));
			this.udAF.Value = new System.Decimal(new int[] {
															   50,
															   0,
															   0,
															   0});
			this.udAF.Visible = ((bool)(resources.GetObject("udAF.Visible")));
			this.udAF.LostFocus += new System.EventHandler(this.udAF_LostFocus);
			this.udAF.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
			this.udAF.ValueChanged += new System.EventHandler(this.udAF_ValueChanged);
			// 
			// lblMIC
			// 
			this.lblMIC.AccessibleDescription = resources.GetString("lblMIC.AccessibleDescription");
			this.lblMIC.AccessibleName = resources.GetString("lblMIC.AccessibleName");
			this.lblMIC.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblMIC.Anchor")));
			this.lblMIC.AutoSize = ((bool)(resources.GetObject("lblMIC.AutoSize")));
			this.lblMIC.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblMIC.Dock")));
			this.lblMIC.Enabled = ((bool)(resources.GetObject("lblMIC.Enabled")));
			this.lblMIC.Font = ((System.Drawing.Font)(resources.GetObject("lblMIC.Font")));
			this.lblMIC.Image = ((System.Drawing.Image)(resources.GetObject("lblMIC.Image")));
			this.lblMIC.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblMIC.ImageAlign")));
			this.lblMIC.ImageIndex = ((int)(resources.GetObject("lblMIC.ImageIndex")));
			this.lblMIC.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblMIC.ImeMode")));
			this.lblMIC.Location = ((System.Drawing.Point)(resources.GetObject("lblMIC.Location")));
			this.lblMIC.Name = "lblMIC";
			this.lblMIC.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblMIC.RightToLeft")));
			this.lblMIC.Size = ((System.Drawing.Size)(resources.GetObject("lblMIC.Size")));
			this.lblMIC.TabIndex = ((int)(resources.GetObject("lblMIC.TabIndex")));
			this.lblMIC.Text = resources.GetString("lblMIC.Text");
			this.lblMIC.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblMIC.TextAlign")));
			this.toolTip1.SetToolTip(this.lblMIC, resources.GetString("lblMIC.ToolTip"));
			this.lblMIC.Visible = ((bool)(resources.GetObject("lblMIC.Visible")));
			// 
			// udMIC
			// 
			this.udMIC.AccessibleDescription = resources.GetString("udMIC.AccessibleDescription");
			this.udMIC.AccessibleName = resources.GetString("udMIC.AccessibleName");
			this.udMIC.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("udMIC.Anchor")));
			this.udMIC.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("udMIC.Dock")));
			this.udMIC.Enabled = ((bool)(resources.GetObject("udMIC.Enabled")));
			this.udMIC.Font = ((System.Drawing.Font)(resources.GetObject("udMIC.Font")));
			this.udMIC.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("udMIC.ImeMode")));
			this.udMIC.Increment = new System.Decimal(new int[] {
																	1,
																	0,
																	0,
																	0});
			this.udMIC.Location = ((System.Drawing.Point)(resources.GetObject("udMIC.Location")));
			this.udMIC.Maximum = new System.Decimal(new int[] {
																  100,
																  0,
																  0,
																  0});
			this.udMIC.Minimum = new System.Decimal(new int[] {
																  0,
																  0,
																  0,
																  0});
			this.udMIC.Name = "udMIC";
			this.udMIC.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("udMIC.RightToLeft")));
			this.udMIC.Size = ((System.Drawing.Size)(resources.GetObject("udMIC.Size")));
			this.udMIC.TabIndex = ((int)(resources.GetObject("udMIC.TabIndex")));
			this.udMIC.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("udMIC.TextAlign")));
			this.udMIC.ThousandsSeparator = ((bool)(resources.GetObject("udMIC.ThousandsSeparator")));
			this.toolTip1.SetToolTip(this.udMIC, resources.GetString("udMIC.ToolTip"));
			this.udMIC.UpDownAlign = ((System.Windows.Forms.LeftRightAlignment)(resources.GetObject("udMIC.UpDownAlign")));
			this.udMIC.Value = new System.Decimal(new int[] {
																50,
																0,
																0,
																0});
			this.udMIC.Visible = ((bool)(resources.GetObject("udMIC.Visible")));
			this.udMIC.LostFocus += new System.EventHandler(this.udMIC_LostFocus);
			this.udMIC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
			this.udMIC.ValueChanged += new System.EventHandler(this.udMIC_ValueChanged);
			// 
			// chkPipe
			// 
			this.chkPipe.AccessibleDescription = resources.GetString("chkPipe.AccessibleDescription");
			this.chkPipe.AccessibleName = resources.GetString("chkPipe.AccessibleName");
			this.chkPipe.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkPipe.Anchor")));
			this.chkPipe.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkPipe.Appearance")));
			this.chkPipe.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkPipe.BackgroundImage")));
			this.chkPipe.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkPipe.CheckAlign")));
			this.chkPipe.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkPipe.Dock")));
			this.chkPipe.Enabled = ((bool)(resources.GetObject("chkPipe.Enabled")));
			this.chkPipe.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkPipe.FlatStyle")));
			this.chkPipe.Font = ((System.Drawing.Font)(resources.GetObject("chkPipe.Font")));
			this.chkPipe.Image = ((System.Drawing.Image)(resources.GetObject("chkPipe.Image")));
			this.chkPipe.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkPipe.ImageAlign")));
			this.chkPipe.ImageIndex = ((int)(resources.GetObject("chkPipe.ImageIndex")));
			this.chkPipe.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkPipe.ImeMode")));
			this.chkPipe.Location = ((System.Drawing.Point)(resources.GetObject("chkPipe.Location")));
			this.chkPipe.Name = "chkPipe";
			this.chkPipe.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkPipe.RightToLeft")));
			this.chkPipe.Size = ((System.Drawing.Size)(resources.GetObject("chkPipe.Size")));
			this.chkPipe.TabIndex = ((int)(resources.GetObject("chkPipe.TabIndex")));
			this.chkPipe.Text = resources.GetString("chkPipe.Text");
			this.chkPipe.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkPipe.TextAlign")));
			this.toolTip1.SetToolTip(this.chkPipe, resources.GetString("chkPipe.ToolTip"));
			this.chkPipe.Visible = ((bool)(resources.GetObject("chkPipe.Visible")));
			this.chkPipe.CheckedChanged += new System.EventHandler(this.chkPipe_CheckedChanged);
			// 
			// grpMultimeter
			// 
			this.grpMultimeter.AccessibleDescription = resources.GetString("grpMultimeter.AccessibleDescription");
			this.grpMultimeter.AccessibleName = resources.GetString("grpMultimeter.AccessibleName");
			this.grpMultimeter.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpMultimeter.Anchor")));
			this.grpMultimeter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpMultimeter.BackgroundImage")));
			this.grpMultimeter.Controls.Add(this.comboMeterTXMode);
			this.grpMultimeter.Controls.Add(this.picMultiMeterDigital);
			this.grpMultimeter.Controls.Add(this.lblMultiSMeter);
			this.grpMultimeter.Controls.Add(this.comboMeterRXMode);
			this.grpMultimeter.Controls.Add(this.txtMultiText);
			this.grpMultimeter.Controls.Add(this.picMultimeterAnalog);
			this.grpMultimeter.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpMultimeter.Dock")));
			this.grpMultimeter.Enabled = ((bool)(resources.GetObject("grpMultimeter.Enabled")));
			this.grpMultimeter.Font = ((System.Drawing.Font)(resources.GetObject("grpMultimeter.Font")));
			this.grpMultimeter.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpMultimeter.ImeMode")));
			this.grpMultimeter.Location = ((System.Drawing.Point)(resources.GetObject("grpMultimeter.Location")));
			this.grpMultimeter.Name = "grpMultimeter";
			this.grpMultimeter.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpMultimeter.RightToLeft")));
			this.grpMultimeter.Size = ((System.Drawing.Size)(resources.GetObject("grpMultimeter.Size")));
			this.grpMultimeter.TabIndex = ((int)(resources.GetObject("grpMultimeter.TabIndex")));
			this.grpMultimeter.TabStop = false;
			this.grpMultimeter.Text = resources.GetString("grpMultimeter.Text");
			this.toolTip1.SetToolTip(this.grpMultimeter, resources.GetString("grpMultimeter.ToolTip"));
			this.grpMultimeter.Visible = ((bool)(resources.GetObject("grpMultimeter.Visible")));
			// 
			// comboMeterTXMode
			// 
			this.comboMeterTXMode.AccessibleDescription = resources.GetString("comboMeterTXMode.AccessibleDescription");
			this.comboMeterTXMode.AccessibleName = resources.GetString("comboMeterTXMode.AccessibleName");
			this.comboMeterTXMode.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("comboMeterTXMode.Anchor")));
			this.comboMeterTXMode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("comboMeterTXMode.BackgroundImage")));
			this.comboMeterTXMode.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("comboMeterTXMode.Dock")));
			this.comboMeterTXMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboMeterTXMode.DropDownWidth = 72;
			this.comboMeterTXMode.Enabled = ((bool)(resources.GetObject("comboMeterTXMode.Enabled")));
			this.comboMeterTXMode.Font = ((System.Drawing.Font)(resources.GetObject("comboMeterTXMode.Font")));
			this.comboMeterTXMode.ForeColor = System.Drawing.Color.Gray;
			this.comboMeterTXMode.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("comboMeterTXMode.ImeMode")));
			this.comboMeterTXMode.IntegralHeight = ((bool)(resources.GetObject("comboMeterTXMode.IntegralHeight")));
			this.comboMeterTXMode.ItemHeight = ((int)(resources.GetObject("comboMeterTXMode.ItemHeight")));
			this.comboMeterTXMode.Location = ((System.Drawing.Point)(resources.GetObject("comboMeterTXMode.Location")));
			this.comboMeterTXMode.MaxDropDownItems = ((int)(resources.GetObject("comboMeterTXMode.MaxDropDownItems")));
			this.comboMeterTXMode.MaxLength = ((int)(resources.GetObject("comboMeterTXMode.MaxLength")));
			this.comboMeterTXMode.Name = "comboMeterTXMode";
			this.comboMeterTXMode.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("comboMeterTXMode.RightToLeft")));
			this.comboMeterTXMode.Size = ((System.Drawing.Size)(resources.GetObject("comboMeterTXMode.Size")));
			this.comboMeterTXMode.TabIndex = ((int)(resources.GetObject("comboMeterTXMode.TabIndex")));
			this.comboMeterTXMode.Text = resources.GetString("comboMeterTXMode.Text");
			this.toolTip1.SetToolTip(this.comboMeterTXMode, resources.GetString("comboMeterTXMode.ToolTip"));
			this.comboMeterTXMode.Visible = ((bool)(resources.GetObject("comboMeterTXMode.Visible")));
			this.comboMeterTXMode.SelectedIndexChanged += new System.EventHandler(this.comboMeterTXMode_SelectedIndexChanged);
			// 
			// picMultiMeterDigital
			// 
			this.picMultiMeterDigital.AccessibleDescription = resources.GetString("picMultiMeterDigital.AccessibleDescription");
			this.picMultiMeterDigital.AccessibleName = resources.GetString("picMultiMeterDigital.AccessibleName");
			this.picMultiMeterDigital.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("picMultiMeterDigital.Anchor")));
			this.picMultiMeterDigital.BackColor = System.Drawing.Color.Black;
			this.picMultiMeterDigital.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picMultiMeterDigital.BackgroundImage")));
			this.picMultiMeterDigital.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picMultiMeterDigital.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("picMultiMeterDigital.Dock")));
			this.picMultiMeterDigital.Enabled = ((bool)(resources.GetObject("picMultiMeterDigital.Enabled")));
			this.picMultiMeterDigital.Font = ((System.Drawing.Font)(resources.GetObject("picMultiMeterDigital.Font")));
			this.picMultiMeterDigital.Image = ((System.Drawing.Image)(resources.GetObject("picMultiMeterDigital.Image")));
			this.picMultiMeterDigital.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("picMultiMeterDigital.ImeMode")));
			this.picMultiMeterDigital.Location = ((System.Drawing.Point)(resources.GetObject("picMultiMeterDigital.Location")));
			this.picMultiMeterDigital.Name = "picMultiMeterDigital";
			this.picMultiMeterDigital.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("picMultiMeterDigital.RightToLeft")));
			this.picMultiMeterDigital.Size = ((System.Drawing.Size)(resources.GetObject("picMultiMeterDigital.Size")));
			this.picMultiMeterDigital.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("picMultiMeterDigital.SizeMode")));
			this.picMultiMeterDigital.TabIndex = ((int)(resources.GetObject("picMultiMeterDigital.TabIndex")));
			this.picMultiMeterDigital.TabStop = false;
			this.picMultiMeterDigital.Text = resources.GetString("picMultiMeterDigital.Text");
			this.toolTip1.SetToolTip(this.picMultiMeterDigital, resources.GetString("picMultiMeterDigital.ToolTip"));
			this.picMultiMeterDigital.Visible = ((bool)(resources.GetObject("picMultiMeterDigital.Visible")));
			this.picMultiMeterDigital.Paint += new System.Windows.Forms.PaintEventHandler(this.picMultiMeterDigital_Paint);
			// 
			// lblMultiSMeter
			// 
			this.lblMultiSMeter.AccessibleDescription = resources.GetString("lblMultiSMeter.AccessibleDescription");
			this.lblMultiSMeter.AccessibleName = resources.GetString("lblMultiSMeter.AccessibleName");
			this.lblMultiSMeter.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblMultiSMeter.Anchor")));
			this.lblMultiSMeter.AutoSize = ((bool)(resources.GetObject("lblMultiSMeter.AutoSize")));
			this.lblMultiSMeter.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblMultiSMeter.Dock")));
			this.lblMultiSMeter.Enabled = ((bool)(resources.GetObject("lblMultiSMeter.Enabled")));
			this.lblMultiSMeter.Font = ((System.Drawing.Font)(resources.GetObject("lblMultiSMeter.Font")));
			this.lblMultiSMeter.Image = ((System.Drawing.Image)(resources.GetObject("lblMultiSMeter.Image")));
			this.lblMultiSMeter.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblMultiSMeter.ImageAlign")));
			this.lblMultiSMeter.ImageIndex = ((int)(resources.GetObject("lblMultiSMeter.ImageIndex")));
			this.lblMultiSMeter.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblMultiSMeter.ImeMode")));
			this.lblMultiSMeter.Location = ((System.Drawing.Point)(resources.GetObject("lblMultiSMeter.Location")));
			this.lblMultiSMeter.Name = "lblMultiSMeter";
			this.lblMultiSMeter.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblMultiSMeter.RightToLeft")));
			this.lblMultiSMeter.Size = ((System.Drawing.Size)(resources.GetObject("lblMultiSMeter.Size")));
			this.lblMultiSMeter.TabIndex = ((int)(resources.GetObject("lblMultiSMeter.TabIndex")));
			this.lblMultiSMeter.Text = resources.GetString("lblMultiSMeter.Text");
			this.lblMultiSMeter.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblMultiSMeter.TextAlign")));
			this.toolTip1.SetToolTip(this.lblMultiSMeter, resources.GetString("lblMultiSMeter.ToolTip"));
			this.lblMultiSMeter.Visible = ((bool)(resources.GetObject("lblMultiSMeter.Visible")));
			// 
			// comboMeterRXMode
			// 
			this.comboMeterRXMode.AccessibleDescription = resources.GetString("comboMeterRXMode.AccessibleDescription");
			this.comboMeterRXMode.AccessibleName = resources.GetString("comboMeterRXMode.AccessibleName");
			this.comboMeterRXMode.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("comboMeterRXMode.Anchor")));
			this.comboMeterRXMode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("comboMeterRXMode.BackgroundImage")));
			this.comboMeterRXMode.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("comboMeterRXMode.Dock")));
			this.comboMeterRXMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboMeterRXMode.DropDownWidth = 72;
			this.comboMeterRXMode.Enabled = ((bool)(resources.GetObject("comboMeterRXMode.Enabled")));
			this.comboMeterRXMode.Font = ((System.Drawing.Font)(resources.GetObject("comboMeterRXMode.Font")));
			this.comboMeterRXMode.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("comboMeterRXMode.ImeMode")));
			this.comboMeterRXMode.IntegralHeight = ((bool)(resources.GetObject("comboMeterRXMode.IntegralHeight")));
			this.comboMeterRXMode.ItemHeight = ((int)(resources.GetObject("comboMeterRXMode.ItemHeight")));
			this.comboMeterRXMode.Location = ((System.Drawing.Point)(resources.GetObject("comboMeterRXMode.Location")));
			this.comboMeterRXMode.MaxDropDownItems = ((int)(resources.GetObject("comboMeterRXMode.MaxDropDownItems")));
			this.comboMeterRXMode.MaxLength = ((int)(resources.GetObject("comboMeterRXMode.MaxLength")));
			this.comboMeterRXMode.Name = "comboMeterRXMode";
			this.comboMeterRXMode.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("comboMeterRXMode.RightToLeft")));
			this.comboMeterRXMode.Size = ((System.Drawing.Size)(resources.GetObject("comboMeterRXMode.Size")));
			this.comboMeterRXMode.TabIndex = ((int)(resources.GetObject("comboMeterRXMode.TabIndex")));
			this.comboMeterRXMode.Text = resources.GetString("comboMeterRXMode.Text");
			this.toolTip1.SetToolTip(this.comboMeterRXMode, resources.GetString("comboMeterRXMode.ToolTip"));
			this.comboMeterRXMode.Visible = ((bool)(resources.GetObject("comboMeterRXMode.Visible")));
			this.comboMeterRXMode.SelectedIndexChanged += new System.EventHandler(this.comboMeterRXMode_SelectedIndexChanged);
			// 
			// txtMultiText
			// 
			this.txtMultiText.AccessibleDescription = resources.GetString("txtMultiText.AccessibleDescription");
			this.txtMultiText.AccessibleName = resources.GetString("txtMultiText.AccessibleName");
			this.txtMultiText.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtMultiText.Anchor")));
			this.txtMultiText.AutoSize = ((bool)(resources.GetObject("txtMultiText.AutoSize")));
			this.txtMultiText.BackColor = System.Drawing.Color.Black;
			this.txtMultiText.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtMultiText.BackgroundImage")));
			this.txtMultiText.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtMultiText.Dock")));
			this.txtMultiText.Enabled = ((bool)(resources.GetObject("txtMultiText.Enabled")));
			this.txtMultiText.Font = ((System.Drawing.Font)(resources.GetObject("txtMultiText.Font")));
			this.txtMultiText.ForeColor = System.Drawing.Color.Yellow;
			this.txtMultiText.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtMultiText.ImeMode")));
			this.txtMultiText.Location = ((System.Drawing.Point)(resources.GetObject("txtMultiText.Location")));
			this.txtMultiText.MaxLength = ((int)(resources.GetObject("txtMultiText.MaxLength")));
			this.txtMultiText.Multiline = ((bool)(resources.GetObject("txtMultiText.Multiline")));
			this.txtMultiText.Name = "txtMultiText";
			this.txtMultiText.PasswordChar = ((char)(resources.GetObject("txtMultiText.PasswordChar")));
			this.txtMultiText.ReadOnly = true;
			this.txtMultiText.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtMultiText.RightToLeft")));
			this.txtMultiText.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtMultiText.ScrollBars")));
			this.txtMultiText.Size = ((System.Drawing.Size)(resources.GetObject("txtMultiText.Size")));
			this.txtMultiText.TabIndex = ((int)(resources.GetObject("txtMultiText.TabIndex")));
			this.txtMultiText.Text = resources.GetString("txtMultiText.Text");
			this.txtMultiText.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtMultiText.TextAlign")));
			this.toolTip1.SetToolTip(this.txtMultiText, resources.GetString("txtMultiText.ToolTip"));
			this.txtMultiText.Visible = ((bool)(resources.GetObject("txtMultiText.Visible")));
			this.txtMultiText.WordWrap = ((bool)(resources.GetObject("txtMultiText.WordWrap")));
			this.txtMultiText.GotFocus += new System.EventHandler(this.HideFocus);
			// 
			// picMultimeterAnalog
			// 
			this.picMultimeterAnalog.AccessibleDescription = resources.GetString("picMultimeterAnalog.AccessibleDescription");
			this.picMultimeterAnalog.AccessibleName = resources.GetString("picMultimeterAnalog.AccessibleName");
			this.picMultimeterAnalog.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("picMultimeterAnalog.Anchor")));
			this.picMultimeterAnalog.BackColor = System.Drawing.Color.Black;
			this.picMultimeterAnalog.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picMultimeterAnalog.BackgroundImage")));
			this.picMultimeterAnalog.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("picMultimeterAnalog.Dock")));
			this.picMultimeterAnalog.Enabled = ((bool)(resources.GetObject("picMultimeterAnalog.Enabled")));
			this.picMultimeterAnalog.Font = ((System.Drawing.Font)(resources.GetObject("picMultimeterAnalog.Font")));
			this.picMultimeterAnalog.Image = ((System.Drawing.Image)(resources.GetObject("picMultimeterAnalog.Image")));
			this.picMultimeterAnalog.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("picMultimeterAnalog.ImeMode")));
			this.picMultimeterAnalog.Location = ((System.Drawing.Point)(resources.GetObject("picMultimeterAnalog.Location")));
			this.picMultimeterAnalog.Name = "picMultimeterAnalog";
			this.picMultimeterAnalog.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("picMultimeterAnalog.RightToLeft")));
			this.picMultimeterAnalog.Size = ((System.Drawing.Size)(resources.GetObject("picMultimeterAnalog.Size")));
			this.picMultimeterAnalog.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("picMultimeterAnalog.SizeMode")));
			this.picMultimeterAnalog.TabIndex = ((int)(resources.GetObject("picMultimeterAnalog.TabIndex")));
			this.picMultimeterAnalog.TabStop = false;
			this.picMultimeterAnalog.Text = resources.GetString("picMultimeterAnalog.Text");
			this.toolTip1.SetToolTip(this.picMultimeterAnalog, resources.GetString("picMultimeterAnalog.ToolTip"));
			this.picMultimeterAnalog.Visible = ((bool)(resources.GetObject("picMultimeterAnalog.Visible")));
			// 
			// grpVFO
			// 
			this.grpVFO.AccessibleDescription = resources.GetString("grpVFO.AccessibleDescription");
			this.grpVFO.AccessibleName = resources.GetString("grpVFO.AccessibleName");
			this.grpVFO.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpVFO.Anchor")));
			this.grpVFO.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpVFO.BackgroundImage")));
			this.grpVFO.Controls.Add(this.btnZeroBeat);
			this.grpVFO.Controls.Add(this.btnRITReset);
			this.grpVFO.Controls.Add(this.btnXITReset);
			this.grpVFO.Controls.Add(this.btnIFtoVFO);
			this.grpVFO.Controls.Add(this.btnVFOSwap);
			this.grpVFO.Controls.Add(this.btnVFOBtoA);
			this.grpVFO.Controls.Add(this.btnVFOAtoB);
			this.grpVFO.Controls.Add(this.udXIT);
			this.grpVFO.Controls.Add(this.chkXIT);
			this.grpVFO.Controls.Add(this.chkRIT);
			this.grpVFO.Controls.Add(this.udRIT);
			this.grpVFO.Controls.Add(this.chkVFOSplit);
			this.grpVFO.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpVFO.Dock")));
			this.grpVFO.Enabled = ((bool)(resources.GetObject("grpVFO.Enabled")));
			this.grpVFO.Font = ((System.Drawing.Font)(resources.GetObject("grpVFO.Font")));
			this.grpVFO.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpVFO.ImeMode")));
			this.grpVFO.Location = ((System.Drawing.Point)(resources.GetObject("grpVFO.Location")));
			this.grpVFO.Name = "grpVFO";
			this.grpVFO.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpVFO.RightToLeft")));
			this.grpVFO.Size = ((System.Drawing.Size)(resources.GetObject("grpVFO.Size")));
			this.grpVFO.TabIndex = ((int)(resources.GetObject("grpVFO.TabIndex")));
			this.grpVFO.TabStop = false;
			this.grpVFO.Text = resources.GetString("grpVFO.Text");
			this.toolTip1.SetToolTip(this.grpVFO, resources.GetString("grpVFO.ToolTip"));
			this.grpVFO.Visible = ((bool)(resources.GetObject("grpVFO.Visible")));
			// 
			// btnZeroBeat
			// 
			this.btnZeroBeat.AccessibleDescription = resources.GetString("btnZeroBeat.AccessibleDescription");
			this.btnZeroBeat.AccessibleName = resources.GetString("btnZeroBeat.AccessibleName");
			this.btnZeroBeat.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnZeroBeat.Anchor")));
			this.btnZeroBeat.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnZeroBeat.BackgroundImage")));
			this.btnZeroBeat.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnZeroBeat.Dock")));
			this.btnZeroBeat.Enabled = ((bool)(resources.GetObject("btnZeroBeat.Enabled")));
			this.btnZeroBeat.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnZeroBeat.FlatStyle")));
			this.btnZeroBeat.Font = ((System.Drawing.Font)(resources.GetObject("btnZeroBeat.Font")));
			this.btnZeroBeat.Image = ((System.Drawing.Image)(resources.GetObject("btnZeroBeat.Image")));
			this.btnZeroBeat.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnZeroBeat.ImageAlign")));
			this.btnZeroBeat.ImageIndex = ((int)(resources.GetObject("btnZeroBeat.ImageIndex")));
			this.btnZeroBeat.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnZeroBeat.ImeMode")));
			this.btnZeroBeat.Location = ((System.Drawing.Point)(resources.GetObject("btnZeroBeat.Location")));
			this.btnZeroBeat.Name = "btnZeroBeat";
			this.btnZeroBeat.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnZeroBeat.RightToLeft")));
			this.btnZeroBeat.Size = ((System.Drawing.Size)(resources.GetObject("btnZeroBeat.Size")));
			this.btnZeroBeat.TabIndex = ((int)(resources.GetObject("btnZeroBeat.TabIndex")));
			this.btnZeroBeat.Text = resources.GetString("btnZeroBeat.Text");
			this.btnZeroBeat.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnZeroBeat.TextAlign")));
			this.toolTip1.SetToolTip(this.btnZeroBeat, resources.GetString("btnZeroBeat.ToolTip"));
			this.btnZeroBeat.Visible = ((bool)(resources.GetObject("btnZeroBeat.Visible")));
			this.btnZeroBeat.Click += new System.EventHandler(this.btnZeroBeat_Click);
			// 
			// btnRITReset
			// 
			this.btnRITReset.AccessibleDescription = resources.GetString("btnRITReset.AccessibleDescription");
			this.btnRITReset.AccessibleName = resources.GetString("btnRITReset.AccessibleName");
			this.btnRITReset.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnRITReset.Anchor")));
			this.btnRITReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRITReset.BackgroundImage")));
			this.btnRITReset.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnRITReset.Dock")));
			this.btnRITReset.Enabled = ((bool)(resources.GetObject("btnRITReset.Enabled")));
			this.btnRITReset.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnRITReset.FlatStyle")));
			this.btnRITReset.Font = ((System.Drawing.Font)(resources.GetObject("btnRITReset.Font")));
			this.btnRITReset.Image = ((System.Drawing.Image)(resources.GetObject("btnRITReset.Image")));
			this.btnRITReset.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnRITReset.ImageAlign")));
			this.btnRITReset.ImageIndex = ((int)(resources.GetObject("btnRITReset.ImageIndex")));
			this.btnRITReset.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnRITReset.ImeMode")));
			this.btnRITReset.Location = ((System.Drawing.Point)(resources.GetObject("btnRITReset.Location")));
			this.btnRITReset.Name = "btnRITReset";
			this.btnRITReset.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnRITReset.RightToLeft")));
			this.btnRITReset.Size = ((System.Drawing.Size)(resources.GetObject("btnRITReset.Size")));
			this.btnRITReset.TabIndex = ((int)(resources.GetObject("btnRITReset.TabIndex")));
			this.btnRITReset.Text = resources.GetString("btnRITReset.Text");
			this.btnRITReset.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnRITReset.TextAlign")));
			this.toolTip1.SetToolTip(this.btnRITReset, resources.GetString("btnRITReset.ToolTip"));
			this.btnRITReset.Visible = ((bool)(resources.GetObject("btnRITReset.Visible")));
			this.btnRITReset.Click += new System.EventHandler(this.btnRITReset_Click);
			// 
			// btnXITReset
			// 
			this.btnXITReset.AccessibleDescription = resources.GetString("btnXITReset.AccessibleDescription");
			this.btnXITReset.AccessibleName = resources.GetString("btnXITReset.AccessibleName");
			this.btnXITReset.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnXITReset.Anchor")));
			this.btnXITReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnXITReset.BackgroundImage")));
			this.btnXITReset.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnXITReset.Dock")));
			this.btnXITReset.Enabled = ((bool)(resources.GetObject("btnXITReset.Enabled")));
			this.btnXITReset.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnXITReset.FlatStyle")));
			this.btnXITReset.Font = ((System.Drawing.Font)(resources.GetObject("btnXITReset.Font")));
			this.btnXITReset.Image = ((System.Drawing.Image)(resources.GetObject("btnXITReset.Image")));
			this.btnXITReset.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnXITReset.ImageAlign")));
			this.btnXITReset.ImageIndex = ((int)(resources.GetObject("btnXITReset.ImageIndex")));
			this.btnXITReset.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnXITReset.ImeMode")));
			this.btnXITReset.Location = ((System.Drawing.Point)(resources.GetObject("btnXITReset.Location")));
			this.btnXITReset.Name = "btnXITReset";
			this.btnXITReset.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnXITReset.RightToLeft")));
			this.btnXITReset.Size = ((System.Drawing.Size)(resources.GetObject("btnXITReset.Size")));
			this.btnXITReset.TabIndex = ((int)(resources.GetObject("btnXITReset.TabIndex")));
			this.btnXITReset.Text = resources.GetString("btnXITReset.Text");
			this.btnXITReset.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnXITReset.TextAlign")));
			this.toolTip1.SetToolTip(this.btnXITReset, resources.GetString("btnXITReset.ToolTip"));
			this.btnXITReset.Visible = ((bool)(resources.GetObject("btnXITReset.Visible")));
			this.btnXITReset.Click += new System.EventHandler(this.btnXITReset_Click);
			// 
			// btnIFtoVFO
			// 
			this.btnIFtoVFO.AccessibleDescription = resources.GetString("btnIFtoVFO.AccessibleDescription");
			this.btnIFtoVFO.AccessibleName = resources.GetString("btnIFtoVFO.AccessibleName");
			this.btnIFtoVFO.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnIFtoVFO.Anchor")));
			this.btnIFtoVFO.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnIFtoVFO.BackgroundImage")));
			this.btnIFtoVFO.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnIFtoVFO.Dock")));
			this.btnIFtoVFO.Enabled = ((bool)(resources.GetObject("btnIFtoVFO.Enabled")));
			this.btnIFtoVFO.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnIFtoVFO.FlatStyle")));
			this.btnIFtoVFO.Font = ((System.Drawing.Font)(resources.GetObject("btnIFtoVFO.Font")));
			this.btnIFtoVFO.Image = ((System.Drawing.Image)(resources.GetObject("btnIFtoVFO.Image")));
			this.btnIFtoVFO.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnIFtoVFO.ImageAlign")));
			this.btnIFtoVFO.ImageIndex = ((int)(resources.GetObject("btnIFtoVFO.ImageIndex")));
			this.btnIFtoVFO.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnIFtoVFO.ImeMode")));
			this.btnIFtoVFO.Location = ((System.Drawing.Point)(resources.GetObject("btnIFtoVFO.Location")));
			this.btnIFtoVFO.Name = "btnIFtoVFO";
			this.btnIFtoVFO.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnIFtoVFO.RightToLeft")));
			this.btnIFtoVFO.Size = ((System.Drawing.Size)(resources.GetObject("btnIFtoVFO.Size")));
			this.btnIFtoVFO.TabIndex = ((int)(resources.GetObject("btnIFtoVFO.TabIndex")));
			this.btnIFtoVFO.Text = resources.GetString("btnIFtoVFO.Text");
			this.btnIFtoVFO.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnIFtoVFO.TextAlign")));
			this.toolTip1.SetToolTip(this.btnIFtoVFO, resources.GetString("btnIFtoVFO.ToolTip"));
			this.btnIFtoVFO.Visible = ((bool)(resources.GetObject("btnIFtoVFO.Visible")));
			this.btnIFtoVFO.Click += new System.EventHandler(this.btnIFtoVFO_Click);
			// 
			// btnVFOSwap
			// 
			this.btnVFOSwap.AccessibleDescription = resources.GetString("btnVFOSwap.AccessibleDescription");
			this.btnVFOSwap.AccessibleName = resources.GetString("btnVFOSwap.AccessibleName");
			this.btnVFOSwap.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnVFOSwap.Anchor")));
			this.btnVFOSwap.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnVFOSwap.BackgroundImage")));
			this.btnVFOSwap.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnVFOSwap.Dock")));
			this.btnVFOSwap.Enabled = ((bool)(resources.GetObject("btnVFOSwap.Enabled")));
			this.btnVFOSwap.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnVFOSwap.FlatStyle")));
			this.btnVFOSwap.Font = ((System.Drawing.Font)(resources.GetObject("btnVFOSwap.Font")));
			this.btnVFOSwap.Image = ((System.Drawing.Image)(resources.GetObject("btnVFOSwap.Image")));
			this.btnVFOSwap.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnVFOSwap.ImageAlign")));
			this.btnVFOSwap.ImageIndex = ((int)(resources.GetObject("btnVFOSwap.ImageIndex")));
			this.btnVFOSwap.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnVFOSwap.ImeMode")));
			this.btnVFOSwap.Location = ((System.Drawing.Point)(resources.GetObject("btnVFOSwap.Location")));
			this.btnVFOSwap.Name = "btnVFOSwap";
			this.btnVFOSwap.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnVFOSwap.RightToLeft")));
			this.btnVFOSwap.Size = ((System.Drawing.Size)(resources.GetObject("btnVFOSwap.Size")));
			this.btnVFOSwap.TabIndex = ((int)(resources.GetObject("btnVFOSwap.TabIndex")));
			this.btnVFOSwap.Text = resources.GetString("btnVFOSwap.Text");
			this.btnVFOSwap.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnVFOSwap.TextAlign")));
			this.toolTip1.SetToolTip(this.btnVFOSwap, resources.GetString("btnVFOSwap.ToolTip"));
			this.btnVFOSwap.Visible = ((bool)(resources.GetObject("btnVFOSwap.Visible")));
			this.btnVFOSwap.Click += new System.EventHandler(this.btnVFOSwap_Click);
			// 
			// btnVFOBtoA
			// 
			this.btnVFOBtoA.AccessibleDescription = resources.GetString("btnVFOBtoA.AccessibleDescription");
			this.btnVFOBtoA.AccessibleName = resources.GetString("btnVFOBtoA.AccessibleName");
			this.btnVFOBtoA.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnVFOBtoA.Anchor")));
			this.btnVFOBtoA.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnVFOBtoA.BackgroundImage")));
			this.btnVFOBtoA.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnVFOBtoA.Dock")));
			this.btnVFOBtoA.Enabled = ((bool)(resources.GetObject("btnVFOBtoA.Enabled")));
			this.btnVFOBtoA.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnVFOBtoA.FlatStyle")));
			this.btnVFOBtoA.Font = ((System.Drawing.Font)(resources.GetObject("btnVFOBtoA.Font")));
			this.btnVFOBtoA.Image = ((System.Drawing.Image)(resources.GetObject("btnVFOBtoA.Image")));
			this.btnVFOBtoA.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnVFOBtoA.ImageAlign")));
			this.btnVFOBtoA.ImageIndex = ((int)(resources.GetObject("btnVFOBtoA.ImageIndex")));
			this.btnVFOBtoA.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnVFOBtoA.ImeMode")));
			this.btnVFOBtoA.Location = ((System.Drawing.Point)(resources.GetObject("btnVFOBtoA.Location")));
			this.btnVFOBtoA.Name = "btnVFOBtoA";
			this.btnVFOBtoA.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnVFOBtoA.RightToLeft")));
			this.btnVFOBtoA.Size = ((System.Drawing.Size)(resources.GetObject("btnVFOBtoA.Size")));
			this.btnVFOBtoA.TabIndex = ((int)(resources.GetObject("btnVFOBtoA.TabIndex")));
			this.btnVFOBtoA.Text = resources.GetString("btnVFOBtoA.Text");
			this.btnVFOBtoA.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnVFOBtoA.TextAlign")));
			this.toolTip1.SetToolTip(this.btnVFOBtoA, resources.GetString("btnVFOBtoA.ToolTip"));
			this.btnVFOBtoA.Visible = ((bool)(resources.GetObject("btnVFOBtoA.Visible")));
			this.btnVFOBtoA.Click += new System.EventHandler(this.btnVFOBtoA_Click);
			// 
			// btnVFOAtoB
			// 
			this.btnVFOAtoB.AccessibleDescription = resources.GetString("btnVFOAtoB.AccessibleDescription");
			this.btnVFOAtoB.AccessibleName = resources.GetString("btnVFOAtoB.AccessibleName");
			this.btnVFOAtoB.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnVFOAtoB.Anchor")));
			this.btnVFOAtoB.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnVFOAtoB.BackgroundImage")));
			this.btnVFOAtoB.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnVFOAtoB.Dock")));
			this.btnVFOAtoB.Enabled = ((bool)(resources.GetObject("btnVFOAtoB.Enabled")));
			this.btnVFOAtoB.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnVFOAtoB.FlatStyle")));
			this.btnVFOAtoB.Font = ((System.Drawing.Font)(resources.GetObject("btnVFOAtoB.Font")));
			this.btnVFOAtoB.Image = ((System.Drawing.Image)(resources.GetObject("btnVFOAtoB.Image")));
			this.btnVFOAtoB.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnVFOAtoB.ImageAlign")));
			this.btnVFOAtoB.ImageIndex = ((int)(resources.GetObject("btnVFOAtoB.ImageIndex")));
			this.btnVFOAtoB.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnVFOAtoB.ImeMode")));
			this.btnVFOAtoB.Location = ((System.Drawing.Point)(resources.GetObject("btnVFOAtoB.Location")));
			this.btnVFOAtoB.Name = "btnVFOAtoB";
			this.btnVFOAtoB.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnVFOAtoB.RightToLeft")));
			this.btnVFOAtoB.Size = ((System.Drawing.Size)(resources.GetObject("btnVFOAtoB.Size")));
			this.btnVFOAtoB.TabIndex = ((int)(resources.GetObject("btnVFOAtoB.TabIndex")));
			this.btnVFOAtoB.Text = resources.GetString("btnVFOAtoB.Text");
			this.btnVFOAtoB.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnVFOAtoB.TextAlign")));
			this.toolTip1.SetToolTip(this.btnVFOAtoB, resources.GetString("btnVFOAtoB.ToolTip"));
			this.btnVFOAtoB.Visible = ((bool)(resources.GetObject("btnVFOAtoB.Visible")));
			this.btnVFOAtoB.Click += new System.EventHandler(this.btnVFOAtoB_Click);
			// 
			// chkVFOSplit
			// 
			this.chkVFOSplit.AccessibleDescription = resources.GetString("chkVFOSplit.AccessibleDescription");
			this.chkVFOSplit.AccessibleName = resources.GetString("chkVFOSplit.AccessibleName");
			this.chkVFOSplit.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkVFOSplit.Anchor")));
			this.chkVFOSplit.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkVFOSplit.Appearance")));
			this.chkVFOSplit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkVFOSplit.BackgroundImage")));
			this.chkVFOSplit.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkVFOSplit.CheckAlign")));
			this.chkVFOSplit.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkVFOSplit.Dock")));
			this.chkVFOSplit.Enabled = ((bool)(resources.GetObject("chkVFOSplit.Enabled")));
			this.chkVFOSplit.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkVFOSplit.FlatStyle")));
			this.chkVFOSplit.Font = ((System.Drawing.Font)(resources.GetObject("chkVFOSplit.Font")));
			this.chkVFOSplit.Image = ((System.Drawing.Image)(resources.GetObject("chkVFOSplit.Image")));
			this.chkVFOSplit.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkVFOSplit.ImageAlign")));
			this.chkVFOSplit.ImageIndex = ((int)(resources.GetObject("chkVFOSplit.ImageIndex")));
			this.chkVFOSplit.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkVFOSplit.ImeMode")));
			this.chkVFOSplit.Location = ((System.Drawing.Point)(resources.GetObject("chkVFOSplit.Location")));
			this.chkVFOSplit.Name = "chkVFOSplit";
			this.chkVFOSplit.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkVFOSplit.RightToLeft")));
			this.chkVFOSplit.Size = ((System.Drawing.Size)(resources.GetObject("chkVFOSplit.Size")));
			this.chkVFOSplit.TabIndex = ((int)(resources.GetObject("chkVFOSplit.TabIndex")));
			this.chkVFOSplit.Text = resources.GetString("chkVFOSplit.Text");
			this.chkVFOSplit.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkVFOSplit.TextAlign")));
			this.toolTip1.SetToolTip(this.chkVFOSplit, resources.GetString("chkVFOSplit.ToolTip"));
			this.chkVFOSplit.Visible = ((bool)(resources.GetObject("chkVFOSplit.Visible")));
			this.chkVFOSplit.CheckedChanged += new System.EventHandler(this.chkVFOSplit_CheckedChanged);
			// 
			// grpSoundControls
			// 
			this.grpSoundControls.AccessibleDescription = resources.GetString("grpSoundControls.AccessibleDescription");
			this.grpSoundControls.AccessibleName = resources.GetString("grpSoundControls.AccessibleName");
			this.grpSoundControls.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpSoundControls.Anchor")));
			this.grpSoundControls.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpSoundControls.BackgroundImage")));
			this.grpSoundControls.Controls.Add(this.udSquelch);
			this.grpSoundControls.Controls.Add(this.lblPWR);
			this.grpSoundControls.Controls.Add(this.udPWR);
			this.grpSoundControls.Controls.Add(this.lblAF);
			this.grpSoundControls.Controls.Add(this.udAF);
			this.grpSoundControls.Controls.Add(this.lblMIC);
			this.grpSoundControls.Controls.Add(this.udMIC);
			this.grpSoundControls.Controls.Add(this.comboPreamp);
			this.grpSoundControls.Controls.Add(this.lblPreamp);
			this.grpSoundControls.Controls.Add(this.chkSquelch);
			this.grpSoundControls.Controls.Add(this.lblAGC);
			this.grpSoundControls.Controls.Add(this.comboAGC);
			this.grpSoundControls.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpSoundControls.Dock")));
			this.grpSoundControls.Enabled = ((bool)(resources.GetObject("grpSoundControls.Enabled")));
			this.grpSoundControls.Font = ((System.Drawing.Font)(resources.GetObject("grpSoundControls.Font")));
			this.grpSoundControls.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpSoundControls.ImeMode")));
			this.grpSoundControls.Location = ((System.Drawing.Point)(resources.GetObject("grpSoundControls.Location")));
			this.grpSoundControls.Name = "grpSoundControls";
			this.grpSoundControls.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpSoundControls.RightToLeft")));
			this.grpSoundControls.Size = ((System.Drawing.Size)(resources.GetObject("grpSoundControls.Size")));
			this.grpSoundControls.TabIndex = ((int)(resources.GetObject("grpSoundControls.TabIndex")));
			this.grpSoundControls.TabStop = false;
			this.grpSoundControls.Text = resources.GetString("grpSoundControls.Text");
			this.toolTip1.SetToolTip(this.grpSoundControls, resources.GetString("grpSoundControls.ToolTip"));
			this.grpSoundControls.Visible = ((bool)(resources.GetObject("grpSoundControls.Visible")));
			// 
			// udSquelch
			// 
			this.udSquelch.AccessibleDescription = resources.GetString("udSquelch.AccessibleDescription");
			this.udSquelch.AccessibleName = resources.GetString("udSquelch.AccessibleName");
			this.udSquelch.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("udSquelch.Anchor")));
			this.udSquelch.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("udSquelch.Dock")));
			this.udSquelch.Enabled = ((bool)(resources.GetObject("udSquelch.Enabled")));
			this.udSquelch.Font = ((System.Drawing.Font)(resources.GetObject("udSquelch.Font")));
			this.udSquelch.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("udSquelch.ImeMode")));
			this.udSquelch.Increment = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		0});
			this.udSquelch.Location = ((System.Drawing.Point)(resources.GetObject("udSquelch.Location")));
			this.udSquelch.Maximum = new System.Decimal(new int[] {
																	  160,
																	  0,
																	  0,
																	  0});
			this.udSquelch.Minimum = new System.Decimal(new int[] {
																	  0,
																	  0,
																	  0,
																	  0});
			this.udSquelch.Name = "udSquelch";
			this.udSquelch.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("udSquelch.RightToLeft")));
			this.udSquelch.Size = ((System.Drawing.Size)(resources.GetObject("udSquelch.Size")));
			this.udSquelch.TabIndex = ((int)(resources.GetObject("udSquelch.TabIndex")));
			this.udSquelch.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("udSquelch.TextAlign")));
			this.udSquelch.ThousandsSeparator = ((bool)(resources.GetObject("udSquelch.ThousandsSeparator")));
			this.toolTip1.SetToolTip(this.udSquelch, resources.GetString("udSquelch.ToolTip"));
			this.udSquelch.UpDownAlign = ((System.Windows.Forms.LeftRightAlignment)(resources.GetObject("udSquelch.UpDownAlign")));
			this.udSquelch.Value = new System.Decimal(new int[] {
																	150,
																	0,
																	0,
																	0});
			this.udSquelch.Visible = ((bool)(resources.GetObject("udSquelch.Visible")));
			this.udSquelch.LostFocus += new System.EventHandler(this.udSquelch_LostFocus);
			this.udSquelch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
			this.udSquelch.ValueChanged += new System.EventHandler(this.udSquelch_ValueChanged);
			// 
			// chkSquelch
			// 
			this.chkSquelch.AccessibleDescription = resources.GetString("chkSquelch.AccessibleDescription");
			this.chkSquelch.AccessibleName = resources.GetString("chkSquelch.AccessibleName");
			this.chkSquelch.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkSquelch.Anchor")));
			this.chkSquelch.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkSquelch.Appearance")));
			this.chkSquelch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkSquelch.BackgroundImage")));
			this.chkSquelch.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkSquelch.CheckAlign")));
			this.chkSquelch.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkSquelch.Dock")));
			this.chkSquelch.Enabled = ((bool)(resources.GetObject("chkSquelch.Enabled")));
			this.chkSquelch.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkSquelch.FlatStyle")));
			this.chkSquelch.Font = ((System.Drawing.Font)(resources.GetObject("chkSquelch.Font")));
			this.chkSquelch.Image = ((System.Drawing.Image)(resources.GetObject("chkSquelch.Image")));
			this.chkSquelch.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkSquelch.ImageAlign")));
			this.chkSquelch.ImageIndex = ((int)(resources.GetObject("chkSquelch.ImageIndex")));
			this.chkSquelch.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkSquelch.ImeMode")));
			this.chkSquelch.Location = ((System.Drawing.Point)(resources.GetObject("chkSquelch.Location")));
			this.chkSquelch.Name = "chkSquelch";
			this.chkSquelch.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkSquelch.RightToLeft")));
			this.chkSquelch.Size = ((System.Drawing.Size)(resources.GetObject("chkSquelch.Size")));
			this.chkSquelch.TabIndex = ((int)(resources.GetObject("chkSquelch.TabIndex")));
			this.chkSquelch.Text = resources.GetString("chkSquelch.Text");
			this.chkSquelch.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkSquelch.TextAlign")));
			this.toolTip1.SetToolTip(this.chkSquelch, resources.GetString("chkSquelch.ToolTip"));
			this.chkSquelch.Visible = ((bool)(resources.GetObject("chkSquelch.Visible")));
			this.chkSquelch.CheckedChanged += new System.EventHandler(this.chkSquelch_CheckedChanged);
			// 
			// grpOptions
			// 
			this.grpOptions.AccessibleDescription = resources.GetString("grpOptions.AccessibleDescription");
			this.grpOptions.AccessibleName = resources.GetString("grpOptions.AccessibleName");
			this.grpOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpOptions.Anchor")));
			this.grpOptions.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpOptions.BackgroundImage")));
			this.grpOptions.Controls.Add(this.comboTuneMode);
			this.grpOptions.Controls.Add(this.chkMUT);
			this.grpOptions.Controls.Add(this.chkMON);
			this.grpOptions.Controls.Add(this.chkTUN);
			this.grpOptions.Controls.Add(this.chkMOX);
			this.grpOptions.Controls.Add(this.chkBIN);
			this.grpOptions.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpOptions.Dock")));
			this.grpOptions.Enabled = ((bool)(resources.GetObject("grpOptions.Enabled")));
			this.grpOptions.Font = ((System.Drawing.Font)(resources.GetObject("grpOptions.Font")));
			this.grpOptions.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpOptions.ImeMode")));
			this.grpOptions.Location = ((System.Drawing.Point)(resources.GetObject("grpOptions.Location")));
			this.grpOptions.Name = "grpOptions";
			this.grpOptions.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpOptions.RightToLeft")));
			this.grpOptions.Size = ((System.Drawing.Size)(resources.GetObject("grpOptions.Size")));
			this.grpOptions.TabIndex = ((int)(resources.GetObject("grpOptions.TabIndex")));
			this.grpOptions.TabStop = false;
			this.grpOptions.Text = resources.GetString("grpOptions.Text");
			this.toolTip1.SetToolTip(this.grpOptions, resources.GetString("grpOptions.ToolTip"));
			this.grpOptions.Visible = ((bool)(resources.GetObject("grpOptions.Visible")));
			// 
			// comboTuneMode
			// 
			this.comboTuneMode.AccessibleDescription = resources.GetString("comboTuneMode.AccessibleDescription");
			this.comboTuneMode.AccessibleName = resources.GetString("comboTuneMode.AccessibleName");
			this.comboTuneMode.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("comboTuneMode.Anchor")));
			this.comboTuneMode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("comboTuneMode.BackgroundImage")));
			this.comboTuneMode.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("comboTuneMode.Dock")));
			this.comboTuneMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTuneMode.DropDownWidth = 42;
			this.comboTuneMode.Enabled = ((bool)(resources.GetObject("comboTuneMode.Enabled")));
			this.comboTuneMode.Font = ((System.Drawing.Font)(resources.GetObject("comboTuneMode.Font")));
			this.comboTuneMode.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("comboTuneMode.ImeMode")));
			this.comboTuneMode.IntegralHeight = ((bool)(resources.GetObject("comboTuneMode.IntegralHeight")));
			this.comboTuneMode.ItemHeight = ((int)(resources.GetObject("comboTuneMode.ItemHeight")));
			this.comboTuneMode.Items.AddRange(new object[] {
															   resources.GetString("comboTuneMode.Items"),
															   resources.GetString("comboTuneMode.Items1"),
															   resources.GetString("comboTuneMode.Items2")});
			this.comboTuneMode.Location = ((System.Drawing.Point)(resources.GetObject("comboTuneMode.Location")));
			this.comboTuneMode.MaxDropDownItems = ((int)(resources.GetObject("comboTuneMode.MaxDropDownItems")));
			this.comboTuneMode.MaxLength = ((int)(resources.GetObject("comboTuneMode.MaxLength")));
			this.comboTuneMode.Name = "comboTuneMode";
			this.comboTuneMode.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("comboTuneMode.RightToLeft")));
			this.comboTuneMode.Size = ((System.Drawing.Size)(resources.GetObject("comboTuneMode.Size")));
			this.comboTuneMode.TabIndex = ((int)(resources.GetObject("comboTuneMode.TabIndex")));
			this.comboTuneMode.Text = resources.GetString("comboTuneMode.Text");
			this.toolTip1.SetToolTip(this.comboTuneMode, resources.GetString("comboTuneMode.ToolTip"));
			this.comboTuneMode.Visible = ((bool)(resources.GetObject("comboTuneMode.Visible")));
			this.comboTuneMode.SelectedIndexChanged += new System.EventHandler(this.comboTuneMode_SelectedIndexChanged);
			// 
			// grpDisplay2
			// 
			this.grpDisplay2.AccessibleDescription = resources.GetString("grpDisplay2.AccessibleDescription");
			this.grpDisplay2.AccessibleName = resources.GetString("grpDisplay2.AccessibleName");
			this.grpDisplay2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpDisplay2.Anchor")));
			this.grpDisplay2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpDisplay2.BackgroundImage")));
			this.grpDisplay2.Controls.Add(this.chkDisplayPeak);
			this.grpDisplay2.Controls.Add(this.comboDisplayMode);
			this.grpDisplay2.Controls.Add(this.chkDisplayAVG);
			this.grpDisplay2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpDisplay2.Dock")));
			this.grpDisplay2.Enabled = ((bool)(resources.GetObject("grpDisplay2.Enabled")));
			this.grpDisplay2.Font = ((System.Drawing.Font)(resources.GetObject("grpDisplay2.Font")));
			this.grpDisplay2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpDisplay2.ImeMode")));
			this.grpDisplay2.Location = ((System.Drawing.Point)(resources.GetObject("grpDisplay2.Location")));
			this.grpDisplay2.Name = "grpDisplay2";
			this.grpDisplay2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpDisplay2.RightToLeft")));
			this.grpDisplay2.Size = ((System.Drawing.Size)(resources.GetObject("grpDisplay2.Size")));
			this.grpDisplay2.TabIndex = ((int)(resources.GetObject("grpDisplay2.TabIndex")));
			this.grpDisplay2.TabStop = false;
			this.grpDisplay2.Text = resources.GetString("grpDisplay2.Text");
			this.toolTip1.SetToolTip(this.grpDisplay2, resources.GetString("grpDisplay2.ToolTip"));
			this.grpDisplay2.Visible = ((bool)(resources.GetObject("grpDisplay2.Visible")));
			// 
			// chkDisplayPeak
			// 
			this.chkDisplayPeak.AccessibleDescription = resources.GetString("chkDisplayPeak.AccessibleDescription");
			this.chkDisplayPeak.AccessibleName = resources.GetString("chkDisplayPeak.AccessibleName");
			this.chkDisplayPeak.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkDisplayPeak.Anchor")));
			this.chkDisplayPeak.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkDisplayPeak.Appearance")));
			this.chkDisplayPeak.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkDisplayPeak.BackgroundImage")));
			this.chkDisplayPeak.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDisplayPeak.CheckAlign")));
			this.chkDisplayPeak.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkDisplayPeak.Dock")));
			this.chkDisplayPeak.Enabled = ((bool)(resources.GetObject("chkDisplayPeak.Enabled")));
			this.chkDisplayPeak.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkDisplayPeak.FlatStyle")));
			this.chkDisplayPeak.Font = ((System.Drawing.Font)(resources.GetObject("chkDisplayPeak.Font")));
			this.chkDisplayPeak.Image = ((System.Drawing.Image)(resources.GetObject("chkDisplayPeak.Image")));
			this.chkDisplayPeak.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDisplayPeak.ImageAlign")));
			this.chkDisplayPeak.ImageIndex = ((int)(resources.GetObject("chkDisplayPeak.ImageIndex")));
			this.chkDisplayPeak.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkDisplayPeak.ImeMode")));
			this.chkDisplayPeak.Location = ((System.Drawing.Point)(resources.GetObject("chkDisplayPeak.Location")));
			this.chkDisplayPeak.Name = "chkDisplayPeak";
			this.chkDisplayPeak.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkDisplayPeak.RightToLeft")));
			this.chkDisplayPeak.Size = ((System.Drawing.Size)(resources.GetObject("chkDisplayPeak.Size")));
			this.chkDisplayPeak.TabIndex = ((int)(resources.GetObject("chkDisplayPeak.TabIndex")));
			this.chkDisplayPeak.Text = resources.GetString("chkDisplayPeak.Text");
			this.chkDisplayPeak.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDisplayPeak.TextAlign")));
			this.toolTip1.SetToolTip(this.chkDisplayPeak, resources.GetString("chkDisplayPeak.ToolTip"));
			this.chkDisplayPeak.Visible = ((bool)(resources.GetObject("chkDisplayPeak.Visible")));
			this.chkDisplayPeak.CheckedChanged += new System.EventHandler(this.chkDisplayPeak_CheckedChanged);
			// 
			// chkDisplayAVG
			// 
			this.chkDisplayAVG.AccessibleDescription = resources.GetString("chkDisplayAVG.AccessibleDescription");
			this.chkDisplayAVG.AccessibleName = resources.GetString("chkDisplayAVG.AccessibleName");
			this.chkDisplayAVG.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkDisplayAVG.Anchor")));
			this.chkDisplayAVG.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkDisplayAVG.Appearance")));
			this.chkDisplayAVG.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkDisplayAVG.BackgroundImage")));
			this.chkDisplayAVG.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDisplayAVG.CheckAlign")));
			this.chkDisplayAVG.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkDisplayAVG.Dock")));
			this.chkDisplayAVG.Enabled = ((bool)(resources.GetObject("chkDisplayAVG.Enabled")));
			this.chkDisplayAVG.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkDisplayAVG.FlatStyle")));
			this.chkDisplayAVG.Font = ((System.Drawing.Font)(resources.GetObject("chkDisplayAVG.Font")));
			this.chkDisplayAVG.Image = ((System.Drawing.Image)(resources.GetObject("chkDisplayAVG.Image")));
			this.chkDisplayAVG.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDisplayAVG.ImageAlign")));
			this.chkDisplayAVG.ImageIndex = ((int)(resources.GetObject("chkDisplayAVG.ImageIndex")));
			this.chkDisplayAVG.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkDisplayAVG.ImeMode")));
			this.chkDisplayAVG.Location = ((System.Drawing.Point)(resources.GetObject("chkDisplayAVG.Location")));
			this.chkDisplayAVG.Name = "chkDisplayAVG";
			this.chkDisplayAVG.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkDisplayAVG.RightToLeft")));
			this.chkDisplayAVG.Size = ((System.Drawing.Size)(resources.GetObject("chkDisplayAVG.Size")));
			this.chkDisplayAVG.TabIndex = ((int)(resources.GetObject("chkDisplayAVG.TabIndex")));
			this.chkDisplayAVG.Text = resources.GetString("chkDisplayAVG.Text");
			this.chkDisplayAVG.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkDisplayAVG.TextAlign")));
			this.toolTip1.SetToolTip(this.chkDisplayAVG, resources.GetString("chkDisplayAVG.ToolTip"));
			this.chkDisplayAVG.Visible = ((bool)(resources.GetObject("chkDisplayAVG.Visible")));
			this.chkDisplayAVG.CheckedChanged += new System.EventHandler(this.chkDisplayAVG_CheckedChanged);
			// 
			// grpMemory
			// 
			this.grpMemory.AccessibleDescription = resources.GetString("grpMemory.AccessibleDescription");
			this.grpMemory.AccessibleName = resources.GetString("grpMemory.AccessibleName");
			this.grpMemory.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpMemory.Anchor")));
			this.grpMemory.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpMemory.BackgroundImage")));
			this.grpMemory.Controls.Add(this.btnMemoryQuickRestore);
			this.grpMemory.Controls.Add(this.btnMemoryQuickSave);
			this.grpMemory.Controls.Add(this.txtMemoryQuick);
			this.grpMemory.Controls.Add(this.labelScannerDelay);
			this.grpMemory.Controls.Add(this.udScannerDelay);
			this.grpMemory.Controls.Add(this.lblMemoryStep);
			this.grpMemory.Controls.Add(this.lblMemoryHigh);
			this.grpMemory.Controls.Add(this.lblMemoryLow);
			this.grpMemory.Controls.Add(this.udScannerStep);
			this.grpMemory.Controls.Add(this.udScannerHigh);
			this.grpMemory.Controls.Add(this.udScannerLow);
			this.grpMemory.Controls.Add(this.chkMemoryScanner);
			this.grpMemory.Controls.Add(this.radScannerFreq);
			this.grpMemory.Controls.Add(this.radScannerChannel);
			this.grpMemory.Controls.Add(this.btnSave);
			this.grpMemory.Controls.Add(this.btnMemoryRecall);
			this.grpMemory.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpMemory.Dock")));
			this.grpMemory.Enabled = ((bool)(resources.GetObject("grpMemory.Enabled")));
			this.grpMemory.Font = ((System.Drawing.Font)(resources.GetObject("grpMemory.Font")));
			this.grpMemory.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpMemory.ImeMode")));
			this.grpMemory.Location = ((System.Drawing.Point)(resources.GetObject("grpMemory.Location")));
			this.grpMemory.Name = "grpMemory";
			this.grpMemory.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpMemory.RightToLeft")));
			this.grpMemory.Size = ((System.Drawing.Size)(resources.GetObject("grpMemory.Size")));
			this.grpMemory.TabIndex = ((int)(resources.GetObject("grpMemory.TabIndex")));
			this.grpMemory.TabStop = false;
			this.grpMemory.Text = resources.GetString("grpMemory.Text");
			this.toolTip1.SetToolTip(this.grpMemory, resources.GetString("grpMemory.ToolTip"));
			this.grpMemory.Visible = ((bool)(resources.GetObject("grpMemory.Visible")));
			// 
			// btnMemoryQuickRestore
			// 
			this.btnMemoryQuickRestore.AccessibleDescription = resources.GetString("btnMemoryQuickRestore.AccessibleDescription");
			this.btnMemoryQuickRestore.AccessibleName = resources.GetString("btnMemoryQuickRestore.AccessibleName");
			this.btnMemoryQuickRestore.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnMemoryQuickRestore.Anchor")));
			this.btnMemoryQuickRestore.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMemoryQuickRestore.BackgroundImage")));
			this.btnMemoryQuickRestore.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnMemoryQuickRestore.Dock")));
			this.btnMemoryQuickRestore.Enabled = ((bool)(resources.GetObject("btnMemoryQuickRestore.Enabled")));
			this.btnMemoryQuickRestore.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnMemoryQuickRestore.FlatStyle")));
			this.btnMemoryQuickRestore.Font = ((System.Drawing.Font)(resources.GetObject("btnMemoryQuickRestore.Font")));
			this.btnMemoryQuickRestore.Image = ((System.Drawing.Image)(resources.GetObject("btnMemoryQuickRestore.Image")));
			this.btnMemoryQuickRestore.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnMemoryQuickRestore.ImageAlign")));
			this.btnMemoryQuickRestore.ImageIndex = ((int)(resources.GetObject("btnMemoryQuickRestore.ImageIndex")));
			this.btnMemoryQuickRestore.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnMemoryQuickRestore.ImeMode")));
			this.btnMemoryQuickRestore.Location = ((System.Drawing.Point)(resources.GetObject("btnMemoryQuickRestore.Location")));
			this.btnMemoryQuickRestore.Name = "btnMemoryQuickRestore";
			this.btnMemoryQuickRestore.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnMemoryQuickRestore.RightToLeft")));
			this.btnMemoryQuickRestore.Size = ((System.Drawing.Size)(resources.GetObject("btnMemoryQuickRestore.Size")));
			this.btnMemoryQuickRestore.TabIndex = ((int)(resources.GetObject("btnMemoryQuickRestore.TabIndex")));
			this.btnMemoryQuickRestore.Text = resources.GetString("btnMemoryQuickRestore.Text");
			this.btnMemoryQuickRestore.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnMemoryQuickRestore.TextAlign")));
			this.toolTip1.SetToolTip(this.btnMemoryQuickRestore, resources.GetString("btnMemoryQuickRestore.ToolTip"));
			this.btnMemoryQuickRestore.Visible = ((bool)(resources.GetObject("btnMemoryQuickRestore.Visible")));
			this.btnMemoryQuickRestore.Click += new System.EventHandler(this.btnMemoryQuickRestore_Click);
			// 
			// btnMemoryQuickSave
			// 
			this.btnMemoryQuickSave.AccessibleDescription = resources.GetString("btnMemoryQuickSave.AccessibleDescription");
			this.btnMemoryQuickSave.AccessibleName = resources.GetString("btnMemoryQuickSave.AccessibleName");
			this.btnMemoryQuickSave.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnMemoryQuickSave.Anchor")));
			this.btnMemoryQuickSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMemoryQuickSave.BackgroundImage")));
			this.btnMemoryQuickSave.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnMemoryQuickSave.Dock")));
			this.btnMemoryQuickSave.Enabled = ((bool)(resources.GetObject("btnMemoryQuickSave.Enabled")));
			this.btnMemoryQuickSave.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnMemoryQuickSave.FlatStyle")));
			this.btnMemoryQuickSave.Font = ((System.Drawing.Font)(resources.GetObject("btnMemoryQuickSave.Font")));
			this.btnMemoryQuickSave.Image = ((System.Drawing.Image)(resources.GetObject("btnMemoryQuickSave.Image")));
			this.btnMemoryQuickSave.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnMemoryQuickSave.ImageAlign")));
			this.btnMemoryQuickSave.ImageIndex = ((int)(resources.GetObject("btnMemoryQuickSave.ImageIndex")));
			this.btnMemoryQuickSave.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnMemoryQuickSave.ImeMode")));
			this.btnMemoryQuickSave.Location = ((System.Drawing.Point)(resources.GetObject("btnMemoryQuickSave.Location")));
			this.btnMemoryQuickSave.Name = "btnMemoryQuickSave";
			this.btnMemoryQuickSave.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnMemoryQuickSave.RightToLeft")));
			this.btnMemoryQuickSave.Size = ((System.Drawing.Size)(resources.GetObject("btnMemoryQuickSave.Size")));
			this.btnMemoryQuickSave.TabIndex = ((int)(resources.GetObject("btnMemoryQuickSave.TabIndex")));
			this.btnMemoryQuickSave.Text = resources.GetString("btnMemoryQuickSave.Text");
			this.btnMemoryQuickSave.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnMemoryQuickSave.TextAlign")));
			this.toolTip1.SetToolTip(this.btnMemoryQuickSave, resources.GetString("btnMemoryQuickSave.ToolTip"));
			this.btnMemoryQuickSave.Visible = ((bool)(resources.GetObject("btnMemoryQuickSave.Visible")));
			this.btnMemoryQuickSave.Click += new System.EventHandler(this.btnMemoryQuickSave_Click);
			// 
			// txtMemoryQuick
			// 
			this.txtMemoryQuick.AccessibleDescription = resources.GetString("txtMemoryQuick.AccessibleDescription");
			this.txtMemoryQuick.AccessibleName = resources.GetString("txtMemoryQuick.AccessibleName");
			this.txtMemoryQuick.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtMemoryQuick.Anchor")));
			this.txtMemoryQuick.AutoSize = ((bool)(resources.GetObject("txtMemoryQuick.AutoSize")));
			this.txtMemoryQuick.BackColor = System.Drawing.Color.White;
			this.txtMemoryQuick.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtMemoryQuick.BackgroundImage")));
			this.txtMemoryQuick.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtMemoryQuick.Dock")));
			this.txtMemoryQuick.Enabled = ((bool)(resources.GetObject("txtMemoryQuick.Enabled")));
			this.txtMemoryQuick.Font = ((System.Drawing.Font)(resources.GetObject("txtMemoryQuick.Font")));
			this.txtMemoryQuick.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtMemoryQuick.ImeMode")));
			this.txtMemoryQuick.Location = ((System.Drawing.Point)(resources.GetObject("txtMemoryQuick.Location")));
			this.txtMemoryQuick.MaxLength = ((int)(resources.GetObject("txtMemoryQuick.MaxLength")));
			this.txtMemoryQuick.Multiline = ((bool)(resources.GetObject("txtMemoryQuick.Multiline")));
			this.txtMemoryQuick.Name = "txtMemoryQuick";
			this.txtMemoryQuick.PasswordChar = ((char)(resources.GetObject("txtMemoryQuick.PasswordChar")));
			this.txtMemoryQuick.ReadOnly = true;
			this.txtMemoryQuick.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtMemoryQuick.RightToLeft")));
			this.txtMemoryQuick.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtMemoryQuick.ScrollBars")));
			this.txtMemoryQuick.Size = ((System.Drawing.Size)(resources.GetObject("txtMemoryQuick.Size")));
			this.txtMemoryQuick.TabIndex = ((int)(resources.GetObject("txtMemoryQuick.TabIndex")));
			this.txtMemoryQuick.Text = resources.GetString("txtMemoryQuick.Text");
			this.txtMemoryQuick.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtMemoryQuick.TextAlign")));
			this.toolTip1.SetToolTip(this.txtMemoryQuick, resources.GetString("txtMemoryQuick.ToolTip"));
			this.txtMemoryQuick.Visible = ((bool)(resources.GetObject("txtMemoryQuick.Visible")));
			this.txtMemoryQuick.WordWrap = ((bool)(resources.GetObject("txtMemoryQuick.WordWrap")));
			// 
			// labelScannerDelay
			// 
			this.labelScannerDelay.AccessibleDescription = resources.GetString("labelScannerDelay.AccessibleDescription");
			this.labelScannerDelay.AccessibleName = resources.GetString("labelScannerDelay.AccessibleName");
			this.labelScannerDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("labelScannerDelay.Anchor")));
			this.labelScannerDelay.AutoSize = ((bool)(resources.GetObject("labelScannerDelay.AutoSize")));
			this.labelScannerDelay.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("labelScannerDelay.Dock")));
			this.labelScannerDelay.Enabled = ((bool)(resources.GetObject("labelScannerDelay.Enabled")));
			this.labelScannerDelay.Font = ((System.Drawing.Font)(resources.GetObject("labelScannerDelay.Font")));
			this.labelScannerDelay.Image = ((System.Drawing.Image)(resources.GetObject("labelScannerDelay.Image")));
			this.labelScannerDelay.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("labelScannerDelay.ImageAlign")));
			this.labelScannerDelay.ImageIndex = ((int)(resources.GetObject("labelScannerDelay.ImageIndex")));
			this.labelScannerDelay.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("labelScannerDelay.ImeMode")));
			this.labelScannerDelay.Location = ((System.Drawing.Point)(resources.GetObject("labelScannerDelay.Location")));
			this.labelScannerDelay.Name = "labelScannerDelay";
			this.labelScannerDelay.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("labelScannerDelay.RightToLeft")));
			this.labelScannerDelay.Size = ((System.Drawing.Size)(resources.GetObject("labelScannerDelay.Size")));
			this.labelScannerDelay.TabIndex = ((int)(resources.GetObject("labelScannerDelay.TabIndex")));
			this.labelScannerDelay.Text = resources.GetString("labelScannerDelay.Text");
			this.labelScannerDelay.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("labelScannerDelay.TextAlign")));
			this.toolTip1.SetToolTip(this.labelScannerDelay, resources.GetString("labelScannerDelay.ToolTip"));
			this.labelScannerDelay.Visible = ((bool)(resources.GetObject("labelScannerDelay.Visible")));
			// 
			// udScannerDelay
			// 
			this.udScannerDelay.AccessibleDescription = resources.GetString("udScannerDelay.AccessibleDescription");
			this.udScannerDelay.AccessibleName = resources.GetString("udScannerDelay.AccessibleName");
			this.udScannerDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("udScannerDelay.Anchor")));
			this.udScannerDelay.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("udScannerDelay.Dock")));
			this.udScannerDelay.Enabled = ((bool)(resources.GetObject("udScannerDelay.Enabled")));
			this.udScannerDelay.Font = ((System.Drawing.Font)(resources.GetObject("udScannerDelay.Font")));
			this.udScannerDelay.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("udScannerDelay.ImeMode")));
			this.udScannerDelay.Increment = new System.Decimal(new int[] {
																			 100,
																			 0,
																			 0,
																			 0});
			this.udScannerDelay.Location = ((System.Drawing.Point)(resources.GetObject("udScannerDelay.Location")));
			this.udScannerDelay.Maximum = new System.Decimal(new int[] {
																		   60000,
																		   0,
																		   0,
																		   0});
			this.udScannerDelay.Minimum = new System.Decimal(new int[] {
																		   50,
																		   0,
																		   0,
																		   0});
			this.udScannerDelay.Name = "udScannerDelay";
			this.udScannerDelay.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("udScannerDelay.RightToLeft")));
			this.udScannerDelay.Size = ((System.Drawing.Size)(resources.GetObject("udScannerDelay.Size")));
			this.udScannerDelay.TabIndex = ((int)(resources.GetObject("udScannerDelay.TabIndex")));
			this.udScannerDelay.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("udScannerDelay.TextAlign")));
			this.udScannerDelay.ThousandsSeparator = ((bool)(resources.GetObject("udScannerDelay.ThousandsSeparator")));
			this.toolTip1.SetToolTip(this.udScannerDelay, resources.GetString("udScannerDelay.ToolTip"));
			this.udScannerDelay.UpDownAlign = ((System.Windows.Forms.LeftRightAlignment)(resources.GetObject("udScannerDelay.UpDownAlign")));
			this.udScannerDelay.Value = new System.Decimal(new int[] {
																		 3000,
																		 0,
																		 0,
																		 0});
			this.udScannerDelay.Visible = ((bool)(resources.GetObject("udScannerDelay.Visible")));
			this.udScannerDelay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
			this.udScannerDelay.ValueChanged += new System.EventHandler(this.udScannerDelay_ValueChanged);
			// 
			// lblMemoryStep
			// 
			this.lblMemoryStep.AccessibleDescription = resources.GetString("lblMemoryStep.AccessibleDescription");
			this.lblMemoryStep.AccessibleName = resources.GetString("lblMemoryStep.AccessibleName");
			this.lblMemoryStep.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblMemoryStep.Anchor")));
			this.lblMemoryStep.AutoSize = ((bool)(resources.GetObject("lblMemoryStep.AutoSize")));
			this.lblMemoryStep.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblMemoryStep.Dock")));
			this.lblMemoryStep.Enabled = ((bool)(resources.GetObject("lblMemoryStep.Enabled")));
			this.lblMemoryStep.Font = ((System.Drawing.Font)(resources.GetObject("lblMemoryStep.Font")));
			this.lblMemoryStep.Image = ((System.Drawing.Image)(resources.GetObject("lblMemoryStep.Image")));
			this.lblMemoryStep.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblMemoryStep.ImageAlign")));
			this.lblMemoryStep.ImageIndex = ((int)(resources.GetObject("lblMemoryStep.ImageIndex")));
			this.lblMemoryStep.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblMemoryStep.ImeMode")));
			this.lblMemoryStep.Location = ((System.Drawing.Point)(resources.GetObject("lblMemoryStep.Location")));
			this.lblMemoryStep.Name = "lblMemoryStep";
			this.lblMemoryStep.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblMemoryStep.RightToLeft")));
			this.lblMemoryStep.Size = ((System.Drawing.Size)(resources.GetObject("lblMemoryStep.Size")));
			this.lblMemoryStep.TabIndex = ((int)(resources.GetObject("lblMemoryStep.TabIndex")));
			this.lblMemoryStep.Text = resources.GetString("lblMemoryStep.Text");
			this.lblMemoryStep.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblMemoryStep.TextAlign")));
			this.toolTip1.SetToolTip(this.lblMemoryStep, resources.GetString("lblMemoryStep.ToolTip"));
			this.lblMemoryStep.Visible = ((bool)(resources.GetObject("lblMemoryStep.Visible")));
			// 
			// lblMemoryHigh
			// 
			this.lblMemoryHigh.AccessibleDescription = resources.GetString("lblMemoryHigh.AccessibleDescription");
			this.lblMemoryHigh.AccessibleName = resources.GetString("lblMemoryHigh.AccessibleName");
			this.lblMemoryHigh.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblMemoryHigh.Anchor")));
			this.lblMemoryHigh.AutoSize = ((bool)(resources.GetObject("lblMemoryHigh.AutoSize")));
			this.lblMemoryHigh.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblMemoryHigh.Dock")));
			this.lblMemoryHigh.Enabled = ((bool)(resources.GetObject("lblMemoryHigh.Enabled")));
			this.lblMemoryHigh.Font = ((System.Drawing.Font)(resources.GetObject("lblMemoryHigh.Font")));
			this.lblMemoryHigh.Image = ((System.Drawing.Image)(resources.GetObject("lblMemoryHigh.Image")));
			this.lblMemoryHigh.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblMemoryHigh.ImageAlign")));
			this.lblMemoryHigh.ImageIndex = ((int)(resources.GetObject("lblMemoryHigh.ImageIndex")));
			this.lblMemoryHigh.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblMemoryHigh.ImeMode")));
			this.lblMemoryHigh.Location = ((System.Drawing.Point)(resources.GetObject("lblMemoryHigh.Location")));
			this.lblMemoryHigh.Name = "lblMemoryHigh";
			this.lblMemoryHigh.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblMemoryHigh.RightToLeft")));
			this.lblMemoryHigh.Size = ((System.Drawing.Size)(resources.GetObject("lblMemoryHigh.Size")));
			this.lblMemoryHigh.TabIndex = ((int)(resources.GetObject("lblMemoryHigh.TabIndex")));
			this.lblMemoryHigh.Text = resources.GetString("lblMemoryHigh.Text");
			this.lblMemoryHigh.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblMemoryHigh.TextAlign")));
			this.toolTip1.SetToolTip(this.lblMemoryHigh, resources.GetString("lblMemoryHigh.ToolTip"));
			this.lblMemoryHigh.Visible = ((bool)(resources.GetObject("lblMemoryHigh.Visible")));
			// 
			// lblMemoryLow
			// 
			this.lblMemoryLow.AccessibleDescription = resources.GetString("lblMemoryLow.AccessibleDescription");
			this.lblMemoryLow.AccessibleName = resources.GetString("lblMemoryLow.AccessibleName");
			this.lblMemoryLow.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblMemoryLow.Anchor")));
			this.lblMemoryLow.AutoSize = ((bool)(resources.GetObject("lblMemoryLow.AutoSize")));
			this.lblMemoryLow.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblMemoryLow.Dock")));
			this.lblMemoryLow.Enabled = ((bool)(resources.GetObject("lblMemoryLow.Enabled")));
			this.lblMemoryLow.Font = ((System.Drawing.Font)(resources.GetObject("lblMemoryLow.Font")));
			this.lblMemoryLow.Image = ((System.Drawing.Image)(resources.GetObject("lblMemoryLow.Image")));
			this.lblMemoryLow.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblMemoryLow.ImageAlign")));
			this.lblMemoryLow.ImageIndex = ((int)(resources.GetObject("lblMemoryLow.ImageIndex")));
			this.lblMemoryLow.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblMemoryLow.ImeMode")));
			this.lblMemoryLow.Location = ((System.Drawing.Point)(resources.GetObject("lblMemoryLow.Location")));
			this.lblMemoryLow.Name = "lblMemoryLow";
			this.lblMemoryLow.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblMemoryLow.RightToLeft")));
			this.lblMemoryLow.Size = ((System.Drawing.Size)(resources.GetObject("lblMemoryLow.Size")));
			this.lblMemoryLow.TabIndex = ((int)(resources.GetObject("lblMemoryLow.TabIndex")));
			this.lblMemoryLow.Text = resources.GetString("lblMemoryLow.Text");
			this.lblMemoryLow.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblMemoryLow.TextAlign")));
			this.toolTip1.SetToolTip(this.lblMemoryLow, resources.GetString("lblMemoryLow.ToolTip"));
			this.lblMemoryLow.Visible = ((bool)(resources.GetObject("lblMemoryLow.Visible")));
			// 
			// udScannerStep
			// 
			this.udScannerStep.AccessibleDescription = resources.GetString("udScannerStep.AccessibleDescription");
			this.udScannerStep.AccessibleName = resources.GetString("udScannerStep.AccessibleName");
			this.udScannerStep.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("udScannerStep.Anchor")));
			this.udScannerStep.DecimalPlaces = 6;
			this.udScannerStep.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("udScannerStep.Dock")));
			this.udScannerStep.Enabled = ((bool)(resources.GetObject("udScannerStep.Enabled")));
			this.udScannerStep.Font = ((System.Drawing.Font)(resources.GetObject("udScannerStep.Font")));
			this.udScannerStep.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("udScannerStep.ImeMode")));
			this.udScannerStep.Increment = new System.Decimal(new int[] {
																			100,
																			0,
																			0,
																			393216});
			this.udScannerStep.Location = ((System.Drawing.Point)(resources.GetObject("udScannerStep.Location")));
			this.udScannerStep.Maximum = new System.Decimal(new int[] {
																		  100,
																		  0,
																		  0,
																		  0});
			this.udScannerStep.Minimum = new System.Decimal(new int[] {
																		  0,
																		  0,
																		  0,
																		  0});
			this.udScannerStep.Name = "udScannerStep";
			this.udScannerStep.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("udScannerStep.RightToLeft")));
			this.udScannerStep.Size = ((System.Drawing.Size)(resources.GetObject("udScannerStep.Size")));
			this.udScannerStep.TabIndex = ((int)(resources.GetObject("udScannerStep.TabIndex")));
			this.udScannerStep.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("udScannerStep.TextAlign")));
			this.udScannerStep.ThousandsSeparator = ((bool)(resources.GetObject("udScannerStep.ThousandsSeparator")));
			this.toolTip1.SetToolTip(this.udScannerStep, resources.GetString("udScannerStep.ToolTip"));
			this.udScannerStep.UpDownAlign = ((System.Windows.Forms.LeftRightAlignment)(resources.GetObject("udScannerStep.UpDownAlign")));
			this.udScannerStep.Value = new System.Decimal(new int[] {
																		1000,
																		0,
																		0,
																		393216});
			this.udScannerStep.Visible = ((bool)(resources.GetObject("udScannerStep.Visible")));
			this.udScannerStep.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
			this.udScannerStep.ValueChanged += new System.EventHandler(this.udScannerStep_ValueChanged);
			// 
			// udScannerHigh
			// 
			this.udScannerHigh.AccessibleDescription = resources.GetString("udScannerHigh.AccessibleDescription");
			this.udScannerHigh.AccessibleName = resources.GetString("udScannerHigh.AccessibleName");
			this.udScannerHigh.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("udScannerHigh.Anchor")));
			this.udScannerHigh.DecimalPlaces = 6;
			this.udScannerHigh.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("udScannerHigh.Dock")));
			this.udScannerHigh.Enabled = ((bool)(resources.GetObject("udScannerHigh.Enabled")));
			this.udScannerHigh.Font = ((System.Drawing.Font)(resources.GetObject("udScannerHigh.Font")));
			this.udScannerHigh.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("udScannerHigh.ImeMode")));
			this.udScannerHigh.Increment = new System.Decimal(new int[] {
																			100,
																			0,
																			0,
																			327680});
			this.udScannerHigh.Location = ((System.Drawing.Point)(resources.GetObject("udScannerHigh.Location")));
			this.udScannerHigh.Maximum = new System.Decimal(new int[] {
																		  100,
																		  0,
																		  0,
																		  0});
			this.udScannerHigh.Minimum = new System.Decimal(new int[] {
																		  0,
																		  0,
																		  0,
																		  0});
			this.udScannerHigh.Name = "udScannerHigh";
			this.udScannerHigh.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("udScannerHigh.RightToLeft")));
			this.udScannerHigh.Size = ((System.Drawing.Size)(resources.GetObject("udScannerHigh.Size")));
			this.udScannerHigh.TabIndex = ((int)(resources.GetObject("udScannerHigh.TabIndex")));
			this.udScannerHigh.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("udScannerHigh.TextAlign")));
			this.udScannerHigh.ThousandsSeparator = ((bool)(resources.GetObject("udScannerHigh.ThousandsSeparator")));
			this.toolTip1.SetToolTip(this.udScannerHigh, resources.GetString("udScannerHigh.ToolTip"));
			this.udScannerHigh.UpDownAlign = ((System.Windows.Forms.LeftRightAlignment)(resources.GetObject("udScannerHigh.UpDownAlign")));
			this.udScannerHigh.Value = new System.Decimal(new int[] {
																		7220000,
																		0,
																		0,
																		393216});
			this.udScannerHigh.Visible = ((bool)(resources.GetObject("udScannerHigh.Visible")));
			this.udScannerHigh.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
			this.udScannerHigh.ValueChanged += new System.EventHandler(this.udScannerHigh_ValueChanged);
			// 
			// udScannerLow
			// 
			this.udScannerLow.AccessibleDescription = resources.GetString("udScannerLow.AccessibleDescription");
			this.udScannerLow.AccessibleName = resources.GetString("udScannerLow.AccessibleName");
			this.udScannerLow.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("udScannerLow.Anchor")));
			this.udScannerLow.DecimalPlaces = 6;
			this.udScannerLow.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("udScannerLow.Dock")));
			this.udScannerLow.Enabled = ((bool)(resources.GetObject("udScannerLow.Enabled")));
			this.udScannerLow.Font = ((System.Drawing.Font)(resources.GetObject("udScannerLow.Font")));
			this.udScannerLow.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("udScannerLow.ImeMode")));
			this.udScannerLow.Increment = new System.Decimal(new int[] {
																		   100,
																		   0,
																		   0,
																		   327680});
			this.udScannerLow.Location = ((System.Drawing.Point)(resources.GetObject("udScannerLow.Location")));
			this.udScannerLow.Maximum = new System.Decimal(new int[] {
																		 100,
																		 0,
																		 0,
																		 0});
			this.udScannerLow.Minimum = new System.Decimal(new int[] {
																		 0,
																		 0,
																		 0,
																		 0});
			this.udScannerLow.Name = "udScannerLow";
			this.udScannerLow.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("udScannerLow.RightToLeft")));
			this.udScannerLow.Size = ((System.Drawing.Size)(resources.GetObject("udScannerLow.Size")));
			this.udScannerLow.TabIndex = ((int)(resources.GetObject("udScannerLow.TabIndex")));
			this.udScannerLow.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("udScannerLow.TextAlign")));
			this.udScannerLow.ThousandsSeparator = ((bool)(resources.GetObject("udScannerLow.ThousandsSeparator")));
			this.toolTip1.SetToolTip(this.udScannerLow, resources.GetString("udScannerLow.ToolTip"));
			this.udScannerLow.UpDownAlign = ((System.Windows.Forms.LeftRightAlignment)(resources.GetObject("udScannerLow.UpDownAlign")));
			this.udScannerLow.Value = new System.Decimal(new int[] {
																	   7200000,
																	   0,
																	   0,
																	   393216});
			this.udScannerLow.Visible = ((bool)(resources.GetObject("udScannerLow.Visible")));
			this.udScannerLow.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
			this.udScannerLow.ValueChanged += new System.EventHandler(this.udScannerLow_ValueChanged);
			// 
			// chkMemoryScanner
			// 
			this.chkMemoryScanner.AccessibleDescription = resources.GetString("chkMemoryScanner.AccessibleDescription");
			this.chkMemoryScanner.AccessibleName = resources.GetString("chkMemoryScanner.AccessibleName");
			this.chkMemoryScanner.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkMemoryScanner.Anchor")));
			this.chkMemoryScanner.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkMemoryScanner.Appearance")));
			this.chkMemoryScanner.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkMemoryScanner.BackgroundImage")));
			this.chkMemoryScanner.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkMemoryScanner.CheckAlign")));
			this.chkMemoryScanner.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkMemoryScanner.Dock")));
			this.chkMemoryScanner.Enabled = ((bool)(resources.GetObject("chkMemoryScanner.Enabled")));
			this.chkMemoryScanner.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkMemoryScanner.FlatStyle")));
			this.chkMemoryScanner.Font = ((System.Drawing.Font)(resources.GetObject("chkMemoryScanner.Font")));
			this.chkMemoryScanner.Image = ((System.Drawing.Image)(resources.GetObject("chkMemoryScanner.Image")));
			this.chkMemoryScanner.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkMemoryScanner.ImageAlign")));
			this.chkMemoryScanner.ImageIndex = ((int)(resources.GetObject("chkMemoryScanner.ImageIndex")));
			this.chkMemoryScanner.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkMemoryScanner.ImeMode")));
			this.chkMemoryScanner.Location = ((System.Drawing.Point)(resources.GetObject("chkMemoryScanner.Location")));
			this.chkMemoryScanner.Name = "chkMemoryScanner";
			this.chkMemoryScanner.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkMemoryScanner.RightToLeft")));
			this.chkMemoryScanner.Size = ((System.Drawing.Size)(resources.GetObject("chkMemoryScanner.Size")));
			this.chkMemoryScanner.TabIndex = ((int)(resources.GetObject("chkMemoryScanner.TabIndex")));
			this.chkMemoryScanner.Text = resources.GetString("chkMemoryScanner.Text");
			this.chkMemoryScanner.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkMemoryScanner.TextAlign")));
			this.toolTip1.SetToolTip(this.chkMemoryScanner, resources.GetString("chkMemoryScanner.ToolTip"));
			this.chkMemoryScanner.Visible = ((bool)(resources.GetObject("chkMemoryScanner.Visible")));
			this.chkMemoryScanner.CheckedChanged += new System.EventHandler(this.chkMemoryScanner_CheckedChanged);
			// 
			// radScannerFreq
			// 
			this.radScannerFreq.AccessibleDescription = resources.GetString("radScannerFreq.AccessibleDescription");
			this.radScannerFreq.AccessibleName = resources.GetString("radScannerFreq.AccessibleName");
			this.radScannerFreq.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radScannerFreq.Anchor")));
			this.radScannerFreq.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radScannerFreq.Appearance")));
			this.radScannerFreq.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radScannerFreq.BackgroundImage")));
			this.radScannerFreq.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radScannerFreq.CheckAlign")));
			this.radScannerFreq.Checked = true;
			this.radScannerFreq.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radScannerFreq.Dock")));
			this.radScannerFreq.Enabled = ((bool)(resources.GetObject("radScannerFreq.Enabled")));
			this.radScannerFreq.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radScannerFreq.FlatStyle")));
			this.radScannerFreq.Font = ((System.Drawing.Font)(resources.GetObject("radScannerFreq.Font")));
			this.radScannerFreq.Image = ((System.Drawing.Image)(resources.GetObject("radScannerFreq.Image")));
			this.radScannerFreq.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radScannerFreq.ImageAlign")));
			this.radScannerFreq.ImageIndex = ((int)(resources.GetObject("radScannerFreq.ImageIndex")));
			this.radScannerFreq.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radScannerFreq.ImeMode")));
			this.radScannerFreq.Location = ((System.Drawing.Point)(resources.GetObject("radScannerFreq.Location")));
			this.radScannerFreq.Name = "radScannerFreq";
			this.radScannerFreq.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radScannerFreq.RightToLeft")));
			this.radScannerFreq.Size = ((System.Drawing.Size)(resources.GetObject("radScannerFreq.Size")));
			this.radScannerFreq.TabIndex = ((int)(resources.GetObject("radScannerFreq.TabIndex")));
			this.radScannerFreq.TabStop = true;
			this.radScannerFreq.Text = resources.GetString("radScannerFreq.Text");
			this.radScannerFreq.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radScannerFreq.TextAlign")));
			this.toolTip1.SetToolTip(this.radScannerFreq, resources.GetString("radScannerFreq.ToolTip"));
			this.radScannerFreq.Visible = ((bool)(resources.GetObject("radScannerFreq.Visible")));
			// 
			// radScannerChannel
			// 
			this.radScannerChannel.AccessibleDescription = resources.GetString("radScannerChannel.AccessibleDescription");
			this.radScannerChannel.AccessibleName = resources.GetString("radScannerChannel.AccessibleName");
			this.radScannerChannel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radScannerChannel.Anchor")));
			this.radScannerChannel.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radScannerChannel.Appearance")));
			this.radScannerChannel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radScannerChannel.BackgroundImage")));
			this.radScannerChannel.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radScannerChannel.CheckAlign")));
			this.radScannerChannel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radScannerChannel.Dock")));
			this.radScannerChannel.Enabled = ((bool)(resources.GetObject("radScannerChannel.Enabled")));
			this.radScannerChannel.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radScannerChannel.FlatStyle")));
			this.radScannerChannel.Font = ((System.Drawing.Font)(resources.GetObject("radScannerChannel.Font")));
			this.radScannerChannel.Image = ((System.Drawing.Image)(resources.GetObject("radScannerChannel.Image")));
			this.radScannerChannel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radScannerChannel.ImageAlign")));
			this.radScannerChannel.ImageIndex = ((int)(resources.GetObject("radScannerChannel.ImageIndex")));
			this.radScannerChannel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radScannerChannel.ImeMode")));
			this.radScannerChannel.Location = ((System.Drawing.Point)(resources.GetObject("radScannerChannel.Location")));
			this.radScannerChannel.Name = "radScannerChannel";
			this.radScannerChannel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radScannerChannel.RightToLeft")));
			this.radScannerChannel.Size = ((System.Drawing.Size)(resources.GetObject("radScannerChannel.Size")));
			this.radScannerChannel.TabIndex = ((int)(resources.GetObject("radScannerChannel.TabIndex")));
			this.radScannerChannel.Text = resources.GetString("radScannerChannel.Text");
			this.radScannerChannel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radScannerChannel.TextAlign")));
			this.toolTip1.SetToolTip(this.radScannerChannel, resources.GetString("radScannerChannel.ToolTip"));
			this.radScannerChannel.Visible = ((bool)(resources.GetObject("radScannerChannel.Visible")));
			// 
			// btnSave
			// 
			this.btnSave.AccessibleDescription = resources.GetString("btnSave.AccessibleDescription");
			this.btnSave.AccessibleName = resources.GetString("btnSave.AccessibleName");
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnSave.Anchor")));
			this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
			this.btnSave.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnSave.Dock")));
			this.btnSave.Enabled = ((bool)(resources.GetObject("btnSave.Enabled")));
			this.btnSave.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnSave.FlatStyle")));
			this.btnSave.Font = ((System.Drawing.Font)(resources.GetObject("btnSave.Font")));
			this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
			this.btnSave.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnSave.ImageAlign")));
			this.btnSave.ImageIndex = ((int)(resources.GetObject("btnSave.ImageIndex")));
			this.btnSave.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnSave.ImeMode")));
			this.btnSave.Location = ((System.Drawing.Point)(resources.GetObject("btnSave.Location")));
			this.btnSave.Name = "btnSave";
			this.btnSave.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnSave.RightToLeft")));
			this.btnSave.Size = ((System.Drawing.Size)(resources.GetObject("btnSave.Size")));
			this.btnSave.TabIndex = ((int)(resources.GetObject("btnSave.TabIndex")));
			this.btnSave.Text = resources.GetString("btnSave.Text");
			this.btnSave.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnSave.TextAlign")));
			this.toolTip1.SetToolTip(this.btnSave, resources.GetString("btnSave.ToolTip"));
			this.btnSave.Visible = ((bool)(resources.GetObject("btnSave.Visible")));
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnMemoryRecall
			// 
			this.btnMemoryRecall.AccessibleDescription = resources.GetString("btnMemoryRecall.AccessibleDescription");
			this.btnMemoryRecall.AccessibleName = resources.GetString("btnMemoryRecall.AccessibleName");
			this.btnMemoryRecall.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnMemoryRecall.Anchor")));
			this.btnMemoryRecall.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMemoryRecall.BackgroundImage")));
			this.btnMemoryRecall.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnMemoryRecall.Dock")));
			this.btnMemoryRecall.Enabled = ((bool)(resources.GetObject("btnMemoryRecall.Enabled")));
			this.btnMemoryRecall.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnMemoryRecall.FlatStyle")));
			this.btnMemoryRecall.Font = ((System.Drawing.Font)(resources.GetObject("btnMemoryRecall.Font")));
			this.btnMemoryRecall.Image = ((System.Drawing.Image)(resources.GetObject("btnMemoryRecall.Image")));
			this.btnMemoryRecall.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnMemoryRecall.ImageAlign")));
			this.btnMemoryRecall.ImageIndex = ((int)(resources.GetObject("btnMemoryRecall.ImageIndex")));
			this.btnMemoryRecall.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnMemoryRecall.ImeMode")));
			this.btnMemoryRecall.Location = ((System.Drawing.Point)(resources.GetObject("btnMemoryRecall.Location")));
			this.btnMemoryRecall.Name = "btnMemoryRecall";
			this.btnMemoryRecall.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnMemoryRecall.RightToLeft")));
			this.btnMemoryRecall.Size = ((System.Drawing.Size)(resources.GetObject("btnMemoryRecall.Size")));
			this.btnMemoryRecall.TabIndex = ((int)(resources.GetObject("btnMemoryRecall.TabIndex")));
			this.btnMemoryRecall.Text = resources.GetString("btnMemoryRecall.Text");
			this.btnMemoryRecall.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnMemoryRecall.TextAlign")));
			this.toolTip1.SetToolTip(this.btnMemoryRecall, resources.GetString("btnMemoryRecall.ToolTip"));
			this.btnMemoryRecall.Visible = ((bool)(resources.GetObject("btnMemoryRecall.Visible")));
			this.btnMemoryRecall.Click += new System.EventHandler(this.btnMemoryRecall_Click);
			// 
			// timer_scanner
			// 
			this.timer_scanner.Interval = 3000;
			this.timer_scanner.Tick += new System.EventHandler(this.timer_scanner_Tick);
			// 
			// timer_peak_text
			// 
			this.timer_peak_text.Interval = 500;
			this.timer_peak_text.Tick += new System.EventHandler(this.timer_peak_text_Tick);
			// 
			// grpDateTime
			// 
			this.grpDateTime.AccessibleDescription = resources.GetString("grpDateTime.AccessibleDescription");
			this.grpDateTime.AccessibleName = resources.GetString("grpDateTime.AccessibleName");
			this.grpDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpDateTime.Anchor")));
			this.grpDateTime.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpDateTime.BackgroundImage")));
			this.grpDateTime.Controls.Add(this.txtTime);
			this.grpDateTime.Controls.Add(this.txtDate);
			this.grpDateTime.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpDateTime.Dock")));
			this.grpDateTime.Enabled = ((bool)(resources.GetObject("grpDateTime.Enabled")));
			this.grpDateTime.Font = ((System.Drawing.Font)(resources.GetObject("grpDateTime.Font")));
			this.grpDateTime.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpDateTime.ImeMode")));
			this.grpDateTime.Location = ((System.Drawing.Point)(resources.GetObject("grpDateTime.Location")));
			this.grpDateTime.Name = "grpDateTime";
			this.grpDateTime.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpDateTime.RightToLeft")));
			this.grpDateTime.Size = ((System.Drawing.Size)(resources.GetObject("grpDateTime.Size")));
			this.grpDateTime.TabIndex = ((int)(resources.GetObject("grpDateTime.TabIndex")));
			this.grpDateTime.TabStop = false;
			this.grpDateTime.Text = resources.GetString("grpDateTime.Text");
			this.toolTip1.SetToolTip(this.grpDateTime, resources.GetString("grpDateTime.ToolTip"));
			this.grpDateTime.Visible = ((bool)(resources.GetObject("grpDateTime.Visible")));
			// 
			// txtTime
			// 
			this.txtTime.AccessibleDescription = resources.GetString("txtTime.AccessibleDescription");
			this.txtTime.AccessibleName = resources.GetString("txtTime.AccessibleName");
			this.txtTime.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtTime.Anchor")));
			this.txtTime.AutoSize = ((bool)(resources.GetObject("txtTime.AutoSize")));
			this.txtTime.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtTime.BackgroundImage")));
			this.txtTime.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtTime.Dock")));
			this.txtTime.Enabled = ((bool)(resources.GetObject("txtTime.Enabled")));
			this.txtTime.Font = ((System.Drawing.Font)(resources.GetObject("txtTime.Font")));
			this.txtTime.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtTime.ImeMode")));
			this.txtTime.Location = ((System.Drawing.Point)(resources.GetObject("txtTime.Location")));
			this.txtTime.MaxLength = ((int)(resources.GetObject("txtTime.MaxLength")));
			this.txtTime.Multiline = ((bool)(resources.GetObject("txtTime.Multiline")));
			this.txtTime.Name = "txtTime";
			this.txtTime.PasswordChar = ((char)(resources.GetObject("txtTime.PasswordChar")));
			this.txtTime.ReadOnly = true;
			this.txtTime.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtTime.RightToLeft")));
			this.txtTime.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtTime.ScrollBars")));
			this.txtTime.Size = ((System.Drawing.Size)(resources.GetObject("txtTime.Size")));
			this.txtTime.TabIndex = ((int)(resources.GetObject("txtTime.TabIndex")));
			this.txtTime.Text = resources.GetString("txtTime.Text");
			this.txtTime.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtTime.TextAlign")));
			this.toolTip1.SetToolTip(this.txtTime, resources.GetString("txtTime.ToolTip"));
			this.txtTime.Visible = ((bool)(resources.GetObject("txtTime.Visible")));
			this.txtTime.WordWrap = ((bool)(resources.GetObject("txtTime.WordWrap")));
			this.txtTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DateTime_MouseDown);
			// 
			// txtDate
			// 
			this.txtDate.AccessibleDescription = resources.GetString("txtDate.AccessibleDescription");
			this.txtDate.AccessibleName = resources.GetString("txtDate.AccessibleName");
			this.txtDate.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtDate.Anchor")));
			this.txtDate.AutoSize = ((bool)(resources.GetObject("txtDate.AutoSize")));
			this.txtDate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtDate.BackgroundImage")));
			this.txtDate.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtDate.Dock")));
			this.txtDate.Enabled = ((bool)(resources.GetObject("txtDate.Enabled")));
			this.txtDate.Font = ((System.Drawing.Font)(resources.GetObject("txtDate.Font")));
			this.txtDate.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtDate.ImeMode")));
			this.txtDate.Location = ((System.Drawing.Point)(resources.GetObject("txtDate.Location")));
			this.txtDate.MaxLength = ((int)(resources.GetObject("txtDate.MaxLength")));
			this.txtDate.Multiline = ((bool)(resources.GetObject("txtDate.Multiline")));
			this.txtDate.Name = "txtDate";
			this.txtDate.PasswordChar = ((char)(resources.GetObject("txtDate.PasswordChar")));
			this.txtDate.ReadOnly = true;
			this.txtDate.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtDate.RightToLeft")));
			this.txtDate.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtDate.ScrollBars")));
			this.txtDate.Size = ((System.Drawing.Size)(resources.GetObject("txtDate.Size")));
			this.txtDate.TabIndex = ((int)(resources.GetObject("txtDate.TabIndex")));
			this.txtDate.Text = resources.GetString("txtDate.Text");
			this.txtDate.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtDate.TextAlign")));
			this.toolTip1.SetToolTip(this.txtDate, resources.GetString("txtDate.ToolTip"));
			this.txtDate.Visible = ((bool)(resources.GetObject("txtDate.Visible")));
			this.txtDate.WordWrap = ((bool)(resources.GetObject("txtDate.WordWrap")));
			this.txtDate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DateTime_MouseDown);
			// 
			// chkVFOLock
			// 
			this.chkVFOLock.AccessibleDescription = resources.GetString("chkVFOLock.AccessibleDescription");
			this.chkVFOLock.AccessibleName = resources.GetString("chkVFOLock.AccessibleName");
			this.chkVFOLock.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkVFOLock.Anchor")));
			this.chkVFOLock.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkVFOLock.Appearance")));
			this.chkVFOLock.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkVFOLock.BackgroundImage")));
			this.chkVFOLock.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkVFOLock.CheckAlign")));
			this.chkVFOLock.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkVFOLock.Dock")));
			this.chkVFOLock.Enabled = ((bool)(resources.GetObject("chkVFOLock.Enabled")));
			this.chkVFOLock.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkVFOLock.FlatStyle")));
			this.chkVFOLock.Font = ((System.Drawing.Font)(resources.GetObject("chkVFOLock.Font")));
			this.chkVFOLock.Image = ((System.Drawing.Image)(resources.GetObject("chkVFOLock.Image")));
			this.chkVFOLock.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkVFOLock.ImageAlign")));
			this.chkVFOLock.ImageIndex = ((int)(resources.GetObject("chkVFOLock.ImageIndex")));
			this.chkVFOLock.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkVFOLock.ImeMode")));
			this.chkVFOLock.Location = ((System.Drawing.Point)(resources.GetObject("chkVFOLock.Location")));
			this.chkVFOLock.Name = "chkVFOLock";
			this.chkVFOLock.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkVFOLock.RightToLeft")));
			this.chkVFOLock.Size = ((System.Drawing.Size)(resources.GetObject("chkVFOLock.Size")));
			this.chkVFOLock.TabIndex = ((int)(resources.GetObject("chkVFOLock.TabIndex")));
			this.chkVFOLock.Text = resources.GetString("chkVFOLock.Text");
			this.chkVFOLock.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkVFOLock.TextAlign")));
			this.toolTip1.SetToolTip(this.chkVFOLock, resources.GetString("chkVFOLock.ToolTip"));
			this.chkVFOLock.Visible = ((bool)(resources.GetObject("chkVFOLock.Visible")));
			this.chkVFOLock.CheckedChanged += new System.EventHandler(this.chkVFOLock_CheckedChanged);
			// 
			// lblCWSpeed
			// 
			this.lblCWSpeed.AccessibleDescription = resources.GetString("lblCWSpeed.AccessibleDescription");
			this.lblCWSpeed.AccessibleName = resources.GetString("lblCWSpeed.AccessibleName");
			this.lblCWSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblCWSpeed.Anchor")));
			this.lblCWSpeed.AutoSize = ((bool)(resources.GetObject("lblCWSpeed.AutoSize")));
			this.lblCWSpeed.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblCWSpeed.Dock")));
			this.lblCWSpeed.Enabled = ((bool)(resources.GetObject("lblCWSpeed.Enabled")));
			this.lblCWSpeed.Font = ((System.Drawing.Font)(resources.GetObject("lblCWSpeed.Font")));
			this.lblCWSpeed.Image = ((System.Drawing.Image)(resources.GetObject("lblCWSpeed.Image")));
			this.lblCWSpeed.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblCWSpeed.ImageAlign")));
			this.lblCWSpeed.ImageIndex = ((int)(resources.GetObject("lblCWSpeed.ImageIndex")));
			this.lblCWSpeed.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblCWSpeed.ImeMode")));
			this.lblCWSpeed.Location = ((System.Drawing.Point)(resources.GetObject("lblCWSpeed.Location")));
			this.lblCWSpeed.Name = "lblCWSpeed";
			this.lblCWSpeed.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblCWSpeed.RightToLeft")));
			this.lblCWSpeed.Size = ((System.Drawing.Size)(resources.GetObject("lblCWSpeed.Size")));
			this.lblCWSpeed.TabIndex = ((int)(resources.GetObject("lblCWSpeed.TabIndex")));
			this.lblCWSpeed.Text = resources.GetString("lblCWSpeed.Text");
			this.lblCWSpeed.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblCWSpeed.TextAlign")));
			this.toolTip1.SetToolTip(this.lblCWSpeed, resources.GetString("lblCWSpeed.ToolTip"));
			this.lblCWSpeed.Visible = ((bool)(resources.GetObject("lblCWSpeed.Visible")));
			// 
			// udCWSpeed
			// 
			this.udCWSpeed.AccessibleDescription = resources.GetString("udCWSpeed.AccessibleDescription");
			this.udCWSpeed.AccessibleName = resources.GetString("udCWSpeed.AccessibleName");
			this.udCWSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("udCWSpeed.Anchor")));
			this.udCWSpeed.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("udCWSpeed.Dock")));
			this.udCWSpeed.Enabled = ((bool)(resources.GetObject("udCWSpeed.Enabled")));
			this.udCWSpeed.Font = ((System.Drawing.Font)(resources.GetObject("udCWSpeed.Font")));
			this.udCWSpeed.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("udCWSpeed.ImeMode")));
			this.udCWSpeed.Increment = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		0});
			this.udCWSpeed.Location = ((System.Drawing.Point)(resources.GetObject("udCWSpeed.Location")));
			this.udCWSpeed.Maximum = new System.Decimal(new int[] {
																	  60,
																	  0,
																	  0,
																	  0});
			this.udCWSpeed.Minimum = new System.Decimal(new int[] {
																	  1,
																	  0,
																	  0,
																	  0});
			this.udCWSpeed.Name = "udCWSpeed";
			this.udCWSpeed.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("udCWSpeed.RightToLeft")));
			this.udCWSpeed.Size = ((System.Drawing.Size)(resources.GetObject("udCWSpeed.Size")));
			this.udCWSpeed.TabIndex = ((int)(resources.GetObject("udCWSpeed.TabIndex")));
			this.udCWSpeed.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("udCWSpeed.TextAlign")));
			this.udCWSpeed.ThousandsSeparator = ((bool)(resources.GetObject("udCWSpeed.ThousandsSeparator")));
			this.toolTip1.SetToolTip(this.udCWSpeed, resources.GetString("udCWSpeed.ToolTip"));
			this.udCWSpeed.UpDownAlign = ((System.Windows.Forms.LeftRightAlignment)(resources.GetObject("udCWSpeed.UpDownAlign")));
			this.udCWSpeed.Value = new System.Decimal(new int[] {
																	25,
																	0,
																	0,
																	0});
			this.udCWSpeed.Visible = ((bool)(resources.GetObject("udCWSpeed.Visible")));
			this.udCWSpeed.LostFocus += new System.EventHandler(this.udCWSpeed_LostFocus);
			this.udCWSpeed.ValueChanged += new System.EventHandler(this.udCWSpeed_ValueChanged);
			// 
			// timer_clock
			// 
			this.timer_clock.Enabled = true;
			this.timer_clock.Tick += new System.EventHandler(this.timer_clock_Tick);
			// 
			// Console
			// 
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.grpBandVHF);
			this.Controls.Add(this.udCWSpeed);
			this.Controls.Add(this.lblCWSpeed);
			this.Controls.Add(this.chkVFOLock);
			this.Controls.Add(this.grpDateTime);
			this.Controls.Add(this.grpMemory);
			this.Controls.Add(this.grpDisplay2);
			this.Controls.Add(this.grpOptions);
			this.Controls.Add(this.grpSoundControls);
			this.Controls.Add(this.grpVFO);
			this.Controls.Add(this.grpMultimeter);
			this.Controls.Add(this.chkPipe);
			this.Controls.Add(this.grpDSP);
			this.Controls.Add(this.lblCPUMeter);
			this.Controls.Add(this.grpMode);
			this.Controls.Add(this.grpDisplay);
			this.Controls.Add(this.grpVFOA);
			this.Controls.Add(this.grpVFOB);
			this.Controls.Add(this.grpBandHF);
			this.Controls.Add(this.grpFilter);
			this.Controls.Add(this.chkPower);
			this.Controls.Add(this.lblFilterHigh);
			this.Controls.Add(this.lblFilterLow);
			this.Controls.Add(this.udFilterHigh);
			this.Controls.Add(this.udFilterLow);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.KeyPreview = true;
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximizeBox = false;
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.Menu = this.mainMenu1;
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "Console";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Console_KeyDown);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WheelTune_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Console_Closing);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Console_KeyUp);
			this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Console_MouseWheel);
			this.grpVFOA.ResumeLayout(false);
			this.grpVFOB.ResumeLayout(false);
			this.grpDisplay.ResumeLayout(false);
			this.grpBandVHF.ResumeLayout(false);
			this.grpMode.ResumeLayout(false);
			this.grpBandHF.ResumeLayout(false);
			this.grpFilter.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbFilterWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbFilterShift)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udFilterHigh)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udFilterLow)).EndInit();
			this.grpDSP.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udXIT)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udRIT)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPWR)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udAF)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udMIC)).EndInit();
			this.grpMultimeter.ResumeLayout(false);
			this.grpVFO.ResumeLayout(false);
			this.grpSoundControls.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udSquelch)).EndInit();
			this.grpOptions.ResumeLayout(false);
			this.grpDisplay2.ResumeLayout(false);
			this.grpMemory.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udScannerDelay)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udScannerStep)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udScannerHigh)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udScannerLow)).EndInit();
			this.grpDateTime.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udCWSpeed)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Main
		// ======================================================
		// Main
		// ======================================================

		[STAThread]
		static void Main(string[] args) 
		{
			try 
			{
				try
				{
					if(!CheckForOpenProcesses())
						return;
				}
				catch(Exception)
				{

				}
//				Application.EnableVisualStyles(); 
//				Application.DoEvents(); 
				Application.Run(new Console(args));
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message+"\n\n"+ex.StackTrace.ToString(), "Fatal Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region Misc Routines
		// ======================================================
		// Misc Routines
		// ======================================================

		private void InitConsole()
		{
#if(DEBUG)
			mnuUCB.Visible = true;
#endif

			UpdateBandStackRegisters();

			Audio.console = this;
			chkDSPNB2.Enabled = true;
			Display.console = this;

			vfoa_hover_digit = -1;
			vfob_hover_digit = -1;

			vhf_text = new ButtonTS[15];
			vhf_text[0] = btnBandVHF0;
			vhf_text[1] = btnBandVHF1;
			vhf_text[2] = btnBandVHF2;
			vhf_text[3] = btnBandVHF3;
			vhf_text[4] = btnBandVHF4;
			vhf_text[5] = btnBandVHF5;
			vhf_text[6] = btnBandVHF6;
			vhf_text[7] = btnBandVHF7;
			vhf_text[8] = btnBandVHF8;
			vhf_text[9] = btnBandVHF9;
			vhf_text[10] = btnBandVHF10;
			vhf_text[11] = btnBandVHF11;
			vhf_text[12] = btnBandVHF12;
			vhf_text[13] = btnBandVHF13;

			atu_tuning = false;
			tune_power = 10;
			calibrating = false;
			run_setup_wizard = true;

			// get culture specific decimal separator
			separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

			//dsp_display_mutex = new Mutex();
			//dsp_meter_mutex = new Mutex();
			pa_power_mutex = new Mutex();
			high_swr_mutex = new Mutex();

			last_band = "";						// initialize bandstack
			
			

			sc2_cal = new int[101];
			for(int i=0; i<101; i++)
				sc2_cal[i] = 50;

			wheel_tune_list = new double[8];		// initialize wheel tuning list array
			wheel_tune_list[0] = 0.000001;
			wheel_tune_list[1] = 0.00001;
			wheel_tune_list[2] = 0.0001;
			wheel_tune_list[3] = 0.001;
			wheel_tune_list[4] = 0.01;
			wheel_tune_list[5] = 0.1;
			wheel_tune_list[6] = 1.0;
			wheel_tune_list[7] = 10.0;
			wheel_tune_index = 3;

			meter_text_history = new float[multimeter_text_peak_samples];

			current_meter_data = -200.0f;

			preamp_offset = new float[4];
			preamp_offset[(int)PreampMode.OFF] = 10.0f;
			preamp_offset[(int)PreampMode.LOW] = 0.0f;
			preamp_offset[(int)PreampMode.MED] = -16.0f;
			preamp_offset[(int)PreampMode.HIGH] = -26.0f;

			this.ActiveControl = chkPower;		// Power has focus initially
			long_crosshair = false;				// initialize long crosshairs

			Display.Target = picDisplay;
			Display.Init();						// Initialize Display variables
			InitDisplayModes();					// Initialize Display Modes
			InitAGCModes();						// Initialize AGC Modes
			InitMultiMeterModes();				// Initialize MultiMeter Modes

			audio_process_thread = new Thread(	// create audio process thread
				new ThreadStart(DttSP.ProcessSamplesThread));
			audio_process_thread.Name = "Audio Process Thread";
			audio_process_thread.Priority = ThreadPriority.Highest;
			audio_process_thread.IsBackground = true;
			audio_process_thread.Start();

			hw = new HW(0x378);					// create hardware object
			Hdw.Init();							// Power down hardware

			siolisten = new SIOListenerII(this);

			CWForm = new CW(this);				// create CW form
			CWForm.StartPosition = FormStartPosition.Manual;

			Keyer = new CWKeyer2(this);			// create new Keyer

			//CWXForm = new CWX(this);				// create CW form

			EQForm = new EQForm();

			SetupForm = new Setup(this);		// create Setup form
			SetupForm.StartPosition = FormStartPosition.Manual;

			EQForm.RestoreSettings();

			CWForm.GetCWOptions();
			//CWXForm.RestoreSettings();

			XVTRForm = new XVTRForm(this);

			MemForm = new Memory(this);			// create Memory form
			MemForm.StartPosition = FormStartPosition.Manual;

			WaveForm = new WaveControl(this);	// create Wave form
			WaveForm.StartPosition = FormStartPosition.Manual;			

			//current_filter = Filter.F2600;

			CurrentAGCMode = AGCMode.MED;				// Initialize front panel controls
			comboPreamp.Text = "High";
			vfob_dsp_mode = DSPMode.LSB;
			vfob_filter = Filter.F2600;
			comboDisplayMode.Text = "Panadapter";
			comboMeterRXMode.SelectedIndex = 0;
			comboTuneMode.SelectedIndex = 0;
			quick_save_filter = Filter.F2600;
			quick_save_mode = DSPMode.LSB;
			udPWR.Value = 50;

			last_filter = new Filter[(int)DSPMode.LAST];
			var1_low = new int[(int)DSPMode.LAST];
			var1_high = new int[(int)DSPMode.LAST];
			var2_low = new int[(int)DSPMode.LAST];
			var2_high = new int[(int)DSPMode.LAST];
			InitFilterVals();					// Initialize filter values
			
			GetState();							// recall saved state

			if(current_dsp_mode == DSPMode.FIRST || current_dsp_mode == DSPMode.LAST)
				radModeLSB.Checked = true;
			if(current_filter == Filter.FIRST || current_filter == Filter.LAST ||
				(current_filter == Filter.NONE && current_dsp_mode != DSPMode.DRM && current_dsp_mode != DSPMode.SPEC))
				radFilter2600.Checked = true;

			PAPresent = pa_present;
			txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			txtVFOBFreq_LostFocus(this, EventArgs.Empty);
			udPWR_ValueChanged(this, EventArgs.Empty);
			udAF_ValueChanged(this, EventArgs.Empty);
			udMIC_ValueChanged(this, EventArgs.Empty);
			tbFilterShift_Scroll(this, EventArgs.Empty);

			Hdw.StandBy();						// initialize hardware device

			wheel_tune_index--;					// Setup wheel tuning
			ChangeWheelTuneLeft();

			SetupForm.initCATandPTTprops();   // wjt added -- get console props setup for cat and ptt 
			if ( CmdLineArgs != null ) 
			{ 
				for ( int i = 0; i < CmdLineArgs.Length; i++ ) 
				{
					switch(CmdLineArgs[i])
					{
						case "--disable-swr-prot-at-my-risk":
							DisableSWRProtection = true; 
							this.Text = this.Text + "  *** SWR Protection Disabled! ***";
							break;
						case "--high-pwr-am":
							Audio.high_pwr_am = true;
							MessageBox.Show("high power am");
							break;
					}
				}
			}

			if(comboMeterTXMode.Items.Count > 0 && comboMeterTXMode.SelectedIndex < 0)
				comboMeterTXMode.SelectedIndex = 0;
			chkMOX.Enabled = false;
		}

		public void ExitConsole()
		{
			if(SetupForm != null)		// make sure Setup form is deallocated
				SetupForm.Dispose();
			if(CWForm != null)			// make sure CW form is deallocated
				CWForm.Dispose();
			if(CWXForm != null)			// make sure CWX form is deallocated
				CWXForm.Dispose();
			chkPower.Checked = false;	// make sure power is off		
			
			//			if(draw_display_thread != null)
			//				draw_display_thread.Abort();
			
			//Parallel.ExitPortTalk();	// close parallel port driver
			PA19.PA_Terminate();		// terminate audio interface
			DB.Exit();					// close and save database
			//Mixer.RestoreState();		// restore initial mixer state
			DttSP.Exit();				// deallocate DSP variables
			Mixer.SetMainVolume(0, main_volume);
			Mixer.SetWaveOutVolume(0, wave_volume);
		}

		public void SaveState()
		{
			// Automatically saves all control settings to the database in the tab
			// pages on this form of the following types: CheckBox, ComboBox,
			// NumericUpDown, RadioButton, TextBox, and TrackBar (slider)

			chkPower.Checked = false;		// turn off the power first

			ArrayList a = new ArrayList();

			foreach(Control c in this.Controls)			// For each control
			{
				if(c.GetType() == typeof(GroupBoxTS))		// if it is a groupbox, check for sub controls
				{
					foreach(Control c2 in ((GroupBoxTS)c).Controls)	// for each sub-control
					{	// check to see if it is a value type we need to save
						if(c2.Enabled)
						{
							if(c2.GetType() == typeof(CheckBoxTS))
								a.Add(c2.Name+"/"+((CheckBoxTS)c2).Checked.ToString());
							else if(c2.GetType() == typeof(ComboBoxTS))
							{
								if(((ComboBoxTS)c2).Items.Count > 0)
									a.Add(c2.Name+"/"+((ComboBoxTS)c2).Text);
							}
							else if(c2.GetType() == typeof(NumericUpDownTS))
								a.Add(c2.Name+"/"+((NumericUpDownTS)c2).Value.ToString());
							else if(c2.GetType() == typeof(RadioButtonTS))
								a.Add(c2.Name+"/"+((RadioButtonTS)c2).Checked.ToString());
							else if(c2.GetType() == typeof(TextBoxTS))
							{
								if(((TextBoxTS)c2).ReadOnly == false)
									a.Add(c2.Name+"/"+((TextBoxTS)c2).Text);
							}
							else if(c2.GetType() == typeof(TrackBarTS))
								a.Add(c2.Name+"/"+((TrackBarTS)c2).Value.ToString());
#if(DEBUG)
							else if(c2.GetType() == typeof(GroupBox) ||
								c2.GetType() == typeof(CheckBox) ||
								c2.GetType() == typeof(ComboBox) ||
								c2.GetType() == typeof(NumericUpDown) ||
								c2.GetType() == typeof(RadioButton) ||
								c2.GetType() == typeof(TextBox) ||
								c2.GetType() == typeof(TrackBar))
								Debug.WriteLine(c2.Name+" needs to be converterd to a Thread Safe control.");
#endif
						}
					}
				}
				else // it is not a group box
				{	// check to see if it is a value type we need to save
					if(c.Enabled)
					{
						if(c.GetType() == typeof(CheckBoxTS))
							a.Add(c.Name+"/"+((CheckBoxTS)c).Checked.ToString());
						else if(c.GetType() == typeof(ComboBoxTS))
						{
							if(((ComboBoxTS)c).SelectedIndex >= 0)
								a.Add(c.Name+"/"+((ComboBoxTS)c).Text);
						}
						else if(c.GetType() == typeof(NumericUpDownTS))
							a.Add(c.Name+"/"+((NumericUpDownTS)c).Value.ToString());
						else if(c.GetType() == typeof(RadioButtonTS))
							a.Add(c.Name+"/"+((RadioButtonTS)c).Checked.ToString());
						else if(c.GetType() == typeof(TextBoxTS))
						{
							if(((TextBoxTS)c).ReadOnly == false)
								a.Add(c.Name+"/"+((TextBoxTS)c).Text);
						}
						else if(c.GetType() == typeof(TrackBarTS))
							a.Add(c.Name+"/"+((TrackBarTS)c).Value.ToString());
#if(DEBUG)
						else if(c.GetType() == typeof(GroupBox) ||
							c.GetType() == typeof(CheckBox) ||
							c.GetType() == typeof(ComboBox) ||
							c.GetType() == typeof(NumericUpDown) ||
							c.GetType() == typeof(RadioButton) ||
							c.GetType() == typeof(TextBox) ||
							c.GetType() == typeof(TrackBar))
							Debug.WriteLine(c.Name+" needs to be converterd to a Thread Safe control.");
#endif
					}
				}
			}

			a.Add("current_datetime_mode/"+ (int)current_datetime_mode);
			a.Add("display_cal_offset/"+display_cal_offset.ToString("f3"));
			a.Add("multimeter_cal_offset/"+multimeter_cal_offset);

			a.Add("txtMemoryQuick/"+ txtMemoryQuick.Text);		// save quick memory settings
			a.Add("quick_save_mode/"+(int)quick_save_mode);
			a.Add("quick_save_filter/"+(int)quick_save_filter);

			for(int i=(int)DSPMode.FIRST+1; i<(int)DSPMode.LAST; i++)
			{	// save filter settings per mode
				a.Add("last_filter["+i.ToString()+"]/"+((int)last_filter[i]).ToString());
				a.Add("var1_low["+i.ToString()+"]/"+var1_low[i].ToString());
				a.Add("var1_high["+i.ToString()+"]/"+var1_high[i].ToString());
				a.Add("var2_low["+i.ToString()+"]/"+var2_low[i].ToString());
				a.Add("var2_high["+i.ToString()+"]/"+var2_high[i].ToString());
			}

			for(int i=(int)PreampMode.FIRST+1; i<(int)PreampMode.LAST; i++)
				a.Add("preamp_offset["+i.ToString()+"]/"+preamp_offset[i].ToString());

			a.Add("wheel_tune_index/"+wheel_tune_index.ToString());		// Save wheel tune value

			a.Add("vfob_dsp_mode/"+((int)vfob_dsp_mode).ToString());			// Save VFO B values 
			a.Add("vfob_filter/"+((int)vfob_filter).ToString());
			
			a.Add("console_top/"+this.Top.ToString());		// save form positions
			a.Add("console_left/"+this.Left.ToString());
			a.Add("setup_top/"+SetupForm.Top.ToString());
			a.Add("setup_left/"+SetupForm.Left.ToString());
			a.Add("cw_top/"+CWForm.Top.ToString());
			a.Add("cw_left/"+CWForm.Left.ToString());
			a.Add("mem_top/"+MemForm.Top.ToString());
			a.Add("mem_left/"+MemForm.Left.ToString());

			a.Add("Version/"+this.Text);		// save the current version

			DB.SaveVars("State", ref a);		// save the values to the DB
		}

		public void GetState()
		{
			// Automatically restores all controls from the database in the
			// tab pages on this form of the following types: CheckBox, ComboBox,
			// NumericUpDown, RadioButton, TextBox, and TrackBar (slider)

			ArrayList checkbox_list = new ArrayList();
			ArrayList combobox_list = new ArrayList();
			ArrayList numericupdown_list = new ArrayList();
			ArrayList radiobutton_list = new ArrayList();
			ArrayList textbox_list = new ArrayList();
			ArrayList trackbar_list = new ArrayList();

			//ArrayList controls = new ArrayList();	// list of controls to restore
			foreach(Control c in this.Controls)
			{
				if(c.GetType() == typeof(GroupBoxTS))	// if control is a groupbox, retrieve all subcontrols
				{
					foreach(Control c2 in ((GroupBoxTS)c).Controls)
					{
						if(c2.Enabled)
						{
							if(c2.GetType() == typeof(CheckBoxTS))			// the control is a CheckBox
								checkbox_list.Add(c2);
							else if(c2.GetType() == typeof(ComboBoxTS))		// the control is a ComboBox
								combobox_list.Add(c2);
							else if(c2.GetType() == typeof(NumericUpDownTS))	// the control is a NumericUpDown
								numericupdown_list.Add(c2);
							else if(c2.GetType() == typeof(RadioButtonTS))	// the control is a RadioButton
								radiobutton_list.Add(c2);
							else if(c2.GetType() == typeof(TextBoxTS))		// the control is a TextBox
								textbox_list.Add(c2);
							else if(c2.GetType() == typeof(TrackBarTS))		// the control is a TrackBar (slider)
								trackbar_list.Add(c2);
						}
					}						
				}
				else
				{
					if(c.Enabled)
					{
						if(c.GetType() == typeof(CheckBoxTS))				// the control is a CheckBox
							checkbox_list.Add(c);
						else if(c.GetType() == typeof(ComboBoxTS))		// the control is a ComboBox
							combobox_list.Add(c);
						else if(c.GetType() == typeof(NumericUpDownTS))	// the control is a NumericUpDown
							numericupdown_list.Add(c);
						else if(c.GetType() == typeof(RadioButtonTS))		// the control is a RadioButton
							radiobutton_list.Add(c);
						else if(c.GetType() == typeof(TextBoxTS))			// the control is a TextBox
							textbox_list.Add(c);
						else if(c.GetType() == typeof(TrackBarTS))		// the control is a TrackBar (slider)
							trackbar_list.Add(c);
					}
				}
			}

			ArrayList a = DB.GetVars("State");							// Get the saved list of controls
			a.Sort();
			int num_controls = checkbox_list.Count + combobox_list.Count +
				numericupdown_list.Count + radiobutton_list.Count +
				textbox_list.Count + trackbar_list.Count;

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
				int num = 0;

				if(name.StartsWith("last_filter["))
				{
					int start = name.IndexOf("[")+1;
					int length = name.IndexOf("]")-start;
					int index = Int32.Parse(name.Substring(start, length));

					last_filter[index] = (Filter)(Int32.Parse(val));
				}
				else if(name.StartsWith("var1_low["))
				{
					int start = name.IndexOf("[")+1;
					int length = name.IndexOf("]")-start;
					int index = Int32.Parse(name.Substring(start, length));

					var1_low[index] = Int32.Parse(val);
				}
				else if(name.StartsWith("var1_high["))
				{
					int start = name.IndexOf("[")+1;
					int length = name.IndexOf("]")-start;
					int index = Int32.Parse(name.Substring(start, length));

					var1_high[index] = Int32.Parse(val);
				}
				else if(name.StartsWith("var2_low["))
				{
					int start = name.IndexOf("[")+1;
					int length = name.IndexOf("]")-start;
					int index = Int32.Parse(name.Substring(start, length));

					var2_low[index] = Int32.Parse(val);
				}
				else if(name.StartsWith("var2_high["))
				{
					int start = name.IndexOf("[")+1;
					int length = name.IndexOf("]")-start;
					int index = Int32.Parse(name.Substring(start, length));

					var2_high[index] = Int32.Parse(val);
				}
				else if(name.StartsWith("preamp_offset["))
				{
					int start = name.IndexOf("[")+1;
					int length = name.IndexOf("]")-start;
					int index = Int32.Parse(name.Substring(start, length));

					preamp_offset[index] = float.Parse(val);
				}

				switch(name)
				{
					case "current_datetime_mode":
						CurrentDateTimeMode = (DateTimeMode)(Int32.Parse(val));
						break;
					case "wheel_tune_index":
						wheel_tune_index = Int32.Parse(val);
						break;
					case "txtMemoryQuick":
						txtMemoryQuick.Text = val;
						break;
					case "display_cal_offset":
						display_cal_offset = float.Parse(val);
						break;
					case "multimeter_cal_offset":
						multimeter_cal_offset = float.Parse(val);
						break;
					case "quick_save_mode":
						quick_save_mode = (DSPMode)(Int32.Parse(val));
						break;
					case "quick_save_filter":
						quick_save_filter = (Filter)(Int32.Parse(val));
						break;
					case "vfob_dsp_mode":
						vfob_dsp_mode = (DSPMode)(Int32.Parse(val));
						break;
					case "vfob_filter":
						vfob_filter = (Filter)(Int32.Parse(val));
						break;
					case "console_top":
						num = Int32.Parse(val);
						if(num < 0 || num > Screen.PrimaryScreen.Bounds.Height)
							num = 0;
						this.Top = num;
						break;
					case "console_left":
						num = Int32.Parse(val);
						if(num < 0 || num > Screen.PrimaryScreen.Bounds.Width)
							num = 0;
						this.Left = num;
						break;
					case "setup_top":
						num = Int32.Parse(val);
						if(num < 0 || num > Screen.PrimaryScreen.Bounds.Height)
							num = 0;
						SetupForm.Top = num;
						break;
					case "setup_left":
						num = Int32.Parse(val);
						if(num < 0 || num > Screen.PrimaryScreen.Bounds.Width)
							num = 0;
						SetupForm.Left = num;
						break;
					case "cw_top":
						num = Int32.Parse(val);
						if(num < 0 || num > Screen.PrimaryScreen.Bounds.Height)
							num = 0;
						CWForm.Top = num;
						break;
					case "cw_left":
						num = Int32.Parse(val);
						if(num < 0 || num > Screen.PrimaryScreen.Bounds.Width)
							num = 0;
						CWForm.Left = num;
						break;
					case "mem_top":
						num = Int32.Parse(val);
						if(num < 0 || num > Screen.PrimaryScreen.Bounds.Height)
							num = 0;
						MemForm.Top = num;
						break;
					case "mem_left":
						num = Int32.Parse(val);
						if(num < 0 || num > Screen.PrimaryScreen.Bounds.Width)
							num = 0;
						MemForm.Left = num;
						break;
					case "SetupWizard":
						if(val == "1")
							run_setup_wizard = false;
						break;
				}
			}

			// restore saved values to the controls
			foreach(string s in a)				// string is in the format "name,value"
			{
				string[] vals = s.Split('/');
				string name = vals[0];
				string val = vals[1];

				if(s.StartsWith("chk"))			// control is a CheckBox
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
							MessageBox.Show("Control not found: "+name, "GetState Error",
								MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}
				else if(s.StartsWith("combo"))	// control is a ComboBox
				{
					for(int i=0; i<combobox_list.Count; i++)
					{	// look through each control to find the matching name
						ComboBoxTS c = (ComboBoxTS)combobox_list[i];
						if(c.Name.Equals(name))		// name found
						{
							c.Text = val;	// restore value
							i = combobox_list.Count+1;
						}
						if(i == combobox_list.Count)
							MessageBox.Show("Control not found: "+name, "GetState Error",
								MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
							MessageBox.Show("Control not found: "+name, "GetState Error",
								MessageBoxButtons.OK, MessageBoxIcon.Warning);	
					}
				}
				else if(s.StartsWith("rad"))
				{	// look through each control to find the matching name
					for(int i=0; i<radiobutton_list.Count; i++)
					{
						RadioButtonTS c = (RadioButtonTS)radiobutton_list[i];
						if(c.Name.Equals(name))		// name found
						{
							if(!val.ToLower().Equals("true") && !val.ToLower().Equals("false"))
								val = "True";
							c.Checked = bool.Parse(val);	// restore value
							i = radiobutton_list.Count+1;
						}
						if(i == radiobutton_list.Count)
							MessageBox.Show("Control not found: "+name, "GetState Error",
								MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
							MessageBox.Show("Control not found: "+name, "GetState Error",
								MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
							MessageBox.Show("Control not found: "+name, "GetState Error",
								MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}
			}
		}

		public void ResetMemForm()
		{
			MemForm = null;
			MemForm = new Memory(this);
			MemForm.StartPosition = FormStartPosition.Manual;
		}

		private void InitFilterVals()
		{
			// used to initialize all the filter variables
			for(int i=(int)DSPMode.FIRST+1; i<(int)DSPMode.LAST; i++)
			{
				switch(i)
				{
					case (int)DSPMode.LSB:
						last_filter[i] = Filter.F2600;
						var1_low[i] = var2_low[i] = -2800;
						var1_high[i] = var2_high[i] = -200;
						break;
					case (int)DSPMode.USB:
						last_filter[i] = Filter.F2600;
						var1_low[i] = var2_low[i] = 200;
						var1_high[i] = var2_high[i] = 2800;
						break;
					case (int)DSPMode.DSB:
						last_filter[i] = Filter.F2600;
						var1_low[i] = var2_low[i] = -2600;
						var1_high[i] = var2_high[i] = 2600;
						break;
					case (int)DSPMode.CWL:
						last_filter[i] = Filter.F1000;
						var1_low[i] = var2_low[i] = -1200;
						var1_high[i] = var2_high[i] = -200;
						break;
					case (int)DSPMode.CWU:
						last_filter[i] = Filter.F1000;
						var1_low[i] = var2_low[i] = 200;
						var1_high[i] = var2_high[i] = 1200;
						break;
					case (int)DSPMode.AM:
					case (int)DSPMode.SAM:
					case (int)DSPMode.FMN:
						last_filter[i] = Filter.F6000;
						var1_low[i] = var2_low[i] = -6000;
						var1_high[i] = var2_high[i] = 6000;
						break;
					case (int)DSPMode.DIGL:
						last_filter[i] = Filter.F2600;
						var1_low[i] = -2800;
						var1_high[i] = -200;
						var2_low[i] = -2380;
						var2_high[i] = -2040;
						break;
					case (int)DSPMode.DIGU:
						last_filter[i] = Filter.F2600;
						var1_low[i] = var2_low[i] = 200;
						var1_high[i] = var2_high[i] = 2800;
						break;
					default:
						last_filter[i] = Filter.NONE;
						var1_low[i] = var2_low[i] = 0;
						var1_high[i] = var2_high[i] = 0;
						break;
				}
			}
		}

		private void InitDisplayModes()
		{
			// populate the display mode list
			for(DisplayMode dm=DisplayMode.FIRST+1; dm<DisplayMode.LAST; dm++)
			{
				string s = dm.ToString().ToLower();
				s = s.Substring(0, 1).ToUpper() + s.Substring(1, s.Length-1);
				comboDisplayMode.Items.Add(s);
			}
		}

		private void InitAGCModes()
		{
			// populate the AGC mode list
			for(AGCMode agc=AGCMode.FIRST+1; agc<AGCMode.LAST; agc++)
			{
				string s = agc.ToString().ToLower();
				s = s.Substring(0, 1).ToUpper() + s.Substring(1, s.Length-1);
				comboAGC.Items.Add(s);
			}
		}

		private void InitMultiMeterModes()
		{
			comboMeterRXMode.Items.Add("Signal");
			comboMeterRXMode.Items.Add("Sig Avg");
			comboMeterRXMode.Items.Add("ADC L");
			comboMeterRXMode.Items.Add("ADC R");
			comboMeterRXMode.Items.Add("Off");

			comboMeterTXMode.Items.Add("Fwd Pwr");
			comboMeterTXMode.Items.Add("Ref Pwr");
			comboMeterTXMode.Items.Add("Mic");
			comboMeterTXMode.Items.Add("EQ");
			comboMeterTXMode.Items.Add("Leveler");
			comboMeterTXMode.Items.Add("Lev Gain");
			comboMeterTXMode.Items.Add("COMP");
			comboMeterTXMode.Items.Add("CPDR");
			comboMeterTXMode.Items.Add("ALC");			
			comboMeterTXMode.Items.Add("ALC Comp");			
			comboMeterTXMode.Items.Add("Off");
		}

		private void DeselectAllFilters()
		{
			foreach(Control c in grpFilter.Controls)
			{
				if(c.GetType() == typeof(RadioButtonTS))
				{
					((RadioButtonTS)c).Checked = false;
				}
			}
			CurrentFilter = Filter.NONE;
		}

		private void DisableAllFilters()
		{
			foreach(Control c in grpFilter.Controls)
			{
				if(c.GetType() == typeof(RadioButtonTS))
				{
					c.Enabled = false;
				}
			}
		}

		private void EnableAllFilters()
		{
			foreach(Control c in grpFilter.Controls)
			{
				if(c.GetType() == typeof(RadioButtonTS))
				{
					c.Enabled = true;

					if(c.BackColor == vfo_text_dark_color)
					{
						c.BackColor = button_selected_color;
					}
				}
			}
		}

		private void DisableAllBands()
		{
			foreach(ButtonTS b in grpBandHF.Controls)
			{
				b.Enabled = false;

				if(b.BackColor == button_selected_color)
				{
					b.BackColor = vfo_text_dark_color;
				}
			}

			foreach(ButtonTS b in grpBandVHF.Controls)
			{
				b.Enabled = false;

				if(b.BackColor == button_selected_color)
				{
					b.BackColor = vfo_text_dark_color;
				}
			}

		}

		private void EnableAllBands()
		{
			foreach(ButtonTS b in grpBandHF.Controls)
			{
				if(b.Text == "2")
					b.Enabled = XVTRPresent;
				else
					b.Enabled = true;

				if(b.BackColor == Color.Olive)
					b.BackColor = button_selected_color;
			}

			foreach(ButtonTS b in grpBandVHF.Controls)
			{
				if(b.Name == "btnBandHF")
					b.Enabled = true;
				else
				{
					int index = Int32.Parse(b.Name.Substring(10));
					if(XVTRForm.GetEnabled(index))
						b.Enabled = true;
				}

				if(b.BackColor == Color.Olive)
					b.BackColor = button_selected_color;
			}
		}

		private void DisableAllModes()
		{
			foreach(RadioButtonTS r in grpMode.Controls)
			{
				r.Enabled = false;
				if(r.BackColor == button_selected_color)
					r.BackColor = Color.Olive;
			}
		}

		private void EnableAllModes()
		{
			foreach(RadioButtonTS r in grpMode.Controls)
			{
				if(r.Text != "")
					r.Enabled = true;
				if(r.BackColor == Color.Olive)
					r.BackColor = button_selected_color;
			}
		}

		private void DisableFilters(int lowcutoff)
		{
			// Disables all filters below the number passed in. 
			// For example, DisableFilters(100) would cause the
			// 100Hz, 50Hz and 25Hz filters to be disabled.

			foreach(Control c in grpFilter.Controls)
			{
				if(c.GetType() == typeof(RadioButtonTS) && c.Name.IndexOf("Var") < 0)
				{
					string name = c.Name;
					int begin, len;
					begin = name.IndexOf("Filter")+6;
					len = name.Length-begin;

					int filter_width = Int32.Parse(name.Substring(begin, len));
					if(filter_width < lowcutoff)
					{
						c.Enabled = false;
						((RadioButtonTS)c).Checked = false;
					}
				}
			}
		}

		private void GetVFOCharWidth()
		{
			// This function calculates the pixel width of the VFO display.
			// This information is used for mouse wheel hover tuning.

			Graphics g = txtVFOAFreq.CreateGraphics();
			
			SizeF size = g.MeasureString("0", txtVFOAFreq.Font, 1000, StringFormat.GenericTypographic);
			vfo_char_width = (int)Math.Round(size.Width-2.0f, 0);	// subtract 2 since measure string includes 1 pixel border on each side
			float float_char_width = size.Width-2.0f;

			size = g.MeasureString("00", txtVFOAFreq.Font, 1000, StringFormat.GenericTypographic);
			vfo_char_space = (int)Math.Round(size.Width-2.0f - 2*float_char_width, 0);
			
			size = g.MeasureString(separator, txtVFOAFreq.Font, 1000, StringFormat.GenericTypographic);
			vfo_decimal_width = (int)(size.Width-2.0f);

			size = g.MeasureString("0"+separator+"0", txtVFOAFreq.Font, 1000, StringFormat.GenericTypographic);
			vfo_decimal_space = (int)Math.Round(size.Width-2.0f - 2*float_char_width, 0);

			size = g.MeasureString("1234.678901", txtVFOAFreq.Font, 1000, StringFormat.GenericTypographic);
			vfo_pixel_offset = (int)Math.Round(size.Width - 2.0f, 0);

			g.Dispose();
		}

		private void SaveBand()
		{
			// Used in Bandstacking algorithm
			double freq = double.Parse(txtVFOAFreq.Text);
			string filter = "";
			string mode = "";

			foreach(Control c in grpFilter.Controls)
			{
				if(c.GetType() == typeof(RadioButtonTS))
				{
					RadioButtonTS r = (RadioButtonTS)c;
					if(r.Checked)
					{
						filter = r.Name.Substring(9, r.Name.Length-9);
						break;
					}
				}
			}

			foreach(RadioButtonTS r in grpMode.Controls)
			{
				if(r.Checked)
				{
					mode = r.Name.Substring(7, r.Name.Length-7);
					break;
				}
			}

			switch(last_band)
			{
				case "160M":
					if(freq >= 1.8 && freq < 2.0)
						DB.SaveBandStack("160M", band_160m_index, mode, filter, Math.Round(freq, 6));
					break;
				case "80M":
					if(freq >= 3.5 && freq < 4.0)
						DB.SaveBandStack("80M", band_80m_index, mode, filter, Math.Round(freq, 6));
					break;
				case "60M":
					if(freq == 5.3305 || freq == 5.3465 || freq == 5.3665 || freq == 5.3715 || freq == 5.4035)
						DB.SaveBandStack("60M", band_60m_index, "USB", filter, Math.Round(freq, 6));
					break;
				case "40M":
					if(freq >= 7.0 && freq < 7.3)
						DB.SaveBandStack("40M", band_40m_index, mode, filter, Math.Round(freq, 6));
					break;
				case "30M":
					if(freq >= 10.1 && freq < 10.15)
						DB.SaveBandStack("30M", band_30m_index, mode, filter, Math.Round(freq, 6));
					break;
				case "20M":
					if(freq >= 14.0 && freq < 14.350)
						DB.SaveBandStack("20M", band_20m_index, mode, filter, Math.Round(freq, 6));
					break;
				case "17M":
					if(freq >= 18.068 && freq < 18.168)
						DB.SaveBandStack("17M", band_17m_index, mode, filter, Math.Round(freq, 6));
					break;
				case "15M":
					if(freq >= 21.0 && freq < 21.45)
						DB.SaveBandStack("15M", band_15m_index, mode, filter, Math.Round(freq, 6));
					break;
				case "12M":
					if(freq >= 24.890 && freq < 24.990)
						DB.SaveBandStack("12M", band_12m_index, mode, filter, Math.Round(freq, 6));
					break;
				case "10M":
					if(freq >= 28.0 && freq < 29.7)
						DB.SaveBandStack("10M", band_10m_index, mode, filter, Math.Round(freq, 6));
					break;
				case "6M":
					if(freq >= 50.0 && freq < 54.0)
						DB.SaveBandStack("6M", band_6m_index, mode, filter, Math.Round(freq, 6));
					break;
				case "2M":
					if(freq >= 144.0 && freq < 146.0)
						DB.SaveBandStack("2M", band_2m_index, mode, filter, Math.Round(freq, 6));
					break;
				case "WWV":
					if(freq == 2.5 || freq == 5.0 || freq == 10.0 || freq == 15.0 || freq == 20.0)
						DB.SaveBandStack("WWV", band_wwv_index, mode, filter, Math.Round(freq, 6));
					break;
				case "GEN":
					DB.SaveBandStack("GEN", band_gen_index, mode, filter, Math.Round(freq, 6));
					break;
				case "VHF0":
					if(freq >= XVTRForm.GetBegin(0) && freq <= XVTRForm.GetEnd(0))
						DB.SaveBandStack("VHF0", band_vhf0_index, mode, filter, freq);
					break;
				case "VHF1":
					if(freq >= XVTRForm.GetBegin(1) && freq <= XVTRForm.GetEnd(1))
						DB.SaveBandStack("VHF1", band_vhf1_index, mode, filter, freq);
					break;
				case "VHF2":
					if(freq >= XVTRForm.GetBegin(2) && freq <= XVTRForm.GetEnd(2))
						DB.SaveBandStack("VHF2", band_vhf2_index, mode, filter, freq);
					break;
				case "VHF3":
					if(freq >= XVTRForm.GetBegin(3) && freq <= XVTRForm.GetEnd(3))
						DB.SaveBandStack("VHF3", band_vhf3_index, mode, filter, freq);
					break;
				case "VHF4":
					if(freq >= XVTRForm.GetBegin(4) && freq <= XVTRForm.GetEnd(4))
						DB.SaveBandStack("VHF4", band_vhf4_index, mode, filter, freq);
					break;
				case "VHF5":
					if(freq >= XVTRForm.GetBegin(5) && freq <= XVTRForm.GetEnd(5))
						DB.SaveBandStack("VHF5", band_vhf5_index, mode, filter, freq);
					break;
				case "VHF6":
					if(freq >= XVTRForm.GetBegin(6) && freq <= XVTRForm.GetEnd(6))
						DB.SaveBandStack("VHF6", band_vhf6_index, mode, filter, freq);
					break;
				case "VHF7":
					if(freq >= XVTRForm.GetBegin(7) && freq <= XVTRForm.GetEnd(7))
						DB.SaveBandStack("VHF7", band_vhf7_index, mode, filter, freq);
					break;
				case "VHF8":
					if(freq >= XVTRForm.GetBegin(8) && freq <= XVTRForm.GetEnd(8))
						DB.SaveBandStack("VHF8", band_vhf8_index, mode, filter, freq);
					break;
				case "VHF9":
					if(freq >= XVTRForm.GetBegin(9) && freq <= XVTRForm.GetEnd(9))
						DB.SaveBandStack("VHF9", band_vhf9_index, mode, filter, freq);
					break;
				case "VHF10":
					if(freq >= XVTRForm.GetBegin(10) && freq <= XVTRForm.GetEnd(10))
						DB.SaveBandStack("VHF10", band_vhf10_index, mode, filter, freq);
					break;
				case "VHF11":
					if(freq >= XVTRForm.GetBegin(11) && freq <= XVTRForm.GetEnd(11))
						DB.SaveBandStack("VHF11", band_vhf11_index, mode, filter, freq);
					break;
				case "VHF12":
					if(freq >= XVTRForm.GetBegin(12) && freq <= XVTRForm.GetEnd(12))
						DB.SaveBandStack("VHF12", band_vhf12_index, mode, filter, freq);
					break;
				case "VHF13":
					if(freq >= XVTRForm.GetBegin(13) && freq <= XVTRForm.GetEnd(13))
						DB.SaveBandStack("VHF13", band_vhf13_index, mode, filter, freq);
					break;
			}
		}

		private void SetBand(string mode, string filter, double freq)
		{
			// Set mode, filter, and frequency according to passed parameters
			mode = "radMode"+mode;
			filter = "radFilter"+filter;
			
			foreach(RadioButtonTS r in grpMode.Controls)
			{
				if(r.Name.Equals(mode))
				{
					r.Checked = true;
					break;
				}
			}

			if(current_dsp_mode != DSPMode.DRM &&
				current_dsp_mode != DSPMode.SPEC)
			{
				foreach(Control c in grpFilter.Controls)
				{
					if(c.GetType() == typeof(RadioButtonTS) &&
						c.Name.Equals(filter))
					{
						RadioButtonTS r = (RadioButtonTS)c;
						r.Checked = true;
						break;
					}
				}
			}

			VFOAFreq = Math.Round(freq, 6);
			txtVFOAFreq_LostFocus(this, EventArgs.Empty);
		}

		public void MemoryRecall(int mode, int filter, double freq, int step, int agc, int squelch)
		{
			// Set mode, filter, and frequency, mouse wheel tune step
			// and AGC according to passed parameters

			CurrentDSPMode = (DSPMode)mode;
			//			((RadioButton)mode_list[mode]).Checked = true;
			if(current_dsp_mode != DSPMode.DRM &&
				current_dsp_mode != DSPMode.SPEC)
				CurrentFilter = (Filter)filter;
			VFOAFreq = Math.Round(freq, 6);
			txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			comboAGC.SelectedIndex = agc;
			udSquelch.Value = squelch;
			wheel_tune_index = step;
			switch(wheel_tune_index)
			{
				case 0: 
					txtWheelTune.Text = "1Hz";
					break;
				case 1:
					txtWheelTune.Text = "10Hz";
					break;
				case 2:
					txtWheelTune.Text = "100Hz";
					break;
				case 3: 
					txtWheelTune.Text = "1kHz";
					break;
				case 4:
					txtWheelTune.Text = "10kHz";
					break;
				case 5:
					txtWheelTune.Text = "100kHz";
					break;
				case 6: 
					txtWheelTune.Text = "1MHz";
					break;
				case 7:
					txtWheelTune.Text = "10MHz";
					break;
			}
		}

		private void ChangeWheelTuneLeft()
		{
			// change mouse wheel tuning step one digit to the left
			wheel_tune_index = (wheel_tune_index+1)%wheel_tune_list.Length;
			switch(wheel_tune_index)
			{
				case 0: 
					txtWheelTune.Text = "1Hz";
					break;
				case 1:
					txtWheelTune.Text = "10Hz";
					break;
				case 2:
					txtWheelTune.Text = "100Hz";
					break;
				case 3: 
					txtWheelTune.Text = "1kHz";
					break;
				case 4:
					txtWheelTune.Text = "10kHz";
					break;
				case 5:
					txtWheelTune.Text = "100kHz";
					break;
				case 6: 
					txtWheelTune.Text = "1MHz";
					break;
				case 7:
					txtWheelTune.Text = "10MHz";
					break;
			}
		}

		private void ChangeWheelTuneRight()
		{
			// change mouse wheel tuning step one digit to the right
			int length = wheel_tune_list.Length;
			wheel_tune_index = (wheel_tune_index-1 + length)%length;
			switch(wheel_tune_index)
			{
				case 0: 
					txtWheelTune.Text = "1Hz";
					break;
				case 1:
					txtWheelTune.Text = "10Hz";
					break;
				case 2:
					txtWheelTune.Text = "100Hz";
					break;
				case 3: 
					txtWheelTune.Text = "1kHz";
					break;
				case 4:
					txtWheelTune.Text = "10kHz";
					break;
				case 5:
					txtWheelTune.Text = "100kHz";
					break;
				case 6: 
					txtWheelTune.Text = "1MHz";
					break;
				case 7:
					txtWheelTune.Text = "10MHz";
					break;
			}
		}

		private void SetBandButtonColor(Band b)
		{
			// Sets band button color based on passed band.

			Button btn = null;
			switch(b)
			{
				case Band.GEN:
					btn = btnBandGEN;
					break;
				case Band.B160M:
					btn = btnBand160;
					break;
				case Band.B80M:
					btn = btnBand80;
					break;
				case Band.B60M:
					btn = btnBand60;
					break;
				case Band.B40M:
					btn = btnBand40;
					break;
				case Band.B30M:
					btn = btnBand30;
					break;
				case Band.B20M:
					btn = btnBand20;
					break;
				case Band.B17M:
					btn = btnBand17;
					break;
				case Band.B15M:
					btn = btnBand15;
					break;
				case Band.B12M:
					btn = btnBand12;
					break;
				case Band.B10M:
					btn = btnBand10;
					break;
				case Band.B6M:
					btn = btnBand6;
					break;
				case Band.B2M:
					btn = btnBand2;
					break;
				case Band.WWV:
					btn = btnBandWWV;
					break;
				case Band.VHF0:
					btn = btnBandVHF0;
					break;
				case Band.VHF1:
					btn = btnBandVHF1;
					break;
				case Band.VHF2:
					btn = btnBandVHF2;
					break;
				case Band.VHF3:
					btn = btnBandVHF3;
					break;
				case Band.VHF4:
					btn = btnBandVHF4;
					break;
				case Band.VHF5:
					btn = btnBandVHF5;
					break;
				case Band.VHF6:
					btn = btnBandVHF6;
					break;
				case Band.VHF7:
					btn = btnBandVHF7;
					break;
				case Band.VHF8:
					btn = btnBandVHF8;
					break;
				case Band.VHF9:
					btn = btnBandVHF9;
					break;
				case Band.VHF10:
					btn = btnBandVHF10;
					break;
				case Band.VHF11:
					btn = btnBandVHF11;
					break;
				case Band.VHF12:
					btn = btnBandVHF12;
					break;
				case Band.VHF13:
					btn = btnBandVHF13;
					break;
			}

			if(b < Band.VHF0)
			{
				if(!grpBandHF.Visible)
				{
					grpBandHF.Visible = true;
					grpBandVHF.Visible = false;
				}

				foreach(Button b2 in grpBandVHF.Controls)
					b2.BackColor = SystemColors.Control;

				foreach(Button b2 in grpBandHF.Controls)
				{
					Color c = SystemColors.Control;
					if(b2 == btn)
						c = button_selected_color;

					b2.BackColor = c;
				}
			}
			else
			{
				if(!grpBandVHF.Visible)
				{
					grpBandVHF.Visible = true;
					grpBandHF.Visible = false;
				}

				foreach(Button b2 in grpBandHF.Controls)
					b2.BackColor = SystemColors.Control;

				foreach(Button b2 in grpBandVHF.Controls)
				{
					Color c = SystemColors.Control;
					if(b2 == btn)
						c = button_selected_color;

					b2.BackColor = c;
				}
			}
		}

		private void SetCurrentBand(float freq)
		{
			Band b = Band.GEN;

			if(current_xvtr_index >= 0)
			{
				b = (Band)(Band.VHF0+current_xvtr_index);
			}
			else if(!extended)
			{
				if(freq >= 1.8 && freq <= 2.0)
					b = Band.B160M;
				else if(freq >= 3.5 && freq <= 4.0)
					b = Band.B80M;
				else if(freq == 5.3305f || freq == 5.3465f ||
					freq == 5.3665f || freq == 5.3715f ||
					freq == 5.4035f)
					b = Band.B60M;
				else if(freq >= 7.0f && freq <= 7.3f)
					b = Band.B40M;
				else if(freq >= 10.1f && freq <= 10.15f)
					b = Band.B30M;
				else if(freq >= 14.0f && freq <= 14.35f)
					b = Band.B20M;
				else if(freq >= 18.068f && freq <= 18.168f)
					b = Band.B17M;
				else if(freq >= 21.0f && freq <= 21.450f)
					b = Band.B15M;
				else if(freq >= 24.89f && freq <= 24.99f)
					b = Band.B12M;
				else if(freq >= 28.0f && freq <= 29.7f)
					b = Band.B10M;
				else if(freq >= 50.0f && freq <= 54.0f)
					b = Band.B6M;
				else if(freq >= 144.0f && freq <= 148.0f)
					b = Band.B2M;
				else if(freq == 2.5f || freq == 5.0f ||
					freq == 10.0f || freq == 15.0f || 
					freq == 20.0f)
					b = Band.WWV;
				else
					b = Band.GEN;
			}
			else
			{
				if(freq >= 0.0 && freq <= 2.75)
					b = Band.B160M;
				else if(freq > 2.75 && freq <= 5.3305)
					b = Band.B80M;
				else if(freq > 5.3305 && freq < 7.0)
					b = Band.B60M;
				else if(freq >= 7.0 && freq <= 8.7)
					b = Band.B40M;
				else if(freq >= 8.7 && freq <= 12.075)
					b = Band.B30M;
				else if(freq >= 12.075 && freq <= 16.209)
					b = Band.B20M;
				else if(freq >= 16.209 && freq <= 19.584)
					b = Band.B17M;
				else if(freq >= 19.584 && freq <= 23.17)
					b = Band.B15M;
				else if(freq >= 23.17 && freq <= 26.495)
					b = Band.B12M;
				else if(freq >= 26.495 && freq <= 29.7)
					b = Band.B10M;
				else if(freq >= 50.0f && freq <= 54.0f)
					b = Band.B6M;
				else if(freq >= 144.0f && freq <= 148.0f)
					b = Band.B2M;
				else if(freq == 2.5f || freq == 5.0f ||
					freq == 10.0f || freq == 15.0f || 
					freq == 20.0f)
					b = Band.WWV;
				else
					b = Band.GEN;
			}

			if(CurrentBand != b)
			{
				if(chkVFOSplit.Checked)
					chkVFOSplit.Checked = false;
			}

			CurrentBand = b;
			if(atu_present && 
				comboTuneMode.SelectedIndex > 0 &&
				(ATUTuneMode)comboTuneMode.SelectedIndex != ATUTuneMode.BYPASS &&
				(tuned_band != b) &&
				tuned_band != Band.FIRST)
			{
				Hdw.PA_ATUTune(ATUTuneMode.BYPASS);
				tuned_band = Band.FIRST;
				chkTUN.BackColor = SystemColors.Control;				
			}

			if(rfe_present && atu_present && xvtr_present)
			{
				if(b == Band.B2M)
					comboTuneMode.Enabled = false;
				else
					comboTuneMode.Enabled = true;
			}

			if(rfe_present && xvtr_present)
			{
				if(b == Band.B2M)
				{
					if(comboPreamp.Items.Contains("Off"))
						comboPreamp.Items.Remove("Off");
					if(comboPreamp.Items.Contains("Med"))
						comboPreamp.Items.Remove("Med");
					if(comboPreamp.SelectedIndex < 0)
						comboPreamp.Text = "High";
				}
				else
				{
					if(!comboPreamp.Items.Contains("Off"))
						comboPreamp.Items.Insert(0, "Off");
					if(!comboPreamp.Items.Contains("Med"))
						comboPreamp.Items.Insert(2, "Med");
				}
			}
		}

		private float GainByBand(Band b)
		{
			float retval = 0;
			switch(b)
			{
				case Band.B160M:
					retval = SetupForm.PAGain160;
					break;
				case Band.B80M:
					retval = SetupForm.PAGain80;
					break;
				case Band.B60M:
					retval = SetupForm.PAGain60;
					break;
				case Band.B40M:
					retval = SetupForm.PAGain40;
					break;
				case Band.B30M:
					retval = SetupForm.PAGain30;
					break;
				case Band.B20M:
					retval = SetupForm.PAGain20;
					break;
				case Band.B17M:
					retval = SetupForm.PAGain17;
					break;
				case Band.B15M:
					retval = SetupForm.PAGain15;
					break;
				case Band.B12M:
					retval = SetupForm.PAGain12;
					break;
				case Band.B10M:
					retval = SetupForm.PAGain10;
					break;
				default:
					retval = 1000;
					break;
			}

			return retval;
		}

		public void CheckSelectedButtonColor()
		{
			// used when changing the background color of selected buttons
			foreach(Control c in this.Controls)
			{
				if(c.GetType() == typeof(GroupBoxTS))
				{
					foreach(Control c2 in ((GroupBoxTS)c).Controls)
					{
						if(c2.GetType() == typeof(RadioButtonTS))
						{
							RadioButtonTS r = (RadioButtonTS)c2;
							if(r.Checked && r.BackColor != SystemColors.Control)
							{
								c2.BackColor = button_selected_color;
							}
						}
						else if(c2.GetType() == typeof(CheckBoxTS))
						{
							CheckBoxTS chk = (CheckBoxTS)c2;
							if(chk.Checked && chk.BackColor != SystemColors.Control)
							{
								c2.BackColor = button_selected_color;
							}
						}
						else if(c2.GetType() == typeof(NumericUpDownTS))
						{
							NumericUpDownTS ud = (NumericUpDownTS)c2;
							if(ud.BackColor != SystemColors.Window)
							{
								c2.BackColor = button_selected_color;
							}
						}
						else if(c2.GetType() == typeof(ButtonTS))
						{
							ButtonTS b = (ButtonTS)c2;
							if(b.BackColor != SystemColors.Control)
							{
								c2.BackColor = button_selected_color;
							}
						}
					}
				}
				else if(c.GetType() == typeof(RadioButtonTS))
				{
					RadioButtonTS r = (RadioButtonTS)c;
					if(r.Checked && r.BackColor != SystemColors.Control)
						c.BackColor = button_selected_color;
				}
				else if(c.GetType() == typeof(CheckBoxTS))
				{
					CheckBoxTS chk = (CheckBoxTS)c;
					if(chk.Checked && chk.BackColor != SystemColors.Control)
						c.BackColor = button_selected_color;
				}
				else if(c.GetType() == typeof(NumericUpDownTS))
				{
					NumericUpDownTS ud = (NumericUpDownTS)c;
					if(ud.BackColor != SystemColors.Window)
						c.BackColor = button_selected_color;
				}
				else if(c.GetType() == typeof(ButtonTS))
				{
					ButtonTS b = (ButtonTS)c;
					if(b.BackColor != SystemColors.Control)
						c.BackColor = button_selected_color;
				}
			}
		}

		private double PABandOffset(Band b)
		{
			double num = 0;
			switch(b)
			{
				case Band.B160M:
					num = SetupForm.PAADC160;
					break;
				case Band.B80M:
					num = SetupForm.PAADC80;
					break;
				case Band.B60M:
					num = SetupForm.PAADC60;
					break;
				case Band.B40M:
					num = SetupForm.PAADC40;
					break;
				case Band.B30M:
					num = SetupForm.PAADC30;
					break;
				case Band.B20M:
					num = SetupForm.PAADC20;
					break;
				case Band.B17M:
					num = SetupForm.PAADC17;
					break;
				case Band.B15M:
					num = SetupForm.PAADC15;
					break;
				case Band.B12M:
					num = SetupForm.PAADC12;
					break;
				case Band.B10M:
					num = SetupForm.PAADC10;
					break;
			}

			if(num == 0) return 0;
			//return 100000 / Math.Pow(num, 2);
			return (double)108/num;
		}

		private double SWR(int adc_fwd, int adc_rev)
		{
			if(adc_fwd == 0 && adc_rev == 0)
				return 1.0;
			else if(adc_rev > adc_fwd)
				return 50.0;
			
			double Ef = ScaledVoltage(adc_fwd);
			double Er = ScaledVoltage(adc_rev);

			double swr = (Ef + Er)/(Ef - Er);

			return swr;
		}

		private double ScaledVoltage(int adc)
		{
			double v_det = adc * 0.062963;			// scale factor in V/bit including pot ratio
			double v_out = v_det * 10.39853;		// scale factor in V/V for bridge output to detector voltage
			return v_out*PABandOffset(CurrentBand);
			//double v_det = adc * 0.0304;
			//			double v_out = 0;
			//			if(v_det >= 1.6)
			//				v_out = (-0.241259304*v_det+12.07915098)*v_det*PABandOffset(CurrentBand);
			//			else if(v_det > 0.35)
			//				v_out = (1/Math.Pow(v_det, 2)+11.3025111)*v_det*PABandOffset(CurrentBand);
			//return v_out;
		}

		private double ADCtodBm(int adc_data)
		{
			if(adc_data == 0)
				return 0;

			double mult = 100000 / Math.Pow(225/PABandOffset(CurrentBand), 2);
			return 10*Math.Log10(mult*Math.Pow(adc_data, 2));
		}

		private double PAPower(int adc)
		{
			double v_out = ScaledVoltage(adc);
			double pow = Math.Pow(v_out, 2)/50;
			pow = Math.Max(pow, 0.0);
			return pow;
		}

		private double WattsTodBm(double watts)
		{
			return 10*Math.Log10(watts/0.001);
		}

		private double dBmToWatts(double dBm)
		{
			return Math.Pow(10, dBm/10)*0.001;
		}

		private static bool CheckForOpenProcesses()
		{
			// find all open PowerSDR processes
			Process[] p = Process.GetProcessesByName("PowerSDR");
			if(p.Length > 1)
			{
				DialogResult dr = MessageBox.Show("There are other PowerSDR instances running.\n"+
					"Are you sure you want to continue?",
					"Continue?",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question);
				if(dr == DialogResult.No)
				{
					return false;
				}
			}
			return true;
		}

		public int VersionTextToInt(string version)	// takes a version string like "1.0.6" 
		{											// and converts it to an int like 1060.
			int mult = 10000;
			string s = version;
			int retval = 0;

			while(s.IndexOf(".") >= 0)
			{
				int first_index = s.IndexOf(".");
				string temp = s.Substring(0, first_index);
				retval +=  mult * Int32.Parse(temp);
				s = s.Substring(first_index+1);
				mult /= 100;
			}

			retval += mult * Int32.Parse(s);

			return retval;
		}

		public void CheckForUpdates()
		{
			// notes taken from http://free.netartmedia.net/CSharp/CSharp5.html
			try
			{
				// Create a 'WebRequest' object with the specified url. 
				WebRequest myWebRequest = WebRequest.Create("http://www.flex-radio.com/PowerSDR_Update.txt"); 

				// Send the 'WebRequest' and wait for response. 
				WebResponse myWebResponse = myWebRequest.GetResponse(); 

				// Obtain a 'Stream' object associated with the response object. 
				Stream ReceiveStream = myWebResponse.GetResponseStream(); 

				// Pipe the stream to a higher level stream reader. 
				StreamReader readStream = new StreamReader(ReceiveStream);

				// Read the entire stream into a string object.
				string strResponse = readStream.ReadToEnd(); 

				// Release the resources. 
				readStream.Close();
				myWebResponse.Close();

				// Split the lines into a string array.
				string[] lines = strResponse.Split('\n');
				
				string temp = "";
				int begin_index = 0, end_index = 0, line_index = 0;
				int version_value = 0;

				// find first line of Beta release info
				for(int i=0; i<lines.Length; i++)
				{
					if(lines[i].StartsWith("========"))
					{
						line_index = i+1;
						break;
					}
				}

				// get current version value in integer form (i.e. 1.1.6 = 116)
				Assembly assembly = Assembly.GetExecutingAssembly();
				FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
				int current_version = VersionTextToInt(fvi.FileVersion);

				if(notify_on_release)
				{
					// find release version value in integer form.
					begin_index = 0;
					end_index = lines[0].IndexOf("Released");
					temp = lines[0].Substring(begin_index, end_index-begin_index);
					temp = temp.Replace("//", "");

					version_value = VersionTextToInt(temp);
					
					// if the new value is higher, display form
					if(version_value > current_version)
					{
						string s = "";
						// search for Bug Fixes
						for(int i=0; i<line_index; i++)
						{
							temp = lines[i].Replace("//", "").Trim();
							if(temp.StartsWith("Bug Fixes"))
							{
								s = "\n\n\nBug Fixes:\n\n";
								for(int j=i+1; j<line_index; j++)
								{
									temp = lines[j].Replace("//", "").Trim();
									if(temp.StartsWith("Issue:"))
									{
										temp = temp.Substring(temp.IndexOf(":")+1).Trim();
										while(!temp.EndsWith("."))
											temp += " "+lines[++j].Replace("//", "").Trim();
										s += temp + "\n";
									}
								}
								break;
							}
						}

						// search for Modifications
						for(int i=0; i<line_index; i++)
						{
							temp = lines[i].Replace("//", "").Trim();
							if(temp.StartsWith("Modifications"))
							{
								s += "\n\nModifications:\n\n";
								for(int j=i+1; j<line_index; j++)
								{
									temp = lines[j].Replace("//", "").Trim();
									if(temp.StartsWith("Feature:"))
									{
										temp = temp.Substring(temp.IndexOf(":")+1).Trim();
										while(!temp.EndsWith("."))
											temp += " "+lines[++j].Replace("//", "").Trim();
										s += temp + "\n";
									}
								}
								break;
							}
						}

						// search for New Features
						for(int i=0; i<line_index; i++)
						{
							temp = lines[i].Replace("//", "").Trim();
							if(temp.StartsWith("New Features"))
							{
								s += "\n\nNew Features:\n\n";
								for(int j=i+1; j<line_index; j++)
								{
									temp = lines[j].Replace("//", "").Trim();
									if(temp.StartsWith("Feature:"))
									{
										temp = temp.Substring(temp.IndexOf(":")+1).Trim();
										while(!temp.EndsWith("."))
											temp += " "+lines[++j].Replace("//", "").Trim();
										s += temp + "\n";
									}
								}
								break;
							}
						}

						MessageBox.Show("New Version: "+lines[0].Substring(2) + s);
					}
				}
				
				// if the line_index = 0, then the response was bad
				// or the file is not formatted correctly.
				if(line_index == 0)
					return;

				if(notify_on_beta)
				{
					int beta_line = line_index;

					// find beta version value in integer form.
					begin_index = lines[line_index].IndexOf(".")-1;
					end_index = lines[line_index].IndexOf("Released");
					temp = lines[line_index].Substring(begin_index, end_index-begin_index);
					temp = temp.Replace("//", "");
					
					version_value = VersionTextToInt(temp);
					
					// if the new value is higher, display form
					if(version_value > current_version)
					{
						string s = "";
						// search for Bug Fixes
						for(int i=line_index; i<lines.Length-1; i++)
						{
							temp = lines[i].Replace("//", "").Trim();
							if(temp.StartsWith("Bug Fixes"))
							{
								s = "\n\n\nBug Fixes:\n\n";
								for(int j=i+1; j<lines.Length-1; j++)
								{
									temp = lines[j].Replace("//", "").Trim();
									if(temp.StartsWith("Issue:"))
									{
										temp = temp.Substring(temp.IndexOf(":")+1).Trim();
										while(!temp.EndsWith("."))
											temp += " "+lines[++j].Replace("//", "").Trim();
										s += temp + "\n";
									}
								}
								line_index = i;
								break;
							}
							
						}
						

						// search for Modifications
						for(int i=line_index; i<lines.Length-1; i++)
						{
							temp = lines[i].Replace("//", "").Trim();
							if(temp.StartsWith("Modifications"))
							{
								s += "\n\nModifications:\n\n";
								for(int j=i+1; j<lines.Length-1; j++)
								{
									temp = lines[j].Replace("//", "").Trim();
									if(temp.StartsWith("Feature:"))
									{
										temp = temp.Substring(temp.IndexOf(":")+1).Trim();
										while(!temp.EndsWith("."))
											temp += " "+lines[++j].Replace("//", "").Trim();
										s += temp + "\n";
									}
								}
								line_index = i;
								break;
							}
						}

						// search for New Features
						for(int i=line_index; i<lines.Length-1; i++)
						{
							temp = lines[i].Replace("//", "").Trim();
							if(temp.StartsWith("New Features"))
							{
								s += "\n\nNew Features:\n\n";
								for(int j=i+1; j<lines.Length-1; j++)
								{
									temp = lines[j].Replace("//", "").Trim();
									if(temp.StartsWith("Feature:"))
									{
										temp = temp.Substring(temp.IndexOf(":")+1).Trim();
										while(!temp.EndsWith("."))
											temp += " "+lines[++j].Replace("//", "").Trim();
										s += temp + "\n";
									}
								}
								break;
							}
						}
						MessageBox.Show("New Beta Version: "+lines[beta_line].Substring(2) + s);
					}
				}				
				
			}
			catch(Exception)
			{
//#if(DEBUG)
//				MessageBox.Show("Error in CheckForUpdates() in console.cs\n\n"+e.Message);
//#endif
				// do nothing here as internet may just not be present
			}
		}

		public bool IsHamBand(BandPlan b, double f)
		{
			if(extended)
				return true;

			switch(b)
			{
				case BandPlan.IARU1:
					if(f >= 1.8 && f <= 2.0)			return true;
					else if(f >= 3.5 && f <= 4.0)		return true;
					else if(f == 5.3305)				return true;
					else if(f == 5.3465)				return true;
					else if(f == 5.3665)				return true;
					else if(f == 5.3715)				return true;
					else if(f == 5.4035)				return true;
					else if(f >= 7.0 && f <= 7.3)		return true;
					else if(f >= 10.1 && f <= 10.15)	return true;
					else if(f >= 14.0 && f <= 14.35)	return true;
					else if(f >= 18.068 && f <= 18.168) return true;
					else if(f >= 21.0 && f <= 21.45)	return true;
					else if(f >= 24.89 && f <= 24.99)	return true;
					else if(f >= 21.0 && f <= 21.45)	return true;
					else if(f >= 28.0 && f <= 29.7)		return true;
					else if(f >= 50.0 && f <= 54.0)		return true;
					else if(f >= 144.0 && f <= 146.0)
					{
						if(rfe_present && xvtr_present)
							return true;
						else
							return false;
					}
					else return false;
				default:
					return false;
					// TODO: Implement other bandplans here
			}
		}

		public void SetHWFilters(double freq)
		{
			if(rfe_present)							// RFE is present
			{
				//Use shift registers on RFE to control BPF and LPF banks
				if(freq <= 2.5)					// DC to 2.5MHz
				{
					if(pa_present)
						Hdw.PA_LPF = PAFBand.B160;

					Hdw.BPFRelay = BPFBand.B160;
					if(freq <= 0.3 && enable_LPF0)
						Hdw.RFE_LPF = RFELPFBand.AUX;													
					else
						Hdw.RFE_LPF = RFELPFBand.B160;
				}
				else if(freq <= 4)				// 2.5MHz to 4MHz
				{
					if(pa_present)
						Hdw.PA_LPF = PAFBand.B80;

					Hdw.BPFRelay = BPFBand.B60;
					Hdw.RFE_LPF = RFELPFBand.B80;
				}
				else if(freq <= 6)				// 4MHz to 6MHz
				{
					if(pa_present)
						Hdw.PA_LPF = PAFBand.B6040;

					Hdw.BPFRelay = BPFBand.B60;
					Hdw.RFE_LPF = RFELPFBand.B60;
				}
				else if(freq <= 7.3)			// 6MHz to 7.3MHz
				{
					if(pa_present)
						Hdw.PA_LPF = PAFBand.B6040;

					Hdw.BPFRelay = BPFBand.B40;
					Hdw.RFE_LPF = RFELPFBand.B40;
				}
				else if(freq <= 10.2)			// 7.3MHz to 10.2MHz
				{
					if(pa_present)
						Hdw.PA_LPF = PAFBand.B3020;

					Hdw.BPFRelay = BPFBand.B40;
					Hdw.RFE_LPF = RFELPFBand.B30;
				}
				else if(freq <= 12)				// 10.2MHz to 12MHz
				{
					if(pa_present)
						Hdw.PA_LPF = PAFBand.NONE;

					Hdw.BPFRelay = BPFBand.B40;
					Hdw.RFE_LPF = RFELPFBand.B30;
				}
				else if(freq <= 14.5)			// 12MHz to 14.5MHz
				{
					if(pa_present)
						Hdw.PA_LPF = PAFBand.B3020;

					Hdw.BPFRelay = BPFBand.B20;
					Hdw.RFE_LPF = RFELPFBand.B20;
				}
				else if(freq <= 21.5)			// 14.5MHz to 21.5MHz
				{
					if(pa_present)
						Hdw.PA_LPF = PAFBand.B1715;

					Hdw.BPFRelay = BPFBand.B20;
					Hdw.RFE_LPF = RFELPFBand.B1715;
				}
				else if(freq <= 24)				// 21.5MHz to 24MHz
				{
					if(pa_present)
						Hdw.PA_LPF = PAFBand.B1210;

					Hdw.BPFRelay = BPFBand.B20;
					Hdw.RFE_LPF = RFELPFBand.B1210;
				}
				else if(freq <= 30)				// 24MHz to 30MHz
				{
					if(pa_present)
						Hdw.PA_LPF = PAFBand.B1210;

					Hdw.BPFRelay = BPFBand.B10;
					Hdw.RFE_LPF = RFELPFBand.B1210;
				}
				else if(freq <= 36)				// 30MHz to 36MHz
				{
					if(pa_present)
						Hdw.PA_LPF = PAFBand.NONE;

					Hdw.BPFRelay = BPFBand.B10;
					Hdw.RFE_LPF = RFELPFBand.B6;
				}
				else if(freq <= 65)				// 36MHz to 65Mhz
				{
					if(pa_present)
						Hdw.PA_LPF = PAFBand.NONE;

					Hdw.BPFRelay = BPFBand.B6;
					Hdw.RFE_LPF = RFELPFBand.B6;
				}
				else if(xvtr_present && freq >= 144 && freq <= 146)			//28MHz IF for transverter
				{
					if(pa_present)
						Hdw.PA_LPF = PAFBand.NONE;

					Hdw.BPFRelay = BPFBand.B10;
					Hdw.RFE_LPF = RFELPFBand.B1210;
					Hdw.XVTR_RF = true;
				}
				if(xvtr_present && freq < 144)
					Hdw.XVTR_RF = false;
			}
			else									// RFE is not present
			{
				//Select the BPF relays using the high frequency cutoff
				if(freq < 2.5)					//DC to 2.5MHz
					Hdw.BPFRelay = BPFBand.B160;
				else if(freq < 6)				//2.5MHz to 6MHz
					Hdw.BPFRelay = BPFBand.B60;
				else if(freq < 12)				//6MHz to 12MHz
					Hdw.BPFRelay = BPFBand.B40;
				else if(freq < 24)				//12MHz to 24MHz
					Hdw.BPFRelay = BPFBand.B20;
				else if(freq < 36)				//24MHz to 36MHz
					Hdw.BPFRelay = BPFBand.B10;
				else								//36MHz to 65Mhz
					Hdw.BPFRelay = BPFBand.B6;
			}
		}

		// kb9yig sr40 mod 		
		// check and see if the band data includes alias data -- if so 
		// zero out (very negative) the portions of the data that are 
		// aliased 
		public void AdjustDisplayDataForBandEdge(ref float[] display_data) 
		{
			if ( current_model != Model.SOFTROCK40)  // -- no aliasing going on 
				return;   

			if ( current_dsp_mode == DSPMode.DRM )  // for now don't worry about aliasing in DRM land 
			{
				return; 
			}

			double hz_per_bin = DttSP.SampleRate/Display.BUFFER_SIZE; 
			double data_center_freq = tuned_freq; 
			if ( data_center_freq == 0 ) 
			{ 
				return; 
			} 
			double data_low_edge_hz = (1e6 * data_center_freq) - DttSP.SampleRate/2; 
			double data_high_edge_hz = (1e6 * data_center_freq) + DttSP.SampleRate/2; 
			double alias_free_low_edge_hz = (1e6 * soft_rock_center_freq) - DttSP.SampleRate/2; 
			double alias_free_high_edge_hz = (1e6 * soft_rock_center_freq) + DttSP.SampleRate/2; 
			if ( data_low_edge_hz < alias_free_low_edge_hz )   // data we have goes below alias free region -- zero it 
			{				
				double hz_this_bin = data_low_edge_hz; 
				int bin_num = 0; 
				while ( hz_this_bin < alias_free_low_edge_hz ) 
				{
					display_data[bin_num] = -200.0f; 
					++bin_num; 
					hz_this_bin += hz_per_bin; 
				}
				// Debug.WriteLine("data_low: " + bin_num); 
			} 
			else if ( data_high_edge_hz > alias_free_high_edge_hz ) 
			{ 				
				double hz_this_bin = data_high_edge_hz; 
				int bin_num = Display.BUFFER_SIZE - 1; 
				while ( hz_this_bin > alias_free_high_edge_hz ) 
				{
					display_data[bin_num] = -200.0f; 
					--bin_num; 
					hz_this_bin -= hz_per_bin; 
				}					
				// Debug.WriteLine("data_high: " + bin_num); 
			}
			return;		
		}
		// end kb9yig sr40 mod 

		private void SelectVarFilter() 
		{ 
			if ( current_filter == Filter.VAR1 ) return; 
			if ( current_filter == Filter.VAR2 ) return; 

			// save current filter bounds, reset to var, set filter bounds 
			decimal hi = udFilterHigh.Value;
			decimal lo = udFilterLow.Value; 
			radFilterVar1.Checked = true;
			SetFilter(Filter.VAR1); 
			udFilterHigh.Value = hi; 
			udFilterLow.Value = lo; 
		} 

		private void UpdateExtCtrl()
		{
			switch(CurrentBand)
			{
				case Band.B160M:
					if(!chkMOX.Checked)
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_160_rx);
					else
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_160_tx);
					break;
				case Band.B80M:
					if(!chkMOX.Checked)
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_80_rx);
					else
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_80_tx);
					break;
				case Band.B60M:
					if(!chkMOX.Checked)
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_60_rx);
					else
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_60_tx);
					break;
				case Band.B40M:
					if(!chkMOX.Checked)
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_40_rx);
					else
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_40_tx);
					break;
				case Band.B30M:
					if(!chkMOX.Checked)
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_30_rx);
					else
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_30_tx);
					break;
				case Band.B20M:
					if(!chkMOX.Checked)
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_20_rx);
					else
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_20_tx);
					break;
				case Band.B17M:
					if(!chkMOX.Checked)
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_17_rx);
					else
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_17_tx);
					break;
				case Band.B15M:
					if(!chkMOX.Checked)
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_15_rx);
					else
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_15_tx);
					break;
				case Band.B12M:
					if(!chkMOX.Checked)
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_12_rx);
					else
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_12_tx);
					break;
				case Band.B10M:
					if(!chkMOX.Checked)
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_10_rx);
					else
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_10_tx);
					break;
				case Band.B6M:
					if(!chkMOX.Checked)
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_6_rx);
					else
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_6_tx);
					break;
				case Band.B2M:
					if(!chkMOX.Checked)
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_2_rx);
					else
						Hdw.X2 = (byte)((Hdw.X2 & 0xC0) | x2_2_tx);
					break;
			}
		}

		// Added 06/24/05 BT for CAT commands
		public void CATMemoryQS()
		{
			btnMemoryQuickSave_Click(this.btnMemoryQuickSave, EventArgs.Empty);
		}

		// Added 06/25/05 BT for CAT commands
		public void CATMemoryQR()
		{
			btnMemoryQuickRestore_Click(this.btnMemoryQuickRestore, EventArgs.Empty);
		}

		// BT 06/30/05 Added for CAT commands
		public int CATBandGroup
		{
			get
			{
				if(grpBandHF.Visible)
					return 0;
				else
					return 1;
			}
			set
			{
				if(value == 0)
					btnBandHF_Click(this.btnBandHF, EventArgs.Empty);
				else if(value == 1)
					btnBandVHF_Click(this.btnBandVHF, EventArgs.Empty);
			}
		}

		//BT 06/17/05 added for CAT commands
		public void SetCATBand(Band pBand)
		{
			Band b = pBand;
			switch(b)
			{
				case Band.B160M:
					btnBand160_Click(this.btnBand160, EventArgs.Empty);
					break;
				case Band.B80M:
					btnBand80_Click(this.btnBand80, EventArgs.Empty);
					break;
				case Band.B60M:
					btnBand60_Click(this.btnBand60, EventArgs.Empty);
					break;
				case Band.B40M:
					btnBand40_Click(this.btnBand40, EventArgs.Empty);
					break;
				case Band.B30M:
					btnBand30_Click(this.btnBand30, EventArgs.Empty);
					break;
				case Band.B20M:
					btnBand20_Click(this.btnBand20, EventArgs.Empty);
					break;
				case Band.B17M:
					btnBand17_Click(this.btnBand17, EventArgs.Empty);
					break;
				case Band.B15M:
					btnBand15_Click(this.btnBand15, EventArgs.Empty);
					break;
				case Band.B12M:
					btnBand12_Click(this.btnBand12, EventArgs.Empty);
					break;
				case Band.B10M:
					btnBand10_Click(this.btnBand10, EventArgs.Empty);
					break;
				case Band.B6M:
					btnBand6_Click(this.btnBand6, EventArgs.Empty);
					break;
				case Band.B2M:
					btnBand2_Click(this.btnBand2, EventArgs.Empty);
					break;
				case Band.GEN:
					btnBandGEN_Click(this.btnBandGEN, EventArgs.Empty);
					break;
				case Band.WWV:
					btnBandWWV_Click(this.btnBandWWV, EventArgs.Empty);
					break;
				default:
					btnBandGEN_Click(this.btnBandGEN, EventArgs.Empty);
					break;
			}
		}

		public void SetVHFText(int index, string text)
		{
			vhf_text[index].Text = text;
		}

		public void SetVHFEnabled(int index, bool b)
		{
			vhf_text[index].Enabled = b;
		}

		private void UpdateBandStackRegisters()
		{
			int[] band_stacks = DB.GetBandStackNum();
			band_160m_register = band_stacks[0];
			band_80m_register = band_stacks[1];
			band_60m_register = band_stacks[2];
			band_40m_register = band_stacks[3];
			band_30m_register = band_stacks[4];
			band_20m_register = band_stacks[5];
			band_17m_register = band_stacks[6];
			band_15m_register = band_stacks[7];
			band_12m_register = band_stacks[8];
			band_10m_register = band_stacks[9];
			band_6m_register = band_stacks[10];
			band_2m_register = band_stacks[11];
			band_wwv_register = band_stacks[12];
			band_gen_register = band_stacks[13];
			band_vhf0_register = band_stacks[14];
			band_vhf1_register = band_stacks[15];
			band_vhf2_register = band_stacks[16];
			band_vhf3_register = band_stacks[17];
			band_vhf4_register = band_stacks[18];
			band_vhf5_register = band_stacks[19];
			band_vhf6_register = band_stacks[20];
			band_vhf7_register = band_stacks[21];
			band_vhf8_register = band_stacks[22];
			band_vhf9_register = band_stacks[23];
			band_vhf10_register = band_stacks[24];
			band_vhf11_register = band_stacks[25];
			band_vhf12_register = band_stacks[26];
			band_vhf13_register = band_stacks[27];
		}

		#endregion

		#region Test and Calibration Routines

		public bool CalibrateFreq(float freq)
		{
			bool retval = false;
			bool rx_only = SetupForm.RXOnly;
			SetupForm.RXOnly = true;

			if(!chkPower.Checked)
			{
				MessageBox.Show("Power must be on in order to calibrate.", "Power Is Off",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}

			string vfo_freq_text = txtVFOAFreq.Text;		// save current frequency

			bool polyphase = SetupForm.Polyphase;			// save current polyphase setting
			SetupForm.Polyphase = false;					// disable polyphase

			int dsp_buf_size = SetupForm.DSPBufferSize;		// save current DSP buffer size
			SetupForm.DSPBufferSize = 2048;					// set DSP Buffer Size to 2048

			DSPMode dsp_mode = current_dsp_mode;			// save current demod mode
			CurrentDSPMode = DSPMode.AM;					// set DSP to AM

			bool rit_on = chkRIT.Checked;					// save current RIT state
			chkRIT.Checked = false;							// set RIT to Off

			int rit_value = (int)udRIT.Value;				// save current RIT value
			udRIT.Value = 0;								// set RIT Value to 0

			Filter filter = CurrentFilter;					// save current filter
			CurrentFilter = Filter.F6000;					// set filter to 2600Hz

			VFOAFreq = freq;								// set frequency to passed value

			Thread.Sleep(500);
			//int ret = 0;

			fixed(float* ptr = &Display.new_display_data[0])
				DttSP.GetSpectrum(ptr);		// get the spectrum values


			fixed(float* ptr = &Display.new_display_data[0])
				DttSP.GetSpectrum(ptr);		// get the spectrum values


			float max = float.MinValue;
			float avg = 0;

			int max_index = 0;
			int low = Display.BUFFER_SIZE>>1;
			int high = low;
			low  += (int)((DttSP.RXDisplayLow * Display.BUFFER_SIZE) / DttSP.SampleRate);
			high += (int)((DttSP.RXDisplayHigh * Display.BUFFER_SIZE) / DttSP.SampleRate);

			for(int i=low; i<high; i++)						// find the maximum signal
			{
				avg += Display.new_display_data[i];
				if(Display.new_display_data[i] > max)
				{
					max = Display.new_display_data[i];
					max_index = i;
				}
			}
			avg /= (high - low);

			if(max < avg + 30)
			{
				MessageBox.Show("Peak is less than 30dB from the noise floor.  " +
					"Please use a larger signal for frequency calibration.",
					"Calibration Error - Weak Signal",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				retval = false;
				goto end;
			}

			// Calculate the difference between the known signal and the measured signal
			float diff = (float)(DttSP.SampleRate / Display.BUFFER_SIZE * ((Display.BUFFER_SIZE>>1) - max_index));

			// Calculate the DDS offset
			int offset = (int)(200.0/ freq * diff);

			int current_clock = SetupForm.ClockOffset;
			if((Math.Max(current_clock + offset, current_clock - offset) > 40000) ||
				(Math.Min(current_clock + offset, current_clock - offset) < -40000))
			{
				MessageBox.Show("Peak is outside valid range.",
					"Calibration Error - Range",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				retval = false;
				goto end;
			}

			if((Math.Max(current_clock + offset, current_clock - offset) > 20000) ||
				(Math.Min(current_clock + offset, current_clock - offset) < -20000))
			{
				DialogResult dr = MessageBox.Show("This value is outside the specifications of the oscillator.  " +
					"Do you want to use this value?",
					"Outside Specifications",
					MessageBoxButtons.YesNo);
				if(dr == DialogResult.No)
				{
					retval = false;
					goto end;
				}
			}

			SetupForm.ClockOffset += offset;				// Offset the clock based on the difference
			retval = true;

		end:
			SetupForm.RXOnly = rx_only;						// restore RX Only setting
			CurrentDSPMode = dsp_mode;						// restore DSP mode
			CurrentFilter = filter;							// restore filter
			chkRIT.Checked = rit_on;						// restore RIT state
			RITValue = rit_value;							// restore RIT value
			VFOAFreq = float.Parse(vfo_freq_text);			// restore frequency
			SetupForm.DSPBufferSize = dsp_buf_size;			// restore DSP buffer size
			SetupForm.Polyphase = polyphase;				// restore polyphase

			return retval;
		}

		public bool CalibrateLevel(float level, float freq, Progress progress)
		{
			// Calibration routine called by Setup Form.
			bool ret_val = false;
			if(!chkPower.Checked)
			{
				MessageBox.Show("Power must be on in order to calibrate.", "Power Is Off",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}

			bool rx_only = SetupForm.RXOnly;					// Save RX Only Setting
			SetupForm.RXOnly = true;

			int screen_delay = display_delay;					// save current FPS
			display_delay = 1000;								// Set FPS to 1

			double vfoa = VFOAFreq;								// save current VFOA

			bool polyphase = SetupForm.Polyphase;				// save current polyphase setting
			SetupForm.Polyphase = false;						// disable polyphase

			int dsp_buf_size = SetupForm.DSPBufferSize;			// save current DSP buffer size
			SetupForm.DSPBufferSize = 2048;						// set DSP Buffer Size to 2048

			string display_mode = comboDisplayMode.Text;		// save current display mode
			comboDisplayMode.Text = "Spectrum";					// set display mode to spectrum

			DSPMode dsp_mode = current_dsp_mode;				// save current DSP demod mode
			CurrentDSPMode = DSPMode.CWU;						// set mode to CWU

			VFOAFreq = freq;									// set VFOA frequency

			Filter filter = CurrentFilter;						// save current filter
			CurrentFilter = Filter.F500;						// set filter to 500Hz

			PreampMode preamp = CurrentPreampMode;				// save current preamp mode
			CurrentPreampMode = PreampMode.HIGH;				// set to medium

			MeterRXMode rx_meter = CurrentMeterRXMode;			// save current RX Meter mode
			CurrentMeterRXMode = MeterRXMode.OFF;				// turn RX Meter off

			bool display_avg = DisplayAVG;						// save current average state
			DisplayAVG = false;									// set average state to off

			DisableAllFilters();
			DisableAllModes();
			VFOLock = true;
			comboPreamp.Enabled = false;
			comboDisplayMode.Enabled = false;

			progress.SetPercent(0.0f);
			int counter = 0;

			int buf_time = (int)((double)block_size1/(double)sample_rate1*1000);
			Thread.Sleep(100);

			float num=0.0f, avg=0.0f, num2=0.0f, avg2=0.0f;
			// get the value of the signal strength meter
			for(int i=0; i<50; i++)
			{
				num += DttSP.CalculateMeter(DttSP.MeterType.SIGNAL_STRENGTH);
				Thread.Sleep(50);
				if(!progress.Visible)
					goto end;
				else progress.SetPercent((float)((float)++counter/160));
			}
			avg = num / 50.0f;

			if(rfe_present)
			{
				CurrentPreampMode = PreampMode.MED;
				Thread.Sleep(100);
								
				// get the value of the signal strength meter
				num2=0.0f;
				for(int i=0; i<50; i++)
				{
					num2 += DttSP.CalculateMeter(DttSP.MeterType.SIGNAL_STRENGTH);
					Thread.Sleep(50);
					if(!progress.Visible)
						goto end;
					else progress.SetPercent((float)((float)++counter/160));
				}
				avg2 = num2 / 50.0f;

				float att_offset = avg2 - avg;

				CurrentPreampMode = PreampMode.LOW;
				Thread.Sleep(100);

				// get the value of the signal strength meter
				num2=0.0f;
				for(int i=0; i<50; i++)
				{
					num2 += DttSP.CalculateMeter(DttSP.MeterType.SIGNAL_STRENGTH);
					Thread.Sleep(50);
					if(!progress.Visible)
						goto end;
					else progress.SetPercent((float)((float)++counter/160));
				}
				avg2 = num2 / 50.0f;

				float gain_offset = avg2 - avg;

				preamp_offset[(int)PreampMode.OFF] = -gain_offset - att_offset;
				preamp_offset[(int)PreampMode.LOW] = -gain_offset;
				preamp_offset[(int)PreampMode.MED] = -att_offset;
				preamp_offset[(int)PreampMode.HIGH] = 0.0f;
			}
			else
			{
				CurrentPreampMode = PreampMode.HIGH;
				Thread.Sleep(100);

				// get the value of the signal strength meter
				num2=0.0f;
				for(int i=0; i<50; i++)
				{
					num2 += DttSP.CalculateMeter(DttSP.MeterType.SIGNAL_STRENGTH);
					Thread.Sleep(50);
					if(!progress.Visible)
						goto end;
					else progress.SetPercent((float)((float)++counter/160));
					counter++;
				}
				avg2 = num2 / 50.0f;

				float gain_offset = avg2 - avg;

				preamp_offset[(int)PreampMode.MED] = gain_offset;
				preamp_offset[(int)PreampMode.HIGH] = 0.0f;
			}

			CurrentPreampMode = PreampMode.HIGH;
			Thread.Sleep(100);
			
			num2 = 0.0f;
			for(int i=0; i<10; i++)
			{
				fixed(float* ptr = &Display.new_display_data[0])
					DttSP.GetSpectrum(ptr);		// read again to clear out changed DSP

				float max = float.MinValue;						// find the max spectrum value
				for(int j=0; j<Display.BUFFER_SIZE; j++)
					if(Display.new_display_data[j] > max) max = Display.new_display_data[j];

				num2 += max;

				Thread.Sleep(50);

				if(!progress.Visible)
					goto end;
				else progress.SetPercent((float)((float)++counter/160));
			}
			avg2 = num2 / 10.0f;

			// calculate the difference between the current value and the correct multimeter value
			float diff = level - (avg + multimeter_cal_offset + preamp_offset[(int)current_preamp_mode]);
			multimeter_cal_offset += diff;

			// calculate the difference between the current value and the correct spectrum value
			diff = level - (avg2 + display_cal_offset + preamp_offset[(int)current_preamp_mode]);
			display_cal_offset += diff;		

			ret_val = true;

		end:
			progress.Hide();
			EnableAllFilters();
			EnableAllModes();
			VFOLock = false;
			comboPreamp.Enabled = true;
			comboDisplayMode.Enabled = true;

			SetupForm.RXOnly = rx_only;							// restore RX Only
			display_delay = screen_delay;						// restore FPS
			comboDisplayMode.Text = display_mode;				// restore display mode
			DisplayAVG = display_avg;							// restore AVG value
			CurrentPreampMode = preamp;							// restore preamp value
			CurrentDSPMode = dsp_mode;							// restore DSP mode
			CurrentFilter = filter;								// restore filter
			if(dsp_buf_size != 2048)
				chkPower.Checked = false;						// go to standby
			SetupForm.DSPBufferSize = dsp_buf_size;				// restore DSP Buffer Size
			VFOAFreq = vfoa;									// restore vfo frequency
			if(dsp_buf_size != 2048)
			{
				Thread.Sleep(1000);
				chkPower.Checked = true;
			}
			CurrentMeterRXMode = rx_meter;						// restore RX Meter mode
			SetupForm.Polyphase = polyphase;					// restore polyphase

			//			Debug.WriteLine("multimeter_cal_offset: "+multimeter_cal_offset);
			//			Debug.WriteLine("display_cal_offset: "+display_cal_offset);
			//			MessageBox.Show("multimeter_cal_offset: "+multimeter_cal_offset.ToString()+"\n"+
			//				"display_cal_offset: "+display_cal_offset.ToString());
			return ret_val;
		}

		public bool CalibrateImage(float freq, Progress progress)
		{
			//			HiPerfTimer t1 = new HiPerfTimer();
			//			t1.Start();
	
			// Setup Rig for Image Null Cal
			bool ret_val = false;

			if(!chkPower.Checked)
			{
				MessageBox.Show("Power must be on in order to calibrate.", "Power Is Off",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}

			bool rx_only = SetupForm.RXOnly;				// save RX Only Setting
			SetupForm.RXOnly = true;

			int screen_delay = display_delay;				// save current FPS
			display_delay = 333;

			bool polyphase = SetupForm.Polyphase;			// save current polyphase setting
			SetupForm.Polyphase = false;					// disable polyphase

			int dsp_buf_size = SetupForm.DSPBufferSize;		// save current DSP buffer size
			SetupForm.DSPBufferSize = 2048;					// set DSP Buffer Size to 2048

			string display_mode = comboDisplayMode.Text;	// save current display mode
			if(display_mode != "Off")
				comboDisplayMode.Text = "Spectrum";			// set display mode to spectrum

			DSPMode dsp_mode = current_dsp_mode;			// save current dsp mode
			CurrentDSPMode = DSPMode.AM;					// set dsp mode to AM

			Filter filter = current_filter;					// save current filter
			CurrentFilter = Filter.F6000;					// set filter to 6kHz

			double vfo_freq = VFOAFreq;						// save current frequency
			VFOAFreq = freq+0.022050f;						// set frequency to passed value + 22kHz

			bool avg = DisplayAVG;								// save current average state
			DisplayAVG = false;									// set average state to off

			DisableAllFilters();
			DisableAllModes();
			VFOLock = true;
			comboPreamp.Enabled = false;
			comboDisplayMode.Enabled = false;

			SetupForm.ImageGainRX = -500.0;
			SetupForm.ImagePhaseRX = -400.0;

			float[] a = new float[Display.BUFFER_SIZE];
			float[] init_max = new float[4];

			//int retval = 0;
			progress.SetPercent(0.0f);
			int counter = 0;

			Thread.Sleep(200);
			
			fixed(float* ptr = &a[0])
				DttSP.GetSpectrum(ptr);// get the spectrum values

			
			Thread.Sleep(45);
			
			fixed(float* ptr = &a[0])
				DttSP.GetSpectrum(ptr);// get the spectrum values

			Thread.Sleep(45);

			float max_signal = float.MinValue;				// find the signal value
			int peak_bin = -1;

			// find peak bin
			for(int j=0; j<Display.BUFFER_SIZE; j++)
			{
				if(a[j] > max_signal)
				{
					peak_bin = j;
					max_signal = a[j];
				}
			}

			SetupForm.ImageGainRX = 0.0;
			SetupForm.ImagePhaseRX = 0.0;

			// Find starting point for Phase
			for(int i=0; i<4; i++)
			{
				// Take 4 values on either side of likely null
				double val = 0.0;

				switch(i)
				{
					case 0: val = -100.0;
						break;
					case 1: val = -25.0;
						break;
					case 2: val = 25.0;
						break;
					case 3: val = 100.0;
						break;
				}			
				
				SetupForm.ImagePhaseRX = val;
				Thread.Sleep(200);
			
				fixed(float* ptr = &a[0])
					DttSP.GetSpectrum(ptr);// get the spectrum values
			
				Thread.Sleep(45);
			
				fixed(float* ptr = &a[0])
					DttSP.GetSpectrum(ptr);// get the spectrum values


				init_max[i] = a[peak_bin];
			}

			// Calculate Phase line coefficients
			double m1 = (init_max[1] - init_max[0])/(-25.0 - -100.0);	// should be negative
			double b1 = init_max[0] - m1*(-100.0);
			double m2 = (init_max[3] - init_max[2])/(100.0 - 25.0);		// should be positive
			double b2 = init_max[3] - m2*(100.0);

			if(m1 > 0 || m2 < 0)
			{
				MessageBox.Show("Error calculating image null.  Please try again.",
					"Image Null Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				goto end;
			}

			// Calculate line intersection for phase starting point
			double phase_intercept = (b2-b1)/(m1-m2);
			Debug.WriteLine("phase intercept: "+phase_intercept.ToString("f4"));
			SetupForm.ImagePhaseRX = 0.0;


			// Find starting point for Gain
			for(int i=0; i<4; i++)
			{
				// Take 4 values on either side of likely null
				double val = 0.0;

				switch(i)
				{
					case 0: val = -100.0;
						break;
					case 1: val = -25.0;
						break;
					case 2: val = 25.0;
						break;
					case 3: val = 100.0;
						break;
				}			
				
				SetupForm.ImageGainRX = val;
				Thread.Sleep(200);
				
				fixed(float* ptr = &a[0])
					DttSP.GetSpectrum(ptr);// get the spectrum values
			
				Thread.Sleep(45);

				fixed(float* ptr = &a[0])
					DttSP.GetSpectrum(ptr);// get the spectrum values


				init_max[i] = a[peak_bin];
			}

			// Calculate Gain line coefficients
			m1 = (init_max[1] - init_max[0])/(-25.0 - -100.0);	// should be negative
			b1 = init_max[0] - m1*(-100.0);
			m2 = (init_max[3] - init_max[2])/(100.0 - 25.0);		// should be positive
			b2 = init_max[3] - m2*(100.0);

			if(m1 > 0 || m2 < 0)
			{
				MessageBox.Show("Error calculating image null.  Please try again.",
					"Image Null Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				goto end;
			}

			// Calculate line intersection for Gain starting point
			double gain_intercept = (b2-b1)/(m1-m2);
			Debug.WriteLine("gain_intercept: "+gain_intercept.ToString("f4"));
			SetupForm.ImageGainRX = 0.0;


			bool progressing = true;
			double phase_step = 5;
			double gain_step = 3;
			double phase_range = 80;
			double gain_range = 100;
			double low_phase, high_phase, low_gain, high_gain;
			double phase_index = phase_intercept;
			double gain_index = gain_intercept;
			double global_min_phase = phase_intercept;
			double global_min_gain = gain_intercept;
			double global_min_value = float.MaxValue;

			while(progressing)
			{
				// find minimum of the peak signal over 
				// the range of Phase settings

				low_phase = global_min_phase - phase_range/2;
				high_phase = global_min_phase + phase_range/2;
				float min_signal = float.MaxValue;
				for(double i=(int)Math.Max(low_phase, -400.0); i<high_phase && i<=400.0; i+=phase_step)
				{
					SetupForm.ImagePhaseRX = i;				// phase slider
					Thread.Sleep(100);
				
					fixed(float* ptr = &a[0])
						DttSP.GetSpectrum(ptr);// get the spectrum values

					Thread.Sleep(45);				
					
					fixed(float* ptr = &a[0])
						DttSP.GetSpectrum(ptr);// get the spectrum values

					if(a[peak_bin] < min_signal)			// if image is less than minimum
					{
						min_signal = a[peak_bin];			// save new minimum
						phase_index = i;					// save phase index
					}

					if(!progress.Visible)
						goto end;
					else progress.SetPercent((float)((float)counter++/1000));
				}	

				if(min_signal < global_min_value)
				{
					global_min_value = min_signal;
					global_min_phase = phase_index;
					global_min_gain = gain_index;
				}

				SetupForm.ImagePhaseRX = global_min_phase;			//set phase slider to min found

				// find minimum of the peak signal over 
				// the range of Gain settings

				low_gain = global_min_gain - gain_range/2;
				high_gain = global_min_gain + gain_range/2;

				min_signal = float.MaxValue;
				for(double i=(int)Math.Max(low_gain, -500.0); i<high_gain && i<=500.0; i+=gain_step)
				{
					SetupForm.ImageGainRX = i;				//set gain slider
					Thread.Sleep(100);
					
					fixed(float* ptr = &a[0])
						DttSP.GetSpectrum(ptr);// get the spectrum values

					Thread.Sleep(45);

					fixed(float* ptr = &a[0])
						DttSP.GetSpectrum(ptr);// get the spectrum values

					if(a[peak_bin] < min_signal)			// if image is less than minimum
					{
						min_signal = a[peak_bin];			// save new minimum
						gain_index = i;						// save phase index
					}	

					if(!progress.Visible)
						goto end;
					else progress.SetPercent((float)((float)counter++/1000));
				}

				if(min_signal < global_min_value)
				{
					global_min_value = min_signal;
					global_min_phase = phase_index;
					global_min_gain = gain_index;
				}

				SetupForm.ImageGainRX = global_min_gain;			//set gain slider to min found
	
				// narrow search range and use more steps
				phase_step /= 2.0; if(phase_step < 0.01) phase_step = 0.01;
				phase_range /= 2.0; if(phase_range < phase_step*4.0) phase_range = phase_step*4.0;
				gain_step /= 2.0; if(gain_step < 0.01) gain_step = 0.01;
				gain_range /= 2.0; if(gain_range < gain_step*4.0) gain_range = gain_step*4.0;

				// stop when range and step are 1 for gain and phase
				if(phase_range == 0.04 && phase_step == 0.01 &&
					gain_step == 0.01 && gain_range == 0.04)
					progressing = false;
			}

			// Finish the algorithm and reset the values
			ret_val = true;			

		end:
			progress.Hide();

			EnableAllFilters();
			EnableAllModes();
			VFOLock = false;
			comboPreamp.Enabled = true;
			comboDisplayMode.Enabled = true;

			SetupForm.RXOnly = rx_only;							// restore RX Only setting
			comboDisplayMode.Text = display_mode;				// restore display mode
			CurrentDSPMode = dsp_mode;							// restore dsp mode
			CurrentFilter = filter;								// restore filter
			VFOAFreq = vfo_freq;								// restore frequency
			txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			display_delay = screen_delay;						// restore FPS
			DisplayAVG = avg;									// restore average state
			SetupForm.DSPBufferSize = dsp_buf_size;				// restore DSP Buffer Size
			SetupForm.Polyphase = polyphase;					// restore polyphase

			//			t1.Stop();
			//			MessageBox.Show(t1.Duration.ToString());
			return ret_val;
		}

		public bool CalibratePAGain(Progress progress, bool[] run, int target_watts) // calibrate PA Gain values
		{
			//			HiPerfTimer t1 = new HiPerfTimer();
			//			t1.Start();

			bool ret_val = false;

			if(!chkPower.Checked)
			{
				MessageBox.Show("Power must be on in order to calibrate.", "Power Is Off",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}

			calibrating = true;

			int screen_delay = display_delay;				// save current FPS
			display_delay = 333;

			string display_mode = comboDisplayMode.Text;	// save current display mode
			comboDisplayMode.Text = "Off";			// set display mode to off

			DSPMode dsp_mode = current_dsp_mode;			// save current dsp mode
			CurrentDSPMode = DSPMode.USB;					// set dsp mode to CWL

			double vfo_freq = VFOAFreq;						// save current frequency

			int pwr = (int)udPWR.Value;						// save current pwr level

			int multimeter_delay = meter_delay;				// save current meter delay
			meter_delay = 1000;								// set meter delay to 1 second

			DisableAllFilters();
			DisableAllModes();
			VFOLock = true;
			comboPreamp.Enabled = false;
			comboDisplayMode.Enabled = false;

			int on_time = 2500;
			int off_time = 2500;

			switch(current_soundcard)
			{
				case SoundCard.AUDIGY_2_ZS:
					on_time = 3000;
					off_time = 4000;
					break;
				case SoundCard.DELTA_44:
					on_time = 2000;
					off_time = 2000;
					break;
			}

			progress.SetPercent(0.0f);

			float[] band_freqs = { 1.9f, 3.75f, 5.3665f, 7.15f, 10.125f, 14.175f, 18.1f, 21.225f, 24.9f, 28.85f };
				
			if(run[0]) SetupForm.PAGain160 = 49.0f;
			if(run[1]) SetupForm.PAGain80 = 49.0f;
			if(run[2]) SetupForm.PAGain60 = 49.0f;
			if(run[3]) SetupForm.PAGain40 = 49.0f;
			if(run[4]) SetupForm.PAGain30 = 49.0f;
			if(run[5]) SetupForm.PAGain20 = 49.0f;
			if(run[6]) SetupForm.PAGain17 = 49.0f;
			if(run[7]) SetupForm.PAGain15 = 49.0f;
			if(run[8]) SetupForm.PAGain12 = 49.0f;
			if(run[9]) SetupForm.PAGain10 = 49.0f;

			for(int i=0; i<band_freqs.Length; i++)
			{
				if(run[i])
				{
					int error_count = 0;
					VFOLock = false;
					VFOAFreq = band_freqs[i];				// set frequency
					VFOLock = true;
					udPWR.Value = target_watts;
					int target = (int)udPWR.Value;

					bool good_result = false;
					while(good_result == false)
					{
						Audio.CurrentAudioState1 = Audio.AudioState.SINL_COSR;

						chkMOX.Checked = true;
						for(int j=0; j < on_time/100; j++)
						{
							Thread.Sleep(100);
							if(!progress.Visible)
								goto end;
						}

						double watts = 0;
						pa_power_mutex.WaitOne();
						watts = PAPower(pa_fwd_power);
						pa_power_mutex.ReleaseMutex();
						chkMOX.Checked = false;
						
						Audio.CurrentAudioState1 = Audio.AudioState.DTTSP;

						Debug.WriteLine("watts: "+watts.ToString());

						if(!progress.Visible)
							goto end;

						if(Math.Abs(watts-target) > 4)
						{
							// convert to dBm
							float diff_dBm = (float)Math.Round((WattsTodBm(watts) - WattsTodBm((double)target)), 1);	

							switch(i)										// fix gain value
							{
								case 0:
									if(SetupForm.PAGain160 + diff_dBm < 38.0)
									{	
										if(++error_count > 6)
											goto error;

										SetupForm.PAGain160 = (float)Math.Max(38.0, SetupForm.PAGain160-2.0);
									}
									else SetupForm.PAGain160 += diff_dBm;
									break;
								case 1:
									if(SetupForm.PAGain80 + diff_dBm < 38.0)
									{
										if(++error_count > 6)
											goto error;

										SetupForm.PAGain80 = (float)Math.Max(38.0, SetupForm.PAGain80-2.0);
									}
									else SetupForm.PAGain80 += diff_dBm;
									break;
								case 2:
									if(SetupForm.PAGain60 + diff_dBm < 38.0)
									{
										if(++error_count > 6)
											goto error;

										SetupForm.PAGain60 = (float)Math.Max(38.0, SetupForm.PAGain60-2.0);
									}
									else SetupForm.PAGain60 += diff_dBm;
									break;
								case 3:
									if(SetupForm.PAGain40 + diff_dBm < 38.0)
									{
										if(++error_count > 6)
											goto error;

										SetupForm.PAGain40 = (float)Math.Max(38.0, SetupForm.PAGain40-2.0);
									}
									else SetupForm.PAGain40 += diff_dBm;
									break;
								case 4:
									if(SetupForm.PAGain30 + diff_dBm < 38.0)
									{
										if(++error_count > 6)
											goto error;

										SetupForm.PAGain30 = (float)Math.Max(38.0, SetupForm.PAGain30-2.0);
									}
									else SetupForm.PAGain30 += diff_dBm;
									break;
								case 5:
									if(SetupForm.PAGain20 + diff_dBm < 38.0)
									{
										if(++error_count > 6)
											goto error;

										SetupForm.PAGain20 = (float)Math.Max(38.0, SetupForm.PAGain20-2.0);
									}
									else SetupForm.PAGain20 += diff_dBm;
									break;
								case 6:
									if(SetupForm.PAGain17 + diff_dBm < 38.0)
									{
										if(++error_count > 6)
											goto error;

										SetupForm.PAGain17 = (float)Math.Max(38.0, SetupForm.PAGain17-2.0);
									}
									else SetupForm.PAGain17 += diff_dBm;
									break;
								case 7:
									if(SetupForm.PAGain15 + diff_dBm < 38.0)
									{
										if(++error_count > 6)
											goto error;

										SetupForm.PAGain15 = (float)Math.Max(38.0, SetupForm.PAGain15-2.0);
									}
									else SetupForm.PAGain15 += diff_dBm;
									break;
								case 8:
									if(SetupForm.PAGain12 + diff_dBm < 38.0)
									{
										if(++error_count > 6)
											goto error;

										SetupForm.PAGain12 = (float)Math.Max(38.0, SetupForm.PAGain12-2.0);
									}
									else SetupForm.PAGain12 += diff_dBm;
									break;
								case 9:
									if(SetupForm.PAGain10 + diff_dBm < 38.0)
									{
										if(++error_count > 6)
											goto error;

										SetupForm.PAGain10 = (float)Math.Max(38.0, SetupForm.PAGain10-2.0);
									}
									else SetupForm.PAGain10 += diff_dBm;
									break;
							}						
						}
						else good_result = true;
						for(int j=0; j < off_time/100; j++)
						{
							Thread.Sleep(100);
							if(!progress.Visible)
								goto end;
						}
					}	
				}
				progress.SetPercent((float)((float)(i+1)/10));
			}

			ret_val = true;
			
			end:
				progress.Hide();

			EnableAllFilters();
			EnableAllModes();
			VFOLock = false;
			comboPreamp.Enabled = true;
			comboDisplayMode.Enabled = true;

			chkMOX.Checked = false;
			Audio.CurrentAudioState1 = Audio.AudioState.DTTSP;
			display_delay = screen_delay;						// restore FPS
			comboDisplayMode.Text = display_mode;				// restore display mode
			CurrentDSPMode = dsp_mode;							// restore dsp mode
			VFOAFreq = vfo_freq;								// restore frequency
			txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			udPWR.Value = pwr;									// restore pwr level
			meter_delay = multimeter_delay;						// restore meter delay

			calibrating = false;

			//t1.Stop();
			//MessageBox.Show(t1.Duration.ToString());
			return ret_val;

			error:
				MessageBox.Show("Calculated gain is invalid.  Please double check connections and try again.\n" +
					"If this problem persists, contact eric@flex-radio.com for support.",
					"Invalid Gain Found",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			goto end;
		}

		public bool LowPowerPASweep(Progress progress, int power) // calibrate PA Gain values
		{
			//			HiPerfTimer t1 = new HiPerfTimer();
			//			t1.Start();

			bool ret_val = false;

			if(!chkPower.Checked)
			{
				MessageBox.Show("Power must be on in order to calibrate.", "Power Is Off",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}

			calibrating = true;

			int screen_delay = display_delay;				// save current FPS
			display_delay = 333;

			DSPMode dsp_mode = CurrentDSPMode;				// save current DSP Mode
			CurrentDSPMode = DSPMode.USB;					// set DSP Mode to USB

			string display_mode = comboDisplayMode.Text;	// save current display mode
			comboDisplayMode.Text = "Off";			// set display mode to off

			double vfo_freq = VFOAFreq;						// save current frequency
			
			int pwr = (int)udPWR.Value;						// save current pwr level
			udPWR.Value = power;							// set pwr level to 100W

			int multimeter_delay = meter_delay;				// save current meter delay
			meter_delay = 1000;								// set meter delay to 1 seconds

			progress.SetPercent(0.0f);

			float[] band_freqs = { 1.9f, 3.75f, 5.3665f, 7.15f, 10.125f, 14.175f, 18.1f, 21.225f, 24.9f, 28.85f };
				
			for(int i=0; i<band_freqs.Length; i++)
			{
				VFOAFreq = band_freqs[i];				// set frequency
				Audio.CurrentAudioState1 = Audio.AudioState.SINL_COSR;
				chkMOX.Checked = true;				
				for(int j=0; j<30; j++)
				{
					Thread.Sleep(100);
					if(!progress.Visible)
						goto end;
				}
				chkMOX.Checked = false;
				Audio.CurrentAudioState1 = Audio.AudioState.DTTSP;

				if(!progress.Visible)
					goto end;

				for(int j=0; j<40; j++)
				{
					Thread.Sleep(100);
					if(!progress.Visible)
						goto end;
				}
				if(!progress.Visible)
					goto end;				

				progress.SetPercent((float)((float)(i+1)/10));
			}

			ret_val = true;
			
			end:
				progress.Hide();
			chkMOX.Checked = false;
			Audio.CurrentAudioState1 = Audio.AudioState.DTTSP;
			comboDisplayMode.Text = display_mode;			// restore display mode
			CurrentDSPMode = dsp_mode;							// restore dsp mode
			VFOAFreq = vfo_freq;								// restore frequency
			txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			display_delay = screen_delay;						// restore FPS
			udPWR.Value = pwr;									// restore pwr level
			meter_delay = multimeter_delay;						// restore meter delay
			
			calibrating = false;

			//t1.Stop();
			//MessageBox.Show(t1.Duration.ToString());
			return ret_val;
		}

		public bool CalibrateSoundCard(Progress progress, int card)
		{
			if(!chkPower.Checked)
			{
				MessageBox.Show("Power must be on in order to calibrate.", "Power Is Off",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}

			progress.SetPercent(0.0f);

			double tx_volume = Audio.RadioVolume;	// save current TX volume
			double rx_volume = Audio.MonitorVolume;	// save current RX volume
			bool twotone = Audio.two_tone;			// save current two tone setting
			Audio.two_tone = false;

			if(num_channels == 4)
			{
				chkMOX.Checked = true;
				Thread.Sleep(200);
				Hdw.TransmitRelay = false;
				Audio.RadioVolume = 1.0;				// set volume to max
				Audio.MonitorVolume = 0.0;
			}
			else
			{
				Mixer.SetMainVolume(mixer_id1, 100);
				Mixer.SetWaveOutVolume(mixer_id1, 100);
				Audio.MonitorVolume = 1.0;
			}			
			
			Audio.CurrentAudioState1 = Audio.AudioState.SINL_COSR;	// Start sending tone
			
			progress.Focus();
            
			while(progress.Visible == true)			// keep sending tone until abort is pressed
				Thread.Sleep(100);

			Audio.CurrentAudioState1 = Audio.AudioState.DTTSP;		// stop sending tone

			if(num_channels == 4)
			{
				Thread.Sleep(200);
				chkMOX.Checked = false;
			}			

			Audio.RadioVolume = tx_volume;			// restore TX volume
			Audio.MonitorVolume = rx_volume;		// restore RX volume
			Audio.two_tone = twotone;				// restore two tone setting

			return true;
		}

		#endregion

		#region Properties
		// ======================================================
		// Properties
		// ======================================================

		[DllImport("user32.dll", EntryPoint="SetWindowPos")]
		private static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

		private bool always_on_top = false;
		public bool AlwaysOnTop
		{
			get { return always_on_top; }
			set
			{
				always_on_top = value;
				if(value)
				{
					SetWindowPos(this.Handle.ToInt32(),
						-1, this.Left, this.Top, this.Width, this.Height, 0);
				}
				else
				{
					SetWindowPos(this.Handle.ToInt32(),
						-2, this.Left, this.Top, this.Width, this.Height, 0);
				}
			}
		}

		private bool quick_qsy = true;
		public bool QuickQSY
		{
			get { return quick_qsy; }
			set { quick_qsy = value; }
		}

		public HW Hdw 
		{
			set 
			{ 
				hw = value; 
				Keyer.Hdw = value;
			}
			get { return hw ; }
		}
		public SIOListenerII Siolisten 
		{
			get { return siolisten;}
			set 
			{
				siolisten = value;
				Keyer.Siolisten = value;
			}
		}
		public bool HideTuneStep
		{
			get { return txtWheelTune.Visible; }
			set
			{
				if(SetupForm != null)
					txtWheelTune.Visible = value;
			}
		}

		public string DisplayModeText
		{
			get { return comboDisplayMode.Text; }
			set { comboDisplayMode.Text = value; }
		}

		private bool auto_mute = false;
		public bool AutoMute
		{
			get { return auto_mute; }
			set { auto_mute = value; }
		}

		private int scope_time = 50;
		public int ScopeTime
		{
			get { return scope_time; }
			set { scope_time = value; }
		}

		private float multimeter_avg_mult_old = 1 - (float)1/10;
		private float multimeter_avg_mult_new = (float)1/10;
		private int multimeter_avg_num_blocks = 10;
		public int MultiMeterAvgBlocks
		{
			get { return multimeter_avg_num_blocks; }
			set
			{
				multimeter_avg_num_blocks = value;
				multimeter_avg_mult_old = 1 - (float)1/multimeter_avg_num_blocks;
				multimeter_avg_mult_new = (float)1/multimeter_avg_num_blocks;
			}
		}

		private bool vac_auto_enable = false;
		public bool VACAutoEnable
		{
			get { return vac_auto_enable; }
			set
			{
				vac_auto_enable = value; 
				if(SetupForm == null) return;
				if(vac_auto_enable)
				{
					switch(current_dsp_mode)
					{
						case DSPMode.DIGL:
						case DSPMode.DIGU:
							SetupForm.VACEnable = true;
							break;
						default:
							SetupForm.VACEnable = false;
							break;
					}
				}
				else SetupForm.VACEnable = false;
			}
		}

		private float display_cal_offset;					// display calibration offset per volume setting in dB
		public float DisplayCalOffset
		{
			get { return display_cal_offset; }
			set { display_cal_offset = value; }
		}

		private int display_cursor_x;						// x-coord of the cursor when over the display
		public int DisplayCursorX
		{
			get { return display_cursor_x; }
			set { display_cursor_y = value; }
		}

		private int display_cursor_y;						// y-coord of the cursor when over the display
		public int DisplayCursorY
		{
			get { return display_cursor_y; }
			set { display_cursor_y = value; }
		}

		private bool long_crosshair;						// If true, the long yellow cursors are displayed
		public bool LongCrosshair
		{
			get { return long_crosshair; }
			set { long_crosshair = value; }
		}

		private DisplayEngine current_display_engine = DisplayEngine.GDI_PLUS;
		public DisplayEngine CurrentDisplayEngine
		{
			get { return current_display_engine; }
			set
			{
				/*switch(value)
				{
					case DisplayEngine.GDI_PLUS:
						current_display_engine = value;
						Display.DirectXRelease();
						break;
					case DisplayEngine.DIRECT_X:
						Display.DirectXInit();
						current_display_engine = value;
						Display.PrepareDisplayVars(Display.CurrentDisplayMode);
						Display.DrawBackground();
						break;
				}*/
			}
		}

		private bool rtty_click_tuning = true;
		public bool RTTYClickTuning
		{
			get { return rtty_click_tuning; }
			set { rtty_click_tuning = value; }
		}

		private bool sstv_click_tuning = true;
		public bool SSTVClickTuning
		{
			get { return sstv_click_tuning; }
			set { sstv_click_tuning = value; }
		}

		private double vox_hang_time = 1500.0;
		public double VOXHangTime
		{
			get { return vox_hang_time; }
			set { vox_hang_time = value; }
		}

		private bool vox_active = false;
		public bool VOXActive
		{
			get { return vox_active; }
			set { vox_active = value; }
		}

		private SoundCard current_soundcard = SoundCard.UNSUPPORTED_CARD;
		public SoundCard CurrentSoundCard
		{
			get { return current_soundcard; }
			set 
			{
				current_soundcard = value;
				Audio.CurSoundCard = value;
				if(SetupForm != null && SetupForm.SoundCardIndex != (int)current_soundcard)
					SetupForm.SoundCardIndex = (int)current_soundcard;
			}
		}

		private Model current_model = Model.SDR1000;
		public Model CurrentModel
		{
			get { return current_model; }
			set
			{
				Model saved_model = current_model;
				current_model = value; 
				switch(current_model)
				{
					case Model.SDR1000:
						MinFreq = 0.011025;
						if(XVTRPresent)
							MaxFreq = 146.0;
						else MaxFreq = 65.0;
						break;
					case Model.SOFTROCK40:
						MinFreq = soft_rock_center_freq - DttSP.SampleRate/2*1e-6;
						MaxFreq = soft_rock_center_freq + DttSP.SampleRate/2*1e-6;
						break;
					case Model.DEMO:
						MinFreq = 0.011025;
						if(XVTRPresent)
							MaxFreq = 146.0;
						else MaxFreq = 65.0;
						break;
				}
				if(SetupForm != null && saved_model != current_model)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
            }
		}

		private DateTimeMode current_datetime_mode = DateTimeMode.LOCAL;
		public DateTimeMode CurrentDateTimeMode
		{
			get { return current_datetime_mode; }
			set
			{
				current_datetime_mode = value;
				if(current_datetime_mode == DateTimeMode.OFF)
				{
					grpDateTime.Text = "Date/Time - Off";
					timer_clock.Enabled = false;
					txtDate.Text = "";
					txtTime.Text = "";
				}
				else
				{
					if(grpDateTime.Text != "Date/Time")
						grpDateTime.Text = "Date/Time";
					if(!timer_clock.Enabled)
						timer_clock.Enabled = true;
				}
			}
		}

		private double soft_rock_center_freq = 7.056;
		public double SoftRockCenterFreq
		{
			get { return soft_rock_center_freq; }
			set
			{
				soft_rock_center_freq = value;
				if(current_model == Model.SOFTROCK40)
				{
					MinFreq = soft_rock_center_freq - DttSP.SampleRate/2*1e-6;
					MaxFreq = soft_rock_center_freq + DttSP.SampleRate/2*1e-6;
					if(SetupForm != null)
						txtVFOAFreq_LostFocus(this, EventArgs.Empty);
				}
			}
		}

		private float saved_vfoa_freq = 7.0f;
		public float SavedVFOAFreq
		{
			get { return saved_vfoa_freq; }
			set { saved_vfoa_freq = value; }
		}

		private float saved_vfob_freq = 7.0f;
		public float SavedVFOBFreq
		{
			get { return saved_vfob_freq; }
			set { saved_vfob_freq = value; }
		}

		private bool ext_ctrl_enabled = false;
		public bool ExtCtrlEnabled
		{
			get { return ext_ctrl_enabled; }
			set { ext_ctrl_enabled = value; }
		}

		private bool cw_semi_break_in_enabled = true;
		public bool CWSemiBreakInEnabled
		{
			get { return cw_semi_break_in_enabled; }
			set { cw_semi_break_in_enabled = value; }
		}

		private bool cw_disable_monitor = false;
		public bool CWDisableMonitor
		{
			get { return cw_disable_monitor; }
			set { cw_disable_monitor = value; }
		}

		public float FilterSizeCalOffset
		{
			get { return filter_size_cal_offset; }
			set { filter_size_cal_offset = value; }
		}

		private PTTMode current_ptt_mode = PTTMode.NONE;
		public PTTMode CurrentPTTMode
		{
			get { return current_ptt_mode; }
			set { current_ptt_mode = value; }
		}

		#region X2 Properties

		private byte x2_160_rx = 0;
		public byte X2160RX
		{
			get { return x2_160_rx; }
			set
			{
				x2_160_rx = value;
				if(current_band == Band.B160M &&
					chkMOX.Checked == false)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}
		
		private byte x2_160_tx = 0;
		public byte X2160TX
		{
			get { return x2_160_tx; }
			set 
			{
				x2_160_tx = value;
				if(current_band == Band.B160M &&
					chkMOX.Checked == true)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_80_rx = 0;
		public byte X280RX
		{
			get { return x2_80_rx; }
			set
			{
				x2_80_rx = value;
				if(current_band == Band.B80M &&
					chkMOX.Checked == false)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_80_tx = 0;
		public byte X280TX
		{
			get { return x2_80_tx; }
			set 
			{
				x2_80_tx = value;
				if(current_band == Band.B80M &&
					chkMOX.Checked == true)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_60_rx = 0;
		public byte X260RX
		{
			get { return x2_60_rx; }
			set
			{
				x2_60_rx = value;
				if(current_band == Band.B60M &&
					chkMOX.Checked == false)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_60_tx = 0;
		public byte X260TX
		{
			get { return x2_60_tx; }
			set 
			{
				x2_60_tx = value;
				if(current_band == Band.B60M &&
					chkMOX.Checked == true)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_40_rx = 0;
		public byte X240RX
		{
			get { return x2_40_rx; }
			set
			{
				x2_40_rx = value;
				if(current_band == Band.B40M &&
					chkMOX.Checked == false)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_40_tx = 0;
		public byte X240TX
		{
			get { return x2_40_tx; }
			set 
			{
				x2_40_tx = value;
				if(current_band == Band.B40M &&
					chkMOX.Checked == true)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_30_rx = 0;
		public byte X230RX
		{
			get { return x2_30_rx; }
			set
			{
				x2_30_rx = value;
				if(current_band == Band.B30M &&
					chkMOX.Checked == false)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_30_tx = 0;
		public byte X230TX
		{
			get { return x2_30_tx; }
			set 
			{
				x2_30_tx = value;
				if(current_band == Band.B30M &&
					chkMOX.Checked == true)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_20_rx = 0;
		public byte X220RX
		{
			get { return x2_20_rx; }
			set
			{
				x2_20_rx = value;
				if(current_band == Band.B20M &&
					chkMOX.Checked == false)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_20_tx = 0;
		public byte X220TX
		{
			get { return x2_20_tx; }
			set 
			{
				x2_20_tx = value;
				if(current_band == Band.B20M &&
					chkMOX.Checked == true)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_17_rx = 0;
		public byte X217RX
		{
			get { return x2_17_rx; }
			set
			{
				x2_17_rx = value;
				if(current_band == Band.B17M &&
					chkMOX.Checked == false)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_17_tx = 0;
		public byte X217TX
		{
			get { return x2_17_tx; }
			set 
			{
				x2_17_tx = value;
				if(current_band == Band.B17M &&
					chkMOX.Checked == true)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_15_rx = 0;
		public byte X215RX
		{
			get { return x2_15_rx; }
			set
			{
				x2_15_rx = value;
				if(current_band == Band.B15M &&
					chkMOX.Checked == false)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_15_tx = 0;
		public byte X215TX
		{
			get { return x2_15_tx; }
			set 
			{
				x2_15_tx = value;
				if(current_band == Band.B15M &&
					chkMOX.Checked == true)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_12_rx = 0;
		public byte X212RX
		{
			get { return x2_12_rx; }
			set
			{
				x2_12_rx = value;
				if(current_band == Band.B12M &&
					chkMOX.Checked == false)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_12_tx = 0;
		public byte X212TX
		{
			get { return x2_12_tx; }
			set 
			{
				x2_12_tx = value;
				if(current_band == Band.B12M &&
					chkMOX.Checked == true)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_10_rx = 0;
		public byte X210RX
		{
			get { return x2_10_rx; }
			set
			{
				x2_10_rx = value;
				if(current_band == Band.B10M &&
					chkMOX.Checked == false)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_10_tx = 0;
		public byte X210TX
		{
			get { return x2_10_tx; }
			set 
			{
				x2_10_tx = value;
				if(current_band == Band.B10M &&
					chkMOX.Checked == true)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_6_rx = 0;
		public byte X26RX
		{
			get { return x2_6_rx; }
			set
			{
				x2_6_rx = value;
				if(current_band == Band.B6M &&
					chkMOX.Checked == false)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_6_tx = 0;
		public byte X26TX
		{
			get { return x2_6_tx; }
			set 
			{
				x2_6_tx = value;
				if(current_band == Band.B6M &&
					chkMOX.Checked == true)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_2_rx = 0;
		public byte X22RX
		{
			get { return x2_2_rx; }
			set
			{
				x2_2_rx = value;
				if(current_band == Band.B2M &&
					chkMOX.Checked == false)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private byte x2_2_tx = 0;
		public byte X22TX
		{
			get { return x2_2_tx; }
			set 
			{
				x2_2_tx = value;
				if(current_band == Band.B2M &&
					chkMOX.Checked == true)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		#endregion

		private bool vfo_lock = false; 
		public bool VFOLock 
		{
			get { return vfo_lock; } 
			set 
			{
				vfo_lock = value;
				bool enabled = !value;
				txtVFOAFreq.Enabled = enabled; 
				btnBand160.Enabled = enabled; 
				btnBand80.Enabled = enabled; 
				btnBand60.Enabled = enabled; 
				btnBand40.Enabled = enabled; 
				btnBand30.Enabled = enabled; 
				btnBand20.Enabled = enabled; 
				btnBand17.Enabled = enabled; 
				btnBand15.Enabled = enabled; 
				btnBand12.Enabled = enabled; 
				btnBand10.Enabled = enabled;
				btnBand6.Enabled = enabled; 
				btnBand2.Enabled = enabled; 
				btnBandWWV.Enabled = enabled; 
				btnBandGEN.Enabled = enabled; 

				btnBandVHF.Enabled = enabled;
				btnBandVHF0.Enabled = enabled;
				btnBandVHF1.Enabled = enabled;
				btnBandVHF2.Enabled = enabled;
				btnBandVHF3.Enabled = enabled;
				btnBandVHF4.Enabled = enabled;
				btnBandVHF5.Enabled = enabled;
				btnBandVHF6.Enabled = enabled;
				btnBandVHF7.Enabled = enabled;
				btnBandVHF8.Enabled = enabled;
				btnBandVHF5.Enabled = enabled;
				btnBandVHF9.Enabled = enabled;
				btnBandVHF10.Enabled = enabled;
				btnBandVHF11.Enabled = enabled;
				btnBandVHF12.Enabled = enabled;
				btnBandVHF13.Enabled = enabled;

				radModeLSB.Enabled = enabled;
				radModeUSB.Enabled = enabled;
				radModeDSB.Enabled = enabled;
				radModeCWL.Enabled = enabled;
				radModeCWU.Enabled = enabled;
				radModeFMN.Enabled = enabled;
				radModeAM.Enabled = enabled;
				radModeSAM.Enabled = enabled;
				radModeSPEC.Enabled = enabled;
				radModeDIGL.Enabled = enabled;
				radModeDIGU.Enabled = enabled;
				radModeDRM.Enabled = enabled;

				btnVFOBtoA.Enabled = enabled; 
				btnVFOSwap.Enabled = enabled; 

				btnMemoryRecall.Enabled = enabled; 
				btnMemoryQuickRestore.Enabled = enabled; 

				chkMemoryScanner.Enabled = enabled; 
			}
		}

		private double wave_freq = 0.0;
		private bool wave_playback = false;
		public bool WavePlayback
		{
			get { return wave_playback; }
			set
			{
				wave_playback = value;
				if(wave_playback)
				{
					wave_freq = dds_freq*1000000%DttSP.SampleRate;
					//DttSP.SetOsc(-11025.0);
				}
				else
				{
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
				}
			}
		}

		private bool rx_only = false;
		public bool RXOnly
		{
			get { return rx_only; }
			set
			{
				rx_only = value;
				if(current_dsp_mode != DSPMode.SPEC &&
					current_dsp_mode != DSPMode.DRM)
					chkMOX.Enabled = !rx_only;
				chkTUN.Enabled = !rx_only;
			}
		}

		private XVTRTRMode current_xvtr_tr_mode = XVTRTRMode.NEGATIVE;
		public XVTRTRMode CurrentXVTRTRMode
		{
			get { return current_xvtr_tr_mode; }
			set	
			{
				current_xvtr_tr_mode = value;
				switch(current_xvtr_tr_mode)
				{
					case XVTRTRMode.NEGATIVE:
						Hdw.XVTR_TR = true;			// Set to receive
						break;
					case XVTRTRMode.POSITIVE:
						Hdw.XVTR_TR = false;			// Set to receive
						break;
					case XVTRTRMode.NONE:
						Hdw.XVTR_TR = false;
						break;
				}
			}
		}

		private double dds_step_size = 0.0;
		private double corrected_dds_clock = 200.0;
		private double dds_clock_correction = 0.0;
		public double DDSClockCorrection
		{
			get	{ return dds_clock_correction; }
			set
			{
				dds_clock_correction = value;
				corrected_dds_clock = 200.0 + dds_clock_correction;
				dds_step_size = corrected_dds_clock / Math.Pow(2, 48);
				DDSFreq = dds_freq;
			}
		}

		private BandPlan current_band_plan = BandPlan.IARU1;
		public BandPlan CurrentBandPlan
		{
			get { return current_band_plan; }
			set { current_band_plan = value; }
		}

		private bool spur_reduction = true;
		public bool SpurReduction
		{
			get { return spur_reduction; }
			set
			{
				spur_reduction = value;
				if(!spur_reduction)
				{
					if(current_dsp_mode == DSPMode.SPEC)
						DttSP.SetOsc(0.0);
					else
						DttSP.SetOsc(-11025.0);
				}
				DDSFreq = dds_freq;
			}
		}

		private double dds_freq = 7.0;
		public double DDSFreq
		{
			get { return dds_freq; }
			set
			{
				dds_freq = value;
				double vfoFreq = value, f = value;
				double dsp_osc_freq = 0;

				//calculate DDS Tuning Word
				if(xvtr_present && f >= 144 && f <= 146)		// If transverter enabled compute 28MHz IF frequency
					f -= 116;									// Subtract 116MHz (144-28) from VFO display frequency

				if(if_shift) 
				{
					f -= if_freq;								// adjust for IF shift
					dsp_osc_freq = -11025.0;
				}

				f += vfo_offset;								// adjust for vfo offset
				long tuning_word = (long)(f / corrected_dds_clock * Math.Pow(2, 48));

				if(spur_reduction)
				{
					long sr_tuning_word = (tuning_word &		// start with current tuning word
						~(0x80007fffffff)) | 0x000080000000;	// clear first bit, low 31 bits; set bit 31

					double software_offset = (sr_tuning_word - tuning_word) * dds_step_size;
					dsp_osc_freq += 1000000.0*software_offset;

					tuning_word = sr_tuning_word;
				}

				DttSP.SetOsc(dsp_osc_freq);
				Hdw.DDSTuningWord = tuning_word;		
				SetHWFilters(dds_freq);

//#if(DEBUG)
//				double freq = TuningWordToFreq(tuning_word);
//				if(!MOX) freq -= DttSP.current_rx_osc / 1000000;
//				else freq += DttSP.current_tx_osc / 1000000;
//
//				Debug.WriteLine("word: "+TuningWordToFreq(tuning_word).ToString("f6") +
//					" rxosc: "+(DttSP.current_rx_osc / 1000000).ToString("f6") +
//					" txosc: "+(DttSP.current_tx_osc / 1000000).ToString("f6") +
//					" sum: "+freq.ToString("f10"));
//#endif
			}
		}

		private double TuningWordToFreq(long word)
		{
			return word * corrected_dds_clock / Math.Pow(2, 48);
		}

		private double min_freq = 0.011025;
		public double MinFreq
		{
			get { return min_freq; }
			set 
			{
				min_freq = value;
				if(VFOAFreq < min_freq)
					VFOAFreq = min_freq;
			}
		}

		private double max_freq = 65.0;
		public double MaxFreq
		{
			get { return max_freq; }
			set
			{
				max_freq = value;
				
				if(SetupForm == null) return;
				if(VFOAFreq > max_freq && current_xvtr_index < 0)
					VFOAFreq = max_freq;
			}
		}

		private double vfo_offset = 0.0;
		public double VFOOffset
		{
			get	{ return vfo_offset; }
			set	{ vfo_offset = value; }
		}

		private double if_freq = 0.011025;
		public double IFFreq
		{
			get { return if_freq; }
			set { if_freq = value; }
		}

		private bool if_shift = true;
		public bool IFShift
		{
			get { return if_shift; }
			set { if_shift = value; }
		}

		private bool extended = false;
		public bool Extended
		{
			get { return extended; }
			set { extended = value; }
		}

		private bool enable_LPF0 = false;
		public bool EnableLPF0
		{
			get { return enable_LPF0; }
			set { enable_LPF0 = value; }
		}

		private int latch_delay = 0;
		public int LatchDelay
		{
			get { return latch_delay;}
			set { latch_delay = value;}
		}

		private bool x2_enabled = false;
		public bool X2Enabled
		{
			get { return x2_enabled; }
			set { x2_enabled = value; }
		}

		private int x2_delay = 500;
		public int X2Delay
		{
			get { return x2_delay; }
			set { x2_delay = value; }
		}

		public bool COMP
		{
			get { return chkDSPComp.Checked; }
			set
			{
				if(SetupForm == null)
					return;

				chkDSPComp.Checked = value;
			}
		}

		public bool CPDR
		{
			get { return chkDSPCompander.Checked; }
			set
			{
				if(SetupForm == null)
					return;
				
				chkDSPCompander.Checked = value;
			}
		}

		public int Mic
		{
			get { return (int)udMIC.Value; }
			set
			{
				if(SetupForm == null)
					return;

				udMIC.Value = value;
			}
		}

		#region CAT Properties
		
		// props for cat control 
		// Added 06/20/05 BT for CAT commands
		private int cat_nr_status = 0;
		public int CATNR
		{
			get{return cat_nr_status;}
			set
			{
				if(value == 0)
					chkNR.Checked = false;
				else if(value == 1)
					chkNR.Checked = true;
			}
		}

		// Added 06/20/05 BT for CAT commands
		private int cat_anf_status = 0;
		public int CATANF
		{
			get{return cat_anf_status;}
			set
			{
				if(value == 0)
					chkANF.Checked = false;
				else if(value == 1)
					chkANF.Checked = true;
			}
		}

		// Added 06/21/05 BT for CAT Commands
		private int cat_nb1_status = 0;
		public int CATNB1
		{
			get{return cat_nb1_status;}
			set
			{
				if(value == 0)
					chkNB.Checked = false;
				else if(value == 1)
					chkNB.Checked = true;
			}
		}

		// Added 06/21/05 BT for CAT commands
		private int cat_nb2_status = 0;
		public int CATNB2
		{
			get{return cat_nb2_status;}
			set
			{
				if(value == 0)
					chkDSPNB2.Checked = false;
				else if(value == 1)
					chkDSPNB2.Checked = true;
			}
		}

		// Added 06/22/05 BT for CAT commands
		private int cat_cmpd_status = 0;
		public int CATCmpd
		{
			get{return cat_cmpd_status;}
			set
			{
				if(value == 0)
					chkDSPCompander.Checked = false;
				else if(value == 1)
					chkDSPCompander.Checked = true;
			}
		}

		// Added 06/22/05 BT for CAT commands
		private int cat_mic_status = 0;
		public int CATMIC
		{
			get
			{
				cat_mic_status = (int) udMIC.Value;
				return cat_mic_status;
			}
			set
			{
				value = Math.Max(0, value);
				value = Math.Min(100, value);
				udMIC.Value = value;
			}
		}

		// Added 06/22/05 BT for CAT commands
		// modified 07/22/05 to fix display problem
		private int cat_filter_width = 0;
		public int CATFilterWidth
		{
			get
			{
				cat_filter_width = tbFilterWidth.Value;
				return cat_filter_width;
			}
			set
			{
				value = Math.Max(1, value);
				value = Math.Min(10000, value);
				tbFilterWidth.Value = value;
				tbFilterWidth_Scroll(this.tbFilterWidth, EventArgs.Empty);	// added
			}
		}

		// Added 07/22/05 for cat commands
		public int CATFilterShift
		{
			get
			{
				return tbFilterShift.Value;
			}
			set
			{
				value = Math.Max(-1000, value);
				value = Math.Min(1000, value);
				tbFilterShift.Value = value;
				tbFilterShift_Scroll(this.tbFilterShift, EventArgs.Empty);
			}
		}

		// Added 07/22/05 for CAT commands
		public int CATFilterShiftReset
		{
			set
			{
				if(value == 1)
					btnFilterShiftReset.PerformClick();
			}
		}

		// Added 06/22/05 BT for CAT commands
		private int cat_bin_status = 0;
		public int CATBIN
		{
			get
			{
				if(chkBIN.Checked)
					cat_bin_status = 1;
				else
					cat_bin_status = 0;

				return cat_bin_status;
			}
			set
			{
				if(value == 1)
					chkBIN.Checked = true;
				else if(value == 0)
					chkBIN.Checked = false;
			}
		}

		// Added/repaired 7/10/05 BT for cat commands
		public int CATPreamp
		{
			set{comboPreamp.SelectedIndex = value;}
			get{return comboPreamp.SelectedIndex;}
		}

		// Added 06/30/05 BT for CAT commands
		public int CATCWSpeed
		{
			get
			{
				return (int) udCWSpeed.Value;
			}
			set
			{
				value = Math.Max(1,value);
				value = Math.Min(60, value);
				udCWSpeed.Value = value;
			}
		}

		// Added 06/30/05 BT for CAT commands
		private int cat_display_avg_status = 0;
		public int CATDisplayAvg
		{
			get
			{
				if(chkDisplayAVG.Checked)
					cat_display_avg_status = 1;
				else
					cat_display_avg_status = 0;

				return cat_display_avg_status;
			}
			set
			{
				if(value == 1)
					chkDisplayAVG.Checked = true;
				else
					chkDisplayAVG.Checked = false;
			}
		}

		// Added 06/30/05 BT for CAT commands
		private int cat_squelch_status = 0;
		public int CATSquelch
		{
			get
			{
				if(chkSquelch.Checked)
					cat_squelch_status = 1;
				else
					cat_squelch_status = 0;

				return cat_squelch_status;
			}
			set
			{
				if(value == 1)
					chkSquelch.Checked = true;
				else
					chkSquelch.Checked = false;
			}
		}

		// Added 7/9/05 BT for cat commands
		public string CATQMSValue
		{
			get{return this.txtMemoryQuick.Text;}
		}

		private SDRSerialSupportII.SDRSerialPort.Parity cat_parity;
		public SDRSerialSupportII.SDRSerialPort.Parity CATParity 
		{
			set {cat_parity = value; }
			get { return cat_parity; }
		}


		private SDRSerialSupportII.SDRSerialPort.StopBits cat_stop_bits; 
		public SDRSerialSupportII.SDRSerialPort.StopBits CATStopBits 
		{
			set { cat_stop_bits = value; }
			get { return cat_stop_bits; } 
		}
		private SDRSerialSupportII.SDRSerialPort.DataBits cat_data_bits; 
		public SDRSerialSupportII.SDRSerialPort.DataBits CATDataBits 
		{
			set { cat_data_bits = value; }
			get { return cat_data_bits; } 
		}
		
		private int cat_baud_rate; 
		public int CATBaudRate 
		{
			set { cat_baud_rate = value; } 
			get { return cat_baud_rate; } 
		}

		private bool cat_enabled; 
		public bool CATEnabled 
		{
			set 
			{
				cat_enabled = value; 
				Keyer.CATEnabled = value;
				if ( siolisten != null )  // if we've got a listener tell them about state change 
				{ 
					if ( cat_enabled ) 
					{ 
						Siolisten.enableCAT();
					}
					else 
					{
						Siolisten.disableCAT();
					}
				}
			}
			get { return cat_enabled; } 
		}
		
		private int cat_rig_type;
		public int CATRigType
		{
			get { return cat_rig_type; }
			set { cat_rig_type = value; }
		}

		private int cat_port; 
		public int CATPort 
		{
			get { return cat_port; }
			set { cat_port = value; } 
		}

		private bool cat_ptt_rts = false; 
		public bool CATPTTRTS 
		{
			get { return cat_ptt_rts; }
			set { cat_ptt_rts = value; }
		}

		private bool cat_ptt_dtr;
		public bool CATPTTDTR
		{
			get { return cat_ptt_dtr; }
			set { cat_ptt_dtr = value; }
		}

		public SerialPortPTT serialPTT = null; 
		private bool ptt_bit_bang_enabled; 
		public bool PTTBitBangEnabled 
		{
			get { return ptt_bit_bang_enabled; }
			set 
			{
				ptt_bit_bang_enabled = value; 
				if ( serialPTT != null )  // kill current serial PTT if we have one 
				{ 
					serialPTT.Destroy(); 
					serialPTT = null; 
				} 
				if ( ptt_bit_bang_enabled )  
				{ 
					// wjt -- don't really like popping a msg box in here ...   nasty when we do a remoted 
					// setup ... will let that wait for the great console refactoring 
					try 
					{ 
						serialPTT = new SerialPortPTT(cat_ptt_bit_bang_port, cat_ptt_rts, cat_ptt_dtr); 
						serialPTT.Init(); 						
					}
					catch ( Exception ex ) 
					{
						ptt_bit_bang_enabled = false; 
						if ( SetupForm != null ) 
						{ 
							SetupForm.copyCATPropsToDialogVars(); // need to make sure the props on the setup page get reset 
						}
						MessageBox.Show("Could not initialize PTT Bit Bang control.  Exception was:\n\n " + ex.Message + 
							"\n\nPTT Bit Bang control has been disabled.", "Error Initializing PTT control", 
							MessageBoxButtons.OK, MessageBoxIcon.Error);
 
					}
				} 
			}
		}

		private int cat_ptt_bit_bang_port; 
		public int CATPTTBitBangPort 
		{
			get { return cat_ptt_bit_bang_port; }
			set { cat_ptt_bit_bang_port = value; }
		}
		
		#endregion

		private bool notify_on_beta = true;
		public bool NotifyOnBeta
		{
			get { return notify_on_beta; }
			set { notify_on_beta = value; }
		}

		private bool notify_on_release = true;
		public bool NotifyOnRelease
		{
			get { return notify_on_release; }
			set { notify_on_release = value; }
		}

		private int tune_power;								// power setting to use when TUN button is pressed
		public int TunePower
		{
			get { return tune_power; }
			set
			{
				tune_power = value;
				if(SetupForm != null)
					SetupForm.TunePower = tune_power;

				if(chkTUN.Checked)
				{
					if(atu_present)
					{
						if((ATUTuneMode)comboTuneMode.SelectedIndex == ATUTuneMode.BYPASS)
							udPWR.Value = tune_power;
					}
					else udPWR.Value = tune_power;
				}
			}
		}

		private bool disable_swr_protection = false;
		public bool DisableSWRProtection
		{
			get { return disable_swr_protection; }
			set { disable_swr_protection = value; }
		}

		private int previous_pwr = 50;
		public int PreviousPWR
		{
			get { return previous_pwr; }
			set	{ previous_pwr = value; }
		}

		private bool no_hardware_offset = false;
		public bool NoHardwareOffset
		{
			get { return no_hardware_offset; }
			set
			{
				no_hardware_offset = value;
				comboPreamp_SelectedIndexChanged(this, EventArgs.Empty);
			}
		}

		#region CAT Properties

		private Band current_band;
		//BT 06/15/05 Made public for CAT commands
		public Band CurrentBand
		{
			get { return current_band; }
			set
			{
				Band old_band = current_band;
				current_band = value;
				if(current_band != old_band)
					udPWR_ValueChanged(this, EventArgs.Empty);
			}
		}

		// Added 06/24/05 BT for CAT commands
		public bool CATVFOLock
		{
			get{return chkVFOLock.Checked;}
			set{chkVFOLock.Checked = value;}
		}

		public string CATGetVersion()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
			//			int current_version = VersionTextToInt(fvi.FileVersion);
			return fvi.FileVersion;
		}

		// Added 07/30/05 BT for cat commands next 8 functions

		public string CATReadSigStrength()
		{
			float num = 0f;
			num = DttSP.CalculateMeter(DttSP.MeterType.SIGNAL_STRENGTH);
			num = num +
				multimeter_cal_offset +
				preamp_offset[(int)current_preamp_mode] +
				filter_size_cal_offset;
			return num.ToString("f1")+" dBm";
		}

		public string CATReadAvgStrength()
		{
			float num = 0f;
			num = DttSP.CalculateMeter(DttSP.MeterType.AVG_SIGNAL_STRENGTH);
			num = num +
				multimeter_cal_offset +
				preamp_offset[(int)current_preamp_mode] +
				filter_size_cal_offset;
			return num.ToString("f1")+" dBm";
		}

		public string CATReadADC_L()
		{
			float num = 0f;
			num = DttSP.CalculateMeter(DttSP.MeterType.ADC_REAL);
			return num.ToString("f1")+" dBFS";
		}

		public string CATReadADC_R()
		{
			float num = 0f;
			num = DttSP.CalculateMeter(DttSP.MeterType.ADC_IMAG);
			return num.ToString("f1")+" dBFS";
		}

		public string CATReadALC()
		{
			float num = 0f;

			if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
			{
				num = (float)Math.Max(-20.0, -DttSP.CalculateMeter(DttSP.MeterType.ALC));
				return num.ToString("f1")+" dB";
			}
			else return "0"+separator+"0 dB";
		}

		public string CATReadFwdPwr()
		{
			double power = 0.0;
			float num = 0f;

			if(pa_present && VFOAFreq < 30.0)
			{
				pa_power_mutex.WaitOne();
				power = PAPower(pa_fwd_power);
				pa_power_mutex.ReleaseMutex();
							
				return power.ToString("f0")+" W";
			}
			else
			{
				if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
				{
					num = (float)Math.Max(0.0,DttSP.CalculateMeter(DttSP.MeterType.PWR));
					num *= (float)((double)udPWR.Value*0.01);
					return num.ToString("f2")+" W";
				}
				else return "0"+separator+"00 W";
			}
		}

		public string CATReadPeakPwr()
		{
			float num = 0f;
			if(pa_present && VFOAFreq < 30.0)
			{
				if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
				{
					num = (float)Math.Max(0.0,DttSP.CalculateMeter(DttSP.MeterType.ALC));
					num *= (float)udPWR.Value;
							
					meter_text_history[meter_text_history_index] = num;
					meter_text_history_index = (meter_text_history_index+1)%multimeter_text_peak_samples;
					float max = float.MinValue;
					for(int i=0; i<multimeter_text_peak_samples; i++)
					{
						if(meter_text_history[i] > max)
							max = meter_text_history[i];
					}
					num = max;

					return num.ToString("f0")+" W";
				}
				else return "0 W";
			}
			else
			{
				if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
				{
					num = (float)Math.Max(0.0,DttSP.CalculateMeter(DttSP.MeterType.ALC));
					num *= (float)((float)udPWR.Value*0.01);
					return num.ToString("f2")+" W";
				}
				else return "0"+separator+"00 W";
			}
		}

		public string CATReadRevPwr()
		{
			double power = 0.0;
			pa_power_mutex.WaitOne();
			power = PAPower(pa_rev_power);
			pa_power_mutex.ReleaseMutex();
			return power.ToString("f0")+" W";
		}

		public string CATReadSWR()
		{
			double swr = 0.0;
			pa_power_mutex.WaitOne();
			swr = SWR(pa_fwd_power, pa_rev_power);
			pa_power_mutex.ReleaseMutex();
			return swr.ToString("f1")+" : 1";
		}

		//*************end of 8 functions.

		#endregion

		private DSPMode current_dsp_mode = DSPMode.FIRST;
		public DSPMode CurrentDSPMode
		{
			get { return current_dsp_mode; }
			set
			{
				RadioButtonTS r = null;
				switch(value)
				{
					case DSPMode.LSB:
						r = radModeLSB;
						break;
					case DSPMode.USB:
						r = radModeUSB;
						break;
					case DSPMode.DSB:
						r = radModeDSB;
						break;
					case DSPMode.CWL:
						r = radModeCWL;
						break;
					case DSPMode.CWU:
						r = radModeCWU;
						break;
					case DSPMode.FMN:
						r = radModeFMN;
						break;
					case DSPMode.AM:
						r = radModeAM;
						break;
					case DSPMode.SAM:
						r = radModeSAM;
						break;
					case DSPMode.SPEC:
						r = radModeSPEC;
						break;
					case DSPMode.DIGL:
						r = radModeDIGL;
						break;
					case DSPMode.DIGU:
						r = radModeDIGU;
						break;
					case DSPMode.DRM:
						r = radModeDRM;
						break;
				}

				r.Checked = true;
			}
		}

		private Filter current_filter = Filter.FIRST;
		public Filter CurrentFilter
		{
			get { return current_filter; }
			set
			{
					RadioButtonTS r = null;
				switch(value)
				{
					case Filter.F6000:
						r = radFilter6000;
						break;
					case Filter.F4000:
						r = radFilter4000;
						break;
					case Filter.F2600:
						r = radFilter2600;
						break;
					case Filter.F2100:
						r = radFilter2100;
						break;
					case Filter.F1000:
						r = radFilter1000;
						break;
					case Filter.F500:
						r = radFilter500;
						break;
					case Filter.F250:
						r = radFilter250;
						break;
					case Filter.F100:
						r = radFilter100;
						break;
					case Filter.F50:
						r = radFilter50;
						break;
					case Filter.F25:
						r = radFilter25;
						break;
					case Filter.VAR1:
						r = radFilterVar1;
						break;
					case Filter.VAR2:
						r = radFilterVar2;
						break;
					case Filter.NONE:
						foreach(Control c in grpFilter.Controls)
						{
							if(c.GetType() == typeof(RadioButtonTS))
							{
								((RadioButtonTS)c).Checked = false;

								if(c.BackColor != SystemColors.Control)
								{
									((RadioButtonTS)c).BackColor = SystemColors.Control;
								}
							}
						}
						current_filter = Filter.NONE;
						break;
				}

				if(r != null)
				{
					if(r.Checked)
					{
						r.Checked = false;
					}

					r.Checked = true;
				}
			}
		}

		private MeterRXMode current_meter_rx_mode = MeterRXMode.SIGNAL_STRENGTH;
		public MeterRXMode CurrentMeterRXMode
		{
			get { return current_meter_rx_mode; }
			set
			{
				string text = "";
				switch(value)
				{
					case MeterRXMode.SIGNAL_STRENGTH:
						text = "Signal";
						break;
					case MeterRXMode.SIGNAL_AVERAGE:
						text = "Sig Avg";
						break;
					case MeterRXMode.ADC_L:
						text = "ADC L";
						break;
					case MeterRXMode.ADC_R:
						text = "ADC R";
						break;
					case MeterRXMode.OFF:	// BT Added 7/24/05 for CAT commands
						text = "Off";
						break;
				}

				if(text == "") return;

				comboMeterRXMode.Text = text;
			}
		}

		private MeterTXMode current_meter_tx_mode = MeterTXMode.FIRST;
		public MeterTXMode CurrentMeterTXMode
		{
			get { return current_meter_tx_mode; }
			set
			{
				string text = "";
				switch(value)
				{
					case MeterTXMode.FORWARD_POWER:
						text = "Fwd Pwr";
						break;
					case MeterTXMode.REVERSE_POWER:
						text = "Ref Pwr";
						break;
					case MeterTXMode.MIC:
						text = "Mic";
						break;
					case MeterTXMode.EQ:
						text = "EQ";
						break;
					case MeterTXMode.LEVELER:
						text = "Leveler";
						break;
					case MeterTXMode.LVL_G:
						text = "Lvl Gain";
						break;
					case MeterTXMode.COMP:
						text = "COMP";
						break;
					case MeterTXMode.CPDR:
						text = "CPDR";
						break;
					case MeterTXMode.ALC:
						text = "ALC";
						break;					
					case MeterTXMode.ALC_G:
						text = "ALC Comp";
						break;					
					case MeterTXMode.SWR:
						text = "SWR";
						break;
					case MeterTXMode.OFF:		// BT Added 07/24/05 for CAT commands
						text = "Off";
						break;
				}
				if(text == "") return;

				comboMeterTXMode.Text = text;
			}
		}

		private int cw_pitch = 600;
		public int CWPitch
		{
			get { return cw_pitch; }
			set
			{
				int diff = value - cw_pitch;
				cw_pitch = value;
				CWForm.CWPitch = value;
				Audio.SineFreq1 = value;

				if(current_dsp_mode == DSPMode.CWL ||			// if in CW Mode
					current_dsp_mode == DSPMode.CWU)
				{												// recalculate filter
					if(current_dsp_mode == DSPMode.CWL)
						DttSP.SetKeyerFreq(cw_pitch);
					else // current_dsp_mode == DSPMode.CWU
						DttSP.SetKeyerFreq(-cw_pitch);

					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
					CurrentFilter = current_filter;
				}
			}
		}

		private int histogram_hang_time = 100;
		public int HistogramHangTime
		{
			get { return histogram_hang_time; }
			set { histogram_hang_time = value; }
		}

		public double VFOAFreq
		{
			get 
			{
				try
				{
					return double.Parse(txtVFOAFreq.Text);
				}
				catch(Exception)
				{
					return 0;
				}
			}
			set
			{
				if(vfo_lock || SetupForm == null) return;
				txtVFOAFreq.Text = value.ToString("f6");
				txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		public double VFOBFreq
		{
			get 
			{
				try
				{
					return double.Parse(txtVFOBFreq.Text);
				}
				catch(Exception)
				{
					return 0;
				}
			}
			set
			{
				txtVFOBFreq.Text = value.ToString("f6");
				txtVFOBFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		public int PWR
		{
			get { return (int)udPWR.Value; }
			set
			{
				value = Math.Max(0, value);			// lower bound
				value = Math.Min(100, value);		// upper bound

				if(current_dsp_mode == DSPMode.FMN)
					value /= 4;

				udPWR.Value = value;
				udPWR_ValueChanged(this, EventArgs.Empty);
			}
		}

		public int AF
		{
			get { return (int)udAF.Value; }
			set
			{
				value = Math.Max(0, value);			// lower bound
				value = Math.Min(100, value);		// upper bound

				udAF.Value = value;
				udAF_ValueChanged(this, EventArgs.Empty);
			}
		}

		private int rxaf = 50;
		public int RXAF
		{
			get { return rxaf; }
			set { rxaf = value; }
		}

		private int txaf = 50;
		public int TXAF
		{
			get { return txaf; }
			set
			{
				txaf = value;
				if(SetupForm != null) 
				{
					SetupForm.TXAF = txaf;
					if(MOX) udAF.Value = txaf;
				}
			}
		}

		public bool DisplayAVG
		{
			get { return chkDisplayAVG.Checked; }
			set	{ chkDisplayAVG.Checked = value; }
		}

		public bool NewKeyer
		{
			get { return new_keyer; }
			set
			{
				new_keyer = value;
				Audio.NewKeyer = value;
				mnuCW.Enabled = !new_keyer;
				if(new_keyer && CWForm != null)
					CWForm.Dispose();
			}
		}

		private double break_in_delay = 400;
		public double BreakInDelay
		{
			get { return break_in_delay; }
			set { break_in_delay = value; }
		}

		public bool MOX
		{
			get { return chkMOX.Checked; }
			set	{ chkMOX.Checked = value; }
		}

		public bool MOXEnabled
		{
			get { return chkMOX.Enabled; }
			set	{ chkMOX.Enabled = value; }
		}

		public bool MON
		{
			get { return chkMON.Checked; }
			set	{ chkMON.Checked = value; }
		}

		public bool MUT
		{
			get { return chkMUT.Checked; }
			set	{ chkMUT.Checked = value; }
		}

		public bool TUN
		{
			get { return chkTUN.Checked; }
			set	{ chkTUN.Checked = value; }
		}

		public bool TUNEnabled
		{
			get { return chkTUN.Enabled; }
			set { chkTUN.Enabled = value; }
		}
		
		public int FilterLowValue
		{
			get { return (int)udFilterLow.Value; }
			set { udFilterLow.Value = value; }
		}

		public int FilterHighValue
		{
			get { return (int)udFilterHigh.Value; }
			set	{ udFilterHigh.Value = value; }
		}

		public int FilterShiftValue
		{
			get { return tbFilterShift.Value; }
			set	{ tbFilterShift.Value = value; }
		}

		private PreampMode current_preamp_mode = PreampMode.MED;
		public PreampMode CurrentPreampMode
		{
			get { return current_preamp_mode; }
			set
			{
				current_preamp_mode = value;
				switch(current_preamp_mode)
				{
					case PreampMode.OFF:
						if(rfe_present)
						{
							Hdw.Attn = true;
							Hdw.GainRelay = true;	// 0dB
						}
						break;
					case PreampMode.LOW:
						if(rfe_present)
						{
							Hdw.Attn = false;
							Hdw.GainRelay = true;	// 0dB
						}
						break;
					case PreampMode.MED:
						if(rfe_present)
						{
							Hdw.Attn = true;
						}
						Hdw.GainRelay = false;	// 26dB
						break;
					case PreampMode.HIGH:
						if(rfe_present)
						{
							Hdw.Attn = false;
							Hdw.GainRelay = false;
						}
						else
						{
							Hdw.GainRelay = true;	// 40dB
						}
						break;
				}
				
				switch(current_preamp_mode)
				{
					case PreampMode.OFF:
						comboPreamp.Text = "Off";
						break;
					case PreampMode.LOW:
						comboPreamp.Text = "Low";
						break;
					case PreampMode.MED:
						comboPreamp.Text = "Med";
						break;
					case PreampMode.HIGH:
						comboPreamp.Text = "High";
						break;
				}
			}
		}

		public int Squelch
		{
			get { return (int)udSquelch.Value; }
			set	{ udSquelch.Value = value; }
		}

		public int StepSize
		{
			get { return wheel_tune_index; }
		}

		public AGCMode CurrentAGCMode
		{
			get { return (AGCMode)comboAGC.SelectedIndex; }
			set	{ comboAGC.SelectedIndex = (int)value; }
		}

		public bool VFOSplit
		{
			get { return chkVFOSplit.Checked; }
			set	{ chkVFOSplit.Checked = value; }
		}

		public bool RIT
		{
			get { return chkRIT.Checked; }
			set	{ chkRIT.Checked = value; }
		}

		public bool RITOn
		{
			get { return chkRIT.Checked; }
			set { chkRIT.Checked = value; }
		}

		public int RITValue
		{
			get { return (int)udRIT.Value; }
			set	{ udRIT.Value = value; }
		}

		public bool XITOn
		{
			get { return chkXIT.Checked; }
			set	{ chkXIT.Checked = value; }
		}

		public int XITValue
		{
			get { return (int)udXIT.Value; }
			set	{ udXIT.Value = value; }
		}

		private int tx_filter_high = 3000;
		public int TXFilterHigh
		{
			get{ return tx_filter_high; }
			set
			{
				tx_filter_high = value;
				DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
				if(chkMOX.Checked)
				{
					switch(Display.CurrentDisplayMode)
					{
						case DisplayMode.PANADAPTER:
						case DisplayMode.SPECTRUM:
						case DisplayMode.HISTOGRAM:
						case DisplayMode.WATERFALL:
							Display.DrawBackground();
							break;
					}
				}
			}
		}

		private int tx_filter_low = 300;
		public int TXFilterLow
		{
			get{ return tx_filter_low; }
			set
			{
				tx_filter_low = value;
				DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
				if(chkMOX.Checked)
				{
					switch(Display.CurrentDisplayMode)
					{
						case DisplayMode.PANADAPTER:
						case DisplayMode.SPECTRUM:
						case DisplayMode.HISTOGRAM:
						case DisplayMode.WATERFALL:
							Display.DrawBackground();
							break;
					}
				}
			}
		}

		private delegate void SetTimerDel(System.Windows.Forms.Timer t, bool enable);
		private void SetTimer(System.Windows.Forms.Timer t, bool enable)
		{
			t.Enabled = enable;
		}

		private bool high_swr = false;
		public bool HighSWR
		{
			get { return high_swr; }
			set
			{
				high_swr = value;
				Display.DrawBackground();
			}
		}

		private bool disable_ptt = false;
		public bool DisablePTT
		{
			get { return disable_ptt; }
			set { disable_ptt = value; }
		}

		public bool PowerOn
		{
			get { return chkPower.Checked; }
			set { chkPower.Checked = value;	}
		}

		public bool PowerEnabled
		{
			get { return chkPower.Enabled; }
			set { chkPower.Enabled = value; }
		}

		private bool second_sound_card_stereo = false;
		public bool SecondSoundCardStereo
		{
			get { return second_sound_card_stereo; }
			set 
			{
				second_sound_card_stereo = value;
				Audio.VACStereo = value;
			}
		}

		private bool vac_enabled = false;
		public bool VACEnabled
		{
			get { return vac_enabled; }
			set 
			{
				vac_enabled = value;
				Audio.VACEnabled = value;
			}
		}

		private int audio_driver_index1 = 0;
		public int AudioDriverIndex1
		{
			get { return audio_driver_index1; }
			set { audio_driver_index1 = value; }
		}

		private int audio_driver_index2 = 0;
		public int AudioDriverIndex2
		{
			get { return audio_driver_index2; }
			set { audio_driver_index2 = value; }
		}

		private int audio_input_index1 = 0;
		public int AudioInputIndex1
		{
			get { return audio_input_index1; }
			set { audio_input_index1 = value; }
		}

		private int audio_input_index2 = 0;
		public int AudioInputIndex2
		{
			get { return audio_input_index2; }
			set { audio_input_index2 = value; }
		}

		private int audio_output_index1 = 0;
		public int AudioOutputIndex1
		{
			get { return audio_output_index1; }
			set { audio_output_index1 = value; }
		}

		private int audio_output_index2 = 0;
		public int AudioOutputIndex2
		{
			get { return audio_output_index2; }
			set { audio_output_index2 = value; }
		}

		private int audio_latency1 = 120;
		public int AudioLatency1
		{
			get { return audio_latency1; }
			set { audio_latency1 = value; }
		}

		private int audio_latency2 = 120;
		public int AudioLatency2
		{
			get { return audio_latency2; }
			set { audio_latency2 = value; }
		}

		private double audio_volts1 = 2.23;
		public double AudioVolts1
		{
			get { return audio_volts1; }
			set { audio_volts1 = value; }
		}

		private int mixer_id1 = 0;
		public int MixerID1
		{
			get { return mixer_id1; }
			set { mixer_id1 = value; }
		}

		private int mixer_id2 = 0;
		public int MixerID2
		{
			get { return mixer_id2; }
			set { mixer_id2 = value; }
		}

		private int mixer_rx_mux_id1 = 0;
		public int MixerRXMuxID1
		{
			get { return mixer_rx_mux_id1; }
			set	{ mixer_rx_mux_id1 = value;	}
		}

		private int mixer_tx_mux_id1 = 0;
		public int MixerTXMuxID1
		{
			get { return mixer_tx_mux_id1; }
			set { mixer_tx_mux_id1 = value; }
		}

		private int mixer_rx_mux_id2 = 0;
		public int MixerRXMuxID2
		{
			get { return mixer_rx_mux_id2; }
			set { mixer_rx_mux_id2 = value; }
		}

		private int mixer_tx_mux_id2 = 0;
		public int MixerTXMuxID2
		{
			get { return mixer_tx_mux_id2; }
			set { mixer_tx_mux_id2 = value; }
		}

		private int sample_rate1 = 48000;
		public int SampleRate1
		{
			get { return sample_rate1; }
			set 
			{
				sample_rate1 = value;
				Audio.SampleRate1 = value;
			}
		}

		private int sample_rate2 = 48000;
		public int SampleRate2
		{
			get { return sample_rate2; }
			set
			{
				sample_rate2 = value;
				Audio.SampleRate2 = value;
			}
		}

		private int num_channels = 2;
		public int NumChannels
		{
			get { return num_channels; }
			set { num_channels = value; }
		}

		private int block_size1;
		public int BlockSize1
		{
			get { return block_size1; }
			set
			{
				block_size1 = value;
				CWForm.CWBlockSize = block_size1;
			}
		}

		private int block_size2;
		public int BlockSize2
		{
			get { return block_size2; }
			set { block_size2 = value; }
		}

		private bool cw_key_mode = false;
		public bool CWKeyMode
		{
			get { return cw_key_mode; }
			set { cw_key_mode = value; }
		}

		private int peak_text_delay = 500;
		public int PeakTextDelay
		{
			get { return peak_text_delay; }
			set
			{
				peak_text_delay = value;
				timer_peak_text.Interval = value;
			}
		}

		private int meter_delay = 100;
		public int MeterDelay
		{
			get { return meter_delay; }
			set
			{
				meter_delay = value;
				MultimeterPeakHoldTime = MultimeterPeakHoldTime;
			}
		}

		private int cpu_meter_delay = 1000;
		public int CPUMeterDelay
		{
			get { return cpu_meter_delay; }
			set
			{
				cpu_meter_delay = value;
				timer_cpu_meter.Interval = value;
			}
		}

		private int display_fps = 15;
		private int display_delay = 1000 / 15;
		public int DisplayFPS
		{
			get{ return display_fps;}
			set
			{
				display_fps = value;
				display_delay = 1000 / display_fps;
			}
		}

		private int multimeter_peak_hold_time = 1000;
		private int multimeter_peak_hold_samples = 10;
		public int MultimeterPeakHoldTime
		{
			get { return multimeter_peak_hold_time; }
			set
			{
				multimeter_peak_hold_time = value;
				multimeter_peak_hold_samples = value / meter_delay;
			}
		}

		private int multimeter_text_peak_time = 500;
		private int multimeter_text_peak_samples = 5;
		public int MultimeterTextPeakTime
		{
			get { return multimeter_text_peak_time; }
			set
			{
				multimeter_text_peak_time = value;
				multimeter_text_peak_samples = value / meter_delay;
				if(multimeter_text_peak_samples > meter_text_history.Length)
				{
					float[] temp = new float[multimeter_text_peak_samples];
					for(int i=0; i<meter_text_history.Length; i++)
						temp[i] = meter_text_history[i];
					meter_text_history = temp;
				}
			}
		}

		private Color vfo_text_light_color = Color.Yellow;
		public Color VFOTextLightColor
		{
			get { return vfo_text_light_color; }
			set
			{
				if(txtVFOAFreq.ForeColor == vfo_text_light_color)
					txtVFOAFreq.ForeColor = value;
				if(txtVFOBFreq.ForeColor == vfo_text_light_color)
					txtVFOBFreq.ForeColor = value;
				vfo_text_light_color = value;
			}
		}

		private Color vfo_text_dark_color = Color.Olive;
		public Color VFOTextDarkColor
		{
			get { return vfo_text_dark_color; }
			set
			{
				if(txtVFOAFreq.ForeColor == vfo_text_dark_color)
					txtVFOAFreq.ForeColor = value;
				if(txtVFOBFreq.ForeColor == vfo_text_dark_color)
					txtVFOBFreq.ForeColor = value;
				vfo_text_dark_color = value;
			}
		}

		private Color band_text_light_color = Color.Lime;
		public Color BandTextLightColor
		{
			get { return band_text_light_color; }
			set
			{
				if(txtVFOABand.ForeColor == band_text_light_color)
					txtVFOABand.ForeColor = value;
				if(txtVFOBBand.ForeColor == band_text_light_color)
					txtVFOBBand.ForeColor = value;
				band_text_light_color = value;
			}
		}

		private Color band_text_dark_color = Color.Green;
		public Color BandTextDarkColor
		{
			get { return band_text_dark_color; }
			set
			{
				if(txtVFOABand.ForeColor == band_text_dark_color)
					txtVFOABand.ForeColor = value;
				if(txtVFOBBand.ForeColor == band_text_dark_color)
					txtVFOBBand.ForeColor = value;
				band_text_dark_color = value;
			}
		}

		private Color peak_text_color = Color.DodgerBlue;
		public Color PeakTextColor
		{
			get { return peak_text_color; }
			set
			{
				peak_text_color = value;
				txtDisplayCurPos.ForeColor = value;
				txtDisplayPeak.ForeColor = value;
			}
		}

		private Color out_of_band_color = Color.DimGray;
		public Color OutOfBandColor
		{
			get { return out_of_band_color; }
			set
			{
				out_of_band_color = value;
				if(SetupForm != null)
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private Color button_selected_color = Color.Yellow;
		public Color ButtonSelectedColor
		{
			get { return button_selected_color; }
			set
			{
				button_selected_color = value;
				CheckSelectedButtonColor();
			}
		}

		private Color meter_left_color = Color.Green;
		public Color MeterLeftColor
		{
			get { return meter_left_color; }
			set
			{
				meter_left_color = value;
				picMultiMeterDigital.Invalidate();
			}
		}

		private Color meter_right_color = Color.Lime;
		public Color MeterRightColor
		{
			get { return meter_right_color; }
			set
			{
				meter_right_color = value;
				picMultiMeterDigital.Invalidate();
			}
		}

		private Keys key_tune_up_1 = Keys.Q;
		public Keys KeyTuneUp1
		{
			get{ return key_tune_up_1;}
			set{ key_tune_up_1 = value;}
		}

		private Keys key_tune_down_1 = Keys.A;
		public Keys KeyTuneDown1
		{
			get{ return key_tune_down_1;}
			set{ key_tune_down_1 = value;}
		}

		private Keys key_tune_up_2 = Keys.W;
		public Keys KeyTuneUp2
		{
			get{ return key_tune_up_2;}
			set{ key_tune_up_2 = value;}
		}

		private Keys key_tune_down_2 = Keys.S;
		public Keys KeyTuneDown2
		{
			get{ return key_tune_down_2;}
			set{ key_tune_down_2 = value;}
		}

		private Keys key_tune_up_3 = Keys.E;
		public Keys KeyTuneUp3
		{
			get{ return key_tune_up_3;}
			set{ key_tune_up_3 = value;}
		}

		private Keys key_tune_down_3 = Keys.D;
		public Keys KeyTuneDown3
		{
			get{ return key_tune_down_3;}
			set{ key_tune_down_3 = value;}
		}

		private Keys key_tune_up_4 = Keys.R;
		public Keys KeyTuneUp4
		{
			get{ return key_tune_up_4;}
			set{ key_tune_up_4 = value;}
		}

		private Keys key_tune_down_4 = Keys.F;
		public Keys KeyTuneDown4
		{
			get{ return key_tune_down_4;}
			set{ key_tune_down_4 = value;}
		}

		private Keys key_tune_up_5 = Keys.T;
		public Keys KeyTuneUp5
		{
			get{ return key_tune_up_5;}
			set{ key_tune_up_5 = value;}
		}

		private Keys key_tune_down_5 = Keys.G;
		public Keys KeyTuneDown5
		{
			get{ return key_tune_down_5;}
			set{ key_tune_down_5 = value;}
		}

		private Keys key_tune_up_6 = Keys.Y;
		public Keys KeyTuneUp6
		{
			get{ return key_tune_up_6;}
			set{ key_tune_up_6 = value;}
		}

		private Keys key_tune_down_6 = Keys.H;
		public Keys KeyTuneDown6
		{
			get{ return key_tune_down_6;}
			set{ key_tune_down_6 = value;}
		}

		private Keys key_tune_up_7 = Keys.U;
		public Keys KeyTuneUp7
		{
			get{ return key_tune_up_7;}
			set{ key_tune_up_7 = value;}
		}

		private Keys key_tune_down_7 = Keys.J;
		public Keys KeyTuneDown7
		{
			get{ return key_tune_down_7;}
			set{ key_tune_down_7 = value;}
		}

		private Keys key_rit_up = Keys.O;
		public Keys KeyRITUp
		{
			get { return key_rit_up; }
			set { key_rit_up = value; }
		}

		private Keys key_rit_down = Keys.I;
		public Keys KeyRITDown
		{
			get { return key_rit_down; }
			set { key_rit_down = value; }
		}

		private int rit_step_rate = 50;
		public int RITStepRate
		{
			get { return rit_step_rate; }
			set { rit_step_rate = value; }
		}

		private Keys key_xit_up = Keys.OemOpenBrackets;
		public Keys KeyXITUp
		{
			get { return key_xit_up; }
			set { key_xit_up = value; }
		}

		private Keys key_xit_down = Keys.P;
		public Keys KeyXITDown
		{
			get { return key_xit_down; }
			set { key_xit_down = value; }
		}

		private int xit_step_rate = 50;
		public int XITStepRate
		{
			get { return xit_step_rate; }
			set { xit_step_rate = value; }
		}

		private Keys key_band_up = Keys.M;
		public Keys KeyBandUp
		{
			get{ return key_band_up;}
			set{ key_band_up = value;}
		}

		private Keys key_band_down = Keys.N;
		public Keys KeyBandDown
		{
			get{ return key_band_down;}
			set{ key_band_down = value;}
		}

		private Keys key_filter_up = Keys.B;
		public Keys KeyFilterUp
		{
			get{ return key_filter_up;}
			set{ key_filter_up = value;}
		}

		private Keys key_filter_down = Keys.V;
		public Keys KeyFilterDown
		{
			get{ return key_filter_down;}
			set{ key_filter_down = value;}
		}

		private Keys key_mode_up = Keys.X;
		public Keys KeyModeUp
		{
			get{ return key_mode_up;}
			set{ key_mode_up = value;}
		}

		private Keys key_mode_down = Keys.Z;
		public Keys KeyModeDown
		{
			get{ return key_mode_down;}
			set{ key_mode_down = value;}
		}

		private Keys key_cw_dot = Keys.OemPeriod;
		public Keys KeyCWDot
		{
			get { return key_cw_dot; }
			set { key_cw_dot = value; }
		}

		private Keys key_cw_dash = Keys.OemQuestion;
		public Keys KeyCWDash
		{
			get { return key_cw_dash; }
			set { key_cw_dash = value; }
		}

		public bool MemoryScanner
		{
			get { return chkMemoryScanner.Checked; }
			set
			{
				chkMemoryScanner.Checked = value;
			}
		}

		private bool rfe_present = true;
		public bool RFEPresent
		{
			get { return rfe_present;}
			set
			{
				rfe_present = value;
				Hdw.RFEPresent = value;
				Hdw.UpdateHardware = Hdw.UpdateHardware;
				if(value)
				{
					if(!comboPreamp.Items.Contains("Off"))
						comboPreamp.Items.Insert(0, "Off");
					if(!comboPreamp.Items.Contains("Low"))
						comboPreamp.Items.Insert(1, "Low");
				}
				else
				{
					if(comboPreamp.Items.Contains("Off"))
						comboPreamp.Items.Remove("Off");
					if(comboPreamp.Items.Contains("Low"))
						comboPreamp.Items.Remove("Low");
					if(comboPreamp.SelectedIndex < 0)
						comboPreamp.SelectedIndex = 0;
				}
			}
		}

		private bool xvtr_present = false;
		public bool XVTRPresent
		{
			get { return xvtr_present; }
			set 
			{
				xvtr_present = value;
				btnBand2.Enabled = value;
				Hdw.XVTRPresent = value;
				if(value)
					MaxFreq = 146.0;
				else
					MaxFreq = 65.0;
			}
		}

		private bool pa_present = false;
		public bool PAPresent
		{
			get { return pa_present; }
			set
			{
				pa_present = value;
				Hdw.PAPresent = value;
				if(pa_present && !comboMeterTXMode.Items.Contains("Ref Pwr"))
				{
					comboMeterTXMode.Items.Insert(1, "Ref Pwr");
					if(comboMeterTXMode.SelectedIndex < 0)
						comboMeterTXMode.SelectedIndex = 0;
				}
				else if(!pa_present && comboMeterTXMode.Items.Contains("Ref Pwr"))
				{
					string cur_txt = comboMeterTXMode.Text;
					comboMeterTXMode.Items.Remove("Ref Pwr");
					if(cur_txt != "Ref Pwr")
					{
						comboMeterTXMode.Text = cur_txt;
					}
					else
					{
						if(comboMeterTXMode.SelectedIndex < 0 &&
							comboMeterTXMode.Items.Count > 0)
							comboMeterTXMode.SelectedIndex = 0;
					}
				}
				udPWR_ValueChanged(this, EventArgs.Empty);
			}
		}

		private bool atu_present = false;
		public bool ATUPresent
		{
			get { return atu_present; }
			set
			{
				atu_present = value;
				comboTuneMode.Visible = atu_present;
				comboTuneMode.SelectedIndex = 0;
			}
		}

		private bool usb_present = false;
		public bool USBPresent
		{
			get { return usb_present; }
			set
			{
				usb_present = value;
				Hdw.USBPresent = value;
				if(SetupForm != null)
					SetupForm.USBPresent = value;
			}
		}

		private static PerformanceCounter cpu_usage;
		public static float CpuUsage
		{
			get
			{
				if (cpu_usage == null)
				{
					cpu_usage = new PerformanceCounter(
						"Processor", "% Processor Time", "_Total", true);
				}
				return cpu_usage.NextValue(); 
			}
		}

		// Added 6/11/05 BT to support CAT
		public float MultiMeterCalOffset
		{
			get{return multimeter_cal_offset;}
		}

		public float PreampOffset
		{
			get{return preamp_offset[(int) current_preamp_mode];}
		}


		#endregion

		#region Display Routines

		private void UpdateDisplay()
		{
			switch(current_display_engine)
			{
				case DisplayEngine.GDI_PLUS:
					picDisplay.Invalidate();
					break;
				/*case DisplayEngine.DIRECT_X:
					Display.RenderDirectX();
					break;
					*/
			}
		}

		private void UpdatePeakText()
		{
			if(txtVFOAFreq.Text == "" ||
				txtVFOAFreq.Text == "." ||
				txtVFOAFreq.Text == ",")
				return;

			// update peak value
			float mx, my;
			PixelToSpectrum(Display.MaxX, Display.MaxY, out mx, out my);
			my = Display.MaxY;

			double freq = double.Parse(txtVFOAFreq.Text) + (double)mx*0.0000010;

			if(current_dsp_mode == DSPMode.CWL)
				freq += (double)cw_pitch*0.0000010;
			else if(current_dsp_mode == DSPMode.CWU)
				freq -= (double)cw_pitch*0.0000010;
			
			Display.MaxY = picDisplay.Height;
			txtDisplayPeak.Text = mx.ToString("f1") + "Hz, " + my.ToString("f1") + "dBm, " + freq.ToString("f6") + "MHz";
		}

		private void PixelToSpectrum(float inx, float iny, out float outx, out float outy)
		{
			int low, high;
			if(Display.CurrentDisplayMode == DisplayMode.PANADAPTER)
			{
				low = (int)(-10000*sample_rate1/48000.0);
				high = (int)(10000*sample_rate1/48000.0);
			}
			else
			{
				if(!chkMOX.Checked)
				{
					low = DttSP.RXDisplayLow;
					high = DttSP.RXDisplayHigh;
				}
				else
				{
					low = DttSP.TXDisplayLow;
					high = DttSP.TXDisplayHigh;
				}
			}
			outx = (float)(low + ((double)inx*(high - low)/picDisplay.Width));
			outy = (float)(Display.SpectrumGridMax - iny * (double)(Display.SpectrumGridMax - Display.SpectrumGridMin) / picDisplay.Height);
		}

		#endregion

		#region Paint Event Handlers
		// ======================================================
		// Paint Event Handlers
		// ======================================================

		private void picDisplay_Paint(object sender, PaintEventArgs e)
		{
			//if(chkPower.Checked)
			{
				switch(current_display_engine)
				{
					case DisplayEngine.GDI_PLUS:
						Display.RenderGDIPlus(ref e);
						break;
					case DisplayEngine.DIRECT_X:
						/*Thread t = new Thread(new ThreadStart(Display.RenderDirectX));
						t.Name = "DirectX Background Update";
						t.IsBackground = true;
						t.Priority = ThreadPriority.Normal;
						t.Start();*/
						break;
				}
			}
		}

		private void picMultiMeterDigital_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int H = picMultiMeterDigital.Height;
			int W = picMultiMeterDigital.Width;
			int pixel_x = 0;

			if(!chkMOX.Checked)
			{
				if(meter_data_ready)
				{
					current_meter_data = new_meter_data;
					//meter_data_ready = false;  We do NOT want to do this before we have consumed it!!!!
				}
				
				double num = current_meter_data;

				switch(current_meter_rx_mode)
				{
					case MeterRXMode.SIGNAL_STRENGTH:
					case MeterRXMode.SIGNAL_AVERAGE:
						switch((int)e.Graphics.DpiX)
						{
							case 96:
								double s = (num + 127)/6;
								if(s <= 9.0F)
									pixel_x = (int)((s * 7.5) + 2);
								else
								{
									double over_s9 = num + 73;
									pixel_x = 69 + (int)(over_s9 * 1.05);
								}	
								break;
							case 120:
								if(num <= -97.0f)
									pixel_x = (int)(0+(num+100.0)/3.0*10);
								else if(num <= -91.0f)
									pixel_x = (int)(10+(num+97.0)/6.0*17);
								else if(num <= -85.0f)
									pixel_x = (int)(27+(num+91.0)/6.0*16);
								else if(num <= -79.0f)
									pixel_x = (int)(43+(num+85.0)/6.0*17);
								else if(num <= -73.0f)
									pixel_x = (int)(60+(num+79.0)/6.0*16);
								else if(num <= -53.0f)
									pixel_x = (int)(76+(num+73.0)/20.0*24);
								else if(num <= -33.0f)
									pixel_x = (int)(100+(num+53.0)/20.0*24);
								else if(num <= -13.0f)
									pixel_x = (int)(124+(num+33.0)/20.0*24);
								else
									pixel_x = (int)(148 + (num+13.0)/20.0*19);
								break;
						}
						break;
					case MeterRXMode.ADC_L:
					case MeterRXMode.ADC_R:
						switch((int)e.Graphics.DpiX)
						{
							case 96:
								pixel_x = (int)(((num + 100)*1.2) + 12);
								break;
							case 120:
								if(num <= -100.0f)
									pixel_x = (int)(0+(num+110.0)/10.0*14);
								else if(num <= -80.0f)
									pixel_x = (int)(14+(num+100.0)/20.0*27);
								else if(num <= -60.0f)
									pixel_x = (int)(41+(num+80.0)/20.0*28);
								else if(num <= -40.0f)
									pixel_x = (int)(69+(num+60.0)/20.0*28);
								else if(num <= -20.0f)
									pixel_x = (int)(97+(num+40.0)/20.0*27);
								else if(num <= 0.0f)
									pixel_x = (int)(124+(num+20.0)/20.0*24);
								else
									pixel_x = (int)(148 + (num-0.0)/10.0*19);
								break;
						}
						break;
					case MeterRXMode.OFF:
						pixel_x = 0;
						break;
				}
			}
			else
			{
				if(meter_data_ready)
				{
					current_meter_data = new_meter_data;
					//  meter_data_ready = false;  NOT HERE, wait until consumed
				}

				double num = current_meter_data;

				switch(current_meter_tx_mode)
				{
					case MeterTXMode.MIC:
					case MeterTXMode.EQ:
					case MeterTXMode.LEVELER:
					case MeterTXMode.COMP:
					case MeterTXMode.CPDR:
					case MeterTXMode.ALC:
						num += 3.0;  // number no longer has fudge factor added in the dsp, must be remove
						switch((int)e.Graphics.DpiX)
						{
							case 96:
								if(num <= -20.0f)
									pixel_x = (int)(0+(num+25.0)/5.0*9);
								else if(num <= -10.0f)
									pixel_x = (int)(9+(num+20.0)/10.0*27);
								else if(num <= -5.0f)
									pixel_x = (int)(36+(num+10.0)/5.0*27);
								else if(num <= 0.0f)
									pixel_x = (int)(63+(num+5.0)/5.0*24);
								else if(num <= 1.0f)
									pixel_x = (int)(87+(num-0.0)/1.0*15);
								else if(num <= 2.0f)
									pixel_x = (int)(102+(num-1.0)/1.0*15);
								else if(num <= 3.0f)
									pixel_x = (int)(117+(num-2.0)/1.0*15);
								else
									pixel_x = (int)(132 + (num-3.0)/0.5*8);
								break;
							case 120:
								if(num <= -20.0f)
									pixel_x = (int)(0+(num+25.0)/5.0*10);
								else if(num <= -10.0f)
									pixel_x = (int)(10+(num+20.0)/10.0*30);
								else if(num <= -5.0f)
									pixel_x = (int)(40+(num+10.0)/5.0*30);
								else if(num <= 0.0f)
									pixel_x = (int)(70+(num+5.0)/5.0*27);
								else if(num <= 1.0f)
									pixel_x = (int)(97+(num-0.0)/1.0*17);
								else if(num <= 2.0f)
									pixel_x = (int)(114+(num-1.0)/1.0*17);
								else if(num <= 3.0f)
									pixel_x = (int)(131+(num-2.0)/1.0*17);
								else
									pixel_x = (int)(148 + (num-3.0)/0.5*23);
								break;
						}
						break;
					case MeterTXMode.FORWARD_POWER:
					case MeterTXMode.REVERSE_POWER:
						switch((int)e.Graphics.DpiX)
						{
							case 96:
								if(num <= 1.0f)
									pixel_x = (int)(0 + num*2);
								else if(num <= 5.0f)
									pixel_x = (int)(2 + (num-1)/4*24);
								else if(num <= 10.0f)
									pixel_x = (int)(26 + (num-5)/5*24);
								else if(num <= 50.0f)
									pixel_x = (int)(50 + (num-10)/40*24);
								else if(num <= 100.0f)
									pixel_x = (int)(74 + (num-50)/50*24);
								else if(num <= 120.0f)
									pixel_x = (int)(98 + (num-100)/20*24);
								else
									pixel_x = (int)(122 + (num-120)/20*16);
								break;
							case 120:
								if(num <= 1.0f)
									pixel_x = (int)(0 + num*3);
								else if(num <= 5.0f)
									pixel_x = (int)(3 + (num-1)/4*26);
								else if(num <= 10.0f)
									pixel_x = (int)(29 + (num-5)/5*26);
								else if(num <= 50.0f)
									pixel_x = (int)(55 + (num-10)/40*27);
								else if(num <= 100.0f)
									pixel_x = (int)(82 + (num-50)/50*28);
								else if(num <= 120.0f)
									pixel_x = (int)(110 + (num-100)/20*27);
								else
									pixel_x = (int)(137 + (num-120)/20*30);
								break;
						}
						break;
					case MeterTXMode.SWR:
						switch((int)e.Graphics.DpiX)
						{
							case 96:
								if(double.IsInfinity(num))
									pixel_x = 200;
								else if(num <= 1.0f)
									pixel_x = (int)(0 + num * 3);
								else if(num <= 1.5f)
									pixel_x = (int)(3 + (num-1.0)/0.5*27);
								else if(num <= 2.0f)
									pixel_x = (int)(30 + (num-1.5)/0.5*20);
								else if(num <= 3.0f)
									pixel_x = (int)(50 + (num-2.0)/1.0*21);
								else if(num <= 5.0f)
									pixel_x = (int)(71 + (num-3.0)/2.0*21);
								else if(num <= 10.0f)
									pixel_x = (int)(92 + (num-5.0)/5.0*21);
								else
									pixel_x = (int)(113 + (num-10.0)/15.0*26);
								break;
							case 120:
								if(double.IsInfinity(num))
									pixel_x = 200;
								else if(num <= 1.0f)
									pixel_x = (int)(0 + num * 3);
								else if(num <= 1.5f)
									pixel_x = (int)(3 + (num-1.0)/0.5*31);
								else if(num <= 2.0f)
									pixel_x = (int)(34 + (num-1.5)/0.5*22);
								else if(num <= 3.0f)
									pixel_x = (int)(56 + (num-2.0)/1.0*22);
								else if(num <= 5.0f)
									pixel_x = (int)(78 + (num-3.0)/2.0*23);
								else if(num <= 10.0f)
									pixel_x = (int)(101 + (num-5.0)/5.0*23);
								else
									pixel_x = (int)(124 + (num-10.0)/15.0*43);
								break;
						}
						break;
					case MeterTXMode.ALC_G:
					case MeterTXMode.LVL_G:
						switch((int)e.Graphics.DpiX)
						{
							case 96:
								if(num <= 0.0f)
									pixel_x = 3;
								else if(num <= 5.0f)
									pixel_x = (int)(3 + (num-0.0)/5.0*28);
								else if(num <= 10.0f)
									pixel_x = (int)(31 + (num-5.0)/5.0*29);
								else if(num <= 15.0f)
									pixel_x = (int)(60 + (num-10.0)/5.0*30);
								else if(num <= 20.0f)
									pixel_x = (int)(90 + (num-15.0)/5.0*31);
								else
									pixel_x = (int)(121 + (num-20.0)/5.0*29);
								break;
							case 120:
								if(num <= 0.0f)
									pixel_x = 3;
								else if(num <= 5.0f)
									pixel_x = (int)(3 + (num-0.0)/5.0*31);
								else if(num <= 10.0f)
									pixel_x = (int)(34 + (num-5.0)/5.0*33);
								else if(num <= 15.0f)
									pixel_x = (int)(77 + (num-10.0)/5.0*33);
								else if(num <= 20.0f)
									pixel_x = (int)(110 + (num-15.0)/5.0*35);
								else
									pixel_x = (int)(145 + (num-20.0)/5.0*32);
								break;
						}
						break;
					case MeterTXMode.OFF:
						pixel_x = 0;
						break;
				}
			}
			if(meter_data_ready)
			{
				meter_data_ready = false;  //We do NOT want to do this before we have consumed it!!!! so do it here.
			}

			switch((int)e.Graphics.DpiX)
			{
				case 96:
					if(pixel_x > 139) pixel_x = 139;
					break;
				case 120:
					if(pixel_x > 167) pixel_x = 167;
					break;
			}

			if((!chkMOX.Checked && current_meter_rx_mode != MeterRXMode.OFF) ||
				(chkMOX.Checked && current_meter_tx_mode != MeterTXMode.OFF))
			{
				if(pixel_x <= 0) pixel_x = 1;

				LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, pixel_x, H),
					meter_left_color, meter_right_color, LinearGradientMode.Horizontal);
				Graphics g = e.Graphics;
				g.FillRectangle(brush, 0, 0, pixel_x, H);
							
				for(int i=0; i<21; i++)
					g.DrawLine(new Pen(Color.Black), 6+i*8, 0, 6+i*8, H);

				g.DrawLine(new Pen(Color.Red), pixel_x, 0, pixel_x, H);
				g.FillRectangle(new SolidBrush(Color.Black), pixel_x+1, 0, W-pixel_x, H);

				if(pixel_x >= meter_peak_value)
				{
					meter_peak_count = 0;
					meter_peak_value = pixel_x;
				}
				else
				{
					if(meter_peak_count++ >= multimeter_peak_hold_samples)
					{
						meter_peak_count = 0;
						meter_peak_value = pixel_x;
					}
					else
					{
						g.DrawLine(new Pen(Color.Red), meter_peak_value, 0, meter_peak_value, H);
						g.DrawLine(new Pen(Color.Red), meter_peak_value-1, 0, meter_peak_value-1, H);
					}
				}
			}
		}

		private void ResetMultiMeterPeak()
		{
			meter_peak_count = multimeter_peak_hold_samples;
		}

		private void panelVFOAHover_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if(vfoa_hover_digit < 0)
				return;

			int x = 0;

			x += (vfo_char_width+vfo_char_space)*vfoa_hover_digit;
			if(vfoa_hover_digit > 3)
				x += (vfo_decimal_space-vfo_char_space);

			e.Graphics.DrawLine(new Pen(txtVFOAFreq.ForeColor, 2.0f), x, 1, x+vfo_char_width, 1);
		}

		private void panelVFOBHover_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if(vfob_hover_digit < 0)
				return;

			int x = 0;

			x += (vfo_char_width+vfo_char_space)*vfob_hover_digit;
			if(vfob_hover_digit > 3)
				x += (vfo_decimal_space-vfo_char_space);

			e.Graphics.DrawLine(new Pen(txtVFOBFreq.ForeColor, 2.0f), x, 1, x+vfo_char_width, 1);
		}

		public void UpdateDisplayAverage() 
		{
			double dttsp_osc = DttSP.RXOsc; 
			// Debug.WriteLine("last vfo: " + avg_last_ddsfreq + " vfo: " + DDSFreq); 
			if(Display.average_buffer[0] == Display.CLEAR_FLAG)
			{
				//Debug.WriteLine("Clearing average buf"); 
				for(int i=0; i<Display.BUFFER_SIZE; i++)
					Display.average_buffer[i] = Display.current_display_data[i];
			}
			else
			{					
				// wjt added -- stop hosing the avg display when scrolling the vfo 
				if((avg_last_ddsfreq != 0 && avg_last_ddsfreq != DDSFreq) || 
					(current_model == Model.SOFTROCK40 &&
					avg_last_dttsp_osc != dttsp_osc))   // vfo has changed, need to shift things around 
				{ 
					//Debug.WriteLine("dttsp_osc: " + dttsp_osc); 
					double delta_vfo; 
					if ( current_model != Model.SOFTROCK40 ) 
					{ 					
						delta_vfo = DDSFreq - avg_last_ddsfreq;
						delta_vfo *= 1e6; // vfo in mhz moron!
					}
					else 
					{ 						
						delta_vfo = dttsp_osc - avg_last_dttsp_osc; 
						delta_vfo = -delta_vfo; 
						//Debug.WriteLine("update from dttsp delta_vfo: " + delta_vfo); 
					} 					
					double hz_per_bin = DttSP.SampleRate/Display.BUFFER_SIZE; 

					int bucket_shift = (int)(delta_vfo/hz_per_bin); 
					double leftover = delta_vfo - ((double)bucket_shift *  hz_per_bin); 
					leftover = leftover/hz_per_bin; // conver to fractions of bucket 
					double total_leftover = leftover + last_bin_shift_leftover; 
					if ( total_leftover < -0.5 ) 
					{ 
						bucket_shift -= 1; 
						total_leftover += 1;
						//Debug.WriteLine("bump down"); 
					} 
					else if  ( total_leftover > 0.5 ) 
					{  
						bucket_shift += 1; 
						total_leftover -= 1;
						//Debug.WriteLine("bump up"); 
					}
					last_bin_shift_leftover = total_leftover; 
					//Debug.WriteLine("leftover: " + leftover + " total_leftover: " + total_leftover); 
					
					// bucket_shift = bucket_shift/2; 						
					//					indexed_value pre_max = findMax(average_buffer, display_buffer_size); 
					//					Debug.WriteLine("\nPre max: " + pre_max.val + " " + pre_max.idx); 
					//					Debug.WriteLine("bshift: " + bucket_shift + " delta_vfo: " + delta_vfo); 
					if ( bucket_shift > 0 ) // vfo increased, need to shift avgs to the left 
					{ 
						if ( bucket_shift >= Display.BUFFER_SIZE )  
						{ 
							Display.average_buffer[0] = Display.CLEAR_FLAG; 
						} 
						else 
						{ 
							for ( int j = 0; j < Display.BUFFER_SIZE - bucket_shift; j++ ) 
								Display.average_buffer[j]  = Display.average_buffer[j+bucket_shift];  // wjt fix use memmove 

							// fill avg with last good data on the end
							for ( int j = Display.BUFFER_SIZE - bucket_shift; j < Display.BUFFER_SIZE; j++ ) 
								Display.average_buffer[j] = Display.average_buffer[Display.BUFFER_SIZE - bucket_shift - 1];  
						}
					} 
					else if ( bucket_shift < 0 ) // vfo decreased, move samples up 
					{ 
						if ( -bucket_shift >= Display.BUFFER_SIZE ) 
						{
							Display.average_buffer[0] = Display.CLEAR_FLAG; 
						}
						else 
						{
							for ( int j = Display.BUFFER_SIZE - 1; j > -bucket_shift; j-- ) 
								Display.average_buffer[j] = Display.average_buffer[j+bucket_shift]; 								

							for ( int j = 0; j  < -bucket_shift; j++ ) 
								Display.average_buffer[j] = Display.average_buffer[-bucket_shift]; 
						}
					}						
					//					indexed_value post_max = findMax(average_buffer, display_buffer_size); 
					//					Debug.WriteLine("Post max: " + post_max.val + " " + post_max.idx); 
					//					indexed_value disp_max = findMax(display_data, display_buffer_size); 		
					//					Debug.WriteLine("Disp max: " + disp_max.val + " " + disp_max.idx); 
				}
				else
				{
					last_bin_shift_leftover = 0; // reset, this vfo = last vfo 
				} 

				for(int i=0; i<Display.BUFFER_SIZE; i++)
					Display.average_buffer[i] = Display.current_display_data[i] = 
						(float)(Display.current_display_data[i]*Display.display_avg_mult_new + 
						Display.average_buffer[i]*Display.display_avg_mult_old);					
			}			

			if ( Display.average_buffer[0] == Display.CLEAR_FLAG )  
			{ 
				avg_last_ddsfreq = 0; 
				avg_last_dttsp_osc = 0; 
			}
			else 
			{
				avg_last_ddsfreq = DDSFreq; 
				avg_last_dttsp_osc = dttsp_osc; 
			}
		}

		#endregion

		#region Thread and Timer Routines
		// ======================================================
		// Thread Routines
		// ======================================================

		private void RunDisplay()
		{
//			display_running = true;
			while(chkPower.Checked && Display.CurrentDisplayMode != DisplayMode.OFF)
			{
				if(!Display.DataReady)
				{
					switch(Display.CurrentDisplayMode)
					{
						case DisplayMode.SPECTRUM:
						case DisplayMode.HISTOGRAM:
						case DisplayMode.WATERFALL:
							fixed(float* ptr = &Display.new_display_data[0])
								DttSP.GetSpectrum(ptr);
							break;
						case DisplayMode.PANADAPTER:
							fixed(float* ptr = &Display.new_display_data[0])
								DttSP.GetPanadapter(ptr);
							break;
						case DisplayMode.SCOPE:
							fixed(float *ptr = &Display.new_display_data[0])
								DttSP.GetScope(ptr, (int)(scope_time*48));
							break;
						case DisplayMode.PHASE:
							fixed(float *ptr = &Display.new_display_data[0])
								DttSP.GetPhase(ptr, Display.PhaseNumPts);
							break;
						case DisplayMode.PHASE2:
							Audio.phase_mutex.WaitOne();
							for(int i=0; i<Display.PhaseNumPts; i++)
							{
								Display.new_display_data[i*2]   = Audio.phase_buf_l[i];
								Display.new_display_data[i*2+1] = Audio.phase_buf_r[i];
							}
							Audio.phase_mutex.ReleaseMutex();
							break;
					}
					Display.DataReady = true;
				}

				UpdateDisplay();
				
				if(chkPower.Checked)
					Thread.Sleep(display_delay);
			}	
//			display_running = false;
		}

		private void UpdateMultimeter()
		{
//			multimeter_running = true;
			while(chkPower.Checked)
			{
				string output = "";
				if(!meter_data_ready)
				{
					if(!chkMOX.Checked)
					{
						if(Audio.CurrentAudioState1 != Audio.AudioState.DTTSP)
							goto end;

						MeterRXMode mode = CurrentMeterRXMode;
						float num = 0f;
						switch(mode)
						{
							case MeterRXMode.SIGNAL_STRENGTH:
								num = DttSP.CalculateMeter(DttSP.MeterType.SIGNAL_STRENGTH);
								num = num +
									multimeter_cal_offset +
									preamp_offset[(int)current_preamp_mode] +
									filter_size_cal_offset;
								output = num.ToString("f1")+" dBm";
								new_meter_data = num;
								break;
							case MeterRXMode.SIGNAL_AVERAGE:
								num = DttSP.CalculateMeter(DttSP.MeterType.AVG_SIGNAL_STRENGTH);
								num = num +
									multimeter_cal_offset +
									preamp_offset[(int)current_preamp_mode] +
									filter_size_cal_offset;
								output = num.ToString("f1")+" dBm";
								new_meter_data = num;
								break;
							case MeterRXMode.ADC_L:
								num = DttSP.CalculateMeter(DttSP.MeterType.ADC_REAL);
								output = num.ToString("f1")+" dBFS";
								new_meter_data = num;
								break;
							case MeterRXMode.ADC_R:
								num = DttSP.CalculateMeter(DttSP.MeterType.ADC_IMAG);
								output = num.ToString("f1")+" dBFS";
								new_meter_data = num;
								break;
							case MeterRXMode.OFF:
								output = "";
								new_meter_data = num;
								break;
						}
					}
					else
					{
						MeterTXMode mode = CurrentMeterTXMode;
						float num = 0f;
						double power = 0.0;

						switch(mode)
						{
							case MeterTXMode.MIC:
								if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
								{
									num = (float)Math.Max(-25.0, -DttSP.CalculateMeter(DttSP.MeterType.MIC));
									output = num.ToString("f1")+" dB";
								}
								else output = "0"+separator+"0 dB";
								new_meter_data = num;
								break;
							case MeterTXMode.EQ:
								if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
								{
									num = (float)Math.Max(-25.0, -DttSP.CalculateMeter(DttSP.MeterType.EQ));
									output = num.ToString("f1")+" dB";
								}
								else output = "0"+separator+"0 dB";
								new_meter_data = num;
								break;
							case MeterTXMode.LEVELER:
								if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
								{
									num = (float)Math.Max(-25.0, -DttSP.CalculateMeter(DttSP.MeterType.LEVELER));
									output = num.ToString("f1")+" dB";
								}
								else output = "0"+separator+"0 dB";
								new_meter_data = num;
								break;
							case MeterTXMode.LVL_G:
								if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
								{
									num = (float)Math.Max(0, DttSP.CalculateMeter(DttSP.MeterType.LVL_G));
									output = num.ToString("f1")+" dB";
								}
								else output = "0"+separator+"0 dB";
								new_meter_data = num;
								break;
							case MeterTXMode.COMP:
								if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
								{
									num = (float)Math.Max(-25.0, -DttSP.CalculateMeter(DttSP.MeterType.COMP));
									output = num.ToString("f1")+" dB";
								}
								else output = "0"+separator+"0 dB";
								new_meter_data = num;
								break;
							case MeterTXMode.CPDR:
								if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
								{
									num = (float)Math.Max(-25.0, -DttSP.CalculateMeter(DttSP.MeterType.CPDR));
									output = num.ToString("f1")+" dB";
								}
								else output = "0"+separator+"0 dB";
								new_meter_data = num;
								break;
							case MeterTXMode.ALC:
								if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
								{
									num = (float)Math.Max(-25.0, -DttSP.CalculateMeter(DttSP.MeterType.ALC));
									output = num.ToString("f1")+" dB";
								}
								else output = "0"+separator+"0 dB";
								new_meter_data = num;
								break;
							case MeterTXMode.ALC_G:
								if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
								{
									num = (float)Math.Max(0, -DttSP.CalculateMeter(DttSP.MeterType.ALC_G));
									output = num.ToString("f1")+" dB";
								}
								else output = "0"+separator+"0 dB";
								new_meter_data = num;
								break;
							case MeterTXMode.FORWARD_POWER:
								if(pa_present && VFOAFreq < 30.0)
								{
									pa_power_mutex.WaitOne();
									power = PAPower(pa_fwd_power);
									pa_power_mutex.ReleaseMutex();
							
									output = power.ToString("f0")+" W";
									new_meter_data = (float)power;
								}
								else
								{
									if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
									{
										num = (float)Math.Max(0.0,DttSP.CalculateMeter(DttSP.MeterType.PWR));
										num *= (float)((double)udPWR.Value*0.01);
										output = num.ToString("f2")+" W";
									}
									else output = "0"+separator+"00 W";
									new_meter_data = num;
								}
								break;
							case MeterTXMode.REVERSE_POWER:
								pa_power_mutex.WaitOne();
								power = PAPower(pa_rev_power);
								output = power.ToString("f0")+" W";
								pa_power_mutex.ReleaseMutex();
								new_meter_data = (float)power;
								break;
							case MeterTXMode.SWR:
								double swr = 0.0;
								pa_power_mutex.WaitOne();
								swr = SWR(pa_fwd_power, pa_rev_power);
								output = swr.ToString("f1")+" : 1";
								pa_power_mutex.ReleaseMutex();
								new_meter_data = (float)swr;
								break;
							case MeterTXMode.OFF:
								output = "";
								new_meter_data = num;
								break;
						}
					}
					meter_data_ready = true;
					txtMultiText.Text = output;
					picMultiMeterDigital.Invalidate();
				}			

			end:
				if(chkPower.Checked)
					Thread.Sleep(meter_delay);
			}
//			multimeter_running = false;
		}

		private static HiPerfTimer vox_timer = new HiPerfTimer();

		private void PollPTT()
		{
//			poll_ptt_running = true;
			while(chkPower.Checked)
			{
				if(!manual_mox && !disable_ptt)
				{
					byte b = Hdw.StatusPort();
					//Debug.WriteLine("Parallel: "+b);

					if(auto_mute)
					{
						bool mute = !((b & (byte)StatusPin.PIN_12) != 0);
						if(chkMUT.Checked != mute) chkMUT.Checked = mute;
					}
                    
					bool mic_ptt = (b & (byte)StatusPin.Dot) != 0;
					bool x2_ptt = (b & (byte)StatusPin.PIN_11) != 0;
					bool cw_ptt = (DttSP.KeyerPlaying() != 0) | Keyer.KeyerPTT | Keyer.MemoryPTT;
					bool cat_ptt = false;
					bool vox_ptt = Audio.VOXActive;
					if(usb_present) x2_ptt = !x2_ptt;

					// wjt added
					if (PTTBitBangEnabled && serialPTT != null && serialPTT.isPTT()) 
						cat_ptt = true; 
					// wjt added ends 

					if (cw_ptt) break_in_timer.Start();

					if(!chkMOX.Checked)
					{
						if(x2_ptt)
						{
							current_ptt_mode = PTTMode.X2;
							if(current_dsp_mode == DSPMode.CWL ||
								current_dsp_mode == DSPMode.CWU)
							{
								if(!new_keyer)
								{
									if(cw_key_mode) 
										CWForm.Send = true;
								}
								else
								{
									if(!cw_disable_monitor)
										chkMON.Checked = true;
									chkMOX.Checked = true;
									if(!chkMOX.Checked)
									{
//										poll_ptt_running = false;
										chkPower.Checked = false;
									}
								}
							}
							else chkMOX.Checked = true;
						}

						if(cat_ptt)
						{
							current_ptt_mode = PTTMode.CAT;
							if(current_dsp_mode == DSPMode.CWL ||
								current_dsp_mode == DSPMode.CWU)
							{
								if(!new_keyer)
								{
									if(cw_key_mode) 
										CWForm.Send = true;
								}
								else
								{
									if(!cw_disable_monitor)
										chkMON.Checked = true;
									chkMOX.Checked = true;
									if(!chkMOX.Checked)
									{
//										poll_ptt_running = false;
										chkPower.Checked = false;
									}
								}
							}
							else chkMOX.Checked = true;
						}

						if((current_dsp_mode == DSPMode.CWL ||
							current_dsp_mode == DSPMode.CWU) &&
							cw_ptt)
						{
							if(!new_keyer)
							{
								current_ptt_mode = PTTMode.CW;
								if(cw_key_mode) 
									CWForm.Send = true;
							}
							else
							{
								if(Keyer.PrimaryConnPort == "SDR" &&
									Keyer.SecondaryConnPort == "None" &&
									!cw_semi_break_in_enabled)
								{
									// do nothing
								}
								else
								{									
									current_ptt_mode = PTTMode.CW;
									if(!cw_disable_monitor)
										chkMON.Checked = true;
									chkMOX.Checked = true;
									if(!chkMOX.Checked)
									{
										//poll_ptt_running = false;
										chkPower.Checked = false;
									}
								}								
							}
						}
						
						if((current_dsp_mode == DSPMode.LSB ||
							current_dsp_mode == DSPMode.USB ||
							current_dsp_mode == DSPMode.DSB ||
							current_dsp_mode == DSPMode.AM  ||
							current_dsp_mode == DSPMode.SAM ||
							current_dsp_mode == DSPMode.DIGU ||
							current_dsp_mode == DSPMode.DIGL ||
							current_dsp_mode == DSPMode.FMN) &&
							mic_ptt)
						{
							current_ptt_mode = PTTMode.MIC;
							chkMOX.Checked = true;
							if(!chkMOX.Checked)
							{
//								poll_ptt_running = false;
								chkPower.Checked = false;
							}
						}

						if((current_dsp_mode == DSPMode.LSB ||
							current_dsp_mode == DSPMode.USB ||
							current_dsp_mode == DSPMode.DSB ||
							current_dsp_mode == DSPMode.AM  ||
							current_dsp_mode == DSPMode.SAM ||
							current_dsp_mode == DSPMode.DIGU ||
							current_dsp_mode == DSPMode.DIGL ||
							current_dsp_mode == DSPMode.FMN) &&
							vox_ptt)
						{
							current_ptt_mode = PTTMode.VOX;
							vox_timer.Start();
							chkMOX.Checked = true;
							if(!chkMOX.Checked)
								chkPower.Checked = false;
						}
					}
					else // if(!chkMOX.Checked)
					{
						switch(current_ptt_mode)
						{
							case PTTMode.X2:
								if(!x2_ptt)
								{
									chkMOX.Checked = false;
									if((current_dsp_mode == DSPMode.CWL ||
										current_dsp_mode == DSPMode.CWU) &&
										!cw_disable_monitor)
										chkMON.Checked = false;
								}
								break;
							case PTTMode.CAT:
								if(!cat_ptt)
								{
									chkMOX.Checked = false;
									if((current_dsp_mode == DSPMode.CWL ||
										current_dsp_mode == DSPMode.CWU) &&
										!cw_disable_monitor)
										chkMON.Checked = false;
								}
								break;
							case PTTMode.MIC:
								if(!mic_ptt)
									chkMOX.Checked = false;
								break;
							case PTTMode.CW:
								if(!cw_ptt)
								{
									if (cw_semi_break_in_enabled)
									{
										break_in_timer.Stop();
										if (break_in_timer.DurationMsec > break_in_delay)
										{
											chkMOX.Checked = false;
											if((current_dsp_mode == DSPMode.CWL ||
												current_dsp_mode == DSPMode.CWU) &&
												!cw_disable_monitor)
												chkMON.Checked = false;
										}
									} 
									else
									{
										break_in_timer.Stop();
										if (break_in_timer.DurationMsec > 43)
										{
											chkMOX.Checked = false;
											if((current_dsp_mode == DSPMode.CWL ||
												current_dsp_mode == DSPMode.CWU) &&
												!cw_disable_monitor)
												chkMON.Checked = false;
										}
									}
								}
								break;
							case PTTMode.VOX:
								if(!vox_ptt)
								{
									vox_timer.Stop();
									if(vox_timer.DurationMsec > vox_hang_time)
										chkMOX.Checked = false;
								}
								else vox_timer.Start();
								break;
						}
					}	
				}

				Thread.Sleep(8);
			}
//			poll_ptt_running = false;
		}

		private void PollPAPWR()
		{
			int first_time_count = 0;
			int high_swr_count = 0;

			while(chkPower.Checked && pa_present)
			{
				if(chkMOX.Checked)
				{
					if(!atu_tuning)
					{
						if(first_time_count++ >= 5)
						{
							pa_power_mutex.WaitOne();

							int temp = (int)Hdw.PA_GetADC(0);
							if(temp >= 0) pa_fwd_power = temp;
							temp = (int)Hdw.PA_GetADC(1);
							if(temp >= 0) pa_rev_power = temp;

							pa_power_mutex.ReleaseMutex();

							//pa_fwd_power = (int)numericUpDown1.Value;
							//pa_rev_power = (int)numericUpDown2.Value;

							//Debug.WriteLine("fwd: "+pa_fwd_power.ToString()+"  rev: "+pa_rev_power.ToString());

							double fwd_power = PAPower(pa_fwd_power);
							double rev_power = PAPower(pa_rev_power);

							if(rev_power >= 11)
							{
								high_swr_count++;
								if((high_swr_count >= 3 && !chkTUN.Checked) ||
									(high_swr_count >= 25 && chkTUN.Checked))
								{
									HighSWR = true;
									UpdateDisplay();
								}
							}
							else
								high_swr_count = 0;							

							if(high_swr && !disable_swr_protection)
							{
								if(Audio.HighSWRScale != 0.1)
									Audio.HighSWRScale = 0.1;
							}

							if((!chkTUN.Checked && (first_time_count % 3) == 0) ||
								(chkTUN.Checked && (first_time_count % 55) == 0))
								high_swr_count = 0;
						}
					}
				}
				else 
				{
					pa_power_mutex.WaitOne();
					pa_fwd_power = 0;
					pa_rev_power = 0;
					first_time_count = 0;
					HighSWR = false;
					pa_power_mutex.ReleaseMutex();					
				}

				Thread.Sleep(100);
				// roughly 132ms loop
			}
		}


		private void timer_scanner_Tick(object sender, System.EventArgs e)
		{
			if(radScannerFreq.Checked)
			{
				if(VFOAFreq >= (double)udScannerHigh.Value)
				{
					timer_scanner.Enabled = false;
					chkMemoryScanner.Checked = false;
				}
				else
					VFOAFreq += (double)udScannerStep.Value;
			}
			else
			{
				if(!MemForm.ScanMemory(false))
				{
					timer_scanner.Enabled = false;
					chkMemoryScanner.Checked = false;
				}
			}
		}

		private void timer_cpu_meter_Tick(object sender, System.EventArgs e)
		{
			lblCPUMeter.Text = "CPU %: " + CpuUsage.ToString("f1");
		}

		private void timer_peak_text_Tick(object sender, System.EventArgs e)
		{
			switch(Display.CurrentDisplayMode)
			{
				case DisplayMode.HISTOGRAM:
				case DisplayMode.PANADAPTER:
				case DisplayMode.SPECTRUM:
				case DisplayMode.WATERFALL:
					UpdatePeakText();
					break;
				default:
					txtDisplayPeak.Text = "";
					break;
			}
		}

		private int last_sec;		// for time of day clock
		private DateTime last_date;	// for date
		private void timer_clock_Tick(object sender, System.EventArgs e)
		{
			
			switch(current_datetime_mode)
			{
				case DateTimeMode.LOCAL:
					DateTime date = DateTime.Now.Date;
					if(date != last_date || txtDate.Text == "")
					{
						last_date = date;
						txtDate.Text = DateTime.Now.ToShortDateString();
					}

					int sec = DateTime.Now.Second;
					if(sec != last_sec)
					{
						last_sec = sec;
						txtTime.Text = "LOC "+DateTime.Now.ToString("HH:mm:ss");
					}
					break;
				case DateTimeMode.UTC:
					date = DateTime.UtcNow.Date;
					if(date != last_date || txtDate.Text == "")
					{
						last_date = date;
						txtDate.Text = DateTime.UtcNow.ToShortDateString();
					}

					sec = DateTime.UtcNow.Second;
					if(sec != last_sec)
					{
						last_sec = sec;
						txtTime.Text = "UTC "+DateTime.UtcNow.ToString("HH:mm:ss");
					}
					break;
				case DateTimeMode.OFF:
					txtDate.Text = "";
					txtTime.Text = "";
					break;
			}
		}

		private void Tune()
		{
			switch((ATUTuneMode)comboTuneMode.SelectedIndex)
			{
				case ATUTuneMode.MEMORY:
					udPWR.Value = 15;
					atu_tuning = true;
					if(!Hdw.PA_ATUTune(ATUTuneMode.MEMORY))
						goto atu_error;
					atu_tuning = false;
					tuned_band = current_band;
					break;
				case ATUTuneMode.FULL:
					udPWR.Value = 15;
					atu_tuning = true;
					if(!Hdw.PA_ATUTune(ATUTuneMode.FULL))
						goto atu_error;
					atu_tuning = false;
					tuned_band = current_band;
					break;
			}

			chkTUN.Checked = false;
			chkTUN.Enabled = true;
			comboTuneMode.Enabled = true;
			chkTUN.BackColor = button_selected_color;
			return;
            
			atu_error:
				chkTUN.Checked = false;
			chkTUN.Enabled = true;
			MessageBox.Show("Error communicating with the ATU",
				"ATU Error",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);			
		}

		private void DelayedDisplayReset()
		{
			Thread.Sleep((int)((double)block_size1/(double)sample_rate1*1000.0));
			Display.ResetDisplayAverage();
		}

		#endregion

		#region Event Handlers
		// ======================================================
		// Event Handlers
		// ======================================================

		// Console Events

		private void picDisplay_Resize(object sender, System.EventArgs e)
		{
			Display.Target = picDisplay;
			Display.Init();
			Display.DrawBackground();
			UpdateDisplay();
		}

		private void Console_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)Keys.Enter)
				btnHidden.Focus();
		}

		private void Console_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.Shift == false)
				shift_down = false;
			
			if(e.KeyCode == key_cw_dot)
			{
				CWForm.KBDot = false;
			}
			else if(e.KeyCode == key_cw_dash)
			{
				CWForm.KBDash = false;
			}
		}

		private void Console_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.Shift == true)
				shift_down = true;

			if(e.Control == true && e.Shift == true && e.KeyCode == Keys.T && pa_present)
			{
				if(PAQualForm == null || PAQualForm.IsDisposed)
					PAQualForm = new PAQualify(this);
				PAQualForm.Show();
				PAQualForm.Focus();
			}
			else if(e.Control == true && e.Shift == true && e.KeyCode == Keys.P)
			{
				if(ProdTestForm == null || ProdTestForm.IsDisposed)
					ProdTestForm = new ProductionTest(this);
				ProdTestForm.Show();
				ProdTestForm.Focus();
			}
			else if(e.Control && e.Alt && e.KeyCode == Keys.U)
			{
				mnuUCB_Click(this, EventArgs.Empty);
			}
			else if(e.Control && !e.Alt)		// control key is pressed
			{
				switch(e.KeyCode)
				{
					case Keys.Left:
						ChangeWheelTuneLeft();
						e.Handled = true;
						break;
					case Keys.Right:
						ChangeWheelTuneRight();
						e.Handled = true;
						break;
					case Keys.Up:
						Console_MouseWheel(this, new MouseEventArgs(MouseButtons.None, 0, 0, 0, 120));
						e.Handled = true;
						break;
					case Keys.Down:
						Console_MouseWheel(this, new MouseEventArgs(MouseButtons.None, 0, 0, 0, -120));
						e.Handled = true;
						break;
					case Keys.A:
						if(CurrentAGCMode == AGCMode.FAST)
							CurrentAGCMode = AGCMode.FIXD;
						else CurrentAGCMode++;
						break;
					case Keys.C:
						btnMemoryQuickSave_Click(this, EventArgs.Empty);
						break;
					case Keys.D:
						switch(Display.CurrentDisplayMode)
						{
							case DisplayMode.PANADAPTER:
								comboDisplayMode.Text = "Spectrum";
								break;
							case DisplayMode.SPECTRUM:
								comboDisplayMode.Text = "Panadapter";
								break;
							default:
								comboDisplayMode.Text = "Panadapter";
								break;
						}
						break;
					case Keys.V:
						btnMemoryQuickRestore_Click(this, EventArgs.Empty);
						break;
					case Keys.M:
						if(chkMOX.Enabled)
							chkMOX.Checked = !chkMOX.Checked;
						break;
					case Keys.P:
						CurrentPreampMode = (PreampMode)(((int)current_preamp_mode+1)%(int)PreampMode.LAST);
						break;
					case Keys.S:
						if(chkVFOSplit.Enabled)
							chkVFOSplit.Checked = !chkVFOSplit.Checked;
						break;
					case Keys.F:
						if(tbFilterWidth.Value != tbFilterWidth.Minimum)
						{
							tbFilterWidth.Value--;
							tbFilterWidth_Scroll(this, EventArgs.Empty);
						}
						break;
					case Keys.L:
						if(chkVFOLock.Enabled)
							chkVFOLock.Checked = !chkVFOLock.Checked;
						break;
					case Keys.W:
						CWForm.WPM_Down();
						break;
				}
			}
			else if(e.Alt && !e.Control)
			{
				switch(e.KeyCode)
				{
					case Keys.F:
						if(tbFilterWidth.Value != tbFilterWidth.Maximum)
						{
							tbFilterWidth.Value++;
							tbFilterWidth_Scroll(this, EventArgs.Empty);
						}
						break;
					case Keys.G:
						btnVFOAtoB_Click(this, EventArgs.Empty);
						break;
					case Keys.H:
						btnVFOBtoA_Click(this, EventArgs.Empty);
						break;
					case Keys.I:
						btnFilterShiftReset_Click(this, EventArgs.Empty);
						break;
					case Keys.Q:
						CWForm.WPM_Up();
						break;
					case Keys.R:
						btnRITReset_Click(this, EventArgs.Empty);
						break;
					case Keys.T:
						chkTUN.Checked = !chkTUN.Checked;
						break;
					case Keys.V:
						btnVFOSwap_Click(this, EventArgs.Empty);
						break;
					case Keys.X:
						btnXITReset_Click(this, EventArgs.Empty);
						break;
					case Keys.Z:
						if(btnZeroBeat.Enabled)
							btnZeroBeat_Click(this, EventArgs.Empty);
						break;
				}
			}
			else if(!e.Alt && !e.Control)
			{
				if(this.ActiveControl is TextBoxTS) return;
				if(this.ActiveControl is NumericUpDownTS) return;

				switch(e.KeyCode)
				{
					case Keys.Multiply:
						chkMUT.Checked = !chkMUT.Checked;
						break;
					case Keys.Add:
						if(udAF.Value != udAF.Maximum)
							udAF.Value++;
						break;
					case Keys.Subtract:
						if(udAF.Value != udAF.Minimum)
							udAF.Value--;
						break;
					case Keys.K:
						if(tbFilterShift.Value != tbFilterShift.Minimum)
							tbFilterShift.Value--;
						tbFilterShift_Scroll(this, EventArgs.Empty);
						break;
					case Keys.L:
						if(tbFilterShift.Value != tbFilterShift.Maximum)
							tbFilterShift.Value++;
						tbFilterShift_Scroll(this, EventArgs.Empty);
						break;
				}
				
				if(e.KeyCode == key_tune_up_1)
				{
					double freq = Double.Parse(txtVFOAFreq.Text);
					freq += 1.0;
					VFOAFreq = freq;
				}
				else if(e.KeyCode == key_tune_down_1)
				{
					double freq = Double.Parse(txtVFOAFreq.Text);
					freq -= 1.0;
					VFOAFreq = freq;
				}
				else if(e.KeyCode == key_tune_up_2)
				{
					double freq = Double.Parse(txtVFOAFreq.Text);
					freq += 0.1;
					VFOAFreq = freq;
				}
				else if(e.KeyCode == key_tune_down_2)
				{
					double freq = Double.Parse(txtVFOAFreq.Text);
					freq -= 0.1;
					VFOAFreq = freq;
				}
				else if(e.KeyCode == key_tune_up_3)
				{
					double freq = Double.Parse(txtVFOAFreq.Text);
					freq += 0.01;
					VFOAFreq = freq;
				}
				else if(e.KeyCode == key_tune_down_3)
				{
					double freq = Double.Parse(txtVFOAFreq.Text);
					freq -= 0.01;
					VFOAFreq = freq;
				}
				else if(e.KeyCode == key_tune_up_4)
				{
					double freq = Double.Parse(txtVFOAFreq.Text);
					freq += 0.001;
					VFOAFreq = freq;
				}
				else if(e.KeyCode == key_tune_down_4)
				{
					double freq = Double.Parse(txtVFOAFreq.Text);
					freq -= 0.001;
					VFOAFreq = freq;
				}
				else if(e.KeyCode == key_tune_up_5)
				{
					double freq = Double.Parse(txtVFOAFreq.Text);
					freq += 0.0001;
					VFOAFreq = freq;
				}
				else if(e.KeyCode == key_tune_down_5)
				{
					double freq = Double.Parse(txtVFOAFreq.Text);
					freq -= 0.0001;
					VFOAFreq = freq;
				}
				else if(e.KeyCode == key_tune_up_6)
				{
					double freq = Double.Parse(txtVFOAFreq.Text);
					freq += 0.00001;
					VFOAFreq = freq;
				}
				else if(e.KeyCode == key_tune_down_6)
				{
					double freq = Double.Parse(txtVFOAFreq.Text);
					freq -= 0.00001;
					VFOAFreq = freq;
				}
				else if(e.KeyCode == key_tune_up_7)
				{
					double freq = Double.Parse(txtVFOAFreq.Text);
					freq += 0.000001;
					VFOAFreq = freq;
				}
				else if(e.KeyCode == key_tune_down_7)
				{
					double freq = Double.Parse(txtVFOAFreq.Text);
					freq -= 0.000001;
					VFOAFreq = freq;
				}
				else if(e.KeyCode == key_rit_up)
				{
					udRIT.Value += rit_step_rate;
				}
				else if(e.KeyCode == key_rit_down)
				{
					udRIT.Value -= rit_step_rate;
				}
				else if(e.KeyCode == key_xit_up)
				{
					udXIT.Value += xit_step_rate;
				}
				else if(e.KeyCode == key_xit_down)
				{
					udXIT.Value -= xit_step_rate;
				}
				else if(e.KeyCode == key_filter_up)
				{
					if(current_filter == Filter.NONE)
						return;
					if(current_filter == Filter.VAR2)
						CurrentFilter = Filter.F6000;
					else
						CurrentFilter++;
				}
				else if(e.KeyCode == key_filter_down)
				{
					if(current_filter == Filter.NONE)
						return;
					if(current_filter == Filter.F6000)
						CurrentFilter = Filter.VAR2;
					else
						CurrentFilter--;
				}
				else if(e.KeyCode == key_mode_up)
				{
					switch(current_dsp_mode)
					{
						case DSPMode.LSB:
							CurrentDSPMode = DSPMode.USB;
							break;
						case DSPMode.USB:
							CurrentDSPMode = DSPMode.DSB;
							break;
						case DSPMode.DSB:
							CurrentDSPMode = DSPMode.CWL;
							break;
						case DSPMode.CWL:
							CurrentDSPMode = DSPMode.CWU;
							break;
						case DSPMode.CWU:
							CurrentDSPMode = DSPMode.FMN;
							break;
						case DSPMode.FMN:
							CurrentDSPMode = DSPMode.AM;
							break;
						case DSPMode.AM:
							CurrentDSPMode = DSPMode.SAM;
							break;
						case DSPMode.SAM:
							CurrentDSPMode = DSPMode.DIGL;
							break;
						case DSPMode.DIGL:
							CurrentDSPMode = DSPMode.DIGU;
							break;
						case DSPMode.DIGU:
							CurrentDSPMode = DSPMode.SPEC;
							break;
						case DSPMode.SPEC:
							CurrentDSPMode = DSPMode.DRM;
							break;
						case DSPMode.DRM:
							CurrentDSPMode = DSPMode.LSB;
							break;
					}
				}
				else if(e.KeyCode == key_mode_down)
				{
					switch(current_dsp_mode)
					{
						case DSPMode.LSB:
							CurrentDSPMode = DSPMode.DRM;
							break;
						case DSPMode.USB:
							CurrentDSPMode = DSPMode.LSB;
							break;
						case DSPMode.DSB:
							CurrentDSPMode = DSPMode.USB;
							break;
						case DSPMode.CWL:
							CurrentDSPMode = DSPMode.DSB;
							break;
						case DSPMode.CWU:
							CurrentDSPMode = DSPMode.CWL;
							break;
						case DSPMode.FMN:
							CurrentDSPMode = DSPMode.CWU;
							break;
						case DSPMode.AM:
							CurrentDSPMode = DSPMode.FMN;
							break;
						case DSPMode.SAM:
							CurrentDSPMode = DSPMode.AM;
							break;
						case DSPMode.DIGL:
							CurrentDSPMode = DSPMode.SAM;
							break;
						case DSPMode.DIGU:
							CurrentDSPMode = DSPMode.DIGL;
							break;
						case DSPMode.SPEC:
							CurrentDSPMode = DSPMode.DIGU;
							break;
						case DSPMode.DRM:
							CurrentDSPMode = DSPMode.SPEC;
							break;
					}
				}
				else if(e.KeyCode == key_band_up && !vfo_lock)
				{
					switch(current_band)
					{
						case Band.B160M:
							if(band_160m_index == 2)
							{
								band_80m_index = 0;
								btnBand80_Click(this, EventArgs.Empty);
							}
							else
							{
								btnBand160_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B80M:
							if(band_80m_index == 2)
							{
								band_60m_index = 0;
								btnBand60_Click(this, EventArgs.Empty);
							}
							else
							{
								btnBand80_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B60M:
							if(band_60m_index == 4)
							{
								band_40m_index = 0;
								btnBand40_Click(this, EventArgs.Empty);
							}
							else
							{
								btnBand60_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B40M:
							if(band_40m_index == 2)
							{
								band_30m_index = 0;
								btnBand30_Click(this, EventArgs.Empty);
							}
							else
							{
								btnBand40_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B30M:
							if(band_30m_index == 2)
							{
								band_20m_index = 0;
								btnBand20_Click(this, EventArgs.Empty);
							}
							else
							{
								btnBand30_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B20M:
							if(band_20m_index == 2)
							{
								band_17m_index = 0;
								btnBand17_Click(this, EventArgs.Empty);
							}
							else
							{
								btnBand20_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B17M:
							if(band_17m_index == 2)
							{
								band_15m_index = 0;
								btnBand15_Click(this, EventArgs.Empty);
							}
							else
							{
								btnBand17_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B15M:
							if(band_15m_index == 2)
							{
								band_12m_index = 0;
								btnBand12_Click(this, EventArgs.Empty);
							}
							else
							{
								btnBand15_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B12M:
							if(band_12m_index == 2)
							{
								band_10m_index = 0;
								btnBand10_Click(this, EventArgs.Empty);
							}
							else
							{
								btnBand12_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B10M:
							if(band_10m_index == 2)
							{
								band_6m_index = 0;
								btnBand6_Click(this, EventArgs.Empty);
							}
							else
							{
								btnBand10_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B6M:
							if(band_6m_index == 2)
							{
								if(XVTRPresent)
								{
									band_2m_index = 0;
									btnBand2_Click(this, EventArgs.Empty);
								}
								else
								{
									band_wwv_index = 0;
									btnBandWWV_Click(this, EventArgs.Empty);
								}
							}
							else
							{
								btnBand6_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B2M:
							if(band_2m_index == 2)
							{
								band_wwv_index = 0;
								btnBandWWV_Click(this, EventArgs.Empty);
							}
							else
							{
								btnBand2_Click(this, EventArgs.Empty);
							}
							break;
						case Band.WWV:
							if(band_wwv_index == 4)
							{
								band_gen_index = 0;
								btnBandGEN_Click(this, EventArgs.Empty);
							}
							else
							{
								btnBandWWV_Click(this, EventArgs.Empty);
							}
							break;
						case Band.GEN:
							if(band_gen_index == 4)
							{
								band_160m_index = 0;
								btnBand160_Click(this, EventArgs.Empty);
							}
							else
							{
								btnBandGEN_Click(this, EventArgs.Empty);
							}
							break;
					}
				}
				else if(e.KeyCode == key_band_down && !vfo_lock)
				{
					switch(current_band)
					{
						case Band.B160M:
							if(band_160m_index == 0)
							{
								band_gen_index = 4;
								btnBandGEN_Click(this, EventArgs.Empty);
							}
							else
							{
								last_band = "160M";
								band_160m_index = (band_160m_index+1)%3;
								btnBand160_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B80M:
							if(band_80m_index == 0)
							{
								band_160m_index = 2;
								btnBand160_Click(this, EventArgs.Empty);
							}
							else
							{
								last_band = "80M";
								band_80m_index = (band_80m_index+1)%3;
								btnBand80_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B60M:
							if(band_60m_index == 0)
							{
								band_80m_index = 2;
								btnBand80_Click(this, EventArgs.Empty);
							}
							else
							{
								last_band = "60M";
								band_60m_index = (band_60m_index+3)%5;
								btnBand60_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B40M:
							if(band_40m_index == 0)
							{
								band_60m_index = 4;
								btnBand60_Click(this, EventArgs.Empty);
							}
							else
							{
								last_band = "40M";
								band_40m_index = (band_40m_index+1)%3;
								btnBand40_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B30M:
							if(band_30m_index == 0)
							{
								band_40m_index = 2;
								btnBand40_Click(this, EventArgs.Empty);
							}
							else
							{
								last_band = "30M";
								band_30m_index = (band_30m_index+1)%3;
								btnBand30_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B20M:
							if(band_20m_index == 0)
							{
								band_30m_index = 2;
								btnBand30_Click(this, EventArgs.Empty);
							}
							else
							{
								last_band = "20M";
								band_20m_index = (band_20m_index+1)%3;
								btnBand20_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B17M:
							if(band_17m_index == 0)
							{
								band_20m_index = 2;
								btnBand20_Click(this, EventArgs.Empty);
							}
							else
							{
								last_band = "17M";
								band_17m_index = (band_17m_index+1)%3;
								btnBand17_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B15M:
							if(band_15m_index == 0)
							{
								band_17m_index = 2;
								btnBand17_Click(this, EventArgs.Empty);
							}
							else
							{
								last_band = "15M";
								band_15m_index = (band_15m_index+1)%3;
								btnBand15_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B12M:
							if(band_12m_index == 0)
							{
								band_15m_index = 2;
								btnBand15_Click(this, EventArgs.Empty);
							}
							else
							{
								last_band = "12M";
								band_12m_index = (band_12m_index+1)%3;
								btnBand12_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B10M:
							if(band_10m_index == 0)
							{
								band_12m_index = 2;
								btnBand12_Click(this, EventArgs.Empty);
							}
							else
							{
								last_band = "10M";
								band_10m_index = (band_10m_index+1)%3;
								btnBand10_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B6M:
							if(band_6m_index == 0)
							{
								band_10m_index = 2;
								btnBand10_Click(this, EventArgs.Empty);
							}
							else
							{
								last_band = "6M";
								band_6m_index = (band_6m_index+1)%3;
								btnBand6_Click(this, EventArgs.Empty);
							}
							break;
						case Band.B2M:
							if(band_2m_index == 0)
							{
								band_6m_index = 2;
								btnBand6_Click(this, EventArgs.Empty);
							}
							else
							{
								last_band = "2M";
								band_2m_index = (band_2m_index+1)%3;
								btnBand6_Click(this, EventArgs.Empty);
							}
							break;
						case Band.WWV:
							if(band_wwv_index == 0)
							{
								if(xvtr_present)
								{
									band_2m_index = 2;
									btnBand2_Click(this, EventArgs.Empty);
								}
								else
								{
									band_6m_index = 2;
									btnBand6_Click(this, EventArgs.Empty);
								}
							}
							else
							{
								last_band = "WWV";
								band_wwv_index = (band_wwv_index+3)%5;
								btnBandWWV_Click(this, EventArgs.Empty);
							}
							break;
						case Band.GEN:
							if(band_gen_index == 0)
							{
								band_wwv_index = 4;
								btnBandWWV_Click(this, EventArgs.Empty);
							}
							else
							{
								last_band = "GEN";
								band_gen_index = (band_gen_index+3)%5;
								btnBandGEN_Click(this, EventArgs.Empty);
							}
							break;
					}

				}
				else if(e.KeyCode == key_cw_dot)
				{
					CWForm.KBDot = true;
				}
				else if(e.KeyCode == key_cw_dash)
				{
					CWForm.KBDash = true;
				}
				else if(vfo_lock || !quick_qsy)
				{
					return;
				}
				else if((int)e.KeyCode >= 48 && (int)e.KeyCode <= 57)
				{
					txtVFOAFreq.Focus();
					txtVFOAFreq.Text = ((int)(e.KeyCode - 48)).ToString();
					txtVFOAFreq.Select(1,0);
				}
				else if((int)e.KeyCode >= 96 && (int)e.KeyCode <= 105)
				{
					txtVFOAFreq.Focus();
					txtVFOAFreq.Text = ((int)e.KeyCode-96).ToString();
					txtVFOAFreq.Select(1,0);
				}
				else if(e.KeyCode.Equals(Keys.Decimal) ||
					(separator == "." && e.KeyCode.Equals(Keys.OemPeriod)) ||
					(separator == "," && e.KeyCode.Equals(Keys.Oemcomma)))
				{
					txtVFOAFreq.Focus();
					txtVFOAFreq.Text = separator;
					txtVFOAFreq.Select(1,0);
				}
			}
		}

		private void Console_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//			if(this.ActiveControl is TextBoxTS && this.ActiveControl != txtVFOAFreq
			//				&& this.ActiveControl != txtVFOBFreq) return;
			//			if(this.ActiveControl is NumericUpDownTS) return;
			if(this.ActiveControl is TextBoxTS ||
				this.ActiveControl is NumericUpDownTS ||
				this.ActiveControl is TrackBarTS)
			{
				Console_KeyPress(this, new KeyPressEventArgs((char)Keys.Enter));
				return;
			}

			int numberToMove = e.Delta / 120;	// 1 per click
			
			if(vfo_char_width == 0)
				GetVFOCharWidth();
			
			if (numberToMove != 0) 
			{
				int left, right, top, bottom;
				left = grpVFOA.Left+txtVFOAFreq.Left;
				right = left + txtVFOAFreq.Width;
				top = grpVFOA.Top+txtVFOAFreq.Top;
				bottom = top + txtVFOAFreq.Height;

				if(e.X > left && e.X < right &&			// Update VFOA
					e.Y > top && e.Y < bottom)
				{	
					double freq = double.Parse(txtVFOAFreq.Text);
					double mult = 1000.0;
					int x = right + 2 - (vfo_pixel_offset - 5);
					while(x < e.X && mult > 0.0000011)
					{
						mult /= 10.0;
						x += vfo_char_width;
						if(mult == 1.0)
							x += vfo_decimal_space;
						else x += vfo_char_space;
					}

					if(mult <= 1.0)
					{
						freq += mult*numberToMove;
						//Debug.WriteLine("freq: "+freq.ToString("f6"));
						VFOAFreq = freq;
					}
				}
				else
				{
					left = grpVFOB.Left+txtVFOBFreq.Left;
					right = left + txtVFOBFreq.Width;
					top = grpVFOB.Top+txtVFOBFreq.Top;
					bottom = top + txtVFOBFreq.Height;
					if(e.X > left && e.X < right &&		// Update VFOB
						e.Y > top && e.Y < bottom)
					{
						double freq = double.Parse(txtVFOBFreq.Text);
						double mult = 1000.0;
						int x = right + 2 - (vfo_pixel_offset - 5);
						while(x < e.X && mult > 0.0000011)
						{
							mult /= 10;
							x += vfo_char_width;
							if(mult == 1.0)
								x += vfo_decimal_space;
							else x += vfo_char_space;
						}

						if(mult <= 1.0)
						{
							freq += mult*numberToMove;
							VFOBFreq = freq;
						}
					}
					else
					{
						double freq = Double.Parse(txtVFOAFreq.Text);
						double mult = wheel_tune_list[wheel_tune_index];
						if(shift_down && mult >= 0.000009) mult /= 10;

						if(numberToMove < 0)
						{
							double temp = freq;
							numberToMove += 1;
							freq = Math.Floor(freq/mult);
							if ((double)Math.Round(freq*mult, 6) == temp) freq -= 1.0;
							freq = (freq+numberToMove)*mult;	
						}
						else
						{
							double temp = freq;
							if (numberToMove > 0) numberToMove -= 1;
							freq = Math.Floor((freq/mult) + 1.0);
							if ((double)Math.Round(freq*mult,6) == temp) freq += 1.0;
							freq = (freq+numberToMove)*mult;
						}

						VFOAFreq = freq;
					}
				}				
			}
		}

		// chkPower
		private void chkPower_CheckedChanged(object sender, System.EventArgs e)
		{
			DttSP.AudioReset();
			Audio.VACRBReset = true;
			if(chkPower.Checked)
			{
				chkPower.Text = "On";
				chkPower.BackColor = button_selected_color;
				txtVFOAFreq.ForeColor = vfo_text_light_color;
				txtVFOABand.ForeColor = band_text_light_color;
				Hdw.PowerOn();
				txtVFOAFreq_LostFocus(this, EventArgs.Empty);

				// wjt added 
				if ( PTTBitBangEnabled && serialPTT == null ) // we are enabled but don't have port object 
				{
					//Debug.WriteLine("Forcing property set on PTTBitBangEnabled"); 
					PTTBitBangEnabled = true; // force creation of serial ptt 
				}
				// wjt added ends 
				SetupForm.AudioReceiveMux1 = SetupForm.AudioReceiveMux1;		// set receive mux

				Audio.CurrentAudioState1 = Audio.AudioState.DTTSP;
				Audio.callback_return = 0;
				/*if(vac_enabled) 
				{
					if (second_sound_card_stereo) Audio.StartAudio(ref callbackVAC, (uint)block_size2, sample_rate2, audio_driver_index2,
													  audio_input_index2, audio_output_index2, 2, 1, audio_latency2);
					else Audio.StartAudio(ref callbackVAC, (uint)block_size2, sample_rate2, audio_driver_index2,
							 audio_input_index2, audio_output_index2, 1, 1, audio_latency2);
					Thread.Sleep(20);
				}

				if(num_channels == 4)
				{
					bool audio_running = Audio.StartAudio(ref callback4port, (uint)block_size1, sample_rate1, audio_driver_index1,
						audio_input_index1, audio_output_index1, 4, 0, audio_latency1);
					if(!audio_running)
					{
						chkPower.Checked = false;
						return;
					}
				}
				else // open for 2 channels
				{				
					bool audio_running = Audio.StartAudio(ref callback1, (uint)block_size1, sample_rate1, audio_driver_index1,
						audio_input_index1, audio_output_index1, 2, 0, audio_latency1);
					if(!audio_running)
					{
						chkPower.Checked = false;
						return;
					}
				}*/

				if(!Audio.Start())
				{
					chkPower.Checked = false;
					return;
				}

				draw_display_thread = new Thread(new ThreadStart(RunDisplay));
				draw_display_thread.Name = "Draw Display Thread";
				draw_display_thread.Priority = ThreadPriority.BelowNormal;
				draw_display_thread.IsBackground = true;
				draw_display_thread.Start();

				multimeter_thread = new Thread(new ThreadStart(UpdateMultimeter));
				multimeter_thread.Name = "Multimeter Thread";
				multimeter_thread.Priority = ThreadPriority.Lowest;
				multimeter_thread.IsBackground = true;
				multimeter_thread.Start();

				poll_ptt_thread = new Thread(new ThreadStart(PollPTT));
				poll_ptt_thread.Name = "Poll PTT Thread";
				poll_ptt_thread.Priority = ThreadPriority.Normal;
				poll_ptt_thread.IsBackground = true;
				poll_ptt_thread.Start();

				if(pa_present)
				{
					poll_pa_pwr_thread = new Thread(new ThreadStart(PollPAPWR));
					poll_pa_pwr_thread.Name = "Poll PA PWR Thread";
					poll_pa_pwr_thread.Priority = ThreadPriority.BelowNormal;
					poll_pa_pwr_thread.IsBackground = true;
					poll_pa_pwr_thread.Start();
				}

				if(!rx_only)
				{
					chkMOX.Enabled = true;
					chkTUN.Enabled = true;
				}
				chkVFOLock.Enabled = true;
				chkMemoryScanner.Enabled = true;

				timer_peak_text.Enabled = true;

				if(atu_present)
				{
					int counter = 0;
					while(((Hdw.StatusPort() & (byte)StatusPin.PA_DATA)) == 0)
					{
						Thread.Sleep(50);
						if(counter++ > 100)		// 5 second time out
						{
							MessageBox.Show("ATU Initialization Timeout.\n"+
								"Please check power to radio",
								"ATU Init Error",
								MessageBoxButtons.OK,
								MessageBoxIcon.Hand);
							chkPower.Checked = false;
							return;
						}
					}
					Hdw.PA_ATUTune(ATUTuneMode.BYPASS);
				}

				if (((DttSP.CurrentMode == DttSP.Mode.CWL) ||
					(DttSP.CurrentMode == DttSP.Mode.CWU)) &&
					new_keyer) 
				{
					DttSP.StartKeyer();
				}
			}
			else
			{
				Audio.callback_return = 2;
                chkPower.Text = "Standby";

				chkMOX.Checked = false;
				chkMOX.Enabled = false;
				chkTUN.Checked = false;
				chkTUN.Enabled = false;

				if (((DttSP.CurrentMode == DttSP.Mode.CWL) ||
					(DttSP.CurrentMode == DttSP.Mode.CWU)) && new_keyer) 
				{
					DttSP.StopKeyer();
				}

				if (!new_keyer)
				{
					if(CWForm.Send)
					{
						Audio.CurrentAudioState1 = Audio.AudioState.DTTSP;
						CWForm.Send = false;					
					}
				}

				// wjt added 
				if ( serialPTT != null )  // let go of serial port
				{ 
					serialPTT.Destroy(); 
					serialPTT = null; 
				}
				// wjt added ends 

				chkVFOLock.Enabled = false;
				if(MemoryScanner)
					MemoryScanner = false;
				chkMemoryScanner.Enabled = false;
				
				Hdw.StandBy();
				chkPower.BackColor = SystemColors.Control;
				txtVFOAFreq.ForeColor = vfo_text_dark_color;
				txtVFOABand.ForeColor = band_text_dark_color;

				timer_peak_text.Enabled = false;

				Display.ResetDisplayAverage();
				Display.ResetDisplayPeak();

//				multimeter_thread.Priority = ThreadPriority.Highest;
//				draw_display_thread.Priority = ThreadPriority.Highest;
//				poll_ptt_thread.Priority = ThreadPriority.Highest;
//
//				do
//				{
//					Thread.Sleep(50);
//				} while(display_running ||
//					multimeter_running ||
//					poll_ptt_running);

				Audio.StopAudio1();
				if(vac_enabled) Audio.StopAudioVAC();
			}

			panelVFOAHover.Invalidate();
			panelVFOBHover.Invalidate();
		}

		//txtVFOAFreq Events
		private void txtVFOAFreq_LostFocus(object sender, System.EventArgs e)
		{
			if(txtVFOAFreq.Text == "." || txtVFOAFreq.Text == "") 
			{
				VFOAFreq = saved_vfoa_freq;
				return;
			}
			
			double freq = double.Parse(txtVFOAFreq.Text);
			txtVFOAFreq.Text = freq.ToString("f6");

			current_xvtr_index = XVTRForm.XVTRFreq(freq);

			if(current_xvtr_index < 0)
			{
				int old_xvtr_index = XVTRForm.XVTRFreq(saved_vfoa_freq);
				if(old_xvtr_index >= 0 && freq >= max_freq)
				{
					VFOAFreq = saved_vfoa_freq;
					return;
				}
			}

			if(current_xvtr_index >= 0)
			{
				int power = XVTRForm.GetPower(current_xvtr_index);
				if(current_dsp_mode == DSPMode.FMN) power /= 4;
				udPWR.Value = power;
				udPWR_ValueChanged(this, EventArgs.Empty);
				SetupForm.RXOnly = XVTRForm.GetRXOnly(current_xvtr_index);
				xvtr_gain_offset = XVTRForm.GetRXGain(current_xvtr_index);
				Hdw.X2 = (byte)((Hdw.X2 & 0xF0) | XVTRForm.GetXVTRAddr(current_xvtr_index));
			}
			else
			{
				xvtr_gain_offset = 0.0f;
			}

			// update Band Info
			string bandInfo;
			bool transmit_allowed = DB.BandText(freq, out bandInfo);
			if(!transmit_allowed)
			{
				txtVFOABand.BackColor = out_of_band_color;
				if(!chkVFOSplit.Checked && chkMOX.Checked)
					chkMOX.Checked = false;
			}
			else txtVFOABand.BackColor = Color.Black;
			txtVFOABand.Text = bandInfo;
			SetCurrentBand((float)freq);
			SetBandButtonColor(current_band);

			if(ext_ctrl_enabled)
				UpdateExtCtrl();

			if(CurrentBand == Band.B60M)
			{
				chkXIT.Enabled = false;
				chkXIT.Checked = false;
			}
			else
				chkXIT.Enabled = true;

			saved_vfoa_freq = (float)freq;

			if(current_xvtr_index >= 0)
				freq = XVTRForm.TranslateFreq(freq);
			else
			{
				if(freq < min_freq)
				{
					freq = min_freq;
					txtVFOAFreq.Text = freq.ToString("f6");
				}
				else if(freq > max_freq)
				{
					freq = max_freq;
					txtVFOAFreq.Text = freq.ToString("f6");
				}
			}

			if(chkMOX.Checked &&
				(CurrentDSPMode == DSPMode.AM ||
				CurrentDSPMode == DSPMode.SAM ||
				CurrentDSPMode == DSPMode.FMN))
				freq -= 0.011025;

			if(current_dsp_mode == DSPMode.CWL)
				freq += (double)CWForm.CWPitch * 0.0000010;
			else if(current_dsp_mode == DSPMode.CWU)
				freq -= (double)CWForm.CWPitch * 0.0000010;

			if(freq < min_freq) freq = min_freq;
			else if(freq > max_freq) freq = max_freq;

			if(chkRIT.Checked && !chkMOX.Checked && !chkVFOSplit.Checked)
				freq += (int)udRIT.Value * 0.000001;
			else if(chkXIT.Checked && chkMOX.Checked && !chkVFOSplit.Checked)
				freq += (int)udXIT.Value * 0.000001;

			if(freq < min_freq) freq = min_freq;
			else if(freq > max_freq) freq = max_freq;
			
			if(chkPower.Checked)
			{
				switch(current_model)
				{
					case Model.SDR1000:
					case Model.DEMO:
						if(Audio.wave_playback)
						{
							double f = (DttSP.SampleRate - 11025.0 + wave_freq - freq*1000000)%DttSP.SampleRate;
							if(f > DttSP.SampleRate/2) f -= DttSP.SampleRate;
							if(f < -DttSP.SampleRate/2) f += DttSP.SampleRate;
							DttSP.SetOsc(f);
						}
						else
						{
							if(chkMOX.Checked && !chkVFOSplit.Checked)
							{
								if(!IsHamBand(current_band_plan, freq))
								{
									MessageBox.Show("The frequency "+freq.ToString("f6")+"MHz is not within the "+
										"IARU Band specifications.",
										"Transmit Error: Out Of Band",
										MessageBoxButtons.OK,
										MessageBoxIcon.Error);
									chkMOX.Checked = false;
									return;
								}
							}
							DDSFreq = freq;
						}
						break;
					case Model.SOFTROCK40:
						//!!!!drm patch
						double osc_freq = soft_rock_center_freq*1e6 - freq*1e6;
						if ( current_dsp_mode  == DSPMode.DRM ) // if we're in DRM mode we need to be offset 12khz
						{
							osc_freq = osc_freq + 12000; 
							// System.Console.WriteLine("setting osc_freq: " + osc_freq); 
						}
						tuned_freq = freq;
						//Debug.WriteLine("osc_freq: "+osc_freq.ToString("f6"));
						DttSP.SetOsc(osc_freq);
						break;
				}
			}

			if(Display.PeakOn) Display.ResetDisplayPeak();
		}

		private static double tuned_freq;

		private void txtVFOAFreq_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			int KeyCode = (int)e.KeyChar;  
			if((KeyCode < 48 || KeyCode > 57) &&			// numeric keys
				KeyCode != 8 &&								// backspace
				!e.KeyChar.ToString().Equals(separator) &&	// decimal
				KeyCode != 27)								// escape
			{
				e.Handled=true;
			}
			else
			{
				if(e.KeyChar.ToString().Equals(separator))
				{
					e.Handled = (((TextBoxTS)sender).Text.IndexOf(separator) >= 0);
				}
				else if(KeyCode == 27)
				{
					VFOAFreq = saved_vfoa_freq;
					btnHidden.Focus();
				}
			}
			if(e.KeyChar == (char)Keys.Enter)
			{
				txtVFOAFreq_LostFocus(txtVFOAFreq, new System.EventArgs());
				btnHidden.Focus();
			}
		}

		private void txtVFOAFreq_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(this.ContainsFocus)
			{
				int old_digit = vfoa_hover_digit;
				int digit_index = 0;
				if(vfo_char_width == 0)
					GetVFOCharWidth();

				int x = txtVFOAFreq.Width - (vfo_pixel_offset - 5);
				while(x < e.X)
				{
					digit_index++;
					x += vfo_char_width;
					if(digit_index == 3)
						x += vfo_decimal_space;
					else
						x += vfo_char_space;
				}

				if(digit_index < 3) digit_index = -1;
				vfoa_hover_digit = digit_index;
				if(vfoa_hover_digit != old_digit &&
					txtVFOAFreq.Enabled)
					panelVFOAHover.Invalidate();
			}
		}

		private void txtVFOAFreq_MouseLeave(object sender, System.EventArgs e)
		{
			vfoa_hover_digit = -1;
			if(txtVFOAFreq.Enabled)
				panelVFOAHover.Invalidate();
		}

		// txtVFOBFreq
		private void txtVFOBFreq_LostFocus(object sender, System.EventArgs e)
		{
			if(txtVFOBFreq.Text == "" || txtVFOBFreq.Text == ".")
			{
				VFOBFreq = saved_vfob_freq;
				return;
			}

			double freq = double.Parse(txtVFOBFreq.Text);
			txtVFOBFreq.Text = freq.ToString("f6"); 

			int xvtr_index = XVTRForm.XVTRFreq(freq);

			if(xvtr_index < 0)
			{
				int old_xvtr_index = XVTRForm.XVTRFreq(saved_vfob_freq);
				if(old_xvtr_index >= 0 && freq >= max_freq)
				{
					VFOBFreq = saved_vfob_freq;
					return;
				}
			}

			// update Band Info
			string bandInfo;
			bool transmit = DB.BandText(freq, out bandInfo);
			if(transmit == false)
			{
				txtVFOBBand.BackColor = Color.DimGray;
				if(chkVFOSplit.Checked && chkMOX.Checked)
					chkMOX.Checked = false;
			}
			else txtVFOBBand.BackColor = Color.Black;
			txtVFOBBand.Text = bandInfo;

			saved_vfob_freq = (float)freq;
			
			if(chkPower.Checked && chkMOX.Checked && chkVFOSplit.Checked)
			{
				SetCurrentBand((float)freq);
				SetBandButtonColor(current_band);

				if(xvtr_index >= 0)
					freq = XVTRForm.TranslateFreq(freq);

				if(ext_ctrl_enabled)
					UpdateExtCtrl();

				if(CurrentBand == Band.B60M)
				{
					chkXIT.Enabled = false;
					chkXIT.Checked = false;
				}
				else
					chkXIT.Enabled = true;

				if(chkMOX.Checked &&
					(CurrentDSPMode == DSPMode.AM ||
					CurrentDSPMode == DSPMode.SAM ||
					CurrentDSPMode == DSPMode.FMN))
					freq -= 0.011025;

				if(current_dsp_mode == DSPMode.CWL)
					freq += (double)CWForm.CWPitch * 0.0000010;
				else if(current_dsp_mode == DSPMode.CWU)
					freq -= (double)CWForm.CWPitch * 0.0000010;
                			
				if(chkXIT.Checked)
					freq += (int)udXIT.Value * 0.0001;
					
				if(freq < min_freq) freq = min_freq;
				else if(freq > max_freq) freq = max_freq;
			
				if(!IsHamBand(current_band_plan, freq))	// out of band
				{
					MessageBox.Show("The frequency "+freq.ToString("f6")+"MHz is not within the "+
						"IARU Band specifications.",
						"Transmit Error: Out Of Band",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
					chkMOX.Checked = false;
					return;
				}
				Debug.WriteLine("freq: "+freq.ToString("f6"));
				DDSFreq = freq;
			}
		}

		private void txtVFOBFreq_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			int KeyCode = (int)e.KeyChar;  
			if((KeyCode < 48 || KeyCode > 57) &&			// numeric keys
				KeyCode != 8 &&								// backspace
				!e.KeyChar.ToString().Equals(separator) &&	// decimal
				KeyCode != 27)								// escape
			{
				e.Handled=true;
			}
			else
			{
				if(e.KeyChar.ToString().Equals(separator))
				{
					e.Handled = (((TextBoxTS)sender).Text.IndexOf(separator) >= 0);
				}
				else if(KeyCode == 27)
				{
					VFOBFreq = saved_vfob_freq;
					btnHidden.Focus();
				}
			}
			if(e.KeyChar == (char)Keys.Enter)
			{
				txtVFOAFreq_LostFocus(txtVFOAFreq, new System.EventArgs());
				btnHidden.Focus();
			}
		}

		private void txtVFOBFreq_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(this.ContainsFocus)
			{
				int old_digit = vfob_hover_digit;
				int digit_index = 0;
				if(vfo_char_width == 0)
					GetVFOCharWidth();

				int x = txtVFOBFreq.Width - (vfo_pixel_offset - 5);
				while(x < e.X)
				{
					digit_index++;
					x += vfo_char_width;
					if(digit_index == 3)
						x += vfo_decimal_width;
					else
						x += vfo_char_space;
				}

				if(digit_index < 3) digit_index = -1;
				vfob_hover_digit = digit_index;
				if(vfob_hover_digit != old_digit &&
					txtVFOBFreq.Enabled)
					panelVFOBHover.Invalidate();
			}
		}

		private void txtVFOBFreq_MouseLeave(object sender, System.EventArgs e)
		{
			vfob_hover_digit = -1;
			if(txtVFOBFreq.Enabled)
				panelVFOBHover.Invalidate();
		}

		private void panelVFOAHover_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			txtVFOAFreq_MouseMove(sender, new MouseEventArgs(MouseButtons.None, 0,
				e.X+panelVFOAHover.Left-10, e.Y+panelVFOAHover.Top, 0));
		}

		private void panelVFOBHover_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			txtVFOBFreq_MouseMove(sender, new MouseEventArgs(MouseButtons.None, 0,
				e.X+panelVFOBHover.Left-10, e.Y+panelVFOBHover.Top, 0));
		}

		private void picDisplay_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				switch(Display.CurrentDisplayMode)
				{
					case DisplayMode.HISTOGRAM:
					case DisplayMode.PANADAPTER:
					case DisplayMode.SPECTRUM:
					case DisplayMode.WATERFALL:
						float x, y;
						display_cursor_x = e.X;
						display_cursor_y = e.Y;
						PixelToSpectrum(e.X, e.Y, out x, out y);
						double freq = double.Parse(txtVFOAFreq.Text) + (double)x*0.0000010;

						if(current_dsp_mode == DSPMode.CWL)
							freq += (double)cw_pitch*0.0000010;
						else if(current_dsp_mode == DSPMode.CWU)
							freq -= (double)cw_pitch*0.0000010;
						txtDisplayCurPos.Text = x.ToString("f1") + "Hz, " + y.ToString("f1") + "dBm, " + freq.ToString("f6") + "MHz";
						break;
					default:
						txtDisplayCurPos.Text = "";
						break;
				}
			}
			catch(Exception)
			{

			}
		}

		private void picDisplay_MouseLeave(object sender, System.EventArgs e)
		{
			txtDisplayCurPos.Text = "";
			display_cursor_x = -1;
			display_cursor_y = -1;
		}

		private void picDisplay_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
			{
				if(long_crosshair)
				{
					switch(Display.CurrentDisplayMode)
					{
						case DisplayMode.SPECTRUM:
						case DisplayMode.WATERFALL:
						case DisplayMode.HISTOGRAM:
						case DisplayMode.PANADAPTER:
							float x,y;
							PixelToSpectrum(e.X, e.Y, out x, out y);
							double freq = double.Parse(txtVFOAFreq.Text) + (double)x*0.0000010;
							switch(current_dsp_mode)
							{
								case DSPMode.CWL:
									freq += (float)cw_pitch*0.0000010;
									break;
								case DSPMode.CWU:
									freq -= (float)cw_pitch*0.0000010;
									break;
								// SN 4/06 setup RTTY FSK
								case DSPMode.DIGL:
									if(rtty_click_tuning) 
									{	// SN 4/06 Added Digital Vars for RTTY
										if(bFSK) freq += (float)2210*0.0000010;	
										else freq += (float)2040*0.0000010;
									}
									break;
								case DSPMode.DIGU:
									if(sstv_click_tuning) freq -= (float)1200*0.0000010;
									break;
							}
							VFOAFreq = Math.Round(freq, 6);;
							break;
						default:
							break;
					}
				}
			}
			else if(e.Button == MouseButtons.Right)
			{
				if(long_crosshair)
					long_crosshair = false;
				else long_crosshair = true;
			}
			else if(e.Button == MouseButtons.Middle)
				ChangeWheelTuneLeft();
		}

		public void comboDisplayMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{			
			if(Display.CurrentDisplayMode == DisplayMode.OFF)
			{
				draw_display_thread = new Thread(new ThreadStart(RunDisplay));
				draw_display_thread.Name = "Draw Display Thread";
				draw_display_thread.Priority = ThreadPriority.BelowNormal;
				draw_display_thread.Start();
			}
			
			switch(comboDisplayMode.Text)
			{
				case "Spectrum":
					Display.CurrentDisplayMode = DisplayMode.SPECTRUM;
					break;
				case "Panadapter":
					Display.CurrentDisplayMode = DisplayMode.PANADAPTER;
					break;
				case "Scope":
					Display.CurrentDisplayMode = DisplayMode.SCOPE;
					break;
				case "Phase":
					Display.CurrentDisplayMode = DisplayMode.PHASE;
					break;
				case "Phase2":
					Display.CurrentDisplayMode = DisplayMode.PHASE2;
					break;
				case "Waterfall":
					Display.CurrentDisplayMode = DisplayMode.WATERFALL;
					break;
				case "Histogram":
					Display.CurrentDisplayMode = DisplayMode.HISTOGRAM;
					break;
				case "Off":
					Display.CurrentDisplayMode = DisplayMode.OFF;
					break;
			}

			switch(Display.CurrentDisplayMode)
			{
				case DisplayMode.PHASE:
				case DisplayMode.PHASE2:
				case DisplayMode.SCOPE:
				case DisplayMode.OFF:
					chkDisplayAVG.Enabled = false;
					chkDisplayAVG.Checked = false;
					chkDisplayPeak.Enabled = false;
					chkDisplayPeak.Checked = false;
					break;
				default:
					chkDisplayAVG.Enabled = true;
					chkDisplayPeak.Enabled = true;
					break;
			}

			if(chkDisplayAVG.Checked)
			{
				switch(Display.CurrentDisplayMode)
				{
					case DisplayMode.PANADAPTER:
					case DisplayMode.HISTOGRAM:
					case DisplayMode.SPECTRUM:
					case DisplayMode.WATERFALL:
						btnZeroBeat.Enabled = true;
						break;
					default:
						btnZeroBeat.Enabled = false;
						break;
				}
			}

			was_panadapter = false;

			if(comboDisplayMode.Focused)
				btnHidden.Focus();
		}

		private void btnMemoryRecall_Click(object sender, System.EventArgs e)
		{
			if(MemForm == null || MemForm.IsDisposed)
				MemForm = new Memory(this);
			MemForm.Show();
			MemForm.Focus();
		}

		private void chkBIN_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkBIN.Checked)
			{
				chkBIN.BackColor = button_selected_color;
				DttSP.SetBIN(1);
			}
			else
			{
				chkBIN.BackColor = SystemColors.Control;
				DttSP.SetBIN(0);
			}
		}	

		private void comboAGC_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(comboAGC.SelectedIndex < 0) return;
			DttSP.SetRXAGC((AGCMode)comboAGC.SelectedIndex);
			
			if((AGCMode)comboAGC.SelectedIndex == AGCMode.CUSTOM)
				SetupForm.CustomRXAGCEnabled = true;
			else SetupForm.CustomRXAGCEnabled = false;

			if(comboAGC.Focused)
				btnHidden.Focus();
		}

		private void Console_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Audio.callback_return = 2;
			if(SetupForm != null) SetupForm.Hide();
			if(CWForm != null) CWForm.Hide();
			if(CWXForm != null) CWXForm.Hide();
			if(EQForm != null) EQForm.Hide();
			if(UCBForm != null) UCBForm.Hide();
			if(XVTRForm != null) XVTRForm.Hide();
			if(ProdTestForm != null) ProdTestForm.Hide();

			chkPower.Checked = false;
			Thread.Sleep(100);
			this.Hide();
			btnFilterShiftReset_Click(this, EventArgs.Empty);
			SaveState();

			if(CWForm != null) CWForm.SaveCWOptions();
			if(CWXForm != null) CWXForm.SaveSettings();
			if(SetupForm != null) SetupForm.SaveOptions();
			if(EQForm != null) EQForm.SaveSettings();
			if(UCBForm != null) UCBForm.SaveSettings();
			if(XVTRForm != null) XVTRForm.SaveSettings();
			if(ProdTestForm != null) ProdTestForm.SaveSettings();
		}

		private void comboPreamp_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch(comboPreamp.Text)
			{
				case "Off":
					CurrentPreampMode = PreampMode.OFF;
					break;
				case "Low":
					CurrentPreampMode = PreampMode.LOW;
					break;
				case "Med":
					CurrentPreampMode = PreampMode.MED;
					break;
				case "High":
					CurrentPreampMode = PreampMode.HIGH;
					break;
			}

			if(comboPreamp.Focused)
				btnHidden.Focus();
		}

		private void chkMUT_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkMUT.Checked)
				chkMUT.BackColor = button_selected_color;
			else
				chkMUT.BackColor = SystemColors.Control;

			if(num_channels == 2)
			{
				Hdw.MuteRelay = chkMUT.Checked;
			}
		
			if(chkMUT.Checked)
				Audio.MonitorVolume = 0.0;
			else
				udAF_ValueChanged(this, EventArgs.Empty);
		}

		private void udPWR_ValueChanged(object sender, System.EventArgs e)
		{
			if(SetupForm == null)
				return;

			float val = (float)udPWR.Value;

			if(udPWR.Value > udPWR.Maximum)
			{
				udPWR.Value = udPWR.Maximum;
				return;
			}

			if(udPWR.Value < udPWR.Minimum)
			{
				udPWR.Value = udPWR.Minimum;
				return;
			}

			/*if(current_xvtr_index >= 0)
			{
				int power = (int)udPWR.Value;
				if(current_dsp_mode == DSPMode.FMN) power *= 4;
				XVTRForm.SetPower(current_xvtr_index, power);
			}*/

			if(pa_present && VFOAFreq < 29.7f)
			{
				if(val == 0)
				{
					Audio.RadioVolume = 0;
					return;
				}

				double target_dbm = 10*(double)Math.Log10((double)val*1000);
				target_dbm -= GainByBand(CurrentBand);

				double target_volts = Math.Sqrt(Math.Pow(10, target_dbm*0.1)*0.05);		// E = Sqrt(P * R) 
				Audio.RadioVolume = target_volts/audio_volts1;
			}
			else
			{
				const double TARGET = 0.8;		// audio in volts needed to hit 1W 
				Audio.RadioVolume = (double)Math.Sqrt((double)udPWR.Value/100.0)/audio_volts1*TARGET;
			}
			if(udPWR.Focused)
				btnHidden.Focus();
		}

		private void udAF_ValueChanged(object sender, System.EventArgs e)
		{
			if(SetupForm == null)
				return;

			if(udAF.Value > udAF.Maximum)
				udAF.Value = udAF.Maximum;
			if(udAF.Value < udAF.Minimum)
				udAF.Value = udAF.Minimum;

			if(chkMUT.Checked)
			{
				Audio.MonitorVolume = 0.0;
				goto end;
			}				

			if((num_channels == 4) &&
				chkMOX.Checked && !chkMON.Checked)
			{
				// monitor is muted
				Audio.MonitorVolume = 0.0;
			}
			else
			{
				Audio.MonitorVolume = (double)udAF.Value/100.0;
			}

			end:
			if(!MOX) RXAF = (int)udAF.Value;
			else TXAF = (int)udAF.Value;
		
			if(udAF.Focused)
				btnHidden.Focus();
		}

		private void udMIC_ValueChanged(object sender, System.EventArgs e)
		{
			Audio.MicPreamp = Math.Pow(10.0, (0.7 * (double)udMIC.Value)/20.0);

			if(udMIC.Focused)
				btnHidden.Focus();
		}

		private void udSquelch_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetSquelchVal(-(float)udSquelch.Value -
				preamp_offset[(int)current_preamp_mode] -
				multimeter_cal_offset -
				filter_size_cal_offset);
			if(udSquelch.Focused)
				btnHidden.Focus();
		}

		private void WheelTune_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Middle)
				ChangeWheelTuneLeft();
		}

		private void chkPipe_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkPipe.Checked)
				Audio.CurrentAudioState1 = Audio.AudioState.PIPE;
			else
				Audio.CurrentAudioState1 = Audio.AudioState.DTTSP;
		}

		private void chkMON_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkMON.Checked)
				chkMON.BackColor = button_selected_color;
			else
				chkMON.BackColor = SystemColors.Control;

			if(num_channels == 4)
			{
				if(!(chkMON.Checked == false && chkMOX.Checked))
					udAF_ValueChanged(this, EventArgs.Empty);
				else
					Audio.MonitorVolume = 0.0;
			}
			else
			{
				if(chkPower.Checked && chkMOX.Checked)
					Hdw.MuteRelay = !chkMON.Checked;
			}
		}

		private void chkMOX_CheckedChanged(object sender, System.EventArgs e)
		{
			Audio.MOX = chkMOX.Checked;
			DttSP.AudioReset();
			Audio.VACRBReset = true;
			const int NUM_SWITCH_BUFFERS = 2;
			bool cw = (current_dsp_mode == DSPMode.CWL) ||
					  (current_dsp_mode == DSPMode.CWU);

			if (cw && new_keyer)
			{
				cw_key_mode = true;
			}

			if(rx_only)
			{
				chkMOX.Checked = false;
				return;
			}

			double tx_vol = Audio.RadioVolume;
			double rx_vol = Audio.MonitorVolume;

			meter_peak_count = multimeter_peak_hold_samples;		// reset multimeter peak

			if(chkMOX.Checked)
			{
				double freq = 0;
				if(chkVFOSplit.Checked)
					freq = double.Parse(txtVFOBFreq.Text);
				else
					freq = double.Parse(txtVFOAFreq.Text);

				if(current_xvtr_index >= 0)
					freq = XVTRForm.TranslateFreq(freq);

				switch(current_dsp_mode)
				{
					case DSPMode.CWL:
						freq += (double)cw_pitch * 0.0000010;
						break;
					case DSPMode.CWU:
						freq -= (double)cw_pitch * 0.0000010;
						break;
				}

				if(chkXIT.Checked)
					freq += (int)udXIT.Value * 0.000001;

				if(!calibrating)
				{
					if(!IsHamBand(current_band_plan, freq))	// out of band
					{
						MessageBox.Show("The frequency "+freq.ToString("f6")+"MHz is not within the "+
							"IARU Band specifications.",
							"Transmit Error: Out Of Band",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error);
						chkMOX.Checked = false;
						return;
					}

					if(btnBand60.BackColor == button_selected_color &&
						current_dsp_mode != DSPMode.USB)
					{
						MessageBox.Show(DttSP.CurrentMode.ToString()+" mode is not allowed on 60M band.",
							"Transmit Error: Mode/Band",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error);
						chkMOX.Checked = false;
						return;
					}
				}

				Audio.RadioVolume = 0;
				Audio.MonitorVolume = 0;

				if(num_channels == 2)
				{
					Mixer.SetMainMute(mixer_id1, true);
				}
				Hdw.UpdateHardware = false;

				if(!cw_key_mode)
				{
					if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP ||
						Audio.CurrentAudioState1 == Audio.AudioState.SWITCH) 
					{
						Audio.SwitchCount = 2048/BlockSize1*NUM_SWITCH_BUFFERS;
						Audio.NextAudioState1 = Audio.AudioState.DTTSP;
						Audio.CurrentAudioState1 = Audio.AudioState.SWITCH;
						Thread.Sleep(43);
						DttSP.SetTRX(DttSP.TransmitState.ON);
					}
					
					switch(Display.CurrentDisplayMode)
					{
						case DisplayMode.PANADAPTER:
						case DisplayMode.SPECTRUM:
						case DisplayMode.HISTOGRAM:
						case DisplayMode.WATERFALL:
							Display.DrawBackground();
							break;
					}

					Mixer.SetMux(mixer_id1, mixer_tx_mux_id1);
				}

				comboMeterRXMode.ForeColor = Color.Gray;
				comboMeterTXMode.ForeColor = Color.Black;
				comboMeterTXMode_SelectedIndexChanged(this, EventArgs.Empty);
				
				if(current_soundcard == SoundCard.AUDIGY_2 ||
					current_soundcard == SoundCard.AUDIGY_2_ZS)
					Mixer.SetLineInMute(mixer_id1, true);

				if(current_dsp_mode == DSPMode.AM ||
					current_dsp_mode == DSPMode.SAM ||
					current_dsp_mode == DSPMode.FMN)
				{
					freq -= 0.011025;
				}

				chkMOX.BackColor = button_selected_color;

				SetupForm.SpurRedEnabled = false;
				spur_reduction = false;
				if_shift = false;

				DDSFreq = freq;
				if(num_channels == 2) Hdw.MuteRelay = !chkMON.Checked;

				if(ext_ctrl_enabled)
				{
					Hdw.UpdateHardware = true;
					UpdateExtCtrl();
					Hdw.UpdateHardware = false;
				}

				if(x2_enabled)
				{
					Hdw.UpdateHardware = true;
					Hdw.X2 = (byte)(Hdw.X2 | 0x40);						
					Hdw.UpdateHardware = false;
					Thread.Sleep(x2_delay);
				}

				if(rfe_present)
				{					
					Hdw.GainRelay = true;		// 0dB
					Hdw.Attn = false;
					
					if(xvtr_present && freq >= 144.0)
					{
						Hdw.XVTR_RF = true;
						if(current_xvtr_tr_mode == XVTRTRMode.POSITIVE)
							Hdw.XVTR_TR = true;
						else if(current_xvtr_tr_mode == XVTRTRMode.NEGATIVE)
							Hdw.XVTR_TR = false;
					}
					else
					{
						if(current_xvtr_index < 0 || !XVTRForm.GetXVTRRF(current_xvtr_index))
						{
							Hdw.RFE_TR = true;
							if(pa_present)
							{
								Hdw.PA_TR_Relay = true;
								Hdw.PABias = true;
							}
						}
						else
						{
							Hdw.XVTR_RF = true;
						}
					}
					
				}
				else
					Hdw.GainRelay = false;		// 26dB
									
				Hdw.TransmitRelay = true;
				Hdw.UpdateHardware = true;				
						
				DisableAllBands();
				DisableAllModes();
				chkVFOSplit.Enabled = false;
				btnVFOAtoB.Enabled = false;
				btnVFOBtoA.Enabled = false;
				btnVFOSwap.Enabled = false;
				chkMemoryScanner.Enabled = false;
				chkPower.BackColor = Color.Red;
				
				if(cw_key_mode)
				{
					if(!chkTUN.Checked)
					{
						Audio.SwitchCount  = 2048/BlockSize1*NUM_SWITCH_BUFFERS;
						Audio.NextAudioState1 = Audio.AudioState.SWITCH;
						Audio.CurrentAudioState1 = Audio.AudioState.CW;
					}
				}

				if(num_channels == 2)
				{
					Mixer.SetMainMute(mixer_id1, false);
				}

				udPWR_ValueChanged(this, EventArgs.Empty);
				
				bool af_changed = ((int)udAF.Value != txaf);
				udAF.Value = txaf;
				if(!af_changed) udAF_ValueChanged(this, EventArgs.Empty);								
			}
			else
			{    // Going from TX to RX
				Audio.RadioVolume = 0;
				Audio.MonitorVolume = 0;
				Hdw.UpdateHardware = false;
				current_ptt_mode = PTTMode.NONE;

				if(num_channels == 2)
				{
					Mixer.SetMainMute(mixer_id1, true);
				}
				
				if(CWForm.Send)
					CWForm.Send = false;

				if(rfe_present)
				{
					if(xvtr_present && Hdw.XVTR_RF)
					{
						Hdw.XVTR_RF = false;
						if(current_xvtr_tr_mode == XVTRTRMode.POSITIVE)
							Hdw.XVTR_TR = false;
						else if(current_xvtr_tr_mode == XVTRTRMode.NEGATIVE)
							Hdw.XVTR_TR = true;
					}
					else
					{	
						if(current_xvtr_index < 0 || !XVTRForm.GetXVTRRF(current_xvtr_index))
						{
							Hdw.RFE_TR = false;
							if(pa_present)
							{
								Hdw.PABias = false;
								Hdw.PA_TR_Relay = false;
							}
						}
						else
						{
							Hdw.XVTR_RF = false;
						}
					}
				}
				Hdw.TransmitRelay = false;

				if(x2_enabled)
				{
					Thread.Sleep(x2_delay);
					Hdw.X2 = (byte)(Hdw.X2 & 0xBF);
				}

				if(cw_key_mode)
				{
					Audio.SwitchCount = 2048/BlockSize1*NUM_SWITCH_BUFFERS;
					Audio.NextAudioState1 = Audio.AudioState.DTTSP;
					Audio.CurrentAudioState1 = Audio.AudioState.SWITCH;
					Thread.Sleep(43);
					DttSP.SetTRX(DttSP.TransmitState.OFF);
				}
				else
				{
					if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP ||
						Audio.CurrentAudioState1 == Audio.AudioState.SWITCH)
					{
						Audio.SwitchCount = 2048/BlockSize1*NUM_SWITCH_BUFFERS;
						Audio.NextAudioState1 = Audio.AudioState.DTTSP;
						Audio.CurrentAudioState1 = Audio.AudioState.SWITCH;
						Thread.Sleep(43);
						DttSP.SetTRX(DttSP.TransmitState.OFF);
					}
					
					switch(Display.CurrentDisplayMode)
					{
						case DisplayMode.PANADAPTER:
						case DisplayMode.SPECTRUM:
						case DisplayMode.HISTOGRAM:
						case DisplayMode.WATERFALL:
							Display.DrawBackground();
							break;
					}
				}

				comboMeterTXMode.ForeColor = Color.Gray;
				comboMeterRXMode.ForeColor = Color.Black;
				comboMeterRXMode_SelectedIndexChanged(this, EventArgs.Empty);

				chkMOX.BackColor = SystemColors.Control;
				spur_reduction = SetupForm.chkGeneralSpurRed.Checked;
				if(!spur_reduction &&
					(current_dsp_mode == DSPMode.AM ||
					current_dsp_mode == DSPMode.SAM ||
					current_dsp_mode == DSPMode.FMN))
					DttSP.SetOsc(-11025.0);
				
				SetupForm.SpurRedEnabled = true;
				if(current_dsp_mode != DSPMode.DRM &&
					current_dsp_mode != DSPMode.SPEC)
					if_shift = true;

				txtVFOAFreq_LostFocus(this, EventArgs.Empty);
				
				Mixer.SetMux(mixer_id1, mixer_rx_mux_id1);
				
				EnableAllBands();
				EnableAllModes();

				chkVFOSplit.Enabled = true;
				btnVFOAtoB.Enabled = true;
				btnVFOBtoA.Enabled = true;
				btnVFOSwap.Enabled = true;
				chkMemoryScanner.Enabled = true;
				chkPower.BackColor = button_selected_color;
				if(!chkMUT.Checked && num_channels == 2)
					Hdw.MuteRelay = false;

				Hdw.UpdateHardware = true;

				if(current_soundcard == SoundCard.AUDIGY_2 ||
					current_soundcard == SoundCard.AUDIGY_2_ZS)
					Mixer.SetLineInMute(mixer_id1, false);

				CurrentPreampMode = current_preamp_mode;

				if(num_channels == 2)
				{
					Mixer.SetMainMute(mixer_id1, false);
				}

				udPWR_ValueChanged(this, EventArgs.Empty);

				bool af_changed = ((int)udAF.Value != rxaf);
				udAF.Value = rxaf;
				if(!af_changed) udAF_ValueChanged(this, EventArgs.Empty);

				pa_fwd_power = 0;
				pa_rev_power = 0;

				Audio.HighSWRScale = 1.0;
				HighSWR = false;

				for(int i=0; i<meter_text_history.Length; i++)
					meter_text_history[i] = 0.0f;
			}	

			SetupForm.MOX = chkMOX.Checked;
			//Display.ResetDisplayAverage();
			ResetMultiMeterPeak();

			Thread t = new Thread(new ThreadStart(DelayedDisplayReset));
			t.Name = "Display Reset";
			t.Priority = ThreadPriority.BelowNormal;
			t.IsBackground = true;
			t.Start();
		}

		private void chkMOX_Click(object sender, System.EventArgs e)
		{
			if(chkMOX.Checked)			// because the CheckedChanged event fires first
				manual_mox = true;
			else
			{
				manual_mox = false;
				if(chkTUN.Checked)
					chkTUN.Checked = false;
			}
		}

		private void comboMeterRXMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(comboMeterRXMode.Items.Count == 0 ||
				comboMeterRXMode.SelectedIndex < 0)
			{
				current_meter_rx_mode = MeterRXMode.FIRST;
			}
			else
			{
				MeterRXMode mode = MeterRXMode.FIRST;
				switch(comboMeterRXMode.Text)
				{
					case "Signal":
						mode = MeterRXMode.SIGNAL_STRENGTH;
						break;
					case "Sig Avg":
						mode = MeterRXMode.SIGNAL_AVERAGE;
						break;
					case "ADC L":
						mode = MeterRXMode.ADC_L;
						break;
					case "ADC R":
						mode = MeterRXMode.ADC_R;
						break;
					case "Off":
						mode = MeterRXMode.OFF;
						break;
				}
				current_meter_rx_mode = mode;

				if(!chkMOX.Checked)
				{
					switch(mode)
					{
						case MeterRXMode.SIGNAL_STRENGTH:
						case MeterRXMode.SIGNAL_AVERAGE:
							lblMultiSMeter.Text = "  1   3   5   7   9  +20 +40 +60";
							break;
						case MeterRXMode.ADC_L:
						case MeterRXMode.ADC_R:
							lblMultiSMeter.Text = "-100  -80   -60   -40   -20    0";
							break;
						case MeterRXMode.OFF:
							lblMultiSMeter.Text = "";
							break;
					}
					ResetMultiMeterPeak();
				}
			}

			if(comboMeterRXMode.Focused)
				btnHidden.Focus();
		}

		private void comboMeterTXMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(comboMeterTXMode.Items.Count == 0 ||
				comboMeterTXMode.SelectedIndex < 0)
			{
				current_meter_tx_mode = MeterTXMode.FIRST;
			}
			else
			{
				MeterTXMode mode = MeterTXMode.FIRST;
				switch(comboMeterTXMode.Text)
				{
					case "Fwd Pwr":
						mode = MeterTXMode.FORWARD_POWER;
						break;
					case "Ref Pwr":
						mode = MeterTXMode.REVERSE_POWER;
						break;
					case "Mic":
						mode = MeterTXMode.MIC;
						break;
					case "EQ":
						mode = MeterTXMode.EQ;
						break;
					case "Leveler":
						mode = MeterTXMode.LEVELER;
						break;
					case "Lev Gain":
						mode = MeterTXMode.LVL_G;
						break;
					case "COMP":
						mode = MeterTXMode.COMP;
						break;
					case "CPDR":
						mode = MeterTXMode.CPDR;
						break;
					case "ALC":
						mode = MeterTXMode.ALC;
						break;
					case "ALC Comp":
						mode = MeterTXMode.ALC_G;
						break;
					case "SWR":
						mode = MeterTXMode.SWR;
						break;
					case "Off":
						mode = MeterTXMode.OFF;
						break;
				}
				current_meter_tx_mode = mode;
			}

			if(chkMOX.Checked)
			{
				switch(current_meter_tx_mode)
				{
					case MeterTXMode.FIRST:
						lblMultiSMeter.Text = "";
						break;
					case MeterTXMode.MIC:
					case MeterTXMode.EQ:
					case MeterTXMode.LEVELER:
					case MeterTXMode.COMP:
					case MeterTXMode.CPDR:
					case MeterTXMode.ALC:
						lblMultiSMeter.Text = "-20    -10     -5      0   1   2   3";
						break;
					case MeterTXMode.FORWARD_POWER:
					case MeterTXMode.REVERSE_POWER:
						if(pa_present)
							lblMultiSMeter.Text = "1      5     10    50   100  120+";
						else
							lblMultiSMeter.Text = "0      0.1     0.2     0.5        1.0";
						break;
					case MeterTXMode.SWR:
						lblMultiSMeter.Text = "1      1.5   2     3     5    10";
						lblMultiSMeter.Text = "0             10              20";
						break;
					case MeterTXMode.OFF:
						lblMultiSMeter.Text = "";
						break;
					case MeterTXMode.LVL_G:
					case MeterTXMode.ALC_G:
						lblMultiSMeter.Text = "0       5       10      15      20";
						break;
				}
				ResetMultiMeterPeak();
			}

			if(comboMeterTXMode.Focused)
				btnHidden.Focus();
		}

		private void chkDisplayAVG_CheckedChanged(object sender, System.EventArgs e)
		{
			Display.AverageOn = chkDisplayAVG.Checked;
			if(chkDisplayAVG.Checked)
			{
				chkDisplayAVG.BackColor = button_selected_color;
			}
			else
			{
				chkDisplayAVG.BackColor = SystemColors.Control;
			}
			
			if(chkDisplayAVG.Checked)
			{
				switch(Display.CurrentDisplayMode)
				{
					case DisplayMode.PANADAPTER:
					case DisplayMode.HISTOGRAM:
					case DisplayMode.SPECTRUM:
					case DisplayMode.WATERFALL:
						btnZeroBeat.Enabled = true; // only allow zerobeat when avg is on 
						break;
					default:
						btnZeroBeat.Enabled = false;
						break;
				}
			}
			else btnZeroBeat.Enabled = false;
		}

		private void chkDisplayPeak_CheckedChanged(object sender, System.EventArgs e)
		{
			Display.PeakOn = chkDisplayPeak.Checked;
			if(chkDisplayPeak.Checked)
			{
				chkDisplayPeak.BackColor = button_selected_color;
			}
			else
			{
				chkDisplayPeak.BackColor = SystemColors.Control;
			}			
		}

		private void chkSquelch_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkSquelch.Checked)
			{
				chkSquelch.BackColor = button_selected_color;
				DttSP.SetSquelchState(1);
			}
			else
			{
				chkSquelch.BackColor = SystemColors.Control;
				DttSP.SetSquelchState(0);
			}
		}

		private void chkTUN_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkTUN.Checked)
			{
				if(!chkPower.Checked)
				{
					MessageBox.Show("Power must be on to turn on the Tune function.",
						"Power is off",
						MessageBoxButtons.OK,
						MessageBoxIcon.Hand);
					chkTUN.Checked = false;
					return;
				}

				chkTUN.BackColor = button_selected_color;
				Audio.CurrentAudioState1 = Audio.AudioState.SINL_COSR;

				CWForm.SendEnabled = false;
				PreviousPWR = (int)udPWR.Value;
				udPWR.Value = tune_power;
				chkMOX.Checked = true;
				if(!chkMOX.Checked)
				{
					chkTUN.Checked = false;
					return;
				}		

				if(atu_present && current_band != Band.B2M &&
					(ATUTuneMode)comboTuneMode.SelectedIndex != ATUTuneMode.BYPASS)
				{
					chkTUN.Enabled = false;
					comboTuneMode.Enabled = false;

					Thread t = new Thread(new ThreadStart(Tune));
					t.IsBackground = true;
					t.Priority = ThreadPriority.Normal;
					t.Name = "TUN Thread";
					t.Start();

					/*switch((ATUTuneMode)comboTuneMode.SelectedIndex)
					{
						case ATUTuneMode.MEMORY:
							udPWR.Value = 15;
							atu_tuning = true;
							if(!Hdw.PA_ATUTune(ATUTuneMode.MEMORY))
								goto atu_error;
							chkTUN.Checked = false;
							tuned_band = current_band;
							break;
						case ATUTuneMode.FULL:
							udPWR.Value = 15;
							atu_tuning = true;
							if(!Hdw.PA_ATUTune(ATUTuneMode.FULL))
								goto atu_error;
							chkTUN.Checked = false;
							tuned_band = current_band;
							break;
					}*/
				}
				else
				{
					if(pa_present && !comboMeterTXMode.Items.Contains("SWR"))
					{
						int index = comboMeterTXMode.SelectedIndex;
						comboMeterTXMode.Items.Add("SWR");
						comboMeterTXMode.SelectedIndex = index;
					}
				}
				return;
/*
				atu_error:
					chkTUN.Checked = false;
					MessageBox.Show("Error communicating with the ATU",
						"ATU Error",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);				
*/
			}
			else
			{
				chkMOX.Checked = false;

				Audio.NextAudioState1 = Audio.AudioState.DTTSP;
				Audio.SwitchCount = 2048/BlockSize1*2;
				Audio.CurrentAudioState1 = Audio.AudioState.SWITCH;

				CWForm.SendEnabled = true;
				chkTUN.BackColor = SystemColors.Control;

				/*if(atu_present && current_band != Band.B2M &&
					((ATUTuneMode)comboTuneMode.SelectedIndex != ATUTuneMode.BYPASS))
				{
					switch((ATUTuneMode)comboTuneMode.SelectedIndex)
					{
						case ATUTuneMode.MEMORY:
							atu_tuning = false;
							udPWR.Value = PreviousPWR;
							break;
						case ATUTuneMode.FULL:
							atu_tuning = false;
							udPWR.Value = PreviousPWR;
							break;
					}
				}
				else*/
				{					
					if(pa_present && comboMeterTXMode.Items.Contains("SWR"))
					{
						comboMeterTXMode.Items.Remove("SWR");
						if(comboMeterTXMode.SelectedIndex < 0)
							comboMeterTXMode.SelectedIndex = 0;
					}
					TunePower = (int)udPWR.Value;
					udPWR.Value = PreviousPWR;
				}
			}
		}

		private void comboTuneMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(atu_present)
			{
				if(comboTuneMode.SelectedIndex == (int)ATUTuneMode.BYPASS)
				{
					Hdw.PA_ATUTune(ATUTuneMode.BYPASS);	
					tuned_band = Band.FIRST;
					if(chkTUN.BackColor == button_selected_color)
						chkTUN.BackColor = SystemColors.Control;
				}
			}

			if(comboTuneMode.Focused)
				btnHidden.Focus();
		}

		private void HideFocus(object sender, EventArgs e)
		{
			btnHidden.Focus();
		}

		private void chkVFOLock_CheckedChanged(object sender, System.EventArgs e)
		{
			VFOLock = chkVFOLock.Checked;
			if(chkVFOLock.Checked)
				chkVFOLock.BackColor = button_selected_color;
			else
				chkVFOLock.BackColor = SystemColors.Control;
		}

		private void btnBandVHF_Click(object sender, System.EventArgs e)
		{
			grpBandVHF.Visible = true;
			grpBandHF.Visible = false;	
		}

		private void btnBandHF_Click(object sender, System.EventArgs e)
		{
			grpBandHF.Visible = true;
			grpBandVHF.Visible = false;
		}

		private void udCWSpeed_ValueChanged(object sender, System.EventArgs e)
		{
			DttSP.SetKeyerSpeed((float)udCWSpeed.Value);
		}

		private void udPWR_LostFocus(object sender, EventArgs e)
		{
			udPWR_ValueChanged(sender, e);
		}

		private void udAF_LostFocus(object sender, EventArgs e)
		{
			udAF_ValueChanged(sender, e);
		}

		private void udMIC_LostFocus(object sender, EventArgs e)
		{
			udMIC_ValueChanged(sender, e);
		}

		private void udSquelch_LostFocus(object sender, EventArgs e)
		{
			udSquelch_ValueChanged(sender, e);
		}

		private void udFilterLow_LostFocus(object sender, EventArgs e)
		{
			udFilterLow_ValueChanged(sender, e);
		}

		private void udFilterHigh_LostFocus(object sender, EventArgs e)
		{
			udFilterHigh_ValueChanged(sender, e);
		}

		private void udRIT_LostFocus(object sender, EventArgs e)
		{
			udRIT_ValueChanged(sender, e);
		}

		private void udXIT_LostFocus(object sender, EventArgs e)
		{
			udXIT_ValueChanged(sender, e);
		}

		private void udCWSpeed_LostFocus(object sender, EventArgs e)
		{
			udCWSpeed_ValueChanged(sender, e);
		}

		private void DateTime_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(current_datetime_mode+1 == DateTimeMode.LAST)
				CurrentDateTimeMode = DateTimeMode.OFF;
			else CurrentDateTimeMode = current_datetime_mode+1;
		}

		#endregion

		#region Band Button Events
// ======================================================
// Band Button Events
// ======================================================

		private void btnBand160_Click(object sender, System.EventArgs e)
		{
			//Hdw.UpdateHardware = false;
			SaveBand();
			if(last_band.Equals("160M"))
			{
				if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					band_160m_index = (band_160m_index-1+band_160m_register)%band_160m_register;
				else
					band_160m_index = (band_160m_index+1)%band_160m_register;
			}
			last_band = "160M";

			string filter, mode;
			double freq;
			if(DB.GetBandStack(last_band, band_160m_index, out mode, out filter, out freq))
			{
				SetBand(mode, filter, freq);
			}
			
//			if(chkPower.Checked)
//				Hdw.UpdateHardware = true;
		}

		private void btnBand80_Click(object sender, System.EventArgs e)
		{
//			Hdw.UpdateHardware = false;
			SaveBand();
			if(last_band.Equals("80M"))
			{
				if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					band_80m_index = (band_80m_index-1+band_80m_register)%band_80m_register;
				else
					band_80m_index = (band_80m_index+1)%band_80m_register;
			}
			last_band = "80M";

			string filter, mode;
			double freq;
			if(DB.GetBandStack(last_band, band_80m_index, out mode, out filter, out freq))
			{
				SetBand(mode, filter, freq);
			}

//			if(chkPower.Checked)
//				Hdw.UpdateHardware = true;
		}

		private void btnBand60_Click(object sender, System.EventArgs e)
		{
//			Hdw.UpdateHardware = false;
			SaveBand();
			if(last_band.Equals("60M"))
			{
				if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					band_60m_index = (band_60m_index-1+band_60m_register)%band_60m_register;
				else
					band_60m_index = (band_60m_index+1)%band_60m_register;
			}
			last_band = "60M";

			string filter, mode;
			double freq;
			if(DB.GetBandStack(last_band, band_60m_index, out mode, out filter, out freq))
			{
				SetBand(mode, filter, freq);
			}

//			if(chkPower.Checked)
//				Hdw.UpdateHardware = true;
		}

		private void btnBand40_Click(object sender, System.EventArgs e)
		{
//			Hdw.UpdateHardware = false;
			SaveBand();
			if(last_band.Equals("40M"))
			{
				if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					band_40m_index = (band_40m_index-1+band_40m_register)%band_40m_register;
				else
					band_40m_index = (band_40m_index+1)%band_40m_register;
			}
			last_band = "40M";

			string filter, mode;
			double freq;
			if(DB.GetBandStack(last_band, band_40m_index, out mode, out filter, out freq))
			{
				SetBand(mode, filter, freq);
			}

//			if(chkPower.Checked)
//				Hdw.UpdateHardware = true;
		}

		private void btnBand30_Click(object sender, System.EventArgs e)
		{
//			Hdw.UpdateHardware = false;
			SaveBand();
			if(last_band.Equals("30M"))
			{
				if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					band_30m_index = (band_30m_index-1+band_30m_register)%band_30m_register;
				else
					band_30m_index = (band_30m_index+1)%band_30m_register;
			}
			last_band = "30M";

			string filter, mode;
			double freq;
			if(DB.GetBandStack(last_band, band_30m_index, out mode, out filter, out freq))
			{
				SetBand(mode, filter, freq);
			}

//			if(chkPower.Checked)
//				Hdw.UpdateHardware = true;
		}

		private void btnBand20_Click(object sender, System.EventArgs e)
		{
//			Hdw.UpdateHardware = false;
			SaveBand();
			if(last_band.Equals("20M"))
			{
				if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					band_20m_index = (band_20m_index-1+band_20m_register)%band_20m_register;
				else
					band_20m_index = (band_20m_index+1)%band_20m_register;
			}
			last_band = "20M";

			string filter, mode;
			double freq;
			if(DB.GetBandStack(last_band, band_20m_index, out mode, out filter, out freq))
			{
				SetBand(mode, filter, freq);
			}

//			if(chkPower.Checked)
//				Hdw.UpdateHardware = true;
		}

		private void btnBand17_Click(object sender, System.EventArgs e)
		{
//			Hdw.UpdateHardware = false;
			SaveBand();
			if(last_band.Equals("17M"))
			{
				if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					band_17m_index = (band_17m_index-1+band_17m_register)%band_17m_register;
				else
					band_17m_index = (band_17m_index+1)%band_17m_register;
			}
			last_band = "17M";

			string filter, mode;
			double freq;
			if(DB.GetBandStack(last_band, band_17m_index, out mode, out filter, out freq))
			{
				SetBand(mode, filter, freq);
			}

//			if(chkPower.Checked)
//				Hdw.UpdateHardware = true;
		}

		private void btnBand15_Click(object sender, System.EventArgs e)
		{
//			Hdw.UpdateHardware = false;
			SaveBand();
			if(last_band.Equals("15M"))
			{
				if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					band_15m_index = (band_15m_index-1+band_15m_register)%band_15m_register;
				else
					band_15m_index = (band_15m_index+1)%band_15m_register;
			}
			last_band = "15M";

			string filter, mode;
			double freq;
			if(DB.GetBandStack(last_band, band_15m_index, out mode, out filter, out freq))
			{
				SetBand(mode, filter, freq);
			}

//			if(chkPower.Checked)
//				Hdw.UpdateHardware = true;
		}

		private void btnBand12_Click(object sender, System.EventArgs e)
		{
//			Hdw.UpdateHardware = false;
			SaveBand();
			if(last_band.Equals("12M"))
			{
				if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					band_12m_index = (band_12m_index-1+band_12m_register)%band_12m_register;
				else
					band_12m_index = (band_12m_index+1)%band_12m_register;
			}
			last_band = "12M";

			string filter, mode;
			double freq;
			if(DB.GetBandStack(last_band, band_12m_index, out mode, out filter, out freq))
			{
				SetBand(mode, filter, freq);
			}

//			if(chkPower.Checked)
//				Hdw.UpdateHardware = true;
		}

		private void btnBand10_Click(object sender, System.EventArgs e)
		{
//			Hdw.UpdateHardware = false;
			SaveBand();
			if(last_band.Equals("10M"))
			{
				if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					band_10m_index = (band_10m_index-1+band_10m_register)%band_10m_register;
				else
					band_10m_index = (band_10m_index+1)%band_10m_register;
			}
			last_band = "10M";

			string filter, mode;
			double freq;
			if(DB.GetBandStack(last_band, band_10m_index, out mode, out filter, out freq))
			{
				SetBand(mode, filter, freq);
			}

//			if(chkPower.Checked)
//				Hdw.UpdateHardware = true;
		}

		private void btnBand6_Click(object sender, System.EventArgs e)
		{
//			Hdw.UpdateHardware = false;
			SaveBand();
			if(last_band.Equals("6M"))
			{
				if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					band_6m_index = (band_6m_index-1+band_6m_register)%band_6m_register;
				else
					band_6m_index = (band_6m_index+1)%band_6m_register;
			}
			last_band = "6M";

			string filter, mode;
			double freq;
			if(DB.GetBandStack(last_band, band_6m_index, out mode, out filter, out freq))
			{
				SetBand(mode, filter, freq);
			}

//			if(chkPower.Checked)
//				Hdw.UpdateHardware = true;
		}

		private void btnBand2_Click(object sender, System.EventArgs e)
		{
//			Hdw.UpdateHardware = false;
			SaveBand();
			if(last_band.Equals("2M"))
			{
				if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					band_2m_index = (band_2m_index-1+band_2m_register)%band_2m_register;
				else
					band_2m_index = (band_2m_index+1)%band_2m_register;
			}
			last_band = "2M";

			string filter, mode;
			double freq;
			if(DB.GetBandStack(last_band, band_2m_index, out mode, out filter, out freq))
			{
				SetBand(mode, filter, freq);
			}

//			if(chkPower.Checked)
//				Hdw.UpdateHardware = true;
		}

		private void btnBandWWV_Click(object sender, System.EventArgs e)
		{
//			Hdw.UpdateHardware = false;
			SaveBand();
			if(last_band.Equals("WWV"))
			{
				if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					band_wwv_index = (band_wwv_index-1+band_wwv_register)%band_wwv_register;
				else
					band_wwv_index = (band_wwv_index+1)%band_wwv_register;
			}
			last_band = "WWV";

			string filter, mode;
			double freq;
			if(DB.GetBandStack(last_band, band_wwv_index, out mode, out filter, out freq))
			{
				SetBand(mode, filter, freq);
			}

//			if(chkPower.Checked)
//				Hdw.UpdateHardware = true;
		}

		private void btnBandGEN_Click(object sender, System.EventArgs e)
		{
//			Hdw.UpdateHardware = false;
			SaveBand();
			if(last_band == "GEN")
			{
				if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					band_gen_index = (band_gen_index-1+band_gen_register)%band_gen_register;
				else
					band_gen_index = (band_gen_index+1)%band_gen_register;
			}
			last_band = "GEN";

			string filter, mode;
			double freq;
			if(DB.GetBandStack(last_band, band_gen_index, out mode, out filter, out freq))
			{
				SetBand(mode, filter, freq);
			}

//			if(chkPower.Checked)
//				Hdw.UpdateHardware = true;
		}

		private void btnBandVHF0_Click(object sender, System.EventArgs e)
		{
			SaveBand();

			string new_band = ((Control)sender).Name.Substring(7);
			int index = -1, register = -1;

			switch(new_band)
			{
				case "VHF0": index = band_vhf0_index; register = band_vhf0_register; break;
				case "VHF1": index = band_vhf1_index; register = band_vhf1_register; break;
				case "VHF2": index = band_vhf2_index; register = band_vhf2_register; break;
				case "VHF3": index = band_vhf3_index; register = band_vhf3_register; break;
				case "VHF4": index = band_vhf4_index; register = band_vhf4_register; break;
				case "VHF5": index = band_vhf5_index; register = band_vhf5_register; break;
				case "VHF6": index = band_vhf6_index; register = band_vhf6_register; break;
				case "VHF7": index = band_vhf7_index; register = band_vhf7_register; break;
				case "VHF8": index = band_vhf8_index; register = band_vhf8_register; break;
				case "VHF9": index = band_vhf9_index; register = band_vhf9_register; break;
				case "VHF10": index = band_vhf10_index; register = band_vhf10_register; break;
				case "VHF11": index = band_vhf11_index; register = band_vhf11_register; break;
				case "VHF12": index = band_vhf12_index; register = band_vhf12_register; break;
				case "VHF13": index = band_vhf13_index; register = band_vhf13_register; break;
			}

			int xvtr_index = Int32.Parse(new_band.Substring(3));
			double start_freq = XVTRForm.GetBegin(xvtr_index);
			double end_freq = XVTRForm.GetEnd(xvtr_index);
			if(register < 3)
			{
				for(int i=0; i<3-register; i++)
					DB.AddBandStack(new_band, "USB", "2600", start_freq+i*0.0010);

				UpdateBandStackRegisters();
				register = 3;
			}
			else
			{
				if(last_band == new_band)
				{
					if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
						index = (index-1+register)%register;
					else
						index = (index+1)%register;
				}

				switch(new_band)
				{
					case "VHF0": band_vhf0_index = index; break;
					case "VHF1": band_vhf1_index = index; break;
					case "VHF2": band_vhf2_index = index; break;
					case "VHF3": band_vhf3_index = index; break;
					case "VHF4": band_vhf4_index = index; break;
					case "VHF5": band_vhf5_index = index; break;
					case "VHF6": band_vhf6_index = index; break;
					case "VHF7": band_vhf7_index = index; break;
					case "VHF8": band_vhf8_index = index; break;
					case "VHF9": band_vhf9_index = index; break;
					case "VHF10": band_vhf10_index = index; break;
					case "VHF11": band_vhf11_index = index; break;
					case "VHF12": band_vhf12_index = index; break;
					case "VHF13": band_vhf13_index = index; break;
				}
			}
			last_band = new_band;

			string filter, mode;
			double freq;

			for(int i=0; i<3; i++)
			{
				DB.GetBandStack(last_band, i, out mode, out filter, out freq);
				if(freq < start_freq || freq > end_freq)
					DB.SaveBandStack(last_band, i, mode, filter, start_freq+i*0.001);
			}
			
			if(DB.GetBandStack(last_band, index, out mode, out filter, out freq))
			{
				SetBand(mode, filter, freq);
			}            
		}

		#endregion

		#region Mode Button Events
// ======================================================
// Mode Button Events
// ======================================================

		private void SetMode(DSPMode new_mode)
		{
			DttSP.CurrentMode = (DttSP.Mode)new_mode;				// set new DSP mode
			Audio.CurDSPMode = new_mode;

			double freq = double.Parse(txtVFOAFreq.Text);

			tbFilterShift.Value = 0;
			btnFilterShiftReset.BackColor = SystemColors.Control;

			switch(current_dsp_mode)
			{
				case DSPMode.LSB:
					radModeLSB.BackColor = SystemColors.Control;
					break;
				case DSPMode.USB:
					radModeUSB.BackColor = SystemColors.Control;
					break;
				case DSPMode.DSB:
					radModeDSB.BackColor = SystemColors.Control;
					break;
				case DSPMode.CWL:
					radModeCWL.BackColor = SystemColors.Control;
					if(new_keyer) 
					{
						DttSP.StopKeyer();
						cw_key_mode = false;
					}
					switch(new_mode)
					{
						case DSPMode.USB:
							freq -= (cw_pitch*0.0000010);
							break;
						case DSPMode.CWU:
							break;
						default:
							freq += (cw_pitch*0.0000010);
							break;
					}
					txtVFOAFreq.Text = freq.ToString("f6");
					break;
				case DSPMode.CWU:
					radModeCWU.BackColor = SystemColors.Control;
					if(new_keyer) 
					{
						DttSP.StopKeyer();
						cw_key_mode = false;
					}
					switch(new_mode)
					{
						case DSPMode.LSB:
							freq += (cw_pitch*0.0000010);
							break;
						case DSPMode.CWL:
							break;
						default:
							freq -= (cw_pitch*0.0000010);
							break;
					}
					txtVFOAFreq.Text = freq.ToString("f6");
					break;
				case DSPMode.FMN:
					radModeFMN.BackColor = SystemColors.Control;
					if(new_mode != DSPMode.AM && 
						new_mode != DSPMode.SAM &&
						new_mode != DSPMode.FMN)
					{
						chkMON.Enabled = true;
						chkBIN.Enabled = true;
					}
					int pwr = (int)udPWR.Value;
					udPWR.Maximum = 100;
					udPWR.Value = pwr*4;
					break;
				case DSPMode.AM:
					radModeAM.BackColor = SystemColors.Control;
					if(new_mode != DSPMode.AM && 
						new_mode != DSPMode.SAM &&
						new_mode != DSPMode.FMN)
					{
						chkMON.Enabled = true;
						chkBIN.Enabled = true;
					}
					break;
				case DSPMode.SAM:
					radModeSAM.BackColor = SystemColors.Control;
					if(new_mode != DSPMode.AM && 
						new_mode != DSPMode.SAM &&
						new_mode != DSPMode.FMN)
					{
						chkMON.Enabled = true;
						chkBIN.Enabled = true;
					}
					break;
				case DSPMode.SPEC:
					radModeSPEC.BackColor = SystemColors.Control;
					comboDisplayMode.Items.Insert(1, "Panadapter");
					tbFilterShift.Enabled = true;
					btnFilterShiftReset.Enabled = true;
					if(new_mode != DSPMode.DRM)
						EnableAllFilters();
					if_shift = true;
					if(!spur_reduction)
						DttSP.SetOsc(-11025.0);
					if(was_panadapter) comboDisplayMode.Text = "Panadapter";
					break;
				case DSPMode.DIGL:
					radModeDIGL.BackColor = SystemColors.Control;
					if(vac_auto_enable &&
						new_mode != DSPMode.DIGU &&
						new_mode != DSPMode.DRM)
					{
						SetupForm.VACEnable = false;
					}
						// SN 04/06 reset unused FSK filters
						EnableAllFilters();
					radModeDIGL.Text = "DIGL";
					break;
				case DSPMode.DIGU:
					radModeDIGU.BackColor = SystemColors.Control;
					if(vac_auto_enable &&
						new_mode != DSPMode.DIGL &&
						new_mode != DSPMode.DRM)
					{
						SetupForm.VACEnable = false;
					}
					break;
				case DSPMode.DRM:
					radModeDRM.BackColor = SystemColors.Control;
					if(vac_auto_enable &&
						new_mode != DSPMode.DIGL &&
						new_mode != DSPMode.DIGU)
						SetupForm.VACEnable = false;
					tbFilterShift.Enabled = true;
					btnFilterShiftReset.Enabled = true;
					if(new_mode != DSPMode.SPEC)
						EnableAllFilters();
					vfo_offset = 0.0;
					if_shift = true;
					break;
			}

			switch(new_mode)
			{
				case DSPMode.LSB:
					radModeLSB.BackColor = button_selected_color;
					grpMode.Text = "Mode - LSB";
					DttSP.SetTXOsc(0.0);
					if(!rx_only && chkPower.Checked)
						chkMOX.Enabled = true;
					DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
					break;
				case DSPMode.USB:
					radModeUSB.BackColor = button_selected_color;
					grpMode.Text = "Mode - USB";
					DttSP.SetTXOsc(0.0);
					if(!rx_only && chkPower.Checked)
						chkMOX.Enabled = true;
					DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
					break;
				case DSPMode.DSB:
					radModeDSB.BackColor = button_selected_color;
					grpMode.Text = "Mode - DSB";
					DttSP.SetTXOsc(0.0);
					if(!rx_only && chkPower.Checked)
						chkMOX.Enabled = true;
					DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
					break;
				case DSPMode.CWL:
					radModeCWL.BackColor = button_selected_color;
					grpMode.Text = "Mode - CWL";
					DttSP.SetTXOsc(0.0);
					DttSP.SetKeyerFreq(cw_pitch);
					if(!rx_only && chkPower.Checked)
					{
						chkMOX.Enabled = true;
						if (new_keyer)
						{
							DttSP.CWRingRestart();
							DttSP.StopKeyer();
							DttSP.StartKeyer();
						}				
					}
					DttSP.SetTXFilters(tx_filter_low, tx_filter_high);

					switch(current_dsp_mode)
					{
						case DSPMode.USB:
							freq += (cw_pitch*0.0000010);
							break;
						case DSPMode.CWU:
							break;
						default:
							freq -= (cw_pitch*0.0000010);
							break;
					}
					txtVFOAFreq.Text = freq.ToString("f6");
					break;
				case DSPMode.CWU:
					radModeCWU.BackColor = button_selected_color;
					grpMode.Text = "Mode - CWU";
					DttSP.SetTXOsc(0.0);
					DttSP.SetKeyerFreq(-cw_pitch);
					if(!rx_only && chkPower.Checked)
					{
						chkMOX.Enabled = true;
						if (new_keyer) 
						{
							DttSP.CWRingRestart();
							DttSP.StopKeyer();
							DttSP.StartKeyer();
						}					
					}
					DttSP.SetTXFilters(tx_filter_low, tx_filter_high);

					switch(current_dsp_mode)
					{
						case DSPMode.LSB:
							freq -= (cw_pitch*0.0000010);
							break;
						case DSPMode.CWL:
							break;
						default:
							freq += (cw_pitch*0.0000010);
							break;
					}
					txtVFOAFreq.Text = freq.ToString("f6");
					break;
				case DSPMode.FMN:
					radModeFMN.BackColor = button_selected_color;
					grpMode.Text = "Mode - FMN";
					int pwr = (int)udPWR.Value;
					udPWR.Value = pwr/4;
					udPWR.Maximum = 25;					
					if(!rx_only && chkPower.Checked)
						chkMOX.Enabled = true;
					chkMON.Checked = false;
					chkMON.Enabled = false;
					chkBIN.Checked = false;
					chkBIN.Enabled = false;
					DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
					DttSP.SetTXOsc(11025.0);
					break;
				case DSPMode.AM:
					radModeAM.BackColor = button_selected_color;
					grpMode.Text = "Mode - AM";
					if(!rx_only && chkPower.Checked)
						chkMOX.Enabled = true;
					chkMON.Checked = false;
					chkMON.Enabled = false;
					chkBIN.Checked = false;
					chkBIN.Enabled = false;
					DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
					DttSP.SetTXOsc(11025.0);
					break;
				case DSPMode.SAM:
					radModeSAM.BackColor = button_selected_color;
					grpMode.Text = "Mode - SAM";
					if(!rx_only && chkPower.Checked)
						chkMOX.Enabled = true;
					chkMON.Checked = false;
					chkMON.Enabled = false;
					chkBIN.Checked = false;
					chkBIN.Enabled = false;
					DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
					DttSP.SetTXOsc(11025.0);
					break;
				case DSPMode.SPEC:
					radModeSPEC.BackColor = button_selected_color;
					grpMode.Text = "Mode - SPEC";
					if_shift = false;
					DttSP.SetOsc(0.0);			
					DeselectAllFilters();
					DisableAllFilters();
					grpFilter.Text = "Filter - "+(DttSP.SampleRate/1000).ToString("f0")+"kHz";
					DttSP.RXDisplayLow = -(int)DttSP.SampleRate/2;
					DttSP.RXDisplayHigh = (int)DttSP.SampleRate/2;
					tbFilterShift.Enabled = false;
					btnFilterShiftReset.Enabled = false;
					bool save_pan;
					if(save_pan = (Display.CurrentDisplayMode == DisplayMode.PANADAPTER))
					{
						comboDisplayMode.Text = "Spectrum";
					}
					comboDisplayMode.Items.Remove("Panadapter");
					was_panadapter = save_pan;
					break;
				case DSPMode.DIGL:
					radModeDIGL.BackColor = button_selected_color;
					grpMode.Text = "Mode - DIGL";
					DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
					// SN 04/06 setup RTTY FSK
					if(SetupForm.RTTYFSK)	// RTTY FSK is selected in setup form
					{	
						bFSK=true;
						radModeDIGL.BackColor = Color.LightPink;	// shows FSK active
						DisableFilters(250);
						radModeDIGL.Text = "FSK";
					}
					else 
					{	
						bFSK=false;
						radModeDIGL.BackColor = button_selected_color;
					}
					//

					if(vac_auto_enable)
						SetupForm.VACEnable = true;
					break;
				case DSPMode.DIGU:
					radModeDIGU.BackColor = button_selected_color;
					grpMode.Text = "Mode - DIGU";
					DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
					if(vac_auto_enable)
						SetupForm.VACEnable = true;
					break;
				case DSPMode.DRM:
					if_shift = false;
					vfo_offset = -0.012;
					radModeDRM.BackColor = button_selected_color;
					grpMode.Text = "Mode - DRM";
					if(vac_auto_enable)
						SetupForm.VACEnable = true;
					chkMOX.Enabled = false;
					DeselectAllFilters();
					DisableAllFilters();
					tbFilterShift.Enabled = false;
					btnFilterShiftReset.Enabled = false;
					grpFilter.Text = "Filter - DRM";
//					DttSP.SetRXFilters(6650, 17250);
					DttSP.SetRXFilters(6700, 17200);
					DttSP.RXDisplayLow = -8000;
					DttSP.RXDisplayHigh = 8000;
					break;
			}

			current_dsp_mode = new_mode;
			if(current_dsp_mode == DSPMode.SPEC ||
				current_dsp_mode == DSPMode.DRM)
				CurrentFilter = Filter.NONE;
			else CurrentFilter = last_filter[(int)new_mode];

			tbFilterWidthScroll_newMode(); // wjt 

			Display.DrawBackground();
			txtVFOAFreq_LostFocus(this, EventArgs.Empty);            
		}

		private void radModeLSB_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeLSB.Checked)
			{
				SetMode(DSPMode.LSB);
			}
		}

		private void radModeUSB_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeUSB.Checked)
			{
				SetMode(DSPMode.USB);
			}
		}

		private void radModeDSB_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeDSB.Checked)
			{
				SetMode(DSPMode.DSB);
			}
		}

		private void radModeCWL_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeCWL.Checked)
			{
				SetMode(DSPMode.CWL);
			}

		}

		private void radModeCWU_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeCWU.Checked)
			{
				SetMode(DSPMode.CWU);
			}
		}

		private void radModeFMN_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeFMN.Checked)
			{
				SetMode(DSPMode.FMN);
			}
		}

		private void radModeAM_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeAM.Checked)
			{
				SetMode(DSPMode.AM);
			}
		}

		private void radModeSAM_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeSAM.Checked)
			{
				SetMode(DSPMode.SAM);
			}
		}

		private void radModeDIGU_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeDIGU.Checked)
			{
				SetMode(DSPMode.DIGU);
			}
		}

		private void radModeSPEC_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeSPEC.Checked)
			{
				SetMode(DSPMode.SPEC);
			}
		}

		private void radModeDIGL_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeDIGL.Checked)
			{
				SetMode(DSPMode.DIGL);
			}
		}

		private void radModeDRM_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeDRM.Checked)
			{
				SetMode(DSPMode.DRM);
			}
		}

		#endregion

		#region Filter Button Events
// ======================================================
// Filter Button Events
// ======================================================

		private void SetFilter(Filter new_filter)
		{
			int low = 0, high = 0;
			if((int)current_dsp_mode < 0) return;
			last_filter[(int)current_dsp_mode] = new_filter;

			switch(current_filter)
			{
				case Filter.F6000:
					radFilter6000.BackColor = SystemColors.Control;
					break;
				case Filter.F4000:
					radFilter4000.BackColor = SystemColors.Control;
					break;
				case Filter.F2600:
					radFilter2600.BackColor = SystemColors.Control;
					break;
				case Filter.F2100:
					radFilter2100.BackColor = SystemColors.Control;
					break;
				case Filter.F1000:
					radFilter1000.BackColor = SystemColors.Control;
					break;
				case Filter.F500:
					radFilter500.BackColor = SystemColors.Control;
					break;
				case Filter.F250:
					radFilter250.BackColor = SystemColors.Control;
					break;
				case Filter.F100:
					radFilter100.BackColor = SystemColors.Control;
					break;
				case Filter.F50:
					radFilter50.BackColor = SystemColors.Control;
					break;
				case Filter.F25:
					radFilter25.BackColor = SystemColors.Control;
					break;
				case Filter.VAR1:
					radFilterVar1.BackColor = SystemColors.Control;
					udFilterLow.BackColor = SystemColors.Window;
					udFilterHigh.BackColor = SystemColors.Window;
					udFilterLow.Enabled = false;
					udFilterHigh.Enabled = false;
					break;
				case Filter.VAR2:
					radFilterVar2.BackColor = SystemColors.Control;
					udFilterLow.BackColor = SystemColors.Window;
					udFilterHigh.BackColor = SystemColors.Window;
					udFilterLow.Enabled = false;
					udFilterHigh.Enabled = false;
					break;
			}

			int saved_filter_shift = tbFilterShift.Value;
			tbFilterShift.Value = 0;
			last_filter_shift = 0;
			last_var1_shift = 0;
			last_var2_shift = 0;
//			btnFilterShiftReset.BackColor = SystemColors.Control;

			current_filter = new_filter;
// SN 04/06 setup RTTY FSK filters 6000 - 250
			switch(new_filter)
			{
				case Filter.F6000:
					radFilter6000.BackColor = button_selected_color;
					grpFilter.Text = "Filter - 6.0kHz";
				switch(current_dsp_mode)
				{
					case DSPMode.DIGU:
						low = 100; high = 6000;
						break;
					case DSPMode.LSB:
						low = -6000; high = 0;
						break;
					case DSPMode.DIGL:
						FilterFreq = 6000;
						if(bFSK)
						{								
							low = -(FSKfreq + (FilterFreq/2)); 
							high = -100;
						}
						else {low = -FilterFreq; high = -100;}
						break;
					case DSPMode.USB:
						low = 100; high = 6000;
						break;
					case DSPMode.CWL:
						high = Math.Min(-cw_pitch + 3000, -100);
						low = high - 6000;
						break;
					case DSPMode.CWU:
						low = Math.Max(cw_pitch - 3000, 100);
						high = low + 6000;
						break;
					case DSPMode.DSB:
					case DSPMode.AM:
					case DSPMode.SAM:
					case DSPMode.FMN:
						low = -6000; high = 6000;
						break;
					default:
						low = -6000; high = 6000;
						break;
				}
					break;
				case Filter.F4000:
					radFilter4000.BackColor = button_selected_color;
					grpFilter.Text = "Filter - 4.0kHz";
				switch(current_dsp_mode)
				{
					case DSPMode.LSB:
						low = -4200; high = -200;
						break;
					case DSPMode.DIGL:
						FilterFreq = 4000;
						if(bFSK)
						{								
							low = -(FSKfreq + (FilterFreq/2)); 
							high = -100;
						}
						else {low = -FilterFreq; high = -100;}
						break;
					case DSPMode.DIGU:
					case DSPMode.USB:
						low = 100; high = 4000;
						break;
					case DSPMode.CWL:
						high = Math.Min(-cw_pitch + 2000, -100);
						low = high - 4000;
						break;
					case DSPMode.CWU:
						low = Math.Max(cw_pitch - 2000, 100);
						high = low + 4000;
						break;
					case DSPMode.DSB:
					case DSPMode.AM:
					case DSPMode.SAM:
					case DSPMode.FMN:
						low = -4000; high = 4000;
						break;
					default:
						low = -4000; high = 4000;
						break;
				}
					break;
				case Filter.F2600:
					radFilter2600.BackColor = button_selected_color;
					grpFilter.Text = "Filter - 2.6kHz";
				switch(current_dsp_mode)
				{
					case DSPMode.LSB:
						low = -2800; high = -200;
						break;
					case DSPMode.DIGL:
						FilterFreq = 2600;
						if(bFSK)
						{							
							low = -(FSKfreq + (FilterFreq/2));
							high = -(FSKfreq - (FilterFreq/2));
						}
						else {low = -FilterFreq; high = -100;}
						break;
					case DSPMode.USB:
					case DSPMode.DIGU:
						low = 200; high = 2800;
						break;
					case DSPMode.CWL:
						high = Math.Min(-cw_pitch + 1300, -100);
						low = high - 2600;
						break;
					case DSPMode.CWU:
						low = Math.Max(cw_pitch - 1300, 100);
						high = low + 2600;
						break;
					case DSPMode.DSB:
					case DSPMode.AM:
					case DSPMode.SAM:
					case DSPMode.FMN:
						low = -2600; high = 2600;
						break;
					default:
						low = -2600; high = 2600;
						break;
				}
					break;
				case Filter.F2100:
					radFilter2100.BackColor = button_selected_color;
					grpFilter.Text = "Filter - 2.1kHz";
				switch(current_dsp_mode)
				{
					case DSPMode.LSB:
						low = -2300; high = -200;
						break;
					case DSPMode.DIGL:
						FilterFreq = 2100;
						if(bFSK)
						{
							low = -(FSKfreq + (FilterFreq/2));
							high = -(FSKfreq - (FilterFreq/2));
						}
						else {low = -FilterFreq; high = -100;}
						break;
					case DSPMode.USB:
					case DSPMode.DIGU:
						low = 200; high = 2300;
						break;
					case DSPMode.CWL:
						high = Math.Min(-cw_pitch + 1050, -100);
						low = high - 2100;
						break;
					case DSPMode.CWU:
						low = Math.Max(cw_pitch - 1050, 100);
						high = low + 2100;
						break;
					case DSPMode.DSB:
					case DSPMode.AM:
					case DSPMode.SAM:
					case DSPMode.FMN:
						low = -2100; high = 2100;
						break;
					default:
						low = -2100; high = 2100;
						break;
				}
					break;
				case Filter.F1000:
					radFilter1000.BackColor = button_selected_color;
					grpFilter.Text = "Filter - 1.0kHz";
				switch(current_dsp_mode)
				{
					case DSPMode.LSB:
						low = -1200; high = -200;
						break;
					case DSPMode.DIGL:
						FilterFreq = 1000;
						if(bFSK)
						{
							low = -(FSKfreq + (FilterFreq/2));
							high = -(FSKfreq - (FilterFreq/2));
						}
						else {low = -FilterFreq; high = -100;}
						break;
					case DSPMode.USB:
					case DSPMode.DIGU:
						low = 200;
						high = 1200;
						break;
					case DSPMode.CWL:
						high = Math.Min(-cw_pitch + 500, -100);
						low = high - 1000;
						break;
					case DSPMode.CWU:
						low = Math.Max(cw_pitch - 500, 100);
						high = low + 1000;
						break;
					case DSPMode.DSB:
					case DSPMode.AM:
					case DSPMode.SAM:
					case DSPMode.FMN:
						low = -1000; high = 1000;
						break;
					default:
						low = -1000; high = 1000;
						break;
				}
					break;
				case Filter.F500:
					radFilter500.BackColor = button_selected_color;
					grpFilter.Text = "Filter - 500Hz";
				switch(current_dsp_mode)
				{
					case DSPMode.LSB:
						low = -500; high = 0;
						break;
					case DSPMode.DIGL:
						FilterFreq = 500;
						if(bFSK)
						{
							low = -(FSKfreq + (FilterFreq/2));
							high = -(FSKfreq - (FilterFreq/2));
						}
						else {low = -FilterFreq; high = -100;}
						break;
					case DSPMode.USB:
					case DSPMode.DIGU:
						low = 350; high = 850;
						break;
					case DSPMode.CWL:
						high = Math.Min(-cw_pitch + 250, -100);
						low = high - 500;
						break;
					case DSPMode.CWU:
						low = Math.Max(cw_pitch - 250, 100);
						high = low + 500;
						break;
					case DSPMode.DSB:
					case DSPMode.AM:
					case DSPMode.SAM:
					case DSPMode.FMN:
						low = -500; high = 500;
						break;
					default:
						low = -500; high = 500;
						break;
				}
					break;
				case Filter.F250:
					radFilter250.BackColor = button_selected_color;
					grpFilter.Text = "Filter - 250Hz";
				switch(current_dsp_mode)
				{
					case DSPMode.LSB:
						low = -250; high = 0;
						break;
					case DSPMode.DIGL:
						FilterFreq = 250;
						if(bFSK)
						{
							low = -(FSKfreq + (FilterFreq/2));
							high = -(FSKfreq - (FilterFreq/2));
						}
						else {low = -FilterFreq; high = -100;}
						break;
					case DSPMode.USB:
					case DSPMode.DIGU:
						low = 475; high = 725;
						break;
					case DSPMode.CWL:
						high = Math.Min(-cw_pitch + 125, -100);
						low = high - 250;
						break;
					case DSPMode.CWU:
						low = Math.Max(cw_pitch - 125, 100);
						high = low + 250;
						break;
					case DSPMode.DSB:
					case DSPMode.AM:
					case DSPMode.SAM:
					case DSPMode.FMN:
						low = -250; high = 250;
						break;
					default:
						low = -250; high = 250;
						break;
				}
					break;
				case Filter.F100:
					radFilter100.BackColor = button_selected_color;
					grpFilter.Text = "Filter - 100Hz";
					switch(current_dsp_mode)
					{
						case DSPMode.LSB:
						case DSPMode.DIGL:
							low = -650; high = -550;
							break;
						case DSPMode.USB:
						case DSPMode.DIGU:
							low = 550; high = 650;
							break;
						case DSPMode.CWL:
							high = Math.Min(-cw_pitch + 50, -100);
							low = high - 100;
							break;
						case DSPMode.CWU:
							low = Math.Max(cw_pitch - 50, 100);
							high = low + 100;
							break;
						case DSPMode.DSB:
						case DSPMode.AM:
						case DSPMode.SAM:
						case DSPMode.FMN:
							low = -100; high = 100;
							break;
						default:
							low = -100; high = 100;
							break;
					}
					break;
				case Filter.F50:
					radFilter50.BackColor = button_selected_color;
					grpFilter.Text = "Filter - 50Hz";
					switch(current_dsp_mode)
					{
						case DSPMode.LSB:
						case DSPMode.DIGL:
							low = -625; high = -575;
							break;
						case DSPMode.USB:
						case DSPMode.DIGU:
							low = 575; high = 625;
							break;
						case DSPMode.CWL:
							high = Math.Min(-cw_pitch + 25, -100);
							low = high - 50;
							break;
						case DSPMode.CWU:
							low = Math.Max(cw_pitch - 25, 100);
							high = low + 50;
							break;
						case DSPMode.DSB:
						case DSPMode.AM:
						case DSPMode.SAM:
						case DSPMode.FMN:
							low = -50; high = 50;
							break;
						default:
							low = -50; high = 50;
							break;
					}
					break;
				case Filter.F25:
					radFilter25.BackColor = button_selected_color;
					grpFilter.Text = "Filter - 25Hz";
					switch(current_dsp_mode)
					{
						case DSPMode.LSB:
						case DSPMode.DIGL:
							low = -613; high = -587;
							break;
						case DSPMode.USB:
						case DSPMode.DIGU:
							low = 587; high = 613;
							break;
						case DSPMode.CWL:
							high = Math.Min(-cw_pitch + 13, -100);
							low = high - 25;
							break;
						case DSPMode.CWU:
							low = Math.Max(cw_pitch - 13, 100);
							high = low + 25;
							break;
						case DSPMode.DSB:
						case DSPMode.AM:
						case DSPMode.SAM:
						case DSPMode.FMN:
							low = -25; high = 25;
							break;
						default:
							low = -25; high = 25;
							break;
					}
					break;
				case Filter.VAR1:
					radFilterVar1.BackColor = button_selected_color;
					udFilterLow.BackColor = button_selected_color;
					udFilterHigh.BackColor = button_selected_color;
					grpFilter.Text = "Filter - Var1";
					udFilterLow.Enabled = udFilterHigh.Enabled = false;
					udFilterLow.Value = low = var1_low[(int)current_dsp_mode];
					udFilterHigh.Value = high = var1_high[(int)current_dsp_mode];		
					udFilterLow.Enabled = udFilterHigh.Enabled = true;
					DttSP.SetRXFilters(low, high);
					break;
				case Filter.VAR2:
					radFilterVar2.BackColor = button_selected_color;
					udFilterLow.BackColor = button_selected_color;
					udFilterHigh.BackColor = button_selected_color;
					grpFilter.Text = "Filter - Var2";
					udFilterLow.Enabled = udFilterHigh.Enabled = false;
					udFilterLow.Value = low = var2_low[(int)current_dsp_mode];
					udFilterHigh.Value = high = var2_high[(int)current_dsp_mode];	
					udFilterLow.Enabled = udFilterHigh.Enabled = true;
					DttSP.SetRXFilters(low, high);
					break;
			}

			bool reset_avg = false;

			if(new_filter != Filter.VAR1 &&
				new_filter != Filter.VAR2 &&
				new_filter != Filter.NONE)
			{
				int rx_display_low = DttSP.RXDisplayLow;
				int rx_display_high = DttSP.RXDisplayHigh;
				
				DttSP.SetRXFilters(low, high);

				// if either display value changed, set a flag to reset the avg
				if(rx_display_high != DttSP.RXDisplayLow ||
					rx_display_high != DttSP.RXDisplayHigh)
					reset_avg = true;

				udFilterLow.Value = low;
				udFilterHigh.Value = high;
			}

			tbFilterWidth_FilterWidthChanged(low, high); // wjt added -- tell the width control to adjust

			pre_shift_filter_low = low;
			pre_shift_filter_high = high;

			FilterShiftValue = saved_filter_shift;
			tbFilterShift_Scroll(this, EventArgs.Empty);

			Display.DrawBackground();

			switch(Display.CurrentDisplayMode)
			{
				case DisplayMode.SPECTRUM:
				case DisplayMode.HISTOGRAM:
				case DisplayMode.WATERFALL:
					if(chkDisplayAVG.Checked && reset_avg)
						Display.ResetDisplayAverage();
					if(chkDisplayPeak.Checked && reset_avg)
						Display.ResetDisplayPeak();
					break;
			}

			if((high - low) > 250)
			{
				udXIT.Increment = 10;
				udRIT.Increment = 10;
			}
			else
			{
				udXIT.Increment = 5;
				udRIT.Increment = 5;
			}
		}

		private void radFilter6000_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radFilter6000.Checked)
			{
				SetFilter(Filter.F6000);
			}
		}

		private void radFilter4000_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radFilter4000.Checked)
				SetFilter(Filter.F4000);
		}

		private void radFilter2600_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radFilter2600.Checked)
				SetFilter(Filter.F2600);
		}

		private void radFilter2100_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radFilter2100.Checked)
				SetFilter(Filter.F2100);
		}

		private void radFilter1000_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radFilter1000.Checked)
				SetFilter(Filter.F1000);
		}

		private void radFilter500_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radFilter500.Checked)
				SetFilter(Filter.F500);
		}

		private void radFilter250_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radFilter250.Checked)
				SetFilter(Filter.F250);
		}

		private void radFilter100_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radFilter100.Checked)
				SetFilter(Filter.F100);
		}

		private void radFilter50_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radFilter50.Checked)
				SetFilter(Filter.F50);
		}

		private void radFilter25_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radFilter25.Checked)
				SetFilter(Filter.F25);
		}

		private void radFilterVar1_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radFilterVar1.Checked)
				SetFilter(Filter.VAR1);
		}

		private void radFilterVar2_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radFilterVar2.Checked)
				SetFilter(Filter.VAR2);
		}

		private void udFilterLow_ValueChanged(object sender, System.EventArgs e)
		{
			if(udFilterLow.Enabled)
			{
				if(udFilterLow.Focused)
				{
					if(tbFilterShift.Value != 0)
					{
						tbFilterShift.Value = 0;
						btnFilterShiftReset.BackColor = SystemColors.Control;
					}
				}

				if(udFilterLow.Value >= udFilterHigh.Value-10)
				{
					udFilterLow.Value = udFilterHigh.Value-10;
					return;
				}

				int i = (int)current_dsp_mode;
				if(current_filter == Filter.VAR1)	// save variable filter values
				{
					if(tbFilterShift.Value != 0)
					{
						var1_low[i] = pre_shift_filter_low;
						var1_high[i] = pre_shift_filter_high;
					}
					else
					{
						var1_low[i] = (int)udFilterLow.Value;
						var1_high[i] = (int)udFilterHigh.Value;
					}
				}
				else if(current_filter == Filter.VAR2)
				{
					if(tbFilterShift.Value != 0)
					{
						var2_low[i] = pre_shift_filter_low;
						var2_high[i] = pre_shift_filter_high;
					}
					else
					{
						var2_low[i] = (int)udFilterLow.Value;
						var2_high[i] = (int)udFilterHigh.Value;
					}
				}
				DttSP.SetRXFilters((int)udFilterLow.Value, (int)udFilterHigh.Value);
				Display.DrawBackground();

				switch(Display.CurrentDisplayMode)
				{
					case DisplayMode.SPECTRUM:
					case DisplayMode.HISTOGRAM:
					case DisplayMode.WATERFALL:
						if(chkDisplayAVG.Checked)
							Display.ResetDisplayAverage();
						if(chkDisplayPeak.Checked)
							Display.ResetDisplayPeak();
						break;
				}
				
				tbFilterWidth_FilterWidthChanged((int)udFilterLow.Value, (int)udFilterHigh.Value);
			}
			if(udFilterLow.Focused)
				btnHidden.Focus();
		}

		private void udFilterHigh_ValueChanged(object sender, System.EventArgs e)
		{
			if(udFilterHigh.Enabled)
			{
				if(udFilterHigh.Focused)
				{
					if(tbFilterShift.Value != 0)
					{
						tbFilterShift.Value = 0;
						btnFilterShiftReset.BackColor = SystemColors.Control;
					}
				}

				if(udFilterHigh.Value <= udFilterLow.Value+10)
				{
					udFilterHigh.Value = udFilterLow.Value+10;
					return;
				}

				int i = (int)current_dsp_mode;
				if(current_filter == Filter.VAR1)	// save variable filter values
				{
					if(tbFilterShift.Value != 0)
					{
						var1_low[i] = pre_shift_filter_low;
						var1_high[i] = pre_shift_filter_high;
					}
					else
					{
						var1_low[i] = (int)udFilterLow.Value;
						var1_high[i] = (int)udFilterHigh.Value;
					}
				}
				else if(current_filter == Filter.VAR2)
				{
					if(tbFilterShift.Value != 0)
					{
						var2_low[i] = pre_shift_filter_low;
						var2_high[i] = pre_shift_filter_high;
					}
					else
					{
						var2_low[i] = (int)udFilterLow.Value;
						var2_high[i] = (int)udFilterHigh.Value;
					}
				}
				DttSP.SetRXFilters((int)udFilterLow.Value, (int)udFilterHigh.Value);
				Display.DrawBackground();
					
				switch(Display.CurrentDisplayMode)
				{
					case DisplayMode.SPECTRUM:
					case DisplayMode.HISTOGRAM:
					case DisplayMode.WATERFALL:
						if(chkDisplayAVG.Checked)
							Display.ResetDisplayAverage();
						if(chkDisplayPeak.Checked)
							Display.ResetDisplayPeak();
						break;
				}

				tbFilterWidth_FilterWidthChanged((int)udFilterLow.Value, (int)udFilterHigh.Value);
			}
			if(udFilterHigh.Focused)
				btnHidden.Focus();
		}

		private void DoFilterShift(int shift, bool redraw)
		{
			// VK6APH: Does the Filter Shift function, alters the filter low and high frequency values 
			// as the Filter Shift slider is moved. We need to keep the last Filter Shift values
			// that the variable filters use since, unlike the other filters, there are 
			// no pre-set bandwidths that they can default to when the Filter Shift is 
			// turned off. These values are stored in the public variables last_var1_shift and
			// last_var2_shift. 
			int IFShift;
			int low;
			int high;
			int bandwidth;
			int max_shift = 9999;		// needed when using variable filters so we can't exceed +/- 10kHz DSP limits

			if(current_dsp_mode == DSPMode.SPEC ||
				current_dsp_mode == DSPMode.DRM)
				return;

			bandwidth = (int)Math.Abs(udFilterHigh.Value - udFilterLow.Value); // calculate current filter bandwidth 

			// set the maximum IF Shift depending on filter bandwidth in use 
			if (bandwidth > 800)
			{
				tbFilterShift.Maximum = 1000;  // max IF Shift +/- 1kHz for filters > 800Hz wide
				tbFilterShift.Minimum = -1000;
			}
			else 
			{
				tbFilterShift.Maximum = 500;	// max IF Shift +/- 500Hz for filters < 800Hz wide
				tbFilterShift.Minimum = -500;	
			}
			// calculate how far the IF Shift slider has moved
			// if we are using variable bandwidth filters need to use their last shift value
			if (current_filter == Filter.VAR1)
				IFShift = shift - last_var1_shift;
			else if (current_filter == Filter.VAR2)
				IFShift = shift - last_var2_shift;
			else
				IFShift = shift - last_filter_shift; 

			high = (int)Math.Min(udFilterHigh.Value + IFShift, max_shift);	// limit high shift to maximum value
			low = (int)Math.Max(udFilterLow.Value + IFShift, -max_shift);	// limit low shift to maximum value
			
			DttSP.SetRXFilters(low, high);					// select new filters
			udFilterLow.Value = low;						// display new low value 
			udFilterHigh.Value = high;						// display new high value
			if(redraw) Display.DrawBackground();			// draw new background for updated filter values
	
			// store the last IF Shift applied for use next time
			if (current_filter == Filter.VAR1)
				last_var1_shift = last_var1_shift + IFShift;
			else if (current_filter == Filter.VAR2)
				last_var2_shift = last_var2_shift + IFShift;
			else 
				last_filter_shift = last_filter_shift + IFShift;				
			// show the IF Shift is active by setting the zero button colour
			if (shift != 0)
				btnFilterShiftReset.BackColor = button_selected_color;		
		}

		private void tbFilterShift_Scroll(object sender, System.EventArgs e)
		{
			DoFilterShift(tbFilterShift.Value, true);

			if(tbFilterShift.Focused)
				btnHidden.Focus();
		}

		private void btnFilterShiftReset_Click(object sender, System.EventArgs e)
		{
			tbFilterShift.Value = 0;
			btnFilterShiftReset.BackColor = SystemColors.Control;	// make button grey
			tbFilterShift_Scroll(this, EventArgs.Empty); 
		}

		private int centerSave = 0;  // remember the nominal center point 
		private void tbFilterWidth_Scroll(object sender, System.EventArgs e)
		{
			if ( current_dsp_mode == DSPMode.DRM || current_dsp_mode == DSPMode.SPEC ) 
			{
				return;  // no good in this mode 
			}

			SelectVarFilter(); // make filter variable 

			// Debug.WriteLine("tbWidth_Scroll!");   
			// Debug.WriteLine("fhi: " + udFilterHigh.Value + " flo: " + udFilterLow.Value);  	
			
			// select filter width with a logarathmic scale 
			// 
			int width_scroll = 10001 - tbFilterWidth.Value;  // invert sense

			double log_width_scroll = Math.Log((double)width_scroll); 
			// scroller ranges 1-10000, so log ranges from 0 to 4 
			// full width is 10khz ... so width is log(scroller)/4 * 10 khz 
			// double width_d = 10001 - ((log_width_scroll/4.0)*(10000)); 
			double width_d = 10001 - ((log_width_scroll/9.2103)*(10000)); 
			int width = (int)width_d; 
			if ( width < 10) width = 10; // minimum width 
			else if ( width > 9999 ) width = 9999;  // maximum width 

			int center = 0; 
			bool side_band_style_mode = false; 
			int h = (int)udFilterHigh.Value; 
			int l = (int)udFilterLow.Value; 
			switch ( current_dsp_mode ) 
			{ 
				case DSPMode.CWL:  // sideband style modes 
				case DSPMode.LSB:
				case DSPMode.USB:
				case DSPMode.CWU:
				case DSPMode.DIGL:
				case DSPMode.DIGU:
					side_band_style_mode = true; 

					if ( current_dsp_mode == DSPMode.LSB || current_dsp_mode == DSPMode.CWL 
						|| current_dsp_mode == DSPMode.DIGL) 
					{ 
						int save_int = l; 
						l = -h; 
						h = -save_int; 						
					} 
					center = ((h - l)/2) + l; 
					break; 

				case DSPMode.AM: 
				case DSPMode.SAM: 
				case DSPMode.DSB: 
				case DSPMode.FMN:
					center = (h-l)/2 + l;
					break; 
			} 
		
			int low = center - width/2; 
			int high = center + width/2;

			if ( side_band_style_mode ) 
			{ 
				if ( low < 0 )  // low can't be less than zero -- adjust 				
				{ 			
					if ( (udFilterHigh.Value > 0 && udFilterLow.Value < 0) || 
						(udFilterHigh.Value < 0 && udFilterLow.Value > 0) )
					{
						// filter is already spanning dc (prob from of shfit), don't cap the bottom @ dc 
						centerSave = 0; 
					}
					else 
					{ 
						if ( centerSave == 0 ) 
						{	
							//Debug.WriteLine("setting centerSave: " + center); 
							centerSave = center; // remember the prior center 
						}
						int too_low = - low; 
						high = high + too_low; 
						low = 0; 
					} 
				}
				else if ( centerSave != 0 )  // we'd previously shifted the center ... see if we can shift back to it 
				{
					int new_low; 
					int new_hi; 
					new_low = centerSave - width/2; 
					new_hi = centerSave + width/2; 
					if ( new_low >= 0 )  // ok, we can go with this one as it fits 
					{ 
						//Debug.WriteLine("resotring prev center of: " + centerSave); 
						centerSave = 0; 
						low = new_low; 
						high = new_hi; 
					} 
					else  // adjust it keeping 
					{ 
						//Debug.WriteLine("not back to centerSave yet!"); 
						int too_low = - new_low; 
						low = 0; 
						high = new_hi + too_low; 					
					} 
		
				}

				if ( high - low <= 11 ) 
				{ 
					high = high + ( 11 - (high - low)) ; 
				} 
				

				if ( current_dsp_mode == DSPMode.LSB ||  // mirror image things for lower sideband
					current_dsp_mode == DSPMode.CWL  ||
					current_dsp_mode == DSPMode.DIGL) 
				{
					int save_int = low; 
					low	= -high; 
					high = -save_int; 					
				}				
			}
			else  // symetric style mode 
			{ 
				low = center -  width; 
				high = center + width; 
			}
			
			//Debug.WriteLine("low: " + low + " high: " + high + " center: " + center + " width: " + width + " lws: " + log_width_scroll); 

			if ( high > 9999 ) 
			{ 
				high = 9999; 
			} 
			if ( low < -9999 ) 
			{ 
				low = -9999; 
			} 

			DttSP.SetRXFilters(low, high);					// select new filters
			udFilterLow.Value = low;						// display new low value 
			udFilterHigh.Value = high;						// display new high value
			Display.DrawBackground();						// draw new background for updated filter values

			if(tbFilterWidth.Focused)
				btnHidden.Focus();
		}

		private void tbFilterWidthScroll_newMode() 
		{
			centerSave = 0;  // dump any save center with scroller is keeping 
			switch ( current_dsp_mode ) 
			{
				case DSPMode.SPEC: 
				case DSPMode.DRM: 
					tbFilterWidth.Enabled = false; 
					break; 

				default: 
					tbFilterWidth.Enabled = true; 
					break; 
			}
		}

		// kd5tfd -- called from change filter to make the width control display the 
		// current filter width 
		// 
		private void tbFilterWidth_FilterWidthChanged(int low, int high) 
		{ 						
			//Debug.WriteLine("fhi: " + udFilterHigh.Value + " flo: " + udFilterLow.Value);  	
			//Debug.WriteLine("fwc hi: " + high + " lo: " + low);  	
			// int width = (int)(udFilterHigh.Value = udFilterLow.Value); 

			int width; 
			if ( current_dsp_mode == DSPMode.AM || current_dsp_mode == DSPMode.SAM || 
				current_dsp_mode == DSPMode.FMN || current_dsp_mode == DSPMode.DSB ) 
				// symetric width centered mode 
			{
				width = ( high - low ) / 2 ; 				
			}
			else 
			{ 
				width = high - low; 
			}

			if ( width < 0 ) width = - width; 

			// convert to log scale the control uses 
			double width_d = (double)width; 

			double ws_d; // double of value to feed scroller 		
			ws_d = Math.Exp((width_d - 10001.0)*(-9.2103/10000));
			int ws = (int)ws_d; 	
			ws = 10001 - ws;  // reverse sense 
			// ws = ws / 10; // scale from original 10k control 
			
			
			// make sure we don't hose the control 
			if ( ws < tbFilterWidth.Minimum ) 
			{ 
				ws = tbFilterWidth.Minimum; 
				//Debug.WriteLine("filter width min limited"); 
			}
			else if	( ws > tbFilterWidth.Maximum ) 
			{ 
				ws = tbFilterWidth.Maximum; 
				//Debug.WriteLine("filter width max limited"); 
			} 
			tbFilterWidth.Value = ws; 
			//Debug.WriteLine("fwc width: " + width + "  ws: " + ws); 
			// centerSave = 0; // reset our save center point 
		}

		#endregion

		#region VFO Button Events
// ======================================================
// VFO Button Events
// ======================================================

		// Added 6/20/05 BT for CAT commands
		public void CATVFOSwap(string pChangec)
		{
			string c = pChangec;
			if(c.Length > 0)
			{
				switch(c)
				{
					case "0":
						btnVFOAtoB_Click(btnVFOAtoB,EventArgs.Empty);
						break;
					case "1":
						btnVFOBtoA_Click(btnVFOBtoA,EventArgs.Empty);
						break;
					case "2":
						btnVFOSwap_Click(btnVFOSwap,EventArgs.Empty);
						break;
				}
			}
		}


		private void btnVFOAtoB_Click(object sender, System.EventArgs e)
		{
			txtVFOBFreq.Text = txtVFOAFreq.Text;
			txtVFOBFreq_LostFocus(this, EventArgs.Empty);
			vfob_dsp_mode = current_dsp_mode;
			vfob_filter = current_filter;
		}

		private void btnVFOBtoA_Click(object sender, System.EventArgs e)
		{
			CurrentDSPMode = vfob_dsp_mode;
			CurrentFilter = vfob_filter;
			txtVFOAFreq.Text = txtVFOBFreq.Text;
			txtVFOAFreq_LostFocus(this, EventArgs.Empty);
		}

		private void btnVFOSwap_Click(object sender, System.EventArgs e)
		{
			string temp = txtVFOAFreq.Text;

			DSPMode mode = current_dsp_mode;
			Filter filter = current_filter;

			CurrentDSPMode = vfob_dsp_mode;
			CurrentFilter = vfob_filter;
			vfob_dsp_mode = mode;
			vfob_filter = filter;
			
			txtVFOAFreq.Text = txtVFOBFreq.Text;
			txtVFOBFreq.Text = temp;
			txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			txtVFOBFreq_LostFocus(this, EventArgs.Empty);
		}

		private void chkVFOSplit_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkVFOSplit.Checked)
			{
				XITOn = false;
				chkVFOSplit.BackColor = button_selected_color;
				grpVFOB.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
				grpVFOB.ForeColor = Color.Red;
				txtVFOBFreq.ForeColor = Color.Red;
				txtVFOBBand.ForeColor = band_text_light_color;
			}
			else
			{
				chkVFOSplit.BackColor = SystemColors.Control;
				grpVFOB.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);
				grpVFOB.ForeColor = Color.Black;
				txtVFOBFreq.ForeColor = vfo_text_dark_color;
				txtVFOBBand.ForeColor = band_text_dark_color;
			}
			// BT 04/2005
			// raises the serial io event
			// EventArgs sioe = EventArgs.Empty;
			// OnNotifySIO(sioe);
		}

		private void chkXIT_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkXIT.Checked)
			{
				chkXIT.BackColor = button_selected_color;
				chkVFOSplit.Checked = false;
				chkRIT.Checked = false;
			}
			else
			{
				chkXIT.BackColor = SystemColors.Control;
			}

			if(chkMOX.Checked)
			{
				if(chkVFOSplit.Checked)
					txtVFOBFreq_LostFocus(this, EventArgs.Empty);
				else
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}
		}

		private void chkRIT_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkRIT.Checked)
			{
				chkRIT.BackColor = button_selected_color;
				XITOn = false;
			}
			else
				chkRIT.BackColor = SystemColors.Control;

			txtVFOAFreq_LostFocus(this, EventArgs.Empty);
		}

		private void udRIT_ValueChanged(object sender, System.EventArgs e)
		{
			if(chkRIT.Checked && !chkMOX.Checked)
				txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			if(udRIT.Focused)
				btnHidden.Focus();
		}

		private void udXIT_ValueChanged(object sender, System.EventArgs e)
		{
			if(chkXIT.Checked && chkMOX.Checked)
			{
				if(chkVFOSplit.Checked)
					txtVFOBFreq_LostFocus(this, EventArgs.Empty);
				else
					txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			}

			if(udXIT.Focused)
				btnHidden.Focus();
		}

		private void btnXITReset_Click(object sender, System.EventArgs e)
		{
			udXIT.Value = 0;
		}

		private void btnRITReset_Click(object sender, System.EventArgs e)
		{
			udRIT.Value = 0;
		}
				
		private void btnZeroBeat_Click(object sender, System.EventArgs e)
		{
			int peak_hz = FindPeakFreqInPassband();
			if ( peak_hz == -1 )
			{
				return; // find peak croaked - bail
			}
			// Debug.WriteLine("peak: " + peak_hz);
			int delta_hz = 0;
			
			// if we're in CW mode, zero beat to CWPitch, provided it is in the passband
			if ( current_dsp_mode == DSPMode.CWL || 
				current_dsp_mode == DSPMode.CWU )
			{
				int local_pitch = CWPitch;
				if ( current_dsp_mode == DSPMode.CWL )
				{
					local_pitch = - local_pitch;
				}
				// is cwoffset in passband?
				if ( local_pitch >= udFilterLow.Value && 
					local_pitch <= udFilterHigh.Value )
				{
					delta_hz = local_pitch - peak_hz;
				}
				// Debug.WriteLine("delta(cw): " + delta_hz);
			}

			// if we get here and delta_hz is still 0, we're either not in
			// CW mode or the current CW pitch is not within the passband.
			// Put strongest peak @ center of passband
			if ( delta_hz == 0 )
			{
				int center_hz = (int)(((udFilterHigh.Value 
					- udFilterLow.Value)/2) + udFilterLow.Value);
				delta_hz = center_hz - peak_hz;
			}

//          Debug.WriteLine("peak: " + peak_hz);
//          Debug.WriteLine("center: " + center_hz);
//          Debug.WriteLine("delta: " + delta_hz + "\n");

			VFOAFreq -= (double)delta_hz/1e6;
		}

		unsafe private int FindPeakFreqInPassband()
		{
			// convert hz to buckets in the averaging data
			int lo_cut_hz = (int)udFilterLow.Value;
			int hi_cut_hz = (int)udFilterHigh.Value;
			double hz_per_bucket = DttSP.SampleRate/Display.BUFFER_SIZE;
			int zero_hz_bucket = Display.BUFFER_SIZE/2;
			int lo_bucket = (int)(((double)lo_cut_hz/hz_per_bucket)) + zero_hz_bucket;
			int hi_bucket = (int)(((double)hi_cut_hz/hz_per_bucket)) + zero_hz_bucket;

			//~~~~ 
			float max_val = float.MinValue;
			int max_bucket = 0;

			float[] spectrum_data; 

			// reuse the existing display data if there is any
			switch ( Display.CurrentDisplayMode ) 
			{
				case DisplayMode.PANADAPTER:
				case DisplayMode.HISTOGRAM:
				case DisplayMode.SPECTRUM:
				case DisplayMode.WATERFALL:
					if ( chkDisplayAVG.Checked ) 
					{
						spectrum_data = Display.average_buffer; 
					}
					else 
					{
						spectrum_data = Display.current_display_data; 
					}
					break; 

					// no specturm data - go get some 
				default: 
					spectrum_data = new float[Display.BUFFER_SIZE];
					if ( spectrum_data == null ) 
					{ 
						return -1; // bail out - not buffer 
					} 
					fixed(float *ptr = &(spectrum_data[0]))
						DttSP.GetSpectrum(ptr);
					break; 										
			}


			for ( int i = lo_bucket; i <= hi_bucket; i++ )
			{
				if ( spectrum_data[i] > max_val )
				{
					max_bucket = i;
					max_val = spectrum_data[i];
				}
			}
			int peak_hz = (int)(((double)(max_bucket - zero_hz_bucket)) * hz_per_bucket);
			return peak_hz;
		}

		private void btnIFtoVFO_Click(object sender, System.EventArgs e)
		{
			int current_if_shift;

			bool is_centered_mode = false;
			bool is_cw_mode = false;
			bool is_lower_sb_mode = false;

			switch ( CurrentFilter )
			{
				case Filter.VAR1:
					current_if_shift = last_var1_shift;
					break;
				case Filter.VAR2:
					current_if_shift = last_var2_shift;
					break;
				default:
					current_if_shift = last_filter_shift;
					break;
			}

//			Debug.WriteLine("current if shift: " + current_if_shift);

			if ( current_if_shift == 0 ) return; // nothing to do

			switch ( CurrentDSPMode )
			{
				case DSPMode.DRM:
				case DSPMode.SPEC:
				case DSPMode.DIGL:
				case DSPMode.DIGU:
					return; // nothing to do for these modes

				case DSPMode.AM:
				case DSPMode.FMN:
				case DSPMode.DSB:
				case DSPMode.SAM:
					is_centered_mode = true;
					break;

				case DSPMode.CWL:
					is_cw_mode = true;
					is_lower_sb_mode = true;
					break;
				case DSPMode.CWU:
					is_cw_mode = true;
					break;
				case DSPMode.LSB:
					is_lower_sb_mode = true;
					break;
				case DSPMode.USB:
					break;
				default:
					// no clue what the mode is -- bail out
					return;
			}

			int current_width = (int)udFilterHigh.Value - (int)udFilterLow.Value;
			int current_center = (int)udFilterLow.Value + (current_width/2);
//			Debug.WriteLine("w: " + current_width + " center: " + current_center + " vfo: " +  VFOAFreq);

			double new_vfo = 0;
			int new_lo = 0;
			int new_hi = 0;

			if ( is_centered_mode )
			{
				new_vfo = VFOAFreq + ((double)current_center)/(1e6);
				new_lo = - (current_width/2);
				new_hi = current_width/2;
			}
			else  // sideband style mode
			{
				int new_center;
				if ( is_cw_mode )
				{
					new_center = CWPitch;
				}
				else  // sideband mode
				{
					new_center = 100 + ( current_width/2 );
				}
				if ( is_lower_sb_mode )
				{
					new_center = -new_center;
				}
				new_vfo = VFOAFreq + ((double)(current_center-new_center))/(1e6);
				// now figure out filter limits
				new_lo = new_center - ( current_width/2 );
				new_hi = new_center + ( current_width/2 );
			}
//			Debug.WriteLine("new vfo: " + new_vfo + " lo: " + new_lo + " hi: " + new_hi );
			if ( VFOAFreq > new_vfo  )  // need to change this in the right order!
			{
				udFilterHigh.Value = new_hi;
				udFilterLow.Value = new_lo;
			}
			else
			{
				udFilterLow.Value = new_lo;
				udFilterHigh.Value = new_hi;
			}

			VFOAFreq = new_vfo;
			centerSave = 0;
			switch ( CurrentFilter )
			{
				case Filter.VAR1:
					last_var1_shift = 0;
					break;
				case Filter.VAR2:
					last_var2_shift = 0;
					break;
				default: 
					last_filter_shift = 0; 
					break; 
			}
			btnFilterShiftReset_Click(this, EventArgs.Empty); 
		}

		#endregion

		#region DSP Button Events

		private void chkNR_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkNR.Checked)
			{
				chkNR.BackColor = button_selected_color;
				DttSP.SetNR(1);
				cat_nr_status = 1;
			}
			else
			{
				chkNR.BackColor = SystemColors.Control;
				DttSP.SetNR(0);
				cat_nr_status = 0;
			}
		}

		private void chkANF_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkANF.Checked)
			{
				chkANF.BackColor = button_selected_color;
				DttSP.SetANF(1);
				cat_anf_status = 1;
			}
			else
			{
				chkANF.BackColor = SystemColors.Control;
				DttSP.SetANF(0);
				cat_anf_status = 0;
			}				
		}

		private void chkNB_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkNB.Checked)
			{
				chkNB.BackColor = button_selected_color;
				DttSP.SetNB(1);
				cat_nb1_status = 1;
			}
			else
			{
				chkNB.BackColor = SystemColors.Control;
				DttSP.SetNB(0);
				cat_nb1_status = 0;
			}
		}

		private void chkDSPComp_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkDSPComp.Checked)
			{
				chkDSPComp.BackColor = button_selected_color;
				DttSP.SetTXAGCFF(1);
			}
			else
			{
				chkDSPComp.BackColor = SystemColors.Control;
				DttSP.SetTXAGCFF(0);
			}
		}

		private void chkDSPNB2_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkDSPNB2.Checked)
			{
				chkDSPNB2.BackColor = button_selected_color;
				DttSP.SetSDROM(1);
				cat_nb2_status = 1;
			}
			else
			{
				chkDSPNB2.BackColor = SystemColors.Control;
				DttSP.SetSDROM(0);
				cat_nb2_status = 0;
			}
		}

		private void chkDSPCompander_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkDSPCompander.Checked)
			{
				DttSP.SetTXCompandSt(1);
				chkDSPCompander.BackColor = button_selected_color;
				cat_cmpd_status = 1;
			}
			else
			{
				DttSP.SetTXCompandSt(0);
				chkDSPCompander.BackColor = SystemColors.Control;
				cat_cmpd_status = 0;
			}
		}

		#endregion

		#region Memory Events
// ======================================================
// Memory Events
// ======================================================

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			SaveMem save = new SaveMem(this);
			save.Show();
			save.Focus();
		}

		private void chkMemoryScanner_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkMemoryScanner.Checked)
			{
				chkMemoryScanner.BackColor = button_selected_color;
				radScannerChannel.Enabled = false;
				radScannerFreq.Enabled = false;

				udScannerDelay.BackColor = button_selected_color;

				if(radScannerFreq.Checked)
				{
					udScannerLow.BackColor = button_selected_color;
					udScannerHigh.BackColor = button_selected_color;
					udScannerStep.BackColor = button_selected_color;

					udScannerLow.Enabled = false;
					udScannerHigh.Enabled = false;
					udScannerStep.Enabled = false;

					VFOAFreq = (float)udScannerLow.Value;
				}
				else 
				{
					MemForm.ScanMemory(true);
				}

				chkMOX.Enabled = false;
								
				timer_scanner.Enabled = true;				
			}
			else
			{
				timer_scanner.Enabled = false;

				udScannerDelay.BackColor = SystemColors.Window;

				radScannerChannel.Enabled = true;
				radScannerFreq.Enabled = true;

				if(radScannerFreq.Checked)
				{
					udScannerLow.BackColor = Color.White;
					udScannerHigh.BackColor = Color.White;
					udScannerStep.BackColor = Color.White;

					udScannerLow.Enabled = true;
					udScannerHigh.Enabled = true;
					udScannerStep.Enabled = true;
				}

				chkMOX.Enabled = true;
				radScannerChannel.Enabled = true;
				radScannerFreq.Enabled = true;
				chkMemoryScanner.BackColor = SystemColors.Control;
			}
		}

		private void udScannerDelay_ValueChanged(object sender, System.EventArgs e)
		{
			timer_scanner.Interval = (int)udScannerDelay.Value;
			if(udScannerDelay.Focused)
				btnHidden.Focus();
		}

		private void btnMemoryQuickSave_Click(object sender, System.EventArgs e)
		{
			txtMemoryQuick.Text = txtVFOAFreq.Text;
			quick_save_mode = CurrentDSPMode;
			quick_save_filter = CurrentFilter;
		}

		private void btnMemoryQuickRestore_Click(object sender, System.EventArgs e)
		{
			CurrentDSPMode = quick_save_mode;
			txtVFOAFreq.Text = txtMemoryQuick.Text;
			txtVFOAFreq_LostFocus(this, EventArgs.Empty);
			CurrentFilter = quick_save_filter;
		}

		private void udScannerLow_ValueChanged(object sender, System.EventArgs e)
		{
			if(udScannerLow.Focused)
				btnHidden.Focus();
		}

		private void udScannerHigh_ValueChanged(object sender, System.EventArgs e)
		{
			if(udScannerHigh.Focused)
				btnHidden.Focus();
		}

		private void udScannerStep_ValueChanged(object sender, System.EventArgs e)
		{
			if(udScannerStep.Focused)
				btnHidden.Focus();
		}

		#endregion

		#region Menu Events

		private void menu_setup_Click(object sender, System.EventArgs e)
		{
			if(SetupForm.IsDisposed)
				SetupForm = new Setup(this);
			SetupForm.Show();
			SetupForm.Focus();
		}

		private void menu_wave_Click(object sender, System.EventArgs e)
		{
			if(WaveForm.IsDisposed)
				WaveForm = new WaveControl(this);
			WaveForm.Show();
			WaveForm.Focus();
		}

		private void menu_cw_Click(object sender, System.EventArgs e)
		{
			if(current_dsp_mode == DSPMode.LSB)
				CurrentDSPMode = DSPMode.CWL;
			else if(current_dsp_mode == DSPMode.USB)
				CurrentDSPMode = DSPMode.CWU;

			if(current_dsp_mode != DSPMode.CWL &&
				current_dsp_mode != DSPMode.CWU)
			{
				MessageBox.Show("The radio must be in CWL or CWU mode in order to open the "+
					"CW Control Form.",
					"CW Error: Wrong Mode",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}

			cw_key_mode = true;
			if(CWForm == null || CWForm.IsDisposed)
			{
				CWForm = new CW(this);
				CWForm.StartPosition = FormStartPosition.Manual;
				CWForm.GetCWOptions();				
			}

			CWForm.Show();
			CWForm.Focus();
		}

		private void mnuEQ_Click(object sender, System.EventArgs e)
		{
			if(EQForm == null || EQForm.IsDisposed)
				EQForm = new EQForm();
			EQForm.Show();
			EQForm.Focus();
		}

		private void mnuUCB_Click(object sender, System.EventArgs e)
		{
			if(UCBForm == null || UCBForm.IsDisposed)
				UCBForm = new UCBForm(this);
			UCBForm.Show();
			UCBForm.Focus();
		}

		private void mnuXVTR_Click(object sender, System.EventArgs e)
		{
			if(XVTRForm == null || XVTRForm.IsDisposed)
				XVTRForm = new XVTRForm(this);

			XVTRForm.Show();
			XVTRForm.Focus();
		}

		private void mnuCWX_Click(object sender, System.EventArgs e)
		{
			if(current_dsp_mode == DSPMode.LSB)
				CurrentDSPMode = DSPMode.CWL;
			else if(current_dsp_mode == DSPMode.USB)
				CurrentDSPMode = DSPMode.CWU;

			if(current_dsp_mode != DSPMode.CWL &&
				current_dsp_mode != DSPMode.CWU)
			{
				MessageBox.Show("The radio must be in CWL or CWU mode in order to open the "+
					"CWX Control Form.",
					"CWX Error: Wrong Mode",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}

			//	cw_key_mode = true;
			if(CWXForm == null || CWXForm.IsDisposed)
			{
				CWXForm = new CWX(this);
				CWXForm.StartPosition = FormStartPosition.Manual;
				CWXForm.RestoreSettings();
			}

			CWXForm.Show();
			CWXForm.Focus();
		}

		#endregion
	}
}
