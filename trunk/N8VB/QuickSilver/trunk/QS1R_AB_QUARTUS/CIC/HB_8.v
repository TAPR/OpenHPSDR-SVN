// -------------------------------------------------------------
//
// Module: HB_8
//
// Generated by MATLAB(R) 7.2 and the Filter Design HDL Coder 1.4.
//
// Generated on: 2007-02-08 13:11:11
//
// -------------------------------------------------------------

// -------------------------------------------------------------
// HDL Code Generation Options:
//
// TargetLanguage: Verilog
// Name: HB_8
// TargetDirectory: C:\Data\QS\QuickSilver\trunk\QS1R_AB_QUARTUS\CIC
// OptimizeForHDL: On
// FIRAdderStyle: tree
// SerialPartition: -1
// InlineConfigurations: Off
// SafeZeroConcat: Off
// TestBenchStimulus: step ramp chirp noise 
//
// Filter Settings:
//
// Discrete-Time FIR Multirate Filter (real)
// -----------------------------------------
// Filter Structure   : Direct-Form FIR Polyphase Decimator
// Decimation Factor  : 8
// Polyphase Length   : 2
// Filter Length      : 11
// Stable             : Yes
// Linear Phase       : Yes (Type 1)
//
// Arithmetic         : fixed
// Numerator          : s16,15 -> [-1 1)
// Input              : s32,15 -> [-65536 65536)
// Filter Internals   : Full Precision
//   Output           : s48,30 -> [-131072 131072)  (auto determined)
//   Product          : s46,30 -> [-32768 32768)  (auto determined)
//   Accumulator      : s48,30 -> [-131072 131072)  (auto determined)
//   Round Mode       : No rounding
//   Overflow Mode    : No overflow
//
// Implementation Cost
// Number of Multipliers : 7
// Number of Adders      : 6
// Number of States      : 8
// MultPerInputSample    : 0.875
// AddPerInputSample     : 0.75
// -------------------------------------------------------------

