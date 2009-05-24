// Push Button debounce routine
// Found on the Internet, author unknown
// used to debounce a switch or push button.
// Input is pb and outout clean_pb.
// Button must be stable for debounce time before state changes,
// debounce time is dependent on clk and counter_bits

// eg with clock = 24MHz and counter_bits = 18 stable time is 

//  (10^-6/24) * 2^18 = 11mS

//  Phil Harman VK6APH 15th February 2006

`timescale 1 ns/100 ps

module debounce(clean_pb, pb, clk);
	
	output clean_pb;
	input pb, clk;
	
parameter counter_bits = 18;

reg [counter_bits:0] count;
reg [3:0] pb_history;
reg clean_pb;

always @ (posedge clk)
begin
	pb_history <= {pb_history[2:0], pb};
	
	if (pb_history[3] != pb_history[2])
		count <= 1'b0;
	else if(count[counter_bits])
		clean_pb <= pb_history[3];
	else
		count <= count + 1'b1;
end 
endmodule