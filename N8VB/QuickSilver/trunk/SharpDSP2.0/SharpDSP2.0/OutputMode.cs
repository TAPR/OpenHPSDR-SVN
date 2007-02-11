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

        public void DoOutput(ref double[] in_real, 
                             ref double[] in_imag, 
                             ref double[] out_l, 
                             ref double[] out_r,
                             Receiver rx)
		{				
			switch (rx.ReceiverOutputRoute)
			{
				case RXOutputRoute.Both:
					DoOutputBoth(ref in_real, ref in_imag, ref out_l, ref out_r, rx);
					break;
				case RXOutputRoute.Right:
					DoOutputRight(ref in_real, ref in_imag, ref out_l, ref out_r, rx);
					break;
				case RXOutputRoute.Left:
					DoOutputLeft(ref in_real, ref in_imag, ref out_l, ref out_r, rx);
					break;
				case RXOutputRoute.Mixer:
					DoOutputMix(ref in_real, ref in_imag, ref out_l, ref out_r, rx);
					break;
			}			
		}

		#endregion

		#region Private members

		private void DoOutputBoth(ref double[] in_real, 
		                          ref double[] in_imag, 
		                          ref double[] out_l, 
		                          ref double[] out_r,
		                          Receiver rx)
		{
			double[] bufferMag = new double[this.size];

			switch (rx.CurrentMode)
			{
				case Mode.AM:

					this.GetMagnitude(ref in_real, ref in_imag, ref bufferMag);
									
					MemoryOps.ScaleFloatMemory(ref bufferMag, rx.VolumeLeft, this.size);
					MemoryOps.CopyFloatMemory(ref out_l, ref bufferMag, this.size);
					MemoryOps.ScaleFloatMemory(ref bufferMag, rx.VolumeRight, this.size);
					MemoryOps.CopyFloatMemory(ref out_r, ref bufferMag, this.size);	
					
					break;

				case Mode.SAM:

					rx.am_pll.DoPLL(ref in_real, ref in_imag, ref bufferMag);

					MemoryOps.ScaleFloatMemory(ref bufferMag, rx.VolumeLeft, this.size);
					MemoryOps.CopyFloatMemory(ref out_l, ref bufferMag, this.size);
					MemoryOps.ScaleFloatMemory(ref bufferMag, rx.VolumeRight, this.size);
					MemoryOps.CopyFloatMemory(ref out_r, ref bufferMag, this.size);	
					
					break;

				case Mode.FMN:

					rx.fm_pll.DoPLL(ref in_real, ref in_imag, ref bufferMag);

					MemoryOps.ScaleFloatMemory(ref bufferMag, rx.VolumeLeft, this.size);
					MemoryOps.CopyFloatMemory(ref out_l, ref bufferMag, this.size);
					MemoryOps.ScaleFloatMemory(ref bufferMag, rx.VolumeRight, this.size);
					MemoryOps.CopyFloatMemory(ref out_r, ref bufferMag, this.size);	
					
					break;

				case Mode.DSB:
				case Mode.USB:
				case Mode.LSB:
				case Mode.CWL:
				case Mode.CWU:

					if (rx.NRSwitchValue)
					{
						rx.lms_ANR.DoLMSFilter(ref in_real);
					}
					if (rx.ANFSwitchValue)
					{
						rx.lms_ANF.DoLMSFilter(ref in_real);
					}

					if (rx.BinauralMode)
					{
						MemoryOps.ScaleFloatMemory(ref in_real, rx.VolumeLeft, this.size);
						MemoryOps.ScaleFloatMemory(ref in_imag, rx.VolumeRight, this.size);
						MemoryOps.CopyFloatMemory(ref out_l, ref in_real, this.size);
						MemoryOps.CopyFloatMemory(ref out_r, ref in_imag, this.size);						
					}
					else
					{
						MemoryOps.ScaleFloatMemory(ref in_real, rx.VolumeLeft, this.size);
						MemoryOps.CopyFloatMemory(ref out_l, ref in_real, this.size);
						MemoryOps.ScaleFloatMemory(ref in_real, rx.VolumeRight, this.size);
						MemoryOps.CopyFloatMemory(ref out_r, ref in_real, this.size);						
					}
					
					break;

				case Mode.SPEC:
				default:

					MemoryOps.ScaleFloatMemory(ref in_real, rx.VolumeLeft, this.size);
					MemoryOps.ScaleFloatMemory(ref in_imag, rx.VolumeRight, this.size);
					MemoryOps.CopyFloatMemory(ref out_l, ref in_real, this.size);
					MemoryOps.CopyFloatMemory(ref out_r, ref in_imag, this.size);	

					break;
			}

		}

		private void DoOutputRight(ref double[] in_real,
		                           ref double[] in_imag,
		                           ref double[] out_l,
		                           ref double[] out_r,
			Receiver rx)
		{
			double[] bufferMag = new double[this.size];

			switch (rx.CurrentMode)
			{
				case Mode.AM:

					this.GetMagnitude(ref in_real, ref in_imag, ref bufferMag);
										
					MemoryOps.ScaleFloatMemory(ref bufferMag, rx.VolumeRight, this.size);
					MemoryOps.CopyFloatMemory(ref out_r, ref bufferMag, this.size);					
         
					break;

				case Mode.SAM:

					rx.am_pll.DoPLL(ref in_real, ref in_imag, ref bufferMag);

					MemoryOps.ScaleFloatMemory(ref bufferMag, rx.VolumeRight, this.size);
					MemoryOps.CopyFloatMemory(ref out_r, ref bufferMag, this.size);
			
					break;

				case Mode.FMN:

					rx.fm_pll.DoPLL(ref in_real, ref in_imag, ref bufferMag);

					MemoryOps.ScaleFloatMemory(ref bufferMag, rx.VolumeRight, this.size);
					MemoryOps.CopyFloatMemory(ref out_r, ref bufferMag, this.size);
			
					break;

				case Mode.DSB:
				case Mode.USB:
				case Mode.LSB:
				case Mode.CWL:
				case Mode.CWU:

					if (rx.NRSwitchValue)
					{
						rx.lms_ANR.DoLMSFilter(ref in_real);
					}
					if (rx.ANFSwitchValue)
					{
						rx.lms_ANF.DoLMSFilter(ref in_real);
					}

					MemoryOps.ScaleFloatMemory(ref in_imag, rx.VolumeRight, this.size);
					MemoryOps.CopyFloatMemory(ref out_r, ref in_imag, this.size);			

					break;

				case Mode.SPEC:
				default:

					MemoryOps.ScaleFloatMemory(ref in_imag, rx.VolumeRight, this.size);
					MemoryOps.CopyFloatMemory(ref out_r, ref in_imag, this.size);	
		
					break;
			}
		}
		
		private void DoOutputLeft(ref double[] in_real, 
		                          ref double[] in_imag, 
		                          ref double[] out_l, 
		                          ref double[] out_r,
		                          Receiver rx)
		{
			double[] bufferMag = new double[this.size];

			switch (rx.CurrentMode)
			{
				case Mode.AM:

					this.GetMagnitude(ref in_real, ref in_imag, ref bufferMag);
									
					MemoryOps.ScaleFloatMemory(ref bufferMag, rx.VolumeLeft, this.size);
					MemoryOps.CopyFloatMemory(ref out_l, ref bufferMag, this.size);	
             
					break;

				case Mode.SAM:

					rx.am_pll.DoPLL(ref in_real, ref in_imag, ref bufferMag);

					MemoryOps.ScaleFloatMemory(ref bufferMag, rx.VolumeLeft, this.size);
					MemoryOps.CopyFloatMemory(ref out_l, ref bufferMag, this.size);	

					break;

				case Mode.FMN:

					rx.fm_pll.DoPLL(ref in_real, ref in_imag, ref bufferMag);

					MemoryOps.ScaleFloatMemory(ref bufferMag, rx.VolumeLeft, this.size);
					MemoryOps.CopyFloatMemory(ref out_l, ref bufferMag, this.size);	

					break;

				case Mode.DSB:
				case Mode.USB:
				case Mode.LSB:
				case Mode.CWL:
				case Mode.CWU:

					if (rx.NRSwitchValue)
					{
						rx.lms_ANR.DoLMSFilter(ref in_real);
					}
					if (rx.ANFSwitchValue)
					{
						rx.lms_ANF.DoLMSFilter(ref in_real);
					}

					MemoryOps.ScaleFloatMemory(ref in_real, rx.VolumeLeft, this.size);
					MemoryOps.CopyFloatMemory(ref out_l, ref in_real, this.size);

					break;

				case Mode.SPEC:
				default:

					MemoryOps.ScaleFloatMemory(ref in_real, rx.VolumeLeft, this.size);
					MemoryOps.CopyFloatMemory(ref out_l, ref in_real, this.size);

					break;
			}
		}

		private void DoOutputMix(ref double[] in_real,
		                         ref double[] in_imag,
		                         ref double[] out_l,
		                         ref double[] out_r,
		                         Receiver rx)
		{
			double[] bufferMag = new double[this.size];

			switch (rx.CurrentMode)
			{
				case Mode.AM:

					this.GetMagnitude(ref in_real, ref in_imag, ref bufferMag);
										
					AudioMixer(ref bufferMag, ref bufferMag, ref out_l, ref out_r, rx.VolumeLeft, rx.VolumeRight);
					
					break;

				case Mode.SAM:

					rx.am_pll.DoPLL(ref in_real, ref in_imag, ref bufferMag);
					
					AudioMixer(ref bufferMag, ref bufferMag, ref out_l, ref out_r, rx.VolumeLeft, rx.VolumeRight);

					break;

				case Mode.FMN:

					rx.fm_pll.DoPLL(ref in_real, ref in_imag, ref bufferMag);
					
					AudioMixer(ref bufferMag, ref bufferMag, ref out_l, ref out_r, rx.VolumeLeft, rx.VolumeRight);

					break;

				case Mode.DSB:
				case Mode.USB:
				case Mode.LSB:
				case Mode.CWL:
				case Mode.CWU:

					if (rx.NRSwitchValue)
					{
						rx.lms_ANR.DoLMSFilter(ref in_real);
					}
					if (rx.ANFSwitchValue)
					{
						rx.lms_ANF.DoLMSFilter(ref in_real);
					}

					if (rx.BinauralMode)
					{
						AudioMixer(ref in_real, ref in_imag, ref out_l, ref out_r, rx.VolumeLeft, rx.VolumeRight);
					}
					else
					{
						AudioMixer(ref in_real, ref in_real, ref out_l, ref out_r, rx.VolumeLeft, rx.VolumeRight);
					}
					
					break;

				case Mode.SPEC:
				default:

					AudioMixer(ref in_real, ref in_imag, ref out_l, ref out_r, rx.VolumeLeft, rx.VolumeRight);

					break;
			}

		}

		private void GetMagnitude(ref double[] x,
		                          ref double[] y,
		                          ref double[] mag)
        {
            for (int i = 0; i < this.size; i++)
            {
                mag[i] = (double)(Math.Sqrt(Math.Pow(x[i], 2) + (Math.Pow(y[i], 2))));
            }
        }

		private void AudioMixer(ref double[] in_l,
		                        ref double[] in_r,
		                        ref double[] out_l,
		                        ref double[] out_r,
		                        double level_l, 
		                        double level_r)
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
