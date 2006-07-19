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
#ifndef INCLUDED_HPSDR_I2C_ADDR_H
#define INCLUDED_HPSDR_I2C_ADDR_H

// I2C addresses

#define I2C_DEV_EEPROM	0x50		// 24LC128:  7-bits 1010xxx

#define	I2C_ADDR_BOOT	(I2C_DEV_EEPROM	 | 0x1)

// format of FX2 BOOT EEPROM
//	00: 0xC0	code for ``Read IDs from EEPROM''
//	01: 0xFE	USB Vendor ID (LSB)
//	02: 0xFF	USB Vendor ID (MSB)
//	03: 0x07	USB Product ID (LSB)
//	04: 0x00	USB Product ID (MSB)
//	05: 0x01	USB Device ID (LSB)	// rev1
//	06: 0x00	USB Device ID (MSB)	// 0 = unconfig'd (no firmware)
//	07: 0x00	option byte

#endif /* INCLUDED_HPSDR_I2C_ADDR_H */

