//-----------------------------------------------------------------------------
// Code that turns a Cypress FX2 USB Controller into an USB JTAG adapter
//-----------------------------------------------------------------------------
// Copyright (C) 2005,2006 Kolja Waschk, ixo.de
//-----------------------------------------------------------------------------
// For hardware configuration, please take a look at the I/O definitions just
// below and in the TD_Init() code before you actually connect your FX2 to
// something. There are some board-specific initializations. Then compile with
// the Keil compiler & load the resulting binary into Cypress FX2 using the
// EZUSB Control Panel! Please read the README text file that should have
// come together with this source code.
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
// This file has been derived from bulksrc.c and vend_Ax.c, both taken
// from the /USB/Examples/Fx2/bulksrc/bulksrc.c delivered with CY3681
// development kit from Cypress Semiconductor. They both are
// Copyright (c) 2000 Cypress Semiconductor. All rights reserved.
//-----------------------------------------------------------------------------
#pragma NOIV               // Do not generate interrupt vectors

#include "fx2.h"
#include "fx2regs.h"
#include "fx2sdly.h"            // SYNCDELAY macro

// These are in shift.a51
extern void ShiftOut(BYTE c);
extern BYTE ShiftInOut(BYTE c);

//-----------------------------------------------------------------------------
// If you're using other pins for JTAG, please also change shift.a51!
// And make sure the I/O initialization in TD_Init is correct for your setup.
sbit TDI = 0xA0+0;
sbit TDO = 0xA0+1;
sbit TCK = 0xA0+2;
sbit TMS = 0xA0+3;
#define bmTDIOE bmBIT0
#define bmTDOOE bmBIT1
#define bmTCKOE bmBIT2
#define bmTMSOE bmBIT3

//-----------------------------------------------------------------------------
// Define USE_MOD256_OUTBUFFER:
// Saves about 256 bytes in code size, improves speed a little.
// A further optimization could be not to use an extra output buffer at 
// all, but to write directly into EP1INBUF. Not implemented yet. When 
// downloading large amounts of data _to_ the target, there is no output
// and thus the output buffer isn't used at all and doesn't slow down things.

#define USE_MOD256_OUTBUFFER 1

//-----------------------------------------------------------------------------
// Global data

static BOOL Running;
static BOOL WriteOnly;
static BYTE ClockBytes;
static WORD Pending;
#ifdef USE_MOD256_OUTBUFFER
  static BYTE FirstDataInOutBuffer;
  static BYTE FirstFreeInOutBuffer;
#else
  static WORD FirstDataInOutBuffer;
  static WORD FirstFreeInOutBuffer;
#endif

#ifdef USE_MOD256_OUTBUFFER
  /* Size of output buffer must be exactly 256 */
  #define OUTBUFFER_LEN 0x100
  /* Output buffer must begin at some address with lower 8 bits all zero */
#else
  #define OUTBUFFER_LEN 0x200
#endif
static BYTE xdata OutBuffer[OUTBUFFER_LEN] _at_ 0xE000;

extern BOOL GotSUD;             // Received setup data flag
extern BOOL Sleep;
extern BOOL Rwuen;
extern BOOL Selfpwr;

BYTE Configuration;             // Current configuration
BYTE AlternateSetting;          // Alternate settings

//-----------------------------------------------------------------------------
// "EEPROM" Content
// If you want to simulate the EEPROM attached to the FT245BM in the
// device as Altera builds it, put something meaningful in here
//-----------------------------------------------------------------------------

#include "eeprom.c"

//-----------------------------------------------------------------------------
// Task Dispatcher hooks
//   The following hooks are called by the task dispatcher.
//-----------------------------------------------------------------------------

