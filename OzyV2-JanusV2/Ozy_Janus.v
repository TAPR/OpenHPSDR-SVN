// V1.32 14 January 2009  
//
// Copyright 2006,2007, 2008 Bill Tracey KD5TFD and Phil Harman VK6APH
//
//  HPSDR - High Performance Software Defined Radio
//
//  Ozy to Janus/Penelope/Mercury/Phoenix  interface.
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
// The AK5394A and the TLV320 are both in slave  mode  and obtain all their clocks from this code.
//
//
// Sends 192/96/48k 24 bit A/D data and 48k/16 bit mic/line data  via FIFO 4 and receives 48k/16 bit
// L/R and I/Q data.
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



// AK5394A and TLV320 MCLK clock is 12.288MHz and is provided on the Atlas bus.
// This version uses 2 state machines, one to manage A/D
// and one to manage the FX2 USB interface. The two are needed
// since the FX2 USB must run off the FX2 24MHz clock and the
// A/D's  must run of the 12.288MHz clock.
//
//
// I and Q data is sent in I2S format to the Atlas bus for use by Janus and Penelope
//
// 	The C&C encoder broadcasts data over the Atlas bus C20 for
//	use by other cards e.g. Mercury and Penelope.  The data is in 
//	I2S format with the clock being CBLCK and the start of each frame
//	being indicated using the negative edge of CLRCLK.
//	
//	The data fomat is as follows:
//	
//	<1 bit PTT><4 bits address><32 bits frequency><4 bits clock_s><7 bits OC><1 bit mode> 
//	
//	for a total of 49 bits. Frequency is in Hz and OC is the open collector data on Penelope
//
//
//
// Built with Quartus II v7.1 Build 178
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
//				Added CLK_MCLK to Atlas for Janus - 22 Feb 2007
//				Added Command and Control data for Penelope and Mercury - 4th March 2007
//				Added SPI control of GPIO lines for SDR 1K Control - 4th March 2007 (kd5tfd) 
//				Changed Command and Control to decode Rx_contol_n  - 7th April 2007
//				Changed AK5394A to run in Slave mode - 8th April 2007
//				Modified I2S Tx to remove potential race condition - 18th April 2007
//				Modified C&C I2S encoder to remove potential  race condition - 18 April 2007
//				Modified Mix I2S receiver to use latest form of I2S receiver - 19 April 2007
//				Modifed to take 12.5MHz clock from Penelope on Atlas A5 - 28 April 2007
//				Added temporary logic level from Atlas A2 to force clocks from Penny - 28 April 2007
//				Test code to auto detect that Penny clock is on Atlas bus - 13 August 2007
//				Minor chages to enable Quartus V7.1 to be used - 15th August 2007
//				Moved C&C decoder prior to FIFO so we can detect clock sources and speed changes - 14th Sept 2007
//				General code tidy up and revised comments - 21 Sept 2007
//				Added clock and microphone selction control from PowerSDR - 26th Sept 2007
//				Added CLK_12MHZ or PCLK_12MHZ selection control from PowerSDR - 26 Sept 2007
//				Added test for CLK_12MHZ or PCLK_12MHZ present and default to FX2/4 clock if not - 28 Sept 2007			
//				If configuration error flash DEBUG_LED0 and inhibit Tx - 28 Sept 2007
//				Force C5 as PCLK_12MHz for testing, see line 555 - 28 Oct 2007
//				CLK_12MHz or PCLK_12MHz slected via conf variable - 29 Oct 2007
//				Add LPF decoder and SPI interface for Alex control - 3 Nov 2007
//				Added HPF decoder and SPI interface for Alex control - 21 Nov 2007
//				Added 12.288MHz clock from Mercury (MCLK_12MHZ)  on Atlas A6 - 12 March 2008
// 				Added IQ select from either Janus (Atlas C10) or Mercury (Atlas A10) - 12 March 2008
//				Added decode logic for clock source from either Janus, Penelope or Mercury - 16 March 2008
//				Special test version - force clocks from Mercury - 17 April 2008
//				Test code removed - now standard version - 19 April 2008
//				Added check for all 12.288MHz clocks, error LED flash if not present and use IFCLK/4 - 21 April 2008
//				Remove 48MHz clock from Atlas if Janus not selected - 25 May 2008
//				Decode ADC settings, PGA, DITHER, RAND from PC - 25 May 2008
//				Decode Alex attenuator setting from PC - 25 May 2008 
//				Include above settings in C&C data stream - 25 May 2008
//				Remove temp SPI interface to Alex via Atlas bus - 25 May 2008 
//				15 June 2008 - moved to Quartus 8.0
//							 - Added Alex relay control to C&C data
//				 1 July 2008 - Added multiple test for C&C data before sending to Atlas bus
//				 8 July 2008 - Removed above code since not required with slower Alex SPI clock
//				11 July 2008 - Added JTAG programming support
//				16 Nov  2008 - Set C&C address to 0x0000 ready for Pheonix support  
//							 - Compiled with Quartus V8.0
//				19 Nov  2008 - Added test code for Phoenix i.e. calculate phase word and 
//							   send via Atlas bus C21
//				26 Dec  2008 - V1.20 release 

