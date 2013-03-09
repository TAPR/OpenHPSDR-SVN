/*  wcpAGC.c

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2011, 2012, 2013 Warren Pratt, NR0V

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

or by paper mail at

Warren Pratt
11303 Empire Grade
Santa Cruz, CA  95060

*/

#include <common.h>
#include <math.h>
#include <wcpAGC.h>

WCPAGC newWcpAGC (	AGCMODE mode,
					int pmode,
					COMPLEX *buff,
					int io_buffsize,
					REAL sample_rate,
					double tau_attack,
					double tau_decay,
					int n_tau,
					double max_gain,
					double var_gain,
					double fixed_gain,
					double max_input,
					double out_targ,
					double tau_fast_backaverage,
					double tau_fast_decay,
					double pop_ratio,
					double tau_hang_backmult,
					double hangtime,
					double hang_thresh,
					double tau_hang_decay,
					char *tag
				 )
{
	WCPAGC a;
	a = (WCPAGC) safealloc (1,sizeof(wcpagc), tag);
	//initialize per call parameters
	a->mode = mode;
	a->pmode = pmode;
	a->buff = buff;
	a->io_buffsize = io_buffsize;
	a->sample_rate = sample_rate;
	a->tau_attack = tau_attack;
	a->tau_decay = tau_decay;
	a->n_tau = n_tau;
	a->max_gain = max_gain;
	a->var_gain = var_gain;
	a->fixed_gain = fixed_gain;
	a->max_input = max_input;
	a->out_targ = out_targ;
	a->tau_fast_backaverage = tau_fast_backaverage;
	a->tau_fast_decay = tau_fast_decay;
	a->pop_ratio = pop_ratio;
	a->tau_hang_backmult = tau_hang_backmult;
	a->hangtime = hangtime;
	a->hang_thresh = hang_thresh;
	a->tau_hang_decay = tau_hang_decay;
	strcpy (a->tag, tag);
	//assign constants
	a->ring_buffsize = RB_SIZE;
	//do one-time initialization
	a->out_index = -1;
	a->ring_max = 0.0;
	a->volts = 0.0;
	a->save_volts = 0.0;
	a->fast_backaverage = 0.0;
	a->hang_backaverage = 0.0;
	a->hang_counter = 0;
	a->decay_type = 0;
	a->state = 0;

	loadWcpAGC (a);

	return a;
}

void loadWcpAGC (WCPAGC a)
{
	double tmp;
	//calculate internal parameters
	a->attack_buffsize = (int)ceil(a->sample_rate * a->n_tau * a->tau_attack);
	a->in_index = a->attack_buffsize + a->out_index;
	a->attack_mult = 1.0 - exp(-1.0 / (a->sample_rate * a->tau_attack));
	a->decay_mult = 1.0 - exp(-1.0 / (a->sample_rate * a->tau_decay));
	a->fast_decay_mult = 1.0 - exp(-1.0 / (a->sample_rate * a->tau_fast_decay));
	a->fast_backmult = 1.0 - exp(-1.0 / (a->sample_rate * a->tau_fast_backaverage));
	a->onemfast_backmult = 1.0 - a->fast_backmult;

	a->out_target = a->out_targ * (1.0 - exp(-a->n_tau)) * 0.99;
	a->min_volts = a->out_target / (a->var_gain * a->max_gain);

	tmp = log10(a->out_target / (a->max_input * a->var_gain * a->max_gain));
	if (tmp == 0.0)
		tmp = 1e-16;
	a->slope_constant = (a->out_target * (1.0 - 1.0 / a->var_gain)) / tmp;

	a->inv_max_input = 1.0 / a->max_input;

	tmp = pow (10.0, (a->hang_thresh - 1.0) / 0.125);
	a->hang_level = (a->max_input * tmp + (a->out_target / 
		(a->var_gain * a->max_gain)) * (1.0 - tmp)) * 0.637;

	a->hang_backmult = 1.0 - exp(-1.0 / (a->sample_rate * a->tau_hang_backmult));
	a->onemhang_backmult = 1.0 - a->hang_backmult;

	a->hang_decay_mult = 1.0 - exp(-1.0 / (a->sample_rate * a->tau_hang_decay));
}

void delWcpAGC (WCPAGC a)
{
	if (a)
		safefree ((char *) a);
}

void WcpAGC_flushbuf (WCPAGC a)
{
	memset ((void *)a->ring, 0, sizeof(double) * RB_SIZE * 2);
	a->ring_max = 0.0;
	memset ((void *)a->abs_ring, 0, sizeof(double)* RB_SIZE);
}

