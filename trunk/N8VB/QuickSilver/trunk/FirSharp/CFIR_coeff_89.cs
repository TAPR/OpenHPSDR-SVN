/*
 * Created by SharpDevelop.
 * User: Phil C
 * Date: 1/24/2007
 * Time: 6:15 PM
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
	 * Filter Length     : 89
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
	 * Stopband Atten.    : 100 dB
	 *
	 * Measurements
	 * Sampling Frequency : N/A (normalized frequency)
	 * Passband Edge      : 0.25
	 * 3-dB Point         : 0.28471
	 * 6-dB Point         : 0.29262
	 * Stopband Edge      : 0.35
	 * Passband Ripple    : 0.048058 dB
	 * Stopband Atten.    : 100.2633 dB
	 * Transition Width   : 0.1
	 *
	 * Implementation Cost
	 * Number of Multipliers : 89
	 * Number of Adders      : 88
	 * Number of States      : 88
	 * MultPerInputSample    : 89
	 * AddPerInputSample     : 88
	 */
 
	public class CFIR_coeff_89
	{
		public CFIR_coeff_89()
		{
		}
		static public int TapCount()
		{
			return h.Length;
		}
		
		public static double[] h = new double[]
		{
		-5.027798956595e-005,-0.0001819021991621,-0.0003859555069292,-0.000548092433418,
		-0.0004789658811995,-5.175069366339e-005,0.0006197703187262, 0.001130156022877,
		0.0009937834889426,5.577530512006e-005,-0.001201413398229,-0.001809006062694,
		-0.0009878730701498, 0.001066177293589, 0.002984709679164, 0.003031729414149,
		0.0005374271657375,-0.003138219111166,-0.005155986379464,-0.003257906972799,
		0.00205118355991, 0.007129992988049, 0.007505062407543, 0.001566167566492,
		-0.007239333150844, -0.01205942896924,-0.007704271008504, 0.004421339299263,
		0.01584884275769,  0.01656182055937, 0.003217321899689, -0.01631322662177,
		-0.02683665292242, -0.01689355708971,  0.01058022961457,  0.03692198772583,
		0.03904072886185, 0.007225092490825, -0.04303987119421, -0.07556409682071,
		-0.05416681712038,  0.03238120907277,   0.1579216521516,   0.2690178155848,
		0.3132679793271,   0.2690178155848,   0.1579216521516,  0.03238120907277,
		-0.05416681712038, -0.07556409682071, -0.04303987119421, 0.007225092490825,
		0.03904072886185,  0.03692198772583,  0.01058022961457, -0.01689355708971,
		-0.02683665292242, -0.01631322662177, 0.003217321899689,  0.01656182055937,
		0.01584884275769, 0.004421339299263,-0.007704271008504, -0.01205942896924,
		-0.007239333150844, 0.001566167566492, 0.007505062407543, 0.007129992988049,
		0.00205118355991,-0.003257906972799,-0.005155986379464,-0.003138219111166,
		0.0005374271657375, 0.003031729414149, 0.002984709679164, 0.001066177293589,
		-0.0009878730701498,-0.001809006062694,-0.001201413398229,5.577530512006e-005,
		0.0009937834889426, 0.001130156022877,0.0006197703187262,-5.175069366339e-005,
		-0.0004789658811995,-0.000548092433418,-0.0003859555069292,-0.0001819021991621,
		-5.027798956595e-005
		};	
	}
}
