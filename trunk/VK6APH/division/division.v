//test code for division

// 64 bit version 

module division(quotient,ready,dividend,divider,clk);
   
   input [31:0]  dividend; // frequency 
   input [31:0]  divider;  // 125MHz
   input         clk;
   output        quotient;
   output        ready;

   reg [95:0]    qr;
   reg [33:0]    diff;



   
//   wire [63:0]   quotient = qr[63:0];
   wire [19:0]   quotient = qr[31:12];  // take top 20 bits for CORDIC




   reg [7:0]     bits; 
   wire          ready = !bits;
   
   always @( posedge clk ) 

     if( ready) begin

        bits = 64;
//        qr = {64'd0,dividend};
        qr = {32'd0,dividend, 32'd0}; // multiply freq by 2^32

     end else begin

        diff = qr[95:63] - {1'b0,divider};

        if( diff[32] )
          qr = {qr[94:0],1'd0};      // negative so use qr shifted left, result = 0
        else
          qr = {diff[32:0],qr[62:0],1'd1}; // positive so use  diff shifted left, result = 1
        
        bits = bits - 1'b1;

     end

endmodule


/*
// 64 bit version 

module division(quotient,ready,dividend,divider,clk);
   
   input [63:0]  dividend; // frequency 
   input [63:0]  divider;  // 125MHz
   input         clk;
   output        quotient;
   output        ready;

   reg [127:0]    qr;
   reg [64:0]    diff;



   
//   wire [63:0]   quotient = qr[63:0];
   wire [19:0]   quotient = qr[31:12];  // take top 20 bits for CORDIC




   reg [7:0]     bits; 
   wire          ready = !bits;
   
   always @( posedge clk ) 

     if( ready) begin

        bits = 64;
//        qr = {64'd0,dividend};
        qr = {32'd0,dividend, 32'd0}; // multiply freq by 2^32

     end else begin

        diff = qr[127:63] - {1'b0,divider};

        if( diff[64] )
          qr = {qr[126:0],1'd0};      // negative so use qr
        else
          qr = {diff[63:0],qr[62:0],1'd1}; // positive so use  diff 
        
        bits = bits - 1'b1;

     end

endmodule

*/



/*
// 32 bit version 
module division(quotient,ready,dividend,divider,clk);
   
   input [31:0]  dividend;
   input [31:0]  divider;
   input         clk;
   output        quotient;
   output        ready;

   reg [63:0]    qr;
   reg [32:0]    diff;



   
   wire [31:0]   quotient = qr[31:0];

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

*/




