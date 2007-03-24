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
		ATLAS_A02, ATLAS_A03, ATLAS_A04, ATLAS_A05, ATLAS_A06, ATLAS_A07, ATLAS_A08, ATLAS_A09,
        ATLAS_A10, ATLAS_A11, ATLAS_A12, ATLAS_A13, ATLAS_A14, ATLAS_A15, ATLAS_A16, ATLAS_A17,
        ATLAS_A18, ATLAS_A19, ATLAS_A22, ATLAS_A23, ATLAS_A24, ATLAS_A25, ATLAS_C02, ATLAS_C03,
        ATLAS_C04, ATLAS_C05, ATLAS_C06, ATLAS_C07, ATLAS_C08, ATLAS_C09, ATLAS_C10, ATLAS_C11,
        ATLAS_C12, ATLAS_C13, ATLAS_C14, ATLAS_C15, ATLAS_C16, ATLAS_C17, ATLAS_C18, ATLAS_C19,
        ATLAS_C22, ATLAS_C23, ATLAS_C24, ATLAS_C25,
	    ATLAS_C20, ATLAS_C21, ATLAS_A27, ATLAS_A29, ATLAS_A31, ATLAS_C27, ATLAS_C29, ATLAS_C31,
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
output ATLAS_A03;
output ATLAS_A04;
output ATLAS_A05;
output ATLAS_A06;
output ATLAS_A07;
output ATLAS_A08;
output ATLAS_A09;
output ATLAS_A10;
output ATLAS_A11;
output ATLAS_A12;
output ATLAS_A13;
output ATLAS_A14;
output ATLAS_A15;
output ATLAS_A16;
output ATLAS_A17;
output ATLAS_A18;
output ATLAS_A19;
output ATLAS_A22;
output ATLAS_A23;
output ATLAS_A24;
output ATLAS_A25;


input ATLAS_C02;
input ATLAS_C03;
input ATLAS_C04;
input ATLAS_C05;
input ATLAS_C06;
input ATLAS_C07;
input ATLAS_C08;
input ATLAS_C09;
input ATLAS_C10;
input ATLAS_C11;
input ATLAS_C12;
input ATLAS_C13;
input ATLAS_C14;
input ATLAS_C15;
input ATLAS_C16;
input ATLAS_C17;
input ATLAS_C18;
input ATLAS_C19;
input ATLAS_C22;
input ATLAS_C23;
input ATLAS_C24;
input ATLAS_C25;


input ATLAS_C20;
input ATLAS_C21;
input ATLAS_A27;
input ATLAS_A29;
input ATLAS_A31;
input ATLAS_C27;
input ATLAS_C29;
input ATLAS_C31;




output FPGA_TXD;  // serial port out line 

reg[1:0] leds; 
reg[24:0] clock_count; 

assign DEBUG_LED0 = ~leds[0]; 		
assign DEBUG_LED1 = ~leds[1]; 		
assign GPIO_nIOE = 0;  // enable GPIO output driver 
	 

wire ATLAS_A02 = clock_count[00];   // 6
wire ATLAS_A03 = clock_count[01];   // 3
wire ATLAS_A04 = clock_count[02];   // 1.5
wire ATLAS_A05 = clock_count[03];   // 750
wire ATLAS_A06 = clock_count[04];   // 375
wire ATLAS_A07 = clock_count[05];   // 187.5
wire ATLAS_A08 = clock_count[06];   // 93.75
wire ATLAS_A09 = ~clock_count[00];
wire ATLAS_A10 = ~clock_count[01];
wire ATLAS_A11 = ~clock_count[02];
wire ATLAS_A12 = ~clock_count[03];
wire ATLAS_A13 = ~clock_count[04];
wire ATLAS_A14 = ~clock_count[05];
wire ATLAS_A15 = ~clock_count[06];
wire ATLAS_A16 = clock_count[00];
wire ATLAS_A17 = clock_count[01];
wire ATLAS_A18 = clock_count[02];
wire ATLAS_A19 = clock_count[03];

