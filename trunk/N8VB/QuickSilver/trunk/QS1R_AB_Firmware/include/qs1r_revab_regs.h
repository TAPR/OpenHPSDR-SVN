/*
 * QuickSilver QS1R Firmware
 *
 * Adapted from USRP firmware 12/07/2006 by Phil Covington N8VB
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
 * These are the register definitions for the Rev AB QS1R */

#ifndef _QS1R_REVAB_REGS_H_
#define _QS1R_REVAB_REGS_H_

#define ALLOCATE_EXTERN		// required

#include "fx2regs.h"

#ifdef ALLOCATE_EXTERN
#define EXTERN
#define _AT_(a) at a
#else
#define EXTERN extern
#define _AT_ ;/ ## /
#endif

//-------------------------------------------------------------
/*  Port A (bit addressable): */

#define QS1R_PA		IOA		// Port A
#define QS1R_PA_OE	OEA		// Port A direction register

#define bmPA_S_DATA_TO_PERIPH	    bmBIT0		// SPI SDI (peripheral rel name)
#define bmPA_S_DATA_FROM_PERIPH	bmBIT1		// SPI SDO (peripheral rel name)
#define bmPA_FIFO_SLOE				    bmBIT2		// FIFO SLOE line
#define bmPA_S_CLK					        bmBIT3		// SPI serial clock
#define bmPA_FIFO_ADR0				    bmBIT4		// FIFO Address line 0
#define bmPA_FIFO_ADR1				    bmBIT5		// FIFO Address line 1
#define bmPA_FIFO_PKTEND			    bmBIT6		// FIFO Packet End
#define bmPA_FIFO_SLCS				    bmBIT7		// FIFO SLCS line

sbit at 0x80+0 bitS_OUT;		// out from FX2 point of view
sbit at 0x80+1 bitS_IN;		// in from FX2 point of view
sbit at 0x80+3 bitS_CLK;		// 0x80 is the bit address of PORT A

#define bmPORT_A_OUTPUTS  (bmPA_S_CLK  | bmPA_S_DATA_TO_PERIPH)

#define bmPORT_A_INITIAL	0x00

//-------------------------------------------------------------
/* Port B: FIFO FD[7:0]	*/

//-------------------------------------------------------------
/* Port C (bit addressable): */

#define QS1R_PC		    IOC		// Port C
#define QS1R_PC_OE		OEC		// Port C direction register

#define QS1R_ALTERA_CONFIG	QS1R_PC

#define bmALTERA_DATA0		    bmBIT0
#define bmALTERA_NCONFIG		bmBIT1
#define bmALTERA_DCLK			bmBIT2
#define bmALTERA_CONF_DONE	bmBIT3
#define bmALTERA_NSTATUS		bmBIT4
#define bmALTERA_NCE			    bmBIT5
#define bmCS_FPGA_N			    bmBIT6		// active low
#define bmCS_CPLD_N			    bmBIT7		// active low

sbit at 0xA0+0 bitALTERA_DATA0;		// 0xA0 is the bit address of PORT C
sbit at 0xA0+2 bitALTERA_DCLK;
sbit at 0xA0+6 bitFPGA_CS;
sbit at 0xA0+7 bitCPLD_CS;

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
				 | bmCS_FPGA_N						\
				 | bmCS_CPLD_N						\
				 )


#define bmPORT_C_INITIAL	0x00
//-------------------------------------------------------------
/* Port D: FIFO FD[15:8] */

//-------------------------------------------------------------
/* Port E: not bit addressible		*/

#define QS1R_PE			IOE		// Port E
#define QS1R_PE_OE	    OEE		// Port E direction register

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
				 | bmPE_PE3	 \
				 | bmPE_PE4	 \
				 | bmPE_PE5   \
				 | bmPE_PE6   \
				 | bmPE_PE7   \
				 )


#define	bmPORT_E_INITIAL	0x00

// Memory mapped IO

EXTERN xdata _AT_(0x6000) volatile BYTE DDC0_DR0;
EXTERN xdata _AT_(0x6001) volatile BYTE DDC0_DR1;
EXTERN xdata _AT_(0x6002) volatile BYTE DDC0_DR2;
EXTERN xdata _AT_(0x6003) volatile BYTE DDC0_DR3;
EXTERN xdata _AT_(0x6004) volatile BYTE DDC0_DR4;
EXTERN xdata _AT_(0x6006) volatile BYTE DDC0_LAR;
EXTERN xdata _AT_(0x6007) volatile BYTE DDC0_AMR;

EXTERN xdata _AT_(0x6010) volatile BYTE DDC1_DR0;
EXTERN xdata _AT_(0x6011) volatile BYTE DDC1_DR1;
EXTERN xdata _AT_(0x6012) volatile BYTE DDC1_DR2;
EXTERN xdata _AT_(0x6013) volatile BYTE DDC1_DR3;
EXTERN xdata _AT_(0x6014) volatile BYTE DDC1_DR4;
EXTERN xdata _AT_(0x6016) volatile BYTE DDC1_LAR;
EXTERN xdata _AT_(0x6017) volatile BYTE DDC1_AMR;

EXTERN xdata _AT_(0x6020) volatile BYTE DDC_CNTRL;
EXTERN xdata _AT_(0x6021) volatile BYTE LTC2208_CNTRL_REG;
EXTERN xdata _AT_(0x6022) volatile BYTE DEBUG_LED_REG;
EXTERN xdata _AT_(0x6023) volatile BYTE ATTN;
EXTERN xdata _AT_(0x6024) volatile BYTE RFFE_CNTRL;
EXTERN xdata _AT_(0x6025) volatile BYTE CPLD_GPIO;

#endif /* _QS1R_REVAB_REGS_H_ */
