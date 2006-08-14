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
	
	<7F7F><7F00><00><00><00><00><I sign extension,I MSB><Q LSB,Q sign extension><Q MSB,Q LSB><0000> etc 
	
	Change log:
	
	12 June 2006 - First test version using state machine NCO
	12 June 2006 - Sign extension for I and Q data added
	13 June 2006 - CORDIC NCO stated 
	28 July 2006 - Modified to use OZY and  16 bit FIFO 
	
	
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

//	 	FX2 pin    to   FPGA pin connections
//
//  	FX2_CLK			- pin 23
//		FX2_FD[0]		- pin 56
//	 	FX2_FD[1]		- pin 57
//		FX2_FD[2]		- pin 58
//	 	FX2_FD[3]		- pin 59
//	 	FX2_FD[4]		- pin 60
//	 	FX2_FD[5]		- pin 61
//	 	FX2_FD[6]		- pin 63
//	 	FX2_FD[7]		- pin 64
//	 	FX2_FD[8]		- pin 208
//	 	FX2_FD[9]		- pin 207
//	 	FX2_FD[10]		- pin 206
//	 	FX2_FD[11]		- pin 205
//	 	FX2_FD[12]		- pin 203
//	 	FX2_FD[13]		- pin 201
//	 	FX2_FD[14]		- pin 200
//	 	FX2_FD[15]		- pin 199
//	 	FX2_flags[0]	- pin 198
//	 	FX2_flags[1]	- pin 197
//	 	FX2_flags[2]	- pin 5
//	 	FX2_PA[0]		- pin 15
//	 	FX2_PA[1]		- pin 14
//	 	FX2_PA[2]		- pin 13
//	 	FX2_PA[3]		- pin 12
//	 	FX2_PA[4]		- pin 11
//	 	FX2_PA[5]		- pin 10
//	 	FX2_PA[6]		- pin 8
//	 	FX2_PA[7]		- pin 6
//	 	FX2_SLRD		- pin 30
//	 	FX2_SLWR		- pin 31
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


module Mercury(clock, ADC, FX2_CLK, FX2_FD, FX2_SLRD, FX2_SLWR, FX2_flags, FX2_PA, LED);

input [15:0]ADC;			// samples from LT2208
input clock;				// 100MHz clock from LT2208

reg [15:0]i_out;
reg [15:0]q_out;
input FX2_CLK; 				// FX2 clock - 24MHz
inout  [15:0] FX2_FD;		// bidirectional FIFO data to/from the FX2
//output  [15:0] FX2_FD;	// bidirectional FIFO data to/from the FX2 *******************************8
							// ***** use this so simulation works 
input [2:0] FX2_flags;		// Flags input from the FX2
output reg FX2_SLWR;		// FX2's write line
output reg FX2_SLRD; 		// FX2's FIFO read line
inout [7:0] FX2_PA;			// bidirectional FX2's Port A data
output [7:0] LED;			// LEDs on OZY board


/* 
 meaning of FX2 flags etc
FIFO2 data available FX2_flags[0] = 1
FIFO3 data available FX2_flags[1] = 1
FIFO4 ready to accept data  FX2_flags[2] = 1
FIFO5 ready to accept data  FX2_PA[7] = 1
set FX2_PA[0] = 1'b1;
set FX2_PA[1] = 1'b1;
set FX2_PA[3] = 1'b1;
FX2_PA[2] = FIFO_DATAOUT_OE;
FX2_PA[6] = FIFO_PKTEND;
FX2_PA[5:4] = FIFO_FIFOADR;

FX2_FD is a bi-directional data bus

*/

// set up FX2 Port A

assign FX2_PA[0] = 1'b1; 	// always set on 
assign FX2_PA[1] = 1'b1; 	// always set on 
assign FX2_PA[2] = FIFO_DATA_OUTPUT_ENABLE;
assign FX2_PA[3] = 1'b1; 	// always set on
assign FX2_PA[5:4] = FIFO_ADDRESS;
assign FX2_PA[6] = 1'b1; 	// set packet end to false 


reg [2:0] state_FX;			// state for FX2
reg data_flag;				// set when data ready to send to Tx FIFO
reg [15:0] register;		// A/D uses this to send its data to Tx FIFO
reg [5:0] loop_counter;		// counts number of times round loop 
reg [11:0] sync_Rx_used; 	// holds # of bytes in Rx_fifo for use by PC
reg [11:0] rx_avail;  		// how much space is avail in the rx fifo
reg strobe;					// set when we want to send data to the Tx FIFO

////////////////////////////////////////////////////////////////////////
//
//		Reset and enable code
//
////////////////////////////////////////////////////////////////////////

// holds reset = 1 and clk_enable = 0 for first 1024 clock counts

wire reset;
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

assign reset = ~clk_enable;

/* 
	multiply the A/D data by 1 and -1 at 25MHz rate  in phase quadratrue.
   	Complement and add 1 to multiply by -1 

	state machine to generate sin and cos at 25MHz 

   __          __
  |  |        |  |
__|  |__    __|  |__     sin 
        |  |
        |__|

      __          __
     |  |        |  |
   __|  |__    __|  |__  cos
           |  |
           |__|



*/

