/* -*- c++ -*- */
/* 
 * Copyright 2013 Thomas C. McDermott, N5EG.
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

#ifndef INCLUDED_HPSDR_HERMESNB_IMPL_H
#define INCLUDED_HPSDR_HERMESNB_IMPL_H

#include <hpsdr/hermesNB.h>

namespace gr {
  namespace hpsdr {

    class hermesNB_impl : public hermesNB
    {
     private:
      // Nothing to declare in this block.

     public:

/*! \brief Construct a HermesNB module
 * \param RxF    Receive frequency, Hz.
 * \param RxSmp  Receive Sample Rate, 192000, 96000, or 48000
 * \param RxPre  Rx Preamp on (1) / off (0)
 * \param Intfc  Ethernet interface to use
 * \param ClkS   HPSDR Clock Source (register C1 when C0 = 0x00)
 * \param AlexC  HPSDR Alex Control (register (MSB)C3, C4 for C0=0 and C3, C4(LSB) for C0 = 0x12)
 * \param NumRx  Number of Receivers (1 or 2)
 *
 */
      hermesNB_impl(int RxF, int RxSmp, int RxPre,
	 const char* Intfc, const char * ClkS, const char * AlexC, int NumRx);
      ~hermesNB_impl();

      // Where all the action really happens
      void forecast (int noutput_items, gr_vector_int &ninput_items_required);

      int general_work(int noutput_items,
		       gr_vector_int &ninput_items,
		       gr_vector_const_void_star &input_items,
		       gr_vector_void_star &output_items);
    };

  } // namespace hpsdr
} // namespace gr

#endif /* INCLUDED_HPSDR_HERMESNB_IMPL_H */

