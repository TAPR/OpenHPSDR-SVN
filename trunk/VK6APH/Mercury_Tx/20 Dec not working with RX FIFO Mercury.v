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
	10 Dec  2006 - Ported to Ozy V2 hardware
	12 Dec  2006 - Added PWM DAC for audio out
		
	
*/
	


/////////////////////////////////////////////////////////////
//
//	Pins
//
//  LT2208		Pin		Ozy-Atlas	FPGA Pin
//				1		
//	    CLK		3		C19			179
//		Ov		5		
//		D15		7		C17			175
//		D14		9		C16			173
//		D13		11		C15			171
//		D12		13		C14			170
//		D11		15		C13			169
//		D10		17		C12			168
//		D9		19		C11			165
//		D8		21		C10			164
//		D7		13		C9			163
//		D6		25		C8			162
//		D5		27		C7			161
//		D4		29		C6			160
//		D3		31		C5			152
//		D2		33		C4			151
//		D1		35		C3			150
//		D0		37		C2			149
//		3.3v	39	
//
//		I_PWM_out		C20			180
//		Q_PWM_out		C21			181

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
//	 DEBUG_LED0		- 4 
//	 DEBUG_LED1		- 33
//	 DEBUG_LED2		- 34
//	 DEBUG_LED3		- 108
//
////////////////////////////////////////////////////////////


module Mercury(clock, ADC,IFCLK, FX2_FD,FIFO_ADR, SLRD, SLWR, SLOE, FLAGA, FLAGC, PKEND, DEBUG_LED0,DEBUG_LED1, DEBUG_LED2, DEBUG_LED3,I_PWM_out,Q_PWM_out );

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
output DEBUG_LED0;			// LEDs on OZY board
output DEBUG_LED1;
output DEBUG_LED2;
output DEBUG_LED3;
output SLOE;				// FX2 data bus enable - active low
output I_PWM_out;			// PWM ADC output
output Q_PWM_out;			// PWM ADC output



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

assign temp_ADC = ADC;

// A Digital Output Randomizer is fitted to the LT2208. This complements bits 15 to 1 if 
// bit 0 is 1. This helps to reduce any pickup by the A/D input of the digital outputs. 
// We need to de-ramdomize the LT2208 data if this is turned on. 

//assign temp_ADC = ADC[0]? {~ADC[15:1],ADC[0]}: ADC;  // Compensate for Digital Output Randomizer

// The randomizer seems to work but at -40dBm input there is a significant increase in the
// level of the baseline noise. Perhaps and earthing issue. Leave off for the time being. 


//////////////////////////////////////////////////////////////
//
//		CLOCKS
//
//
//////////////////////////////////////////////////////////////

// Generate ~48kHz clock for PWM ADC

reg PWM_clock;
reg [10:0]PWM_count;
always @ (posedge clock)
begin
	if (PWM_count == 1041) // divide 100MHz clock by 1042 to give 47.98kHz
		begin
		PWM_clock <= ~PWM_clock;
		PWM_count <= 0;
		end
	else PWM_count <= PWM_count + 11'b1;
end



//////////////////////////////////////////////////////////////
//
//		CORDIC NCO 
//
//
//////////////////////////////////////////////////////////////

//Code rotates A/D input at set frequency  and produces I & Q /

// IMPORTANT: set Iin to be 16'd0 since we only have a single input 

