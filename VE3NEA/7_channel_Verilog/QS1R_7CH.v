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



module QS1R_7CH(
input ADC_clka,   //ADC clock, 125 MHz
input FX2_clk,    //FX2.8051 clock, 48 MHz
input FX2_ifclk,  //FX2.FIFO clock, 48 MHz

input [15:0] ADC_data,
input [2:0] FX2_flags,
input FX2_multi_wr,
input FX2_multi_clk,
input FX2_multi_stb,
input FX2_PC4,
input FX2_PC5,
input FX2_PC6,
input FX2_PC7,
input ADC_OVFL,

inout [14:0] GPIO,
inout [19:0] EXT,
inout [11:0] RFB_IO,
inout [7:0]  FX2_PE,
inout [15:0] FX2_fifodata,

output ADC_dith,
output ADC_rand,
output ADC_pga,
output ADC_shutdown,
output FX2_INT4,
output FX2_INT5,
output LED_OVFL,
output [3:0] LED,
output [1:0] FX2_fifoadr,
output FX2_pktend,
output FX2_sloe,
output FX2_slcs,
output FX2_slwr,
output FX2_slrd,
output DAC_LRCK,
output DAC_DATA,
output DAC_BCK,
output DAC_SCKI,
output DAC_PD
);


//unused
assign ADC_shutdown = 1'b0;
assign FX2_INT4 = 1'b0; //inactive when 0
assign FX2_INT5 = 1'b1; //inactive when 1
assign LED_OVFL = ~ADC_OVFL;
assign GPIO = 15'bZZZZZZZZZZZZZZZ;
assign EXT = 20'bZZZZZZZZZZZZZZZZZZZZ;
assign RFB_IO[11:4] = 8'bZZZZZZZZ;
assign LED[0] = 1'b0;
assign ADC_dith = 1'b0;
assign ADC_rand = 1'b1;
assign ADC_pga = 1'b0;
assign RFB_IO[3:0] = 4'bzzzz;
assign LED[1] = 1'b1;
assign LED[2] = 1'b1;
assign LED[3] = 1'b1;






