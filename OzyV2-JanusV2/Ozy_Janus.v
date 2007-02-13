// V1.30 13th February 2007
//
// Copyright 2006  Bill Tracey KD5TFD and Phil Harman VK6APH
//
//  HPSDR - High Performance Software Defined Radio
//
//
//  Janus to Ozy interface.
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
//
// Full duplex Ozy interface for AK5394A  and TI TLV320AIC23B A/D converters
//
// The AK5394A is in master mode and the TLV320 is in slave  mode  and obtains all its clocks from this code.
//
//
// Sends 192/96/48k 24 bit A/D data and 48k/16 bit mic/line data  via FIFO 4 and receives 48k/16 bit L/R and I/Q data.
//
// A flag of 0x7F7F7F is sent prior to 5 bytes of control data then 3 bytes of
// left data, 3 bytes of right data and 2 bytes of mic/line data.  The  data sequence is then repeated
// Every 512 bytes the sync and control data is is repeated.

// The format for data TO the PC is:-
//
//  <0x7F7F><0x7F,C0><C1,C2><C3,C4><Left><Left LSB,Right MSB><Right LSW><Mic data >... etc
//
// where Cn is a control byte - see protocol design document for full description.
//
// C0 obxxxx_xx00 PTT,dot and dash inactive
// C0 0bxxxx_xx01 PTT or dot key  activated 
// C0 0bxxxx_xx10 dash key activated
//
// where x = don't care
// etc....see protocol design document for full C&C format.

//               Left                             Right
//0-------------------------------------------------------------------------63
//0                 16       24                          35         51 52    BCLK counter (AD_state)
//<   Left data     ><  LSB  >       <   Right data      ><   LSB   >       - AK5394A
//<Mic MSB ><Mic LSB>                                                       - TLV320


// Sync and control bytes are sent as follows:

// 0            2         4        5      6     BCLK counter (AD_state)
//      <0x7F7F>  <0x7F,C0> <C1,C2> <C3,C4>


// The format for data FROM the PC is the same sync & control sequence followed by 48k/16 bit data:-
//
//   <0x7F7F><0x7F,C0><C1,C2><C3,C4>< Left data >< Right data >< I data >< Q data > etc...

// Control bytes are as follows:
//
// C0 = 0bxxxx_xxx0   MOX in PowerSDR inactive
// C0 = 0bxxxx_xxx1   MOX active
//
// AK5394A speed is set with the following command FROM the PC:
//
// When C0 = 0b0000_000x
// then
//      C1 = 0bxxxx_xx00 speed is 48k (default)
//      C1 = 0bxxxx_xx01 speed is 96k
//      C1 = 0bxxxx_xx10 speed is 192k
//
// where x = don't care
// etc....see protocol design document for full C&C format.

//
// A/D data is in 2's complement format.
// AK5394A is set to be 192/96/48kHz, 24 bit in I2S mode.
//
// TLV320 is set via I2C to be 48kHz, 16 bit and I2S mode as follows:
//
// TLV320  - 1A 1E 00 - Reset chip
//         - 1A 12 01 - set digital interface active
//         - 1A 0A 00 - turn D/A mute off
//         - 1A 08 15 - D/A on, mic input, mic 20dB boost
//         - 1A 0C 00 - All chip power on
//         - 1A 0E 02 - Slave, 16 bit, I2S
//         - 1A 10 00 - Clock/2, 48k, Normal mode
//
//         I2C address for TLV320 is 0x1A 



