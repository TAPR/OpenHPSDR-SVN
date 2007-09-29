
/////////////////////////////////////////////////////////////////
//
// Single bit PWM 16 bit D/A converters
//
/////////////////////////////////////////////////////////////////

// This runs off the 125MHz clock to provide 125/0.048 = 11 bits of resolution.

reg [15:0] I_Data;
reg [15:0] Q_Data;
reg [15:0] I_Data_in;
reg [15:0] Q_Data_in;
reg [16:0] I_PWM_accumulator;
reg [16:0] Q_PWM_accumulator;


always @(negedge clock)
begin
   I_Data_in <= I_Data + 16'h8000;         // so that 0 in gives 50:50 mark/space
   Q_Data_in <= Q_Data + 16'h8000;
   I_PWM_accumulator <= I_PWM_accumulator[15:0] + I_Data_in;
   Q_PWM_accumulator <= Q_PWM_accumulator[15:0] + Q_Data_in;

end

assign I_PWM_out = I_PWM_accumulator[16];       // send to FPGA pins
assign Q_PWM_out = Q_PWM_accumulator[16];
