// 
// Copyright (c) Christopher T. Day 2007 (ae6vk@yahoo.com)
//
//  HPSDR - High Performance Software Defined Radio
//
//
//  LED test program of TAPR's RefLock II (tm)  
// 
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
// CPLD code RefLock II. Lights the LEDs.
module LEDTest
	(
		output LED1,
		output LED2
	);

	// Light up active-high LEDs
	assign LED1 = 1'b1;
	assign LED2 = 1'b1;

endmodule
