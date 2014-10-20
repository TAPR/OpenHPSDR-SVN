//  V1.7 Aug 31, 2009 by Kirk Weedman KD7IRS
//  V1.8 Jan 10, 2011 by Phil Harman  VK6APH
//  V1.9 Jul 30, 2011 by Phil Harman  VK6APH 
//  V2.0 Sep 15, 2011 by Phil Harman  VK6APH 
//
//  Copyright 2006,2007, 2008 Bill Tracey KD5TFD and Phil Harman VK6APH
//  Copyright 2011 Phil Harman VK6APH
//
//  HPSDR - High Performance Software Defined Radio
//
//  Mercury, Penelope, Janus and Ozy interface.
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
// The AK5394A and the TLV320 are both in slave mode
//
//
// Sends 192/96/48k 24 bit A/D data and 48k/16 bit mic/line data and receives 48k 16 bit
// L/R and I/Q data.
//
// A flag of 0x7F7F7F is sent prior to 5 bytes of control data then 3 bytes of
// left data, 3 bytes of right data and 2 bytes of mic/line data.  The control/data
// sequence is then repeated every 512 bytes.

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
//   <0x7F7F> <0x7F,C0> <C1,C2> <C3,C4>


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

// Built with Quartus II V9.0  Build 132
//

// Change log:  Redesign of OzyJanus FPGA based upon only using IF_clk for logic - FEb 15, 2009
// Aug 2, 2009: 1. Fix for "distortion" problem seen in audio.  NWire_xmit for LRAudio rate was
//                 increased and PWM logic handshake improvements were made (see IF_PWM_state logic)
//              2. New modules cdc_sync, cdc_mcp and pulsegen where added to replace discrete clock
//                 domain crossing logic. These modules are now added to the "common" code area.
//              3. General naming convention cleanup.  A signal such as Rx_fifo_rdata in the IF_clk
//                 domain is renamed to IF_Rx_fifo_rdata (more needs to be done here)
//              4. 1st attempt to get Janus working - untested    Kirk KD7IRS
// Aug 12, 2009 1. Janus bug fix
//              2. Support for Duplex mode with 1 Penelope and up to 8 Mercury boards/channels
//              3. Separation of DASH/DOT/PTT data in FPGA to PC data block. See changes
//                 made to Tx_fifo_ctrl.v  Kirk KD7IRS
//
// Aug 18, 2009 Altered PTT, dash, dot pin connections - Phil VK6APH 
// Aug 22, 2009 Changed NWire_xmit to run at 10kHz, released as V1.6
//
// Aug 31, 2009 1. Changed from Async_usb to sync_usb to increase throughput when multiple
//                 Mercury data channels are used.
//              2. Changed Tx_fifo, Rx_fifo, SP_fifo to use a common file FIFO.v instead of
//                 Megawizard functions
// Sept 2, 2009 1. Added code to support multiple NWire_rcv's for multiple Mercury cards
//              2. Added support for sending data from multiple mercury receivers to PC
//                 Note: currently only support for 3 channels but support is in place to increase to 8
//              3. Temporarily changed back to Async_usb
// Sept 26,2009 - release as V1.7
// Jan  10,2011 - Added support for PennyLane. Use Atlas_C18 for nWire Tx with Drive_Level. Release as V1.8
// Jul  30,2011 - Added support for FWD and REV power from Penny(Lane).Release as V1.9
// Sep  15,2011 - Added support for manual control of Alex filter relays. Added Drive level to C&C, removed
//                nWire Drive level from Atlas_C18. Release as V2.0
//		  29,2011 - Added support for mutiple receivers in one Mercury board.
//					   Added support for 1PPS time stamp from Atlas A13 to LSB of mic data, rest of mic data set to zero.
//						Selected when C0 = 0 and C4[6] = 1.  
// Oct  8, 2012 - Fixed bug in C&C byte stream selection/assignments of frequencies for multiple receivers
//					 - Added independent manual control of HPF/LPF relays for Alex 2
//					   Released as v2.3 - built using Quartus II v11.1 sp2
// Oct 26, 2012 - fixed bug in Rx freq assignments that resulted in improper freq assignments 
//						when certain hardware configurations/PC options are selected
//					 - Fixed bug in selection of 122.88 MHz clock at power up when Penelope is chosen as source
//						Released as v2.4 - built using Quartus II v11.1 sp2
// May 4, 2013  - Added an i2c "monitor" module to extract data appearing on the i2c bus, including firmware
//						version numbers for Penny(Lane) and all Mercury boards (up to four), Penny(Lane) output power,
//						FWD output power from Alex, REV power from Alex, and ADC overflow status for all Mercury boards
//						via the i2c bus instead of using NWire comm on Atlas A2, A3, A4, A5, and A6.  Removed references
//						to Atlas A2 and A6 as well as the NWire comm modules previously used for those. Split I and Q data paths
//						to implement indepentdent I and Q data paths on the Atlas bus in order to achieve up to 384ksps sampling
//						rates.  Rx1 uses Atlas A10 to receive I data from Mercury and Atlas A5 for Q data, Rx2 uses Atlas A9 to
//						receive I data from Mercury and A4 for Q data, Rx3 uses Atlas A8 for I data from Mercury and Atlas A3 for
//						Q data. Created a new Ozy_Janus.sdc timing constraint file to meet timing requirements for all but the 
//						Atlas C5 comm to Janus.  Janus support may be addressed later. 
//						Released as v2.5  - built using Quartus II v12.1.
//
// May 11, 2014  - Added Iambic keyer. Built using Quartus V13.0.
//					  - Released as v2.6
//               - Constrained the design, closed timing.
//  May 17, 2014 - Added independent Penelope (vs PennyLane) drive level control in accordance with the latest USB comm protocol 
//					    specifications, re-closed timing.
// June 6, 2014	- Added PC control of "20 dB Mercury Rx attenuation on Tx" via C&C bit C3[4] when C0 = 0001_010x; "0" = disable, "1" = enable
//						- Released as v2.7
// Change log:  Redesign of OzyJanus FPGA based upon only using IF_clk for logic - FEb 15, 2009
// Aug 2, 2009: 1. Fix for "distortion" problem seen in audio.  NWire_xmit for LRAudio rate was
//                 increased and PWM logic handshake improvements were made (see IF_PWM_state logic)
//              2. New modules cdc_sync, cdc_mcp and pulsegen where added to replace discrete clock
//                 domain crossing logic. These modules are now added to the "common" code area.
//              3. General naming convention cleanup.  A signal such as Rx_fifo_rdata in the IF_clk
//                 domain is renamed to IF_Rx_fifo_rdata (more needs to be done here)
//              4. 1st attempt to get Janus working - untested    Kirk KD7IRS
// Aug 12, 2009 1. Janus bug fix
//              2. Support for Duplex mode with 1 Penelope and up to 8 Mercury boards/channels
//              3. Separation of DASH/DOT/PTT data in FPGA to PC data block. See changes
//                 made to Tx_fifo_ctrl.v  Kirk KD7IRS
//
// Aug 18, 2009 Altered PTT, dash, dot pin connections - Phil VK6APH 
// Aug 22, 2009 Changed NWire_xmit to run at 10kHz, released as V1.6
//
// Aug 31, 2009 1. Changed from Async_usb to sync_usb to increase throughput when multiple
//                 Mercury data channels are used.
//              2. Changed Tx_fifo, Rx_fifo, SP_fifo to use a common file FIFO.v instead of
//                 Megawizard functions
// Sept 2, 2009 1. Added code to support multiple NWire_rcv's for multiple Mercury cards
//              2. Added support for sending data from multiple mercury receivers to PC
//                 Note: currently only support for 3 channels but support is in place to increase to 8
//              3. Temporarily changed back to Async_usb
// Sept 26,2009 - release as V1.7
// Jan  10,2011 - Added support for PennyLane. Use Atlas_C18 for nWire Tx with Drive_Level. Release as V1.8
// Jul  30,2011 - Added support for FWD and REV power from Penny(Lane).Release as V1.9
// Sep  15,2011 - Added support for manual control of Alex filter relays. Added Drive level to C&C, removed
//                nWire Drive level from Atlas_C18. Release as V2.0
//		  29,2011 - Added support for mutiple receivers in one Mercury board.
//					   Added support for 1PPS time stamp from Atlas A13 to LSB of mic data, rest of mic data set to zero.
//						Selected when C0 = 0 and C4[6] = 1.  
// Oct  8, 2012 - Fixed bug in C&C byte stream selection/assignments of frequencies for multiple receivers
//					 - Added independent manual control of HPF/LPF relays for Alex 2
//					   Released as v2.3 - built using Quartus II v11.1 sp2
// Oct 26, 2012 - fixed bug in Rx freq assignments that resulted in improper freq assignments 
//						when certain hardware configurations/PC options are selected
//					 - Fixed bug in selection of 122.88 MHz clock at power up when Penelope is chosen as source
//						Released as v2.4 - built using Quartus II v11.1 sp2
// May 4, 2013  - Added an i2c "monitor" module to extract data appearing on the i2c bus, including firmware
//						version numbers for Penny(Lane) and all Mercury boards (up to four), Penny(Lane) output power,
//						FWD output power from Alex, REV power from Alex, and ADC overflow status for all Mercury boards
//						via the i2c bus instead of using NWire comm on Atlas A2, A3, A4, A5, and A6.  Removed references
//						to Atlas A2 and A6 as well as the NWire comm modules previously used for those. Split I and Q data paths
//						to implement indepentdent I and Q data paths on the Atlas bus in order to achieve up to 384ksps sampling
//						rates.  Rx1 uses Atlas A10 to receive I data from Mercury and Atlas A5 for Q data, Rx2 uses Atlas A9 to
//						receive I data from Mercury and A4 for Q data, Rx3 uses Atlas A8 for I data from Mercury and Atlas A3 for
//						Q data. Created a new Ozy_Janus.sdc timing constraint file to meet timing requirements for all but the 
//						Atlas C5 comm to Janus.  Janus support may be addressed later. 
//						Released as v2.5  - built using Quartus II v12.1.
//
// May 11, 2014  - Added Iambic keyer. Built using Quartus V13.0.
//					  - Released as v2.6
//               - Constrained the design, closed timing.
//  May 17, 2014 - Added independent Penelope (vs PennyLane) drive level control in accordance with the latest USB comm protocol 
//					    specifications, re-closed timing.
// June 6, 2014	- Added PC control of "20 dB Mercury Rx attenuation on Tx" via C&C bit C3[4] when C0 = 0001_010x; "0" = disable, "1" = enable
//						- Released as v2.7
// Sept 14,2014  - Fixed CW profile error and better matched power out in CW and TUNE modes. 
//               - Released as v2.8
//
/////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////
//
//                      Pin Assignments
//
/////////////////////////////////////////////////////////////
//
//    AK5394A and LTV320AIC23B connections to OZY FPGA pins
//
//    AK_reset      - Atlas C2  - pin 149 - AK5394A reset
//    C48_clk       - Atlas C3  - pin 150 - 48MHz clock to PWM DACs
//    J_LR_data     - Atlas C4  - pin 151 - L/R audio to Janus in I2S format 
//    C5            - Atlas C5  - pin 152 - 12.288MHz clock from Janus
//    C6            - Atlas C6  - pin 160 - BCLK to Janus
//    C7            - Atlas C7  - pin 161 - LRCLK to Janus
//    C8            - Atlas C8  - pin 162 - CBCLK to Janus
//    C9            - Atlas C9  - pin 163 - CLRCLK to Janus
//    DOUT          - Atlas C10 - pin 164 - AK5394A
//    CDOUT         - Atlas C11 - pin 165 - Mic from TLV320 on Janus 
//    J_IQ_data     - Atlas C12 - pin 168 - I/Q audio (TLV320) to Janus in I2S format 
//    DFS0          - Atlas C13 - pin 169 - AK5394A speed setting
//    DFS1          - Atlas C14 - pin 170 - AK5394A speed setting
//    PTT_in        - Atlas C15 - pin 171 - PTT input from Janus
//                  - Atlas C16 - pin 173 - 10MHz reference 
//                  - Altas C17 - pin 175 - Master clock to Atlas for Janus etc 
//    P_IQ_data     - Atlas C19 - pin 179 - P_IQ_data (TLV320) to Penelope
//    P_IQ_sync     - Atlas C22 - pin 182 - P_IQ_sync from Penelope
//    M_LR_sync     - Atlas C23 - pin 185 - M_LR_sync from Mercury
//    M_LR_data     - Atlas C24 - pin 151 - M_LR_data to Mercury
//
//    A5            - Atlas A5  - pin 144 - Penelope NWire serial number, etc
//    A6            - Atlas A6	- pin 143 - Mercury NWire serial number, etc 
//    MDOUT_I	     - Atlas A8,A9,A10 - pins 141,139,138 - I data from Mercury
//		MDOUT_Q		  - Atlas A3,A4,A5  - pins 146,145,144 - Q data from Mercury
//    CDOUT_P       - Atlas A11 - pin 137 - Mic for TLV320 on Penelope
//    A12           = Atlas A12 - pin 135 - NWire spectrum data from Mercury 
//    A13           = Atlas A13 - pin 134 - 1PPS input from GPS for time stamping data for Chirp mode
//
//    DB9 - pin connections
//
//    PTT           - FPGA       pin 96  - PTT       - DB9 pin 8
//    dash          - FPGA       pin 97  - dash  key - DB9 pin 6
//	  dot           - FPGA       pin 99  - dot   key - DB9 pin 7
//
//
//    FX2 pin    to   FPGA pin connections
//
//    IF_clk        - pin 24
//    FX2_CLK       - pin 23
//    FX2_FD[0]     - pin 56
//    FX2_FD[1]     - pin 57
//    FX2_FD[2]     - pin 58
//    FX2_FD[3]     - pin 59
//    FX2_FD[4]     - pin 60
//    FX2_FD[5]     - pin 61
//    FX2_FD[6]     - pin 63
//    FX2_FD[7]     - pin 64
//    FX2_FD[8]     - pin 208
//    FX2_FD[9]     - pin 207
//    FX2_FD[10]    - pin 206
//    FX2_FD[11]    - pin 205
//    FX2_FD[12]    - pin 203
//    FX2_FD[13]    - pin 201
//    FX2_FD[14]    - pin 200
//    FX2_FD[15]    - pin 199
//    FLAGA         - pin 198
//    FLAGB         - pin 197
//    FLAGC         - pin 5
//    SLOE          - pin 13
//    FIFO_ADR[0]   - pin 11
//    FIFO_ADR[1]   - pin 10
//    PKEND         - pin 8
//    SLRD          - pin 30
//    SLWR          - pin 31
//
//
//   General FPGA pins
//
//    DEBUG_LED0    - pin 4
//    DEBUG_LED1    - pin 33
//    DEBUG_LED2    - pin 34
//    DEBUG_LED3    - pin 108
//    FPGA_GPIO1    - pin 67
//    FPGA_GPIO2    - pin 68
//    FPGA_GPIO3    - pin 69
//    FPGA_GPIO4    - pin 70
//    FPGA_GPIO5    - pin 72
//    FPGA_GPIO6    - pin 74
//    FPGA_GPIO7    - pin 75
//    FPGA_GPIO8    - pin 76
//    FPGA_GPIO9    - pin 77
//    FPGA_GPIO10   - pin 80
//    FPGA_GPIO11   - pin 81
//    FPGA_GPIO12   - pin 82
//    FPGA_GPIO13   - pin 84
//    FPGA_GPIO14   - pin 86
//    FPGA_GPIO15   - pin 87
//    FPGA_GPIO16   - pin 88
//    FPGA_GPIO17   - pin 89
//
//
////////////////////////////////////////////////////////////////////////////////////
`timescale 1 ns/100 ps

module Ozy_Janus(
        IF_clk, FX2_FD, FLAGA, FLAGB, FLAGC, SLWR, SLRD, SLOE, PKEND, FIFO_ADR, DOUT,
        A12, A13, A20, A21, C4, C5, C6, C7, C8, C9, C12, C13, C14, C19, C21, C22, C23, C24,
        CDOUT, CDOUT_P, PTT_in, AK_reset,DEBUG_LED0,
        DEBUG_LED1, DEBUG_LED2,DEBUG_LED3, C48_clk, CC, MDOUT_I, MDOUT_Q, /*FX2_CLK,*/
        SPI_SCK, SPI_SI, SPI_SO, SPI_CS, GPIO_OUT, GPIO_IN, GPIO_nIOE,
        /*FX2_PE0,*/ FX2_PE1, /*FX2_PE2, FX2_PE3,*/ SDOBACK /*, TDO, TCK, TMS */ );

parameter M_TPD   = 4;
parameter IF_TPD  = 2;

localparam Ozy_serialno = 8'd28;		// Serial number of this version
localparam NR = 4;                  //Maximum number of Mercury receiver channels

localparam RX_FIFO_SZ  = 2048; 		// 16 by 2048 deep RX FIFO
localparam TX_FIFO_SZ  = 4096; 		// 16 by 4096 deep TX FIFO
localparam SP_FIFO_SZ  = 1024; 		// 16 by 1024 deep SP FIFO

input  wire          IF_clk;      	// FX2 IF clock - 48MHz
input  wire          DOUT;        	// Data from AK5394A
input  wire [NR-1:0] MDOUT_I;       // I data from Mercury
input  wire [NR-1:0] MDOUT_Q;       // Q data from Mercury
inout  wire   [15:0] FX2_FD;     	// bidirectional FIFO data to/from the FX2

input  wire          FLAGA;
input  wire          FLAGB;
input  wire          FLAGC;
output wire          SLWR;        // FX2 write - active low
output wire          SLRD;        // FX2 read - active low
output wire          SLOE;        // FX2 data bus enable - active low

output wire          PKEND;
output wire    [1:0] FIFO_ADR;    // FX2 register address
output wire          DEBUG_LED0;  // LEDs on OZY board
output wire          DEBUG_LED1;
output wire          DEBUG_LED2;
output wire          DEBUG_LED3;
input  wire          CDOUT;       // A/D data from TLV320AIC23B
input  wire          PTT_in;      // PTT active high from Atlas bus

input  wire          A12;         // NWire spectrum data from Mercury
input  wire          A13;         // 1PPS from GPS receiver to time stamp data for Chirp signals
input  wire          A20;			 // SCL I2C bus line, pin 115
input  wire          A21;         // SDA I2C bus line, pin 114

output wire          C4;          // J_LR_data - Left & Right audio data in I2S format to Janus
input  wire          C5;          // 12.288Mhz from Janus
output wire          C6;          // BCLK to Janus
output wire          C7;          // LRCLK to Janus
output wire          C8;          // CBCLK to Janus
output wire          C9;          // CLRCLK to Janus
output wire          C12;         // J_IQ_data - Rx data to TLV320AIC23B to Janus
output wire          C13;         // DFS0
output wire          C14;         // DFS1
output wire          C19;         // P_IQ_data - Rx data to TLV320AIC23B on Penelope
output wire          C21;         // Spectrum data Trigger signal to Mercury
input  wire          C22;         // P_IQ_sync from Penelope
input  wire          C23;         // M_LR_sync from Mercury
output wire          C24;         // M_LR_data - Left & Right audio data in NWire format to Mercury


output wire          AK_reset;    // reset for AK5394A - active low, also used to synchronize multiple Mercs on startup, uses Atlas C2, pin 149
output wire          C48_clk;     // 48MHz clock to Janus for PWM DACs 
output wire          CC;          // Command and Control data to Atlas bus 
input  wire          CDOUT_P;     // Mic data from Penelope

// interface lines for GPIO control 
//input  wire          FX2_CLK;     // master system clock from FX2 
input  wire          SPI_SCK;     // SPI SCK from FX2
input  wire          SPI_SI;      // SPI serial in from FX2
inout  wire          SPI_SO;      // SPI serial out to FX2
input  wire          SPI_CS;      // FPGA chip select from FX2
output wire   [15:0] GPIO_OUT;    // OZY GPIO lines
input  wire    [7:0] GPIO_IN;     // OZY GPIO lines
output wire 	     GPIO_nIOE;   // enable GPIO driver chips 

// interface pins for JTAG programming via Atlas bus
//input  wire          FX2_PE0;     // Port E on FX2
output wire          FX2_PE1;
//input  wire          FX2_PE2;
//input  wire          FX2_PE3;
input  wire          SDOBACK;     // A25 on Atlas
//output wire          TDO;         // A27 on Atlas 
//output wire          TCK;         // A24 on Atlas
//output wire          TMS;         // A23 on Atlas

// internal signals
wire          dot_n;    // CW dot key, active low
wire          dash_n;   // CW dash key, active low
wire 	      PTT_n;    // PTT from DB9 pin 8, active low 

// alias dot, dash and PTT to appropriate GPIO lines 
assign dot_n   	= GPIO_IN[6];
assign dash_n  	= GPIO_IN[5];   
assign PTT_n 	= GPIO_IN[7]; 

assign GPIO_nIOE = 1'b0; 

// link JTAG pins through
//assign TMS = FX2_PE3;
//assign TCK = FX2_PE2;
//assign TDO = FX2_PE0;  // TDO on our slot ties to TDI on next slot  
assign FX2_PE1 = SDOBACK;


//Chirp mode
wire PPS;									// 1PPS from GPS receiver to time stamp data
assign PPS = A13;


///////////////////////////////////////////////////////////////
//
//              GPIO
//
///////////////////////////////////////////////////////////////
// instantiate gpio control block 
gpio_control gpio_controlSDR(
                .SCK(SPI_SCK), 
                .SI(SPI_SI), 
                .SO(SPI_SO), 
                .CS(SPI_CS), 
                .GPIO({GPIO_IN,GPIO_OUT})
							 );
///////////////////////////////////////////////////////////////
//
//              3X clock multiplier  48MHz -> 144Mhz
//
///////////////////////////////////////////////////////////////
wire C144_clk;
wire C144_clk_locked;
reg  IF_rst, cmult_rst;

clkmult3 cm3 (.areset(cmult_rst), .inclk0(IF_clk),.c0(C144_clk), .c1(pro_clock_200), .locked(C144_clk_locked)); // 144 and 19.2MHz

//-------------------------------------------------------------
//				CW Clocks
//-------------------------------------------------------------

// CW clocks,  profile clock = 96kHz, iambic_clk = 30kHz
wire pro_clock_200, iambic_clk_640;	

// divide 19.2MHz to 96kHz
reg [10:0]divide_200;
reg pro_clock;
always @ (posedge pro_clock_200)
begin 
	if (divide_200 == 99) begin
		divide_200 <= 0;
		pro_clock <= ~pro_clock;
	end
	else divide_200 <= divide_200 + 1'd1;
end

// divide 19.2MHz to 30kHz
reg [9:0]divide_640;
reg iambic_clk;
always @ (posedge pro_clock_200
)
begin 
	if (divide_640 == 319) begin
		divide_640 <= 0;
		iambic_clk <= ~iambic_clk;
	end
	else divide_640 <= divide_640 + 1'd1;
end


//////////////////////////////////////////////////////////////
//
// 48MHz clock to Janus
//
/////////////////////////////////////////////////////////////

reg   [1:0] IF_conf;

// **** only send 48MHz clock to Atlas bus if Mercury not fitted to see effect on spurs
// IF_conf[1] = 1 if Mercury selected for Rx output
assign C48_clk = IF_conf[1] ? 1'b0 : IF_clk; 	// 12MHz clock to PWM DAC on Janus only if Mercury not selected

//////////////////////////////////////////////////////////////
//
// cmult_rst, C144_rst, IF_rst, AK_reset, C12_rst
//
/////////////////////////////////////////////////////////////

/*
        Reset AL5394A at power on and force into 48kHz sampling rate.
        Hold the A/D chip in reset until 2^28 CLK_MCLK have passed - about 3 seconds. This
        is to allow the AK4593A to calibrate correctly.
*/

reg [28:0] IF_count;
wire       C144_rst;

always @ (posedge IF_clk)
begin: IF_RESET
  reg i0;
  
  if (!IF_count[28])
    IF_count <= IF_count + 1'b1; // count up from 0 (powerup reset value) till IF_count[28] is set

  cmult_rst <= (IF_count[28:10] == 0) ? 1'b1 : 1'b0; // This will be a global power up reset for the IF_clk domain

  {IF_rst, i0} <= {i0, !C144_clk_locked}; // clock multiplier needs to be locked before any code runs
end

reg   C12_rst;
wire  C12_clk;  // 12.288 Mhz from Janus

always @ (posedge C12_clk)
begin: C12_RESET
  reg c0;
  
  {C12_rst, c0} <= {c0, !C144_clk_locked}; // clock multiplier needs to be locked before any code runs
end

assign C144_rst  = !C144_clk_locked;

assign AK_reset = IF_count[28];  // AK_reset (active low) is long after powerup

///////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Create BCLK/LRCLK and CBCLK/CLRCLK for Janus
//
// make sure CBCLK/CLRCLK and BCLK/LRCLK stay synced when SPEED changes
//
///////////////////////////////////////////////////////////////////////////////////////////////////////
reg        C12_cgen_rst;
wire [1:0] C12_speed;
reg        IF_DFS0;       // used to set AK5394A speed
reg        IF_DFS1;       // ditto 
reg  [1:0] IF_last_DFS;
wire       IF_dfs_change;
wire       C12_ack;

localparam SPEED_48K = 2'b00;

assign C13 = IF_DFS0;
assign C14 = IF_DFS1;

assign C12_clk = C5;

always @(posedge IF_clk)
begin
  if (IF_rst)
    IF_last_DFS <= #IF_TPD 2'b00;
  else
    IF_last_DFS <= #IF_TPD {IF_DFS1, IF_DFS0};
end

assign IF_dfs_change = (IF_last_DFS != {IF_DFS1, IF_DFS0});

// transfer IF_DFS1, IF_DFS0 into C12_clk domain
cdc_mcp #(2) dfs (.a_rst(IF_rst),  .a_clk(IF_clk),  .a_data({IF_DFS1,IF_DFS0}), .a_data_rdy(IF_dfs_change),
                  .b_rst(C12_rst), .b_clk(C12_clk), .b_data(C12_speed), .b_data_ack(C12_ack));

always @(posedge C12_clk)
begin
  if (C12_rst)
    C12_cgen_rst <= 1'b1;
  else if (C12_ack)
    C12_cgen_rst <= 1'b1; // resynchronize all the BCLK/LRCLK/CBCLK/CLRCLK for the new speed
  else
    C12_cgen_rst <= 1'b0;
end

wire C12_LRCLK, C12_cbrise, C12_cbfall, C12_rise, C12_fall;
wire C12_CBCLK, C12_CLRCLK;

clk_lrclk_gen #(.CLK_FREQ(12288000))
         lrgen (.reset(C12_cgen_rst), .CLK_IN(C12_clk), .Brise(C12_cbrise), .Bfall(C12_cbfall),
                .BCLK(),  .LRCLK(C12_LRCLK),  .Speed(C12_speed));
clk_lrclk_gen #(.CLK_FREQ(12288000))
        clrgen (.reset(C12_cgen_rst), .CLK_IN(C12_clk), .Brise(C12_rise), .Bfall(C12_fall),
                .BCLK(C12_CBCLK), .LRCLK(C12_CLRCLK), .Speed(SPEED_48K));

assign C6 = C12_clk;
assign C7 = C12_LRCLK;
assign C8 = C12_CBCLK;
assign C9 = C12_CLRCLK;

//--------------------------------------------------------------------------------------------
//  	Iambic CW Keyer
//--------------------------------------------------------------------------------------------
// parameter is clock speed in kHz. Keyer data from PC is sent in I channel when in FPGA CW mode. 
wire keyout;
wire dot, dash, CWX;
assign dot  = (IF_I_PWM[2] & internal_CW);
assign dash = (IF_I_PWM[1] & internal_CW);
assign  CWX = (IF_I_PWM[0] & internal_CW);
iambic #(30) iambic_inst (.clock(iambic_clk), .cw_speed(keyer_speed),  .iambic_mode(keyer_mode), .weight(keyer_weight), 
                          .letter_space(keyer_spacing), .dot_key(!dot_n | dot), .dash_key(!dash_n | dash),
								  .CWX(CWX), .paddle_swap(key_reverse), .keyer_out(keyout));
								  
//--------------------------------------------------------------------------------------------
//  	Calculate  Raised Cosine profile for sidetone and CW envelope when internal CW selected 
//--------------------------------------------------------------------------------------------
wire CW_char;
assign CW_char = (keyout & internal_CW  & sync_err == 0);		// set if running, internal_CW is enabled and either CW key is active
wire [15:0] CW_RF;
wire [15:0] profile;
wire CW_PTT;
profile profile_sidetone (.clock(pro_clock), .CW_char(CW_char), .profile(profile),  .delay(8'd0));
profile profile_CW       (.clock(pro_clock), .CW_char(CW_char), .profile(CW_RF),    .delay(RF_delay), .hang(hang), .PTT(CW_PTT));

//--------------------------------------------------------
//			Generate CW sidetone with raised cosine profile
//--------------------------------------------------------	
wire signed [15:0] IF_sidetone;
sidetone2 sidetone_inst( .clock(IF_clk), .tone_freq(tone_freq), .sidetone_level(sidetone_level), .CW_PTT(CW_PTT),
                        .sidetone_out(IF_sidetone),  .profile(profile));
								
// select sidetone  when CW key active and sidetone_level is not zero, else Rx audio.
wire [31:0] Rx_audio;
assign Rx_audio = CW_PTT && (sidetone_level != 0) ? {IF_sidetone, IF_sidetone}  : {IF_Left_Data,IF_Right_Data}; 

// When using Penelope RF level is set by level of I&Q. When using FPGA CW I&Q is not used so 
// to adjust power level use the Drive level to vary the level of CW_RF.

wire [15:0]level;
multiply multiply_inst ({IF_Drive_Level,8'd0}, CW_RF, level); // 16 x 16 multiply with 16 bit result

// select I&Q data or CW_RF if in CW mode. If Penelope selected then use Drive level to set RF output level. 
wire signed [15:0] I;
wire signed [15:0] Q;
assign  I =  CW_PTT  ? (penny ? (level + (level >>> 1) + (level >>> 2)) : CW_RF) : IF_I_PWM;   // <<<  to try and match CW and Tune power levels level
assign  Q =  CW_PTT  ? 16'd0 : IF_Q_PWM; 	
	
	
///////////////////////////////////////////////////////////////////////////////////////////////////////
//
//  Receive MDOUT/DOUT and CDOUT/CDOUT_P data to put in TX FIFO
//
///////////////////////////////////////////////////////////////////////////////////////////////////////
wire   [15:0] IF_P_mic_Data;
wire          IF_P_mic_Data_rdy;
wire   [15:0] IF_J_mic_Data;
wire          IF_J_mic_Data_rdy;
//wire   [47:0] IF_M_IQ_Data [0:NR-1];
//wire [NR-1:0] IF_M_IQ_Data_rdy;
wire   [23:0] IF_M_I_Data [0:NR-1];
wire [NR-1:0] IF_M_I_Data_rdy;
wire   [23:0] IF_M_Q_Data [0:NR-1];
wire [NR-1:0] IF_M_Q_Data_rdy;
wire   [47:0] IF_J_IQ_Data;
wire          IF_J_IQ_Data_rdy;
reg           IF_mic;
reg     [2:0] IF_clock_s;
wire   [63:0] IF_tx_IQ_mic_data;
reg           IF_tx_IQ_mic_rdy;
wire          IF_tx_IQ_mic_ack;
wire   [47:0] IF_IQ_Data;
wire   [15:0] IF_mic_Data;
wire    [2:0] IF_chan; // which Mercury receiver channel Tx_fifo_ctrl.v is interested in
wire    [2:0] IF_last_chan; // number of Mercury channels to send back to PC

always @*
begin
  if (IF_rst)
    IF_tx_IQ_mic_rdy = 1'b0;
  else if (IF_conf[1])
    IF_tx_IQ_mic_rdy = IF_M_I_Data_rdy[0]; // always uses channel 0 ready
  else if (IF_conf[0])
    IF_tx_IQ_mic_rdy = IF_P_mic_Data_rdy;
  else
    IF_tx_IQ_mic_rdy = IF_J_IQ_Data_rdy;
end

assign IF_IQ_Data    = IF_conf[1] ? {IF_M_I_Data[IF_chan], IF_M_Q_Data[IF_chan]}  : IF_J_IQ_Data;
                    
assign IF_mic_Data   = IF_mic ? IF_P_mic_Data : IF_J_mic_Data;

// if time_stamp is set then send 1PPS from Atlas A13 to LSB of mic data and zero all other bits.                    
assign IF_tx_IQ_mic_data = IF_time_stamp ? {IF_IQ_Data,{15'b0,PPS}}: {IF_IQ_Data, IF_mic_Data};  

NWire_rcv #(.DATA_BITS(16), .ICLK_FREQ(144000000), .XCLK_FREQ(48000000), .SLOWEST_FREQ(20000))
    P_MIC (.irst(C144_rst), .iclk(C144_clk), .xrst(IF_rst), .xclk(IF_clk),
           .xrcv_rdy(IF_P_mic_Data_rdy), .xrcv_ack(IF_tx_IQ_mic_ack),
           .xrcv_data(IF_P_mic_Data), .din(CDOUT_P) );

// Get I2S CDOUT mic data from Janus.  NOTE: only 16 bits used
I2S_rcv #(32,2,1) // WARNING: values 2,1 may need adjusting for best capture of data
    J_MIC (.xrst(IF_rst), .xclk(IF_clk), .BCLK(C12_CBCLK), .LRCLK(C12_CLRCLK), .din(CDOUT),
           .xData(IF_J_mic_Data), .xData_rdy(IF_J_mic_Data_rdy));


generate
  genvar c;
  for (c = 0; c < NR; c = c + 1) // Note: only 3 receivers on Mercury right now!
  begin: MDC // NR Mercury Data Channels/Recievers

    //NWire_rcv #(.DATA_BITS(48), .ICLK_FREQ(144000000), .XCLK_FREQ(48000000), .SLOWEST_FREQ(20000))
    //      M_IQ (.irst(C144_rst), .iclk(C144_clk), .xrst(IF_rst), .xclk(IF_clk),
    //            .xrcv_rdy(IF_M_IQ_Data_rdy[c]), .xrcv_ack(IF_tx_IQ_mic_ack),
    //            .xrcv_data(IF_M_IQ_Data[c]), .din(MDOUT[c]) );
    NWire_rcv #(.DATA_BITS(24), .ICLK_FREQ(144000000), .XCLK_FREQ(48000000), .SLOWEST_FREQ(20000))
          M_I (.irst(C144_rst), .iclk(C144_clk), .xrst(IF_rst), .xclk(IF_clk),
                .xrcv_rdy(IF_M_I_Data_rdy[c]), .xrcv_ack(IF_tx_IQ_mic_ack),
                .xrcv_data(IF_M_I_Data[c]), .din(MDOUT_I[c]) );
    NWire_rcv #(.DATA_BITS(24), .ICLK_FREQ(144000000), .XCLK_FREQ(48000000), .SLOWEST_FREQ(20000))
          M_Q (.irst(C144_rst), .iclk(C144_clk), .xrst(IF_rst), .xclk(IF_clk),
                .xrcv_rdy(IF_M_Q_Data_rdy[c]), .xrcv_ack(IF_tx_IQ_mic_ack),
                .xrcv_data(IF_M_Q_Data[c]), .din(MDOUT_Q[c]) );
  end
endgenerate

   

// Get I2S DOUT IQ data from Janus
I2S_rcv #(48,2,1) // WARNING: values 2,1 may need adjusting for best capture of data
    J_IQ (.xrst(IF_rst), .xclk(IF_clk), .BCLK(C12_clk), .LRCLK(C12_LRCLK), .din(DOUT),
          .xData(IF_J_IQ_Data), .xData_rdy(IF_J_IQ_Data_rdy));

///////////////////////////////////////////////////////////////
//
//     Tx_fifo Control - creates IF_tx_fifo_wdata and IF_tx_fifo_wreq signals
//
//////////////////////////////////////////////////////////////
localparam RFSZ = clogb2(RX_FIFO_SZ-1);  // number of bits needed to hold 0 - (RX_FIFO_SZ-1)
localparam TFSZ = clogb2(TX_FIFO_SZ-1);  // number of bits needed to hold 0 - (TX_FIFO_SZ-1)
localparam SFSZ = clogb2(SP_FIFO_SZ-1);  // number of bits needed to hold 0 - (SP_FIFO_SZ-1)

wire     [15:0] IF_tx_fifo_wdata;   // AK5394A A/D uses this to send its data to Tx FIFO
wire            IF_tx_fifo_wreq;    // set when we want to send data to the Tx FIFO
wire            IF_tx_fifo_full;
wire [TFSZ-1:0] IF_tx_fifo_used;
wire     [15:0] IF_tx_fifo_rdata;
wire            IF_tx_fifo_rreq;
wire            IF_tx_fifo_empty;

wire [RFSZ-1:0] IF_Rx_fifo_used;    // read side count
wire            IF_Rx_fifo_full;

wire            clean_dash;      // debounced dash
wire            clean_dot;       // debounced dot
wire            clean_PTT_in;    // debounced button
wire       [7:0] Penny_serialno;
wire       [7:0] Merc_serialno;
wire       [7:0] Merc2_version;
wire       [7:0] Merc3_version;
wire      [7:0] Merc4_version;
wire      [11:0] Penny_ALC;

wire             ADC_OVERLOAD;
wire             ADC_OVERLOAD2;
wire             ADC_OVERLOAD3;
wire             ADC_OVERLOAD4;
wire   [RFSZ:0] RX_USED;
wire            IF_tx_fifo_clr;
reg       [2:0] IF_merc;

assign RX_USED = {IF_Rx_fifo_full,IF_Rx_fifo_used};

// Note: 0 for Mercury will be replaced by a number we get from the PC; i.e., IF_merc
assign IF_last_chan = IF_conf[1] ? IF_merc : 3'b0; 


Tx_fifo_ctrl #(RX_FIFO_SZ, TX_FIFO_SZ) TXFC 
           (IF_rst, IF_clk, IF_tx_fifo_wdata, IF_tx_fifo_wreq, IF_tx_fifo_full,
            IF_tx_fifo_used, IF_tx_fifo_clr, IF_tx_IQ_mic_rdy, IF_tx_IQ_mic_ack,
            IF_chan, IF_last_chan, IF_tx_IQ_mic_data,
            clean_dash, clean_dot, (clean_PTT_in | CW_PTT), ADC_OVERLOAD, ADC_OVERLOAD2,
            ADC_OVERLOAD3, ADC_OVERLOAD4, Penny_serialno, Merc_serialno, 
            Merc2_version, Merc3_version, Merc4_version, Ozy_serialno, Penny_ALC, FWD, REV);

///////////////////////////////////////////////////////////////
//
//     Tx_fifo (4096 words) single clock FIFO
//
//////////////////////////////////////////////////////////////


// NOTE: Reset Tx_fifo when {IF_DFS1,IF_DFS0} changes!!!???
FIFO #(4096) TXF (.rst(IF_rst || IF_tx_fifo_clr), .clk (IF_clk), .full(IF_tx_fifo_full),
             .empty(IF_tx_fifo_empty), .usedw(IF_tx_fifo_used),
             .wrreq (IF_tx_fifo_wreq), .data (IF_tx_fifo_wdata),
             .rdreq (IF_tx_fifo_rreq), .q    (IF_tx_fifo_rdata) );


/////////////////////////////////////////////////////////////
//
//   Rx_fifo  (2048 words) single clock FIFO
//
/////////////////////////////////////////////////////////////

wire [15:0] IF_Rx_fifo_rdata;
reg         IF_Rx_fifo_rreq;    // controls reading of fifo

wire [15:0] IF_Rx_fifo_wdata;
reg         IF_Rx_fifo_wreq;

// NOTE: Reset Rx_fifo when {IF_DFS1,IF_DFS0} changes!!!???
FIFO #(2048) RXF (.rst(IF_rst), .clk (IF_clk), .full(IF_Rx_fifo_full), .usedw(IF_Rx_fifo_used),
          .wrreq (IF_Rx_fifo_wreq), .data (IF_Rx_fifo_wdata), 
          .rdreq (IF_Rx_fifo_rreq), .q    (IF_Rx_fifo_rdata) );

/////////////////////////////////////////////////////////////
//
//   SP_fifo  (1024 words) single clock FIFO
//
/////////////////////////////////////////////////////////////

/*
        The spectrum data FIFO is 16 by 1024 words long.
        NB: The output flags are only valid after a read/write clock has taken place
*/

wire     [15:0] sp_fifo_rdata;
wire            sp_fifo_rreq;    // controls reading of fifo

wire     [15:0] sp_fifo_wdata;
reg             sp_fifo_wreq;

wire            sp_fifo_full;
wire            sp_fifo_empty;
wire [SFSZ-1:0] sp_fifo_used;    // read side count

FIFO #(1024) SPF (.rst(IF_rst), .clk (IF_clk), .full(sp_fifo_full), .usedw(sp_fifo_used),
             .wrreq (sp_fifo_wreq), .data (sp_fifo_wdata), .rdreq (sp_fifo_rreq),
             .q(sp_fifo_rdata) );

///////////////////////////////////////////////////////////////////////////////////////////////////////
//
//  Receive Spectrum Data from Mercury
//
///////////////////////////////////////////////////////////////////////////////////////////////////////
wire spd_rdy;
reg  spd_ack;
wire trigger;

always @(posedge IF_clk)
begin
  if (IF_rst)
    spd_ack <= #IF_TPD 1'b0;
  else
    spd_ack <= spd_rdy; // one IF_clk delay

  if (IF_rst)
    sp_fifo_wreq <= #IF_TPD 1'b0;
  else
    sp_fifo_wreq <= spd_rdy & !spd_ack;
end

NWire_rcv #(.DATA_BITS(16), .ICLK_FREQ(144000000), .XCLK_FREQ(48000000), .SLOWEST_FREQ(80000))
       SPD (.irst(C144_rst), .iclk(C144_clk), .xrst(IF_rst), .xclk(IF_clk),
            .xrcv_rdy(spd_rdy), .xrcv_ack(spd_ack), .xrcv_data(sp_fifo_wdata), .din(A12) );

assign C21 = trigger;

sp_rcv_ctrl SPC (.rst(IF_rst), .clk(IF_clk), .trigger(trigger), .fifo_wreq(sp_fifo_wreq),
                 .flag(FLAGB));

//////////////////////////////////////////////////////////////
//
//   Interface to FX2 USB interface and FIFOs
//
//////////////////////////////////////////////////////////////
wire IF_Rx_fifo_drdy;

async_usb #(3, RX_FIFO_SZ, 64, TX_FIFO_SZ, 256, SP_FIFO_SZ, 64)
    usb1 (IF_clk, IF_rst, FX2_FD, FLAGA, FLAGB, FLAGC, SLWR, SLRD, SLOE, PKEND, FIFO_ADR,
          IF_Rx_fifo_drdy, IF_Rx_fifo_wdata, IF_Rx_fifo_used, IF_Rx_fifo_full,
          IF_tx_fifo_rreq, IF_tx_fifo_rdata, IF_tx_fifo_used, IF_tx_fifo_full,
          sp_fifo_rreq, sp_fifo_rdata, sp_fifo_used, sp_fifo_full);

wire tmc_err;

/*TestModeCheck TMC (.rst(IF_rst), .clk(IF_clk), .sync_state(IF_SYNC_state),
                   .data_in(IF_Rx_fifo_wdata), .wrreq(IF_Rx_fifo_wreq),
                   .data_out(IF_Rx_fifo_rdata), .rdreq(IF_Rx_fifo_rreq),
                   .used(IF_Rx_fifo_used), .xrdy(IF_xmit_req), .C23(IF_C23), .tmc_err(tmc_err));
*/
//////////////////////////////////////////////////////////////
//
//   Sync and  C&C  Detector
//
//////////////////////////////////////////////////////////////

/*

  Read the value of IF_Rx_fifo_wdata whenever IF_Rx_fifo_wreq is set.
  Look for sync and if found decode the C&C data.
  Then send subsequent data to Rx FIF0 until end of frame.
	
*/

reg   [2:0] IF_SYNC_state;
reg   [2:0] IF_SYNC_state_next;
reg   [7:0] IF_SYNC_frame_cnt; // 256-4 words = 252 words
reg   [7:0] IF_Rx_ctrl_0;   // control C0 from PC
reg   [7:0] IF_Rx_ctrl_1;   // control C1 from PC
reg   [7:0] IF_Rx_ctrl_2;   // control C2 from PC
reg   [7:0] IF_Rx_ctrl_3;   // control C3 from PC
reg   [7:0] IF_Rx_ctrl_4;   // control C4 from PC
reg         IF_Rx_save;

localparam SYNC_IDLE   = 1'd0,
           SYNC_START  = 1'd1,
           SYNC_RX_1_2 = 2'd2,
           SYNC_RX_3_4 = 2'd3,
           SYNC_FINISH = 3'd4;

always @ (posedge IF_clk)
begin
  if (IF_rst)
    IF_SYNC_state <= #IF_TPD SYNC_IDLE;
  else
    IF_SYNC_state <= #IF_TPD IF_SYNC_state_next;

  if (IF_rst)
    IF_Rx_save <= #IF_TPD 1'b0;
  else
    IF_Rx_save <= #IF_TPD IF_Rx_fifo_drdy && (IF_SYNC_state == SYNC_RX_3_4);

  if (IF_Rx_fifo_drdy && (IF_SYNC_state == SYNC_START) && (IF_Rx_fifo_wdata[15:8] == 8'h7F))
    IF_Rx_ctrl_0  <= #IF_TPD IF_Rx_fifo_wdata[7:0];

  if (IF_Rx_fifo_drdy && (IF_SYNC_state == SYNC_RX_1_2))
  begin
    IF_Rx_ctrl_1  <= #IF_TPD IF_Rx_fifo_wdata[15:8];
    IF_Rx_ctrl_2  <= #IF_TPD IF_Rx_fifo_wdata[7:0];
  end

  if (IF_Rx_fifo_drdy && (IF_SYNC_state == SYNC_RX_3_4))
  begin
    IF_Rx_ctrl_3  <= #IF_TPD IF_Rx_fifo_wdata[15:8];
    IF_Rx_ctrl_4  <= #IF_TPD IF_Rx_fifo_wdata[7:0];
  end

  if (IF_SYNC_state == SYNC_START)
    IF_SYNC_frame_cnt <= 0;					    // reset sync counter
  else if (IF_Rx_fifo_drdy && (IF_SYNC_state == SYNC_FINISH))
    IF_SYNC_frame_cnt <= IF_SYNC_frame_cnt + 1'b1;
end

always @*
begin
  case (IF_SYNC_state)
    // state SYNC_IDLE  - loop until we find start of sync sequence
    SYNC_IDLE:
    begin
      IF_Rx_fifo_wreq  = 1'b0;             // Note: Sync bytes not saved in Rx_fifo

      if (IF_rst || !IF_Rx_fifo_drdy)              
        IF_SYNC_state_next = SYNC_IDLE;    // wait till we get data from PC
      else if (IF_Rx_fifo_wdata == 16'h7F7F)
        IF_SYNC_state_next = SYNC_START;   // possible start of sync
      else
        IF_SYNC_state_next = SYNC_IDLE;
    end	

    // check for 0x7F  sync character & get Rx control_0 
    SYNC_START:
    begin
      IF_Rx_fifo_wreq  = 1'b0;             // Note: Sync bytes not saved in Rx_fifo

      if (!IF_Rx_fifo_drdy)              
        IF_SYNC_state_next = SYNC_START;   // wait till we get data from PC
      else if (IF_Rx_fifo_wdata[15:8] == 8'h7F)
        IF_SYNC_state_next = SYNC_RX_1_2;  // have sync so continue
      else
        IF_SYNC_state_next = SYNC_IDLE;    // start searching for sync sequence again
    end

    
    SYNC_RX_1_2:                        // save Rx control 1 & 2
    begin
      IF_Rx_fifo_wreq  = 1'b0;             // Note: Rx control 1 & 2 not saved in Rx_fifo

      if (!IF_Rx_fifo_drdy)              
        IF_SYNC_state_next = SYNC_RX_1_2;  // wait till we get data from PC
      else
        IF_SYNC_state_next = SYNC_RX_3_4;
    end

    SYNC_RX_3_4:                        // save Rx control 3 & 4
    begin
      IF_Rx_fifo_wreq  = 1'b0;             // Note: Rx control 3 & 4 not saved in Rx_fifo

      if (!IF_Rx_fifo_drdy)              
        IF_SYNC_state_next = SYNC_RX_3_4;  // wait till we get data from PC
      else
        IF_SYNC_state_next = SYNC_FINISH;
    end

    // Remainder of data goes to Rx_fifo, re-start looking
    // for a new SYNC at end of this frame.
    SYNC_FINISH:
    begin
      IF_Rx_fifo_wreq  = IF_Rx_fifo_drdy;

      if (IF_SYNC_frame_cnt == ((512-8)/2)) // frame ended, go get sync again
        IF_SYNC_state_next = SYNC_IDLE;
      else
        IF_SYNC_state_next = SYNC_FINISH;
    end

    default:
    begin
      IF_Rx_fifo_wreq  = 1'b0;

      IF_SYNC_state_next = SYNC_IDLE;
    end
	endcase
end

//////////////////////////////////////////////////////////////
//
//              Decode Command & Control data
//
//////////////////////////////////////////////////////////////

/*
	Decode IF_Rx_ctrl_0....IF_Rx_ctrl_4.

	Decode frequency (for Mercury and Penelope), PTT and Speed 

	The current frequency is set by the PC by decoding 
	IF_Rx_ctrl_1... IF_Rx_ctrl_4 when IF_Rx_ctrl_0[7:1] = 7'b0000_001
		
      The sampling rate, either 192k, 96k or 48k is set by
      the PC by decoding IF_Rx_ctrl_1 when IF_Rx_ctrl_0[7:1] are all zero. IF_Rx_ctrl_1
      decodes as follows:

      IF_Rx_ctrl_1 = 8'bxxxx_xx00  - 48kHz
      IF_Rx_ctrl_1 = 8'bxxxx_xx01  - 96kHz
      IF_Rx_ctrl_1 = 8'bxxxx_xx10  - 192kHz

	Decode PTT from PowerSDR. Held in IF_Rx_ctrl_0[0] as follows
	
	0 = PTT inactive
	1 = PTT active
	
	Decode clock sources, when IF_Rx_ctrl_0[7:1] = 0,  IF_Rx_ctrl_1[4:2] indicates the following
	
	x00  = 10MHz reference from Atlas bus ie Gibraltar
	x01  = 10MHz reference from Penelope
	x10  = 10MHz reference from Mercury
	0xx  = 12.288MHz source from Penelope 
	1xx  = 12.288MHz source from Mercury 
	
	Decode configuration, when IF_Rx_ctrl_0[7:1] = 0, IF_Rx_ctrl_1[6:5] indicates the following
	
	00 = No Tx Rx boards
	01 = Penelope fitted
	10 = Mercury fitted
	11 = Both Penelope and Mercury fitted
	
	Decode Attenuator settings on Alex, when IF_Rx_ctrl_0[7:1] = 0, IF_Rx_ctrl_3[1:0] indicates the following 
	
	00 = 0dB
	01 = 10dB
	10 = 20dB
	11 = 30dB
	
	Decode ADC settings on Mercury, when IF_Rx_ctrl_0[7:1] = 0, IF_Rx_ctrl_3[4:2] indicates the following
	
	000 = Random, Dither, gain off
	1xx = Random ON
	x1x = Dither ON
	xx1 = Gain ON 
	
	Decode Rx relay settigs on Alex, when IF_Rx_ctrl_0[7:1] = 0, IF_Rx_ctrl_3[6:5] indicates the following
	
	00 = None
	01 = Rx 1
	10 = Rx 2
	11 = Transverter
	
	Decode number of receivers to send back, when IF_Rx_ctrl_0[7:1] = 0, IF_Rx_ctrl4[5:3] contains the number of
	receiver IQ streams to interleave and send to the PC (000 = 1, 001 = 2, 010 = 3, 011 = 4)

	Decode Tx relay settigs on Alex, when IF_Rx_ctrl_0[7:1] = 0, IF_Rx_ctrl_4[1:0] indicates the following
	
	00 = Tx 1
	01 = Tx 2
	10 = Tx 3
	
	Decode Rx_1_out relay settings on Alex, when IF_Rx_ctrl_0[7:1] = 0, IF_Rx_ctrl_3[7] indicates the following

	1 = Rx_1_out on 
	
	Decode 1PPS time stamp from GPS on Atlas A13, when IF_Rx_ctrl_0[7:1] = 0, IF_Rx_ctrl_4[6] = 1 then selected

	When IF_Rx_ctrl_0[7:1] == 7'b0001_001 decodes as follows:- 
	
	IF_Mic_boost	  <= IF_Rx_ctrl_2[0];   // decode mic boost 0 = 0dB, 1 = 20dB
	IF_Drive_Level	  <= IF_Rx_ctrl_1;	    // decode drive level for PennyLane and Hermes 
	IF_Line_in		  <= IF_Rx_ctrl_2[1];   // decode input, 0 = mic, 1 = line 
	IF_Alex 		  <= {IF_Rx_ctrl_2[6], IF_Rx_ctrl_4[6:0], IF_Rx_ctrl_3[6:0]}; // Alex manual relay selections 
	
	where IF_Alex data is 15 bits as follows:
	
	Manual, 17/15m LPF, 12/10m LPF, 6m LPF, 160m LPF, 80m LPF, 60/40m LPF, 30/20m LPF, 6m preamp, bypass HPFs, 1.5MHz HPF,
	6.5MHz HPF, 9.5MHz HPF, 20MHz HPF, 13MHz HPF
	
	
	
*/

wire        PTT_out;
reg   [6:0] IF_OC;       		// open collectors on Penelope
reg         IF_mode;     		// normal or Class E PA operation 
reg         IF_RAND;     		// when set randomizer in ADC on Mercury on
reg         IF_DITHER;   		// when set dither in ADC on Mercury on
reg         IF_PREAMP;      	// when zero 20 dB attenuator on Mercury #1 is inserted
reg   [1:0] IF_ATTEN;    		// decode attenuator setting on Alex
reg   [1:0] IF_TX_relay; 		// Tx relay setting on Alex
reg         IF_Rout;     		// Rx1 out on Alex
reg   [1:0] IF_RX_relay; 		// Rx relay setting on Alex 
reg  [31:0] IF_frequency [0:4]; // Penny, Merc1, Merc2, Merc3, Merc4
reg         IF_duplex;
reg   [7:0] IF_Drive_Level; 	// Tx drive level for PennyLane and Hermes
reg  [14:0] IF_Alex[0:3];		// Alex auto/manual mode select and HPF/LPF relay switching when in manual mode
reg			Alex_manual;		// Alex auto/manual selection (0=auto, 1=manual)
reg         IF_time_stamp;		// if set 1PPS input is connected to LSB of mic data, rest are zero
reg         common_Merc_freq;	// 0 = permit independent freqs to Mercury boards, 1 = common freq for all Mercury boards
reg         Decode_IF_f;		// set if RX_CNTRL_0 is specifying frequencies for multiple receivers
reg   [3:0] RxN_preamps;		// Mercury boards preamp states (0000=all preamps OFF, 0001=Merc1 preamp ON, 0010=Merc2 preamp ON, etc)
										// Note: presently there is a redundancy for Mercury #1 "pre-amp" selection because IF_PREAMP above also 
										// controls the Mercury #1 pre-amp state; this is to maintain compatability with earlier PC programs that select 
										// "preamp" via C&C byte C3[2] when C0[7:1] is zero.
reg   [4:0] IF_line_boost;		// line-in gain setting
reg         internal_CW;		// set when internal CW generation selected
reg   [7:0] sidetone_level;	// 0 - 100, sets internal sidetone level
reg   [7:0] RF_delay;			// 0 - 255, sets delay in mS from CW Key activation to RF out
reg   [9:0] hang;					// 0 - 1000, sets delay in mS from release of CW Key to dropping of PTT
reg  [11:0] tone_freq;			// 200 to 2250 Hz, sets sidetone frequency.
reg         key_reverse;		// reverse CW keyes if set
reg   [5:0] keyer_speed; 		// CW keyer speed 0-60 WPM
reg   [1:0] keyer_mode;			// 00 = straight/external/bug, 01 = Mode A, 10 = Mode B
reg   [7:0] keyer_weight;		// keyer weight 33-66
reg         keyer_spacing;		// 0 = off, 1 = on
reg         penny;				// 1 = Penelope board selected
reg			Merc_atten_on_Tx; // 0 = disabled, 1 = enabled

always @ (posedge IF_clk)
begin 
  if (IF_rst)
  begin // set up default values - 0 for now
    // RX_CONTROL_1
    {IF_DFS1, IF_DFS0} <= 2'b00;   // speed 
    IF_clock_s         <= 3'b000; // clock source
    IF_conf            <= 2'b00;   // configuration
    IF_mic             <= 1'b0;    // decode microphone source
     // RX_CONTROL_2
    IF_mode            <= 1'b0;    // mode, normal or Class E PA
    IF_OC              <= 7'b0;    // open collectors on Penelope
    // RX_CONTROL_3
    IF_ATTEN           <= 2'b0;    // Alex attenuator setting 
    IF_PREAMP          <= 1'b0;    // "pre-amp" selection
    IF_DITHER          <= 1'b0;    // dither on or off
    IF_RAND            <= 1'b0;    // randomizer on or off
    IF_RX_relay        <= 2'b0;    // Alex Rx relays
    IF_Rout            <= 1'b0;    // Alex Rx_1_out relay
    // RX_CONTROL_4
    IF_TX_relay        <= 2'b0;    // Alex Tx Relays
    IF_duplex          <= 1'b0;    // not in duplex mode
    IF_merc            <= 3'b000;  // default single receiver
    IF_Drive_Level     <= 8'b0;	  // drive at minimum 
	 IF_Alex[1]			  <= 15'b0;	  // Alex 2 manual HPF/LPF relay selection
	 Alex_manual		  <= 1'b0;	  // Alex auto/manual selection (0=auto, 1=manual)
	 IF_time_stamp		  <= 1'b0;	  // 1PPS time stamp
	 common_Merc_freq   <= 1'b0;    // default, permit independent Mercury board freqs
	 RxN_preamps        <= 4'b0;    // default, all Mercury preamps OFF	 
    internal_CW        <= 1'b0;	  // default internal CW generation is off
    sidetone_level     <= 8'b0;	  // default sidetone level is 0
    RF_delay           <= 8'b0;	  // default CW Key activation to RF out
    hang               <= 10'b0;	  // default hang time 
	 tone_freq  		  <= 12'b0;	  // default sidetone frequency
    key_reverse		  <= 1'b0;    // reverse CW keyes if set
    keyer_speed        <= 6'b0; 	  // CW keyer speed 0-60 WPM
    keyer_mode         <= 2'b0;	  // 00 = straight/external/bug, 01 = Mode A, 10 = Mode B
    keyer_weight       <= 8'b0;	  // keyer weight 33-66
    keyer_spacing      <= 1'b0;	  // 0 = off, 1 = on
	 penny				  <= 1'b0;	  // 1 indicates a Penelope board selected
	 Merc_atten_on_Tx   <= 1'b1;	  // default is "enabled", i.e., put the Mercury on-board 20 dB attenuator in the Rx path when Tx is active
	 end
  else if (IF_Rx_save) // all Rx_control bytes are ready to be saved
  begin 								// Need to ensure that C&C data is stable 
    if (IF_Rx_ctrl_0[7:1] == 7'b0000_000)
    begin
      // RX_CONTROL_1
      {IF_DFS1, IF_DFS0}  <= IF_Rx_ctrl_1[1:0]; // decode speed 
      IF_clock_s          <= IF_Rx_ctrl_1[4:2]; // decode clock source 
      IF_conf             <= IF_Rx_ctrl_1[6:5]; // decode configuration
      IF_mic              <= IF_Rx_ctrl_1[7];   // decode microphone source
      // RX_CONTROL_2
      IF_mode             <= IF_Rx_ctrl_2[0];   // decode mode, normal or Class E PA
      IF_OC               <= IF_Rx_ctrl_2[7:1]; // decode open collectors on Penelope
      // RX_CONTROL_3
      IF_ATTEN            <= IF_Rx_ctrl_3[1:0]; // decode Alex attenuator setting 
      IF_PREAMP           <= IF_Rx_ctrl_3[2];   // decode "pre-amp" selection
      IF_DITHER           <= IF_Rx_ctrl_3[3];   // decode dither on or off
      IF_RAND             <= IF_Rx_ctrl_3[4];   // decode randomizer on or off
      IF_RX_relay         <= IF_Rx_ctrl_3[6:5]; // decode Alex Rx relays
      IF_Rout             <= IF_Rx_ctrl_3[7];   // decode Alex Rx_1_out relay
      // RX_CONTROL_4
      IF_TX_relay         <= IF_Rx_ctrl_4[1:0]; // decode Alex Tx Relays
      IF_duplex           <= IF_Rx_ctrl_4[2];   // save duplex mode
      IF_merc     	     <= IF_Rx_ctrl_4[5:3]; // number of IQ streams to send to PC
		IF_time_stamp       <= IF_Rx_ctrl_4[6];   // 1PPS time stamp
		common_Merc_freq    <= IF_Rx_ctrl_4[7];   // force common freq to all Mercury boards

    end
    if (IF_Rx_ctrl_0[7:1] == 7'b0001_001)
    begin
	  //Note: Mic-boost and Line-in settings are not handled via C&C bytes when Ozy is used, unlike when Metis is 
	  //used for HPSDR/PC comm, the Mic-boost and Line-in settings are sent to the TLV320 on Penny via
	  //the I2C bus as a result of the PC program calling Write_I2c() in the PC program to instruct the FX2 on Ozy 
	  //to send those values to the TLV320 on Penny when Ozy is present on the Atlas bus
	  IF_Drive_Level	  <= IF_Rx_ctrl_1;	    // decode drive level for PennyLane and Hermes 
	  IF_Alex[0] 		  <= {IF_Rx_ctrl_2[6], IF_Rx_ctrl_4[6:0], IF_Rx_ctrl_3[6:0]}; // Alex 1 manual relay selections
	  Alex_manual		  <= IF_Rx_ctrl_2[6];   // decode auto/manual selection for Alex (0=auto, 1=manual)
	 end
    if (IF_Rx_ctrl_0[7:1] == 7'b0001_010)
    begin
		RxN_preamps      <= IF_Rx_ctrl_1[3:0];  //decode preamp states for Mercury 1, 2, 3, 4
		penny				  <= IF_Rx_ctrl_2[7];	 //set if Penelope selected
		Merc_atten_on_Tx <= IF_Rx_ctrl_3[4];	 // 0 = disabled, 1 = enabled
	 end 
	 if (IF_Rx_ctrl_0[7:1] == 7'b00001_011)
	 begin
		IF_Alex[1]			<= {Alex_manual,IF_Rx_ctrl_3[6:0], IF_Rx_ctrl_2[6:0]}; // decode HPF/LPF settings for manual Alex 2 control
		key_reverse		   <= IF_Rx_ctrl_2[6];     // reverse CW keyes if set
		keyer_speed       <= IF_Rx_ctrl_3[5:0];  	// CW keyer speed 0-60 WPM
		keyer_mode        <= IF_Rx_ctrl_3[7:6];	// 00 = straight/external/bug, 01 = Mode A, 10 = Mode B
		keyer_weight      <= IF_Rx_ctrl_4[6:0];	// keyer weight 33-66
		keyer_spacing     <= IF_Rx_ctrl_4[7];	   // 0 = off, 1 = on
	end
	if (IF_Rx_ctrl_0[7:1] == 7'b0001_111)
	begin
	  internal_CW       <= IF_Rx_ctrl_1[0];		// decode internal CW 0 = off, 1 = on
	  sidetone_level    <= IF_Rx_ctrl_2;			// decode CW sidetone volume
	  RF_delay			  <= IF_Rx_ctrl_3;			// decode delay from pressing CW Key to RF out	
	end
	if (IF_Rx_ctrl_0[7:1] == 7'b0010_000)
	begin
		hang[9:2]			<= IF_Rx_ctrl_1;			// decode CW hang time, 10 bits
		hang[1:0]	 		<= IF_Rx_ctrl_2[1:0];
		tone_freq [11:4]  <= IF_Rx_ctrl_3;			// decode sidetone frequency, 12 bits
		tone_freq [3:0]   <= IF_Rx_ctrl_4[3:0];	
	end
  end
end	

always @ (posedge IF_clk)
begin 
  if (IF_rst)
  begin // set up default values - 0 for now
    IF_frequency[0]    <= 32'd0;
    IF_frequency[1]    <= 32'd0;
    IF_frequency[2]    <= 32'd0;
    IF_frequency[3]    <= 32'd0;
    IF_frequency[4]    <= 32'd0;
  end
  else if (IF_Rx_save)
  begin   // decode IF_frequency[c] frequencies
	if (IF_Rx_ctrl_0[7:1] == 7'b0000_001) // specifies that C1-C4 now contains Tx and Rx1 frequency if not duplex; or Tx frequency if duplex
		IF_frequency[0]   <= {IF_Rx_ctrl_1, IF_Rx_ctrl_2, IF_Rx_ctrl_3, IF_Rx_ctrl_4}; // decodes IF_frequency[0] frequency
   if (!IF_duplex && ((IF_last_chan == 3'b000) | common_Merc_freq))
		begin
		IF_frequency[1] <= IF_frequency[0];
		IF_frequency[2] <= IF_frequency[0];
		IF_frequency[3] <= IF_frequency[0];
		IF_frequency[4] <= IF_frequency[0];
		end 
  
  Decode_IF_f = (IF_Rx_ctrl_0[7:1] >= 7'd2) && (IF_Rx_ctrl_0[7:1] <= 7'd9) ? 1'b1 : 1'b0;
  
  if (Decode_IF_f & (!common_Merc_freq | IF_duplex)) // decodes IF_frequency[c] frequencies except IF_frequency[0]
		IF_frequency[IF_Rx_ctrl_0[4:1] - 1'b1] <= {IF_Rx_ctrl_1, IF_Rx_ctrl_2, IF_Rx_ctrl_3, IF_Rx_ctrl_4}; //Rx1 Rx2 Rx3 Rx4 etc frequencies		

	end
end

assign PTT_out = (IF_Rx_ctrl_0[0] | CW_PTT); // IF_Rx_ctrl_0 only updated when we get correct sync sequence


//////////////////////////////////////////////////////////////
//
//   State Machine to manage PWM interface
//
//////////////////////////////////////////////////////////////
/*

    The code loops until there are at least 4 words in the Rx_FIFO.

    The first word is the Left audio followed by the Right audio
    which is followed by I data and finally the Q data.
    	
    The words sent to the D/A converters must be sent at the sample rate
    of the A/D converters (48kHz) so is synced on the negative edge of the CLRCLK.
*/

reg   [2:0] IF_PWM_state;      // state for PWM
reg   [2:0] IF_PWM_state_next; // next state for PWM
reg  [15:0] IF_Left_Data;      // Left 16 bit PWM data for D/A converter
reg  [15:0] IF_Right_Data;     // Right 16 bit PWM data for D/A converter
reg  [15:0] IF_I_PWM;          // I 16 bit PWM data for D/A conveter
reg  [15:0] IF_Q_PWM;          // Q 16 bit PWM data for D/A conveter
reg         IF_get_samples;
wire        IF_get_rx_data;
wire        IF_bleed;
reg  [12:0] IF_bleed_cnt;
reg         IF_xmit_req;
reg         IF_xack, IF_xrdy;

// Bleed the RX FIFO if no data is being sent to Mercury/Janus or Penelope/Janus so that
// new RX Control data keeps coming in. Otherwise everything will come to a halt.  Have
// to keep getting RX Control data so we have new C & C data - specifically clock_s[].
// Bleed time is set to occur if the dead time is greater than one 24Khz period since
// CLRCLK is normally 48KHz.  Dead time can be different than one 24Khz period so long
// as it longer than one 48Khz period.  This problem occurs when switching 122.88Mhz clock
// sources
assign IF_get_rx_data = IF_get_samples || IF_bleed;

assign IF_bleed  = (IF_bleed_cnt == (48000000/12000));

localparam PWM_IDLE     = 0,
           PWM_START    = 1,
           PWM_LEFT     = 2,
           PWM_RIGHT    = 3,
           PWM_I_AUDIO  = 4,
           PWM_Q_AUDIO  = 5,
           PWM_WAIT     = 6,
           PWM_REQ      = 7;

always @ (posedge IF_clk) 
begin
  if (IF_rst)
    IF_PWM_state   <= #IF_TPD PWM_IDLE;
  else
    IF_PWM_state   <= #IF_TPD IF_PWM_state_next;

  if (IF_rst)
    IF_bleed_cnt   <= #IF_TPD 1'b0;
  else if (IF_get_rx_data)
    IF_bleed_cnt   <= #IF_TPD 1'b0; // keep clearing IF_bleed count any time we get data from RX FIFO
  else
    IF_bleed_cnt   <= #IF_TPD IF_bleed_cnt + 1'b1;

  // get Left audio
  if (IF_PWM_state == PWM_LEFT)
    IF_Left_Data   <= #IF_TPD IF_Rx_fifo_rdata;

  // get Right audio
  if (IF_PWM_state == PWM_RIGHT)
    IF_Right_Data  <= #IF_TPD IF_Rx_fifo_rdata;

  // get I audio
  if (IF_PWM_state == PWM_I_AUDIO)
    IF_I_PWM       <= #IF_TPD IF_Rx_fifo_rdata;

  // get Q audio
  if (IF_PWM_state == PWM_Q_AUDIO)
    IF_Q_PWM       <= #IF_TPD IF_Rx_fifo_rdata;

  if (IF_rst)
    IF_xmit_req    <= #IF_TPD 1'b0;
  else
    IF_xmit_req    <= #IF_TPD (IF_PWM_state == PWM_REQ) ? 1'b1 : 1'b0; // all data ready to be sent now

end

always @*
begin
  case (IF_PWM_state)
    PWM_IDLE:
    begin
      IF_Rx_fifo_rreq = 1'b0;

      if (!IF_get_rx_data)
        IF_PWM_state_next = PWM_IDLE;    // wait until time to get the donuts every 48/96/192Khz from oven (RX_FIFO)
      else if (RX_USED[RFSZ:2] == 1'b0)  // RX_USED < 4
        IF_PWM_state_next = PWM_WAIT;    // no new donuts so go empty handed = error
      else
        IF_PWM_state_next = PWM_START;   // ah! now it's time to get the donuts
    end

    // Start packaging the donuts
    PWM_START:
    begin
      IF_Rx_fifo_rreq    = 1'b1;
      IF_PWM_state_next  = PWM_LEFT;
    end

    // get Left audio
    PWM_LEFT:
    begin
      IF_Rx_fifo_rreq    = 1'b1;
      IF_PWM_state_next  = PWM_RIGHT;
    end

    // get Right audio
    PWM_RIGHT:
    begin
      IF_Rx_fifo_rreq    = 1'b1;
      IF_PWM_state_next  = PWM_I_AUDIO;
    end

    // get I audio
    PWM_I_AUDIO:
    begin
      IF_Rx_fifo_rreq    = 1'b1;
      IF_PWM_state_next  = PWM_Q_AUDIO;
    end

    // get Q audio
    PWM_Q_AUDIO:
    begin
      IF_Rx_fifo_rreq    = 1'b0;
      IF_PWM_state_next  = PWM_WAIT;
    end

    PWM_WAIT: // got donuts from oven and pack them.
    begin     // Now wait for shipping truck (NWire_xmit) ready to load them
      IF_Rx_fifo_rreq      = 1'b0;
      if (!IF_xrdy)
        IF_PWM_state_next  = PWM_WAIT;
      else
        IF_PWM_state_next  = PWM_REQ;
    end

    PWM_REQ: // load donuts and wait for shipping truck to leave
    begin
      IF_Rx_fifo_rreq      = 1'b0;
      if (!IF_xack)
        IF_PWM_state_next  = PWM_REQ;
      else
        IF_PWM_state_next  = PWM_IDLE; // truck has left the shipping dock
    end

    default:
    begin
      IF_Rx_fifo_rreq    = 1'b0;
      IF_PWM_state_next  = PWM_IDLE;
    end
  endcase
end

///////////////////////////////////////////////////////////////////////////////
//
// Left/Right Audio data transfers to Mercury(C24)/Janus(C4)
// I/Q Audio data transfer to Penelope(C19)/Janus(C12)
//
///////////////////////////////////////////////////////////////////////////////
wire       IF_m_rdy, IF_m_ack, IF_p_rdy, IF_p_ack, IF_j_rdy, IF_j_ack;

wire IF_C23, IF_C22, IF_CLRCLK;
wire IF_m_pulse, IF_p_pulse, IF_j_pulse;

cdc_sync cdc_c23 (.siga(C23), .rstb(IF_rst), .clkb(IF_clk), .sigb(IF_C23)); // C23 = M_LR_sync
pulsegen cdc_m   (.sig(IF_C23), .rst(IF_rst), .clk(IF_clk), .pulse(IF_m_pulse));

cdc_sync cdc_c22 (.siga(C22), .rstb(IF_rst), .clkb(IF_clk), .sigb(IF_C22)); // C22 = P_IQ_sync
pulsegen cdc_p   (.sig(IF_C22), .rst(IF_rst), .clk(IF_clk), .pulse(IF_p_pulse));

cdc_sync cdc_clr (.siga(C12_CLRCLK), .rstb(IF_rst), .clkb(IF_clk), .sigb(IF_CLRCLK));
pulsegen cdc_j   (.sig(IF_CLRCLK), .rst(IF_rst), .clk(IF_clk), .pulse(IF_j_pulse));

// IF_get_samples produces a single pulse telling when its time (48/96/192Khz) to get
// new data from RX_FIFO and send it to Mercury, Penelope or Janus
always @*
begin
  if (IF_conf[1])
    IF_get_samples = IF_m_pulse;  // Mercury installed so use rising edge of C23 (M_LR_sync)
  else if (IF_conf[0])
    IF_get_samples = IF_p_pulse;  // Penelope installed so use rising edge of C22 (P_IQ_sync)
  else
    IF_get_samples = IF_j_pulse;  // use Janus so use rising edge of C23 (C12_CLRCLK)
end

always @*
begin
  if (IF_conf[1])           // Mercury installed
    IF_xrdy = IF_m_rdy;
  else if (IF_conf[0])      // Penelope installed 
    IF_xrdy = IF_p_rdy;
  else
    IF_xrdy = IF_j_rdy;
end

assign IF_j_ack = !IF_j_rdy;

always @*
begin
  if (IF_conf[1])           // Mercury installed
    IF_xack = IF_m_ack;
  else if (IF_conf[0])      // Penelope installed
    IF_xack = IF_p_ack;
  else
    IF_xack = IF_j_ack;
end

wire   C12_j_rdy;
wire   C12_j_ack;
wire  [31:0] C12_LR_Data, C12_IQPWM;

cdc_sync cdc_jrdy (.siga(C12_j_rdy), .rstb(IF_rst), .clkb(IF_clk), .sigb(IF_j_rdy));
cdc_sync cdc_jack (.siga(IF_j_rdy), .rstb(C12_rst), .clkb(C12_clk), .sigb(C12_j_ack));
// transfer IF_Left_Data, IF_Right_Data into C12_clk domain.  These only change once every 48Khz
cdc_mcp #(32) lra (.a_rst(IF_rst),  .a_clk(IF_clk),  .a_data({IF_Left_Data,IF_Right_Data}), .a_data_rdy(IF_xmit_req),
                   .b_rst(C12_rst), .b_clk(C12_clk), .b_data(C12_LR_Data));

// 16 bits, two channels for PWM DAC on Mercury or Janus
NWire_xmit #(.SEND_FREQ(50000),.DATA_BITS(32), .ICLK_FREQ(144000000), .XCLK_FREQ(48000000))
  M_LRAudio (.irst(C144_rst), .iclk(C144_clk), .xrst(IF_rst), .xclk(IF_clk),
             .xdata(Rx_audio), .xreq(IF_xmit_req), .xrdy(IF_m_rdy),
             .xack(IF_m_ack), .dout(C24));

I2S_xmit #(.DATA_BITS(32))
  J_LRAudio (.rst(C12_rst), .lrclk(C12_CLRCLK), .clk(C12_CBCLK), .CBrise(C12_cbrise),
             .CBfall(C12_cbfall), .sample(C12_LR_Data), .outbit(C4),
             .xmit_rdy(C12_j_rdy), .xmit_ack(C12_j_ack));

// 16 bits, two channels for TLV320AIC23B D/A converter on Penelope or Janus
NWire_xmit #(.SEND_FREQ(50000),.DATA_BITS(32), .ICLK_FREQ(144000000), .XCLK_FREQ(48000000))
  P_IQPWM (.irst(C144_rst), .iclk(C144_clk), .xrst(IF_rst), .xclk(IF_clk),
           .xdata({I,Q}), .xreq(IF_xmit_req), .xrdy(IF_p_rdy),
           .xack(IF_p_ack), .dout(C19) );

// transfer IF_I_PWM, IF_Q_PWM into C12_clk domain.  These only change once every 48Khz
cdc_mcp #(32) iqp (.a_rst(IF_rst),  .a_clk(IF_clk),  .a_data({IF_I_PWM,IF_Q_PWM}), .a_data_rdy(IF_xmit_req),
                   .b_rst(C12_rst), .b_clk(C12_clk), .b_data(C12_IQPWM));
I2S_xmit #(.DATA_BITS(32))
  J_IQPWM (.rst(C12_rst), .lrclk(C12_CLRCLK), .clk(C12_CBCLK), .CBrise(C12_cbrise),
           .CBfall(C12_cbfall), .sample(C12_IQPWM), .outbit(C12),
           .xmit_rdy(), .xmit_ack(C12_j_ack));

///////////////////////////////////////////////////////////////
//
//              Implements Command & Control  encoder 
//
///////////////////////////////////////////////////////////////
/*
	The C&C encoder broadcasts data over the Atlas bus C20 for
	use by other cards e.g. Mercury and Penelope.
	
	The data format is as follows:
	
	<[90:89]DFS1,DFS0><[88]PTT><[87:84]address><[83:52]frequency><[51:49]clock_source><[48:42]OC>
	<[41]Mode><[40]PREAMP><[39]DITHER><[38]RAND><[37:36]ATTEN><[35:34]TX_relay><[33]Rout><[32:31]RX_relay>
	<[30:16]Alex><[15:8]Drive_Level><[7:5]multiRx_mode><[4:1]RxN_preamps><[0]common_Merc_freq> 
	
	
	Total of 91 bits. Frequency is in Hz and OC is the open collector data on Penelope.
	The clock source decodes as follows:
	
	x00  = 10MHz reference from Atlas bus ie Gibraltar
	x01  = 10MHz reference from Penelope
	x10  = 10MHz reference from Mercury
	0xx  = 122.88MHz source from Penelope 
	1xx  = 122.88MHz source from Mercury

		
	For future expansion the three bit address enables specific C&C data to be send to individual boards.
	For the present for use with Mercury and Penelope the address is ignored. 

*/

wire [90:0] IF_xmit_data;
reg   [3:0] CC_address;     // C&C address  0 - 7 
reg   [2:0] multiRx_mode;   // 000 = single receiver; 001 = two receivers, 002 = three receivers, 003 = four receivers
wire        IF_CC_rdy, IF_CC_pulse;

pulsegen CC_p   (.sig(IF_CC_rdy), .rst(IF_rst), .clk(IF_clk), .pulse(IF_CC_pulse));

// change address each data transmission 
always @ (posedge IF_clk)
begin
  if (IF_rst)
    CC_address <= #IF_TPD 1'b0;
  else if (IF_CC_pulse) // occurs at each rising edge of IF_CC_rdy
  begin
    if (CC_address == 4'd4)
      CC_address <= 1'b0; // Penny = 0
    else
      CC_address <= #IF_TPD CC_address + 1'b1; // 1 <= Mercury <= 4
  end
  multiRx_mode = IF_last_chan[2:0];  //get number of receivers being requested by the PC  
end


////////////////////////////////////////////////////////////////////////////////
// Mercury preamp (attenuator) control 
// note legacy control of Merc1 preamp via IF_preamp and RxN_preamps for control of up to 4 Mercury boards
// Turn preamp(s) OFF on Tx when Merc_atten_on_Tx is set; preamp OFF = 20 dB attenuation on Mercury

wire Merc1_preamp;
wire [3:0] MercN_preamps;

assign Merc1_preamp = ((PTT_out | CW_PTT) & Merc_atten_on_Tx) ? 1'b0 : IF_PREAMP;
assign MercN_preamps = ((PTT_out | CW_PTT) & Merc_atten_on_Tx) ? 4'b0 : RxN_preamps;

assign IF_xmit_data = {IF_DFS1,IF_DFS0,(PTT_out | CW_PTT),CC_address,IF_frequency[CC_address],IF_clock_s,IF_OC,IF_mode,Merc1_preamp,
                       IF_DITHER, IF_RAND, IF_ATTEN, IF_TX_relay, IF_Rout, IF_RX_relay, IF_Alex[CC_address], IF_Drive_Level, 
							  multiRx_mode, MercN_preamps, common_Merc_freq};

NWire_xmit  #(.DATA_BITS(91), .ICLK_FREQ(48000000), .XCLK_FREQ(48000000), .SEND_FREQ(10000)) 
      CCxmit (.irst(IF_rst), .iclk(IF_clk), .xrst(IF_rst), .xclk(IF_clk),
              .xdata(IF_xmit_data), .xreq(1'b1), .xrdy(IF_CC_rdy), .xack(), .dout(CC));


////////////////////////////////////////////////////////////////
//       I2C bus monitor module
//
//  This module is used to obtain firmware version numbers from Penelope and Mercury boards and 
//  to monitor Penelope ALC, FWD, and REV values and ADC overflow status for all Mercury boards
//  The 8051 controller in the FX2 processor on Ozy is master on the bus, controlled from
//  the PC program via the USB comm path.  This monitor module cannot/does not assert either of 
//  the SDA/SCL I2C bus lines; it only monitors signals on them to extract information to pass 
//  to the PC via the Command & Control bytes according to the USB comm protocol for HPSDR.  
//
//////////////////////////////////////////////////////////////////

wire [11:0] FWD;			// Forward Power from Penny(Lane)
wire [11:0] REV; 			// REverse Power from Penny(Lane)

I2C_monitor I2C_inst (.CLK(IF_clk), .sda(A21), .scl(A20), .Penny_FW(Penny_serialno), .Merc1_version(Merc_serialno),
                .Merc2_version(Merc2_version), .Merc3_version(Merc3_version), .Merc4_version(Merc4_version), 
					 .Merc1_ovld(ADC_OVERLOAD), .Merc2_ovld(ADC_OVERLOAD2), .Merc3_ovld(ADC_OVERLOAD3),
					 .Merc4_ovld(ADC_OVERLOAD4), .ALC(Penny_ALC), .FWD(FWD), .REV(REV));
				  

				  
           
///////////////////////////////////////////////////////
//
//  Debounce PTT from Atlas bus OR pin 6 of DB9 (active low)
//
///////////////////////////////////////////////////////


debounce de_PTT(.clean_pb(clean_PTT_in), .pb(PTT_in || ~PTT_n), .clk(IF_clk));


///////////////////////////////////////////////////////
//
//  Debounce dot key - active low
//
///////////////////////////////////////////////////////


debounce de_dot(.clean_pb(clean_dot), .pb(~dot_n), .clk(IF_clk));


///////////////////////////////////////////////////////
//
//  Debounce dash key - active low
//
///////////////////////////////////////////////////////


debounce de_dash(.clean_pb(clean_dash), .pb(~dash_n), .clk(IF_clk));


wire led0_off;
wire led3_off;

// Flash the LEDs to show something is working! - LEDs are active low

assign DEBUG_LED0 = led0_off; //D1 LED
assign DEBUG_LED1 = ~IF_conf[1];	// test config setting  
assign DEBUG_LED2 = ~PTT_out; // lights with PTT active
assign DEBUG_LED3 = led3_off; // D4 LED 
//assign DEBUG_LED1 = (IF_Rx_ctrl_0[7:1] == 0) ?  IF_Rx_ctrl_3[0] : DEBUG_LED1;
//assign DEBUG_LED2 = (IF_Rx_ctrl_0[7:1] == 0) ?  IF_Rx_ctrl_3[1] : DEBUG_LED2;

wire [3:0] err_sigs;
wire [1:0] sync_err;

reg [23:0] LRcnt;  // just for debuggin purposes to see how long a particular signal is high or low
always @(posedge IF_clk)
begin
  if (IF_rst)
    LRcnt <= 0;
  else if (IF_tx_IQ_mic_rdy)
    LRcnt <= 0;
  else
    LRcnt <= LRcnt + 1'b1;    // how long the signal is low  
end

assign err_sigs = {(RX_USED[RFSZ:2] == 1'b0), (LRcnt > 24'h60000), IF_Rx_fifo_full, IF_tx_fifo_full & IF_tx_fifo_wreq};
assign sync_err[0] = (IF_SYNC_state == SYNC_START) && IF_Rx_fifo_drdy && (IF_Rx_fifo_wdata[15:8] != 8'h7F);
assign sync_err[1] = (IF_SYNC_state == SYNC_IDLE) && IF_Rx_fifo_drdy && (IF_Rx_fifo_wdata != 16'h7F7F); // sync error
//assign sync_err[2] = tmc_err; // SignalTap test mode error

led_blinker #(3, 48000000) BLINK_D1 (IF_clk, err_sigs, led0_off);
led_blinker #(2, 48000000) BLINK_D4 (IF_clk, sync_err, led3_off);

function integer clogb2;
input [31:0] depth;
begin
  for(clogb2=0; depth>0; clogb2=clogb2+1)
  depth = depth >> 1;
end
endfunction

endmodule

