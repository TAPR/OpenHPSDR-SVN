// V0.6 9th February 2007
//
// Copyright 2007 Phil Harman VK6APH
//
//  HPSDR - High Performance Software Defined Radio
//
//
//  Mercury Transmitter.
//
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA



/***********************************************************
*
*	Test program for AD9744
*
************************************************************/


/* 	This program interfaces the AD9744 ADC to PowerSDR over USB.
	The data to the AD9744  is in 14 bit parallel format and 
	is sent at the negative edge of the LT2208 100MHz clock.
	
	The ~48kHz I and Q data from PowerSDR is interpolated by 2048 in a CIC to 
	give a data rate of 100MHz. 
	
	The data is processed by a CORDIC NCO to produce I and Q
	outputs.  These are then subtracted to remove the unwanted sideband
	and passed to the AD9744.
	
	In order to use PowerSDR dummy data at ~48kHz is sent via the FX2.
	
	The data is sent and received  in the Janus format vis:-
	
	<7F7F><7F00><0000><FF00><I MSB,I ><I LSB,Q MSB><Q,Q LSB><0000> etc 
	
	Change log:
	
	11 Jan  2007 - Started transmitter code
	13 Jan  2007 - first code working
	21 Jan  2007 - SSB version working
	22 Jan  2007 - Start of ERR version 
	4  Feb  2007 - Moved to AD9744 DAC
	9  Feb  2007 - Testing completed, Penelope started
	26 Mar  2007 - Modified to use 125MHz clock 
	 2 Apr  2007 - Removed adder on output of CORDIC  
		
	
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

//    DAC
//		D13		1		A2			147
//		D12		2		A3			146
//		D11		3		A4			145
//		D10		4		A5			144
//		D9		5		A6			143
//		D8		6		A7			142
//		D7		7		A8			141
//		D6		8		A9			139
//		D5		9		A10			138
//		D4		10		A11			137
//		D3		11		A12			135
//		D2		12		A13			134
//		D1		13		A14			133
//		D0		14		A15			128
//		Clock   28		A16         127



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
//	 FPGA_GPIO1		- 67
//	 FPGA_GPIO2		- 68
//	 FPGA_GPIO3		- 69
//	 FPGA_GPIO4		- 70
//	 FPGA_GPIO5		- 72
//	 FPGA_GPIO6		- 74
//	 FPGA_GPIO7		- 75
//	 FPGA_GPIO8		- 76
//	 FPGA_GPIO9		- 77
//	 FPGA_GPIO10	- 80
//	 FPGA_GPIO11	- 81
//	 FPGA_GPIO12	- 82
//	 FPGA_GPIO13	- 84
//	 FPGA_GPIO14	- 86
//	 FPGA_GPIO15	- 87
//	 FPGA_GPIO16	- 88
//
////////////////////////////////////////////////////////////


module Mercury(clock, ADC,IFCLK, FX2_FD,FIFO_ADR, SLRD, SLWR, SLOE, FLAGA, FLAGC, PKEND,
	   DEBUG_LED0,DEBUG_LED1, DEBUG_LED2, DEBUG_LED3,DAC,DAC_CLOCK,
		 FPGA_GPIO1, FPGA_GPIO2, FPGA_GPIO3, FPGA_GPIO4, FPGA_GPIO5, FPGA_GPIO6,
		 FPGA_GPIO7, FPGA_GPIO8, FPGA_GPIO9, FPGA_GPIO10, FPGA_GPIO11,
		 FPGA_GPIO12, FPGA_GPIO13, FPGA_GPIO14, FPGA_GPIO15, FPGA_GPIO16);

input [15:0]ADC;			// samples from LT2208
input clock;				// 125MHz clock from LT2208
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
output FPGA_GPIO1;
output FPGA_GPIO2;
output FPGA_GPIO3;		
output FPGA_GPIO4;	
output FPGA_GPIO5;	
output FPGA_GPIO6;	
output FPGA_GPIO7;	
output FPGA_GPIO8;		
output FPGA_GPIO9;		
output FPGA_GPIO10;	
output FPGA_GPIO11;
output FPGA_GPIO12;	
output FPGA_GPIO13;	
output FPGA_GPIO14;	
output FPGA_GPIO15;	
output FPGA_GPIO16;
output [13:0]DAC;
output DAC_CLOCK; 



wire PKEND;
reg [3:0] state_FX;			// state for FX2
reg data_flag;				// set when data ready to send to Tx FIFO
reg [15:0] register;		// LT2208 A/D uses this to send its data to Tx FIFO
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
wire [13:0]DAC;


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


//////////////////////////////////////////////////////////////
//
//		CLOCKS
//
//
//////////////////////////////////////////////////////////////

// Generate ~48kHz clock for PWM 

reg PWM_clock;
reg [10:0]PWM_count;
always @ (posedge clock)
begin
	if (PWM_count == 1079) // divide 125MHz clock by 1280 to give 48.828kHz
		begin
		PWM_clock <= ~PWM_clock;
		PWM_count <= 0;
		end
	else PWM_count <= PWM_count + 11'b1;
end

// sync frequecy change to 125MHz clock
reg [31:0]sync_frequency;

always @ (posedge clock)
begin
	sync_frequency <= frequency;
end

// send data to DAC on negative edge of 125MHz clock

assign DAC_CLOCK = ~clock;


// latch I & Q data into  CIC when ce_out_x goes high. 

reg [15:0]cic_i;
reg [15:0]cic_q;

always @ (posedge ce_out_i)
begin
	cic_i <= I_PWM;
end 

always @ (posedge ce_out_q)
begin
	cic_q <= Q_PWM;
end 
	
////////////////////////////////////////////////////////////////
//
//  Interpolating CIC filter  R = 2560 N = 5
//
////////////////////////////////////////////////////////////////

wire [15:0]cic_out_i;
wire [15:0]cic_out_q;
wire ce_out_i;				// narrow pulse when data required
wire ce_out_q;				// narrow pulse when data required

cicint cic_I( .clk(clock),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(cic_i),.filter_out(cic_out_i),.ce_out(ce_out_i));
cicint cic_Q( .clk(clock),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(cic_q),.filter_out(cic_out_q),.ce_out(ce_out_q));


//////////////////////////////////////////////////////////////
//
//		CORDIC NCO 
//
//
//////////////////////////////////////////////////////////////

//Code rotates input at set frequency  and produces I & Q /

wire [15:0]i_out;
wire [15:0]q_out;
wire [15:0]temp_ADC;
wire [31:0] phase;
reg  [31:0]frequency;

wire [16:0]q_temp;
wire [16:0]i_temp;

	
//assign sync_frequency = 32'h24538EF3; // 14.190MHz i.e. FREQ /(100e6/2^32)

// The phase accumulator takes a 32 bit frequency dword and outputs a 32 bit phase dword on each clock
phase_accumulator rx_phase_accumulator(.clk(clock),.reset(~clk_enable),.frequency(sync_frequency),.phase_out(phase));

// The cordic takes I and Q in along with the top 15 bits of the phase dword.  The I and Q out are freq shifted
cordic rx_cordic(.clk(clock),.reset(~clk_enable),.Iin(cic_out_i),.Qin(cic_out_q),.PHin(phase[31:16]),.Iout(i_out),.Qout(q_out),.PHout());

/* 
	We can use either the I or Q output from the CORDIC directly to drive the DAC.

   	exp(jw) = cos(w) + j sin(w)

	When multplying two complex sinusoids f1 and f2, you get only f1+f2, no
	difference frequency.

  		Z = exp(j*f1) * exp(j*f2) = exp(j*(f1+f2))
    	  = cos(f1 + f2) + j sin(f1 + f2)
*/

