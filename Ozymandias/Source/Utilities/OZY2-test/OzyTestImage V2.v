//
// Copyright (c) Bill Tracey 2006 (bill@ewjt.com)
//
// Simple test load for the Ozy FPGA -- takes in a clock, divides it down via a register and feeds it out to all the FPGA pins so one can
// validate board wiring
//
// 2 December 2006 - Modified for V2 of the OZY board, clock now 24MHz from FX2_CLK - VK6APH
// 10 December 2006 - added GPIO pins except 17-24 since theses are inputs - VK6APH


module OzyTestImageTop(
     input   FX2_CLK,
    output ATLAS_A2,
    output ATLAS_A3,
    output ATLAS_A4,
    output ATLAS_A5,
	output ATLAS_A6,
	output ATLAS_A7,
	output ATLAS_A8,
	output ATLAS_A9,
	output ATLAS_A10,
	output ATLAS_A11,
	output ATLAS_A12,
	output ATLAS_A13,
	output ATLAS_A14,
	output ATLAS_A15,
	output ATLAS_A16,
	output ATLAS_A17,
	output ATLAS_A18,
	output ATLAS_A19,
	output ATLAS_A20,
	output ATLAS_A21,
	output ATLAS_A22,
	output ATLAS_A23,
	output ATLAS_A24,
	output ATLAS_A25,
	output ATLAS_A27,
	output ATLAS_A29,
	output ATLAS_A31,
	output ATLAS_C2,
    output ATLAS_C3,
    output ATLAS_C4,
    output ATLAS_C5,
	output ATLAS_C6,
	output ATLAS_C7,
	output ATLAS_C8,
	output ATLAS_C9,
	output ATLAS_C10,
	output ATLAS_C11,
	output ATLAS_C12,
	output ATLAS_C13,
	output ATLAS_C14,
	output ATLAS_C15,
	output ATLAS_C16,
	output ATLAS_C17,
	output ATLAS_C18,
	output ATLAS_C19,
	output ATLAS_C20,
	output ATLAS_C21,
	output ATLAS_C22,
	output ATLAS_C23,
	output ATLAS_C24,
	output ATLAS_C25,
	output ATLAS_C27,
	output ATLAS_C29,
	output ATLAS_C31,
	output DEBUG_LED0,
	output DEBUG_LED1,
	output DEBUG_LED2,
	output DEBUG_LED3,
	output FPGA_GPIO1,
	output FPGA_GPIO2,
	output FPGA_GPIO3,
	output FPGA_GPIO4,
	output FPGA_GPIO5,
	output FPGA_GPIO6,
	output FPGA_GPIO7,
	output FPGA_GPIO8,
	output FPGA_GPIO9,
	output FPGA_GPIO10,
	output FPGA_GPIO11,
	output FPGA_GPIO12,
	output FPGA_GPIO13,
	output FPGA_GPIO14,
	output FPGA_GPIO15,
	output FPGA_GPIO16
); 

        // divided down clock signals

        // 0 24 Mhz
        // 1 12 Mhz
        // 2 6 Mhz
        // 3 3 Mhz
        // 4 1.5 Mhz
        // 5 750 khz



reg [19:0] clock_reg;

always @ ( posedge  FX2_CLK ) begin
        clock_reg <= clock_reg + 20'b1;
end

