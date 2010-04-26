
//------------------------------------------------------------------------------
//           Copyright (c) 2009 Kirk Weedman KD7IRS
//------------------------------------------------------------------------------



module ramcic( rst, clk, in_strobe, out_strobe, in_data, out_data );

//design parameters
parameter STAGES = 19;
parameter DECIMATION = 4;  
parameter IN_WIDTH = 28;

//computed parameters
//parameter ACC_WIDTH = IN_WIDTH + STAGES * Ceil(Log2(DECIMATION));
parameter ACC_WIDTH = IN_WIDTH + STAGES * clogb2(DECIMATION-1);
parameter OUT_WIDTH = 24;

input rst;
input clk;
input in_strobe;
output reg out_strobe;
input wire signed [IN_WIDTH-1:0] in_data;
output reg signed [OUT_WIDTH-1:0] out_data;

//------------------------------------------------------------------------------
//                               internal variables
//------------------------------------------------------------------------------

localparam SS = 2; // log2(DECIMATION)
reg       [SS-1:0] sample_no;
reg  signed [ACC_WIDTH-1:0] wr_data;
reg  signed [ACC_WIDTH-1:0] rd_data;
reg  signed [ACC_WIDTH-1:0] sum;
reg                out_strobe_flag;
reg                wr;
reg          [4:0] rcnt, wcnt; // make these a variable size based on STAGES
reg                r_ic, w_ic;
reg         [ACC_WIDTH-1:0] mem [0:63]; // this should cause two M9K's to be used
// NOTE: mem is divided into 2 sections for use.
// section: 1'b1 = comb previous data
//          1'b0 = integrator in or out data
reg          [2:0] cic_state, cic_next;

localparam CIC_START   = 4'd0,
           INT_START   = 4'd1,
           INT_S2      = 4'd2,
           INT         = 4'd3,
           COMB_START  = 4'd4,
           COMB_S2     = 4'd5,
           COMB        = 4'd6,
           COMB2       = 4'd7;
//------------------------------------------------------------------------------
//                               control
//------------------------------------------------------------------------------


always @(posedge clk)
begin
  if (rst)
    sample_no <= 1'b0;
  else if (in_strobe)
  begin
    if (sample_no == (DECIMATION-1))
      sample_no <= 1'b0;
    else
      sample_no <= sample_no + 1'b1;
  end

  if (rst)
    out_strobe <= 1'b0;
  else 
    out_strobe <= (cic_state == COMB_START);

  if (rst)
    out_strobe_flag <= 1'b0;
  else if (in_strobe && (sample_no == (DECIMATION-1)))
    out_strobe_flag <= 1'b1;
  else if (cic_state == COMB_START)
    out_strobe_flag <= 1'b0;
end

//------------------------------------------------------------------------------
//                 Integrator/comb stages stored in RAM
//------------------------------------------------------------------------------


always @(posedge clk)
begin
  if (rst)
    cic_state <= CIC_START;
  else
    cic_state <= cic_next;

  if ((cic_state == CIC_START) || (cic_state == COMB_START))
    rcnt <= 1'b0;
  else if ((cic_state == INT_START) || (cic_state == INT_S2) ||
           (cic_state == INT) || (cic_state == COMB_S2) || (cic_state == COMB))
    rcnt <= rcnt + 1'b1;

  if (cic_state == CIC_START)
    r_ic <= 1'b0;
  else if (cic_state == COMB_START)
    r_ic <= 1'b1;

  if (rst)
    rd_data <= 1'b0;
  else
    rd_data <= mem[{r_ic,rcnt}];

  if ((cic_state == COMB_S2) || (cic_state == INT_S2))
    wcnt <= 1'b0;
  else if ((cic_state == COMB) || (cic_state == INT))
    wcnt <= wcnt + 1'b1;

  if (cic_state == CIC_START)
    w_ic <= 1'b0;
  else if (cic_state == COMB_START)
    w_ic <= 1'b1;

  if ((cic_state == INT_S2) || (cic_state == INT) ||
      (cic_state == COMB_S2) || (cic_state == COMB))
    wr <= 1'b1;
  else
    wr <= 1'b0;

  if (rst)
    wr_data <= 1'b0;
  else if (cic_state == INT_START)
    wr_data <= in_data;
  else if ((cic_state == INT_S2) || (cic_state == INT))
    wr_data <= wr_data + rd_data;
  else if (cic_state == COMB_S2)
    wr_data <= sum;
  else if (cic_state == COMB)
    wr_data <= wr_data - rd_data;

  if (wr)
    mem[{w_ic, wcnt}] <= wr_data;

  if (rst)
    sum <= 1'b0;
  else if (cic_state == INT)
    sum <= wr_data;

//------------------------------------------------------------------------------
//                            output rounding
//------------------------------------------------------------------------------
  if (rst)
    out_data <= 1'b0;
  else if (cic_state == COMB2)
//    out_data <= wr_data [ACC_WIDTH-1:ACC_WIDTH-OUT_WIDTH];
    out_data <= wr_data [ACC_WIDTH-1:ACC_WIDTH-OUT_WIDTH] +
                       {{(OUT_WIDTH-1){1'b0}}, wr_data [ACC_WIDTH-OUT_WIDTH-1]};
end

/* Sequence of events
*/

always @*
begin
  case(cic_state)
    CIC_START:
    begin
      if (in_strobe)
        cic_next = INT_START;
      else
        cic_next = CIC_START;
    end

    INT_START:
      cic_next = INT_S2;
    
    INT_S2:
      cic_next = INT;

    INT:
    begin
      if (wcnt != (STAGES-1))
        cic_next = INT;
      else if (out_strobe_flag)
        cic_next = COMB_START;
      else
        cic_next = CIC_START;
    end

    COMB_START:
      cic_next = COMB_S2;

    COMB_S2:
      cic_next = COMB;
  
    COMB:
      if (wcnt != (STAGES-1))
        cic_next = COMB;
      else
        cic_next = COMB2;

    COMB2: // save out_data
      cic_next = CIC_START;
 
    default:
      cic_next = CIC_START;
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

