//=================================================================
// wave_options.cs
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
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PowerSDR
{
	/// <summary>
	/// Summary description for WaveOptions.
	/// </summary>
	public class WaveOptions : System.Windows.Forms.Form
	{
		#region Variable Declaration

		private System.Windows.Forms.GroupBoxTS grpReceive;
		private System.Windows.Forms.RadioButtonTS radRXPreProcessed;
		private System.Windows.Forms.GroupBoxTS groupBox1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.RadioButtonTS radRXPostProcessed;
		private System.Windows.Forms.RadioButtonTS radTXPostProcessed;
		private System.Windows.Forms.RadioButtonTS radTXPreProcessed;
		private System.Windows.Forms.GroupBoxTS grpAudioSampleRate1;
		private System.Windows.Forms.ComboBoxTS comboSampleRate;
		private System.ComponentModel.IContainer components;

		#endregion

		#region Constructor and Destructor

		public WaveOptions()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			comboSampleRate.Text = Audio.SampleRate1.ToString();
			Common.RestoreForm(this, "WaveOptions", false);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WaveOptions));
			this.grpReceive = new System.Windows.Forms.GroupBoxTS();
			this.radRXPostProcessed = new System.Windows.Forms.RadioButtonTS();
			this.radRXPreProcessed = new System.Windows.Forms.RadioButtonTS();
			this.groupBox1 = new System.Windows.Forms.GroupBoxTS();
			this.radTXPostProcessed = new System.Windows.Forms.RadioButtonTS();
			this.radTXPreProcessed = new System.Windows.Forms.RadioButtonTS();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.comboSampleRate = new System.Windows.Forms.ComboBoxTS();
			this.grpAudioSampleRate1 = new System.Windows.Forms.GroupBoxTS();
			this.grpReceive.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.grpAudioSampleRate1.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpReceive
			// 
			this.grpReceive.Controls.Add(this.radRXPostProcessed);
			this.grpReceive.Controls.Add(this.radRXPreProcessed);
			this.grpReceive.Location = new System.Drawing.Point(8, 8);
			this.grpReceive.Name = "grpReceive";
			this.grpReceive.Size = new System.Drawing.Size(168, 80);
			this.grpReceive.TabIndex = 0;
			this.grpReceive.TabStop = false;
			this.grpReceive.Text = "Receive";
			// 
			// radRXPostProcessed
			// 
			this.radRXPostProcessed.Image = null;
			this.radRXPostProcessed.Location = new System.Drawing.Point(16, 48);
			this.radRXPostProcessed.Name = "radRXPostProcessed";
			this.radRXPostProcessed.Size = new System.Drawing.Size(144, 24);
			this.radRXPostProcessed.TabIndex = 1;
			this.radRXPostProcessed.Text = "Post-Processed Audio";
			this.toolTip1.SetToolTip(this.radRXPostProcessed, "The demodulated filtered audio you listen to.");
			this.radRXPostProcessed.CheckedChanged += new System.EventHandler(this.radRXPostProcessed_CheckedChanged);
			// 
			// radRXPreProcessed
			// 
			this.radRXPreProcessed.Checked = true;
			this.radRXPreProcessed.Image = null;
			this.radRXPreProcessed.Location = new System.Drawing.Point(16, 24);
			this.radRXPreProcessed.Name = "radRXPreProcessed";
			this.radRXPreProcessed.Size = new System.Drawing.Size(144, 24);
			this.radRXPreProcessed.TabIndex = 0;
			this.radRXPreProcessed.TabStop = true;
			this.radRXPreProcessed.Text = "Pre-Processed Audio";
			this.toolTip1.SetToolTip(this.radRXPreProcessed, "The raw audio coming out of the radio (11kHz IF)");
			this.radRXPreProcessed.CheckedChanged += new System.EventHandler(this.radRXPreProcessed_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radTXPostProcessed);
			this.groupBox1.Controls.Add(this.radTXPreProcessed);
			this.groupBox1.Location = new System.Drawing.Point(8, 96);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(168, 80);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Transmit";
			// 
			// radTXPostProcessed
			// 
			this.radTXPostProcessed.Checked = true;
			this.radTXPostProcessed.Image = null;
			this.radTXPostProcessed.Location = new System.Drawing.Point(16, 48);
			this.radTXPostProcessed.Name = "radTXPostProcessed";
			this.radTXPostProcessed.Size = new System.Drawing.Size(144, 24);
			this.radTXPostProcessed.TabIndex = 1;
			this.radTXPostProcessed.TabStop = true;
			this.radTXPostProcessed.Text = "Post-Processed Audio";
			this.toolTip1.SetToolTip(this.radTXPostProcessed, "Filtered, modulated, EQ\'d, and AGC\'d Signal");
			this.radTXPostProcessed.CheckedChanged += new System.EventHandler(this.radTXPostProcessed_CheckedChanged);
			// 
			// radTXPreProcessed
			// 
			this.radTXPreProcessed.Image = null;
			this.radTXPreProcessed.Location = new System.Drawing.Point(16, 24);
			this.radTXPreProcessed.Name = "radTXPreProcessed";
			this.radTXPreProcessed.Size = new System.Drawing.Size(144, 24);
			this.radTXPreProcessed.TabIndex = 0;
			this.radTXPreProcessed.Text = "Pre-Processed Audio";
			this.toolTip1.SetToolTip(this.radTXPreProcessed, "Raw Input (Microphone, Digital, etc)");
			this.radTXPreProcessed.CheckedChanged += new System.EventHandler(this.radTXPreProcessed_CheckedChanged);
			// 
			// comboSampleRate
			// 
			this.comboSampleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSampleRate.DropDownWidth = 64;
			this.comboSampleRate.Items.AddRange(new object[] {
																 "6000",
																 "12000",
																 "24000",
																 "48000",
																 "96000",
																 "192000"});
			this.comboSampleRate.Location = new System.Drawing.Point(16, 24);
			this.comboSampleRate.Name = "comboSampleRate";
			this.comboSampleRate.Size = new System.Drawing.Size(64, 21);
			this.comboSampleRate.TabIndex = 4;
			this.toolTip1.SetToolTip(this.comboSampleRate, "Sample Rate -- Range is dependent on selected sound card! ");
			// 
			// grpAudioSampleRate1
			// 
			this.grpAudioSampleRate1.Controls.Add(this.comboSampleRate);
			this.grpAudioSampleRate1.Location = new System.Drawing.Point(184, 8);
			this.grpAudioSampleRate1.Name = "grpAudioSampleRate1";
			this.grpAudioSampleRate1.Size = new System.Drawing.Size(96, 56);
			this.grpAudioSampleRate1.TabIndex = 36;
			this.grpAudioSampleRate1.TabStop = false;
			this.grpAudioSampleRate1.Text = "Sample Rate";
			// 
			// WaveOptions
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(336, 182);
			this.Controls.Add(this.grpAudioSampleRate1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.grpReceive);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "WaveOptions";
			this.Text = "Wave Record Options";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.WaveOptions_Closing);
			this.grpReceive.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.grpAudioSampleRate1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties

		public int SampleRate
		{
			get { return int.Parse(comboSampleRate.Text); }
		}

		#endregion

		#region Event Handler

		private void WaveOptions_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
			Common.SaveForm(this, "WaveOptions");
		}

		private void radRXPreProcessed_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radRXPreProcessed.Checked)
			{
				Audio.RecordRXPreProcessed = true;
			}
		}

		private void radRXPostProcessed_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radRXPostProcessed.Checked)
			{
				Audio.RecordRXPreProcessed = false;
			}
		}

		private void radTXPreProcessed_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radTXPreProcessed.Checked)
			{
				Audio.RecordTXPreProcessed = true;
			}
		}

		private void radTXPostProcessed_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radTXPostProcessed.Checked)
			{
				Audio.RecordTXPreProcessed = false;
			}
		}

		#endregion
	}
}
