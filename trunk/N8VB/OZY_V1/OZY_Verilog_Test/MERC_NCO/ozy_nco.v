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
			
	// SPI
		
	wire [6:0] serial_address;
 	wire [31:0] serial_data;
 	wire	   serial_strobe;
	wire [31:0] freqset_register;	// freq set is read/write
	wire [31:0] option_sel_register;
		
	// SPI communications module
 	SPI_REGS #(32) spi_regs(	.FX2_CLK(FX2_CLK),
								.SI(SI),
								.SO(SO),
								.SCK(SCK),
								.CS(CS),
								.saddr(serial_address),
								.sdata(serial_data),
								.sstrobe(serial_strobe),
								.GPReg0(freqset_register), // readback for freqset
								.GPReg1(option_sel_register), 
								.GPReg2(), 
								.GPReg3(),
								.GPReg4(),
								.GPReg5(),
								.GPReg6(),
								.GPReg7());
	
	// Freq Setting Register, Address = 0x01	
 	RegisterX #(7'h1, 32) freqsetreg(.CLK(FX2_CLK),
									.STB(serial_strobe),
									.ADDR(serial_address),
									.IN(serial_data),
									.OUT(freqset_register),
									.CHANGED());
	
	// Freq Setting Register, Address = 0x02	
 	RegisterX #(7'h2, 32) optionreg(.CLK(FX2_CLK),
									.STB(serial_strobe),
									.ADDR(serial_address),
									.IN(serial_data),
									.OUT(option_sel_register),
									.CHANGED());
																						
	//parameter FREQ = 32'hF5C28F5; // (6 MHz/100 MHz)*(2^32)
	//parameter FREQ = 32'hB3333333; // 70 MHz
	//parameter FREQ = 32'h4CCCCCCC; // 30 MHz
	//parameter FREQ = 32'h33333333; // 20 MHz
	//parameter FREQ = 32'h19999999; // 10 MHz
	//parameter FREQ = 32'hCCCCCCC; // 5 MHz
	//parameter FREQ = 32'h1;		// slow!!!
	
	wire [31:0] phase;
	
	//wire [31:0] rand_num;	
	
	/*
	// used for phase dithering	
	rng random(	.clk(ENC_CLK),
				.reset(1'b1),
				.loadseed_i(~clk_enable),
				.seed_i(32'hF0FF0FFF),
				.number_o(rand_num));		
	
					
	phase_accumulator rx_phase_accumulator(	.clk(ENC_CLK),
											.reset(~clk_enable),
											.frequency(freqset_register),
											.random_in({20'h0,rand_num[11:0]}),
											.phase_out(phase));
	*/
	
	wire [15:0] i_cordic_out;
	wire [15:0] q_cordic_out;		
		
	phase_accumulator rx_phase_accumulator(	.clk(ENC_CLK),
											.reset(~clk_enable),
											.frequency(freqset_register),
											.random_in(32'h0),
											.phase_out(phase));
											
	
	cordic rx_cordic(	.clk(ENC_CLK),
						.reset(~clk_enable),
						.Iin(ADC[15:0]),
						.Qin(16'h0),
						.PHin(phase[31:16]),
						.Iout(i_cordic_out),
						.Qout(q_cordic_out),
						.PHout());	
					
	wire i_strobe1;
	wire q_strobe1;
	wire i_strobe2;
	wire q_strobe2;
	wire i_strobe3;
	wire q_strobe3;
	
	wire [15:0] i_cic_out1;
	wire [15:0] q_cic_out1;
	//wire [19:0] i_cic_out2;
	//wire [19:0] q_cic_out2;
	wire [19:0] i_hb_out;
	wire [19:0] q_hb_out;
	
	/*	
	CIC_D65 I_CAS1(	.clk(ENC_CLK),
			                .clk_enable(clk_enable),
			                .reset(~clk_enable),
			                .filter_in(i_cordic_out),
			                .filter_out(i_cic_out1),
			                .ce_out(i_strobe1)
			                );

	CIC_D65 Q_CAS1(	.clk(ENC_CLK),
			                .clk_enable(clk_enable),
			                .reset(~clk_enable),
			                .filter_in(q_cordic_out),
			                .filter_out(q_cic_out1),
			                .ce_out(q_strobe1)
			                );	
	
	
	CIC_D4 I_CAS2(	.clk(ENC_CLK),
			                .clk_enable(i_strobe1),
			                .reset(~clk_enable),
			                .filter_in(i_cic_out1[19:4]),
			                .filter_out(i_cic_out2),
			                .ce_out(i_strobe2)
			                );

	CIC_D4 Q_CAS2(	.clk(ENC_CLK),
			                .clk_enable(q_strobe1),
			                .reset(~clk_enable),
			                .filter_in(q_cic_out1[19:4]),
			                .filter_out(q_cic_out2),
			                .ce_out(q_strobe2)
			                );
			
	HB I_HB(
            .clk(ENC_CLK),
            .clk_enable(i_strobe2),
            .reset(~clk_enable),
            .filter_in(i_cic_out2[19:4]),
            .filter_out(i_hb_out),
            .ce_out(i_strobe3)
            );	

	HB Q_HB(
            .clk(ENC_CLK),
            .clk_enable(q_strobe2),
            .reset(~clk_enable),
            .filter_in(q_cic_out2[19:4]),
            .filter_out(q_hb_out),
            .ce_out(q_strobe3)
            );	
	
	*/
	
	cic_decim cic_i ( 	.clock(ENC_CLK),
						.reset(~clk_enable),
						.enable(clk_enable),
						.rate(8'b1000000),
						.strobe_in(1'b1),
						.strobe_out(i_strobe1),
						.signal_in(i_cordic_out),
						.signal_out(i_cic_out1));
						
	cic_decim cic_q ( 	.clock(ENC_CLK),
						.reset(~clk_enable),
						.enable(clk_enable),
						.rate(8'b1000000),
						.strobe_in(1'b1),
						.strobe_out(q_strobe1),
						.signal_in(q_cordic_out),
						.signal_out(q_cic_out1));
						
	halfband_decim hb_i  (	.clock(ENC_CLK),
							.reset(~clk_enable),
							.enable(clk_enable), 
							.strobe_in(i_strobe1), 
							.strobe_out(i_strobe2),
   							.data_in(i_cic_out1), 
							.data_out(i_hb_out),
							.debugctrl());
							
	halfband_decim hb_q  (	.clock(ENC_CLK),
							.reset(~clk_enable),
							.enable(clk_enable), 
							.strobe_in(q_strobe1), 
							.strobe_out(q_strobe2),
   							.data_in(q_cic_out1), 
							.data_out(q_hb_out),
							.debugctrl());
		
	wire [15:0] i_fifo_out;
	wire [15:0] q_fifo_out;
					
	wire i_sel_strobe;
	wire q_sel_strobe;
	
	RXMUX I_MUX (
				.clock(ENC_CLK),
				.data0x(ADC),
				.data1x(i_cordic_out),
				.data2x(i_cic_out1),
				.data3x(i_hb_out),
				.sel(option_sel_register[1:0]),
				.result(i_fifo_out));
				
	RXMUX Q_MUX (
				.clock(ENC_CLK),
				.data0x(16'h0),
				.data1x(q_cordic_out),
				.data2x(q_cic_out1),
				.data3x(q_hb_out),
				.sel(option_sel_register[1:0]),
				.result(q_fifo_out));
	
	assign i_sel_strobe = (option_sel_register[1:0] == 2'b10) ? i_strobe1 : 1'b1;
	assign q_sel_strobe = (option_sel_register[1:0] == 2'b10) ? q_strobe1 : 1'b1;
	
	// I FIFO		
	tx_fifo tx_fifo_i(.aclr(txfifoclr), .wrclk(ENC_CLK), 
			.rdreq(WRITE_FX2FIFO & (fx2st == 4'd2)), .rdclk(IFCLK), .wrreq(adc_go & i_sel_strobe),
			.data(i_fifo_out), .rdempty(ireadfifoempty), .wrempty(), 
			.wrfull(iwritefifofull),	.rdfull(), 
			.q(IWORD), .wrusedw(), .rdusedw());
	
	// Q FIFO		
	tx_fifo tx_fifo_q(.aclr(txfifoclr), .wrclk(ENC_CLK), 
			.rdreq(WRITE_FX2FIFO & (fx2st == 4'd3)), .rdclk(IFCLK), .wrreq(adc_go & q_sel_strobe),
			.data(q_fifo_out), .rdempty(qreadfifoempty), .wrempty(), 
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
	
	// USB 					
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
					FD <= QWORD; // changed order 9/8/2006 was IWORD
					fx2st <= 4'd3;
				end
			else begin
				WRITE_FX2FIFO <= 1'b0;
				end
			end
		4'd3: begin
				FD <= IWORD; // changed order 9/8/2006
				fx2st <= 4'd2;
				end
		default: begin
			fx2st <= 4'd0;
			end
		endcase			
	end
	
endmodule