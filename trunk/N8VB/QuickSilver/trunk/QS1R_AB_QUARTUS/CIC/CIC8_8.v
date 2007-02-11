// -------------------------------------------------------------
//
// Module: CIC8_8
//
// Generated by MATLAB(R) 7.2 and the Filter Design HDL Coder 1.4.
//
// Generated on: 2007-02-08 16:38:49
//
// -------------------------------------------------------------

// -------------------------------------------------------------
// HDL Code Generation Options:
//
// TargetLanguage: Verilog
// Name: CIC8_8
// TargetDirectory: C:\Data\QS\QuickSilver\trunk\QS1R_AB_QUARTUS\CIC
// OptimizeForHDL: On
// SerialPartition: -1
// InlineConfigurations: Off
// SafeZeroConcat: Off
// CastBeforeSum: On
// TestBenchStimulus: step ramp chirp noise 
//
// Filter Settings:
//
// Discrete-Time FIR Multirate Filter (real)
// -----------------------------------------
// Filter Structure        : Cascaded Integrator-Comb Decimator
// Decimation Factor       : 8
// Differential Delay      : 1
// Number of Sections      : 8
// Stable                  : Yes
// Linear Phase            : Yes (Type 1)
//
// Input                   : s32,15
// Output                  : s56,15
// Filter Internals        : Full Precision
//   Integrator Section 1  : s56,15
//   Integrator Section 2  : s56,15
//   Integrator Section 3  : s56,15
//   Integrator Section 4  : s56,15
//   Integrator Section 5  : s56,15
//   Integrator Section 6  : s56,15
//   Integrator Section 7  : s56,15
//   Integrator Section 8  : s56,15
//   Comb Section 1        : s56,15
//   Comb Section 2        : s56,15
//   Comb Section 3        : s56,15
//   Comb Section 4        : s56,15
//   Comb Section 5        : s56,15
//   Comb Section 6        : s56,15
//   Comb Section 7        : s56,15
//   Comb Section 8        : s56,15
//
// Implementation Cost
// Number of Multipliers : 0
// Number of Adders      : 16
// Number of States      : 16
// MultPerInputSample    : 0
// AddPerInputSample     : 9
// -------------------------------------------------------------