// AK5394A and TLV320 MCLK clock is 12.288MHz and is generated on the Janus board CPLD
// This version uses 2 state machines, one to manage A/D
// and one to manage the FX2 USB interface. The two are needed
// since the FX2 USB must run off the FX2 24MHz clock and the
// A/D's  must run of the 12.288MHz clock.
//
//
// I and Q data is sent in I2S format to Atlas bus for use by Janus and Penelope
//
//
// Built with Quartus II v6.1 Build 201
//
// Change log:  Ported from Duplex.v - 22 July 2006
//              Code comments updated - 23 July 2006
//              Altered pin names to agree with PhilC conventions - 3 Aug 2006
//              Changed endian nature of 16 bit words for sync only - 4 Aug 2006
//              Updated FX2 interface and changed endian for all signals - 5 Aug 2006
//              Built using Quartus II V6.0 - 7 Aug 2006
//              Modified FX2 interface code - 10 Aug 2006
//              Added sync indication - 13 Aug 2006
//              Added POR for AK5394A via Atlas C2 - 13 Aug 2006
//              Decoded speed setting for AK5394A  - 13 Aug 2006
//              Added PTT from PowerSDR and Janus - 14 Aug 2006
//              Added CW key inputs - 14 Aug 2006
//              Speed up PTT and CW code - 15 Aug 2006
//              Changed Atlas bus connections to free I2C pins - 19 Aug 2006
//              Fixed I lo byte sample bug, I & Q last bit sample bug, added test mode - 27 Aug 2006
//				Fixed bug in speed decode now we are sending current frequency from PowerSDR - 9 Nov 2006
//				Modified to support Janus V2 hardware - 20 Nov 2006
//				Modified to support Ozy V2 hardware using EP2C8Q208C8- 4 Dec 2006
//				Upgraded to Quartus V6.1 - 25th Jan 2006
//				Modified PWM DAC to run off IFCLK to increase resolution - 25th Jan 2007
//				Added I and Q outputs in I2S format for use with Janus and Penelope - 10th Feb 2007
//				Moved PWM DAC to Janus CPLD - 11th Feb 2007
//				Moved I and Q outputs to TLV320 and Left/Right audio to PWM DAC - 13th Feb 2007
//				
//
////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////
//
//                      Pin Assignments
//
/////////////////////////////////////////////////////////////
//
//   AK5394A and LTV320AIC23B connections to OZY FPGA pins
//
//   AK_reset             	- Atlas C2  - pin 149 - AK5394A reset
//   Q_PWM_out           	- Atlas C3  - pin 150
//   I_PWM_out           	- Atlas C4  - pin 151
//   12.288MHZ clock 		- Atlas C5  - pin 152 - (CLK_12MHZ)
//   BCLK                   - Atlas C6  - pin 160 - AK5394A (SCLK)
//   LRCLK                  - Atlas C7  - pin 161 - AK5394A
//   CBCLK                  - Atlas C8  - pin 162 - TLV320
//   CLRCLK                 - Atlas C9  - pin 163 - TLV320
//   DOUT                   - Atlas C10 - pin 164 - AK5394A
//   CDOUT                  - Atlas C11 - pin 165 - TLV320
//   CDIN                   - Atlas C12 - pin 168 - TLV320
//   DFS0                   - Atlas C13 - pin 169 - AK5394A speed setting
//   DFS1                   - Atlas C14 - pin 170 - AK5394A speed setting
//   PTT_in                 - Atlas C15 - pin 171 - PTT input from Janus
//							- Atlas C16 -         - 10MHz reference 
//	 IQOUT					- Altas C17 - pin 175 - I & Q out in I2S format
//
//   DB9 - pin connections
//
//   dot                     - Atlas       pin 97  - PTT/dot key - DB9 pin 7
//   dash                    - Atlas       pin 96  - dash key    - DB9 pin 6
//
//
//       FX2 pin    to   FPGA pin connections
//
//       IFCLK                  - pin 24
//   	 FX2_CLK                - pin 23
//       FX2_FD[0]              - pin 56
//       FX2_FD[1]              - pin 57
//       FX2_FD[2]              - pin 58
//       FX2_FD[3]              - pin 59
//       FX2_FD[4]              - pin 60
//       FX2_FD[5]              - pin 61
//       FX2_FD[6]              - pin 63
//       FX2_FD[7]              - pin 64
//       FX2_FD[8]              - pin 208
//       FX2_FD[9]              - pin 207
//       FX2_FD[10]             - pin 206
//       FX2_FD[11]             - pin 205
//       FX2_FD[12]             - pin 203
//       FX2_FD[13]             - pin 201
//       FX2_FD[14]             - pin 200
//       FX2_FD[15]             - pin 199
//       FLAGA                  - pin 198
//       FLAGB                  - pin 197
//       FLAGC                  - pin 5
//       SLOE                   - pin 13
//       FIFO_ADR[0]            - pin 11
//       FIFO_ADR[1]            - pin 10
//       PKEND                  - pin 8
//       SLRD                   - pin 30
//       SLWR                   - pin 31
//
//
//   General FPGA pins
//
//       DEBUG_LED0             - pin 4
//       DEBUG_LED1             - pin 33
//       DEBUG_LED2             - pin 34
//       DEBUG_LED3             - pin 108
//		 FPGA_GPIO1				- pin 67
//		 FPGA_GPIO2				- pin 68
//		 FPGA_GPIO3				- pin 69
//		 FPGA_GPIO4				- pin 70
//		 FPGA_GPIO5				- pin 72
//		 FPGA_GPIO6				- pin 74
//		 FPGA_GPIO7				- pin 75
//		 FPGA_GPIO8				- pin 76
//		 FPGA_GPIO9				- pin 77
//		 FPGA_GPIO10			- pin 80
//		 FPGA_GPIO11			- pin 81
//		 FPGA_GPIO12			- pin 82
//		 FPGA_GPIO13			- pin 84
//		 FPGA_GPIO14			- pin 86
//		 FPGA_GPIO15			- pin 87
//		 FPGA_GPIO16			- pin 88
//		 FPGA_GPIO17			- pin 89
//
//
////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////
//
//              TODO list
//
//////////////////////////////////////////////////////////////

