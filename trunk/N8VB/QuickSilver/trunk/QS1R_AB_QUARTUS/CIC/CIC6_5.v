// -------------------------------------------------------------
//
// Module: CIC6_5
//
// Generated by MATLAB(R) 7.2 and the Filter Design HDL Coder 1.4.
//
// Generated on: 2007-01-26 14:32:56
//
// -------------------------------------------------------------

// -------------------------------------------------------------
// HDL Code Generation Options:
//
// TargetLanguage: Verilog
// Name: CIC6_5
// TargetDirectory: C:\Data\QS\QuickSilver\trunk\QS1R_AB_QUARTUS\CIC
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
// Decimation Factor       : 5
// Differential Delay      : 1
// Number of Sections      : 6
// Stable                  : Yes
// Linear Phase            : Yes (Type 1)
//
// Input                   : s28,15
// Output                  : s42,15
// Filter Internals        : Full Precision
//   Integrator Section 1  : s42,15
//   Integrator Section 2  : s42,15
//   Integrator Section 3  : s42,15
//   Integrator Section 4  : s42,15
//   Integrator Section 5  : s42,15
//   Integrator Section 6  : s42,15
//   Comb Section 1        : s42,15
//   Comb Section 2        : s42,15
//   Comb Section 3        : s42,15
//   Comb Section 4        : s42,15
//   Comb Section 5        : s42,15
//   Comb Section 6        : s42,15
//
// Implementation Cost
// Number of Multipliers : 0
// Number of Adders      : 12
// Number of States      : 12
// MultPerInputSample    : 0
// AddPerInputSample     : 7.2
// -------------------------------------------------------------

