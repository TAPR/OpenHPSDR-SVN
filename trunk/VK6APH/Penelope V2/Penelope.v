// V1.4  9th  September 2007
//
// Copyright 2007 Phil Harman VK6APH
//
//  HPSDR - High Performance Software Defined Radio
//
//
//  Penelope Exciter.
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



/* 	
	This program interfaces the AD9744 ADC to PowerSDR over USB.
	The data to the AD9744  is in 14 bit parallel format and 
	is sent at the negative edge of the 125MHz clock.
	
	The ~48kHz I and Q data from PowerSDR is interpolated by 2560 in a CIC filter to 
	give a data rate of 125MHz. The I and Q data in I2S format is taken from the 
	Atlas bus. 
	
	The data is processed by a CORDIC NCO and passed to the AD9744 DAC. 
	
	Command and control data is broadcast  over the Atlas bus (C20) for
	use by other cards e.g. Mercury and Penelope.  The data is in 
	I2S format with the clock being CBLCK and the start of each frame
	being indicated using the negative edge of CLRCLK.
	
	The data format is as follows:
	
	<[48]PTT><[47:44]address><[43:12]frequency><[11:8]band><[7:1]OC><[0]mode> 
	
	for a total of 49 bits. Frequency is in Hz and 32 bit binary format and 
	OC is the open collector data on Penelope.
	
	Change log:
	
	17 Jul  2007 - Modified V1 code to commence this version
	24 Aug  2007 - Added RF output bar graph on LEDs
	31 Aug  2007 - Changed CIC to interpolate by 2560 for use with 125MHz clock
	 1 Sep  2007 - Added gain after CORDIC so we can drive to 1/2W on 6m
	 3 Sept 2007 - Using alternative CORDIC
	 9 Sept 2007 - Finalised gain to compensate for loss through CIC and added ALC code. 
	
	
	
	TO DO:
	
	- add ext_ref to C&C commands to enable external 10MHz reference
	- add code to enable internal 10MHz reference to appear on Atlas bus
		
	
*/

//		Atlas bus
//
//		Pin 	Signal 		Function
//		C12		CDIN		I2S format I&Q data from Ozy
//		C17		CLK_MCLK	12.5MHz master clock to Atlas bus
//		C11		CDOUT		I2S mic data to Ozy
//		C8		CBCLK		~3MHZ clock from Atlas via Ozy
//		C9		CLRCLK		48.8kHz clock from Atlas via Ozy
//		C4      LRCLK 		LR Rx Audio from Atlas via Ozy 	
//		C20		CC			Command and Control data from Ozy       
//		A2      +3.3v		Indicates Penelope is present on Altas bus
//
////////////////////////////////////////////////////////////


module Penelope(
				input   _10MHZ,
				output  ext_10MHZ, 	// 10MHz reference to Atlas pin C16
				input   _125MHZ,
				input   _125MHZLVDS,
				output  LVDSCLK,  	// 125MHz to LVDS driver
				output A2,			// set high to force Ozy to use clock on A5
				output A5, 			// PCLK_12MHZ (12.5MHz) to Atlas bus
				output A11,			// CDOUT (Mic) to Atlas bus
				input  C4, 			// LROUT (Rx audio) from Atlas bus
				input  C8,			// CBLCK from Atlas bus
				input  C9, 			// CLRCLK from Atlas bus
				input  C12,			// CDIN (I&Q) from Atlas bus
				input  C17, 		// CLK_MCLK from Atlas bus
				output LED2,
				output LED3,
				output LED4,
				output LED5,
				output LED6,
				output LED7,
				output USEROUT0,
				output USEROUT1,
				output USEROUT2,
				output USEROUT3,
				output USEROUT4,
				output USEROUT5,
				output USEROUT6,
				output DACCLK,
				output [13:0]DAC,
				output nLVDSRXE,
				output LVDSTXE,
				output FPGA_PLL,
				input  PTT,			// PTT from mic connector or DB25 pin 1
				inout  PTT_in,		// PTT on Atlas bus - C15
				output reg MOSI,
				output reg SSCK,
				output reg nCS,
				output CMODE,
				input  CDOUT,		// Mic or Line In out to Atlas Bus
				output CBCLK,
				output CLRCIN,
				output CLRCOUT,
				output LROUT,		// LR Rx audio from Atlas C4
				output CMCLK,		// 12.5MHz CLK_MCLK from Atlas C17
				input  CC, 			// Command & Control data from Ozy C12
				output ADCMOSI,
				output ADCCLK,
				input  ADCMISO,
				output nADCCS,
				output PWM0,
				output PWM1,
				output PWM2,
				output FPGA_PTT		// controls PTT FET Q3
				);
				

