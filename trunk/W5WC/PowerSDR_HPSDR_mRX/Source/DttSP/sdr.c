/* sdr.c

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2004, 2005, 2006 by Frank Brickle, AB2KT and Bob McGwier, N4HY.
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
#include <wcpAGC.h>
#include <analyzer.h>

#define NEW_DISPLAYS
FILE *wcpfile;	// (NR0V)

//========================================================================
/* initialization and termination */
int const MAX_NOTCHES_IN_PASSBAND = 9; //18;
void
reset_meters (unsigned int thread)
{
	if (uni[thread].meter.flag)
	{                           // reset metering completely
		int i, k;
		for (i = 0; i < RXMETERPTS; i++)
			for (k = 0; k < MAXRX; k++)
				uni[thread].meter.rx.val[k][i] = -200.0;
		for (i = 0; i < TXMETERPTS; i++)
			uni[thread].meter.tx.val[i] = -200.0;
	}
}

void
reset_spectrum (unsigned int thread)
{
	if (uni[thread].spec.flag)
		reinit_spectrum (&uni[thread].spec);
}

void
reset_counters (unsigned thread)
{
	int k;
	for (k = 0; k < uni[thread].multirx.nrx; k++)
		rx[thread][k].tick = 0;
	tx[thread].tick = 0;
}

//========================================================================

/* global and general info,
   not specifically attached to
   tx, rx, or scheduling */

PRIVATE void
setup_all (REAL rate, int buflen, SDRMODE mode, char *wisdom,
	int specsize, int numrecv, int cpdsize, unsigned int thread)
{
	uni[thread].samplerate = rate;
	uni[thread].buflen = buflen;
	uni[thread].mode.sdr = mode;
	if (thread != 1) uni[thread].mode.trx = RX;
	else uni[thread].mode.trx = TX;

	uni[thread].wisdom.path = wisdom;
	uni[thread].wisdom.bits = FFTW_ESTIMATE;
	{
		FILE *f = fopen (uni[thread].wisdom.path, "r");
		if (f)
		{
			char wisdomstring[32768];
			fread(wisdomstring,1,32768,f);
			if (fftwf_import_wisdom_from_string (wisdomstring) != 0)
				uni[thread].wisdom.bits = FFTW_MEASURE;
			fclose (f);
		}
	}

	if (uni[thread].meter.flag)
	{
		reset_meters (thread);
	}

	uni[thread].spec.rxk = 0;
	uni[thread].spec.buflen = uni[thread].buflen;
	uni[thread].spec.scale = SPEC_PWR;
	uni[thread].spec.type = SPEC_POST_FILT;
	uni[thread].spec.size = specsize;
	uni[thread].spec.planbits = uni[thread].wisdom.bits;
	init_spectrum (&uni[thread].spec);
	//fprintf(stderr,"Created spectrum\n"),fflush(stderr);

	// set which receiver is listening to commands
	uni[thread].multirx.lis = 0;
	uni[thread].multirx.nrx = numrecv;

	// set mixing of input from aux ports
	uni[thread].mix.rx.flag = uni[thread].mix.tx.flag = FALSE;
	uni[thread].mix.rx.gain = uni[thread].mix.tx.gain = 1.0;

	uni[thread].cpdlen = cpdsize;

	uni[thread].tick = uni[thread].oldtick = 0;
}

/* purely rx */

PRIVATE void
setup_rx (int k, unsigned int thread)
{
	int i;

	/* conditioning */
	if (thread == 0) {
		diversity.gain = 1.0;
		diversity.scalar = Cmplx(1.0,0);
	}
	rx[thread][k].iqfix = newCorrectIQ (0.0, 1.0, 0.000f);
	rx[thread][k].filt.low = 150.0;  // (NR0V)
	rx[thread][k].filt.high = 2850.0;
	rx[thread][k].filt.coef = newFIR_Bandpass_COMPLEX ((float)rx[thread][k].filt.low,
		(float)rx[thread][k].filt.high,	uni[thread].samplerate, uni[thread].buflen + 1);
	rx[thread][k].filt.ovsv =
		newFiltOvSv (FIRcoef (rx[thread][k].filt.coef), FIRsize (rx[thread][k].filt.coef),
		uni[thread].wisdom.bits);
	rx[thread][k].filt.ovsv_notch =		// (NR0V)
		newFiltOvSv (FIRcoef (rx[thread][k].filt.coef), FIRsize (rx[thread][k].filt.coef),
		uni[thread].wisdom.bits);
	rx[thread][k].resample.flag = FALSE;
	normalize_vec_COMPLEX (rx[thread][k].filt.ovsv->zfvec, rx[thread][k].filt.ovsv->fftlen, rx[thread][k].filt.ovsv->scale);
	normalize_vec_COMPLEX (rx[thread][k].filt.ovsv_notch->zfvec, rx[thread][k].filt.ovsv_notch->fftlen, rx[thread][k].filt.ovsv_notch->scale);	// (NR0V)

	rx[thread][k].output_gain=1.0f;

	// hack for EQ
	rx[thread][k].filt.save =
		newvec_COMPLEX (rx[thread][k].filt.ovsv->fftlen, "RX filter cache");
	memcpy ((char *) rx[thread][k].filt.save, (char *) rx[thread][k].filt.ovsv->zfvec,
		rx[thread][k].filt.ovsv->fftlen * sizeof (COMPLEX));

	/* buffers */
	/* note we overload the internal filter buffers
	we just created */
	rx[thread][k].buf.i = newCXB (FiltOvSv_fetchsize (rx[thread][k].filt.ovsv),
		FiltOvSv_fetchpoint (rx[thread][k].filt.ovsv),
		"init rx[thread][k].buf.i");

	rx[thread][k].buf.o = newCXB (FiltOvSv_storesize (rx[thread][k].filt.ovsv),
		FiltOvSv_storepoint (rx[thread][k].filt.ovsv),
		"init rx[thread][k].buf.o");

	rx[thread][k].buf.i_notch = newCXB (FiltOvSv_fetchsize (rx[thread][k].filt.ovsv_notch),		// (NR0V)
		FiltOvSv_fetchpoint (rx[thread][k].filt.ovsv_notch),
		"init rx[thread][k].buf.i_notch");

	rx[thread][k].buf.o_notch = newCXB (FiltOvSv_storesize (rx[thread][k].filt.ovsv_notch),
		FiltOvSv_storepoint (rx[thread][k].filt.ovsv_notch),
		"init rx[thread][k].buf.o_notch");

	rx[thread][k].dcb = newDCBlocker(DCB_SINGLE_POLE, rx[thread][k].buf.i);
	rx[thread][k].dcb->flag = FALSE;

	/* conversion */
	rx[thread][k].osc.freq = -9000.0;
	rx[thread][k].osc.phase = 0.0;
	rx[thread][k].osc.gen = newOSC (uni[thread].buflen,
		ComplexTone,
		rx[thread][k].osc.freq,
		rx[thread][k].osc.phase,
		uni[thread].samplerate, "SDR RX Oscillator");

	rx[thread][k].wcpagc.gen = newWcpAGC ( // (NR0V)
		agcMED,
		0,								//pmode
		CXBbase (rx[thread][k].buf.o),	//buff pointer
		CXBsize (rx[thread][k].buf.o),	//io_buffsize
		uni[thread].samplerate,			//sample rate
		0.002,							//tau_attack
		0.250,							//tau_decay
		4,								//n_tau
		10000.0,						//max_gain
		1.5,							//var_gain
		1000.0,							//fixed_gain
		1.0,							//max_input
		1.0,							//out_targ
		0.250,							//tau_fast_backaverage
		0.005,							//tau_fast_decay
		5.0,							//pop_ratio
		0.500,							//tau_hang_backmult
		0.250,							//hangtime
		0.250,							//hang_thresh
		0.100,							//tau_hang_decay
		"AGC");							//tag
		
	rx[thread][k].wcpagc.flag = TRUE;

	rx[thread][k].anf.gen = newANF	(	// (NR0V) added	
		CXBsize (rx[thread][k].buf.o),	//buff_size
		CXBbase (rx[thread][k].buf.o),	//buff pointer
		DLINE_SIZE,	
		256,
		64,				
		0.0001,	
		0.1,

		1.0,
		0.0,
		200.0,
		6.25e-12,
		6.25e-10,
		1.0,
		3.0,
		"ANF");
	rx[thread][k].anf.flag = FALSE;
	rx[thread][k].anf.position = 0;

	rx[thread][k].anr.gen = newANR	(	// (NR0V) added	
		CXBsize (rx[thread][k].buf.o),	//buff_size
		CXBbase (rx[thread][k].buf.o),	//buff pointer
		DLINE_SIZE,	
		256,
		64,				
		0.0001,	
		0.1,

		120.0,
		120.0,
		200.0,
		0.001,
		6.25e-10,
		1.0,
		3.0,
		"ANR");
	rx[thread][k].anr.flag = FALSE;
	rx[thread][k].anr.position = 0;

	rx[thread][k].grapheq.gen = new_EQ (CXBsize (rx[thread][k].buf.o), rx[thread][k].buf.o, uni[thread].samplerate, uni[thread].wisdom.bits);
	rx[thread][k].grapheq.flag = FALSE;

	/* demods */
	rx[thread][k].amd.gen = newAMD (	// (NR0V)
		CXBsize (rx[thread][k].buf.o),
		CXBbase (rx[thread][k].buf.o),
		CXBbase (rx[thread][k].buf.o),
		0,
		0,
		0,
		uni[thread].samplerate,
		-2000.0,
		2000.0,
		1.0,
		0.025,
		0.02,
		1.4,
		"amd"
		);

	rx[thread][k].fm.gen = newFMD (
		uni[thread].samplerate,			// REAL samprate
		0.0,							// REAL f_initial
		-8000.0,						// REAL f_lobound
		8000.0,							// REAL f_hibound
		16000.0,						// REAL f_bandwid
		CXBsize (rx[thread][k].buf.o),	// int size
		CXBbase (rx[thread][k].buf.o),	// COMPLEX *ivec
		CXBbase (rx[thread][k].buf.o),	// COMPLEX *ovec
		"New FM Demod structure");		// char *error message;

	/* block auto-notch filter */
	rx[thread][k].banf.gen = new_blms(
		rx[thread][k].buf.o,    // CXB signal,
		0.01f,				// REAL adaptation_rate,
		0.00000f,				// REAL leakage,
		LMADF_INTERFERENCE,		// type
		uni->wisdom.bits);      // fftw wisdom
	rx[thread][k].banf.flag = FALSE;

	/* block auto-noise filter */
	rx[thread][k].banr.gen = new_blms(
		rx[thread][k].buf.o,    // CXB signal,
		0.001f,					// REAL adaptation_rate,
		0.000001f,				// REAL leakage,
		LMADF_NOISE,			// type
		uni->wisdom.bits);      // fftw wisdom
	rx[thread][k].banr.flag = FALSE;


	rx[thread][k].nb.thresh = 3.3f;
	rx[thread][k].nb.gen = new_noiseblanker (rx[thread][k].buf.i, rx[thread][k].nb.thresh);
	rx[thread][k].nb.flag = FALSE;

	rx[thread][k].nb_sdrom.thresh = 2.5f;
	rx[thread][k].nb_sdrom.gen = new_noiseblanker (rx[thread][k].buf.i, rx[thread][k].nb_sdrom.thresh);
	rx[thread][k].nb_sdrom.flag = FALSE;

	for(i=0; i<MAX_NOTCHES_IN_PASSBAND; i++)
	{
		rx[thread][k].notch[i].gen = new_IIR_2P2Z(
			rx[thread][k].buf.o,	// Buffer
			1.0,					// Gain
			1.0,					// Parameter value - Q in this case
			Q,						// type of parameter
			NOTCH,					// type of filter
			uni[thread].samplerate, // sample rate
			400.0);					// frequency for the notch
		rx[thread][k].notch[i].gen->doComplex = TRUE;
	}

	rx[thread][k].spot.gen = newSpotToneGen (
		-12.0,						// gain
		700.0,						// freq
		5.0,						// ms rise
		5.0,						// ms fall
		uni[thread].buflen,			// length of spot tone buffer
		uni[thread].samplerate);	// sample rate

	memset ((char *) &rx[thread][k].squelch, 0, sizeof (rx[thread][k].squelch));
	rx[thread][k].squelch.thresh = -150.0;
	rx[thread][k].squelch.power = 0.0;
	rx[thread][k].squelch.flag = rx[thread][k].squelch.running = rx[thread][k].squelch.set = FALSE;
	rx[thread][k].squelch.num = uni[thread].buflen - 48;

	rx[thread][k].cpd.gen = newWSCompander (uni[thread].cpdlen, 0.0, rx[thread][k].buf.o);
	rx[thread][k].cpd.flag = FALSE;

	rx[thread][k].mode = uni[thread].mode.sdr;
	rx[thread][k].bin.flag = FALSE;

	{
		//REAL pos = 0.5,             // 0 <= pos <= 1, left->right
		//theta = (REAL) ((1.0 - pos) * M_PI / 2.0);
		rx[thread][k].azim = Cmplx (1.0f, 1.0f);
	}
	rx[thread][k].hpsdr.specflag = 0;
	rx[thread][k].tick = 0;
	rx[thread][k].cbl.gen = newCBL(	CXBsize (rx[thread][k].buf.o),
									CXBbase (rx[thread][k].buf.o),
									CXBbase (rx[thread][k].buf.o),
									0,
									uni[thread].samplerate,
									0.02,
									"cbl");
}

