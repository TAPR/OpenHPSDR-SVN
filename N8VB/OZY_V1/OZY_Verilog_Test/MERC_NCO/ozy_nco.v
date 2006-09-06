module ozy_nco(	ENC_CLK,
				 	FX2_CLK,
					IFCLK,
					DA,
					FD,
					SLWR,
					SLRD,
					SLOE,
					FIFO_ADR,
					FLAGA,					
					FLAGB,
					FLAGC,
					GPIO1,
					GPIO2,
					GPIO3,
					GPIO4,
					GPIO5,
					GPIO6,
					GPIO7,
					EVALPIN39,
					SCK, 
					SI, 
					SO, 
					CS
					 );

	input ENC_CLK;
	input FX2_CLK;
	input IFCLK;
	input [15:0] DA;
	output [15:0] FD;
	output SLWR;
	output SLRD;
	output SLOE;
	output [1:0] FIFO_ADR;
	input FLAGA;
	input FLAGB;
	input FLAGC;
	
	output GPIO1;
	output GPIO2;
	output GPIO3;
	output GPIO4;
	output GPIO5;
	output GPIO6;
	output GPIO7;
	output EVALPIN39;
	
	input SCK;		// SPI SCK from FX2
 	input SI;		// SPI serial in from FX2
 	inout SO;		// SPI serial out to FX2
 	input CS;       // FPGA chip select from FX2
	
	reg txfifoclr;
	reg writefifo;
	wire ireadfifoempty;
	wire iwritefifofull;
	wire qreadfifoempty;
	wire qwritefifofull;
	
	assign EVALPIN39 = 1'b1; // VCC to eval board
	
	assign GPIO1 = ~iwritefifofull;
	assign GPIO2 = ~qwritefifofull;
	assign GPIO3 = ~ireadfifoempty;
	assign GPIO4 = ~qreadfifoempty;
	assign GPIO5 = FLAGA;					
	assign GPIO6 = FLAGB;
	assign GPIO7 = FLAGC;
			
	wire reset;
	reg clk_enable;

	// reset logic ~20.48uS
	reg [10:0]reset_count;

	always @ (posedge ENC_CLK) begin
		if (reset_count[10]) begin
			clk_enable <= 1'b1;		// clk_enable high to run
			end
		else begin
			clk_enable <= 0;
			reset_count <= reset_count + 1'b1;
		end
	end 
		
	reg [15:0] ADC;
	reg adc_go;
	
	// ADC I read
	always @(posedge ENC_CLK)
	begin
		if (~clk_enable) begin
			adc_go <= 1'b0;
			end
		ADC <= DA[15:0]; // buffer the ADC data
		if (ireadfifoempty) begin
			adc_go <= 1'b1;
			end
		if (iwritefifofull) begin
			adc_go <= 1'b0;
			end			
	end
			
	wire [15:0] i_out;
	wire [15:0] q_out;
	
	`define FREQSETADRREG 7'h1 // freq setting register address = 0x01
	
	wire [6:0] serial_address;
 	wire [7:0] serial_data;
 	wire	   serial_strobe;
	wire [31:0] freqset_register;	// freq set is read/write
	
	// SPI communications module
 	SPI_REGS #(8) spi_regs(	.FX2_CLK(FX2_CLK),
								.SI(SI),
								.SO(SO),
								.SCK(SCK),
								.CS(CS),
								.saddr(serial_address),
								.sdata(serial_data),
								.sstrobe(serial_strobe),
								.GPReg0(freqset_register), // readback for freqset
								.GPReg1(), 
								.GPReg2(), 
								.GPReg3(),
								.GPReg4(),
								.GPReg5(),
								.GPReg6(),
								.GPReg7());
	
	// Freq Setting Register, Address = 0x01	
 	RegisterX #(`FREQSETADRREG, 8) freqsetreg(.CLK(FX2_CLK),
									.STB(serial_strobe),
									.ADDR(serial_address),
									.IN(serial_data),
									.OUT(freqset_register),
									.CHANGED());
													
	//parameter FREQ = 32'hF5C28F5; // (6 MHz/100 MHz)*(2^32)
	//parameter FREQ = 32'hB3333333; // 70 MHz
	//parameter FREQ = 32'h4CCCCCCC; // 30 MHz
	//parameter FREQ = 32'h33333333; // 20 MHz
	//parameter FREQ = 32'h19999999; // 10 MHz
	//parameter FREQ = 32'hCCCCCCC; // 5 MHz
	//parameter FREQ = 32'h1;		// slow!!!
	
	wire [31:0] phase;
	
	phase_accumulator rx_phase_accumulator(	.clk(ENC_CLK),
											.reset(~clk_enable),
											.frequency({freqset_register, 24'b0}),
											.phase_out(phase));
		
	
	cordic rx_cordic(	.clk(ENC_CLK),
						.reset(~clk_enable),
						.Iin(ADC),
						.Qin(16'h0),
						.PHin(phase[31:16]),
						.Iout(i_out),
						.Qout(q_out),
						.PHout());
			
	tx_fifo tx_fifo_i(.aclr(txfifoclr), .wrclk(ENC_CLK), 
			.rdreq(WRITE_FX2FIFO & (fx2st == 4'd2)), .rdclk(IFCLK), .wrreq(adc_go),
			.data(i_out), .rdempty(ireadfifoempty), .wrempty(), 
			.wrfull(iwritefifofull),	.rdfull(), 
			.q(IWORD), .wrusedw(), .rdusedw());
			
	tx_fifo tx_fifo_q(.aclr(txfifoclr), .wrclk(ENC_CLK), 
			.rdreq(WRITE_FX2FIFO & (fx2st == 4'd3)), .rdclk(IFCLK), .wrreq(adc_go),
			.data(q_out), .rdempty(qreadfifoempty), .wrempty(), 
			.wrfull(qwritefifofull),	.rdfull(), 
			.q(QWORD), .wrusedw(), .rdusedw());
	
	
	// Write to EP6
				
	reg [3:0] fx2st;
	reg WRITE_FX2FIFO;
	
	assign SLRD = 1'b1; // never write
	assign SLOE = 1'b1;
	assign FIFO_ADR = 2'b10; // always select EP6
	
	wire FIFO_FULL = ~FLAGB; // FLAGs are active low
	wire FIFO_EMPTY = ~FLAGC;
	wire SLWR = ~WRITE_FX2FIFO; // SLWR is active low
	
	wire [15:0] QWORD;
	wire [15:0] IWORD;
			
	reg [15:0] FD; // FX2 FIFO DATA
	
	wire usb_write;
			
	assign usb_write = (~ireadfifoempty) & (~FIFO_FULL);
						
	always @(posedge IFCLK)
	begin
		case (fx2st)
		4'd0: begin
			txfifoclr <= 1'b1;
			WRITE_FX2FIFO <= 1'b0;
			fx2st <= 4'd1;
			end
		4'd1: begin
			txfifoclr <= 1'b0;
			fx2st <= 4'd2;
			end
		4'd2: begin
			if (usb_write) begin
					WRITE_FX2FIFO <= 1'b1;
					FD <= IWORD;
					fx2st <= 4'd3;
				end
			else begin
				WRITE_FX2FIFO <= 1'b0;
				end
			end
		4'd3: begin
				WRITE_FX2FIFO <= 1'b1;
				FD <= QWORD;
				fx2st <= 4'd2;
				end
		default: begin
			fx2st <= 4'd0;
			end
		endcase			
	end
	
endmodule