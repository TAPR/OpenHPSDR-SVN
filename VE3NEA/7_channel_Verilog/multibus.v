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



module multibus(
  //FX2 side
  input FX2_multi_clk,  //FX2 port A[3]
  input FX2_multi_stb,  //FX2 port A[1]
  input FX2_multi_wr,   //FX2 port A[0]
  inout [7:0] FX2_PE,   //FX2 port E

  //FPGA side
  input [REG_COUNT*32-1:0] multibus_in_all,
  output [REG_COUNT*32-1:0] multibus_out_all
  );


parameter REG_COUNT = 16;


reg fx2pe_oe;
reg [3:0] state;
reg [7:0] addr;
reg [31:0] inreg;
reg [31:0] outreg;


initial fx2pe_oe = 0;
initial state = 0;


//vector to array mapping
wire [31:0] in_dwords [0:REG_COUNT-1];
reg [31:0] out_dwords [0:REG_COUNT-1];
genvar i;
generate
  for (i=0; i<REG_COUNT; i=i+1)
    begin : dword_mapping
    assign multibus_out_all[(i*32)+31 : i*32] = out_dwords[i];
    assign in_dwords[i] = multibus_in_all[(i*32)+31 : i*32];
    end
endgenerate


assign FX2_PE = fx2pe_oe ? inreg[7:0] : 8'bzzzzzzzz;


always @(posedge FX2_multi_clk)
  case ({FX2_multi_stb, state})
    'h10: //save address
      begin
      addr <= FX2_PE;
      fx2pe_oe <= ~FX2_multi_wr;
      state <= 1;
      end

    'h11: //read/write 1-st byte
      begin
      outreg[31:24] <= FX2_PE;
      //register input data 
      inreg <= in_dwords[addr];
      state <= 2;
      end

    'h12, 'h13, 'h14: //read/write 3 more bytes
      begin
      outreg <= {FX2_PE, outreg[31:8]};
      inreg <= inreg >> 8;
      state <= state + 4'b1;
      end

    'h05: //done
      begin
      //register output data
      if (FX2_multi_wr) out_dwords[addr] <= outreg;
      fx2pe_oe <= 0;
      state <= 0;
      end

    //in case of any problems return to state 0
    default
      begin
      fx2pe_oe <= 0;
      state <= 0;
      end
  endcase


endmodule