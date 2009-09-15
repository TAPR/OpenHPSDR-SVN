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

#include <vcl.h>
#include <windows.h>
#pragma hdrstop

#include "WUSBfunc.h"

//---------------------------------------------------------------------------
bool GetDevicePath(LPGUID InterfaceGuid, char* DevicePath, size_t BufLen)
{
   bool bResult;
   HDEVINFO deviceInfo;
   SP_DEVICE_INTERFACE_DATA interfaceData;
   PSP_DEVICE_INTERFACE_DETAIL_DATA detailData;
   ULONG length;
   ULONG requiredLength=0;
   HRESULT hr;

   deviceInfo = SetupDiGetClassDevs(InterfaceGuid, NULL, NULL,
									DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
//Error handling code to be added

   int nMemberIndex = 0;  // we are interested only in the first USB device associated
						              // with the device interface class
   interfaceData.cbSize = sizeof(SP_DEVICE_INTERFACE_DATA);
   bResult = SetupDiEnumDeviceInterfaces(deviceInfo, NULL, InterfaceGuid, nMemberIndex,
										 &interfaceData);

   if(!bResult) return false;

   SetupDiGetDeviceInterfaceDetail(deviceInfo, &interfaceData, NULL, 0,
								   &requiredLength, NULL);
   detailData = (PSP_DEVICE_INTERFACE_DETAIL_DATA)
							  LocalAlloc(LMEM_FIXED, requiredLength);
   if(NULL == detailData)
   {
	   SetupDiDestroyDeviceInfoList(deviceInfo);
	   return false;
   }

   detailData->cbSize = sizeof(SP_DEVICE_INTERFACE_DETAIL_DATA);
   length = requiredLength;
   bResult = SetupDiGetDeviceInterfaceDetail(deviceInfo, &interfaceData,
						   detailData, length, &requiredLength, NULL);

   if(!bResult)
   {
	   LocalFree(detailData);
	   return false;
   }

   if(strlen(detailData->DevicePath) < MAX_DEVPATH_LENGTH)
	  strcpy(DevicePath, detailData->DevicePath);
   else
   {
	  SetupDiDestroyDeviceInfoList(deviceInfo);
	  LocalFree(detailData);
   }

   LocalFree(detailData);
   return bResult;
}
//---------------------------------------------------------------------------
HANDLE OpenDevice(BOOL bSync)
{
   GUID guid = HPSDR_GUID;
   LPGUID pguid = &guid;
   HANDLE hDev;
   char devicePath[MAX_DEVPATH_LENGTH];
   BOOL retVal;

   if(!GetDevicePath(pguid, devicePath, sizeof(devicePath)))
	   return NULL;

   hDev = CreateFile(devicePath,
					 GENERIC_WRITE | GENERIC_READ,
					 FILE_SHARE_WRITE | FILE_SHARE_READ,
					 NULL,
					 OPEN_EXISTING,
					 FILE_ATTRIBUTE_NORMAL | FILE_FLAG_OVERLAPPED,
					 NULL);
//Error handling code to be added

   return hDev;
}
//---------------------------------------------------------------------------
void Close_HPSDR()
{
   if(hpwInfo->winUSBHandle) WinUsb_Free(hpwInfo->winUSBHandle);

   if(hpwInfo->deviceHandle) CloseHandle(hpwInfo->deviceHandle);
}
//---------------------------------------------------------------------------
int Initialize_HPSDR(HPSDR_WUSB_INFO& hpw)
{
   bool bResult;
   UCHAR speed;
   ULONG length;
   int   linect;

   hpwInfo = &hpw;

   if(hpwInfo->winUSBHandle != NULL)
	  return ifaceDescriptor.bNumEndpoints + 1000; // +1000 indicates already inited

   if(hpwInfo->deviceHandle == NULL)
	  hpwInfo->deviceHandle = OpenDevice(TRUE);

   if(hpwInfo->deviceHandle <= 0) return -1;

   bResult = WinUsb_Initialize(hpwInfo->deviceHandle, &usbHandle);

   if(bResult) hpwInfo->winUSBHandle = usbHandle;

// ------------ load now the FX2 firmware ----------------------------

   FX2_Reset(true);

   bResult = (linect = HPSDR_UploadFX2Firmware()) > 0; 
   hpwInfo->FX2linesloaded = linect;

   FX2_Reset(false);

   if(bResult)
   {
	   Sleep(3000);

	   Close_HPSDR();
	   hpwInfo->deviceHandle = OpenDevice(TRUE);
	   WinUsb_Initialize(hpwInfo->deviceHandle, &usbHandle);
	   hpwInfo->winUSBHandle = usbHandle;
	   bResult = ConfigurePipes();

// ------------ load now the FPGA firmware ----------------------------

	   if(bResult) bResult = (linect = HPSDR_UploadFPGAFirmware()) > 0;
	   hpwInfo->FPGAbytesloaded = linect;
   }

// ------------ take note of the speed of the device ------------------

   if(bResult)
   {
	   length = sizeof(UCHAR);
	   bResult = WinUsb_QueryDeviceInformation(hpwInfo->winUSBHandle,
											 DEVICE_SPEED,
											 &length,
											 &speed);
	   if(bResult) hpwInfo->deviceSpeed = speed;
   }

   if(bResult) return ifaceDescriptor.bNumEndpoints;
   else return -1;
}
//---------------------------------------------------------------------------
int Write_FX2_RAM(int startaddr, char * buffer, int length)
{
	WINUSB_SETUP_PACKET wusb_setup_packet;

	int pkt_size = MAX_EP0_PACKET_SIZE;
	int addr, nsize;
	unsigned long count = 0;

	for (addr = startaddr; addr < startaddr + length; addr += pkt_size)
	{
		nsize = length + startaddr - addr;
		if (nsize > pkt_size) nsize = pkt_size;

		wusb_setup_packet.RequestType = VRT_VENDOR_OUT;
		wusb_setup_packet.Request = FX2_WRITE_RAM_REQ;
		wusb_setup_packet.Value = addr;
		wusb_setup_packet.Index = 0;
		wusb_setup_packet.Length = nsize;

		if (WinUsb_ControlTransfer(hpwInfo->winUSBHandle, wusb_setup_packet,
								  (unsigned char *)(buffer + (addr - startaddr)),
								   nsize, &count, NULL))
		{
		   return count;
		}
		else
		{
		   return -1;
		}
	}
	return (int)count;
}
//---------------------------------------------------------------------------------------------------
bool FX2_Reset(bool reset)
{
// PUT CPU IN RESET, OR TAKE IT OUT OF RESET, DEPENDING ON THE PARAMETER

	char value = (reset) ? 1 : 0;       // 1 if reset, 0 if unreset
	if (!Write_FX2_RAM(FX2_RAM_RESET, &value, 1))
	{
	   return false;
	}
	else return true;
}
//---------------------------------------------------------------------------------------------------
static int hex2UInt(char c)
{
	c = tolower(c);

	if (c >= '0' && c <= '9') return c - '0';
	if (c >= 'a' && c <= 'f') return 10 + (c - 'a');

	return -1;
}
//---------------------------------------------------------------------------

static int hexString2UInt(char *p, unsigned int count)
// returns <0 on failure, unsigned int on success
{
	unsigned int i, result = 0;
	int this_hex;

	for (i = 0; i < count; i++)
	{
		if((this_hex = hex2UInt(*p++)) < 0) return -1;
		result *= 16;
		result += this_hex;
	}
	return result;
}
//---------------------------------------------------------------------------
int __fastcall HPSDR_UploadFX2Firmware()
// returns 0 on error
{
	FILE *ifile;
	int linecount = 0;
	int length;
	int addr;
	int type;
	char readbuf[1030];
	char wbuf[256], s[512];
	unsigned char my_cksum;
	unsigned char cksum;
	int this_val;
	int i;


	ifile = fopen(hpwInfo->FX2fname, "r");

	if (ifile == NULL)
	{
		sprintf(hpwInfo->lastMsg, "Could not open: \'%s\'", hpwInfo->FX2fname);
		return 0;
	}

	while (fgets(readbuf, sizeof(readbuf), ifile) != NULL)
	{
		++linecount;
		if (readbuf[0] != ':')
		{
			strcpy(hpwInfo->lastMsg, "HPSDR_UploadFX2Firmware: bad record");
			return 0;
		}

		length = hexString2UInt(readbuf+1, 2);
		addr   = hexString2UInt(readbuf+3, 4);
		type   = hexString2UInt(readbuf+7, 2);

		if (length < 0 || addr < 0 || type < 0)
		{
			strcpy(hpwInfo->lastMsg,
				   "HPSDR_UploadFX2Firmware: bad length, addr or type");
			return 0;
		}

		switch (type)
		{
			case 0: /* record */
				my_cksum = (unsigned char)(length + (addr & 0xff) +
										  ((addr >>8) + type));

				for (i = 0; i < length; i++)
				{
					this_val = hexString2UInt(readbuf+9+(i*2), 2);

					if(this_val < 0)
					{
						strcpy(hpwInfo->lastMsg,
							   "HPSDR_UploadFX2Firmware: bad record data");
						return 0;
					}

					wbuf[i] = (unsigned char)this_val;
					my_cksum += wbuf[i];
				}

				this_val = hexString2UInt(readbuf+9+(length*2),2);

				if (this_val < 0)
				{
					strcpy(hpwInfo->lastMsg,
						   "HPSDR_UploadFX2Firmware: bad checksum data");
					return 0;
				}
				cksum = (unsigned char)this_val;

				if (((cksum + my_cksum) & 0xff) != 0)
				{
					strcpy(hpwInfo->lastMsg,
						   "HPSDR_UploadFX2Firmware: bad checksum");
					return 0;
				}

				if (Write_FX2_RAM(addr, wbuf, length) < 1)
				{
					strcpy(hpwInfo->lastMsg,
						   "HPSDR_UploadFX2Firmware: bad write");
					return 0;
				}
				break;

			case 1: /* EOF */
				break;

			default: /* invalid */
				strcpy(hpwInfo->lastMsg,
					   "HPSDR_UploadFX2Firmware: invalid type");
				return 0;

		}
	}

	return linecount;
}
//---------------------------------------------------------------------------

int __fastcall HPSDR_UploadFPGAFirmware()
{
	WINUSB_SETUP_PACKET wusb_setup_packet;

	FILE *rbffile;
	char buf[MAX_EP0_PACKET_SIZE];
	size_t  bytes_read;
	size_t total_bytes_xferd = 0;
	size_t rc;
	unsigned long count = 0;
	char s[256];


	rbffile = fopen(hpwInfo->FPGAfname, "rb");
	if ( rbffile == NULL )
	{
	  sprintf(hpwInfo->lastMsg, "Failed to open: \'%s\'\n", hpwInfo->FPGAfname);
	  return -1;
	}

	wusb_setup_packet.RequestType = VRT_VENDOR_OUT;
	wusb_setup_packet.Request = VRQ_FPGA_LOAD;
	wusb_setup_packet.Value = 0;
	wusb_setup_packet.Index = FL_BEGIN;
	wusb_setup_packet.Length = 0;

	if (!WinUsb_ControlTransfer(hpwInfo->winUSBHandle, wusb_setup_packet,
								0, 0, &count, NULL))
	{
	  fclose(rbffile);
	  strcpy(hpwInfo->lastMsg,
			 "HPSDR_UploadFPGAFirmware failed in FL_BEGIN load stage");
	  return -1;
	}

// read the rbf and send it over the wire, 64 bytes at a time

	while((bytes_read = fread(buf, 1, sizeof(buf), rbffile)) > 0)
	{
	   wusb_setup_packet.RequestType = VRT_VENDOR_OUT;
	   wusb_setup_packet.Request = VRQ_FPGA_LOAD;
	   wusb_setup_packet.Value = 0;
	   wusb_setup_packet.Index = FL_XFER;
	   wusb_setup_packet.Length = bytes_read;

	   if(!WinUsb_ControlTransfer(hpwInfo->winUSBHandle, wusb_setup_packet,
								  buf, (int)bytes_read, &count, NULL))
	   {
		  fclose(rbffile);
		  strcpy(hpwInfo->lastMsg,
				 "HPSDR_UploadFPGAFirmware failed in FL_XFER load stage");
		  return -1;
	   }

	   total_bytes_xferd += count;
	}

	wusb_setup_packet.RequestType = VRT_VENDOR_OUT;
	wusb_setup_packet.Request = VRQ_FPGA_LOAD;
	wusb_setup_packet.Value = 0;
	wusb_setup_packet.Index = FL_END;
	wusb_setup_packet.Length = 0;

	if (!WinUsb_ControlTransfer(hpwInfo->winUSBHandle, wusb_setup_packet,
								NULL, 0, &count, NULL))
	{
	   fclose(rbffile);
	   strcpy(hpwInfo->lastMsg,
			  "HPSDR_UploadFPGAFirmware failed in FL_END load stage");
	   return -1;
	}

	fclose(rbffile);

	return total_bytes_xferd;
}
//---------------------------------------------------------------------------
bool ConfigurePipes()
{
	ZeroMemory(&ifaceDescriptor, sizeof(USB_INTERFACE_DESCRIPTOR));
	ZeroMemory(&pipeInfo, sizeof(WINUSB_PIPE_INFORMATION));

	if (WinUsb_QueryInterfaceSettings(hpwInfo->winUSBHandle, 0, &ifaceDescriptor))
	{
	  for (int i = 0; i <= ifaceDescriptor.bNumEndpoints - 1; i++)
	  {
		WinUsb_QueryPipe(hpwInfo->winUSBHandle, 0, (unsigned char)i, &pipeInfo);

		if ((pipeInfo.PipeType == UsbdPipeTypeBulk) &&
			 USB_ENDPOINT_DIRECTION_IN(pipeInfo.PipeId))
		{
		  unsigned char ENABLED = 1;
		  int timeout = 2000;

		  if(pipeInfo.PipeId == 0x86)
			 hpwInfo->bulkInPipe = pipeInfo.PipeId;
		  else  // the only other IN pipe is the raw input
			 hpwInfo->rawInPipe  = pipeInfo.PipeId;

		   if (!WinUsb_SetPipePolicy(hpwInfo->winUSBHandle, pipeInfo.PipeId,
									PIPE_TRANSFER_TIMEOUT, sizeof(int), &timeout))
			return false;

		  if (!WinUsb_SetPipePolicy(hpwInfo->winUSBHandle, pipeInfo.PipeId,
									RAW_IO, sizeof(char), &ENABLED))
			return false;
								}
		  else if ((pipeInfo.PipeType == UsbdPipeTypeBulk) &&
					USB_ENDPOINT_DIRECTION_OUT(pipeInfo.PipeId))
			   {
				 unsigned char DISABLED = 0;
				 int timeout = 2000;

				 hpwInfo->bulkOutPipe = pipeInfo.PipeId;

				 if (!WinUsb_SetPipePolicy(hpwInfo->winUSBHandle, pipeInfo.PipeId,
										   SHORT_PACKET_TERMINATE, sizeof(char), &DISABLED))
				   return false;

				 if (!WinUsb_SetPipePolicy(hpwInfo->winUSBHandle, pipeInfo.PipeId,
										   PIPE_TRANSFER_TIMEOUT, sizeof(int), &timeout))
				   return false;
			   }
	  }
	}
	else
	{
	  return false;
	}

	return true;
}
//---------------------------------------------------------------------------
unsigned long ReadFromHPSDR(UCHAR *buf, int size)
{
	ULONG bufSize = size;
	bool bResult;
	ULONG bytesRead = 0;

	bResult = WinUsb_ReadPipe(hpwInfo->winUSBHandle,
							  hpwInfo->bulkInPipe,
							  buf,
							  bufSize,
							  &bytesRead,
							  NULL);
	return bytesRead;
}
//---------------------------------------------------------------------------
bool ReadNBFromHPSDR(UCHAR *buf,  int size, OVERLAPPED& olap)
{
	ULONG bufSize = size;
	bool bResult;
	ULONG bytesRead;

	bResult = WinUsb_ReadPipe(hpwInfo->winUSBHandle,
							  hpwInfo->bulkInPipe,
							  buf,
							  bufSize,
							  &bytesRead,
							  &olap);
	return bResult;
}
//---------------------------------------------------------------------------
bool ReadRawFromHPSDR(UCHAR *buf)
{
	ULONG bufSize = 4096;
	bool bResult;
	ULONG bytesRead;

	bResult = WinUsb_ReadPipe(hpwInfo->winUSBHandle,
							  hpwInfo->rawInPipe,
							  buf,
							  bufSize,
							  &bytesRead,
							  NULL);
	return bResult;
}
//---------------------------------------------------------------------------
bool WriteToHPSDR(UCHAR *buf,  int size)
{
	ULONG bufSize = size;
	bool bResult;
	ULONG bytesWritten;

	bResult = WinUsb_WritePipe(hpwInfo->winUSBHandle,
							   hpwInfo->bulkOutPipe,
							   buf,
							   bufSize,
							   &bytesWritten,
							   NULL);
	return bResult;
}
//---------------------------------------------------------------------------
bool WriteNBToHPSDR(UCHAR *buf,  int size, OVERLAPPED& olap)
{
	ULONG bufSize = size;
	bool bResult;
	ULONG bytesWritten;

	bResult = WinUsb_WritePipe(hpwInfo->winUSBHandle,
							   hpwInfo->bulkOutPipe,
							   buf,
							   bufSize,
							   &bytesWritten,
							   &olap);
	return bResult;
}
//---------------------------------------------------------------------------

#pragma package(smart_init)