//				6 Jan   2009 - Get spectrum data on Atlas A12 and send as mic data
//							 - when on Rx add start flag in C1, bit  1  
//				9 Jan  2009 - Added ADC_OVERLOAD to C&C data 
//	           12 Jan  2009 - Added sofware serial numbers to C&C data
//			   13 Jan  2009 - Special test version, no PTT reset on loss of sync
//							- Removed rx_avail since no longer required by PC code
//							- Moved serial numbers positions in C&C data
//							- NOTE: VOX WILL NOT WORK WITH THIS VERSION   
//
////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////
//
//              Quartus V7.1/8.0/8.1 Notes
//
//////////////////////////////////////////////////////////////

/*
	In order to get this code to compile without timing errors under
	Quartus V7.1 I needed to use the following settings:
	
	- Analysis and Synthesis Settings\Power Up Dont Care [not checked]
	- Analysis and Synthesis Settings\Restructure Multiplexers  [OFF]
	- Fitter Settings\Optimise fast-corner timing [ON]
	
	Same setting for Quartus V8.0 and V8.1
	
*/


//////////////////////////////////////////////////////////////
//
//                      Pin Assignments
//
/////////////////////////////////////////////////////////////
//
//   AK5394A and LTV320AIC23B connections to OZY FPGA pins
//
//   AK_reset             	- Atlas C2  - pin 149 - AK5394A reset
//   CLK_48MHZ           	- Atlas C3  - pin 150 - 48MHz clock to PWM DACs
//   LROUT		           	- Atlas C4  - pin 151 - L/R audio in I2S format
//   CLK_12MHZ   	 		- Atlas C5  - pin 152 - 12.288MHz clock from Janus
//	 PCLK_12MHZ				- Atlas A5  - pin 144 - 12.288MHz clock from Penelope
//   MCLK_12MHZ				- Atlas A6	- pin 143 - 12.288MHz clock from Mercury  
//   BCLK                   - Atlas C6  - pin 160 - AK5394A (SCLK)
//   LRCLK                  - Atlas C7  - pin 161 - AK5394A
//   CBCLK                  - Atlas C8  - pin 162 - TLV320
//   CLRCLK                 - Atlas C9  - pin 163 - TLV320
//   DOUT                   - Atlas C10 - pin 164 - AK5394A
//	 MDOUT					- Atlas A10 - pin 138 - IQ from Mercury 
//   CDOUT                  - Atlas C11 - pin 165 - Mic from TLV320 on Janus 
//	 CDOUT_P				- Atlas A11 - pin 137 - Mic for TLV320 on Penelope
//   CDIN                   - Atlas C12 - pin 168 - TLV320 (I and Q audio)
//   DFS0                   - Atlas C13 - pin 169 - AK5394A speed setting
//   DFS1                   - Atlas C14 - pin 170 - AK5394A speed setting
//   PTT_in                 - Atlas C15 - pin 171 - PTT input from Janus
//							- Atlas C16 - pin 173 - 10MHz reference 
//	 CLK_MCLK				- Altas C17 - pin 175 - Master clock to Atlas for Janus etc 
//
//   DB9 - pin connections
//
//   dash                    - Atlas       pin 97  - dash key    - DB9 pin 6
//   dot                     - Atlas       pin 96  - PTT/dot key - DB9 pin 7
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
        CBCLK, CLRCLK, CDOUT,CDOUT_P, CDIN, DFS0, DFS1, LROUT, PTT_in, AK_reset,  DEBUG_LED0,
		DEBUG_LED1, DEBUG_LED2,DEBUG_LED3, CLK_48MHZ, CC, PCLK_12MHZ, MCLK_12MHZ, MDOUT,
		FX2_CLK, SPI_SCK, SPI_SI, SPI_SO, SPI_CS, GPIO, GPIO_nIOE, CLK_MCLK,
		FX2_PE0, FX2_PE1, FX2_PE2, FX2_PE3,SDOBACK,TDO,TCK, TMS, spectrum_in, ADC_OVERLOAD, serno);
		


input CLK_12MHZ;               // 12.288MHz clock from Janus
input PCLK_12MHZ; 			   // 12.288MHz clock from Penelope
input MCLK_12MHZ;			   // 12.288MHz clock from Mercury
input IFCLK;                   // FX2 IFCLOCK - 48MHz
output CLK_MCLK; 			   // Master Clock  to Altas for other boards
input  DOUT;				   // Data from AK5394A
input  MDOUT;				   // I&Q data from Mercury 
output BCLK, LRCLK;
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
output DEBUG_LED0;              // LEDs on OZY board
output DEBUG_LED1;
output DEBUG_LED2;
output DEBUG_LED3;
output CBCLK, CLRCLK;           // Clocks to TLV320AIC23B
output CDIN;                    // Rx data to TLV320AIC23B
output LROUT;					// Left  and Right audio data in I2S format to Atlas
input  CDOUT;                   // A/D data from TLV320AIC23B
input  PTT_in;                  // PTT active high
output DFS0,DFS1;               // speed control for AK5394A
output AK_reset;                // reset for AK5394A
reg    DFS0;					// used to set AK5394A speed
reg    DFS1;					// ditto 
output CLK_48MHZ; 				// 48MHz clock to Janus for PWM DACs 
output CC;						// Command and Control data to Atlas bus
input  CDOUT_P;					// Mic data from Penelope
input  spectrum_in;				// spectrum data from Mercury on A12
input  ADC_OVERLOAD;			// ADC overload from Mercury C18
input  serno;					// serial number of HPSDR boards software C19
// interface lines for GPIO control 
input 				FX2_CLK;		// master system clock from FX2 
input 				SPI_SCK;		// SPI SCK from FX2
input 				SPI_SI;			// SPI serial in from FX2
inout 				SPI_SO;			// SPI serial out to FX2
input 				SPI_CS;         // FPGA chip select from FX2
inout [23:0]		GPIO;			// OZY GPIO lines
output 				GPIO_nIOE;      // enable GPIO driver chips 

