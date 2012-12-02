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
	/// Averaging Noise Blanker
	/// </summary>
	public class AveragingNoiseBlanker : IDSPObject
	{
		#region Private Members

        private DSPBuffer d = null;
        private DSPState s = null;
		                
        private float average_signal_real = 0.0f;
        private float average_signal_imag = 0.0f;        
        private float average_magnitude = 0.0f;
        
        #endregion
        		        
        #region Constructor
        
		public AveragingNoiseBlanker(ref DSPBuffer dsp_buffer_obj)
		{
            this.d = dsp_buffer_obj;
            this.s = d.State;

			this.average_magnitude = 1f;                  
		}
						
		#endregion
		
		#region Public Methods
        
        public void Process()
        {   
            for (int i = 0; i < s.DSPBlockSize; i++)
            {
            	float magnitude = d.GetMagnitude(i);
                this.average_signal_real = (this.average_signal_real * 0.75f) + (d.cpx[i].real * 0.25f);
                this.average_signal_imag = (this.average_signal_imag * 0.75f) + (d.cpx[i].imag * 0.25f);
                this.average_magnitude = 0.999f * (this.average_magnitude) + 0.001f * magnitude;
                if (magnitude > (this.AveNBThreshold * this.average_magnitude))
                {
                    d.cpx[i].real = this.average_signal_real;
                    d.cpx[i].imag = this.average_signal_imag;
                }
            }
        }

        unsafe public void Process(float * real, float * imag, int size)
        {
            for (int i = 0; i < size; i++)
            {
                float magnitude = (float)Math.Sqrt(real[i] * real[i] + imag[i] * imag[i]);
                this.average_signal_real = (this.average_signal_real * 0.75f) + (real[i] * 0.25f);
                this.average_signal_imag = (this.average_signal_imag * 0.75f) + (imag[i] * 0.25f);                
                this.average_magnitude = 0.999f * (this.average_magnitude) + 0.001f * magnitude;
                if (magnitude > (this.AveNBThreshold * this.average_magnitude))
                {
                    real[i] = this.average_signal_real;
                    imag[i] = this.average_signal_imag;
                }
            }
        }

        #endregion

        #region Properties

        private float anb_threshold = 15.0f;
        public float AveNBThreshold
        {
            get { return anb_threshold; }
            set { anb_threshold = value; }
        }

        private bool anb_switch = false;
        public bool AveNBSwitchOn
        {
            get { return anb_switch; }
            set { anb_switch = value; }
        }

        #endregion
    }
}
