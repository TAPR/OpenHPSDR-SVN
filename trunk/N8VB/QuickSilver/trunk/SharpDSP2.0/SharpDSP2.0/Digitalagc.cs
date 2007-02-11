//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU Public License
//==============================================================
//digitalagc.cs
//
//digital agc, both linear and exponential decay
//
//exponential decay code from Phil Harman's (VK6APH) code
//==============================================================

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace SharpDSP
{
	[Serializable()]
    public struct DigitalAGC
    {
        #region Private members

        private int agcHang;
        private int holdLoop;
        private int agcLoop;
        private int agcAttackTime;
        private int recover_count;
        private int size;

        private double minGain;
        private double gain;
        private double prevGain;
        private double[] G;
        private double[] agcbuf_real;
        private double[] agcbuf_imag;

        #endregion

        #region Properties

        private AGCType agc_type;
        public AGCType AgcType
        {
            get { return this.agc_type; }
            set
            {
                this.agc_type = value;
            }
        }

        private int hang;
        public int Hang
        {
            get { return this.hang; }
            set
            {
                this.hang = value;
            }
        }
        
        private double agcAttackTimeScale;
        public double AgcAttackTimeScale
        {
            get { return agcAttackTimeScale; }
            set
            {
                agcAttackTimeScale = value;
            }
        }

        private double maxGain;
        public double AgcMaxGain
        {
            get { return this.maxGain; }
            set
            {
                this.maxGain = (double)Math.Pow(10.0, value / 20.0); 				
            }
        }

		public double AgcMinGain
		{
			get { return this.minGain; }
			set
			{
				this.minGain = (double)Math.Pow(10.0, value / 20.0); 
			}
		}
        
        private double agcFixedGain;
        public double AgcFixedGain
        {
            get { return this.agcFixedGain; }
            set
            {
                this.agcFixedGain = (double)Math.Pow(10.0, value / 20.0);
            }
        }
        private double agcLimit;
        public double AgcLimit
        {
            get { return agcLimit; }
            set
            {
                agcLimit = value * 0.001F;
            }
        }
        
        private bool recover;
        public bool Recover
        {
            get { return this.recover; }
            set
            {
                this.recover = value;
            }
        }

        #endregion

        #region Constructor

        public DigitalAGC(AGCType AgcMode, int Hang, int AgcHang, int AttackTime, double MaxGain,
          double MinGain, double Limit, double initialGain, double fixedGain, int length)
        {
            this.agc_type = AgcMode;
            this.hang = Hang;
            this.agcHang = AgcHang;
            this.agcLoop = 0;
            this.holdLoop = 0;
            this.agcAttackTime = AttackTime;
            this.agcAttackTimeScale = 1.0F / this.agcAttackTime;
            this.maxGain = (double)Math.Pow(10.0, MaxGain / 20.0);
            this.minGain = (double)Math.Pow(10.0, MinGain / 20.0);
            this.agcFixedGain = (double)Math.Pow(10.0, fixedGain / 20.0);
            this.gain = (double)Math.Pow(10.0, initialGain / 20.0);
            this.prevGain = (double)Math.Pow(10.0, initialGain / 20.0);
            this.agcLimit = Limit * 0.001F;
            this.recover = true;
            this.size = length;
            this.agcbuf_real = new double[size];
            this.agcbuf_imag = new double[size];
            this.G = new double[24];
            this.recover_count = 0;
        }

        #endregion

        #region Public Methods

        public void DoAGC(ref double[] real, ref double[] imag)
        {
            if (this.agc_type == AGCType.agcOff)
            {
                for (int i = 0; i < this.size; i++)
                {
                    real[i] *= this.agcFixedGain;
                    imag[i] *= this.agcFixedGain;
                }
                return;
            }

            if (this.recover)
            {
                recover_count = 3;
                this.recover = false;
                for (int i = 0; i < this.agcHang; i++)
                    this.G[i] = this.maxGain;
            }

            if (recover_count != 0)
            {
                recover_count--;
                this.agcHang = 1;  // force fast setting
            }
            else
            {
                this.agcHang = this.hang; // use users setting 
            }

            if (this.agcLoop < (this.agcHang - 1))
            {
                this.agcLoop += 1;
            }
            else
            {
                this.agcLoop = 0;
            }

            double Vpeak = 0F;
            for (int i = 0; i < this.size; i++)
            {
                double Magnitude = (double)Math.Sqrt(real[i] * real[i] + imag[i] * imag[i]);
                if (Vpeak < Magnitude)
                    Vpeak = Magnitude;
            }

            if (Vpeak != 0)
            {
                this.G[this.agcLoop] = this.agcLimit / Vpeak;
                this.gain = this.G[0];
                for (int i = 1; i < this.hang; i++)
                {
                    if (this.G[i] < this.gain)
                    {
                        this.gain = this.G[i];
                    }
                }
                if (this.gain > this.maxGain)
                    this.gain = this.maxGain;
                if (this.gain < this.minGain)
                    this.gain = this.minGain;

                if (this.prevGain != this.gain)
                {
                    double GainStep = (this.gain - this.prevGain) * this.agcAttackTimeScale;
                    for (int i = 0; i < this.agcAttackTime; i++)
                    {
                        real[i] *= this.prevGain + (i + 1) * GainStep;
                        imag[i] *= this.prevGain + (i + 1) * GainStep;
                    }
                    for (int i = this.agcAttackTime; i < this.size; i++)
                    {
                        real[i] *= this.gain;
                        imag[i] *= this.gain;
                    }
                    this.prevGain = this.gain;
                }
                else
                {
                    for (int i = 0; i < this.size; i++)
                    {
                        real[i] *= this.gain;
                        imag[i] *= this.gain;
                    }
                }
            }
        }

        public void DoExponentialAGC(ref double[] real, ref double[] imag)
        {
            if (this.agc_type == AGCType.agcOff)
            {
                for (int i = 0; i < this.size; i++)
                {
                    real[i] *= this.agcFixedGain;
                    imag[i] *= this.agcFixedGain;
                }
                return;
            }

            // select exponential decay time based on console setting

            int Decay = 0;

            switch (this.agc_type)
            {
                case AGCType.agcSlow:
                    Decay = 40;		// slow decay
                    break;
                case AGCType.agcMedium:
                    Decay = 15;		// medium decay
                    break;
                case AGCType.agcFast:
                    Decay = 2;		// fast decay
                    break;
            }

            if (this.recover)
            {
                recover_count = 3;
                this.recover = false;
                for (int i = 0; i < this.agcHang; i++)
                    this.G[i] = this.maxGain;
            }

            if (recover_count != 0)
            {
                recover_count--;
                this.agcHang = 1;  // force fast setting
                Decay = 2;
            }
            else
            {
                this.agcHang = this.hang; // use users setting 
            }

            if (this.agcLoop < (this.agcHang - 1))
            {
                this.agcLoop += 1;
            }
            else
            {
                this.agcLoop = 0;
            }

            double Vpeak = 0F;
            for (int i = 0; i < this.size; i++)
            {
                double Magnitude = (double)Math.Sqrt(real[i] * real[i] + imag[i] * imag[i]);
                if (Vpeak < Magnitude)
                    Vpeak = Magnitude;
            }

            if (Vpeak != 0)
            {
                this.G[this.agcLoop] = this.agcLimit / Vpeak;
                this.gain = this.G[0];
                for (int i = 1; i < this.hang; i++)
                {
                    if (this.G[i] < this.gain)
                    {
                        this.gain = this.G[i];
                    }
                }
                if (this.gain > this.maxGain)
                    this.gain = this.maxGain;
                if (this.gain < this.minGain)
                    this.gain = this.minGain;

                if (this.gain < this.prevGain) // AGC Gain is decreasing since signal is increasing
                {
                    double GainStep = (this.prevGain - this.gain) * this.agcAttackTimeScale;

                    for (int i = 0; i < this.agcAttackTime; i++)
                    {
                        real[i] *= this.prevGain + (i + 1) * GainStep;
                        imag[i] *= this.prevGain + (i + 1) * GainStep;
                    }
                    for (int i = this.agcAttackTime; i < this.size; i++)
                    {
                        real[i] *= this.gain;
                        imag[i] *= this.gain;
                    }
                    this.prevGain = this.gain;
                }
                else if (this.gain > this.prevGain) // AGC Gain is increasing since signal is decreasing
                {
                    if (this.agc_type == AGCType.agcLong) // use Hang AGC 
                    {
                        double GainStep = (this.gain - this.prevGain) * this.agcAttackTimeScale;

                        for (int i = 0; i < this.agcAttackTime; i++)
                        {
                            real[i] *= this.prevGain + (i + 1) * GainStep;
                            imag[i] *= this.prevGain + (i + 1) * GainStep;
                        }
                        for (int i = this.agcAttackTime; i < this.size; i++)
                        {
                            real[i] *= this.gain;
                            imag[i] *= this.gain;
                        }
                    }
                    else // use exponential decay AGC 
                    {
                        this.gain = this.prevGain + ((this.gain - this.prevGain) / Decay); //VK6APH 16 March 2005

                        for (int i = 0; i < this.size; i++)
                        {
                            real[i] *= this.gain;
                            imag[i] *= this.gain;
                        }
                    }
                    this.prevGain = this.gain;
                }
                else
                {
                    for (int i = 0; i < this.size; i++)
                    {
                        real[i] *= this.gain;
                        imag[i] *= this.gain;
                    }
                }
            }
        }

        #endregion
    }
}
