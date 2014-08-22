/*  delay.h

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

#ifndef _delay_h
#define _delay_h

#define WSDEL	65		// number of supported whole sample delays

typedef struct _delay
{
	int run;			// run
	int size;			// number of input samples per buffer
	double* in;			// input buffer
	double* out;		// output buffer
	int rate;			// samplerate
	double tdelta;		// delay increment required (seconds)
	double tdelay;		// delay requested (seconds)

	int L;				// interpolation factor
	int ncoef;			// number of coefficients
	int cpp;			// coefficients per phase
	double ft;			// normalized cutoff frequency
	double* h;			// coefficients
	int snum;			// starting sample number (0 for sub-sample delay)
	int phnum;			// phase number

	int idx_in;			// index for input into ring
	int rsize;			// ring size in complex samples
	double* ring;		// ring buffer

	double adelta;		// actual delay increment
	double adelay;		// actual delay

	CRITICAL_SECTION cs_update;

} delay, *DELAY;

extern DELAY create_delay (int run, int size, double* in, double* out, int rate, double tdelta, double tdelay);

extern void destroy_delay (DELAY a);

extern void flush_delay (DELAY a);

extern void xdelay (DELAY a);

// Properties

extern void SetDelayRun (DELAY a, int run);

extern double SetDelayValue (DELAY a, double delay);		// returns actual delay in seconds

extern void SetDelayBuffs (DELAY a, int size, double* in, double* out);

#endif