//=================================================================
// eqform.cs
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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PowerSDR
{
	/// <summary>
	/// Summary description for EQForm.
	/// </summary>
	public class EQForm : System.Windows.Forms.Form
	{
		#region Variable Declaration

		private System.Windows.Forms.GroupBoxTS grpRXEQ;
		private System.Windows.Forms.GroupBoxTS grpTXEQ;
		private System.Windows.Forms.TrackBarTS tbRXEQ1;
		private System.Windows.Forms.TrackBarTS tbRXEQ2;
		private System.Windows.Forms.TrackBarTS tbRXEQ3;
		private System.Windows.Forms.TrackBarTS tbTXEQ3;
		private System.Windows.Forms.TrackBarTS tbTXEQ1;
		private System.Windows.Forms.TrackBarTS tbTXEQ2;
		private System.Windows.Forms.LabelTS lblRXEQ0dB;
		private System.Windows.Forms.LabelTS lblTXEQ0dB;
		private System.Windows.Forms.LabelTS lblRXEQ1;
		private System.Windows.Forms.LabelTS lblRXEQ2;
		private System.Windows.Forms.LabelTS lblRXEQ3;
		private System.Windows.Forms.LabelTS lblTXEQ3;
		private System.Windows.Forms.LabelTS lblTXEQ2;
		private System.Windows.Forms.LabelTS lblTXEQ1;
		private System.Windows.Forms.LabelTS lblRXEQPreamp;
		private System.Windows.Forms.LabelTS lblTXEQPreamp;
		private System.Windows.Forms.CheckBoxTS chkTXEQEnabled;
		private System.Windows.Forms.TrackBarTS tbRXEQPreamp;
		private System.Windows.Forms.TrackBarTS tbTXEQPreamp;
		private System.Windows.Forms.CheckBoxTS chkRXEQEnabled;
		private System.Windows.Forms.PictureBox picRXEQ;
		private System.Windows.Forms.PictureBox picTXEQ;
		private System.Windows.Forms.Button btnTXEQReset;
		private System.Windows.Forms.Button btnRXEQReset;
		private System.Windows.Forms.LabelTS lblRXEQ15db;
		private System.Windows.Forms.LabelTS lblTXEQ15db;
		private System.Windows.Forms.LabelTS lblRXEQminus12db;
		private System.Windows.Forms.LabelTS lblTXEQminus12db;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBoxTS chkTXEQ160Notch;
		private System.ComponentModel.IContainer components;
		
		#endregion

		#region Constructor and Destructor

		public EQForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			RestoreSettings();

			tbRXEQ_Scroll(this, EventArgs.Empty);
			tbTXEQ_Scroll(this, EventArgs.Empty);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(EQForm));
			this.grpRXEQ = new System.Windows.Forms.GroupBoxTS();
			this.picRXEQ = new System.Windows.Forms.PictureBox();
			this.btnRXEQReset = new System.Windows.Forms.Button();
			this.chkRXEQEnabled = new System.Windows.Forms.CheckBoxTS();
			this.tbRXEQ1 = new System.Windows.Forms.TrackBarTS();
			this.tbRXEQ2 = new System.Windows.Forms.TrackBarTS();
			this.tbRXEQ3 = new System.Windows.Forms.TrackBarTS();
			this.lblRXEQ1 = new System.Windows.Forms.LabelTS();
			this.lblRXEQ2 = new System.Windows.Forms.LabelTS();
			this.lblRXEQ3 = new System.Windows.Forms.LabelTS();
			this.lblRXEQPreamp = new System.Windows.Forms.LabelTS();
			this.tbRXEQPreamp = new System.Windows.Forms.TrackBarTS();
			this.lblRXEQ15db = new System.Windows.Forms.LabelTS();
			this.lblRXEQ0dB = new System.Windows.Forms.LabelTS();
			this.lblRXEQminus12db = new System.Windows.Forms.LabelTS();
			this.grpTXEQ = new System.Windows.Forms.GroupBoxTS();
			this.chkTXEQ160Notch = new System.Windows.Forms.CheckBoxTS();
			this.picTXEQ = new System.Windows.Forms.PictureBox();
			this.btnTXEQReset = new System.Windows.Forms.Button();
			this.chkTXEQEnabled = new System.Windows.Forms.CheckBoxTS();
			this.tbTXEQ1 = new System.Windows.Forms.TrackBarTS();
			this.tbTXEQ2 = new System.Windows.Forms.TrackBarTS();
			this.tbTXEQ3 = new System.Windows.Forms.TrackBarTS();
			this.lblTXEQ1 = new System.Windows.Forms.LabelTS();
			this.lblTXEQ2 = new System.Windows.Forms.LabelTS();
			this.lblTXEQ3 = new System.Windows.Forms.LabelTS();
			this.lblTXEQPreamp = new System.Windows.Forms.LabelTS();
			this.tbTXEQPreamp = new System.Windows.Forms.TrackBarTS();
			this.lblTXEQ15db = new System.Windows.Forms.LabelTS();
			this.lblTXEQ0dB = new System.Windows.Forms.LabelTS();
			this.lblTXEQminus12db = new System.Windows.Forms.LabelTS();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.grpRXEQ.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQPreamp)).BeginInit();
			this.grpTXEQ.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQPreamp)).BeginInit();
			this.SuspendLayout();
			// 
			// grpRXEQ
			// 
			this.grpRXEQ.Controls.Add(this.picRXEQ);
			this.grpRXEQ.Controls.Add(this.btnRXEQReset);
			this.grpRXEQ.Controls.Add(this.chkRXEQEnabled);
			this.grpRXEQ.Controls.Add(this.tbRXEQ1);
			this.grpRXEQ.Controls.Add(this.tbRXEQ2);
			this.grpRXEQ.Controls.Add(this.tbRXEQ3);
			this.grpRXEQ.Controls.Add(this.lblRXEQ1);
			this.grpRXEQ.Controls.Add(this.lblRXEQ2);
			this.grpRXEQ.Controls.Add(this.lblRXEQ3);
			this.grpRXEQ.Controls.Add(this.lblRXEQPreamp);
			this.grpRXEQ.Controls.Add(this.tbRXEQPreamp);
			this.grpRXEQ.Controls.Add(this.lblRXEQ15db);
			this.grpRXEQ.Controls.Add(this.lblRXEQ0dB);
			this.grpRXEQ.Controls.Add(this.lblRXEQminus12db);
			this.grpRXEQ.Location = new System.Drawing.Point(8, 8);
			this.grpRXEQ.Name = "grpRXEQ";
			this.grpRXEQ.Size = new System.Drawing.Size(280, 224);
			this.grpRXEQ.TabIndex = 1;
			this.grpRXEQ.TabStop = false;
			this.grpRXEQ.Text = "Receive Equalizer";
			// 
			// picRXEQ
			// 
			this.picRXEQ.BackColor = System.Drawing.Color.Black;
			this.picRXEQ.Location = new System.Drawing.Point(88, 16);
			this.picRXEQ.Name = "picRXEQ";
			this.picRXEQ.Size = new System.Drawing.Size(112, 24);
			this.picRXEQ.TabIndex = 111;
			this.picRXEQ.TabStop = false;
			this.picRXEQ.Paint += new System.Windows.Forms.PaintEventHandler(this.picRXEQ_Paint);
			// 
			// btnRXEQReset
			// 
			this.btnRXEQReset.Location = new System.Drawing.Point(208, 16);
			this.btnRXEQReset.Name = "btnRXEQReset";
			this.btnRXEQReset.Size = new System.Drawing.Size(56, 20);
			this.btnRXEQReset.TabIndex = 110;
			this.btnRXEQReset.Text = "Reset";
			this.btnRXEQReset.Click += new System.EventHandler(this.btnRXEQReset_Click);
			// 
			// chkRXEQEnabled
			// 
			this.chkRXEQEnabled.Image = null;
			this.chkRXEQEnabled.Location = new System.Drawing.Point(16, 24);
			this.chkRXEQEnabled.Name = "chkRXEQEnabled";
			this.chkRXEQEnabled.Size = new System.Drawing.Size(72, 16);
			this.chkRXEQEnabled.TabIndex = 109;
			this.chkRXEQEnabled.Text = "Enabled";
			this.chkRXEQEnabled.CheckedChanged += new System.EventHandler(this.chkRXEQEnabled_CheckedChanged);
			// 
			// tbRXEQ1
			// 
			this.tbRXEQ1.AutoSize = false;
			this.tbRXEQ1.LargeChange = 3;
			this.tbRXEQ1.Location = new System.Drawing.Point(88, 64);
			this.tbRXEQ1.Maximum = 15;
			this.tbRXEQ1.Minimum = -12;
			this.tbRXEQ1.Name = "tbRXEQ1";
			this.tbRXEQ1.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbRXEQ1.Size = new System.Drawing.Size(32, 128);
			this.tbRXEQ1.TabIndex = 4;
			this.tbRXEQ1.TickFrequency = 3;
			this.tbRXEQ1.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
			// 
			// tbRXEQ2
			// 
			this.tbRXEQ2.AutoSize = false;
			this.tbRXEQ2.LargeChange = 3;
			this.tbRXEQ2.Location = new System.Drawing.Point(128, 64);
			this.tbRXEQ2.Maximum = 15;
			this.tbRXEQ2.Minimum = -12;
			this.tbRXEQ2.Name = "tbRXEQ2";
			this.tbRXEQ2.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbRXEQ2.Size = new System.Drawing.Size(32, 128);
			this.tbRXEQ2.TabIndex = 5;
			this.tbRXEQ2.TickFrequency = 3;
			this.tbRXEQ2.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
			// 
			// tbRXEQ3
			// 
			this.tbRXEQ3.AutoSize = false;
			this.tbRXEQ3.LargeChange = 3;
			this.tbRXEQ3.Location = new System.Drawing.Point(168, 64);
			this.tbRXEQ3.Maximum = 15;
			this.tbRXEQ3.Minimum = -12;
			this.tbRXEQ3.Name = "tbRXEQ3";
			this.tbRXEQ3.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbRXEQ3.Size = new System.Drawing.Size(32, 128);
			this.tbRXEQ3.TabIndex = 6;
			this.tbRXEQ3.TickFrequency = 3;
			this.tbRXEQ3.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
			// 
			// lblRXEQ1
			// 
			this.lblRXEQ1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblRXEQ1.Image = null;
			this.lblRXEQ1.Location = new System.Drawing.Point(88, 48);
			this.lblRXEQ1.Name = "lblRXEQ1";
			this.lblRXEQ1.Size = new System.Drawing.Size(32, 16);
			this.lblRXEQ1.TabIndex = 43;
			this.lblRXEQ1.Text = "Low";
			this.toolTip1.SetToolTip(this.lblRXEQ1, "0-400Hz");
			// 
			// lblRXEQ2
			// 
			this.lblRXEQ2.Image = null;
			this.lblRXEQ2.Location = new System.Drawing.Point(128, 48);
			this.lblRXEQ2.Name = "lblRXEQ2";
			this.lblRXEQ2.Size = new System.Drawing.Size(32, 16);
			this.lblRXEQ2.TabIndex = 44;
			this.lblRXEQ2.Text = "Mid";
			this.toolTip1.SetToolTip(this.lblRXEQ2, "400-1500Hz");
			// 
			// lblRXEQ3
			// 
			this.lblRXEQ3.Image = null;
			this.lblRXEQ3.Location = new System.Drawing.Point(168, 48);
			this.lblRXEQ3.Name = "lblRXEQ3";
			this.lblRXEQ3.Size = new System.Drawing.Size(32, 16);
			this.lblRXEQ3.TabIndex = 45;
			this.lblRXEQ3.Text = "High";
			this.toolTip1.SetToolTip(this.lblRXEQ3, "1500-6000Hz");
			// 
			// lblRXEQPreamp
			// 
			this.lblRXEQPreamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblRXEQPreamp.Image = null;
			this.lblRXEQPreamp.Location = new System.Drawing.Point(8, 48);
			this.lblRXEQPreamp.Name = "lblRXEQPreamp";
			this.lblRXEQPreamp.Size = new System.Drawing.Size(48, 16);
			this.lblRXEQPreamp.TabIndex = 74;
			this.lblRXEQPreamp.Text = "Preamp";
			this.lblRXEQPreamp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tbRXEQPreamp
			// 
			this.tbRXEQPreamp.AutoSize = false;
			this.tbRXEQPreamp.LargeChange = 3;
			this.tbRXEQPreamp.Location = new System.Drawing.Point(16, 64);
			this.tbRXEQPreamp.Maximum = 15;
			this.tbRXEQPreamp.Minimum = -12;
			this.tbRXEQPreamp.Name = "tbRXEQPreamp";
			this.tbRXEQPreamp.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbRXEQPreamp.Size = new System.Drawing.Size(32, 128);
			this.tbRXEQPreamp.TabIndex = 35;
			this.tbRXEQPreamp.TickFrequency = 3;
			this.tbRXEQPreamp.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
			// 
			// lblRXEQ15db
			// 
			this.lblRXEQ15db.Image = null;
			this.lblRXEQ15db.Location = new System.Drawing.Point(204, 70);
			this.lblRXEQ15db.Name = "lblRXEQ15db";
			this.lblRXEQ15db.Size = new System.Drawing.Size(32, 16);
			this.lblRXEQ15db.TabIndex = 40;
			this.lblRXEQ15db.Text = "15dB";
			this.lblRXEQ15db.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblRXEQ0dB
			// 
			this.lblRXEQ0dB.Image = null;
			this.lblRXEQ0dB.Location = new System.Drawing.Point(204, 126);
			this.lblRXEQ0dB.Name = "lblRXEQ0dB";
			this.lblRXEQ0dB.Size = new System.Drawing.Size(32, 16);
			this.lblRXEQ0dB.TabIndex = 41;
			this.lblRXEQ0dB.Text = "  0dB";
			this.lblRXEQ0dB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblRXEQminus12db
			// 
			this.lblRXEQminus12db.Image = null;
			this.lblRXEQminus12db.Location = new System.Drawing.Point(200, 170);
			this.lblRXEQminus12db.Name = "lblRXEQminus12db";
			this.lblRXEQminus12db.Size = new System.Drawing.Size(34, 16);
			this.lblRXEQminus12db.TabIndex = 42;
			this.lblRXEQminus12db.Text = "-12dB";
			this.lblRXEQminus12db.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// grpTXEQ
			// 
			this.grpTXEQ.Controls.Add(this.chkTXEQ160Notch);
			this.grpTXEQ.Controls.Add(this.picTXEQ);
			this.grpTXEQ.Controls.Add(this.btnTXEQReset);
			this.grpTXEQ.Controls.Add(this.chkTXEQEnabled);
			this.grpTXEQ.Controls.Add(this.tbTXEQ1);
			this.grpTXEQ.Controls.Add(this.tbTXEQ2);
			this.grpTXEQ.Controls.Add(this.tbTXEQ3);
			this.grpTXEQ.Controls.Add(this.lblTXEQ1);
			this.grpTXEQ.Controls.Add(this.lblTXEQ2);
			this.grpTXEQ.Controls.Add(this.lblTXEQ3);
			this.grpTXEQ.Controls.Add(this.lblTXEQPreamp);
			this.grpTXEQ.Controls.Add(this.tbTXEQPreamp);
			this.grpTXEQ.Controls.Add(this.lblTXEQ15db);
			this.grpTXEQ.Controls.Add(this.lblTXEQ0dB);
			this.grpTXEQ.Controls.Add(this.lblTXEQminus12db);
			this.grpTXEQ.Location = new System.Drawing.Point(296, 8);
			this.grpTXEQ.Name = "grpTXEQ";
			this.grpTXEQ.Size = new System.Drawing.Size(280, 224);
			this.grpTXEQ.TabIndex = 1;
			this.grpTXEQ.TabStop = false;
			this.grpTXEQ.Text = "Transmit Equalizer";
			// 
			// chkTXEQ160Notch
			// 
			this.chkTXEQ160Notch.Image = null;
			this.chkTXEQ160Notch.Location = new System.Drawing.Point(16, 192);
			this.chkTXEQ160Notch.Name = "chkTXEQ160Notch";
			this.chkTXEQ160Notch.Size = new System.Drawing.Size(96, 16);
			this.chkTXEQ160Notch.TabIndex = 113;
			this.chkTXEQ160Notch.Text = "160Hz Notch";
			this.chkTXEQ160Notch.CheckedChanged += new System.EventHandler(this.chkTXEQ160Notch_CheckedChanged);
			// 
			// picTXEQ
			// 
			this.picTXEQ.BackColor = System.Drawing.Color.Black;
			this.picTXEQ.Location = new System.Drawing.Point(88, 16);
			this.picTXEQ.Name = "picTXEQ";
			this.picTXEQ.Size = new System.Drawing.Size(112, 24);
			this.picTXEQ.TabIndex = 112;
			this.picTXEQ.TabStop = false;
			this.picTXEQ.Paint += new System.Windows.Forms.PaintEventHandler(this.picTXEQ_Paint);
			// 
			// btnTXEQReset
			// 
			this.btnTXEQReset.Location = new System.Drawing.Point(208, 16);
			this.btnTXEQReset.Name = "btnTXEQReset";
			this.btnTXEQReset.Size = new System.Drawing.Size(56, 20);
			this.btnTXEQReset.TabIndex = 107;
			this.btnTXEQReset.Text = "Reset";
			this.btnTXEQReset.Click += new System.EventHandler(this.btnTXEQReset_Click);
			// 
			// chkTXEQEnabled
			// 
			this.chkTXEQEnabled.Image = null;
			this.chkTXEQEnabled.Location = new System.Drawing.Point(16, 24);
			this.chkTXEQEnabled.Name = "chkTXEQEnabled";
			this.chkTXEQEnabled.Size = new System.Drawing.Size(72, 16);
			this.chkTXEQEnabled.TabIndex = 106;
			this.chkTXEQEnabled.Text = "Enabled";
			this.chkTXEQEnabled.CheckedChanged += new System.EventHandler(this.chkTXEQEnabled_CheckedChanged);
			// 
			// tbTXEQ1
			// 
			this.tbTXEQ1.AutoSize = false;
			this.tbTXEQ1.LargeChange = 3;
			this.tbTXEQ1.Location = new System.Drawing.Point(88, 64);
			this.tbTXEQ1.Maximum = 15;
			this.tbTXEQ1.Minimum = -12;
			this.tbTXEQ1.Name = "tbTXEQ1";
			this.tbTXEQ1.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbTXEQ1.Size = new System.Drawing.Size(32, 128);
			this.tbTXEQ1.TabIndex = 4;
			this.tbTXEQ1.TickFrequency = 3;
			this.tbTXEQ1.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
			// 
			// tbTXEQ2
			// 
			this.tbTXEQ2.AutoSize = false;
			this.tbTXEQ2.LargeChange = 3;
			this.tbTXEQ2.Location = new System.Drawing.Point(128, 64);
			this.tbTXEQ2.Maximum = 15;
			this.tbTXEQ2.Minimum = -12;
			this.tbTXEQ2.Name = "tbTXEQ2";
			this.tbTXEQ2.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbTXEQ2.Size = new System.Drawing.Size(32, 128);
			this.tbTXEQ2.TabIndex = 5;
			this.tbTXEQ2.TickFrequency = 3;
			this.tbTXEQ2.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
			// 
			// tbTXEQ3
			// 
			this.tbTXEQ3.AutoSize = false;
			this.tbTXEQ3.LargeChange = 3;
			this.tbTXEQ3.Location = new System.Drawing.Point(168, 64);
			this.tbTXEQ3.Maximum = 15;
			this.tbTXEQ3.Minimum = -12;
			this.tbTXEQ3.Name = "tbTXEQ3";
			this.tbTXEQ3.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbTXEQ3.Size = new System.Drawing.Size(32, 128);
			this.tbTXEQ3.TabIndex = 6;
			this.tbTXEQ3.TickFrequency = 3;
			this.tbTXEQ3.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
			// 
			// lblTXEQ1
			// 
			this.lblTXEQ1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblTXEQ1.Image = null;
			this.lblTXEQ1.Location = new System.Drawing.Point(88, 48);
			this.lblTXEQ1.Name = "lblTXEQ1";
			this.lblTXEQ1.Size = new System.Drawing.Size(32, 16);
			this.lblTXEQ1.TabIndex = 74;
			this.lblTXEQ1.Text = "Low";
			this.toolTip1.SetToolTip(this.lblTXEQ1, "0-400Hz");
			// 
			// lblTXEQ2
			// 
			this.lblTXEQ2.Image = null;
			this.lblTXEQ2.Location = new System.Drawing.Point(128, 48);
			this.lblTXEQ2.Name = "lblTXEQ2";
			this.lblTXEQ2.Size = new System.Drawing.Size(32, 16);
			this.lblTXEQ2.TabIndex = 75;
			this.lblTXEQ2.Text = "Mid";
			this.toolTip1.SetToolTip(this.lblTXEQ2, "400-1500Hz");
			// 
			// lblTXEQ3
			// 
			this.lblTXEQ3.Image = null;
			this.lblTXEQ3.Location = new System.Drawing.Point(168, 48);
			this.lblTXEQ3.Name = "lblTXEQ3";
			this.lblTXEQ3.Size = new System.Drawing.Size(32, 16);
			this.lblTXEQ3.TabIndex = 76;
			this.lblTXEQ3.Text = "High";
			this.toolTip1.SetToolTip(this.lblTXEQ3, "1500-6000Hz");
			// 
			// lblTXEQPreamp
			// 
			this.lblTXEQPreamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblTXEQPreamp.Image = null;
			this.lblTXEQPreamp.Location = new System.Drawing.Point(8, 48);
			this.lblTXEQPreamp.Name = "lblTXEQPreamp";
			this.lblTXEQPreamp.Size = new System.Drawing.Size(48, 16);
			this.lblTXEQPreamp.TabIndex = 105;
			this.lblTXEQPreamp.Text = "Preamp";
			this.lblTXEQPreamp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tbTXEQPreamp
			// 
			this.tbTXEQPreamp.AutoSize = false;
			this.tbTXEQPreamp.LargeChange = 3;
			this.tbTXEQPreamp.Location = new System.Drawing.Point(16, 64);
			this.tbTXEQPreamp.Maximum = 15;
			this.tbTXEQPreamp.Minimum = -12;
			this.tbTXEQPreamp.Name = "tbTXEQPreamp";
			this.tbTXEQPreamp.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbTXEQPreamp.Size = new System.Drawing.Size(32, 128);
			this.tbTXEQPreamp.TabIndex = 36;
			this.tbTXEQPreamp.TickFrequency = 3;
			this.tbTXEQPreamp.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
			// 
			// lblTXEQ15db
			// 
			this.lblTXEQ15db.Image = null;
			this.lblTXEQ15db.Location = new System.Drawing.Point(204, 70);
			this.lblTXEQ15db.Name = "lblTXEQ15db";
			this.lblTXEQ15db.Size = new System.Drawing.Size(32, 16);
			this.lblTXEQ15db.TabIndex = 43;
			this.lblTXEQ15db.Text = "15dB";
			this.lblTXEQ15db.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTXEQ0dB
			// 
			this.lblTXEQ0dB.Image = null;
			this.lblTXEQ0dB.Location = new System.Drawing.Point(204, 126);
			this.lblTXEQ0dB.Name = "lblTXEQ0dB";
			this.lblTXEQ0dB.Size = new System.Drawing.Size(32, 16);
			this.lblTXEQ0dB.TabIndex = 0;
			this.lblTXEQ0dB.Text = "  0dB";
			this.lblTXEQ0dB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTXEQminus12db
			// 
			this.lblTXEQminus12db.Image = null;
			this.lblTXEQminus12db.Location = new System.Drawing.Point(200, 170);
			this.lblTXEQminus12db.Name = "lblTXEQminus12db";
			this.lblTXEQminus12db.Size = new System.Drawing.Size(34, 16);
			this.lblTXEQminus12db.TabIndex = 45;
			this.lblTXEQminus12db.Text = "-12dB";
			this.lblTXEQminus12db.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// EQForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 238);
			this.Controls.Add(this.grpRXEQ);
			this.Controls.Add(this.grpTXEQ);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EQForm";
			this.Text = "Equalizer Settings";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.EQForm_Closing);
			this.grpRXEQ.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQPreamp)).EndInit();
			this.grpTXEQ.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQPreamp)).EndInit();
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

			DB.SaveVars("EQForm", ref a);		// save the values to the DB
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

			ArrayList a = DB.GetVars("EQForm");						// Get the saved list of controls
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

		#region Properties

		public int[] RXEQ
		{
			get
			{
				int[] eq = new int[4];
				eq[0] = tbRXEQPreamp.Value;
				eq[1] = tbRXEQ1.Value;
				eq[2] = tbRXEQ2.Value;
				eq[3] = tbRXEQ3.Value;
				return eq;
			}

			set
			{
				// checks for min/max limits
				tbRXEQPreamp.Value = Math.Max(tbRXEQPreamp.Minimum, Math.Min(tbRXEQPreamp.Maximum, value[0]));
				tbRXEQ1.Value = Math.Max(tbRXEQ1.Minimum, Math.Min(tbRXEQ1.Maximum, value[1]));
				tbRXEQ2.Value = Math.Max(tbRXEQ2.Minimum, Math.Min(tbRXEQ2.Maximum, value[2]));
				tbRXEQ3.Value = Math.Max(tbRXEQ3.Minimum, Math.Min(tbRXEQ3.Maximum, value[3]));
			}
		}

		public int[] TXEQ
		{
			get 
			{
				int[] eq = new int[4];
				eq[0] = tbTXEQPreamp.Value;
				eq[1] = tbTXEQ1.Value;
				eq[2] = tbTXEQ2.Value;
				eq[3] = tbTXEQ3.Value;
				return eq;
			}
			set
			{
				tbTXEQPreamp.Value = Math.Max(tbTXEQ1.Minimum, Math.Min(tbTXEQ1.Maximum, value[0]));
				tbTXEQ1.Value = Math.Max(tbTXEQ1.Minimum, Math.Min(tbTXEQ1.Maximum, value[1]));
				tbTXEQ2.Value = Math.Max(tbTXEQ2.Minimum, Math.Min(tbTXEQ2.Maximum, value[2]));
				tbTXEQ3.Value = Math.Max(tbTXEQ3.Minimum, Math.Min(tbTXEQ3.Maximum, value[3]));
			}
		}

		public bool RXEQEnabled
		{
			get { return chkRXEQEnabled.Checked; }
			set { chkRXEQEnabled.Checked = value; }
		}

		public bool TXEQEnabled
		{
			get { return chkTXEQEnabled.Checked; }
			set { chkTXEQEnabled.Checked = value; }
		}

		#endregion

		#region Event Handlers

		private void EQForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
			SaveSettings();
		}

		private void tbRXEQ_Scroll(object sender, System.EventArgs e)
		{
			int[] rxeq = RXEQ;
			DttSP.SetGrphRXEQ(rxeq);
			picRXEQ.Invalidate();
		}

		private void tbTXEQ_Scroll(object sender, System.EventArgs e)
		{
			int[] txeq = TXEQ;
			DttSP.SetGrphTXEQ(txeq);
			picTXEQ.Invalidate();
		}

		private void picRXEQ_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int[] rxeq = RXEQ;
			if(!chkRXEQEnabled.Checked)
			{
				for(int i=0; i<rxeq.Length; i++)
					rxeq[i] = 0;
			}

			Point[] points = new Point[rxeq.Length-1];
			for(int i=1; i<rxeq.Length; i++)
			{
				points[i-1].X = (int)((i-1)*picRXEQ.Width/(float)(rxeq.Length-2));
				points[i-1].Y = picRXEQ.Height/2 - (int)(rxeq[i]*(picRXEQ.Height-6)/2/15.0f +
					tbRXEQPreamp.Value * 3 / 15.0f);
			}

			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, picRXEQ.Width, picRXEQ.Height);
			e.Graphics.DrawLines(new Pen(Color.LightGreen), points);
		}

		private void picTXEQ_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int[] txeq = TXEQ;
			if(!chkTXEQEnabled.Checked)
			{
				for(int i=0; i<txeq.Length; i++)
					txeq[i] = 0;
			}

			Point[] points = new Point[txeq.Length-1];
			for(int i=1; i<txeq.Length; i++)
			{
				points[i-1].X = (int)((i-1)*picTXEQ.Width/(float)(txeq.Length-2));
				points[i-1].Y = picTXEQ.Height/2 - (int)(txeq[i]*(picTXEQ.Height-6)/2/15.0f +
					tbTXEQPreamp.Value * 3 / 15.0f);
			}

			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, picTXEQ.Width, picTXEQ.Height);
			e.Graphics.DrawLines(new Pen(Color.LightGreen), points);
		}

		private void chkRXEQEnabled_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkRXEQEnabled.Checked)
				DttSP.SetGrphRXEQcmd(1);
			else
				DttSP.SetGrphRXEQcmd(0);
			picRXEQ.Invalidate();
		}

		private void chkTXEQEnabled_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkTXEQEnabled.Checked)
				DttSP.SetGrphTXEQcmd(1);
			else
				DttSP.SetGrphTXEQcmd(0);
			picTXEQ.Invalidate();
		}

		private void btnRXEQReset_Click(object sender, System.EventArgs e)
		{
			DialogResult dr = MessageBox.Show(
				"Are you sure you want to reset the Receive Equalizer\n"+
				"to flat (zero)?",
				"Are you sure?",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);
			
			if(dr == DialogResult.No)
				return;

			foreach(Control c in grpRXEQ.Controls)
			{
				if(c.GetType() == typeof(TrackBarTS))
					((TrackBarTS)c).Value = 0;
			}

			tbRXEQ_Scroll(this, EventArgs.Empty);
		}

		private void btnTXEQReset_Click(object sender, System.EventArgs e)
		{
			DialogResult dr = MessageBox.Show(
				"Are you sure you want to reset the Transmit Equalizer\n"+
				"to flat (zero)?",
				"Are you sure?",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);
			
			if(dr == DialogResult.No)
				return;

			foreach(Control c in grpTXEQ.Controls)
			{
				if(c.GetType() == typeof(TrackBarTS))
					((TrackBarTS)c).Value = 0;
			}

			tbTXEQ_Scroll(this, EventArgs.Empty);
		}

		private void chkTXEQ160Notch_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkTXEQ160Notch.Checked)
				DttSP.SetNotch160(1);
			else
				DttSP.SetNotch160(0);
		}

		#endregion		
	}
}