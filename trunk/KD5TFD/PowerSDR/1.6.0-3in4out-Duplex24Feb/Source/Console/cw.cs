//=================================================================
// cw.cs
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
// Originally written in VB by Richard Allen W5SXD.
// Translated and modified by Eric Wachsmann KE5DTO.
// Optimizations added by Bob McGwier N4HY.
//==============================================================

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
	public class CW : System.Windows.Forms.Form
	{
		#region Variable Declaration

		private enum CWMode
		{
			AUTO_KEYBOARD = 0,
			MEMORY,
			EXT_KEY,
			MANUAL_KEYBOARD,
			MOUSE,
			JOYSTICK,
			KEYDOWN,			
		}

		private CWMode current_mode;
		private int current_buffer;
		private Console console;
		private const double PI = 3.14159265358979;
		private const double TWOPI = 6.28318530717959;    // 2 * Pi
		// the following is not Fs change friendly ...
		private const int NEDGE = 24000;			// enuf for a 500 ms edge
		private const double TWPM = 1.2;			// 60 seconds / length of 'PARIS '

		private double srate;						// sample rate in Hz
		private float[] edge;						// edge table
		//private int last_cw_pitch;				// last value for center freq
		private double tonephase;					// current tone phase
		private double tonedelta;					// current tone delta phase

		//public double wpm;						// speed in words/minute
		//public double tedge;						// edge time in seconds
		//public double weight;						// weight as a percent of element
		//public double level;						// output level

		private double tel;							// element time in seconds
		private int spel;							// element time in samples
		private int nbel;							// buffers/element for paddles
		private int spdot;							// total samples in a dot+space
		private int spdash;							// total samples in a dash+space
		private int spedge;							// samples in a tedge second edge
		private int ondot;							// weighted on time for a dot
		private int ondash;							// weighted on time for a dash
		private int spdotsp;						// samples in the space after a dot
		private int spdashsp;						// samples in the space after a dash

		private MSTATE current_mstate;				// current state of the char mode tone modulation state machine

		private enum MSTATE
		{
			MOP_UNINITIALISED = 0,					// at power up
			MOP_IDLE,								// cyclin' on station ...
			MOP_INCHAR,								// setup character element
			MOP_LEADING,							// in the leading edge
			MOP_KEYON,								// doing the key on
			MOP_TRAILING,							// doing the trailing edge
			MOP_SPACING,							// timing the space inside character
			MOP_XSPACE,								// timing the space outside character
			MOP_STANDBY,							// standby state
			MOP_END,								// exiting
		}

		private KSTATE current_kstate;				// current state of the paddle keyer state machine

		// define cases of kstate at the sample rate (the paddle keyer states)
		
		private enum KSTATE
		{
			KOP_UNINITIALISED = 0,					// at power up
			KOP_IDLE,								// cycling on station ...
			KOP_DOT,								// doing a dot
			KOP_DASH,								// doing a dash
			KOP_SPACE,								// doing a space
			KOP_END,								// exiting
		}

		private QSTATE current_qstate;				// current state of the keyer mode tone modulator

		//define cases of qstate at the sample rate
        
		private enum QSTATE
		{
			QOP_UNINITIALISED = 0,					// at power up
			QOP_IDLE,								// cyclin' on station ...
			QOP_LEADING,							// in the leading edge
			QOP_KEYON,								// doing the key on
			QOP_TRAILING,							// doing the trailing edge
			QOP_END,								// exiting
		}

		//private int mankey;						// last key from the keyboard
		private bool gogo;							// restart flag
		private int ndown;							// number of samples with key down in the block
		private int nn;								// decoded number of elements
        private int dd;								// decoded dashes
        private int wt;								// a weight factor whilst in keyer mode
		private bool isdot;							// dot side is pushed
		private bool isdash;						// dash side is pushed
		private bool keydown;						// keydown is checked
		private bool kkeydown;						// paddle keyer is keyed
		private bool merging;						// if we are merging codes
		private bool dashing;						// if we are doing a long dash
		private bool spacing;						// if we are doing a long space
		private bool msp;							// if we are exiting a merge
		private bool iambic;
		//private int savedwidth;						// form width on entry

		private string queue;						// message queue
		private int qin;							// current character in q
		private int nq;								// number of characters in the q

		private bool initial_dot;
		private bool initial_dash;
		private bool extkey_dot;
		private bool extkey_dash;
		private bool kb_dot;
		private bool kb_dash;
		private bool mouse_dot;
		private bool mouse_dash;
		private bool joy_dot;
		private bool joy_dash;

		// DoKeyer static variables
		private bool dokeyer_lastdot;				// last was a dot
		private int dokeyer_instate;				// cycles instate
		private bool dokeyer_dotmemory;				// the dot memory
		private bool dokeyer_dashmemory;			// the dash memory    

		// GetLevel static variables
		private double getlevel_s;					// current output level
		private int getlevel_instate;				// in state counter
		private int getlevel_e;						// current edge value during edges
		private bool getlevel_do_dot;				// current element will be a dot
		private int getlevel_dashes;				// dash register
		private int getlevel_nel;					// element counter
		private int getlevel_mask;					// shifting bits mask
		private string getlevel_cc;					// current character

		// GetKeyerLevel static variables
		private double getkeyerlevel_s;				// current output level
		private int getkeyerlevel_instate;			// in state counter
		private int getkeyerlevel_e;				// current edge value during edges

		private int pass;							// pass counter
		private int break_in_counter;

		private bool mon_before_cw;
		private bool manual_send;
		private bool freeze_input;
		private CWKeyer.Paddle joystick;

		private Thread pollkey_thread;

		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.GroupBoxTS grpMessageBuffers;
		private System.Windows.Forms.GroupBoxTS grpManualBuffer;
		private System.Windows.Forms.CheckBoxTS chkNoMonitor;
		private System.Windows.Forms.CheckBoxTS chkLoopBuffer;
		private System.Windows.Forms.TextBoxTS txtMemory5;
		private System.Windows.Forms.TextBoxTS txtMemory4;
		private System.Windows.Forms.TextBoxTS txtMemory3;
		private System.Windows.Forms.TextBoxTS txtMemory2;
		private System.Windows.Forms.TextBoxTS txtMemory1;
		private System.Windows.Forms.LabelTS lblEdge;
		private System.Windows.Forms.NumericUpDownTS udEdge;
		private System.Windows.Forms.LabelTS lblWeight;
		private System.Windows.Forms.NumericUpDownTS udWeight;
		private System.Windows.Forms.LabelTS lblWPM;
		private System.Windows.Forms.NumericUpDownTS udWPM;
		private System.Windows.Forms.Panel panelDot;
		private System.Windows.Forms.Panel panelDash;
		private System.Windows.Forms.LabelTS lblDot;
		private System.Windows.Forms.LabelTS lblDash;
		private System.Windows.Forms.RadioButtonTS radModeMouse;
		private System.Windows.Forms.RadioButtonTS radModeMemory;
		private System.Windows.Forms.TextBoxTS txtKeyboardBuffer;
		private System.Windows.Forms.GroupBoxTS grpOptions;
		private System.Windows.Forms.CheckBoxTS chkReversePaddles;
		private System.Windows.Forms.Panel panelDeedle;
		private System.Windows.Forms.LabelTS lblDeedle;
		private System.Windows.Forms.CheckBoxTS chkMemory4;
		private System.Windows.Forms.CheckBoxTS chkMemory5;
		private System.Windows.Forms.CheckBoxTS chkMemory3;
		private System.Windows.Forms.CheckBoxTS chkMemory2;
		private System.Windows.Forms.CheckBoxTS chkMemory1;
		private System.Windows.Forms.LabelTS lblSending;
		private System.Windows.Forms.LabelTS label1;
		private System.Windows.Forms.LabelTS label2;
		private System.Windows.Forms.LabelTS label3;
		private System.Windows.Forms.GroupBoxTS grpAutoMode;
		private System.Windows.Forms.RadioButtonTS radModeAutoKeyboard;
		private System.Windows.Forms.RadioButtonTS radModeExtKey;
		private System.Windows.Forms.GroupBoxTS grpManualMode;
		private System.Windows.Forms.RadioButtonTS radModeManualKeyboard;
		private System.Windows.Forms.GroupBoxTS grpSettings;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBoxTS chkKeyer;
		private System.Windows.Forms.CheckBoxTS chkSemiBreakIn;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.RadioButtonTS radModeKeydown;
		private System.Windows.Forms.LabelTS lblBreakinDelay;
		private System.Windows.Forms.NumericUpDownTS udSemiBreakInDelay;
		private System.Windows.Forms.Panel panelMouseMode;
		private System.Windows.Forms.CheckBoxTS chkIambic;
		private System.Windows.Forms.RadioButtonTS radModeJoystick;
		private System.Windows.Forms.CheckBoxTS chkSend;		

		#endregion

		#region Constructor and Destructor

		public CW(Console c)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			console = c;

			joystick = new CWKeyer.Paddle();
			radModeJoystick.Enabled = joystick.JoystickPresent;
			joystick.Changed += new CWKeyer.Paddle.PaddleEventHandler(joystick_Changed);

			edge = new float[NEDGE];
			Audio.cw_form = this;
			current_mode = CWMode.MOUSE;
			GenTables();

			pollkey_thread = new Thread(new ThreadStart(PollKey));
			pollkey_thread.Name = "Key Polling Thread";
			pollkey_thread.Priority = ThreadPriority.Normal;
			pollkey_thread.IsBackground = true;

			ActiveControl = chkSend;

			if(!chkMemory1.Checked && !chkMemory2.Checked &&
				!chkMemory3.Checked && !chkMemory4.Checked &&
				!chkMemory5.Checked)
				chkMemory1.Checked = true;

			//freeze_input = false;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CW));
			this.grpMessageBuffers = new System.Windows.Forms.GroupBoxTS();
			this.chkMemory4 = new System.Windows.Forms.CheckBoxTS();
			this.chkMemory5 = new System.Windows.Forms.CheckBoxTS();
			this.chkMemory3 = new System.Windows.Forms.CheckBoxTS();
			this.chkMemory2 = new System.Windows.Forms.CheckBoxTS();
			this.txtMemory5 = new System.Windows.Forms.TextBoxTS();
			this.txtMemory4 = new System.Windows.Forms.TextBoxTS();
			this.txtMemory3 = new System.Windows.Forms.TextBoxTS();
			this.txtMemory2 = new System.Windows.Forms.TextBoxTS();
			this.txtMemory1 = new System.Windows.Forms.TextBoxTS();
			this.chkMemory1 = new System.Windows.Forms.CheckBoxTS();
			this.grpManualBuffer = new System.Windows.Forms.GroupBoxTS();
			this.txtKeyboardBuffer = new System.Windows.Forms.TextBoxTS();
			this.grpAutoMode = new System.Windows.Forms.GroupBoxTS();
			this.radModeMemory = new System.Windows.Forms.RadioButtonTS();
			this.radModeAutoKeyboard = new System.Windows.Forms.RadioButtonTS();
			this.radModeMouse = new System.Windows.Forms.RadioButtonTS();
			this.radModeExtKey = new System.Windows.Forms.RadioButtonTS();
			this.grpOptions = new System.Windows.Forms.GroupBoxTS();
			this.chkIambic = new System.Windows.Forms.CheckBoxTS();
			this.udSemiBreakInDelay = new System.Windows.Forms.NumericUpDownTS();
			this.lblBreakinDelay = new System.Windows.Forms.LabelTS();
			this.chkNoMonitor = new System.Windows.Forms.CheckBoxTS();
			this.chkKeyer = new System.Windows.Forms.CheckBoxTS();
			this.chkReversePaddles = new System.Windows.Forms.CheckBoxTS();
			this.chkSemiBreakIn = new System.Windows.Forms.CheckBoxTS();
			this.lblEdge = new System.Windows.Forms.LabelTS();
			this.udEdge = new System.Windows.Forms.NumericUpDownTS();
			this.lblWeight = new System.Windows.Forms.LabelTS();
			this.udWeight = new System.Windows.Forms.NumericUpDownTS();
			this.lblWPM = new System.Windows.Forms.LabelTS();
			this.udWPM = new System.Windows.Forms.NumericUpDownTS();
			this.chkLoopBuffer = new System.Windows.Forms.CheckBoxTS();
			this.panelDot = new System.Windows.Forms.Panel();
			this.panelDash = new System.Windows.Forms.Panel();
			this.lblDot = new System.Windows.Forms.LabelTS();
			this.lblDash = new System.Windows.Forms.LabelTS();
			this.chkSend = new System.Windows.Forms.CheckBoxTS();
			this.panelDeedle = new System.Windows.Forms.Panel();
			this.lblDeedle = new System.Windows.Forms.LabelTS();
			this.lblSending = new System.Windows.Forms.LabelTS();
			this.label1 = new System.Windows.Forms.LabelTS();
			this.label2 = new System.Windows.Forms.LabelTS();
			this.label3 = new System.Windows.Forms.LabelTS();
			this.grpManualMode = new System.Windows.Forms.GroupBoxTS();
			this.radModeKeydown = new System.Windows.Forms.RadioButtonTS();
			this.radModeManualKeyboard = new System.Windows.Forms.RadioButtonTS();
			this.radModeJoystick = new System.Windows.Forms.RadioButtonTS();
			this.grpSettings = new System.Windows.Forms.GroupBoxTS();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.panelMouseMode = new System.Windows.Forms.Panel();
			this.grpMessageBuffers.SuspendLayout();
			this.grpManualBuffer.SuspendLayout();
			this.grpAutoMode.SuspendLayout();
			this.grpOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udSemiBreakInDelay)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udEdge)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udWeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udWPM)).BeginInit();
			this.grpManualMode.SuspendLayout();
			this.grpSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpMessageBuffers
			// 
			this.grpMessageBuffers.Controls.Add(this.chkMemory4);
			this.grpMessageBuffers.Controls.Add(this.chkMemory5);
			this.grpMessageBuffers.Controls.Add(this.chkMemory3);
			this.grpMessageBuffers.Controls.Add(this.chkMemory2);
			this.grpMessageBuffers.Controls.Add(this.txtMemory5);
			this.grpMessageBuffers.Controls.Add(this.txtMemory4);
			this.grpMessageBuffers.Controls.Add(this.txtMemory3);
			this.grpMessageBuffers.Controls.Add(this.txtMemory2);
			this.grpMessageBuffers.Controls.Add(this.txtMemory1);
			this.grpMessageBuffers.Controls.Add(this.chkMemory1);
			this.grpMessageBuffers.Location = new System.Drawing.Point(16, 304);
			this.grpMessageBuffers.Name = "grpMessageBuffers";
			this.grpMessageBuffers.Size = new System.Drawing.Size(280, 152);
			this.grpMessageBuffers.TabIndex = 1;
			this.grpMessageBuffers.TabStop = false;
			this.grpMessageBuffers.Text = "Memory Buffers";
			// 
			// chkMemory4
			// 
			this.chkMemory4.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkMemory4.Location = new System.Drawing.Point(8, 96);
			this.chkMemory4.Name = "chkMemory4";
			this.chkMemory4.Size = new System.Drawing.Size(19, 19);
			this.chkMemory4.TabIndex = 8;
			this.chkMemory4.Text = "4";
			this.chkMemory4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkMemory4.CheckedChanged += new System.EventHandler(this.chkMemory4_CheckedChanged);
			// 
			// chkMemory5
			// 
			this.chkMemory5.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkMemory5.Location = new System.Drawing.Point(8, 120);
			this.chkMemory5.Name = "chkMemory5";
			this.chkMemory5.Size = new System.Drawing.Size(19, 19);
			this.chkMemory5.TabIndex = 7;
			this.chkMemory5.Text = "5";
			this.chkMemory5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkMemory5.CheckedChanged += new System.EventHandler(this.chkMemory5_CheckedChanged);
			// 
			// chkMemory3
			// 
			this.chkMemory3.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkMemory3.Location = new System.Drawing.Point(8, 72);
			this.chkMemory3.Name = "chkMemory3";
			this.chkMemory3.Size = new System.Drawing.Size(19, 19);
			this.chkMemory3.TabIndex = 6;
			this.chkMemory3.Text = "3";
			this.chkMemory3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkMemory3.CheckedChanged += new System.EventHandler(this.chkMemory3_CheckedChanged);
			// 
			// chkMemory2
			// 
			this.chkMemory2.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkMemory2.Location = new System.Drawing.Point(8, 48);
			this.chkMemory2.Name = "chkMemory2";
			this.chkMemory2.Size = new System.Drawing.Size(19, 19);
			this.chkMemory2.TabIndex = 5;
			this.chkMemory2.Text = "2";
			this.chkMemory2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkMemory2.CheckedChanged += new System.EventHandler(this.chkMemory2_CheckedChanged);
			// 
			// txtMemory5
			// 
			this.txtMemory5.Location = new System.Drawing.Point(32, 120);
			this.txtMemory5.Name = "txtMemory5";
			this.txtMemory5.Size = new System.Drawing.Size(232, 20);
			this.txtMemory5.TabIndex = 4;
			this.txtMemory5.Text = "w5sxd w5sxd`";
			// 
			// txtMemory4
			// 
			this.txtMemory4.Location = new System.Drawing.Point(32, 96);
			this.txtMemory4.Name = "txtMemory4";
			this.txtMemory4.Size = new System.Drawing.Size(232, 20);
			this.txtMemory4.TabIndex = 3;
			this.txtMemory4.Text = "*000* test de w5sxd/b em02xs texas |ar|^00^ ";
			// 
			// txtMemory3
			// 
			this.txtMemory3.Location = new System.Drawing.Point(32, 72);
			this.txtMemory3.Name = "txtMemory3";
			this.txtMemory3.Size = new System.Drawing.Size(232, 20);
			this.txtMemory3.TabIndex = 2;
			this.txtMemory3.Text = "5nn ntx k`";
			// 
			// txtMemory2
			// 
			this.txtMemory2.Location = new System.Drawing.Point(32, 48);
			this.txtMemory2.Name = "txtMemory2";
			this.txtMemory2.Size = new System.Drawing.Size(232, 20);
			this.txtMemory2.TabIndex = 1;
			this.txtMemory2.Text = "73|sk|ee`";
			// 
			// txtMemory1
			// 
			this.txtMemory1.Location = new System.Drawing.Point(32, 24);
			this.txtMemory1.Name = "txtMemory1";
			this.txtMemory1.Size = new System.Drawing.Size(232, 20);
			this.txtMemory1.TabIndex = 0;
			this.txtMemory1.Text = "cq cq cq de w5sxd w5sxd @k`";
			// 
			// chkMemory1
			// 
			this.chkMemory1.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkMemory1.Location = new System.Drawing.Point(8, 24);
			this.chkMemory1.Name = "chkMemory1";
			this.chkMemory1.Size = new System.Drawing.Size(19, 19);
			this.chkMemory1.TabIndex = 1;
			this.chkMemory1.Text = "1";
			this.chkMemory1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkMemory1.CheckedChanged += new System.EventHandler(this.chkMemory1_CheckedChanged);
			// 
			// grpManualBuffer
			// 
			this.grpManualBuffer.Controls.Add(this.txtKeyboardBuffer);
			this.grpManualBuffer.Location = new System.Drawing.Point(16, 168);
			this.grpManualBuffer.Name = "grpManualBuffer";
			this.grpManualBuffer.Size = new System.Drawing.Size(280, 128);
			this.grpManualBuffer.TabIndex = 2;
			this.grpManualBuffer.TabStop = false;
			this.grpManualBuffer.Text = "Keyboard Buffer";
			// 
			// txtKeyboardBuffer
			// 
			this.txtKeyboardBuffer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtKeyboardBuffer.Location = new System.Drawing.Point(16, 24);
			this.txtKeyboardBuffer.Multiline = true;
			this.txtKeyboardBuffer.Name = "txtKeyboardBuffer";
			this.txtKeyboardBuffer.Size = new System.Drawing.Size(248, 88);
			this.txtKeyboardBuffer.TabIndex = 0;
			this.txtKeyboardBuffer.Text = "the quick brown fox jumped over the lazy dog.`";
			// 
			// grpAutoMode
			// 
			this.grpAutoMode.Controls.Add(this.radModeMemory);
			this.grpAutoMode.Controls.Add(this.radModeAutoKeyboard);
			this.grpAutoMode.Location = new System.Drawing.Point(16, 8);
			this.grpAutoMode.Name = "grpAutoMode";
			this.grpAutoMode.Size = new System.Drawing.Size(128, 152);
			this.grpAutoMode.TabIndex = 3;
			this.grpAutoMode.TabStop = false;
			this.grpAutoMode.Text = "Automatic Modes";
			// 
			// radModeMemory
			// 
			this.radModeMemory.Location = new System.Drawing.Point(16, 48);
			this.radModeMemory.Name = "radModeMemory";
			this.radModeMemory.Size = new System.Drawing.Size(72, 24);
			this.radModeMemory.TabIndex = 3;
			this.radModeMemory.Text = "Memory";
			this.toolTip1.SetToolTip(this.radModeMemory, "Memory Buffers");
			this.radModeMemory.CheckedChanged += new System.EventHandler(this.radModeMemory_CheckedChanged);
			// 
			// radModeAutoKeyboard
			// 
			this.radModeAutoKeyboard.Location = new System.Drawing.Point(16, 24);
			this.radModeAutoKeyboard.Name = "radModeAutoKeyboard";
			this.radModeAutoKeyboard.Size = new System.Drawing.Size(72, 24);
			this.radModeAutoKeyboard.TabIndex = 0;
			this.radModeAutoKeyboard.Text = "Keyboard";
			this.toolTip1.SetToolTip(this.radModeAutoKeyboard, "Keyboard Buffer");
			this.radModeAutoKeyboard.CheckedChanged += new System.EventHandler(this.radModeAutoKeyboard_CheckedChanged);
			// 
			// radModeMouse
			// 
			this.radModeMouse.Checked = true;
			this.radModeMouse.Location = new System.Drawing.Point(16, 72);
			this.radModeMouse.Name = "radModeMouse";
			this.radModeMouse.Size = new System.Drawing.Size(88, 24);
			this.radModeMouse.TabIndex = 2;
			this.radModeMouse.TabStop = true;
			this.radModeMouse.Text = "Mouse";
			this.toolTip1.SetToolTip(this.radModeMouse, "Left/Right mouse buttons control dot/dash (click on Yellow area beside Send butto" +
				"n)");
			this.radModeMouse.CheckedChanged += new System.EventHandler(this.radModeMouse_CheckedChanged);
			// 
			// radModeExtKey
			// 
			this.radModeExtKey.Location = new System.Drawing.Point(16, 24);
			this.radModeExtKey.Name = "radModeExtKey";
			this.radModeExtKey.Size = new System.Drawing.Size(88, 24);
			this.radModeExtKey.TabIndex = 1;
			this.radModeExtKey.Text = "External Key";
			this.toolTip1.SetToolTip(this.radModeExtKey, "Connected via SDR-1000");
			this.radModeExtKey.CheckedChanged += new System.EventHandler(this.radModeExtKey_CheckedChanged);
			// 
			// grpOptions
			// 
			this.grpOptions.Controls.Add(this.chkIambic);
			this.grpOptions.Controls.Add(this.udSemiBreakInDelay);
			this.grpOptions.Controls.Add(this.lblBreakinDelay);
			this.grpOptions.Controls.Add(this.chkNoMonitor);
			this.grpOptions.Controls.Add(this.chkKeyer);
			this.grpOptions.Controls.Add(this.chkReversePaddles);
			this.grpOptions.Controls.Add(this.chkSemiBreakIn);
			this.grpOptions.Location = new System.Drawing.Point(304, 8);
			this.grpOptions.Name = "grpOptions";
			this.grpOptions.Size = new System.Drawing.Size(216, 152);
			this.grpOptions.TabIndex = 4;
			this.grpOptions.TabStop = false;
			this.grpOptions.Text = "Options";
			// 
			// chkIambic
			// 
			this.chkIambic.Enabled = false;
			this.chkIambic.Location = new System.Drawing.Point(120, 96);
			this.chkIambic.Name = "chkIambic";
			this.chkIambic.Size = new System.Drawing.Size(80, 24);
			this.chkIambic.TabIndex = 10;
			this.chkIambic.Text = "Iambic";
			this.chkIambic.CheckedChanged += new System.EventHandler(this.chkIambic_CheckedChanged);
			// 
			// udSemiBreakInDelay
			// 
			this.udSemiBreakInDelay.Enabled = false;
			this.udSemiBreakInDelay.Location = new System.Drawing.Point(160, 24);
			this.udSemiBreakInDelay.Maximum = new System.Decimal(new int[] {
																			   500,
																			   0,
																			   0,
																			   0});
			this.udSemiBreakInDelay.Name = "udSemiBreakInDelay";
			this.udSemiBreakInDelay.Size = new System.Drawing.Size(40, 20);
			this.udSemiBreakInDelay.TabIndex = 9;
			this.udSemiBreakInDelay.Value = new System.Decimal(new int[] {
																			 100,
																			 0,
																			 0,
																			 0});
			// 
			// lblBreakinDelay
			// 
			this.lblBreakinDelay.Location = new System.Drawing.Point(120, 24);
			this.lblBreakinDelay.Name = "lblBreakinDelay";
			this.lblBreakinDelay.Size = new System.Drawing.Size(40, 23);
			this.lblBreakinDelay.TabIndex = 8;
			this.lblBreakinDelay.Text = "Delay";
			this.lblBreakinDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkNoMonitor
			// 
			this.chkNoMonitor.Location = new System.Drawing.Point(16, 48);
			this.chkNoMonitor.Name = "chkNoMonitor";
			this.chkNoMonitor.TabIndex = 7;
			this.chkNoMonitor.Text = "No  Monitor";
			// 
			// chkKeyer
			// 
			this.chkKeyer.Location = new System.Drawing.Point(16, 72);
			this.chkKeyer.Name = "chkKeyer";
			this.chkKeyer.TabIndex = 6;
			this.chkKeyer.Text = "Keyer";
			this.chkKeyer.CheckedChanged += new System.EventHandler(this.chkKeyer_CheckedChanged);
			// 
			// chkReversePaddles
			// 
			this.chkReversePaddles.Enabled = false;
			this.chkReversePaddles.Location = new System.Drawing.Point(16, 96);
			this.chkReversePaddles.Name = "chkReversePaddles";
			this.chkReversePaddles.TabIndex = 2;
			this.chkReversePaddles.Text = "Reverse Paddle";
			// 
			// chkSemiBreakIn
			// 
			this.chkSemiBreakIn.Location = new System.Drawing.Point(16, 24);
			this.chkSemiBreakIn.Name = "chkSemiBreakIn";
			this.chkSemiBreakIn.TabIndex = 4;
			this.chkSemiBreakIn.Text = "Semi Break In";
			this.chkSemiBreakIn.CheckedChanged += new System.EventHandler(this.chkSemiBreakIn_CheckedChanged);
			// 
			// lblEdge
			// 
			this.lblEdge.Location = new System.Drawing.Point(8, 88);
			this.lblEdge.Name = "lblEdge";
			this.lblEdge.Size = new System.Drawing.Size(56, 16);
			this.lblEdge.TabIndex = 15;
			this.lblEdge.Text = "Edge (ms)";
			// 
			// udEdge
			// 
			this.udEdge.Location = new System.Drawing.Point(72, 88);
			this.udEdge.Minimum = new System.Decimal(new int[] {
																   1,
																   0,
																   0,
																   0});
			this.udEdge.Name = "udEdge";
			this.udEdge.Size = new System.Drawing.Size(40, 20);
			this.udEdge.TabIndex = 14;
			this.toolTip1.SetToolTip(this.udEdge, "Determines the sharpness of the tone shaping envelope used when beginning or endi" +
				"ng a tone.");
			this.udEdge.Value = new System.Decimal(new int[] {
																 5,
																 0,
																 0,
																 0});
			this.udEdge.ValueChanged += new System.EventHandler(this.udEdge_ValueChanged);
			// 
			// lblWeight
			// 
			this.lblWeight.Location = new System.Drawing.Point(8, 56);
			this.lblWeight.Name = "lblWeight";
			this.lblWeight.Size = new System.Drawing.Size(56, 16);
			this.lblWeight.TabIndex = 11;
			this.lblWeight.Text = "Weight";
			// 
			// udWeight
			// 
			this.udWeight.Location = new System.Drawing.Point(72, 56);
			this.udWeight.Maximum = new System.Decimal(new int[] {
																	 99,
																	 0,
																	 0,
																	 0});
			this.udWeight.Minimum = new System.Decimal(new int[] {
																	 99,
																	 0,
																	 0,
																	 -2147483648});
			this.udWeight.Name = "udWeight";
			this.udWeight.Size = new System.Drawing.Size(40, 20);
			this.udWeight.TabIndex = 10;
			this.toolTip1.SetToolTip(this.udWeight, "Determines the ratio of the dot to dash length.");
			this.udWeight.ValueChanged += new System.EventHandler(this.udEdge_ValueChanged);
			// 
			// lblWPM
			// 
			this.lblWPM.Location = new System.Drawing.Point(8, 24);
			this.lblWPM.Name = "lblWPM";
			this.lblWPM.Size = new System.Drawing.Size(56, 16);
			this.lblWPM.TabIndex = 9;
			this.lblWPM.Text = "WPM";
			// 
			// udWPM
			// 
			this.udWPM.Location = new System.Drawing.Point(72, 24);
			this.udWPM.Maximum = new System.Decimal(new int[] {
																  60,
																  0,
																  0,
																  0});
			this.udWPM.Minimum = new System.Decimal(new int[] {
																  1,
																  0,
																  0,
																  0});
			this.udWPM.Name = "udWPM";
			this.udWPM.Size = new System.Drawing.Size(40, 20);
			this.udWPM.TabIndex = 8;
			this.toolTip1.SetToolTip(this.udWPM, "Words Per Minute -- speed of transmit.  Maximum is set by samples per buffer (Set" +
				"up->Audio).");
			this.udWPM.Value = new System.Decimal(new int[] {
																15,
																0,
																0,
																0});
			this.udWPM.ValueChanged += new System.EventHandler(this.udEdge_ValueChanged);
			// 
			// chkLoopBuffer
			// 
			this.chkLoopBuffer.Location = new System.Drawing.Point(216, 456);
			this.chkLoopBuffer.Name = "chkLoopBuffer";
			this.chkLoopBuffer.TabIndex = 5;
			this.chkLoopBuffer.Text = "Loop Buffer";
			// 
			// panelDot
			// 
			this.panelDot.BackColor = System.Drawing.Color.Black;
			this.panelDot.Location = new System.Drawing.Point(328, 368);
			this.panelDot.Name = "panelDot";
			this.panelDot.Size = new System.Drawing.Size(24, 24);
			this.panelDot.TabIndex = 6;
			// 
			// panelDash
			// 
			this.panelDash.BackColor = System.Drawing.Color.Black;
			this.panelDash.Location = new System.Drawing.Point(376, 368);
			this.panelDash.Name = "panelDash";
			this.panelDash.Size = new System.Drawing.Size(24, 24);
			this.panelDash.TabIndex = 7;
			// 
			// lblDot
			// 
			this.lblDot.Location = new System.Drawing.Point(320, 352);
			this.lblDot.Name = "lblDot";
			this.lblDot.Size = new System.Drawing.Size(40, 16);
			this.lblDot.TabIndex = 8;
			this.lblDot.Text = "Dot";
			this.lblDot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDash
			// 
			this.lblDash.Location = new System.Drawing.Point(368, 352);
			this.lblDash.Name = "lblDash";
			this.lblDash.Size = new System.Drawing.Size(40, 16);
			this.lblDash.TabIndex = 9;
			this.lblDash.Text = "Dash";
			this.lblDash.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chkSend
			// 
			this.chkSend.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkSend.Location = new System.Drawing.Point(328, 312);
			this.chkSend.Name = "chkSend";
			this.chkSend.Size = new System.Drawing.Size(72, 24);
			this.chkSend.TabIndex = 10;
			this.chkSend.Text = "Send";
			this.chkSend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkSend.Click += new System.EventHandler(this.chkSend_Click);
			this.chkSend.CheckedChanged += new System.EventHandler(this.chkSend_CheckedChanged);
			// 
			// panelDeedle
			// 
			this.panelDeedle.BackColor = System.Drawing.Color.Black;
			this.panelDeedle.Location = new System.Drawing.Point(344, 424);
			this.panelDeedle.Name = "panelDeedle";
			this.panelDeedle.Size = new System.Drawing.Size(40, 32);
			this.panelDeedle.TabIndex = 11;
			// 
			// lblDeedle
			// 
			this.lblDeedle.Location = new System.Drawing.Point(344, 408);
			this.lblDeedle.Name = "lblDeedle";
			this.lblDeedle.Size = new System.Drawing.Size(40, 16);
			this.lblDeedle.TabIndex = 12;
			this.lblDeedle.Text = "Deedle";
			// 
			// lblSending
			// 
			this.lblSending.Location = new System.Drawing.Point(24, 464);
			this.lblSending.Name = "lblSending";
			this.lblSending.Size = new System.Drawing.Size(100, 16);
			this.lblSending.TabIndex = 13;
			this.lblSending.Text = "Sending: ";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(608, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(280, 240);
			this.label1.TabIndex = 14;
			this.label1.Text = "label1";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(608, 272);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(280, 23);
			this.label2.TabIndex = 15;
			this.label2.Text = "label2";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(608, 304);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(280, 23);
			this.label3.TabIndex = 16;
			this.label3.Text = "label3";
			// 
			// grpManualMode
			// 
			this.grpManualMode.Controls.Add(this.radModeKeydown);
			this.grpManualMode.Controls.Add(this.radModeManualKeyboard);
			this.grpManualMode.Controls.Add(this.radModeMouse);
			this.grpManualMode.Controls.Add(this.radModeExtKey);
			this.grpManualMode.Controls.Add(this.radModeJoystick);
			this.grpManualMode.Location = new System.Drawing.Point(152, 8);
			this.grpManualMode.Name = "grpManualMode";
			this.grpManualMode.Size = new System.Drawing.Size(144, 152);
			this.grpManualMode.TabIndex = 17;
			this.grpManualMode.TabStop = false;
			this.grpManualMode.Text = "Manual Modes";
			// 
			// radModeKeydown
			// 
			this.radModeKeydown.Location = new System.Drawing.Point(16, 96);
			this.radModeKeydown.Name = "radModeKeydown";
			this.radModeKeydown.TabIndex = 4;
			this.radModeKeydown.Text = "Keydown";
			this.radModeKeydown.CheckedChanged += new System.EventHandler(this.radModeKeydown_CheckedChanged);
			// 
			// radModeManualKeyboard
			// 
			this.radModeManualKeyboard.Location = new System.Drawing.Point(16, 48);
			this.radModeManualKeyboard.Name = "radModeManualKeyboard";
			this.radModeManualKeyboard.Size = new System.Drawing.Size(88, 24);
			this.radModeManualKeyboard.TabIndex = 3;
			this.radModeManualKeyboard.Text = "Keyboard";
			this.toolTip1.SetToolTip(this.radModeManualKeyboard, "Period/For. Slash control dot/dash");
			this.radModeManualKeyboard.CheckedChanged += new System.EventHandler(this.radModeManualKeyboard_CheckedChanged);
			// 
			// radModeJoystick
			// 
			this.radModeJoystick.Location = new System.Drawing.Point(16, 120);
			this.radModeJoystick.Name = "radModeJoystick";
			this.radModeJoystick.Size = new System.Drawing.Size(72, 24);
			this.radModeJoystick.TabIndex = 4;
			this.radModeJoystick.Text = "Joystick";
			this.radModeJoystick.CheckedChanged += new System.EventHandler(this.radModeJoystick_CheckedChanged);
			// 
			// grpSettings
			// 
			this.grpSettings.Controls.Add(this.lblEdge);
			this.grpSettings.Controls.Add(this.udEdge);
			this.grpSettings.Controls.Add(this.lblWeight);
			this.grpSettings.Controls.Add(this.udWeight);
			this.grpSettings.Controls.Add(this.lblWPM);
			this.grpSettings.Controls.Add(this.udWPM);
			this.grpSettings.Location = new System.Drawing.Point(304, 168);
			this.grpSettings.Name = "grpSettings";
			this.grpSettings.Size = new System.Drawing.Size(128, 128);
			this.grpSettings.TabIndex = 4;
			this.grpSettings.TabStop = false;
			this.grpSettings.Text = "Settings";
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// panelMouseMode
			// 
			this.panelMouseMode.BackColor = System.Drawing.Color.Yellow;
			this.panelMouseMode.Location = new System.Drawing.Point(424, 344);
			this.panelMouseMode.Name = "panelMouseMode";
			this.panelMouseMode.Size = new System.Drawing.Size(88, 80);
			this.panelMouseMode.TabIndex = 18;
			this.panelMouseMode.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CW_MouseUp);
			this.panelMouseMode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CW_MouseDown);
			// 
			// CW
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(536, 494);
			this.Controls.Add(this.panelMouseMode);
			this.Controls.Add(this.grpSettings);
			this.Controls.Add(this.grpManualMode);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblSending);
			this.Controls.Add(this.lblDeedle);
			this.Controls.Add(this.panelDeedle);
			this.Controls.Add(this.chkSend);
			this.Controls.Add(this.lblDash);
			this.Controls.Add(this.lblDot);
			this.Controls.Add(this.panelDash);
			this.Controls.Add(this.panelDot);
			this.Controls.Add(this.grpOptions);
			this.Controls.Add(this.grpAutoMode);
			this.Controls.Add(this.grpManualBuffer);
			this.Controls.Add(this.grpMessageBuffers);
			this.Controls.Add(this.chkLoopBuffer);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "CW";
			this.Text = "PowerSDR CW Keyer";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CW_KeyDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.CW_Closing);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CW_KeyUp);
			this.grpMessageBuffers.ResumeLayout(false);
			this.grpManualBuffer.ResumeLayout(false);
			this.grpAutoMode.ResumeLayout(false);
			this.grpOptions.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udSemiBreakInDelay)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udEdge)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udWeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udWPM)).EndInit();
			this.grpManualMode.ResumeLayout(false);
			this.grpSettings.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Misc Routines

		public void SaveCWOptions()
		{
			// Automatically saves all control settings to the database in the tab
			// pages on this form of the following types: CheckBox, ComboBox,
			// NumericUpDown, RadioButton, TextBox, and TrackBar (slider)

			chkSend.Checked = false;		// stop sending first
			//chkSemiBreakIn.Checked = false;	// no Semi Break In

			ArrayList a = new ArrayList();

			foreach(Control c in this.Controls)			// For each control
			{
				if(c.GetType() == typeof(GroupBoxTS))		// if it is a groupbox, check for sub controls
				{
					foreach(Control c2 in ((GroupBoxTS)c).Controls)	// for each sub-control
					{	// check to see if it is a value type we need to save
						if(c2.GetType() == typeof(CheckBoxTS))
							a.Add(c2.Name+"/"+((CheckBoxTS)c2).Checked.ToString());
						else if(c2.GetType() == typeof(ComboBoxTS))
						{
							//if(((ComboBox)c2).SelectedIndex >= 0)
								a.Add(c2.Name+"/"+((ComboBoxTS)c2).Text);
						}
						else if(c2.GetType() == typeof(NumericUpDownTS))
							a.Add(c2.Name+"/"+((NumericUpDownTS)c2).Value.ToString());
						else if(c2.GetType() == typeof(RadioButtonTS))
							a.Add(c2.Name+"/"+((RadioButtonTS)c2).Checked.ToString());
						else if(c2.GetType() == typeof(TextBoxTS))
							a.Add(c2.Name+"/"+((TextBoxTS)c2).Text);
						else if(c2.GetType() == typeof(TrackBarTS))
							a.Add(c2.Name+"/"+((TrackBarTS)c2).Value.ToString());
					}						
				}
				else // it is not a group box
				{	// check to see if it is a value type we need to save
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
				}
			}

			DB.SaveVars("CW", ref a);		// save the values to the DB
		}

		public void GetCWOptions()
		{
			// Automatically restores all controls from the database in the
			// tab pages on this form of the following types: CheckBox, ComboBox,
			// NumericUpDown, RadioButton, TextBox, and TrackBar (slider)

			// get list of live controls
			ArrayList checkbox_list = new ArrayList();
			ArrayList combobox_list = new ArrayList();
			ArrayList numericupdown_list = new ArrayList();
			ArrayList radiobutton_list = new ArrayList();
			ArrayList textbox_list = new ArrayList();
			ArrayList trackbar_list = new ArrayList();

			foreach(Control c in this.Controls)
			{
				if(c.GetType() == typeof(GroupBoxTS))	// if control is a groupbox, retrieve all subcontrols
				{
					foreach(Control c2 in ((GroupBoxTS)c).Controls)
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
				else
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

			ArrayList a = DB.GetVars("CW");						// Get the saved list of controls
			a.Sort();
			int num_controls = checkbox_list.Count + combobox_list.Count +
				numericupdown_list.Count + radiobutton_list.Count +
				textbox_list.Count + trackbar_list.Count;

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
							c.Text = val;	// restore value
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
							if(!val.ToLower().Equals("true") && !val.ToLower().Equals("false"))
								val = "True";
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
			}
		}

		public void WPM_Down()
		{
			if(udWPM.Value != udWPM.Minimum)
				udWPM.Value--;
		}

		public void WPM_Up()
		{
			if(udWPM.Value != udWPM.Maximum)
				udWPM.Value++;
		}

		private void RefreshPanels(bool dot, bool dash)
		{
			Color c = Color.Black;
			if(dot)
				c = console.ButtonSelectedColor;

			DotColor = c;

			if(dash)
				c = console.ButtonSelectedColor;
			else
				c = Color.Black;

			DashColor = c;
		}

		private void StopSend()
		{
			console.MOX = false;
			console.MON = mon_before_cw;
			console.TUNEnabled = true;

			current_mstate = MSTATE.MOP_IDLE;
			current_kstate = KSTATE.KOP_IDLE;
			current_qstate = QSTATE.QOP_IDLE;
			nq = 0;

			switch(current_mode)
			{
				case CWMode.AUTO_KEYBOARD:
					ModeMemoryEnable = true;
					ModeExtKeyEnable = true;
					ModeManualKeyboardEnable = true;
					ModeMouseEnable = true;
					ModeKeydownEnable = true;
					break;
				case CWMode.MEMORY:
					ModeAutoKeyboardEnable = true;
					ModeExtKeyEnable = true;
					ModeManualKeyboardEnable = true;
					ModeMouseEnable = true;
					ModeKeydownEnable = true;
					break;
				case CWMode.EXT_KEY:
				case CWMode.MANUAL_KEYBOARD:
				case CWMode.MOUSE:
				case CWMode.KEYDOWN:
				case CWMode.JOYSTICK:
					ModeAutoKeyboardEnable = true;
					ModeMemoryEnable = true;
					break;
			}
			gogo = false;
				
			DotColor = Color.Black;
			DashColor = Color.Black;	
			SendColor = SystemColors.Control;
		}

		private void PollKey()
		{
			int count = 0;

			while(true)
			{
				if(this.Visible && console.PowerOn)
				{
					byte b = console.Hdw.StatusPort();
					extkey_dash = ((b & (byte)StatusPin.Dash) != 0);
					extkey_dot = ((b & (byte)StatusPin.Dot) != 0);
				
					if(!manual_send)
					{
						if((current_mode == CWMode.EXT_KEY) &&
							chkSemiBreakIn.Checked &&
							(extkey_dot || extkey_dash))
						{
							if(!Send)
							{
								freeze_input = true;
								initial_dot = extkey_dot;
								initial_dash = extkey_dash;
								Send = true;
							}
							count = 0;
						}
						else if(current_mode == CWMode.EXT_KEY &&
							chkSemiBreakIn.Checked &&
							chkSend.Checked &&
							(!extkey_dot && !extkey_dash) &&
							 !freeze_input)
						{
							count++;
						}

						if(count > (int)udSemiBreakInDelay.Value &&
							chkSemiBreakIn.Checked &&
							!freeze_input)
						{
							if(Send)
								Send = false;
						}
					}
				}
				else
				{
					extkey_dash = false;
					extkey_dot = false;
				}
				
				Thread.Sleep(1);
				if(!this.Visible || !console.PowerOn)
					Thread.Sleep(1000);
			}
		}

		private delegate void SetTimerDel(System.Windows.Forms.Timer t, bool enable);
		private void SetTimer(System.Windows.Forms.Timer t, bool enable)
		{
			t.Enabled = enable;
		}

		#endregion

		#region CW Routines 

		unsafe public void CWGen(float* out_l_ptr, float* out_r_ptr, int nsamp)
		{
//			float *ptr2 = outptr;
//			int TABLE_SIZE = nsamp/10;
//			int lphase=0, rphase=0;
//
//			for(int j=0; j<nsamp; j++)
//			{
//				*ptr2++ = (float)Math.Sin(((double)lphase/(double)TABLE_SIZE) * Math.PI * 2);  // left  					
//				*ptr2++ = (float)Math.Sin(((double)rphase/(double)TABLE_SIZE) * Math.PI * 2);  // right 
//				lphase += 1;
//				if(lphase >= TABLE_SIZE ) lphase -= TABLE_SIZE;
//				rphase += 2; // higher pitch so we can distinguish left and right. 
//				if(rphase >= TABLE_SIZE ) rphase -= TABLE_SIZE;
//			}
//			
//			return;

			float *lptr = out_l_ptr;
			float *rptr = out_r_ptr;

			if(current_mstate == MSTATE.MOP_END &&
				current_kstate == KSTATE.KOP_END &&
				current_qstate == QSTATE.QOP_END)
				return;								// exiting so return to callback

			double s;

			if(freeze_input)
			{
				//Debug.WriteLine("freeze");
				isdot = initial_dot;
				isdash = initial_dash;
			}

			switch(current_mode)
			{
				case CWMode.MANUAL_KEYBOARD:
					if(!freeze_input)
					{
						isdot = KBDot;
						isdash = KBDash;
					}

					if(chkReversePaddles.Checked)
					{
						bool temp = isdot;
						isdot = isdash;
						isdash = temp;
					}

					RefreshPanels(isdot, isdash);
					if(chkKeyer.Checked)
						DoKeyer();
					else
						kkeydown = isdash || isdot;
					break;
				case CWMode.MOUSE:
					if(!freeze_input)
					{
						isdot = MouseDot;
						isdash = MouseDash;
					}

					if(chkReversePaddles.Checked)
					{
						bool temp = isdot;
						isdot = isdash;
						isdash = temp;
					}

					RefreshPanels(isdot, isdash);
					if(chkKeyer.Checked)
						DoKeyer();
					else
						kkeydown = isdash || isdot;
					break;
				case CWMode.EXT_KEY:
					if(!freeze_input)
					{
						isdash = ExtKeyDash;
						isdot = ExtKeyDot;
					}
				
					if(chkReversePaddles.Checked)
					{
						bool temp = isdot;
						isdot = isdash;
						isdash = temp;
					}

					RefreshPanels(isdot, isdash);
					if(chkKeyer.Checked)
						DoKeyer();
					else
						kkeydown = isdash || isdot;
					break;
				case CWMode.JOYSTICK:
					if(!freeze_input)
					{
						isdash = joy_dash;
						isdot = joy_dot;
					}

					RefreshPanels(isdot, isdash);
					if(chkReversePaddles.Checked)
					{
						bool temp = isdot;
						isdot = isdash;
						isdash = temp;
					}

					if(chkKeyer.Checked)
						DoKeyer();
					else
						kkeydown = isdash || isdot;
					break;
			}

			if(!chkKeyer.Checked && freeze_input)
				freeze_input = false;
	
			int mode = 0;
			if(DttSP.CurrentMode == DttSP.Mode.CWU)
				mode = 1;

			double cosval = Math.Cos(tonephase);
			double sinval = Math.Sin(tonephase);
			double cosdelta = Math.Cos(tonedelta);
			double sindelta = Math.Sin(tonedelta);
			double tmpval;

			ndown = 0;											// reset deedle light counter

			double volume = 1.0;

			if (mode == 0) 
			{
				for(int i=0; i<nsamp; i++)							// for each sample
				{
					if(!chkSend.Checked) return;

					if(current_mstate == MSTATE.MOP_END &&
						current_kstate == KSTATE.KOP_END &&
						current_qstate == QSTATE.QOP_END)
						return;								// exiting so return to callback

					if(current_mode == CWMode.AUTO_KEYBOARD || 
						current_mode == CWMode.MEMORY)
						s = GetLevel();								// process the character state machine giving a level
					else
						s = GetKeyerLevel();						// process the keyer tone state machine giving a level

					s *= volume;
					tmpval = cosval*cosdelta - sinval*sindelta;
					sinval = cosval*sindelta + sinval*cosdelta;
					cosval = tmpval;

					*lptr++ = (float)(cosval * s);		// stuff the scaled samples

					*rptr++ = (float)(sinval * s);

					tonephase += tonedelta;							// bump oscillator phase
				}
			}
			else 
			{
				for(int i=0; i<nsamp; i++)							// for each sample
				{
					if(!chkSend.Checked) return;

					if(current_mstate == MSTATE.MOP_END &&
						current_kstate == KSTATE.KOP_END &&
						current_qstate == QSTATE.QOP_END)
						return;								// exiting so return to callback

					if(current_mode == CWMode.AUTO_KEYBOARD || 
						current_mode == CWMode.MEMORY)
						s = GetLevel();								// process the character state machine giving a level
					else
						s = GetKeyerLevel();						// process the keyer tone state machine giving a level

					s *= volume;
					tmpval = cosval*cosdelta - sinval*sindelta;
					sinval = cosval*sindelta + sinval*cosdelta;
					cosval = tmpval;

					*lptr++ = (float)(sinval * s);		// stuff the scaled samples

					*rptr++ = (float)(cosval * s);

					tonephase += tonedelta;							// bump oscillator phase
					if(tonephase > TWOPI) 
						tonephase -= TWOPI;
				} 
			}				   

			if(ndown > (nsamp / 2))								// if over half the samples are key down
				Deedle = Color.Green;			// make the deedle green
			else
				Deedle = Color.Black;			// otherwise black
		}

		private void DoKeyer()
		{    
			switch(current_kstate)
			{
				case KSTATE.KOP_UNINITIALISED:					// at power up
					current_kstate = KSTATE.KOP_IDLE;
					break;
				case KSTATE.KOP_IDLE:							// cyclin' on station ...
					if(isdash && isdot)
					{
						if(iambic)
						{
							dokeyer_dashmemory = true;
							dokeyer_dotmemory = true;
							if(dokeyer_lastdot)						// last wuz dot so do a dash
							{
								dokeyer_instate = nbel * 3 + wt;	// length of a dash plus fudge
								current_kstate = KSTATE.KOP_DASH;	// go to dash state
								kkeydown = true;					// and key the pig ...
							}
							else									// last wuz dash so do a dot
							{
								dokeyer_instate = nbel + wt;		// length of a dot plus fudge
								current_kstate = KSTATE.KOP_DOT;	// go to dot state
								kkeydown = true;					// and key the pig ...
							}
						}
						else
						{
							if(dokeyer_dotmemory)
							{
								dokeyer_instate = nbel + wt;		// length of a dot plus fudge
								current_kstate = KSTATE.KOP_DOT;	// go to dot state
								kkeydown = true;					// and key the pig ...
							}
							else
							{
								dokeyer_instate = nbel * 3 + wt;	// length of a dash plus fudge
								current_kstate = KSTATE.KOP_DASH;	// go to dash state
								kkeydown = true;					// and key the pig ...
							}
						}
					}
					else if(dokeyer_dotmemory && dokeyer_dashmemory)
					{
						if(dokeyer_lastdot)						// last wuz dot so do a dash
						{
							dokeyer_instate = nbel * 3 + wt;	// length of a dash plus fudge
							current_kstate = KSTATE.KOP_DASH;	// go to dash state
							kkeydown = true;					// and key the pig ...
						}
						else									// last wuz dash so do a dot
						{
							dokeyer_instate = nbel + wt;		// length of a dot plus fudge
							current_kstate = KSTATE.KOP_DOT;	// go to dot state
							kkeydown = true;					// and key the pig ...
						}
					}
					else if(isdash || dokeyer_dashmemory)		// start a dash
					{
						dokeyer_dashmemory = false;				// clear dash memory
						dokeyer_instate = nbel * 3 + wt;		// length of a dash plus fudge
						current_kstate = KSTATE.KOP_DASH;		// go to dash state
						kkeydown = true;						// and key the pig ...
					}
					else if(isdot || dokeyer_dotmemory)			// start a dot
					{
						dokeyer_dotmemory = false;				// clear dot memory
						dokeyer_instate = nbel + wt;			// length of a dot plus fudge
						current_kstate = KSTATE.KOP_DOT;		// go to dot state
						kkeydown = true;						// and key the pig ...
					}					
					break;
				case KSTATE.KOP_DOT:							// in a dot
					if(iambic)
					{
						if(isdash)
							dokeyer_dashmemory = true;
						dokeyer_dotmemory = false;				// kill dot memory
					}
					else
					{
						if(isdash && !isdot)
						{
							dokeyer_dashmemory = true;
							dokeyer_dotmemory = false;
						}
						else if(isdash && isdot)
						{
							dokeyer_dashmemory = false;
							dokeyer_dotmemory = true;
						}
					}
					if(--dokeyer_instate < 1)					// tally me banana ...; dot is done
					{
						if((initial_dot || (initial_dash && chkReversePaddles.Checked))
							&& freeze_input)
							freeze_input = false;
						dokeyer_lastdot = true;					// we just did a dot
						dokeyer_instate = nbel - wt;			// set length of a space less the fudge
						kkeydown = false;						// unkey the pig
						current_kstate = KSTATE.KOP_SPACE;		// and space it out
					}
					break;
				case KSTATE.KOP_DASH:							// in a dash
					if(iambic)
					{
						if(isdot)
							dokeyer_dotmemory = true;				// memorize the dot
						dokeyer_dashmemory = false;
					}
					else
					{
						if(isdot && !isdash)
						{
							dokeyer_dotmemory = true;
							dokeyer_dashmemory = false;
						}
						else if(isdot && isdash)
						{
							dokeyer_dotmemory = false;
							dokeyer_dashmemory = true;
						}
					}
					if(--dokeyer_instate < 1)					// tally; dash is done
					{
						if((initial_dash || (initial_dot && chkReversePaddles.Checked))
							&& freeze_input)
							freeze_input = false;
						dokeyer_lastdot = false;				// since we just did a dash
						dokeyer_instate = nbel - wt;			// set length of a space less the fudge
						kkeydown = false;						// unkey the pig
						current_kstate = KSTATE.KOP_SPACE;		// and space it out
					}
					break;
				case KSTATE.KOP_SPACE:							// doing the inter-element space
//					if(isdot) dokeyer_dotmemory = true;
//					if(isdash) dokeyer_dashmemory = true;
					if(--dokeyer_instate < 1)
						current_kstate = KSTATE.KOP_IDLE;
					break;
				case KSTATE.KOP_END:
					//current_kstate = KSTATE.KOP_UNINITIALISED;
					break;
			}
		}

		private double GetLevel()
		{
			// char mode tone modulator

			switch(current_mstate)								// process the state machine
			{        
				case MSTATE.MOP_UNINITIALISED:					// at power up
					merging = false;
					dashing = false;
					spacing = false;
					current_mstate = MSTATE.MOP_IDLE;
					getlevel_s = 0;
					nq = 0;
					break;

				case MSTATE.MOP_IDLE:							// cyclin' on station ...
					if(current_mode == CWMode.MEMORY)
					{
						if(nq == 0)								// reload the queue
						{
							switch(current_buffer)
							{
								case 1:
									queue = " "+txtMemory1.Text;
//									txtMemory1.BackColor = Color.Yellow;
//									txtMemory1.ReadOnly = true;
									break;
								case 2:
									queue = " "+txtMemory2.Text;
//									txtMemory2.BackColor = Color.Yellow;
//									txtMemory2.ReadOnly = true;
									break;
								case 3:
									queue = " "+txtMemory3.Text;
//									txtMemory3.BackColor = Color.Yellow;
//									txtMemory3.ReadOnly = true;
									break;
								case 4:
									queue = " "+txtMemory4.Text;
//									txtMemory4.BackColor = Color.Yellow;
//									txtMemory4.ReadOnly = true;
									break;
								case 5:
									queue = " "+txtMemory5.Text;
//									txtMemory5.BackColor = Color.Yellow;
//									txtMemory5.ReadOnly = true;
									break;
								default:
									queue = " ";
									break;
							}
							nq = queue.Length;					// and find the length
							if(nq < 1)
							{
								queue = " ";
								nq = 1;
							}

							merging = false;
							dashing = false;
							spacing = false;
                   
							qin = 0;							// -> 1st character
							getlevel_cc = next_char();
							mdq(getlevel_cc);					// convert character
						}
						else
						{
							getlevel_cc = next_char();
							mdq(getlevel_cc);					// convert character
						}
					}
					else										// see if any characters left in manual queue
					{
						if(txtKeyboardBuffer.Text.Length > 0)
						{
							mdq(mandq());
							nq = 0;
						}
						else
							mdq(" ");
					}
            
					if(nn == 0)									// 'tis a space or msp
					{
						if(msp)
						{
							msp = false;
							getlevel_instate = spel * 2;
						}
						else
							getlevel_instate = spel * 4;		// three out allready, do 4 more
						current_mstate = MSTATE.MOP_XSPACE;		// (but not quite correct for adjacent spaces)
					}
					else										// 'tis a normal character
					{
						getlevel_dashes = dd;					// the selected dash bits
						getlevel_nel = nn;						// and the number of elements
						getlevel_mask = 32768;					// initialise the bit mask
						current_mstate = MSTATE.MOP_INCHAR;		// start up the character
					}
					break;
            
				case MSTATE.MOP_INCHAR:							// in the character, start element
					if(getlevel_nel < 1)						// all done the character elements
					{
						if(merging)
							getlevel_instate = 1;				// effectively none here
						else
							getlevel_instate = spel * 2;		// add extra 2, 1 out allready
						current_mstate = MSTATE.MOP_XSPACE;		// go space out the trail
					}
					else
					{
						getlevel_do_dot = ((getlevel_dashes & getlevel_mask) == 0);		// which type element
						getlevel_mask /= 2;						// shift the mask right
                        
						getlevel_nel--;							// and tally me banana ...
						getlevel_instate = spedge;				// set edge size
						getlevel_e = 0;							// -> start of edge (PI)
						getlevel_s = 0;							// start with key up
						current_mstate = MSTATE.MOP_LEADING;	// start leading edge
					}
					break;
            
				case MSTATE.MOP_LEADING:						// in the leading edge
					getlevel_s = edge[getlevel_e++];			// fetch current edge value and point to the next
					if(--getlevel_instate <= 0)					// tally; ready to start key down state
					{
						if(getlevel_do_dot)
							getlevel_instate = ondot;
						else
							getlevel_instate = ondash;
						current_mstate = MSTATE.MOP_KEYON;
					}
					break;

				case MSTATE.MOP_KEYON:							// doing the key on, s at 1.0
					ndown++;
					getlevel_instate--;							// tally sample
					if(getlevel_instate <= 0)					// ready to start ramping down
					{
						getlevel_instate = spedge;				// for this many samples
						getlevel_e = spedge - 1;				// -> top of edge array PI*2
						current_mstate = MSTATE.MOP_TRAILING;	// and start trailin'
					}
					break;
            
				case MSTATE.MOP_TRAILING:						// doing the trailing edge
					getlevel_s = edge[getlevel_e--];			// fetch current edge value and point to the next (back down)
					if(--getlevel_instate <= 0)					// tally sample; time to start space after element
					{
						if(getlevel_do_dot)
							getlevel_instate = spdotsp;
						else
							getlevel_instate = spdashsp;
						current_mstate = MSTATE.MOP_SPACING;	// space 'er out
						getlevel_s = 0;							// ensure key up
					}
					break;
            
				case MSTATE.MOP_SPACING:						// timing the space inside character
					getlevel_s = 0;
					getlevel_instate--;							// tally sample
					if(getlevel_instate <= 0)
						current_mstate = MSTATE.MOP_INCHAR;		// go fetch next character element
					break;
            
				case MSTATE.MOP_XSPACE:							// timing the space outside character
					getlevel_s = 0;
					getlevel_instate--;							// tally sample
					if(getlevel_instate <= 0)
					{
						current_mstate = MSTATE.MOP_IDLE;		// go fetch next character
						if(nq == 0 && !chkLoopBuffer.Checked)
							current_mstate = MSTATE.MOP_STANDBY;
					}
					break;
            
				case MSTATE.MOP_STANDBY:						// standby 'til gogo set
					getlevel_s = 0;
					if(gogo || (txtKeyboardBuffer.Text.Length > 0))
					{
						nq = 0;
						gogo = false;
						current_mstate = MSTATE.MOP_IDLE;
					}
					break;
				case MSTATE.MOP_END:
					//current_mstate = MSTATE.MOP_UNINITIALISED;
					break;
			}
    
			if(spacing && !keydown)
			{
				getlevel_s = 0;
				ndown = 0;
			}
			else if(keydown || dashing)
			{
				getlevel_s = 1;
				ndown++;
			}
            
			return getlevel_s;	// return this
		}

		private double GetKeyerLevel()
		{    
			switch(current_qstate)								// process the state machine
			{
				case QSTATE.QOP_UNINITIALISED:					// at power up
					current_qstate = QSTATE.QOP_IDLE;
					getkeyerlevel_s = 0;
					kkeydown = false;
					break;

				case QSTATE.QOP_IDLE:							// cyclin' on station ...
					getkeyerlevel_s = 0;						// idle with key up
					if(kkeydown)								// need to start
					{
						getkeyerlevel_instate = spedge;			// edge size
						getkeyerlevel_e = 0;					// -> start of edge (PI)
						current_qstate = QSTATE.QOP_LEADING;	// start leading edge
					}
					break;

				case QSTATE.QOP_LEADING:						// in the leading edge
					getkeyerlevel_s = edge[getkeyerlevel_e++];	// fetch current edge value; and point to the next
					if(--getkeyerlevel_instate <= 0)			// tally; ready to start key down state
						current_qstate = QSTATE.QOP_KEYON;
					break;

				case QSTATE.QOP_KEYON:							// doing the key on, s at 1.0
					ndown++;
					if(!kkeydown)								// ready to start ramping down
					{
						getkeyerlevel_instate = spedge;			// for this many samples
						getkeyerlevel_e = spedge - 1;			// -> top of edge array PI*2
						current_qstate = QSTATE.QOP_TRAILING;	// and start trailin'
					}
					break;
            
				case QSTATE.QOP_TRAILING:						// doing the trailing edge
					getkeyerlevel_s = edge[getkeyerlevel_e--];		// fetch current edge value; and point to the next (back down)
					if(--getkeyerlevel_instate <= 0)			// tally sample; time to quit
					{
						current_qstate = QSTATE.QOP_IDLE;		// back to idle
						getkeyerlevel_s = 0;					// insure key up
					}
					break;
				case QSTATE.QOP_END:
					//current_qstate = QSTATE.QOP_UNINITIALISED;
					break;
			}
    
			if(keydown)
			{
				getkeyerlevel_s = 1;
				ndown++;
			}
    
			return getkeyerlevel_s;	// return this
		}

		private string mandq()									// returns top character of manual queue
		{

			string cc = "";
			bool done = false;    
    
			while(done == false)
			{
				string s = txtKeyboardBuffer.Text;
				if(s.Length < 1)
					s = " ";									// in case he led off with a control
				cc = s.Substring(0, 1);							// fetch first character
				int select = txtKeyboardBuffer.SelectionStart;	// save the cursor location
				KeyboardBuffer = s.Substring(1, s.Length-1);	//truncate first
				if(select > 0)									// keep the cursor location
					txtKeyboardBuffer.Select(select-1, 0);
				if(cc == "|")									// start or stop merging
				{
					if(merging)
					{
						merging = false;
						cc = "space";							// special space for end
						done = true;
					}
					else
					{
						merging = true;
						cc = " ";
					}
				}
				else if(cc == "*")								// start or stop a long dash
				{
					dashing = !dashing;
					cc = " ";									// in case of end
				}
				else if(cc == "^")								// start or stop a long space
				{
					spacing = !spacing;
					cc = " ";									// in case of end
				}
				else
					done = true;
			}

			if(cc == "`")
			{
				Send = false;
			}
    
			return cc;
		}

		private void mdq(string s)
		{
			// fetch Morse values for an ASCII code

			string q;
    
			msp = false;
    
			if(s == "`")
				q = " ";
            else if(s == "space")
				q = " ";
			else
				q = s;
    
			Sending = "Sending: "+q;
    
			switch(s.ToLower())
			{
				case "space":
					nn = 0;	dd = 0x0;	msp = true;	break;	// special end of merge space					
				case "#":
					nn = 7;	dd = 0x0;		break;			// 00000000
/*			    case (string)10:
                    nn = 0;	dd = 0x0;		break;
                case (string)13:
                    nn = 0;	dd = 0x0;		break;
                case " ":
                    nn = 0;	dd = 0x0;		break;
*/              case ".":
                    nn = 6;	dd = 0x5400;	break;			// 01010100
				case ",":
                    nn = 6;	dd = 0xCC00;	break;			// 11001100
				case "?":
					nn = 6;	dd = 0x3000;	break;			// 00110000
				case "-":
					nn = 5;	dd = 0x8800;	break;			// 10001000
				case ":":
					nn = 6;	dd = 0xE000;	break;			// 11100000
				case ";":
					nn = 6;	dd = 0xA800;	break;			// 10101000
				case "(":
					nn = 6;	dd = 0xB400;	break;			// 10110100
				case ")":
					nn = 6;	dd = 0xB400;	break;			// 10110100
				case "@":
					nn = 5;	dd = 0x5000;	break;			// 01010000
				case "$":
					nn = 6;	dd = 0x1400;	break;			// 00010100
				case "!":
					nn = 5;	dd = 0x4000;	break;			// 01000000
				case "a":
					nn = 2;	dd = 0x4000;	break;			// 01000000
				case "b":
					nn = 4;	dd = 0x8000;	break;			// 10000000
				case "c":
					nn = 4;	dd = 0xA000;	break;			// 10100000
				case "d":
					nn = 3;	dd = 0x8000;	break;			// 10000000
				case "e":
					nn = 1;	dd = 0x0;		break;			// 00000000
				case "f":
					nn = 4;	dd = 0x2000;	break;			// 00100000
				case "g":
					nn = 3;	dd = 0xC000;	break;			// 11000000
				case "h":
					nn = 4;	dd = 0x0;		break;			// 00000000
				case "i":
					nn = 2;	dd = 0x0;		break;			// 00000000
				case "j":
					nn = 4;	dd = 0x7000;	break;			// 01110000
				case "k":
					nn = 3;	dd = 0xA000;	break;			// 10100000
				case "l":
					nn = 4;	dd = 0x4000;	break;			// 01000000
				case "m":
					nn = 2;	dd = 0xC000;	break;			// 11000000
				case "n":
					nn = 2;	dd = 0x8000;	break;			// 10000000
				case "o":
					nn = 3;	dd = 0xE000;	break;			// 11100000
				case "p":
					nn = 4;	dd = 0x6000;	break;			// 01100000
				case "q":
					nn = 4;	dd = 0xD000;	break;			// 11010000
				case "r":
					nn = 3;	dd = 0x4000;	break;			// 01000000
				case "s":
					nn = 3;	dd = 0x0;		break;			// 00000000
				case "t":
					nn = 1;	dd = 0x8000;	break;			// 10000000
				case "u":
					nn = 3;	dd = 0x2000;	break;			// 00100000
				case "v":
					nn = 4;	dd = 0x1000;	break;			// 00010000
				case "w":
					nn = 3;	dd = 0x6000;	break;			// 01100000
				case "x":
					nn = 4;	dd = 0x9000;	break;			// 10010000
				case "y":
					nn = 4;	dd = 0xB000;	break;			// 10110000
				case "z":
					nn = 4;	dd = 0xC000;	break;			// 11000000
				case "0":
					nn = 5;	dd = 0xF800;	break;			// 11111000
				case "1":
					nn = 5;	dd = 0x7800;	break;			// 01111000
				case "2":
					nn = 5;	dd = 0x3800;	break;			// 00111000
				case "3":
					nn = 5;	dd = 0x1800;	break;			// 00011000
				case "4":
					nn = 5;	dd = 0x800;		break;			// 00001000
				case "5":
					nn = 5;	dd = 0x0;		break;			// 00000000
				case "6":
					nn = 5;	dd = 0x8000;	break;			// 10000000
				case "7":
					nn = 5;	dd = 0xC000;	break;			// 11000000
				case "8":
					nn = 5;	dd = 0xE000;	break;			// 11100000
				case "9":
					nn = 5;	dd = 0xF000;	break;			// 11110000
				case "/":
					nn = 5;	dd = 0x9000;	break;		    // 10010000
				default:
					nn = 0;	dd = 0x0;		break;			// 00000000
			}
		}

		private string next_char()						// next character from memory buffer
		{
			string cc = "";								// current character
			bool done = false;
    
			while(done == false)
			{
				cc = queue.Substring(qin++, 1);			// fetch next character; move to next character
        
				if(--nq <= 0)							// and tally ...
					done = true;
        
				// check for special merge character codes
				if(cc == "|")							// start or stop merging
				{
					if(merging)
					{
						merging = false;
						cc = "space";					// special space for end
						done = true;
					}
					else
					{
						merging = true;
						cc = " ";
					}
				}
				else if(cc == "*")						// start or stop a long dash
				{
					dashing = !dashing;
					cc = " ";							// in case of end
				}
				else if(cc == "^")						// start or stop a long space
				{
					spacing = !spacing;
					cc = " ";							// in case of end
				}
				else
					done = true;
			}

			if(cc == "`")
			{
				if(!chkLoopBuffer.Checked)
				{
					Send = false;
				}
//				txtMemory1.BackColor = SystemColors.Window;
//				txtMemory1.ReadOnly = false;
//				txtMemory2.BackColor = SystemColors.Window;
//				txtMemory2.ReadOnly = false;
//				txtMemory3.BackColor = SystemColors.Window;
//				txtMemory3.ReadOnly = false;
//				txtMemory4.BackColor = SystemColors.Window;
//				txtMemory4.ReadOnly = false;
//				txtMemory5.BackColor = SystemColors.Window;
//				txtMemory5.ReadOnly = false;
				return " ";
			}

			return cc;									// return the value
		}

		private void GenTables()						// generate the tables and timing values
		{
			// some data from following links:
			// http://www.arrl.org/files/infoserv/tech/code-std.txt
			// http://www.qsl.net/n9bor/n0hff.htm

            double phase, delta;
            string ss, tt;
            double w, actual, tb;
    
			srate = DttSP.SampleRate;

			pass++;
            //tel = TWPM / (int)udWPM.Value;				// element time in seconds
			tel = TWPM / (double)udWPM.Value;
    
			spel = (int)((tel * srate) + 0.5);			// samples per element
            nbel = 1 + (spel - 1) / cw_block_size;				// # buffahs/paddle element
            if(nbel < 1) nbel = 1;						// cannot be less than 1
            //w = (double)nbel * (int)udWeight.Value / 100.0;	// in real buffers
			w = (double)nbel * (double)udWeight.Value / 100.0;


			tb = (double)cw_block_size / srate;			// buffer time in seconds
			actual = TWPM / (tb * (double)nbel);		// actual speed
    
			wt = (int)w;								// calculate keyer weight fudge (in buffers)
                
			spdot = (int)((2.0 * tel * srate) + 0.5);	// samples / dot+space
			spdash = (int)((4.0 * tel * srate) + 0.5);	// samples / dash+space
			spedge = (int)(((int)udEdge.Value * 0.001 * srate) + 0.5);		// samples / edge
    
			if(spedge > NEDGE)
                spedge = NEDGE;							// limit to allocated size
            
            tt = udWPM.Value + " wpm, tel " + (tel * 1000.0 + 0.5) + " ms, pass " + pass + "\n";
			ss = "spdot " + spdot + ", spdash " + spdash + ", spedge " + spedge + "\n";
			tt = tt + ss;
    
			ondot = (int)(((1.0 + ((int)udWeight.Value / 100.0)) * tel * srate) + 0.5);				// sample / on dot
			ondot -= spedge * 2;						// less the edge time
            ondash = (int)(((1.0 + ((int)udWeight.Value / 100.0)) * (tel * 3.0) * srate) + 0.5);	// sample / on dash
            ondash -= spedge * 2;						// less the edge time
			spdotsp = spdot - (spedge * 2) - ondot;		// dot space time remaining
			spdashsp = spdash - (spedge * 2) - ondash;	// dash space time remaining
    
			ss = "ondot " + ondot + ", ondash " + ondash;
			tt = tt + ss;
			ss = ", spdotsp " + spdotsp + ", spdashsp " + spdashsp;
			tt = tt + ss;
		    
			ss = "\n" + "weight " + udWeight.Value + ", edge " + (int)udEdge.Value * 0.001;
			tt = tt + ss;
		    
			ss = "\n" + "nbel " + nbel + ", wt " + wt + ", actual wpm " + actual.ToString("F1");
			tt = tt + ss;

			label1.Text = tt;
			label2.Text = nbel + ", " + wt;
			label3.Text = actual.ToString("f1");
		    
			// generate the edge table
			phase = PI;
			delta = PI / (double)spedge;
			for(int i=0; i<spedge; i++)
			{
				edge[i] = (float)((Math.Cos(phase) + 1.0) / 2.0);
				phase += delta;
			}
		}

		#endregion

		#region Properties

		private int cw_block_size = 2048;		// number of samples per callback
		public int CWBlockSize
		{
			get { return cw_block_size; }
			set
			{
				cw_block_size = value;
				GenTables();
				double d = Math.Log(cw_block_size, 2);
				udWPM.Maximum = (decimal)Math.Pow(2, (11 - d)) * 15;
			}
		}

		private int cw_pitch = 600;
		public int CWPitch
		{
			get{ return cw_pitch; }
			set
			{										// calculate new phase delta if pitch changed
				cw_pitch = value;
				tonedelta = (double)TWOPI / (DttSP.SampleRate / cw_pitch);
				tonephase = 0;
			}
		}

		public bool Keydown
		{
			get { return radModeKeydown.Checked; }
			set
			{
				radModeKeydown.Checked = value;
			}
		}

		public bool Send
		{
			get { return chkSend.Checked; }
			set
			{
				chkSend.Checked = value;
			}
		}

		public bool SendEnabled
		{
			get { return chkSend.Enabled; }
			set
			{
				chkSend.Enabled = value;
			}
		}

		public Color SendColor
		{
			get { return chkSend.BackColor; }
			set
			{
				chkSend.BackColor = value;
			}
		}

		public Color Deedle
		{
			get { return panelDeedle.BackColor; }
			set
			{
				if(panelDeedle.InvokeRequired)
				{
					UI.SetCtrlDel del = new UI.SetCtrlDel(UI.SetPanel);
					object[] param = { panelDeedle, value };
					this.Invoke(del, param);
				}
				else panelDeedle.BackColor = value;
			}
		}

		public Color DotColor
		{
			get { return panelDot.BackColor; }
			set
			{
				if(panelDot.InvokeRequired)
				{
					UI.SetCtrlDel del = new UI.SetCtrlDel(UI.SetPanel);
					object[] param = { panelDot, value };
					this.Invoke(del, param);
				}
				else panelDot.BackColor = value;
			}
		}

		public Color DashColor
		{
			get { return panelDash.BackColor; }
			set
			{
				if(panelDash.InvokeRequired)
				{
					UI.SetCtrlDel del = new UI.SetCtrlDel(UI.SetPanel);
					object[] param = { panelDash, value };
					this.Invoke(del, param);
				}
				else panelDash.BackColor = value;
			}
		}

		public string KeyboardBuffer
		{
			get { return txtKeyboardBuffer.Text; }
			set
			{
				txtKeyboardBuffer.Text = value;
			}
		}

		public string Sending
		{
			get { return lblSending.Text; }
			set
			{
				lblSending.Text = value;
			}
		}

		public bool ModeMemoryEnable
		{
			get { return radModeMemory.Enabled; }
			set
			{
				radModeMemory.Enabled = value;
			}
		}

		public bool ModeExtKeyEnable
		{
			get { return radModeExtKey.Enabled; }
			set
			{
				radModeExtKey.Enabled = value;
			}
		}

		public bool ModeManualKeyboardEnable
		{
			get { return radModeManualKeyboard.Enabled; }
			set
			{
				radModeManualKeyboard.Enabled = value;
			}
		}

		public bool ModeMouseEnable
		{
			get { return radModeMouse.Enabled; }
			set
			{
				radModeMouse.Enabled = value;
			}
		}

		public bool ModeKeydownEnable
		{
			get { return radModeKeydown.Enabled; }
			set
			{
				radModeKeydown.Enabled = value;
			}
		}

		public bool ModeAutoKeyboardEnable
		{
			get { return radModeAutoKeyboard.Enabled; }
			set
			{
				radModeAutoKeyboard.Enabled = value;
			}
		}

		private Mutex mouse_dot_mutex = new Mutex();
		public bool MouseDot
		{
			get
			{
				mouse_dot_mutex.WaitOne();
				bool retval = mouse_dot;
				mouse_dot_mutex.ReleaseMutex();
				return retval;
			}
			set
			{
				mouse_dot_mutex.WaitOne();
				mouse_dot = value;
				mouse_dot_mutex.ReleaseMutex();
			}
		}

		private Mutex mouse_dash_mutex = new Mutex();
		public bool MouseDash
		{
			get
			{
				mouse_dash_mutex.WaitOne();
				bool retval = mouse_dash;
				mouse_dash_mutex.ReleaseMutex();
				return retval;
			}
			set
			{
				mouse_dash_mutex.WaitOne();
				mouse_dash = value;
				mouse_dash_mutex.ReleaseMutex();
			}
		}

		private Mutex kb_dot_mutex = new Mutex();
		public bool KBDot
		{
			get
			{
				kb_dot_mutex.WaitOne();
				bool retval = kb_dot;
				kb_dot_mutex.ReleaseMutex();
				return retval;
			}
			set
			{
				kb_dot_mutex.WaitOne();
				kb_dot = value;
				kb_dot_mutex.ReleaseMutex();
			}
		}

		private Mutex kb_dash_mutex = new Mutex();
		public bool KBDash
		{
			get
			{
				kb_dash_mutex.WaitOne();
				bool retval = kb_dash;
				kb_dash_mutex.ReleaseMutex();
				return retval;
			}
			set
			{
				kb_dash_mutex.WaitOne();
				kb_dash = value;
				kb_dash_mutex.ReleaseMutex();
			}
		}

		private Mutex extkey_dot_mutex = new Mutex();
		public bool ExtKeyDot
		{
			get
			{
				extkey_dot_mutex.WaitOne();
				bool retval = extkey_dot;
				extkey_dot_mutex.ReleaseMutex();
				return retval;
			}
			set
			{
				extkey_dot_mutex.WaitOne();
				extkey_dot = value;
				extkey_dot_mutex.ReleaseMutex();
			}
		}

		private Mutex extkey_dash_mutex = new Mutex();
		public bool ExtKeyDash
		{
			get
			{
				extkey_dash_mutex.WaitOne();
				bool retval = extkey_dash;
				extkey_dash_mutex.ReleaseMutex();
				return retval;
			}
			set
			{
				extkey_dash_mutex.WaitOne();
				extkey_dash = value;
				extkey_dash_mutex.ReleaseMutex();
			}
		}

		#endregion

		#region Event Handlers

		private void CW_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			console.CWKeyMode = false;
			this.Hide();
			e.Cancel = true;
			SaveCWOptions();
		}

		private void chkSend_Click(object sender, System.EventArgs e)
		{
			if(chkSend.Checked)			// because the CheckedChanged event fires first
				manual_send = true;
			else
				manual_send = false;
		}

		private void chkSend_CheckedChanged(object sender, System.EventArgs e)
		{
			// radio must be on
			if(chkSend.Checked)
			{
				if(!console.PowerOn)
				{
					MessageBox.Show("Please power on the radio", "Power Is Off");
					Send = false;
					return;
				}

				// mode must be CW
				if(DttSP.CurrentMode != DttSP.Mode.CWL && 
					DttSP.CurrentMode != DttSP.Mode.CWU)
				{
					MessageBox.Show("Radio not in CW mode");
					Send = false;
					return;
				}
			
				console.TUNEnabled = false;

				// Set monitor
				mon_before_cw = console.MON;
				if(!chkNoMonitor.Checked)
					console.MON = true;           // Turn Monitor on

				// Toggle MOX if in Ham Band
				console.MOX = true;
				if(console.MOX != true)
				{
					Send = false;
					return;
				}

				chkSend.BackColor = Color.Yellow;

				current_mstate = MSTATE.MOP_UNINITIALISED;
				current_kstate = KSTATE.KOP_UNINITIALISED;
				current_qstate = QSTATE.QOP_UNINITIALISED;
				nq = 0;

				switch(current_mode)
				{
					case CWMode.AUTO_KEYBOARD:
						ModeMemoryEnable = false;
						ModeExtKeyEnable = false;
						ModeManualKeyboardEnable = false;
						ModeMouseEnable = false;
						ModeKeydownEnable = false;
						break;
					case CWMode.MEMORY:
						ModeAutoKeyboardEnable = false;
						ModeExtKeyEnable = false;
						ModeManualKeyboardEnable = false;
						ModeMouseEnable = false;
						ModeKeydownEnable = false;
						break;
					case CWMode.EXT_KEY:
					case CWMode.MANUAL_KEYBOARD:
					case CWMode.MOUSE:
					case CWMode.KEYDOWN:
					case CWMode.JOYSTICK:
						ModeAutoKeyboardEnable = false;
						ModeMemoryEnable = false;
						break;
				}

				gogo = true;
				//current_mstate = MSTATE.MOP_STANDBY;
				merging = false;
				dashing = false;
				spacing = false;
			}
			else
			{
				StopSend();
			}
		}

		private void CW_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
				MouseDot = true;
			else if(e.Button == MouseButtons.Right)
				MouseDash = true;
			if((current_mode == CWMode.MOUSE) &&
				chkSemiBreakIn.Checked &&
				(MouseDot || MouseDash))
			{
				if(!Send)
				{
					freeze_input = true;
					initial_dot = MouseDot;
					initial_dash = MouseDash;
					Send = true;
				}
			}
		}

		private void CW_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
				MouseDot = false;
			else if(e.Button == MouseButtons.Right)
				MouseDash = false;
			if(current_mode == CWMode.MOUSE &&
				chkSemiBreakIn.Checked &&
				chkSend.Checked &&
				(!MouseDot && !MouseDash))
			{
				timer1.Enabled = true;
			}
		}

		private void CW_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.OemPeriod)
				KBDot = true;
			else if(e.KeyCode == Keys.OemQuestion)
				KBDash = true;
			else if(e.KeyCode == Keys.F1)
				chkMemory1.Checked = true;
			else if(e.KeyCode == Keys.F2)
				chkMemory2.Checked = true;
			else if(e.KeyCode == Keys.F3)
				chkMemory3.Checked = true;
			else if(e.KeyCode == Keys.F4)
				chkMemory4.Checked = true;
			else if(e.KeyCode == Keys.F5)
				chkMemory5.Checked = true;

			if(current_mode == CWMode.MANUAL_KEYBOARD &&
				chkSemiBreakIn.Checked &&
				(KBDot || KBDash))
			{
				if(!Send)
				{
					//freeze_input = true;
//					initial_dot = KBDot;
//					initial_dash = KBDash;
					Send = true;
				}
			}
		}

		private void CW_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.OemPeriod)
				KBDot = false;
			else if(e.KeyCode == Keys.OemQuestion)
				KBDash = false;
			if(current_mode == CWMode.MANUAL_KEYBOARD &&
				chkSemiBreakIn.Checked && 
				chkSend.Checked &&
				(!KBDot && !KBDash))
			{
				timer1.Enabled = true;
			}
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
//			if(!freeze_input)
//			{
				switch(current_mode)
				{
					case CWMode.EXT_KEY:
						if(ExtKeyDot || ExtKeyDash)
							break_in_counter = 0;
						else
							break_in_counter++;
						break;
					case CWMode.MOUSE:
						if(MouseDot || MouseDash)
							break_in_counter = 0;
						else
							break_in_counter++;
						break;
					case CWMode.MANUAL_KEYBOARD:
						if(KBDot || KBDash)
							break_in_counter = 0;
						else
							break_in_counter++;
						break;
					case CWMode.JOYSTICK:
						if(joy_dot || joy_dash)
							break_in_counter = 0;
						else
							break_in_counter++;
						break;
				}

				if(break_in_counter > (int)udSemiBreakInDelay.Value/timer1.Interval)
				{
					timer1.Enabled = false;
					break_in_counter = 0;
					Send = false;
				}
