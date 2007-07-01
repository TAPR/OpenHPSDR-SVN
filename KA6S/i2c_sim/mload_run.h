              
              wr_slave(SADR1, 8'h00,8'h21);
              wr_slave_continue(8'h);
              wr_slave_continue(8'h03);
              wr_slave_continue(8'h04);
              wr_slave_continue(8'h05);
              wr_slave_continue(8'h06);
              wr_slave_continue(8'h07);
              wr_slave_finish(8'h08);

              #100000;
              read_state = 1;
	      //
	      // access slave (read)
	      //

              rd_slave(SADR1,8'h00,8'h01);
              rd_slave_continue(8'h02);
              rd_slave_continue(8'h03);
              rd_slave_continue(8'h04);
              rd_slave_continue(8'h05);
              rd_slave_continue(8'h06);
              rd_slave_continue(8'h07);
              rd_slave_finish(8'h08);


