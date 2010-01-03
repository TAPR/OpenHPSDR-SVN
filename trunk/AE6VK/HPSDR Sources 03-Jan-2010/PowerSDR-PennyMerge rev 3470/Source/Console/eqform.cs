//=================================================================
// eqform.cs
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

		private Console console;
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
		private System.Windows.Forms.ButtonTS btnTXEQReset;
		private System.Windows.Forms.ButtonTS btnRXEQReset;
		private System.Windows.Forms.LabelTS lblRXEQ15db;
		private System.Windows.Forms.LabelTS lblTXEQ15db;
		private System.Windows.Forms.LabelTS lblRXEQminus12db;
		private System.Windows.Forms.LabelTS lblTXEQminus12db;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBoxTS chkTXEQ160Notch;
		private System.Windows.Forms.TrackBarTS tbRXEQ4;
		private System.Windows.Forms.TrackBarTS tbRXEQ5;
		private System.Windows.Forms.TrackBarTS tbRXEQ6;
		private System.Windows.Forms.TrackBarTS tbRXEQ7;
		private System.Windows.Forms.TrackBarTS tbRXEQ8;
		private System.Windows.Forms.TrackBarTS tbRXEQ9;
		private System.Windows.Forms.TrackBarTS tbRXEQ10;
		private System.Windows.Forms.LabelTS lblRXEQ15db2;
		private System.Windows.Forms.LabelTS lblRXEQ0dB2;
		private System.Windows.Forms.LabelTS lblRXEQminus12db2;
		private System.Windows.Forms.LabelTS lblTXEQ15db2;
		private System.Windows.Forms.LabelTS lblTXEQ0dB2;
		private System.Windows.Forms.LabelTS lblTXEQminus12db2;
		private System.Windows.Forms.RadioButtonTS rad3Band;
		private System.Windows.Forms.RadioButtonTS rad10Band;
		private System.Windows.Forms.TrackBarTS tbTXEQ4;
		private System.Windows.Forms.TrackBarTS tbTXEQ5;
		private System.Windows.Forms.TrackBarTS tbTXEQ6;
		private System.Windows.Forms.TrackBarTS tbTXEQ7;
		private System.Windows.Forms.TrackBarTS tbTXEQ8;
		private System.Windows.Forms.TrackBarTS tbTXEQ9;
		private System.Windows.Forms.TrackBarTS tbTXEQ10;
		private System.Windows.Forms.LabelTS lblRXEQ4;
		private System.Windows.Forms.LabelTS lblRXEQ5;
		private System.Windows.Forms.LabelTS lblRXEQ6;
		private System.Windows.Forms.LabelTS lblRXEQ7;
		private System.Windows.Forms.LabelTS lblRXEQ8;
		private System.Windows.Forms.LabelTS lblRXEQ9;
		private System.Windows.Forms.LabelTS lblRXEQ10;
		private System.Windows.Forms.LabelTS lblTXEQ4;
		private System.Windows.Forms.LabelTS lblTXEQ5;
		private System.Windows.Forms.LabelTS lblTXEQ6;
		private System.Windows.Forms.LabelTS lblTXEQ7;
		private System.Windows.Forms.LabelTS lblTXEQ8;
		private System.Windows.Forms.LabelTS lblTXEQ9;
		private System.Windows.Forms.LabelTS lblTXEQ10;
		private System.ComponentModel.IContainer components;
		
		#endregion

		#region Constructor and Destructor

		public EQForm(Console c)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			console = c;
			Common.RestoreForm(this, "EQForm", false);
            
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
			this.lblRXEQ15db2 = new System.Windows.Forms.LabelTS();
			this.lblRXEQ0dB2 = new System.Windows.Forms.LabelTS();
			this.lblRXEQminus12db2 = new System.Windows.Forms.LabelTS();
			this.lblRXEQ10 = new System.Windows.Forms.LabelTS();
			this.tbRXEQ10 = new System.Windows.Forms.TrackBarTS();
			this.lblRXEQ7 = new System.Windows.Forms.LabelTS();
			this.lblRXEQ8 = new System.Windows.Forms.LabelTS();
			this.lblRXEQ9 = new System.Windows.Forms.LabelTS();
			this.tbRXEQ7 = new System.Windows.Forms.TrackBarTS();
			this.tbRXEQ8 = new System.Windows.Forms.TrackBarTS();
			this.tbRXEQ9 = new System.Windows.Forms.TrackBarTS();
			this.tbRXEQ4 = new System.Windows.Forms.TrackBarTS();
			this.tbRXEQ5 = new System.Windows.Forms.TrackBarTS();
			this.tbRXEQ6 = new System.Windows.Forms.TrackBarTS();
			this.lblRXEQ4 = new System.Windows.Forms.LabelTS();
			this.lblRXEQ5 = new System.Windows.Forms.LabelTS();
			this.lblRXEQ6 = new System.Windows.Forms.LabelTS();
			this.picRXEQ = new System.Windows.Forms.PictureBox();
			this.btnRXEQReset = new System.Windows.Forms.ButtonTS();
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
			this.lblTXEQ15db2 = new System.Windows.Forms.LabelTS();
			this.lblTXEQ0dB2 = new System.Windows.Forms.LabelTS();
			this.lblTXEQminus12db2 = new System.Windows.Forms.LabelTS();
			this.tbTXEQ10 = new System.Windows.Forms.TrackBarTS();
			this.lblTXEQ10 = new System.Windows.Forms.LabelTS();
			this.tbTXEQ7 = new System.Windows.Forms.TrackBarTS();
			this.tbTXEQ8 = new System.Windows.Forms.TrackBarTS();
			this.tbTXEQ9 = new System.Windows.Forms.TrackBarTS();
			this.lblTXEQ7 = new System.Windows.Forms.LabelTS();
			this.lblTXEQ8 = new System.Windows.Forms.LabelTS();
			this.lblTXEQ9 = new System.Windows.Forms.LabelTS();
			this.tbTXEQ4 = new System.Windows.Forms.TrackBarTS();
			this.tbTXEQ5 = new System.Windows.Forms.TrackBarTS();
			this.tbTXEQ6 = new System.Windows.Forms.TrackBarTS();
			this.lblTXEQ4 = new System.Windows.Forms.LabelTS();
			this.lblTXEQ5 = new System.Windows.Forms.LabelTS();
			this.lblTXEQ6 = new System.Windows.Forms.LabelTS();
			this.chkTXEQ160Notch = new System.Windows.Forms.CheckBoxTS();
			this.picTXEQ = new System.Windows.Forms.PictureBox();
			this.btnTXEQReset = new System.Windows.Forms.ButtonTS();
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
			this.rad3Band = new System.Windows.Forms.RadioButtonTS();
			this.rad10Band = new System.Windows.Forms.RadioButtonTS();
			this.grpRXEQ.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ10)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ8)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ9)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQPreamp)).BeginInit();
			this.grpTXEQ.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ10)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ8)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ9)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQPreamp)).BeginInit();
			this.SuspendLayout();
			// 
			// grpRXEQ
			// 
			this.grpRXEQ.Controls.Add(this.lblRXEQ15db2);
			this.grpRXEQ.Controls.Add(this.lblRXEQ0dB2);
			this.grpRXEQ.Controls.Add(this.lblRXEQminus12db2);
			this.grpRXEQ.Controls.Add(this.lblRXEQ10);
			this.grpRXEQ.Controls.Add(this.tbRXEQ10);
			this.grpRXEQ.Controls.Add(this.lblRXEQ7);
			this.grpRXEQ.Controls.Add(this.lblRXEQ8);
			this.grpRXEQ.Controls.Add(this.lblRXEQ9);
			this.grpRXEQ.Controls.Add(this.tbRXEQ7);
			this.grpRXEQ.Controls.Add(this.tbRXEQ8);
			this.grpRXEQ.Controls.Add(this.tbRXEQ9);
			this.grpRXEQ.Controls.Add(this.tbRXEQ4);
			this.grpRXEQ.Controls.Add(this.tbRXEQ5);
			this.grpRXEQ.Controls.Add(this.tbRXEQ6);
			this.grpRXEQ.Controls.Add(this.lblRXEQ4);
			this.grpRXEQ.Controls.Add(this.lblRXEQ5);
			this.grpRXEQ.Controls.Add(this.lblRXEQ6);
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
			this.grpRXEQ.Location = new System.Drawing.Point(8, 40);
			this.grpRXEQ.Name = "grpRXEQ";
			this.grpRXEQ.Size = new System.Drawing.Size(528, 224);
			this.grpRXEQ.TabIndex = 1;
			this.grpRXEQ.TabStop = false;
			this.grpRXEQ.Text = "Receive Equalizer";
			// 
			// lblRXEQ15db2
			// 
			this.lblRXEQ15db2.Image = null;
			this.lblRXEQ15db2.Location = new System.Drawing.Point(483, 78);
			this.lblRXEQ15db2.Name = "lblRXEQ15db2";
			this.lblRXEQ15db2.Size = new System.Drawing.Size(32, 16);
			this.lblRXEQ15db2.TabIndex = 126;
			this.lblRXEQ15db2.Text = "15dB";
			this.lblRXEQ15db2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblRXEQ0dB2
			// 
			this.lblRXEQ0dB2.Image = null;
			this.lblRXEQ0dB2.Location = new System.Drawing.Point(483, 134);
			this.lblRXEQ0dB2.Name = "lblRXEQ0dB2";
			this.lblRXEQ0dB2.Size = new System.Drawing.Size(32, 16);
			this.lblRXEQ0dB2.TabIndex = 127;
			this.lblRXEQ0dB2.Text = "  0dB";
			this.lblRXEQ0dB2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblRXEQminus12db2
			// 
			this.lblRXEQminus12db2.Image = null;
			this.lblRXEQminus12db2.Location = new System.Drawing.Point(480, 178);
			this.lblRXEQminus12db2.Name = "lblRXEQminus12db2";
			this.lblRXEQminus12db2.Size = new System.Drawing.Size(34, 16);
			this.lblRXEQminus12db2.TabIndex = 128;
			this.lblRXEQminus12db2.Text = "-12dB";
			this.lblRXEQminus12db2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblRXEQ10
			// 
			this.lblRXEQ10.Image = null;
			this.lblRXEQ10.Location = new System.Drawing.Point(440, 56);
			this.lblRXEQ10.Name = "lblRXEQ10";
			this.lblRXEQ10.Size = new System.Drawing.Size(40, 16);
			this.lblRXEQ10.TabIndex = 125;
			this.lblRXEQ10.Text = "16K";
			this.lblRXEQ10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblRXEQ10.Visible = false;
			// 
			// tbRXEQ10
			// 
			this.tbRXEQ10.AutoSize = false;
			this.tbRXEQ10.LargeChange = 3;
			this.tbRXEQ10.Location = new System.Drawing.Point(448, 72);
			this.tbRXEQ10.Maximum = 15;
			this.tbRXEQ10.Minimum = -12;
			this.tbRXEQ10.Name = "tbRXEQ10";
			this.tbRXEQ10.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbRXEQ10.Size = new System.Drawing.Size(32, 128);
			this.tbRXEQ10.TabIndex = 124;
			this.tbRXEQ10.TickFrequency = 3;
			this.tbRXEQ10.Visible = false;
			this.tbRXEQ10.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
			// 
			// lblRXEQ7
			// 
			this.lblRXEQ7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblRXEQ7.Image = null;
			this.lblRXEQ7.Location = new System.Drawing.Point(320, 56);
			this.lblRXEQ7.Name = "lblRXEQ7";
			this.lblRXEQ7.Size = new System.Drawing.Size(40, 16);
			this.lblRXEQ7.TabIndex = 121;
			this.lblRXEQ7.Text = "2K";
			this.lblRXEQ7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblRXEQ7.Visible = false;
			// 
			// lblRXEQ8
			// 
			this.lblRXEQ8.Image = null;
			this.lblRXEQ8.Location = new System.Drawing.Point(360, 56);
			this.lblRXEQ8.Name = "lblRXEQ8";
			this.lblRXEQ8.Size = new System.Drawing.Size(40, 16);
			this.lblRXEQ8.TabIndex = 122;
			this.lblRXEQ8.Text = "4K";
			this.lblRXEQ8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblRXEQ8.Visible = false;
			// 
			// lblRXEQ9
			// 
			this.lblRXEQ9.Image = null;
			this.lblRXEQ9.Location = new System.Drawing.Point(400, 56);
			this.lblRXEQ9.Name = "lblRXEQ9";
			this.lblRXEQ9.Size = new System.Drawing.Size(40, 16);
			this.lblRXEQ9.TabIndex = 123;
			this.lblRXEQ9.Text = "High";
			this.lblRXEQ9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.lblRXEQ9, "1500-6000Hz");
			// 
			// tbRXEQ7
			// 
			this.tbRXEQ7.AutoSize = false;
			this.tbRXEQ7.LargeChange = 3;
			this.tbRXEQ7.Location = new System.Drawing.Point(328, 72);
			this.tbRXEQ7.Maximum = 15;
			this.tbRXEQ7.Minimum = -12;
			this.tbRXEQ7.Name = "tbRXEQ7";
			this.tbRXEQ7.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbRXEQ7.Size = new System.Drawing.Size(32, 128);
			this.tbRXEQ7.TabIndex = 118;
			this.tbRXEQ7.TickFrequency = 3;
			this.tbRXEQ7.Visible = false;
			this.tbRXEQ7.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
			// 
			// tbRXEQ8
			// 
			this.tbRXEQ8.AutoSize = false;
			this.tbRXEQ8.LargeChange = 3;
			this.tbRXEQ8.Location = new System.Drawing.Point(368, 72);
			this.tbRXEQ8.Maximum = 15;
			this.tbRXEQ8.Minimum = -12;
			this.tbRXEQ8.Name = "tbRXEQ8";
			this.tbRXEQ8.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbRXEQ8.Size = new System.Drawing.Size(32, 128);
			this.tbRXEQ8.TabIndex = 119;
			this.tbRXEQ8.TickFrequency = 3;
			this.tbRXEQ8.Visible = false;
			this.tbRXEQ8.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
			// 
			// tbRXEQ9
			// 
			this.tbRXEQ9.AutoSize = false;
			this.tbRXEQ9.LargeChange = 3;
			this.tbRXEQ9.Location = new System.Drawing.Point(408, 72);
			this.tbRXEQ9.Maximum = 15;
			this.tbRXEQ9.Minimum = -12;
			this.tbRXEQ9.Name = "tbRXEQ9";
			this.tbRXEQ9.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbRXEQ9.Size = new System.Drawing.Size(32, 128);
			this.tbRXEQ9.TabIndex = 120;
			this.tbRXEQ9.TickFrequency = 3;
			this.tbRXEQ9.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
			// 
			// tbRXEQ4
			// 
			this.tbRXEQ4.AutoSize = false;
			this.tbRXEQ4.LargeChange = 3;
			this.tbRXEQ4.Location = new System.Drawing.Point(208, 72);
			this.tbRXEQ4.Maximum = 15;
			this.tbRXEQ4.Minimum = -12;
			this.tbRXEQ4.Name = "tbRXEQ4";
			this.tbRXEQ4.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbRXEQ4.Size = new System.Drawing.Size(32, 128);
			this.tbRXEQ4.TabIndex = 112;
			this.tbRXEQ4.TickFrequency = 3;
			this.tbRXEQ4.Visible = false;
			this.tbRXEQ4.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
			// 
			// tbRXEQ5
			// 
			this.tbRXEQ5.AutoSize = false;
			this.tbRXEQ5.LargeChange = 3;
			this.tbRXEQ5.Location = new System.Drawing.Point(248, 72);
			this.tbRXEQ5.Maximum = 15;
			this.tbRXEQ5.Minimum = -12;
			this.tbRXEQ5.Name = "tbRXEQ5";
			this.tbRXEQ5.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbRXEQ5.Size = new System.Drawing.Size(32, 128);
			this.tbRXEQ5.TabIndex = 113;
			this.tbRXEQ5.TickFrequency = 3;
			this.tbRXEQ5.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
			// 
			// tbRXEQ6
			// 
			this.tbRXEQ6.AutoSize = false;
			this.tbRXEQ6.LargeChange = 3;
			this.tbRXEQ6.Location = new System.Drawing.Point(288, 72);
			this.tbRXEQ6.Maximum = 15;
			this.tbRXEQ6.Minimum = -12;
			this.tbRXEQ6.Name = "tbRXEQ6";
			this.tbRXEQ6.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbRXEQ6.Size = new System.Drawing.Size(32, 128);
			this.tbRXEQ6.TabIndex = 114;
			this.tbRXEQ6.TickFrequency = 3;
			this.tbRXEQ6.Visible = false;
			this.tbRXEQ6.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
			// 
			// lblRXEQ4
			// 
			this.lblRXEQ4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblRXEQ4.Image = null;
			this.lblRXEQ4.Location = new System.Drawing.Point(200, 56);
			this.lblRXEQ4.Name = "lblRXEQ4";
			this.lblRXEQ4.Size = new System.Drawing.Size(40, 16);
			this.lblRXEQ4.TabIndex = 115;
			this.lblRXEQ4.Text = "250";
			this.lblRXEQ4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblRXEQ4.Visible = false;
			// 
			// lblRXEQ5
			// 
			this.lblRXEQ5.Image = null;
			this.lblRXEQ5.Location = new System.Drawing.Point(240, 56);
			this.lblRXEQ5.Name = "lblRXEQ5";
			this.lblRXEQ5.Size = new System.Drawing.Size(40, 16);
			this.lblRXEQ5.TabIndex = 116;
			this.lblRXEQ5.Text = "Mid";
			this.lblRXEQ5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.lblRXEQ5, "400-1500Hz");
			// 
			// lblRXEQ6
			// 
			this.lblRXEQ6.Image = null;
			this.lblRXEQ6.Location = new System.Drawing.Point(280, 56);
			this.lblRXEQ6.Name = "lblRXEQ6";
			this.lblRXEQ6.Size = new System.Drawing.Size(40, 16);
			this.lblRXEQ6.TabIndex = 117;
			this.lblRXEQ6.Text = "1K";
			this.lblRXEQ6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblRXEQ6.Visible = false;
			// 
			// picRXEQ
			// 
			this.picRXEQ.BackColor = System.Drawing.Color.Black;
			this.picRXEQ.Location = new System.Drawing.Point(88, 24);
			this.picRXEQ.Name = "picRXEQ";
			this.picRXEQ.Size = new System.Drawing.Size(384, 24);
			this.picRXEQ.TabIndex = 111;
			this.picRXEQ.TabStop = false;
			this.picRXEQ.Paint += new System.Windows.Forms.PaintEventHandler(this.picRXEQ_Paint);
			// 
			// btnRXEQReset
			// 
			this.btnRXEQReset.Image = null;
			this.btnRXEQReset.Location = new System.Drawing.Point(16, 200);
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
			this.tbRXEQ1.Location = new System.Drawing.Point(88, 72);
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
			this.tbRXEQ2.Location = new System.Drawing.Point(128, 72);
			this.tbRXEQ2.Maximum = 15;
			this.tbRXEQ2.Minimum = -12;
			this.tbRXEQ2.Name = "tbRXEQ2";
			this.tbRXEQ2.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbRXEQ2.Size = new System.Drawing.Size(32, 128);
			this.tbRXEQ2.TabIndex = 5;
			this.tbRXEQ2.TickFrequency = 3;
			this.tbRXEQ2.Visible = false;
			this.tbRXEQ2.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
			// 
			// tbRXEQ3
			// 
			this.tbRXEQ3.AutoSize = false;
			this.tbRXEQ3.LargeChange = 3;
			this.tbRXEQ3.Location = new System.Drawing.Point(168, 72);
			this.tbRXEQ3.Maximum = 15;
			this.tbRXEQ3.Minimum = -12;
			this.tbRXEQ3.Name = "tbRXEQ3";
			this.tbRXEQ3.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbRXEQ3.Size = new System.Drawing.Size(32, 128);
			this.tbRXEQ3.TabIndex = 6;
			this.tbRXEQ3.TickFrequency = 3;
			this.tbRXEQ3.Visible = false;
			this.tbRXEQ3.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
			// 
			// lblRXEQ1
			// 
			this.lblRXEQ1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblRXEQ1.Image = null;
			this.lblRXEQ1.Location = new System.Drawing.Point(80, 56);
			this.lblRXEQ1.Name = "lblRXEQ1";
			this.lblRXEQ1.Size = new System.Drawing.Size(40, 16);
			this.lblRXEQ1.TabIndex = 43;
			this.lblRXEQ1.Text = "Low";
			this.lblRXEQ1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.lblRXEQ1, "0-400Hz");
			// 
			// lblRXEQ2
			// 
			this.lblRXEQ2.Image = null;
			this.lblRXEQ2.Location = new System.Drawing.Point(120, 56);
			this.lblRXEQ2.Name = "lblRXEQ2";
			this.lblRXEQ2.Size = new System.Drawing.Size(40, 16);
			this.lblRXEQ2.TabIndex = 44;
			this.lblRXEQ2.Text = "63";
			this.lblRXEQ2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblRXEQ2.Visible = false;
			// 
			// lblRXEQ3
			// 
			this.lblRXEQ3.Image = null;
			this.lblRXEQ3.Location = new System.Drawing.Point(160, 56);
			this.lblRXEQ3.Name = "lblRXEQ3";
			this.lblRXEQ3.Size = new System.Drawing.Size(40, 16);
			this.lblRXEQ3.TabIndex = 45;
			this.lblRXEQ3.Text = "125";
			this.lblRXEQ3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblRXEQ3.Visible = false;
			// 
			// lblRXEQPreamp
			// 
			this.lblRXEQPreamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblRXEQPreamp.Image = null;
			this.lblRXEQPreamp.Location = new System.Drawing.Point(8, 56);
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
			this.tbRXEQPreamp.Location = new System.Drawing.Point(16, 72);
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
			this.lblRXEQ15db.Location = new System.Drawing.Point(56, 78);
			this.lblRXEQ15db.Name = "lblRXEQ15db";
			this.lblRXEQ15db.Size = new System.Drawing.Size(32, 16);
			this.lblRXEQ15db.TabIndex = 40;
			this.lblRXEQ15db.Text = "15dB";
			this.lblRXEQ15db.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblRXEQ0dB
			// 
			this.lblRXEQ0dB.Image = null;
			this.lblRXEQ0dB.Location = new System.Drawing.Point(56, 134);
			this.lblRXEQ0dB.Name = "lblRXEQ0dB";
			this.lblRXEQ0dB.Size = new System.Drawing.Size(32, 16);
			this.lblRXEQ0dB.TabIndex = 41;
			this.lblRXEQ0dB.Text = "  0dB";
			this.lblRXEQ0dB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblRXEQminus12db
			// 
			this.lblRXEQminus12db.Image = null;
			this.lblRXEQminus12db.Location = new System.Drawing.Point(52, 178);
			this.lblRXEQminus12db.Name = "lblRXEQminus12db";
			this.lblRXEQminus12db.Size = new System.Drawing.Size(34, 16);
			this.lblRXEQminus12db.TabIndex = 42;
			this.lblRXEQminus12db.Text = "-12dB";
			this.lblRXEQminus12db.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// grpTXEQ
			// 
			this.grpTXEQ.Controls.Add(this.lblTXEQ15db2);
			this.grpTXEQ.Controls.Add(this.lblTXEQ0dB2);
			this.grpTXEQ.Controls.Add(this.lblTXEQminus12db2);
			this.grpTXEQ.Controls.Add(this.tbTXEQ10);
			this.grpTXEQ.Controls.Add(this.lblTXEQ10);
			this.grpTXEQ.Controls.Add(this.tbTXEQ7);
			this.grpTXEQ.Controls.Add(this.tbTXEQ8);
			this.grpTXEQ.Controls.Add(this.tbTXEQ9);
			this.grpTXEQ.Controls.Add(this.lblTXEQ7);
			this.grpTXEQ.Controls.Add(this.lblTXEQ8);
			this.grpTXEQ.Controls.Add(this.lblTXEQ9);
			this.grpTXEQ.Controls.Add(this.tbTXEQ4);
			this.grpTXEQ.Controls.Add(this.tbTXEQ5);
			this.grpTXEQ.Controls.Add(this.tbTXEQ6);
			this.grpTXEQ.Controls.Add(this.lblTXEQ4);
			this.grpTXEQ.Controls.Add(this.lblTXEQ5);
			this.grpTXEQ.Controls.Add(this.lblTXEQ6);
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
			this.grpTXEQ.Location = new System.Drawing.Point(8, 272);
			this.grpTXEQ.Name = "grpTXEQ";
			this.grpTXEQ.Size = new System.Drawing.Size(528, 224);
			this.grpTXEQ.TabIndex = 1;
			this.grpTXEQ.TabStop = false;
			this.grpTXEQ.Text = "Transmit Equalizer";
			// 
			// lblTXEQ15db2
			// 
			this.lblTXEQ15db2.Image = null;
			this.lblTXEQ15db2.Location = new System.Drawing.Point(483, 78);
			this.lblTXEQ15db2.Name = "lblTXEQ15db2";
			this.lblTXEQ15db2.Size = new System.Drawing.Size(32, 16);
			this.lblTXEQ15db2.TabIndex = 129;
			this.lblTXEQ15db2.Text = "15dB";
			this.lblTXEQ15db2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTXEQ0dB2
			// 
			this.lblTXEQ0dB2.Image = null;
			this.lblTXEQ0dB2.Location = new System.Drawing.Point(483, 134);
			this.lblTXEQ0dB2.Name = "lblTXEQ0dB2";
			this.lblTXEQ0dB2.Size = new System.Drawing.Size(32, 16);
			this.lblTXEQ0dB2.TabIndex = 128;
			this.lblTXEQ0dB2.Text = "  0dB";
			this.lblTXEQ0dB2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTXEQminus12db2
			// 
			this.lblTXEQminus12db2.Image = null;
			this.lblTXEQminus12db2.Location = new System.Drawing.Point(480, 178);
			this.lblTXEQminus12db2.Name = "lblTXEQminus12db2";
			this.lblTXEQminus12db2.Size = new System.Drawing.Size(34, 16);
			this.lblTXEQminus12db2.TabIndex = 130;
			this.lblTXEQminus12db2.Text = "-12dB";
			this.lblTXEQminus12db2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tbTXEQ10
			// 
			this.tbTXEQ10.AutoSize = false;
			this.tbTXEQ10.LargeChange = 3;
			this.tbTXEQ10.Location = new System.Drawing.Point(448, 72);
			this.tbTXEQ10.Maximum = 15;
			this.tbTXEQ10.Minimum = -12;
			this.tbTXEQ10.Name = "tbTXEQ10";
			this.tbTXEQ10.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbTXEQ10.Size = new System.Drawing.Size(32, 128);
			this.tbTXEQ10.TabIndex = 126;
			this.tbTXEQ10.TickFrequency = 3;
			this.tbTXEQ10.Visible = false;
			this.tbTXEQ10.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
			// 
			// lblTXEQ10
			// 
			this.lblTXEQ10.Image = null;
			this.lblTXEQ10.Location = new System.Drawing.Point(440, 56);
			this.lblTXEQ10.Name = "lblTXEQ10";
			this.lblTXEQ10.Size = new System.Drawing.Size(40, 16);
			this.lblTXEQ10.TabIndex = 127;
			this.lblTXEQ10.Text = "16K";
			this.lblTXEQ10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.lblTXEQ10, "1500-6000Hz");
			this.lblTXEQ10.Visible = false;
			// 
			// tbTXEQ7
			// 
			this.tbTXEQ7.AutoSize = false;
			this.tbTXEQ7.LargeChange = 3;
			this.tbTXEQ7.Location = new System.Drawing.Point(328, 72);
			this.tbTXEQ7.Maximum = 15;
			this.tbTXEQ7.Minimum = -12;
			this.tbTXEQ7.Name = "tbTXEQ7";
			this.tbTXEQ7.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbTXEQ7.Size = new System.Drawing.Size(32, 128);
			this.tbTXEQ7.TabIndex = 120;
			this.tbTXEQ7.TickFrequency = 3;
			this.tbTXEQ7.Visible = false;
			this.tbTXEQ7.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
			// 
			// tbTXEQ8
			// 
			this.tbTXEQ8.AutoSize = false;
			this.tbTXEQ8.LargeChange = 3;
			this.tbTXEQ8.Location = new System.Drawing.Point(368, 72);
			this.tbTXEQ8.Maximum = 15;
			this.tbTXEQ8.Minimum = -12;
			this.tbTXEQ8.Name = "tbTXEQ8";
			this.tbTXEQ8.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbTXEQ8.Size = new System.Drawing.Size(32, 128);
			this.tbTXEQ8.TabIndex = 121;
			this.tbTXEQ8.TickFrequency = 3;
			this.tbTXEQ8.Visible = false;
			this.tbTXEQ8.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
			// 
			// tbTXEQ9
			// 
			this.tbTXEQ9.AutoSize = false;
			this.tbTXEQ9.LargeChange = 3;
			this.tbTXEQ9.Location = new System.Drawing.Point(408, 72);
			this.tbTXEQ9.Maximum = 15;
			this.tbTXEQ9.Minimum = -12;
			this.tbTXEQ9.Name = "tbTXEQ9";
			this.tbTXEQ9.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbTXEQ9.Size = new System.Drawing.Size(32, 128);
			this.tbTXEQ9.TabIndex = 122;
			this.tbTXEQ9.TickFrequency = 3;
			this.tbTXEQ9.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
			// 
			// lblTXEQ7
			// 
			this.lblTXEQ7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblTXEQ7.Image = null;
			this.lblTXEQ7.Location = new System.Drawing.Point(320, 56);
			this.lblTXEQ7.Name = "lblTXEQ7";
			this.lblTXEQ7.Size = new System.Drawing.Size(40, 16);
			this.lblTXEQ7.TabIndex = 123;
			this.lblTXEQ7.Text = "2K";
			this.lblTXEQ7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.lblTXEQ7, "0-400Hz");
			this.lblTXEQ7.Visible = false;
			// 
			// lblTXEQ8
			// 
			this.lblTXEQ8.Image = null;
			this.lblTXEQ8.Location = new System.Drawing.Point(360, 56);
			this.lblTXEQ8.Name = "lblTXEQ8";
			this.lblTXEQ8.Size = new System.Drawing.Size(40, 16);
			this.lblTXEQ8.TabIndex = 124;
			this.lblTXEQ8.Text = "4K";
			this.lblTXEQ8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.lblTXEQ8, "400-1500Hz");
			this.lblTXEQ8.Visible = false;
			// 
			// lblTXEQ9
			// 
			this.lblTXEQ9.Image = null;
			this.lblTXEQ9.Location = new System.Drawing.Point(400, 56);
			this.lblTXEQ9.Name = "lblTXEQ9";
			this.lblTXEQ9.Size = new System.Drawing.Size(40, 16);
			this.lblTXEQ9.TabIndex = 125;
			this.lblTXEQ9.Text = "High";
			this.lblTXEQ9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.lblTXEQ9, "1500-6000Hz");
			// 
			// tbTXEQ4
			// 
			this.tbTXEQ4.AutoSize = false;
			this.tbTXEQ4.LargeChange = 3;
			this.tbTXEQ4.Location = new System.Drawing.Point(208, 72);
			this.tbTXEQ4.Maximum = 15;
			this.tbTXEQ4.Minimum = -12;
			this.tbTXEQ4.Name = "tbTXEQ4";
			this.tbTXEQ4.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbTXEQ4.Size = new System.Drawing.Size(32, 128);
			this.tbTXEQ4.TabIndex = 114;
			this.tbTXEQ4.TickFrequency = 3;
			this.tbTXEQ4.Visible = false;
			this.tbTXEQ4.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
			// 
			// tbTXEQ5
			// 
			this.tbTXEQ5.AutoSize = false;
			this.tbTXEQ5.LargeChange = 3;
			this.tbTXEQ5.Location = new System.Drawing.Point(248, 72);
			this.tbTXEQ5.Maximum = 15;
			this.tbTXEQ5.Minimum = -12;
			this.tbTXEQ5.Name = "tbTXEQ5";
			this.tbTXEQ5.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbTXEQ5.Size = new System.Drawing.Size(32, 128);
			this.tbTXEQ5.TabIndex = 115;
			this.tbTXEQ5.TickFrequency = 3;
			this.tbTXEQ5.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
			// 
			// tbTXEQ6
			// 
			this.tbTXEQ6.AutoSize = false;
			this.tbTXEQ6.LargeChange = 3;
			this.tbTXEQ6.Location = new System.Drawing.Point(288, 72);
			this.tbTXEQ6.Maximum = 15;
			this.tbTXEQ6.Minimum = -12;
			this.tbTXEQ6.Name = "tbTXEQ6";
			this.tbTXEQ6.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbTXEQ6.Size = new System.Drawing.Size(32, 128);
			this.tbTXEQ6.TabIndex = 116;
			this.tbTXEQ6.TickFrequency = 3;
			this.tbTXEQ6.Visible = false;
			this.tbTXEQ6.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
			// 
			// lblTXEQ4
			// 
			this.lblTXEQ4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblTXEQ4.Image = null;
			this.lblTXEQ4.Location = new System.Drawing.Point(200, 56);
			this.lblTXEQ4.Name = "lblTXEQ4";
			this.lblTXEQ4.Size = new System.Drawing.Size(40, 16);
			this.lblTXEQ4.TabIndex = 117;
			this.lblTXEQ4.Text = "250";
			this.lblTXEQ4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.lblTXEQ4, "0-400Hz");
			this.lblTXEQ4.Visible = false;
			// 
			// lblTXEQ5
			// 
			this.lblTXEQ5.Image = null;
			this.lblTXEQ5.Location = new System.Drawing.Point(240, 56);
			this.lblTXEQ5.Name = "lblTXEQ5";
			this.lblTXEQ5.Size = new System.Drawing.Size(40, 16);
			this.lblTXEQ5.TabIndex = 118;
			this.lblTXEQ5.Text = "Mid";
			this.lblTXEQ5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.lblTXEQ5, "400-1500Hz");
			// 
			// lblTXEQ6
			// 
			this.lblTXEQ6.Image = null;
			this.lblTXEQ6.Location = new System.Drawing.Point(280, 56);
			this.lblTXEQ6.Name = "lblTXEQ6";
			this.lblTXEQ6.Size = new System.Drawing.Size(40, 16);
			this.lblTXEQ6.TabIndex = 119;
			this.lblTXEQ6.Text = "1K";
			this.lblTXEQ6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.lblTXEQ6, "1500-6000Hz");
			this.lblTXEQ6.Visible = false;
			// 
			// chkTXEQ160Notch
			// 
			this.chkTXEQ160Notch.Image = null;
			this.chkTXEQ160Notch.Location = new System.Drawing.Point(104, 200);
			this.chkTXEQ160Notch.Name = "chkTXEQ160Notch";
			this.chkTXEQ160Notch.Size = new System.Drawing.Size(96, 16);
			this.chkTXEQ160Notch.TabIndex = 113;
			this.chkTXEQ160Notch.Text = "160Hz Notch";
			this.chkTXEQ160Notch.Visible = false;
			// 
			// picTXEQ
			// 
			this.picTXEQ.BackColor = System.Drawing.Color.Black;
			this.picTXEQ.Location = new System.Drawing.Point(88, 24);
			this.picTXEQ.Name = "picTXEQ";
			this.picTXEQ.Size = new System.Drawing.Size(384, 24);
			this.picTXEQ.TabIndex = 112;
			this.picTXEQ.TabStop = false;
			this.picTXEQ.Paint += new System.Windows.Forms.PaintEventHandler(this.picTXEQ_Paint);
			// 
			// btnTXEQReset
			// 
			this.btnTXEQReset.Image = null;
			this.btnTXEQReset.Location = new System.Drawing.Point(16, 200);
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
			this.tbTXEQ1.Location = new System.Drawing.Point(88, 72);
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
			this.tbTXEQ2.Location = new System.Drawing.Point(128, 72);
			this.tbTXEQ2.Maximum = 15;
			this.tbTXEQ2.Minimum = -12;
			this.tbTXEQ2.Name = "tbTXEQ2";
			this.tbTXEQ2.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbTXEQ2.Size = new System.Drawing.Size(32, 128);
			this.tbTXEQ2.TabIndex = 5;
			this.tbTXEQ2.TickFrequency = 3;
			this.tbTXEQ2.Visible = false;
			this.tbTXEQ2.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
			// 
			// tbTXEQ3
			// 
			this.tbTXEQ3.AutoSize = false;
			this.tbTXEQ3.LargeChange = 3;
			this.tbTXEQ3.Location = new System.Drawing.Point(168, 72);
			this.tbTXEQ3.Maximum = 15;
			this.tbTXEQ3.Minimum = -12;
			this.tbTXEQ3.Name = "tbTXEQ3";
			this.tbTXEQ3.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbTXEQ3.Size = new System.Drawing.Size(32, 128);
			this.tbTXEQ3.TabIndex = 6;
			this.tbTXEQ3.TickFrequency = 3;
			this.tbTXEQ3.Visible = false;
			this.tbTXEQ3.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
			// 
			// lblTXEQ1
			// 
			this.lblTXEQ1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblTXEQ1.Image = null;
			this.lblTXEQ1.Location = new System.Drawing.Point(80, 56);
			this.lblTXEQ1.Name = "lblTXEQ1";
			this.lblTXEQ1.Size = new System.Drawing.Size(40, 16);
			this.lblTXEQ1.TabIndex = 74;
			this.lblTXEQ1.Text = "Low";
			this.lblTXEQ1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.lblTXEQ1, "0-400Hz");
			// 
			// lblTXEQ2
			// 
			this.lblTXEQ2.Image = null;
			this.lblTXEQ2.Location = new System.Drawing.Point(120, 56);
			this.lblTXEQ2.Name = "lblTXEQ2";
			this.lblTXEQ2.Size = new System.Drawing.Size(40, 16);
			this.lblTXEQ2.TabIndex = 75;
			this.lblTXEQ2.Text = "63";
			this.lblTXEQ2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.lblTXEQ2, "400-1500Hz");
			this.lblTXEQ2.Visible = false;
			// 
			// lblTXEQ3
			// 
			this.lblTXEQ3.Image = null;
			this.lblTXEQ3.Location = new System.Drawing.Point(160, 56);
			this.lblTXEQ3.Name = "lblTXEQ3";
			this.lblTXEQ3.Size = new System.Drawing.Size(40, 16);
			this.lblTXEQ3.TabIndex = 76;
			this.lblTXEQ3.Text = "125";
			this.lblTXEQ3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.lblTXEQ3, "1500-6000Hz");
			this.lblTXEQ3.Visible = false;
			// 
			// lblTXEQPreamp
			// 
			this.lblTXEQPreamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblTXEQPreamp.Image = null;
			this.lblTXEQPreamp.Location = new System.Drawing.Point(8, 56);
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
			this.tbTXEQPreamp.Location = new System.Drawing.Point(16, 72);
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
			this.lblTXEQ15db.Location = new System.Drawing.Point(56, 80);
			this.lblTXEQ15db.Name = "lblTXEQ15db";
			this.lblTXEQ15db.Size = new System.Drawing.Size(32, 16);
			this.lblTXEQ15db.TabIndex = 43;
			this.lblTXEQ15db.Text = "15dB";
			this.lblTXEQ15db.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTXEQ0dB
			// 
			this.lblTXEQ0dB.Image = null;
			this.lblTXEQ0dB.Location = new System.Drawing.Point(56, 134);
			this.lblTXEQ0dB.Name = "lblTXEQ0dB";
			this.lblTXEQ0dB.Size = new System.Drawing.Size(32, 16);
			this.lblTXEQ0dB.TabIndex = 0;
			this.lblTXEQ0dB.Text = "  0dB";
			this.lblTXEQ0dB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTXEQminus12db
			// 
			this.lblTXEQminus12db.Image = null;
			this.lblTXEQminus12db.Location = new System.Drawing.Point(52, 176);
			this.lblTXEQminus12db.Name = "lblTXEQminus12db";
			this.lblTXEQminus12db.Size = new System.Drawing.Size(34, 16);
			this.lblTXEQminus12db.TabIndex = 45;
			this.lblTXEQminus12db.Text = "-12dB";
			this.lblTXEQminus12db.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// rad3Band
			// 
			this.rad3Band.Checked = true;
			this.rad3Band.Image = null;
			this.rad3Band.Location = new System.Drawing.Point(16, 8);
			this.rad3Band.Name = "rad3Band";
			this.rad3Band.Size = new System.Drawing.Size(120, 24);
			this.rad3Band.TabIndex = 2;
			this.rad3Band.TabStop = true;
			this.rad3Band.Text = "3-Band Equalizer";
			this.rad3Band.CheckedChanged += new System.EventHandler(this.rad3Band_CheckedChanged);
			// 
			// rad10Band
			// 
			this.rad10Band.Image = null;
			this.rad10Band.Location = new System.Drawing.Point(144, 8);
			this.rad10Band.Name = "rad10Band";
			this.rad10Band.Size = new System.Drawing.Size(120, 24);
			this.rad10Band.TabIndex = 3;
			this.rad10Band.Text = "10-Band Equalizer";
			this.rad10Band.CheckedChanged += new System.EventHandler(this.rad10Band_CheckedChanged);
			// 
			// EQForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(544, 502);
			this.Controls.Add(this.rad10Band);
			this.Controls.Add(this.rad3Band);
			this.Controls.Add(this.grpRXEQ);
			this.Controls.Add(this.grpTXEQ);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EQForm";
			this.Text = "Equalizer Settings";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.EQForm_Closing);
			this.grpRXEQ.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ10)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ8)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ9)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQ3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbRXEQPreamp)).EndInit();
			this.grpTXEQ.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ10)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ8)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ9)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQ3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbTXEQPreamp)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties

		public int NumBands
		{
			get 
			{
				if(rad3Band.Checked) return 3;
				else return 10;
			}
			set
			{
				switch(value)
				{
					case 3: rad3Band.Checked = true; break;
					case 10: rad10Band.Checked = true; break;
				}
			}
		}

		public int[] RXEQ
		{
			get
			{
				if(rad3Band.Checked)
				{
					int[] eq = new int[4];
					eq[0] = tbRXEQPreamp.Value;
					eq[1] = tbRXEQ1.Value;
					eq[2] = tbRXEQ5.Value;
					eq[3] = tbRXEQ9.Value;
					return eq;
				}
				else //if(rad10Band.Checked)
				{
					int[] eq = new int[11];
					eq[0] = tbRXEQPreamp.Value;
					eq[1] = tbRXEQ1.Value;
					eq[2] = tbRXEQ2.Value;
					eq[3] = tbRXEQ3.Value;
					eq[4] = tbRXEQ4.Value;
					eq[5] = tbRXEQ5.Value;
					eq[6] = tbRXEQ6.Value;
					eq[7] = tbRXEQ7.Value;
					eq[8] = tbRXEQ8.Value;
					eq[9] = tbRXEQ9.Value;
					eq[10] = tbRXEQ10.Value;
					return eq;
				}
			}

			set
			{
				if(rad3Band.Checked)
				{
					if(value.Length < 4) 
					{
						MessageBox.Show("Error setting RX EQ");
						return;
					}
					tbRXEQPreamp.Value = Math.Max(tbRXEQPreamp.Minimum, Math.Min(tbRXEQPreamp.Maximum, value[0]));
					tbRXEQ1.Value = Math.Max(tbRXEQ1.Minimum, Math.Min(tbRXEQ1.Maximum, value[1]));
					tbRXEQ5.Value = Math.Max(tbRXEQ5.Minimum, Math.Min(tbRXEQ5.Maximum, value[2]));
					tbRXEQ9.Value = Math.Max(tbRXEQ9.Minimum, Math.Min(tbRXEQ9.Maximum, value[3]));					
				}
				else if(rad10Band.Checked)
				{
					if(value.Length < 11)
					{
						MessageBox.Show("Error setting RX EQ");
						return; 
					}
					tbRXEQPreamp.Value = Math.Max(tbRXEQPreamp.Minimum, Math.Min(tbRXEQPreamp.Maximum, value[0]));
					tbRXEQ1.Value = Math.Max(tbRXEQ1.Minimum, Math.Min(tbRXEQ1.Maximum, value[1]));
					tbRXEQ2.Value = Math.Max(tbRXEQ2.Minimum, Math.Min(tbRXEQ2.Maximum, value[2]));
					tbRXEQ3.Value = Math.Max(tbRXEQ3.Minimum, Math.Min(tbRXEQ3.Maximum, value[3]));	
					tbRXEQ4.Value = Math.Max(tbRXEQ4.Minimum, Math.Min(tbRXEQ4.Maximum, value[4]));	
					tbRXEQ5.Value = Math.Max(tbRXEQ5.Minimum, Math.Min(tbRXEQ5.Maximum, value[5]));	
					tbRXEQ6.Value = Math.Max(tbRXEQ6.Minimum, Math.Min(tbRXEQ6.Maximum, value[6]));	
					tbRXEQ7.Value = Math.Max(tbRXEQ7.Minimum, Math.Min(tbRXEQ7.Maximum, value[7]));	
					tbRXEQ8.Value = Math.Max(tbRXEQ8.Minimum, Math.Min(tbRXEQ8.Maximum, value[8]));	
					tbRXEQ9.Value = Math.Max(tbRXEQ9.Minimum, Math.Min(tbRXEQ9.Maximum, value[9]));	
					tbRXEQ10.Value = Math.Max(tbRXEQ10.Minimum, Math.Min(tbRXEQ10.Maximum, value[10]));	
				}

				picRXEQ.Invalidate();
				tbRXEQ_Scroll(this, EventArgs.Empty);
			}
		}

		public int[] TXEQ
		{
			get 
			{
				if(rad3Band.Checked)
				{
					int[] eq = new int[4];
					eq[0] = tbTXEQPreamp.Value;
					eq[1] = tbTXEQ1.Value;
					eq[2] = tbTXEQ5.Value;
					eq[3] = tbTXEQ9.Value;
					return eq;
				}
				else //if(rad10Band.Checked)
				{
					int[] eq = new int[11];
					eq[0] = tbTXEQPreamp.Value;
					eq[1] = tbTXEQ1.Value;
					eq[2] = tbTXEQ2.Value;
					eq[3] = tbTXEQ3.Value;
					eq[4] = tbTXEQ4.Value;
					eq[5] = tbTXEQ5.Value;
					eq[6] = tbTXEQ6.Value;
					eq[7] = tbTXEQ7.Value;
					eq[8] = tbTXEQ8.Value;
					eq[9] = tbTXEQ9.Value;
					eq[10] = tbTXEQ10.Value;
					return eq;
				}
			}
			set
			{
				if(rad3Band.Checked)
				{
					if(value.Length < 4)
					{
						MessageBox.Show("Error setting TX EQ");
						return;
					}
					tbTXEQPreamp.Value = Math.Max(tbTXEQPreamp.Minimum, Math.Min(tbTXEQPreamp.Maximum, value[0]));
					tbTXEQ1.Value = Math.Max(tbTXEQ1.Minimum, Math.Min(tbTXEQ1.Maximum, value[1]));
					tbTXEQ5.Value = Math.Max(tbTXEQ5.Minimum, Math.Min(tbTXEQ5.Maximum, value[2]));
					tbTXEQ9.Value = Math.Max(tbTXEQ9.Minimum, Math.Min(tbTXEQ9.Maximum, value[3]));
				}
				else if(rad10Band.Checked)
				{
					if(value.Length < 11)
					{
						MessageBox.Show("Error setting TX EQ");
						return;
					}
					tbTXEQPreamp.Value = Math.Max(tbTXEQPreamp.Minimum, Math.Min(tbTXEQPreamp.Maximum, value[0]));
					tbTXEQ1.Value = Math.Max(tbTXEQ1.Minimum, Math.Min(tbTXEQ1.Maximum, value[1]));
					tbTXEQ2.Value = Math.Max(tbTXEQ2.Minimum, Math.Min(tbTXEQ2.Maximum, value[2]));
					tbTXEQ3.Value = Math.Max(tbTXEQ3.Minimum, Math.Min(tbTXEQ3.Maximum, value[3]));
					tbTXEQ4.Value = Math.Max(tbTXEQ4.Minimum, Math.Min(tbTXEQ4.Maximum, value[4]));
					tbTXEQ5.Value = Math.Max(tbTXEQ5.Minimum, Math.Min(tbTXEQ5.Maximum, value[5]));
					tbTXEQ6.Value = Math.Max(tbTXEQ6.Minimum, Math.Min(tbTXEQ6.Maximum, value[6]));
					tbTXEQ7.Value = Math.Max(tbTXEQ7.Minimum, Math.Min(tbTXEQ7.Maximum, value[7]));
					tbTXEQ8.Value = Math.Max(tbTXEQ8.Minimum, Math.Min(tbTXEQ8.Maximum, value[8]));
					tbTXEQ9.Value = Math.Max(tbTXEQ9.Minimum, Math.Min(tbTXEQ9.Maximum, value[9]));
					tbTXEQ10.Value = Math.Max(tbTXEQ10.Minimum, Math.Min(tbTXEQ10.Maximum, value[10]));
				}
				picTXEQ.Invalidate();
				tbTXEQ_Scroll(this, EventArgs.Empty);
			}
		}

		public bool RXEQEnabled
		{
			get
			{
				if(chkRXEQEnabled != null) return chkRXEQEnabled.Checked;
				else return false;
			}
			set
			{
				if(chkRXEQEnabled != null) chkRXEQEnabled.Checked = value;
			}
		}

		public bool TXEQEnabled
		{
			get
			{
				if(chkTXEQEnabled != null) return chkTXEQEnabled.Checked;
				else return false;
			}
			set 
			{
				if(chkTXEQEnabled != null) chkTXEQEnabled.Checked = value;
			}
		}

		#endregion

		#region Event Handlers

		private void EQForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
			Common.SaveForm(this, "EQForm");
		}

		private void tbRXEQ_Scroll(object sender, System.EventArgs e)
		{
			int[] rxeq = RXEQ;
			if(rad3Band.Checked)
			{
				console.radio.GetDSPRX(0, 0).RXEQ3 = rxeq;
				console.radio.GetDSPRX(0, 1).RXEQ3 = rxeq;
				console.radio.GetDSPRX(1, 0).RXEQ3 = rxeq;
			}
			else
			{
				console.radio.GetDSPRX(0, 0).RXEQ10 = rxeq;
				console.radio.GetDSPRX(0, 1).RXEQ10 = rxeq;
				console.radio.GetDSPRX(1, 0).RXEQ10 = rxeq;
			}
			picRXEQ.Invalidate();
		}

		private void tbTXEQ_Scroll(object sender, System.EventArgs e)
		{
			int[] txeq = TXEQ;
			if(rad3Band.Checked) 
			{
				console.radio.GetDSPTX(0).TXEQ3 = txeq;
			}
			else
			{
				console.radio.GetDSPTX(0).TXEQ10 = txeq;
			}
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
			console.radio.GetDSPRX(0, 0).RXEQOn = chkRXEQEnabled.Checked;
			picRXEQ.Invalidate();
			console.RXEQ = chkRXEQEnabled.Checked;
		}

		private void chkTXEQEnabled_CheckedChanged(object sender, System.EventArgs e)
		{
			console.radio.GetDSPTX(0).TXEQOn = chkTXEQEnabled.Checked;
			picTXEQ.Invalidate();
			console.TXEQ = chkTXEQEnabled.Checked;
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
			console.radio.GetDSPTX(0).Notch160 = chkTXEQ160Notch.Checked;
		}

		private void rad3Band_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rad3Band.Checked)
			{
				lblRXEQ2.Visible = false;
				lblRXEQ3.Visible = false;
				lblRXEQ4.Visible = false;
				lblRXEQ6.Visible = false;
				lblRXEQ7.Visible = false;
				lblRXEQ8.Visible = false;
				lblRXEQ10.Visible = false;

				tbRXEQ2.Visible = false;
				tbRXEQ3.Visible = false;
				tbRXEQ4.Visible = false;
				tbRXEQ6.Visible = false;
				tbRXEQ7.Visible = false;
				tbRXEQ8.Visible = false;
				tbRXEQ10.Visible = false;

				lblRXEQ1.Text = "Low";
				lblRXEQ5.Text = "Mid";
				lblRXEQ9.Text = "High";

				toolTip1.SetToolTip(lblRXEQ1, "0-400Hz");
				toolTip1.SetToolTip(tbRXEQ1, "0-400Hz");
				toolTip1.SetToolTip(lblRXEQ5, "400-1500Hz");
				toolTip1.SetToolTip(tbRXEQ5, "400-1500Hz");
				toolTip1.SetToolTip(lblRXEQ9, "1500-6000Hz");
				toolTip1.SetToolTip(tbRXEQ9, "1500-6000Hz");

				lblTXEQ2.Visible = false;
				lblTXEQ3.Visible = false;
				lblTXEQ4.Visible = false;
				lblTXEQ6.Visible = false;
				lblTXEQ7.Visible = false;
				lblTXEQ8.Visible = false;
				lblTXEQ10.Visible = false;

				tbTXEQ2.Visible = false;
				tbTXEQ3.Visible = false;
				tbTXEQ4.Visible = false;
				tbTXEQ6.Visible = false;
				tbTXEQ7.Visible = false;
				tbTXEQ8.Visible = false;
				tbTXEQ10.Visible = false;

				lblTXEQ1.Text = "Low";
				lblTXEQ5.Text = "Mid";
				lblTXEQ9.Text = "High";

				toolTip1.SetToolTip(lblTXEQ1, "0-400Hz");
				toolTip1.SetToolTip(tbTXEQ1, "0-400Hz");
				toolTip1.SetToolTip(lblTXEQ5, "400-1500Hz");
				toolTip1.SetToolTip(tbTXEQ5, "400-1500Hz");
				toolTip1.SetToolTip(lblTXEQ9, "1500-6000Hz");
				toolTip1.SetToolTip(tbTXEQ9, "1500-6000Hz");

				RXEQ = console.radio.GetDSPRX(0, 0).RXEQ3;
				TXEQ = console.radio.GetDSPTX(0).TXEQ3;

				tbRXEQ_Scroll(this, EventArgs.Empty);
				tbTXEQ_Scroll(this, EventArgs.Empty);

				picRXEQ.Invalidate();
				picTXEQ.Invalidate();
		
				console.radio.GetDSPRX(0, 0).RXEQNumBands = 3;
				console.radio.GetDSPTX(0).TXEQNumBands = 3;
			}
		}

		private void rad10Band_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rad10Band.Checked)
			{
				lblRXEQ2.Visible = true;
				lblRXEQ3.Visible = true;
				lblRXEQ4.Visible = true;
				lblRXEQ6.Visible = true;
				lblRXEQ7.Visible = true;
				lblRXEQ8.Visible = true;
				lblRXEQ10.Visible = true;

				tbRXEQ2.Visible = true;
				tbRXEQ3.Visible = true;
				tbRXEQ4.Visible = true;
				tbRXEQ6.Visible = true;
				tbRXEQ7.Visible = true;
				tbRXEQ8.Visible = true;
				tbRXEQ10.Visible = true;

				lblRXEQ1.Text = "32";
				lblRXEQ5.Text = "500";
				lblRXEQ9.Text = "8K";

				toolTip1.SetToolTip(lblRXEQ1, "");
				toolTip1.SetToolTip(tbRXEQ1, "");
				toolTip1.SetToolTip(lblRXEQ5, "");
				toolTip1.SetToolTip(tbRXEQ5, "");
				toolTip1.SetToolTip(lblRXEQ9, "");
				toolTip1.SetToolTip(tbRXEQ9, "");

				lblTXEQ2.Visible = true;
				lblTXEQ3.Visible = true;
				lblTXEQ4.Visible = true;
				lblTXEQ6.Visible = true;
				lblTXEQ7.Visible = true;
				lblTXEQ8.Visible = true;
				lblTXEQ10.Visible = true;

				tbTXEQ2.Visible = true;
				tbTXEQ3.Visible = true;
				tbTXEQ4.Visible = true;
				tbTXEQ6.Visible = true;
				tbTXEQ7.Visible = true;
				tbTXEQ8.Visible = true;
				tbTXEQ10.Visible = true;

				lblTXEQ1.Text = "32";
				lblTXEQ5.Text = "500";
				lblTXEQ9.Text = "8K";

				toolTip1.SetToolTip(lblTXEQ1, "");
				toolTip1.SetToolTip(tbTXEQ1, "");
				toolTip1.SetToolTip(lblTXEQ5, "");
				toolTip1.SetToolTip(tbTXEQ5, "");
				toolTip1.SetToolTip(lblTXEQ9, "");
				toolTip1.SetToolTip(tbTXEQ9, "");

				RXEQ = console.radio.GetDSPRX(0, 0).RXEQ10;
				TXEQ = console.radio.GetDSPTX(0).TXEQ10;

				tbRXEQ_Scroll(this, EventArgs.Empty);
				tbTXEQ_Scroll(this, EventArgs.Empty);

				picRXEQ.Invalidate();
				picTXEQ.Invalidate();	
			
				console.radio.GetDSPRX(0, 0).RXEQNumBands = 10;
				console.radio.GetDSPTX(0).TXEQNumBands = 10;
			}
		}

		#endregion		
	}
}