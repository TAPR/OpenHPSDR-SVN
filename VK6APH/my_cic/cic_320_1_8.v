/* Cascaded Integrator Comb filter   cic_320_1_8  */


module my_cic(in, out, cken, clk);

input in, cken, clk;
output out;

wire [23:0] in;
wire [23:0] out;

cic_320_1_8(in, out, cken, clk);

endmodule


module integrator_91(in, out, clk);
   input [90:0]       in;
   output [90:0]      out;
   reg [90:0]         out;            
   input              clk;
   initial
     out <= 0;
   always @(posedge clk)
     out <= out + in;
endmodule 

module comb_91_cken(in, out, cken, clk);
   input [90:0]    in;
   output [90:0]   out;
   reg [90:0]      out;
   input           cken;
   input           clk;
   reg [90:0]      temp;
   always @ (posedge cken)
       begin
          temp <= in;
          out <= in - temp;
       end
endmodule 


// N=8, M=1 , R = 320
// Calculate bit growth from  N * Log2(R)
// eg. decim rate of 320,  bit growth  = 67  hence register size is 24 + 67 = 91 

// in and out is 24 bits

module cic_320_1_8(in, out, cken, clk);
   input [23:0]    in;
   output reg [23:0]   out;
   input           cken;
   input           clk;
   wire [90:0]     i1, i2, i3,i4, i5,i6, i7, i8, c1, c2, c3, c4, c5, c6, c7, c8;
   /* Integrators */
   /* Sign extend input */
   integrator_91 int1({{65{in[23]}}, in[23:0]}, i1, clk);
   integrator_91 int2(i1, i2, clk);
   integrator_91 int3(i2, i3, clk);
   integrator_91 int4(i3, i4, clk);
   integrator_91 int5(i4, i5, clk);
   integrator_91 int6(i5, i6, clk);
   integrator_91 int7(i6, i7, clk);
   integrator_91 int8(i6, i8, clk);

   // Combs
   comb_91_cken comb1(i8, c1, cken, clk);
   comb_91_cken comb2(c1, c2, cken, clk);
   comb_91_cken comb3(c2, c3, cken, clk);
   comb_91_cken comb4(c3, c4, cken, clk);
   comb_91_cken comb5(c4, c5, cken, clk);
   comb_91_cken comb6(c5, c6, cken, clk);
   comb_91_cken comb7(c6, c7, cken, clk);
   comb_91_cken comb8(c7, c8, cken, clk);

   always @ (posedge clk)
     begin
        out <= c8[90:67]; 
     end
endmodule 





