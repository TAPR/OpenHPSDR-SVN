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


#include <stdio.h> 
#include <stddef.h> 
#include <usb.h> 
#include "hpsdrusb.h"

/* returns 0 on failure */ 
int HPSDR_SetLED(usb_dev_handle *devh, int which, int on) {
	int rc; 
	int val; 
	
	if ( on ) { 
		val = 1; 
	}
	else { 
		val = 0; 
	}
	
	rc = usb_control_msg(devh, VENDOR_REQ_TYPE_OUT, VENDOR_REQ_SET_LED, 
						 val, which, NULL, 0, USB_TIMEOUT_MSECS);
	
	if ( rc < 0 ) { 
		return 0;
	}
	return 1;	
} 



int HPSDR_LoadFPGA(usb_dev_handle *devh, char *rbf_fnamep) {
	
	FILE *rbffile;
	char buf[MAX_EPO_PACKET_SIZE];
	int bytes_read;
	int total_bytes_xferd = 0;
	int rc; 
		
	rbffile = fopen(rbf_fnamep, "rb"); 
	if ( rbffile == NULL ) {
		fprintf(stderr, "Failed to open: \'%s\'\n", rbf_fnamep); 
		return 0; 
	}
	
	rc = usb_control_msg(devh, VENDOR_REQ_TYPE_OUT, VENDOR_REQ_FPGA_LOAD, 
			             0, FL_BEGIN, NULL, 0, USB_TIMEOUT_MSECS); 
	
	if ( rc < 0 ) {
		fprintf(stderr, "LoadFPGA: failed @ FL_BEGIN\n"); 
		fclose(rbffile); 
		return 0; 
	} 
	
	/* 
	 *  read the rbf and send it over the wire, 64 bytes at a time 
	 */ 
	while ( (bytes_read = fread(buf, 1, sizeof(buf), rbffile)) > 0 ) {
		rc = usb_control_msg(devh, VENDOR_REQ_TYPE_OUT, VENDOR_REQ_FPGA_LOAD, 
					         0, FL_XFER, buf, bytes_read, USB_TIMEOUT_MSECS);
		total_bytes_xferd += bytes_read; 
		if ( rc < 0 ) {
			fprintf(stderr, "LoadFPGA: failed @ FL_XFER\n"); 
			fclose(rbffile); 
			return 0; 
		}				
	}
	printf("%d bytes transferred.\n", total_bytes_xferd);
	fclose(rbffile); 
	rc = usb_control_msg(devh, VENDOR_REQ_TYPE_OUT, VENDOR_REQ_FPGA_LOAD, 
				          0, FL_END, NULL, 0, USB_TIMEOUT_MSECS);
	if ( rc < 0 ) {
		fprintf(stderr, "LoadFPGA: failed @ FL_END\n"); 		 
		return 0; 
	}		
		
	return 1; 
} 







