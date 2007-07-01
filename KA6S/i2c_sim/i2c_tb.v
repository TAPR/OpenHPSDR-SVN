/////////////////////////////////////////////////////////////////////
////                                                             ////
////  WISHBONE rev.B2 compliant I2C Master controller Testbench  ////
////                                                             ////
////                                                             ////
////  Author: Richard Herveille                                  ////
////          richard@asics.ws                                   ////
////          www.asics.ws                                       ////
////                                                             ////
////  Downloaded from: http://www.opencores.org/projects/i2c/    ////
////                                                             ////
/////////////////////////////////////////////////////////////////////
////                                                             ////
//// Copyright (C) 2001 Richard Herveille                        ////
////                    richard@asics.ws                         ////
////                                                             ////
//// This source file may be used and distributed without        ////
//// restriction provided that this copyright statement is not   ////
//// removed from the file and that any derivative work contains ////
//// the original copyright notice and the associated disclaimer.////
////                                                             ////
////     THIS SOFTWARE IS PROVIDED ``AS IS'' AND WITHOUT ANY     ////
//// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED   ////
//// TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS   ////
//// FOR A PARTICULAR PURPOSE. IN NO EVENT SHALL THE AUTHOR      ////
//// OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,         ////
//// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES    ////
//// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE   ////
//// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR        ////
//// BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF  ////
//// LIABILITY, WHETHER IN  CONTRACT, STRICT LIABILITY, OR TORT  ////
//// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT  ////
//// OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE         ////
//// POSSIBILITY OF SUCH DAMAGE.                                 ////
////                                                             ////
/////////////////////////////////////////////////////////////////////

//  CVS Log
//
//  $Id: tst_bench_top.v,v 1.8 2006/09/04 09:08:51 rherveille Exp $
//
//  $Date: 2006/09/04 09:08:51 $
//  $Revision: 1.8 $
//  $Author: rherveille $
//  $Locker:  $
//  $State: Exp $
//
// Change History:
//               $Log: tst_bench_top.v,v $
//               Revision 1.8  2006/09/04 09:08:51  rherveille
//               fixed (n)ack generation
//
//               Revision 1.7  2005/02/27 09:24:18  rherveille
//               Fixed scl, sda delay.
//
//               Revision 1.6  2004/02/28 15:40:42  rherveille
//               *** empty log message ***
//
//               Revision 1.4  2003/12/05 11:04:38  rherveille
//               Added slave address configurability
//
//               Revision 1.3  2002/10/30 18:11:06  rherveille
//               Added timing tests to i2c_model.
//               Updated testbench.
//
//               Revision 1.2  2002/03/17 10:26:38  rherveille
//               Fixed some race conditions in the i2c-slave model.
//               Added debug information.
//               Added headers.
//
//               Revision 1.3  2007/03/02 7:54:00 stevew@ka6s.com
//               Extended for use in HPSDR i2C slave/Master 
//               integration. 

