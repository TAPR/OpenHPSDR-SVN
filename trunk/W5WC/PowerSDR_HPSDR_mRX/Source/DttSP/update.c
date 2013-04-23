/* update.c

common defs and code for parm update

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2004, 2005, 2006, 2007 by Frank Brickle, AB2KT and Bob McGwier, N4HY
Copyright (C) 2011 FlexRadio Systems - Manual Notch Cardinality
Copyright (C) 2011, 2012 Warren Pratt, NR0V - Changes for AGC, ALC, Leveler, Compressor, ANF

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

The authors can be reached by email at

ab2kt@arrl.net
or
rwmcgwier@comcast.net

or by paper mail at

The DTTS Microwave Society
6 Kathleen Place
Bridgewater, NJ 08807
*/

#include <common.h>

////////////////////////////////////////////////////////////////////////////
// for commands affecting RX, which RX is Listening
unsigned int threadno=2;
unsigned int thread_com;

#define RL (uni[thread].multirx.lis)
#define asinh(value) (RanEAL)log(value + sqrt(value * value + 1))
////////////////////////////////////////////////////////////////////////////

PRIVATE REAL INLINE
dB2lin (REAL dB)
{
	return (REAL) pow (10.0, (REAL) dB / 20.0);
}

PRIVATE REAL INLINE
gmean (REAL x, REAL y)
{
	return (REAL)sqrt(x*y);
}

DttSP_EXP void
Setup_SDR (char *app_data_path)
{
	extern void setup (char *app_data_path);
	setup (app_data_path);
}

DttSP_EXP void
Destroy_SDR ()
{
	extern void closeup ();
	closeup ();
}


DttSP_EXP void
SetThreadNo(unsigned int setit)
{
	sem_wait(&top[0].sync.upd.sem);
	sem_wait(&top[1].sync.upd.sem);
	sem_wait(&top[2].sync.upd.sem);
	if ((setit > 0) && (setit<4)) threadno = setit;
	sem_post(&top[2].sync.upd.sem);
	sem_post(&top[1].sync.upd.sem);
	sem_post(&top[0].sync.upd.sem);
}

DttSP_EXP void
SetThreadCom(unsigned int thread)
{
	if (thread < 3)
	{
		sem_wait(&top[0].sync.upd.sem);
		sem_wait(&top[1].sync.upd.sem);
		sem_wait(&top[2].sync.upd.sem);
		thread_com = thread;
		sem_post(&top[2].sync.upd.sem);
		sem_post(&top[1].sync.upd.sem);
		sem_post(&top[0].sync.upd.sem);
	}
}

DttSP_EXP void 
SetSwchFlag(unsigned int thread, unsigned int val)
{
	//fprintf(stderr, "DttSP: SetSwchFlag(%u, %u)\n", thread, val), fflush(stderr);

	sem_wait(&top[thread].sync.upd.sem);
	top[thread].swch.flag = val;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetSwchRiseThresh(unsigned int thread, REAL val)
{
	// fprintf(stderr, "DttSP: SetSwchRiseThresh(%u, %f)\n", thread, val), fflush(stderr);

	sem_wait(&top[thread].sync.upd.sem);
	top[thread].swch.rise_thresh.threshold = val;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetThreadProcessingMode(unsigned int thread, RUNMODE runmode)
{
	sem_wait(&top[thread].sync.upd.sem);

	// this block here to eliminate glitching on 3K due to TR relay impulse
	if(top[thread].swch.flag && top[thread].state == RUN_MUTE && runmode == RUN_PLAY)
	{
		top[thread].swch.env.curr.type = SWCH_RISE;
		top[thread].swch.env.curr.cnt = 0;
		top[thread].swch.env.curr.val = 0.0;
		top[thread].swch.rise_thresh.flag = FALSE;
		top[thread].swch.rise_thresh.count = 0;
		top[thread].swch.rise_thresh.count_limit = (int)(uni[thread].samplerate * 0.050);
		//top[thread].swch.rise_thresh.threshold = 3e-4f;
		top[thread].swch.run.last = RUN_PLAY;
		top[thread].state = RUN_SWCH;		
	}
	else
	{
		top[thread].state = runmode;
	}

	/*if(runmode == RUN_MUTE) // added to clear the filter on TX->RX transitions
	{
		int k;

		for(k=0; k<2; k++)
		{
			reset_OvSv (rx[thread][k].filt.ovsv);
			DttSPAgc_flushbuf(rx[thread][k].dttspagc.gen);
		}
	}*/

	sem_post(&top[thread].sync.upd.sem);
	//fprintf(stderr,"thread: %0u setting mode to %0d\n", thread, (int)runmode),fflush(stderr);
}

DttSP_EXP int
SetMode (unsigned int thread, unsigned int subrx, SDRMODE m)
{
	int rtn=0;
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].mode = m;
	if (tx[thread].mode == LSB) tx[thread].hlb.gen->invert = TRUE;
	else tx[thread].hlb.gen->invert = FALSE;
	rx[thread][subrx].mode = m;
	if (m == SAM) rx[thread][subrx].amd.gen->mode = 1;		//NR0V
	if (m == AM) rx[thread][subrx].amd.gen->mode = 0;		//NR0V
	sem_post(&top[thread].sync.upd.sem);
	return rtn;
}

DttSP_EXP void
AudioReset (void)
{
	extern BOOLEAN reset_em;
	//fprintf(stdout,"AudioReset: reset_em = TRUE\n"), fflush(stdout);
	reset_em = TRUE;
}

DttSP_EXP void
SetRXManualNotchEnable(unsigned int thread, unsigned int subrx, unsigned int index, BOOLEAN setit)
{
	fprintf(stderr, "DttSP::SetRXManualNotchEnable(%u, %u, %u, %u)\n", thread, subrx, index, setit);
	fflush(stderr);

	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].notch[index].flag = setit;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetRXManualNotchBW(unsigned int thread, unsigned int subrx, unsigned int index, double BW)
{
	//REAL w0,sw,cw,alpha;
	fprintf(stderr, "DttSP::SetRXManualNotchBW(%u, %u, %u, %lf)\n", thread, subrx, index, BW);
	fflush(stderr);

	/*w0 = (REAL)(2*M_PI*rx[thread][subrx].notch[index].gen->F0/rx[thread][subrx].notch[index].gen->Fs);
	sw = (REAL)sin(w0);
	cw = (REAL)cos(w0);
	rx[thread][subrx].notch[index].gen->BW = (REAL)BW;
	alpha = (REAL)(sw*sinh(log(2)*0.5*rx[thread][subrx].notch[index].gen->BW*w0/sw));
	rx[thread][subrx].notch[index].gen->Q = 1/(2*sinh(log(2)/2 * BW*w0/sw));*/

	sem_wait(&top[thread].sync.upd.sem);
	/*rx[thread][subrx].notch[index].gen->BW = (REAL)(2*sw*asinh(1/(2*rx[thread][subrx].notch[index].gen->Q))/log(2)/w0);
	rx[thread][subrx].notch[index].gen->B[0] = 1/(1+alpha);
	rx[thread][subrx].notch[index].gen->B[1] =  -2*cw/(1+alpha);
	rx[thread][subrx].notch[index].gen->B[2] = 1/(1+alpha);
	rx[thread][subrx].notch[index].gen->A[0] = 1;
	rx[thread][subrx].notch[index].gen->A[1] = -2*cw/(1+alpha);
	rx[thread][subrx].notch[index].gen->A[2] = (1 - alpha)/(1+alpha);*/

	rx[thread][subrx].notch[index].gen->BW = (REAL)BW;
	
	sem_post(&top[thread].sync.upd.sem);

}
DttSP_EXP void
SetRXManualNotchFreq(unsigned int thread, unsigned int subrx, unsigned int index, double F0)
{
	REAL w0,sw,cw,alpha;
	fprintf(stderr, "DttSP::SetRXManualNotchFreq(%u, %u, %u, %lf)\n", thread, subrx, index, F0);
	fflush(stderr);

	w0 = (REAL)(2*M_PI*F0/rx[thread][subrx].notch[index].gen->Fs);
	sw = (REAL)sin(w0);
	cw = (REAL)cos(w0);
	//alpha = (REAL)(sw*sinh(log(2)*0.5*rx[thread][subrx].notch[index].gen->BW*w0/sw));
	alpha = (REAL)(sw/(2*F0/rx[thread][subrx].notch[index].gen->BW));

	sem_wait(&top[thread].sync.upd.sem);
	//rx[thread][subrx].notch[index].gen->BW = (REAL)(2*sw*asinh(1/(2*rx[thread][subrx].notch[index].gen->Q))/log(2)/w0);
	rx[thread][subrx].notch[index].gen->B[0] = 1/(1+alpha);
	rx[thread][subrx].notch[index].gen->B[1] =  -2*cw/(1+alpha);
	rx[thread][subrx].notch[index].gen->B[2] = 1/(1+alpha);
	rx[thread][subrx].notch[index].gen->A[0] = 1;
	rx[thread][subrx].notch[index].gen->A[1] = -2*cw/(1+alpha);
	rx[thread][subrx].notch[index].gen->A[2] = (1 - alpha)/(1+alpha);
	
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetRXDCBlock(unsigned int thread, unsigned int subrx, BOOLEAN setit)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].dcb->flag = setit;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetRXDCBlockGain(unsigned int thread, unsigned int subrx, REAL gain)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].dcb->gain = gain;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetTXDCBlock (unsigned int thread, BOOLEAN setit)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].dcb.flag = setit;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetTXFMDeviation(unsigned int thread, double deviation)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].fm.cvtmod2freq = (REAL) (deviation * TWOPI / uni[thread].samplerate);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetRXFMDeviation(unsigned int thread, unsigned int k, double deviation)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][k].fm.gen->deviation = (REAL)deviation;
	rx[thread][k].fm.gen->cvt = (REAL)(uni[thread].samplerate / (deviation * TWOPI));
	fprintf(stderr, "dttsp SetRXFMDeviation: %f\n", deviation);
	fflush(stderr);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetCTCSSFreq(unsigned int thread, double freq_hz)
{
	sem_wait(&top[thread].sync.upd.sem);
	//tx[thread].fm.ctcss.freq_hz = (REAL)freq_hz;
	OSCfreq(tx[thread].fm.ctcss.osc) = TWOPI * (REAL)freq_hz / uni[thread].samplerate;
	sem_post(&top[thread].sync.upd.sem);

}

