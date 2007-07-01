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
// The purpose of the code is to act as an    
// I2C slave. Note that there is NO provision for dealing with
// the 50ns glitch requirements in I2C provided within this code.
// Consequently - use a CLEAN line or implement external filters.
//
// V1:  Basically works - has a problem with address recognition.
// V2:  Modify sda_oe to add target to 2nd minterm.
// V3:  Changed sda_oe to not drive when NOT selected after addr phase.
//        Changed active edge that i2c_sm clocks on.
//        Made target a latch. 
// V4:  Added APB bus and do some debug 
// V5:  Blew away APB and put in Wishbone interface - 
//      this is to allow mulit-masters to access the I2C master
//      hardware. Seems to work now.
//
`include "timescale.v"
`define SM_IDLE   3'b000
`define SM_ADDR   3'b001
`define SM_WRITE  3'b010
`define SM_READ   3'b011
`define SM_WAIT   3'b100
`define NOTORIG

module i2c_slave (
	          rst_n,
                  clk,
                  my_addr,
	          sda_i, 
		  sda_out, 
		  sda_oe, 
		  scl_in,

                  // Wishbone Master Interface
                  // Used to talk to Other periphs
                  adr_o,
                  dat_i,
                  dat_o,
                  we_o,
                  sel_o,
                  stb_o,
                  ack_i,
                  cyc_o,
 
                  debug 
);

input             rst_n;
input             clk;
input [6:0]       my_addr;
input             sda_i;    // Serial Data In
output            sda_out;   // Serial Data out
output            sda_oe;    // Serial Data Out Enable
input             scl_in;    // Serial Clock In

// Wishbone interface
output [7:0]      adr_o;     // Address out
input  [7:0]      dat_i;     // Data Input
output [7:0]      dat_o;     // Data output
output            we_o;      // Write strobe
output            sel_o;     // Valid data asserted
output            stb_o;     // Data valid
input             ack_i;     // Slave Cycle Ack
output            cyc_o;     // Valid cycle
 
reg [7:0] adr_o;
reg [7:0] dat_o;
reg       we_o;
reg       sel_o;
reg       stb_o;
reg       cyc_o;


output [11:0]     debug;

reg start;
reg stop;
reg [9:0] rcv_reg;
reg [3:0] rcv_cntr;
reg [2:0] i2c_sm;
reg [2:0] nxt_i2c_sm;
reg [2:0] cyc_cnt;
reg       dat_o_d;
reg       cyc_start;
reg [1:0] wb_sm;

reg       target;
wire      ack;
wire      nack;
wire      int_sda;
reg  sda_in;
wire      read;
wire      write;

always @(sda_i)
  #1 sda_in = sda_i;


//
// Capture Start flag - SDA falls while SCL is high
//
wire start_clr = ~rst_n | (rcv_cntr == 4'h2);

always @(negedge sda_in or posedge start_clr)
  if(start_clr)
    start <= 1'b0;
  else
  if(scl_in == 1)
    start <= 1'b1;
//
// Capture Stop Flag - SDA rises while SCL is high
//
wire stop_clr = ~rst_n | ack | start;
always @(posedge sda_in or posedge stop_clr)
  if(stop_clr)
    stop <= 1'b0;
  else	    
  if(scl_in == 1)
    stop <= 1'b1;

// 
// RCV_CNTR - counts from 0 - 8 for each 8 bit packet + ACK/NACK
//
wire cnt8 = (rcv_cntr == 4'h8);
wire cnt7 = (rcv_cntr == 4'h7);
wire cntr_rst = ~rst_n;
always @(negedge scl_in or posedge cntr_rst)
  if(cntr_rst)
    rcv_cntr <= 4'b0;
  else
  if(cnt8 | start & ((i2c_sm == `SM_WRITE) | (i2c_sm == `SM_READ)) )  
    rcv_cntr <= 4'b0;
  else
  if(i2c_sm != `SM_IDLE)
    rcv_cntr <= rcv_cntr + 4'h1;	  

