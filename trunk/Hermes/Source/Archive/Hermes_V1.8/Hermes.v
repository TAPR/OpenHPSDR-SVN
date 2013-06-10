/***********************************************************
*
*	Hermes - Ethernet 
*
************************************************************/


//
//  HPSDR - High Performance Software Defined Radio
//
//  Hermes code. 
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

// (C) Phil Harman VK6APH, Kirk Weedman KD7IRS  2006, 2007, 2008, 2009, 2010, 2011, 2012 





/* 	This program interfaces the LTC2208 to a PC over Ethernet.
	The data from the LTC2208 is in 16 bit parallel format and 
	is valid at the positive edge of the LTC2208 122.88MHz clock.
	
	The data is processed by a CORDIC NCO to produce I and Q
	outputs.  These are decimated by 640/1280/2560 in CIC and CFIR filters to 
	give output data at 192/96/48kHz to feed to the PHY and hence via 
	the Ethernet to a PC.
	
	The program takes microphone/line-in samples from an ADC at 48kHz
	and passed them to the PC via Ethernet. The PC process these and returns them as 
	I&Q signals.  These are passed to CIC interpolating filters (x2560) then 
	to a complex input CORDIC NCO. The ouput of the CORDIC is at the required user
	frequency and passed to the final DAC.  
	
	The data format over Ethenet is the same as that used my Metis.
	
	Change log:

	 2  Mar  2012  - Released as V1.3
	 5  April      - Added wide spectrum support. Set serial numbers for Penny and Mercury to zero
	               - Added support for second receiver - Joe Martin, K5SO
	               - Released as V1.4
	13  April      - Fixed bug in TLV320 code that muted audio when Line-in select.
	               - J13 in selects Apollo and out Alex
	               - Released as V1.5
	14  April      - Added user input IO8
					   - Released as V1.6
	21  April      - Added support for Alex in auto mode
						- tidy LED designations
						- Designated V1.7
	28  April      - Increased Apollo clock from 30kHz to 150kHz	
	 5  July       - Fixed sync byte error in Apollo.v and MAC address read error in EEPROM.v
	               - Increased wide spectrum FIFO from 4k to 16k
	13             - Increased wide spectrum FIFO to 32k for testing by DL3HVH
	18             - Test code for Hermes VNA, set to one receiver
	                 In VNA mode set the Rx and Tx CORDIC phase words to be equal. Set the I input of the Tx CORDIC to
						  0 and the Q to 0x7FFF/1.7 to remove the CORDIC gain.  Run the VNA when the PTT from the PC is active.
	 6  Aug        - using CIC and CFIR outputs with I of Tx cordic set to 0 and Q to 0x7FFF/1.7	. See line 1307.	
   11  Sep        - Use C&C (when C0 = 0001 001x, C2[7]]) to enable VNA mode. 
   15  Sep        - Set wide spectrum FIFO to 16k.  Enabled second receiver.
					   - Alex/Apollo selected via C&C rather than J13 											
                  - released as V1.7
	23  Sep			- changes by Joe K5SO
							- Modified FilterSelect to match USB protocol document v1.42 spec for C2[5] when C0[7:1]=0001001
							- Added dual-Rx automatic Alex LPF/HPF filter switching logic 
							- Added additional Alex LPF filter switching logic during transmit to accommodate SPLT mode operation correctly
							- Modified Alex Tx RED LED operation to illuminate when transmitting
							- Modified HPF/LPF automatic frequency switch-point logic  
							- Added manual Alex switching logic
							- Added line-in gain control
						- Renamed version number to V1.8
	
*** change global clock name **** 
  

NOTES: 

	LEDs:  
	
	DEBUG_LED1  	- Lights when an Ethernet broadcast is detected
	DEBUG_LED2  	- Lights when traffic to the boards MAC address is detected
	DEBUG_LED3  	- Lights when detect a received sequence error or ASMI is busy
	DEBUG_LED4 		- Displays state of PHY negotiations - fast flash if no Ethernet connection, slow flash if 100T and on if 1000T
	DEBUG_LED5		- Lights when the PHY receives Ethernet traffic
	DEBUG_LED6  	- Lights when the PHY transmits Ethernet traffic
	DEBUG_LED7  	- Displays state of DHCP negotiations or static IP - on if ACK, slow flash if NAK, fast flash if time out 
					     and long then short flash if static IP
	DEBUG_LED8  	- Lights when sync (0x7F7F7F) received from PC
	DEBUG_LED9  	- Lights when a Metis discovery packet is received
	DEBUG_LED10 	- Lights when a Metis discovery packet reply is sent
	
	Status_LED	    - Flashes once per second
	

*/

//---------------------------------------------------------
//              Quartus V11.1sp2 Notes
//---------------------------------------------------------

/*
	In order to get this code to compile without timing errors under
	Quartus V11.1sp2 I needed to use the following settings:
	
	- Timing Analysis Settings - Use Classic 
	- Analysis and Synthesis Settings\Power Up Dont Care [not checked]
	- Analysis and Synthesis Settings\Restructure Multiplexers  [OFF]
	- Analysis and Synthesis Settings\State Machine Processing = User Encoding
	- Fitter Settings\Optimise fast-corner timing [ON]
	- Restructure Multiplexers = OFF
	- Perform Physical Synthesis for Combinational Logic for Performance = ON
	- Perform Register Duplication for Performance = ON
	- Perform Register Retiming for Performance =  ON
	
*/
	

module Hermes(INA,  
				CBCLK, CLRCIN, CDOUT, CDIN, PTT, Status_LED,KEY_DOT, KEY_DASH, FPGA_PTT,
				CMCLK, LTC2208_122MHz, CLRCOUT, ATTN_CLK, ATTN_DATA, ATTN_LE, RAND, PGA, DITH, SHDN, OVERFLOW,
				USEROUT0, USEROUT1, USEROUT2, USEROUT3, USEROUT4, USEROUT5, USEROUT6,
				OSC_10MHZ, FPGA_PLL, CMODE, nCS, MOSI, SSCK, DAC_ALC, DACD,_122MHz,
				SPI_SDO, SPI_SDI, SPI_SCK, ADCMOSI, ADCCLK, ADCMISO, nADCCS,
				IO1, IO2,IO4, IO5, IO6, IO8, DEBUG_LED1, DEBUG_LED2, DEBUG_LED3, DEBUG_LED4, DEBUG_LED5, DEBUG_LED6,
				DEBUG_LED7, DEBUG_LED8, DEBUG_LED9, DEBUG_LED10,				
				PHY_TX,PHY_RX,RX_DV,PHY_TX_CLOCK,PHY_TX_EN, J15_5, J15_6, 
				PHY_RX_CLOCK,PHY_CLK125,PHY_MDIO,PHY_MDC,PHY_INT_N,PHY_RESET_N,CLK_25MHZ,
				MODE2,SCK, SI, SO, CS, NCONFIG,	ANT_TUNE );
				
parameter M_TPD   = 4;
parameter IF_TPD  = 2;

parameter  Hermes_serialno = 8'd18;		// Serial number of this version
localparam Penny_serialno = 8'd00;		// Use same value as equ1valent Penny code 
localparam Merc_serialno = 8'd00;		// Use same value as equivalent Mercury code

localparam RX_FIFO_SZ  = 4096; 			// 16 by 4096 deep RX FIFO
localparam TX_FIFO_SZ  = 1024; 			// 16 by 1024 deep TX FIFO  
localparam SP_FIFO_SZ  = 4096; 			// 16 by 4096 deep SP FIFO

localparam read_reg_address = 5'd31; 	// PHY register to read from - gives connect speed and fully duplex		

input wire [15:0]INA;						// samples from LTC2208
input wire LTC2208_122MHz;					// 122.88MHz from LTC2208_122MHz pin 
input wire _122MHz;				   		// 122.88MHz from VCXO
output wire CMCLK; 			   			// Master Clock to TLV320 
output wire Status_LED;             	// FPGA Status LED
output wire CBCLK;  							// Clocks to TLV320AIC23B
output wire CLRCIN, CLRCOUT;
output wire CDIN;                   	// Rx data to TLV320AIC23B
output wire CMODE;							// sets TLV320 mode - I2C or SPI
output wire nCS;								// chip select on TLV320
output wire MOSI;								// SPI data for TLV320
output wire SSCK;								// SPI clock for TLV320
input  wire PTT;           		    	// PTT active low
input  wire CDOUT;							// Mic data from TLV320
input  wire KEY_DOT;							// dot input from J11
input  wire KEY_DASH;						// dash input from J11
output SPI_SDO;								// SPI data to Alex or Apollo 
input  SPI_SDI;								// SPI data from Apollo 
output SPI_SCK;								// SPI clock to Alex or Apollo 
output J15_5;									// SPI Rx data load strobe to Alex / Apollo enable
output J15_6;									// SPI Tx data load strobe to Alex / Apollo ~reset
output ATTN_CLK;								// clock for input attenuator 
output ATTN_DATA;								// data for input attenuator
output ATTN_LE;								// Latch enable for input attenuator
output wire RAND;								// high turns ramdom on
output wire PGA;								// high turns LTC2208 internal preamp on
output wire DITH;								// high turns LTC2208 dither on 
output wire SHDN;								// x shuts LTC2208 off
input  wire OVERFLOW;						// high indicates LTC2208 have overflow
output wire FPGA_PTT;						// high turns Q4 on for PTTOUT
output wire USEROUT0;						// user outputs
output wire USEROUT1;
output wire USEROUT2;
output wire USEROUT3;
output wire USEROUT4;
output wire USEROUT5;
output wire USEROUT6;
output wire DEBUG_LED1;						// debug LEDs
output wire DEBUG_LED2;
output wire DEBUG_LED3;
output wire DEBUG_LED4;
output wire DEBUG_LED5;
output wire DEBUG_LED6;
output wire DEBUG_LED7;
output wire DEBUG_LED8;
output wire DEBUG_LED9;
output wire DEBUG_LED10;
input  wire OSC_10MHZ;						// 10MHz reference in	
output wire FPGA_PLL;						// 122.88MHz VCXO contol voltage
output reg  DAC_ALC;							// sets Tx DAC output level
output reg signed [13:0]DACD;				// Tx DAC data bus
output wire ADCMOSI;							// 12 bit ADC contols 
output wire ADCCLK;
input  wire ADCMISO;
output wire nADCCS;
output  wire IO1;								// high to mute AF amp				
input  wire IO2;								// PTT, used by Apollo			
input  wire IO4;								// user digital inputs
input  wire IO5;
input  wire IO6;
input  wire IO8;

// PHY
output  wire [3:0]PHY_TX;
input   wire [3:0]PHY_RX;		   
input	wire     RX_DV;						// PHY has data flag
output	wire     PHY_TX_CLOCK;			// PHY Tx data clock
output	wire	 PHY_TX_EN;					// PHY Tx enable
input	wire	 PHY_RX_CLOCK;      			// PHY Rx data clock
input	wire     PHY_CLK125;					// 125MHz clock from PHY PLL
inout	wire     PHY_MDIO;					// data line to PHY MDIO
output	wire 	 PHY_MDC;					// 2.5MHz clock to PHY MDIO
input	wire 	 PHY_INT_N;	
output	wire     PHY_RESET_N;
input	wire 	 CLK_25MHZ;						// 25MHz clock 


input wire MODE2;								// Not currently used


assign RAND = IF_RAND;
assign PGA = 0;								// 1 = gain of 3dB, 0 = gain of 0dB
assign DITH = IF_DITHER;
assign SHDN = 1'b0;							// normal LTC2208 operation

// MAC EEPROM
output wire		SCK; 							// clock on MAC EEPROM
output wire		SI;							// serial in on MAC EEPROM
input  wire 	SO; 							// SO on MAC EEPROM
output wire 	CS;							// CS on MAC EEPROM

// Reload FPGA 
output wire 	NCONFIG;						// when high causes FPGA to reload from EPCS16 

// ATU
input wire 	ANT_TUNE;

assign USEROUT0 = IF_OC[0];					
assign USEROUT1 = IF_OC[1];  				
assign USEROUT2 = IF_OC[2]; 					
assign USEROUT3 = IF_OC[3]; 		
assign USEROUT4 = IF_OC[4];
assign USEROUT5 = IF_OC[5];
assign USEROUT6 = IF_OC[6];

