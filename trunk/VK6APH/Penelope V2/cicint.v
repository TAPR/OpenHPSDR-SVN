// -------------------------------------------------------------
//
// Module: cicint
//
// Generated by MATLAB(R) 7.2 and the Filter Design HDL Coder 1.4.
//
// Generated on: 2007-09-09 20:28:44
//
// -------------------------------------------------------------

// -------------------------------------------------------------
// HDL Code Generation Options:
//
// TargetLanguage: Verilog
// Name: cicint
// TargetDirectory: C:\DOCUME~1\Philip\LOCALS~1\Temp\tp284267\hdlsrc
// SerialPartition: -1
// CastBeforeSum: On
//
// Filter Settings:
//
// Discrete-Time FIR Multirate Filter (real)
// -----------------------------------------
// Filter Structure        : Cascaded Integrator-Comb Interpolator
// Interpolation Factor    : 2560
// Differential Delay      : 1
// Number of Sections      : 5
// Stable                  : Yes
// Linear Phase            : Yes (Type 2)
//
// Input                   : s16,0
// Output                  : s16,-46
// Filter Internals        : Minimum Word Lengths
//   Integrator Section 1  : s17,0
//   Integrator Section 2  : s18,0
//   Integrator Section 3  : s19,0
//   Integrator Section 4  : s20,0
//   Integrator Section 5  : s20,0
//   Comb Section 1        : s20,0
//   Comb Section 2        : s31,0
//   Comb Section 3        : s41,0
//   Comb Section 4        : s51,0
//   Comb Section 5        : s62,0
//
// Implementation Cost
// Number of Multipliers : 0
// Number of Adders      : 10
// Number of States      : 10
// MultPerInputSample    : 0
// AddPerInputSample     : 12805
// -------------------------------------------------------------

