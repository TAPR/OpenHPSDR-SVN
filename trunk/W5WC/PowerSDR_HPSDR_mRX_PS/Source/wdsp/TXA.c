/*  TXA.c

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2013, 2014 Warren Pratt, NR0V

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

void create_txa (int channel)
{
	txa[channel].mode = TXA_LSB;
	txa[channel].inbuff  = (double *) malloc0 (1 * ch[channel].dsp_insize  * sizeof (complex));
	txa[channel].outbuff = (double *) malloc0 (1 * ch[channel].dsp_outsize * sizeof (complex));
	txa[channel].midbuff = (double *) malloc0 (2 * ch[channel].dsp_size    * sizeof (complex));

	txa[channel].rsmpin.p = create_resample (
		0,											// run - will be turned on below if needed
		ch[channel].dsp_insize,						// input buffer size
		txa[channel].inbuff,						// pointer to input buffer
		txa[channel].midbuff,						// pointer to output buffer
		ch[channel].in_rate,						// input sample rate
		ch[channel].dsp_rate, 						// output sample rate
		0.0,										// select cutoff automatically
		0,											// select ncoef automatically
		1.0);										// gain

	txa[channel].gen0.p = create_gen (
		0,											// run
		ch[channel].dsp_size,						// buffer size
		txa[channel].midbuff,						// input buffer
		txa[channel].midbuff,						// output buffer
		ch[channel].dsp_rate,						// sample rate
		2);											// mode
	
	txa[channel].panel.p = create_panel (
		channel,									// channel number
		1,											// run
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// pointer to input buffer
		txa[channel].midbuff,						// pointer to output buffer
		1.0,										// gain1
		1.0,										// gain2I
		1.0,										// gain2Q
		2,											// 1 to use Q, 2 to use I for input
		0);											// 0, no copy

	txa[channel].micmeter.p = create_meter (
		1,											// run
		0,											// optional pointer to another 'run'
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// pointer to buffer
		ch[channel].dsp_rate,						// samplerate
		0.100,										// averaging time constant
		0.100,										// peak decay time constant
		txa[channel].meter,							// result vector
		txa[channel].pmtupdate,						// locks for meter access
		TXA_MIC_AV,									// index for average value
		TXA_MIC_PK,									// index for peak value
		-1,											// index for gain value
		0);											// pointer for gain computation

	txa[channel].amsq.p = create_amsq (
		0,											// run
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// input buffer
		txa[channel].midbuff,						// output buffer
		txa[channel].midbuff,						// trigger buffer
		ch[channel].dsp_rate,						// sample rate
		0.010,										// time constant for averaging signal
		0.004,										// up-slew time
		0.004,										// down-slew time
		0.180,										// signal level to initiate tail
		0.200,										// signal level to initiate unmute
		0.000,										// minimum tail length
		0.025,										// maximum tail length
		0.200);										// muted gain
	
	{
	double default_F[11] = {0.0,  32.0,  63.0, 125.0, 250.0, 500.0, 1000.0, 2000.0, 4000.0, 8000.0, 16000.0};
	double default_G[11] = {0.0, -12.0, -12.0, -12.0,  -1.0,  +1.0,   +4.0,   +9.0,  +12.0,  -10.0,   -10.0};
	//double default_G[11] =   {0.0,   0.0,   0.0,   0.0,   0.0,   0.0,    0.0,    0.0,    0.0,    0.0,     0.0};
	txa[channel].eq.p = create_eq (
		0,											// run - OFF by default
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// pointer to input buffer
		txa[channel].midbuff,						// pointer to output buffer
		10,											// nfreqs
		default_F,									// vector of frequencies
		default_G,									// vector of gain values
		0,											// cutoff mode
		1,											// impulse calculation method
		ch[channel].dsp_rate);						// samplerate
	}

	txa[channel].eqmeter.p = create_meter (	
		1,											// run
		&(txa[channel].eq.p->run),					// pointer to eq 'run'
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// pointer to buffer
		ch[channel].dsp_rate,						// samplerate
		0.100,										// averaging time constant
		0.100,										// peak decay time constant
		txa[channel].meter,							// result vector
		txa[channel].pmtupdate,						// locks for meter access
		TXA_EQ_AV,									// index for average value
		TXA_EQ_PK,									// index for peak value
		-1,											// index for gain value
		0);											// pointer for gain computation

	txa[channel].preemph.p = create_emph (
		1,											// run
		1,											// position
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// input buffer
		txa[channel].midbuff,						// output buffer,
		ch[channel].dsp_rate,						// sample rate
		0,											// pre-emphasis type
		300.0,										// f_low
		3000.0);									// f_high

	txa[channel].leveler.p = create_wcpagc (
		0,											// run - OFF by default
		5,											// mode
		0,											// 0 for max(I,Q), 1 for envelope
		txa[channel].midbuff,						// input buff pointer
		txa[channel].midbuff,						// output buff pointer
		ch[channel].dsp_size,						// io_buffsize
		ch[channel].dsp_rate,						// sample rate
		0.001,										// tau_attack
		0.500,										// tau_decay
		6,											// n_tau
		1.778,										// max_gain
		1.0,										// var_gain
		1.0,										// fixed_gain
		1.0,										// max_input
		1.05,										// out_targ
		0.250,										// tau_fast_backaverage
		0.005,										// tau_fast_decay
		5.0,										// pop_ratio
		0,											// hang_enable
		0.500,										// tau_hang_backmult
		0.500,										// hangtime
		2.000,										// hang_thresh
		0.100);										// tau_hang_decay

	txa[channel].lvlrmeter.p = create_meter (
		1,											// run
		&(txa[channel].leveler.p->run),				// pointer to leveler 'run'
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// pointer to buffer
		ch[channel].dsp_rate,						// samplerate
		0.100,										// averaging time constant
		0.100,										// peak decay time constant
		txa[channel].meter,							// result vector
		txa[channel].pmtupdate,						// locks for meter access
		TXA_LVLR_AV,								// index for average value
		TXA_LVLR_PK,								// index for peak value
		TXA_LVLR_GAIN,								// index for gain value
		&txa[channel].leveler.p->gain);				// pointer for gain computation

	txa[channel].bp0.p = create_bandpass (
		1,											// always runs
		0,											// position
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// pointer to input buffer
		txa[channel].midbuff,						// pointer to output buffer 
		32.0,										// low freq cutoff
		23000.0,									// high freq cutoff
		ch[channel].dsp_rate,						// samplerate
		1,											// wintype
		2.0);										// gain

	txa[channel].pfgain0.p = create_gain (
		0,											// run - depends upon mode
		0,											// 
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// pointer to input buffer
		txa[channel].midbuff,						// pointer to output buffer
		0.5,										// Igain
		0.0);										// Qgain

	txa[channel].compressor.p = create_compressor (
		0,											// run - OFF by default
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// pointer to input buffer
		txa[channel].midbuff,						// pointer to output buffer
		3.0);										// gain

	txa[channel].bp1.p = create_bandpass (
		0,											// ONLY RUNS WHEN COMPRESSOR IS USED
		0,											// position
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// pointer to input buffer
		txa[channel].midbuff,						// pointer to output buffer 
		32.0,										// low freq cutoff
		23000.0,									// high freq cutoff
		ch[channel].dsp_rate,						// samplerate
		1,											// wintype
		2.0);										// gain	

	txa[channel].pfgain1.p = create_gain (
		0,											// run - depends upon mode
		&(txa[channel].compressor.p->run),			// run only if compressor is on
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// pointer to input buffer
		txa[channel].midbuff,						// pointer to output buffer
		0.5,										// Igain
		0.0);										// Qgain


	txa[channel].osctrl.p = create_osctrl (
		0,											// run
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// input buffer
		txa[channel].midbuff,						// output buffer
		ch[channel].dsp_rate,						// sample rate
		1.95);										// gain for clippings

	txa[channel].bp2.p = create_bandpass (
		0,											// ONLY RUNS WHEN COMPRESSOR IS USED
		0,											// position
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// pointer to input buffer
		txa[channel].midbuff,						// pointer to output buffer 
		32.0,										// low freq cutoff
		23000.0,									// high freq cutoff
		ch[channel].dsp_rate,						// samplerate
		1,											// wintype
		1.0);										// gain

	txa[channel].compmeter.p = create_meter (
		1,											// run
		&(txa[channel].compressor.p->run),			// pointer to compressor 'run'
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// pointer to buffer
		ch[channel].dsp_rate,						// samplerate
		0.100,										// averaging time constant
		0.100,										// peak decay time constant
		txa[channel].meter,							// result vector
		txa[channel].pmtupdate,						// locks for meter access
		TXA_COMP_AV,								// index for average value
		TXA_COMP_PK,								// index for peak value
		-1,											// index for gain value
		0);											// pointer for gain computation

	txa[channel].alc.p = create_wcpagc (
		1,											// run - always ON
		5,											// mode
		1,											// 0 for max(I,Q), 1 for envelope
		txa[channel].midbuff,						// input buff pointer
		txa[channel].midbuff,						// output buff pointer
		ch[channel].dsp_size,						// io_buffsize
		ch[channel].dsp_rate,						// sample rate
		0.001,										// tau_attack
		0.010,										// tau_decay
		6,											// n_tau
		1.0,										// max_gain
		1.0,										// var_gain
		1.0,										// fixed_gain
		1.0,										// max_input
		1.0,										// out_targ
		0.250,										// tau_fast_backaverage
		0.005,										// tau_fast_decay
		5.0,										// pop_ratio
		0,											// hang_enable
		0.500,										// tau_hang_backmult
		0.500,										// hangtime
		2.000,										// hang_thresh
		0.100);										// tau_hang_decay

	txa[channel].ammod.p = create_ammod (
		0,											// run - OFF by default
		0,											// mode:  0=>AM, 1=>DSB
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// pointer to input buffer
		txa[channel].midbuff,						// pointer to output buffer
		0.5);										// carrier level


	txa[channel].fmmod.p = create_fmmod (
		0,											// run - OFF by default
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// pointer to input buffer
		txa[channel].midbuff,						// pointer to input buffer
		ch[channel].dsp_rate,						// samplerate
		5000.0,										// deviation
		300.0,										// low cutoff frequency
		3000.0,										// high cutoff frequency
		1,											// ctcss run control
		0.10,										// ctcss level
		100.0,										// ctcss frequency
		1);											// run bandpass filter
	
	txa[channel].gen1.p = create_gen (
		0,											// run
		ch[channel].dsp_size,						// buffer size
		txa[channel].midbuff,						// input buffer
		txa[channel].midbuff,						// output buffer
		ch[channel].dsp_rate,						// sample rate
		0);											// mode

	txa[channel].uslew.p = create_uslew (
		channel,									// channel
		&ch[channel].iob.ch_upslew,					// pointer to channel upslew flag
		ch[channel].dsp_size,						// buffer size
		txa[channel].midbuff,						// input buffer
		txa[channel].midbuff,						// output buffer
		ch[channel].dsp_rate,						// sample rate
		0.000,										// delay time
		0.005);										// upslew time

		txa[channel].alcmeter.p = create_meter (
		1,											// run
		0,											// optional pointer to a 'run'
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// pointer to buffer
		ch[channel].dsp_rate,						// samplerate
		0.100,										// averaging time constant
		0.100,										// peak decay time constant
		txa[channel].meter,							// result vector
		txa[channel].pmtupdate,						// locks for meter access
		TXA_ALC_AV,									// index for average value
		TXA_ALC_PK,									// index for peak value
		TXA_ALC_GAIN,								// index for gain value
		&txa[channel].alc.p->gain);					// pointer for gain computation

	txa[channel].sip1.p = create_siphon (
		1,											// run
		ch[channel].dsp_size,						// input buffer size
		txa[channel].midbuff,						// input buffer
		4096,										// number of samples to buffer
		4096,										// fft size for spectrum
		1);											// specmode

	txa[channel].calcc.p = create_calcc (			
		channel,									// channel number
		1,											// run calibration
		1024,										// input buffer size
		ch[channel].in_rate,						// samplerate
		16,											// ints
		256,										// spi
		(1.0 / 0.4072),								// hw_scale
		0.1,										// mox delay
		0.0,										// loop delay
		0.8,										// ptol
		0,											// mox
		1);											// solidmox

	txa[channel].iqc.p0 = txa[channel].iqc.p1 = create_iqc (
		0,											// run
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// input buffer
		txa[channel].midbuff,						// output buffer
		(double)ch[channel].dsp_rate,				// sample rate
		16,											// ints
		0.005,										// changeover time
		256);										// spi

	txa[channel].cfir.p = create_cfir(
		0,											// run
		ch[channel].dsp_size,						// size
		txa[channel].midbuff,						// input buffer
		txa[channel].midbuff,						// output buffer
		ch[channel].dsp_rate,						// input sample rate
		ch[channel].out_rate,						// CIC input sample rate
		1,											// CIC differential delay
		640,										// CIC interpolation factor
		5,											// CIC integrator-comb pairs
		20000.0,									// cutoff frequency
		0,											// fourth-power rolloff
		0.0);										// raised-cosine transition width

	txa[channel].rsmpout.p = create_resample (
		0,											// run - will be turned ON below if needed
		ch[channel].dsp_size,						// input size
		txa[channel].midbuff,						// pointer to input buffer
		txa[channel].outbuff,						// pointer to output buffer
		ch[channel].dsp_rate,						// input sample rate
		ch[channel].out_rate,						// output sample rate
		0.0,										// select cutoff automatically
		0,											// select ncoef automatically
		0.980);										// gain

	txa[channel].outmeter.p = create_meter (
		1,											// run
		0,											// optional pointer to another 'run'
		ch[channel].dsp_outsize,					// size
		txa[channel].outbuff,						// pointer to buffer
		ch[channel].out_rate,						// samplerate
		0.100,										// averaging time constant
		0.100,										// peak decay time constant
		txa[channel].meter,							// result vector
		txa[channel].pmtupdate,						// locks for meter access
		TXA_OUT_AV,									// index for average value
		TXA_OUT_PK,									// index for peak value
		-1,											// index for gain value
		0);											// pointer for gain computation

	// turn OFF / ON resamplers as needed
	TXAResCheck (channel);
}

void destroy_txa (int channel)
{
	// in reverse order, free each item we created
	destroy_meter (txa[channel].outmeter.p);
	destroy_resample (txa[channel].rsmpout.p);
	destroy_cfir(txa[channel].cfir.p);
	destroy_iqc (txa[channel].iqc.p0);
	destroy_calcc (txa[channel].calcc.p);
	destroy_siphon (txa[channel].sip1.p);
	destroy_meter (txa[channel].alcmeter.p);
	destroy_uslew (txa[channel].uslew.p);
	destroy_gen (txa[channel].gen1.p);
	destroy_fmmod (txa[channel].fmmod.p);
	destroy_ammod (txa[channel].ammod.p);
	destroy_wcpagc (txa[channel].alc.p);
	destroy_meter (txa[channel].compmeter.p);
	destroy_bandpass (txa[channel].bp2.p);
	destroy_osctrl (txa[channel].osctrl.p);
	destroy_gain (txa[channel].pfgain1.p);
	destroy_bandpass (txa[channel].bp1.p);
	destroy_compressor (txa[channel].compressor.p);
	destroy_gain (txa[channel].pfgain0.p);
	destroy_bandpass (txa[channel].bp0.p);
	destroy_meter (txa[channel].lvlrmeter.p);
	destroy_wcpagc (txa[channel].leveler.p);
	destroy_emph (txa[channel].preemph.p);
	destroy_meter (txa[channel].eqmeter.p);
	destroy_eq (txa[channel].eq.p);
	destroy_amsq (txa[channel].amsq.p);
	destroy_meter (txa[channel].micmeter.p);
	destroy_panel (txa[channel].panel.p);
	destroy_gen (txa[channel].gen0.p);
	destroy_resample (txa[channel].rsmpin.p);
	_aligned_free (txa[channel].midbuff);
	_aligned_free (txa[channel].outbuff);
	_aligned_free (txa[channel].inbuff);
}

void flush_txa (int channel)
{
	memset (txa[channel].inbuff,  0, 1 * ch[channel].dsp_insize  * sizeof (complex));
	memset (txa[channel].outbuff, 0, 1 * ch[channel].dsp_outsize * sizeof (complex));
	memset (txa[channel].midbuff, 0, 2 * ch[channel].dsp_size    * sizeof (complex));
	flush_resample (txa[channel].rsmpin.p);
	flush_gen (txa[channel].gen0.p);
	flush_panel (txa[channel].panel.p);
	flush_meter (txa[channel].micmeter.p);
	flush_amsq (txa[channel].amsq.p);
	flush_eq (txa[channel].eq.p);
	flush_meter (txa[channel].eqmeter.p);
	flush_emph (txa[channel].preemph.p);
	flush_wcpagc (txa[channel].leveler.p);
	flush_meter (txa[channel].lvlrmeter.p);
	flush_bandpass (txa[channel].bp0.p);
	flush_gain (txa[channel].pfgain0.p);
	flush_compressor (txa[channel].compressor.p);
	flush_bandpass (txa[channel].bp1.p);
	flush_gain (txa[channel].pfgain1.p);
	flush_osctrl (txa[channel].osctrl.p);
	flush_bandpass (txa[channel].bp2.p);
	flush_meter (txa[channel].compmeter.p);
	flush_wcpagc (txa[channel].alc.p);
	flush_ammod (txa[channel].ammod.p);
	flush_fmmod (txa[channel].fmmod.p);
	flush_gen (txa[channel].gen1.p);
	flush_uslew (txa[channel].uslew.p);
	flush_meter (txa[channel].alcmeter.p);
	flush_siphon (txa[channel].sip1.p);
	flush_iqc (txa[channel].iqc.p0);
	flush_cfir(txa[channel].cfir.p);
	flush_resample (txa[channel].rsmpout.p);
	flush_meter (txa[channel].outmeter.p);
}

void xtxa (int channel)
{
	xresample (txa[channel].rsmpin.p);
	xgen (txa[channel].gen0.p);
	xpanel (txa[channel].panel.p);
	xmeter (txa[channel].micmeter.p);
	xamsqcap (txa[channel].amsq.p);
	xamsq (txa[channel].amsq.p);
	xeq (txa[channel].eq.p);
	xmeter (txa[channel].eqmeter.p);
	xemph (txa[channel].preemph.p, 0);
	xwcpagc (txa[channel].leveler.p);
	xmeter (txa[channel].lvlrmeter.p);
	xbandpass (txa[channel].bp0.p, 0);
	xgain (txa[channel].pfgain0.p);
	xcompressor (txa[channel].compressor.p);
	xbandpass (txa[channel].bp1.p, 0);
	xgain (txa[channel].pfgain1.p);
	xosctrl (txa[channel].osctrl.p);
	xbandpass (txa[channel].bp2.p, 0);
	xmeter (txa[channel].compmeter.p);
	xwcpagc (txa[channel].alc.p);
	xammod (txa[channel].ammod.p);
	xemph (txa[channel].preemph.p, 1);
	xfmmod (txa[channel].fmmod.p);
	xgen (txa[channel].gen1.p);
	xuslew (txa[channel].uslew.p);
	xmeter (txa[channel].alcmeter.p);
	xsiphon (txa[channel].sip1.p);
	xiqc (txa[channel].iqc.p0);
	xcfir(txa[channel].cfir.p);
	xresample (txa[channel].rsmpout.p);
	xmeter (txa[channel].outmeter.p);
	// print_peak_env ("env_exception.txt", ch[channel].dsp_outsize, txa[channel].outbuff, 0.7);
}

void setInputSamplerate_txa (int channel)
{
	// buffers
	_aligned_free (txa[channel].inbuff);
	txa[channel].inbuff = (double *)malloc0(1 * ch[channel].dsp_insize  * sizeof(complex));
	// input resampler
	setBuffers_resample (txa[channel].rsmpin.p, txa[channel].inbuff, txa[channel].midbuff);
	setSize_resample (txa[channel].rsmpin.p, ch[channel].dsp_insize);
	setInRate_resample (txa[channel].rsmpin.p, ch[channel].in_rate);
	TXAResCheck (channel);
}

void setOutputSamplerate_txa (int channel)
{
	// buffers
	_aligned_free (txa[channel].outbuff);
	txa[channel].outbuff = (double *)malloc0(1 * ch[channel].dsp_outsize * sizeof(complex));
	// cfir - needs to know input rate of firmware CIC
	setOutRate_cfir (txa[channel].cfir.p, ch[channel].out_rate);
	// output resampler
	setBuffers_resample (txa[channel].rsmpout.p, txa[channel].midbuff, txa[channel].outbuff);
	setOutRate_resample (txa[channel].rsmpout.p, ch[channel].out_rate);
	TXAResCheck (channel);
	// output meter
	setBuffers_meter (txa[channel].outmeter.p, txa[channel].outbuff);
	setSize_meter (txa[channel].outmeter.p, ch[channel].dsp_outsize);
	setSamplerate_meter (txa[channel].outmeter.p, ch[channel].out_rate);
}

void setDSPSamplerate_txa (int channel)
{
	// buffers
	_aligned_free (txa[channel].inbuff);
	txa[channel].inbuff = (double *)malloc0(1 * ch[channel].dsp_insize  * sizeof(complex));
	_aligned_free (txa[channel].outbuff);
	txa[channel].outbuff = (double *)malloc0(1 * ch[channel].dsp_outsize * sizeof(complex));
	// input resampler
	setBuffers_resample (txa[channel].rsmpin.p, txa[channel].inbuff, txa[channel].midbuff);
	setSize_resample (txa[channel].rsmpin.p, ch[channel].dsp_insize);
	setOutRate_resample (txa[channel].rsmpin.p, ch[channel].dsp_rate);
	// dsp_rate blocks
	setSamplerate_gen (txa[channel].gen0.p, ch[channel].dsp_rate);
	setSamplerate_panel (txa[channel].panel.p, ch[channel].dsp_rate);
	setSamplerate_meter (txa[channel].micmeter.p, ch[channel].dsp_rate);
	setSamplerate_amsq (txa[channel].amsq.p, ch[channel].dsp_rate);
	setSamplerate_eq (txa[channel].eq.p, ch[channel].dsp_rate);
	setSamplerate_meter (txa[channel].eqmeter.p, ch[channel].dsp_rate);
	setSamplerate_emph (txa[channel].preemph.p, ch[channel].dsp_rate);
	setSamplerate_wcpagc (txa[channel].leveler.p, ch[channel].dsp_rate);
	setSamplerate_meter (txa[channel].lvlrmeter.p, ch[channel].dsp_rate);
	setSamplerate_bandpass (txa[channel].bp0.p, ch[channel].dsp_rate);
	setSamplerate_gain (txa[channel].pfgain0.p, ch[channel].dsp_rate);
	setSamplerate_compressor (txa[channel].compressor.p, ch[channel].dsp_rate);
	setSamplerate_bandpass (txa[channel].bp1.p, ch[channel].dsp_rate);
	setSamplerate_gain (txa[channel].pfgain1.p, ch[channel].dsp_rate);
	setSamplerate_osctrl (txa[channel].osctrl.p, ch[channel].dsp_rate);
	setSamplerate_bandpass (txa[channel].bp2.p, ch[channel].dsp_rate);
	setSamplerate_meter (txa[channel].compmeter.p, ch[channel].dsp_rate);
	setSamplerate_wcpagc (txa[channel].alc.p, ch[channel].dsp_rate);
	setSamplerate_ammod (txa[channel].ammod.p, ch[channel].dsp_rate);
	setSamplerate_fmmod (txa[channel].fmmod.p, ch[channel].dsp_rate);
	setSamplerate_gen (txa[channel].gen1.p, ch[channel].dsp_rate);
	setSamplerate_uslew (txa[channel].uslew.p, ch[channel].dsp_rate);
	setSamplerate_meter (txa[channel].alcmeter.p, ch[channel].dsp_rate);
	setSamplerate_siphon (txa[channel].sip1.p, ch[channel].dsp_rate);
	setSamplerate_iqc (txa[channel].iqc.p0, ch[channel].dsp_rate);
	setSamplerate_cfir (txa[channel].cfir.p, ch[channel].dsp_rate);
	// output resampler
	setBuffers_resample (txa[channel].rsmpout.p, txa[channel].midbuff, txa[channel].outbuff);
	setInRate_resample (txa[channel].rsmpout.p, ch[channel].dsp_rate);
	TXAResCheck (channel);
	// output meter
	setBuffers_meter (txa[channel].outmeter.p, txa[channel].outbuff);
	setSize_meter (txa[channel].outmeter.p, ch[channel].dsp_outsize);
}

void setDSPBuffsize_txa (int channel)
{
	// buffers
	_aligned_free (txa[channel].inbuff);
	txa[channel].inbuff = (double *)malloc0(1 * ch[channel].dsp_insize  * sizeof(complex));
	_aligned_free (txa[channel].midbuff);
	txa[channel].midbuff = (double *)malloc0(2 * ch[channel].dsp_size * sizeof(complex));
	_aligned_free (txa[channel].outbuff);
	txa[channel].outbuff = (double *)malloc0(1 * ch[channel].dsp_outsize * sizeof(complex));
	// input resampler
	setBuffers_resample (txa[channel].rsmpin.p, txa[channel].inbuff, txa[channel].midbuff);
	setSize_resample (txa[channel].rsmpin.p, ch[channel].dsp_insize);
	// dsp_size blocks
	setBuffers_gen (txa[channel].gen0.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_gen (txa[channel].gen0.p, ch[channel].dsp_size);
	setBuffers_panel (txa[channel].panel.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_panel (txa[channel].panel.p, ch[channel].dsp_size);
	setBuffers_meter (txa[channel].micmeter.p, txa[channel].midbuff);
	setSize_meter (txa[channel].micmeter.p, ch[channel].dsp_size);
	setBuffers_amsq (txa[channel].amsq.p, txa[channel].midbuff, txa[channel].midbuff, txa[channel].midbuff);
	setSize_amsq (txa[channel].amsq.p, ch[channel].dsp_size);
	setBuffers_eq (txa[channel].eq.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_eq (txa[channel].eq.p, ch[channel].dsp_size);
	setBuffers_meter (txa[channel].eqmeter.p, txa[channel].midbuff);
	setSize_meter (txa[channel].eqmeter.p, ch[channel].dsp_size);
	setBuffers_emph (txa[channel].preemph.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_emph (txa[channel].preemph.p, ch[channel].dsp_size);
	setBuffers_wcpagc (txa[channel].leveler.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_wcpagc (txa[channel].leveler.p, ch[channel].dsp_size);
	setBuffers_meter (txa[channel].lvlrmeter.p, txa[channel].midbuff);
	setSize_meter (txa[channel].lvlrmeter.p, ch[channel].dsp_size);
	setBuffers_bandpass (txa[channel].bp0.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_bandpass (txa[channel].bp0.p, ch[channel].dsp_size);
	setBuffers_gain (txa[channel].pfgain0.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_gain (txa[channel].pfgain0.p, ch[channel].dsp_size);
	setBuffers_compressor (txa[channel].compressor.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_compressor (txa[channel].compressor.p, ch[channel].dsp_size);
	setBuffers_bandpass (txa[channel].bp1.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_bandpass (txa[channel].bp1.p, ch[channel].dsp_size);
	setBuffers_gain (txa[channel].pfgain1.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_gain (txa[channel].pfgain1.p, ch[channel].dsp_size);
	setBuffers_osctrl (txa[channel].osctrl.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_osctrl (txa[channel].osctrl.p, ch[channel].dsp_size);
	setBuffers_bandpass (txa[channel].bp2.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_bandpass (txa[channel].bp2.p, ch[channel].dsp_size);
	setBuffers_meter (txa[channel].compmeter.p, txa[channel].midbuff);
	setSize_meter (txa[channel].compmeter.p, ch[channel].dsp_size);
	setBuffers_wcpagc (txa[channel].alc.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_wcpagc (txa[channel].alc.p, ch[channel].dsp_size);
	setBuffers_ammod (txa[channel].ammod.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_ammod (txa[channel].ammod.p, ch[channel].dsp_size);
	setBuffers_fmmod (txa[channel].fmmod.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_fmmod (txa[channel].fmmod.p, ch[channel].dsp_size);
	setBuffers_gen (txa[channel].gen1.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_gen (txa[channel].gen1.p, ch[channel].dsp_size);
	setBuffers_uslew (txa[channel].uslew.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_uslew (txa[channel].uslew.p, ch[channel].dsp_size);
	setBuffers_meter (txa[channel].alcmeter.p, txa[channel].midbuff);
	setSize_meter (txa[channel].alcmeter.p, ch[channel].dsp_size);
	setBuffers_siphon (txa[channel].sip1.p, txa[channel].midbuff);
	setSize_siphon (txa[channel].sip1.p, ch[channel].dsp_size);
	setBuffers_iqc (txa[channel].iqc.p0, txa[channel].midbuff, txa[channel].midbuff);
	setSize_iqc (txa[channel].iqc.p0, ch[channel].dsp_size);
	setBuffers_cfir (txa[channel].cfir.p, txa[channel].midbuff, txa[channel].midbuff);
	setSize_cfir (txa[channel].cfir.p, ch[channel].dsp_size);
	// output resampler
	setBuffers_resample (txa[channel].rsmpout.p, txa[channel].midbuff, txa[channel].outbuff);
	setSize_resample (txa[channel].rsmpout.p, ch[channel].dsp_size);
	// output meter
	setBuffers_meter (txa[channel].outmeter.p, txa[channel].outbuff);
	setSize_meter (txa[channel].outmeter.p, ch[channel].dsp_outsize);
}

/********************************************************************************************************
*																										*
*											TXA Properties												*
*																										*
********************************************************************************************************/

