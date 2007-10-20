// extend the lenght of the ce_out_i pulse so that A/D code has time to see it 
reg [2:0]pulse;

always @ (negedge clock)
begin
case (pulse)
0:begin 
	if (ce_out_i)begin
		data_ready <= 1'b1;
		pulse <= pulse + 1'b1;
	end	
	else begin
		data_ready <= 1'b0;
		pulse <= 0;
	end
  end 
7:begin
	data_ready <= 1'b0;
	pulse <= 0;
  end
	
default: pulse <= pulse + 1'b1;
endcase
end