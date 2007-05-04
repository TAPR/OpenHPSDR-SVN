// ALC test code

// Fixed point multiplier. Sample is a 16 bit integer. Multiply is a 16 bit fraction [0].[16]
// in the form  0.xxxx  e.g. to multiply Sample by  0.1234  use  1234 for multiply.
// Result is a 16 bit integer. 

// Shift fractional part left by 16 bits and divide by power of 10 being used to represent fraction.
// Need 16 bits for fraction so we can represent 0.9999  (i.e. 9999).

module ALC (out, sample, multiply);

input [15:0]sample;
input [15:0]multiply;
output wire [15:0]out;
wire [15:0]fraction;
wire [47:0]result;


assign fraction = {multiply, 16'd0} / 10000;  // [16].[16]/10000 = [0].[16]

assign result = {sample,16'd0} * fraction;   // [16].[16] * [0].[16] = [16].[32] = [47:0]

assign out = result[47:32]; // [15:0]


endmodule
