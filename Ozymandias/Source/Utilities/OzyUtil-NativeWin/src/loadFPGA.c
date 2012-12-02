/* HPSDR-USB - High Performance Software Defined Radio, USB support 
 *  
 * Copyright (C) 2007 Bill Tracey, KD5TFD  
 * Copyright (C) 2006 Philip A. Covington, N8VB
 * Copyright (C) 2012 George Byrkit, K9TRV: add manifest to get 'administrator' privilege on Vista, Win7 and Win8
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
 * This is an adaptation of Phil Covington's C# USB support routines to C for 
 * use on Linux
 * 
 * Author: Bill Tracey (bill@ewjt.com) 
 */


#include <stdio.h> 
#include <stddef.h> 
#include <usb.h> 
#include "hpsdrusb.h" 

char *copyright = "HPSDR-USB - High Performance Software Defined Radio, USB support Copyright (C) 2007 Bill Tracey, KD5TFD  Copyright (C) 2006 Philip A. Covington, N8VB";
		

void printRC(char *strp, int rc) { 
  printf("%s returned: %d\n", strp, rc); 
} 

void bailOut(char *strp, int rc) { 
  printf("%s\n"); 
  exit(rc); 
} 


unsigned int VID; 
unsigned int PID; 
char *rbfFileNamep; 

void usage(void) {
  printf("loadFPGA built @: %s %s\n", __DATE__, __TIME__);
  printf("usage: loadFPGA 0xVID 0xPID rbf_filename\n"); 
  return; 
} 

void parseArgs(int argc, char *argv[]) { 

  if ( argc != 4 ) { 
    usage(); exit(1); 
  }
  VID = strtol(argv[1], NULL, 16); 
  PID = strtol(argv[2], NULL, 16); 
  rbfFileNamep = argv[3]; 
  return; 
} 


int main(int argc, char *argv[]) { 
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

  
  /* dumpDevice(devp, stdout, " ");  */ 
  devh = usb_open(devp); 
  if ( devh == NULL ) { 
    printf("Could not open device VID: 0x04x PID: 0x04x\n", VID, PID); 
    exit(98);
  }
  

  rc = HPSDR_SetLED(devh, 1, 1); 
  if ( rc == 0 ) { 
	  printf("HPSDR_SetLED failed.\n"); 
	  exit(97);
  }
   
 
  rc = HPSDR_LoadFPGA(devh, rbfFileNamep); 
  if ( rc == 0 ) {
	  printf("LoadFPGA failed!\n");
  
  }
  else { 
	  printf("LoadFPGA succeeded!\n"); 
  }
  
  rc = HPSDR_SetLED(devh, 1, 0); 
  if ( rc == 0 ) { 
	  printf("HPSDR_SetLED failed.\n"); 
  	  exit(96);
  }
 
  usb_close(devh);
  
 } 
