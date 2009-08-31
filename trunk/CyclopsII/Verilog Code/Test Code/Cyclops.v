// V1.1  27 AUG 2009 --- Modified by Graham Haddock, KE9H
//      Modified to revise VCO1 and VCO2 reference frequencies,
//      VCO2 offset, rule that B >= A, where N=B*P+A, and
//      that VCO1 has negative CP polarity.
//
// V1.0  22 July 2009
//
// Copyright 2009 Phil Harman VK6APH
//
//  HPSDR - High Performance Software Defined Radio
//
//
//  Cyclops Spectum Analyzer
//
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


/*  Test program for ADF4112 PLL chips on Cyclops PCB.

Analog Devices ADF4112

External Reference Clock = 10 MHz for Atlas C16

Nomenclature:
The AFD4112 has four control latches, which are addressed
by the two least significant bits in the 24 bit SPI control word.

That is, the register address is implicit in the SPI control word.

RLC: "Reference Counter Latch"
NCL: "N Counter Latch", also interchangably referred to as "A B Counter Latch"
FNL: "Function Latch"    (Same control bit map as INL, but does not reset chip.)
INL: "Initialization Latch" (Same control bit map as FNL, but also resets chip.)

==

Initialization Sequence: (Three 24 bit SPI control words)
Write to INL, then RCL, then NCL.

The write to INL puts the chip into reset;
writing to NCL releases it to run.

Once it is running and you want to change something in the INL/FNL
address control space, you address it as the FNL, not the INL.

=============================================================================

    Connections

    Atlas   FPGA
             24     IFCLK from FX2 at 48MHz
    A13     134     ADF4112_2_SPI_clock
    A14     133     ADF4112_SPI_data
    A15     128     LE2                 // Second LO
    A16     127     MUX2                // Second LO
    A17     118     LE1                 // First LO
    A19     116     MUX1                // First LO

    Change log
    22 July 2009  - Port previous LMX2326 code to run ADF4112
                  - Test program for ADF4112 PLL chips on Cyclops PCB,
                        1st LO = 1030MHz, 2nd LO = 1126MHz
*/

/*

//       FX2 pin    to   FPGA pin connections
//
//       IFCLK                  - pin 24
//       FX2_CLK                - pin 23
//       FX2_FD[0]              - pin 56
//       FX2_FD[1]              - pin 57
//       FX2_FD[2]              - pin 58
//       FX2_FD[3]              - pin 59
//       FX2_FD[4]              - pin 60
//       FX2_FD[5]              - pin 61
//       FX2_FD[6]              - pin 63
//       FX2_FD[7]              - pin 64
//       FX2_FD[8]              - pin 208
//       FX2_FD[9]              - pin 207
//       FX2_FD[10]             - pin 206
//       FX2_FD[11]             - pin 205
//       FX2_FD[12]             - pin 203
//       FX2_FD[13]             - pin 201
//       FX2_FD[14]             - pin 200
//       FX2_FD[15]             - pin 199
//       FLAGA                  - pin 198
//       FLAGB                  - pin 197
//       FLAGC                  - pin 5
//       SLOE                   - pin 13
//       FIFO_ADR[0]            - pin 11
//       FIFO_ADR[1]            - pin 10
//       PKEND                  - pin 8
//       SLRD                   - pin 30
//       SLWR                   - pin 31
//

*/

//////////////////////////////////////////////////////////////
//
//              Quartus V9.0 Notes
//
//////////////////////////////////////////////////////////////

/*
    Quartus V9.0 -  use the following settings:

    - Analysis and Synthesis Settings\
        Power Up Dont Care [not checked]
    - Analysis and Synthesis Settings\More Settings
        Synthesis options for State Machine Processing  = User Encoded

*/

