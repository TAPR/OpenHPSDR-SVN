/*
---- Begin Copyright Notice

     This is part of the interfacing DLL between Winrad and the HPSDR, based on WinUSB
     Copyright (C) 2009  Alberto di Bene,  I2PHD

     This library is free software; you can redistribute it and/or
     modify it under the terms of the GNU Lesser General Public
     License as published by the Free Software Foundation; either
     version 2.1 of the License, or (at your option) any later version.

     This library is distributed in the hope that it will be useful,
     but WITHOUT ANY WARRANTY; without even the implied warranty of
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
     Lesser General Public License for more details.

     You should have received a copy of the GNU Lesser General Public
     License along with this library; if not, write to the Free Software
     Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, 
     MA  02110-1301  USA


--- End Copyright Notice

*/
//---------------------------------------------------------------------------

#ifndef WUSBfuncH
#define WUSBfuncH

#include <initguid.h>
#include <setupapi.h>
#include <winusb.h>
#include <usb100.h>
#include <stdio.h>
#include <stdlib.h>
#include "hpsdr_wusb.h"

#define HPSDR_GUID {0xF0564D09, 0xCAC6, 0x46aa, 0xB0, 0x76, 0x38, 0x06, 0xBB, \
                    0x35, 0x4E, 0xEE}
#define MAX_DEVPATH_LENGTH 256
#define MAX_STRING_LEN 256

#define MAX_EP0_PACKET_SIZE     64
#define FX2_RAM_RESET           0xE600
#define FX2_WRITE_RAM_REQ       0xA0;

/* Vendor Request Types */
#define VRT_VENDOR_IN           0xC0
#define VRT_VENDOR_OUT          0x40
#define VRQ_FPGA_LOAD           (0x02)
#define FL_BEGIN (0)   // wIndexL: begin fpga programming cycle.  stalls if trouble.
#define FL_XFER (1)    // wIndexL: xfer up to 64 bytes of data
#define FL_END (2)     // wIndexL: end programming cycle, check for success.


static WINUSB_INTERFACE_HANDLE usbHandle;
static WINUSB_PIPE_INFORMATION pipeInfo;
static USB_INTERFACE_DESCRIPTOR ifaceDescriptor;
static HPSDR_WUSB_INFO *hpwInfo;


//---------------------------------------------------------------------------

static bool ConfigurePipes(void);
static void Close_HPSDR(void);
static bool FX2_Reset(bool reset);
static int __fastcall HPSDR_UploadFX2Firmware(void);
static int __fastcall HPSDR_UploadFPGAFirmware(void);

//---------------------------------------------------------------------------
#endif
