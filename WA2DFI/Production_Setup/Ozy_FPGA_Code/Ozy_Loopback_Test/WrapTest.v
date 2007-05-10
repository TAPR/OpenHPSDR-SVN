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
// 28 Mar 2007	Scott Cowling	Change baud rate to 9600 (38400 when running FX2 code)
//								Change D1-D4 LEDs to rotate unless error. If error, all stay on
// 2 Apr 2007	SC				Rewrite test to check in parallel
//
/*
Test matrix

loopback				Ozy			Ozy			sch				sch
result			test	conn		conn		signal			signal
bit				nr		output		input		output			input
--------		----	-------		-------		---------		---------
0				0x00	J31-A25		J31-C25		ATLAS_A25		ATLAS_C25
1				0x01	J31-A24		J31-C24		ATLAS_A24		ATLAS_C24
2				0x02	J31-A23		J31-C23		ATLAS_A23		ATLAS_C23
3				0x03	J31-A22		J31-C22		ATLAS_A22		ATLAS_C22
4				0x04	J31-A19		J31-C19		ATLAS_A19		ATLAS_C19
5				0x05	J31-A18		J31-C18		ATLAS_A18		ATLAS_C18
6				0x06	J31-A17		J31-C17		ATLAS_A17		ATLAS_C17
7				0x07	J31-A16		J31-C16		ATLAS_A16		ATLAS_C16
8				0x08	J31-A15		J31-C15		ATLAS_A15		ATLAS_C15
9				0x09	J31-A14		J31-C14		ATLAS_A14		ATLAS_C14
10				0x0a	J31-A13		J31-C13		ATLAS_A13		ATLAS_C13
11				0x0b	J31-A12		J31-C12		ATLAS_A12		ATLAS_C12
12				0x0c	J31-A11		J31-C11		ATLAS_A11		ATLAS_C11
13				0x0d	J31-A10		J31-C10		ATLAS_A10		ATLAS_C10
14				0x0e	J31-A09		J31-C09		ATLAS_A9		ATLAS_C9
15				0x0f	J31-A08		J31-C08		ATLAS_A8		ATLAS_C8
16				0x10	J31-A07		J31-C07		ATLAS_A7		ATLAS_C7
17				0x11	J31-A06		J31-C06		ATLAS_A6		ATLAS_C6
18				0x12	J31-A05		J31-C05		ATLAS_A5		ATLAS_C5
19				0x13	J31-A04		J31-C04		ATLAS_A4		ATLAS_C4
20				0x14	J31-A03		J31-C03		ATLAS_A3		ATLAS_C3
21				0x15	J31-A02		J31-C02		ATLAS_A2		ATLAS_C2
22				0x16	J20-2		J20-10		FPGA_GPIO1		FPGA_GPIO17
23				0x17	J20-3		J31-C20		FPGA_GPIO2		ATLAS_C20
24				0x18	J20-4		J31-C21		FPGA_GPIO3		ATLAS_C21
25				0x19	J20-5		J31-A27		FPGA_GPIO4		ATLAS_A27
26				0x1a	J20-6		J31-A29		FPGA_GPIO5		ATLAS_A29
27				0x1b	J20-7		J31-A31		FPGA_GPIO6		ATLAS_A31
28				0x1c	J20-8		J31-C27		FPGA_GPIO7		ATLAS_C27
29				0x1d	J20-9		J31-C29		FPGA_GPIO8		ATLAS_C29
30				0x1e	J20-1		J31-C31		FPGA_GPIO9		ATLAS_C31
31				0x1f	J20-14		J20-11		FPGA_GPIO10		FPGA_GPIO18
32				0x20	J20-16		J20-12		FPGA_GPIO11		FPGA_GPIO19
33				0x21	J20-17		J20-13		FPGA_GPIO12		FPGA_GPIO20
34				0x22	J8-1		J20-15		FPGA_GPIO13		FPGA_GPIO21
25				0x23	J8-2		J8-6		FPGA_GPIO14		FPGA_GPIO22
36				0x24	J8-3		J8-7		FPGA_GPIO15		FPGA_GPIO23
37				0x25	J8-4		J8-8		FPGA_GPIO16		FPGA_GPIO24
*/


