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
//  19 August 2006 - Interface to Atlas pins changed - V1.0
//  21 August 2006 - Change to use I2C interface to TLV320AIC23B  -- spi code commented out w/ '//i2c' comments  (kd5tfd) 
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
   	output QPWM,
   	input  SCLK,
  	input  SDOUT,
   	output SMODE1,
   	output SMODE2,
   	input  SSCK,  // was output for spi interface 
   	input  MOSI,  // was output for spi interface 
   	output ZCAL,
   	input  C2,
	input  C3,
	input  C4,
	output C5,
	output C6,
	output C7,
	input  C8,
	input  C9,
	output C10,
	output C11,
	input  C12,
	input  C13,
	input  C14,
	inout  C15
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
//i2c reg TLV_nCS;



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

//i2c always @ (posedge index)		// why can't this just be 'always @(index)' ?? 
//i2c begin
//i2c load <= load + 3'b1;			// select next data word to send
//i2c case (load)
//i2c 3'd0: tdata <= 16'h1E00;		// data to load into TLV320
//i2c 3'd1: tdata <= 16'h1201;
//i2c 3'd2: tdata <= 16'h0815;
//i2c 3'd3: tdata <= 16'h0C00;
//i2c 3'd4: tdata <= 16'h0E02;
//i2c 3'd5: tdata <= 16'h1000;
//i2c 3'd6: tdata <= 16'h0A00;
//i2c default: load <= 0;
//i2c endcase
//i2c end

// State machine to send data to TLV320 via SPI interface

//i2c always @ (posedge CLK_24MHZ)
//i2c begin
//i2c case (TLV)
//i2c 4'd0: begin
//i2c         TLV_nCS <= 1'b1;                                // set TLV320 CS high
//i2c         bit_count <= 4'd15;                             // set starting bit count to 15
//i2c         index <= ~index;                                // load next data to send
//i2c         TLV <= TLV + 4'b1;
//i2c    end
//i2c 4'd1: begin
//i2c         TLV_nCS <= 1'b0;                                // start data transfer with nCS low
//i2c         TLV_data <= tdata;
//i2c         data <= TLV_data[bit_count];    // set data up
//i2c         TLV <= TLV + 4'b1;
//i2c         end
//i2c 4'd2: begin
//i2c         TLV_CLK <= 1'b1;                                // clock data into TLV320
//i2c         TLV <= TLV + 4'b1;
//i2c         end
//i2c 4'd3: begin
//i2c         TLV_CLK <= 1'b0;                                // reset clock
//i2c         TLV <= TLV + 4'b1;
//i2c         end
//i2c 4'd4: begin
//i2c                 if(bit_count == 0) begin        // word transfer is complete, check for any more
//i2c                         index <= ~index;
//i2c                         TLV <= 4'd5;
//i2c                 end
//i2c                 else begin
//i2c                         bit_count <= bit_count - 1'b1;
//i2c                         TLV <= 4'b1;                    // go round again
//i2c                 end
//i2c         end                                                             // end transfer
//i2c 4'd5: begin
//i2c                 if (load == 7)begin                             // stop when all data sent
//i2c                         TLV <= 4'd5;                            // hang out here forever
//i2c                         TLV_nCS <= 1'b1;                        // set CS high
//i2c                 end
//i2c                 else TLV <= 0;                                  // else get next data
//i2c         end
//i2c default: TLV <= 0;
//i2c endcase
//i2c end

// Atlas outputs
assign C5 = CLK_24MHZ;		
assign C6 = SCLK; 			// is actually BCLK
assign C7 = LRCLK;
assign C10 = SDOUT; 
assign C11 = CDOUT;
assign C15 = ~PTT; 			// sent not PTT 

// Atlas inputs
assign nRST = C2;			// reset AK5394A on power up
assign QPWM = C3;
assign IPWM = C4;
assign CBCLK = C8;
assign CLRCIN = C9; 		// LRCLK for TLV320
assign CLRCOUT = C9;		// LRCLK for TLV320
assign CDIN = C12;
assign DFS0 = C13; 		    // set AK speed
assign DFS1 = C14; 			// set AK speed

// AK5394A pins
assign MCLK = clock_by_2; 	// 24.576MHz/2
assign HPF = 1'b1; 			// HPF in AK on
assign SMODE1 = 1'b1; 		// Master mode, I2S
assign SMODE2 = 1'b1; 		// Master mode, I2S
assign ZCAL = 1'b1;			// Calibrate AK from A/D inputs

// LTV320 pins
assign CMCLK = clock_by_2; 	// 24.576MHz/2
//i2c assign CMODE = 1'b1;		// Set to 1 for SPI mode 
assign CMODE = 1'b0;		// Set to 0 for I2C
//i2c assign nCS = TLV_nCS;
assign nCS = 1'b0;   // this results in an i2c addr of 0x1a 
//i2c assign SSCK = TLV_CLK;		// SPI clock on TLV320
//i2c assign MOSI = data; 		    // SPI data to send to TLV320 

// EXTRA pins
assign EXP1 = 1'b0;			
assign EXP2 = 1'b0;			
assign EXP3 = 1'b0;
assign EXP4 = 1'b0;

endmodule 
