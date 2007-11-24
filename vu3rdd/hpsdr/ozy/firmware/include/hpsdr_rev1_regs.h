/*
 * HPSDR/OZY - High Performance Software Defined Radio, OZY Firmware
 *
 * Adapted from USRP firmware 07/10/2006 by Phil Covington N8VB
 *
 * Copyright (C) 2003 Free Software Foundation, Inc.
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
 * These are the register definitions for the Rev 1 HPSDR/OZY */

#ifndef _HPSDR_REV1_REGS_H_
#define _HPSDR_REV1_REGS_H_

#include "fx2regs.h"

//-------------------------------------------------------------
/*  Port A (bit addressable): */

#define HPSDR_PA		IOA		// Port A
#define HPSDR_PA_OE	        OEA		// Port A direction register

#define bmPA_S_CLK		bmBIT0		// SPI serial clock
#define bmPA_S_DATA_TO_PERIPH	bmBIT1		// SPI SDI (peripheral rel name)
#define bmPA_FIFO_SLOE		bmBIT2		// FIFO SLOE line
#define bmPA_S_DATA_FROM_PERIPH	bmBIT3		// SPI SDO (peripheral rel name)
#define bmPA_FIFO_ADR0		bmBIT4		// FIFO Address line 0
#define bmPA_FIFO_ADR1		bmBIT5		// FIFO Address line 1
#define bmPA_FIFO_PKTEND	bmBIT6		// FIFO Packet End
#define bmPA_FIFO_SLCS		bmBIT7		// FIFO SLCS line

sbit at 0x80+0 bitS_CLK;		        // 0x80 is the bit address of PORT A
sbit at 0x80+1 bitS_OUT;		        // out from FX2 point of view
sbit at 0x80+3 bitS_IN;			        // in from FX2 point of view


/* all outputs except S_DATA_FROM_PERIPH, FX2_2, FX2_3 */

#define bmPORT_A_OUTPUTS  (bmPA_S_CLK  | bmPA_S_DATA_TO_PERIPH)

#define bmPORT_A_INITIAL	0x00

//-------------------------------------------------------------
/* Port B: FIFO FD[7:0]	*/

//-------------------------------------------------------------
/* Port C (bit addressable): */

#define HPSDR_PC		IOC		// Port C
#define HPSDR_PC_OE		OEC		// Port C direction register

#define HPSDR_ALTERA_CONFIG	HPSDR_PC

#define bmALTERA_DATA0		bmBIT0
#define bmALTERA_NCONFIG	bmBIT1
#define bmALTERA_DCLK		bmBIT2
#define bmALTERA_CONF_DONE	bmBIT3
#define bmALTERA_NSTATUS	bmBIT4
#define bmALTERA_NCE		bmBIT5
#define bmPC_LED0		bmBIT6		// active low
#define bmPC_LED1		bmBIT7		// active low

sbit at 0xA0+0 bitALTERA_DATA0;		// 0xA0 is the bit address of PORT C
sbit at 0xA0+2 bitALTERA_DCLK;


#define bmALTERA_BITS	(bmALTERA_DATA0 			\
			 | bmALTERA_NCONFIG	 				\
			 | bmALTERA_DCLK 					\
			 | bmALTERA_CONF_DONE				\
			 | bmALTERA_NSTATUS | bmALTERA_NCE	\
			 )

#define bmPORT_C_OUTPUTS	(bmALTERA_DATA0 		\
				 | bmALTERA_NCONFIG					\
				 | bmALTERA_DCLK					\
				 | bmALTERA_NCE						\
				 | bmPC_LED0						\
				 | bmPC_LED1						\
				 )


#define bmPORT_C_INITIAL	(bmPC_LED0 | bmPC_LED1)


#define HPSDR_LED_REG	HPSDR_PC
#define bmLED0			bmPC_LED0
#define bmLED1			bmPC_LED1

//-------------------------------------------------------------
/* Port D: FIFO FD[15:8] */

//-------------------------------------------------------------
/* Port E: not bit addressible		*/

#define HPSDR_PE			IOE		// Port E
#define HPSDR_PE_OEOEE		// Port E direction register

#define bmPA_CS_FPGA		bmPE_PE7

#define bmPE_PE0			bmBIT0		//
#define bmPE_PE1			bmBIT1		//
#define bmPE_PE2			bmBIT2		//
#define bmPE_PE3			bmBIT3		//
#define bmPE_PE4			bmBIT4		//
#define bmPE_PE5			bmBIT5		//
#define bmPE_PE6			bmBIT6		//
#define bmPE_PE7			bmBIT7		//


#define	bmPORT_E_OUTPUTS	(bmPE_PE0	\
				 | bmPE_PE1 	 \
				 | bmPE_PE2	 \
				 | bmPE_PE3	  \
				 /* | bmPE_PE4 */   \
				 | bmPE_PE5   \
				 | bmPE_PE6   \
				 | bmPE_PE7   \
				 )


#define	bmPORT_E_INITIAL	0x00 

#endif /* _HPSDR_REV1_REGS_H_ */