module WrapTest(
        IFCLK, FX2_FD, FLAGA, FLAGB, FLAGC, SLWR, SLRD, SLOE, PKEND, FIFO_ADR, 
 	    DEBUG_LED0, DEBUG_LED1, DEBUG_LED2, DEBUG_LED3, 	
		FX2_CLK, SPI_SCK, SPI_SI, SPI_SO, SPI_CS, GPin, GPout, GPIO_nIOE,
		ATLAS_A02, ATLAS_A03, ATLAS_A04, ATLAS_A05, ATLAS_A06, ATLAS_A07, ATLAS_A08, ATLAS_A09,
        ATLAS_A10, ATLAS_A11, ATLAS_A12, ATLAS_A13, ATLAS_A14, ATLAS_A15, ATLAS_A16, ATLAS_A17,
        ATLAS_A18, ATLAS_A19, ATLAS_A22, ATLAS_A23, ATLAS_A24, ATLAS_A25, ATLAS_C02, ATLAS_C03,
        ATLAS_C04, ATLAS_C05, ATLAS_C06, ATLAS_C07, ATLAS_C08, ATLAS_C09, ATLAS_C10, ATLAS_C11,
        ATLAS_C12, ATLAS_C13, ATLAS_C14, ATLAS_C15, ATLAS_C16, ATLAS_C17, ATLAS_C18, ATLAS_C19,
        ATLAS_C22, ATLAS_C23, ATLAS_C24, ATLAS_C25,
	    ATLAS_C20, ATLAS_C21, ATLAS_A27, ATLAS_A29, ATLAS_A31, ATLAS_C27, ATLAS_C29, ATLAS_C31,
		ATLAS_A20, ATLAS_A21,
   		FPGA_TXD, FPGA_RXD
		);
		
		
input			IFCLK;			// FX2 IFCLOCK: 48MHz
input [15:0]	FX2_FD;			// bidirectional FIFO data to/from the FX2

input 			FLAGA;
input 			FLAGB;
input 			FLAGC;

output 			SLWR;
output 			SLRD;
output 			SLOE;

output 			PKEND;
output [1:0] 	FIFO_ADR;

// LEDs on OZY board
//
output DEBUG_LED0;		// D1: 1=off, 0=on
output DEBUG_LED1;		// D2: 1=off, 0=on
output DEBUG_LED2;		// D3: 1=off, 0=on
output DEBUG_LED3;		// D4: 1=off, 0=on	

// interface lines for GPIO control 
//
input 			FX2_CLK;		// master system clock from FX2 
input 			SPI_SCK;		// SPI SCK from FX2
input 			SPI_SI;			// SPI serial in from FX2
inout 			SPI_SO;			// SPI serial out to FX2
input 			SPI_CS;         // FPGA chip select from FX2
input  [23:16]	GPin;			// OZY GPIO input lines
output [15:0]	GPout;			// OZY GPIO output lines
output 			GPIO_nIOE;      // enable GPIO driver chips 

// stimulus outputs
//
output 	ATLAS_A02;	
output 	ATLAS_A03;
output 	ATLAS_A04;
output 	ATLAS_A05;
output 	ATLAS_A06;
output 	ATLAS_A07;
output 	ATLAS_A08;
output 	ATLAS_A09;
output 	ATLAS_A10;
output 	ATLAS_A11;
output 	ATLAS_A12;
output 	ATLAS_A13;
output 	ATLAS_A14;
output 	ATLAS_A15;
output 	ATLAS_A16;
output 	ATLAS_A17;
output 	ATLAS_A18;
output 	ATLAS_A19;

output 	ATLAS_A22;
output 	ATLAS_A23;
output 	ATLAS_A24;
output 	ATLAS_A25;

// response inputs
//
input 	ATLAS_C02;	
input 	ATLAS_C03;
input 	ATLAS_C04;
input 	ATLAS_C05;
input 	ATLAS_C06;
input 	ATLAS_C07;
input 	ATLAS_C08;
input 	ATLAS_C09;
input 	ATLAS_C10;
input 	ATLAS_C11;
input 	ATLAS_C12;
input 	ATLAS_C13;
input 	ATLAS_C14;
input 	ATLAS_C15;
input 	ATLAS_C16;
input 	ATLAS_C17;
input 	ATLAS_C18;
input 	ATLAS_C19;
input 	ATLAS_C22;
input 	ATLAS_C23;
input 	ATLAS_C24;
input 	ATLAS_C25;

