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
// Implement the control registers for the EPI board.
`include "timescale.v"
`define MASK        4'h0
`define RELAY       4'h1
`define OC_OUT      4'h2
`define CROSS_PT_0  3'h3
`define CROSS_PT_1  4'h4
`define CROSS_PT_2  4'h5
`define CROSS_PT_3  4'h6
`define CROSS_PT_4  4'h7
`define CROSS_PT_5  4'h8

module epi_ctl_regs (
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

        // External Pin I/O
        go_ahead,
        cross_point_ctl,
        relay_out,
        receive_in,
        oc_out,
        stat_in
);
// Housekeeping
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

// External Pin I/O
output       go_ahead;
output [47:0] cross_point_ctl;
output [7:0] relay_out;
input  [7:0] receive_in;
output [7:0] oc_out;
output [7:0] stat_in;

reg [7:0] cp0;  // crosspoint 7-0
reg [7:0] cp1;  // crosspoint 15-8
reg [7:0] cp2;  // crosspoint 23-16
reg [7:0] cp3;  // crosspoint 31-24
reg [7:0] cp4;  // crosspoint 39-32
reg [7:0] cp5;  // crosspoint 47-40
assign cross_point_ctl = {cp5,cp4,cp3,cp2,cp1,cp0};

reg [7:0] relay_out;
reg [7:0] oc_out;
reg [7:0] stat_in;
reg [7:0] receive_mask;
reg [7:0] rcv_in;
reg [7:0] last_rcv_in;

wire val_wr = sel_i & cyc_i & we_i;

// Receive Mask 
always @(posedge clk or negedge rst_n)
  if(~rst_n)
    receive_mask <= 8'hff;
  else
  if((addr_i == `MASK) & val_wr)
    receive_mask <= dat_i;

// Cross point control reg 0
always @(posedge clk or negedge rst_n)
  if(~rst_n)
    cp0 <= 8'b0;
  else
  if((addr_i == `CROSS_PT_0) & val_wr)
    cp0 <= dat_i;

// Cross point control reg 1
always @(posedge clk or negedge rst_n)
  if(~rst_n)
    cp1 <= 8'b0;
  else
  if((addr_i == `CROSS_PT_1) & val_wr)
    cp1 <= dat_i;

// Cross point control reg 2
always @(posedge clk or negedge rst_n)
  if(~rst_n)
    cp2 <= 8'b0;
  else
  if((addr_i == `CROSS_PT_2) & val_wr)
    cp2 <= dat_i;

// Cross point control reg 3
always @(posedge clk or negedge rst_n)
  if(~rst_n)
    cp3 <= 8'b0;
  else
  if((addr_i == `CROSS_PT_3) & val_wr)
    cp3 <= dat_i;

// Cross point control reg 4
always @(posedge clk or negedge rst_n)
  if(~rst_n)
    cp4 <= 8'b0;
  else
  if((addr_i == `CROSS_PT_4) & val_wr)
    cp4 <= dat_i;

// Cross point control reg 5
always @(posedge clk or negedge rst_n)
  if(~rst_n)
    cp5 <= 8'b0;
  else
  if((addr_i == `CROSS_PT_5) & val_wr)
    cp5 <= dat_i;

always @(posedge clk or negedge rst_n)
  if(~rst_n)
    relay_out <= 8'h0;
  else
  if((addr_i == `RELAY) & val_wr)
    relay_out <= dat_i;

always @(posedge clk or negedge rst_n)
  if(~rst_n)
    oc_out  <= 8'h0;
  else
  if((addr_i == `OC_OUT) & val_wr)
    oc_out <= dat_i;

always @(posedge clk or negedge rst_n)
  if(~rst_n)
    rcv_in <= 8'h0;
  else 
    rcv_in <= receive_in;

always @(posedge clk or negedge rst_n)
  if(~rst_n)
    last_rcv_in <= 8'h0;
  else  
    last_rcv_in <= rcv_in & receive_mask;


// Just supply stat_in as the data output of the control
assign dat_o = stat_in;

wire [7:0] new_vect = rcv_in & receive_mask;
wire [7:0] old_vect = last_rcv_in & receive_mask;
wire [7:0] xor_vect =  new_vect ^ old_vect;

assign go_ahead = |xor_vect;

always @(posedge clk or negedge rst_n)
  if(~rst_n)
    stat_in <= 8'h0;
  else
  if(go_ahead)
    stat_in <= xor_vect;



endmodule
