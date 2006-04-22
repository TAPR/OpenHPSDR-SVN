// V3.10 1st March 2006

// 
// Full duplex Xylo interface for Wolfson WM8785  and TI TLV320AIC23B A/D converters

// TLV320 is set in slave  mode  and obtains all its clocks from 
// the Wolfson. 



// Sends 48k/24 bit A/D data and 48k/16 bit mic/line data  via FIFO 4 and receives 48k/16 bit L/R and I/Q data.

// A flag of 0x80_00_00 is sent prior to 5 bytes of control data then 3 bytes of 
// left data, 3 bytes of right data and 2 bytes of mic data.  The  data sequence is then repeated 
// Every 512 bytes the sync and control data is is repeated.  

// The format for A/D data to the PC is:-
//
//  <0x80><0x00><0x00><C0><C1><C2><C3><C4><Left MSB><Left><Left LSB><Right MSB><Right><Right LSB><Mic MSB><Mic LSB>... etc 
//
// where Cn is a control byte - see protocol design document for full description. 

//               Left                             Right 
//0---------------------------------31--------------------------------------63
//0        8        16       24                40        48         56 58 59  BCLK counter
//<   MSB  ><       ><  LSB  > 	     <   MSB   ><        ><   LSB   >       - Wolfson 
//<Mic MSB ><Mic LSB> 	     								                - TLV 


// Sync and control bytes are sent as follows:

// 0     1     2     3   4   5   6   7  BCLK counter
//<0x80><0x00><0x00><C0><C1><C2><C3><C4> 


// The format for D/A data from the PC is the same control sequence followed by  48k/16 bit data:-

//
//                                            Left      Right      I         Q
//   <0x80><0x00><0x00><C0><C1><C2><C3><C4><MSB><LSB><MSB><LSB><MSB><LSB><MSB><LSB> etc...

// Control bytes are as follows:
//
// C0 bit 0 = 1 when PTT or dot key  activated (i.e. button on Xylo) or set by MOX in PowerSDR
// C0 bit 1 = 1 when dash key activated. 
// etc....see protocol design document for full C&C format. 


//
// A/D data is in 2's complement format. 
// Wolfson is set via I2C to be 48kHz, 24 bit and Left Justify mode. 
//
// TLV320 is set via I2C to be 48kHz, 16 bit and Left Justify mode. 
//
// I2C command strings 
// Wolfson 	- 34 00 25, 34 02 02, send this prior to using.
//
// TLV320  	- 36 1E 00 - Reset chip
//			- 36 12 01 - set digital interface active
//			- 36 0A 00 - turn D/A mute off 
//			- 36 08 15 - D/A on, mic input, mic 20dB boost
//			- 36 0C 00 - All chip power on
//			- 36 0E 01 - Slave, 16 bit, Left Justified
//			- 36 10 40 - Clock/2, 48k, Normal mode