/* purely tx */

PRIVATE void // (NR0V) modified
setup_tx (unsigned int thread)
{
	/* conditioning */
	tx[thread].iqfix = newCorrectIQ (0.0, 1.0, 0.0);
	tx[thread].filt.coef = newFIR_Bandpass_COMPLEX (300.0, 3000.0, 
		uni[thread].samplerate, uni[thread].buflen + 1);
	tx[thread].filt.ovsv = newFiltOvSv (FIRcoef (tx[thread].filt.coef),
		FIRsize (tx[thread].filt.coef), uni[thread].wisdom.bits);
	tx[thread].filt.ovsv_pre = newFiltOvSv (FIRcoef (tx[thread].filt.coef),  // (NR0V)
		FIRsize (tx[thread].filt.coef), uni[thread].wisdom.bits);
	normalize_vec_COMPLEX (tx[thread].filt.ovsv->zfvec, tx[thread].filt.ovsv->fftlen,tx[thread].filt.ovsv->scale);
	normalize_vec_COMPLEX (tx[thread].filt.ovsv_pre->zfvec, tx[thread].filt.ovsv_pre->fftlen,tx[thread].filt.ovsv_pre->scale);  //(NR0V)

	// hack for EQ
	tx[thread].filt.save = newvec_COMPLEX (tx[thread].filt.ovsv->fftlen, "TX filter cache");
	memcpy ((char *) tx[thread].filt.save,
		(char *) tx[thread].filt.ovsv->zfvec,
		tx[thread].filt.ovsv->fftlen * sizeof (COMPLEX));

	/* buffers */
	tx[thread].buf.i = newCXB (FiltOvSv_fetchsize (tx[thread].filt.ovsv),
		FiltOvSv_fetchpoint (tx[thread].filt.ovsv), "init tx[thread].buf.i");
	tx[thread].buf.o = newCXB (FiltOvSv_storesize (tx[thread].filt.ovsv),
		FiltOvSv_storepoint (tx[thread].filt.ovsv), "init tx[thread].buf.o");
	tx[thread].buf.i_pre = newCXB (FiltOvSv_fetchsize (tx[thread].filt.ovsv_pre),  // (NR0V)
		FiltOvSv_fetchpoint (tx[thread].filt.ovsv_pre), "init tx[thread].buf.i_pre");
	tx[thread].buf.o_pre = newCXB (FiltOvSv_storesize (tx[thread].filt.ovsv_pre),
		FiltOvSv_storepoint (tx[thread].filt.ovsv_pre), "init tx[thread].buf.o_pre");

	tx[thread].dcb.flag = FALSE;
	tx[thread].dcb.gen = newDCBlocker (DCB_MED, tx[thread].buf.i);

	/* conversion */
	tx[thread].osc.freq = 0.0;
	tx[thread].osc.phase = 0.0;
	tx[thread].osc.gen = newOSC (uni[thread].buflen,
		ComplexTone,
		tx[thread].osc.freq,
		tx[thread].osc.phase, uni[thread].samplerate, "SDR TX Oscillator");

	tx[thread].am.carrier_level = 0.5f;
	tx[thread].fm.cvtmod2freq = (REAL) (5000.0f * TWOPI / uni[thread].samplerate); //5 kHz deviation..used to be 3
	tx[thread].fm.phase = 0.0;
	tx[thread].fm.k_preemphasis = (REAL)(1.0f + uni[thread].samplerate/(TWOPI*3000.0f));  //3.546
	tx[thread].fm.k_deemphasis = (REAL)(1.0f + uni[thread].samplerate/(TWOPI*250.0f));  //3.546
	tx[thread].fm.preemphasis_filter = 0.0f;
	tx[thread].fm.deemphasis_out = 0.0f;
	tx[thread].fm.clip_threshold = 0.75f;
	tx[thread].fm.output_LPF1 = new_IIR_LPF_2P(tx[thread].buf.i,uni[thread].samplerate, 3500.0f, 0.25f);	//4 pole butterworth Q = 0.76537, 1.84776	
	tx[thread].fm.output_LPF2 = new_IIR_LPF_2P(tx[thread].buf.i,uni[thread].samplerate, 3500.0f, 1.75f);	//4 pole butterworth Q = 0.76537, 1.84776	

	tx[thread].fm.input_LPF1 = new_IIR_LPF_2P(tx[thread].buf.i,uni[thread].samplerate, 3500.0f, 0.25f);	//4 pole butterworth Q = 0.76537, 1.84776	
	tx[thread].fm.input_LPF2 = new_IIR_LPF_2P(tx[thread].buf.i,uni[thread].samplerate, 3500.0f, 1.75f);	//4 pole butterworth Q = 0.76537, 1.84776
	tx[thread].fm.input_HPF1 = new_IIR_HPF_2P(tx[thread].buf.i,uni[thread].samplerate, 150.0f, 0.34f);	//4 pole butterworth Q = 0.76537, 1.84776	
	tx[thread].fm.input_HPF2 = new_IIR_HPF_2P(tx[thread].buf.i,uni[thread].samplerate, 150.0f, 0.94f);	//4 pole butterworth Q = 0.76537, 1.84776	

	tx[thread].fm.ctcss.flag = FALSE;
	tx[thread].fm.ctcss.amp = .13f;
	tx[thread].fm.ctcss.freq_hz = 100.0;
	tx[thread].fm.ctcss.osc = newOSC (uni[thread].buflen, ComplexTone, 100.0, 0.0, uni[thread].samplerate, "SDR TX CTTS Oscillator");

	tx[thread].leveler.gen = newWcpAGC (// (NR0V)
		(AGCMODE)5,						//mode
		0,								//0 for max(I,Q), 1 for envelope
		CXBbase (tx[thread].buf.i),		//buff pointer
		CXBsize (tx[thread].buf.i),		//io_buffsize
		uni[thread].samplerate,			//sample rate
		0.002,							//tau_attack
		0.500,							//tau_decay
		8,								//n_tau	
		1.778,							//max_gain
		1.0,							//var_gain
		1.0,							//fixed_gain
		1.0,							//max_input
		1.05,							//out_targ	
		0.250,							//tau_fast_backaverage
		0.005,							//tau_fast_decay
		5.0,							//pop_ratio
		0.500,							//tau_hang_backmult
		0.500,							//hangtime
		2.000,							//hang_thresh
		0.100,							//tau_hang_decay
		"LVL");							//tag
	tx[thread].leveler.flag = TRUE;

	tx[thread].grapheq.gen = new_EQ (CXBsize (tx[thread].buf.i), tx[thread].buf.i, uni[thread].samplerate, uni[thread].wisdom.bits);
	tx[thread].grapheq.flag = FALSE;

	tx[thread].compressor.gen = newCompressor ( // (NR0V)
		CXBbase (tx[thread].buf.o_pre),
		CXBbase (tx[thread].buf.i),
		CXBsize (tx[thread].buf.i),
		3.0f,
		"CMP");
	tx[thread].compressor.flag = FALSE;

	memset ((char *) &tx[thread].squelch, 0, sizeof (tx[thread].squelch));
	tx[thread].squelch.thresh = -40.0;
	tx[thread].squelch.atten = 80.0;
	tx[thread].squelch.power = 0.0;
	tx[thread].squelch.flag = FALSE;
	tx[thread].squelch.running = tx[thread].squelch.set = FALSE;
	tx[thread].squelch.num = uni[thread].buflen - 48;

	tx[thread].alc.gen = newWcpAGC (  // (NR0V)
		(AGCMODE)5,						//mode
		1,								//0 for max(I,Q), 1 for envelope
		CXBbase (tx[thread].buf.o),		//buff pointer
		CXBsize (tx[thread].buf.o),		//io_buffsize
		uni[thread].samplerate,			//sample rate
		0.002,							//tau_attack
		0.010,							//tau_decay
		8,								//n_tau	
		1.0,							//max_gain
		1.0,							//var_gain
		1.0,							//fixed_gain
		1.0,							//max_input
		1.0,							//out_targ
		0.250,							//tau_fast_backaverage
		0.005,							//tau_fast_decay
		5.0,							//pop_ratio
		0.500,							//tau_hang_backmult
		0.500,							//hangtime
		2.000,							//hang_thresh	
		0.100,							//tau_hang_decay
		"ALC");							//tag
	tx[thread].alc.flag = TRUE;

	tx[thread].spr.gen =
		newSpeechProc (0.4f, 3.0, CXBbase (tx[thread].buf.i), CXBsize (tx[thread].buf.o));
	tx[thread].spr.flag = FALSE;

	tx[thread].hlb.gen = newHilbertsim(tx[thread].buf.i, tx[thread].buf.i);
	tx[thread].hlb.flag = TRUE;

	//tx[thread].scl.dc = cxzero;

	tx[thread].mode = uni[thread].mode.sdr;

	tx[thread].tick = 0;
}

