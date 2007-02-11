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
    public struct LocalOscillator
    {
        #region private members

        private double frequencyTone;
        private double phaseTone;
        private double magnitudeTone;
        private double stepTone;
        private double CosN;
        private double SinN;
        private double samplerate;
        private readonly double TWO_PI;

        #endregion
        
        #region Constructor

        public LocalOscillator(double frequency, 
                               double phase, 
                               double magnitude, 
                               double sampleRate)
        {
            this.frequencyTone = frequency / sampleRate;
            this.magnitudeTone = (double)magnitude;
            this.stepTone = this.frequencyTone * Math.PI * 2.0;
            this.phaseTone = phase;
            this.CosN = 0;
            this.SinN = 0;
            this.samplerate = (double)sampleRate;	
			this.TWO_PI = (double)Math.PI * 2;
        }

        #endregion

        #region Public members

        public void DoMixer(ref double[] realin,
                            ref double[] imagin,
                            ref double[] realout,
                            ref double[] imagout,
                            int length)
        {
            for (int i = 0; i < length; i++)
            {
				if (this.phaseTone >= this.TWO_PI)
					this.phaseTone -= this.TWO_PI;
				if (this.phaseTone < 0)
					this.phaseTone += this.TWO_PI;

                this.CosN = Math.Cos(this.phaseTone);
                this.SinN = Math.Sin(this.phaseTone);
                
                realout[i] = (double)((this.CosN * realin[i]) - (this.SinN * imagin[i])) * this.magnitudeTone;
                imagout[i] = (double)((this.CosN * imagin[i]) + (this.SinN * realin[i])) * this.magnitudeTone; 
               	
				this.phaseTone += this.stepTone;
            }
        }
		
		public void ChangeFrequency(double newfreq)
		{
			this.frequencyTone = newfreq / this.samplerate;
			this.stepTone = this.frequencyTone * Math.PI * 2.0;
		}	
        
        #endregion

    }

}
