//  V1.0 August 18, 2006
//
//  Copyright (C) 2006  P. Covington, N8VB & K. Weedman, KD7IRS
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
// Implements NxWIDTH registers

module SPI_REGS  (SI,SO,SCK,CS,saddr,sdata,sstrobe,GPIO);
				
parameter WIDTH = 8;      // width of each data port
parameter NUM_PORTS = 3;  // number of data ports needed
 
input  wire             SCK;      // serial data clock 
input  wire             SI;       // serial data input 
input  wire             CS;       // chip select - active high
inout  wire             SO;       // serial data output
 
output reg        [5:0] saddr;    // serial address = lower 6 bits
output reg  [WIDTH-1:0] sdata;    // serial data
output reg              sstrobe;  // serial strobe
input  wire [(NUM_PORTS*WIDTH)-1:0] GPIO;


// -------------------------------------------------------------------------
// DECLARATIONS
// -------------------------------------------------------------------------
reg              sRd;				// serial Read, asserted in read cycle
reg  [WIDTH-1:0] BitCounter; // Bit Counter
  
wire       [5:0] saddr_next; // serial address = lower 6 bits

 
// -------------------------------------------------------------------------
// CORE LOGIC
// -------------------------------------------------------------------------
//
//	
assign SO = sRd ? sdata[WIDTH-1] : 1'bz; // drive SO when in read, otherwise hi-z
	
// -------------------------------------------------------------------------
// 1.01: Bit Clock Counter
// -------------------------------------------------------------------------

always @(posedge SCK)
begin
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
	
	// saddr 7 6 5 4 3 2 1 0
	//         x a a a a a a
	// x = 1 for read, 0 for write (in bit 7 of address)
	// a = address, up to 64 registers
	
	always @(posedge SCK)
	begin
		if (~CS)
			sRd <= 1'b0;
		else if ((BitCounter == 7)&&(saddr[5] == 1'b1)) // read set
			sRd <= 1'b1;
	end
	
// -------------------------------------------------------------------------
// Address Decoder / Data Decoder
// -------------------------------------------------------------------------
  assign saddr_next = {saddr[4:0], SI};
  
	always @(posedge SCK)
	begin		
		if (BitCounter < 8)
			saddr   <= saddr_next; // read in address byte.
	end
	
	always @(posedge SCK)
	begin		
		if (BitCounter == 7)    // load gpio or direction data for transmission
		  sdata <= GPIO[(saddr_next-1)*WIDTH +: WIDTH];
		else if (BitCounter >= 8)
			sdata <= {sdata[WIDTH-2:0], SI}; // read in data byte, shift data out
	end
	
// -------------------------------------------------------------------------
// Serial Strobe Generator
// -------------------------------------------------------------------------
	
  always @(posedge SCK)
    sstrobe <= CS & !sRd & (BitCounter == (WIDTH+7));
endmodule