assign DAC[13:0] = i_out[15:2];  // use q_out if 90 degree phase shift required by EER Tx etc

//////////////////////////////////////////////////////////////////////
//
//		Sync and data interface
//
//////////////////////////////////////////////////////////////////////

// we can use a slower clock for sending dummy data to PowerSDR, say 6.25MHz


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
0:	begin
		if (PWM_clock)AD_state <= 0;		// loop until 48kHz clock is low
		else AD_state <= 5'd1;
	end
1:	begin
	if (PWM_clock)							// loop until 48kHz clock is high
	begin
		if (loop_counter == 0)				// if zero  then send sync and C&C bytes
		begin
			register <= 16'h7F7F;	
			strobe <= 1'b1;					// strobe start if sync (80) into Tx FIFO
			rx_avail <= 12'd4095 - sync_Rx_used;    
			AD_state <= 5'd2;
		end
		else AD_state <= 5'd5;
	end
	else AD_state <= 1; 
	end
2:	begin
		register <= 16'h7F00;				// rest of sync and C0 set to 0
		strobe <= 1'b1;
		AD_state <= AD_state + 1'b1;
	end	
3: begin
		register <= 16'h0000;				// C1 and C2 set to 0
		strobe <= 1'b1;
		AD_state <= AD_state + 1'b1;
	end