void TD_Init(void)              // Called once at startup
{
   WORD tmp;

   Running = FALSE;
   ClockBytes = 0;
   Pending = 0;
   WriteOnly = TRUE;
   FirstDataInOutBuffer = 0;
   FirstFreeInOutBuffer = 0;

   /* The following code depends on your actual circuit design.
      Make required changes _before_ you try the code! */

   // set the CPU clock to 48MHz, enable clock output to FPGA
   CPUCS = bmCLKOE | bmCLKSPD1;

   // Use internal 48 MHz, enable output, use "Port" mode for all pins
   IFCONFIG = bmIFCLKSRC | bm3048MHZ | bmIFCLKOE;

   // If you're using other pins for JTAG, please also change shift.a51!
   // activate JTAG outputs on Port C
   OEC = bmTDIOE | bmTCKOE | bmTMSOE;

   // power on the FPGA and all other VCCs, de-assert RESETN
   IOE = 0x1F;
   OEE = 0x1F;
   EZUSB_Delay(500); // wait for supply to come up
 
   // The remainder of the code however should be left unchanged...

   // Make Timer2 reload at 100 Hz to trigger Keepalive packets

   tmp = 65536 - ( 48000000 / 12 / 100 );
   RCAP2H = tmp >> 8;
   RCAP2L = tmp & 0xFF;
   CKCON = 0; // Default Clock
   T2CON = 0x04; // Auto-reload mode using internal clock, no baud clock.

   // Enable Autopointer

   EXTACC = 1;  // Enable
   APTR1FZ = 1; // Don't freeze
   APTR2FZ = 1; // Don't freeze

   // we are just using the default values, yes this is not necessary...
   EP1OUTCFG = 0xA0;
   EP1INCFG = 0xA0;
   SYNCDELAY;                    // see TRM section 15.14
   EP2CFG = 0xA2;
   SYNCDELAY;                    // 
   EP4CFG = 0xA0;
   SYNCDELAY;                    // 
   EP6CFG = 0xE2;
   SYNCDELAY;                    // 
   EP8CFG = 0xE0;

   // out endpoints do not come up armed
   
   // since the defaults are double buffered we must write dummy byte counts twice
   SYNCDELAY;                    // 
   EP2BCL = 0x80;                // arm EP2OUT by writing byte count w/skip.
   SYNCDELAY;                    // 
   EP4BCL = 0x80;    
   SYNCDELAY;                    // 
   EP2BCL = 0x80;                // arm EP4OUT by writing byte count w/skip.
   SYNCDELAY;                    // 
   EP4BCL = 0x80;    

}

void OutputByte(BYTE d)
{
#ifdef USE_MOD256_OUTBUFFER
   OutBuffer[FirstFreeInOutBuffer] = d;
   FirstFreeInOutBuffer = ( FirstFreeInOutBuffer + 1 ) & 0xFF;
#else
   OutBuffer[FirstFreeInOutBuffer++] = d;
   if(FirstFreeInOutBuffer >= OUTBUFFER_LEN) FirstFreeInOutBuffer = 0;
#endif
   Pending++;
}

