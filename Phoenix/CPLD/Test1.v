// 28 Nov 2008 - Phil Harman VK6APH 
// Test1 file for Phoenix CPLD 
//
//
// Built using Quartus II v8.1  build 163

// Change log:
//
//	6  Nov 2008 - Started!
//	7  Nov 2008 - Added SPI interface to LTC1451 DAC
//			    - Added AD9912 phase word decoder
//	14 Nov 2008 - Added C&C PTT decode from C20 for testing
//				- Added 48MHz clock to DDS_CLK for testing AD9912
//  16 Nov 2008 - Added AD9912 test code at 14.195MHz and 1MHz
//  19 Nov 2008 - Added decode of Phoenix C&C data from Atlas C21
//  28 Nov 2008 - C&C decode working, DDS now feeds 90 degree clock divider
//				  Drive AD9912 from 48MHz clock
//  30 Nov 2008 - Only send AD9912 data when frequency changes
//				  Drive AD9912 from external 48MHz clock
//   1 Dec 2008 - Added QSE signals and control
//	 4 Dec 2008 - Test code for SoftRock 
//   6 Dec 2008 - remove freq x 4 for Mercury tests 
//  30 Mar 2009 - Moved C&C to C24 



module Test1(
			  output LED1,			// Yellow LED
			  output LED2,			// Green LED
			  output DDS_CLK,		// 125MHz clock 
			  input  DDS_OUT, 		// Clock from AD9912
			  input  CLK_48MHZ,
			  input  CBCLK, 		// 3.072MHz clock from Atlas C8
			  input  CLRCLK,		// 48kHz clock from Atlas C9
			  input  PTT,			// PTT from Atlas C15, active high
			  input  CC,			// C&C data from C21 for testing
			  output reg I_CLKRX,	// QSD I clock
			  output reg Q_CLKRX,	// QSD Q clock
			  output reg I_CLKTX,		// QSE I clock
			  output reg Q_CLKTX,		// QSE Q clock
			  output reg RXOE1,		// QSD enable 
			  output reg RXOE2,
			  output  TXOE1,		// QSE enable 
			  output  TXOE2,
			  output reg DIN,		// LTC1451 SPI interface 
			  output reg DCLK,
			  output reg DLD,
			  output reg IO_UPD,	// AD9912 control signals 
			  output reg CSB,
			  output reg SDIO,
			  output reg SCLK			  
			  );
			  
assign LED1 = 1'b0;  		// turn Green Led on
assign LED2 = !PTT_out;   	// turn Yellow  Led on when PTT active 

// temp link DDS_CLK to 48MHz clock from Atlas so can test AD9912 
//assign DDS_CLK = CLK_48MHZ;


// This code divides the DDS clock from U23 by 4 to produce two signals
// in phase quadrature to drive the QSD & QSE


reg [1:0]state;
//always @ (posedge CLK_48MHZ)
always @ (posedge DDS_OUT)
begin
case (state)
0:	begin
	//I_CLKRX <= 1'b1;
	//Q_CLKRX <= 1'b1;
	I_CLKTX <= 1'b1;
	Q_CLKTX <= 1'b0;
	//RXOE1 <= 1'b1;
	//RXOE2 <= 1'b0;
	state <= 1;
	end 
1:	begin
	I_CLKTX <= 1'b1;
	Q_CLKTX <= 1'b1;
	//RXOE1 <= 1'b0;
	//RXOE2 <= 1'b1;
	state <= 2;
	end 	
2:	begin
	I_CLKTX <= 1'b0;
	Q_CLKTX <= 1'b1;
	//I_CLKRX <= 1'b0;
	//Q_CLKRX <= 1'b0;
	//RXOE1 <= 1'b1;
	//RXOE2 <= 1'b0;
	state <= 3;
	end 
3:	begin
	I_CLKTX <= 1'b0;
	Q_CLKTX <= 1'b0;
	//RXOE1 <= 1'b0;
	//RXOE2 <= 1'b1;
	state <= 0;
	end 
endcase
end

//////////////////////////////////////////////////////////////
//
// 		SPI interface to TLC1451 ADC
//
/////////////////////////////////////////////////////////////

wire[11:0]DAC_data;
wire spi_clock;

reg [2:0]spi_state;
reg [3:0]data_count;
reg [11:0]previous_DAC_data;		// used in future to detect change in data 

assign DAC_data = 12'b011111111111; // set DAC to mid scale for now
assign spi_clock = CBCLK;

always @ (posedge spi_clock)
begin
case (spi_state)
0:	begin
		if (DAC_data != previous_DAC_data)begin
			data_count <= 11;		// set starting bit count to 11
			spi_state <= 1;
		end
		else spi_state <= 0; 		// wait for Alex data to change
	end		
1:	begin
	DIN <= DAC_data[data_count];	// set up data to send
	spi_state <= 2;
	end
2:	begin
	DCLK <= 1'b1;					// set clock high
	spi_state <= 3;
	end
3:	begin
	DCLK <= 1'b0;					// set clock low
	spi_state <= 4;
	end
