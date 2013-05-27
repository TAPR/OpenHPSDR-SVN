////////////////////////////////////////////////////////////////////
////                                                           ////
//// Copyright (C) 2006 Phil Harman VK6APH                     ////
////                    pvharman@arach.net.au                 	////
//// Copyright (C) 2012 Joe Martin K5SO								////
////							k5so@valornet.com
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published
//  by the Free Software Foundation; version 1 
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
//  This design is LGPL software written for the HPSDR project
//  by Phil Harman, VK6APH.  The author assumes no libability on how the
//  code might be used or warranty that the code is fit for any 
//  particular application. 
//
//  The purpose of the code is to act as an I2C bus activity monitor in order to 
//  intercept data being sent by boards on the Atlas bus in order to route that 
//  data to the Command and Control byte stream to the PC.
//
//  I2C addresses for which this module is responsive are assigned as follows: 
//
//  0x10 = Mercury 1 firmware version number and ADC overflow status
//  0x11 = Mercury 2 firmware version number and ADC overflow status
//  0x12 = Mercury 3 firmware version number and ADC overflow status
//  0x13 = Mercury 4 firmware version number and ADC overflow status
//
//  0x15 = Penelope/PennyLane firmware version number
//  0x16 = ALC (output power from Penelope/PennyLane)
//  0x17 = FWD (forward power from Alex via Penelope/PennyLane)
//  0x18 = REV (reverse power from Alex via Penelope/PennyLane)
//
//  Intercepts data on the I2C bus and passes values to the Ozy C&C output byte stream to the PC

module I2C_monitor (CLK, sda, scl, Penny_FW, Merc1_version, Merc2_version, 
					Merc3_version, Merc4_version, Merc1_ovld, Merc2_ovld, 
					Merc3_ovld, Merc4_ovld, ALC, FWD, REV);

input CLK;  							//48MHz clock 
input sda;
input scl;
output reg [7:0]	Penny_FW;		// Penelope/PennyLane FW version number
output reg [7:0]	Merc1_version;	// Mercury 1 FW version number
output reg [7:0]	Merc2_version;	// Mercury 2 FW version number
output reg [7:0]	Merc3_version;	// Mercury 3 FW version number
output reg [7:0]	Merc4_version;	// Mercury 4 FW version number
output reg 			Merc1_ovld;		// Mercury 1 ADC overflow status
output reg 			Merc2_ovld;		// Mercury 2 ADC overflow status
output reg 			Merc3_ovld;		// Mercury 3 ADC overflow status
output reg 			Merc4_ovld;		// Mercury 4 ADC overflow status
output reg [11:0]	ALC;				// Penelope/PennyLane power out, 12 bits
output reg [11:0]	FWD;				// Forward power at Alex via Penelope/PennyLane, 12 bits
output reg [11:0]	REV;				// Reverse power at Alex via Penelope/PennyLane, 12 bits

reg enable;
reg [7:0]rcv_reg0; 		// data intercepted on the I2C bus, byte0
reg [7:0]rcv_reg1; 		// data intercepted on the I2C bus, byte1
wire dg_scl;				// de-glitched scl 
wire dg_sda;				// de-glitched sda

// enable logic 
reg [5:0]enable_count;

always @ (posedge CLK)
begin
	if (enable_count[5]) begin
		enable <= 1'b1;		// enable high to run
	end
	else begin
		enable <= 0;
		enable_count <= enable_count + 1'b1;
	end
end 

// de-glitch sda and scl using CLK
// sda and scl have to be stable for 1 consecutive CLK sample before a transition is considered valid

deglitch deglitch_scl(.clock(CLK), .in(scl),.out(dg_scl));
deglitch deglitch_sda(.clock(CLK), .in(sda),.out(dg_sda));

//
// Capture Start flag - SDA falls while SCL is high
//
reg	start;
reg	start_clr;

always @(negedge dg_sda or posedge start_clr)
  if(start_clr)
    start <= 1'b0;
  else
  if(dg_scl)
    start <= 1'b1;

reg	[4:0]state; 
reg	[4:0]counter;
reg	[6:0]address_targeted;

///  STATE MACHINE	 
always @ (posedge dg_scl) begin
	if (enable) begin
			start_clr <= 1'b0;
			if (start) begin
			case (state)
				0: begin
					address_targeted[6 - counter] <= dg_sda;
					counter <= counter + 1;
					if (counter < 7) state <= 0;
					else begin						//r/w bit position (ignore it), all transactions are treated as I2C master writes
					counter <= 0;
					state <= state + 1;
					end
			    end // 0:
				 
				 1: begin							//ack position in the stream
				 if ((address_targeted >= 7'h10 & address_targeted <= 7'h13) | (address_targeted >= 7'h15 & address_targeted <= 7'h18)) begin
							if (dg_sda) begin    //NACK, board did not respond, go wait for another start
								start_clr <= 1'b1;
								state <= 0;
							end
							else begin
								counter <= 0;
								state <= state + 1; // ACK, board did respond, interecept its data
							end
					 end
					 else begin						//address sent is not relevant for us; ignore the transmission
					 start_clr <= 1'b1; 			//clear start condition and wait for another start
					 state <= 0;
					 end
				  end // 1:
				  
				  2: begin							//clock in first byte
						rcv_reg0[7 - counter] <= dg_sda;
						counter <= counter + 1; 
						if (counter < 8) state <= 2;
						else begin					//now at ack position in I2C stream
						counter <= 0;
						case (address_targeted)	//route the data
							
								7'h10: Merc1_ovld <= ~rcv_reg0[0];	// bit from high byte (inverted)
								7'h11: Merc2_ovld <= ~rcv_reg0[0];
								7'h12: Merc3_ovld <= ~rcv_reg0[0];
								7'h13: Merc4_ovld <= ~rcv_reg1[0];
								7'h16: ALC[11:8] <= rcv_reg0[3:0];	// 4 bits from high byte
								7'h17: FWD[11:8] <= rcv_reg0[3:0];
								7'h18: REV[11:8] <= rcv_reg0[3:0];
								
								default: begin
											end
								
							endcase //address_targeted
/*							if (dg_sda) begin    //NACK, board did not respond, go wait for another start
							start_clr <= 1'b1;
								state <= 0;
							end //dg_sda
							else begin 
							state <= state + 1;*/
							state <= state + 1;
							end //else
				   end //2:

/*
					3: begin
							if (dg_sda) begin    //NACK, board did not respond, go wait for another start
							start_clr <= 1'b1;
								state <= 0;
							end //dg_sda
							else begin
							state <= state + 1;
							end
						end //3:
*/						
					3: begin						//clock in second byte (if no NACK occurred in 3:)
						rcv_reg1[7 - counter] <= dg_sda;
						counter <= counter + 1; 
						if (counter < 8) state <= 3;
						else begin
						counter <= 0;
						case (address_targeted)
							
								7'h10: Merc1_version <= rcv_reg1;	//low byte
								7'h11: Merc2_version <= rcv_reg1;
								7'h12: Merc3_version <= rcv_reg1;
								7'h13: Merc4_version <= rcv_reg1;
								7'h15: Penny_FW <= rcv_reg1;
								7'h16: ALC[7:0] <= rcv_reg1;
								7'h17: FWD[7:0] <= rcv_reg1;
								7'h18: REV[7:0] <= rcv_reg1;
								
								default: begin
											end
								
							endcase //address_targeted
							start_clr <= 1'b1;
							state <= 0;			// done, go wait for a new start
						end //else
					 end //4:					
		  endcase //state
		end // start
	end //enable
end //always
					
endmodule