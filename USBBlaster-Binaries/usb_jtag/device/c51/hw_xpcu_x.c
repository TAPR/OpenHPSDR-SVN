/*-----------------------------------------------------------------------------
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

#include "delay.h"
#include "syncdelay.h"
#include "hardware.h"

#include "isr.h"
#include "timer.h"
#include "delay.h"
#include "fx2regs.h"
#include "fx2utils.h"
#include "usb_common.h"
#include "usb_descriptors.h"
#include "usb_requests.h"
#include "syncdelay.h"
#include "eeprom.h"

#define HAVE_OE_LED 1
sbit at 0x80+1        OELED; // red LED on S3E Starter Kit (0x80+0 is green one)
#define SetOELED(x)   do{OELED=(x);}while(0)

void ProgIO_Poll(void)    {}
void ProgIO_Enable(void)  {}
void ProgiO_Disable(void) {}
void ProgiO_Deinit(void)  {}

static unsigned char curios;

const unsigned char wavedata[64] =
{
  /* Single Write:
     s0: BITS=D0     NEXT/SGLCRC DATA WAIT 4
     s1: BITS=       DATA WAIT 4
     s2: BITS=D1|D0  DATA WAIT 4
     s3: BITS=D1     DATA WAIT 3
     s4: BITS=D1     DATA DP IF(RDY0) THEN 5 ELSE 2
     s5: BITS=D1|D0  DATA WAIT 4
     s6: BITS=D1     DATA WAIT 3
     s7: BITS=D1     DATA FIN */

  4, 4, 4, 3, 0x2A, 4, 3, 7,
  6, 2, 2, 2, 3,    2, 2, 2,
  1, 0, 3, 2, 2,    3, 2, 2,
  0, 0, 0, 0, 0,    0, 0, 0x3F,

  /* Single Read:
     s0: BITS=D0     WAIT 4
     s1: BITS=       WAIT 4
     s2: BITS=D1|D0  WAIT 4
     s3: BITS=D1     WAIT 4
     s4: BITS=D1|D0  WAIT 3
     s5: BITS=D1|D0  DP IF(RDY0) THEN 6 ELSE 3
     s6: BITS=D1     DATA WAIT 4
     s7: BITS=D1     FIN */

  4, 4, 4, 4, 3, 0x33, 4, 7,
  0, 0, 0, 0, 0, 1,    2, 0,
  1, 0, 3, 2, 3, 3,    2, 2,
  0, 0, 0, 0, 0, 0,    0, 0x3F
};

void ProgIO_Init(void)
{
  unsigned char i;

  /* The following code depends on your actual circuit design.
     Make required changes _before_ you try the code! */

  // set the CPU clock to 48MHz, enable clock output to FPGA
  CPUCS = bmCLKOE | bmCLKSPD1;

  // Use internal 48 MHz, enable output, GPIF Master mode
  IFCONFIG = bmIFCLKSRC | bm3048MHZ | bmIFCLKOE | bmIFGPIF;

  PORTACFG = 0x00; OEA = 0xFB; IOA = 0x20;
  PORTCCFG = 0x00; OEC = 0xFF; IOC = 0x10;
  PORTECFG = 0x00; OEE = 0xFC; IOE = 0xC0;
 
  GPIFABORT    = 0xFF;

  GPIFREADYCFG = 0xA0;
  GPIFCTLCFG   = 0x00;
  GPIFIDLECS   = 0x00;
  GPIFIDLECTL  = 0x00;
  GPIFWFSELECT = 0x01;

  // Copy waveform data
  AUTOPTRSETUP = 0x07;
  APTR1H = MSB( &wavedata );
  APTR1L = LSB( &wavedata );
  AUTOPTRH2 = 0xE4;
  AUTOPTRL2 = 0x00;
  for ( i = 0; i < 64; i++ ) EXTAUTODAT2 = EXTAUTODAT1;

  SYNCDELAY;
  GPIFADRH      = 0x00;
  SYNCDELAY;
  GPIFADRL      = 0x00;

  FLOWSTATE     = 0x00;
  FLOWLOGIC     = 0x00;
  FLOWEQ0CTL    = 0x00;
  FLOWEQ1CTL    = 0x00;
  FLOWHOLDOFF   = 0x00;
  FLOWSTB       = 0x00;
  FLOWSTBEDGE   = 0x00;
  FLOWSTBHPERIOD = 0x00;

  curios = 0;
}

unsigned char GetTDO(unsigned char r)
{
  unsigned char x;

  IOC = 0x41;
  while(!(GPIFTRIG & 0x80)); x = XGPIFSGLDATLX;
  while(!(GPIFTRIG & 0x80)); x = XGPIFSGLDATLNOX;

  if(IOA & 0x20) IOA |= 0x40; else IOA &= ~0x40;

  return (x&1) ? r : 0;
}

#define SetPins(x) while(!(GPIFTRIG & 0x80)); XGPIFSGLDATLX = (x)

void ProgIO_Set_State(unsigned char d)
{
  /* Set state of output pins:
   *
   * d.0 => TCK (PE.6)
   * d.1 => TMS (PE.5)
   * d.4 => TDI (PE.4)
   * d.6 => LED / Output Enable
   */
 
  curios  = (d & bmBIT0) ? 0x40 : 0; // TCK
  curios |= (d & bmBIT1) ? 0x20 : 0; // TMS
  curios |= (d & bmBIT4) ? 0x10 : 0; // TDI

  IOC = 0x81;
  SetPins(curios);

  if(d & bmBIT6) IOA=(IOA&~3)|1; else IOA=(IOA&~3)|2;
}

unsigned char ProgIO_Set_Get_State(unsigned char d)
{
  ProgIO_Set_State(d);

  /* Read state of input pins:
   *
   * TDO => d.0
   * DATAOUT => d.1 (only #ifdef HAVE_AS_MODE)
   */

  return 2|GetTDO(0x01);
}

void ProgIO_ShiftOut(unsigned char c)
{
  unsigned char r,i;
  unsigned char locios = curios & ~0x50;

  IOC = 0x81;

  for(i=0,r=1;i<8;i++)
  {
    unsigned char t = locios;
    if(c & r) t |= 0x10;
    SetPins(t);
    SetPins(t|0x40);
    r <<= 1;
    SetPins(t);
  };

  curios = locios;
}


unsigned char ProgIO_ShiftInOut(unsigned char c)
{
  unsigned char r,i,n;
  unsigned char locios = curios & ~0x50;

  for(i=0,r=1,n=0;i<8;i++)
  {
    unsigned char t = locios;

    n |= GetTDO(r);

    IOC = 0x81;
    if(c & r) t |= 0x10;
    SetPins(t);
    SetPins(t|0x40);
    r <<= 1;
    SetPins(t);
  };

  curios = locios;
  return n;
}


