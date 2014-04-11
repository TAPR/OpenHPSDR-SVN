/*  iir.h

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2014 Warren Pratt, NR0V

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

#ifndef _snotch_h
#define _snotch_h

typedef struct _snotch
{
	int run;
	int size;
	double* in;
	double* out;
	double rate;
	double f;
	double bw;
	double a0, a1, a2, b1, b2;
	double x0, x1, x2, y1, y2;
	CRITICAL_SECTION cs_update;
} snotch, *SNOTCH;

extern SNOTCH create_snotch (int run, int size, double* in, double* out, int rate, double f, double bw);

extern void destroy_snotch (SNOTCH a);

extern void flush_snotch (SNOTCH a);

extern void xsnotch (SNOTCH a);

extern void SetSNCTCSSFreq (SNOTCH a, double freq);

extern void SetSNCTCSSRun (SNOTCH a, int run);

#endif