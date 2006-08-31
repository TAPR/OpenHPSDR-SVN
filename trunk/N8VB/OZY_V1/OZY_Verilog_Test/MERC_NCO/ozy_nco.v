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
					EVALPIN39
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
	
	reg txfifoclr;
	reg writefifo;
	wire readfifoempty;
	wire readfifofull;
	wire writefifoempty;
	wire writefifofull;
	
	assign EVALPIN39 = 1'b1; // VCC to eval board
			
	wire reset;
	reg clk_enable;

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
	wire [15:0] i_out;
	
	reg adc_go;
	
	//parameter FREQ = 32'hF5C28F5; // (6 MHz/100 MHz)*(2^32)
	//parameter FREQ = 32'h4CCCCCCC; // 30 MHz
	//parameter FREQ = 32'h19999999; // 10 MHz
	parameter FREQ = 32'h15;		// 0.5 Hz
	
	wire [31:0] phase;
	
	phase_accumulator rx_phase_accumulator(	.clk(ENC_CLK),
											.reset(~clk_enable),
											.frequency(FREQ),
											.phase_out(phase));
	
	cordic rx_cordic(	.clk(ENC_CLK),
						.reset(~clk_enable),
						.Iin(ADC),
						.Qin(16'h0),
						.PHin(phase[31:16]),
						.Iout(i_out),
						.Qout(),
						.PHout());
		
	always @(posedge ENC_CLK)
	begin
		if (~clk_enable) begin
			adc_go <= 1'b0;
			end
		ADC <= DA[15:0]; // buffer the ADC data
		if (readfifoempty) begin
			adc_go <= 1'b1;
			end
		if (writefifofull) begin
			adc_go <= 1'b0;
			end			
	end
		
	reg [2:0] adcst;

	assign GPIO1 = ~writefifoempty;
	assign GPIO2 = ~writefifofull;
	assign GPIO3 = ~readfifoempty;
	assign GPIO4 = ~readfifofull;
	assign GPIO5 = FLAGA;					
	assign GPIO6 = FLAGB;
	assign GPIO7 = FLAGC;
	
	wire [9:0] read_used;
	
	tx_fifo tx_fifo(.aclr(txfifoclr), .wrclk(ENC_CLK), 
			.rdreq(WRITE_FX2FIFO), .rdclk(IFCLK), .wrreq(adc_go),
			.data(i_out), .rdempty(readfifoempty), .wrempty(writefifoempty), 
			.wrfull(writefifofull),	.rdfull(readfifofull), 
			.q(FD), .wrusedw(), .rdusedw(read_used));
				
	reg [3:0] fx2st;
	reg WRITE_FX2FIFO;
	
	assign SLRD = 1'b1; // never write
	assign SLOE = 1'b1;
	assign FIFO_ADR = 2'b10; // always select EP6
	
	wire FIFO_FULL = ~FLAGB; // FLAGs are active low
	wire FIFO_EMPTY = ~FLAGC;
	wire SLWR = ~WRITE_FX2FIFO; // SLWR is active low
	
	wire [15:0] FD; // FX2 FIFO DATA
	
	assign usb_write = (~readfifoempty) & (~FIFO_FULL);
						
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
				end
			else begin
				WRITE_FX2FIFO <= 1'b0;
				end
			end		
		default: begin
			fx2st <= 4'd0;
			end
		endcase			
	end
	
endmodule