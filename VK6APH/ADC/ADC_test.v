// test program for  ADC79H90 ADC

// Penelope uses inputs AIN1 to AIN5
// SCLK can run at 8MHz max and is = clock/4 
// Note: AINx data can be latched using nCS

module ADC_test(clock, SCLK, nCS, MISO, MOSI, AIN5);

output  nCS;
input  clock;
input  MISO;
output MOSI;
output SCLK;
output [11:0]AIN5;

wire [15:0] data;
reg  [2:0]ADC;
reg  [3:0]bit_cnt;
reg  nCS;
reg  SCLK;
reg  MOSI;
reg  [11:0] AIN5; 		// reg holds VFWD volts
reg [11:0] temp_AIN5;	// 

assign data = 16'b0010_0000_0000_0000; // address of AIN5

always @ (posedge clock)
begin
case (ADC)

0:	begin
	nCS <= 1'b1; 			// set nCS high
	bit_cnt <= 4'd15; 		// set bit counter
	ADC <= 1;
	end
	
1:  begin
	nCS <= 0; 				// select ADC
	MOSI <= data[bit_cnt];	// shift data out to MOSI
	ADC <= 2;
	end
	
2:  begin
	SCLK <= 1;				// SCLK high
	ADC <= 3;
	end
	
3:  begin
	SCLK <= 0;				// SCLK low
	ADC <= 4;
	end

4:  begin
	if (bit_cnt == 0)begin 	// restart ? - will change address here later on
		ADC <= 0;
	end
	else begin
		bit_cnt <= bit_cnt - 1'b1; // do all again
		ADC <= 1;
	end
	end 
	
default: ADC <= 0;
endcase
end 

// SPI receiver - doesn't get much simpler than this!

always @ (negedge SCLK)
begin
if (bit_cnt > 11)				// start capturing data at bit counter = 11
	AIN5 = temp_AIN5;			// latch data when not shifting
else 
	temp_AIN5[bit_cnt] <= MISO; // capture incoming data
end 

	
	
endmodule 

