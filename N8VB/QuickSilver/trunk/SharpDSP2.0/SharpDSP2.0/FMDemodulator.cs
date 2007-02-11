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
        private double frequency;
        private double lowlimit;
        private double hilimit;
        private double phase;
        private double delay_real;
        private double delay_imag;
        private double alpha;
        private double beta;
        private double TWOPI;
        private double afc;
        private double cvt;

        #endregion

        #region Constructor

        public FMPLL(double samplerate, double frequency, double lowlimit, double hilimit, double bandwidth, int size)
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
            this.afc = 0F;
            this.cvt = 0.45F * samplerate / (double)(Math.PI * bandwidth);
        }

        #endregion

        #region Public Methods

        public void DoPLL(ref double[] real, ref double[] imag, ref double[] demod)
        {
            for (int i = 0; i < this.size; i++)
            {
                double tempCos = (double)Math.Cos(this.phase);
                double tempSin = (double)Math.Sin(this.phase);

                this.delay_real = tempCos * real[i] + tempSin * imag[i];
                this.delay_imag = -tempSin * real[i] + tempCos * imag[i];

                double difference = (double)(Math.Atan2(this.delay_imag, this.delay_real));

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
