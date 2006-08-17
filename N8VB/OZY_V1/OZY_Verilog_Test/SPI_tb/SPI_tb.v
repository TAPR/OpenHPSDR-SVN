module SPI_tb(SCLK, SI, SO, CS);

input SCLK;
input SI;
output SO;
input CS;

wire [7:0] reg0;
wire [7:0] reg1;

SPI_REGS SPI_REGS(.SI(SI),
					.SO(SO),
					.SCK(SCLK),
					.CS(CS),
					.GPReg0(reg0),
					.GPReg1(reg1),
					.GPReg2(),
					.GPReg3(),
					.GPReg4(),
					.GPReg5(),
					.GPReg6(),
					.GPReg7(),
					.MPM(8'b00001111));

endmodule

