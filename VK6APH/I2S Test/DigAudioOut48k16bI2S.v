// Copyright Phil Harman VK6APH and Bill Tracey  KD5TFD 2006, 2007 
// 
// Implements I2S digital audio out, 16 bits, two channels, for TLV320AIC23B D/A converter 
// 
// 
// In I2S mode the MSB of the left sample is available on the second rising edge of bclk_i
// following a falling  edge on lrclk_i.  Simiarily the MSB of the right sample will be available on 
// the second rising edge of bclk_i following the rising edge of lrclk_i.

// NOTE: the data to be sent is latched on the positive edge of lrclk

// Notation used: 
// _i indicates an input signal to a module 
// _o indicates an output signal from a module 
// _io indicates an inout signal to/from a module 
// no suffix indicates a signal local to a module

module I2SAudioOut(lrclk_i, bclk_i, left_sample_i, right_sample_i, outbit_o); 

input lrclk_i;             // left/right clock 
input bclk_i;              // bit clock 
// left and right samples are clocked into the module @ rising edge or lrclk 
input [15:0] left_sample_i;     // data to be clocked out 
input [15:0] right_sample_i;    // right input to be clocked out 
output outbit_o;           // output bit 

reg outbit_o;       // current bit being clocked out 
reg [15:0] data; // data to be  clocked out 
reg [15:0] local_right_sample; // local copy of right sample 
reg [3:0] bit_count;     // how many bits clocked, must preset to MSB 
reg [2:0]TLV_state;

always @(posedge bclk_i)
begin
case(TLV_state)
0:	begin
	if (!lrclk_i)TLV_state <= 0;					// loop until lrclk_i is high
	else begin
		TLV_state <= 1;
		data <= left_sample_i;					// get local copy of data since
		local_right_sample <=  right_sample_i;		// it will change part way through otherwise
		end
	end
1:	if (lrclk_i)TLV_state <= 1;						// loop until lrclk_i is low
	else begin
	bit_count <= 15;
	TLV_state <= 2;
	end
2:	begin
		if (bit_count == 0)	TLV_state <= 3; 
		else begin
		bit_count <= bit_count - 1'b1;
		TLV_state <= 2;  
		end
	end
3:	if (!lrclk_i)TLV_state <= 3; 					// loop until lrclk_i is high
	else begin
	bit_count <= 15;
	data <= local_right_sample;
	TLV_state <= 4;
	end
4:	begin
		if (bit_count == 0)	TLV_state <= 0; 		// done so start again
		else begin
		bit_count <= bit_count - 1'b1;
		TLV_state <= 4;
		end					
	end 
default: TLV_state <= 0;
endcase
end

// the I2S data must be available on the 2nd postive edge of bclk after the clrclk transition

always @ (negedge bclk_i)
begin
	outbit_o <= data[bit_count];
end 


endmodule 