/* -*- c++ -*- */
/* 
 * Copyright 2012 Thomas C. McDermott, N5EG.
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

//
// Version: November 16, 2012


#ifndef INCLUDED_HPSDR_HERMESNB_H
#define INCLUDED_HPSDR_HERMESNB_H

#include <hpsdr_api.h>
#include <gr_block.h>



class hpsdr_hermesNB;
typedef boost::shared_ptr<hpsdr_hermesNB> hpsdr_hermesNB_sptr;

/*! \brief Make a HermesNB module
 * \param param1  Receive frequency, Hz.
 * \param param2  Receive Sample Rate, 192000, 96000, or 48000
 * \param param3  Rx Preamp on (1) / off (0)
 */

HPSDR_API hpsdr_hermesNB_sptr hpsdr_make_hermesNB (int param1, int param2, int param3);

/*!
 * \brief Prototype for Hermes module
 *
 * \details
 * Initial prototype to allow recieving IQ data from Hermes
 * Supports callbacks which allow sliders to change values at runtime.
 *
 * \param param1  Receive frequency, Hz.
 * \param param2  Receive Sample Rate, 192000, 96000, or 48000
 * \param param3  Rx Preamp on (1) / off (0)
 * 
 */
class HPSDR_API hpsdr_hermesNB : public gr_block
{
	friend HPSDR_API hpsdr_hermesNB_sptr hpsdr_make_hermesNB (int param1, int param2, int param3);

	hpsdr_hermesNB (int param1, int param2, int param3);

 public:
	~hpsdr_hermesNB();
	void set_ReceiveFrequency(float);	// callback
	void set_RxSampRate(int);		// callback
	void set_RxPreamp(int);		// callback

  int general_work (int noutput_items,
		    gr_vector_int &ninput_items,
		    gr_vector_const_void_star &input_items,
		    gr_vector_void_star &output_items);
};

#endif /* INCLUDED_HPSDR_HERMESNB_H */

