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
        private double frequency;
        private double lowlimit;
        private double hilimit;
        private double phase;
        private double delay_real;
        private double delay_imag;
        private double alpha;
        private double beta;
        private double TWOPI;
        private double lockcurrent;
        private double lockprevious;
        private double dc;

        #endregion

        #region Constructor

        public AMPLL(double samplerate, double frequency, double lowlimit, double hilimit, double bandwidth, int size)
        {
            this.TWOPI = (double)Math.PI * 2.0F;

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

        public void DoPLL(ref double[] real, 
                          ref double[] imag, 
                          ref double[] demod)
        {
            for (int i = 0; i < this.size; i++)
            {
                double tempCos = (double)Math.Cos(this.phase);
                double tempSin = (double)Math.Sin(this.phase);

                this.delay_real = tempCos * real[i] + tempSin * imag[i];
                this.delay_imag = -tempSin * real[i] + tempCos * imag[i];

                double difference = (double)(Math.Sqrt((real[i] * real[i]) + (imag[i] * imag[i])) 
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

                this.lockcurrent = 0.999F * this.lockcurrent + 0.001F * (double)Math.Abs(this.delay_imag);
                this.lockprevious = this.lockcurrent;
                this.dc = 0.99F * this.dc + 0.01F * this.delay_real;
                demod[i] = this.delay_real - this.dc;
            }
        }

        #endregion

    }

} 
