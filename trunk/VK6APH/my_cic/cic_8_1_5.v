/* Cascaded Integrator Comb filter   cic_8_1_5  */

module integrator_39(in, out, clk);
   input [38:0]       in;
   output [38:0]      out;
   reg [38:0]         out;            
   input              clk;
   initial
     out <= 0;
   always @(posedge clk)
     out <= out + in;
endmodule 

module comb_39_cken(in, out, cken, clk);
   input [38:0]    in;
   output [38:0]   out;
   reg [38:0]      out;
   input           cken;
   input           clk;
   reg [38:0]      temp;
   always @ (posedge cken)
       begin
          temp <= in;
          out <= in - temp;
       end
endmodule 


// N=5, M=1 , R = 8
// Calculate bit growth from  N * Log2(R)
// eg. decim rate of 10 bit growth  = 15  hence register size is 24 + 15 = 39 

// in and out is 24 bits

module cic_8_1_5(in, out, cken, clk);
   input [23:0]    in;
   output reg [23:0]   out;
   input           cken;
   input           clk;
   wire [38:0]     i1, i2, i3,i4, i5, c1, c2, c3, c4, c5;
   /* Integrators */
   /* Sign extend input */
   integrator_39 int1({{15{in[23]}}, in[23:0]}, i1, clk);
   integrator_39 int2(i1, i2, clk);
   integrator_39 int3(i2, i3, clk);
   integrator_39 int4(i3, i4, clk);
   integrator_39 int5(i4, i5, clk);

   // Combs
   comb_39_cken comb1(i5, c1, cken, clk);
   comb_39_cken comb2(c1, c2, cken, clk);
   comb_39_cken comb3(c2, c3, cken, clk);
   comb_39_cken comb4(c3, c4, cken, clk);
   comb_39_cken comb5(c4, c5, cken, clk);
   always @ (posedge clk)
     begin
        out <= c5[38:15]; 
     end
endmodule 





