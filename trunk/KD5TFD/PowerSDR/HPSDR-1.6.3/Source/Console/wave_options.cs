//=================================================================
// wave_options.cs
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
		private System.ComponentModel.IContainer components;

		#endregion

		#region Constructor and Destructor

		public WaveOptions()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			RestoreSettings();
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
			this.radRXPreProcessed = new System.Windows.Forms.RadioButtonTS();
			this.radRXPostProcessed = new System.Windows.Forms.RadioButtonTS();
			this.groupBox1 = new System.Windows.Forms.GroupBoxTS();
			this.radTXPostProcessed = new System.Windows.Forms.RadioButtonTS();
			this.radTXPreProcessed = new System.Windows.Forms.RadioButtonTS();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.grpReceive.SuspendLayout();
			this.groupBox1.SuspendLayout();
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
			// radRXPreProcessed
			// 
			this.radRXPreProcessed.Checked = true;
			this.radRXPreProcessed.Location = new System.Drawing.Point(16, 24);
			this.radRXPreProcessed.Name = "radRXPreProcessed";
			this.radRXPreProcessed.Size = new System.Drawing.Size(144, 24);
			this.radRXPreProcessed.TabIndex = 0;
			this.radRXPreProcessed.TabStop = true;
			this.radRXPreProcessed.Text = "Pre-Processed Audio";
			this.toolTip1.SetToolTip(this.radRXPreProcessed, "The raw audio coming out of the radio (11kHz IF)");
			this.radRXPreProcessed.CheckedChanged += new System.EventHandler(this.radRXPreProcessed_CheckedChanged);
			// 
			// radRXPostProcessed
			// 
			this.radRXPostProcessed.Location = new System.Drawing.Point(16, 48);
			this.radRXPostProcessed.Name = "radRXPostProcessed";
			this.radRXPostProcessed.Size = new System.Drawing.Size(144, 24);
			this.radRXPostProcessed.TabIndex = 1;
			this.radRXPostProcessed.Text = "Post-Processed Audio";
			this.toolTip1.SetToolTip(this.radRXPostProcessed, "The demodulated filtered audio you listen to.");
			this.radRXPostProcessed.CheckedChanged += new System.EventHandler(this.radRXPostProcessed_CheckedChanged);
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
			this.radTXPreProcessed.Location = new System.Drawing.Point(16, 24);
			this.radTXPreProcessed.Name = "radTXPreProcessed";
			this.radTXPreProcessed.Size = new System.Drawing.Size(144, 24);
			this.radTXPreProcessed.TabIndex = 0;
			this.radTXPreProcessed.Text = "Pre-Processed Audio";
			this.toolTip1.SetToolTip(this.radTXPreProcessed, "Raw Input (Microphone, Digital, etc)");
			this.radTXPreProcessed.CheckedChanged += new System.EventHandler(this.radTXPreProcessed_CheckedChanged);
			// 
			// WaveOptions
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(184, 182);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.grpReceive);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "WaveOptions";
			this.Text = "Wave Record Options";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.WaveOptions_Closing);
			this.grpReceive.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

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

		#endregion

		#region Event Handler

		private void WaveOptions_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
			SaveSettings();
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
