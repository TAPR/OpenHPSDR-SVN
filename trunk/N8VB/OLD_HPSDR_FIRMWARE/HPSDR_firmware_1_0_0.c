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

#define ALLOCATE_EXTERN // Allocate explicit register addresses here

#include "hpsdr.h"
#include "fx2regs.h"
#include "fx2_macros.h"
#include "fx2_types.h"
#include "fx2sdly.h"
#include "fx2_utils.h"

void init_fx2 (void)
{
	FX2_SET_CLKSPD_48();			// CPU runs at 48 MHz
	FX2_SET_REVCTL();				// setup as per TRM recommendation
	FX2_SPDAUTO_ENABLE();			// setup autopointer enabled
	FX2_RESET_EP1OUT_STALL();		// reset stall bit on EP1 OUT 
	FX2_ARM_EP1OUT();				// re-arm for EP1 out transfer
	FX2_RESET_EP1IN_STALL();		// reset if EP1 is stalled
	FX2_CLR_IFCONFIG();				// clear ifclock config
	FX2_SET_IFCLKSRC_INT();			// set to internal ifclock source
	FX2_SET_FIFOGPIF_CLK48();		// set ifclock speed to 48 Mhz
}

void setup_io (void)
{
	FX2_CLR_PORTACFG();				// port a all gp io
	FX2_CLR_PORTECFG();				// port e all gp io

	OEA=0x03;						// pins 1 and 2 output
	IOA=0x03;

	OEE=0x02;						// pin 2 output
	IOE=0x02;
}

void setup_interrupts (void)
{
	FX2_USB_INT_ENABLE();								// enable USB interrupts
	FX2_INT2_AV_ENABLE();								// USB autovector enable
	FX2_USB_INT_MASK(bmSUDAV | bmSUTOK | bmHSGRANT);	// enable SUDAV, SUTOK, and HSGRANT interrupts
	FX2_USB_EPIE_MASK(bmEP1OUT);						// enable EP1OUT interrupt
	FX2_8051_INT_ENABLE();								// enable 8051 interrupts
}

void setup_descriptor_pointers (void)
{
	pDeviceDscr = (WORD)&DeviceDscr;
	pDeviceQualDscr = (WORD)&DeviceQualDscr;
	pHighSpeedConfigDscr = (WORD)&HighSpeedConfigDscr;
	pFullSpeedConfigDscr = (WORD)&FullSpeedConfigDscr;
	pStringDscr = (WORD)&StringDscr0;
	pConfigDscr = pHighSpeedConfigDscr;
	pOtherConfigDscr = pFullSpeedConfigDscr;
}

void clear_ext_ram(void)
{
	WORD i;

	pExtMemory = (WORD xdata *)0x4000;

	for (i=0; i < 0x1000; i++)
	{
		*(pExtMemory++) = 0xFFFF; 
	}
}

// Ep1 data (EP0) request 
void process_ep1_data(void)
{
	BYTE xdata i;

	if (!(EP1INCS & bmEPBUSY))		// can we send on EP1 ?
	{
		if (EP1OUTBUF[0] == 1)
		{
			if (EP1OUTBUF[1] == 1)
			{
				IOA = 0x03;
			} else
			{
				IOA = 0x02;
			}
		} else if (EP1OUTBUF[0] == 2)
		{
			pExtMemory = (WORD xdata *)(EP1OUTBUF[1] + (EP1OUTBUF[2]<<8));
			pExtByteMemory = (BYTE xdata *)pExtMemory;

			for (i=0; i < EP1OUTBUF[3]; i++)
			{
				EP1INBUF[i] = *(pExtByteMemory++); 
			}
			FX2_RESET_EP1IN_STALL();
			EP1INBC = i + 1;			// bytes to transfer, initates send on EP1 IN
		} else if (EP1OUTBUF[0] == 3)
		{
			pExtMemory = (WORD xdata *)(EP1OUTBUF[1] + (EP1OUTBUF[2]<<8));
			pExtByteMemory = (BYTE xdata *)pExtMemory;

			for (i=0; i < EP1OUTBUF[3]; i++)
			{
				*(pExtByteMemory++) = (BYTE)EP1OUTBUF[i+4]; 
			}
		}
	}

	FX2_RESET_EP1OUT_STALL();   ;	//reset stall bit on EP1 OUT
	FX2_ARM_EP1OUT();				//re-arm for EP1 out transfer
}

