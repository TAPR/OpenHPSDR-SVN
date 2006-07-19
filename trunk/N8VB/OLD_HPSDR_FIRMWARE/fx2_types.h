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

// FX2 TYPE DEFINITIONS

#ifndef FX2_TYPES_H   /* Header Sentry */
#define FX2_TYPES_H

#include "fx2regs.h"

#define BLOCKSIZE 64
#define TRUE 1
#define FALSE 0

#define bmSTRETCH 0x07
#define FW_STRETCH_VALUE 0x0      // Set stretch to 0 

#define DEVICE_DSCR      0x01      // Descriptor type: Device
#define CONFIG_DSCR      0x02      // Descriptor type: Configuration
#define STRING_DSCR      0x03      // Descriptor type: String
#define INTRFC_DSCR      0x04      // Descriptor type: Interface
#define ENDPNT_DSCR      0x05      // Descriptor type: End Point
#define DEVQUAL_DSCR     0x06      // Descriptor type: Device Qualifier
#define OTHERSPEED_DSCR  0x07      // Descriptor type: Other Speed Configuration

#define bmBUSPWR  bmBIT7         // Config. attribute: Bus powered
#define bmSELFPWR bmBIT6         // Config. attribute: Self powered
#define bmRWU     bmBIT5         // Config. attribute: Remote Wakeup

#define bmEPOUT   bmBIT7
#define bmEPIN    0x00

#define EP_CONTROL   0x00        // End Point type: Control
#define EP_ISO       0x01        // End Point type: Isochronous
#define EP_BULK      0x02        // End Point type: Bulk
#define EP_INT       0x03        // End Point type: Interrupt

#define SUD_SIZE            8      // Setup data packet size
									// 
// this table is used by the epcs macro 
const char code  EPCS_Offset_Lookup_Table[] =
{
   0,    // EP1OUT
   1,    // EP1IN
   2,    // EP2OUT
   2,    // EP2IN
   3,    // EP4OUT
   3,    // EP4IN
   4,    // EP6OUT
   4,    // EP6IN
   5,    // EP8OUT
   5,    // EP8IN
};

// Datatypes

typedef unsigned long   DWORD;

typedef struct
{
   BYTE   length;
   BYTE   type;
}DSCR;


typedef struct            // Device Descriptor
{
   BYTE   length;         // Descriptor length ( = sizeof(DEVICEDSCR) )
   BYTE   type;         // Decriptor type (Device = 1)
   BYTE   spec_ver_minor;   // Specification Version (BCD) minor
   BYTE   spec_ver_major;   // Specification Version (BCD) major
   BYTE   dev_class;      // Device class
   BYTE   sub_class;      // Device sub-class
   BYTE   protocol;      // Device sub-sub-class
   BYTE   max_packet;      // Maximum packet size
   WORD   vendor_id;      // Vendor ID
   WORD   product_id;      // Product ID
   WORD   version_id;      // Product version ID
   BYTE   mfg_str;      // Manufacturer string index
   BYTE   prod_str;      // Product string index
   BYTE   serialnum_str;   // Serial number string index
   BYTE   configs;      // Number of configurations
}DEVICEDESCR;

typedef struct            // Device Qualifier Descriptor
{
   BYTE   length;         // Descriptor length ( = sizeof(DEVICEQUALDSCR) )
   BYTE   type;         // Decriptor type (Device Qualifier = 6)
   BYTE   spec_ver_minor;   // Specification Version (BCD) minor
   BYTE   spec_ver_major;   // Specification Version (BCD) major
   BYTE   dev_class;      // Device class
   BYTE   sub_class;      // Device sub-class
   BYTE   protocol;      // Device sub-sub-class
   BYTE   max_packet;      // Maximum packet size
   BYTE   configs;      // Number of configurations
   BYTE  reserved0;
}DEVICEQUALDSCR;

typedef struct
{
   BYTE   length;         // Configuration length ( = sizeof(CONFIGDSCR) )
   BYTE   type;         // Descriptor type (Configuration = 2)
   WORD   config_len;      // Configuration + End Points length
   BYTE   interfaces;      // Number of interfaces
   BYTE   index;         // Configuration number
   BYTE   config_str;      // Configuration string
   BYTE   attrib;         // Attributes (b7 - buspwr, b6 - selfpwr, b5 - rwu
   BYTE   power;         // Power requirement (div 2 ma)
}CONFIGDSCR;

typedef struct
{
   BYTE   length;         // Interface descriptor length ( - sizeof(INTRFCDSCR) )
   BYTE   type;         // Descriptor type (Interface = 4)
   BYTE   index;         // Zero-based index of this interface
   BYTE   alt_setting;   // Alternate setting
   BYTE   ep_cnt;         // Number of end points 
   BYTE   class;         // Interface class
   BYTE   sub_class;      // Interface sub class
   BYTE   protocol;      // Interface sub sub class
   BYTE   interface_str;   // Interface descriptor string index
}INTRFCDSCR;

typedef struct
{
   BYTE   length;         // End point descriptor length ( = sizeof(ENDPNTDSCR) )
   BYTE   type;         // Descriptor type (End point = 5)
   BYTE   addr;         // End point address
   BYTE   ep_type;      // End point type
   BYTE   mp_L;         // Maximum packet size
   BYTE   mp_H;
   BYTE   interval;      // Interrupt polling interval
}ENDPNTDSCR;

typedef struct
{
   BYTE   length;         // String descriptor length
   BYTE   type;         // Descriptor type
}STRINGDSCR;

typedef struct
{
   BYTE   cntrl;         // End point control register
   BYTE   bytes;         // End point buffer byte count
}EPIOC;

typedef struct 
{
   BYTE   length;
   BYTE   *dat;
   BYTE   count;
   BYTE   status;
}I2CPCKT;

#endif //FX2_TYPES_H
