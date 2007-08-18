//
// Copyright (c) Bill Tracey KD5TFD  2006 (bill@ewjt.com) Phil Harman 2007 VK6APH (pvharman@arach.net.au)
//
// Simple test program for the Penny FPGA -- takes in a clock, divides it down via a register and 
// feeds it out to all the FPGA pins so one can validate board wiring.

// Select either the 10MHz or 125MHz clock source.

// WARNING: Since this code tests all FPGA pins and sets them as outputs make sure no other devices are loaded
// on the PCB before running this code. 


module PennyTestImage(
   	input  _125MHZ,
	input  _10MHZ,
	output A2,
	output A3,
	output A4,
	output A5,
	output A6,
	output A7,
	output A8,
	output A9,
	output A10,
	output A11,
	output A12,
	output A13,
	output A14,
	output A15,
	output A16,
	output A17,
	output A18,
	output A19,
	output A20,
	output A21,
	output A22,
//	output A23,  JTAG
//	output A24,   "
//	output A25,   "
//	output A27,   "
//	output A29,   "
	output A31,
	output C2,
	output C3,
	output C4,
	output C5,
	output C6,
	output C7,
	output C8,
	output C9,
	output C10,
	output C11,
	output C12,
	output C13,
	output C14,
	output C15,
	output C16,
	output C17,
	output C18,
	output C19,
	output C20,
	output C21,
	output C22,
	output C23,
	output C24,
	output C25,
	output C27,
	output C29,
	output C31,
	output LED2,
	output LED3,
	output LED4,
	output LED5,
	output LED6,
	output LED7,
	output FPGA_PTT,
	output USROUT0,
	output USROUT1,
	output USROUT2,
	output USROUT3,
	output USROUT4,
	output USROUT5,
	output USROUT6,
	output FPGA_PLL,
	output DACCLK,
	output DACD0,
	output DACD1,
	output DACD2,
	output DACD3,
	output DACD4,
	output DACD5, 
	output DACD6, 
	output DACD7,
	output DACD8,
	output DACD9,
	output DACD10,
	output DACD11,
	output DACD12,
	output DACD13,
	output MOSI,
	output SSCK,
	output CMODE,
	output CDIN,
	output CDOUT,
	output CBCLK,
	output CLRCIN,
	output CLRCOUT, 
	output CMCLK, 
	output ADCMOSI,
	output ADCCLK,
	output ADCMISO,
	output ADCCCS,
	output PWM0,
	output PWM1,
	output PWM2,
	output LVDSCLK,
	output nLVDSRXE,
	output LVDSTXE 

	); 

        // divided down clock signals using 125MHz clock

        // 0 62.5MHz
        // 1 31.25MHz
        // 2 15.625MHz
        // 3 7.8MHz
        // 4 3.9MHz
        // 5 1.9MHz
        // 6 976kHz
        // 7 488kHz
        // 8 244kHz
        // 9 122kHz
        // 10 61kHz
        // 11 30.5kHz
        // 12 15.25kHz
        // 13  7.63kHz
        // 14  3.8kHz
        // 15  1.9kHz
        // 16  953Hz
        // 17  477Hz
        // 18  238Hz
		// 19  119Hz
		// 20   59Hz
		// 21   29Hz
		// 22   14Hz
		// 23    7Hz
		// 24   3.5Hz
		// 25   1.8Hz
		// 26     1Hz
		// 27   0.5Hz


reg [27:0] clock_reg;

always @ ( posedge  _125MHZ ) begin
//always @ ( posedge  _10MHZ ) begin
        clock_reg <= clock_reg + 1'b1;
