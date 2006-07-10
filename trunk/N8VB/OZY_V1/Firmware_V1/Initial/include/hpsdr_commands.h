/*
 * HPSDR/OZY - High Performance Software Defined Radio, OZY Firmware
 *
 * Adapted from USRP firmware 07/10/2006 by Phil Covington N8VB
 *
 * Copyright (C) 2003,2004 Free Software Foundation, Inc.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

#ifndef _HPSDR_COMMANDS_H_
#define _HPSDR_COMMANDS_H_

#include <hpsdr_interfaces.h>
#include <hpsdr_spi_defs.h>

#define	MAX_EP0_PKTSIZE		       64	// max size of EP0 packet on FX2

// ----------------------------------------------------------------
//			Vendor bmRequestType's
// ----------------------------------------------------------------

#define	VRT_VENDOR_IN			0xC0
#define	VRT_VENDOR_OUT		0x40

// ----------------------------------------------------------------
//			  HPSDR Vendor Requests
//
// Note that Cypress reserves [0xA0,0xAF].
// 0xA0 is the firmware load function.
// ----------------------------------------------------------------


// IN commands

#define	VRQ_I2C_READ			0x81		// wValueL: i2c address; length: how much to read

#define	VRQ_SPI_READ			0x82		// wValue: optional header bytes
											// wIndexH:	enables
											// wIndexL:	format
											// len: how much to read

// OUT commands

#define	VRQ_SET_LED			0x01		// wValueL off/on {0,1}; wIndexL: which {0,1}

#define	VRQ_FPGA_LOAD			0x02
#define	FL_BEGIN				0	// wIndexL: begin fpga programming cycle.  stalls if trouble.
#define	FL_XFER					1	// wIndexL: xfer up to 64 bytes of data
#define	FL_END					2	// wIndexL: end programming cycle, check for success.
									//stalls endpoint if trouble.

#define	VRQ_I2C_WRITE			0x08		// wValueL: i2c address; data: data

#define	VRQ_SPI_WRITE			0x09		// wValue: optional header bytes
							// wIndexH:	enables
							// wIndexL:	format
							// len: how much to write

#endif /* _HPSDR_COMMANDS_H_ */
