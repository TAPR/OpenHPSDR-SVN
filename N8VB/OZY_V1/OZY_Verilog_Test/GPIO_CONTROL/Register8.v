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
//  Implements an 8 bit register
//

module RegisterX(CLK,STB,ADDR,IN,OUT,CHANGED);

 parameter 			my_address = 0;
 parameter			WIDTH = 8;

 input 				CLK;		// system clock
 input 				STB;		// serial strobe
 input wire [6:0] 	ADDR;		// serial address in
 input wire [7:0] 	IN;			// serial data in
 output reg [7:0] 	OUT;		// register out
 output reg 		CHANGED;	// register changed flag

 always @(posedge CLK) begin
	// On strobe, check if address matches
	if (STB & (my_address == ADDR)) begin
		OUT <= IN;  // match, xfer to register
		CHANGED <= 1'b1;
	end
	else // no match
		CHANGED <= 1'b0;
 end

endmodule
