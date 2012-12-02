////////////////////////////////////////////////////////////////////
////                                                             ////
//// Copyright (C) 2009 Steven Wilson                            ////
////                    ka6s@yahoo.com                           ////
//
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published
//  by the Free Software Foundation; version 1
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
// This design is LGPL software written for the HPSDR project
// by Steve Wilson, KA6S.  The author assumes no libability on how the
// code might be used or warranty that the code is fit for any
// particular application. 
//
// This code is a simple BFM modeling the behavior of the CY7C68013 USB
// FIFO interface.  
//   
// History: 1/11/09 First release 
//          1/11/09 Changed call to RD_PACKET, and add USB_RD task
//
/////////////////////////////////////////////////////////////////////////////
//
// Comments below are copied from Ozy_Janus.v
// Fifo Data Format:
// A flag of 0x7F7F7F is sent prior to 5 bytes of control data then 3 bytes of
// left data, 3 bytes of right data and 2 bytes of mic/line data.  The  data 
// sequence is then repeated..
//
// Every 512 bytes the sync and control data is is repeated.
//
// The format for data TO the PC is:-
//
//  <0x7F7F><0x7F,C0><C1,C2><C3,C4><Left><Left LSB,Right MSB><Right LSW><Mic data >... etc
//
// where Cn is a control byte - see protocol design document for full description.
//
// C0 obxxxx_xx00 PTT,dot and dash inactive
// C0 0bxxxx_xx01 PTT or dot key  activated
// C0 0bxxxx_xx10 dash key activated
//
// where x = don't care
// etc....see protocol design document for full C&C format.

//               Left                             Right
//0-------------------------------------------------------------------------63
//0                 16       24                          35         51 52    BCLK counter (AD_state)
//<   Left data     ><  LSB  >       <   Right data      ><   LSB   >       - AK5394A
//<Mic MSB ><Mic LSB>                                                       - TLV320


// Sync and control bytes are sent as follows:

// 0            2         4        5      6     BCLK counter (AD_state)
//      <0x7F7F>  <0x7F,C0> <C1,C2> <C3,C4>
//
//
// The format for data FROM the PC is the same sync & control sequence followed by 48k/16 bit data:-
//
//   <0x7F7F><0x7F,C0><C1,C2><C3,C4>< Left data >< Right data >< I data >< Q data > etc...
//
// Control bytes are as follows:
//
// C0 = 0bxxxx_xxx0   MOX in PowerSDR inactive
// C0 = 0bxxxx_xxx1   MOX active
//
// AK5394A speed is set with the following command FROM the PC:
//
// When C0 = 0b0000_000x
// then
//      C1 = 0bxxxx_xx00 speed is 48k (default)
//      C1 = 0bxxxx_xx01 speed is 96k
//      C1 = 0bxxxx_xx10 speed is 192k
//
// where x = don't care
// etc....see protocol design document for full C&C format.
//
//
// A/D data is in 2's complement format.
// AK5394A is set to be 192/96/48kHz, 24 bit in I2S mode.
//
// TLV320 is set via I2C to be 48kHz, 16 bit and I2S mode as follows:
//
// TLV320  - 1A 1E 00 - Reset chip

// Janus ADC = 24 bits per sample
// Line ADC = 16 bits per sample

`timescale 1ns/1ps  

module cy7c68013_usb (
        ifclk,
        flaga,
        flagb,
        flagc,
        data,
        sloe_n,
        slrd_n,
        slwr_n
);
input  ifclk; // Interface clock
output flaga; // Read Interace has data
output flagb;
output flagc; // Write interface can accept data

inout [15:0] data;

input sloe_n; // Output enable
input slrd_n; // Read Strobe
input slwr_n; // Write Strobe 

reg [7:0] wmem [0:511]; // Memory for Write task 
reg [7:0] rmem [0:511]; // Memory for Read Task 
reg       flaga;        // Flag indicating Receive data present
reg  [15:0] data_out;
integer   debug;
reg       drive_out;
time      wr_fall_time;
time      wr_rise_time;
time      data_change;
time      data_change_d;
reg       saw_rise_edge;
reg       flagc;
reg       enable;

// times are in nanos
`define TRDpwi 50
`define TRDpwh 50
`define Txflg 70
`define Txfd 15
`define TOEon 10.5
`define TOEoff 10.5
`define TWRpwi   50
`define TSFD   10
`define TFDH   10
`define TXFD   70
`define TWRpwh 70

