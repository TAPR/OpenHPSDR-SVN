/*
 * JanusAudio.dll - Support library for HPSDR.org's Janus/Ozy Audio card
 * Copyright (C) 2006,2007  Bill Tracey (bill@ejwt.com) (KD5TFD)
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
// #define PERF_DEBUG 1 
#include "private.h" 




// #define TEST_TONE (1) 
#ifdef TEST_TONE 
int TestToneIdx = 0; 
#define NUM_TEST_TONE_VALS (32) 
float TestToneVals[NUM_TEST_TONE_VALS] = { 
	0.000000,
	0.195090,
	0.382684,
	0.555571,
	0.707107,
	0.831470,
	0.923880,
	0.980785,
	1.000000,
	0.980785,
	0.923879,
	0.831469,
	0.707106,
	0.555569,
	0.382682,
	0.195088,
	-0.000002,
	-0.195093,
	-0.382686,
	-0.555573,
	-0.707109,
	-0.831471,
	-0.923881,
	-0.980786,
	-1.000000,
	-0.980785,
	-0.923878,
	-0.831467,
	-0.707104,
	-0.555567,
	-0.382679,
	-0.195086
};
		 
#endif

int cb_keep_running; 

// #define USE_NEW_FLOAT_CONVERT (1)

#ifdef USE_NEW_FLOAT_CONVERT 


/* returns 1 based index of highest set bit in i.  0 is returned if i is 0
 */ 
int findHiBit(unsigned int i) {
    /* use a bit of assembler to find highest set bit -- in Visual Studio 2005 we can use _BitScanReverse, 
	   but for Visual Studio .NET 2003 we do in line assembler.  

	   For gcc 3.4 or greater use __builtin_clz (Count Leading Zeros) 
     */	
	__asm { 
		mov eax, i ;  1st arg
		bsr eax, eax   
		jz arg_is_zero 
		inc eax    ; we want a 1 based idx 
		jmp all_done 
	arg_is_zero: 
		xor eax, eax 
	all_done: 
	}
}

/* 
 * make a float from sign, exp and mantissa 
 */ 
float makeFloat(unsigned int s, unsigned int e, unsigned int m) {
    float result;
    unsigned int bits;

    bits = s << 31;
    e = e & 0xff; /* make sure we only have 8 bits */
    e = e << 23;  /* move 'em over */
    bits |= e;    /* or 'em in */
    m = m & 0x7fffff;  /* let only 23 bits thru */
    bits |= m;
    result = *((float *)(&bits));
    return result;
}

 

#define MAKE_FLOAT(f, s, e, m)   *(unsigned int *)(&f) = ((s) << 31 ) | ( ((e) & 0xff) << 23)  | ((m) & 0x7fffff)


/* 
 * convert an signed int num bits wide to a float  --  with the 
 * interpratation that max pos int converts to  +1.0 and max neg int converts to -1.0 -- 
 * used for feeding data to dttsp since it is looking for data between 1.0 and -1.0 
 */ 
float genericIntToFloat(int num_bits, int i) { 
    int sign = 0;
    int exp;
    // int hi_bit;
    int mantissa;
    float result;

    // if ( i < 0 ) {
    //    sign = 1;
    //    i = -i;
    // }
    /* hi_bit = findHiBit(i); */ 
    // exp = (127 - num_bits) + hi_bit;
    // mantissa = i << (24-hi_bit);
	
	__asm { 	
		mov eax, i ;  1st arg
		cmp eax, 0
		jge arg_positive_or_zero  
		mov ebx, 1       ; set sign var 
		mov sign, ebx 
		not eax          ; i = -i 
	arg_positive_or_zero:
		or eax, eax 
		jz arg_is_zero  
		mov ebx, eax ; save i in ebx as well 
		mov ecx, 23
		bsr eax, eax   
		; inc eax    ; we want a 1 based idx  
		; eax is now hi_bit 		
		sub ecx, eax  ;; ecx = 24 - hi_bit 
		add eax, 128 
		sub eax, num_bits 
		; eax is now exp 
		mov exp, eax 
        shl ebx, cl 
		mov mantissa, ebx 
		jmp all_done 
	arg_is_zero: 
		xor eax, eax
		mov mantissa, eax 
	all_done: 

	}

	if ( mantissa == 0 ) { 
		result = 0.0f;
	} 
	else { 
		// result = makeFloat(sign, exp, mantissa);
		MAKE_FLOAT(result, sign, exp, mantissa); 
	}
    return result;
} 

