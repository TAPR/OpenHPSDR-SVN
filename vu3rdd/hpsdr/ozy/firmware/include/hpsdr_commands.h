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

#include "hpsdr_interfaces.h"
#include "hpsdr_spi_defs.h"

#define MAX_EP0_PKTSIZE                64       // max size of EP0 packet on FX2

// ----------------------------------------------------------------
//                      Vendor bmRequestType's
// ----------------------------------------------------------------

#define VRT_VENDOR_IN                   0xC0
#define VRT_VENDOR_OUT                  0x40

// ----------------------------------------------------------------
//                        HPSDR Vendor Requests
//
// Note that Cypress reserves [0xA0,0xAF].
// 0xA0 is the firmware load function.
// ----------------------------------------------------------------


// IN commands

#define VRQ_I2C_READ                    0x81            // wValueL: i2c address; length: how much to read
#define VRQ_SPI_READ                    0x82            // wValue: optional header bytes
                                                                                        // wIndexH:     enables
                                                                                        // wIndexL:     format
                                                                                        // len: how much to read

#define VRQ_RS232_READ                  0x83
#define VRQ_EEPROM_TYPE_READ            0x84
#define VRQ_I2C_SPEED_READ              0x85

// OUT commands

#define VRQ_SET_LED                     0x01            // wValueL off/on {0,1}; wIndexL: which {0,1}
#define VRQ_FPGA_LOAD                   0x02
#define FL_BEGIN                        0       // wIndexL: begin fpga programming cycle.  stalls if trouble.
#define FL_XFER                         1       // wIndexL: xfer up to 64 bytes of data
#define FL_END                          2       // wIndexL: end programming cycle, check for success.
                                                                        //stalls endpoint if trouble.

#define VRQ_FPGA_SET_RESET              0x04            // wValueL: {0,1}
#define VRQ_I2C_WRITE                   0x08            // wValueL: i2c address; data: data
#define VRQ_SPI_WRITE                   0x09            // wValue: optional header bytes
                                                        // wIndexH:     enables
                                                        // wIndexL:     format
                                                        // len: how much to write

#define VRQ_RS232_WRITE                 0x0A
#define VRQ_I2C_SPEED_SET               0x0B  // wValueL: {0,1}
#define VRQ_CPU_SPEED_SET               0x0C // wValueL: {0, 1, 2}

// SDR-1000 specific commands
#ifdef  SDR1K_CONTROL

#define VRQ_SDR1K_CTL                   0x0D

#define SDR1KCTRL_SET_DATA_REG          0x1 
#define SDR1KCTRL_LATCH                 0x2   // wIndexH - latch to set, wIndexL - value to set it to 
#define SDR1KCTRL_DDS_RESET             0x3   // no parms 
#define SDR1KCTRL_DDS_WRITE             0x4   // wIndexH - register to set, wIndexL - value to set it to 
#define SDR1KCTRL_SR_LOAD               0x5
#define SDR1KCTRL_READ_STATUS           0x6   // read status port
#define SDR1KCTRL_READ_VERSION          0x7   // read status port      

#endif 


#endif /* _HPSDR_COMMANDS_H_ */
