/*  iqc.h

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

#ifndef _iqc_h
#define _iqc_h

typedef struct _iqc
{
	volatile long run;
	volatile long busy;
	int size;
	double* in;
	double* out;
	double rate;
	int ints;
	double* t;
	int cset;
	double* cm[2];
	double* cc[2];
	double* cs[2];
	double tup;
	double* cup;
	int count;
	int ntup;
	int state;
} iqc, *IQC;

extern IQC create_iqc (int run, int size, double* in, double* out, double rate, int ints, double tup);

extern void destroy_iqc (IQC a);

extern void flush_iqc (IQC a);

extern void xiqc (IQC a);

// TXA Properties

extern __declspec (dllexport)  void GetTXAiqcValues (int channel, double* cm, double* cc, double* cs);

extern __declspec (dllexport)  void SetTXAiqcValues (int channel, double* cm, double* cc, double* cs);

extern __declspec (dllexport)  void SetTXAiqcSwap (int channel, double* cm, double* cc, double* cs);

extern __declspec (dllexport)  void SetTXAiqcStart (int channel, double* cm, double* cc, double* cs);

extern __declspec (dllexport)  void SetTXAiqcEnd (int channel);

#endif