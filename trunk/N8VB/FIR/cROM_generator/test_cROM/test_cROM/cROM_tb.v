module cROM_tb;

reg clk, reset;
reg [7:0] addr;
wire signed [23:0] data;

initial begin
	$dumpfile("cROM.vcd");
	$dumpvars(0, U0);
	$monitor("addr=%d, data=%d", addr, data);
	clk = 0;
	reset = 0;
	addr = 8'd0;
	#5 reset = 1;
	#15 reset = 0;
	#10 addr = 8'd1;
	#10 addr = 8'd5;
	#10 addr = 8'd255;
	#20 $finish;
end

always begin
	#5 clk = !clk;
end

cROM U0 (
.clk(clk),
.addr(addr),
.data(data),
.reset(reset)
);

endmodule
