#ifndef _correctIQ_h
#define _correctIQ_h

#ifdef __LINUX__
#include <bufvec.h>
#else
#include "bufvec.h"
#endif

typedef struct _iqstate
{
  REAL phase, gain;
} *IQ, iqstate;

extern IQ newCorrectIQ (REAL phase, REAL gain);
extern void delCorrectIQ (IQ iq);
extern void correctIQ (CXB sigbuf, IQ iq);

#endif
