/* sdr.c

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2004, 2005, 2006 by Frank Brickle, AB2KT and Bob McGwier, N4HY.

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

//========================================================================
/* initialization and termination */

void
reset_meters (void)
{
  if (uni.meter.flag)
    {				// reset metering completely
      int i, k;
      for (i = 0; i < RXMETERPTS; i++)
		for (k = 0; k < MAXRX; k++)
			uni.meter.rx.val[k][i] = 0.0;
      for (i = 0; i < TXMETERPTS; i++)
		  uni.meter.tx.val[i] = 0.0;
    }
}

void
reset_spectrum (void)
{
  if (uni.spec.flag)
    reinit_spectrum (&uni.spec);
}

void
reset_counters (void)
{
  int k;
  for (k = 0; k < uni.multirx.nrx; k++)
    rx[k].tick = 0;
  tx.tick = 0;
}

//========================================================================

/* global and general info,
   not specifically attached to
   tx, rx, or scheduling */

PRIVATE void
setup_all (REAL rate,
	   int buflen,
	   SDRMODE mode, char *wisdom, int specsize, int numrecv, int cpdsize)
{

  uni.samplerate = rate;
  uni.buflen = buflen;
  uni.mode.sdr = mode;
  uni.mode.trx = RX;

  uni.wisdom.path = wisdom;
  uni.wisdom.bits = FFTW_ESTIMATE;
  {
    FILE *f = fopen (uni.wisdom.path, "r");
    if (f)
      {
	if (fftwf_import_wisdom_from_file (f) != 0)
	  uni.wisdom.bits = FFTW_MEASURE;
	fclose (f);
      }
  }

  if (uni.meter.flag)
    {
      reset_meters ();
    }

  uni.spec.rxk = 0;
  uni.spec.buflen = uni.buflen;
  uni.spec.scale = SPEC_PWR;
  uni.spec.type = SPEC_POST_FILT;
  uni.spec.size = specsize;
  uni.spec.planbits = uni.wisdom.bits;
  init_spectrum (&uni.spec);

  // set which receiver is listening to commands
  uni.multirx.lis = 0;
  uni.multirx.nrx = numrecv;

  // set mixing of input from aux ports
  uni.mix.rx.flag = uni.mix.tx.flag = FALSE;
  uni.mix.rx.gain = uni.mix.tx.gain = 1.0;

  uni.cpdlen = cpdsize;

  uni.tick = 0;
}

/* purely rx */

