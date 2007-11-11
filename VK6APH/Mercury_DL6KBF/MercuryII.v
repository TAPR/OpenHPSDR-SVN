/***********************************************************
*
*	Test program for LT2208
*
************************************************************/

// (C) Phil Harman VK6APH 2006,2007


/* 	This program interfaces the LT2208 to PowerSDR over USB.
	The data from the LT2208 is in 16 bit parallel format and 
	is valid at the positive edge of the LT2208 125MHz clock.
	
	The data is processed by a CORDIC NCO to produce I and Q
	outputs.  These are decimated by 640 in CIC and CFIR filters to 
	give output data at ~195kHz to feed to the FX2 and hence via 
	the USB to PowerSDR.
	
	The data is sent to the PC in the Janus format vis:-
	
	<7F7F><7F00><0000><FF00><I MSB,I ><I LSB,Q MSB><Q,Q LSB><0000> etc 
	
	Change log:
	
	3 Nov 2007 - start using DIN version 

	sync FIR data to posedge LRCLK_192
	
	4 Nov 2007 
	
	added mic from Penny 
	added C&C decode and encoder 
	change have_sync from toggle to on or off

	
	5 Nov 2007

	change Tx to use !BCLK and strobe in Tx_fifo 
	change Rx clock_8 to BCLK  
	add sync detector before Rx fifo 
	add rest of code 
	
	6 Nov 2007
	
	added SPI interface for Alex - 
	added CW support 
	
	10 Nov 2007
	
	changed FPGA pin connections for DL6KBF interface board
	
	ADC		J17		FPGA_GPIO	FPGA
	15		4		16			88
	14		3		17			89
	13		6		14			86
	12		5		15			87
	11		8		12			82
	10		7		13			84
	9		10		10			80
	8		9		11			81
	7		12		8			76
	6		11		9			77
	5		14		6			74
	4		13		7			75
	3		16		4			70
	2		15		5			72
	1		18		2			68
	0		17		3			69
	
	11 Nov 2007
	
	removed PCLK_12MHZ from Atlas C5 to prevent clash with same clock from Janus 
	
*/
	

module Mercury(ADC,
        IFCLK, FX2_FD, FLAGA, FLAGC, SLWR, SLRD, SLOE, PKEND, FIFO_ADR, 
        CBCLK, CLRCLK, CDOUT_P, CDIN, LROUT, PTT_in, DEBUG_LED0, dot, dash,
		DEBUG_LED1, DEBUG_LED2,DEBUG_LED3,CLK_MCLK, CC, FPGA_CLK6IN, // PCLK_12MHZ,
		SPI_data, SPI_clock, Tx_load_strobe);
		

input [15:0]ADC;				// samples from LT2208
input FPGA_CLK6IN;				// 125MHz clock from Penelope via LVDS
input IFCLK;                   	// FX2 IFCLOCK - 48MHz
output CLK_MCLK; 			   	// Master Clock  to Altas for Janus 
inout  [15:0] FX2_FD;           // bidirectional FIFO data to/from the FX2
input FLAGA;
input FLAGC;
output SLWR;
output SLRD;
output SLOE;
output PKEND;
output [1:0] FIFO_ADR;
output DEBUG_LED0;               // LEDs on OZY board
output DEBUG_LED1;
output DEBUG_LED2;
output DEBUG_LED3;
output CBCLK, CLRCLK;           // Clocks to TLV320AIC23B
output CDIN;                    // Rx data to TLV320AIC23B
output LROUT;			  		// Left  and Right audio data in I2S format to Atlas
input  PTT_in;                   // PTT active high
output CC;						// Command and Control data to Atlas bus 
input  CDOUT_P;					// Mic data from Penelope
//output PCLK_12MHZ; 				// 12.5MHz out to Atlas C5
input  dot;						// dot and PTT input from J8
input  dash;					// dash input from J8
wire dot;						// CW dot key, active low
wire dash; 						// CW dash key, active low
output SPI_data;				// SPI data to Alex
output SPI_clock;				// SPI clock to Alex
output Tx_load_strobe;			// SPI Tx data load strobe to Alex