DttSP_EXP void
SetCTCSSFlag(unsigned int thread, BOOLEAN flag)
{
	sem_wait(&top[thread].sync.upd.sem);
	//tx[thread].fm.ctcss.freq_hz = (REAL)freq_hz;
	tx[thread].fm.ctcss.flag = flag;
	//fprintf(stderr, "dttsp SETCTCSSFlag: %d\n", flag);
	//fflush(stderr);
	sem_post(&top[thread].sync.upd.sem);

}

DttSP_EXP void
SetFMSquelchThreshold(unsigned int thread, unsigned int k, REAL threshold)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][k].fm.gen->squelch_threshold_weak = threshold;
	rx[thread][k].fm.gen->squelch_threshold_unmute = threshold*0.9f;
	rx[thread][k].fm.gen->squelch_threshold_strong = threshold*0.5f;
	//fprintf(stderr, "dttsp SetFMSquelchThreshold: %f\n", threshold);
	//fflush(stderr);
	sem_post(&top[thread].sync.upd.sem);
}


DttSP_EXP int // (NR0V) modified
SetTXFilter (unsigned int thread, double low_frequency, double high_frequency)
{
	int ncoef = uni[thread].buflen + 1;
	int i, fftlen = 2 * uni[thread].buflen, rtn=0;
	fftwf_plan ptmp, ptmp_pre; //
	COMPLEX *zcvec;

	if (fabs (low_frequency) >= 0.5 * uni[thread].samplerate)
		rtn = -1;
	if (fabs (high_frequency) >= 0.5 * uni[thread].samplerate)
		rtn = -2;
	if ((low_frequency + 10) >= high_frequency)
		rtn = -3;

	if (rtn == 0)
	{
		sem_wait(&top[thread].sync.upd.sem);
		delFIR_COMPLEX (tx[thread].filt.coef);

		tx[thread].filt.coef = newFIR_Bandpass_COMPLEX ((REAL)low_frequency,
			(REAL)high_frequency,
			uni[thread].samplerate, ncoef);

		zcvec = newvec_COMPLEX (fftlen, "filter z vec in setFilter");
		ptmp = fftwf_plan_dft_1d (fftlen,
			(fftwf_complex *) zcvec,
			(fftwf_complex *) tx[thread].filt.ovsv->zfvec,
			FFTW_FORWARD, uni[thread].wisdom.bits);
		ptmp_pre = fftwf_plan_dft_1d (fftlen, //
			(fftwf_complex *) zcvec,
			(fftwf_complex *) tx[thread].filt.ovsv_pre->zfvec,
			FFTW_FORWARD, uni[thread].wisdom.bits);
#ifdef LHS
		for (i = 0; i < ncoef; i++)
			zcvec[i] = tx[thread].filt.coef->coef[i];
#else
		for (i = 0; i < ncoef; i++)
			zcvec[fftlen - ncoef + i] = tx[thread].filt.coef->coef[i];
#endif
		fftwf_execute (ptmp);
		fftwf_execute (ptmp_pre); //
		fftwf_destroy_plan (ptmp);
		fftwf_destroy_plan (ptmp_pre); //
		delvec_COMPLEX (zcvec);
		normalize_vec_COMPLEX (tx[thread].filt.ovsv->zfvec, tx[thread].filt.ovsv->fftlen,tx[thread].filt.ovsv->scale);
		normalize_vec_COMPLEX (tx[thread].filt.ovsv_pre->zfvec, tx[thread].filt.ovsv_pre->fftlen,tx[thread].filt.ovsv_pre->scale); //
		memcpy ((char *) tx[thread].filt.save, (char *) tx[thread].filt.ovsv->zfvec,
			tx[thread].filt.ovsv->fftlen * sizeof (COMPLEX));

		sem_post(&top[thread].sync.upd.sem);
	}
	return rtn;
}

