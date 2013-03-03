/*  anb.c

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

#define specHPSDR_EXPORT
#include <nob.h>

#define REAL float

void initBlanker(ANB a)
{
    int i;
	a->trans_count = (int)(a->tau * a->samplerate);
    a->hang_count = (int)(a->hangtime * a->samplerate);
    a->adv_count = (int)(a->advtime * a->samplerate);
    a->in_idx = a->trans_count + a->adv_count;
    a->out_idx = 0;
    a->coef = PI / a->trans_count;
    a->state = 0;
    a->avg = 1.0;
    a->power = 1.0;
    a->backmult = exp(-1.0 / (a->samplerate * a->backtau));
    a->ombackmult = 1.0 - a->backmult;
    for (i = 0; i <= a->trans_count; i++)
        a->wave[i] = 0.5 * cos(i * a->coef);
	memset(a->dline, 0, a->dline_size * sizeof(double) * 2);
}

specHPSDR_PORT
void CreateANB(
	int disp,
	int ss,
	int buffsize,
	double samplerate,
	double tau,
	double hangtime,
	double advtime,
	double backtau,
	double threshold
	)
{
	ANB a;
	a = (ANB) malloc (sizeof(anb));
	a->buffsize = buffsize;
	a->samplerate = samplerate;
	a->tau = tau;
	a->hangtime = hangtime;
	a->advtime = advtime;
	a->backtau = backtau;
	a->threshold = threshold;
	a->wave = (double *)malloc(((int)(MAX_SAMPLERATE * MAX_TAU) + 1) * sizeof(double));
	a->dline_size = (int)((MAX_TAU + MAX_ADVTIME) * MAX_SAMPLERATE) + 1;
	a->dline = (double *)malloc(a->dline_size * sizeof(double) * 2);
	initBlanker(a);
	pnob[disp][ss] = a;
}

specHPSDR_PORT
void DestroyANB (int disp, int ss)
{
	ANB a = pnob[disp][ss];
	if (a->dline)	free ((void *)a->dline);
	if (a->wave)	free ((void *)a->wave);
	
	if (a)			free ((void *) a);
}

specHPSDR_PORT
void SetANBBuffSize(int disp, int ss, int BuffSize)
{
	pnob[disp][ss]->buffsize = BuffSize;
}

specHPSDR_PORT
void SetANBSampleRate(int disp, int ss, double SampleRate)
{
	ANB a;
	a = pnob[disp][ss];
	a->samplerate = SampleRate;
	initBlanker(a);
}

specHPSDR_PORT
void SetANBthreshold(int disp, int ss, double threshold)
{
	pnob[disp][ss]->threshold = threshold;
}

specHPSDR_PORT
void SetANBtranstime(int disp, int ss, double transtime)
{
	ANB a;
	a = pnob[disp][ss];
	a->tau = transtime;
	initBlanker(a);
}

specHPSDR_PORT
void SetANBadvtime(int disp, int ss, double advtime)
{
	ANB a;
	a = pnob[disp][ss];
	a->advtime = advtime;
	initBlanker(a);
}

specHPSDR_PORT
void SetANBhangtime(int disp, int ss, double hangtime)
{
	ANB a;
	a = pnob[disp][ss];
	a->hangtime = hangtime;
	initBlanker(a);
}

specHPSDR_PORT
void blanker(int disp, int ss, REAL* Ibuff, REAL* Qbuff)
{
    double scale;
    double mag;
	int i;
	ANB a = pnob[disp][ss];
            
    for (i = 0; i < a->buffsize; i++)
    {
        mag = sqrt(Ibuff[i] * Ibuff[i] + Qbuff[i] * Qbuff[i]);
        a->avg = a->backmult * a->avg + a->ombackmult * mag;
        a->dline[2 * a->in_idx + 0] = Ibuff[i];
        a->dline[2 * a->in_idx + 1] = Qbuff[i];
        if (mag > (a->avg * a->threshold))
            a->count = a->trans_count + a->adv_count;

        switch (a->state)
        {
            case 0:
                {
                    Ibuff[i] = (REAL)a->dline[2 * a->out_idx + 0];
                    Qbuff[i] = (REAL)a->dline[2 * a->out_idx + 1];
                    if (a->count > 0)
                    {
                        a->state = 1;
                        a->dtime = 0;
                        a->power = 1.0;
                    }
                    break;
                }
            case 1:
                {
                    scale = a->power * (0.5 + a->wave[a->dtime]);
                    Ibuff[i] = (REAL)(a->dline[2 * a->out_idx + 0] * scale);
                    Qbuff[i] = (REAL)(a->dline[2 * a->out_idx + 1] * scale);
                    if (++a->dtime > a->trans_count)
                    {
                        a->state = 2;
                        a->atime = 0;
                    }
                    break;
                }
            case 2:
                {
                    Ibuff[i] = 0.0;
                    Qbuff[i] = 0.0;
                    if (++a->atime > a->adv_count)
                        a->state = 3;
                    break;
                }
            case 3:
                {
                    if (a->count > 0)
                        a->htime = -a->count;
                                
                    Ibuff[i] = 0.0;
                    Qbuff[i] = 0.0;
                    if (++a->htime > a->hang_count)
                    {
                        a->state = 4;
                        a->itime = 0;
                    }
                    break;
                }
            case 4:
                {
                    scale = 0.5 - a->wave[a->itime];
                    Ibuff[i] = (REAL)(a->dline[2 * a->out_idx + 0] * scale);
                    Qbuff[i] = (REAL)(a->dline[2 * a->out_idx + 1] * scale);
                    if (a->count > 0)
                    {
                        a->state = 1;
                        a->dtime = 0;
                        a->power = scale;
                    }
                    else if (++a->itime > a->trans_count)
                        a->state = 0;
                    break;
                }
        }
        if (a->count > 0) a->count--;
        if (++a->in_idx == a->dline_size) a->in_idx = 0; 
        if (++a->out_idx == a->dline_size) a->out_idx = 0;
    }
}