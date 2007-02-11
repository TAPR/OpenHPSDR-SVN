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
        private float average_magnitude;
        private float sdrom_average_magnitude;
        private float average_signal_real;
        private float average_signal_imag;

        #endregion

        #region Properties

        private float threshold;
        public float Threshold
        {
            get { return threshold; }
            set
            {
                threshold = value;
            }
        }

        private float sdromthreshold;
        public float SDROMThreshold
        {
            get { return sdromthreshold; }
            set
            {
                sdromthreshold = value;
            }
        }

        #endregion

        #region Constructor

        public NoiseBlanker(float threshold, float sdromThreshold, int size)
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

        unsafe public void DoNoiseBlanker(float* real, float* imag)
        {
            for (int i = 0; i < this.size; i++)
            {
                float magnitude = (float)Math.Sqrt(real[i] * real[i] + imag[i] * imag[i]);
                this.average_magnitude = 0.999F * (this.average_magnitude) + 0.001F * magnitude;
                if (magnitude > (this.threshold * this.average_magnitude))
                {
                    real[i] = 0F;
                    imag[i] = 0F;
                }
            }
        }

        unsafe public void DoSDROMBlanker(float* real, float* imag)
        {
            for (int i = 0; i < this.size; i++)
            {
                float magnitude = (float)Math.Sqrt(real[i] * real[i] + imag[i] * imag[i]);
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