DttSP_EXP int  // (NR0V) modified
SetRXFilter (unsigned int thread, unsigned int subrx, double low_frequency, double high_frequency)
{
	int ncoef = uni[thread].buflen + 1;
	int i, fftlen = 2 * uni[thread].buflen,rtn=0;
	fftwf_plan ptmp, ptmp_notch;	//
	COMPLEX *zcvec;

	//fprintf(stderr, "DSP: SetRXFilter(%u, %u, %f, %f)\n", thread, subrx, low_frequency, high_frequency);
	//fflush(stderr);

	rx[thread][subrx].filt.low = low_frequency;  //NR0V
	rx[thread][subrx].filt.high = high_frequency;
	
	if (fabs (low_frequency) >= 0.5 * uni[thread].samplerate)
		rtn = -1;
	if (fabs (high_frequency) >= 0.5 * uni[thread].samplerate)
		rtn = -2;
	if ((low_frequency + 10) >= high_frequency)
		rtn = -3;

	if (rtn == 0)
	{
		sem_wait(&top[thread].sync.upd.sem);
		delFIR_COMPLEX (rx[thread][subrx].filt.coef);

		rx[thread][subrx].filt.coef = newFIR_Bandpass_COMPLEX ((REAL)low_frequency,
			(REAL)high_frequency,
			uni[thread].samplerate, ncoef);

		zcvec = newvec_COMPLEX (fftlen, "filter z vec in setFilter");
		ptmp = fftwf_plan_dft_1d (fftlen,
			(fftwf_complex *) zcvec,
			(fftwf_complex *) rx[thread][subrx].filt.ovsv->zfvec,
			FFTW_FORWARD, uni[thread].wisdom.bits);
		ptmp_notch = fftwf_plan_dft_1d (fftlen,	//
			(fftwf_complex *) zcvec,
			(fftwf_complex *) rx[thread][subrx].filt.ovsv_notch->zfvec,
			FFTW_FORWARD, uni[thread].wisdom.bits);
#ifdef LHS
		for (i = 0; i < ncoef; i++)
			zcvec[i] = rx[thread][subrx].filt.coef->coef[i];
#else
		for (i = 0; i < ncoef; i++)
			zcvec[fftlen - ncoef + i] = rx[thread][subrx].filt.coef->coef[i];
#endif
		fftwf_execute (ptmp);
		fftwf_execute (ptmp_notch);	//
		fftwf_destroy_plan (ptmp);
		fftwf_destroy_plan (ptmp_notch);	//
		delvec_COMPLEX (zcvec);
		normalize_vec_COMPLEX (rx[thread][subrx].filt.ovsv->zfvec, rx[thread][subrx].filt.ovsv->fftlen,
			rx[thread][subrx].filt.ovsv->scale);
		normalize_vec_COMPLEX (rx[thread][subrx].filt.ovsv_notch->zfvec, rx[thread][subrx].filt.ovsv_notch->fftlen,	//
			rx[thread][subrx].filt.ovsv_notch->scale);
		memcpy ((char *) rx[thread][subrx].filt.save, (char *) rx[thread][subrx].filt.ovsv->zfvec,
			rx[thread][subrx].filt.ovsv->fftlen * sizeof (COMPLEX));

		sem_post(&top[thread].sync.upd.sem);
	}
	return rtn;
 }

DttSP_EXP void
Release_Update ()
{
	sem_post (&top[0].sync.upd.sem);
	sem_post (&top[1].sync.upd.sem);
	sem_post (&top[2].sync.upd.sem);
}

DttSP_EXP void
SetRXOutputGain(unsigned int thread, unsigned int subrx, double g)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].output_gain = (REAL)g;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetOscPhase(double phase)
{
	int i,j;
	sem_wait(&top[0].sync.upd.sem);
	sem_wait(&top[1].sync.upd.sem);
	sem_wait(&top[2].sync.upd.sem);
	for(i=0;i<3;i++) {
		for(j=0;j<uni[i].multirx.nrx;j++) rx[i][j].osc.phase = phase;
		tx[i].osc.phase = phase;
	}
	sem_post(&top[2].sync.upd.sem);
	sem_post(&top[1].sync.upd.sem);
	sem_post(&top[0].sync.upd.sem);
}

DttSP_EXP int
SetRXOsc (unsigned int thread, unsigned subrx, double newfreq)
{
	if (fabs (newfreq) >= 0.5 * uni[thread].samplerate)
		return -1;

	newfreq *= 2.0 * M_PI / uni[thread].samplerate;
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].osc.gen->Frequency = (REAL)newfreq;
	sem_post(&top[thread].sync.upd.sem);
	return 0;
}

DttSP_EXP int
SetTXOsc (unsigned int thread, double newfreq)
{
	if (fabs (newfreq) >= 0.5 * uni[thread].samplerate)
		return -1;

	newfreq *= 2.0 * M_PI / uni[thread].samplerate;
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].osc.gen->Frequency = (REAL)newfreq;
	sem_post(&top[thread].sync.upd.sem);
	return 0;
}

DttSP_EXP int
SetSampleRate (double newSampleRate)
{
	extern int reset_for_samplerate (REAL);
	int rtn = -1;

	unsigned int thread;
	REAL samplerate = (REAL)newSampleRate;


	for(thread = 0;thread < 3; thread++)
	{
		top[thread].susp = TRUE;
		sem_wait (&top[thread].sync.upd.sem);
	}
	if (samplerate != uni[0].samplerate)
	{
		if (reset_for_samplerate (samplerate) != -1)
			rtn = 0;
	}
	for(thread = 0;thread < 3; thread++)
		top[thread].susp = FALSE;
	sem_post (&top[2].sync.upd.sem);
	sem_post (&top[1].sync.upd.sem);
	sem_post (&top[0].sync.upd.sem);
	return rtn;
}