//
//  check pin names are consistent all the way through
//  make output data/levels safe when sync is lost
//

module Ozy_Janus(
        IFCLK, CLK_12MHZ, FX2_FD, FLAGA, FLAGB, FLAGC, SLWR, SLRD, SLOE, PKEND, FIFO_ADR, BCLK, DOUT, LRCLK,
        CBCLK, CLRCLK, CDOUT, CDIN, DFS0, DFS1, LROUT, PTT_in, AK_reset, dot, dash, DEBUG_LED0,
		DEBUG_LED1, DEBUG_LED2,DEBUG_LED3, CLK_48MHZ);

input CLK_12MHZ;               // From Janus board 12.288MHz
input IFCLK;                   // FX2 IFCLOCK - 48MHz
input BCLK, DOUT, LRCLK;
inout  [15:0] FX2_FD;           // bidirectional FIFO data to/from the FX2
//output  [15:0] FX2_FD;        // bidirectional FIFO data to/from the FX2
                                // ***** use this so simulation works
input FLAGA;
input FLAGB;
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
output LROUT;					// Left  and Right audio data in I2S format to Atlas
input CDOUT;                    // A/D data from TLV320AIC23B
input PTT_in;                   // PTT active high
input dot;                      // CW dot key, active low
input dash;                     // CW dash key, active low
output DFS0,DFS1;               // speed control for AK5394A
output AK_reset;                // reset for AK5394A
wire DFS0;
wire DFS1;
output CLK_48MHZ; 				// 48MHz clock to Janus for PWM decoder 



