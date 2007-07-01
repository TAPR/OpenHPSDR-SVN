/////////////////////////////////////////////////////////////////////
////                                                             ////
//// Copyright (C) 2006 Steven Wilson                            ////
////                    ka6s@yahoo.com                           ////
//
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published
//  by the Free Software Foundation; version 1 
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
// This design is LGPL software written for the HPSDR project
// by Steve Wilson, KA6S.  The author assumes no libability on how the
// code might be used or warranty that the code is fit for any 
// particular application. Further, I2C is a patented technology 
// of Philips corporation and requires a license. If you are going
// to use this in a product - get the license!
//
// Implement a two channel arbiter/mux for wishbone for use in the 
// HPSDR project.
//
module wb_arb_mux (
        wb_clk_i,
        wb_rst_i,

        // Channel 0 WB Interface
        adr_0_i,
        dat_0_i,
        dat_0_o,
        we_0_i,
        sel_0_i,
        stb_0_i,
        ack_0_o,
        cyc_0_i,

        // Channel 1 WB Interface
        adr_1_i,
        dat_1_i,
        dat_1_o,
        we_1_i,
        sel_1_i,
        stb_1_i,
        ack_1_o,
        cyc_1_i,

        adr_o,
        dat_o,
        dat_i,
        we_o,
        sel_o,
        stb_o,
        cyc_o,
        ack_i 
);
input   wb_clk_i;
input   wb_rst_i;

// Channel 0 WB Interface
input [7:0] adr_0_i;
input [7:0] dat_0_i;
output [7:0] dat_0_o;
input       we_0_i;
input       sel_0_i;
input       stb_0_i;
output      ack_0_o;
input       cyc_0_i;

// Channel 1 WB Interface
input [7:0] adr_1_i;
input [7:0] dat_1_i;
output [7:0] dat_1_o;
input       we_1_i;
input       sel_1_i;
input       stb_1_i;
output      ack_1_o;
input       cyc_1_i;

output [7:0] adr_o;
output [7:0] dat_o;
input  [7:0] dat_i;
output       we_o;
output       sel_o;
output       stb_o;
output       cyc_o;
input        ack_i;

// Basic Arbiter goes here
`define IDLE 2'b00
`define CH0  2'b01
`define CH1  2'b10

reg [1:0] arb_sm;
always @(posedge wb_clk_i or negedge wb_rst_i)
  if(~wb_rst_i)
    arb_sm <= `IDLE;
  else
  if(cyc_0_i)
    arb_sm <= `CH0;
  else
  if(~cyc_0_i & cyc_1_i)
    arb_sm <= `CH1;
  else
  if((arb_sm == `CH0) & cyc_0_i & ~ack_i)
    arb_sm <= `CH0;
  else
  if((arb_sm == `CH1) & cyc_1_i & ~ack_i)
    arb_sm <= `CH1;
  else
    arb_sm <= `IDLE;

wire channel_1 = (arb_sm == `CH1);

// Simple mux goes here to select between the two 
// channels.

assign adr_o = channel_1 ? adr_1_i : adr_0_i;
assign dat_o = channel_1 ? dat_1_i : dat_0_i;
assign dat_0_o = dat_i; // Feed both channels the 
assign dat_1_o = dat_i; // the dat_i bus 
assign we_o  = channel_1 ? we_1_i : we_0_i;
assign stb_o = channel_1 ? stb_1_i : stb_0_i;
assign cyc_o = channel_1 ? cyc_1_i : cyc_0_i;
assign ack_0_o = (arb_sm == `CH0) & ack_i;
assign ack_1_o = channel_1 & ack_i;

endmodule
