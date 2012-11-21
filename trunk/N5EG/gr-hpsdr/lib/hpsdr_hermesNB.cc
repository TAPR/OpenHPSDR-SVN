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
 * \param param1  Receive Frequency
 * \param param2  Receive Sample Rate
 * \param param3  Receive Preamp
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
  // NOTE:  Do not consume more than ninput_items from the input.
  // NOTE:  Do not put more than noutput_items to the output.

  //for (int i=0; i< noutput_items; i++)
  //  out[i] = in[i] * d_p1 + d_p2;

  
  // we always get 128 I and 128 Q samples per request from HermesProxy, interleaved
  // See how many 128 sample buffers we can send to Gnuradio

  IQBuf_t Rx;
  int CanSendBuffers = noutput_items / 128;

  //fprintf(stderr, "noutput_items = %d   CanSendBuffers = %d  ninput_items = %d  ", noutput_items, CanSendBuffers, ninput_items[0]);

  int BufCount;

  for( BufCount=0; BufCount<CanSendBuffers; BufCount++)
  {
    if( (Rx = Hermes->GetRxIQ()) == NULL)	//no more available from the radio
      break; 					
    
    for(int j=0; j<128; j++)			// produce 128 complex samples
      out[j] = gr_complex(*Rx++, *Rx++);

  }

  //fprintf(stderr, "BufCount = %d\n", BufCount);

  // Tell runtime system how many input items we consumed on
  // each input stream.


  //fprintf(stderr, "input_items.size(): %d    ninput_items.size(): %d", input_items.size(), ninput_items.size());
 
  //if (input_items.size() > 0)	// if we have any input ports...
  //consume_each (BufCount*128);  // just throw the samples away (not even needed here...)

  return(BufCount*128);  	// Tell runtime system how many output items we produced

}

