// ---------------------------------------------
//
// QS1R Receiver FPGA
// Copyright 2006 P.Covington N8VB
//
// Ver 1.0
//

module fpga_top(

input signed [15:0] INA,
input FX2CLK,
input IFCLK,
input CLKA,
input CLK4,
input CLK5,

output DDC0_CLK,
output DDC0_A_B,
output [15:0] DDC0_DIN,

input [15:0] DDC0_DOUT,
input DDC0_DVOUT,
input DDC0_IQOUT,
input DDC0_A_B_OUT,
input DDC0_DTACK_RDY,

output DDC1_CLK,
output DDC1_A_B,
output [15:0] DDC1_DIN,

input [15:0] DDC1_DOUT,
input DDC1_DVOUT,
input DDC1_IQOUT,
input DDC1_A_B_OUT,
input DDC1_DTACK_RDY,

input CS_FPGA_M_N,
inout SDI_M,
inout SDO_M,
input SCLK_M,
input CS_FPGA_N,
input FX2_SDI,
inout FX2_SDO,
input FX2_SCLK,

output SLCS,
input [2:0] FLAG,
output [1:0] FIFO_ADR,
output PKEND,
output SLOE,
output SLWR,
output SLRD,
output reg [15:0] FD

);

assign DDC0_CLK = FX2CLK;
//assign DDC0_CLK = clk_domain_A;
assign DDC0_A_B = 1'b1;
assign DDC0_DIN = 16'b0;

assign DDC1_CLK = FX2CLK;
//assign DDC1_CLK = clk_domain_A;
assign DDC1_A_B = 1'b1;
assign DDC1_DIN = 16'd0;

assign SDI_M = 1'bZ;
assign SDO_M = 1'bZ;

assign SLCS = 1'b1;

wire ireadfifoempty;
wire iwritefifofull;
wire qreadfifoempty;
wire qwritefifofull;

wire reset;
reg clk_enable;

reg [10:0]reset_count;

always @ (posedge CLKA) begin
	if (reset_count[10]) begin
		clk_enable <= 1'b1;		// clk_enable high to run
		end
	else begin
		clk_enable <= 0;
		reset_count <= reset_count + 1'b1;
	end
end 
	
reg signed [15:0] ADC;

//
// ADC Data Register
//
// We register the ADC data to prevent glitches
// and avoid noise problems
//

always @(posedge CLKA)
begin
	ADC <= INA[15:0]; // buffer the ADC data
end

//
// Derandomizer
//

wire signed [15:0] DR_ADC = ADC[0] ? {~ADC[15:1], ADC[0]} : ADC;  // de-RAND mode

//
// ADC DC Offset Correction
//

/*
wire [15:0] DC_ADC;
wire [15:0] dc_offset_read;

dc_offset_correct dc_offset_correct_inst(
.clk(clk_domain_A),
.clken(clk_enable),
.data_in(DR_ADC),
.data_out(DC_ADC),
.dc_level_out(dc_offset_read)
);
*/

//
// SPI Interface
//
		
wire [6:0] serial_address;
wire [31:0] serial_data;
wire serial_strobe;
wire [31:0] freqset_register;	// freq set is read/write
wire [31:0] dcoffset_register;	// dc offset correct register
	
// SPI communications module
SPI_REGS #(32) spi_regs(	.FX2_CLK(FX2CLK),
							.SI(FX2_SDI),
							.SO(FX2_SDO),
							.SCK(FX2_SCLK),
							.CS(CS_FPGA_N),
							.saddr(serial_address),
							.sdata(serial_data),
							.sstrobe(serial_strobe),
							.GPReg0(freqset_register), // readback for freqset addr: 0x41
							.GPReg1(dcoffset_register), // addr: 0x42
							.GPReg2(), // addr: 0x43
							.GPReg3(), // addr: 0x44
							.GPReg4(), // addr: 0x45
							.GPReg5(), // addr: 0x46
							.GPReg6(), // addr: 0x47
							.GPReg7()); // addr: 0x48

