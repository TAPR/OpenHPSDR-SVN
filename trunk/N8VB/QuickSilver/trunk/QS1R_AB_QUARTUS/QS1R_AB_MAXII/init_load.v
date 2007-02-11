// QS1R AB
// Copyright 2006 P.Covington N8VB
// MAX II Firmware for U25 MMIO
// Set CKCON = 2 in FX2 when using 48 MHz clock
//

module init_load(

input FX2CLK,
input CLKA,
input DDC0_CLK,
input DDC1_CLK,
inout [7:0] D,

output DITH,
output SHDN,
output RAND,
output PGA,
input OVERFLOW,

output ADC_OVFL_IND,
output reg CPLD_RESET_OUT,
output HPF_BYPASS,
output HPF_BYPASS_N,
output LPF_BYPASS,
output LPF_BYPASS_N,
output RFA_BYPASS,
output RFA_BYPASS_N,
output [5:0] ATTN,
output [4:0] DEBUG_LED,
inout [3:0] CPLD_GPIO,

input FX2_SDA,
input FX2_SCL,
input FPGA_INIT_DONE,
output DDC1_PAR_SER,
output DDC0_PAR_SER,
output DDC1_RESET_N,
output DDC0_RESET_N,
output SYNC_RCF,
output SYNC_CIC,
output SYNC_NCO,
output FX2_INT4,

inout CS_FPGA_M_N,
inout SDI_M,
inout SDO_M,
inout SCLK_M,
input CS_CPLD_N,
input FX2_SDI,
input FX2_SDO,
input FX2_SCLK,

input RD_N,
input WR_N,
input [10:0] A,
input A13,
//input A14,

output [2:0] DDC_A,
output DDC_RD_N,
output DDC_WR_N,
output CS_DDC0_N,
output CS_DDC1_N

);

`define DDC0_DR0 6'h000
`define DDC0_DR1 6'h001
`define DDC0_DR2 6'h002
`define DDC0_DR3 6'h003
`define	DDC0_DR4 6'h004
`define DDC0_LAR 6'h006
`define DDC0_AMR 6'h007 

`define DDC1_DR0 6'h010
`define DDC1_DR1 6'h011
`define DDC1_DR2 6'h012
`define DDC1_DR3 6'h013
`define	DDC1_DR4 6'h014
`define DDC1_LAR 6'h016
`define DDC1_AMR 6'h017 

`define DDC_CNTRL 6'h020
`define ADC_CNTRL 6'h021
`define DEBUG_LED 6'h022
`define ATTN	  6'h023
`define RFFE_CNTRL 6'h024
`define CPLD_GPIO 6'h025

assign ADC_OVFL_IND = ~OVERFLOW;

assign FX2_INT4 = 1'b0;

assign CS_FPGA_M_N = 1'b1;
assign SDI_M = 1'bZ;
assign SDO_M = 1'bZ;
assign SCLK_M = 1'bZ;

//
// Initial Reset
//

reg [7:0] reset_counter;
reg CPLD_reset;