wire ATLAS_A22 = clock_count[00];
wire ATLAS_A23 = clock_count[01];
wire ATLAS_A24 = clock_count[02];
wire ATLAS_A25 = clock_count[03];



// note -- these are 0 indexed -- they are 1 indexed on the schematics 
assign GPIO[00] = ~clock_count[04];
assign GPIO[09] = ~clock_count[05];
assign GPIO[10] = clock_count[02];
assign GPIO[11] = clock_count[03];
assign GPIO[12] = clock_count[04];
assign GPIO[13] = clock_count[05];
assign GPIO[14] = clock_count[06];
assign GPIO[15] = ~clock_count[05];
assign GPIO[01] = ~clock_count[01];
assign GPIO[02] = ~clock_count[02];
assign GPIO[03] = ~clock_count[03];
assign GPIO[04] = ~clock_count[04];
assign GPIO[05] = ~clock_count[05];
assign GPIO[06] = ~clock_count[06];
assign GPIO[07] = clock_count[00];
assign GPIO[08] = clock_count[01];



reg[37:0] loopback_reset; 
wire[37:0] loopback_result; 

assign DEBUG_LED2 = ~loopback_result[0];
assign DEBUG_LED3 = ~loopback_result[1];



loopBackCheck loopBackA02C02( .ref_sig_i(ATLAS_A02), .check_sig_i(ATLAS_C02), .not_ok_o(loopback_result[00]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[00]) );
loopBackCheck loopBackA03C03( .ref_sig_i(ATLAS_A03), .check_sig_i(ATLAS_C03), .not_ok_o(loopback_result[01]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[01]) );
loopBackCheck loopBackA04C04( .ref_sig_i(ATLAS_A04), .check_sig_i(ATLAS_C04), .not_ok_o(loopback_result[02]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[02]) );
loopBackCheck loopBackA05C05( .ref_sig_i(ATLAS_A05), .check_sig_i(ATLAS_C05), .not_ok_o(loopback_result[03]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[03]) );
loopBackCheck loopBackA06C06( .ref_sig_i(ATLAS_A06), .check_sig_i(ATLAS_C06), .not_ok_o(loopback_result[04]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[04]) );
loopBackCheck loopBackA07C07( .ref_sig_i(ATLAS_A07), .check_sig_i(ATLAS_C07), .not_ok_o(loopback_result[05]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[05]) );
loopBackCheck loopBackA08C08( .ref_sig_i(ATLAS_A08), .check_sig_i(ATLAS_C08), .not_ok_o(loopback_result[06]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[06]) );
loopBackCheck loopBackA09C09( .ref_sig_i(ATLAS_A09), .check_sig_i(ATLAS_C09), .not_ok_o(loopback_result[07]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[07]) );
loopBackCheck loopBackA10C10( .ref_sig_i(ATLAS_A10), .check_sig_i(ATLAS_C10), .not_ok_o(loopback_result[08]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[08]) );
loopBackCheck loopBackA11C11( .ref_sig_i(ATLAS_A11), .check_sig_i(ATLAS_C11), .not_ok_o(loopback_result[09]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[09]) );
loopBackCheck loopBackA12C12( .ref_sig_i(ATLAS_A12), .check_sig_i(ATLAS_C12), .not_ok_o(loopback_result[10]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[10]) );
loopBackCheck loopBackA13C13( .ref_sig_i(ATLAS_A13), .check_sig_i(ATLAS_C13), .not_ok_o(loopback_result[11]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[11]) );
loopBackCheck loopBackA14C14( .ref_sig_i(ATLAS_A14), .check_sig_i(ATLAS_C14), .not_ok_o(loopback_result[12]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[12]) );
loopBackCheck loopBackA15C15( .ref_sig_i(ATLAS_A15), .check_sig_i(ATLAS_C15), .not_ok_o(loopback_result[13]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[13]) );
loopBackCheck loopBackA16C16( .ref_sig_i(ATLAS_A16), .check_sig_i(ATLAS_C16), .not_ok_o(loopback_result[14]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[14]) );
loopBackCheck loopBackA17C17( .ref_sig_i(ATLAS_A17), .check_sig_i(ATLAS_C17), .not_ok_o(loopback_result[15]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[15]) );
loopBackCheck loopBackA18C18( .ref_sig_i(ATLAS_A18), .check_sig_i(ATLAS_C18), .not_ok_o(loopback_result[16]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[16]) );
loopBackCheck loopBackA19C19( .ref_sig_i(ATLAS_A19), .check_sig_i(ATLAS_C19), .not_ok_o(loopback_result[17]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[17]) );

