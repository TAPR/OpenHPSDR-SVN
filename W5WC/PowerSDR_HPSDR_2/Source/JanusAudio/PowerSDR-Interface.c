/*A
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


// PowerSDR-Interface.c - this file contains routines called from PowerSDR to
// access the FPGA based USB connected sound device
//

//#define KD5TFDVK6APHAUDIO_EXPORTS
#include <stdio.h>
#include "KD5TFD-VK6APH-Audio.h"
#include "private.h"


//
// StartAudio -- called when we need to start reading audio and passing it to PowerSDR via the callback.
//
// This rountine allocates needed buffers, resamplers, and opens the device to read from.
// If it fails it cleans up and return non zero, returns 0 on success
// Things allocated/created/opened here need to be freed/destroyed/closed in StopAudio
//
// error returns:
//      2 - could not open Xylo
//  3 - bad state - already started
//  4 - failed creating IOThread
//  5 - failed to alloc mem for callback input buffer
//  6 - failed to alloc mem for input buffers
//  7 - failed creating fifo for io -> callback
//  8 - failed creating callback -> io fifo
//  9 - failed creating callback thread
// 10 - failed alloc of callback return buffer
// 13 - failed creating resampler
// 14 - failed alloc of low level USB buffers
//
// this is the 4 in 4 out (actually 3 in 4 out) version of start audio
//

KD5TFDVK6APHAUDIO_API int StartAudioNative(int sample_rate, int samples_per_block,
                      int (__stdcall *callbackp)(void *inp, void *outp, int framcount, void *timeinfop, int flags, void *userdata),
                                           int sample_bits, int no_send)
{
        int rc;
        int myrc = 0;
        int *in_sample_bufp = NULL;
        float *bufp = NULL;


        //
        // DttSP runs at a single sampling rate - the IQ in sampling rate.  The Janus hardware supports selection of (192,96,48)
        // khz on IQ in, 48 khz on mic in and IQ and LR output.
        //
        // what's our sampling rate?  The code is setup to read IQ and 192, 96 or 48 khz.  Samples on the mic input match
        // the IQ sample rate, but only the 1st of (4/2/1) sample is valid others are duplicated because the mic is always sampled
        // at 48 khz.  If the IQ rate does not equal 48 khz the mic data is resampled to match the IQ rate.
        //
        // Output (xmit IQ, and LR audio) is always at 48 khz - downsampled by dropping samples
        //
        HaveSync = 0;
        SampleRate = sample_rate;
        FPGAWriteBufSize = 512; // 512;
        FPGAReadBufp = NULL;
        FPGAWriteBufp = NULL;
        SampleBits = sample_bits;
        ForceNoSend = no_send;
        IQConversionDivisor = (float)8388607.0;  // (2**23)-1
        if ( SampleBits == 16 ) {
                IQConversionDivisor = (float)32767.0;
        }


        /* right size our buffers -- if the dsp is running small bufs, we need to
         * do the same.
         */

        if ( samples_per_block < 1024 ) {
                FPGAWriteBufSize = 1024;
        }
        else {
                FPGAWriteBufSize = 2048;
        }

		if ( isMetis ) {  // force write buf size on Metis -- we can only send 2x512 usb frames in an enet frame for Metis so lock this down 
						  // 
			FPGAWriteBufSize = 1024; 
		} 

        // setup sampling rate, buffer sizes  create resampler if needed
        switch ( SampleRate ) {
                case 48000:
                        SampleRateIn2Bits = 0;
                        FPGAReadBufSize = FPGAWriteBufSize; // was 512;
                        MicResamplerP = NULL;
                        break;
#ifndef LINUX
                case 96000:
                        SampleRateIn2Bits = 1;
                        FPGAReadBufSize = 2 * FPGAWriteBufSize;  // was 1024;
                        MicResamplerP = NewResamplerF(48000, 96000);
                        if ( MicResamplerP == NULL ) {
                                fprintf(stderr, "Warning NewResamplerF failed in PowerSDR-Interface.c\n");
                        }
                        break;
                case 192000:
                        SampleRateIn2Bits = 2;
                        FPGAReadBufSize = 4 * FPGAWriteBufSize;  // was 2048;
                        MicResamplerP = NewResamplerF(48000, 192000);
                        if ( MicResamplerP == NULL ) {
                                fprintf(stderr, "Warning NewResamplerF failed in PowerSDR-Interface.c\n");
                        }
                        break;
#else
#warning message("info - LINUX code missing ... NewResamplerF");
#endif

                default:
                        SampleRateIn2Bits = 3;
                        break;
        }
        BlockSize = samples_per_block;
        Callback = callbackp;

        printf("sa: samples_per_block: %d\n", samples_per_block); fflush(stdout);

        // make sure we're not already opened
        //
