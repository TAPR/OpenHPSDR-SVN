/* winmain.c

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2004, 2005, 2006-5 by Frank Brickle, AB2KT and Bob McGwier, N4HY

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

#include <common.h>
/////////////////////////////////////////////////////////////////////////

// elementary defaults
struct _loc loc;

/////////////////////////////////////////////////////////////////////////
// most of what little we know here about the inner loop,
// functionally speaking

extern void reset_meters (void);
extern void reset_spectrum (void);
extern void reset_counters (void);
extern void process_samples (float *, float *, float *, float *, int);
extern void setup_workspace (REAL rate,
			     int buflen,
			     SDRMODE mode,
			     char *wisdom,
			     int specsize, int numrecv, int cpdsize);
extern void destroy_workspace (void);

//========================================================================
#if 0

PRIVATE void
spectrum_thread (void)
{
	DWORD NumBytesWritten;
	while (top.running)
    {
		sem_wait (&top.sync.pws.sem);
		sem_wait (&top.sync.upd.sem);
		compute_spectrum (&uni.spec);
		WriteFile (top.meas.spec.fd, (LPVOID) & uni.spec.label,
			sizeof (int), &NumBytesWritten, NULL);
		WriteFile (top.meas.spec.fd, (LPVOID) uni.spec.output,
			sizeof (float) * uni.spec.size, &NumBytesWritten, NULL);
		sem_post (&top.sync.upd.sem);
    }
	pthread_exit (0);
}

PRIVATE void
meter_thread (void)
{
	DWORD NumBytesWritten;
	while (top.running)
	{
		sem_wait (&top.sync.mtr.sem);
		sem_wait (&top.sync.upd.sem);
		WriteFile (top.meas.mtr.fd, (LPVOID) & uni.meter.label, sizeof (int),
			&NumBytesWritten, NULL);
		WriteFile (top.meas.mtr.fd, (LPVOID) & uni.meter.snap.rx,
			sizeof (REAL) * MAXRX * RXMETERPTS, &NumBytesWritten, NULL);
		WriteFile (top.meas.mtr.fd, (LPVOID) & uni.meter.snap.tx,
			sizeof (REAL) * TXMETERPTS, &NumBytesWritten, NULL);
		sem_post (&top.sync.upd.sem);
    }
	pthread_exit (0);
}
#endif
//========================================================================

PRIVATE void
monitor_thread (void)
{
	while (top.running)
	{
		sem_wait (&top.sync.mon.sem);
		/* If there is anything that needs monitoring, do it here */
		fprintf (stderr,
			"@@@ mon [%d]: cb = %d rbi = %d rbo = %d xr = %d\n",
			uni.tick,
			top.jack.blow.cb,
			top.jack.blow.rb.i, top.jack.blow.rb.o, top.jack.blow.xr);
		fflush (stderr);
		memset ((char *) &top.jack.blow, 0, sizeof (top.jack.blow));
	}
	pthread_exit (0);
}

//========================================================================

PRIVATE void
process_updates_thread (void)
{
	while (top.running)
	{
		DWORD NumBytesRead;
		//pthread_testcancel();
		while (ReadFile (top.parm.fd, top.parm.buff, 4096, &NumBytesRead, NULL))
		{
			top.parm.buff[NumBytesRead] = 0;

			if (NumBytesRead != 0)
				do_update (top.parm.buff, top.verbose ? stderr : 0);
		}
	}
	pthread_exit (0);
}

//========================================================================

BOOLEAN reset_em;

