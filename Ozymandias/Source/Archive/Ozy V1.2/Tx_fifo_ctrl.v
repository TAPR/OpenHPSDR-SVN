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

        Each frame is 512 bytes long = 8 initial bytes + NUM_LOOPS*8 - where NUM_LOOPS = 63

        If PTT_in, dot or dash inputs are acitve they are sent in C&C
*/

`timescale 1 ns/100 ps

module Tx_fifo_ctrl(IF_reset, IFCLK, Tx_fifo_wdata, Tx_fifo_wreq, Tx_fifo_full, Tx_fifo_used,
                    Tx_fifo_clr, Rx_fifo_used, Rx_fifo_full, Tx_IQ_mic_rdy, Tx_IQ_mic_data,
                    clean_dash, clean_dot, clean_PTT_in, ADC_OVERLOAD,
                    Penny_serialno, Merc_serialno, Ozy_serialno, Penny_ALC);
parameter RX_FIFO_SZ = 2048;
parameter TX_FIFO_SZ = 4096;
parameter IF_TPD = 1;

localparam RFSZ = clogb2(RX_FIFO_SZ-1);  // number of bits needed to hold 0 - (RX_FIFO_SZ-1)
localparam TFSZ = clogb2(TX_FIFO_SZ-1);  // number of bits needed to hold 0 - (TX_FIFO_SZ-1)

input  wire            IF_reset;
input  wire            IFCLK;
output reg      [15:0] Tx_fifo_wdata;    // AK5394A A/D uses this to send its data to Tx FIFO
output reg             Tx_fifo_wreq;     // set when we want to send data to the Tx FIFO
input  wire            Tx_fifo_full;
input  wire [TFSZ-1:0] Tx_fifo_used;
output reg             Tx_fifo_clr;

input  wire [RFSZ-1:0] Rx_fifo_used;    // read side count
input  wire            Rx_fifo_full;

input  wire            Tx_IQ_mic_rdy;
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
reg       [5:0] loop_counter;     // counts number of times round loop

reg       [3:0] AD_state;
reg       [3:0] AD_state_next;
reg       [5:0] AD_timer;

reg             tx_flag;

localparam  AD_IDLE               = 0,
            AD_SEND_SYNC1         = 1,
            AD_SEND_SYNC2         = 2,
            AD_SEND_CTL1_2        = 3,
            AD_SEND_CTL3_4        = 4,
            AD_SEND_MJ1           = 5,
            AD_SEND_MJ2           = 6,
            AD_SEND_MJ3           = 7,
            AD_SEND_PJ            = 8,
            AD_WAIT               = 9,
            AD_LOOP_CHK           = 10,
            AD_ERR                = 11;

localparam  NUM_LOOPS = 63; // number of loops that will send 8 bytes of info to fifo
                            // total data = 8 initial bytes + NUM_LOOPS*8

always @ (posedge IFCLK)
begin
  if ((AD_state == AD_IDLE) || (AD_state == AD_SEND_SYNC1))
    loop_counter  <= #IF_TPD 1'b0;
  else if (AD_state == AD_LOOP_CHK)
    loop_counter  <= #IF_TPD loop_counter + 1'b1; // at end of loop so increment loop counter

  if (IF_reset)
    AD_state <= #IF_TPD AD_IDLE;
  else
    AD_state <= #IF_TPD AD_state_next;

  if (IF_reset)
    tx_flag <= #IF_TPD 1'b0;
  else if (AD_state == AD_SEND_CTL3_4) // toggle it for each frame
    tx_flag <= #IF_TPD ~tx_flag;

  if (IF_reset)
    AD_timer <= #IF_TPD 0;
  else if (AD_state == AD_ERR)
    AD_timer <= #IF_TPD 0;
  else if (!AD_timer[5])
    AD_timer <= #IF_TPD AD_timer + 1'b1;

  Tx_fifo_clr <= #IF_TPD (AD_state == AD_ERR);
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
      Tx_fifo_wdata   = {8'h7F, 5'b0, tx_flag, clean_dash, (clean_dot || clean_PTT_in)};
      Tx_fifo_wreq    = 1'b1;
      AD_state_next   = AD_SEND_CTL1_2;
    end

    AD_SEND_CTL1_2:
    begin
      if (tx_flag)
        Tx_fifo_wdata   = {4'b0,Penny_ALC}; // TX_CONTROL_1, TX_CONTROL_2
      else
        Tx_fifo_wdata   = {7'b0,ADC_OVERLOAD,Merc_serialno}; // TX_CONTROL_1, TX_CONTROL_2
      Tx_fifo_wreq    = 1'b1;
      AD_state_next   = AD_SEND_CTL3_4;
    end

    AD_SEND_CTL3_4:
    begin 
      Tx_fifo_wdata   = {Penny_serialno,Ozy_serialno}; // TX_CONTROL_3, TX_CONTROL_4
      Tx_fifo_wreq    = 1'b1;
      AD_state_next   = AD_SEND_MJ1;
    end


    AD_SEND_MJ1:
    begin
      Tx_fifo_wdata   = Tx_IQ_mic_data[63:48];
      Tx_fifo_wreq    = Tx_IQ_mic_rdy;
      if (!Tx_IQ_mic_rdy)
        AD_state_next = AD_SEND_MJ1;
      else
        AD_state_next = AD_SEND_MJ2;
    end

    AD_SEND_MJ2:
    begin
      Tx_fifo_wdata   = Tx_IQ_mic_data[47:32];
      Tx_fifo_wreq    = 1'b1;
      AD_state_next   = AD_SEND_MJ3;
    end

    AD_SEND_MJ3:
    begin 
      Tx_fifo_wdata   = Tx_IQ_mic_data[31:16];       // Mercury/Janus Right Lower data word
      Tx_fifo_wreq    = 1'b1;
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
      if (loop_counter != (NUM_LOOPS-1))
        AD_state_next = AD_SEND_MJ1;
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