/* how the outside world sees it */

void
setup_workspace (REAL rate, int buflen, SDRMODE mode,
                 char *wisdom, int specsize, int numrecv, int cpdsize, unsigned int thread)
{
	int k;
	//wcpfile = fopen ("wcptest.txt", "w"); // (NR0V)	

	setup_all (rate, buflen, mode, wisdom, specsize, numrecv, cpdsize, thread);

	for (k = 0; k < uni[thread].multirx.nrx; k++)
	{
		setup_rx (k, thread);
		uni[thread].multirx.act[k] = FALSE;
	}
	uni[thread].multirx.act[0] = TRUE;
	uni[thread].multirx.nac = 1;

	setup_tx (thread);
}

void
destroy_workspace (unsigned int thread)
{
	int i, k;
	//fflush (wcpfile);	
	//fclose (wcpfile); // (NR0V)

	/* TX */
	delHilsim(tx[thread].hlb.gen);
	delSpeechProc (tx[thread].spr.gen);
	delWcpAGC (tx[thread].leveler.gen); // (NR0V)
	delWcpAGC (tx[thread].alc.gen);
	delCompressor (tx[thread].compressor.gen);
	delOSC (tx[thread].osc.gen);
	delDCBlocker (tx[thread].dcb.gen);
	delvec_COMPLEX (tx[thread].filt.save);
	delFiltOvSv (tx[thread].filt.ovsv);
	delFiltOvSv (tx[thread].filt.ovsv_pre);  // (NR0V)
	delFIR_Bandpass_COMPLEX (tx[thread].filt.coef);
	delCorrectIQ (tx[thread].iqfix);
	delCXB (tx[thread].buf.o);
	delCXB (tx[thread].buf.i);
	delCXB (tx[thread].buf.o_pre); // (NR0V)
	delCXB (tx[thread].buf.i_pre);

	delEQ (tx[thread].grapheq.gen);
	delOSC (tx[thread].fm.ctcss.osc);

	// Delete preemphasis and pinching filters
	del_IIR_LPF_2P(tx[thread].fm.output_LPF1);
	del_IIR_LPF_2P(tx[thread].fm.output_LPF2);
	del_IIR_LPF_2P(tx[thread].fm.input_LPF1); 	
	del_IIR_LPF_2P(tx[thread].fm.input_LPF2);
	del_IIR_HPF_2P(tx[thread].fm.input_HPF1);	
	del_IIR_HPF_2P(tx[thread].fm.input_HPF2);

	/* RX */
	for (k = 0; k < uni[thread].multirx.nrx; k++)
	{
		del_cbl(rx[thread][k].cbl.gen);
		delWSCompander (rx[thread][k].cpd.gen);
		delSpotToneGen (rx[thread][k].spot.gen);
		delWcpAGC (rx[thread][k].wcpagc.gen); // (NR0V)
		del_nb (rx[thread][k].nb_sdrom.gen);
		del_nb (rx[thread][k].nb.gen);
		del_anf (rx[thread][k].anf.gen);      // (NR0V)
		del_anr (rx[thread][k].anr.gen);      // (NR0V)
		del_amd (rx[thread][k].amd.gen);		// (NR0V)
		delFMD (rx[thread][k].fm.gen);
		delOSC (rx[thread][k].osc.gen);
		delvec_COMPLEX (rx[thread][k].filt.save);
		delFiltOvSv (rx[thread][k].filt.ovsv);
		delFiltOvSv (rx[thread][k].filt.ovsv_notch);	// (NR0V)
		delFIR_Bandpass_COMPLEX (rx[thread][k].filt.coef);
		for(i=0; i<MAX_NOTCHES_IN_PASSBAND; i++)
			del_IIR_2P2Z(rx[thread][k].notch[i].gen);
		delCorrectIQ (rx[thread][k].iqfix);
		delCXB (rx[thread][k].buf.o);
		delCXB (rx[thread][k].buf.i);
		delCXB (rx[thread][k].buf.o_notch);
		delCXB (rx[thread][k].buf.i_notch);

		delEQ (rx[thread][k].grapheq.gen);
		del_blms(rx[thread][k].banf.gen);
		del_blms(rx[thread][k].banr.gen);
		delDCBlocker(rx[thread][k].dcb);
	}

	/* all */
	finish_spectrum (&uni[thread].spec);
}

