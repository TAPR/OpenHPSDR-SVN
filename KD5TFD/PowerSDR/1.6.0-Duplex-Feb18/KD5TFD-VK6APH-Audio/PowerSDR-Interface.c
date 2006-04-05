// Copyright (C) Bill Tracey (KD5TFD) 2006 
// email: bill@ewjt.com 
// PowerSDR-Interface.c - this file contains routines called from PowerSDR to 
// access the FPGA based USB connected sound device 
// 

#define KD5TFDVK6APHAUDIO_EXPORTS 
#include <stdio.h> 
#include "KD5TFD-VK6APH-Audio.h"
#include "private.h" 

// 
// StartAudio -- returns 0 on succes, !0 on failure 
// samples_per_block is how many samples powersdr desires in a block -- the total number of samples 
// returned in a block will be 2x this (2 channels) 
// 
// error returns: 
//	2 - could not open Xylo 
//  3 - bad state - already started 
//  4 - failed creating IOThread
//  5 - failed to alloc mem for callback input buffer 
//  6 - failed to alloc mem for input buffers 
//  7 - failed creating fifo for io -> callback 
//  8 - failed creating callback -> io fifo 
//  9 - failed creating callback thread 
// 10 - failed alloc of callback return buffer 

KD5TFDVK6APHAUDIO_API int StartAudio(int samples_per_block, int (__stdcall *callbackp)(void *inp, void *outp, int framcount, void *timeinfop, int flags, void *userdata)) { 
	int rc; 
	int myrc = 0; 
	int *in_sample_bufp = NULL; 
	float *bufp = NULL; 

	BlockSize = samples_per_block; 
	Callback = callbackp;


	// make sure we're not already opened 
	// 
	if ( XyloH != NULL || IOThreadRunning ) { 
		return 3; 
	} 

	do { // once 
		// allocate buffers for callback buffers 
		bufp = (float *)malloc(sizeof(float) * BlockSize * 4);  // 2 channels for in and 2 channels for out 
		if ( bufp == NULL ) { 
			myrc = 5; 
			break; 
		} 
		CallbackInLbufp = bufp; 
		CallbackInRbufp = bufp + BlockSize; 
		CallbackOutLbufp = bufp + (2*BlockSize); 
		CallbackOutRbufp = bufp + (3*BlockSize); 

		// allocate buffers for inbound and outbound samples from USB
		in_sample_bufp = (int *)malloc(sizeof(int) * BlockSize * 2);  // 2 channels in and out 
		if ( in_sample_bufp == NULL ) { 
			myrc = 6; 
			break; 
		} 
		IOSampleInBufp = in_sample_bufp; 

		CBSampleOutBufp = (short *)malloc(sizeof(short) * BlockSize *2); 
		if ( CBSampleOutBufp == NULL ) { 
			myrc = 10; 
			break; 
		} 
		
		// go open the xylo usb device 
		XyloH = XyloOpen(); 
		if ( XyloH == NULL ) { 
			myrc =  2; 
			break; 
		} 

		// create FIFO for inbound samples 
		InSampleFIFOp = createFIFO();
		if ( InSampleFIFOp == NULL ) { 
			myrc = 7; 
			break; 
		} 

		OutSampleFIFOp = createFIFO(); 
		if ( OutSampleFIFOp == NULL ) { 
			myrc = 8; 
			break; 
		} 
		
#if 0 
		// ok, lets get the threads going, callback first 
		// 
		sem_init(&CallbackThreadInitSem, 0, 0); 
		rc = pthread_create(&CallbackThreadID, NULL, CallbackThreadMain, NULL); 
		if ( rc != 0 ) { 
			fprintf(stderr, "pthread_created failed on CallbackThread w/ rc=%d\n", rc); 
			myrc =  9; 
			break; 
		} 
#endif 

		// ok the xylo is open - start the io_thread 
		// 
		sem_init(&IOThreadInitSem, 0, 0); 
		rc = pthread_create(&IOThreadID, NULL,  IOThreadMain, NULL); 
		if ( rc != 0 ) {  // failed 
			fprintf(stderr, "pthread_created failed on IOThread w/ rc=%d\n", rc); 
			myrc =  4; 
			break; 
		} 
		sem_wait(&IOThreadInitSem);  // wait for the thread to get going 		 
	} while ( 0 ); 

	if ( myrc != 0 ) {  // we failed -- clean up 
		if ( bufp != NULL ) { 
			CallbackInLbufp = NULL;
			CallbackInRbufp = NULL;
			CallbackOutLbufp = NULL;
			CallbackOutRbufp = NULL;
			free(bufp); 
		} 
		if ( in_sample_bufp != NULL ) { 
			IOSampleInBufp = NULL; 
			CBSampleOutBufp = NULL; 
			free(in_sample_bufp); 
		} 
		if ( CBSampleOutBufp != NULL ) { 
			short *tmp = CBSampleOutBufp; 
			CBSampleOutBufp = NULL; 
			free(tmp); 
		}
		if ( InSampleFIFOp != NULL ) { 
			destroyFIFO(InSampleFIFOp); 
			InSampleFIFOp = NULL; 
		} 
		if ( OutSampleFIFOp != NULL ) { 
			destroyFIFO(OutSampleFIFOp); 
			OutSampleFIFOp = NULL; 
		} 
		if ( XyloH != NULL ) { 
			XyloClose(XyloH); 
			XyloH = NULL; 
		} 		
	} 
	return myrc; 
}


KD5TFDVK6APHAUDIO_API void StopAudio() { 
	int rc; 
	printf("stop audio called\n");  fflush(stdout); 
	rc = IOThreadStop(); 
	if ( rc != 0 ) { 
		fprintf(stderr, "Warning: IOThreadStop failed with rc=%d\n", rc); 
	} 
	printf("iothread stopped\n");   fflush(stdout); 
	if ( InSampleFIFOp != NULL ) { 
		destroyFIFO(InSampleFIFOp); 
		InSampleFIFOp = NULL; 
	}
	printf("fifo destroyted\n");   fflush(stdout); 


	if ( IOSampleInBufp != NULL ) { 
		free(IOSampleInBufp); 
		IOSampleInBufp = NULL; 
	} 

	printf("sample buf freed\n");   fflush(stdout); 		
	if ( CallbackInLbufp != NULL ) { 		
			CallbackInRbufp = NULL;
			CallbackOutLbufp = NULL;
			CallbackOutRbufp = NULL;
			free(CallbackInLbufp); 
			CallbackInLbufp = NULL;
	} 
	printf("call back buf freed\n");   fflush(stdout); 
	if ( XyloH != NULL ) { 
		XyloClose(XyloH); 
		XyloH = NULL; 
	} 
	printf("xylo closed\n");   fflush(stdout); 
	return;
}