4:	begin
		if (data_count == 0)begin		// transfer complete
			DLD <= 1'b1; 				// strobe data
			spi_state <= 5;
		end
		else begin
	     	data_count <= data_count - 1'b1;
			spi_state  <= 1;  			// go round again
		end
	end
5:	begin
	DLD <= 1'b0;						// reset data strobe
	previous_DAC_data <= DAC_data; 		// save current data 
	spi_state <= 0;						// reset for next run
	end
endcase
end

///////////////////////////////////////////////////////////
//
//    Phoenix Command and Control Decoder 
//
///////////////////////////////////////////////////////////
/*

	The temporary Phoenix C&C encoder in Ozy broadcasts data over the Atlas bus (C21) for
	use by Phoenix.  The data is in I2S format with the clock being CBLCK and the start of each frame
	being indicated using the negative edge of CLRCLK.
	
	The data format is as follows:
	
	<[32]PTT><[31:0]phase_word>
	
	for a total of 33 bits. 
	
*/


reg [5:0] bits;     // how many bits clocked 
reg [1:0]CC_state;
reg [32:0] CCdata;	// 33 bits of C&C data

always @(posedge CBCLK)  // use CBCLK  from Atlas C8 
begin
case(CC_state)
0:	begin
	if (CLRCLK == 0)CC_state <= 0;			// loop until CLRLCK is high   
	else CC_state <= 1;
	end
1:	begin
		if (CLRCLK)	CC_state <= 1;			// loop until CLRCLK is low  
		else begin
		bits <= 6'd32;						
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
// Multiply phase_word by 4 to give a 4 x clock for the 90 degree
// clock generator.

reg PTT_out;
reg [47:0] phase_word;

always @ (negedge CLRCLK)  
begin 
	PTT_out <= CCdata[32]; 	    			// PTT from PC via USB
	//phase_word <= {CCdata[29:0],18'h00000};	// 32 bit AD9912 phase word x 4 & extended to 48 bits 
	phase_word <= {CCdata[31:0],16'h0000};	// 32 bit AD9912 phase word & extended to 48 bits 
end



//////////////////////////////////////////////////////////////
//
// 		AD9912 Interface
//
/////////////////////////////////////////////////////////////


// Test code using 48MHz Atlas clock x 20 = 960MHz 
// send data to SDIO since in 3 wire mode


reg [5:0]bit_count;
reg [39:0]DDS_setup;
reg [63:0]DDS_data;
wire write = 0;			// Write is active low

reg [47:0] prev_phase_word;

reg [3:0]AD9912;

always @ (posedge CBCLK)
begin 
case (AD9912)
0: 	begin 
	bit_count <=  39;
	DDS_setup <= {write,1'b1,1'b0,13'h0022,8'h80,8'h00,8'h08}; // set up registers 0022,0021 & 0020
	AD9912 <= 1;
	end 
1:  begin
	CSB <= 1'b0; 					// AD9912 chip select low
	SDIO <= DDS_setup[bit_count];
	AD9912 <= 2;
	end
2:	begin
	SCLK <= 1; 						// clock data into AD9912
	AD9912 <= 3;
	end
3:  begin
	SCLK <= 0;
	if (bit_count == 0)begin
		bit_count <= 63; 			// reset counter for next register
		DDS_data <= {write,1'b1,1'b1,13'h01AB,phase_word}; // set up phase word from Ozy
		AD9912 <= 4;				// done so do next register
		end 
	else begin
		bit_count <= bit_count - 1'b1;
		AD9912 <= 1; 				// send next bit
		end
	end
4:  begin
	SDIO <= DDS_data[bit_count];
	AD9912 <= 5;
	end
5:	begin
	SCLK <= 1; // clock data into AD9912
	AD9912 <= 6;
	end
6:	begin
	SCLK <= 0;
	if (bit_count == 0)begin
		CSB <= 1'b1; 				// de-select AD9912
		AD9912 <= 7;
		prev_phase_word <= phase_word; 
		end
	else begin
		bit_count <= bit_count - 1'b1;
		AD9912 <= 4;
		end 
	end
7: begin
	IO_UPD <= 1;					// strobe I/O update pin 
	AD9912 <= 8;
	end
// wait until new phase word is sent otherwise loop here
8: begin
	IO_UPD <= 0;
	if (prev_phase_word == phase_word)
		AD9912 <= 8;  					// loop until new phase word 
	else begin
		AD9912 <= 3; 				// have new phase word so send it
		CSB <= 1'b0; 				// AD9912 chip select low
		end
	end	
endcase
end


//////////////////////////////////////////////////////////////
//
// 		QSE Control
//
/////////////////////////////////////////////////////////////


// enable QSE when PTT inactive else disable

//assign TXOE1 = (PTT || PTT_out) ? RXOE1 : 1'b1; 
//assign TXOE2 = (PTT || PTT_out) ? RXOE2 : 1'b1; 
//assign I_CLKTX = (PTT || PTT_out) ?	I_CLKRX : 1'b0;
//assign Q_CLKTX = (PTT || PTT_out) ?	Q_CLKRX : 1'b0;

endmodule
