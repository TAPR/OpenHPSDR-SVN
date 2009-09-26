`timescale 1 ns/100 ps

module led_blinker (clk, err_sigs, led_off);
parameter NUM_SIGS = 2;
parameter CLK_SPEED = 12288000; //frequency of clk
input  wire                clk;
input  wire [NUM_SIGS-1:0] err_sigs;
output wire                led_off;

localparam LED_MAX = 4; // LED_MAX pulses = LED_MAX seconds of blink time
reg  [2:0] LED_state, LED_state_next;
reg  [3:0] led_code, led_code_next; // value to blink -> 0 = do not blink the code
reg  [4:0] led_cnt;
reg [25:0] led_timer;
reg [NUM_SIGS-1:0] ld;  // latched error signals
reg  [3:0] bit_sel;

localparam LED_IDLE   = 0,
           LED_FIND   = 1,
           LED_ERR    = 2,
           LED_BLINK  = 3,
           LED_NXT    = 4,
           LED_WAIT   = 5;

localparam M_TPD = 2;

assign  led_off = (led_cnt >= LED_MAX) ||
                  (LED_state != LED_BLINK) ||
                  (led_timer < ((CLK_SPEED*1)/10)) ||
                  (led_timer > ((CLK_SPEED*9)/10)) ||
                  ((led_timer < ((CLK_SPEED*4)/10)) && !led_code[0]) ||
                  ((led_timer > ((CLK_SPEED*5)/10)) && !led_code[0]);

         
always @(posedge clk) // all errors related to clk
begin
  LED_state <= #M_TPD LED_state_next;

  if (LED_state == LED_ERR)
    led_cnt <= #M_TPD 1'b0;
  else if ((led_cnt < 16) && (led_timer == CLK_SPEED))
    led_cnt <= #M_TPD led_cnt + 1'b1;

  if (LED_state == LED_ERR)
    led_timer <= #M_TPD 1'b0;
  else if (led_timer < CLK_SPEED)
    led_timer <= #M_TPD led_timer + 1'b1;
  else
    led_timer <= #M_TPD 1'b0;

  if ((LED_state == LED_BLINK) && (led_timer == CLK_SPEED))
    led_code <= #M_TPD led_code >> 1;
  else
    led_code <= #M_TPD led_code_next;

  if (LED_state == LED_IDLE)
    bit_sel <= #M_TPD 1'b0;
  else if (((LED_state == LED_FIND) && !ld[bit_sel]) || (LED_state == LED_NXT))
  begin
    if (bit_sel < (NUM_SIGS-1))
      bit_sel <= #M_TPD bit_sel + 1'b1; //rotate - round robin on selecting which code to vlink
    else
      bit_sel <= #M_TPD 1'b0;
  end
end

genvar k;
generate
begin: WHICH_ERR
  for (k = 0; k < NUM_SIGS; k = k + 1)
  begin: Latched_data
    always @(posedge clk)
    begin
      if (err_sigs[k])
        ld[k] <= #M_TPD 1'b1;
      else if ((LED_state == LED_BLINK) && (bit_sel == k) && (led_cnt == LED_MAX))
        ld[k] <= #M_TPD 1'b0; // clear the latched error
    end
  end
end
endgenerate

always @* // all errors related to clk
begin
  case(LED_state)
    LED_IDLE:
    begin
      led_code_next = 4'd0;
      LED_state_next = LED_FIND;
    end

    LED_FIND:
    begin
      if (ld[bit_sel])
      begin
        led_code_next = bit_sel; // determine which code we'll blink
        LED_state_next = LED_ERR;
      end
      else
      begin
        led_code_next = 4'h0;
        LED_state_next = LED_FIND; // keep searching through each of the latched bits (i.e. ld[bit_sel])
      end
    end

    LED_ERR:
    begin
      led_code_next = led_code;
      LED_state_next = LED_BLINK;
    end

    LED_BLINK:
    begin
      led_code_next = led_code;
      if (led_cnt == LED_MAX)
        LED_state_next = LED_NXT;
      else
        LED_state_next = LED_BLINK;
    end

    LED_NXT:
    begin
      led_code_next = led_code;
      LED_state_next = LED_WAIT;
    end

    LED_WAIT:
    begin
      led_code_next = led_code;
      if (led_cnt == (LED_MAX + 2)) // pause between blinking code
        LED_state_next = LED_FIND;
      else
        LED_state_next = LED_WAIT;
    end

    default:
    begin
      led_code_next  = 4'd0;
      LED_state_next = LED_IDLE;
    end
  endcase
end

endmodule