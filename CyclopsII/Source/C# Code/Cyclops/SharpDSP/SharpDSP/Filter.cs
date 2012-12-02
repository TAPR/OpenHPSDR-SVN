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
	/// Filter (Frequency Domain)
	/// </summary>
		
	public class Filter : IDSPObject
	{
		#region Private members
                                
        private FilterDesigner filter_designer;
        private DSPBuffer d = null;
        private DSPState s = null;

        private CPX[] ovlp_cpx;
        private CPX[] filter_cpx;
        private CPX[] tmp_cpx;
        
        private IntPtr plan_fwd_filter;
		private GCHandle h_filter;
		private GCHandle h_temp;
		
        #endregion
        
		#region Constructor

		public Filter(ref DSPBuffer dsp_buffer_obj)
        {
            this.d = dsp_buffer_obj;
            this.s = d.State;
                    	
        	this.ovlp_cpx = new CPX[s.DSPBlockSize];
        	this.filter_cpx = new CPX[s.DSPBlockSize * 2];
            this.tmp_cpx = new CPX[s.DSPBlockSize * 2];
        	this.filter_designer = new FilterDesigner(); 
        	
        	h_filter = GCHandle.Alloc(filter_cpx, GCHandleType.Pinned);
        	h_temp = GCHandle.Alloc(tmp_cpx, GCHandleType.Pinned);
        	
        	// in: tmp_cpx out: filter_cpx
            plan_fwd_filter = FFTW.dft_1d(s.DSPBlockSize * 2,
			                              h_temp.AddrOfPinnedObject(),
			                              h_filter.AddrOfPinnedObject(),
			                              FFTW.fftw_direction.Forward,
			                         	  FFTW.fftw_flags.Measure);
        	
        }
				
        #endregion
        
        #region Public Methods		
        
        public void Exit()
        {
        	FFTW.destroy_plan(plan_fwd_filter);
			h_filter.Free();
			h_temp.Free();
        }
        
        public void MakeFilter()
        					   
        {   
        	ClearFilter();
        	ClearTmp();
        	switch (this.FilterType)
            {   
            	case FilterType_e.LowPass:
            		this.filter_designer.MakeFirLowpass(this.FilterFrequencyHigh, 
            		                                    s.DSPSampleRate, 
            		                                    this.FilterWindowType, 
            		                                    ref tmp_cpx, 
            		                                    s.DSPBlockSize + 1);            		
            		DoFFTForwardFilter();
            		break;
                case FilterType_e.BandPass:
                    this.filter_designer.MakeFirBandpass(this.FilterFrequencyLow, 
                                                         this.FilterFrequencyHigh, 
                                                         s.DSPSampleRate, 
                                                         this.FilterWindowType, 
                                                         ref tmp_cpx, 
                                                         s.DSPBlockSize + 1);                    
                    DoFFTForwardFilter();
                    break;                
                default:
                    break;
            }
        }
        
        internal void DoFFTForwardFilter()
		{
			FFTW.execute(plan_fwd_filter);
		}
        
        internal void ClearFilter()
        {
        	Array.Clear(this.filter_cpx, 0, this.filter_cpx.Length);
        }
        
        internal void ClearTmp()
        {
        	Array.Clear(this.tmp_cpx, 0, this.tmp_cpx.Length);
        }
        
        public void Process()
        {
        	d.DoFFTForwardMain(); // in: cpx buffer, out: tmp_cpx_1 buffer
        	
        	DoConvolution(); // in: tmp_cpx_1 buffer, out: tmp_cpx_2 buffer

            d.DoFFTReverseMain(); // in: tmp_cpx_2 buffer, out: tmp_cpx_3 buffer

            d.NormalizeTemp3(); // in: tmp_cpx_3 buffer, out: tmp_cpx_3 buffer
        	
        	DoOverlapAdd(); // tmp_cpx_3 buffer, out: cpx buffer
        } 
        
        #endregion
        
        #region Private Methods        
                        
        private void DoConvolution()
        {
            int size = s.DSPBlockSize * 2;
            
        	for (int i = 0; i < size; i++)  //convolution in frequency here
            {
        		d.tmp_cpx_2[i].real = (filter_cpx[i].real * d.tmp_cpx_1[i].real) - (filter_cpx[i].imag * d.tmp_cpx_1[i].imag);
                d.tmp_cpx_2[i].imag = (filter_cpx[i].real * d.tmp_cpx_1[i].imag) + (filter_cpx[i].imag * d.tmp_cpx_1[i].real);                  
            }        	
        }       
                       
        private void DoOverlapAdd()
        {   
        	int size = s.DSPBlockSize;
                    	
        	for (int i = 0; i < size; i++)
        	{
				d.cpx[i] = d.tmp_cpx_3[i] + this.ovlp_cpx[i];
				this.ovlp_cpx[i] = d.tmp_cpx_3[i + size];
        	}
        }
        
        #endregion                

        #region Properties

        private float freq_lo = 10.0f;
        public float FilterFrequencyLow
        {
            get { return freq_lo; }
            set 
            { 
                freq_lo = value;
                MakeFilter();
            }
        }
        private float freq_hi = 3000.0f;
        public float FilterFrequencyHigh
        {
            get { return freq_hi; }
            set 
            { 
                freq_hi = value;
                MakeFilter();
            }
        }
        private FilterType_e filter_type = FilterType_e.BandPass;
        public FilterType_e FilterType
        {
            get { return filter_type; }
            set { filter_type = value; }
        }
        private WindowType_e window_type = WindowType_e.BLACKMANHARRIS_WINDOW;
        public WindowType_e FilterWindowType
        {
            get { return window_type; }
            set { window_type = value; }
        }

        #endregion

    }
}
