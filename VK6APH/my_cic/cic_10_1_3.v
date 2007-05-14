/* Cascaded Integrator Comb filter   cic_10_1_3.v  */

module integrator_34(in, out, clk);
   input [33:0]       in;
   output [33:0]      out;
   reg [33:0]         out;            
   input              clk;
   initial
     out <= 0;
   always @(posedge clk)
     out <= out + in;
endmodule 

module comb_34_cken(in, out, cken, clk);
   input [33:0]    in;
   output [33:0]   out;
   reg [33:0]      out;
   input           cken;
   input           clk;
   reg [33:0]      temp;
   always @ (posedge cken)
       begin
          temp <= in;
          out <= in - temp;
       end
endmodule 


// N=3, M=1 , R = 10
// Calculate bit growth from  N * Log2(R)
// eg. decim rate of 10 bit growth  = 10  hence register size is 24 + 10 = 34 

module cic_10_1_3(in, out, cken, clk);
   input [17:0]    in;
   output reg [23:0]   out;
   input           cken;
   input           clk         ;
   wire [33:0]     i1, i2, i3, c1, c2, c3;
   /* Integrators */
   /* Sign extend input */
   integrator_34 int1({{16{in[17]}}, in[17:0]}, i1, clk);
   integrator_34 int2(i1, i2, clk);
   integrator_34 int3(i2, i3, clk);

   // Combs
   comb_34_cken comb1(i3, c1, cken, clk);
   comb_34_cken comb2(c1, c2, cken, clk);
   comb_34_cken comb3(c2, c3, cken, clk);
   always @ (posedge clk)
     begin
        out <= c3[33:10]; 
     end
endmodule 





