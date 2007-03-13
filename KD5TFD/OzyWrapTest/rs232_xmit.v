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

module rs232_xmit(data_i, clk_i, space_avail_o, write_req_i, baud_clock_i, xmit_o);

input[7:0] data_i;  
input clk_i; 
output space_avail_o; 
input write_req_i; 
input baud_clock_i; 
output xmit_o; 
reg xmit_o; 
// declare our fifo 

reg fifo_read_strobe; 
reg fifo_write_strobe; 
wire fifo_empty; 
wire fifo_full; 
reg[7:0] xmit_bits; 

rs232_tx_fifo tx_fifo(.clock(clk_i), 
					  .data(data_in_holding), 
				      .rdreq(fifo_read_strobe), 
					  .wrreq(fifo_write_strobe), 
					  .empty(fifo_empty), 
					  .full(fifo_full), 
					  .q(xmit_bits)					
	                 );


reg[7:0] data_in_holding; 

reg[1:0] input_state; 

// input state machine - take char on d and put in fifo 
always @ ( posedge clk_i ) begin 

	case ( input_state ) 
	
		0: begin  // waiting for write_req_i to go hi 
			if ( write_req_i == 1 ) begin 
				input_state <= 1; 
				data_in_holding <= data_i; 
			end 
		end 
	
		1: begin  // strobe data into fifo 
			fifo_write_strobe <= 1; 
			input_state <= 2; 		
		end 
		
		2: begin
			fifo_write_strobe <= 0; 
			input_state <= 0; 
		end 
	endcase 
end // always 


// output side state machine 
reg[3:0] xmit_count;
reg last_baud_clock; 
reg[1:0] output_state; 
 
always @ ( posedge clk_i ) begin 

	last_baud_clock <=  baud_clock_i; 
	case ( output_state ) 
		0: begin  // waiting for char to become available 
			xmit_o <= 1;  // hi state is idle 
			if ( !fifo_empty ) begin  // char is available 
				output_state <= 1; 
				fifo_read_strobe <= 1; 				
				xmit_count <= 0; 
			end 
		end 
				
		1: begin  // starting xmit/waiting for baudclock to go hi
			fifo_read_strobe <= 0; 
			if ( xmit_count == 11 ) begin  // all done this byte -- go get another 
				output_state <= 0; 
			end 
					
			if ( last_baud_clock == 0 && baud_clock_i == 1 ) begin 
				output_state <= 2; 
			end 
		end 

		2: begin  // baudclock just went hi -- transmit bit 
			if ( xmit_count == 0 ) begin 
				xmit_o <= 0; // start_bit 
			end 
			else if ( xmit_count == 9 || xmit_count == 10 ) begin  // start and stop bits 
				xmit_o <= 1;  	
			end 
			else begin 
				xmit_o <= xmit_bits[xmit_count-1]; 
			end 
			xmit_count <= xmit_count + 1; 
			output_state <= 1; 
		end 
	endcase 
end



endmodule 




//module rs232_tx_fifo (
//        clock,
//        data,
//        rdreq,
//        wrreq,
//        empty,
//        full,
//        q);

//        input     clock;
//        input   [7:0]  data;
//        input     rdreq;
//        input     wrreq;
//        output    empty;
//        output    full;
//        output  [7:0]  q;   