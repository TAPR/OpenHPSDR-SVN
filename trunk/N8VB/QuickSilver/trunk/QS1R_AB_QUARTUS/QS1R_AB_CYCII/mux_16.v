// ---------------------------------------------
//
// QS1R Receiver FPGA
// Copyright 2006 P.Covington N8VB
//
// Ver 1.0
//
//
//mux16 #(32) mux16_inst(
//.clk(),
//.din_0(),
//.din_1(),
//.din_2(),
//.din_3(),
//.din_4(),
//.din_5(),
//.din_6(),
//.din_7(),
//.din_8(),
//.din_9(),
//.din_10(),
//.din_11(),
//.din_12(),
//.din_13(),
//.din_14(),
//.din_15(),
//.select(),
//.mux_out()
//);
//

module mux16(
clk,
din_0,
din_1,
din_2,
din_3,
din_4,
din_5,
din_6,
din_7,
din_8,
din_9,
din_10,
din_11,
din_12,
din_13,
din_14,
din_15,
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
input [WIDTH-1:0] din_8;
input [WIDTH-1:0] din_9;
input [WIDTH-1:0] din_10;
input [WIDTH-1:0] din_11;
input [WIDTH-1:0] din_12;
input [WIDTH-1:0] din_13;
input [WIDTH-1:0] din_14;
input [WIDTH-1:0] din_15;
input [3:0] select;

output reg [WIDTH-1:0] mux_out;

always @(posedge clk)
begin : MUX
	case (select)
		4'd0: mux_out <= din_0;
		4'd1: mux_out <= din_1;
		4'd2: mux_out <= din_2;
		4'd3: mux_out <= din_3;
		4'd4: mux_out <= din_4;
		4'd5: mux_out <= din_5;
		4'd6: mux_out <= din_6;
		4'd7: mux_out <= din_7;
		4'd8: mux_out <= din_8;
		4'd9: mux_out <= din_9;
		4'd10: mux_out <= din_10;
		4'd11: mux_out <= din_11;
		4'd12: mux_out <= din_12;
		4'd13: mux_out <= din_13;
		4'd14: mux_out <= din_14;
		4'd15: mux_out <= din_15;
	endcase
end
endmodule

