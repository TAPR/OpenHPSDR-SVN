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

// FX2 MACRO DEFINITIONS

#ifndef FX2_MACROS_H   /* Header Sentry */
#define FX2_MACROS_H

#include "fx2regs.h"

// macro for generating the address of an endpoint's control and status register (EPnCS)
#define epcs(EP) (EPCS_Offset_Lookup_Table[(EP & 0x7E) | (EP > 128)] + 0xE6A1)

// macro to reset and endpoint data toggle
#define FX2_RESET_DATA_TOGGLE(ep)	    TOGCTL = (((ep & 0x80) >> 3) + (ep & 0x0F));\
                                        TOGCTL |= bmRESETTOGGLE

#define MSB(word)	(BYTE)(((WORD)(word) >> 8) & 0xff)
#define LSB(word)   (BYTE)((WORD)(word) & 0xff)

#define FX2_USB_INT_ENABLE()   EIE |= bmBIT0
#define FX2_USB_INT_DISABLE()   EIE &=~bmBIT0
#define FX2_IRQ_CLEAR()   EXIF &=~0x10  

#define FX2_INT2_AV_ENABLE() INTSETUP |= bmAV2EN    // Enable INT 2 (USB) autovector

#define FX2_8051_INT_ENABLE() IE |= bmBIT7           // Enable 8051 interrupts
#define FX2_8051_INT_DISABLE() IE &=~bmBIT7          // Disable 8051 interrupts

#define FX2_USB_INT_MASK(x)  USBIE |= (x)  			// Enable selected interrupts in USBIE
#define FX2_USB_EPIE_MASK(x)  EPIE |= (x)			// Enable selected interrupts in EPIE

#define FX2_HIGHSPEED()	(USBCS & bmHSM)				// switch to high speed

#define FX2_CLEAR_USB_IRQ(x)  USBIRQ = (x) 			// Clear USB IRQ in USBIRQ
#define FX2_CLEAR_USB_EPIRQ(x) EPIRQ = (x)			// Clear USB IRQ in EPIRQ

#define FX2_USB_CONNECT()	USBCS &=~bmDISCON
#define FX2_USB_DISCONNECT()	USBCS |=bmDISCON

#define FX2_USB_SET_RENUM()  USBCS |= bmRENUM
#define FX2_USB_CLR_RENUM() USBCS &=~bmRENUM
#define FX2_USB_DISCONNECT_AND_RENUM() USBCS |= (bmDISCON | bmRENUM)

#define FX2_NEED_TO_RENUM() (!(USBCS & bmRENUM))

#define FX2_STALL_EP0()	EP0CS |= bmEPSTALL

#define FX2_SPDAUTO_ENABLE() SUDPTRCTL = 1
#define FX2_SPDAUTO_DISABLE()  SUDPTRCTL = 0

#define FX2_DATA_WAIT_STATE(x) CKCON = x			// valid values are between 0 and 7

#define FX2_RESET_EP1IN_STALL() EP1INCS &= ~bmEPSTALL
#define FX2_RESET_EP1OUT_STALL() EP1OUTCS &= ~bmEPSTALL

#define FX2_ACK_HANDSHAKE()  EP0CS |= bmHSNAK

#define FX2_SET_CLK_SPEED(x) CPUCS = x

#define FX2_ARM_EP1OUT() EP1OUTBC = 0x40;			//re-arm for EP1 out transfer

#define FX2_SET_REVCTL() SYNCDELAY;REVCTL = 0x03;SYNCDELAY	// TRM recommends setting bit 0 and bit 1 