PRIVATE void
gethold (void)
{
	if (ringb_float_write_space (top.jack.ring.o.l) < top.hold.size.frames)
	{
		// pathology
		reset_em = TRUE;
		top.jack.blow.rb.o++;
	}
	else
	{
		ringb_float_write (top.jack.ring.o.l, top.hold.buf.l,
			top.hold.size.frames);
		ringb_float_write (top.jack.ring.o.r, top.hold.buf.r,
			top.hold.size.frames);
#ifdef USE_AUXILIARY
		ringb_float_write (top.jack.auxr.o.l, top.hold.aux.l,
			top.hold.size.frames);
		ringb_float_write (top.jack.auxr.o.r, top.hold.aux.r,
			top.hold.size.frames);
#else
		ringb_float_write (top.jack.auxr.o.l, top.hold.buf.l,
			top.hold.size.frames);
		ringb_float_write (top.jack.auxr.o.r, top.hold.buf.r,
			top.hold.size.frames);
    }
#endif
	if (ringb_float_read_space (top.jack.ring.i.l) < top.hold.size.frames)
	{
		// pathology
		memset ((char *) top.hold.buf.l, 0, top.hold.size.bytes);
		memset ((char *) top.hold.buf.r, 0, top.hold.size.bytes);
		memset ((char *) top.hold.aux.l, 0, top.hold.size.bytes);
		memset ((char *) top.hold.aux.r, 0, top.hold.size.bytes);
		reset_em = TRUE;
		top.jack.blow.rb.i++;
	}
	else
	{
		ringb_float_read (top.jack.ring.i.l,
			top.hold.buf.l, top.hold.size.frames);
		ringb_float_read (top.jack.ring.i.r,
			top.hold.buf.r, top.hold.size.frames);
#ifdef USE_AUXILIARY
		ringb_float_read (top.jack.auxr.i.l,
			top.hold.aux.l, top.hold.size.frames);
		ringb_float_read (top.jack.auxr.i.r,
			top.hold.aux.r, top.hold.size.frames);
#else
		ringb_float_read (top.jack.auxr.i.l,
			top.hold.buf.l, top.hold.size.frames);
		ringb_float_read (top.jack.auxr.i.r,
			top.hold.buf.r, top.hold.size.frames);
#endif
    }
}

PRIVATE BOOLEAN
canhold (void)
{
	return (ringb_float_read_space (top.jack.ring.i.l) >=
		(size_t) top.hold.size.frames);
}


//------------------------------------------------------------------------

PRIVATE void
run_mute (void)
{
	memset ((char *) top.hold.buf.l, 0, top.hold.size.bytes);
	memset ((char *) top.hold.buf.r, 0, top.hold.size.bytes);
	memset ((char *) top.hold.aux.l, 0, top.hold.size.bytes);
	memset ((char *) top.hold.aux.r, 0, top.hold.size.bytes);
	uni.tick++;
}

PRIVATE void
run_pass (void)
{
	uni.tick++;
}

PRIVATE void
run_play (void)
{
	process_samples (top.hold.buf.l, top.hold.buf.r,
		top.hold.aux.l, top.hold.aux.r, top.hold.size.frames);
}

// NB do not set RUN_SWCH directly via setRunState;
// use setSWCH instead