// Setup Command (EP0) data request
void process_ep0_data(void)
{
	STRINGDSCR xdata *   dscr;
	BOOL found;
	BYTE index;

	switch (SETUPDAT[1])
	{
	case SC_GET_DESCRIPTOR:					 // *** Get Descriptor

		switch (SETUPDAT[3])
		{
		case GD_DEVICE:						// Device
			SUDPTRH = MSB(pDeviceDscr);
			SUDPTRL = LSB(pDeviceDscr);
			break;
		case GD_DEVICE_QUALIFIER:			 // Device Qualifier
			SUDPTRH = MSB(pDeviceQualDscr);
			SUDPTRL = LSB(pDeviceQualDscr);
			break;
		case GD_CONFIGURATION:				// Configuration
			SUDPTRH = MSB(pConfigDscr);
			SUDPTRL = LSB(pConfigDscr);
			break;
		case GD_OTHER_SPEED_CONFIGURATION:		// Other Speed Configuration
			SUDPTRH = MSB(pOtherConfigDscr);
			SUDPTRL = LSB(pOtherConfigDscr);
			break;
		case GD_STRING:						// String
			found = FALSE;
			index = SETUPDAT[2];
			dscr = (STRINGDSCR xdata *) pStringDscr;
			while (dscr->type == STRING_DSCR)
			{
				if (!index--)
				{
					SUDPTRH = MSB(dscr);
					SUDPTRL = LSB(dscr);
					found = TRUE;
					break;
				}
				dscr = (STRINGDSCR xdata *)((WORD)dscr + dscr->length);
			}
			if (!found)
			{
				FX2_STALL_EP0();		// Stall End Point 0
			}
			break;
		default:							// Invalid request
			FX2_STALL_EP0();			// Stall End Point 0
		}
		break;

	case SC_GET_INTERFACE:				 // *** Get Interface
		EP0BUF[0] = AlternateSetting;
		EP0BCH = 0;
		EP0BCL = 1;
		break;

	case SC_SET_INTERFACE:				 // *** Set Interface
		AlternateSetting = SETUPDAT[2];
		break;

	case SC_SET_CONFIGURATION:			 // *** Set Configuration
		Configuration = SETUPDAT[2];
		break;

	case SC_GET_CONFIGURATION:			 // *** Get Configuration
		EP0BUF[0] = Configuration;
		EP0BCH = 0;
		EP0BCL = 1;
		break;

	case SC_GET_STATUS:					 // *** Get Status

		switch (SETUPDAT[0])
		{
		case GS_DEVICE:					 // Device
			EP0BUF[0] = ((BYTE)RWUEN << 1) | (BYTE)SELFPWR;
			EP0BUF[1] = 0;
			EP0BCH = 0;
			EP0BCL = 2;
			break;
		case GS_INTERFACE:				 // Interface
			EP0BUF[0] = 0;
			EP0BUF[1] = 0;
			EP0BCH = 0;
			EP0BCL = 2;
			break;
		case GS_ENDPOINT:				 // End Point
			EP0BUF[0] = *(BYTE xdata *) epcs(SETUPDAT[4]) & bmEPSTALL;
			EP0BUF[1] = 0;
			EP0BCH = 0;
			EP0BCL = 2;
			break;
		default:						 // Invalid Command
			FX2_STALL_EP0();			  // Stall End Point 0
		}
		break;

	case SC_CLEAR_FEATURE:				  // *** Clear Feature

		switch (SETUPDAT[0])
		{
		case FT_DEVICE:				 // Device
			if (SETUPDAT[2] == 1)
				RWUEN = FALSE;		   // Disable Remote Wakeup
			else
				FX2_STALL_EP0();	   // Stall End Point 0
			break;
		case FT_ENDPOINT:			 // End Point
			if (SETUPDAT[2] == 0)
			{
				*(BYTE xdata *) epcs(SETUPDAT[4]) &= ~bmEPSTALL;
				FX2_RESET_DATA_TOGGLE( SETUPDAT[4] );
			} else
				FX2_STALL_EP0();	   // Stall End Point 0
			break;
		}
		break;

	case SC_SET_FEATURE:				  // *** Set Feature

		switch (SETUPDAT[0])
		{
		case FT_DEVICE:			   // Device

			if (SETUPDAT[2] == 1)
				RWUEN = TRUE;	   // Enable Remote Wakeup
			else if (SETUPDAT[2] == 2)
				break;

			else
				FX2_STALL_EP0();   // Stall End Point 0
			break;

		case FT_ENDPOINT:		  // End Point
			*(BYTE xdata *) epcs(SETUPDAT[4]) |= bmEPSTALL;
			break;
		}
		break;

	default:					 // *** Invalid Command
		FX2_STALL_EP0();		 // Stall End Point 0

	}

	FX2_ACK_HANDSHAKE(); // Acknowledge handshake phase of device request
}

