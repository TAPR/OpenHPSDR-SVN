module cROM_Sequencer_tb;

reg clock, reset;
wire [23:0] data;
integer i;

initial begin
	$dumpfile("crom_seq.vcd");
	$dumpvars(0, U0);
	$monitor ("clk=%b,data=%h",clock,data);
	clock = 0;
	reset = 0;	
	#2 reset = 1;
	#2 reset = 0;
	for (i=0; i < 256; i=i+1) begin
		#2 reset = 0;		
	end					
	#2 $finish;
end

always begin
	#1 clock = !clock;
end

cROM_Sequencer U0 (
.clk(clock),
.reset(reset),
.data(data)
);

endmodule
