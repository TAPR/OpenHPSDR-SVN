/*  fmd.h

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2013 Warren Pratt, NR0V

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

The author can be reached by email at  

warren@wpratt.com

*/

#ifndef _fmd_h
#define _fmd_h
#include "iir.h"
typedef struct _fmd
{
	int run;
	int size;
	double* in;
	double* out;
	double rate;
	double f_low;						// audio low cutoff
	double f_high;						// audio high cutoff
	// pll
	double fmin;						// pll - minimum carrier freq to lock
	double fmax;						// pll - maximum carrier freq to lock
	double omega_min;					// pll - minimum lock check parameter
	double omega_max;					// pll - maximum lock check parameter
	double zeta;						// pll - damping factor; as coded, must be <=1.0
	double omegaN;						// pll - natural frequency
	double phs;							// pll - phase accumulator
	double omega;						// pll - locked pll frequency
	double fil_out;						// pll - filter output
	double g1, g2;						// pll - filter gain parameters
	double pllpole;						// pll - pole frequency
	// for dc removal
	double tau;
	double mtau;
	double onem_mtau;
	double fmdc;
	// pll audio gain
	double deviation;
	double again;
	double* audio;						// buffer to hold audio before de-emphasis
	// for de-emphasis filter
	double* infilt;
	double* product;
	double* mults;
	double* outfilt;
	fftw_plan CFor;
	fftw_plan CRev;
	// for audio filter
	double* ainfilt;
	double* aproduct;
	double* amults;
	double afgain;
	fftw_plan aCFor;
	fftw_plan aCRev;
	// CTCSS removal
	SNOTCH sntch;
	int sntch_run;
	double ctcss_freq;
} fmd, *FMD;

extern FMD create_fmd ( int run, int size, double* in, double* out, int rate, double deviation, double f_low, double f_high, double fmin, double fmax, double zeta, double omegaN, double tau, double afgain, int sntch_run, double ctcss_freq);

extern void destroy_fmd (FMD a);

extern void flush_fmd (FMD a);

extern void xfmd (FMD a);

extern void setBuffers_fmd (FMD a, double* in, double* out);

extern void setSamplerate_fmd (FMD a, int rate);

extern void setSize_fmd (FMD a, int size);

// RXA Properties

extern __declspec (dllexport) void SetRXAFMDeviation (int channel, double deviation);

#endif