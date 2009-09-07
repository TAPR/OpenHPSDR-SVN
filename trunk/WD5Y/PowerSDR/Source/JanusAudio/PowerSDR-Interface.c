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
        if ( OzyH != NULL || IOThreadRunning ) {
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
        }
        DotDashBits = 0;
        // printf("StartAudioNative - myrc: %d\n", myrc);
        return myrc;

}


KD5TFDVK6APHAUDIO_API void SetVFOfreq(double ff) {
        // ff = (1000000.0  * ff * 4294967296.0) / ( 125.0 * 1000000);  // 1Mhz * (f in mhz) * (2**32) / (125 mhz)
    ff = (1000000.0 *ff); // VK6APH - send frequecy in Hz, conversion to phase now done in FPGA 11 April 2007
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
        return;
}

KD5TFDVK6APHAUDIO_API int GetDotDash() {
        return DotDashBits & 0x3;
}

KD5TFDVK6APHAUDIO_API void SetXmitBit(int xmit) {   // bit xmitbit ==0, recv mode, != 0, xmit mode
        if ( xmit == 0 ) {
                XmitBit = 0;
        }
        else {
                XmitBit = 1;
        }
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

