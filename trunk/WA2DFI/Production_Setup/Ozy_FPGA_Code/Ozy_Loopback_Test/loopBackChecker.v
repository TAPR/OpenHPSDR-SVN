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
//
module loopBackCheck(ref_sig_i, check_sig_i, not_ok_o, master_clock_i, reset_i); 

input ref_sig_i; 
input check_sig_i; 
output not_ok_o; 
input master_clock_i; 
input reset_i; 

reg not_ok_o; 
reg last_ref_sig; 
reg armed;
reg edge_det; 

always @ (negedge master_clock_i) begin 
	if ( reset_i ) begin 
		not_ok_o 		<= 0;
		edge_det		<= 0; 
		armed 			<= 0;
		last_ref_sig 	<= ref_sig_i; 
	end 
	else begin 
		last_ref_sig <= ref_sig_i;
		if ( armed == 1 ) begin 
			if ( check_sig_i != ref_sig_i ) begin 
				not_ok_o <= 1; 
			end 
			armed 		<= 0;
			edge_det	<= 0; 
		end 
		else if ( edge_det == 1 ) begin
				armed	<= 1;
		end
		else begin 
			if ( last_ref_sig != ref_sig_i ) begin
				edge_det <= 1; 
			end 
		end 
	end 
end 

endmodule 