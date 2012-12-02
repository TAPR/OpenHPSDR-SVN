/*
 * HPSDR/OZY - High Performance Software Defined Radio, OZY Firmware
 * 
 * SDR 1000 Control Support 
 *
 *
 * Copyright (C) 2007 Bill Tracey, KD5TFD (bill@ewjt.com)
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

#ifndef SDR1K_CTL_INCLUDED 
#define SDR1K_CTL_INCLUDED
// returns 1 on success, 0 on failure 
// 
unsigned char SDR1k_ControlOut(unsigned char wValueH, unsigned char wValueL,
							  unsigned char wIndexH, unsigned char wIndexL, 
							  const xdata unsigned char *bufp, unsigned char buflen);
							  
							  
unsigned char SDR1k_ControlIn(unsigned char wValueH, unsigned char wValueL,
							  unsigned char wIndexH, unsigned char wIndexL, 
							  const xdata unsigned char *bufp, unsigned char buflen);
  
#endif 
 