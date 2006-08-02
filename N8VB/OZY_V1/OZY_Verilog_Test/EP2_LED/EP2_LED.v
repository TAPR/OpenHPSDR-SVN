//  V1.0 August 2, 2006
//
//  Copyright 2006  P. Covington, N8VB
//
//  HPSDR - High Performance Software Defined Radio
//
//  EP2 OUT test code 
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
//  Demonstrates SYNC Read from EP2 FIFO
//
module EP2_LED(FX2_CLK, IFCLK, FLAGA, FLAGB, FLAGC, FX2_FD, SLWR, SLRD, SLOE, PKEND, FIFO_ADR, LEDS);

	input FX2_CLK;
	input IFCLK;
	input FLAGA;
	input FLAGB;
	input FLAGC;
	input [15:0] FX2_FD;
	output SLWR;
	output SLRD;
	output SLOE;
	output PKEND;
	output [1:0] FIFO_ADR;
	output [7:0] LEDS;
	
	assign SLWR = 1'b1;
	assign PKEND = 1'b1;
	
	wire EP2_has_data = FLAGA;
	reg SLOE;
	reg SLRD;
	reg [1:0] FIFO_ADR;
	reg [7:0] LEDS;	
	
	reg [2:0] state;
	
	always @(posedge IFCLK)
		begin
			case (state)
				3'd0:
					begin
						SLRD <= 1'b1;
						SLOE <= 1'b1;
						FIFO_ADR <= 2'b00; // select EP2
						state <= 3'd1;
					end
				3'd1:
					begin
						state <= 3'd2; //delay 1 IFCLOCK CYCLE
									  // this is necessary at 48MHz
									  // to allow FIFO_ADR to settle
					end		
				3'd2:
					begin
						if(EP2_has_data)
							begin
								state <= 3'd3;
								SLOE <= 1'b0; //assert SLOE								
							end
						else
							state <= 3'd2; // should be 3'd0 if checking other FIFOs
					end	
				3'd3:
					begin
						SLRD <= 1'b0; //assert SLRD
						state <= 3'd4;
					end		
				3'd4:
					begin
						LEDS[7:0] <= FX2_FD[7:0]; // read FD[16:0] here						
						state <= 3'd5;
					end
				3'd5:
					begin
						SLRD <= 1'b1; // reset SLRD
						state <= 3'd6;
					end
				3'd6:
					begin
						SLOE <= 1'b1; //reset SLOE
						state <= 3'd0;
					end
			endcase
		end			

endmodule