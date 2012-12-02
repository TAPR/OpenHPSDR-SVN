// Created by Kirk Weedman KD7IRS - Jan 25, 2009
//
// This is a parameterized module
//
// SYS_LData and SYS_RData are all generated synchronously to SYS_clk.
// Since x_BCLK and x_LRCLK are not synchronous to SYS_clk they will be double buffered to
// to the SYS_clk domain
//
`timescale 1 ns/100 ps

module I2S_LR_Capture 
       (SYS_rst, SYS_clk, SYS_lrclk, SYS_Brise, SYS_Bfall, SYS_LData, SYS_RData,
        SYS_LData_rdy, SYS_RData_rdy, SYS_LRrise, SYS_LRfall, x_BCLK, x_LRCLK, x_Data);
parameter DS        = 16;            // size of left and right data
parameter BCNT      = 1;             // which SYS_b_clk, after SYS_lr_clk goes low, to start using
parameter DSTRB     = 0;             // which position in SYS_b_clk to grab data
// BCNT and DS can fine tune where we capture

localparam SS = clogb2(DS+BCNT+1);   // number of bits to hold range from 0 - ((DS+BCNT+1)-1)

input   wire          SYS_rst;       // reset
input   wire          SYS_clk;
output  wire          SYS_lrclk;
output  wire          SYS_Brise;
output  wire          SYS_Bfall;
output  reg  [DS-1:0] SYS_LData;     // buffer Left channel data
output  reg  [DS-1:0] SYS_RData;     // buffer Right channel data
output  reg           SYS_LData_rdy; // one SYS_clk wide pulse
output  reg           SYS_RData_rdy; // one SYS_clk wide pulse 
output  wire          SYS_LRrise;
output  wire          SYS_LRfall;
input   wire          x_BCLK;          // not in SYS_clk domain
input   wire          x_LRCLK;         // not in SYS_clk domain
input   wire          x_Data;          // data synchronous to x_BCLK/x_LRCLK

// internal signals
reg  [SS-1:0] SYS_shift_cnt;          // shift counter
reg  [DS-1:0] SYS_temp_data;          // holds DOUT
reg           SYS_b_clk, SYS_lr_clk;  // these are in the "SYS_clk" domain
reg           SYS_bc1, bc0;           // used in getting x_BCLK into "SYS_clk" domain
reg           SYS_lr1, lr0;           // used in getting x_LRCLK into "SYS_clk" domain
reg     [9:0] SYS_b_clk_cnt;          // how many SYS_clk's after rising edge of SYS_b_clk to grab x_Data
reg           SYS_d2, SYS_d1, d0;

localparam IF_TPD = 1;
localparam I2S_IDLE  = 0,
           I2S_LEFT  = 1,
           I2S_RIGHT = 2;

assign SYS_Brise  =  SYS_bc1 & !SYS_b_clk;
assign SYS_Bfall  = !SYS_bc1 &  SYS_b_clk;
assign SYS_LRrise =  SYS_lr1 & !SYS_lr_clk;
assign SYS_LRfall = !SYS_lr1 &  SYS_lr_clk;

assign SYS_lrclk = SYS_lr_clk;

always @ (posedge SYS_clk)
begin
  if (SYS_rst)
    SYS_b_clk_cnt <= #IF_TPD 0;
  else if (SYS_Brise)
    SYS_b_clk_cnt <= #IF_TPD 0; // rising edge - reset position
  else
    SYS_b_clk_cnt <= #IF_TPD SYS_b_clk_cnt + 1'b1; // 0, 1, ...

  if (SYS_b_clk_cnt == DSTRB) // DSTRB should be small enough so this happens once every x_BCLK cycle
    SYS_temp_data[DS-1:0] <= {SYS_temp_data[DS-2:0], SYS_d2};

  {SYS_d2,     SYS_d1,  d0}  <= #IF_TPD {SYS_d1,  d0,  x_Data};
  {SYS_b_clk,  SYS_bc1, bc0} <= #IF_TPD {SYS_bc1, bc0, x_BCLK};
  {SYS_lr_clk, SYS_lr1, lr0} <= #IF_TPD {SYS_lr1, lr0, x_LRCLK};

  if (SYS_rst)
    SYS_shift_cnt <= #IF_TPD 0;
  else if (SYS_LRfall || SYS_LRrise)
    SYS_shift_cnt <= #IF_TPD 0;
  else if (SYS_shift_cnt != {SS{1'b1}}) // wait here so we dont accidentally reload SYS_LData & SYS_RData
  begin
    if (SYS_Brise)
      SYS_shift_cnt <= #IF_TPD SYS_shift_cnt + 1'b1;
  end

  if ((SYS_shift_cnt == (DS+BCNT)) && (SYS_b_clk_cnt == DSTRB) && !SYS_lr_clk)
  begin
    SYS_LData_rdy <= 1'b1;
    SYS_LData     <= #IF_TPD SYS_temp_data;
  end
  else
    SYS_LData_rdy <= 1'b0;

  if ((SYS_shift_cnt == (DS+BCNT)) && (SYS_b_clk_cnt == DSTRB) && SYS_lr_clk)
  begin
    SYS_RData_rdy <= 1'b1;
    SYS_RData     <= #IF_TPD SYS_temp_data;
  end
  else
    SYS_RData_rdy <= 1'b0;
end

function integer clogb2;
input [31:0] depth;
begin
  for(clogb2=0; depth>0; clogb2=clogb2+1)
  depth = depth >> 1;
end
endfunction

endmodule