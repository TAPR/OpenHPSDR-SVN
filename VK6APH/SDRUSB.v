// Xylo interface for Wolfson WM8785 A/D converter
// Sends 16 bit A/D data for left channel only via FIFO 4
// Wolfson is set via I2C to be 48kHz, 16 bit 
// and DSP/PCM data Mode B.  I2C command string is
// 34 00 65, 34 02 04. 
// Built with Quartus II v5.1 10 December 2005

module SDRUSB(
	FX2_CLK, FX2_FD, FX2_SLRD, FX2_SLWR, FX2_flags, FX2_PA, BCLK, DOUT, LRCLK, LED
);

input FX2_CLK, BCLK, DOUT, LRCLK;
inout [7:0] FX2_FD;			// bidirectional FIFO data to/from the FX2
input [2:0] FX2_flags;		// Flags input from the FX2
output FX2_SLWR, FX2_SLRD; 	// FX2's FIFO read and write lines
inout [7:0] FX2_PA;			// bidirectional FX2's Port A data
output [1:0] LED;			// Red and Green LEDs on Xylo board

reg [5:0]count = 0;

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

// set up FX2 Port A

assign FX2_PA[0] = 1'b1; // always set on 
assign FX2_PA[1] = 1'b1; // always set on 
assign FX2_PA[2] = 1'b1; // set FIFO bus to write always
assign FX2_PA[3] = 1'b1; // always set on
assign FX2_PA[5:4] = 2'b10; // Select FIFO 4 to send A/D data to PC
assign FX2_PA[6] = 1'b1; // set packet end to false 

assign FX2_SLRD  = 1'b1; // set Read to false

reg [2:0] state = 0;
reg [7:0] q;
reg [7:0] fifo_data;

always @ (posedge FX2_CLK)
begin
	case(state)
	0: if (LRCLK) state <= 1;  	// wait for LRCLK to go high
	1: if (BCLK) state <= 2; 	// wait for Wolfson clock to go high
	2: begin
		q[7:1] <= q[6:0];		// shift current data left
		q[0] <= DOUT;			// get the new data
		count <= count + 1;		// increment the counter
		state <= 3;				// next state 
		end
	3: begin
			if (count == 8  || count == 16) // FX2 is set up in 8 bit FIFO mode
			begin							// whilst A/D is in 16 bit so need two write strobes
				fifo_data <= q;  // set up data on FIFO input
				state <= 4;		// write strobe
			end
			else 
				state <= 6;		// not ready to write so loop again
		end
	4: state <= 5;				// Write strobe done, move to next state 
	5: begin
		if (count == 16)		// has all left channel data been sent?
		begin
			count <= 0;			// yes, reset counter and start again
			state <= 0;
		end
		else 					// no, more data to send.
			state <= 6;
	   end

	6: if(~BCLK) state <= 1;	// wait for BCLK to go low then continue
	
	default: state <= 0;					

	endcase 
end
 
// strobe the WR line at state 4 if FIFO 4 is not full
assign FX2_SLWR = ~(state == 4 && FX2_flags[2]); 
// send the data to  FX2 FIFO 4
assign FX2_FD = fifo_data; // send the data to the FX2 FIFO 

// Flash the LEDs to show something is working!

assign LED[0] = DOUT;			// Red LED Monitor A/D data
assign LED[1] = FX2_flags[2];	// Green LED on  when we can write 

endmodule