// link through FPGA where required

assign A11 = CDOUT;
assign CBCLK = C8;
assign CLK_MCLK = C17;
assign A5 = PCLK_12MHZ; 
assign LROUT =  C4;			// Rx audio (actually CDIN on TLV320) 
assign CDIN =  C12;     	// I&Q from Atlas bus
assign ext_10MHZ = _10MHZ; 	// 10MHz TCXO to C16 on Atlas bus 
assign A2 = 1'b1;			// force high so that Ozy uses PCLK_12MHZ as its master clock

wire clock;
//assign clock = _125MHZLVDS; // use this when using external LVDS clock 
assign clock = _125MHZ;

////////////////////////////////////////////////////////////////////////
//
//		Reset
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
// 		Set up TLV320 using SPI 
//
/////////////////////////////////////////////////////////////

/* Data to send to TLV320 is 

 	1E 00 - Reset chip
 	12 01 - set digital interface active
 	08 15 - D/A on, mic input, mic 20dB boost
   (08 14 - ditto but no mic boost)
 	0C 00 - All chip power on
 	0E 02 - Slave, 16 bit, I2S
 	10 00 - 48k, Normal mode
 	0A 00 - turn D/A mute off

*/

reg index;
reg [15:0]tdata;
reg [2:0]load;
reg [3:0]TLV;
reg [15:0] TLV_data;
reg [3:0] bit_cnt;

// Set up TLV320 data to send 

always @ (posedge index)		
begin
load <= load + 3'b1;			// select next data word to send
case (load)
3'd0: tdata <= 16'h1E00;		// data to load into TLV320
3'd1: tdata <= 16'h1201;
3'd2: tdata <= 16'h0815;
3'd3: tdata <= 16'h0C00;
3'd4: tdata <= 16'h0E02;
3'd5: tdata <= 16'h1000;
3'd6: tdata <= 16'h0A00;
default: load <= 0;
endcase
end

// State machine to send data to TLV320 via SPI interface

assign CMODE = 1'b1;		// Set to 1 for SPI mode

always @ (posedge CMCLK)	// use 12.5MHz clock for SPI
begin
case (TLV)
4'd0: begin
         nCS <= 1'b1;                   // set TLV320 CS high
         bit_cnt <= 4'd15;             	// set starting bit count to 15
         index <= ~index;               // load next data to send
         TLV <= TLV + 4'b1;
      end
 4'd1: begin
         nCS <= 1'b0;                   // start data transfer with nCS low
         TLV_data <= tdata;
         MOSI <= TLV_data[bit_cnt];    	// set data up
         TLV <= TLV + 4'b1;
       end
 4'd2: begin
         SSCK <= 1'b1;                  // clock data into TLV320
         TLV <= TLV + 4'b1;
       end
 4'd3: begin
         SSCK <= 1'b0;                  // reset clock
         TLV <= TLV + 4'b1;
       end
 4'd4: begin
          if(bit_cnt == 0) begin   		// word transfer is complete, check for any more
            index <= ~index;
            TLV <= 4'd5;
          end
          else begin
            bit_cnt <= bit_cnt - 1'b1;
            TLV <= 4'b1;                   // go round again
          end
       end                                 // end transfer
 4'd5: begin
         if (load == 7)begin               // stop when all data sent
            TLV <= 4'd5;                   // hang out here forever
            nCS <= 1'b1;                   // set CS high
         end
         else TLV <= 0;                    // else get next data
       end
 default: TLV <= 0;
 endcase
 end


