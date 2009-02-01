/*
 * HPSDR/OZY - High Performance Software Defined Radio, OZY Firmware
 *
 * Adapted from USRP firmware 07/10/2006 by Phil Covington N8VB
 * Further modified January 2009 for HPSDR by Bill Tracey (bill@ewjt.com) kd5tfd 
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

// Borrowed from USRP project, Phil C N8VB
/*
 * common code for HPSDR
 */

#include "../include/hpsdr_common.h"

void init_board (void);

void
init_hpsdr (void)
{
  CPUCS = bmCLKSPD1 | bmCLKOE;	// CPU runs @ 48 MHz  Changed 08/04/2006
                                                        // CLKOUT is Enabled!
                                                        // CLKOUT is the FPGA CLK1
                                                        // bmCLKSPD0 = 24 MHz
                                                        // bmCLKSPD1 = 48 MHz

  CKCON = 0;		// MOVX takes 2 cycles

  // IFCLK is generated internally and runs at 48 MHz; Slave FIFO mode - PAC 07/10/2006
  IFCONFIG = bmIFCLKSRC | bm3048MHZ | bmIFCLKOE | bmIFCLKPOL | bmIFFIFO;
  SYNCDELAY;

//Clear and reset all FIFOs see pg. 15-20 of TRM V2.1
  SYNCDELAY;
  FIFORESET = 0x80;
  SYNCDELAY;
  FIFORESET = 0x02;
  SYNCDELAY;
  FIFORESET = 0x04;
  SYNCDELAY;
  FIFORESET = 0x06;
  SYNCDELAY;
  FIFORESET = 0x08;
  SYNCDELAY;
  FIFORESET = 0x00;
  SYNCDELAY;

  // configure IO ports (B and D are used by FIFO)

  IOA = bmPORT_A_INITIAL;	// Port A initial state
  OEA = bmPORT_A_OUTPUTS;	// Port A direction register

  IOC = bmPORT_C_INITIAL;	// Port C initial state
  OEC = bmPORT_C_OUTPUTS;	// Port C direction register

  IOE = bmPORT_E_INITIAL;	// Port E initial state
  OEE = bmPORT_E_OUTPUTS;	// Port E direction register

  // configure end points

// we are just using the default values, yes this is not necessary...
  EP1OUTCFG = bmVALID | bmBULK;
  EP1INCFG = bmVALID | bmBULK;
  SYNCDELAY;
  EP2CFG = bmVALID | bmBULK | bmDOUBLEBUF;
  SYNCDELAY;
  EP4CFG = bmVALID | bmBULK | bmDOUBLEBUF | bmIN; 
  SYNCDELAY;
  EP6CFG = bmVALID | bmBULK | bmQUADBUF | bmIN;
  SYNCDELAY;
// EP8CFG = bmVALID | bmBULK | bmIN;
  SYNCDELAY;

  FIFOPINPOLAR=0x00; //default polarities: SLWR active low
                                    // see pg 15-22 TRM V2.1

  //EP2

  EP2FIFOCFG = bmWORDWIDE; //core needs to see 0 to 1 transistion of AUTOOUT
  SYNCDELAY;
  EP2FIFOCFG = bmAUTOOUT | bmWORDWIDE;
  SYNCDELAY;
  EP2AUTOINLENH = 0x02; //MSB
  SYNCDELAY;
  EP2AUTOINLENL = 0x00; //LSB
  SYNCDELAY;

  //EP4

  
  EP4FIFOCFG = bmWORDWIDE; //core needs to see 0 to 1 transistion of AUTOOUT
  SYNCDELAY;
  EP4FIFOCFG = bmAUTOIN | bmWORDWIDE;
  SYNCDELAY;
  EP4AUTOINLENH = 0x02; //MSB
  SYNCDELAY;
  EP4AUTOINLENL = 0x00; //LSB
  SYNCDELAY;
  
  //EP6

  EP6FIFOCFG = bmAUTOIN | bmWORDWIDE;
  SYNCDELAY;
  EP6AUTOINLENH = 0x02; //MSB
  SYNCDELAY;
  EP6AUTOINLENL = 0x00; //LSB
  SYNCDELAY;

//  //EP8
//
//  EP8FIFOCFG = bmAUTOIN | bmWORDWIDE;
//  SYNCDELAY;
//  EP8AUTOINLENH = 0x02; //MSB
//  SYNCDELAY;
//  EP8AUTOINLENL = 0x00; //LSB

  // Set up FIFO FLAGS A through D
  SYNCDELAY;
  // PINFLAGSAB = bmFLAGB3 | bmFLAGB0 | bmFLAGA3;
  PINFLAGSAB = bmFLAGB3 | bmFLAGB2 | bmFLAGB0 | bmFLAGA3;
  SYNCDELAY;
  PINFLAGSCD = bmFLAGD3 | bmFLAGD2 | bmFLAGD1
                        | bmFLAGD0 | bmFLAGC3 | bmFLAGC2
                        | bmFLAGC1;

  // enable dual autopointer feature
  AUTOPTRSETUP |= 0x01;

  EP0BCH = 0;
  SYNCDELAY;

  // arm EP1OUT so we can receive "out" packets (TRM pg 8-8)

  EP1OUTBC = 0;
  SYNCDELAY;

  // init serial

UART230 |= 1;  // hi speed baud rate generator 
SCON0 = 0x50;   
// TMOD |= 0x20;   // MI1 = 1 
// TH1 = 0xB2;
// TR1 = 1;        // TCON bit 6 (0x40) 
TI = 1;			

  init_board ();

}

