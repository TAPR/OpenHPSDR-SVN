// -------------------------------------------------------------
//
// Module: CASCADE_D260_stage1
//
// Generated by MATLAB(R) 7.2 and the Filter Design HDL Coder 1.4.
//
// Generated on: 2006-09-19 13:16:46
//
// -------------------------------------------------------------

// -------------------------------------------------------------
// HDL Code Generation Options:
//
// TargetLanguage: Verilog
// Name: CASCADE_D260_stage1
// TargetDirectory: c:\data
// InputPort: filter_in_stage1
// OutputPort: filter_out_stage1
// FIRAdderStyle: tree
// CoeffMultipliers: csd
// SerialPartition: -1
// AddPipelineRegisters: On
// ClockEnableInputPort: clk_enable_stage1
// ClockEnableOutputPort: ce_out_stage1
//
// Filter Settings:
//
// Discrete-Time FIR Multirate Filter (real)
// -----------------------------------------
// Filter Structure        : Cascaded Integrator-Comb Decimator
// Decimation Factor       : 65
// Differential Delay      : 1
// Number of Sections      : 5
// Stable                  : Yes
// Linear Phase            : Yes (Type 1)
//
// Input                   : s16,0
// Output                  : s18,-29
// Filter Internals        : Minimum Word Lengths
//   Integrator Section 1  : s47,0
//   Integrator Section 2  : s41,-6
//   Integrator Section 3  : s36,-11
//   Integrator Section 4  : s31,-16
//   Integrator Section 5  : s26,-21
//   Comb Section 1        : s24,-23
//   Comb Section 2        : s23,-24
//   Comb Section 3        : s22,-25
//   Comb Section 4        : s21,-26
//   Comb Section 5        : s21,-26
//
// Implementation Cost
// Number of Multipliers : 0
// Number of Adders      : 10
// Number of States      : 10
// MultPerInputSample    : 0
// AddPerInputSample     : 5.0769
// -------------------------------------------------------------

