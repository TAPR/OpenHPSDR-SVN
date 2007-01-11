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
// Major portions ripped-off from USRP FPGA verilog code Copyright (C) 2003 Matt Ettus
//
// HPSDR CORDIC
// P. Covington N8VB
// CORDIC
//
// Inputs:
// clk - clock 
// reset - reset the cordic stages
// Iin - I data in, width depends on CORDIC_WIDTH parameter, default = 16
// Qin - Q data in, width depends on CORDIC_WIDTH parameter, default = 16
// PHin - phase data in, width depends on PHASE_WIDTH parameter, default = 16
//
// Outputs:
// Iout - I data out, width depends on CORDIC_WIDTH parameter, default = 16
// Qout - Q data out, width depends on CORDIC_WIDTH parameter, default = 16
// PHout - phase data out, width depends on CORDIC_WIDTH parameter, default = 16

module cordic(clk,reset,Iin,Qin,PHin,Iout,Qout,PHout);
	parameter CORDIC_WIDTH = 16;
	parameter PHASE_WIDTH = 16; 

	input	clk;
	input 	reset;
	input	[CORDIC_WIDTH-1:0] Iin;
	input	[CORDIC_WIDTH-1:0] Qin;
	input	[PHASE_WIDTH-1:0] PHin;
	
	output 	[CORDIC_WIDTH-1:0] Iout;
	output 	[CORDIC_WIDTH-1:0] Qout;
	output 	[PHASE_WIDTH-1:0] PHout;
	
	reg		[CORDIC_WIDTH+1:0] Istage0;
	reg		[CORDIC_WIDTH+1:0] Qstage0;
	reg		[PHASE_WIDTH-2:0] PHstage0;
	
	// set up I stage wires
	wire	[CORDIC_WIDTH+1:0] Istage1, Istage2, Istage3, Istage4, Istage5, Istage6, Istage7;
	wire	[CORDIC_WIDTH+1:0] Istage8, Istage9, Istage10, Istage11, Istage12,Istage13,Istage14;
	wire	[CORDIC_WIDTH+1:0] Istage15, Istage16;
	
	// set up Q stage wires
	wire	[CORDIC_WIDTH+1:0] Qstage1, Qstage2, Qstage3, Qstage4, Qstage5, Qstage6, Qstage7;
	wire	[CORDIC_WIDTH+1:0] Qstage8, Qstage9, Qstage10, Qstage11, Qstage12,Qstage13,Qstage14;
	wire	[CORDIC_WIDTH+1:0] Qstage15, Qstage16;
	
	// set up Phase (PH) stage wires
	wire	[PHASE_WIDTH-2:0] PHstage1, PHstage2, PHstage3, PHstage4, PHstage5, PHstage6, PHstage7;
	wire	[PHASE_WIDTH-2:0] PHstage8, PHstage9, PHstage10, PHstage11, PHstage12,PHstage13,PHstage14;
	wire	[PHASE_WIDTH-2:0] PHstage15, PHstage16;
	
	// I and Q extention wires
	wire	[CORDIC_WIDTH+1:0] Iin_Extended = {{2{Iin[CORDIC_WIDTH-1]}},Iin};
	wire	[CORDIC_WIDTH+1:0] Qin_Extended = {{2{Qin[CORDIC_WIDTH-1]}},Qin};
		
	always @(posedge clk)
		if(reset)
			begin
				Istage0 <= #1 0;
				Qstage0 <= #1 0;
				PHstage0 <= #1 0;
			end
		else
			begin
				PHstage0 <= #1 PHin[PHASE_WIDTH-2:0];
				case (PHin[PHASE_WIDTH-1:PHASE_WIDTH-2])
					2'b00, 2'b11:
						begin
							Istage0 <= #1 Iin_Extended;
							Qstage0 <= #1 Qin_Extended;
						end
					2'b01, 2'b10:
						begin
							Istage0 <= #1 -Iin_Extended;
							Qstage0 <= #1 -Qin_Extended;
						end
				endcase
			end
		
		// this is the cordic pipeline
		// The constants are calcuated as follows:  Using base 2^13 then 
		// arctan 1 = 45 degrees = 2^13 = d8192
		// arctan 1/2 = 26.565 degrees = 2^13 * 26.565/45 = d4836
		// arctan 1/4 = 14.036 degrees = 2^13 * 14.036/45 = d2555
		// arctan 1/8 =  7.125 degrees = 2^13 *  7.125/45 = d1297 etc
		
		
		
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,0) 
			cordic_stage0(clk,reset,Istage0,Qstage0,PHstage0,16'd8192,Istage1,Qstage1,PHstage1);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,1) 
			cordic_stage1(clk,reset,Istage1,Qstage1,PHstage1,16'd4836,Istage2,Qstage2,PHstage2);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,2) 
			cordic_stage2(clk,reset,Istage2,Qstage2,PHstage2,16'd2555,Istage3,Qstage3,PHstage3);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,3) 
			cordic_stage3(clk,reset,Istage3,Qstage3,PHstage3,16'd1297,Istage4,Qstage4,PHstage4);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,4) 
			cordic_stage4(clk,reset,Istage4,Qstage4,PHstage4,16'd651,Istage5,Qstage5,PHstage5);	
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,5) 
			cordic_stage5(clk,reset,Istage5,Qstage5,PHstage5,16'd326,Istage6,Qstage6,PHstage6);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,6) 
			cordic_stage6(clk,reset,Istage6,Qstage6,PHstage6,16'd163,Istage7,Qstage7,PHstage7);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,7) 
			cordic_stage7(clk,reset,Istage7,Qstage7,PHstage7,16'd81,Istage8,Qstage8,PHstage8);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,8) 
			cordic_stage8(clk,reset,Istage8,Qstage8,PHstage8,16'd41,Istage9,Qstage9,PHstage9);	
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,9) 
			cordic_stage9(clk,reset,Istage9,Qstage9,PHstage9,16'd20,Istage10,Qstage10,PHstage10);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,10) 
			cordic_stage10(clk,reset,Istage10,Qstage10,PHstage10,16'd10,Istage11,Qstage11,PHstage11);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,11) 
			cordic_stage11(clk,reset,Istage11,Qstage11,PHstage11,16'd5,Istage12,Qstage12,PHstage12);
		//cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,12) 
			//cordic_stage12(clk,reset,Istage12,Qstage12,PHstage12,16'd3,Istage13,Qstage13,PHstage13);
		//cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,13) 
			//cordic_stage13(clk,reset,Istage13,Qstage13,PHstage13,16'd1,Istage14,Qstage14,PHstage14);
		//cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,14) 
		//	cordic_stage14(clk,reset,Istage14,Qstage14,PHstage14,16'd1,Istage15,Qstage15,PHstage15);
		//cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,15) 
		//	cordic_stage15(clk,reset,Istage15,Qstage15,PHstage15,16'd0,Istage16,Qstage16,PHstage16);
			
		/*
		// this version uses base 2^15 to try and obtain higher accuracy
		
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,0) 
			cordic_stage0(clk,reset,Istage0,Qstage0,PHstage0,16'd32768,Istage1,Qstage1,PHstage1);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,1) 
			cordic_stage1(clk,reset,Istage1,Qstage1,PHstage1,16'd19344,Istage2,Qstage2,PHstage2);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,2) 
			cordic_stage2(clk,reset,Istage2,Qstage2,PHstage2,16'd10221,Istage3,Qstage3,PHstage3);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,3) 
			cordic_stage3(clk,reset,Istage3,Qstage3,PHstage3,16'd5188,Istage4,Qstage4,PHstage4);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,4) 
			cordic_stage4(clk,reset,Istage4,Qstage4,PHstage4,16'd2604,Istage5,Qstage5,PHstage5);	
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,5) 
			cordic_stage5(clk,reset,Istage5,Qstage5,PHstage5,16'd1303,Istage6,Qstage6,PHstage6);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,6) 
			cordic_stage6(clk,reset,Istage6,Qstage6,PHstage6,16'd652,Istage7,Qstage7,PHstage7);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,7) 
			cordic_stage7(clk,reset,Istage7,Qstage7,PHstage7,16'd326,Istage8,Qstage8,PHstage8);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,8) 
			cordic_stage8(clk,reset,Istage8,Qstage8,PHstage8,16'd163,Istage9,Qstage9,PHstage9);	
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,9) 
			cordic_stage9(clk,reset,Istage9,Qstage9,PHstage9,16'd81,Istage10,Qstage10,PHstage10);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,10) 
			cordic_stage10(clk,reset,Istage10,Qstage10,PHstage10,16'd41,Istage11,Qstage11,PHstage11);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,11) 
			cordic_stage11(clk,reset,Istage11,Qstage11,PHstage11,16'd20,Istage12,Qstage12,PHstage12);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,12) 
			cordic_stage12(clk,reset,Istage12,Qstage12,PHstage12,16'd10,Istage13,Qstage13,PHstage13);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,13) 
			cordic_stage13(clk,reset,Istage13,Qstage13,PHstage13,16'd5,Istage14,Qstage14,PHstage14);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,14) 
			cordic_stage14(clk,reset,Istage14,Qstage14,PHstage14,16'd3,Istage15,Qstage15,PHstage15);
		cordic_stage #(CORDIC_WIDTH+2,PHASE_WIDTH-1,15) 
			cordic_stage15(clk,reset,Istage15,Qstage15,PHstage15,16'd1,Istage16,Qstage16,PHstage16);
		*/
		
			
		assign Iout = Istage12[CORDIC_WIDTH:1];
		assign Qout = Qstage12[CORDIC_WIDTH:1];
		assign PHout = PHstage12;				
endmodule