//  V1.0 August 18, 2006
//
//  Copyright (C) 2006  P. Covington, N8VB
//
//  HPSDR - High Performance Software Defined Radio
//
//	GPIO CONTROL module for OZY V1
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
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
//  Controls OZY GPIO
//

module gpio_control(FX2_CLK, SCK, SI, SO, CS, GPIO);

 `define PORT0ADRREG 7'h1		// port0 register address = 0x01
 `define PORT1ADRREG 7'h2       // port1 register address = 0x02
 `define PORT2ADRREG 7'h3       // port2 register address = 0x03
	
 input 				FX2_CLK;	// master system clock
 input 				SCK;		// SPI SCK from FX2
 input 				SI;			// SPI serial in from FX2
 inout 				SO;			// SPI serial out to FX2
 input 				CS;         // FPGA chip select from FX2
 inout [23:0]		GPIO;		// OZY GPIO lines
 
 wire [6:0]			serial_address;
 wire [7:0]			serial_data;
 wire				serial_strobe;

 wire [7:0]			port0register;	// port0 is read/write
 wire [7:0]			port1register;	// port1 is read/write
 
 assign GPIO[7:0] = 	(1'b1) ? port0register : 8'bz; 	// port0 always output
 assign GPIO[15:8] = 	(1'b1) ? port1register : 8'bz; 	// port1 always output
 assign GPIO[23:16] =  	(1'b1) ? 8'bz : 8'bz;			// port2 always input

 wire [7:0] port2register = GPIO[23:16]; // port2 is read only
											
 // SPI communications module
 SPI_REGS spi_regs(	.FX2_CLK(FX2_CLK),
					.SI(SI),
					.SO(SO),
					.SCK(SCK),
					.CS(CS),
					.saddr(serial_address),
					.sdata(serial_data),
					.sstrobe(serial_strobe),
					.GPReg0(port0register), // readback for port0
					.GPReg1(port1register), // readback for port1
					.GPReg2(port2register), // readback for port2
					.GPReg3(),
					.GPReg4(),
					.GPReg5(),
					.GPReg6(),
					.GPReg7());

 // GPIO 1-8 OUTPUTS, Address = 0x01	
 RegisterX #(`PORT0ADRREG, 8) port0reg(.CLK(FX2_CLK),
									.STB(serial_strobe),
									.ADDR(serial_address),
									.IN(serial_data),
									.OUT(port0register),
									.CHANGED());

 // GPIO 9-16 OUTPUTS, Address = 0x02					
 RegisterX #(`PORT1ADRREG, 8) port1reg(.CLK(FX2_CLK),
									.STB(serial_strobe),
									.ADDR(serial_address),
									.IN(serial_data),
									.OUT(port1register),
									.CHANGED());

 // GPIO 17-24 INPUTS, , Address = 0x03					
 RegisterX #(`PORT2ADRREG, 8) port2reg(.CLK(FX2_CLK),
									.STB(serial_strobe),
									.ADDR(serial_address),
									.IN(serial_data),
									.OUT(), 		// don't allow writes
									.CHANGED());

endmodule