/*

****** THIS NEEDS UPDATING TO AGREE WITH PHIL C's TERMS ********
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

assign PKEND = 1'b1;

reg [2:0] state_A_D;            // state for A/D
reg [3:0] state_FX;             // state for FX2
reg [15:0] q;                   // holds DOUT from AK5394A
reg [15:0] Tx_q;                // holds DOUT from TLV
reg data_flag;                  // set when data ready to send to Tx FIFO
reg [15:0] register;            // AK5394A A/D uses this to send its data to Tx FIFO
reg [15:0] Tx_data;             // Tx mic audio from TLV320
reg [6:0] loop_counter;         // counts number of times round loop
reg [11:0] sync_Rx_used;        // holds # of bytes in Rx_fifo for use by PC
reg [11:0] rx_avail;            // how much space is avail in the rx fifo
reg strobe;                     // set when we want to send data to the Tx FIFO



//////////////////////////////////////////////////////////////
//
//                      AK5394A Information
//
/////////////////////////////////////////////////////////////

/*
        The AK5394A A/D is in Master mode.

        NOTE: The AK5394A uses a (128 x fs) BCLK at 48k and (64 x fs) at 96k/192k

*/

//////////////////////////////////////////////////////////////
//
//                      AK5394A Reset
//
/////////////////////////////////////////////////////////////

/*
        Reset AL5394A at power on and force into 48kHz sampling rate.
        Hold the A/D chip in reset until 2^25 CLK_12MHZ have passed - about 3 seconds. This
        is to allow the AK4593A to calibrate correctly.
*/

reg AK_reset;
reg [25:0] ad_count;

always @ (posedge CLK_12MHZ) begin
        if(!ad_count[25])begin
        AK_reset <=0;
        ad_count <= ad_count + 25'b1;
        end
        else AK_reset <= 1;
end


//////////////////////////////////////////////////////////////
//
//                              Clocks for TLV320AIC23B etc
//
//////////////////////////////////////////////////////////////

/*      The following code provides CBCLK and CLRCLK clocks as required for the TLV320
        and PWM D/A converters since these always operate at 48kHz.
        Note the phase relationship required betweeen LRCLK and BCLK.
        LRCLK must change state on the negative edge of BCLK.
*/

// divide  CLK_12MHZ (12.288MHz) from Janus to give clocks for the TLV320 
// using Altera Megafunction

wire [7:0]clock_out;
wire CBCLK;
wire CLRCLK;

clocks clocks(CLK_12MHZ, clock_out);

assign CBCLK = clock_out[1] ;           // 3.072MHz
assign CLRCLK = clock_out[7];           // 48kHz
assign CLK_48MHZ = IFCLK; 				// 48MHz clock to PWM DAC on Janus


//////////////////////////////////////////////////////////////
//
//      Decode A/D speed
//
//////////////////////////////////////////////////////////////
/*
        The speed the AK5394A runs at, either 192k,96k or 48k is set by
        the PC by decoding C1 when C0[7:1] are all zero. C1 decodes as
        follows:

        C1 = 8'bxxxx_xx00  - 48kHz
        C1 = 8'bxxxx_xx01  - 96kHz
        C1 = 8'bxxxx_xx10  - 192kHz

        The speed change should only be enabled when have_sync is true.
        When reseting the AK5394A force to 48kHz.

*/

assign {DFS1,DFS0} = (AK_reset && have_sync) ? SpeedBits[1:0] : 2'b0;


//////////////////////////////////////////////////////////////
//
//              Decode Command & Control data
//
//////////////////////////////////////////////////////////////

/*
        Add code here to decode C0-C5.
        Only decode when  have_sync is true otherwise set safe values.

*/

// decode PTT from PowerSDR. Held in Rx_control_0[0]

wire PTT_out;
assign PTT_out = have_sync ? Rx_control_0[0] : 1'b0;


//////////////////////////////////////////////////////////////
//
//      Read A/D converter, send sync and C&C to Tx FIFO
//
//////////////////////////////////////////////////////////////

