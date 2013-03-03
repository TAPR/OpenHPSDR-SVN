/*
This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2007, 2008 Philip A Covington

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

The author can be reached by email at

p.covington@gmail.com

*/

#ifndef _OZYCOMMANDS_H
#define	_OZYCOMMANDS_H

#ifdef	__cplusplus
extern "C" {
#endif

#define MAX_EP0_PACKET_SIZE 	64
    
/* Vendor Request Types */   
#define VRT_VENDOR_IN       	0xC0
#define VRT_VENDOR_OUT      	0x40
#define VRQ_SDR1K_CTL           0x0d
#define SDR1KCTRL_READ_VERSION  0x7

/* Vendor In Commands */
#define	VRQ_I2C_READ		0x81	// wValueL: i2c address; length: how much to read
#define	VRQ_SPI_READ		0x82	// wValue: optional header bytes
					// wIndexH:	enables
					// wIndexL:	format
					// len: how much to read

#define VRQ_SN_READ     	0x83

#define VRQ_EEPROM_TYPE_READ	0x84
#define VRQ_I2C_SPEED_READ      0x85
#define VRQ_MULTI_READ          0x86

/* Vendor Out Commands */
#define VRQ_FPGA_LOAD       	0x02
#define FL_BEGIN            	0
#define FL_XFER             	1
#define FL_END              	2

#define VRQ_FPGA_SET_RESET  	0x04	// wValueL: {0,1}
#define VRQ_MULTI_WRITE     	0x05
#define VRQ_REQ_I2C_WRITE  	0x08	// wValueL: i2c address; data: data
#define VRQ_REQ_SPI_WRITE 	0x09	// wValue: optional header bytes
					// wIndexH:	enables
					// wIndexL:	format
					// len: how much to write
														
#define VRQ_I2C_SPEED_SET  	0x0B  	// wValueL: {0,1}
#define VRQ_CPU_SPEED_SET   0x0C 	// wValueL: {0, 1, 2}

#ifdef	__cplusplus
}
#endif

#endif	/* _OZYCOMMANDS_H */

