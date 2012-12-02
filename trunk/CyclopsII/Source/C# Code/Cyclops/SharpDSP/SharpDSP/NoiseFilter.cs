/*
This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2007, 2008 Philip A Covington

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

The author can be reached by email at

p.covington@gmail.com

*/

using System;

namespace SharpDSP2._1
{
	/// <summary>
	/// LMS Noise Filter
	/// </summary>
	public class NoiseFilter : IDSPObject
	{
		#region Private members

        private DSPBuffer d = null;
        private DSPState s = null;

        private int lms_size;
        private int delay_line_index;
        private int mask;
        private float[] delay_line;
        private float[] coefficients;            
                
        #endregion
        
        #region Constructor
        
		public NoiseFilter(ref DSPBuffer dsp_buffer_obj)
		{
            this.d = dsp_buffer_obj;
            this.s = d.State;

			this.lms_size = 128;    // 45
            this.mask = this.lms_size - 1;
            this.delay_line = new float[128 /*this.lms_size */];
            this.coefficients = new float[128];
            this.delay_line_index = 0;
		}
						
		#endregion
		
		#region Public Methods

        public void Process()
        {
            int blocksize = s.DSPBlockSize;
            
        	float scl1 = 1.0F - this.nfadaptation_rate * this.nfleakage;

            for (int i = 0; i < blocksize; i++)
            {
                this.delay_line[this.delay_line_index] = d.cpx[i].real;

                float accum = 0.0F;
                float sum_sq = 0.0F;
                int k = 0;

                for (int j = 0; j < this.nfadaptive_filter_size; j++)
                {
                    k = (j + this.nfdelay + this.delay_line_index) & this.mask;
                    sum_sq += this.delay_line[k] * this.delay_line[k];
                    accum += this.coefficients[j] * this.delay_line[k];
                }

                float error = d.cpx[i].real - accum;
                d.cpx[i].real = accum;  //this makes it a noise filter
                d.cpx[i].imag = accum;
                
                float scl2 = this.nfadaptation_rate / (sum_sq + 1e-10F);
                error *= scl2;

                for (int j = 0; j < this.nfadaptive_filter_size; j++)
                {
                    k = (j + this.nfdelay + this.delay_line_index) & this.mask;
                    this.coefficients[j] = this.coefficients[j] * scl1 + error * this.delay_line[k];
                }

                this.delay_line_index = (this.delay_line_index + this.mask) & this.mask;				
            }			
        }

        unsafe public void Process(float * real, float * imag, int blocksize)
        {            
            float scl1 = 1.0F - this.nfadaptation_rate * this.nfleakage;

            for (int i = 0; i < blocksize; i++)
            {
                this.delay_line[this.delay_line_index] = real[i];

                float accum = 0.0F;
                float sum_sq = 0.0F;
                int k = 0;

                for (int j = 0; j < this.nfadaptive_filter_size; j++)
                {
                    k = (j + this.nfdelay + this.delay_line_index) & this.mask;
                    sum_sq += this.delay_line[k] * this.delay_line[k];
                    accum += this.coefficients[j] * this.delay_line[k];
                }

                float error = real[i] - accum;
                real[i] = accum;  //this makes it a noise filter
                imag[i] = accum;

                float scl2 = this.nfadaptation_rate / (sum_sq + 1e-10F);
                error *= scl2;

                for (int j = 0; j < this.nfadaptive_filter_size; j++)
                {
                    k = (j + this.nfdelay + this.delay_line_index) & this.mask;
                    this.coefficients[j] = this.coefficients[j] * scl1 + error * this.delay_line[k];
                }

                this.delay_line_index = (this.delay_line_index + this.mask) & this.mask;
            }
        }

        #endregion

        #region Properties

        private int nfdelay = 64;
        public int NoiseFilterDelay
        {
            get { return nfdelay; }
            set { nfdelay = value; }
        }

        private float nfadaptation_rate = 0.01f;
        public float NoiseFilterAdaptationRate
        {
            get { return nfadaptation_rate; }
            set { nfadaptation_rate = value; }
        }

        private float nfleakage = 0.00001f;
        public float NoiseFilterLeakage
        {
            get { return nfleakage; }
            set { nfleakage = value; }
        }

        private int nfadaptive_filter_size = 128;
        public int NoiseFilterAdaptiveFilterSize
        {
            get { return nfadaptive_filter_size; }
            set { nfadaptive_filter_size = value; }
        }

        private bool nfswitch = false;
        public bool NoiseFilterSwitchOn
        {
            get { return nfswitch; }
            set { nfswitch = value; }
        }

        #endregion
    }
}