void WcpAGC (WCPAGC a)
{
	int i, j, k;
	double mult;

	if (a->mode == 0)
	{
		for (i = 0; i < a->io_buffsize; i++)
		{
			a->buff[i].re *= (float)a->fixed_gain;
			a->buff[i].im *= (float)a->fixed_gain;
		}
		return;
	}
	
	for (i = 0; i < a->io_buffsize; i++)
	{
		if (++a->out_index >= a->ring_buffsize)
			a->out_index -= a->ring_buffsize;
		if (++a->in_index >= a->ring_buffsize)
			a->in_index -= a->ring_buffsize;
	
		a->out_sample[0] = a->ring[a->out_index][0];
		a->out_sample[1] = a->ring[a->out_index][1];
		a->abs_out_sample = a->abs_ring[a->out_index];
		a->ring[a->in_index][0] = a->buff[i].re;
		a->ring[a->in_index][1] = a->buff[i].im;
		if (a->pmode == 0)
			a->abs_ring[a->in_index] = max(fabs(a->ring[a->in_index][0]), fabs(a->ring[a->in_index][1]));
		else
			a->abs_ring[a->in_index] = sqrt(a->ring[a->in_index][0] * a->ring[a->in_index][0] + a->ring[a->in_index][1] * a->ring[a->in_index][1]);

		a->fast_backaverage = a->fast_backmult * a->abs_out_sample + a->onemfast_backmult * a->fast_backaverage;
		a->hang_backaverage = a->hang_backmult * a->abs_out_sample + a->onemhang_backmult * a->hang_backaverage;

		if ((a->abs_out_sample >= a->ring_max) && (a->abs_out_sample > 0))
		{
			a->ring_max = 0.0;
			k = a->out_index;
			for (j = 0; j < a->attack_buffsize; j++)
			{
				if (++k == a->ring_buffsize)
					k = 0;
				if (a->abs_ring[k] > a->ring_max)
					a->ring_max = a->abs_ring[k];
			}
		}
		if (a->abs_ring[a->in_index] > a->ring_max)
			a->ring_max = a->abs_ring[a->in_index];

		if (a->hang_counter > 0)
			--a->hang_counter;

		switch (a->state)
		{
		case 0:
			{
				if (a->ring_max >= a->volts)
				{
					a->volts += (a->ring_max - a->volts) * a->attack_mult;
				}
				else
				{
					if (a->volts > a->pop_ratio * a->fast_backaverage)
					{
						a->state = 1;
						a->volts += (a->ring_max - a->volts) * a->fast_decay_mult;
					}
					else
					{
						if (a->hang_backaverage > a->hang_level)
						{
							a->state = 2;
							a->hang_counter = (int)(a->hangtime * a->sample_rate);
							a->decay_type = 1;
						}
						else
						{
							a->state = 3;
							a->volts += (a->ring_max - a->volts) * a->decay_mult;
							a->decay_type = 0;
						}
					}
				}
				break;
			}
		case 1:
			{
				if (a->ring_max >= a->volts)
				{
					a->state = 0;
					a->volts += (a->ring_max - a->volts) * a->attack_mult;
				}
				else
				{
					if (a->volts > a->save_volts)
					{
						a->volts += (a->ring_max - a->volts) * a->fast_decay_mult;
					}
					else
					{
						if (a->hang_counter > 0)
						{
							a->state = 2;
						}
						else
						{
							if (a->decay_type == 0)
							{
								a->state = 3;
								a->volts += (a->ring_max - a->volts) * a->decay_mult;
							}
							else
							{
								a->state = 4;
								a->volts += (a->ring_max - a->volts) * a->hang_decay_mult;
							}
						}
					}
				}
				break;
			}
		case 2:
			{
				if (a->ring_max >= a->volts)
				{
					a->state = 0;
					a->save_volts = a->volts;
					a->volts += (a->ring_max - a->volts) * a->attack_mult;
				}
				else
				{
					if (a->hang_counter == 0)
					{
						a->state = 4;
						a->volts += (a->ring_max - a->volts) * a->hang_decay_mult;
					}
				}
				break;
			}
		case 3:
			{
				if (a->ring_max >= a->volts)
				{
					a->state = 0;
					a->save_volts = a->volts;
					a->volts += (a->ring_max - a->volts) * a->attack_mult;
				}
				else
				{
					a->volts += (a->ring_max - a->volts) * a->decay_mult;
				}
				break;
			}
		case 4:
			{
				if (a->ring_max >= a->volts)
				{
					a->state = 0;
					a->save_volts = a->volts;
					a->volts += (a->ring_max - a->volts) * a->attack_mult;
				}
				else
				{
					a->volts += (a->ring_max - a->volts) * a->hang_decay_mult;
				}
				break;
			}
		}

		if (a->volts < a->min_volts)
			a->volts = a->min_volts;

		mult = (a->out_target - a->slope_constant * min(0.0, log10(a->inv_max_input * a->volts))) / a->volts;
		a->buff[i].re = (float)(a->out_sample[0] * mult);
		a->buff[i].im = (float)(a->out_sample[1] * mult);
	}
	return;
}

