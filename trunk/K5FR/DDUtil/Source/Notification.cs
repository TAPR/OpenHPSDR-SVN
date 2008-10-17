using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Media;
using TransDlg;

namespace DataDecoder
{
	public class Notification : TransDialog
	{
        #region Ctor, init code and dispose
		public Notification()
            : base(true)
		{
			InitializeComponent();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
        #endregion // Ctor and init code

        #region Event handler
        public static string notiMsg;
        public static bool useAudio;
        SoundPlayer myPlayer = new System.Media.SoundPlayer();

        private void Notification_Load(object sender, System.EventArgs e)
        {
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            this.Left =( screenWidth - this.Width)-25;
            this.Top = (screenHeight - this.Height)-25;
            label1.Text = notiMsg;
            timer1.Enabled = true;
            if (useAudio)
            {
                myPlayer.SoundLocation = "Alert.wav";
                myPlayer.Play();
            }
        }
        // linger timer fired
        private void timer1_Tick(object sender, System.EventArgs e)
        {
            this.Close();
        }

        #endregion // Event handler
        
        #region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(328, 80);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hello!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.MouseLeave += new System.EventHandler(this.label1_MouseLeave);
            this.label1.MouseHover += new System.EventHandler(this.label1_MouseHover);
            // 
            // timer1
            // 
            this.timer1.Interval = 7000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Notification
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(324, 76);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Notification";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Notification_Load);
            this.ResumeLayout(false);

        }
		#endregion

        public Label label1;

        #region Designer generated variables

        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.IContainer components;
        #endregion

        private void label1_MouseHover(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

	}
}
