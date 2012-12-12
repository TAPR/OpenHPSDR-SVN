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



module receiver_vna(
  input clock,                  //122.88 MHz
  input vna,						  // set if in VNA mode
  input [1:0] rate,             //00=48, 01=96, 10=192 kHz
  input [31:0] frequency,
  output out_strobe,

  input signed [15:0] in_data,

  output signed[23:0] out_data_I,
  output signed[23:0] out_data_Q
  );

wire signed [21:0] cordic_outdata_I;
wire signed [21:0] cordic_outdata_Q;
wire signed [23:0] rx_out_data_I;
wire signed [23:0] rx_out_data_Q;

// select output data relative to receiver or VNA mode

//assign out_data_I = vna ? vna_out_I : rx_out_data_I;
//assign out_data_Q = vna ? vna_out_Q : rx_out_data_Q;

assign out_data_I = rx_out_data_I;  // use this code to force VNA to use CIC + CFIR output 
assign out_data_Q = rx_out_data_Q;


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

//------------------------------------------------------------------------------
//                CIC decimator #1, decimation factor 80/160/320
//------------------------------------------------------------------------------
//I channel

wire cic_outstrobe_1;
wire signed [22:0] cic_outdata_I1;
wire signed [22:0] cic_outdata_Q1;

varcic #(.STAGES(5), .DECIMATION(80), .IN_WIDTH(22), .ACC_WIDTH(64), .OUT_WIDTH(23))
  varcic_inst_I1(
    .clock(clock),
    .in_strobe(1'b1),
    .extra_decimation(rate),
    .out_strobe(cic_outstrobe_1),
    .in_data(cordic_outdata_I),
    .out_data(cic_outdata_I1)
    );


//Q channel
varcic #(.STAGES(5), .DECIMATION(80), .IN_WIDTH(22), .ACC_WIDTH(64), .OUT_WIDTH(23))
  varcic_inst_Q1(
    .clock(clock),
    .in_strobe(1'b1),
    .extra_decimation(rate),
    .out_strobe(),
    .in_data(cordic_outdata_Q),
    .out_data(cic_outdata_Q1)
    );

//------------------------------------------------------------------------------
//                  CIC decimator #2, decimation factor 4
//------------------------------------------------------------------------------
//I channel
wire cic_outstrobe_2;
wire signed [23:0] cic_outdata_I2;
wire signed [23:0] cic_outdata_Q2;

cic #(.STAGES(11), .DECIMATION(4), .IN_WIDTH(23), .ACC_WIDTH(45), .OUT_WIDTH(24))
  cic_inst_I2(
    .clock(clock),
    .in_strobe(cic_outstrobe_1),
    .out_strobe(cic_outstrobe_2),
    .in_data(cic_outdata_I1),
    .out_data(cic_outdata_I2)
    );


//Q channel
cic #(.STAGES(11), .DECIMATION(4), .IN_WIDTH(23), .ACC_WIDTH(45), .OUT_WIDTH(24))
  cic_inst_Q2(
    .clock(clock),
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
  .clock(clock),
  .start(cic_outstrobe_2),
  .coeff(fir_coeff)
  );


//------------------------------------------------------------------------------
//                            FIR decimator
//------------------------------------------------------------------------------
fir #(.OUT_WIDTH(24))
  fir_inst_I(
    .clock(clock),
    .start(cic_outstrobe_2), 
    .coeff(fir_coeff),
    .in_data(cic_outdata_I2),
    .out_data(rx_out_data_I),
    .out_strobe(out_strobe)
    );


fir #(.OUT_WIDTH(24))
  fir_inst_Q(
    .clock(clock),
    .start(cic_outstrobe_2),
    .coeff(fir_coeff),
    .in_data(cic_outdata_Q2),
    .out_data(rx_out_data_Q),
    .out_strobe()
    );

//-----------------------------------------------------------------------------------
//                           VNA 
//-----------------------------------------------------------------------------------

// average the CORDIC I&Q samples by 512 so is independant of sampling rate
// Note samples are 2's complement so need to set variables to signed. 

reg [9:0]  count;
reg signed [23:0] vna_out_I;
reg signed [23:0] vna_out_Q;

reg signed [31:0] sum_I;
reg signed [31:0] sum_Q;

localparam average = 10'd512;  // number of consecutive samples to sum 
	
always @ (posedge clock)
begin 
	if (vna) begin 
		if (out_strobe)begin	
			count <= 0; 						// reset average counter and sums
			sum_I <= 0;
			sum_Q <= 0;
		end	
		else if (count != average) begin
			sum_I <= sum_I + cordic_outdata_I;   // accumalate 512 samples 
			sum_Q <= sum_Q + cordic_outdata_Q;
			count <= count + 1'd1;
		end 
		else begin								// count is zero so latch summed values and divide by 512
			vna_out_I <= sum_I/512;			
			vna_out_Q <= sum_Q/512;
		end
	end
end 

endmodule
