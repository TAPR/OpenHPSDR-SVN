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
	/// Local Oscillator Class
	/// </summary>
	public class Oscillator : IDSPObject
	{
		#region Private Members

        private DSPBuffer d = null;
        private DSPState s = null;

        private double frequencyTone;
        private double phaseTone;
        private float magnitudeTone;
        private double stepTone;
        private double CosN;
        private double SinN;
        private readonly float TWO_PI = (float)Math.PI * 2;

        private CPX[] temp;

        #endregion
        
		#region Constructor

		public Oscillator(ref DSPBuffer dsp_buffer_obj)
		{
            this.d = dsp_buffer_obj;
            this.s = d.State;

			this.frequencyTone = this.LOFrequency / s.DSPSampleRate;
			this.magnitudeTone = (float)this.LOMagnitude;
			this.stepTone = this.LOFrequency * this.TWO_PI;
            this.phaseTone = this.LOPhase;
            this.CosN = 0;
            this.SinN = 0;

            this.temp = new CPX[this.s.PowerSpectrumBlockSize];
		}
						
		#endregion
        
        #region Public members
        
        public void Process()
        {        				
        	if (this.localoscon && (this.lofrequency != 0.0f))
        	{
        		int blocksize = s.DSPBlockSize;
        	
        		this.frequencyTone = this.lofrequency / s.DSPSampleRate;
				this.stepTone = this.frequencyTone * this.TWO_PI;
							
	        	for (int i = 0; i < blocksize; i++)
	            {
					if (this.phaseTone >= this.TWO_PI)
						this.phaseTone -= this.TWO_PI;
					if (this.phaseTone < 0)
						this.phaseTone += this.TWO_PI;
	
	                this.CosN = Math.Cos(this.phaseTone);
	                this.SinN = Math.Sin(this.phaseTone);
	                
	                d.tmp_cpx_1[i].real = (float)((this.CosN * d.cpx[i].real) - (this.SinN * d.cpx[i].imag)) * this.magnitudeTone;
	                d.tmp_cpx_1[i].imag = (float)((this.CosN * d.cpx[i].imag) + (this.SinN * d.cpx[i].real)) * this.magnitudeTone;
	               	
					this.phaseTone += this.stepTone;					
	            }
	        	d.CopyTemp1BotToMainBot();	        	
        	}
        	else { return; }
        }

        public void Process(float[] real, float[] imag, int size, float spec_samplerate)
        {
            if (this.localoscon && (this.lofrequency != 0.0f))
            {
                this.frequencyTone = this.lofrequency / spec_samplerate;
                this.stepTone = this.frequencyTone * this.TWO_PI;

                for (int i = 0; i < size; i++)
                {
                    if (this.phaseTone >= this.TWO_PI)
                        this.phaseTone -= this.TWO_PI;
                    if (this.phaseTone < 0)
                        this.phaseTone += this.TWO_PI;

                    this.CosN = Math.Cos(this.phaseTone);
                    this.SinN = Math.Sin(this.phaseTone);

                    this.temp[i].real = (float)((this.CosN * real[i]) - (this.SinN * imag[i])) * this.magnitudeTone;
                    this.temp[i].imag = (float)((this.CosN * imag[i]) + (this.SinN * real[i])) * this.magnitudeTone;

                    this.phaseTone += this.stepTone;

                    real[i] = this.temp[i].real;
                    imag[i] = this.temp[i].imag;
                }                
            }
            else { return; }
        }
        
		#endregion

        #region Properties

        private float lofrequency = 10.0f;
        public float LOFrequency
        {
            get { return lofrequency; }
            set { lofrequency = value; }
        }
        private float lophase = 0.0f;
        public float LOPhase
        {
            get { return lophase; }
            set { lophase = value; }
        }
        private float lomagnitude = 1.0f;
        public float LOMagnitude
        {
            get { return lomagnitude; }
            set { lomagnitude = value; }
        }
        private bool localoscon = false;
        public bool LocalOscillatorOn
        {
            get { return localoscon; }
            set { localoscon = value; }
        }

        #endregion
    }
}
