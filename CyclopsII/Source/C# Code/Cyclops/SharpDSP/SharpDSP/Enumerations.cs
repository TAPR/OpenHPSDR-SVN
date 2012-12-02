/*
This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2007, 2008 Philip A Covington

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

The author can be reached by email at

p.covington@gmail.com

*/

using System;

namespace SharpDSP2._1
{
	/// <summary>
	/// Enumerations for SharpDSP2
	/// </summary>
	#region Enumerations

	public enum DSPMode_e : int
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
		
	public enum MeterType_e
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
	
	public enum MeterPosition_e : int
	{
		PREFILTER,
		POSTFILTER
	}
				
	public enum Window_e
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

	public enum RXType_e
	{
		MainRX,
		SubRX
	}

	public enum RXOutputRoute_e
	{
		None,
		Both,
		Left,
		Right,
		Mixer
	}

    public enum AGCMethod : int
    {
        AGCHang,    // original DoAGC
        AGCNoHang,  // DoAGC2
        AGCHarman   // DoHarmanAGC
    }

    [Flags]
    public enum AGC_Status : int
    {
        None = 0,
        Fast_hang = 1,
        Hang = 2,
        OverFastThreshold = 4,
        OverThreshold = 8
    }

    public enum AGCType_e : int
	{
		agcOff,
		agcLong,
		agcSlow,
		agcMedium,
		agcFast,
        agcUser
	}

	public enum FilterType_e : int
	{
		LowPass,
		BandPass,
		ALLPASS,		
	}

	public enum WindowType_e : int
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
		
	public enum PowerSpectrumMode_e : int
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

	public enum PowerSpectrumPosition_e : int
	{
		INITIAL,
		PREFILTER,
		POSTFILTER,           
		POSTAGC,
		FINAL
	}
	
	public enum FFTDirection_e
	{
		FORWARD,
		INVERSE,
		INVERSE_NORMALIZE,
		BOTH,
	}
        
	#endregion
}