/* 
 * convert 24 bit integer to a float 
 */ 
float int24ToFloat(int i) {
	return genericIntToFloat(24, i); 
}


/* convert 16 bit integer to a float */ 
float int16ToFloat(int i) { 
	return genericIntToFloat(16, i); 
} 

unsigned int getExp(float f) {
    unsigned int i;
    i = *((int *)(&f));
    i = i >> 23;
    i = i & 0xff;
    return i;
}


unsigned int getMantissa(float f) {
    unsigned int i;
    i = *((int *)(&f));
    i = i & 0x7fffff;
    return i;
}


unsigned int getSign(float f) {
    unsigned int i;
    i = *((int *)(&f));
    i = i >> 31;
    i = i & 1;
    return i;
}


int genericFloatToInt(int num_int_bits, float f) {
    int sign = 0;
    int exp;
    int mantissa;
    int hi_bit;
    int mask;

    int result;

    sign = getSign(f);
    exp = getExp(f);
    mantissa = getMantissa(f);
    hi_bit = exp - (127 - num_int_bits);
    if ( exp <= (127-num_int_bits) ) {
        result = 0;
    }
    else {
        result = mantissa >> (24-hi_bit);
        mask = 1 << (hi_bit-1);
        result |= mask;
    }
    if ( sign ) {
        result = -result;
    }
    return result;
}


int floatToInt24(float f) {
    return genericFloatToInt(24, f);
}


int floatToInt16(float f) {
    return genericFloatToInt(16, f);
}


#endif 




// extern void Dump(FILE *ofile,                /* file handle to dump to - assumed to be      */
//                                         /* open.                                       */
//    unsigned char *buf,            /* pointer to data to dump                     */
//    unsigned int count,            /* number of bytes to dump                      */
//    unsigned char *comment); 

// int cb_dump_count = 0; 

