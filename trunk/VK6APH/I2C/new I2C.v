/////////////////////////////////////////////////////////////////////
////                                                             ////
//// Copyright (C) 2006 Phil Harman VK6APH                     	 ////
////                    pvharman@arach.net.au                 	 ////
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
// by Phil Harman, VK6APH.  The author assumes no libability on how the
// code might be used or warranty that the code is fit for any 
// particular application. 
//
// The purpose of the code is to act as an I2C slave. 
//
// Sends data to I2C slave module in FPGA, output is connected to the Ozy LEDs

`define ADDRESS 7'h27 // this is the address that the I2C will respond to NOTE: 7 bits

module I2C(CLK, sda, scl, LED, debug);

input CLK;  //48MHz from FX2 
inout sda;
input scl;
output [7:0]LED;
output [9:0]debug;

reg reset;
wire [7:0]int_reg0; // received data [7:0]
wire [7:0]int_reg1; // received data [15:8]
wire [7:0]rcv_reg0;	// send data [7:0]
wire [7:0]rcv_reg1; // send data [15:8]
wire sda_out;
wire sda_oe;
wire dg_scl;		// de-glitched scl 
wire dg_sda;		// de-glitched sda		

// reset logic 
reg [5:0]reset_count;

always @ (posedge CLK) begin
	if (reset_count[5]) begin
		reset <= 1'b1;		// reset high to run
	end
	else begin
		reset <= 0;
		reset_count <= reset_count + 1'b1;
	end
end 


// de-glitch sda and scl using CLK
// sda and scl have to be stable for 4 consecutive CLK samples before a transition is considered valid

deglitch(.clock(CLK), .in(scl),.out(dg_scl));
deglitch(.clock(CLK), .in(sda),.out(dg_sda));

//I2C slave module
/*
i2c_slave(.rst_n(reset),.my_addr(`ADDRESS),.sda_i(dg_sda),.sda_out(sda_out),.sda_oe(sda_oe),.scl_in(dg_scl),.int_reg(LED)
			,.debug(debug));
*/			
			
i2c_slave(.rst_n(reset),.my_addr(`ADDRESS),.sda_i(dg_sda),.sda_out(sda_out),.sda_oe(sda_oe),.scl_in(dg_scl),
			.int_reg0(),.int_reg1(LED),.rcv_reg0(rcv_reg0),.rcv_reg1(rcv_reg1),.debug(debug)); 
			
			
// sda can pull to 0v but must go high Z to signal a logic 1 

//assign sda = sda_oe ? (sda_out ? 1'bz : 1'b0) : 1'bz;	

assign sda = sda_oe ? sda_out  : 1'bz;		

endmodule