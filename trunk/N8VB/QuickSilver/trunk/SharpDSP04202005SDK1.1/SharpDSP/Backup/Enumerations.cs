//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General internal License
//==============================================================
//enumerations.cs
//
//enumerations for DSP objects
//
//==============================================================

using System;

namespace SharpDSP
{	
	#region Enumerations

	public enum Mode : int
	{
		FIRST = -1,
		LSB,
		USB,
		DSB,
		CWL,
		CWU,
		FMN,
		AM,
		PSK,
		SPEC,
		RTTY,
		SAM,
		DRM,
		LAST,
	}

	public enum MeterType
	{
		SIGNAL_STRENGTH,
		AVG_SIGNAL_STRENGTH,
		ADC_REAL,
		ADC_IMAG,
		AGC_GAIN,
		ALC,
		PWR,
		PKPWR
	}
				
	public enum Window
	{
		FIRST = -1,
		RECTANGULAR,
		HANNING,
		WELCH,
		PARZEN,
		BARTLETT,
		HAMMING,
		BLACKMAN2,
		BLACKMAN3,
		BLACKMAN4,
		EXPONENTIAL,
		RIEMANN,
		BLKHARRIS,
		LAST,
	}

	public enum RXType
	{
		MainRX,
		SubRX
	}

	public enum RXOutputRoute
	{
		None,
		Both,
		Left,
		Right,
		Mixer
	}

	public enum AGCType : int
	{
		agcOff,
		agcLong,
		agcSlow,
		agcMedium,
		agcFast
	}

	public enum FilterType : int
	{
		LowPass,
		BandPass
	}

	public enum WindowType : int
	{
		RECTANGULAR_WINDOW,
		HANNING_WINDOW,
		WELCH_WINDOW,
		PARZEN_WINDOW,
		BARTLETT_WINDOW,
		HAMMING_WINDOW,
		BLACKMAN2_WINDOW,
		BLACKMAN3_WINDOW,
		BLACKMAN4_WINDOW,
		EXPONENTIAL_WINDOW,
		RIEMANN_WINDOW,
		BLACKMANHARRIS_WINDOW,
	}

	public enum LMSFilterType
	{
		Interference,
		Noise
	}
	
	public enum MeterPosition : int
	{
		PREFILTER,
		POSTFILTER
	}

	public enum PowerSpectrumMode : int
	{
		SPECTRUM,
		PANADAPTER,
		SCOPE,
		PHASE,
		PHASE2,
		WATERFALL,
		HISTOGRAM,
		OFF,
		SPECMODE
	}

	public enum PowerSpectrumPosition : int
	{
		INITIAL,
		PREFILTER,
		POSTFILTER,           
		POSTAGC,
		FINAL
	}

	#endregion

}