PRIVATE void
setup_rx (int k)
{

  /* conditioning */
  rx[k].iqfix = newCorrectIQ (0.0, 1.0);
  rx[k].filt.coef = newFIR_Bandpass_COMPLEX (-4800.0,
					     4800.0,
					     uni.samplerate, uni.buflen + 1);
  rx[k].filt.ovsv =
    newFiltOvSv (FIRcoef (rx[k].filt.coef), FIRsize (rx[k].filt.coef),
		 uni.wisdom.bits);
  normalize_vec_COMPLEX (rx[k].filt.ovsv->zfvec, rx[k].filt.ovsv->fftlen);

  // hack for EQ
  rx[k].filt.save =
    newvec_COMPLEX (rx[k].filt.ovsv->fftlen, "RX filter cache");
  memcpy ((char *) rx[k].filt.save, (char *) rx[k].filt.ovsv->zfvec,
	  rx[k].filt.ovsv->fftlen * sizeof (COMPLEX));

  /* buffers */
  /* note we overload the internal filter buffers
     we just created */
  rx[k].buf.i = newCXB (FiltOvSv_fetchsize (rx[k].filt.ovsv),
			FiltOvSv_fetchpoint (rx[k].filt.ovsv),
			"init rx[k].buf.i");

  rx[k].buf.o = newCXB (FiltOvSv_storesize (rx[k].filt.ovsv),
			FiltOvSv_storepoint (rx[k].filt.ovsv),
			"init rx[k].buf.o");

  /* conversion */
  rx[k].osc.freq = -11025.0;
  rx[k].osc.phase = 0.0;
  rx[k].osc.gen = newOSC (uni.buflen,
			  ComplexTone,
			  rx[k].osc.freq,
			  rx[k].osc.phase,
			  uni.samplerate, "SDR RX Oscillator");


  rx[k].dttspagc.gen = newDttSPAgc (1,	// mode kept around for control reasons alone
				    CXBbase (rx[k].buf.o),	// input buffer
				    CXBsize (rx[k].buf.o),	// output buffer
				    1.0f,	// Target output 
				    2.0f,	// Attack time constant in ms
				    500,	// Decay time constant in ms
				    1.0,	// Slope
				    500,	//Hangtime in ms
				    uni.samplerate,	// Sample rate
				    2511.886f,	// Maximum gain as a multipler, linear not dB
				    0.00001f,	// Minimum gain as a multipler, linear not dB
				    1.0,	// Set the current gain
				    "AGC"	// Set a tag for an error message if the memory allocation fails
    );

  rx[k].dttspagc.flag = TRUE;

  rx[k].grapheq.gen = new_EQ (rx[k].buf.o, uni.samplerate, uni.wisdom.bits);
  rx[k].grapheq.flag = FALSE;


  /* demods */
  rx[k].am.gen = newAMD (uni.samplerate,	// REAL samprate
			 0.0,	// REAL f_initial
			 -500.0,	// REAL f_lobound,
			 500.0,	// REAL f_hibound,
			 400.0,	// REAL f_bandwid,
			 CXBsize (rx[k].buf.o),	// int size,
			 CXBbase (rx[k].buf.o),	// COMPLEX *ivec,
			 CXBbase (rx[k].buf.o),	// COMPLEX *ovec,
			 AMdet,	// AM Mode AMdet == rectifier,
			 //         SAMdet == synchronous detector
			 "AM detector blew");	// char *tag
  rx[k].fm.gen = newFMD (uni.samplerate,	// REAL samprate
			 0.0,	// REAL f_initial
			 -6000.0,	// REAL f_lobound
			 6000.0,	// REAL f_hibound
			 5000.0,	// REAL f_bandwid
			 CXBsize (rx[k].buf.o),	// int size
			 CXBbase (rx[k].buf.o),	// COMPLEX *ivec
			 CXBbase (rx[k].buf.o),	// COMPLEX *ovec
			 "New FM Demod structure");	// char *error message;

  /* noise reduction */
  rx[k].anf.gen = new_lmsr (rx[k].buf.o,	// CXB signal,
			    64,	// int delay,
			    0.01f,	// REAL adaptation_rate,
			    0.00001f,	// REAL leakage,
			    45,	// int adaptive_filter_size,
			    LMADF_INTERFERENCE);
  rx[k].anf.flag = FALSE;
  rx[k].banf.gen =
    new_blms (rx[k].buf.o, 0.00001f, 0.005f, LMADF_INTERFERENCE,
	      uni.wisdom.bits);
  rx[k].banf.flag = FALSE;

  rx[k].anr.gen = new_lmsr (rx[k].buf.o,	// CXB signal,
			    64,	// int delay,
			    0.01f,	// REAL adaptation_rate,
			    0.00001f,	// REAL leakage,
			    45,	// int adaptive_filter_size,
			    LMADF_NOISE);
  rx[k].anr.flag = FALSE;
  rx[k].banr.gen =
    new_blms (rx[k].buf.o, 0.00001f, 0.005f, LMADF_NOISE, uni.wisdom.bits);
  rx[k].banr.flag = FALSE;

  rx[k].nb.thresh = 3.3f;
  rx[k].nb.gen = new_noiseblanker (rx[k].buf.i, rx[k].nb.thresh);
  rx[k].nb.flag = FALSE;

  rx[k].nb_sdrom.thresh = 2.5f;
  rx[k].nb_sdrom.gen = new_noiseblanker (rx[k].buf.i, rx[k].nb_sdrom.thresh);
  rx[k].nb_sdrom.flag = FALSE;

  rx[k].spot.gen = newSpotToneGen (-12.0,	// gain
				   700.0,	// freq
				   5.0,	// ms rise
				   5.0,	// ms fall
				   uni.buflen,	// length of spot tone buffer
				   uni.samplerate	// sample rate
    );


  memset ((char *) &rx[k].squelch, 0, sizeof (rx[k].squelch));
  rx[k].squelch.thresh = -150.0;
  rx[k].squelch.power = 0.0;
  rx[k].squelch.flag = rx[k].squelch.running = rx[k].squelch.set = FALSE;
  rx[k].squelch.num = uni.buflen - 48;

  rx[k].cpd.gen = newWSCompander (uni.cpdlen, 0.0, rx[k].buf.o);
  rx[k].cpd.flag = FALSE;

  rx[k].mode = uni.mode.sdr;
  rx[k].bin.flag = FALSE;

  {
    REAL pos = 0.5,		// 0 <= pos <= 1, left->right
      theta = (REAL) ((1.0 - pos) * M_PI / 2.0);
    rx[k].azim = Cmplx ((REAL) cos (theta), (IMAG) sin (theta));
  }

  rx[k].tick = 0;
}

/* purely tx */

