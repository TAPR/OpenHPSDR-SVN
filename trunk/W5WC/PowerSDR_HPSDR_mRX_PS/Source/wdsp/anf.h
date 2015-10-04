/*  anf.h

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2012, 2013 Warren Pratt, NR0V

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

#ifndef _anf_h
#define _anf_h

#define ANF_DLINE_SIZE 2048

typedef struct _anf
{
	int run;
	int position;
	int buff_size;
	double *in_buff;
	double *out_buff;
	int dline_size;
	int mask;
	int n_taps;
	int delay;
	double two_mu;
	double gamma;
	double d [ANF_DLINE_SIZE];
	double w [ANF_DLINE_SIZE];
	int in_idx;

	double lidx;
	double lidx_min;
	double lidx_max;
	double ngamma;
	double den_mult;
	double lincr;
	double ldecr;
} anf, *ANF;

extern ANF create_anf	(
				int run,
				int position,
				int buff_size,
				double *in_buff,
				double *out_buff,
				int dline_size,
				int n_taps,
				int delay,
				double two_mu,
				double gamma,

				double lidx,
				double lidx_min,
				double lidx_max,
				double ngamma,
				double den_mult,
				double lincr,
				double ldecr
			);

extern void destroy_anf (ANF a);

extern void flush_anf (ANF a);

extern void xanf (ANF a, int position);

extern void setBuffers_anf (ANF a, double* in, double* out);

extern void setSamplerate_anf (ANF a, int rate);

extern void setSize_anf (ANF a, int size);

// RXA Properties

extern __declspec (dllexport) void SetRXAANFRun (int channel, int setit);

extern __declspec (dllexport) void SetRXAANFVals (int channel, int taps, int delay, double gain, double leakage);

extern __declspec (dllexport) void SetRXAANFTaps (int channel, int taps);

extern __declspec (dllexport) void SetRXAANFDelay (int channel, int delay);

extern __declspec (dllexport) void SetRXAANFGain (int channel, double gain);

extern __declspec (dllexport) void SetRXAANFLeakage (int channel, double leakage);

extern __declspec (dllexport) void SetRXAANFPosition (int channel, int position);

#endif