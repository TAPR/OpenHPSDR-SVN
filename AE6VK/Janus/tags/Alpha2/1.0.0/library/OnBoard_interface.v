// Copyright (c) 2007 Christopher T. Day (AE6VK@Yahoo.com)
//
// HPSDR - High Performance Software Defined Radio
//
// Interface between Janus OnBoard discrete hardware and the
// JanusCPLD system.
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
module OnBoard_interface (
	// Pins
	input	OnBoard_XO_out,
	output	OnBoard_XO_tune,
	output	OnBoard_PWMFilter_I,
	output	OnBoard_PWMFilter_Q,
	input	OnBoard_nPTT,
	output	Ref_OK,
	output	Lock_OK,
	inout	OnBoard_TP3,
	// Wires
	output	Clk,
	input	Ref_Clk,
	input	PWMI,
	input	PWMQ,
	output	nPTT,
	inout	TP3
	);
	
	// Assemble the full interface from its components.	
	XO_interface XO (
		OnBoard_XO_out, 
		OnBoard_XO_tune, 
		Ref_OK,
		Lock_OK,
		Ref_Clk,
		Clk);
	
	PWMFilter_interface PWMFilter (OnBoard_PWMFilter_I, 
								   OnBoard_PWMFilter_Q,
								   PWMI, PWMQ);
		
	PTT_interface PTTHrdw (OnBoard_nPTT, nPTT);
	
	TestHeader_interface TestHeaderJ7 (OnBoard_TP3, TP3);

endmodule
