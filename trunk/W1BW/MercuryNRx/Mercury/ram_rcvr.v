//------------------------------------------------------------------------------
//           Copyright (c) 2009 Kirk Weedman KD7IRS
//------------------------------------------------------------------------------

module ram_rcvr(
  input        rst,
  input        clk,              //122.88 MHz
  input  [1:0] rate,             //00=48, 01=96, 10=192 kHz
  input [31:0] frequency,
  input        in_strobe,
  output       out_strobe,

  input signed [15:0] in_data,

  output       [23:0] out_data_I,
  output       [23:0] out_data_Q
  );

wire signed [21:0] cordic_outdata_I;
wire signed [21:0] cordic_outdata_Q;

//------------------------------------------------------------------------------
//                               cordic
//------------------------------------------------------------------------------

cordic cordic_inst(
  .clock(clk),
  .in_data(in_data),             //16 bit 
  .frequency(frequency),         //32 bit
  .out_data_I(cordic_outdata_I), //22 bit
  .out_data_Q(cordic_outdata_Q)
  );

//------------------------------------------------------------------------------
//                CIC decimator #1, decimation factor 80/160/320
//------------------------------------------------------------------------------
//I channel

//ACC_WIDTH = 31; // 22 + STAGES*log2(320);
//OUT_WIDTH reduced by 3 bits to allow rounding for decimations of 80/160/320

wire cic_outstrobe_1;
wire signed [27:0] cic_outdata_I1;
wire signed [27:0] cic_outdata_Q1;

varcic #(.STAGES(1), .DECIMATION(80), .IN_WIDTH(22), .ACC_WIDTH(31), .OUT_WIDTH(28))
  varcic_inst_I1(
    .clock(clk),
    .in_strobe(1'b1),
    .extra_decimation(rate),
    .out_strobe(cic_outstrobe_1),
    .in_data(cordic_outdata_I),
    .out_data(cic_outdata_I1)
    );


//Q channel
varcic #(.STAGES(1), .DECIMATION(80), .IN_WIDTH(22), .ACC_WIDTH(31), .OUT_WIDTH(28))
  varcic_inst_Q1(
    .clock(clk),
    .in_strobe(1'b1),
    .extra_decimation(rate),
    .out_strobe(),
    .in_data(cordic_outdata_Q),
    .out_data(cic_outdata_Q1)
    );

//------------------------------------------------------------------------------
//                  RAMCIC decimator #2, decimation factor 4
//------------------------------------------------------------------------------
//I channel
wire               cic_outstrobe_2;
wire signed [23:0] cic_outdata_I2;
wire signed [23:0] cic_outdata_Q2;

//ACC_WIDTH = 66; // 28 + STAGES*log2(4) Bit gain due to STAGES and Decimation by 4

//Doesnt work -??? ramcic #(.STAGES(15), .DECIMATION(4), .IN_WIDTH(28), .OUT_WIDTH(24))
ramcic #(.STAGES(12), .DECIMATION(4), .IN_WIDTH(28),  .OUT_WIDTH(24))
  cic_inst_I2(
    .rst(rst),
    .clk(clk),
    .in_strobe(cic_outstrobe_1),
    .out_strobe(cic_outstrobe_2),
    .in_data(cic_outdata_I1),
    .out_data(cic_outdata_I2)
    );


//Q channel
//Doesnt work -??? ramcic #(.STAGES(15), .DECIMATION(4), .IN_WIDTH(28), .OUT_WIDTH(24))
ramcic #(.STAGES(12), .DECIMATION(4), .IN_WIDTH(28), .OUT_WIDTH(24))
  cic_inst_Q2(
    .rst(rst),
    .clk(clk),
    .in_strobe(cic_outstrobe_1),
    .out_strobe(),
    .in_data(cic_outdata_Q1),
    .out_data(cic_outdata_Q2)
    );

//------------------------------------------------------------------------------
//                     FIR coefficients and sequencing
//------------------------------------------------------------------------------
wire signed [23:0] fir_coeff;

fir_coeffs fir_coeffs_inst(
  .clock(clk),
  .start(cic_outstrobe_2),
  .coeff(fir_coeff)
  );


//------------------------------------------------------------------------------
//                            FIR decimator
//------------------------------------------------------------------------------
fir #(.OUT_WIDTH(24))
  fir_inst_I(
    .clock(clk),
    .start(cic_outstrobe_2), 
    .coeff(fir_coeff),
    .in_data(cic_outdata_I2),
    .out_data(out_data_I),
    .out_strobe(out_strobe)
    );


fir #(.OUT_WIDTH(24))
  fir_inst_Q(
    .clock(clk),
    .start(cic_outstrobe_2),
    .coeff(fir_coeff),
    .in_data(cic_outdata_Q2),
    .out_data(out_data_Q),
    .out_strobe()
    );



endmodule
