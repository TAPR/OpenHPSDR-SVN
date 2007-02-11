// CIC5
//
// January 19, 2007
// P. Covington, N8VB

module cic5_top(
clk,
clk_enable,
reset,
drate_in,
data_in,
data_out,
output_scaling,
ds_out
);

parameter INTERNAL_WIDTH = 80;
parameter SECTIONS = 5;
 
input clk; 
input clk_enable; 
input reset;
input [15:0] drate_in; 
input signed [15:0] data_in;
input [7:0] output_scaling; 
output signed [31:0] data_out;
output ds_out;

reg [15:0] deci_count; 
wire strobe; 
reg ds_out_reg; 

//
// Decimation counter
//

always @ (posedge clk or posedge reset)
begin: ds_output
  if (reset == 1'b1) begin
    deci_count <= 16'd0;
  end
  else begin
    if (clk_enable == 1'b1) begin
      if (deci_count == drate_in) begin // drate_in is decimation rate - 1
        deci_count <= 16'd0;
      end
      else begin
        deci_count <= deci_count + 1;
      end
    end
  end
end // ds_output

// strobe goes high to indicate data available on output
assign strobe = (deci_count == 16'd1 && clk_enable == 1'b1)? 1 : 0;

//
// Data strobe output register
//

always @ (posedge clk or posedge reset)
begin: ds_output_register
  if (reset == 1'b1) begin
    ds_out_reg <= 1'b0;
  end
  else begin
      ds_out_reg <= strobe;
  end
end // ds_output_register

assign ds_out = ds_out_reg;
   
//
// Input data register
//

reg signed [15:0] indata_reg; 

always @ (posedge clk or posedge reset)
begin: input_register
  if (reset == 1'b1) begin
    indata_reg <= 0;
  end
  else begin
    if (clk_enable == 1'b1) begin
      indata_reg <= data_in;
    end
  end
end // input_register

//
// Output data register
//

wire signed [31:0] output_typeconvert;
reg signed [31:0] out_data_register;

always @ (posedge clk or posedge reset)
begin: output_register
  if (reset == 1'b1) begin
    out_data_register <= 0;
  end
  else begin
    if (strobe == 1'b1) begin
      out_data_register <= output_typeconvert;
    end
  end
end // output_register

assign output_typeconvert = comb_last_out[INTERNAL_WIDTH-1:INTERNAL_WIDTH-32];
assign data_out = outdata_register;

wire signed [15:0] first_stage_data = indata_reg;
wire signed [INTERNAL_WIDTH-1:0] resize_input_data = $signed({{(INTERNAL_WIDTH-16){first_stage_data[15]}}, first_stage_data});

//
// First Integrator instance
//  

wire signed [INTERNAL_WIDTH-1:0] integrator1_out;
  
integrator integrator1_inst(
.clk(clk),
.clk_enable(clk_enable),
.reset(reset),
.data_in(resize_input_data),
.data_out(integrator1_out)
);

//
// Second Integrator instance
//  

wire signed [INTERNAL_WIDTH-1:0] integrator2_out;
  
integrator integrator2_inst(
.clk(clk),
.clk_enable(clk_enable),
.reset(reset),
.data_in(integrator1_out),
.data_out(integrator2_out)
);

//
// Third Integrator instance
//  

wire signed [INTERNAL_WIDTH-1:0] integrator3_out;
  
integrator integrator3_inst(
.clk(clk),
.clk_enable(clk_enable),
.reset(reset),
.data_in(integrator2_out),
.data_out(integrator3_out)
);

//
// Fourth Integrator instance
//  

wire signed [INTERNAL_WIDTH-1:0] integrator4_out;
  
integrator integrator4_inst(
.clk(clk),
.clk_enable(clk_enable),
.reset(reset),
.data_in(integrator3_out),
.data_out(integrator4_out)
);

//
// Fifth Integrator instance
//  

wire signed [INTERNAL_WIDTH-1:0] integrator5_out;
  
integrator integrator5_inst(
.clk(clk),
.clk_enable(clk_enable),
.reset(reset),
.data_in(integrator4_out),
.data_out(integrator5_out)
);

wire signed [INTERNAL_WIDTH-1:0] integrator_last_out = integrator5_out;

//
// First Comb instance
//

wire signed [INTERNAL_WIDTH-1:0] comb1_out;

comb comb1_inst(
.clk(clk),
.strobe(strobe),
.reset(reset),
.data_in(integrator_last_out),
.data_out(comb1_out)
);

//
// Second Comb instance
//

wire signed [INTERNAL_WIDTH-1:0] comb2_out;

comb comb2_inst(
.clk(clk),
.strobe(strobe),
.reset(reset),
.data_in(comb1_out),
.data_out(comb2_out)
);

//
// Third Comb instance
//

wire signed [INTERNAL_WIDTH-1:0] comb3_out;

comb comb3_inst(
.clk(clk),
.strobe(strobe),
.reset(reset),
.data_in(comb2_out),
.data_out(comb3_out)
);

//
// Fourth Comb instance
//

wire signed [INTERNAL_WIDTH-1:0] comb4_out;

comb comb4_inst(
.clk(clk),
.strobe(strobe),
.reset(reset),
.data_in(comb3_out),
.data_out(comb4_out)
);

//
// Fifth Comb instance
//

wire signed [INTERNAL_WIDTH-1:0] comb5_out;

comb comb5_inst(
.clk(clk),
.strobe(strobe),
.reset(reset),
.data_in(comb4_out),
.data_out(comb5_out)
);

wire signed [INTERNAL_WIDTH-1:0] comb_last_out = comb5_out;
     
endmodule //cic5_top

//
// ---------------------------------
//
// CIC Integrator Module
//
// ---------------------------------
//
module integrator(
clk,
clk_enable,
reset,
data_in,
data_out
);

parameter INTERNAL_WIDTH = 80;

input clk; 
input clk_enable; 
input reset;
input signed [INTERNAL_WIDTH-1:0] data_in;
output signed [INTERNAL_WIDTH-1:0] data_out;

wire signed [INTERNAL_WIDTH:0] add = data_in + data_out;  //make 1 bit larger for add
wire signed [INTERNAL_WIDTH-1:0] sum = add[INTERNAL_WIDTH-1:0];

always @ (posedge clk or posedge reset)
begin: integrator_process
  if (reset == 1'b1) begin
    data_out <= 0;
  end
  else begin
    if (clk_enable == 1'b1) begin
      data_out <= sum;
    end
  end
end // integrator_process
    
endmodule // integrator

//
// ---------------------------------
//
// CIC Comb Module
//
// ---------------------------------
//
module comb(
clk,
strobe,
reset,
data_in,
data_out
);

parameter INTERNAL_WIDTH = 80;

input clk; 
input strobe; 
input reset;
input signed [INTERNAL_WIDTH-1:0] data_in;
output signed [INTERNAL_WIDTH-1:0] data_out;

reg signed [INTERNAL_WIDTH-1:0] diff;
wire signed [INTERNAL_WIDTH:0] sub = data_in - diff;  //make 1 bit larger for subtraction

always @ (posedge clk or posedge reset)
begin: comb_process
  if (reset == 1'b1) begin
    diff <= 0;
  end
  else begin
    if (strobe == 1'b1) begin
      diff <= data_in;
    end
  end
end // comb_process

assign data_out = sub[INTERNAL_WIDTH-1:0];
    
endmodule // comb
