//=================================================================
// fwcAntForm.cs
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PowerSDR
{
	public class FWCAntForm : System.Windows.Forms.Form
	{
		#region Variable Declaration

		private Console console;
		private System.Windows.Forms.ComboBox comboRX1Ant;
		private System.Windows.Forms.Label lblTX;
		private System.Windows.Forms.ComboBox comboTXAnt;
		private System.Windows.Forms.Label lblRX2;
		private System.Windows.Forms.ComboBox comboRX2Ant;
		private System.Windows.Forms.RadioButtonTS radModeSimple;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label lblBand;
		private System.Windows.Forms.ComboBox comboBand;
		private System.Windows.Forms.RadioButtonTS radModeExpert;
		private System.Windows.Forms.Label lblRX1;
		private System.Windows.Forms.GroupBox grpComplexity;
		private System.Windows.Forms.GroupBox grpAntenna;
		private System.Windows.Forms.GroupBox grpMisc;
		private System.Windows.Forms.GroupBox grpSwitchRelay;
		private System.Windows.Forms.CheckBox chkRCATX1;
		private System.Windows.Forms.CheckBox chkRCATX2;
		private System.Windows.Forms.CheckBox chkRCATX3;
		private System.Windows.Forms.NumericUpDown udPower;
		private System.Windows.Forms.Label lblPower;
		private System.Windows.Forms.Label lblRFGain;
		private System.Windows.Forms.NumericUpDown udRF;
		private System.Windows.Forms.TextBox txtStatus;
		private System.Windows.Forms.CheckBoxTS chkLock;
		private System.Windows.Forms.CheckBoxTS chkRX1Loop;
		private System.ComponentModel.IContainer components;

		#endregion

		#region Constructor and Destructor

		public FWCAntForm(Console c)
		{
			InitializeComponent();
			console = c;
			FWC.GetRX2OK(out rx2_ok);
			RX2OK = rx2_ok;
			Common.RestoreForm(this, "FWCAnt", false);

			if(radModeSimple.Checked)
				radModeSimple_CheckedChanged(this, EventArgs.Empty);
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FWCAntForm));
			this.chkRX1Loop = new System.Windows.Forms.CheckBoxTS();
			this.comboRX1Ant = new System.Windows.Forms.ComboBox();
			this.lblRX1 = new System.Windows.Forms.Label();
			this.lblTX = new System.Windows.Forms.Label();
			this.comboTXAnt = new System.Windows.Forms.ComboBox();
			this.lblRX2 = new System.Windows.Forms.Label();
			this.comboRX2Ant = new System.Windows.Forms.ComboBox();
			this.radModeSimple = new System.Windows.Forms.RadioButtonTS();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.radModeExpert = new System.Windows.Forms.RadioButtonTS();
			this.chkRCATX1 = new System.Windows.Forms.CheckBox();
			this.chkRCATX2 = new System.Windows.Forms.CheckBox();
			this.chkRCATX3 = new System.Windows.Forms.CheckBox();
			this.chkLock = new System.Windows.Forms.CheckBoxTS();
			this.lblBand = new System.Windows.Forms.Label();
			this.comboBand = new System.Windows.Forms.ComboBox();
			this.grpComplexity = new System.Windows.Forms.GroupBox();
			this.grpMisc = new System.Windows.Forms.GroupBox();
			this.udRF = new System.Windows.Forms.NumericUpDown();
			this.lblRFGain = new System.Windows.Forms.Label();
			this.udPower = new System.Windows.Forms.NumericUpDown();
			this.lblPower = new System.Windows.Forms.Label();
			this.grpSwitchRelay = new System.Windows.Forms.GroupBox();
			this.grpAntenna = new System.Windows.Forms.GroupBox();
			this.txtStatus = new System.Windows.Forms.TextBox();
			this.grpComplexity.SuspendLayout();
			this.grpMisc.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udRF)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPower)).BeginInit();
			this.grpSwitchRelay.SuspendLayout();
			this.grpAntenna.SuspendLayout();
			this.SuspendLayout();
			// 
			// chkRX1Loop
			// 
			this.chkRX1Loop.Image = null;
			this.chkRX1Loop.Location = new System.Drawing.Point(8, 64);
			this.chkRX1Loop.Name = "chkRX1Loop";
			this.chkRX1Loop.Size = new System.Drawing.Size(168, 24);
			this.chkRX1Loop.TabIndex = 9;
			this.chkRX1Loop.Text = "Use RX1 Out to RX1 In Loop";
			this.toolTip1.SetToolTip(this.chkRX1Loop, "Check this box to route the main receiver\'s RF path out RX1 Out and back in RX1 I" +
				"n.  For use with external preamps/filters/etc.");
			this.chkRX1Loop.CheckedChanged += new System.EventHandler(this.chkRX1ExtAnt_CheckedChanged);
			// 
			// comboRX1Ant
			// 
			this.comboRX1Ant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboRX1Ant.Items.AddRange(new object[] {
															 "N/C",
															 "ANT 1",
															 "ANT 2",
															 "ANT 3",
															 "RX1 IN"});
			this.comboRX1Ant.Location = new System.Drawing.Point(8, 40);
			this.comboRX1Ant.Name = "comboRX1Ant";
			this.comboRX1Ant.Size = new System.Drawing.Size(64, 21);
			this.comboRX1Ant.TabIndex = 10;
			this.toolTip1.SetToolTip(this.comboRX1Ant, "Selects the Main Receiver Antenna");
			this.comboRX1Ant.SelectedIndexChanged += new System.EventHandler(this.comboRX1Ant_SelectedIndexChanged);
			// 
			// lblRX1
			// 
			this.lblRX1.Location = new System.Drawing.Point(8, 24);
			this.lblRX1.Name = "lblRX1";
			this.lblRX1.Size = new System.Drawing.Size(72, 16);
			this.lblRX1.TabIndex = 11;
			this.lblRX1.Text = "Receiver 1:";
			// 
			// lblTX
			// 
			this.lblTX.Location = new System.Drawing.Point(160, 24);
			this.lblTX.Name = "lblTX";
			this.lblTX.Size = new System.Drawing.Size(64, 16);
			this.lblTX.TabIndex = 13;
			this.lblTX.Text = "Transmit:";
			// 
			// comboTXAnt
			// 
			this.comboTXAnt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTXAnt.Items.AddRange(new object[] {
															"ANT 1",
															"ANT 2",
															"ANT 3"});
			this.comboTXAnt.Location = new System.Drawing.Point(160, 40);
			this.comboTXAnt.Name = "comboTXAnt";
			this.comboTXAnt.Size = new System.Drawing.Size(64, 21);
			this.comboTXAnt.TabIndex = 12;
			this.toolTip1.SetToolTip(this.comboTXAnt, "Selects the Transmitter Antenna");
			this.comboTXAnt.SelectedIndexChanged += new System.EventHandler(this.comboTXAnt_SelectedIndexChanged);
			// 
			// lblRX2
			// 
			this.lblRX2.Location = new System.Drawing.Point(80, 24);
			this.lblRX2.Name = "lblRX2";
			this.lblRX2.Size = new System.Drawing.Size(72, 16);
			this.lblRX2.TabIndex = 15;
			this.lblRX2.Text = "Receiver 2:";
			// 
			// comboRX2Ant
			// 
			this.comboRX2Ant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboRX2Ant.Items.AddRange(new object[] {
															 "N/C",
															 "ANT 1",
															 "RX2 IN",
															 "RX1 Tap"});
			this.comboRX2Ant.Location = new System.Drawing.Point(80, 40);
			this.comboRX2Ant.Name = "comboRX2Ant";
			this.comboRX2Ant.Size = new System.Drawing.Size(72, 21);
			this.comboRX2Ant.TabIndex = 14;
			this.toolTip1.SetToolTip(this.comboRX2Ant, "Selects the second receivers antenna (only available if optional 2nd RX is instal" +
				"led)");
			this.comboRX2Ant.SelectedIndexChanged += new System.EventHandler(this.comboRX2Ant_SelectedIndexChanged);
			// 
			// radModeSimple
			// 
			this.radModeSimple.Checked = true;
			this.radModeSimple.Image = null;
			this.radModeSimple.Location = new System.Drawing.Point(16, 24);
			this.radModeSimple.Name = "radModeSimple";
			this.radModeSimple.Size = new System.Drawing.Size(64, 24);
			this.radModeSimple.TabIndex = 16;
			this.radModeSimple.TabStop = true;
			this.radModeSimple.Text = "Simple";
			this.toolTip1.SetToolTip(this.radModeSimple, "One setting for all bands");
			this.radModeSimple.CheckedChanged += new System.EventHandler(this.radModeSimple_CheckedChanged);
			// 
			// radModeExpert
			// 
			this.radModeExpert.Image = null;
			this.radModeExpert.Location = new System.Drawing.Point(96, 24);
			this.radModeExpert.Name = "radModeExpert";
			this.radModeExpert.Size = new System.Drawing.Size(56, 24);
			this.radModeExpert.TabIndex = 20;
			this.radModeExpert.Text = "Expert";
			this.toolTip1.SetToolTip(this.radModeExpert, "More settings for each individual band");
			this.radModeExpert.CheckedChanged += new System.EventHandler(this.radModeExpert_CheckedChanged);
			// 
			// chkRCATX1
			// 
			this.chkRCATX1.Checked = true;
			this.chkRCATX1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkRCATX1.Location = new System.Drawing.Point(16, 16);
			this.chkRCATX1.Name = "chkRCATX1";
			this.chkRCATX1.Size = new System.Drawing.Size(72, 32);
			this.chkRCATX1.TabIndex = 0;
			this.chkRCATX1.Text = "TX1 (Red)";
			this.toolTip1.SetToolTip(this.chkRCATX1, "When checked, the red RCA plug labeled TX1 will switch with TR transitions.  This" +
				" can be used to switch an external linear, transverter, preselector, etc.");
			this.chkRCATX1.CheckedChanged += new System.EventHandler(this.chkRCATX1_CheckedChanged);
			// 
			// chkRCATX2
			// 
			this.chkRCATX2.Checked = true;
			this.chkRCATX2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkRCATX2.Location = new System.Drawing.Point(96, 16);
			this.chkRCATX2.Name = "chkRCATX2";
			this.chkRCATX2.Size = new System.Drawing.Size(80, 32);
			this.chkRCATX2.TabIndex = 1;
			this.chkRCATX2.Text = "TX2 (White)";
			this.toolTip1.SetToolTip(this.chkRCATX2, "When checked, the white RCA plug labeled TX2 will switch with TR transitions.  Th" +
				"is can be used to switch an external linear, transverter, preselector, etc.");
			this.chkRCATX2.CheckedChanged += new System.EventHandler(this.chkRCATX2_CheckedChanged);
			// 
			// chkRCATX3
			// 
			this.chkRCATX3.Checked = true;
			this.chkRCATX3.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkRCATX3.Location = new System.Drawing.Point(176, 16);
			this.chkRCATX3.Name = "chkRCATX3";
			this.chkRCATX3.Size = new System.Drawing.Size(64, 32);
			this.chkRCATX3.TabIndex = 2;
			this.chkRCATX3.Text = "TX3 (Yellow)";
			this.toolTip1.SetToolTip(this.chkRCATX3, "When checked, the yellow RCA plug labeled TX3 will switch with TR transitions.  T" +
				"his can be used to switch an external linear, transverter, preselector, etc.");
			this.chkRCATX3.CheckedChanged += new System.EventHandler(this.chkRCATX3_CheckedChanged);
			// 
			// chkLock
			// 
			this.chkLock.Image = null;
			this.chkLock.Location = new System.Drawing.Point(192, 64);
			this.chkLock.Name = "chkLock";
			this.chkLock.Size = new System.Drawing.Size(48, 24);
			this.chkLock.TabIndex = 16;
			this.chkLock.Text = "Lock";
			this.toolTip1.SetToolTip(this.chkLock, "Check this box to lock RX1 and TX antenna selections.");
			this.chkLock.CheckedChanged += new System.EventHandler(this.chkLock_CheckedChanged);
			// 
			// lblBand
			// 
			this.lblBand.Location = new System.Drawing.Point(160, 24);
			this.lblBand.Name = "lblBand";
			this.lblBand.Size = new System.Drawing.Size(40, 24);
			this.lblBand.TabIndex = 19;
			this.lblBand.Text = "Band:";
			this.lblBand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblBand.Visible = false;
			// 
			// comboBand
			// 
			this.comboBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBand.Items.AddRange(new object[] {
														   "GEN",
														   "160m",
														   "80m",
														   "60m",
														   "40m",
														   "30m",
														   "20m",
														   "17m",
														   "15m",
														   "12m",
														   "10m",
														   "6m",
														   "2m",
														   "WWV",
														   "VHF0",
														   "VHF1",
														   "VHF2",
														   "VHF3",
														   "VHF4",
														   "VHF5",
														   "VHF6",
														   "VHF7",
														   "VHF8",
														   "VHF9",
														   "VHF10",
														   "VHF11",
														   "VHF12",
														   "VHF13"});
			this.comboBand.Location = new System.Drawing.Point(200, 24);
			this.comboBand.Name = "comboBand";
			this.comboBand.Size = new System.Drawing.Size(56, 21);
			this.comboBand.TabIndex = 18;
			this.comboBand.Visible = false;
			this.comboBand.SelectedIndexChanged += new System.EventHandler(this.comboBand_SelectedIndexChanged);
			// 
			// grpComplexity
			// 
			this.grpComplexity.Controls.Add(this.radModeSimple);
			this.grpComplexity.Controls.Add(this.radModeExpert);
			this.grpComplexity.Controls.Add(this.lblBand);
			this.grpComplexity.Controls.Add(this.comboBand);
			this.grpComplexity.Location = new System.Drawing.Point(8, 8);
			this.grpComplexity.Name = "grpComplexity";
			this.grpComplexity.Size = new System.Drawing.Size(264, 56);
			this.grpComplexity.TabIndex = 21;
			this.grpComplexity.TabStop = false;
			this.grpComplexity.Text = "Complexity";
			// 
			// grpMisc
			// 
			this.grpMisc.Controls.Add(this.udRF);
			this.grpMisc.Controls.Add(this.lblRFGain);
			this.grpMisc.Controls.Add(this.udPower);
			this.grpMisc.Controls.Add(this.lblPower);
			this.grpMisc.Location = new System.Drawing.Point(8, 240);
			this.grpMisc.Name = "grpMisc";
			this.grpMisc.Size = new System.Drawing.Size(264, 56);
			this.grpMisc.TabIndex = 22;
			this.grpMisc.TabStop = false;
			this.grpMisc.Text = "Misc";
			this.grpMisc.Visible = false;
			// 
			// udRF
			// 
			this.udRF.Location = new System.Drawing.Point(200, 24);
			this.udRF.Maximum = new System.Decimal(new int[] {
																 120,
																 0,
																 0,
																 0});
			this.udRF.Minimum = new System.Decimal(new int[] {
																 20,
																 0,
																 0,
																 -2147483648});
			this.udRF.Name = "udRF";
			this.udRF.Size = new System.Drawing.Size(40, 20);
			this.udRF.TabIndex = 3;
			this.udRF.Value = new System.Decimal(new int[] {
															   90,
															   0,
															   0,
															   0});
			this.udRF.LostFocus += new System.EventHandler(this.udRF_LostFocus);
			this.udRF.ValueChanged += new System.EventHandler(this.udRF_ValueChanged);
			// 
			// lblRFGain
			// 
			this.lblRFGain.Location = new System.Drawing.Point(128, 24);
			this.lblRFGain.Name = "lblRFGain";
			this.lblRFGain.Size = new System.Drawing.Size(80, 23);
			this.lblRFGain.TabIndex = 2;
			this.lblRFGain.Text = "AGC-T (dB):";
			// 
			// udPower
			// 
			this.udPower.Location = new System.Drawing.Point(72, 24);
			this.udPower.Name = "udPower";
			this.udPower.Size = new System.Drawing.Size(40, 20);
			this.udPower.TabIndex = 0;
			this.udPower.Value = new System.Decimal(new int[] {
																  100,
																  0,
																  0,
																  0});
			this.udPower.LostFocus += new System.EventHandler(this.udPower_LostFocus);
			this.udPower.ValueChanged += new System.EventHandler(this.udPower_ValueChanged);
			// 
			// lblPower
			// 
			this.lblPower.Location = new System.Drawing.Point(16, 24);
			this.lblPower.Name = "lblPower";
			this.lblPower.Size = new System.Drawing.Size(64, 16);
			this.lblPower.TabIndex = 1;
			this.lblPower.Text = "Power (W):";
			// 
			// grpSwitchRelay
			// 
			this.grpSwitchRelay.Controls.Add(this.chkRCATX3);
			this.grpSwitchRelay.Controls.Add(this.chkRCATX2);
			this.grpSwitchRelay.Controls.Add(this.chkRCATX1);
			this.grpSwitchRelay.Location = new System.Drawing.Point(8, 176);
			this.grpSwitchRelay.Name = "grpSwitchRelay";
			this.grpSwitchRelay.Size = new System.Drawing.Size(264, 56);
			this.grpSwitchRelay.TabIndex = 21;
			this.grpSwitchRelay.TabStop = false;
			this.grpSwitchRelay.Text = "Switch Relay with TR";
			this.grpSwitchRelay.Visible = false;
			// 
			// grpAntenna
			// 
			this.grpAntenna.Controls.Add(this.chkLock);
			this.grpAntenna.Controls.Add(this.comboRX1Ant);
			this.grpAntenna.Controls.Add(this.chkRX1Loop);
			this.grpAntenna.Controls.Add(this.comboRX2Ant);
			this.grpAntenna.Controls.Add(this.lblRX2);
			this.grpAntenna.Controls.Add(this.comboTXAnt);
			this.grpAntenna.Controls.Add(this.lblTX);
			this.grpAntenna.Controls.Add(this.lblRX1);
			this.grpAntenna.Location = new System.Drawing.Point(8, 72);
			this.grpAntenna.Name = "grpAntenna";
			this.grpAntenna.Size = new System.Drawing.Size(264, 96);
			this.grpAntenna.TabIndex = 20;
			this.grpAntenna.TabStop = false;
			this.grpAntenna.Text = "Antenna";
			// 
			// txtStatus
			// 
			this.txtStatus.Location = new System.Drawing.Point(8, 304);
			this.txtStatus.Name = "txtStatus";
			this.txtStatus.ReadOnly = true;
			this.txtStatus.Size = new System.Drawing.Size(264, 20);
			this.txtStatus.TabIndex = 23;
			this.txtStatus.Text = "Simple Mode: Settings are applied to all bands";
			// 
			// FWCAntForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(280, 326);
			this.Controls.Add(this.txtStatus);
			this.Controls.Add(this.grpComplexity);
			this.Controls.Add(this.grpAntenna);
			this.Controls.Add(this.grpSwitchRelay);
			this.Controls.Add(this.grpMisc);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FWCAntForm";
			this.Text = "FLEX-5000 Antenna Selection";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FWCAntForm_Closing);
			this.grpComplexity.ResumeLayout(false);
			this.grpMisc.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udRF)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPower)).EndInit();
			this.grpSwitchRelay.ResumeLayout(false);
			this.grpAntenna.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties

		private bool rx2_ok = false;
		public bool RX2OK
		{
			get { return rx2_ok; }
			set
			{
				rx2_ok = value;
				comboRX2Ant.Enabled = value;
			}
		}

		public AntMode CurrentAntMode
		{
			get
			{ 
				if(radModeSimple.Checked) return AntMode.Simple;
				else /*if(radModeExpert.Checked)*/ return AntMode.Expert;
			}
			set
			{
				switch(value)
				{
					case AntMode.Simple:
						radModeSimple.Checked = true;
						break;
					case AntMode.Expert:
						radModeExpert.Checked = true;
						break;
				}
			}
		}

		public FWCAnt RX1Ant
		{
			get	{ return StringToAnt(comboRX1Ant.Text); }
			set { comboRX1Ant.Text = AntToString(value); }
		}

		public FWCAnt RX2Ant
		{
			get { return StringToAnt(comboRX2Ant.Text); }
			set { comboRX2Ant.Text = AntToString(value); }
		}

		public FWCAnt TXAnt
		{
			get { return StringToAnt(comboTXAnt.Text); }
			set { comboTXAnt.Text = AntToString(value); }
		}

		public bool RX1Loop
		{
			get { return chkRX1Loop.Checked; }
			set { chkRX1Loop.Checked = value; }
		}

		public bool RCATX1
		{
			get{return chkRCATX1.Checked;}
			set{chkRCATX1.Checked = value;}
		}

		public bool RCATX2
		{
			get{return chkRCATX2.Checked;}
			set{chkRCATX2.Checked = value;}
		}

		public bool RCATX3
		{
			get{return chkRCATX3.Checked;}
			set{chkRCATX3.Checked = value;}
		}

		#endregion

		#region Misc Routines

		public void SetBand(Band b)
		{
			comboBand.Text = BandToString(b);
		}

		private string BandToString(Band b)
		{
			string ret_val = "";
			switch(b)
			{
				case Band.GEN: ret_val = "GEN"; break;
				case Band.B160M: ret_val = "160m"; break;
				case Band.B80M: ret_val = "80m"; break;
				case Band.B60M: ret_val = "60m"; break;
				case Band.B40M: ret_val = "40m"; break;
				case Band.B30M: ret_val = "30m"; break;
				case Band.B20M: ret_val = "20m"; break;
				case Band.B17M: ret_val = "17m"; break;
				case Band.B15M: ret_val = "15m"; break;
				case Band.B12M: ret_val = "12m"; break;
				case Band.B10M: ret_val = "10m"; break;
				case Band.B6M: ret_val = "6m"; break;
				case Band.B2M: ret_val = "2m"; break;
				case Band.WWV: ret_val = "WWV"; break;
				case Band.VHF0: ret_val = "VHF0"; break;
				case Band.VHF1: ret_val = "VHF1"; break;
				case Band.VHF2: ret_val = "VHF2"; break;
				case Band.VHF3: ret_val = "VHF3"; break;
				case Band.VHF4: ret_val = "VHF4"; break;
				case Band.VHF5: ret_val = "VHF5"; break;
				case Band.VHF6: ret_val = "VHF6"; break;
				case Band.VHF7: ret_val = "VHF7"; break;
				case Band.VHF8: ret_val = "VHF8"; break;
				case Band.VHF9: ret_val = "VHF9"; break;
				case Band.VHF10: ret_val = "VHF10"; break;
				case Band.VHF11: ret_val = "VHF11"; break;
				case Band.VHF12: ret_val = "VHF12"; break;
				case Band.VHF13: ret_val = "VHF13"; break;
			}
			return ret_val;
		}

		private Band StringToBand(string s)
		{
			Band b = Band.GEN;
			switch(s)
			{
				case "GEN": b = Band.GEN; break;
				case "160m": b = Band.B160M; break;
				case "80m": b = Band.B80M; break;
				case "60m": b = Band.B60M; break;
				case "40m": b = Band.B40M; break;
				case "30m": b = Band.B30M; break;
				case "20m": b = Band.B20M; break;
				case "17m": b = Band.B17M; break;
				case "15m": b = Band.B15M; break;
				case "12m": b = Band.B12M; break;
				case "10m": b = Band.B10M; break;
				case "6m": b = Band.B6M; break;
				case "2m": b = Band.B2M; break;
				case "WWV": b = Band.WWV; break;
				case "VHF0": b = Band.VHF0; break;
				case "VHF1": b = Band.VHF1; break;
				case "VHF2": b = Band.VHF2; break;
				case "VHF3": b = Band.VHF3; break;
				case "VHF4": b = Band.VHF4; break;
				case "VHF5": b = Band.VHF5; break;
				case "VHF6": b = Band.VHF6; break;
				case "VHF7": b = Band.VHF7; break;
				case "VHF8": b = Band.VHF8; break;
				case "VHF9": b = Band.VHF9; break;
				case "VHF10": b = Band.VHF10; break;
				case "VHF11": b = Band.VHF11; break;
				case "VHF12": b = Band.VHF12; break;
				case "VHF13": b = Band.VHF13; break;
			}
			return b;
		}

		private string AntToString(FWCAnt ant)
		{
			string ret_val = "";
			switch(ant)
			{
				case FWCAnt.NC: ret_val = "N/C"; break;
				case FWCAnt.ANT1: ret_val = "ANT 1"; break;
				case FWCAnt.ANT2: ret_val = "ANT 2"; break;
				case FWCAnt.ANT3: ret_val = "ANT 3"; break;
				case FWCAnt.RX1IN: ret_val = "RX1 IN"; break;
				case FWCAnt.RX2IN: ret_val = "RX2 IN"; break;
				case FWCAnt.RX1TAP: ret_val = "RX1 Tap"; break;
			}
			return ret_val;
		}

		private FWCAnt StringToAnt(string s)
		{
			FWCAnt ant = FWCAnt.ANT1;
			switch(s)
			{
				case "N/C": ant = FWCAnt.NC; break;
				case "ANT 1": ant = FWCAnt.ANT1; break;
				case "ANT 2": ant = FWCAnt.ANT2; break;
				case "ANT 3": ant = FWCAnt.ANT3; break;
				case "RX1 IN": ant = FWCAnt.RX1IN; break;
				case "RX2 IN": ant = FWCAnt.RX2IN; break;
				case "RX1 Tap": ant = FWCAnt.RX1TAP; break;
			}
			return ant;
		}

		#endregion

		#region Event Handlers

		private void FWCAntForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
			Common.SaveForm(this, "FWCAnt");
		}

		private void chkRX1ExtAnt_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeSimple.Checked)
			{
				for(int i=0; i<(int)Band.LAST; i++)
					console.SetRX1Loop((Band)i, chkRX1Loop.Checked);
			}
			else
			{
				console.SetRX1Loop(StringToBand(comboBand.Text), chkRX1Loop.Checked);
			}
		}

		private void radModeSimple_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeSimple.Checked)
			{
				console.CurrentAntMode = AntMode.Simple;

				lblBand.Visible = false;
				comboBand.Visible = false;
				grpSwitchRelay.Visible = true;
				grpMisc.Visible = false;

				comboRX1Ant.Text = AntToString(console.RX1Ant);
				comboRX2Ant.Text = AntToString(console.RX2Ant);
				comboTXAnt.Text = AntToString(console.TXAnt);
				chkRX1Loop.Checked = console.RX1Loop;

				if(console.CurrentBand == Band.B6M)
					comboTXAnt.Enabled = false;

				chkRCATX1_CheckedChanged(this, EventArgs.Empty);
				chkRCATX2_CheckedChanged(this, EventArgs.Empty);
				chkRCATX3_CheckedChanged(this, EventArgs.Empty);

				txtStatus.Text = "Simple Mode: Settings are applied to all bands";
			}
		}

		private void radModeExpert_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeExpert.Checked)
			{
				console.CurrentAntMode = AntMode.Expert;

				lblBand.Visible = true;
				comboBand.Visible = true;
				grpSwitchRelay.Visible = true;
				grpMisc.Visible = true;
				comboTXAnt.Enabled = true;

				comboBand.Text = BandToString(console.CurrentBand);

				txtStatus.Text = "Expert Mode: Settings applied only to selected band";
			}
		}

		private void comboBand_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Band band = StringToBand(comboBand.Text);
			if(!radModeSimple.Checked)
			{				
				switch(band)
				{
					case Band.B6M:
						if(comboTXAnt.Items.Contains("ANT 1"))
							comboTXAnt.Items.Remove("ANT 1");
						if(comboTXAnt.Items.Contains("ANT 2"))
							comboTXAnt.Items.Remove("ANT 2");
						break;
					default:
						if(!comboTXAnt.Items.Contains("ANT 2"))
							comboTXAnt.Items.Insert(0, "ANT 2");
						if(!comboTXAnt.Items.Contains("ANT 1"))
							comboTXAnt.Items.Insert(0, "ANT 1");
						break;
				}
				comboRX1Ant.Text = AntToString(console.GetRX1Ant(band));
				comboRX2Ant.Text = AntToString(console.GetRX2Ant(band));
				if(band == Band.B6M) comboTXAnt.Text = "ANT 3";
				else comboTXAnt.Text = AntToString(console.GetTXAnt(band));
				chkRX1Loop.Checked = console.GetRX1Loop(band);
				chkRCATX1.Checked = console.GetTX1(band);
				chkRCATX2.Checked = console.GetTX2(band);
				chkRCATX3.Checked = console.GetTX3(band);
				udPower.Value = console.GetPower(band);
				udRF.Value = console.GetRFGain(band);
			}
			else
			{
				switch(band)
				{
					case Band.B6M:
						comboTXAnt.Text = AntToString(FWCAnt.ANT3);
						comboTXAnt.Enabled = false;
						break;
					default:
						comboTXAnt.Text = AntToString(console.TXAnt);
						comboTXAnt.Enabled = true;
						break;
				}
			}
		}

		private void comboRX1Ant_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(comboRX1Ant.Text == "RX1 IN")
				chkRX1Loop.Enabled = false;
			else
				chkRX1Loop.Enabled = true;

			console.SetRX1Ant(StringToBand(comboBand.Text), StringToAnt(comboRX1Ant.Text));
			
			/*string save_rx2 = comboRX2Ant.Text;
			comboRX2Ant.Items.Clear();
			comboRX2Ant.Items.Add(comboRX1Ant.Text);
			comboRX2Ant.Items.Add("RX2 IN");
			switch(save_rx2)
			{
				case "ANT 1":
				case "ANT 2":
				case "ANT 3":
				case "RX1 IN":
					comboRX2Ant.Text = comboRX1Ant.Text;
					break;
				default:
					comboRX2Ant.Text = "RX2 IN"; // RX2 In
					break;
			}*/

			if(chkLock.Checked)
			{
				switch(comboRX1Ant.Text)
				{
					case "ANT 1":
					case "ANT 2":
					case "ANT 3":
						comboTXAnt.Text = comboRX1Ant.Text;
						break;
				}
			}
		}

		private void comboRX2Ant_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			console.SetRX2Ant(StringToBand(comboBand.Text), StringToAnt(comboRX2Ant.Text));
		}

		private void comboTXAnt_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch(StringToBand(comboBand.Text))
			{
				case Band.B6M: // do nothing
					break;
				default:
					console.SetTXAnt(StringToBand(comboBand.Text), StringToAnt(comboTXAnt.Text));
					break;
			}

			if(chkLock.Checked)
				comboRX1Ant.Text = comboTXAnt.Text;
		}

		private void chkRCATX1_CheckedChanged(object sender, System.EventArgs e)
		{
			console.SetTX1(StringToBand(comboBand.Text), chkRCATX1.Checked);
		}

		private void chkRCATX2_CheckedChanged(object sender, System.EventArgs e)
		{
			console.SetTX2(StringToBand(comboBand.Text), chkRCATX2.Checked);
		}

		private void chkRCATX3_CheckedChanged(object sender, System.EventArgs e)
		{
			console.SetTX3(StringToBand(comboBand.Text), chkRCATX3.Checked);
		}

		private void udPower_ValueChanged(object sender, System.EventArgs e)
		{
			if(!radModeSimple.Checked)
				console.SetPower(StringToBand(comboBand.Text), (int)udPower.Value);
		}

		private void udRF_ValueChanged(object sender, System.EventArgs e)
		{
			if(!radModeSimple.Checked)
				console.SetRFGain(StringToBand(comboBand.Text), (int)udRF.Value);
		}

		#endregion

		private void chkLock_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkLock.Checked)
			{
				switch(comboRX1Ant.Text)
				{
					case "ANT 1":
					case "ANT 2":
					case "ANT 3":
						comboTXAnt.Text = comboRX1Ant.Text;
						break;
				}
			}
		}

		private void udPower_LostFocus(object sender, EventArgs e)
		{
			udPower_ValueChanged(sender, e);
		}

		private void udRF_LostFocus(object sender, EventArgs e)
		{
			udRF_ValueChanged(sender, e);
		}
	}
}
