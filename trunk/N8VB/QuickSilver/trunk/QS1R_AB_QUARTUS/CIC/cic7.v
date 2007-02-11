// CIC7
//
// January 25, 2007
// P. Covington, N8VB
//
// Instance usage
// 	cic7_top cic7_inst
//	(
//	.clk(),
//	.clk_enable(),
//	.reset(),
//	.drate_in(),
//	.data_in(),
//	.data_out(),
//	.output_scaling(),
//	.ds_out()
//	);
//

module cic7_top(
clk,
clk_enable,
reset,
drate_in,
data_in,
data_out,
output_scaling,
ds_out
);


// NOTE: for decimation = R INTERNAL_WIDTH should be >= 16bits + 7 * log2(R)
parameter INTERNAL_WIDTH = 56;
 
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
        deci_count <= deci_count + 16'd1;
      end
    end
  end
end // ds_output

// strobe goes high to indicate data available on output
assign strobe = (deci_count == 16'd1 && clk_enable == 1'b1)? 1'b1 : 1'b0;

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
assign data_out = out_data_register;

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

//
// Sixth Integrator instance
//  

wire signed [INTERNAL_WIDTH-1:0] integrator6_out;
  
integrator integrator6_inst(
.clk(clk),
.clk_enable(clk_enable),
.reset(reset),
.data_in(integrator5_out),
.data_out(integrator6_out)
);

//
// Seventh Integrator instance
//  

wire signed [INTERNAL_WIDTH-1:0] integrator7_out;
  
integrator integrator7_inst(
.clk(clk),
.clk_enable(clk_enable),
.reset(reset),
.data_in(integrator6_out),
.data_out(integrator7_out)
);

wire signed [INTERNAL_WIDTH-1:0] integrator_last_out = integrator7_out;

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

//
// Sixth Comb instance
//

wire signed [INTERNAL_WIDTH-1:0] comb6_out;

comb comb6_inst(
.clk(clk),
.strobe(strobe),
.reset(reset),
.data_in(comb5_out),
.data_out(comb6_out)
);

//
// Seventh Comb instance
//

wire signed [INTERNAL_WIDTH-1:0] comb7_out;

comb comb7_inst(
.clk(clk),
.strobe(strobe),
.reset(reset),
.data_in(comb6_out),
.data_out(comb7_out)
);

wire signed [INTERNAL_WIDTH-1:0] comb_last_out = comb7_out;
     
endmodule //cic7_top
