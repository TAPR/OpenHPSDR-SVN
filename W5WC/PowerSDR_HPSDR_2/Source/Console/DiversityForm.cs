//=================================================================
// DiversityForm.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2011  FlexRadio Systems
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
// You may contact us via email at: gpl@flexradio.com.
// Paper mail may be sent to: 
//    FlexRadio Systems
//    4616 W. Howard Lane  Suite 1-150
//    Austin, TX 78728
//    USA
//=================================================================

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Threading;

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
        //private Image crosshair = new Bitmap("C:\\crosshair.png");
		private Console console;

		private System.Windows.Forms.PictureBox picRadar;
		private System.Windows.Forms.CheckBox chkAuto;
        private System.Windows.Forms.CheckBox chkLockAngle;
		private System.Windows.Forms.NumericUpDownTS udR;
		private System.Windows.Forms.NumericUpDownTS udAngle;
		private System.Windows.Forms.CheckBox chkLockR;
		private System.Windows.Forms.Button btnSync;
		private System.Windows.Forms.CheckBox chkEnable;
        private ButtonTS btnBump45;
        private ButtonTS btnBump180;
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
            console.RadarColorUpdate = true;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiversityForm));
            this.picRadar = new System.Windows.Forms.PictureBox();
            this.chkAuto = new System.Windows.Forms.CheckBox();
            this.chkLockAngle = new System.Windows.Forms.CheckBox();
            this.chkLockR = new System.Windows.Forms.CheckBox();
            this.btnSync = new System.Windows.Forms.Button();
            this.chkEnable = new System.Windows.Forms.CheckBox();
            this.btnBump180 = new System.Windows.Forms.ButtonTS();
            this.btnBump45 = new System.Windows.Forms.ButtonTS();
            this.udAngle = new System.Windows.Forms.NumericUpDownTS();
            this.udR = new System.Windows.Forms.NumericUpDownTS();
            ((System.ComponentModel.ISupportInitialize)(this.picRadar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udR)).BeginInit();
            this.SuspendLayout();
            // 
            // picRadar
            // 
            this.picRadar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.picRadar.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.picRadar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picRadar.Location = new System.Drawing.Point(0, 0);
            this.picRadar.Name = "picRadar";
            this.picRadar.Size = new System.Drawing.Size(400, 400);
            this.picRadar.TabIndex = 0;
            this.picRadar.TabStop = false;
            this.picRadar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picRadar_MouseMove);
            this.picRadar.Resize += new System.EventHandler(this.picRadar_Resize);
            this.picRadar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picRadar_MouseDown);
            this.picRadar.Paint += new System.Windows.Forms.PaintEventHandler(this.picRadar_Paint);
            this.picRadar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picRadar_MouseUp);
            // 
            // chkAuto
            // 
            this.chkAuto.Enabled = false;
            this.chkAuto.Location = new System.Drawing.Point(12, 453);
            this.chkAuto.Name = "chkAuto";
            this.chkAuto.Size = new System.Drawing.Size(48, 24);
            this.chkAuto.TabIndex = 1;
            this.chkAuto.Text = "Auto";
            this.chkAuto.Visible = false;
            // 
            // chkLockAngle
            // 
            this.chkLockAngle.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.chkLockAngle.ForeColor = System.Drawing.Color.White;
            this.chkLockAngle.Location = new System.Drawing.Point(5, 406);
            this.chkLockAngle.Name = "chkLockAngle";
            this.chkLockAngle.Size = new System.Drawing.Size(80, 24);
            this.chkLockAngle.TabIndex = 2;
            this.chkLockAngle.Text = "Lock Angle";
            this.chkLockAngle.CheckedChanged += new System.EventHandler(this.chkLockAngle_CheckedChanged);
            // 
            // chkLockR
            // 
            this.chkLockR.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.chkLockR.ForeColor = System.Drawing.Color.White;
            this.chkLockR.Location = new System.Drawing.Point(5, 424);
            this.chkLockR.Name = "chkLockR";
            this.chkLockR.Size = new System.Drawing.Size(104, 24);
            this.chkLockR.TabIndex = 3;
            this.chkLockR.Text = "Lock Magnitude";
            this.chkLockR.CheckedChanged += new System.EventHandler(this.chkLockR_CheckedChanged);
            // 
            // btnSync
            // 
            this.btnSync.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSync.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSync.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSync.FlatAppearance.BorderSize = 0;
            this.btnSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSync.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSync.ForeColor = System.Drawing.Color.White;
            this.btnSync.Location = new System.Drawing.Point(112, 413);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(56, 26);
            this.btnSync.TabIndex = 47;
            this.btnSync.Text = "Sync";
            this.btnSync.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // chkEnable
            // 
            this.chkEnable.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.chkEnable.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkEnable.BackColor = System.Drawing.Color.Transparent;
            this.chkEnable.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chkEnable.FlatAppearance.BorderSize = 0;
            this.chkEnable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkEnable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnable.ForeColor = System.Drawing.Color.White;
            this.chkEnable.Location = new System.Drawing.Point(167, 413);
            this.chkEnable.Name = "chkEnable";
            this.chkEnable.Size = new System.Drawing.Size(56, 26);
            this.chkEnable.TabIndex = 48;
            this.chkEnable.Text = "Enable";
            this.chkEnable.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkEnable.UseVisualStyleBackColor = false;
            this.chkEnable.CheckedChanged += new System.EventHandler(this.chkEnable_CheckedChanged);
            // 
            // btnBump180
            // 
            this.btnBump180.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnBump180.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBump180.FlatAppearance.BorderSize = 0;
            this.btnBump180.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBump180.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBump180.ForeColor = System.Drawing.Color.White;
            this.btnBump180.Image = null;
            this.btnBump180.Location = new System.Drawing.Point(276, 415);
            this.btnBump180.Name = "btnBump180";
            this.btnBump180.Size = new System.Drawing.Size(39, 23);
            this.btnBump180.TabIndex = 50;
            this.btnBump180.Text = "180";
            this.btnBump180.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBump180.UseVisualStyleBackColor = true;
            this.btnBump180.Click += new System.EventHandler(this.btnBump180_Click);
            // 
            // btnBump45
            // 
            this.btnBump45.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnBump45.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBump45.FlatAppearance.BorderSize = 0;
            this.btnBump45.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBump45.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBump45.ForeColor = System.Drawing.Color.White;
            this.btnBump45.Image = null;
            this.btnBump45.Location = new System.Drawing.Point(236, 415);
            this.btnBump45.Name = "btnBump45";
            this.btnBump45.Size = new System.Drawing.Size(39, 23);
            this.btnBump45.TabIndex = 49;
            this.btnBump45.Text = "45";
            this.btnBump45.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBump45.UseVisualStyleBackColor = true;
            this.btnBump45.Click += new System.EventHandler(this.btnBump45_Click);
            // 
            // udAngle
            // 
            this.udAngle.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.udAngle.DecimalPlaces = 3;
            this.udAngle.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.udAngle.Location = new System.Drawing.Point(320, 456);
            this.udAngle.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.udAngle.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            -2147483648});
            this.udAngle.Name = "udAngle";
            this.udAngle.Size = new System.Drawing.Size(56, 20);
            this.udAngle.TabIndex = 6;
            this.udAngle.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udAngle.ValueChanged += new System.EventHandler(this.udTheta_ValueChanged);
            // 
            // udR
            // 
            this.udR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.udR.DecimalPlaces = 3;
            this.udR.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.udR.Location = new System.Drawing.Point(248, 456);
            this.udR.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.udR.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
            this.udR.Name = "udR";
            this.udR.Size = new System.Drawing.Size(56, 20);
            this.udR.TabIndex = 5;
            this.udR.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udR.ValueChanged += new System.EventHandler(this.udR_ValueChanged);
            // 
            // DiversityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5, 13);
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(400, 451);
            this.Controls.Add(this.btnBump180);
            this.Controls.Add(this.btnBump45);
            this.Controls.Add(this.chkEnable);
            this.Controls.Add(this.btnSync);
            this.Controls.Add(this.udAngle);
            this.Controls.Add(this.udR);
            this.Controls.Add(this.chkLockR);
            this.Controls.Add(this.chkLockAngle);
            this.Controls.Add(this.chkAuto);
            this.Controls.Add(this.picRadar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DiversityForm";
            this.Text = "Enhanced Signal Clarity ™";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.DiversityForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.picRadar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udR)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        public bool CATEnable
        {
            get { return chkEnable.Checked; }
            set
            {
                chkEnable.Checked = value;
            }
        }


        private Color imageColorTop, imageColorBottom, consoleColorBottom;
		private void picRadar_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			int size = Math.Min(picRadar.Width, picRadar.Height);
            if (console.RadarColorUpdate)
            {
                string panadapterBackgroundPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins\\" + console.CurrentSkin + "\\Console\\picDisplay.png";

                string consoleBackgroundPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins\\" + console.CurrentSkin + "\\Console\\Console.png";

                string buttonOffPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins\\" + console.CurrentSkin + "\\Console\\chkMON-0.png";

                string buttonOnPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    "\\FlexRadio Systems\\PowerSDR\\Skins\\" + console.CurrentSkin + "\\Console\\chkMON-1.png";

                Bitmap buttonOffImage = new Bitmap(buttonOffPath);
                Bitmap buttonOnImage = new Bitmap(buttonOnPath);
                chkEnable.BackgroundImage = buttonOffImage;
                btnSync.BackgroundImage = buttonOffImage;
                btnBump45.BackgroundImage = buttonOffImage;
                btnBump180.BackgroundImage = buttonOffImage;

                btnSync.FlatAppearance.BorderColor = imageColorBottom;
                btnBump45.FlatAppearance.BorderColor = imageColorBottom;
                btnBump180.FlatAppearance.BorderColor = imageColorBottom;

                Bitmap panadapterBackground = new Bitmap(panadapterBackgroundPath);
                imageColorTop = panadapterBackground.GetPixel((int)(panadapterBackground.Width -5), (int)(panadapterBackground.Height -5));
                imageColorBottom = panadapterBackground.GetPixel((int)(panadapterBackground.Width/9), (int)(panadapterBackground.Height/9));
                

                Bitmap consoleBackground = new Bitmap(consoleBackgroundPath);
                consoleColorBottom = consoleBackground.GetPixel((int)(consoleBackground.Width - 5), (int)(consoleBackground.Height - 5));
                picRadar.BackgroundImage = consoleBackground;
                //this.BackgroundImage = consoleBackground;
                this.BackColor = consoleColorBottom;
                
                console.RadarColorUpdate = false;
            }
            
            Pen pen = new Pen(lineColor);
			// set a couple of graphics properties to make the
			// output image look nice
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.InterpolationMode = InterpolationMode.Bicubic;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = TextRenderingHint.AntiAlias;
			// draw the background of the radar
            //g.FillEllipse(new LinearGradientBrush(new Point((int)(size / 2), 0), new Point((int)(size / 2), size - 1), topColor, bottomColor), 0, 0, size - 1, size - 1);
            g.FillEllipse(new LinearGradientBrush(new Point((int)(size / 2), 0), new Point((int)(size / 2), size - 1), imageColorTop, imageColorBottom), 0, 0, size - 1, size - 1);

            //TextureBrush tBrush = new TextureBrush(largeImage, new Rectangle(0, 20, largeImage.Width, largeImage.Height - 40));
            //tBrush.WrapMode = System.Drawing.Drawing2D.WrapMode.TileFlipXY;
            //g.FillEllipse(tBrush, 0, 0, size - 1, size - 1);

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

            Pen crosshairPen = new Pen(Color.Red, 2);
			//g.FillEllipse(Brushes.Yellow, p.X-4, p.Y-4, 8, 8);
            g.DrawLine(crosshairPen, p.X, p.Y - 16, p.X, p.Y - 2);
            g.DrawLine(crosshairPen, p.X, p.Y + 16, p.X, p.Y + 2);
            g.DrawLine(crosshairPen, p.X-2, p.Y, p.X-13, p.Y);
            g.DrawLine(crosshairPen, p.X+2, p.Y, p.X+13, p.Y); 
            g.DrawEllipse(crosshairPen, p.X - 8, p.Y - 8, 16, 16);

            //p.X = p.X - 13;
            //p.Y = p.Y - 13;
            //g.DrawImage(crosshair, new Rectangle((p.X-(int)(crosshair.Width/2)), p.Y - (int)(crosshair.Height/2), 20, 20));
		}

		private Point PolarToXY(double r, double angle)
		{
			int L = (int)Math.Min(picRadar.Width, picRadar.Height);
			return new Point((int)(r*Math.Cos(angle)*L/2)+L/2, (int)(r*Math.Sin(angle)*L/2)+L/2);
		}

		private void picRadar_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            if(!mouse_down) return;

			int W = picRadar.Width;
			int H = picRadar.Height;
			int L = (int)Math.Min(W, H);

			// get coords relative to middle
			int x = e.X - L/2;
			int y = e.Y - L/2;

			// change coordinate space form pixels to percentage of half width
			double xf = (double)x / (L/2);
			double yf = -(double)y / (L/2);

			double _r = Math.Min(Math.Sqrt(Math.Pow(xf, 2.0) + Math.Pow(yf, 2.0)), 2.0);
			double _angle = Math.Atan2(yf, xf);
            //Debug.WriteLine("xf: "+xf.ToString("f2")+"  yf: "+yf.ToString("f2")+"  r: " + r.ToString("f4") + "  angle: " + angle.ToString("f4"));

			if(mouse_down)
			{
				if(chkLockR.Checked && chkLockAngle.Checked) return;
				if(chkLockR.Checked)
				{
					p = PolarToXY(locked_r, _angle);
                    angle = _angle;
				}
				else if(chkLockAngle.Checked)
				{
                    if (_angle > 0 && locked_angle < 0)
                        locked_angle += Math.PI;
                    else if (_angle < 0 && locked_angle > 0)
                        locked_angle -= Math.PI;
                    
					p = PolarToXY(_r, locked_angle);
					locked_r = _r;
                    r = _r;
                    angle = locked_angle;
				}
				else
				{
					p = new Point(e.X, e.Y);
					locked_r = _r;
                    //Debug.WriteLine("locked_r: " + r.ToString("f4"));
					locked_angle = _angle;
                    r = _r;
                    angle = _angle;
				}

                udR.Value = (decimal)r;
                udAngle.Value = (decimal)angle;
				
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

        private double r = 0.0;
        private double angle = 0.0;

		private void UpdateDiversity()
		{
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

            string buttonOnPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins\\" + console.CurrentSkin + "\\Console\\chkMON-1.png";

            string buttonOffPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins\\" + console.CurrentSkin + "\\Console\\chkMON-0.png";

            Bitmap buttonOffImage = new Bitmap(buttonOffPath);
            Bitmap buttonOnImage = new Bitmap(buttonOnPath);
            this.btnSync.BackgroundImage = buttonOnImage;
            Thread.Sleep(200);
            this.btnSync.BackgroundImage = buttonOffImage;
            
            

		}

		private void chkEnable_CheckedChanged(object sender, System.EventArgs e)
		{
            //if(chkEnable.Checked) chkEnable.BackColor = console.ButtonSelectedColor;
            //else chkEnable.BackColor = SystemColors.Control;

            string buttonOffPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins\\" + console.CurrentSkin + "\\Console\\chkMON-0.png";

            string buttonOnPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins\\" + console.CurrentSkin + "\\Console\\chkMON-1.png";

            Bitmap buttonOffImage = new Bitmap(buttonOffPath);
            Bitmap buttonOnImage = new Bitmap(buttonOnPath);

            if (chkEnable.Checked) chkEnable.BackgroundImage = buttonOnImage;
            else chkEnable.BackgroundImage = buttonOffImage;

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

        private void picRadar_Resize(object sender, EventArgs e)
        {
            UpdateDiversity();
        }

        private void btnBump45_Click(object sender, EventArgs e)
        {
            double _angle = angle;
            _angle += Math.PI / 4;
            if (_angle > 2 * Math.PI)
                _angle -= 2 * Math.PI;
            angle = _angle;
            UpdateDiversity();
        }

        private void btnBump180_Click(object sender, EventArgs e)
        {
            double _angle = angle;
            _angle += Math.PI;
            if (_angle > 2 * Math.PI)
                _angle -= 2 * Math.PI;
            angle = _angle;
            UpdateDiversity();
        }

        private void chkLockAngle_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLockAngle.Checked)
                locked_angle = angle;
        }

        private void chkLockR_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLockR.Checked)
                locked_r = r;
        }
	}
}
