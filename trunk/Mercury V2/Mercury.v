/***********************************************************
*
*	Mercury DDC receiver 
*
************************************************************/

// V2.5 31 Jan 2009 

// (C) Phil Harman VK6APH 2006,2007,2008, 2009


//  HPSDR - High Performance Software Defined Radio
//
//  Mercury to Atlas bus interface.
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


/* 	This program interfaces the LT2208 to a PC over USB.
	The data from the LT2208 is in 16 bit parallel format and 
	is valid at the positive edge of the LT2208 122.88MHz clock.
	
	The data is processed by a CORDIC NCO to produce I and Q
	outputs.  These are decimated by 320/640/1280 in CIC filters then by 2 
	in an  CFIR filter to give output data at 192/96/48kHz to feed
	the Atlas bus in I2S format for tranfer via Ozy to a suitable PC program.
	
	Change log:
	
	23 Dec 2008 - Production release
	23 Dec 2008 - Added code from Alex VE3NEA that provides 48/96/192KHz sampling
	26 Dec 2008 - Working on pin drive issues
	            - Changed CORDIC to wide_phase to give 1Hz steps
	30 Dec 2008 - Changed pins to 3.0v LVTTL, set A6 and C16 to 16mA, no R, no PCI diode, output termination off
	            - added modifications by Kirk Weedman, KD7IRS.
	            - released as V2.1
	 1 Jan 2009 - Used Kirk's oddClockDiv.
                - Tidy comments
                - Move the instantiation of certain variables up higher in the lines of
                  code so that compilers like ModelSim won't produce errors
     3 Jan 2009 - Fix Alex relay data not being sent correctly
                - changed frequency to phase_word since that is correct term
     4 Jan 2009 - Code tidy by Kirk, KD7IRS, released as v2.3
     5 Jan 2009 - Added spectrum data to A12
     9 Jan 2009 - Change A10 to 16mA, no R, no PCI diode, output termination off 
			    - Added ADC overflow bit on C18
    10 Jan 2009 - Changed spectrum data to 0,1,2....4095 for testing
				- Released as V2.4
	12 Jan 2009 - Spectrum data now ADC samples
				- Simplified ADC Overlow flag code 
				- same release number
				- Added software serial number on serno, C19 
	26 Jan 2009 - fixed spectum data, was sending every 16th word. 
			    - same release no
    31 Jan 2009 - Added seperate code for serial # and spectrum 
				- released as V2.5
                 
				
	 
*/

//////////////////////////////////////////////////////////////
//
//              Quartus V8.1  Notes
//
//////////////////////////////////////////////////////////////

