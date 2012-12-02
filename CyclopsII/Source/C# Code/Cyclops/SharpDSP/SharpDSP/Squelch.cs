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
	/// Squelch
	/// </summary>
	public class Squelch : IDSPObject
    {
        #region Private members

        private DSPBuffer d = null;
        private DSPState s = null;

        private bool running = false;
        private int num;
        private float power = 0.0f;        
        
        #endregion
                
        #region Constructor

        public Squelch(ref DSPBuffer dsp_buffer_obj)
        {
            this.d = dsp_buffer_obj;
            this.s = d.State;
        }
                       
        #endregion

        #region Public Methods

        public void Process()
        {
        	int blocksize = s.DSPBlockSize;
            
        	this.num = (int)(blocksize * blocksize / s.DSPSampleRate); 
        	if (this.sqflag)
            {
                float temp = 0F;
                this.power = 0F;
                for (int i = 0; i < blocksize; i++)
                {
                	this.power += d.GetPower(i);
                }
                temp = (float)(10.0 * Math.Log10(this.power) + this.sqoffset_meter + this.sqoffset_attenuation + this.sqoffset_gain);
                if (this.sqthreshold > temp)
                {
                    if (this.running == false)
                    {
                        for (int i = 0; i < this.num; i++)
                        {
                            d.cpx[i].real *= 1.0F - (float)(i / this.num);
                            d.cpx[i].imag *= 1.0F - (float)(i / this.num);
                        }
                        for (int i = this.num; i < blocksize; i++)
                        {
                            d.cpx[i].real = 0F;
                            d.cpx[i].imag = 0F;
                        }
                        this.running = true;
                    }
                    else
                    {
                        for (int i = 0; i < blocksize; i++)
                        {
                            d.cpx[i].real = 0F;
                            d.cpx[i].imag = 0F;
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
                        d.cpx[i].real *= 1.0F - (float)(i / this.num);
                        d.cpx[i].imag *= 1.0F - (float)(i / this.num);
                    }
                    this.running = false;
                }
            }
        }       
       
        #endregion

        #region Properties

        private float sqthreshold = -100.0f;
        public float SquelchThreshold
        {
            get { return sqthreshold; }
            set { sqthreshold = value; }
        }

        private float sqoffset_meter = 0.0f;
        public float SquelchMeterOffset
        {
            get { return sqoffset_meter; }
            set { sqoffset_meter = value; }
        }

        private float sqoffset_gain = 0.0f;
        public float SquelchGainOffset
        {
            get { return sqoffset_gain; }
            set { sqoffset_gain = value; }
        }

        private float sqoffset_attenuation = 0.0f;
        public float SquelchAttnOffset
        {
            get { return sqoffset_attenuation; }
            set { sqoffset_attenuation = value; }
        }

        private bool sqflag = false;
        public bool SquelchOn
        {
            get { return sqflag; }
            set { sqflag = value; }
        }

        #endregion

    }
}