PRIVATE void
setup_tx (void)
{

  /* conditioning */
  tx.iqfix = newCorrectIQ (0.0, 1.0);
  tx.filt.coef = newFIR_Bandpass_COMPLEX (300.0,
					  3000.0,
					  uni.samplerate, uni.buflen + 1);
  tx.filt.ovsv = newFiltOvSv (FIRcoef (tx.filt.coef),
			      FIRsize (tx.filt.coef), uni.wisdom.bits);
  normalize_vec_COMPLEX (tx.filt.ovsv->zfvec, tx.filt.ovsv->fftlen);

  // hack for EQ
  tx.filt.save = newvec_COMPLEX (tx.filt.ovsv->fftlen, "TX filter cache");
  memcpy ((char *) tx.filt.save,
	  (char *) tx.filt.ovsv->zfvec,
	  tx.filt.ovsv->fftlen * sizeof (COMPLEX));

  /* buffers */
  tx.buf.i = newCXB (FiltOvSv_fetchsize (tx.filt.ovsv),
		     FiltOvSv_fetchpoint (tx.filt.ovsv), "init tx.buf.i");
  tx.buf.o = newCXB (FiltOvSv_storesize (tx.filt.ovsv),
		     FiltOvSv_storepoint (tx.filt.ovsv), "init tx.buf.o");

  tx.dcb.flag = FALSE;
  tx.dcb.gen = newDCBlocker (DCB_MED, tx.buf.i);

  /* conversion */
  tx.osc.freq = 0.0;
  tx.osc.phase = 0.0;
  tx.osc.gen = newOSC (uni.buflen,
		       ComplexTone,
		       tx.osc.freq,
		       tx.osc.phase, uni.samplerate, "SDR TX Oscillator");


  tx.am.carrier_level = 0.5f;
  tx.fm.cvtmod2freq = (REAL) (2500.0 * TWOPI / uni.samplerate);	//5 kHz deviation

  tx.leveler.gen = newDttSPAgc (1,	// mode kept around for control reasons
				CXBbase (tx.buf.i),	// input buffer
				CXBsize (tx.buf.i),	// output buffer
				1.1f,	// Target output
				2,	// Attack time constant in ms
				500,	// Decay time constant in ms
				1,	// Slope
				500,	//Hangtime in ms
				uni.samplerate,	// Sample rate
				5.62f,	// Maximum gain as a multipler, linear not dB
				1.0,	// Minimum gain as a multipler, linear not dB
				1.0,	// Set the current gain
				"LVL"	// Set a tag for an error message if the memory allocation fails
    );
  tx.leveler.flag = TRUE;

  tx.grapheq.gen = new_EQ (tx.buf.i, uni.samplerate, uni.wisdom.bits);
  tx.grapheq.flag = FALSE;


  memset ((char *) &tx.squelch, 0, sizeof (tx.squelch));
  tx.squelch.thresh = -40.0;
  tx.squelch.power = 0.0;
  tx.squelch.flag = FALSE;
  tx.squelch.running = tx.squelch.set = FALSE;
  tx.squelch.num = uni.buflen - 48;


  tx.alc.gen = newDttSPAgc (1,	// mode kept around for control reasons alone
			    CXBbase (tx.buf.i),	// input buffer
			    CXBsize (tx.buf.i),	// output buffer
			    1.2f,	// Target output 
			    2,	// Attack time constant in ms
			    10,	// Decay time constant in ms
			    1,	// Slope
			    500,	//Hangtime in ms
			    uni.samplerate, 1.0,	// Maximum gain as a multipler, linear not dB
			    .000001f,	// Minimum gain as a multipler, linear not dB
			    1.0,	// Set the current gain
			    "ALC"	// Set a tag for an error message if the memory allocation fails
    );
  tx.alc.flag = TRUE;


  tx.spr.gen =
    newSpeechProc (0.4f, 3.0, CXBbase (tx.buf.i), CXBsize (tx.buf.o));
  tx.spr.flag = FALSE;

  tx.cpd.gen = newWSCompander (uni.cpdlen, -3.0, tx.buf.i);
  tx.cpd.flag = FALSE;

  //tx.scl.dc = cxzero;

  tx.mode = uni.mode.sdr;

  tx.tick = 0;
  /* not much else to do for TX */
}

/* how the outside world sees it */

void
setup_workspace (REAL rate,
		 int buflen,
		 SDRMODE mode,
		 char *wisdom, int specsize, int numrecv, int cpdsize)
{
  int k;

  setup_all (rate, buflen, mode, wisdom, specsize, numrecv, cpdsize);

  for (k = 0; k < uni.multirx.nrx; k++)
    {
      setup_rx (k);
      uni.multirx.act[k] = FALSE;
    }
  uni.multirx.act[0] = TRUE;
  uni.multirx.nac = 1;

  setup_tx ();
}

void
destroy_workspace (void)
{
  int k;

  /* TX */
  delWSCompander (tx.cpd.gen);
  delSpeechProc (tx.spr.gen);
  delDttSPAgc (tx.leveler.gen);
  delDttSPAgc (tx.alc.gen);
  delOSC (tx.osc.gen);
  delDCBlocker (tx.dcb.gen);
  delvec_COMPLEX (tx.filt.save);
  delFiltOvSv (tx.filt.ovsv);
  delFIR_Bandpass_COMPLEX (tx.filt.coef);
  delCorrectIQ (tx.iqfix);
  delCXB (tx.buf.o);
  delCXB (tx.buf.i);

  /* RX */
  for (k = 0; k < uni.multirx.nrx; k++)
    {
      delWSCompander (rx[k].cpd.gen);
      delSpotToneGen (rx[k].spot.gen);
      delDttSPAgc (rx[k].dttspagc.gen);
      del_nb (rx[k].nb_sdrom.gen);
      del_nb (rx[k].nb.gen);
      del_lmsr (rx[k].anf.gen);
      del_lmsr (rx[k].anr.gen);
      del_blms (rx[k].banf.gen);
      del_blms (rx[k].banr.gen);
      delAMD (rx[k].am.gen);
      delFMD (rx[k].fm.gen);
      delOSC (rx[k].osc.gen);
      delvec_COMPLEX (rx[k].filt.save);
      delFiltOvSv (rx[k].filt.ovsv);
      delFIR_Bandpass_COMPLEX (rx[k].filt.coef);
      delCorrectIQ (rx[k].iqfix);
      delCXB (rx[k].buf.o);
      delCXB (rx[k].buf.i);
    }

  /* all */
  finish_spectrum (&uni.spec);
}

