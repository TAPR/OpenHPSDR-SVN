//
// Copyright (c) Bill Tracey 2006 (bill@ewjt.com)
//
// Simple test load for the Ozy FPGA -- takes in a clock, divides it down via a register and feeds it out to all the FPGA pins so one can
// validate board wiring
//


module OzyTestImageTop(
    input FPGA_CLK6IN,
    // output FPGA_ASD0,
    // output FPGA_NCS0,
    output FX2_PC6,
    output FX2_CTL3,
    output FX2_CTL2,
    output FX2_PA7,
    output FX2_PA6,
    output FX2_PA5,
    output FX2_PA4,
    output FX2_PA3,
    output FX2_PA2,
    output FX2_PA1,
    output FX2_PA0,
    // output FX2_CLK,
    // output FX2_IFCLK,
    // output FPGA_CLK2IN,
    // output FPGA_CLK3IN,
    output FX2_RDY0,
    output FX2_RDY1,
    output FX2_RDY2,
    output FX2_RDY3,
    output FX2_RDY4,
    output FX2_RDY5,
    output FX2_PE0,
    output FX2_PE1,
    output FX2_PE2,
    output FX2_PE3,
    output FX2_PE4,
    output FX2_PE5,
    output FX2_PE6,
    output FX2_PE7,
    output FPGA_CLKXOUT,
    output FPGA_CLKYOUT,
    output FX2_PB0,
    output FX2_PB1,
    output FX2_PB2,
    output FX2_PB3,
    output FX2_PB4,
    output FX2_PB5,
    output FX2_PB6,
    output FX2_PB7,
    output FPGA_GPIO1,
    output FPGA_GPIO2,
    output FPGA_GPIO3,
    output FPGA_GPIO4,
    output FPGA_GPIO5,
    output FPGA_GPIO6,
    output FPGA_GPIO7,
    output FPGA_GPIO8,
    output FPGA_GPIO9,
    output FPGA_GPIO15,
    output FPGA_GPIO16,
    output FPGA_GPIO17,
    output FPGA_GPIO18,
    output FPGA_GPIO19,
    output FPGA_GPIO10,
    output FPGA_GPIO11,
    output FPGA_GPIO12,
    output FPGA_GPIO13,
    output FPGA_GPIO14,
    output FX2_INT4,
    output FX2_TO,
    output FPGA_TXD,
    output FPGA_RXD,
    output ATLAS_X26DC,
    output ATLAS_X25DC,
    output FPGA_INIT_DONE,
    output FX2_T1,
    output ATLAS_X24DC,
    output ATLAS_X23C7,
    output ATLAS_X22C6,
    output ATLAS_X21C5,
    output ATLAS_X20C4,
    output ATLAS_X19C3,
    output ATLAS_X18C2,
    output ATLAS_X17C1,
    output ATLAS_X16C0,
    output ATLAS_X15B7,
    // output FPGA_NSTATUS,
    // output FPGA_CONFIG_DONE,
    // output FPGA_MSEL1,
    // output FPGA_MSEL0,
    output ATLAS_X14B6,
    output ATLAS_X13B5,
    // output FPGA_CLK7IN,
    // output FPGA_CLK5IN,
    // output FPGA_CLK4IN,
    output ATLAS_X12B4,
    output ATLAS_X11B3,
    output ATLAS_X10B2,
    output ATLAS_X9B1,
    output ATLAS_X8B0,
    output ATLAS_X7A7,
    output ATLAS_X6A6,
    output ATLAS_X5A5,
    output ATLAS_X4A4,
    output ATLAS_X3A3,
    output ATLAS_X2A2,
    output ATLAS_X1A1,
    output ATLAS_X0A0,
    output ATLAS_Y0A0,
    output ATLAS_Y1A1,
    output ATLAS_Y2A2,
    output ATLAS_Y3A3,
    output ATLAS_Y4A4,
    output ATLAS_Y5A5,
    output ATLAS_Y6A6,
    output ATLAS_Y7A7,
    output ATLAS_Y8B0,
    output ATLAS_Y9B1,
    output ATLAS_Y10B2,
    output ATLAS_Y11B3,
    output ATLAS_Y12B4,
    output ATLAS_Y13B5,
    output ATLAS_Y14B6,
    output ATLAS_Y15B7,
    output ATLAS_Y16C0,
    output ATLAS_Y17C1,
    output ATLAS_Y18C2,
    output ATLAS_Y19C3,
    output ATLAS_Y20C4,
    output ATLAS_Y21C5,
    output ATLAS_Y22C6,
    output ATLAS_Y23C7,
    output ATLAS_Y24DC,
    output ATLAS_Y25DC,
    output ATLAS_Y26DC,
    output FX2_CTL1,
    output FX2_CTL0,
    output FX2_PD7,
    output FX2_PD6,
    output FX2_PD5,
    output FX2_PD4,
    output FX2_PD3,
    output FX2_PD2,
    output FX2_PD1,
    output FX2_PD0,
    output FPGA_GPIO20,
    output FPGA_GPIO21,
    output FPGA_GPIO22,
    output FPGA_GPIO23,
    output FPGA_GPIO24
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

always @ ( posedge  FPGA_CLK6IN ) begin
        clock_reg <= clock_reg + 1;
end

assign FX2_PC6 = clock_reg[2];                // 2 3.072 Mhz
assign FX2_CTL3 = clock_reg[3];               // 3 1.536
assign FX2_CTL2 = clock_reg[4];               // 4 768 khz
assign FX2_PA7 = clock_reg[5];                // 5 384 khz
assign FX2_PA6 = clock_reg[6];                // 6 192khz
assign FX2_PA5 = clock_reg[7];                // 7  96khz
assign FX2_PA4 = clock_reg[8];                // 8  48khz
assign FX2_PA3 = clock_reg[9];                // 9 24khz
assign FX2_PA2 = clock_reg[10];               // 10 12khz
assign FX2_PA1 = clock_reg[11];               // 11 6khz
assign FX2_PA0 = clock_reg[12];               // 12 3khz
assign FX2_RDY0 = clock_reg[17];              // 17 93.75hz
assign FX2_RDY1 = clock_reg[18];              // 18 46.875hz
assign FX2_RDY2 = clock_reg[19];              // 19 23.4375 hz
assign FX2_RDY3 = clock_reg[0];               // 0 12.288 Mhz
assign FX2_RDY4 = clock_reg[1];               // 1 6.144 Mhz
assign FX2_RDY5 = clock_reg[2];               // 2 3.072 Mhz
assign FX2_PE0 = clock_reg[3];                // 3 1.536
assign FX2_PE1 = clock_reg[4];                // 4 768 khz
assign FX2_PE2 = clock_reg[5];                // 5 384 khz
assign FX2_PE3 = clock_reg[6];                // 6 192khz
assign FX2_PE4 = clock_reg[7];                // 7  96khz
assign FX2_PE5 = clock_reg[8];                // 8  48khz
assign FX2_PE6 = clock_reg[9];                // 9 24khz
assign FX2_PE7 = clock_reg[10];               // 10 12khz
assign FPGA_CLKXOUT = clock_reg[11];          // 11 6khz
assign FPGA_CLKYOUT = clock_reg[12];          // 12 3khz
assign FX2_PB0 = clock_reg[13];               // 13 1.5khz
assign FX2_PB1 = clock_reg[14];               // 14  750hz
assign FX2_PB2 = clock_reg[15];               // 15 375hz
assign FX2_PB3 = clock_reg[16];               // 16 187.5hz
assign FX2_PB4 = clock_reg[17];               // 17 93.75hz
assign FX2_PB5 = clock_reg[18];               // 18 46.875hz
assign FX2_PB6 = clock_reg[19];               // 19 23.4375 hz
assign FX2_PB7 = clock_reg[0];                // 0 12.288 Mhz
assign FPGA_GPIO1 = clock_reg[1];             // 1 6.144 Mhz
assign FPGA_GPIO2 = clock_reg[2];             // 2 3.072 Mhz
assign FPGA_GPIO3 = clock_reg[3];             // 3 1.536
assign FPGA_GPIO4 = clock_reg[4];             // 4 768 khz
assign FPGA_GPIO5 = clock_reg[5];             // 5 384 khz
assign FPGA_GPIO6 = clock_reg[6];             // 6 192khz
assign FPGA_GPIO7 = clock_reg[7];             // 7  96khz
assign FPGA_GPIO8 = clock_reg[8];             // 8  48khz
assign FPGA_GPIO9 = clock_reg[9];             // 9 24khz
assign FPGA_GPIO15 = clock_reg[10];           // 10 12khz
assign FPGA_GPIO16 = clock_reg[11];           // 11 6khz
assign FPGA_GPIO17 = clock_reg[12];           // 12 3khz
assign FPGA_GPIO18 = clock_reg[13];           // 13 1.5khz
assign FPGA_GPIO19 = clock_reg[14];           // 14  750hz
assign FPGA_GPIO10 = clock_reg[15];           // 15 375hz
assign FPGA_GPIO11 = clock_reg[16];           // 16 187.5hz
assign FPGA_GPIO12 = clock_reg[17];           // 17 93.75hz
assign FPGA_GPIO13 = clock_reg[18];           // 18 46.875hz
assign FPGA_GPIO14 = clock_reg[19];           // 19 23.4375 hz
assign FX2_INT4 = clock_reg[0];               // 0 12.288 Mhz
assign FX2_TO = clock_reg[1];                 // 1 6.144 Mhz
assign FPGA_TXD = clock_reg[2];               // 2 3.072 Mhz
assign FPGA_RXD = clock_reg[3];               // 3 1.536
assign ATLAS_X26DC = clock_reg[4];            // 4 768 khz
assign ATLAS_X25DC = clock_reg[5];            // 5 384 khz
assign FPGA_INIT_DONE = clock_reg[6];         // 6 192khz
assign FX2_T1 = clock_reg[7];                 // 7  96khz
assign ATLAS_X24DC = clock_reg[8];            // 8  48khz
assign ATLAS_X23C7 = clock_reg[9];            // 9 24khz
assign ATLAS_X22C6 = clock_reg[10];           // 10 12khz
assign ATLAS_X21C5 = clock_reg[11];           // 11 6khz
assign ATLAS_X20C4 = clock_reg[12];           // 12 3khz
assign ATLAS_X19C3 = clock_reg[13];           // 13 1.5khz
assign ATLAS_X18C2 = clock_reg[14];           // 14  750hz
assign ATLAS_X17C1 = clock_reg[15];           // 15 375hz
assign ATLAS_X16C0 = clock_reg[16];           // 16 187.5hz
assign ATLAS_X15B7 = clock_reg[17];           // 17 93.75hz
assign ATLAS_X14B6 = clock_reg[2];            // 2 3.072 Mhz
assign ATLAS_X13B5 = clock_reg[3];            // 3 1.536
// assign FPGA_CLK6IN = clock_reg[5];            // 5 384 khz
assign FPGA_CLK5IN = clock_reg[6];            // 6 192khz
assign FPGA_CLK4IN = clock_reg[7];            // 7  96khz
assign ATLAS_X12B4 = clock_reg[8];            // 8  48khz
assign ATLAS_X11B3 = clock_reg[9];            // 9 24khz
assign ATLAS_X10B2 = clock_reg[10];           // 10 12khz
assign ATLAS_X9B1 = clock_reg[11];            // 11 6khz
assign ATLAS_X8B0 = clock_reg[12];            // 12 3khz
assign ATLAS_X7A7 = clock_reg[13];            // 13 1.5khz
assign ATLAS_X6A6 = clock_reg[14];            // 14  750hz
assign ATLAS_X5A5 = clock_reg[15];            // 15 375hz
assign ATLAS_X4A4 = clock_reg[16];            // 16 187.5hz
assign ATLAS_X3A3 = clock_reg[17];            // 17 93.75hz
assign ATLAS_X2A2 = clock_reg[18];            // 18 46.875hz
assign ATLAS_X1A1 = clock_reg[19];            // 19 23.4375 hz
assign ATLAS_X0A0 = clock_reg[0];             // 0 12.288 Mhz
assign ATLAS_Y0A0 = clock_reg[1];             // 1 6.144 Mhz
assign ATLAS_Y1A1 = clock_reg[2];             // 2 3.072 Mhz
assign ATLAS_Y2A2 = clock_reg[3];             // 3 1.536
assign ATLAS_Y3A3 = clock_reg[4];             // 4 768 khz
assign ATLAS_Y4A4 = clock_reg[5];             // 5 384 khz
assign ATLAS_Y5A5 = clock_reg[6];             // 6 192khz
assign ATLAS_Y6A6 = clock_reg[7];             // 7  96khz
assign ATLAS_Y7A7 = clock_reg[8];             // 8  48khz
assign ATLAS_Y8B0 = clock_reg[9];             // 9 24khz
assign ATLAS_Y9B1 = clock_reg[10];            // 10 12khz
assign ATLAS_Y10B2 = clock_reg[11];           // 11 6khz
assign ATLAS_Y11B3 = clock_reg[12];           // 12 3khz
assign ATLAS_Y12B4 = clock_reg[13];           // 13 1.5khz
assign ATLAS_Y13B5 = clock_reg[14];           // 14  750hz
assign ATLAS_Y14B6 = clock_reg[15];           // 15 375hz
assign ATLAS_Y15B7 = clock_reg[16];           // 16 187.5hz
assign ATLAS_Y16C0 = clock_reg[17];           // 17 93.75hz
assign ATLAS_Y17C1 = clock_reg[18];           // 18 46.875hz
assign ATLAS_Y18C2 = clock_reg[19];           // 19 23.4375 hz
assign ATLAS_Y19C3 = clock_reg[0];            // 0 12.288 Mhz
assign ATLAS_Y20C4 = clock_reg[1];            // 1 6.144 Mhz
assign ATLAS_Y21C5 = clock_reg[2];            // 2 3.072 Mhz
assign ATLAS_Y22C6 = clock_reg[3];            // 3 1.536
assign ATLAS_Y23C7 = clock_reg[4];            // 4 768 khz
assign ATLAS_Y24DC = clock_reg[5];            // 5 384 khz
assign ATLAS_Y25DC = clock_reg[6];            // 6 192khz
assign ATLAS_Y26DC = clock_reg[7];            // 7  96khz
assign FX2_CTL1 = clock_reg[8];               // 8  48khz
assign FX2_CTL0 = clock_reg[9];               // 9 24khz
assign FX2_PD7 = clock_reg[10];               // 10 12khz
assign FX2_PD6 = clock_reg[11];               // 11 6khz
assign FX2_PD5 = clock_reg[12];               // 12 3khz
assign FX2_PD4 = clock_reg[13];               // 13 1.5khz
assign FX2_PD3 = clock_reg[14];               // 14  750hz
assign FX2_PD2 = clock_reg[15];               // 15 375hz
assign FX2_PD1 = clock_reg[16];               // 16 187.5hz
assign FX2_PD0 = clock_reg[17];               // 17 93.75hz
assign FPGA_GPIO20 = clock_reg[18];           // 18 46.875hz
assign FPGA_GPIO21 = clock_reg[19];           // 19 23.4375 hz
assign FPGA_GPIO22 = clock_reg[00];           // 0 12.288 Mhz
assign FPGA_GPIO23 = clock_reg[01];           // 1 6.144 Mhz
assign FPGA_GPIO24 = clock_reg[02];           // 2 3.072 Mhz







endmodule
