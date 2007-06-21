/*-----------------------------------------------------------------------------
 *
 * Hardware-dependent code for usb_jtag
 *-----------------------------------------------------------------------------
 * Copyright (C) 2007 Kolja Waschk, ixo.de
 *-----------------------------------------------------------------------------
 * This code is part of usbjtag. usbjtag is free software; you can redistribute
 * it and/or modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2 of the License,
 * or (at your option) any later version. usbjtag is distributed in the hope
 * that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.  You should have received a
 * copy of the GNU General Public License along with this program in the file
 * COPYING; if not, write to the Free Software Foundation, Inc., 51 Franklin
 * St, Fifth Floor, Boston, MA  02110-1301  USA
 *-----------------------------------------------------------------------------
 */

#include "hardware.h"
#include "fx2regs.h"
#include "syncdelay.h"

//---------------------------------------------------------------------------

#define SetTCK(x)     do{if(x) IOE|=0x08; else IOE&=~0x08; }while(0)
#define SetTMS(x)     do{if(x) IOE|=0x10; else IOE&=~0x10; }while(0)
#define SetTDI(x)     do{if(x) IOE|=0x40; else IOE&=~0x40; }while(0)
#define GetTDO()      ((IOE>>5)&1)

/* XPCU has neither AS nor PS mode pins */

#define HAVE_OE_LED 1
/* +0=green led, +1=red led */
sbit at 0x80+1        OELED;
#define SetOELED(x)   do{OELED=(x);}while(0)

//-----------------------------------------------------------------------------

void ProgIO_Poll(void)    {}
void ProgIO_Enable(void)  {}
void ProgIO_Disable(void) {}
void ProgIO_Deinit(void)  {}

void ProgIO_Init(void)
{
  /* The following code depends on your actual circuit design.
     Make required changes _before_ you try the code! */

  // set the CPU clock to 48MHz, enable clock output to FPGA
  CPUCS = bmCLKOE | bmCLKSPD1;

  // Use internal 48 MHz, enable output, use "Port" mode for all pins
  IFCONFIG = bmIFCLKSRC | bm3048MHZ | bmIFCLKOE;

  GPIFABORT = 0xFF;

  PORTACFG = 0x00; OEA = 0x03; IOA=0x01;
  PORTCCFG = 0x00; OEC = 0x00; IOC=0x00;
  PORTECFG = 0x00; OEE = 0x58; IOE=0x00;
}

void ProgIO_Set_State(unsigned char d)
{
  /* Set state of output pins:
   *
   * d.0 => TCK
   * d.1 => TMS
   * d.2 => nCE (only #ifdef HAVE_AS_MODE)
   * d.3 => nCS (only #ifdef HAVE_AS_MODE)
   * d.4 => TDI
   * d.6 => LED / Output Enable
   */

  SetTCK((d & bmBIT0) ? 1 : 0);
  SetTMS((d & bmBIT1) ? 1 : 0);
  SetTDI((d & bmBIT4) ? 1 : 0);
#ifdef HAVE_OE_LED
  SetOELED((d & bmBIT5) ? 1 : 0);
#endif
}

unsigned char ProgIO_Set_Get_State(unsigned char d)
{
  /* Read state of input pins:
   *
   * TDO => d.0
   * DATAOUT => d.1 (only #ifdef HAVE_AS_MODE)
   */

  ProgIO_Set_State(d);
  return 2|GetTDO();
}

void ProgIO_ShiftOut(unsigned char c)
{
  /* Shift out byte C:
   *
   * 8x {
   *   Output least significant bit on TDI
   *   Raise TCK
   *   Shift c right
   *   Lower TCK
   * }
   */

  unsigned char lc=c;

  if(lc&1) IOE|=0x40; else IOE&=~0x40; IOE|=0x08; lc>>=1; IOE&=~0x08;
  if(lc&1) IOE|=0x40; else IOE&=~0x40; IOE|=0x08; lc>>=1; IOE&=~0x08;
  if(lc&1) IOE|=0x40; else IOE&=~0x40; IOE|=0x08; lc>>=1; IOE&=~0x08;
  if(lc&1) IOE|=0x40; else IOE&=~0x40; IOE|=0x08; lc>>=1; IOE&=~0x08;

  if(lc&1) IOE|=0x40; else IOE&=~0x40; IOE|=0x08; lc>>=1; IOE&=~0x08;
  if(lc&1) IOE|=0x40; else IOE&=~0x40; IOE|=0x08; lc>>=1; IOE&=~0x08;
  if(lc&1) IOE|=0x40; else IOE&=~0x40; IOE|=0x08; lc>>=1; IOE&=~0x08;
  if(lc&1) IOE|=0x40; else IOE&=~0x40; IOE|=0x08; lc>>=1; IOE&=~0x08;
}

unsigned char ProgIO_ShiftInOut(unsigned char c)
{
  /* Shift out byte C, shift in from TDO:
   *
   * 8x {
   *   Read carry from TDO
   *   Output least significant bit on TDI
   *   Raise TCK
   *   Shift c right, append carry (TDO) at left
   *   Lower TCK
   * }
   * Return c.
   */

  unsigned char carry;
  unsigned char lc=c;

  carry = (IOE&0x20)<<2; if(lc&1) IOE|=0x40; else IOE&=~0x40; IOE|=0x08; lc=carry|(lc>>1); IOE&=~0x08;
  carry = (IOE&0x20)<<2; if(lc&1) IOE|=0x40; else IOE&=~0x40; IOE|=0x08; lc=carry|(lc>>1); IOE&=~0x08;
  carry = (IOE&0x20)<<2; if(lc&1) IOE|=0x40; else IOE&=~0x40; IOE|=0x08; lc=carry|(lc>>1); IOE&=~0x08;
  carry = (IOE&0x20)<<2; if(lc&1) IOE|=0x40; else IOE&=~0x40; IOE|=0x08; lc=carry|(lc>>1); IOE&=~0x08;

  carry = (IOE&0x20)<<2; if(lc&1) IOE|=0x40; else IOE&=~0x40; IOE|=0x08; lc=carry|(lc>>1); IOE&=~0x08;
  carry = (IOE&0x20)<<2; if(lc&1) IOE|=0x40; else IOE&=~0x40; IOE|=0x08; lc=carry|(lc>>1); IOE&=~0x08;
  carry = (IOE&0x20)<<2; if(lc&1) IOE|=0x40; else IOE&=~0x40; IOE|=0x08; lc=carry|(lc>>1); IOE&=~0x08;
  carry = (IOE&0x20)<<2; if(lc&1) IOE|=0x40; else IOE&=~0x40; IOE|=0x08; lc=carry|(lc>>1); IOE&=~0x08;

  return lc;
}


