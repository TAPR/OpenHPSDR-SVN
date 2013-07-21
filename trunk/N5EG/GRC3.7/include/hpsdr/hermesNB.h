/* -*- c++ -*- */
/* 
 * Copyright 2013 <+YOU OR YOUR COMPANY+>.
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


#ifndef INCLUDED_HPSDR_HERMESNB_H
#define INCLUDED_HPSDR_HERMESNB_H

#include <hpsdr/api.h>
#include <gnuradio/block.h>

namespace gr {
  namespace hpsdr {

    /*!
     * \brief <+description of block+>
     * \ingroup hpsdr
     *
     */
    class HPSDR_API hermesNB : virtual public gr::block
    {
     public:
      typedef boost::shared_ptr<hermesNB> sptr;

      /*!
       * \brief Return a shared_ptr to a new instance of hpsdr::hermesNB.
       *
       * To avoid accidental use of raw pointers, hpsdr::hermesNB's
       * constructor is in a private implementation
       * class. hpsdr::hermesNB::make is the public interface for
       * creating new instances.
       */
      static sptr make(int RxF, int RxSmp, int RxPre,
	 const char* Intfc, const char * ClkS, const char * AlexC, int NumRx);

      void set_Receive0Frequency(float);	// callback
      void set_Receive1Frequency(float);	// callback
      void set_TransmitFrequency(float);	// callback
      void set_RxSampRate(int);			// callback
      void set_RxPreamp(int);			// callback
      void set_PTTMode(int);			// callback
      void set_PTTOffMutesTx(int);		// callback
      void set_PTTOnMutesRx(int);		// callback
      void set_TxDrive(int);			// callback
      void set_ClockSource(const char *);	// callback
      void set_AlexControl(const char *);	// callback

      bool stop();				// override
      bool start();				// override

    };

  } // namespace hpsdr
} // namespace gr

#endif /* INCLUDED_HPSDR_HERMESNB_H */

