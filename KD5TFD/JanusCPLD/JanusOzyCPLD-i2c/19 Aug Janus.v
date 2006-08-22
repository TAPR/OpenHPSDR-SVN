// 
// Copyright (c) Bill Tracey 2006 (bill@ewjt.com) & Phil Harman (pvharman@arach.net.au)
//
//  HPSDR - High Performance Software Defined Radio
//
//
//  Janus to Ozy interface. 
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
// 
// CPLD code for Janus board to interface with a Xylo board.
// Sets I/O pins on ADC and DAC chips plus provides clocks.
//
// TLV320 is in slave mode and AK5394A is Master. Note that both devices are fed from 
// the CLK_24MHz/2. 
//
// Sets up TLV320 via its SPI interface
//
// All unused pins on the CPLD have been set as tristate inputs in Quartus II
// 
// Bulit using Quartus II v6.0 SP1 build 202.


// Change log:
//
//	12 August 2006 - increased reset to 3 seconds otherwise AK 5394A did not 
//                   reset correctly when connected to OZY. VK6APH 
//	12 August 2006 - forced speed to 48k for testing 
//  13 August 2006 - AK5394A now reset at power on and speed setting via Ozy
//  14 August 2006 - inverted PTT pass to Atlas C3
//
//
//  IMPORTANT: AK5394A nRST is connected to AK_reset input. Unless this is connected to 
//  +3.3v or an Ozy board the AK5394A will remain in reset and produce no clocks etc.

module Janus(
   input  CLK_24MHZ,
   input  CAL,
   output CBCLK,
   output CDIN,
   input  CDOUT,
   output CLRCIN,
   output CLRCOUT,
   output CMCLK,
   output CMODE,
   output DFS0,
   output DFS1,
   output EXP1,
   output EXP2,
   output EXP3,
   output EXP4,
   input  FSYNC,
   output HPF,
   output IPWM,
   input  LRCLK,
   output MCLK,
   output nCS,
   output nRST,
   input  PTT,
   output C3,
   output QPWM,
   input  SCLK,
   input  SDOUT,
   output SMODE1,
   output SMODE2,
   output SSCK,
   output MOSI,
   output ZCAL,
   output YB7,
   input  YC6,
   input  YA3,
   output YB4,
   input  YA7,
   input  YC3,
   output YC2,
   input  YC1,
   input  YA6,
   input  YB3,
   output YB1,
   output YB5,
   output YB6,
   input  AK_reset

); 

reg clock_by_2; 
reg index;
reg [15:0]tdata;
reg [2:0]load;
reg [3:0]TLV;
reg [15:0] TLV_data;
reg TLV_CLK;
reg data; 
reg [3:0] bit_count;
reg TLV_nCS;



// divide 24.576MHz clock by two for TLV320 and AK5394A
always @ ( posedge  CLK_24MHZ) begin 
	clock_by_2 <= ~clock_by_2;
	end 

//////////////////////////////////////////////////////////////
//
// 		Set up TLV320 using SPI until I2C on OZY is going 
//
/////////////////////////////////////////////////////////////

/* Data to send to TLV320 is 

 	1E 00 - Reset chip
 	12 01 - set digital interface active
 	08 15 - D/A on, mic input, mic 20dB boost
 	08 14 - ditto but no mic boost
 	0C 00 - All chip power on
 	0E 02 - Slave, 16 bit, I2S
 	10 00 - 48k, Normal mode
 	0A 00 - turn D/A mute off

*/

// Set up TLV320 data to send 

always @ (posedge index)		// why can't this just be 'always @(index)' ?? 
begin
load <= load + 3'b1;			// select next data word to send
case (load)
3'd0: tdata <= 16'h1E00;		// data to load into TLV320
3'd1: tdata <= 16'h1201;
3'd2: tdata <= 16'h0815;
3'd3: tdata <= 16'h0C00;
3'd4: tdata <= 16'h0E02;
3'd5: tdata <= 16'h1000;
3'd6: tdata <= 16'h0A00;
default: load <= 0;
endcase
end

// State machine to send data to TLV320 via SPI interface

always @ (posedge CLK_24MHZ)
begin
case (TLV)
4'd0: begin
	TLV_nCS <= 1'b1;				// set TLV320 CS high
	bit_count <= 4'd15;				// set starting bit count to 15
	index <= ~index;				// load next data to send	
	TLV <= TLV + 4'b1;
   end
4'd1: begin
	TLV_nCS <= 1'b0; 				// start data transfer with nCS low
	TLV_data <= tdata;
	data <= TLV_data[bit_count];	// set data up 
	TLV <= TLV + 4'b1;
	end
4'd2: begin
	TLV_CLK <= 1'b1; 				// clock data into TLV320
	TLV <= TLV + 4'b1;	
	end
4'd3: begin
	TLV_CLK <= 1'b0; 				// reset clock  
	TLV <= TLV + 4'b1;	
	end
4'd4: begin
		if(bit_count == 0) begin	// word transfer is complete, check for any more
			index <= ~index;
			TLV <= 4'd5; 
		end
		else begin
			bit_count <= bit_count - 1'b1;
			TLV <= 4'b1; 			// go round again 				
		end
	end								// end transfer
4'd5: begin
		if (load == 7)begin				// stop when all data sent 
			TLV <= 4'd5;				// hang out here forever 
			TLV_nCS <= 1'b1; 			// set CS high
		end 
		else TLV <= 0; 					// else get next data 
	end
default: TLV <= 0;
endcase
end 


// set up pins 

assign YB7 = CLK_24MHZ;		
assign CBCLK = YA7;
assign CDIN = YC3;
assign YC2 = CDOUT;
assign CLRCIN = YA6; 		// LRCLK for TLV320
assign CLRCOUT = YA6;		// LRCLK for TLV320
assign CMCLK = clock_by_2; 	// 24.576MHz/2
assign CMODE = 1'b1;		// Set to 1 for SPI mode 
assign DFS0 = YB3; 		    // set AK speed
assign DFS1 = YC1; 			// set AK speed
assign EXP1 = YB3;			// AK speed test point
assign EXP2 = YC1;			// AK speed test point
assign EXP3 = 1'b0;
assign EXP4 = 1'b0;
assign HPF = 1'b1; 			// HPF in AK on
assign IPWM = YC6;
assign YB4 = LRCLK;
assign MCLK = clock_by_2; 	// 24.576MHz/2
assign nCS = TLV_nCS;
assign nRST = AK_reset;		// reset AK5394A on power up
assign YB1 = PTT;			// PTT from Janus 
assign QPWM = YA3;
assign YB6 = SCLK; 			// is actually BCLK
assign YB5 = SDOUT; 
assign SMODE1 = 1'b1; 		// Master mode, I2S
assign SMODE2 = 1'b1; 		// Master mode, I2S
assign SSCK = TLV_CLK;		// SPI clock on TLV320
assign MOSI = data; 		// SPI data to send to TLV320 
assign ZCAL = 1'b1;			// Calibrate AK from A/D inputs
assign C3 = !PTT; 			// Pass not PTT line through to Atlas C3



endmodule 


