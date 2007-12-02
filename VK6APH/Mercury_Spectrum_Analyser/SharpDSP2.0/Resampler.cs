//==============================================================
// Copyright (C) 2007 Philip A Covington, N8VB
//
// This software is licensed under the GNU General public License
//==============================================================
// Resampler.cs
//
//
//
//==============================================================

using System;

namespace SharpDSP
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class Resampler
	{
		private int output_samples_count;
		private int filter_memory_buffer_index;
		private SharpDSP.FilterDesigner filter_des = new FilterDesigner();
		private double[] taps;
		private int num_taps = 512;
		private double[] filter_memory;
		private int mask;
		private int filter_phase = 0;
		private int interpolation_factor = 1;
		private int decimation_factor = 1;
		private int output_sample_count = 0;
		
		public Resampler()
		{
			taps = new double[num_taps];
			filter_memory = new double[num_taps];
			mask = filter_memory.Length - 1;
			
			filter_des.MakeFirLowpass( 0.4f, //0.45f, 
			                          interpolation_factor, 
			                          SharpDSP.WindowType.BLACKMANHARRIS_WINDOW, 
			                          ref taps, 
			                          taps.Length);

            //System.Console.WriteLine("Taps:");
            //for (int i = 0; i < taps.Length; i++)
            //{
            //    System.Console.WriteLine("tap[" + i + "]: " + taps[i]); 
            //} 
		}


        public Resampler(float cutoff_freq)
        {
            taps = new double[num_taps];
            filter_memory = new double[num_taps];
            mask = filter_memory.Length - 1;

            filter_des.MakeFirLowpass(cutoff_freq,
                                      interpolation_factor,
                                      SharpDSP.WindowType.BLACKMANHARRIS_WINDOW, 
                                      ref taps,
                                      taps.Length);

            //System.Console.WriteLine("Taps:");
            //for (int i = 0; i < taps.Length; i++)
            //{
            //    System.Console.WriteLine("tap[" + i + "]: " + taps[i]);
            //}
        }



        //int debug_mask = 0; 

        public void doFilter(ref float[] input_data, ref float[] output_data)
        {

            //if (debug_mask == 0)
            //{
            //    System.Console.WriteLine("doFilter");
            //    for (int i = 0; i < input_data.Length; i++)
            //    {
            //        System.Console.WriteLine("input_data[" + i + "]: " + input_data[i]); 
            //    } 
            //} 

            for (int ii = 0; ii < output_data.Length; ii++)
            {
                output_data[ii] = 0.0f; 
            }
            for (int i = 0; i < input_data.Length; i++) // over all the input samples 
            {
                filter_memory[filter_memory_buffer_index] = input_data[i];
                int j = filter_memory_buffer_index;
                filter_memory_buffer_index = ( filter_memory_buffer_index + 1 ) & mask;

                            

                for (int tap = 0; tap < num_taps; tap++)
                {
#if false
                    if (debug_mask == 0)
                    {
                        System.Console.WriteLine("itapj: " + i + " " + tap + " " + j); 
                    } 
#endif
                    output_data[i] += (float)(taps[tap] * filter_memory[j]);
                    --j;
                    if (j < 0)
                    {
                        j = num_taps - 1; 
                    }
                } 
            }

            //if (debug_mask == 0)
            //{
            //    // System.Console.WriteLine("doFilter");
            //    for (int i = 0; i < output_data.Length; i++)
            //    {
            //        System.Console.WriteLine("output_data[" + i + "]: " + output_data[i]);
            //    }
            //} 

            //++debug_mask;
            //if (debug_mask == 100)
            //{
            //    debug_mask = 0; 
            //} 
        }


        public void Resample(ref float[] input_data, ref float[] output_data)
		{
			output_samples_count = 0;

            for (int ii = 0; ii < output_data.Length; ii++)
            {
                output_data[ii] = 0.0f;
            } 
			
			for (int i=0; i< input_data.Length; i++)
			{
				filter_memory[filter_memory_buffer_index] = input_data[i];
				int j = filter_memory_buffer_index;
				filter_memory_buffer_index = (filter_memory_buffer_index + 1) & mask;
				
				while (filter_phase < interpolation_factor)
				{
					for (int k=filter_phase; k<num_taps; k+=interpolation_factor)
					{
						output_data[j] = (float)(filter_memory[j] * taps[k]);
						j = (j+mask) & mask;
					}
					output_data[j] *= (float)interpolation_factor;
					output_samples_count += 1;
					filter_phase += decimation_factor;
				}
				filter_phase -= interpolation_factor;
			}
		}
	}
}