PRIVATE void
run_swch(void)
{
	int i, n = top.hold.size.frames;
	REAL w;
//	static int count = 0;

	process_samples(top.hold.buf.l, top.hold.buf.r,
		top.hold.aux.l, top.hold.aux.r,
		top.hold.size.frames);

	for (i = 0; i < n; i++)
	{
//		count++;
		if (top.swch.env.curr.type == SWCH_FALL)
		{
			top.swch.env.curr.val += top.swch.env.fall.incr;
			w = (REAL)sin(top.swch.env.curr.val * M_PI /  2.0f);
			top.hold.buf.l[i] *= w, top.hold.buf.r[i] *= w;
			top.hold.aux.l[i] *= w, top.hold.aux.r[i] *= w;
//			if (top.swch.env.curr.cnt == 0) fprintf(stderr, "FALL\n"),fflush(stderr);
//			if(top.swch.env.curr.cnt == 0) top.hold.buf.l[i] = top.hold.buf.r[i] = -1.0;
			if (++top.swch.env.curr.cnt >= top.swch.env.fall.size)
			{
				//top.hold.buf.l[i] = top.hold.buf.r[i] = -1.0;
				top.swch.env.curr.type = SWCH_STDY;
				top.swch.env.curr.cnt = 0;
				top.swch.env.curr.val = 0.0;
//				fprintf(stderr, "Fall End: %d\n", count);
			}
		}
		else if (top.swch.env.curr.type == SWCH_STDY)
		{
			top.hold.buf.l[i]= top.hold.buf.r[i] =
				top.hold.aux.l[i] =  top.hold.aux.r[i] = 0.0;
//			if (top.swch.env.curr.cnt == 0) fprintf(stderr, "STDY\n"),fflush(stderr);
			if (++top.swch.env.curr.cnt >= top.swch.env.stdy.size)
			{
//				top.hold.buf.l[i] = top.hold.buf.r[i] = -1.0;
				top.swch.env.curr.type = SWCH_RISE;
				top.swch.env.curr.cnt = 0;
				top.swch.env.curr.val = 0.0;
//				fprintf(stderr, "Stdy End: %d\n", count);
			}
		}
		else if (top.swch.env.curr.type == SWCH_RISE)
		{
			top.swch.env.curr.val += top.swch.env.rise.incr;
			w = (REAL)sin(top.swch.env.curr.val * M_PI /  2.0f);
			top.hold.buf.l[i] *= w, top.hold.buf.r[i] *= w;
			top.hold.aux.l[i] *= w, top.hold.aux.r[i] *= w;
//			if (top.swch.env.curr.cnt == 0) fprintf(stderr, "RISE\n"),fflush(stderr);
			if (++top.swch.env.curr.cnt >= top.swch.env.rise.size)
			{
//				reset_meters();
//				reset_spectrum();
//				reset_counters();
	
				uni.mode.trx = top.swch.trx.next;
				top.state = top.swch.run.last;
				break;
//				fprintf(stderr, "Rise End: %d\n", count);
			}
		}
	}
}

/*
PRIVATE void
run_swch (void)
{
	if (top.swch.bfct.have == 0)
	{
		// first time
		// apply ramp down
		int i, m = top.swch.fade, n = top.swch.tail;
		for (i = 0; i < m; i++)
		{
			float w = (float) 1.0 - (float) i / (float) m;
			top.hold.buf.l[i] *= w, top.hold.buf.r[i] *= w;
		}
		memset ((char *) (top.hold.buf.l + m), 0, n);
		memset ((char *) (top.hold.buf.r + m), 0, n);
		top.swch.bfct.have++;
	}
	else if (top.swch.bfct.have < top.swch.bfct.want)
	{
		// in medias res
		memset ((char *) top.hold.buf.l, 0, top.hold.size.bytes);
		memset ((char *) top.hold.buf.r, 0, top.hold.size.bytes);
		top.swch.bfct.have++;
	}
	else
	{
		// last time
		// apply ramp up
		int i, m = top.swch.fade, n = top.swch.tail;
		for (i = 0; i < m; i++)
		{
			float w = (float) i / m;
			top.hold.buf.l[i] *= w, top.hold.buf.r[i] *= w;
		}
		uni.mode.trx = top.swch.trx.next;
		top.state = top.swch.run.last;
		top.swch.bfct.want = top.swch.bfct.have = 0;
	}

	process_samples (top.hold.buf.l, top.hold.buf.r,
		top.hold.aux.l, top.hold.aux.r, top.hold.size.frames);

}
*/
//========================================================================

static void reset_system_audio(size_t nframes)
{
	size_t reset_size = max (top.jack.reset_size, nframes);
	const float zero = 0.;
	int i;

	reset_em = FALSE;

	ringb_float_reset (top.jack.ring.i.l);
	ringb_float_reset (top.jack.ring.i.r);
	ringb_float_reset (top.jack.auxr.i.l);
	ringb_float_reset (top.jack.auxr.i.r);
	
	if (top.offset < 0)
	{
		for(i=top.offset;i<0;i++)
		{
			ringb_float_write(top.jack.ring.i.l,&zero,1);
			ringb_float_write(top.jack.auxr.i.l,&zero,1);
		}
	}
	else
	{
		for(i=0;i<top.offset;i++)
		{
			ringb_float_write(top.jack.ring.i.r,&zero,1);
			ringb_float_write(top.jack.auxr.i.r,&zero,1);
		}
	}

	ringb_float_restart (top.jack.ring.o.r, reset_size);
	ringb_float_restart (top.jack.ring.o.l, reset_size);
	ringb_float_restart (top.jack.auxr.o.r, reset_size);
	ringb_float_restart (top.jack.auxr.o.l, reset_size);	
}

