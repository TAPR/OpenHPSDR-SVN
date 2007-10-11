/***********************************************************
*
*	Test program for LT2208
*
************************************************************/

// (C) Phil Harman VK6APH 2006,2007


/* 	This program interfaces the LT2208 to PowerSDR over USB.
	The data from the LT2208 is in 16 bit parallel format and 
	is valid at the positive edge of the LT2208 100MHz clock.
	
	The data is processed by a CORDIC NCO to produce I and Q
	outputs.  These are decimated by 320 in a CIC filter then by 2 
	in an  CFIR filter to give output data at ~195kHz to feed
	the Atlas bus in I2S format for tranfer via Ozy to PowerSDR.
	
	Change log:
	
	29 Sept 2007 - Start as basis of 20 pin version with Ozy
	30 Sept 2007 - Working with Penelope
	30 Sept 2007 - Testing my CORDIC
		
	
*/
	
module Mercury(ADC, DEBUG_LED0,DEBUG_LED1, DEBUG_LED2, DEBUG_LED3, C5, FPGA_CLK6IN,
				CC, DOUT, LROUT, CBCLK, CLRCLK, CLK_MCLK, BCLK, LRCLK, FPGA_CLKXOUT);

input [15:0]ADC;			// samples from LT2208
input FPGA_CLK6IN;			// 125MHz clock from LT2208
output DEBUG_LED0;			// LEDs on OZY board
output DEBUG_LED1;
output DEBUG_LED2;
output DEBUG_LED3;
output C5;					// PCLK_12MHZ out to C5 on Atlas bus
input  CC;					// C & C from Atlas C20
output DOUT;				// I and Q out to Atlas bus on C10
input  LROUT;				// Left and Right Rx audio in from Atlas C4
input  CBCLK; 				// ~3MHz BCLK from Atlas C8
input  CLRCLK; 				// 48.8KHz L/R clock from Atlas C9
input  CLK_MCLK; 			// 12.5MHz master clock from Atlas C17
input  BCLK;				// for 192kHz sampling rate
input  LRCLK;				// for 192kHz sampling rate
output FPGA_CLKXOUT;		// 125MHz clock out to LVDS

reg data_ready;				// set at end of decimation


////////////////////////////////////////////////////////////////////////
//
//		Reset and enable code
//
////////////////////////////////////////////////////////////////////////

// holds clk_enable = 0 for first 1024 clock counts


reg clk_enable;

reg [10:0]reset_count;

always @ (posedge clock) begin
	if (reset_count[10]) begin
		clk_enable <= 1'b1;		// clk_enable high to run
		end
	else begin
		clk_enable <= 0;
		reset_count <= reset_count + 1'b1;
	end
end 



// A Digital Output Randomizer is fitted to the LT2208. This complements bits 15 to 1 if 
// bit 0 is 1. This helps to reduce any pickup by the A/D input of the digital outputs. 
// We need to de-ramdomize the LT2208 data if this is turned on. 

always @ (posedge clock)
begin 
	if (ADC[0]) temp_ADC <= {~ADC[15:1],ADC[0]};
	else temp_ADC <= ADC;
end 

//////////////////////////////////////////////////////////////
//
//		CLOCKS
//
//////////////////////////////////////////////////////////////

// 125MHz clock from LT2208

wire clock;
assign clock = FPGA_CLK6IN;

// Generate 125MHz/10 PCLK_12MHZ for Atlas bus

reg PCLK_12MHZ;
reg [2:0]PCLK_count;
always @ (posedge clock)
begin
	if (PCLK_count == 4)  // divide 125MHz clock by 10 to give 12.5MHz
		begin
		PCLK_12MHZ <= ~PCLK_12MHZ;
		PCLK_count <= 0;
		end
	else PCLK_count <= PCLK_count + 1'b1;
end
		
// temp code *** send PCLK_12MHZ to C5

assign C5 = PCLK_12MHZ;

// send 125MHz clock out to LVDS

assign FPGA_CLKXOUT = clock; // appears on J3
		
//////////////////////////////////////////////////////////////
//
//		Convert frequency to phase word 
//
//////////////////////////////////////////////////////////////

/*	
	Calculates  (frequency * 2^32) /125e6
	Each calculation takes ~ 0.6uS @ 125MHz
	This method is quite fast enough and uses much lower LEs than a Megafunction

*/

wire [31:0]freq;
wire ready;
always @ (posedge ready)		// strobe frequecy when ready is set
begin
	frequency <= frequency_HZ;	// frequecy_HZ is current frequency in Hz e.g. 14,195,000Hz
end 

