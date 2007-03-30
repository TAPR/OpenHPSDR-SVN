/*
 * HPSDR/OZY - High Performance Software Defined Radio, OZY Firmware
 *
 * Adapted from USRP firmware 07/10/2006 by Phil Covington N8VB
 *
 * Copyright 2004 Free Software Foundation, Inc.
 *
 * This file is part of GNU Radio
 *
 * GNU Radio is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * GNU Radio is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with GNU Radio; see the file COPYING.  If not, write to
 * the Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */

#include "../include/fx2regs.h"
#include "../include/hpsdr_common.h"
#include "../include/spi.h"

void
putchar(char c)
{
	// HPSDR_LED_REG ^= bmLED0;
	if ( c == '\n' ) {
		c = '\r'; 
	}
    while(!TI);
    TI=0;
    SBUF0 = c;
}

void putstr(char *s)
{
    char i=0;
    char c;
    while ((c=*(s+(i++)))!=0) putchar(c);
}

void
set_led_0 (unsigned char on)
{
  if (!on)			// active low
    HPSDR_PC |= bmPC_LED0;
  else
    HPSDR_PC &= ~bmPC_LED0;
}

void
set_led_1 (unsigned char on)
{
  if (!on)			// active low
    HPSDR_PC |= bmPC_LED1;
  else
    HPSDR_PC &= ~bmPC_LED1;
}

void
toggle_led_0 (void)
{
  HPSDR_PC ^= bmPC_LED0;
}

void
toggle_led_1 (void)
{
  HPSDR_PC ^= bmPC_LED1;
}

void
init_board (void)
{
	init_spi();

}
