// -------------------------------------------------------------
//
// Module: CASCADE_D520_stage2
//
// Generated by MATLAB(R) 7.2 and the Filter Design HDL Coder 1.4.
//
// Generated on: 2006-09-19 12:41:02
//
// -------------------------------------------------------------

// -------------------------------------------------------------
// HDL Code Generation Options:
//
// TargetLanguage: Verilog
// Name: CASCADE_D520_stage2
// TargetDirectory: c:\data
// InputPort: filter_in_stage2
// OutputPort: filter_out_stage2
// AddInputRegister: Off
// FIRAdderStyle: tree
// CoeffMultipliers: csd
// SerialPartition: -1
// AddPipelineRegisters: On
// ClockEnableInputPort: clk_enable_stage2
// ClockEnableOutputPort: ce_out_stage2
//
// Filter Settings:
//
// Discrete-Time FIR Multirate Filter (real)
// -----------------------------------------
// Filter Structure         : Cascaded Integrator-Comb Decimator
// Decimation Factor        : 2
// Differential Delay       : 1
// Number of Sections       : 10
// Stable                   : Yes
// Linear Phase             : Yes (Type 1)
//
// Input                    : s18,15
// Output                   : s24,11
// Filter Internals         : Minimum Word Lengths
//   Integrator Section 1   : s28,15
//   Integrator Section 2   : s28,15
//   Integrator Section 3   : s28,15
//   Integrator Section 4   : s28,15
//   Integrator Section 5   : s28,15
//   Integrator Section 6   : s28,15
//   Integrator Section 7   : s28,15
//   Integrator Section 8   : s28,15
//   Integrator Section 9   : s28,15
//   Integrator Section 10  : s28,15
//   Comb Section 1         : s28,15
//   Comb Section 2         : s28,15
//   Comb Section 3         : s28,15
//   Comb Section 4         : s28,15
//   Comb Section 5         : s28,15
//   Comb Section 6         : s28,15
//   Comb Section 7         : s28,15
//   Comb Section 8         : s28,15
//   Comb Section 9         : s28,15
//   Comb Section 10        : s27,14
//
// Implementation Cost
// Number of Multipliers : 0
// Number of Adders      : 20
// Number of States      : 20
// MultPerInputSample    : 0
// AddPerInputSample     : 15
// -------------------------------------------------------------

