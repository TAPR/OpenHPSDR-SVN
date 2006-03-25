//
// Count a clock against a reference Count 
// 
// Copyright Bill Tracey 2006  (bill@ewjt.com) 
// 
//
module RefCount(countee_i,       // signal to be counted 
                refclk_i,        // refclk to be used as the counter gate 
                count_o,         // the count output 
                count_avail_o,   // indicates a new count is avail when state changes 
				err              // indicates count looks fishy 
);

input countee_i;       // clock to be counted -- everything is synced to this clock 
input refclk_i;        // reference clock to be used for the counting 
output [31:0] count_o; // the count 
output count_avail_o;  // tells when a new count is available by changing level 
output err;            // used for debugging - indicates count_o does not appear correct 

reg [31:0] count_o;    // output count 
reg [31:0] count_reg;  // free running count register
reg [31:0] delta;      // used for debugging -- delta between current count and last count 

reg count_avail_o;     // output count is availble 
reg last_refclk;       // last state of refclk 
reg state;             
reg err;               // bad count indicator 
reg sync_refclk;       // sync'd version of refclk 

always @(posedge countee_i) begin
	count_reg <= count_reg + 1;  // bump count 
	sync_refclk <= refclk_i;     // sync the refclock 
	case ( state ) 
		0: begin // just counting 
				if ( sync_refclk == 1 && last_refclk == 0 ) begin  // refclk has gone neg to positive -- publush a new count 
					delta <= count_reg - count_o + 1; 
					count_o <= count_reg + 1; 		
					state <= 1; 
				end 
				else begin  // no new ref clk - come back to this state 
					count_o <= count_o; 
					count_avail_o <= count_avail_o; 
					state <= 0; 
				end 
			end 				
			
		1: begin // copied a new count on last clock -- tell the world 
				count_avail_o <= ~count_avail_o; 
				state <= 0; 
				count_o <= count_o; 
				if ( delta < 2300 || delta > 2500 ) begin  // debug -- count looks bad 
					err <= 1; 
				end 
				else begin
					err <= 0; 
				end 		
			end				
					
		default:
			state <= 0; 										
	endcase 
	last_refclk <= sync_refclk; 
end 

endmodule