//////////////////////////////////////////////////////////////////////////
// execution
//////////////////////////////////////////////////////////////////////////

//========================================================================
// util

PRIVATE void
CXBscl (CXB buff, REAL scl)
{
  int i;
  for (i = 0; i < CXBhave (buff); i++)
    CXBdata (buff, i) = Cscl (CXBdata (buff, i), scl);
}

PRIVATE REAL
CXBnorm (CXB buff)
{
  int i;
  REAL sum = 0.0;
  for (i = 0; i < CXBhave (buff); i++)
    sum += Csqrmag (CXBdata (buff, i));
  return (REAL) sqrt (sum);
}

PRIVATE REAL
CXBnormsqr (CXB buff)
{
  int i;
  REAL sum = 0.0;
  for (i = 0; i < CXBhave (buff); i++)
    sum += Csqrmag (CXBdata (buff, i));
  return (REAL) (sum);
}

PRIVATE REAL
CXBpeak (CXB buff)
{
  int i;
  REAL maxsam = 0.0;
  for (i = 0; i < CXBhave (buff); i++)
    maxsam = max (Cmag (CXBdata (buff, i)), maxsam);
  return maxsam;

}

PRIVATE REAL
CXBpeakpwr (CXB buff)
{
  int i;
  REAL maxpwr = 0.0;
  for (i = 0; i < CXBhave (buff); i++)
    maxpwr = max (Csqrmag (CXBdata (buff, i)), maxpwr);
  return maxpwr;

}

//========================================================================
/* all */

// unfortunate duplication here, due to
// multirx vs monotx

PRIVATE void
do_rx_meter (int k, CXB buf, int tap)
{
  COMPLEX *vec = CXBbase (buf);
  int i, len = CXBhave (buf);


  switch (tap) {
    case RXMETER_PRE_CONV:
      for (i = 0; i < len; i++)
		  uni.meter.rx.val[k][ADC_REAL] =
		  (REAL) max (fabs (vec[i].re), uni.meter.rx.val[k][ADC_REAL]);
		  uni.meter.rx.val[k][ADC_REAL] = (REAL) (20.0 * log10 (uni.meter.rx.val[k][ADC_REAL] + 1e-10));
      for (i = 0; i < len; i++)
		  uni.meter.rx.val[k][ADC_IMAG] =
		  (REAL) max (fabs (vec[i].im), uni.meter.rx.val[k][ADC_IMAG]);
		  uni.meter.rx.val[k][ADC_IMAG] = (REAL) (20.0 * log10 (uni.meter.rx.val[k][ADC_IMAG] + 1e-10));
      break;
    case RXMETER_POST_FILT:
	  uni.meter.rx.val[k][SIGNAL_STRENGTH] = 0;
      for (i = 0; i < len; i++)
		  uni.meter.rx.val[k][SIGNAL_STRENGTH] += Csqrmag (vec[i]);
	  rx[k].norm = uni.meter.rx.val[k][SIGNAL_STRENGTH] / (REAL) len;
      uni.meter.rx.val[k][SIGNAL_STRENGTH] =
		  (REAL) (10.0 * log10 (uni.meter.rx.val[k][SIGNAL_STRENGTH] + 1e-20));
	  if (uni.meter.rx.mode[k] == SIGNAL_STRENGTH) uni.meter.rx.val[k][AVG_SIGNAL_STRENGTH] = uni.meter.rx.val[k][SIGNAL_STRENGTH];
 	  uni.meter.rx.val[k][AVG_SIGNAL_STRENGTH] =
		  (REAL) (0.95 * uni.meter.rx.val[k][AVG_SIGNAL_STRENGTH] +
		  0.05 *uni.meter.rx.val[k][SIGNAL_STRENGTH]);
      break;
    case RXMETER_POST_AGC:
      uni.meter.rx.val[k][AGC_GAIN] =
		  (REAL) (20.0 * log10 (rx[k].dttspagc.gen->gain.now + 1e-10));
      break;
    default:
      break;
    }
}


PRIVATE void
do_rx_spectrum (int k, CXB buf, int type)
{
  if (uni.spec.flag && k == uni.spec.rxk && type == uni.spec.type)
    {
      memcpy ((char *) &CXBdata (uni.spec.accum, uni.spec.fill),
	      (char *) CXBbase (buf), CXBsize (buf) * sizeof (COMPLEX));
      uni.spec.fill = (uni.spec.fill + CXBsize (buf)) & uni.spec.mask;
    }
}

PRIVATE void
do_tx_spectrum (CXB buf)
{
  memcpy ((char *) &CXBdata (uni.spec.accum, uni.spec.fill),
	  (char *) CXBbase (buf), CXBsize (buf) * sizeof (COMPLEX));
  uni.spec.fill = (uni.spec.fill + CXBsize (buf)) & uni.spec.mask;
}

