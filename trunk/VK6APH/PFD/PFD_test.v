// test code for Phase Frequency Detector


module PFD_test(ref_in,	osc_in, pfd_out, LED);

input ref_in;
input osc_in;
output pfd_out;
output [7:0]LED;

wire pdf_out;
reg ref_1k;
reg osc_1k;
reg [14:0]ref_count;
reg [14:0]osc_count;



// divide 48MHz reference clock down to 1kHz

always @ (posedge ref_in)
begin
	if (ref_count == 15'd23999) 
		begin 
			ref_1k <= ~ref_1k;
			ref_count <= 15'd0;
		end
	else ref_count <= ref_count + 1'b1;
end 

// divide 5MHz test oscillator clock down to 1kHz

always @ (posedge osc_in)
begin
	if (osc_count == 15'd2499) 
		begin 
			osc_1k <= ~osc_1k;
			osc_count <= 15'd0;
		end
	else osc_count <= osc_count + 1'b1;
end 

// apply to PFD

 pfd(.ref_in(ref_1k),.osc_in(osc_1k),.pfd_out(pfd_out));


assign LED[0] = ref_in;
assign LED[1] = osc_in;
assign LED[2] = ref_1k;
assign LED[3] = osc_1k;
assign LED[4] = pfd_out;
assign LED[5] = 1'b1;
assign LED[6] = 1'b1;
assign LED[7] = 1'b1;


endmodule