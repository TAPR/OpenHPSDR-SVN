// HPSDR CORDIC
// P. Covington N8VB
// CORDIC
//
// Inputs:
//	clk - clock 
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
	
	output [CORDIC_WIDTH-1:0] Iout;
	output [CORDIC_WIDTH-1:0] Qout;
	output [PHASE_WIDTH-1:0] PHout;
	
	always @(posedge clk)
		if(reset)
			begin
				Istage0 <= #1 0;
				Qstage0 <= #1 0;
				Pstage0 <= #1 0;
			end
		else
			begin
				Pstage0 <= #1 PHin[PHASE_WIDTH-2:0];
				case (PHi[PHASE_WIDTH-1:PHASE_WIDTH-2])
					2'b00, 2'b11:
						begin
							Istage0 <= #1 Iin_Extend;
							Qstage0 <= #1 Qin_Extend;


endmodule