DttSP_EXP void
SetTXSquelchSt (unsigned int thread, BOOLEAN setit)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].squelch.flag = setit;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetTXSquelchVal (unsigned int thread, float setit)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].squelch.thresh = setit;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetTXSquelchAtt (unsigned int thread, float setit)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].squelch.atten = setit;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetNB (unsigned int thread, unsigned subrx, BOOLEAN setit)
{
	sem_wait(&top[thread].sync.upd.sem);

	rx[thread][subrx].nb.flag = setit;

	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetNBvals (unsigned int thread, unsigned subrx, double threshold)
{
	sem_wait(&top[thread].sync.upd.sem);

	rx[thread][subrx].nb.gen->threshold = (REAL)threshold;

	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetSDROM (unsigned int thread, unsigned subrx, BOOLEAN setit)
{
	sem_wait(&top[thread].sync.upd.sem);

	rx[thread][subrx].nb_sdrom.flag = setit;

	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetSDROMvals (unsigned int thread, unsigned subrx, double threshold)
{
	sem_wait(&top[thread].sync.upd.sem);

	rx[thread][subrx].nb_sdrom.gen->threshold = (REAL)threshold;

	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetBIN (unsigned int thread, unsigned subrx, BOOLEAN setit)
{
	sem_wait(&top[thread].sync.upd.sem);

	rx[thread][subrx].bin.flag = setit;

	sem_post(&top[thread].sync.upd.sem);
}


DttSP_EXP void
SetTXAMCarrierLevel (unsigned int thread, double setit)
{
	sem_wait(&top[thread].sync.upd.sem);

	tx[thread].am.carrier_level = (REAL)setit;

	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetCorrectIQ (unsigned int thread, unsigned int subrx, double phase, double gain)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].iqfix->phase = (REAL) (0.001 * phase);
	rx[thread][subrx].iqfix->gain = (REAL) (1.0 + 0.001 * gain);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetCorrectIQGain (unsigned int thread, unsigned int subrx, double gain)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].iqfix->gain = (REAL) (1.0 + 0.001 * gain);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetCorrectIQPhase (unsigned int thread, unsigned int subrx, double phase)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].iqfix->phase = (REAL) (0.001 * phase);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetCorrectIQEnable(int setit)
{
	extern int IQdoit;
	sem_wait(&top[0].sync.upd.sem);

	IQdoit = setit;
	//fprintf(stderr,"setit = %d\n",setit),fflush(stderr);
	sem_post(&top[0].sync.upd.sem);
}

DttSP_EXP void
SetCorrectRXIQMu (unsigned int thread, unsigned int subrx, double mu)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].iqfix->mu = (REAL)mu;
	//memset((void *)rx[thread][subrx].iqfix->w,0,16*sizeof(COMPLEX));
	//memset((void *)rx[thread][subrx].iqfix->del,0,16*sizeof(COMPLEX));
	//memset((void *)rx[thread][subrx].iqfix->y,0,16*sizeof(COMPLEX));
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP REAL
GetCorrectRXIQMu(unsigned int thread, unsigned int subrx)
{
	return rx[thread][subrx].iqfix->mu;
}

DttSP_EXP void
SetCorrectRXIQw (unsigned int thread, unsigned int subrx, REAL wr, REAL wi, unsigned int index)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].iqfix->w[index] = Cmplx((REAL)wr,(REAL)wi);
	if (index == 0) memset((void *)&rx[thread][subrx].iqfix->w[1],0,15*sizeof(COMPLEX));
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
GetCorrectRXIQw(int thread, int subrx, REAL *realw, REAL *imagw, unsigned int index)
{
	sem_wait(&top[thread].sync.upd.sem);
	*realw = rx[thread][subrx].iqfix->w[index].re;
	*imagw = rx[thread][subrx].iqfix->w[index].im;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetCorrectRXIQwReal (unsigned int thread, unsigned int subrx, REAL wr, unsigned int index)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].iqfix->w[index].re = (REAL)wr;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetCorrectRXIQwImag (unsigned int thread, unsigned int subrx, REAL wi, unsigned int index)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].iqfix->w[index].im = (REAL)wi;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetCorrectTXIQ (unsigned int thread, double phase, double gain)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].iqfix->phase = (REAL) (0.001 * phase);
	tx[thread].iqfix->gain = (REAL) (1.0 + 0.001 * gain);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetCorrectTXIQGain (unsigned int thread, double gain)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].iqfix->gain = (REAL) (1.0 + 0.001 * gain);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetCorrectTXIQPhase (unsigned int thread, double phase)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].iqfix->phase = (REAL) (0.001 * phase);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetCorrectTXIQMu (unsigned int thread, double mu)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].iqfix->mu = (REAL)mu;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetCorrectTXIQW (unsigned int thread, double wr, double wi)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].iqfix->w[0] = Cmplx((REAL)wr,(REAL)wi);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetPWSmode (unsigned thread, unsigned subrx, int setit)
{
	if (rx[thread][subrx].mode == SPEC)
		setit = SPEC_SEMI_RAW;

	sem_wait(&top[thread].sync.upd.sem);
	switch (setit)
	{
		case 0:
			uni[thread].spec.type = SPEC_POST_FILT;
			break;
		case 1:
			uni[thread].spec.type = SPEC_PRE_FILT;
			break;
		case 2:
			uni[thread].spec.type = SPEC_SEMI_RAW;
			break;
		case 4:
			uni[thread].spec.type = SPEC_POST_DET;
			break;
		default:
			break;
	}
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetWindow (unsigned int thread, Windowtype window)
{
	sem_wait(&top[thread].sync.upd.sem);
	if (!uni[thread].spec.polyphase)
		makewindow (window, uni[thread].spec.size, uni[thread].spec.window);
	uni[thread].spec.wintype = window;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetSpectrumPolyphase (unsigned int thread, BOOLEAN setit)
{
	sem_wait(&top[thread].sync.upd.sem);
	if (uni[thread].spec.polyphase != setit)
	{
		if (setit)
		{
			uni[thread].spec.polyphase = TRUE;
			uni[thread].spec.mask = (8 * uni[thread].spec.size) - 1;
			{
				RealFIR WOLAfir;
				REAL MaxTap = 0;
				int i;
				WOLAfir =
					newFIR_Lowpass_REAL (1.0, (REAL) uni[thread].spec.size,
					8 * uni[thread].spec.size - 1);
				memset (uni[thread].spec.window, 0, 8 * sizeof (REAL) * uni[thread].spec.size);
				memcpy (uni[thread].spec.window, FIRcoef (WOLAfir),
					sizeof (REAL) * (8 * uni[thread].spec.size - 1));
				for (i = 0; i < 8 * uni[thread].spec.size; i++)
					MaxTap = max (MaxTap, (REAL) fabs (uni[thread].spec.window[i]));
				MaxTap = 0.707107f / MaxTap;
				for (i = 0; i < 8 * uni[thread].spec.size; i++)
					uni[thread].spec.window[i] *= MaxTap;
				delFIR_REAL (WOLAfir);
			}
		}
		else
		{
			uni[thread].spec.polyphase = FALSE;
			uni[thread].spec.mask = uni[thread].spec.size - 1;
			memset (uni[thread].spec.window, 0, sizeof (REAL) * uni[thread].spec.size);
			makewindow (uni[thread].spec.wintype, uni[thread].spec.size - 1, uni[thread].spec.window);
		}
		reinit_spectrum (&uni[thread].spec);
	}
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetGrphTXEQ (unsigned int thread, int *txeq)
{
	int i;
	fftwf_plan ptmp;
	COMPLEX *filtcoef, *tmpcoef;
	ComplexFIR tmpfilt;
	REAL preamp, gain[3];

	EQ a = tx[thread].grapheq.gen;

	filtcoef = newvec_COMPLEX (a->eqsize, "filter for EQ");
	tmpcoef = newvec_COMPLEX (a->eqsize/2 + 1, "tmp filter for EQ");
	preamp  = (REAL) dB2lin ((REAL) txeq[0]) * 0.5f;
	gain[0] = (REAL) dB2lin ((REAL) txeq[1]) * preamp;
	gain[1] = (REAL) dB2lin ((REAL) txeq[2]) * preamp;
	gain[2] = (REAL) dB2lin ((REAL) txeq[3]) * preamp;

	sem_wait(&top[thread].sync.upd.sem);

	tmpfilt = newFIR_Bandpass_COMPLEX (-400, 400, uni[thread].samplerate, a->eqsize/2 + 1);
	for (i = 0; i < a->eqsize/2 + 1; i++)
		tmpcoef[i] = Cscl (tmpfilt->coef[i], (REAL)gain[0]);
	delFIR_Bandpass_COMPLEX (tmpfilt);

	tmpfilt = newFIR_Bandpass_COMPLEX (400, 1500, uni[thread].samplerate, a->eqsize/2 + 1);
	for (i = 0; i < a->eqsize/2 + 1; i++)
		tmpcoef[i] = Cadd (tmpcoef[i], Cscl (tmpfilt->coef[i], (REAL)gain[1]));
	delFIR_Bandpass_COMPLEX (tmpfilt);

	tmpfilt = newFIR_Bandpass_COMPLEX (-1500, -400, uni[thread].samplerate, a->eqsize/2 + 1);
	for (i = 0; i < a->eqsize/2 + 1; i++)
		tmpcoef[i] = Cadd (tmpcoef[i], Cscl (tmpfilt->coef[i], (REAL)gain[1]));
	delFIR_Bandpass_COMPLEX (tmpfilt);

	tmpfilt = newFIR_Bandpass_COMPLEX (1500, 6000, uni[thread].samplerate, a->eqsize/2 + 1);
	for (i = 0; i < a->eqsize/2 + 1; i++)
		tmpcoef[i] = Cadd (tmpcoef[i], Cscl (tmpfilt->coef[i], (REAL)gain[2]));
	delFIR_Bandpass_COMPLEX (tmpfilt);

	tmpfilt = newFIR_Bandpass_COMPLEX (-6000, -1500, uni[thread].samplerate, a->eqsize/2 + 1);
	for (i = 0; i < a->eqsize/2 + 1; i++)
		tmpcoef[i] = Cadd (tmpcoef[i], Cscl (tmpfilt->coef[i], (REAL)gain[2]));
	delFIR_Bandpass_COMPLEX (tmpfilt);
	for (i = 0; i < a->eqsize/2 + 1; i++)
		filtcoef[a->eqsize/2 - 1 + i] = Cscl(tmpcoef[i],(REAL)(1.0/(double)a->eqsize));
	ptmp =
		fftwf_plan_dft_1d (a->eqsize, (fftwf_complex *) filtcoef,
		(fftwf_complex *) tx[thread].grapheq.gen->p->zfvec,
		FFTW_FORWARD, uni[thread].wisdom.bits);

	fftwf_execute (ptmp);
	fftwf_destroy_plan (ptmp);
	delvec_COMPLEX (filtcoef);
	delvec_COMPLEX(tmpcoef);

	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetGrphTXEQ10(unsigned int thread, int *txeq) 
{
/*  if (n < 11)
    return -1;
  else {
    int band, i, j;
    fftwf_plan ptmp;
    ComplexFIR tmpfilt;
    COMPLEX *filtcoef = newvec_COMPLEX(512, "filter for EQ"),
            *tmpcoef  = newvec_COMPLEX(257, "tmp filter for EQ");
    REAL preamp = dB2lin(atof(p[0])) * 0.5; */

    int i,j,band;
	fftwf_plan ptmp;
	EQ a = tx[thread].grapheq.gen;
	COMPLEX *filtcoef = newvec_COMPLEX(a->eqsize, "filter for EQ"),
		*tmpcoef  = newvec_COMPLEX(a->eqsize/2 + 1, "tmp filter for EQ");
	ComplexFIR tmpfilt;
	REAL preamp;

	preamp = dB2lin((REAL) txeq[0]) * 0.5f;

	for (j = 1, band = 15; j <= 10; j++, band += 3) 
	{
		REAL f_here  = ISOband_get_nominal(band),
			f_below = gmean(f_here / 2.0f, f_here),
			f_above = gmean(f_here, f_here * 2.0f),
			g_here  = dB2lin((REAL) txeq[j]) * preamp;

		tmpfilt = newFIR_Bandpass_COMPLEX(-f_above, -f_below, uni->samplerate, a->eqsize/2 + 1);
		for (i = 0; i < a->eqsize/2 + 1; i++)
			tmpcoef[i] = Cadd(tmpcoef[i], Cscl(tmpfilt->coef[i], g_here));
		delFIR_Bandpass_COMPLEX(tmpfilt);

		tmpfilt = newFIR_Bandpass_COMPLEX(f_below, f_above, uni->samplerate, a->eqsize/2 + 1);
		for (i = 0; i < a->eqsize/2 + 1; i++)
			tmpcoef[i] = Cadd(tmpcoef[i], Cscl(tmpfilt->coef[i], g_here));
		delFIR_Bandpass_COMPLEX(tmpfilt);
	}

	for (i = 0; i < a->eqsize/2 + 1; i++)
		filtcoef[a->eqsize/2 - 2 + i] = Cscl(tmpcoef[i],(REAL)(1.0/(double)a->eqsize));

    ptmp = fftwf_plan_dft_1d(a->eqsize,
		(fftwf_complex *) filtcoef,
		(fftwf_complex *) tx[thread].grapheq.gen->p->zfvec,
		FFTW_FORWARD,
		uni->wisdom.bits);

	fftwf_execute(ptmp);
	fftwf_destroy_plan(ptmp);
	delvec_COMPLEX(filtcoef);
	delvec_COMPLEX(tmpcoef);
}


DttSP_EXP void
SetGrphTXEQcmd (unsigned int thread, BOOLEAN state)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].grapheq.flag = state;
	sem_post(&top[thread].sync.upd.sem);
}


DttSP_EXP void
SetNotch160 (unsigned int thread, BOOLEAN state)
{

}

DttSP_EXP void
SetGrphRXEQ (unsigned int thread, unsigned int subrx, int *rxeq)
{
	int i;
    fftwf_plan ptmp;
    COMPLEX *filtcoef, *tmpcoef;
    ComplexFIR tmpfilt;
    REAL preamp, gain[3];

	EQ a = rx[thread][subrx].grapheq.gen;

	filtcoef = newvec_COMPLEX (a->eqsize, "filter for EQ");
	tmpcoef = newvec_COMPLEX (a->eqsize/2 + 1, "tmp filter for EQ");
    preamp  = (REAL) dB2lin ((REAL) rxeq[0]) * 0.5f;
    gain[0] = (REAL) dB2lin ((REAL) rxeq[1]) * preamp;
    gain[1] = (REAL) dB2lin ((REAL) rxeq[2]) * preamp;
    gain[2] = (REAL) dB2lin ((REAL) rxeq[3]) * preamp;


    sem_wait(&top[thread].sync.upd.sem);

    tmpfilt = newFIR_Bandpass_COMPLEX (-400, 400, uni[thread].samplerate, a->eqsize/2 + 1);
    for (i = 0; i < a->eqsize/2 + 1; i++)
		tmpcoef[i] = Cscl (tmpfilt->coef[i], (REAL)gain[0]);
    delFIR_Bandpass_COMPLEX (tmpfilt);

    tmpfilt = newFIR_Bandpass_COMPLEX (400, 1500, uni[thread].samplerate, a->eqsize/2 + 1);
    for (i = 0; i < a->eqsize/2 + 1; i++)
		tmpcoef[i] = Cadd (tmpcoef[i], Cscl (tmpfilt->coef[i], (REAL)gain[1]));
    delFIR_Bandpass_COMPLEX (tmpfilt);

    tmpfilt = newFIR_Bandpass_COMPLEX (-1500, -400, uni[thread].samplerate, a->eqsize/2 + 1);
    for (i = 0; i < a->eqsize/2 + 1; i++)
		tmpcoef[i] = Cadd (tmpcoef[i], Cscl (tmpfilt->coef[i], (REAL)gain[1]));
    delFIR_Bandpass_COMPLEX (tmpfilt);

    tmpfilt = newFIR_Bandpass_COMPLEX (1500, 6000, uni[thread].samplerate, a->eqsize/2 + 1);
    for (i = 0; i < a->eqsize/2 + 1; i++)
		tmpcoef[i] = Cadd (tmpcoef[i], Cscl (tmpfilt->coef[i], (REAL)gain[2]));
    delFIR_Bandpass_COMPLEX (tmpfilt);

    tmpfilt = newFIR_Bandpass_COMPLEX (-6000, -1500, uni[thread].samplerate, a->eqsize/2 + 1);
    for (i = 0; i < a->eqsize/2 + 1; i++)
		tmpcoef[i] = Cadd (tmpcoef[i], Cscl (tmpfilt->coef[i], (REAL)gain[2]));
    delFIR_Bandpass_COMPLEX (tmpfilt);
    for (i = 0; i < a->eqsize/2 + 1; i++)
		filtcoef[a->eqsize/2 - 1 + i] = Cscl(tmpcoef[i],(REAL)(1.0/(double)a->eqsize));
	ptmp =
		fftwf_plan_dft_1d (a->eqsize, (fftwf_complex *) filtcoef,
		(fftwf_complex *) rx[thread][subrx].grapheq.gen->p->zfvec,
		FFTW_FORWARD, uni[thread].wisdom.bits);

	fftwf_execute (ptmp);
    fftwf_destroy_plan (ptmp);
    delvec_COMPLEX (filtcoef);
	delvec_COMPLEX(tmpcoef);
	
	sem_post(&top[thread].sync.upd.sem);
	//fprintf(stderr,"%f %f %f %f\n",preamp, gain[0],gain[1],gain[2]),fflush(stderr);
}

DttSP_EXP void
SetGrphRXEQ10(unsigned int thread, unsigned int subrx, int *rxeq) 
{
/*  if (n < 11)
	return -1;
	else {
	int band, i, j;
	fftwf_plan ptmp;
	ComplexFIR tmpfilt;
	COMPLEX *filtcoef = newvec_COMPLEX(512, "filter for EQ"),
	*tmpcoef  = newvec_COMPLEX(257, "tmp filter for EQ");
	REAL preamp = dB2lin(atof(p[0])) * 0.5; */

	int i,j,band;
	fftwf_plan ptmp;
	EQ a = rx[thread][subrx].grapheq.gen;
	COMPLEX *filtcoef = newvec_COMPLEX(a->eqsize, "filter for EQ"),
            *tmpcoef  = newvec_COMPLEX(a->eqsize/2 + 1, "tmp filter for EQ");
    ComplexFIR tmpfilt;
    REAL preamp;

	preamp = dB2lin((REAL) rxeq[0]) * 0.5f;
	
    for (j = 1, band = 15; j <= 10; j++, band += 3)
	{
		REAL f_here  = ISOband_get_nominal(band),
			f_below = gmean(f_here / 2.0f, f_here),
			f_above = gmean(f_here, f_here * 2.0f),
			g_here  = dB2lin((REAL) rxeq[j]) * preamp;

		tmpfilt = newFIR_Bandpass_COMPLEX(-f_above, -f_below, uni->samplerate, a->eqsize/2 + 1);
		for (i = 0; i < a->eqsize/2 + 1; i++)
			tmpcoef[i] = Cadd(tmpcoef[i], Cscl(tmpfilt->coef[i], g_here));
		delFIR_Bandpass_COMPLEX(tmpfilt);

		tmpfilt = newFIR_Bandpass_COMPLEX(f_below, f_above, uni->samplerate, a->eqsize/2 + 1);
		for (i = 0; i < a->eqsize/2 + 1; i++)
			tmpcoef[i] = Cadd(tmpcoef[i], Cscl(tmpfilt->coef[i], g_here));
		delFIR_Bandpass_COMPLEX(tmpfilt);
	}

    for (i = 0; i < a->eqsize/2 + 1; i++)
		filtcoef[a->eqsize/2 - 2 + i] = Cscl(tmpcoef[i],(REAL)(1.0/(double)a->eqsize));;

    ptmp = fftwf_plan_dft_1d(a->eqsize,
		(fftwf_complex *) filtcoef,
		(fftwf_complex *) rx[thread][subrx].grapheq.gen->p->zfvec,
		FFTW_FORWARD,
		uni->wisdom.bits);

    fftwf_execute(ptmp);
    fftwf_destroy_plan(ptmp);
    delvec_COMPLEX(filtcoef);
    delvec_COMPLEX(tmpcoef);
}

DttSP_EXP void
SetGrphRXEQcmd (unsigned int thread, unsigned int subrx,BOOLEAN state)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].grapheq.flag = state;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetTXAGCFF (unsigned int thread, BOOLEAN setit)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].spr.flag = setit;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetTXAGCFFCompression (unsigned int thread, double txcompression)
{
	sem_wait(&top[thread].sync.upd.sem);
	tx[thread].spr.gen->MaxGain = (REAL) pow (10.0, txcompression * 0.05);
	tx[thread].spr.gen->fac = (REAL)
		((((0.0000401002 * txcompression) - 0.0032093390) * txcompression +
		0.0612862687) * txcompression + 0.9759745718);
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetSquelchVal (unsigned int thread, unsigned int subrx, float setit)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].squelch.thresh = setit;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetSquelchState (unsigned int thread, unsigned int subrx,BOOLEAN setit)
{
	sem_wait(&top[thread].sync.upd.sem);
	rx[thread][subrx].squelch.flag = setit;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetTRX (unsigned int thread, TRXMODE setit)
{
//	int i;
	sem_wait (&top[thread].sync.upd.sem);
	uni[thread].mode.trx = setit;

#if 0
	if(setit == RX)
	{
		for(i=0; i<uni[thread].multirx.nrx; i++)
			reset_OvSv(rx[thread][i].filt.ovsv);
	}
	else
		reset_OvSv(tx[thread].filt.ovsv);
#endif

	sem_post (&top[thread].sync.upd.sem);
}

DttSP_EXP void // (NR0V) modified
FlushAllBufs (unsigned int thread, BOOLEAN trx)
{
	int i;
	sem_wait (&top[thread].sync.upd.sem);

	if(trx)
	{
		reset_OvSv(tx[thread].filt.ovsv);
		reset_OvSv(tx[thread].filt.ovsv_pre);
		memset(top[thread].hold.buf.l,0,top[thread].hold.size.frames*sizeof(REAL));
		memset(top[thread].hold.buf.r,0,top[thread].hold.size.frames*sizeof(REAL));
		WcpAGC_flushbuf(tx[thread].leveler.gen);
		WcpAGC_flushbuf(tx[thread].alc.gen);
	}
	else
	{
		memset(top[thread].hold.buf.l,0,top[thread].hold.size.frames*sizeof(REAL));
		memset(top[thread].hold.buf.r,0,top[thread].hold.size.frames*sizeof(REAL));
		for(i=0; i<uni[thread].multirx.nrx; i++)
		{
			reset_OvSv(rx[thread][i].filt.ovsv);
			WcpAGC_flushbuf(rx[thread][i].wcpagc.gen);
			resetDCBlocker(rx[thread][i].dcb, DCB_SINGLE_POLE);
		}
	}

	sem_post (&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetDSPBuflen (unsigned int thread, int newBuffSize)
{
	extern int reset_for_buflen (unsigned int, int);
	extern BOOLEAN reset_em;
	sem_wait (&top[thread].sync.upd.sem);
	top[0].susp = TRUE;
	reset_for_buflen (thread,     newBuffSize);
	//fprintf(stdout,"SetDSPBuflen: reset_em = TRUE\n"), fflush(stdout);
	reset_em = TRUE;
	top[0].susp = FALSE;
	sem_post (&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetAudioSize (unsigned int size)
{
	unsigned int thread;
	for(thread = 0;thread < 3; thread++)
	{
		top[thread].susp = TRUE;
		sem_wait (&top[thread].sync.upd.sem);
	}
	for(thread=0; thread < 3; thread++)
	{
		ringb_float_reset (top[thread].jack.ring.i.l);
		ringb_float_reset (top[thread].jack.ring.i.r);
		ringb_float_reset (top[thread].jack.auxr.i.l);
		ringb_float_reset (top[thread].jack.auxr.i.r);
		ringb_float_restart (top[thread].jack.ring.o.l, top[thread].hold.size.frames);
		ringb_float_restart (top[thread].jack.ring.o.r, top[thread].hold.size.frames);
	}
	for(thread = 0;thread < 3; thread++)
	{
		top[thread].susp = FALSE;
		sem_post (&top[thread].sync.upd.sem);
	}
}

DttSP_EXP void
SetTXAGCCompression (unsigned int thread, double txcompression)
{

}

DttSP_EXP void
Process_ComplexSpectrum (unsigned int thread, float *results)
{
	uni[thread].spec.type = SPEC_POST_FILT;
	uni[thread].spec.scale = SPEC_PWR;
	//sem_wait (&top[thread].sync.upd.sem);
	snap_spectrum (&uni[thread].spec, uni[thread].spec.type);
	//sem_post (&top[thread].sync.upd.sem);
	compute_complex_spectrum (&uni[thread].spec);
	memcpy ((void *) results, uni[thread].spec.coutput, uni[thread].spec.size * sizeof (COMPLEX));
}

DttSP_EXP void
Process_Spectrum (unsigned int thread, float *results)
{
	uni[thread].spec.type = SPEC_POST_FILT;
	uni[thread].spec.scale = SPEC_PWR;
	//sem_wait (&top[thread].sync.upd.sem);
	snap_spectrum (&uni[thread].spec, uni[thread].spec.type);
	//sem_post (&top[thread].sync.upd.sem);
	compute_spectrum (&uni[thread].spec);
	memcpy ((void *) results, uni[thread].spec.output, uni[thread].spec.size * sizeof (float));
}

DttSP_EXP void
Process_Panadapter (unsigned int thread, float *results)
{
	extern BOOLEAN reset_em;
	//sem_wait (&top[thread].sync.upd.sem);
	if (uni[thread].mode.trx == TX)
		uni[thread].spec.type = SPEC_POST_FILT;
	else
		uni[thread].spec.type = SPEC_PRE_FILT;

	uni[thread].spec.scale = SPEC_PWR;

	if (reset_em)
	{
		memset(results,0,uni[thread].spec.size * sizeof (float));
		//sem_post (&top[thread].sync.upd.sem);
		return;
	}
	snap_spectrum (&uni[thread].spec, uni[thread].spec.type);
	//sem_post (&top[thread].sync.upd.sem);
	compute_spectrum (&uni[thread].spec);
	memcpy ((void *) results, uni[thread].spec.output, uni[thread].spec.size * sizeof (float));
}

DttSP_EXP void
Process_Phase (unsigned int thread, float *results, int numpoints)
{
	int i, j;
	extern BOOLEAN reset_em;
	if (reset_em) 
	{
		memset(results,0,numpoints * sizeof (float));
		return;
	}
	//sem_wait (&top[thread].sync.upd.sem);
	uni[thread].spec.type = SPEC_POST_AGC;
	uni[thread].spec.scale = SPEC_PWR;
	uni[thread].spec.rxk = 0;

	snap_scope (&uni[thread].spec, uni[thread].spec.type);
	//sem_post (&top[thread].sync.upd.sem);
	for (i = 0, j = 0; i < numpoints; i++, j += 2)
	{
		results[j] = (float) CXBreal (uni[thread].spec.timebuf, i);
		results[j + 1] = (float) CXBimag (uni[thread].spec.timebuf, i);
	}
}

DttSP_EXP void
Process_Scope (unsigned int thread, float *results, int numpoints)
{
	int i;
	extern BOOLEAN reset_em;
	if (reset_em)
	{
		memset(results,0,numpoints * sizeof (float));
		return;
	}
	//sem_wait (&top[thread].sync.upd.sem);
	uni[thread].spec.type = SPEC_POST_AGC;
	uni[thread].spec.scale = SPEC_PWR;
	uni[thread].spec.rxk = 0;

	snap_scope (&uni[thread].spec, uni[thread].spec.type);
	//sem_post (&top[thread].sync.upd.sem);
	for (i = 0; i < numpoints; i++)
	{
		results[i] = (float) CXBreal (uni[thread].spec.timebuf, i);
	}
}

DttSP_EXP void
SetRingBufferOffset(unsigned int thread, int offset)
{
	extern BOOLEAN reset_em;
	sem_wait(&top[thread].sync.upd.sem);
	top[thread].offset = offset;
	//fprintf(stdout,"SetRingBufferOffset: reset_em = TRUE\n"), fflush(stdout);
	reset_em = TRUE;
	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP float
CalculateRXMeter (unsigned int thread, unsigned int subrx, METERTYPE mt)
{
	float returnval = 0;
	//sem_wait (&top[thread].sync.upd.sem);
	uni[thread].meter.rx.mode[subrx] = (RXMETERTYPE)mt;
	switch (mt)
	{
		case SIGNAL_STRENGTH:
			returnval = uni[thread].meter.rx.val[subrx][RX_SIGNAL_STRENGTH];
			break;
		case AVG_SIGNAL_STRENGTH:
			returnval = (float) uni[thread].meter.rx.val[subrx][RX_AVG_SIGNAL_STRENGTH];
			break;
		case ADC_REAL:
			returnval = (float) uni[thread].meter.rx.val[subrx][RX_ADC_IMAG];
			break;
		case ADC_IMAG:
			returnval = (float) uni[thread].meter.rx.val[subrx][RX_ADC_REAL];
			break;
		case AGC_GAIN:
			returnval = (float) uni[thread].meter.rx.val[subrx][RX_AGC_GAIN];
			break;
		default:
			returnval = -200;
			break;
	}

	//sem_post (&top[thread].sync.upd.sem);
	return returnval;
}

DttSP_EXP float
CalculateTXMeter (unsigned int thread, METERTYPE mt)
{
	float returnval = 0;
	//sem_wait (&top[thread].sync.upd.sem);
	uni[thread].meter.tx.mode = (TXMETERTYPE)mt;
	switch(mt)
	{
		case MIC:
			returnval = (float) uni[thread].meter.tx.val[TX_MIC];
			break;
		case PWR:
			returnval = (float) uni[thread].meter.tx.val[TX_PWR];
			break;
		case ALC:
			returnval = (float) uni[thread].meter.tx.val[TX_ALC];
			break;
		case EQtap:
			returnval = (float) uni[thread].meter.tx.val[TX_EQ];
			break;
		case LEVELER:
			returnval = (float) uni[thread].meter.tx.val[TX_LVL];
			break;
		case COMP:
			returnval = (float) uni[thread].meter.tx.val[TX_COMP];
			break;
		case CPDR:
			returnval = (float) uni[thread].meter.tx.val[TX_CPDR];
			break;
		case ALC_G:
			returnval = (float) uni[thread].meter.tx.val[TX_ALC_G];
			break;
		case LVL_G:
			returnval = (float) uni[thread].meter.tx.val[TX_LVL_G];
			break;
		case MIC_PK:
			returnval = (float) uni[thread].meter.tx.val[TX_MIC_PK];
			break;
		case ALC_PK:
			returnval = (float) uni[thread].meter.tx.val[TX_ALC_PK];
			break;
		case EQ_PK:
			returnval = (float) uni[thread].meter.tx.val[TX_EQ_PK];
			break;
		case LEVELER_PK:
			returnval = (float) uni[thread].meter.tx.val[TX_LVL_PK];
			break;
		case COMP_PK:
			returnval = (float) uni[thread].meter.tx.val[TX_COMP_PK];
			break;
		case CPDR_PK:
			returnval = (float) uni[thread].meter.tx.val[TX_CPDR_PK];
			break;
		default:
			returnval = -200;
	}
	//sem_post (&top[thread].sync.upd.sem);
	return returnval;
}

DttSP_EXP void *
NewResampler (int samplerate_in, int samplerate_out)
{
	ResSt tmp;
	//int lcm = 28224000, interpFactor, deciFactor;
	int lcm = 56448000, interpFactor, deciFactor;
	interpFactor = lcm / samplerate_in;
	deciFactor = lcm / samplerate_out;
	tmp = newPolyPhaseFIR (32768, 0, interpFactor, 0, deciFactor);
	return (void *) tmp;
}

DttSP_EXP void
DoResampler (COMPLEX * input, COMPLEX * output, int numsamps, int *outsamps,
             ResSt ptr)
{
	ptr->input = input;
	ptr->output = output;
	ptr->inputArrayLength = numsamps;
	PolyPhaseFIR (ptr);
	*outsamps = ptr->numOutputSamples;
}

DttSP_EXP void
DelPolyPhaseFIR (ResSt resst)
{
	extern void delPolyPhaseFIR (ResSt resst);
	delPolyPhaseFIR (resst);
}

DttSP_EXP void *
NewResamplerF (int samplerate_in, int samplerate_out)
{
	ResStF tmp;
	//int lcm = 28224000, interpFactor, deciFactor;
	int lcm = 56448000, interpFactor, deciFactor;
	interpFactor = lcm / samplerate_in;
	deciFactor = lcm / samplerate_out;
	tmp = newPolyPhaseFIRF (4096, 0, interpFactor, 0, deciFactor);
	return (void *) tmp;
}

DttSP_EXP void
DoResamplerF (float *input, float *output, int numsamps, int *outsamps,
              ResStF ptr)
{
	ptr->input = input;
	ptr->output = output;
	ptr->inputArrayLength = numsamps;
	PolyPhaseFIRF (ptr);
	*outsamps = ptr->numOutputSamples;
}

DttSP_EXP void
DelPolyPhaseFIRF (ResSt resst)
{
	extern void delPolyPhaseFIRF (ResSt resst);
	delPolyPhaseFIRF (resst);
}

DttSP_EXP int
SetSubRXSt(unsigned int thread, unsigned int subrx, BOOLEAN setit)
{
	int rtn = 0;
	switch (setit)
	{
	case TRUE:
		if (subrx >= (unsigned)uni[thread].multirx.nrx)
		{
			rtn = -1;
		}
		else
		{
			if (uni[thread].multirx.act[subrx])
			{
				rtn = -1;
			}
			else
			{
				sem_wait(&top[thread].sync.upd.sem);
				uni[thread].multirx.act[subrx] = TRUE;
				uni[thread].multirx.nac++;
				rx[thread][subrx].tick = 0;
				rtn = 0;
				sem_post(&top[thread].sync.upd.sem);
			}
		}
		break;
	case FALSE:
		if ( subrx >= (unsigned)uni[thread].multirx.nrx)
		{
			rtn = -1;
		}
		else
		{
			if (!uni[thread].multirx.act[subrx])
				rtn = -1;
			else
			{
				sem_wait(&top[thread].sync.upd.sem);
				uni[thread].multirx.act[subrx] = FALSE;
				--uni[thread].multirx.nac;
				rtn = 0;
				sem_post(&top[thread].sync.upd.sem);
			}
		}
		break;
	}
	return rtn;
}

// Determines the left (0.0) to right (1.0) audio field for this RX.
DttSP_EXP void
SetRXPan(unsigned int thread, unsigned int subrx, float pos)
{
	float theta, gain_l, gain_r;
	sem_wait(&top[thread].sync.upd.sem);
	if (pos < 0.0f) pos = 0.0f; // catch lower limit
	if (pos > 1.0f) pos = 1.0f; // catch upper limit

	if(pos <= 0.5f) gain_l = 1.0f;
	else 
	{
		// theta runs from 0 to PI/2 as pos goes from 0.5 to 1.0
		theta = (REAL) ((pos - 0.5) * M_PI);
		gain_l = (REAL)cos(theta);
	}

	if(pos >= 0.5f) gain_r = 1.0f;
	else
	{	
		// theta runs from 0 to PI/2 as pos goes from 0.0 to 0.5
		theta = (REAL) (pos * M_PI);
		gain_r = (REAL)sin(theta);
	}

	// Right first because imag is copied to the left channel in process_samples
	rx[thread][subrx].azim = Cmplx (gain_r, gain_l);

	//theta = (REAL) (pos * M_PI / 2.0);
	//rx[thread][subrx].azim = Cmplx ((REAL) cos (theta), (REAL) sin (theta));

	sem_post(&top[thread].sync.upd.sem);
}

DttSP_EXP void
SetDiversity (int setit)
{
	extern BOOLEAN reset_em;
	//fprintf(stderr, "SetDiversity: %u\n", setit), fflush(stderr);
	sem_wait(&top[0].sync.upd.sem);
	sem_wait(&top[1].sync.upd.sem);
	sem_wait(&top[2].sync.upd.sem);
	//reset_em=TRUE;
	diversity.flag = setit;
	sem_post(&top[0].sync.upd.sem);
	sem_post(&top[1].sync.upd.sem);
	sem_post(&top[2].sync.upd.sem);
}

DttSP_EXP void
SetDiversityScalar(REAL re, REAL im)
{
	//fprintf(stderr, "SetDiversityScalar: %f, %f\n", re, im), fflush(stderr);
	sem_wait(&top[2].sync.upd.sem);
	diversity.scalar = Cmplx(re,im);
	sem_post(&top[2].sync.upd.sem);
}

DttSP_EXP void
SetDiversityGain(REAL gain)
{
	//fprintf(stderr, "SetDiversityGain: %f\n", gain), fflush(stderr);
	sem_wait(&top[2].sync.upd.sem);
	diversity.gain = gain;
	sem_post(&top[2].sync.upd.sem);
}

// kd5tfd added - experimental support for MercuryXmit and EerXmit
int MercuryXmit = 0;

DttSP_EXP void SetMercuryXmit(int on_off) {
    MercuryXmit = on_off;
}

int EerXmit = 0;

DttSP_EXP void SetEerXmit(int on_off) {
    EerXmit = on_off;
}

DttSP_EXP void
SetHPSDRSpectrum(unsigned int thread, unsigned int subrx, int flag, int disp, int ss, int LO)
{
	rx[thread][subrx].hpsdr.specflag = flag;
	rx[thread][subrx].hpsdr.disp = disp;
	rx[thread][subrx].hpsdr.ss = ss;
	rx[thread][subrx].hpsdr.LO = LO;
}