//-----------------------------------------------------------------------------
// TD_Poll does most of the work. It now happens to behave just like the 
// combination of FT245BM and Altera-programmed EPM7064 CPLD in Altera's
// USB-Blaster. The CPLD knows two major modes: Bit banging mode and Byte
// shift mode. It starts in Bit banging mode. While bytes are received
// from the host on EP2OUT, each byte B of them is processed as follows:
//
// Please note: nCE, nCS, LED pins and DATAOUT actually aren't supported here.
// Support for these would be required for AS/PS mode and isn't too complicated,
// but I haven't had the time yet.
//
// Bit banging mode:
// 
//   1. Remember bit 6 (0x40) in B as the "Read bit".
//
//   2. If bit 7 (0x40) is set, switch to Byte shift mode for the coming
//      X bytes ( X := B & 0x3F ), and don't do anything else now.
//
//    3. Otherwise, set the JTAG signals as follows:
//        TCK/DCLK high if bit 0 was set (0x01), otherwise low
//        TMS/nCONFIG high if bit 1 was set (0x02), otherwise low
//        nCE high if bit 2 was set (0x04), otherwise low
//        nCS high if bit 3 was set (0x08), otherwise low
//        TDI/ASDI/DATA0 high if bit 4 was set (0x10), otherwise low
//        Output Enable/LED active if bit 5 was set (0x20), otherwise low
//
//    4. If "Read bit" (0x40) was set, record the state of TDO(CONF_DONE) and
//        DATAOUT(nSTATUS) pins and put it as a byte ((DATAOUT<<1)|TDO) in the
//        output FIFO _to_ the host (the code here reads TDO only and assumes
//        DATAOUT=1)
//
// Byte shift mode:
//
//   1. Load shift register with byte from host
//
//   2. Do 8 times (i.e. for each bit of the byte; implemented in shift.a51)
//      2a) if nCS=1, set carry bit from TDO, else set carry bit from DATAOUT
//      2b) Rotate shift register through carry bit
//      2c) TDI := Carry bit
//      2d) Raise TCK, then lower TCK.
//
//   3. If "Read bit" was set when switching into byte shift mode,
//      record the shift register content and put it into the FIFO
//      _to_ the host.
//
// Some more (minor) things to consider to emulate the FT245BM:
//
//   a) The FT245BM seems to transmit just packets of no more than 64 bytes
//      (which perfectly matches the USB spec). Each packet starts with
//      two non-data bytes (I use 0x31,0x60 here). A USB sniffer on Windows
//      might show a number of packets to you as if it was a large transfer
//      because of the way that Windows understands it: it _is_ a large
//      transfer until terminated with an USB packet smaller than 64 byte.
//
//   b) The Windows driver expects to get some data packets (with at least
//      the two leading bytes 0x31,0x60) immediately after "resetting" the
//      FT chip and then in regular intervals. Otherwise a blue screen may
//      appear... In the code below, I make sure that every 10ms there is
//      some packet.
//
//   c) Vendor specific commands to configure the FT245 are mostly ignored
//      in my code. Only those for reading the EEPROM are processed. See
//      DR_GetStatus and DR_VendorCmd below for my implementation.
//
//   All other TD_ and DR_ functions remain as provided with CY3681.
//
//-----------------------------------------------------------------------------

void TD_Poll(void)              // Called repeatedly while the device is idle
{
   if(!Running) return;
   
   if(!(EP1INCS & bmEPBUSY))
   {
      if(Pending > 0)
      {
         BYTE o, n;

         AUTOPTRH2 = MSB( EP1INBUF );
         AUTOPTRL2 = LSB( EP1INBUF );
       
         XAUTODAT2 = 0x31;
         XAUTODAT2 = 0x60;
       
         if(Pending > 0x3E) { n = 0x3E; Pending -= n; } 
                     else { n = Pending; Pending = 0; };
       
         o = n;

#ifdef USE_MOD256_OUTBUFFER
         AUTOPTR1H = MSB( OutBuffer );
         AUTOPTR1L = FirstDataInOutBuffer;
         while(n--)
         {
            XAUTODAT2 = XAUTODAT1;
            AUTOPTR1H = MSB( OutBuffer ); // Stay within 256-Byte-Buffer
         };
         FirstDataInOutBuffer = AUTOPTR1L;
#else
         AUTOPTR1H = MSB( &(OutBuffer[FirstDataInOutBuffer]) );
         AUTOPTR1L = LSB( &(OutBuffer[FirstDataInOutBuffer]) );
         while(n--)
         {
            XAUTODAT2 = XAUTODAT1;

            if(++FirstDataInOutBuffer >= OUTBUFFER_LEN)
            {
               FirstDataInOutBuffer = 0;
               AUTOPTR1H = MSB( OutBuffer );
               AUTOPTR1L = LSB( OutBuffer );
            };
         };
#endif
         SYNCDELAY;
         EP1INBC = 2 + o;
         TF2 = 1; // Make sure there will be a short transfer soon
      }
      else if(TF2)
      {
         EP1INBUF[0] = 0x31;
         EP1INBUF[1] = 0x60;
         SYNCDELAY;
         EP1INBC = 2;
         TF2 = 0;
      };
   };

   if(!(EP2468STAT & bmEP2EMPTY) && (Pending < OUTBUFFER_LEN-0x3F))
   {
      BYTE i, n = EP2BCL;

      AUTOPTR1H = MSB( EP2FIFOBUF );
      AUTOPTR1L = LSB( EP2FIFOBUF );

      for(i=0;i<n;)
      {
         if(ClockBytes > 0)
         {
            BYTE m;

            m = n-i;
            if(ClockBytes < m) m = ClockBytes;
            ClockBytes -= m;
            i += m;
         
            if(WriteOnly) /* Shift out 8 bits from d */
            {
               while(m--) ShiftOut(XAUTODAT1);
            }
            else /* Shift in 8 bits at the other end  */
            {
               while(m--) OutputByte(ShiftInOut(XAUTODAT1));
            }
        }
        else
        {
            BYTE d = XAUTODAT1;
            WriteOnly = (d & bmBIT6) ? FALSE : TRUE;

            if(d & bmBIT7)
            {
               /* Prepare byte transfer, do nothing else yet */

               ClockBytes = d & 0x3F;
            }
            else
            {
               /* Set state of output pins */

               TCK = (d & bmBIT0) ? 1 : 0;
               TMS = (d & bmBIT1) ? 1 : 0;
               TDI = (d & bmBIT4) ? 1 : 0;

               /* Optionally read state of input pins and put it in output buffer */

               if(!WriteOnly) OutputByte(2|TDO);
            };
            i++;
         };
      };

      SYNCDELAY;
      EP2BCL = 0x80; // Re-arm endpoint 2
   };
}

