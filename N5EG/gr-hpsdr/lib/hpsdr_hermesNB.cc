/* -*- c++ -*- */
/* 
 * Copyright 2012 Tom McDermott, N5EG
 * 
 * This is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 3, or (at your option)
 * any later version.
 * 
 * This software is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this software; see the file COPYING.  If not, write to
 * the Free Software Foundation, Inc., 51 Franklin Street,
 * Boston, MA 02110-1301, USA.
 */

#ifdef HAVE_CONFIG_H
#include "config.h"
#endif

#include <gr_io_signature.h>
#include <hpsdr_hermesNB.h>

#include "HermesProxy.h"
#include <stdio.h>	// for DEBUG PRINTF's

HermesProxy* Hermes;	// make it visible to metis.cc


/*! \brief Make a hermesNB module
 * \param RxF    Receive Frequency
 * \param RxSmp  Receive Sample Rate
 * \param RxPre  Receive Preamp
 */

hpsdr_hermesNB_sptr
hpsdr_make_hermesNB (int RxF, int RxSmp, int RxPre)
{
    return gnuradio::get_initial_sptr(new hpsdr_hermesNB (RxF, RxSmp, RxPre));
}


/*!
 * \brief Prototype for Hermes module
 *
 * \details
 * Supports callbacks which allow sliders to change values at runtime.
 *
 * \param RxF    Receive Frequency
 * \param RxSmp  Receive Sample Rate
 * \param RxPre  Receive Preamp
 * 
 */

hpsdr_hermesNB::hpsdr_hermesNB (int RxF, int RxSmp, int RxPre)
	: gr_block ("hermesNB",
		gr_make_io_signature (1,1, sizeof (gr_complex)),  // input stream signature (min one, max one input)
		gr_make_io_signature (1,1, sizeof (gr_complex)))  // output stream signature (min one, max one output)
{
	Hermes = new HermesProxy(RxF);	// Create proxy, do Hermes ethernet discovery
	Hermes->RxSampleRate = RxSmp;
	Hermes->RxPreamp = RxPre;
}


void hpsdr_hermesNB::set_ReceiveFrequency (float RxF) // callback to allow slider to set frequency
{
	Hermes->ReceiveFrequency = (unsigned)RxF;	// slider must be of type real, convert to unsigned
	Hermes->UpdateHermes();			// FIXME: temporary until have TxData-> Hermes coded
}

void hpsdr_hermesNB::set_RxSampRate(int RxSmp)	// callback to set RxSampleRate
{
	Hermes->RxSampleRate = RxSmp;
	Hermes->UpdateHermes();
}

void hpsdr_hermesNB::set_RxPreamp(int RxPre)	// callback to set RxPreamp on or off
{
	Hermes->RxPreamp = (bool)RxPre;
	Hermes->UpdateHermes();
}

void hpsdr_hermesNB::set_PTTMode(int PTTmode)	// callback to set PTTMode (Off, Vox, On)
{
	Hermes->PTTMode = PTTmode;
	Hermes->UpdateHermes();
}

void hpsdr_hermesNB::set_PTTOffMutesTx(int PTTTx)	// callback to set PTTOffMmutesTx (Off, On)
{
	Hermes->PTTOffMutesTx = PTTTx;
	Hermes->UpdateHermes();
}

void hpsdr_hermesNB::set_PTTOnMutesRx(int PTTRx)	// callback to set PTTOnMutesRx (Off, On)
{
	Hermes->PTTOnMutesRx = PTTRx;
	Hermes->UpdateHermes();
}
 
void hpsdr_hermesNB::set_TxDrive(int TxD)	// callback to set Transmit Drive Level (0..255)
{
	Hermes->TxDrive = TxD;
	Hermes->UpdateHermes();
}



hpsdr_hermesNB::~hpsdr_hermesNB()
{
	delete Hermes;
}


int
hpsdr_hermesNB::general_work (int noutput_items,
			       gr_vector_int &ninput_items,
			       gr_vector_const_void_star &input_items,
			       gr_vector_void_star &output_items)
{
  const gr_complex *in = (const gr_complex *) input_items[0];
  gr_complex *out = (gr_complex *) output_items[0];

  // NOTE:  noutput_items is always less_than_or_equal_to ninput_items.
  // NOTE:  input_items[0..n] are the input streams.
  // NOTE:  ninput_items[0..n] is the input item count for each input stream.
  // NOTE:  All output streams must have the same number of items (noutput_items).
  // NOTE:  Each input stream can have a different number of items (ninput_items[stream#]).
  // NOTE:  input_items.size() is the number of input streams.
  // NOTE:  Do not consume more than ninput_items[stream#] from input stream#.
  // NOTE:  Do not put more than noutput_items to the output.

  //for (int i=0; i< noutput_items; i++)
  //  out[i] = in[i] * d_p1 + d_p2;

  
  // We always get 128 I and 128 Q samples per request from HermesProxy, interleaved
  // See how many 128 sample buffers we can send to Gnuradio

  IQBuf_t Rx;
  int CanSendBuffers = noutput_items / 128;

  //fprintf(stderr, "noutput_items = %d   CanSendBuffers = %d  ninput_items = %d  ", noutput_items, CanSendBuffers, ninput_items[0]);

  int BufCount;

  for( BufCount=0; BufCount<CanSendBuffers; BufCount++)
  {
    if( (Rx = Hermes->GetRxIQ()) == NULL)	//no more available from the radio
      break; 					
    
    for(int j=0; j<128; j++)			// get 128 complex samples from Hermes
      out[j] = gr_complex(*Rx++, *Rx++);

  }

  // Send I and Q samples received on input port to HermesProxy, it may or may not
  // consume them. Hermes needs 63 complex samples in each HPSDR-USB frame.

  if (ninput_items[0] >= 63)
  {
    int consumed = Hermes->PutTxIQ(in, 63);
    consume_each(consumed);  	// Tell runtime system how many input items we consumed on
  				// each input stream.
  };

  //fprintf(stderr, "BufCount = %d\n", BufCount);

  return(BufCount*128);  	// Tell runtime system how many output items we produced
}

