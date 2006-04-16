// Copyright (C) Bill Tracey (KD5TFD) 2006 
// email: bill@ewjt.com 

// This code is derived from FPGA4FUN's Xylo sample code - copyright below 
// Sample code for Xylo's USB-2  interface
// (c) fpga4fun.com KNJN LLC - 2005

#include <windows.h>
#include <assert.h>
#include <stdio.h>
#include "private.h"

///////////////////////////////////////////////////
// Open and close the USB driver
HANDLE * XyloOpen()
{
	HANDLE * h;
	h = CreateFile("\\\\.\\EzUSB-0", GENERIC_WRITE, FILE_SHARE_WRITE, NULL, OPEN_EXISTING, 0, NULL);
    if ( h == INVALID_HANDLE_VALUE ) {
		return NULL;
    }
    return h;    
}

void XyloClose(HANDLE *h)
{
	CloseHandle(h);
}

///////////////////////////////////////////////////
// USB functions to send and receive bulk packets
int XyloBulkWrite(HANDLE *h, ULONG pipe, void* buffer, ULONG buffersize)
{
        int nBytes;
        assert(buffersize<0x10000);
        DeviceIoControl(h, 0x222051, &pipe, sizeof(pipe), buffer, buffersize, &nBytes, NULL);

        // assert(nBytes == buffersize);      // make sure everything was sent
        return nBytes;
}



WORD XyloBulkRead(HANDLE *h, ULONG pipe, void* buffer, ULONG buffersize)
{
        int nBytes;
        assert(buffersize<0x10000);
        DeviceIoControl(h, 0x22204E, &pipe, sizeof(pipe), buffer, buffersize, &nBytes, NULL);

        return nBytes;
}



#if 0 
char buf[32768];

int main()
{
        size_t num_read;
        FILE *ofile; /* output file */
                int block_count = 0;

        ofile = fopen("usbsuck.dat", "wb");
        if ( ofile == NULL ) {
            fprintf(stderr, "Failed to open usbsuck.dat\n");
            exit(99);
        }

                printf("Opening USB device...");
        XyloDeviceHandle = USB_Open();
                if ( XyloDeviceHandle == NULL ) {
                        fprintf(stderr, "\nFailed to open Xylo device\n");
                        exit(99);
                }
                printf("opened.\n");

                if ( SetPriorityClass(GetCurrentProcess(), HIGH_PRIORITY_CLASS) == 0 ) {
                        printf("SetPriorityClass failed!!\n");
                }

        while ( 1 ) {
                        // printf("Reading %d bytes...", sizeof(buf));
            num_read = USB_BulkRead(XyloDeviceHandle, 4, buf, sizeof(buf));
                        if ( num_read == sizeof(buf) ) {
                                fprintf(stderr, ".");
                        }
                        else {
                                fprintf(stderr, "X");
                        }
                        if ( block_count % 60 == 0) {
                                fprintf(stderr, "\n");
                        }
                        ++block_count;
                        // printf(".. got %d bytes\n", num_read);
                        if ( num_read != 0 ) {
                                // fprintf(stderr, "num_read: %d\n", num_read);
                                fwrite(buf, 1, num_read, ofile);
                        }
                        // else {
                        //      fprintf(stderr, "0 byte read!\n");
                        //}
                }
                fclose(ofile);
                printf("Closing USB device...");
        USB_Close(XyloDeviceHandle);
                printf("closed.\n");
}

#endif