loopBackCheck loopBackA22C22( .ref_sig_i(ATLAS_A22), .check_sig_i(ATLAS_C22), .not_ok_o(loopback_result[18]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[18]) );
loopBackCheck loopBackA23C23( .ref_sig_i(ATLAS_A23), .check_sig_i(ATLAS_C23), .not_ok_o(loopback_result[19]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[19]) );
loopBackCheck loopBackA24C24( .ref_sig_i(ATLAS_A24), .check_sig_i(ATLAS_C24), .not_ok_o(loopback_result[20]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[20]) );
loopBackCheck loopBackA25C25( .ref_sig_i(ATLAS_A25), .check_sig_i(ATLAS_C25), .not_ok_o(loopback_result[21]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[21]) );




loopBackCheck loopBackGPIO01GPIO017( .ref_sig_i(GPIO[00]), .check_sig_i(GPIO[16]), .not_ok_o(loopback_result[22]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[22]) );
loopBackCheck loopBackGPIO10GPIO018( .ref_sig_i(GPIO[09]), .check_sig_i(GPIO[17]), .not_ok_o(loopback_result[23]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[23]) );
loopBackCheck loopBackGPIO11GPIO019( .ref_sig_i(GPIO[10]), .check_sig_i(GPIO[18]), .not_ok_o(loopback_result[24]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[24]) );
loopBackCheck loopBackGPIO12GPIO020( .ref_sig_i(GPIO[11]), .check_sig_i(GPIO[19]), .not_ok_o(loopback_result[25]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[25]) );
loopBackCheck loopBackGPIO13GPIO021( .ref_sig_i(GPIO[12]), .check_sig_i(GPIO[20]), .not_ok_o(loopback_result[26]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[26]) );
loopBackCheck loopBackGPIO14GPIO022( .ref_sig_i(GPIO[13]), .check_sig_i(GPIO[21]), .not_ok_o(loopback_result[27]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[27]) );
loopBackCheck loopBackGPIO15GPIO023( .ref_sig_i(GPIO[14]), .check_sig_i(GPIO[22]), .not_ok_o(loopback_result[28]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[28]) );
loopBackCheck loopBackGPIO16GPIO024( .ref_sig_i(GPIO[15]), .check_sig_i(GPIO[23]), .not_ok_o(loopback_result[29]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[29]) );

loopBackCheck loopBackGPIO02ATLASC20( .ref_sig_i(GPIO[01]), .check_sig_i(ATLAS_C20), .not_ok_o(loopback_result[30]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[30]) );
loopBackCheck loopBackGPIO03ATLASC21( .ref_sig_i(GPIO[02]), .check_sig_i(ATLAS_C21), .not_ok_o(loopback_result[31]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[31]) );
loopBackCheck loopBackGPIO04ATLASA27( .ref_sig_i(GPIO[03]), .check_sig_i(ATLAS_A27), .not_ok_o(loopback_result[32]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[32]) );
loopBackCheck loopBackGPIO05ATLASA29( .ref_sig_i(GPIO[04]), .check_sig_i(ATLAS_A29), .not_ok_o(loopback_result[33]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[33]) );
loopBackCheck loopBackGPIO06ATLASA31( .ref_sig_i(GPIO[05]), .check_sig_i(ATLAS_A31), .not_ok_o(loopback_result[34]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[34]) );
loopBackCheck loopBackGPIO07ATLASC27( .ref_sig_i(GPIO[06]), .check_sig_i(ATLAS_C27), .not_ok_o(loopback_result[35]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[35]) );
loopBackCheck loopBackGPIO08ATLASC29( .ref_sig_i(GPIO[07]), .check_sig_i(ATLAS_C29), .not_ok_o(loopback_result[36]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[36]) );
loopBackCheck loopBackGPIO09ATLASC31( .ref_sig_i(GPIO[08]), .check_sig_i(ATLAS_C31), .not_ok_o(loopback_result[37]), .master_clock_i(FX2_CLK), .reset_i(loopback_reset[37]) );



