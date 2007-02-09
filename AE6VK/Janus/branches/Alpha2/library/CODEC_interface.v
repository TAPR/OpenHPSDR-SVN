// Copyright (c) 2007 Christopher T. Day (AE6VK@Yahoo.com)
//
// HPSDR - High Performance Software Defined Radio
//
// Interface between Janus TI TLV320AIC23B CODEC and the JanusCPLD
// system. On Janus, the TLV320AIC23B runs in I2S Master Mode.
// [I plan to change this to Master Mode as an exersize.]
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
module CODEC_interface (
	// Pins
	output	CODEC_CSn,
	output	CODEC_MODE,
	inout	CODEC_SDIN,
	inout	CODEC_SCLK,
	output	CODEC_DIN,
	input	CODEC_DOUT,
	output	CODEC_LRCIN,
	output	CODEC_LRCOUT,
	output	CODEC_BCLK,
	output	CODEC_MCLK,
	// Wires
	inout	[3:0] CODEC_I2S,	// I2S[SDIN, SDOUT, WSINOUT, SCK]
	inout	[1:0] CODEC_I2C,	// I2C[SDIN, SCLK]
	input	CODEC_Clk			// Master clock
	);
	
	// Wire-up I2S TXVR bus
	assign CODEC_DIN    = CODEC_I2S[3];
	assign CODEC_LRCIN  = CODEC_I2S[1];
	assign CODEC_I2S[2] = CODEC_DOUT;
	assign CODEC_LRCOUT = CODEC_I2S[1];
	assign CODEC_BCLK   = CODEC_I2S[0];
	// Wire-up Master clock
	assign CODEC_MCLK = CODEC_Clk;

	// Fix TLV320AIC23B in I2C mode & I2C address
	// to 0x1A. Ozy currently does all the initialization.
	assign CODEC_MODE = 1'b0;
	assign CODEC_CSn  = 1'b0;
	
	// Wire-up I2C bus
	tran SDIN (CODEC_SDIN, CODEC_I2C[1]);
	tran SCLK (CODEC_SCLK, CODEC_I2C[0]);
	
endmodule