`timescale 1 ns / 1 ns

module CASCADE_D520_stage2 
               (
                clk,
                clk_enable_stage2,
                reset,
                filter_in_stage2,
                filter_out_stage2,
                ce_out_stage2
                );

  input   clk; 
  input   clk_enable_stage2; 
  input   reset; 
  input   signed [17:0] filter_in_stage2; //sfix18_En15
  output  signed [23:0] filter_out_stage2; //sfix24_En11
  output  ce_out_stage2; 

////////////////////////////////////////////////////////////////
//Module Architecture: CASCADE_D520_stage2
////////////////////////////////////////////////////////////////
  // Local Functions
  // Type Definitions
  // Constants
  // Signals
  reg  [1:0] cur_count; // ufix2
  wire phase_0; // boolean
  wire ce_delayline; // boolean
  reg  int_delay_pipe [0:8] ; // boolean
  wire ce_gated; // boolean
  reg  ce_out_reg; // boolean
  wire signed [17:0] input_typeconvert; // sfix18_En15
  //   -- Section 1 Signals 
  wire signed [17:0] section_in1; // sfix18_En15
  wire signed [27:0] section_cast1; // sfix28_En15
  wire signed [27:0] sum1; // sfix28_En15
  reg  signed [27:0] section_out1; // sfix28_En15
  wire signed [27:0] add_signext; // sfix28_En15
  wire signed [27:0] add_signext_1; // sfix28_En15
  wire signed [28:0] add_temp; // sfix29_En15
  //   -- Section 2 Signals 
  wire signed [27:0] section_in2; // sfix28_En15
  wire signed [27:0] sum2; // sfix28_En15
  reg  signed [27:0] section_out2; // sfix28_En15
  wire signed [27:0] add_signext_2; // sfix28_En15
  wire signed [27:0] add_signext_3; // sfix28_En15
  wire signed [28:0] add_temp_1; // sfix29_En15
  //   -- Section 3 Signals 
  wire signed [27:0] section_in3; // sfix28_En15
  wire signed [27:0] sum3; // sfix28_En15
  reg  signed [27:0] section_out3; // sfix28_En15
  wire signed [27:0] add_signext_4; // sfix28_En15
  wire signed [27:0] add_signext_5; // sfix28_En15
  wire signed [28:0] add_temp_2; // sfix29_En15
  //   -- Section 4 Signals 
  wire signed [27:0] section_in4; // sfix28_En15
  wire signed [27:0] sum4; // sfix28_En15
  reg  signed [27:0] section_out4; // sfix28_En15
  wire signed [27:0] add_signext_6; // sfix28_En15
  wire signed [27:0] add_signext_7; // sfix28_En15
  wire signed [28:0] add_temp_3; // sfix29_En15
  //   -- Section 5 Signals 
  wire signed [27:0] section_in5; // sfix28_En15
  wire signed [27:0] sum5; // sfix28_En15
  reg  signed [27:0] section_out5; // sfix28_En15
  wire signed [27:0] add_signext_8; // sfix28_En15
  wire signed [27:0] add_signext_9; // sfix28_En15
  wire signed [28:0] add_temp_4; // sfix29_En15
  //   -- Section 6 Signals 
  wire signed [27:0] section_in6; // sfix28_En15
  wire signed [27:0] sum6; // sfix28_En15
  reg  signed [27:0] section_out6; // sfix28_En15
  wire signed [27:0] add_signext_10; // sfix28_En15
  wire signed [27:0] add_signext_11; // sfix28_En15
  wire signed [28:0] add_temp_5; // sfix29_En15
  //   -- Section 7 Signals 
  wire signed [27:0] section_in7; // sfix28_En15
  wire signed [27:0] sum7; // sfix28_En15
  reg  signed [27:0] section_out7; // sfix28_En15
  wire signed [27:0] add_signext_12; // sfix28_En15
  wire signed [27:0] add_signext_13; // sfix28_En15
  wire signed [28:0] add_temp_6; // sfix29_En15
  //   -- Section 8 Signals 
  wire signed [27:0] section_in8; // sfix28_En15
  wire signed [27:0] sum8; // sfix28_En15
  reg  signed [27:0] section_out8; // sfix28_En15
  wire signed [27:0] add_signext_14; // sfix28_En15
  wire signed [27:0] add_signext_15; // sfix28_En15
  wire signed [28:0] add_temp_7; // sfix29_En15
  //   -- Section 9 Signals 
  wire signed [27:0] section_in9; // sfix28_En15
  wire signed [27:0] sum9; // sfix28_En15
  reg  signed [27:0] section_out9; // sfix28_En15
  wire signed [27:0] add_signext_16; // sfix28_En15
  wire signed [27:0] add_signext_17; // sfix28_En15
  wire signed [28:0] add_temp_8; // sfix29_En15
  //   -- Section 10 Signals 
  wire signed [27:0] section_in10; // sfix28_En15
  wire signed [27:0] sum10; // sfix28_En15
  reg  signed [27:0] section_out10; // sfix28_En15
  wire signed [27:0] add_signext_18; // sfix28_En15
  wire signed [27:0] add_signext_19; // sfix28_En15
  wire signed [28:0] add_temp_9; // sfix29_En15
  //   -- Section 11 Signals 
  wire signed [27:0] section_in11; // sfix28_En15
  reg  signed [27:0] diff1; // sfix28_En15
  wire signed [27:0] section_out11; // sfix28_En15
  wire signed [27:0] sub_signext; // sfix28_En15
  wire signed [27:0] sub_signext_1; // sfix28_En15
  wire signed [28:0] sub_temp; // sfix29_En15
  reg  signed [27:0] cic_pipeline11; // sfix28_En15
  //   -- Section 12 Signals 
  wire signed [27:0] section_in12; // sfix28_En15
  reg  signed [27:0] diff2; // sfix28_En15
  wire signed [27:0] section_out12; // sfix28_En15
  wire signed [27:0] sub_signext_2; // sfix28_En15
  wire signed [27:0] sub_signext_3; // sfix28_En15
  wire signed [28:0] sub_temp_1; // sfix29_En15
  reg  signed [27:0] cic_pipeline12; // sfix28_En15
  //   -- Section 13 Signals 
  wire signed [27:0] section_in13; // sfix28_En15
  reg  signed [27:0] diff3; // sfix28_En15
  wire signed [27:0] section_out13; // sfix28_En15
  wire signed [27:0] sub_signext_4; // sfix28_En15
  wire signed [27:0] sub_signext_5; // sfix28_En15
  wire signed [28:0] sub_temp_2; // sfix29_En15
  reg  signed [27:0] cic_pipeline13; // sfix28_En15
  //   -- Section 14 Signals 
  wire signed [27:0] section_in14; // sfix28_En15
  reg  signed [27:0] diff4; // sfix28_En15
  wire signed [27:0] section_out14; // sfix28_En15
  wire signed [27:0] sub_signext_6; // sfix28_En15
  wire signed [27:0] sub_signext_7; // sfix28_En15
  wire signed [28:0] sub_temp_3; // sfix29_En15
  reg  signed [27:0] cic_pipeline14; // sfix28_En15
  //   -- Section 15 Signals 
  wire signed [27:0] section_in15; // sfix28_En15
  reg  signed [27:0] diff5; // sfix28_En15
  wire signed [27:0] section_out15; // sfix28_En15
  wire signed [27:0] sub_signext_8; // sfix28_En15
  wire signed [27:0] sub_signext_9; // sfix28_En15
  wire signed [28:0] sub_temp_4; // sfix29_En15
  reg  signed [27:0] cic_pipeline15; // sfix28_En15
  //   -- Section 16 Signals 
  wire signed [27:0] section_in16; // sfix28_En15
  reg  signed [27:0] diff6; // sfix28_En15
  wire signed [27:0] section_out16; // sfix28_En15
  wire signed [27:0] sub_signext_10; // sfix28_En15
  wire signed [27:0] sub_signext_11; // sfix28_En15
  wire signed [28:0] sub_temp_5; // sfix29_En15
  reg  signed [27:0] cic_pipeline16; // sfix28_En15
  //   -- Section 17 Signals 
  wire signed [27:0] section_in17; // sfix28_En15
  reg  signed [27:0] diff7; // sfix28_En15
  wire signed [27:0] section_out17; // sfix28_En15
  wire signed [27:0] sub_signext_12; // sfix28_En15
  wire signed [27:0] sub_signext_13; // sfix28_En15
  wire signed [28:0] sub_temp_6; // sfix29_En15
  reg  signed [27:0] cic_pipeline17; // sfix28_En15
  //   -- Section 18 Signals 
  wire signed [27:0] section_in18; // sfix28_En15
  reg  signed [27:0] diff8; // sfix28_En15
  wire signed [27:0] section_out18; // sfix28_En15
  wire signed [27:0] sub_signext_14; // sfix28_En15
  wire signed [27:0] sub_signext_15; // sfix28_En15
  wire signed [28:0] sub_temp_7; // sfix29_En15
  reg  signed [27:0] cic_pipeline18; // sfix28_En15
  //   -- Section 19 Signals 
  wire signed [27:0] section_in19; // sfix28_En15
  reg  signed [27:0] diff9; // sfix28_En15
  wire signed [27:0] section_out19; // sfix28_En15
  wire signed [27:0] sub_signext_16; // sfix28_En15
  wire signed [27:0] sub_signext_17; // sfix28_En15
  wire signed [28:0] sub_temp_8; // sfix29_En15
  reg  signed [27:0] cic_pipeline19; // sfix28_En15
  //   -- Section 20 Signals 
  wire signed [27:0] section_in20; // sfix28_En15
  wire signed [26:0] section_cast20; // sfix27_En14
  reg  signed [26:0] diff10; // sfix27_En14
  wire signed [26:0] section_out20; // sfix27_En14
  wire signed [26:0] sub_signext_18; // sfix27_En14
  wire signed [26:0] sub_signext_19; // sfix27_En14
  wire signed [27:0] sub_temp_9; // sfix28_En14
  wire signed [23:0] output_typeconvert; // sfix24_En11
  //   
  reg  signed [23:0] output_register; // sfix24_En11

  // Block Statements
  //   ------------------ CE Output Generation ------------------

  always @ (posedge clk or posedge reset)
    begin: ce_output
      if (reset == 1'b1) begin
        cur_count <= 2'b00;
      end
      else begin
        if (clk_enable_stage2 == 1'b1) begin
          if (cur_count == 2'b01) begin
            cur_count <= 2'b00;
          end
          else begin
            cur_count <= cur_count + 1;
          end
        end
      end
    end // ce_output

  assign  phase_0 = (cur_count == 2'b00 && clk_enable_stage2 == 1'b1)? 1 : 0;

  always @( posedge clk or posedge reset)
    begin: ce_delay
      if (reset == 1'b1) begin
        int_delay_pipe[0] <= 1'b0;
        int_delay_pipe[1] <= 1'b0;
        int_delay_pipe[2] <= 1'b0;
        int_delay_pipe[3] <= 1'b0;
        int_delay_pipe[4] <= 1'b0;
        int_delay_pipe[5] <= 1'b0;
        int_delay_pipe[6] <= 1'b0;
        int_delay_pipe[7] <= 1'b0;
        int_delay_pipe[8] <= 1'b0;
      end
      else begin
        if (phase_0 == 1'b1) begin
        int_delay_pipe[0] <= clk_enable_stage2;
        int_delay_pipe[1] <= int_delay_pipe[0];
        int_delay_pipe[2] <= int_delay_pipe[1];
        int_delay_pipe[3] <= int_delay_pipe[2];
        int_delay_pipe[4] <= int_delay_pipe[3];
        int_delay_pipe[5] <= int_delay_pipe[4];
        int_delay_pipe[6] <= int_delay_pipe[5];
        int_delay_pipe[7] <= int_delay_pipe[6];
        int_delay_pipe[8] <= int_delay_pipe[7];
        end
      end
    end // ce_delay

    assign ce_delayline = int_delay_pipe[8];
  assign ce_gated =  ce_delayline & phase_0;

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

  assign input_typeconvert = filter_in_stage2;

  //   ------------------ Section # 1 : Integrator ------------------

  assign section_in1 = input_typeconvert;

  assign section_cast1 = $signed({{10{section_in1[17]}}, section_in1});

  assign add_signext = section_cast1;
  assign add_signext_1 = section_out1;
  assign add_temp = add_signext + add_signext_1;
  assign sum1 = add_temp[27:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section1
      if (reset == 1'b1) begin
        section_out1 <= 0;
      end
      else begin
        if (clk_enable_stage2 == 1'b1) begin
          section_out1 <= sum1;
        end
      end
    end // integrator_delay_section1

  //   ------------------ Section # 2 : Integrator ------------------

  assign section_in2 = section_out1;

  assign add_signext_2 = section_in2;
  assign add_signext_3 = section_out2;
  assign add_temp_1 = add_signext_2 + add_signext_3;
  assign sum2 = add_temp_1[27:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section2
      if (reset == 1'b1) begin
        section_out2 <= 0;
      end
      else begin
        if (clk_enable_stage2 == 1'b1) begin
          section_out2 <= sum2;
        end
      end
    end // integrator_delay_section2

  //   ------------------ Section # 3 : Integrator ------------------

  assign section_in3 = section_out2;

  assign add_signext_4 = section_in3;
  assign add_signext_5 = section_out3;
  assign add_temp_2 = add_signext_4 + add_signext_5;
  assign sum3 = add_temp_2[27:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section3
      if (reset == 1'b1) begin
        section_out3 <= 0;
      end
      else begin
        if (clk_enable_stage2 == 1'b1) begin
          section_out3 <= sum3;
        end
      end
    end // integrator_delay_section3

  //   ------------------ Section # 4 : Integrator ------------------

  assign section_in4 = section_out3;

  assign add_signext_6 = section_in4;
  assign add_signext_7 = section_out4;
  assign add_temp_3 = add_signext_6 + add_signext_7;
  assign sum4 = add_temp_3[27:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section4
      if (reset == 1'b1) begin
        section_out4 <= 0;
      end
      else begin
        if (clk_enable_stage2 == 1'b1) begin
          section_out4 <= sum4;
        end
      end
    end // integrator_delay_section4

  //   ------------------ Section # 5 : Integrator ------------------

  assign section_in5 = section_out4;

  assign add_signext_8 = section_in5;
  assign add_signext_9 = section_out5;
  assign add_temp_4 = add_signext_8 + add_signext_9;
  assign sum5 = add_temp_4[27:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section5
      if (reset == 1'b1) begin
        section_out5 <= 0;
      end
      else begin
        if (clk_enable_stage2 == 1'b1) begin
          section_out5 <= sum5;
        end
      end
    end // integrator_delay_section5

  //   ------------------ Section # 6 : Integrator ------------------

  assign section_in6 = section_out5;

  assign add_signext_10 = section_in6;
  assign add_signext_11 = section_out6;
  assign add_temp_5 = add_signext_10 + add_signext_11;
  assign sum6 = add_temp_5[27:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section6
      if (reset == 1'b1) begin
        section_out6 <= 0;
      end
      else begin
        if (clk_enable_stage2 == 1'b1) begin
          section_out6 <= sum6;
        end
      end
    end // integrator_delay_section6

  //   ------------------ Section # 7 : Integrator ------------------

  assign section_in7 = section_out6;

  assign add_signext_12 = section_in7;
  assign add_signext_13 = section_out7;
  assign add_temp_6 = add_signext_12 + add_signext_13;
  assign sum7 = add_temp_6[27:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section7
      if (reset == 1'b1) begin
        section_out7 <= 0;
      end
      else begin
        if (clk_enable_stage2 == 1'b1) begin
          section_out7 <= sum7;
        end
      end
    end // integrator_delay_section7

  //   ------------------ Section # 8 : Integrator ------------------

  assign section_in8 = section_out7;

  assign add_signext_14 = section_in8;
  assign add_signext_15 = section_out8;
  assign add_temp_7 = add_signext_14 + add_signext_15;
  assign sum8 = add_temp_7[27:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section8
      if (reset == 1'b1) begin
        section_out8 <= 0;
      end
      else begin
        if (clk_enable_stage2 == 1'b1) begin
          section_out8 <= sum8;
        end
      end
    end // integrator_delay_section8

  //   ------------------ Section # 9 : Integrator ------------------

  assign section_in9 = section_out8;

  assign add_signext_16 = section_in9;
  assign add_signext_17 = section_out9;
  assign add_temp_8 = add_signext_16 + add_signext_17;
  assign sum9 = add_temp_8[27:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section9
      if (reset == 1'b1) begin
        section_out9 <= 0;
      end
      else begin
        if (clk_enable_stage2 == 1'b1) begin
          section_out9 <= sum9;
        end
      end
    end // integrator_delay_section9

  //   ------------------ Section # 10 : Integrator ------------------

  assign section_in10 = section_out9;

  assign add_signext_18 = section_in10;
  assign add_signext_19 = section_out10;
  assign add_temp_9 = add_signext_18 + add_signext_19;
  assign sum10 = add_temp_9[27:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section10
      if (reset == 1'b1) begin
        section_out10 <= 0;
      end
      else begin
        if (clk_enable_stage2 == 1'b1) begin
          section_out10 <= sum10;
        end
      end
    end // integrator_delay_section10

  //   ------------------ Section # 11 : Comb ------------------

  assign section_in11 = section_out10;

  assign sub_signext = section_in11;
  assign sub_signext_1 = diff1;
  assign sub_temp = sub_signext - sub_signext_1;
  assign section_out11 = sub_temp[27:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section11
      if (reset == 1'b1) begin
        diff1 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          diff1 <= section_in11;
        end
      end
    end // comb_delay_section11

  always @ (posedge clk or posedge reset)
    begin: cic_pipeline_process_section11
      if (reset == 1'b1) begin
        cic_pipeline11 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          cic_pipeline11 <= section_out11;
        end
      end
    end // cic_pipeline_process_section11

  //   ------------------ Section # 12 : Comb ------------------

  assign section_in12 = cic_pipeline11;

  assign sub_signext_2 = section_in12;
  assign sub_signext_3 = diff2;
  assign sub_temp_1 = sub_signext_2 - sub_signext_3;
  assign section_out12 = sub_temp_1[27:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section12
      if (reset == 1'b1) begin
        diff2 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          diff2 <= section_in12;
        end
      end
    end // comb_delay_section12

  always @ (posedge clk or posedge reset)
    begin: cic_pipeline_process_section12
      if (reset == 1'b1) begin
        cic_pipeline12 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          cic_pipeline12 <= section_out12;
        end
      end
    end // cic_pipeline_process_section12

  //   ------------------ Section # 13 : Comb ------------------

  assign section_in13 = cic_pipeline12;

  assign sub_signext_4 = section_in13;
  assign sub_signext_5 = diff3;
  assign sub_temp_2 = sub_signext_4 - sub_signext_5;
  assign section_out13 = sub_temp_2[27:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section13
      if (reset == 1'b1) begin
        diff3 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          diff3 <= section_in13;
        end
      end
    end // comb_delay_section13

  always @ (posedge clk or posedge reset)
    begin: cic_pipeline_process_section13
      if (reset == 1'b1) begin
        cic_pipeline13 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          cic_pipeline13 <= section_out13;
        end
      end
    end // cic_pipeline_process_section13

  //   ------------------ Section # 14 : Comb ------------------

  assign section_in14 = cic_pipeline13;

  assign sub_signext_6 = section_in14;
  assign sub_signext_7 = diff4;
  assign sub_temp_3 = sub_signext_6 - sub_signext_7;
  assign section_out14 = sub_temp_3[27:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section14
      if (reset == 1'b1) begin
        diff4 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          diff4 <= section_in14;
        end
      end
    end // comb_delay_section14

  always @ (posedge clk or posedge reset)
    begin: cic_pipeline_process_section14
      if (reset == 1'b1) begin
        cic_pipeline14 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          cic_pipeline14 <= section_out14;
        end
      end
    end // cic_pipeline_process_section14

  //   ------------------ Section # 15 : Comb ------------------

  assign section_in15 = cic_pipeline14;

  assign sub_signext_8 = section_in15;
  assign sub_signext_9 = diff5;
  assign sub_temp_4 = sub_signext_8 - sub_signext_9;
  assign section_out15 = sub_temp_4[27:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section15
      if (reset == 1'b1) begin
        diff5 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          diff5 <= section_in15;
        end
      end
    end // comb_delay_section15

  always @ (posedge clk or posedge reset)
    begin: cic_pipeline_process_section15
      if (reset == 1'b1) begin
        cic_pipeline15 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          cic_pipeline15 <= section_out15;
        end
      end
    end // cic_pipeline_process_section15

  //   ------------------ Section # 16 : Comb ------------------

  assign section_in16 = cic_pipeline15;

  assign sub_signext_10 = section_in16;
  assign sub_signext_11 = diff6;
  assign sub_temp_5 = sub_signext_10 - sub_signext_11;
  assign section_out16 = sub_temp_5[27:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section16
      if (reset == 1'b1) begin
        diff6 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          diff6 <= section_in16;
        end
      end
    end // comb_delay_section16

  always @ (posedge clk or posedge reset)
    begin: cic_pipeline_process_section16
      if (reset == 1'b1) begin
        cic_pipeline16 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          cic_pipeline16 <= section_out16;
        end
      end
    end // cic_pipeline_process_section16

  //   ------------------ Section # 17 : Comb ------------------

  assign section_in17 = cic_pipeline16;

  assign sub_signext_12 = section_in17;
  assign sub_signext_13 = diff7;
  assign sub_temp_6 = sub_signext_12 - sub_signext_13;
  assign section_out17 = sub_temp_6[27:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section17
      if (reset == 1'b1) begin
        diff7 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          diff7 <= section_in17;
        end
      end
    end // comb_delay_section17

  always @ (posedge clk or posedge reset)
    begin: cic_pipeline_process_section17
      if (reset == 1'b1) begin
        cic_pipeline17 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          cic_pipeline17 <= section_out17;
        end
      end
    end // cic_pipeline_process_section17

  //   ------------------ Section # 18 : Comb ------------------

  assign section_in18 = cic_pipeline17;

  assign sub_signext_14 = section_in18;
  assign sub_signext_15 = diff8;
  assign sub_temp_7 = sub_signext_14 - sub_signext_15;
  assign section_out18 = sub_temp_7[27:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section18
      if (reset == 1'b1) begin
        diff8 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          diff8 <= section_in18;
        end
      end
    end // comb_delay_section18

  always @ (posedge clk or posedge reset)
    begin: cic_pipeline_process_section18
      if (reset == 1'b1) begin
        cic_pipeline18 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          cic_pipeline18 <= section_out18;
        end
      end
    end // cic_pipeline_process_section18

  //   ------------------ Section # 19 : Comb ------------------

  assign section_in19 = cic_pipeline18;

  assign sub_signext_16 = section_in19;
  assign sub_signext_17 = diff9;
  assign sub_temp_8 = sub_signext_16 - sub_signext_17;
  assign section_out19 = sub_temp_8[27:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section19
      if (reset == 1'b1) begin
        diff9 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          diff9 <= section_in19;
        end
      end
    end // comb_delay_section19

  always @ (posedge clk or posedge reset)
    begin: cic_pipeline_process_section19
      if (reset == 1'b1) begin
        cic_pipeline19 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          cic_pipeline19 <= section_out19;
        end
      end
    end // cic_pipeline_process_section19

  //   ------------------ Section # 20 : Comb ------------------

  assign section_in20 = cic_pipeline19;

  assign section_cast20 = section_in20[27:1];

  assign sub_signext_18 = section_cast20;
  assign sub_signext_19 = diff10;
  assign sub_temp_9 = sub_signext_18 - sub_signext_19;
  assign section_out20 = sub_temp_9[26:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section20
      if (reset == 1'b1) begin
        diff10 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          diff10 <= section_cast20;
        end
      end
    end // comb_delay_section20

  assign output_typeconvert = section_out20[26:3];

  //   ------------------ Output Register ------------------

  always @ (posedge clk or posedge reset)
    begin: output_reg_process
      if (reset == 1'b1) begin
        output_register <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          output_register <= output_typeconvert;
        end
      end
    end // output_reg_process

  // Assignment Statements
  assign ce_out_stage2 = ce_out_reg;
  assign filter_out_stage2 = output_register;
endmodule  // CASCADE_D520_stage2
