/* keyd.c */
/*
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

//#include <linux/rtc.h>
#include <fromsys.h>
#include <banal.h>
#include <splitfields.h>
#include <datatypes.h>
#include <bufvec.h>
#include <cxops.h>
#include <ringb.h>
#include <oscillator.h>
#include <cwtones.h>
#include <pthread.h>
#include <semaphore.h>
#include <keyer.h>
#include <windows.h>
#include <MMsystem.h>
#include <spottone.h>
#include <sdrexport.h>
MMRESULT timerid = 0;

//PRIVATE CRITICAL_SECTION CS_CW;
//PRIVATE LPCRITICAL_SECTION cs_cw;

REAL SAMP_RATE = 48000.0;
// # times key is sampled per sec
// > 64 requires root on Linux
int key_poll_period = 1;
//#define RTC_RATE (64)
BOOLEAN HiPerformance = FALSE;

// # samples generated during 1 clock tick at RTC_RATE
//#define TONE_SIZE (SAMP_RATE / RTC_RATE)
unsigned int TONE_SIZE = 48;
unsigned int SIZEBUF = 1024;
// ring buffer size; > 1 sec at this sr
//#define RING_SIZE (1<<020)
//#define RING_SIZE (1<<017)
#define RING_SIZE 8192
//#define INTERLEAVED


KeyerState ks;
KeyerLogic kl;

static pthread_t play, key, timer;
sem_t clock_fired, keyer_started, poll_fired, update_ok;

ringb_float_t *lring, *rring;


CWToneGen gen;
static BOOLEAN playing = FALSE, iambic = FALSE, bug = FALSE, 
	cw_ring_reset = FALSE;
static REAL wpm = 18.0, freq = 600.0, ramp = 5.0, gain = 0.0;


//------------------------------------------------------------

#ifndef INTERLEAVED
DttSP_EXP void
CWtoneExchange (float *bufl, float *bufr, int nframes)
{
  size_t numsamps, bytesize = sizeof (float) * nframes;
  if (cw_ring_reset)
    {
	  size_t reset_size = max(SIZEBUF,nframes);
      cw_ring_reset = FALSE;
      ringb_float_restart (lring, reset_size);
      ringb_float_restart (rring, reset_size);
      memset (bufl, 0, nframes * sizeof (REAL));
      memset (bufr, 0, nframes * sizeof (REAL));
      return;
    }
  if ((numsamps = ringb_float_read_space (lring)) < (size_t) nframes)
    {
      memset (bufl, 0, bytesize);
      memset (bufr, 0, bytesize);
      if (numsamps != 0)
	{
	  ringb_float_read (lring, bufl, nframes - numsamps);
	  ringb_float_read (rring, bufr, nframes - numsamps);
	}
    }
  else
    {
      ringb_float_read (lring, bufl, nframes);
      ringb_float_read (rring, bufr, nframes);
    }
}

// generated tone -> output ringbuffer
void
send_tone (void)
{
  if (ringb_float_write_space (lring) < TONE_SIZE)
    {
      cw_ring_reset = TRUE;
    }
  else
    {
      int i;
      //EnterCriticalSection (cs_cw);
      for (i = 0; i < gen->size; i++)
	{
	  float l = (float) CXBreal (gen->buf, i),
	    r = (float) CXBimag (gen->buf, i);
	  ringb_float_write (lring, (float *) &l, 1);
	  ringb_float_write (rring, (float *) &r, 1);
	}
      //LeaveCriticalSection (cs_cw);
    }
}

// silence -> output ringbuffer
void
send_silence (void)
{
  if (ringb_float_write_space (lring) < TONE_SIZE)
    {
      cw_ring_reset = TRUE;
    }
  else
    {
      int i;
      //EnterCriticalSection (cs_cw);
      for (i = 0; i < gen->size; i++)
	{
	  float zero = 0.0;
	  ringb_float_write (lring, &zero, 1);
	  ringb_float_write (rring, &zero, 1);
	}
      //LeaveCriticalSection (cs_cw);
    }
}

//------------------------------------------------------------------------


#else
DttSP_EXP void
CWtoneExchange (float *bufl, int nframes, int numchans)
{
  size_t numframes, bytesize = sizeof (float) * nframes * numchans;
  float *locbuf = bufl;
  if ((numframes =
       ringb_float_read_space (lring)) < (size_t) numchans * nframes)
    {
      memset (bufl, 0, bytesize);
      if (numframes != 0)
	{
	  int i, j;
	  numframes /= numchans;
	  //EnterCriticalSection (cs_cw);
	  for (i = 0; i < nframes - (int) numframes; i++)
	    {
	      float l, r;
	      ringb_float_read (lring, &l, 1);
	      ringb_float_read (lring, &r, 1);
	      for (j = 0; j < numchans; j += 2, locbuf += 2)
		{
		  locbuf[0] = l;
		  locbuf[1] = r;
		}
	    }
	  //LeaveCriticalSection (cs_cw);
	}
    }
  else
    {
      int i, j;
      //EnterCriticalSection (cs_cw);
      for (i = 0; i < nframes; i++)
	{
	  float l, r;
	  ringb_float_read (lring, &l, 1);
	  ringb_float_read (lring, &r, 1);
	  for (j = 0; j < numchans; j += 2, locbuf += 2)
	    {
	      locbuf[0] = l;
	      locbuf[1] = r;
	    }
	}
      //LeaveCriticalSection (cs_cw);
    }
}


// generated tone -> output ringbuffer
void
send_tone (void)
{
  if (ringb_float_write_space (lring) < 2 * TONE_SIZE)
    {
      //write(2, "overrun tone\n", 13);
      //EnterCriticalSection (cs_cw);
      ringb_float_restart (lring, SIZEBUF);
      //LeaveCriticalSection (cs_cw);
    }
  else
    {
      //EnterCriticalSection (cs_cw);
      ringb_float_write (lring, (float *) CXBbase (gen->buf), 2 * gen->size);
      //LeaveCriticalSection (cs_cw);
    }
}

// silence -> output ringbuffer
void
send_silence (void)
{
  if (ringb_float_write_space (lring) < 2 * TONE_SIZE)
    {
      //write(2, "overrun zero\n", 13);
      //EnterCriticalSection (cs_cw);
      ringb_float_restart (lring, SIZEBUF);
      //LeaveCriticalSection (cs_cw);
    }
  else
    {
      int i;
      //EnterCriticalSection (cs_cw);
      for (i = 0; i < 2 * gen->size; i++)
	{
	  float zero = 0.0;
	  ringb_float_write (lring, &zero, 1);
	}
      //LeaveCriticalSection (cs_cw);
    }
}

//------------------------------------------------------------------------

#endif
//------------------------------------------------------------

// sound/silence generation
// tone turned on/off asynchronously

CALLBACK
timer_callback (UINT uTimerID, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dw1,
		DWORD_PTR dw2)
{
  sem_post (&poll_fired);
}



DttSP_EXP void
sound_thread_keyd (void)
{
  for (;;)
    {
      sem_wait (&clock_fired);

      if (playing)
	{

	  // CWTone keeps playing for awhile after it's turned off,
	  // in order to allow for a decay envelope;
	  // returns FALSE when it's actually done.
	  playing = CWTone (gen);
	  sem_wait (&update_ok);
	  send_tone ();
	  sem_post (&update_ok);
	}
      else
	{
	  sem_wait (&update_ok);
	  send_silence ();
	  // only let updates run when we've just generated silence
	  sem_post (&update_ok);
	}
    }

  pthread_exit (0);
}


BOOLEAN
read_key (REAL del, BOOLEAN dot, BOOLEAN dash)
{
  extern BOOLEAN read_straight_key (KeyerState ks, BOOLEAN keyed);
  extern BOOLEAN read_iambic_key (KeyerState ks, BOOLEAN dot,
				  BOOLEAN dash, KeyerLogic kl, REAL ticklen);


  if (bug)
    {
      if (dash)
	return read_straight_key (ks, dash);
      else
	return read_iambic_key (ks, dot, FALSE, kl, del);
    }
  if (iambic)
    return read_iambic_key (ks, dot, dash, kl, del);
  return read_straight_key (ks, dot | dash);
}

/// Main keyer function,  called by a thread in the C#
BOOLEAN dotkey = FALSE;
PRIVATE BOOLEAN __inline
whichkey (BOOLEAN dot, BOOLEAN dash)
{
  if (dotkey)
    return dot;
  return dash;
}
DttSP_EXP void
SetWhichKey (BOOLEAN isdot)
{
  if (isdot)
    dotkey = TRUE;
  else
    dotkey = FALSE;
}
DttSP_EXP void
key_thread_process (REAL del, BOOLEAN dash, BOOLEAN dot, BOOLEAN keyprog)
{
  BOOLEAN keydown;
  extern BOOLEAN read_straight_key (KeyerState ks, BOOLEAN keyed);
  // read key; tell keyer elapsed time since last call
  if (!keyprog)
    keydown = read_key (del, dot, dash);
  else
    keydown = read_straight_key (ks, whichkey (dot, dash));


  if (!playing && keydown)
    CWToneOn (gen), playing = TRUE;
  else if (playing && !keydown)
    CWToneOff (gen);

  sem_post (&clock_fired);
}

DttSP_EXP BOOLEAN
KeyerPlaying ()
{
  return playing;
}

//------------------------------------------------------------------------


DttSP_EXP void
SetKeyerBug (BOOLEAN bg)
{
  sem_wait (&update_ok);
  if (bg)
    {
      iambic = FALSE;
      ks->flag.mdlmdB = FALSE;
      ks->flag.memory.dah = FALSE;
      ks->flag.memory.dit = FALSE;
      bug = TRUE;
    }
  else
    bug = FALSE;
  sem_post (&update_ok);

}
DttSP_EXP void
SetKeyerSpeed (REAL speed)
{
  sem_wait (&update_ok);
  wpm = ks->wpm = speed;
  sem_post (&update_ok);
}
DttSP_EXP void
SetKeyerWeight (int newweight)
{
  sem_wait (&update_ok);
  ks->weight = newweight;
  sem_post (&update_ok);
}
DttSP_EXP void
SetKeyerIambic (BOOLEAN setit)
{
  sem_wait (&update_ok);
  if (setit)
    {
      iambic = TRUE;
      ks->flag.mdlmdB = TRUE;
      ks->flag.memory.dah = TRUE;
      ks->flag.memory.dit = TRUE;
    }
  else
    {
      iambic = FALSE;
      ks->flag.mdlmdB = FALSE;
      ks->flag.memory.dah = FALSE;
      ks->flag.memory.dit = FALSE;
    }
  sem_post (&update_ok);
}
DttSP_EXP void
SetKeyerFreq (REAL newfreq)
{
  sem_wait (&update_ok);
  freq = newfreq;
  setCWToneGenVals (gen, gain, freq, ramp, ramp);
  sem_post (&update_ok);
}
DttSP_EXP void
SetKeyerGain (REAL newgain)
{
  if ((newgain >= 0.0) && (newgain <= 1.0))
    {
      sem_wait (&update_ok);
      gain = (REAL) (20.0 * log10 (newgain));
      setCWToneGenVals (gen, gain, freq, ramp, ramp);
      sem_post (&update_ok);
    }

}
DttSP_EXP void
SetKeyerRamp (REAL newramp)
{
  sem_wait (&update_ok);
  ramp = newramp;
  setCWToneGenVals (gen, gain, freq, ramp, ramp);
  sem_post (&update_ok);
}
DttSP_EXP void
SetKeyerMode (int newmode)
{
  sem_wait (&update_ok);
  if (newmode == 1)
    ks->mode = MODE_B;
  if (newmode == 0)
    ks->mode = MODE_A;
  if (newmode == 2)
    iambic = FALSE;
  sem_post (&update_ok);
}

DttSP_EXP void
SetKeyerDeBounce (int db)
{
  sem_wait (&update_ok);
  ks->debounce = db;
  sem_post (&update_ok);
}

DttSP_EXP void
SetKeyerRevPdl (BOOLEAN rvp)
{
  sem_wait (&update_ok);
  ks->flag.revpdl = !rvp;
  sem_post (&update_ok);
}

/*updateKeyer(REAL nfreq, BOOLEAN niambic, REAL ngain, REAL nramp, REAL nwpm,
			BOOLEAN revpdl, int weight, REAL SampleRate) {
	ks->flag.iambic = niambic;
	iambic = niambic;
	ks->flag.revpdl = revpdl;
	ks->weight = weight;
	wpm = nwpm;
	gain = ngain;
	ramp = nramp;
	freq = nfreq;
	gen->osc.freq = 2.0 * M_PI * freq / SampleRate;
} */
DttSP_EXP void
SetKeyerPerf (BOOLEAN hiperf)
{
  MMRESULT tmp_timer;
  tmp_timer = timerid;
  if (timerid != 0)
    {
      sem_wait (&update_ok);
      timeKillEvent ((UINT) timerid);
      timerid = 0;
      Sleep (11);
    }
  delCWToneGen (gen);
  if (hiperf)
    {
      HiPerformance = TRUE;
      key_poll_period = 1;
      TONE_SIZE = 48;
    }
  else
    {
      HiPerformance = FALSE;
      key_poll_period = 5;
      TONE_SIZE = 240;
    }
  gen = newCWToneGen (gain, freq, ramp, ramp, TONE_SIZE, SAMP_RATE);
  if (tmp_timer != 0)
    {
#ifndef INTERLEAVED
      ringb_float_restart (lring, SIZEBUF);
      ringb_float_restart (rring, SIZEBUF);
#else
      ringb_float_restart (lring, SIZEBUF);
#endif
      if ((timerid =
	   timeSetEvent (key_poll_period, 1,
			 (LPTIMECALLBACK) timer_callback,
			 (DWORD_PTR) NULL, TIME_PERIODIC)) == (MMRESULT) NULL)
	fprintf (stderr, "Timer failed\n"), fflush (stderr);
      sem_post (&update_ok);
    }
}
DttSP_EXP void
NewKeyer (REAL freq, BOOLEAN niambic, REAL gain, REAL ramp, REAL wpm,
	  REAL SampleRate)
{


  kl = newKeyerLogic ();
  ks = newKeyerState ();
  ks->flag.iambic = niambic;
  ks->flag.revpdl = TRUE;	// depends on port wiring
  ks->flag.autospace.khar = ks->flag.autospace.word = FALSE;
  ks->flag.mdlmdB = TRUE;
  ks->flag.memory.dah = TRUE;
  ks->flag.memory.dit = TRUE;
  ks->debounce = 1;		// could be more if sampled faster
  ks->mode = MODE_B;
  ks->weight = 50;
  ks->wpm = wpm;
  iambic = niambic;
#ifndef INTERLEAVED
  lring = ringb_float_create (RING_SIZE);
  rring = ringb_float_create (RING_SIZE);
#else
  lring = ringb_float_create (2 * RING_SIZE);
#endif
  sem_init (&clock_fired, 0, 0);
  sem_init (&poll_fired, 0, 0);
  sem_init (&keyer_started, 0, 0);
  sem_init (&update_ok, 0, 0);
  sem_post (&update_ok);
  //cs_cw = &CS_CW;
  //InitializeCriticalSection (cs_cw);
  if (HiPerformance)
    {
      key_poll_period = 1;
      TONE_SIZE = 48 * (int) (uni.samplerate / 48000.0);
    }
  else
    {
      key_poll_period = 5;
      TONE_SIZE = 240 * (int) (uni.samplerate / 48000.0);
    }
  //------------------------------------------------------------

  gen = newCWToneGen (gain, freq, ramp, ramp, TONE_SIZE, SampleRate);

  //------------------------------------------------------------
//  if (timeSetEvent(5,1,(LPTIMECALLBACK)timer_callback,(DWORD_PTR)NULL,TIME_PERIODIC) == (MMRESULT)NULL) {
//        fprintf(stderr,"Timer failed\n"),fflush(stderr);
//  }
}
DttSP_EXP void
StartKeyer ()
{
#ifndef INTERLEAVED
  ringb_float_restart (lring, SIZEBUF);
  ringb_float_restart (rring, SIZEBUF);
#else
  ringb_float_restart (lring, SIZEBUF);
#endif
  if ((timerid =
       timeSetEvent (key_poll_period, 1, (LPTIMECALLBACK) timer_callback,
		     (DWORD_PTR) NULL, TIME_PERIODIC)) == (MMRESULT) NULL)
    fprintf (stderr, "Timer failed\n"), fflush (stderr);
  sem_post (&keyer_started);
}
DttSP_EXP void
StopKeyer ()
{
  sem_wait (&update_ok);
  if (timerid)
    timeKillEvent ((UINT) timerid);
  sem_post (&update_ok);
  timerid = 0;
}

