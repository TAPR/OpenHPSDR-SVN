/* fm_demod.c */

/* This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2004, 2005, 2006, 2007 by Frank Brickle, AB2KT and Bob McGwier, N4HY

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

The authors can be reached by email at

ab2kt@arrl.net
or
rwmcgwier@comcast.net

or by paper mail at

The DTTS Microwave Society
6 Kathleen Place
Bridgewater, NJ 08807
*/

#include <fm_demod.h>

/*------------------------------------------------------------------------------*/
/* private to FM */
/*------------------------------------------------------------------------------*/

PRIVATE void
init_pll (FMD fm,
	  REAL samprate, REAL freq, REAL lofreq, REAL hifreq, REAL bandwidth)
{
	REAL fac = (REAL) (TWOPI / samprate);

	fm->pll.freq.f = freq * fac;
	fm->pll.freq.l = lofreq * fac;
	fm->pll.freq.h = hifreq * fac;
	fm->pll.phs = 0.0;
	fm->pll.delay = cxJ;

	fm->pll.iir.alpha = bandwidth * fac;					/* arm filter */
	fm->pll.alpha = fm->pll.iir.alpha * 0.3f;				/* pll bandwidth */
	fm->pll.beta = fm->pll.alpha * fm->pll.alpha * 0.25f;	/* second order term */
}

PRIVATE void
pll (FMD fm, COMPLEX sig)
{
	COMPLEX z = Cmplx ((REAL) cos (fm->pll.phs), (IMAG) sin (fm->pll.phs));
	REAL diff;

	fm->pll.delay.re = z.re * sig.re + z.im * sig.im;
	fm->pll.delay.im = -z.im * sig.re + z.re * sig.im;
	diff = fast_atan2 (fm->pll.delay.im, fm->pll.delay.re);

	fm->pll.freq.f += fm->pll.beta * diff;

	if (fm->pll.freq.f < fm->pll.freq.l)
		fm->pll.freq.f = fm->pll.freq.l;

	if (fm->pll.freq.f > fm->pll.freq.h)
		fm->pll.freq.f = fm->pll.freq.h;

	fm->pll.phs += fm->pll.freq.f + fm->pll.alpha * diff;

	while (fm->pll.phs >= TWOPI)
		fm->pll.phs -= (REAL) TWOPI;
	while (fm->pll.phs < 0)
		fm->pll.phs += (REAL) TWOPI;
}

/*------------------------------------------------------------------------------*/
/* public */
/*------------------------------------------------------------------------------*/

void
FMDemod (FMD fm)
{
	int i;
	static int count = 0;
	REAL deemphasis_in;
	REAL rectify;
	REAL dummy = 0;
	
	for (i = 0; i < CXBsize (fm->ibuf); i++)
	{
		//FM Demodulator
		pll (fm, CXBdata (fm->ibuf, i));
		fm->afc = (REAL) (0.9999 * fm->afc + 0.0001 * fm->pll.freq.f);
		CXBreal (fm->obuf, i) = CXBimag (fm->obuf, i) = (fm->pll.freq.f - fm->afc) * fm->cvt;
	}

	// compensate for 1500 Hz low pass in demodulation PLL
	do_IIR_1P1Z(fm->demod_comp_filter);

	// copy to the squelch processing buffer
	memcpy(CXBbase(fm->squelch_obuf), CXBbase(fm->obuf), CXBsize(fm->obuf)*sizeof(COMPLEX));

	for (i = 0; i < CXBsize (fm->ibuf); i++)
	{
		//// copy to the squelch processing buffer
		//CXBreal(fm->squelch_obuf,i) = CXBreal(fm->obuf, i);

		//Demphasis (low-pass filter)
		deemphasis_in = 4.0f * CXBreal (fm->obuf, i);
		fm->deemphasis_out = (deemphasis_in/fm->k_deemphasis) + (fm->k_deemphasis-1.0f)/fm->k_deemphasis*fm->deemphasis_out;
		CXBreal (fm->obuf, i) = CXBimag (fm->obuf, i) = fm->deemphasis_out;
	}

	// Squelch filtering on Demodulated output
	// call the same 2-pole filter twice to get 4 poles
	do_IIR_HPF_2P(fm->squelch_HPF);
	do_IIR_HPF_2P(fm->squelch_HPF);


	for (i = 0; i < CXBsize (fm->squelch_obuf); i++)
	{
		/////Noise Squelch Detector
		rectify = abs(CXBreal(fm->squelch_obuf, i));

		fm->squelch_filter = fm->squelch_k*rectify + (1-fm->squelch_k)* fm->squelch_filter;

		if(fm->squelch_muted)	//muted
		{
			if(fm->squelch_filter < fm->squelch_threshold_unmute )
				fm->squelch_muted = FALSE;
			else
				CXBreal(fm->obuf, i) = (REAL)(rand()%3 - 1) * 1e-16f;

			if(fm->squelch_strong_timer > 0)
				fm->squelch_strong_timer--;
		}

		else	//unmuted
		{
			if(fm->squelch_filter < fm->squelch_threshold_strong)	//strong signal
			{
				fm->squelch_strong_timer = fm->squelch_strong_timer_hang;
				fm->squelch_weak_timer = 0;
			}

			else if(fm->squelch_filter < fm->squelch_threshold_weak)	//weak signal
			{
				if(fm->squelch_strong_timer == 0)
					fm->squelch_weak_timer = fm->squelch_weak_timer_hang;
				else
					fm->squelch_strong_timer--;
			}
			else	//no signal?
			{
				if(fm->squelch_weak_timer > 0)
					fm->squelch_weak_timer--;
				else
				{
					fm->squelch_muted = TRUE;
					CXBreal(fm->obuf, i) = 0.0f;
				}

				if(fm->squelch_strong_timer > 0)
					fm->squelch_strong_timer--;
			}
		}

		//if(count++ % 96000 == 0)
		//{
		//	fprintf(stderr, "filter: %f, thresh: %3f, muted: %d\n", fm->squelch_filter, fm->squelch_threshold_weak, fm->squelch_muted);
		//	fflush(stderr);
		//}
	}

	//300-3000 BPF
	do_IIR_LPF_2P(fm->input_LPF1);
	do_IIR_LPF_2P(fm->input_LPF2);
	do_IIR_HPF_2P(fm->input_HPF1);
	do_IIR_HPF_2P(fm->input_HPF2);

}

