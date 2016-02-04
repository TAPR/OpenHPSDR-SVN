/*  amd.h

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

#ifndef _amd_h
#define _amd_h

// ff defines for sbdemod
#ifndef STAGES
#define STAGES		7
#endif

#ifndef OUT_IDX
#define OUT_IDX		(3 * STAGES)
#endif

typedef struct _amd
{
	int run;
	int buff_size;						// buffer size
	double *in_buff;					// pointer to input buffer
	double *out_buff;					// pointer to output buffer
	int mode;							// demodulation mode
	double sample_rate;					// sample rate
	double dc;							// dc component in demodulated output
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
	double tauR;						// carrier removal time constant
	double tauI;						// carrier insertion time constant
	double mtauR;						// carrier removal multiplier
	double onem_mtauR;					// 1.0 - carrier_removal_multiplier
	double mtauI;						// carrier insertion multiplier
	double onem_mtauI;					// 1.0 - carrier_insertion_multiplier
	double a[3 * STAGES + 3];			// Filter a variables
	double b[3 * STAGES + 3];			// Filter b variables
	double c[3 * STAGES + 3];			// Filter c variables
	double d[3 * STAGES + 3];			// Filter d variables
	double c0[STAGES];					// Filter coefficients - path 0
	double c1[STAGES];					// Filter coefficients - path 1
	double dsI;							// delayed sample, I path
	double dsQ;							// delayed sample, Q path
	double dc_insert;					// dc component to insert in output
	int sbmode;							// sideband mode
	int levelfade;						// Fade Leveler switch

}amd, *AMD;

extern AMD create_amd 
	(
	int run,
	int buff_size,
	double *in_buff,
	double *out_buff,
	int mode,
	int levelfade,
	int sbmode,
	int sample_rate,
	double fmin,
	double fmax,
	double zeta,
	double omegaN,
	double tauR,
	double tauI
	);

extern void init_amd (AMD a);

extern void destroy_amd (AMD a);

extern void flush_amd (AMD a);

extern void xamd (AMD a);

extern void setBuffers_amd (AMD a, double* in, double* out);

extern void setSamplerate_amd (AMD a, int rate);

extern void setSize_amd (AMD a, int size);

// RXA Properties

extern __declspec (dllexport) void SetRXAAMDRun(int channel, int run);

extern __declspec (dllexport) void SetRXAAMDSBMode(int channel, int sbmode);

extern __declspec (dllexport) void SetRXAAMDFadeLevel(int channel, int levelfade);

#endif