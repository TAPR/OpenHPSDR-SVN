// megafunction wizard: %PARALLEL_ADD%VBB%
// GENERATION: STANDARD
// VERSION: WM1.0
// MODULE: parallel_add 

// ============================================================
// File Name: sigadder.v
// Megafunction Name(s):
// 			parallel_add
//
// Simulation Library Files(s):
// 			altera_mf
// ============================================================
// ************************************************************
// THIS IS A WIZARD-GENERATED FILE. DO NOT EDIT THIS FILE!
//
// 9.1 Build 304 01/25/2010 SP 1 SJ Web Edition
// ************************************************************

//Copyright (C) 1991-2010 Altera Corporation
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

module sigadder (
	clock,
	data0x,
	data1x,
	data2x,
	data3x,
	data4x,
	data5x,
	data6x,
	data7x,
	result);

	input	  clock;
	input	[15:0]  data0x;
	input	[15:0]  data1x;
	input	[15:0]  data2x;
	input	[15:0]  data3x;
	input	[15:0]  data4x;
	input	[15:0]  data5x;
	input	[15:0]  data6x;
	input	[15:0]  data7x;
	output	[18:0]  result;
`ifndef ALTERA_RESERVED_QIS
// synopsys translate_off
`endif
	tri0	  clock;
`ifndef ALTERA_RESERVED_QIS
// synopsys translate_on
`endif

endmodule

// ============================================================
// CNX file retrieval info
// ============================================================
// Retrieval info: PRIVATE: INTENDED_DEVICE_FAMILY STRING "Cyclone II"
// Retrieval info: PRIVATE: SYNTH_WRAPPER_GEN_POSTFIX STRING "0"
// Retrieval info: LIBRARY: altera_mf altera_mf.altera_mf_components.all
// Retrieval info: CONSTANT: MSW_SUBTRACT STRING "NO"
// Retrieval info: CONSTANT: PIPELINE NUMERIC "1"
// Retrieval info: CONSTANT: REPRESENTATION STRING "SIGNED"
// Retrieval info: CONSTANT: RESULT_ALIGNMENT STRING "LSB"
// Retrieval info: CONSTANT: SHIFT NUMERIC "0"
// Retrieval info: CONSTANT: SIZE NUMERIC "8"
// Retrieval info: CONSTANT: WIDTH NUMERIC "16"
// Retrieval info: CONSTANT: WIDTHR NUMERIC "19"
// Retrieval info: USED_PORT: clock 0 0 0 0 INPUT GND "clock"
// Retrieval info: USED_PORT: data0x 0 0 16 0 INPUT NODEFVAL "data0x[15..0]"
// Retrieval info: USED_PORT: data1x 0 0 16 0 INPUT NODEFVAL "data1x[15..0]"
// Retrieval info: USED_PORT: data2x 0 0 16 0 INPUT NODEFVAL "data2x[15..0]"
// Retrieval info: USED_PORT: data3x 0 0 16 0 INPUT NODEFVAL "data3x[15..0]"
// Retrieval info: USED_PORT: data4x 0 0 16 0 INPUT NODEFVAL "data4x[15..0]"
// Retrieval info: USED_PORT: data5x 0 0 16 0 INPUT NODEFVAL "data5x[15..0]"
// Retrieval info: USED_PORT: data6x 0 0 16 0 INPUT NODEFVAL "data6x[15..0]"
// Retrieval info: USED_PORT: data7x 0 0 16 0 INPUT NODEFVAL "data7x[15..0]"
// Retrieval info: USED_PORT: result 0 0 19 0 OUTPUT NODEFVAL "result[18..0]"
// Retrieval info: CONNECT: @data 0 0 16 96 data6x 0 0 16 0
// Retrieval info: CONNECT: @data 0 0 16 80 data5x 0 0 16 0
// Retrieval info: CONNECT: @data 0 0 16 64 data4x 0 0 16 0
// Retrieval info: CONNECT: @data 0 0 16 48 data3x 0 0 16 0
// Retrieval info: CONNECT: @data 0 0 16 32 data2x 0 0 16 0
// Retrieval info: CONNECT: @data 0 0 16 16 data1x 0 0 16 0
// Retrieval info: CONNECT: @data 0 0 16 0 data0x 0 0 16 0
// Retrieval info: CONNECT: result 0 0 19 0 @result 0 0 19 0
// Retrieval info: CONNECT: @clock 0 0 0 0 clock 0 0 0 0
// Retrieval info: CONNECT: @data 0 0 16 112 data7x 0 0 16 0
// Retrieval info: GEN_FILE: TYPE_NORMAL sigadder.v TRUE
// Retrieval info: GEN_FILE: TYPE_NORMAL sigadder.inc FALSE
// Retrieval info: GEN_FILE: TYPE_NORMAL sigadder.cmp FALSE
// Retrieval info: GEN_FILE: TYPE_NORMAL sigadder.bsf FALSE
// Retrieval info: GEN_FILE: TYPE_NORMAL sigadder_inst.v FALSE
// Retrieval info: GEN_FILE: TYPE_NORMAL sigadder_bb.v TRUE
// Retrieval info: LIB_FILE: altera_mf