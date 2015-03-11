/*  gain.c

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

GAIN create_gain (int run, int size, double* in, double* out, double Igain, double Qgain)
{
	GAIN a = (GAIN) malloc0 (sizeof (gain));
	a->run = run;
	a->size = size;
	a->in = in;
	a->out = out;
	a->Igain = Igain;
	a->Qgain = Qgain;
	return a;
}

void destroy_gain (GAIN a)
{
	_aligned_free (a);
}

void flush_gain (GAIN a)
{

}

void xgain (GAIN a)
{
	if (a->run)
	{
		int i;
		for (i = 0; i < a->size; i++)
		{
			a->out[2 * i + 0] = a->Igain * a->in[2 * i + 0];
			a->out[2 * i + 1] = a->Qgain * a->in[2 * i + 1];
		}
	}
	else if (a->in != a->out)
		memcpy (a->out, a->in, a->size * sizeof (complex));
}