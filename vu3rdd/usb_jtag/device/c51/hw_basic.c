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

#include <fx2regs.h>
#include "hardware.h"
#include "delay.h"

//-----------------------------------------------------------------------------

#define HAVE_PS_MODE 1
#define HAVE_AS_MODE 1
#define HAVE_OE_LED  1

//-----------------------------------------------------------------------------

/* JTAG TCK, AS/PS DCLK */

sbit at 0xA2          TCK; /* Port C.0 */
#define bmTCKOE       bmBIT2
#define SetTCK(x)     do{TCK=(x);}while(0)

/* JTAG TDI, AS ASDI, PS DATA0 */

sbit at 0xA0          TDI; /* Port C.2 */
#define bmTDIOE       bmBIT0
#define SetTDI(x)     do{TDI=(x);}while(0)

/* JTAG TMS, AS/PS nCONFIG */

sbit at 0xA3          TMS; /* Port C.3 */
#define bmTMSOE       bmBIT3
#define SetTMS(x)     do{TMS=(x);}while(0)

/* JTAG TDO, AS/PS CONF_DONE */

sbit at 0xA1          TDO; /* Port C.1 */
#define bmTDOOE       bmBIT1
#define GetTDO(x)     TDO

//-----------------------------------------------------------------------------

#if defined(HAVE_PS_MODE) || defined(HAVE_AS_MODE)

  /* AS DATAOUT, PS nSTATUS */

  sbit at 0xA6        ASDO; /* Port C.6 */
  #define bmASDOOE    bmBIT6
  #define GetASDO(x)  ASDO

#else

  #define bmASDOOE    0
  #define GetASDO(x)  0

#endif

//-----------------------------------------------------------------------------

#if defined(HAVE_AS_MODE)

  /* AS Mode nCS */

  sbit at 0xA4        NCS; /* Port C.4 */
  #define bmNCSOE     bmBIT4
  #define SetNCS(x)   do{NCS=(x);}while(0)
  #define GetNCS(x)   NCS

  /* AS Mode nCE */

  sbit at 0xA5        NCE; /* Port C.5 */
  #define bmNCEOE     bmBIT5
  #define SetNCE(x)   do{NCE=(x);}while(0)

  unsigned char ProgIO_ShiftInOut_AS(unsigned char x);

#else

  #define bmNCSOE     0
  #define SetNCS(x)   while(0){}
  #define GetNCS(x)   1
  #define bmNCEOE     0
  #define SetNCE(x)   while(0){}

  #define ProgIO_ShiftInOut_AS(x) ProgIO_ShiftInOut(x)

#endif

//-----------------------------------------------------------------------------

#ifdef HAVE_OE_LED

  sbit at 0xA7        OELED; /* Port C.7 */
  #define bmOELEDOE   bmBIT7
  #define SetOELED(x) do{OELED=(x);}while(0)

#else

  #define bmOELEDOE   0
  #define SetOELED(x) while(0){}

#endif

//-----------------------------------------------------------------------------

#define bmPROGOUTOE (bmTCKOE|bmTDIOE|bmTMSOE|bmNCEOE|bmNCSOE|bmOELEDOE)
#define bmPROGINOE  (bmTDOOE|bmASDOOE)

//-----------------------------------------------------------------------------

void ProgIO_Poll(void)    {}
// These aren't called anywhere in usbjtag.c, but I plan to do so...
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

  // power on the onboard FPGA and all other VCCs, de-assert RESETN
  IOE = 0x1F;
  OEE = 0x1F;
  mdelay(500); // wait for supply to come up

  // TDO input, others output
  OEC=(OEC&~bmPROGINOE) | bmPROGOUTOE;
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
#ifdef HAVE_AS_MODE
  SetNCE((d & bmBIT2) ? 1 : 0);
  SetNCS((d & bmBIT3) ? 1 : 0);
#endif
  SetTDI((d & bmBIT4) ? 1 : 0);
#ifdef HAVE_OE_LED
  SetOELED((d & bmBIT5) ? 1 : 0);
#endif
}