assign data = drive_out ? data_out : 16'bz;

//
// RD_PACKET - Send 512 bytes of data to FPGA
//    c0 - MOX control - 0=inactive, 1= active
//    c1 - Speed ctl - 00=48k, 01=96k,02=192K 
//    c2 - ?
//    c3 - ?
//    c4 - ?
//    left_seed - Initial value of packet left channel
//    left_incr - Amount to increment Left channel by
//    right_seed - Initial value of packet left channel
//    right_incr - Amount to increment right channel by
//    mic_seed - Initial value of mic left channel
//    mic_incr - Amount to increment mic channel by

task RD_PACKET;
input [7:0] c0;  // MOX control - 0=inactive, 1= active
input [7:0] c1;  // Speed ctl - 00=48k, 01=96k,02=192K
input [7:0] c2;  // ?
input [7:0] c3;  // ?
input [7:0] c4;  // ?
input [23:0] left_seed; // Initial value of left channel
input [23:0] left_incr; // Amount to increment Left channel
input [23:0] right_seed;// Intiial value of right channel
input [23:0] right_incr;// Amount to increment Right channel
input [15:0] mic_seed;  // Initial value of Mic channel
input [15:0] mic_incr;  // Amount to increment Mic channel

// Define local variables
reg  [23:0] left_sum;
reg  [23:0] right_sum;
reg  [15:0] mic_sum;

integer i;

