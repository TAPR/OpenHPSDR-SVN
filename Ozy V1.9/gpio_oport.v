//  V1.0 November 30, 2008
//
//  Copyright (C) 2006  Kirk Weedman, KD7IRS
//
//  HPSDR - High Performance Software Defined Radio
//
//	gpio SPI REGS for OZY V1
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
//  along with this program; if not, wr_strb to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
//  Implements an 8 bit register
//

`timescale 1 ns/100 ps

module gpio_oport(clk,wr_strb,addr,s_in,gpio);

parameter  my_address = 0;
parameter  WIDTH = 8;

input 					        clk;      // system clock
input                   wr_strb;  // serial strobe
input  wire       [5:0] addr;     // serial address in
input  wire [WIDTH-1:0] s_in;     // serial data in
inout  wire [WIDTH-1:0] gpio;     // gpio

reg [WIDTH-1:0] data_reg;
 
genvar i;
generate
begin: IO
  for (i = 0; i < WIDTH; i = i + 1)
  begin: IO_DRIVE
    assign gpio[i] = data_reg[i];
  end
end
endgenerate
 
always @(posedge clk)
begin
  // On strobe, check if address matches
  if (wr_strb && (my_address == addr))
    data_reg  <= s_in;  // match, xfer to data register
end

endmodule
