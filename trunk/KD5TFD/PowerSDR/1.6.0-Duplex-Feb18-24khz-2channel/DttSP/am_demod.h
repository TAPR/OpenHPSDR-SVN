/* am_demod.h */

#ifndef _am_demod_h
#define _am_demod_h

#include <fromsys.h>
#include <banal.h>
#include <splitfields.h>
#include <datatypes.h>
#include <bufvec.h>
#include <cxops.h>
#include <fastrig.h>
#include <update.h>
#include <lmadf.h>
#include <fftw3.h>
#include <ovsv.h>
#include <filter.h>
#include <oscillator.h>

typedef enum _ammode
{ AMdet, SAMdet } AMMode;
typedef struct _am_demod
{
  int size;
  CXB ibuf, obuf;

  struct
  {
    REAL alpha, beta, fast_alpha;
    struct
    {
      REAL f, l, h;
    } freq;
    REAL phs;
    struct
    {
      REAL alpha;
    } iir;
    COMPLEX delay;
  } pll;

  struct
  {
    REAL curr, prev;
  } lock;

  REAL dc;
  REAL smooth;
  AMMode mode;
} AMDDesc, *AMD;

extern void AMDemod (AMD am);
extern AMD newAMD (REAL samprate,
		   REAL f_initial,
		   REAL f_lobound,
		   REAL f_hibound,
		   REAL f_bandwid,
		   int size,
		   COMPLEX * ivec, COMPLEX * ovec, AMMode mode, char *tag);
extern void delAMD (AMD am);

#ifndef TWOPI
#define TWOPI (2.0*M_PI)
#endif

#endif
