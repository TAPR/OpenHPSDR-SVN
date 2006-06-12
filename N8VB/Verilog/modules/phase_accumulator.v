// HPSDR Phase Accumulator
// P. Covington N8VB
// Phase Accumulator
//
// Inputs:
//	clk - clock 
// reset - reset bit, resets the phase accumulator to 0;
// frequency - tuning word/dword, size based on RESOLUTION parameter, default = 32
//
// Outputs:
// phase_out - phase accumulator current value, size based on RESOLUTION parameter, default = 32
//
module phase_accumulator(clk,reset,frequency,phase_out);
	parameter RESOLUTION = 32;
	
	input	clk;
	input	reset;
	input	[RESOLUTION-1:0] frequency;
	
	output reg [RESOLUTION-1:0] phase_out;
	
	always @(posedge clk)
		if(reset)
			phase_out <= #1 32'b0; // reset the phase accumulator to 0
		else
			phase_out <= #1 phase_out + frequency; // add frequency increment to phase accumulator
			
endmodule
	