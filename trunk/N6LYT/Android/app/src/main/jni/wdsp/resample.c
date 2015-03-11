/*  resample.c

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

#include "comm.h"

/************************************************************************************************
*																								*
*							  VERSION FOR COMPLEX DOUBLE-PRECISION								*
*																								*
************************************************************************************************/

RESAMPLE create_resample ( int run, int size, double* in, double* out, int in_rate, int out_rate, double gain)
{
	RESAMPLE a = (RESAMPLE) malloc0 (sizeof (resample));
	int x, y, z;
	int i, j, k;
	int min_rate;
	double full_rate;
	double fc;
	double fc_norm;
	double* impulse;
	a->run = run;
	a->size = size;
	a->in = in;
	a->out = out;
	a->gain = gain;
	x = in_rate;
	y = out_rate;
	while (y != 0)
	{
		z = y;
		y = x % y;
		x = z;
    }
	a->L = out_rate / x;
	a->M = in_rate / x;
	if (in_rate < out_rate) min_rate = in_rate;
	else min_rate = out_rate;
	fc = 0.45 * (double)min_rate;
	full_rate = (double)(in_rate * a->L);
	fc_norm = fc / full_rate;
	a->ncoef = (int)(60.0 / fc_norm);
	a->ncoef = (a->ncoef / a->L + 1) * a->L;
	a->cpp = a->ncoef / a->L;
	a->h = (double *) malloc0 (a->ncoef * sizeof (double));
	impulse = fir_bandpass (a->ncoef, -fc_norm, +fc_norm, 1.0, 1, 0, a->gain * (double)a->L);
	i = 0;
	for (j = 0; j < a->L; j ++)
		for (k = 0; k < a->ncoef; k += a->L)
			a->h[i++] = impulse[j + k];
	a->ringsize = a->cpp;
	a->ring = (double *) malloc0 (a->ringsize * sizeof (complex));
	a->idx_in = a->ringsize - 1;
	a->phnum = 0;
	_aligned_free (impulse);
	return a;
}

void destroy_resample (RESAMPLE a)
{
	_aligned_free (a->ring);
	_aligned_free (a->h);
	_aligned_free (a);
}

void flush_resample (RESAMPLE a)
{
	memset (a->ring, 0, a->ringsize * sizeof (complex));
	a->idx_in = a->ringsize - 1;
	a->phnum = 0;
}

int xresample (RESAMPLE a)
{
	int outsamps = 0;
	if (a->run)
	{
		int i, j, n;
		int idx_out;
		double I, Q;

		for (i = 0; i < a->size; i++)
		{
			a->ring[2 * a->idx_in + 0] = a->in[2 * i + 0];
			a->ring[2 * a->idx_in + 1] = a->in[2 * i + 1];
			while (a->phnum < a->L)
			{
				I = 0.0;
				Q = 0.0;
				n = a->cpp * a->phnum;
				for (j = 0; j < a->cpp; j++)
				{
					if ((idx_out = a->idx_in + j) >= a->ringsize) idx_out -= a->ringsize;
					I += a->h[n + j] * a->ring[2 * idx_out + 0];
					Q += a->h[n + j] * a->ring[2 * idx_out + 1];
				}
				a->out[2 * outsamps + 0] = I;
				a->out[2 * outsamps + 1] = Q;
				outsamps++;
				a->phnum += a->M;
			}
			a->phnum -= a->L;
			if (--a->idx_in < 0) a->idx_in = a->ringsize - 1;
		}
	}
	else if (a->in != a->out)
		memcpy (a->out, a->in, a->size * sizeof (complex));
	return outsamps;
}

/************************************************************************************************
*																								*
*							  VERSION FOR NON-COMPLEX FLOATS									*
*																								*
************************************************************************************************/

RESAMPLEF create_resampleF ( int run, int size, float* in, float* out, int in_rate, int out_rate)
{
	RESAMPLEF a = (RESAMPLEF) malloc0 (sizeof (resampleF));
	int x, y, z;
	int i, j, k;
	int min_rate;
	double full_rate;
	double fc;
	double fc_norm;
	double* impulse;
	a->run = run;
	a->size = size;
	a->in = in;
	a->out = out;
	x = in_rate;
	y = out_rate;
	while (y != 0)
	{
		z = y;
		y = x % y;
		x = z;
    }
	a->L = out_rate / x;
	a->M = in_rate / x;
	if (in_rate < out_rate) min_rate = in_rate;
	else min_rate = out_rate;
	fc = 0.45 * (double)min_rate;
	full_rate = (double)(in_rate * a->L);
	fc_norm = fc / full_rate;
	a->ncoef = (int)(60.0 / fc_norm);
	a->ncoef = (a->ncoef / a->L + 1) * a->L;
	a->cpp = a->ncoef / a->L;
	a->h = (double *) malloc0 (a->ncoef * sizeof (double));
	impulse = fir_bandpass (a->ncoef, -fc_norm, +fc_norm, 1.0, 1, 0, (double)a->L);
	i = 0;
	for (j = 0; j < a->L; j ++)
		for (k = 0; k < a->ncoef; k += a->L)
			a->h[i++] = impulse[j + k];
	a->ringsize = a->cpp;
	a->ring = (double *) malloc0 (a->ringsize * sizeof (double));
	a->idx_in = a->ringsize - 1;
	a->phnum = 0;
	_aligned_free (impulse);
	return a;
}

void destroy_resampleF (RESAMPLEF a)
{
	_aligned_free (a->ring);
	_aligned_free (a->h);
	_aligned_free (a);
}

void flush_resampleF (RESAMPLEF a)
{
	memset (a->ring, 0, a->ringsize * sizeof (double));
	a->idx_in = a->ringsize - 1;
	a->phnum = 0;
}

int xresampleF (RESAMPLEF a)
{
	int outsamps = 0;
	if (a->run)
	{
		int i, j, n;
		int idx_out;
		double I;

		for (i = 0; i < a->size; i++)
		{
			a->ring[a->idx_in] = (double)a->in[i];

			while (a->phnum < a->L)
			{
				I = 0.0;
				n = a->cpp * a->phnum;
				for (j = 0; j < a->cpp; j++)
				{
					if ((idx_out = a->idx_in + j) >= a->ringsize) idx_out -= a->ringsize;
					I += a->h[n + j] * a->ring[idx_out];
				}
				a->out[outsamps] = (float)I;

				outsamps++;
				a->phnum += a->M;
			}
			a->phnum -= a->L;
			if (--a->idx_in < 0) a->idx_in = a->ringsize - 1;
		}
	}
	else if (a->in != a->out)
		memcpy (a->out, a->in, a->size * sizeof (float));
	return outsamps;
}

// Exported calls

PORT
void* create_resampleFV (int in_rate, int out_rate)
{
	return (void *)create_resampleF (1, 0, 0, 0, in_rate, out_rate);
}

PORT
void xresampleFV (float* input, float* output, int numsamps, int* outsamps, void* ptr)
{
	RESAMPLEF a = (RESAMPLEF)ptr;
	a->in = input;
	a->out = output;
	a->size = numsamps;
	*outsamps = xresampleF(a);
}

PORT
void destroy_resampleFV (void* ptr)
{
	destroy_resampleF ( (RESAMPLEF)ptr );
}