// baudclock generator -- generate 115200 baud clock from 12e6 fx2 clock 
reg[16:0] baud_reg; 
always @ (posedge FX2_CLK) begin
	baud_reg <= baud_reg[15:0] + 629; 
end 
 
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
reg [32:0] idle_counter; 
reg [7:0] char_to_xmit; 
reg [15:0] rs232_delay_count; 
reg [7:0] pass_count; 
reg [7:0] loop_counter; 

//  state machine for the loopback tester 

always @ ( posedge FX2_CLK ) 
begin 
	case ( test_loop_state ) 
		0: begin  // init -- reset all loopbacks 
            loopback_reset <= {38{1'b1}}; 
			test_circ_idx <= 0; 
			test_loop_state <= 1; 
		end
		
		1: begin  // come out of init - go into idling state 
			loopback_reset <= 0; 
			test_loop_state <= 2; 
			idle_counter <= 0; 
			loop_counter <= 0; 
		end 
		
		2: begin 
			if ( idle_counter >= 316000 ) begin // idle state 
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
			char_to_xmit <= 70;  // 'F'
			test_loop_state <= 6; 
		end 
		
		5: begin  // circuit passed 
			char_to_xmit <= 80;  // 'P'
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
			char_to_xmit <= NibbleToChar(test_circ_idx[7:4]); 
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
			char_to_xmit <= NibbleToChar(test_circ_idx[3:0]); 
			post_xmit_state <= 12;  // send line feed 
			test_loop_state <= 8;  			
		end 
		
		12: begin  // send lf char 
			char_to_xmit <= 13;   // 10 == line feed, 13==cr
			post_xmit_state <= 13; 
			test_loop_state <= 8;  // send char 
		end 
		
		13: begin  // pass fail message has been sent, reset this circuit, increment circ_idx and  back to idle 
			loopback_reset[test_circ_idx] <= 1; 
			test_loop_state <= 14; 				      
		end 
		
		14: begin  // all done printing mesaage - see if we're at end of loop and if all passed print msg - else go on to next circ 
			loopback_reset[test_circ_idx] <= 0; 
			if ( test_circ_idx == 37 ) begin  // are we at end of list?? reset to 0
				test_circ_idx <= 0;  
				loop_counter <= loop_counter + 1; 
				if ( pass_count == 38 ) begin  // did all circuits pass? 
					test_loop_state <= 16;  // go print 'All Pass!" message 
				end 
				else begin
					test_loop_state <= 27; // we had fails on this loop - print a new line and  go back to the top 
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
			post_xmit_state <= 28; 
		end 				
		
		25: begin  // print "All Pass!" message 
			char_to_xmit <= 13; // LF==10  CR==13
			test_loop_state <= 8; 
			post_xmit_state <= 26; 
		end 					
		
		26: begin  // "All Pass!" message printed - reset loop and restart 
			test_loop_state <= 27;  
		end
			
	    27: begin  // print a new line char and go to idle state 
			char_to_xmit <= 13; 
			test_loop_state <=8; 
			post_xmit_state <= 2; 
		end 
		
		28: begin // printing "All Pass! nn" message - space before nn 
			char_to_xmit <= 32;  // ' '
			test_loop_state <= 8; 
			post_xmit_state <= 29; 
		end
		
		29: begin  // print hi nibble of loop count 
			char_to_xmit <= NibbleToChar(loop_counter[7:4]); 
			test_loop_state <= 8; 
			post_xmit_state <= 30; 
		end 
		
		30: begin  // print lo nibble of loop count 
			char_to_xmit <= NibbleToChar(loop_counter[3:0]); 
			test_loop_state <= 8; 
			post_xmit_state <= 25;  // go print line end 
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
