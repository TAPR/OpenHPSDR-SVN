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
//           Algorithm by Darrell Harmon modified by Cathy Moss
//            Code Copyright (c) 2008 Alex Shovkoplyas, VE3NEA
//------------------------------------------------------------------------------


module cordic(
  input clock,
  input signed [WF-1:0] frequency,
  input signed [IN_WIDTH-1:0] in_data,
  output signed [WO-1:0] out_data_I,
  output signed [WO-1:0] out_data_Q
  );


parameter IN_WIDTH   = 16;
parameter EXTRA_BITS = 4; //spur reduction 6 dB per bit


//internal params
localparam STG = IN_WIDTH + EXTRA_BITS - 1; //19 number of stages
localparam WR =  IN_WIDTH + EXTRA_BITS + 2; //22 data regs
localparam WZ =  IN_WIDTH + EXTRA_BITS;     //20 angle regs
localparam WO =  WR;                        //22 output width

localparam WF = 32;   //NCO freq,  -Pi..Pi per clock cycle
localparam WP = WZ+1; //NCO phase, 0..2*Pi







//------------------------------------------------------------------------------
//                             arctan table
//------------------------------------------------------------------------------
localparam WT = 32;

wire signed [WT-1:0] atan_table [1:WT-1];


//     atan_table[00] = 32'b01000000000000000000000000000000; //1073741824  FYI
assign atan_table[01] = 32'b00100101110010000000101000111011; //633866811
assign atan_table[02] = 32'b00010011111101100111000010110111; //334917815
assign atan_table[03] = 32'b00001010001000100010001110101000; //170009512
assign atan_table[04] = 32'b00000101000101100001101010000110; //85334662
assign atan_table[05] = 32'b00000010100010111010111111000011; //42708931
assign atan_table[06] = 32'b00000001010001011110110000111101; //21359677
assign atan_table[07] = 32'b00000000101000101111100010101010; //10680490
assign atan_table[08] = 32'b00000000010100010111110010100111; //5340327
assign atan_table[09] = 32'b00000000001010001011111001011101; //2670173
assign atan_table[10] = 32'b00000000000101000101111100110000; //1335088
assign atan_table[11] = 32'b00000000000010100010111110011000; //667544
assign atan_table[12] = 32'b00000000000001010001011111001100; //333772
assign atan_table[13] = 32'b00000000000000101000101111100110; //166886
assign atan_table[14] = 32'b00000000000000010100010111110011; //83443
assign atan_table[15] = 32'b00000000000000001010001011111010; //41722
assign atan_table[16] = 32'b00000000000000000101000101111101; //20861
assign atan_table[17] = 32'b00000000000000000010100010111110; //10430
assign atan_table[18] = 32'b00000000000000000001010001011111; //5215
assign atan_table[19] = 32'b00000000000000000000101000110000; //2608
assign atan_table[20] = 32'b00000000000000000000010100011000; //1304
assign atan_table[21] = 32'b00000000000000000000001010001100; //652
assign atan_table[22] = 32'b00000000000000000000000101000110; //326
assign atan_table[23] = 32'b00000000000000000000000010100011; //163
assign atan_table[24] = 32'b00000000000000000000000001010001; //81
assign atan_table[25] = 32'b00000000000000000000000000101001; //41
assign atan_table[26] = 32'b00000000000000000000000000010100; //20
assign atan_table[27] = 32'b00000000000000000000000000001010; //10
assign atan_table[28] = 32'b00000000000000000000000000000101; //5
assign atan_table[29] = 32'b00000000000000000000000000000011; //3
assign atan_table[30] = 32'b00000000000000000000000000000001; //1
assign atan_table[31] = 32'b00000000000000000000000000000001; //1








//------------------------------------------------------------------------------
//                              registers
//------------------------------------------------------------------------------
//NCO
reg [WP-1:0] phase;


//stage outputs
reg signed [WR-1:0] X [0:STG-1];
reg signed [WR-1:0] Y [0:STG-1];
reg signed [WZ-1:0] Z [0:STG-1];






//------------------------------------------------------------------------------
//                               stage 0
//------------------------------------------------------------------------------
//input sign-extended by 1 bit, zero-padded at the right
wire signed [WR-1:0] in_data_ext = {in_data[IN_WIDTH-1], in_data, {(WR-IN_WIDTH-1){1'b0}}};
wire [1:0] quadrant = phase[WP-1:WP-2];


always @(posedge clock)
  begin
  //rotate to the required quadrant, pre-rotate by +Pi/4. Gain = Sqrt(2)
  case (quadrant)
    0: begin X[0] <=  in_data_ext;   Y[0] <=  in_data_ext; end
    1: begin X[0] <= -in_data_ext;   Y[0] <=  in_data_ext; end
    2: begin X[0] <= -in_data_ext;   Y[0] <= -in_data_ext; end
    3: begin X[0] <=  in_data_ext;   Y[0] <= -in_data_ext; end
  endcase

  //subtract quadrant and Pi/4 from the angle
  Z[0] <= {~phase[WZ-2], ~phase[WZ-2], phase[WZ-3:0]};

  //advance NCO
  phase <= phase + frequency[WF-1:WF-WP];
  end






//------------------------------------------------------------------------------
//                           stages 1 to STG-1
//------------------------------------------------------------------------------
genvar n;


generate
  for (n=0; n<=(STG-2); n=n+1)
    begin : stages
    //data from prev stage, shifted
    wire signed [WR-1:0] X_shr = {{(n+1){X[n][WR-1]}}, X[n][WR-1:n+1]};
    wire signed [WR-1:0] Y_shr = {{(n+1){Y[n][WR-1]}}, Y[n][WR-1:n+1]};


    //rounded arctan
    wire [WZ-2-n:0] atan = atan_table[n+1][WT-2-n:WT-WZ] + atan_table[n+1][WT-WZ-1];


    //the sign of the residual
    wire Z_sign = Z[n][WZ-1-n];


    always @(posedge clock)
      begin
      //add/subtract shifted and rounded data
      X[n+1] <= Z_sign ? X[n] + Y_shr + Y[n][n] : X[n] - Y_shr - Y[n][n];
      Y[n+1] <= Z_sign ? Y[n] - X_shr - X[n][n] : Y[n] + X_shr + X[n][n];

      //update angle
      if (n < STG-2)
        begin : angles
        Z[n+1][WZ-2-n:0] <= Z_sign ? Z[n][WZ-2-n:0] + atan : Z[n][WZ-2-n:0] - atan;
        end
      end
    end
endgenerate






//------------------------------------------------------------------------------
//                                 output
//------------------------------------------------------------------------------
/*
always @(posedge clock)
  begin
  out_data_I <= X[STG-1][WR-1 : WR-WO] + X[STG-1][WR-1-WO];
  out_data_Q <= Y[STG-1][WR-1 : WR-WO] + Y[STG-1][WR-1-WO];
  end
*/

assign out_data_I = X[STG-1];
assign out_data_Q = Y[STG-1];


endmodule