//			}
		}

		private void udEdge_ValueChanged(object sender, System.EventArgs e)
		{
			GenTables();
		}

		private void radModeAutoKeyboard_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeAutoKeyboard.Checked)
			{
				current_mode = CWMode.AUTO_KEYBOARD;
				radModeExtKey.Checked = false;
				radModeManualKeyboard.Checked = false;
				radModeMouse.Checked = false;
				radModeKeydown.Checked = false;
				chkKeyer.Checked = false;
				chkKeyer.Enabled = false;
				chkSemiBreakIn.Enabled = false;
				chkSemiBreakIn.Checked = false;
			}
		}

		private void radModeMemory_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeMemory.Checked)
			{
				current_mode = CWMode.MEMORY;
				radModeExtKey.Checked = false;
				radModeManualKeyboard.Checked = false;
				radModeMouse.Checked = false;
				radModeKeydown.Checked = false;
				chkKeyer.Checked = false;
				chkKeyer.Enabled = false;
				chkSemiBreakIn.Enabled = false;
				chkSemiBreakIn.Checked = false;
			}
		}

		private void radModeExtKey_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeExtKey.Checked)
			{
				if((pollkey_thread.ThreadState & System.Threading.ThreadState.Unstarted) != 0)
					pollkey_thread.Start();
				else
					pollkey_thread.Resume();

				current_mode = CWMode.EXT_KEY;
				radModeAutoKeyboard.Checked = false;
				radModeMemory.Checked = false;
				chkKeyer.Enabled = true;
				chkSemiBreakIn.Enabled = true;
			}
			else
				pollkey_thread.Suspend();
		}

		private void radModeManualKeyboard_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeManualKeyboard.Checked)
			{
				current_mode = CWMode.MANUAL_KEYBOARD;
				radModeAutoKeyboard.Checked = false;
				radModeMemory.Checked = false;
				chkKeyer.Enabled = true;
				chkSemiBreakIn.Enabled = true;
			}
		}

		private void radModeMouse_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeMouse.Checked)
			{
				current_mode = CWMode.MOUSE;
				radModeAutoKeyboard.Checked = false;
				radModeMemory.Checked = false;
				chkKeyer.Enabled = true;
				chkSemiBreakIn.Enabled = true;
			}
			panelMouseMode.Visible = radModeMouse.Checked;
		}

		private void radModeKeydown_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeKeydown.Checked)
			{
				current_mode = CWMode.KEYDOWN;
				radModeAutoKeyboard.Checked = false;
				radModeMemory.Checked = false;
				chkKeyer.Enabled = false;
				chkKeyer.Checked = false;
				chkSemiBreakIn.Enabled = false;
				chkSemiBreakIn.Checked = false;
			}
			keydown = radModeKeydown.Checked;
		}

		private void radModeJoystick_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeJoystick.Checked)
			{
				current_mode = CWMode.JOYSTICK;
				radModeAutoKeyboard.Checked = false;
				radModeMemory.Checked = false;
				chkKeyer.Enabled = true;
				chkSemiBreakIn.Enabled = true;
			}
			joystick.TimerEnabled = radModeJoystick.Checked;			
		}

		private void chkMemory1_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkMemory1.Checked)
			{
				chkMemory2.Checked = false;
				chkMemory3.Checked = false;
				chkMemory4.Checked = false;
				chkMemory5.Checked = false;
				chkMemory1.BackColor = Color.Yellow;
				current_buffer = 1;
			}
			else
				chkMemory1.BackColor = SystemColors.Control;
		}

		private void chkMemory2_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkMemory2.Checked)
			{
				chkMemory1.Checked = false;
				chkMemory3.Checked = false;
				chkMemory4.Checked = false;
				chkMemory5.Checked = false;
				chkMemory2.BackColor = Color.Yellow;
				current_buffer = 2;
			}
			else
				chkMemory2.BackColor = SystemColors.Control;
		}

		private void chkMemory3_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkMemory3.Checked)
			{
				chkMemory1.Checked = false;
				chkMemory2.Checked = false;
				chkMemory4.Checked = false;
				chkMemory5.Checked = false;
				chkMemory3.BackColor = Color.Yellow;
				current_buffer = 3;
			}
			else
				chkMemory3.BackColor = SystemColors.Control;
		}

		private void chkMemory4_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkMemory4.Checked)
			{
				chkMemory1.Checked = false;
				chkMemory2.Checked = false;
				chkMemory3.Checked = false;
				chkMemory5.Checked = false;
				chkMemory4.BackColor = Color.Yellow;
				current_buffer = 4;
			}
			else
				chkMemory4.BackColor = SystemColors.Control;
		}

		private void chkMemory5_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkMemory5.Checked)
			{
				chkMemory1.Checked = false;
				chkMemory2.Checked = false;
				chkMemory3.Checked = false;
				chkMemory4.Checked = false;
				chkMemory5.BackColor = Color.Yellow;
				current_buffer = 5;
			}
			else
				chkMemory5.BackColor = SystemColors.Control;
		}

		private void chkSemiBreakIn_CheckedChanged(object sender, System.EventArgs e)
		{
			udSemiBreakInDelay.Enabled = chkSemiBreakIn.Checked;
			if(!chkSemiBreakIn.Checked && Send)
				Send = false;
		}

		private void chkIambic_CheckedChanged(object sender, System.EventArgs e)
		{
			iambic = chkIambic.Checked;
		}

		private void chkKeyer_CheckedChanged(object sender, System.EventArgs e)
		{
			chkReversePaddles.Enabled = chkKeyer.Checked;
			chkIambic.Enabled = chkKeyer.Checked;
		}

		private void joystick_Changed(int[] args)
		{
			joy_dot = (args[0] != 0);
			joy_dash = (args[1] != 0);
			//Debug.WriteLine("joy_dot: "+joy_dot.ToString()+"  joy_dash: "+joy_dash.ToString());
			if(chkSemiBreakIn.Checked &&
				(joy_dot || joy_dash))
			{
				if(!Send)
				{
					//freeze_input = true;
//					initial_dot = joy_dot;
//					initial_dash = joy_dash;
					Send = true;
				}
			}
			else if(chkSemiBreakIn.Checked &&
				(!joy_dot || !joy_dash))
			{
				SetTimerDel del = new SetTimerDel(SetTimer);
				object[] param = {timer1, true};
				this.Invoke(del, param);
			}
		}

		#endregion
	}
}
