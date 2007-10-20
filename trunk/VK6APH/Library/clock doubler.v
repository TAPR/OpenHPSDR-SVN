///////////////////////////////////////////////////////////
//
//    Clock doubler - converts 48MHz clock to 96MHz
//
//	see http://www.deepchip.com/items/0308-12.html
//
///////////////////////////////////////////////////////////

wire D;
wire dclk;
reg Q;

always @(posedge dclk)
begin
	Q = D;
end

assign D = ~Q;
assign dclk = CLK_48MHZ ^ Q;
