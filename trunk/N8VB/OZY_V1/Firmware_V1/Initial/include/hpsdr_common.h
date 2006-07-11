/*
 * HPSDR/OZY - High Performance Software Defined Radio, OZY Firmware
 *
 * Adapted from USRP firmware 07/10/2006 by Phil Covington N8VB
 *
 * Copyright (C) 2003,2006 Free Software Foundation, Inc.
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
 * common defines and prototypes for HPSDR
 *
 * In comments below "TRM" refers to the EZ-USB FX2 Technical Reference Manual
 */

 // Borrowed from USRP code 07/07/2006 Phil C N8VB

#ifndef _HPSDRCOMMON_H_
#define _HPSDRCOMMON_H_

#include "hpsdr_config.h"
#include "hpsdr_rev1_regs.h"
#include "syncdelay.h"

/*
 * From TRM page 15-105:
 *
 * Under certain conditions, some read and write access to the FX2
 * registers must be separated by a "synchronization delay".  The
 * delay is necessary only under the following conditions:
 *
 *   - between a write to any register in the 0xE600 - 0xE6FF range 
 *     and a write to one of the registers listed below.
 *
 *   - between a write to one of the registers listed below and a read 
 *     from any register in the 0xE600 - 0xE6FF range.
 *
 *   Registers which require a synchronization delay:
 *
 *	FIFORESET			FIFOPINPOLAR
 *	INPKTEND			EPxBCH:L
 *	EPxFIFOPFH:L			EPxAUTOINLENH:L
 *	EPxFIFOCFG			EPxGPIFFLGSEL
 *	PINFLAGSAB			PINFLAGSCD
 *	EPxFIFOIE			EPxFIFOIRQ
 *	GPIFIE				GPIFIRQ
 *	UDMACRCH:L			GPIFADRH:L
 *	GPIFTRIG			EPxGPIFTRIG
 *	OUTPKTEND			REVCTL
 *	GPIFTCB3			GPIFTCB2
 *	GPIFTCB1			GPIFTCB0
 */

#define	TRUE		1
#define	FALSE		0


void init_hpsdr (void);

void set_led_0 (unsigned char on);
void set_led_1 (unsigned char on);
void toggle_led_0 (void);
void toggle_led_1 (void);

#endif /* _HPSDRCOMMON_H_ */