DttSP_EXP void
Audio_Callback (float *input_l, float *input_r, float *output_l,
	float *output_r, unsigned int nframes)
{
	BOOLEAN b = reset_em;
	if (top.susp)
	{
		memset (output_l, 0, nframes * sizeof (float));
		memset (output_r, 0, nframes * sizeof (float));
		return;
	}

	if (reset_em)
	{
		reset_system_audio(nframes);
		memset (output_l, 0, nframes * sizeof (float));
		memset (output_r, 0, nframes * sizeof (float));
    }

	if ((ringb_float_read_space (top.jack.ring.o.l) >= nframes)
		&& (ringb_float_read_space (top.jack.ring.o.r) >= nframes))
	{
		ringb_float_read (top.jack.auxr.o.l, output_l, nframes);
		ringb_float_read (top.jack.auxr.o.r, output_r, nframes);
		ringb_float_read (top.jack.ring.o.l, output_l, nframes);
		ringb_float_read (top.jack.ring.o.r, output_r, nframes);
	}
	else
	{	// rb pathology
		reset_system_audio(nframes);
		memset (output_l, 0, nframes * sizeof (float));
		memset (output_r, 0, nframes * sizeof (float));
		top.jack.blow.rb.o++;
	}

	// input: copy from port to ring
	if ((ringb_float_write_space (top.jack.ring.i.l) >= nframes)
		&& (ringb_float_write_space (top.jack.ring.i.r) >= nframes))
	{
		ringb_float_write (top.jack.ring.i.l, (float *) input_l, nframes);
		ringb_float_write (top.jack.ring.i.r, (float *) input_r, nframes);
		ringb_float_write (top.jack.auxr.i.l, (float *) input_l, nframes);
		ringb_float_write (top.jack.auxr.i.r, (float *) input_r, nframes);
    }
	else
	{	// rb pathology
		reset_system_audio(nframes);
		top.jack.blow.rb.i++;
	}

	// if enough accumulated in ring, fire dsp
	if (ringb_float_read_space (top.jack.ring.i.l) >= top.hold.size.frames)
		sem_post (&top.sync.buf.sem);

	// check for blowups
	if ((top.jack.blow.cb > 0) ||
		(top.jack.blow.rb.i > 0) || (top.jack.blow.rb.o > 0))
		sem_post (&top.sync.mon.sem);
}


//========================================================================


DttSP_EXP void
process_samples_thread (void)
{
	while (top.running)
	{
		sem_wait (&top.sync.buf.sem);
		do
		{
			gethold ();
			sem_wait (&top.sync.upd.sem);
			switch (top.state)
			{
				case RUN_MUTE:
					run_mute ();
					break;
				case RUN_PASS:
					run_pass ();
					break;
				case RUN_PLAY:
					run_play ();
					break;
				case RUN_SWCH:
					run_swch ();
					break;
			}
			sem_post (&top.sync.upd.sem);
		}
		while (canhold ());
    }
}


void
closeup (void)
{
	top.running = FALSE;
	top.susp = TRUE;
	Sleep (96);
	ringb_float_free (top.jack.auxr.i.l);
	ringb_float_free (top.jack.auxr.i.r);
	ringb_float_free (top.jack.auxr.o.l);
	ringb_float_free (top.jack.auxr.o.r);

	ringb_float_free (top.jack.ring.o.r);
	ringb_float_free (top.jack.ring.o.l);
	ringb_float_free (top.jack.ring.i.r);
	ringb_float_free (top.jack.ring.i.l);

	CloseHandle (top.parm.fp);
	DisconnectNamedPipe (top.parm.fd);
	CloseHandle (top.parm.fd);


	if (uni.meter.flag)
	{
		CloseHandle (top.meas.mtr.fp);
		DisconnectNamedPipe (top.meas.mtr.fd);
		CloseHandle (top.meas.mtr.fd);
	}

	if (uni.spec.flag)
	{
		CloseHandle (top.meas.spec.fp);
		DisconnectNamedPipe (top.meas.spec.fd);
		CloseHandle (top.meas.spec.fd);
    }
	destroy_workspace ();
}

