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
	/// The AM and FM PLLs
	/// </summary>
	public class PLL : IDSPObject
    {
        #region Private members

        private DSPBuffer d = null;
        private DSPState s = null;
        private float TWOPI = (float)(Math.PI * 2);
        
        #endregion        
                                
        #region Constructor

        public PLL(ref DSPBuffer dsp_buffer_obj)
        {
            this.d = dsp_buffer_obj;
            this.s = d.State;

        	this.pll_internal_state.phase = 0.0F;
            this.pll_internal_state.delay_real = 0F;
            this.pll_internal_state.delay_imag = 1F;
            this.pll_internal_state.lockcurrent = 0.5F;
            this.pll_internal_state.lockprevious = 1F;
            this.pll_internal_state.dc = 0F;
            this.pll_internal_state.afc = 0F;             
        }       
          
        #endregion
                        
        #region Public Methods
        
        public void Process()
        {  
        	switch (s.DSPMode)
        	{   
        		case DSPMode_e.AM:
    				DoMagnitude();
    				break;
    			case DSPMode_e.SAM:
    				DoAMPLL();
    				break;
    			case DSPMode_e.FMN:
    				DoFMPLL();
    				break;
    			default:
    				break;
    		}        		
        }
        
		#endregion
		
		#region Private Methods
		
		private void DoMagnitude()
        {
			int blocksize = s.DSPBlockSize;
            
            for (int i = 0; i < blocksize; i++)
            {
            	float magn = d.GetMagnitude(i);
                pll_internal_state.dc = 0.999f * pll_internal_state.dc + 0.001f * magn;
                pll_internal_state.smooth = 0.70f * pll_internal_state.smooth + 0.30f * (magn - pll_internal_state.dc);
                d.cpx[i].real = pll_internal_state.smooth;
                d.cpx[i].imag = pll_internal_state.smooth;                
            }                                   
        }
		
		private void DoAMPLL()
        {
            int blocksize = s.DSPBlockSize;
            float alpha = pll_internal_state.alpha;
            float beta = pll_internal_state.beta;
            float lowlimit = PLLLowLimit;
            float hilimit = PLLHighLimit;

            for (int i = 0; i < blocksize; i++)
            {
                float tempCos = (float)Math.Cos(pll_internal_state.phase);
                float tempSin = (float)Math.Sin(pll_internal_state.phase);

                float delay_real = tempCos * d.cpx[i].real + tempSin * d.cpx[i].imag;
                float delay_imag = -tempSin * d.cpx[i].real + tempCos * d.cpx[i].imag;

                float difference = d.GetMagnitude(i) * 
                					(float)Math.Atan2(delay_imag, delay_real);

                PLLFrequency += beta * difference;

                if (PLLFrequency < lowlimit)
                    PLLFrequency = lowlimit;
                if (PLLFrequency > hilimit)
                    PLLFrequency = hilimit;

                pll_internal_state.phase += PLLFrequency + alpha * difference;

                while (pll_internal_state.phase >= this.TWOPI)
                    pll_internal_state.phase -= this.TWOPI;
                while (pll_internal_state.phase < 0)
                    pll_internal_state.phase += this.TWOPI;

                pll_internal_state.lockcurrent = 0.999F * pll_internal_state.lockcurrent + 0.001F * (float)Math.Abs(delay_real);
                pll_internal_state.lockprevious = pll_internal_state.lockcurrent;
                pll_internal_state.dc = 0.9999F * pll_internal_state.dc + 0.0001F * delay_real;
                d.tmp_cpx_1[i].real = delay_real - pll_internal_state.dc;
                d.tmp_cpx_1[i].imag = d.tmp_cpx_1[i].real;
            }
            d.CopyTemp1BotToMainBot();
        }

        private void DoFMPLL()
        {
            int blocksize = s.DSPBlockSize;
            float alpha = pll_internal_state.alpha;
            float beta = pll_internal_state.beta;
            float lowlimit = PLLLowLimit;
            float hilimit = PLLHighLimit;
            
            for (int i = 0; i < blocksize; i++)
            {
                float tempCos = (float)Math.Cos(pll_internal_state.phase);
                float tempSin = (float)Math.Sin(pll_internal_state.phase);

                float delay_real = tempCos *  d.cpx[i].real + tempSin *  d.cpx[i].imag;
                float delay_imag = -tempSin *  d.cpx[i].real + tempCos * d.cpx[i].imag;
                
                float difference = (float)(Math.Atan2(delay_imag, delay_real));

                PLLFrequency += beta * difference;

                if (PLLFrequency < lowlimit)
                    PLLFrequency = lowlimit;
                if (PLLFrequency > hilimit)
                    PLLFrequency = hilimit;

                pll_internal_state.phase += PLLFrequency + alpha * difference;

                while (pll_internal_state.phase >= this.TWOPI)
                    pll_internal_state.phase -= this.TWOPI;
                while (pll_internal_state.phase < 0)
                    pll_internal_state.phase += this.TWOPI;

                pll_internal_state.afc = 0.9999F * pll_internal_state.afc + 0.0001F * PLLFrequency;
                d.tmp_cpx_1[i].real = (PLLFrequency - pll_internal_state.afc) * pll_internal_state.cvt;
                d.tmp_cpx_1[i].imag = d.tmp_cpx_1[i].real;
            }
            d.CopyTemp1BotToMainBot();
        }
        
        #endregion

        #region Properties

        internal struct PLLInternal
        {
            internal float phase;
            internal float delay_real;
            internal float delay_imag;
            internal float alpha;
            internal float beta;
            internal float lockcurrent;
            internal float lockprevious;
            internal float dc;
            internal float afc;
            internal float cvt;
            internal float smooth;
        }

        internal PLLInternal pll_internal_state = new PLLInternal();
        
        private float pllfrequency = 0.0f;
        private float pllfrequencynorm;
        public float PLLFrequency
        {
            get { return pllfrequency; }
            set { pllfrequency = value;
                pllfrequencynorm = pllfrequency * this.TWOPI / s.DSPSampleRate;
            }
        }
        private float plllowlimit = -1000f;
        private float plllowlimitnorm;
        public float PLLLowLimit
        {
            get { return plllowlimit; }
            set { plllowlimit = value;
                plllowlimitnorm = plllowlimit * this.TWOPI / s.DSPSampleRate;
            }
        }
        private float pllhilimit = 1000f;
        private float pllhilimitnorm;
        public float PLLHighLimit
        {
            get { return pllhilimit; }
            set { pllhilimit = value;
                pllhilimitnorm = pllhilimit * this.TWOPI / s.DSPSampleRate;
            }
        }
        private float pllbandwidth = 500f;
        private float pllbandwidthnorm;
        public float PLLBandwidth
        {
            get { return pllbandwidth; }
            set
            {
                pllbandwidth = value;
                pllbandwidthnorm = pllbandwidth * this.TWOPI / s.DSPSampleRate;

                pll_internal_state.alpha = 0.3F * pllbandwidthnorm; pll_internal_state.beta =
                    pll_internal_state.alpha * pll_internal_state.alpha * 0.25F;
                pll_internal_state.cvt = 0.45F * s.DSPSampleRate / (float)(Math.PI * pllbandwidth);
            }
        }


        #endregion

    }
}
