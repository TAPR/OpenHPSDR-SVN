//  V1.0 August 9, 2006
//
//  Copyright 2006  P. Covington, N8VB
//
//  HPSDR - High Performance Software Defined Radio
//
//  OZY-JANUSV2 
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
//  Streams data from AK5394 to EP6
//

module OZY_JANUSV2(
	FX2_CLK,
	IFCLK,
	FD,
	SLWR,
	SLRD,
	SLOE,
	FIFO_ADR,
	GPIO,
	FLAGA,
	FLAGC,
	C14, // LRCLK
	C15, // SDOUT
	C16, // SCLK
	C13, // DFS0
	C19, // DFS1
	C11, // PTT
	C17, // CLK_24MHz,
	GPIO9,
	GPIO10,
	C20, // CDOUT
	C21, // CDIN
	C9, // CBCLK
	C8, // CLRCIN
	C5, // QPWM
	C24 // IPWM
);

	input FX2_CLK;
	input IFCLK;
	output [15:0] FD;
	output SLWR;
	output SLRD;
	output SLOE;
	output [1:0] FIFO_ADR;
	output [8:1] GPIO;
	input FLAGA;
	input FLAGC;
	input C14; // LRCLK
	input C15; // SDOUT
	input C16; // SCLK
	output C13; // DFS0
	output C19; // DFS1
	input C11; // PTT
	input C17; // CLK_24MHz,
	output GPIO9;
	output GPIO10;
	input C20; // CDOUT
	output C21; // CDIN
	output C9; // CBCLK
	output C8; // CLRCIN
	output C5; // QPWM
	output C24; // IPWM

	reg TXDEN; // tx data enable, places tx_register on FD[15:0]
	reg [15:0] lr_data;
	reg SLRD;
	reg SLWR;
	reg SLOE;
	reg [1:0] FIFO_ADR;
			
	//reg [7:0] LEDS;
	reg [7:0] st;
	reg [7:0] count;
		
	wire SCLK;
	wire FSYNC;
	wire LRCLK;
	wire SDOUT;	
	
	assign SCLK = C16;
	assign LRCLK = C14;
	assign SDOUT = C15;
	assign C21 = 1'b0; // CDIN
	assign C9 = 1'b0; // CBCLK
	assign C8 = 1'b0; // CLRCIN
	assign C5 = 1'b0; // QPWM
	assign C24 = 1'b0; // IPWM
	
	assign C13 = 1'b0;  //SELECT 192 kHz DFS0
	assign C19 = 1'b1;  //SELECT 192 kHz DFS1
	
	assign GPIO9 = LRCLK;
	assign GPIO10 = SDOUT;
	
	assign FD[15:0] = tx_bus_16[15:0];
		
	// the following will only work at 96 and 192 kHz		
	always @(posedge SCLK)
		begin			
			case (st)				
				0: begin
					count <= 0;
					txfifoclr <= 1'b1;
					writefifo <= 1'b0;
					if (LRCLK)
						st <= 1;
					else
						st <= 0;
					end
				1: begin
					txfifoclr <= 1'b0;
					writefifo <= 1'b0;
					if (~LRCLK) begin  // look for left start
						count <= 0;
						st <= 2;
						end
					else
						st <= 1;
					end
				2: begin // left state word 1
					writefifo <= 1'b0;
					count <= count + 1;					
					// clock ldata here
					lr_data[15:0] <= {lr_data[14:0], SDOUT};					
					if (count == 16)
						begin
							writefifo <= 1'b1;
							count <= 0;
							st <= 3;	
						end
					else
						begin
							st <= 2;
						end						
					end
				3:	begin // left state word 2
					writefifo <= 1'b0;
					count <= count + 1;
					// clock ldata here
					lr_data[15:0] <= {lr_data[14:0], SDOUT};					
					if (count == 16)
						begin
							writefifo <= 1'b1;
							count <= 0;
							st <= 4;	
						end
					else
						begin
							st <= 3;
						end						
					end				
				4: begin // right state word 1
					writefifo <= 1'b0;
					count <= count + 1;
					// clock rdata here
					lr_data[15:0] <= {lr_data[14:0], SDOUT};					
					if (count == 16)
						begin
							writefifo <= 1'b1;
							count <= 0;
							st <= 5;	
						end
					else
						begin
							st <= 4;
						end						
					end
				5: begin // right state word 2
					writefifo <= 1'b0;
					count <= count + 1;
					// clock rdata here
					lr_data[15:0] <= {lr_data[14:0], SDOUT};					
					if (count == 16)
						begin
							writefifo <= 1'b1;
							count <= 0;
							st <= 2;	
						end
					else
						begin
							st <= 5;
						end						
					end								
				default:
					begin
						st <= 0;
						count <= 0;					
					end			
			endcase	
		end
		
		reg txfifoclr;
		reg writefifo;
		reg readfifo;
		wire readfifoempty;
		wire readfifofull;
		wire writefifoempty;
		wire writefifofull;
		wire [15:0] tx_bus_16;		
						
		tx_fifo tx_fifo(.aclr(txfifoclr), .wrclk(SCLK), 
			.rdreq(readfifo), .rdclk(IFCLK), .wrreq(writefifo),
			.data(lr_data), .rdempty(readfifoempty), .wrempty(writefifoempty), 
			.wrfull(writefifofull),	.q(tx_bus_16), .wrusedw(), .rdusedw());	
		
		reg [7:0] fx2st;
			
		always @(negedge IFCLK)
			begin
				case (fx2st)
					8'd0:	begin
							SLWR <= 1'b0;
							readfifo <= 1'b0;
							SLRD <= 1'b1;
							SLOE <= 1'b1;
							TXDEN <= 1'b0;
							FIFO_ADR <= 2'b10;
							TXDEN <= 1'b1;
							fx2st <= 8'd1;
							end
					8'd1:	begin
							TXDEN <= 1'b1;
							fx2st <= 8'd2;
							end
					8'd2:	begin
							if ((~readfifoempty) & FLAGC)
								begin									
									readfifo <= 1'b1;
									SLWR <= 1'b0;
									fx2st <= 8'd2;
								end
							else
								begin
									SLWR <= 1'b1;
									readfifo <= 1'b0;
									fx2st <= 8'd5;
								end
							end					
					8'd5:	begin
								fx2st <= 8'd2;
							end
					default:
						begin
							fx2st <= 8'd0;
						end
				endcase
			end
		
		

endmodule