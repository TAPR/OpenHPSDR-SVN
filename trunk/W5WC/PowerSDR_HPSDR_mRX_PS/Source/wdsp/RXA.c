/*  RXA.c

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

void create_rxa (int channel)
{
	rxa[channel].mode = RXA_LSB;
	rxa[channel].inbuff  = (double *) malloc0 (1 * ch[channel].dsp_insize  * sizeof (complex));
	rxa[channel].outbuff = (double *) malloc0 (1 * ch[channel].dsp_outsize * sizeof (complex));
	rxa[channel].midbuff = (double *) malloc0 (2 * ch[channel].dsp_size    * sizeof (complex));

	// shift to select a slice of spectrum
	rxa[channel].shift.p = create_shift (
		1,												// run
		ch[channel].dsp_insize,							// input buffer size
		rxa[channel].inbuff,							// pointer to input buffer
		rxa[channel].inbuff,							// pointer to output buffer
		ch[channel].in_rate,							// samplerate
		0.0);											// amount to shift (Hz)

	// resample to dsp rate for main processing
	rxa[channel].rsmpin.p = create_resample (
		0,												// run - will be turned ON below if needed
		ch[channel].dsp_insize,							// input buffer size
		rxa[channel].inbuff,							// pointer to input buffer
		rxa[channel].midbuff,							// pointer to output buffer
		ch[channel].in_rate,							// input samplerate
		ch[channel].dsp_rate,							// output samplerate
		1.0);											// gain

	// signal generator
	rxa[channel].gen0.p = create_gen (
		0,												// run
		ch[channel].dsp_size,							// buffer size
		rxa[channel].midbuff,							// input buffer
		rxa[channel].midbuff,							// output buffer
		ch[channel].dsp_rate,							// sample rate
		2);												// mode

	// adc (input) meter
	rxa[channel].adcmeter.p = create_meter (
		1,												// run
		0,												// optional pointer to another 'run'
		ch[channel].dsp_size,							// size
		rxa[channel].midbuff,							// pointer to buffer
		ch[channel].dsp_rate,							// samplerate
		0.100,											// averaging time constant
		0.100,											// peak decay time constant
		rxa[channel].meter,								// result vector
		RXA_ADC_AV,										// index for average value
		RXA_ADC_PK,										// index for peak value
		-1,												// index for gain value
		0);												// pointer for gain computation

	// bandpass filter
	rxa[channel].bp0.p = create_bandpass (
		1,												// run - always ON
		ch[channel].dsp_size,							// buffer size
		rxa[channel].midbuff,							// pointer to input buffer
		rxa[channel].midbuff,							// pointer to output buffer
		-4150.0,										// lower filter frequency
		-150.0,											// upper filter frequency
		ch[channel].dsp_rate,							// sample rate
		1,												// wintype
		1.0);											// gain

	// send spectrum display
	rxa[channel].sender.p = create_sender (
		channel == 0,									// run
		0,												// flag
		0,												// mode
		ch[channel].dsp_size,							// size
		rxa[channel].midbuff,							// pointer to input buffer
		0,												// arg0 <- disp
		1,												// arg1 <- ss
		0,												// arg2 <- LO
		0);												// arg3 <- NOT USED

	// S-meter
	rxa[channel].smeter.p = create_meter (
		1,												// run
		0,												// optional pointer to another 'run'
		ch[channel].dsp_size,							// size
		rxa[channel].midbuff,							// pointer to buffer
		ch[channel].dsp_rate,							// samplerate
		0.100,											// averaging time constant
		0.100,											// peak decay time constant
		rxa[channel].meter,								// result vector
		RXA_S_AV,										// index for average value
		RXA_S_PK,										// index for peak value
		-1,												// index for gain value
		0);												// pointer for gain computation

	// AM squelch
	rxa[channel].amsq.p = create_amsq (
		0,												// run
		ch[channel].dsp_size,							// buffer size
		rxa[channel].midbuff,							// pointer to signal input buffer used by xamsq
		rxa[channel].midbuff,							// pointer to signal output buffer used by xamsq
		rxa[channel].midbuff,							// pointer to trigger buffer that xamsqcap will capture
		ch[channel].dsp_rate,							// sample rate
		0.010,											// time constant for averaging signal level
		0.070,											// signal up transition time
		0.070,											// signal down transition time
		0.009,											// signal level to initiate tail
		0.010,											// signal level to initiate unmute
		0.000,											// minimum tail length
		1.500,											// maximum tail length
		0.0);											// muted gain

	// AM demod
	rxa[channel].amd.p = create_amd (
		0,												// run - OFF by default
		ch[channel].dsp_size,							// buffer size
		rxa[channel].midbuff,							// pointer to input buffer
		rxa[channel].midbuff,							// pointer to output buffer
		0,												// mode:  0->AM, 1->SAM
		1,												// levelfade:  0->OFF, 1->ON
		0,												// sideband mode:  0->OFF
		ch[channel].dsp_rate,							// sample rate
		-2000.0,										// minimum lock frequency
		+2000.0,										// maximum lock frequency
		1.0,											// zeta
		250.0,											// omegaN
		0.02,											// tauR
		1.4);											// tauI

	// FM demod
	rxa[channel].fmd.p = create_fmd (
		0,												// run
		ch[channel].dsp_size,							// buffer size
		rxa[channel].midbuff,							// pointer to input buffer
		rxa[channel].midbuff,							// pointer to output buffer
		ch[channel].dsp_rate,							// sample rate
		5000.0,											// deviation
		300.0,											// f_low
		3000.0,											// f_high
		-8000.0,										// fmin
		+8000.0,										// fmax
		1.0,											// zeta
		20000.0,										// omegaN
		0.02,											// tau - for dc removal
		0.5,											// audio gain
		1,												// run tone filter
		254.1);											// ctcss frequency

	// FM squelch
	rxa[channel].fmsq.p = create_fmsq (
		0,												// run
		ch[channel].dsp_size,							// buffer size
		rxa[channel].midbuff,							// pointer to input signal buffer
		rxa[channel].midbuff,							// pointer to output signal buffer
		rxa[channel].fmd.p->audio,						// pointer to trigger buffer
		ch[channel].dsp_rate,							// sample rate
		5000.0,											// cutoff freq for noise filter (Hz)
		rxa[channel].fmd.p->pllpole,					// pole frequency of the fmd pll (Hz)
		0.100,											// delay time after channel flush
		0.001,											// tau for noise averaging
		0.100,											// tau for long noise averaging
		0.050,											// signal up transition time
		0.010,											// signal down transition time
		0.750,											// noise level to initiate tail
		0.562,											// noise level to initiate unmute
		0.000,											// minimum tail time
		1.200);											// maximum tail time

	// EQ
	{
	double default_F[11] = {0.0,  32.0,  63.0, 125.0, 250.0, 500.0, 1000.0, 2000.0, 4000.0, 8000.0, 16000.0};
	//double default_G[11] = {0.0, -12.0, -12.0, -12.0,  -1.0,  +1.0,   +4.0,   +9.0,  +12.0,  -10.0,   -10.0};
	double default_G[11] =   {0.0,   0.0,   0.0,   0.0,   0.0,   0.0,    0.0,    0.0,    0.0,    0.0,     0.0};
	rxa[channel].eq.p = create_eq (
		0,												// run - OFF by default
		ch[channel].dsp_size,
		rxa[channel].midbuff,							// pointer to input buffer
		rxa[channel].midbuff,							// pointer to output buffer
		10,												// number of frequencies
		default_F,										// frequency vector
		default_G,										// gain vector
		0,												// cutoff mode
		1,												// method
		ch[channel].dsp_rate);							// sample rate
	}

	// ANF
	rxa[channel].anf.p = create_anf (
		0,												// run - OFF by default
		0,												// position
		ch[channel].dsp_size,							// buffer size
		rxa[channel].midbuff,							// pointer to input buffer
		rxa[channel].midbuff,							// pointer to output buffer
		ANF_DLINE_SIZE,									// dline_size
		256,											// taps
		64,												// delay
		0.0001,											// two_mu
		0.1,											// gamma
		1.0,											// lidx
		0.0,											// lidx_min
		200.0,											// lidx_max
		6.25e-12,										// ngamma
		6.25e-10,										// den_mult
		1.0,											// lincr
		3.0);											// ldecr

	// ANR
	rxa[channel].anr.p = create_anr (
		0,												// run - OFF by default
		0,												// position
		ch[channel].dsp_size,							// buffer size
		rxa[channel].midbuff,							// pointer to input buffer
		rxa[channel].midbuff,							// pointer to output buffer
		ANR_DLINE_SIZE,									// dline_size
		256,											// taps
		64,												// delay			
		0.0001,											// two_mu	
		0.1,											// gamma
		120.0,											// lidx
		120.0,											// lidx_min
		200.0,											// lidx_max
		0.001,											// ngamma
		6.25e-10,										// den_mult
		1.0,											// lincr
		3.0);											// ldecr

	// AGC
	rxa[channel].agc.p = create_wcpagc (
		1,												// run
		3,												// mode
		1,												// peakmode = envelope
		rxa[channel].midbuff,							// pointer to input buffer
		rxa[channel].midbuff,							// pointer to output buffer
		ch[channel].dsp_size,							// buffer size
		ch[channel].dsp_rate,							// sample rate
		0.002,											// tau_attack
		0.250,											// tau_decay
		4,												// n_tau
		10000.0,										// max_gain
		1.5,											// var_gain
		1000.0,											// fixed_gain
		1.0,											// max_input
		1.0,											// out_target
		0.250,											// tau_fast_backaverage
		0.005,											// tau_fast_decay
		5.0,											// pop_ratio
		0.500,											// tau_hang_backmult
		0.250,											// hangtime
		0.250,											// hang_thresh
		0.100);											// tau_hang_decay

	// agc gain meter
	rxa[channel].agcmeter.p = create_meter (
		1,												// run
		0,												// optional pointer to another 'run'
		ch[channel].dsp_size,							// size
		rxa[channel].midbuff,							// pointer to buffer
		ch[channel].dsp_rate,							// samplerate
		0.100,											// averaging time constant
		0.100,											// peak decay time constant
		rxa[channel].meter,								// result vector
		RXA_AGC_AV,										// index for average value
		RXA_AGC_PK,										// index for peak value
		RXA_AGC_GAIN,									// index for gain value
		&rxa[channel].agc.p->gain);						// pointer for gain computation

	// bandpass filter
	rxa[channel].bp1.p = create_bandpass (
		1,												// run - used only with ( AM || ANF || ANR )
		ch[channel].dsp_size,							// buffer size
		rxa[channel].midbuff,							// pointer to input buffer
		rxa[channel].midbuff,							// pointer to output buffer
		-4150.0,										// lower filter frequency
		-150.0,											// upper filter frequency
		ch[channel].dsp_rate,							// sample rate
		1,												// wintype
		1.0);											// gain

	// pull phase & scope display data
	rxa[channel].sip1.p = create_siphon (
		1,												// run - needed only for phase display
		ch[channel].dsp_size,							// size of input buffer
		rxa[channel].midbuff,							// input buffer
		4096,											// number of samples to store
		4096,											// fft size for spectrum
		0);												// specmode

	// carrier block
	rxa[channel].cbl.p = create_cbl (
		0,												// run - needed only if set to ON
		ch[channel].dsp_size,							// buffer size
		rxa[channel].midbuff,							// pointer to input buffer
		rxa[channel].midbuff,							// pointer to output buffer
		0,												// mode
		ch[channel].dsp_rate,							// sample rate
		0.02);											// tau

	// patchpanel
	rxa[channel].panel.p = create_panel (
		channel,										// channel number
		1,												// run
		ch[channel].dsp_size,							// size
		rxa[channel].midbuff,							// pointer to input buffer
		rxa[channel].midbuff,							// pointer to output buffer
		4.0,											// gain1
		1.0,											// gain2I
		1.0,											// gain2Q
		3,												// 3 for I and Q
		0);												// no copy

	// resample
	rxa[channel].rsmpout.p = create_resample (
		0,												// run - will be turned ON below if needed
		ch[channel].dsp_size,							// input buffer size			 
		rxa[channel].midbuff,							// pointer to input buffer
		rxa[channel].outbuff,							// pointer to output buffer
		ch[channel].dsp_rate,							// input sample rate
		ch[channel].out_rate,							// output sample rate
		1.0);											// gain

	// turn OFF / ON resamplers as needed
	RXAResCheck (channel);
}

void destroy_rxa (int channel)
{
	destroy_resample (rxa[channel].rsmpout.p);
	destroy_panel (rxa[channel].panel.p);
	destroy_cbl (rxa[channel].cbl.p);
	destroy_siphon (rxa[channel].sip1.p);
	destroy_bandpass (rxa[channel].bp1.p);
	destroy_meter (rxa[channel].agcmeter.p);
	destroy_wcpagc (rxa[channel].agc.p);
	destroy_anr (rxa[channel].anr.p);
	destroy_anf (rxa[channel].anf.p);
	destroy_eq (rxa[channel].eq.p);
	destroy_fmsq (rxa[channel].fmsq.p);
	destroy_fmd (rxa[channel].fmd.p);
	destroy_amd (rxa[channel].amd.p);
	destroy_amsq (rxa[channel].amsq.p);
	destroy_meter (rxa[channel].smeter.p);
	destroy_sender (rxa[channel].sender.p);
	destroy_bandpass (rxa[channel].bp0.p);
	destroy_meter (rxa[channel].adcmeter.p);
	destroy_gen (rxa[channel].gen0.p);
	destroy_resample (rxa[channel].rsmpin.p);
	destroy_shift (rxa[channel].shift.p);
	_aligned_free (rxa[channel].midbuff);
	_aligned_free (rxa[channel].outbuff);
	_aligned_free (rxa[channel].inbuff);
}

void flush_rxa (int channel)
{
	memset (rxa[channel].inbuff,  0, 1 * ch[channel].dsp_insize  * sizeof (complex));
	memset (rxa[channel].outbuff, 0, 1 * ch[channel].dsp_outsize * sizeof (complex));
	memset (rxa[channel].midbuff, 0, 2 * ch[channel].dsp_size    * sizeof (complex));
	flush_shift (rxa[channel].shift.p);
	flush_resample (rxa[channel].rsmpin.p);
	flush_gen (rxa[channel].gen0.p);
	flush_meter (rxa[channel].adcmeter.p);
	flush_bandpass (rxa[channel].bp0.p);
	flush_sender (rxa[channel].sender.p);
	flush_meter (rxa[channel].smeter.p);
	flush_amsq (rxa[channel].amsq.p);
	flush_amd (rxa[channel].amd.p);
	flush_fmd (rxa[channel].fmd.p);
	flush_fmsq (rxa[channel].fmsq.p);
	flush_eq (rxa[channel].eq.p);
	flush_anf (rxa[channel].anf.p);
	flush_anr (rxa[channel].anr.p);
	flush_wcpagc (rxa[channel].agc.p);
	flush_meter (rxa[channel].agcmeter.p);
	flush_bandpass (rxa[channel].bp1.p);
	flush_siphon (rxa[channel].sip1.p);
	flush_cbl (rxa[channel].cbl.p);
	flush_panel (rxa[channel].panel.p);
	flush_resample (rxa[channel].rsmpout.p);
}

void xrxa (int channel)
{
	xshift (rxa[channel].shift.p);
	xresample (rxa[channel].rsmpin.p);
	xgen (rxa[channel].gen0.p);
	xmeter (rxa[channel].adcmeter.p);
	xbandpass (rxa[channel].bp0.p);
	xmeter (rxa[channel].smeter.p);
	xsender (rxa[channel].sender.p);
	xamsqcap (rxa[channel].amsq.p);
	xamd (rxa[channel].amd.p);
	xfmd (rxa[channel].fmd.p);
	xfmsq (rxa[channel].fmsq.p);
	xeq (rxa[channel].eq.p);
	xanf (rxa[channel].anf.p, 0);
	xanr (rxa[channel].anr.p, 0);
	xwcpagc (rxa[channel].agc.p);
	xanf (rxa[channel].anf.p, 1);
	xanr (rxa[channel].anr.p, 1);
	xmeter (rxa[channel].agcmeter.p);
	xbandpass (rxa[channel].bp1.p);
	xsiphon (rxa[channel].sip1.p);
	xcbl (rxa[channel].cbl.p);
	xpanel (rxa[channel].panel.p);
	xamsq (rxa[channel].amsq.p);
	xresample (rxa[channel].rsmpout.p);
}

/********************************************************************************************************
*																										*
*											RXA Properties												*
*																										*
********************************************************************************************************/

