/* HPSDR-USB - High Performance Software Defined Radio, USB support 
 *  
 * Copyright (C) 2007 Bill Tracey, KD5TFD  
 * Copyright (C) 2006 Philip A. Covington, N8VB
 * Copyright (C) 2007 Bob Campbell, VK4XV 
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
 * Author: Bob Campbell, VK4XV  
 */
// This program is a command line utility to read the I2C bus through the FX2.
// It finds the FX2 by VID and PID



#include <stdio.h> 
#include <stddef.h> 
#include <usb.h> 
#include "hpsdrusb.h" 

unsigned char *copyright = "HPSDR-USB - High Performance Software Defined Radio, USB support Copyright (C) 2007 Bill Tracey, KD5TFD  Copyright (C) 2006 Philip A. Covington, N8VB  Copyright (C) 2007 Bob Campbell, VK4XV";

unsigned int VID; 
unsigned int PID; 
unsigned int i2c_address;
	char count;
	char buf[MAX_EPO_PACKET_SIZE];
	char *bufp;


void usage(void) {
  printf("read_i2c built @: %s %s\n", __DATE__, __TIME__);
  printf("Usage: read_I2C <VID> <PID> <i2c_address in hex> <length in decimal>"); 
  return; 
} 

void parseArgs(int argc, char *argv[]) { 

  if ( argc != 5 || argc == 0 ) { 
    usage();
    printf("argc: = %d\n",argc);
    exit(-1); 
  }

  VID = strtol(argv[1], NULL, 16);	// Hex required 
  PID = strtol(argv[2], NULL, 16); 
  i2c_address = strtol(argv[3], NULL, 16);
  count = strtol(argv[4], NULL, 10);	// note decimal

	if ((VID==0)||(PID==0)||(i2c_address==0)||(count==0)) {
	usage();
	exit(-2);
	}

//#if 0
  printf("VID: 0x%04x PID: 0x%02x ADD: 0x%02x Count %d \n", VID, PID, i2c_address, count);
//#endif
  return;

}

void bailOut(char *strp, int rc) { 
  printf("%s\n"); 
  exit(rc); 
} 


void printRC(char *strp, int rc) { 
  printf("%s returned: %d\n", strp, rc); 
} 

int main(int argc, char *argv[]) { 
  int i; 
  int rc; 
  struct usb_bus *busp; 
  struct usb_device *devp; 
  struct usb_dev_handle *devh; 

  parseArgs(argc, argv);

  usb_init(); 
  rc = usb_find_busses();
  printRC("usb_find_busses", rc); 


  rc = usb_find_devices(); 
  printRC("usb_find_devices", rc); 

  busp = usb_get_busses(); 
  if ( busp == NULL ) { 
    bailOut("busp is NULL", 99); 
  }
  
  devp = HPSDR_findDevice(busp, VID, PID); 
  if ( devp == NULL ) { 
    printf("\nCould not find: VID: 0x%04x PID: 0x%04x\n\n", VID, PID);
#if 0     
    dumpBus(busp, stdout);
#endif     
    exit(99); 
  } 

#if 0 
  printf("Found it!\n");
#endif 
  
  /* dumpDevice(devp, stdout, " ");  */ 
  devh = usb_open(devp); 
  if ( devh == NULL )
	{ 
	printf("Could not open device VID: 0x04x PID: 0x04x\n", VID, PID); 
	exit(97);
	}
//#if 0
		printf("Found it!...%d\n",devh);
		printf("Address = 0x%02x ",i2c_address);
		printf("Count = %d  ",count);
		for (i = 0; i <= count; ++i)
			{ printf("data %d = 0x%02x ", i, buf[i]);
			}
		printf("\n");
//#endif
	rc = HPSDR_Read_I2C(devh, i2c_address, buf, count);
	if (rc <= 0)
		{
		printf(" ***** Read to I2C Failed ***** ");
		}
	printf("I2C Bytes Read = %d \n",rc);

 usb_close(devh);
}  
