/*  gen.c

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

void calc_tone (GEN a)
{
	a->tone.phs = 0.0;
	a->tone.delta = TWOPI * a->tone.freq / a->rate;
	a->tone.cosdelta = cos (a->tone.delta);
	a->tone.sindelta = sin (a->tone.delta);
}

void calc_tt (GEN a)
{
	a->tt.phs1 = 0.0;
	a->tt.phs2 = 0.0;
	a->tt.delta1 = TWOPI * a->tt.f1 / a->rate;
	a->tt.delta2 = TWOPI * a->tt.f2 / a->rate;
	a->tt.cosdelta1 = cos (a->tt.delta1);
	a->tt.cosdelta2 = cos (a->tt.delta2);
	a->tt.sindelta1 = sin (a->tt.delta1);
	a->tt.sindelta2 = sin (a->tt.delta2);
}

void calc_sweep (GEN a)
{
	a->sweep.phs = 0.0;
	a->sweep.dphs = TWOPI * a->sweep.f1 / a->rate;
	a->sweep.d2phs = TWOPI * a->sweep.sweeprate / (a->rate * a->rate);
	a->sweep.dphsmax = TWOPI * a->sweep.f2 / a->rate;
}

GEN create_gen (int run, int size, double* in, double* out, int rate, int mode)
{
	GEN a = (GEN) malloc0 (sizeof (gen));
	a->run = run;
	a->size = size;
	a->in = in;
	a->out = out;
	a->rate = (double)rate;
	a->mode = mode;
	// tone
	a->tone.mag = 1.0;
	a->tone.freq = 1000.0;
	calc_tone (a);
	// two-tone
	a->tt.mag1 = 0.5;
	a->tt.mag2 = 0.5;
	a->tt.f1 = +  900.0;
	a->tt.f2 = + 1700.0;
	calc_tt (a);
	// noise
	srand ((unsigned int)time (0));
	a->noise.mag = 1.0;
	// sweep
	a->sweep.mag = 1.0;
	a->sweep.f1 = -20000.0;
	a->sweep.f2 = +20000.0;
	a->sweep.sweeprate = +4000.0;
	calc_sweep (a);
	return a;
}

void destroy_gen (GEN a)
{
	
	_aligned_free (a);
}

void flush_gen (GEN a)
{

}

void xgen (GEN a)
{
	if (a->run)
	{
		switch (a->mode)
		{
		case 0:	// tone
			{
				int i;
				double t1, t2;
				double cosphase = cos (a->tone.phs);
				double sinphase = sin (a->tone.phs);
				for (i = 0; i < a->size; i++)
				{
					a->out[2 * i + 0] = + a->tone.mag * cosphase;
					a->out[2 * i + 1] = - a->tone.mag * sinphase;
					t1 = cosphase;
					t2 = sinphase;
					cosphase = t1 * a->tone.cosdelta - t2 * a->tone.sindelta;
					sinphase = t1 * a->tone.sindelta + t2 * a->tone.cosdelta;
					a->tone.phs += a->tone.delta;
					if (a->tone.phs >= TWOPI) a->tone.phs -= TWOPI;
					if (a->tone.phs <   0.0 ) a->tone.phs += TWOPI;
				}
				break;
			}
		case 1:	// two-tone
			{
				int i;
				double tcos, tsin;
				double cosphs1 = cos (a->tt.phs1);
				double sinphs1 = sin (a->tt.phs1);
				double cosphs2 = cos (a->tt.phs2);
				double sinphs2 = sin (a->tt.phs2);
				for (i = 0; i < a->size; i++)
				{
					a->out[2 * i + 0] = + a->tt.mag1 * cosphs1 + a->tt.mag2 * cosphs2;
					a->out[2 * i + 1] = - a->tt.mag1 * sinphs1 - a->tt.mag2 * sinphs2;
					tcos = cosphs1;
					tsin = sinphs1;
					cosphs1 = tcos * a->tt.cosdelta1 - tsin * a->tt.sindelta1;
					sinphs1 = tcos * a->tt.sindelta1 + tsin * a->tt.cosdelta1;
					a->tt.phs1 += a->tt.delta1;
					if (a->tt.phs1 >= TWOPI) a->tt.phs1 -= TWOPI;
					if (a->tt.phs1 <   0.0 ) a->tt.phs1 += TWOPI;
					tcos = cosphs2;
					tsin = sinphs2;
					cosphs2 = tcos * a->tt.cosdelta2 - tsin * a->tt.sindelta2;
					sinphs2 = tcos * a->tt.sindelta2 + tsin * a->tt.cosdelta2;
					a->tt.phs2 += a->tt.delta2;
					if (a->tt.phs2 >= TWOPI) a->tt.phs2 -= TWOPI;
					if (a->tt.phs2 <   0.0 ) a->tt.phs2 += TWOPI;
				}
				break;
			}
		case 2: // noise
			{
				int i;
				double r1, r2, c, rad;
				for (i = 0; i < a->size; i++)
				{
					do
					{
						r1 = 2.0 * (double)rand() / (double)RAND_MAX - 1.0;
						r2 = 2.0 * (double)rand() / (double)RAND_MAX - 1.0;
						c = r1 * r1 + r2 * r2;
					} while (c >= 1.0);
					rad = sqrt (-2.0 * log (c) / c);
					a->out[2 * i + 0] = a->noise.mag * rad * r1;
					a->out[2 * i + 1] = a->noise.mag * rad * r2;
				}
				break;
			}
		case 3:  // sweep
			{
				int i;
				for (i = 0; i < a->size; i++)
				{
					a->out[2 * i + 0] = + a->sweep.mag * cos(a->sweep.phs);
					a->out[2 * i + 1] = - a->sweep.mag * sin(a->sweep.phs);
					a->sweep.phs += a->sweep.dphs;
					a->sweep.dphs += a->sweep.d2phs;
					if (a->sweep.phs >= TWOPI) a->sweep.phs -= TWOPI;
					if (a->sweep.phs <   0.0 ) a->sweep.phs += TWOPI;
					if (a->sweep.dphs > a->sweep.dphsmax)
						a->sweep.dphs = TWOPI * a->sweep.f1 / a->rate;
				}
				break;
			}
		default:	// silence
			{
				memset (a->out, 0, a->size * sizeof (complex));
				break;
			}
		}
	}
	else if (a->in != a->out)
		memcpy (a->out, a->in, a->size * sizeof (complex));
}


/********************************************************************************************************
*																										*
*											RXA Properties												*
*																										*
********************************************************************************************************/

