//  V1.0 August 18, 2006
//
//  Copyright (C) 2006  P. Covington, N8VB
//
//  HPSDR - High Performance Software Defined Radio
//
//	GPIO SPI REGS for OZY V1
//
//	Based on USRP code, Copyright (C) 2003,2004 Matt Ettus
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

// -------------------------------------------------------------------------
// CORE LOGIC
// -------------------------------------------------------------------------
//
// 1.01: Bit Clock Counter
// 1.02: Instruction Decoder
// 1.03: Instruction Register
// 1.04: Serial Strobe Generator
//
// Implements 8xWIDTH registers

module SPI_REGS(FX2_CLK,SI,SO,SCK,CS,saddr,sdata,
				sstrobe,GPReg0,GPReg1,GPReg2,GPReg3,
				GPReg4,GPReg5,GPReg6,GPReg7);
				
 parameter WIDTH = 8;

 input 	FX2_CLK;				// FX2 Master Clock
 input  SCK;					// serial data clock 
 input 	SI;						// serial data input 
 input 	CS;						// chip select - active high
 inout wire SO;					// serial data output
 output reg [6:0] saddr;		// serial address 7 bits
 output reg [WIDTH-1:0] sdata;	// serial data
 output wire sstrobe;			// serial strobe
 input	[WIDTH-1:0] GPReg0;
 input	[WIDTH-1:0] GPReg1;
 input	[WIDTH-1:0] GPReg2;
 input	[WIDTH-1:0] GPReg3;
 input	[WIDTH-1:0] GPReg4;
 input	[WIDTH-1:0] GPReg5;
 input	[WIDTH-1:0] GPReg6;
 input	[WIDTH-1:0] GPReg7;

// -------------------------------------------------------------------------
// DECLARATIONS
// -------------------------------------------------------------------------

 reg				sRd;				// serial Read, asserted in read cycle
 reg [WIDTH-1:0] 	BitCounter;			// Bit Counter
  
 reg				CS_ph1;				// Chip select phase 1
 reg				CS_ph2;				// Chip select phase 2
 
// -------------------------------------------------------------------------
// CORE LOGIC
// -------------------------------------------------------------------------
//
//	
 assign SO = sRd ? sdata[WIDTH-1] : 1'bz; // drive SO when in read, otherwise hi-z
	
// -------------------------------------------------------------------------
// 1.01: Bit Clock Counter
// -------------------------------------------------------------------------

	always @(posedge SCK, negedge CS) begin
		if (~CS)
			BitCounter <= 8'd0;
		else if (BitCounter == (WIDTH+7))  // reset BitCounter when 16 bits xfered
			BitCounter <= 8'd0;
		else
			BitCounter <= BitCounter + 8'd1;
	end
	
// -------------------------------------------------------------------------
// 1.02: Instruction Decoder
// -------------------------------------------------------------------------
	
	// saddr 6 5 4 3 2 1 0
	//       x a a a a a a
	// x = 1 for read, 0 for write (in bit 7 of address)
	// a = address, up to 64 registers
	
	always @(posedge SCK, negedge CS) begin
		if (~CS)
			sRd <= 1'b0;
		else if ((BitCounter == 7)&&(saddr[5] == 1'b1)) // read set
			sRd <= 1'b1;
	end
	
// -------------------------------------------------------------------------
// 1.03: Address Decoder / Data Decoder
// -------------------------------------------------------------------------

	always @(posedge SCK) begin		
		if (sRd & (BitCounter == 8)) // xfer readback registers into sdata
			case (saddr[5:0])		 // for transmission
				6'd1: sdata <= GPReg0;
				6'd2: sdata <= GPReg1;
				6'd3: sdata <= GPReg2;
				6'd4: sdata <= GPReg3;
				6'd5: sdata <= GPReg4;
				6'd6: sdata <= GPReg5;
				6'd7: sdata <= GPReg6;
				6'd8: sdata <= GPReg7;
				default: sdata <= 0;
			endcase			
		else if (BitCounter >= 8)
			sdata <= {sdata[WIDTH-2:0], SI}; // read in data byte, shift data out
		else if (BitCounter < 8)
			saddr <= {saddr[5:0], SI}; // read in address byte		
	end
	
// -------------------------------------------------------------------------
// 1.04: Serial Strobe Generator
// -------------------------------------------------------------------------

	// generate a strobe to indicate serial data was transferred
	always @(posedge FX2_CLK) begin
		CS_ph1 <= CS;
		CS_ph2 <= CS_ph1;
	end
	
	assign sstrobe = CS_ph2 & ~CS_ph1;	
	
endmodule