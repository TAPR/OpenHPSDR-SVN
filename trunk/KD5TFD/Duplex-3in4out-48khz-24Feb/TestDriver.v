// Copyright Bill Tracey (bill@ewjt.com) 2006 
//
// Test driver for LeftJustifiedAudioOut module 
//
module DigiAudioOutTest(mclk_i, bclk_o, lrclk_o, databit_o, left_sample, right_sample); 

input mclk_i;       // 24.576 MHz master osc 
output bclk_o;      // bit clock fed to TLV... 
output lrclk_o;     // left right clock fed to TLV...  
output databit_o;   // data out fed to TLV... 

// following 2 sigs are on the parameter list to force the simulator to preserve the signal so they 
// can be seen in the simulation output 
// output [15:0] sample_reg; 
output [15:0] left_sample; 
output [15:0] right_sample; 

wire databit_o; 

// wire [15:0] sample_reg;   // sample input to the audio out module 
reg [15:0] left_sample;   // holding reg for left sample 
reg [15:0] right_sample;  // holding reg for right sample 
reg [5:0] bcount; 


// instantiate clock generator and output modules 
ClockGen48k(mclk_i, bclk_o, lrclk_o);                   // generate bclk and lrclk from master audio clock 
LeftJustifiedAudioOut(lrclk_o, bclk_o, left_sample, right_sample, databit_o); 

// assign sample_reg = ( lrclk_o )  ? left_sample : right_sample;  // select left or right sample based on lrclck

reg last_lr; 
reg reg_lr;
// generate test samples for simulation - count up on the left and down on the right 
always @ (posedge bclk_o ) begin 
	reg_lr = lrclk_o; 
	if ( last_lr == 0 && reg_lr) begin //  leading edge of lrclk 
		bcount <= 0; 
	end 
	else begin
		bcount <= bcount + 6'd1; 
	end 
	if ( bcount == 62 ) begin 
		left_sample[14:0] <= left_sample[14:0] + 14'd1; 
		right_sample[14:0] <= right_sample[14:0] - 14'd1; 
		left_sample[15] <= 1; 
		right_sample[15] <= 1; 
	end 
	last_lr <= reg_lr; 
end 
endmodule 