division division_DDS(.quotient(freq),.ready(ready),.dividend(frequency),.divider(32'd125000000),.clk(clock));

// sync frequecy change to 125MHz clock
reg [31:0]sync_frequency;
always @ (posedge clock)
begin
	sync_frequency <= freq;
end


//////////////////////////////////////////////////////////////
//
//		CORDIC NCO 
//
//////////////////////////////////////////////////////////////

//Code rotates A/D input at set frequency  and produces I & Q 


wire [17:0]i_out;
wire [17:0]q_out;
reg  [15:0]temp_ADC;
wire[31:0] phase;
reg [31:0]frequency;
	
//assign frequency = 32'h1D0FA58F; // 14.190MHz

// The phase accumulator takes a 32 bit frequency dword and outputs a 32 bit phase dword on each clock
phase_accumulator rx_phase_accumulator(.clk(~clock),.reset(~clk_enable),.frequency(sync_frequency),.phase_out(phase));
//phase_accumulator rx_phase_accumulator(.clk(clock),.reset(~clk_enable),.frequency(32'h1D0FA58F),.phase_out(phase));


//cordic_16  cordic (.in(temp_ADC),.iout(i_out),.qout(q_out),.ain(phase[31:12]),.clk(clock));

// set i_in to 0 for real inputs 
cordic_VK6APH cordic (.i_in(16'd0), .q_in(temp_ADC), .iout(i_out), .qout(q_out), .ain(phase[31:12]), .clk(clock));


///////////////////////////////////////////////////////////////
//
//		CIC Filters - designed using MatLab
//
///////////////////////////////////////////////////////////////

wire signed [23:0]cic_out_i;
wire signed [23:0]cic_out_q;
wire signed [23:0]cic_out_i_1;
wire signed [23:0]cic_out_q_1;
wire signed [23:0]cic_out_i_2;
wire signed [23:0]cic_out_q_2;
wire ce_out_i_1;				// narrow pulse when data available
wire ce_out_q_1;				// narrow pulse when data available
wire ce_out_i_2;				// narrow pulse when data available
wire ce_out_q_2;				// narrow pulse when data available
wire ce_out_i;					// narrow pulse when data available
wire ce_out_q;					// narrow pulse when data available

//first CIC - decimate by 10, 3rd order - clock out is 12.5MHz
cic_10_1_3_nofraction cic_I_1( .clk(clock),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(i_out),.filter_out(cic_out_i_1),.ce_out(ce_out_i_1));
cic_10_1_3_nofraction cic_Q_1( .clk(clock),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(q_out),.filter_out(cic_out_q_1),.ce_out(ce_out_q_1));

//second CIC - decimate by 8, 5th order - clock out is 1.5625MHz
cic_8_1_5_nofraction cic_I_2( .clk(ce_out_i_1),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(cic_out_i_1),.filter_out(cic_out_i_2),.ce_out(ce_out_i_2));
cic_8_1_5_nofraction cic_Q_2( .clk(ce_out_q_1),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(cic_out_q_1),.filter_out(cic_out_q_2),.ce_out(ce_out_q_2));

//third CIC - decimate by 4, 8th order - clock out is 390.625kHz
cic_4_1_8_nofraction cic_I_3( .clk(ce_out_i_2),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(cic_out_i_2),.filter_out(cic_out_i),.ce_out(ce_out_i));
cic_4_1_8_nofraction cic_Q_3( .clk(ce_out_q_2),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(cic_out_q_2),.filter_out(cic_out_q),.ce_out(ce_out_q));

//////////////////////////////////////////////////////////////////////
//
//		CFIR 
//
//////////////////////////////////////////////////////////////////////

// compensates for sinx/x response of CIC, provides ultimage rejection and decimates by 2
// output data rate is 195.3125kHz

wire signed [47:0]FIR_i_out;
wire signed [47:0]FIR_q_out;
wire FIR_strobe;

FIR_top  FIR(.clk(clock),.reset(~clk_enable),.data_in_I(cic_out_i),.data_in_Q(cic_out_q),.strobe_in(ce_out_i ),.data_out_I(FIR_i_out),.data_out_Q(FIR_q_out),.strobe_out(FIR_strobe));

//////////////////////////////////////////////////////////////////////
//
//		Remove DC offset from CFIR output 
//
//////////////////////////////////////////////////////////////////////

wire signed [23:0]i_no_dc;
wire signed [23:0]q_no_dc;
wire signed [23:0]dc_in_i;
wire signed [23:0]dc_in_q;

assign dc_in_i = FIR_i_out[47:24];
assign dc_in_q = FIR_q_out[47:24];

dc_offset_correct_new dc_offset_correct_i(.clk(~FIR_strobe),.clken(clk_enable),.data_in(dc_in_i),.data_out(i_no_dc),.dc_level_out());
dc_offset_correct_new dc_offset_correct_q(.clk(~FIR_strobe),.clken(clk_enable),.data_in(dc_in_q),.data_out(q_no_dc),.dc_level_out());

//assign i_no_dc = FIR_i_out[47:24];
//assign q_no_dc = FIR_q_out[47:24];


// Sync dc correct output to neg edge of BCLK

reg signed [23:0]i;
reg signed [23:0]q;

always @ (negedge BCLK)
begin
	i <= i_no_dc;
	q <= q_no_dc;
end 

// I2S encoder to send I and Q data to Atlas on C10

I2SEncode  I2S(.LRCLK(LRCLK), .BCLK(BCLK), .left_sample(i), .right_sample(q), .outbit(DOUT)); 


///////////////////////////////////////////////////////////
//
//    Command and Control Decoder 
//
///////////////////////////////////////////////////////////

/*

	The C&C encoder in Ozy broadcasts data over the Atlas bus (C20) for
	use by other cards e.g. Mercury and Penelope.  The data is in 
	I2S format with the clock being CBLCK and the start of each frame
	being indicated using the negative edge of CLRCLK.
	
	The data format is as follows:
	
	<[48]PTT><[47:44]address><[43:12]frequency><[11:8]clock_select><[7:1]OC><[0]Mode> 
	
	for a total of 49 bits. Frequency is in Hz and 32 bit binary format and 
	OC is the open collector data on Penelope.
	
	The clock source decodes as follows:
	
	0x00  = 10MHz reference from Atlas bus ie Gibraltar
	0x01  = 10MHz reference from Penelope
	0x10  = 10MHz reference from Mercury
	00xx  = 125MHz source from Penelope 
	01xx  = 125MHz source from Mercury 
	
*/

reg [5:0] bits;     // how many bits clocked 
reg [1:0]CC_state;
reg [48:0] CCdata;	// 49 bits of C&C data

always @(posedge CBCLK)  
begin
case(CC_state)
0:	begin
	if (CLRCLK == 0)CC_state <= 0;			// loop until CLRLCK is high   
	else CC_state <= 1;
	end
1:	begin
		if (CLRCLK)	CC_state <= 1;			// loop until CLRCLK is low  
		else begin
		bits <= 6'd48;						
		CC_state <= 2;
		end
	end
2:	begin
	CCdata[bits] <= CC;						// this is the second CBCLK after negedge of CLRCLK
		if (bits == 0)CC_state <= 0; 		// done so restart
		else begin
		bits <= bits - 1'b1;
		CC_state <= 2;  
		end
	end
default: CC_state <= 0;
endcase
end


// decode C & C data into variables and sync to 48kHz LR clock



//reg [3:0]Address;		// Address in C&C header, set to 0 for now
reg [31:0]frequency_HZ;	// frequency control bits for CORDIC
reg [3:0]clock_select;	// 10MHz and 125MHz clock selection
//reg [6:0]OC;			// Open Collector outputs data
//wire ref_ext;			// Set when internal 10MHz reference sent to Atlas C16
//wire source_125MHZ;		// Set when internal 125MHz source is used and sent to LVDS

always @ (negedge CLRCLK)  
begin 
	//Address <= CCdata[47:44];
	// check address match here in the future - ignore for now
	frequency_HZ <= CCdata[43:12];
	clock_select <= CCdata[11:8];     
	//OC <= CCdata[7:1];
end

//assign ref_ext = clock_select[0] ? 1'b1 :1'b0; // if set use internally and send to C16 else get from C16
//assign source_125MHZ = clock_select[2] ? 1'b0 : 1'b1; // if set use internally and send to LVDS else
													  	// get from LVDS 
///////////////////////////////////////////////////////////
//
//    PLL 
//
///////////////////////////////////////////////////////////

/* 
	Divide the 10MHz reference and 125MHz clock to give 2.5MHz signals.
	Apply these to an EXOR phase detector. If the 10MHz reference is not
	present the EXOR output will be a 2.5MHz square wave. When passed through 
	the loop filter this will provide a dc level of (3.3/2)v which will
	set the 125MHz VCXO to its nominal frequency.
	The selection of the internal or external 10MHz reference for the PLL
	is made using  ext_ref.
	
*/

/*

// divide 10MHz reference clock by 4 to give 2.5MHz

reg [2:0]ref_count;
reg ref_2_5M;
reg osc_2_5M;

always @ (posedge reference)
begin
	if (ref_count == 1)
	begin
		ref_2_5M <= ~ref_2_5M;
		ref_count <= 0;
	end
	else ref_count <= ref_count + 1'b1;
end

// divide 125MHz  clock by 50 to give 2.5MHz

reg [5:0]osc_count;

always @ (posedge _125MHZ)
begin
	if (osc_count == 24)
	begin
		osc_2_5M <= ~osc_2_5M;
		osc_count <= 0;
	end 
	else osc_count <= osc_count + 1'b1;
end

// Apply to EXOR phase detector 
assign FPGA_PLL = ref_2_5M ^ osc_2_5M;

*/

// LEDs for testing

assign DEBUG_LED0 = PCLK_12MHZ; 	// LED 0 on when we have sync	
assign DEBUG_LED1 = PCLK_count[0];		// LED D3 on when we can write to EP6
assign DEBUG_LED2 = PCLK_count[1];    // LED on when LT2208 has data 
assign DEBUG_LED3 = PCLK_count[2];  // LED on when we receive data 




endmodule 