`timescale 1 ns / 1 ns

module HB_8 
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
  output  signed [47:0] filter_out; //sfix48_En30
  output  ce_out; 

////////////////////////////////////////////////////////////////
//Module Architecture: HB_8
////////////////////////////////////////////////////////////////
  // Local Functions
  // Type Definitions
  // Constants
  parameter signed [15:0] coeffphase1_1 = 16'b0000000100111110; //sfix16_En15
  parameter signed [15:0] coeffphase1_2 = 16'b1111100010001111; //sfix16_En15
  parameter signed [15:0] coeffphase2_1 = 16'b0000000000000000; //sfix16_En15
  parameter signed [15:0] coeffphase2_2 = 16'b0000000000000000; //sfix16_En15
  parameter signed [15:0] coeffphase3_1 = 16'b1111100010001111; //sfix16_En15
  parameter signed [15:0] coeffphase3_2 = 16'b0000000100111110; //sfix16_En15
  parameter signed [15:0] coeffphase4_1 = 16'b0000000000000000; //sfix16_En15
  parameter signed [15:0] coeffphase4_2 = 16'b0000000000000000; //sfix16_En15
  parameter signed [15:0] coeffphase5_1 = 16'b0010011000111000; //sfix16_En15
  parameter signed [15:0] coeffphase5_2 = 16'b0000000000000000; //sfix16_En15
  parameter signed [15:0] coeffphase6_1 = 16'b0100000000000000; //sfix16_En15
  parameter signed [15:0] coeffphase6_2 = 16'b0000000000000000; //sfix16_En15
  parameter signed [15:0] coeffphase7_1 = 16'b0010011000111000; //sfix16_En15
  parameter signed [15:0] coeffphase7_2 = 16'b0000000000000000; //sfix16_En15
  parameter signed [15:0] coeffphase8_1 = 16'b0000000000000000; //sfix16_En15
  parameter signed [15:0] coeffphase8_2 = 16'b0000000000000000; //sfix16_En15
  // Signals
  reg  [7:0] ring_count; // ufix8
  wire phase_0; // boolean
  wire phase_1; // boolean
  wire phase_2; // boolean
  wire phase_3; // boolean
  wire phase_4; // boolean
  wire phase_5; // boolean
  wire phase_6; // boolean
  wire phase_7; // boolean
  reg  ce_out_reg; // boolean
  reg  signed [31:0] input_register; // sfix32_En15
  reg  signed [31:0] input_pipeline_phase0 [0:0] ; // sfix32_En15
  reg  signed [31:0] input_pipeline_phase2 [0:1] ; // sfix32_En15
  reg  signed [31:0] input_pipeline_phase4; // sfix32_En15
  reg  signed [31:0] input_pipeline_phase5; // sfix32_En15
  reg  signed [31:0] input_pipeline_phase6; // sfix32_En15
  wire signed [45:0] product_phase0_1; // sfix46_En30
  wire signed [47:0] mul_temp; // sfix48_En30
  wire signed [45:0] product_phase0_2; // sfix46_En30
  wire signed [47:0] mul_temp_1; // sfix48_En30
  wire signed [45:0] product_phase2_1; // sfix46_En30
  wire signed [47:0] mul_temp_2; // sfix48_En30
  wire signed [45:0] product_phase2_2; // sfix46_En30
  wire signed [47:0] mul_temp_3; // sfix48_En30
  wire signed [45:0] product_phase4_1; // sfix46_En30
  wire signed [47:0] mul_temp_4; // sfix48_En30
  wire signed [45:0] product_phase5_1; // sfix46_En30
  wire signed [45:0] product_phase6_1; // sfix46_En30
  wire signed [47:0] mul_temp_5; // sfix48_En30
  wire signed [47:0] sum1_1; // sfix48_En30
  wire signed [45:0] add_signext; // sfix46_En30
  wire signed [45:0] add_signext_1; // sfix46_En30
  wire signed [46:0] add_temp; // sfix47_En30
  wire signed [47:0] sum1_2; // sfix48_En30
  wire signed [45:0] add_signext_2; // sfix46_En30
  wire signed [45:0] add_signext_3; // sfix46_En30
  wire signed [46:0] add_temp_1; // sfix47_En30
  wire signed [47:0] sum1_3; // sfix48_En30
  wire signed [45:0] add_signext_4; // sfix46_En30
  wire signed [45:0] add_signext_5; // sfix46_En30
  wire signed [46:0] add_temp_2; // sfix47_En30
  wire signed [47:0] sum2_1; // sfix48_En30
  wire signed [47:0] add_signext_6; // sfix48_En30
  wire signed [47:0] add_signext_7; // sfix48_En30
  wire signed [48:0] add_temp_3; // sfix49_En30
  wire signed [47:0] sum2_2; // sfix48_En30
  wire signed [47:0] add_signext_8; // sfix48_En30
  wire signed [47:0] add_signext_9; // sfix48_En30
  wire signed [48:0] add_temp_4; // sfix49_En30
  wire signed [47:0] sum3_1; // sfix48_En30
  wire signed [47:0] add_signext_10; // sfix48_En30
  wire signed [47:0] add_signext_11; // sfix48_En30
  wire signed [48:0] add_temp_5; // sfix49_En30
  reg  signed [47:0] output_register; // sfix48_En30

  // Block Statements
  always @ (posedge clk or posedge reset)
    begin: ce_output
      if (reset == 1'b1) begin
        ring_count <= 1;
      end
      else begin
        if (clk_enable == 1'b1) begin
          ring_count <= {ring_count[0], ring_count[7 : 1]};
        end
      end
    end // ce_output

  assign  phase_0 = ring_count[0]  && clk_enable;

  assign  phase_1 = ring_count[1]  && clk_enable;

  assign  phase_2 = ring_count[2]  && clk_enable;

  assign  phase_3 = ring_count[3]  && clk_enable;

  assign  phase_4 = ring_count[4]  && clk_enable;

  assign  phase_5 = ring_count[5]  && clk_enable;

  assign  phase_6 = ring_count[6]  && clk_enable;

  assign  phase_7 = ring_count[7]  && clk_enable;

  //   ------------------ CE Output Generation ------------------

  //   ------------------ CE Output Register ------------------

  always @ (posedge clk or posedge reset)
    begin: ce_output_register
      if (reset == 1'b1) begin
        ce_out_reg <= 1'b0;
      end
      else begin
          ce_out_reg <= phase_7;
      end
    end // ce_output_register

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

  always @ (posedge clk or posedge reset)
    begin: Delay_Pipeline_Phase0_process
      if (reset == 1'b1) begin
        input_pipeline_phase0 <= 0;
      end
      else begin
        if (phase_7 == 1'b1) begin
          input_pipeline_phase0 <= input_register;
        end
      end
    end // Delay_Pipeline_Phase0_process

  always @( posedge clk or posedge reset)
    begin: Delay_Pipeline_Phase2_process
      if (reset == 1'b1) begin
        input_pipeline_phase2[0] <= 0;
        input_pipeline_phase2[1] <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          input_pipeline_phase2[0] <= input_register;
          input_pipeline_phase2[1] <= input_pipeline_phase2[0];
        end
      end
    end // Delay_Pipeline_Phase2_process


  always @ (posedge clk or posedge reset)
    begin: Delay_Pipeline_Phase4_process
      if (reset == 1'b1) begin
        input_pipeline_phase4 <= 0;
      end
      else begin
        if (phase_3 == 1'b1) begin
          input_pipeline_phase4 <= input_register;
        end
      end
    end // Delay_Pipeline_Phase4_process

  always @ (posedge clk or posedge reset)
    begin: Delay_Pipeline_Phase5_process
      if (reset == 1'b1) begin
        input_pipeline_phase5 <= 0;
      end
      else begin
        if (phase_4 == 1'b1) begin
          input_pipeline_phase5 <= input_register;
        end
      end
    end // Delay_Pipeline_Phase5_process

  always @ (posedge clk or posedge reset)
    begin: Delay_Pipeline_Phase6_process
      if (reset == 1'b1) begin
        input_pipeline_phase6 <= 0;
      end
      else begin
        if (phase_5 == 1'b1) begin
          input_pipeline_phase6 <= input_register;
        end
      end
    end // Delay_Pipeline_Phase6_process

  assign mul_temp = input_register * coeffphase1_1;
  assign product_phase0_1 = mul_temp[45:0];

  assign mul_temp_1 = input_pipeline_phase0[0] * coeffphase1_2;
  assign product_phase0_2 = mul_temp_1[45:0];

  assign mul_temp_2 = input_pipeline_phase2[0] * coeffphase3_1;
  assign product_phase2_1 = mul_temp_2[45:0];

  assign mul_temp_3 = input_pipeline_phase2[1] * coeffphase3_2;
  assign product_phase2_2 = mul_temp_3[45:0];

  assign mul_temp_4 = input_pipeline_phase4 * coeffphase5_1;
  assign product_phase4_1 = mul_temp_4[45:0];

  assign product_phase5_1 = $signed({input_pipeline_phase5[31:0], 14'b00000000000000});

  assign mul_temp_5 = input_pipeline_phase6 * coeffphase7_1;
  assign product_phase6_1 = mul_temp_5[45:0];

  assign add_signext = product_phase6_1;
  assign add_signext_1 = product_phase5_1;
  assign add_temp = add_signext + add_signext_1;
  assign sum1_1 = $signed({{1{add_temp[46]}}, add_temp});

  assign add_signext_2 = product_phase4_1;
  assign add_signext_3 = product_phase2_1;
  assign add_temp_1 = add_signext_2 + add_signext_3;
  assign sum1_2 = $signed({{1{add_temp_1[46]}}, add_temp_1});

  assign add_signext_4 = product_phase2_2;
  assign add_signext_5 = product_phase0_1;
  assign add_temp_2 = add_signext_4 + add_signext_5;
  assign sum1_3 = $signed({{1{add_temp_2[46]}}, add_temp_2});

  assign add_signext_6 = sum1_1;
  assign add_signext_7 = sum1_2;
  assign add_temp_3 = add_signext_6 + add_signext_7;
  assign sum2_1 = add_temp_3[47:0];

  assign add_signext_8 = sum1_3;
  assign add_signext_9 = $signed({{2{product_phase0_2[45]}}, product_phase0_2});
  assign add_temp_4 = add_signext_8 + add_signext_9;
  assign sum2_2 = add_temp_4[47:0];

  assign add_signext_10 = sum2_1;
  assign add_signext_11 = sum2_2;
  assign add_temp_5 = add_signext_10 + add_signext_11;
  assign sum3_1 = add_temp_5[47:0];

  always @ (posedge clk or posedge reset)
    begin: output_register_process
      if (reset == 1'b1) begin
        output_register <= 0;
      end
      else begin
        if (phase_7 == 1'b1) begin
          output_register <= sum3_1;
        end
      end
    end // output_register_process

  // Assignment Statements
  assign ce_out = ce_out_reg;
  assign filter_out = output_register;
endmodule  // HB_8