//........................................................................

/*PRIVATE void
setup_switching (void)
{
  top.swch.fade = (int) (0.2 * uni.buflen + 0.5);
  top.swch.tail = (top.hold.size.frames - top.swch.fade);
}*/

PRIVATE void
setup_local_audio (void)
{
	top.hold.size.frames = uni.buflen;
	top.hold.size.bytes = top.hold.size.frames * sizeof (float);
	top.hold.buf.l =
		(float *) safealloc (top.hold.size.frames, sizeof (float),
		"main hold buffer left");
	top.hold.buf.r =
		(float *) safealloc (top.hold.size.frames, sizeof (float),
		"main hold buffer right");
	top.hold.aux.l =
		(float *) safealloc (top.hold.size.frames, sizeof (float),
		"aux hold buffer left");
	top.hold.aux.r =
	(float *) safealloc (top.hold.size.frames, sizeof (float),
		"aux hold buffer right");
}

#include <lmerr.h>

PRIVATE void
DisplayErrorText (DWORD dwLastError)
{
  HMODULE hModule = NULL;	// default to system source
  LPSTR MessageBuffer;
  DWORD dwBufferLength;

  DWORD dwFormatFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER |
    FORMAT_MESSAGE_IGNORE_INSERTS | FORMAT_MESSAGE_FROM_SYSTEM;

  //
  // If dwLastError is in the network range, 
  //  load the message source.
  //

  if (dwLastError >= NERR_BASE && dwLastError <= MAX_NERR)
    {
      hModule = LoadLibraryEx (TEXT ("netmsg.dll"),
			       NULL, LOAD_LIBRARY_AS_DATAFILE);

      if (hModule != NULL)
	dwFormatFlags |= FORMAT_MESSAGE_FROM_HMODULE;
    }
  //
  // Call FormatMessage() to allow for message 
  //  text to be acquired from the system 
  //  or from the supplied module handle.
  //

  if (dwBufferLength = FormatMessageA (dwFormatFlags, hModule,	// module to get message from (NULL == system)
				       dwLastError, MAKELANGID (LANG_NEUTRAL, SUBLANG_DEFAULT),	// default language
				       (LPSTR) & MessageBuffer, 0, NULL))
    {
      DWORD dwBytesWritten;

      //
      // Output message string on stderr.
      //
      WriteFile (GetStdHandle (STD_ERROR_HANDLE),
		 MessageBuffer, dwBufferLength, &dwBytesWritten, NULL);

      //
      // Free the buffer allocated by the system.
      //
      LocalFree (MessageBuffer);
    }
  //
  // If we loaded a message source, unload it.
  //
  if (hModule != NULL)
    FreeLibrary (hModule);
}



PRIVATE sem_t setup_update_sem;

PRIVATE void
setup_update_server ()
{

  if (INVALID_HANDLE_VALUE ==
      (top.parm.fd =
       CreateNamedPipe (top.parm.path, PIPE_ACCESS_INBOUND,
			PIPE_WAIT | PIPE_TYPE_MESSAGE |
			PIPE_READMODE_MESSAGE, PIPE_UNLIMITED_INSTANCES,
			512, 512, INFINITE, NULL)))
    {
      fprintf (stderr, "Update server pipe setup failed:\n"), fflush (stderr);
      DisplayErrorText (GetLastError ());
    }

  sem_post (&setup_update_sem);
  if (ConnectNamedPipe (top.parm.fd, NULL))
    {

    }
  else
    {
      fprintf (stderr,
	       "Connected the server to the Update pipe failed\n"),
	fflush (stderr);
      DisplayErrorText (GetLastError ());
    }
  pthread_exit (0);
}