wire dot;							// CW dot key, active low
wire dash; 							// CW dash key, active low

assign dot = GPIO[22];   			// alias dot and dash to appropriate GPIO lines 
assign dash = GPIO[21]; 

assign GPIO_nIOE = 0; 

// interface pins for JTAG programming via Atlas bus
input  FX2_PE0;		// Port E on FX2
output FX2_PE1;
input  FX2_PE2;
input  FX2_PE3;
output TDO;			// A27 on Atlas 
input  SDOBACK;		// A25 on Atlas
output TCK;			// A24 on Atlas
output TMS;			// A23 on Atlas

// link JTAG pins through
assign TMS = FX2_PE3;
assign TCK = FX2_PE2;
assign TDO = FX2_PE0;  // TDO on our slot ties to TDI on next slot  
assign FX2_PE1 = SDOBACK;




// instantiate gpio control block 
gpio_control gpio_controlSDR(.FX2_CLK(FX2_CLK), 
		    				 .SCK(SPI_SCK), 
							 .SI(SPI_SI), 
							 .SO(SPI_SO), 
							 .CS(SPI_CS), 
							 .GPIO(GPIO)
							 );
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
reg Tx_fifo_enable;             // set when we want to send data to the Tx FIFO



//////////////////////////////////////////////////////////////
//
//                      AK5394A Information
//
/////////////////////////////////////////////////////////////

/*
        The AK5394A A/D is in Slave mode.

        NOTE: The AK5394A uses a (128 x fs) BCLK at 48k and (64 x fs) at 96k/192k

*/

//////////////////////////////////////////////////////////////
//
//                      AK5394A Reset
//
/////////////////////////////////////////////////////////////

/*
        Reset AL5394A at power on and force into 48kHz sampling rate.
        Hold the A/D chip in reset until 2^25 CLK_MCLK have passed - about 3 seconds. This
        is to allow the AK4593A to calibrate correctly.
*/

reg AK_reset;
reg [25:0] ad_count;

always @ (posedge CLK_MCLK) begin
        if(!ad_count[25])begin
        AK_reset <=0;
        ad_count <= ad_count + 25'b1;
        end
        else AK_reset <= 1;
end


//////////////////////////////////////////////////////////////
//
//                    Clocks for TLV320AIC23B etc
//
//////////////////////////////////////////////////////////////

/*      
		The following code provides clocks as required for the TLV320, 
        PWM D/A converters and AK539A. The first two  always operate at 48kHz.
        Note the phase relationship required betweeen LRCLK and BCLK.
        LRCLK must change state on the negative edge of BCLK.
*/

// divide  CLK_MCLK (12.288MHz) to give clocks for the TLV320 etc
// using Altera Megafunction 

wire [7:0]clock_out;
wire CBCLK;
wire CLRCLK;

clocks clocks(CLK_MCLK, clock_out);

wire BCLK_96_48;
wire BCLK_192;
wire LRCLK_48;
wire LRCLK_96;
wire LRCLK_192;
assign CBCLK = clock_out[1] ;           // 3.072MHz for TLV320 and PWM 
assign BCLK_96_48 = clock_out[0]; 		// 6.144MHz for AK5394A at 48/96kHz
assign BCLK_192   = CLK_MCLK; 			// 12.288MHz for AK5394A at 192kHz
assign CLRCLK = clock_out[7];           // for TLV320 at 48kHz
assign LRCLK_48 = clock_out[7]; 		// for AK5394A at 48kHz
assign LRCLK_96 = clock_out[6];			// for AK5394A at 96kHz
assign LRCLK_192 = clock_out[5];		// for AK5394A at 192kHz

// Only send 48MHz clock to Atlas bus if Mercury not fitted 
// conf[1] = 1 if Mercury selected for Rx output
assign CLK_48MHZ = conf[1] ? 1'b0 : IFCLK; 	// 48MHz clock to PWM DAC on Janus only if Mercury not slected

/* 
		Select AK5394A clocks dependant on speed settings. The pseudo code is as follows:
		
		if (reset or speed = 48k) 
			BCLK = BCLK_48_96
			LRCLK = LRCLK_48
		else if (speed = 96k)
			BCLK = BCLK_48_96
			LRCLK = LRCLK_96
		else
			BCLK = BCLK_192
			LRCLK = LRCLK_192
*/

