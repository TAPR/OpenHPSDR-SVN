//  V1.0 August 16, 2006
//
//  Copyright 2006  P. Covington, N8VB
//
//  HPSDR - High Performance Software Defined Radio
//
//	GPIO SPI REGS for OZY V1
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
//  Implements a 8 bit register
//

module Register8(CLK,STB,ADDR,IN,OUT,DELTA);

 parameter 			my_address = 0;

 input 				CLK;
 input 				STB;
 input wire [6:0] 	ADDR;
 input wire [7:0] 	IN;
 output reg [7:0] 	OUT;
 output reg 		DELTA;

 always @(posedge CLK) begin
	if (STB & (my_address == ADDR)) begin
		OUT <= IN;
		DELTA <= 1'b1;
	end
	else 
		DELTA <= 1'b0;
 end

endmodule
