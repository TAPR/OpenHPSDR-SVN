// Upload I2C master parameters
wr_slave(SADR1, 8'h40 | PRER_LO ,8'hc8);// Prescale divider init
wr_finish(8'h00);

wr_slave (SADR1, 8'h40 | CTR ,8'h80);    //  Core enabled
wr_finish(8'h00);

              
// Upload Microcode
// Just try to send a Slave Addr cycle & 
// Poll for transmit done.
//
wr_slave(SADR1, 8'h00,8'h31);// Address 18 - write
wr_slave_continue({MCRUN,MCWRT,MCWORD,TXR}); 
wr_slave_continue(STA | WRB );
wr_slave_continue({MCRUN,MCWRT,MCWORD,CR});
wr_slave_continue(8'h02);
wr_slave_finish({MCRUN,MCLOOP,MCWORD,SR});

