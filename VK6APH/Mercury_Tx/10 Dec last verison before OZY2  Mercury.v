/***********************************************************
*
*	Test program for LT2208
*
************************************************************/


/* 	This program interfaces the LT2208 to PowerSDR over USB.
	The data from the LT2208 is in 16 bit parallel format and 
	is valid at the positive edge of the LT2208 100MHz clock.
	
	The data is processed by a CORDIC NCO to produce I and Q
	outputs.  These are decimated by 2048 in a CIC filter to 
	give output data at ~48kHz to feed to the FX2 and hence via 
	the USB to PowerSDR.
	
	The data is sent in the Janus format vis:-
	
	<7F7F><7F00><0000><0000><I sign extension,I MSB><Q LSB,Q sign extension><Q MSB,Q LSB><0000> etc 
	
	Change log:
	
	12 June 2006 - First test version using state machine NCO
	12 June 2006 - Sign extension for I and Q data added
	13 June 2006 - CORDIC NCO started 
	28 July 2006 - Modified to use OZY and  16 bit FIFO 
	17 Aug  2006 - modified so that CIC takes 16 bits in and gives 24 out
	28 Aug  2006 - uses  USRP CORDIC 
	29 Aug  2006 - CIC changed to decimate by 512 to give 196kHz output
	 3 Sept 2006 - Added decode of frequency from PowerSDR
		
	
*/
	


/////////////////////////////////////////////////////////////
//
//	Pins
//
//  LT2208		Pin		Ozy-Atlas	FPGA Pin
//				1		
//	    CLK		3		C19			118
//		Ov		5		
//		D15		7		C17			120
//		D14		9		C16			127
//		D13		11		C15			128
//		D12		13		C14			133
//		D11		15		C13			134
//		D10		17		C12			135
//		D9		19		C11			137
//		D8		21		C10			138
//		D7		13		C9			139
//		D6		25		C8			141
//		D5		27		C7			142
//		D4		29		C6			143
//		D3		31		C5			144
//		D2		33		C4			145
//		D1		35		C3			146
//		D0		37		C2			147
//		3.3v	39		

//
//	 FX2 pin    to   FPGA pin connections
//
//	 IFCLK			- pin 24
//   FX2_CLK		- pin 23
//	 FX2_FD[0]		- pin 56
//	 FX2_FD[1]		- pin 57
//	 FX2_FD[2]		- pin 58
//	 FX2_FD[3]		- pin 59
//	 FX2_FD[4]		- pin 60
//	 FX2_FD[5]		- pin 61
//	 FX2_FD[6]		- pin 63
//	 FX2_FD[7]		- pin 64
//	 FX2_FD[8]		- pin 208
//	 FX2_FD[9]		- pin 207
//	 FX2_FD[10]		- pin 206
//	 FX2_FD[11]		- pin 205
//	 FX2_FD[12]		- pin 203
//	 FX2_FD[13]		- pin 201
//	 FX2_FD[14]		- pin 200
//	 FX2_FD[15]		- pin 199
//	 FLAGA			- pin 198
//	 FLAGB			- pin 197
//	 FLAGC			- pin 5
//	 SLOE			- pin 13
//	 FIFO_ADR[0]	- pin 11
//	 FIFO_ADR[1]	- pin 10
//	 PKEND			- pin 8
//	 SLRD			- pin 30
//	 SLWR			- pin 31
//
//
//   General FPGA pins
//
//	 	LED[0]			- pin 67
//	 	LED[1]			- pin 68
//	 	LED[2]			- pin 69
//	 	LED[3]			- pin 70
//	 	LED[4]			- pin 72
//	 	LED[5]			- pin 74
//	 	LED[6]			- pin 75
//	 	LED[7]			- pin 76
//
////////////////////////////////////////////////////////////


module Mercury(clock, ADC,IFCLK, FX2_FD,FIFO_ADR, SLRD, SLWR, SLOE, FLAGA, FLAGC, PKEND, LED);

