/*
 * QuickSilver QS1R Firmware
 *
 * Adapted from USRP firmware 12/07/2006 by Phil Covington N8VB
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

#ifndef _QS1R_COMMANDS_H_
#define _QS1R_COMMANDS_H_

#include "qs1r_interfaces.h"
#include "qs1r_spi_defs.h"

#define	MAX_EP0_PKTSIZE		       64	// max size of EP0 packet on FX2

// ----------------------------------------------------------------
//			Vendor bmRequestType's
// ----------------------------------------------------------------

#define	VRT_VENDOR_IN			0xC0
#define	VRT_VENDOR_OUT		0x40

// ----------------------------------------------------------------
//			  QS1R Vendor Requests
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

#define VRQ_SN_READ      0x83

#define VRQ_EEPROM_TYPE_READ  0x84
#define VRQ_I2C_SPEED_READ      0x85
#define VRQ_DEBUG_LED_READ   0x86
#define VRQ_LTC2208_CNTRL_READ 0x87
#define VRQ_ATTN_READ 0x88
#define VRQ_RFFE_READ 0x89
#define VRQ_DDC_CNTRL_READ 0x8A
#define VRQ_CPLD_GPIO_READ 0x8B
#define VRQ_DDC0_DR0_READ 0x8C
#define VRQ_DDC0_DR1_READ 0x8D
#define VRQ_DDC0_DR2_READ 0x8E
#define VRQ_DDC0_DR3_READ 0x8F
#define VRQ_DDC0_DR4_READ 0x90
#define VRQ_DDC0_LAR_READ 0x91
#define VRQ_DDC0_AMR_READ 0x92
#define VRQ_DDC1_DR0_READ 0x93
#define VRQ_DDC1_DR1_READ 0x94
#define VRQ_DDC1_DR2_READ 0x95
#define VRQ_DDC1_DR3_READ 0x96
#define VRQ_DDC1_DR4_READ 0x97
#define VRQ_DDC1_LAR_READ 0x98
#define VRQ_DDC1_AMR_READ 0x99

#define VRQ_DDC_MCR_READ 0x9A
#define VRQ_DDC_NCR_READ 0x9B
#define VRQ_DDC_NSCR_READ 0x9C
#define VRQ_DDC_NFRE_READ 0x9D
#define VRQ_DDC_NPO_READ 0x9E
#define VRQ_DDC_C2SR_READ 0x9F
#define VRQ_DDC_C2DEC_READ 0xA0
#define VRQ_DDC_C5SR_READ 0xA1
#define VRQ_DDC_C5DEC_READ 0xA2
#define VRQ_DDC_RCR_READ 0xA3
#define VRQ_DDC_RDEC_READ 0xA4
#define VRQ_DDC_RAO_READ 0xA5
#define VRQ_DDC_RTAPS_READ 0xA6
#define VRQ_DDC_RSVR_READ 0xA7
#define VRQ_DDC_COEF_READ 0xA8

// OUT commands

#define   VRQ_DEBUG_LED_SET     0x01  // wValueL: {0-255}

#define	VRQ_FPGA_LOAD			0x02
#define	FL_BEGIN				0	// wIndexL: begin fpga programming cycle.  stalls if trouble.
#define	FL_XFER					1	// wIndexL: xfer up to 64 bytes of data
#define	FL_END					2	// wIndexL: end programming cycle, check for success.
									//stalls endpoint if trouble.

#define VRQ_FPGA_SET_RESET		0x04		// wValueL: {0,1}

#define	VRQ_I2C_WRITE			0x08		// wValueL: i2c address; data: data

#define	VRQ_SPI_WRITE			0x09		// wValue: optional header bytes
                                                                        // wIndexH:	enables
                                                                        // wIndexL:	format
                                                                        // len: how much to write

#define VRQ_LTC2208_CNTRL_SET   0x0A // wValueL: {0-255}

#define VRQ_I2C_SPEED_SET  0x0B  // wValueL: {0,1}

#define VRQ_CPU_SPEED_SET   0x0C // wValueL: {0, 1, 2}

#define VRQ_ATTN_SET 0x0D // wValueL: {0-255}

#define VRQ_RFFE_SET 0x0E //  wValueL: {0-255}

#define VRQ_DDC_CNTRL_SET 0x0F

#define VRQ_CPLD_GPIO_SET 0x10

#define VRQ_DDC0_DR0_SET 0x11
#define VRQ_DDC0_DR1_SET 0x12
#define VRQ_DDC0_DR2_SET 0x13
#define VRQ_DDC0_DR3_SET 0x14
#define VRQ_DDC0_DR4_SET 0x15
#define VRQ_DDC0_LAR_SET 0x16
#define VRQ_DDC0_AMR_SET 0x17
#define VRQ_DDC1_DR0_SET 0x18
#define VRQ_DDC1_DR1_SET 0x19
#define VRQ_DDC1_DR2_SET 0x1A
#define VRQ_DDC1_DR3_SET 0x1B
#define VRQ_DDC1_DR4_SET 0x1C
#define VRQ_DDC1_LAR_SET 0x1D
#define VRQ_DDC1_AMR_SET 0x1E

#define VRQ_DDC_MCR_SET 0x1F
#define VRQ_DDC_NCR_SET 0x20
#define VRQ_DDC_NSCR_SET 0x21
#define VRQ_DDC_NFRE_SET 0x22
#define VRQ_DDC_NPO_SET 0x23
#define VRQ_DDC_C2SR_SET 0x24
#define VRQ_DDC_C2DEC_SET 0x25
#define VRQ_DDC_C5SR_SET 0x26
#define VRQ_DDC_C5DEC_SET 0x27
#define VRQ_DDC_RCR_SET 0x28
#define VRQ_DDC_RDEC_SET 0x29
#define VRQ_DDC_RAO_SET 0x2A
#define VRQ_DDC_RTAPS_SET 0x2B
#define VRQ_DDC_RSVR_SET 0x2C
#define VRQ_DDC_RAM_CLR 0x2D
#define VRQ_DDC_COEF_CLR 0x2E
#define VRQ_DDC_COEF_SET 0x2F

#endif /* _QS1R_COMMANDS_H_ */
