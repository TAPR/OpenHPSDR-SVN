/* -*- c++ -*- */
/* 
 * Copyright 2013 - 2015 Thomas C. McDermott, N5EG
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

// -----------------------------------------------------------------
// Additions for ALEX friendly registers 03/01/2015
// On the alex branch.
// -----------------------------------------------------------------

#ifdef HAVE_CONFIG_H
#include "config.h"
#endif

#include <gnuradio/io_signature.h>
#include "hermesWB_impl.h"

#include "HermesProxyW.h"
#include <stdio.h>	// for DEBUG PRINTF's


HermesProxyW* HermesW;	// make it visible to metis.cc

namespace gr {
  namespace hpsdr {

    hermesWB::sptr
    hermesWB::make(bool RxPre, const char* Intfc, const char * ClkS,
		   int AlexRA, int AlexTA, int AlexHPF, int AlexLPF,
		   const char* MACAddr)
    {
      return gnuradio::get_initial_sptr
        (new hermesWB_impl(RxPre, Intfc, ClkS, AlexRA, AlexTA, AlexHPF, AlexLPF, MACAddr));
    }

    /*
     * The private constructor
     */
    hermesWB_impl::hermesWB_impl(bool RxPre, const char* Intfc, const char * ClkS,
			 int AlexRA, int AlexTA, int AlexHPF, int AlexLPF,
			 const char* MACAddr)
      : gr::block("hermesWB",
              gr::io_signature::make(0, 0, 0),				// No inputs to hermesWB block
              gr::io_signature::make(1, 1, 16384 * sizeof(float)) )	// output from hermesWB block
    {
	HermesW = new HermesProxyW(RxPre, Intfc, ClkS, AlexRA, AlexTA, AlexHPF, AlexLPF, MACAddr);	// Create proxy, do Hermes ethernet discovery
    }

    /*
     * Our virtual destructor. With Ver 3.9, never gets called.
     */
    hermesWB_impl::~hermesWB_impl()
    {
	// delete HermesW;
    }



bool hermesWB::stop()		// override base class
    {
	HermesW->Stop();		// stop ethernet activity on Hermes
	delete HermesW;			// print stats, dispose buffers.
	return gr::block::stop();	// call base class stop()
    }

bool hermesWB::start()		// override base class
    {
	HermesW->Start();		// start rx stream on Hermes
	return gr::block::start();	// call base class start()
    }


void hermesWB::set_RxPreamp(int RxPre)	// callback to set RxPreamp on or off
    {
	HermesW->RxPreamp = (bool)RxPre;
    }

void hermesWB::set_ClockSource(const char * ClkS)	// callback to set Clock source
    {
	unsigned int ck;
	sscanf(ClkS, "%x", &ck);   	// convert char string to 8 bits
	ck &= 0xFC;			// mask lower bits
	HermesW->ClockSource = ck;
    }

void hermesWB::set_AlexRxAntenna(int RxA)		// callback to set Alex Rx Antenna Selector
{
	HermesW->AlexRxAnt = RxA;
}

void hermesWB::set_AlexTxAntenna(int TxA)		// callback to set Alex Tx Antenna Selector
{
	HermesW->AlexTxAnt = TxA;
}

void hermesWB::set_AlexRxHPF(int HPF)		// callback to select Alex Rx High Pass Filter
{
	HermesW->AlexRxHPF = HPF;
}

void hermesWB::set_AlexTxLPF(int LPF)		// callback to set Alex Tx Low Pass filter
{
	HermesW->AlexTxLPF = LPF;
}



void hermesWB_impl::forecast (int noutput_items, gr_vector_int &ninput_items_required)
    {
        /* <+forecast+> e.g. ninput_items_required[0] = noutput_items */
    }

int hermesWB_impl::general_work (int noutput_items,
                       gr_vector_int &ninput_items,
                       gr_vector_const_void_star &input_items,
                       gr_vector_void_star &output_items)
    {

       float *out0 = (float *) output_items[0];		// WB Rcvr samples
    
  // We always get 256 Real samples per USB frame (Read buffer) from HermesProxyW
  //
  // We need to send 16,384 floats to gnuradio as one item ( a vector).
  // This requires aligning to the 64 receive buffers belonging to the same
  // time sequence. If not aligned, then throw away buffers until aligned.
  //

	if (!HermesW->RxReadBufAligned())  	// not aligned - we have a problem
     	  for (int i=0; i<63; i++)
	  {
	    if (HermesW->RxBufFillCount() == 0) // we're out of buffers, do nothing
		return 0;	    
	    IQBuf_t dummy = HermesW->GetNextRxReadBuf();  // consume a buffer
	    if (HermesW->RxReadBufAligned())
		return 0;
	  }
  	if (HermesW->RxBufFillCount() < 64)	// aligned but not enough buffers, do nothing
	  return 0;

   // aligned and have enough Read buffers - emit one complete vector to out0[]

	IQBuf_t ReadBuf = HermesW->GetCurrentRxReadBuf();
	IQBuf_t out = out0;

	for (int i=0; i<64; i++)
	{
//	  for (int j=0; j<256; j++)
//	    out0[i*256+j] = ReadBuf[j];
	  memcpy(out, ReadBuf, 256*sizeof(float));
	  out += 256;  
	  ReadBuf = HermesW->GetNextRxReadBuf();
	}
	return(1);


    }	// general_work

  } /* namespace hpsdr */
} /* namespace gr */