/*
	In order to get this code to compile without timing errors under
	Quartus  V8.1 I needed to use the following settings:
	
	- Timing Analysis Settings - Use Classic 
	- Analysis and Synthesis Settings\Power Up Dont Care [not checked]
	- Analysis and Synthesis Settings\Restructure Multiplexers  [OFF]
	- Fitter Settings\Optimise fast-corner timing [ON]
	
*/

	
module Mercury(

input         OSC_10MHZ,  // 10MHz TCXO input 
inout         ext_10MHZ,  // 10MHz reference to/from Atlas pin C16
input         CLKA,       // 122.88MHz clock from LT2208
input  [15:0] INA,        // samples from LT2208
input         CC,         // Command & Control from Atlas C20
output reg    ATTRLY,     // Antenna relay control
output        A6,         // MCLK_12MHZ (12.288MHz) to Atlas bus A6
input         C4,         // LROUT (Rx audio) from Atlas bus
input         C8,         // CBLCK from Atlas bus
input         C9,         // CLRCLK from Atlas bus
input         C17,        // CLK_MCLK from Atlas bus
output        MDOUT,      // I and Q out to Atlas bus on A10  
input         BCLK,       // 12.288MHz from Atlas bus C6 for I2S encoder
input         LRCLK,      // 192kHz from Atlas bus C7 for I2S encoder
output        CDIN,       // Rx audio out to TLV320
output        CBCLK,      // 3.072MHz BCLK from Atlas C8
output        CLRCLK,     // 48KHz L/R clock from Atlas C9
output        CLRCOUT,    // ditto 
output        CMCLK,      // 12.288MHz master clock from Atlas C17
output        CMODE,      // SPI interface to TLV320
output reg    MOSI,       // SPI interface to TLV320
output reg    SCLK,       // SPI interface to TLV320
output reg    nCS,        // SPI interface to TLV320
output        SPI_data,   // SPI data to Alex
output        SPI_clock,  // SPI clock to Alex
output        Tx_load_strobe,   // SPI Tx data load strobe to Alex
output        Rx_load_strobe,   // SPI Rx data load strobe to Alex
output        FPGA_PLL,   // PLL control volts to loop filter 
output        LVDS_TXE,   // LVDS Tx enable
output        LVDS_RXE_N, // LVDS Rx enable
input         OVERFLOW,   // ADC overflow bit
output reg    DITHER,     // ADC dither control bit
output        SHDN,       // ADC shutdown bit
output        PGA,        // ADC preamp gain
output reg    RAND,       // ADC ramdonizer bit
output        INIT_DONE,  // INIT_DONE LED 
output        TEST0,      // Test point 
output        TEST1,      // Test point
output        TEST2,      // Test point
output        TEST3,      // Test point
output        DEBUG_LED0, // Debug LED
output        DEBUG_LED1, // Debug LED
output        DEBUG_LED2, // Debug LED
output        DEBUG_LED3, // Debug LED
output        DEBUG_LED4, // Debug LED
output        DEBUG_LED5, // Debug LED
output        DEBUG_LED6, // Debug LED
output        DEBUG_LED7, // Debug LED
input         DFS0, DFS1, // I/Q sampling rate selection
output reg	  spectrum,	  // ADC samples for bandscope A12
output reg    ADC_OVERFLOW, // set when ADC overflows C18
inout  wire   serno,	    // serial # out to Atlas bus on C19
input  wire   full			// high when spectrum FIFO in Ozy is full
);


localparam SERIAL = 8'd25;  // software version serial number - 2.5
reg  [15:0] temp_ADC;
wire        CLK_122MHz;

// Assign FPGA pass through connections
assign CDIN       = C4;   // Rx audio data in I2S format to TLV320
assign CLRCLK     = C9;   // 48kHz CLRLCK from Atlas C9
assign CLRCOUT    = C9;   // ditto 
assign CBCLK      = C8;   // 3.072MHz CBCLK from Atlas C8
assign CMCLK      = C17;  // 12.288MHz CLK_MCLK from Atlas C17
// enable LT2208 
assign SHDN       = 1'b0;	// 0 = normal operation
assign INIT_DONE  = 1'b0;	// turn INIT_DONE LED on
assign CLK_122MHz = CLKA;	// use clock out of LT2208 as master clock
assign PGA        = 1'b0; 	// 1 = gain of 1.5(3dB), 0 = gain of 1


// A Digital Output Randomizer is fitted to the LT2208. This complements bits 15 to 1 if 
// bit 0 is 1. This helps to reduce any pickup by the A/D input of the digital outputs. 
// We need to de-ramdomize the LT2208 data if this is turned on. 

always @ (posedge CLK_122MHz) 
begin 
  if (RAND)
  begin	// RAND set so de-ramdomize
    if (INA[0])
      temp_ADC <= {~INA[15:1],INA[0]};
    else
      temp_ADC <= INA;
  end
  else
    temp_ADC <= INA;  // not set so just copy data
end 

//////////////////////////////////////////////////////////////
//
// 		Set up TLV320 using SPI 
//
/////////////////////////////////////////////////////////////

/* 

NOTE: TLV320 is set up via SPI rather then I2C since with
a complete system i.e. Mercury, Penelope and Janus, then 
there will be 3 TLV320s and only two options for I2C addresses.

Data to send to TLV320 is 

 	1E 00 - Reset chip
 	12 01 - set digital interface active
 	08 14 - D/A on
 	0C 00 - All chip power on
 	0E 02 - Slave, 16 bit, I2S
 	10 00 - 48k, Normal mode
 	0A 00 - turn D/A mute off

*/

reg   [2:0] load;
reg   [3:0] TLV;
reg  [15:0] TLV_data;
reg   [3:0] bit_cnt;

