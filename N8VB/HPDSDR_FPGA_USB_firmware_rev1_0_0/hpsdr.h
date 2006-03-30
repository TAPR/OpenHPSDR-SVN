/*
Copyright (c) 2005 Philip A Covington, N8VB

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

// HPSDR SPECIFIC DEFINITIONS

#ifndef HPSDR_H   /* Header Sentry */
#define HPSDR_H

#include "fx2regs.h"

#define BOOL unsigned char

#define SC_GET_STATUS         	0x00   // Setup command: Get Status
#define SC_CLEAR_FEATURE      	0x01   // Setup command: Clear Feature
#define SC_RESERVED            	0x02   // Setup command: Reserved
#define SC_SET_FEATURE         	0x03   // Setup command: Set Feature
#define SC_SET_ADDRESS         	0x05   // Setup command: Set Address
#define SC_GET_DESCRIPTOR      	0x06   // Setup command: Get Descriptor
#define SC_SET_DESCRIPTOR      	0x07   // Setup command: Set Descriptor
#define SC_GET_CONFIGURATION   	0x08   // Setup command: Get Configuration
#define SC_SET_CONFIGURATION   	0x09   // Setup command: Set Configuration
#define SC_GET_INTERFACE      	0x0a   // Setup command: Get Interface
#define SC_SET_INTERFACE      	0x0b   // Setup command: Set Interface
#define SC_SYNC_FRAME			0x0c   // Setup command: Sync Frame
#define SC_ANCHOR_LOAD         	0xa0   // Setup command: Anchor load
   
#define GD_DEVICE          		0x01  // Get descriptor: Device
#define GD_CONFIGURATION   		0x02  // Get descriptor: Configuration
#define GD_STRING          		0x03  // Get descriptor: String
#define GD_INTERFACE       		0x04  // Get descriptor: Interface
#define GD_ENDPOINT        		0x05  // Get descriptor: Endpoint
#define GD_DEVICE_QUALIFIER 	0x06  // Get descriptor: Device Qualifier
#define GD_OTHER_SPEED_CONFIGURATION 0x07  // Get descriptor: Other Configuration
#define GD_INTERFACE_POWER 		0x08  // Get descriptor: Interface Power
#define GD_HID	            	0x21	// Get descriptor: HID
#define GD_REPORT	         	0x22	// Get descriptor: Report

#define GS_DEVICE          		0x80  // Get Status: Device
#define GS_INTERFACE       		0x81  // Get Status: Interface
#define GS_ENDPOINT        		0x82  // Get Status: End Point

#define FT_DEVICE          		0x00  // Feature: Device
#define FT_ENDPOINT        		0x02  // Feature: End Point

#define I2C_IDLE              	0     // I2C Status: Idle mode
#define I2C_SENDING           	1     // I2C Status: I2C is sending data
#define I2C_RECEIVING         	2     // I2C Status: I2C is receiving data
#define I2C_PRIME             	3     // I2C Status: I2C is receiving the first byte of a string
#define I2C_STOP              	5     // I2C Status: I2C waiting for stop completion
#define I2C_BERROR            	6     // I2C Status: I2C error; Bit Error
#define I2C_NACK              	7     // I2C Status: I2C error; No Acknowledge
#define I2C_OK                	8     // I2C positive return code
#define I2C_WAITSTOP          	9     // I2C Status: Wait for STOP complete

// Globals

volatile BOOL xdata RxSUD; 		//flag to indicate setup data available
BOOL xdata RWUEN; 				// remote wakeup flag
BOOL xdata SELFPWR; 			// self powered flag
BOOL xdata EP1HASDATA;

BOOL xdata got_vend_cmd;
BOOL xdata wascalled;

BYTE xdata Configuration;      	// Current configuration
BYTE xdata AlternateSetting;   	// Alternate settings

BYTE xdata at (0x4002) test_extern_mem1;
BYTE xdata at (0x4004) test_extern_mem2;

// for descriptor table, see dscr.asm

extern xdata const char DeviceDscr;
extern xdata const char DeviceQualDscr;
extern xdata const char HighSpeedConfigDscr;
extern xdata const char FullSpeedConfigDscr;
extern xdata unsigned char StringDscr;
extern xdata unsigned char StringDscr0;
extern xdata unsigned char StringDscr1;
extern xdata unsigned char StringDscr2;
extern xdata unsigned char UserDscr;

WORD xdata pDeviceDscr;
WORD xdata pDeviceQualDscr;
WORD xdata pHighSpeedConfigDscr;
WORD xdata pFullSpeedConfigDscr;	
WORD xdata pConfigDscr;
WORD xdata pOtherConfigDscr;
WORD xdata pStringDscr;

WORD xdata * pExtMemory;
BYTE xdata * pExtByteMemory;

//-----------------------------------------------------------------------------
// Prototypes
//-----------------------------------------------------------------------------
void process_ep0_data(void);
void process_ep1_data(void);
void clear_ext_ram(void);
void main_loop (void);
void setup_interrupts (void);
void setup_io (void);
void setup_descriptor_pointers (void);

#endif //HPSDR_H
	   