FMD
newFMD (REAL samprate,
		REAL f_initial,
		REAL f_lobound,
		REAL f_hibound,
		REAL f_bandwid, int size, COMPLEX * ivec, COMPLEX * ovec, char *tag)
{
	FMD fm = (FMD) safealloc (1, sizeof (FMDDesc), tag);

	fm->size = size;
	fm->ibuf = newCXB (size, ivec, tag);
	fm->obuf = newCXB (size, ovec, tag);
	fm->squelch_obuf = newCXB (size, NULL, tag);

	init_pll (fm, samprate, f_initial, f_lobound, f_hibound, f_bandwid);

	fm->lock = 0.5;
	fm->afc = 0.0;
	//fm->cvt = (REAL) (0.45 * samprate / (M_PI * f_bandwid));
	fm->deviation = 5000.0f;
	fm->cvt = (REAL)(samprate / (fm->deviation * TWOPI) * 0.4 * pow(10, -0.5)); // overwritten on PowerSDR init
	fm->k_deemphasis = (REAL)(1.0f + samprate / (TWOPI * 250.0f)); 
	fm->demod_comp_filter = new_IIR_1P1Z(fm->obuf, samprate, 24000, 1500);		// compensate for 1500 Hz low pass in PLL

	fm->input_HPF1 = new_IIR_HPF_2P(fm->obuf, samprate, 250.0f, 0.765f);		//300 Hz high-pass butterworth
	fm->input_HPF2 = new_IIR_HPF_2P(fm->obuf, samprate, 250.0f, 1.848f);		//300 Hz high-pass butterworth
	fm->input_LPF1 = new_IIR_LPF_2P(fm->obuf, samprate, 3500.0f, 0.25f);	//3000 Hz low-pass butterworth
	fm->input_LPF2 = new_IIR_LPF_2P(fm->obuf, samprate, 3500.0f, 1.75f);	//3000 Hz low-pass butterworth

	fm->squelch_HPF = new_IIR_HPF_2P(fm->squelch_obuf, samprate, 5500.0f, 0.707f);
	fm->squelch_k = (REAL)(1.0f/(samprate/TWOPI/200.0f + 1.0f));
	fm->squelch_threshold_weak = 0.05f;
	fm->squelch_threshold_strong = 0.025f;
	fm->squelch_threshold_unmute = 0.045f;
	fm->squelch_filter = 1.0f;
	fm->squelch_strong_timer_hang = (int)(0.02*samprate);
	fm->squelch_weak_timer_hang = (int)(0.15*samprate);

	return fm;
}

void
delFMD (FMD fm)
{
	if (fm)
	{
		delCXB (fm->ibuf);
		delCXB (fm->obuf);
		// delete deemphasis and pinching filters
		del_IIR_HPF_2P(fm->input_HPF1);
		del_IIR_HPF_2P(fm->input_HPF2);
		del_IIR_LPF_2P(fm->input_LPF1);
		del_IIR_LPF_2P(fm->input_LPF2);
		del_IIR_HPF_2P(fm->squelch_HPF);
		del_IIR_1P1Z(fm->demod_comp_filter);
		safefree ((char *) fm);
	}
}
