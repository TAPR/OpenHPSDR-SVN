//////////////////////////////////////////////////////////////
//
//		Band Decoder
//
//////////////////////////////////////////////////////////////

	Divides the current frequency in Hz by 10e5. This results in the following values per band
	
	1.8MHz	= 18
	3.5MHz	= 35
	.....
	30MHz	= 300
	55MHz 	= 550
	
	Each band is then decoded and the appropriate LPF selected as follows
	
	160m 	band = 0000
	 80m	band = 0001
	 60m	band = 0010
	 40m	band = 0011
	 30m	band = 0100
	 20m	band = 0101
	 17m	band = 0110
	 15m	band = 0111
	 12m	band = 1000
	 10m	band = 1001
	 6m		band = 1010
	
	NOTE: When used with PowerSDR we need to add the IF frequency.
*/

/*	
reg [3:0]band;
wire [9:0]temp;
reg [31:0]freq_IF;
wire ready2;

always @ (posedge ready2)		// strobe frequecy when ready is set
begin
	freq_IF <= (frequency_HZ + 32'd12000);	// frequecy_HZ is current frequency in Hz e.g. 14,195,000Hz
end	
//	calculate (frequency_HZ + 12000)/100000
	
	divide divide_freq(.quotient(temp),.ready(ready2),.dividend(freq_IF),.divider(32'd100000),.clk(clock));

// Select highest LPF dependant on frequency in use
		
	always @ (posedge PWM_clock)
	begin 
		if 		(temp > 297) band <= 4'b1010;	// > 10m so use 6m LPF
		else if (temp > 249) band <= 4'b1001;	// > 12m so use 10m LPF
		else if (temp > 214) band <= 4'b1000;  	// > 15m so use 12m LPF
		else if (temp > 181) band <= 4'b0111;  	// > 17m so use 15m LPF
		else if (temp > 143) band <= 4'b0110;  	// > 20m so use 17m LPF
		else if (temp > 101) band <= 4'b0101;	// > 30m so use 20m LPF
		else if (temp > 72)  band <= 4'b0100;  	// > 40m so use 30m LPF  
		else if (temp > 54)  band <= 4'b0011;  	// > 60m so use 40m LPF
		else if (temp > 40)  band <= 4'b0010;  	// > 80m so use 60m LPF
		else if (temp > 19)  band <= 4'b0001;  	// > 160m so use 80m LPF  
		else band <= 4'b0000; 					// < 2MHz so use 160m LPF
	end 	