`timescale 1 ns / 1 ns

module CIC6_5 
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
  input   signed [27:0] filter_in; //sfix28_En15
  output  signed [41:0] filter_out; //sfix42_En15
  output  ce_out; 

////////////////////////////////////////////////////////////////
//Module Architecture: CIC6_5
////////////////////////////////////////////////////////////////
  // Local Functions
  // Type Definitions
  // Constants
  // Signals
  reg  [2:0] cur_count; // ufix3
  wire phase_1; // boolean
  reg  ce_out_reg; // boolean
  //   
  reg  signed [27:0] input_register; // sfix28_En15
  //   -- Section 1 Signals 
  wire signed [27:0] section_in1; // sfix28_En15
  wire signed [41:0] section_cast1; // sfix42_En15
  wire signed [41:0] sum1; // sfix42_En15
  reg  signed [41:0] section_out1; // sfix42_En15
  wire signed [41:0] add_cast; // sfix42_En15
  wire signed [41:0] add_cast_1; // sfix42_En15
  wire signed [42:0] add_temp; // sfix43_En15
  //   -- Section 2 Signals 
  wire signed [41:0] section_in2; // sfix42_En15
  wire signed [41:0] sum2; // sfix42_En15
  reg  signed [41:0] section_out2; // sfix42_En15
  wire signed [41:0] add_cast_2; // sfix42_En15
  wire signed [41:0] add_cast_3; // sfix42_En15
  wire signed [42:0] add_temp_1; // sfix43_En15
  //   -- Section 3 Signals 
  wire signed [41:0] section_in3; // sfix42_En15
  wire signed [41:0] sum3; // sfix42_En15
  reg  signed [41:0] section_out3; // sfix42_En15
  wire signed [41:0] add_cast_4; // sfix42_En15
  wire signed [41:0] add_cast_5; // sfix42_En15
  wire signed [42:0] add_temp_2; // sfix43_En15
  //   -- Section 4 Signals 
  wire signed [41:0] section_in4; // sfix42_En15
  wire signed [41:0] sum4; // sfix42_En15
  reg  signed [41:0] section_out4; // sfix42_En15
  wire signed [41:0] add_cast_6; // sfix42_En15
  wire signed [41:0] add_cast_7; // sfix42_En15
  wire signed [42:0] add_temp_3; // sfix43_En15
  //   -- Section 5 Signals 
  wire signed [41:0] section_in5; // sfix42_En15
  wire signed [41:0] sum5; // sfix42_En15
  reg  signed [41:0] section_out5; // sfix42_En15
  wire signed [41:0] add_cast_8; // sfix42_En15
  wire signed [41:0] add_cast_9; // sfix42_En15
  wire signed [42:0] add_temp_4; // sfix43_En15
  //   -- Section 6 Signals 
  wire signed [41:0] section_in6; // sfix42_En15
  wire signed [41:0] sum6; // sfix42_En15
  reg  signed [41:0] section_out6; // sfix42_En15
  wire signed [41:0] add_cast_10; // sfix42_En15
  wire signed [41:0] add_cast_11; // sfix42_En15
  wire signed [42:0] add_temp_5; // sfix43_En15
  //   -- Section 7 Signals 
  wire signed [41:0] section_in7; // sfix42_En15
  reg  signed [41:0] diff1; // sfix42_En15
  wire signed [41:0] section_out7; // sfix42_En15
  wire signed [41:0] sub_cast; // sfix42_En15
  wire signed [41:0] sub_cast_1; // sfix42_En15
  wire signed [42:0] sub_temp; // sfix43_En15
  //   -- Section 8 Signals 
  wire signed [41:0] section_in8; // sfix42_En15
  reg  signed [41:0] diff2; // sfix42_En15
  wire signed [41:0] section_out8; // sfix42_En15
  wire signed [41:0] sub_cast_2; // sfix42_En15
  wire signed [41:0] sub_cast_3; // sfix42_En15
  wire signed [42:0] sub_temp_1; // sfix43_En15
  //   -- Section 9 Signals 
  wire signed [41:0] section_in9; // sfix42_En15
  reg  signed [41:0] diff3; // sfix42_En15
  wire signed [41:0] section_out9; // sfix42_En15
  wire signed [41:0] sub_cast_4; // sfix42_En15
  wire signed [41:0] sub_cast_5; // sfix42_En15
  wire signed [42:0] sub_temp_2; // sfix43_En15
  //   -- Section 10 Signals 
  wire signed [41:0] section_in10; // sfix42_En15
  reg  signed [41:0] diff4; // sfix42_En15
  wire signed [41:0] section_out10; // sfix42_En15
  wire signed [41:0] sub_cast_6; // sfix42_En15
  wire signed [41:0] sub_cast_7; // sfix42_En15
  wire signed [42:0] sub_temp_3; // sfix43_En15
  //   -- Section 11 Signals 
  wire signed [41:0] section_in11; // sfix42_En15
  reg  signed [41:0] diff5; // sfix42_En15
  wire signed [41:0] section_out11; // sfix42_En15
  wire signed [41:0] sub_cast_8; // sfix42_En15
  wire signed [41:0] sub_cast_9; // sfix42_En15
  wire signed [42:0] sub_temp_4; // sfix43_En15
  //   -- Section 12 Signals 
  wire signed [41:0] section_in12; // sfix42_En15
  reg  signed [41:0] diff6; // sfix42_En15
  wire signed [41:0] section_out12; // sfix42_En15
  wire signed [41:0] sub_cast_10; // sfix42_En15
  wire signed [41:0] sub_cast_11; // sfix42_En15
  wire signed [42:0] sub_temp_5; // sfix43_En15
  //   
  reg  signed [41:0] output_register; // sfix42_En15

  // Block Statements
  //   ------------------ CE Output Generation ------------------

  always @ (posedge clk or posedge reset)
    begin: ce_output
      if (reset == 1'b1) begin
        cur_count <= 3'b000;
      end
      else begin
        if (clk_enable == 1'b1) begin
          if (cur_count == 3'b100) begin
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

  assign section_cast1 = $signed({{14{section_in1[27]}}, section_in1});

  assign add_cast = section_cast1;
  assign add_cast_1 = section_out1;
  assign add_temp = add_cast + add_cast_1;
  assign sum1 = add_temp[41:0];

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
  assign sum2 = add_temp_1[41:0];

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
  assign sum3 = add_temp_2[41:0];

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
  assign sum4 = add_temp_3[41:0];

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
  assign sum5 = add_temp_4[41:0];

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
  assign sum6 = add_temp_5[41:0];

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

  //   ------------------ Section # 7 : Comb ------------------

  assign section_in7 = section_out6;

  assign sub_cast = section_in7;
  assign sub_cast_1 = diff1;
  assign sub_temp = sub_cast - sub_cast_1;
  assign section_out7 = sub_temp[41:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section7
      if (reset == 1'b1) begin
        diff1 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff1 <= section_in7;
        end
      end
    end // comb_delay_section7

  //   ------------------ Section # 8 : Comb ------------------

  assign section_in8 = section_out7;

  assign sub_cast_2 = section_in8;
  assign sub_cast_3 = diff2;
  assign sub_temp_1 = sub_cast_2 - sub_cast_3;
  assign section_out8 = sub_temp_1[41:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section8
      if (reset == 1'b1) begin
        diff2 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff2 <= section_in8;
        end
      end
    end // comb_delay_section8

  //   ------------------ Section # 9 : Comb ------------------

  assign section_in9 = section_out8;

  assign sub_cast_4 = section_in9;
  assign sub_cast_5 = diff3;
  assign sub_temp_2 = sub_cast_4 - sub_cast_5;
  assign section_out9 = sub_temp_2[41:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section9
      if (reset == 1'b1) begin
        diff3 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff3 <= section_in9;
        end
      end
    end // comb_delay_section9

  //   ------------------ Section # 10 : Comb ------------------

  assign section_in10 = section_out9;

  assign sub_cast_6 = section_in10;
  assign sub_cast_7 = diff4;
  assign sub_temp_3 = sub_cast_6 - sub_cast_7;
  assign section_out10 = sub_temp_3[41:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section10
      if (reset == 1'b1) begin
        diff4 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff4 <= section_in10;
        end
      end
    end // comb_delay_section10

  //   ------------------ Section # 11 : Comb ------------------

  assign section_in11 = section_out10;

  assign sub_cast_8 = section_in11;
  assign sub_cast_9 = diff5;
  assign sub_temp_4 = sub_cast_8 - sub_cast_9;
  assign section_out11 = sub_temp_4[41:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section11
      if (reset == 1'b1) begin
        diff5 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff5 <= section_in11;
        end
      end
    end // comb_delay_section11

  //   ------------------ Section # 12 : Comb ------------------

  assign section_in12 = section_out11;

  assign sub_cast_10 = section_in12;
  assign sub_cast_11 = diff6;
  assign sub_temp_5 = sub_cast_10 - sub_cast_11;
  assign section_out12 = sub_temp_5[41:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section12
      if (reset == 1'b1) begin
        diff6 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff6 <= section_in12;
        end
      end
    end // comb_delay_section12

  //   ------------------ Output Register ------------------

  always @ (posedge clk or posedge reset)
    begin: output_reg_process
      if (reset == 1'b1) begin
        output_register <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          output_register <= section_out12;
        end
      end
    end // output_reg_process

  // Assignment Statements
  assign ce_out = ce_out_reg;
  assign filter_out = output_register;
endmodule  // CIC6_5