PORT
void SetTXAMode (int channel, int mode)
{
	EnterCriticalSection (&ch[channel].csDSP);
	txa[channel].mode = mode;
	txa[channel].ammod.p->run   = 0;
	txa[channel].fmmod.p->run   = 0;
	txa[channel].pfgain0.p->run = 0;
	txa[channel].pfgain1.p->run = 0;
	txa[channel].preemph.p->run = 0;
	switch (mode)
	{
	case TXA_AM:
	case TXA_SAM:
		txa[channel].ammod.p->run   = 1;
		txa[channel].ammod.p->mode  = 0;
		txa[channel].pfgain0.p->run = 1;
		txa[channel].pfgain1.p->run = 1;
		break;
	case TXA_DSB:
		txa[channel].ammod.p->run   = 1;
		txa[channel].ammod.p->mode  = 1;
		txa[channel].pfgain0.p->run = 1;
		txa[channel].pfgain1.p->run = 1;
		break;
	case TXA_FM:
		txa[channel].fmmod.p->run   = 1;
		txa[channel].pfgain0.p->run = 1;
		txa[channel].pfgain1.p->run = 1;
		txa[channel].preemph.p->run = 1;
		break;
	default:

		break;
	}
	LeaveCriticalSection (&ch[channel].csDSP);
}

/********************************************************************************************************
*																										*
*										TXA Internal Functions											*
*																										*
********************************************************************************************************/

void TXAResCheck (int channel)
{
	RESAMPLE a = txa[channel].rsmpin.p;
	if (ch[channel].in_rate  != ch[channel].dsp_rate)	a->run = 1;
	else												a->run = 0;
	a = txa[channel].rsmpout.p;
	if (ch[channel].dsp_rate != ch[channel].out_rate)	a->run = 1;
	else												a->run = 0;
}

int TXAUslewCheck (int channel)
{
	return	(txa[channel].ammod.p->run == 1) ||
			(txa[channel].fmmod.p->run == 1) ||
			(txa[channel].gen0.p->run  == 1) ||
			(txa[channel].gen1.p->run  == 1);
}