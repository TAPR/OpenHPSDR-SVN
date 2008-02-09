// V1.7  29th September 2007
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
	is sent at the negative edge of the 122.88MHz clock.
	
	The ~48kHz I and Q data from PowerSDR is interpolated by 2560 in a CIC filter to 
	give a data rate of 122.88MHz. The I and Q data in I2S format is taken from the 
	Atlas bus. 
	
	The data is processed by a CORDIC NCO and passed to the AD9744 DAC. 
	
	Command and control data is broadcast  over the Atlas bus (C20) for
	use by other cards e.g. Mercury and Penelope.  The data is in 
	I2S format with the clock being CBLCK and the start of each frame
	being indicated using the negative edge of CLRCLK.
	
	The data format is as follows:
	
	<[48]PTT><[47:44]address><[43:12]frequency><[11:8]clock_select><[7:1]OC><[0]Mode> 
	
	for a total of 49 bits. Frequency is in Hz and 32 bit binary format and 
	OC is the open collector data on Penelope.
	
	Change log:
	
	17 Jul  2007 - Modified V1 code to commence this version
	24 Aug  2007 - Added RF output bar graph on LEDs
	31 Aug  2007 - Changed CIC to interpolate by 2560 for use with 125MHz clock
	 1 Sep  2007 - Added gain after CORDIC so we can drive to 1/2W on 6m
	 3 Sept 2007 - Using alternative CORDIC
	 9 Sept 2007 - Finalised gain to compensate for loss through CIC and added ALC code. 
	14 Sept 2007 - Minor tweaks to gain distrubution
	21 Sept 2007 - Temp fix to reverse sidebands bug
	22 Sept 2007 - Added fast attach, slow decay ALC
	22 Sept 2007 - TLV320 now set up via I2C
	23 Sept 2007 - Reduced ALC gain from 2 to 1.5 to increase power out to 0.5w
	23 Sept 2007 - Added clock selection code
	26 Sept 2007 - Moved CDOUT back to Altas bus A11
	29 Sept 2007 - Only send PCLK_MCLK to bus if 125MHz clock selected as master so can share with Mercury
	 1 Oct  2007 - Added receive 125MHz clock from LVDS
	 2 Oct  2007 - Changed CORDIC output to stage 14 to reduce noise floor.
	29 Oct  2007 - Modified hardware so that 125MHz clock feeds DAC and LVDS Tx directly.
	 9 Dec  2007 - Changed clock from 125MHz to 122.88MHz so this divides exactly to n*48kHz
	
	
	
	TO DO:
	- inhibit RF output if PTT_out not active
		
	
*/

//		Atlas bus
//
//		Pin 	Signal 		Function
//		C12		CDIN		I2S format I&Q data from Ozy
//		C17		CLK_MCLK	12.288MHz master clock to Atlas bus
//		A11		CDOUT_P		I2S mic data to Ozy
//		C8		CBCLK		~3MHZ clock from Atlas via Ozy
//		C9		CLRCLK		48kHz clock from Atlas via Ozy
//		C4      LRCLK 		LR Rx Audio from Atlas via Ozy 	
//		C20		CC			Command and Control data from Ozy       
//
////////////////////////////////////////////////////////////


