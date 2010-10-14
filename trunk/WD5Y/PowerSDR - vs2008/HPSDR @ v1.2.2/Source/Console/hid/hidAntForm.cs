//=================================================================
// hidAntForm.cs
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

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PowerSDR
{
	public class HIDAntForm : System.Windows.Forms.Form
	{
		#region Variable Declaration

		private Console console;
		private System.Windows.Forms.ComboBoxTS comboRXAnt;
		private System.Windows.Forms.LabelTS lblTX;
        private System.Windows.Forms.ComboBoxTS comboTXAnt;
		private System.Windows.Forms.RadioButtonTS radModeSimple;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.LabelTS lblBand;
		private System.Windows.Forms.ComboBoxTS comboBand;
		private System.Windows.Forms.RadioButtonTS radModeExpert;
		private System.Windows.Forms.LabelTS lblRX1;
		private System.Windows.Forms.GroupBoxTS grpMode;
		private System.Windows.Forms.GroupBoxTS grpAntenna;
		private System.Windows.Forms.GroupBoxTS grpFlexWirePTTOut;
        private System.Windows.Forms.CheckBoxTS chkPTTOutEnable;
		private System.Windows.Forms.TextBoxTS txtStatus;
        private System.Windows.Forms.CheckBoxTS chkLock;
		private System.Windows.Forms.CheckBoxTS chkTX1DelayEnable;
        private System.Windows.Forms.NumericUpDownTS udTX1Delay;
		private System.ComponentModel.IContainer components;

		#endregion

		#region Constructor and Destructor

		public HIDAntForm(Console c)
		{
			InitializeComponent();
			console = c;

			// Set mode first
			ArrayList a = DB.GetVars("HIDAnt");
			a.Sort();

			foreach(string s in a)
			{
				if(s.StartsWith("radModeExpert") && s.IndexOf("True") >= 0)
				{
					radModeExpert.Checked = true;
					break;
				}
			}

			Common.RestoreForm(this, "HIDAnt", false);

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HIDAntForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtStatus = new System.Windows.Forms.TextBoxTS();
            this.grpMode = new System.Windows.Forms.GroupBoxTS();
            this.radModeSimple = new System.Windows.Forms.RadioButtonTS();
            this.radModeExpert = new System.Windows.Forms.RadioButtonTS();
            this.lblBand = new System.Windows.Forms.LabelTS();
            this.comboBand = new System.Windows.Forms.ComboBoxTS();
            this.grpAntenna = new System.Windows.Forms.GroupBoxTS();
            this.chkLock = new System.Windows.Forms.CheckBoxTS();
            this.comboRXAnt = new System.Windows.Forms.ComboBoxTS();
            this.comboTXAnt = new System.Windows.Forms.ComboBoxTS();
            this.lblTX = new System.Windows.Forms.LabelTS();
            this.lblRX1 = new System.Windows.Forms.LabelTS();
            this.grpFlexWirePTTOut = new System.Windows.Forms.GroupBoxTS();
            this.udTX1Delay = new System.Windows.Forms.NumericUpDownTS();
            this.chkTX1DelayEnable = new System.Windows.Forms.CheckBoxTS();
            this.chkPTTOutEnable = new System.Windows.Forms.CheckBoxTS();
            this.grpMode.SuspendLayout();
            this.grpAntenna.SuspendLayout();
            this.grpFlexWirePTTOut.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udTX1Delay)).BeginInit();
            this.SuspendLayout();
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(8, 214);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(264, 20);
            this.txtStatus.TabIndex = 23;
            this.txtStatus.Text = "Simple Mode: Settings are applied to all bands";
            // 
            // grpMode
            // 
            this.grpMode.Controls.Add(this.radModeSimple);
            this.grpMode.Controls.Add(this.radModeExpert);
            this.grpMode.Controls.Add(this.lblBand);
            this.grpMode.Controls.Add(this.comboBand);
            this.grpMode.Location = new System.Drawing.Point(8, 8);
            this.grpMode.Name = "grpMode";
            this.grpMode.Size = new System.Drawing.Size(264, 56);
            this.grpMode.TabIndex = 21;
            this.grpMode.TabStop = false;
            this.grpMode.Text = "Mode";
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
            // lblBand
            // 
            this.lblBand.Image = null;
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
            this.comboBand.DropDownWidth = 56;
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
            // grpAntenna
            // 
            this.grpAntenna.Controls.Add(this.chkLock);
            this.grpAntenna.Controls.Add(this.comboRXAnt);
            this.grpAntenna.Controls.Add(this.comboTXAnt);
            this.grpAntenna.Controls.Add(this.lblTX);
            this.grpAntenna.Controls.Add(this.lblRX1);
            this.grpAntenna.Location = new System.Drawing.Point(8, 72);
            this.grpAntenna.Name = "grpAntenna";
            this.grpAntenna.Size = new System.Drawing.Size(264, 77);
            this.grpAntenna.TabIndex = 20;
            this.grpAntenna.TabStop = false;
            this.grpAntenna.Text = "Antenna";
            // 
            // chkLock
            // 
            this.chkLock.Checked = true;
            this.chkLock.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLock.Image = null;
            this.chkLock.Location = new System.Drawing.Point(192, 38);
            this.chkLock.Name = "chkLock";
            this.chkLock.Size = new System.Drawing.Size(64, 24);
            this.chkLock.TabIndex = 16;
            this.chkLock.Text = "Lock";
            this.toolTip1.SetToolTip(this.chkLock, "Check this box to lock RX1 and TX antenna selections.");
            this.chkLock.CheckedChanged += new System.EventHandler(this.chkLock_CheckedChanged);
            // 
            // comboRXAnt
            // 
            this.comboRXAnt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRXAnt.DropDownWidth = 64;
            this.comboRXAnt.Items.AddRange(new object[] {
            "PA",
            "XVTX/COM",
            "XVRX"});
            this.comboRXAnt.Location = new System.Drawing.Point(8, 40);
            this.comboRXAnt.Name = "comboRXAnt";
            this.comboRXAnt.Size = new System.Drawing.Size(80, 21);
            this.comboRXAnt.TabIndex = 10;
            this.toolTip1.SetToolTip(this.comboRXAnt, "Selects the Main Receiver Antenna");
            this.comboRXAnt.SelectedIndexChanged += new System.EventHandler(this.comboRXAnt_SelectedIndexChanged);
            // 
            // comboTXAnt
            // 
            this.comboTXAnt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTXAnt.DropDownWidth = 64;
            this.comboTXAnt.Items.AddRange(new object[] {
            "PA",
            "XVTX/COM"});
            this.comboTXAnt.Location = new System.Drawing.Point(93, 40);
            this.comboTXAnt.Name = "comboTXAnt";
            this.comboTXAnt.Size = new System.Drawing.Size(80, 21);
            this.comboTXAnt.TabIndex = 12;
            this.toolTip1.SetToolTip(this.comboTXAnt, "Selects the Transmitter Antenna");
            this.comboTXAnt.SelectedIndexChanged += new System.EventHandler(this.comboTXAnt_SelectedIndexChanged);
            // 
            // lblTX
            // 
            this.lblTX.Image = null;
            this.lblTX.Location = new System.Drawing.Point(93, 24);
            this.lblTX.Name = "lblTX";
            this.lblTX.Size = new System.Drawing.Size(64, 16);
            this.lblTX.TabIndex = 13;
            this.lblTX.Text = "Transmit:";
            // 
            // lblRX1
            // 
            this.lblRX1.Image = null;
            this.lblRX1.Location = new System.Drawing.Point(8, 24);
            this.lblRX1.Name = "lblRX1";
            this.lblRX1.Size = new System.Drawing.Size(72, 16);
            this.lblRX1.TabIndex = 11;
            this.lblRX1.Text = "Receive:";
            // 
            // grpFlexWirePTTOut
            // 
            this.grpFlexWirePTTOut.Controls.Add(this.udTX1Delay);
            this.grpFlexWirePTTOut.Controls.Add(this.chkTX1DelayEnable);
            this.grpFlexWirePTTOut.Controls.Add(this.chkPTTOutEnable);
            this.grpFlexWirePTTOut.Location = new System.Drawing.Point(8, 155);
            this.grpFlexWirePTTOut.Name = "grpFlexWirePTTOut";
            this.grpFlexWirePTTOut.Size = new System.Drawing.Size(264, 53);
            this.grpFlexWirePTTOut.TabIndex = 21;
            this.grpFlexWirePTTOut.TabStop = false;
            this.grpFlexWirePTTOut.Text = "FlexWire PTT Out";
            this.grpFlexWirePTTOut.Visible = false;
            // 
            // udTX1Delay
            // 
            this.udTX1Delay.Enabled = false;
            this.udTX1Delay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udTX1Delay.Location = new System.Drawing.Point(184, 23);
            this.udTX1Delay.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.udTX1Delay.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udTX1Delay.Name = "udTX1Delay";
            this.udTX1Delay.Size = new System.Drawing.Size(56, 20);
            this.udTX1Delay.TabIndex = 4;
            this.udTX1Delay.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udTX1Delay.ValueChanged += new System.EventHandler(this.udTX1Delay_ValueChanged);
            // 
            // chkTX1DelayEnable
            // 
            this.chkTX1DelayEnable.Image = null;
            this.chkTX1DelayEnable.Location = new System.Drawing.Point(101, 16);
            this.chkTX1DelayEnable.Name = "chkTX1DelayEnable";
            this.chkTX1DelayEnable.Size = new System.Drawing.Size(77, 32);
            this.chkTX1DelayEnable.TabIndex = 3;
            this.chkTX1DelayEnable.Text = "Delay (ms)";
            this.toolTip1.SetToolTip(this.chkTX1DelayEnable, "When checked, the red RCA plug labeled TX1 will delay before switching on TR tran" +
                    "sitions by the amount selected in milliseconds.");
            this.chkTX1DelayEnable.CheckedChanged += new System.EventHandler(this.chkTX1DelayEnable_CheckedChanged);
            // 
            // chkPTTOutEnable
            // 
            this.chkPTTOutEnable.Checked = true;
            this.chkPTTOutEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPTTOutEnable.Image = null;
            this.chkPTTOutEnable.Location = new System.Drawing.Point(16, 16);
            this.chkPTTOutEnable.Name = "chkPTTOutEnable";
            this.chkPTTOutEnable.Size = new System.Drawing.Size(79, 32);
            this.chkPTTOutEnable.TabIndex = 0;
            this.chkPTTOutEnable.Text = "Enable";
            this.toolTip1.SetToolTip(this.chkPTTOutEnable, "When checked, the red RCA plug labeled TX1 will switch with TR transitions.  This" +
                    " can be used to switch an external linear, transverter, preselector, etc.");
            this.chkPTTOutEnable.CheckedChanged += new System.EventHandler(this.chkRCATX1_CheckedChanged);
            // 
            // HIDAntForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(280, 237);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.grpMode);
            this.Controls.Add(this.grpAntenna);
            this.Controls.Add(this.grpFlexWirePTTOut);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HIDAntForm";
            this.Text = "FLEX-1500 Antenna Selection";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.HIDAntForm_Closing);
            this.grpMode.ResumeLayout(false);
            this.grpAntenna.ResumeLayout(false);
            this.grpFlexWirePTTOut.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udTX1Delay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Properties

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

		public HIDAnt RXAnt
		{
			get	{ return StringToAnt(comboRXAnt.Text); }
			set { comboRXAnt.Text = AntToString(value); }
		}

		public HIDAnt TXAnt
		{
			get { return StringToAnt(comboTXAnt.Text); }
			set { comboTXAnt.Text = AntToString(value); }
		}

		public bool PTTOut
		{
			get{ return chkPTTOutEnable.Checked; }
			set{ chkPTTOutEnable.Checked = value; }
		}

		public bool TX1DelayEnable
		{
			get{return chkTX1DelayEnable.Checked;}
			set{chkTX1DelayEnable.Checked = value;}
		}

        public bool AntLock
        {
            get { return chkLock.Checked; }
            set { chkLock.Checked = value; }
        }

		public uint TX1Delay
		{
			get{return (uint)udTX1Delay.Value;}
			set{udTX1Delay.Value = value;}
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

		private string AntToString(HIDAnt ant)
		{
			string ret_val = "";
			switch(ant)
			{
				case HIDAnt.PA:         ret_val = "PA"; break;
                case HIDAnt.XVTX_COM:   ret_val = "XVTX/COM"; break;
                case HIDAnt.XVRX:       ret_val = "XVRX"; break;
			}
			return ret_val;
		}

		private HIDAnt StringToAnt(string s)
		{
			HIDAnt ant = HIDAnt.PA;
			switch(s)
			{
				case "PA":          ant = HIDAnt.PA; break;
				case "XVTX/COM":    ant = HIDAnt.XVTX_COM; break;
				case "XVRX":        ant = HIDAnt.XVRX; break;
			}
			return ant;
		}

		#endregion

		#region Event Handlers

		private void HIDAntForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
			Common.SaveForm(this, "HIDAnt");
		}

		private void radModeSimple_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radModeSimple.Checked)
			{
				console.CurrentAntMode = AntMode.Simple;

				lblBand.Visible = false;
				comboBand.Visible = false;
				grpFlexWirePTTOut.Visible = true;

				comboRXAnt.Text = AntToString(console.RXAnt1500);
				comboTXAnt.Text = AntToString(console.TXAnt1500);

				if(console.TXBand == Band.B6M)
					comboTXAnt.Enabled = false;

				chkRCATX1_CheckedChanged(this, EventArgs.Empty);

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
				grpFlexWirePTTOut.Visible = true;

				comboBand.Text = BandToString(console.RX1Band);
				comboRXAnt.Text = AntToString(console.RXAnt1500);
				comboTXAnt.Text = AntToString(console.TXAnt1500);

				txtStatus.Text = "Expert Mode: Settings applied only to selected band";
			}
		}

		private void comboBand_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Band band = StringToBand(comboBand.Text);
			if(!radModeSimple.Checked)
			{
				comboRXAnt.Text = AntToString(console.GetRXAnt1500(band));
        		comboTXAnt.Text = AntToString(console.GetTXAnt1500(band));
				chkPTTOutEnable.Checked = console.GetTX1(band);
			}
		}

		private void comboRXAnt_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			console.SetRXAnt1500(StringToBand(comboBand.Text), StringToAnt(comboRXAnt.Text));

			if(chkLock.Checked)
			{
				switch(comboRXAnt.Text)
				{
					case "PA":
					case "XVTX/COM":
						comboTXAnt.Text = comboRXAnt.Text;
						break;
				}
			}
		}

		private void comboTXAnt_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			console.SetTXAnt1500(StringToBand(comboBand.Text), StringToAnt(comboTXAnt.Text));

			if(chkLock.Checked)
				comboRXAnt.Text = comboTXAnt.Text;
		}

		private void chkRCATX1_CheckedChanged(object sender, System.EventArgs e)
		{
			//console.SetTX1(StringToBand(comboBand.Text), chkPTTOutEnable.Checked);
			chkTX1DelayEnable.Enabled = chkPTTOutEnable.Checked;
		}

		private void chkLock_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkLock.Checked)
			{
				switch(comboRXAnt.Text)
				{
					case "PA":
					case "XVTX/COM":
						comboTXAnt.Text = comboRXAnt.Text;
						break;
				}
			}
		}

		private void chkTX1DelayEnable_CheckedChanged(object sender, System.EventArgs e)
		{
			//USBHID.SetAmpTX1DelayEnable(chkTX1DelayEnable.Checked);
			udTX1Delay.Enabled = chkTX1DelayEnable.Checked;
		}

		private void udTX1Delay_ValueChanged(object sender, System.EventArgs e)
		{
            //USBHID.SetAmpTX1Delay((uint)udTX1Delay.Value);
		}

		#endregion
	}
}