// send data_ready flag at ~48kHz  TODO: change to using oe with a latch


reg [1:0]state;
reg [9:0]dec_counter;
reg data_ready;		// set at end of decimation
wire [15:0]temp_I;
wire [15:0]temp_Q; 
reg [15:0]temp_ADC;


always @ (posedge clock)
begin
if (dec_counter == 1023) begin
//		temp_I <= i_out;				// save I and Q values
//		temp_Q <= q_out;
		data_ready <= ~data_ready;		// toggle data ready flag 
		end
dec_counter <= dec_counter + 10'b1;

temp_ADC <= ADC;


// A Digital Output Randomizer is fitted to the LT2208. This complements bits 15 to 1 if 
// bit 0 is 1. This helps to reduce any pickup by the A/D input of the digital outouts. 
// We need to de-ramdomize the LT2208 data if this is turned on. 

if (ADC[0]) temp_ADC[15:1] <= ~ADC[15:1];  // Compensate for Digital Output Randomizer



case (state)
2'd0:	begin 
	i_out <= temp_ADC;			// x1
	q_out <= 0;				// x-1
	state <= 2'd1;
	end
2'd1:	begin
	i_out <= 0;				// x1
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

//////////////////////////////////////////////////////////////
//
//		CORDIC NCO - set at ~25MHz
//
//
//////////////////////////////////////////////////////////////

/* Code rotates A/D input at set frequency  and produces I & Q */

/*

	parameter FREQ = 32'b0100000000000000;  //~25MHz  i.e. FREQ /(100e6/2^16)
	wire	[31:0] phase;

	// The phase accumulator takes a 32 bit frequency dword and outputs a 32 bit phase dword on each clock
	phase_accumulator rx_phase_accumulator(.clk(clock),.reset(reset),.frequency(FREQ),.phase_out(phase));

	// The cordic takes I and Q in along with the top 16 bits of the phase dword.  The I and Q out are freq shifted
	cordic rx_cordic(.clk(clock),.reset(reset),.Iin(temp_ADC),.Qin(temp_ADC),.PhaseIn(phase[31:16]),.Iout(i_out),.Qout(q_out),.PHout());

*/

///////////////////////////////////////////////////////////////
//
//		CIC Filter - designed using MatLab
//
//
///////////////////////////////////////////////////////////////

// Discrete-Time FIR Multirate Filter (real)
// -----------------------------------------
// Filter Structure        : Cascaded Integrator-Comb Decimator
// Decimation Factor       : 2048
// Differential Delay      : 1
// Number of Sections      : 3


wire [15:0]cic_out_i;
wire [15:0]cic_out_q;
wire ce_out_i;				// narrow pulse when data available
wire ce_out_q;				// narrow pulse when data available


filter cic_I( .clk(clock),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(i_out),.filter_out(cic_out_i),.ce_out(ce_out_i));
filter cic_Q( .clk(clock),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(q_out),.filter_out(cic_out_q),.ce_out(ce_out_q));

assign temp_I = cic_out_i;
assign temp_Q = cic_out_q;


//////////////////////////////////////////////////////////////////////
//
//		Sync and data interface
//
//////////////////////////////////////////////////////////////////////

// since we are decimating by 2048 we can use a slower clock for reading
// the A/D, say 12.5MHz

reg clock_8;
reg [4:0]clock_count;

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
		if(data_ready) AD_state <= 0;		// loop until data_ready is low
		else AD_state <= 5'd1;				// low so next state
		end
5'd1:	begin
		if (data_ready)AD_state <= 5'd2;	// loop until data_read is high
		else AD_state <= 5'd1;
		end
5'd2:	begin
		if (loop_counter == 0) begin		// if zero  then send sync and C&C bytes
			register <= 16'h7F7F;	
			strobe <= 1'b1;					// strobe start if sync (80) into Tx FIFO
			AD_state <= 5'd3;
			end
		else AD_state <= 5'd5;
		end
5'd3:	begin
		register <= 16'h7F00;				// rest of sync and C&C all set to 0
		strobe <= 1'b1;
		AD_state <= AD_state + 1'b1;
		end	
5'd4: begin
		register <= 16'h0000;					// rest of sync and C&C all set to 0
		strobe <= 1'b1;
		AD_state <= AD_state + 1'b1;
		end
