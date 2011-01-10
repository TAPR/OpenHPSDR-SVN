//  V1.0 August 18, 2006
//
//  Copyright (C) 2006  P. Covington, N8VB
//  and Kirk Weedman KD7IRS
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

`timescale 1 ns/100 ps

module gpio_control(SCK, SI, SO, CS, GPIO);

localparam PORT1ADRREG = 6'h1; // port1 register address = 0x01
localparam PORT2ADRREG = 6'h2; // port2 register address = 0x02

input wire         SCK;        // SPI SCK from FX2
input wire         SI;         // SPI serial in from FX2
inout wire         SO;         // SPI serial out to FX2
input wire         CS;         // FPGA chip select from FX2
inout wire  [23:0] GPIO;       // OZY GPIO lines

wire   [5:0] serial_address;
wire   [7:0] serial_data;
wire         serial_wr;
						
// SPI communications module
SPI_REGS #(8,3) spi_regs(
					.SI(SI),
					.SO(SO),
					.SCK(SCK),
					.CS(CS),
					.saddr(serial_address),
					.sdata(serial_data),
					.sstrobe(serial_wr),
					.GPIO(GPIO));


// GPIO 1-8 OUTPUTS, Address = 0x01	= Least Significant byte of GPIO
gpio_oport #(PORT1ADRREG, 8) port1reg (.clk(SCK),
									.wr_strb(serial_wr),
									.addr(serial_address),
									.s_in(serial_data),
									.gpio(GPIO[7:0]) );

// GPIO 9-16 OUTPUTS, Address = 0x02					
gpio_oport #(PORT2ADRREG, 8) port2reg (.clk(SCK),
									.wr_strb(serial_wr),
									.addr(serial_address),
									.s_in(serial_data),
									.gpio(GPIO[15:8]) );

// GPIO 17-24 INPUTS, Address = 0x03 -- no output drivers needed for just inputs!				

endmodule