input [15:0]ADC;			// samples from LT2208
input clock;				// 100MHz clock from LT2208
input IFCLK;				// 48MHz clock from FX2
inout  [15:0] FX2_FD;		// bidirectional FIFO data to/from the FX2
//output  [15:0] FX2_FD;	// bidirectional FIFO data to/from the FX2 
							// ***** use this so simulation works 
input  FLAGA;				// Flags input from the FX2
input  FLAGC;
output SLWR;				// FX2's write line
output SLRD; 				// FX2's FIFO read line
output PKEND; 				// signals packed end, not used
output [1:0] FIFO_ADR;		// FX2 register address 
output [7:0] LED;			// LEDs on OZY board
output SLOE;				// FX2 data bus enable - active low



wire PKEND;
reg [3:0] state_FX;			// state for FX2
reg data_flag;				// set when data ready to send to Tx FIFO
reg [15:0] register;		// AK5394A A/D uses this to send its data to Tx FIFO
reg [15:0] Tx_data;		  	// Tx mic audio from TLV320
reg [6:0] loop_counter;		// counts number of times round loop 
reg [11:0] sync_Rx_used; 	// holds # of bytes in Rx_fifo for use by PC
reg [11:0] rx_avail;  		// how much space is avail in the rx fifo
reg strobe;					// set when we want to send data to the Tx FIFO
reg SLOE;					// FX2 data bus enable - active low
reg SLEN; 					// Put data on FX2 bus
reg SLRD;					// FX2 read - active low
reg SLWR;					// FX2 write - active low 
reg [1:0] FIFO_ADR;			// FX2 register address 

assign PKEND = 1'b1;
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

//assign temp_ADC = ADC;

// A Digital Output Randomizer is fitted to the LT2208. This complements bits 15 to 1 if 
// bit 0 is 1. This helps to reduce any pickup by the A/D input of the digital outputs. 
// We need to de-ramdomize the LT2208 data if this is turned on. 

// assign temp_ADC = ADC[0]? {~ADC[15:1],ADC[0]}: ADC;  // Compensate for Digital Output Randomizer


/*

// multiply by 1 and -1 at 25MHz

reg [15:0]i_out;
reg [15:0]q_out;
reg [15:0]temp_ADC;

reg [1:0]state;

always @ (posedge clock)
begin

temp_ADC <= ADC;

// A Digital Output Randomizer is fitted to the LT2208. This complements bits 15 to 1 if 
// bit 0 is 1. This helps to reduce any pickup by the A/D input of the digital outputs. 
// We need to de-ramdomize the LT2208 data if this is turned on. 

//if (ADC[0]) temp_ADC[15:1] <= ~ADC[15:1];  // Compensate for Digital Output Randomizer

case (state)
2'd0:	begin 
	i_out <= temp_ADC;			// x1
	q_out <= 0;					// x-1
	state <= 2'd1;
	end
2'd1:	begin
	i_out <= 0;					// x1
	q_out <= temp_ADC;			// x1
	state <= 2'd2;
	end
2'd2: 	begin
	i_out <= ~temp_ADC + 16'd1;
	q_out <= 0;
	state <=2'd3;
	end
2'd3:	begin
	i_out <= 0;
	q_out <= ~temp_ADC + 16'd1;
	state <= 2'd0;
	end
default: state <= 2'd0;
endcase
end
*/



//////////////////////////////////////////////////////////////
//
//		CORDIC NCO - set at ~25MHz
//
//
//////////////////////////////////////////////////////////////

//Code rotates A/D input at set frequency  and produces I & Q /

// IMPORTANT: set Qin to be 16'd0 since we only have a single input 