// Set up TLV320 data to send 
always @*	
begin
  case (load)
  //3'd0: TLV_data = 16'h8889; // simulation test case
  3'd0: TLV_data = 16'h1E00;  // data to load into TLV320
  3'd1: TLV_data = 16'h1201;
  3'd2: TLV_data = 16'h0814;		  // D/A on 
  3'd3: TLV_data = 16'h0C00;
  3'd4: TLV_data = 16'h0E02;
  3'd5: TLV_data = 16'h1000;
  3'd6: TLV_data = 16'h0A00;
  default: TLV_data = 0;
  endcase
end

// State machine to send data to TLV320 via SPI interface

assign CMODE = 1'b1;		// Set to 1 for SPI mode

reg [23:0] tlv_timeout;

always @ (posedge BCLK)		// use 12.288MHz BCLK for SPI
begin
  if (tlv_timeout != (200*12288))        // 200mS @BCLK = 12.288Mhz
    tlv_timeout <= tlv_timeout + 1'd1;

  case (TLV)
  4'd0:
  begin
    nCS <= 1'b1;        	// set TLV320 CS high
    bit_cnt <= 4'd15;   	// set starting bit count to 15
    if (tlv_timeout == (200*12288)) // wait for 200mS timeout
      TLV <= 4'd1;
    else
      TLV <= 4'd0;
  end

  4'd1:
  begin
    nCS  <= 1'b0;                // start data transfer with nCS low
    MOSI <= TLV_data[bit_cnt];  // set data up
    TLV  <= 4'd2;
  end

  4'd2:
  begin
    SCLK <= 1'b1;               // clock data into TLV320
    TLV  <= 4'd3;
  end

  4'd3:
  begin
    SCLK <= 1'b0;               // reset clock
    TLV  <= 4'd4;
  end

  4'd4:
  begin
    if (bit_cnt == 0) // word transfer is complete, check for any more
      TLV <= 4'd5;
    else
    begin
      bit_cnt <= bit_cnt - 1'b1;
      TLV <= 4'd1;    // go round again
    end
  end

  4'd5:
  begin
    if (load == 6)
    begin                 // stop when all data sent
      TLV <= 4'd5;        // hang out here forever
      nCS <= 1'b1;        // set CS high
    end
    else
    begin                 // else get next data	
      TLV  <= 4'd0;           
      load <= load + 3'b1;  // select next data word to send
    end
  end
  
  default: TLV <= 4'd0;
  endcase
end

//////////////////////////////////////////////////////////////
//
//		CLOCKS
//
//////////////////////////////////////////////////////////////

// Generate 122.88MHz/10 MCLK_12MHZ for Atlas bus

reg       MCLK_12MHZ;
reg [2:0] MCLK_count;

always @ (posedge CLK_122MHz)
begin
  if (MCLK_count == 4)  // divide 122.88MHz clock by 10 to give 12.288MHz
  begin
    MCLK_12MHZ <= ~MCLK_12MHZ;
    MCLK_count <= 0;
  end
  else
    MCLK_count <= MCLK_count + 1'b1;
end

assign A6 = MCLK_12MHZ; // send MCLK_12MHZ to Atlas A6
	
// Generate CBCLK (3.072MHz)/4 for SPI interface
	
reg SPI_clk;
reg SPI_count;
always @(posedge CBCLK)
begin
  if(SPI_count == 1)
  begin
    SPI_clk <= ~SPI_clk;
    SPI_count <=0;
  end 
  else
    SPI_count <= SPI_count + 1'b1;
end	

// Select 122.88MHz source. If source_122MHZ set then use Mercury's 122.88MHz clock and send to LVDS
// Otherwise get external clock from LVDS
wire source_122MHZ;		// Set when internal 122.88MHz source is used and sent to LVDS

assign LVDS_RXE_N = source_122MHZ ? 1'b1 : 1'b0;  // enable LVDS receiver if clock is external
assign LVDS_TXE   = source_122MHZ ? 1'b1 : 1'b0;  // enable LVDS transmitter if  Mercury is the source 

// select 10MHz reference source. If ref_ext is set use Mercury's 10MHz ref and send to Atlas C16
wire reference;
wire ref_ext;			// Set when internal 10MHz reference sent to Atlas C16

assign reference = ref_ext ? OSC_10MHZ : ext_10MHZ ; 
assign ext_10MHZ = ref_ext ? OSC_10MHZ : 1'bZ ; 		// C16 is bidirectional so set high Z if input. 

