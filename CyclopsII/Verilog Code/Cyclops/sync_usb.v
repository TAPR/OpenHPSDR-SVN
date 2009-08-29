//////////////////////////////////////////////////////////////
//
//      State Machine to manage FX2 USB interface
//
//////////////////////////////////////////////////////////////
/*
        The state machine checks if there is any data from the PC (in the FX2's USB
        FIFO) to be read by checking FLAGA.  If this flag is set it assumes there's
        at least 512 bytes of info in the USB FIFO and transfers this amount to the
        RX_FIFO. If the flag is not set it then checks to see if FLAGC is set and if
        there's at least 512 bytes worth of data in the TX FIFO to send to the USBs
        FIFO. If so, it transfers the data.  Then this whole process is repeated
        forever.
*/

module sync_usb(IFCLK, IF_reset, FX2_FD, FLAGA, FLAGC, SLWR, SLRD, SLOE, PKEND, FIFO_ADR,
                Rx_fifo_wdata, Rx_fifo_drdy, Rx_fifo_used, Rx_fifo_full,
                Tx_fifo_rreq, Tx_fifo_full, Tx_fifo_used, Tx_fifo_rdata,Tx_fifo_empty);
parameter IF_TPD  = 3;
parameter RX_FIFO_SZ  = 4096; // 16 by 4096 deep RX FIFO
parameter RX_BURST_SZ = 512;  // number of 16 bit words to transfer
parameter TX_FIFO_SZ  = 2048; // 16 by 2048 deep TX FIFO
parameter TX_BURST_SZ = 512;

localparam RFSZ = clogb2(RX_FIFO_SZ-1);  // number of bits needed to hold 0 - (RX_FIFO_SZ-1)
localparam RBSZ = clogb2(RX_BURST_SZ-1);
localparam TFSZ = clogb2(TX_FIFO_SZ-1);  // number of bits needed to hold 0 - (TX_FIFO_SZ-1)
localparam TBSZ = clogb2(TX_BURST_SZ-1);
localparam BSZ  = (RBSZ < TBSZ) ? TBSZ : RBSZ;

input  wire             IFCLK;
input  wire             IF_reset;
inout  wire      [15:0] FX2_FD;         // bidirectional FIFO data to/from the FX2                                    // ***** use this so simulation works

input  wire             FLAGA;
input  wire             FLAGC;

output reg              SLWR;           // FX2 write - active low
output reg              SLRD;           // FX2 read - active low
output reg              SLOE;           // FX2 data bus enable - active low
output wire             PKEND;
output reg        [1:0] FIFO_ADR;       // FX2 register address

output reg       [15:0] Rx_fifo_wdata;
output reg              Rx_fifo_drdy;   // flag for SYNC_state to know when Rx_fifo_wdata is valid
input  wire  [RFSZ-1:0] Rx_fifo_used;   // number of 16bit words used in RX FIFO
input  wire             Rx_fifo_full;

output reg              Tx_fifo_rreq;
input  wire             Tx_fifo_full;
input  wire  [TFSZ-1:0] Tx_fifo_used;
input  wire      [15:0] Tx_fifo_rdata;   // number of 16bit words used in RX FIFO
input  wire             Tx_fifo_empty;

//reg    [BSZ-1:0] FX_Burst_cnt; // use this when all is working
reg [16:0] FX_Burst_cnt; // keep fixed size while using SignalTap
reg        [5:0] FX_state;         // state for FX2
reg        [5:0] FX_state_next;    // next state for FX2
reg              SLEN;             // Put data on FX2 bus
wire             to_pc_rdy;
wire             from_pc_rdy;
//reg              tx_start;
//reg   [TFSZ-1:0] tx_min;

localparam EP2_ADR = 2'b00;
localparam EP6_ADR = 2'b10;

localparam FX_IDLE          = 0,
           FX_FROM_PC_ADDR  = 1,
           FX_FROM_PC_CHECK = 2,
           FX_TO_PC_ADDR    = 3,
           FX_TO_PC_CHECK   = 4,
           FX_F1            = 5,
           FX_F2            = 6,
           FX_F3            = 7,
           FX_F4            = 8,
           FX_F5            = 9,
           FX_FDONE         = 10,
           FX_T1            = 11,
           FX_T2            = 12,
           FX_T3            = 13,
           FX_T4            = 14,
           FX_T5            = 15,
           FX_TDONE         = 16;

localparam SLWR_ASSERT = 1'b0;
localparam SLWR_NEGATE = 1'b1;

