// 
// Copyright (c) Bill Tracey 2006 (bill@ewjt.com) 
// 
// Simple test load for the Janus CPLD -- takes in a clock, divides it down via a register and feeds it out to all the CPLD pins so one can 
// validate board wiring 
// 

module JanusTestTop(
   input CLK_24MHZ,
   output CAL,
   output CBCLK,
   output CDIN,
   output CDOUT,
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
   output FSYNC,
   output HPF,
   output IPWM,
   output LRCLK,
   output MCLK,
   output MOSI,
   output nCS,
   output nRST,
   output PTT,
   output QPWM,
   output SCLK,
   output SDOUT,
   output SMODE1,
   output SMODE2,
   output SSCK,
   output XA0,
   output XA1,
   output XA2,
   output XA3,
   output XA4,
   output XA5,
   output XA6,
   output XA7,
   output XB0,
   output XB1,
   output XB2,
   output XB3,
   output XB4,
   output XB5,
   output XB6,
   output XB7,
   output XC0,
   output XC1,
   output XC2_ISCSCK,
   output XC3_I2CSDA,
   output XC4,
   output XDC6,
   output YA0,
   output YA1,
   output YA2,
   output YA3,
   output YA4,
   output YA5,
   output YA6,
   output YA7,
   output YB0,
   output YB1,
   output YB2,
   output YB3,
   output YB4,
   output YB5,
   output YB6,
   output YB7,
   output YC0,
   output YC1,
   output YC2,
   output YC3,
   output YC4,
   output YC5,
   output YC6,
   output YC7,
   output YDC0,
   output YDC3,
   output YDC6,
   output ZCAL
); 
        

// divided down clock signals 
// 0 12.288 Mhz
// 1 6.144 Mhz 
// 2 3.072 Mhz 
// 3 1.536
// 4 768 khz
// 5 384 khz
// 6 192khz
// 7  96khz
// 8  48khz
// 9 24khz
// 10 12khz
// 11 6khz
// 12 3khz
// 13 1.5khz
// 14  750hz
// 15 375hz
// 16 187.5hz
// 17 93.75hz
// 18 46.875hz
// 19 23.4375 hz 


reg [19:0] clock_reg; 
  
always @ ( posedge  CLK_24MHZ) begin 
	clock_reg <= clock_reg + 1;
end 

reg clock_by_3; 
reg[1:0] clock_by_3_reg; 
 
always @ ( posedge CLK_24MHZ) begin 
	if ( clock_by_3_reg == 2 ) begin
		clock_by_3 <= ~clock_by_3; 
		clock_by_3_reg <= 0; 
	end 
	else begin 
		clock_by_3_reg <= clock_by_3_reg + 1; 
	end 			
end 


reg clock_by_5; 
reg[2:0] clock_by_5_reg; 
 
always @ ( posedge CLK_24MHZ) begin 
	if ( clock_by_5_reg == 4 ) begin
		clock_by_5 <= ~clock_by_5; 
		clock_by_5_reg <= 0; 
	end 
	else begin 
		clock_by_5_reg <= clock_by_5_reg + 1; 
	end 			
end 




// put clocks on the pins 

assign CAL = clock_reg[00];
assign CBCLK = clock_by_3; // clock_reg[01];
assign CDIN = clock_by_5;  // clock_reg[02];
assign CDOUT = clock_reg[03];
assign CLRCIN = clock_reg[04];
assign CLRCOUT = clock_reg[05];
assign CMCLK = clock_reg[06];
assign CMODE = clock_reg[07];
assign DFS0 = clock_reg[08];
assign DFS1 = clock_reg[09];
assign EXP1 = clock_reg[10];
assign EXP2 = clock_reg[11];
assign EXP3 = clock_reg[12];
assign EXP4 = clock_reg[13];
assign FSYNC = clock_reg[14];
assign HPF = clock_reg[15];
assign IPWM = clock_reg[16];
assign LRCLK = clock_reg[17];
assign MCLK = clock_reg[18];
assign MOSI = clock_reg[19];
assign nCS = clock_reg[00];
assign nRST = clock_reg[01];
assign PTT = clock_reg[02];
assign QPWM = clock_reg[03];
assign SCLK = clock_reg[04];
assign SDOUT = clock_reg[05];
assign SMODE1 = clock_reg[06];
assign SMODE2 = clock_reg[07];
assign SSCK = clock_reg[08];
assign XA0 = clock_reg[09];
assign XA1 = clock_reg[10];
assign XA2 = clock_reg[11];
assign XA3 = clock_reg[12];
assign XA4 = clock_reg[13];
assign XA5 = clock_reg[14];
assign XA6 = clock_reg[15];
assign XA7 = clock_reg[16];
assign XB0 = clock_reg[17];
assign XB1 = clock_reg[18];
assign XB2 = clock_reg[19];
assign XB3 = clock_reg[00];
assign XB4 = clock_reg[01];
assign XB5 = clock_reg[02];
assign XB6 = clock_reg[03];
assign XB7 = clock_reg[04];
assign XC0 = clock_reg[05];
assign XC1 = clock_reg[06];
assign XC2_ISCSCK = clock_reg[07];
assign XC3_I2CSDA = clock_reg[08];
assign XC4 = clock_reg[09];
assign XDC6 = clock_reg[10];
assign YA0 = clock_reg[11];
assign YA1 = clock_reg[12];
assign YA2 = clock_reg[13];
assign YA3 = clock_reg[14];
assign YA4 = clock_reg[15];
assign YA5 = clock_reg[16];
assign YA6 = clock_reg[17];
assign YA7 = clock_reg[18];
assign YB0 = clock_reg[19];
assign YB1 = clock_reg[00];
assign YB2 = clock_reg[01];
assign YB3 = clock_reg[02];
assign YB4 = clock_reg[03];
assign YB5 = clock_reg[04];
assign YB6 = clock_reg[05];
assign YB7 = clock_reg[06];
assign YC0 = clock_reg[07];
assign YC1 = clock_reg[08];
assign YC2 = clock_reg[09];
assign YC3 = clock_reg[10];
assign YC4 = clock_reg[11];
assign YC5 = clock_reg[12];
assign YC6 = clock_reg[13];
assign YC7 = clock_reg[14];
assign YDC0 = clock_reg[15];
assign YDC3 = clock_reg[16];
assign YDC6 = clock_reg[17];
assign ZCAL = clock_reg[18];


endmodule 


