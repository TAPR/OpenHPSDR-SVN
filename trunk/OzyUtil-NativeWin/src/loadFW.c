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

unsigned char *copyright = "HPSDR-USB - High Performance Software Defined Radio, USB support Copyright (C) 2007 Bill Tracey, KD5TFD  Copyright (C) 2006 Philip A. Covington, N8VB";
		

void printRC(char *strp, int rc) { 
  printf("%s returned: %d\n", strp, rc); 
} 

void bailOut(char *strp, int rc) { 
  printf("%s\n"); 
  exit(rc); 
} 

#if 0 
void dumpDevDescriptor(struct usb_device_descriptor *descp, FILE *fstream, char *prefixp) {
  fprintf(fstream, "%sbLength: %d\n", prefixp, descp->bLength); 
  fprintf(fstream, "%sbDescriptorType: %d\n", prefixp, descp->bDescriptorType); 
  fprintf(fstream, "%sbcdUSB: %d\n", prefixp, descp->bcdUSB); 
  fprintf(fstream, "%sbDeviceClass: %d\n", prefixp, descp->bDeviceClass);
  fprintf(fstream, "%sbDeviceSubClass: %d\n", prefixp, descp->bDeviceSubClass);
  fprintf(fstream, "%sbDeviceProtocol: %d\n", prefixp, descp->bDeviceProtocol);
  fprintf(fstream, "%sbMaxPacketSize0: %d\n", prefixp, descp->bMaxPacketSize0);
  fprintf(fstream, "%sidVendor: 0x%04x\n", prefixp, descp->idVendor);
  fprintf(fstream, "%sidProduct: 0x%04x\n", prefixp, descp->idProduct);
  fprintf(fstream, "%sbcdDevice: %d\n", prefixp, descp->bcdDevice);
  fprintf(fstream, "%siManufacturer: %d\n", prefixp, descp->iManufacturer);
  fprintf(fstream, "%siProduct: %d\n", prefixp, descp->iProduct);
  fprintf(fstream, "%siSerialNumber: %d\n", prefixp, descp->iSerialNumber);
  fprintf(fstream, "%sbNumConfigurations: %d\n", prefixp, descp->bNumConfigurations);
  return;
}

void dumpDevice(struct usb_device *devp, FILE *fstream, char *prefixp) { 
  int busaddr; 
  int devaddr; 
  char *my_prefixp; 
  int slen; 

  
  devaddr = (int)devp; 
  busaddr = (int)(devp->bus); 
  fprintf(fstream, "%sDevice @ 0x%08x fname: \'%s\' devnun: %d num_child: %d bus %: 0x%08x\n", 
          prefixp, devaddr, devp->filename, devp->devnum, devp->num_children, busaddr); 
  fprintf(fstream,"%sDevice Descriptor:\n", prefixp);
  slen = strlen(prefixp); 
  my_prefixp = malloc(slen+1+4); 
  if ( my_prefixp != NULL ) { 
    strcpy(my_prefixp, prefixp); 
    strcat(my_prefixp, "    "); 
  } 
  else {  /* alloc failed, just ue the prefix passed in */ 
    my_prefixp = prefixp; 
  } 
  dumpDevDescriptor(&(devp->descriptor), fstream, my_prefixp); 
  if ( prefixp != my_prefixp ) { 
    free(my_prefixp); 
  } 
  return; 
}


void dumpBus(struct usb_bus *busp, FILE *fstream) { 
  int busaddr; 
  int rootdev; 
  while ( busp != NULL ) {
    struct usb_device *devp; 
    busaddr = (int)busp; 
    rootdev = (int)(busp->root_dev); 
    fprintf(fstream, "Bus @ 0x%08x loc: %d dirname: \'%s\' rootdev @ 0x%08x\n", 
	    busaddr, busp->location, busp->dirname, rootdev); 
    devp = busp->devices;
    while ( devp != NULL ) { 
      dumpDevice(devp, fstream, "  "); 
      devp = devp->next; 
    }
    
    busp = busp->next; 
  } 
  return; 
}
#endif 

unsigned int VID; 
unsigned int PID; 
char *hexFileNamep; 

void usage(void) {
  printf("loadFW built @: %s %s\n", __DATE__, __TIME__);
  printf("usage: loadFW 0xVID 0xPID hex_filename\n"); 
  return; 
} 

void parseArgs(int argc, char *argv[]) { 

  if ( argc != 4 ) { 
    usage(); exit(1); 
  }
  VID = strtol(argv[1], NULL, 16); 
  PID = strtol(argv[2], NULL, 16); 
  hexFileNamep = argv[3]; 
  return; 
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
  if ( devh == NULL ) { 
    printf("Could not open device VID: 0x04x PID: 0x04x\n", VID, PID); 
    exit(98);
  }
  
  printf("ResetCPU(true)....");
  rc = HPSDR_ResetCPU(devh, 1);
  printf("...returns %d\n", rc); 
  
  printf("UploadFirmware('\\%s\\')...", hexFileNamep); 
  rc = HPSDR_UploadFirmware(devh, hexFileNamep); 
  printf("...returns %d\n", rc);
  
  printf("ResetCPU(false)....");
  rc = HPSDR_ResetCPU(devh, 0);
  printf("...returns %d\n", rc);
  
  
  usb_close(devh);
  
 } 