assign BCLK  = (AK_reset && (DFS0 == 0 && DFS1 == 1))? BCLK_192 : BCLK_96_48; 
assign LRCLK = (!AK_reset || DFS0 == 0 && DFS1 == 0) ? LRCLK_48 : ((DFS0 == 1 && DFS1 == 0 )? LRCLK_96 : LRCLK_192);

// Divide 48MHz IFCLK by 4 for use as CLK_MCLK if Janus clock missing
reg IFCLK_4;
reg IF_count;
always @ (posedge IFCLK)
begin
	if (IF_count == 1)begin
		IFCLK_4 <= ~IFCLK_4;
		IF_count <= 0;
		end
	else
		IF_count <= IF_count + 1'b1;
end 


 /*

 	The 12.288MHz ADC/DAC clock  will come from Janus on Atlas C5  as CLK_12MHZ unless a Mercury or
 	Penelope board is selected in which case it will come from the Atlas bus on A5 as PCLK_12MHZ from 
	Penelope or A6 as MCLK_12MHZ from Mercury
 	
 	Which clock is used either Janus, Penelope or Mercury is determined by the conf and clock_s setting.
	The clock selection is make in the PC and encoded in the C&C data. 
	
	Select Janus,  Penelope or Mercury (12.288MHz)  master clock depending on configuration
	set via PowerSDR  (i.e. conf).
	
	conf decodes as follows:
	
	00 =  Janus alone present 
	01 =  Penelope present
	10 =  Mercury present
	11 =  Penelope and Mercury present 
	
	clock_s[2] decodes as follows:
	
	 0 = use 122.88MHz clock from Penelope (PCLK_12MHZ)
	 1 = use 122.88MHz clock from Mercury  (MCLK_12MHZ)
	
	decoder logic is as follows:
		if only Janus fitted use CLK_12MHz 
		if other board fitted follow clock_s
		
	If a clock is selected but not detected then default to IFCLK/4 so we can still 
	send sync data to the PC. Flash LED0 to indicate a clock error.
	
*/

wire JCLK_OK;
wire PCLK_OK;
wire MCLK_OK;

// Check if CLK_12MHz clock from Janus is present, if so set a flag.
clock_det janus_clock(.clock(CLK_12MHZ),.flag(JCLK_OK));

// Check if  PCLK_12MHZ clock from Penelope is present, if so set a flag.
clock_det penny_clock(.clock(PCLK_12MHZ),.flag(PCLK_OK));

// Check if  MCLK_12MHZ clock from Mercury is present, if so set a flag.
clock_det mercury_clock(.clock(MCLK_12MHZ),.flag(MCLK_OK));

// select CLK_MCLK depending on conf, clock_s settings and valid clocks.
// If a clock is selected that does not have a valid flag then  
// IFCLK_4 is selected and the clock_error flag is set 