// this is the main loop of the callback thread -- basically suck stuff out of the fifo from the io 
// thread and feed it to the power sdr callback 
// 
#if 0 
void CallbackThreadMainLoop(void) { 
	int rc; 
	int *bufp; 
	int buflen; 
	int i; 
	float *callback_in[2]; 
	float *callback_out[2]; 
#ifdef PERF_DEBUG 
	__int64 start_t, stop_t, delta_t; 
#endif 
	

	while ( cb_keep_running ) { 
		bufp = getFIFO(InSampleFIFOp, &buflen, 1);   // wait for data to become available 
		if ( bufp == NULL ) {
			fprintf(stderr, "Warning: NULL read from FIFO!!\n"); 
			cb_keep_running = 0;  // bail out 
			continue; 
		} 
		if ( buflen != (2*sizeof(int)*BlockSize) ) {  // should never happen 
			fprintf(stderr, "Warning: Short read on FIFO - frame dropped.\n"); 
			continue; 
		} 
		// ok if we get here we've got a good buffer of samples, need to de interleave them 
		// and convert to floats 
		//
		// deinterleave and convert 
		// 
		// TODO:  is there a cool SIMD way to do this? 
		// 

#if 0
		if ( cb_dump_count < 4 ) { 
			++cb_dump_count; 
			Dump(stdout, (unsigned char *)bufp, buflen, "CB Buffer In"); 
			fflush(stdout); 
		}
#endif

#ifdef PERF_DEBUG
		start_t = getPerfTicks(); 
#endif 
		for ( i = 0; i < BlockSize; i++ ) { 
#ifdef USE_NEW_FLOAT_CONVERT 
			CallbackInLbufp[i] = convertToFloat(bufp[2*i]); 
			CallbackInRbufp[i] = convertToFloat(bufp[(2*i)+1]); 
#else 
			CallbackInLbufp[i] = ((float)(bufp[2*i]))/(float)8388607.0;  // (2**23) - 1			
			CallbackInRbufp[i] = ((float)(bufp[(2*i)+1]))/(float)8388607.0; 
#endif 
		} 	
#ifdef PERF_DEBUG 
		stop_t = getPerfTicks(); 
		delta_t = stop_t - start_t; 
		updateHLA(&InConvertHLA, delta_t); 
#endif 

// #define LOOPBACK 1 
#ifdef LOOPBACK 
		for ( i = 0; i < 2*BlockSize; i++ ) { 
			CBSampleOutBufp[i] = i; /* bufp[i]; */ 
		}
#endif 
		// all done with data from fifo -- free it 
		freeFIFOdata(bufp); 
		bufp = NULL; 

		// build bufs to pass to callback -- wants an array of pointers to buffers of floats for in and out 
		callback_in[0] = CallbackInLbufp; 
		callback_in[1] = CallbackInRbufp; 
		callback_out[0] = CallbackOutLbufp; 
		callback_out[1] = CallbackOutRbufp; 

		// call the callback  		
#ifdef PERF_DEBUG 
		start_t = getPerfTicks(); 
#endif

#ifndef LOOPBACK  
		(*Callback)(callback_in, callback_out, BlockSize, NULL, 0, NULL); 
#ifdef PERF_DEBUG 
		stop_t = getPerfTicks(); 
		delta_t = stop_t - start_t; 
		updateHLA(&CallbackHLA, delta_t); 
		start_t = getPerfTicks(); 
#endif 

#ifdef TEST_TONE
		for ( i = 0; i < BlockSize; i++ ) { 
			CallbackOutRbufp[i] = -TestToneVals[TestToneIdx]; 
			CallbackOutLbufp[i] = TestToneVals[TestToneIdx]; 
			++TestToneIdx; 
			if ( TestToneIdx >= NUM_TEST_TONE_VALS ) { 
				TestToneIdx = 0; 
			} 
		} 
#endif 

		// ok now take the output buffer, int it and interleave it and put it on the fifo to the IO thread 
		// 
		for ( i = 0; i < BlockSize; i++ ) { 
			// limit data to +/- 1.0 
			if ( CallbackOutRbufp[i] > 1.0 ) { 
				CallbackOutRbufp[i] = 1.0; 
			}
			else if ( CallbackOutRbufp[i] < -1.0 ) { 
				CallbackOutRbufp[i] = -1.0; 
			}
			if ( CallbackOutLbufp[i] > 1.0 ) { 
				CallbackOutLbufp[i] = 1.0; 
			}
			else if ( CallbackOutLbufp[i] < -1.0 ) { 
				CallbackOutLbufp[i] = -1.0; 
			}
			CBSampleOutBufp[2*i] = (short)(CallbackOutLbufp[i] * 32767.0); 
			CBSampleOutBufp[(2*i)+1] = (short)(CallbackOutRbufp[i] * 32767.0); 
		}
#ifdef PERF_DEBUG 
		stop_t = getPerfTicks(); 
		delta_t = stop_t - start_t; 
		updateHLA(&OutConvertHLA, delta_t); 
#endif 
#endif 

		// ok buf is built - put it in the outbound fifo 
		rc = putFIFO(OutSampleFIFOp, CBSampleOutBufp, 2 * sizeof(short) * BlockSize ); 
		// printf("cb: putFifo rc=%d\n", rc); fflush(stdout); 

		if ( rc != 0 ) { 
			printf("warning: putFIFO on outbound failed rc=%d\n", rc); 
		} 


#if 0 
		if ( memcmp(CallbackInLbufp, CallbackOutRbufp, BlockSize * sizeof(float)) != 0 ) { 
			printf("check failed - inl\n"); 
		} 
		if ( memcmp(CallbackInRbufp, CallbackOutLbufp, BlockSize * sizeof(float)) != 0 ) { 
			printf("check failed - inr\n"); 
		} 
#endif 
		
	} 
} 

#endif 