//////////////////////////////////////////////////////////////
//
//		Convert frequency to phase word 
//
//////////////////////////////////////////////////////////////

/*	
	Calculates  ratio = fo/fs = frequency/122.88Mhz where frequency is in MHz
	Each calculation should take no more than 1 CBCLK
*/

reg  [31:0] frequency_HZ;   // frequency control bits for CORDIC
reg  [31:0] phase_word;     // CBCLK domain
reg  [31:0] sync_phase_word, spw0;
wire [63:0] ratio;

localparam M2 = 32'd1172812403;  // B57 = 2^57.  B57/122880000 = M2

assign ratio = frequency_HZ * M2; // B0 * B57 number = B57 number

always @ (posedge CBCLK)   // save phase word
begin
  phase_word <= ratio[56:25]; // B57 -> B32 number since R is always >= 0
end

always @ (posedge CLK_122MHz)   
begin
  {sync_phase_word, spw0} <= {spw0, phase_word};  // from CBCLK domain to clock domain
end 


//------------------------------------------------------------------------------
//                 All DSP code is in the Receiver module
//------------------------------------------------------------------------------
wire [23:0] rx_out_data_I;
wire [23:0] rx_out_data_Q;

receiver receiver_inst(
  //control
  .clock(CLK_122MHz),
  .rate({DFS1, DFS0}), //00=48, 01=96, 10=192 kHz
  .frequency(sync_phase_word),
  .out_strobe(),
  //input
  .in_data(temp_ADC),
  //output
  .out_data_I(rx_out_data_I),
  .out_data_Q(rx_out_data_Q)
  );


// I2S encoder to send I and Q data to Atlas on C10

I2SEncode  I2S(.LRCLK(LRCLK), .BCLK(BCLK), .left_sample(rx_out_data_I), .right_sample(rx_out_data_Q), .outbit(MDOUT)); 


///////////////////////////////////////////////////////////
//
//   ADC Overflow set
//
///////////////////////////////////////////////////////////

always @ (posedge LRCLK)
	 ADC_OVERFLOW <= OVERFLOW;



//////////////////////////////////////////////////////////////
//
//		Spectrum data capture
//
//////////////////////////////////////////////////////////////

// Load consecutive ADC samples into FIFO until it is full, 4096 words.
// Wait until all data has been sent and then repeat.

reg [7:0] test_count;
always @ (negedge CLK_122MHz) 
begin
if (wrempty) begin
	wrreq <= 1'b1;			// enable FIFO write
	//test_count <= 0;
	end 
else if (wrfull) wrreq <= 1'b0;		// disable FIFO write
//if (wrreq) test_count <= test_count + 1'b1;
end

// 16 x 4096 data FIFO
reg rdreq; reg wrreq; wire wrfull; wire [15:0]q; wire wrempty;
spectrum_FIFO inst_spectrum_FIFO(.data(temp_ADC),.rdclk(~BCLK),.rdreq(rdreq),.wrclk(CLK_122MHz),.wrreq(wrreq),.q(q),.wrempty(wrempty),.wrfull(wrfull));
//spectrum_FIFO inst_spectrum_FIFO(.data({8'b0,test_count}),.rdclk(~BCLK),.rdreq(rdreq),.wrclk(CLK_122MHz),.wrreq(wrreq),.q(q),.wrempty(wrempty),.wrfull(wrfull));

// read data from spectrum_FIFO using BCLK and LRCLK, send data using I2S format over Atlas bus to Ozy
// read data using first BCLK after negative edge of LRCLK
// Send spectrum data to Ozy using I2S format on negative edge of LRCLK


// this is for spectum - need to skip if FIFO not ready
reg [1:0]spec_send;
reg [3:0]spec_data_count;
always @ (posedge BCLK)
begin	
case(spec_send)
0:  if (LRCLK)	 spec_send <= 1;	// loop until LRCLK is high
	else spec_send <= 0;
	
1:	if (LRCLK) spec_send <= 1;			// wait until it goes low - this is first BCLK after negedge of LRCLK
	else begin
		if (full) spec_send <= 0;   	// don't send data if Ozy spectrum FIFO is full, wait until next time
		else begin 
			spec_data_count <= 15; 		// need to have data available for Ozy on next BCLK
			rdreq <= 1'b1;
			spec_send <= 2;
		end 
	end
