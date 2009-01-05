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



module fir_coeffs(
  input clock,
  output reg fir_start,
  output reg fir_done,
  output signed [23:0] fir_coeff
  );


//cic decimation factor minus 1
localparam HI_DECIMATOR = 2*650 - 1;


reg [3:0] state;
reg [7:0] coeff_no;
reg [11:0] decimator;
reg even_sample;


initial
  begin
  fir_start = 0;
  fir_done = 0;
  even_sample = 0;
  state = 0;
  end


fir_coeffs_rom fir_coeffs_rom_inst(
  .clock(clock),
  .address(coeff_no),
  .q(fir_coeff)
  );


always @(posedge clock)
    case (state)
      0:
        begin //set address counter
        fir_start <= 1;
        if (even_sample) fir_done <= 1;        
        coeff_no <= 0;
        state <= 1;
        end

      1:
        begin //register address in rom
        fir_start <= 0;
        fir_done <= 0;        
        coeff_no <= 1;
        state <= 2;
        end

      2: //register 1-st coefficient at rom output
        begin        
        coeff_no <= 2;
        decimator <= 3;  
        state <= 3;
        end

      3:
        begin //register next coefficient
        coeff_no <= coeff_no + 8'd1;
        decimator <= decimator + 11'd1;
        if (decimator == HI_DECIMATOR) 
          begin
          even_sample <= ~even_sample;
          state <= 0;
          end
        end

  endcase   



endmodule
