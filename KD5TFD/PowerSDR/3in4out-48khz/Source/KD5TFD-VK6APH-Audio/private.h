// Copyright (C) Bill Tracey (KD5TFD) 2006 
// email: bill@ewjt.com 

// this header includes declarations for private functions -- that is stuff that should not be called from outside 
// the dll 

#ifndef KD5TFDVK6APHAUDIO_PRIVATE_INCLUDED 
#define KD5TFDVK6APHAUDIO_PRIVATE_INCLUDED 
#include "KD5TFD-VK6APH-Audio.h"
#ifdef PERF_DEBUG 
#include "nanotimer.h"
#endif 
#include <windows.h> 

// includes for pthread things 
#include <pthread.h> 
#include <semaphore.h> 


// PowerSDR interface routines 
// extern KD5TFDVK6APHAUDIO_API int StartAudio(int block_size); 
extern KD5TFDVK6APHAUDIO_API int StartAudio(int samples_per_block, int (__stdcall *callback)(void *inp, void *outp, int framcount, void *timeinfop, int flags, void *userdata));
extern KD5TFDVK6APHAUDIO_API void StopAudio(void); 
extern KD5TFDVK6APHAUDIO_API int GetDotDashBits(void); 

#if 0 
extern KD5TFDVK6APHAUDIO_API int StartAudio_4port(int samples_per_block, int (__stdcall *callback)(void *inp, void *outp, int framcount, void *timeinfop, int flags, void *userdata));
extern KD5TFDVK6APHAUDIO_API void StopAudio_4port(void); 
#endif 

// Xylo IO routines 
extern HANDLE *XyloOpen(void); 
extern void XyloClose(HANDLE *h); 
extern int XyloBulkWrite(HANDLE *h, ULONG pipe, void* buffer, ULONG buffersize);
extern WORD XyloBulkRead(HANDLE *h, ULONG pipe, void* buffer, ULONG buffersize);

// IOThread rountines 
extern void *IOThreadMain(void *argp); 
extern int IOThreadStop(void); 

// callback thread routines 
extern void *CallbackThreadMain(void *argp);
extern void Callback_ProcessBuffer(int *bufp, int buflen); 

// fifo routines 
extern void *createFIFO(void);
extern int putFIFO(void *fifoh, void *datap, int dlen);
extern void freeFIFOdata(void *p);
extern void *getFIFO(void *fifoh, int *dlenp, int wait);
extern void destroyFIFO(void *fifoh);

// all extern declarations need to be above this point 


// global variables for the DLL 
#ifdef GLOBAL_DECL 
#define extern 
#endif 
extern HANDLE *XyloH;         // handle to the Xylo 
extern int BlockSize;         // num samples in a block -- total sample count in block is number of channels time this 
extern pthread_t IOThreadID;  // tid of IOthread 
extern pthread_t CallbackThreadID; // tid of callback thread 
extern int IOThreadRunning;   // non zero if IOThread is running 
extern int CallbackThreadRunning; 
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
// buffer for the IO Thread 
extern int *IOSampleInBufp;  // samples in iothread coming from audio device 
extern short *CBSampleOutBufp; // sample in iothread headed for the audio device 
#ifdef PERF_DEBUG
extern HLA_COUNTER InConvertHLA; 
extern HLA_COUNTER OutConvertHLA; 
extern HLA_COUNTER CallbackHLA; 
#endif 
extern int DotDashBits; 

#ifdef GLOBAL_DECL
#undef extern
#endif
#endif 