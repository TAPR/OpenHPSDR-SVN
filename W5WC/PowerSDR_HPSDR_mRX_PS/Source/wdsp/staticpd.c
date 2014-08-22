/*  staticpd.c

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

STATICPD create_staticpd (int run, int size, double* in, double* out)
{
	STATICPD a = (STATICPD) malloc0 (sizeof (staticpd));
	a->run = run;
	a->size = size;
	a->in = in;
	a->out = out;
	a->a0 = 1.0;
	a->b[0] = 1.0;
	return a;
}

void destroy_staticpd (STATICPD a)
{
	_aligned_free (a);
}

void flush_staticpd (STATICPD a)
{

}

void xstaticpd (STATICPD a)
{
	int i;
	double I, Q, mag, C;
	if (a->run)
	{
		for (i = 0; i < a->size; i++)
		{
			I = a->in[2 * i + 0];
			Q = a->in[2 * i + 1];
			mag = sqrt (I * I + Q * Q);
			C = a->a0 / (a->b[0] + mag * (a->b[1] + mag * (a->b[2] + mag * (a->b[3] + mag * (a->b[4] + mag * a->b[5])))));
			a->out[2 * i + 0] = I * C;
			a->out[2 * i + 1] = Q * C;
		}
	}
	else if (a->in != a->out)
		memcpy (a->out, a->in, a->size * sizeof (complex));
}

/********************************************************************************************************
*																										*
*											TXA Properties												*
*																										*
********************************************************************************************************/

PORT
void SetTXAStaticPDRun (int channel, int run)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].staticpd.p->run = run;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAStaticPDGain (int channel, double* coefs)
{
	STATICPD a;
	EnterCriticalSection (&ch[channel].csDSP);
	a = txa[channel].staticpd.p;
	a->a0 = coefs[0];
	a->b[0] = coefs[1];
	a->b[1] = coefs[2];
	a->b[2] = coefs[3];
	a->b[3] = coefs[4];
	a->b[4] = coefs[5];
	a->b[5] = coefs[6];
	LeaveCriticalSection (&ch[channel].csDSP);
}