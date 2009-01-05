module peakdetector(
  input clock,
  input signed [15:0] in_data,
  output reg signed [15:0] dc,
  output reg [15:0] peak,
  output reg [31:0] power
  );


//compute peak amplitude and average power over 2^WINDOW_BITS samples
//output rate = 125MHz/2^WINDOW_BITS ~ 4Hz
parameter WINDOW_BITS = 25;


//internal registers
reg [WINDOW_BITS:0] count;
reg signed [15:0] ac;
reg [15:0] abs;
reg [15:0] max;
reg signed [15+WINDOW_BITS:0] sum1;
reg [31+WINDOW_BITS:0] sum2;


initial
  begin
  count = 0;
  ac = 0;
  dc = 0;
  abs = 0;
  max = 0;
  sum1 = 0;
  sum2 = 0;
  end




always @(posedge clock)
  begin
  //dc correction
  ac <= in_data - dc;
  //absolute value
  if (ac[15]) abs <= -ac; else abs <= ac;

  if (count[WINDOW_BITS])
    begin
    //output
    peak <= max;
    dc <= sum1[15+WINDOW_BITS:WINDOW_BITS] + sum1[WINDOW_BITS-1];
    power <= sum2[31+WINDOW_BITS:WINDOW_BITS] + sum2[WINDOW_BITS-1];
    //reset
    max <= 0;
    sum1 <= 0;
    sum2 <= 0;
    count <= 0;
    end
  else
    begin
    //update
    if (abs > max) max <= abs;
    sum1 <= sum1 + in_data;
    sum2 <= sum2 + sqr;
    count <= count + 1'b1;
    end
  end





//16-bit square function, registered output
square16 square16_inst(
  .clock(clock),
  .dataa(ac),
  .datab(ac),
  .result(sqr)
  );


wire [31:0] sqr;




endmodule