// Wolfson clock is 24.576MHz (AD_clock on pin 66) and TLV320 is the
// same but is divided in two within the chip. 
// This version uses 2 state machines, one to manage A/D
// and one to manage the FX2 USB interface. The two are needed
// since the FX2 USB must run off the FX2 24MHz clock and the
// A/D's  must run of the 24.576MHz clock.
//
// Uses single bit D/A converters using 16 bits for I and Q signals.
//
// 
// Built with Quartus II v5.1 
//
// Change log: 	First working version 14th Jan 2006. 
//				Start Rx FIFO version 17th Jan 2006.
//				2048 byte  Rx FIFO implemented 21 Jan 2006.
//				Start Tx fifo 21 Jan 2006
//				Modifications to Rx fifo (so that it works!) 23 Jan 2006
// 				Modifications to A/D state machine to tidy code 31st Jan 2006 
//				Modifications to use four byte sync every 512 bytes
//              Mods to use 3 byte sync and control bytes 9th Feb 2006
//				Push button to test data bytes added to control Red LED 9th Feb 2006 
//              Mods to use 48k/24 bit from A/D and 48k/16 bit to D/A  10th Feb 2006 
//				Mods to test TLV320AIC23B A/D 13th Feb 2006  
//				TLV put in Slave mode and its clocks obtained from the Wolfson 14th Feb 2006
//				PTT added such that when active A/D comes from LTD230 else Wolfson 14th Feb 2006
//				Added debounce for PTT button 15th Feb 2006
//				Changed protocol so that mic data is send all the time 16th Feb 2006
//				Changed to 48k 24 bit from Wolfson and added I and Q PWM D/A decode 20th Feb 2006
//				Added Clock pins for Xtal on Xlyo board 21 Feb 2006
//				Increased Rx fifo to  4096 bytes and added how much space in the fifo - KD5TFD - 20th Feb 2006
//				Added support for TLV320AIC23B D/A converters to provide L/R audio output - KD5TFD. Added I/Q D/A
//				output using single bit PWM D/A converters - 24th Feb 2006. 
//				Fixed bug in code that calculates how much space in Rx fifo - 25th Feb 2006.
//				Added dot and dash inputs to Control byte 0 - 28th Feb 2006.
// 	
////////////////////////////////////////////////////
//   WM8785 and LTV320AIC23B connections to Xylo board
//
//   24.576MHZ clock - pin 66 (AD_CLK)
//   BCLK			 - pin 72 - Wolfson
//   DOUT 			 - pin 74 - Wolfson
//   LRCLK 		     - pin 71 - Wolfson
//   I_PWM_out	 	 - pin 90
//   Q_PWM_out		 - pin 88
//   button 		 - pin 97 - PTT button 
//	 TX_BCLK		 - pin 77 - TLV320
//   TX_LRCLK		 - pin 76 - TLV320
//   TX_DOUT		 - pin 78 - TLV320
//   RX_DIN			 - pin 79 - TLV320
//   RX_INLRCLK		 - pin 84 - TLV320
//   alt_clk_i       - pin 34 (hardwired to alt osc socket on xylo)
//   alt_clk_o       - pin 70 - pass alt osc clock thru to wolson board, & send it back on 66
//   dbgout          - pin 89
//	 dot			 - pin 73 - CW dot key - active low 
//	 dash			 - pin 75 - CW dash key - active low 
//   ad_reset        - pin 70 - reset pin on a/d chip - active low (chip held in reset while low)    
//
//   Pins 87 and 85 are reserved for ref counter



//   Pins 87 and 85 are reserved for ref counter
//
/////////////////////////////////////////////////////

module Duplex(
	FX2_CLK, AD_CLK, FX2_FD, FX2_SLRD, FX2_SLWR, FX2_flags, FX2_PA, BCLK, DOUT, LRCLK, LED, TxD, I_PWM_out,
	Q_PWM_out, button, TX_BCLK, TX_LRCLK, TX_DOUT, RX_DIN, RX_INLRCLK, alt_clk_i, alt_clk_o, 
    dbgout, dot, dash, ad_reset);

input AD_CLK; 				// From A/D converter 24.576MHz 
input FX2_CLK; 				// FX2 clock - 24MHz
input BCLK, DOUT, LRCLK;
inout  [7:0] FX2_FD;		// bidirectional FIFO data to/from the FX2
//output  [7:0] FX2_FD;	// bidirectional FIFO data to/from the FX2 *******************************8
							// ***** use this so simulation works 
input [2:0] FX2_flags;		// Flags input from the FX2
input button;				// push button in Xylo board, active low, pin 97
output reg FX2_SLWR;		// FX2's write line
output reg FX2_SLRD; 		// FX2's FIFO read line
inout [7:0] FX2_PA;			// bidirectional FX2's Port A data
output [1:0] LED;			// Red and Green LEDs on Xylo board
output TxD;					// RS232 Tx data 
output I_PWM_out;			// PWM D/A converter output
output Q_PWM_out; 
output TX_BCLK, TX_LRCLK, RX_INLRCLK; // Clocks to TLV320AIC23B 
output RX_DIN; 				// Rx data to TLV320AIC23B
input TX_DOUT; 				// A/D data from TLV320AIC23B
input alt_clk_i;
output alt_clk_o;
output dbgout;   			// wjt added -- tied to 89
reg dbgout;
input dot; 					// CW dot key, active low
input dash;					// CW dash key, active low
output ad_reset;
reg ad_reset;



assign alt_clk_o = alt_clk_i; // pass thru alt osc clock


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


