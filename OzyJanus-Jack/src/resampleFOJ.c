/* resampleF.c

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

#include <stdio.h> 
#include <stdlib.h>
#include <math.h>
#include "resampleFOJ.h"


// Added from benal.c

REAL
sqr (REAL x)
{
  return x * x;
}


//added from bufvec.c

void
safefree (char *p)
{
  if (p)
    free ((void *) p);
}


void
delvec_REAL (REAL * vec)
{
  if (vec)
    free ((char *) vec);
}



//Added from filter.c

//static REAL onepi = 3.141592653589793;
//#define M_PI 3.14159265358928
#define onepi M_PI
#define twopi (2*onepi)

void
delFIR_REAL (RealFIR p)
{
  if (p)
    {
      delvec_REAL (FIRcoef (p));
      free ((void *) p);
    }
}


char *
safealloc (int count, int nbytes, char *tag)
{
  char *p = calloc (count, nbytes);
//  char *p = (char *)malloc((size_t)(nbytes*count));
//  memset(p,0,(size_t)(count*nbytes));
  if (!p)
    {
      if (tag && *tag)
	fprintf (stderr, "safealloc: %s\n", tag);
      else
	perror ("safealloc");
      exit (1);
    }
  return p;
}

REAL *
newvec_REAL (int size, char *tag)
{
  return (REAL *) safealloc (size, sizeof (REAL), tag);
}

RealFIR
newFIR_REAL (int size, char *tag)
{
  RealFIR p = (RealFIRDesc *) safealloc (1, sizeof (RealFIRDesc), tag);
  FIRcoef (p) = (REAL *) safealloc (size, sizeof (REAL), tag);
  FIRsize (p) = size;
  FIRtype (p) = FIR_Undef;
  FIRiscomplex (p) = FALSE;
  FIRfqlo (p) = FIRfqhi (p) = -1.0;
  return p;
}

// Added from window.c

REAL *
makewindow (Windowtype type, int size, REAL * window)
{
  int i, j, midn, midp1, midm1;
  REAL freq, rate, sr1, angle, expn, expsum, cx, two_pi;

  midn = size >> 1;
  midp1 = (size + 1) / 2;
  midm1 = (size - 1) / 2;
  two_pi = (REAL) (8.0 * atan (1.0));
  freq = two_pi / (REAL) size;
  rate = (REAL) (1.0 / (REAL) midn);
  angle = 0.0;
  expn = (REAL) (log (2.0) / (REAL) midn + 1.0);
  expsum = 1.0;

  switch (type)
    {
    case RECTANGULAR_WINDOW:
      for (i = 0; i < size; i++)
	window[i] = 1.0;
      break;
    case HANNING_WINDOW:	/* Hann would be more accurate */
      for (i = 0, j = size - 1, angle = 0.0; i <= midn;
	   i++, j--, angle += freq)
	window[j] = (window[i] = (REAL) (0.5 - 0.5 * cos (angle)));
      break;
    case WELCH_WINDOW:
      for (i = 0, j = size - 1; i <= midn; i++, j--)
	window[j] =
	  (window[i] =
	   (REAL) (1.0 - sqr ((REAL) (i - midm1) / (REAL) midp1)));
      break;
    case PARZEN_WINDOW:
      for (i = 0, j = size - 1; i <= midn; i++, j--)
	window[j] =
	  (window[i] =
	   (REAL) (1.0 - fabs ((REAL) (i - midm1) / (REAL) midp1)));
      break;
    case BARTLETT_WINDOW:
      for (i = 0, j = size - 1, angle = 0.0; i <= midn;
	   i++, j--, angle += rate)
	window[j] = (window[i] = angle);
      break;
    case HAMMING_WINDOW:
      for (i = 0, j = size - 1, angle = 0.0; i <= midn;
	   i++, j--, angle += freq)
	window[j] = (window[i] = (REAL) (0.54 - 0.46 * cos (angle)));
      break;
    case BLACKMAN2_WINDOW:	/* using Chebyshev polynomial equivalents here */
      for (i = 0, j = size - 1, angle = 0.0; i <= midn;
	   i++, j--, angle += freq)
	{
	  cx = (REAL) cos (angle);
	  window[j] = (window[i] =
		       (REAL) (.34401 + (cx * (-.49755 + (cx * .15844)))));
	}
      break;
    case BLACKMAN3_WINDOW:
      for (i = 0, j = size - 1, angle = 0.0; i <= midn;
	   i++, j--, angle += freq)
	{
	  cx = (REAL) cos (angle);
	  window[j] =
	    (window[i] =
	     (REAL) (.21747 +
		     (cx * (-.45325 + (cx * (.28256 - (cx * .04672)))))));
	}
      break;
    case BLACKMAN4_WINDOW:
      for (i = 0, j = size - 1, angle = 0.0; i <= midn;
	   i++, j--, angle += freq)
	{
	  cx = (REAL) cos (angle);
	  window[j] = (window[i] = (REAL)
		       (.084037 +
			(cx *
			 (-.29145 +
			  (cx *
			   (.375696 + (cx * (-.20762 + (cx * .041194)))))))));
	}
      break;
    case EXPONENTIAL_WINDOW:
      for (i = 0, j = size - 1; i <= midn; i++, j--)
	{
	  window[j] = (window[i] = (REAL) (expsum - 1.0));
	  expsum *= expn;
	}
      break;
    case RIEMANN_WINDOW:
      sr1 = two_pi / (REAL) size;
      for (i = 0, j = size - 1; i <= midn; i++, j--)
	{
	  if (i == midn)
	    window[j] = (window[i] = 1.0);
	  else
	    {
	      /* split out because NeXT C compiler can't handle the full expression */
	      cx = sr1 * (midn - i);
	      window[i] = (REAL) (sin (cx) / cx);
	      window[j] = window[i];
	    }
	}
      break;
    case BLACKMANHARRIS_WINDOW:
      {
	REAL a0 = 0.35875f, a1 = 0.48829f, a2 = 0.14128f, a3 = 0.01168f;


	for (i = 0; i < size; i++)
	  {
	    window[i] =
	      (REAL) (a0 -
		      a1 * cos (two_pi * (REAL) (i + 0.5) /
				(REAL) (size - 1)) +
		      a2 * cos (2.0 * two_pi * (REAL) (i + 0.5) /
				(REAL) (size - 1)) -
		      a3 * cos (3.0 * two_pi * (REAL) (i + 0.5) /
				(REAL) (size - 1)));
	  }
      }
      break;
    case NUTTALL_WINDOW:
      {
	REAL a0 = 0.3635819f, a1 = 0.4891775f, a2 = 0.1365995f, a3 =
	  0.0106411f;


	for (i = 0; i < size; i++)
	  {
	    window[i] =
	      (REAL) (a0 -
		      a1 * cos (two_pi * (REAL) (i + 0.5) /
				(REAL) (size - 1)) +
		      a2 * cos (2.0 * two_pi * (REAL) (i + 0.5) /
				(REAL) (size - 1)) -
		      a3 * cos (3.0 * two_pi * (REAL) (i + 0.5) /
				(REAL) (size - 1)));
	  }
      }
      break;
    default:
      return 0;
      break;
    }

  return window;
}