// enable AF Amp
assign  IO1 = 1'b0;  						// low to enable, high to mute

//--------------------------------------------------------------
// Reset Lines - C122_rst, IF_rst
//--------------------------------------------------------------

wire  IF_rst;
	
assign IF_rst 	 = (!IF_locked || reset);		// hold code in reset until PLLs are locked & PHY operational

assign PHY_RESET_N = 1'b1;  						// Allow PYH to run for now

// transfer IF_rst to 122.88MHz clock domain to generate C122_rst
cdc_sync #(1)
	reset_C122 (.siga(IF_rst), .rstb(IF_rst), .clkb(C122_clk), .sigb(C122_rst)); // 122.88MHz clock domain reset
	
//---------------------------------------------------------
//		CLOCKS
//---------------------------------------------------------

wire C122_clk = LTC2208_122MHz;
wire IF_clk;
wire CLRCLK;

wire C122_cbclk, C122_cbrise, C122_cbfall;
Hermes_clk_lrclk_gen clrgen (.reset(C122_rst), .CLK_IN(C122_clk), .BCLK(C122_cbclk),
                             .Brise(C122_cbrise), .Bfall(C122_cbfall), .LRCLK(CLRCLK));

assign CBCLK   = C122_cbclk;
assign CLRCIN  = CLRCLK;
assign CLRCOUT = CLRCLK;


// Generate C122_cbclk/4 for Alex SPI interface
wire      SPI_clk;
reg       [1:0] spc;

always @(posedge C122_cbclk)  
begin
  if (C122_rst)
    spc <= 2'b00;
  else  spc <= spc + 2'b01;
end

assign SPI_clk = spc[1];

wire	Apollo_clk;
wire 	IF_locked;
// Generate IF_clk (48MHz), CMCKL (12.288MHz) and Apollo_clk(150kHz*) from 122.88MHz using PLL  *** was 30kHz
PLL_IF PLL_IF_inst (.inclk0(C122_clk), .c0(IF_clk), .c1(CMCLK), .c2(Apollo_clk), .locked(IF_locked));

//----------------------------PHY Clocks-------------------

wire C125_clk; 	assign C125_clk = PHY_CLK125;	// use PHY 125MHz clock for system clock
wire Tx_clock;
wire Tx_clock_2;
wire C125_locked; 										// high when PLL locked
wire PHY_data_clock;
wire PHY_speed;											// 0 = 100T, 1 = 1000T
wire EEPROM_clock;										// 2.5MHz

// use PLL to generate 2.5MHz, 25MHz and 12.5MHz from 125MHz
// C0 = 2.5MHz, C1 = 25MHz, C2 = 12.5MHz

PLL_clocks PLL_clocks_inst(.areset(), .inclk0(C125_clk), .c0(EEPROM_clock), .c1(Tx_clock), .c2(Tx_clock_2), .locked(C125_locked));

assign PHY_TX_CLOCK = ~Tx_clock;

assign PHY_speed = 1'b0;		// high for 1000T, low for 100T; force 100T for now

// select data clock speed based on JP2 and speed that network is running at
// assign PHY_data_clock = (PHY_speed & speed_1000T) ? PHY_RX_CLOCK : ~PHY_100T_state;

// force 100T for now 
assign PHY_data_clock = ~PHY_100T_state;

//------------------------------------------------------------
//  Reset and initialisation
//------------------------------------------------------------

/* 
	Hold the code in reset whilst we do the following:
	
	Get the boards MAC address from the EEPROM.
	
	Then setup the PHY registers and read from the PHY until it indicates it has 
	negotiated a speed.  Read connection speed and that we are running full duplex.
	
	LED0 incates PHY status - fast flash if no Ethernet connection, slow flash if 100T and on if 1000T
	
	Then wait a second (for the network to stabilise) then  attempt to obtain an IP address using DHCP
	- supplied address is in YIADDR.  If the DHCP request either times out, or results in a NAK, retry four 
	additional times with a 2 second delay between each retry.
	
	If after the retries a DHCP assigned IP address is not available use an APIPA IP address or an assigned one
	from Flash.
	
	Inhibit replying to a Metis Discovery request until an IP address has been applied.
	
	LED6 indicates the result of DHCP - on if ACK, slow flash if NAK, fast flash if time out and 
	long then short flash if static IP
	
	Once an IP address has been assigned set IP_valid flag. When set enables a response to a Discovery request.
	
	Wait for a Metis discovery frame - once received enable HPSDR data to PC.
	
	Enable rest of code.
	
*/

reg reset;
reg [4:0]start_up;
reg [47:0]This_MAC; 			// holds the MAC address of this Metis board
reg read_MAC; 
wire MAC_ready;
reg DHCP_start;
reg [24:0]delay;
reg duplex;						// set when we are connected full duplex
reg speed_100T;				// set when we are connected at 100MHz
reg speed_1000T;				// set when we are connected at 1GHz
reg Tx_reset;					// when set prevents HPSDR UDP/IP Tx data being sent
reg [2:0]DHCP_retries;		// DHCP retry counter
reg IP_valid;					// set when Metis has a valid IP address assigned by DHCP or APIPA
reg Assigned_IP_valid;		// set if IP address assigned by PC is not 0.0.0.0. or 255.255.255.255
reg use_IPIPA;					// set when no DHCP or assigned IP available so use APIAP

always @ (posedge Tx_clock_2)
begin
	case (start_up)
	// get the MAC address for this board
0:	begin 
		IP_valid <= 1'b0;							// clear IP valid flag
		Assigned_IP_valid <= 1'b0;				// clear IP in flash memory valid
		reset <= 1'b1;
		Tx_reset <= 1'b1;							// prevent I&Q data Tx until all initialised 
		read_MAC <= 1'b1;
		use_IPIPA <= 0;							// clear IPIPA flag
		start_up <= start_up + 1'b1;
	end
	// wait until we have read the EEPROM then the IP address
1:  begin
		if (MAC_ready) begin 					// MAC_ready goes high when EEPROM read
			read_MAC <= 0;
			read_IP <= 1'b1;						// set read IP flag
			start_up <= start_up + 1'b1;
		end
		else start_up <= 1'b1;
	end
	// read the IP address from EEPROM then set up the PHY
2:	begin
		if (IP_ready) begin
			read_IP <= 0;
    		write_PHY <= 1'b1;					// set write to PHY flag
			start_up <= start_up + 1'b1;
		end
		else start_up <= 2;    
    end			
	// check the IP address read from the flash memory is valid. Set up the PHY MDIO registers