BOOL TD_Suspend(void)          // Called before the device goes into suspend mode
{
   return(TRUE);
}

BOOL TD_Resume(void)          // Called after the device resumes
{
   return(TRUE);
}

//-----------------------------------------------------------------------------
// Device Request hooks
//   The following hooks are called by the end point 0 device request parser.
//-----------------------------------------------------------------------------

BOOL DR_GetDescriptor(void)
{
   return(TRUE);
}

BOOL DR_SetConfiguration(void)   // Called when a Set Configuration command is received
{
   Configuration = SETUPDAT[2];
   return(TRUE);            // Handled by user code
}

BOOL DR_GetConfiguration(void)   // Called when a Get Configuration command is received
{
   EP0BUF[0] = Configuration;
   EP0BCH = 0;
   EP0BCL = 1;
   return(TRUE);            // Handled by user code
}

BOOL DR_SetInterface(void)       // Called when a Set Interface command is received
{
   AlternateSetting = SETUPDAT[2];
   return(TRUE);            // Handled by user code
}

BOOL DR_GetInterface(void)       // Called when a Set Interface command is received
{
   EP0BUF[0] = AlternateSetting;
   EP0BCH = 0;
   EP0BCL = 1;
   return(TRUE);            // Handled by user code
}

BOOL DR_GetStatus(void)
{
   if(SETUPDAT[0]==0x40)
   {
      Running = 1;
      return FALSE;
   };

   return(TRUE);
}

BOOL DR_ClearFeature(void)
{
   return(TRUE);
}

BOOL DR_SetFeature(void)
{
   return(TRUE);
}

BOOL DR_VendorCmnd(void)
{
   if(SETUPDAT[1] == 0x90) // READ EEPROM
   {
      BYTE addr = (SETUPDAT[4]<<1) & 0x7F;
      EP0BUF[0] = PROM[addr];
      EP0BUF[1] = PROM[addr+1];
   }
   else
   {
      EP0BUF[0] = 0x36;
      EP0BUF[1] = 0x83;
   }

   EP0BCH = 0;
   EP0BCL = 2; // Arm endpoint with # bytes to transfer
   EP0CS |= bmHSNAK; // Acknowledge handshake phase of device request

   return(FALSE); // no error; command handled OK
}

//-----------------------------------------------------------------------------
// USB Interrupt Handlers
//   The following functions are called by the USB interrupt jump table.
//-----------------------------------------------------------------------------