RealFIR
newFIR_Lowpass_REAL (REAL cutoff, REAL sr, int size)
{
  if ((cutoff < 0.0) || (cutoff > (sr / 2.0)))
    return 0;
  else if (size < 1)
    return 0;
  else
    {
      RealFIR p;
      REAL *h, *w, fc = cutoff / sr;
      REAL corrector = 0.0;
      int i, midpoint;

      if (!(size & 01))
	size++;
      midpoint = (size >> 01) | 01;
      p = newFIR_REAL (size, "newFIR_Lowpass_REAL");
      h = FIRcoef (p);
      w = newvec_REAL (size, "newFIR_Lowpass_REAL window");
      (void) makewindow (BLACKMANHARRIS_WINDOW, size, w);

      for (i = 1; i <= size; i++)
	{
	  int j = i - 1;
	  if (i != midpoint)
	    h[j] =
	      (REAL) ((sin (twopi * (i - midpoint) * fc) /
		       (onepi * (i - midpoint))) * w[j]);
	  else
	    h[midpoint - 1] = 2.0f * fc;
	}

      delvec_REAL (w);
      FIRtype (p) = FIR_Lowpass;
      return p;
    }
}


//Added from banel.c

int
npoof2 (int n)
{
  int i = 0;
  --n;
  while (n > 0)
    n >>= 1, i++;
  return i;
}

int
nblock2 (int n)
{
  return 1 << npoof2 (n);
}



//DttSP_EXP 
ResStF
newPolyPhaseFIRF (int filterMemoryBuffLength,
		  int indexfiltMemBuf,
		  int interpFactor, int filterPhaseNum, int deciFactor)
{
  ResStF tmp;
  tmp = (ResStF) safealloc (1, sizeof (resamplerF), "PF Resampler");
  tmp->indexfiltMemBuf = indexfiltMemBuf;
  tmp->interpFactor = interpFactor;
  tmp->filterPhaseNum = filterPhaseNum;
  tmp->deciFactor = deciFactor;
  tmp->numFiltTaps = 19839;
  tmp->filterMemoryBuffLength =
    nblock2 (max (filterMemoryBuffLength, tmp->numFiltTaps));
  tmp->MASK = tmp->filterMemoryBuffLength - 1;
  tmp->filterMemoryBuff =
    (float *) safealloc (tmp->filterMemoryBuffLength, sizeof (REAL),
			 "Filter buff: resampler");
  tmp->filter =
    newFIR_Lowpass_REAL (0.45f, (REAL) interpFactor, tmp->numFiltTaps);

  return tmp;
}

//DttSP_EXP 
void
delPolyPhaseFIRF (ResStF resst)
{
  if (resst)
    {
      delFIR_Lowpass_REAL (resst->filter);
      safefree ((char *) resst->filterMemoryBuff);
      safefree ((char *) resst);
    }
}

