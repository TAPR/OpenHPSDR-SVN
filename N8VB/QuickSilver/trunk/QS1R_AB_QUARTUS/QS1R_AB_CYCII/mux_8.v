// ---------------------------------------------
//
// QS1R Receiver FPGA
// Copyright 2006 P.Covington N8VB
//
// Ver 1.0
//
//
//mux8 #(32) mux8_inst(
//.clk(),
//.din_0(),
//.din_1(),
//.din_2(),
//.din_3(),
//.din_4(),
//.din_5(),
//.din_6(),
//.din_7(),
//.select(),
//.mux_out()
//);
//

module mux8(
clk,
din_0,
din_1,
din_2,
din_3,
din_4,
din_5,
din_6,
din_7,
select,
mux_out
);

parameter WIDTH = 32;

input clk;
input [WIDTH-1:0] din_0;
input [WIDTH-1:0] din_1;
input [WIDTH-1:0] din_2;
input [WIDTH-1:0] din_3;
input [WIDTH-1:0] din_4;
input [WIDTH-1:0] din_5;
input [WIDTH-1:0] din_6;
input [WIDTH-1:0] din_7;
input [2:0] select;

output reg [WIDTH-1:0] mux_out;

always @(posedge clk)
begin : MUX
	case (select)
		3'd0: mux_out <= din_0;
		3'd1: mux_out <= din_1;
		3'd2: mux_out <= din_2;
		3'd3: mux_out <= din_3;
		3'd4: mux_out <= din_4;
		3'd5: mux_out <= din_5;
		3'd6: mux_out <= din_6;
		3'd7: mux_out <= din_7;		
	endcase
end
endmodule

