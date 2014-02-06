/*
 * JanusAudio.dll - Support library for HPSDR.org's Janus/Ozy Audio card
 * Copyright (C) 2006,2007  Bill Tracey (bill@ejwt.com) (KD5TFD)
 * Copyright (C) 2010-2013 Doug Wigley (W5WC)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 *
 */

// callback-thread.c -- this module contains the callback thread.  It takes data from the fifo fed from the io thread and calls the powersdr callback 
// 

#include <stdio.h> 
#include "KD5TFD-VK6APH-Audio.h"
#include "private.h" 

int cb_keep_running; 

// this is the main loop of the callback thread -- basically suck stuff out of the fifo from the io 
// thread and feed it to the power sdr callback 
// 

int sctr = 0;
void Callback_ProcessBuffer(int *bufp, int buflen) { 
	int rc; 
	int j;
	int k;
	int index;

	int i; 
	float *callback_in[12]; 
	float *callback_out[8]; 
	int out_sample_incr;  
	int outidx; 
	int nc = 2 * nreceivers + 1;
		
	if ( buflen != nc * sizeof(int) * BlockSize ) { 
		fprintf(stderr, "Warning: short block in Callback_ProcessBuffer - frame dropped\n"); 
		return; 
	}

	// ok if we get here we've got a good buffer of samples, need to de interleave them 
	// and convert to floats 
	//
	// deinterleave and convert 
	// 
	for ( i = 0; i < BlockSize; i++ ) { 
		for (j = 0; j < nc - 1; j++)
		{
			(INpointer[j])[i] = ((float)(bufp[nc*i + j]))/IQConversionDivisor;
		}
		(INpointer[nc - 1])[i] = ((float)(bufp[(nc*i)+ nc - 1]))/(float)32767.0;
	}

	// DIVERSITY SECTION
	if (diversitymode2) {
	 switch (MercSource)
	{
	case 2:
		for (i = 0; i < BlockSize; i++)
			{
				(INpointer[0])[i] = (INpointer[2])[i];
				(INpointer[1])[i] = (INpointer[3])[i];
			}
		break;
	case 3:
		if (refMerc == 1)
		{
			for (i = 0; i < BlockSize; i++)
                {
                    (INpointer[0])[i] += (I_RotateA * (INpointer[2])[i] - Q_RotateA * (INpointer[3])[i]);
                    (INpointer[1])[i] += (I_RotateA * (INpointer[3])[i] + Q_RotateA * (INpointer[2])[i]);
                }
		}
		if (refMerc == 2)
		{
			for (i = 0; i < BlockSize; i++)
                {
                    float rx1I = (INpointer[0])[i];
                    float rx1Q = (INpointer[1])[i];
                    (INpointer[0])[i] = (INpointer[2])[i] + (I_RotateA * rx1I - Q_RotateA * rx1Q);
                    (INpointer[1])[i] = (INpointer[3])[i] + (I_RotateA * rx1Q + Q_RotateA * rx1I);
                }
		}
		break;
	default:
		break;
	} 
  } 
	// END DIVERSITY

	if ( MicResamplerP != NULL && MicResampleBufp != NULL ) {  // we need to resample mic data 
		int out_sample_count; 
		int sample_incr; 
		int j; 
		int outidx; 
		switch ( SampleRate ) { 
			case 96000: 
				sample_incr = 2; 
				break; 
			case 192000:
				sample_incr = 4; 
				break; 
			case 384000:
				sample_incr = 8; 
				break; 
			default: 
				fprintf(stderr, "Warning: callback_thread.c: unsupported sampled rate: %d\n", SampleRate); 
				break; 
		} 
		// copy every sample_incr sample over to sample rate input buf and resample 
		for ( j = 0, outidx = 0; j < BlockSize; j += sample_incr, outidx++ ) { 
			MicResampleBufp[outidx] = (INpointer[2 * nreceivers])[j]; 
		} 
		xresampleFV(MicResampleBufp, INpointer[2 * nreceivers], BlockSize/sample_incr, &out_sample_count, MicResamplerP); 		
	}
	memcpy((INpointer[2 * nreceivers + 1]), INpointer[2 * nreceivers], sizeof(float) * BlockSize); // copy left to right so it does not matter which PowerSDR is actually looking at 
 
	// printf("cb: buf build\n"); fflush(stdout); 
	// build bufs to pass to callback -- wants an array of pointers to buffers of floats for in and out 

		for (i = 0; i < 4; i++)
			callback_in[i] = INpointer[i];
		for (i = 4; i < 6; i++)
			callback_in[i] = INpointer[2 * nreceivers + i - 4];
		for (i = 6; i < 2 * nreceivers + 2; i++)
			callback_in[i] = INpointer[i - 2];

		for (i = 0; i < 8; i++)
			callback_out[i] = OUTpointer[i];

	//callback_out[0] = CallbackOutLbufp; // TX I
	//callback_out[1] = CallbackOutRbufp; // TX Q
	//callback_out[2] = CallbackMonOutLbufp; //RX & Mon audio
	//callback_out[3] = CallbackMonOutRbufp; //RX & Mon audio

	//callback_out[4] = CallbackOutL2bufp;  //RX2 Audio out from PSDR
	//callback_out[5] = CallbackOutR2bufp;  //RX2 Audio out from PSDR
	//callback_out[6] = CallbackOutL3bufp;
	//callback_out[7] = CallbackOutR3bufp;

// call the callback  		
	(*Callback)(callback_in, callback_out, BlockSize, NULL, 0, NULL); 

//#define LIMIT_SAMPLE(x)(x) = (float)(( (x) > 1.0 ? 1.0 : ( (x) < -1.0 ? -1.0 : (x) ) ))
	// ok now take the output buffer 
	// convert to 48khz sampling by skipping samples as needed 
	// limt it to +/- 1.0, convert to ints 
	// interleave it and put it on the fifo to the IO thread 
	// 
	switch ( SampleRate ) {
	case 48000: 
		out_sample_incr = 1;	
		break; 

	case 96000: 
		out_sample_incr = 2; 
		break; 

	case 192000: 
		out_sample_incr = 4; 	
		break;

	case 384000:
		out_sample_incr = 8; 
		break; 

	default:
		fprintf(stderr, "Mayday Mayday - bad sample rate in callback-thread.c"); 
	} 

	for ( i = 0, outidx = 0 ; i < BlockSize; i += out_sample_incr, outidx++ ) { 
		for (j = 0; j < 4; j++)

	/*	LIMIT_SAMPLE(CallbackMonOutLbufp[i]);
		LIMIT_SAMPLE(CallbackMonOutLbufp[i]);
		LIMIT_SAMPLE(CallbackMonOutRbufp[i]); 
		LIMIT_SAMPLE(CallbackMonOutRbufp[i]); 

		LIMIT_SAMPLE(CallbackOutLbufp[i]);
		LIMIT_SAMPLE(CallbackOutLbufp[i]); 
		LIMIT_SAMPLE(CallbackOutRbufp[i]);
		LIMIT_SAMPLE(CallbackOutRbufp[i]); */

		//for ( k = 0; k < 2; k++)
		//LIMIT_SAMPLE((OUTpointer[j])[i]);

		CBSampleOutBufp[(4*outidx)+j] = (short)((OUTpointer[j])[outidx] * 32767.0);

	//	CBSampleOutBufp[4*outidx] = (short)(CallbackMonOutLbufp[i] * 32767.0); 
	//	CBSampleOutBufp[(4*outidx)+1] = (short)(CallbackMonOutRbufp[i] * 32767.0); 

	//	CBSampleOutBufp[(4*outidx)+2] = (short)(CallbackOutLbufp[i] * 32767.0); 
	//	CBSampleOutBufp[(4*outidx)+3] = (short)(CallbackOutRbufp[i] * 32767.0); 
	}

	// ok buf is built - put it in the outbound fifo 
	rc = putFIFO(OutSampleFIFOp, CBSampleOutBufp, 4 * sizeof(short) * (BlockSize/out_sample_incr) ); 
	// printf("cb: putFifo rc=%d\n", rc); fflush(stdout); 

	if ( rc != 0 ) { 
		printf("warning: putFIFO on outbound failed rc=%d\n", rc); 
	} 

	return;		
} 

