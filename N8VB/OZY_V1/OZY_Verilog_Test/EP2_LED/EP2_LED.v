//  V1.1 August 7, 2006
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
	inout [15:0] FX2_FD;
	output SLWR;
	output SLRD;
	output SLOE;
	output PKEND;
	output [1:0] FIFO_ADR;
	output [7:0] LEDS;
	
	reg [15:0] Tx_register;
		
	wire EP2_has_data = FLAGA;
	wire EP6_has_room = FLAGC;
	
	reg SLOE;
	reg SLRD;
	reg SLWR;
	reg TXDEN;
	reg PKEND;
	
	assign FX2_FD[15:0] = (TXDEN) ? Tx_register[15:0] : 16'bZ;
	
	reg [1:0] FIFO_ADR;
	reg [7:0] HIGHBYTE;
	reg [7:0] LEDS;	
			
	reg [5:0] state;
	
	always @(negedge IFCLK)
		begin
			case (state)
				0:	begin
						PKEND <= 1'b1;
						TXDEN <= 1'b0;
						SLWR <= 1'b1;
						SLRD <= 1'b1;
						SLOE <= 1'b1;
						FIFO_ADR <= 2'b00; // select EP2
						state <= state + 1'b1;
					end
				// Wait 2 IFCLK for FIFO_ADR to stabilize 
				2: 	begin
						if(EP2_has_data)
							begin
								state <= state + 1'b1;
								SLOE <= 1'b0; //assert SLOE														
							end
						else
							state <= state;  // loop here until we have data 
					end	
				// Wait 2 IFCLK before we assert SLRD	
				4:	begin
						SLRD <= 1'b0; //assert SLRD
						LEDS[7:0] <= FX2_FD[7:0]; // read FD[16:0] here	
						HIGHBYTE <=  FX2_FD[15:8];												
						state<= state + 1'b1;
					end
				// Reset SLD and SLO and load Tx_register	
				5: 	begin
						SLRD <= 1'b1; // reset SLRD
						SLOE <= 1'b1; //reset SLOE	
						Tx_register[7:0] <= LEDS[7:0];
						Tx_register[15:8] <= HIGHBYTE;
						state<= state + 1'b1;
					end	
				// Check that EP6 has room and if so send Tx data					
				6: 	begin
						if (EP6_has_room)
							begin
								FIFO_ADR <= 2'b10; 	// select EP6
								TXDEN <= 1'b1; 		// put data on FD bus ******								
								state<= state + 1'b1;
							end
						else state <= 4; // FIFO full, go back to RX
					end
				// Wait 2 IFCLK for FIFO_ADR to stabilize, assert SLWR 
				// NOTE: seems OK with 2 waits, may need more.	
				8: begin // was 10/9
						SLWR <= 1'b0; // assert SLWR
						state<= state + 1'b1;
					end	
				// reset SLWR and SLEN, loop back to Rx again 							
				9: begin // was 11/10 
						SLWR <= 1'b1; // reset SLWR	
						TXDEN <= 1'b0; // tristate bus																		
						state<= 0; 
					end
				default:
					state <= state + 1'b1;
					
			endcase
		end			

endmodule
