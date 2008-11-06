// 6 Nov 2008 - Phil Harman VK6APH 
// Test1 file for Phoenix CPLD 
//
//
// Built using Quartus II v8.0  build 215

// Change log:
//
//	6 Nov 2008 - Started!

module Test1(
			  output LED1,			// Yellow LED
			  output LED2,			// Green LED
			  input  CLK_48MHZ,
			  input  PTT,			// PTT from Atlas C15, active high
			  output reg I_CLKRX,	// QSD I clock
			  output reg Q_CLKRX,	// QSD Q clock
			  output I_CLKTX,		// QSE I clock
			  output Q_CLKTX,		// QSE Q clock
			  output RXOE1,			// QSD enable 
			  output RXOE2,
			  output TXOE1,			// QSE enable 
			  output TXOE2
			  //output IO_UPD,		// AD9912 control signals 
			  //output CSB,
			  //output SDIO,
			  //output SCLK			  
			  );
			  
assign LED1 = 1'b0;  // turn Yellow Led on
assign LED2 = PTT;   // turn Green  Led on when PTT active 


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

// to do 


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

assign RXOE1 = PTT ? 1'b1 : 1'b0; 
assign RXOE2 = RXOE1; 


// enable QSE when PTT active, chip is active low 

assign TXOE1 = ~RXOE1;
assign TXOE2 = ~RXOE2;




endmodule