end

	assign A2 = clock_reg[0];
	assign A3 = clock_reg[1];
	assign A4 = clock_reg[2];
	assign A5 = clock_reg[3];
	assign A6 = clock_reg[4];
	assign A7 = clock_reg[5];
	assign A8 = clock_reg[6];
	assign A9 = clock_reg[7];
	assign A10 = clock_reg[8];
	assign A11 = clock_reg[9];
	assign A12 = clock_reg[10];
	assign A13 = clock_reg[11];
	assign A14 = clock_reg[12];
	assign A15 = clock_reg[13];
	assign A16 = clock_reg[14];
	assign A17 = clock_reg[15];
	assign A18 = clock_reg[16];
	assign A19 = clock_reg[17];
	assign A20 = clock_reg[18];
	assign A21 = clock_reg[19];
	assign A22 = clock_reg[20];
//	output A23,  JTAG
//	output A24,   "
//	output A25,   "
//	output A27,   "
//	output A29,   "
	assign A31 = clock_reg[21];
	assign C2 = clock_reg[22];
	assign C3 = clock_reg[23];
	assign C4 = clock_reg[24];
	assign C5 = clock_reg[25];
	assign C6 = clock_reg[26];
	assign C7 = clock_reg[27];
	assign C8 = clock_reg[0];
	assign C9 = clock_reg[1];
	assign C10 = clock_reg[2];
	assign C11 = clock_reg[3];
	assign C12 = clock_reg[4];
	assign C13 = clock_reg[5];
	assign C14 = clock_reg[6];
	assign C15 = clock_reg[7];
	assign C16 = clock_reg[8];
	assign C17 = clock_reg[9];
	assign C18 = clock_reg[10];
	assign C19 = clock_reg[11];
	assign C20 = clock_reg[12];
	assign C21 = clock_reg[13];
	assign C22 = clock_reg[14];
	assign C23 = clock_reg[15];
	assign C24 = clock_reg[16];
	assign C25 = clock_reg[17];
	assign C27 = clock_reg[18];
	assign C29 = clock_reg[19];
	assign C31 = clock_reg[20];
	assign LED2 = clock_reg[20];
	assign LED3 = clock_reg[21];
	assign LED4 = clock_reg[22];
	assign LED5 = clock_reg[23];
	assign LED6 = clock_reg[24];
	assign LED7 = clock_reg[25];
	assign FPGA_PTT = clock_reg[23];
	assign USROUT0 = clock_reg[22];
	assign USROUT1 = clock_reg[21];
	assign USROUT2 = clock_reg[20];
	assign USROUT3 = clock_reg[19];
	assign USROUT4 = clock_reg[18];
	assign USROUT5 = clock_reg[17];
	assign USROUT6 = clock_reg[16];
	assign FPGA_PLL = clock_reg[15];
	assign DACCLK = clock_reg[15];
	assign DACD0 = clock_reg[14];
	assign DACD1 = clock_reg[13];
	assign DACD2 = clock_reg[12];
	assign DACD3 = clock_reg[11];
	assign DACD4 = clock_reg[10];
	assign DACD5 = clock_reg[9];
	assign DACD6 = clock_reg[8];
	assign DACD7 = clock_reg[7];
	assign DACD8 = clock_reg[6];
	assign DACD9 = clock_reg[5];
	assign DACD10 = clock_reg[4];
	assign DACD11 = clock_reg[3];
	assign DACD12 = clock_reg[2];
	assign DACD13 = clock_reg[2];
	assign MOSI = clock_reg[2];
	assign SSCK = clock_reg[2];
	assign CMODE = clock_reg[2];
	assign CDIN = clock_reg[2];
	assign CDOUT = clock_reg[2];
	assign CBCLK = clock_reg[2];
	assign CLRCIN = clock_reg[2];
	assign CLRCOUT = clock_reg[2];
	assign CMCLK = clock_reg[2];
	assign ADCMOSI = clock_reg[2];
	assign ADCCLK = clock_reg[2];
	assign ADCMISO = clock_reg[2];
	assign ADCCCS = clock_reg[2];
	assign PWM0 = clock_reg[2];
	assign PWM1 = clock_reg[2];
	assign PWM2 = clock_reg[2];
	assign LVDSCLK = clock_reg[2];
	assign nLVDSRXE = clock_reg[2];
	assign LVDSTXE = clock_reg[2];

endmodule