DttSP_EXP void
SetRXAGC (unsigned int thread, unsigned subrx, AGCMODE setit)
{
	sem_wait(&top[thread].sync.upd.sem);

	switch (setit)
	{
		case agcOFF:
			rx[thread][subrx].wcpagc.gen->mode = agcOFF;
			rx[thread][subrx].wcpagc.flag = TRUE;
			loadWcpAGC ( rx[thread][subrx].wcpagc.gen );
			break;
		case agcLONG:
			rx[thread][subrx].wcpagc.gen->mode = agcLONG;
			rx[thread][subrx].wcpagc.flag = TRUE;
			rx[thread][subrx].wcpagc.gen->hangtime = 2.000;
			rx[thread][subrx].wcpagc.gen->tau_decay = 2.000;
			loadWcpAGC ( rx[thread][subrx].wcpagc.gen );
			break;
		case agcSLOW:
			rx[thread][subrx].wcpagc.gen->mode = agcSLOW;
			rx[thread][subrx].wcpagc.flag = TRUE;
			rx[thread][subrx].wcpagc.gen->hangtime = 1.000;
			rx[thread][subrx].wcpagc.gen->tau_decay = 0.500;
			loadWcpAGC ( rx[thread][subrx].wcpagc.gen );
			break;
		case agcMED:
			rx[thread][subrx].wcpagc.gen->mode = agcMED;
			rx[thread][subrx].wcpagc.flag = TRUE;
			rx[thread][subrx].wcpagc.gen->hang_thresh = 1.0;
			rx[thread][subrx].wcpagc.gen->hangtime = 0.000;
			rx[thread][subrx].wcpagc.gen->tau_decay = 0.250;
			loadWcpAGC ( rx[thread][subrx].wcpagc.gen );
			break;
		case agcFAST:
			rx[thread][subrx].wcpagc.gen->mode = agcSLOW;
			rx[thread][subrx].wcpagc.flag = TRUE;
			rx[thread][subrx].wcpagc.gen->hang_thresh = 1.0;
			rx[thread][subrx].wcpagc.gen->hangtime = 0.000;
			rx[thread][subrx].wcpagc.gen->tau_decay = 0.050;
			loadWcpAGC ( rx[thread][subrx].wcpagc.gen );
			break;
		default:
			rx[thread][subrx].wcpagc.gen->mode = 5;
			rx[thread][subrx].wcpagc.flag = TRUE;
			break;
	}
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetRXAGCAttack (unsigned int thread, unsigned subrx, int attack)
{
	REAL tmp = (REAL)attack;
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].wcpagc.gen->tau_attack = (double)attack / 1000.0;
	loadWcpAGC ( rx[thread][subrx].wcpagc.gen );
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetRXAGCDecay (unsigned int thread, unsigned subrx, int decay)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].wcpagc.gen->tau_decay = (double)decay / 1000.0;
	loadWcpAGC ( rx[thread][subrx].wcpagc.gen );
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetRXAGCHang (unsigned int thread, unsigned subrx, int hang)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].wcpagc.gen->hangtime = (double)hang / 1000.0;
	loadWcpAGC ( rx[thread][subrx].wcpagc.gen );
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void				
GetRXAGCHangLevel(unsigned int thread, unsigned int subrx, double *hangLevel)
//for line on bandscope
{
	sem_wait(&top[thread].sync.upd.sem);
	*hangLevel = 20.0 * log10( rx[thread][subrx].wcpagc.gen->hang_level / 0.637 );
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void			
SetRXAGCHangLevel(unsigned int thread, unsigned int subrx, double hangLevel)
//for line on bandscope
{
	double convert, tmp;
	sem_wait(&top[thread].sync.upd.sem);
	if (rx[thread][subrx].wcpagc.gen->max_input > rx[thread][subrx].wcpagc.gen->min_volts)
	{
		convert = pow (10.0, hangLevel / 20.0);
		tmp = max(1e-8, (convert - rx[thread][subrx].wcpagc.gen->min_volts) / 
			(rx[thread][subrx].wcpagc.gen->max_input - rx[thread][subrx].wcpagc.gen->min_volts));
		rx[thread][subrx].wcpagc.gen->hang_thresh = 1.0 + 0.125 * log10 (tmp);
	}
	else
		rx[thread][subrx].wcpagc.gen->hang_thresh = 1.0;
	loadWcpAGC ( rx[thread][subrx].wcpagc.gen );
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void				
GetRXAGCHangThreshold(unsigned int thread, unsigned int subrx, int *hangthreshold)
//for slider in setup
{
	sem_wait(&top[thread].sync.upd.sem);
	*hangthreshold = (int)(100.0 * rx[thread][subrx].wcpagc.gen->hang_thresh);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetRXAGCHangThreshold (unsigned int thread, unsigned int subrx, int hangthreshold)
//For slider in setup
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].wcpagc.gen->hang_thresh = (double)hangthreshold / 100.0;
	loadWcpAGC ( rx[thread][subrx].wcpagc.gen );
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void				
GetRXAGCThresh(unsigned int thread, unsigned int subrx, double *thresh)
//for line on bandscope.
{
	double noise_offset = 10.0 * log10((rx[thread][subrx].filt.high - rx[thread][subrx].filt.low) 
		* uni[thread].spec.size / uni[thread].samplerate);
	sem_wait(&top[thread].sync.upd.sem);
	*thresh = 20.0 * log10( rx[thread][subrx].wcpagc.gen->min_volts ) - noise_offset;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void				
SetRXAGCThresh(unsigned int thread, unsigned int subrx, double thresh)
//for line on bandscope
{
	double noise_offset = 10.0 * log10((rx[thread][subrx].filt.high - rx[thread][subrx].filt.low) 
		* uni[thread].spec.size / uni[thread].samplerate);
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].wcpagc.gen->max_gain = rx[thread][subrx].wcpagc.gen->out_target / 
		(rx[thread][subrx].wcpagc.gen->var_gain * pow (10.0, (thresh + noise_offset) / 20.0));
	loadWcpAGC ( rx[thread][subrx].wcpagc.gen );
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void			
GetRXAGCTop(unsigned int thread, unsigned int subrx, double *max_agc)
//for AGC Max Gain in setup
{
	sem_wait(&top[thread].sync.upd.sem);
	*max_agc = 20 * log10 (rx[thread][subrx].wcpagc.gen->max_gain);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetRXAGCTop (unsigned int thread, unsigned int subrx, double max_agc)
//for AGC Max Gain in setup
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].wcpagc.gen->max_gain = pow (10.0, (double)max_agc / 20.0);
	loadWcpAGC ( rx[thread][subrx].wcpagc.gen );
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetRXAGCSlope (unsigned int thread, unsigned subrx, int slope)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].wcpagc.gen->var_gain = pow (10.0, (double)slope / 20.0 / 10.0);
	loadWcpAGC ( rx[thread][subrx].wcpagc.gen );
	sem_post(&top[thread].sync.upd.sem);
}


DttSP_EXP void
SetTXALCAttack (unsigned int thread, int attack)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].alc.gen->tau_attack = (double)attack / 1000.0;
	loadWcpAGC(tx[thread].alc.gen);
	sem_post(&top[thread].sync.upd.sem);
}
DttSP_EXP void
SetTXALCDecay (unsigned int thread, int decay)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].alc.gen->tau_decay = (double)decay / 1000.0;
	loadWcpAGC(tx[thread].alc.gen);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetTXALCBot (unsigned int thread, double max_agc)
{
	
}

