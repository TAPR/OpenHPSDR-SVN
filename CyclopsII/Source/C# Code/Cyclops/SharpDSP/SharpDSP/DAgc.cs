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

using System.Runtime.InteropServices;

namespace SharpDSP2._1
{
    class DAgc : IDSPObject
    {
        [DllImport("kernel32.dll")]
        static extern void OutputDebugString(string lpOutputString);

        private float gain_bottom; 
        private float gain_fix;
        private float gain_limit;
        private float fast_now;
        private float gain_now;
        private float gain_top;
        private float gain_old;
        
        private int hangindex;
        private int indx;
        private int fastindx;
        private int mask;
        private int sndx;
        

        private float hangthresh;
        private float hangtime;
        private float hangdecay;
        private float one_m_hangdecay;

        private int fasthang;
        private float fasthangtime;

        private float attack;
        private float one_m_attack;

        private float fast_attack;
        private float one_m_fastattack;

        private float one_m_decay;
        private float decay;

        private float fast_decay;
        private float one_m_fastdecay;
        
        private float slope;

        private CPX[] G;
        
        private DSPBuffer d = null;
        private DSPState s = null;

        // int hangcount = 0;

        private float fastattacktime = 0.2F;
        private float fastdecaytime = 3.0F;

        private float hangdecaytime = 5.0F;

        public DAgc(ref DSPBuffer dsp_buffer_obj)
        {
            this.d = dsp_buffer_obj;
            this.s = d.State;

            fastattacktime = 0.2F;
            fastdecaytime = 3.0F;
            ComputeFastAttackTime(fastattacktime);
            ComputeFastDecayTime(fastdecaytime);
            
            ComputeAttackTime(newattack);            
            ComputeDecayTime(newdecay);
            ComputeHangDecayTime(hangdecaytime);
            
            this.hangindex = this.indx = 0;
            this.hangtime = newhangtime * 0.001f;
            this.hangthresh = 1.0f;
            this.gain_fix = newfixedgain;

            this.slope = newslope;
            this.gain_top = newmaxgain;
            this.hangthresh = this.gain_bottom = newmingain;
            this.fast_now = this.gain_old = this.gain_now = newcurrentgain;

            this.gain_limit = newlimit;

            this.mask = 2 * s.DSPBlockSize;
            this.G = new CPX[this.mask];
            this.mask -= 1;

            fasthang = 0;
            fasthangtime = 0.1F * hangtime;  
        }

        private void ComputeHangDecayTime(float decayTimeMilliseconds)
        {
            this.hangdecay = (float)(1.0f - Math.Exp(-1000.0 / (s.DSPSampleRate * decayTimeMilliseconds)));
            this.one_m_hangdecay = (float)Math.Exp(-1000.0 / (s.DSPSampleRate * decayTimeMilliseconds));
        }

        private void ComputeFastDecayTime(float decayTimeMilliseconds)
        {
            this.fast_decay = (float)(1.0f - Math.Exp(-1000.0 / (s.DSPSampleRate * decayTimeMilliseconds)));
            this.one_m_fastdecay = (float)Math.Exp(-1000.0 / (s.DSPSampleRate * decayTimeMilliseconds));
        }

        private void ComputeFastAttackTime(float attackTimeMilliseconds)
        {
            this.fast_attack = (float)(1.0f - Math.Exp(-1000.0 / (s.DSPSampleRate * attackTimeMilliseconds)));
            this.one_m_fastattack = (float)Math.Exp(-1000.0 / (s.DSPSampleRate * attackTimeMilliseconds));

            if ((oldFastSampleRate != s.DSPSampleRate) || (attackTimeMilliseconds != oldFastAttackTime))
            {
                // update this ONLY if sample rate changes or attack time changes!
                this.fastindx = (int)(.002F * s.DSPSampleRate);

                oldFastSampleRate = s.DSPSampleRate;
                oldFastAttackTime = attackTimeMilliseconds;
            }
        }

        private void ComputeDecayTime(float decayTimeMilliseconds)
        {
            this.decay = (float)(1.0f - Math.Exp(-1000.0 / (s.DSPSampleRate * decayTimeMilliseconds)));
            this.one_m_decay = (float)Math.Exp(-1000.0 / (s.DSPSampleRate * decayTimeMilliseconds));
        }

        float oldFastSampleRate = 0;
        float oldFastAttackTime = 0;

        float oldSampleRate = 0;
        float oldAttackTime = 0;

        private void ComputeAttackTime(float attackTimeMilliseconds)
        {
            this.attack = (float)(1.0f - Math.Exp(-1000.0 / (s.DSPSampleRate * attackTimeMilliseconds)));
            this.one_m_attack = (float)Math.Exp(-1000.0 / (s.DSPSampleRate * attackTimeMilliseconds));

            if ((oldSampleRate != s.DSPSampleRate) || (attackTimeMilliseconds != oldAttackTime))
            {
                // update this ONLY if sample rate changes or attack time changes!
                this.sndx = (int)(s.DSPSampleRate * (attackTimeMilliseconds / 333.33f));

                oldSampleRate = s.DSPSampleRate;
                oldAttackTime = attackTimeMilliseconds;
            }
        }

