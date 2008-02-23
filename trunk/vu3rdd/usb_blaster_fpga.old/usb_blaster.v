//////////////////////////////////////////////////////////////
//
//                      Pin Assignments
//
/////////////////////////////////////////////////////////////
//
//
// B0 - TDI
// B1 - TDO
// B2 - TCK
// B3 - TMS
// A23 - Atlas TMS - output
// A24 - Atlas TCK - output
// A27 - Atlas TDO - input
// A29 - Atlas TDI - output

// 17th August 2007 - added LED flash so we can see the FPGA is loaded correctly

module usb_blaster_emulation(
        IFCLK,B0, B1, B2, B3,
		A23, A24, A27, A29, LED0
		);
		

input  IFCLK;		
input  B0;
output B1;
input  B2;
input  B3;

output A23;
output A24;
input  A27;
output A29;
output LED0;

assign A23 = B3;
assign A24 = B2;
assign B1  = A27;
assign A29 = B0;

// flash a LED so we can see FPGA is loaded correctly

reg [24:0]counter;
always @(posedge IFCLK)
begin
counter = counter + 1'b1;
end

assign LED0 = counter[24];




endmodule

