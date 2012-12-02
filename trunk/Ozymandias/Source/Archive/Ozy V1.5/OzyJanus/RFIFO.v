module RFIFO (rst, clk, full, usedw, wrreq, data, rdreq, q);

input  wire        rst;
input  wire        clk;
output wire        full;
output reg  [10:0] usedw;
input  wire        wrreq;
input  wire [15:0] data;
input  wire        rdreq;
output reg  [15:0] q;

reg [15:0] mem [0:2047];
reg [10:0] inptr, outptr;

assign full = (usedw == 12'hFFF);

always @(posedge clk)
begin
  if (rst)
    inptr <= 1'b0;
  else if (wrreq)
    inptr <= inptr + 1'b1;

  if (rst)
    outptr <= 1'b0;
  else if (rdreq)
    outptr <= outptr + 1'b1;

  if (rst)
    usedw <= 1'b0;
  else if (rdreq & !wrreq)
    usedw <= usedw - 1'b1;
  else if (wrreq & !rdreq)
    usedw <= usedw + 1'b1;

  if (wrreq)
    mem[inptr] <= data;

  if (rdreq)
    q <= mem[outptr];
end

endmodule