wire clock_error;
assign CLK_MCLK = (conf == 0 & JCLK_OK)			 ? CLK_12MHZ  :
				   conf == 0 					 ? IFCLK_4    :
				  (clock_s[2] == 1'b0 & PCLK_OK) ? PCLK_12MHZ :
				  (clock_s[2] == 1'b1 & MCLK_OK) ? MCLK_12MHZ : IFCLK_4;


// set clock_error if we have selected IFCLK_4

assign clock_error = (CLK_MCLK == IFCLK_4) ? 1'b1 : 1'b0;

				
// Flash LED0 to indicate we have a clock selection error

flash flash_LED(.clock(IFCLK_4),.flag(clock_error),.LED(DEBUG_LED0));


//////////////////////////////////////////////////////////////
//
//      Select Janus or Mercury I&Q data to send to USB 
//
//////////////////////////////////////////////////////////////

/*
	Uses the conf signal to select either I&Q data from Janus or Mercury.
	Decodes as follows:
	
	conf	I&Q
	0x		Janus
	1x		Mercury
	
*/

wire select_DOUT;
assign select_DOUT = conf[1] ? MDOUT : DOUT;  // select Janus or Mercury I&Q data 



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

        If PTT_in, dot or dash inputs are acitve they are sent in C&C Tx_control_0
*/

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
`define DOUTbit select_DOUT
`endif

reg [7:0] Tx_control_0;    // control 0 to PC
reg [7:0] Tx_control_1;    // control 1 to PC
reg [7:0] Tx_control_2;    // control 2 to PC
reg [7:0] Tx_control_3;    // control 3 to PC, number of words free in Rx FIFO
reg [7:0] Tx_control_4;    // control 4 to PC

reg [11:0] spectrum_count; // 0 to 4095 for number of spectrum samples
reg spectrum_flag; 		   // set at start of block of spectrum samples

always @ (posedge BCLK)
// temp values until we use these control signals
begin
Tx_control_0[7:2] <= 6'd0;
Tx_control_1 <= {6'b0,spectrum_flag,ADC_OVERLOAD};// ADC_OVERLOAD in bit 0
Tx_control_2 <= Merc_serialno;
Tx_control_3 <= Penny_serialno;
Tx_control_4 <= 8'b0;


q[15:0] <= {q[14:0],`DOUTbit};                  // shift current AK5394A data left and add next bit
Tx_fifo_enable <= 1'b0;                         // reset Tx FIFO strobe
if (loop_counter == 63) loop_counter <= 0; 		// count how many times through the loop to
case (AD_state)                                 // see if sync is to be sent
6'd0:   begin
        if(!LRCLK) AD_state <= 6'd0;            // loop until LRCLK is high
        else AD_state <= 6'd1;
        end
6'd1:   begin
        if(!LRCLK) AD_state <= 6'd2;            // loop until LRCLK is low
        else AD_state <= 6'd1;
        end
6'd2:   begin
        if (loop_counter == 0) begin            // if zero  then send sync and C&C bytes
           register <= 16'h7F7F;
           Tx_fifo_enable <= 1'b1;              // strobe start of sync (7F7F) into Tx FIFO
           end
       	AD_state <= AD_state + 1'b1;
        end
6'd3:   begin  
        if (spectrum_count == 0) spectrum_flag <= 1'b1;		// set start of spectrum block
		else spectrum_flag <= 1'b0; 
        if(loop_counter == 0) begin             // send C&C bytes, this is C0
            register[15:8] <= 8'h7F;			// send rest of sync
        	register[7:0]  <= {Tx_control_0[7:2], ~clean_dash, (~clean_dot || clean_PTT_in)};
            Tx_fifo_enable <= 1'b1;
            end
       	AD_state <= AD_state + 1'b1;
        end
6'd4:   begin 
        if(loop_counter == 0)begin
        	register <= {Tx_control_1, Tx_control_2};  
			Tx_fifo_enable <= 1'b1;
            end
      	AD_state <= AD_state + 1'b1;
        end
6'd5:   begin 
        spectrum_count <= spectrum_count + 1'b1; // increment spectrum block count
        if(loop_counter == 0)begin
        	register <= {Tx_control_3,Tx_control_4}; // C3 - number of bytes in Rx FIFO
			Tx_fifo_enable <= 1'b1;
            end
        AD_state <= AD_state + 1'b1;
        end
6'd18:  begin   
        register <= q;                                  // AK5394A data for left channel
        Tx_fifo_enable <= 1'b1;
        AD_state <= AD_state + 1'b1;
        end
6'd26:  begin  
        register[15:8] <= q[7:0];         
        AD_state <= AD_state + 1'b1;
        end
6'd28:  begin
        if(!LRCLK)AD_state <= 6'd28;            		// wait until LRCLK goes high
        else AD_state <= AD_state + 1'b1;
        end
6'd37:  begin 
        register[7:0] <= q[7:0];                        // AK5394A data for right channel
        Tx_fifo_enable <= 1'b1;
        AD_state <= AD_state + 1'b1;
        end
6'd53:  begin  
        register <= q;
        Tx_fifo_enable <= 1'b1;
        AD_state <= AD_state + 1'b1;
        end
// If we are receiving then send spectrum data otherwise send mic data       
6'd56:  begin
        if (PTT_out)
			register <= Tx_data; 		// if PTT send microphone or line in data to Tx FIFO
		else register <= spectrum;		// else send spectrum data
        Tx_fifo_enable <= 1'b1;
        AD_state <= AD_state + 1'b1;
        end
6'd58:  begin
        loop_counter <= loop_counter + 1'b1; 			// at end of loop so increment loop counter
        AD_state <= 6'd0;                               // done so loop again
        end
default:AD_state <= AD_state + 1'b1;
endcase
end


//////////////////////////////////////////////////////////////
//
//      Read TLV320 A/D converter for microphone or line in
//
//////////////////////////////////////////////////////////////

/*      
		Since the TLV320 always runs at 48k we need a separate clock and 
        routine to read its data. TLV320 is in I2S mode.
		Read mic data on negative edge of CLRCLK. Data can be read
		any time after case 18 i.e. on negative or positive edge of CLRCLK. 
*/

reg [4:0]TX_state;

always @ (posedge CBCLK)
begin
// select microphone source depending on mic setting
begin
	if (mic) Tx_q[15:0] <= {Tx_q[14:0], CDOUT_P};			// shift Penelope mic data one data left and add next bit
	else Tx_q[15:0] <= {Tx_q[14:0], CDOUT}; 				// mic data from Janus
end           	
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
//   Rx_fifo  (4096 words) Dual clock FIFO - Altera Megafunction (dcfifo)
//
/////////////////////////////////////////////////////////////

/*
        The write clock of the FIFO is SLRD and the read clock CLK_MCLK.
        Data from the FX2_FIFO is written to the FIFO using !SLRD. Data from the
        FIFO is read on the positive edge of CLK_MCLK when fifo_enable is high. The
        FIFO is 4096 words long.
        NB: The output flags are only valid after a read/write clock has taken place
*/


wire [15:0] Rx_data;
wire [11:0] Rx_used;  	 // read side count

Rx_fifo Rx_fifo(.wrclk (~SLRD),.rdreq (fifo_enable),.rdclk (CLK_MCLK),.wrreq (Rx_fifo_enable), 
                .data (Rx_FIFO),.q (Rx_data),.rdusedw(Rx_used));

///////////////////////////////////////////////////////////////
//
//     Tx_fifo (2048 words) Dual clock FIFO  - Altera Megafunction
//
//////////////////////////////////////////////////////////////

Tx_fifo Tx_fifo(.wrclk (!BCLK),.rdreq (1'b1),.rdclk (Tx_read_clock),.wrreq(Tx_fifo_enable),
                .data (register),.q (Tx_register), .wrusedw(write_used));

wire [15:0] Tx_register;                // holds data from A/D to send to FX2
reg  Tx_read_clock;                     // when goes high sends data to Tx_register
wire [10:0] write_used;                 // indicates how may bytes in the Tx buffer
reg  [10:0] syncd_write_used;   		// ditto but synced to FX2 clock

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
        it checks 'EP2_has_data'again and repeats.
*/

reg SLOE;                             // FX2 data bus enable - active low
reg SLEN;                             // Put data on FX2 bus
reg SLRD;                             // FX2 read - active low
reg SLWR;                             // FX2 write - active low
reg [1:0] FIFO_ADR;                   // FX2 register address
wire EP2_has_data = FLAGA;            // high when EP2 has data available
wire EP6_ready = FLAGC;               // high when we can write to EP6
reg [15:0] Rx_register;               // data from PC goes here


always @ (negedge IFCLK)
begin
syncd_write_used <= write_used;
case(state_FX)
// state 0 - set up to check for Rx data from EP2
4'd0:begin
    SLWR <= 1;                        // reset FX2 FIFO write stobe
        Tx_read_clock <= 1'b0;        // reset Tx fifo read strobe
        SLRD <= 1'b1;
        SLOE <= 1'b1;
        SLEN <= 1'b0;
        FIFO_ADR <= 2'b00;            // select EP2
        state_FX <= state_FX + 1'b1;
        end
// delay 2 IFCLOCK cycle, this is necessary at 48MHZ to allow FIFO_ADR to settle
// check for Rx data
4'd2:begin
	if(EP2_has_data)begin
       state_FX <= state_FX + 1'b1;
       SLOE <= 1'b0;                  //assert SLOE
       end
    else begin
       state_FX <= 4'd6;              // No Rx data so check for Tx data
       end
    end
// Wait 2 IFCLK before we assert SLRD then load received data
4'd4:begin
        SLRD <= 1'b0;
        Rx_register[15:8] <= FX2_FD[7:0];  //  swap endian
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
4'd6:begin
		if (syncd_write_used[10] == 1'b1) begin // data available, so let's start the xfer...
        	SLWR <= 1;
            state_FX <= state_FX + 1'b1;
            FIFO_ADR <= 2'b10;              	// select EP6
            end
     	else state_FX <= 4'd2;                  // No Tx data so check for Rx data,
        end                                     // note we already have address set
// Wait 2 IFCLK for FIFO_ADR to stabilize, assert SLWR
// NOTE: seems OK with 2 waits, may need more.
4'd8:begin
	state_FX <= state_FX + 1'b1;
    Tx_read_clock <= 1'b1;                      // start transfer from Tx fifo
    end
// check Tx FIFO is ready then set Write strobe
4'd9:begin
	if (EP6_ready) begin                        // if EP6 is ready, write to it and exit this state
    	Tx_read_clock <= 1'b0;          		// end of transfer from Tx fifo
        SLEN <= 1'b1;
        state_FX <= state_FX + 1'b1;
        end
    else begin                                  // otherwise, hang out here until fifo is ready
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

assign have_sync = (got_sync && (Rx_used > 3)) ? 1'b1 : 1'b0; 




//////////////////////////////////////////////////////////////
//
//              Decode Command & Control data
//
//////////////////////////////////////////////////////////////

/*
	Decode Rx_control_0....Rx_control_4.
    //Only decode when  have_sync is true otherwise set safe values.

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
	0xx  = 122.88MHz source from Penelope 
	1xx  = 122.88MHz source from Mercury 
	
	Decode configuration, when Rx_control_0[7:1] = 0, Rx_control_1[6:5] indicates the following
	
	00 = No Tx Rx boards
	01 = Penelope fitted
	10 = Mercury fitted
	11 = Both Penelope and Mercury fitted
	
	Decode microphone source, when Rx_control_0[7:1] = 0, Rx_control_1[7] indicates the following
	
	0 = microphone source is Janus
	1 = microphone source is Penelope
	
	Decode Attenuator settings on Alex, when Rx_control_0[7:1] = 0, Rx_control_3[1:0] indicates the following 
	
	00 = 0dB
	01 = 10dB
	10 = 20dB
	11 = 30dB
	
	Decode ADC settings on Mercury, when Rx_control_0[7:1] = 0, Rx_control_3[4:2] indicates the following
	
	000 = Random, Dither, gain off
	1xx = Random ON
	x1x = Dither ON
	xx1 = Gain ON 
	
	Decode Rx relay settigs on Alex, when Rx_control_0[7:1] = 0, Rx_control_3[6:5] indicates the following
	
	00 = None
	01 = Rx 1
	10 = Rx 2
	11 = Transverter
	
	Decode Tx relay settigs on Alex, when Rx_control_0[7:1] = 0, Rx_control_4[1:0] indicates the following
	
	00 = Tx 1
	01 = Tx 2
	10 = Tx 3
	
	Decode Rx_1_out relay settigs on Alex, when Rx_control_0[7:1] = 0, Rx_control_3[7] indicates the following

	1 = Rx_1_out on 
	
*/

reg [1:0] Speed;
wire  PTT_out;
reg [3:0]clock_s;
reg mic;
reg [1:0]conf;
reg [6:0]OC; 		// open collectors on Penelope
reg mode;	 		// normal or Class E PA operation 
reg RAND;    		// when set randomizer in ADC on Mercury on
reg DITHER;	 		// when set dither in ADC on Mercury on
reg PGA; 	 		// when set gain in ADC on Mercury set to 3dB else 0dB
reg [1:0]ATTEN;	 	// decode attenuator setting on Alex
reg [1:0]TX_relay;	// Tx relay setting on Alex
reg Rout;			// Rx1 out on Alex
reg [1:0]RX_relay;	// Rx relay setting on Alex 

always @ (posedge SLRD)
begin 
if(state_sync == 4)begin 								// Need to ensure that C&C data is stable 
    if (Rx_control_0[7:1] == 7'b0000_000)begin
		    Speed <= {Rx_control_1[1], Rx_control_1[0]}; // decode speed 
			clock_s[3:0] <= {1'b0,Rx_control_1[4:2]};	// decode clock source
			conf <= Rx_control_1[6:5];					// decode configuration
			mic <= Rx_control_1[7];						// decode microphone source
			OC <= Rx_control_2[7:1];					// decode open collectors on Penelope
			mode <= Rx_control_2[0];					// decode mode, normal or Class E PA
			ATTEN <= Rx_control_3[1:0];					// decode Alex attenuator setting 
			RAND <= Rx_control_3[4];					// decode randomizer on or off
			DITHER <= Rx_control_3[3];					// decode dither on or off
			PGA <= Rx_control_3[2];						// decode ADC gain high or low
			RX_relay <= Rx_control_3[6:5]; 				// decode Alex Rx relays
			Rout <= Rx_control_3[7];					// decode Alex Rx_1_out relay
			TX_relay <= Rx_control_4[1:0];				// decode Alex Tx Relays
			end
	if (Rx_control_0[7:1] == 7'b0000_001)				// decode frequency 
		frequency <= {Rx_control_1, Rx_control_2, Rx_control_3, Rx_control_4};
	else frequency <= frequency;
	end
end	

//******** TEMP TEST - Don't inhibit PTT if sync lost **************
//assign PTT_out = (Rx_control_0[0] && have_sync) ? 1'b1 : 1'b0; // inhibit PTT if no sync
assign PTT_out = (Rx_control_0[0]) ? 1'b1 : 1'b0; // inhibit PTT if no sync

// Set speed of AK5394A, can only change  AK5394A speed on posedge BCLK - I found out the hard way!

always @ (posedge BCLK)
begin
	if (!AK_reset)							// force speed to 48Khz on AK reset
		{DFS1,DFS0} <= 2'b00;
	else {DFS1,DFS0} <= Speed;
end 



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

///////////////////////////////////////////////////////////////
//
//              Implements Command & Control  encoder 
//
///////////////////////////////////////////////////////////////

/*
	The C&C encoder broadcasts data over the Atlas bus C20 for
	use by other cards e.g. Mercury and Penelope.  The data is in pseudo
	I2S format with the clock being CBLCK and the start of each frame
	being indicated using the negative edge of CLRCLK.
	
	The data fomat is as follows:
	
	<1 bit PTT><4 bits address><32 bits frequency><4 bits clock_s><7 bits OC><1 bit mode>
	<1 bit PGA><1 bit DITHER><1 bit RAND><2 bits ATTEN><2 bits Tx antenna><1 bit Rx out>
	<2 bits Rx antenna> 
	
	for a total of 59 bits. Frequency is in Hz and 	OC is the open collector data on Penelope.
	The clock source decodes as follows:
	
	0x00  = 10MHz reference from Atlas bus ie Gibraltar
	0x01  = 10MHz reference from Penelope
	0x10  = 10MHz reference from Mercury
	00xx  = 122.88MHz source from Penelope 
	01xx  = 122.88MHz source from Mercury

		
	For future expansion the four bit address enables specific C&C data to be send to individual boards.
	For the present for use with Mercury and Penelope the address is ignored. 

*/

wire [58:0]CCdata;				// current C&C data
reg  [6:0] CCcount;
reg  CC;						// C&C data out to Atlas bus 
wire [3:0] CC_address;			// C&C address, fixed for now 
reg  [31:0]frequency;
reg  [2:0]same_count;


//set address data to 0000, this will change when Pheonix is used
assign CC_address = 4'b0000;

assign CCdata = {PTT_out,CC_address,frequency,clock_s,OC,mode,PGA,DITHER,RAND,ATTEN,TX_relay, Rout, RX_relay}; // concatenate data to send 

// send C&C data to Atlas bus in I2S format

reg [1:0]CCstate;

always @ (posedge CBCLK)
begin	
case(CCstate)
0:  if (CLRCLK)	 CCstate <= 1;		// loop until CLRCLK is high
	else CCstate <= 0;
	
1:	if (CLRCLK)	CCstate <= 1;		// wait until it goes low - this is first CBCLK after negedge of CLRCLK
	else begin
	CCcount <= 58; 					// need to have data available for Rx on next CBCLK
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
	CC <= CCdata[CCcount];			// shift data out to Atlas bus MSB first
end

//////////////////////////////////////////////////////////////
//
//		Convert frequency to phase word for Phoenix 
//		since the CPLD on that board it too small to 
//		do the calculation
//
//////////////////////////////////////////////////////////////

/*	
	Calculates  phase_word = (frequency * 2^32) /960e6  i.e. 48MHz clock x 20 in AD9912
	Each calculation takes ~ 1.5uS @ 48MHz
	This method is quite fast enough and uses much fewer LEs than a Megafunction.
	
*/

wire [31:0]phase_word;
reg  [31:0]freq;
wire ready;
always @ (posedge ready)		// strobe frequecy when ready is set
begin
	freq <= frequency;	// frequency is current frequency in Hz e.g. 14,195,000Hz
end 

division division_phoenix(.quotient(phase_word),.ready(ready),.dividend(freq),.divider(32'd960000000),.clk(IFCLK));

///////////////////////////////////////////////////////////////
//
//  Implements tempory  Command & Control  encoder for Phoenix 
//
///////////////////////////////////////////////////////////////

/*
	The temp C&C encoder broadcasts data over the Atlas bus C21 for
	use by Phoenix.  The data is in pseudo I2S format with the clock
      being CBLCK and the start of each frame being indicated using the
      negative edge of CLRCLK.

	This tempory code will be used until Penelope is modified to detect the address field
      contained in the C&C data at Atlas C20.
	
	The data format is as follows:
	
	<[32]PTT><[31:0]phase_word>
	
	for a total of 33 bits. 
*/

/*
wire [32:0]phoenix_data;		// current phoenix C&C data
reg  PCC;					// C&C data out to Atlas bus C21

assign phoenix_data = {PTT_out,phase_word}; // concatenate data to send 

// send Phoenix C&C data to Atlas bus in I2S format
// Reuse the CCcount register from the main C&C encoder which runs from 58 to 0
// Hence subtract 26 from this to give 32 to 0

// I2S data must be available on the 2nd positive edge of CBCLK after the CLRCLK transition
always @ (negedge CBCLK)
begin
	if (CCcount > 25) 
	PCC <= phoenix_data[CCcount - 26];	// shift data out to Atlas bus MSB first
end

*/

///////////////////////////////////////////////////////////////
//
//  Get spectrum data from Mercury on Atlas A12 
//
///////////////////////////////////////////////////////////////

/*
	Spectrum data is in I2S format, 16 bits following negative 
	edge of LRCLK
*/


reg  [3:0] bits;     		// how many bits clocked 
reg  [1:0] spectrum_state;
reg [15:0] spectrum_data;	// 16 bits of spectrum data
reg [15:0] spectrum; 


always @(posedge BCLK)  // use BCLK  from Atlas C6 
begin
  case(spectrum_state)
  0:
  begin
    if (LRCLK == 0)
      spectrum_state  <= 0;     // loop until LRLCK is high   
    else
      spectrum_state  <= 1;
  end

  1:
  begin
    if (LRCLK)
      spectrum_state  <= 1;     // loop until LRCLK is low  
    else
    begin
      bits      <= 15;						
      spectrum_state  <= 2;
    end
  end

  2:
  begin
    spectrum_data[bits] <= spectrum_in;   // this is the second BCLK after negedge of LRCLK
    if (bits == 0)
      spectrum_state  <= 0;      // done so restart
    else
    begin
      bits <= bits - 1'b1;
      spectrum_state  <= 2;  
    end
  end

  default:
    spectrum_state <= 0;
  endcase
end

// latch spectrum data on positive edge of LRCLK

always @ (posedge LRCLK)
	spectrum <= spectrum_data;
	
///////////////////////////////////////////////////////////////
//
//  Get Software serial numbers from cards on Atlas bus
//
///////////////////////////////////////////////////////////////	

// share state machine from spectrum code 

reg [7:0]temp_Merc_serialno;		// holds serial number of Mercury board sofware
reg [7:0]Merc_serialno;
reg [7:0]temp_Penny_serialno;
reg [7:0]Penny_serialno;

always @ (posedge BCLK)
begin
	if (spectrum_state == 2 && bits > 7)		// get Penelope serial #
		temp_Penny_serialno[bits]  <= serno;
	if (spectrum_state == 2 && bits < 8)		// get Mercury serial #
		temp_Merc_serialno[bits]  <= serno;
end 
	
// latch  serial numbers on positive edge of LRCLK

always @ (posedge LRCLK)
begin
	Penny_serialno <= temp_Penny_serialno;
	Merc_serialno <= temp_Merc_serialno;
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

// DEBUG_LED0 - flashes if 12.288MHz Atlas clock selected but not present

assign DEBUG_LED1 = ~conf[1];	// test config setting 
assign DEBUG_LED2 = ~PTT_out; 	// lights with PTT active
assign DEBUG_LED3 = ~have_sync; // lights when sync from PowerSDR detected 


endmodule

