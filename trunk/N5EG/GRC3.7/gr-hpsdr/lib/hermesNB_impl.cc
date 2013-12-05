/* -*- c++ -*- */
/* 
 * Copyright 2013 Thomas C. McDermott, N5EG
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

#include <gnuradio/io_signature.h>
#include "hermesNB_impl.h"

#include "HermesProxy.h"
#include <stdio.h>	// for DEBUG PRINTF's

HermesProxy* Hermes;	// make it visible to metis.cc


namespace gr {
  namespace hpsdr {

    hermesNB::sptr
    hermesNB::make(int RxFreq0, int RxFreq1, int TxFreq, bool RxPre,
			 int PTTModeSel, bool PTTTxMute, bool PTTRxMute,
			 unsigned char TxDr, int RxSmp, const char* Intfc, 
			 const char * ClkS, const char * AlexC, int NumRx)
    {
      return gnuradio::get_initial_sptr
        (new hermesNB_impl(RxFreq0, RxFreq1, TxFreq, RxPre, PTTModeSel, PTTTxMute,
			PTTRxMute, TxDr, RxSmp, Intfc,  ClkS,  AlexC, NumRx));
    }

    /*
     * The private constructor
     */
    hermesNB_impl::hermesNB_impl(int RxFreq0, int RxFreq1, int TxFreq, bool RxPre,
			 int PTTModeSel, bool PTTTxMute, bool PTTRxMute,
			 unsigned char TxDr, int RxSmp, const char* Intfc, 
			 const char * ClkS, const char * AlexC, int NumRx)
      : gr::block("hermesNB",
              gr::io_signature::make(1, 1, sizeof(gr_complex)),		// inputs to hermesNB block
              gr::io_signature::make(1, 2, sizeof(gr_complex)) )	// outputs from hermesNB block
    {
	Hermes = new HermesProxy(RxFreq0, RxFreq1, TxFreq, RxPre, PTTModeSel, PTTTxMute,
		 PTTRxMute, TxDr, RxSmp, Intfc, ClkS, AlexC, NumRx);	// Create proxy, do Hermes ethernet discovery
	//Hermes->RxSampleRate = RxSmp;
	//Hermes->RxPreamp = RxPre;

	gr::block::set_output_multiple(256);		// process outputs in groups of at least 256 samples
	//gr::block::set_relative_rate((double) NumRx);	// FIXME - need to also account for Rx sample rate

    }

    /*
     * Our virtual destructor.
     */
    hermesNB_impl::~hermesNB_impl()
    {
	delete Hermes;
    }



bool hermesNB::stop()		// override base class
    {
	Hermes->Stop();			// stop ethernet activity on Hermes
	return gr::block::stop();	// call base class stop()
    }

bool hermesNB::start()		// override base class
    {
	Hermes->Start();		// start rx stream on Hermes
	return gr::block::start();	// call base class start()
    }

void hermesNB::set_Receive0Frequency (float Rx0F) // callback to allow slider to set frequency
    {
	Hermes->Receive0Frequency = (unsigned)Rx0F;	// slider must be of type real, convert to unsigned
    }

void hermesNB::set_Receive1Frequency (float Rx1F) // callback to allow slider to set frequency
    {
	Hermes->Receive1Frequency = (unsigned)Rx1F;	// slider must be of type real, convert to unsigned
    }

void hermesNB::set_TransmitFrequency (float TxF) // callback to allow slider to set frequency
    {
	Hermes->TransmitFrequency = (unsigned)TxF;	// slider must be of type real, convert to unsigned
    }

void hermesNB::set_RxSampRate(int RxSmp)	// callback to set RxSampleRate
    {
	Hermes->RxSampleRate = RxSmp;
    }

void hermesNB::set_RxPreamp(int RxPre)	// callback to set RxPreamp on or off
    {
	Hermes->RxPreamp = (bool)RxPre;
    }

void hermesNB::set_PTTMode(int PTTmode)	// callback to set PTTMode (Off, Vox, On)
    {
	Hermes->PTTMode = PTTmode;
    }

