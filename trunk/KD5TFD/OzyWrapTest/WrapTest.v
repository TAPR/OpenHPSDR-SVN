// WrapTest.v - Ozy wrap plug test 
// 
// Copyright 2007  Bill Tracey KD5TFD (bill@ewjt.com)
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
module WrapTest(
        IFCLK, FX2_FD, FLAGA, FLAGB, FLAGC, SLWR, SLRD, SLOE, PKEND, FIFO_ADR, 
 	    DEBUG_LED0, DEBUG_LED1, DEBUG_LED2,DEBUG_LED3, 	
		FX2_CLK, SPI_SCK, SPI_SI, SPI_SO, SPI_CS, GPIO, GPIO_nIOE,
		ATLAS_A02, ATLAS_A10, ATLAS_C02, ATLAS_C10, ATLAS_A06, ATLAS_A04, 
		FPGA_TXD
		);
		
		
input IFCLK;                   // FX2 IFCLOCK - 48MHz
inout  [15:0] FX2_FD;           // bidirectional FIFO data to/from the FX2

input FLAGA;
input FLAGB;
input FLAGC;
output SLWR;
output SLRD;
output SLOE;
output PKEND;
output [1:0] FIFO_ADR;
		
		
output DEBUG_LED0;               // LEDs on OZY board
output DEBUG_LED1;
output DEBUG_LED2;
output DEBUG_LED3;		
		
		
		

// interface lines for GPIO control 
input 				FX2_CLK;		// master system clock from FX2 
input 				SPI_SCK;		// SPI SCK from FX2
input 				SPI_SI;			// SPI serial in from FX2
inout 				SPI_SO;			// SPI serial out to FX2
input 				SPI_CS;         // FPGA chip select from FX2
inout [23:0]		GPIO;			// OZY GPIO lines
output 				GPIO_nIOE;      // enable GPIO driver chips 

output ATLAS_A02; 
output ATLAS_A10; 
input ATLAS_C02; 
input ATLAS_C10; 

output ATLAS_A06; 
output ATLAS_A04; 

output FPGA_TXD;  // serial port out line 

reg[1:0] leds; 
reg[24:0] clock_count; 

assign DEBUG_LED0 = ~leds[0]; 		
assign DEBUG_LED1 = ~leds[1]; 		
// assign DEBUG_LED2 = ~leds[2]; 		
// assign DEBUG_LED3 = ~leds[3]; 		

assign ATLAS_A04 = FX2_CLK; 

wire ATLAS_A02 = clock_count[2]; 
wire ATLAS_A10 = clock_count[3];  

wire result_A02C02; 
wire result_A10C10; 

assign DEBUG_LED2 = ~result_A02C02;
assign DEBUG_LED3 = ~result_A10C10;

reg loopback_reset;  

// instantiate two loopback checkers 

loopBackCheck loopBackA02C02( .ref_sig_i(ATLAS_A02), 
                              .check_sig_i(ATLAS_C02), 
					     	  .not_ok_o(result_A02C02),
							  .master_clock_i(FX2_CLK), 
							  .reset_i(loopback_reset)
						     );
							
loopBackCheck loopBackA10C10( .ref_sig_i(ATLAS_A10), 
                              .check_sig_i(ATLAS_C10), 
							  .not_ok_o(result_A10C10),
							  .master_clock_i(FX2_CLK), 
							  .reset_i(loopback_reset)
							);							


// baudclock 
reg[16:0] baud_reg; 
always @ (posedge FX2_CLK) begin
	baud_reg <= baud_reg[15:0] + 629; 
end 

assign ATLAS_A06 = baud_reg[16]; 

always @ (posedge FX2_CLK) begin 
	if ( leds == 0 ) begin 
		leds <= 1; 
	end 
	else begin 
		if ( clock_count != 12000000 ) begin 
			loopback_reset <= 0; 
		end 
		if ( clock_count == 12000000 ) begin
			clock_count <= 0; 
			loopback_reset <= 1; 
			if ( leds[1:0] == 2'b10 ) begin 
				leds <= 1; 
			end 
			else begin 
				leds <= leds << 1; 
			end 
		end 		
		else begin 
			clock_count <= clock_count + 1; 
		end 
	end 
end 

// instantiate our rs232 xmitter 
reg rs232_write_strobe; 
wire rs232_space_avail; 
reg[7:0] rs232_xmit_reg; 
rs232_xmit Ozy_rs232_xmit( .data_i(rs232_xmit_reg), 
				           .clk_i(FX2_CLK),
				           .space_avail_o(rs232_space_avail),
				           .write_req_i(rs232_write_strobe), 
				           .baud_clock_i(baud_reg[16]), 
				           .xmit_o(FPGA_TXD)
				         ); 


reg[24:0] serial_clock_count; 

always @ (posedge FX2_CLK ) begin 
	if ( serial_clock_count == 12000000) begin // xmit a char 
		rs232_write_strobe <= 1; 
		serial_clock_count <= 0; 	
	end 	
	else begin // count is
		serial_clock_count <= serial_clock_count + 1; 
	end 
	
	if ( serial_clock_count == 0 ) begin 
		rs232_write_strobe <= 0; 
		if ( rs232_xmit_reg == 0 || rs232_xmit_reg == 122 ) begin // 122 = 'z' 
			rs232_xmit_reg <= 97;  // 97 = 'a' 
		end 
		else begin 
			rs232_xmit_reg <= rs232_xmit_reg + 1; 
		end 
	end 	
end 


endmodule 
