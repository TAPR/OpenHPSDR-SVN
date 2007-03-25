using System;
using System.Threading; 
using System.Diagnostics;
using PowerSDR; 

namespace JanusTester
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class MainClass
	{


		public static int CallbackCount = 0; 
		public static int SampleRate = 48000; 
		public static double CosPhase = 0; 
		public static int OscFreq = 22450; 
		public static readonly int BlockSize = 2048;
		public static double AmplitudeScale = 1.0; 

		public static float[] save_buf_r1 = new float[BlockSize]; 
		public static float[] save_buf_r2 = new float[BlockSize]; 
		public static float[] save_buf_l1 = new float[BlockSize]; 
		public static float[] save_buf_l2 = new float[BlockSize]; 


		public static readonly int[] TestFreqs = { 200, 1000, 3000, 9000, 12000, 18000, 22000 }; 

		[STAThread]
		static void Main(string[] args)
		{
			Process p = Process.GetCurrentProcess();
			p.PriorityClass = ProcessPriorityClass.RealTime;

			System.Console.WriteLine("Janus Tester"); 
			do  
			{ 
				for ( int j = 0; j < TestFreqs.Length; j++ ) 
				{ 
					float ampl = 1.0F; 
					for ( int i = 0; i < 10; i++ ) 
					{ 
						doTest(48000, TestFreqs[j], ampl); 
						ampl -= 0.1F; 
					}				
					ampl = 1.0F; 
					for ( int i = 0; i < 10; i++ ) 
					{ 
						doTest(96000, TestFreqs[j], ampl); 
						ampl -= 0.1F; 
					}				
					ampl = 1.0F; 
					for ( int i = 0; i < 10; i++ ) 
					{ 
						doTest(192000, TestFreqs[j], ampl); 
						ampl -= 0.1F; 
					}				
				}
			} while ( false ); 
		}

		public static readonly double VppToRmsConversionFactor = (Math.Sqrt(2.0)/4.0); 

		public static void doTest(int sample_rate, int freq, float amplitude_scale) 
		{ 
			SampleRate = sample_rate; 
			OscFreq = freq; 
			AmplitudeScale = amplitude_scale; 
			CosPhase = 0; 
			CallbackCount = 0; 
			System.Console.WriteLine("-----------------------------------------------------"); 
			System.Console.WriteLine("Sample Rate: " + SampleRate + " Freq: " + OscFreq + " Amplitude: " + AmplitudeScale); 
			TFDAPHaudio.StartAudio(SampleRate, BlockSize, CallBack, 24, 0); 			
			while ( CallbackCount < 100 ) 
			{
				Thread.Sleep(10); 
			}
			System.Console.WriteLine("Stoppping audio..."); 
			TFDAPHaudio.StopAudio(); 
			// ok dump out the received data 
#if false
			for ( int i = 0; i < BlockSize; i++ ) 
			{
				System.Console.WriteLine(save_buf_l1[i] + " " + save_buf_r1[i] + " " + save_buf_l2[i] + " " + save_buf_r2[i]); 
			}		
#endif 

			float freq_l1 = getFreqFromBuf(save_buf_l1, BlockSize, SampleRate); 
			float freq_r1 = getFreqFromBuf(save_buf_r1, BlockSize, SampleRate); 
			System.Console.WriteLine("freq left: "  + freq_l1 + " right: " + freq_r1); 
			System.Console.WriteLine("freq % diff left: " + percentDiff((float)OscFreq, freq_l1)  + " right: " + 
									percentDiff((float)OscFreq, freq_r1));
			float rms_l1 = getRmsFromBuf(save_buf_l1, BlockSize); 
			float rms_r1 = getRmsFromBuf(save_buf_r1, BlockSize); 
			System.Console.WriteLine("RMS left: " + rms_l1 + " right: " + rms_r1); 
			float max_l1 = getMaxFromBuf(save_buf_l1, BlockSize); 
			float max_r1 = getMaxFromBuf(save_buf_r1, BlockSize); 
			System.Console.WriteLine("Max left: " + max_l1 + " right: " + max_r1); 
			float min_l1 = getMinFromBuf(save_buf_l1, BlockSize); 
			float min_r1 = getMinFromBuf(save_buf_r1, BlockSize); 
			System.Console.WriteLine("Min left: " + min_l1 + " right: " + min_r1); 
			float vpp_l1 = max_l1 - min_l1; 
			float vpp_r1 = max_r1 - min_r1; 
			float rms_frompp_l1 = (float)(VppToRmsConversionFactor * vpp_l1); 
			float rms_frompp_r1 = (float)(VppToRmsConversionFactor * vpp_r1); 
			System.Console.WriteLine("RMS from Vpp left: " + rms_frompp_l1 + " right: " + rms_frompp_r1); 

			float avg_sum_lr = getAvgSumFrom2Bufs(save_buf_l1, save_buf_l2, BlockSize); 
			System.Console.WriteLine("Avg L+R: " + avg_sum_lr); 


			float freq_l2 = getFreqFromBuf(save_buf_l2, BlockSize, SampleRate); 
			float rms_l2 = getRmsFromBuf(save_buf_l1, BlockSize); 
			float max_l2 = getMaxFromBuf(save_buf_l1, BlockSize); 
			float min_l2 = getMinFromBuf(save_buf_l1, BlockSize); 
			float vpp_l2 = max_l2 - min_l2; 
			float rms_frompp_l2 = (float)(VppToRmsConversionFactor * vpp_l2); 
			System.Console.WriteLine("freq left2: "  + freq_l2); 
			System.Console.WriteLine("freq % diff left: " + percentDiff((float)OscFreq, freq_l2) );
			System.Console.WriteLine("RMS left2: " + rms_l2 ); 
			System.Console.WriteLine("Max left2: " + max_l1 ); 
			System.Console.WriteLine("Min left2: " + min_l1 ); 
			System.Console.WriteLine("RMS from Vpp left2: " + rms_frompp_l2); 

			System.Console.Write("Result: "); 
			System.Console.Write(sample_rate + "," + freq + "," + amplitude_scale + ","); 
			System.Console.Write(freq_l1 + "," +  freq_r1 + "," + freq_l2 + ","); 
			System.Console.WriteLine(rms_l1 + "," + rms_r1 + "," + rms_l2);
				
			
		} 

		public static float percentDiff(float nominal, float measured) 
		{
			float result; 
			result = (  measured - nominal ) / nominal; 
			result = result * 100; 
			return result; 
		}
		

		public static float getMaxFromBuf(float[] buf, int num_samples) 
		{ 
			float max = -9999999.0F; 
			for ( int i = 0; i < num_samples; i++ ) 
			{ 
				if ( buf[i] > max ) 
				{
					max = buf[i]; 
				}
			} 
			return max; 
		}


		public static float getMinFromBuf(float[] buf, int num_samples) 
		{ 
			float min = 9999999.0F; 
			for ( int i = 0; i < num_samples; i++ ) 
			{ 
				if ( buf[i] < min ) 
				{
					min = buf[i]; 
				}
			} 
			return min; 
		}


		public static float getRmsFromBuf(float[] buf, int num_samples) 
		{
			double sum = 0; 
			for ( int i = 0; i < num_samples; i++ ) 
			{ 
				sum += (buf[i] * buf[i]);
			}
			sum = sum / (double)num_samples;
			sum = Math.Sqrt(sum); 
			return (float)sum; 
		}

		public static float getAvgSumFrom2Bufs(float[] buf1, float[] buf2, int num_samples) 
		{
			double sum = 0; 
			for ( int i = 0; i < num_samples; i++ ) 
			{
				sum += buf1[i] + buf2[i]; 
			}
			sum = sum / (double)num_samples; 
			return (float)sum; 
		}

		public static float getFreqFromBuf(float[] buf, int num_samples, int sample_rate) 
		{ 
			int num_sign_changes = 0; 
			int first_change_idx = num_samples; 
			int last_change_idx = 0; 			

			for ( int i = 0; i < num_samples-1; i++ ) 
			{ 
				if ( buf[i] < 0  && buf[i+1] >= 0 )  // positive going sign change 
				{
					++num_sign_changes; 
					// System.Console.WriteLine("pos @: " + i); 
					if ( i < first_change_idx ) 
					{
						first_change_idx = i; 
					}
					if ( i > last_change_idx ) 
					{
						last_change_idx = i; 
					}
				}
			} 
			// System.Console.WriteLine("num_sign_changes: " + num_sign_changes); 
			// System.Console.WriteLine("1stChangeIDX: " + first_change_idx + " lastIdx: " + last_change_idx); 

			float result = (float)( (num_sign_changes-1) * sample_rate); 
			result = result / (float)(last_change_idx - first_change_idx);
			return result; 
		} 


		unsafe private static PA19.PaStreamCallback CallBack = new PA19.PaStreamCallback(EchoCallback4Port);	   


		unsafe public static double CosGen(float *buf, int samples, double phase, double freq) 
		{ 
			double phase_step = freq/SampleRate*2*Math.PI;
			double cosval = Math.Cos(phase);
			double sinval = Math.Sin(phase);
			double cosdelta = Math.Cos(phase_step);
			double sindelta = Math.Sin(phase_step);
			double tmpval;

			for(int i=0; i<samples; i++ )
			{
				tmpval = cosval*cosdelta - sinval*sindelta;
				sinval = cosval*sindelta + sinval*cosdelta;
				cosval = tmpval;
						
				buf[i] = (float)(AmplitudeScale * cosval);
				// System.Console.WriteLine("cos: " + buf[i]); 
											 
				phase += phase_step;
			}
			return phase;
		}


		unsafe public static int EchoCallback4Port(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData) 
		{ 
			++CallbackCount; 


			float* in_l = null, in_r = null;
			float* out_l1 = null, out_r1 = null, out_l2 = null, out_r2 = null;

			int* array_ptr = (int *)input;
			float* in_l_ptr1 = (float *)array_ptr[0];
			float* in_r_ptr1 = (float *)array_ptr[1];
			float* in_l_ptr2 = (float *)array_ptr[2];
			float* in_r_ptr2 = (float *)array_ptr[3];
			array_ptr = (int *)output;
			float* out_l_ptr1 = (float *)array_ptr[0];
			float* out_r_ptr1 = (float *)array_ptr[1];
			float* out_l_ptr2 = (float *)array_ptr[2];
			float* out_r_ptr2 = (float *)array_ptr[3];

			if ( CallbackCount == 50 )  // is this the frame we want to look at  
			{ 
				for ( int j = 0; j < frameCount; j++ ) 
				{
					save_buf_r1[j] = in_r_ptr1[j]; 
					save_buf_r2[j] = in_r_ptr2[j]; 
					save_buf_l1[j] = in_l_ptr1[j]; 
					save_buf_l2[j] = in_l_ptr2[j]; 
				}
			} 

			CosPhase = CosGen(out_l_ptr1, frameCount, CosPhase, OscFreq); 
			for ( int i = 0; i < frameCount; i++ ) 
			{ 
				out_r_ptr1[i] = -out_l_ptr1[i];
				out_r_ptr2[i] = (float)(0.0286 * out_l_ptr1[i]);
				out_l_ptr2[i] = out_r_ptr2[i]; 
			} 
			return 0;
		}
	}
}