5'd5:	begin
		if (temp_I[15]) register <= {8'b11111111,temp_I[15:8]} ;	// send I data with sign extension to 24 bits
		else register <= {8'b0, temp_I[15:8]};
		strobe <= 1'b1;
		AD_state <= AD_state + 1'b1;
		end
5'd6:	begin
		if (temp_Q[15]) register <= {temp_I[7:0], 8'b11111111};	// send Q data with sign extension to 24 bits
		else register <= {temp_I[7:0], 8'b0};				  		
		strobe <= 1'b1; 
		AD_state <= AD_state + 1'b1;
		end
5'd7:	begin
		register <= temp_Q;			// send Q data
		strobe <= 1'b1; 
		AD_state <= AD_state + 1'b1;
		end
5'd8: begin
		register <= 0;				// send dummy mic  data
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
	edge of clock_8 if strobe is set. The pulse strobes data in 'register'
	into the Tx FIFO
*/

reg d;

always @ (negedge clock_8 or posedge data_flag)
begin	
		if (data_flag)
			data_flag <= 1'b0;
		else
			if (strobe) data_flag <= 1'b1;
end

///////////////////////////////////////////////////////////////
//
//				Tx_fifo - 2048 Words
//
//////////////////////////////////////////////////////////////

Tx_fifo 	Tx_fifo(.wrclk (data_flag),.rdreq (1'b1),.rdclk (Tx_read_clock),.wrreq (1'b1),
				.data (register),.rdempty (Tx_rdempty), .wrempty (Tx_write_empty),
				.wrfull (Tx_write_full),.q (Tx_register), .wrusedw(write_used));
				
wire Tx_rdempty;				// High when Tx fifo empty
wire [15:0] Tx_register; 		// holds data from A/D to send to FX2 
wire Tx_write_full;				
wire Tx_write_empty; 
reg  syncd_Tx_write_full;
reg  Tx_read_clock; 			// when goes high sends data to Tx_register
wire [9:0] write_used; 			// indicates how may bytes in the Tx buffer
reg  [9:0] syncd_write_used; 	// ditto but synced to FX2 clock 

//////////////////////////////////////////////////////////////
//
//				State Machine to manage FX2 USB interface
//
//////////////////////////////////////////////////////////////
/*
	The state machine checks if there are characters to be read
	in the FX2 Rx FIFO by checking 'fifo_data_available'  If set it loads the byte
	read into the Rx_register. On the next clock it  checks if the Tx_data_fag is set
	 - if so it sends the data in 'register'to the Tx FIFO. After the Tx data has been sent 
	it checks 'fifo_data_available' etc.
*/

wire fifo_data_available = FX2_flags[0];	// goes high when FIFO 2 has data available 
wire fifo_ready = FX2_flags[2]; 	// high when we can write to FIFO 4
reg  [15:0] Rx_register;				// data from PC goes here
reg [1:0] FIFO_ADDRESS;				// FIFO address selector, uses 2 bits
reg FIFO_DATA_OUTPUT_ENABLE;		// select FX2 FIFO to read or write	



always @ (posedge FX2_CLK)
begin
	syncd_write_used <= write_used;
	case(state_FX)
// state 0 - check for Rx data
  3'h0: begin
        FX2_SLWR <= 1;								// reset FX2 FIFO write stobe
		Tx_read_clock <= 1'b0;						// reset Tx fifo read strobe
		if (fifo_data_available)					// does  FX2 FIFO have data
			begin
			state_FX <= 3'h1; 						// yes,we have Rx data so set up to get it
			FIFO_ADDRESS <= 2'b00;					// select FIFO 2
			FIFO_DATA_OUTPUT_ENABLE <= 1'b0;		// enable receive on FIFO bus
			end
		else begin
			state_FX <= 3'h2;				 		// no Rx data so check for Tx data
			end	
		end
// state 1 - get Rx data 
  3'h1: begin
		FX2_SLRD <= 0; 								// set read strobe low
		Rx_register <= FX2_FD;			// get data from FIFO
		state_FX <= 3'h2;
		end
// state 2 - check for Tx data - Tx fifo must be at least half full before we Tx
  3'h2:  begin
		 FX2_SLRD <= 1; 							// reset read strobe
            if (syncd_write_used[9] == 1'b1) begin  // data available, so let's start the xfer...
				Tx_read_clock <= 1'b1;				// start transfer from Tx fifo
                FX2_SLWR <= 1;
                state_FX <= 3'h3;
				FIFO_ADDRESS <= 2'b10;				// select FIFO 4
				FIFO_DATA_OUTPUT_ENABLE <= 1'b1;	// enable send on FIFO bus
            end 
            else begin            					// No Tx data so check for Rx data
                FX2_SLWR <= 1;
                state_FX <= 3'h0;
            end
        end
// state 3 - check Tx FIFO is ready then set Write strobe 
  3'h3: begin
            if (fifo_ready) begin  	// if FIFO 4 is ready, write to it and exit this state
				Tx_read_clock <= 1'b0;				// end of transfer from Tx fifo
                FX2_SLWR <= 0;
                state_FX <= 3'h0;
            end
            else begin                  // otherwise, hang out here until fifo is rdy.
                FX2_SLWR <= 1;
                state_FX <= 3'h3;
            end
        end
	default: state_FX <= 3'h0;
	endcase
end

// FX2_FD is tristate when write is hi, otherwise it's the Tx_register value.

assign FX2_FD = FX2_SLWR ? 15'bZ : Tx_register;

// LEDs for testing

assign LED[0] = ce_out_i; 		//  LED lights when we have A/D data 
assign LED[1] = FX2_flags[2];	//  LED on when we can write to FX2 FIFO

endmodule 