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


// this header includes declarations for private functions -- that is stuff that should not be called from outside
// the dll


// incldue dttsp stuff
#ifndef LINUX
#include <common.h>
#endif

#undef XYLO
#define OZY 1
//#define EP4DRAIN_ENABLED (1)
//#define EP6FILEDUMP (1)

#ifndef LINUX
#if 1
//#pragma message("info: Using explict externs for resamplers - should be in a DttSP header!")
extern DttSP_EXP void *NewResamplerF (int samplerate_in, int samplerate_out);
extern DttSP_EXP void DoResamplerF (float *input, float *output, int numsamps, int *outsamps, /*ResStF*/ void *ptr);
extern DttSP_EXP void DelPolyPhaseFIRF (/*ResSt*/ void * resst);
#else
#include <resamplef.h>
#endif
#endif

#ifndef KD5TFDVK6APHAUDIO_PRIVATE_INCLUDED
#define KD5TFDVK6APHAUDIO_PRIVATE_INCLUDED
#include "KD5TFD-VK6APH-Audio.h"
#ifdef PERF_DEBUG
#include "nanotimer.h"
#endif
#ifndef LINUX
#include <windows.h>
#endif


#ifdef LINUX
#define __stdcall
#endif


// includes for pthread things
#include <pthread.h>
#include <semaphore.h>


// PowerSDR interface routines
// extern KD5TFDVK6APHAUDIO_API int StartAudio(int block_size);
extern KD5TFDVK6APHAUDIO_API int StartAudioNative(int sample_rate, int samples_per_block, int (__stdcall *callback)(void *inp, void *outp, int framcount, void *timeinfop, int flags, void *userdata), int sample_bits, int no_send);

extern KD5TFDVK6APHAUDIO_API void StopAudio(void);
extern KD5TFDVK6APHAUDIO_API int GetDotDashBits(void);
extern KD5TFDVK6APHAUDIO_API void SetXmitBit(int xmitbit);  // bit xmitbit ==0, recv mode, != 0, xmit mode
extern KD5TFDVK6APHAUDIO_API int GetDiagData(int *a, int count);
extern KD5TFDVK6APHAUDIO_API void SetVFOfreq(double f);
//extern KD5TFDVK6APHAUDIO_API void SetAlexEnable(int bit);

#if 0
extern KD5TFDVK6APHAUDIO_API int StartAudio_4port(int samples_per_block, int (__stdcall *callback)(void *inp, void *outp, int framcount, void *timeinfop, int flags, void *userdata));
extern KD5TFDVK6APHAUDIO_API void StopAudio_4port(void);
#endif

#ifdef XYLO
// Xylo IO routines  - now obsolete
extern HANDLE *XyloOpen(void);
extern void XyloClose(HANDLE *h);
extern int XyloBulkWrite(HANDLE *h, ULONG pipe, void* buffer, ULONG buffersize);
extern WORD XyloBulkRead(HANDLE *h, ULONG pipe, void* buffer, ULONG buffersize);
#endif

#ifdef OZY
// Ozy IO Routines
struct OzyHandle {
        struct usb_device *devp;
        struct usb_dev_handle *h;
};

extern KD5TFDVK6APHAUDIO_API int OzyBulkWrite(struct OzyHandle *h, int ep, void* buffer, int buffersize);
extern KD5TFDVK6APHAUDIO_API int OzyBulkRead(struct OzyHandle *h, int ep, void* buffer, int buffersize);
extern KD5TFDVK6APHAUDIO_API int LoadFirmware(char * filename);
extern KD5TFDVK6APHAUDIO_API int LoadFPGA(char * filename);

#endif

// IOThread rountines
extern void *IOThreadMain(void *argp);
extern int IOThreadStop(void);

#ifdef EP4DRAIN_ENABLED
// EP4DrainThread rountines
extern void *EP4DrainThreadMain(void *argp);
extern int EP4DrainThreadStop(void);
#endif

// callback thread routines
extern void *CallbackThreadMain(void *argp);
extern void Callback_ProcessBuffer(int *bufp, int buflen);

// fifo routines
extern void *createFIFO(void);
extern int putFIFO(void *fifoh, void *datap, int dlen);
extern void freeFIFOdata(void *p);
extern void *getFIFO(void *fifoh, int *dlenp, int wait);
extern void destroyFIFO(void *fifoh);


// ringbuffer routines
extern void *createRingBuffer(int size);
extern void destroyRingBuffer(void *p);
extern int getRingBuffer(void *rbh, char *bufp, int dlen, int wait);
extern int putRingBuffer(void *rbh, char *datap, int dlen);
extern int resetRingBuffer(void *rbh);

// metis stuff
extern int MetisStartReadThread(void);
extern void MetisStopReadThread(void);
extern int MetisBulkRead(int endpoint, char *bufp, int buflen);
extern int MetisBulkWrite(int endpoint, char *bufp, int buflen);
extern int MetisReadDirect(char *bufp, int buflen);


extern void ForceCandCFrame(void);
// all extern declarations need to be above this point


// global variables for the DLL
#ifdef GLOBAL_DECL
#define extern
#endif
#ifdef XYLO
extern HANDLE *XyloH;         // handle to the Xylo
#endif
#ifdef OZY
extern struct OzyHandle *OzyH;
#endif
extern int BlockSize;         // num samples in a block -- total sample count in block is number of channels time this
extern pthread_t IOThreadID;  // tid of IOthread