/*
        The following  code sends the sync bytes, control bytes and A/D samples.

        The code is structured round counting the number of BCLKs that occur following
        the negative edge of the LRCLK. At the positive edge of BCLK a state counter (AD_state)
        increments.  At each state we determine what data needs to to be latched into a 2048 word Tx FIFO.

        We need to loop through the following code 62 times. The first time though
        we generate the 3 sync bytes, 5 control bytes, 3 left bytes, 3 right bytes and 2 microphone/line
        bytes for a total of (3 + 5 + 3 + 3 + 2) 16 bytes. Subsequent times through we do not generate
        the sync or control bytes so generate ( 3 + 3 + 2) 8 bytes.

        Since each frame is 512 bytes long we need to go through the loop

        (512 - 16)/8 = 62 times

        since the counter is incremented before we read it we actually test for
        the loop counter to be 63.

        If PTT_in, dot or dash inputs are acitve they are sent in C & C byte C0
*/

wire [7:0]C0;
assign C0 = 8'd0; // temp value, this needs to hold C&C data  in the future

reg [6:0] AD_state;

//
// The conditional block below stubs out the AK5394a DOUT and feeds
// the A/D state machine with samples incremented/decremented by 4049 -- this
// results in a perfect triangle wave on the I and Q channels that can be validated
// using the PowerSDR fpga-reads.dat output to validate the A/D loop is operating
// properly.  The increment from one sample to the next is chosen to be a prime numbner so
// the sequence does not repeat immediately from cycle to cycle.
//
// uncomment the line below to activate test mode
// `define TESTMODE 1
`ifdef TESTMODE
`define DOUTbit DOUTtest

reg DOUTtest;
reg [23:0] test_left_count;
reg [23:0] test_right_count;
reg [23:0] test_out_shift;
reg last_lr;
reg sync_lr;

always @(posedge BCLK) begin
        sync_lr = LRCLK;
        if ( last_lr == sync_lr )  begin  // lrclk has not changed  - just shift over the current sample
                DOUTtest <= test_out_shift[23];
                test_out_shift[23:1] <= test_out_shift[22:0];  // left shift
                test_out_shift[0] <= 0; // shift in 0's
        end
        else if ( sync_lr ) begin  // lrclk has gone hi, was low, posedge - left channel
                DOUTtest <= test_left_count[23];
                test_out_shift[23:1] <= test_left_count[22:0];  // left shift
                test_left_count <= test_left_count + 4049;
        end
        else begin  // negedge lrclk, right channel
                DOUTtest <= test_right_count[23];
                test_out_shift[23:1] <= test_right_count[22:0];  // left shift
                test_right_count <= test_right_count - 4049;
        end
        last_lr <= sync_lr;
end
`else
`define DOUTbit DOUT
`endif


always @ (posedge BCLK)
begin
                q[15:0] <= {q[14:0],`DOUTbit};                  // shift current AK5394A data left and add next bit
                sync_Rx_used[11:0] <= Rx_used_rdside;
                strobe <= 1'b0;                                                 // reset Tx FIFO strobe
                if (loop_counter == 63) loop_counter <= 0; // count how many times through the loop to
case (AD_state)                                                                    // see if sync is to be sent
6'd0:   begin
                if(!LRCLK) AD_state <= 6'd0;                    // loop until LRCLK is high
                else AD_state <= 6'd1;
                end
6'd1:   begin
                if(!LRCLK) AD_state <= 6'd2;                    // loop until LRCLK is low
                else AD_state <= 6'd1;
                end
6'd2:   begin
                if (loop_counter == 0) begin                    // if zero  then send sync and C&C bytes
                        register <= 16'h7F7F;
                        strobe <= 1'b1;                                         // strobe start of sync (7F7F) into Tx FIFO
                        end
                AD_state <= AD_state + 1'b1;
                end

6'd4:   begin
                if(loop_counter == 0) begin                                     // send C&C bytes, this is C0
                        register <= {C0[7:2], ~clean_dash, (~clean_dot || clean_PTT_in)};
                        register[15:8] <= 8'h7F;
                        rx_avail <= 12'd4095 - sync_Rx_used;    //  must match rx fifo size
                        strobe <= 1'b1;
                        end
                AD_state <= AD_state + 1'b1;
                end