#ifdef XYLO
        if ( XyloH != NULL || IOThreadRunning ) {
#endif
#ifdef OZY
        if ( OzyH != NULL || IOThreadRunning  ) {
#endif
                return 3;
        }

        do { // once
                // allocate buffers for callback buffers
                bufp = (float *)malloc(sizeof(float) * BlockSize * 8);  // 4 channels for in and 4 channels for out
                if ( bufp == NULL ) {
                        myrc = 5;
                        break;
                }
                // printf("callback buffers at: 0x%08x, len=%d\n", (unsigned long)bufp, 8*BlockSize*sizeof(float));
                CallbackInLbufp = bufp;
                CallbackInRbufp = bufp + BlockSize;
                CallbackMicLbufp = bufp + (2*BlockSize);
                CallbackMicRbufp = bufp + (3*BlockSize);
                CallbackOutLbufp = bufp + (4*BlockSize);
                CallbackOutRbufp = bufp + (5*BlockSize);
                CallbackMonOutLbufp = bufp + (6*BlockSize);
                CallbackMonOutRbufp = bufp + (7*BlockSize);

                // allocate buffers for inbound and outbound samples from USB
                in_sample_bufp = (int *)malloc(sizeof(int) * BlockSize * 4);  // 4 channels in and out
                if ( in_sample_bufp == NULL ) {
                        myrc = 6;
                        break;
                }
                IOSampleInBufp = in_sample_bufp;
                // printf("IOSample buffer at: 0x%08x, len=%d\n", (unsigned long)in_sample_bufp, 4*BlockSize*sizeof(int));

                CBSampleOutBufp = (short *)malloc(sizeof(short) * BlockSize * 4);
                if ( CBSampleOutBufp == NULL ) {
                        myrc = 10;
                        break;
                }
                // printf("CBSampleOut buffer at: 0x%08x, len=%d\n", (unsigned long)CBSampleOutBufp, 4*BlockSize*sizeof(short));  fflush(stdout);

                if ( MicResamplerP != NULL ) {  // we're going to resample mic data - need a buffer for it
                        MicResampleBufp = (float *)malloc(samples_per_block * sizeof(float));
                        if ( MicResampleBufp == NULL ) {
                                myrc = 13;
                                break;
                        }
                }

                // allocate buffer for low level USB I/O
                FPGAReadBufp = (char *)malloc( sizeof(char) * (FPGAReadBufSize + FPGAWriteBufSize));
                if ( FPGAReadBufp == NULL ) {
                        myrc = 14;
                        break;
                }
                /*else*/
                FPGAWriteBufp = FPGAReadBufp + FPGAReadBufSize;

				if ( !isMetis ) { 
#ifdef XYLO
					// go open the xylo usb device
					XyloH = XyloOpen();
					if ( XyloH == NULL ) {
							myrc =  2;
							break;
					}
#endif
#ifdef OZY
					// go open the ozy
					OzyH = OzyOpen();
					if ( OzyH == NULL ) {
						    myrc =  2;
							break;
					}
					// printf("Ozy openend!\n");				
				
				
#endif
				}
				else { // is Metis 
					


					MetisStartReadThread(); 
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

#ifdef EP4DRAIN_ENABLED 
				// 
				// start the ep4 drain thread 
				// 
				sem_init(&EP4DrainThreadInitSem, 0, 0);
                rc = pthread_create(&EP4DrainThreadID, NULL,  EP4DrainThreadMain, NULL);
                if ( rc != 0 ) {  // failed
                        fprintf(stderr, "pthread_created failed on EP4DrainThread w/ rc=%d\n", rc);
                        /* myrc =  4; */ 
                        break;
                }
                sem_wait(&EP4DrainThreadInitSem);  // wait for the thread to get going
#endif 

        } while ( 0 );

        if ( myrc != 0 ) {  // we failed -- clean up
                if ( bufp != NULL ) {
                        CallbackInLbufp = NULL;
                        CallbackInRbufp = NULL;
                        CallbackOutLbufp = NULL;
                        CallbackOutRbufp = NULL;
                        CallbackMicLbufp = NULL;
                        CallbackMicRbufp = NULL;
                        CallbackMonOutLbufp = NULL;
                        CallbackMonOutRbufp = NULL;
                        free(bufp);
                }

                if ( FPGAReadBufp != NULL ) {
                        free(FPGAReadBufp);
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
#ifdef XYLO
                if ( XyloH != NULL ) {
                        XyloClose(XyloH);
                        XyloH = NULL;
                }
#endif
#ifdef OZY
                if ( OzyH != NULL ) {
                        OzyClose(OzyH);
                        OzyH = NULL;
                }
#endif
				if ( isMetis ) { 
					MetisStopReadThread(); /* is a no op if not running */ 
				}
        }
        DotDashBits = 0;
        // printf("StartAudioNative - myrc: %d\n", myrc);
        return myrc;

}


// ff in hz 
KD5TFDVK6APHAUDIO_API void SetVFOfreq_native(double ff) {
        // ff = (1000000.0  * ff * 4294967296.0) / ( 125.0 * 1000000);  // 1Mhz * (f in mhz) * (2**32) / (125 mhz)
        // ff = (1000000.0 *ff); // VK6APH - send frequecy in Hz, conversion to phase now done in FPGA 11 April 2007

        VFOfreq = (int)ff;
        return;
}

//
// StopAudio -- undo what start audio did.  Close/Free/Destroy that which StartAudio Opened/Alloc'd/Created.
//
KD5TFDVK6APHAUDIO_API void StopAudio() {
        int rc;
        printf("stop audio called\n");  fflush(stdout);
        rc = IOThreadStop();
        if ( rc != 0 ) {
                fprintf(stderr, "Warning: IOThreadStop failed with rc=%d\n", rc);
        }
        printf("iothread stopped\n");   fflush(stdout);

		if ( isMetis ) { 
			MetisStopReadThread(); 
		} 
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
#ifdef XYLO
        if ( XyloH != NULL ) {
                XyloClose(XyloH);
                XyloH = NULL;
        }
        printf("xylo closed\n");   fflush(stdout);
#endif
#ifdef OZY
        if ( OzyH != NULL ) {
                OzyClose(OzyH);
                OzyH = NULL;
        }
        printf("Ozy closed\n");   fflush(stdout);
#endif

#ifndef LINUX
        if ( MicResamplerP != NULL ) {
                DelPolyPhaseFIRF(MicResamplerP);
                MicResamplerP = NULL;
        }
#else
#warning message("info - LINUX code missing ... DelPolyPhaseFIRF");
#endif

        if ( FPGAReadBufp != NULL ) {
                free(FPGAReadBufp);
                FPGAReadBufp = NULL;
                FPGAWriteBufp = NULL;
        }

		DotDashBits = 0;

#ifdef EP4DRAIN_ENABLED 
		#pragma message("info: EP4DRAIN is enabled.")
		rc = EP4DrainThreadStop();
        if ( rc != 0 ) {
                fprintf(stderr, "Warning: EP4DrainThreadStop failed with rc=%d\n", rc);
        }
        printf("ep4 drain thread stopped\n");   fflush(stdout);
		do { /* once */ 
			FILE *ofile; 
			ofile = fopen("ep4.dat", "wb"); 
			if ( ofile == NULL ) { 
				fprintf(stderr, "Failed to open ep4.dat"); 				
				break; 
			} 
			fwrite(EP4_DumpBuf, 1, EP4_DumpBufBytesUsed, ofile); 
			fclose(ofile); 
		} while ( 0 ); 
		EP4_DumpBufBytesUsed = 0; 
		
#endif 
        
#ifdef EP6FILEDUMP 
#pragma message("info: EP6FILEDUMP is enabled.")
		do { /* once */ 
			FILE *ofile; 
			ofile = fopen("ep6.dat", "wb"); 
			if ( ofile == NULL ) { 
				fprintf(stderr, "Failed to open ep6.dat"); 				
				break; 
			} 
			fwrite(EP6_DumpBuf, 1, EP6_DumpBufBytesUsed, ofile); 
			fclose(ofile); 
		} while ( 0 ); 
		EP6_DumpBufBytesUsed = 0; 
#endif 

        return;
}

KD5TFDVK6APHAUDIO_API void SetC1Bits(int bits) { 
	C1Mask = bits; 
	return;
}

KD5TFDVK6APHAUDIO_API int GetC1Bits(void) { 
	return C1Mask; 
}


KD5TFDVK6APHAUDIO_API void SetPennyOCBits(int b) { 
	PennyOCBits = b; 
	return;
}

KD5TFDVK6APHAUDIO_API void SetAlexAntBits(int rx_ant, int tx_ant, int rx_out) {  
	

	rx_ant = ( rx_ant << 5); 
	AlexRxAnt = rx_ant & 0x60; 

	
	AlexTxAnt = (tx_ant - 1) & 0x3;
	if ( AlexTxAnt >= 3 ) AlexTxAnt = 1;  
	
	if ( rx_out ) { 
		AlexRxOut = 0x80; 
	} 
	else { 
		AlexRxOut = 0; 
	} 
	return;
}

KD5TFDVK6APHAUDIO_API void SetNRx(int nrx) {  
	
	nrx = ( nrx << 3); 
	NRx = nrx & 0x38; 

	return;
}

int getDDPTTcount = 0; 
int last_DDP = 0; 

KD5TFDVK6APHAUDIO_API int nativeGetDotDashPTT() {
		
	if ( last_DDP != (DotDashBits & 0x7) ) { 
		last_DDP = DotDashBits & 0x7; 
		printf("ddp: 0x%04x\n", last_DDP);  fflush(stdout); 
	}		
    return DotDashBits & 0x7;
}

KD5TFDVK6APHAUDIO_API void SetAlexManEnable(int bit) { 
        if ( bit == 0 ) {
                AlexManEnable = 0;
        }
        else {
                AlexManEnable = (1 << 6);
        }
//printf("SetAlexEnable: %d\n", bit); fflush(stdout); 
}

KD5TFDVK6APHAUDIO_API void SetAlexEnabled(int bit) { 
        if ( bit == 0 ) {
                AlexEnabled = 0;
        }
        else {
                AlexEnabled = 1;
        }
//printf("SetAlexEnable: %d\n", bit); fflush(stdout); 
}

KD5TFDVK6APHAUDIO_API void SetAlexHPFBits(int bits) { 
	AlexHPFMask = bits; 
	return;
}

KD5TFDVK6APHAUDIO_API void SetAlexLPFBits(int bits) { 
	AlexLPFMask = bits; 
	return;
}

KD5TFDVK6APHAUDIO_API void SetXmitBit(int xmit) {   // bit xmitbit ==0, recv mode, != 0, xmit mode
        if ( xmit == 0 ) {
                XmitBit = 0;
        }
        else {
                XmitBit = 1;
        }
}

KD5TFDVK6APHAUDIO_API void SetDuplex(int dupx) {   // dupx == 0, half duplex, != 0, full duplex
        if ( dupx != 0 ) {
                Duplex = (1 << 2);
        }
        else {
                Duplex = 0;
        }
		return;
}

KD5TFDVK6APHAUDIO_API void EnableHermesPower(int enabled) { 
	printf("HermesPowerEndabled: %d\n", enabled); fflush(stdout); 
	HermesPowerEnabled = enabled; 
} 

KD5TFDVK6APHAUDIO_API void SetMicBoost(int bits) { // 0 == 0dB, 1 == 20dB
        if ( bits == 0 ) {
                MicBoost = 0;
        }
        else {
                MicBoost = 1;
        }
		return;
}

KD5TFDVK6APHAUDIO_API void SetLineIn(int bits) { // 0 == Mic-In, 1 == Line-In
	if ( bits != 0 ) { 
		LineIn = (1 << 1); 
	} 
	else { 
		LineIn = 0;
	}	
	return;
}

KD5TFDVK6APHAUDIO_API void SetAlexAtten(int bits) { 
	MercAtten = bits; 
	if ( MercAtten > 3 ) MercAtten = 0; 
	return;
}

KD5TFDVK6APHAUDIO_API void SetMercDither(int bits) { 
	if ( bits != 0 ) { 
		MercDither = (1 << 3); 
	} 
	else { 
		MercDither = 0;
	}	
	return;
}

KD5TFDVK6APHAUDIO_API void SetMercPreamp(int bits) { 
	if ( bits != 0 ) { 
		MercPreamp = (1 << 2); 
	} 
	else { 
		MercPreamp = 0; 
	}	
	return;
}

KD5TFDVK6APHAUDIO_API void SetMercRandom(int bits) { 
	if ( bits != 0 ) { 
		MercRandom = (1 << 4); 
	} 
	else { 
		MercRandom = 0; 
	}	
	return;
}

KD5TFDVK6APHAUDIO_API void SetFPGATestMode(int i) {
	FPGATestMode = i;
}

KD5TFDVK6APHAUDIO_API void SetOutputPowerFactor(int u) {
	OutputPowerFactor = (unsigned int)(u & 0xff);
}

KD5TFDVK6APHAUDIO_API void SetLegacyDotDashPTT(int bit) { 
	if ( bit ) { 
		CandCAddrMask = 0xfc; 
		CandCFwdPowerBit = 0x4; 
		DotDashMask = 0x3;
	}
	else { 
		CandCAddrMask = 0xf8; 
		CandCFwdPowerBit = 0x8; 
		DotDashMask = 0x7;
	} 
}

KD5TFDVK6APHAUDIO_API int getAndResetADC_Overload() { 
	int n; 
	n = ADC_Overloads; 
	ADC_Overloads = 0; 
	return n; 
} 

KD5TFDVK6APHAUDIO_API int getMercuryFWVersion() { 
	return MercuryFWVersion; 
} 

KD5TFDVK6APHAUDIO_API int getMercury2FWVersion() { 
	return Mercury2FWVersion; 
} 

KD5TFDVK6APHAUDIO_API int getMercury3FWVersion() { 
	return Mercury3FWVersion; 
} 

KD5TFDVK6APHAUDIO_API int getMercury4FWVersion() { 
	return Mercury4FWVersion; 
} 

KD5TFDVK6APHAUDIO_API int getPenelopeFWVersion() { 
	return PenelopeFWVersion; 
} 

KD5TFDVK6APHAUDIO_API int getOzyFWVersion() { 
	return OzyFWVersion; 
} 

KD5TFDVK6APHAUDIO_API int getHaveSync() { 
	return HaveSync; 
} 

KD5TFDVK6APHAUDIO_API int getFwdPower() { 
	return FwdPower; 
} 

KD5TFDVK6APHAUDIO_API int getRefPower() { 
	return RefPower; 
} 

KD5TFDVK6APHAUDIO_API int getAlexFwdPower() { 
	return AlexFwdPower; 
} 

KD5TFDVK6APHAUDIO_API int getControlByteIn(int n) { 
	if ( n < 0 || n > 4 ) { 
		return -1; 
	} 
	return ControlBytesIn[n];  
} 

//
// bufp MUST point to a 4096 byte buffer 
// 
// returns 0 on success 
// -1 - receiver not on 
// -2 - timeout 
// -3 - short read 
KD5TFDVK6APHAUDIO_API int GetEP4Data(char *bufp) { 
	int numread; 
	if ( OzyH == NULL || !IOThreadRunning ) { 
		return -1; 
	}
	numread = OzyBulkRead(OzyH, 0x84, bufp, 4096); 
	if ( numread <= 0 )  {  /* read failed - bail out */ 
		return -2; 		
	} 
	if ( numread != 4096 ) { 
		return -3; 
	} 
	return 0; 
}

// diag data mapping
// 0-4 C0-C4 in
// 5-9 C0-C4 out
// 10 sync gain count
// 11 sync lost count
// 12 not ok to send count
// 13 have sync
KD5TFDVK6APHAUDIO_API int GetDiagData(int *a, int count) {
        int i;
        for ( i = 0; i < 5; i++ ) {  /* do the ControlBytesIn */
                if ( count == i ) return count; /* bail if we're out of space */
                a[i] = ControlBytesIn[i];
        }
        for ( i = 5; i < 10; i++ ) {
                if ( count == i ) return count;
                a[i] = ControlBytesOut[i-5];
        }
        if ( count == 10 ) return 10;
        a[10] = SyncGainedCount;
        if ( count == 11 ) return 11;
        a[11] = LostSyncCount;
        if ( count == 12 ) return 12;
        a[12] = NotOKtoSendCount;
        if ( count == 13 ) return 13;
        a[13] = HaveSync;
        return 14;
}

///
/// Obsolete 2 channel versions of the code
///

//
// StartAudio -- returns 0 on succes, !0 on failure
// samples_per_block is how many samples powersdr desires in a block -- the total number of samples
// returned in a block will be 2x this (2 channels)
//
// error returns:
//      2 - could not open Xylo
//  3 - bad state - already started
//  4 - failed creating IOThread
//  5 - failed to alloc mem for callback input buffer
//  6 - failed to alloc mem for input buffers
//  7 - failed creating fifo for io -> callback
//  8 - failed creating callback -> io fifo
//  9 - failed creating callback thread
// 10 - failed alloc of callback return buffer

#if 0
KD5TFDVK6APHAUDIO_API int StartAudio(int samples_per_block, int (__stdcall *callbackp)(void *inp, void *outp, int framcount, void *timeinfop, int flags, void *userdata)) {
        int rc;
        int myrc = 0;
        int *in_sample_bufp = NULL;
        float *bufp = NULL;

        BlockSize = samples_per_block;
        Callback = callbackp;

        printf("tfd: start-audio starts\n"); fflush(stdout);

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
                // and a fifo for the outbound stuff
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


#endif