DttSP_EXP BOOLEAN
KeyerRunning ()
{
  return (timerid != 0);
}

DttSP_EXP void
DeleteKeyer ()
{
  if (clock_fired)
    {
      sem_destroy (&clock_fired);
      clock_fired = NULL;
    }
  if (poll_fired)
    {
      sem_destroy (&poll_fired);
      poll_fired = NULL;
    }
  if (keyer_started)
    {
      sem_destroy (&keyer_started);
      keyer_started = NULL;
    }
  delCWToneGen (gen);
  delKeyerState (ks);
  delKeyerLogic (kl);
#ifndef INTERLEAVED
  ringb_float_free (lring);
  ringb_float_free (rring);
#else
  ringb_float_free (lring);
#endif
  /* if (cs_cw)
     {
     DeleteCriticalSection (cs_cw);
     cs_cw = NULL;
     } */
}
DttSP_EXP void
KeyerClockFireWait ()
{
  sem_wait (&clock_fired);
}
DttSP_EXP void
KeyerClockFireRelease ()
{
  sem_post (&clock_fired);
}
DttSP_EXP void
KeyerStartedWait ()
{
  sem_wait (&keyer_started);
}
DttSP_EXP void
KeyerStartedRelease ()
{
  sem_post (&keyer_started);
}
DttSP_EXP void
PollTimerWait ()
{
  sem_wait (&poll_fired);
}
DttSP_EXP void
PollTimerRelease ()
{
  sem_post (&poll_fired);
}
DttSP_EXP void
SetKeyerResetSize (unsigned int sizer)
{
  SIZEBUF = sizer;
  cw_ring_reset = TRUE;
}

DttSP_EXP void
CWRingRestart ()
{
  cw_ring_reset = TRUE;
}

DttSP_EXP void
SetKeyerSampleRate (REAL sr)
{
  int factor = (int) (sr / 48000.0f);
  if (HiPerformance)
    {
      key_poll_period = 1;
      TONE_SIZE = 48 * factor;
    }
  else
    {
      key_poll_period = 5;
      TONE_SIZE = 240 * factor;
    }
  SIZEBUF = 512 * factor;

  delCWToneGen (gen);
  gen = newCWToneGen (gain, freq, ramp, ramp, TONE_SIZE, sr);
}

//------------------------------------------------------------------------
