// test programs for new I2S Rx and Tx

// note that the counter has to be equal to the first bit to send e.g. for 16 bits it will be 15

module I2Stest(CBCLK, CLRCLK,DIN, data_left, data_right);



input CLRCLK;
input CBCLK;
output reg DIN;
output reg [15:0] data_left;
output reg [15:0] data_right;

// NOTE: the data to be sent is latched on the positive edge of lrclk

// use new module for Tx

I2SAudioOut I2SAudioOut(.lrclk_i(CLRCLK), .bclk_i(CBCLK), .left_sample_i(16'h8F50), .right_sample_i(16'h8F50), .outbit_o(DIN));


/*

//Alternative Tx using CBCLK

reg [3:0] CCstate;
reg [4:0] counter;
wire [15:0]data_in = 16'h8F50;

always @ (posedge CBCLK)
begin
case (CCstate)

0:	if (CLRCLK)	CCstate <= 1;	// if already low loop until it goes high
	else CCstate <= 1'b0;

1:	if (CLRCLK)	CCstate <= 1;		// wait until it goes low - this is first CBCLK after negedge of CLRCLK
	else begin
	counter <= 15; 					// need to have data available for Rx on next CBCLK
	CCstate <= 2;
	end

2:	begin
	if (counter == 0)	CCstate <= 3;	// have all the left data now get the right
	else begin
	counter <= counter - 1'b1;
	CCstate <= 2;
	end
	end

3:	if (CLRCLK == 0) CCstate <=3; // wait for CLRCLK to go high for right data
	else begin
	counter <= 15; 
	CCstate <= 4;
	end

4:	begin
	if (counter == 0) CCstate <= 0;
	else begin
	counter <= counter - 1'b1;
	CCstate <= 4;
	end
	end

endcase
end

// I2S data must be available on the 2nd positive edge of CBCLK after the CLRCLK transition
always @ (negedge CBCLK)
begin
	DIN <= data_in[counter];
end
*/

/*
// Rx code 
// NOTE: Data out should be latched on the negative edge of CLRCLK 


reg [2:0] Rx_state;
reg [3:0] Rx_counter; // count 15-0
reg [15:0] temp_data_left;
reg [15:0] temp_data_right;

always @ (posedge CBCLK)
begin
case (Rx_state)

0:	if (CLRCLK) Rx_state <= 1; // if already low loop until it goes high
	else Rx_state <= 0;

1:	if (CLRCLK) Rx_state <= 1;		// wait until it goes low - this is first CBCLK after negedge of CLRCLK
	else begin
	Rx_counter <= 15;   			// set counter to start of data 
	Rx_state <= 2;
	end

2:	begin
	temp_data_left[Rx_counter] <= DIN;
	if (Rx_counter == 0)Rx_state <= 3; // can set state <= 0 for MIC input and restart 
	else begin
	Rx_counter <= Rx_counter - 1'b1;
	Rx_state <= 2;
	end
	end

3:	if (CLRCLK == 0) Rx_state <= 3; // wait for CLRCLK to go high for right data
	else begin
	Rx_counter <= 15;	
	Rx_state <= 4;
	end

4:	begin
	temp_data_right[Rx_counter] <= DIN;
	if (Rx_counter == 0) Rx_state <= 0;
	else begin
	Rx_counter <= Rx_counter - 1'b1;
	Rx_state <= 4;
	end
	end

endcase
end	

// latch data when counter = 0 

always @ (negedge CLRCLK)
begin
//	if (Rx_counter == 0)begin
	data_left <= temp_data_left;
	data_right <= temp_data_right;
//	end
end

*/

// Alternative code for Rx

reg [5:0]TX_state;
reg [15:0] Tx_q;

always @ (posedge CBCLK)
begin
    Tx_q[15:0] <= {Tx_q[14:0], DIN};            // shift current TLV320 data left and add next bit
case (TX_state)
0:   begin
     if(!CLRCLK) TX_state <= 5'd0;               // loop until CLRCLK is high
     else TX_state <= TX_state + 1'b1;
     end
1:   begin
     if(CLRCLK) TX_state <= 5'd1;         		// loop until CLRCLK is low
     else TX_state <= TX_state + 1'b1;
     end
// next state is number of bits  + previous state + 1 i.e. 16 + 1 + 1 = 18
18:  begin
     data_left <= Tx_q;                         // TLV320 (microphone or line in)data
     TX_state <= TX_state + 1'b1;         		// state = 0 if stop here     
     end
19:	 begin
	 if(!CLRCLK) TX_state <= 19;				// wait for CLRCLK to go high 
	 else TX_state <= TX_state + 1'b1;
	 end
// next state is number of bits + previous state + 1 i.e. 16 + 1 + 19 = 36				
36:  begin		
	 data_right <= Tx_q;
	 TX_state <= 5'd0;         			    	// done so loop again
     end

default:TX_state <= TX_state + 1'b1;
endcase
end





endmodule

