PRIVATE void
setup_update_client ()
{

  WaitNamedPipe (top.parm.path, NMPWAIT_USE_DEFAULT_WAIT);

  if (INVALID_HANDLE_VALUE == (top.parm.fp = CreateFile (top.parm.path,
							 GENERIC_WRITE, 0,
							 NULL,
							 OPEN_EXISTING,
							 FILE_ATTRIBUTE_NORMAL,
							 NULL)))
    {
      fprintf (stderr, "The Update Client Open Failed\n"), fflush (stderr);
      DisplayErrorText (GetLastError ());
    }
  sem_post (&setup_update_sem);
  pthread_exit (0);
}

PRIVATE void
setup_meter_server ()
{
  top.meas.mtr.fd = CreateNamedPipe (top.meas.mtr.path,
				     PIPE_ACCESS_OUTBOUND,
				     PIPE_WAIT | PIPE_TYPE_MESSAGE |
				     PIPE_READMODE_MESSAGE,
				     PIPE_UNLIMITED_INSTANCES, 512, 512,
				     INFINITE, NULL);

  if (top.meas.mtr.fd == INVALID_HANDLE_VALUE)
    {
      fprintf (stderr, "Meter server pipe setup failed:\n"), fflush (stderr);
      DisplayErrorText (GetLastError ());
    }
  else
    {

      sem_post (&setup_update_sem);
      if (ConnectNamedPipe (top.meas.mtr.fd, NULL))
	{

	}
      else
	{
	  fprintf (stderr, "Meter Pipe Connect failed\n"), fflush (stderr);
	  DisplayErrorText (GetLastError ());
	}
    }
  pthread_exit (0);
}

PRIVATE void
setup_meter_client ()
{

  if (WaitNamedPipe (top.meas.mtr.path, NMPWAIT_USE_DEFAULT_WAIT))
    {

      if (INVALID_HANDLE_VALUE ==
	  (top.meas.mtr.fp =
	   CreateFile (top.meas.mtr.path, GENERIC_READ, 0, NULL,
		       OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL)))
	{
	  fprintf (stderr, "The Meter Client Open Failed\n"), fflush (stderr);
	  DisplayErrorText (GetLastError ());
	}
    }
  else
    {
      fprintf (stderr, "Wait for meter pipe failed: Error message %d\n",
	       GetLastError ()), fflush (stderr);
    }
  sem_post (&setup_update_sem);
  pthread_exit (0);
}

PRIVATE void
setup_spec_server ()
{

  if (INVALID_HANDLE_VALUE ==
      (top.meas.spec.fd =
       CreateNamedPipe (top.meas.spec.path, PIPE_ACCESS_OUTBOUND,
			PIPE_WAIT | PIPE_TYPE_MESSAGE |
			PIPE_READMODE_MESSAGE, PIPE_UNLIMITED_INSTANCES,
			32768, 32768, INFINITE, NULL)))
    {
      fprintf (stderr, "Spectrum pipe create failed\n"), fflush (stderr);
      DisplayErrorText (GetLastError ());
    }
  else
    {

      sem_post (&setup_update_sem);
      if (!ConnectNamedPipe (top.meas.spec.fd, NULL))
	{
	  fprintf (stderr, "Spectrum pipe connect failed\n"), fflush (stderr);
	  DisplayErrorText (GetLastError ());
	}
    }
  pthread_exit (0);
}

