// Copyright Bill Tracey (bill@ewjt.com) 2006 
// 
// Implements left justified digital audio out, 16 bits, two channels  for TLV320AIC23B D/A converter 
// 
// 
// In left justified mode the MSB of the left sample is available on the rising edge of blck
// following a rising edge on lrclk.  Simiarily the MSB of the right sample will be available on 
// the rising edge of blck following the falling edge of lrclk 
// 

// Notation used: 
// _i indicates an input signal to a module 
// _o indicates an output signal from a module 
// _io indicates an inout signal to/from a module 
// no suffix indicates a signal local to a module



module LeftJustifiedAudioOut(lrclk_i, bclk_i, left_sample_i, right_sample_i, outbit_o); 

input lrclk_i;             // left/right clock 
input bclk_i;              // bit clock 
// left and right samples are clocked into the module @ rising edge or lrclk 
input [15:0] left_sample_i;     // left input sample to be clocked out 
input [15:0] right_sample_i;    // right input to be clocked out 
output outbit_o;           // output bit 

reg outbit_o;       // current bit being clocked out 
reg [2:0] state; 
reg [14:0] local_left_sample; // local copy of left sample being clocked out 
reg [15:0] local_right_sample; // local copy of right sample 
reg [3:0] bit_count;     // how many bits clocked 

// Main state machine -- pretty simple, wait for lrclk change and then 
//                       register the input sample and clock out the bits 
// states 
// 0 - waiting for LRCLK to go hi, right sample completed  
// 1 - clocking out left sample 
// 2 - clocking out left sample completed, waiting for LRCLK to go lo 
// 3 - clocking out right sample 


// keep track of bitclock count within lrclock 
reg [5:0] bclk_count; 
reg last_lr; 
reg reg_lr; 
always @ (posedge bclk_i) begin 
	reg_lr = lrclk_i;
	if ( reg_lr == 1 && last_lr == 0 ) begin
		bclk_count <= 0; 
	end 
	else begin
		bclk_count <= bclk_count + 6'd1; 
	end 
	last_lr <= reg_lr; 
end 

always @ (negedge bclk_i) begin 
	case ( state ) 	
		2'd0: begin   // waiting for lrclk to go hi 
			if ( bclk_count == 63  ) begin              // LRCLK has gone hi, time to move left sample bits out, and grab right sample
				outbit_o <= left_sample_i[15];        // setup first bit  
				local_left_sample[14:0] <= left_sample_i[14:0];  // grab remaining 14 bits 
				local_right_sample <= right_sample_i;       // and stash away the right sample; 
				state <= 2'd1;                // go to clock out left bits state 
				bit_count <= 4'd14; 		  // setup count 
			end 
			else begin 
				outbit_o <= 0; 
			end 
		end 
		
		2'd1: begin  // shift out a left bit 	
			bit_count <= bit_count - 4'd1;   // decrement count of bits sent 
			outbit_o <= local_left_sample[14];    // output next bit 
			local_left_sample[14:1] <= local_left_sample[13:0];  // shift over remaining bits 
			if ( bit_count == 0 ) begin // are we done
				state <= 2'd2;    // yes - go wait for falling edge of lrclk 
			end			
		end 
		
		2'd2: begin // wait for falling edge of lrclk
			if ( bclk_count == 31 ) begin // lrclk has gone lo -- handle the right sample 
				outbit_o <= local_right_sample[15]; 
				state <=2'd3; 
				bit_count <= 4'd14; 
			end
			else begin 
				outbit_o <= 0; 
			end 
		end 
		
		2'd3: begin // clock out the right sample bits 
			bit_count <= bit_count - 4'd1;   // decrement count of bits sent 
			outbit_o <= local_right_sample[14];    // output next bit 
			local_right_sample[14:1] <= local_right_sample[13:0];  // shift over remaining bits 
			if ( bit_count == 0 ) begin // are we done
				state <= 2'd0;    // yes - go wait for rising edge of lrclk 
			end								
		end 
	
		default: begin 
			state <= 2'd0; 
		end 
	endcase 
end 
endmodule 