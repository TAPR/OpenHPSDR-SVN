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

		private System.Windows.Forms.NumericUpDown udHarm;
		private System.Windows.Forms.Label lblHarm;
		private System.Windows.Forms.Label lblAmplitude;
		private System.Windows.Forms.TrackBar tbPhase;
		private System.Windows.Forms.Label lblPhase;
		private System.Windows.Forms.NumericUpDown udAmp;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructor and Destructor

		public Predistest()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Predistest));
			this.udHarm = new System.Windows.Forms.NumericUpDown();
			this.lblHarm = new System.Windows.Forms.Label();
			this.lblAmplitude = new System.Windows.Forms.Label();
			this.udAmp = new System.Windows.Forms.NumericUpDown();
			this.tbPhase = new System.Windows.Forms.TrackBar();
			this.lblPhase = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.udHarm)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udAmp)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbPhase)).BeginInit();
			this.SuspendLayout();
			// 
			// udHarm
			// 
			this.udHarm.Location = new System.Drawing.Point(80, 8);
			this.udHarm.Maximum = new System.Decimal(new int[] {
																   20,
																   0,
																   0,
																   0});
			this.udHarm.Minimum = new System.Decimal(new int[] {
																   20,
																   0,
																   0,
																   -2147483648});
			this.udHarm.Name = "udHarm";
			this.udHarm.Size = new System.Drawing.Size(48, 20);
			this.udHarm.TabIndex = 3;
			this.udHarm.Value = new System.Decimal(new int[] {
																 3,
																 0,
																 0,
																 -2147483648});
			this.udHarm.ValueChanged += new System.EventHandler(this.udHarm_ValueChanged);
			// 
			// lblHarm
			// 
			this.lblHarm.Location = new System.Drawing.Point(16, 8);
			this.lblHarm.Name = "lblHarm";
			this.lblHarm.Size = new System.Drawing.Size(56, 20);
			this.lblHarm.TabIndex = 4;
			this.lblHarm.Text = "Harmonic:";
			this.lblHarm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblAmplitude
			// 
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
			this.udAmp.Increment = new System.Decimal(new int[] {
																	1,
																	0,
																	0,
																	393216});
			this.udAmp.Location = new System.Drawing.Point(80, 32);
			this.udAmp.Maximum = new System.Decimal(new int[] {
																  1,
																  0,
																  0,
																  131072});
			this.udAmp.Minimum = new System.Decimal(new int[] {
																  1,
																  0,
																  0,
																  458752});
			this.udAmp.Name = "udAmp";
			this.udAmp.Size = new System.Drawing.Size(80, 20);
			this.udAmp.TabIndex = 5;
			this.udAmp.Value = new System.Decimal(new int[] {
																1,
																0,
																0,
																393216});
			this.udAmp.ValueChanged += new System.EventHandler(this.udAmp_ValueChanged);
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
			// lblPhase
			// 
			this.lblPhase.Location = new System.Drawing.Point(16, 56);
			this.lblPhase.Name = "lblPhase";
			this.lblPhase.Size = new System.Drawing.Size(64, 20);
			this.lblPhase.TabIndex = 8;
			this.lblPhase.Text = "Phase:";
			this.lblPhase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// Predistest
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 110);
			this.Controls.Add(this.lblPhase);
			this.Controls.Add(this.tbPhase);
			this.Controls.Add(this.lblAmplitude);
			this.Controls.Add(this.udAmp);
			this.Controls.Add(this.lblHarm);
			this.Controls.Add(this.udHarm);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Predistest";
			this.Text = "Pre-Distortion Test";
			((System.ComponentModel.ISupportInitialize)(this.udHarm)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udAmp)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbPhase)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Event Handlers

		private void udHarm_ValueChanged(object sender, System.EventArgs e)
		{
			//Audio.Harm = (int)udHarm.Value;
			DttSP.SetKeyerHarmonicRemove((double)udHarm.Value, (tbPhase.Value / (double)tbPhase.Maximum)*Math.PI*2, (double)udAmp.Value);
		}

		private void udAmp_ValueChanged(object sender, System.EventArgs e)
		{
			//Audio.HarmAmp = (float)udAmp.Value;
			DttSP.SetKeyerHarmonicRemove((double)udHarm.Value, (tbPhase.Value / (double)tbPhase.Maximum)*Math.PI*2, (double)udAmp.Value);
		}

		private void tbPhase_Scroll(object sender, System.EventArgs e)
		{
			//Audio.HarmPhaseOffset = (tbPhase.Value / (double)tbPhase.Maximum)*Math.PI*2;
			DttSP.SetKeyerHarmonicRemove((double)udHarm.Value, (tbPhase.Value / (double)tbPhase.Maximum)*Math.PI*2, (double)udAmp.Value);
		}

		#endregion
	}
}
