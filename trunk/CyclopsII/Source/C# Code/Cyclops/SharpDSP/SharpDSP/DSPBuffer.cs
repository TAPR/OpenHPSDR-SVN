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

#define WIN32

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SharpDSP2._1
{		
		
	#region FFTW 
	internal class FFTW
	{
		private const string library_name = "libfftw3f.dll";		
		
		internal enum fftw_direction : int
		{
			Forward=-1,
			Backward=1
		}		
		
		internal enum fftw_flags : uint
		{		
			Measure=0,		
			DestroyInput=1,		
			Unaligned=2,		
			ConserveMemory=4,		
			Exhaustive=8,		
			PreserveInput=16,		
			Patient=32,		
			Estimate=64
		}
		
		[DllImport(library_name,
		EntryPoint = "fftwf_destroy_plan",
		ExactSpelling = true),
		System.Security.SuppressUnmanagedCodeSecurity]
		internal static extern void destroy_plan(IntPtr plan);
		
		[DllImport(library_name,
		EntryPoint = "fftwf_cleanup",
		ExactSpelling = true),
		System.Security.SuppressUnmanagedCodeSecurity]
		internal static extern void cleanup();
		
		[DllImport(library_name,
		EntryPoint = "fftwf_execute",
		ExactSpelling = true),
		System.Security.SuppressUnmanagedCodeSecurity]
		internal static extern void execute(IntPtr plan);
		
		[DllImport(library_name,
		EntryPoint = "fftwf_plan_dft_1d",
		ExactSpelling = true),
		System.Security.SuppressUnmanagedCodeSecurity]
		internal static extern IntPtr dft_1d(int n, IntPtr input, IntPtr output, 
		fftw_direction direction, fftw_flags flags);
	}
	
	#endregion
	
	/// <summary>
	/// The DSP Buffer
	/// </summary>
	
	[Serializable()]
	public class DSPBuffer
	{
		#region Public Members		
		public DSPState State;
		public CPX[] cpx;
		#endregion
		
		#region Internal Members
		
		internal CPX[] tmp_cpx_1; // used as a working buffer
		internal CPX[] tmp_cpx_2; // primarily stores fft temporary results
		internal CPX[] tmp_cpx_3; // used to assemble output buffer 
		internal CPX[] dec_cpx; // downsampled buffer

        //internal System.Collections.Queue qreal;
        //internal System.Collections.Queue qimag;

        internal RingBuffer audio_ring_buffer;        

		#endregion
		
		#region Private Members
		
		private IntPtr plan_fwd_main;
		private IntPtr plan_rev_main;
		private GCHandle h_cpx;
		private GCHandle h_tmp1;
		private GCHandle h_tmp2;
		private GCHandle h_tmp3;
		
		private int size;
		private int halfsize;
		private int leftside;
		private int rightside;
		private float scalefactor;
		
		private int[] stage;
		private int index;
		
		#endregion
		
		#region Constructor
		
		public DSPBuffer(DSPState state)
		{
            this.State = state;

            size = this.State.DSPBlockSize * 2;

            audio_ring_buffer = new RingBuffer(size * 8);

			cpx = new CPX[size];
			tmp_cpx_1 = new CPX[size];
			tmp_cpx_2 = new CPX[size];
			tmp_cpx_3 = new CPX[size];
			dec_cpx = new CPX[size/2];
			
			halfsize = size/2;
			leftside = 0;
			rightside = halfsize; 
			scalefactor = (float)1.0f/size;
			
			h_cpx = GCHandle.Alloc(cpx, GCHandleType.Pinned);
			h_tmp1 = GCHandle.Alloc(tmp_cpx_1, GCHandleType.Pinned);
			h_tmp2 = GCHandle.Alloc(tmp_cpx_2, GCHandleType.Pinned);
			h_tmp3 = GCHandle.Alloc(tmp_cpx_3, GCHandleType.Pinned);
			
			// in: cpx out: tmp_cpx_1
			plan_fwd_main = FFTW.dft_1d(size,
			                         	h_cpx.AddrOfPinnedObject(),
			                         	h_tmp1.AddrOfPinnedObject(),
			                         	FFTW.fftw_direction.Forward,
			                         	FFTW.fftw_flags.Measure);
			
			// in: tmp_cpx_2 out: tmp_cpx_3
			plan_rev_main = FFTW.dft_1d(size,
			                         	h_tmp2.AddrOfPinnedObject(),
			                         	h_tmp3.AddrOfPinnedObject(),
			                         	FFTW.fftw_direction.Backward,
			                         	FFTW.fftw_flags.Measure);			
			
			this.stage = new int[State.DSPBlockSize];
			this.index = 0;
			
			for (int i = 0; i < State.OutputRateDivisor; i++)
			{
				stage[i] = (int)((double)State.DSPBlockSize * ((double)i/(double)State.OutputRateDivisor));				
			}
			
		}
		
		public void Exit() // destructor
		{
			FFTW.destroy_plan(plan_fwd_main);
			FFTW.destroy_plan(plan_rev_main);
			h_cpx.Free();
			h_tmp1.Free();
			h_tmp2.Free();
			h_tmp3.Free();						
		}
		
		#endregion
		
		#region Public Methods
		
		public void ClearAll()
		{
			Array.Clear(cpx, 0, size);
			Array.Clear(tmp_cpx_1, 0, size);
			Array.Clear(tmp_cpx_2, 0, size);
			Array.Clear(tmp_cpx_3, 0, size);
			Array.Clear(dec_cpx, 0, size/2);
		}

        public void ClearCpxTop()
        {
            Array.Clear(cpx, halfsize, halfsize);
        }

		public float GetMagnitude(int index)
		{
            return (float)Math.Sqrt(cpx[index].real *
                                    cpx[index].real +
                                    cpx[index].imag *
                                    cpx[index].imag);

            //if (Math.Abs(cpx[index].real) > Math.Abs(cpx[index].imag))
            //    return (float)(Math.Abs(cpx[index].real) + 0.4 * Math.Abs(cpx[index].imag));
            //else
            //    return (float)(Math.Abs(cpx[index].imag) + 0.4 * Math.Abs(cpx[index].real));
		}

        public float GetBlockPeakMagnitude(int size)
        {
            float mag = 0.0f;

            for (int i = 0; i < size; i++)
            {
                float tmp = GetMagnitude(i);
                if (tmp >= mag)
                    mag = tmp; 
            }

            return mag;
        }

        public float GetBlockPeakMagnitudeWithGain(float gain, int size)
        {
            float mag = 0.0f;

            for (int i = 0; i < size; i++)
            {
                float tmp = GetMagnitude(i) * gain;
                if (tmp >= mag)
                    mag = tmp;
            }

            return mag;
        }

        public float GetBlockAvgMagnitudeWithGain(float gain, int size)
        {
            float mag = 0.0f;

            for (int i = 0; i < size; i++)
            {
                mag += (GetMagnitude(i) * gain);                
            }
            return mag/size;
        }

        public float GetPower(int index)
		{
			return cpx[index].real * cpx[index].real + cpx[index].imag * cpx[index].imag;
		}
		
		public float GetPowerTemp(int index)
		{
			return tmp_cpx_1[index].real * tmp_cpx_1[index].real + tmp_cpx_1[index].imag * tmp_cpx_1[index].imag;
		}
						
		public void CopyImagToReal()
		{
			for (int i = 0; i < halfsize; i++)
			{
				cpx[i].real = cpx[i].imag;
			}
		}
				
		public void Scale(float rscaling, float iscaling)
		{
			for (int i = 0; i < halfsize; i++)
			{
				cpx[i].real *= rscaling;
				cpx[i].imag *= iscaling;
			}
		}
										
		public void Fill(ref CPX[] cpxbuf)
		{
			Array.Copy(cpxbuf, cpx, halfsize);						
		}
		
		#endregion
		
		#region Internal Methods
		
		internal void DoFFTForwardMain()
		{
			FFTW.execute(plan_fwd_main);
		}
						
		internal void DoFFTReverseMain()
		{
			FFTW.execute(plan_rev_main);			
		}
											
		internal void NormalizeCPX()
		{
			for (int i = 0; i < size; i++)
			{
				cpx[i].real *= this.scalefactor;
				cpx[i].imag *= this.scalefactor;
			}
		}
		
		internal void NormalizeTemp1()
		{
			for (int i = 0; i < size; i++)
			{
				tmp_cpx_1[i].real *= this.scalefactor;
				tmp_cpx_1[i].imag *= this.scalefactor;
			}
		}
		
		internal void NormalizeTemp2()
		{
			for (int i = 0; i < size; i++)
			{
				tmp_cpx_2[i].real *= this.scalefactor;
				tmp_cpx_2[i].imag *= this.scalefactor;
			}
		}
		
		internal void NormalizeTemp3()
		{
			for (int i = 0; i < size; i++)
			{
				tmp_cpx_3[i].real *= this.scalefactor;
				tmp_cpx_3[i].imag *= this.scalefactor;
			}
		}
		
		internal void ClearTemp1All()
		{
			Array.Clear(tmp_cpx_1, 0, size);
		}			
						
		internal void CopyTemp1ToMain()
		{
			Array.Copy(tmp_cpx_1, cpx, size);
		}	
		
		internal void CopyTemp1BotToMainBot()
		{
			Array.Copy(tmp_cpx_1, cpx, halfsize);
		}		
		
		internal void ClearTemp2()
		{
			Array.Clear(tmp_cpx_2, 0, size);
		}			
		
		internal void CopyTemp2ToMain()
		{
			Array.Copy(tmp_cpx_2, cpx, size);
		}		
		
		internal void ClearTemp3()
		{
			Array.Clear(tmp_cpx_3, 0, size);
		}			
		
		internal void ClearTemp3Top()
		{
			Array.Clear(tmp_cpx_3, halfsize , halfsize);
		}		
		
		internal void ClearTemp3Bottom()
		{
			Array.Clear(tmp_cpx_3, 0, halfsize);
		}		
		
		internal void CopyTemp3ToMain()
		{
			Array.Copy(tmp_cpx_3, cpx, size);
		}	
		
		internal void CopyTemp2ToTemp3()
		{
			Array.Copy(tmp_cpx_2, tmp_cpx_3, size);
		}					
		
		internal void CopyMainLeftToTemp3Right()
		{
			Array.Copy(cpx,
			           leftside,
			           tmp_cpx_3,
			           rightside,
			           halfsize);
		}
		
		internal void CopyTemp3LeftToMainLeft()
		{
			Array.Copy(tmp_cpx_3,
			           leftside,
			           cpx,
			           leftside,
			           halfsize);
		}
		
		internal void CopyTemp3RightToTemp3Left()
		{
			Array.Copy(tmp_cpx_3,
			           rightside,
			           tmp_cpx_3,
			           leftside,
			           halfsize);
		}
		
		internal float GetMagnitudeTemp1(int index)
		{
			return (float)Math.Sqrt(tmp_cpx_1[index].real * 
			                        tmp_cpx_1[index].real + 
			                        tmp_cpx_1[index].imag * 
			                        tmp_cpx_1[index].imag);
		}
						
		internal void ScaleTemp1(float rscaling, float iscaling)
		{
			for (int i = 0; i < halfsize; i++)
			{
				tmp_cpx_1[i].real *= rscaling;
				tmp_cpx_1[i].imag *= iscaling;
			}
		}
		
		internal void ScaleTemp1(float rscaling, float iscaling, int index)
		{			
			tmp_cpx_1[index].real *= rscaling;
			tmp_cpx_1[index].imag *= iscaling;
		}
		
		internal bool DownSampleMain()
		{
			for (int i = 0, j = stage[this.index]; i < State.DSPBlockSize; i+=State.OutputRateDivisor, j++)
			{
				dec_cpx[j] = cpx[i];
			}				
			if (this.index == (State.OutputRateDivisor - 1))
			{
				this.index = 0;
				return true;
			} 
			else 
			{ 
				this.index++;
				return false;
			}
		}
		
		#endregion
	}
}