void hermesNB::set_PTTOffMutesTx(int PTTTx)	// callback to set PTTOffMmutesTx (Off, On)
    {
	Hermes->PTTOffMutesTx = PTTTx;
    }

void hermesNB::set_PTTOnMutesRx(int PTTRx)	// callback to set PTTOnMutesRx (Off, On)
    {
	Hermes->PTTOnMutesRx = PTTRx;
    }
 
void hermesNB::set_TxDrive(int TxD)	// callback to set Transmit Drive Level (0..255)
    {
	Hermes->TxDrive = (unsigned char)TxD;
    }

void hermesNB::set_ClockSource(const char * ClkS)	// callback to set Clock source
    {
	unsigned int ck;
	sscanf(ClkS, "%x", &ck);   	// convert char string to 8 bits
	ck &= 0xFC;			// mask lower bits
	Hermes->ClockSource = ck;
    }

void hermesNB::set_AlexControl(const char * AlexC)  // callback to set Alex Control Word
    {
	unsigned int ac;
	sscanf(AlexC, "%x", &ac);   	// convert char string to 32 bits
	Hermes->AlexControl = ac;
    }


void hermesNB_impl::forecast (int noutput_items, gr_vector_int &ninput_items_required)
    {
        /* <+forecast+> e.g. ninput_items_required[0] = noutput_items */
    }

int hermesNB_impl::general_work (int noutput_items,
                       gr_vector_int &ninput_items,
                       gr_vector_const_void_star &input_items,
                       gr_vector_void_star &output_items)
    {

       const gr_complex *in0 = (const gr_complex *) input_items[0];	// Tx samples
 //      const gr_complex *in1 = (const gr_complex *) input_items[1];	// Audio output samples

       gr_complex *out0 = (gr_complex *) output_items[0];		// Rcvr 0 samples
    
       gr_complex *out1;						// Rcvr 1 samples
       if (output_items.size() == 2)
	 out1 = (gr_complex *) output_items[1];

  // We always get 128 I and 128 Q samples per request from HermesProxy, interleaved
  // See how many 128 sample buffers we can send to Gnuradio

       IQBuf_t Rx;
       int CanSendBuffers;

       if(output_items.size() == 1)
         CanSendBuffers = noutput_items / 128;	// 128 samples per buffer - 1 Rcvr
       else
         CanSendBuffers = noutput_items / 64;	//  64 samples per buffer - 2 Rcvrs


//  fprintf(stderr, "noutput_items = %d   CanSendBuffers = %d  ninput_items = %d  output_items.size = %d\n", noutput_items, CanSendBuffers, ninput_items[0], output_items.size());

       int BufCount;					// # of 256-byte buffers (regardless of format)

       for( BufCount=0; BufCount<CanSendBuffers; BufCount++)
       {
         if( (Rx = Hermes->GetRxIQ()) == NULL)	//no more available from the radio
         break; 					

         if (output_items.size() == 1)		// one receiver
           for(int j=0; j<128; j++)
             out0[(BufCount * 128) + j] = gr_complex(*Rx++, *Rx++);	// get 128 complex samples as 2 sets of 64 samples
         else
           for(int j=0; j<64; j++)			// two receivers
           {
             out0[(BufCount * 64) + j] = gr_complex(*Rx++, *Rx++);	// get 128 complex samples as 2 sets of 64 samples
             out1[(BufCount * 64) + j] = gr_complex(*Rx++, *Rx++);
           }
        }

  // Send I and Q samples received on input port to HermesProxy, it may or may not
  // consume them. Hermes needs 63 complex samples in each HPSDR-USB frame.

       if ((ninput_items[0] >= 63) /*&& (ninput_items[1] >= 63)*/)
       {
         int consumed = Hermes->PutTxIQ(in0, /*in1,*/ 63);
         consume_each(consumed); // Tell runtime system how many input items we consumed on
  				 // each input stream.
       };

  //fprintf(stderr, "BufCount = %d\n", BufCount);


       if(output_items.size() == 1)
         return(BufCount*128);  	// Tell gnuradio how many output items we produced per stream
       else
         return(BufCount*64);  	// Tell gnuradio how many output items we produced per stream

    }	// general_work

  } /* namespace hpsdr */
} /* namespace gr */

