/*  eer.c

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

#include "comm.h"

EER create_eer (int run, int size, double* in, double* out, int rate, double mgain, double pgain, double pdelay)
{
	EER a = (EER) malloc0 (sizeof (eer));
	a->run = run;
	a->size = size;
	a->in = in;
	a->out = out;
	a->rate = rate;
	a->mgain = mgain;
	a->pgain = pgain / PI;
	a->pdelay = pdelay;
	a->pdel = create_delay (
		1,											// run
		a->size,									// size
		a->out,										// input buffer
		a->out,										// output buffer
		a->rate,									// sample rate
		20.0e-09,									// delta (delay stepsize)
		a->pdelay,									// delay
		2);											// delay only Q
	return a;
}

void destroy_eer (EER a)
{
	destroy_delay (a->pdel);
	_aligned_free (a);
}

void flush_eer (EER a)
{
	flush_delay (a->pdel);
}

void xeer (EER a)
{
	if (a->run)
	{
		int i;
		double I, Q;
		for (i = 0; i < a->size; i++)
		{
			I = a->in[2 * i + 0];
			Q = a->in[2 * i + 1];
			a->out[2 * i + 0] = a->mgain * sqrt (I * I + Q * Q);
			a->out[2 * i + 1] = a->pgain * atan2 (Q, I);
		}
		xdelay (a->pdel);
	}
	else if (a->out != a->in)
		memcpy (a->out, a->in, a->size * sizeof (complex));
}

/********************************************************************************************************
*																										*
*											TXA Properties												*
*																										*
********************************************************************************************************/

PORT
void SetTXAEERRun (int channel, int run)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].eer.p->run = run;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAEERMgain (int channel, double gain)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].eer.p->mgain = gain;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAEERPgain (int channel, double gain)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].eer.p->pgain = gain / PI;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAEERPdelay (int channel, double delay)
{
	EER a;
	EnterCriticalSection (&ch[channel].csDSP);
	a = txa[channel].eer.p;
	a->pdelay = delay;
	SetDelayValue (a->pdel, a->pdelay);
	LeaveCriticalSection (&ch[channel].csDSP);
}