reg [2:0] state_A_D;		// state for A/D
reg [2:0] state_FX;			// state for FX2
reg [7:0] q;				// holds DOUT from Wolfson
reg [7:0] Tx_q; 			// holds DOUT from TLV
reg [5:0]count;				// counts from 0 to 63
reg data_flag;				// set when A/D has data ready
reg [7:0] register;		  	// Wolfson A/D this to send its data to FX2
reg [7:0] Tx_MSB;		  	// MSB of Tx mic audio
reg [7:0] Tx_LSB;			// LSB of Tx mic audio
reg [6:0] loop_counter;		// counts number of times round loop 
reg [11:0] sync_Rx_used; 	// holds # of bytes in Rx_fifo for use by PC
reg [11:0] rx_avail;  		// kd5tfd -- how much space is avail in the rx fifo


// hold the A/D chip in reset until 2**24 (16.6 million) ad_clocks have passed - about 3/4 a second
// kd5tfd added
reg ad_enabled;
reg [23:0] ad_count;

always @ (posedge AD_CLK) begin
        ad_count <= ad_count + 1;
        if ( ad_enabled == 0 && ad_count[23] ) begin
                        ad_enabled <= 1;
                        ad_reset <= 1;  // turn on the ad_converter
        end
end


//////////////////////////////////////////////////////////////
//
//  			Clocks for TLV320AIC23B
//
/////////////////////////////////////////////////////////////

/*
	The Wolfson A/D is in Master mode and  provides 
	the source of all the clocks required by the TLV320AIC23B 
	which is in Slave mode.
*/

assign 	TX_BCLK = BCLK;
assign  TX_LRCLK = LRCLK;
assign  RX_INLRCLK = LRCLK; 

//////////////////////////////////////////////////////////////
//
//				State Machine to manage A/D converter
//
//////////////////////////////////////////////////////////////

/* 
	The code is required to send the sync bytes and contol bytes as well as 
	the A/D samples. 
	
	We need to loop through the following code 62 times. The first time though
	we generate the 3 sync bytes, 5 control bytes, 3 left bytes, 3 right bytes and 2 microphone 
	bytes for a total of (3 + 5 + 3 + 3 + 2) 16 bytes. Subsequent times through we do not generate 
	the sync or control bytes so generate ( 3 + 3 + 2) 8 bytes. 
	
	Since each frame is 512 bytes long we need to go through the loop 
	
	(512 - 16)/8 = 62 times
	
	since the counter is incremented before we read it we actually test for 
	the loop counter to be 63. 
	
	If PTT or MOX is active then Rx_control_0[0] = 1
	

*/

reg PTT; 	// holds the current state of the PTT button 

always @ (posedge AD_CLK)
begin
	sync_Rx_used[11:0] <= Rx_used_rdside; 		// kd5tfd 
	case(state_A_D)
	
// state 0 - loop until LRCLK goes low
	 3'h0: begin
			if (LRCLK) state_A_D <= 3'h0;		// wait here until LRCLK goes low 
			else state_A_D <= 3'h1;
		   end
			
// state 1 - loop until LRCLK goes high
	3'h1: if (LRCLK) 			  			// wait for A/D LRCLK to go high
		begin
		if (loop_counter == 63) loop_counter <= 0; 
			count <= 0;
			q <= q;
			if (Rx_control_0[0]) PTT <= 1'b1;	// check for PTT 
			else PTT <= 1'b0; 
			state_A_D <= 3'h2;
		end
	   else
		begin
		 	state_A_D <= 3'h1;
			count <= 0;
			q <= q;
			if (Rx_control_0[0]) PTT <= 1'b1;	// check for PTT 
			else PTT <= 1'b0; 
		if (loop_counter == 63) loop_counter <= 0; 
		end
		
	
