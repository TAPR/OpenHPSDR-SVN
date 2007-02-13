// HPSDR Cordic Function
// 
// Author: Steve Wilson
// License: LGPL 
// Date: 8/20/2006 
// This function receives a ADC Value and returns the I and Q values
//
// The ADC Value is expected to be 16 bits 
// The Phase is in degrees also following a 9.16 format.  The
// integer part of the phase should go between 0 and 360 degrees, while
// the decimal part will go from 0-ffff where addtion of 24'h1 would 
// equal 1 degree.
//
// Per Phil there is no gain correction. 
//
//
//
module hpsdr_cordic(clk, rst, adc_val,phase,i,q);

parameter AWIDTH = 16;  // ADC 
parameter PWIDTH = 25;  // Phase Width 9 bits int.16 bits decimal
input               clk;  // Conversion Clock
input               rst;  // Power on reset
input  [AWIDTH-1:0] adc_val;
input  [PWIDTH-1:0] phase;
output [AWIDTH-1:0] i;
output [AWIDTH-1:0] q;

reg [PWIDTH-1:0] cur_angle;
reg [AWIDTH-1:0] x,y;
`define ANG90   25'h5a_0000 
`define ANG180  25'hb4_0000
`define ANG270  25'h010e_0000
`define ANG360  25'h0168_0000

  /* Coeff table from original test bench 
  // Decimal point is between bits 17 & 16
  angle[0] = 32'h2d0000; // atan(1) = 45 degrees * 2^16
  angle[1] = 32'h1a90a3; // atan(1/2) = 26.565 degrees * 2^16
  angle[2] = 32'he0944;  // etc.
  angle[3] = 32'h72001;
  angle[4] = 32'h3938a;
  angle[5] = 32'h1ca37;
  angle[6] = 32'he52a;
  angle[7] = 32'h7296;
  angle[8] = 32'h394b;
  angle[9] = 32'h1ca5;
  angle[10] = 32'he52;
  angle[11] = 32'h729;
  angle[12] = 32'h394;
  angle[13] = 32'h1ca;
  angle[14] = 32'he5;
  angle[15] = 32'h72;
  angle[16] = 32'h39;
  angle[17] = 32'h1c;
*/ 

  // Wire defines
  wire [AWIDTH-1:0] i0,i1,i2,i3,i4,i5,i6,i7,i8,i9,i10,i11,i12,i13,i14,i15,i16;
  wire [AWIDTH-1:0] q0,q1,q2,q3,q4,q5,q6,q7,q8,q9,q10,q11,q12,q13,q14,q15,q16;
  wire [AWIDTH-1:0] p0,p1,p2,p3,p4,p5,p6,p7,p8,p9,p10,p11,p12,p13,p14,p15,p16;
  //
  // Receive incoming phase and calculate initial conditions to 
  // the Calculation arra
  //
