//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General public License
//==============================================================
//ToneOscillator.cs
//implements a local oscillator 
//
//==============================================================

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace SharpDSP
{
	[Serializable()]
    unsafe public struct LocalOscillator
    {
        #region private members

        private double frequencyTone;
        private double phaseTone;
        private float magnitudeTone;
        private double stepTone;
        private double CosN;
        private double SinN;
        private float samplerate;
		private readonly float TWO_PI;

        #endregion
        
        #region Constructor

        public LocalOscillator(double frequency, double phase, double magnitude, double sampleRate)
        {
            this.frequencyTone = frequency / sampleRate;
            this.magnitudeTone = (float)magnitude;
            this.stepTone = this.frequencyTone * Math.PI * 2.0;
            this.phaseTone = phase;
            this.CosN = 0;
            this.SinN = 0;
            this.samplerate = (float)sampleRate;	
			this.TWO_PI = (float)Math.PI * 2;
        }

        #endregion

        #region Public members

        public void DoMixer(float* realin, float* imagin, float* realout, float* imagout, int length)
        {
            for (int i = 0; i < length; i++)
            {
				if (this.phaseTone >= this.TWO_PI)
					this.phaseTone -= this.TWO_PI;
				if (this.phaseTone < 0)
					this.phaseTone += this.TWO_PI;

                this.CosN = Math.Cos(this.phaseTone);
                this.SinN = Math.Sin(this.phaseTone);
                
                realout[i] = (float)((this.CosN * realin[i]) - (this.SinN * imagin[i])) * this.magnitudeTone;
                imagout[i] = (float)((this.CosN * imagin[i]) + (this.SinN * realin[i])) * this.magnitudeTone; 
               	
				this.phaseTone += this.stepTone;
            }
        }
		
		public void ChangeFrequency(float newfreq)
		{
			this.frequencyTone = newfreq / this.samplerate;
			this.stepTone = this.frequencyTone * Math.PI * 2.0;
		}	
        
        #endregion

    }

}
