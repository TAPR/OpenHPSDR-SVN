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



module fir(
  input clock,
  input start,
  input signed [23:0] coeff,
  input signed [23:0] in_data,
  output reg signed [31:0] out_data
  );


reg [2:0] state;
reg shr_clk_enable;
reg new_sample;
reg [7:0] sample_no;


initial
  begin
  new_sample = 0;
  shr_clk_enable <= 0; 
  state = 0;
  end


always @(posedge clock)
    case (state)
      0: 
        if (start) 
          //write new sample to shiftreg, clear mac
          begin 
          new_sample <= 1;
          shr_clk_enable <= 1;
          state <= 1;
          end

      1: //switch shiftreg to shift mode; enable mac
        begin         
        new_sample <= 0;
        sample_no <= 0;
        state <= 2;
        end

      2: //do mac for 256 samples
        begin
        sample_no <= sample_no + 8'd1;

        //done, stop shifting shiftreg
        if (sample_no == 8'd255)
          begin
          shr_clk_enable <= 0;
          state <= 3;
          end
        end 

      3, 4, 5: //wait for mac pipeline to finish
        state <= state + 1'b1;

      6: //round and register mac output
        begin       
        out_data <= mac_out[55:24] + mac_out[23];
        state <= 0;
        end

    endcase





//------------------------------------------------------------------------------
//                    circular shift register 256 x 24 bit
//------------------------------------------------------------------------------
wire [23:0] shr_in = new_sample ? in_data : shr_out;


fir_shiftreg fir_shiftreg_inst(
  .clock(clock),
  .clken(shr_clk_enable),
  .shiftin(shr_in),
  .shiftout(shr_out)
  );


wire [23:0] shr_out;






//------------------------------------------------------------------------------
//                        multiplier / accumulator
//------------------------------------------------------------------------------
fir_mac fir_mac_inst(
  .clock(clock),
  .clear(new_sample),
  .in_data_1(shr_out),
  .in_data_2(coeff),
  .out_data(mac_out)
  );


wire signed [55:0] mac_out;




endmodule
