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
	/// Description of agc_new.
	/// </summary>
	public class AGC : IDSPObject
	{
		#region Private members

        private DSPBuffer d = null;
        private DSPState s = null;

        private float setpoint = 1.0f;
        private float error = 0f;
        private float previous_error = 0f;
        private float derivative = 0f;
        private float integral = 0f;
        private float proportional = 0f;
        private float gain = 1.0f;

        private float Kpa = 5.0f; // attack
        private float Kpd = 1500.0f; // decay

        private float mingain = 1.0f;

        #endregion
                          	
        #region Constructor
        
		public AGC(ref DSPBuffer dsp_buffer_obj)
		{
            this.d = dsp_buffer_obj;
            this.s = d.State;                        
		}
		
		#endregion
		
		#region Public Methods
		
        public void Process()
        {
            switch (agcmode)
            {
                case AGCType_e.agcLong:
                    Kpa = 0.5f;
                    Kpd = 675.0f;
                    break;
                case AGCType_e.agcSlow:
                    Kpa = 1.0f;
                    Kpd = 1250.0f;
                    break;
                case AGCType_e.agcMedium:
                    Kpa = 2.0f;
                    Kpd = 2500.0f;
                    break;
                case AGCType_e.agcFast:
                    Kpa = 5.0f;
                    Kpd = 5000.0f;
                    break;
                case AGCType_e.agcOff:
                    d.Scale(fixedgain, fixedgain);
                    return;                    
            }			
        	DoAGC();            
        }
        
        #endregion
        
        #region Private Methods

        private void DoAGC()
        {
            int blocksize = s.DSPBlockSize;

            float envelope = d.GetBlockAvgMagnitudeWithGain(gain, blocksize);

            previous_error = error;
            
            // compute error
            error = setpoint - envelope;

            // deadband
            if ((float)Math.Abs(error) < 0.1f)
            {
                integral = 0f;
                d.Scale(gain, gain);
                gain -= gain_leakage;
                return;
            }

            // computer proportional term
            if (error < 0f)
            {
                proportional = error * Kpd;  // gain decreasing
            }
            else
            {
                proportional = error * Kpa; // gain increasing
            }
            
            //compute integral term
            integral += error * Ki;

            if (integral > integral_limit)
                integral = integral_limit;

            // compute derivative term
            derivative = (error - previous_error) * Kd;

            float Po = proportional + integral + derivative;

            gain += Po;
            if (gain >= maxgain)
                gain = maxgain;
            if (gain < mingain)
                gain = mingain;

            // apply sgc
            d.Scale(gain, gain);

            // gain leakage
            gain -= gain_leakage;
        }                       
                        
        #endregion

        #region Properties

        private float Ki = 10f;
        private float Ki_log = 10f;
        public float AGCIntegralGainDB
        {
            get { return Ki_log; }
            set 
            { 
                Ki_log = value;
                Ki = (float)Math.Pow(10.0, Ki_log / 20.0);
            }
        }

        private float Kd = 50f;
        private float Kd_log = 20f;
        public float AGCDifferentialGainDB
        {
            get { return Kd_log; }
            set 
            { 
                Kd_log = value;
                Kd = (float)Math.Pow(10.0, Kd_log / 20.0);
            }
        }

        private float maxgain = 31622f;
        private float maxloggain = 90f;
        public float AGCMaximumGainDB
        {
            get { return maxloggain; }
            set 
            { 
                maxloggain = value;
                maxgain = (float)Math.Pow(10.0, maxloggain / 20.0);
            }
        }

        private float fixedgain = 316f;
        private float fixedloggain = 50f;
        public float AGCFixedGainDB
        {
            get { return fixedloggain; }
            set
            {
                fixedloggain = value;
                fixedgain = (float)Math.Pow(10.0, fixedloggain / 20.0);
            }
        }

        private float integral_limit = 100f;
        private float integral_log_limit = 20f;
        public float AGCIntegralLimitDB
        {
            get { return integral_log_limit; }
            set
            {
                integral_log_limit = value;
                integral_limit = (float)Math.Pow(10.0, integral_log_limit / 20.0);
            }
        }

        private float gain_leakage = 4f; 
        private float gain_log_leakage = 12f;
        public float AGCGainLeakageDB
        {
            get { return gain_log_leakage; }
            set
            {
                gain_log_leakage = value;
                gain_leakage = (float)Math.Pow(10.0, gain_log_leakage / 20.0);
            }
        }

        private AGCType_e agcmode = AGCType_e.agcMedium;
        public AGCType_e AGCMode
        {
            get { return agcmode; }
            set { agcmode = value; }
        }

        #endregion
    }
}
