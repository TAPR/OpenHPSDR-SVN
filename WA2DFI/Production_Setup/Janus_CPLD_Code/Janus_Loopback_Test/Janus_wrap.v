// 
// Copyright (c) Scott Cowling 2007 (scotty@tonks.com)
//
//  HPSDR - High Performance Software Defined Radio
//
//
//  Janus loop-back test code. 
// 
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// CPLD code for Janus board to test all pins to the DIN connector.
// Sets 27 pins as outputs (P1A) and 27 pins as inputs (P1C) and expects them to be
// externally looped back (row A to Row C).
//
// All TLV320 and AKM pins are unused.
//
// Enables the 12.288MHz clock to free run.
// The 8kHz from the divided VCXO output is fed to the tune pin so that 
// the VCXO LPF output is constant at 3.3v/2 which sets the VCXO to its nominal value.
//
// All unused pins on the CPLD have been set as tristate inputs in Quartus II
// 
// Built using Quartus II v6.1  build 201.


// Change log:
//
// 22 April 2007	SC	Rev 1.0 Initial design 
//
//

module Janus_wrap(
						// on-board clock pins
   	input  CLK_12MHZ,	// pin 4
   	output tune,		// pin 85

						// ADC control pins
   	output SCLK,		// pin 21
   	output LRCK,		// pin 20
   	input  FSYNC,		// pin 12
  	input  SDOUT,		// pin 14
   	output SMODE1,		// pin 19
   	output SMODE2,		// pin 18

   	output MCLK,		// pin ??
   	output DFS0,		// pin 7
   	output DFS1,		// pin 5

   	output HPF,			// pin 6

   	output ZCAL,		// pin 15
	input  CAL,			// pin 16
   	output nRST,		// pin 17


						// codec control pins
   	output CMCLK,		// pin 2
   	output CBCLK,		// pin 97
   	output CLRCIN,		// pin 95
   	output CLRCOUT,		// pin 91
   	output CDIN,		// pin 96
   	input  CDOUT,		// pin 92

   	input  MOSI,  		// pin 100	was output for spi interface 
   	input  SSCK,  		// pin 1	was output for spi interface 
   	output nCS,			// pin 98
   	output CMODE,		// pin 99

   	output IPWM,		// pin 90
   	output QPWM,		// pin 89 	Q data out

   	input  PTT,			// pin 3

						// LED outputs
   	output LED1,		// pin 86
   	output LED2,		// pin 87

						// unused test point output
   	output EXP4,		// pin 88

						// 27 pins of input
   	input  C2,			// pin 84	P1-C2 - nRST
	input  C3,			// pin 82	P1-C3 - CLK_48MHZ: 48MHz clock from FX2 for PWM 
	input  C4,			// pin 78	P1-C4 - IQOUT: I and Q data in I2S format 
	input  C5,			// pin 76	P1-C5 - 12.288MHz clock to Atlas bus
	input  C6,			// pin 74	P1-C6 - SCLK (BCLK)
	input  C7,			// pin 72	P1-C7 - LRCK
	input  C8,  		// pin 70	P1-C8 - CBCLK
	input  C9,			// pin 68	P1-C9 - CLRCIN/CLRCOUT
	input  C10,			// pin 66	P1-C10 - SDOUT
	input  C11,			// pin 62	P1-C11 - CDOUT
	input  C12,			// pin 58	P1-C12 - CDIN
	input  C13,			// pin 56	P1-C13 - DFS0
	input  C14,			// pin 54	P1-C14 - DFS1
	input  C15, 		// pin 52	P1-C15 - !PTT
	input  C16, 		// pin 50	P1-C16 - ref_in: 10MHz reference in from Atlas bus
	input  C17,		 	// pin 48	P1-C17 - CLK_MCLK: Master clock from Ozy
	input  C18,			// pin 44	P1-C18
	input  C19, 		// pin 42	P1-C19 
	input  C20,			// pin 40	P1-C20
	input  C21, 		// pin 38	P1-C21 
	input  C22,			// pin 36	P1-C22
	input  C23, 		// pin 34	P1-C23 - no test
	input  C24,			// pin 33	P1-C24 - no test
	input  C25, 		// pin 30	P1-C25 - no test
	input  C27,			// pin 29	P1-C27 - no test
	input  C29, 		// pin 28	P1-C29 - no test
	input  C31,			// pin 26	P1-C31

						// 27 pins of output
   	output  A2,			// pin 83	P1-A2
	output  A3,			// pin 81	P1-A3
	output  A4,			// pin 77	P1-A4 
	output  A5,			// pin 75	P1-A5
	output  A6,			// pin 73	P1-A6
	output  A7,			// pin 71	P1-A7
	output  A8,  		// pin 69	P1-A8
	output  A9,			// pin 67	P1-A9
	output  A10,		// pin 64	P1-A10
	output  A11,		// pin 61	P1-A11
	output  A12,		// pin 57	P1-A12
	output  A13,		// pin 55	P1-A13
	output  A14,		// pin 53	P1-A14
	output  A15, 		// pin 51	P1-A15
	output  A16, 		// pin 49	P1-A16
	output  A17,	 	// pin 47	P1-A17
	output  A18,		// pin 43	P1-A18
	output  A19, 		// pin 41	P1-A19 
	output  I2CSCK,		// pin 39	P1-A20
	output  I2CSDA, 	// pin 37	P1-A21 
	output  A22,		// pin 35	P1-A22
//	output  CTMS, 		// pin 22	P1-A23 
//	output  CTCK,		// pin 24	P1-A24
//	output  SDOBACK,	// JP12-2	P1-A25 - can jumper to CTDO pin 25
//	output  CTDO,		// pin 25	P1-A27
//	output  CTDI, 		// pin 23	P1-A29 
	output  A31			// pin 27	P1-A31
);
 
