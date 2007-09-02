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


extern int HPSDR_WriteRAM(usb_dev_handle *devh, int fx2_start_addr, char *bufp, int count);

extern struct usb_device *HPSDR_findDevice(struct usb_bus *bus_rootp, 
		 							 unsigned int vid, 
		 							 unsigned int pid);


extern int HPSDR_ResetCPU(usb_dev_handle *devh, int reset); 


// returns 0 on error 
extern int HPSDR_UploadFirmware(usb_dev_handle *devh, char *fnamep);


#endif /*HPSDRUSB_H_*/