4: begin
		register <= {rx_avail[11:4],8'b0};     // C3 - number of bytes free in Rx FIFO, C4 - sequence number
		strobe <= 1'b1;
		AD_state <= AD_state + 1'b1;
	end		
5:	begin
		register <= 0;						// dummy I and Q data for PowerSDR
		strobe <= 1'b1;
		AD_state <= AD_state + 1'b1;
	end
6:	begin
		register <= 0;						// last 8 bits of I and first 8 of Q			  		
		strobe <= 1'b1; 
		AD_state <= AD_state + 1'b1;
	end
7:	begin
		register <= 0;			// send Q data
		strobe <= 1'b1; 
		AD_state <= AD_state + 1'b1;
	end
8: begin
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
wire write_full;                         // high when tx side of fifo is full
wire read_full;
wire [11:0] Rx_used;             		// how many bytes in FX2 side Rx fifo
wire [11:0] Rx_used_rdside;  			// read side count

Rx_fifo Rx_fifo(.wrclk (SLRD),.rdreq (fifo_enable),.rdclk (clock_8),.wrreq (1'b1),
                .data (Rx_register),.q (Rx_data), .wrfull (write_full),.wrusedw(Rx_used),
                .rdusedw(Rx_used_rdside), .rdfull(read_full));

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
//                              State Machine to manage PWM interface
//
//////////////////////////////////////////////////////////////
/*
        The state machine changes state on the negative edge of clock_8.
        The code loops until there are at least 1024 bytes in the Rx_FIFO.
        The code then loops looking for a sync sequence (0x7F7F7F). 

        After successfully finding sync it  reads the next 5 bytes
        which are control bytes C0-C4. The next word is the Left audio and the following the
        Right audio which are sent to the TLV320 D/A converters.
    	The next word is the  I data and the following the Q data.
    	The I and Q data is sent to the interpolating CIC filter and the the NCO. 

        The words sent to the D/A converters must be sent at the sample rate
        of the A/D converters (48kHz) so is synced on the positive edge of the PWM_clock. Further reads
        of Rx_data are inhibited until the Rx FIFO has at least 8 bytes in it.

        We need to loop through the following code 62 times. The first time though
        we look for the 3 sync bytes, 5 control bytes, 2 left bytes, 2 right bytes,  2 I bytes and 2 Q
        bytes for a total of (3 + 5 + 2 + 2 + 2 + 2) 16 bytes. Subsequent times through we do not look for
        the sync or control bytes so receive ( 2 + 2 + 2 + 2) 8 bytes.

        Since each frame is 512 bytes long we need to go through the loop

        (512 - 16)/8 = 62 times

        since the counter is incremented before we read it we actually test for
        the byte counter to be 63.

        At the end of the loop the next 3 bytes should be the sync sequence (state 3).

        Note that we use clock_8 to read the Rx_FIFO. This is so we can remove data from it quickly. We then wait for
        the 48kHz PWM_clock so the received data is available at the correct time for the DACs.
		Also note that the NEGATIVE edge of clock_8 is used so the FIFO data is stable when we read it
		since it is written on the positive edge.

*/

reg [4:0] state_PWM;                    // state for PWM  counts 0 to 13
reg [15:0] Left_PWM;                    // Left 16 bit PWM data for D/A converter
reg [15:0] Right_PWM;                   // Right 16 bit PWM data for D/A converter
reg [15:0] I_PWM;                       // I 14 bit PWM data for transmitter
reg [15:0] I_PWM_temp;
reg [15:0] Q_PWM;                       // Q 14 bit PWM data for transmitter
reg [15:0] Q_PWM_temp;
reg fifo_enable;                        // controls reading of dual clock fifo
reg [11:0] synced_Rx_used;              // how may bytes in FX2 side Rx fifos synced to clock_8
reg [6:0] byte_count;                   // counts number of times round loop
reg [7:0] Rx_control_0;                 // control C0 from PC, MOX active if bit 0 set
reg [7:0] Rx_control_1;                 // control C1 from PC, decode bits 1,0 for A/D speed setting
reg [7:0] Rx_control_2;                 // control C2 from PC
reg [7:0] Rx_control_3;                 // control C3 from PC
reg [7:0] Rx_control_4;                 // control C4 from PC
reg [8:0]sync_count;
reg have_sync;                          // toggles each time we get sync


always @ (negedge clock_8)
begin
synced_Rx_used <= Rx_used;      
case(state_PWM)
  0: begin
	byte_count <= 0;
    if(synced_Rx_used > 1023)begin       // wait until we have at least 1024 words to check
        fifo_enable <= 1'b1;             // enable read of dual clock fifo
        if (Rx_data == 16'h7F7F)begin    // have start of sync
            state_PWM <= 1;
        end
        else state_PWM <= 0;             // no sync so look again
        end
	 else begin
     	fifo_enable <= 1'b0;            // prevent FIFO reads until we have >1023 available
        state_PWM <= 0;                 // loop here if not
     end
     end
// state 1 - look for balance of sync  - if true continue else start again
  1: begin
     if (Rx_data[15:8] == 8'h7F)begin
		Rx_control_0 <= Rx_data[7:0];	// get Rx_control 0
        have_sync <= ~have_sync;        // toggle sync  
        state_PWM <= 2;
     end
     else state_PWM <= 0;
     end
// state 2 - Check if we are sending NCO frequency, if so get it
  2: begin
		if (Rx_control_0[7:1] == 7'b0000_001)
		begin
			frequency[31:16]<= Rx_data;
		end
        state_PWM <= 3;
     end
// state 3 - get balance of NCO frequency 
  3: begin
		if (Rx_control_0[7:1] == 7'b0000_001)
		begin
			frequency[15:0]<= Rx_data;
		end
        state_PWM <= 4;
	end
// state 4 - get Left audio 
4: begin
	fifo_enable <= 1'b1; // enable reads from Rx fifo 
	I_Data <= Rx_data;
	state_PWM <= 5;
   end
// state 5 - get Right audio
5: begin
	Q_Data <= Rx_data;
	state_PWM <= 6;
   end
// state 6 - get I PWM
6: begin
	I_PWM_temp <= Rx_data;
	state_PWM <= 7;
   end
// state 7 - get Q PWM
7: begin
	Q_PWM_temp <= Rx_data;
	state_PWM <= 8;
   end
// state 8 - loop here until 48kHz clock goes low 
8: begin
	fifo_enable <= 1'b0; // stop reading from Rx fifo while we wait for 48kHz clock
	if(PWM_clock) state_PWM <= 8;
	else state_PWM <= 9;
   end
// state 9 - wait until 48kHz clock goes high
9: begin
	if(PWM_clock)begin
		I_PWM <= I_PWM_temp;	// ensure that I and Q update at the same time 
		Q_PWM <= Q_PWM_temp;  
		byte_count <= byte_count + 1'b1;
	 	state_PWM <= 10;
	end
	else state_PWM <= 9;
   end
// state 10 - check loop count and either continue or look for sync
10: begin
		if(synced_Rx_used > 7) // make sure we have enough data to do a loop
		begin
	  		if(byte_count == 63) state_PWM <= 0; // look for sync if we have done 62 loops
  	  		else 	state_PWM <= 4;
		end
		else state_PWM <= 10;
   end

default: state_PWM <= 0;
endcase
end 


/////////////////////////////////////////////////////////////////
//
// Single bit PWM 16 bit D/A converters
//
/////////////////////////////////////////////////////////////////

// This runs off the 125MHz clock to simplify the LPF requirements.

reg [15:0] I_Data;
reg [15:0] Q_Data;
reg [15:0] I_Data_in;
reg [15:0] Q_Data_in;
reg [16:0] I_PWM_accumulator;
reg [16:0] Q_PWM_accumulator;


always @(posedge clock)
begin
        I_Data_in <= I_PWM + 16'h8000;         // so that 0 in gives 50:50 mark/space
        Q_Data_in <= Q_PWM + 16'h8000;
        I_PWM_accumulator <= I_PWM_accumulator[15:0] + I_Data_in;
        Q_PWM_accumulator <= Q_PWM_accumulator[15:0] + Q_Data_in;

end

assign I_PWM_out = I_PWM_accumulator[16];       // send to FPGA pins
assign Q_PWM_out = Q_PWM_accumulator[16];


// LEDs for testing

assign DEBUG_LED0 = ~have_sync; 	// LED 0 toggles each time we have sync	
assign DEBUG_LED1 = ~EP6_ready;		// LED D3 on when we can write to EP6
assign DEBUG_LED2 = ce_out_i;		//  strobe from CIC filter
assign DEBUG_LED3 = ~EP2_has_data;  // LED on when we receive data 

// Debug pins

assign FPGA_GPIO1 = PWM_clock;
assign FPGA_GPIO2 = SLRD;
assign FPGA_GPIO3 = fifo_enable;		
assign FPGA_GPIO4 = state_PWM[0];	
assign FPGA_GPIO5 = clock_8;	
assign FPGA_GPIO6 = state_PWM[1];	
assign FPGA_GPIO7 = write_full;	
assign FPGA_GPIO8 = state_PWM[2];		
assign FPGA_GPIO9 = read_full;		
assign FPGA_GPIO10 = state_PWM[3];	
assign FPGA_GPIO11 = 1'b1;
assign FPGA_GPIO12 = state_PWM[4];	
assign FPGA_GPIO13 = 1'b0;	
assign FPGA_GPIO14 = I_PWM_out;	
assign FPGA_GPIO15 = 1'b0;	
assign FPGA_GPIO16 = Q_PWM_out;


endmodule 


