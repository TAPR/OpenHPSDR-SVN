//deglitch module
//input has to be stable for 4 consecutive clock samples before a transition is considered valid

`include "timescale.v"
module  deglitch(rst_n,clock, in, out);

input rst_n;
input clock;
input in;
output out;
wire clock;
wire in;
reg out;
reg in_n1, in_n2, in_n3;

//
// de-glitch input using clock
// input has to be stable for 4 consecutive CLK samples before a transition is considered valid
//

always @ (posedge clock or negedge rst_n)
  if(~rst_n)
     out <= 1'b1;
  else
  if (in & in_n1 & in_n2 & in_n3) 
    out <= 1; 			 // all have to be 1 before valid 
  else 
  if (~in & ~in_n1 & ~in_n2 & ~in_n3) 
    out <=0;	 // all have to be 0 before valid


always @ (posedge clock or negedge rst_n)
  if(~rst_n)
   begin
	in_n1 <= 1'b1;				// shift previous samples along 
	in_n2 <= 1'b1;
	in_n3 <= 1'b1;	
   end
  else
   begin
	in_n1 <= in;				// shift previous samples along 
	in_n2 <= in_n1;
	in_n3 <= in_n2;	
   end

endmodule
