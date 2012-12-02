// V1.0 21st April 2008 
//
// Copyright 2006,2007, 2008  Phil Harman VK6APH
//
//  HPSDR - High Performance Software Defined Radio
//
//  Clock detector.
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


// clock detector, check if clock is present, if so set a flag.


module clock_det(clock, flag);
input  wire clock;
output reg  flag;
		
reg [10:0] clock_check;

always @ (posedge clock)
begin
  if (clock_check > 2000) // check that we have 2000 clock pulses before setting the flag
    flag        <= 1'b1;		  // set clock OK  flag
  else
    clock_check <= clock_check + 11'd1;
end 

endmodule
