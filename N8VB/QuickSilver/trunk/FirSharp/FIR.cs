using System;
using Win32;

namespace FirSharp
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class FIR
	{
		static double fir_basic(double input, ref double[] h, ref double[] z)       
		{
			double accum;
		
			/* store input at the beginning of the delay line */
			z[0] = input;

			/* calc FIR */
			accum = 0;
			for (int i=0; i < h.Length; i++) 
			{
				accum += h[i] * z[i];
			}

			/* shift delay line */
			for (int i=h.Length - 2; i >= 0; i--) 
			{
				z[i + 1] = z[i];
			}

			return accum;
		} 
		
		static double fir_double_z(double input, ref double[] h, ref double[] z, ref int state)
		{
			double accum;
			
			/* store input at the beginning of the delay line as well as ntaps more */
			z[state] = z[state + h.Length] = input;

			/* calculate the filter */
			accum = 0;
			for (int i=0; i < h.Length; i++) 
			{
				accum += h[i] * z[i + state];
			}

			/* decrement state, wrapping if below zero */
			if (--state < 0) 
			{
				state += h.Length;
			}
			
			return accum;
		}

		static double fir_split(double input, ref double[] h, ref double[] z, ref int state)
		{
			double accum;

			/* setup the filter */
			accum = 0;
			
			z[0] = input;

			/* calculate the end part */
			int end_ntaps = h.Length - state;
			for (int i = 0; i < end_ntaps; i++) 
			{
				accum += h[i] * z[i + state];
			}

			/* calculate the beginning part */
			for (int i = 0; i < state; i++) 
			{
				accum += h[i] * z[i];
			}

			/* decrement the state, wrapping if below zero */
			if (--state < 0) 
			{
				state += h.Length;
			}			

			return accum;
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//double[] h = new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };

			//double[] h = FirSharp.CFIR_coeff_64.h;
			double[] h = FirSharp.CFIR_coeff_89.h;
			
			Console.WriteLine("Tap Count = " + h.Length);
			
			Win32.HiPerfTimer hpt = new HiPerfTimer();

			/* make impulse input signal */
			double[] imp = new double[2048];
			imp[5] = 1.0;
			
			/* test FIR algorithms */

			Console.WriteLine("Testing fir_basic:");
			double[] z = new double[h.Length];
			double[] output = new double[imp.Length];

			hpt.Start();
			for (int i=0; i < imp.Length; i++) 
			{
				output[i] = fir_basic(imp[i], ref h, ref z);				
			}

			hpt.Stop();
			Console.WriteLine(hpt.Duration);
//			for (int i=0; i < imp.Length; i++)
//			{
//				Console.Write(" " + output[i].ToString("f") + " , ");
//			}
            			
			Console.WriteLine();
			Console.WriteLine();

			Console.WriteLine("Testing fir_double_z:");
			z = new double[h.Length * 2];
			int state = 0;

			hpt.Start();
			for (int i=0; i < imp.Length; i++) 
			{
				output[i] = fir_double_z(imp[i], ref h, ref z, ref state);				
			}

			hpt.Stop();
			Console.WriteLine(hpt.Duration);

//			for (int i=0; i < imp.Length; i++)
//			{
//				Console.Write(" " + output[i].ToString("f") + " , ");
//			}

			Console.WriteLine();
			Console.WriteLine();

			Console.WriteLine("Testing fir_split:");
			z = new double[h.Length * 2];
			state = 0;

			hpt.Start();
			for (int i=0; i < imp.Length; i++) 
			{
				output[i] = fir_split(imp[i], ref h, ref z, ref state);				
			}

			hpt.Stop();
			Console.WriteLine(hpt.Duration);

			Console.Read();
			
		}
	}
}
