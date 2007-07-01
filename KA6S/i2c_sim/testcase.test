
	initial
	  begin
	      //
	      // program core
	      //

	      // program internal registers
	      u0.wb_write(1, PRER_LO, 8'hfa); // load prescaler lo-byte
	      u0.wb_write(1, PRER_LO, 8'hc8); // load prescaler lo-byte
	      u0.wb_write(1, PRER_HI, 8'h00); // load prescaler hi-byte
	      $display("status: %t programmed registers", $time);

	      u0.wb_cmp(0, PRER_LO, 8'hc8); // verify prescaler lo-byte
	      u0.wb_cmp(0, PRER_HI, 8'h00); // verify prescaler hi-byte
	      $display("status: %t verified registers", $time);

	      u0.wb_write(1, CTR,     8'h80); // enable core
	      $display("status: %t core enabled", $time);


              read_state = 0;
              invalid_state = 0;
              
              //`include "testcase.h"              
              
              // Upload I2C master parameters
              read_state = 1;
              wr_single(SADR1, 8'h40 | PRER_LO ,8'hc8);// Prescale divider init
              wr_single(SADR1, 8'h40 | PRER_HI ,8'h00);// Prescale divider init

              #10000;

              read_state = 0;
              wr_single (SADR1, 8'h40 | CTR ,8'h80);    //  Core enabled
              
              // Upload Microcode
              // Just try to send a Slave Addr cycle & 
              // Poll for transmit done.

              #10000;
              
              read_state = 1;
              wr_single(SADR1, 8'h00,8'h30);// 00
              wr_single(SADR1, 8'h01,{MCRUN,MCWRT,MCWORD,TXR});  // 00
              
              wr_single(SADR1, 8'h02, STA | WRB ); // 01
              wr_single(SADR1, 8'h03, {MCRUN,MCWRT,MCWORD,CR}); //01
              
              wr_single(SADR1, 8'h04, 8'h02);     // 02
              wr_single(SADR1, 8'h05, {MCRUN,MCRDC,MCWORD,SR}); //02
              
              wr_single(SADR1, 8'h06, 8'h0);  // Uses STAT_IN noop
              wr_single(SADR1, 8'h07,{MCRUN,MCWRT,STAT_IN,TXR}); //03
              
              wr_single(SADR1, 8'h08, STO | WRB );
              wr_single(SADR1, 8'h09, {MCRUN,MCWRT,MCWORD,CR});

              wr_single(SADR1, 8'h0A, 8'h02 );
              wr_single(SADR1, 8'h0B, {MCSTOP,MCRDC,MCWORD,SR}); //02

              rcv_vect = 8'h55;

	      #250000; // wait 250us
	      $display("\n\nstatus: %t Testbench done", $time);
	      $finish;
	  end
