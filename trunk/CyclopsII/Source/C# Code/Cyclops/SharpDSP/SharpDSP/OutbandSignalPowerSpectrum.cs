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
using System.Runtime.InteropServices;

namespace SharpDSP2._1
{
	/// <summary>
	/// Power Spectrum of Signal
	/// </summary>
	public class OutbandPowerSpectrumSignal
	{
		#region PSpectrum Event 
	
		public delegate void PSpectrumEventHandler(object source, PSpectrumEvent e);
		
		public class PSpectrumEvent : EventArgs
	    {
			public float[] buffer;
	
			public PSpectrumEvent(ref float[] buffer, int size)
			{
				this.buffer = new float[size];
				this.buffer = buffer;
	        }
	    }
	
		public static event PSpectrumEventHandler ps_event = null;
		
		#endregion		
	
		#region Private members

        private DSPBuffer d = null;
        private DSPState s = null;

        private float[] window;
        private float[] ps_results;     //holds power spectrum results
        private float[] ps_average;     //holds power spectrum results

        private FilterDesigner filt_design;
		        
        private CPX[] ps_cpx; // power spectrum results		
        private CPX[] tmp_cpx;
        
        private IntPtr plan_fwd_ps;
		private GCHandle h_ps;		
		private GCHandle h_temp;

        private int block_size;

        #endregion
        		        
        #region Constructor
        
		public OutbandPowerSpectrumSignal(ref DSPBuffer dsp_buffer_obj)
		{
            this.d = dsp_buffer_obj;
            this.s = d.State;

            this.block_size = s.DSPBlockSize;
            
			this.window = new float[s.DSPBlockSize];
            this.ps_results = new float[s.DSPBlockSize * 2];
            this.ps_average = new float[s.DSPBlockSize * 2];
            this.filt_design = new FilterDesigner();
            
			filt_design.makewindow(WindowType_e.HAMMING_WINDOW,
                                   s.DSPBlockSize, 
            	                   ref window);

            this.ps_cpx = new CPX[s.DSPBlockSize * 2];
            this.tmp_cpx = new CPX[s.DSPBlockSize * 2];
			
            h_ps = GCHandle.Alloc(ps_cpx, GCHandleType.Pinned);
			h_temp = GCHandle.Alloc(tmp_cpx, GCHandleType.Pinned);
			
            // in: tmp_cpx_1 out: ps_cpx
            plan_fwd_ps = FFTW.dft_1d(s.DSPBlockSize * 2,
			                          h_temp.AddrOfPinnedObject(),
			                          h_ps.AddrOfPinnedObject(),
			                          FFTW.fftw_direction.Forward,
			                          FFTW.fftw_flags.Measure);
		}

        public OutbandPowerSpectrumSignal(int nsize)
        {            
            this.block_size = nsize;

            this.window = new float[nsize];
            this.ps_results = new float[nsize * 2];
            this.ps_average = new float[nsize * 2];
            this.filt_design = new FilterDesigner();

            filt_design.makewindow(WindowType_e.BLACKMANHARRIS_WINDOW,
                                   nsize,
                                   ref window);

            this.ps_cpx = new CPX[nsize * 2];
            this.tmp_cpx = new CPX[nsize * 2];

            h_ps = GCHandle.Alloc(ps_cpx, GCHandleType.Pinned);
            h_temp = GCHandle.Alloc(tmp_cpx, GCHandleType.Pinned);

            // in: tmp_cpx_1 out: ps_cpx
            plan_fwd_ps = FFTW.dft_1d(nsize * 2,
                                      h_temp.AddrOfPinnedObject(),
                                      h_ps.AddrOfPinnedObject(),
                                      FFTW.fftw_direction.Forward,
                                      FFTW.fftw_flags.Measure);
        }		

		public void Exit()
		{
			FFTW.destroy_plan(plan_fwd_ps);
			h_ps.Free();
			h_temp.Free();
		}
		
		#endregion
		
		#region Public Methods
		
		public void Process(float[] real, float[] imag, int nsize)
		{
            int blocksize = nsize;
            int topsize = (blocksize * 2) - 1;

            if (!spectrum_switch) return;
                        
			if (ps_event != null)
			{                               
				// window the data
                
				for (int i = 0; i < blocksize; i++)
				{
					tmp_cpx[i].real = real[i] * this.window[i];
					tmp_cpx[i].imag = imag[i] * this.window[i];                    
				}	
				                
				DoFFTForwardPS();  // note: this reverses the order, need to flip (see below).
                                
                for (int i=0; i<blocksize; i++) // do reorder and flip here
				{
                    ps_results[topsize - i] =
                        (float)(10.0 * Math.Log10(GetPowerPS(i + blocksize) + 1e-180) + this.pscorrection);

                    ps_results[blocksize - i] =
                        (float)(10.0 * Math.Log10(GetPowerPS(i) + 1e-180) + this.pscorrection);                    
                }            
				
                float smoothing_factor = this.pssmoothing;

                if (this.psaveragingon)
                {
                    for (int i = 0; i < this.ps_results.Length; i++)
                    {
                        this.ps_average[i] = (this.ps_results[i] * (1 - smoothing_factor)) + (this.ps_average[i] * smoothing_factor);
                    }
                    ps_event(this, new PSpectrumEvent(ref this.ps_average,
                                                      this.ps_average.Length)); //raise event to indicate power spectrum complete
                    return;
                }
                else
                {
                    for (int i = 0; i < this.ps_results.Length; i++)
                    {
                        this.ps_average[i] = this.ps_results[i]; //(this.ps_results[i] * (0.50f)) + (this.ps_average[i] * 0.50f);
                    }
                    ps_event(this, new PSpectrumEvent(ref this.ps_average,
                                                      this.ps_results.Length)); //raise event to indicate power spectrum complete
                }
			}
		}
		#endregion
		
		#region Private Methods
		
		private float GetPowerPS(int index)
		{
			return ps_cpx[index].real * ps_cpx[index].real + ps_cpx[index].imag * ps_cpx[index].imag;
		}
		
		private void DoFFTForwardPS()
		{
			FFTW.execute(plan_fwd_ps);
		}	
		
		#endregion

        #region Properties

        private bool psaveragingon = false;
        public bool PowerSpectrumAveragingOn
        {
            get { return psaveragingon; }
            set 
            { 
                psaveragingon = value;
                if (psaveragingon) ps_average = (float[])ps_results.Clone();
            }
        }

        private float pssmoothing = 0.99f;
        public float PowerSpectrumSmoothingFactor
        {
            get { return pssmoothing; }
            set { pssmoothing = value; }
        }

        private float pscorrection = 0.0f;
        public float PowerSpectrumCorrection
        {
            get { return pscorrection; }
            set { pscorrection = value; }
        }

        private int rate = 10;
        public int UpdatesPerSecond
        {
            get { return rate; }
            set
            {
                rate = value;                
            }
        }
                
        private bool spectrum_switch = true;
        public bool SpectrumSwitchOn
        {
            get { return spectrum_switch; }
            set { spectrum_switch = value; }
        }

        #endregion
    }
}