PORT
void SetRXAMode (int channel, int mode)
{
	EnterCriticalSection (&ch[channel].csDSP);
	rxa[channel].mode = mode;
	rxa[channel].amd.p->run  = 0;
	rxa[channel].fmd.p->run  = 0;
	rxa[channel].agc.p->run  = 1;
	switch (mode)
	{
	case RXA_AM:
		rxa[channel].amd.p->run  = 1;
		rxa[channel].amd.p->mode = 0;
		break;
	case RXA_SAM:
		rxa[channel].amd.p->run  = 1;
		rxa[channel].amd.p->mode = 1;
		break;
	case RXA_DSB:
		
		break;
	case RXA_FM:
		rxa[channel].fmd.p->run  = 1;
		rxa[channel].agc.p->run  = 0;
		break;
	default:

		break;
	}
	RXAbp1Check (channel);
	LeaveCriticalSection (&ch[channel].csDSP);
}

void RXAResCheck (int channel)
{	// turn OFF/ON resamplers depending upon whether they're needed
	RESAMPLE a = rxa[channel].rsmpin.p;
	if (ch[channel].in_rate  != ch[channel].dsp_rate)	a->run = 1;
	else												a->run = 0;
	a = rxa[channel].rsmpout.p;
	if (ch[channel].dsp_rate != ch[channel].out_rate)	a->run = 1;
	else												a->run = 0;
}

void RXAbp1Check (int channel)
{
	int old = rxa[channel].bp1.p->run;
	if ((rxa[channel].amd.p->run == 1) ||
		(rxa[channel].anf.p->run == 1) ||
		(rxa[channel].anr.p->run == 1))		rxa[channel].bp1.p->run = 1;
	else									rxa[channel].bp1.p->run = 0;
	if (!old && rxa[channel].bp1.p->run) flush_bandpass (rxa[channel].bp1.p);
}