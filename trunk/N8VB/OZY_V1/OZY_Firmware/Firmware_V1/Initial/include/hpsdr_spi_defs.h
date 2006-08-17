/*
 * HPSDR/OZY - High Performance Software Defined Radio, OZY Firmware
 *
 * Adapted from USRP firmware 07/10/2006 by Phil Covington N8VB
 *
 * Copyright 2004 Free Software Foundation, Inc.
 *
 * This file is part of GNU Radio
 *
 * GNU Radio is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * GNU Radio is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with GNU Radio; see the file COPYING.  If not, write to
 * the Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */

#ifndef INCLUDED_USRP_SPI_DEFS_H
#define INCLUDED_USRP_SPI_DEFS_H

/*
 * defines for the VRQ_SPI_READ and VRQ_SPI_WRITE commands
 *
 * SPI == "Serial Port Interface".  SPI is a 3 wire bus plus a
 * separate enable for each peripheral.  The common lines are SCLK,
 * SDI and SDO.  The FX2 always drives SCLK and SDI, the clock and
 * data lines from the FX2 to the peripheral.  When enabled, a
 * peripheral may drive SDO, the data line from the peripheral to the
 * FX2.
 *
 * The SPI_READ and SPI_WRITE commands are formatted identically.
 * Each specifies which peripherals to enable, whether the bits should
 * be transmistted Most Significant Bit first or Least Significant Bit
 * first, the number of bytes in the optional header, and the number
 * of bytes to read or write in the body.
 *
 * The body is limited to 64 bytes.  The optional header may contain
 * 0, 1 or 2 bytes.  For an SPI_WRITE, the header bytes are
 * transmitted to the peripheral followed by the the body bytes.  For
 * an SPI_READ, the header bytes are transmitted to the peripheral,
 * then len bytes are read back from the peripheral.
 */

/*
 * SPI_FMT_* goes in wIndexL
 */
#define SPI_FMT_xSB_MASK	0x80
#  define	SPI_FMT_LSB	0x80	// least signficant bit first
#  define	SPI_FMT_MSB	0x00	// most significant bit first
#define	SPI_FMT_HDR_MASK	0x60
#  define	SPI_FMT_HDR_0	0x00	// 0 header bytes
#  define	SPI_FMT_HDR_1	0x20	// 1 header byte
#  define	SPI_FMT_HDR_2	0x40	// 2 header bytes

/*
 * SPI_ENABLE_*  goes in wIndexH
 *
 * For the software interface, the enables are active high.
 * For reads, it's an error to have more than one enable set.
 *
 * [FWIW, the hardware implements them as active low.  Don't change the
 * definitions of these.  They are related to usrp_rev1_regs.h]
 */
#define	SPI_ENABLE_FPGA		0x80	// select FPGA

/*
 * If there's one header byte, it goes in wValueL.
 *
 * If there are two header bytes, they go in wValueH | wValueL.
 * The transmit order of the bytes (and bits within them) is
 * determined by SPI_FMT_*SB
 */

#endif /* INCLUDED_USRP_SPI_DEFS_H */
