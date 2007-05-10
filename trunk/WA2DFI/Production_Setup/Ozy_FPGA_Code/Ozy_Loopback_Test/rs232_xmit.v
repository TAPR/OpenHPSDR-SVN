// 
// 
// Copyright 2007  Bill Tracey KD5TFD (bill@ewjt.com)
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
//  3 Apr 2007	Scott Cowling	Add space_avail_o signal to input state machine
//								Clean up comments and format
//

module rs232_xmit	(data_i,
					clk_i,
					space_avail_o,
					write_req_i,
					baud_clock_i,
					xmit_o);

//
// Declare I/O pins
//
input [7:0]	data_i;  
input 		clk_i; 
output 		space_avail_o; 
input 		write_req_i; 
input 		baud_clock_i; 
output 		xmit_o;

//
// Declare types for outputs
// 
reg 		xmit_o;
reg			space_avail_o;


//--------------------------------------------------------
//
// Local declarations
//

// FIFO pins
//
reg  [7:0] 	data_in_holding; 
reg 		fifo_read_strobe; 
reg 		fifo_write_strobe; 
wire 		fifo_empty; 
wire 		fifo_full; 
wire [7:0] 	xmit_bits; 

// Input state machine
//
reg  [1:0] 	input_state; 			// state bits

// Output state machine
//
reg  [3:0]	xmit_count;				// transmit bit counter
reg 		last_baud_clock; 		// last value used for edge detect
reg  [1:0]	output_state; 			// state bits


//--------------------------------------------------------

// 
// Instantiate the transmit fifo 
//
rs232_tx_fifo tx_fifo(.clock(clk_i), 
					  .data(data_in_holding), 
				      .rdreq(fifo_read_strobe), 
					  .wrreq(fifo_write_strobe), 
					  .empty(fifo_empty), 
					  .full(fifo_full), 
					  .q(xmit_bits)					
	                 );




//--------------------------------------------------------
//
// input state machine - take char on d and put in fifo 
//
always @ ( posedge clk_i ) begin 

	case ( input_state ) 
	
		0: begin  // waiting for write_req_i to go hi 
			if ( write_req_i == 1'b1 ) begin 
				data_in_holding <= data_i;
				space_avail_o	<= 1'b0;
				input_state 	<= 2'h1; 
			end
			else begin
				space_avail_o	<= ~fifo_full;
				input_state		<= 2'h0;
			end 
		end 
	
		1: begin  // strobe data into fifo 
			fifo_write_strobe 	<= 1'b1; 
			input_state 		<= 2'h2; 		
		end 
		
		2: begin
			fifo_write_strobe 	<= 1'b0; 
			input_state 		<= 2'h0; 
		end 
		
		default: begin
			input_state			<= 2'h0;
		end

	endcase 
end

//--------------------------------------------------------
//
// output side state machine - take char from FIFO and send it
//
always @ ( posedge clk_i ) begin 

	last_baud_clock <=  baud_clock_i; 
	case ( output_state ) 
		0: begin  								// waiting for char to become available 
			xmit_o 	<= 1;  						// hi state is idle 
			if ( !fifo_empty ) begin  			// char is available 
				fifo_read_strobe 	<= 1; 				
				xmit_count 			<= 0;
				output_state 		<= 1;  
			end 
		end 
				
		1: begin  								// starting xmit/waiting for baudclock to go hi
			fifo_read_strobe <= 0; 
			if ( xmit_count == 11 ) begin  		// all done this byte -- go get another 
				output_state <= 0; 
			end 
					
			if ( last_baud_clock == 0 && baud_clock_i == 1 ) begin 
				output_state <= 2; 
			end 
		end 

		2: begin  								// baudclock just went hi -- transmit bit 
			if ( xmit_count == 0 ) begin 
				xmit_o 		<= 0; 				// start_bit 
			end 
			else if ( xmit_count == 9 || xmit_count == 10 ) begin  // start and stop bits 
				xmit_o 		<= 1;  	
			end 
			else begin 
				xmit_o 		<= xmit_bits[xmit_count-1]; 
			end 
			xmit_count 		<= xmit_count + 4'h1; 
			output_state 	<= 1; 
		end

		default: begin
			output_state	<= 0;
		end

	endcase 
end

endmodule 