module Penelope(
				input   _10MHZ,
				inout   ext_10MHZ, 	// 10MHz reference to Atlas pin C16
				input   _122MHZ,
				input   _122MHZLVDS, // *** keep for now to allow testing from Mercury clock 
				//output  LVDSCLK,  	// 122.88MHz to LVDS driver
				inout A5, 			// PCLK_12MHZ (12.288MHz) to Atlas bus - tri state
				output A11,			// CDOUT_P (Mic) to Atlas bus 
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
				//output DACCLK,
				output wire [13:0]DAC,
				output nLVDSRXE,
				output LVDSTXE,
				output FPGA_PLL,
				input  PTT,			// PTT from mic connector or DB25 pin 1
				inout  PTT_in,		// PTT on Atlas bus - C15
				output nCS,
				output CMODE,
				input  CDOUT,		// Mic or Line In out to Atlas Bus
				output CBCLK,
				output CLRCIN,
				output CLRCOUT,
				output LROUT,		// LR Rx audio from Atlas C4
				output CMCLK,		// 12.288MHz CLK_MCLK from Atlas C17
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
				
wire CLK_MCLK;
wire CDIN;
wire CLRCLK;
// link through FPGA where required

assign A11 = CDOUT;			// 
assign CBCLK = C8;
assign CLK_MCLK = C17;
assign LROUT =  C4;			// Rx audio (actually CDIN on TLV320) 
assign CDIN =  C12;     	// I&Q from Atlas bus
assign CMODE = 1'b0;		// Set to 0 for I2C mode
assign nCS = 1'b1; 			// I2C address is 0x1B


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
//		CLOCKS
//
//////////////////////////////////////////////////////////////

// Generate PCLK_12MHZ for Atlas bus


reg [2:0] CMCLK_counter;
reg PCLK_12MHZ;
always @ (posedge clock)
begin
	if (CMCLK_counter == 4) // divide 122.88MHz clock by 10 to give 12.288MHz
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

// Select 122.88MHz source. If source_122MHZ set then use Penelope's 122.88MHz clock and send to LVDS
// Otherwise get external clock from LVDS

wire clock;

assign clock = source_122MHZ ? ~_122MHZ : ~_122MHZLVDS; // clock is either on board or external via LVDS
assign nLVDSRXE = source_122MHZ ? 1'b1 : 1'b0; // enable LVDS receiver if clock is external
assign LVDSTXE = source_122MHZ ? 1'b1 : 1'b0;  // enable LVDS transmitter if  Penny is the source 



// send PCLK_12MHZ to Atlas A5 if source_122.88MHZ set else set A5 high Z

assign A5 = source_122MHZ ? PCLK_12MHZ : 1'bZ; 

// select 10MHz reference source. If ref_ext is set use Penelope's 10MHz ref and send to Atlas C16

wire reference;

assign reference = ref_ext ? _10MHZ : ext_10MHZ ; 
assign ext_10MHZ = ref_ext ? _10MHZ : 1'bZ ; 		// C16 is bidirectional so set high Z if input. 


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
	Calculates  (frequency * 2^32) /122.88e6
	Each calculation takes ~ 0.6uS @ 122.88MHz
	This method is quite fast enough and uses much fewer LEs than the divide Megafunction

*/

wire [31:0]freq;
wire ready;
always @ (posedge ready)		// strobe frequency when ready is set
begin
	frequency <= frequency_HZ;	// frequecy_HZ is current frequency in Hz e.g. 14,195,000Hz
end 

 division division_DDS(.quotient(freq),.ready(ready),.dividend(frequency),.divider(32'd122880000),.clk(clock));


// sync frequency change to 122MHz clock
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
//assign PWM1_Data = ALC_q;
assign PWM1_Data = {1'b0,AIN5,3'd0}; // PWM1 has RF output envelope. 
assign PWM2_Data = {1'b0,ALC_out[20:6]}; // PWM2 has ALC volts for testing

////////////////////////////////////////////////////////////////
//
//  ALC
//
////////////////////////////////////////////////////////////////

// The flowing code provides fast attack and slow decay for the 
// ALC voltage. The output from the ALC ADC is compared with its
// previous sample. If higher, or the same, the new value is used.
// If lower then the previous value is used but decremented by 1 each 
// time through the loop. This provides a (linear) slow decay of
// approximately 2 seconds. Extend ALC input to 21 bits to 
// get sufficient delay. 

reg [20:0]ALC_out;
reg [20:0]ALC_in;
reg [20:0]previous;
reg ALC;

always @ (posedge ADCCLK)
begin
ALC_in <= {AIN5,9'd0};					// extend 12 to 21 bits
case (ALC)
0:  begin
	ALC <= 1'b1;
	if(ALC_in >= previous)
		ALC_out <= ALC_in;				// use current sample
	else
		ALC_out <= previous - 1'b1;  	// use previous sample
	end
1:  begin
	previous <= ALC_out;				// save previous sample
	ALC <= 1'b0;
	end 
endcase
end


/*
	The Automatic Level Control (ALC) works as follows. The I and Q samples are multipled 
	by a value 'gain'. This is a fractional value such that 0.9999 is represented as 9999 etc. 
	With no RF output the gain is set to 1 (actually 0.9999). When RF is produced this is
	converted into a DC level, linearized and fed to a 12 bit ADC. The output of the ADC is 
	then subtracted from the gain, hence as the RF output increase the gain reduces which
	in the asemtote is a preset level that corresponds to ~0.5W of RF output.
*/
wire [15:0]set_level;
wire [15:0]ALC_i;
wire [15:0]ALC_q;
wire [15:0]gain;

assign set_level = 16'h9999; // corresponds to 0.9999 i.e. unity gain

//wire [15:0]ALC_level = ({4'd0,ALC_out[20:9]} +  {5'd0,ALC_out[20:10]}); // gain for ALC signal (x1.5) 
wire [15:0]ALC_level = {5'd0,ALC_out[20:10]}; // gain for ALC signal (x0.5) 

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
	cic_i <= ALC_i;
end 

always @ (posedge ce_out_q)
begin
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

cicint cic_I(.clk(~clock),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(cic_i),.filter_out(cic_out_i),.ce_out(ce_out_i));
cicint cic_Q(.clk(~clock),.clk_enable(clk_enable),.reset(~clk_enable),.filter_in(cic_q),.filter_out(cic_out_q),.ce_out(ce_out_q));


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


// The phase accumulator takes a 32 bit frequency dword and outputs a 32 bit phase dword on each clock
phase_accumulator rx_phase_accumulator(.clk(clock),.reset(~clk_enable),.frequency(sync_frequency),.phase_out(phase));


// The cordic takes I and Q in along with the top 15 bits of the phase dword.  The I and Q out are freq shifted
//cordic_16 tx_cordic(.i_in({cic_out_q[15],cic_out_q[13:0],1'b0}),.q_in({cic_out_i[15],cic_out_i[13:0],1'b0}),.iout(i_out),.qout(q_out),.ain(phase[31:12]),.clk(clock));
cordic_16 tx_cordic(.i_in(cic_out_q),.q_in(cic_out_i),.iout(i_out),.qout(q_out),.ain(phase[31:12]),.clk(clock));
//cordic_16 tx_cordic(.i_in({cic_out_q[14:0],1'b0}),.q_in({cic_out_i[14:0],1'b0}),.iout(i_out),.qout(q_out),.ain(phase[31:12]),.clk(clock));


// NOTE:  I and Q inputs reversed to give correct sideband out - FIX THIS 

/* 
	We can use either the I or Q output from the CORDIC directly to drive the DAC.

   	exp(jw) = cos(w) + j sin(w)

	When multplying two complex sinusoids f1 and f2, you get only f1+f2, no
	difference frequency.

  		Z = exp(j*f1) * exp(j*f2) = exp(j*(f1+f2))
    	  = cos(f1 + f2) + j sin(f1 + f2)
*/

// Add some gain  before we feed the DAC so we can drive to 1/2W on 6m. This is necessary since the 
// interpolating CIC has a loss since it does not interpolate by 2^n. 

// sync DAC data to positive edge of clock, DAC is being clocked directly from the 122.88MHz clock or via LVDS

//always @ (posedge clock)
//begin
assign 	DAC[13:0] = {i_out[17], i_out[15:3]}; 	// use q_out if 90 degree phase shift required by EER Tx etc
//end


/////////////////////////////////////////////////////////////////
//
// Single bit PWM 16 bit D/A converters
//
/////////////////////////////////////////////////////////////////

// This runs off the 122.88MHz clock to provide adequate resolution.

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

	The C&C encoder in Ozy broadcasts data over the Atlas bus (C20) for
	use by other cards e.g. Mercury and Penelope.  The data is in 
	I2S format with the clock being CBLCK and the start of each frame
	being indicated using the negative edge of CLRCLK.
	
	The data format is as follows:
	
	<[48]PTT><[47:44]address><[43:12]frequency><[11:8]clock_select><[7:1]OC><[0]Mode> 
	
	for a total of 49 bits. Frequency is in Hz and 32 bit binary format and 
	OC is the open collector data on Penelope.
	
	The clock source decodes as follows:
	
	0x00  = 10MHz reference from Atlas bus ie Gibraltar
	0x01  = 10MHz reference from Penelope
	0x10  = 10MHz reference from Mercury
	00xx  = 122.88MHz source from Penelope 
	01xx  = 122.88MHz source from Mercury 
	
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
reg [3:0]clock_select;	// 10MHz and 122.88MHz clock selection
reg [6:0]OC;			// Open Collector outputs data
wire ref_ext;			// Set when internal 10MHz reference sent to Atlas C16
wire source_122MHZ;		// Set when internal 122.88MHz source is used and sent to LVDS

always @ (negedge CLRCLK)  
begin 
	PTT_out <= (CCdata[48]); // local PTT also sets PTT_out active to avoid delay over USB link

	Address <= CCdata[47:44];
	// check address match here in the future - ignore for now
	frequency_HZ <= CCdata[43:12];
	clock_select <= CCdata[11:8];     
	OC <= CCdata[7:1];
end

assign ref_ext = clock_select[0] ? 1'b1 :1'b0; // if set use internally and send to C16 else get from C16
assign source_122MHZ = clock_select[2] ? 1'b0 : 1'b1; // if set use internally and send to LVDS else
													  // get from LVDS 



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
	Divide the 10MHz reference and 122.88MHz clock to give 80kHz signals.
	Apply these to an EXOR phase detector. If the 10MHz reference is not
	present the EXOR output will be a 80kHz square wave. When passed through 
	the loop filter this will provide a dc level of (3.3/2)v which will
	set the 122.88MHz VCXO to its nominal frequency.
	The selection of the internal or external 10MHz reference for the PLL
	is made using  ext_ref.
	
*/

// div 10 MHz ref clock by 125 to get 80 khz 

wire ref_80khz; 
reg osc_80khz; 

oddClockDivider refClockDivider(reference, ref_80khz); 

// Divide  122.88 MHz by 1536 to get 80 khz 
reg [9:0] count_12288; 

always @ ( posedge _122MHZ ) begin
        if ( count_12288 == 767 ) begin
                count_12288 <= 0;
                osc_80khz <= ~osc_80khz; 
        end
        else begin
                count_12288 <= 1 + count_12288;
        end
end


// Apply to EXOR phase detector 
assign FPGA_PLL = ref_80khz ^ osc_80khz; 


/////////////////////////////////////////////////////////////////
//
// General I O pins
//
/////////////////////////////////////////////////////////////////


// LEDs for testing				PCB LED Marking

assign LED2 = ~ref_ext;
assign LED3 = ~source_122MHZ; 

// Bar graph for power output 
//assign LED2 = (AIN5 > 250)?  1'b0 : 1'b1;  
//assign LED3 = (AIN5 > 500)? 1'b0 : 1'b1; 
assign LED4 = (AIN5 > 1000)? 1'b0 : 1'b1;  
assign LED5 = (AIN5 > 2000)? 1'b0 : 1'b1;  
assign LED6 = (AIN5 > 3000)? 1'b0 : 1'b1;  

assign LED7 = 0;				// LED7 ON so we can see code has loaded OK 

// User open collector outputs 
assign USEROUT0 = OC[0];
assign USEROUT1 = OC[1];
assign USEROUT2 = OC[2];
assign USEROUT3 = OC[3];
assign USEROUT4 = OC[4];
assign USEROUT5 = OC[5];
assign USEROUT6 = OC[6];


endmodule 

