module cbuffer_tb;

reg clock, reset, wren, rden;
reg [47:0] din;
wire [47:0] dout;
integer i;

initial begin
	$dumpfile("cbuff.vcd");
	$dumpvars(0, U0);
	$monitor ("clk=%b,wren=%b,rden=%b,din=%h,dout=%h", clock,wren,rden,din,dout);
	clock = 0;
	reset = 0;
	wren = 0;
	rden = 0;
	#2 reset = 1;
	#2 reset = 0;
	for (i=1; i < 11; i=i+1) begin
		#2 wren = 1'b1; din = i; 		
		#2 wren = 0; rden = 1;
		#2 rden = 1;
		#2 rden = 1;
		#2 rden = 1;		
		#2 rden = 0;
	end					
	#2 $finish;
end

always begin
	#1 clock = !clock;
end

cbuffer_top #(48,2) U0 (
.clk(clock),
.reset(reset),
.data_in(din),
.wren(wren),
.rden(rden),
.data_out(dout)
);

endmodule