`timescale 1 ns / 1 ns

module CASCADE_D260_stage1 
               (
                clk,
                clk_enable_stage1,
                reset,
                filter_in_stage1,
                filter_out_stage1,
                ce_out_stage1
                );

  input   clk; 
  input   clk_enable_stage1; 
  input   reset; 
  input   signed [15:0] filter_in_stage1; //sfix16
  output  signed [17:0] filter_out_stage1; //sfix18_E29
  output  ce_out_stage1; 

////////////////////////////////////////////////////////////////
//Module Architecture: CASCADE_D260_stage1
////////////////////////////////////////////////////////////////
  // Local Functions
  // Type Definitions
  // Constants
  // Signals
  reg  [6:0] cur_count; // ufix7
  wire phase_1; // boolean
  wire ce_delayline; // boolean
  reg  int_delay_pipe [0:3] ; // boolean
  wire ce_gated; // boolean
  reg  ce_out_reg; // boolean
  //   
  reg  signed [15:0] input_register; // sfix16
  //   -- Section 1 Signals 
  wire signed [15:0] section_in1; // sfix16
  wire signed [46:0] section_cast1; // sfix47
  wire signed [46:0] sum1; // sfix47
  reg  signed [46:0] section_out1; // sfix47
  wire signed [46:0] add_signext; // sfix47
  wire signed [46:0] add_signext_1; // sfix47
  wire signed [47:0] add_temp; // sfix48
  //   -- Section 2 Signals 
  wire signed [46:0] section_in2; // sfix47
  wire signed [40:0] section_cast2; // sfix41_E6
  wire signed [40:0] sum2; // sfix41_E6
  reg  signed [40:0] section_out2; // sfix41_E6
  wire signed [40:0] add_signext_2; // sfix41_E6
  wire signed [40:0] add_signext_3; // sfix41_E6
  wire signed [41:0] add_temp_1; // sfix42_E6
  //   -- Section 3 Signals 
  wire signed [40:0] section_in3; // sfix41_E6
  wire signed [35:0] section_cast3; // sfix36_E11
  wire signed [35:0] sum3; // sfix36_E11
  reg  signed [35:0] section_out3; // sfix36_E11
  wire signed [35:0] add_signext_4; // sfix36_E11
  wire signed [35:0] add_signext_5; // sfix36_E11
  wire signed [36:0] add_temp_2; // sfix37_E11
  //   -- Section 4 Signals 
  wire signed [35:0] section_in4; // sfix36_E11
  wire signed [30:0] section_cast4; // sfix31_E16
  wire signed [30:0] sum4; // sfix31_E16
  reg  signed [30:0] section_out4; // sfix31_E16
  wire signed [30:0] add_signext_6; // sfix31_E16
  wire signed [30:0] add_signext_7; // sfix31_E16
  wire signed [31:0] add_temp_3; // sfix32_E16
  //   -- Section 5 Signals 
  wire signed [30:0] section_in5; // sfix31_E16
  wire signed [25:0] section_cast5; // sfix26_E21
  wire signed [25:0] sum5; // sfix26_E21
  reg  signed [25:0] section_out5; // sfix26_E21
  wire signed [25:0] add_signext_8; // sfix26_E21
  wire signed [25:0] add_signext_9; // sfix26_E21
  wire signed [26:0] add_temp_4; // sfix27_E21
  //   -- Section 6 Signals 
  wire signed [25:0] section_in6; // sfix26_E21
  wire signed [23:0] section_cast6; // sfix24_E23
  reg  signed [23:0] diff1; // sfix24_E23
  wire signed [23:0] section_out6; // sfix24_E23
  wire signed [23:0] sub_signext; // sfix24_E23
  wire signed [23:0] sub_signext_1; // sfix24_E23
  wire signed [24:0] sub_temp; // sfix25_E23
  reg  signed [23:0] cic_pipeline6; // sfix24_E23
  //   -- Section 7 Signals 
  wire signed [23:0] section_in7; // sfix24_E23
  wire signed [22:0] section_cast7; // sfix23_E24
  reg  signed [22:0] diff2; // sfix23_E24
  wire signed [22:0] section_out7; // sfix23_E24
  wire signed [22:0] sub_signext_2; // sfix23_E24
  wire signed [22:0] sub_signext_3; // sfix23_E24
  wire signed [23:0] sub_temp_1; // sfix24_E24
  reg  signed [22:0] cic_pipeline7; // sfix23_E24
  //   -- Section 8 Signals 
  wire signed [22:0] section_in8; // sfix23_E24
  wire signed [21:0] section_cast8; // sfix22_E25
  reg  signed [21:0] diff3; // sfix22_E25
  wire signed [21:0] section_out8; // sfix22_E25
  wire signed [21:0] sub_signext_4; // sfix22_E25
  wire signed [21:0] sub_signext_5; // sfix22_E25
  wire signed [22:0] sub_temp_2; // sfix23_E25
  reg  signed [21:0] cic_pipeline8; // sfix22_E25
  //   -- Section 9 Signals 
  wire signed [21:0] section_in9; // sfix22_E25
  wire signed [20:0] section_cast9; // sfix21_E26
  reg  signed [20:0] diff4; // sfix21_E26
  wire signed [20:0] section_out9; // sfix21_E26
  wire signed [20:0] sub_signext_6; // sfix21_E26
  wire signed [20:0] sub_signext_7; // sfix21_E26
  wire signed [21:0] sub_temp_3; // sfix22_E26
  reg  signed [20:0] cic_pipeline9; // sfix21_E26
  //   -- Section 10 Signals 
  wire signed [20:0] section_in10; // sfix21_E26
  reg  signed [20:0] diff5; // sfix21_E26
  wire signed [20:0] section_out10; // sfix21_E26
  wire signed [20:0] sub_signext_8; // sfix21_E26
  wire signed [20:0] sub_signext_9; // sfix21_E26
  wire signed [21:0] sub_temp_4; // sfix22_E26
  wire signed [17:0] output_typeconvert; // sfix18_E29
  //   
  reg  signed [17:0] output_register; // sfix18_E29

  // Block Statements
  //   ------------------ CE Output Generation ------------------

  always @ (posedge clk or posedge reset)
    begin: ce_output
      if (reset == 1'b1) begin
        cur_count <= 7'b0000000;
      end
      else begin
        if (clk_enable_stage1 == 1'b1) begin
          if (cur_count == 7'b1000000) begin
            cur_count <= 7'b0000000;
          end
          else begin
            cur_count <= cur_count + 1;
          end
        end
      end
    end // ce_output

  assign  phase_1 = (cur_count == 7'b0000001 && clk_enable_stage1 == 1'b1)? 1 : 0;

  always @( posedge clk or posedge reset)
    begin: ce_delay
      if (reset == 1'b1) begin
        int_delay_pipe[0] <= 1'b0;
        int_delay_pipe[1] <= 1'b0;
        int_delay_pipe[2] <= 1'b0;
        int_delay_pipe[3] <= 1'b0;
      end
      else begin
        if (phase_1 == 1'b1) begin
        int_delay_pipe[0] <= clk_enable_stage1;
        int_delay_pipe[1] <= int_delay_pipe[0];
        int_delay_pipe[2] <= int_delay_pipe[1];
        int_delay_pipe[3] <= int_delay_pipe[2];
        end
      end
    end // ce_delay

    assign ce_delayline = int_delay_pipe[3];
  assign ce_gated =  ce_delayline & phase_1;

  //   ------------------ CE Output Register ------------------

  always @ (posedge clk or posedge reset)
    begin: ce_output_register
      if (reset == 1'b1) begin
        ce_out_reg <= 1'b0;
      end
      else begin
          ce_out_reg <= ce_gated;
      end
    end // ce_output_register

  //   ------------------ Input Register ------------------

  always @ (posedge clk or posedge reset)
    begin: input_reg_process
      if (reset == 1'b1) begin
        input_register <= 0;
      end
      else begin
        if (clk_enable_stage1 == 1'b1) begin
          input_register <= filter_in_stage1;
        end
      end
    end // input_reg_process

  //   ------------------ Section # 1 : Integrator ------------------

  assign section_in1 = input_register;

  assign section_cast1 = $signed({{31{section_in1[15]}}, section_in1});

  assign add_signext = section_cast1;
  assign add_signext_1 = section_out1;
  assign add_temp = add_signext + add_signext_1;
  assign sum1 = add_temp[46:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section1
      if (reset == 1'b1) begin
        section_out1 <= 0;
      end
      else begin
        if (clk_enable_stage1 == 1'b1) begin
          section_out1 <= sum1;
        end
      end
    end // integrator_delay_section1

  //   ------------------ Section # 2 : Integrator ------------------

  assign section_in2 = section_out1;

  assign section_cast2 = section_in2[46:6];

  assign add_signext_2 = section_cast2;
  assign add_signext_3 = section_out2;
  assign add_temp_1 = add_signext_2 + add_signext_3;
  assign sum2 = add_temp_1[40:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section2
      if (reset == 1'b1) begin
        section_out2 <= 0;
      end
      else begin
        if (clk_enable_stage1 == 1'b1) begin
          section_out2 <= sum2;
        end
      end
    end // integrator_delay_section2

  //   ------------------ Section # 3 : Integrator ------------------

  assign section_in3 = section_out2;

  assign section_cast3 = section_in3[40:5];

  assign add_signext_4 = section_cast3;
  assign add_signext_5 = section_out3;
  assign add_temp_2 = add_signext_4 + add_signext_5;
  assign sum3 = add_temp_2[35:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section3
      if (reset == 1'b1) begin
        section_out3 <= 0;
      end
      else begin
        if (clk_enable_stage1 == 1'b1) begin
          section_out3 <= sum3;
        end
      end
    end // integrator_delay_section3

  //   ------------------ Section # 4 : Integrator ------------------

  assign section_in4 = section_out3;

  assign section_cast4 = section_in4[35:5];

  assign add_signext_6 = section_cast4;
  assign add_signext_7 = section_out4;
  assign add_temp_3 = add_signext_6 + add_signext_7;
  assign sum4 = add_temp_3[30:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section4
      if (reset == 1'b1) begin
        section_out4 <= 0;
      end
      else begin
        if (clk_enable_stage1 == 1'b1) begin
          section_out4 <= sum4;
        end
      end
    end // integrator_delay_section4

  //   ------------------ Section # 5 : Integrator ------------------

  assign section_in5 = section_out4;

  assign section_cast5 = section_in5[30:5];

  assign add_signext_8 = section_cast5;
  assign add_signext_9 = section_out5;
  assign add_temp_4 = add_signext_8 + add_signext_9;
  assign sum5 = add_temp_4[25:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section5
      if (reset == 1'b1) begin
        section_out5 <= 0;
      end
      else begin
        if (clk_enable_stage1 == 1'b1) begin
          section_out5 <= sum5;
        end
      end
    end // integrator_delay_section5

  //   ------------------ Section # 6 : Comb ------------------

  assign section_in6 = section_out5;

  assign section_cast6 = section_in6[25:2];

  assign sub_signext = section_cast6;
  assign sub_signext_1 = diff1;
  assign sub_temp = sub_signext - sub_signext_1;
  assign section_out6 = sub_temp[23:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section6
      if (reset == 1'b1) begin
        diff1 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff1 <= section_cast6;
        end
      end
    end // comb_delay_section6

  always @ (posedge clk or posedge reset)
    begin: cic_pipeline_process_section6
      if (reset == 1'b1) begin
        cic_pipeline6 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          cic_pipeline6 <= section_out6;
        end
      end
    end // cic_pipeline_process_section6

  //   ------------------ Section # 7 : Comb ------------------

  assign section_in7 = cic_pipeline6;

  assign section_cast7 = section_in7[23:1];

  assign sub_signext_2 = section_cast7;
  assign sub_signext_3 = diff2;
  assign sub_temp_1 = sub_signext_2 - sub_signext_3;
  assign section_out7 = sub_temp_1[22:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section7
      if (reset == 1'b1) begin
        diff2 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff2 <= section_cast7;
        end
      end
    end // comb_delay_section7

  always @ (posedge clk or posedge reset)
    begin: cic_pipeline_process_section7
      if (reset == 1'b1) begin
        cic_pipeline7 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          cic_pipeline7 <= section_out7;
        end
      end
    end // cic_pipeline_process_section7

  //   ------------------ Section # 8 : Comb ------------------

  assign section_in8 = cic_pipeline7;

  assign section_cast8 = section_in8[22:1];

  assign sub_signext_4 = section_cast8;
  assign sub_signext_5 = diff3;
  assign sub_temp_2 = sub_signext_4 - sub_signext_5;
  assign section_out8 = sub_temp_2[21:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section8
      if (reset == 1'b1) begin
        diff3 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff3 <= section_cast8;
        end
      end
    end // comb_delay_section8

  always @ (posedge clk or posedge reset)
    begin: cic_pipeline_process_section8
      if (reset == 1'b1) begin
        cic_pipeline8 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          cic_pipeline8 <= section_out8;
        end
      end
    end // cic_pipeline_process_section8

  //   ------------------ Section # 9 : Comb ------------------

  assign section_in9 = cic_pipeline8;

  assign section_cast9 = section_in9[21:1];

  assign sub_signext_6 = section_cast9;
  assign sub_signext_7 = diff4;
  assign sub_temp_3 = sub_signext_6 - sub_signext_7;
  assign section_out9 = sub_temp_3[20:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section9
      if (reset == 1'b1) begin
        diff4 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff4 <= section_cast9;
        end
      end
    end // comb_delay_section9

  always @ (posedge clk or posedge reset)
    begin: cic_pipeline_process_section9
      if (reset == 1'b1) begin
        cic_pipeline9 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          cic_pipeline9 <= section_out9;
        end
      end
    end // cic_pipeline_process_section9

  //   ------------------ Section # 10 : Comb ------------------

  assign section_in10 = cic_pipeline9;

  assign sub_signext_8 = section_in10;
  assign sub_signext_9 = diff5;
  assign sub_temp_4 = sub_signext_8 - sub_signext_9;
  assign section_out10 = sub_temp_4[20:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section10
      if (reset == 1'b1) begin
        diff5 <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          diff5 <= section_in10;
        end
      end
    end // comb_delay_section10

  assign output_typeconvert = section_out10[20:3];

  //   ------------------ Output Register ------------------

  always @ (posedge clk or posedge reset)
    begin: output_reg_process
      if (reset == 1'b1) begin
        output_register <= 0;
      end
      else begin
        if (phase_1 == 1'b1) begin
          output_register <= output_typeconvert;
        end
      end
    end // output_reg_process

  // Assignment Statements
  assign ce_out_stage1 = ce_out_reg;
  assign filter_out_stage1 = output_register;
endmodule  // CASCADE_D260_stage1
