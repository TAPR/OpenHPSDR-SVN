/*
 * Created by SharpDevelop.
 * User: Phil C
 * Date: 1/24/2007
 * Time: 5:16 PM
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
	 * Filter Length     : 77
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
	 * Stopband Atten.    : 80 dB
	 *
	 * Measurements
	 * Sampling Frequency : N/A (normalized frequency)
	 * Passband Edge      : 0.25
	 * 3-dB Point         : 0.28841
	 * 6-dB Point         : 0.29698
	 * Stopband Edge      : 0.35
	 * Passband Ripple    : 0.048693 dB
	 * Stopband Atten.    : 80.2057 dB
	 * Transition Width   : 0.1
	 *
	 * Implementation Cost
	 * Number of Multipliers : 77
	 * Number of Adders      : 76
	 * Number of States      : 76
	 * MultPerInputSample    : 77
	 * AddPerInputSample     : 76
	 */
	
	public class CFIR_coeff_77
	{
		public CFIR_coeff_77()
		{
		}
		
		static public int TapCount()
		{
			return h.Length;
		}
		
		public static double[] h = new double[]
		{
		0.0001987063420842,0.0003952529336641,0.0004149102871244,4.454470505849e-006,
		-0.0007924167769868,-0.001481027829309,-0.001354410465989,-0.0001086918886669,
		0.001628974918735, 0.002466909301393, 0.001267787366067,-0.001637710645742,
		-0.004178007401492,-0.003853128965243,5.204487602354e-005, 0.005168752230839,
		0.007220891455564, 0.003379063168124,-0.004670832695477, -0.01090028688616,
		-0.009160242030481, 0.001263097380147,  0.01346251575764,  0.01700947518762,
		0.006336745233942, -0.01296787966886, -0.02609442851061, -0.01958753820034,
		0.006458265122918,   0.0347016651856,  0.04063941497563,  0.01146307738683,
		-0.0395895812208, -0.07579796791987, -0.05801788809022,  0.02796977975091,
		0.156527497628,   0.2717892969552,   0.3179504500256,   0.2717892969552,
		0.156527497628,  0.02796977975091, -0.05801788809022, -0.07579796791987,
		-0.0395895812208,  0.01146307738683,  0.04063941497563,   0.0347016651856,
		0.006458265122918, -0.01958753820034, -0.02609442851061, -0.01296787966886,
		0.006336745233942,  0.01700947518762,  0.01346251575764, 0.001263097380147,
		-0.009160242030481, -0.01090028688616,-0.004670832695477, 0.003379063168124,
		0.007220891455564, 0.005168752230839,5.204487602354e-005,-0.003853128965243,
		-0.004178007401492,-0.001637710645742, 0.001267787366067, 0.002466909301393,
		0.001628974918735,-0.0001086918886669,-0.001354410465989,-0.001481027829309,
		-0.0007924167769868,4.454470505849e-006,0.0004149102871244,0.0003952529336641,
		0.0001987063420842
		};	
	}
}
