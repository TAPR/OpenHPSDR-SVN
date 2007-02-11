//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General public License
//==============================================================
//synchronousAM.cs
//AM PLL for SAM mode
//
//Adapted from Bob, N4HY's PLL algorithms
//==============================================================

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace SharpDSP
{
	[Serializable()]
    public struct AMPLL
    {
        #region Private members

        private int size;
        private float frequency;
        private float lowlimit;
        private float hilimit;
        private float phase;
        private float delay_real;
        private float delay_imag;
        private float alpha;
        private float beta;
        private float TWOPI;
        private float lockcurrent;
        private float lockprevious;
        private float dc;

        #endregion

        #region Constructor

        public AMPLL(float samplerate, float frequency, float lowlimit, float hilimit, float bandwidth, int size)
        {
            this.TWOPI = (float)Math.PI * 2.0F;

            this.size = size;
            this.frequency = frequency * this.TWOPI / samplerate;
            this.lowlimit = lowlimit * this.TWOPI / samplerate;
            this.hilimit = hilimit * this.TWOPI / samplerate;
            this.phase = 0.0F;
            this.delay_real = 0F;
            this.delay_imag = 1F;
            this.alpha = 0.3F * bandwidth * this.TWOPI / samplerate;
            this.beta = this.alpha * this.alpha * 0.25F;
            this.lockcurrent = 0.5F;
            this.lockprevious = 1F;
            this.dc = 0F;
        }

        #endregion

        #region Public Methods

        unsafe public void DoPLL(float* real, float* imag, float* demod)
        {
            for (int i = 0; i < this.size; i++)
            {
                float tempCos = (float)Math.Cos(this.phase);
                float tempSin = (float)Math.Sin(this.phase);

                this.delay_real = tempCos * real[i] + tempSin * imag[i];
                this.delay_imag = -tempSin * real[i] + tempCos * imag[i];

                float difference = (float)(Math.Sqrt((real[i] * real[i]) + (imag[i] * imag[i])) 
                    * Math.Atan2(this.delay_imag, this.delay_real));

                this.frequency += this.beta * difference;

                if (this.frequency < this.lowlimit)
                    this.frequency = this.lowlimit;
                if (this.frequency > this.hilimit)
                    this.frequency = this.hilimit;

                this.phase += this.frequency + this.alpha * difference;

                while (this.phase >= this.TWOPI) 
                    this.phase -= this.TWOPI;
                while (this.phase < 0) 
                    this.phase += this.TWOPI;

                this.lockcurrent = 0.999F * this.lockcurrent + 0.001F * (float)Math.Abs(this.delay_imag);
                this.lockprevious = this.lockcurrent;
                this.dc = 0.99F * this.dc + 0.01F * this.delay_real;
                demod[i] = this.delay_real - this.dc;
            }
        }

        #endregion

    }

} 