// -*- verilog -*-
//
//  HPSDR - High Performance Software Defined Radio
//
//  Copyright (C) 2006 Philip A Covington, N8VB
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
// July 6, 2006
// Simple Test code for OZY REVA ALPHA
// Just counts the 12 MHz clock and blinks LEDs 1, 3-9
//
// NOTES:
// Make sure all unused pins are configured as inputs, tri-stated...
//
module blinkity(fx2_clk, Pin4, Pin33, Pin34, Pin108); 

	input fx2_clk;
	output Pin4;
	output Pin33;
	output Pin34;
	output Pin108;
	
	reg [32:0] COUNTER;
	
	always @(posedge fx2_clk)
		begin
			COUNTER <= COUNTER + 32'd1;
		end
	
	assign Pin4 = COUNTER[22];
	assign Pin33 = COUNTER[23];
	assign Pin34 = COUNTER[24];
	assign Pin108 = COUNTER[25];
	
endmodule