#ifdef EP4DRAIN_ENABLED
extern pthread_t EP4DrainThreadID;  // tid of IOthread
extern int EP4DrainThreadRunning;   // non zero if IOThread is running
extern sem_t EP4DrainThreadInitSem; // sem gating init of EP4DrainThread
#endif

extern int IOThreadRunning;   // non zero if IOThread is running

extern int CallbackThreadRunning;
extern pthread_t CallbackThreadID; // tid of callback thread
extern sem_t IOThreadInitSem; // sem gating init of IOThread

extern sem_t CallbackThreadInitSem; // sem gating init of callback thread
extern void *InSampleFIFOp;  // FIFO for samples coming in from device
extern void *OutSampleFIFOp;  // FIFO for outbound samples
extern int (__stdcall *Callback)(void *inp, void *outp, int framcount, void *timeinfop, int flags, void *userdata); // pointer to callback function
// buffers for the callback thread
extern float *CallbackInLbufp;  // left buffer for samples headed to powersdr
extern float *CallbackInRbufp;  // right buffer for samples headed to powersdr
extern float *CallbackMicLbufp; // left buffer of mic samples
extern float *CallbackMicRbufp; // right buffer of mic samples
extern float *CallbackOutLbufp; // left buffer for samples headed to the audio device  (iq)
extern float *CallbackOutRbufp; // right buffer for samples header to the audio device  (iq)
extern float *CallbackMonOutLbufp;  // left for monitor out
extern float *CallbackMonOutRbufp;  // right for monitor out
extern float *MicResampleBufp; // buffer for mic resampling -- holds pre resample data
// buffer for the IO Thread
extern int *IOSampleInBufp;  // samples in iothread coming from audio device
extern short *CBSampleOutBufp; // sample in iothread headed for the audio device
#ifdef PERF_DEBUG
extern HLA_COUNTER InConvertHLA;
extern HLA_COUNTER OutConvertHLA;
extern HLA_COUNTER CallbackHLA;
#endif
extern int DotDashBits;
extern int XmitBit;
extern int SampleRate;
extern unsigned char SampleRateIn2Bits; // value of sample rate to send to fpga
extern void *MicResamplerP; // Mic resampler filter
extern int SampleBits;  // how many bits in a sample
extern int ForceNoSend;  // force no sending of data to Janus
extern float IQConversionDivisor;  // divisor to use converting sample ints to floats

extern unsigned char ControlBytesIn[5];
extern unsigned char ControlBytesOut[5];
extern unsigned int LostSyncCount;
extern unsigned int SyncGainedCount;
extern unsigned int NotOKtoSendCount;
extern int HaveSync;
extern int VFOfreq;
extern int FPGATestMode;

#ifdef EP6FILEDUMP
#define EP6FRAMEDUMPLIMIT (100*512)
extern int EP6_DumpBufBytesUsed;
extern char EP6_DumpBuf[EP6FRAMEDUMPLIMIT];
#endif

#ifdef EP4DRAIN_ENABLED
#define EP4FRAMEDUMPLIMIT (100*512)
extern int EP4_DumpBufBytesUsed;
extern char EP4_DumpBuf[EP6FRAMEDUMPLIMIT];
#endif


#if 0
#define DIAG_C0_IN (0)
#define DIAG_C1_IN (1)
#define DIAG_C2_IN (2)
#define DIAG_C3_IN (3)
#define DIAG_C4_IN (4)
#define DIAG_C0_OUT (5)
#define DIAG_C1_OUT (6)
#define DIAG_C2_OUT (7)
#define DIAG_C3_OUT (8)
#define DIAG_C4_OUT (9)

#define DIAG_DATA_COUNT (10)
extern int DiagData[DIAG_DATA_COUNT];
#endif

// buffers to handle variable size buffers to/from FPGA
extern int FPGAReadBufSize;
extern int FPGAWriteBufSize;
extern char *FPGAReadBufp;
extern char *FPGAWriteBufp;

extern int C1Mask;
extern char PennyOCBits; /* pennny open collector bits */

extern int MercAtten;
extern int MercDither;
extern int MercPreamp;
extern int MercRandom;

extern int MicBoost;
extern int LineIn;

extern unsigned char AlexManEnable;
extern unsigned char AlexEnabled;
extern int AlexHPFMask;
extern int AlexLPFMask;

extern int Duplex;
extern int NRx;

extern int AlexRxAnt;
extern int AlexTxAnt;
extern int AlexRxOut;
extern int FwdPower;
extern int RefPower;
extern int AlexFwdPower;

extern int ADC_Overloads;

extern int MercuryFWVersion;
extern int Mercury2FWVersion;
extern int Mercury3FWVersion;
extern int Mercury4FWVersion;
extern int PenelopeFWVersion;
extern int OzyFWVersion;
extern unsigned int OutputPowerFactor;
extern int HermesPowerEnabled;

#ifndef GLOBAL_DECL
extern int CandCAddrMask;
extern int CandCFwdPowerBit;
extern int DotDashMask;
#else
int CandCAddrMask = 0xf8;
int CandCFwdPowerBit = 0x8;
int DotDashMask = 0x7;
#endif

/* Metis stuff */
extern int isMetis;
extern SOCKET listenSock;
// extern SOCKET discoverySock;
extern WSADATA WSAdata;
extern int WSAinitialized;
// extern int MetisReadThreadRunning;

#if 0
// dbg buffer
extern char wjtDbgBuf[80];
#endif

#ifdef GLOBAL_DECL
#undef extern
#endif
#endif

