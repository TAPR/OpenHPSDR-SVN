/*
 * HPSDR/OZY - High Performance Software Defined Radio, OZY Firmware
 *
 * Adapted from USRP firmware 07/10/2006 by Phil Covington N8VB
 *
 * Copyright 2006 Free Software Foundation, Inc.
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

#include "eeprom_io.h"
#include "i2c.h"
#include "delay.h"

// returns non-zero if successful, else 0
unsigned char
eeprom_read (unsigned char i2c_addr, unsigned char eeprom_offset,
	     xdata unsigned char *buf, unsigned char len)
{
  // We setup a random read by first doing a "zero byte write".
  // Writes carry an address.  Reads use an implicit address.
  // HPSDR uses a 24LC128 EEPROM so two bytes are used
  // to set the address 07/19/2006 PAC N8VB

  static xdata unsigned char cmd[2];
  cmd[0] = 0; // <-- address high byte, set to 0 since we are going to always
                    // read addresses less than 256 for now...
  cmd[1] = eeprom_offset; // <-- address low byte
  if (!i2c_write(i2c_addr, cmd, 2))
    return 0;

  return i2c_read(i2c_addr, buf, len);
}


#if 0

// returns non-zero if successful, else 0
unsigned char
eeprom_write (unsigned char i2c_addr, unsigned char eeprom_offset,
	      const xdata unsigned char *buf, unsigned char len)
{
  static xdata unsigned char cmd[2];
  unsigned char ok;

  while (len-- > 0){
    cmd[0] = eeprom_offset++;
    cmd[1] = *buf++;
    ok = i2c_write(i2c_addr, cmd, 2);
    mdelay(10);		// delay 10ms worst case write time
    if (!ok)
      return 0;
  }
  return 1;
}

#endif