
module CounterToFifo(mclk_i, sernum_i, count_i, write_strobe_i, fifo_in_o, fifo_write_strobe_o, fifo_full_i, busy_o); 

input mclk_i;  // master clock 
input [15:0] sernum_i;  // serial number of packet 
input [31:0] count_i; // the count to be sent 
input write_strobe_i; // write into us 
output [7:0] fifo_in_o;  // data out to fifo we're writing to 
output fifo_write_strobe_o; // write strobe to write into fifo we're writing to 
input fifo_full_i; // is the fifo we're righting to full 
output busy_o;  // indicates we're busy 


reg [31:0] local_count; 
reg [15:0] local_sernum; 
reg last_write_strobe; 
reg [3:0] state; 
reg [3:0] post_strobe_state; 
reg [7:0] fifo_in_o; 
reg busy_o; 
reg fifo_write_strobe_o; 
reg [3:0] fifo_write_strobe_count; 


always @(posedge mclk_i) begin

	case ( state ) 	
		0:  begin // idle state -- wait until we are strobed 
			if ( write_strobe_i == 1 && last_write_strobe == 0 ) begin  // write strobe just went postive 
				local_count <= count_i;      // strobe in count 
				if ( !fifo_full_i ) begin
					local_sernum <= sernum_i; 
			    end
				else begin
					local_sernum <= 0;  
				end 
				busy_o <= 1; 
				state <= 1; 				
			end 
			else begin 
				local_count <= local_count; 
				state <= 0; 
				busy_o <= 0; 
			end 
		end 
		
		1: begin  // start copying the word into the fifo 			
			fifo_in_o <= 8'hff; 
			post_strobe_state <= 4;
			state <= 2; 
		end 
		
		2: begin  // fifo strobe on 
			fifo_write_strobe_o <= 1; 
			fifo_write_strobe_count <= 4'b0;
			state <= 3; 
		end 
		
		3: begin  // fifo strobe off 
			fifo_write_strobe_count <= fifo_write_strobe_count - 1; 
			if ( fifo_write_strobe_count == 0 ) begin 
				fifo_write_strobe_o <= 0; 
				state <= post_strobe_state; 			
			end 
			else begin
				fifo_write_strobe_o <= 1; 
				state <= 3; 
			end 				
		end 
		
		4: begin  // 2nd 2nd 0xff 
			fifo_in_o <= 8'hff; 
			post_strobe_state <= 5; 
			state <= 2; 
		end 
			
			
		5:  begin  // send hi byte of sernum 
			fifo_in_o <= local_sernum[15:8]; 
			post_strobe_state <= 6; 
			state <= 2; 		
		end 
		
		6: begin // send lo byte of sernum 
			fifo_in_o <= local_sernum[7:0]; 
			post_strobe_state <= 7;
			state <= 2; 
		end 
		
		7: begin  // send byte[3] of count 
			fifo_in_o <= local_count[31:24]; 
			post_strobe_state <= 8; 
			state <= 2; 
		end 		
		
		8: begin  // send byte[2] of count 
			fifo_in_o <= local_count[23:16]; 
			post_strobe_state <= 9; 
			state <= 2; 
		end 
		
		9: begin  // send byte[2] of count 
			fifo_in_o <= local_count[15:8]; 
			post_strobe_state <= 10; 
			state <= 2; 
		end 
		
		10: begin  // send byte[2] of count 
			fifo_in_o <= local_count[7:0]; 
			post_strobe_state <= 11; 
			state <= 2; 
		end 
		
		11: begin // all done - go back to idle 
			busy_o <= 0; 
			state <= 0; 
			local_count <= 32'hffffffff;
			local_sernum <= 16'hffff; 
		end 
		
		default: 
			state <= 0; 
			
	endcase 
	
	last_write_strobe <= write_strobe_i; 
	
end 

endmodule 