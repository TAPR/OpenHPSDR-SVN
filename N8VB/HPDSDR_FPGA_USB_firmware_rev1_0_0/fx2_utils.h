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

// FX2 UTILITIES

#ifndef FX2_UTILS_H   /* Header Sentry */
#define FX2_UTILS_H

#include <stdio.h>
#include "fx2regs.h"

// function prototypes

void FX2_Delay1ms (void);
void FX2_Delay(WORD);
STRINGDSCR xdata *	FX2_GetStringDscr(BYTE);
void FX2_Discon(BOOL renum);

/* Part of Delay routine FX2_Delay() */

void FX2_Delay1ms (void)
{
	_asm

	mov	A, #0           
	mov _DPS,A   
	mov DPTR,#(0xffff-602)
	mov R4,#5

	loop:           
	inc     DPTR            
	mov     A,DPL           
	orl     A,DPH           
	jnz     loop                      

	er_end:     
	ret

	_endasm;
}

/* Delay routine which loops for x milliseconds */

void FX2_Delay(WORD ms)
{
	//
	// Adjust the delay based on the CPU clock
	// FX2_Delay1ms() assumes a 24MHz clock
	//
	if ((CPUCS & bmCLKSPD) == 0)			  // 12Mhz
		ms = (ms + 1) / 2;					   // Round up before dividing so we can accept 1.
	else if ((CPUCS & bmCLKSPD) == bmCLKSPD1)	// 48Mhz
		ms = ms * 2;

	while (ms--)
		FX2_Delay1ms();

	return;
}


// renumeration for FX2

void FX2_Discon(BOOL renum)
{
   if(renum)                                // If renumerate (i.e. 8051 will handle SETUP commands)
      FX2_USB_DISCONNECT_AND_RENUM();       // disconnect from USB and set the renumerate bit
   else
      FX2_USB_DISCONNECT();                 // just disconnect from USB
		
   FX2_Delay(1500);      					// Wait 1500 ms

   USBIRQ = 0xff;          					// Clear any pending USB interrupt requests.
   EPIRQ = 0xff;

   FX2_IRQ_CLEAR();
   FX2_USB_CONNECT();      					// reconnect USB
}


#endif //FX2_UTILS_H
