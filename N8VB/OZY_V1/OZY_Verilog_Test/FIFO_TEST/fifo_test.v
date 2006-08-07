//  V1.0 August 4, 2006
//
//  Copyright 2006  P. Covington, N8VB
//
//  HPSDR - High Performance Software Defined Radio
//
//  EP6 IN test code 
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
//  Sends counter data to EP6
//
module fifo_test(FX2_CLK, IFCLK, FLAGA, FLAGB, FLAGC, FX2_FD, SLWR, SLRD, SLOE, PKEND, FIFO_ADR, LEDS);

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
	reg SLEN;
	reg PKEND;
	
	assign FX2_FD[15:0] = (SLEN) ? Tx_register[15:0] : 16'bZZZZZZZZZZZZZZZZ;
	assign LEDS[0] = FLAGA;
	assign LEDS[1] = FLAGB;
	assign LEDS[2] = FLAGC;
	assign LEDS[7:3] = 5'b11111;
	
	reg [1:0] FIFO_ADR;
	reg [7:0] HIGHBYTE;
		
	reg [15:0] counter;
	reg [3:0] state;

/*	
	// EP6 write state machine
	always @(posedge IFCLK)
		begin
			counter <= counter + 16'd1;
			Tx_register[15:0] <= counter[15:0];
			case (state)
				4'd0:
					begin
						PKEND <= 1'b1;
						SLEN <= 1'b0;
						SLWR <= 1'b1;
						SLRD <= 1'b1;
						SLOE <= 1'b1;
						FIFO_ADR <= 2'b10; // select EP6
						state <= 4'd1;
					end
				4'd1:
					begin
						state <= 4'd2; //delay 1 IFCLOCK CYCLE
									  // this is necessary at 48MHz
									  // to allow FIFO_ADR to settle
					end				
				4'd2:
					begin
						if (EP6_has_room)
							begin
								SLEN <= 1'b1; // drive bus
								SLWR <= 1'b0; // assert SLWR
								state <= 4'd5;	
							end
						else
							begin
								state <= 4'd2; // FIFO full, go back to RX
							end
					end					
				4'd3: 
					begin						
						state <= 4'd4; // let data bus stabilize
					end
				4'd4:
					begin
						SLWR <= 1'b0; // assert SLWR
						state <= 4'd5;
					end							
				4'd5:
					begin
						SLWR <= 1'b1; // reset SLWR	
						SLEN <= 1'b0;					
						state <= 4'd2; // wait state for SLWR
					end
				4'd6: 
					begin
						state <= 4'd7;
					end
				4'd7:
					begin
						SLEN <= 1'b0;
						state <= 4'd2;
					end
				default:
					state <= 4'd0;
					
			endcase
		end	*/
		
		// EP6 burst write state machine
		always @(posedge IFCLK)
		begin
			counter <= counter + 16'd1;
			Tx_register[15:0] <= counter[15:0];
			case (state)
				4'd0:
					begin
						PKEND <= 1'b1;
						SLEN <= 1'b0;
						SLWR <= 1'b1;
						SLRD <= 1'b1;
						SLOE <= 1'b1;
						FIFO_ADR <= 2'b10; // select EP6
						state <= 4'd1;
					end
				4'd1:
					begin
						state <= 4'd2; //delay 1 IFCLOCK CYCLE
									  // this is necessary at 48MHz
									  // to allow FIFO_ADR to settle
					end				
				4'd2:
					begin
						if (EP6_has_room)
							begin
								SLEN <= 1'b1; // drive bus
								SLWR <= 1'b0; // assert SLWR
								state <= 4'd5;	
							end
						else
							begin
								state <= 4'd2; // FIFO full, go back to RX
							end
					end					
				4'd3: 
					begin						
						state <= 4'd4; // let data bus stabilize
					end
				4'd4:
					begin
						SLWR <= 1'b0; // assert SLWR
						state <= 4'd5;
					end							
				4'd5:
					begin
						if (EP6_has_room)
							begin
								state <= 4'd5;
							end
						else
							begin
								SLWR <= 1'b1; // reset SLWR	
								SLEN <= 1'b0;					
								state <= 4'd2; // wait state for SLWR
							end
					end
				4'd6: 
					begin
						state <= 4'd7;
					end
				4'd7:
					begin
						SLEN <= 1'b0;
						state <= 4'd2;
					end
				default:
					state <= 4'd0;
					
			endcase
		end		

endmodule