        public void Process()
        {
            if (agc_mode == AGCType_e.agcOff)
            {
                this.d.Scale(this.gain_fix, this.gain_fix);
                return;
            }

            // compute these, in case either the times have changed or the sample rate has changed...
            // otherwise, they would not need recomputation.  Best to recompute here, as then there
            // is no possiblity of an other thread being used by the UI changing the value while a block
            // is being processed by another thread...
            ComputeAttackTime(newattack);
            ComputeDecayTime(newdecay);

            ComputeFastAttackTime(fastattacktime);
            ComputeFastDecayTime(fastdecaytime);

            ComputeHangDecayTime(hangdecaytime);

            switch (agcMethod)
            {
                case AGCMethod.AGCHang:
                    DoAGC();
                    break;

                case AGCMethod.AGCNoHang:
                    DoAGC2();
                    break;

                case AGCMethod.AGCHarman:
                    DoHarmanAGC();
                    break;
            }
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

            // clear before each block is processed
            agcStatus = AGC_Status.None;

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
                        this.gain_now = this.one_m_hangdecay * this.gain_now + this.hangdecay * (float)Math.Min(this.gain_top, tmp);
                    }
                    else
                    {
                        agcStatus |= AGC_Status.Hang;
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

        /// <summary>
        /// implements AGC with fast and slow time constants (the Phil Harman VK6APH AGC...)
        /// </summary>
        private void DoHarmanAGC()
        {

            int lochangtime = (int)(s.DSPSampleRate * this.hangtime);
            int locfasthangtime = (int)(s.DSPSampleRate * this.fasthangtime);

            float lochangthresh;

            if (this.hangthresh > 0)
            {
                lochangthresh = this.gain_top * this.hangthresh + this.gain_bottom * (float)(1.0 - this.hangthresh);
            }
            else
            {
                lochangthresh = 0.0f;
            }

            // clear before each block is processed
            agcStatus = AGC_Status.None;

            for (int i = 0; i < s.DSPBlockSize; i++)
            {
                float tmp;

                this.G[this.indx] = d.cpx[i];

                // is times 1.0, not 1.1, in DttSP\dttspagc.c...
                tmp = 1.0f * GetMagnitude(this.G[this.indx]);

                if (tmp > 0.0000005F)
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
                    // if hangtime is over, use a fast decay rate specialy for hang...
                    if (this.hangindex++ > lochangtime)
                    {
                        this.gain_now = this.one_m_hangdecay * this.gain_now + this.hangdecay * (float)Math.Min(this.gain_top, tmp);
                    }
                    else
                    {
                        agcStatus |= AGC_Status.Hang;
                    }
                }
                else
                {
                    this.hangindex = 0;
                    this.gain_now = this.one_m_attack * this.gain_now + this.attack * (float)Math.Max(tmp, this.gain_bottom);
                }

                // stuff added for the Harman AGC (fast gain and slow gain)
                tmp = GetMagnitude(this.G[this.fastindx]);
                if (tmp > 0.0000005F)
                {
                    tmp = this.gain_limit / tmp;
                }
                else
                {
                    tmp = this.fast_now;
                }

                if (tmp > this.fast_now)
                {
                    // if fasthangtime (impulse hang time) is over, use a fast decay rate specialy for fast hang...
                    if (this.fasthang++ > locfasthangtime)
                    {
                        this.fast_now = (float)Math.Min(this.one_m_fastdecay * this.fast_now + this.fast_decay * (float)Math.Min(this.gain_top, tmp), this.gain_top);
                    }
                    else
                    {
                        agcStatus |= AGC_Status.Fast_hang;
                    }
                }
                else
                {
                    this.fasthang = 0;
                    this.fast_now = (float)Math.Min(one_m_fastattack * this.fast_now + this.fast_attack * (float)Math.Max(tmp, this.gain_bottom), this.gain_bottom);
                }

                this.fast_now = (float)Math.Max((float)Math.Min(this.fast_now, this.gain_top), this.gain_bottom);
                this.gain_now = (float)Math.Max((float)Math.Min(this.gain_now, this.gain_top), this.gain_bottom);
                // end stuff added for the Harman AGC

                // now scale this datapoint (maths are different for Harman AGC...)
                //this.d.cpx[i] = Scale(this.G[this.sndx], (float)Math.Min(this.slope * this.gain_now, this.gain_top));
                this.d.cpx[i] = Scale(this.G[this.sndx], (float)Math.Min(this.fast_now, (float)Math.Min(this.slope * this.gain_now, this.gain_top)));

                // adjust indices (they become one less, with wrap-around...)
                this.indx = (this.indx + this.mask) & this.mask;
                this.sndx = (this.sndx + this.mask) & this.mask;
                this.fastindx = (this.fastindx + this.mask) & this.mask;
            }
        }

        private void DoAGC2()
        {
            // clear before each block is processed
            agcStatus = AGC_Status.None;

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

        private AGCMethod agcMethod = AGCMethod.AGCNoHang;

        public AGCMethod AgcMethod
        {
            get { return agcMethod; }
            set { agcMethod = value; }
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

        public float AGCFastAttackTime
        {
            get { return fastattacktime; }
            set
            {
                fastattacktime = value;
            }
        }

        public float AGCFastDecayTime
        {
            get { return fastdecaytime; }
            set
            {
                fastdecaytime = value;
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

        private float newhangdecaytime = 5.0F;
        public float AGCHangDecayTime
        {
            get { return newhangdecaytime; }
            set
            {
                newhangdecaytime = value;
                this.hangdecaytime = newhangdecaytime;
            }
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

        private AGC_Status agcStatus;

        public AGC_Status AGCStatus
        {
            get { return agcStatus; }
        }
    }
}
