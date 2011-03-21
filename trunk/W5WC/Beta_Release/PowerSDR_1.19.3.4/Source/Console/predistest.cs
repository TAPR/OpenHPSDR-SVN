using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PowerSDR
{
	/// <summary>
	/// Summary description for predistest.
	/// </summary>
	public class Predistest : System.Windows.Forms.Form
	{
		#region Variable Declaration

		private NumericUpDownTS udHarm;
		private LabelTS lblHarm;
		private LabelTS lblAmplitude;
		private TrackBarTS tbPhase;
		private LabelTS lblPhase;
		private NumericUpDownTS udAmp;
        private NumericUpDownTS udPhase;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructor and Destructor

		public Predistest()
		{
			InitializeComponent();
            Common.RestoreForm(this, "PredisForm", false);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Predistest));
            this.udPhase = new System.Windows.Forms.NumericUpDownTS();
            this.lblPhase = new System.Windows.Forms.LabelTS();
            this.tbPhase = new System.Windows.Forms.TrackBarTS();
            this.lblAmplitude = new System.Windows.Forms.LabelTS();
            this.udAmp = new System.Windows.Forms.NumericUpDownTS();
            this.lblHarm = new System.Windows.Forms.LabelTS();
            this.udHarm = new System.Windows.Forms.NumericUpDownTS();
            ((System.ComponentModel.ISupportInitialize)(this.udPhase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPhase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udAmp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udHarm)).BeginInit();
            this.SuspendLayout();
            // 
            // udPhase
            // 
            this.udPhase.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPhase.Location = new System.Drawing.Point(12, 79);
            this.udPhase.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udPhase.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udPhase.Name = "udPhase";
            this.udPhase.Size = new System.Drawing.Size(51, 20);
            this.udPhase.TabIndex = 9;
            this.udPhase.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udPhase.ValueChanged += new System.EventHandler(this.udPhase_ValueChanged);
            // 
            // lblPhase
            // 
            this.lblPhase.Image = null;
            this.lblPhase.Location = new System.Drawing.Point(16, 56);
            this.lblPhase.Name = "lblPhase";
            this.lblPhase.Size = new System.Drawing.Size(64, 20);
            this.lblPhase.TabIndex = 8;
            this.lblPhase.Text = "Phase:";
            this.lblPhase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbPhase
            // 
            this.tbPhase.Location = new System.Drawing.Point(80, 56);
            this.tbPhase.Maximum = 1000;
            this.tbPhase.Name = "tbPhase";
            this.tbPhase.Size = new System.Drawing.Size(200, 45);
            this.tbPhase.TabIndex = 7;
            this.tbPhase.TickFrequency = 125;
            this.tbPhase.Scroll += new System.EventHandler(this.tbPhase_Scroll);
            // 
            // lblAmplitude
            // 
            this.lblAmplitude.Image = null;
            this.lblAmplitude.Location = new System.Drawing.Point(16, 32);
            this.lblAmplitude.Name = "lblAmplitude";
            this.lblAmplitude.Size = new System.Drawing.Size(64, 20);
            this.lblAmplitude.TabIndex = 6;
            this.lblAmplitude.Text = "Amplitude:";
            this.lblAmplitude.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // udAmp
            // 
            this.udAmp.DecimalPlaces = 8;
            this.udAmp.Increment = new decimal(new int[] {
            1,
            0,
            0,
            393216});
            this.udAmp.Location = new System.Drawing.Point(80, 32);
            this.udAmp.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.udAmp.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            458752});
            this.udAmp.Name = "udAmp";
            this.udAmp.Size = new System.Drawing.Size(80, 20);
            this.udAmp.TabIndex = 5;
            this.udAmp.Value = new decimal(new int[] {
            1,
            0,
            0,
            393216});
            this.udAmp.ValueChanged += new System.EventHandler(this.udAmp_ValueChanged);
            // 
            // lblHarm
            // 
            this.lblHarm.Image = null;
            this.lblHarm.Location = new System.Drawing.Point(16, 8);
            this.lblHarm.Name = "lblHarm";
            this.lblHarm.Size = new System.Drawing.Size(56, 20);
            this.lblHarm.TabIndex = 4;
            this.lblHarm.Text = "Harmonic:";
            this.lblHarm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // udHarm
            // 
            this.udHarm.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udHarm.Location = new System.Drawing.Point(80, 8);
            this.udHarm.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.udHarm.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            this.udHarm.Name = "udHarm";
            this.udHarm.Size = new System.Drawing.Size(48, 20);
            this.udHarm.TabIndex = 3;
            this.udHarm.Value = new decimal(new int[] {
            3,
            0,
            0,
            -2147483648});
            this.udHarm.ValueChanged += new System.EventHandler(this.udHarm_ValueChanged);
            // 
            // Predistest
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 110);
            this.Controls.Add(this.udPhase);
            this.Controls.Add(this.lblPhase);
            this.Controls.Add(this.tbPhase);
            this.Controls.Add(this.lblAmplitude);
            this.Controls.Add(this.udAmp);
            this.Controls.Add(this.lblHarm);
            this.Controls.Add(this.udHarm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Predistest";
            this.Text = "Pre-Distortion Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Predistest_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.udPhase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPhase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udAmp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udHarm)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        #region Properties

        public double Phase
        {
            get { return (double)udPhase.Value / 1000.0; }
            set { udPhase.Value = (int)(value*1000); }
        }

        public double Ampl
        {
            get { return (double)udAmp.Value; }
            set { udAmp.Value = (decimal)value; }
        }

        #endregion

        #region Event Handlers

        private void udHarm_ValueChanged(object sender, System.EventArgs e)
		{
			//Audio.Harm = (int)udHarm.Value;
			//DttSP.SetKeyerHarmonicRemove((double)udHarm.Value, (tbPhase.Value / (double)tbPhase.Maximum)*Math.PI*2, (double)udAmp.Value);
		}

		private void udAmp_ValueChanged(object sender, System.EventArgs e)
		{
			//Audio.HarmAmp = (float)udAmp.Value;
			//DttSP.SetKeyerHarmonicRemove((double)udHarm.Value, (tbPhase.Value / (double)tbPhase.Maximum)*Math.PI*2, (double)udAmp.Value);
		}

		private void tbPhase_Scroll(object sender, System.EventArgs e)
		{
			//Audio.HarmPhaseOffset = (tbPhase.Value / (double)tbPhase.Maximum)*Math.PI*2;
			//DttSP.SetKeyerHarmonicRemove((double)udHarm.Value, (tbPhase.Value / (double)tbPhase.Maximum)*Math.PI*2, (double)udAmp.Value);
            udPhase.Value = tbPhase.Value;
		}

        private void udPhase_ValueChanged(object sender, EventArgs e)
        {
            tbPhase.Value = (int)udPhase.Value;
            //DttSP.SetKeyerHarmonicRemove((double)udHarm.Value, (tbPhase.Value / (double)tbPhase.Maximum) * Math.PI * 2, (double)udAmp.Value);
        }

        private void Predistest_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "PredisForm");
        }

		#endregion        
	}
}
