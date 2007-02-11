/*
 * Created by SharpDevelop.
 * User: Phil C
 * Date: 1/24/2007
 * Time: 5:27 PM
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
	 * Filter Length     : 65
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
	 * Passband Ripple    : 0.05 dB
	 * Stopband Atten.    : 60 dB
	 *
	 * Measurements
	 * Sampling Frequency : N/A (normalized frequency)
	 * Passband Edge      : 0.25
	 * 3-dB Point         : 0.29351
	 * 6-dB Point         : 0.30286
	 * Stopband Edge      : 0.35
	 * Passband Ripple    : 0.047173 dB
	 * Stopband Atten.    : 60.4844 dB
	 * Transition Width   : 0.1
	 *
	 * Implementation Cost
	 * Number of Multipliers : 65
	 * Number of Adders      : 64
	 * Number of States      : 64
	 * MultPerInputSample    : 65
	 * AddPerInputSample     : 64
	 */
	
	public class CFIR_coeff_64
	{
		public CFIR_coeff_64()
		{
		}
		static public int TapCount()
		{
			return h.Length;
		}
		
		public static double[] h = new double[]
		{
		-0.0007250568016306,-0.0001929452241053, 0.000760292479592,   0.0018283833145,
		0.001860430321085,0.0001911851640272,-0.002347027343774,-0.003586175129253,
		-0.00171975676326, 0.002653861206756, 0.006162334692599, 0.005008704641415,
		-0.001378637273422,-0.008638145384814, -0.01007776623282,-0.002419966863956,
		0.009945452458603,  0.01677736370266, 0.009934626045776,-0.008314734805823,
		-0.02433792384583, -0.02247863378816, 0.001079658051101,   0.0312993049264,
		0.0422446043729,  0.01696272800158,  -0.0347011308858,  -0.0756897380671,
		-0.06293171381736,  0.02204791525321,   0.1545523871505,   0.2754634769572,
		0.3242423834671,   0.2754634769572,   0.1545523871505,  0.02204791525321,
		-0.06293171381736,  -0.0756897380671,  -0.0347011308858,  0.01696272800158,
		0.0422446043729,   0.0312993049264, 0.001079658051101, -0.02247863378816,
		-0.02433792384583,-0.008314734805823, 0.009934626045776,  0.01677736370266,
		0.009945452458603,-0.002419966863956, -0.01007776623282,-0.008638145384814,
		-0.001378637273422, 0.005008704641415, 0.006162334692599, 0.002653861206756,
		-0.00171975676326,-0.003586175129253,-0.002347027343774,0.0001911851640272,
		0.001860430321085,   0.0018283833145, 0.000760292479592,-0.0001929452241053,
		-0.0007250568016306
		};	
	
	}
}
