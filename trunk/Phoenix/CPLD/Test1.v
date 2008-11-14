// 6 Nov 2008 - Phil Harman VK6APH 
// Test1 file for Phoenix CPLD 
//
//
// Built using Quartus II v8.0  build 215

// Change log:
//
//	6  Nov 2008 - Started!
//	7  Nov 2008 - Added SPI interface to LTC1451 DAC
//			    - Added AD9912 phase word decoder
//	14 Nov 2008 - Added C&C PTT decode from C20 for testing
//				- Added 48MHz clock to DDS_CLK for testing AD9912  


module Test1(
			  output LED1,			// Yellow LED
			  output LED2,			// Green LED
			  output  DDS_CLK,		// 125MHz clock 
			  input  CLK_48MHZ,
			  input  CBCLK, 		// 3.072MHz clock from Atlas C8
			  input  CLRCLK,		// 48kHz clock from Atlas C9
			  input  PTT,			// PTT from Atlas C15, active high
			  input  phase_data,	// AD9912 phase data from Atlas C21
			  input  CC,			// C&C data from C20 for testing
			  output reg I_CLKRX,	// QSD I clock
			  output reg Q_CLKRX,	// QSD Q clock
			  output I_CLKTX,		// QSE I clock
			  output Q_CLKTX,		// QSE Q clock
			  output RXOE1,			// QSD enable 
			  output RXOE2,
			  output TXOE1,			// QSE enable 
			  output TXOE2,
			  output reg DIN,		// LTC1451 SPI interface 
			  output reg DCLK,
			  output reg DLD
			  //output IO_UPD,		// AD9912 control signals 
			  //output CSB,
			  //output SDIO,
			  //output SCLK			  
			  );
			  
assign LED1 = 1'b0;  // turn Green Led on
//assign LED1 = AD9912_phase[0]; // temp to get phase word code to compile 
assign LED2 = !PTT_out;   // turn Yellow  Led on when PTT active 

// temp link DDS_CLK to 48MHz clock from Atlas so can test AD9912 
assign DDS_CLK = CLK_48MHZ;

// This test code divides the 48MHz clock from 
// the Atlas bus on C3 by 4 to produce two signals
// in phase quadrature to drive the QSD & QSE

reg [1:0]state;

always @ (posedge CLK_48MHZ)
begin
case (state)
0:	begin
	I_CLKRX <= 1'b1;
	Q_CLKRX <= 1'b0;
	state <= 1;
	end 
1:	begin
	I_CLKRX <= 1'b1;
	Q_CLKRX <= 1'b1;
	state <= 2;
	end 	
2:	begin
	I_CLKRX <= 1'b0;
	Q_CLKRX <= 1'b1;
	state <= 3;
	end 
3:	begin
	I_CLKRX <= 1'b0;
	Q_CLKRX <= 1'b0;
	state <= 0;
	end 
endcase
end

// set the QSD and QSE drive signals equal for testing

assign I_CLKTX = I_CLKRX;
assign Q_CLKTX = Q_CLKRX;

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

//////////////////////////////////////////////////////////////
//
// 		Phase Word  receiver
//
/////////////////////////////////////////////////////////////

/*

	The Phase Word  encoder in Ozy broadcasts data over the Atlas bus (C21)**** 
	the phase word used by the AD9912.  The data is in 
	I2S format with the clock being CBLCK and the start of each frame
	being indicated using the negative edge of CLRCLK.
	
	The data format is as follows:
	
	<[32]PTT><[31:0]phase_word> **** not yet implemented
	use C&C on C20 for testing to give PTT signal
	
*/

// to do

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
	
*/

reg [5:0] bits;     // how many bits clocked 
reg [1:0]CC_state;
reg [58:0] CCdata;	// 54 bits of C&C data

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
		bits <= 6'd58;						
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

reg PTT_out;

always @ (negedge CLRCLK)  
begin 
	PTT_out <= (CCdata[58]); 	// PTT from PC via USB 
end


//////////////////////////////////////////////////////////////
//
// 		AD9912 Interface
//
/////////////////////////////////////////////////////////////

// to do 


//////////////////////////////////////////////////////////////
//
// 		General IO
//
/////////////////////////////////////////////////////////////

// enable QSD when PTT inactive, chip is active low

assign RXOE1 = (PTT || PTT_out) ? 1'b1 : 1'b0; 
assign RXOE2 = RXOE1; 


// enable QSE when PTT active, chip is active low 

assign TXOE1 = ~RXOE1;
assign TXOE2 = ~RXOE2;






endmodule
