// Top Level Module for RefCounter 
// Copyright Bill Tracey 2006 (bill@ewjt.com) 
//
// This is the top level module for a reference counter for the Xylo board.  
//   
// 
module RefCounter(
        FX2_CLK, AD_CLK, FX2_FD, FX2_SLRD, FX2_SLWR, FX2_flags, FX2_PA, BCLK, DOUT, LRCLK, LED,
        alt_clk_i, alt_clk_o, one_pps_i, tenk_pps_i, dbgout
);


input AD_CLK;                           // From A/D converter 24.576MHz
input FX2_CLK;                          // FX2 clock - 24MHz
input BCLK, DOUT, LRCLK;                // lines used for wolfson -- not used in this project 
//inout  [7:0] FX2_FD;          // bidirectional FIFO data to/from the FX2
output  [7:0] FX2_FD;           // bidirectional FIFO data to/from the FX2 *******************************8
                                                                       // ***** use this so smulation works
input [2:0] FX2_flags;          // Flags input from the FX2
output FX2_SLWR;                // FX2's write line
output FX2_SLRD;                // FX2's FIFO read line
inout [7:0] FX2_PA;             // bidirectional FX2's Port A data
output [1:0] LED;               // Red and Green LEDs on Xylo board

input alt_clk_i;				// input for the osc mounted on socket on Xylo  -- tied to pin ?? 
output alt_clk_o;               // passthru of alt osc -- used in Wolfson setup 
input one_pps_i;                // input for 1 pps reference clock 
input tenk_pps_i;               // input for 10khs ref clock 

output dbgout;   // wjt added -- tied to 89
reg dbgout;


reg [15:0] dbgcnt; 
always @ (posedge one_pps_i) begin
	dbgcnt <= dbgcnt + 1; 
end


reg [15:0] dbgcnt2; 

always @(posedge FX2_SLWR) begin
	dbgcnt2 <= dbgcnt2 + 1;
end 

// LED[1] -- green 
// LED[0] -- red 
// assign LED[0] = dbgcnt[2]; 
assign LED[0] = FX2_flags[2]; 
// assign LED[0] = dbgcnt2[12]; 
// assign LED[1] = dbgcnt[12]; 
// assign LED[0] = busy_o; 
// assign LED[1] = sync_fifo_empty; 
assign LED[1] = err; 
// reg FX2_SLWR; 


assign alt_clk_o = alt_clk_i; // pass thru alt osc clock


// module RefCount(countee_i,       // signal to be counted 
//                refclk_i,        // refclk to be used as the counter gate 
//                count_o,         // the count output 
//                count_avail_o);



// module synchro_fifo(reset_i,        // reset the fifo -- active low! 
//                    data_i,         // data input
//                    write_strobe_i, // write strobe input
//                    data_o,         // data output
//                    read_strobe_i,  // read strobe input
//                    full_o,         // is fifo full
//                    empty_o         // if fifo empty
//                   );



// module fx2_usb_pipe(clk_i,   // fx2 clk 
//                      fd_io,   // bidi fifo data to/from fx2 
//                      slrd_o,   // fx2 fifo write line 
//                     slwr_o,   // 
//                      flags_i, 
//                     pa_i,
//                      data_i,  // data from module user 
//                     data_read_strobe_o, // line to strobe to read data in 
//                     data_avail_i // flag indicating data is available 
// );


//module CounterToFifo(mclk_i, sernum_i, count_i, write_strobe_i, fifo_in_o, fifo_write_strobe_o, busy_o); 



// intstantiate a RefCount to count the FX2 clk against the 1 pps signal 
wire [31:0] fx2_1_pps_count;
wire fx2_1_pps_count_status; 
wire err; 
RefCount CountFX2via1PPS(FX2_CLK, one_pps_i, fx2_1_pps_count, fx2_1_pps_count_status, err);


// instantiate a fifo to sit in front of the FX2 fifo  - the RefCount module write's to this fifo, the usb_pipe module reads from it 
wire [7:0] sync_fifo_in; 
wire sync_fifo_write_strobe;
dc_fifo SyncFifoToFX2(sync_fifo_in, FX2_CLK, sync_fifo_read_strobe, FX2_CLK, sync_fifo_write_strobe, sync_fifo_out, sync_fifo_empty, sync_fifo_full);

wire [7:0] sync_fifo_out; 
wire sync_fifo_read_strobe; 
wire sync_fifo_data_avail; 
wire sync_fifo_full; 
wire sync_fifo_empty; 

assign sync_fifo_data_avail = ~sync_fifo_empty; 

// and instantiate the fx2 usb pipe, it will be fed from SyncFifoToFX2
fx2_usb_pipe usb_pipe(FX2_CLK, FX2_FD, FX2_SLRD, FX2_SLWR, FX2_flags, FX2_PA, 
                      sync_fifo_out, sync_fifo_read_strobe, sync_fifo_data_avail); 


// instantiate a counter_to_fifo  -- takes data from the RefCount and frames it and puts it into the FIFO on it's way to the  FX2 USB fifo 
wire busy_o;
reg counter_to_fifo_strobe; 
reg [15:0] sernum; 
reg [31:0] fx2_1_pps_count_local; 
reg last_fx2_1_pps_count_status; 

CounterToFifo(FX2_CLK, sernum, fx2_1_pps_count_local, counter_to_fifo_strobe, sync_fifo_in, sync_fifo_write_strobe, busy_o);   
reg [3:0] state; 
always @(posedge FX2_CLK) begin 

	dbgout <= sync_fifo_read_strobe; 

	case ( state ) 	
		0: begin  // waiting for status of count to change 
			if ( fx2_1_pps_count_status != last_fx2_1_pps_count_status ) begin // staus has changed -- copy count and get it on it's way 
				fx2_1_pps_count_local <= fx2_1_pps_count; 
				sernum <= sernum + 1; 
				state <= 1; 			
			end 
			else begin 
				state <= 0; 
			end 
		end 
		
		1: begin  // strobe into the counterToFifo block 
			counter_to_fifo_strobe <= 1; 
			state <= 2; 			
		end 
		
		2: begin  // clear strobe, back to idle 
			counter_to_fifo_strobe <= 0; 
			state <= 0; 
		end 
				
		
		default: state <= 0; 
		
	endcase 
	
	last_fx2_1_pps_count_status <= fx2_1_pps_count_status; 
end 

endmodule