//////////////////////////////////////////////////////////////////////////
// execution
//////////////////////////////////////////////////////////////////////////

//========================================================================
// util

PRIVATE void
CXBscl (CXB buff, REAL scl)
{
	int i;
	for (i = 0; i < CXBhave (buff); i++)
		CXBdata (buff, i) = Cscl (CXBdata (buff, i), scl);
}

PRIVATE REAL
CXBnorm (CXB buff)
{
	int i;
	REAL sum = 0.0;
	for (i = 0; i < CXBhave (buff); i++)
		sum += Csqrmag (CXBdata (buff, i));
	return (REAL) sqrt (sum);
}

PRIVATE REAL
CXBnormsqr (CXB buff)
{
	int i;
	REAL sum = 0.0;
	for (i = 0; i < CXBhave (buff); i++)
		sum += Csqrmag (CXBdata (buff, i));
	return (REAL) (sum);
}

PRIVATE REAL
CXBpeak (CXB buff)
{
	int i;
	REAL maxsam = 0.0;
	for (i = 0; i < CXBhave (buff); i++)
		maxsam = max (Cmag (CXBdata (buff, i)), maxsam);
	return maxsam;
}

PRIVATE REAL peakl(CXB buff)
{
	int i;
	REAL maxpwr = 0.0;
	for(i=0; i<CXBhave(buff); i++)
		maxpwr = max(CXBreal(buff, i), maxpwr);
	return maxpwr;
}

PRIVATE REAL peakr(CXB buff)
{
	int i;
	REAL maxpwr = 0.0;
	for(i=0; i<CXBhave(buff); i++)
		maxpwr = max(CXBimag(buff, i), maxpwr);
	return maxpwr;
}

PRIVATE REAL
CXBpeakpwr (CXB buff)
{
	int i;
	REAL maxpwr = 0.0;
	for (i = 0; i < CXBhave (buff); i++)
		maxpwr = max (Csqrmag (CXBdata (buff, i)), maxpwr);
	return maxpwr;
}
PRIVATE REAL lpeakmag(CXB buff) // (NR0V)
{
	int i;
	REAL maxmag = 0.0;
	for(i=0; i<CXBhave(buff); i++)
		maxmag = max(abs(CXBreal(buff, i)), maxmag);
	return maxmag;
}
PRIVATE REAL rpeakmag(CXB buff) // (NR0V)
{
	int i;
	REAL maxmag = 0.0;
	for(i=0; i<CXBhave(buff); i++)
		maxmag = max(abs(CXBimag(buff, i)), maxmag);
	return maxmag;
}

//========================================================================
/* all */

// unfortunate duplication here, due to
// multirx vs monotx

PRIVATE void
do_rx_meter (int k, unsigned int thread, CXB buf, int tap)
{
	COMPLEX *vec = CXBbase (buf);
	int i, len = CXBhave (buf);
	REAL tmp;

	switch (tap)
	{
		case RXMETER_PRE_CONV:
			tmp = -10000.0f;
			for (i = 0; i < len; i++)
				tmp = (REAL) max (fabs (vec[i].re), tmp);
			//fprintf(stderr, "adc_r max: %f\n", uni[thread].meter.rx.val[k][ADC_REAL]), fflush(stderr);
			uni[thread].meter.rx.val[k][ADC_REAL] = (REAL) (20.0 * log10 (tmp + 1e-10));
			tmp = -10000.0f;
			for (i = 0; i < len; i++)
				tmp = (REAL) max (fabs (vec[i].im), tmp);
			uni[thread].meter.rx.val[k][ADC_IMAG] = (REAL) (20.0 * log10 (tmp + 1e-10));
			break;
		case RXMETER_POST_FILT:
			tmp = 0;
			for (i = 0; i < len; i++)
				tmp += Csqrmag (vec[i]);
			rx[thread][k].norm = tmp / (REAL) len;
			uni[thread].meter.rx.val[k][SIGNAL_STRENGTH] =
				(REAL) (10.0 * log10 (tmp + 1e-20));
			if (uni[thread].meter.rx.mode[k] == SIGNAL_STRENGTH)
				uni[thread].meter.rx.val[k][AVG_SIGNAL_STRENGTH] = uni[thread].meter.rx.val[k][SIGNAL_STRENGTH];
			tmp = uni[thread].meter.rx.val[k][AVG_SIGNAL_STRENGTH];
			uni[thread].meter.rx.val[k][AVG_SIGNAL_STRENGTH] =
				(REAL) (0.95 * tmp +
				0.05 *uni[thread].meter.rx.val[k][SIGNAL_STRENGTH]);
			break;
		case RXMETER_POST_AGC:
			//uni[thread].meter.rx.val[k][AGC_GAIN] =
				//(REAL) (20.0 * log10 (rx[thread][k].dttspagc.gen->gain.now + 1e-10));
			uni[thread].meter.rx.val[k][AGC_GAIN] =	
				(REAL)(20.0*log10(1.0 / (rx[thread][k].wcpagc.gen->volts + 1e-16))); // (NR0V)
			//fprintf(stdout, "rx gain: %15.12f\n", uni[thread].meter.rx.val[k][AGC_GAIN]);
			//fflush(stdout);
			break;
		default:
			break;
	}
}


PRIVATE void
do_rx_spectrum (int k, unsigned int thread, CXB buf, int type)
{
	if (uni[thread].spec.flag && k == uni[thread].spec.rxk && type == uni[thread].spec.type)
	{
		if ((uni[thread].spec.type == SPEC_POST_DET) && (!rx[thread][k].bin.flag)) 
		{
			int i;
			for (i=0; i<CXBhave(rx[thread][k].buf.o);i++)
				CXBdata(uni[thread].spec.accum, uni[thread].spec.fill+i) = Cmplx(CXBreal(rx[thread][k].buf.o, i)*1.414f, 0.0);
		}
		else
		{
			memcpy ((char *) &CXBdata (uni[thread].spec.accum, uni[thread].spec.fill),
				(char *) CXBbase (buf), CXBsize (buf) * sizeof (COMPLEX));
		}
		uni[thread].spec.fill = (uni[thread].spec.fill + CXBsize (buf)) & uni[thread].spec.mask;
	}
}

PRIVATE void
do_tx_spectrum (unsigned int thread, CXB buf)
{
	if (uni[thread].spec.type == SPEC_PREMOD) 
	{
		int i;
		for (i=0; i<CXBhave(tx[thread].buf.i);i++)
			CXBdata(uni[thread].spec.accum, uni[thread].spec.fill+i) = Cmplx(CXBreal(tx[thread].buf.i, i), 0.0);
	}
	else
	{
		memcpy ((char *) &CXBdata (uni[thread].spec.accum, uni[thread].spec.fill),
			(char *) CXBbase (buf), CXBsize (buf) * sizeof (COMPLEX));
	}
	uni[thread].spec.fill = (uni[thread].spec.fill + CXBsize (buf)) & uni[thread].spec.mask;
}

//========================================================================
/* RX processing */

PRIVATE void
should_do_rx_squelch (int k, unsigned int thread)
{
	if (rx[thread][k].squelch.flag)
	{
		int i, n = CXBhave (rx[thread][k].buf.o);
		rx[thread][k].squelch.power = 0.0;

		for (i = 0; i < n; i++)
			rx[thread][k].squelch.power += Csqrmag (CXBdata (rx[thread][k].buf.o, i));

		if(10.0 * log10 (rx[thread][k].squelch.power + 1e-17) < rx[thread][k].squelch.thresh)
			rx[thread][k].squelch.set = TRUE;
		else
			rx[thread][k].squelch.set = FALSE;
	}
	else
	{
		rx[thread][k].squelch.set = FALSE;
	}
}

PRIVATE void
should_do_tx_squelch (unsigned int thread)
{
	if (tx[thread].squelch.flag)
	{
		int i, n = CXBsize (tx[thread].buf.i);
		tx[thread].squelch.power = 0.0;

		for (i = 0; i < n; i++)
			tx[thread].squelch.power += Csqrmag (CXBdata (tx[thread].buf.i, i));

		if((-30 + 10.0 * log10 (tx[thread].squelch.power + 1e-17)) < tx[thread].squelch.thresh)
			tx[thread].squelch.set = TRUE;
		else
			tx[thread].squelch.set = FALSE;

	}
	else
	{
		tx[thread].squelch.set = FALSE;
	}
}

// apply squelch
// slew into silence first time