// FX2 LOOPS HERE

void main_loop (void)
{
	while (TRUE)
	{
		if (RxSUD)
		{
			process_ep0_data();			// do setup command
			RxSUD = FALSE;				// clear SUDAV flag
		}

		if (EP1HASDATA)
		{
			process_ep1_data();			// do ep1 data
			EP1HASDATA = FALSE;			// clear EP1 flag
		}
	}
}

// MAIN (USER PROGRAM STARTS HERE)

void main (void)
{
	RxSUD = FALSE;
	EP1HASDATA = FALSE;

	init_fx2();
	setup_io();
	setup_descriptor_pointers();
	setup_interrupts();

	if (FX2_NEED_TO_RENUM())
	{
		FX2_Discon(TRUE);				// renumerate
	}

	FX2_USB_CONNECT();					// Connect USB
	FX2_DATA_WAIT_STATE(0);				// MOVX takes 2 cycles (valid 0-7)

	IOA = 0x02;
	IOE = 0x02;

	main_loop();
}

// USB INTERRUPT HANDLERS 

// Setup Data Available interrupt Handler
void ISR_Sudav(void) interrupt  
{
	RxSUD = TRUE;			 // Set flag
	FX2_IRQ_CLEAR();
	FX2_CLEAR_USB_IRQ(bmSUDAV);	 // Clear SUDAV IRQ
}

// Setup Token interrupt Handler
void ISR_Sutok(void) interrupt  
{
	FX2_IRQ_CLEAR();
	FX2_CLEAR_USB_IRQ(bmSUTOK);	 // Clear SUTOK IRQ
}

void ISR_Usb_Reset(void) interrupt  
{
	// whenever we get a USB reset, we should revert to full speed mode
	pConfigDscr = pFullSpeedConfigDscr;
	pOtherConfigDscr = pHighSpeedConfigDscr;

	FX2_IRQ_CLEAR();
	FX2_CLEAR_USB_IRQ(bmURES);	 // Clear URES IRQ
}

void ISR_Highspeed(void) interrupt  
{
	if (FX2_HIGHSPEED())
	{
		pConfigDscr = pHighSpeedConfigDscr;
		pOtherConfigDscr = pFullSpeedConfigDscr;
	}

	FX2_IRQ_CLEAR();
	FX2_CLEAR_USB_IRQ(bmHSGRANT);	 // Clear HSGrant IRQ
}

void ISR_Ep1out(void) interrupt
{
	EP1HASDATA = TRUE;			  // Set flag
	FX2_IRQ_CLEAR();
	FX2_CLEAR_USB_EPIRQ(bmEP1OUT);	 // Clear EP1OUT IRQ
}
