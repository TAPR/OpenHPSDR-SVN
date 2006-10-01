/*
 * This implements a 31-tap halfband filter that decimates by two.
 * The coefficients are symmetric, and with the exception of the middle tap,
 * every other coefficient is zero.  The middle section of taps looks like this:
 *
 *  ..., -1468, 0, 2950, 0, -6158, 0, 20585, 32768, 20585, 0, -6158, 0, 2950, 0, -1468, ...
 *                                             |
 *                           middle tap -------+
 *
 * See coeff_rom.v for the full set.  The taps are scaled relative to 32768,
 * thus the middle tap equals 1.0.  Not counting the middle tap, there are 8
 * non-zero taps on each side, and they are symmetric.  A naive implementation
 * requires a mulitply for each non-zero tap.  Because of symmetry, we can
 * replace 2 multiplies with 1 add and 1 multiply.  Thus, to compute each output
 * sample, we need to perform 8 multiplications.  Since the middle tap is 1.0,
 * we just add the corresponding delay line value.
 *
 * About timing: We implement this with a single multiplier, so it takes
 * 8 cycles to compute a single output.  However, since we're decimating by two 
 * we can accept a new input value every 4 cycles.  strobe_in is asserted when
 * there's a new input sample available.  Depending on the overall decimation
 * rate, strobe_in may be asserted less frequently than once every 4 clocks.
 * On the output side, we assert strobe_out when output contains a new sample.
 *
 * Implementation: Every time strobe_in is asserted we store the new data into
 * the delay line.  We split the delay line into two components, one for the
 * even samples, and one for the odd samples.  ram16_odd is the delay line for
 * the odd samples.  This ram is written on each odd assertion of strobe_in, and
 * is read on each clock when we're computing the dot product.  ram16_even is
 * similar, although because it holds the even samples we must be able to read
 * two samples from different addresses at the same time, while writing the incoming
 * even samples. Thus it's "triple-ported".
 */
module coeff_rom (input clock, input [2:0] addr, output reg [15:0] data);

/*
   always @(posedge clock)
     case (addr)
       3'd0 : data <= #1 -16'd16; //-.0005
       3'd1 : data <= #1 16'd74; //.0023
       3'd2 : data <= #1 -16'd254; //-.0078
       3'd3 : data <= #1 16'd669; //.0204
       3'd4 : data <= #1 -16'd1468; //-.0448
       3'd5 : data <= #1 16'd2950; //.0090
       3'd6 : data <= #1 -16'd6158; //-.1879
       3'd7 : data <= #1 16'd20585; //.6282
     endcase // case(addr)
*/

	always @(posedge clock)
     case (addr)
       3'd0 : data <= #1 -16'd1337; 
       3'd1 : data <= #1 16'd1558; 
       3'd2 : data <= #1 -16'd1857; 
       3'd3 : data <= #1 16'd2285; 
       3'd4 : data <= #1 -16'd2955; 
       3'd5 : data <= #1 16'd4154; 
       3'd6 : data <= #1 -16'd6943; 
       3'd7 : data <= #1 16'd20857; 
     endcase // case(addr)
      
endmodule // coeff_rom


