// -*- verilog -*-
//
//  USRP - Universal Software Radio Peripheral
//
//  Copyright (C) 2003 Matt Ettus
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

// Following defines conditionally include RX path circuitry

module rx_chain
  (input clock,
   input reset,
   input enable,
   input wire [7:0] decim_rate,
   input sample_strobe,
   input decimator_strobe,
   output wire hb_strobe,
   input [6:0] serial_addr, input [31:0] serial_data, input serial_strobe,
   input wire [15:0] i_in,
   input wire [15:0] q_in,
   output wire [23:0] i_out,
   output wire [23:0] q_out
   );

   parameter FREQADDR = 0;
   parameter PHASEADDR = 0;
   
   wire [31:0] phase;
   wire [15:0] bb_i, bb_q;
   wire [15:0] hb_in_i, hb_in_q;
      
   	phase_acc #(FREQADDR,PHASEADDR,32) rx_phase_acc
     (.clk(clock),.reset(reset),.enable(enable),
      .serial_addr(serial_addr),.serial_data(serial_data),.serial_strobe(serial_strobe),
      .strobe(sample_strobe),.phase(phase) );

   	cordic rx_cordic
     ( .clock(clock),.reset(reset),.enable(enable), 
       .xi(i_in),.yi(q_in),.zi(phase[31:16]),
       .xo(bb_i),.yo(bb_q),.zo() );
   
	cic_decim cic_decim_i_0
     ( .clock(clock),.reset(reset),.enable(enable),
       .rate(decim_rate),.strobe_in(sample_strobe),.strobe_out(decimator_strobe),
       .signal_in(bb_i),.signal_out(i_out) );

	cic_decim cic_decim_q_0
     ( .clock(clock),.reset(reset),.enable(enable),
       .rate(decim_rate),.strobe_in(sample_strobe),.strobe_out(decimator_strobe),
       .signal_in(bb_q),.signal_out(q_out) );
	
endmodule // rx_chain