`include "timescale.v"

module tst_bench_top();

	//
	// wires && regs
	//
	reg  clk;
	reg  rstn;

	wire [31:0] adr;
	wire [ 7:0] dat_i, dat_o, dat0_i, dat1_i;
	wire we;
	wire stb;
	wire cyc;
	wire ack;
	wire inta;

	reg [7:0] q, qq;
        reg read_state;
        reg invalid_state;
        reg [7:0] rcv_vect;

	wire scl, scl0_o, scl0_oen, scl1_o, scl1_oen;
	wire sda, sda0_o, sda0_oen, sda1_o, sda1_oen;

        // I2C Master Registers
	parameter PRER_LO = 3'b000;
	parameter PRER_HI = 3'b001;
	parameter CTR     = 3'b010;
	parameter RXR     = 3'b011;
	parameter TXR     = 3'b011;
	parameter CR      = 3'b100;
	parameter SR      = 3'b100;
 
        // EPI Mcode bit defines
        parameter STAT_IN = 2'b01;
        parameter MCWORD  = 2'b00;       
        parameter MCWRT   = 2'b00;
        parameter MCRDC   = 2'b01;
        parameter MCLOOP  = 2'b10;
        parameter MCRUN   = 1'b0;
        parameter MCSTOP  = 1'b1; 

	parameter TXR_R   = 3'b101; // undocumented / reserved output
	parameter CR_R    = 3'b110; // undocumented / reserved output

	parameter RD      = 1'b1;
	parameter WR      = 1'b0;
	parameter SADR    = 7'b0010_000;
	parameter SADR1   = 7'b0010_001;
        parameter STA     = 8'b1000_0000;
        parameter STO     = 8'b0100_0000;
        parameter RDB     = 8'b0010_0000;
        parameter WRB     = 8'b0001_0000;
        parameter ACK     = 8'b0000_1000;
        parameter IACK    = 8'b0000_0001;
       
        

	//
	// Module body
	//

reg [3:0] flag;

// Task WR_SINGLE: 
//    Task writes an Address/Data pair to I2C Slave
//    input 1: Slave Address on I2C bus - should always be SADR1
//    input 2: Wishbone Address 
//    input 3: Wishbone Data
//
task wr_single;
  input [6:0] sl_addr;
  input [7:0] d_addr; 
  input [7:0] data;
 begin
   flag = 4'h1;
   u0.wb_write(1, TXR, {sl_addr,WR} ); // present slave address, set write-bit
   u0.wb_write(0, CR, 8'h90 );         // set command (start, write)
   // Wait till data xmitted
   u0.wb_read(1, SR, q);
   while(q[1])
     u0.wb_read(0, SR, q); // poll it until it is zero
   flag = 4'h2;
   // send memory address
   u0.wb_write(1, TXR,     d_addr); // present slave's memory address
   u0.wb_write(0, CR,      8'h10); // set command (write)

   // check tip bit
   u0.wb_read(1, SR, q);
   while(q[1])
     u0.wb_read(0, SR, q); // poll it until it is zero

   flag = 4'h3;
   // send memory contents
   u0.wb_write(1, TXR,     data); // present data
   u0.wb_write(0, CR, 8'h10 | STO); // set command (write)
   $display("WR_SINGLE: Slv_adr=%h, WB_AD=%h, Data=%h",sl_addr,d_addr,data);

   u0.wb_read(1, SR, q);
   while(q[1])
      u0.wb_read(0, SR, q); // poll it until it is zero
   flag = 4'h4;

   // while (scl) #1;
   // force scl= 1'b0;
   // #100000;
   // release scl;
 end
endtask

// Task WR_SLAVE: 
//    Task writes an Address/Data pair to I2C Slave
//    input 1: Slave Address on I2C bus - should always be SADR1
//    input 2: Wishbone Address 
//    input 3: Wishbone Data
//

task wr_slave;
  input [6:0] sl_addr;
  input [7:0] d_addr; 
  input [7:0] data;
 begin
   flag = 4'h1;
   u0.wb_write(1, TXR, {sl_addr,WR} ); // present slave address, set write-bit
   u0.wb_write(0, CR, 8'h90 );         // set command (start, write)
   // Wait till data xmitted
   u0.wb_read(1, SR, q);
   while(q[1])
     u0.wb_read(0, SR, q); // poll it until it is zero

   // send memory address
   u0.wb_write(1, TXR,     d_addr); // present slave's memory address
   u0.wb_write(0, CR,      8'h10); // set command (write)

   // check tip bit
   u0.wb_read(1, SR, q);
   while(q[1])
     u0.wb_read(0, SR, q); // poll it until it is zero

   // send memory contents
   u0.wb_write(1, TXR,     data); // present data
   u0.wb_write(0, CR,      8'h10); // set command (write)
   $display("WR_SLAVE: Slv_adr=%h, WB_AD=%h, Data=%h",sl_addr,d_addr,data);

   u0.wb_read(1, SR, q);
   while(q[1])
      u0.wb_read(0, SR, q); // poll it until it is zero

   // while (scl) #1;
   // force scl= 1'b0;
   // #100000;
   // release scl;
 end
endtask

// Task WR_SLAVE_CONTINUE: 
//    Task writes to next address in burst write
//    input 1: Wishbone Data
//

task wr_slave_continue;
  input [7:0] data;
 begin
     flag = flag + 1;
   // check tip bit
    //u0.wb_read(1, SR, q);
    //while(q[1])
       //u0.wb_read(1, SR, q); // poll it until it is zero

    // send memory contents for next memory address (auto_inc)
    u0.wb_write(1, TXR,     data); // present data
    u0.wb_write(0, CR,      8'h10); // set command (write)

    // check tip bit
    u0.wb_read(1, SR, q);
    while(q[1])
      u0.wb_read(1, SR, q); // poll it until it is zero

   $display("WR_SLAVE_CONTINUE: Data=%h",data);

 end
endtask

// Task WR_SLAVE_FINISH: 
//    Task writes to next address in burst write
//    input 1: Wishbone Data
//

task wr_slave_finish;
  input [7:0] data;
 begin
     flag = flag + 1;
   // check tip bit
    //u0.wb_read(1, SR, q);
    //while(q[1])
       //u0.wb_read(1, SR, q); // poll it until it is zero

    // send memory contents for next memory address (auto_inc)
    u0.wb_write(1, TXR,     data); // present data
    u0.wb_write(0, CR, STO | WRB ); // set command (write)

    // check tip bit
    u0.wb_read(1, SR, q);
    while(q[1])
      u0.wb_read(1, SR, q); // poll it until it is zero

   $display("WR_SLAVE_FINISH: Data=%h",data);

 end
endtask


// Task RD_SLAVE: 
//    Task read from Address/Data pair on I2C Slave and validates
//    input 1: Slave Address on I2C bus - should always be SADR1
//    input 2: Wishbone Address 
//    input 3: Wishbone Data - used for comparison
//
task rd_slave;
  input [6:0] sl_addr;
  input [7:0] d_addr; 
  input [7:0] data; 
begin
  //
  // access slave (read)
  //
  // drive slave address
  flag = 1;
  u0.wb_write(1, TXR,{sl_addr,WR} ); // present slave address, set write-bit
  u0.wb_write(0, CR,     8'h90 ); // set command (start, write)

  // check tip bit
  u0.wb_read(1, SR, q);
  while(q[1])
     u0.wb_read(1, SR, q); // poll it until it is zero

  flag =2;
  // send memory address
  u0.wb_write(1, TXR,     d_addr); // present slave's memory address
  u0.wb_write(0, CR,      8'h10); // set command (write)

  // check tip bit
  u0.wb_read(1, SR, q);
  while(q[1])
    u0.wb_read(1, SR, q); // poll it until it is zero

   flag = 3;
   // drive slave address
   u0.wb_write(1, TXR, {SADR1,RD} ); // present slave's address, set read-bit
   u0.wb_write(0, CR,  8'h90 ); // set command (start, write)

   // check tip bit
   u0.wb_read(1, SR, q);
   while(q[1])
      u0.wb_read(1, SR, q); // poll it until it is zero

// SDW
   flag = 4;
   // read data from slave
   u0.wb_write(1, CR,      8'h20); // set command (read, ack_read)
   //u0.wb_write(1, CR, RDB | ACK );   // set command (read, ack_read)

   // check tip bit
   u0.wb_read(1, SR, q);
   while(q[1])
     u0.wb_read(1, SR, q); // poll it until it is zero

   // check data just received
   u0.wb_read(1, RXR, qq);
   if(qq !== data)
	$display("\n*** ERROR: RD_SLAVE: Expd = %h, Rcvd = %h",data,qq);
   else
        $display("RD_SLAVE: Rcvd = %h",qq);

end
endtask 

//
// Task RD_SLAVE_CONTINUE: 
//    Task read from I2C_SLAVE AFTER RD_SLAVE has been run. This allows
//       burst reads.
//    input 3: Wishbone Data - used for comparison
//
task rd_slave_continue;
  input [7:0] data;
  begin
   // read data from slave
    u0.wb_write(1, CR,      8'h20); // set command (read, ack_read)

   // check tip bit
   u0.wb_read(1, SR, q);
   while(q[1])
	u0.wb_read(1, SR, q); // poll it until it is zero

   // check data just received
   u0.wb_read(1, RXR, qq);
   if(qq !== data)
	$display("\n*** ERROR: RD_SLAVE_CONTINUE: Expd = %h, Rcvd = %h",data,qq);
   else
        $display("RD_SLAVE_CONTINUE: Rcvd = %h",qq);
  end
endtask

//
// Task RD_SLAVE_FINISH: 
//    Task read from I2C_SLAVE AFTER RD_SLAVE has been run. This allows
//       burst reads.
//    input 3: Wishbone Data - used for comparison
//
task rd_slave_finish;
  input [7:0] data;
  begin
   // read data from slave
    u0.wb_write(1, CR, RDB | STO ); // set command (read, ack_read)

   // check tip bit
   u0.wb_read(1, SR, q);
   while(q[1])
	u0.wb_read(1, SR, q); // poll it until it is zero

   // check data just received
   u0.wb_read(1, RXR, qq);
   if(qq !== data)
	$display("\n*** ERROR: RD_SLAVE_FINISH: Expd = %h, Rcvd = %h",data,qq);
   else
        $display("RD_SLAVE_FINISH: Rcvd = %h",qq);
  end
endtask

	// generate clock
	always #5 clk = ~clk;

	// hookup wishbone master model
	wb_master_model #(8, 32) u0 (
		.clk(clk),
		.rst(rstn),
		.adr(adr),
		.din(dat_i),
		.dout(dat_o),
		.cyc(cyc),
		.stb(stb),
		.we(we),
		.sel(),
		.ack(ack),
		.err(1'b0),
		.rty(1'b0)
	);

	wire stb0 = stb & ~adr[3];
	wire stb1 = stb &  adr[3];

	assign dat_i = ({{8'd8}{stb0}} & dat0_i) | ({{8'd8}{stb1}} & dat1_i);

	// hookup wishbone_i2c_master core
	i2c_master_top i2c_top (

		// wishbone interface
		.wb_clk_i(clk),
		.wb_rst_i(1'b0),
		.arst_i(rstn),
		.wb_adr_i(adr[2:0]),
		.wb_dat_i(dat_o),
		.wb_dat_o(dat0_i),
		.wb_we_i(we),
		.wb_stb_i(stb0),
		.wb_cyc_i(cyc),
		.wb_ack_o(ack),
		.wb_inta_o(inta),

		// i2c signals
		.scl_pad_i(scl),
		.scl_pad_o(scl0_o),
		.scl_padoen_o(scl0_oen),
		.sda_pad_i(sda),
		.sda_pad_o(sda0_o),
		.sda_padoen_o(sda0_oen)
	),

        i2c_top2 (

                // wishbone interface
                .wb_clk_i(clk),
                .wb_rst_i(1'b0),
                .arst_i(rstn),
                .wb_adr_i(adr[2:0]),
                .wb_dat_i(dat_o),
                .wb_dat_o(dat1_i),
                .wb_we_i(we),
                .wb_stb_i(stb1),
                .wb_cyc_i(cyc),
                .wb_ack_o(ack),
                .wb_inta_o(inta),

                // i2c signals
                .scl_pad_i(scl),
                .scl_pad_o(scl1_o),
                .scl_padoen_o(scl1_oen),
                .sda_pad_i(sda),
                .sda_pad_o(sda1_o),
                .sda_padoen_o(sda1_oen)
        );


	// hookup i2c slave model
/*
	i2c_slave_model #(SADR) i2c_slave (
		.scl(scl),
		.sda(sda)
	);
*/

        wire [11:0] debug;
        buf u_buf0 (slave_scl,scl);
        buf u_buf1 (slave_sda,sda);
        
	initial
	  begin
              flag = 0;
              rcv_vect = 0;
              read_state = 0;
              invalid_state = 0;
//	      force i2c_slave.debug = 1'b1; // enable i2c_slave debug information
//	      force i2c_slave.debug = 1'b0; // disable i2c_slave debug information

	      $display("\nstatus: %t Testbench started\n\n", $time);

	      //$dumpfile("bench.vcd");
	      //$dumpvars(0, tst_bench_top);
              //$recordvars();

	      // initially values
	      clk = 0;

	      // reset system
	      rstn = 1'b1; // negate reset
	      #2;
	      rstn = 1'b0; // assert reset
	      repeat(1) @(posedge clk);
	      rstn = 1'b1; // negate reset

	      $display("status: %t done reset", $time);

	      @(posedge clk);
          end

wire mstr_sda;
wire mstr_scl;
pullup(mstr_sda);
pullup(mstr_scl);

        epi hpsdrr_i2c_slave (
           .CLK(clk),
           .reset_n(rstn),
           .slv_sda(sda),
           .slv_scl(scl),
           .mstr_sda(mstr_sda),
           .mstr_scl(mstr_scl),
           .cross_point_ctl(),
           .relay_out(),
           .receive_in(rcv_vect),
           .oc_out() 
        );

       i2c_slave_model #(7'b001_1000) i2c_slave_2 (
                .scl(mstr_scl),
                .sda(mstr_sda)
        );

        // Decode hpsdr i2c_slave
        wire slv_scl_in = debug[11];
        wire slv_target = debug[10];
        wire slv_stop   = debug[9];
        wire slv_start  = debug[8];
        wire [3:0] slv_rcv_cntr = debug [7:4];
        wire slv_sda_oe = debug [3];
        wire  [2:0] slv_i2c_sm = debug [2:0];

        // create i2c lines
	delay m0_scl (scl0_oen ? 1'bz : scl0_o, scl),
	      m1_scl (scl1_oen ? 1'bz : scl1_o, scl),
	      m0_sda (sda0_oen ? 1'bz : sda0_o, sda),
	      m1_sda (sda1_oen ? 1'bz : sda1_o, sda);

	pullup p1(scl); // pullup scl line
	pullup p2(sda); // pullup sda line

        `include "testcase.tst"

endmodule

module delay (in, out);
  input  in;
  output out;

  assign out = in;

  specify
    (in => out) = (600,600);
  endspecify
endmodule


