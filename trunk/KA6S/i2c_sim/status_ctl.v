/////////////////////////////////////////////////////////////////////
////                                                             ////
//// Copyright (C) 2007 Steven Wilson                            ////
////                    ka6s@yahoo.com                           ////
//
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as 
//  published by the Free Software Foundation; version 1 
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, 
//  Boston, MA  02111-1307  USA
//
// This design is LGPL software written for the HPSDR project
// by Steve Wilson, KA6S.  The author assumes no libability on how the
// code might be used or warranty that the code is fit for any 
// particular application. Further, I2C is a patented technology 
// of Philips corporation and requires a license. If you are going
// to use this in a product - get the license!
//
// This module constitutes a vertically micro-coded engine that
// drives the I2C master module.  
//
// The SM waits till it receives a go_ahead strobe from the 
// status change detection logic, and proceeds to send the message
// stored in the micro-code ram to the I2C master.
//
// The state machine interacts with the I2C master via the wishbone
// bus. Each Micro control word causes a single transaction on 
// the wishbone bus.

`include "timescale.v"
module status_ctl (
              // Misc
              clk,
              rst_n,
              
              // Status interface 
              go_ahead,
              stat_in,

              // Microstore Interface
              mc_word,
              mc_addr,
              
              // Wishbone
              addr_o,
              dat_o,
              dat_i,
              we_o,
              stb_o,
              cyc_o,
              ack_i,
              int
);
// Housekeeping
input    clk;
input    rst_n;

// Status Intf
input        go_ahead;
input [7:0]  stat_in;

input [15:0] mc_word;
output [2:0] mc_addr;

output [2:0] addr_o;
output [7:0] dat_o;
input  [7:0] dat_i;
output       we_o;
output       stb_o;
output       cyc_o;
input        ack_i;
input        int;

`define IDLE 3'b000
`define RUN0 3'b001
`define RUN1 3'b010
`define WAIT 3'b100

//
// Micro-code mapping 
// 
// mc_word[7:0]  - Data Out/Comparison word
// mc_addr[10:8] - Address to I2C master
// mc_word[11]   - Source 0=mc_word[7:0], 1=stat_in
// mc_word[13]  -  00 - write
// mc_word[14]     01 - Read & Compare/loop  
//                 10 - Wait till Interrupt
// mc_word[15]  - Stop execution after transaction
//
assign addr_o = mc_word[10:8];
wire   stat_sel = mc_word[11];
assign dat_o = stat_sel ? stat_in : mc_word[7:0];
assign we_o  = (mc_word[14:13] == 2'b00);
wire loop    = (mc_word[14:13] == 2'b01);
wire wait_till_int =  (mc_word[14:13] == 2'b10);
wire stop = mc_word[15];

reg [2:0] stat_sm, nxt_stat_sm;
reg cyc_o;
wire neq = |(dat_i & mc_word[7:0]);
reg run_cyc;
reg incr_mcadr;
reg clr_mcadr;
 

// 
// Microcode state machine
//
// IDLE - 0 stays put until a go_ahead strobe is received
// RUN0 - Looks at whether wait_till_int is asserted, if so
//        branches to WAIT:
// RUN1 - Runs a Wishbone Cycle
//        if(loop & neq & read cycle) then re-run cycle
//        if(stop) goto IDLE, clear mc_addr.
//        else incr mc_addr & goto RUN0:
// WAIT - Wait till interrupt occurs 
//


always @(go_ahead or loop or int or wait_till_int or 
         neq or stop or ack_i or we_o)
  case(stat_sm)
    `IDLE:  if(go_ahead)
              begin
                nxt_stat_sm = `RUN0;
                run_cyc = 1'b1;
                incr_mcadr = 1'b0;
                clr_mcadr = 1'b0;
              end
            else
              begin
                nxt_stat_sm = `IDLE;
                run_cyc  = 0;
                incr_mcadr = 1'b0;
                clr_mcadr = 1'b0;
              end
    `RUN0:  if(wait_till_int)      
              begin
                nxt_stat_sm = `WAIT;
                run_cyc = 0;
                incr_mcadr = 1'b0;
                clr_mcadr = 1'b0;
              end
            else
              begin
                nxt_stat_sm = `RUN1;
                run_cyc = 1;
                incr_mcadr = 1'b0;
                clr_mcadr = 1'b0;
              end
    `RUN1:  if(~ack_i)
              begin
                nxt_stat_sm = `RUN1;
                run_cyc = 0;
                incr_mcadr = 1'b0;
                clr_mcadr = 1'b0;
              end
            else
            if(loop & neq & ~we_o) // Check for Read & Compare
              begin
                nxt_stat_sm = `RUN1;
                run_cyc = 1;
                incr_mcadr = 1'b0;
                clr_mcadr = 1'b0;
              end
            else
            if(stop) // We're done
              begin
                nxt_stat_sm = `IDLE;
                run_cyc = 0;
                incr_mcadr = 1'b0;
                clr_mcadr = 1'b1;
              end
            else
              begin // Go to the next Microword
                nxt_stat_sm = `RUN0;
                run_cyc = 0;
                incr_mcadr = 1'b1;
                clr_mcadr = 1'b0;
              end
    `WAIT: if(int)
              begin // Found interrupt
                nxt_stat_sm = `RUN1;
                run_cyc = 1;
                incr_mcadr = 1'b0;
                clr_mcadr = 1'b0;
              end
           else
              begin // still waiting
                nxt_stat_sm = `WAIT;
                run_cyc = 0;
                incr_mcadr = 1'b0;
                clr_mcadr = 1'b0;
              end 
    default:
              begin // still waiting
                nxt_stat_sm = `IDLE;
                run_cyc = 0;
                incr_mcadr = 1'b0;
                clr_mcadr = 1'b0;
               end
  endcase
          
// State register for State machine
always @(posedge clk or negedge rst_n)
  if(~rst_n)
    stat_sm <= `IDLE;
  else
    stat_sm <= nxt_stat_sm;

reg [2:0] mc_addr;

// Micro Code Address generator 
always @(posedge clk or negedge rst_n)
  if(~rst_n)
    mc_addr <= 3'b000;
  else
  if(clr_mcadr)
    mc_addr <= 3'b000;
  else
  if(incr_mcadr)
    mc_addr <= mc_addr + 3'b001;
 

//
// Wishbone Cycle generator
//
reg  stb_o;
always @(posedge clk or negedge rst_n)
  if(~rst_n)
    begin
      cyc_o <= 1'b0;
      stb_o <= 1'b0;  
    end
  else
  if(run_cyc)
    begin
      cyc_o <= 1'b1;
      stb_o <= 1'b1;  
    end
  else
  if(ack_i)
    begin
      cyc_o <= 1'b0;
      stb_o <= 1'b0;  
    end

endmodule
              


