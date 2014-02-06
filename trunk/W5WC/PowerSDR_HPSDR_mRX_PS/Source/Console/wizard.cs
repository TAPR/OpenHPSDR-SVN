//=================================================================
// wizard.cs
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
using System.IO;
using System.Windows.Forms;

namespace PowerSDR
{
	public class SetupWizard : Form
	{
		#region Variable Declaration

		System.Resources.ResourceManager resource;

		private enum Page
		{
			WELCOME,
			DATABASE,
			MODEL,
 			XVTR,
			PA,
			ATU,
			EXT_CLOCK,
			USB,
            HPSDR_HARDWARE_SELECT, 
            SOUND_CARD,
            REGION,
			FINISHED
		}

		bool done;

 		bool xvtr_present;
		bool pa_present;
		bool atu_present;
		bool usb_present;
		bool ext_clock;
		int xvtr_index;
		int pll_mult;
		int sound_card_index;
        int region_index;
		float[] gain_by_band;
		Model model;
        bool alex_present = false;
        bool mercury_present = false;
        bool penelope_present = false;
        bool pennylane_present = false;
        bool excalibur_present = false;

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
		private System.Windows.Forms.GroupBoxTS grpModel;
		private System.Windows.Forms.RadioButtonTS radGenModelFLEX5000;
        private System.Windows.Forms.GroupBoxTS groupBoxHPSDR_Hardware;
        private System.Windows.Forms.CheckBoxTS chkMercury;
        private System.Windows.Forms.CheckBoxTS chkPenny;
        private System.Windows.Forms.CheckBoxTS chkAlex;
        private RadioButtonTS radGenModelHPSDR;
        private RadioButtonTS radGenModelHermes;
        private CheckBoxTS chkPennyLane;
        private CheckBoxTS chkExcalibur;
        private RadioButtonTS radOzy;
        private RadioButtonTS radMetis;
        private ComboBoxTS comboBox10;
        private RadioButtonTS radGenModelANAN100D;
        private RadioButtonTS radGenModelANAN100;
        private RadioButtonTS radGenModelANAN10;
        private System.ComponentModel.Container components = null;

		#endregion

		#region Constructor and Destructor

		public SetupWizard(Console c, int sound_card_index)
		{
			InitializeComponent();

			console = c;
			done = false;

			resource = new System.Resources.ResourceManager(typeof(SetupWizard));
  //          rfe_present = console.RFEPresent;
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
				case Model.FLEX3000:
					radGenModelFLEX5000.Text = "FLEX-3000";
					//radGenModelFLEX5000.Checked = true;
					break;
                case Model.HPSDR:
                    radGenModelHPSDR.Checked = true;
                    break;
                case Model.HERMES:
                    radGenModelHermes.Checked = true;
                    break;
                case Model.ANAN10:
                    radGenModelANAN10.Checked = true;
                    break;
                case Model.ANAN100:
                    radGenModelANAN100.Checked = true;
                    break;
                case Model.ANAN100D:
                    radGenModelANAN100D.Checked = true;
                    break;
            }

