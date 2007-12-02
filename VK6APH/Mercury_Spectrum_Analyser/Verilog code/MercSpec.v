/***********************************************************
*
*	Test program for LT2208
*
************************************************************/

// (C) Phil Harman VK6APH 2006,2007


/* 	This program interfaces the LT2208 to PowerSDR over USB.
	The data from the LT2208 is in 16 bit parallel format and 
	is valid at the positive edge of the LT2208 125MHz clock.
	
	Change log:
	
	11 Nov 2007
	
	Start coding based on Mercury II code
	Load RAM from file with 12 cycles of sine wave and send continously to FX2.
	This simulates 12.5MHz sine wave when sampling at 125MHz.
	Runs off IFCLK.
	

*/
	

module MercSpec(ADC,
        IFCLK, FX2_FD, FLAGA, FLAGC, SLWR, SLRD, SLOE, PKEND, FIFO_ADR, DEBUG_LED0,
		DEBUG_LED1, DEBUG_LED2,DEBUG_LED3, FPGA_CLK6IN );
		

input [15:0]ADC;				// samples from LT2208
input FPGA_CLK6IN;				// 125MHz clock from Penelope via LVDS
input IFCLK;                   	// FX2 IFCLOCK - 48MHz
//inout  [15:0] FX2_FD;           // bidirectional FIFO data to/from the FX2
output  [15:0] FX2_FD;           // *** use so simulation works
input FLAGA;
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



wire PKEND;
reg [1:0] state_FX;				// state for FX2
reg SLOE;						// FX2 data bus enable - active low
reg SLEN; 						// Put data on FX2 bus
reg SLRD;						// FX2 read - active low
reg SLWR;						// FX2 write - active low 
reg [1:0] FIFO_ADR;				// FX2 register address 

assign PKEND = 1'b1;



assign clock = FPGA_CLK6IN;

// use Megafunction to create 4096 word dual port RAM

reg [11:0]address = 4095; // current read address
reg [11:0]wraddress = 4095; // current write address
reg write_enable = 0;
wire [15:0]q;				

RAM_4096_dualport RAM(.data(temp_ADC),.rdaddress(address),.rdclock(IFCLK),.wraddress(wraddress),.wrclock(clock),.wren(write_enable),.q(q));

// RAM is loaded with a 12.5MHz sine wave using sine_table.mif at start up
// send this data to the PC via FX2 


// read RAM data and send to FX2

wire EP6_ready = FLAGC; 		// high when we can write to EP6
reg run ;

always @ (negedge IFCLK) // posedge looks cleaner on the simulation
begin
SLOE <= 1'b1;
SLRD <= 1'b1;
FIFO_ADR <= 2'b10;				// select EP6

//if (!run)   // run in loop for now
//begin
case (state_FX)
	// check Tx FIFO is ready then set Write strobe 
	0: begin
		if (EP6_ready) begin  					// if EP6 is ready, write to it and exit this state
			address <= address + 1'b1;
			SLEN <= 1'b1;
	    	state_FX <= 1;
		end
		else begin                  			// otherwise, hang out here until fifo is ready
		SLWR <= 1;
		state_FX <= 0;  
		end
	end
	//  set SLWR, data is written to FX2 fifo on negative edge of SLWR
	1: begin
		SLWR <= 1'b0;
		state_FX <= 2;
		end
	//  reset SLWR and tristate SLEN
	2: begin
		SLWR <= 1;
		SLEN <= 1'b0;
//			if (address == 4095)
//				run <= 1'b1; 
		state_FX <= 0;
		end
		default: state_FX <= state_FX + 1'b1;
		endcase
	end
//end

// Swap endian so data is correct at PC end

assign FX2_FD[15:8] = q[7:0];
assign FX2_FD[7:0]  = q[15:8];	

//assign FX2_FD = q; // don't swap to simlify simulation!
 


assign DEBUG_LED0 = 0;
assign DEBUG_LED1 = 0;
assign DEBUG_LED2 = 0;
assign DEBUG_LED3 = 0;

endmodule 


// A Digital Output Randomizer is fitted to the LT2208. This complements bits 15 to 1 if 
// bit 0 is 1. This helps to reduce any pickup by the A/D input of the digital outputs. 
// We need to de-ramdomize the LT2208 data if this is turned on. 

/*
reg [15:0]temp_ADC;

always @ (posedge clock)
begin 
	if (ADC[0]) temp_ADC <= {~ADC[15:1],ADC[0]};
	else temp_ADC <= ADC;
end 
*/


/*
// load ADC data to RAM

always @ (posedge clock)
begin 
write_enable <= 0; 					// disable write to ram
	if (run)
	begin
		wraddress <= wraddress + 1'b1;
		write_enable <= 1'b1; 		// write current data to RAM 
		if(address == 4095)
		run <= 0;
	end
end
*/

