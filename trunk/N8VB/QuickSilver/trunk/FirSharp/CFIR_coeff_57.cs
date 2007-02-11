/*
 * Created by SharpDevelop.
 * User: Phil C
 * Date: 1/24/2007
 * Time: 4:57 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace FirSharp
{
	/*
	 * Discrete-Time FIR Filter (real)
	 * -------------------------------
	 * Filter Structure  : Direct-Form FIR
	 * Filter Length     : 57
	 * Stable            : Yes
	 * Linear Phase      : Yes (Type 1)
	 *
	 * Design Options
	 * DensityFactor : 16
	 * MinOrder      : any
	 * MinPhase      : false
	 * StopbandDecay : 0
	 * StopbandShape : flat
	 *
	 * Design Specifications
	 * Sampling Frequency : N/A (normalized frequency)
	 * Response           : CIC Compensator
	 * Specification      : Fp,Fst,Ap,Ast
	 * NumberOfSections   : 7
	 * DifferentialDelay  : 1
	 * Passband Edge      : 0.25
	 * Stopband Edge      : 0.35
	 * Passband Ripple    : 1 dB
	 * Stopband Atten.    : 80 dB
	 *
	 * Measurements
	 * Sampling Frequency : N/A (normalized frequency)
	 * Passband Edge      : 0.25
	 * 3-dB Point         : 0.27501
	 * 6-dB Point         : 0.28473
	 * Stopband Edge      : 0.35
	 * Passband Ripple    : 0.87792 dB
	 * Stopband Atten.    : 81.104 dB
	 * Transition Width   : 0.1
	 *
	 * Implementation Cost
	 * Number of Multipliers : 57
	 * Number of Adders      : 56
	 * Number of States      : 56
	 * MultPerInputSample    : 57
	 * AddPerInputSample     : 56
	 */
 	
	public class CFIR_coeff_57
	{
		
		public static double[] h = new double[] 
		{
			0.0004242991519433, 0.001774518442746, 0.004255885085231, 0.007354993348068,
			0.009388853231236, 0.008415948760944, 0.003510966934543, -0.00369157189124,
			-0.009234945116456,-0.008881585281162,-0.001510676570509, 0.008973500367994,
			0.01503906874728,   0.0105525942704,-0.003855758664136,  -0.0192297357036,
			-0.02319972655072,-0.009073570235231,  0.01697313752144,   0.0370930578752,
			0.03260235653691,-0.001347238339109, -0.04756946599427,  -0.0726739300367,
			-0.04583457836133,  0.04017560170695,   0.1595064620354,   0.2631792380391,
			0.3041642368546,   0.2631792380391,   0.1595064620354,  0.04017560170695,
			-0.04583457836133,  -0.0726739300367, -0.04756946599427,-0.001347238339109,
			0.03260235653691,   0.0370930578752,  0.01697313752144,-0.009073570235231,
			-0.02319972655072,  -0.0192297357036,-0.003855758664136,   0.0105525942704,
			0.01503906874728, 0.008973500367994,-0.001510676570509,-0.008881585281162,
			-0.009234945116456, -0.00369157189124, 0.003510966934543, 0.008415948760944,
			0.009388853231236, 0.007354993348068, 0.004255885085231, 0.001774518442746,
			0.0004242991519433
		};
		public CFIR_coeff_57()
		{
		}
		static public int TapCount()
		{
			return h.Length;
		}
	}
}
	
	

