/*  txgain.c

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2015 Warren Pratt, NR0V

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

#include "cmcomm.h"

TXGAIN create_txgain(
	int run_fixed,
	int run_amp_protect,
	int size,
	double* in,
	double* out,
	double Igain,
	double Qgain,
	int ain4Value,
	int adc_supply,
	int (*GetAIN4Value) ()
	)
{
	TXGAIN a = (TXGAIN)malloc0(sizeof(txgain));
	if (run_fixed)
		InterlockedBitTestAndSet(&a->run_fixed, 0);
	if (run_amp_protect)
		InterlockedBitTestAndSet(&a->run_amp_protect, 0);
	a->size = size;
	a->in = in;
	a->out = out;
	a->Igain = Igain;
	a->Qgain = Qgain;
	a->ain4Value = ain4Value;
	a->adc_supply = adc_supply;
	a->GetAIN4Value = GetAIN4Value;
	InitializeCriticalSectionAndSpinCount(&a->cs_update, 2500);
	return a;
}

void destroy_txgain(TXGAIN a)
{
	DeleteCriticalSection(&a->cs_update);
	_aligned_free(a);
}

void xtxgain(TXGAIN a)
{
	int i;
	if (_InterlockedAnd(&a->run_fixed, 1))
	{
		EnterCriticalSection(&a->cs_update);
		for (i = 0; i < a->size; i++)
		{
			a->out[2 * i + 0] = a->Igain * a->in[2 * i + 0];
			a->out[2 * i + 1] = a->Qgain * a->in[2 * i + 1];
		}
		LeaveCriticalSection(&a->cs_update);
	}
	else if (a->out != a->in)
		memcpy(a->out, a->in, a->size * sizeof(complex));

	if (_InterlockedAnd(&a->run_amp_protect, 1))
	{
		const int thresh = 20;
		double ptn;
		a->ain4Value = (*a->GetAIN4Value)();
		if (a->ain4Value >= thresh)
		{
			a->ain4Value -= thresh;
			InterlockedBitTestAndSet(&a->amp_protect_warning, 0);
			switch (a->adc_supply)
			{
			case 33:
				ptn = 1.0 / pow (10.0, (double)a->ain4Value / 2730.0);
				break;
			case 50:
				ptn = 1.0 / pow (10.0, (double)a->ain4Value / 1802.0);
				break;
			default:
				ptn = 0.0;
				break;
			}
			for (i = 0; i < 2 * a->size; i++)
			{
				a->out[i] *= ptn;
			}
		}
	}
}


void SetTXGainSize(TXGAIN p, int size)
{
	p->size = size;
}

PORT
void SetTXFixedGainRun(int txid, int run)
{
	TXGAIN a = pcm->xmtr[txid].pgain;
	if (run)
		InterlockedBitTestAndSet(&a->run_fixed, 0);
	else
		InterlockedBitTestAndReset(&a->run_fixed, 0);
}

PORT
void SetTXFixedGain(int txid, double Igain, double Qgain)
{
	TXGAIN a = pcm->xmtr[txid].pgain;
	EnterCriticalSection(&a->cs_update);
	a->Igain = Igain;
	a->Qgain = Qgain;
	LeaveCriticalSection(&a->cs_update);
}

PORT
void SendpGetAIN4Value(int txid, int(*GetAIN4Value)())
{
	TXGAIN a = pcm->xmtr[txid].pgain;
	a->GetAIN4Value = GetAIN4Value;
}

PORT
int GetAndResetAmpProtect(int txid)
{
	TXGAIN a = pcm->xmtr[txid].pgain;
	return InterlockedBitTestAndReset(&a->amp_protect_warning, 0);
}

PORT
void SetAmpProtectRun(int txid, int run)
{
	TXGAIN a = pcm->xmtr[txid].pgain;
	if (run)
		InterlockedBitTestAndSet(&a->run_amp_protect, 0);
	else
		InterlockedBitTestAndReset(&a->run_amp_protect, 0);
}

PORT
void SetADCSupply(int txid, int v)
{
	TXGAIN a = pcm->xmtr[txid].pgain;
	a->adc_supply = v;
}
