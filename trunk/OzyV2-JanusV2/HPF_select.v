// V1.0 19th November 2007
//
// Copyright 2006,2007 Phil Harman VK6APH
//
//  HPSDR - High Performance Software Defined Radio
//
//  Alex SPI interface.
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


// selects HPF based on frequency in use.

module HPF_select(frequency,HPF);

input wire [31:0]frequency;
output reg [5:0]HPF;

always @ (frequency)
begin
if 		(frequency <  1500000) HPF <= 6'b00_0001; 	// bypass
else if	(frequency <  6500000) HPF <= 6'b10_0000;	// 1.5MHz HPF	
else if (frequency <  9500000) HPF <= 6'b01_0000;	// 6.5MHz HPF
else if (frequency < 13000000) HPF <= 6'b00_1000;	// 9.5MHz HPF
else if (frequency < 20000000) HPF <= 6'b00_0100;	// 13MHz HPF
else    					   HPF <= 6'b00_0010;	// 20MHz HPF
end
endmodule