2:	begin
	rdreq <= 1'b0; 					// we have a 16 bit sample, now serialise it!
	if (spec_data_count == 0)begin
		spec_send <= 0;
	end
	else begin
		spec_data_count <= spec_data_count - 1'b1;
		spec_send <= 2;
	end
	end 
default: spec_send <= 0;
endcase
end 


// this is for serial number 
reg [1:0]sno_send;
reg [3:0]sno_data_count;

always @ (posedge BCLK)
begin	
case(sno_send)
0:  if (LRCLK)	 sno_send <= 1;	// loop until LRCLK is high
	else sno_send <= 0;
1:	if (LRCLK) sno_send <= 1;			// wait until it goes low - this is first BCLK after negedge of LRCLK
	else begin
		sno_data_count <= 15; 		// need to have data available for Ozy on next BCLK
		sno_send <= 2;
	end
2:	begin
	if (sno_data_count == 0)begin
		sno_send <= 0;
	end
	else begin
		sno_data_count <= sno_data_count - 1'b1;
		sno_send <= 2;
	end
	end 
default: sno_send <= 0;
endcase
end 

///////////////////////////////////////////////////////////
//
//    Spectrum and Serial Number Encoder 
//
///////////////////////////////////////////////////////////

// Sends current software serial # as an 8 bit value in I2S
// format. Sends the serial number as bits 7 to 0 of 16.
// Penny will use bits 15 to 8.

reg serial_number;
// I2S data must be available on the 2nd positive edge of CBCLK after the CLRCLK transition
always @ (negedge BCLK)
begin
	if (spec_send == 2)	spectrum <= q[spec_data_count];		// shift data out to Atlas bus MSB first
	if (sno_send == 2 && sno_data_count < 8)
		serial_number <= SERIAL[sno_data_count];			// send serial number
end

assign serno = (sno_send == 2 && sno_data_count < 8) ? serial_number : 1'bz;  // serial # bus is high Z


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
	
	<[58]PTT><[57:54]address><[53:22]frequency><[21:18]clock_select><[17:11]OC><[10]Mode>
	<[9]PGA><[8]DITHER><[7]RAND><[6:5]ATTEN><[4:3]TX_relay><[2]Rout><[1:0]RX_relay> 
	
	for a total of 59 bits. Frequency is in Hz and 32 bit binary format and 
	OC is the open collector data on Penelope. Mode is for a future Class E PA,
	PGA, DITHER and RAND are ADC settings and ATTEN the attenuator on Alex
	
	The clock source (clock_select) decodes as follows:
	
	0x00  = 10MHz reference from Atlas bus ie Gibraltar
	0x01  = 10MHz reference from Penelope
	0x10  = 10MHz reference from Mercury
	00xx  = 122.88MHz source from Penelope 
	01xx  = 122.88MHz source from Mercury 
	
*/

reg  [5:0] bits;     // how many bits clocked 
reg  [1:0] CC_state;
reg [58:0] CCdata;	// 59 bits of C&C data

always @(posedge CBCLK)  // use CBCLK  from Atlas C8 
begin
  case(CC_state)
  0:
  begin
    if (CLRCLK == 0)
      CC_state  <= 0;     // loop until CLRLCK is high   
    else
      CC_state  <= 1;
  end

  1:
  begin
    if (CLRCLK)
      CC_state  <= 1;     // loop until CLRCLK is low  
    else
    begin
      bits      <= 6'd58;						
      CC_state  <= 2;
    end
  end

  2:
  begin
    CCdata[bits] <= CC;   // this is the second CBCLK after negedge of CLRCLK
    if (bits == 0)
      CC_state  <= 0;      // done so restart
    else
    begin
      bits      <= bits - 1'b1;
      CC_state  <= 2;  
    end
  end

  default:
    CC_state <= 0;
  endcase
end

// decode C & C data into variables and sync to 48kHz LR clock

reg         PTT_out;
reg   [3:0] clock_select;   // 10MHz and 122.88MHz clock selection
reg   [1:0] ATTEN;          // attenuator setting on Alex
reg   [1:0] TX_relay;       // Tx relay setting on Alex
reg         Rout;           // Rx1 out on Alex
reg   [1:0] RX_relay;       // Rx relay setting on Alex
localparam  ADDRESS = 4'b0; // Address for Mercury data

