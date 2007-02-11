//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General public License
//==============================================================
//squelch.cs
//
//==============================================================

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace SharpDSP
{
	[Serializable()]
    public struct Squelch
    {
        #region Private members

        private bool running;
        private int num;
        private int size;
        private double samplerate;
        private double power;

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
        
        private double offset_meter;
        public double MeterOffset
        {
            get { return offset_meter; }
            set
            {
                offset_meter = value;
            }
        }

        private double offset_attenuation;
        public double AttenuationOffset
        {
            get { return offset_attenuation; }
            set
            {
                offset_attenuation = value;
            }
        }

        private double offset_gain;
        public double GainOffset
        {
            get { return offset_gain; }
            set
            {
                offset_gain = value;
            }
        }

        private bool flag;
        public bool SquelchOn
        {
            get { return flag; }
            set
            {
                flag = value;
            }
        }

        #endregion

        #region Constructor

        public Squelch(double threshold, 
                       double samplerate, 
                       int size)
        {
            this.size = size;
            this.threshold = threshold;
            this.power = 0F;
            this.offset_meter = 0F;
            this.offset_attenuation = 0F;
            this.offset_gain = 0F;
            this.flag = false;
            this.running = false;
            this.samplerate = samplerate;
            this.num = (int)(this.size * this.size / this.samplerate);
        }

        #endregion

        #region Public Methods

        public void DoSquelch(ref double[] real, 
                              ref double[] imag)
        {
            if (this.flag)
            {
                double temp = 0F;
                this.power = 0F;
                for (int i = 0; i < this.size; i++)
                {
                    this.power += (real[i] * real[i] + imag[i] * imag[i]);
                }
                temp = (double)(10.0 * Math.Log10(this.power) + this.offset_meter + this.offset_attenuation + this.offset_gain);
                if (this.threshold > temp)
                {
                    if (this.running == false)
                    {
                        for (int i = 0; i < this.num; i++)
                        {
                            real[i] *= 1.0F - (double)(i / this.num);
                            imag[i] *= 1.0F - (double)(i / this.num);
                        }
                        for (int i = this.num; i < this.size; i++)
                        {
                            real[i] = 0F;
                            imag[i] = 0F;
                        }
                        this.running = true;
                    }
                    else
                    {
                        for (int i = 0; i < this.size; i++)
                        {
                            real[i] = 0F;
                            imag[i] = 0F;
                        }
                    }
                }

            }
            else
            {
                if (this.running)
                {
                    for (int i = 0; i < this.num; i++)
                    {
                        real[i] *= 1.0F - (double)(i / this.num);
                        imag[i] *= 1.0F - (double)(i / this.num);
                    }
                    this.running = false;
                }
            }

        }

        #endregion

    }
}