wire [15:0]i_out;
wire [15:0]q_out;
wire [15:0]temp_ADC;

	//parameter FREQ = 32'h40000000;  //~25MHz  i.e. FREQ /(100e6/2^32)
	//parameter FREQ = 32'h24538EF3; // 14.190MHz
	wire	[31:0] phase;

	// The phase accumulator takes a 32 bit frequency dword and outputs a 32 bit phase dword on each clock
	phase_accumulator rx_phase_accumulator(.clk(clock),.reset(~clk_enable),.frequency(frequency),.phase_out(phase));

	// The cordic takes I and Q in along with the top 18 bits of the phase dword.  The I and Q out are freq shifted
	cordic rx_cordic(.clk(clock),.reset(~clk_enable),.Iin(16'd0),.Qin(temp_ADC),.PHin(phase[31:16]),.Iout(i_out),.Qout(q_out),.PHout());




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

/////////////////////////////////////////////////////////////
//
//   Rx_fifo  (4096) Dual clock FIFO - Altera Megafunction (dcfifo)
//
/////////////////////////////////////////////////////////////

/*
        The write clock of the FIFO is SLRD and the read clock BCLK.
        Data from the FX2_FIFO is written to the FIFO using SLRD. Data from the
        FIFO is read on the positive edge of BCLK when fifo_enable is high. The
        FIFO is 4096 words long.
        NB: The output flags are only valid after a read/write clock has taken place
*/


wire [15:0] Rx_data;
wire write_full;                         // high when tx side of fifo is full
wire [11:0] Rx_used;             // how many bytes in FX2 side Rx fifo
wire [11:0] Rx_used_rdside;  // read side count

Rx_fifo Rx_fifo(.wrclk (SLRD),.rdreq (fifo_enable),.rdclk (clock_8),.wrreq (1'b1),
                .data (Rx_register),.q (Rx_data), .wrfull (write_full),.wrusedw(Rx_used),
                .rdusedw(Rx_used_rdside));

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
//                              State Machine to manage PWM interface
//
//////////////////////////////////////////////////////////////
/*
        The state machine changes state on the negative edge of BCLK.
        The code loops until there are at least 1024 bytes in the Rx_FIFO.
        The code then loops looking for a sync sequence (0x7F7F7F). Once located it
        sleeps for 512 bytes (256 words) and then looks for the sync sequence again. If located
        it contiunes, if not it restarts.

        Whilst sync is being detected,or restarted, then all logic outputs are set to safe values.

        After successfully finding sync it  reads the next 5 bytes
        which are control bytes C0-C4. The next word is the Left audio and the following the
        Right audio which are sent to the TLV320 D/A converters.
    	The next worid is the  I data and the following the Q data.
    	The I and Q data is sent to individual 16 bit PWM D/A converters.

        The words sent to the D/A converters must be sent at the sample rate
        of the A/D converters (48kHz) so is synced on the positive edge of the CLRCLK. Further reads
        of Rx_data are inhibited until the Rx FIFO has at least 16 bytes in it.

        We need to loop through the following code 62 times. The first time though
        we look for the 3 sync bytes, 5 control bytes, 2 left bytes, 2 right bytes,  2 I bytes and 2 Q
        bytes for a total of (3 + 5 + 2 + 2 + 2 + 2) 16 bytes. Subsequent times through we do not look for
        the sync or control bytes so receive ( 2 + 2 + 2 + 2) 8 bytes.

        Since each frame is 512 bytes long we need to go through the loop

        (512 - 16)/8 = 62 times

        since the counter is incremented before we read it we actually test for
        the byte counter to be 63.

        At the end of the loop the next 3 bytes should be the sync sequence (state 3).
        If the sync sequence is found then we continue otherwise we revert to the beginning
        (state 0) and try to gain sync again.

        Note that we use BCLK to read the Rx_FIFO. This is so we can remove data from it quickly. We then wait for
        the 48kHz CLRCLK so the received data is available at the correct time for the DACs.

*/

reg [4:0] state_PWM;                    // state for PWM  counts 0 to 13
reg [15:0] Left_PWM;                    // Left 16 bit PWM data for D/A converter
reg [15:0] Right_PWM;                   // Right 16 bit PWM data for D/A converter
reg [15:0] I_PWM;                       // I 16 bit PWM data for D/A conveter
reg [15:0] Q_PWM;                       // Q 16 bit PWM data for D/A conveter
reg fifo_enable;                        // controls reading of dual clock fifo
reg [11:0] synced_Rx_used;              // how may bytes in FX2 side Rx fifos synced to BCLK
reg [6:0] byte_count;                   // counts number of times round loop
reg [7:0] Rx_control_0;                 // control C0 from PC, MOX active if bit 0 set
reg [7:0] Rx_control_1;                 // control C1 from PC, decode bits 1,0 for A/D speed setting
reg [7:0] Rx_control_2;                 // control C2 from PC
reg [7:0] Rx_control_3;                 // control C3 from PC
reg [7:0] Rx_control_4;                 // control C4 from PC
reg [8:0]sync_count;
reg have_sync;                          // high when we have sync
reg [31:0]frequency;


always @ (negedge clock_8)
begin
        synced_Rx_used <= Rx_used;      // sync Rx_used to BCLK since this runs of FX2 clock
case(state_PWM)
  0: begin
// IMPORTANT:  We are looking for, or have lost sync, at this point - use this state to set all
// outputs etc to a safe value.
                have_sync <= 1'b0;                                                              // turn sync LED off
                if(synced_Rx_used > 1023)begin                                  // wait until we have at lease 1024 bytes to check
                        byte_count <= 0;                                                        // reset byte count
                        fifo_enable <= 1'b1;                                            // enable read of dual clock fifo
                        sync_count <= 0;                                                        // reset sync counter
                        if (Rx_data == 16'h7F7F)begin                           // have start of sync
                                sync_count <= sync_count + 9'b1;
                                state_PWM <= 1;
                        end
                        else state_PWM <= 0;                                            // no sync so look again
                end     else begin
                        fifo_enable <= 1'b0;                                            // prevent FIFO reads until we have >1023 available
                        state_PWM <= 0;                                                         // loop here if not
                end
         end
// state 1 - check for 0x7F  sync character
  1: begin
                if (Rx_data[15:8] == 8'h7F)begin                                // have middle of sync
                        sync_count <= sync_count + 9'b1;
                        state_PWM <= 2;
                end
                else state_PWM <= 0;                                                    // not sync, restart
         end

// state 2 - loop until the next sync is due and check we receive it
  2: begin
                if(sync_count == 255)begin  // we are counting words so this is actually 511 bytes
                        state_PWM <= 3;
                end
                else begin
                sync_count <= sync_count + 9'b1;
                state_PWM <= 2;
                end
         end
// state 3 - look for sync again - if true continue else start again
// Once in sync this is where the code re-enters.
  3: begin
                byte_count <= 0;                                                        // reset byte count
                fifo_enable <= 1'b1;                                            // enable read of dual clock fifo
                if (Rx_data == 16'h7F7F) state_PWM <= 4;
                else state_PWM <= 0;
         end
// state 4 - look for sync again - if true continue else start again
  4: begin
                if (Rx_data[15:8] == 8'h7F)begin
                        Rx_control_0 <= Rx_data[7:0];                   // We have sync so get Rx_control_0
                        have_sync <= 1'b1;                                      // toggle sync LED.
                        state_PWM <= 5;
                end
                else state_PWM <= 0;
         end
// state 5 - get Rx_control_1 & Rx_control_2
  5: begin
                Rx_control_1 <= Rx_data[15:8];
                Rx_control_2 <= Rx_data[7:0];
                state_PWM <= 6;
        end
// state 6 - get Rx_control_3 & Rx_control_4
  6: begin
                Rx_control_3 <= Rx_data[15:8];
                Rx_control_4 <= Rx_data[7:0];
                state_PWM <= 7;
                end
// state 7 - get Left audio and frequency 
  7: begin
                if ( Rx_control_0[7:1] == 7'b0000_001) begin 
					frequency <= {Rx_control_1,Rx_control_2,Rx_control_3,Rx_control_4};
				end
				fifo_enable <= 1'b1;       // enable Rx_fifo
                Left_PWM <= Rx_data;
                state_PWM <= 8;
                end
// state 8 - get Right audio
  8: begin
                Right_PWM <= Rx_data;
                state_PWM <= 9;
                end
// state 9 - get I audio
  9: begin
                I_PWM <= Rx_data;
                state_PWM <= 10;
                end
// state 10 - get Q audio
  10:   begin
                Q_PWM <= Rx_data;
                state_PWM <= 11;
                end
// state 11 - check that LRCLK is low
  11: begin
                fifo_enable <= 1'b0;                                    // disable read of Rx_FIFO whilst we wait
                if (PWM_clock) state_PWM <= 11;                            // wait for A/D LRCLK to go low so we are in sync
            else state_PWM <= 12;
           end
// state 12 - wait for positive edge of LRCLK
  12: begin
                if (PWM_clock)                                                           // wait for A/D LRCLK to go high so we are in sync
                begin
                        byte_count <= byte_count + 1'b1;
                        I_Data[15:0] <=  Left_PWM[15:0];                   // set up I D/A data
                        Q_Data[15:0] <=  Right_PWM[15:0];                   // set up Q D/A data
                        state_PWM <= 13;
                end
                else state_PWM <= 12;
          end
// state 21 - wait until Rx FIFO has at least 16 bytes in it before restarting
  13: begin
                if (synced_Rx_used > 15)
                begin
                        if(byte_count == 63) state_PWM <= 3; // look for sync if we have done 62 loops
                        else state_PWM <= 7;
                end
                else state_PWM <= 13;                                   // wait until we have 16 bytes in the fifo
          end
        default: state_PWM <= 4'h0;
        endcase
end

assign frequency = 32'h24538EF3;  //14.190MHz


/*
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

*/
	
	
/////////////////////////////////////////////////////////////////
//
// Single bit PWM 16 bit D/A converters
//
/////////////////////////////////////////////////////////////////

// This runs off the 100MHz clock to simplify the LPF requirements.

reg [15:0] I_Data;
reg [15:0] Q_Data;
reg [15:0] I_Data_in;
reg [15:0] Q_Data_in;
reg [16:0] I_PWM_accumulator;
reg [16:0] Q_PWM_accumulator;


always @(negedge clock)
begin
        I_Data_in <= I_Data + 16'h8000;         // so that 0 in gives 50:50 mark/space
        Q_Data_in <= Q_Data + 16'h8000;
        I_PWM_accumulator <= I_PWM_accumulator[15:0] + I_Data_in;
        Q_PWM_accumulator <= Q_PWM_accumulator[15:0] + Q_Data_in;

end

assign I_PWM_out = I_PWM_accumulator[16];       // send to FPGA pins
assign Q_PWM_out = Q_PWM_accumulator[16];


// LEDs for testing

assign DEBUG_LED0 = ~have_sync; 	// LED 0 on when we have sync	
assign DEBUG_LED1 = ~EP6_ready;		// LED D3 on when we can write to EP6
assign DEBUG_LED2 = data_ready; 
assign DEBUG_LED3 = ~EP2_has_data; // LED on when we receive data 
//assign DEBUG_LED3 = PWM_clock;


endmodule 