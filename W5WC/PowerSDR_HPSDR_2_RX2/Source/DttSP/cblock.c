/*  cblock.c

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

#include <common.h>
#include <math.h>
#include <cblock.h>

#define		INREAL		float
#define		OUTREAL		float

CBL newCBL
	(
	int buff_size,
	COMPLEX *in_buff,
	COMPLEX *out_buff,
	int mode,
	float sample_rate,
	double tau,
	char *tag
	)
{
	CBL a;
	a = (CBL) safealloc (1,sizeof(cbl), tag);
	a->buff_size = buff_size;
	a->in_buff = in_buff;
	a->out_buff = out_buff;
	a->mode = mode;
	a->sample_rate = sample_rate;
	a->tau = tau;

	a->dcI = 0.0;
	a->dcQ = 0.0;
	a->mtau = exp(-1.0 / (a->sample_rate * a->tau));
	a->onem_mtau = 1.0 - a->mtau;

	return a;
}

void del_cbl(CBL a)
{
	if (a) safefree((char *) a);
}

void c_block(CBL a)
{
	int i;

	for (i = 0; i < a->buff_size; i++)
	{
		a->dcI = a->mtau * a->dcI + a->onem_mtau * a->in_buff[i].re;
		a->dcQ = a->mtau * a->dcQ + a->onem_mtau * a->in_buff[i].im;
		a->out_buff[i].re = (OUTREAL)(a->in_buff[i].re - a->dcI);
		a->out_buff[i].im = (OUTREAL)(a->in_buff[i].im - a->dcQ);
	}
}

DttSP_EXP void
SetCBL(unsigned int thread, unsigned int subrx, int setit)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].cbl.flag = setit;
	sem_post(&top[thread].sync.upd.sem);
}