`timescale 1 ns / 1 ns

module cicint 
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
  input   signed [15:0] filter_in; //sfix16
  output  signed [15:0] filter_out; //sfix16_E46
  output  ce_out; 

////////////////////////////////////////////////////////////////
//Module Architecture: cicint
////////////////////////////////////////////////////////////////
  // Local Functions
  // Type Definitions
  // Constants
  parameter signed [19:0] zeroconst = 20'b00000000000000000000; //sfix20
  // Signals
  reg  [11:0] cur_count; // ufix12
  wire phase_0; // boolean
  reg  ce_out_reg; // boolean
  //   
  reg  signed [15:0] input_register; // sfix16
  //   -- Section 1 Signals 
  wire signed [15:0] section_in1; // sfix16
  wire signed [16:0] section_cast1; // sfix17
  reg  signed [16:0] diff1; // sfix17
  wire signed [16:0] section_out1; // sfix17
  wire signed [16:0] sub_cast; // sfix17
  wire signed [16:0] sub_cast_1; // sfix17
  wire signed [17:0] sub_temp; // sfix18
  //   -- Section 2 Signals 
  wire signed [16:0] section_in2; // sfix17
  wire signed [17:0] section_cast2; // sfix18
  reg  signed [17:0] diff2; // sfix18
  wire signed [17:0] section_out2; // sfix18
  wire signed [17:0] sub_cast_2; // sfix18
  wire signed [17:0] sub_cast_3; // sfix18
  wire signed [18:0] sub_temp_1; // sfix19
  //   -- Section 3 Signals 
  wire signed [17:0] section_in3; // sfix18
  wire signed [18:0] section_cast3; // sfix19
  reg  signed [18:0] diff3; // sfix19
  wire signed [18:0] section_out3; // sfix19
  wire signed [18:0] sub_cast_4; // sfix19
  wire signed [18:0] sub_cast_5; // sfix19
  wire signed [19:0] sub_temp_2; // sfix20
  //   -- Section 4 Signals 
  wire signed [18:0] section_in4; // sfix19
  wire signed [19:0] section_cast4; // sfix20
  reg  signed [19:0] diff4; // sfix20
  wire signed [19:0] section_out4; // sfix20
  wire signed [19:0] sub_cast_6; // sfix20
  wire signed [19:0] sub_cast_7; // sfix20
  wire signed [20:0] sub_temp_3; // sfix21
  //   -- Section 5 Signals 
  wire signed [19:0] section_in5; // sfix20
  reg  signed [19:0] diff5; // sfix20
  wire signed [19:0] section_out5; // sfix20
  wire signed [19:0] sub_cast_8; // sfix20
  wire signed [19:0] sub_cast_9; // sfix20
  wire signed [20:0] sub_temp_4; // sfix21
  wire signed [19:0] upsampling; // sfix20
  //   -- Section 6 Signals 
  wire signed [19:0] section_in6; // sfix20
  wire signed [19:0] sum1; // sfix20
  reg  signed [19:0] section_out6; // sfix20
  wire signed [19:0] add_cast; // sfix20
  wire signed [19:0] add_cast_1; // sfix20
  wire signed [20:0] add_temp; // sfix21
  //   -- Section 7 Signals 
  wire signed [19:0] section_in7; // sfix20
  wire signed [30:0] section_cast7; // sfix31
  wire signed [30:0] sum2; // sfix31
  reg  signed [30:0] section_out7; // sfix31
  wire signed [30:0] add_cast_2; // sfix31
  wire signed [30:0] add_cast_3; // sfix31
  wire signed [31:0] add_temp_1; // sfix32
  //   -- Section 8 Signals 
  wire signed [30:0] section_in8; // sfix31
  wire signed [40:0] section_cast8; // sfix41
  wire signed [40:0] sum3; // sfix41
  reg  signed [40:0] section_out8; // sfix41
  wire signed [40:0] add_cast_4; // sfix41
  wire signed [40:0] add_cast_5; // sfix41
  wire signed [41:0] add_temp_2; // sfix42
  //   -- Section 9 Signals 
  wire signed [40:0] section_in9; // sfix41
  wire signed [50:0] section_cast9; // sfix51
  wire signed [50:0] sum4; // sfix51
  reg  signed [50:0] section_out9; // sfix51
  wire signed [50:0] add_cast_6; // sfix51
  wire signed [50:0] add_cast_7; // sfix51
  wire signed [51:0] add_temp_3; // sfix52
  //   -- Section 10 Signals 
  wire signed [50:0] section_in10; // sfix51
  wire signed [61:0] section_cast10; // sfix62
  wire signed [61:0] sum5; // sfix62
  reg  signed [61:0] section_out10; // sfix62
  wire signed [61:0] add_cast_8; // sfix62
  wire signed [61:0] add_cast_9; // sfix62
  wire signed [62:0] add_temp_4; // sfix63
  wire signed [15:0] output_typeconvert; // sfix16_E46
  //   
  reg  signed [15:0] output_register; // sfix16_E46

  // Block Statements
  //   ------------------ CE Output Generation ------------------

  always @ (posedge clk or posedge reset)
    begin: ce_output
      if (reset == 1'b1) begin
        cur_count <= 12'b000000000000;
      end
      else begin
        if (clk_enable == 1'b1) begin
          if (cur_count == 12'b100111111111) begin
            cur_count <= 12'b000000000000;
          end
          else begin
            cur_count <= cur_count + 1;
          end
        end
      end
    end // ce_output

  assign  phase_0 = (cur_count == 12'b000000000000 && clk_enable == 1'b1)? 1 : 0;

  //   ------------------ CE Output Register ------------------

  always @ (posedge clk or posedge reset)
    begin: ce_output_register
      if (reset == 1'b1) begin
        ce_out_reg <= 1'b0;
      end
      else begin
          ce_out_reg <= phase_0;
      end
    end // ce_output_register

  //   ------------------ Input Register ------------------

  always @ (posedge clk or posedge reset)
    begin: input_reg_process
      if (reset == 1'b1) begin
        input_register <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          input_register <= filter_in;
        end
      end
    end // input_reg_process

  //   ------------------ Section # 1 : Comb ------------------

  assign section_in1 = input_register;

  assign section_cast1 = $signed({{1{section_in1[15]}}, section_in1});

  assign sub_cast = section_cast1;
  assign sub_cast_1 = diff1;
  assign sub_temp = sub_cast - sub_cast_1;
  assign section_out1 = sub_temp[16:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section1
      if (reset == 1'b1) begin
        diff1 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          diff1 <= section_cast1;
        end
      end
    end // comb_delay_section1

  //   ------------------ Section # 2 : Comb ------------------

  assign section_in2 = section_out1;

  assign section_cast2 = $signed({{1{section_in2[16]}}, section_in2});

  assign sub_cast_2 = section_cast2;
  assign sub_cast_3 = diff2;
  assign sub_temp_1 = sub_cast_2 - sub_cast_3;
  assign section_out2 = sub_temp_1[17:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section2
      if (reset == 1'b1) begin
        diff2 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          diff2 <= section_cast2;
        end
      end
    end // comb_delay_section2

  //   ------------------ Section # 3 : Comb ------------------

  assign section_in3 = section_out2;

  assign section_cast3 = $signed({{1{section_in3[17]}}, section_in3});

  assign sub_cast_4 = section_cast3;
  assign sub_cast_5 = diff3;
  assign sub_temp_2 = sub_cast_4 - sub_cast_5;
  assign section_out3 = sub_temp_2[18:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section3
      if (reset == 1'b1) begin
        diff3 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          diff3 <= section_cast3;
        end
      end
    end // comb_delay_section3

  //   ------------------ Section # 4 : Comb ------------------

  assign section_in4 = section_out3;

  assign section_cast4 = $signed({{1{section_in4[18]}}, section_in4});

  assign sub_cast_6 = section_cast4;
  assign sub_cast_7 = diff4;
  assign sub_temp_3 = sub_cast_6 - sub_cast_7;
  assign section_out4 = sub_temp_3[19:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section4
      if (reset == 1'b1) begin
        diff4 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          diff4 <= section_cast4;
        end
      end
    end // comb_delay_section4

  //   ------------------ Section # 5 : Comb ------------------

  assign section_in5 = section_out4;

  assign sub_cast_8 = section_in5;
  assign sub_cast_9 = diff5;
  assign sub_temp_4 = sub_cast_8 - sub_cast_9;
  assign section_out5 = sub_temp_4[19:0];

  always @ (posedge clk or posedge reset)
    begin: comb_delay_section5
      if (reset == 1'b1) begin
        diff5 <= 0;
      end
      else begin
        if (phase_0 == 1'b1) begin
          diff5 <= section_in5;
        end
      end
    end // comb_delay_section5

  assign upsampling = (phase_0 == 1'b1) ? section_out5 : zeroconst;
  //   ------------------ Section # 6 : Integrator ------------------

  assign section_in6 = upsampling;

  assign add_cast = section_in6;
  assign add_cast_1 = section_out6;
  assign add_temp = add_cast + add_cast_1;
  assign sum1 = add_temp[19:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section6
      if (reset == 1'b1) begin
        section_out6 <= 0;
      end
      else begin
        if (clk_enable == 1'b1) begin
          section_out6 <= sum1;
        end
      end
    end // integrator_delay_section6

  //   ------------------ Section # 7 : Integrator ------------------

  assign section_in7 = section_out6;

  assign section_cast7 = $signed({{11{section_in7[19]}}, section_in7});

  assign add_cast_2 = section_cast7;
  assign add_cast_3 = section_out7;
  assign add_temp_1 = add_cast_2 + add_cast_3;
  assign sum2 = add_temp_1[30:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section7
      if (reset == 1'b1) begin
        section_out7 <= 0;
      end
      else begin
        if (clk_enable == 1'b1) begin
          section_out7 <= sum2;
        end
      end
    end // integrator_delay_section7

  //   ------------------ Section # 8 : Integrator ------------------

  assign section_in8 = section_out7;

  assign section_cast8 = $signed({{10{section_in8[30]}}, section_in8});

  assign add_cast_4 = section_cast8;
  assign add_cast_5 = section_out8;
  assign add_temp_2 = add_cast_4 + add_cast_5;
  assign sum3 = add_temp_2[40:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section8
      if (reset == 1'b1) begin
        section_out8 <= 0;
      end
      else begin
        if (clk_enable == 1'b1) begin
          section_out8 <= sum3;
        end
      end
    end // integrator_delay_section8

  //   ------------------ Section # 9 : Integrator ------------------

  assign section_in9 = section_out8;

  assign section_cast9 = $signed({{10{section_in9[40]}}, section_in9});

  assign add_cast_6 = section_cast9;
  assign add_cast_7 = section_out9;
  assign add_temp_3 = add_cast_6 + add_cast_7;
  assign sum4 = add_temp_3[50:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section9
      if (reset == 1'b1) begin
        section_out9 <= 0;
      end
      else begin
        if (clk_enable == 1'b1) begin
          section_out9 <= sum4;
        end
      end
    end // integrator_delay_section9

  //   ------------------ Section # 10 : Integrator ------------------

  assign section_in10 = section_out9;

  assign section_cast10 = $signed({{11{section_in10[50]}}, section_in10});

  assign add_cast_8 = section_cast10;
  assign add_cast_9 = section_out10;
  assign add_temp_4 = add_cast_8 + add_cast_9;
  assign sum5 = add_temp_4[61:0];

  always @ (posedge clk or posedge reset)
    begin: integrator_delay_section10
      if (reset == 1'b1) begin
        section_out10 <= 0;
      end
      else begin
        if (clk_enable == 1'b1) begin
          section_out10 <= sum5;
        end
      end
    end // integrator_delay_section10

  assign output_typeconvert = section_out10[61:46];

  //   ------------------ Output Register ------------------

  always @ (posedge clk or posedge reset)
    begin: output_reg_process
      if (reset == 1'b1) begin
        output_register <= 0;
      end
      else begin
        if (clk_enable == 1'b1) begin
          output_register <= output_typeconvert;
        end
      end
    end // output_reg_process

  // Assignment Statements
  assign ce_out = ce_out_reg;
  assign filter_out = output_register;
endmodule  // cicint