//------------------------------------------------------------------------------
//                             derandomizer
//------------------------------------------------------------------------------
always @(posedge ADC_clka)
  adc_derandomized <= ADC_data ^ {{15{ADC_data[0]}}, 1'b0};


reg signed [15:0] adc_derandomized;






//------------------------------------------------------------------------------
//                     FIR coefficients and sequencing
//------------------------------------------------------------------------------
fir_coeffs fir_coeffs_inst(
  .clock(ADC_clka),
  .fir_start(fir_start),
  .fir_done(fir_done),
  .fir_coeff(fir_coeff)
  );


wire fir_start;
wire fir_done;
wire signed [23:0] fir_coeff;






//------------------------------------------------------------------------------
//                              receivers
//------------------------------------------------------------------------------
genvar i;
generate
  for (i=0; i<7; i=i+1)
    begin : receivers

      receiver receiver_inst(
        .clock(ADC_clka),
        //fir control
        .fir_start(fir_start),
        .fir_coeff(fir_coeff),
        //input
        .in_data(adc_derandomized),
        .frequency(multibus_out[i]),
        //output
        .out_data_I(recv_outdata[i*64+31 : i*64]),
        .out_data_Q(recv_outdata[i*64+63 : i*64+32])
        );
    end
endgenerate


wire [511:0] recv_outdata;






//------------------------------------------------------------------------------
//                               iq-to-usb fifo
//------------------------------------------------------------------------------
dc_fifo_64x512bit iq_fifo(
  .wrclk(ADC_clka),
  .wrreq(fir_done),
  .wrfull(),
  .data(recv_outdata),
  .rdclk(FX2_ifclk),
  .rdreq(iq_fifo_rdreq),
  .rdempty(iq_fifo_rdempty),
  .q(iq_fifo_q)
  );


wire iq_fifo_rdempty;
wire [511:0] iq_fifo_q;






//------------------------------------------------------------------------------
//                       usb endpoint read/write
//------------------------------------------------------------------------------
fx2endpoints fx2endpoints_inst(
  .FX2_ifclk(FX2_ifclk),
  .FX2_flags(FX2_flags),
  .FX2_fifodata(FX2_fifodata),
  .FX2_fifoadr(FX2_fifoadr),
  .FX2_sloe(FX2_sloe),
  .FX2_slrd(FX2_slrd),
  .FX2_slwr(FX2_slwr),
  .FX2_slcs(FX2_slcs),
  .FX2_pktend(FX2_pktend),

  .iq_fifo_rdempty(iq_fifo_rdempty),
  .iq_fifo_rdreq(iq_fifo_rdreq),
  .iq_fifo_q(iq_fifo_q),

  .dac_fifo_wrfull(dac_fifo_wrfull),
  .dac_fifo_wrreq(dac_fifo_wrreq),
  .dac_fifo_data(dac_fifo_data)
  );


wire iq_fifo_rdreq;
wire dac_fifo_wrreq;
wire [63:0] dac_fifo_data;






//------------------------------------------------------------------------------
//                             usb-to-dac fifo
//------------------------------------------------------------------------------
dc_fifo_64x48bit	dac_fifo(
  .wrclk(FX2_ifclk),
  .wrreq(dac_fifo_wrreq),
  .wrfull(dac_fifo_wrfull),
  .data({dac_fifo_data[63:40], dac_fifo_data[31:8]}),

  .rdclk(dac_clock),
  .rdreq(dac_fifo_rdreq),
  .rdempty(dac_fifo_rdempty),
  .q({dac_fifo_outdata_R, dac_fifo_outdata_L})
  );


wire dac_fifo_rdempty;
wire dac_fifo_wrfull;
wire [23:0] dac_fifo_outdata_L;
wire [23:0] dac_fifo_outdata_R;






//------------------------------------------------------------------------------
//                              DAC clock
//------------------------------------------------------------------------------
//48 kHz x 128
dac_pll	dac_pll_inst(
  .inclk0(ADC_clka),
  .c0(dac_clock)
  );


wire dac_clock;






//------------------------------------------------------------------------------
//                              PCM1771 DAC
//------------------------------------------------------------------------------
dac dac_inst(
  .clock(dac_clock),

  .DAC_PD(DAC_PD),
  .DAC_SCKI(DAC_SCKI),
  .DAC_BCK(DAC_BCK),
  .DAC_LRCK(DAC_LRCK),
  .DAC_DATA(DAC_DATA),

  .data_ready(~dac_fifo_rdempty),
  .data_rdreq(dac_fifo_rdreq),
  .data_L(dac_fifo_outdata_L),
  .data_R(dac_fifo_outdata_R)
);


wire dac_fifo_rdreq;






//------------------------------------------------------------------------------
//                                multibus
//------------------------------------------------------------------------------
localparam MB_REG_COUNT = 16;


multibus  #(MB_REG_COUNT) multibus_inst(
  .FX2_multi_clk(FX2_multi_clk),
  .FX2_multi_stb(FX2_multi_stb),
  .FX2_multi_wr(FX2_multi_wr),
  .FX2_PE(FX2_PE),

  .multibus_in_all(multibus_in_all),
  .multibus_out_all(multibus_out_all)
  );


wire [MB_REG_COUNT*32-1:0] multibus_in_all;
wire [MB_REG_COUNT*32-1:0] multibus_out_all;


wire [31:0] multibus_in [0:MB_REG_COUNT-1];
wire [31:0] multibus_out [0:MB_REG_COUNT-1];


//kludge; use SystemVerilog instead
genvar j;
generate
  for (j=0; j<MB_REG_COUNT; j=j+1)
    begin : mb_dword_mapping
    assign multibus_out[j] = multibus_out_all[(j*32)+31 : j*32];
    assign multibus_in_all[(j*32)+31 : j*32] = multibus_in[j];

    //debug
    assign multibus_in[j] = multibus_out[j];
    end
endgenerate






endmodule
