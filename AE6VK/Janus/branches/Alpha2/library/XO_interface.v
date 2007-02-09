// Copyright (c) 2007 Christopher T. Day (AE6VK@Yahoo.com)
//
// HPSDR - High Performance Software Defined Radio
//
// Interface between Janus OnBoard crystal Oscillator (XO) and 
// JanusCPLD system clock, which runs at 12.288MHz.
// 
// The software supports the Alpha2 version of the Janus board.
//
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 
// 02111-1307  USA
//
//
module XO_interface (
	// Pins
	input	OnBoard_XO_out,
	output	OnBoard_XO_tune,
	output	reg Ref_OK,
	output	Lock_OK,
	// Wires
	input Ref_Clk,
	output Clk
	);
	
	wire Clk_Lock;	// PLL loop locked
	wire Tune;		// Tunes XO to reference
	wire Center;	// Tunes XO to zero offset
	
	// Since the Alpha2 XO runs at 12.288MHz, we can wire it 
	// directly to the system clock.
	assign Clk = OnBoard_XO_out;
	
	// Instantiate PLL
	JanusPLL PLL(.Ref_10000kHz(Ref_Clk), 
				 .Clk_12288kHz(OnBoard_XO_out), 
				 .lock(Clk_Lock),
				 .pfd_out(Tune),
				 .center_tune(Center));
	
	// If Ref_Clk is never high, there is no Ref_Clk
	always @ (posedge Ref_Clk or posedge Center) 
	begin
		Ref_OK <= Ref_Clk;
	end
	
	// Tune the XO if reference is present
	assign OnBoard_XO_tune = Ref_OK ? Tune : Center;
	
	// Signal good lock
	assign Lock_OK = Clk_Lock & Ref_OK;
	
endmodule
