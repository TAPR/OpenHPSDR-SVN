//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General Public License
//==============================================================
//transmitter.cs
//implements transmitter function for SDR
//==============================================================

using System;
using System.Runtime.Serialization;

namespace SharpDSP
{
	[Serializable()]
	public class Transmitter
	{
		public Transmitter()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		unsafe public void ProcessAudioStreamTX(float* input_l, float* input_r, float* output_l, float* output_r, int numsamples)
		{
//			if (numsamples != DSPstate.BlockSize) { return; }
//
//			float* buffer_l = stackalloc float[DSPstate.FFTSize];
//			float* buffer_r = stackalloc float[DSPstate.FFTSize];
//		
//			//Make sure there is no garbage in the upper half of the buffers, zero them out
//			//This is not needed in .NET 2.0 since stackalloc zeros the buffers when initialized
//			for (int i = DSPstate.FFTSize / 2; i < DSPstate.FFTSize; i++)
//			{
//				buffer_l[i] = 0.0F;
//				buffer_r[i] = 0.0F;
//			}
//
//			//TX CODE GOES HERE

		}

		//public int ChangeTXOsc(double freq)
			//        {
			//            DSPstate.TXFreqOscValue = (float)(freq);
			//            return 0;
			//        }

		public void SetTXScale(double setit)
		{
		}
     

        

       

			//        public void ChangeTXAGC(int setit)
			//        {
			//            
			//        }
			//
			//        public void ChangeTXAGCLimit(int setit)
			//        {
			//            
			//        }
			//
			//        public void ChangeTXAGCFF(int setit)
			//        {
			//            DSPstate.TXAGCOffValue = setit;
			//        }
			//
			//        public void ChangeTXAGCCompression(double txcompression)
			//        {
			//            DSPstate.TXAGCCompressionValue = txcompression;
			//        }
			//
			//        public void ChangeTXAGCFFCompression(double txcompression)
			//        {
			//            DSPstate.TXAGCFFCompressionValue = txcompression;
			//        }
			//
			//        public void ChangeTXAGCHang(int txhang)
			//        {
			//            
			//        }

			//        public void SetTXPreScale(double scale)
			//        {
			//            
			//        }

			//        public void SetTXEQ(int[] txeq)
			//        {
			//
			//        }

        

        

       

        

			
        
                
		//        public void SetTRX(State.TransmitState trx_on)
		//        {
		//            DSPstate.TRXState = (State.TransmitState)trx_on;
		//        }

       
		
		

		public void ChangeTXFilters(int low, int high)
		{
		//			switch (DSPstate.RXTXMode)
		//			{
		//				case State.Mode.LSB:
		//				case State.Mode.CWL:
		//					DSPstate.TXFilterLowCut = -high;
		//					DSPstate.TXFilterHighCut = -low;
		//					break;
		//				case State.Mode.USB:
		//				case State.Mode.CWU:
		//					DSPstate.TXFilterLowCut = low;
		//					DSPstate.TXFilterHighCut = high;
		//					break;
		//				case State.Mode.DSB:
		//					DSPstate.TXFilterLowCut = -high;
		//					DSPstate.TXFilterHighCut = high;
		//					break;
		//				case State.Mode.AM:
		//				case State.Mode.SAM:
		//					DSPstate.TXFilterLowCut = -high;
		//					DSPstate.TXFilterHighCut = high;
		//					break;
		//				case State.Mode.FMN:
		//					DSPstate.TXFilterLowCut = -6000;
		//					DSPstate.TXFilterHighCut = 6000;
		//					break;
		//			}
		//			UpdateTXDisplayVars();
		//
		//			int i = ChangeFilter(DSPstate.TXFilterLowCut, DSPstate.TXFilterHighCut, DSPstate.FilterTaps, State.TransmitState.ON);
		//
		}

		

		public void UpdateTXDisplayVars()
		{
			//			if (DSPstate.TXFilterLowCut < 0 && DSPstate.TXFilterHighCut <= 0)
			//			{
			//				DSPstate.TXDisplayHigh = 0;
			//				if (DSPstate.TXFilterLowCut >= -910)
			//					DSPstate.TXDisplayLow = -1000;
			//				else
			//					DSPstate.TXDisplayLow = (int)(DSPstate.TXFilterLowCut * 1.1);
			//			}
			//			else if (DSPstate.TXFilterLowCut >= 0 && DSPstate.TXFilterHighCut > 0)
			//			{
			//				DSPstate.TXDisplayLow = 0;
			//				if (DSPstate.TXFilterHighCut <= 910)
			//					DSPstate.TXDisplayHigh = 1000;
			//				else
			//					DSPstate.TXDisplayHigh = (int)(DSPstate.TXFilterLowCut * 1.1);
			//			}
			//			else if (DSPstate.TXFilterLowCut < 0 && DSPstate.TXFilterHighCut > 0)
			//			{
			//				int max_edge = Math.Max(-DSPstate.TXFilterLowCut, DSPstate.TXFilterHighCut);
			//				DSPstate.TXDisplayLow = (int)(max_edge * -1.1);
			//				DSPstate.TXDisplayHigh = (int)(max_edge * 1.1);
			//			}
		}

//		public void ChangeFilter(double low, double high, int taps, DSP.TransmitState trx_on)
//		{
//			filter.MakeFilter((float)low, (float)high, DSPState.SampleRate, Filter.FilterType.BandPass, (FilterDesigner.WindowType)DSPState.WindowType);
//			DSPState.LowFilter = (float)low;
//			DSPState.HighFilter = (float)high;
//		}

	}
}
