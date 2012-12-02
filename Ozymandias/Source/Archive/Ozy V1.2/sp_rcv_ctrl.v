// Copyright 2009  Kirk Weedman KD7IRS 

//  HPSDR - High Performance Software Defined Radio
//
//  Mercury to Atlas bus interface.
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
//
// Change log:
//
// 15 Feb 2009 - first version


//////////////////////////////////////////////////////////////
//
//      State Machine to manage Spectrum data transfers
//
// By: Kirk Weedman KD7IRS
//
//////////////////////////////////////////////////////////////

`timescale 1 ns/100 ps

module sp_rcv_ctrl (rst, clk, trigger, fifo_wreq, flag);
input  wire   rst;
input  wire   clk;
output reg    trigger;
input  wire   fifo_wreq;
input  wire   flag;

reg  sp_state;
reg  sp_state_next;

reg  [11:0] count;

localparam IDLE      = 1'b0,
           CHK       = 1'b1;
           
localparam TPD = 1;

always @(posedge clk)
begin
  if (rst)
    sp_state <= #TPD 1'b0;
  else
    sp_state <= #TPD sp_state_next;

  if (rst)
    trigger <= #TPD 1'b0;
  else
    trigger <= #TPD (sp_state == CHK) && (count[11:1] == 0); // trigger = true until we start getting data

  if (rst)
    count <= #TPD 1'b0;
  else if (sp_state == IDLE)
    count <= #TPD 1'b0;
  else if (fifo_wreq)
    count <= #TPD count + 1'b1;
end

always @*
begin
  case (sp_state)
    IDLE:
    begin
      if (flag) // is USB EP4 FIFO ready for more data
        sp_state_next = CHK;
      else
        sp_state_next = IDLE;
    end

    CHK:
    begin
      if (fifo_wreq && (count == 4095))
        sp_state_next = IDLE;
      else
        sp_state_next = CHK;
    end

    default:
      sp_state_next = IDLE;
  endcase
end

endmodule