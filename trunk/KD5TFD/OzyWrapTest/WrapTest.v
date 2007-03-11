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



reg[1:0] loopback_reset; 
wire[1:0] loopback_result; 


assign DEBUG_LED2 = ~loopback_result[0];
assign DEBUG_LED3 = ~loopback_result[1];


// instantiate two loopback checkers 

loopBackCheck loopBackA02C02( .ref_sig_i(ATLAS_A02), 
                              .check_sig_i(ATLAS_C02), 
					     	  .not_ok_o(loopback_result[0]),
							  .master_clock_i(FX2_CLK), 
							  .reset_i(loopback_reset[0])
						     );
							
loopBackCheck loopBackA10C10( .ref_sig_i(ATLAS_A10), 
                              .check_sig_i(ATLAS_C10), 
							  .not_ok_o(loopback_result[1]),
							  .master_clock_i(FX2_CLK), 
							  .reset_i(loopback_reset[1])
							);							




// baudclock generator -- generate 115200 baud clock from 12e6 fx2 clock 
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
		if ( clock_count == 12000000 ) begin
			clock_count <= 0; 
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

// get a hex char from a nibble 							
function [7:0] NibbleToChar; 
	input [3:0] nibble; 
	begin 
		if ( nibble < 10 ) begin 
			NibbleToChar = 48 + nibble;  // 48 = '0' 
		end 
		else begin 
			NibbleToChar = 97 - 10 + nibble;  // 97 = 'a' 
		end 
	end 
endfunction 


reg [7:0] test_circ_idx; 
reg [4:0] test_loop_state; 
reg [4:0] post_xmit_state; 
reg [15:0] idle_counter; 
reg [7:0] char_to_xmit; 
reg [15:0] rs232_delay_count; 
reg [7:0] pass_count; 

//  state machine for the loopback tester 

