//	Copyright (C) 1988-2004 Altera Corporation

//	Any megafunction design, and related net list (encrypted or decrypted),
//	support information, device programming or simulation file, and any other
//	associated documentation or information provided by Altera or a partner
//	under Altera's Megafunction Partnership Program may be used only to
//	program PLD devices (but not masked PLD devices) from Altera.  Any other
//	use of such megafunction design, net list, support information, device
//	programming or simulation file, or any other related documentation or
//	information is prohibited for any other purpose, including, but not
//	limited to modification, reverse engineering, de-compiling, or use with
//	any other silicon devices, unless such use is explicitly licensed under
//	a separate agreement with Altera or a megafunction partner.  Title to
//	the intellectual property, including patents, copyrights, trademarks,
//	trade secrets, or maskworks, embodied in any such megafunction design,
//	net list, support information, device programming or simulation file, or
//	any other related documentation or information provided by Altera or a
//	megafunction partner, remains with Altera, the megafunction partner, or
//	their respective licensors.  No other licenses, including any licenses
//	needed under any third party's intellectual property, are provided herein.

//   NCO COMPILER ver 2.2.2 VERILOG HDL TESTBENCH
`timescale 1ns / 100ps
module a_nco_tb;
wire [15:0] sin_val;
wire [15:0] cos_val;
reg [31:0] phi;
wire data_ready;
reg reset;
reg clken;
reg clk;

initial
  begin
    $dumpvars;
    #0 clk = 1'b0;
    #0 reset = 1'b1;
    #0 clken = 1'b1;
    #0 phi = 32'b00000010100011110101110000101001;
    #70 reset = 1'b0;
  end

always
  begin
    #5 clk = 1;
    #5 clk = 0;
  end

integer sin_ch, sin_print;
integer cos_ch, cos_print;
initial
  begin
    sin_ch = $fopen ("fsin_o_ver_a_nco.txt");
    cos_ch = $fopen ("fcos_o_ver_a_nco.txt");
  end

always @(posedge clk)
  begin
    if(reset==1'b0 & data_ready==1'b1)
      begin
        if (sin_val[15:0] < (1<<15))
          sin_print = sin_val[15:0];
        else
          sin_print =  sin_val[15:0] - (1<<16);

      if (cos_val[15:0] < (1<<15))
        cos_print = cos_val[15:0];
      else
        cos_print =  cos_val[15:0] - (1<<16);

    $fdisplay (sin_ch, "%d", sin_print);
    $fdisplay (cos_ch, "%d", cos_print);
      end
end

a_nco i_a_nco (
    .data_ready(data_ready),
    .fsin_o(sin_val[15:0]),
    .fcos_o(cos_val[15:0]),
    .phi_inc_i(phi[31:0]),
    .reset(reset),
    .clken(clken),
    .clk(clk)
    );

endmodule