begin
  // Load the synch bytes
  $display("RD_PACKET : Entering task");
  data_out = 16'bz; // This way we keep driving z till TOEon expires
  rmem[0] = 8'h7f;
  rmem[1] = 8'h7f;
  rmem[2] = 8'h7f;
  // Now load the control bytes 
  rmem[3] = c0;
  rmem[4] = c1;
  rmem[5] = c2;
  rmem[6] = c3;
  rmem[7] = c4;

  // Note Order of packet is 23-16 left sample
  //                         15-8  Left sample
  //                         7-0   Left sample
  //                         23-16 Right sample
  //                         15-8  Right sample
  //                         7-0   Right sample
  //                         15-8  Mic
  //                         7-0   Mic
  left_sum = left_seed;
  right_sum = right_seed;
  mic_sum = mic_seed;

  $display("RD_PACKET : Initializing read packet memory");
  for(i=8; i<512; i=i+8) 
   begin 
     rmem[i    ] = left_sum[23:16]; 
     rmem[i + 1] = left_sum[15:8]; 
     rmem[i + 2] = left_sum[7:0]; 
     rmem[i + 3] = right_sum[23:16]; 
     rmem[i + 4] = right_sum[15:8]; 
     rmem[i + 5] = right_sum[7:0]; 
     rmem[i + 6] = mic_sum[15:8];
     rmem[i + 7] = mic_sum[7:0];

     left_sum = left_sum + left_seed;
     right_sum = right_sum + right_seed;
     mic_sum = mic_sum + mic_seed;
   end 


   // Assert FLAG
   @(posedge ifclk)
   #`Txflg;  // Can't prove this is required ....
   $display("RD_PACKET : Asserting FLAGA");
   flaga = 1;  

   // Send FIFO data
   fork  // Fork here to deal with SLOE_N on every cycle and driving first data out
     begin
       debug = 55;
       @(negedge sloe_n);
       $display("RD_PACKET : SLOE_N seen");
       #(`TOEon);
       debug = 56;
       data_out = {rmem[1], rmem[i]};
     end
      
     for(i = 0; i < 511; i=i+2)
       USB_RD ({rmem[i+1], rmem[i]}, 1'b0, i );
   join

   $display("RD_PACKET : Leaving task");
   data_out = 16'bz; 
end


endtask

//
// USB_RD:  transfer a single word from USB - used by RD_PACKET
//    data - 16 bit transfer amount
//    flag - Use flag handshake for single transfer when=1
//    count - Count amount reported when used by RD_PACKET
//
task USB_RD;
input [15:0] data;     // 16 bit data
input        flag;     // Assert Flag for each transaction
input [31:0] count;    // printout count if FLAG==0
  begin : usb_rd_begin
    debug = 0;

    if(flag) // This doesn't occur in RD_PACKET
      begin
         @(posedge ifclk)
         #`Txflg;  // Can't prove this is required ....
         flaga = 1;  
      end

    debug = 1;
    // Since sloe_n is going active at the same time as slrd_n 
    // in the current code - we need to not look for slrd_n
    // the first time..

    if((count != 0) & ~flag | flag)  
      begin
        @(negedge slrd_n); 
        debug = 2;
      end
    if(sloe_n !== 1'b0)
       $display("USB_RD: ERROR - SLOE_N not set when SLRD_N active");

    fork // Move the data
      begin  // Send the data
        #`Txfd;
        data_out = data;
        debug = 4;
        if(~flag)
          $display("USB_RD: sending cnt=%d data=%h",count,data_out);
        else
          $display("USB_RD: single cycle data=%h",count,data_out);
      end
      begin // If the last word - turn off flag
        if(flag | ~flag & (count == 510)) 
          begin
            #`Txflg;
            flaga = 1'b0;
          end
      end
      @(posedge slrd_n);
    join
    debug = 7;
  end // usb_rd_begin
endtask

//
// These two blocks handle SLOEN active/inactive delays
//
always @(negedge sloe_n)
  begin
    #(`TOEon);
    drive_out = 1'b1;
  end

always @(posedge sloe_n)
  begin
    #(`TOEoff)
    drive_out = 1'b0;
  end


always @(negedge slwr_n)
   wr_fall_time = $realtime;

reg [15:0] debug_reg;
always @(posedge slwr_n)
    wr_rise_time = $realtime; 

// Sample data
always @(negedge slwr_n)
  begin
    if((wr_rise_time - wr_fall_time) < `TWRpwi)
     begin
       $display("TX_USB: ERROR - TWRpwi violated at %t by %t",
                  $realtime, wr_rise_time - wr_fall_time);
       end
    #(`TWRpwi-1); // sample at Write pulse width...
    sim.TX_FIFO_DATA <= {data[7:0], data[15:8]};
    debug_reg <= {data[7:0], data[15:8]};
  end
    

// Record every time the data change
always @(data)
  begin
    data_change_d = data_change;
    data_change = $realtime;
  end

// Measure TFDH
always @(posedge slwr_n)
  begin
    #(`TFDH+1);
        if(((data_change  - wr_rise_time) < `TFDH) & (data_change !== 0))
           $display("TX_USB: ERROR - TFDH violated at %t actual hold time %t", $realtime,  data_change-wr_rise_time);
  end

task TX_USB;
begin
  // Assert FLAGC
  enable = 1'b1;
  @(posedge ifclk);
  flagc = 1'b1;
  @(negedge slwr_n);
  saw_rise_edge = 1'b1;
  fork
     begin // Wait for slwr_n
       @(posedge slwr_n)
       if(($realtime - data_change) < `TSFD )
         $display("TX_USB: ERROR - TSFD violated at %t",$realtime);
     end
     begin
        #(`TXFD); // Deassert flagc
        flagc = 1'b0;
     end
  join
  // Return results data
end
endtask

initial begin
          flaga = 0;
          flagc = 0;
          drive_out = 0;
          data_out = 16'bz;
          data_change  = $realtime;
          data_change_d = $realtime;
          saw_rise_edge = 0;
          enable = 0;
        end


endmodule