// Setup Data Available Interrupt Handler
void ISR_Sudav(void) interrupt 0
{
   GotSUD = TRUE;            // Set flag
   EZUSB_IRQ_CLEAR();
   USBIRQ = bmSUDAV;         // Clear SUDAV IRQ
}

// Setup Token Interrupt Handler
void ISR_Sutok(void) interrupt 0
{
   EZUSB_IRQ_CLEAR();
   USBIRQ = bmSUTOK;         // Clear SUTOK IRQ
}

void ISR_Sof(void) interrupt 0
{
   EZUSB_IRQ_CLEAR();
   USBIRQ = bmSOF;            // Clear SOF IRQ
}

void ISR_Ures(void) interrupt 0
{
   if (EZUSB_HIGHSPEED())
   {
      pConfigDscr = pHighSpeedConfigDscr;
      pOtherConfigDscr = pFullSpeedConfigDscr;
   }
   else
   {
      pConfigDscr = pFullSpeedConfigDscr;
      pOtherConfigDscr = pHighSpeedConfigDscr;
   }
   
   EZUSB_IRQ_CLEAR();
   USBIRQ = bmURES;         // Clear URES IRQ
}

void ISR_Susp(void) interrupt 0
{
   Sleep = TRUE;
   EZUSB_IRQ_CLEAR();
   USBIRQ = bmSUSP;
}

void ISR_Highspeed(void) interrupt 0
{
   if (EZUSB_HIGHSPEED())
   {
      pConfigDscr = pHighSpeedConfigDscr;
      pOtherConfigDscr = pFullSpeedConfigDscr;
   }
   else
   {
      pConfigDscr = pFullSpeedConfigDscr;
      pOtherConfigDscr = pHighSpeedConfigDscr;
   }

   EZUSB_IRQ_CLEAR();
   USBIRQ = bmHSGRANT;
}
void ISR_Ep0ack(void) interrupt 0
{
}
void ISR_Stub(void) interrupt 0
{
}
void ISR_Ep0in(void) interrupt 0
{
}
void ISR_Ep0out(void) interrupt 0
{
}
void ISR_Ep1in(void) interrupt 0
{
}
void ISR_Ep1out(void) interrupt 0
{
}
void ISR_Ep2inout(void) interrupt 0
{
}
void ISR_Ep4inout(void) interrupt 0
{
}
void ISR_Ep6inout(void) interrupt 0
{
}
void ISR_Ep8inout(void) interrupt 0
{
}
void ISR_Ibn(void) interrupt 0
{
}
void ISR_Ep0pingnak(void) interrupt 0
{
}
void ISR_Ep1pingnak(void) interrupt 0
{
}
void ISR_Ep2pingnak(void) interrupt 0
{
}
void ISR_Ep4pingnak(void) interrupt 0
{
}
void ISR_Ep6pingnak(void) interrupt 0
{
}
void ISR_Ep8pingnak(void) interrupt 0
{
}
void ISR_Errorlimit(void) interrupt 0
{
}
void ISR_Ep2piderror(void) interrupt 0
{
}
void ISR_Ep4piderror(void) interrupt 0
{
}
void ISR_Ep6piderror(void) interrupt 0
{
}
void ISR_Ep8piderror(void) interrupt 0
{
}
void ISR_Ep2pflag(void) interrupt 0
{
}
void ISR_Ep4pflag(void) interrupt 0
{
}
void ISR_Ep6pflag(void) interrupt 0
{
}
void ISR_Ep8pflag(void) interrupt 0
{
}
void ISR_Ep2eflag(void) interrupt 0
{
}
void ISR_Ep4eflag(void) interrupt 0
{
}
void ISR_Ep6eflag(void) interrupt 0
{
}
void ISR_Ep8eflag(void) interrupt 0
{
}
void ISR_Ep2fflag(void) interrupt 0
{
}
void ISR_Ep4fflag(void) interrupt 0
{
}
void ISR_Ep6fflag(void) interrupt 0
{
}
void ISR_Ep8fflag(void) interrupt 0
{
}
void ISR_GpifComplete(void) interrupt 0
{
}
void ISR_GpifWaveform(void) interrupt 0
{
}
