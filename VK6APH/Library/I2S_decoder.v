////////////////////////////////////////////////////////////////
//
//  I2S Receiver for I and Q data on Atlas Bus
//
////////////////////////////////////////////////////////////////

// receive I and Q data from Atlas bus in I2S format
// I and Q data is on CDIN on pin C12 of Atlas Bus

reg [3:0] bit_count;     // how many bits clocked 
reg [2:0]IQ_state;
reg [15:0] I_data;
reg [15:0] Q_data;

always @(posedge CBCLK)
begin

case(IQ_state)
0:	begin
	if (!CLRCLK)IQ_state <= 0;					// loop until CLRLCK is high
	else IQ_state <= 1;
	end
1:	begin
	if (CLRCLK)IQ_state <= 1;					// loop until CLRCLK is low
	else begin
		bit_count <= 15;
		IQ_state <= 2;
		end
	end
2:	begin
	I_data[bit_count] <= CDIN;					// get 16 bits of I data 
		if (bit_count == 0)	IQ_state <= 3; 
		else begin
		bit_count <= bit_count - 1'b1;
		IQ_state <= 2;
		end   
	end
3:	begin
	if (!CLRCLK)IQ_state <= 3; 					// loop until CLRLCK is high
	else begin
		bit_count <= 15;
		IQ_state <= 4;
		end
	end
4:	begin
	Q_data[bit_count] <= CDIN;					// get 16 bits of Q data
		if (bit_count == 0)	IQ_state <= 0; 		// done so start again
		else begin
		bit_count <= bit_count - 1'b1;
		IQ_state <= 4;
		end					
	end 
	default: IQ_state <= 0;
endcase
end

// sync I and Q data to 48kHz LR clock

reg [15:0] I_sync_data;
reg [15:0] Q_sync_data;

always @ (negedge CLRCLK)  // use negative edge so that both I and Q are from same frame.
begin 
	I_sync_data <= I_data; 
	Q_sync_data <= Q_data;
end 