// state 2 - left shift A/D data, check count and take action if necessary
	3'h2: if (!BCLK) state_A_D <= 3'h2; 			// wait for A/D BCLK clock to go high
		  else
		 begin
		Tx_q[7:0] <= {Tx_q[6:0], TX_DOUT};		// shift current Wolfson data left and add new data
		q[7:0] <= {q[6:0],DOUT};				// ditto for TLV 
						
		count <= count + 1'b1;

		if (count == 8 || count == 16  || count == 24 || count == 40 || count == 48  || count == 56)
		begin 
				register <= q;
				if (count == 8)
					Tx_MSB <= Tx_q;				// get MSB and LSB of mic audio
				else if (count == 16)
					Tx_LSB <= Tx_q;
				state_A_D <= 3'h3;				// strobe Tx_fifo
		end
		else if (loop_counter == 0 && count == 0)
		begin
				register <= 8'b10000000; 		// send 0x80 of sync word						
			 	state_A_D <= 3'h3;		 		// strobe Tx_fifo
		end	
		else if (loop_counter == 0 && (count == 1 || count == 2  || count == 4 || count == 5  ))
		begin 
				register <= 8'b00000000;		// send 0x00 of sync word						
				state_A_D <= 3'h3;				// strobe Tx_fifo
		end
		else if (loop_counter == 0 && count == 3)
		// set bit 0 in Control 0 if PTT or dot and bit 1 if dash
		begin
				register <= {register[7:2], ~clean_dash, (~clean_dot || ~clean_PTT)}; 
				rx_avail <= 12'd4095 - sync_Rx_used;  // kd5tfd added -- must match rx fifo size 
				state_A_D <= 3'h3;
		end	
		else if (loop_counter == 0 && count == 6)		// control 3 - send # of bytes in Rx fifo
		begin
				register[7:0] <= rx_avail[11:4];         // kd5tfd 
				state_A_D <= 3'h3;
		end			
		else if (loop_counter == 0 && count == 7)		// control 4 - return counter from PC 
		begin
				register <= Rx_control_4;
				state_A_D <= 3'h3;
		end
		else if (count == 58)					// send mic MSB 
				begin
				register <= Tx_MSB;	
				state_A_D <= 3'h3;				// strobe Tx_fifo
				end
		else if (count == 59)					// send mic LSB 
				begin	
				register <= Tx_LSB;	
				state_A_D <= 3'h3;				// strobe Tx_fifo
				end		
		else state_A_D <= 3'h4;
	end // end begin
		
// state 3 - set data flag to strobe data into Tx_fifo		
 	3'h3: begin 
			data_flag <= 1;						// create a one clock cycle wide pulse
			state_A_D <= 3'h4;  				// to let FX2 know that is data to send via USB
   	      end

// state 4 - reset data flag wait for BCLK to go low and either loop again or start over if counter = 63 
    3'h4: begin 
        	data_flag <= 0;						// clear data flag
            if (!BCLK) begin
               if (count == 63)begin
				 state_A_D <= 3'h0; 			// go and wait for LRCLK to be 0.
				 loop_counter <= loop_counter + 1'b1; 
				end
               else state_A_D <= 3'h2;
			end
            else state_A_D <= 3'h4;				// wait for BCLK to go low 
	   	  end 
	default: state_A_D <= 3'h0;					
	endcase 
end

///////////////////////////////////////////////////////////////
//
//				Tx_fifo - 512 Bytes
//
//////////////////////////////////////////////////////////////

Tx_fifo 	Tx_fifo(.wrclk (data_flag),.rdreq (1'b1),.rdclk (Tx_read_clock),.wrreq (1'b1),
				.data (register),.rdempty (Tx_rdempty), .wrempty (Tx_write_empty),
				.wrfull (Tx_write_full),.q (Tx_register), .wrusedw(write_used));
				
wire Tx_rdempty;				// High when Tx fifo empty
wire [7:0] Tx_register; 		// holds data from A/D to send to TFX2 
wire Tx_write_full;				
wire Tx_write_empty; 
reg  syncd_Tx_write_full;
reg  Tx_read_clock; 			// when goes high sends data to Tx_register
wire [8:0] write_used; 			// indicates how may bytes in the Tx buffer
reg  [8:0] syncd_write_used; 	// ditto but synced to FX2 clock 

//////////////////////////////////////////////////////////////
//
//				State Machine to manage FX2 USB interface
//
//////////////////////////////////////////////////////////////
/*
	The state machine checks if there are characters to be read
	in the FX2 Rx FIFO by checking 'fifo_data_available'  If set it loads the byte
	read into the Rx_register. On the next clock it  checks if the Tx_data_fag is set
	 - if so it sends the data 	to the Tx FIFO. After the Tx data has been sent 
	it checks 'fifo_data_available' etc.
*/