6'd6:   begin
                if(loop_counter == 0)begin
                        register <= {RX_wait, TX_wait,};                // C1 - RX wait loop counter, C2 - Tx wait loop counter
                        strobe <= 1'b1;
                        end
                AD_state <= AD_state + 1'b1;
                end
6'd8:   begin
                if(loop_counter == 0)begin
                        register <= {rx_avail[11:4],Rx_control_4}; // C3 - number of bytes in Rx FIFO, C4 - sequence number
                        strobe <= 1'b1;
                        end
                AD_state <= AD_state + 1'b1;
                end
6'd18:  begin   
                register <= q;                                          // AK5394A data for left channel
                strobe <= 1'b1;
                AD_state <= AD_state + 1'b1;
                end
6'd26:  begin  
                register[15:8] <= q[7:0];         
                AD_state <= AD_state + 1'b1;
                end
6'd28:  begin
                if(!LRCLK)AD_state <= 6'd28;            // wait until LRCLK goes high
                else AD_state <= AD_state + 1'b1;
                end
6'd37:  begin 
                register[7:0] <= q[7:0];                        // AK5394A data for right channel
                strobe <= 1'b1;
                AD_state <= AD_state + 1'b1;
                end
6'd53:  begin  
                register <= q;
                strobe <= 1'b1;
                AD_state <= AD_state + 1'b1;
                end

6'd56:  begin
                register <= Tx_data;                            // send microphone or line in data to Tx FIFO
                strobe <= 1'b1;
                AD_state <= AD_state + 1'b1;
                end
6'd58:  begin
                loop_counter <= loop_counter + 1'b1; // at end of loop so increment loop counter
                AD_state <= 6'd0;                                        // done so loop again
                end
default:AD_state <= AD_state + 1'b1;
endcase
end


//////////////////////////////////////////////////////////////
//
//      Read TLV320 A/D converter for microphone or line in
//
//////////////////////////////////////////////////////////////

/*      Since the TLV320 always runs at 48k we need a separate
        routine to read its data. TLV320 is in I2S mode.
*/

reg [4:0]TX_state;

always @ (posedge CBCLK)
begin
                Tx_q[15:0] <= {Tx_q[14:0], CDOUT};                      // shift current TLV320 data left and add next bit
case (TX_state)
5'd0:   begin
                if(CLRCLK) TX_state <= 5'd0;                            // loop until CLRCLK is low
                else TX_state <= 5'd1;
                end
5'd1:   begin
                if(CLRCLK) TX_state <= TX_state + 1'b1;         // loop until CLRCLK is high
                else TX_state <= 5'd1;
                end
5'd18:  begin
                Tx_data <= Tx_q;                                                        // TLV320 (microphone or line in)data
                TX_state <= 5'd0;                                                       // done so loop again
                end
default:TX_state <= TX_state + 1'b1;
endcase
end

//////////////////////////////////////////////////////////////
//
//                              One-shot to strobe data into Tx FIFO
//
//////////////////////////////////////////////////////////////

/*
        The code generates a short pulse (data_flag) on the negative
        edge of BCLK if strobe is set. The pulse strobes data in 'register'
        into the Tx FIFO
*/

always @ (negedge BCLK)  // now width of 2x BCLK since else too fast for FIFO
begin
                if (data_flag)
                        data_flag <= 1'b0;
                else
                        if (strobe) data_flag <= 1'b1;
end


/*
always @ (negedge BCLK or posedge data_flag)
begin
                if (data_flag)
                        data_flag <= 1'b0;
                else
                        if (strobe) data_flag <= 1'b1;
end
*/

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

