//////////////////////////////////////////////////////////////
//
//      Read A/D converter, send sync and C&C to Tx FIFO
//
//////////////////////////////////////////////////////////////

/*
        The following code sends the sync bytes, control bytes and A/D samples.

        The code is structured around watching transitions of LRCLK which will
        be as fast or faster than CLRCLK (48kHz). 
        
        At each state we determine what data needs to to be latched into a TX_FIFO_SZ word Tx FIFO.

        Each frame is 512 bytes long. Initially 8 bytes of sync & control bytes are sent,
        followed by 512-8 bytes of AK5394A and TLV320 data.  The initial 8 bytes are
        comprised of 3 sync bytes and 5 control bytes. The 504 bytes are comprised of
        NUM_LOOPS *(3 AK539A left bytes, 3 AK5394A right bytes and 2 TLV320 microphone/line
        bytes.

        Each frame is 512 bytes long = 8 initial bytes + NUM_LOOPS*8

        If PTT_in, dot or dash inputs are acitve they are sent in C&C
*/

`timescale 1 ns/100 ps

module Tx_fifo_ctrl(IF_reset, IF_clk, Tx_fifo_wdata, Tx_fifo_wreq, Tx_fifo_full, Tx_fifo_used,
                    Tx_fifo_clr, Tx_IQ_mic_rdy, Tx_IQ_mic_ack,
                    IF_chan, IF_max_chan, Tx_IQ_mic_data,
                    clean_dash, clean_dot, clean_PTT_in, ADC_OVERLOAD,
                    Penny_serialno, Merc_serialno, Ozy_serialno, Penny_ALC);
parameter RX_FIFO_SZ = 2048;
parameter TX_FIFO_SZ = 4096;
parameter IF_TPD = 1;

localparam RFSZ = clogb2(RX_FIFO_SZ-1);  // number of bits needed to hold 0 - (RX_FIFO_SZ-1)
localparam TFSZ = clogb2(TX_FIFO_SZ-1);  // number of bits needed to hold 0 - (TX_FIFO_SZ-1)

input  wire            IF_reset;
input  wire            IF_clk;
output reg      [15:0] Tx_fifo_wdata;    // AK5394A A/D uses this to send its data to Tx FIFO
output reg             Tx_fifo_wreq;     // set when we want to send data to the Tx FIFO
input  wire            Tx_fifo_full;
input  wire [TFSZ-1:0] Tx_fifo_used;
output reg             Tx_fifo_clr;

input  wire            Tx_IQ_mic_rdy;
output wire            Tx_IQ_mic_ack;
output reg       [2:0] IF_chan; // which IF_mic_IQ_Data is needed
input  wire      [2:0] IF_max_chan;
input  wire     [63:0] Tx_IQ_mic_data;

input  wire            clean_dash;       // debounced dash
input  wire            clean_dot;        // debounced dot
input  wire            clean_PTT_in;     // debounced button

input  wire            ADC_OVERLOAD;

input  wire      [7:0] Penny_serialno;
input  wire      [7:0] Merc_serialno;
input  wire      [7:0] Ozy_serialno;

input  wire     [11:0] Penny_ALC;

// internal signals
reg       [3:0] AD_state;
reg       [3:0] AD_state_next;
reg       [5:0] AD_timer;

localparam  MAX_ADDR = 1; // can go from 0 to 31 if needed
reg       [4:0] tx_addr; // round robin address from 0 to MAX_ADDR
reg       [7:0] C1_DATA, C2_DATA, C3_DATA, C4_DATA;

localparam  AD_IDLE               = 0,
            AD_SEND_SYNC1         = 1,
            AD_SEND_SYNC2         = 2,
            AD_SEND_CTL1_2        = 3,
            AD_SEND_CTL3_4        = 4,
            AD_WAIT_MJ_RDY        = 5,
            AD_SEND_MJ1           = 6,
            AD_SEND_MJ2           = 7,
            AD_SEND_MJ3           = 8,
            AD_SEND_PJ            = 9,
            AD_WAIT               = 10,
            AD_LOOP_CHK           = 11,
            AD_PAD_CHK            = 12,
            AD_ERR                = 13;

reg [6:0] loop_cnt, num_loops;
reg [3:0] pad_cnt, pad_loops;

always @*
begin
  case (IF_max_chan)
    0: num_loops = 62; // 252 words / 4  = 63
    1: num_loops = 35; // 252 words / 7  = 36
    2: num_loops = 24; // 252 words / 10 = 25.2
    3: num_loops = 18; // 252 words / 13 = 19.38
    4: num_loops = 14; // 252 words / 16 = 15.75
    5: num_loops = 12; // 252 words / 19 = 13.26
    6: num_loops = 10; // 252 words / 22 = 11.45
    7: num_loops = 9;  // 252 words / 25 = 10.08 
    default: num_loops = 62;
  endcase
end

always @*
begin
  case (IF_max_chan)
    0: pad_loops = 0;  // 252 words / 4  = 63 remainder = 0
    1: pad_loops = 0;  // 252 words / 7  = 36 remainder = 0
    2: pad_loops = 2;  // 252 words / 10 = 25 remainder = 2 words
    3: pad_loops = 5;  // 252 words / 13 = 19 remainder 5 words
    4: pad_loops = 12; // 252 words / 16 = 15 remainder 12 words
    5: pad_loops = 5;  // 252 words / 19 = 13 remainder 5 words
    6: pad_loops = 10; // 252 words / 22 = 11 remainder 10 words
    7: pad_loops = 2;  // 252 words / 25 = 10 remainder 2 words
    default: pad_loops = 0;
  endcase
end

assign Tx_IQ_mic_ack = (AD_state == AD_WAIT);

always @ (posedge IF_clk)
begin
  if ((AD_state == AD_IDLE) || (AD_state == AD_SEND_SYNC1))
    loop_cnt  <= #IF_TPD 1'b0;
  else if (AD_state == AD_LOOP_CHK)
    loop_cnt  <= #IF_TPD loop_cnt + 1'b1; // at end of loop so increment loop counter

  if ((AD_state == AD_IDLE) || (AD_state == AD_SEND_SYNC1))
    pad_cnt  <= #IF_TPD 1'b0;
  else if (AD_state == AD_PAD_CHK)
    pad_cnt  <= #IF_TPD pad_cnt + 1'b1; // at end of loop so increment loop counter

  if (IF_reset)
    AD_state <= #IF_TPD AD_IDLE;
  else
    AD_state <= #IF_TPD AD_state_next;

  if (IF_reset)
    tx_addr <= #IF_TPD 1'b0;
  else if (AD_state == AD_SEND_CTL3_4) // toggle it for each frame
  begin
    if (tx_addr != MAX_ADDR)
      tx_addr <= #IF_TPD tx_addr + 1'b1;
    else
      tx_addr <= #IF_TPD 1'b0;
  end

  if (IF_reset)
    AD_timer <= #IF_TPD 0;
  else if (AD_state == AD_ERR)
    AD_timer <= #IF_TPD 0;
  else if (!AD_timer[5])
    AD_timer <= #IF_TPD AD_timer + 1'b1;

  Tx_fifo_clr <= #IF_TPD (AD_state == AD_ERR);

  if (IF_reset)
    IF_chan <= #IF_TPD 1'b0;
  else if (AD_state == AD_WAIT_MJ_RDY)
    IF_chan <= #IF_TPD 1'b0;
  else if (AD_state == AD_SEND_MJ3)
    IF_chan <= #IF_TPD IF_chan + 1'b1;
end

always @*
begin 
  case (tx_addr)
    0:
    begin
      C1_DATA = {7'b0,ADC_OVERLOAD};
      C2_DATA = Merc_serialno;
      C3_DATA = Penny_serialno;
      C4_DATA = Ozy_serialno;
    end

    1:
    begin
      C1_DATA = {4'b0,Penny_ALC[11:8]};
      C2_DATA = Penny_ALC[7:0];
      C3_DATA = 8'b0; // reserved for Bits 7-0 of Reverse Power from Alex
      C4_DATA = 8'b0; // reserved for Bits 7-0 of Reverse Power from Alex
    end
   
    default:
    begin
      C1_DATA = 8'b0;
      C2_DATA = 8'b0;
      C3_DATA = 8'b0; // reserved for Bits 7-0 of Reverse Power from Alex
      C4_DATA = 8'b0; // reserved for Bits 7-0 of Reverse Power from Alex
    end   
  endcase
end


always @*
begin 
  case (AD_state)
    AD_IDLE:
    begin
      Tx_fifo_wdata   = {16{1'bx}};
      Tx_fifo_wreq    = 1'b0;
      if (IF_reset || !AD_timer[5])
        AD_state_next = AD_IDLE;
      else // Tx_fifo can't immediately take data after reset
        AD_state_next = AD_SEND_SYNC1;
    end

    AD_SEND_SYNC1:
    begin
      Tx_fifo_wdata   = 16'h7F7F;
      Tx_fifo_wreq  = 1'b1;      // strobe sync (7F7F) into Tx FIFO
      if (Tx_fifo_full)          // Oops! buffer overflow!  Hate it when that happens...
        AD_state_next = AD_ERR;  // error handling will need to clear the fifo
      else
        AD_state_next = AD_SEND_SYNC2;
    end

    AD_SEND_SYNC2:
    begin  
      Tx_fifo_wdata   = {8'h7F, tx_addr, clean_dot, clean_dash, clean_PTT_in};
      Tx_fifo_wreq    = 1'b1;
      AD_state_next   = AD_SEND_CTL1_2;
    end

    AD_SEND_CTL1_2:
    begin
      Tx_fifo_wdata   = {C1_DATA, C2_DATA};
      Tx_fifo_wreq    = 1'b1;
      AD_state_next   = AD_SEND_CTL3_4;
    end

    AD_SEND_CTL3_4:
    begin 
      Tx_fifo_wdata   = {C3_DATA, C4_DATA};
      Tx_fifo_wreq    = 1'b1;
      AD_state_next   = AD_WAIT_MJ_RDY;
    end


    AD_WAIT_MJ_RDY:
    begin
      Tx_fifo_wdata   = {16{1'bx}};
      Tx_fifo_wreq    = 1'b0;
      if (!Tx_IQ_mic_rdy)
        AD_state_next = AD_WAIT_MJ_RDY;
      else
        AD_state_next = AD_SEND_MJ1;
    end
  
    AD_SEND_MJ1:
    begin
      Tx_fifo_wdata   = Tx_IQ_mic_data[63:48];
      Tx_fifo_wreq    = 1'b1;
      AD_state_next   = AD_SEND_MJ2;
    end

    AD_SEND_MJ2:
    begin
      Tx_fifo_wdata   = Tx_IQ_mic_data[47:32];
      Tx_fifo_wreq    = 1'b1;
      AD_state_next   = AD_SEND_MJ3;
    end

    AD_SEND_MJ3:
    begin 
      Tx_fifo_wdata   = Tx_IQ_mic_data[31:16]; // Mercury/Janus Right Lower data word
      Tx_fifo_wreq    = 1'b1;
      if (IF_chan != IF_max_chan)
        AD_state_next   = AD_SEND_MJ1;
      else
        AD_state_next   = AD_SEND_PJ;
    end

    AD_SEND_PJ:
    begin 
      Tx_fifo_wdata   = Tx_IQ_mic_data[15:0];
      Tx_fifo_wreq    = 1'b1;
      AD_state_next   = AD_WAIT;
    end

    AD_WAIT:
    begin 
      Tx_fifo_wdata   = {16{1'bx}};
      Tx_fifo_wreq    = 1'b0;
      if (Tx_IQ_mic_rdy)
        AD_state_next = AD_WAIT; // wait here till Tx_IQ_mic_rdy goes back low
      else
        AD_state_next = AD_LOOP_CHK;
    end

    AD_LOOP_CHK:
    begin 
      Tx_fifo_wdata   = {16{1'bx}};
      Tx_fifo_wreq    = 1'b0;
      if (loop_cnt != num_loops)
        AD_state_next = AD_WAIT_MJ_RDY;
      else
        AD_state_next = AD_PAD_CHK;
    end

    AD_PAD_CHK:
    begin 
      Tx_fifo_wdata   = 16'b0;
      Tx_fifo_wreq    = (pad_cnt != pad_loops);
      if (pad_cnt != pad_loops)
        AD_state_next = AD_PAD_CHK;
      else
        AD_state_next = AD_SEND_SYNC1;
    end

    AD_ERR:
    begin
      Tx_fifo_wdata   = {16{1'bx}};
      Tx_fifo_wreq    = 1'b0;
      AD_state_next   = AD_IDLE; // wait for TX FIFO to reset
    end

    default:
    begin
      Tx_fifo_wdata   = {16{1'bx}};
      Tx_fifo_wreq    = 1'b0;
      AD_state_next   = AD_IDLE;
    end
  endcase
end

function integer clogb2;
input [31:0] depth;
begin
  for(clogb2=0; depth>0; clogb2=clogb2+1)
  depth = depth >> 1;
end
endfunction

endmodule