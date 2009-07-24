// V1.0  23rd July 2009
//
// Copyright 2008 Phil Harman VK6APH
//
//  HPSDR - High Performance Software Defined Radio
//
//
//  Cyclops Spectum Analyzer  - ADF4112 SPI interface driver 
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
			  +--+  +--+  +--+  +--+  +--+  +--+  +--+  +--+  +--+  +--+  +--+  +--+  +--+  +--+  +--+  
	clock ----+  +--+  +--+  +--+  +--+  +--+  +--+  +--+  +--+  +--+  +--+  +//+  +--+  +--+  +--+  +--+

			  ------+																		      +-------------
	ready           +--------------------------------------------------------// ------------------+

				 +--------- //
	start     ---+

			  ------------+																		  +--------------
	SPI_LE                +--------------------------------------------------//-------------------+
	
								+-----+   		  +-----+           +-----+           +-----+          
	SPI_CLOCK ------------------+     +-----------+     +-----------+     +- // ------+     +--------------------
	
	
	SPI_DATA  ------------[	bit 23           ][ bit 22        ][ bit 21     // [ bit 0      ]        
	
	*/




module ADF4112_SPI ( 
				input clock,
				input start,	  	// high when data avaiable to be sent to LMX2326
				output reg ready, 	// high when module ready to take data
				input [23:0]data,	// data to send to LMX2326
				output reg SPI_clock,
				output reg SPI_LE,
				output reg SPI_data
				);
				
reg [2:0]SPI_state;
reg [4:0]bit_count;

				
always @ (posedge clock)
begin

case (SPI_state)
// send data to ADF2214 

0:	begin
	if (start) begin
		ready <= 1'b0;				// module busy
		bit_count <= 24;
		SPI_state <= 1;
		end
	else begin
		ready <= 1'b1;				// module ready
		SPI_LE <= 1'b1;	
		SPI_state <= 0;				// loop until data changes
		end
	end 
1:	begin
	SPI_LE <= 1'b0;
	if (bit_count == 0)begin 		// all data sent 
		SPI_LE <= 1'b1;				// set LE high 
		SPI_state <= 0;				// wait for next data set
		end
	else begin 
		SPI_data <= data[bit_count - 1'b1];
		SPI_state <= 2;
		end
	end
	
// clock data into ADF4112
2:	begin
	SPI_clock <= 1'b1;				// set clock high 
	SPI_state <= 3;
	end
	
// decrement  bit counter
3:	begin
	SPI_clock <= 1'b0;				// set clock low
	bit_count <= bit_count - 1'b1;	// decrement bit counter
	SPI_state <= 1;
	end 
	
endcase
end

endmodule