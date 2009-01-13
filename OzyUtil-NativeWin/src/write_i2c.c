/* HPSDR-USB - High Performance Software Defined Radio, USB support 
 *  
 * Copyright (C) 2007,2009 Bill Tracey, KD5TFD  
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


#include <stdio.h> 
#include <stddef.h> 
#include <usb.h> 
#include "hpsdrusb.h" 

unsigned char *copyright = "HPSDR-USB - High Performance Software Defined Radio, USB support Copyright (C) 2007,2009 Bill Tracey, KD5TFD  Copyright (C) 2006 Philip A. Covington, N8VB  Copyright (C) 2007 Bob Campbell, VK4XV";

unsigned int VID; 
unsigned int PID; 
unsigned int i2c_address;
	char value1;
	char value2;
	char value3;
	char count;
	char buf[4];
	char *bufp;


void usage(void) {
  printf("writeI2C built @: %s %s\n", __DATE__, __TIME__);
  printf("usage: writeI2C <VID> <PID> <i2c_address in hex> <count in hex> <value1 in hex> <value2 in hex> <value3 in hex> \n"); 
  printf("eg: writeI2C 0xfffe 0x7 0x1a 0x2 0x1e 0x00\n"); 
  return; 
} 

void parseArgs(int argc, char *argv[]) { 

  if ( argc < 6 || argc == 0 ) { 
    usage();
    printf("argc: = %d\n",argc);
    exit(1); 
  }

//printf("debug point 1\n");

  VID = strtol(argv[1], NULL, 16); 
  PID = strtol(argv[2], NULL, 16); 
  i2c_address = strtol(argv[3], NULL, 16);
  count = strtol(argv[4], NULL, 16);
  value1 = strtol(argv[5], NULL, 16);
  if ( argc >= 7 ) value2 = strtol(argv[6], NULL, 16);
  if ( argc >= 8 ) value3 = strtol(argv[7], NULL, 16);
  
//printf("debug point 2\n");

#if 0 
  printf("count: %d\n", count); 
#endif 

  switch (count)
            {
                case 1:
                    buf[0] = value1;
                    break;
                case 2:
                    buf[0] = value1;
                    buf[1] = value2;
                    break;
                case 3:
                    buf[0] = value1;
                    buf[1] = value2;
                    buf[2] = value3;
                    break;
                default:
                    printf("An undefined error occurred");
                    exit(1);
                    return;
            }            
#if 0
  printf("VID: 0x%04x PID: 0x%02x ADD: 0x%02x Value1: 0x%02x Value2: 0x%02x Value3: 0x%02x Count %d \n", VID, PID, i2c_address, value1, value2, value3, count);
#endif
 
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
  
#if 0 
  printf("VID: 0x%08x PID: 0x%04x hexfn: >%s<\n", VID, PID, hexFileNamep); 

  printf("sizeof(struct usb_bus): %d\n", sizeof(struct usb_bus)); 
  printf("usb_bus offsets:  dirname: %d  devices: %d  location: %d\n", 
         offsetof(struct usb_bus, dirname), offsetof(struct usb_bus, devices), 
         offsetof(struct usb_bus, location));
#endif  

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
	exit(98);
	}
#if 0
		printf("Found it!...%d\n",devh);
		printf("Count = %d  ",count);
		printf("Address = 0x%02x ",i2c_address);
		for (i = 0; i <= count; ++i)
			{ printf("data %d = 0x%02x ", i, buf[i]);
			}
		printf("\n");
#endif
	rc = HPSDR_Write_I2C(devh, i2c_address, buf, count);
	if (rc <= 0)
		{
		printf(" ***** Write to I2C Failed ***** ");
		}
	printf("I2C Bytes written = %d \n",rc);

 usb_close(devh);
}
