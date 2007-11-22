/*
 * HPSDR/OZY - High Performance Software Defined Radio, OZY Firmware
 *
 * Adapted from USRP firmware 07/10/2006 by Phil Covington N8VB
 *
 * Copyright 2004,2006 Free Software Foundation, Inc.
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

#include "spi.h"
#include "hpsdr_rev1_regs.h"

static void
setup_enables (unsigned char enables)
{

    if (enables == 0)
        HPSDR_PE = 0x0;
    else
        HPSDR_PE = HPSDR_PE | enables;
  
}

#define disable_all()	setup_enables (0)

void
init_spi (void)
{
  disable_all ();		/* disable all devs	  */
  bitS_OUT = 0;			/* idle state has CLK = 0 */
}

#if 0
static unsigned char
count_bits8 (unsigned char v)
{
  static unsigned char count4[16] = {
    0,	// 0
    1,	// 1
    1,	// 2
    2,	// 3
    1,	// 4
    2,	// 5
    2,	// 6
    3,	// 7
    1,	// 8
    2,	// 9
    2,	// a
    3,	// b
    2,	// c
    3,	// d
    3,	// e
    4	// f
  };
  return count4[v & 0xf] + count4[(v >> 4) & 0xf];
}

#else

static unsigned char
count_bits8 (unsigned char v)
{
  unsigned char count = 0;
  if (v & (1 << 0)) count++;
  if (v & (1 << 1)) count++;
  if (v & (1 << 2)) count++;
  if (v & (1 << 3)) count++;
  if (v & (1 << 4)) count++;
  if (v & (1 << 5)) count++;
  if (v & (1 << 6)) count++;
  if (v & (1 << 7)) count++;
  return count;
}
#endif

static void
write_byte_msb (unsigned char v);

static void
write_bytes_msb (const xdata unsigned char *buf, unsigned char len);

static void
read_bytes_msb (xdata unsigned char *buf, unsigned char len);


// returns non-zero if successful, else 0
unsigned char
spi_read (unsigned char header_hi, unsigned char header_lo,
	  unsigned char enables, unsigned char format,
	  xdata unsigned char *buf, unsigned char len)
{
  if (count_bits8 (enables) > 1)
    return 0;		// error, too many enables set

  setup_enables (enables);

  if (format & SPI_FMT_LSB){		// order: LSB
#if 1
    return 0;		// error, not implemented
#else
    switch (format & SPI_FMR_HDR_MASK){
    case SPI_FMT_HDR_0:
      break;
    case SPI_FMT_HDR_1:
      write_byte_lsb (header_lo);
      break;
    case SPI_FMT_HDR_2:
      write_byte_lsb (header_lo);
      write_byte_lsb (header_hi);
      break;
    default:
      return 0;		// error
    }
    if (len != 0)
      read_bytes_lsb (buf, len);
#endif
  }

  else {		// order: MSB

    switch (format & SPI_FMT_HDR_MASK){
    case SPI_FMT_HDR_0:
      break;
    case SPI_FMT_HDR_1:
      write_byte_msb (header_lo);
      break;
    case SPI_FMT_HDR_2:
      write_byte_msb (header_hi);
      write_byte_msb (header_lo);
      break;
    default:
      return 0;		// error
    }
    if (len != 0)
      read_bytes_msb (buf, len);
  }

  disable_all ();
  return 1;		// success
}


// returns non-zero if successful, else 0
unsigned char
spi_write (unsigned char header_hi, unsigned char header_lo,
	   unsigned char enables, unsigned char format,
	   const xdata unsigned char *buf, unsigned char len)
{
  setup_enables (enables);

  if (format & SPI_FMT_LSB){		// order: LSB
#if 1 
    return 0;		// error, not implemented
#else
    switch (format & SPI_FMR_HDR_MASK){
    case SPI_FMT_HDR_0:
      break;
    case SPI_FMT_HDR_1:
      write_byte_lsb (header_lo);
      break;
    case SPI_FMT_HDR_2:
      write_byte_lsb (header_lo);
      write_byte_lsb (header_hi);
      break;
    default:
      return 0;		// error
    }
    if (len != 0)
      write_bytes_lsb (buf, len);
#endif
  }

  else {		// order: MSB

    switch (format & SPI_FMT_HDR_MASK){
    case SPI_FMT_HDR_0:
      break;
    case SPI_FMT_HDR_1:
      write_byte_msb (header_lo);
      break;
    case SPI_FMT_HDR_2:
      write_byte_msb (header_hi);
      write_byte_msb (header_lo);
      break;
    default:
      return 0;		// error
    }
    if (len != 0)
      write_bytes_msb (buf, len);
  }

  disable_all ();
  return 1;		// success
}

