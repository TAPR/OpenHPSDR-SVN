// ---------------------------------------------
//
// QS1R Receiver FPGA
// Copyright 2007 P.Covington N8VB
//
// Ver 1.0
//
// See Xilinx appnote: "Digitally Removing A DC Offset: Part2"
//
// dc_offset_correct dc_offset_correct_inst(
// .clk(),
// .clken(),
// .data_in(),
// .data_out(),
// .dc_level_out);


module dc_offset_correct(
input clk,
input clken,
input signed [15:0] data_in,
output signed [15:0] data_out,
output signed [15:0] dc_level_out
);

wire signed [15:0] dc_level = accumulator[31:16];
wire signed [15:0] corrected = data_in[15:0] - dc_level[15:0];
wire signed [15:0] sign_extended = (corrected[15] == 1'b1) ? 16'hFFFF: 16'h0000;

reg signed [31:0] accumulator;

always @(posedge clk) begin
	if (clken == 1'b0) begin
		accumulator <= 32'd0;
		end
	else begin
		accumulator <= accumulator + {sign_extended[15:0], corrected[15:0]};
		end
end

assign data_out[15:0] = corrected[15:0];
assign dc_level_out = dc_level;

endmodule
