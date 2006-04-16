//=================================================================
// savemem.cs
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

using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PowerSDR
{
	public class SaveMem : System.Windows.Forms.Form
	{
		#region Variable Declaration

		Console console;

		private System.Windows.Forms.ComboBoxTS comboGroup;
		private System.Windows.Forms.LabelTS lblGroup;
		private System.Windows.Forms.LabelTS lblFrequency;
		private System.Windows.Forms.TextBoxTS txtFreq;
		private System.Windows.Forms.ComboBoxTS comboMode;
		private System.Windows.Forms.LabelTS lblMode;
		private System.Windows.Forms.LabelTS lblFilter;
		private System.Windows.Forms.ComboBoxTS comboFilter;
		private System.Windows.Forms.TextBoxTS txtCallsign;
		private System.Windows.Forms.LabelTS lblCallsign;
		private System.Windows.Forms.LabelTS lblComments;
		private System.Windows.Forms.TextBoxTS txtComments;
		private System.Windows.Forms.CheckBoxTS chkScan;
		private System.Windows.Forms.LabelTS lblSquelch;
		private System.Windows.Forms.LabelTS lblStepSize;
		private System.Windows.Forms.ComboBoxTS comboStepSize;
		private System.Windows.Forms.ButtonTS btnCancel;
		private System.Windows.Forms.ButtonTS btnOK;
		private System.Windows.Forms.LabelTS lblAGC;
		private System.Windows.Forms.ComboBoxTS comboAGC;
		private System.Windows.Forms.NumericUpDownTS udSquelch;
		
        private System.ComponentModel.Container components = null;

		#endregion

		#region Constructor and Destructor

		public SaveMem(Console c)
		{
			InitializeComponent();
			console = c;

			InitAGCModes();
			InitDSPModes();

			comboGroup.DataSource = DB.dsMemory.Tables["GroupList"];
			comboGroup.ValueMember = "GroupID";
			comboGroup.DisplayMember = "GroupName";
			comboMode.SelectedIndex = (int)DttSP.CurrentMode;
			if(DttSP.CurrentMode != DttSP.Mode.DRM &&
				DttSP.CurrentMode != DttSP.Mode.SPEC)
				comboFilter.SelectedIndex = (int)console.CurrentFilter;
			comboStepSize.SelectedIndex = console.StepSize;
			comboAGC.SelectedIndex = (int)console.CurrentAGCMode;
			udSquelch.Value = console.Squelch;

			txtFreq.Text = console.VFOAFreq.ToString("f6");
			chkScan.Checked = true;
			comboGroup.SelectedIndex = 0;

			this.ActiveControl = btnOK;		// OK has focus initially
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SaveMem));
			this.comboGroup = new System.Windows.Forms.ComboBoxTS();
			this.lblGroup = new System.Windows.Forms.LabelTS();
			this.lblFrequency = new System.Windows.Forms.LabelTS();
			this.txtFreq = new System.Windows.Forms.TextBoxTS();
			this.comboMode = new System.Windows.Forms.ComboBoxTS();
			this.lblMode = new System.Windows.Forms.LabelTS();
			this.lblFilter = new System.Windows.Forms.LabelTS();
			this.comboFilter = new System.Windows.Forms.ComboBoxTS();
			this.txtCallsign = new System.Windows.Forms.TextBoxTS();
			this.lblCallsign = new System.Windows.Forms.LabelTS();
			this.lblComments = new System.Windows.Forms.LabelTS();
			this.txtComments = new System.Windows.Forms.TextBoxTS();
			this.chkScan = new System.Windows.Forms.CheckBoxTS();
			this.lblSquelch = new System.Windows.Forms.LabelTS();
			this.lblStepSize = new System.Windows.Forms.LabelTS();
			this.comboStepSize = new System.Windows.Forms.ComboBoxTS();
			this.btnCancel = new System.Windows.Forms.ButtonTS();
			this.btnOK = new System.Windows.Forms.ButtonTS();
			this.lblAGC = new System.Windows.Forms.LabelTS();
			this.comboAGC = new System.Windows.Forms.ComboBoxTS();
			this.udSquelch = new System.Windows.Forms.NumericUpDownTS();
			((System.ComponentModel.ISupportInitialize)(this.udSquelch)).BeginInit();
			this.SuspendLayout();
			// 
			// comboGroup
			// 
			this.comboGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGroup.Location = new System.Drawing.Point(72, 16);
			this.comboGroup.Name = "comboGroup";
			this.comboGroup.Size = new System.Drawing.Size(112, 21);
			this.comboGroup.TabIndex = 0;
			// 
			// lblGroup
			// 
			this.lblGroup.Location = new System.Drawing.Point(16, 16);
			this.lblGroup.Name = "lblGroup";
			this.lblGroup.Size = new System.Drawing.Size(48, 23);
			this.lblGroup.TabIndex = 1;
			this.lblGroup.Text = "Group:";
			// 
			// lblFrequency
			// 
			this.lblFrequency.Location = new System.Drawing.Point(200, 16);
			this.lblFrequency.Name = "lblFrequency";
			this.lblFrequency.Size = new System.Drawing.Size(64, 23);
			this.lblFrequency.TabIndex = 2;
			this.lblFrequency.Text = "Frequency:";
			// 
			// txtFreq
			// 
			this.txtFreq.Location = new System.Drawing.Point(264, 16);
			this.txtFreq.Name = "txtFreq";
			this.txtFreq.Size = new System.Drawing.Size(88, 20);
			this.txtFreq.TabIndex = 3;
			this.txtFreq.Text = "";
			// 
			// comboMode
			// 
			this.comboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboMode.Location = new System.Drawing.Point(72, 48);
			this.comboMode.Name = "comboMode";
			this.comboMode.Size = new System.Drawing.Size(112, 21);
			this.comboMode.TabIndex = 4;
			this.comboMode.SelectedIndexChanged += new System.EventHandler(this.comboMode_SelectedIndexChanged);
			// 
			// lblMode
			// 
			this.lblMode.Location = new System.Drawing.Point(16, 48);
			this.lblMode.Name = "lblMode";
			this.lblMode.Size = new System.Drawing.Size(48, 23);
			this.lblMode.TabIndex = 5;
			this.lblMode.Text = "Mode:";
			// 
			// lblFilter
			// 
			this.lblFilter.Location = new System.Drawing.Point(16, 80);
			this.lblFilter.Name = "lblFilter";
			this.lblFilter.Size = new System.Drawing.Size(48, 23);
			this.lblFilter.TabIndex = 6;
			this.lblFilter.Text = "Filter:";
			// 
			// comboFilter
			// 
			this.comboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFilter.Items.AddRange(new object[] {
															 "6.0kHz",
															 "4.0kHz",
															 "2.6kHz",
															 "2.1kHz",
															 "1.0kHz",
															 "500Hz",
															 "250Hz",
															 "100Hz",
															 "50Hz",
															 "25Hz",
															 "Var1",
															 "Var2",
															 "None"});
			this.comboFilter.Location = new System.Drawing.Point(72, 80);
			this.comboFilter.Name = "comboFilter";
			this.comboFilter.Size = new System.Drawing.Size(112, 21);
			this.comboFilter.TabIndex = 7;
			// 
			// txtCallsign
			// 
			this.txtCallsign.Location = new System.Drawing.Point(264, 48);
			this.txtCallsign.Name = "txtCallsign";
			this.txtCallsign.Size = new System.Drawing.Size(88, 20);
			this.txtCallsign.TabIndex = 8;
			this.txtCallsign.Text = "";
			// 
			// lblCallsign
			// 
			this.lblCallsign.Location = new System.Drawing.Point(200, 48);
			this.lblCallsign.Name = "lblCallsign";
			this.lblCallsign.Size = new System.Drawing.Size(48, 24);
			this.lblCallsign.TabIndex = 9;
			this.lblCallsign.Text = "Callsign:";
			// 
			// lblComments
			// 
			this.lblComments.Location = new System.Drawing.Point(16, 144);
			this.lblComments.Name = "lblComments";
			this.lblComments.Size = new System.Drawing.Size(64, 23);
			this.lblComments.TabIndex = 10;
			this.lblComments.Text = "Comments:";
			// 
			// txtComments
			// 
			this.txtComments.Location = new System.Drawing.Point(72, 144);
			this.txtComments.Name = "txtComments";
			this.txtComments.Size = new System.Drawing.Size(280, 20);
			this.txtComments.TabIndex = 11;
			this.txtComments.Text = "";
			// 
			// chkScan
			// 
			this.chkScan.Location = new System.Drawing.Point(160, 176);
			this.chkScan.Name = "chkScan";
			this.chkScan.Size = new System.Drawing.Size(56, 24);
			this.chkScan.TabIndex = 12;
			this.chkScan.Text = "Scan";
			// 
			// lblSquelch
			// 
			this.lblSquelch.Location = new System.Drawing.Point(200, 80);
			this.lblSquelch.Name = "lblSquelch";
			this.lblSquelch.Size = new System.Drawing.Size(48, 23);
			this.lblSquelch.TabIndex = 13;
			this.lblSquelch.Text = "Squelch:";
			// 
			// lblStepSize
			// 
			this.lblStepSize.Location = new System.Drawing.Point(16, 112);
			this.lblStepSize.Name = "lblStepSize";
			this.lblStepSize.Size = new System.Drawing.Size(56, 23);
			this.lblStepSize.TabIndex = 15;
			this.lblStepSize.Text = "Step Size:";
			// 
			// comboStepSize
			// 
			this.comboStepSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStepSize.Items.AddRange(new object[] {
															   "1Hz",
															   "10Hz",
															   "100Hz",
															   "1kHz",
															   "10kHz",
															   "100kHz",
															   "1MHz",
															   "10MHz"});
			this.comboStepSize.Location = new System.Drawing.Point(72, 112);
			this.comboStepSize.Name = "comboStepSize";
			this.comboStepSize.Size = new System.Drawing.Size(112, 21);
			this.comboStepSize.TabIndex = 16;
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(216, 208);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 17;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(72, 208);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 18;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lblAGC
			// 
			this.lblAGC.Location = new System.Drawing.Point(200, 112);
			this.lblAGC.Name = "lblAGC";
			this.lblAGC.Size = new System.Drawing.Size(48, 23);
			this.lblAGC.TabIndex = 19;
			this.lblAGC.Text = "AGC:";
			// 
			// comboAGC
			// 
			this.comboAGC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAGC.Location = new System.Drawing.Point(264, 112);
			this.comboAGC.Name = "comboAGC";
			this.comboAGC.Size = new System.Drawing.Size(88, 21);
			this.comboAGC.TabIndex = 20;
			// 
			// udSquelch
			// 
			this.udSquelch.Location = new System.Drawing.Point(264, 80);
			this.udSquelch.Maximum = new System.Decimal(new int[] {
																	  160,
																	  0,
																	  0,
																	  0});
			this.udSquelch.Name = "udSquelch";
			this.udSquelch.Size = new System.Drawing.Size(48, 20);
			this.udSquelch.TabIndex = 21;
			// 
			// SaveMem
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(368, 244);
			this.Controls.Add(this.udSquelch);
			this.Controls.Add(this.comboAGC);
			this.Controls.Add(this.lblAGC);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.comboStepSize);
			this.Controls.Add(this.lblStepSize);
			this.Controls.Add(this.txtComments);
			this.Controls.Add(this.txtCallsign);
			this.Controls.Add(this.txtFreq);
			this.Controls.Add(this.lblSquelch);
			this.Controls.Add(this.chkScan);
			this.Controls.Add(this.lblComments);
			this.Controls.Add(this.lblCallsign);
			this.Controls.Add(this.comboFilter);
			this.Controls.Add(this.lblFilter);
			this.Controls.Add(this.lblMode);
			this.Controls.Add(this.comboMode);
			this.Controls.Add(this.lblFrequency);
			this.Controls.Add(this.lblGroup);
			this.Controls.Add(this.comboGroup);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "SaveMem";
			this.Text = "PowerSDR Save Memory Channel";
			((System.ComponentModel.ISupportInitialize)(this.udSquelch)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Misc Routines

		private void InitAGCModes()
		{
			for(AGCMode agc=AGCMode.FIRST+1; agc<AGCMode.LAST; agc++)
			{
				string s = agc.ToString().ToLower();
				s = s.Substring(0, 1).ToUpper() + s.Substring(1, s.Length-1);
				comboAGC.Items.Add(s);
			}
		}

		private void InitDSPModes()
		{
			for(DSPMode m=DSPMode.FIRST+1; m<DSPMode.LAST; m++)
				comboMode.Items.Add(m.ToString());
		}

		#endregion

		#region Event Handlers

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			DataRow dr = DB.dsMemory.Tables["Memory"].NewRow();

			dr["GroupID"] = comboGroup.SelectedIndex;

			if(txtFreq.Text != "")
				dr["Frequency"] = txtFreq.Text;

			dr["ModeID"] = comboMode.SelectedIndex;

			dr["FilterID"] = comboFilter.SelectedIndex;

			if(txtCallsign.Text != "")
				dr["Callsign"] = txtCallsign.Text;

			if(txtComments.Text != "")
				dr["Comments"] = txtComments.Text;

			dr["Scan"] = (int)chkScan.CheckState;

			dr["Squelch"] = (int)udSquelch.Value;

			dr["StepSizeID"] = comboStepSize.SelectedIndex;

			dr["AGCID"] = comboAGC.SelectedIndex;

			DB.dsMemory.Tables["Memory"].Rows.Add(dr);
			if(DB.dsMemory.Tables["Memory"].Rows.Count == 1)
				console.MemForm.SetIndex();
			this.Close();
		}

		private void comboMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(comboMode.SelectedIndex == (int)DSPMode.DRM ||
				comboMode.SelectedIndex == (int)DSPMode.SPEC)
			{
				comboFilter.SelectedIndex = (int)Filter.NONE;
				comboFilter.Enabled = false;
			}
			else comboFilter.Enabled = true;
		}

		#endregion
	}
}
