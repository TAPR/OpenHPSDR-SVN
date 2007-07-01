/////////////////////////////////////////////////////////////////////
////                                                             ////
//// Copyright (C) 2007 Steven Wilson                            ////
////                    ka6s@yahoo.com                           ////
//
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
// This design is LGPL software written for the HPSDR project
// by Steve Wilson, KA6S.  The author assumes no libability on how the
// code might be used or warranty that the code is fit for any 
// particular application. Further, I2C is a patented technology 
// of Philips corporation and requires a license. If you are going
// to use this in a product - get the license!
//
// This code incorporates the HPSDR I2C slave, and an open cores 
// I2C Master along with a Wishbone bus tieing the two together.
// There is a two channel wish bone master that allows for either the
// I2C slave to program the master OR the on-chip state machine (still
// to be coded) which will be able to send status info to the host
// via the I2C master using the second port on the wb_arbiter.
//
`include "timescale.v"
`define ADDRESS 7'h11

module epi (CLK, 
            reset_n,
            slv_sda, 
            slv_scl, 
            mstr_sda,
            mstr_scl,
            cross_point_ctl,
            relay_out,
            receive_in,
            oc_out 
);

input        CLK;  //48MHz from FX2 
input        reset_n;
inout        slv_sda;
input        slv_scl;
inout        mstr_sda;
inout        mstr_scl;

// GPIO sigs
output [47:0] cross_point_ctl;
output [7:0] relay_out;
input  [7:0] receive_in;
output [7:0] oc_out;

wire sda_out;
wire sda_oe;
wire dg_slv_scl;		// de-glitched scl 
wire dg_slv_sda;		// de-glitched sda		

// reset logic 


// de-glitch sda and scl using CLK
// sda and scl have to be stable for 4 consecutive CLK samples before a transition is considered valid

deglitch u_deg_scl (.rst_n(reset_n),.clock(CLK), .in(slv_scl),.out(dg_slv_scl));
deglitch2 u_deg_sda (.rst_n(reset_n),.clock(CLK), .in(slv_sda),.out(dg_slv_sda));
deglitch u_deg_mstr_scl (.rst_n(reset_n),.clock(CLK), .in(mstr_scl),.out(dg_mstr_scl));
deglitch2 u_deg_mstr_sda (.rst_n(reset_n),.clock(CLK), .in(mstr_sda),.out(dg_mstr_sda));

wire [7:0] int_reg0;
wire [7:0] int_reg1;

//I2C slave module

// Channel 0 Master WB signals
wire [7:0] adr_0;
wire [7:0] dat_0_o;
wire [7:0] dat_0_i;
wire       we_0;
wire       stb_0;
wire       ack_0;
wire       cyc_0;

// Channel 1 Master WB signals
wire [7:0] adr_1;
wire [7:0] dat_1_o;
wire [7:0] dat_1_i;
wire       we_1;
wire       stb_1;
wire       ack_1;
wire       cyc_1;