PRIVATE void
do_squelch (int k, unsigned int thread)
{
	if (!rx[thread][k].squelch.running)
	{
		int i, m = rx[thread][k].squelch.num, n = CXBhave (rx[thread][k].buf.o) - m;

		for (i = 0; i < m; i++)
		{
			CXBdata (rx[thread][k].buf.o, i) =
				Cscl (CXBdata (rx[thread][k].buf.o, i), (REAL) (1.0 - (REAL) i / m));
		}

		memset ((void *) (CXBbase (rx[thread][k].buf.o) + m), 0, n * sizeof (COMPLEX));
		rx[thread][k].squelch.running = TRUE;
	}
	else
	{
		memset ((void *) CXBbase (rx[thread][k].buf.o),
			0, CXBhave (rx[thread][k].buf.o) * sizeof (COMPLEX));
	}
}

PRIVATE void
do_tx_squelch (unsigned int thread)
{
	int i, m = tx[thread].squelch.num, n = CXBhave (tx[thread].buf.i);
	int l = ((int)tx[thread].squelch.atten * m) / 100;

	if (!tx[thread].squelch.running)
	{
		for (i = 0; i < n; i++)
		{
			REAL scale = (REAL) (1.0 - (REAL) (i < l ? i : l) / m);
			CXBdata (tx[thread].buf.i, i) =
				Cscl (CXBdata (tx[thread].buf.i, i), scale);
		}
		tx[thread].squelch.running = TRUE;
	}
	else if (l != m)
	{
		REAL scale = (REAL) (1.0 - (REAL) l / m);
		for (i = 0; i < n; i++)
		{
			CXBdata (tx[thread].buf.i, i) =
				Cscl (CXBdata (tx[thread].buf.i, i), scale);
		}
	}
	else
	{
		memset ((void *) CXBbase (tx[thread].buf.i),
			0, CXBhave (tx[thread].buf.i) * sizeof (COMPLEX));
	}
}

// lift squelch
// slew out from silence to full scale

PRIVATE void
no_squelch (int k, unsigned int thread)
{
	if (rx[thread][k].squelch.running)
	{
		int i, m = rx[thread][k].squelch.num;

		for (i = 0; i < m; i++)
		{
			CXBdata (rx[thread][k].buf.o, i) =
				Cscl (CXBdata (rx[thread][k].buf.o, i), (REAL) i / m);
		}
		rx[thread][k].squelch.running = FALSE;
	}
}

PRIVATE void
no_tx_squelch (unsigned int thread)
{
	int i, m = tx[thread].squelch.num, n = CXBhave (tx[thread].buf.i);
	int l = (((INT) tx[thread].squelch.atten) * m) / 100;

	if (tx[thread].squelch.running)
	{
		for (i = 0; i < m; i++)
		{
			REAL scale = (REAL) (i < l ? l : i) / m;
			CXBdata (tx[thread].buf.i, i) =
				Cscl (CXBdata (tx[thread].buf.i, i), scale);
		}
		tx[thread].squelch.running = FALSE;
	}
}
/* Routine to do the actual adding of buffers through the complex linear combination required */

#if 0
void
do_rx_diversity_combine()
{
	int i, n=CXBhave (rx[0][0].buf.i);
	for (i=0;i<n;i++)
	{
		CXBdata(rx[0][0].buf.i,i) = Cscl(Cadd(CXBdata(rx[0][0].buf.i,i),Cmul(CXBdata(rx[2][0].buf.i,i),diversity.scalar)),diversity.gain);
	}
}
#endif
/* pre-condition for (nearly) all RX modes */
PRIVATE void
do_rx_pre (int k, unsigned int thread)
{
	int i, n = min (CXBhave (rx[thread][k].buf.i), uni[thread].buflen);

	// metering for uncorrected values here
	do_rx_meter (k, thread, rx[thread][k].buf.i, RXMETER_PRE_CONV);	

	if (rx[thread][k].dcb->flag) DCBlock(rx[thread][k].dcb);

	if (rx[thread][k].nb.flag)
		noiseblanker (rx[thread][k].nb.gen);
	if (rx[thread][k].nb_sdrom.flag)
		SDROMnoiseblanker (rx[thread][k].nb_sdrom.gen);

	correctIQ (rx[thread][k].buf.i, rx[thread][k].iqfix, FALSE, k);

	/* 2nd IF conversion happens here */
	if (rx[thread][k].osc.gen->Frequency != 0.0)
	{
		ComplexOSC (rx[thread][k].osc.gen);
		for (i = 0; i < n; i++)
			CXBdata (rx[thread][k].buf.i, i) = Cmul (CXBdata (rx[thread][k].buf.i, i),
			OSCCdata (rx[thread][k].osc.gen, i));
	}

	/* filtering, metering, spectrum, squelch, & AGC */

	//do_rx_meter (k, rx[thread][k].buf.i, RXMETER_PRE_FILT);

#ifndef NEW_DISPLAYS
	do_rx_spectrum (k, thread, rx[thread][k].buf.i, SPEC_PRE_FILT);
#endif
	
	if (rx[thread][k].mode != SPEC)
	{
		if (rx[thread][k].resample.flag) {
			PolyPhaseFIRF(rx[thread][k].resample.gen1r);
			PolyPhaseFIRF(rx[thread][k].resample.gen1i);
		}
		if (rx[thread][k].tick == 0)
			reset_OvSv (rx[thread][k].filt.ovsv);

		filter_OvSv (rx[thread][k].filt.ovsv);
	}
	else
	{
		memcpy (CXBbase (rx[thread][k].buf.o), CXBbase (rx[thread][k].buf.i),
			sizeof (COMPLEX) * CXBhave (rx[thread][k].buf.i));
	}
    
	CXBhave (rx[thread][k].buf.o) = CXBhave (rx[thread][k].buf.i);

	do_rx_meter (k, thread, rx[thread][k].buf.o, RXMETER_POST_FILT); //used for S-meter

#ifdef NEW_DISPLAYS
	if (rx[thread][k].hpsdr.specflag)
		Spectrum2(rx[thread][k].hpsdr.disp, rx[thread][k].hpsdr.ss, rx[thread][k].hpsdr.LO, (REAL *)CXBbase(rx[thread][k].buf.o));
#else
	do_rx_spectrum (k, thread, rx[thread][k].buf.o, SPEC_POST_FILT);
#endif

	if (rx[thread][k].cpd.flag)
		WSCompand (rx[thread][k].cpd.gen);

	should_do_rx_squelch (k, thread);

}

static int count = 0;

PRIVATE void
do_rx_post (int k, unsigned int thread)
{
	int i, n = CXBhave (rx[thread][k].buf.o);

	if(rx[thread][k].mode != FM)
	{
		if(rx[thread][k].squelch.set)
		{
			do_squelch (k, thread);
		}
		else no_squelch (k, thread);
	}

	if (rx[thread][k].grapheq.flag)
	{
		switch(rx[thread][k].mode)
		{
			case DRM:
			case DIGL:
			case DIGU: // do nothing in digital modes
				break;
			default:
				graphiceq (rx[thread][k].grapheq.gen);
				break;
		}
	}

#ifndef NEW_DISPLAYS
	do_rx_spectrum(k, thread, rx[thread][k].buf.o, SPEC_POST_DET);
#endif

	if((rx[thread][k].anf.position == 1) && rx[thread][k].mode != FM) // (NR0V)
		WcpAGC (rx[thread][k].wcpagc.gen);
	//BEGIN ANF / ANR / LATE_FILTER BLOCK
	if (rx[thread][k].anf.flag)
	{
		switch(rx[thread][k].mode)
		{
			case DRM:
			case DIGL:
			case DIGU:
			case CWL:
			case CWU:
				break;
			default:
				{
				notch (rx[thread][k].anf.gen);			// (NR0V)
				break;
				}
		}
	}
	if(rx[thread][k].anr.flag)
		noise_reduce(rx[thread][k].anr.gen);
	if (rx[thread][k].tick == 0)
		reset_OvSv (rx[thread][k].filt.ovsv_notch);
	if(rx[thread][k].anf.flag || rx[thread][k].anr.flag || (rx[thread][k].mode == AM) || (rx[thread][k].mode == SAM))
	{
		for (i = 0; i < CXBhave(rx[thread][k].buf.o); i++)	
			CXBdata(rx[thread][k].buf.i_notch, i) = Cmplx (2.0f * CXBreal(rx[thread][k].buf.o, i), 0.0f);
		filter_OvSv (rx[thread][k].filt.ovsv_notch);
		memcpy (CXBbase(rx[thread][k].buf.o), CXBbase(rx[thread][k].buf.o_notch), sizeof(COMPLEX) * CXBhave(rx[thread][k].buf.o));
	}
	//END BLOCK
#if 0
	if (diversity.flag && (k==0) && (thread==2))
		for (i = 0; i < n; i++) CXBdata(rx[thread][k].buf.o,i) = cxzero;
	else 
#endif
	for(i=0; i<MAX_NOTCHES_IN_PASSBAND; i++)
	{

		if (rx[thread][k].notch[i].flag)
			do_IIR_2P2Z(rx[thread][k].notch[i].gen);
	}
			

	if((rx[thread][k].anf.position == 0) && rx[thread][k].mode != FM) // (NR0V)
		WcpAGC (rx[thread][k].wcpagc.gen);

	do_rx_meter(k, thread, rx[thread][k].buf.o, RXMETER_POST_AGC);

	do_rx_spectrum (k, thread, rx[thread][k].buf.o, SPEC_POST_AGC);	//still used for phase display

	if (!rx[thread][k].bin.flag)
	{
		for (i = 0; i < CXBhave (rx[thread][k].buf.o); i++)
			CXBimag (rx[thread][k].buf.o, i) = CXBreal (rx[thread][k].buf.o, i);
	}

	if(uni[thread].multirx.nac == 1)
	{
		for (i = 0; i < n; i++)
			CXBdata(rx[thread][k].buf.o, i) = Cscl(Cmplx(rx[thread][k].azim.re*CXBreal(rx[thread][k].buf.o, i),
														 rx[thread][k].azim.im*CXBimag(rx[thread][k].buf.o, i)), 1.414f);
	}
	else
	{
		for (i = 0; i < n; i++)
			CXBdata(rx[thread][k].buf.o, i) = Cmplx(rx[thread][k].azim.re * CXBreal(rx[thread][k].buf.o, i),
													rx[thread][k].azim.im * CXBimag(rx[thread][k].buf.o, i));
	}

	if ((thread == 2) && (diversity.flag))
	{
		for (i=0;i< n; i++) 
			CXBdata(rx[thread][k].buf.o,i) = cxzero;
	}
	else
	{
		if (rx[thread][k].output_gain != 1.0)
		{
			for (i = 0; i < n; i++) 
				CXBdata(rx[thread][k].buf.o,i) = Cscl(CXBdata(rx[thread][k].buf.o,i),rx[thread][k].output_gain);
		}
	}

	if (rx[thread][k].resample.flag) 
	{
		PolyPhaseFIRF(rx[thread][k].resample.gen2r);
		PolyPhaseFIRF(rx[thread][k].resample.gen2i);
	}
	if (rx[thread][k].cbl.flag)
		c_block (rx[thread][k].cbl.gen);
	//if (rx[thread][k].hpsdr.specflag)
	//	Spectrum2(rx[thread][k].hpsdr.disp, rx[thread][k].hpsdr.ss, rx[thread][k].hpsdr.LO, (REAL *)CXBbase(rx[thread][k].buf.o));

}