unsigned char ProgIO_Set_Get_State(unsigned char d)
{
  ProgIO_Set_State(d);

  /* Read state of input pins:
   *
   * TDO => d.0
   * DATAOUT => d.1 (only #ifdef HAVE_AS_MODE)
   */

   return (GetASDO()<<1)|GetTDO();
}

//-----------------------------------------------------------------------------

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
 
  (void)c; /* argument passed in DPL */

  _asm
        MOV  A,DPL
        ;; Bit0
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        ;; Bit1
        RRC  A
        CLR  _TCK
        MOV  _TDI,C
        SETB _TCK
        ;; Bit2
        RRC  A
        CLR  _TCK
        MOV  _TDI,C
        SETB _TCK
        ;; Bit3
        RRC  A
        CLR  _TCK
        MOV  _TDI,C
        SETB _TCK
        ;; Bit4
        RRC  A
        CLR  _TCK
        MOV  _TDI,C
        SETB _TCK
        ;; Bit5
        RRC  A
        CLR  _TCK
        MOV  _TDI,C
        SETB _TCK
        ;; Bit6
        RRC  A
        CLR  _TCK
        MOV  _TDI,C
        SETB _TCK
        ;; Bit7
        RRC  A
        CLR  _TCK
        MOV  _TDI,C
        SETB _TCK
        NOP 
        CLR  _TCK
        ret
  _endasm;
}

/*
;; For ShiftInOut, the timing is a little more
;; critical because we have to read _TDO/shift/set _TDI
;; when _TCK is low. But 20% duty cycle at 48/4/5 MHz
;; is just like 50% at 6 Mhz, and that's still acceptable
*/

#if HAVE_AS_MODE

unsigned char ProgIO_ShiftInOut_JTAG(unsigned char c);
unsigned char ProgIO_ShiftInOut_AS(unsigned char c);

unsigned char ProgIO_ShiftInOut(unsigned char c)
{
  if(GetNCS(x)) return ProgIO_ShiftInOut_JTAG(c);
  return ProgIO_ShiftInOut_AS(c);
}

#else /* HAVE_AS_MODE */

#define ProgIO_ShiftInOut_JTAG(x) ProgIO_ShiftInOut(x)

#endif

unsigned char ProgIO_ShiftInOut_JTAG(unsigned char c)
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

   (void)c; /* argument passed in DPL */

  _asm
        MOV  A,DPL

        ;; Bit0
        MOV  C,_TDO
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        CLR  _TCK
        ;; Bit1
        MOV  C,_TDO
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        CLR  _TCK
        ;; Bit2
        MOV  C,_TDO
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        CLR  _TCK
        ;; Bit3
        MOV  C,_TDO
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        CLR  _TCK
        ;; Bit4
        MOV  C,_TDO
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        CLR  _TCK
        ;; Bit5
        MOV  C,_TDO
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        CLR  _TCK
        ;; Bit6
        MOV  C,_TDO
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        CLR  _TCK
        ;; Bit7
        MOV  C,_TDO
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        CLR  _TCK

        MOV  DPL,A
        ret
  _endasm;

  /* return value in DPL */

  return c;
}

#ifdef HAVE_AS_MODE

unsigned char ProgIO_ShiftInOut_AS(unsigned char c)
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

  (void)c; /* argument passed in DPL */

  _asm
        MOV  A,DPL

        ;; Bit0
        MOV  C,_ASDO
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        CLR  _TCK
        ;; Bit1
        MOV  C,_ASDO
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        CLR  _TCK
        ;; Bit2
        MOV  C,_ASDO
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        CLR  _TCK
        ;; Bit3
        MOV  C,_ASDO
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        CLR  _TCK
        ;; Bit4
        MOV  C,_ASDO
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        CLR  _TCK
        ;; Bit5
        MOV  C,_ASDO
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        CLR  _TCK
        ;; Bit6
        MOV  C,_ASDO
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        CLR  _TCK
        ;; Bit7
        MOV  C,_ASDO
        RRC  A
        MOV  _TDI,C
        SETB _TCK
        CLR  _TCK

        MOV  DPL,A
        ret
  _endasm;
  return c;
}

#endif /* HAVE_AS_MODE */


