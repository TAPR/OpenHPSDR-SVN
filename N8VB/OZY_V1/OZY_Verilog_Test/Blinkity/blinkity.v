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
module blinkity(fx2_clk, Pin67, Pin68, Pin69, 
Pin70, Pin72, Pin74, Pin75, Pin76);

	input fx2_clk;
	output Pin67;
	output Pin68;
	output Pin69;
	output Pin70;
	output Pin72;
	output Pin74;
	output Pin75;
	output Pin76;
	
	reg [32:0] COUNTER;
	
	always @(posedge fx2_clk)
		begin
			COUNTER <= COUNTER + 32'd1;
		end
	
	assign Pin67 = COUNTER[22];
	assign Pin68 = COUNTER[23];
	assign Pin69 = COUNTER[24];
	assign Pin70 = COUNTER[25];
	assign Pin72 = COUNTER[26];
	assign Pin74 = COUNTER[27];
	assign Pin75 = COUNTER[28];
	assign Pin76 = COUNTER[29];
	
endmodule

