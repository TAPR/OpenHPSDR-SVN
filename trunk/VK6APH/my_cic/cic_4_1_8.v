/* Cascaded Integrator Comb filter   cic_4_1_8  */

module integrator_40(in, out, clk);
   input [39:0]       in;
   output [39:0]      out;
   reg [39:0]         out;            
   input              clk;
   initial
     out <= 0;
   always @(posedge clk)
     out <= out + in;
endmodule 

module comb_40_cken(in, out, cken, clk);
   input [39:0]    in;
   output [39:0]   out;
   reg [39:0]      out;
   input           cken;
   input           clk;
   reg [39:0]      temp;
   always @ (posedge cken)
       begin
          temp <= in;
          out <= in - temp;
       end
endmodule 


// N=4, M=1 , R = 8
// Calculate bit growth from  N * Log2(R)
// eg. decim rate of 4 bit growth  = 16  hence register size is 24 + 16 = 40 

// in and out is 24 bits

module cic_4_1_8(in, out, cken, clk);
   input [23:0]    in;
   output reg [23:0]   out;
   input           cken;
   input           clk;
   wire [39:0]     i1, i2, i3,i4, i5,i6, i7, i8, c1, c2, c3, c4, c5, c6, c7, c8;
   /* Integrators */
   /* Sign extend input */
   integrator_40 int1({{16{in[23]}}, in[23:0]}, i1, clk);
   integrator_40 int2(i1, i2, clk);
   integrator_40 int3(i2, i3, clk);
   integrator_40 int4(i3, i4, clk);
   integrator_40 int5(i4, i5, clk);
   integrator_40 int6(i5, i6, clk);
   integrator_40 int7(i6, i7, clk);
   integrator_40 int8(i6, i8, clk);

   // Combs
   comb_40_cken comb1(i8, c1, cken, clk);
   comb_40_cken comb2(c1, c2, cken, clk);
   comb_40_cken comb3(c2, c3, cken, clk);
   comb_40_cken comb4(c3, c4, cken, clk);
   comb_40_cken comb5(c4, c5, cken, clk);
   comb_40_cken comb6(c5, c6, cken, clk);
   comb_40_cken comb7(c6, c7, cken, clk);
   comb_40_cken comb8(c7, c8, cken, clk);

   always @ (posedge clk)
     begin
        out <= c8[39:16]; 
     end
endmodule 