//========================================================================
/* RX processing */

PRIVATE BOOLEAN
should_do_rx_squelch (int k)
{
  if (rx[k].squelch.flag)
    {
      int i, n = CXBhave (rx[k].buf.o);
      rx[k].squelch.power = 0.0;

      for (i = 0; i < n; i++)
	rx[k].squelch.power += Csqrmag (CXBdata (rx[k].buf.o, i));

      return
	10.0 * log10 (rx[k].squelch.power + 1e-17) < rx[k].squelch.thresh;

    }
  else
    return rx[k].squelch.set = FALSE;
}

PRIVATE BOOLEAN
should_do_tx_squelch (void)
{
  if (tx.squelch.flag)
    {
      int i, n = CXBsize (tx.buf.i);
      tx.squelch.power = 0.0;

      for (i = 0; i < n; i++)
	tx.squelch.power += Csqrmag (CXBdata (tx.buf.i, i));

      return
	(-30 + 10.0 * log10 (tx.squelch.power + 1e-17)) < tx.squelch.thresh;

    }
  else
    return tx.squelch.set = FALSE;
}

// apply squelch
// slew into silence first time

PRIVATE void
do_squelch (int k)
{
  rx[k].squelch.set = TRUE;

  if (!rx[k].squelch.running)
    {
      int i, m = rx[k].squelch.num, n = CXBhave (rx[k].buf.o) - m;

      for (i = 0; i < m; i++)
	CXBdata (rx[k].buf.o, i) =
	  Cscl (CXBdata (rx[k].buf.o, i), (REAL) (1.0 - (REAL) i / m));

      memset ((void *) (CXBbase (rx[k].buf.o) + m), 0, n * sizeof (COMPLEX));
      rx[k].squelch.running = TRUE;

    }
  else
    memset ((void *) CXBbase (rx[k].buf.o),
	    0, CXBhave (rx[k].buf.o) * sizeof (COMPLEX));
}
PRIVATE void
do_tx_squelch (void)
{
  tx.squelch.set = TRUE;

  if (!tx.squelch.running)
    {
      int i, m = tx.squelch.num, n = CXBhave (tx.buf.i) - m;

      for (i = 0; i < m; i++)
	CXBdata (tx.buf.i, i) =
	  Cscl (CXBdata (tx.buf.i, i), (REAL) (1.0 - (REAL) i / m));

      memset ((void *) (CXBbase (tx.buf.i) + m), 0, n * sizeof (COMPLEX));
      tx.squelch.running = TRUE;

    }
  else
    memset ((void *) CXBbase (tx.buf.i),
	    0, CXBhave (tx.buf.i) * sizeof (COMPLEX));
}

// lift squelch
// slew out from silence to full scale

PRIVATE void
no_squelch (int k)
{
  if (rx[k].squelch.running)
    {
      int i, m = rx[k].squelch.num;

      for (i = 0; i < m; i++)
	CXBdata (rx[k].buf.o, i) =
	  Cscl (CXBdata (rx[k].buf.o, i), (REAL) i / m);

      rx[k].squelch.running = FALSE;
    }
}
PRIVATE void
no_tx_squelch ()
{
  if (tx.squelch.running)
    {
      int i, m = tx.squelch.num;

      for (i = 0; i < m; i++)
	CXBdata (tx.buf.i, i) = Cscl (CXBdata (tx.buf.i, i), (REAL) i / m);

      tx.squelch.running = FALSE;
    }
}

/* pre-condition for (nearly) all RX modes */
PRIVATE void
do_rx_pre (int k)
{
  int i, n = min (CXBhave (rx[k].buf.i), uni.buflen);


  if (rx[k].nb.flag)
    noiseblanker (rx[k].nb.gen);
  if (rx[k].nb_sdrom.flag)
    SDROMnoiseblanker (rx[k].nb_sdrom.gen);

  // metering for uncorrected values here

  do_rx_meter (k, rx[k].buf.i, RXMETER_PRE_CONV);

  correctIQ (rx[k].buf.i, rx[k].iqfix);

  /* 2nd IF conversion happens here */

  if (rx[k].osc.gen->Frequency != 0.0)
    {
      ComplexOSC (rx[k].osc.gen);
      for (i = 0; i < n; i++)
	CXBdata (rx[k].buf.i, i) = Cmul (CXBdata (rx[k].buf.i, i),
					 OSCCdata (rx[k].osc.gen, i));
    }

  /* filtering, metering, spectrum, squelch, & AGC */


  //do_rx_meter (k, rx[k].buf.i, RXMETER_PRE_FILT);
  do_rx_spectrum (k, rx[k].buf.i, SPEC_PRE_FILT);
  if (rx[k].mode != SPEC)
    {
      if (rx[k].tick == 0)
	reset_OvSv (rx[k].filt.ovsv);

      filter_OvSv (rx[k].filt.ovsv);
    }
  else
    memcpy (CXBbase (rx[k].buf.o), CXBbase (rx[k].buf.i),
	    sizeof (COMPLEX) * CXBhave (rx[k].buf.i));


  CXBhave (rx[k].buf.o) = CXBhave (rx[k].buf.i);

  do_rx_meter (k, rx[k].buf.o, RXMETER_POST_FILT);
  do_rx_spectrum (k, rx[k].buf.o, SPEC_POST_FILT);

  if (rx[k].cpd.flag)
    WSCompand (rx[k].cpd.gen);

  if (should_do_rx_squelch (k))
    do_squelch (k);
  else
    DttSPAgc (rx[k].dttspagc.gen, rx[k].tick);
  do_rx_meter(k, rx[k].buf.o,RXMETER_POST_AGC);
  do_rx_spectrum (k, rx[k].buf.o, SPEC_POST_AGC);

}