wire fifo_data_available = FX2_flags[0];	// goes high when FIFO 2 has data available 
wire fifo_ready = FX2_flags[2]; 	// high when we can write to FIFO 4
reg  [7:0] Rx_register;				// data from PC goes here
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
		else
		begin
			state_FX <= 3'h2;				 		// no Rx data so check for Tx data
		end 
		end
// state 1 - get Rx data 
  3'h1: begin
		FX2_SLRD <= 0; 								// set read strobe low
		Rx_register[7:0] <= FX2_FD[7:0];			// get data from FIFO
		state_FX <= 3'h2;
		end
// state 2 - check for Tx data - Tx fifo must be at least half full before we Tx
  3'h2:  begin
		 FX2_SLRD <= 1; 							// reset read strobe
            if (syncd_write_used[8] == 1'b1) begin  // data available, so let's start the xfer...
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

/////////////////////////////////////////////////////////////
//
//   Rx_fifo  (4096) Dual clock FIFO - Altera Megafunction (dcfifo)
//
/////////////////////////////////////////////////////////////

/* 
	The write clock of the FIFO is FX2_SLRD and the read clock BCLK.
	Data from the FX2_FIFO is written to the FIFO using FX2_SLRD. Data from the
	FIFO is read on the positive edge of BCLK when fifo_enable is high. The 
	FIFO is 4096 bytes long. 
	NB: The output flags are only valid after a read/write clock has taken place
*/
	

wire [7:0] Rx_data;
wire write_full;			 // high when tx side of fifo is full 
wire [11:0] Rx_used; 		// how many bytes in FX2 side Rx fifo
wire [11:0] Rx_used_rdside;  // read side count // kd5tfd added  

Rx_fifo	Rx_fifo(.wrclk (FX2_SLRD),.rdreq (fifo_enable),.rdclk (BCLK),.wrreq (1'b1),
		.data (Rx_register),.q (Rx_data), .wrfull (write_full),.wrusedw(Rx_used),
		.rdusedw(Rx_used_rdside)                  // kd5tfd added
		);

//////////////////////////////////////////////////////////////
//
//				State Machine to manage PWM interface
//
//////////////////////////////////////////////////////////////
/*
	The state machine changes state on the negative  edge of the BCLK pulse. 
	The code loops looking for a sync sequence (0x80_00_00) then stores the next 5 bytes 
	which are control bytes. It then concatenates  the next two bytes 
	(Left audio MSB and Left audio LSB) to form a 16 bit word and the next two bytes
	(Right audio MSB and Right audio LSB) to send to the 
	LTC320  D/A converters. The next two bytes are concatented to from the I data and the next 
	two the Q data. The I and Q data is sent to individual 16 bit PWM D/A converters. 
	
	
	The word sent to the converters must be send at the sample rate
	of the A/D converter so is synced on the positive edge of the LRCLK. Further reads
	of Rx_data are inhibited until the Rx FIFO has at least 16 bytes in it. 
	
	
	We need to loop through the following code 62 times. The first time though
	we look for the 3 sync bytes, 5 control bytes, 2 left bytes, 2 right bytes,  2 I bytes and 2 Q  
	bytes for a total of (3 + 5 + 2 + 2 + 2 + 2) 16 bytes. Subsequent times through we do not look for
	the sync or control bytes so receive ( 2 + 2 + 2 + 2) 8 bytes. 
	
	Since each frame is 512 bytes long we need to go through the loop 
	
	(512 - 16)/8 = 62 times
	
	since the counter is incremented before we read it we actually test for 
	the byte counter to be 63. 
	
*/

reg [4:0] state_PWM;			// state for PWM  counts 0 to 18  
reg [15:0] Left_PWM; 			// Left 16 bit PWM data for D/A converter
reg [15:0] Right_PWM;			// Right 16 bit PWM data for D/A converter
reg [15:0] I_PWM;				// I 16 bit PWM data for D/A conveter
reg [15:0] Q_PWM;				// Q 16 bit PWM data for D/A conveter
reg fifo_enable;				// controls reading of dual clock fifo
reg [11:0] synced_Rx_used; 		// how may bytes in FX2 side Rx fifos synced to BCLK
reg [6:0] byte_count;			// counts number of times round loop
reg [7:0] Rx_control_0; 		// control 0 from PC, PTT or MOX active if bit 0 set 
reg [7:0] Rx_control_1; 		// control 1 from PC
reg [7:0] Rx_control_2; 		// control 2 from PC
reg [7:0] Rx_control_3; 		// control 3 from PC
reg [7:0] Rx_control_4; 		// control 4 from PC



always @ (negedge BCLK)
begin
	synced_Rx_used <= Rx_used;					// sync Rx_used to BCLK
   case(state_PWM)
  0: begin
		byte_count <= 0;
		fifo_enable <= 1'b1;						// enable read of dual clock fifo
		if (Rx_data == 8'h80) state_PWM <= 1; 		// have start of sync
		else state_PWM <= 0;						// loop here if not
		end
// state 1 - check for 0x00 of sync character
  1: begin
		if (Rx_data == 8'h00) state_PWM <= 2; 		// have middle of sync
		else state_PWM <= 0;						// not sync, restart
		end
		
// state 2 - check for 0x00 of sync character
  2: begin
		if (Rx_data == 8'h00) state_PWM <= 3; 		// have end  of sync
		else state_PWM <= 0;						// not sync, restart
		end		
		
// state 3 - We have sync so get Rx_control_0 
  3: begin
		Rx_control_0 <= Rx_data;
		state_PWM <= 4; 
		end	
		
// state 4 - get Rx_control_1
  4: begin
		Rx_control_1 <= Rx_data;
		state_PWM <= 5; 	
		end	

// state 5 - get Rx_control_2
  5: begin
		Rx_control_2 <= Rx_data;
		state_PWM <= 6; 	
		end	

// state 6 - get Rx_control_3
  6: begin
		Rx_control_3 <= Rx_data;
		state_PWM <= 7; 	
		end	

// state 7- get Rx_control_4
  7: begin
		Rx_control_4 <= Rx_data;
		state_PWM <= 8; 	
		end	
		
// state 8 - get MSB of Left audio
  8: begin
		fifo_enable <= 1'b1;						// enable Rx_fifo
		Left_PWM[15:8] <= Rx_data[7:0];
		state_PWM <= 9;
		end
// state 9 - get LSB of Left audio amd combine with MSB to form 16 bit PWM data 
  9: begin
		Left_PWM[7:0] <= Rx_data[7:0];
		state_PWM <= 10;	
		end
// state 10 - get MSB of Right audio
  10: begin
		Right_PWM[15:8] <= Rx_data[7:0];
		state_PWM <= 11;
		end
// state 11 - get LSB of Right audio and combine with MSB to form 16 bit PWM data 
  11: begin
		Right_PWM[7:0] <= Rx_data[7:0];
		state_PWM <= 12;							
		end
		
// state 12 - get MSB of I audio 
  12: begin
		I_PWM[15:8] <= Rx_data[7:0];
		state_PWM <= 13;
		end
		
// state 13 - get LSB of I audio and combine with MSB to form 16 bit PWM data 
  13: begin
		I_PWM[7:0] <= Rx_data[7:0];
		state_PWM <= 14;							
		end
		
// state 14 - get MSB of Q audio 
  14:	begin
		Q_PWM[15:8] <= Rx_data[7:0];
		state_PWM <= 15;
		end
		
// state 15 - get LSB of Q audio and combine with MSB to form 16 bit PWM data 
  15: begin
		Q_PWM[7:0] <= Rx_data[7:0];
		state_PWM <= 16;							
		end

// state 16 - check that LRCLK is low 
  16: begin
		fifo_enable <= 1'b0;		    // disable read of dual clock fifo
		if (LRCLK) begin 
			state_PWM <= 16;		// wait for A/D LRCLK to go low so we are in sync

		end
	    else state_PWM <= 17;
	    end	
// state 17 - wait for leading edge of LRCLK
  17: begin
		if (LRCLK) 			  			// wait for A/D LRCLK to go high so we are in sync
		begin
			byte_count <= byte_count + 1'b1;
			I_Data[15:0] <=  I_PWM[15:0];  	// set up I D/A data 
			Q_Data[15:0] <=  Q_PWM[15:0];  	// set up Q D/A data 
			state_PWM <= 18;  
		end
		else state_PWM <= 17;
		end
// state 18 - wait until Rx fifo has at least 16 bytes in it before restarting
  18: begin
		if (synced_Rx_used > 15)
		begin
			if(byte_count == 63) state_PWM <= 0; // restart if we have done 62 loops
			else state_PWM <= 8;
		end 
		else state_PWM <= 18; 					// wait until we have 16 bytes in the fifo
		end
	default: state_PWM <= 4'h0;
	endcase
end

/////////////////////////////////////////////////////////////////
//
// Single bit PWM 16 bit D/A converters
//
/////////////////////////////////////////////////////////////////

reg [15:0] I_Data;
reg [15:0] Q_Data;
reg [15:0] I_Data_in;
reg [15:0] Q_Data_in;
reg [16:0] I_PWM_accumulator;
reg [16:0] Q_PWM_accumulator;


always @(negedge BCLK)
begin
	I_Data_in <= I_Data + 16'h8000;		// so that 0 in gives 50:50 mark/space 
	Q_Data_in <= Q_Data + 16'h8000;		
 	I_PWM_accumulator <= I_PWM_accumulator[15:0] + I_Data_in;
 	Q_PWM_accumulator <= Q_PWM_accumulator[15:0] + Q_Data_in;

end

assign I_PWM_out = I_PWM_accumulator[16];	// send to FPGA pins
assign Q_PWM_out = Q_PWM_accumulator[16];

///////////////////////////////////////////////////////////////
//
//		Implements left justified digital audio out,
//		16 bits, two channels  for TLV320AIC23B D/A converter 
//
///////////////////////////////////////////////////////////////


LeftJustifiedAudioOut  LJAO(.lrclk_i(TX_LRCLK), .bclk_i(TX_BCLK), .left_sample_i(Left_PWM), .right_sample_i(Right_PWM),.outbit_o(RX_DIN)); 



///////////////////////////////////////////////////////
//
//			Debounce PTT button 
//
///////////////////////////////////////////////////////

 wire clean_PTT; // debounced button 

debounce de_PTT(.clean_pb(clean_PTT), .pb(button), .clk(FX2_CLK));


///////////////////////////////////////////////////////
//
//			Debounce dot key
//
///////////////////////////////////////////////////////

 wire clean_dot; // debounced dot 

debounce de_dot(.clean_pb(clean_dot), .pb(dot), .clk(FX2_CLK));


///////////////////////////////////////////////////////
//
//			Debounce dash key 
//
///////////////////////////////////////////////////////

 wire clean_dash; // debounced dash 

debounce de_dash(.clean_pb(clean_dash), .pb(dash), .clk(FX2_CLK));


 
		
///////////////////////////////////////////////////////
//
//				RS232 Transmitter (for debug!)
//
///////////////////////////////////////////////////////

/*
	Pin 68 is used as Tx data out to the PC at 115kB. Data to be send 
	is loaded into outputdata and send when TxD_start goes high.
	Can use Rx_data_flag and Rx_register to display what comes from RX FIFO
*/
/*
parameter ClkFrequency = 24000000;	// make sure Xylo is set to this!

wire [7:0] outputdata = RxD_data;

async_transmitter TX(.clk(FX2_CLK), .TxD(TxD), .TxD_start(TxD_start), .TxD_data(outputdata));
defparam TX.ClkFrequency = ClkFrequency;

*/

// FX2_FD is tristate when write is hi, otherwise it's the Tx_register value.

assign FX2_FD[7:0] = FX2_SLWR ? 8'bZ : Tx_register[7:0];

// Flash the LEDs to show something is working!

//assign LED[0] = Rx_control_0[0]; // C&C test, simulates PTT. Red LED ON when button pressed.

assign LED[0] = write_full; // Red LED lights when Rx fifo is full. 
assign LED[1] = FX2_flags[2];	// Green LED on when we can write to FX2 FIFO

endmodule

