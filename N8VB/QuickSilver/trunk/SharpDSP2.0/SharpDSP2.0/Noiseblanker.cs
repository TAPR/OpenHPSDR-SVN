//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General public License
//==============================================================
//noiseblanker.cs
//
//==============================================================

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace SharpDSP
{
	[Serializable()]
    public struct NoiseBlanker
    {
        #region Private members

        private int size;
        private double average_magnitude;
        private double sdrom_average_magnitude;
        private double average_signal_real;
        private double average_signal_imag;

        #endregion

        #region Properties

        private double threshold;
        public double Threshold
        {
            get { return threshold; }
            set
            {
                threshold = value;
            }
        }

        private double sdromthreshold;
        public double SDROMThreshold
        {
            get { return sdromthreshold; }
            set
            {
                sdromthreshold = value;
            }
        }

        #endregion

        #region Constructor

        public NoiseBlanker(double threshold, double sdromThreshold, int size)
        {
            this.threshold = threshold;
            this.sdromthreshold = sdromThreshold;
            this.size = size;
            this.average_magnitude = 1F;
            this.sdrom_average_magnitude = 1F;
            this.average_signal_real = 0F;
            this.average_signal_imag = 0F;
        }

        #endregion

        #region Public Methods

        public void DoNoiseBlanker(ref double[] real, ref double[] imag)
        {
            for (int i = 0; i < this.size; i++)
            {
                double magnitude = (double)Math.Sqrt(real[i] * real[i] + imag[i] * imag[i]);
                this.average_magnitude = 0.999F * (this.average_magnitude) + 0.001F * magnitude;
                if (magnitude > (this.threshold * this.average_magnitude))
                {
                    real[i] = 0F;
                    imag[i] = 0F;
                }
            }
        }

        public void DoSDROMBlanker(ref double[] real, ref double[] imag)
        {
            for (int i = 0; i < this.size; i++)
            {
                double magnitude = (double)Math.Sqrt(real[i] * real[i] + imag[i] * imag[i]);
                this.average_signal_real *= 0.75F;
                this.average_signal_imag *= 0.75F;
                this.average_signal_real += real[i] * 0.25F;
                this.average_signal_imag += imag[i] * 0.25F;
                this.sdrom_average_magnitude = 0.999F * (this.sdrom_average_magnitude) + 0.001F * magnitude;
                if (magnitude > (this.sdromthreshold * this.sdrom_average_magnitude))
                {
                    real[i] = this.average_signal_real;
                    imag[i] = this.average_signal_imag;
                }
            }
        }

        #endregion

    }
} 
