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



module dac(
  input reset,
  input clock,

  //dac side
  output reg DAC_PD,
  output DAC_SCKI,
  output DAC_BCK,
  output DAC_LRCK,
  output DAC_DATA,

  //fifo side
  input data_ready,
  output reg data_rdreq,
  input [23:0] data_L,
  input [23:0] data_R
  );


assign DAC_SCKI = clock;
assign DAC_BCK = state[0];
assign DAC_LRCK = state[6];
assign DAC_DATA = shift_reg[55];


reg [6:0] state;
reg sample_available;
reg [55:0] shift_reg; //2x24 plus 8 zeros in between


initial 
  begin
  state = 7'b0;
  shift_reg = 56'b0;
  DAC_PD = 0;
  end



always @(posedge clock)
    begin
    state <= state + 7'b1;

    case (state)
      0:
        begin
        if (data_ready) data_rdreq <= 1'b1;
        sample_available <= data_ready;
        end

      1:
        begin
        shift_reg <= sample_available ? {data_L, 8'd0, data_R} : 56'b0;
        data_rdreq <= 1'b0;
        end

      default
        begin
        if (state == 7'b1111111) DAC_PD <= 1;
        if (state[0]) shift_reg <= shift_reg << 1;
        end
      endcase
    end

endmodule