wire [15:0]i_out;
wire [15:0]q_out;
wire [15:0]temp_ADC;

	//parameter FREQ = 32'h40000000;  //~25MHz  i.e. FREQ /(100e6/2^32)
	//parameter FREQ = 32'h24538EF3; // 14.190MHz
	wire	[31:0] phase;

	// The phase accumulator takes a 32 bit frequency dword and outputs a 32 bit phase dword on each clock
	phase_accumulator rx_phase_accumulator(.clk(clock),.reset(~clk_enable),.frequency(frequency),.phase_out(phase));

	// The cordic takes I and Q in along with the top 18 bits of the phase dword.  The I and Q out are freq shifted
	cordic rx_cordic(.clk(clock),.reset(~clk_enable),.Iin(16'd0),.Qin(ADC),.PHin(phase[31:16]),.Iout(i_out),.Qout(q_out),.PHout());




///////////////////////////////////////////////////////////////
//
//		CIC Filter - designed using MatLab
//
//
///////////////////////////////////////////////////////////////

// -------------------------------------------------------------
//
// Module: filter_14
//
// HDL Code Generation Options:
//
// TargetLanguage: Verilog
// Name: filter_14
// TargetDirectory: C:\DOCUME~1\phil\LOCALS~1\Temp\tp124049\hdlsrc
// SerialPartition: -1
// CastBeforeSum: On
//
// Filter Settings:
//
// Discrete-Time FIR Multirate Filter (real)
// -----------------------------------------
// Filter Structure        : Cascaded Integrator-Comb Decimator
// Decimation Factor       : 512
// Differential Delay      : 1
// Number of Sections      : 4
// Stable                  : Yes
// Linear Phase            : Yes (Type 1)
//
// Input                   : s16,15
// Output                  : s24,-13                : s24,-21

wire [23:0]cic_out_i;
wire [23:0]cic_out_q;
wire ce_out_i;				// narrow pulse when data available
wire ce_out_q;				// narrow pulse when data available


filter_14 cic_I( .clk(clock),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(i_out),.filter_out(cic_out_i),.ce_out(ce_out_i));
filter_14 cic_Q( .clk(clock),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(q_out),.filter_out(cic_out_q),.ce_out(ce_out_q));


//////////////////////////////////////////////////////////////////////
//
//		Sync and data interface
//
//////////////////////////////////////////////////////////////////////

// since we are decimating by 512 we can use a slower clock for reading
// the A/D, say 12.5MHz

reg clock_8;
reg [4:0]clock_count;
wire [23:0] temp_I;
wire [23:0] temp_Q;

always @ (posedge clock)
begin
	if (clock_count == 7)
		begin
		clock_8 <= ~clock_8;
		clock_count <= 0;
		end
	else clock_count <= clock_count + 5'b1;
end

reg [4:0]AD_state;

always @ (posedge clock_8)
begin
strobe <= 1'b0;								// reset Tx FIFO strobe
if (loop_counter == 63) loop_counter <= 0; 	// count how many times through the loop to see if sync is to be sent

case (AD_state)
5'd0:	begin
		if (data_ready)AD_state <= 5'd1;	// loop until data_read is high
		else AD_state <= 5'd0;
		end
5'd1:	begin
		if (loop_counter == 0) begin		// if zero  then send sync and C&C bytes
			register <= 16'h7F7F;	
			strobe <= 1'b1;					// strobe start if sync (80) into Tx FIFO
			AD_state <= 5'd2;
			end
		else AD_state <= 5'd5;
		end
5'd2:	begin
		register <= 16'h7F00;				// rest of sync and C0 set to 0
		strobe <= 1'b1;
		AD_state <= AD_state + 1'b1;
		end	
5'd3: begin
		register <= 16'h0000;				// C1 and C2 set to 0
		strobe <= 1'b1;
		AD_state <= AD_state + 1'b1;
		end
5'd4: begin
		register <= 16'hFF00;				// C3 set to 255 and  C4 set to 0
		strobe <= 1'b1;
		AD_state <= AD_state + 1'b1;
		end		
5'd5:	begin
		register <= cic_out_i[23:8];			// first 16 bits of I
		strobe <= 1'b1;
		AD_state <= AD_state + 1'b1;
		end
5'd6:	begin
		register <= {cic_out_i[7:0], cic_out_q[23:16]};	// last 8 bits of I and first 8 of Q			  		
		strobe <= 1'b1; 
		AD_state <= AD_state + 1'b1;
		end
5'd7:	begin
		register <= cic_out_q[15:0];			// send Q data
		strobe <= 1'b1; 
		AD_state <= AD_state + 1'b1;
		end
5'd8: begin
		register <= 0;				// send dummy mic  data set to 0
		strobe <= 1'b1; 
		loop_counter <= loop_counter + 1'b1;
		AD_state <= 0;
		end
default: AD_state <= 0;
endcase
end

//////////////////////////////////////////////////////////////
//
//				One-shot to strobe data into Tx FIFO 
//
//////////////////////////////////////////////////////////////

/*
	The code generates a short pulse (data_flag) on the negative
	edge of clock_count[2] if strobe is set. The pulse strobes data in 'register'
	into the Tx FIFO
*/

always @ (negedge clock_count[2])
begin	
		if (data_flag)
			data_flag <= 1'b0;
		else
			if (strobe) data_flag <= 1'b1;
end


// extend the lenght of the ce_out_i pulse so that A/D code has time to see it 
reg [3:0]pulse;

always @ (negedge clock)
begin
case (pulse)
0:begin 
	if (ce_out_i)begin
		data_ready <= 1'b1;
		pulse <= pulse + 1'b1;
	end	
	else begin
		data_ready <= 1'b0;
		pulse <= 0;
	end
  end 
7:begin
	data_ready <= 1'b0;
	pulse <= 0;
  end
	
default: pulse <= pulse + 1'b1;
endcase
end

///////////////////////////////////////////////////////////////
//
//				Tx_fifo - 2048 words - Altera Megafunction
//
//////////////////////////////////////////////////////////////

Tx_fifo 	Tx_fifo(.wrclk (data_flag),.rdreq (1'b1),.rdclk (Tx_read_clock),.wrreq (1'b1),
				.data (register),.rdempty (Tx_rdempty), .wrempty (Tx_write_empty),
				.wrfull (),.q (Tx_register), .wrusedw(write_used));
				
wire Tx_rdempty;				// High when Tx fifo empty
wire [15:0] Tx_register; 		// holds data from A/D to send to FX2 
wire Tx_write_empty; 
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
	 - if so it sends the data in 'register'to the Tx FIFO. After the Tx data has been sent 
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
	Rx_register[7:0]  <= FX2_FD[15:8]; 		// done need RX data, just ignore it for now. 
	state_FX <= state_FX + 1'b1;
	end	
// reset SLRD and SLOE
4'd5:begin
	SLRD <= 1'b1; 
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
//				Decode NCO frequency from PowerSDR
//
//////////////////////////////////////////////////////////////

reg [31:0]frequency;
reg [1:0] freq;
reg led0;

always @ (posedge SLRD)  // positive edge of FX2 FIFO read strobe
begin 
case (freq)
0: 	if (Rx_register == 16'h7F7F) freq <= 1; // look for start of sync
	else freq <= 0;
1: 	if (Rx_register[15:1] == 15'b0111_1111_0000_001) // 7F, 0000 001x
	begin
		freq <= 2;
		led0 = ~led0; // toggle test led 
	end 
	else freq <= 0; // look for rest of sync and address
2:	begin 
	frequency[31:16]<= Rx_register;
	freq <= 3;
	end
3:  begin
	frequency[15:0] <= Rx_register[15:0];
	freq <= 0;
	end
default: freq <= 0;
endcase
end 
	


// LEDs for testing

assign LED[0] = led0; 		
assign LED[1] = ~EP6_ready;		// LED D3 on when we can write to EP6
assign LED[2] = data_ready; 
assign LED[3] = 1'b1;
assign LED[4] = 1'b1; 
assign LED[5] = 1'b1; 
assign LED[6] = ~SLRD; 
assign LED[7] = ~EP2_has_data; // LED on when we receive data 

endmodule 