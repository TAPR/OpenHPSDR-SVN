//=================================================================
// DiversityForm.cs
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
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PowerSDR
{
	/// <summary>
	/// Summary description for DiversityForm.
	/// </summary>
	public class DiversityForm : System.Windows.Forms.Form
	{
		private Point p = new Point(200, 200);
		private Color topColor = Color.FromArgb(0, 120, 0);
		private Color bottomColor = Color.FromArgb(0, 40, 0);
		private Color lineColor = Color.FromArgb(0, 255, 0);
		private double locked_r = 0.0f;
		private double locked_angle = 0.0f;
		private Console console;

		private System.Windows.Forms.PictureBox picRadar;
		private System.Windows.Forms.CheckBox chkAuto;
		private System.Windows.Forms.CheckBox chkLockAngle;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.NumericUpDownTS udR;
		private System.Windows.Forms.NumericUpDownTS udAngle;
		private System.Windows.Forms.CheckBox chkLockR;
		private System.Windows.Forms.Button btnSync;
		private System.Windows.Forms.CheckBox chkEnable;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DiversityForm(Console c)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			console = c;

			Common.RestoreForm(this, "DiversityForm", false);
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DiversityForm));
			this.picRadar = new System.Windows.Forms.PictureBox();
			this.chkAuto = new System.Windows.Forms.CheckBox();
			this.chkLockAngle = new System.Windows.Forms.CheckBox();
			this.chkLockR = new System.Windows.Forms.CheckBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.udR = new System.Windows.Forms.NumericUpDownTS();
			this.udAngle = new System.Windows.Forms.NumericUpDownTS();
			this.btnSync = new System.Windows.Forms.Button();
			this.chkEnable = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.udR)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udAngle)).BeginInit();
			this.SuspendLayout();
			// 
			// picRadar
			// 
			this.picRadar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.picRadar.BackColor = System.Drawing.SystemColors.Control;
			this.picRadar.Location = new System.Drawing.Point(0, 0);
			this.picRadar.Name = "picRadar";
			this.picRadar.Size = new System.Drawing.Size(400, 400);
			this.picRadar.TabIndex = 0;
			this.picRadar.TabStop = false;
			this.picRadar.Paint += new System.Windows.Forms.PaintEventHandler(this.picRadar_Paint);
			this.picRadar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picRadar_MouseUp);
			this.picRadar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picRadar_MouseMove);
			this.picRadar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picRadar_MouseDown);
			// 
			// chkAuto
			// 
			this.chkAuto.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.chkAuto.Enabled = false;
			this.chkAuto.Location = new System.Drawing.Point(16, 424);
			this.chkAuto.Name = "chkAuto";
			this.chkAuto.Size = new System.Drawing.Size(48, 24);
			this.chkAuto.TabIndex = 1;
			this.chkAuto.Text = "Auto";
			// 
			// chkLockAngle
			// 
			this.chkLockAngle.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.chkLockAngle.Location = new System.Drawing.Point(88, 424);
			this.chkLockAngle.Name = "chkLockAngle";
			this.chkLockAngle.Size = new System.Drawing.Size(80, 24);
			this.chkLockAngle.TabIndex = 2;
			this.chkLockAngle.Text = "Lock Angle";
			// 
			// chkLockR
			// 
			this.chkLockR.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.chkLockR.Location = new System.Drawing.Point(192, 424);
			this.chkLockR.Name = "chkLockR";
			this.chkLockR.Size = new System.Drawing.Size(80, 24);
			this.chkLockR.TabIndex = 3;
			this.chkLockR.Text = "Lock Gain";
			// 
			// textBox1
			// 
			this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.textBox1.Enabled = false;
			this.textBox1.Location = new System.Drawing.Point(56, 456);
			this.textBox1.Name = "textBox1";
			this.textBox1.TabIndex = 4;
			this.textBox1.Text = "";
			// 
			// udR
			// 
			this.udR.DecimalPlaces = 3;
			this.udR.Increment = new System.Decimal(new int[] {
																  1,
																  0,
																  0,
																  196608});
			this.udR.Location = new System.Drawing.Point(248, 456);
			this.udR.Maximum = new System.Decimal(new int[] {
																2,
																0,
																0,
																0});
			this.udR.Minimum = new System.Decimal(new int[] {
																2,
																0,
																0,
																-2147483648});
			this.udR.Name = "udR";
			this.udR.Size = new System.Drawing.Size(56, 20);
			this.udR.TabIndex = 5;
			this.udR.ValueChanged += new System.EventHandler(this.udR_ValueChanged);
			// 
			// udAngle
			// 
			this.udAngle.DecimalPlaces = 3;
			this.udAngle.Increment = new System.Decimal(new int[] {
																	  1,
																	  0,
																	  0,
																	  196608});
			this.udAngle.Location = new System.Drawing.Point(320, 456);
			this.udAngle.Maximum = new System.Decimal(new int[] {
																	4,
																	0,
																	0,
																	0});
			this.udAngle.Minimum = new System.Decimal(new int[] {
																	4,
																	0,
																	0,
																	-2147483648});
			this.udAngle.Name = "udAngle";
			this.udAngle.Size = new System.Drawing.Size(56, 20);
			this.udAngle.TabIndex = 6;
			this.udAngle.ValueChanged += new System.EventHandler(this.udTheta_ValueChanged);
			// 
			// btnSync
			// 
			this.btnSync.Location = new System.Drawing.Point(336, 424);
			this.btnSync.Name = "btnSync";
			this.btnSync.Size = new System.Drawing.Size(48, 23);
			this.btnSync.TabIndex = 47;
			this.btnSync.Text = "Sync";
			this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
			// 
			// chkEnable
			// 
			this.chkEnable.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkEnable.Location = new System.Drawing.Point(272, 424);
			this.chkEnable.Name = "chkEnable";
			this.chkEnable.Size = new System.Drawing.Size(48, 24);
			this.chkEnable.TabIndex = 48;
			this.chkEnable.Text = "Enable";
			this.chkEnable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkEnable.CheckedChanged += new System.EventHandler(this.chkEnable_CheckedChanged);
			// 
			// DiversityForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(400, 486);
			this.Controls.Add(this.chkEnable);
			this.Controls.Add(this.btnSync);
			this.Controls.Add(this.udAngle);
			this.Controls.Add(this.udR);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.chkLockR);
			this.Controls.Add(this.chkLockAngle);
			this.Controls.Add(this.chkAuto);
			this.Controls.Add(this.picRadar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DiversityForm";
			this.Text = "Diversity Control";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.DiversityForm_Closing);
			((System.ComponentModel.ISupportInitialize)(this.udR)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udAngle)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void picRadar_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			int size = Math.Min(picRadar.Width, picRadar.Height);

			Pen pen = new Pen(lineColor);
			// set a couple of graphics properties to make the
			// output image look nice
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.InterpolationMode = InterpolationMode.Bicubic;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = TextRenderingHint.AntiAlias;
			// draw the background of the radar
			g.FillEllipse(new LinearGradientBrush(new Point((int)(size / 2), 0), new Point((int)(size / 2), size - 1), topColor, bottomColor), 0, 0, size - 1, size - 1);
			// draw the outer ring (0° elevation)
			g.DrawEllipse(pen, 0, 0, size - 1, size - 1);
			// draw the inner ring (60° elevation)
			int interval = size / 3;
			g.DrawEllipse(pen, (size - interval) / 2, (size - interval) / 2, interval, interval);
			// draw the middle ring (30° elevation)
			interval *= 2;
			g.DrawEllipse(pen, (size - interval) / 2, (size - interval) / 2, interval, interval);
			// draw the x and y axis lines
			g.DrawLine(pen, new Point(0, (int)(size / 2)), new Point(size - 1, (int)(size / 2)));
			g.DrawLine(pen, new Point((int)(size / 2), 0), new Point((int)(size / 2), size - 1));

			g.FillEllipse(Brushes.Yellow, p.X-4, p.Y-4, 8, 8); 
		}

		private Point PolarToXY(double r, double angle)
		{
			int L = (int)Math.Min(picRadar.Width, picRadar.Height);
			return new Point((int)(r*Math.Cos(angle)*L/2)+L/2, (int)(r*Math.Sin(angle)*L/2)+L/2);
		}

		private void picRadar_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            // if(!mouse_down) return;

			int W = picRadar.Width;
			int H = picRadar.Height;
			int L = (int)Math.Min(W, H);

			// get coords relative to middle
			int x = e.X - L/2;
			int y = e.Y - L/2;

			// change coordinate space form pixels to percentage of half width
			double xf = (double)x / (L/2);
			double yf = -(double)y / (L/2);

			double r = Math.Min(Math.Sqrt(Math.Pow(xf, 2.0) + Math.Pow(yf, 2.0)), 2.0);
			double angle = Math.Atan2(yf, xf);
            Debug.WriteLine("xf: "+xf.ToString("f2")+"  yf: "+yf.ToString("f2")+"  r: " + r.ToString("f4") + "  angle: " + angle.ToString("f4"));

			if(mouse_down)
			{
				if(chkLockR.Checked && chkLockAngle.Checked) return;
				if(chkLockR.Checked)
				{
					p = PolarToXY(locked_r, angle);
					locked_angle = angle;
                    udR.Value = (decimal)locked_r;
                    udAngle.Value = (decimal)angle;
				}
				else if(chkLockAngle.Checked)
				{
                    if (angle > 0 && locked_angle < 0)
                        locked_angle += Math.PI;
                    else if (angle < 0 && locked_angle > 0)
                        locked_angle -= Math.PI;
                    
					p = PolarToXY(r, locked_angle);
					locked_r = r;
                    udR.Value = (decimal)r;
                    udAngle.Value = (decimal)locked_angle;
				}
				else
				{
					p = new Point(e.X, e.Y);
					locked_r = r;
                    Debug.WriteLine("locked_r: " + r.ToString("f4"));
					locked_angle = angle;
                    udR.Value = (decimal)r;
                    udAngle.Value = (decimal)angle;
				}
				
				picRadar.Invalidate();
			}
		}

		private bool mouse_down = false;

		private void picRadar_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			mouse_down = true;
			picRadar_MouseMove(sender, e);
		}

		private void picRadar_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			mouse_down = false;
		}

		private void udR_ValueChanged(object sender, System.EventArgs e)
		{
			UpdateDiversity();
		}

		private void udTheta_ValueChanged(object sender, System.EventArgs e)
		{
			UpdateDiversity();
		}

		private void UpdateDiversity()
		{
			double r = (double)udR.Value;
			double angle = (double)udAngle.Value;
			DttSP.SetDiversityScalar((float)((r*1.5)*Math.Cos(angle)), (float)((r*1.5)*Math.Sin(angle)));

			int L = (int)Math.Min(picRadar.Width, picRadar.Height);
			p = new Point((int)(r*L/2*Math.Cos(angle))+L/2, -(int)(r*L/2*Math.Sin(angle))+L/2);
			picRadar.Invalidate();
		}

		private void btnSync_Click(object sender, System.EventArgs e)
		{
            console.RX2SpurReduction = console.SpurReduction;
            console.RX2DSPMode = console.RX1DSPMode;
            console.RX2Filter = console.RX1Filter;
            console.RX2PreampMode = console.RX1PreampMode;
            console.VFOSync = true;
			console.radio.GetDSPRX(1, 0).Copy(console.radio.GetDSPRX(0, 0));
		}

		private void chkEnable_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkEnable.Checked) chkEnable.BackColor = console.ButtonSelectedColor;
			else chkEnable.BackColor = SystemColors.Control;
			if(chkEnable.Checked)
			{
				if(!console.RX2Enabled)
					console.RX2Enabled = true;
			}
			DttSP.SetDiversity(Convert.ToInt16(chkEnable.Checked));
		}

		private void DiversityForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Common.SaveForm(this, "DiversityForm");
		}
	}
}
