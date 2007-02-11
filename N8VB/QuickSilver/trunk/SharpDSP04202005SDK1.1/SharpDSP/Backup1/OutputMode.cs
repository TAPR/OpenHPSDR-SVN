//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General public License
//==============================================================
//OutputMode.cs
//handles writing to the output buffers based on mode
//
//==============================================================

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace SharpDSP
{
	[Serializable()]
    public struct OutputMode
    {
        #region Private members

        private int size;

        #endregion

        #region Constructor

        public OutputMode(int size)
        {
            this.size = size;
        }

        #endregion

        #region Public Methods

		unsafe public void DoOutput(float* in_real, float* in_imag, float* out_l, float* out_r,
			Receiver rx)
		{				
			switch (rx.ReceiverOutputRoute)
			{
				case RXOutputRoute.Both:
					DoOutputBoth(in_real, in_imag, out_l, out_r, rx);
					break;
				case RXOutputRoute.Right:
					DoOutputRight(in_real, in_imag, out_l, out_r, rx);
					break;
				case RXOutputRoute.Left:
					DoOutputLeft(in_real, in_imag, out_l, out_r, rx);
					break;
				case RXOutputRoute.Mixer:
					DoOutputMix(in_real, in_imag, out_l, out_r, rx);
					break;
			}			
		}

		#endregion

		#region Private members

        unsafe private void DoOutputBoth(float* in_real, float* in_imag, float* out_l, float* out_r,
			Receiver rx)
		{
			float* bufferMag = stackalloc float[this.size];

			switch (rx.CurrentMode)
			{
				case Mode.AM:

					this.GetMagnitude(in_real, in_imag, bufferMag);
									
					Memory.ScaleFloatMemory(bufferMag, rx.VolumeLeft, this.size);
					Memory.CopyFloatMemory(out_l, bufferMag, this.size);
					Memory.ScaleFloatMemory(bufferMag, rx.VolumeRight, this.size);
					Memory.CopyFloatMemory(out_r, bufferMag, this.size);	
					
					break;

				case Mode.SAM:

					rx.am_pll.DoPLL(in_real, in_imag, bufferMag);

					Memory.ScaleFloatMemory(bufferMag, rx.VolumeLeft, this.size);
					Memory.CopyFloatMemory(out_l, bufferMag, this.size);
					Memory.ScaleFloatMemory(bufferMag, rx.VolumeRight, this.size);
					Memory.CopyFloatMemory(out_r, bufferMag, this.size);	
					
					break;

				case Mode.FMN:

					rx.fm_pll.DoPLL(in_real, in_imag, bufferMag);

					Memory.ScaleFloatMemory(bufferMag, rx.VolumeLeft, this.size);
					Memory.CopyFloatMemory(out_l, bufferMag, this.size);
					Memory.ScaleFloatMemory(bufferMag, rx.VolumeRight, this.size);
					Memory.CopyFloatMemory(out_r, bufferMag, this.size);	
					
					break;

				case Mode.DSB:
				case Mode.USB:
				case Mode.LSB:
				case Mode.CWL:
				case Mode.CWU:

					if (rx.NRSwitchValue)
					{
						rx.lms_ANR.DoLMSFilter(in_real);
					}
					if (rx.ANFSwitchValue)
					{
						rx.lms_ANF.DoLMSFilter(in_real);
					}

					if (rx.BinauralMode)
					{
						Memory.ScaleFloatMemory(in_real, rx.VolumeLeft, this.size);
						Memory.ScaleFloatMemory(in_imag, rx.VolumeRight, this.size);
						Memory.CopyFloatMemory(out_l, in_real, this.size);
						Memory.CopyFloatMemory(out_r, in_imag, this.size);						
					}
					else
					{
						Memory.ScaleFloatMemory(in_real, rx.VolumeLeft, this.size);
						Memory.CopyFloatMemory(out_l, in_real, this.size);
						Memory.ScaleFloatMemory(in_real, rx.VolumeRight, this.size);
						Memory.CopyFloatMemory(out_r, in_real, this.size);						
					}
					
					break;

				case Mode.SPEC:
				default:

					Memory.ScaleFloatMemory(in_real, rx.VolumeLeft, this.size);
					Memory.ScaleFloatMemory(in_imag, rx.VolumeRight, this.size);
					Memory.CopyFloatMemory(out_l, in_real, this.size);
					Memory.CopyFloatMemory(out_r, in_imag, this.size);	

					break;
			}

		}

		unsafe private void DoOutputRight(float* in_real, float* in_imag, float* out_l, float* out_r,
			Receiver rx)
		{
			float* bufferMag = stackalloc float[this.size];

			switch (rx.CurrentMode)
			{
				case Mode.AM:

					this.GetMagnitude(in_real, in_imag, bufferMag);
										
					Memory.ScaleFloatMemory(bufferMag, rx.VolumeRight, this.size);
					Memory.CopyFloatMemory(out_r, bufferMag, this.size);					
         
					break;

				case Mode.SAM:

					rx.am_pll.DoPLL(in_real, in_imag, bufferMag);

					Memory.ScaleFloatMemory(bufferMag, rx.VolumeRight, this.size);
					Memory.CopyFloatMemory(out_r, bufferMag, this.size);
			
					break;

				case Mode.FMN:

					rx.fm_pll.DoPLL(in_real, in_imag, bufferMag);

					Memory.ScaleFloatMemory(bufferMag, rx.VolumeRight, this.size);
					Memory.CopyFloatMemory(out_r, bufferMag, this.size);
			
					break;

				case Mode.DSB:
				case Mode.USB:
				case Mode.LSB:
				case Mode.CWL:
				case Mode.CWU:

					if (rx.NRSwitchValue)
					{
						rx.lms_ANR.DoLMSFilter(in_real);
					}
					if (rx.ANFSwitchValue)
					{
						rx.lms_ANF.DoLMSFilter(in_real);
					}

					Memory.ScaleFloatMemory(in_imag, rx.VolumeRight, this.size);
					Memory.CopyFloatMemory(out_r, in_imag, this.size);			

					break;

				case Mode.SPEC:
				default:

					Memory.ScaleFloatMemory(in_imag, rx.VolumeRight, this.size);
					Memory.CopyFloatMemory(out_r, in_imag, this.size);	
		
					break;
			}
		}
		
		unsafe private void DoOutputLeft(float* in_real, float* in_imag, float* out_l, float* out_r,
			Receiver rx)
		{
			float* bufferMag = stackalloc float[this.size];

			switch (rx.CurrentMode)
			{
				case Mode.AM:

					this.GetMagnitude(in_real, in_imag, bufferMag);
									
					Memory.ScaleFloatMemory(bufferMag, rx.VolumeLeft, this.size);
					Memory.CopyFloatMemory(out_l, bufferMag, this.size);	
             
					break;

				case Mode.SAM:

					rx.am_pll.DoPLL(in_real, in_imag, bufferMag);

					Memory.ScaleFloatMemory(bufferMag, rx.VolumeLeft, this.size);
					Memory.CopyFloatMemory(out_l, bufferMag, this.size);	

					break;

				case Mode.FMN:

					rx.fm_pll.DoPLL(in_real, in_imag, bufferMag);

					Memory.ScaleFloatMemory(bufferMag, rx.VolumeLeft, this.size);
					Memory.CopyFloatMemory(out_l, bufferMag, this.size);	

					break;

				case Mode.DSB:
				case Mode.USB:
				case Mode.LSB:
				case Mode.CWL:
				case Mode.CWU:

					if (rx.NRSwitchValue)
					{
						rx.lms_ANR.DoLMSFilter(in_real);
					}
					if (rx.ANFSwitchValue)
					{
						rx.lms_ANF.DoLMSFilter(in_real);
					}

					Memory.ScaleFloatMemory(in_real, rx.VolumeLeft, this.size);
					Memory.CopyFloatMemory(out_l, in_real, this.size);

					break;

				case Mode.SPEC:
				default:

					Memory.ScaleFloatMemory(in_real, rx.VolumeLeft, this.size);
					Memory.CopyFloatMemory(out_l, in_real, this.size);

					break;
			}
		}

		unsafe private void DoOutputMix(float* in_real, float* in_imag, float* out_l, float* out_r,
			Receiver rx)
		{
			float* bufferMag = stackalloc float[this.size];

			switch (rx.CurrentMode)
			{
				case Mode.AM:

					this.GetMagnitude(in_real, in_imag, bufferMag);
										
					AudioMixer(bufferMag, bufferMag, out_l, out_r, rx.VolumeLeft, rx.VolumeRight);
					
					break;

				case Mode.SAM:

					rx.am_pll.DoPLL(in_real, in_imag, bufferMag);
					
					AudioMixer(bufferMag, bufferMag, out_l, out_r, rx.VolumeLeft, rx.VolumeRight);

					break;

				case Mode.FMN:

					rx.fm_pll.DoPLL(in_real, in_imag, bufferMag);
					
					AudioMixer(bufferMag, bufferMag, out_l, out_r, rx.VolumeLeft, rx.VolumeRight);

					break;

				case Mode.DSB:
				case Mode.USB:
				case Mode.LSB:
				case Mode.CWL:
				case Mode.CWU:

					if (rx.NRSwitchValue)
					{
						rx.lms_ANR.DoLMSFilter(in_real);
					}
					if (rx.ANFSwitchValue)
					{
						rx.lms_ANF.DoLMSFilter(in_real);
					}

					if (rx.BinauralMode)
					{
						AudioMixer(in_real, in_imag, out_l, out_r, rx.VolumeLeft, rx.VolumeRight);
					}
					else
					{
						AudioMixer(in_real, in_real, out_l, out_r, rx.VolumeLeft, rx.VolumeRight);
					}
					
					break;

				case Mode.SPEC:
				default:

					AudioMixer(in_real, in_imag, out_l, out_r, rx.VolumeLeft, rx.VolumeRight);

					break;
			}

		}

        unsafe private void GetMagnitude(float* x, float* y, float* mag)
        {
            for (int i = 0; i < this.size; i++)
            {
                mag[i] = (float)(Math.Sqrt(Math.Pow(x[i], 2) + (Math.Pow(y[i], 2))));
            }
        }

		unsafe private void AudioMixer(float *in_l, float *in_r, float *out_l, float *out_r, float level_l, float level_r)
		{						
			for (int i = 0; i < this.size; i++)
			{
				out_l[i] = out_l[i] * level_l + in_l[i] * level_l;
				out_r[i] = out_r[i] * level_r + in_r[i] * level_r;
			}
		}

        #endregion
    }

}