// 'PreGen', gen0

PORT
void SetRXAPreGenRun (int channel, int run)
{
	EnterCriticalSection (&ch[channel].csDSP);
	rxa[channel].gen0.p->run = run;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetRXAPreGenMode (int channel, int mode)
{
	EnterCriticalSection (&ch[channel].csDSP);
	rxa[channel].gen0.p->mode = mode;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetRXAPreGenToneMag (int channel, double mag)
{
	EnterCriticalSection (&ch[channel].csDSP);
	rxa[channel].gen0.p->tone.mag = mag;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetRXAPreGenToneFreq (int channel, double freq)
{
	EnterCriticalSection (&ch[channel].csDSP);
	rxa[channel].gen0.p->tone.freq = freq;
	calc_tone (rxa[channel].gen0.p);
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetRXAPreGenNoiseMag (int channel, double mag)
{
	EnterCriticalSection (&ch[channel].csDSP);
	rxa[channel].gen0.p->noise.mag = mag;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetRXAPreGenSweepMag (int channel, double mag)
{
	EnterCriticalSection (&ch[channel].csDSP);
	rxa[channel].gen0.p->sweep.mag = mag;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetRXAPreGenSweepFreq (int channel, double freq1, double freq2)
{
	EnterCriticalSection (&ch[channel].csDSP);
	rxa[channel].gen0.p->sweep.f1 = freq1;
	rxa[channel].gen0.p->sweep.f2 = freq2;
	calc_sweep (rxa[channel].gen0.p);
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetRXAPreGenSweepRate (int channel, double rate)
{
	EnterCriticalSection (&ch[channel].csDSP);
	rxa[channel].gen0.p->sweep.sweeprate = rate;
	calc_sweep (rxa[channel].gen0.p);
	LeaveCriticalSection (&ch[channel].csDSP);
}


/********************************************************************************************************
*																										*
*											TXA Properties												*
*																										*
********************************************************************************************************/

// 'PreGen', gen0

PORT
void SetTXAPreGenRun (int channel, int run)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen0.p->run = run;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAPreGenMode (int channel, int mode)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen0.p->mode = mode;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAPreGenToneMag (int channel, double mag)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen0.p->tone.mag = mag;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAPreGenToneFreq (int channel, double freq)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen0.p->tone.freq = freq;
	calc_tone (txa[channel].gen0.p);
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAPreGenNoiseMag (int channel, double mag)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen0.p->noise.mag = mag;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAPreGenSweepMag (int channel, double mag)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen0.p->sweep.mag = mag;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAPreGenSweepFreq (int channel, double freq1, double freq2)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen0.p->sweep.f1 = freq1;
	txa[channel].gen0.p->sweep.f2 = freq2;
	calc_sweep (txa[channel].gen0.p);
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAPreGenSweepRate (int channel, double rate)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen0.p->sweep.sweeprate = rate;
	calc_sweep (txa[channel].gen0.p);
	LeaveCriticalSection (&ch[channel].csDSP);
}

// 'PostGen', gen1

PORT
void SetTXAPostGenRun (int channel, int run)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen1.p->run = run;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAPostGenMode (int channel, int mode)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen1.p->mode = mode;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAPostGenToneMag (int channel, double mag)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen1.p->tone.mag = mag;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAPostGenToneFreq (int channel, double freq)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen1.p->tone.freq = freq;
	calc_tone (txa[channel].gen1.p);
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAPostGenTTMag (int channel, double mag1, double mag2)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen1.p->tt.mag1 = mag1;
	txa[channel].gen1.p->tt.mag2 = mag2;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAPostGenTTFreq (int channel, double freq1, double freq2)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen1.p->tt.f1 = freq1;
	txa[channel].gen1.p->tt.f2 = freq2;
	calc_tt (txa[channel].gen1.p);
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAPostGenSweepMag (int channel, double mag)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen1.p->sweep.mag = mag;
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAPostGenSweepFreq (int channel, double freq1, double freq2)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen1.p->sweep.f1 = freq1;
	txa[channel].gen1.p->sweep.f2 = freq2;
	calc_sweep (txa[channel].gen1.p);
	LeaveCriticalSection (&ch[channel].csDSP);
}

PORT
void SetTXAPostGenSweepRate (int channel, double rate)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].gen1.p->sweep.sweeprate = rate;
	calc_sweep (txa[channel].gen1.p);
	LeaveCriticalSection (&ch[channel].csDSP);
}