/* demod processing */

PRIVATE void
do_rx_SBCW (int k, unsigned int thread)
{

}

PRIVATE void
do_rx_AM (int k, unsigned int thread)
{
	am_demod (rx[thread][k].amd.gen);
}

PRIVATE void
do_rx_FM (int k, unsigned int thread)
{
	FMDemod (rx[thread][k].fm.gen);
}

PRIVATE void
do_rx_DRM (int k, unsigned int thread)
{

}

PRIVATE void
do_rx_SPEC (int k, unsigned int thread)
{

}

PRIVATE void
do_rx_NIL (int k, unsigned int thread)
{
	int i, n = min (CXBhave (rx[thread][k].buf.i), uni[thread].buflen);
	for (i = 0; i < n; i++)
		CXBdata (rx[thread][k].buf.o, i) = cxzero;
}

/* overall dispatch for RX processing */

void
dump_buf (const char* filename, CXB buf)
{
	int n = CXBsize(buf);
	int i = 0;

	unsigned int temp = 0;
	unsigned short temp2 = 0;

	FILE* file = fopen(filename, "w");
	fprintf(file, "RIFF");
	
	temp = 36 + 4*2*n;
	fwrite((void*)&temp, sizeof(unsigned int), 1, file);

	fprintf(file, "WAVE");
	fprintf(file, "fmt ");
	
	temp = 16; // size of fmt chunk
	fwrite((void*)&temp, sizeof(unsigned int), 1, file);
	
	temp2 = 3; // FormatTab -- 3 for float
	fwrite((void*)&temp2, sizeof(unsigned short), 1, file);

	temp2 = 2; // wChannels
	fwrite((void*)&temp2, sizeof(unsigned short), 1, file);

	temp = (unsigned int)uni[0].samplerate; // dwSamplesPerSec
	fwrite((void*)&temp, sizeof(unsigned int), 1, file);

	temp = 2 * (unsigned int)uni[0].samplerate * 4; // dwAvgBytesPerSec
	fwrite((void*)&temp, sizeof(unsigned int), 1, file);

	temp2 = 2 * 4; // wblockAlign
	fwrite((void*)&temp2, sizeof(unsigned short), 1, file);

	temp2 = 32; // wBitsPerSample
	fwrite((void*)&temp2, sizeof(unsigned short), 1, file);

	fprintf(file, "data");

	temp = 8*n;
	fwrite((void*)&temp, sizeof(unsigned int), 1, file);

	fwrite((void*)buf->data, sizeof(REAL), n*2, file);
	/*for(i=0; i<n; i++)
	{
		fwrite((void*)&CXBreal(buf, i), sizeof(REAL), 1, file); fflush(file);
		fwrite((void*)&CXBimag(buf, i), sizeof(REAL), 1, file); fflush(file);
	}*/

	fflush(file);
	fclose(file);

	/*writer.Write(0x46464952);								// "RIFF"		-- descriptor chunk ID
			writer.Write(data_length + 36);							// size of whole file -- 1 for now
			writer.Write(0x45564157);								// "WAVE"		-- descriptor type
			writer.Write(0x20746d66);								// "fmt "		-- format chunk ID
			writer.Write((int)16);									// size of fmt chunk
			writer.Write((short)3);									// FormatTag	-- 3 for floats
			writer.Write(channels);									// wChannels
			writer.Write(sample_rate);								// dwSamplesPerSec
			writer.Write((int)(channels*sample_rate*bit_depth/8));	// dwAvgBytesPerSec
			writer.Write((short)(channels*bit_depth/8));			// wBlockAlign
			writer.Write(bit_depth);								// wBitsPerSample
			writer.Write(0x61746164);								// "data" -- data chunk ID
			writer.Write(data_length);								// chunkSize = length of data
			writer.Flush();											// write the file*/
}

PRIVATE void
do_rx (int k, unsigned int thread)
{
	do_rx_pre (k, thread);
	switch (rx[thread][k].mode)
	{
		case DIGU:
		case DIGL:
		case USB:
		case LSB:
		case CWU:
		case CWL:
		case DSB:
			do_rx_SBCW (k, thread);
			break;
		case AM:
		case SAM:
			do_rx_AM (k, thread);
			break;
		case FM:
			do_rx_FM (k, thread);
			break;
		case DRM:
			do_rx_DRM (k, thread);
			break;
		case SPEC:
		default:
			do_rx_SPEC (k, thread);
			break;
	}
	do_rx_post (k, thread);
}

//==============================================================
/* TX processing */
PRIVATE REAL mic_avg = 0.0f, mic_pk = 0.0f,
	alc_avg = 0.0f, alc_pk = 0.0f,
	lev_avg = 0.0f, lev_pk = 0.0f,
	eq_avg = 0.0f, eq_pk = 0.0f,
	comp_avg = 0.0f, comp_pk = 0.0f,
	cpdr_avg = 0.0f, cpdr_pk = 0.0f;

