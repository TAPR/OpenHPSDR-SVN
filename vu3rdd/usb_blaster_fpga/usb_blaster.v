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

module usb_blaster_emulation(
        B0, B1, B2, B3,
		A23, A24, A27, A29
		);
		
input  B0;
output B1;
input  B2;
input  B3;

output A23;
output A24;
input  A27;
output A29;

assign A23 = B3;
assign A24 = B2;
assign B1  = A27;
assign A29 = B0;


endmodule

