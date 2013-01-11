// megafunction wizard: %Shift register (RAM-based)%
// GENERATION: STANDARD
// VERSION: WM1.0
// MODULE: altshift_taps 

// ============================================================
// File Name: fir_shiftreg.v
// Megafunction Name(s):
// 			altshift_taps
//
// Simulation Library Files(s):
// 			altera_mf
// ============================================================
// ************************************************************
// THIS IS A WIZARD-GENERATED FILE. DO NOT EDIT THIS FILE!
//
// 8.1 Build 163 10/28/2008 SJ Web Edition
// ************************************************************


//Copyright (C) 1991-2008 Altera Corporation
//Your use of Altera Corporation's design tools, logic functions 
//and other software and tools, and its AMPP partner logic 
//functions, and any output files from any of the foregoing 
//(including device programming or simulation files), and any 
//associated documentation or information are expressly subject 
//to the terms and conditions of the Altera Program License 
//Subscription Agreement, Altera MegaCore Function License 
//Agreement, or other applicable license agreement, including, 
//without limitation, that your use is for the sole purpose of 
//programming logic devices manufactured by Altera and sold by 
//Altera or its authorized distributors.  Please refer to the 
//applicable agreement for further details.


// synopsys translate_off
`timescale 1 ps / 1 ps
// synopsys translate_on
module fir_shiftreg (
	clken,
	clock,
	shiftin,
	shiftout,
	taps);

	input	  clken;
	input	  clock;
	input	[24:0]  shiftin;
	output	[24:0]  shiftout;
	output	[24:0]  taps;

	wire [24:0] sub_wire0;
	wire [24:0] sub_wire1;
	wire [24:0] taps = sub_wire0[24:0];
	wire [24:0] shiftout = sub_wire1[24:0];

	altshift_taps	altshift_taps_component (
				.clken (clken),
				.clock (clock),
				.shiftin (shiftin),
				.taps (sub_wire0),
				.shiftout (sub_wire1)
				// synopsys translate_off
				,
				.aclr ()
				// synopsys translate_on
				);
	defparam
		altshift_taps_component.lpm_hint = "RAM_BLOCK_TYPE=AUTO",
		altshift_taps_component.lpm_type = "altshift_taps",
		altshift_taps_component.number_of_taps = 1,
		altshift_taps_component.tap_distance = 256,
		altshift_taps_component.width = 25;


endmodule

// ============================================================
// CNX file retrieval info
// ============================================================
// Retrieval info: PRIVATE: ACLR NUMERIC "0"
// Retrieval info: PRIVATE: CLKEN NUMERIC "1"
// Retrieval info: PRIVATE: GROUP_TAPS NUMERIC "0"
// Retrieval info: PRIVATE: INTENDED_DEVICE_FAMILY STRING "Cyclone III"
// Retrieval info: PRIVATE: NUMBER_OF_TAPS NUMERIC "1"
// Retrieval info: PRIVATE: RAM_BLOCK_TYPE NUMERIC "3"
// Retrieval info: PRIVATE: SYNTH_WRAPPER_GEN_POSTFIX STRING "0"
// Retrieval info: PRIVATE: TAP_DISTANCE NUMERIC "256"
// Retrieval info: PRIVATE: WIDTH NUMERIC "25"
// Retrieval info: CONSTANT: LPM_HINT STRING "RAM_BLOCK_TYPE=AUTO"
// Retrieval info: CONSTANT: LPM_TYPE STRING "altshift_taps"
// Retrieval info: CONSTANT: NUMBER_OF_TAPS NUMERIC "1"
// Retrieval info: CONSTANT: TAP_DISTANCE NUMERIC "256"
// Retrieval info: CONSTANT: WIDTH NUMERIC "25"
// Retrieval info: USED_PORT: clken 0 0 0 0 INPUT VCC clken
// Retrieval info: USED_PORT: clock 0 0 0 0 INPUT NODEFVAL clock
// Retrieval info: USED_PORT: shiftin 0 0 25 0 INPUT NODEFVAL shiftin[24..0]
// Retrieval info: USED_PORT: shiftout 0 0 25 0 OUTPUT NODEFVAL shiftout[24..0]
// Retrieval info: USED_PORT: taps 0 0 25 0 OUTPUT NODEFVAL taps[24..0]
// Retrieval info: CONNECT: @shiftin 0 0 25 0 shiftin 0 0 25 0
// Retrieval info: CONNECT: shiftout 0 0 25 0 @shiftout 0 0 25 0
// Retrieval info: CONNECT: taps 0 0 25 0 @taps 0 0 25 0
// Retrieval info: CONNECT: @clock 0 0 0 0 clock 0 0 0 0
// Retrieval info: CONNECT: @clken 0 0 0 0 clken 0 0 0 0
// Retrieval info: LIBRARY: altera_mf altera_mf.altera_mf_components.all
// Retrieval info: GEN_FILE: TYPE_NORMAL fir_shiftreg.v TRUE
// Retrieval info: GEN_FILE: TYPE_NORMAL fir_shiftreg.inc FALSE
// Retrieval info: GEN_FILE: TYPE_NORMAL fir_shiftreg.cmp FALSE
// Retrieval info: GEN_FILE: TYPE_NORMAL fir_shiftreg.bsf FALSE
// Retrieval info: GEN_FILE: TYPE_NORMAL fir_shiftreg_inst.v FALSE
// Retrieval info: GEN_FILE: TYPE_NORMAL fir_shiftreg_bb.v FALSE
// Retrieval info: GEN_FILE: TYPE_NORMAL fir_shiftreg_waveforms.html FALSE
// Retrieval info: GEN_FILE: TYPE_NORMAL fir_shiftreg_wave*.jpg FALSE
// Retrieval info: LIB_FILE: altera_mf