wire PKEND;
reg [3:0] state_FX;				// state for FX2
reg data_flag;					// set when data ready to send to Tx FIFO
reg [15:0] register;			// LT2208 A/D uses this to send its data to Tx FIFO
reg [6:0] loop_counter;			// counts number of times round loop 
reg [11:0] sync_Rx_used; 		// holds # of bytes in Rx_fifo for use by PC
reg [11:0] rx_avail;  			// how much space is avail in the rx fifo
reg Tx_fifo_enable;				// set when we want to send data to the Tx FIFO
reg SLOE;						// FX2 data bus enable - active low
reg SLEN; 						// Put data on FX2 bus
reg SLRD;						// FX2 read - active low
reg SLWR;						// FX2 write - active low 
reg [1:0] FIFO_ADR;				// FX2 register address 

assign PKEND = 1'b1;
reg data_ready;					// set at end of decimation





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
//                    Clocks for TLV320AIC23B etc
//
//////////////////////////////////////////////////////////////

/*      
	  The following code provides clocks as required for the TLV320, 
        PWM D/A converters. The first two  always operate at 48kHz.
        Note the phase relationship required betweeen LRCLK and BCLK.
        LRCLK must change state on the negative edge of BCLK.
*/

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

// divide  PCLK_12MHZ (12.5MHz) to give clocks for the TLV320 etc
// using Altera Megafunction

wire [7:0]clock_out;
wire CBCLK;
wire CLRCLK;

clocks clocks(PCLK_12MHZ, clock_out);

assign CBCLK = clock_out[1] ;           // 3.072MHz for TLV320 and PWM 
assign BCLK_192   = CLK_MCLK; 	    	// 12.5MHz for Rx data at 192kHz
assign CLRCLK = clock_out[7];           // for TLV320 at 48kHz
assign LRCLK_192 = clock_out[5];		// for LT2208 at 192kHz
assign BCLK  = BCLK_192; 
assign CLK_MCLK =  PCLK_12MHZ;
assign clock = FPGA_CLK6IN;


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

// The phase accumulator takes a 32 bit frequency dword and outputs a 32 bit phase dword on each clock
phase_accumulator rx_phase_accumulator(.clk(~clock),.reset(~clk_enable),.frequency(sync_frequency),.phase_out(phase));

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

// Sync dc correct output to positive edge of 192k LRCLK
reg signed [23:0]i;
reg signed [23:0]q;

always @ (posedge LRCLK_192)
begin
	i <= i_no_dc;
	q <= q_no_dc;
end 

//////////////////////////////////////////////////////////////////////
//
//		Sync and data interface
//
//////////////////////////////////////////////////////////////////////

reg [4:0]AD_state;
reg [7:0]Tx_control_0 = 0;

always @ (posedge BCLK)
begin
Tx_fifo_enable <= 1'b0;								// reset Tx FIFO strobe
if (loop_counter == 63) loop_counter <= 0; 	// count how many times through the loop to see if sync is to be sent

case (AD_state)
0:	begin
		if (!LRCLK_192)AD_state <= 0;		// loop until data_read is high
		else AD_state <= 1;
		end
1:	begin
		if (!LRCLK_192)AD_state <= 2;		// loop until data_read is low 
		else AD_state <= 1;
		end
2:	begin
		if (loop_counter == 0) begin		// if zero  then send sync and C&C bytes
			register <= 16'h7F7F;	
			Tx_fifo_enable <= 1'b1;					// strobe start if sync into Tx FIFO
			rx_avail <= 12'd4095 - sync_Rx_used;    
			AD_state <= 3;
			end
		else AD_state <= 6;
		end