PRIVATE void
setup_spec_client ()
{

  if (WaitNamedPipe (top.meas.spec.path, NMPWAIT_USE_DEFAULT_WAIT))
    {

      if (INVALID_HANDLE_VALUE ==
	  (top.meas.spec.fp =
	   CreateFile (top.meas.spec.path, GENERIC_READ, 0, NULL,
		       OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL)))
	{
	  fprintf (stderr, "The Spectrum Client Open Failed\n"),
	    fflush (stderr);
	  DisplayErrorText (GetLastError ());
	}
    }
  else
    {
      fprintf (stderr, "Wait for spec pipe failed\n"), fflush (stderr);
      DisplayErrorText (GetLastError ());
    }
  sem_post (&setup_update_sem);
  pthread_exit (0);
}
PRIVATE pthread_t id1, id2, id3, id4, id5, id6;
PRIVATE void
setup_updates (void)
{

  top.parm.path = loc.path.parm;
  sem_init (&setup_update_sem, 0, 0);


  if (uni.meter.flag)
    {
      top.meas.mtr.path = loc.path.meter;
    }
  if (uni.spec.flag)
    {
      top.meas.spec.path = loc.path.spec;
    }

  uni.update.fp = fopen ("command_memory.txt", "w+");
  uni.update.flag = TRUE;


  // Do this STUPID stuff to make use of the Named Pipe Mechanism in Windows
  // For the update server


  pthread_create (&id1, NULL, (void *) setup_update_server, NULL);
  sem_wait (&setup_update_sem);
  Sleep (100);
  pthread_create (&id2, NULL, (void *) setup_update_client, NULL);
  sem_wait (&setup_update_sem);
  if (uni.meter.flag)
    {
      pthread_create (&id3, NULL, (void *) setup_meter_server, NULL);
      sem_wait (&setup_update_sem);
      Sleep (100);
      pthread_create (&id4, NULL, (void *) setup_meter_client, NULL);
      sem_wait (&setup_update_sem);
    }

  if (uni.spec.flag)
    {
      pthread_create (&id5, NULL, (void *) setup_spec_server, NULL);
      sem_wait (&setup_update_sem);
      Sleep (100);
      pthread_create (&id6, NULL, (void *) setup_spec_client, NULL);
      sem_wait (&setup_update_sem);
    }
  sem_destroy (&setup_update_sem);
}
PRIVATE void
setup_system_audio (void)
{
  sprintf (top.jack.name, "sdr-%d", top.pid);
  top.jack.size = 2048;

  memset ((char *) &top.jack.blow, 0, sizeof (top.jack.blow));
  top.jack.ring.i.l = ringb_float_create (top.jack.size * loc.mult.ring);
  top.jack.ring.i.r = ringb_float_create (top.jack.size * loc.mult.ring);
  top.jack.ring.o.l = ringb_float_create (top.jack.size * loc.mult.ring);
  top.jack.ring.o.r = ringb_float_create (top.jack.size * loc.mult.ring);

  top.jack.auxr.i.l = ringb_float_create (top.jack.size * loc.mult.ring);
  top.jack.auxr.i.r = ringb_float_create (top.jack.size * loc.mult.ring);
  top.jack.auxr.o.l = ringb_float_create (top.jack.size * loc.mult.ring);
  top.jack.auxr.o.r = ringb_float_create (top.jack.size * loc.mult.ring);

  ringb_float_clear (top.jack.ring.o.l, top.jack.size);
  ringb_float_clear (top.jack.ring.o.r, top.jack.size);
}

PRIVATE void
setup_threading (void)
{
  top.susp = FALSE;
  sem_init (&top.sync.upd.sem, 0, 0);
  pthread_create (&top.thrd.upd.id, NULL, (void *) process_updates_thread,
		  NULL);
  sem_init (&top.sync.buf.sem, 0, 0);
  //pthread_create(&top.thrd.trx.id, NULL, (void *) process_samples_thread, NULL);
  sem_init (&top.sync.mon.sem, 0, 0);
  pthread_create (&top.thrd.mon.id, NULL, (void *) monitor_thread, NULL);
/*  if (uni.meter.flag)
    {
      sem_init (&top.sync.mtr.sem, 0, 0);
      pthread_create (&top.thrd.mtr.id, NULL, (void *) meter_thread, NULL);
    }
  if (uni.spec.flag)
    {
      sem_init (&top.sync.pws.sem, 0, 0);
      //pthread_create(&top.thrd.pws.id, NULL, (void *) spectrum_thread, NULL);
    }
*/
}

//========================================================================
// hard defaults, then environment

