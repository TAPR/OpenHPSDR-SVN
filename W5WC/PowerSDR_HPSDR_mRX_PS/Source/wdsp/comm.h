/*  comm.h

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2013 Warren Pratt, NR0V

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

The author can be reached by email at  

warren@wpratt.com

*/

#include <Windows.h>
#include <process.h>
#include <intrin.h>
#include <math.h>
#include <time.h>
#include "fftw3.h"

#include "amd.h"
#include "ammod.h"
#include "amsq.h"
#include "anf.h"
#include "anr.h"
#include "bandpass.h"
#include "calcc.h"
#include "cblock.h"
#include "channel.h"
#include "compress.h"
#include "delay.h"
#include "eq.h"
#include "fcurve.h"
#include "fir.h"
#include "fmd.h"
#include "fmmod.h"
#include "fmsq.h"
#include "gain.h"
#include "gen.h"
#include "iobuffs.h"
#include "iqc.h"
#include "main.h"
#include "meter.h"
#include "meterlog10.h"
#include "patchpanel.h"
#include "resample.h"
#include "RXA.h"
#include "sender.h"
#include "shift.h"
#include "siphon.h"
#include "slew.h"
#include "TXA.h"
#include "utilities.h"
#include "wcpAGC.h"

#define DSP_MULT						2					// number of dsp_buffsizes that are held in an iobuff pseudo-ring
#define INREAL							float				// data type for channel input buffer
#define OUTREAL							float				// data type for channel output buffer
#define MAX_CHANNELS					32					// maximum number of supported channels

#define PI								3.1415926535897932
#define TWOPI							6.2831853071795864

typedef double complex[2];

#define PORT		__declspec( dllexport )