//////////////////////////////////////////////////////////////
//
//		CLOCKS
//
//////////////////////////////////////////////////////////////

// Generate PCLK_12MHZ for Atlas bus


reg [2:0] CMCLK_counter;
reg PCLK_12MHZ;
always @ (posedge clock)
begin
	if (CMCLK_counter == 4) // divide 125MHz clock by 10 to give 12.5MHz
		begin
		PCLK_12MHZ <= ~PCLK_12MHZ;
		CMCLK_counter <= 0;
		end
	else CMCLK_counter <= CMCLK_counter + 3'b1;
end

// send CLRCLK to TLV320 and I2S decoder

assign CLRCIN  = C9;		// C9 on Altas bus is CLRCLK 
assign CLRCOUT = C9;
assign CLRCLK  = C9;

// send CMCLK to TLV320 

assign CMCLK = CLK_MCLK; 

// set up LVDS Driver

assign nLVDSRXE = 1'b0;		// enable LVDS receiver
assign LVDSTXE	= 1'b1; 	// enable LVDS transmitter
assign LVDSCLK = _125MHZ;   // if fitted send local clock  to LVDS transmitter

// send data to DAC on negative edge of 125MHz clock

assign DACCLK = ~clock;

//////////////////////////////////////////////////////////////
//
//		ADC SPI interface 
//
//////////////////////////////////////////////////////////////

wire [11:0]AIN5;  // holds 12 bit ADC value of Forward Power detector.

ADC ADC_SPI(.clock(CBCLK), .SCLK(ADCCLK), .nCS(nADCCS), .MISO(ADCMISO), .MOSI(ADCMOSI), .AIN5(AIN5));



//////////////////////////////////////////////////////////////
//
//		Convert frequency to phase word 
//
//////////////////////////////////////////////////////////////

/*	
	Calculates  (frequency * 2^32) /125e6
	Each calculation takes ~ 0.6uS @ 125MHz
	This method is quite fast enough and uses much fewer LEs than the divide Megafunction

*/

wire [31:0]freq;
wire ready;
always @ (posedge ready)		// strobe frequency when ready is set
begin
	frequency <= frequency_HZ;	// frequecy_HZ is current frequency in Hz e.g. 14,195,000Hz
