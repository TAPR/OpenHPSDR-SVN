//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General public License
//==============================================================
//FMDemodulator.cs
//FM PLL for FM mode
//
//Adapted from Bob, N4HY's PLL algorithms
//==============================================================

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace SharpDSP
{
	[Serializable()]
    public struct FMPLL
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
        private float afc;
        private float cvt;

        #endregion

        #region Constructor

        public FMPLL(float samplerate, float frequency, float lowlimit, float hilimit, float bandwidth, int size)
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
            this.afc = 0F;
            this.cvt = 0.45F * samplerate / (float)(Math.PI * bandwidth);
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

                float difference = (float)(Math.Atan2(this.delay_imag, this.delay_real));

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

                this.afc = 0.9999F * this.afc + 0.0001F * this.frequency;
                demod[i] = (this.frequency - this.afc) * this.cvt;
            }
        }

        #endregion

    }

}