//DttSP_EXP 
void
PolyPhaseFIRF (ResStF resst)
/******************************************************************************
* CALLING PARAMETERS:
* Name          Use    Description
* ____          ___    ___________
* *input               pointer to input data array
* *output              pointer to output data array
* *filtcoeff           pointer to filter coefficients array
* *filterMemoryBuff    pointer to buffer used as filter memory. Initialized
*                      all data to 0 before 1st call.  length is calculated
*                      from numFiltTaps
* filterMemoryBuffLength length of filterMemoryBuff
* inputArrayLength     length of input array :note that "output" may differ
*                      in length
* numFiltTaps          number of filter taps in array "filtcoeff": <filterMemoryBuffLength
* indexfiltMemBuf     index to where next input sample is to be stored in
*                      "filterMemoryBuff",initalized 0 to before first call
* interpFactor         interpolation factor: output rate = input rate *
*                      "interpFactor" / "deciFactor".
* filterPhaseNum      filter phase number (index), initialized to 0 before
*                      first call
* deciFactor           decimation factor:
*                      output rate = (input rate * "interpFactor"/"deciFactor")
* numOutputSamples    number of output samples placed in array "output"
*
* CALLED BY:
*
* RETURN VALUE:
* Name      Description
* ____      ___________
* none
*
* DESCRIPTION: This function is used to change the sampling rate of the data.
*              The input is first upsampled to a multiple of the desired
*              sampling rate and then down sampled to the desired sampling rate.
*
*              Ex. If we desire a 7200 Hz sampling rate for a signal that has
*                  been sampled at 8000 Hz the signal can first be upsampled
*                  by a factor of 9 which brings it up to 72000 Hz and then
*                  down sampled by a factor of 10 which results in a sampling
*                  rate of 7200 Hz.
*
* NOTES:
*        Also, the "*filterMemoryBuff" MUST be 2^N floats long. This
*        routine uses circular addressing on this buffer assuming that
*        it is 2^N floats in length.
*
******************************************************************************/
{
/******************************************************************************
*               LOCAL VARIABLE DECLARATIONS
*******************************************************************************
* Type              Name                 Description
* ____              ____                 ___________                         */
  int i, j, k, jj;		/* counter variables */
  float *outptr;

  resst->numOutputSamples = 0;


  for (i = 0; i < resst->inputArrayLength; i++)
    {

      /*
       * save data in circular buffer
       */

      resst->filterMemoryBuff[resst->indexfiltMemBuf] = resst->input[i];
      j = resst->indexfiltMemBuf;
      jj = j;


      /*
       * circular addressing
       */

      resst->indexfiltMemBuf = (resst->indexfiltMemBuf + 1) & resst->MASK;

      /*
       * loop through each filter phase: interpolate then decimate
       */

      while (resst->filterPhaseNum < resst->interpFactor)
	{
	  j = jj;
	  /*         output[*numOutputSamples] = 0.0; */
	  outptr = resst->output + resst->numOutputSamples;
	  *outptr = 0.0;

	  /*
	   * perform convolution
	   */

	  for (k = resst->filterPhaseNum; k < resst->numFiltTaps;
	       k += resst->interpFactor)
	    {
	      *outptr +=
		(float) FIRtap (resst->filter,
				k) * resst->filterMemoryBuff[j];

	      /*
	       * circular adressing
	       */

	      j = (j + resst->MASK) & resst->MASK;
	    }

	  /*
	   * scale the data
	   */

	  *outptr *= (float) resst->interpFactor;
	  resst->numOutputSamples += 1;

	  /*
	   * increment interpolation phase # by decimation factor
	   */

	  resst->filterPhaseNum += (resst->deciFactor);

	}			/* end while *filterPhaseNum < interpFactor */

      resst->filterPhaseNum -= resst->interpFactor;

    }				/* end for inputArrayLength */
}				/* end PolyPhaseFir */

// Added from update.c

//DttSP_EXP 
void *
NewResamplerF (int samplerate_in, int samplerate_out)
{
  ResStF tmp;
  int lcm = 28224000, interpFactor, deciFactor;
  interpFactor = lcm / samplerate_in;
  deciFactor = lcm / samplerate_out;
  tmp = newPolyPhaseFIRF (32768, 0, interpFactor, 0, deciFactor);
  return (void *) tmp;
}

//DttSP_EXP 
void DelPolyPhaseFIRF (ResStF resst)
 {
//  extern void delPolyPhaseFIRF (ResSt resst);
  void delPolyPhaseFIRF (ResStF resst);
  delPolyPhaseFIRF (resst);
 }

//DttSP_EXP 
void
DoResamplerF (float *input, float *output, int numsamps, int *outsamps,
              ResStF ptr)
{
  ptr->input = input;
  ptr->output = output;
  ptr->inputArrayLength = numsamps;
  PolyPhaseFIRF (ptr);
  *outsamps = ptr->numOutputSamples;
}


