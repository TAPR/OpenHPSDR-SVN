module FIR_tb;

reg clock, reset, stb_in;
reg signed [23:0] dinI;
reg signed [23:0] dinQ;
wire signed [48:0] doutI;
wire signed [48:0] doutQ;
wire stb_out;

integer i, j;

initial begin
	$dumpfile("Fir_test.vcd");
	$dumpvars(0, U0);
	$monitor ("stb_in=%b,stb_out=%b,dinI=%h,dinQ=%h,doutI=%h", stb_in,stb_out,dinI,dinQ,doutI);
	clock = 0;
	reset = 0;
	stb_in = 0;
	#2 reset = 1;
	#2 reset = 0;
	#2 stb_in = 1'b1; dinI = 1; dinQ = 0;	
	for (j=0; j < 256; j=j+1) begin		
		for (i=0; i < 520; i=i+1) begin				
			#2 stb_in = 0;
		end
		#2 stb_in = 1'b1; dinI = 0; dinQ = 0;	
	end				
	#2 $finish;
end

always begin
	#1 clock = !clock;
end

FIR_top U0 (
.clk(clock),
.reset(reset),
.data_in_I(dinI),
.data_in_Q(dinQ),
.strobe_in(stb_in),
.data_out_I(doutI),
.data_out_Q(doutQ),
.strobe_out(stb_out)
);

endmodule
