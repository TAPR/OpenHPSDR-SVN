// V1.0 21st April 2008
//
// Copyright 2006,2007, 2008 Phil Harman VK6APH
//
//  HPSDR - High Performance Software Defined Radio
//
//  LED Flash
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


// Flash LED to indicate we have a clock selection error

module flash(
			 input clock,
			 input flag,
			 output reg LED);


reg[19:0]error_count;

always @ (posedge clock)
begin
	if (flag) begin
		if (error_count > 1000000)begin
			error_count <= 0;
			LED <= ~LED;			// error so flash LED
		end else 
			error_count <= error_count + 1'b1;
		end
	else begin	LED <= 1'b1;		// no error so LED off 
	end
end 

endmodule
