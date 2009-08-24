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
using System.Collections.Generic;
using System.Text;

namespace SharpDSP2._1
{
    class DCCorrector : IDSPObject
    {
        #region Private Members

        private DSPBuffer d = null;
        private DSPState s = null;
        private float dc_i = 0.0f;
        private float dc_q = 0.0f;
        private float smooth = 0f;
        private float oneminussmooth = 0f;

        #endregion

        #region Constructor

        public DCCorrector(ref DSPBuffer dsp_buffer_obj)
        {
            this.d = dsp_buffer_obj;
            this.s = d.State;
            this.smooth = 0.001f;
            this.oneminussmooth = 1.0f - smooth;
        }

        #endregion

        #region Public Methods

        public void Process()
        {
            int blocksize = s.DSPBlockSize;
                        
            for (int i = 0; i < blocksize; i++)
            {
                this.dc_i = oneminussmooth * this.dc_i + smooth * d.cpx[i].real;
                this.dc_q = oneminussmooth * this.dc_q + smooth * d.cpx[i].imag;

                if (this.dc_i >= this.dc_limit)
                {
                    this.dc_i = this.dc_limit;
                }
                else if (this.dc_i <= -this.dc_limit)
                {
                    this.dc_i = -this.dc_limit;
                }

                if (this.dc_q >= this.dc_limit)
                {
                    this.dc_q = this.dc_limit;
                }
                else if (this.dc_q <= -this.dc_limit)
                {
                    this.dc_q = -this.dc_limit;
                }

                d.cpx[i].real -= this.dc_i;
                d.cpx[i].imag -= this.dc_q;
            }
        }

        unsafe public void Process(float * real, float * imag, int nsize)
        {
            int blocksize = nsize;

            for (int i = 0; i < blocksize; i++)
            {
                this.dc_i = oneminussmooth * this.dc_i + smooth * real[i];
                this.dc_q = oneminussmooth * this.dc_q + smooth * imag[i];

                if (this.dc_i >= this.dc_limit)
                {
                    this.dc_i = this.dc_limit;
                }
                else if (this.dc_i <= -this.dc_limit)
                {
                    this.dc_i = -this.dc_limit;
                }

                if (this.dc_q >= this.dc_limit)
                {
                    this.dc_q = this.dc_limit;
                }
                else if (this.dc_q <= -this.dc_limit)
                {
                    this.dc_q = -this.dc_limit;
                }
                
                real[i] -= this.dc_i;
                imag[i] -= this.dc_q;
            }
        }

        #endregion

        #region Properties

        public float DCCorrectionI
        {
            get { return this.dc_i; }
            set { this.dc_i = value; }
        }

        public float DCCorrectionQ
        {
            get { return this.dc_q; }
            set { this.dc_q = value; }
        }

        private float dc_limit = 0.00010f;
        public float DCLimit
        {
            get { return dc_limit; }
            set { dc_limit = value; }
        }

        #endregion
    }
}