/* pre-condition for (nearly) all TX modes */
PRIVATE REAL peaksmooth = 0.0;
PRIVATE void
do_tx_meter (unsigned int thread, CXB buf, TXMETERTYPE mt)
{
	COMPLEX *vec = CXBbase (buf);
	int i, len = CXBhave (buf);
	REAL tmp = 0.0f;

	switch (mt)
	{
		case TX_MIC:
			for (i = 0; i < CXBhave (tx[thread].buf.i); i++) // calculate avg Mic
				mic_avg = (REAL) (0.9995 * mic_avg +
				0.0005 * Csqrmag (CXBdata (tx[thread].buf.i, i)));
			uni[thread].meter.tx.val[TX_MIC] = (REAL) (-10.0 * log10 (mic_avg + 1e-16));

			mic_pk = CXBpeak(tx[thread].buf.i);		// calculate peak mic                 
			uni[thread].meter.tx.val[TX_MIC_PK] = (REAL) (-20.0 * log10 (mic_pk + 1e-16));
			break;

		case TX_PWR:
			for (i = 0, tmp = 0.0000001f;
				i < CXBhave (tx[thread].buf.o); i++)
				tmp += Csqrmag (CXBdata (tx[thread].buf.o, i));
			uni[thread].meter.tx.val[TX_PWR] = tmp/(REAL) len;
			break;

		case TX_ALC:
			for (i = 0; i < CXBhave (tx[thread].buf.i); i++)
				alc_avg = (REAL) (0.9995 * alc_avg +
				0.0005 * Csqrmag (CXBdata (tx[thread].buf.o, i)));
			uni[thread].meter.tx.val[TX_ALC] = (REAL) (-10.0 * log10 (alc_avg + 1e-16));

			alc_pk = CXBpeak(tx[thread].buf.o);
			uni[thread].meter.tx.val[TX_ALC_PK] = (REAL) (-20.0 * log10 (alc_pk+ 1e-16));
			uni[thread].meter.tx.val[TX_ALC_G] = (REAL)(20.0*log10(1.0 / (tx[thread].alc.gen->volts + 1e-16))); // (NR0V)
			//fprintf(stdout, "pk: %15.12f  comp: %15.12f\n", uni[thread].meter.tx.val[TX_ALC_PK], uni[thread].meter.tx.val[TX_ALC_G]);
			//fflush(stdout);
			break;

		case TX_EQ:
			for (i = 0; i < CXBhave (tx[thread].buf.i); i++)
				eq_avg = (REAL) (0.9995 * eq_avg +
				0.0005 * Csqrmag (CXBdata (tx[thread].buf.i, i)));
			uni[thread].meter.tx.val[TX_EQ] = (REAL) (-10.0 * log10 (eq_avg + 1e-16));

			eq_pk = CXBpeak(tx[thread].buf.i);
			uni[thread].meter.tx.val[TX_EQ_PK] = (REAL) (-20.0 * log10 (eq_pk + 1e-16));
			break;

		case TX_LVL:
			for (i = 0; i < CXBhave (tx[thread].buf.i); i++)
				lev_avg = (REAL) (0.9995 * lev_avg +
				0.0005 * Csqrmag (CXBdata (tx[thread].buf.i, i)));
			uni[thread].meter.tx.val[TX_LVL] = (REAL) (-10.0 * log10 (lev_avg + 1e-16));

			lev_pk = CXBpeak(tx[thread].buf.i);
			uni[thread].meter.tx.val[TX_LVL_PK] = (REAL) (-20.0 * log10 (lev_pk + 1e-16));
			//uni[thread].meter.tx.val[TX_LVL_G] = (REAL)(20.0*log10(tx[thread].leveler.gen->gain.now + 1e-16));
			uni[thread].meter.tx.val[TX_LVL_G] = (REAL)(20.0*log10(1.0 / (tx[thread].leveler.gen->volts + 1e-16))); // (NR0V)
			break;

		case TX_COMP:
			for (i = 0; i < CXBhave (tx[thread].buf.i); i++)
				comp_avg = (REAL) (0.9995 * comp_avg +
				0.0005 * Csqrmag (CXBdata (tx[thread].buf.i, i)));
			uni[thread].meter.tx.val[TX_COMP] = (REAL) (-10.0 * log10 (comp_avg + 1e-16));

			comp_pk = CXBpeak(tx[thread].buf.i);
			uni[thread].meter.tx.val[TX_COMP_PK] = (REAL) (-20.0 * log10 (comp_pk + 1e-16));
			break;

		case TX_CPDR:
			for (i = 0; i < CXBhave (tx[thread].buf.i); i++)
				cpdr_avg = (REAL) (0.9995 * cpdr_avg +
				0.0005 * Csqrmag (CXBdata (tx[thread].buf.i, i)));
			uni[thread].meter.tx.val[TX_CPDR] = (REAL) (-10.0 * log10 (cpdr_avg + 1e-16));

			cpdr_pk = CXBpeak(tx[thread].buf.i);
			uni[thread].meter.tx.val[TX_CPDR_PK] = (REAL) (-20.0 * log10 (cpdr_pk + 1e-16));
			break;

		default:
			break;
	}
}

PRIVATE void
do_tx_pre (unsigned int thread)
{
	int i, n = CXBhave (tx[thread].buf.i);
	for (i = 0; i < n; i++)
		CXBdata (tx[thread].buf.i, i) = Cmplx (CXBimag (tx[thread].buf.i, i), 0.0);
	//hilsim_transform(tx[thread].hlb.gen);
//	fprintf(stderr,"Peak value = %f\n",CXBpeakpwr(tx[thread].buf.i));
	if (tx[thread].dcb.flag)
		DCBlock (tx[thread].dcb.gen);

	do_tx_meter (thread, tx[thread].buf.i, TX_MIC);
	//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));

	should_do_tx_squelch(thread);
	if(tx[thread].squelch.set)
		do_tx_squelch (thread);
	else //if (!tx[thread].squelch.set)
		no_tx_squelch (thread);

	switch(tx[thread].mode)
	{
		case DIGU:
		case DIGL:
		case DRM:
			do_tx_meter (thread, tx[thread].buf.i, TX_EQ);
			do_tx_meter (thread, tx[thread].buf.i, TX_LVL);
			do_tx_meter (thread, tx[thread].buf.i, TX_COMP);
			do_tx_meter (thread, tx[thread].buf.i, TX_CPDR);
			break;
		default:
			if (tx[thread].grapheq.flag)
				graphiceq (tx[thread].grapheq.gen);
			do_tx_meter (thread, tx[thread].buf.i, TX_EQ);
			//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));

			if (tx[thread].leveler.flag) // (NR0V)
				WcpAGC (tx[thread].leveler.gen);	
			do_tx_meter (thread, tx[thread].buf.i, TX_LVL);
			//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));

			//do_tx_meter (thread, tx[thread].buf.i, TX_ALC);
			//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));

			if (tx[thread].spr.flag)
				SpeechProcessor (tx[thread].spr.gen);						
			do_tx_meter (thread, tx[thread].buf.i, TX_COMP);
			//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));

			if(tx[thread].mode != FM)
			{
				if (tx[thread].compressor.flag) // (NR0V)
				{
					for (i = 0; i < CXBhave(tx[thread].buf.i); i++)	
						CXBdata(tx[thread].buf.i_pre, i) = Cmplx (2.0f * CXBreal(tx[thread].buf.i, i), 0.0);

					CXBhave(tx[thread].buf.i_pre) = CXBhave(tx[thread].buf.i);
					CXBhave (tx[thread].buf.o_pre) = CXBhave (tx[thread].buf.i_pre);

					if (tx[thread].tick == 0)
						reset_OvSv (tx[thread].filt.ovsv_pre);
					//fprintf (wcpfile, "[%.2f,%.2f]  IN Pre Filter\n", lpeakmag(tx[thread].buf.i_pre), rpeakmag(tx[thread].buf.i_pre));
					filter_OvSv (tx[thread].filt.ovsv_pre);
					//fprintf (wcpfile, "[%.2f,%.2f]  OUT Pre Filter\n", lpeakmag(tx[thread].buf.o_pre), rpeakmag(tx[thread].buf.o_pre));

					Compressor (tx[thread].compressor.gen);
					//fprintf(wcpfile, "[%.2f,%.2f]  OUT Compressor\n", lpeakmag(tx[thread].buf.i), rpeakmag(tx[thread].buf.i));
				}

				do_tx_meter (thread, tx[thread].buf.i, TX_CPDR);
			}
			
			//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));

			break;						
	}
}

PRIVATE void
do_tx_post (unsigned int thread)
{
	CXBhave (tx[thread].buf.o) = CXBhave (tx[thread].buf.i);

	if (tx[thread].tick == 0)
		reset_OvSv (tx[thread].filt.ovsv);

	//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));
	filter_OvSv (tx[thread].filt.ovsv);
	
	//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.o), peakr(tx[thread].buf.o));

	if (tx[thread].alc.flag || (tx[thread].mode == AM) || (tx[thread].mode == SAM))  // (NR0V)
		WcpAGC (tx[thread].alc.gen);

	if ((tx[thread].mode == AM) || (tx[thread].mode == SAM))
	{
		int i;
		for (i = 0; i < CXBhave (tx[thread].buf.o); i++)
		{
			CXBreal(tx[thread].buf.o, i) = 
				tx[thread].am.carrier_level + (1.0f - tx[thread].am.carrier_level) * CXBreal(tx[thread].buf.o, i);
			CXBimag(tx[thread].buf.o, i) = 0.0f;
		}
	}


	do_tx_meter (thread, tx[thread].buf.o, TX_ALC);

	if (uni[thread].spec.flag)
		do_tx_spectrum (thread, tx[thread].buf.o);
	
	if (tx[thread].osc.gen->Frequency != 0.0)
	{
		int i;
		ComplexOSC (tx[thread].osc.gen);
		for (i = 0; i < CXBhave (tx[thread].buf.o); i++)
		{
			CXBdata (tx[thread].buf.o, i) =
				Cmul (CXBdata (tx[thread].buf.o, i), OSCCdata (tx[thread].osc.gen, i));
		}
	}

	correctIQ (tx[thread].buf.o, tx[thread].iqfix, TRUE,0);

	// meter modulated signal
	do_tx_meter (thread, tx[thread].buf.o, TX_PWR);

	//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.o), peakr(tx[thread].buf.o));
	//fprintf(stderr,"\n");
	//fflush(stderr);
	/*{	
		double buffpeak = CXBpeak(tx[thread].buf.o);
		fprintf (wcpfile, "%.17f\n", buffpeak);
		if (buffpeak >= 1.0) fprintf (wcpfile, "******************************************************\n");
	}*/
}

