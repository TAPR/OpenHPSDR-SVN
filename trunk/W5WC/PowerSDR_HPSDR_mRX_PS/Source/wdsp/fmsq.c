/*  fmsq.c

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

FMSQ create_fmsq (int run, int size, double* insig, double* outsig, double* trigger, int rate, double fc, double pllpole, double avtau, double longtau, double tup, double tdown, double tail_thresh, double unmute_thresh, double min_tail, double max_tail)
{
	FMSQ a = (FMSQ) malloc0 (sizeof (fmsq));
	double delta, theta;
	int i;
	a->run = run;
	a->size = size;
	a->insig = insig;
	a->outsig = outsig;
	a->trigger = trigger;
	a->rate = (double)rate;
	// noise filter
	a->fc = fc;
	a->pllpole = pllpole;
	a->infilt  = (double *) malloc0 (2 * a->size * sizeof (complex));
	a->product = (double *) malloc0 (2 * a->size * sizeof (complex));
	a->noise   = (double *) malloc0 (2 * a->size * sizeof (complex));
	a->F[0] = 0.0;
	a->F[1] = a->fc;
	a->F[2] = a->pllpole;
	a->F[3] = 20000.0;
	a->G[0] = 0.0;
	a->G[1] = 0.0;
	a->G[2] = 3.0;
	a->G[3] = + 20.0 * log10 (20000.0 / a->pllpole);
	a->mults = eq_mults (a->size, 3, a->F, a->G, a->rate, 1.0 / (2.0 * a->size), 0, 1);
	a->CFor = fftw_plan_dft_1d(2 * a->size, (fftw_complex *)a->infilt,  (fftw_complex *)a->product, FFTW_FORWARD,  FFTW_PATIENT);
	a->CRev = fftw_plan_dft_1d(2 * a->size, (fftw_complex *)a->product, (fftw_complex *)a->noise,   FFTW_BACKWARD, FFTW_PATIENT);
	// noise averaging
	a->avtau = avtau;
	a->avm = exp (-1.0 / (a->rate * a->avtau));
	a->onem_avm = 1.0 - a->avm;
	a->avnoise = 100.0;
	a->longtau = longtau;
	a->longavm = exp (-1.0 / (a->rate * a->longtau));
	a->onem_longavm = 1.0 - a->longavm;
	a->longnoise = 1.0;
	// level change
	a->tup = tup;
	a->tdown = tdown;
	a->ntup = (int)(a->tup * a->rate);
	a->ntdown = (int)(a->tdown * a->rate);
	a->cup   = (double *) malloc0 ((a->ntup + 1) * sizeof (double));
	a->cdown = (double *) malloc0 ((a->ntdown + 1) * sizeof (double));
	delta = PI / (double)a->ntup;
	theta = 0.0;
	for (i = 0; i <= a->ntup; i++)
	{
		a->cup[i] = 0.5 * (1.0 - cos (theta));
		theta += delta;
	}
	delta = PI / (double)a->ntdown;
	theta = 0.0;
	for (i = 0; i <= a->ntdown; i++)
	{
		a->cdown[i] = 0.5 * (1 + cos (theta));
		theta += delta;
	}
	// control
	a->tail_thresh = tail_thresh;
	a->unmute_thresh = unmute_thresh;
	a->min_tail = min_tail;
	a->max_tail = max_tail;
	a->state = 0;
	return a;
}

void destroy_fmsq (FMSQ a)
{
	_aligned_free (a->cdown);
	_aligned_free (a->cup);
	fftw_destroy_plan (a->CRev);
	fftw_destroy_plan (a->CFor);
	_aligned_free (a->mults);
	_aligned_free (a->noise);
	_aligned_free (a->product);
	_aligned_free (a->infilt);
	_aligned_free (a);
}

void flush_fmsq (FMSQ a)
{
	memset (a->infilt, 0, 2 * a->size * sizeof (complex));
	a->avnoise = 100.0;
	a->longnoise = 1.0;
	a->state = 0;
}

enum _fmsqstate
{
	MUTED,
	INCREASE,
	UNMUTED,
	TAIL,
	DECREASE
};

void xfmsq (FMSQ a)
{
	if (a->run)
	{
		int i;
		double I, Q, noise, lnlimit;
		memcpy (&(a->infilt[2 * a->size]), a->trigger, a->size * sizeof (complex));
		fftw_execute (a->CFor);
		for (i = 0; i < 2 * a->size; i++)
		{
			I = a->product[2 * i + 0];
			Q = a->product[2 * i + 1];
			a->product[2 * i + 0] = I * a->mults[2 * i + 0] - Q * a->mults[2 * i + 1];
			a->product[2 * i + 1] = I * a->mults[2 * i + 1] + Q * a->mults[2 * i + 0];
		}
		fftw_execute (a->CRev);
		memcpy (a->infilt, &(a->infilt[2 * a->size]), a->size * sizeof(complex));

		for (i = 0; i < a->size; i++)
		{
			noise = sqrt(a->noise[2 * i + 0] * a->noise[2 * i + 0] + a->noise[2 * i + 1] * a->noise[2 * i + 1]);
			a->avnoise = a->avm * a->avnoise + a->onem_avm * noise;
			a->longnoise = a->longavm * a->longnoise + a->onem_longavm * noise;
			switch (a->state)
			{
			case MUTED:
				if (a->avnoise < a->unmute_thresh)
				{
					a->state = INCREASE;
					a->count = a->ntup;
				}
				a->outsig[2 * i + 0] = 0.0;
				a->outsig[2 * i + 1] = 0.0;
				break;
			case INCREASE:
				if (a->count-- == 0)
					a->state = UNMUTED;
				a->outsig[2 * i + 0] = a->insig[2 * i + 0] * a->cup[a->ntup - a->count];
				a->outsig[2 * i + 1] = a->insig[2 * i + 1] * a->cup[a->ntup - a->count];
				break;
			case UNMUTED:
				if (a->avnoise > a->tail_thresh)
				{
					a->state = TAIL;
					if ((lnlimit = a->longnoise) > 1.0) lnlimit = 1.0;
					a->count = (int)((a->min_tail + (a->max_tail - a->min_tail) * lnlimit) * a->rate);
				}
				a->outsig[2 * i + 0] = a->insig[2 * i + 0];
				a->outsig[2 * i + 1] = a->insig[2 * i + 1];
				break;
			case TAIL:
				if (a->avnoise < a->unmute_thresh)
					a->state = UNMUTED;
				else if (a->count-- == 0)
				{
					a->state = DECREASE;
					a->count = a->ntdown;
				}
				a->outsig[2 * i + 0] = a->insig[2 * i + 0];
				a->outsig[2 * i + 1] = a->insig[2 * i + 1];
				break;
			case DECREASE:
				if (a->count-- == 0)
					a->state = MUTED;
				a->outsig[2 * i + 0] = a->insig[2 * i + 0] * a->cdown[a->ntdown - a->count];
				a->outsig[2 * i + 1] = a->insig[2 * i + 1] * a->cdown[a->ntdown - a->count];
				break;
			}
		}
	}
	else if (a->insig != a->outsig)
		memcpy (a->outsig, a->insig, a->size * sizeof (complex));
}

/********************************************************************************************************
*																										*
*											RXA Properties												*
*																										*
********************************************************************************************************/

PORT
void SetRXAFMSQRun (int channel, int run)
{
	EnterCriticalSection (&ch[channel].csDSP);
	rxa[channel].fmsq.p->run = run;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetRXAFMSQThreshold (int channel, double threshold)
{
	EnterCriticalSection (&ch[channel].csDSP);
	rxa[channel].fmsq.p->tail_thresh = threshold;
	rxa[channel].fmsq.p->unmute_thresh = 0.9 * threshold;
	LeaveCriticalSection (&ch[channel].csDSP);
}