end 

 division division_DDS(.quotient(freq),.ready(ready),.dividend(frequency),.divider(32'd125000000),.clk(clock));


// sync frequency change to 125MHz clock
reg [31:0]sync_frequency;
always @ (posedge clock)
begin
	sync_frequency <= freq;
end


////////////////////////////////////////////////////////////////
//
//  I2S Receiver for I and Q data on Atlas Bus
//
////////////////////////////////////////////////////////////////

// receive I and Q data from Atlas bus in I2S format
// I and Q data is on CDIN on pin C12 of Atlas Bus

reg [3:0] bit_count;     // how many bits clocked 
reg [2:0]IQ_state;
reg [15:0] I_data;
reg [15:0] Q_data;

always @(posedge CBCLK)
begin

case(IQ_state)
0:	begin
	if (!CLRCLK)IQ_state <= 0;					// loop until CLRLCK is high
	else IQ_state <= 1;
	end
1:	begin
	if (CLRCLK)IQ_state <= 1;					// loop until CLRCLK is low
	else begin
		bit_count <= 15;
		IQ_state <= 2;
		end
	end
2:	begin
	I_data[bit_count] <= CDIN;					// get 16 bits of I data 
		if (bit_count == 0)	IQ_state <= 3; 
		else begin
		bit_count <= bit_count - 1'b1;
		IQ_state <= 2;
		end   
	end
3:	begin
	if (!CLRCLK)IQ_state <= 3; 					// loop until CLRLCK is high
	else begin
		bit_count <= 15;
		IQ_state <= 4;
		end
	end
4:	begin
	Q_data[bit_count] <= CDIN;					// get 16 bits of Q data
		if (bit_count == 0)	IQ_state <= 0; 		// done so start again
		else begin
		bit_count <= bit_count - 1'b1;
		IQ_state <= 4;
		end					
	end 
	default: IQ_state <= 0;
endcase
end

// sync I and Q data to 48kHz LR clock

reg [15:0] I_sync_data;
reg [15:0] Q_sync_data;

always @ (negedge CLRCLK)  // use negative edge so that both I and Q are from same frame.
begin 
	I_sync_data <= I_data; 
	Q_sync_data <= Q_data;
end 

// ******  for testing send I&Q data  to PWM DACs ******

assign PWM0_Data = ALC_i;
assign PWM1_Data = ALC_q;
assign PWM2_Data = ALC_i;

////////////////////////////////////////////////////////////////
//
//  ALC
//
////////////////////////////////////////////////////////////////

/*
	The Automatic Level Control (ALC) works as follows. The I and Q samples are multipled 
	by a value 'gain'. This is a fractional value such that 0.9999 is represented as 9999 etc. 
	With no RF output the gain is set to 1 (actually 0.9999). When RF is produced this is
	converted into a DC level, linearized and fed to a 12 bit ADC. The output of the ADC is 
	then subtracted from the gain, hence as the RF output increase the gain reduces which
	in the asemtote is a preset level that corresponds to 0.5W of RF output.
*/
wire [15:0]set_level;
wire [15:0]ALC_i;
wire [15:0]ALC_q;
wire [15:0]gain;

assign set_level = 16'h9999; // corresponds to 0.9999 i.e. unity gain

wire [15:0]ALC_level = {3'd0,AIN5,1'd0}; // gain for ALC signal 

assign gain = (set_level - ALC_level);

// use this to turn ALC off

// assign gain = set_level;

// multiply I & Q by gain 

ALC ALC_I(.out(ALC_i), .sample(I_sync_data), .multiply(gain));

ALC ALC_Q(.out(ALC_q), .sample(Q_sync_data), .multiply(gain));

// latch I & Q data into  CIC when ce_out_x goes high. 

reg [15:0]cic_i;
reg [15:0]cic_q;

always @ (posedge ce_out_i)
begin
	//cic_i <= I_sync_data; // no ALC
	cic_i <= ALC_i;
end 

always @ (posedge ce_out_q)
begin
	//cic_q <= Q_sync_data;	// no ALC
	cic_q <= ALC_q;
end 

////////////////////////////////////////////////////////////////
//
//  Interpolating CIC filter  R = 2560  N = 5
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
//////////////////////////////////////////////////////////////

//Code rotates input at set frequency  and produces I & Q /

wire [17:0]i_out; 
wire [17:0]q_out; 
wire [15:0]temp_ADC;
wire [31:0] phase;
reg  [31:0]frequency;

wire [16:0]q_temp;
wire [16:0]i_temp;

	
//assign sync_frequency = 32'h1D0FA58F; // 14.190MHz i.e. FREQ /(125e6/2^32)

// The phase accumulator takes a 32 bit frequency dword and outputs a 32 bit phase dword on each clock
phase_accumulator rx_phase_accumulator(.clk(clock),.reset(~clk_enable),.frequency(sync_frequency),.phase_out(phase));

// The cordic takes I and Q in along with the top 15 bits of the phase dword.  The I and Q out are freq shifted
cordic_16 tx_cordic(.i_in(cic_out_i),.q_in(cic_out_q),.iout(i_out),.qout(q_out),.ain(phase[31:12]),.clk(clock));


/* 
	We can use either the I or Q output from the CORDIC directly to drive the DAC.

   	exp(jw) = cos(w) + j sin(w)

	When multplying two complex sinusoids f1 and f2, you get only f1+f2, no
	difference frequency.

  		Z = exp(j*f1) * exp(j*f2) = exp(j*(f1+f2))
    	  = cos(f1 + f2) + j sin(f1 + f2)
*/

// Add some gain  before we feed the DAC so we can drive to 1/2W on 6m. This is necessary since the 
// interpolating CIC has a loss because it does not interpolate by 2^n. 
// Gain is x4, using left shifts. 

assign DAC[13:0] = {i_out[17], i_out[14:2]}; 	// use q_out if 90 degree phase shift required by EER Tx etc

/////////////////////////////////////////////////////////////////
//
// Single bit PWM 16 bit D/A converters
//
/////////////////////////////////////////////////////////////////

// This runs off the 125MHz clock to provide adequate resolution.

wire [15:0] PWM0_Data;
wire [15:0] PWM1_Data;
wire [15:0] PWM2_Data;
reg [15:0] PWM0_Data_in;
reg [15:0] PWM1_Data_in;
reg [15:0] PWM2_Data_in;
reg [16:0] PWM0_accumulator;
reg [16:0] PWM1_accumulator;
reg [16:0] PWM2_accumulator;


always @(negedge clock) 
begin
        PWM0_Data_in <= PWM0_Data + 16'h8000;         // so that 0 in gives 50:50 mark/space
        PWM1_Data_in <= PWM1_Data + 16'h8000;
		PWM2_Data_in <= PWM2_Data + 16'h8000;
        PWM0_accumulator <= PWM0_accumulator[15:0] + PWM0_Data_in;
        PWM1_accumulator <= PWM1_accumulator[15:0] + PWM1_Data_in;
		PWM2_accumulator <= PWM2_accumulator[15:0] + PWM2_Data_in;
end

assign PWM0 = PWM0_accumulator[16];       // send to PWM LPFs for now 
assign PWM1 = PWM1_accumulator[16]; 
assign PWM2 = PWM2_accumulator[16]; 


///////////////////////////////////////////////////////////
//
//    Command and Control Decoder 
//
///////////////////////////////////////////////////////////

/*

	The C&C encoder broadcasts data over the Atlas bus C20 for
	use by other cards e.g. Mercury and Penelope.  The data is in 
	I2S format with the clock being CBLCK and the start of each frame
	being indicated using the negative edge of CLRCLK.
	
	The data format is as follows:
	
	<[48]PTT><[47:44]address><[43:12]frequency><[11:8]band><[7:1]OC><[0]mode> 
	
	for a total of 49 bits. Frequency is in Hz and 32 bit binary format and 
	OC is the open collector data on Penelope.
	
	TODO: add ext_ref to enable selection of exteral 10MHZ reference
	TODO: add ref_ext to enable internal 10MHz ref on Atlas C16
	
*/

reg [5:0] bits;     // how many bits clocked 
reg [1:0]CC_state;
reg [48:0] CCdata;	// 49 bits of C&C data

always @(posedge CBCLK)  
begin
case(CC_state)
0:	begin
	if (CLRCLK == 0)CC_state <= 0;			// loop until CLRLCK is high   
	else CC_state <= 1;
	end
1:	begin
		if (CLRCLK)	CC_state <= 1;			// loop until CLRCLK is low  
		else begin
		bits <= 6'd48;						
		CC_state <= 2;
		end
	end
2:	begin
	CCdata[bits] <= CC;						// this is the second CBCLK after negedge of CLRCLK
		if (bits == 0)CC_state <= 0; 		// done so restart
		else begin
		bits <= bits - 1'b1;
		CC_state <= 2;  
		end
	end
default: CC_state <= 0;
endcase
end


// decode C & C data into variables and sync to 48kHz LR clock

reg PTT_out;			// PTT to Penelope
reg [3:0]Address;		// Address in C&C header, set to 0 for now
reg [31:0]frequency_HZ;	// frequency control bits for CORDIC
reg [3:0]band;			// Frequency band to use
reg [6:0]OC;			// Open Collector outputs data
reg Mode; 				// Mode, I&Q or phase & magnitude
//reg ext_ref;			// Set when external 10MHz reference selected
//reg ref_ext;			// Set when internal 10MHz reference sent to Atlas C16

always @ (negedge CLRCLK)  
begin 
	PTT_out <= (CCdata[48]); // local PTT also sets PTT_out active to avoid delay over USB link

	Address <= CCdata[47:44];
	// check address match here in the future - ignore for now
	frequency_HZ <= CCdata[43:12];
	//frequency_HZ = 32'h1D0FA58F; // force 14.190MHz i.e. FREQ /(125e6/2^32) for testing 
	//band <= CCdata[11:8];     // not used since filters seperate from Penelope 
	OC <= CCdata[7:1];
	Mode <= CCdata[0];
	//ext_ref <= CCdata[x];
	//ref_ext <= CCdata[x];
end

///////////////////////////////////////////////////////////
//
//    PTT 
//
///////////////////////////////////////////////////////////

/* 
	The Atlas bus has a tri-state PTT line (C15 PTT_in) that
	when high indicates  a board on the bus has requested PTT. 
	When inactive set high Z, a pull down resistor on  the bus will 
	set PTT_in to logic 0.
*/

assign PTT_in = PTT ? 1'bz: 1'b1;  // set high if PTT active(low) else high Z 
assign  FPGA_PTT = PTT_out;		   // turn PTT FET Q3 on when Txing

///////////////////////////////////////////////////////////
//
//    PLL 
//
///////////////////////////////////////////////////////////

/* 
	Divide the 10MHz reference and 125MHz clock to give 2.5MHz signals.
	Apply these to an EXOR phase detector. If the 10MHz reference is not
	present the EXOR output will be a 2.5MHz square wave. When passed through 
	the loop filter this will provide a dc level of (3.3/2)v which will
	set the 125MHz VCXO to its nominal frequency.
	The selection of the internal or external 10MHz reference for the PLL
	is made using  ext_ref.
	
	TODO: if ref_out is set then send 10MHz reference to Atlas pin C16.
	
*/

wire reference;
wire ext_ref = 1'b0;  // force internal reference 

assign reference = ext_ref ? ext_10MHZ : _10MHZ;

// divide 10MHz reference clock by 4 to give 2.5MHz

reg [2:0]ref_count;
reg ref_2_5M;
reg osc_2_5M;

always @ (posedge reference)
begin
	if (ref_count == 1)
	begin
		ref_2_5M <= ~ref_2_5M;
		ref_count <= 0;
	end
	else ref_count <= ref_count + 1'b1;
end

// divide 125MHz  clock by 50 to give 2.5MHz

reg [5:0]osc_count;

always @ (posedge _125MHZ)
begin
	if (osc_count == 24)
	begin
		osc_2_5M <= ~osc_2_5M;
		osc_count <= 0;
	end 
	else osc_count <= osc_count + 1'b1;
end

// Apply to EXOR phase detector 
assign FPGA_PLL = ref_2_5M ^ osc_2_5M;


/////////////////////////////////////////////////////////////////
//
// General I O pins
//
/////////////////////////////////////////////////////////////////


// LEDs for testing				PCB LED Marking

assign LED7 = 0;				// LED7 ON so we can see code has loaded OK 

// Bar graph for power output - 50mW = 1295

assign LED2 = (AIN5 > 250)?  1'b0 : 1'b1;  // 0.1W = 1831
assign LED3 = (AIN5 > 500)? 1'b0 : 1'b1;  // 0.2W = 2590
assign LED4 = (AIN5 > 1000)? 1'b0 : 1'b1;  // 0.3W = 3172
assign LED5 = (AIN5 > 2000)? 1'b0 : 1'b1;  // 0.4W = 3663
assign LED6 = (AIN5 > 3000)? 1'b0 : 1'b1;  // 0.5W = 4096

// User outputs TODO: Map these to OC
assign USEROUT0 = 0;
assign USEROUT1 = 0;
assign USEROUT2 = 0;
assign USEROUT3 = 0;
assign USEROUT4 = 0;
assign USEROUT5 = 0;
assign USEROUT6 = 0;


endmodule 

