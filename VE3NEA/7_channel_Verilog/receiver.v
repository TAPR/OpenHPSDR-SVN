/*
--------------------------------------------------------------------------------
This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Library General Public
License as published by the Free Software Foundation; either
version 2 of the License, or (at your option) any later version.
This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Library General Public License for more details.
You should have received a copy of the GNU Library General Public
License along with this library; if not, write to the
Free Software Foundation, Inc., 51 Franklin St, Fifth Floor,
Boston, MA  02110-1301, USA.
--------------------------------------------------------------------------------
*/


//------------------------------------------------------------------------------
//           Copyright (c) 2008 Alex Shovkoplyas, VE3NEA
//------------------------------------------------------------------------------



module receiver(
  input clock,
  input fir_start,
  input [31:0] frequency,

  input signed [15:0] in_data,
  input signed [23:0] fir_coeff,

  output [31:0] out_data_I,
  output [31:0] out_data_Q
  );






//------------------------------------------------------------------------------
//                               cordic
//------------------------------------------------------------------------------
cordic cordic_inst(
  .clock(clock),
  .in_data(in_data),             //16 bit 
  .frequency(frequency),         //32 bit
  .out_data_I(cordic_outdata_I), //22 bit
  .out_data_Q(cordic_outdata_Q)
  );


wire signed [21:0] cordic_outdata_I;
wire signed [21:0] cordic_outdata_Q;






//------------------------------------------------------------------------------
//                     register-based CIC decimator
//------------------------------------------------------------------------------
//I channel
cic #(.STAGES(3), .DECIMATION(50), .IN_WIDTH(22), .ACC_WIDTH(39), .OUT_WIDTH(24))
  cic_inst_I1(
    .clock(clock),
    .in_strobe(1'b1),
    .out_strobe(cic_outstrobe_1),
    .in_data(cordic_outdata_I),
    .out_data(cic_outdata_I1)
    );


//Q channel
cic #(.STAGES(3), .DECIMATION(50), .IN_WIDTH(22), .ACC_WIDTH(39), .OUT_WIDTH(24))
  cic_inst_Q1(
    .clock(clock),
    .in_strobe(1'b1),
    .out_strobe(),
    .in_data(cordic_outdata_Q),
    .out_data(cic_outdata_Q1)
    );


wire cic_outstrobe_1;
wire signed [23:0] cic_outdata_I1;
wire signed [23:0] cic_outdata_Q1;






//------------------------------------------------------------------------------
//                       memory-based CIC decimator
//------------------------------------------------------------------------------
memcic #(.STAGES(10), .DECIMATION(26)) 
  memcic_inst_I(
    .clock(clock),
    .in_strobe(cic_outstrobe_1),
    .out_strobe(),
    .in_data(cic_outdata_I1),
    .out_data(cic_outdata_I2)
    );


memcic #(.STAGES(10), .DECIMATION(26)) 
  memcic_inst_Q(
    .clock(clock),
    .in_strobe(cic_outstrobe_1),
    .out_strobe(),
    .in_data(cic_outdata_Q1),
    .out_data(cic_outdata_Q2)
    );


wire signed [23:0] cic_outdata_I2;
wire signed [23:0] cic_outdata_Q2;






//------------------------------------------------------------------------------
//                            FIR decimator
//------------------------------------------------------------------------------
fir fir_inst_I(
  .clock(clock),
  .start(fir_start), 
  .coeff(fir_coeff),
  .in_data(cic_outdata_I2),
  .out_data(out_data_I)
  );


fir fir_inst_Q(
  .clock(clock),
  .start(fir_start),
  .coeff(fir_coeff),
  .in_data(cic_outdata_Q2),
  .out_data(out_data_Q)
  );



endmodule