//always @ (negedge CLRCLK)
//begin

always @(posedge CBCLK) // stay in same time domain as CCdata so timing analyzer can evaluate correctly
begin
  if ((CC_state == 1) && !CLRCLK) // "negedge CLRCLK" - 1st rising edge of CBCLK after CLRCLK goes low
  begin                           // grab CCdata 1 CBCLK before it starts changing again
    PTT_out     <= CCdata[58];    // PTT from PC via USB 
    if (CCdata[57:54] == ADDRESS) // check that the C&C data is for Mercury
    begin
      frequency_HZ <= CCdata[53:22];
      clock_select <= CCdata[21:18];     
      //OC        <= CCdata[17:11]; // Penelope Open Collectors, not used by Mercury
      ATTRLY    <=  ~CCdata[9];   // 1 = Attenuator on, 0 = Preamp on 
      DITHER    <= CCdata[8];     // 1 = dither on
      RAND      <= CCdata[7];     // 1 = randomizer on 
      ATTEN     <= CCdata[6:5];   // Attenuator setting on Alex
      TX_relay  <= CCdata[4:3];   // Tx relay selection on Alex
      Rout      <= CCdata[2];     // Rx_1_out on Alex
      RX_relay  <= CCdata[1:0];   // Rx relay selection on Alex
    end
  end
end

assign ref_ext = clock_select[1];       // if set use internally and send to C16 else get from C16
assign source_122MHZ = clock_select[2]; // if set use internally and send to LVDS else get from LVDS
													
//////////////////////////////////////////////////////////////
//
//		Alex Filter selection
//
//	The frequency sent by PowerSDR is the indicated frequency
//  less the 9kHz IF. In order to select filters at the correct
//  frequency we need to add the IF offset to the current frequency.
//
//////////////////////////////////////////////////////////////

wire [6:0]LPF;
wire [5:0]select_HPF;
wire [31:0]frequency_plus_IF; // add PowerSDR IF frequecy (9kHz) to current frequency

assign frequency_plus_IF = frequency_HZ + 32'd9000; // add 9kHz IF offset 

LPF_select Alex_LPF_select(.frequency(frequency_plus_IF), .LPF(LPF));
HPF_select Alex_HPF_select(.frequency(frequency_plus_IF), .HPF(select_HPF));

//////////////////////////////////////////////////////////////
//
//		Alex Antenna relay selection
//
//		Antenna relays decode as follows
//
//		TX_relay[1:0]	Antenna selected
//			00			Tx 1
//			01			Tx 2
//			10			Tx 3
//
//		RX_relay[1:0]	Antenna selected
//			00			None
//			01			Rx 1
//			10			Rx 2
//			11			Transverter
//
//		Rout			Rx_1_out
//			0			Not selected
//			1			Selected
//
//////////////////////////////////////////////////////////////

wire ANT1;			
wire ANT2;
wire ANT3;
wire Rx_1_out;
wire Transverter;
wire Rx_2_in;
wire Rx_1_in;

assign Rx_1_out = Rout;