always @(phase or adc_val)
  if((phase >= `ANG90) && (phase < `ANG180))
    begin
      x = 0;
      y = adc_val;
      cur_angle = `ANG90; 
    end	    
  else    
  if((phase >= `ANG270) && (phase < `ANG360))
    begin
      x = 0;
      y = -(adc_val);
      cur_angle = `ANG270; 
    end	    
  else   
  if((phase >= `ANG180) && (phase < `ANG270))
    begin
      x = -(adc_val);
      y = 0;
      cur_angle = `ANG180; 
    end	    
  else
    begin	   
      x = adc_val;
      y = 0;
      cur_angle = 0;
    end

cslice #(AWIDTH,PWIDTH,0) cs0   (clk,rst,25'h2d0000,phase,  x,  y,cur_angle,i0,q0,p0);
cslice #(AWIDTH,PWIDTH,1) cs1   (clk,rst,25'h1a90a3,phase, i0, q0, p0, i1, q1, p1);
cslice #(AWIDTH,PWIDTH,2) cs2   (clk,rst,25'he0944 ,phase, i1, q1, p1, i2, q2, p2);
cslice #(AWIDTH,PWIDTH,3) cs3   (clk,rst,25'h72001 ,phase, i2, q2, p2, i3, q3, p3);
cslice #(AWIDTH,PWIDTH,4) cs4   (clk,rst,25'h3938a ,phase, i3, q3, p3, i4, q4, p4);
cslice #(AWIDTH,PWIDTH,5) cs5   (clk,rst,25'h1ca37 ,phase, i4, q4, p4, i5, q5, p5);
cslice #(AWIDTH,PWIDTH,6) cs6   (clk,rst,25'he52a  ,phase, i5, q5, p5, i6, q6, p6);
cslice #(AWIDTH,PWIDTH,7) cs7   (clk,rst,25'h7296  ,phase, i6, q6, p6, i7, q7, p7);
cslice #(AWIDTH,PWIDTH,8) cs8   (clk,rst,25'h394b  ,phase, i7, q7, p7, i8, q8, p8);
cslice #(AWIDTH,PWIDTH,9) cs9   (clk,rst,25'h1ca5  ,phase, i8, q8, p8, i9, q9, p9);
cslice #(AWIDTH,PWIDTH,10) cs10 (clk,rst,25'he52   ,phase, i9, q9, p9,i10,q10,p10);
cslice #(AWIDTH,PWIDTH,11) cs11 (clk,rst,25'h729   ,phase,i10,q10,p10,i11,q11,p11);
cslice #(AWIDTH,PWIDTH,12) cs12 (clk,rst,25'h394   ,phase,i11,q11,p11,i12,q12,p12);
cslice #(AWIDTH,PWIDTH,13) cs13 (clk,rst,25'h1ca   ,phase,i12,q12,p12,i13,q13,p13);
cslice #(AWIDTH,PWIDTH,14) cs14 (clk,rst,25'he5    ,phase,i13,q13,p13,i14,q14,p14);
cslice #(AWIDTH,PWIDTH,15) cs15 (clk,rst,25'h72    ,phase,i14,q14,p14,i15,q15,p15);
cslice #(AWIDTH,PWIDTH,16) cs16 (clk,rst,25'h39    ,phase,i15,q15,p15,i16,q16,p16);


assign i = i16;
assign q = q16;

endmodule
//
// This code is used to unroll the original for loop that implments the
// 1 +/- tan(0) cordic algorithm.
//
module cslice (
                   clk, 
                   rst, 
				 coeff,
		   		 tphase,
                   i_in, 
                   q_in, 
                   phase_in, 
                   i_out, 
                   q_out, 
                   phase_out
);
parameter IQ_WIDTH = 25;  // I & Q width 
parameter PH_WIDTH = 25;  // Phase angle Width
parameter SHIFT     = 1;   // Shift Amount
input                 clk;
input                 rst;
input [PH_WIDTH-1:0]  tphase;
input [PH_WIDTH-1:0]  coeff;
input [IQ_WIDTH-1:0]  i_in;
input [IQ_WIDTH-1:0]  q_in;
input [PH_WIDTH-1:0]  phase_in;
output [IQ_WIDTH-1:0]  i_out;
output [IQ_WIDTH-1:0]  q_out;
output [PH_WIDTH-1:0]  phase_out;

// Determine whether we have a positive or negative operation
reg  [IQ_WIDTH-1:0]  i_reg;
reg  [IQ_WIDTH-1:0]  q_reg;
reg  [PH_WIDTH-1:0]  phase_reg;
wire [PH_WIDTH-1:0]  phase_diff = tphase - phase_in;

// This code deals with sign extension after the verilog shift operation.

wire [IQ_WIDTH-1:0] q_ext = ({IQ_WIDTH{q_in[IQ_WIDTH-1]}} << (IQ_WIDTH-SHIFT)) &
                            {IQ_WIDTH{1'b1}};

wire [IQ_WIDTH-1:0] i_ext = ({IQ_WIDTH{i_in[IQ_WIDTH-1]}} <<(IQ_WIDTH-SHIFT)) & 
	                    {IQ_WIDTH{1'b1}};  

// Basic working loop
always @(posedge clk or posedge rst)
  if(rst)	 
    begin
      i_reg     <= 0; 
      q_reg     <= 0;
      phase_reg <= 0;
    end	  
  else
  if(~phase_diff[PH_WIDTH-1])
    begin
      i_reg     <= i_in - (q_ext | (q_in >> SHIFT));
      q_reg     <= {i_ext | (i_in>>SHIFT) } + q_in;
      phase_reg <= phase_in + coeff;
    end
  else
    begin
      i_reg     <=  i_in + (q_ext | (q_in >> SHIFT));
      q_reg     <= -(i_ext | (i_in >> SHIFT))+q_in;
      phase_reg <= phase_in - coeff;
    end  

assign i_out = i_reg;
assign q_out = q_reg;
assign phase_out = phase_reg;

endmodule