localparam SLRD_ASSERT = 1'b0;
localparam SLRD_NEGATE = 1'b1;

localparam SLOE_ASSERT = 1'b0;
localparam SLOE_NEGATE = 1'b1;

localparam SLEN_ASSERT = 1'b1;
localparam SLEN_NEGATE = 1'b0;

// FLAGC high and at least TBSZ words (TBSZ*2 bytes) in buffer
assign #IF_TPD to_pc_rdy = FLAGC && ({Tx_fifo_full,Tx_fifo_used} >= TX_BURST_SZ);
//assign #IF_TPD to_pc_rdy = FLAGC && !Tx_fifo_empty;
//assign #IF_TPD to_pc_rdy = FLAGC && ({Tx_fifo_full,Tx_fifo_used} > tx_min);

assign #IF_TPD from_pc_rdy = FLAGA && ({Rx_fifo_full,Rx_fifo_used} < (252*6));

// if EP2 is ready and RX FIFO is low then get more data

assign PKEND = 1'b1;

/*
reg sloe, slen, slwr, slrd;
always @ (negedge IFCLK)
begin
  SLOE <= sloe;
  SLEN <= slen;
  SLRD <= slrd;
  SLWR <= slwr;
end
*/

always @ (posedge IFCLK)
begin
/*
  if (IF_reset)
    tx_start <= #IF_TPD 1'b1;
  else if ({Tx_fifo_full,Tx_fifo_used} > tx_min)
    tx_start <= #IF_TPD 1'b0;

  if (tx_start)
    tx_min <= #IF_TPD 2048; // inital transfer starting point
  else
    tx_min <= #IF_TPD TX_BURST_SZ;
*/
  if (IF_reset)
    FX_state <= #IF_TPD FX_IDLE;
  else
    FX_state <= #IF_TPD FX_state_next;

  if (IF_reset)
    SLOE <= #IF_TPD SLOE_NEGATE;
  else if ((FX_state == FX_F1) || (FX_state == FX_F2) || (FX_state == FX_F3))
    SLOE <= #IF_TPD SLOE_ASSERT; // asserted during FX_F3, FX_F4 and FX_F5 states
  else
    SLOE <= #IF_TPD SLOE_NEGATE;
  
  if (IF_reset)
    SLRD <= #IF_TPD SLRD_NEGATE;
  else if (FX_state == FX_F3)
    SLRD <= #IF_TPD SLRD_ASSERT; // asserted during FX_F4
  else
    SLRD <= #IF_TPD SLRD_NEGATE;

  if (IF_reset)
    SLWR <= #IF_TPD SLWR_NEGATE;
  else if (FX_state == FX_T3)
    SLWR <= #IF_TPD SLWR_ASSERT; // asserted during FX_T4
  else
    SLWR <= #IF_TPD SLWR_NEGATE;

  if (IF_reset)
    SLEN <= #IF_TPD SLEN_NEGATE;
  else if ((FX_state == FX_T2) || (FX_state == FX_T3) || (FX_state == FX_T4))
    SLEN <= #IF_TPD SLEN_ASSERT; // asserted during FX_T3, FX_T4, FX_T5 state
  else
    SLEN <= #IF_TPD SLEN_NEGATE;

  if (IF_reset)
    Rx_fifo_drdy <= #IF_TPD 1'b0;
  else
    Rx_fifo_drdy <= #IF_TPD (FX_state == FX_F4) ? 1'b1 : 1'b0;

  if (IF_reset)
    Rx_fifo_wdata <= #IF_TPD 16'b0;
  else if (FX_state == FX_F3)  // data from PC now available since tXFD = 15nS max
    Rx_fifo_wdata <= #IF_TPD {FX2_FD[7:0],FX2_FD[15:8]}; // swap endian

  if (IF_reset)
    Tx_fifo_rreq <= #IF_TPD 1'b0;
  else // make next Tx_fifo_rdata appear?
    Tx_fifo_rreq <= #IF_TPD (FX_state == FX_T1) ? 1'b1 : 1'b0; // must read first to get data out of FIFO!

  if ((FX_state == FX_IDLE) || (FX_state == FX_TDONE)) // FX_IDLE or FX_TDONE -> FX_FROM_PC_ADDR
    FIFO_ADR <= #IF_TPD EP2_ADR; // Must be setup 10.7nS min before checking FLAGS
  else if ((FX_state == FX_FDONE))// FX_FXDONE - > FX_TO_PC_ADDR
    FIFO_ADR <= #IF_TPD EP6_ADR; // Must be setup 10.7nS min before checking FLAGS

  if ((FX_state == FX_IDLE) || (FX_state == FX_TDONE) || (FX_state == FX_FDONE))
    FX_Burst_cnt <= #IF_TPD 1'b0;
  else if ((FX_state == FX_T5) || (FX_state == FX_F5))
    FX_Burst_cnt <= #IF_TPD FX_Burst_cnt + 1'b1;