Rx_fifo Rx_fifo(.wrclk (SLRD),.rdreq (fifo_enable),.rdclk (BCLK),.wrreq (1'b1),
                .data (Rx_register),.q (Rx_data), .wrfull (write_full),.wrusedw(Rx_used),
                .rdusedw(Rx_used_rdside)
                );

///////////////////////////////////////////////////////////////
//
//                              Tx_fifo - 2048 words - Altera Megafunction
//
//////////////////////////////////////////////////////////////

Tx_fifo         Tx_fifo(.wrclk (BCLK),.rdreq (1'b1),.rdclk (Tx_read_clock),.wrreq (data_flag),
                                .data (register),.rdempty (Tx_rdempty), .wrempty (Tx_write_empty),
                                .wrfull (),.q (Tx_register), .wrusedw(write_used));

wire Tx_rdempty;                                // High when Tx fifo empty
wire [15:0] Tx_register;                // holds data from A/D to send to FX2
wire Tx_write_empty;
reg  Tx_read_clock;                     // when goes high sends data to Tx_register
wire [10:0] write_used;                 // indicates how may bytes in the Tx buffer
reg  [10:0] syncd_write_used;   // ditto but synced to FX2 clock

//////////////////////////////////////////////////////////////
//
//                              State Machine to manage FX2 USB interface
//
//////////////////////////////////////////////////////////////
/*
        The state machine checks if there are characters to be read
        in the FX2 Rx FIFO by checking 'EP2_has_data'  If set it loads the word
        read into the Rx_register. On the next clock it  checks if the Tx_data_fag is set
         - if so it sends the data in 'register'to the Tx FIFO. After the Tx data has been sent
        it checks 'EP2_has_data' in round robin fashion.
*/

reg SLOE;                                                                       // FX2 data bus enable - active low
reg SLEN;                                                                       // Put data on FX2 bus
reg SLRD;                                                                       // FX2 read - active low
reg SLWR;                                                                       // FX2 write - active low
reg [1:0] FIFO_ADR;                                                     // FX2 register address
wire EP2_has_data = FLAGA;                                      // high when EP2 has data available
wire EP6_ready = FLAGC;                                         // high when we can write to EP6
reg [15:0] Rx_register;                                         // data from PC goes here
reg [7:0]TX_wait;                                                       // increments when  we have to wait for TX_ FIFO to be free (C1)
reg [7:0]RX_wait;                                                       // increments when there is no receive data     (C2)


always @ (negedge IFCLK)
begin
        syncd_write_used <= write_used;
case(state_FX)
// state 0 - set up to check for Rx data from EP2
4'd0:begin
    SLWR <= 1;                                                  // reset FX2 FIFO write stobe
        Tx_read_clock <= 1'b0;                                  // reset Tx fifo read strobe
        SLRD <= 1'b1;
        SLOE <= 1'b1;
        SLEN <= 1'b0;
        FIFO_ADR <= 2'b00;                                      // select EP2
        state_FX <= state_FX + 1'b1;
        end
// delay 2 IFCLOCK cycle, this is necessary at 48MHZ to allow FIFO_ADR to settle
// check for Rx data
4'd2:begin
                if(EP2_has_data)
                        begin
                        state_FX <= state_FX + 1'b1;
                        SLOE <= 1'b0;                             //assert SLOE
                        end
                else begin
                        RX_wait <= RX_wait + 1'b1;              // increment RX_wait counter
                        state_FX <= 4'd6;                       // No Rx data so check for Tx data
                end
        end
// Wait 2 IFCLK before we assert SLRD then load received data
4'd4:begin
        SLRD <= 1'b0;
        Rx_register[15:8] <= FX2_FD[7:0];               //  swap endian
        Rx_register[7:0]  <= FX2_FD[15:8];
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
                                FIFO_ADR <= 2'b10;              // select EP6
                                end
            else state_FX <= 4'd2; //was 2                      // No Tx data so check for Rx data,
         end                                                    // note we already have address set
// Wait 2 IFCLK for FIFO_ADR to stabilize, assert SLWR
// NOTE: seems OK with 2 waits, may need more.
4'd8:begin
                state_FX <= state_FX + 1'b1;
                Tx_read_clock <= 1'b1;                          // start transfer from Tx fifo
         end
// check Tx FIFO is ready then set Write strobe
4'd9:   begin
            if (EP6_ready) begin                                // if EP6 is ready, write to it and exit this state
                                Tx_read_clock <= 1'b0;          // end of transfer from Tx fifo
                SLEN <= 1'b1;
                state_FX <= state_FX + 1'b1;
            end
            else begin                                          // otherwise, hang out here until fifo is ready
                SLWR <= 1;
                state_FX <= 4'd9;
                                TX_wait <= TX_wait + 1'b1;      // increment TX_wait counter
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
reg [15:0] Left_Data;                    // Left 16 bit PWM data for D/A converter
reg [15:0] Right_Data;                   // Right 16 bit PWM data for D/A converter
reg [15:0] I_PWM;                       // I 16 bit PWM data for D/A conveter
reg [15:0] Q_PWM;                       // Q 16 bit PWM data for D/A conveter
reg [15:0] I_Data;
reg [15:0] Q_Data; 
reg fifo_enable;                        // controls reading of dual clock fifo
reg [11:0] synced_Rx_used;              // how may bytes in FX2 side Rx fifos synced to BCLK
reg [6:0] byte_count;                   // counts number of times round loop
reg [7:0] Rx_control_0;                 // control C0 from PC, MOX active if bit 0 set
reg [7:0] Rx_control_1;                 // control C1 from PC, decode bits 1,0 for A/D speed setting
reg [7:0] Rx_control_2;                 // control C2 from PC
reg [7:0] Rx_control_3;                 // control C3 from PC
reg [7:0] Rx_control_4;                 // control C4 from PC
reg [1:0] SpeedBits;                    // Sampling rate bits (kd5tfd added) 00=48, 01=96 10=192 

reg [8:0]sync_count;
reg have_sync;                                  // high when we have sync


always @ (negedge BCLK)
begin
        synced_Rx_used <= Rx_used;                                                      // sync Rx_used to BCLK since this runs of FX2 clock
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
				if ( Rx_control_0[7:1] == 7'h00  ) begin   //set speed bits register if Rx0 indicates speed in Rx1
					SpeedBits[1:0] <= Rx_data[9:8]; 
				end 
                state_PWM <= 6;
        end
// state 6 - get Rx_control_3 & Rx_control_4
  6: begin
                Rx_control_3 <= Rx_data[15:8];
                Rx_control_4 <= Rx_data[7:0];
                state_PWM <= 7;
                end
// state 7 - get Left audio
  7: begin
                fifo_enable <= 1'b1;                                            // enable Rx_fifo
                Left_Data <= Rx_data;
                state_PWM <= 8;
                end
// state 8 - get Right audio
  8: begin
                Right_Data <= Rx_data;
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
                if (CLRCLK) state_PWM <= 11;                            // wait for A/D LRCLK to go low so we are in sync
            else state_PWM <= 12;
           end
// state 12 - wait for positive edge of LRCLK
  12: begin
                if (CLRCLK)                                                             // wait for A/D LRCLK to go high so we are in sync
                begin
                        byte_count <= byte_count + 1'b1;
                        //I_Data[15:0] <=  I_PWM[15:0];                   // set up I D/A data
                        //Q_Data[15:0] <=  Q_PWM[15:0];                   // set up Q D/A data
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
//              16 bits, two channels for PWM DAC on Janus
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

debounce de_dash(.clean_pb(clean_dash), .pb(dash), .clk(IFCLK));


// Flash the LEDs to show something is working! - LEDs are active low

assign DEBUG_LED0 = ~write_full;            // LED D0 on when Rx fifo is full.
assign DEBUG_LED1 = ~EP6_ready;             // LED D1 on when we can write to EP6
assign DEBUG_LED2 = ~have_sync;             // LED D2 toggles each time we get sync
assign DEBUG_LED3 = ~EP2_has_data;          // LED D3 on when EP2 has data 

endmodule

