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
    class DAgc : IDSPObject
    {
        private float gain_bottom;
        private float gain_fix;
        private float gain_limit;
        private float gain_now;
        private float gain_top;
        private float gain_old;
        
        private int hangindex;
        private int indx;
        private int mask;
        private int sndx;
        
        private float attack;
        private float decay;
        private float hangthresh;
        private float hangtime;
        private float one_m_attack;
        private float one_m_decay;
        private float slope;

        private CPX[] G;
        
        private DSPBuffer d = null;
        private DSPState s = null;

        int hangcount = 0;

        public DAgc(ref DSPBuffer dsp_buffer_obj)
        {
            this.d = dsp_buffer_obj;
            this.s = d.State;

            ComputeAttackTime(newattack);            
            //this.attack = (float)(1.0f - Math.Exp(-1000.0 / (s.DSPSampleRate * newattack)));
            //this.one_m_attack = (float)Math.Exp(-1000.0 / (s.DSPSampleRate * newattack));
            //this.sndx = (int)(s.DSPSampleRate * (newattack / 333.33f));

            ComputeDecayTime(newdecay);
            //this.decay = (float)(1.0f - Math.Exp(-1000.0 / (s.DSPSampleRate * newdecay)));
            //this.one_m_decay = (float)Math.Exp(-1000.0 / (s.DSPSampleRate * newdecay));
            
            this.hangindex = this.indx = 0;
            this.hangtime = newhangtime * 0.001f;
            this.hangthresh = 1.0f;
            this.gain_fix = newfixedgain;

            this.slope = newslope;
            this.gain_top = newmaxgain;
            this.hangthresh = this.gain_bottom = newmingain;
            this.gain_old = this.gain_now = newcurrentgain;

            this.gain_limit = newlimit;

            this.mask = 2 * s.DSPBlockSize;
            this.G = new CPX[this.mask];
            this.mask -= 1;                          
        }

        private void ComputeDecayTime(float decayTimeMilliseconds)
        {
            this.decay = (float)(1.0f - Math.Exp(-1000.0 / (s.DSPSampleRate * decayTimeMilliseconds)));
            this.one_m_decay = (float)Math.Exp(-1000.0 / (s.DSPSampleRate * decayTimeMilliseconds));
        }

        float oldSampleRate = 0;
        private void ComputeAttackTime(float attackTimeMilliseconds)
        {
            this.attack = (float)(1.0f - Math.Exp(-1000.0 / (s.DSPSampleRate * attackTimeMilliseconds)));
            this.one_m_attack = (float)Math.Exp(-1000.0 / (s.DSPSampleRate * attackTimeMilliseconds));
            if (oldSampleRate != s.DSPSampleRate)
            {
                // update this ONLY if sample rate changes!
                this.sndx = (int)(s.DSPSampleRate * (attackTimeMilliseconds / 333.33f));
                oldSampleRate = s.DSPSampleRate;
            }
        }

        public void Process()
        {
#if false
            switch (agc_mode)
            {
                case AGCType_e.agcOff:
                    this.d.Scale(this.gain_fix, this.gain_fix);
                    return;
                case AGCType_e.agcLong:
                    this.decay = 1.041661241e-5f;
                    this.one_m_decay = 0.99998958f;
                    break;
                case AGCType_e.agcSlow:
                    this.decay = 4.1665798623191641e-5f; // 500 ms
                    this.one_m_decay = 0.99995833420137681f;                    
                    break;
                case AGCType_e.agcMedium:
                    this.decay = 8.3329861207515066e-5f; // 250 ms
                    this.one_m_decay = 0.99991667013879248f;
                    break;
                case AGCType_e.agcFast:
                    this.decay = 0.0002083116334513635f; // 100 ms
                    this.one_m_decay = 0.99979168836654864f;
                    break;
            }
#else
            if (agc_mode == AGCType_e.agcOff)
            {
                this.d.Scale(this.gain_fix, this.gain_fix);
                return;
            }

            ComputeAttackTime(newattack);            

#endif
            DoAGC2();
        }
        
        private void DoAGC()
        {
          
            int lochangtime = (int)(s.DSPSampleRate * this.hangtime);
            float lochangthresh;

            if (this.hangthresh > 0)
            {
                lochangthresh = this.gain_top * this.hangthresh + this.gain_bottom * (float)(1.0 - this.hangthresh);
            }
            else
            {
                lochangthresh = 0.0f;
            }
                        
            for (int i = 0; i < s.DSPBlockSize; i++)
            {
                float tmp;

                this.G[this.indx] = d.cpx[i];

                tmp = 1.1f * GetMagnitude(this.G[this.indx]);

                if (tmp != 0.0f)
                {
                    tmp = this.gain_limit / tmp;
                }
                else
                {
                    tmp = this.gain_now;
                }

                if (tmp < lochangthresh)
                {
                    this.hangindex = lochangtime;
                }

                if (tmp > this.gain_now)
                {
                    if (this.hangindex++ > lochangtime)
                    {
                        this.gain_now = this.one_m_decay * this.gain_now + this.decay * (float)Math.Min(this.gain_top, tmp);
                    }
                }
                else
                {
                    this.hangindex = 0;
                    this.gain_now = this.one_m_attack * this.gain_now + this.attack * (float)Math.Max(tmp, this.gain_bottom);
                }
                               
                this.d.cpx[i] = Scale(this.G[this.sndx], (float)Math.Min(this.slope * this.gain_now, this.gain_top));
                
                this.indx = (this.indx + this.mask) & this.mask;
                this.sndx = (this.sndx + this.mask) & this.mask;                
            }
        }

        private void DoAGC2()
        {            
            for (int i = 0; i < s.DSPBlockSize; i++)
            {
                this.G[this.indx] = d.cpx[i];

                float tmp = 1.1f * GetMagnitude(this.G[this.indx]);
                                
                if (tmp == 0.0f)
                {
                    tmp = this.gain_now;
                }
                else
                {                    
                    tmp = this.gain_limit / tmp;
                }

                if (tmp > this.gain_now)
                {
                    this.gain_now = this.one_m_decay * this.gain_now + this.decay * (float)Math.Min(this.gain_top, tmp);
                }
                else
                {
                    this.gain_now = this.one_m_attack * this.gain_now + this.attack * (float)Math.Max(tmp, this.gain_bottom);
                }

                this.d.cpx[i] = Scale(this.G[this.sndx], (float)Math.Min(this.slope * this.gain_now, this.gain_top));

                this.indx = (this.indx + this.mask) & this.mask;
                this.sndx = (this.sndx + this.mask) & this.mask;
            }
        }

        private float GetMagnitude(CPX cpx)
        {
            return (float)Math.Sqrt(cpx.real * cpx.real + cpx.imag * cpx.imag);
            //if (Math.Abs(cpx.real) > Math.Abs(cpx.imag))
            //    return (float) (Math.Abs(cpx.real) + 0.4 * Math.Abs(cpx.imag));
            //else
            //    return (float) (Math.Abs(cpx.imag) + 0.4 * Math.Abs(cpx.real));
        }

        private CPX Scale(CPX cpx, float value)
        {
            cpx.real *= value;
            cpx.imag *= value;

            return cpx;
        }

        private float newmaxgain = 31622f;
        private float maxloggain = 90f;
        public float AGCMaximumGainDB
        {
            get { return maxloggain; }
            set
            {
                maxloggain = value;
                newmaxgain = (float)Math.Pow(10.0, maxloggain / 20.0);
                this.gain_top = newmaxgain;
            }
        }

        private float newfixedgain = 31622f;
        private float fixedloggain = 90f;
        public float AGCFixedGainDB
        {
            get { return fixedloggain; }
            set
            {
                fixedloggain = value;
                newfixedgain = (float)Math.Pow(10.0, fixedloggain / 20.0);
                this.gain_fix = newfixedgain;
            }
        }

        private float newattack = 0.258f;
        public float AGCAttackTime
        {
            get { return newattack; }
            set 
            {
                newattack = value;
                this.indx = 0;
                this.G = new CPX[this.G.Length];

                ComputeAttackTime(newattack);
            }
        }

        private float longdecay = 2000f;
        private float slowdecay = 500f;
        private float mediumdecay = 250f;
        private float fastdecay = 100f;
        private float userdecay = 100f;

        private float newdecay = 250f;
        public float AGCDecayTime
        {
            get { return newdecay; }
            set 
            { 
                newdecay = value;
                SetAGCDecayTime(value, agc_mode);
            }
        }

        public void SetAGCDecayTime(float decayTimeMilliseconds, AGCType_e mode)
        {
            switch (agc_mode)
            {
                case AGCType_e.agcFast:
                    fastdecay = decayTimeMilliseconds;
                    break;
                case AGCType_e.agcLong:
                    longdecay = decayTimeMilliseconds;
                    break;
                case AGCType_e.agcMedium:
                    mediumdecay = decayTimeMilliseconds;
                    break;
                case AGCType_e.agcSlow:
                    slowdecay = decayTimeMilliseconds;
                    break;
                case AGCType_e.agcUser:
                    userdecay = decayTimeMilliseconds;
                    break;
            }
        }

        public float GetAGCDecayTime(AGCType_e mode)
        {
            switch (agc_mode)
            {
                case AGCType_e.agcFast:
                    return fastdecay;
                case AGCType_e.agcLong:
                    return longdecay;
                case AGCType_e.agcMedium:
                    return mediumdecay;
                case AGCType_e.agcSlow:
                    return slowdecay;
                case AGCType_e.agcUser:
                    return userdecay;
            }

            return 0.0F;
        }

        private float newhangtime = 0.75f;
        public float AGCHangTime
        {
            get { return newhangtime; }
            set 
            { 
                newhangtime = value;
                this.hangtime = newhangtime;
            }
        }

        private float newhangthres = 1.0f;
        public float AGCHangThres
        {
            get { return newhangthres; }
            set
            {
                newhangthres = value;
                this.hangthresh = newhangthres;
            }
        }

        private float newslope = 1.0f;
        public float AGCSlope
        {
            get { return newslope; }
            set 
            { 
                newslope = value;
                this.slope = newslope;
            }
        }

        private float newmingain = 0.01f;
        private float newcurrentgain = 1f;
        private float newlimit = 1.0f;

        private void UpdateCurrentAGCDelayFromMode()
        {
            switch (agc_mode)
            {
                case AGCType_e.agcFast:
                    newdecay = fastdecay;
                    break;
                case AGCType_e.agcLong:
                    newdecay = longdecay;
                    break;
                case AGCType_e.agcMedium:
                    newdecay = mediumdecay;
                    break;
                case AGCType_e.agcSlow:
                    newdecay = slowdecay;
                    break;
                case AGCType_e.agcUser:
                    newdecay = userdecay;
                    break;
            }
        }

        private AGCType_e agc_mode = AGCType_e.agcMedium;
        public AGCType_e AGCMode
        {
            get { return agc_mode; }
            set { 
                agc_mode = value;
                UpdateCurrentAGCDelayFromMode();
            }
        }        
    }
}