always @(posedge FX2CLK) begin
	if (reset_counter < 8'd255) begin
		reset_counter <= reset_counter + 8'd1;
		CPLD_reset <= 1'b1;
		CPLD_RESET_OUT <= 1'b1;
	end
	else begin
		reset_counter <= 8'd255;
		CPLD_reset <= 1'b0;
		CPLD_RESET_OUT <= 1'b0;
	end
end

// 
// Address decoder section
//

wire do_read;
wire do_write;
//wire A14; // dummy address line for now
wire [5:0] address;
reg [7:0] data;

reg [7:0] DEBUG_LED_REG;    
reg [7:0] LTC2208_CNTRL_REG;
reg [7:0] ATTN_REG;
reg [7:0] RFFE_CNTRL_REG;
reg [7:0] CPLD_GPIO_REG;
reg [7:0] DDC_CNTRL_REG;

assign do_read = ((RD_N == 1'b0) && (CS_DDC0_N == 1'b1) && (CS_DDC1_N == 1'b1)) ? 1'b1 : 1'b0;
assign do_write = (~WR_N);
assign D[7:0] = (do_read) ? data[7:0] : 8'bZZZZZZZZ;


// need to correct for A14 in later rev
// for now A14 is set to 1'b1;
// A14 & A13 & XX & A10:0
//assign A14 = 1'b1; // remove this in later rev when A14 is hooked up
//assign address = {A14, {{A13, 2'b00}, A_reg[10:0]}};

always @(posedge FX2CLK) begin

	// these are the reset values for the registers
	if (CPLD_reset) begin
		DEBUG_LED_REG <= 8'd1;
		LTC2208_CNTRL_REG <= 8'b00000010;
		ATTN_REG <= 8'd0;
		RFFE_CNTRL_REG <= 8'b00000111;
		CPLD_GPIO_REG <= 8'b11110000;
		DDC_CNTRL_REG <= 8'b00000101;
		
		end
	
	else begin
				
		case ({A[5:0]}) 
			
			`DEBUG_LED:  begin // DEBUG_LED_REG
				if (do_read) data <= DEBUG_LED_REG;
				if (do_write) DEBUG_LED_REG <= D;
				end
				
			`ADC_CNTRL:  begin // LTC2208_CNTRL_REG
				if (do_read) data <= LTC2208_CNTRL_REG;
				if (do_write) LTC2208_CNTRL_REG <= D;
				end
				
			`ATTN: begin // ATTN REG
				if (do_read) data <= ATTN_REG;
				if (do_write) ATTN_REG <= D;
				end
				
			`RFFE_CNTRL: begin // RFFE REG				
				if (do_read) data <= RFFE_CNTRL_REG;
				if (do_write) RFFE_CNTRL_REG <= D;
				end
				
			`CPLD_GPIO: begin // CPLD GPIO REG
				if (do_read) data <= CPLD_GPIO_SHADOW;
				if (do_write) CPLD_GPIO_REG <= D;
				end	
				
			`DDC_CNTRL: begin // DDC RESET REG		
				if (do_read) data <= DDC_CNTRL_REG;
				if (do_write) DDC_CNTRL_REG <= D;
				end												
		endcase				
		end
		
end

// Debug LED register bit assignment
assign DEBUG_LED[4:0] = DEBUG_LED_REG[4:0];

// ADC Register bit assignment
assign DITH = LTC2208_CNTRL_REG[0];
assign SHDN = LTC2208_CNTRL_REG[1];
assign RAND = LTC2208_CNTRL_REG[2];
assign PGA = LTC2208_CNTRL_REG[3];

// ATTN Register bit assignment
assign ATTN[5:0] = ATTN_REG[5:0];

// RFFE Control Register bit assigment
assign HPF_BYPASS = RFFE_CNTRL_REG[0];
assign HPF_BYPASS_N = ~HPF_BYPASS;
assign LPF_BYPASS = RFFE_CNTRL_REG[1];
assign LPF_BYPASS_N = ~LPF_BYPASS;
assign RFA_BYPASS = RFFE_CNTRL_REG[2];
assign RFA_BYPASS_N = ~RFA_BYPASS;

// CPLD GPIO Control Register bit assignment
assign CPLD_GPIO[0] = (CPLD_GPIO_REG[4] == 1'b1) ? 1'bZ : CPLD_GPIO_REG[0];
assign CPLD_GPIO[1] = (CPLD_GPIO_REG[5] == 1'b1) ? 1'bZ : CPLD_GPIO_REG[1];
assign CPLD_GPIO[2] = (CPLD_GPIO_REG[6] == 1'b1) ? 1'bZ : CPLD_GPIO_REG[2];
assign CPLD_GPIO[3] = (CPLD_GPIO_REG[7] == 1'b1) ? 1'bZ : CPLD_GPIO_REG[3];

wire [7:0] CPLD_GPIO_SHADOW;
assign CPLD_GPIO_SHADOW[7:0] = {CPLD_GPIO_REG[7:4], CPLD_GPIO[3:0]};

// DDC Control Register bit assignments
assign DDC0_PAR_SER = DDC_CNTRL_REG[0];
assign DDC0_RESET_N = DDC_CNTRL_REG[1];
assign DDC1_PAR_SER = DDC_CNTRL_REG[2];
assign DDC1_RESET_N = DDC_CNTRL_REG[3];
assign SYNC_RCF = DDC_CNTRL_REG[4];
assign SYNC_CIC = DDC_CNTRL_REG[5];
assign SYNC_NCO = DDC_CNTRL_REG[6];

assign DDC_A = A[2:0]; 

// DDC addressing decode
wire decode_ddc0;
wire decode_ddc1;

// valid range 0x6000 - 0x6007
assign decode_ddc0 = ((A13 == 1'b1) && (A[10:0] <= 6'd15) && (~WR_N | ~RD_N)) ? 1'b1 : 1'b0;
// valid range 0x6010 - 0x6017
assign decode_ddc1 = ((A13 == 1'b1) && (A[10:0] <= 6'd23) && (A[10:0] > 6'd15) && (~WR_N | ~RD_N)) ? 1'b1 : 1'b0;

assign CS_DDC0_N = ~decode_ddc0;
assign CS_DDC1_N = ~decode_ddc1;
assign DDC_RD_N = RD_N;
assign DDC_WR_N = WR_N;

endmodule // init_load