PRIVATE void
do_rx_post (int k)
{
  int i, n = CXBhave (rx[k].buf.o);

  if (!rx[k].squelch.set)
    {
      no_squelch (k);
      // spotting tone
      if (rx[k].spot.flag)
	{
	  // remember whether it's turned itself off during this pass
	  rx[k].spot.flag = SpotTone (rx[k].spot.gen);
	  for (i = 0; i < n; i++)
	    CXBdata (rx[k].buf.o, i) = Cadd (CXBdata (rx[k].buf.o, i),
					     CXBdata (rx[k].spot.gen->buf,
						      i));
	}
      if (rx[k].grapheq.flag)
	graphiceq (rx[k].grapheq.gen);
    }
  // not binaural?
  // position in stereo field

//  if (!rx[k].bin.flag)
//    for (i = 0; i < n; i++)
//      CXBdata(rx[k].buf.o, i) = Cscl(rx[k].azim, 1.41421356f*CXBreal(rx[k].buf.o, i));
}

/* demod processing */

PRIVATE void
do_rx_SBCW (int k)
{
  if (rx[k].bin.flag)
    {
      if ((rx[k].banr.flag) && (rx[k].anr.flag))
	blms_adapt (rx[k].banr.gen);
      if ((rx[k].banf.flag) && (rx[k].anf.flag))
	blms_adapt (rx[k].banf.gen);

    }
  else
    {
      int i;
      if (rx[k].anr.flag)
	if (rx[k].banr.flag)
	  blms_adapt (rx[k].banr.gen);
	else
	  lmsr_adapt (rx[k].anr.gen);
      if (rx[k].anf.flag)
	if (rx[k].banf.flag)
	  blms_adapt (rx[k].banf.gen);
	else
	  lmsr_adapt (rx[k].anf.gen);
      for (i = 0; i < CXBhave (rx[k].buf.o); i++)
	CXBimag (rx[k].buf.o, i) = CXBreal (rx[k].buf.o, i);
    }
}

PRIVATE void
do_rx_AM (int k)
{
  AMDemod (rx[k].am.gen);
  if (rx[k].anf.flag)
    if (!rx[k].banf.flag)
      lmsr_adapt (rx[k].anf.gen);
    else
      blms_adapt (rx[k].banf.gen);
}

PRIVATE void
do_rx_FM (int k)
{
  FMDemod (rx[k].fm.gen);
}

PRIVATE void
do_rx_DRM (int k)
{
}

PRIVATE void
do_rx_SPEC (int k)
{
}

PRIVATE void
do_rx_NIL (int k)
{
  int i, n = min (CXBhave (rx[k].buf.i), uni.buflen);
  for (i = 0; i < n; i++)
    CXBdata (rx[k].buf.o, i) = cxzero;
}

/* overall dispatch for RX processing */

PRIVATE void
do_rx (int k)
{
  do_rx_pre (k);
  switch (rx[k].mode)
    {
    case DIGU:
    case DIGL:
    case USB:
    case LSB:
    case CWU:
    case CWL:
    case DSB:
      do_rx_SBCW (k);
      break;
    case AM:
    case SAM:
      do_rx_AM (k);
      break;
    case FMN:
      do_rx_FM (k);
      break;
    case DRM:
      do_rx_DRM (k);
      break;
    case SPEC:
    default:
      do_rx_SPEC (k);
      break;
    }
  do_rx_post (k);
}

//==============================================================
/* TX processing */
PRIVATE REAL micsave = 0.0f, alcsave = 0.0f, pwrsave =
    0.0f, levelersave = 0.0f, eqtapsave = 0.0f, compsave =
    0.0f, cpdrsave = 0.0f;

