//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Major portions ripped-off from USRP FPGA verilog code Copyright (C) 2003 Matt Ettus
//
// HPSDR CORDIC NCO TOP LEVEL DESIGN FILE
// P. Covington N8VB
// CORDIC NCO
//
module CORDIC_NCO_TOP(clk,reset,Iin,Qin,Iout,Qout);
	parameter FREQ = 32'b0100000000001111;

	input clk;
	input reset;
	input 	[15:0] Iin;
	input 	[15:0] Qin;

	output 	[15:0] Iout;
	output 	[15:0] Qout;

	wire	[31:0] phase;

	// The phase accumulator takes a 32 bit frequency dword and outputs a 32 bit phase dword on each clock
	phase_accumulator rx_phase_accumulator(.clk(clk),.reset(reset),.frequency(FREQ),.phase_out(phase));

	// The cordic takes I and Q in along with the top 16 bits of the phase dword.  The I and Q out are freq shifted
	cordic rx_cordic(.clk(clk),.reset(reset),.Iin(Iin),.Qin(Qin),.PHin(phase[31:16]),.Iout(Iout),.Qout(Qout),.PHout());

endmodule