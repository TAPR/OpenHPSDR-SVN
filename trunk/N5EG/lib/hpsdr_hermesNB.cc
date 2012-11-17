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
 * \param param1  the amplitude scaling factor
 * \param param2  the offset value (is added to input * scale)
 */

hpsdr_hermesNB_sptr
hpsdr_make_hermesNB (int param1, int param2, int param3)
{
    return gnuradio::get_initial_sptr(new hpsdr_hermesNB (param1, param2, param3));
}


/*!
 * \brief Prototype for Hermes module
 *
 * \details
 * Supports callbacks which allow sliders to change values at runtime.
 *
 * \param param1  the amplitude scaling factor (multiply each input by this value).
 * \param param2  the offset value (is added to input * scale)
 * 
 */

hpsdr_hermesNB::hpsdr_hermesNB (int param1, int param2, int param3)
	: gr_block ("hermesNB",
		gr_make_io_signature (1,1, sizeof (float)),	 // input stream signature
		gr_make_io_signature (1,1, sizeof (gr_complex))) // output stream signature
{
	Hermes = new HermesProxy(param1);	// Create proxy, do Hermes ethernet discovery
	Hermes->RxSampleRate = param2;
	Hermes->RxPreamp = param3;
}


void hpsdr_hermesNB::set_ReceiveFrequency (float param1) // callback to allow slider to set frequency
{
	Hermes->ReceiveFrequency = (unsigned)param1;	// slider must be of type real, convert to unsigned
	Hermes->UpdateHermes();			// FIXME: temporary until have TxData-> Hermes coded
}

void hpsdr_hermesNB::set_RxSampRate(int param2)	// callback to set RxSampleRate
{
	Hermes->RxSampleRate = param2;
	Hermes->UpdateHermes();
}

void hpsdr_hermesNB::set_RxPreamp(int param3)	// callback to set RxPreamp on or off
{
	Hermes->RxPreamp = (bool)param3;
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
  const float *in = (const float *) input_items[0];
  gr_complex *out = (gr_complex *) output_items[0];

  // NOTE:  noutput_items is always less_than_or_equal_to ninput_items.
  // NOTE:  ninput_items[0] is the first stream.
  // NOTE:  ninput_items[1..n] are additional input stream item counts if they exist.
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
  consume_each (BufCount*128);  // just throw them away

  return(BufCount*128/*noutput_items*/);  	// Tell runtime system how many output items we produced

}

