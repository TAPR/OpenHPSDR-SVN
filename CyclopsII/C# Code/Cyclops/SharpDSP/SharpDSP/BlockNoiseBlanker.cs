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
	/// Block Noise Blanker
	/// </summary>
	public class BlockNoiseBlanker : IDSPObject
	{
		#region Private Members

        private DSPBuffer d = null;
        private DSPState s = null;

		private float average_magnitude;
        		
        private int sigindex = 0;
        private int delayindex = 2;
        private int hangtime = 0;
        
        private CPX[] delay_line = new CPX[8];
        
        #endregion
        		        
        #region Constructor
        
		public BlockNoiseBlanker(ref DSPBuffer dsp_buffer_obj)
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
                this.delay_line[this.sigindex] = d.cpx[i];
                this.average_magnitude = 0.999F * (this.average_magnitude) + 0.001F * magnitude;
                
                if ((this.hangtime == 0) && (magnitude > (this.BlockNBThreshold * this.average_magnitude)))
                {
                	this.hangtime = 7;
                }
                
                if (this.hangtime > 0)
                {
                	d.cpx[i].real = 0.0f;
                	d.cpx[i].imag = 0.0f;
                	this.hangtime--;
                }
                else
                {
                	d.cpx[i] = this.delay_line[this.delayindex];
                }
                
                this.sigindex = (this.sigindex + 7) & 7;
                this.delayindex = (this.delayindex + 7) & 7;
            }
        }

        unsafe public void Process(float * real, float * imag, int size)
        {
            for (int i = 0; i < size; i++)
            {
                float magnitude = (float)Math.Sqrt(real[i] * real[i] + imag[i] * imag[i]);

                this.delay_line[this.sigindex].real = real[i];
                this.delay_line[this.sigindex].imag = imag[i];
                this.average_magnitude = 0.999F * (this.average_magnitude) + 0.001F * magnitude;

                if ((this.hangtime == 0) && (magnitude > (this.BlockNBThreshold * this.average_magnitude)))
                {
                    this.hangtime = 7;
                }

                if (this.hangtime > 0)
                {
                    real[i] = 0.0f;
                    imag[i] = 0.0f;
                    this.hangtime--;
                }
                else
                {
                    real[i] = this.delay_line[this.delayindex].real;
                    imag[i] = this.delay_line[this.delayindex].imag;
                }

                this.sigindex = (this.sigindex + 7) & 7;
                this.delayindex = (this.delayindex + 7) & 7;
            }
        }

        #endregion

        #region Properties

        private float bnb_threshold = 10.0f;
        public float BlockNBThreshold
        {
            get { return bnb_threshold; }
            set { bnb_threshold = value; }
        }

        private bool bnb_switch = false;
        public bool BlockNBSwitchOn
        {
            get { return bnb_switch; }
            set { bnb_switch = value; }
        }

        #endregion
    }
}
