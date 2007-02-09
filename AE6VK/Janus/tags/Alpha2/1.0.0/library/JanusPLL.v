// Copyright (c) 2007 Christopher T. Day (AE6VK@Yahoo.com)
//
// HPSDR - High Performance Software Defined Radio
//
// PLL between Janus OnBoard VCXO and a 10MHz reference clock
// on the Atlas bus.
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
module JanusPLL (
	// Wires
	input	Ref_10000kHz,
	input	Clk_12288kHz,
	output	lock,
	output	pfd_out,
	output	center_tune
	);
	
	wire RefDividedBy25;
	wire RefDividedBy625;
	reg Ref_8kHz;
	wire ClkDividedBy24;
	wire ClkDividedBy768;
	reg Clk_8kHz;
	wire nLock;
	
	// Count reference clock to 16kHz
	// Two steps to allow the chip
	// placement to meet timing requirements.	
	DivideBy25 FirstFactor25 (
		.clock(Ref_10000kHz), 
		.cout(RefDividedBy25));
	DivideBy25 SecondFactor25 (
		.clock(RefDividedBy25), 
		.cout(RefDividedBy625));
	
	// Count VCXO clock to 16kHz
	// Two steps to allow the chip
	// placement to meet timing requirements.	
	DivideBy24 FirstFactor24 (
		.clock(Clk_12288kHz), 
		.cout(ClkDividedBy24));
	DivideBy32 SecondFactor32 (
		.clock(ClkDividedBy24), 
		.cout(ClkDividedBy768));
	
	// Square up clocks to 8kHz square waves
	always @ (posedge RefDividedBy625)
	begin
		Ref_8kHz <= ~Ref_8kHz;
	end
	always @ (posedge ClkDividedBy768)
	begin
		Clk_8kHz <= ~Clk_8kHz;
	end
			
	// Feed the divided clocks to the PFD	
	pfd PFD_8kHz (
		.ref_in(Ref_8kHz),
		.osc_in(Clk_8kHz),
		.lock(nLock),
		.pfd_out(pfd_out));
	
	// Wire-up outputs
	// pfd_out is already wired from PFD_8Kz block.
	// nLock is inverted to be positive logic.
	// center_tune is 50% duty cycle 8kHz default tune.
	assign lock = ~nLock;
	assign center_tune = Clk_8kHz;
endmodule