`timescale 1 ns / 1 ns

module CIC8_8 
               (
                clk,
                clk_enable,
                reset,
                filter_in,
                filter_out,
                ce_out
                );

  input   clk; 
  input   clk_enable; 
  input   reset; 
  input   signed [31:0] filter_in; //sfix32_En15
  output  signed [55:0] filter_out; //sfix56_En15
  output  ce_out; 

////////////////////////////////////////////////////////////////
//Module Architecture: CIC8_8
////////////////////////////////////////////////////////////////
  // Local Functions
  // Type Definitions
  // Constants
  // Signals
  reg  [2:0] cur_count; // ufix3
  wire phase_1; // boolean
  reg  ce_out_reg; // boolean
  //   
  reg  signed [31:0] input_register; // sfix32_En15
  //   -- Section 1 Signals 
  wire signed [31:0] section_in1; // sfix32_En15
  wire signed [55:0] section_cast1; // sfix56_En15
  wire signed [55:0] sum1; // sfix56_En15
  reg  signed [55:0] section_out1; // sfix56_En15
  wire signed [55:0] add_cast; // sfix56_En15
  wire signed [55:0] add_cast_1; // sfix56_En15
  wire signed [56:0] add_temp; // sfix57_En15
  //   -- Section 2 Signals 
  wire signed [55:0] section_in2; // sfix56_En15
  wire signed [55:0] sum2; // sfix56_En15
  reg  signed [55:0] section_out2; // sfix56_En15
  wire signed [55:0] add_cast_2; // sfix56_En15
  wire signed [55:0] add_cast_3; // sfix56_En15
  wire signed [56:0] add_temp_1; // sfix57_En15
  //   -- Section 3 Signals 
  wire signed [55:0] section_in3; // sfix56_En15
  wire signed [55:0] sum3; // sfix56_En15
  reg  signed [55:0] section_out3; // sfix56_En15
  wire signed [55:0] add_cast_4; // sfix56_En15
  wire signed [55:0] add_cast_5; // sfix56_En15
  wire signed [56:0] add_temp_2; // sfix57_En15
  //   -- Section 4 Signals 
  wire signed [55:0] section_in4; // sfix56_En15
  wire signed [55:0] sum4; // sfix56_En15
  reg  signed [55:0] section_out4; // sfix56_En15
  wire signed [55:0] add_cast_6; // sfix56_En15
  wire signed [55:0] add_cast_7; // sfix56_En15
  wire signed [56:0] add_temp_3; // sfix57_En15
  //   -- Section 5 Signals 
  wire signed [55:0] section_in5; // sfix56_En15
  wire signed [55:0] sum5; // sfix56_En15
  reg  signed [55:0] section_out5; // sfix56_En15
  wire signed [55:0] add_cast_8; // sfix56_En15
  wire signed [55:0] add_cast_9; // sfix56_En15
  wire signed [56:0] add_temp_4; // sfix57_En15
  //   -- Section 6 Signals 
  wire signed [55:0] section_in6; // sfix56_En15
  wire signed [55:0] sum6; // sfix56_En15
  reg  signed [55:0] section_out6; // sfix56_En15
  wire signed [55:0] add_cast_10; // sfix56_En15
  wire signed [55:0] add_cast_11; // sfix56_En15
  wire signed [56:0] add_temp_5; // sfix57_En15
  //   -- Section 7 Signals 
  wire signed [55:0] section_in7; // sfix56_En15
  wire signed [55:0] sum7; // sfix56_En15
  reg  signed [55:0] section_out7; // sfix56_En15
  wire signed [55:0] add_cast_12; // sfix56_En15
  wire signed [55:0] add_cast_13; // sfix56_En15
  wire signed [56:0] add_temp_6; // sfix57_En15
  //   -- Section 8 Signals 
  wire signed [55:0] section_in8; // sfix56_En15
  wire signed [55:0] sum8; // sfix56_En15
  reg  signed [55:0] section_out8; // sfix56_En15
  wire signed [55:0] add_cast_14; // sfix56_En15
  wire signed [55:0] add_cast_15; // sfix56_En15
  wire signed [56:0] add_temp_7; // sfix57_En15
  //   -- Section 9 Signals 
  wire signed [55:0] section_in9; // sfix56_En15
  reg  signed [55:0] diff1; // sfix56_En15
  wire signed [55:0] section_out9; // sfix56_En15
  wire signed [55:0] sub_cast; // sfix56_En15
  wire signed [55:0] sub_cast_1; // sfix56_En15
  wire signed [56:0] sub_temp; // sfix57_En15
  //   -- Section 10 Signals 
  wire signed [55:0] section_in10; // sfix56_En15
  reg  signed [55:0] diff2; // sfix56_En15
  wire signed [55:0] section_out10; // sfix56_En15
  wire signed [55:0] sub_cast_2; // sfix56_En15
  wire signed [55:0] sub_cast_3; // sfix56_En15
  wire signed [56:0] sub_temp_1; // sfix57_En15
  //   -- Section 11 Signals 
  wire signed [55:0] section_in11; // sfix56_En15
  reg  signed [55:0] diff3; // sfix56_En15
  wire signed [55:0] section_out11; // sfix56_En15
  wire signed [55:0] sub_cast_4; // sfix56_En15
  wire signed [55:0] sub_cast_5; // sfix56_En15
  wire signed [56:0] sub_temp_2; // sfix57_En15
  //   -- Section 12 Signals 
  wire signed [55:0] section_in12; // sfix56_En15
  reg  signed [55:0] diff4; // sfix56_En15
  wire signed [55:0] section_out12; // sfix56_En15
  wire signed [55:0] sub_cast_6; // sfix56_En15
  wire signed [55:0] sub_cast_7; // sfix56_En15
  wire signed [56:0] sub_temp_3; // sfix57_En15
  //   -- Section 13 Signals 
  wire signed [55:0] section_in13; // sfix56_En15
  reg  signed [55:0] diff5; // sfix56_En15
  wire signed [55:0] section_out13; // sfix56_En15
  wire signed [55:0] sub_cast_8; // sfix56_En15
  wire signed [55:0] sub_cast_9; // sfix56_En15
  wire signed [56:0] sub_temp_4; // sfix57_En15
  //   -- Section 14 Signals 
  wire signed [55:0] section_in14; // sfix56_En15
  reg  signed [55:0] diff6; // sfix56_En15
  wire signed [55:0] section_out14; // sfix56_En15
  wire signed [55:0] sub_cast_10; // sfix56_En15
  wire signed [55:0] sub_cast_11; // sfix56_En15
  wire signed [56:0] sub_temp_5; // sfix57_En15
  //   -- Section 15 Signals 
  wire signed [55:0] section_in15; // sfix56_En15
  reg  signed [55:0] diff7; // sfix56_En15
  wire signed [55:0] section_out15; // sfix56_En15
  wire signed [55:0] sub_cast_12; // sfix56_En15
  wire signed [55:0] sub_cast_13; // sfix56_En15
  wire signed [56:0] sub_temp_6; // sfix57_En15
  //   -- Section 16 Signals 
  wire signed [55:0] section_in16; // sfix56_En15
  reg  signed [55:0] diff8; // sfix56_En15
  wire signed [55:0] section_out16; // sfix56_En15
  wire signed [55:0] sub_cast_14; // sfix56_En15
  wire signed [55:0] sub_cast_15; // sfix56_En15
  wire signed [56:0] sub_temp_7; // sfix57_En15
  //   
  reg  signed [55:0] output_register; // sfix56_En15

  // Block Statements
  //   ------------------ CE Output Generation ------------------

  always @ (posedge clk or posedge reset)
    begin: ce_output
      if (reset == 1'b1) begin
        cur_count <= 3'b000;
      end
      else begin
        if (clk_enable == 1'b1) begin
          if (cur_count == 3'b111) begin
            cur_count <= 3'b000;
          end
          else begin
            cur_count <= cur_count + 1;
          end
        end
      end
    end // ce_output

  assign  phase_1 = (cur_count == 3'b001 && clk_enable == 1'b1)? 1 : 0;

  //   ------------------ CE Output Register ------------------

  always @ (posedge clk or posedge reset)
    begin: ce_output_register
      if (reset == 1'b1) begin
        ce_out_reg <= 1'b0;
      end
      else begin
          ce_out_reg <= phase_1;
      end
    end // ce_output_register

  //   ------------------ Input Register ------------------

  always @ (posedge clk or posedge reset)
    begin: input_reg_process
      if (reset == 1'b1) begin
        input_register <= 0;
      end
      else begin
        if (clk_enable == 1'b1) begin
          input_register <= filter_in;
        end
      end
    end // input_reg_process

  //   ------------------ Section # 1 : Integrator ------------------

  assign section_in1 = input_register;

  assign section_cast1 = $signed({{24{section_in1[31]}}, section_in1});

  assign add_cast = section_cast1;
  assign add_cast_1 = section_out1;
  assign add_temp = add_cast + add_cast_1;
  assign sum1 = add_temp[55:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section1
      if (reset == 1'b1) begin
        section_out1 <= 0;
      end
      else begin
        if (clk_enable == 1'b1) begin
          section_out1 <= sum1;
        end
      end
    end // integrator_delay_section1

  //   ------------------ Section # 2 : Integrator ------------------

  assign section_in2 = section_out1;

  assign add_cast_2 = section_in2;
  assign add_cast_3 = section_out2;
  assign add_temp_1 = add_cast_2 + add_cast_3;
  assign sum2 = add_temp_1[55:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section2
      if (reset == 1'b1) begin
        section_out2 <= 0;
      end
      else begin
        if (clk_enable == 1'b1) begin
          section_out2 <= sum2;
        end
      end
    end // integrator_delay_section2

  //   ------------------ Section # 3 : Integrator ------------------

  assign section_in3 = section_out2;

  assign add_cast_4 = section_in3;
  assign add_cast_5 = section_out3;
  assign add_temp_2 = add_cast_4 + add_cast_5;
  assign sum3 = add_temp_2[55:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section3
      if (reset == 1'b1) begin
        section_out3 <= 0;
      end
      else begin
        if (clk_enable == 1'b1) begin
          section_out3 <= sum3;
        end
      end
    end // integrator_delay_section3

  //   ------------------ Section # 4 : Integrator ------------------

  assign section_in4 = section_out3;

  assign add_cast_6 = section_in4;
  assign add_cast_7 = section_out4;
  assign add_temp_3 = add_cast_6 + add_cast_7;
  assign sum4 = add_temp_3[55:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section4
      if (reset == 1'b1) begin
        section_out4 <= 0;
      end
      else begin
        if (clk_enable == 1'b1) begin
          section_out4 <= sum4;
        end
      end
    end // integrator_delay_section4

  //   ------------------ Section # 5 : Integrator ------------------

  assign section_in5 = section_out4;

  assign add_cast_8 = section_in5;
  assign add_cast_9 = section_out5;
  assign add_temp_4 = add_cast_8 + add_cast_9;
  assign sum5 = add_temp_4[55:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section5
      if (reset == 1'b1) begin
        section_out5 <= 0;
      end
      else begin
        if (clk_enable == 1'b1) begin
          section_out5 <= sum5;
        end
      end
    end // integrator_delay_section5

  //   ------------------ Section # 6 : Integrator ------------------

  assign section_in6 = section_out5;

  assign add_cast_10 = section_in6;
  assign add_cast_11 = section_out6;
  assign add_temp_5 = add_cast_10 + add_cast_11;
  assign sum6 = add_temp_5[55:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section6
      if (reset == 1'b1) begin
        section_out6 <= 0;
      end
      else begin
        if (clk_enable == 1'b1) begin
          section_out6 <= sum6;
        end
      end
    end // integrator_delay_section6

  //   ------------------ Section # 7 : Integrator ------------------

  assign section_in7 = section_out6;

  assign add_cast_12 = section_in7;
  assign add_cast_13 = section_out7;
  assign add_temp_6 = add_cast_12 + add_cast_13;
  assign sum7 = add_temp_6[55:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section7
      if (reset == 1'b1) begin
        section_out7 <= 0;
      end
      else begin
        if (clk_enable == 1'b1) begin
          section_out7 <= sum7;
        end
      end
    end // integrator_delay_section7

  //   ------------------ Section # 8 : Integrator ------------------

  assign section_in8 = section_out7;

  assign add_cast_14 = section_in8;
  assign add_cast_15 = section_out8;
  assign add_temp_7 = add_cast_14 + add_cast_15;
  assign sum8 = add_temp_7[55:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section8
      if (reset == 1'b1) begin
        section_out8 <= 0;
      end
      else begin
        if (clk_enable == 1'b1) begin
          section_out8 <= sum8;
        end
      end
    end // integrator_delay_section8

  //   ------------------ Section # 9 : Comb ------------------

  assign section_in9 = section_out8;

  assign sub_cast = section_in9;
  assign sub_cast_1 = diff1;
  assign sub_temp = sub_cast - sub_cast_1;
  assign section_out9 = sub_temp[55:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section9
      if (reset == 1'b1) begin
        diff1 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff1 <= section_in9;
        end
      end
    end // comb_delay_section9

  //   ------------------ Section # 10 : Comb ------------------

  assign section_in10 = section_out9;

  assign sub_cast_2 = section_in10;
  assign sub_cast_3 = diff2;
  assign sub_temp_1 = sub_cast_2 - sub_cast_3;
  assign section_out10 = sub_temp_1[55:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section10
      if (reset == 1'b1) begin
        diff2 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff2 <= section_in10;
        end
      end
    end // comb_delay_section10

  //   ------------------ Section # 11 : Comb ------------------

  assign section_in11 = section_out10;

  assign sub_cast_4 = section_in11;
  assign sub_cast_5 = diff3;
  assign sub_temp_2 = sub_cast_4 - sub_cast_5;
  assign section_out11 = sub_temp_2[55:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section11
      if (reset == 1'b1) begin
        diff3 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff3 <= section_in11;
        end
      end
    end // comb_delay_section11

  //   ------------------ Section # 12 : Comb ------------------

  assign section_in12 = section_out11;

  assign sub_cast_6 = section_in12;
  assign sub_cast_7 = diff4;
  assign sub_temp_3 = sub_cast_6 - sub_cast_7;
  assign section_out12 = sub_temp_3[55:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section12
      if (reset == 1'b1) begin
        diff4 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff4 <= section_in12;
        end
      end
    end // comb_delay_section12

  //   ------------------ Section # 13 : Comb ------------------

  assign section_in13 = section_out12;

  assign sub_cast_8 = section_in13;
  assign sub_cast_9 = diff5;
  assign sub_temp_4 = sub_cast_8 - sub_cast_9;
  assign section_out13 = sub_temp_4[55:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section13
      if (reset == 1'b1) begin
        diff5 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff5 <= section_in13;
        end
      end
    end // comb_delay_section13

  //   ------------------ Section # 14 : Comb ------------------

  assign section_in14 = section_out13;

  assign sub_cast_10 = section_in14;
  assign sub_cast_11 = diff6;
  assign sub_temp_5 = sub_cast_10 - sub_cast_11;
  assign section_out14 = sub_temp_5[55:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section14
      if (reset == 1'b1) begin
        diff6 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff6 <= section_in14;
        end
      end
    end // comb_delay_section14

  //   ------------------ Section # 15 : Comb ------------------

  assign section_in15 = section_out14;

  assign sub_cast_12 = section_in15;
  assign sub_cast_13 = diff7;
  assign sub_temp_6 = sub_cast_12 - sub_cast_13;
  assign section_out15 = sub_temp_6[55:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section15
      if (reset == 1'b1) begin
        diff7 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff7 <= section_in15;
        end
      end
    end // comb_delay_section15

  //   ------------------ Section # 16 : Comb ------------------

  assign section_in16 = section_out15;

  assign sub_cast_14 = section_in16;
  assign sub_cast_15 = diff8;
  assign sub_temp_7 = sub_cast_14 - sub_cast_15;
  assign section_out16 = sub_temp_7[55:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section16
      if (reset == 1'b1) begin
        diff8 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff8 <= section_in16;
        end
      end
    end // comb_delay_section16

  //   ------------------ Output Register ------------------

  always @ (posedge clk or posedge reset)
    begin: output_reg_process
      if (reset == 1'b1) begin
        output_register <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          output_register <= section_out16;
        end
      end
    end // output_reg_process

  // Assignment Statements
  assign ce_out = ce_out_reg;
  assign filter_out = output_register;
endmodule  // CIC8_8
