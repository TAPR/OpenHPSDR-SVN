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

#include "hpsdr_common.h"
#include "fpga_load.h"
#include "delay.h"

/*
 * setup altera FPGA serial load (PS).
 *
 * On entry:
 *	don't care
 *
 * On exit:
 *	ALTERA_DCLK    = 0
 *	ALTERA_NCONFIG = 1
 *	ALTERA_NSTATUS = 1 (input)
 */
unsigned char
fpga_load_begin (void)
{
    unsigned char counter = 0;

	HPSDR_ALTERA_CONFIG &= ~bmALTERA_BITS;		// clear all bits (NCONFIG low)
  	udelay (40);					// wait 40 us
  	HPSDR_ALTERA_CONFIG |= bmALTERA_NCONFIG;	// set NCONFIG high

  	while ((HPSDR_ALTERA_CONFIG & bmALTERA_NSTATUS) == 0)
    {
        counter++;
        udelay(50);
        if (counter  >= 255)
        {
            return 0;
        }
    } // wait for NSTATUS to go high
  	// ready to xfer now
  	return 1;
}

/*
 * clock out the low bit of bits.
 *
 * On entry:
 *	ALTERA_DCLK    = 0
 *	ALTERA_NCONFIG = 1
 *	ALTERA_NSTATUS = 1 (input)
 *
 * On exit:
 *	ALTERA_DCLK    = 0
 *	ALTERA_NCONFIG = 1
 *	ALTERA_NSTATUS = 1 (input)
 */


static void
clock_out_config_byte (unsigned char bits) _naked
{
    _asm
	mov	a, dpl

	rrc	a
	mov	_bitALTERA_DATA0,c
	setb	_bitALTERA_DCLK
	clr	_bitALTERA_DCLK

	rrc	a
	mov	_bitALTERA_DATA0,c
	setb	_bitALTERA_DCLK
	clr	_bitALTERA_DCLK

	rrc	a
	mov	_bitALTERA_DATA0,c
	setb	_bitALTERA_DCLK
	clr	_bitALTERA_DCLK

	rrc	a
	mov	_bitALTERA_DATA0,c
	setb	_bitALTERA_DCLK
	clr	_bitALTERA_DCLK

	rrc	a
	mov	_bitALTERA_DATA0,c
	setb	_bitALTERA_DCLK
	clr	_bitALTERA_DCLK

	rrc	a
	mov	_bitALTERA_DATA0,c
	setb	_bitALTERA_DCLK
	clr	_bitALTERA_DCLK

	rrc	a
	mov	_bitALTERA_DATA0,c
	setb	_bitALTERA_DCLK
	clr	_bitALTERA_DCLK

	rrc	a
	mov	_bitALTERA_DATA0,c
	setb	_bitALTERA_DCLK
	clr	_bitALTERA_DCLK

	ret

    _endasm;
}

static void
clock_out_bytes (unsigned char bytecount,
		 unsigned char xdata *p)
{
  while (bytecount-- > 0)
    clock_out_config_byte (*p++);
}

/*
 * Transfer block of bytes from packet to FPGA serial configuration port
 *
 * On entry:
 *	ALTERA_DCLK    = 0
 *	ALTERA_NCONFIG = 1
 *	ALTERA_NSTATUS = 1 (input)
 *
 * On exit:
 *	ALTERA_DCLK    = 0
 *	ALTERA_NCONFIG = 1
 *	ALTERA_NSTATUS = 1 (input)
 */
unsigned char
fpga_load_xfer (xdata unsigned char *p, unsigned char bytecount)
{
  clock_out_bytes (bytecount, p);
  return 1;
}

/*
 * check for successful load...
 */
unsigned char
fpga_load_end (void)
{
  unsigned char status = HPSDR_ALTERA_CONFIG;

  if ((status & bmALTERA_NSTATUS) == 0)		// failed to program
    return 0;

  if ((status & bmALTERA_CONF_DONE) == bmALTERA_CONF_DONE)
    return 1;					// everything's cool

  // I don't think this should happen.  It indicates that
  // programming is still in progress.

  return 0;
}
