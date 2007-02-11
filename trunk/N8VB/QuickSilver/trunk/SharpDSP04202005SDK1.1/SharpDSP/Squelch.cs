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
        private float samplerate;
        private float power;

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
        
        private float offset_meter;
        public float MeterOffset
        {
            get { return offset_meter; }
            set
            {
                offset_meter = value;
            }
        }

        private float offset_attenuation;
        public float AttenuationOffset
        {
            get { return offset_attenuation; }
            set
            {
                offset_attenuation = value;
            }
        }

        private float offset_gain;
        public float GainOffset
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

        public Squelch(float threshold, float samplerate, int size)
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

        unsafe public void DoSquelch(float* real, float* imag)
        {
            if (this.flag)
            {
                float temp = 0F;
                this.power = 0F;
                for (int i = 0; i < this.size; i++)
                {
                    this.power += (real[i] * real[i] + imag[i] * imag[i]);
                }
                temp = (float)(10.0 * Math.Log10(this.power) + this.offset_meter + this.offset_attenuation + this.offset_gain);
                if (this.threshold > temp)
                {
                    if (this.running == false)
                    {
                        for (int i = 0; i < this.num; i++)
                        {
                            real[i] *= 1.0F - (float)(i / this.num);
                            imag[i] *= 1.0F - (float)(i / this.num);
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
                        real[i] *= 1.0F - (float)(i / this.num);
                        imag[i] *= 1.0F - (float)(i / this.num);
                    }
                    this.running = false;
                }
            }

        }

        #endregion

    }
}