3: begin
	   if (AssignIP != 0 && AssignIP != 32'hFF_FF_FF_FF)
			Assigned_IP_valid <= 1'b1;	
	   if (write_done) begin
			write_PHY <= 0;						// clear write PHY flag so it does not run again
			duplex <= 0;							// clear duplex and speed flags
			speed_100T <= 0;
			speed_1000T <= 0; 
			read_PHY <= 1'b1;						// set read from PHY flag
			start_up <= start_up + 1'b1;
		end 
		else start_up <= 3;						// loop here till write is done
	end 
	
	// loop reading PHY Register 31 bits [3],[5] & [6] to determine if final connection is full duplex at 100T or 1000T.
	// Set speed and duplex bits.
	// If an IP address has been assigned (i.e. != 0) then continue else	
	// once connected delay 1 second before trying DHCP to give network time to stabilise.
4: begin
		if (read_done  && (register_data[5] || register_data[6])) begin
			duplex <= register_data[3];			// get connection status and speed
			speed_100T  <= register_data[5];
			speed_1000T <= register_data[6];
			read_PHY <= 0;								// clear read PHY flag so it does not run again	
			reset <= 0;	
			if (duplex) begin							// loop here is not fully duplex network connection
				// if an IP address has been assigned then skip DHCP etc
				if (Assigned_IP_valid) start_up <= 6;
				// allow rest of code to run now so we can get IP address. If 						
				else if (delay == 12500000) begin	// delay 1 second so that PHY is ready for DHCP transaction
					DHCP_start <= 1'b1;					// start DHCP process
					if (time_out)							// loop until the DHCP module has cleared its time_out flag
						start_up <= 4;
					else begin
						delay <= 0;							// reset delay for DHCP retries
						start_up <= start_up + 1'b1;
					end 
				end 
				else delay <= delay + 1'b1;
			end 
		end 
		else start_up <= 4;								// keep reading Register 1 until we have a valid speed and full duplex		
   end 

	// get an IP address from the DHCP server, move to next state if successful, retry 3 times if NAK or time out.		
5:  begin 
		DHCP_start <= 0;
		if (DHCP_ACK) 										// have DHCP assigned IP address so continue
			start_up <= start_up + 1'b1;
		else if (DHCP_NAK || time_out) begin		// try again 3 more times with 1 second delay between attempts
			if (DHCP_retries == 3) begin				// no more DHCP retries so use IPIPA address and  continue
				use_IPIPA <= 1'b1;
				start_up <= start_up + 1'b1;
			end
			else begin
				DHCP_retries <= DHCP_retries + 1'b1;	// try DHCP again
				start_up <= 4;
			end	
		end		
		else start_up <= 5;
	end
	
	// Have a valid IP address and a full duplex PHY connection so enable Tx code 
6:  begin
	IP_valid <= 1'b1;					// we now have a valid IP address so can respond to Discovery requests etc
	Tx_reset <= 0;						// release reset so UDP/IP Tx code can run
	start_up <= start_up + 1'b1;						
	read_PHY <= 1'b1;					// set read from PHY flag
	end
	// loop checking we still have a Network connection by reading speed from PHY registers - restart if network connection lost
7:	begin
		if (read_done) begin 
			read_PHY <= 0;
			if (register_data[5] || register_data[6])
				start_up <= 6;								// network connection OK
			else start_up <= 0;							// lost network connection so re-start
		end 
	end
	default: start_up <= 0;
    endcase
end 

//----------------------------------------------------------------------------------
// read and write to the EEPROM	(NOTE: Max clock frequency is 20MHz)
//----------------------------------------------------------------------------------
wire read_IP;
wire IP_ready;
				
EEPROM EEPROM_inst(.clock(EEPROM_clock), .read_MAC(read_MAC), .read_IP(read_IP), .write_IP(), 
				   .IP_to_write(), .CS(CS), .SCK(SCK), .SI(SI), .SO(SO), .This_MAC(This_MAC),
				   .This_IP(AssignIP), .MAC_ready(MAC_ready), .IP_ready(IP_ready), .IP_write_done());
				   				 
								
//------------------------------------------------------------------------------------
//  If DHCP provides an IP address for Metis use that else use a random APIPA address
//------------------------------------------------------------------------------------

// Use an APIPA address of 169.254.(last two bytes of the MAC address)

wire [31:0] This_IP;
wire [31:0]AssignIP;			// IP address read from EEPROM

assign This_IP =  Assigned_IP_valid ? AssignIP : 
				          use_IPIPA ? {8'd169, 8'd254, This_MAC[15:0]} : YIADDR;

//----------------------------------------------------------------------------------
// Read/Write the  PHY MDIO registers (NOTE: Max clock frequency is 2.5MHz)
//----------------------------------------------------------------------------------
wire write_done; 
reg write_PHY;
reg read_PHY;
wire PHY_clock;
wire read_done;
wire [15:0]register_data; 
wire PHY_MDIO_clk;
assign PHY_MDIO_clk = EEPROM_clock;

MDIO MDIO_inst(.clk(PHY_MDIO_clk), .write_PHY(write_PHY), .write_done(write_done), .read_PHY(read_PHY),
	  .clock(PHY_MDC), .MDIO_inout(PHY_MDIO), .read_done(read_done),
	  .read_reg_address(read_reg_address), .register_data(register_data),.speed(PHY_speed));

//----------------------------------------------------------------------------------
//  Renew the DHCP supplied IP address at half the lease period
//----------------------------------------------------------------------------------

/*
	Request a DHCP IP address at IP_lease/2 seconds if we have a valid DHCP assigned IP address.
	The IP_lease is obtained from the DHCP server and returned during the DHCP ACK.
	This is the number of seconds that the IP lease is valid. 
	
	Divide this value by 2 then multiply by the clock rate to give the delay time.
	
	If an IP_lease time of zero is received then the lease time is set to 24 days.
*/

wire [51:0]lease_time;
assign lease_time = (IP_lease == 0) ?  52'h7735_8C8C_A6C0 : (IP_lease >> 1) * 12500000; // 24 days if no lease time given
// assign lease_time = (IP_lease == 0) ? 52'h7735_8C8C_A6C0  : (52'd4 * 52'd12500000);  // every 4 seconds for testing


reg [24:0]IP_delay;
reg DHCP_renew;
reg [3:0]renew_DHCP_retries;
reg [51:0]renew_counter;
reg [24:0]renew_timer; 
reg [2:0]renew;
reg printf;
reg DHCP_request_renew;
reg second_time;						// set if can't get a DHCP IP address after two tries.
reg DHCP_discover_broadcast;    // last ditch attempt so do a discovery broadcast

always @(posedge Tx_clock_2)
begin 
case (renew)

0:	begin 
	renew_timer <= 0;
		if (DHCP_ACK) begin							 // only run if we have a  valid DHCP supplied IP address
			if (renew_counter == lease_time )begin
				renew_counter <= 0;
				renew <= renew + 1'b1;
			end
			else renew_counter <= renew_counter + 1'b1;
		end 
		else renew <= 0;
	end 
// Renew DHCP IP address
1:	begin
		if (second_time) 
			renew <= 4;
		else begin 
			DHCP_request_renew <= 1'b1;
			renew <= renew + 1'b1;
		end 
	end

// delay so the request is seen then return
2:	renew <= renew + 1'b1;

 
// get an IP address from the DHCP server, move to next state if successful, if not reset lease timer to 1/4 previous value
3: begin
	DHCP_request_renew <= 0;
		if (renew_timer != 2 * 12500000) begin  // delay for 2 seconds before we look for ACK, NAK or time_out
			renew_timer <= renew_timer + 1'b1;
			renew <= 3;
		end 		
		else begin
			if (DHCP_NAK || time_out) begin		// did not renew so set timer to lease_time/4
				second_time <= 1'b1;
				renew_counter = (lease_time - lease_time >> 4);  // i.e. 0.75 * lease_time
				renew <= 0;
			end
			else begin	
				renew_counter <= 0; 					// did renew so reset counter and continue.
				renew <= 0;
			end 
		end
	end 

// have not got an IP address the second time we tryed so use a broadcast and loop here
4:	begin 
	DHCP_discover_broadcast <= 1'b1;				// do a DHCP discovery
	renew <= renew + 1'b1;
	end 
	
// if we get a DHCP_ACK then continue else give up 
5:	begin
	DHCP_discover_broadcast <= 0;
		if (renew_timer != 2 * 12500000) begin  // delay for 2 seconds before we look for ACK, NAK or time_out
			renew_timer <= renew_timer + 1'b1;
			renew <= 5;
		end 
		else if (DHCP_NAK || time_out) 			// did not renew so give up
				renew <= 5;
		else begin 										// did renew so continue
			second_time <= 0;
			renew <= 0;
		end 
	end 	
default: renew <= 0;
endcase
end 

//----------------------------------------------------------------------------------
//  See if we can get an IP address using DHCP
//----------------------------------------------------------------------------------

wire time_out;
wire DHCP_request;

DHCP DHCP_inst(Tx_clock_2, (DHCP_start || DHCP_discover_broadcast), DHCP_renew, DHCP_discover , DHCP_offer, time_out, DHCP_request, DHCP_ACK);

//---------------------------------------------------------
// 		Set up TLV320 using SPI 
//---------------------------------------------------------

TLV320_SPI TLV (.clk(CMCLK), .CMODE(CMODE), .nCS(nCS), .MOSI(MOSI), .SSCK(SSCK), .boost(IF_Mic_boost), .line(IF_Line_In), .line_in_gain(IF_Line_In_Gain));

//-----------------------------------------------------
//   Rx_MAC - PHY Receive Interface  
//-----------------------------------------------------

wire [7:0]ping_data[0:59];
wire [15:0]Port;
wire [15:0]Discovery_Port;		// PC port doing a Discovery
wire broadcast;
wire ARP_request;
wire ping_request;
wire Rx_enable;
wire this_MAC;  					// set when packet addressed to this MAC
wire DHCP_offer; 					// set when we get a valid DHCP_offer
wire [31:0]YIADDR;				// DHCP supplied IP address for this board
wire [31:0]DHCP_IP;  			// IP address of DHCP server offering IP address 
wire DHCP_ACK, DHCP_NAK;
wire [31:0]PC_IP;					// IP address of the PC we are connecting to
wire [31:0]Discovery_IP;		// IP address of the PC doing a Discovery
wire [47:0]PC_MAC;				// MAC address of the PC we are connecting to
wire [47:0]Discovery_MAC;		// MAC address of the PC doing a Discovery
wire [31:0]Use_IP;				// Assigned IP address, if zero then use DHCP
wire METIS_discovery;			// pulse high when Metis_discovery received
wire [47:0]ARP_PC_MAC; 			// MAC address of PC requesting ARP
wire [31:0]ARP_PC_IP;			// IP address of PC requesting ARP
wire [47:0]Ping_PC_MAC; 		// MAC address of PC requesting ping
wire [31:0]Ping_PC_IP;			// IP address of PC requesting ping
wire [15:0]Length;				// Lenght of frame - used by ping
wire data_match;					// for debug use 
wire PHY_100T_state;				// used as system clock at 100T
wire [7:0] Rx_fifo_data;		// byte from PHY to send to Rx_fifo
wire rs232_write_strobe;
wire seq_error;					// set when we receive a sequence error
wire run;							// set to send data to PC
wire wide_spectrum;				// set to send wide spectrum data
wire [31:0]IP_lease;				// holds IP lease in seconds from DHCP ACK packet
wire [47:0]DHCP_MAC;				// MAC address of DHCP server 
wire erase;							// set when we receive an erase EPCS16 command
wire erase_ACK;					// set when ASMI interface acks the erase command
wire [31:0]num_blocks;			// number of 256 byte blocks to save in EPCS16
wire EPCS_FIFO_enable;			// EPCS fifo write enable


Rx_MAC Rx_MAC_inst (.PHY_RX_CLOCK(PHY_RX_CLOCK), .PHY_data_clock(PHY_data_clock),.RX_DV(RX_DV), .PHY_RX(PHY_RX),
			        .broadcast(broadcast), .ARP_request(ARP_request), .ping_request(ping_request),  
			        .Rx_enable(Rx_enable), .this_MAC(this_MAC), .Rx_fifo_data(Rx_fifo_data), .ping_data(ping_data),
			        .DHCP_offer(DHCP_offer),
			        .This_MAC(This_MAC), .YIADDR(YIADDR), .DHCP_ACK(DHCP_ACK), .DHCP_NAK(DHCP_NAK),
			        .METIS_discovery(METIS_discovery), .METIS_discover_sent(METIS_discover_sent), .PC_IP(PC_IP), .PC_MAC(PC_MAC),
			        .This_IP(This_IP), .Length(Length), .PHY_100T_state(PHY_100T_state),
			        .ARP_PC_MAC(ARP_PC_MAC), .ARP_PC_IP(ARP_PC_IP), .Ping_PC_MAC(Ping_PC_MAC), 
			        .Ping_PC_IP(Ping_PC_IP), .Port(Port), .seq_error(seq_error), .data_match(data_match),
			        .run(run), .IP_lease(IP_lease), .DHCP_IP(DHCP_IP), .DHCP_MAC(DHCP_MAC),
			        .erase(erase), .erase_ACK(erase_ACK), .num_blocks(num_blocks), .EPCS_FIFO_enable(EPCS_FIFO_enable),
			        .wide_spectrum(wide_spectrum)
			        );
			        


//-----------------------------------------------------
//   Tx_MAC - PHY Transmit Interface  
//-----------------------------------------------------

wire [10:0] PHY_Tx_rdused;  
wire LED;
wire Tx_fifo_rdreq;
wire ARP_sent;
wire  DHCP_discover;
reg  [7:0] RS232_data;
reg  RS232_Tx;
wire DHCP_request_sent;
wire DHCP_discover_sent;
wire METIS_discover_sent;
wire Tx_CTL;
wire [3:0]TD;


Tx_MAC Tx_MAC_inst (.Tx_clock(Tx_clock), .Tx_clock_2(Tx_clock_2), .IF_rst(IF_rst),
					.Send_ARP(Send_ARP),.ping_reply(ping_reply),.PHY_Tx_data(PHY_Tx_data),
					.PHY_Tx_rdused(PHY_Tx_rdused), .ping_data(ping_data), .LED(LED),
					.Tx_fifo_rdreq(Tx_fifo_rdreq),.Tx_CTL(PHY_TX_EN), .ARP_sent(ARP_sent),
					.ping_sent(ping_sent), .TD(PHY_TX),.DHCP_request(DHCP_request),
					.DHCP_discover_sent(DHCP_discover_sent), .This_MAC(This_MAC),
					.DHCP_discover(DHCP_discover), .DHCP_IP(DHCP_IP), .DHCP_request_sent(DHCP_request_sent),
					.METIS_discovery(METIS_discovery), .PC_IP(PC_IP), .PC_MAC(PC_MAC), .Length(Length),
			        .Port(Port), .This_IP(This_IP), .METIS_discover_sent(METIS_discover_sent),
			        .ARP_PC_MAC(ARP_PC_MAC), .ARP_PC_IP(ARP_PC_IP), .Ping_PC_IP(Ping_PC_IP),
			        .Ping_PC_MAC(Ping_PC_MAC), .speed_100T(1'b1), .Tx_reset(Tx_reset),
			        .run(run), .IP_valid(IP_valid), .printf(printf), .IP_lease(IP_lease),
			        .DHCP_MAC(DHCP_MAC), .DHCP_request_renew(DHCP_request_renew),
			        .erase_done(erase_done), .erase_done_ACK(erase_done_ACK), .send_more(send_more),
			        .send_more_ACK(send_more_ACK), .Hermes_serialno(Hermes_serialno),
			        .sp_fifo_rddata(sp_fifo_rddata), .sp_fifo_rdreq(sp_fifo_rdreq), 
			        .sp_fifo_rdused(), .wide_spectrum(wide_spectrum), .have_sp_data(sp_data_ready)
			        ); 

//------------------------ sequence ARP and Ping requests -----------------------------------

// send ARP reply, set Send_ARP on the request and clear when done ** may not be requried if ARP_request is not too fast.
reg Send_ARP;
reg ping_sent;
reg ping_reply;

always @ (posedge PHY_RX_CLOCK)
begin
	if (ARP_request)
		Send_ARP <= 1'b1;
	if (ARP_sent)
		Send_ARP <= 0;
	if (ping_request)
		ping_reply <= 1'b1;
	if (ping_sent)
		ping_reply <= 0;	
end

//----------------------------------------------------
//   Receive PHY FIFO 
//----------------------------------------------------

/*
					    PHY_Rx_fifo (16k bytes) 
					
						---------------------
	  Rx_fifo_data |data[7:0]	  wrfull | PHY_wrfull ----> Flash LED!
						|				         |
		Rx_enable	|wrreq				   |
						|					      |									    
	PHY_data_clock	|>wrclk	 			   |
						---------------------								
  IF_PHY_drdy     |rdreq		  q[15:0]| IF_PHY_data [swap Endian] 
					   |					      |					  			
			       	|   		     rdempty| IF_PHY_rdempty 
			         |                    | 							
			 IF_clk	|>rdclk rdusedw[12:0]| 		    
					   ---------------------								
					   |                    |
			 IF_rst  |aclr                |								
					   ---------------------								
 
 NOTE: the rdempty stays asserted until enough words have been written to the input port to fill an entire word on the 
 output port. Hence 4 writes must take place for this to happen. 
 Also, rdusedw indicates how many 16 bit samples are available to be read. 
 
*/

wire PHY_wrfull;
wire IF_PHY_rdempty;
wire IF_PHY_drdy;


PHY_Rx_fifo PHY_Rx_fifo_inst(.wrclk (PHY_data_clock),.rdreq (IF_PHY_drdy),.rdclk (IF_clk),.wrreq(Rx_enable),
                .data (Rx_fifo_data),.q ({IF_PHY_data[7:0],IF_PHY_data[15:8]}), .rdempty(IF_PHY_rdempty),
                .wrfull(PHY_wrfull),.aclr(IF_rst | PHY_wrfull));


					 
					 
//------------------------------------------------
//   SP_fifo  (16384 words) dual clock FIFO
//------------------------------------------------

/*
        The spectrum data FIFO is 16 by 16384 words long on the input.
        Output is in Bytes for easy interface to the PHY code
        NB: The output flags are only valid after a read/write clock has taken place

       
							   SP_fifo
						---------------------
	      temp_ADC |data[15:0]	   wrfull| sp_fifo_wrfull
						|				         |
	sp_fifo_wrreq	|wrreq	     wrempty| sp_fifo_wrempty
						|				         |
			C122_clk	|>wrclk              | 
						---------------------
	sp_fifo_rdreq	|rdreq		   q[7:0]| sp_fifo_rddata
						|                    | 
						|				         |
		Tx_clock_2	|>rdclk		         | 
						|		               | 
						---------------------
						|                    |
	 C122_rst OR   |aclr                |
		!run   	   |                    |
	    				---------------------
		
*/

wire  sp_fifo_rdreq;
wire [7:0]sp_fifo_rddata;
wire sp_fifo_wrempty;
wire sp_fifo_wrfull;
wire sp_fifo_wrreq;


//--------------------------------------------------
//   Wideband Spectrum Data 
//--------------------------------------------------

//	When wide_spectrum is set and sp_fifo_wrempty then fill fifo with 16k words 
// of consecutive ADC samples.  Pass have_sp_data to Tx_MAC to indicate that 
// data is available.
// Reset fifo when !run so the data always starts at a known state.


wire have_sp_data;


SP_fifo  SPF (.aclr(C122_rst | !run), .wrclk (C122_clk), .rdclk(Tx_clock_2), 
             .wrreq (sp_fifo_wrreq), .data (temp_ADC), .rdreq (sp_fifo_rdreq),
             .q(sp_fifo_rddata), .wrfull(sp_fifo_wrfull), .wrempty(sp_fifo_wrempty)); 					 
					 
					 
sp_rcv_ctrl SPC (.clk(C122_clk), .reset(C122_rst), .sp_fifo_wrempty(sp_fifo_wrempty),
                 .sp_fifo_wrfull(sp_fifo_wrfull), .write(sp_fifo_wrreq), .have_sp_data(have_sp_data));	
				 
// the wideband data is presented too fast for the PC to swallow so slow down to 12500/4096 = 3kHz
// use a counter and when zero enable the wide spectrum data

reg [15:0]sp_delay;   
wire sp_data_ready;

always @ (posedge Tx_clock_2)
		sp_delay <= sp_delay + 15'd1;
		
assign sp_data_ready = (sp_delay == 0 && have_sp_data); 


	
//--------------------------------------------------------------------------
//			EPCS16 Erase and Program code 
//--------------------------------------------------------------------------

/*
					    EPCS_fifo (1k bytes) 
					
					    ---------------------
	  Rx_fifo_data  |data[7:0]	         | 
					    |				         |
 EPCS_FIFO_enable  |wrreq		         | 
					    |					      |									    
	PHY_data_clock  |>wrclk	 			   |
					    ---------------------								
	   EPCS_rdreq   |rdreq		  q[7:0] | EPCS_data
					    |					      |					  			
			     	    |   		            |  
			          |                   | 							
         Tx_clock  |>rdclk rdusedw[9:0]| EPCS_Rx_used	    
					    ---------------------								
					    |                    |
			  IF_rst  |aclr                |								
					    ---------------------						
*/

wire [7:0]EPCS_data;
wire [9:0]EPCS_Rx_used;
wire  EPCS_rdreq;

EPCS_fifo EPCS_fifo_inst(.wrclk (PHY_data_clock),.rdreq (EPCS_rdreq),.rdclk (Tx_clock),.wrreq(EPCS_FIFO_enable), 
                .data (Rx_fifo_data),.q (EPCS_data), .rdusedw(EPCS_Rx_used), .aclr(IF_rst));

//----------------------------
// 			ASMI Interface
//----------------------------
wire busy;
wire erase_done;
wire send_more;
wire erase_done_ACK;
wire send_more_ACK;

ASMI_interface  ASMI_int_inst(.clock(Tx_clock), .busy(busy), .erase(erase), .erase_ACK(erase_ACK), .IF_PHY_data(EPCS_data),
							 .IF_Rx_used(EPCS_Rx_used), .rdreq(EPCS_rdreq), .erase_done(erase_done), .num_blocks(num_blocks),
							 .erase_done_ACK(erase_done_ACK), .send_more(send_more), .send_more_ACK(send_more_ACK), .NCONFIG(NCONFIG)); 

//---------------------------------------------------------
//		Send L/R audio to TLV320 in I2S format
//---------------------------------------------------------
             
I2S_xmit #(.DATA_BITS(32))  // CLRCLK running at 48KHz
  LR (.rst(C122_rst), .lrclk(CLRCLK), .clk(C122_clk), .CBrise(C122_cbrise),
      .CBfall(C122_cbfall), .sample(C122_LR_data), .outbit(CDIN));

      
//----------------------------------------------------------------------------
//		Get mic data from  TLV320 in I2S format and transfer to IF_clk domain
//---------------------------------------------------------------------------- 

wire [31:0] C122_mic_LR;
wire        C122_mic_rdy;
reg  [15:0] C122_mic_data;
      
// Get I2S CDOUT mic data from TLV320.  NOTE: only 16 bits used
I2S_rcv #(32,2,1) // WARNING: values 2,1 may need adjusting for best capture of data
    MIC (.xrst(C122_rst), .xclk(C122_clk), .BCLK(CBCLK), .LRCLK(CLRCLK), .din(CDOUT),.xData(C122_mic_LR),.xData_rdy(C122_mic_rdy));
    
always @(posedge C122_clk)
begin
  if (C122_mic_rdy) // this happens before LRfall
    C122_mic_data <= C122_mic_LR[31:16]; // we're only using the Left data
end
    
// transfer mic data into the IF_clk domain
cdc_sync #(16)
	cdc_mic (.siga(C122_mic_data), .rstb(IF_rst), .clkb(IF_clk), .sigb(IF_mic_Data)); 

//---------------------------------------------------------
//		De-ramdomizer
//--------------------------------------------------------- 

/*

 A Digital Output Randomizer is fitted to the LTC2208. This complements bits 15 to 1 if 
 bit 0 is 1. This helps to reduce any pickup by the A/D input of the digital outputs. 
 We need to de-ramdomize the LTC2208 data if this is turned on. 
 
*/

reg [15:0]temp_ADC;

always @ (posedge C122_clk) 
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

//------------------------------------------------------------------------------
//                 Transfer  Data from IF clock to 122.88MHz clock domain
//------------------------------------------------------------------------------

// cdc_sync is used to transfer from a slow to a fast clock domain

wire  [31:0] C122_LR_data;
wire  C122_DFS0, C122_DFS1;
wire  C122_rst;
wire  signed [15:0] C122_I_PWM;
wire  signed [15:0] C122_Q_PWM;

cdc_sync #(32)
	freq0 (.siga(IF_frequency[0]), .rstb(C122_rst), .clkb(C122_clk), .sigb(C122_frequency_HZ_Tx)); // transfer Tx frequency
	
cdc_sync #(32)
	freq1 (.siga(IF_frequency[1]), .rstb(C122_rst), .clkb(C122_clk), .sigb(C122_frequency_HZ[0])); // transfer Rx1 frequency

cdc_sync #(32)
	freq2 (.siga(IF_frequency[2]), .rstb(C122_rst), .clkb(C122_clk), .sigb(C122_frequency_HZ[1])); // transfer Rx2 frequency

cdc_sync #(32)
	LR_audio (.siga({IF_Left_Data,IF_Right_Data}), .rstb(C122_rst), .clkb(C122_clk), .sigb(C122_LR_data)); // transfer Left and Right audio

cdc_sync #(2)
	rate (.siga({IF_DFS1,IF_DFS0}), .rstb(C122_rst), .clkb(C122_clk), .sigb({C122_DFS1, C122_DFS0})); // sample rate
	
cdc_sync #(16)
    Tx_I  (.siga(IF_I_PWM), .rstb(C122_rst), .clkb(_122MHz), .sigb(C122_I_PWM )); // Tx I data
    
cdc_sync #(16)
    Tx_Q  (.siga(IF_Q_PWM), .rstb(C122_rst), .clkb(_122MHz), .sigb(C122_Q_PWM)); // Tx Q data
    
   

reg signed [15:0]C122_cic_i;
reg signed [15:0]C122_cic_q;
wire C122_ce_out_i;
wire C122_ce_out_q; 

// latch I&Q data on strobe from CIC filters
always @ (posedge _122MHz)
begin 
	if (C122_ce_out_i)
		C122_cic_i = C122_I_PWM;
	if (C122_ce_out_q)
		C122_cic_q = C122_Q_PWM;	
end 


//------------------------------------------------------------------------------
//                 Pulse generators
//------------------------------------------------------------------------------

wire IF_CLRCLK;

//  Create short pulse from posedge of CLRCLK synced to IF_clk for RXF read timing
//  First transfer CLRCLK into IF clock domain
cdc_sync cdc_CRLCLK (.siga(CLRCLK), .rstb(IF_rst), .clkb(IF_clk), .sigb(IF_CLRCLK)); 
//  Now generate the pulse
pulsegen cdc_m   (.sig(IF_CLRCLK), .rst(IF_rst), .clk(IF_clk), .pulse(IF_get_samples));


//---------------------------------------------------------
//		Convert frequency to phase word 
//---------------------------------------------------------

/*	
     Calculates  ratio = fo/fs = frequency/122.88Mhz where frequency is in MHz
	 Each calculation should take no more than 1 CBCLK

	 B scalar multiplication will be used to do the F/122.88Mhz function
	 where: F * C = R
	 0 <= F <= 65,000,000 hz
	 C = 1/122,880,000 hz
	 0 <= R < 1

	 This method will use a 32 bit by 32 bit multiply to obtain the answer as follows:
	 1. F will never be larger than 65,000,000 and it takes 26 bits to hold this value. This will
		be a B0 number since we dont need more resolution than 1 Hz - i.e. fractions of a hertz.
	 2. C is a constant.  Notice that the largest value we could multiply this constant by is B26
		and have a signed value less than 1.  Multiplying again by B31 would give us the biggest
		signed value we could hold in a 32 bit number.  Therefore we multiply by B57 (26+31).
		This gives a value of M2 = 1,172,812,403 (B57/122880000)
	 3. Now if we multiply the B0 number by the B57 number (M2) we get a result that is a B57 number.
		This is the result of the desire single 32 bit by 32 bit multiply.  Now if we want a scaled
		32 bit signed number that has a range -1 <= R < 1, then we want a B31 number.  Thus we shift
		the 64 bit result right 32 bits (B57 -> B31) or merely select the appropriate bits of the
		64 bit result. Sweet!  However since R is always >= 0 we will use an unsigned B32 result
*/

//------------------------------------------------------------------------------
//                 All DSP code is in the Receiver module
//------------------------------------------------------------------------------

localparam NR = 2; // number of receivers to implement

reg       [31:0] C122_frequency_HZ [0:NR-1];   // frequency control bits for CORDIC
reg       [31:0] C122_frequency_HZ_Tx;
reg       [31:0] C122_last_freq [0:NR-1];
reg       [31:0] C122_last_freq_Tx;
reg       [31:0] C122_sync_phase_word [0:NR-1];
reg       [31:0] C122_sync_phase_word_Tx;
wire      [63:0] C122_ratio [0:NR-1];
wire      [63:0] C122_ratio_Tx;
wire      [23:0] rx_I [0:NR-1];
wire      [23:0] rx_Q [0:NR-1];
wire             strobe [0:NR-1];
wire  IF_IQ_Data_rdy;
wire [47:0] IF_IQ_Data;

localparam M2 = 32'd1172812403;  // B57 = 2^57.   M2 = B57/122880000

generate
  genvar c;
  for (c = 0; c < NR; c = c + 1) // calc freq phase word for 2 freqs (Rx1, Rx2)
   begin: MDC 
    assign C122_ratio[c] = C122_frequency_HZ[c] * M2; // B0 * B57 number = B57 number

    always @ (posedge C122_clk)
    begin
      if (C122_cbrise) // time between C122_cbrise is enough for ratio calculation to settle
      begin
        C122_last_freq[c] <= C122_frequency_HZ[c];
        if (C122_last_freq[c] != C122_frequency_HZ[c]) // frequency changed)
          C122_sync_phase_word[c] <= C122_ratio[c][56:25]; // B57 -> B32 number since R is always >= 0  
      end 		
    end	 
  end
endgenerate

// calc frequency phase word for Tx
assign C122_ratio_Tx = C122_frequency_HZ_Tx * M2;

always @ (posedge C122_clk)
begin
  if (C122_cbrise)
  begin
    C122_last_freq_Tx <= C122_frequency_HZ_Tx;
	 if (C122_last_freq_Tx != C122_frequency_HZ_Tx)
	  C122_sync_phase_word_Tx <= C122_ratio_Tx[56:25];
  end
end


// Transfer the receiver data and strobe from C122_clk to IF_clk
cdc_mcp #(48)
	IQ_sync0 (.a_data ({rx_I[0], rx_Q[0]}), .a_clk(C122_clk),.b_clk(IF_clk), .a_data_rdy(strobe[0]),
			 .a_rst(C122_rst), .b_rst(IF_rst), .b_data(IF_M_IQ_Data[0]), .b_data_ack(IF_M_IQ_Data_rdy[0]));

cdc_mcp #(48)
	IQ_sync1 (.a_data ({rx_I[1], rx_Q[1]}), .a_clk(C122_clk),.b_clk(IF_clk), .a_data_rdy(strobe[1]),
			 .a_rst(C122_rst), .b_rst(IF_rst), .b_data(IF_M_IQ_Data[1]), .b_data_ack(IF_M_IQ_Data_rdy[1]));

receiver receiver_inst0(
  //control
  .clock(C122_clk),
  .rate({C122_DFS1, C122_DFS0}), //00=48, 01=96, 10=192 kHz
  .frequency(C122_sync_phase_word[0]),
  .out_strobe(strobe[0]),
  //input
  .in_data(temp_ADC),
  //output
  .out_data_I(rx_I[0]),
  .out_data_Q(rx_Q[0])
  );
  
receiver receiver_inst1(
  //control
  .clock(C122_clk),
  .rate({C122_DFS1, C122_DFS0}), //00=48, 01=96, 10=192 kHz
  .frequency(C122_sync_phase_word[1]),
  .out_strobe(strobe[1]),
  //input
  .in_data(temp_ADC),
  //output
  .out_data_I(rx_I[1]),
  .out_data_Q(rx_Q[1])
  ); 			 

//---------------------------------------------------------
//    ADC SPI interface 
//---------------------------------------------------------

wire [11:0] AIN1;
wire [11:0] AIN2;
wire [11:0] AIN3;
wire [11:0] AIN4;
wire [11:0] AIN5;  // holds 12 bit ADC value of Forward Power detector.
wire [11:0] AIN6;  // holds 12 bit ADC of 13.8v measurement 

Hermes_ADC ADC_SPI(.clock(CBCLK), .SCLK(ADCCLK), .nCS(nADCCS), .MISO(ADCMISO), .MOSI(ADCMOSI),
				   .AIN1(AIN1), .AIN2(AIN2), .AIN3(AIN3), .AIN4(AIN4), .AIN5(AIN5), .AIN6(AIN6));	
				   
				   
//---------------------------------------------------------
//	Apollo interface
//	Whenever frequency changes, send new value to Apollo.
//	Whenever Filter, Tuner, or PTT status changes, send enable or disable message to Apollo.
//	If PTT high continues beyond timeout, send additional PTT enable messages, so Apollo doesn't turn off the PA
//---------------------------------------------------------

localparam timeout = 16'd1000;	// default timeout (ms) for Apollo PA bias
// Maybe this value will eventually be sent from the PC in the C&C bytes.

wire Alex_SPI_SDO;
wire Alex_SPI_SCK;
wire SPI_TX_LOAD;
wire SPI_RX_LOAD;

wire Apollo_SPI_SDO;
wire Apollo_SPI_SCK;
wire ApolloReset;
wire ApolloEnable;
wire ApolloStatus;

wire FilterSelect;

assign FilterSelect = IF_Apollo;  // 0 = Alex, 1 = Apollo.

assign SPI_SDO = FilterSelect ? Apollo_SPI_SDO : Alex_SPI_SDO;		// select which module has control of data
assign SPI_SCK = FilterSelect ? Apollo_SPI_SCK : Alex_SPI_SCK;		// and clock for serial data transfer
assign J15_5   = FilterSelect ? ApolloReset : SPI_RX_LOAD;			// Alex Rx_load or Apollo Reset
assign J15_6   = FilterSelect ? ApolloEnable : SPI_TX_LOAD;      // Alex Tx_load or Apollo Enable 


reg IF_Filter;
reg IF_Tuner;
reg IF_autoTune;

Apollo Apollo_inst(
	.reset(IF_rst),
	.clock(Apollo_clk),
	.frequency(IF_frequency[0]),	
	.timeout(timeout),
	.PTT(FPGA_PTT),
	.Filter(1'b1),  // Filter(IF_Filter),
	.Tuner(IF_Tuner),
	.ANT_TUNE(IF_autoTune),
	.SPI_SDI(SPI_SDI),							// serial data from Apollo
	.SPI_SDO(Apollo_SPI_SDO),					// serial data to Apollo
	.SPI_SCK(Apollo_SPI_SCK),					// clock for Apollo serial data transfer
	.ApolloStatusLine(ApolloStatus),			// Apollo sets this low when it wants to send data
	.ApolloReset(ApolloReset),
	.ApolloEnable(ApolloEnable),
	.statusAvailable(ApolloStatusAvailable),	// set true when ApolloStatusBytes have been updated
	.status(ApolloStatusBytes),					// Status bytes from Apollo.  currently unused.  Some day send some to PC via C&C bytes.
	.FilterSelect(FilterSelect)
	);
	
				   
//---------------------------------------------------------
//                 Transmitter code 
//---------------------------------------------------------	

/* 
	The gain distribution of the transmitter code is as follows.
	Since the CIC interpolating filters do not interpolate by 2^n they have an overall loss.
	
	The overall gain in the interpolating filter is ((RM)^N)/R.  So in this case its 2560^4.
	This is normalised by dividing by ceil(log2(2560^4)).
	
	In which case the normalized gain would be (2560^4)/(2^46) = .6103515625
	
	The CORDIC has an overall gain of 1.647.
	
	Since the CORDIC takes 16 bit I & Q inputs but output needs to be truncated to 14 bits, in order to
	interface to the DAC, the gain is reduced by 1/4 to 0.41175
	
	We need to be able to drive to DAC to its full range in order to maximise the S/N ratio and 
	minimise the amount of PA gain.  We can increase the output of the CORDIC by multiplying it by 4.
	This is simply achieved by setting the CORDIC output width to 16 bits and assigning bits [13:0] to the DAC.
	
	The gain distripution is now:
	
	0.61 * 0.41174 * 4 = 1.00467 
	
	This means that the DAC output will wrap if a full range 16 bit I/Q signal is received. 
	This can be prevented by reducing the output of the CIC filter.
	
	If we subtract 1/128 of the CIC output from itself the level becomes
	
	1 - 1/128 = 0.9921875
	
	Hence the overall gain is now 
	
	0.61 * 0.9921875 * 0.41174 * 4 = 0.996798
	

*/				   

//---------------------------------------------------------
//  Interpolating CIC filter  R = 2560  N = 5
//---------------------------------------------------------

wire signed [15:0] C122_cic_out_i;
wire signed [15:0] C122_cic_out_q;

cicint cic_I(.clk(_122MHz), .clk_enable(1'b1), .reset(C122_rst), .filter_in(C122_cic_i),
             .filter_out(C122_cic_out_i), .ce_out(C122_ce_out_i));
             
cicint cic_Q(.clk(_122MHz), .clk_enable(1'b1), .reset(C122_rst), .filter_in(C122_cic_q),
             .filter_out(C122_cic_out_q), .ce_out(C122_ce_out_q));
             

// multiply CIC outputs by 0.9921875, >>> is the Veriloig arrithmetic shift right
    
wire signed [15:0] C122_out_i;
wire signed [15:0] C122_out_q;   

assign C122_out_i = C122_cic_out_i - (C122_cic_out_i >>> 7);
assign C122_out_q = C122_cic_out_q - (C122_cic_out_q >>> 7);

//---------------------------------------------------------
//    CORDIC NCO 
//---------------------------------------------------------

// Code rotates input at set frequency and produces I & Q 

wire signed [14:0] C122_cordic_i_out;
wire signed [31:0] C122_phase_word_Tx;

wire signed [15:0] I;
wire signed [15:0] Q;

// if in VNA mode use the Rx[0] phase word for the Tx
assign C122_phase_word_Tx = VNA ? C122_sync_phase_word[0] : C122_sync_phase_word_Tx;
assign                  I = VNA ? 16'd19274 : C122_out_q;   	// select VNA mode if active. Set CORDIC for max DAC output
assign                  Q = VNA ? 0 : C122_out_i; 					// taking into account CORDICs gain i.e. 0x7FFF/1.7


// NOTE:  I and Q inputs reversed to give correct sideband out 

cpl_cordic #(.OUT_WIDTH(16))
 		cordic_inst (.clock(_122MHz), .frequency(C122_phase_word_Tx), .in_data_I(I),			
		.in_data_Q(Q), .out_data_I(C122_cordic_i_out), .out_data_Q());		
			 	 
/* 
  We can use either the I or Q output from the CORDIC directly to drive the DAC.

    exp(jw) = cos(w) + j sin(w)

  When multplying two complex sinusoids f1 and f2, you get only f1 + f2, no
  difference frequency.

      Z = exp(j*f1) * exp(j*f2) = exp(j*(f1+f2))
        = cos(f1 + f2) + j sin(f1 + f2)
*/

// the CORDIC output is stable on the negative edge of the clock

always @ (negedge _122MHz)
	DACD = C122_cordic_i_out[13:0];   //gain of 4


//------------------------------------------------------------
//  Set Power Output 
//------------------------------------------------------------

// PWM DAC to set drive current to DAC. PWM_count increments 
// using IF_clk. If the count is less than the drive 
// level set by the PC then DAC_ALC will be high, otherwise low.  

reg [7:0] PWM_count;
always @ (posedge _122MHz)
begin 
	PWM_count <= PWM_count + 1'b1;
	if (IF_Drive_Level >= PWM_count)
		DAC_ALC <= 1'b1;
	else 
		DAC_ALC <= 1'b0;
end 


//---------------------------------------------------------
//  Receive DOUT and CDOUT data to put in TX FIFO
//---------------------------------------------------------

wire   [15:0] IF_P_mic_Data;
wire          IF_P_mic_Data_rdy;
wire   [47:0] IF_M_IQ_Data [0:NR-1];
wire [NR-1:0] IF_M_IQ_Data_rdy;
wire   [63:0] IF_tx_IQ_mic_data;
reg           IF_tx_IQ_mic_rdy;
wire   [15:0] IF_mic_Data;
wire    [2:0] IF_chan;
wire    [2:0] IF_last_chan;
wire     [47:0] IF_chan_test;

always @*
begin
  if (IF_rst)
    IF_tx_IQ_mic_rdy = 1'b0;
  else 
      IF_tx_IQ_mic_rdy = IF_M_IQ_Data_rdy[0]; 	// this the strobe signal from the ADC now in IF clock domain
end

assign IF_IQ_Data = IF_M_IQ_Data[IF_chan];

// concatenate the IQ and Mic data to form a 64 bit data word
assign IF_tx_IQ_mic_data = {IF_IQ_Data, IF_mic_Data};  

//----------------------------------------------------------------------------
//     Tx_fifo Control - creates IF_tx_fifo_wdata and IF_tx_fifo_wreq signals
//----------------------------------------------------------------------------

localparam RFSZ = clogb2(RX_FIFO_SZ-1);  // number of bits needed to hold 0 - (RX_FIFO_SZ-1)
localparam TFSZ = clogb2(TX_FIFO_SZ-1);  // number of bits needed to hold 0 - (TX_FIFO_SZ-1)
localparam SFSZ = clogb2(SP_FIFO_SZ-1);  // number of bits needed to hold 0 - (SP_FIFO_SZ-1)

wire     [15:0] IF_tx_fifo_wdata;   		// LTC2208 ADC uses this to send its data to Tx FIFO
wire            IF_tx_fifo_wreq;    		// set when we want to send data to the Tx FIFO
wire            IF_tx_fifo_full;
wire [TFSZ-1:0] IF_tx_fifo_used;
wire            IF_tx_fifo_rreq;
wire            IF_tx_fifo_empty;

wire [RFSZ-1:0] IF_Rx_fifo_used;    		// read side count
wire            IF_Rx_fifo_full;

wire            clean_dash;      			// debounced dash key
wire            clean_dot;       			// debounced dot key
wire            clean_PTT_in;    			// debounced PTT button
wire     [11:0] Penny_ALC;

wire   [RFSZ:0] RX_USED;
wire            IF_tx_fifo_clr;

assign RX_USED = {IF_Rx_fifo_full,IF_Rx_fifo_used};


assign Penny_ALC = AIN5; 

Hermes_Tx_fifo_ctrl #(RX_FIFO_SZ, TX_FIFO_SZ) TXFC 
           (IF_rst, IF_clk, IF_tx_fifo_wdata, IF_tx_fifo_wreq, IF_tx_fifo_full,
            IF_tx_fifo_used, IF_tx_fifo_clr, IF_tx_IQ_mic_rdy,
            IF_tx_IQ_mic_data, IF_chan, IF_last_chan, clean_dash, clean_dot, clean_PTT_in, OVERFLOW,
            Penny_serialno, Merc_serialno, Hermes_serialno, Penny_ALC, AIN1, AIN2,
            AIN3, AIN4, AIN6, IO4, IO5, IO6, IO8);

//------------------------------------------------------------------------
//   Tx_fifo  (1024 words) Dual clock FIFO - Altera Megafunction (dcfifo)
//------------------------------------------------------------------------

/*
        Data from the Tx FIFO Controller  is written to the FIFO using IF_tx_fifo_wreq. 
        FIFO is 1024 WORDS long.
        NB: The output flags are only valid after a read/write clock has taken place
        
        
							--------------------
	IF_tx_fifo_wdata 	|data[15:0]		 wrful| IF_tx_fifo_full
						   |				         |
	IF_tx_fifo_wreq	|wreq		     wrempty| IF_tx_fifo_empty
						   |				   	   |
		IF_clk			|>wrclk	 wrused[9:0]| IF_tx_fifo_used
						   ---------------------
    Tx_fifo_rdreq		|rdreq		   q[7:0]| PHY_Tx_data
						   |					      |
	   Tx_clock_2		|>rdclk		  rdempty| 
						   |		  rdusedw[10:0]| PHY_Tx_rdused  (0 to 2047 bytes)
						   ---------------------
						   |                    |
 IF_tx_fifo_clr OR  	|aclr                |
	IF_rst				---------------------
				
        

*/

Tx_fifo Tx_fifo_inst(.wrclk (IF_clk),.rdreq (Tx_fifo_rdreq),.rdclk (Tx_clock_2),.wrreq (IF_tx_fifo_wreq), 
                .data ({IF_tx_fifo_wdata[7:0], IF_tx_fifo_wdata[15:8]}),.q (PHY_Tx_data),.wrusedw(IF_tx_fifo_used), .wrfull(IF_tx_fifo_full),
                .rdempty(),.rdusedw(PHY_Tx_rdused),.wrempty(IF_tx_fifo_empty),.aclr(IF_rst || IF_tx_fifo_clr ));

wire [7:0] PHY_Tx_data;
reg [3:0]sync_TD;
wire PHY_Tx_rdempty;             
             


//---------------------------------------------------------
//   Rx_fifo  (2048 words) single clock FIFO
//---------------------------------------------------------

wire [15:0] IF_Rx_fifo_rdata;
reg         IF_Rx_fifo_rreq;    // controls reading of fifo
wire [15:0] IF_PHY_data;

wire [15:0] IF_Rx_fifo_wdata;
reg         IF_Rx_fifo_wreq;

FIFO #(RX_FIFO_SZ) RXF (.rst(IF_rst), .clk (IF_clk), .full(IF_Rx_fifo_full), .usedw(IF_Rx_fifo_used), 
          .wrreq (IF_Rx_fifo_wreq), .data (IF_PHY_data), 
          .rdreq (IF_Rx_fifo_rreq), .q (IF_Rx_fifo_rdata) );


//------------------------------------------------------------
//   Sync and  C&C  Detector
//------------------------------------------------------------

/*

  Read the value of IF_PHY_data whenever IF_PHY_drdy is set.
  Look for sync and if found decode the C&C data.
  Then send subsequent data to Rx FIF0 until end of frame.
	
*/

reg   [2:0] IF_SYNC_state;
reg   [2:0] IF_SYNC_state_next;
reg   [7:0] IF_SYNC_frame_cnt; 	// 256-4 words = 252 words
reg   [7:0] IF_Rx_ctrl_0;   		// control C0 from PC
reg   [7:0] IF_Rx_ctrl_1;   		// control C1 from PC
reg   [7:0] IF_Rx_ctrl_2;   		// control C2 from PC
reg   [7:0] IF_Rx_ctrl_3;   		// control C3 from PC
reg   [7:0] IF_Rx_ctrl_4;   		// control C4 from PC
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
    IF_Rx_save <= #IF_TPD IF_PHY_drdy && (IF_SYNC_state == SYNC_RX_3_4);

  if (IF_PHY_drdy && (IF_SYNC_state == SYNC_START) && (IF_PHY_data[15:8] == 8'h7F))
    IF_Rx_ctrl_0  <= #IF_TPD IF_PHY_data[7:0];

  if (IF_PHY_drdy && (IF_SYNC_state == SYNC_RX_1_2))
  begin
    IF_Rx_ctrl_1  <= #IF_TPD IF_PHY_data[15:8];
    IF_Rx_ctrl_2  <= #IF_TPD IF_PHY_data[7:0];
  end

  if (IF_PHY_drdy && (IF_SYNC_state == SYNC_RX_3_4))
  begin
    IF_Rx_ctrl_3  <= #IF_TPD IF_PHY_data[15:8];
    IF_Rx_ctrl_4  <= #IF_TPD IF_PHY_data[7:0];
  end

  if (IF_SYNC_state == SYNC_START)
    IF_SYNC_frame_cnt <= 0;					    					// reset sync counter
  else if (IF_PHY_drdy && (IF_SYNC_state == SYNC_FINISH))
    IF_SYNC_frame_cnt <= IF_SYNC_frame_cnt + 1'b1;		    // increment if we have data to store
end

always @*
begin
  case (IF_SYNC_state)
    // state SYNC_IDLE  - loop until we find start of sync sequence
    SYNC_IDLE:
    begin
      IF_Rx_fifo_wreq  = 1'b0;             // Note: Sync bytes not saved in Rx_fifo

      if (IF_rst || !IF_PHY_drdy) 
        IF_SYNC_state_next = SYNC_IDLE;    // wait till we get data from PC
      else if (IF_PHY_data == 16'h7F7F)
        IF_SYNC_state_next = SYNC_START;   // possible start of sync
      else
        IF_SYNC_state_next = SYNC_IDLE;
    end	

    // check for 0x7F  sync character & get Rx control_0 
    SYNC_START:
    begin
      IF_Rx_fifo_wreq  = 1'b0;             // Note: Sync bytes not saved in Rx_fifo

      if (!IF_PHY_drdy)              
        IF_SYNC_state_next = SYNC_START;   // wait till we get data from PC
      else if (IF_PHY_data[15:8] == 8'h7F)
        IF_SYNC_state_next = SYNC_RX_1_2;  // have sync so continue
      else
        IF_SYNC_state_next = SYNC_IDLE;    // start searching for sync sequence again
    end

    
    SYNC_RX_1_2:                        	 // save Rx control 1 & 2
    begin
      IF_Rx_fifo_wreq  = 1'b0;             // Note: Rx control 1 & 2 not saved in Rx_fifo

      if (!IF_PHY_drdy)              
        IF_SYNC_state_next = SYNC_RX_1_2;  // wait till we get data from PC
      else
        IF_SYNC_state_next = SYNC_RX_3_4;
    end

    SYNC_RX_3_4:                        	 // save Rx control 3 & 4
    begin
      IF_Rx_fifo_wreq  = 1'b0;             // Note: Rx control 3 & 4 not saved in Rx_fifo

      if (!IF_PHY_drdy)              
        IF_SYNC_state_next = SYNC_RX_3_4;  // wait till we get data from PC
      else
        IF_SYNC_state_next = SYNC_FINISH;
    end

    // Remainder of data goes to Rx_fifo, re-start looking
    // for a new SYNC at end of this frame. 
    // Note: due to the use of IF_PHY_drdy data will only be written to the 
    // Rx fifo if there is room. Also the frame_count will only be incremented if IF_PHY_drdy is true.
    SYNC_FINISH:
    begin    
	  IF_Rx_fifo_wreq  = IF_PHY_drdy;
	  if (IF_SYNC_frame_cnt == ((512-8)/2)) begin  // frame ended, go get sync again
		IF_SYNC_state_next = SYNC_IDLE;
	  end 
	  else IF_SYNC_state_next = SYNC_FINISH;
    end

    default:
    begin
      IF_Rx_fifo_wreq  = 1'b0;
      IF_SYNC_state_next = SYNC_IDLE;
    end
	endcase
end

wire have_room;
assign have_room = (IF_Rx_fifo_used < RX_FIFO_SZ - ((512-8)/2)) ? 1'b1 : 1'b0;  // the /2 is because we send 16 bit values

// prevent read from PHY fifo if empty and writing to Rx fifo if not enough room 
assign  IF_PHY_drdy = have_room & ~IF_PHY_rdempty;

//---------------------------------------------------------
//              Decode Command & Control data
//---------------------------------------------------------

/*
	Decode IF_Rx_ctrl_0....IF_Rx_ctrl_4.

	Decode frequency (both Tx and Rx if full duplex selected), PTT, Speed etc

	The current frequency is set by the PC by decoding 
	IF_Rx_ctrl_1... IF_Rx_ctrl_4 when IF_Rx_ctrl_0[7:1] = 7'b0000_001
		
      The Rx Sampling Rate, either 192k, 96k or 48k is set by
      the PC by decoding IF_Rx_ctrl_1 when IF_Rx_ctrl_0[7:1] are all zero. IF_Rx_ctrl_1
      decodes as follows:

      IF_Rx_ctrl_1 = 8'bxxxx_xx00  - 48kHz
      IF_Rx_ctrl_1 = 8'bxxxx_xx01  - 96kHz
      IF_Rx_ctrl_1 = 8'bxxxx_xx10  - 192kHz

	Decode PTT from PC. Held in IF_Rx_ctrl_0[0] as follows
	
	0 = PTT inactive
	1 = PTT active
	
	Decode Attenuator settings on Alex, when IF_Rx_ctrl_0[7:1] = 0, IF_Rx_ctrl_3[1:0] indicates the following 
	
	00 = 0dB
	01 = 10dB
	10 = 20dB
	11 = 30dB
	
	Decode ADC & Attenuator settings on Hermes, when IF_Rx_ctrl_0[7:1] = 0, IF_Rx_ctrl_3[4:2] indicates the following
	
	000 = Random, Dither, Preamp OFF
	1xx = Random ON
	x1x = Dither ON
	xx1 = Preamp ON **** replace with attenuator
	
	Decode Rx relay settings on Alex, when IF_Rx_ctrl_0[7:1] = 0, IF_Rx_ctrl_3[6:5] indicates the following
	
	00 = None
	01 = Rx 1
	10 = Rx 2
	11 = Transverter
	
	Decode Tx relay settigs on Alex, when IF_Rx_ctrl_0[7:1] = 0, IF_Rx_ctrl_4[1:0] indicates the following
	
	00 = Tx 1
	01 = Tx 2
	10 = Tx 3
	
	Decode Rx_1_out relay settigs on Alex, when IF_Rx_ctrl_0[7:1] = 0, IF_Rx_ctrl_3[7] indicates the following

	1 = Rx_1_out on 

	When IF_Rx_ctrl_0[7:1] == 7'b0001_010 decodes as follows:
	
	IF_Line_In_Gain		<= IF_Rx_ctrl2[4:0]	// decode 5-bit line gain setting
	
*/

reg   [6:0] IF_OC;       			// open collectors on Hermes
reg         IF_mode;     			// normal or Class E PA operation 
reg         IF_RAND;     			// when set randomizer in ADCon
reg         IF_DITHER;   			// when set dither in ADC on
reg   [1:0] IF_ATTEN;    			// decode attenuator setting on Alex
reg         Preamp;					// selects input attenuator setting, 1 = 20dB, 0 = 0dB
reg   [1:0] IF_TX_relay; 			// Tx relay setting on Alex
reg         IF_Rout;     			// Rx1 out on Alex
reg   [1:0] IF_RX_relay; 			// Rx relay setting on Alex 
reg  [31:0] IF_frequency[0:2]; 	// Tx, Rx1, Rx2
reg         IF_duplex;
reg         IF_DFS1;
reg			IF_DFS0;
reg   [7:0] IF_Drive_Level; 		// Tx drive level
reg         IF_Mic_boost;			// Mic boost 0 = 0dB, 1 = 20dB
reg         IF_Line_In;				// Selects input, mic = 0, line = 1
reg   [4:0] IF_Line_In_Gain;		// Sets Line-In Gain value (00000=-32.4 dB to 11111=+12 dB in 1.5 dB steps)
reg         IF_Apollo;				// Selects Alex (0) or Apollo (1)
reg 			VNA;						// Selects VNA mode when set. 
reg		   Alex_manual; 	  		// set if manual selection of Alex relays active
reg         Alex_6m_preamp; 		// set if manual selection and 6m preamp selected
reg   [6:0] Alex_manual_LPF;		// Alex LPF relay selection in manual mode
reg   [5:0] Alex_manual_HPF;		// Alex HPF relay selection in manual mode

always @ (posedge IF_clk)
begin 
  if (IF_rst)
  begin // set up default values - 0 for now
    // RX_CONTROL_1
    {IF_DFS1, IF_DFS0} <= 2'b00;   	// decode speed 
    // RX_CONTROL_2
    IF_mode            <= 1'b0;    	// decode mode, normal or Class E PA
    IF_OC              <= 7'b0;    	// decode open collectors on Hermes
    // RX_CONTROL_3
    IF_ATTEN           <= 2'b0;    	// decode Alex attenuator setting 
    Preamp             <= 1'b0;    	// decode Preamp (Attenuator), default on
    IF_DITHER          <= 1'b0;    	// decode dither on or off
    IF_RAND            <= 1'b0;    	// decode randomizer on or off
    IF_RX_relay        <= 2'b0;    	// decode Alex Rx relays
    IF_Rout            <= 1'b0;    	// decode Alex Rx_1_out relay
    // RX_CONTROL_4
    IF_TX_relay        <= 2'b0;    	// decode Alex Tx Relays
    IF_duplex          <= 1'b0;    	// not in duplex mode
	 IF_last_chan       <= 3'b000;  	// default single receiver
    IF_Mic_boost       <= 1'b0;    	// mic boost off 
    IF_Drive_Level     <= 8'b0;	   // drive at minimum
	 IF_Line_In			  <= 1'b0;		// select Mic input, not Line in
	 IF_Filter			  <= 1'b0;		// Apollo filter disabled (bypassed)
	 IF_Tuner			  <= 1'b0;		// Apollo tuner disabled (bypassed)
	 IF_autoTune	     <= 1'b0;		// Apollo auto-tune disabled
	 IF_Apollo			  <= 1'b0;     //	Alex selected		
	 VNA					  <= 1'b0;		// VNA disabled
	 Alex_manual		  <= 1'b0; 	  	// default manual Alex filter selection (0 = auto selection, 1 = manual selection)
	 Alex_manual_HPF	  <= 6'b0;		// default manual settings, no Alex HPF filters selected
	 Alex_6m_preamp	  <= 1'b0;		// default not set
	 Alex_manual_LPF	  <= 7'b0;		// default manual settings, no Alex LPF filters selected
	 IF_Line_In_Gain	  <= 5'b0;		// default line-in gain at min
	
  end
  else if (IF_Rx_save) 					// all Rx_control bytes are ready to be saved
  begin 										// Need to ensure that C&C data is stable 
    if (IF_Rx_ctrl_0[7:1] == 7'b0000_000)
    begin
      // RX_CONTROL_1
      {IF_DFS1, IF_DFS0}  <= IF_Rx_ctrl_1[1:0]; // decode speed 
      // RX_CONTROL_2
      IF_mode             <= IF_Rx_ctrl_2[0];   // decode mode, normal or Class E PA
      IF_OC               <= IF_Rx_ctrl_2[7:1]; // decode open collectors on Penelope
      // RX_CONTROL_3
      IF_ATTEN            <= IF_Rx_ctrl_3[1:0]; // decode Alex attenuator setting 
      Preamp              <= ~IF_Rx_ctrl_3[2];  // decode Preamp (Attenuator)  1 = On, 0 = Off
      IF_DITHER           <= IF_Rx_ctrl_3[3];   // decode dither on or off
      IF_RAND             <= IF_Rx_ctrl_3[4];   // decode randomizer on or off
      IF_RX_relay         <= IF_Rx_ctrl_3[6:5]; // decode Alex Rx relays
      IF_Rout             <= IF_Rx_ctrl_3[7];   // decode Alex Rx_1_out relay
      // RX_CONTROL_4
      IF_TX_relay         <= IF_Rx_ctrl_4[1:0]; // decode Alex Tx Relays
      IF_duplex           <= IF_Rx_ctrl_4[2];   // save duplex mode
      IF_last_chan	     <= IF_Rx_ctrl_4[5:3]; // number of IQ streams to send to PC
    end
    if (IF_Rx_ctrl_0[7:1] == 7'b0001_001)
    begin
	  IF_Drive_Level	  <= IF_Rx_ctrl_1;	    	// decode drive level 
	  IF_Mic_boost		  <= IF_Rx_ctrl_2[0];   	// decode mic boost 0 = 0dB, 1 = 20dB  
	  IF_Line_In		  <= IF_Rx_ctrl_2[1];		// 0 = Mic input, 1 = Line In
	  IF_Filter			  <= IF_Rx_ctrl_2[2];		// 1 = enable Apollo filter
	  IF_Tuner			  <= IF_Rx_ctrl_2[3];		// 1 = enable Apollo tuner
	  IF_autoTune		  <= IF_Rx_ctrl_2[4];		// 1 = begin Apollo auto-tune
	  IF_Apollo         <= IF_Rx_ctrl_2[5];      // 1 = Apollo enabled, 0 = Alex enabled 
	  Alex_manual		  <= IF_Rx_ctrl_2[6]; 	  	// manual Alex HPF/LPF filter selection (0 = disable, 1 = enable)
	  VNA					  <= IF_Rx_ctrl_2[7];		// 1 = enable VNA mode
	  Alex_manual_HPF	  <= IF_Rx_ctrl_3[5:0];		// Alex HPF filters select
	  Alex_6m_preamp	  <= IF_Rx_ctrl_3[6];		// 6M low noise amplifier (0 = disable, 1 = enable)
	  Alex_manual_LPF	  <= IF_Rx_ctrl_4[6:0];		// Alex LPF filters select	  
	end
	if (IF_Rx_ctrl_0[7:1] == 7'b0001_010)
	begin
	  IF_Line_In_Gain   <= IF_Rx_ctrl_2[4:0];		// decode line-in gain setting
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
  end
  else if (IF_Rx_save)
  begin
      if (IF_Rx_ctrl_0[7:1] == 7'b0000_001)   // decode IF_frequency[0]
      begin
		  IF_frequency[0]   <= {IF_Rx_ctrl_1, IF_Rx_ctrl_2, IF_Rx_ctrl_3, IF_Rx_ctrl_4}; // Tx frequency
			if (!IF_duplex && (IF_last_chan == 3'b000))
				IF_frequency[1] <= IF_frequency[0]; //				  
		end
		if (IF_Rx_ctrl_0[7:1] == 7'b0000_010) // decode Rx1 frequency
      begin
			if (!IF_duplex && (IF_last_chan == 3'b000)) // Rx1 frequency
				IF_frequency[1] <= IF_frequency[0];				  
         else
				IF_frequency[1] <= {IF_Rx_ctrl_1, IF_Rx_ctrl_2, IF_Rx_ctrl_3, IF_Rx_ctrl_4}; 
		end
		if (IF_Rx_ctrl_0[7:1] == 7'b0000_011) // decode Rx2 frequency
		begin
       IF_frequency[2] <= {IF_Rx_ctrl_1, IF_Rx_ctrl_2, IF_Rx_ctrl_3, IF_Rx_ctrl_4};  // Rx2 frequency 
      end
  end
end

assign FPGA_PTT = IF_Rx_ctrl_0[0]; // IF_Rx_ctrl_0 only updated when we get correct sync sequence


//------------------------------------------------------------
//  Attenuator 
//------------------------------------------------------------

// for now set the attenuator to 0dB or 20dB

wire [4:0] atten_data;

assign atten_data = Preamp ? 5'b10100 : 5'b0;  // 10100 = 16dB + 4dB = 20dB

Attenuator Attenuator_inst (.clk(IF_clk), .data(atten_data), .ATTN_CLK(ATTN_CLK), .ATTN_DATA(ATTN_DATA), .ATTN_LE(ATTN_LE));



//////////////////////////////////////////////////////////////
//
//		Alex Filter selection
//
//////////////////////////////////////////////////////////////

wire  [6:0] C122_LPF;
wire  [6:0] C122_LPF_auto;
wire  [5:0] C122_select_HPF;
wire  [5:0] C122_select_HPF_auto;
reg   [31:0] C122_freq_max;
reg	[31:0] C122_freq_min;
reg   [31:0] C122_HPF_freq;
reg	[31:0] C122_LPF_freq;

always @ (posedge C122_clk) begin
	if (C122_cbrise) begin
		C122_freq_max <= C122_frequency_HZ[0];
		C122_freq_min <= C122_frequency_HZ[0];
		if (C122_frequency_HZ[1] > C122_freq_max) C122_freq_max <= C122_frequency_HZ[1];
		if (C122_frequency_HZ[1] < C122_freq_min && C122_frequency_HZ[1] > 0) C122_freq_min <= C122_frequency_HZ[1];
		C122_HPF_freq <= C122_freq_min;
		C122_LPF_freq <= FPGA_PTT ? C122_frequency_HZ_Tx : C122_freq_max;
	end
end

// if Alex_manual selected then use HPF & LPF setting provided by user
assign C122_LPF 		= Alex_manual ? Alex_manual_LPF : C122_LPF_auto;
assign C122_select_HPF  = Alex_manual ? Alex_manual_HPF : C122_select_HPF_auto;

LPF_select Alex_LPF_select(.clock(C122_clk), .frequency(C122_LPF_freq), .LPF(C122_LPF_auto));
HPF_select Alex_HPF_select(.clock(C122_clk), .frequency(C122_HPF_freq), .HPF(C122_select_HPF_auto));

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

wire C122_ANT1;			
wire C122_ANT2;
wire C122_ANT3;
wire C122_Rx_1_out;
wire C122_Transverter;
wire C122_Rx_2_in;
wire C122_Rx_1_in;

assign C122_Rx_1_out = IF_Rout;

assign C122_ANT1 = (IF_TX_relay == 2'b00) ? 1'b1 : 1'b0; 		// select Tx antenna 1
assign C122_ANT2 = (IF_TX_relay == 2'b01) ? 1'b1 : 1'b0; 		// select Tx antenna 2
assign C122_ANT3 = (IF_TX_relay == 2'b10) ? 1'b1 : 1'b0; 		// select Tx antenna 3

assign C122_Rx_1_in     = (IF_RX_relay == 2'b01) ? 1'b1 : 1'b0; // select Rx antenna 1
assign C122_Rx_2_in     = (IF_RX_relay == 2'b10) ? 1'b1 : 1'b0; // select Rx antenna 2
assign C122_Transverter = (IF_RX_relay == 2'b11) ? 1'b1 : 1'b0; // select Transverter input 


//////////////////////////////////////////////////////////////
//
//		Alex SPI interface
//
//////////////////////////////////////////////////////////////

localparam  TX_YELLOW_LED = 1'b1;
localparam  RX_YELLOW_LED = 1'b1;

wire        C122_6m_preamp;
wire        C122_Tx_red_led;
wire        C122_Rx_red_led;
wire        C122_TR_relay;
wire [15:0] C122_Alex_Tx_data;
wire [15:0] C122_Alex_Rx_data;

// assign attenuators
wire C122_10dB_atten = IF_ATTEN[0];
wire C122_20dB_atten = IF_ATTEN[1];

// define and concatenate the Tx data to send to Alex via SPI
assign C122_Tx_red_led = FPGA_PTT; // turn red led on when we Tx
assign C122_TR_relay   = FPGA_PTT; // turn on TR relay when PTT active

assign C122_Alex_Tx_data = {C122_LPF[6:4], C122_Tx_red_led, C122_TR_relay, C122_ANT3, C122_ANT2,
                       C122_ANT1, C122_LPF[3:0], TX_YELLOW_LED, 3'b000};

// define and concatenate the Rx data to send to Alex via SPI
assign C122_Rx_red_led = FPGA_PTT;	// turn red led on when we Rx

// turn 6m preamp on if frequency > 50MHz 
assign C122_6m_preamp = (C122_frequency_HZ[0] > 50000000) ? 1'b1 : 1'b0;

// if 6m preamp selected disconnect all filters 
wire [5:0] C122_HPF;
assign C122_HPF = C122_6m_preamp ? 6'd0 : C122_select_HPF; 

// V3 Alex hardware
assign C122_Alex_Rx_data = {C122_Rx_red_led, C122_10dB_atten, C122_20dB_atten, C122_HPF[5],
                       C122_Rx_1_out, C122_Rx_1_in, C122_Rx_2_in, C122_Transverter, 1'b0,
                       C122_HPF[4:2], C122_6m_preamp, C122_HPF[1:0], RX_YELLOW_LED};
					   
// concatenate Tx and Rx data and send to SPI interface. SPI interface only sends on a change of Alex_data.
// All data is sent in about 120uS.
wire [31:0] C122_Alex_data;
reg  [31:0] SPI_Alex_data;

assign C122_Alex_data = {C122_Alex_Tx_data[15:0], C122_Alex_Rx_data[15:0]};

// should move Alex data into SPI_clk domain, but since it changes slowly should be OK for now. 

SPI Alex_SPI_Tx (.Alex_data(C122_Alex_data), .SPI_data(Alex_SPI_SDO),
                 .SPI_clock(Alex_SPI_SCK), .Tx_load_strobe(SPI_TX_LOAD),
                 .Rx_load_strobe(SPI_RX_LOAD), .spi_clock(SPI_clk));	

//---------------------------------------------------------
//   State Machine to manage PWM interface
//---------------------------------------------------------
/*

    The code loops until there are at least 4 words in the Rx_FIFO.

    The first word is the Left audio followed by the Right audio
    which is followed by I data and finally the Q data.
    	
    The words sent to the D/A converters must be sent at the sample rate
    of the A/D converters (48kHz) so is synced to the negative edge of the CLRCLK (via IF_get_rx_data).
*/

reg   [2:0] IF_PWM_state;      // state for PWM
reg   [2:0] IF_PWM_state_next; // next state for PWM
reg  [15:0] IF_Left_Data;      // Left 16 bit PWM data for D/A converter
reg  [15:0] IF_Right_Data;     // Right 16 bit PWM data for D/A converter
reg  [15:0] IF_I_PWM;          // I 16 bit PWM data for D/A conveter
reg  [15:0] IF_Q_PWM;          // Q 16 bit PWM data for D/A conveter
wire        IF_get_samples;
wire        IF_get_rx_data;

assign IF_get_rx_data = IF_get_samples;

localparam PWM_IDLE     = 0,
           PWM_START    = 1,
           PWM_LEFT     = 2,
           PWM_RIGHT    = 3,
           PWM_I_AUDIO  = 4,
           PWM_Q_AUDIO  = 5;

always @ (posedge IF_clk) 
begin
  if (IF_rst)
    IF_PWM_state   <= #IF_TPD PWM_IDLE;
  else
    IF_PWM_state   <= #IF_TPD IF_PWM_state_next;

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

end

always @*
begin
  case (IF_PWM_state)
    PWM_IDLE:
    begin
      IF_Rx_fifo_rreq = 1'b0;

      if (!IF_get_rx_data  || RX_USED[RFSZ:2] == 1'b0 ) // RX_USED < 4
        IF_PWM_state_next = PWM_IDLE;    // wait until time to get the donuts every 48kHz from oven (RX_FIFO)
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
      IF_PWM_state_next  = PWM_IDLE; // truck has left the shipping dock
    end

    default:
    begin
      IF_Rx_fifo_rreq    = 1'b0;
      IF_PWM_state_next  = PWM_IDLE;
    end
  endcase
end

//---------------------------------------------------------
//  Debounce PTT input - active low
//---------------------------------------------------------

debounce de_PTT(.clean_pb(clean_PTT_in), .pb(~PTT), .clk(IF_clk));


//---------------------------------------------------------
//  Debounce dot key - active low
//---------------------------------------------------------

debounce de_dot(.clean_pb(clean_dot), .pb(~KEY_DOT), .clk(IF_clk));


//---------------------------------------------------------
//  Debounce dash key - active low
//---------------------------------------------------------

debounce de_dash(.clean_pb(clean_dash), .pb(~KEY_DASH), .clk(IF_clk));


//---------------------------------------------------------
//    PLLs 
//---------------------------------------------------------


/* 
	Divide the 10MHz reference and 122.88MHz clock to give 80kHz signals.
	Apply these to an EXOR phase detector. If the 10MHz reference is not
	present the EXOR output will be a 80kHz square wave. When passed through 
	the loop filter this will provide a dc level of (3.3/2)v which will
	set the 122.88MHz VCXO to its nominal frequency.
	The selection of the internal or external 10MHz reference for the PLL
	is made using a PCB jumper.

*/

wire ref_80khz; 
wire osc_80khz; 

// Use a PLL to divide 10MHz clock to 80kHz
C10_PLL PLL2_inst (.inclk0(OSC_10MHZ), .c0(ref_80khz), .locked());

// Use a PLL to divide 122.88MHz clock to 80kHz								
C122_PLL PLL_inst (.inclk0(_122MHz), .c0(osc_80khz), .locked());	
	
// Apply to EXOR phase detector 
assign FPGA_PLL = ref_80khz ^ osc_80khz; 

//-----------------------------------------------------------
//  LED Control  
//-----------------------------------------------------------

/*
	LEDs:  
	
	DEBUG_LED1  	- Lights when an Ethernet broadcast is detected
	DEBUG_LED2  	- Lights when traffic to the boards MAC address is detected
	DEBUG_LED3  	- Lights when detect a received sequence error or ASMI is busy
	DEBUG_LED4 		- Displays state of PHY negotiations - fast flash if no Ethernet connection, slow flash if 100T and on if 1000T
	DEBUG_LED5		- Lights when the PHY receives Ethernet traffic
	DEBUG_LED6  	- Lights when the PHY transmits Ethernet traffic
	DEBUG_LED7  	- Displays state of DHCP negotiations or static IP - on if ACK, slow flash if NAK, fast flash if time out 
					     and long then short flash if static IP
	DEBUG_LED8  	- Lights when sync (0x7F7F7F) received from PC
	DEBUG_LED9  	- Lights when a Metis discovery packet is received
	DEBUG_LED10 	- Lights when a Metis discovery packet reply is sent	
	
	Status_LED	    - Flashes once per second
	
	A LED is flashed for the selected period on the positive edge of the signal.
	If the signal period is greater than the LED period the LED will remain on.


*/

parameter half_second = 10000000; // at 48MHz clock rate

// flash LED1 for ~0.2 seconds whenever we detect a broadcast
Led_flash Flash_LED1(.clock(IF_clk), .signal(broadcast), .LED(DEBUG_LED1), .period(half_second));

// flash LED2 for ~0.2 seconds whenever we detect a packet addressed to this MAC address
Led_flash Flash_LED2(.clock(IF_clk), .signal(this_MAC), .LED(DEBUG_LED2), .period(half_second));

// flash LED3 for ~0.2 seconds when we have detected a received sequence error or ASMI is busy
Led_flash Flash_LED3(.clock(IF_clk), .signal(seq_error || busy), .LED(DEBUG_LED3), .period(half_second)); 

// flash LED7 for ~0.2 seconds whenever we detect a ping request
//Led_flash Flash_LED7(.clock(IF_clk), .signal(ping_request), .LED(DEBUG_LED3), .period(half_second));

// flash LED5 for ~ 0.2 second whenever the PHY gets data
Led_flash Flash_LED5(.clock(IF_clk), .signal(RX_DV), .LED(DEBUG_LED5), .period(half_second)); 	

// flash LED6 for ~ 0.2 second whenever the PHY sends data
Led_flash Flash_LED6(.clock(IF_clk), .signal(PHY_TX_EN), .LED(DEBUG_LED6), .period(half_second)); 	

// flash LED8 for ~0.2 seconds when we have detected sync 
Led_flash Flash_LED8(.clock(IF_clk), .signal(IF_SYNC_state == SYNC_RX_1_2), .LED(DEBUG_LED8), .period(half_second));

// flash LED9 for ~0.2 seconds whenever we detect a Metis discovery request
Led_flash Flash_LED9(.clock(IF_clk), .signal(METIS_discovery), .LED(DEBUG_LED9), .period(half_second));

// flash LED10 for ~0.2 seconds whenever we detect a Metis discovery reply
Led_flash Flash_LED10(.clock(IF_clk), .signal(METIS_discover_sent), .LED(DEBUG_LED10), .period(half_second));

//Flash Heart beat LED
reg [26:0]HB_counter;
always @(posedge PHY_CLK125) HB_counter = HB_counter + 1'b1;
assign Status_LED = HB_counter[25];  // Blink



//------------------------------------------------------------
//   Multi-state LED Control   - code in Led_control is for active LOW LEDs
//------------------------------------------------------------

parameter clock_speed = 25000000; // 25MHz clock 

// display state of PHY negotiations  - fast flash if no Ethernet connection, slow flash if 100T, on if 1000T
// and swap between fast and slow flash if not full duplex
Led_control #(clock_speed) Control_LED0(.clock(Tx_clock), .on(speed_1000T), .fast_flash(~speed_100T || ~speed_1000T),
										.slow_flash(speed_100T), .vary(!duplex), .LED(DEBUG_LED4));  
										
// display state of DHCP negotiations - on if ACK, slow flash if NAK, fast flash if time out and swap between fast and slow 
// if using a static IP address
Led_control # (clock_speed) Control_LED1(.clock(Tx_clock), .on(DHCP_ACK), .slow_flash(DHCP_NAK),
										.fast_flash(time_out), .vary(Assigned_IP_valid), .LED(DEBUG_LED7));	

function integer clogb2;
input [31:0] depth;
begin
  for(clogb2=0; depth>0; clogb2=clogb2+1)
  depth = depth >> 1;
end
endfunction


endmodule 