			CurPage = Page.WELCOME;
			btnNext_Click(this, EventArgs.Empty);

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupWizard));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.grpModel = new System.Windows.Forms.GroupBoxTS();
            this.radGenModelANAN100D = new System.Windows.Forms.RadioButtonTS();
            this.radGenModelANAN100 = new System.Windows.Forms.RadioButtonTS();
            this.radGenModelANAN10 = new System.Windows.Forms.RadioButtonTS();
            this.radGenModelHermes = new System.Windows.Forms.RadioButtonTS();
            this.radGenModelHPSDR = new System.Windows.Forms.RadioButtonTS();
            this.radGenModelFLEX5000 = new System.Windows.Forms.RadioButtonTS();
            this.radGenModelDemoNone = new System.Windows.Forms.RadioButtonTS();
            this.radGenModelSoftRock40 = new System.Windows.Forms.RadioButtonTS();
            this.radGenModelSDR1000 = new System.Windows.Forms.RadioButtonTS();
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
            this.udPAGainByBand30 = new System.Windows.Forms.NumericUpDownTS();
            this.lblPAGainByBand40 = new System.Windows.Forms.LabelTS();
            this.udPAGainByBand40 = new System.Windows.Forms.NumericUpDownTS();
            this.lblPAGainByBand60 = new System.Windows.Forms.LabelTS();
            this.udPAGainByBand60 = new System.Windows.Forms.NumericUpDownTS();
            this.lblPAGainByBand80 = new System.Windows.Forms.LabelTS();
            this.udPAGainByBand80 = new System.Windows.Forms.NumericUpDownTS();
            this.lblPAGainByBand160 = new System.Windows.Forms.LabelTS();
            this.udPAGainByBand160 = new System.Windows.Forms.NumericUpDownTS();
            this.lblPAGainByBand30 = new System.Windows.Forms.LabelTS();
            this.comboBox3 = new System.Windows.Forms.ComboBoxTS();
            this.button1 = new System.Windows.Forms.ButtonTS();
            this.comboBox2 = new System.Windows.Forms.ComboBoxTS();
            this.comboBox1 = new System.Windows.Forms.ComboBoxTS();
            this.lblMessage2 = new System.Windows.Forms.LabelTS();
            this.radNo = new System.Windows.Forms.RadioButtonTS();
            this.radYes = new System.Windows.Forms.RadioButtonTS();
            this.btnFinished = new System.Windows.Forms.ButtonTS();
            this.btnNext = new System.Windows.Forms.ButtonTS();
            this.btnPrevious = new System.Windows.Forms.ButtonTS();
            this.groupBoxHPSDR_Hardware = new System.Windows.Forms.GroupBoxTS();
            this.radOzy = new System.Windows.Forms.RadioButtonTS();
            this.radMetis = new System.Windows.Forms.RadioButtonTS();
            this.chkExcalibur = new System.Windows.Forms.CheckBoxTS();
            this.chkPennyLane = new System.Windows.Forms.CheckBoxTS();
            this.chkAlex = new System.Windows.Forms.CheckBoxTS();
            this.chkPenny = new System.Windows.Forms.CheckBoxTS();
            this.chkMercury = new System.Windows.Forms.CheckBoxTS();
            this.comboBox10 = new System.Windows.Forms.ComboBoxTS();
            this.lblMessage1 = new System.Windows.Forms.LabelTS();
            this.lblCombo = new System.Windows.Forms.LabelTS();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.grpModel.SuspendLayout();
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
            this.groupBoxHPSDR_Hardware.SuspendLayout();
            this.SuspendLayout();
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
            // openFileDialog1
            // 
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // grpModel
            // 
            this.grpModel.Controls.Add(this.radGenModelANAN100D);
            this.grpModel.Controls.Add(this.radGenModelANAN100);
            this.grpModel.Controls.Add(this.radGenModelANAN10);
            this.grpModel.Controls.Add(this.radGenModelHermes);
            this.grpModel.Controls.Add(this.radGenModelHPSDR);
            this.grpModel.Controls.Add(this.radGenModelFLEX5000);
            this.grpModel.Controls.Add(this.radGenModelDemoNone);
            this.grpModel.Controls.Add(this.radGenModelSoftRock40);
            this.grpModel.Controls.Add(this.radGenModelSDR1000);
            this.grpModel.Location = new System.Drawing.Point(256, 24);
            this.grpModel.Name = "grpModel";
            this.grpModel.Size = new System.Drawing.Size(120, 182);
            this.grpModel.TabIndex = 20;
            this.grpModel.TabStop = false;
            this.grpModel.Text = "Model";
            this.grpModel.Visible = false;
            // 
            // radGenModelANAN100D
            // 
            this.radGenModelANAN100D.AutoSize = true;
            this.radGenModelANAN100D.Image = null;
            this.radGenModelANAN100D.Location = new System.Drawing.Point(19, 117);
            this.radGenModelANAN100D.Name = "radGenModelANAN100D";
            this.radGenModelANAN100D.Size = new System.Drawing.Size(84, 17);
            this.radGenModelANAN100D.TabIndex = 11;
            this.radGenModelANAN100D.Text = "ANAN-100D";
            this.radGenModelANAN100D.UseVisualStyleBackColor = true;
            this.radGenModelANAN100D.CheckedChanged += new System.EventHandler(this.radGenModelANAN100D_CheckedChanged);
            // 
            // radGenModelANAN100
            // 
            this.radGenModelANAN100.AutoSize = true;
            this.radGenModelANAN100.Image = null;
            this.radGenModelANAN100.Location = new System.Drawing.Point(19, 93);
            this.radGenModelANAN100.Name = "radGenModelANAN100";
            this.radGenModelANAN100.Size = new System.Drawing.Size(76, 17);
            this.radGenModelANAN100.TabIndex = 10;
            this.radGenModelANAN100.Text = "ANAN-100";
            this.radGenModelANAN100.UseVisualStyleBackColor = true;
            this.radGenModelANAN100.CheckedChanged += new System.EventHandler(this.radGenModelANAN100_CheckedChanged);
            // 
            // radGenModelANAN10
            // 
            this.radGenModelANAN10.AutoSize = true;
            this.radGenModelANAN10.Image = null;
            this.radGenModelANAN10.Location = new System.Drawing.Point(19, 69);
            this.radGenModelANAN10.Name = "radGenModelANAN10";
            this.radGenModelANAN10.Size = new System.Drawing.Size(70, 17);
            this.radGenModelANAN10.TabIndex = 9;
            this.radGenModelANAN10.Text = "ANAN-10";
            this.radGenModelANAN10.UseVisualStyleBackColor = true;
            this.radGenModelANAN10.CheckedChanged += new System.EventHandler(this.radGenModelANAN10_CheckedChanged);
            // 
            // radGenModelHermes
            // 
            this.radGenModelHermes.AutoSize = true;
            this.radGenModelHermes.Image = null;
            this.radGenModelHermes.Location = new System.Drawing.Point(19, 45);
            this.radGenModelHermes.Name = "radGenModelHermes";
            this.radGenModelHermes.Size = new System.Drawing.Size(71, 17);
            this.radGenModelHermes.TabIndex = 8;
            this.radGenModelHermes.Text = "HERMES";
            this.radGenModelHermes.UseVisualStyleBackColor = true;
            this.radGenModelHermes.CheckedChanged += new System.EventHandler(this.radGenModelHermes_CheckedChanged);
            // 
            // radGenModelHPSDR
            // 
            this.radGenModelHPSDR.AutoSize = true;
            this.radGenModelHPSDR.Image = null;
            this.radGenModelHPSDR.Location = new System.Drawing.Point(19, 20);
            this.radGenModelHPSDR.Name = "radGenModelHPSDR";
            this.radGenModelHPSDR.Size = new System.Drawing.Size(63, 17);
            this.radGenModelHPSDR.TabIndex = 7;
            this.radGenModelHPSDR.Text = "HPSDR";
            this.radGenModelHPSDR.UseVisualStyleBackColor = true;
            this.radGenModelHPSDR.CheckedChanged += new System.EventHandler(this.radGenModelHPSDR_CheckedChanged);
            // 
            // radGenModelFLEX5000
            // 
            this.radGenModelFLEX5000.Checked = true;
            this.radGenModelFLEX5000.Image = null;
            this.radGenModelFLEX5000.Location = new System.Drawing.Point(18, 141);
            this.radGenModelFLEX5000.Name = "radGenModelFLEX5000";
            this.radGenModelFLEX5000.Size = new System.Drawing.Size(88, 24);
            this.radGenModelFLEX5000.TabIndex = 6;
            this.radGenModelFLEX5000.TabStop = true;
            this.radGenModelFLEX5000.Text = "FLEX-5000";
            this.radGenModelFLEX5000.Visible = false;
            this.radGenModelFLEX5000.CheckedChanged += new System.EventHandler(this.radGenModelFLEX5000_CheckedChanged);
            // 
            // radGenModelDemoNone
            // 
            this.radGenModelDemoNone.Image = null;
            this.radGenModelDemoNone.Location = new System.Drawing.Point(18, 141);
            this.radGenModelDemoNone.Name = "radGenModelDemoNone";
            this.radGenModelDemoNone.Size = new System.Drawing.Size(88, 24);
            this.radGenModelDemoNone.TabIndex = 5;
            this.radGenModelDemoNone.Text = "Demo/None";
            this.radGenModelDemoNone.Visible = false;
            this.radGenModelDemoNone.CheckedChanged += new System.EventHandler(this.radGenModelDemoNone_CheckedChanged);
            // 
            // radGenModelSoftRock40
            // 
            this.radGenModelSoftRock40.Image = null;
            this.radGenModelSoftRock40.Location = new System.Drawing.Point(18, 141);
            this.radGenModelSoftRock40.Name = "radGenModelSoftRock40";
            this.radGenModelSoftRock40.Size = new System.Drawing.Size(88, 24);
            this.radGenModelSoftRock40.TabIndex = 4;
            this.radGenModelSoftRock40.Text = "Soft Rock 40";
            this.radGenModelSoftRock40.Visible = false;
            this.radGenModelSoftRock40.CheckedChanged += new System.EventHandler(this.radGenModelSoftRock40_CheckedChanged);
            // 
            // radGenModelSDR1000
            // 
            this.radGenModelSDR1000.Image = null;
            this.radGenModelSDR1000.Location = new System.Drawing.Point(18, 141);
            this.radGenModelSDR1000.Name = "radGenModelSDR1000";
            this.radGenModelSDR1000.Size = new System.Drawing.Size(88, 24);
            this.radGenModelSDR1000.TabIndex = 3;
            this.radGenModelSDR1000.Text = "SDR-1000";
            this.radGenModelSDR1000.Visible = false;
            this.radGenModelSDR1000.CheckedChanged += new System.EventHandler(this.radGenModelSDR1000_CheckedChanged);
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
            this.groupBox2.Controls.Add(this.udPAGainByBand30);
            this.groupBox2.Controls.Add(this.lblPAGainByBand40);
            this.groupBox2.Controls.Add(this.udPAGainByBand40);
            this.groupBox2.Controls.Add(this.lblPAGainByBand60);
            this.groupBox2.Controls.Add(this.udPAGainByBand60);
            this.groupBox2.Controls.Add(this.lblPAGainByBand80);
            this.groupBox2.Controls.Add(this.udPAGainByBand80);
            this.groupBox2.Controls.Add(this.lblPAGainByBand160);
            this.groupBox2.Controls.Add(this.udPAGainByBand160);
            this.groupBox2.Controls.Add(this.lblPAGainByBand30);
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
            this.udPAGainByBand10.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPAGainByBand10.Location = new System.Drawing.Point(152, 112);
            this.udPAGainByBand10.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udPAGainByBand10.Minimum = new decimal(new int[] {
            390,
            0,
            0,
            65536});
            this.udPAGainByBand10.Name = "udPAGainByBand10";
            this.udPAGainByBand10.Size = new System.Drawing.Size(48, 20);
            this.udPAGainByBand10.TabIndex = 18;
            this.udPAGainByBand10.Value = new decimal(new int[] {
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
            this.udPAGainByBand12.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPAGainByBand12.Location = new System.Drawing.Point(152, 88);
            this.udPAGainByBand12.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udPAGainByBand12.Minimum = new decimal(new int[] {
            390,
            0,
            0,
            65536});
            this.udPAGainByBand12.Name = "udPAGainByBand12";
            this.udPAGainByBand12.Size = new System.Drawing.Size(48, 20);
            this.udPAGainByBand12.TabIndex = 16;
            this.udPAGainByBand12.Value = new decimal(new int[] {
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
            this.udPAGainByBand15.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPAGainByBand15.Location = new System.Drawing.Point(152, 64);
            this.udPAGainByBand15.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udPAGainByBand15.Minimum = new decimal(new int[] {
            390,
            0,
            0,
            65536});
            this.udPAGainByBand15.Name = "udPAGainByBand15";
            this.udPAGainByBand15.Size = new System.Drawing.Size(48, 20);
            this.udPAGainByBand15.TabIndex = 14;
            this.udPAGainByBand15.Value = new decimal(new int[] {
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
            this.udPAGainByBand17.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPAGainByBand17.Location = new System.Drawing.Point(152, 40);
            this.udPAGainByBand17.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udPAGainByBand17.Minimum = new decimal(new int[] {
            390,
            0,
            0,
            65536});
            this.udPAGainByBand17.Name = "udPAGainByBand17";
            this.udPAGainByBand17.Size = new System.Drawing.Size(48, 20);
            this.udPAGainByBand17.TabIndex = 12;
            this.udPAGainByBand17.Value = new decimal(new int[] {
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
            this.udPAGainByBand20.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPAGainByBand20.Location = new System.Drawing.Point(152, 16);
            this.udPAGainByBand20.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udPAGainByBand20.Minimum = new decimal(new int[] {
            390,
            0,
            0,
            65536});
            this.udPAGainByBand20.Name = "udPAGainByBand20";
            this.udPAGainByBand20.Size = new System.Drawing.Size(48, 20);
            this.udPAGainByBand20.TabIndex = 10;
            this.udPAGainByBand20.Value = new decimal(new int[] {
            483,
            0,
            0,
            65536});
            // 
            // udPAGainByBand30
            // 
            this.udPAGainByBand30.DecimalPlaces = 1;
            this.udPAGainByBand30.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPAGainByBand30.Location = new System.Drawing.Point(56, 112);
            this.udPAGainByBand30.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udPAGainByBand30.Minimum = new decimal(new int[] {
            390,
            0,
            0,
            65536});
            this.udPAGainByBand30.Name = "udPAGainByBand30";
            this.udPAGainByBand30.Size = new System.Drawing.Size(48, 20);
            this.udPAGainByBand30.TabIndex = 8;
            this.udPAGainByBand30.Value = new decimal(new int[] {
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
            this.udPAGainByBand40.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPAGainByBand40.Location = new System.Drawing.Point(56, 88);
            this.udPAGainByBand40.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udPAGainByBand40.Minimum = new decimal(new int[] {
            390,
            0,
            0,
            65536});
            this.udPAGainByBand40.Name = "udPAGainByBand40";
            this.udPAGainByBand40.Size = new System.Drawing.Size(48, 20);
            this.udPAGainByBand40.TabIndex = 6;
            this.udPAGainByBand40.Value = new decimal(new int[] {
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
            this.udPAGainByBand60.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPAGainByBand60.Location = new System.Drawing.Point(56, 64);
            this.udPAGainByBand60.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udPAGainByBand60.Minimum = new decimal(new int[] {
            390,
            0,
            0,
            65536});
            this.udPAGainByBand60.Name = "udPAGainByBand60";
            this.udPAGainByBand60.Size = new System.Drawing.Size(48, 20);
            this.udPAGainByBand60.TabIndex = 4;
            this.udPAGainByBand60.Value = new decimal(new int[] {
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
            this.udPAGainByBand80.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPAGainByBand80.Location = new System.Drawing.Point(56, 40);
            this.udPAGainByBand80.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udPAGainByBand80.Minimum = new decimal(new int[] {
            390,
            0,
            0,
            65536});
            this.udPAGainByBand80.Name = "udPAGainByBand80";
            this.udPAGainByBand80.Size = new System.Drawing.Size(48, 20);
            this.udPAGainByBand80.TabIndex = 2;
            this.udPAGainByBand80.Value = new decimal(new int[] {
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
            this.udPAGainByBand160.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.udPAGainByBand160.Location = new System.Drawing.Point(56, 16);
            this.udPAGainByBand160.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udPAGainByBand160.Minimum = new decimal(new int[] {
            390,
            0,
            0,
            65536});
            this.udPAGainByBand160.Name = "udPAGainByBand160";
            this.udPAGainByBand160.Size = new System.Drawing.Size(48, 20);
            this.udPAGainByBand160.TabIndex = 0;
            this.udPAGainByBand160.Value = new decimal(new int[] {
            490,
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
            "HPSDR",
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
            // lblMessage2
            // 
            this.lblMessage2.Image = null;
            this.lblMessage2.Location = new System.Drawing.Point(8, 192);
            this.lblMessage2.Name = "lblMessage2";
            this.lblMessage2.Size = new System.Drawing.Size(464, 48);
            this.lblMessage2.TabIndex = 7;
            this.lblMessage2.Text = "lblMessage2";
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
            // btnFinished
            // 
            this.btnFinished.Enabled = false;
            this.btnFinished.Image = null;
            this.btnFinished.Location = new System.Drawing.Point(400, 248);
            this.btnFinished.Name = "btnFinished";
            this.btnFinished.Size = new System.Drawing.Size(75, 23);
            this.btnFinished.TabIndex = 2;
            this.btnFinished.Text = "Finish";
            this.btnFinished.Click += new System.EventHandler(this.btnFinished_Click);
            // 
            // btnNext
            // 
            this.btnNext.Image = null;
            this.btnNext.Location = new System.Drawing.Point(312, 248);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Next";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Image = null;
            this.btnPrevious.Location = new System.Drawing.Point(224, 248);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnPrevious.TabIndex = 0;
            this.btnPrevious.Text = "Previous";
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // groupBoxHPSDR_Hardware
            // 
            this.groupBoxHPSDR_Hardware.Controls.Add(this.radOzy);
            this.groupBoxHPSDR_Hardware.Controls.Add(this.radMetis);
            this.groupBoxHPSDR_Hardware.Controls.Add(this.chkExcalibur);
            this.groupBoxHPSDR_Hardware.Controls.Add(this.chkPennyLane);
            this.groupBoxHPSDR_Hardware.Controls.Add(this.chkAlex);
            this.groupBoxHPSDR_Hardware.Controls.Add(this.chkPenny);
            this.groupBoxHPSDR_Hardware.Controls.Add(this.chkMercury);
            this.groupBoxHPSDR_Hardware.Location = new System.Drawing.Point(20, 32);
            this.groupBoxHPSDR_Hardware.Name = "groupBoxHPSDR_Hardware";
            this.groupBoxHPSDR_Hardware.Size = new System.Drawing.Size(183, 157);
            this.groupBoxHPSDR_Hardware.TabIndex = 21;
            this.groupBoxHPSDR_Hardware.TabStop = false;
            this.groupBoxHPSDR_Hardware.Text = "HPSDR Hardware";
            this.groupBoxHPSDR_Hardware.Visible = false;
            // 
            // radOzy
            // 
            this.radOzy.AutoSize = true;
            this.radOzy.Image = null;
            this.radOzy.Location = new System.Drawing.Point(114, 20);
            this.radOzy.Name = "radOzy";
            this.radOzy.Size = new System.Drawing.Size(43, 17);
            this.radOzy.TabIndex = 24;
            this.radOzy.Text = "Ozy";
            this.radOzy.UseVisualStyleBackColor = true;
            // 
            // radMetis
            // 
            this.radMetis.AutoSize = true;
            this.radMetis.Checked = true;
            this.radMetis.Image = null;
            this.radMetis.Location = new System.Drawing.Point(114, 46);
            this.radMetis.Name = "radMetis";
            this.radMetis.Size = new System.Drawing.Size(50, 17);
            this.radMetis.TabIndex = 23;
            this.radMetis.TabStop = true;
            this.radMetis.Text = "Metis";
            this.radMetis.UseVisualStyleBackColor = true;
            // 
            // chkExcalibur
            // 
            this.chkExcalibur.Image = null;
            this.chkExcalibur.Location = new System.Drawing.Point(16, 123);
            this.chkExcalibur.Name = "chkExcalibur";
            this.chkExcalibur.Size = new System.Drawing.Size(80, 24);
            this.chkExcalibur.TabIndex = 22;
            this.chkExcalibur.Text = "Excalibur";
            this.chkExcalibur.CheckedChanged += new System.EventHandler(this.chkExcalibur_CheckedChanged);
            // 
            // chkPennyLane
            // 
            this.chkPennyLane.Image = null;
            this.chkPennyLane.Location = new System.Drawing.Point(16, 70);
            this.chkPennyLane.Name = "chkPennyLane";
            this.chkPennyLane.Size = new System.Drawing.Size(80, 24);
            this.chkPennyLane.TabIndex = 3;
            this.chkPennyLane.Text = "PennyLane";
            this.chkPennyLane.CheckedChanged += new System.EventHandler(this.chkPennyLane_CheckedChanged);
            // 
            // chkAlex
            // 
            this.chkAlex.Image = null;
            this.chkAlex.Location = new System.Drawing.Point(16, 97);
            this.chkAlex.Name = "chkAlex";
            this.chkAlex.Size = new System.Drawing.Size(80, 24);
            this.chkAlex.TabIndex = 2;
            this.chkAlex.Text = "Alex";
            this.chkAlex.CheckedChanged += new System.EventHandler(this.chkAlex_CheckedChanged);
            // 
            // chkPenny
            // 
            this.chkPenny.Image = null;
            this.chkPenny.Location = new System.Drawing.Point(16, 43);
            this.chkPenny.Name = "chkPenny";
            this.chkPenny.Size = new System.Drawing.Size(80, 24);
            this.chkPenny.TabIndex = 1;
            this.chkPenny.Text = "Penelope";
            this.chkPenny.CheckedChanged += new System.EventHandler(this.chkPenny_CheckedChanged);
            // 
            // chkMercury
            // 
            this.chkMercury.Image = null;
            this.chkMercury.Location = new System.Drawing.Point(16, 16);
            this.chkMercury.Name = "chkMercury";
            this.chkMercury.Size = new System.Drawing.Size(80, 24);
            this.chkMercury.TabIndex = 0;
            this.chkMercury.Text = "Mercury";
            this.chkMercury.CheckedChanged += new System.EventHandler(this.chkMercury_CheckedChanged);
            //
            // comboBox10
            // 
            this.comboBox10.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox10.DropDownWidth = 184;
            this.comboBox10.Items.AddRange(new object[] {
            "Australia",
            "Europe",
            "Italy",
            "Japan",
            "Spain",
            "United Kingdom",
            "United States",
            "Norway",
            "Denmark",
            "Latvia",
            "Slovakia",
            "Bulgaria",
            "Greece",
            "Hungary",
            "Netherlands",
            "France",
            "Russia",
            "Extended"});
            this.comboBox10.Location = new System.Drawing.Point(76, 69);
            this.comboBox10.MaxDropDownItems = 10;
            this.comboBox10.Name = "comboBox10";
            this.comboBox10.Size = new System.Drawing.Size(184, 21);
            this.comboBox10.TabIndex = 22;
           // 
            // lblMessage1
            // 
            this.lblMessage1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage1.Image = null;
            this.lblMessage1.Location = new System.Drawing.Point(16, 8);
            this.lblMessage1.Name = "lblMessage1";
            this.lblMessage1.Size = new System.Drawing.Size(456, 136);
            this.lblMessage1.TabIndex = 4;
            this.lblMessage1.Text = "lblMessage1";
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
            // SetupWizard
            // 
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
            this.Controls.Add(this.btnFinished);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.groupBoxHPSDR_Hardware);
            this.Controls.Add(this.comboBox10);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblMessage1);
            this.Controls.Add(this.lblCombo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupWizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PowerSDR Setup Wizard - Welcome";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.SetupWizard_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.grpModel.ResumeLayout(false);
            this.grpModel.PerformLayout();
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
            this.groupBoxHPSDR_Hardware.ResumeLayout(false);
            this.groupBoxHPSDR_Hardware.PerformLayout();
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
                    comboBox10.Visible = false;
					groupBox2.Visible = false;
					lblCombo.Visible = false;
					grpModel.Visible = false;
                    lblMessage1.Text = "Welcome to the PowerSDR Setup Wizard.";
					lblMessage2.Text = "";
					pictureBox1.Image = null;
					pictureBox1.Visible = false;
					radYes.Visible = false;
					radNo.Visible = false;
                    groupBoxHPSDR_Hardware.Visible = false;
                    break;
				case Page.DATABASE:
					this.Text = "PowerSDR Setup Wizard - Database Import";
					btnFinished.Enabled = false;
					btnNext.Enabled = true;
					btnPrevious.Enabled = false;
					button1.Visible = false;
					comboBox1.Visible = false;
					comboBox2.Visible = false;
					comboBox3.Visible = false;
					groupBox2.Visible = false;
					lblCombo.Visible = false;
					grpModel.Visible = false;
					lblMessage1.Text = "Would you like to import a previous database? (Database files from " +
						"versions prior to 0.1.9 are incompatible).";
					lblMessage2.Text = "If you are upgrading from a previous version, you may use this " +
						"opportunity to import your old database settings (Setup, Memories, etc).";
					pictureBox1.Image = null;
					pictureBox1.Visible = false;
					radNo.Select();
					radYes.Visible = true;
					radNo.Visible = true;
                    groupBoxHPSDR_Hardware.Visible = false;
                    break;
				case Page.MODEL:
					this.Text = "PowerSDR Setup Wizard - Radio Model";
					btnFinished.Enabled = false;
					btnNext.Enabled = true;
					btnPrevious.Enabled = false;
					button1.Visible = false;
					comboBox1.Visible = false;
					comboBox2.Visible = false;
					comboBox3.Visible = false;
					groupBox2.Visible = false;
                    comboBox10.Visible = false;
					lblCombo.Visible = false;
					grpModel.Visible = true;
					lblMessage1.Text = "Please select the model of the radio you will be using.";
					lblMessage2.Text = " ";
					radGenModelFLEX5000_CheckedChanged(this, EventArgs.Empty);
					radGenModelSDR1000_CheckedChanged(this, EventArgs.Empty);
					radGenModelSoftRock40_CheckedChanged(this, EventArgs.Empty);
					radGenModelDemoNone_CheckedChanged(this, EventArgs.Empty);
					pictureBox1.Visible = true;
					radNo.Visible = false;
					radYes.Visible = false;
                    groupBoxHPSDR_Hardware.Visible = false;
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
					lblMessage2.Text = "This Down East Microwave 2M Transverter mounts on top of the " +
						"board stack and uses a 28MHz IF to get into the 144-146MHz range.  For more " +
						"information, see http://www.flex-radio.com.";
					pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.demi144-28frs.jpg"));
					pictureBox1.Visible = true;
					if(xvtr_present)
						radYes.Select();
					else
						radNo.Select();
					radYes.Visible = true;
					radNo.Visible = true;
                    groupBoxHPSDR_Hardware.Visible = false;
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
					lblMessage2.Text = "The Power Amplifier bumps the output power of the SDR-1000 from " +
						"1W up to 100W.  For more information, see http://www.flex-radio.com.";
					pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.sdr-pa100.jpg"));
					pictureBox1.Visible = true;
					if(pa_present)
						radYes.Select();
					else
						radNo.Select();
					radYes.Visible = true;
					radNo.Visible = true;
                    groupBoxHPSDR_Hardware.Visible = false;
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
					lblMessage2.Text = "The integrated ATU allows the user to tune coax antennas with an SWR of up to " +
						"10:1.  For more information, see http://www.flex-radio.com.";
					pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.sdr-atu.jpg"));
					pictureBox1.Visible = true;
					if(atu_present)
						radYes.Select();
					else
						radNo.Select();
					radYes.Visible = true;
					radNo.Visible = true;
                    groupBoxHPSDR_Hardware.Visible = false;
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
                    groupBoxHPSDR_Hardware.Visible = false;
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
					lblMessage2.Text = "The USB to Parallel adapter eliminates the need for a parallel port interface " +
						"on your computer.  Unlike other off-the-shelf adapters, the FlexRadio Systems adapter implements " +
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
                    groupBoxHPSDR_Hardware.Visible = false;
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
						"Tab of the Setup Form when finished with this wizard.";
					pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.soundcard.jpg"));
					pictureBox1.Visible = true;
					radYes.Visible = false;
					radNo.Visible = false;
                    groupBoxHPSDR_Hardware.Visible = false;
                    break;

                case Page.HPSDR_HARDWARE_SELECT:
                    this.Text = "PowerSDR Setup Wizard - HPSDR Hardwware Selection ";
                    btnFinished.Enabled = false;
                    btnNext.Enabled = true;
                    btnPrevious.Enabled = true;
                    button1.Visible = false;
                    comboBox1.Visible = false;
                    comboBox2.Visible = false;
                    comboBox3.Visible = false;
                    comboBox10.Visible = false;
                    groupBox2.Visible = false;
                    groupBoxHPSDR_Hardware.Visible = true;
                    grpModel.Visible = false;
                    lblCombo.Visible = false;
                    lblMessage1.Text = "Select the HPSDR hardware in your installation.";
                    lblMessage2.Visible = false;
                    pictureBox1.Image = null;
                    pictureBox1.Visible = false;
                    radYes.Visible = false;
                    radNo.Visible = false;
                    break;

                case Page.REGION:
                    this.Text = "PowerSDR Setup Wizard - HPSDR Region Selection ";
                    btnFinished.Enabled = false;
                    btnNext.Enabled = true;
                    btnPrevious.Enabled = true;
                    button1.Visible = false;
                    comboBox1.Visible = false;
                    comboBox2.Visible = false;
                    comboBox3.Visible = false;
                    comboBox10.Visible = true;
                    groupBox2.Visible = false;
                    groupBoxHPSDR_Hardware.Visible = false;
                    grpModel.Visible = false;
                    lblCombo.Visible = false;
                    lblMessage1.Text = "Select your Region.";
                    lblMessage2.Visible = false;
                    pictureBox1.Image = null;
                    pictureBox1.Visible = false;
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
					lblMessage1.Text = "Setup is now complete. ";
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
					CurPage = Page.MODEL;
					btnNext.Focus();
					break;
				case Page.DATABASE:
					CurPage = Page.MODEL;
					btnNext.Focus();
					break;
				case Page.MODEL:
					switch(model)
					{
						case Model.SDR1000:
							CurPage = Page.XVTR;
							btnNext.Focus();
							break;
						case Model.FLEX5000:
						case Model.FLEX3000:
							CurPage = Page.FINISHED;
							btnFinished.Focus();
							break;
                        case Model.HPSDR:
                        case Model.HERMES:
                        case Model.ANAN10:
                        case Model.ANAN100:
                        case Model.ANAN100D:
                            CurPage = Page.HPSDR_HARDWARE_SELECT;
                            btnNext.Focus();
                            break;
                        default:
							CurPage = Page.SOUND_CARD;
							btnNext.Focus();
							break;
					}					
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
                case Page.HPSDR_HARDWARE_SELECT:
                    CurPage = Page.FINISHED;
                  //  CurPage = Page.REGION;
                    btnFinished.Focus();
                    break;
                case Page.REGION:
                    region_index = comboBox10.SelectedIndex;
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
					CurPage = Page.WELCOME;
					btnPrevious.Focus();
					break;
                case Page.XVTR:
					xvtr_index = comboBox1.SelectedIndex;
					CurPage = Page.MODEL;
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
					CurPage = Page.ATU;
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
                case Page.HPSDR_HARDWARE_SELECT:
                    CurPage = Page.MODEL;
                    btnPrevious.Focus();
                    break;

                case Page.FINISHED:
	                //if (model == Model.HPSDR || model == Model.HERMES ||
                      //  model == Model.ANAN10 || model == Model.ANAN100 || model == Model.ANAN100D)
                        CurPage = Page.HPSDR_HARDWARE_SELECT;
                 
					btnPrevious.Focus();
					break;
			}
		}

		private void radYes_CheckedChanged(object sender, System.EventArgs e)
		{
			switch(current_page)
			{
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
			switch(model)
			{
				case Model.FLEX5000:
				case Model.FLEX3000:
                    break;
                case Model.HERMES:
                    console.SetupForm.PennyLanePresent = pennylane_present;
                    console.SetupForm.MercuryPresent = mercury_present;
                    console.SetupForm.AlexPresent = alex_present;
                    console.SetupForm.forceAudioSampleRate1("192000");
                    break;
                case Model.HPSDR:
                    console.SetupForm.PenelopePresent = penelope_present;
                    console.SetupForm.PennyLanePresent = pennylane_present;
                    console.SetupForm.MercuryPresent = mercury_present;
                    console.SetupForm.AlexPresent = alex_present;
                    console.SetupForm.ExcaliburPresent = excalibur_present;
                    console.SetupForm.radOzyUSB.Checked = radOzy.Checked;
                    console.SetupForm.radMetis.Checked = radMetis.Checked;
                    console.SetupForm.forceAudioSampleRate1("192000");
  					break;
                case Model.ANAN10:
                    console.SetupForm.PennyLanePresent = pennylane_present;
                    console.SetupForm.MercuryPresent = mercury_present;
                    console.SetupForm.AlexPresent = alex_present;
                    console.SetupForm.forceAudioSampleRate1("192000");
                    break;
                case Model.ANAN100:
                    console.SetupForm.PennyLanePresent = pennylane_present;
                    console.SetupForm.MercuryPresent = mercury_present;
                    console.SetupForm.AlexPresent = alex_present;
                    console.SetupForm.forceAudioSampleRate1("192000");
                    break;
                case Model.ANAN100D:
                    console.SetupForm.PennyLanePresent = pennylane_present;
                    console.SetupForm.MercuryPresent = mercury_present;
                    console.SetupForm.AlexPresent = alex_present;
                    console.SetupForm.forceAudioSampleRate1("192000");
                    break;
                default:
                  //  console.SetupForm.XVTRPresent = xvtr_present;
					//console.SetupForm.PAPresent = pa_present;
					//console.SetupForm.USBPresent = usb_present;
					//console.SetupForm.ATUPresent = atu_present;

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
                            case "HPSDR":
                                card = SoundCard.HPSDR;
                                break;
                                
						}
						console.CurrentSoundCard = card;
						
					}
				
					//if(xvtr_present)
					//	console.SetupForm.XVTRSelection = xvtr_index;					

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
					break;
			}
            console.SetupForm.CurrentModel = model;

          /*  if (region_index >= 0)
            {
                FRSRegion CurrentRegion = FRSRegion.US;
                if (comboBox10.Text == "")
                {
                    console.SetupForm.comboFRSRegion.Text = "United States";
                    console.CurrentRegion = CurrentRegion;
                    return;
                }
                switch (comboBox10.Text)
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
                        Display.Init();
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
                    case "Russia":
                        CurrentRegion = FRSRegion.Russia;
                        console.Extended = false;
                        break;
                    case "Extended":
                        console.Extended = true;
                        break;
                }
                console.SetupForm.comboFRSRegion.Text = comboBox10.Text;
                console.CurrentRegion = CurrentRegion;
            } */
            
            console.SetupForm.comboFRSRegion.Text = "United States";
            //console.CurrentRegion = FRSRegion.US;

			ArrayList a = new ArrayList();
			a.Add("SetupWizard/1");
			DB.SaveVars("State", ref a);

			console.SetupForm.SaveOptions();
			console.SaveState();

			done = true;
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
			/*if(DB.ImportDatabase(openFileDialog1.FileName))
				MessageBox.Show("Database Imported Successfully",
					"Database Imported",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information);

			console.SetupForm.GetTxProfiles();

			console.SetupForm.GetOptions();			// load all database values
			console.GetState();				
			if(console.EQForm != null) Common.RestoreForm(console.EQForm, "EQForm", false);
			if(console.XVTRForm != null) Common.RestoreForm(console.XVTRForm, "XVTR", false);
			if(console.ProdTestForm != null) Common.RestoreForm(console.ProdTestForm, "ProdTest", false);

			console.SetupForm.SaveOptions();		// save all database values
			console.SaveState();
			if(console.EQForm != null) Common.SaveForm(console.EQForm, "EQForm");
			if(console.XVTRForm != null) Common.SaveForm(console.XVTRForm, "XVTR");
			if(console.ProdTestForm != null) Common.SaveForm(console.ProdTestForm, "ProdTest");
			done = true;
			console.ResetMemForm();
			this.Close();*/
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			string path = console.AppDataPath;
			path = path.Substring(0, path.LastIndexOf("\\"));
			openFileDialog1.InitialDirectory = path;
			openFileDialog1.ShowDialog();
		}

		private void comboBox3_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(comboBox3.Text == "Unsupported Card" && comboBox3.Focused)
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

		private void radGenModelFLEX5000_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radGenModelFLEX5000.Checked)
			{
				if(radGenModelFLEX5000.Text == "FLEX-5000")
				{
					model = Model.FLEX5000;
					//if(grpModel.Visible)
                    pictureBox1.Image = null; //new Bitmap(GetResource("PowerSDR.images.flex-5000.jpg"));
				}
				else
				{
					model = Model.FLEX3000;
					//if(grpModel.Visible)
                    pictureBox1.Image = null;// new Bitmap(GetResource("PowerSDR.images.flex-3000.jpg"));
				}
			}
		}

		private void radGenModelSDR1000_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radGenModelSDR1000.Checked)
			{
				model = Model.SDR1000;
				//if(grpModel.Visible)
                pictureBox1.Image = null;// new Bitmap(GetResource("PowerSDR.images.sdr-1000.jpg"));
			}
		}

		private void radGenModelSoftRock40_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radGenModelSoftRock40.Checked)
			{
				model = Model.SOFTROCK40;
				//if(grpModel.Visible)
					pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.softrock40.jpg"));
			}
		}

		private void radGenModelDemoNone_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radGenModelDemoNone.Checked)
			{
				model = Model.DEMO;
				//if(grpModel.Visible)
					pictureBox1.Image = null;//new Bitmap(GetResource("PowerSDR.images.no_hardware.jpg"));
                    comboBox3.Text = "Unsupported Card";
			}
		}

        private void radGenModelHPSDR_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelHPSDR.Checked)
            {
                model = Model.HPSDR;
                //if (grpModel.Visible)
                pictureBox1.Image = null;
                pictureBox1.Visible = false;
                //pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.hpsdr.jpg"));
                chkMercury.Enabled = true;
                chkPennyLane.Enabled = true;
                chkPenny.Enabled = true;
                chkExcalibur.Enabled = true;
                chkAlex.Enabled = true;
                chkMercury.Checked = false;
                chkPennyLane.Checked = false;
                chkMercury.Checked = false;
                chkAlex.Enabled = true;
                chkExcalibur.Checked = false;
                radOzy.Enabled = true;
               // radMetis.Checked = false;
                radMetis.Enabled = true;
                radMetis.Enabled = true;
                radMetis.Text = "Metis";
            }
        }

        private void radGenModelHermes_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelHermes.Checked)
            {
                model = Model.HERMES;
                //if (grpModel.Visible)
                pictureBox1.Image = null;
                pictureBox1.Visible = false;
                //pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.hpsdr.jpg"));
                chkMercury.Checked = radGenModelHermes.Checked;
                chkPennyLane.Checked = radGenModelHermes.Checked;
                chkMercury.Enabled = false;
                chkPennyLane.Enabled = false;
                chkPenny.Enabled = false;
                chkPenny.Checked = false;
                chkExcalibur.Enabled = false;
                chkExcalibur.Checked = false;
                chkAlex.Checked = false;
                radMetis.Checked = true;
                radMetis.Enabled = false;
                radMetis.Enabled = false;
                radMetis.Text = "Hermes";
                radOzy.Enabled = false;
            }
        }

        private void radGenModelANAN10_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelANAN10.Checked)
            {
                model = Model.ANAN10;
                //if (grpModel.Visible)
                pictureBox1.Image = null;
                pictureBox1.Visible = false;
                //pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.hpsdr.jpg"));
                chkMercury.Checked = radGenModelANAN10.Checked;
                chkPennyLane.Checked = radGenModelANAN10.Checked;
                chkMercury.Enabled = false;
                chkPennyLane.Enabled = false;
                chkPenny.Enabled = false;
                chkPenny.Checked = false;
                chkExcalibur.Enabled = false;
                chkExcalibur.Checked = false;              
                chkAlex.Checked = true;
                chkAlex.Enabled = false;
                radMetis.Checked = true;
                radMetis.Enabled = false;
                radMetis.Text = "ANAN";
                radOzy.Enabled = false;
            }
        }

        private void radGenModelANAN100_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelANAN100.Checked)
            {
                model = Model.ANAN100;
                //if (grpModel.Visible)
                pictureBox1.Image = null;
                pictureBox1.Visible = false;
                //pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.hpsdr.jpg"));
                chkMercury.Checked = radGenModelANAN100.Checked;
                chkPennyLane.Checked = radGenModelANAN100.Checked;
                chkMercury.Enabled = false;
                chkPennyLane.Enabled = false;
                chkPenny.Enabled = false;
                chkPenny.Checked = false;
                chkExcalibur.Enabled = false;
                chkExcalibur.Checked = false;
                chkAlex.Checked = true;
                chkAlex.Enabled = false;
                radMetis.Checked = true;
                radMetis.Enabled = false;
                radMetis.Text = "ANAN";
                radOzy.Enabled = false;
            }
        }

        private void radGenModelANAN100D_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radGenModelANAN100D.Checked)
            {
                model = Model.ANAN100D;
                //if (grpModel.Visible)
                pictureBox1.Image = null;
                pictureBox1.Visible = false;
                //pictureBox1.Image = new Bitmap(GetResource("PowerSDR.images.hpsdr.jpg"));
                chkMercury.Checked = radGenModelANAN100D.Checked;
                chkPennyLane.Checked = radGenModelANAN100D.Checked;
                chkMercury.Enabled = false;
                chkPennyLane.Enabled = false;
                chkPenny.Enabled = false;
                chkPenny.Checked = false;
                chkExcalibur.Enabled = false;
                chkExcalibur.Checked = false;
                chkAlex.Checked = true;
                chkAlex.Enabled = false;
                radMetis.Checked = true;
                radMetis.Enabled = false;
                radMetis.Text = "ANAN";
                radOzy.Enabled = false;
            }
        }

        private void chkMercury_CheckedChanged(object sender, System.EventArgs e)
        {
            mercury_present = chkMercury.Checked;
        }

        private void chkPenny_CheckedChanged(object sender, System.EventArgs e)
        {
             penelope_present = chkPenny.Checked;
             if (chkPenny.Checked) chkPennyLane.Checked = false;
        }

        private void chkPennyLane_CheckedChanged(object sender, System.EventArgs e)
        {
            pennylane_present = chkPennyLane.Checked;
            if (chkPennyLane.Checked) chkPenny.Checked = false;
        }

        private void chkAlex_CheckedChanged(object sender, System.EventArgs e)
        {
            alex_present = chkAlex.Checked;
        }

        private void chkExcalibur_CheckedChanged(object sender, System.EventArgs e)
        {
            excalibur_present = chkExcalibur.Checked;
        }
        #endregion
    }
}
