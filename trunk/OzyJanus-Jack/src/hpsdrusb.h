/* HPSDR-USB - High Performance Software Defined Radio, USB support 
 *  
 * Copyright (C) 2007 Bill Tracey, KD5TFD  
 * Copyright (C) 2006 Philip A. Covington, N8VB
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


/* 
 * This is an adapatation of Phil Covingtons' C# USB support routines to C for 
 * use on Linux
 * 
 * Author: Bill Tracey (bill@ewjt.com) 
 */


#ifndef HPSDRUSB_H_
#define HPSDRUSB_H_


#define MAX_EPO_PACKET_SIZE (64)
#define USB_TIMEOUT_MSECS (1000)


#define MAX_EP0_PACKETSIZE (64)

        /* OZY VID PID */
#define OZY_VID (0xFFFE)
#define OZY_PID (0x0007)

        /* Vendor Request Types */
#define VENDOR_REQ_TYPE_IN (0xc0)
#define VENDOR_REQ_TYPE_OUT (0x40)

        /* Vendor In Commands */
#define VENDOR_REQ_I2C_READ (0x81)    // wValueL: i2c address
#define VENDOR_REQ_SPI_READ (0x82)    // wValue: optional header bytes
                                                        // wIndexH:	enables
                                                        // wIndexL:	format
                                                        // len: how much to read
#define SPI_FMT_MSB ((0)
#define SPI_FMT_HDR_0 ((0)
#define SPI_FMT_HDR_1 ((1)
#define SPI_FMT_HDR_2 ((2)
#define SPI_EN_FPGA (0x80)

#define VENDOR_REQ_RS232_READ (0x83)
#define VENDOR_REQ_EEPROM_TYPE_READ (0x84)
#define VENDOR_REQ_I2C_SPEED_READ (0x85)

        /* Vendor Out Commands */
#define VENDOR_REQ_SET_LED (0x01)     // wValueL {0,255}
#define VENDOR_REQ_FPGA_LOAD (0x02)
#define FL_BEGIN (0)	                // wIndexL: begin fpga programming cycle.  stalls if trouble.
#define FL_XFER (1)	                // wIndexL: xfer up to 64 bytes of data
#define FL_END (2)	                // wIndexL: end programming cycle, check for success.
									                    // stalls endpoint if trouble.

#define VENDOR_REQ_I2C_WRITE (0x08)	// wValueL: i2c address

#define VENDOR_REQ_SPI_WRITE (0x09)	// wValue: optional header bytes
							                            // wIndexH:	enables
							                            // wIndexL:	format
							                            // len: how much to write

#define VENDOR_REQ_I2C_SPEED_SET (0x0B) // wValueL 100kHz/400kHz {0,1}

#define VENDOR_REQ_CPU_SPEED_SET (0x0C) // wValueL 100kHz/400kHz {0,1,3}







extern int HPSDR_SetLED(usb_dev_handle *devh, int which, int on);

extern int HPSDR_LoadFPGA(usb_dev_handle *devh, char *rbf_fnamep);

extern int HPSDR_WriteRAM(usb_dev_handle *devh, int fx2_start_addr, char *bufp, int count);

extern struct usb_device *HPSDR_findDevice(struct usb_bus *bus_rootp, 
		 							 unsigned int vid, 
		 							 unsigned int pid);


extern int HPSDR_ResetCPU(usb_dev_handle *devh, int reset); 


// returns 0 on error 
extern int HPSDR_UploadFirmware(usb_dev_handle *devh, char *fnamep);


#endif /*HPSDRUSB_H_*/
