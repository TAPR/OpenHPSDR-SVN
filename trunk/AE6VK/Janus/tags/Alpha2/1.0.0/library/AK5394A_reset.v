// Copyright (c) 2007 Christopher T. Day (AE6VK@Yahoo.com)
//
// HPSDR - High Performance Software Defined Radio
//
// Reset logic for the AKM AK5394A ADC chip. We assume that
// the ADC_MCLK clock runs at 12.288MHz.
// 
// The software supports the Alpha1 version of the Janus board.
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
module AK5394A_reset (
	// Pins
	output reg ADC_RSTN,
	// Wires
	input ADC_nReset,
	input ADC_MCLK
	);
	
	reg nResetSync;
	reg nReset;
	reg nWideReset;
	reg [1:0] resetWidth;
	reg powerOn;
	reg [24:0] powerDelay;
	
	initial
	begin
		nResetSync <= 0;
		nReset <= 0;
		nWideReset <= 0;
		resetWidth <= 0;
		// Different reset procedure if the power was off
		powerOn <= 0;
		powerDelay <= 0;
	end
	
	// Synchronize the external reset signal
	always @ (posedge ADC_MCLK)
	begin
		nResetSync <= ADC_nReset;
		nReset <= nResetSync;
	end
	
	always @ (posedge ADC_MCLK)
	begin
		// Restart wide reset pulse on external reset
		if (!nReset)
		begin
			nWideReset <= 1'b0;
			resetWidth <= 2'b00;
		end
		// Make sure reset to AK5394A is at least 3 clocks long
		if (!nWideReset & (resetWidth < 2'b11))
		begin
			nWideReset <= 1'b0;
			resetWidth <= resetWidth + 2'b01;
		end
		// If power was off, delay reset to AK5394A
		// so VREF stabilizes before calibration starts.
		if (!powerOn) 
		begin
			if (powerDelay < 25'hCE5000)	// ~1.1sec
			begin
				powerOn <= 1'b0;
				// Take AK5394A to stabilized VREF
				nWideReset <= 1'b1;
				powerDelay = powerDelay + 25'b1;
			end
			else begin
				// Now reset AK5394A to calibrate
				resetWidth <= 1'b0;
				nWideReset <= 1'b0;
				// Don't repeat this cycle until power fails
				powerOn <= 1'b1;
			end
		end
		// Reset AK5394A
		ADC_RSTN <= nWideReset;
	end
	
endmodule