void Callback_ProcessBuffer(int *bufp, int buflen) { 
	int rc; 
	// int *bufp; 
	// int buflen; 
	int i; 
	float *callback_in[4]; 
	float *callback_out[4]; 
	int out_sample_incr;  
	int outidx; 
#ifdef USE_NEW_FLOAT_CONVERT 
	int *ip; 
	short *sp; 
#endif
	
#ifdef PERF_DEBUG 
	__int64 start_t, stop_t, delta_t; 
#endif 
	
	// printf("cb: bufp=0x%08x, len=%d\n", (unsigned long)bufp, buflen); fflush(stdout); 

	if ( buflen != 4*sizeof(int)*BlockSize ) { 
		fprintf(stderr, "Warning: short block in Callback_ProcessBuffer - frame dropped\n"); 
		return; 
	}


	
	// ok if we get here we've got a good buffer of samples, need to de interleave them 
	// and convert to floats 
	//
	// deinterleave and convert 
	// 
	// TODO:  is there a cool SIMD way to do this? 
	// 





#ifdef PERF_DEBUG
	start_t = getPerfTicks(); 
#endif 
	for ( i = 0    /* ,  ip = bufp */ ; i < BlockSize; i++ ) { 
#if 0  
		CallbackInLbufp[i] = genericIntToFloat(SampleBits, bufp[3*i] /* *ip */ ); // ++ip;
		CallbackInRbufp[i] = genericIntToFloat(SampleBits, bufp[(3*i)+1] /* *ip */); // ++ip;
#else 
		CallbackInLbufp[i] = ((float)(bufp[3*i]))/IQConversionDivisor;  
		CallbackInRbufp[i] = ((float)(bufp[(3*i)+1]))/IQConversionDivisor;
		
		// CallbackInLbufp[i] = ((float)(bufp[3*i]))/IQConversionDivisor;  
		// CallbackInRbufp[i] =  ((float)(bufp[(3*i)+1]))/IQConversionDivisor; 
#endif 

		// wjt -- todo!! -- this should be in the resampler loop below -- we're converting more samples to 
		// floats than we need to for 96000 and 192000 sample rates -- should downsample first, convert to float 
		// and then resample 
#if 0 
		CallbackMicLbufp[i] = genericIntToFloat(16, bufp[(3*i)+2] /* *ip */ );  // ++ip;		
#else 
		CallbackMicLbufp[i] = ((float)(bufp[(3*i)+2]))/(float)32767.0;  // (2**15) - 1  (mic samples are 16 bits) 
#endif 
		
	} 

#ifndef LINUX 
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
			default: 
				fprintf(stderr, "Warning: callback_thread.c: unsupported sampled rate: %d\n", SampleRate); 
				break; 
		} 
		// copy every sample_incr sample over to sample rate input buf and resample 
		for ( j = 0, outidx = 0; j < BlockSize; j += sample_incr, outidx++ ) { 
			MicResampleBufp[outidx] = CallbackMicLbufp[j]; 
		} 
		DoResamplerF(MicResampleBufp, CallbackMicLbufp, BlockSize/sample_incr, &out_sample_count, MicResamplerP); 		
	}
#else 
#warning message("info - LINUX code missing ... DoResamplerF") 
#endif 

	memcpy(CallbackMicRbufp, CallbackMicLbufp, sizeof(float) * BlockSize); // copy left to right so it does not matter which PowerSDR is actually looking at 
	
#ifdef PERF_DEBUG 
	stop_t = getPerfTicks(); 
	delta_t = stop_t - start_t; 
	updateHLA(&InConvertHLA, delta_t); 
#endif 

// #define LOOPBACK 1 
#ifdef LOOPBACK 
	for ( i = 0; i < 2*BlockSize; i++ ) { 
		CBSampleOutBufp[i] = i; /* bufp[i]; */ 
	}
#endif 

	// printf("cb: buf build\n"); fflush(stdout); 
	// build bufs to pass to callback -- wants an array of pointers to buffers of floats for in and out 
	callback_in[0] = CallbackInLbufp; 
	callback_in[1] = CallbackInRbufp; 
	callback_in[2] = CallbackMicLbufp; 
	callback_in[3] = CallbackMicRbufp; 

	callback_out[0] = CallbackOutLbufp; 
	callback_out[1] = CallbackOutRbufp; 
	callback_out[2] = CallbackMonOutLbufp; 
	callback_out[3] = CallbackMonOutRbufp; 

	// call the callback  		
#ifdef PERF_DEBUG 
	start_t = getPerfTicks(); 
#endif

#ifndef LOOPBACK  
	// printf("cb: calling cb\n"); fflush(stdout); 
	(*Callback)(callback_in, callback_out, BlockSize, NULL, 0, NULL); 
	// printf("cb: cb returned\n"); fflush(stdout); 
#ifdef PERF_DEBUG 
	stop_t = getPerfTicks(); 
	delta_t = stop_t - start_t; 
	updateHLA(&CallbackHLA, delta_t); 

	start_t = getPerfTicks(); 
#endif 

#ifdef TEST_TONE
	for ( i = 0; i < BlockSize; i++ ) { 
		CallbackOutRbufp[i] = -TestToneVals[TestToneIdx]; 
		CallbackOutLbufp[i] = TestToneVals[TestToneIdx]; 
		++TestToneIdx; 
		if ( TestToneIdx >= NUM_TEST_TONE_VALS ) { 
			TestToneIdx = 0; 
		} 
	} 
#endif 

#define LIMIT_SAMPLE(x)  (x) = (float)(( (x) > 1.0 ? 1.0 : ( (x) < -1.0 ? -1.0 : (x) ) ))
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

		default:
			fprintf(stderr, "Mayday Mayday - bad sample rate in callback-thread.c"); 
	} 