module Cyclops (
                input   IFCLK,                  // 48MHz clock from FX2
                //input   [15:0]ADC,            // LTC2208 data out
                //input   FPGA_CLK6IN,          // 122.88MHz ADC clock
                output  ADF4112_SPI_clock,      // SPI clock to ADF4112s
                output  ADF4112_SPI_data,       // SPI data to ADF4112s
                output  LE1,                    // Data latch, First LO
                output  LE2,                    // Data latch, Second LO
                //input   MUX1,                 // MUX out from First LO
                //input   MUX2                  // MUX out from Second LO
                output  DEBUG_LED0              // debug LEDs on Ozy board
                //output  DEBUG_LED1,
                //output  DEBUG_LED2,
                //output  DEBUG_LED3
                //inout  [15:0] FX2_FD,         // bidirectional FIFO data to/from the FX2
                //input     FLAGA,
                //input     FLAGB,
                //input     FLAGC,
                //output    reg SLWR,           // FX2 write - active low
                //output    reg SLRD,           // FX2 read - active low
                //output    reg SLOE,           // FX2 data bus enable - active low
                //output    PKEND,
                //output  reg [1:0] FIFO_ADR    // FX2 register address
                );

////////////////////////////////////////////////////////////////////////
//
//  ADF4112 configuration
//
////////////////////////////////////////////////////////////////////////


/* *****************************************************************************
 *  Initialization Latches
 *
 *  INL:    Normal operation
 *          Digital lock detect on MUXOUT
 *          Positive CP polarity for VCO2, Negative for VCO1
 *          No "fast lock" at this time (future implementation)
 *          CP current = 2.5 mA.
 *          Prescaler P=16
 */

parameter P2    = 1'b0;
parameter P1    = 1'b1;     // prescaler 16/17
parameter PD2   = 1'b0;
parameter CP16  = 1'b1;     // Increase current for "fast lock"
//
parameter CP15  = 1'b1;     // Fast lock not currently implemented
parameter CP14  = 1'b1;     // Icp = 2.5mA 
parameter CP13  = 1'b0;
parameter CP12  = 1'b1;
//
parameter CP11  = 1'b1;     // R = 4.7k
parameter TC4   = 1'b1;
parameter TC3   = 1'b0;
parameter TC2   = 1'b0;
//
parameter TC1   = 1'b0;     // TIMEOUT PFD = 31 cycles
parameter F5    = 1'b0;
parameter F4    = 1'b0;     // fast lock disabled
parameter F3    = 1'b0;     // outout normal
//
parameter F2_VCO1   = 1'b0;     // Charge Pump VCO1 = negative
parameter F2_VCO2   = 1'b1;     // Charge Pump VCO2 = positive
parameter M3    = 1'b0;
parameter M2    = 1'b0;
parameter M1    = 1'b1;     // Digital lock detect on MUXOUT
//
parameter PD1   = 1'b0;     // normal operation
parameter F1    = 1'b0;     // Counter operation - normal
parameter C2    = 1'b1;
parameter C1    = 1'b1;

parameter Second_LO_INL = {P2,P1,PD2,CP16,CP15,CP14,CP13,CP12,CP11,TC4,TC3,TC2,TC1,F5,F4,F3,F2_VCO2,M3,M2,M1,PD1,F1,C2,C1}; // 24'h5DC093

// Note:  F2 is 0 for first local oscillator since loop amplifier inverts
parameter First_LO_INL  = {P2,P1,PD2,CP16,CP15,CP14,CP13,CP12,CP11,TC4,TC3,TC2,TC1,F5,F4,F3,F2_VCO1,M3,M2,M1,PD1,F1,C2,C1};  // 24'h5DC013


/* *****************************************************************************
 *  Local Oscillator 2 -- Reference Counter Latch
 *
 *  RCL:    Reference divisor = 20 (Internal ref = 0.5 MHz.)
 *          Anti Backlash pulse = 3 ns. (I picked the middle value)
 *          Lock detect precision = High
 *          Delay Sync = Normal
 */

parameter LO2_BD23   = 1'b0;        //don't care
parameter LO2_DLY    = 1'b0;
parameter LO2_SYNC   = 1'b0;        // normal operation
parameter LO2_LDP    = 1'b1;        // Lock detect precision = High
parameter LO2_T1     = 1'b0;
parameter LO2_T2     = 1'b0;        // test mode = normal
parameter LO2_ABP2   = 1'b0;
parameter LO2_ABP1   = 1'b0;        // Anti Backlash pulse = 3 ns
parameter [13:0]LO2_R = 14'b00_0000_0001_0100;  // divide by 20

