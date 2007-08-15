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
// A23 - Atlas TMS
// A24 - Atlas TCK
// A27 - Atlas TDO
// A29 - Atlas TDI

module usb_blaster_emulation(
        B0, B1, B2, B3,
		A23, A24, A27, A29
		);
		
input B0;
input B1;
input B2;
input B3;

output A23;
output A24;
output A27;
output A29;

assign A23 = B3;
assign A24 = B2;
assign A27 = B1;
assign A29 = B0;


endmodule

