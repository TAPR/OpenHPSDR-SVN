//
// Copyright (c) Bill Tracey 2006 (bill@ewjt.com)
//
// Simple test load for the Ozy FPGA -- takes in a clock, divides it down via a register and feeds it out to all the FPGA pins so one can
// validate board wiring
//


module OzyTestImageTop(
     input   FX2_CLK,
    output ATLAS_X26DC,
    output ATLAS_X25DC,
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
    output ATLAS_X14B6,
    output ATLAS_X13B5,
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
    output ATLAS_Y26DC
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

always @ ( posedge  FX2_CLK ) begin
        clock_reg <= clock_reg + 1;
end

assign ATLAS_X26DC = clock_reg[4];            // 4 768 khz
assign ATLAS_X25DC = clock_reg[5];            // 5 384 khz
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



endmodule