// Muxed WB signals
wire [7:0] adr;
wire [7:0] dat_o;
wire [7:0] dat_i;
wire       we;
wire       stb;
wire       slv_sel_0 = (adr[7:6] == 2'b00); // Slave 0
wire       slv_sel_1 = (adr[7:6] == 2'b01); // Slave 1
wire       slv_sel_2 = (adr[7:6] == 2'b10); // Slave 2
wire       slv_sel_3 = (adr[7:6] == 2'b11); // Slave 3 - not used
wire       slv_ack_0;
wire       slv_ack_1;
wire       slv_ack_2;
wire       ack = slv_sel_0 & slv_ack_0 |
                 slv_sel_1 & slv_ack_1 |     
                 slv_sel_2 & slv_ack_2;     

wire [7:0] slv_0_dout;
wire [7:0] slv_1_dout;
wire [7:0] slv_2_dout;

// Mux the data read from each of the slaves
assign dat_i = slv_sel_0 ? slv_0_dout :
               slv_sel_1 ? slv_1_dout :
               slv_sel_2 ? slv_2_dout :
                           8'h0;

// Microcode bus 
wire [2:0] mc_addr;
wire [15:0] mc_word;

wire go_ahead;
wire [47:0] cross_point_ctl;
wire [7:0] relay_out;
wire [7:0] receive_in;
wire [7:0] oc_out;
wire [7:0] stat_in;

// 
// I2C Slave
// 
// Used to program up rest of device
//
i2c_slave u_i2c_slave (
             .clk(CLK),
             .rst_n(reset_n),
             .my_addr(`ADDRESS),
             .sda_i(dg_slv_sda),
             .sda_out(slv_sda_out),
             .sda_oe(slv_sda_oe),
             .scl_in(dg_slv_scl),

             // WB Interface
             .adr_o(adr_0),
             .dat_i(dat_0_i),
             .dat_o(dat_0_o),
             .we_o(we_0),
             .sel_o(),  // Not used
             .stb_o(stb_0),
             .ack_i(ack_0),
             .cyc_o(cyc_0), 
	     .debug()
);


// Tristate drivers for I2C 
assign slv_sda = slv_sda_oe ? slv_sda_out : 1'bz;
wire mstr_scl_oe, mstr_scl_out;
wire mstr_sda_oe, mstr_sda_out;
assign mstr_scl = ~mstr_scl_oe ? mstr_scl_out : 1'bz;
assign mstr_sda = ~mstr_sda_oe ? mstr_sda_out : 1'bz;

// OpenCores I2C Master - 
// Wishbone slave addr 0-7
// Wishbone Slave 1
i2c_master_top u_i2c_master (
        // Wishbone in
        .wb_clk_i(CLK), 
        .wb_rst_i(~reset_n), 
        .arst_i(reset_n), 
        .wb_adr_i(adr[2:0]), 
        .wb_dat_i(dat_o), 
        .wb_dat_o(slv_1_dout),
        .wb_we_i(we), 
        .wb_stb_i(slv_sel_1), 
        .wb_cyc_i(cyc), 
        .wb_ack_o(slv_ack_1), 
        .wb_inta_o(i2c_int),

        // Master I2C path
        .scl_pad_i(dg_mstr_scl), 
        .scl_pad_o(mstr_scl_out), 
        .scl_padoen_o(mstr_scl_oe), 
        .sda_pad_i(dg_mstr_sda), 
        .sda_pad_o(mstr_sda_out), 
        .sda_padoen_o(mstr_sda_oe)
       );
 

//
// Status State Machine
//
status_ctl u_status_ctl (
              // Misc
        .clk(CLK),
        .rst_n(reset_n),
              
        // Status interface 
        .go_ahead(go_ahead),
        .stat_in(stat_in),

        // Microstore Interface
        .mc_word(mc_word),
        .mc_addr(mc_addr),
              
        // Wishbone
        .addr_o(adr_1[2:0]),
        .dat_o(dat_1_o),
        .dat_i(dat_1_i),
        .we_o(we_1),
        .stb_o(stb_1),
        .cyc_o(cyc_1),
        .ack_i(ack_1),
        .int(i2c_int)
);

// 
// Microcode Store
// 
// WB slave 0

status_mcode u_status_mcode (
        // Housekeeping
        .clk(CLK),
        .rst_n(reset_n),
     
        // Wishbone
        .addr_i(adr[3:0]),
        .dat_i(dat_o),
        .dat_o(slv_0_dout),
        .sel_i(slv_sel_0 & stb),
        .cyc_i(cyc),
        .we_i(we),
        .ack_o(slv_ack_0),

        .mc_addr(mc_addr),
        .mc_word(mc_word)
);

// Wishbone Slave 2 - EPI control registers
epi_ctl_regs u_epi_ctl (
        .clk(CLK),
        .rst_n(reset_n),
 
        // Wishbone
        .addr_i(adr[3:0]),
        .dat_i(),
        .dat_o(slv_2_dout),
        .sel_i(slv_sel_2 & stb),
        .cyc_i(cyc),
        .we_i(we),
        .ack_o(slv_ack_2),
       
        // External Pin I/O
        .go_ahead(go_ahead), // Received status change
        .cross_point_ctl(cross_point_ctl),
        .relay_out(relay_out),
        .receive_in(receive_in),
        .oc_out(oc_out),
        .stat_in(stat_in) 
);

// Simple Wishbone/Arbiter Mux

wb_arb_mux u_wb_arb_mux (
        .wb_clk_i(CLK),
        .wb_rst_i(reset_n),

        // Channel 0 WB Interface
        .adr_0_i(adr_0),
        .dat_0_i(dat_0_o),
        .dat_0_o(dat_0_i),
        .we_0_i(we_0),
        .sel_0_i(1'b1),
        .stb_0_i(stb_0),
        .ack_0_o(ack_0),
        .cyc_0_i(cyc_0), 

        // Channel 1 WB Interface
        .adr_1_i({5'b0100_0,adr_1[2:0]}),
        .dat_1_i(dat_1_o),
        .dat_1_o(dat_1_i),
        .we_1_i(we_1),
        .sel_1_i(1'b1),
        .stb_1_i(stb_1),
        .ack_1_o(ack_1),
        .cyc_1_i(cyc_1),

        // Muxed WB Interface to all slaves
        .adr_o(adr),
        .dat_o(dat_o),
        .dat_i(dat_i),
        .we_o(we),
        .sel_o(),    // Not really used
        .stb_o(stb),
        .cyc_o(cyc),
        .ack_i(ack)
);



endmodule
