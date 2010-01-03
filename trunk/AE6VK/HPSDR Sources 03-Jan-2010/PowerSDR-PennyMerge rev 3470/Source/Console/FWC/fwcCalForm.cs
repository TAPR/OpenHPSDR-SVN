//=================================================================
// fwcCalForm.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2008  FlexRadio Systems
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

using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
	public class FWCCalForm : System.Windows.Forms.Form
	{
		#region Variable Declaration

		private Console console;
		private System.Windows.Forms.ButtonTS btnSaveToEEPROM;
		private System.Windows.Forms.ButtonTS btnCalRXImageAllBands;
		private System.Windows.Forms.ButtonTS btnCalTXImageAllBands;
		private System.Windows.Forms.GroupBoxTS grpAllBands;
		private System.Windows.Forms.ButtonTS btnCalTXCarrierAllBands;
		private System.Windows.Forms.ButtonTS btnClearAll;
		private System.Windows.Forms.ButtonTS btnCheckAll;
		private System.Windows.Forms.CheckBoxTS chk6;
		private System.Windows.Forms.CheckBoxTS chk10;
		private System.Windows.Forms.CheckBoxTS chk12;
		private System.Windows.Forms.CheckBoxTS chk15;
		private System.Windows.Forms.CheckBoxTS chk17;
		private System.Windows.Forms.CheckBoxTS chk20;
		private System.Windows.Forms.CheckBoxTS chk30;
		private System.Windows.Forms.CheckBoxTS chk40;
		private System.Windows.Forms.CheckBoxTS chk60;
		private System.Windows.Forms.CheckBoxTS chk80;
		private System.Windows.Forms.CheckBoxTS chk160;
		private System.Windows.Forms.ButtonTS btnTestCal;
		private System.Windows.Forms.ButtonTS btnCalPAPowerAllBands;
		private System.Windows.Forms.GroupBox grpBands;
		private System.Windows.Forms.ButtonTS btnCalRXLevelAllBands;
		private System.Windows.Forms.ButtonTS btnRestoreFromEEPROM;
		private System.Windows.Forms.ButtonTS btnCalPABridge;
		private System.Windows.Forms.ButtonTS btnCalPABias;
		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructor and Destructor

		public FWCCalForm(Console c)
		{
			InitializeComponent();
			console = c;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FWCCalForm));
			this.btnSaveToEEPROM = new System.Windows.Forms.ButtonTS();
			this.btnCalRXImageAllBands = new System.Windows.Forms.ButtonTS();
			this.btnCalTXImageAllBands = new System.Windows.Forms.ButtonTS();
			this.btnTestCal = new System.Windows.Forms.ButtonTS();
			this.grpAllBands = new System.Windows.Forms.GroupBoxTS();
			this.btnCalRXLevelAllBands = new System.Windows.Forms.ButtonTS();
			this.btnCalPAPowerAllBands = new System.Windows.Forms.ButtonTS();
			this.btnCalTXCarrierAllBands = new System.Windows.Forms.ButtonTS();
			this.btnCalPABias = new System.Windows.Forms.ButtonTS();
			this.btnCalPABridge = new System.Windows.Forms.ButtonTS();
			this.btnClearAll = new System.Windows.Forms.ButtonTS();
			this.btnCheckAll = new System.Windows.Forms.ButtonTS();
			this.chk6 = new System.Windows.Forms.CheckBoxTS();
			this.chk10 = new System.Windows.Forms.CheckBoxTS();
			this.chk12 = new System.Windows.Forms.CheckBoxTS();
			this.chk15 = new System.Windows.Forms.CheckBoxTS();
			this.chk17 = new System.Windows.Forms.CheckBoxTS();
			this.chk20 = new System.Windows.Forms.CheckBoxTS();
			this.chk30 = new System.Windows.Forms.CheckBoxTS();
			this.chk40 = new System.Windows.Forms.CheckBoxTS();
			this.chk60 = new System.Windows.Forms.CheckBoxTS();
			this.chk80 = new System.Windows.Forms.CheckBoxTS();
			this.chk160 = new System.Windows.Forms.CheckBoxTS();
			this.grpBands = new System.Windows.Forms.GroupBox();
			this.btnRestoreFromEEPROM = new System.Windows.Forms.ButtonTS();
			this.grpAllBands.SuspendLayout();
			this.grpBands.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnSaveToEEPROM
			// 
			this.btnSaveToEEPROM.Image = null;
			this.btnSaveToEEPROM.Location = new System.Drawing.Point(448, 136);
			this.btnSaveToEEPROM.Name = "btnSaveToEEPROM";
			this.btnSaveToEEPROM.Size = new System.Drawing.Size(112, 40);
			this.btnSaveToEEPROM.TabIndex = 0;
			this.btnSaveToEEPROM.Text = "Save Calibration Data To EEPROM";
			this.btnSaveToEEPROM.Click += new System.EventHandler(this.btnSaveToEEPROM_Click);
			// 
			// btnCalRXImageAllBands
			// 
			this.btnCalRXImageAllBands.Image = null;
			this.btnCalRXImageAllBands.Location = new System.Drawing.Point(16, 24);
			this.btnCalRXImageAllBands.Name = "btnCalRXImageAllBands";
			this.btnCalRXImageAllBands.Size = new System.Drawing.Size(80, 24);
			this.btnCalRXImageAllBands.TabIndex = 1;
			this.btnCalRXImageAllBands.Text = "RX Image";
			this.btnCalRXImageAllBands.Click += new System.EventHandler(this.btnCalRXImageAllBands_Click);
			// 
			// btnCalTXImageAllBands
			// 
			this.btnCalTXImageAllBands.Image = null;
			this.btnCalTXImageAllBands.Location = new System.Drawing.Point(112, 24);
			this.btnCalTXImageAllBands.Name = "btnCalTXImageAllBands";
			this.btnCalTXImageAllBands.Size = new System.Drawing.Size(80, 24);
			this.btnCalTXImageAllBands.TabIndex = 11;
			this.btnCalTXImageAllBands.Text = "TX Image";
			this.btnCalTXImageAllBands.Click += new System.EventHandler(this.btnCalTXImageAllBands_Click);
			// 
			// btnTestCal
			// 
			this.btnTestCal.Image = null;
			this.btnTestCal.Location = new System.Drawing.Point(336, 136);
			this.btnTestCal.Name = "btnTestCal";
			this.btnTestCal.Size = new System.Drawing.Size(88, 32);
			this.btnTestCal.TabIndex = 12;
			this.btnTestCal.Text = "Do Not Press *Endless Cal*";
			this.btnTestCal.Click += new System.EventHandler(this.btnTestCal_Click);
			// 
			// grpAllBands
			// 
			this.grpAllBands.Controls.Add(this.btnCalRXLevelAllBands);
			this.grpAllBands.Controls.Add(this.btnCalPAPowerAllBands);
			this.grpAllBands.Controls.Add(this.btnCalTXCarrierAllBands);
			this.grpAllBands.Controls.Add(this.btnCalRXImageAllBands);
			this.grpAllBands.Controls.Add(this.btnCalTXImageAllBands);
			this.grpAllBands.Controls.Add(this.btnCalPABias);
			this.grpAllBands.Controls.Add(this.btnCalPABridge);
			this.grpAllBands.Location = new System.Drawing.Point(8, 8);
			this.grpAllBands.Name = "grpAllBands";
			this.grpAllBands.Size = new System.Drawing.Size(304, 152);
			this.grpAllBands.TabIndex = 13;
			this.grpAllBands.TabStop = false;
			this.grpAllBands.Text = "Multi Band";
			// 
			// btnCalRXLevelAllBands
			// 
			this.btnCalRXLevelAllBands.Image = null;
			this.btnCalRXLevelAllBands.Location = new System.Drawing.Point(16, 64);
			this.btnCalRXLevelAllBands.Name = "btnCalRXLevelAllBands";
			this.btnCalRXLevelAllBands.Size = new System.Drawing.Size(80, 24);
			this.btnCalRXLevelAllBands.TabIndex = 32;
			this.btnCalRXLevelAllBands.Text = "RX Level";
			this.btnCalRXLevelAllBands.Click += new System.EventHandler(this.btnCalRXLevelAllBands_Click);
			// 
			// btnCalPAPowerAllBands
			// 
			this.btnCalPAPowerAllBands.Image = null;
			this.btnCalPAPowerAllBands.Location = new System.Drawing.Point(208, 64);
			this.btnCalPAPowerAllBands.Name = "btnCalPAPowerAllBands";
			this.btnCalPAPowerAllBands.Size = new System.Drawing.Size(80, 24);
			this.btnCalPAPowerAllBands.TabIndex = 31;
			this.btnCalPAPowerAllBands.Text = "PA Power";
			this.btnCalPAPowerAllBands.Click += new System.EventHandler(this.btnCalPAPowerAllBands_Click);
			// 
			// btnCalTXCarrierAllBands
			// 
			this.btnCalTXCarrierAllBands.Image = null;
			this.btnCalTXCarrierAllBands.Location = new System.Drawing.Point(112, 64);
			this.btnCalTXCarrierAllBands.Name = "btnCalTXCarrierAllBands";
			this.btnCalTXCarrierAllBands.Size = new System.Drawing.Size(80, 24);
			this.btnCalTXCarrierAllBands.TabIndex = 12;
			this.btnCalTXCarrierAllBands.Text = "TX Carrier";
			this.btnCalTXCarrierAllBands.Click += new System.EventHandler(this.btnCalTXCarrierAllBands_Click);
			// 
			// btnCalPABias
			// 
			this.btnCalPABias.Image = null;
			this.btnCalPABias.Location = new System.Drawing.Point(208, 24);
			this.btnCalPABias.Name = "btnCalPABias";
			this.btnCalPABias.Size = new System.Drawing.Size(80, 24);
			this.btnCalPABias.TabIndex = 33;
			this.btnCalPABias.Text = "PA Bias";
			this.btnCalPABias.Click += new System.EventHandler(this.btnCalPABias_Click);
			// 
			// btnCalPABridge
			// 
			this.btnCalPABridge.Image = null;
			this.btnCalPABridge.Location = new System.Drawing.Point(16, 104);
			this.btnCalPABridge.Name = "btnCalPABridge";
			this.btnCalPABridge.Size = new System.Drawing.Size(80, 24);
			this.btnCalPABridge.TabIndex = 34;
			this.btnCalPABridge.Text = "PA Bridge";
			this.btnCalPABridge.Click += new System.EventHandler(this.btnCalPABridge_Click);
			// 
			// btnClearAll
			// 
			this.btnClearAll.Image = null;
			this.btnClearAll.Location = new System.Drawing.Point(112, 64);
			this.btnClearAll.Name = "btnClearAll";
			this.btnClearAll.Size = new System.Drawing.Size(64, 20);
			this.btnClearAll.TabIndex = 30;
			this.btnClearAll.Text = "Clear All";
			this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
			// 
			// btnCheckAll
			// 
			this.btnCheckAll.Image = null;
			this.btnCheckAll.Location = new System.Drawing.Point(24, 64);
			this.btnCheckAll.Name = "btnCheckAll";
			this.btnCheckAll.Size = new System.Drawing.Size(64, 20);
			this.btnCheckAll.TabIndex = 29;
			this.btnCheckAll.Text = "Check All";
			this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
			// 
			// chk6
			// 
			this.chk6.Checked = true;
			this.chk6.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chk6.Image = null;
			this.chk6.Location = new System.Drawing.Point(216, 40);
			this.chk6.Name = "chk6";
			this.chk6.Size = new System.Drawing.Size(32, 24);
			this.chk6.TabIndex = 28;
			this.chk6.Text = "6";
			// 
			// chk10
			// 
			this.chk10.Checked = true;
			this.chk10.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chk10.Image = null;
			this.chk10.Location = new System.Drawing.Point(176, 40);
			this.chk10.Name = "chk10";
			this.chk10.Size = new System.Drawing.Size(40, 24);
			this.chk10.TabIndex = 27;
			this.chk10.Text = "10";
			// 
			// chk12
			// 
			this.chk12.Checked = true;
			this.chk12.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chk12.Image = null;
			this.chk12.Location = new System.Drawing.Point(136, 40);
			this.chk12.Name = "chk12";
			this.chk12.Size = new System.Drawing.Size(40, 24);
			this.chk12.TabIndex = 26;
			this.chk12.Text = "12";
			// 
			// chk15
			// 
			this.chk15.Checked = true;
			this.chk15.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chk15.Image = null;
			this.chk15.Location = new System.Drawing.Point(96, 40);
			this.chk15.Name = "chk15";
			this.chk15.Size = new System.Drawing.Size(40, 24);
			this.chk15.TabIndex = 25;
			this.chk15.Text = "15";
			// 
			// chk17
			// 
			this.chk17.Checked = true;
			this.chk17.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chk17.Image = null;
			this.chk17.Location = new System.Drawing.Point(56, 40);
			this.chk17.Name = "chk17";
			this.chk17.Size = new System.Drawing.Size(40, 24);
			this.chk17.TabIndex = 24;
			this.chk17.Text = "17";
			// 
			// chk20
			// 
			this.chk20.Checked = true;
			this.chk20.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chk20.Image = null;
			this.chk20.Location = new System.Drawing.Point(16, 40);
			this.chk20.Name = "chk20";
			this.chk20.Size = new System.Drawing.Size(40, 24);
			this.chk20.TabIndex = 23;
			this.chk20.Text = "20";
			// 
			// chk30
			// 
			this.chk30.Checked = true;
			this.chk30.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chk30.Image = null;
			this.chk30.Location = new System.Drawing.Point(184, 16);
			this.chk30.Name = "chk30";
			this.chk30.Size = new System.Drawing.Size(40, 24);
			this.chk30.TabIndex = 22;
			this.chk30.Text = "30";
			// 
			// chk40
			// 
			this.chk40.Checked = true;
			this.chk40.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chk40.Image = null;
			this.chk40.Location = new System.Drawing.Point(144, 16);
			this.chk40.Name = "chk40";
			this.chk40.Size = new System.Drawing.Size(40, 24);
			this.chk40.TabIndex = 21;
			this.chk40.Text = "40";
			// 
			// chk60
			// 
			this.chk60.Checked = true;
			this.chk60.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chk60.Image = null;
			this.chk60.Location = new System.Drawing.Point(104, 16);
			this.chk60.Name = "chk60";
			this.chk60.Size = new System.Drawing.Size(40, 24);
			this.chk60.TabIndex = 20;
			this.chk60.Text = "60";
			// 
			// chk80
			// 
			this.chk80.Checked = true;
			this.chk80.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chk80.Image = null;
			this.chk80.Location = new System.Drawing.Point(64, 16);
			this.chk80.Name = "chk80";
			this.chk80.Size = new System.Drawing.Size(40, 24);
			this.chk80.TabIndex = 19;
			this.chk80.Text = "80";
			// 
			// chk160
			// 
			this.chk160.Checked = true;
			this.chk160.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chk160.Image = null;
			this.chk160.Location = new System.Drawing.Point(16, 16);
			this.chk160.Name = "chk160";
			this.chk160.Size = new System.Drawing.Size(48, 24);
			this.chk160.TabIndex = 18;
			this.chk160.Text = "160";
			// 
			// grpBands
			// 
			this.grpBands.Controls.Add(this.chk6);
			this.grpBands.Controls.Add(this.chk10);
			this.grpBands.Controls.Add(this.chk12);
			this.grpBands.Controls.Add(this.chk15);
			this.grpBands.Controls.Add(this.chk17);
			this.grpBands.Controls.Add(this.chk20);
			this.grpBands.Controls.Add(this.chk30);
			this.grpBands.Controls.Add(this.chk40);
			this.grpBands.Controls.Add(this.chk60);
			this.grpBands.Controls.Add(this.chk80);
			this.grpBands.Controls.Add(this.chk160);
			this.grpBands.Controls.Add(this.btnClearAll);
			this.grpBands.Controls.Add(this.btnCheckAll);
			this.grpBands.Location = new System.Drawing.Point(320, 8);
			this.grpBands.Name = "grpBands";
			this.grpBands.Size = new System.Drawing.Size(256, 104);
			this.grpBands.TabIndex = 14;
			this.grpBands.TabStop = false;
			this.grpBands.Text = "Bands";
			// 
			// btnRestoreFromEEPROM
			// 
			this.btnRestoreFromEEPROM.Image = null;
			this.btnRestoreFromEEPROM.Location = new System.Drawing.Point(8, 168);
			this.btnRestoreFromEEPROM.Name = "btnRestoreFromEEPROM";
			this.btnRestoreFromEEPROM.Size = new System.Drawing.Size(112, 48);
			this.btnRestoreFromEEPROM.TabIndex = 15;
			this.btnRestoreFromEEPROM.Text = "Restore Calibration Data To Database from EEPROM";
			this.btnRestoreFromEEPROM.Click += new System.EventHandler(this.btnRestoreFromEEPROM_Click);
			// 
			// FWCCalForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 230);
			this.Controls.Add(this.btnRestoreFromEEPROM);
			this.Controls.Add(this.grpBands);
			this.Controls.Add(this.grpAllBands);
			this.Controls.Add(this.btnTestCal);
			this.Controls.Add(this.btnSaveToEEPROM);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FWCCalForm";
			this.Text = "FLEX-5000 Calibration";
			this.grpAllBands.ResumeLayout(false);
			this.grpBands.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Event Handlers

		private void btnSaveToEEPROM_Click(object sender, System.EventArgs e)
		{
			DialogResult dr = MessageBox.Show("Are you sure you want to write the current database calibration values to the EEPROM?",
				"Overwrite EEPROM?",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);
			if(dr == DialogResult.No) return;
			
			btnSaveToEEPROM.BackColor = console.ButtonSelectedColor;
			btnSaveToEEPROM.Enabled = false;
			Thread t = new Thread(new ThreadStart(WriteToEEPROM));
			t.Name = "Write To EEPROM Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;
			t.Start();
		}

		private void WriteToEEPROM()
		{
			console.FLEX5000WriteCalData();
			btnSaveToEEPROM.BackColor = SystemColors.Control;
			btnSaveToEEPROM.Enabled = true;
		}

		private void btnCalRXImageAllBands_Click(object sender, System.EventArgs e)
		{
			grpAllBands.Enabled = false;
			btnCalRXImageAllBands.BackColor = console.ButtonSelectedColor;
			CallCalFWCRXImage();
		}

		private void btnCalRXLevelAllBands_Click(object sender, System.EventArgs e)
		{
			grpAllBands.Enabled = false;
			btnCalRXLevelAllBands.BackColor = console.ButtonSelectedColor;
			CallCalFWCRXLevel();
		}

		private void btnCalTXImageAllBands_Click(object sender, System.EventArgs e)
		{
			grpAllBands.Enabled = false;
			btnCalTXImageAllBands.BackColor = console.ButtonSelectedColor;
			CallCalFWCTXImage();
		}

		private void btnCalTXCarrierAllBands_Click(object sender, System.EventArgs e)
		{
			grpAllBands.Enabled = false;
			btnCalTXCarrierAllBands.BackColor = console.ButtonSelectedColor;
			CallCalFWCTXCarrier();
		}

		private void btnCalPAPowerAllBands_Click(object sender, System.EventArgs e)
		{
			string s = "Is a 50 Ohm dummy load connected to the correct antenna port (";
			switch(FWCAnt.ANT1)
			{
				case FWCAnt.ANT1: s += "ANT 1"; break;
				/*case FWCAnt.ANT2: s += "ANT 2"; break;
				case FWCAnt.ANT3: s += "ANT 3"; break;*/
			}
			s += ")?";
			DialogResult dr = MessageBox.Show(s+"\n" +
				"Failure to connect a dummy load properly could cause damage to the radio.",
				"Warning: Is dummy load properly connected?",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning);

			if(dr == DialogResult.No)
				return;

			grpAllBands.Enabled = false;
			btnCalPAPowerAllBands.BackColor = console.ButtonSelectedColor;
			progress = new Progress("Calibrate PA Gain");

			Thread t = new Thread(new ThreadStart(CalibratePAGain));
			t.Name = "PA Gain Calibration Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.AboveNormal;
			t.Start();

			if(console.PowerOn)
				progress.Show();
		}

		private void btnCalPABias_Click(object sender, System.EventArgs e)
		{
			grpAllBands.Enabled = false;
			btnCalPABias.BackColor = console.ButtonSelectedColor;
			progress = new Progress("Calibrate PA Bias");
			Thread t = new Thread(new ThreadStart(CalFWCPABias));
			t.Name = "Cal PA Bias Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;
			t.Start();
			progress.Show();
		}

		public void CalFWCPABias()
		{			
			console.CalibratePABias(progress, 0.200f, 1.400f, 0.005f, 0);
			grpAllBands.Enabled = true;
			btnCalPABias.BackColor = SystemColors.Control;
		}

		private Progress progress;
		private void btnTestCal_Click(object sender, System.EventArgs e)
		{
			btnTestCal.BackColor = console.ButtonSelectedColor;
			progress = new Progress("Calibration Tester");
			Thread t = new Thread(new ThreadStart(TestCal));
			t.Name = "Cal Test Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;
			t.Start();
		}

		private void TestCal()
		{
			while(true)
			{
				RunCalFWCRXImage();
				if(progress.Text == "") break;
				/*RunCalFWCTXImage();
				if(progress.Text == "") break;*/
			}
			btnTestCal.BackColor = SystemColors.Control;
		}

		private void btnCheckAll_Click(object sender, System.EventArgs e)
		{
			chk160.Checked = true;
			chk80.Checked = true;
			chk60.Checked = true;
			chk40.Checked = true;
			chk30.Checked = true;
			chk20.Checked = true;
			chk17.Checked = true;
			chk15.Checked = true;
			chk12.Checked = true;
			chk10.Checked = true;
			chk6.Checked = true;
		}

		private void btnClearAll_Click(object sender, System.EventArgs e)
		{
			chk160.Checked = false;
			chk80.Checked = false;
			chk60.Checked = false;
			chk40.Checked = false;
			chk30.Checked = false;
			chk20.Checked = false;
			chk17.Checked = false;
			chk15.Checked = false;
			chk12.Checked = false;
			chk10.Checked = false;
			chk6.Checked = false;
		}

		#endregion

		#region All Band Calibration Routines

		public void CallCalFWCRXImage()
		{
			progress = new Progress("Calibrate RX Image");
			Thread t = new Thread(new ThreadStart(RunCalFWCRXImage));
			t.Name = "Calibrate RX Image Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;
			t.Start();
		}

		public void RunCalFWCRXImage()
		{
			double vfoa = console.VFOAFreq;
			double vfob = console.VFOBFreq;
			HiPerfTimer t1 = new HiPerfTimer();
			t1.Start();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
			float[] band_freqs = { 1.85f, 3.75f, 5.3665f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f, 50.11f};
			for(int i=0; i<band_freqs.Length; i++)
			{
				bool do_band = false;
				switch(bands[i])
				{
					case Band.B160M: do_band = chk160.Checked; break;
					case Band.B80M: do_band = chk80.Checked; break;
					case Band.B60M: do_band = chk60.Checked; break;
					case Band.B40M: do_band = chk40.Checked; break;
					case Band.B30M: do_band = chk30.Checked; break;
					case Band.B20M: do_band = chk20.Checked; break;
					case Band.B17M: do_band = chk17.Checked; break;
					case Band.B15M: do_band = chk15.Checked; break;
					case Band.B12M: do_band = chk12.Checked; break;
					case Band.B10M:	do_band = chk10.Checked; break;
					case Band.B6M: do_band = chk6.Checked; break;
				}

				if(do_band)
				{
					progress.SetPercent(0.0f);
					Invoke(new MethodInvoker(progress.Show));
					Application.DoEvents();
					console.VFOAFreq = band_freqs[i]+2*console.IFFreq;
					console.VFOBFreq = band_freqs[i];
					console.CalibrateRXImage(band_freqs[i], progress, false);
					if(progress.Text == "") break;
					Thread.Sleep(500);				
				}
			}
			console.VFOAFreq = vfoa;
			console.VFOBFreq = vfob;
			btnCalRXImageAllBands.BackColor = SystemColors.Control;
			grpAllBands.Enabled = true;
			t1.Stop();
			Debug.WriteLine("RX Image Timer: "+t1.Duration);
		}

		public void CallCalFWCRXLevel()
		{
			progress = new Progress("Calibrate RX Level");
			Thread t = new Thread(new ThreadStart(RunCalFWCRXLevel));
			t.Name = "Calibrate RX Level Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;
			t.Start();
		}

		public void RunCalFWCRXLevel()
		{
			double vfoa = console.VFOAFreq;
			double vfob = console.VFOBFreq;
			HiPerfTimer t1 = new HiPerfTimer();
			t1.Start();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
			float[] band_freqs = { 1.85f, 3.75f, 5.3665f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f, 50.11f};

			for(int i=0; i<band_freqs.Length; i++)
			{
				bool do_band = false;
				switch(bands[i])
				{
					case Band.B160M: do_band = chk160.Checked; break;
					case Band.B80M: do_band = chk80.Checked; break;
					case Band.B60M: do_band = chk60.Checked; break;
					case Band.B40M: do_band = chk40.Checked; break;
					case Band.B30M: do_band = chk30.Checked; break;
					case Band.B20M: do_band = chk20.Checked; break;
					case Band.B17M: do_band = chk17.Checked; break;
					case Band.B15M: do_band = chk15.Checked; break;
					case Band.B12M: do_band = chk12.Checked; break;
					case Band.B10M:	do_band = chk10.Checked; break;
					case Band.B6M: do_band = chk6.Checked; break;
				}

				if(do_band)
				{
					progress.SetPercent(0.0f);
					Invoke(new MethodInvoker(progress.Show));
					Application.DoEvents();
					console.VFOAFreq = band_freqs[i];
					console.VFOBFreq = band_freqs[i];
					console.CalibrateLevel(-24.0f, band_freqs[i], progress);
					if(progress.Text == "") break;
					Thread.Sleep(500);				
				}
			}
			console.VFOAFreq = vfoa;
			console.VFOBFreq = vfob;
			btnCalRXLevelAllBands.BackColor = SystemColors.Control;
			grpAllBands.Enabled = true;
			t1.Stop();
			Debug.WriteLine("RX Level Timer: "+t1.Duration);
		}

		public void CallCalFWCTXImage()
		{
			progress = new Progress("Calibrate TX Image");
			Thread t = new Thread(new ThreadStart(RunCalFWCTXImage));
			t.Name = "Calibrate TX Image Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;
			t.Start();
		}	

		public void RunCalFWCTXImage()
		{
			double vfoa = console.VFOAFreq;
			double vfob = console.VFOBFreq;
			HiPerfTimer t1 = new HiPerfTimer();
			t1.Start();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
			float[] band_freqs = { 1.85f, 3.75f, 5.3665f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f, 50.11f};
			for(int i=0; i<band_freqs.Length; i++)
			{
				bool do_band = false;
				switch(bands[i])
				{
					case Band.B160M: do_band = chk160.Checked; break;
					case Band.B80M: do_band = chk80.Checked; break;
					case Band.B60M: do_band = chk60.Checked; break;
					case Band.B40M: do_band = chk40.Checked; break;
					case Band.B30M: do_band = chk30.Checked; break;
					case Band.B20M: do_band = chk20.Checked; break;
					case Band.B17M: do_band = chk17.Checked; break;
					case Band.B15M: do_band = chk15.Checked; break;
					case Band.B12M: do_band = chk12.Checked; break;
					case Band.B10M:	do_band = chk10.Checked; break;
					case Band.B6M: do_band = chk6.Checked; break;
				}

				if(do_band)
				{
					progress.SetPercent(0.0f);
					Invoke(new MethodInvoker(progress.Show));
					Application.DoEvents();
					console.VFOAFreq = band_freqs[i];
					console.VFOBFreq = band_freqs[i];
					console.CalibrateTXImage(band_freqs[i], progress, false);
					if(progress.Text == "") break;
					Thread.Sleep(500);				
				}
			}
			console.VFOAFreq = vfoa;
			console.VFOBFreq = vfob;
			btnCalTXImageAllBands.BackColor = SystemColors.Control;
			grpAllBands.Enabled = true;
			t1.Stop();
			Debug.WriteLine("TX Image Timer: "+t1.Duration);
		}

		public void CallCalFWCTXCarrier()
		{
			progress = new Progress("Calibrate TX Carrier");
			Thread t = new Thread(new ThreadStart(RunCalFWCTXCarrier));
			t.Name = "Run Calibrate TX Carrier Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;
			t.Start();
		}

		public void RunCalFWCTXCarrier()
		{		
			double vfoa = console.VFOAFreq;
			double vfob = console.VFOBFreq;
			HiPerfTimer t1 = new HiPerfTimer();
			t1.Start();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
			float[] band_freqs = { 1.85f, 3.75f, 5.3665f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f, 50.11f};
			for(int i=0; i<band_freqs.Length; i++)
			{
				bool do_band = false;
				switch(bands[i])
				{
					case Band.B160M: do_band = chk160.Checked; break;
					case Band.B80M: do_band = chk80.Checked; break;
					case Band.B60M: do_band = chk60.Checked; break;
					case Band.B40M: do_band = chk40.Checked; break;
					case Band.B30M: do_band = chk30.Checked; break;
					case Band.B20M: do_band = chk20.Checked; break;
					case Band.B17M: do_band = chk17.Checked; break;
					case Band.B15M: do_band = chk15.Checked; break;
					case Band.B12M: do_band = chk12.Checked; break;
					case Band.B10M:	do_band = chk10.Checked; break;
					case Band.B6M: do_band = chk6.Checked; break;
				}

				if(do_band)
				{
					progress.SetPercent(0.0f);
					Invoke(new MethodInvoker(progress.Show));
					Application.DoEvents();
					console.VFOAFreq = band_freqs[i];
					console.VFOBFreq = band_freqs[i];
					console.CalibrateTXCarrier(band_freqs[i], progress, false);
					if(progress.Text == "") break;
					Thread.Sleep(500);				
				}
			}
			console.VFOAFreq = vfoa;
			console.VFOBFreq = vfob;
			btnCalTXCarrierAllBands.BackColor = SystemColors.Control;
			grpAllBands.Enabled = true;
			t1.Stop();
			Debug.WriteLine("TX Carrier Timer: "+t1.Duration);
		}	

		private void CalFWCPABridge()
		{
			double vfoa = console.VFOAFreq;
			double vfob = console.VFOBFreq;
			HiPerfTimer t1 = new HiPerfTimer();
			t1.Start();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
			float[] band_freqs = { 1.85f, 3.75f, 5.3665f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f, 50.11f};
			for(int i=0; i<band_freqs.Length; i++)
			{
				bool do_band = false;
				switch(bands[i])
				{
					case Band.B160M: do_band = chk160.Checked; break;
					case Band.B80M: do_band = chk80.Checked; break;
					case Band.B60M: do_band = chk60.Checked; break;
					case Band.B40M: do_band = chk40.Checked; break;
					case Band.B30M: do_band = chk30.Checked; break;
					case Band.B20M: do_band = chk20.Checked; break;
					case Band.B17M: do_band = chk17.Checked; break;
					case Band.B15M: do_band = chk15.Checked; break;
					case Band.B12M: do_band = chk12.Checked; break;
					case Band.B10M:	do_band = chk10.Checked; break;
					case Band.B6M: do_band = chk6.Checked; break;
				}

				if(do_band)
				{
					console.VFOAFreq = band_freqs[i];
					console.CalibratePABridge(progress);
					if(progress.Text == "") break;
					Thread.Sleep(500);
				}
			}
			console.VFOAFreq = vfoa;
			console.VFOBFreq = vfob;
			btnCalPABridge.BackColor = SystemColors.Control;
			grpAllBands.Enabled = true;
			btnCalPABridge.Enabled = true;
		}

		private void CalibratePAGain()
		{			
			bool[] run = new bool[11];
			run[0] = chk160.Checked;
			run[1] = chk80.Checked;
			run[2] = chk60.Checked;
			run[3] = chk40.Checked;
			run[4] = chk30.Checked;
			run[5] = chk20.Checked;
			run[6] = chk17.Checked;
			run[7] = chk15.Checked;
			run[8] = chk12.Checked;
			run[9] = chk10.Checked;
			run[10] = chk6.Checked;
			console.CalibratePAGain2(progress, run, false);
			btnCalPAPowerAllBands.BackColor = SystemColors.Control;
			grpAllBands.Enabled = true;
		}

		#endregion

		private void btnRestoreFromEEPROM_Click(object sender, System.EventArgs e)
		{
			DialogResult dr = MessageBox.Show("Are you sure you want to read the current EEPROM data into\n"+
				"the database overwriting any current values?",
				"Overwrite database?",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);
			if(dr == DialogResult.No) return;
			
			btnRestoreFromEEPROM.BackColor = console.ButtonSelectedColor;
			btnRestoreFromEEPROM.Enabled = false;
			Thread t = new Thread(new ThreadStart(RestoreFromEEPROM));
			t.Name = "Write To EEPROM Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;
			t.Start();
		}

		private void RestoreFromEEPROM()
		{
			console.FLEX5000RestoreCalData();
			btnRestoreFromEEPROM.BackColor = SystemColors.Control;
			btnRestoreFromEEPROM.Enabled = true;
		}

		private void btnCalPABridge_Click(object sender, System.EventArgs e)
		{
			grpAllBands.Enabled = false;
			btnCalPABridge.Enabled = false;
			btnCalPABridge.BackColor = console.ButtonSelectedColor;
			progress = new Progress("Calibrate PA Bridge");
			Thread t = new Thread(new ThreadStart(CalFWCPABridge));
			t.Name = "Cal PA Bridge Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;
			t.Start();
			progress.Show();
		}
	}
}