// IFCONFIG SETTINGS
#define FX2_CLR_IFCONFIG()			IFCONFIG = 0
#define FX2_SET_IFCLKSRC_EXT()  	IFCONFIG &= ~bmIFCLKSRC
#define FX2_SET_IFCLKSRC_INT()  	IFCONFIG |= bmIFCLKSRC
#define FX2_SET_FIFOGPIF_CLK30()  	IFCONFIG &= ~bm3048MHZ
#define FX2_SET_FIFOGPIF_CLK48()  	IFCONFIG |= bm3048MHZ
#define FX2_IFCLK_OUT_ENABLE()		IFCONFIG |= bmIFCLKOE
#define FX2_IFCLK_OUT_DISABLE()		IFCONFIG &= ~bmIFCLKOE
#define FX2_IFCLK_POL_NORM()		IFCONFIG &= ~bmIFCLKPOL
#define FX2_IFCLK_POL_INV()			IFCONFIG |= bmIFCLKPOL
#define FX2_SET_FIFOGPIF_SYNC()		IFCONFIG &= ~bmASYNC
#define FX2_SET_FIFOGPIF_ASYNC()	IFCONFIG |= bmASYNC
#define FX2_SET_PE_GSTATE_EN()		IFCONFIG |= bmGSTATE
#define FX2_SET_PE_GSTATE_DIS()		IFCONFIG &= ~bmGSTATE
#define FX2_SET_INTRF_MODE_PORTS()	IFCONFIG &= ~(bmIFCFG1 | bmIFCFG0)
#define FX2_SET_INTRF_MODE_GPIF()	IFCONFIG |= 0x02
#define FX2_SET_INTRF_MODE_FIFO()	IFCONFIG |= 0x03

// PORTACFG SETTINGS
#define FX2_CLR_PORTA_CFG()			PORTACFG = 0x00
#define FX2_SET_PORTA7_IOPIN() 		PORTACFG &= ~(bmFLAGD | bmSLCS)
#define FX2_SET_PORTA7_FLAGD()		PORTACFG |= bmFLAGD;PORTACFG &= ~bmSLCS
#define FX2_SET_PORTA7_SLCS()		PORTACFG |= bmSLCS;PORTACFG &= ~bmFLAGD
#define FX2_SET_PORTA1_IOPIN()		PORTACFG &= ~bmINT1
#define FX2_SET_PORTA0_IOPIN()		PORTACFG &= ~bmINT0
#define FX2_SET_PORTA1_INT1()		PORTACFG |= bmINT1
#define FX2_SET_PORTA0_INT0()		PORTACFG |= bmINT0

// PORTCCFG SETTINGS
#define FX2_CLR_PORTC_CFG()			PORTCCFG = 0x00
#define FX2_SET_PORTC7_GPIFA7()		PORTCCFG |= bmGPIFA7
#define FX2_SET_PORTC6_GPIFA6()		PORTCCFG |= bmGPIFA6
#define FX2_SET_PORTC5_GPIFA5()		PORTCCFG |= bmGPIFA5
#define FX2_SET_PORTC4_GPIFA4()		PORTCCFG |= bmGPIFA4
#define FX2_SET_PORTC3_GPIFA3()		PORTCCFG |= bmGPIFA3
#define FX2_SET_PORTC2_GPIFA2()		PORTCCFG |= bmGPIFA2
#define FX2_SET_PORTC1_GPIFA1()		PORTCCFG |= bmGPIFA1
#define FX2_SET_PORTC0_GPIFA0()		PORTCCFG |= bmGPIFA0
#define FX2_SET_PORTC0_GPIFALL()	PORTCCFG |= 0xFF

// PORTECFG SETTINGS
#define FX2_CLR_PORTE_CFG()			PORTECFG=0x00
#define FX2_SET_PORTE7_GPIFA8()		PORTECFG |= bmGPIFA8		
#define FX2_SET_PORTE6_T2EX()		PORTECFG |= bmT2EX 
#define FX2_SET_PORTE5_INT6()		PORTECFG |= bmINT6
#define FX2_SET_PORTE4_RXD1OUT()	PORTECFG |= bmRXD1OUT
#define FX2_SET_PORTE3_RXD0OUT()	PORTECFG |= bmRXD0OUT
#define FX2_SET_PORTE2_T2OUT()		PORTECFG |= bmT2OUT
#define FX2_SET_PORTE1_T1OUT()		PORTECFG |= bmT1OUT
#define FX2_SET_PORTE0_T0OUT()		PORTECFG |= bmT0OUT

// CLK SPEED SETTINGS
#define FX2_SET_CLKSPD_12()	CPUCS &= ~bmCLKSPD
#define FX2_SET_CLKSPD_24()	CPUCS |= bmCLKSPD0
#define FX2_SET_CLKSPD_48()	CPUCS |= bmCLKSPD1

#endif //FX2_MACROS_H
