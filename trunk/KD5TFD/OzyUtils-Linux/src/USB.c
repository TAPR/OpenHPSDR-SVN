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


/* 
 *  returns bytes_written, negative on error 
 */  
int HPSDR_WriteRAM(usb_dev_handle *devh, int fx2_start_addr, char *bufp, int count) {
	int pkt_size = MAX_EPO_PACKET_SIZE; 
	int len = count; 
	int bytes_written = 0;
	int addr; 
	int bytes_written_this_write; 
	int nsize; 
	 
	
	for ( addr = fx2_start_addr; addr < fx2_start_addr + len; addr += pkt_size, bufp += pkt_size ) { 
		nsize = len + fx2_start_addr - addr; 
		if ( nsize > pkt_size ) nsize = pkt_size;
		bytes_written_this_write = usb_control_msg(devh, 0x40, 0xa0, addr, 0, bufp, nsize, USB_TIMEOUT_MSECS);
		if ( bytes_written_this_write >= 0  ) { 
			bytes_written += bytes_written_this_write; 
		}
		else {
			return bytes_written_this_write; 
		}
	}
	return bytes_written; 
}



// returns !0 on succcess 0 on failure 
int HPSDR_ResetCPU(usb_dev_handle *devh, int reset) {
	
	char write_buf; 
	if ( reset ) write_buf = 1; 
	else write_buf = 0; 
	
	if ( HPSDR_WriteRAM(devh, 0xe600, &write_buf, 1) != 1 ) return 0;
	else return 1;
	
}


struct usb_device *HPSDR_findDevice(struct usb_bus *bus_rootp, unsigned int vid, unsigned int pid) { 
  struct usb_bus *busp;
  struct usb_device *devp; 

  busp = bus_rootp; 
  while ( busp != NULL ) { 

    devp = busp->devices; 
    while ( devp != NULL ) { 
      if ( (devp->descriptor.idVendor == vid) &&  (devp->descriptor.idProduct == pid) ) { 
	return devp; 
      }
      /* else */ 
      devp = devp->next;         
    } 
    busp = busp->next; 
  } 
  return NULL; 
} 


static unsigned int hexitToUInt(char c) { 
	c = tolower(c);
	if ( c >= '0' && c <= '9' ) { 
		return c - '0'; 
	}
	else if ( c >= 'a' && c <= 'f' ) {
		return 10 + (c - 'a'); 		
	}
	return 0;			
}

static int ishexit(c) {
	c = tolower(c); 
	if ( c >= '0' && c <= '9' ) return 1; 
	if ( c >= 'a' && c <= 'f' ) return 1; 
	return 0;  
}


// returns <0 on failure, unsigned int 
static int hexitsToUInt(char *p, int count) { 
	unsigned int result = 0; 
	int i; 
	char c;
	unsigned int this_hex; 
	for ( i = 0; i < count; i++ ) { 
		c = *p; 
		++p; 
		if ( !ishexit(c) ) {
			return -1; 
		}
		this_hex = hexitToUInt(c); 
		result *= 16; 
		result += this_hex; 		
	}
	return result; 	
}



// returns 0 on error 
int HPSDR_UploadFirmware(usb_dev_handle *devh, char *fnamep) {
	FILE *ifile; 
	int linecount = 0; 
	int length; 
	int addr; 
	int type;
	char readbuf[1030]; 
	char wbuf[256]; 
	unsigned char my_cksum;
	unsigned char cksum; 
	int this_val;  
	int i; 
	
	
	ifile = fopen(fnamep, "r"); 
	if ( ifile == NULL ) { 
		fprintf(stderr, "Could not open: \'%s\'\n", fnamep); 
		return 0; 
	}
	
	while (  fgets(readbuf, sizeof(readbuf), ifile) != NULL ) {
		++linecount; 
		if ( readbuf[0] != ':' ) { 
			fprintf(stderr, "HPSDR_UploadFirmware: bad record\n");
			return 0; 
		}
		length = hexitsToUInt(readbuf+1, 2); 
		addr = hexitsToUInt(readbuf+3, 4);
		type = hexitsToUInt(readbuf+7, 2); 
		if ( length < 0 || addr < 0 || type < 0 ) {
			fprintf(stderr, "HPSDR_UploadFirmware: bad length, addr or type\n"); 
			return 0; 
		}
		switch ( type ) {
			case 0: /* record */
				my_cksum = (unsigned char)(length + (addr & 0xff) + (addr >>8 + type));
				for ( i = 0; i < length; i++ ) { 
					this_val = hexitsToUInt(readbuf+9+(i*2),2);
#if 0 
					printf("i: %d val: 0x%02x\n", i, this_val); 
#endif 
					
					if ( this_val < 0 ) { 
						fprintf(stderr, "HPSDR_UploadFirmware: bad record data\n"); 
						return 0; 
					}
					wbuf[i] = (unsigned char)this_val;
					my_cksum += wbuf[i];
				}
				
				this_val = hexitsToUInt(readbuf+9+(length*2),2); 
				if ( this_val < 0 ) { 
					fprintf(stderr, "HPSDR_UploadFirmware: bad checksum data\n"); 
					return 0;					
				}
				cksum = (unsigned char)this_val;
#if 0 
				printf("\n%s", readbuf); 
				printf("len: %d (0x%02x) addr: 0x%04x mychk: 0x%02x chk: 0x%02x", 
						length, length, addr, my_cksum, cksum); 
#endif 
				
				if ( ((cksum + my_cksum) & 0xff) != 0 ) { 
					fprintf(stderr, "HPSDR_UploadFirmware: bad checksum\n"); 
					return 0;										
				}
				if ( HPSDR_WriteRAM(devh,addr, wbuf, length) < 1 ) { 
					fprintf(stderr, "HPSDR_UploadFirmware: bad write\n");
					return 0; 
				}
				break;
				
			case 1: /* EOF */ 
				break; 
				
			default: /* invalid */
				fprintf(stderr, "HPSDR_UploadFirmware: invalid type\n"); 
				return 0;
		
		}
	}
	fprintf(stderr, "HPSDR_UploadFirmware: Processed %d lines.\n", linecount);
	return linecount; 
}