input 	ATLAS_C20;
input 	ATLAS_C21;

input 	ATLAS_A27;
input 	ATLAS_A29;
input 	ATLAS_A31;

input 	ATLAS_C27;
input 	ATLAS_C29;
input 	ATLAS_C31;

input	ATLAS_A20;
input	ATLAS_A21;

// Serial Port
//
output	FPGA_TXD;  	// serial port output line
input	FPGA_RXD;	// serial port input line 

//-----------------------------------------------------------
//
// Define signal types for module pins
//

//
//
wire 		SLWR;
wire 		SLRD;
wire 		SLOE;
wire 		PKEND;
wire [1:0]	FIFO_ADR;

wire		SPI_SO;

// stimulus outputs
//
wire 		ATLAS_A02;		// hardware ATLAS_A2 (J31-A2)	
wire 		ATLAS_A03;		// hardware ATLAS_A3 (J31-A3)
wire 		ATLAS_A04;		// hardware ATLAS_A4 (J31-A4)
wire 		ATLAS_A05;		// hardware ATLAS_A5 (J31-A5)
wire 		ATLAS_A06;		// hardware ATLAS_A6 (J31-A6)
wire 		ATLAS_A07;		// hardware ATLAS_A7 (J31-A7)
wire 		ATLAS_A08;		// hardware ATLAS_A8 (J31-A8)
wire 		ATLAS_A09;		// hardware ATLAS_A9 (J31-A9)
wire 		ATLAS_A10;		// hardware ATLAS_A10 (J31-A10)
wire 		ATLAS_A11;		// hardware ATLAS_A11 (J31-A11)
wire 		ATLAS_A12;		// hardware ATLAS_A12 (J31-A12)
wire 		ATLAS_A13;		// hardware ATLAS_A13 (J31-A13)
wire 		ATLAS_A14;		// hardware ATLAS_A14 (J31-A14)
wire 		ATLAS_A15;		// hardware ATLAS_A15 (J31-A15)
wire 		ATLAS_A16;		// hardware ATLAS_A16 (J31-A16)
wire 		ATLAS_A17;		// hardware ATLAS_A17 (J31-A17)
wire 		ATLAS_A18;		// hardware ATLAS_A18 (J31-A18)
wire 		ATLAS_A19;		// hardware ATLAS_A19 (J31-A19)
wire 		ATLAS_A22;		// hardware ATLAS_A22 (J31-A22)
wire 		ATLAS_A23;		// hardware ATLAS_A23 (J31-A23)
wire 		ATLAS_A24;		// hardware ATLAS_A24 (J31-A24)
wire 		ATLAS_A25;		// hardware ATLAS_A25 (J31-A25)




//-----------------------------------------------------------
//
// Local signal declarations
//

//
//
wire [15:0]	GPout;					// GPIO outputs

// LED control
//
reg [3:0] 	leds; 					// LEDs 1=off, 0=on
reg [24:0] 	clock_count; 			// counter for delaying LED rotation


// State machine
//
reg [7:0]	test_circ_idx;			// index into pass and reset vectors (0 to 37)
reg [4:0] 	test_loop_state; 		// state bits
reg [4:0] 	post_xmit_state; 		// next state after transmit
reg [32:0] 	idle_counter; 			// counter for delay after each output line
reg [7:0] 	char_to_xmit; 			// output character register
reg [15:0] 	rs232_delay_count; 		// counter for delay after each output character
reg [7:0] 	pass_count; 			// number of circuits that pass on each scan (0 to 38)

// Vectors for stimulus outputs
//
reg [21:0]	atlas_vec;			// vector for atlas stimulus outputs
reg [15:0]	gpio_vec;			// vector for gpio stimulus outputs

// Vectors for response inputs
//
wire [21:0]	atlas_resp;			// vector for atlas response inputs
wire [15:0]	gpio_resp;			// vector for gpio response inputs

// Loopback vectors
//
reg	[37:0]	loopback_reset; 	// 1=reset loopback bit, 0=do nothing
reg [37:0]	loopback_result; 	// 1=error, 0=OK