//
// Receive shift register
//
always @(posedge scl_in or negedge rst_n)
 if(~rst_n)
   rcv_reg <= 10'b0;
 else
 if(rcv_cntr <= 4'h7)
   rcv_reg <= {rcv_reg[8:0],sda_in};

// Decode ACK/NACK
// Note that sync_sda is a leading edge detector looking
// for the falling edge of sda_in;
//
assign ack  = cnt8 & ~sda_in & sync_write | 
              cnt8 & ~sda_in & ~sync_write;

assign nack = cnt8 & sda_in;

// rcv_reg decode
wire [6:0] slave_addr = rcv_reg[7:1];
wire       i2c_write = ~rcv_reg[0] & (i2c_sm == `SM_ADDR);
wire       addr_dec  = (slave_addr == my_addr) & (i2c_sm == `SM_ADDR);
assign     read = (i2c_sm == `SM_READ);
assign     write = (i2c_sm == `SM_WRITE);

reg lcl_write;
wire sm_rst = stop | ~rst_n;   
always @(negedge scl_in or posedge sm_rst)
  if(sm_rst)
     lcl_write <= 1'b0;
  else
  if((i2c_sm == `SM_ADDR) & (rcv_cntr == 5'h7))
     lcl_write <= ~rcv_reg[0];

//
// I2C State Machine
//  
// The state machine waits for a START sequence, then waits 
// to become the target of the transaction. If it isn't the 
// target, then it goes back to IDLE.  If it IS the target,
// then it decides whether it's a READ or WRITE sequence. 
// When we're in a READ sequence - and we get to the end, 
// we move to WAIT until a ACK occurs. 
//

always @(start or cnt8 or addr_dec or
         ack or i2c_write or nack or i2c_sm)
  case(i2c_sm)
     `SM_IDLE:  if(start)
	          nxt_i2c_sm = `SM_ADDR;
	        else
	          nxt_i2c_sm = `SM_IDLE;
     `SM_ADDR:  if(cnt8 & ~addr_dec)
	          nxt_i2c_sm = `SM_IDLE;
	        else
	        if(cnt8 & addr_dec & ack & i2c_write)
	          nxt_i2c_sm = `SM_WRITE;
	        else
                if(cnt8 & addr_dec & ack & ~i2c_write)
                  nxt_i2c_sm = `SM_READ;
	        else
                if(cnt8 & addr_dec & nack)
                  nxt_i2c_sm = `SM_IDLE;
	        else
                  nxt_i2c_sm = `SM_ADDR;
     `SM_WRITE: if(start)
                  nxt_i2c_sm = `SM_ADDR;	
                else
                if(cnt8 & ack)
	          nxt_i2c_sm = `SM_WRITE;
	        else
                  nxt_i2c_sm = `SM_WRITE;
     `SM_READ:  if(start)
                  nxt_i2c_sm = `SM_ADDR;
                else
                if(cnt8 & nack)			 
	          nxt_i2c_sm = `SM_WAIT;
	        else
	          nxt_i2c_sm = `SM_READ;
     `SM_WAIT:  nxt_i2c_sm = `SM_WAIT;
      default:  nxt_i2c_sm = `SM_IDLE;
   endcase

always @(negedge scl_in or posedge sm_rst)
  if(sm_rst)
    i2c_sm <= `SM_IDLE;
  else
    i2c_sm <= nxt_i2c_sm;


//
// This counter is used to drive successive 8 bit registes in a write burst or
// Read burst. The cntr increments on the ack.
//
wire cyc_rst = ~rst_n | stop  | start;
always @(posedge scl_in or posedge cyc_rst)
   if(cyc_rst == 1'b1)
      cyc_cnt <= 3'b0;
   else	   
   if(target & cnt8)
      cyc_cnt <= cyc_cnt + 3'b1;

//
// Determine when to drive SDA to indicate an ACK
//
// You drive SDA to 0 when it's an ACK and we're the receiving
// party.  We receive SDA at cnt8 when we're the transmitter  looking
// for an ACK.
//

`ifdef NOTORIG
wire sda_oe = target & (i2c_sm == `SM_READ) & ~cnt8 | // Enable rd data
              (i2c_sm == `SM_ADDR) & cnt8 & addr_dec  |
	      target & (i2c_sm == `SM_WRITE) & cnt8 ; // enbl Write ACK

`else
reg sda_oe;
always @(negedge scl_in or negedge rst_n)
  if(~rst_n)
    sda_oe <= 1'b0;
  else
    sda_oe <= target & read & ~cnt7 |
              (i2c_sm == `SM_ADDR) & cnt7 |
              write & cnt7;

`endif

assign  sda_out = (target & read & ~cnt8) ? int_sda : // Read case
	                            1'b0;     // ACK case

//
// Set Target when my_addr has been sent on the first
// transaction in the I2C sequence to us.  This means
// we're the target of the transaction.
//
`ifdef NOTORIG
always @(cyc_rst or scl_in or i2c_sm or addr_dec or cnt8 or stop)
  if(cyc_rst)
    target = 1'b0;
  else
  if(~scl_in & cnt8 & (i2c_sm == `SM_ADDR))
       target = addr_dec;
  else 
  if(stop )
      target = 0;
   
`else
always @(posedge scl_in or posedge cyc_rst)
  if(cyc_rst == 1'b1)
    target <= 1'b0;
  else
  if( (i2c_sm == `SM_ADDR))
    target <= addr_dec;
`endif

//
// Int_reg is the internal register that we are writting too.
//

reg [7:0] int_reg0;
reg [7:0] int_reg1;
wire load_int_reg = ack & target & write;

always @(posedge scl_in or negedge rst_n)
  if(~rst_n)
    begin
     int_reg0 <=  8'b0;
     int_reg1 <= 8'b0;
    end
  else
  if(load_int_reg)
    case(cyc_cnt)
       3'h0: int_reg0 <= rcv_reg[7:0];
       3'h1: int_reg1 <= rcv_reg[7:0];
    endcase

//
// Mux down the received regs to the one that corresponds to the current
// transfer count.
//


reg [7:0] data_out;
always @(posedge clk or negedge rst_n)
  if(~rst_n)
    data_out <= 8'h0;
  else
  if(ack_i & ~we_o)
    data_out <= dat_i;

assign int_sda = data_out[7-rcv_cntr[2:0]]; 
assign debug   = {scl_in,target,stop,start,rcv_cntr,sda_oe,i2c_sm};

`define CYC_IDLE 3'b000
`define CYC_WT0  3'b001
`define CYC_ADDR 3'b010
`define CYC_WDATA 3'b011
`define CYC_MVDT 3'b100
`define CYC_RDATA 3'b101
reg [2:0] nxt_cyc_sm, cyc_sm;

wire sync_start, sync_ack, sync_target, sync_write, sync_stop;
wire sync_sda;

sync_le u_sync_sda       (clk,rst_n,~sda_in,sync_sda);
sync_le u_sync_ack       (clk,rst_n,ack,sync_ack);
sync_le u_sync_start     (clk,rst_n,start,sync_start);
sync_le u_sync_stop      (clk,rst_n,stop,sync_stop);
sync_level u_sync_target (clk,rst_n,target,sync_target);
sync_level u_sync_write  (clk,rst_n,lcl_write,sync_write);


// 
// Cycle state machine - turns Successive cycles on 
// I2C into Bus Mode cycles
// 

always @(sync_start or sync_ack or sync_target or
         sync_write or cyc_sm)
  case(cyc_sm)
     `CYC_IDLE:  if(sync_start)
                   nxt_cyc_sm = `CYC_WT0;
                 else
                   nxt_cyc_sm = `CYC_IDLE;
     //`CYC_WT0:   if(sync_ack & ~sync_write & sync_target) 
     //              nxt_cyc_sm = `CYC_RDATA;
     //            else
     //            if(sync_ack & sync_write & sync_target) 
     //              nxt_cyc_sm = `CYC_ADDR;
     //            else
     //              nxt_cyc_sm = `CYC_WT0;
     `CYC_WT0:   if(sync_start) 
                    nxt_cyc_sm = `CYC_WT0;
                 else
                 if(sync_ack &  sync_target & sync_write ) 
                    nxt_cyc_sm = `CYC_ADDR;
                 else
                 if(sync_ack &  sync_target & ~sync_write ) 
                    nxt_cyc_sm = `CYC_RDATA;
                 else
                    nxt_cyc_sm = `CYC_WT0;

     `CYC_ADDR:  if(sync_ack & sync_write)
                   nxt_cyc_sm = `CYC_WDATA;
                 else
                 if(sync_ack & ~sync_write)
                   nxt_cyc_sm = `CYC_RDATA;
                 else
                   nxt_cyc_sm = `CYC_ADDR;
     `CYC_WDATA: if(sync_start) 
                   nxt_cyc_sm = `CYC_WT0;
                 else
                 if(sync_ack)
                   nxt_cyc_sm = `CYC_MVDT;
                 else
                   nxt_cyc_sm = `CYC_WDATA;
     `CYC_MVDT:  nxt_cyc_sm = `CYC_WDATA;
     `CYC_RDATA: if(sync_start)
                   nxt_cyc_sm <= `CYC_WT0;
                 else 
                 if(sync_stop)
                   nxt_cyc_sm <= `CYC_IDLE;
                 else
                   nxt_cyc_sm <= `CYC_RDATA;
      endcase

always @(posedge clk or negedge rst_n)
  if(~rst_n)
     cyc_sm <= `CYC_IDLE;
  else
  if(sync_stop) 
     cyc_sm <= `CYC_IDLE;
  else
     cyc_sm <= nxt_cyc_sm;

// Decode SM & Generate Wishbone bus

// Load on ADDR phase, and incr in WDATA mode
always @(posedge clk or negedge rst_n)
  if(!rst_n)
    adr_o <= 8'h0;
  else
  if(sync_target & (cyc_sm == `CYC_ADDR) & sync_ack)
    adr_o <= rcv_reg[7:0];
  else
  if((sync_target & (cyc_sm == `CYC_WDATA) & ack_i) |
     (sync_target & (cyc_sm == `CYC_RDATA) & ack_i))
    adr_o <= adr_o + 8'h1;

always @(posedge clk or negedge rst_n)
  if(!rst_n)
    dat_o <= 8'h0;
  else
  if(sync_target & cyc_sm == `CYC_WDATA & sync_ack)
    dat_o <= rcv_reg[7:0];

always @(posedge clk or negedge rst_n)
  if(~rst_n)
    cyc_start  <= 1'b0; 
  else
    cyc_start   <= 
                   (sync_target & (cyc_sm == `CYC_WDATA) & sync_ack  |
           sync_target & (cyc_sm == `CYC_RDATA) & sync_sda) & cnt8 |
           sync_target & (cyc_sm == `CYC_WT0) & sync_ack & ~sync_write;

always @(posedge clk or negedge rst_n)
  if(~rst_n)
    begin
      cyc_o <= 1'b0;
      sel_o <= 1'b0;
      stb_o <= 1'b0;
      we_o  <= 1'b0;
    end
  else
    begin
      cyc_o <= cyc_start | cyc_o & ~ack_i;
      sel_o <= cyc_start | sel_o & ~ack_i;
      stb_o <= cyc_start | stb_o & ~ack_i;
      we_o  <= cyc_start & lcl_write | we_o & ~ack_i;
    end

endmodule 

module sync_le (clk,rst_n,din,dout);
input clk;
input rst_n;
input din;
output dout;

reg q;
reg q_d;
reg q_dd;
  
always @(posedge clk or negedge rst_n)
  if(~rst_n)
    begin
      q <= 0;
      q_d <= 0;
      q_dd <= 0;
    end     
  else
    begin
      q <= din;
      q_d <= q;
      q_dd <= q_d;
    end

assign dout = ~q_dd & q_d;
 
endmodule

module sync_level (clk,rst_n,din,dout);
input clk;
input rst_n;
input din;
output dout;

reg q;
reg q_d;
  
always @(posedge clk or negedge rst_n)
  if(~rst_n)
    begin
      q <= 0;
      q_d <= 0;
    end     
  else
    begin
      q <= din;
      q_d <= q;
    end

assign dout = q_d;
 
endmodule