PRIVATE void
setup_defaults ()
{
  loc.name[0] = 0;		// no default name for jack client
  sprintf (loc.path.rcfile, "%s%0lu", RCBASE, top.pid);
  sprintf (loc.path.parm, "%s%0lu", PARMPATH, top.pid);
  sprintf (loc.path.meter, "%s%0lu", METERPATH, top.pid);
  sprintf (loc.path.spec, "%s%0lu", SPECPATH, top.pid);
  sprintf (loc.path.wisdom, "%s%0lu", WISDOMPATH, top.pid);
  loc.def.rate = DEFRATE;
  loc.def.size = DEFSIZE;
  loc.def.nrx = MAXRX;
  loc.def.mode = DEFMODE;
  loc.def.spec = DEFSPEC;
  loc.mult.ring = RINGMULT;
  loc.def.comp = DEFCOMP;
}

//========================================================================
void
setup ()
{


  top.pid = GetCurrentThreadId ();
  top.uid = 0L;
  top.start_tv = now_tv ();
  top.running = TRUE;
  top.verbose = FALSE;
  top.state = RUN_PLAY;
  top.offset = 0;
  top.jack.reset_size = 1024;
  reset_em =TRUE;
  setup_defaults ();

  uni.meter.flag = TRUE;
  uni.spec.flag = TRUE;
  top.swch.env.fall.size = (int)(loc.def.rate * 0.005);
  top.swch.env.stdy.size = (int)(loc.def.rate * 0.050);
  top.swch.env.rise.size = (int)(loc.def.rate * 0.005);


  top.swch.env.curr.val = 0.0;
  top.swch.env.curr.cnt = 0;
  top.swch.env.rise.incr = 1.0f/(float)top.swch.env.rise.size;
  top.swch.env.fall.incr = 1.0f/(float)top.swch.env.fall.size;
  setup_workspace (loc.def.rate,
		   loc.def.size,
		   loc.def.mode,
		   loc.path.wisdom, loc.def.spec, loc.def.nrx, loc.def.comp);

  setup_updates ();

  setup_local_audio ();
  setup_system_audio ();

  setup_threading ();
//  setup_switching ();
  uni.spec.flag = TRUE;
  uni.spec.type = SPEC_POST_FILT;
  uni.spec.scale = SPEC_PWR;
  uni.spec.rxk = 0;
  reset_meters ();
  reset_spectrum ();
  reset_counters ();
}

int
reset_for_buflen (int new_buflen)
{

  // make sure new size is power of 2
  if (popcnt (new_buflen) != 1)
    return -1;
  safefree ((char *) top.hold.buf.r);
  safefree ((char *) top.hold.buf.l);
  safefree ((char *) top.hold.aux.r);
  safefree ((char *) top.hold.aux.l);
  uni.buflen = new_buflen;
  top.jack.reset_size = new_buflen;
  

  destroy_workspace ();
  loc.def.size = new_buflen;
  setup_workspace (loc.def.rate,
		   loc.def.size,
		   loc.def.mode,
		   loc.path.wisdom, loc.def.spec, loc.def.nrx, loc.def.size);

  setup_local_audio ();
//  setup_switching ();

  reset_meters ();
  reset_spectrum ();
  reset_counters ();

  return 0;
}

int
reset_for_samplerate (REAL new_samplerate)
{

  // make sure new sample rate works
  destroy_workspace ();
  loc.def.rate = uni.samplerate = new_samplerate;
  top.swch.env.fall.size = (int)(loc.def.rate * 0.005);
  top.swch.env.stdy.size = (int)(loc.def.rate * 0.050);
  top.swch.env.rise.size = (int)(loc.def.rate * 0.005);
  top.swch.env.curr.val = 0.0;
  top.swch.env.curr.cnt = 0;
  top.swch.env.fall.incr = 1.0f/(float)top.swch.env.fall.size;
  top.swch.env.rise.incr = 1.0f/(float)top.swch.env.rise.size;
  setup_workspace (loc.def.rate,
		   loc.def.size,
		   loc.def.mode,
		   loc.path.wisdom, loc.def.spec, loc.def.nrx, loc.def.size);
  setup_local_audio ();
//  setup_switching ();
  reset_meters ();
  reset_spectrum ();
  reset_counters ();
  return 0;
}
