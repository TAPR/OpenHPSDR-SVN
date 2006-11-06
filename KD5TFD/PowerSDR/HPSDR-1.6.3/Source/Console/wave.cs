//=================================================================
// wave.cs
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
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
	public class WaveControl : System.Windows.Forms.Form
	{
		#region Variable Declaration
		
		private Console console;
		private WaveOptions waveOptionsForm;
		private ArrayList file_list;

		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.CheckBoxTS checkBoxPlay;
		private System.Windows.Forms.GroupBoxTS groupBox2;
		private System.Windows.Forms.CheckBoxTS checkBoxRecord;
		private System.Windows.Forms.GroupBoxTS grpPlayback;
		private System.Windows.Forms.ButtonTS btnStop;
		private System.Windows.Forms.CheckBoxTS checkBoxPause;
		private System.Windows.Forms.ButtonTS btnPrevious;
		private System.Windows.Forms.ButtonTS btnNext;
		private System.Windows.Forms.ListBox lstPlaylist;
		private System.Windows.Forms.ButtonTS btnAdd;
		private System.Windows.Forms.ButtonTS btnRemove;
		private System.Windows.Forms.CheckBoxTS checkBoxRandom;
		private System.Windows.Forms.GroupBox grpPlaylist;
		private System.Windows.Forms.TextBoxTS txtCurrentFile;
		private System.Windows.Forms.LabelTS lblCurrentlyPlaying;
		private System.Windows.Forms.CheckBoxTS checkBoxLoop;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem mnuWaveOptions;
		private System.Windows.Forms.NumericUpDownTS udPreamp;
		private System.Windows.Forms.GroupBoxTS groupBoxTS1;
		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructor and Destructor

		public WaveControl(Console c)
		{
			InitializeComponent();
			console = c;
			file_list = new ArrayList();
			currently_playing = -1;
			waveOptionsForm = new WaveOptions();
			this.ActiveControl = btnAdd;
			RestoreSettings();
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WaveControl));
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.checkBoxPlay = new System.Windows.Forms.CheckBoxTS();
			this.grpPlayback = new System.Windows.Forms.GroupBoxTS();
			this.txtCurrentFile = new System.Windows.Forms.TextBoxTS();
			this.lblCurrentlyPlaying = new System.Windows.Forms.LabelTS();
			this.btnNext = new System.Windows.Forms.ButtonTS();
			this.btnPrevious = new System.Windows.Forms.ButtonTS();
			this.checkBoxPause = new System.Windows.Forms.CheckBoxTS();
			this.btnStop = new System.Windows.Forms.ButtonTS();
			this.groupBox2 = new System.Windows.Forms.GroupBoxTS();
			this.checkBoxRecord = new System.Windows.Forms.CheckBoxTS();
			this.grpPlaylist = new System.Windows.Forms.GroupBox();
			this.checkBoxRandom = new System.Windows.Forms.CheckBoxTS();
			this.checkBoxLoop = new System.Windows.Forms.CheckBoxTS();
			this.btnAdd = new System.Windows.Forms.ButtonTS();
			this.lstPlaylist = new System.Windows.Forms.ListBox();
			this.btnRemove = new System.Windows.Forms.ButtonTS();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.mnuWaveOptions = new System.Windows.Forms.MenuItem();
			this.udPreamp = new System.Windows.Forms.NumericUpDownTS();
			this.groupBoxTS1 = new System.Windows.Forms.GroupBoxTS();
			this.grpPlayback.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.grpPlaylist.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udPreamp)).BeginInit();
			this.groupBoxTS1.SuspendLayout();
			this.SuspendLayout();
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.Filter = "WAV files (*.wav)|*.wav|All files (*.*)|*.*";
			this.openFileDialog1.Multiselect = true;
			this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
			// 
			// checkBoxPlay
			// 
			this.checkBoxPlay.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkBoxPlay.Enabled = false;
			this.checkBoxPlay.Image = null;
			this.checkBoxPlay.Location = new System.Drawing.Point(80, 56);
			this.checkBoxPlay.Name = "checkBoxPlay";
			this.checkBoxPlay.Size = new System.Drawing.Size(40, 23);
			this.checkBoxPlay.TabIndex = 3;
			this.checkBoxPlay.Text = "Play";
			this.checkBoxPlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBoxPlay.CheckedChanged += new System.EventHandler(this.checkBoxPlay_CheckedChanged);
			// 
			// grpPlayback
			// 
			this.grpPlayback.Controls.Add(this.txtCurrentFile);
			this.grpPlayback.Controls.Add(this.lblCurrentlyPlaying);
			this.grpPlayback.Controls.Add(this.btnNext);
			this.grpPlayback.Controls.Add(this.btnPrevious);
			this.grpPlayback.Controls.Add(this.checkBoxPause);
			this.grpPlayback.Controls.Add(this.btnStop);
			this.grpPlayback.Controls.Add(this.checkBoxPlay);
			this.grpPlayback.Location = new System.Drawing.Point(8, 8);
			this.grpPlayback.Name = "grpPlayback";
			this.grpPlayback.Size = new System.Drawing.Size(304, 88);
			this.grpPlayback.TabIndex = 4;
			this.grpPlayback.TabStop = false;
			this.grpPlayback.Text = "Playback";
			// 
			// txtCurrentFile
			// 
			this.txtCurrentFile.Location = new System.Drawing.Point(104, 24);
			this.txtCurrentFile.Name = "txtCurrentFile";
			this.txtCurrentFile.ReadOnly = true;
			this.txtCurrentFile.Size = new System.Drawing.Size(184, 20);
			this.txtCurrentFile.TabIndex = 9;
			this.txtCurrentFile.Text = "";
			// 
			// lblCurrentlyPlaying
			// 
			this.lblCurrentlyPlaying.Image = null;
			this.lblCurrentlyPlaying.Location = new System.Drawing.Point(16, 24);
			this.lblCurrentlyPlaying.Name = "lblCurrentlyPlaying";
			this.lblCurrentlyPlaying.Size = new System.Drawing.Size(96, 23);
			this.lblCurrentlyPlaying.TabIndex = 10;
			this.lblCurrentlyPlaying.Text = "Currently Playing:";
			// 
			// btnNext
			// 
			this.btnNext.Enabled = false;
			this.btnNext.Image = null;
			this.btnNext.Location = new System.Drawing.Point(232, 56);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(40, 23);
			this.btnNext.TabIndex = 8;
			this.btnNext.Text = "Next";
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnPrevious
			// 
			this.btnPrevious.Enabled = false;
			this.btnPrevious.Image = null;
			this.btnPrevious.Location = new System.Drawing.Point(184, 56);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.Size = new System.Drawing.Size(40, 23);
			this.btnPrevious.TabIndex = 7;
			this.btnPrevious.Text = "Prev";
			this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
			// 
			// checkBoxPause
			// 
			this.checkBoxPause.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkBoxPause.Enabled = false;
			this.checkBoxPause.Image = null;
			this.checkBoxPause.Location = new System.Drawing.Point(128, 56);
			this.checkBoxPause.Name = "checkBoxPause";
			this.checkBoxPause.Size = new System.Drawing.Size(48, 23);
			this.checkBoxPause.TabIndex = 5;
			this.checkBoxPause.Text = "Pause";
			this.checkBoxPause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBoxPause.CheckedChanged += new System.EventHandler(this.checkBoxPause_CheckedChanged);
			// 
			// btnStop
			// 
			this.btnStop.Enabled = false;
			this.btnStop.Image = null;
			this.btnStop.Location = new System.Drawing.Point(32, 56);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(40, 23);
			this.btnStop.TabIndex = 4;
			this.btnStop.Text = "Stop";
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkBoxRecord);
			this.groupBox2.Location = new System.Drawing.Point(320, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(88, 64);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Record";
			// 
			// checkBoxRecord
			// 
			this.checkBoxRecord.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkBoxRecord.Image = null;
			this.checkBoxRecord.Location = new System.Drawing.Point(16, 24);
			this.checkBoxRecord.Name = "checkBoxRecord";
			this.checkBoxRecord.Size = new System.Drawing.Size(56, 24);
			this.checkBoxRecord.TabIndex = 0;
			this.checkBoxRecord.Text = "Record";
			this.checkBoxRecord.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBoxRecord.CheckedChanged += new System.EventHandler(this.checkBoxRecord_CheckedChanged);
			// 
			// grpPlaylist
			// 
			this.grpPlaylist.Controls.Add(this.checkBoxRandom);
			this.grpPlaylist.Controls.Add(this.checkBoxLoop);
			this.grpPlaylist.Controls.Add(this.btnAdd);
			this.grpPlaylist.Controls.Add(this.lstPlaylist);
			this.grpPlaylist.Controls.Add(this.btnRemove);
			this.grpPlaylist.Location = new System.Drawing.Point(8, 104);
			this.grpPlaylist.Name = "grpPlaylist";
			this.grpPlaylist.Size = new System.Drawing.Size(304, 184);
			this.grpPlaylist.TabIndex = 6;
			this.grpPlaylist.TabStop = false;
			this.grpPlaylist.Text = "Playlist";
			// 
			// checkBoxRandom
			// 
			this.checkBoxRandom.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkBoxRandom.Enabled = false;
			this.checkBoxRandom.Image = null;
			this.checkBoxRandom.Location = new System.Drawing.Point(224, 24);
			this.checkBoxRandom.Name = "checkBoxRandom";
			this.checkBoxRandom.Size = new System.Drawing.Size(56, 23);
			this.checkBoxRandom.TabIndex = 13;
			this.checkBoxRandom.Text = "Random";
			this.checkBoxRandom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBoxRandom.Visible = false;
			this.checkBoxRandom.CheckedChanged += new System.EventHandler(this.checkBoxRandom_CheckedChanged);
			// 
			// checkBoxLoop
			// 
			this.checkBoxLoop.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkBoxLoop.Enabled = false;
			this.checkBoxLoop.Image = null;
			this.checkBoxLoop.Location = new System.Drawing.Point(176, 24);
			this.checkBoxLoop.Name = "checkBoxLoop";
			this.checkBoxLoop.Size = new System.Drawing.Size(40, 23);
			this.checkBoxLoop.TabIndex = 12;
			this.checkBoxLoop.Text = "Loop";
			this.checkBoxLoop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBoxLoop.CheckedChanged += new System.EventHandler(this.checkBoxLoop_CheckedChanged);
			// 
			// btnAdd
			// 
			this.btnAdd.Image = null;
			this.btnAdd.Location = new System.Drawing.Point(24, 24);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(48, 23);
			this.btnAdd.TabIndex = 6;
			this.btnAdd.Text = "Add...";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// lstPlaylist
			// 
			this.lstPlaylist.Location = new System.Drawing.Point(16, 56);
			this.lstPlaylist.Name = "lstPlaylist";
			this.lstPlaylist.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstPlaylist.Size = new System.Drawing.Size(272, 108);
			this.lstPlaylist.TabIndex = 0;
			this.lstPlaylist.DoubleClick += new System.EventHandler(this.lstPlaylist_DoubleClick);
			this.lstPlaylist.SelectedIndexChanged += new System.EventHandler(this.lstPlaylist_SelectedIndexChanged);
			// 
			// btnRemove
			// 
			this.btnRemove.Enabled = false;
			this.btnRemove.Image = null;
			this.btnRemove.Location = new System.Drawing.Point(80, 24);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(56, 23);
			this.btnRemove.TabIndex = 11;
			this.btnRemove.Text = "Remove";
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuWaveOptions});
			// 
			// mnuWaveOptions
			// 
			this.mnuWaveOptions.Index = 0;
			this.mnuWaveOptions.Text = "Options";
			this.mnuWaveOptions.Click += new System.EventHandler(this.mnuWaveOptions_Click);
			// 
			// udPreamp
			// 
			this.udPreamp.BackColor = System.Drawing.SystemColors.Window;
			this.udPreamp.ForeColor = System.Drawing.SystemColors.ControlText;
			this.udPreamp.Increment = new System.Decimal(new int[] {
																	   1,
																	   0,
																	   0,
																	   0});
			this.udPreamp.Location = new System.Drawing.Point(16, 24);
			this.udPreamp.Maximum = new System.Decimal(new int[] {
																	 70,
																	 0,
																	 0,
																	 0});
			this.udPreamp.Minimum = new System.Decimal(new int[] {
																	 70,
																	 0,
																	 0,
																	 -2147483648});
			this.udPreamp.Name = "udPreamp";
			this.udPreamp.Size = new System.Drawing.Size(40, 20);
			this.udPreamp.TabIndex = 52;
			this.udPreamp.Value = new System.Decimal(new int[] {
																   0,
																   0,
																   0,
																   0});
			this.udPreamp.ValueChanged += new System.EventHandler(this.udPreamp_ValueChanged);
			// 
			// groupBoxTS1
			// 
			this.groupBoxTS1.Controls.Add(this.udPreamp);
			this.groupBoxTS1.Location = new System.Drawing.Point(320, 80);
			this.groupBoxTS1.Name = "groupBoxTS1";
			this.groupBoxTS1.Size = new System.Drawing.Size(88, 56);
			this.groupBoxTS1.TabIndex = 53;
			this.groupBoxTS1.TabStop = false;
			this.groupBoxTS1.Text = "TX Gain (dB)";
			// 
			// WaveControl
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 302);
			this.Controls.Add(this.groupBoxTS1);
			this.Controls.Add(this.grpPlaylist);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.grpPlayback);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.Name = "WaveControl";
			this.Text = "Wave File Controls";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.WaveControl_Closing);
			this.grpPlayback.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.grpPlaylist.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udPreamp)).EndInit();
			this.groupBoxTS1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties

		private int currently_playing;
		private int CurrentlyPlaying
		{
			get { return currently_playing; }
			set
			{
				if(value > lstPlaylist.Items.Count-1) 
					value = lstPlaylist.Items.Count-1;
				
				currently_playing = value;
				if(currently_playing == 0)
					btnPrevious.Enabled = false;
				else 
					btnPrevious.Enabled = true;

				if(currently_playing == lstPlaylist.Items.Count-1)
				{
					if(!checkBoxLoop.Checked)
						btnNext.Enabled = false;
				}
				else
					btnNext.Enabled = true;
			}
		}

		#endregion

		#region Misc Routines

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

		public void SaveSettings()
		{
			ArrayList a = new ArrayList();
			ArrayList temp = new ArrayList();

			ControlList(this, ref temp);

			foreach(Control c in temp)				// For each control
			{
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

			DB.SaveVars("WaveOptions", ref a);		// save the values to the DB
		}

		public void RestoreSettings()
		{
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

			ArrayList a = DB.GetVars("WaveOptions");			// Get the saved list of controls
			a.Sort();
			
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
		}

		private bool OpenWaveFile(string filename)
		{
			RIFFChunk riff = null;
			fmtChunk fmt  = null;
			dataChunk data_chunk  = null;

			if(!File.Exists(filename))
			{
				MessageBox.Show("Filename doesn't exist. ("+filename+")",
					"Bad Filename",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				file_list.RemoveAt(currently_playing);
				return false;
			}

			BinaryReader reader = null;
			try
			{
				reader = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));
			}
			catch(Exception)
			{
				MessageBox.Show("File is already open.");
				return false;
			}

			if(reader.PeekChar() != 'R')
			{
				reader.Close();
				MessageBox.Show("File is not in the correct format.",
					"Wrong File Format",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				file_list.RemoveAt(currently_playing);
				return false;
			}

			while((data_chunk == null ||
				riff == null || fmt == null) &&
				reader.PeekChar() != -1)
			{
				Chunk chunk = Chunk.ReadChunk(ref reader);
				if(chunk.GetType() == typeof(RIFFChunk))
					riff = (RIFFChunk)chunk;
				else if(chunk.GetType() == typeof(fmtChunk))
					fmt = (fmtChunk)chunk;
				else if(chunk.GetType() == typeof(dataChunk))
					data_chunk = (dataChunk)chunk;
			}

			if(reader.PeekChar() == -1)
			{
				reader.Close();
				MessageBox.Show("File is not in the correct format.",
					"Wrong File Format",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				file_list.RemoveAt(currently_playing);
				return false;
			}

			if(riff.riff_type != 0x45564157)
			{
				reader.Close();	
				MessageBox.Show("File is not an RIFF Wave file.",
					"Wrong file format",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				file_list.RemoveAt(currently_playing);
				return false;
			}

			if(fmt.sample_rate != (int)DttSP.SampleRate)
			{
				reader.Close();	
				MessageBox.Show("File has the wrong sample rate.",
					"Wrong Sample Rate",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				file_list.RemoveAt(currently_playing);
				return false;
			}

			if(fmt.channels != 2)
			{
				reader.Close();	
				MessageBox.Show("Wave File is not stereo.",
					"Wrong Number of Channels",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				file_list.RemoveAt(currently_playing);
				return false;
			}

			Audio.wave_file_reader = new WaveFileReader(
				this,
				console.BlockSize1,
				(int)fmt.format,	// use floating point
				ref reader);

			return true;
		}

		private void UpdatePlaylist()
		{
			lstPlaylist.BeginUpdate();
			lstPlaylist.Items.Clear();
			int index = lstPlaylist.SelectedIndex;
			foreach(string s in file_list)
			{
				int i = s.LastIndexOf("\\")+1;
				string file = s.Substring(i, s.IndexOf(".wav")-i);
				lstPlaylist.Items.Add(file);
			}

			if(index < 0 && lstPlaylist.Items.Count > 0)
				lstPlaylist.SelectedIndex = 0;
			else if(lstPlaylist.Items.Count > index)
				lstPlaylist.SelectedIndex = index;
			lstPlaylist.EndUpdate();

			if(lstPlaylist.Items.Count > 0)
			{
				checkBoxPlay.Enabled = true;
				btnRemove.Enabled = true;
				checkBoxLoop.Enabled = true;
			}
			else
			{
				checkBoxPlay.Enabled = false;
				checkBoxPlay.Checked = false;
				btnRemove.Enabled = false;
				checkBoxLoop.Enabled = false;
			}

			if(lstPlaylist.Items.Count > 1)
				checkBoxRandom.Enabled = true;
			else
				checkBoxRandom.Enabled = false;				
		}

		public void Next()
		{
			if(checkBoxPlay.Checked)
			{
				if(btnNext.Enabled)
					btnNext_Click(this, EventArgs.Empty);
				else if(checkBoxLoop.Checked && lstPlaylist.Items.Count == 1)
				{
					checkBoxPlay_CheckedChanged(this, EventArgs.Empty);
				}
				else
					checkBoxPlay.Checked = false;
			}
		}

		#endregion

		#region Event Handlers

		private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			foreach(string s in openFileDialog1.FileNames)
			{
				if(!file_list.Contains(s))
					file_list.Add(s);
			}

			UpdatePlaylist();
		}

		private void checkBoxPlay_CheckedChanged(object sender, System.EventArgs e)
		{
			if(checkBoxPlay.Checked)
			{
				string filename = (string)file_list[currently_playing];
				if(!OpenWaveFile(filename))
				{
					checkBoxPlay.Checked = false;
					currently_playing = -1;
					UpdatePlaylist();
					return;
				}				
							
				txtCurrentFile.Text = (string)lstPlaylist.Items[currently_playing];
				checkBoxPlay.BackColor = console.ButtonSelectedColor;
				checkBoxPause.Enabled = true;
			}
			else
			{
				if(Audio.wave_file_reader != null)
					Audio.wave_file_reader.Stop();
				if(checkBoxPause.Checked) checkBoxPause.Checked = false;
				checkBoxPause.Enabled = false;
				txtCurrentFile.Text = "";
				checkBoxPlay.BackColor = SystemColors.Control;
			}
			console.WavePlayback = checkBoxPlay.Checked;
			Audio.wave_playback = checkBoxPlay.Checked;
		}

		private void checkBoxRecord_CheckedChanged(object sender, System.EventArgs e)
		{
			if(checkBoxRecord.Checked)
			{
				checkBoxRecord.BackColor = console.ButtonSelectedColor;
				string file_name = console.CurrentDSPMode.ToString()+" ";
				file_name += console.VFOAFreq.ToString("f6")+"MHz ";
				file_name += DateTime.Now.ToString()+".wav";
				file_name = file_name.Replace("/", "-");
				file_name = file_name.Replace(":", " ");
				file_name = Application.StartupPath+"\\"+file_name;
				Audio.wave_file_writer = new WaveFileWriter(console.BlockSize1, 2, (int)DttSP.SampleRate, file_name);
			}
			else
			{
				string file_name = Audio.wave_file_writer.Stop();
				checkBoxRecord.BackColor = SystemColors.Control;
				MessageBox.Show("The file has been written to the following location:\n"+file_name);
			}
			Audio.wave_record = checkBoxRecord.Checked;
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			openFileDialog1.ShowDialog();
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			if(lstPlaylist.Items.Count == 0 ||
				lstPlaylist.SelectedIndices.Count == 0) return;
			
			ArrayList selections = new ArrayList();
			
			foreach(int i in lstPlaylist.SelectedIndices)
			{
				if(i == currently_playing && checkBoxPlay.Checked)
				{
					DialogResult dr = MessageBox.Show(
						(string)lstPlaylist.Items[i]+
						" is currently playing.\n"+
						"Stop playing and remove from Playlist?",
						"Stop and Remove?",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Question);

					if(dr == DialogResult.Yes)
					{
						selections.Add(i);
						checkBoxPlay.Checked = false;
					}
				}
				else
					selections.Add(i);
			}
			
			selections.Sort();

			for(int i=selections.Count-1; i>=0; i--)
				file_list.RemoveAt((int)selections[i]);
			UpdatePlaylist();
		}

		private void checkBoxLoop_CheckedChanged(object sender, System.EventArgs e)
		{
			if(checkBoxLoop.Checked)
				checkBoxLoop.BackColor = console.ButtonSelectedColor;
			else
				checkBoxLoop.BackColor = SystemColors.Control;
		}

		private void btnStop_Click(object sender, System.EventArgs e)
		{
			checkBoxPlay.Checked = false;
		}

		private void checkBoxRandom_CheckedChanged(object sender, System.EventArgs e)
		{
			if(checkBoxRandom.Checked)
				checkBoxRandom.BackColor = console.ButtonSelectedColor;
			else
				checkBoxRandom.BackColor = SystemColors.Control;
		}

		private void lstPlaylist_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(lstPlaylist.SelectedIndex < 0)
			{
				btnPrevious.Enabled = false;
				btnNext.Enabled = false;
				return;
			}

			if(!checkBoxPlay.Checked)
			{
				CurrentlyPlaying = lstPlaylist.SelectedIndex;
			}
		}

		private void btnPrevious_Click(object sender, System.EventArgs e)
		{
			if(checkBoxPlay.Checked)
			{
				checkBoxPlay.Checked = false;
				CurrentlyPlaying--;
				checkBoxPlay.Checked = true;
			}
			else
				lstPlaylist.SelectedIndex--;
		}

		private void btnNext_Click(object sender, System.EventArgs e)
		{
			if(checkBoxPlay.Checked)
			{
				checkBoxPlay.Checked = false;
				if(CurrentlyPlaying == lstPlaylist.Items.Count-1)
				{
					CurrentlyPlaying = 0;
				}
				else CurrentlyPlaying++;
				checkBoxPlay.Checked = true;
			}
			else
			{
				int temp = lstPlaylist.SelectedIndex+1;
				if(temp == lstPlaylist.Items.Count) temp = 0;
				lstPlaylist.SelectedIndex = -1;
				lstPlaylist.SelectedIndex = temp;
			}
		}

		private void WaveControl_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
			SaveSettings();
		}

		private void checkBoxPause_CheckedChanged(object sender, System.EventArgs e)
		{
			if(checkBoxPlay.Checked)
				Audio.wave_playback = !checkBoxPause.Checked;

			if(checkBoxPause.Checked)
				checkBoxPause.BackColor = console.ButtonSelectedColor;
			else
				checkBoxPause.BackColor = SystemColors.Control;
		}

		private void lstPlaylist_DoubleClick(object sender, System.EventArgs e)
		{
			if(checkBoxPlay.Checked)
			{
				CurrentlyPlaying = lstPlaylist.SelectedIndex;
				checkBoxPlay.Checked = false;
				checkBoxPlay.Checked = true;
			}
			else checkBoxPlay.Checked = true;
		}

		private void mnuWaveOptions_Click(object sender, System.EventArgs e)
		{
			if(waveOptionsForm == null || waveOptionsForm.IsDisposed)
				waveOptionsForm = new WaveOptions();

			waveOptionsForm.Show();
			waveOptionsForm.Focus();
		}

		#endregion

		private void udPreamp_ValueChanged(object sender, System.EventArgs e)
		{
			Audio.WavePreamp = Math.Pow(10.0, (int)udPreamp.Value/20.0); // convert to scalar
		}
	}

	#region Wave File Header Helper Classes

	public class Chunk
	{
		public int chunk_id;

		public static Chunk ReadChunk(ref BinaryReader reader)
		{
			int data = reader.ReadInt32();
			if(data == 0x46464952)	// RIFF chunk
			{
				RIFFChunk riff = new RIFFChunk();
				riff.chunk_id = data;
				riff.file_size = reader.ReadInt32();
				riff.riff_type = reader.ReadInt32();
				return riff;
			}
			else if(data == 0x20746D66)	// fmt chunk
			{
				fmtChunk fmt = new fmtChunk();
				fmt.chunk_id = data;
				fmt.chunk_size = reader.ReadInt32();
				fmt.format = reader.ReadInt16();
				fmt.channels = reader.ReadInt16();
				fmt.sample_rate = reader.ReadInt32();
				fmt.bytes_per_sec = reader.ReadInt32();
				fmt.block_align = reader.ReadInt16();
				fmt.bits_per_sample = reader.ReadInt16();
				return fmt;
			}
			else if(data == 0x61746164) // data chunk
			{
				dataChunk data_chunk = new dataChunk();
				data_chunk.chunk_id = data;
				data_chunk.chunk_size = reader.ReadInt32();
				return data_chunk;
			}
			else
			{
				Chunk c = new Chunk();
				c.chunk_id = data;
				return c;
			}
		}
	}

	public class RIFFChunk : Chunk
	{
		public int file_size;
		public int riff_type;
	}

	public class fmtChunk : Chunk
	{
		public int chunk_size;
		public short format;
		public short channels;
		public int sample_rate;
		public int bytes_per_sec;
		public short block_align;
		public short bits_per_sample;
	}

	public class dataChunk : Chunk
	{
		public int chunk_size;
		public int[] data;
	}

	#endregion

	#region WaveFile Class

	public class WaveFile
	{
		#region Variable Declaration

		private string filename;
		private int format;
		private int sample_rate;
		private int channels;
		private TimeSpan length;
		private bool valid = false;

		#endregion

		#region Constructor

		public WaveFile(string file)
		{
			RIFFChunk riff = null;
			fmtChunk fmt = null;
			dataChunk data_chunk  = null;

			filename = file;			
			if(!File.Exists(filename))
			{
				valid = false;
				return;
			}

			BinaryReader reader = null;
			try
			{
				reader = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));
			}
			catch(Exception)
			{
				MessageBox.Show("File is already open.");
				valid = false;
				return;
			}

			while((data_chunk == null ||
				riff == null || fmt == null) &&
				reader.PeekChar() != -1)
			{
				Chunk chunk = Chunk.ReadChunk(ref reader);
				if(chunk.GetType() == typeof(RIFFChunk))
					riff = (RIFFChunk)chunk;
				else if(chunk.GetType() == typeof(fmtChunk))
					fmt = (fmtChunk)chunk;
				else if(chunk.GetType() == typeof(dataChunk))
					data_chunk = (dataChunk)chunk;
			}

			reader.Close();

			format = fmt.format;
			sample_rate = fmt.sample_rate;
			channels = fmt.channels;

			if(fmt.bytes_per_sec == 0)
			{
				valid = false;
				return;
			}

			length = new TimeSpan(0, 0, data_chunk.data.Length / fmt.bytes_per_sec);

			valid = true;
		}

		#endregion

		#region Properties

		public int Format
		{
			get { return format; }
		}

		public int SampleRate
		{
			get { return sample_rate; }
		}

		public int Channels
		{
			get { return channels; }
		}

		public TimeSpan Length
		{
			get { return length; }
		}

		public bool Valid
		{
			get { return valid; }
		}

		#endregion

		#region Misc Routines

		public new string ToString()
		{
			string s = filename.PadRight(20, ' ');
			s += length.Hours.ToString("10") + ":" +
				length.Minutes.ToString("nn") + ":" +
				length.Seconds.ToString("nn");
			return s;
		}

		#endregion
	}

	#endregion

	#region Playlist

	public class Playlist
	{
		//ArrayList wave_files;
		
		public void Add(WaveFile w)
		{
			//wave_files.Add(w);
		}

		public void Remove(int i)
		{
			//wave_files.RemoveAt(i);
		}
	}

	#endregion

	#region Wave File Writer Class

	public class WaveFileWriter
	{
		private BinaryWriter writer;
		private bool record;
		private int frames_per_buffer;
		private short channels;
		private int sample_rate;
		private int length_counter;
		private RingBuffer rb;
		private float[] in_buf;
		private float[] out_buf;
		private byte[] byte_buf;
		private const int RB_BLOCK = 2048;
		private string filename;

		public WaveFileWriter(int frames, short chan, int samp_rate, string file)
		{
			rb = new RingBuffer(RB_BLOCK*16);
			frames_per_buffer = frames;
			in_buf = new float[frames_per_buffer*2];
			out_buf = new float[RB_BLOCK*2];
			byte_buf = new byte[RB_BLOCK*2*4];
			channels = chan;
			sample_rate = samp_rate;
			length_counter = 0;
			record = true;

			writer = new BinaryWriter(File.Open(file, FileMode.Create));
			filename = file;

			Thread t = new Thread(new ThreadStart(ProcessRecordBuffers));
			t.Name = "Wave File Write Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;
			t.Start();
		}

		private void ProcessRecordBuffers()
		{
			WriteWaveHeader(ref writer, channels, sample_rate, 32, 0);
 
			while(record == true || rb.ReadSpace() > 0)
			{
				if(rb.ReadSpace() > RB_BLOCK || 
					(record == false && rb.ReadSpace() > 0))
				{
					WriteBuffer(ref writer, ref length_counter);
				}
				Thread.Sleep(3);
			}

			writer.Seek(0, SeekOrigin.Begin);
			WriteWaveHeader(ref writer, channels, sample_rate, 32, length_counter);
			writer.Flush();
			writer.Close();
		}

		unsafe public void AddWriteBuffer(float *left, float *right)
		{
			for(int i=0; i<frames_per_buffer; i++)
			{
				in_buf[i*2] = *left++;
				in_buf[i*2+1] = *right++;
			}

			rb.Write(in_buf, frames_per_buffer*2);

			//Debug.WriteLine("ReadSpace: "+rb.ReadSpace());
		}

		public string Stop()
		{
			record = false;
			return filename;
		}

		private void WriteBuffer(ref BinaryWriter writer, ref int count)
		{
			int cnt = rb.Read(out_buf, RB_BLOCK*2);

            byte[] temp = new byte[4];		
			for(int i=0; i<cnt; i++)
			{
				temp = BitConverter.GetBytes(out_buf[i]);
				for(int j=0; j<4; j++)
					byte_buf[i*4+j] = temp[j];
			}

			writer.Write(byte_buf, 0, cnt*4);
			count += cnt*4;
		}

		private void WriteWaveHeader(ref BinaryWriter writer, short channels, int sample_rate, short bit_depth, int data_length)
		{
			writer.Write(0x46464952);								// "RIFF"		-- descriptor chunk ID
			writer.Write(data_length + 36);							// size of whole file -- 1 for now
			writer.Write(0x45564157);								// "WAVE"		-- descriptor type
			writer.Write(0x20746d66);								// "fmt "		-- format chunk ID
			writer.Write((int)16);									// size of fmt chunk
			writer.Write((short)3);									// FormatTag	-- 3 for floats
			writer.Write(channels);									// wChannels
			writer.Write(sample_rate);								// dwSamplesPerSec
			writer.Write((int)(channels*sample_rate*bit_depth/8));	// dwAvgBytesPerSec
			writer.Write((short)(channels*bit_depth/8));			// wBlockAlign
			writer.Write(bit_depth);								// wBitsPerSample
			writer.Write(0x61746164);								// "data" -- data chunk ID
			writer.Write(data_length);								// chunkSize = length of data
			writer.Flush();											// write the file
		}
	}

	#endregion

	#region Wave File Reader Class

	public class WaveFileReader
	{
		private WaveControl wave_form;
		private BinaryReader reader;
		private int format;
		private bool playback;
		private int frames_per_buffer;
		private RingBuffer rb_l;
		private RingBuffer rb_r;
		private float[] buf_l_in;
		private float[] buf_r_in;
		private float[] buf_l_out;
		private float[] buf_r_out;
		private const int RB_BLOCK = 2048;
		private byte[] io_buf;
		private int io_buf_size;

		public WaveFileReader(
			WaveControl form,
			int frames,
			int fmt,
			ref BinaryReader binread)
		{
			wave_form = form;
			frames_per_buffer = frames;
			format = fmt;
			rb_l = new RingBuffer(8*RB_BLOCK);
			rb_r = new RingBuffer(8*RB_BLOCK);
			buf_l_in = new float[RB_BLOCK];
			buf_r_in = new float[RB_BLOCK];
			buf_l_out = new float[RB_BLOCK];
			buf_r_out = new float[RB_BLOCK];
			if(format == 1)
				io_buf_size = 2048*2*2;
			else if(format == 3)
				io_buf_size = 2048*4*2;
			io_buf = new byte[io_buf_size];

			playback = true;
			reader = binread;

			Thread t = new Thread(new ThreadStart(ProcessBuffers));
			t.Name = "Wave File Read Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;

			do
			{
				ReadBuffer(ref reader, ref buf_l_in, ref buf_r_in);
				rb_l.Write(buf_l_in, RB_BLOCK);
				rb_r.Write(buf_r_in, RB_BLOCK);
			} while(rb_l.WriteSpace() > RB_BLOCK);

			t.Start();
		}

		private void ProcessBuffers()
		{
			while(playback == true)
			{
				
				while (rb_l.WriteSpace() >= RB_BLOCK)
				{
					//Debug.WriteLine("loop 2");
					ReadBuffer(ref reader, ref buf_l_in, ref buf_r_in);
					rb_l.Write(buf_l_in, RB_BLOCK);
					rb_r.Write(buf_r_in, RB_BLOCK);
					if(playback == false)
						goto end;
				}

				if(playback == false)
					goto end;

				Thread.Sleep(10);				
			}

			end:
				reader.Close();
		}

		private void ReadBuffer(
			ref BinaryReader reader,
			ref float[] buf_l,
			ref float[] buf_r)
		{
			
			//Debug.WriteLine("ReadBuffer ("+rb_l.ReadSpace()+")");
			int i=0, num_reads=RB_BLOCK;
			int val = reader.Read(io_buf, 0, io_buf_size);

			if(val < io_buf_size)
			{
				switch(format)
				{
					case 1:		// ints
						num_reads = val / 4;
						break;
					case 3:		// floats
						num_reads = val / 8;
						break;
				}
			}

			for(; i<num_reads; i++)
			{
				switch(format)
				{
					case 1:		// ints
						buf_l[i] = (float)((double)BitConverter.ToInt16(io_buf, i*4) / 32767.0);
						buf_r[i] = (float)((double)BitConverter.ToInt16(io_buf, i*4+2) / 32767.0);
						break;
					case 3:		// floats
						buf_l[i] = BitConverter.ToSingle(io_buf, i*8);
						buf_r[i] = BitConverter.ToSingle(io_buf, i*8+4);
						break;
				}
			}

			if(num_reads < RB_BLOCK)
			{
				for(int j=i; j<RB_BLOCK; j++)
					buf_l[j] = buf_r[j] = 0.0f;

				reader.Close();
				wave_form.Next();
			}
		}

		unsafe public void GetPlayBuffer(float *l_ptr, float *r_ptr)
		{
			//Debug.WriteLine("GetPlayBuffer ("+rb_l.ReadSpace()+")");
			int count = rb_l.ReadSpace();
			if(count == 0) return;

			if(count > frames_per_buffer)
				count = frames_per_buffer;

			rb_l.Read(buf_l_out, count);
			rb_r.Read(buf_r_out, count);
			if(count < frames_per_buffer)
			{
				for(int i=count; i<frames_per_buffer; i++)
					buf_l_out[i] = buf_r_out[i] = 0.0f;
			}

			for(int i=0; i<frames_per_buffer; i++)
			{
				*l_ptr++ = buf_l_out[i];
				*r_ptr++ = buf_r_out[i];
			}
		}

		// FIXME: implement interleaved version of Get Play Buffer
		

		public void Stop()
		{
			playback = false;
		}
	}

	#endregion
}
