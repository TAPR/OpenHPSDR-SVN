/*
--------------------------------------------------------------------------------
This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Library General Public
License as published by the Free Software Foundation; either
version 2 of the License, or (at your option) any later version.
This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Library General Public License for more details.
You should have received a copy of the GNU Library General Public
License along with this library; if not, write to the
Free Software Foundation, Inc., 51 Franklin St, Fifth Floor,
Boston, MA  02110-1301, USA.
--------------------------------------------------------------------------------
*/


//------------------------------------------------------------------------------
//           Copyright (c) 2008 Alex Shovkoplyas, VE3NEA
//------------------------------------------------------------------------------



/*
FX2 firmware configures FIFO as follows:

IFCLK:
  FX2-internal
  48 MHz
  enabled
  normal polarity (posedge active)
  synchronous
  slave

PKTEND, SLOE, SLRD, SLWR, EF, FF:
  low active

EP2:
  valid
  bulk
  quadbuf
  OUT
  auto-out (no access to fifo from cpu)
  16-bit
  packetsize 512 bytes

EP4:
  not valid

EP6, EP8:
  valid
  bulk
  doublebuf
  IN
  auto-in
  16-bit
  packetsize 512 bytes
*/


module fx2endpoints(
  //FX2 side
  input reset,
  input FX2_ifclk,
  input [2:0] FX2_flags,
  inout [15:0] FX2_fifodata,
  output reg [1:0] FX2_fifoadr,
  output reg FX2_sloe,
  output reg FX2_slrd,
  output reg FX2_slwr,
  output FX2_slcs,
  output FX2_pktend,

  //FPGA side
  input iq_fifo_rdempty,
  output reg iq_fifo_rdreq,
  input [511:0] iq_fifo_q,  //IQ data from 8 receivers = 8 x (2x32)

  input dac_fifo_wrfull,
  output reg dac_fifo_wrreq,
  output reg [63:0] dac_fifo_data
  );



localparam EP2_FIFO_ADDR = 2'b00;
localparam EP6_FIFO_ADDR = 2'b10;
localparam EP8_FIFO_ADDR = 2'b11;


assign FX2_slcs = 0;   //enable FIFO pins in FX2
assign FX2_pktend = 1; //short packets not used


wire fx2_has_space = FX2_flags[1];
wire fx2_has_data = FX2_flags[2];


reg [4:0] state;
reg [4:0] word_no;
reg [511:0] outreg;


//3-state data port
assign FX2_fifodata = FX2_sloe? outreg[15:0] : 16'bzzzzzzzzzzzzzzzz;


initial
  begin
  FX2_sloe = 1;
  FX2_slrd = 1;
  FX2_slwr = 1;
  iq_fifo_rdreq = 0;
  dac_fifo_wrreq = 0;
  state = 0;
  word_no = 0;
  end


always @(posedge FX2_ifclk)
    case (state)


      //WRITE 512-BIT IQ DATA TO FX2


      00:
        //select fifo
        begin
        FX2_sloe <= 1;
        FX2_fifoadr <= EP6_FIFO_ADDR;
        state <= 1;
        end

      01:
        //let fifo selection take effect
        state <= 2;

      02:
        //FX2_fifoadr had two clocks to take effect, 
        //the fx2_has_space flag is now valid

        //see if can write
        if (fx2_has_space & ~iq_fifo_rdempty)
          begin
          //get data to write
          iq_fifo_rdreq <= 1;
          state <= 3;
          end
        else
          //can't write, go read
          state <= 10;

      03:
        //data are being asserted at iq fifo output, 
        //deassert iq_fifo_rdreq
        begin
        iq_fifo_rdreq <= 0;
        state <= 4;
        end

      04:
        begin
        //iq_fifo_q is now valid
        //register iq_fifo_q, assert FX2_slwr
        //write will occur on the next posedge of FX2_ifclk
        outreg <= iq_fifo_q;
        word_no <= 0;
        FX2_slwr <= 0;
        state <= 5;
        end

      05:
        begin
        //the word is being written to FX2 fifo, get the next word
        outreg <= outreg >> 16;
        word_no <= word_no + 5'b1;
        //if writing the last word, finish
        if (word_no == 5'b11111) 
          begin
          FX2_slwr <= 1;
          state <= 2;
          end
        end



      //READ 64-BIT DAC DATA FROM FX2


      10:
        //select fifo
        begin
        FX2_sloe <= 0;
        FX2_fifoadr <= EP2_FIFO_ADDR;
        state <= 11;
        end

      11:
        state <= 12;

      12:
        //see if can read
        if (fx2_has_data & ~dac_fifo_wrfull)
          begin
          FX2_slrd <= 0;
          word_no <= 0;
          state <= 13;
          end
        else
          //can't read, go write
          state <= 0;

      13:
        //read 4 16-bit words
        begin
        dac_fifo_data <= {FX2_fifodata, dac_fifo_data[63:16]};
        word_no <= word_no + 5'b1;
        if (word_no == 5'b00011)
          begin
          FX2_slrd <= 1;
          state <= 14;
          end
        end

      14:
        //write received data to DAC
        begin
        dac_fifo_wrreq <= 1;
        state <= 15;
        end

      15:
        //done, see if there is more data
        begin
        dac_fifo_wrreq <= 0;
        state <= 12;
        end

    endcase

endmodule
