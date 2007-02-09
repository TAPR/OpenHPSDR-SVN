// Copyright (c) 2007 Christopher T. Day (AE6VK@Yahoo.com)
//
// HPSDR - High Performance Software Defined Radio
//
// Interface between Janus OnBoard J7 Test Header and the
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
module TestHeader_interface (
	// Pins
	inout	OnBoard_TP3,
	// Wires
	inout	TP3
	);
	
	// Wire up Test Header.	
	tran (OnBoard_TP3, TP3);
endmodule
