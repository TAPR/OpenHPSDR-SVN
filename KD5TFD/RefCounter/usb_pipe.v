//
// usb-pipe -- Drive data from a fifo to the fx2 usb fifo to be read by the PC 
//
// Derived from Phil Harman's (VK6APH) Wolfson Verilog code 
// 
// Copyright Bill Tracey 2006 (bill@ewjt.com)
//

//
// This module takes data from a fifo and puts it into the FX2 fifo to send it over the USB bus to the PC 
// 
module fx2_usb_pipe(clk_i,   // fx2 clk 
                     fd_io,   // bidi fifo data to/from fx2 
                     slrd_o,   // fx2 fifo read line 
                     slwr_o,   // fx2 fifo write line
                     flags_i,  // fx2 flags
                     pa_io,    // fx2 port a 
                     data_i,  // data from module user 
                     read_strobe_o, // line to strobe to read data in from fifo 
                     data_avail_i // flag indicating data is available in fifo  
); 
				

input clk_i;                 // FX2 clock - 24MHz
output  [7:0] fd_io;         // bidirectional FIFO data to/from the FX2 *******************************8
output reg slwr_o;          // FX2's write line
output slrd_o;              // FX2's FIFO read line
input [2:0] flags_i;        // Flags input from the FX2
inout [7:0] pa_io;           // bidirectional FX2's Port A data
input [7:0] data_i; 
output reg read_strobe_o; 
input data_avail_i; 


// set up FX2 Port A

assign pa_io[0] = 1'b1; // always set on
assign pa_io[1] = 1'b1; // always set on
assign pa_io[2] = 1'b1; // set FIFO bus to write always
assign pa_io[3] = 1'b1; // always set on
assign pa_io[5:4] = 2'b10; // Select FIFO 4 to send A/D data to PC
assign pa_io[6] = 1'b1; // set packet end to false

assign slrd_o = 1'b1; // set Read to false


// reg read_strobe_o; 

/*
 meaning of FX2 flags etc
FIFO2 data available FX2_flags[0] = 1
FIFO3 data available FX2_flags[1] = 1
FIFO4 ready to accept data  FX2_flags[2] = 1
FIFO5 ready to accept data  FX2_PA[7] = 1
set FX2_PA[0] = 1'b1;
set FX2_PA[1] = 1'b1;
set FX2_PA[3] = 1'b1;
FX2_PA[2] = FIFO_DATAOUT_OE;
FX2_PA[6] = FIFO_PKTEND;
FX2_PA[5:4] = FIFO_FIFOADR;

FX2_FD is a bi-directional data bus

*/




//////////////////////////////////////////////////////////////
//
//                              State Machine to manage FX2 USB interface
//
//////////////////////////////////////////////////////////////
reg syncd_drdy;

reg [3:0] state; 
reg [3:0] read_strobe_count;   // not really used anymore 

always @ (posedge clk_i)
begin

        syncd_drdy <= data_avail_i;

        case ( state )
    0:  begin
            if (syncd_drdy) begin       // data ready, so let's start the xfer... -- get it out of the inbound fifo
               	read_strobe_o <= 1;
				read_strobe_count <= 4'b0;			
	            state <= 5;
            end
            else begin                          // hang out here until data is ready.
                // slwr_o <= 1;
                state <= 0;
                // sent_flag <= 0;
                read_strobe_o <= 0;
            end
			slwr_o <= 1; 
        end

    5:      begin // turn off read strobe if count popped, and put the data into the fx2
				read_strobe_count <= read_strobe_count - 4'b0001; 
				if ( read_strobe_count == 0 ) begin
	                read_strobe_o <= 0;
					state <= 3; 
				end 
				else begin
					read_strobe_o <= 1; 
    	            state <= 5;
				end
				slwr_o <= 1; 
            end

    3:      begin    // holding register loaded -- get the data out of here
            	slwr_o <= 1;
	            state <= 1;
				//  sent_flag <= 0;
        	    read_strobe_o <= 0;
            end

    1: begin
            read_strobe_o <= 0;
            if ( flags_i[2] ) begin     // if fifo is ready, write to it and exit this state
                slwr_o <= 0;
                state <= 2;
                // sent_flag <= 1;                 // (this'll go hi on the next clock)
            end
            else begin                  // otherwise, hang out here until fifo is rdy.
                slwr_o <= 1;
                state <= 1;
                // sent_flag <= 0;
            end
        end

    2: begin                                                 // data written to fifo, go back to 0
                read_strobe_o <= 0;
                // sent_flag <= 0;
                slwr_o <= 1;
                state <= 0;
        end

        default: state <= 0;
        endcase
end



// FX2_FD is tristate when write is hi, otherwise it's the register value.

assign fd_io[0] = slwr_o ? 1'bZ : data_i[0];
assign fd_io[1] = slwr_o ? 1'bZ : data_i[1];
assign fd_io[2] = slwr_o ? 1'bZ : data_i[2];
assign fd_io[3] = slwr_o ? 1'bZ : data_i[3];
assign fd_io[4] = slwr_o ? 1'bZ : data_i[4];
assign fd_io[5] = slwr_o ? 1'bZ : data_i[5];
assign fd_io[6] = slwr_o ? 1'bZ : data_i[6];
assign fd_io[7] = slwr_o ? 1'bZ : data_i[7];


endmodule

