/* Cascaded Integrator Comb filter   cic_320_1_8  */


module my_cic(in, out, cken, cken2, cken3, clk);

input in, cken, cken2, cken3, clk;
output out;

wire [17:0] in;
wire [23:0] out;

wire [23:0]out1, out2;

cic_10_1_3 cic_1(.in(in), .out(out1), .cken(cken), .clk(clk));
cic_8_1_5  cic_2(.in(out1), .out(out2), .cken(cken2), .clk(cken));
cic_4_1_8  cic_3(.in(out2),.out(out), .cken(cken3),.clk(cken2));

endmodule