always @ ( posedge FX2_CLK ) 
begin 
	case ( test_loop_state ) 
		0: begin  // init -- reset all loopbacks 
			loopback_reset[0] <= 1; 
			loopback_reset[1] <= 1; 
			test_circ_idx <= 0; 
			test_loop_state <= 1; 
		end
		
		1: begin  // come out of init - go into idling state 
			loopback_reset[0] <= 0; 
			loopback_reset[1] <= 0; 
			test_loop_state <= 2; 
			idle_counter <= 0; 
		end 
		
		2: begin 
			if ( idle_counter >= 50000 ) begin // idle state 
				idle_counter <= 0;  // prep for next time 
				test_loop_state <= 3; // get out of idle state 		
			end 
			else begin  // count not met yet -- stay here 
				idle_counter <= idle_counter + 1; 
				test_loop_state <= 2; 
			end 		
		end 
		
		3:  begin // coming out of idle state - check the circ and select next state appropriately 
			if ( loopback_result[test_circ_idx] == 1 ) begin  //  failed 				
				test_loop_state <= 4; 
			end 
			else begin // passed 
				// test_loop_state <= 5; // use this to print passes on individual circs 
				test_loop_state <= 13;   // use this to not print passes on ind circs 
				pass_count <= pass_count + 1; 
			end 
		end 
		
		4: begin  // circuit failed 
			char_to_xmit = 70;  // 'F'
			test_loop_state <= 6; 
		end 
		
		5: begin  // circuit passed 
			char_to_xmit = 80;  // 'P'
			test_loop_state <= 6; 
		end 
		
		6: begin // xmit pass fail message 
			post_xmit_state <= 7; 
			test_loop_state <= 8; 
		end 
		
		7: begin // pass fail char completed printing - print a space 
			char_to_xmit <= 32;  // 32 == ' ' 
			post_xmit_state <= 9; 
			test_loop_state <= 8; 
		end 
		
		8: begin // xmit a char over serial port 
			rs232_xmit_reg <= char_to_xmit; 
			rs232_write_strobe <= 1; 
			test_loop_state <= 10; 
		end 
		
		9: begin  // space transmitted -- xmit circ number  -- hi nibble first 
			char_to_xmit = NibbleToChar(test_circ_idx[7:4]); 
			post_xmit_state <= 11; 
			rs232_write_strobe <= 0; 
			test_loop_state <= 8;  
		end 
		
		10: begin  // char to xmit strobed - unstrobe and on to next state 
			rs232_write_strobe <= 0;
			test_loop_state <= 15; 
			rs232_delay_count <= 10000;  
		end 
		
		11: begin // send low nibble of circ number 
			char_to_xmit = NibbleToChar(test_circ_idx[3:0]); 
			post_xmit_state <= 12;  // send line feed 
			test_loop_state <= 8;  			
		end 
		
		12: begin  // send lf char 
			char_to_xmit = 10;   // 10 == line feed 
			post_xmit_state <= 13; 
			test_loop_state <= 8;  // send char 
		end 
		
		13: begin  // pass fail message has been sent, reset this circuit, increment circ_idx and  back to idle 
			loopback_reset[test_circ_idx] <= 1; 
			test_loop_state <= 14; 				      
		end 
		
		14: begin  // all done printing mesaage - see if we're at end of loop and if all passed print msg - else go on to next circ 
			loopback_reset[test_circ_idx] <= 0; 
			if ( test_circ_idx == 1 ) begin  // are we at end of list?? reset to 0
				test_circ_idx <= 0;  
				if ( pass_count == 2 ) begin  // did all circuits pass? 
					test_loop_state <= 16;  // go print 'All Pass!" message 
				end 
				else begin
					test_loop_state <= 2; // we had fails on this loop - just go back to the top 
				end
				pass_count <= 0; 
			end 
			else begin 
				test_circ_idx <= test_circ_idx + 1; 
				test_loop_state <= 2; 
			end 
		end 							
		
		15: begin  // post rs232 xmit delay 
			if ( rs232_delay_count == 0 ) begin
				test_loop_state <= post_xmit_state; 
			end 
			else begin 
				rs232_delay_count <= rs232_delay_count - 1; 
				test_loop_state <= 15;  // stay here 
			end 
		end	
		
		16: begin  // print "All Pass!" message 
			char_to_xmit <= 65; // A
			test_loop_state <= 8; 
			post_xmit_state <= 17; 
		end 
		
		17: begin  // print "All Pass!" message 
			char_to_xmit <= 108; // l
			test_loop_state <= 8; 
			post_xmit_state <= 18; 
		end 
		
		18: begin  // print "All Pass!" message 
			char_to_xmit <= 108; // l
			test_loop_state <= 8; 
			post_xmit_state <= 19; 
		end 
		
		19: begin  // print "All Pass!" message 
			char_to_xmit <= 32; // ' ' 
			test_loop_state <= 8; 
			post_xmit_state <= 20; 
		end 
		
		20: begin  // print "All Pass!" message 
			char_to_xmit <= 80; // P
			test_loop_state <= 8; 
			post_xmit_state <= 21; 
		end 
		
		21: begin  // print "All Pass!" message 
			char_to_xmit <= 97; // a
			test_loop_state <= 8; 
			post_xmit_state <= 22; 
		end 		
		
		22: begin  // print "All Pass!" message 
			char_to_xmit <= 115; // s
			test_loop_state <= 8; 
			post_xmit_state <= 23; 
		end 				
		
		23: begin  // print "All Pass!" message 
			char_to_xmit <= 115; // s
			test_loop_state <= 8; 
			post_xmit_state <= 24; 
		end 				
		
		24: begin  // print "All Pass!" message 
			char_to_xmit <= 33; // !
			test_loop_state <= 8; 
			post_xmit_state <= 25; 
		end 				
		
		25: begin  // print "All Pass!" message 
			char_to_xmit <= 10; // LF
			test_loop_state <= 8; 
			post_xmit_state <= 26; 
		end 					
		
		26: begin  // "All Pass!" message printed - reset loop and restart 
			test_loop_state <= 2;  
		end 					
		
		
	endcase 
	
end // module 




// reg[24:0] serial_clock_count; 
//
// always @ (posedge FX2_CLK ) begin 
//	if ( serial_clock_count == 12000000) begin // xmit a char 
//		rs232_write_strobe <= 1; 
//		serial_clock_count <= 0; 	
//	end 	
//	else begin // count is
//		serial_clock_count <= serial_clock_count + 1; 
//	end 
//	
//	if ( serial_clock_count == 0 ) begin 
//		rs232_write_strobe <= 0; 
//		if ( rs232_xmit_reg == 0 || rs232_xmit_reg == 122 ) begin // 122 = 'z' 
//			rs232_xmit_reg <= 97;  // 97 = 'a' 
//		end 
//		else begin 
//			rs232_xmit_reg <= rs232_xmit_reg + 1; 
//		end 
//	end 	
// end 

endmodule 