DttSP_EXP void
SetTXALCHang (unsigned int thread, int decay)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].alc.gen->hangtime = (double)decay / 1000.0;
	loadWcpAGC(tx[thread].alc.gen);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetTXLevelerSt (unsigned int thread, BOOLEAN state)
{
	sem_wait(&top[thread].sync.upd.sem);

	tx[thread].leveler.flag = state;

	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void // (W5WC) ALC On/Off Control
SetTXALCSt (unsigned int thread, BOOLEAN state)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].alc.flag = state;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetTXLevelerAttack (unsigned int thread, int attack)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].leveler.gen->tau_attack = (double)attack / 1000.0;
	loadWcpAGC(tx[thread].leveler.gen);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetTXLevelerDecay (unsigned int thread, int decay)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].leveler.gen->tau_decay = (double)decay / 1000.0;
	loadWcpAGC(tx[thread].leveler.gen);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetTXLevelerHang (unsigned int thread, int decay)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].leveler.gen->hangtime = (double)decay / 1000.0;
	loadWcpAGC(tx[thread].leveler.gen);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetTXLevelerTop (unsigned int thread, double maxgain)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].leveler.gen->max_gain = pow (10.0,(double)maxgain / 20.0);
	loadWcpAGC(tx[thread].leveler.gen);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetFixedAGC (unsigned int thread, unsigned int subrx, double fixed_agc)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].wcpagc.gen->fixed_gain = pow (10.0, (double)fixed_agc / 20.0);
	loadWcpAGC ( rx[thread][subrx].wcpagc.gen );
	sem_post(&top[thread].sync.upd.sem);
}
