// Copyright (c) 2007 Christopher T. Day (AE6VK@Yahoo.com)
//
// HPSDR - High Performance Software Defined Radio
//
// Interface between Janus AKM AK5394A ADC and the JanusCPLD system.
// On Janus, the AK5394A runs in I2S Master Mode.
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
module ADC_interface (
	// Pins
	input	ADC_SCLK,
	input	ADC_LRCK,
	input	ADC_FSYNC,
	input	ADC_SDATA,
	output	ADC_SMODE1,
	output	ADC_SMODE2,
	output	ADC_MCLK,
	output	ADC_DFS0,
	output	ADC_DFS1,
	output	ADC_HPFE,
	output	ADC_ZCAL,
	input	ADC_CAL,
	output	ADC_RSTN,
	// Wires
	output	[2:0] ADC_I2S,		// I2S[SD, WS, SCK]
	input	[1:0] ADC_Speed,
	input	ADC_Clk,			// Master clock
	input	ADC_nReset
	);
	
	// Set ADC into I2S Master mode
	assign ADC_SMODE1 = 1'b1;
	assign ADC_SMODE2 = 1'b1;
	// Set to calibrate from A/D inputs
	assign ADC_ZCAL = 1'b1;
	// Set High Pass Filter (>1Hz) on
	assign ADC_HPFE = 1'b1;
	
	// Wire-up I2S bus
	// N.B. - ADC_FSYNC is ignored by Janus
	assign ADC_I2S = {ADC_SDATA, ADC_LRCK, ADC_SCLK};
	// Wire-up Speed bus
	assign {ADC_DFS1, ADC_DFS0} = ADC_Speed;
	// Wire-up Master clock
	assign ADC_MCLK = ADC_Clk;

	// Instantiate AK5394A reset circuitry.
	AK5394A_reset ADC_reset (ADC_RSTN, ADC_nReset, ADC_Clk);
	
endmodule
