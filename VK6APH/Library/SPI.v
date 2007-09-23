//////////////////////////////////////////////////////////////
//
// 		Set up TLV320 using SPI 
//
/////////////////////////////////////////////////////////////

/* Data to send to TLV320 is 

 	1E 00 - Reset chip
 	12 01 - set digital interface active
 	08 15 - D/A on, mic input, mic 20dB boost
   (08 14 - ditto but no mic boost)
 	0C 00 - All chip power on
 	0E 02 - Slave, 16 bit, I2S
 	10 00 - 48k, Normal mode
 	0A 00 - turn D/A mute off

*/

reg index;
reg [15:0]tdata;
reg [2:0]load;
reg [3:0]TLV;
reg [15:0] TLV_data;
reg [3:0] bit_cnt;

// Set up TLV320 data to send 

always @ (posedge index)		
begin
load <= load + 3'b1;			// select next data word to send
case (load)
3'd0: tdata <= 16'h1E00;		// data to load into TLV320
3'd1: tdata <= 16'h1201;
3'd2: tdata <= 16'h0815;
3'd3: tdata <= 16'h0C00;
3'd4: tdata <= 16'h0E02;
3'd5: tdata <= 16'h1000;
3'd6: tdata <= 16'h0A00;
default: load <= 0;
endcase
end

// State machine to send data to TLV320 via SPI interface

assign CMODE = 1'b1;		// Set to 1 for SPI mode

always @ (posedge CMCLK)	// use 12.5MHz clock for SPI
begin
case (TLV)
4'd0: begin
         nCS <= 1'b1;                   // set TLV320 CS high
         bit_cnt <= 4'd15;             	// set starting bit count to 15
         index <= ~index;               // load next data to send
         TLV <= TLV + 4'b1;
      end
 4'd1: begin
         nCS <= 1'b0;                   // start data transfer with nCS low
         TLV_data <= tdata;
         MOSI <= TLV_data[bit_cnt];    	// set data up
         TLV <= TLV + 4'b1;
       end
 4'd2: begin
         SSCK <= 1'b1;                  // clock data into TLV320
         TLV <= TLV + 4'b1;
       end
 4'd3: begin
         SSCK <= 1'b0;                  // reset clock
         TLV <= TLV + 4'b1;
       end
 4'd4: begin
          if(bit_cnt == 0) begin   		// word transfer is complete, check for any more
            index <= ~index;
            TLV <= 4'd5;
          end
          else begin
            bit_cnt <= bit_cnt - 1'b1;
            TLV <= 4'b1;                   // go round again
          end
       end                                 // end transfer
 4'd5: begin
         if (load == 7)begin               // stop when all data sent
            TLV <= 4'd5;                   // hang out here forever
            nCS <= 1'b1;                   // set CS high
         end
         else TLV <= 0;                    // else get next data
       end
 default: TLV <= 0;
 endcase
 end