/* pre-condition for (nearly) all TX modes */
PRIVATE REAL peaksmooth = 0.0;
PRIVATE void
do_tx_meter (CXB buf, TXMETERTYPE mt)
{
  COMPLEX *vec = CXBbase (buf);
  int i, len = CXBhave (buf);

  switch (mt) {
    case TX_MIC:
      for (i = 0; i < CXBhave (tx.buf.i); i++)
			micsave =
			(REAL) (0.9995 * micsave +
				0.0005 * Csqrmag (CXBdata (tx.buf.i, i)));
			uni.meter.tx.val[TX_MIC] = (REAL) (-10.0 * log10 (micsave + 1e-16)); 
      break;

    case TX_PWR:
		for (i = 0, uni.meter.tx.val[TX_PWR] = 0.0000001f;
			i < CXBhave (tx.buf.o); i++)
				uni.meter.tx.val[TX_PWR] += Csqrmag (CXBdata (tx.buf.o, i));
			uni.meter.tx.val[TX_PWR] /= (REAL) len;
      break;

    case TX_ALC:
      for (i = 0; i < CXBhave (tx.buf.i); i++)
		alcsave =
		(REAL) (0.9995 * alcsave +
			0.0005 * Csqrmag (CXBdata (tx.buf.i, i)));
 		uni.meter.tx.val[TX_ALC] = (REAL) (-10.0 * log10 (alcsave+ 1e-16));
		uni.meter.tx.val[TX_ALC_G] = (REAL)(20.0*log10(tx.alc.gen->gain.now+1e-16));
      break;

    case TX_EQtap:
      for (i = 0; i < CXBhave (tx.buf.i); i++)
		eqtapsave =
		(REAL) (0.9995 * eqtapsave +
			0.0005 * Csqrmag (CXBdata (tx.buf.i, i)));
		uni.meter.tx.val[TX_EQtap] =
				(REAL) (-10.0 * log10 (eqtapsave + 1e-16));

       break;

    case TX_LEVELER:
      for (i = 0; i < CXBhave (tx.buf.i); i++)
		levelersave =
		(REAL) (0.9995 * levelersave +
			0.0005 * Csqrmag (CXBdata (tx.buf.i, i)));
		uni.meter.tx.val[TX_LEVELER] = (REAL) (-10.0 * log10 (levelersave + 1e-16));
		uni.meter.tx.val[TX_LVL_G] = (REAL)(20.0*log10(tx.leveler.gen->gain.now+1e-16));
      break;

    case TX_COMP:
      for (i = 0; i < CXBhave (tx.buf.i); i++)
		compsave =
		(REAL) (0.9995 * compsave +
			0.0005 * Csqrmag (CXBdata (tx.buf.i, i)));
		uni.meter.tx.val[TX_COMP] = (REAL) (-10.0 * log10 (compsave+1e-16));
      break;

    case TX_CPDR:
      for (i = 0; i < CXBhave (tx.buf.i); i++)
		cpdrsave =
		(REAL) (0.9995 * cpdrsave +
			0.0005 * Csqrmag (CXBdata (tx.buf.i, i)));
	  uni.meter.tx.val[TX_CPDR] = (REAL) (-10.0 * log10 (cpdrsave + 1e-16));
      break;

    default:
      break;
    }
}

PRIVATE void
do_tx_pre (void)
{
  int i, n = CXBhave (tx.buf.i);
  for (i = 0; i < n; i++)
    CXBdata (tx.buf.i, i) = Cmplx (CXBimag (tx.buf.i, i), 0.0);
  //fprintf(stderr,"Peak value = %f\n",CXBpeakpwr(tx.buf.i));
  if (tx.dcb.flag)
    DCBlock (tx.dcb.gen);

  do_tx_meter (tx.buf.i, TX_MIC);

  if (should_do_tx_squelch ())
    do_tx_squelch ();
  else
    {
      if (!tx.squelch.set)
	no_tx_squelch ();


      if ((tx.mode != DIGU) && (tx.mode != DIGL))
	{
	  if (tx.grapheq.flag)
	    graphiceq (tx.grapheq.gen);
	  do_tx_meter (tx.buf.i, TX_EQtap);
	  if (tx.leveler.flag)
	    DttSPAgc (tx.leveler.gen, tx.tick);
	  do_tx_meter (tx.buf.i, TX_LEVELER);
	  if (tx.spr.flag)
	    SpeechProcessor (tx.spr.gen);
	  do_tx_meter (tx.buf.i, TX_COMP);
	  if (tx.cpd.flag)
	    WSCompand (tx.cpd.gen);
	  do_tx_meter (tx.buf.i, TX_CPDR);
	}
	  else 
	  {
		do_tx_meter (tx.buf.i, TX_EQtap);
		do_tx_meter (tx.buf.i, TX_LEVELER);
		do_tx_meter (tx.buf.i, TX_LVL_G);
		do_tx_meter (tx.buf.i, TX_COMP);
		do_tx_meter (tx.buf.i, TX_CPDR);
	  }

    }

}



PRIVATE void
do_tx_post (void)
{
  CXBhave (tx.buf.o) = CXBhave (tx.buf.i);


  if (tx.tick == 0)
    reset_OvSv (tx.filt.ovsv);

  filter_OvSv (tx.filt.ovsv);
  if (uni.spec.flag)
    do_tx_spectrum (tx.buf.o);

  // meter modulated signal

  if (tx.osc.gen->Frequency != 0.0)
    {
      int i;
      ComplexOSC (tx.osc.gen);
      for (i = 0; i < CXBhave (tx.buf.o); i++)
	{
	  CXBdata (tx.buf.o, i) =
	    Cmul (CXBdata (tx.buf.o, i), OSCCdata (tx.osc.gen, i));
	}
    }
  correctIQ (tx.buf.o, tx.iqfix);
  do_tx_meter (tx.buf.o, TX_PWR);
  //fprintf(stderr,"Output peak: %f average power: %f Peak sample=%f\n",CXBpeakpwr(tx.buf.o), CXBnormsqr(tx.buf.o)/(REAL)CXBhave(tx.buf.o), CXBpeak(tx.buf.o)),fflush(stderr);
}