#ifdef USE_NEW_FLOAT_CONVERT 
	for ( i = 0, sp = CBSampleOutBufp ; i < BlockSize; i += out_sample_incr ) { 
#else 
	for ( i = 0, outidx = 0 ; i < BlockSize; i += out_sample_incr, outidx++ ) { 
#endif 
#if 1
		LIMIT_SAMPLE(CallbackOutRbufp[i]);
		LIMIT_SAMPLE(CallbackOutRbufp[i]); 
		LIMIT_SAMPLE(CallbackOutLbufp[i]);
		LIMIT_SAMPLE(CallbackOutLbufp[i]); 
		LIMIT_SAMPLE(CallbackMonOutLbufp[i]);
		LIMIT_SAMPLE(CallbackMonOutLbufp[i]);
		LIMIT_SAMPLE(CallbackMonOutRbufp[i]); 
		LIMIT_SAMPLE(CallbackMonOutRbufp[i]);  
#else 
		// limit data to +/- 1.0 
		if ( CallbackOutRbufp[i] > 1.0 ) { 
			CallbackOutRbufp[i] = 1.0; 
		}
		else if ( CallbackOutRbufp[i] < -1.0 ) { 
			CallbackOutRbufp[i] = -1.0; 
		}
		if ( CallbackOutLbufp[i] > 1.0 ) { 
			CallbackOutLbufp[i] = 1.0; 
		}
		else if ( CallbackOutLbufp[i] < -1.0 ) { 
			CallbackOutLbufp[i] = -1.0; 
		}
		if ( CallbackMonOutLbufp[i] > 1.0 ) { 
			CallbackMonOutLbufp[i] = 1.0; 
		}
		else if ( CallbackMonOutLbufp[i] < -1.0 ) { 
			CallbackMonOutLbufp[i] = -1.0; 
		}
		if ( CallbackMonOutRbufp[i] > 1.0 ) { 
			CallbackMonOutRbufp[i] = 1.0; 
		}
		else if ( CallbackMonOutRbufp[i] < -1.0 ) { 
			CallbackMonOutRbufp[i] = -1.0; 
		}
#endif 
#ifdef USE_NEW_FLOAT_CONVERT 
	    *sp = (short)floatToInt16(CallbackOutLbufp[i]);  ++sp; 		
		*sp = (short)floatToInt16(CallbackOutRbufp[i]);  ++sp; 
		*sp = (short)floatToInt16(CallbackMonOutLbufp[i]); ++sp; 		
		*sp = (short)floatToInt16(CallbackMonOutRbufp[i]); ++sp; 
#else 
		CBSampleOutBufp[4*outidx] = (short)(CallbackOutLbufp[i] * 32767.0); 
		CBSampleOutBufp[(4*outidx)+1] = (short)(CallbackOutRbufp[i] * 32767.0); 
#if 1 
		CBSampleOutBufp[(4*outidx)+2] = (short)(CallbackMonOutLbufp[i] * 32767.0); 
		CBSampleOutBufp[(4*outidx)+3] = (short)(CallbackMonOutRbufp[i] * 32767.0); 
#else
		// put same sigs on LR and IQ for diagnostic purposes 
		CBSampleOutBufp[(4*outidx)+2] =  CBSampleOutBufp[4*outidx];
		CBSampleOutBufp[(4*outidx)+3] =  CBSampleOutBufp[(4*outidx)+1];
#endif
#endif
	}
#ifdef PERF_DEBUG 
	stop_t = getPerfTicks(); 
	delta_t = stop_t - start_t; 
	updateHLA(&OutConvertHLA, delta_t); 
#endif 
#endif 

	// ok buf is built - put it in the outbound fifo 
	rc = putFIFO(OutSampleFIFOp, CBSampleOutBufp, 4 * sizeof(short) * (BlockSize/out_sample_incr) ); 
	// printf("cb: putFifo rc=%d\n", rc); fflush(stdout); 

	if ( rc != 0 ) { 
		printf("warning: putFIFO on outbound failed rc=%d\n", rc); 
	} 

	return;		
} 


#if 0
void *CallbackThreadMain(void *argp) {				
		CallbackThreadRunning = 1; 
		cb_keep_running = 1; 

		SetThreadPriority(GetCurrentThread(), THREAD_PRIORITY_HIGHEST); 
	    sem_post(&CallbackThreadInitSem); // tell launching thread we're rockin and rollin 				
		CallbackThreadMainLoop(); 		
		CallbackThreadRunning = 0;		
		return NULL; 
}
#endif