assign ANT1 = (TX_relay == 2'b00) ? 1'b1 : 1'b0;  		// select Tx antenna 1
assign ANT2 = (TX_relay == 2'b01) ? 1'b1 : 1'b0;		// select Tx antenna 2
assign ANT3 = (TX_relay == 2'b10) ? 1'b1 : 1'b0;		// select Tx antenna 3

assign Rx_1_in = (RX_relay == 2'b01) ? 1'b1 : 1'b0;		// select Rx antenna 1
assign Rx_2_in = (RX_relay == 2'b10) ? 1'b1 : 1'b0;		// select Rx antenna 2
assign Transverter = (RX_relay == 2'b11) ? 1'b1 : 1'b0;	// select Transverter input 


//////////////////////////////////////////////////////////////
//
//		Alex SPI interface
//
//////////////////////////////////////////////////////////////

wire _6m_preamp;
wire Tx_yellow_led = 1'b1; 	// indicate we have some SPI data
wire Rx_yellow_led = 1'b1;	// ditto
wire Tx_red_led;
wire Rx_red_led;
wire TR_relay;
wire [15:0]Alex_Tx_data;
wire [15:0]Alex_Rx_data;

// assign attenuators
wire _10dB_atten = ATTEN[0];
wire _20dB_atten = ATTEN[1];

// define and concatinate the Tx data to send to Alex via SPI
assign Tx_red_led = PTT_out; // turn red led on when we Tx
assign TR_relay   = PTT_out; // turn on TR relay when PTT active

assign Alex_Tx_data = {LPF[6:4],Tx_red_led,TR_relay,ANT3,ANT2,ANT1,LPF[3:0],Tx_yellow_led,3'b000};

// define and concatinate the Rx data to send to Alex via SPI
assign Rx_red_led = PTT_out;	// turn red led on when we Tx

// turn 6m preamp on if frequency > 50MHz 
assign _6m_preamp = (frequency_plus_IF > 50000000) ? 1'b1 : 1'b0;

// if 6m preamp selected disconnect all filters 
wire [5:0] HPF;
assign HPF = _6m_preamp ? 6'd0 : select_HPF; 

// V3 Alex hardware
assign Alex_Rx_data = {Rx_red_led,_10dB_atten ,_20dB_atten, HPF[5], Rx_1_out,Rx_1_in,Rx_2_in,
                       Transverter, 1'b0, HPF[4:2],_6m_preamp,HPF[1:0],Rx_yellow_led};
					   
// concatinate Tx and Rx data and send to SPI interface. SPI interface only sends on a change of Alex_data.
// All data is sent in about 120uS.
wire [31:0] Alex_data;

assign Alex_data = {Alex_Tx_data[15:0],Alex_Rx_data[15:0]};

SPI Alex_SPI_Tx (.Alex_data(Alex_data), .SPI_data(SPI_data),
                 .SPI_clock(SPI_clock), .Tx_load_strobe(Tx_load_strobe),
                 .Rx_load_strobe(Rx_load_strobe),.spi_clock(SPI_clk));												
													
													
///////////////////////////////////////////////////////////
//
//    PLL 
//
///////////////////////////////////////////////////////////

/* 
	Divide the 10MHz reference and 122.88MHz clock to give 80kHz signals.
	Apply these to an EXOR phase detector. If the 10MHz reference is not
	present the EXOR output will be a 80kHz square wave. When passed through 
	the loop filter this will provide a dc level of (3.3/2)v which will
	set the 122.88MHz VCXO to its nominal frequency.
	The selection of the internal or external 10MHz reference for the PLL
	is made using ref_ext.
*/

// div 10 MHz ref clock by 125 to get 80 khz 

wire ref_80khz; 
reg osc_80khz; 

oddClockDivider refClockDivider(reference, ref_80khz); 

// Divide  122.88 MHz by 1536 to get 80 khz 
reg [9:0] count_12288; 

always @ (posedge CLK_122MHz)
begin
  if (count_12288 == 767)
  begin
    count_12288 <= 0;
    osc_80khz   <= ~osc_80khz; 
  end
  else
    count_12288 <= count_12288 + 1'b1;
end

// NOTE: If external reference is not available then phase detector 
// will be fed with 80kHz from 122.88MHz clock. Loop filter will 
// set VCXO control volts to 3.3v/2

// Apply to EXOR phase detector 
assign FPGA_PLL = ref_80khz ^ osc_80khz; 


// LEDs for testing 0 = off, 1 = on
assign DEBUG_LED0 = OVERFLOW; 		// LED 0 on when ADC Overflow

// check for correct Alex relay selection

assign DEBUG_LED3 = TX_relay[0]; 
assign DEBUG_LED4 = TX_relay[1];
assign DEBUG_LED5 = RX_relay[0];
assign DEBUG_LED6 = RX_relay[1];
assign DEBUG_LED7 = Rout;


// Test pins
assign TEST0 = osc_80khz; // 80kHz from 122.88MHz
assign TEST1 = ref_80khz; // 80kHz from 10MHz
assign TEST2 = FPGA_PLL;  // phase detector output
assign TEST3 = 1'b0; 

//------------------------------------------------------------------------------
//                          blink!
//------------------------------------------------------------------------------
reg [26:0]counter;
always @(posedge CLKA) counter = counter + 1'b1;
assign {DEBUG_LED2,DEBUG_LED1} = counter[23:22];  // real fast flash for this version!


endmodule 



