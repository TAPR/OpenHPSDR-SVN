// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the NANOTIMER_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// NANOTIMER_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifndef wjtNANOTIMER_INCLUDED
#define wjtNANOTIMER_INCLUDED 1 
#ifdef NANOTIMER_EXPORTS
#define NANOTIMER_API __declspec(dllexport)
#else
#define NANOTIMER_API __declspec(dllimport)
#endif

#include <stdio.h> 
#if 0 
// This class is exported from the nanotimer.dll
class NANOTIMER_API Cnanotimer {
public:
	Cnanotimer(void);
	// TODO: add your methods here.
};

extern NANOTIMER_API int nnanotimer;

NANOTIMER_API int fnnanotimer(void);
#endif 

NANOTIMER_API __int64 getPerfTicks(void); 
NANOTIMER_API __int64 getPerfFreq(void); 
NANOTIMER_API __int64 perfTicksToNanos(__int64 ticks);


/* hi low average routines */ 
typedef struct HLA_count { 
	__int64 sum; 
	__int64 count; 
	__int64 hi; 
	__int64 lo; 
}  HLA_COUNTER; 

NANOTIMER_API void updateHLA(HLA_COUNTER *p, __int64 v); 
NANOTIMER_API void initHLA(HLA_COUNTER *p); 
NANOTIMER_API void printHLA(HLA_COUNTER *p, /* FILE *f, */ unsigned char *prefix); 
NANOTIMER_API void printHLANano(HLA_COUNTER *p, unsigned char *prefix); 

#endif