/* modulator processing */

PRIVATE void
do_tx_SBCW (void)
{
  int n = min (CXBhave (tx.buf.i), uni.buflen);

  if (tx.alc.flag && (tx.mode != DIGU) && (tx.mode != DIGL))
    DttSPAgc (tx.alc.gen, tx.tick);
  do_tx_meter (tx.buf.i, TX_ALC);
 if (tx.mode != DSB)
    CXBscl (tx.buf.i, 2.0f);
}

PRIVATE void
do_tx_AM (void)
{
  int i, n = min (CXBhave (tx.buf.i), uni.buflen);
  if (tx.alc.flag)
    DttSPAgc (tx.alc.gen, tx.tick);
  do_tx_meter (tx.buf.i, TX_ALC);


  for (i = 0; i < n; i++)
    {
      CXBdata (tx.buf.i, i) = Cmplx ((REAL)
				     (tx.am.carrier_level +
				      (1.0f -
				       tx.am.carrier_level) *
				      CXBreal (tx.buf.i, i)), 0.0);
    }
}

PRIVATE void
do_tx_FM (void)
{
  int i, n = min (CXBhave (tx.buf.i), uni.buflen);
  if (tx.alc.flag)
    DttSPAgc (tx.alc.gen, tx.tick);
  do_tx_meter (tx.buf.i, TX_ALC);

  for (i = 0; i < n; i++)
    {
      tx.osc.phase += CXBreal (tx.buf.i, i) * tx.fm.cvtmod2freq;
      CXBdata (tx.buf.i, i) =
	Cmplx ((REAL) cos (tx.osc.phase), (IMAG) sin (tx.osc.phase));
    }
}

PRIVATE void
do_tx_NIL (void)
{
  int i, n = min (CXBhave (tx.buf.i), uni.buflen);
  for (i = 0; i < n; i++)
    CXBdata (tx.buf.i, i) = cxzero;
}

/* general TX processing dispatch */

PRIVATE void
do_tx (void)
{
  do_tx_pre ();
  switch (tx.mode)
    {
    case USB:
    case LSB:
    case CWU:
    case CWL:
    case DIGU:
    case DIGL:
    case DSB:
      do_tx_SBCW ();
      break;
    case AM:
    case SAM:
      do_tx_AM ();
      break;
    case FMN:
      do_tx_FM ();
      break;
    case DRM:
    case SPEC:
    default:
      do_tx_NIL ();
      break;
    }
  do_tx_post ();
  //fprintf(stderr,"%f\n",Cmag(CXBdata(tx.buf.o,0))),fflush(stderr);
}

//========================================================================
/* overall buffer processing;
   come here when there are buffers to work on */

void
process_samples (float *bufl, float *bufr, float *auxl, float *auxr, int n)
{
  int i, k;

  switch (uni.mode.trx)
    {

    case RX:

      // make copies of the input for all receivers
      for (k = 0; k < uni.multirx.nrx; k++)
	if (uni.multirx.act[k])
	  {
	    for (i = 0; i < n; i++)
	      CXBimag (rx[k].buf.i, i) =
		bufl[i], CXBreal (rx[k].buf.i, i) = bufr[i];
	    CXBhave (rx[k].buf.i) = n;
	  }
      // prepare buffers for mixing
      memset ((char *) bufl, 0, n * sizeof (float));
      memset ((char *) bufr, 0, n * sizeof (float));

      // run all receivers
      for (k = 0; k < uni.multirx.nrx; k++)
	if (uni.multirx.act[k])
	  {
	    do_rx (k), rx[k].tick++;
	    // mix
	    for (i = 0; i < n; i++)
	      bufl[i] += (float) CXBimag (rx[k].buf.o, i),
		bufr[i] += (float) CXBreal (rx[k].buf.o, i);
	    CXBhave (rx[k].buf.o) = n;
	  }
      // late mixing of aux buffers
      if (uni.mix.rx.flag)
	for (i = 0; i < n; i++)
	  bufl[i] += (float) (auxl[i] * uni.mix.rx.gain),
	    bufr[i] += (float) (auxr[i] * uni.mix.rx.gain);

      break;

    case TX:

      // early mixing of aux buffers
      if (uni.mix.tx.flag)
	for (i = 0; i < n; i++)
	  bufl[i] += (float) (auxl[i] * uni.mix.tx.gain),
	    bufr[i] += (float) (auxr[i] * uni.mix.tx.gain);

      for (i = 0; i < n; i++)
	CXBimag (tx.buf.i, i) = bufl[i], CXBreal (tx.buf.i, i) = bufr[i];
      CXBhave (tx.buf.i) = n;
      tx.norm = CXBpeak (tx.buf.i);

      do_tx (), tx.tick++;

      for (i = 0; i < n; i++)
	bufl[i] = (float) CXBimag (tx.buf.o, i), bufr[i] =
	  (float) CXBreal (tx.buf.o, i);
      CXBhave (tx.buf.o) = n;

      break;
    }

  uni.tick++;
}