/* modulator processing */

PRIVATE void
do_tx_SBCW (unsigned int thread)
{
	int n = min (CXBhave (tx[thread].buf.i), uni[thread].buflen);

	//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));
	if (tx[thread].mode != DSB)
		CXBscl (tx[thread].buf.i, 2.0f);
}

PRIVATE void
do_tx_FM (unsigned int thread)
{
	int i, n = min (CXBhave (tx[thread].buf.i), uni[thread].buflen);
	REAL clip = 1.0;
	REAL threshold = tx[thread].fm.clip_threshold;
	REAL mag = 0;
	REAL deemphasis_in = 0;
	REAL preemphasis_in = 0;
	REAL k_preemphasis = tx[thread].fm.k_preemphasis;
	REAL k_deemphasis = tx[thread].fm.k_deemphasis;

	//fprintf(stderr,"[%.2f,%.2f]  ", peakl(tx[thread].buf.i), peakr(tx[thread].buf.i));

	//Input BPF 300-3000 Hz
	do_IIR_LPF_2P(tx[thread].fm.input_LPF1);
	do_IIR_LPF_2P(tx[thread].fm.input_LPF2);
	do_IIR_HPF_2P(tx[thread].fm.input_HPF1);
	do_IIR_HPF_2P(tx[thread].fm.input_HPF2);

	for (i = 0; i < n; i++)
	{
		//Preemphasis (high-pass filter)
		//		temp = (1/k)in + (k-1)/k * temp
		//		out = in - temp
		//		k = 1 + Fs/(2*pi*fo)

		preemphasis_in = CXBreal (tx[thread].buf.i, i);
		tx[thread].fm.preemphasis_filter = (preemphasis_in/k_preemphasis) + (k_preemphasis-1.0f)/k_preemphasis*tx[thread].fm.preemphasis_filter;
		CXBreal(tx[thread].buf.i, i) = 3.0f*(preemphasis_in - tx[thread].fm.preemphasis_filter);
	

		////Soft Clipper
		mag = abs(CXBreal(tx[thread].buf.i, i));
		if (CXBreal(tx[thread].buf.i, i) > threshold)
			CXBreal(tx[thread].buf.i, i) = (1.0f-threshold)*(1.0f - expf((mag-threshold)/(threshold-1.0f)))+threshold;
		else if (CXBreal(tx[thread].buf.i, i) < -threshold)
			CXBreal(tx[thread].buf.i, i) = -((1.0f-threshold)*(1.0f - expf((mag-threshold)/(threshold-1.0f)))+threshold);
	}
		
	//3000 Hz LPF Output
	do_IIR_LPF_2P(tx[thread].fm.output_LPF1);
	do_IIR_LPF_2P(tx[thread].fm.output_LPF2);

	if(tx[thread].fm.ctcss.flag)
		ComplexOSC(tx[thread].fm.ctcss.osc);
	
	for(i = 0; i < n; i++)
	{
		////Demphasis (low-pass filter) -- Not needed in TX
		//deemphasis_in = 4.0 * CXBreal(tx[thread].buf.i, i);
		//tx[thread].fm.deemphasis_out = (deemphasis_in/k_deemphasis) + (k_deemphasis-1.0)/k_deemphasis*tx[thread].fm.deemphasis_out;
		if(tx[thread].fm.ctcss.flag)
			CXBreal(tx[thread].buf.i, i) = CXBreal(tx[thread].buf.i, i) + tx[thread].fm.ctcss.amp*OSCCdata(tx[thread].fm.ctcss.osc, i).re;

		//FM modulator
		tx[thread].fm.phase += CXBreal(tx[thread].buf.i, i) * tx[thread].fm.cvtmod2freq;
		//tx[thread].fm.phase += tx[thread].fm.deemphasis_out * tx[thread].fm.cvtmod2freq;
		CXBdata (tx[thread].buf.i, i) =	Cmplx ((REAL) cos (tx[thread].fm.phase), (IMAG) sin (tx[thread].fm.phase));


		//tx[thread].fm.phase += CXBreal (tx[thread].buf.i, i) * tx[thread].fm.cvtmod2freq;
		//CXBdata (tx[thread].buf.i, i) =
		//	Cmplx ((REAL) cos (tx[thread].fm.phase), (IMAG) sin (tx[thread].fm.phase));
	}

	do_tx_meter (thread, tx[thread].buf.i, TX_CPDR);
}

PRIVATE void
do_tx_NIL (unsigned int thread)
{
	int i, n = min (CXBhave (tx[thread].buf.i), uni[thread].buflen);
	for (i = 0; i < n; i++)
		CXBdata (tx[thread].buf.i, i) = cxzero;
}

/* general TX processing dispatch */

PRIVATE void
do_tx (unsigned int thread)
{
	do_tx_pre (thread);
	switch (tx[thread].mode)
	{
		case USB:
		case LSB:
		case CWU:
		case CWL:
		case DIGU:
		case DIGL:
		case DRM:
		case DSB:
			do_tx_SBCW (thread);
			break;
		case AM:
		case SAM:

			break;
		case FM:
			do_tx_FM (thread);
			break;
		case SPEC:
		default:
			do_tx_NIL (thread);
			break;
	}
	do_tx_post (thread);
	//fprintf(stderr,"%f\n",Cmag(CXBdata(tx[thread].buf.o,0))),fflush(stderr);
}

//========================================================================
/* overall buffer processing;
   come here when there are buffers to work on */

void
process_samples (float *bufl, float *bufr, float *auxl, float *auxr, int n, unsigned int thread)
{
	int i, k;

	switch (uni[thread].mode.trx)
	{
		case RX:


			// make copies of the input for all receivers
			for (k = 0; k < uni[thread].multirx.nrx; k++)
			{
				BOOLEAN kdone=FALSE;
				int kone = -1;
				if (uni[thread].multirx.act[k])
				{
					if (!kdone) 
					{
						kdone = TRUE;
						kone = k;
						for (i = 0; i < n; i++)
						{
							CXBimag (rx[thread][k].buf.i, i) =
								bufl[i], CXBreal (rx[thread][k].buf.i, i) = bufr[i];
						}
						CXBhave (rx[thread][k].buf.i) = n;
					} 
					else memcpy(rx[thread][k].buf.i,rx[thread][kone].buf.i,CXBhave(rx[thread][kdone].buf.i)*sizeof(COMPLEX));
				}
			}
			
			// prepare buffers for mixing
			memset ((char *) bufl, 0, n * sizeof (float));
			memset ((char *) bufr, 0, n * sizeof (float));

			// run all receivers
			for (k = 0; k < uni[thread].multirx.nrx; k++)
			{
                if (uni[thread].multirx.act[k])
				{
					do_rx (k, thread), rx[thread][k].tick++;
					// mix
					for (i = 0; i < n; i++)
					{
						bufl[i] += (float) CXBimag (rx[thread][k].buf.o, i);
						bufr[i] += (float) CXBreal (rx[thread][k].buf.o, i);
					}
					CXBhave (rx[thread][k].buf.o) = n;
				}
			}

			// late mixing of aux buffers
#if 0
			if (uni[thread].mix.rx.flag)
			{
				for (i = 0; i < n; i++)
				{
					bufl[i] += (float) (auxl[i] * uni[thread].mix.rx.gain),
						bufr[i] += (float) (auxr[i] * uni[thread].mix.rx.gain);
				}
			}
#endif
			break;

		case TX:
#if 0
			// early mixing of aux buffers
			if (uni[thread].mix.tx.flag)
			{
				for (i = 0; i < n; i++)
				{
					bufl[i] += (float) (auxl[i] * uni[thread].mix.tx.gain),
						bufr[i] += (float) (auxr[i] * uni[thread].mix.tx.gain);
				}
			}
#endif
			for (i = 0; i < n; i++)
			{
				CXBimag (tx[thread].buf.i, i) = bufl[i];
				CXBreal (tx[thread].buf.i, i) = bufr[i];
			}

			CXBhave (tx[thread].buf.i) = n;
			tx[thread].norm = CXBpeak (tx[thread].buf.i);

			do_tx (thread), tx[thread].tick++;

			for (i = 0; i < n; i++)
				bufl[i] = (float) CXBimag (tx[thread].buf.o, i),
				bufr[i] = (float) CXBreal (tx[thread].buf.o, i);
			CXBhave (tx[thread].buf.o) = n;

			break;
	}

	uni[thread].tick++;
}
