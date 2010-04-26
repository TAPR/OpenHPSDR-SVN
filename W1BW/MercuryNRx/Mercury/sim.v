`timescale 1 ns/100 ps


// Mercury simulation

module sim ();

reg           OSC_10MHZ;      // 10MHz TCXO input 
wire          ext_10MHZ;      // 10MHz reference to/from Atlas pin C16
reg           CLKA;           // 122.88MHz clock from LT2208
reg    [15:0] INA;            // samples from LT2208
reg           CC;             // Command & Control from Atlas C20
reg           ATTRLY;         // Antenna relay control
wire          A6;             // MCLK_12MHZ (12.288MHz) to Atlas bus A6
reg           C4;             // LROUT (Rx audio) from Atlas bus
reg           CBCLK;          // CBLCK from Atlas bus
reg           CLRCLK;         // 48KHz L/R clock from Atlas C9
reg           C17;            // CLK_MCLK from Atlas bus
wire          MDOUT;          // I and Q out to Atlas bus on A10  
wire          BCLK;           // 12.288MHz from Atlas bus C6 for I2S encoder
wire          LRCLK;          // 192kHz from Atlas bus C7 for I2S encoder
wire          CDIN;           // Rx audio out to TLV320
wire          MOSI;           // SPI interface to TLV320
wire          SCLK;           // SPI interface to TLV320
wire          nCS;            // SPI interface to TLV320
wire          SPI_data;       // SPI data to Alex
wire          SPI_clock;      // SPI clock to Alex
wire          Tx_load_strobe;	// SPI Tx data load strobe to Alex
wire          Rx_load_strobe;	// SPI Rx data load strobe to Alex
wire          FPGA_PLL;       // PLL control volts to loop filter 
wire          LVDS_TXE;       // LVDS Tx enable
wire          LVDS_RXE_N;     // LVDS Rx enable
reg           OVERFLOW;       // ADC overflow bit
reg           DFS0, DFS1;     // I/Q sampling rate selection

reg RUN;


//	The data fomat is as follows:
//	
//	<1 bit PTT><4 bits address><32 bits frequency><4 bits clock_s><7 bits OC><1 bit mode> 
//	
//	for a total of 49 bits. Frequency is in Hz and OC is the open collector data on Penelope
reg    [58:0] CC_STREAM;

// Initialize Inputs
initial
begin
  RUN = 1'b0; // make _10MHZ clock wait a while
  
  // Simulate power up reset to internal registers that matter
  M1.load  = 0;
//  M1.index = 1'b0;
//  M1.tdata = 0;
  M1.SCLK = 0;
  M1.TLV_data = 0;
  M1.nCS = 0;
  M1.MOSI = 0;
  M1.bit_cnt = 0;
  M1.TLV = 0;
  M1.tlv_timeout = 0;

  // initialize necessary input pins
  CC_STREAM = 0;
  OVERFLOW = 0;
  INA = 0;
  DFS0 = 0;
  DFS1 = 0;
  C4 = 0;
  ATTRLY = 0;
  #500;
  RUN = 1'b1;
  if (OSC_10MHZ === 1'bx)
  begin
    $display ("ERROR: OSC_10MHZ clock not running");
    $stop;
  end  
  if (CLKA === 1'bx)
  begin
    $display ("ERROR: CLKA clock not running");
    $stop;
  end  
  if (CBCLK === 1'bx)
  begin
    $display ("ERROR: CBCLK clock not running");
    $stop;
  end  
  if (CLRCLK === 1'bx)
  begin
    $display ("ERROR: CLRCLK clock not running");
    $stop;
  end  

  wait (M1.TLV != 0);
  @(negedge CLRCLK);
  @(negedge CLRCLK);
  @(negedge CLRCLK);
  @(negedge CLRCLK);
  @(negedge CLRCLK);
  @(negedge CLRCLK);
  @(negedge CLRCLK);
  @(negedge CLRCLK);
  @(negedge CLRCLK);
  @(negedge CLRCLK);
  @(negedge CLRCLK);
  $display ("Succesful simulation completion");
  $stop;
end


Mercury M1 (.OSC_10MHZ(OSC_10MHZ),.ext_10MHZ(ext_10MHZ),.CLKA(CLKA),.INA(INA),
            .CC(CC),.ATTRLY(),.A6(A6),.C4(C4),.C8(CBCLK),.C9(CLRCLK),
            .C17(C17),.MDOUT(MDOUT),.BCLK(BCLK),.LRCLK(LRCLK),.CDIN(CDIN),
            .CBCLK(),.CLRCLK(),.CLRCOUT(),.CMCLK(),.CMODE(),.MOSI(MOSI),
            .SCLK(SCLK),.nCS(nCS),.SPI_data(SPI_data),.SPI_clock(SPI_clock),
            .Tx_load_strobe(Tx_load_strobe),.Rx_load_strobe(Rx_load_strobe),
            .FPGA_PLL(FPGA_PLL),.LVDS_TXE(LVDS_TXE),.LVDS_RXE_N(LVDS_RXE_N),
            .OVERFLOW(OVERFLOW),.DITHER(),.SHDN(),.PGA(),.RAND(),.INIT_DONE(),
            .TEST0(),.TEST1(),.TEST2(),.TEST3(),.DEBUG_LED0(),.DEBUG_LED1(),
            .DEBUG_LED2(),.DEBUG_LED3(),.DEBUG_LED4(),.DEBUG_LED5(),
            .DEBUG_LED6(),.DEBUG_LED7()
);


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// continuously send C & C data on the CC pin

reg [58:0] ccd;
initial
begin
  #2000;
  while(1)
  begin
    ccd <= CC_STREAM;
    @(negedge CLRCLK);
    CC_DATA(ccd);
  end
end

task CC_DATA;
input [58:0] din;
integer acnt;
begin
  @(negedge CBCLK); // 1st falling edge of CBCLK after LRCLK goes low
//  @(negedge CBCLK); // 2nd falling edge of CBCLK after LRCLK goes low
  for (acnt = 0; acnt < 59; acnt = acnt + 1)
  begin
    CC   = din[58-acnt]; // MSB first
    @(negedge CBCLK);
  end
end
endtask

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
parameter CB_SPEED = 162.8;  // CBCLK = 12.288Mhz/4 = 3.07 = 325.5nS, CB_SPEED =  325.5nS/2 = 162.8nS

initial
begin
  CBCLK = 1'b0;
  $display ("CBCLK started ");
  #66;
  wait(RUN);
  forever
  begin
    #CB_SPEED CBCLK = 1'b1;
    #CB_SPEED CBCLK = 1'b0;
  end
end

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
integer k;
initial
begin
  CLRCLK = 1'b0;
  $display ("CLRCLK started ");
  #3;
  wait(RUN);
  @ (posedge CBCLK);
  forever
  begin
    @ (negedge CBCLK);
    #15
    CLRCLK = 1'b0;
    for (k = 0; k < 32; k = k + 1)
      @ (posedge CBCLK);
    @ (negedge CBCLK);
    #15
    CLRCLK = 1'b1;
    for (k = 0; k < 32; k = k + 1)
      @ (posedge CBCLK);
  end
end

assign LRCLK = CLRCLK;
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
parameter CLK10_SPEED = 50.0;  // CLK_MCLK = 10Mhz = 100nS, CPUSPEED = 100nS/2 = 50.0nS

assign ext_10MHZ = 1'bz;

initial
begin
  OSC_10MHZ = 1'b0;
  $display ("OSC_10MHZ started ");
  #5;
  wait(RUN);
  forever
  begin
    #CLK10_SPEED OSC_10MHZ = 1'b1;
    #CLK10_SPEED OSC_10MHZ = 1'b0;
  end
end


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
parameter CLK123_SPEED = 4.07;  // IFCLK = 122.88Mhz = 8.14nS, CPUSPEED = 8.14nS/2 = 4.07nS

initial
begin
  $display ("CLKA started");
  CLKA = 1'b0;
  #15  //start at a different time than CLK_MCLK
  forever
  begin
    #CLK123_SPEED CLKA = 1'b1;
    #CLK123_SPEED CLKA = 1'b0;
  end
end
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
parameter C17_SPEED = 40.7;  // CLK_MCLK = 12.288Mhz = 81.38nS, CPUSPEED = 81.38nS/2 = 40.7nS

assign BCLK = C17;

initial
begin
  C17 = 1'b0;
  $display ("C17 (i.e. CLK_MCLK) started");
  #47;
  wait(RUN);
  forever
  begin
    #C17_SPEED C17 = 1'b1;
    #C17_SPEED C17 = 1'b0;
  end
end


endmodule