assign ATLAS_A2 = clock_reg[4];            // 4 
assign ATLAS_A3 = clock_reg[5];            // 5 
assign ATLAS_A4 = clock_reg[8];            // 8  
assign ATLAS_A5 = clock_reg[9];            // 9 
assign ATLAS_A6 = clock_reg[10];           // 10 
assign ATLAS_A7 = clock_reg[11];           // 11 
assign ATLAS_A8 = clock_reg[12];           // 12 
assign ATLAS_A9 = clock_reg[13];           // 13 
assign ATLAS_A10 = clock_reg[14];           // 14  
assign ATLAS_A11 = clock_reg[15];           // 15 
assign ATLAS_A12 = clock_reg[16];           // 16 
assign ATLAS_A13 = clock_reg[17];           // 17 
assign ATLAS_A14 = clock_reg[2];            // 2 
assign ATLAS_A15 = clock_reg[3];            // 3 
assign ATLAS_A16 = clock_reg[8];            // 8  
assign ATLAS_A17 = clock_reg[9];            // 9 
assign ATLAS_A18 = clock_reg[10];           // 10 
assign ATLAS_A19 = clock_reg[11];            // 11 
assign ATLAS_A20 = clock_reg[12];            // 12 
assign ATLAS_A21 = clock_reg[13];            // 13 
assign ATLAS_A22 = clock_reg[14];            // 14  
assign ATLAS_A23 = clock_reg[15];            // 15 
assign ATLAS_A24 = clock_reg[16];            // 16 
assign ATLAS_A25 = clock_reg[17];            // 17 
assign ATLAS_A27 = clock_reg[18];            // 18 
assign ATLAS_A29 = clock_reg[19];            // 19 
assign ATLAS_A31 = clock_reg[0];             // 0 
assign ATLAS_C2 = clock_reg[4];            // 4 
assign ATLAS_C3 = clock_reg[5];            // 5 
assign ATLAS_C4 = clock_reg[8];            // 8  
assign ATLAS_C5 = clock_reg[9];            // 9 
assign ATLAS_C6 = clock_reg[10];           // 10 
assign ATLAS_C7 = clock_reg[11];           // 11 
assign ATLAS_C8 = clock_reg[12];           // 12 
assign ATLAS_C9 = clock_reg[13];           // 13 
assign ATLAS_C10 = clock_reg[14];           // 14  
assign ATLAS_C11 = clock_reg[15];           // 15 
assign ATLAS_C12 = clock_reg[16];           // 16 
assign ATLAS_C13 = clock_reg[17];           // 17 
assign ATLAS_C14 = clock_reg[2];            // 2 
assign ATLAS_C15 = clock_reg[3];            // 3 
assign ATLAS_C16 = clock_reg[8];            // 8  
assign ATLAS_C17 = clock_reg[9];            // 9 
assign ATLAS_C18 = clock_reg[10];           // 10 
assign ATLAS_C19 = clock_reg[11];            // 11 
assign ATLAS_C20 = clock_reg[12];            // 12 
assign ATLAS_C21 = clock_reg[13];            // 13 
assign ATLAS_C22 = clock_reg[14];            // 14  
assign ATLAS_C23 = clock_reg[15];            // 15 
assign ATLAS_C24 = clock_reg[16];            // 16 
assign ATLAS_C25 = clock_reg[17];            // 17 
assign ATLAS_C27 = clock_reg[18];            // 18 
assign ATLAS_C29 = clock_reg[19];            // 19 
assign ATLAS_C31 = clock_reg[0];             // 0 
assign FPGA_GPIO1 = clock_reg[2]; 
assign FPGA_GPIO2 = clock_reg[3];
assign FPGA_GPIO3 = clock_reg[8];
assign FPGA_GPIO4 = clock_reg[9];
assign FPGA_GPIO5 = clock_reg[10];
assign FPGA_GPIO6 = clock_reg[11];
assign FPGA_GPIO7 = clock_reg[12];
assign FPGA_GPIO8 = clock_reg[13];
assign FPGA_GPIO9 = clock_reg[14];
assign FPGA_GPIO10 = clock_reg[15];
assign FPGA_GPIO11 = clock_reg[16];
assign FPGA_GPIO12 = clock_reg[17];
assign FPGA_GPIO13 = clock_reg[18];
assign FPGA_GPIO14 = clock_reg[19];
assign FPGA_GPIO15 = clock_reg[0];
assign FPGA_GPIO16 = clock_reg[2]; 


// LEDs
assign DEBUG_LED0 = 0; // on
assign DEBUG_LED1 = 0; // on
assign DEBUG_LED2 = 0; // on
assign DEBUG_LED3 = 0; // on


endmodule
