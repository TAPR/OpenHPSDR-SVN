// 32 bit version 


module divide(quotient,ready,dividend,divider,clk);
   
   input [31:0]  dividend;
   input [31:0]  divider;
   input         clk;
   output [31:0] quotient;
   output        ready;

   reg [63:0]    qr;
   reg [32:0]    diff;
   reg [31:0]   quotient;

   always @ (negedge clk)
   begin
		if (ready)				// get result when ready 
			quotient <= qr[31:0];
   		else 
			quotient <= quotient;
	end

   reg [5:0]     bits; 
   wire          ready = !bits;
   
   always @( posedge clk ) 

     if( ready) begin

        bits = 32;
        qr = {32'd0,dividend};

     end else begin

        diff = qr[63:31] - {1'b0,divider};

        if( diff[32] )
          qr = {qr[62:0],1'd0};
        else
          qr = {diff[31:0],qr[30:0],1'd1};
        
        bits = bits - 1'b1;

     end

endmodule