3:	begin
        register[15:8] <= 8'h7F;			// send rest of sync
        register[7:0]  <= {Tx_control_0[7:2], ~clean_dash, (~clean_dot || clean_PTT_in)};
		Tx_fifo_enable <= 1'b1;
		AD_state <= AD_state + 1'b1;
		end	
4: begin
		register <= 16'h0000;				// C1 and C2 set to 0
		Tx_fifo_enable <= 1'b1;
		AD_state <= AD_state + 1'b1;
		end
5: begin
		register <= {rx_avail[11:4],8'b0};  // C3 - number of bytes free in Rx FIFO, C4 - sequence number
		Tx_fifo_enable <= 1'b1;
		AD_state <= AD_state + 1'b1;
		end		
6:	begin
		register <= i[23:8];				// first 16 bits of I
		Tx_fifo_enable <= 1'b1;
		AD_state <= AD_state + 1'b1;
		end
7:	begin
		register <= {i[7:0], q[23:16]};		// last 8 bits of I and first 8 of Q			  		
		Tx_fifo_enable <= 1'b1; 
		AD_state <= AD_state + 1'b1;
		end
8:	begin
		register <= q[15:0];				// send Q data
		Tx_fifo_enable <= 1'b1; 
		AD_state <= AD_state + 1'b1;
		end
9: begin
		register <= Tx_data;				// mic data from Penny 
		Tx_fifo_enable <= 1'b1; 
		loop_counter <= loop_counter + 1'b1;
		AD_state <= 0;
		end
default: AD_state <= AD_state + 1'b1;
endcase
end

//////////////////////////////////////////////////////////////
//
//      Read TLV320 A/D converter on Penny for microphone or line in
//
//////////////////////////////////////////////////////////////

/*      
		Since the TLV320 always runs at 48k we need a separate clock and 
        routine to read its data. TLV320 is in I2S mode.
		Read mic data on negative edge of CLRCLK. Data can be read
		any time after case 18 i.e. on negative or positive edge of CLRCLK. 
*/

reg [4:0]TX_state;
reg [15:0] Tx_data;             // Tx mic audio from TLV320
reg [15:0] Tx_q;                // holds DOUT from TLV


always @ (posedge CBCLK)
begin
Tx_q[15:0] <= {Tx_q[14:0], CDOUT_P};				// shift Penelope mic data one data left and add next bit
case (TX_state)
5'd0:   begin
        if(CLRCLK) TX_state <= 5'd1;                    // loop until CLRCLK is high
        else TX_state <= 5'd0;
        end
5'd1:   begin
        if(!CLRCLK) TX_state <= TX_state + 1'b1;         // loop until CLRCLK is low
        else TX_state <= 5'd1;
        end
// next state # is calculated by adding (# bits + last state + 1) i.e. 16 + 1 + 1 = 18
5'd18:  begin
        Tx_data <= Tx_q;                                // TLV320 (microphone or mono line in)data
        TX_state <= 5'd0;                               // done so loop again
        end
// For stereo line in add code here to wait for CLRCLK to go high then read 16 bits of data 

default:TX_state <= TX_state + 1'b1;
endcase
end


/////////////////////////////////////////////////////////////
//
//   Rx_fifo  (4096) Dual clock FIFO - Altera Megafunction (dcfifo)
//
/////////////////////////////////////////////////////////////

/*
        The write clock of the FIFO is SLRD and the read clock clock_8.
        Data from the FX2_FIFO is written to the FIFO using SLRD. Data from the
        FIFO is read on the positive edge of clock_8 when fifo_enable is high. The
        FIFO is 4096 words long.
        NB: The output flags are only valid after a read/write clock has taken place
*/


wire [15:0] Rx_data;
wire read_full;
wire [11:0] Rx_used;             // how many bytes in FX2 side Rx fifo

Rx_fifo Rx_fifo(.wrclk (~SLRD),.rdreq (fifo_enable),.rdclk (CLK_MCLK),.wrreq (Rx_fifo_enable),
                .data (Rx_FIFO),.q (Rx_data), .rdusedw(Rx_used));

///////////////////////////////////////////////////////////////
//
//				Tx_fifo - 2048 words - Altera Megafunction
//
//////////////////////////////////////////////////////////////

Tx_fifo 	Tx_fifo(.wrclk (!BCLK),.rdreq (1'b1),.rdclk (Tx_read_clock),.wrreq (Tx_fifo_enable),
				.data (register),.q (Tx_register), .wrusedw(write_used));
				
wire [15:0] Tx_register; 		// holds data from A/D to send to FX2 
reg  Tx_read_clock; 			// when goes high sends data to Tx_register
wire [10:0] write_used; 		// indicates how may bytes in the Tx buffer
reg  [10:0] syncd_write_used; 	// ditto but synced to FX2 clock 

//////////////////////////////////////////////////////////////
//
//				State Machine to manage FX2 USB interface
//
//////////////////////////////////////////////////////////////
/*
	The state machine checks if there are characters to be read
	in the FX2 Rx FIFO by checking 'EP2_has_data'  If set it loads the word
	read into the Rx_register. On the next clock it  checks if the Tx_data_flag is set
	 - if so it sends the data in 'register' to the Tx FIFO. After the Tx data has been sent 
	it checks 'EP2_has_data' in round robin fashion.
*/


wire EP2_has_data = FLAGA;					// high when EP2 has data available
wire EP6_ready = FLAGC; 					// high when we can write to EP6
reg [15:0] Rx_register;						// data from PC goes here


always @ (negedge IFCLK)
begin
	syncd_write_used <= write_used;
case(state_FX)
// state 0 - set up to check for Rx data from EP2
4'd0:begin
    SLWR <= 1;								// reset FX2 FIFO write stobe
	Tx_read_clock <= 1'b0;					// reset Tx fifo read strobe
	SLRD <= 1'b1;
	SLOE <= 1'b1;
	SLEN <= 1'b0;
	FIFO_ADR <= 2'b00; 						// select EP2
	state_FX <= state_FX + 1'b1;
	end
// delay 2 IFCLOCK cycle, this is necessary at 48MHZ to allow FIFO_ADR to settle	
// check for Rx data						
4'd2:begin
		if(EP2_has_data)
			begin
			state_FX <= state_FX + 1'b1;
			SLOE <= 1'b0; 					//assert SLOE								
			end
		else begin
			state_FX <= 4'd6; 				// No Rx data so check for Tx data ******
		end 
	end	
// Wait 2 IFCLK before we assert SLRD then load received data 
4'd4:begin
	SLRD <= 1'b0; 
	Rx_register[15:8] <= FX2_FD[7:0]; 		//  swap endian 
	Rx_register[7:0]  <= FX2_FD[15:8]; 		
	state_FX <= state_FX + 1'b1;
	end	
// reset SLRD and SLOE
4'd5:begin
	SLRD <= 1'b1; 							// positive edge of SLRD clocks data into Rx_fifo
	SLOE <= 1'b1;					
	state_FX <= state_FX + 1'b1;
	end
// check for Tx data - Tx fifo must be at least half full before we Tx
4'd6:  begin
            if (syncd_write_used[10] == 1'b1) begin // data available, so let's start the xfer...
				SLWR <= 1;
                state_FX <= state_FX + 1'b1;
				FIFO_ADR <= 2'b10;				// select EP6
				end 
            else state_FX <= 4'd2; 		  		// No Tx data so check for Rx data, 
         end									// note we already have address set 
// Wait 2 IFCLK for FIFO_ADR to stabilize, assert SLWR 
// NOTE: seems OK with 2 waits, may need more.	
4'd8:begin  
		state_FX <= state_FX + 1'b1;
		Tx_read_clock <= 1'b1;				// start transfer from Tx fifo 
	 end		
// check Tx FIFO is ready then set Write strobe 
4'd9:   begin
            if (EP6_ready) begin  					// if EP6 is ready, write to it and exit this state
				Tx_read_clock <= 1'b0;				// end of transfer from Tx fifo
                SLEN <= 1'b1;
                state_FX <= state_FX + 1'b1;
            end
            else begin                  			// otherwise, hang out here until fifo is ready
                SLWR <= 1;
                state_FX <= 4'd9;  
            end
        end
//  set SLWR
4'd10: begin
		SLWR <= 1'b0;
		state_FX <= state_FX + 1'b1;
	   end
//  reset SLWR and tristate SLEN
4'd11: begin
		SLWR <= 1;
		SLEN <= 1'b0;					
        state_FX <= 4'd0; 
		end
	default: state_FX <= state_FX + 1'b1;
	endcase
end

// FX2_FD is tristate when SLEN  is low, otherwise it's the Tx_register value.
// Swap endian so data is correct at PC end

assign FX2_FD[15:8] = SLEN ? Tx_register[7:0]  : 8'bZ;
assign FX2_FD[7:0]  = SLEN ? Tx_register[15:8] : 8'bZ;


//////////////////////////////////////////////////////////////
//
//   Sync and  C&C  Detector
//
//////////////////////////////////////////////////////////////

/*

	Read the value of Rx_register on the positive edge of SLRD.
	Look for sync and if found decode the C&C data. Then send subsequent
	data to Rx FIF0 until end of frame.
	
*/

reg [3:0]state_sync;
reg [1:0]temp_speed;
reg [8:0]CC_sync_count;
reg [7:0] Rx_control_0;   // control C0 from PC
reg [7:0] Rx_control_1;   // control C1 from PC
reg [7:0] Rx_control_2;   // control C2 from PC
reg [7:0] Rx_control_3;   // control C3 from PC
reg [7:0] Rx_control_4;   // control C4 from PC
reg [15:0]Rx_FIFO; 		  // holds data from FX2
reg Rx_fifo_enable;
reg got_sync;			  // high when we detect sync sequence
wire have_sync;			  // high when sync valid

always @ (posedge SLRD)
begin
case(state_sync)
// state 0  - loop until we find start of sync sequence
0:	begin
	Rx_fifo_enable <= 1'b0;						// don't send this data to Rx FIFO
	if (Rx_register == 16'h7F7F)begin
		state_sync <= 1;						// possible start of sync
		end
	else begin
		got_sync <= 1'b0;						// clear sync flag
		state_sync <= 0;
		end
	end	
// state 1 - check for 0x7F  sync character & get Rx control_0 
1:  begin
	if (Rx_register[15:8] == 8'h7F)begin		// have sync so continue
		Rx_control_0 <= Rx_register[7:0];
		got_sync <= 1'b1;						// set sync flag
		state_sync <= 2;
		end										
	else begin
		got_sync <= 1'b0;						// no sync so clear sync flag
		state_sync <= 0;
		end	
	end
// state 2 - get Rx control_1 & 2
2: 	begin
	Rx_control_1 <= Rx_register[15:8];
	Rx_control_2 <= Rx_register[7:0];
	state_sync <= 3;
	end
// state 3 - get Rx control_3 & 4
3:	begin
	Rx_control_3 <= Rx_register[15:8];
	Rx_control_4 <= Rx_register[7:0];
	state_sync <= 4;
	end
// state 4 - Remainder of data goes to Rx FIFO, re-start at end of frame.
// Data gets written to FIFO on next negative edge of SLRD so set up data
// to send to FIFO and enable fifo in this state. 
4:	begin
	Rx_fifo_enable <= 1'b1;						// next word goes to Rx FIFO on negedge of SLRD
	Rx_FIFO <= Rx_register;						// load data to send to Rx FIFO 
	if(CC_sync_count == 251)begin				// frame ended, go get sync again but first 
												// send this data to FIFO!
		CC_sync_count <= 1'b0;					// reset sync counter 
		state_sync <= 0;
		end	   
	else begin									// keep looping until end of frame
	    CC_sync_count <= CC_sync_count + 9'b1;
        state_sync <= 4;
		end
	end	
	
	endcase
end

// Need to clear the have_sync flag if no data is received from the 
// PC via USB. We can use the fact that there will be data on the 
// output side of the Rx FIFO if we are getting data from the PC.

assign have_sync = (got_sync & (Rx_used > 3)) ? 1'b1 : 1'b0;



//////////////////////////////////////////////////////////////
//
//   State Machine to manage PWM interface
//
//////////////////////////////////////////////////////////////
/*

        The state machine changes state on the negative edge of CLK_MCLK.
        The code loops until there are at least 3 words in the Rx_FIFO.

		The first word is the Left audio and the following the
        Right audio the next word is the  I data and the following the Q data.
    	
        The words sent to the D/A converters must be sent at the sample rate
        of the A/D converters (48kHz) so is synced on the negative edge of the CLRCLK.
		Reads from the Rx FIFO are inhibited whilst we are waiting for this edge.
*/

reg [4:0] state_PWM;                    // state for PWM  counts 0 to 13
reg [15:0] Left_Data;                   // Left 16 bit PWM data for D/A converter
reg [15:0] Right_Data;                  // Right 16 bit PWM data for D/A converter
reg [15:0] I_PWM;                       // I 16 bit PWM data for D/A conveter
reg [15:0] Q_PWM;                       // Q 16 bit PWM data for D/A conveter
reg fifo_enable;                        // controls reading of dual clock fifo
reg [6:0] byte_count;                   // counts number of times round loop
reg [8:0]sync_count;


always @ (negedge CLK_MCLK) 
begin
case(state_PWM)
0: 	begin
	if(Rx_used > 3)begin
		if (!CLRCLK) state_PWM <= 0; 	//  wait for A/D LRCLK to prepare for sync
        else state_PWM <= 1;
		end
    else begin
         state_PWM <= 0;                 // loop here if not
         end
	end
1: 	begin
   	if(!CLRCLK)begin                      // wait for A/D CLRCLK to go low so we are in sync
		fifo_enable <= 1'b1;
		state_PWM <= 2;
        end
        else state_PWM <= 1;			  // loop here until low
  	end	
// state 1 - get Left audio
2: 	begin
     Left_Data <= Rx_data;
     state_PWM <= 3;
     end
// state 2 - get Right audio
3: 	begin
     Right_Data <= Rx_data;
     state_PWM <= 4;
     end
// state 3 - get I audio
4: 	begin
     I_PWM <= Rx_data;
     state_PWM <= 5;
     end
// state 4 - get Q audio
5: 	begin
    Q_PWM <= Rx_data;
	fifo_enable <= 1'b0;				// inhibit reads from FIFO at next posedge CLK_MCLK
    state_PWM <= 0;
    end
 default: state_PWM <= 1'b0;
        endcase
end


//////////////////////////////////////////////////////////////
//
//              Decode Command & Control data
//
//////////////////////////////////////////////////////////////

/*
	Decode Rx_control_0....Rx_control_4.
    Only decode when  have_sync is true otherwise set safe values.

	Decode frequency (for Mercury and Penelope), PTT and Speed 

	The current frequency is set by the PC by decoding 
	Rx_control_1... Rx_control_4 when Rx_control_0[7:1] = 7'b0000_001
		
	The speed the AK5394A runs at, either 192k,96k or 48k is set by
    the PC by decoding Rx_control_1 when Rx_control_0[7:1] are all zero. Rx_control_1 decodes as
    follows:

    Rx_control_1 = 8'bxxxx_xx00  - 48kHz
    Rx_control_1 = 8'bxxxx_xx01  - 96kHz
    Rx_control_1 = 8'bxxxx_xx10  - 192kHz

	Decode PTT from PowerSDR. Held in Rx_control_0[0] as follows
	
	0 = PTT inactive
	1 = PTT active
	
	Decode clock sources, when Rx_control_0[7:1] = 0,  Rx_control_1[4:2] indicates the following
	
	x00  = 10MHz reference from Atlas bus ie Gibraltar
	x01  = 10MHz reference from Penelope
	x10  = 10MHz reference from Mercury
	0xx  = 125MHz source from Penelope 
	1xx  = 125MHz source from Mercury 
	
	Decode configuration, when Rx_control_0[7:1] = 0, Rx_control_1[6:5] indicates the following
	
	00 = No Tx Rx boards
	01 = Penelope fitted
	10 = Mercury fitted
	11 = Both Penelope and Mercury fitted
	
	Decode microphone source, when Rx_control_0[7:1] = 0, Rx_control_1[7] indicates the following
	
	0 = microphone source is Janus
	1 = microphone source is Janus

	
*/

wire  PTT_out;
reg [3:0]clock_s;
reg [31:0]frequency_HZ;

always @ (posedge SLRD)
begin 
if(state_sync == 4)begin 								// Need to ensure that C&C data is stable 
    if (Rx_control_0[7:1] == 7'b0000_000)begin
			clock_s[3:0] <= {1'b0,Rx_control_1[4:2]};	// decode clock source
			end
	if (Rx_control_0[7:1] == 7'b0000_001)				// decode frequency 
		frequency_HZ <= {Rx_control_1, Rx_control_2, Rx_control_3, Rx_control_4};
	else frequency_HZ <= frequency_HZ;
	end
end	

assign PTT_out = (Rx_control_0[0] && have_sync) ? 1'b1 : 1'b0; // inhibit PTT if no sync


///////////////////////////////////////////////////////////////
//
//              Implements Command & Control  encoder 
//
///////////////////////////////////////////////////////////////

/*
	The C&C encoder broadcasts data over the Atlas bus C20 for
	use by other cards e.g. Mercury and Penelope.  The data is in 
	I2S format with the clock being CBLCK and the start of each frame
	being indicated using the negative edge of CLRCLK.
	
	The data fomat is as follows:
	
	<1 bit PTT><4 bits address><32 bits frequency><4 bits clock_s><7 bits OC><1 bit mode> 
	
	for a total of 49 bits. Frequency format is the DDC data word i.e. FREQ x 2^32/Clock and 
	OC is the open collector data on Penelope. The clock source decodes as follows:
	
	0x00  = 10MHz reference from Atlas bus ie Gibraltar
	0x01  = 10MHz reference from Penelope
	0x10  = 10MHz reference from Mercury
	00xx  = 125MHz source from Penelope 
	01xx  = 125MHz source from Mercury

		
	For future expansion the four bit address enables specific C&C data to be send to individual boards.
	For the present for use with Mercury and Penelope the address is ignored. 

	NOTE: PTT_out is inhibited if clock_error is set

*/

wire [48:0]CCdata;
reg [6:0] CCcount;
reg CC;							// C&C data out to Atlas bus 
wire [3:0] CC_address;			// C&C address, fixed at 0 for now 
wire [6:0] OC;					// Open Collector outputs on Penelope board 
wire mode;
wire clock_error = 1'b0;		// dummy data for now 

// dummy data for testing

assign CC_address = 4'b0110;
assign OC = 7'b1100111;
assign mode = 0;

assign 	CCdata = {(PTT_out & !clock_error), CC_address, frequency, clock_s, OC, mode}; // concatenate data to send 

// send C&C data to Atlas bus in I2S format

reg [1:0]CCstate;

always @ (posedge CBCLK)
begin	
case(CCstate)
0:	begin
	if (CLRCLK)	 CCstate <= 1;		// loop until CLRCLK is high
	else CCstate <= 0;
	end
	
1:	if (CLRCLK)	CCstate <= 1;		// wait until it goes low - this is first CBCLK after negedge of CLRCLK
	else begin
	CCcount <= 48; 					// need to have data available for Rx on next CBCLK
	CCstate <= 2;
	end
	
2:	begin
	if (CCcount == 0) CCstate <= 0;
	else begin
	CCcount <= CCcount - 1'b1;
	CCstate <= 2;
	end
	end 
	
default: CCstate <= 0;
endcase
end 

// I2S data must be available on the 2nd positive edge of CBCLK after the CLRCLK transition
always @ (negedge CBCLK)
begin
	CC <= CCdata[CCcount];			// shift data out to Altas bus MSB first
end

//////////////////////////////////////////////////////////////
//
//		Alex Tx Band Decoder & LPF selection
//
//////////////////////////////////////////////////////////////

wire [6:0]LPF;

LPF_select Alex_LPF_select(.frequency(frequency), .LPF(LPF));

//////////////////////////////////////////////////////////////
//
//		Alex SPI interface
//
//////////////////////////////////////////////////////////////

// Assign dummy data where values not currently set

wire yellow_led = 1'b1; 	
wire ANT1 = 1'b1;			// select antenna 1 for now
wire ANT2 = 1'b0;
wire ANT3 = 1'b0;
wire red_led;
wire TR_relay;
wire [15:0]Alex_Tx_data;
wire SPI_data;
wire SPI_clock;
wire Tx_load_strobe;

// define and concatinate the data to send to Alex via SPI

assign red_led = PTT_out; 		// turn red led on when we Tx
assign TR_relay = PTT_out;		// turn on TR relay when PTT active

assign Alex_Tx_data = {3'b000,yellow_led,LPF[6:3],ANT1,ANT2,ANT3,TR_relay,red_led,LPF[2:0]};

SPI   Alex_SPI_Tx(.Alex_data(Alex_Tx_data), .SPI_data(SPI_data),
				  .SPI_clock(SPI_clock), .Tx_load_strobe(Tx_load_strobe),.spi_clock(CBCLK));


///////////////////////////////////////////////////////////////
//
//              Implements I2S format I and Q  out,
//              16 bits, two channels  for TLV320AIC23B D/A converter and Penelope
//
///////////////////////////////////////////////////////////////

I2SAudioOut  I2SAO(.lrclk_i(CLRCLK), .bclk_i(CBCLK), .left_sample_i(I_PWM), .right_sample_i(Q_PWM),.outbit_o(CDIN));

///////////////////////////////////////////////////////////////
//
//              Implements I2S format Left and Right audio out,
//              16 bits, two channels for DAC on Janus/Penelope
//
///////////////////////////////////////////////////////////////

I2SAudioOut  I2SIQO(.lrclk_i(CLRCLK), .bclk_i(CBCLK), .left_sample_i(Left_Data), .right_sample_i(Right_Data),.outbit_o(LROUT));

///////////////////////////////////////////////////////
//
//                      Debounce PTT button
//
///////////////////////////////////////////////////////

 wire clean_PTT_in; // debounced button

debounce de_PTT(.clean_pb(clean_PTT_in), .pb(PTT_in), .clk(IFCLK));


///////////////////////////////////////////////////////
//
//                      Debounce dot key
//
///////////////////////////////////////////////////////

 wire clean_dot; // debounced dot

debounce de_dot(.clean_pb(clean_dot), .pb(dot), .clk(IFCLK));


///////////////////////////////////////////////////////
//
//                      Debounce dash key
//
///////////////////////////////////////////////////////

 wire clean_dash; // debounced dash

debounce de_dash(.clean_pb(clean_dash), .pb(dash), .clk(IFCLK));// LEDs for testing



assign DEBUG_LED0 = ~have_sync; 		// LED 0 on when we have sync	
assign DEBUG_LED1 = SLRD;				// LED 
assign DEBUG_LED2 = ~PTT_out;    		// LED on when PTT active
assign DEBUG_LED3 = Rx_fifo_enable;  	// LED on when we receive data 

endmodule 



