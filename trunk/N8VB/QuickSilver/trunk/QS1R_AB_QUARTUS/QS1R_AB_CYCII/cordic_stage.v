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
// HPSDR CORDIC STAGE
// P. Covington N8VB
// CORDIC STAGE
//
// Inputs:
// clk - clock 
// reset - reset the cordic stages
// Iin - I data in, width depends on CORDIC_WIDTH parameter, default = 16
// Qin - Q data in, width depends on CORDIC_WIDTH parameter, default = 16
// PHin - phase data in, width depends on PHASE_WIDTH parameter, default = 16
// coeff - arc tan coefficients
//
// Outputs:
// Iout - I data out, width depends on CORDIC_WIDTH parameter, default = 20
// Qout - Q data out, width depends on CORDIC_WIDTH parameter, default = 20
// PHout - phase data out, width depends on CORDIC_WIDTH parameter, default = 20

module cordic_stage(clk,reset,Iin,Qin,PHin,coeff,Iout,Qout,PHout);
	parameter CORDIC_WIDTH = 20;
	parameter PHASE_WIDTH = 20;
	parameter SHIFT = 1;
	
	input	clk;
	input	reset;
	input	[CORDIC_WIDTH-1:0] Iin;
	input	[CORDIC_WIDTH-1:0] Qin;
	input 	[PHASE_WIDTH-1:0] PHin;
	input	[PHASE_WIDTH-1:0] coeff;
	
	output	[CORDIC_WIDTH-1:0] Iout;
	output	[CORDIC_WIDTH-1:0] Qout;
	output	[PHASE_WIDTH-1:0] PHout;
	
	wire	phase_positive = ~PHin[PHASE_WIDTH-1]; // test for positive phase, flag
	
	reg		[CORDIC_WIDTH-1:0] Iout;
	reg		[CORDIC_WIDTH-1:0] Qout;
	reg		[PHASE_WIDTH-1:0] PHout;
	
	always @(posedge clk)
		if(reset)
			begin
				Iout <=  0;
				Qout <=  0;
				PHout <=  0;
			end
		else
			begin
				Iout <=  phase_positive ?
					Iin - {{SHIFT+1{Qin[CORDIC_WIDTH-1]}},Qin[CORDIC_WIDTH-2:SHIFT]} :
					Iin + {{SHIFT+1{Qin[CORDIC_WIDTH-1]}},Qin[CORDIC_WIDTH-2:SHIFT]};
				Qout <=  phase_positive ?
					Qin + {{SHIFT+1{Iin[CORDIC_WIDTH-1]}},Iin[CORDIC_WIDTH-2:SHIFT]} :
					Qin - {{SHIFT+1{Iin[CORDIC_WIDTH-1]}},Iin[CORDIC_WIDTH-2:SHIFT]};  // changed sign to match USRP
				PHout <=  phase_positive ?
					PHin - coeff :
					PHin + coeff ;
			end	
endmodule