// Loopback test signals
//
reg	[7:0] 	cmp_cnt;			// used to determine interval for pattern shifting

// Baud clock generator
//
reg[16:0] baud_reg; 			// counter used to generate baud rate clock



//-----------------------------------------------------------
//

// invert led vector onto LED output pins	
//
assign DEBUG_LED0 = ~leds[0]; 		
assign DEBUG_LED1 = ~leds[1];
assign DEBUG_LED2 = ~leds[2]; 		
assign DEBUG_LED3 = ~leds[3];

// Unused output pins are set high
//
assign	SLWR 		= 1'b0;
assign	SLRD 		= 1'b0;
assign  SLOE 		= 1'b0;
assign  PKEND 		= 1'b0;
assign	FIFO_ADR	= 2'b00;

// enable GPIO output driver
//
assign GPIO_nIOE = 0;

// Unused SPI output pin is set high-z
//
assign SPI_SO = (1'b0) ? 1'h0 : 1'bz;

// Assign stimulus output vector bits to output pins
//
assign ATLAS_A02 = atlas_vec[21];
assign ATLAS_A03 = atlas_vec[20];
assign ATLAS_A04 = atlas_vec[19];
assign ATLAS_A05 = atlas_vec[18];
assign ATLAS_A06 = atlas_vec[17];
assign ATLAS_A07 = atlas_vec[16];
assign ATLAS_A08 = atlas_vec[15];
assign ATLAS_A09 = atlas_vec[14];
assign ATLAS_A10 = atlas_vec[13];
assign ATLAS_A11 = atlas_vec[12];
assign ATLAS_A12 = atlas_vec[11];
assign ATLAS_A13 = atlas_vec[10];
assign ATLAS_A14 = atlas_vec[9];
assign ATLAS_A15 = atlas_vec[8];
assign ATLAS_A16 = atlas_vec[7];
assign ATLAS_A17 = atlas_vec[6];
assign ATLAS_A18 = atlas_vec[5];
assign ATLAS_A19 = atlas_vec[4];
assign ATLAS_A22 = atlas_vec[3];
assign ATLAS_A23 = atlas_vec[2];
assign ATLAS_A24 = atlas_vec[1];
assign ATLAS_A25 = atlas_vec[0];

assign GPout[15:0] = gpio_vec;


// Assign input pins to response vector
//
assign atlas_resp = {ATLAS_C02,		// hardware ATLAS_C2 (J31-C2)
					ATLAS_C03,		// hardware ATLAS_C3 (J31-C3)
					ATLAS_C04,		// hardware ATLAS_C4 (J31-C4)
					ATLAS_C05,		// hardware ATLAS_C5 (J31-C5)
					ATLAS_C06,		// hardware ATLAS_C6 (J31-C6)
					ATLAS_C07,		// hardware ATLAS_C7 (J31-C7)
					ATLAS_C08,		// hardware ATLAS_C8 (J31-C8)
					ATLAS_C09,		// hardware ATLAS_C9 (J31-C9)
					ATLAS_C10,		// hardware ATLAS_C10 (J31-C10)
					ATLAS_C11,		// hardware ATLAS_C11 (J31-C11)
					ATLAS_C12,		// hardware ATLAS_C12 (J31-C12)
					ATLAS_C13,		// hardware ATLAS_C13 (J31-C13)
					ATLAS_C14,		// hardware ATLAS_C14 (J31-C14)
					ATLAS_C15,		// hardware ATLAS_C15 (J31-C15)
					ATLAS_C16,		// hardware ATLAS_C16 (J31-C16)
					ATLAS_C17,		// hardware ATLAS_C17 (J31-C17)
					ATLAS_C18,		// hardware ATLAS_C18 (J31-C18)
					ATLAS_C19,		// hardware ATLAS_C19 (J31-C19)
					ATLAS_C22,		// hardware ATLAS_C22 (J31-C22)
					ATLAS_C23,		// hardware ATLAS_C23 (J31-C23)
					ATLAS_C24,		// hardware ATLAS_C24 (J31-C24)
					ATLAS_C25};		// hardware ATLAS_C25 (J31-C25)
					
// These are zero-based and the hardware is 1-based. They are response inputs.
//
assign gpio_resp = {~GPin[23],		// hardware FPGA_GPIO24 (J8-8)
					~GPin[22],		// hardware FPGA_GPIO23 (J8-7)
					~GPin[21],		// hardware FPGA_GPIO22 (J8-6)
					~GPin[20],		// hardware FPGA_GPIO21 (J20-15)
					GPin[19],		// hardware FPGA_GPIO20 (J20-13)
					GPin[18],		// hardware FPGA_GPIO19 (J20-12)
					GPin[17],		// hardware FPGA_GPIO18 (J20-11)
					ATLAS_C31,		// hardware ATLAS_C31 (J31-C31)
					ATLAS_C29,		// hardware ATLAS_C29 (J31-C29)
					ATLAS_C27,		// hardware ATLAS_C27 (J31-C27)
					ATLAS_A31,		// hardware ATLAS_A31 (J31-A31)
					ATLAS_A29,		// hardware ATLAS_A29 (J31-A29)
					ATLAS_A27,		// hardware ATLAS_A27 (J31-A27)
					ATLAS_C21,		// hardware ATLAS_C21 (J31-C21)
					ATLAS_C20,		// hardware ATLAS_C20 (J31-C20)
					GPin[16]		// hardware FPGA_GPIO17 (J20-10)
					};




//-----------------------------------------------------------
//
// Generate test pattern on GPIO and atlas vectors.
//
always @ (posedge FX2_CLK) begin
	if (clock_count == 25'h0) begin
		atlas_vec	<= 22'h155555;		// clear both vectors
		gpio_vec	<= 16'hAAAA;
		cmp_cnt		<= 8'h0;		// clear delay count
		loopback_result	<= 38'h0;	// clear errors
	end
	else begin 
		if (cmp_cnt	== 8'h10) begin				// time to update loopback_result
			loopback_result	<= (({gpio_vec, atlas_vec} ^ {gpio_resp, atlas_resp}) | loopback_result) & (~loopback_reset);
		end

		if (cmp_cnt == 8'h20) begin				// time to change stimulus
			cmp_cnt			<= 8'h0;			// and restart delay count
			loopback_result	<= loopback_result & (~loopback_reset);		// clear errors if required		
			if (gpio_vec == 16'h8000 && atlas_vec == 22'h0) begin
				atlas_vec	<= 22'h155555;		// if rotation complete, start with initial pattern
				gpio_vec	<= 16'hAAAA;
			end
			else begin
			{gpio_vec, atlas_vec}	<= {gpio_vec, atlas_vec} << 1;	// shift left
			end
		end
		else begin
			cmp_cnt			<= cmp_cnt + 8'b1;				// increment count
		end
	end
end 

// baudclock generator -- generate 115200 baud clock from 12e6 fx2 clock
//
// SC 03-28-2007
// switch to 9600 baud from 12MHz default FX2 clock
// this will be 38400 baud when FX2 is initialized and running at 48MHz 
//

always @ (posedge FX2_CLK) begin
//	baud_reg <= baud_reg[15:0] + 16'd629; 	// 115,200 baud (erroneous)
	baud_reg <= baud_reg + 17'd105; 			// 9600 baud
//	baud_reg <= baud_reg + 17'd629; 			// 57,600 baud
end 

//
// LED rotation generator
//
// Four LEDs rotate unless an error is detected in the loopback result
// register. When error is detected, they hold all on.
//
always @ (posedge FX2_CLK) begin 
	if ( leds == 0 ) begin 
		leds <= 1; 
	end 	
	else begin 
		if ( clock_count == 1200000 ) begin
			clock_count <= 0; 
			if (|loopback_result[37:0]) begin
				leds <= 4'b1111;
			end
			else if ( leds[3] == 1'b1 ) begin 
				leds <= 4'b0001; 
			end 
			else begin 
				leds <= leds << 1; 
			end 
		end 		
		else begin 
			clock_count <= clock_count + 25'h1; 
		end 
	end 
end 


//-----------------------------------------------------------
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
				
				
//-----------------------------------------------------------
//
// get a hex char from a nibble 
//							
function [7:0] NibbleToChar; 
	input [3:0] nibble; 
	begin 
		if ( nibble < 4'd10 ) begin 
			NibbleToChar = 8'd48 + {4'd0, nibble};  		// 48 = '0' 
		end 
		else begin 
			NibbleToChar = 8'd97 - 8'd10 + {4'd0, nibble};  // 97 = 'a' 
		end 
	end 
endfunction 

//-----------------------------------------------------------
//


//  state machine for the loopback tester 
//
// This state machine scans the loopback_result vector for errors. When
// any are found, it displays it via the serial port and then resets the
// bit via the loopback_reset vector.

always @ ( posedge FX2_CLK ) 
begin 
	case ( test_loop_state ) 
		0: begin  										// init -- reset all loopbacks 
			loopback_reset		<= 38'h3F_FFFF_FFFF; 
			test_circ_idx 		<= 8'h0; 
			test_loop_state 	<= 5'd1; 
		end
		
		1: begin  										// come out of init - go into idling state 
			loopback_reset 		<= 38'h0; 
			idle_counter 		<= 33'd0;
			test_loop_state 	<= 5'd2; 
		end 
		
		2: begin 
			if ( idle_counter >= 33'd316000 ) begin 	// idle state - delay after every line
				idle_counter 	<= 33'd0;				// prep for next time 
				test_loop_state <= 5'd3; 				// get out of idle state 		
			end 
			else begin  								// count not met yet -- stay here 
				idle_counter 	<= idle_counter + 33'h1; 
				test_loop_state <= 5'd2; 
			end 		
		end 
		
		3:  begin 										// coming out of idle state - check the circ and select next state appropriately 
			if ( loopback_result[test_circ_idx] == 1 ) begin  	// failed 				
				test_loop_state		<= 5'd4; 
			end 
			else begin 									// passed 
				pass_count 			<= pass_count + 8'h1; 
				// test_loop_state 	<= 5'h5; 			// use this to print passes on individual circs 
				test_loop_state 	<= 5'd13;   		// use this to not print passes on ind circs 
			end 
		end 
		
		4: begin  										// circuit failed 
			char_to_xmit 	<= 8'd70;  					// 'F'
			test_loop_state <= 5'd6; 
		end 
		
		5: begin  										// circuit passed 
			char_to_xmit 	<= 8'd80;					// 'P'
			test_loop_state <= 5'd6; 
		end 
		
		6: begin 										// xmit pass fail message 
			post_xmit_state <= 5'd7; 
			test_loop_state <= 5'd8; 
		end 
		
		7: begin 										// pass fail char completed printing - print a space 
			char_to_xmit 	<= 8'd32;  					// 32 == ' ' 
			post_xmit_state <= 5'd9; 
			test_loop_state <= 5'd8; 
		end 
		
		8: begin 										// xmit a char over serial port 
			rs232_xmit_reg 		<= char_to_xmit; 
			rs232_write_strobe 	<= 1'b1; 
			test_loop_state 	<= 5'd10; 
		end 
		
		9: begin  // space transmitted -- xmit circ number  -- hi nibble first 
			char_to_xmit 		<= NibbleToChar(test_circ_idx[7:4]); 
			rs232_write_strobe 	<= 1'b0; 
			post_xmit_state 	<= 5'd11; 
			test_loop_state 	<= 5'd8;  
		end 
		
		10: begin  // char to xmit strobed - unstrobe and on to next state 
			rs232_write_strobe	<= 1'b0;
			rs232_delay_count 	<= 16'd10000;  			// delay after every character
			test_loop_state 	<= 5'd15; 
		end 
		
		11: begin // send low nibble of circ number 
			char_to_xmit 		<= NibbleToChar(test_circ_idx[3:0]); 
			post_xmit_state 	<= 5'd12;  				// send line feed 
			test_loop_state 	<= 5'd8;  			
		end 
		
		12: begin  // send lf char 
			char_to_xmit 		<= 8'd13;   			// 10 == line feed, 13==cr
			post_xmit_state 	<= 5'd13; 
			test_loop_state 	<= 5'd8;  				// send char 
		end 
		
		13: begin  										// pass fail message has been sent, reset this circuit, increment circ_idx and  back to idle 
			loopback_reset[test_circ_idx] 	<= 1'b1; 
			test_loop_state 				<= 5'd14; 				      
		end 
		
		14: begin  										// all done printing message - see if we're at end of loop and if all passed print msg - else go on to next circ 
			loopback_reset[test_circ_idx] 	<= 1'b0; 
			if ( test_circ_idx == 8'd37 ) begin  		// are we at end of list?? reset to 0
				test_circ_idx 		<= 8'd0;  
				if ( pass_count == 8'd38 ) begin  		// did all circuits pass? 
					test_loop_state <= 5'd16;  			// go print 'All Pass!" message 
				end 
				else begin
					test_loop_state <= 5'd27; 			// we had fails on this loop - print a new line and  go back to the top 
				end
				pass_count 			<= 8'd0; 
			end 
			else begin 
				test_circ_idx 		<= test_circ_idx + 8'b1; 
				test_loop_state 	<= 5'd2; 
			end 
		end 							
		
		15: begin  										// post rs232 xmit delay 
			if ( rs232_delay_count == 16'd0 ) begin
				test_loop_state <= post_xmit_state; 
			end 
			else begin 
				rs232_delay_count <= rs232_delay_count - 16'b1; 
				test_loop_state <= 5'd15;  				// stay here 
			end 
		end	
		
		16: begin  										// print "All Pass!" message 
			char_to_xmit 		<= 8'd65; 				// A
			test_loop_state 	<= 5'd8; 
			post_xmit_state 	<= 5'd17; 
		end 
		
		17: begin  										// print "All Pass!" message 
			char_to_xmit 		<= 8'd108; 				// l
			test_loop_state 	<= 5'd8; 
			post_xmit_state 	<= 5'd18; 
		end 
		
		18: begin  										// print "All Pass!" message 
			char_to_xmit 		<= 8'd108; 				// l
			test_loop_state 	<= 5'd8; 
			post_xmit_state 	<= 5'd19; 
		end 
		
		19: begin  										// print "All Pass!" message 
			char_to_xmit 		<= 8'd32; 				// ' ' 
			test_loop_state 	<= 5'd8; 
			post_xmit_state 	<= 5'd20; 
		end 
		
		20: begin  										// print "All Pass!" message 
			char_to_xmit 		<= 8'd80; 				// P
			test_loop_state 	<= 5'd8; 
			post_xmit_state 	<= 5'd21; 
		end 
		
		21: begin  										// print "All Pass!" message 
			char_to_xmit 		<= 8'd97; 				// a
			test_loop_state 	<= 5'd8; 
			post_xmit_state 	<= 5'd22; 
		end 		
		
		22: begin  										// print "All Pass!" message 
			char_to_xmit 		<= 8'd115;				// s
			test_loop_state 	<= 5'd8; 
			post_xmit_state 	<= 5'd23; 
		end 				
		
		23: begin  										// print "All Pass!" message 
			char_to_xmit 		<= 8'd115;				// s
			test_loop_state 	<= 5'd8; 
			post_xmit_state 	<= 5'd24; 
		end 				
		
		24: begin  										// print "All Pass!" message 
			char_to_xmit 		<= 8'd33;				// !
			test_loop_state 	<= 5'd8; 
			post_xmit_state 	<= 5'd25; 
		end 				
		
		25: begin  										// print "All Pass!" message 
			char_to_xmit 		<= 8'd13;				// LF==10  CR==13
			test_loop_state 	<= 5'd8; 
			post_xmit_state 	<= 5'd26; 
		end 					
		
		26: begin  										// "All Pass!" message printed - reset loop and restart 
			test_loop_state 	<= 5'd27;  
		end
			
	    27: begin  										// print a new line char and go to idle state 
			if ( idle_counter >= 33'd40000000 ) begin	// delay at end of all errors 
				idle_counter 	<= 33'd0;  				// prep for next time 
				char_to_xmit 	<= 8'd13; 
				test_loop_state <= 5'd8; 
				post_xmit_state <= 5'd2;
			end 
			else begin  								// count not met yet -- stay here 
				idle_counter 	<= idle_counter + 33'd1; 
				test_loop_state <= 5'd27; 
			end 		
		end 

		default: begin
			test_loop_state <= 5'd0;
		end

	endcase 
	
end 

endmodule 