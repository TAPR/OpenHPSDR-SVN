//=================================================================
// wizard.cs
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
	public class SetupWizard : System.Windows.Forms.Form
	{
		#region Variable Declaration

		System.Resources.ResourceManager resource;

		private enum Page
		{
			WELCOME,
			DATABASE,
			MODEL,
			RFE,
			XVTR,
			PA,
			ATU,
			EXT_CLOCK,
			USB,
			SOUND_CARD,
			FINISHED
		}

		bool done;

		bool rfe_present;
		bool xvtr_present;
		bool pa_present;
		bool atu_present;
		bool usb_present;
		bool ext_clock;
		int xvtr_index;
		int pll_mult;
		int sound_card_index;
		float[] gain_by_band;
		Model model;

		Console console;
		private System.Windows.Forms.ButtonTS btnPrevious;
		private System.Windows.Forms.ButtonTS btnNext;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.LabelTS lblMessage1;
		private System.Windows.Forms.RadioButtonTS radYes;
		private System.Windows.Forms.RadioButtonTS radNo;
		private System.Windows.Forms.LabelTS lblMessage2;
		private System.Windows.Forms.ComboBoxTS comboBox1;
		private System.Windows.Forms.LabelTS lblCombo;
		private System.Windows.Forms.ButtonTS btnFinished;
		private System.Windows.Forms.ComboBoxTS comboBox2;
		private System.Windows.Forms.ButtonTS button1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ComboBoxTS comboBox3;
		private System.Windows.Forms.GroupBoxTS groupBox2;
		private System.Windows.Forms.LabelTS lblPAGainByBand10;
		private System.Windows.Forms.NumericUpDownTS udPAGainByBand10;
		private System.Windows.Forms.LabelTS lblPAGainByBand12;
		private System.Windows.Forms.NumericUpDownTS udPAGainByBand12;
		private System.Windows.Forms.LabelTS lblPAGainByBand15;
		private System.Windows.Forms.NumericUpDownTS udPAGainByBand15;
		private System.Windows.Forms.LabelTS lblPAGainByBand17;
		private System.Windows.Forms.NumericUpDownTS udPAGainByBand17;
		private System.Windows.Forms.LabelTS lblPAGainByBand20;
		private System.Windows.Forms.NumericUpDownTS udPAGainByBand20;
		private System.Windows.Forms.LabelTS lblPAGainByBand30;
		private System.Windows.Forms.NumericUpDownTS udPAGainByBand30;
		private System.Windows.Forms.LabelTS lblPAGainByBand40;
		private System.Windows.Forms.NumericUpDownTS udPAGainByBand40;
		private System.Windows.Forms.LabelTS lblPAGainByBand60;
		private System.Windows.Forms.NumericUpDownTS udPAGainByBand60;
		private System.Windows.Forms.LabelTS lblPAGainByBand80;
		private System.Windows.Forms.NumericUpDownTS udPAGainByBand80;
		private System.Windows.Forms.LabelTS lblPAGainByBand160;
		private System.Windows.Forms.NumericUpDownTS udPAGainByBand160;
		private System.Windows.Forms.RadioButtonTS radGenModelDemoNone;
		private System.Windows.Forms.RadioButtonTS radGenModelSoftRock40;
		private System.Windows.Forms.RadioButtonTS radGenModelSDR1000;
		private System.Windows.Forms.GroupBox grpModel;
		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructor and Destructor

		public SetupWizard(Console c, int sound_card_index)
		{
			InitializeComponent();

			console = c;
			done = false;

			resource = new System.Resources.ResourceManager(typeof(SetupWizard));

			CurPage = Page.WELCOME;
			rfe_present = console.RFEPresent;
			xvtr_present = console.XVTRPresent;
			pa_present = console.PAPresent;
			atu_present = console.ATUPresent;
			usb_present = console.USBPresent;
			ext_clock = false;
			xvtr_index = 0;
			pll_mult = 0;

			gain_by_band = new float[10];
			gain_by_band[0] = console.SetupForm.PAGain160;
			gain_by_band[1] = console.SetupForm.PAGain80;
			gain_by_band[2] = console.SetupForm.PAGain60;
			gain_by_band[3] = console.SetupForm.PAGain40;
			gain_by_band[4] = console.SetupForm.PAGain30;
			gain_by_band[5] = console.SetupForm.PAGain20;
			gain_by_band[6] = console.SetupForm.PAGain17;
			gain_by_band[7] = console.SetupForm.PAGain15;
			gain_by_band[8] = console.SetupForm.PAGain12;
			gain_by_band[9] = console.SetupForm.PAGain10;

			model = console.CurrentModel;
			switch(model)
			{
				case Model.SDR1000:
					radGenModelSDR1000.Checked = true;
					break;
				case Model.SOFTROCK40:
					radGenModelSoftRock40.Checked = true;
					break;
				case Model.DEMO:
					radGenModelDemoNone.Checked = true;
					break;
			}

			openFileDialog1.Filter = "PowerSDR Database Files (*.mdb) | *.mdb";

			comboBox3.SelectedIndex = sound_card_index;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SetupWizard));
			this.btnPrevious = new System.Windows.Forms.ButtonTS();
			this.btnNext = new System.Windows.Forms.ButtonTS();
			this.btnFinished = new System.Windows.Forms.ButtonTS();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblMessage1 = new System.Windows.Forms.LabelTS();
			this.radYes = new System.Windows.Forms.RadioButtonTS();
			this.radNo = new System.Windows.Forms.RadioButtonTS();
			this.lblMessage2 = new System.Windows.Forms.LabelTS();
			this.comboBox1 = new System.Windows.Forms.ComboBoxTS();
			this.lblCombo = new System.Windows.Forms.LabelTS();
			this.comboBox2 = new System.Windows.Forms.ComboBoxTS();
			this.button1 = new System.Windows.Forms.ButtonTS();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.comboBox3 = new System.Windows.Forms.ComboBoxTS();
			this.groupBox2 = new System.Windows.Forms.GroupBoxTS();
			this.lblPAGainByBand10 = new System.Windows.Forms.LabelTS();
			this.udPAGainByBand10 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand12 = new System.Windows.Forms.LabelTS();
			this.udPAGainByBand12 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand15 = new System.Windows.Forms.LabelTS();
			this.udPAGainByBand15 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand17 = new System.Windows.Forms.LabelTS();
			this.udPAGainByBand17 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand20 = new System.Windows.Forms.LabelTS();
			this.udPAGainByBand20 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand30 = new System.Windows.Forms.LabelTS();
			this.udPAGainByBand30 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand40 = new System.Windows.Forms.LabelTS();
			this.udPAGainByBand40 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand60 = new System.Windows.Forms.LabelTS();
			this.udPAGainByBand60 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand80 = new System.Windows.Forms.LabelTS();
			this.udPAGainByBand80 = new System.Windows.Forms.NumericUpDownTS();
			this.lblPAGainByBand160 = new System.Windows.Forms.LabelTS();
			this.udPAGainByBand160 = new System.Windows.Forms.NumericUpDownTS();
			this.grpModel = new System.Windows.Forms.GroupBox();
			this.radGenModelDemoNone = new System.Windows.Forms.RadioButtonTS();
			this.radGenModelSoftRock40 = new System.Windows.Forms.RadioButtonTS();
			this.radGenModelSDR1000 = new System.Windows.Forms.RadioButtonTS();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand10)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand12)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand15)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand17)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand20)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand30)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand40)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand60)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand80)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand160)).BeginInit();
			this.grpModel.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnPrevious
			// 
			this.btnPrevious.Image = null;
			this.btnPrevious.Location = new System.Drawing.Point(224, 248);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.TabIndex = 0;
			this.btnPrevious.Text = "Previous";
			this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
			// 
			// btnNext
			// 
			this.btnNext.Image = null;
			this.btnNext.Location = new System.Drawing.Point(312, 248);
			this.btnNext.Name = "btnNext";
			this.btnNext.TabIndex = 1;
			this.btnNext.Text = "Next";
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnFinished
			// 
			this.btnFinished.Enabled = false;
			this.btnFinished.Image = null;
			this.btnFinished.Location = new System.Drawing.Point(400, 248);
			this.btnFinished.Name = "btnFinished";
			this.btnFinished.TabIndex = 2;
			this.btnFinished.Text = "Finish";
			this.btnFinished.Click += new System.EventHandler(this.btnFinished_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(42, 56);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(171, 128);
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Visible = false;
			// 
			// lblMessage1
			// 
			this.lblMessage1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblMessage1.Image = null;
			this.lblMessage1.Location = new System.Drawing.Point(16, 8);
			this.lblMessage1.Name = "lblMessage1";
			this.lblMessage1.Size = new System.Drawing.Size(456, 136);
			this.lblMessage1.TabIndex = 4;
			this.lblMessage1.Text = "lblMessage1";
			// 
			// radYes
			// 
			this.radYes.Image = null;
			this.radYes.Location = new System.Drawing.Point(272, 32);
			this.radYes.Name = "radYes";
			this.radYes.Size = new System.Drawing.Size(48, 16);
			this.radYes.TabIndex = 5;
			this.radYes.Text = "Yes";
			this.radYes.Visible = false;
			this.radYes.CheckedChanged += new System.EventHandler(this.radYes_CheckedChanged);
			// 
			// radNo
			// 
			this.radNo.Image = null;
			this.radNo.Location = new System.Drawing.Point(328, 32);
			this.radNo.Name = "radNo";
			this.radNo.Size = new System.Drawing.Size(48, 16);
			this.radNo.TabIndex = 6;
			this.radNo.Text = "No";
			this.radNo.Visible = false;
			this.radNo.CheckedChanged += new System.EventHandler(this.radNo_CheckedChanged);
			// 
			// lblMessage2
			// 
			this.lblMessage2.Image = null;
			this.lblMessage2.Location = new System.Drawing.Point(8, 192);
			this.lblMessage2.Name = "lblMessage2";
			this.lblMessage2.Size = new System.Drawing.Size(464, 48);
			this.lblMessage2.TabIndex = 7;
			this.lblMessage2.Text = "lblMessage2";
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.DropDownWidth = 136;
			this.comboBox1.Items.AddRange(new object[] {
														   "DEMI144-28FRS",
														   "DEMI144-28 (25w)"});
			this.comboBox1.Location = new System.Drawing.Point(336, 112);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(136, 21);
			this.comboBox1.TabIndex = 8;
			this.comboBox1.Visible = false;
			// 
			// lblCombo
			// 
			this.lblCombo.Image = null;
			this.lblCombo.Location = new System.Drawing.Point(272, 112);
			this.lblCombo.Name = "lblCombo";
			this.lblCombo.Size = new System.Drawing.Size(192, 72);
			this.lblCombo.TabIndex = 9;
			this.lblCombo.Text = "lblCombo";
			// 
			// comboBox2
			// 
			this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox2.DropDownWidth = 56;
			this.comboBox2.Items.AddRange(new object[] {
														   "10",
														   "20"});
			this.comboBox2.Location = new System.Drawing.Point(384, 112);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(56, 21);
			this.comboBox2.TabIndex = 10;
			// 
			// button1
			// 
			this.button1.Image = null;
			this.button1.Location = new System.Drawing.Point(312, 128);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 23);
			this.button1.TabIndex = 11;
			this.button1.Text = "Select File ...";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
			// 
			// comboBox3
			// 
			this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox3.DropDownWidth = 184;
			this.comboBox3.Items.AddRange(new object[] {
														   "M-Audio Delta 44 (PCI)",
														   "PreSonus FireBox (FireWire)",
														   "Edirol FA-66 (FireWire)",
														   "SB Audigy (PCI)",
														   "SB Audigy 2 (PCI)",
														   "SB Audigy 2 ZS (PCI)",
														   "Sound Blaster Extigy (USB)",
														   "Sound Blaster MP3+ (USB)",
														   "Turtle Beach Santa Cruz (PCI)",
														   "Unsupported Card"});
			this.comboBox3.Location = new System.Drawing.Point(264, 104);
			this.comboBox3.MaxDropDownItems = 10;
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new System.Drawing.Size(184, 21);
			this.comboBox3.TabIndex = 12;
			this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lblPAGainByBand10);
			this.groupBox2.Controls.Add(this.udPAGainByBand10);
			this.groupBox2.Controls.Add(this.lblPAGainByBand12);
			this.groupBox2.Controls.Add(this.udPAGainByBand12);
			this.groupBox2.Controls.Add(this.lblPAGainByBand15);
			this.groupBox2.Controls.Add(this.udPAGainByBand15);
			this.groupBox2.Controls.Add(this.lblPAGainByBand17);
			this.groupBox2.Controls.Add(this.udPAGainByBand17);
			this.groupBox2.Controls.Add(this.lblPAGainByBand20);
			this.groupBox2.Controls.Add(this.udPAGainByBand20);
			this.groupBox2.Controls.Add(this.lblPAGainByBand30);
			this.groupBox2.Controls.Add(this.udPAGainByBand30);
			this.groupBox2.Controls.Add(this.lblPAGainByBand40);
			this.groupBox2.Controls.Add(this.udPAGainByBand40);
			this.groupBox2.Controls.Add(this.lblPAGainByBand60);
			this.groupBox2.Controls.Add(this.udPAGainByBand60);
			this.groupBox2.Controls.Add(this.lblPAGainByBand80);
			this.groupBox2.Controls.Add(this.udPAGainByBand80);
			this.groupBox2.Controls.Add(this.lblPAGainByBand160);
			this.groupBox2.Controls.Add(this.udPAGainByBand160);
			this.groupBox2.Location = new System.Drawing.Point(240, 48);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(216, 136);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Gain By Band (dB)";
			this.groupBox2.Visible = false;
			// 
			// lblPAGainByBand10
			// 
			this.lblPAGainByBand10.Image = null;
			this.lblPAGainByBand10.Location = new System.Drawing.Point(112, 112);
			this.lblPAGainByBand10.Name = "lblPAGainByBand10";
			this.lblPAGainByBand10.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand10.TabIndex = 19;
			this.lblPAGainByBand10.Text = "10m:";
			// 
			// udPAGainByBand10
			// 
			this.udPAGainByBand10.DecimalPlaces = 1;
			this.udPAGainByBand10.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   65536});
			this.udPAGainByBand10.Location = new System.Drawing.Point(152, 112);
			this.udPAGainByBand10.Maximum = new System.Decimal(new int[] {
																			 100,
																			 0,
																			 0,
																			 0});
			this.udPAGainByBand10.Minimum = new System.Decimal(new int[] {
																			 390,
																			 0,
																			 0,
																			 65536});
			this.udPAGainByBand10.Name = "udPAGainByBand10";
			this.udPAGainByBand10.Size = new System.Drawing.Size(48, 20);
			this.udPAGainByBand10.TabIndex = 18;
			this.udPAGainByBand10.Value = new System.Decimal(new int[] {
																		   430,
																		   0,
																		   0,
																		   65536});
			// 
			// lblPAGainByBand12
			// 
			this.lblPAGainByBand12.Image = null;
			this.lblPAGainByBand12.Location = new System.Drawing.Point(112, 88);
			this.lblPAGainByBand12.Name = "lblPAGainByBand12";
			this.lblPAGainByBand12.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand12.TabIndex = 17;
			this.lblPAGainByBand12.Text = "12m:";
			// 
			// udPAGainByBand12
			// 
			this.udPAGainByBand12.DecimalPlaces = 1;
			this.udPAGainByBand12.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   65536});
			this.udPAGainByBand12.Location = new System.Drawing.Point(152, 88);
			this.udPAGainByBand12.Maximum = new System.Decimal(new int[] {
																			 100,
																			 0,
																			 0,
																			 0});
			this.udPAGainByBand12.Minimum = new System.Decimal(new int[] {
																			 390,
																			 0,
																			 0,
																			 65536});
			this.udPAGainByBand12.Name = "udPAGainByBand12";
			this.udPAGainByBand12.Size = new System.Drawing.Size(48, 20);
			this.udPAGainByBand12.TabIndex = 16;
			this.udPAGainByBand12.Value = new System.Decimal(new int[] {
																		   474,
																		   0,
																		   0,
																		   65536});
			// 
			// lblPAGainByBand15
			// 
			this.lblPAGainByBand15.Image = null;
			this.lblPAGainByBand15.Location = new System.Drawing.Point(112, 64);
			this.lblPAGainByBand15.Name = "lblPAGainByBand15";
			this.lblPAGainByBand15.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand15.TabIndex = 15;
			this.lblPAGainByBand15.Text = "15m:";
			// 
			// udPAGainByBand15
			// 
			this.udPAGainByBand15.DecimalPlaces = 1;
			this.udPAGainByBand15.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   65536});
			this.udPAGainByBand15.Location = new System.Drawing.Point(152, 64);
			this.udPAGainByBand15.Maximum = new System.Decimal(new int[] {
																			 100,
																			 0,
																			 0,
																			 0});
			this.udPAGainByBand15.Minimum = new System.Decimal(new int[] {
																			 390,
																			 0,
																			 0,
																			 65536});
			this.udPAGainByBand15.Name = "udPAGainByBand15";
			this.udPAGainByBand15.Size = new System.Drawing.Size(48, 20);
			this.udPAGainByBand15.TabIndex = 14;
			this.udPAGainByBand15.Value = new System.Decimal(new int[] {
																		   481,
																		   0,
																		   0,
																		   65536});
			// 
			// lblPAGainByBand17
			// 
			this.lblPAGainByBand17.Image = null;
			this.lblPAGainByBand17.Location = new System.Drawing.Point(112, 40);
			this.lblPAGainByBand17.Name = "lblPAGainByBand17";
			this.lblPAGainByBand17.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand17.TabIndex = 13;
			this.lblPAGainByBand17.Text = "17m:";
			// 
			// udPAGainByBand17
			// 
			this.udPAGainByBand17.DecimalPlaces = 1;
			this.udPAGainByBand17.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   65536});
			this.udPAGainByBand17.Location = new System.Drawing.Point(152, 40);
			this.udPAGainByBand17.Maximum = new System.Decimal(new int[] {
																			 100,
																			 0,
																			 0,
																			 0});
			this.udPAGainByBand17.Minimum = new System.Decimal(new int[] {
																			 390,
																			 0,
																			 0,
																			 65536});
			this.udPAGainByBand17.Name = "udPAGainByBand17";
			this.udPAGainByBand17.Size = new System.Drawing.Size(48, 20);
			this.udPAGainByBand17.TabIndex = 12;
			this.udPAGainByBand17.Value = new System.Decimal(new int[] {
																		   493,
																		   0,
																		   0,
																		   65536});
			// 
			// lblPAGainByBand20
			// 
			this.lblPAGainByBand20.Image = null;
			this.lblPAGainByBand20.Location = new System.Drawing.Point(112, 16);
			this.lblPAGainByBand20.Name = "lblPAGainByBand20";
			this.lblPAGainByBand20.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand20.TabIndex = 11;
			this.lblPAGainByBand20.Text = "20m:";
			// 
			// udPAGainByBand20
			// 
			this.udPAGainByBand20.DecimalPlaces = 1;
			this.udPAGainByBand20.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   65536});
			this.udPAGainByBand20.Location = new System.Drawing.Point(152, 16);
			this.udPAGainByBand20.Maximum = new System.Decimal(new int[] {
																			 100,
																			 0,
																			 0,
																			 0});
			this.udPAGainByBand20.Minimum = new System.Decimal(new int[] {
																			 390,
																			 0,
																			 0,
																			 65536});
			this.udPAGainByBand20.Name = "udPAGainByBand20";
			this.udPAGainByBand20.Size = new System.Drawing.Size(48, 20);
			this.udPAGainByBand20.TabIndex = 10;
			this.udPAGainByBand20.Value = new System.Decimal(new int[] {
																		   483,
																		   0,
																		   0,
																		   65536});
			// 
			// lblPAGainByBand30
			// 
			this.lblPAGainByBand30.Image = null;
			this.lblPAGainByBand30.Location = new System.Drawing.Point(16, 112);
			this.lblPAGainByBand30.Name = "lblPAGainByBand30";
			this.lblPAGainByBand30.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand30.TabIndex = 9;
			this.lblPAGainByBand30.Text = "30m:";
			// 
			// udPAGainByBand30
			// 
			this.udPAGainByBand30.DecimalPlaces = 1;
			this.udPAGainByBand30.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   65536});
			this.udPAGainByBand30.Location = new System.Drawing.Point(56, 112);
			this.udPAGainByBand30.Maximum = new System.Decimal(new int[] {
																			 100,
																			 0,
																			 0,
																			 0});
			this.udPAGainByBand30.Minimum = new System.Decimal(new int[] {
																			 390,
																			 0,
																			 0,
																			 65536});
			this.udPAGainByBand30.Name = "udPAGainByBand30";
			this.udPAGainByBand30.Size = new System.Drawing.Size(48, 20);
			this.udPAGainByBand30.TabIndex = 8;
			this.udPAGainByBand30.Value = new System.Decimal(new int[] {
																		   489,
																		   0,
																		   0,
																		   65536});
			// 
			// lblPAGainByBand40
			// 
			this.lblPAGainByBand40.Image = null;
			this.lblPAGainByBand40.Location = new System.Drawing.Point(16, 88);
			this.lblPAGainByBand40.Name = "lblPAGainByBand40";
			this.lblPAGainByBand40.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand40.TabIndex = 7;
			this.lblPAGainByBand40.Text = "40m:";
			// 
			// udPAGainByBand40
			// 
			this.udPAGainByBand40.DecimalPlaces = 1;
			this.udPAGainByBand40.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   65536});
			this.udPAGainByBand40.Location = new System.Drawing.Point(56, 88);
			this.udPAGainByBand40.Maximum = new System.Decimal(new int[] {
																			 100,
																			 0,
																			 0,
																			 0});
			this.udPAGainByBand40.Minimum = new System.Decimal(new int[] {
																			 390,
																			 0,
																			 0,
																			 65536});
			this.udPAGainByBand40.Name = "udPAGainByBand40";
			this.udPAGainByBand40.Size = new System.Drawing.Size(48, 20);
			this.udPAGainByBand40.TabIndex = 6;
			this.udPAGainByBand40.Value = new System.Decimal(new int[] {
																		   469,
																		   0,
																		   0,
																		   65536});
			// 
			// lblPAGainByBand60
			// 
			this.lblPAGainByBand60.Image = null;
			this.lblPAGainByBand60.Location = new System.Drawing.Point(16, 64);
			this.lblPAGainByBand60.Name = "lblPAGainByBand60";
			this.lblPAGainByBand60.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand60.TabIndex = 5;
			this.lblPAGainByBand60.Text = "60m:";
			// 
			// udPAGainByBand60
			// 
			this.udPAGainByBand60.DecimalPlaces = 1;
			this.udPAGainByBand60.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   65536});
			this.udPAGainByBand60.Location = new System.Drawing.Point(56, 64);
			this.udPAGainByBand60.Maximum = new System.Decimal(new int[] {
																			 100,
																			 0,
																			 0,
																			 0});
			this.udPAGainByBand60.Minimum = new System.Decimal(new int[] {
																			 390,
																			 0,
																			 0,
																			 65536});
			this.udPAGainByBand60.Name = "udPAGainByBand60";
			this.udPAGainByBand60.Size = new System.Drawing.Size(48, 20);
			this.udPAGainByBand60.TabIndex = 4;
			this.udPAGainByBand60.Value = new System.Decimal(new int[] {
																		   474,
																		   0,
																		   0,
																		   65536});
			// 
			// lblPAGainByBand80
			// 
			this.lblPAGainByBand80.Image = null;
			this.lblPAGainByBand80.Location = new System.Drawing.Point(16, 40);
			this.lblPAGainByBand80.Name = "lblPAGainByBand80";
			this.lblPAGainByBand80.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand80.TabIndex = 3;
			this.lblPAGainByBand80.Text = "80m:";
			// 
			// udPAGainByBand80
			// 
			this.udPAGainByBand80.DecimalPlaces = 1;
			this.udPAGainByBand80.Increment = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   65536});
			this.udPAGainByBand80.Location = new System.Drawing.Point(56, 40);
			this.udPAGainByBand80.Maximum = new System.Decimal(new int[] {
																			 100,
																			 0,
																			 0,
																			 0});
			this.udPAGainByBand80.Minimum = new System.Decimal(new int[] {
																			 390,
																			 0,
																			 0,
																			 65536});
			this.udPAGainByBand80.Name = "udPAGainByBand80";
			this.udPAGainByBand80.Size = new System.Drawing.Size(48, 20);
			this.udPAGainByBand80.TabIndex = 2;
			this.udPAGainByBand80.Value = new System.Decimal(new int[] {
																		   480,
																		   0,
																		   0,
																		   65536});
			// 
			// lblPAGainByBand160
			// 
			this.lblPAGainByBand160.Image = null;
			this.lblPAGainByBand160.Location = new System.Drawing.Point(16, 16);
			this.lblPAGainByBand160.Name = "lblPAGainByBand160";
			this.lblPAGainByBand160.Size = new System.Drawing.Size(40, 16);
			this.lblPAGainByBand160.TabIndex = 1;
			this.lblPAGainByBand160.Text = "160m:";
			// 
			// udPAGainByBand160
			// 
			this.udPAGainByBand160.DecimalPlaces = 1;
			this.udPAGainByBand160.Increment = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				65536});
			this.udPAGainByBand160.Location = new System.Drawing.Point(56, 16);
			this.udPAGainByBand160.Maximum = new System.Decimal(new int[] {
																			  100,
																			  0,
																			  0,
																			  0});
			this.udPAGainByBand160.Minimum = new System.Decimal(new int[] {
																			  390,
																			  0,
																			  0,
																			  65536});
			this.udPAGainByBand160.Name = "udPAGainByBand160";
			this.udPAGainByBand160.Size = new System.Drawing.Size(48, 20);
			this.udPAGainByBand160.TabIndex = 0;
			this.udPAGainByBand160.Value = new System.Decimal(new int[] {
																			490,
																			0,
																			0,
																			65536});
			// 
			// grpModel
			// 
			this.grpModel.Controls.Add(this.radGenModelDemoNone);
			this.grpModel.Controls.Add(this.radGenModelSoftRock40);
			this.grpModel.Controls.Add(this.radGenModelSDR1000);
			this.grpModel.Location = new System.Drawing.Point(272, 72);
			this.grpModel.Name = "grpModel";
			this.grpModel.Size = new System.Drawing.Size(120, 104);
			this.grpModel.TabIndex = 20;
			this.grpModel.TabStop = false;
			this.grpModel.Text = "Model";
			this.grpModel.Visible = false;
			// 
			// radGenModelDemoNone
			// 
			this.radGenModelDemoNone.Image = null;
			this.radGenModelDemoNone.Location = new System.Drawing.Point(16, 72);
			this.radGenModelDemoNone.Name = "radGenModelDemoNone";
			this.radGenModelDemoNone.Size = new System.Drawing.Size(88, 24);
			this.radGenModelDemoNone.TabIndex = 5;
			this.radGenModelDemoNone.Text = "Demo/None";
			this.radGenModelDemoNone.CheckedChanged += new System.EventHandler(this.radGenModelDemoNone_CheckedChanged);
			// 
			// radGenModelSoftRock40
			// 
			this.radGenModelSoftRock40.Image = null;
			this.radGenModelSoftRock40.Location = new System.Drawing.Point(16, 48);
			this.radGenModelSoftRock40.Name = "radGenModelSoftRock40";
			this.radGenModelSoftRock40.Size = new System.Drawing.Size(88, 24);
			this.radGenModelSoftRock40.TabIndex = 4;
			this.radGenModelSoftRock40.Text = "Soft Rock 40";
			this.radGenModelSoftRock40.CheckedChanged += new System.EventHandler(this.radGenModelSoftRock40_CheckedChanged);
			// 
			// radGenModelSDR1000
			// 
			this.radGenModelSDR1000.Checked = true;
			this.radGenModelSDR1000.Image = null;
			this.radGenModelSDR1000.Location = new System.Drawing.Point(16, 24);
			this.radGenModelSDR1000.Name = "radGenModelSDR1000";
			this.radGenModelSDR1000.Size = new System.Drawing.Size(88, 24);
			this.radGenModelSDR1000.TabIndex = 3;
			this.radGenModelSDR1000.TabStop = true;
			this.radGenModelSDR1000.Text = "SDR-1000";
			this.radGenModelSDR1000.CheckedChanged += new System.EventHandler(this.radGenModelSDR1000_CheckedChanged);
			// 
			// SetupWizard
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(488, 286);
			this.Controls.Add(this.grpModel);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.comboBox3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.comboBox2);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.lblMessage2);
			this.Controls.Add(this.radNo);
			this.Controls.Add(this.radYes);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.btnFinished);
			this.Controls.Add(this.btnNext);
			this.Controls.Add(this.btnPrevious);
			this.Controls.Add(this.lblCombo);
			this.Controls.Add(this.lblMessage1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SetupWizard";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "PowerSDR Setup Wizard - Welcome";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.SetupWizard_Closing);
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand10)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand12)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand15)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand17)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand20)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand30)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand40)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand60)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand80)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPAGainByBand160)).EndInit();
			this.grpModel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Misc Routines

		private Stream GetResource(string name)
		{
			return this.GetType().Assembly.GetManifestResourceStream(name);
		}

		private void SwitchPage(Page p)
		{
			switch(p)
			{
				case Page.WELCOME:
					this.Text = "PowerSDR Setup Wizard - Welcome";
					btnFinished.Enabled = false;
					btnNext.Enabled = true;
					btnPrevious.Enabled = false;		// first screen			
					button1.Visible = false;
					comboBox1.Visible = false;					
					comboBox2.Visible = false;
					comboBox3.Visible = false;	
					groupBox2.Visible = false;
					lblCombo.Visible = false;
					lblMessage1.Text = "Welcome to the PowerSDR Setup Wizard.  This Setup Wizard is " +
						"intended to simplify the setup process by providing you with an easy-to-use " +
						"question/answer format.  Suggestions to improve this wizard are enouraged " +
						"and can be emailed to support@flex-radio.com or posted on our forums at " +
						"http://www.flex-radio.com/forum (PowerSDR Section).";
					lblMessage2.Text = "Note: More experieced users may perform these setup steps " +
						"manually by closing this Wizard and opening the Setup Form.";
					pictureBox1.Image = null;
					pictureBox1.Visible = false;
					radYes.Visible = false;
					radNo.Visible = false;									
					break;
				case Page.DATABASE:
					this.Text = "PowerSDR Setup Wizard - Database Import";
					btnFinished.Enabled = false;
					btnNext.Enabled = true;
					btnPrevious.Enabled = true;
					button1.Visible = false;
					comboBox1.Visible = false;
					comboBox2.Visible = false;
					comboBox3.Visible = false;
					groupBox2.Visible = false;
					lblCombo.Visible = false;
					grpModel.Visible = false;
					lblMessage1.Text = "Would you like to import a previous database? (Database files from " +
						"versions prior to 0.1.9 are incompatible)";
					lblMessage2.Text = "If you are upgrading from a previous version, you may use this " +
						"opportunity to import your old database settings (Setup, Memories, etc).";
					pictureBox1.Image = null;
					pictureBox1.Visible = false;
					radNo.Select();
					radYes.Visible = true;
					radNo.Visible = true;								
					break;
				case Page.MODEL:
					this.Text = "PowerSDR Setup Wizard - Radio Model";
					btnFinished.Enabled = false;
					btnNext.Enabled = true;
					btnPrevious.Enabled = true;
					button1.Visible = false;
					comboBox1.Visible = false;
					comboBox2.Visible = false;
					comboBox3.Visible = false;
					groupBox2.Visible = false;
					lblCombo.Visible = false;
					grpModel.Visible = true;
					lblMessage1.Text = "Please select the model of the radio you will be using.";
					lblMessage2.Text = "If you are just trying out the software or will be running " +
						"the software without any hardware connected, select the Demo/None option.";
					radGenModelSDR1000_CheckedChanged(this, EventArgs.Empty);
					radGenModelSoftRock40_CheckedChanged(this, EventArgs.Empty);
					radGenModelDemoNone_CheckedChanged(this, EventArgs.Empty);
					pictureBox1.Visible = true;
					radNo.Visible = false;
					radYes.Visible = false;
					break;
				case Page.RFE:
					this.Text = "PowerSDR Setup Wizard - Hardware Setup";
					btnFinished.Enabled = false;
					btnNext.Enabled = true;
					btnPrevious.Enabled = true;					
					button1.Visible = false;					
					comboBox1.Visible = false;
					comboBox2.Visible = false;
					comboBox3.Visible = false;
					groupBox2.Visible = false;
					lblCombo.Visible = true;
					grpModel.Visible = false;
					lblCombo.Text = "Starting in 2005, all of the packages sold by FlexRadio Systems " +
						"include the RFE.";
					lblMessage1.Text = "Does your board stack include the RFE (RF Expansion) board?";
					lblMessage2.Text = "The RF Expansion board improves the radio performance using " +
						"a preamplifier and Ham Band low pass filters.  This board also provides " +
						"connections for other boards such as the 2M Transverter and the Power " +
						"Amplifier.  For more information, see http://www.flex-radio.com.";
					pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.sdr-rfe.jpg"));
					pictureBox1.Visible = true;
					if(rfe_present)
						radYes.Select();
					else
						radNo.Select();
					radYes.Visible = true;
					radNo.Visible = true;					
					break;
				case Page.XVTR:
					this.Text = "PowerSDR Setup Wizard - Hardware Setup";
					btnFinished.Enabled = false;
					btnNext.Enabled = true;
					btnPrevious.Enabled = true;					
					button1.Visible = false;
					comboBox1.Visible = xvtr_present;
					comboBox1.SelectedIndex = xvtr_index;
					comboBox2.Visible = false;
					comboBox3.Visible = false;
					grpModel.Visible = false;
					lblCombo.Text = "Model: ";
					lblCombo.Visible = xvtr_present;
					lblMessage1.Text = "Does your board stack include the Down East Microwave (DEMI) 2M Transverter?";
					lblMessage2.Text = "This Down East Microwave 2M Transverter mounts on top of the " +						"board stack and uses a 28MHz IF to get into the 144-146MHz range.  For more " +						"information, see http://www.flex-radio.com.";
					pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.demi144-28frs.jpg"));
					pictureBox1.Visible = true;
					if(xvtr_present)
						radYes.Select();
					else
						radNo.Select();
					radYes.Visible = true;
					radNo.Visible = true;					
					break;
				case Page.PA:
					this.Text = "PowerSDR Setup Wizard - Hardware Setup";
					btnFinished.Enabled = false;
					btnNext.Enabled = true;
					btnPrevious.Enabled = true;
					button1.Visible = false;					
					comboBox1.Visible = false;
					comboBox2.Visible = false;
					comboBox3.Visible = false;
					groupBox2.Visible = false;
					grpModel.Visible = false;
					lblCombo.Visible = false;
					lblMessage1.Text = "Is the 100W Power Amplifier (PA) included in your hardware configuration?";
					lblMessage2.Text = "The Power Amplifier bumps the output power of the SDR-1000 from " +						"1W up to 100W.  For more information, see http://www.flex-radio.com.";
					pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.sdr-pa100.jpg"));
					pictureBox1.Visible = true;
					if(pa_present)
						radYes.Select();
					else
						radNo.Select();
					radYes.Visible = true;
					radNo.Visible = true;					
					break;
				case Page.ATU:
					this.Text = "PowerSDR Setup Wizard - Hardware Setup";
					btnFinished.Enabled = false;
					btnNext.Enabled = true;
					btnPrevious.Enabled = true;
					button1.Visible = false;					
					comboBox1.Visible = false;
					comboBox2.Visible = false;
					comboBox3.Visible = false;
					groupBox2.Visible = false;
					grpModel.Visible = false;
					lblCombo.Visible = false;
					lblMessage1.Text = "Is the LDG Z-100 Antenna Tuning Unit (ATU) included in your hardware configuration?";
					lblMessage2.Text = "The integrated ATU allows the user to tune coax antennas with an SWR of up to " +						"10:1.  For more information, see http://www.flex-radio.com.";
					pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.sdr-atu.jpg"));
					pictureBox1.Visible = true;
					if(atu_present)
						radYes.Select();
					else
						radNo.Select();
					radYes.Visible = true;
					radNo.Visible = true;					
					break;
				case Page.EXT_CLOCK:
					this.Text = "PowerSDR Setup Wizard - Hardware Setup";
					btnFinished.Enabled = false;
					btnNext.Enabled = true;
					btnPrevious.Enabled = true;					
					button1.Visible = false;
					comboBox1.Visible = false;
					if(pll_mult == 10 || pll_mult == 0)
						comboBox2.SelectedIndex = 0;
					else
						comboBox2.SelectedIndex = 1;	// 20
					comboBox2.Visible = ext_clock;
					comboBox3.Visible = false;
					groupBox2.Visible = false;
					grpModel.Visible = false;
					lblCombo.Visible = ext_clock;
					lblCombo.Text = "Clock Freq (MHz):";
					lblMessage1.Text = "Does your hardware configuration include the External Clock Reference Option?";
					lblMessage2.Text = "The External Clock Reference Option allows the DDS to be synchronized " +
						"with a more stable clock source.  For more information, see http://www.flex-radio.com.";
					pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.clock.jpg"));
					pictureBox1.Visible = true;
					if(ext_clock)
						radYes.Select();
					else
						radNo.Select();
					radYes.Visible = true;
					radNo.Visible = true;				
					break;
				case Page.USB:
					this.Text = "PowerSDR Setup Wizard - Hardware Setup";
					btnFinished.Enabled = false;
					btnNext.Enabled = true;
					btnPrevious.Enabled = true;
					button1.Visible = false;					
					comboBox1.Visible = false;
					comboBox2.Visible = false;
					comboBox3.Visible = false;
					groupBox2.Visible = false;
					grpModel.Visible = false;
					lblCombo.Visible = false;
					lblMessage1.Text = "Is the USB to Parallel adapter included in your hardware configuration?";
					lblMessage2.Text = "The USB to Parallel adapter eliminates the need for a parallel port interface " +						"on your computer.  Unlike other off-the-shelf adapters, the FlexRadio Systems adapter implements " +
						"all the data, status, and control lines for complete integration with existing parallel port " +
						"hardware.  For more information, see http://www.flex-radio.com.";
					pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.sdr-usb.jpg"));
					pictureBox1.Visible = true;
					if(usb_present)
						radYes.Select();
					else
						radNo.Select();
					radYes.Visible = true;
					radNo.Visible = true;	
					break;
				case Page.SOUND_CARD:
					this.Text = "PowerSDR Setup Wizard - Sound Card Setup";
					btnFinished.Enabled = false;
					btnNext.Enabled = true;
					btnPrevious.Enabled = true;
					button1.Visible = false;
					comboBox1.Visible = false;
					comboBox2.Visible = false;
					comboBox3.Visible = true;
					groupBox2.Visible = false;
					grpModel.Visible = false;
					lblCombo.Visible = false;
					lblMessage1.Text = "Please select your sound card";
					lblMessage2.Text = "If you don't see your card in the list, select Unsupported Card.\n" +
						"If using an Unsupported Card, you will need to modify the settings in the Audio "+
						"Tab of the Setup Form when finished with this wizard";
					pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.soundcard.jpg"));
					pictureBox1.Visible = true;
					radYes.Visible = false;
					radNo.Visible = false;		
					break;
				case Page.FINISHED:
					this.Text = "PowerSDR Setup Wizard - Finished";
					btnFinished.Enabled = true;
					btnNext.Enabled = false;
					btnPrevious.Enabled = true;					
					button1.Visible = false;
					comboBox1.Visible = false;					
					comboBox2.Visible = false;
					comboBox3.Visible = false;
					groupBox2.Visible = false;
					grpModel.Visible = false;
					lblCombo.Visible = false;
					lblMessage1.Text = "Setup is now complete.  To run this wizard again, select Setup " +
						"from the main form and click the wizard button.  To close the wizard, click " +
						"the Finish button.";
					lblMessage2.Visible = false;
					pictureBox1.Image = null;
					pictureBox1.Visible = false;
					radYes.Visible = false;
					radNo.Visible = false;					
					break;
			}
		}

		#endregion

		#region Properties

		private Page current_page = Page.WELCOME;	
		private Page CurPage
		{
			get { return current_page; }
			set
			{
				current_page = value;
				SwitchPage(current_page);
			}
		}

		#endregion

		#region Event Handlers

		private void btnNext_Click(object sender, System.EventArgs e)
		{
			switch(current_page)
			{
				case Page.WELCOME:
					CurPage = Page.DATABASE;
					btnNext.Focus();
					break;
				case Page.DATABASE:
					CurPage = Page.MODEL;
					btnNext.Focus();
					break;
				case Page.MODEL:
					if(model == Model.SDR1000)
						CurPage = Page.RFE;
					else CurPage = Page.SOUND_CARD;
					btnNext.Focus();
					break;
				case Page.RFE:
					if(rfe_present)
						CurPage = Page.XVTR;
					else
						CurPage = Page.EXT_CLOCK;
					btnNext.Focus();
					break;
				case Page.XVTR:
					xvtr_index = comboBox1.SelectedIndex;
					CurPage = Page.PA;
					btnNext.Focus();
					break;
				case Page.PA:
					gain_by_band[0] = (float)udPAGainByBand160.Value;
                    gain_by_band[1] = (float)udPAGainByBand80.Value;
					gain_by_band[2] = (float)udPAGainByBand60.Value;
					gain_by_band[3] = (float)udPAGainByBand40.Value;
					gain_by_band[4] = (float)udPAGainByBand30.Value;
					gain_by_band[5] = (float)udPAGainByBand20.Value;
					gain_by_band[6] = (float)udPAGainByBand17.Value;
					gain_by_band[7] = (float)udPAGainByBand15.Value;
					gain_by_band[8] = (float)udPAGainByBand12.Value;
					gain_by_band[9] = (float)udPAGainByBand10.Value;
					CurPage = Page.ATU;
					btnNext.Focus();
					break;
				case Page.ATU:
					CurPage = Page.EXT_CLOCK;
					btnNext.Focus();
					break;
				case Page.EXT_CLOCK:
					if(ext_clock)
					{
						switch(comboBox2.Text)
						{
							case "10":
								pll_mult = 20;
								break;
							case "20":
								pll_mult = 10;
								break;
						}
					}
					CurPage = Page.USB;
					btnNext.Focus();
					break;
				case Page.USB:
					CurPage = Page.SOUND_CARD;
					btnNext.Focus();
					break;
				case Page.SOUND_CARD:
					sound_card_index = comboBox3.SelectedIndex;
					CurPage = Page.FINISHED;
					btnFinished.Focus();
					break;
				case Page.FINISHED:
					break;
			}
		}

		private void btnPrevious_Click(object sender, System.EventArgs e)
		{
			switch(current_page)
			{
				case Page.WELCOME:
					break;
				case Page.DATABASE:
					CurPage = Page.WELCOME;
					btnPrevious.Focus();
					break;
				case Page.MODEL:
					CurPage = Page.DATABASE;
					btnPrevious.Focus();
					break;
				case Page.RFE:
					CurPage = Page.MODEL;
					btnNext.Focus();
					break;
				case Page.XVTR:
					xvtr_index = comboBox1.SelectedIndex;
					CurPage = Page.RFE;
					btnPrevious.Focus();
					break;
				case Page.PA:
					gain_by_band[0] = (float)udPAGainByBand160.Value;
					gain_by_band[1] = (float)udPAGainByBand80.Value;
					gain_by_band[2] = (float)udPAGainByBand60.Value;
					gain_by_band[3] = (float)udPAGainByBand40.Value;
					gain_by_band[4] = (float)udPAGainByBand30.Value;
					gain_by_band[5] = (float)udPAGainByBand20.Value;
					gain_by_band[6] = (float)udPAGainByBand17.Value;
					gain_by_band[7] = (float)udPAGainByBand15.Value;
					gain_by_band[8] = (float)udPAGainByBand12.Value;
					gain_by_band[9] = (float)udPAGainByBand10.Value;
					CurPage = Page.XVTR;
					btnPrevious.Focus();
					break;
				case Page.ATU:
					CurPage = Page.PA;
					btnPrevious.Focus();
					break;
				case Page.EXT_CLOCK:
					if(ext_clock)
					{
						switch(comboBox2.Text)
						{
							case "10":
								pll_mult = 20;
								break;
							case "20":
								pll_mult = 10;
								break;
						}
					}
					if(rfe_present)
						CurPage = Page.ATU;
					else
						CurPage = Page.RFE;
					btnPrevious.Focus();
					break;
				case Page.USB:
					CurPage = Page.EXT_CLOCK;
					btnPrevious.Focus();
					break;
				case Page.SOUND_CARD:
					sound_card_index = comboBox3.SelectedIndex;
					if(model == Model.SDR1000)
						CurPage = Page.USB;
					else CurPage = Page.MODEL;
					btnPrevious.Focus();
					break;
				case Page.FINISHED:
					CurPage = Page.SOUND_CARD;
					btnPrevious.Focus();
					break;
			}
		}

		private void radYes_CheckedChanged(object sender, System.EventArgs e)
		{
			switch(current_page)
			{
				case Page.RFE:
					rfe_present = true;
					break;
				case Page.XVTR:
					xvtr_present = true;
					lblCombo.Visible = true;
					comboBox1.Visible = true;
					break;
				case Page.PA:
					pa_present = true;
					groupBox2.Visible = true;
					break;
				case Page.ATU:
					atu_present = true;
					break;
				case Page.EXT_CLOCK:
					ext_clock = true;
					lblCombo.Visible = true;
					comboBox2.Visible = true;
					break;
				case Page.USB:
					usb_present = true;
					break;
				case Page.DATABASE:
					button1.Visible = true;
					break;
			}
		}

		private void radNo_CheckedChanged(object sender, System.EventArgs e)
		{
			switch(current_page)
			{
				case Page.RFE:
					rfe_present = false;
					break;
				case Page.XVTR:
					xvtr_present = false;
					lblCombo.Visible = false;
					comboBox1.Visible = false;
					break;
				case Page.PA:
					pa_present = false;
					groupBox2.Visible = false;
					break;
				case Page.ATU:
					atu_present = false;
					break;
				case Page.EXT_CLOCK:
					ext_clock = false;
					lblCombo.Visible = false;
					comboBox2.Visible = false;
					break;
				case Page.USB:
					usb_present = false;
					break;
				case Page.DATABASE:
					button1.Visible = false;
					break;
			}
		}

		private void btnFinished_Click(object sender, System.EventArgs e)
		{
			console.SetupForm.RFEPresent = rfe_present;
			console.SetupForm.XVTRPresent = xvtr_present;
			console.SetupForm.PAPresent = pa_present;
			console.SetupForm.USBPresent = usb_present;
			console.SetupForm.ATUPresent = atu_present;

			if(sound_card_index >= 0)
			{
				SoundCard card = SoundCard.FIRST;
				switch(comboBox3.Text)
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
					case "Unsupported Card":
						card = SoundCard.UNSUPPORTED_CARD;
						break;
				}
				console.CurrentSoundCard = card;
			}
				
			if(xvtr_present)
				console.SetupForm.XVTRSelection = xvtr_index;
			if(ext_clock)
				console.SetupForm.PllMult = pll_mult;
			done = true;

			if(pa_present)
			{
				console.SetupForm.PAGain160 = gain_by_band[0];
				console.SetupForm.PAGain80 = gain_by_band[1];
				console.SetupForm.PAGain60 = gain_by_band[2];
				console.SetupForm.PAGain40 = gain_by_band[3];
				console.SetupForm.PAGain30 = gain_by_band[4];
				console.SetupForm.PAGain20 = gain_by_band[5];
				console.SetupForm.PAGain17 = gain_by_band[6];
				console.SetupForm.PAGain15 = gain_by_band[7];
				console.SetupForm.PAGain12 = gain_by_band[8];
				console.SetupForm.PAGain10 = gain_by_band[9];
			}

			console.SetupForm.CurrentModel = model;

			ArrayList a = new ArrayList();
			a.Add("SetupWizard/1");
			DB.SaveVars("State", ref a);

			console.SetupForm.SaveOptions();
			console.SaveState();

			this.Close();
		}

		private void SetupWizard_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(!done)
			{
				DialogResult result = MessageBox.Show("Closing the wizard without finishing will not save results.  " +
					"Do you want to close it anyways?",
					"Wizard Not Complete Warning",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning);

				if(result == DialogResult.No)
					e.Cancel = true;
			}
		}

		private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			CompleteImport();
		}

		private void CompleteImport()
		{
			if(DB.ImportDatabase(openFileDialog1.FileName))
				MessageBox.Show("Database Imported Successfully",
					"Database Imported",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information);

			console.SetupForm.GetTxProfiles();

			console.SetupForm.GetOptions();			// load all database values
			console.GetState();				
			if(console.EQForm != null) console.EQForm.RestoreSettings();
			if(console.XVTRForm != null) console.XVTRForm.RestoreSettings();
			if(console.ProdTestForm != null) console.ProdTestForm.RestoreSettings();

			console.SetupForm.SaveOptions();		// save all database values
			console.SaveState();
			if(console.EQForm != null) console.EQForm.SaveSettings();
			if(console.XVTRForm != null) console.XVTRForm.SaveSettings();
			if(console.ProdTestForm != null) console.ProdTestForm.SaveSettings();
			done = true;
			console.ResetMemForm();
			this.Close();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			string path = Application.StartupPath;
			path = path.Substring(0, path.LastIndexOf("\\"));
			openFileDialog1.InitialDirectory = path;
			openFileDialog1.ShowDialog();
		}

		private void comboBox3_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(comboBox3.Text == "Unsupported Card")
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

		private void radGenModelSDR1000_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radGenModelSDR1000.Checked)
			{
				model = Model.SDR1000;
				if(grpModel.Visible)
					pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.sdr-1000.jpg"));
			}
		}

		private void radGenModelSoftRock40_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radGenModelSoftRock40.Checked)
			{
				model = Model.SOFTROCK40;
				if(grpModel.Visible)
					pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.softrock40.jpg"));
			}
		}

		private void radGenModelDemoNone_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radGenModelDemoNone.Checked)
			{
				model = Model.DEMO;
				if(grpModel.Visible)
					pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.no_hardware.jpg"));
			}
		}

		#endregion		
	}
}