// ----------------------------------------------------------------

static void
write_byte_msb (unsigned char v)
{
  v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
  bitS_OUT = v & 0x1;
  bitS_CLK = 1;
  bitS_CLK = 0;

  v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
  bitS_OUT = v & 0x1;
  bitS_CLK = 1;
  bitS_CLK = 0;

  v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
  bitS_OUT = v & 0x1;
  bitS_CLK = 1;
  bitS_CLK = 0;

  v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
  bitS_OUT = v & 0x1;
  bitS_CLK = 1;
  bitS_CLK = 0;

  v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
  bitS_OUT = v & 0x1;
  bitS_CLK = 1;
  bitS_CLK = 0;

  v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
  bitS_OUT = v & 0x1;
  bitS_CLK = 1;
  bitS_CLK = 0;

  v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
  bitS_OUT = v & 0x1;
  bitS_CLK = 1;
  bitS_CLK = 0;

  v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
  bitS_OUT = v & 0x1;
  bitS_CLK = 1;
  bitS_CLK = 0;
}

static void
write_bytes_msb (const xdata unsigned char *buf, unsigned char len)
{
  while (len-- != 0){
    write_byte_msb (*buf++);
  }
}

#if 0
/*
 * This is incorrectly compiled by SDCC 2.4.0
 */
static unsigned char
read_byte_msb (void)
{
  unsigned char v = 0;

  bitS_CLK = 1;
  v |= bitS_IN;
  bitS_CLK = 0;

  v = v << 1;
  bitS_CLK = 1;
  v |= bitS_IN;
  bitS_CLK = 0;

  v = v << 1;
  bitS_CLK = 1;
  v |= bitS_IN;
  bitS_CLK = 0;

  v = v << 1;
  bitS_CLK = 1;
  v |= bitS_IN;
  bitS_CLK = 0;

  v = v << 1;
  bitS_CLK = 1;
  v |= bitS_IN;
  bitS_CLK = 0;

  v = v << 1;
  bitS_CLK = 1;
  v |= bitS_IN;
  bitS_CLK = 0;

  v = v << 1;
  bitS_CLK = 1;
  v |= bitS_IN;
  bitS_CLK = 0;

  v = v << 1;
  bitS_CLK = 1;
  v |= bitS_IN;
  bitS_CLK = 0;

  return v;
}
#else
static unsigned char
read_byte_msb (void) _naked
{
  _asm
	clr	a

	setb	_bitS_CLK
        mov	c, _bitS_IN
	rlc	a
	clr	_bitS_CLK

	setb	_bitS_CLK
        mov	c, _bitS_IN
	rlc	a
	clr	_bitS_CLK

	setb	_bitS_CLK
        mov	c, _bitS_IN
	rlc	a
	clr	_bitS_CLK

	setb	_bitS_CLK
        mov	c, _bitS_IN
	rlc	a
	clr	_bitS_CLK

	setb	_bitS_CLK
        mov	c, _bitS_IN
	rlc	a
	clr	_bitS_CLK

	setb	_bitS_CLK
        mov	c, _bitS_IN
	rlc	a
	clr	_bitS_CLK

	setb	_bitS_CLK
        mov	c, _bitS_IN
	rlc	a
	clr	_bitS_CLK

	setb	_bitS_CLK
        mov	c, _bitS_IN
	rlc	a
	clr	_bitS_CLK

	mov	dpl,a
	ret
  _endasm;
}
#endif

static void
read_bytes_msb (xdata unsigned char *buf, unsigned char len)
{
  while (len-- != 0){
    *buf++ = read_byte_msb ();
  }
}

