// V1.0 17th July 2007
//
// Copyright 2007 Phil Harman VK6APH
//
//  HPSDR - High Performance Software Defined Radio
//
//
//  ALC Module
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



// Fixed point multiplier. Sample is a 16 bit signed integer. Multiply is a 16 bit fraction [0].[16]
// in the form  0.xxxx  e.g. to multiply Sample by  0.1234  use  1234 for multiply.
// Result is a 16 bit integer. 

// Shift fractional part left by 16 bits and divide by power of 10 being used to represent fraction.
// Need 16 bits for fraction so we can represent 0.9999  (i.e. 9999).

module ALC (out, sample, multiply);

input [15:0]sample;
input [15:0]multiply;
output wire [15:0]out;
wire [15:0]fraction;
wire [47:0]result;
wire [15:0] pos_sample;

assign pos_sample = sample[15] ? (~sample + 1'b1) : sample;  // convert to positive if negative 

assign fraction = {multiply, 16'd0} / 10000;  // [16].[16]/10000 = [0].[16]

assign result = {pos_sample,16'd0} * fraction;   // [16].[16] * [0].[16] = [16].[32] = [47:0]

assign out = sample[15] ? (~result[47:32] + 1) : result[47:32] ; // [15:0], revert to negative if needed


endmodule