// 32 bit Freq Setting Register, Address = 0x01	
RegisterX #(7'h1, 32) freqsetreg(.CLK(FX2CLK),
								.STB(serial_strobe),
								.ADDR(serial_address),
								.IN(serial_data),
								.OUT(freqset_register),
								.CHANGED());
// 32 bit DC Correct Select Register, Address = 0x02	
RegisterX #(7'h2, 32) dcoffsetreg(.CLK(FX2CLK),
								.STB(serial_strobe),
								.ADDR(serial_address),
								.IN(serial_data),
								.OUT(dcoffset_register),
								.CHANGED());

								
//
// NCO section
//

wire [31:0] phase;
wire signed [19:0] i_cordic_out;
wire signed [19:0] q_cordic_out;	

//
// PHASE ACCUMULATOR
//

phase_accumulator rx_phase_accumulator(	.clk(CLKA),
										.reset(~clk_enable),
										.frequency(freqset_register),
										.random_in(32'h0),
										.phase_out(phase));
//										
// CORDIC
//

cordic rx_cordic(.clk(CLKA),
					.reset(~clk_enable),
					.Iin({{4{DR_ADC[15]}}, DR_ADC[15:0]}),
					.Qin(20'd0),
					.PHin(phase[31:12]),
					.Iout(i_cordic_out),
					.Qout(q_cordic_out),					
					.PHout());						



//
// Connection to DDC0
//


wire i_sel_strobe = clk_enable;
wire q_sel_strobe = clk_enable;
/*
reg [15:0] i_ddc0_out;
reg [15:0] q_ddc0_out;

assign DDC0_DIN =DR_ADC[15:0];
//assign DDC0_DIN = DC_ADC[15:0];
//assign DDC0_DIN = ADC[15:0];

always @(posedge clk_domain_A) begin
	if (DDC0_DVOUT == 1'b1) begin
		if (DDC0_IQOUT == 1'b1) begin
				i_ddc0_out <= DDC0_DOUT;
				i_sel_strobe <= 1'b1;
				q_sel_strobe <= 1'b0;
			end
			else begin
				q_ddc0_out <= DDC0_DOUT;
				i_sel_strobe <= 1'b0;
				q_sel_strobe <= 1'b1;
			end
		end
	else begin
		i_sel_strobe <= 1'b0;
		q_sel_strobe <= 1'b0;
	end	
end
*/

wire f1_clk_enable_I1 = i_sel_strobe;
wire f1_clk_enable_Q1 = q_sel_strobe;
wire signed [19:0] f1_Data_In_I1 = i_cordic_out[19:0];
wire signed [19:0] f1_Data_In_Q1 = q_cordic_out[19:0];
wire signed [95:0] f1_Data_Out_I1;
wire signed [95:0] f1_Data_Out_Q1;
wire i1_f1_strobe;
wire q1_f1_strobe;

CIC6_520 f1_inst1_I(
                .clk(CLKA),
                .clk_enable(f1_clk_enable_I1),
                .reset(~clk_enable),
                .filter_in(f1_Data_In_I1),
                .filter_out(f1_Data_Out_I1),
                .ce_out(i1_f1_strobe)
                );

CIC6_520 f1_inst1_Q(
                .clk(CLKA),
                .clk_enable(f1_clk_enable_Q1),
                .reset(~clk_enable),
                .filter_in(f1_Data_In_Q1),
                .filter_out(f1_Data_Out_Q1),
                .ce_out(q1_f1_strobe)
                );



/*

// CIC3_13 decimate by 13

wire f1_clk_enable_I1 = i_sel_strobe;
wire f1_clk_enable_Q1 = q_sel_strobe;
wire signed [19:0] f1_Data_In_I1 = i_cordic_out[19:0];
wire signed [19:0] f1_Data_In_Q1 = q_cordic_out[19:0];
wire signed [23:0] f1_Data_Out_I1;
wire signed [23:0] f1_Data_Out_Q1;
wire i1_f1_strobe;
wire q1_f1_strobe;

CIC3_13 f1_inst1_I(
                .clk(CLKA),
                .clk_enable(f1_clk_enable_I1),
                .reset(~clk_enable),
                .filter_in(f1_Data_In_I1),
                .filter_out(f1_Data_Out_I1),
                .ce_out(i1_f1_strobe)
                );

CIC3_13 f1_inst1_Q(
                .clk(CLKA),
                .clk_enable(f1_clk_enable_Q1),
                .reset(~clk_enable),
                .filter_in(f1_Data_In_Q1),
                .filter_out(f1_Data_Out_Q1),
                .ce_out(q1_f1_strobe)
                );


// CIC5_5 decimate by 5

wire f2_clk_enable_I1 = i1_f1_strobe;
wire f2_clk_enable_Q1 = q1_f1_strobe;
wire signed [23:0] f2_Data_In_I1 = f1_Data_Out_I1[23:0];
wire signed [23:0] f2_Data_In_Q1 = f1_Data_Out_Q1[23:0];
wire signed [23:0] f2_Data_Out_I1;
wire signed [23:0] f2_Data_Out_Q1;
wire i1_f2_strobe;
wire q1_f2_strobe;

CIC5_5 f2_inst1_I(
                .clk(CLKA),
                .clk_enable(f2_clk_enable_I1),
                .reset(~clk_enable),
                .filter_in(f2_Data_In_I1),
                .filter_out(f2_Data_Out_I1),
                .ce_out(i1_f2_strobe)
                );

CIC5_5 f2_inst1_Q(
                .clk(CLKA),
                .clk_enable(f2_clk_enable_Q1),
                .reset(~clk_enable),
                .filter_in(f2_Data_In_Q1),
                .filter_out(f2_Data_Out_Q1),
                .ce_out(q1_f2_strobe)
                );

// CIC5_4 decimate by 4

wire f3_clk_enable_I1 = i1_f2_strobe;
wire f3_clk_enable_Q1 = q1_f2_strobe;
wire signed [23:0] f3_Data_In_I1 = f2_Data_Out_I1[23:0];
wire signed [23:0] f3_Data_In_Q1 = f2_Data_Out_Q1[23:0];
wire signed [23:0] f3_Data_Out_I1;
wire signed [23:0] f3_Data_Out_Q1;
wire i1_f3_strobe;
wire q1_f3_strobe;

CIC5_4 f3_inst1_I(
                .clk(CLKA),
                .clk_enable(f3_clk_enable_I1),
                .reset(~clk_enable),
                .filter_in(f3_Data_In_I1),
                .filter_out(f3_Data_Out_I1),
                .ce_out(i1_f3_strobe)
                );

CIC5_4 f3_inst1_Q(
                .clk(CLKA),
                .clk_enable(f3_clk_enable_Q1),
                .reset(~clk_enable),
                .filter_in(f3_Data_In_Q1),
                .filter_out(f3_Data_Out_Q1),
                .ce_out(q1_f3_strobe)
                );



// HB decimate by 2

wire f4_clk_enable_I1 = i1_f3_strobe;
wire f4_clk_enable_Q1 = q1_f3_strobe;
wire signed [23:0] f4_Data_In_I1 = f1_Data_Out_I1[23:0];
wire signed [23:0] f4_Data_In_Q1 = f1_Data_Out_Q1[23:0];
wire signed [42:0] f4_Data_Out_I1;
wire signed [42:0] f4_Data_Out_Q1;
wire i1_f4_strobe;
wire q1_f4_strobe;

HB_2 f4_inst1_I(
                .clk(CLKA),
                .clk_enable(f4_clk_enable_I1),
                .reset(~clk_enable),
                .filter_in(f4_Data_In_I1),
                .filter_out(f4_Data_Out_I1),
                .ce_out(i1_f4_strobe)
                );

HB_2 f4_inst1_Q(
                .clk(CLKA),
                .clk_enable(f4_clk_enable_Q1),
                .reset(~clk_enable),
                .filter_in(f4_Data_In_Q1),
                .filter_out(f4_Data_Out_Q1),
                .ce_out(q1_f4_strobe)
                );

*/
	
//
// TX FIFOs
//
// The ADC is in the CLKA domain, the USB is in the IFCLK domain.
// The FIFOs take care of syncing the two clock domains.  There
// is a FIFO for the I data and another FIFO for Q data.
//

wire signed [31:0] i_fifo_in = f1_Data_Out_I1[95:64];  // in CLKA domain
wire signed [31:0] q_fifo_in = f1_Data_Out_Q1[95:64];  // in CLKA domain
wire i_fifo_in_strobe = i1_f1_strobe;
wire q_fifo_in_strobe = q1_f1_strobe;

wire [15:0] QWORD; // in IFCLK domain
wire [15:0] IWORD; // in IFCLK domain
wire [15:0] QWORDHI; // in IFCLK domain
wire [15:0] IWORDHI; // in IFCLK domain

reg txfifoclr;
	
// I FIFO		
tx_fifo tx_fifo_i(.aclr(txfifoclr), .wrclk(CLKA), 
		.rdreq(WRITE_FX2FIFO & (fx2st == 8'd4)), .rdclk(IFCLK), .wrreq(i_fifo_in_strobe),
		.data(i_fifo_in), .rdempty(ireadfifoempty), .wrempty(), 
		.wrfull(iwritefifofull),	.rdfull(), 
		.q({IWORDHI,IWORD}), .wrusedw(), .rdusedw());

// Q FIFO		
tx_fifo tx_fifo_q(.aclr(txfifoclr), .wrclk(CLKA), 
		.rdreq(WRITE_FX2FIFO & (fx2st == 8'd2)), .rdclk(IFCLK), .wrreq(q_fifo_in_strobe),
		.data(q_fifo_in), .rdempty(qreadfifoempty), .wrempty(), 
		.wrfull(qwritefifofull),	.rdfull(), 
		.q({QWORDHI,QWORD}), .wrusedw(), .rdusedw());

//		
// USB FIFO Interface (in IFCLK domain)
// uses EP6
//
// This just shoots interleaved I and Q data out EP6
// When the FIFOs get full, no more data is added until
// some of it is consumed by USB.
//
				
reg WRITE_FX2FIFO;

assign SLRD = 1'b1; // never read
assign SLOE = 1'b1; // output enabled
assign FIFO_ADR = 2'b10; // always selects EP6
assign PKEND = 1'b0;  // not using short packets

wire FIFO_FULL = ~FLAG[1]; // FLAGs are active low
wire FIFO_EMPTY = ~FLAG[2];

assign SLWR = ~WRITE_FX2FIFO; // SLWR is active low

wire usb_write = (~ireadfifoempty) & (~FIFO_FULL);

reg [7:0] fx2st; // register for the USB TX state machine

// USB state machine (IFCLK domain)					
always @(posedge IFCLK)
begin
	case (fx2st)
	8'd0: begin
		txfifoclr <= 1'b1;
		WRITE_FX2FIFO <= 1'b0;
		fx2st <= 8'd1;
		end
	8'd1: begin
		txfifoclr <= 1'b0;
		fx2st <= 8'd2;
		end
	8'd2: begin
		if (usb_write) begin
				WRITE_FX2FIFO <= 1'b1;
				FD <= IWORD; // I LOWORD
				fx2st <= 8'd3;
			end
		else begin
			WRITE_FX2FIFO <= 1'b0;
			end
		end
	8'd3: begin
			FD <= IWORDHI; // I HIWORD
			fx2st <= 8'd4;
			end	
	8'd4: begin
			FD <= QWORD; // Q LOWORD
			fx2st <= 8'd5;
			end	
	8'd5: begin
			FD <= QWORDHI; // Q HIWORD
			fx2st <= 8'd2;
			end	
	default: begin
		fx2st <= 8'd0;
		end
	endcase			
end
	
endmodule