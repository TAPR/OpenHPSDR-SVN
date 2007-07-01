/////////////////////////////////////////////////////////////////////
////                                                             ////
//// Copyright (C) 2007 Steven Wilson                            ////
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
// Implements an 8 deep 1 write, 2 read port reg file
// Reg file is 16 bits wide for 2nd port, 8 bits wide on
// wishbone.
// Write & 1 read port tied to wishbone,
// 2nd read port tied to micro-coded state machine
//
`include "timescale.v"
module status_mcode (
           // Housekeeping
           clk,
           rst_n,
     
           // Wishbone
           addr_i,
           dat_i,
           dat_o,
           sel_i,
           cyc_i,
           we_i,
           ack_o,

           mc_addr,
           mc_word 
);
input        clk;
input        rst_n;
     
           // Wishbone
input [3:0]  addr_i;
input [7:0]  dat_i;
output [7:0] dat_o;
input        sel_i;
input        cyc_i;
input        we_i;
output       ack_o;

input [2:0]   mc_addr;
output [15:0] mc_word;

reg [7:0] bank0 [0:7];
reg [7:0] bank1 [0:7];
reg       ack_o;

// Wishbone write cycle
always @(posedge clk or negedge rst_n)
  if(~rst_n)
    begin
      bank0[0] <= 8'b0;
      bank0[1] <= 8'b0;
      bank0[2] <= 8'b0;
      bank0[3] <= 8'b0;
      bank0[4] <= 8'b0;
      bank0[5] <= 8'b0;
      bank0[6] <= 8'b0;
      bank0[7] <= 8'b0;
      bank1[0] <= 8'b0;
      bank1[1] <= 8'b0;
      bank1[2] <= 8'b0;
      bank1[3] <= 8'b0;
      bank1[4] <= 8'b0;
      bank1[5] <= 8'b0;
      bank1[6] <= 8'b0;
      bank1[7] <= 8'b0;
    end
  else
  if(we_i & cyc_i & sel_i)
    if(addr_i[0] == 0)
      bank0[addr_i[3:1]] <= dat_i;
   else 
      bank1[addr_i[3:1]] <= dat_i;

// Wishbone read port
assign dat_o = addr_i[0] ? bank1[addr_i[3:1]] :
                           bank0[addr_i[3:1]];

// Wishbone ACK
always @(posedge clk or negedge rst_n)
  if(~rst_n)
    ack_o <= 1'b0;
  else
  if(cyc_i & ~ack_o)
    ack_o <= 1'b1;
  else
    ack_o <= 1'b0;

// Microcode Read port
assign mc_word = { bank1[mc_addr], bank0[mc_addr]};

wire [7:0] wd0 = bank0[0] ;
wire [7:0] wd1 = bank1[0];
wire [7:0] wd2 = bank0[1];
wire [7:0] wd3 = bank1[1];
wire [7:0] wd4 = bank0[2];
wire [7:0] wd5 = bank1[2];
wire [7:0] wd6 = bank0[3];
wire [7:0] wd7 = bank1[3] ;
wire [7:0] wd8 = bank0[4] ;
wire [7:0] wd9 = bank1[4] ;
wire [7:0] wda = bank0[5] ;
wire [7:0] wdb = bank1[5] ;
wire [7:0] wdc = bank0[6] ;
wire [7:0] wdd = bank1[6] ;
wire [7:0] wde = bank0[7] ;
wire [7:0] wdf = bank1[7] ;

endmodule
