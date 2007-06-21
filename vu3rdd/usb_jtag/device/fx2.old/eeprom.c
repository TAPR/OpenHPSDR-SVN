#ifndef _EEPROM_CONTENT
#define _EEPROM_CONTENT 1

//-----------------------------------------------------------------------------
// "EEPROM" Content
// If you want to simulate the EEPROM attached to the FT245BM in the
// device as Altera builds it, put something meaningful in here
//-----------------------------------------------------------------------------
// Copyright (C) 2005..2007 Kolja Waschk, ixo.de
//-----------------------------------------------------------------------------
// This code is part of usbjtag. usbjtag is free software; you can redistribute
// it and/or modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; either version 2 of the License,
// or (at your option) any later version. usbjtag is distributed in the hope
// that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
// warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.  You should have received a
// copy of the GNU General Public License along with this program in the file
// COPYING; if not, write to the Free Software Foundation, Inc., 51 Franklin
// St, Fifth Floor, Boston, MA  02110-1301  USA
//-----------------------------------------------------------------------------

// If you make changes to the EEPROM, you also need to update
// the checksum at the end. Try the opensourced libftdi/ftdi_eeprom!

const BYTE xdata PROM[128] = {
   0,0,
   0xC0,0x16, /* Vendor ID 0x16C0 */
   0xAD,0x06, /* Product ID 0x06AD */
   0x00,0x04, /* 0x400 indicates *BM device */
   0x80,      /* No remote wakeup, not self-powered */
   0x4B,      /* Need 2*75 mA max. */
   0x1C,0x00, /* Set USB version, use version string, enable suspend pulldowns */
   0x10,0x01, /* Support USB version 1.10 */
   0x94,0x0E, /* Offset of the "ixo.de" string (0x14) + 0x80, and length (14) */
   0xA2,0x18, /* Offset of the "USB-JTAG-IF" string (0x22) + 0x80, and length (24) */
   0xBA,0x12, /* Offset of the "00000000" serial string (0x3A) + 0x80, and length (18) */
   0x0E,0x03,'i',0,'x',0,'o',0,'.',0,'d',0,'e',0, /* 14 Bytes: "ixo.de" */
   0x18,0x03,'U',0,'S',0,'B',0,'-',0,'J',0,'T',0,'A',0,'G',0,'-',0,'I',0,'F',0, /* "USB-JTAG-IF" */
   0x12,0x03,'0',0,'0',0,'0',0,'0',0,'0',0,'0',0,'0',0,'0',0, /* 18 Bytes: "00000000" */
   1,2,3,1,0,0,0,0,
   0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
   0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
   0x08,0xA7 /* Checksum 0xA708 */
};

#endif
