/* resampleF.h

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2004, 2005, 2006 by Frank Brickle, AB2KT and Bob McGwier, N4HY

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

The authors can be reached by email at

ab2kt@arrl.net
or
rwmcgwier@comcast.net

or by paper mail at

The DTTS Microwave Society
6 Kathleen Place
Bridgewater, NJ 08807
*/

#ifndef _RESAMPLEF_H
#define _RESAMPLEF_H

//#include <common.h>
//Added from Window.h
#include "complex.h"

typedef enum _windowtype
{
  RECTANGULAR_WINDOW,
  HANNING_WINDOW,
  WELCH_WINDOW,
  PARZEN_WINDOW,
  BARTLETT_WINDOW,
  HAMMING_WINDOW,
  BLACKMAN2_WINDOW,
  BLACKMAN3_WINDOW,
  BLACKMAN4_WINDOW,
  EXPONENTIAL_WINDOW,
  RIEMANN_WINDOW,
  BLACKMANHARRIS_WINDOW,
  NUTTALL_WINDOW,
} Windowtype;



//Added from bnal.h
#define max(a, b) ((a) > (b) ? (a) : (b))
#define FALSE 0

//Added from filter.h

#define delFIR_Lowpass_REAL(p) delFIR_REAL(p)

#define FIRcoef(p) ((p)->coef)
#define FIRtap(p, i) (FIRcoef(p)[(i)])
#define FIRsize(p) ((p)->size)
#define FIRtype(p) ((p)->type)
#define FIRiscomplex(p) ((p)->cplx)
#define FIRisreal(p) (!FIRiscomplex(p))
#define FIRfqlo(p) ((p)->freq.lo)
#define FIRfqhi(p) ((p)->freq.hi)


// added from filter.h
//#include <fftw3.h>

typedef enum
{
  FIR_Undef, FIR_Lowpass, FIR_Bandpass, FIR_Highpass, FIR_Hilbert,
  FIR_Bandstop
} FIR_response_type;

typedef struct _real_FIR
{
  REAL *coef;
  int size;
  FIR_response_type type;
  BOOLEAN cplx;
  struct
  {
    REAL lo, hi;
  } freq;
} RealFIRDesc, *RealFIR;
// above


typedef struct resampleF_state
{
  REAL *input, *output, *filterMemoryBuff;
  RealFIR filter;
  int filterMemoryBuffLength, inputArrayLength, numFiltTaps,
    indexfiltMemBuf, interpFactor, filterPhaseNum, deciFactor,
    numOutputSamples;
  int MASK;
} resamplerF, *ResStF;

//DttSP_EXP 
extern ResStF newPolyPhaseFIRF (int filterMemoryBuffLength,
					  int indexfiltMemBuf,
					  int interpFactor,
					  int filterPhaseNum, int deciFactor);
//DttSP_EXP 
extern void PolyPhaseFIRF (ResStF resst);
//DttSP_EXP 
extern void delpolyPhaseFIRF (ResStF resst);
#endif