end

always @*
begin
  case (FX_state)
    FX_IDLE:
    begin
      FX_state_next = FX_FROM_PC_ADDR;
    end

    FX_FROM_PC_ADDR: // cannot check "from_pc_rdy" during this state
      FX_state_next = FX_FROM_PC_CHECK;

    FX_FROM_PC_CHECK:                 // check for data from PC
    begin
      if (from_pc_rdy)
        FX_state_next = FX_F1;
      else
        FX_state_next = FX_FDONE;  // No data from PC so check for data ready to go to PC
    end


/* Synchronous reads from CY7C68013A
   FIFOADR[1:0]-+--------------------------------------+------------
               -+--------------------------------------+--------------
                                  ->| |<-- tRDH
   IFCLK --+    +----+    +----+    +----+    +----+    +----+ 
           +----+    +----+    +----+    +----+    +----+    +-
                     tSRD -->|      |<--
   SLRD ---------------------+        +-----------------------
                             +--------+                       
                                    |<- tXFLG ->|
   Flags ---------------------------------------+------------
          --------------------------------------+--------------
                                 -->|   |<--tXFD
                          +-------------+---------------------+
   Data  -----------------+      N      +    N+1              +----------------
                          +-------------+---------------------+
                     <-A->                               <-B->
   SLOE  ------------+                                   +-------------------
                     +-----------------------------------+
                                 
         tCLK   = 20.83nS minimum -> 1 clocks at 48Mhz
         tXFD   = 11nS max
         tXFLG  = 9.5nS max
         tRDH   =  0nS min
         tSRD   = 18.7nS min
         A = tOEon  = 10.5nS max
         B = tOEoff = 10.5nS max
                    
*/
    FX_F1:
      FX_state_next = FX_F2;

    FX_F2:
      FX_state_next = FX_F3;

    FX_F3:
      FX_state_next = FX_F4;

    FX_F4:
      FX_state_next = FX_F5;

    FX_F5:
    begin
      if (FX_Burst_cnt != (RX_BURST_SZ-1)) // RX_BURST_SZ words = RX_BURST_SZ*2 bytes
//      if (FLAGA && !Rx_fifo_full) // While EP2 has data - normally 512 bytes = 256 words, keep getting it
        FX_state_next = FX_F1;
      else
        FX_state_next = FX_FDONE;
    end

    FX_FDONE:
      FX_state_next = FX_TO_PC_ADDR;



    FX_TO_PC_ADDR: // cannot check "to_pc_rdy" during this state
      FX_state_next = FX_TO_PC_CHECK;

    FX_TO_PC_CHECK: // check for Tx data - Tx fifo must be at least half full before we Tx
    begin
      if (to_pc_rdy)
        FX_state_next = FX_T1;
      else
        FX_state_next = FX_TDONE; // No Tx data so check for Rx data again
    end

/* Async writes to CY7C68013A
*/
    FX_T1:
      FX_state_next = FX_T2;

    FX_T2:
      FX_state_next = FX_T3;

    FX_T3:
      FX_state_next = FX_T4;

    FX_T4:
      FX_state_next = FX_T5;

    FX_T5:
    begin
      if (FX_Burst_cnt != (TX_BURST_SZ-1)) // TX_BURST_SZ words = TX_BURST_SZ*2 bytes
//      if (FLAGC && (FX_Burst_cnt != (TX_BURST_SZ-1))) // TX_BURST_SZ words = TX_BURST_SZ*2 bytes
        FX_state_next = FX_T1;
      else
        FX_state_next = FX_TDONE;
    end

    FX_TDONE:
      FX_state_next = FX_FROM_PC_ADDR;

    default:
      FX_state_next = FX_IDLE;
  endcase
end

// FX2_FD is tristate when SLEN is negated, otherwise it's the Tx_fifo_rdata value.
// Swap endian so data is correct at PC end

assign FX2_FD = SLEN ? {Tx_fifo_rdata[7:0], Tx_fifo_rdata[15:8]}  : {16{1'bz}};

function integer clogb2;
input [31:0] depth;
begin
  for(clogb2=0; depth>0; clogb2=clogb2+1)
  depth = depth >> 1;
end
endfunction

endmodule