// stimulus outputs
//
/* wire	A2;
wire	A3;
wire	A4;
wire	A5;
wire	A6;
wire	A7;
wire	A8;
wire	A9;
wire	A10;
wire	A11;
wire	A12;
wire	A13;
wire	A14;
wire	A15;
wire	A16;
wire	A17;
wire	A18;
wire	A19;
wire	I2CSCK;
wire	I2CSDA; 
wire	A22;
wire	CTMS;
wire	CTCK;
wire	SDOBACK;
wire	CTDO;
wire	CTDI;
wire	A31;
*/



//----------------------------------------------------------
//
// 		Local signal definitions
//
//----------------------------------------------------------
//
reg		[21:0] stimulus;		// 22 pins of output stimulus
wire	[21:0] response;		// 22 pins of input response
reg		[21:0] loopback_result;	// 22 bits of result

reg	[7:0]	cmp_cnt;			// bit stabilization delay counter
reg	[24:0]	clock_count;		// big delay from 12MHz 
reg	[1:0]	leds;				// 2 bit LED register

//----------------------------------------------------------
//
// 		Free run 12.288MHz clock
//
//----------------------------------------------------------

// Uses Altera LPM_COUNTER Megafunction for the dividers

// divide 12.288MHz ADC  clock by 1024 to give 12kHz
//
// divide by 1024
//
reg [10:0] ctr;

always @ (posedge CLK_12MHZ)
	ctr	<= ctr + 11'h1;

// use 12kHz clock for the tune output
//always @ (posedge cout1)
assign	tune = ctr[9];


//----------------------------------------------------------
//
// 		LED rotation generator and clock counter
//
//----------------------------------------------------------
// 
//
// Two LEDs alternate unless an error is detected in the loopback result
// register. When error is detected, they hold all on.
//
// Also, if PTT is detected (active-low), hold LEDs on.
//
always @ (posedge CLK_12MHZ) begin 
	if ( leds == 0 ) begin 
		leds <= 2'b01; 
	end 	
	else begin 
		if ( clock_count == 1200000 ) begin
			clock_count <= 0; 
			if (|loopback_result[21:0] | !PTT) begin
				leds <= 2'b11;
			end
			else if ( leds == 2'b10 ) begin 
				leds <= 2'b01; 
			end 
			else begin 
				leds <= leds << 1; 
			end 
		end 		
		else begin 
			clock_count <= clock_count + 25'h1; 
		end 
	end 
end

assign	LED1 	= ~leds[0];		// LED outputs are active low
assign	LED2 	= ~leds[1];

//----------------------------------------------------------
//
// 		Loopback Test on 54 pins
//
//----------------------------------------------------------

// Assign outputs to stimulus vector
//
assign A2		= stimulus[21];
assign A3		= stimulus[20];
assign A4		= stimulus[19];
assign A5		= stimulus[18];
assign A6		= stimulus[17];
assign A7		= stimulus[16];
assign A8		= stimulus[15];
assign A9		= stimulus[14];
assign A10		= stimulus[13];
assign A11		= stimulus[12];
assign A12		= stimulus[11];
assign A13		= stimulus[10];
assign A14		= stimulus[9];
assign A15		= stimulus[8];
assign A16		= stimulus[7];
assign A17		= stimulus[6];
assign A18		= stimulus[5];
assign A19		= stimulus[4];
assign I2CSCK	= stimulus[3];
assign I2CSDA	= stimulus[2];
assign A22		= stimulus[1];
assign A31		= stimulus[0];


// Assign inputs to response vector
//
assign response[21:0] = {C2,
						C3,
						C4,
						C5,
						C6,
						C7,
						C8,
						C9,
						C10,
						C11,
						C12,
						C13,
						C14,
						C15,
						C16,
						C17,
						C18,
						C19,
						C20,
						C21,
						C22,
						C31
						};


//----------------------------------------------------------
//
// 		Generate test pattern on vectors
//
//----------------------------------------------------------
//
//
//
always @ (posedge CLK_12MHZ) begin
	if (clock_count == 25'h0) begin
		stimulus	<= 22'h155555;		// clear both vectors
		cmp_cnt		<= 8'h0;			// clear delay count
		loopback_result	<= 22'h0;		// clear errors
	end
	else begin 
		if (cmp_cnt	== 8'h10) begin		// time to update loopback_result
			loopback_result	<= ((stimulus ^ response) | loopback_result);
		end

		if (cmp_cnt == 8'h20) begin				// time to change stimulus
			cmp_cnt			<= 8'h0;			// and restart delay count
			loopback_result	<= loopback_result; // hold errors until count is zero		
			if (stimulus == 22'h200000) begin
				stimulus	<= 22'h155555;		// if rotation complete, start with initial pattern
			end
			else begin
			stimulus		<= stimulus << 1;	// shift left
			end
		end
		else begin
			cmp_cnt			<= cmp_cnt + 8'b1;	// increment count
		end
	end
end 




endmodule 
