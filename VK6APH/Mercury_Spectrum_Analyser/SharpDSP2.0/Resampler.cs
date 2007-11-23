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
			
			filter_des.MakeFirLowpass(0.45f, 
			                          interpolation_factor, 
			                          SharpDSP.WindowType.HANNING_WINDOW, 
			                          ref taps, 
			                          taps.Length);
			
		}
		
		public void Resample(ref float[] input_data, ref float[] output_data)
		{
			output_samples_count = 0;
			
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