parameter Second_LO_RCL = {LO2_DB23,LO2_DLY,LO2_SYNC,LO2_LDP,LO2_T1,LO2_T2,LO2_ABP2,LO2_ABP1,LO2_R,2'b00};  //24'h100050;


/* *****************************************************************************
 *  Local Oscillator 2 -- N Counter Latch
 *
 *  Internal divider range ( N = B*P+A ) where B >= A
 *
 *      N = 2253  for fixed 1126.5 MHz.
 *
 *  If P = 16, and the upper two bits of A are held at 00,
 *  then B concatenated with the lower four bits of A can be considered
 *  a single binary 17 bit "N" register.
 *
 *  where:
 *  B13 B12 B11 | B10 B09 B08 B07 | B06 B05 B04 B03 | B02 B01 A6 A5 | A4 A3 A2 A1
 *   ----------- Upper bits of N control word -------------- | 0  0 | lower nybble
 *
 *  N = 2253 = $008CD => 000 0010 0011 0000 1101
 *
 */

parameter LO2_DB23    = 1'b0;       // don't care
parameter LO2_DB22    = 1'b0;       // don't care
parameter LO2_G1      = 1'b0;       // setting 1 is used
parameter [18:0]LO2_BA = 19'b000_0010_0011_0000_1101;  // 2253, P=16

parameter Second_LO_NCL = {LO2_DB23, LO2_DB22, LO2_G1, LO2_BA, 2'b01};
                                // 24'h008C35 For fixed frequency of 1126.5 MHz


/* *****************************************************************************
 *  Local Oscillator 1 -- Reference Counter Latch
 *
 *  RCL:    Reference divisor = 10 (Internal ref = 1.0 MHz.)
 *          Anti Backlash pulse = 3 ns. (I picked the middle value)
 *          Lock detect precision = High
 *          Delay Sync = Normal
 */

parameter LO1_BD23   = 1'b0;        //don't care
parameter LO1_DLY    = 1'b0;
parameter LO1_SYNC   = 1'b0;        // normal operation
parameter LO1_LDP    = 1'b1;        // Lock detect precision = High
parameter LO1_T1     = 1'b0;
parameter LO1_T2     = 1'b0;        // test mode = normal
parameter LO1_ABP2   = 1'b0;
parameter LO1_ABP1   = 1'b0;        // Anti Backlash pulse = 3 ns
parameter [13:0]LO1_R = 14'b00_0000_0000_1010;      // divide by 10

parameter First_LO_RCL = {LO1_DB23,LO1_DLY,LO1_SYNC,LO1_LDP,LO1_T1,LO1_T2,LO1_ABP2,LO1_ABP1,LO1_R,2'b00};   // RCL: 24'h100028


/* *****************************************************************************
 *  Local Oscillator 1 -- N Counter Latch
 *
 *  Desired output: 1030.0 (Test mode)
 *                  1030 to 2029 MHz in one MHz steps (Operational)
 *
 *  Internal Reference = 1.0 MHz.
 *  Reference counter = divide by 10  (R = 10)
 *
 *  Internal divider range ( N = B*P+A )   where B >= A
 *
 *          N = 1030 for test mode.
 *          N = 1030 through 2029 (sequential integers) for operational mode.
 *
 *  If P = 16, and the upper two bits of A are held at 00,
 *  then B concatenated with the lower four bits of A can be considered
 *  a single binary 17 bit "N" register.
 *
 *  where
 *  B13 B12 B11 | B10 B09 B08 B07 | B06 B05 B04 B03 | B02 B01 A6 A5 | A4 A3 A2 A1
 *   ----------------- Upper bits of N control word -------- | 0  0 | lower nybble
 *
 *  N = 1030 = $00406 => 000 0001 0000 0000 0110
 *  N = 1031 = $00407 => 000 0001 0000 0000 0111
 *   ..
 *  N = 1040 = $00410 => 000 0001 0000 0100 0000
 *   ..
 *  N = 2029 = $007ED => 000 0001 1111 1000 1101
 *
 */

parameter LO1_DB23    = 1'b0;       // don't care
parameter LO1_DB22    = 1'b0;       // don't care
parameter LO1_G1      = 1'b0;       // setting 1 is used
parameter [18:0]LO1_BA = 19'b000_0001_0000_0000_0110;  // N=1030 for P=16

parameter First_LO_NCL = {LO1_DB23, LO1_DB22, LO1_G1, LO1_BA, 2'b01};  // 24'h004019;



////////////////////////////////////////////////////////////////////////
//
//      Reset and enable code
//
////////////////////////////////////////////////////////////////////////

// holds clk_enable = 0 for first 1024 clock counts


reg clk_enable;

reg [10:0]reset_count;

//always @ (posedge ADC_clock) begin
//  if (reset_count[10]) begin
//      clk_enable <= 1'b1;     // clk_enable high to run
//      end
//  else begin
//      clk_enable <= 0;
//      reset_count <= reset_count + 1'b1;
//  end
//end


//////////////////////////////////////////////////
//
//  Clocks
//
//////////////////////////////////////////////////

//wire ADC_clock;
//assign ADC_clock = FPGA_CLK6IN;


// SPI Clock - 48/10  = 4.8MHZ

reg clock;
reg [2:0]spi_clock;

always @ (posedge IFCLK)
begin
    if (spi_clock == 4)begin
        spi_clock <= 0;
        clock <= ~clock;
    end
    else spi_clock <= spi_clock + 1'b1;
end

///////////////////////////////////////////////////////////////////
//
// Set 2nd Local Oscillator ADF4112 to 1126MHz using SPI interface
//
///////////////////////////////////////////////////////////////////
wire [23:0]SPI_DATA;  // 24 bit wide data and address register to ADF4112
reg [23:0]Second_LO_data;

wire  LE;

reg [3:0]SPI_state;
reg start1, start2;

always @ (posedge clock)
begin
case (SPI_state)

// Write to INL register
0:  begin
    if (ready) begin            // send ADSF4113 data
        start2 <= 1'b1;
        Second_LO_data  <= Second_LO_INL;
        SPI_state <= SPI_state + 1'b1;
        end
    else SPI_state <= 0;        // loop until SPI module ready
    end
// delay so that SPI module see the start signal
2:  begin
    start2 <= 1'b0;             // stop sending data
    SPI_state <= SPI_state + 1'b1;
    end

// Write to RCL register
3:  begin
    if (ready) begin
        start2 <= 1;
        Second_LO_data <= Second_LO_RCL;
        SPI_state <= SPI_state + 1'b1;
        end
    else SPI_state <= 3;        // loop until SPI module ready
    end
// delay
5:  begin
    start2 <= 1'b0;
    SPI_state <= SPI_state + 1'b1;
    end
// Write to NCL register
6:  begin
    if (ready) begin
        start2 <= 1;
        Second_LO_data <= Second_LO_NCL;
        SPI_state <= SPI_state + 1'b1;
        end
    else SPI_state <= 6;
    end
//delay
8: begin
    start2 <= 1'b0;
    SPI_state <= SPI_state + 1'b1;
    end
// wait for data to be sent
9:  begin
    if (ready) SPI_state <= SPI_state + 1'b1;
    else SPI_state <= 9;
    end
// all data sent so loop here
10: SPI_state <= 6;
default: SPI_state <= SPI_state + 1'b1;
endcase
end

////////////////////////////////////////////////////////////
//
// First Local Oscillator ADF4112 to 1030MHz using SPI interface
//
///////////////////////////////////////////////////////////

/*
// The frequency to set the First LO to comes from the PC
// and is in 16 bit register Rx_register.  Need to extend this to
// 18 bits and send to LMX2326

wire [17:0]PLL_frequency = {2'b00, Rx_register};
*/

// for first test set first LO to 1030MHz

reg [23:0]First_LO_data;

reg [3:0]SPI2_state;

always @ (posedge clock)
begin
case (SPI2_state)

// Write to INL register, don't run until LO 2 has been setup
0:  begin
    if (ready && SPI_state == 11) begin         // send ADF4112  data
        start1 <= 1'b1;
        First_LO_data <= First_LO_INL;
        SPI2_state <= SPI2_state + 1'b1;
        end
    else SPI2_state <= 0;       // loop until SPI module ready
    end
// delay so SPI module can see the ready flag

// Write to RCL register
2:  begin
    start1 <= 1'b0;             // stop sending data
    SPI2_state <= SPI2_state + 1'b1;
    end

// send data
3:  begin
    if (ready) begin
        start1 <= 1;
        First_LO_data <= First_LO_RCL;
        SPI2_state <= SPI2_state + 1'b1;
        end
    else SPI2_state <= 3;       // loop until SPI module ready
    end
// delay
// Write to NCL register
5:  begin
    start1 <= 1'b0;
    SPI2_state <= SPI2_state + 1'b1;
    end

// send data
6:  begin
    if (ready) begin
        start1 <= 1;
        First_LO_data <= First_LO_NCL;
        SPI2_state <= SPI2_state + 1'b1;
        end
    else SPI2_state <= 6;
    end
// delay
// stop
8:  begin
    start1 <= 1'b0;
    SPI2_state <= 8;  // loop here once programming completed
    end
default: SPI2_state <= SPI2_state + 1'b1;
endcase
end

// assign data and latch enable depending on which ADF4112 we are addressing
assign SPI_DATA = (SPI_state < 10) ? Second_LO_data : First_LO_data;
assign LE1 = (SPI_state == 10) ? LE : 1'b1;
assign LE2 = (SPI_state < 10)  ? LE : 1'b1;

wire ready;
// SPI interface to ADF4112s
ADF4112_SPI ADF4112(.clock(clock),.start(start1 || start2),.ready(ready),.data(SPI_DATA),
                    .SPI_clock(ADF4112_SPI_clock),.SPI_LE(LE),.SPI_data(ADF4112_SPI_data));


/*

////////////////////////////////////////////////////////////////////
//
//  Get data from ADC, derandomize and send to FIFO
//
////////////////////////////////////////////////////////////////////


// A Digital Output Randomizer is fitted to the LT2208. This complements bits 15 to 1 if
// bit 0 is 1. This helps to reduce any pickup by the A/D input of the digital outputs.
// We need to de-ramdomize the LT2208 data if this is turned on.


reg [15:0]temp_ADC;

always @ (posedge ADC_clock)
begin
    if (ADC[0]) temp_ADC <= {~ADC[15:1],ADC[0]};
    else temp_ADC <= ADC;
end

//////////////////////////////////////////////////////////////
//
//      CORDIC NCO
//
//////////////////////////////////////////////////////////////

//Code rotates A/D input at set frequency  and produces I & Q
wire [17:0]i_out;
wire [17:0]q_out;
wire[31:0] phase;
reg [31:0]frequency;

// frequency = f * 2^32/clock
// The phase accumulator takes a 32 bit frequency dword and outputs a 32 bit phase dword on each clock.
// Set to 26.88MHz i.e. h38000000
phase_accumulator rx_phase_accumulator(.clk(~ADC_clock),.reset(~clk_enable),.frequency(32'h38000000),.phase_out(phase));


// set i_in to 0 for real inputs
cordic_VK6APH cordic (.i_in(16'd0), .q_in(temp_ADC), .iout(i_out), .qout(q_out), .ain(phase[31:12]), .clk(ADC_clock));

// apply CIC filter and decimate

//wire signed [15:0]cic_out_i;
//wire signed [15:0]cic_out_q;
wire signed [23:0]cic_out_i_1;
wire signed [23:0]cic_out_q_1;
wire signed [23:0]cic_out_i_2;
wire signed [23:0]cic_out_q_2;
wire signed [23:0]cic_out_i;
wire signed [23:0]cic_out_q;
wire ce_out_i_1;
wire ce_out_q_1;
wire ce_out_i_2;
wire ce_out_q_2;
wire ce_out_i;
wire ce_out_q;


//first CIC - decimate by 60, 10th order - clock out is 2.048MHz
//cic_60_1_10_nofraction cic_I_1( .clk(ADC_clock),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(i_out),.filter_out(cic_out_i),.ce_out(ce_out_i));
//cic_60_1_10_nofraction cic_Q_1( .clk(ADC_clock),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(q_out),.filter_out(cic_out_q),.ce_out(ce_out_q));


//first CIC - decimate by 10, 3rd order - clock out is 12.288MHz
cic_10_1_3_nofraction cic_I_1( .clk(ADC_clock),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(i_out),.filter_out(cic_out_i_1),.ce_out(ce_out_i_1));
cic_10_1_3_nofraction cic_Q_1( .clk(ADC_clock),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(q_out),.filter_out(cic_out_q_1),.ce_out(ce_out_q_1));


//second CIC - decimate by 8, 5th order - clock out is 1.5625MHz
//cic_8_1_5_nofraction cic_I_2( .clk(ce_out_i_1),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(cic_out_i_1),.filter_out(cic_out_i_2),.ce_out(ce_out_i_2));
//cic_8_1_5_nofraction cic_Q_2( .clk(ce_out_q_1),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(cic_out_q_1),.filter_out(cic_out_q_2),.ce_out(ce_out_q_2));

//cic_8_1_5_nofraction cic_I_2( .clk(ADC_clock),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in({i_out,6'b0}),.filter_out(cic_out_i_1),.ce_out(ce_out_i_1));
//cic_8_1_5_nofraction cic_Q_2( .clk(ADC_clock),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in({q_out,6'b0}),.filter_out(cic_out_q_1),.ce_out(ce_out_q_1));


//third CIC - decimate by 4, 8th order - clock out is 390.625kHz
cic_4_1_8_nofraction cic_I_3( .clk(ce_out_i_1),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(cic_out_i_1),.filter_out(cic_out_i),.ce_out(ce_out_i));
cic_4_1_8_nofraction cic_Q_3( .clk(ce_out_q_1),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(cic_out_q_1),.filter_out(cic_out_q),.ce_out(ce_out_q));

//////////////////////////////////////////////////////////////////////
//
//      CFIR
//
//////////////////////////////////////////////////////////////////////

// compensates for sinx/x response of CIC, provides ultimage rejection and decimates by 2
// output data rate is 192kHz

wire signed [47:0]FIR_i_out;
wire signed [47:0]FIR_q_out;
wire FIR_strobe;

FIR_top  FIR(.clk(ADC_clock),.reset(~clk_enable),.data_in_I(cic_out_i),.data_in_Q(cic_out_q),.strobe_in(ce_out_i ),.data_out_I(FIR_i_out),.data_out_Q(FIR_q_out),.strobe_out(FIR_strobe));


// concatinate I&Q
//wire [31:0]fifo_data = {cic_out_i, cic_out_q};
//wire [31:0]fifo_data = {cic_out_i_2[23:8], cic_out_q_2[23:8]};
//wire [31:0]fifo_data = {cic_out_i[23:8],cic_out_q[23:8] };
//wire [31:0]fifo_data = {16'd100, 16'd200};

wire [31:0]fifo_data = {FIR_i_out[47:32],FIR_q_out[47:32]};


// Keep loading ADC data, a total of 4096 32 bit  concecutive words, to write side of FIFO until it is full.
// Wait until all data has been sent to FX2 and then repeat.

reg flag;
always @ (negedge FIR_strobe)
begin
    if (wrempty)wrreq <= 1'b1;          // enable FIFO write
    else if (wrfull)wrreq <= 1'b0;      // disable FIFO write
end


// 16 x 4096 data FIFO
reg rdreq; reg wrreq; wire wrfull; wire [15:0]q; wire wrempty;wire [11:0] write_used;
FIFO data_fifo(.data(fifo_data),.rdclk(IFCLK),.rdreq(rdreq),.wrclk(FIR_strobe),.wrreq(wrreq),
               .q(q),.wrempty(wrempty),.wrfull(wrfull));


//////////////////////////////////////////////////////////////
//
//  State Machine to manage FX2 USB interface
//
//////////////////////////////////////////////////////////////
/*
        FX2 interface - read LO frequency from PC code and send ADC data

        The state machine checks if there are characters to be read
        from the FX2 Rx FIFO by checking 'EP2_has_data'  If set it loads the word
        read into the Rx_register. Then it sends a word from the data FIFO to the FX2 Tx FIFO.
        After the Tx word has been sent it loops.
*/

/*
// set up FX2 Port A
assign PKEND = 1'b1;

wire EP2_has_data = FLAGA;            // high when EP2 has data available
wire EP6_ready = FLAGC;               // high when we can write to EP6
reg [3:0] state_FX;                   // state for FX2
reg [15:0] Rx_register;               // data from PC goes here
reg [15:0] Tx_register;               // data to PC goes here
reg SLEN;                             // Put data on FX2 bus
reg sync_write_not_empty;             // FIFO write side empty synced to FX2 clock


always @ (negedge IFCLK)
begin
sync_write_not_empty <= !wrempty;       // sync to FX2 clock
case(state_FX)
// state 0 - set up to check for Rx data from EP2
4'd0:begin
    SLWR <= 1;                    // reset FX2 FIFO write stobe
    SLRD <= 1'b1;
    SLOE <= 1'b1;
    SLEN <= 1'b0;
    FIFO_ADR <= 2'b00;            // select EP2
    state_FX <= state_FX + 1'b1;
    end
// delay 2 IFCLOCK cycle, this is necessary at 48MHZ to allow FIFO_ADR to settle
// check for Rx data
4'd2:begin
    if(EP2_has_data)begin
       state_FX <= state_FX + 1'b1;
       SLOE <= 1'b0;                  //assert SLOE
       end
    else begin
       state_FX <= 4'd6;              // No Rx data so check for Tx data
       end
    end
// Wait 2 IFCLK before we assert SLRD then load received data
4'd4:begin
        SLRD <= 1'b0;
        Rx_register[15:8] <= FX2_FD[7:0];  //  swap endian
        Rx_register[7:0]  <= FX2_FD[15:8];
        state_FX <= state_FX + 1'b1;
        end
// reset SLRD and SLOE
4'd5:begin
        SLRD <= 1'b1;
        SLOE <= 1'b1;
        state_FX <= state_FX + 1'b1;
        end

// Tx data here - send data in FIFO
4'd6:begin
        SLWR <= 1;
        state_FX <= state_FX + 1'b1;
        FIFO_ADR <= 2'b10;                  // select EP6
    end

// Wait 2 IFCLK for FIFO_ADR to stabilize, the get FIFO data and assert SLWR
4'd7:   begin
        rdreq <= 1'b1;                      // set FIFO read required true
        state_FX <= state_FX + 1'b1;
        end

4'd8:   begin
        rdreq <= 1'b0;                      // set FIFO read requried false
        state_FX <= state_FX + 1'b1;
        end

// check Tx FIFO is ready then set Write strobe
4'd9:begin
    if (EP6_ready && sync_write_not_empty) begin  // if EP6 is ready, write to it and exit this state
    //if (EP6_ready && syncd_write_used[11]) begin
        SLEN <= 1'b1;
        state_FX <= state_FX + 1'b1;
        end
    else state_FX <= 4'd9;                      // otherwise, hang out here until fifo is ready
    end
//  set SLWR
4'd10: begin
       SLWR <= 1'b0;
       state_FX <= state_FX + 1'b1;
       end
//  reset SLWR, tristate SLEN and loop back to receive
4'd11: begin
        SLWR <= 1;
        SLEN <= 1'b0;
        state_FX <= 4'd0;
        end

default: state_FX <= state_FX + 1'b1;
endcase
end

// FX2_FD is tristate when SLEN  is low, otherwise it's the RAM data.
// NOTE: Did not swap endian due to way data is read in PC program.

assign FX2_FD = SLEN ? q  : 16'bZ;

*/



//  Test LEDs
assign DEBUG_LED0 = ~ready;     // On when SPI module is ready
//assign DEBUG_LED1 = ~wrempty;
//assign DEBUG_LED2 = ~wrfull;
//assign DEBUG_LED3 = ~Rx_register[3];


endmodule