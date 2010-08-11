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


  
module wspr_xmit (
	input clock,
	input [31:0] phase_words [3:0], 
	input [1:0] symnum, 
	input [15:0] in_level,
	output [14:0] i_out
	);
	 
	
	cpl_cordic #(.OUT_WIDTH(15))
              cordic_inst (.clock(clock), 
                            // .frequency(492726016),
                            .frequency(phase_words[symnum]), 
                            .in_data_I(in_level),
                            .in_data_Q(16'd0), .out_data_I(i_out) 
                            //  .out_data_Q(C122_q_out)
                            );

	
	
endmodule	
	