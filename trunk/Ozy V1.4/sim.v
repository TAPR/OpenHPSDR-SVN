`timescale 1 ns/100 ps


// Ozy_Janus simulation

module sim ();

reg                 CLK_12MHZ;      // 12.288MHz clock from Janus
reg                 IF_clk;          // FX2 IFCLOCK - 48MHz
wire                CLK_MCLK;       // Master Clock to Altas for other boards
wire                DOUT;           // Data from AK5394A
wire                MDOUT;          // I&Q data from Mercury 
wire                BCLK;           // AK5394A SCLK
wire                LRCLK;          // AK5394A LRCK
wire         [15:0] FX2_FD;         // bidirectional FIFO data to/from the FX2

wire                FLAGA;
reg                 FLAGB;
wire                FLAGC;
wire                SLWR;           // FX2 write - active low
wire                SLRD;           // FX2 read - active low
wire                SLOE;           // FX2 data bus enable - active low

wire                PKEND;
wire          [1:0] FIFO_ADR;       // FX2 register address
wire                DEBUG_LED0;     // LEDs on OZY board
wire                DEBUG_LED1;
wire                DEBUG_LED2;
wire                DEBUG_LED3;
wire                J_LR_data;      // C4 from Ozy to Janus - I2S format
wire                J_IQ_data;      // C12 from Ozy to Janus - I2S format
wire                P_IQ_sync;      // C22 from Penelope
wire                M_LR_sync;      // C23 from Mercury
wire                P_IQ_data;      // Rx data to TLV320AIC23B
wire                M_LR_data;      // Left  and Right audio data in I2S format to Atlas
wire                CDOUT;          // A/D data from TLV320AIC23B
reg                 PTT_in;         // PTT active high
wire                DFS0;           // used to set AK5394A speed
wire                DFS1;           // ditto 

wire                AK_reset;       // reset for AK5394A
wire                C48_clk;        // 48MHz clock to Janus for PWM DACs 
wire                CC;             // Command and Control data to Atlas bus 
wire                CDOUT_P;        // Mic data from Penelope

// interface lines for GPIO control 
wire                FX2_CLK;        // master system clock from FX2 
reg                 SPI_SCK;        // SPI SCK from FX2
reg                 SPI_SI;         // SPI serial in from FX2
wire                SPI_SO;         // SPI serial out to FX2
reg                 SPI_CS;         // FPGA chip select from FX2
wire         [23:0] GPIO;           // OZY GPIO lines
wire                GPIO_nIOE;      // enable GPIO driver chips 

// interface pins for JTAG programming via Atlas bus
reg                 FX2_PE0;        // Port E on FX2
wire                FX2_PE1;
reg                 FX2_PE2;
reg                 FX2_PE3;
wire                TDO;            // A27 on Atlas 
reg                 SDOBACK;        // A25 on Atlas
wire                TCK;            // A24 on Atlas
wire                TMS;            // A23 on Atlas

// internal simulation signals
reg           [7:0] S_data;         // captured SPI_SO data;
reg          [15:0] USB_RD_DATA;    // returned by USB_RD
reg          [23:0] ak5394A_ldata;
reg          [23:0] ak5394A_rdata;
reg          [23:0] AK5394A_LD, AK5394A_RD;
reg          [15:0] j;
/*
localparam AK_LD_INIT = 24'h000156;    // can be random values
localparam AK_RD_INIT = 24'h000007;
localparam AK_LD_INC  = 24'h000073;
localparam AK_RD_INC  = 24'h000009;
*/
localparam AK_LD_INIT = 24'h800000;    // can be random values
localparam AK_RD_INIT = 24'h800001;
localparam AK_LD_INC  = 24'h000002;
localparam AK_RD_INC  = 24'h000002;

reg    [1:0] blinkybits;
wire         led0_off;

wire  [15:0] rcv_data;
wire         rcv_rdy;
reg          M_rst;
reg          MP_SEND;

/*
initial
begin
  $dumpvars(0, sim);
end
*/


// Initialize Inputs
initial
begin
  OJ.IF_count  = 0;  // kick start a reset to the system
  FLAGB     = 0;
  PTT_in    = 0;        // PTT active high
  SPI_SCK   = 0;        // SPI SCK from FX2
  SPI_SI    = 0;        // SPI serial in from FX2
  SPI_CS    = 0;        // FPGA chip select from FX2
  FX2_PE0   = 0;        // Port E on FX2
  FX2_PE2   = 0;
  FX2_PE3   = 0;
  SDOBACK   = 0;        // A25 on Atlas
  M_rst     = 1'b1;
  MP_SEND   = 1'b0;
  #500
  M_rst = 1'b0;

  force OJ.IF_count = {28{1'b1}}; // trick AK_reset into thinking IF_count has counted up 2^28
  force OJ.conf = 2'b10; // force it to select Mercury MDOUT and not Janus DOUT
  force OJ.mic = 1'b1; // force it to select Penelope mic and not Janus mic
  #100;
  release OJ.IF_count;

  #100;
  if (AK_reset != 1)
  begin
    $display ("ERROR: AK_reset should have gone high");
    $stop;
  end
  #200
  force OJ.IF_clock_s = 4'b0100; // have to do this after reset so it doesnt go back to 0 when released
//  #100
//  release OJ.IF_clock_s;

  MP_SEND = 1'b1;  // flag to start sending MDOUT and CDOUT_P data.
  // See code driving NWire_wmit modules named PJDOUT and MDIQ


/*
  blinkybits = 0;
  @(posedge CLK_MCLK)
  blinkybits = 2'b01;
  @(posedge CLK_MCLK)
  blinkybits = 2'b10;
  @(posedge CLK_MCLK)
  blinkybits = 2'b01;
  #60000;// $stop;
*/

  SEND_USB_DATA; // Simulate sending a block of 512 bytes from PowerSDR to Ozy's FPGA

  GPIO_SPI_TEST;
 
  $display("MDOUT/CDOUT_P data that was written into TX FIFO will now be checked as its taken out");
  GET_USB_DATA; // Now let's take the data out of the USB and see if everything is OK.

  #50000
  $display ("Succesful simulation completion");
  $stop;
end

reg  CLK_122MHz;
wire M_LR_sync_fall, P_IQ_sync_fall;
// simulate M_LR_sync from Mercury (C23) and Penelope (C22)
clk_lrclk_gen clrgen (.reset(M_rst), .CLK_IN(CLK_122MHz), // run at 48KHz for simulation
                      .LRCLK(M_LR_sync), .LRfall(M_LR_sync_fall), .Speed(2'b00));

assign P_IQ_sync_fall = M_LR_sync_fall;
assign P_IQ_sync = M_LR_sync;              // for now for this test

// simulate sending Mercury serial no, ADC_OVERLOAD
wire A6;
NWire_xmit  #(.DATA_BITS(9), .ICLK_FREQ(122880000), .XCLK_FREQ(122880000), .SEND_FREQ(150000),
              .NUM_DLY_CLKS (50)) 
    M_ser_no (.irst(M_rst), .iclk(CLK_122MHz), .xrst(SYS_rst), .xclk(CLK_122MHz),
              .xdata(9'h0_A5), .xreq(1'b1), .xrdy(), .xack(), .dout(A6));

led_blinker #(2, 20) BLINK (.clk(IF_clk), .err_sigs(blinkybits), .led_off(led0_off));

//cy7c68013_usb (.ifclk(IF_clk),.flaga(FLAGA),.flagb(FLAGB),.flagc(FLAGC),
//               .data(FX2_FD),.sloe_n(SLOE),.slrd_n(SLRD),.slwr_n(SLWR));

Ozy_Janus OJ (
        .IF_clk(IF_clk), .FX2_FD(FX2_FD), .FLAGA(FLAGA),
        .FLAGB(FLAGB), .FLAGC(FLAGC), .SLWR(SLWR), .SLRD(SLRD),
        .SLOE(SLOE), .PKEND(PKEND), .FIFO_ADR(FIFO_ADR),
        .DOUT(DOUT),  .A5(), .A6(A6), .A12(), .C4(J_LR_data), .C5(CLK_12MHZ), .C6(), .C7(), .C8(), .C9(),
        .C12(J_IQ_data), .C13(DFS0), .C14(DFS1), .C19(P_IQ_data), .C21(), .C22(P_IQ_sync),
        .C23(M_LR_sync), .C24(M_LR_data), .CDOUT(CDOUT),
        .CDOUT_P(CDOUT_P), .PTT_in(PTT_in), .AK_reset(AK_reset),
        .DEBUG_LED0(DEBUG_LED0), .DEBUG_LED1(DEBUG_LED1),
        .DEBUG_LED2(DEBUG_LED2), .DEBUG_LED3(DEBUG_LED3),
        .C48_clk(C48_clk), .CC(CC), .MDOUT(MDOUT),
        /*.FX2_CLK(FX2_CLK),*/ .SPI_SCK(SPI_SCK), .SPI_SI(SPI_SI),
        .SPI_SO(SPI_SO), .SPI_CS(SPI_CS), .GPIO_OUT(GPIO[15:0]), .GPIO_IN(GPIO[23:16]),
        .GPIO_nIOE(GPIO_nIOE), /*.FX2_PE0(FX2_PE0),*/ .FX2_PE1(FX2_PE1),
        /*.FX2_PE2(FX2_PE2), .FX2_PE3(FX2_PE3),*/ .SDOBACK(SDOBACK)/*, .TDO(TDO), .TCK(TCK), .TMS(TMS)*/ );

    
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//  GPIO test and SPI
//
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
task GPIO_SPI_TEST;
begin
  #20000;  // manually check LRCLK and BCLK waveforms here
  SPI_SI = 1'b0;
  SPI_CS = 1'b0;
  #500;
  SPI_SEND(8'b0_0_000010, 8'hc3); // write port 2 --> {use data port, write, address}, data
  SPI_SEND(8'b0_0_000001, 8'hAA); // write port 1
//  SPI_SEND(8'b0_0_000000, 8'hAA); // write port 0
//  SPI_SEND(8'b1_0_000000, 8'hFF); // write port 2 --> {use direction port, write, address}, data
//  SPI_SEND(8'b1_0_000001, 8'hFF); // write port 1
//  SPI_SEND(8'b1_0_000010, 8'hFF); // write port 0
  SPI_SEND(8'b0_1_000001, 8'hFF); // read port 1 data register
  if (S_data !== 8'hAA)
  begin
    $display ("ERROR: Invalid Port#2 read of %4h. Should be 8'hAA", S_data);
    $stop;
  end
/*
  SPI_SEND(8'b1_1_000001, 8'hff); // read port 2 direction register
  if (S_data != 8'hff)
  begin
    $display ("ERROR: Invalid Port#2 read of %4h. Should be 8'hff", S_data);
    $stop;
  end
*/
end
endtask
// ---- SPI_REGS Interface test-------
///////////////////////////////////////
parameter SCK_SPEED = 123;

initial  // test the SPI_REGS module at the same time
begin
  $display ("SPI_SCK started");
  SPI_SCK = 1'b0;
  #37  //start at a different time than FX2_CLK
  forever
  begin
    #SCK_SPEED SPI_SCK = 1'b1;
    #SCK_SPEED SPI_SCK = 1'b0;
  end
end
assign FX2_CLK = CLK_12MHZ;

///////////////////////////////////////
task SPI_SEND;
input [7:0] addr;
input [7:0] data;
integer i;
begin
  for (i = 0; i < 8; i = i + 1)  // first address
  begin
    @(posedge SPI_SCK)
    #7
    SPI_CS   = 1'b1;
    SPI_SI   = addr[7-i];
  end
  for (i = 0; i < 8; i = i + 1) // then data if in write mode
  begin
    @(posedge SPI_SCK)
    #7;
    SPI_CS   = 1'b1;
    SPI_SI   = addr[6] ? 1'bz : data[7-i];
    @(negedge SPI_SCK)
    #7;
    S_data[7-i] = SPI_SO;
 end
  @(posedge SPI_SCK);
  #7;
  SPI_CS   = 1'b0;
  @(posedge SPI_SCK);
  @(posedge SPI_SCK);
end
endtask

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//  Simulate sending a block of data (512 bytes) from PowerSDR to Ozy's FPGA
//
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
task SEND_USB_DATA;
begin: SUD
  reg [15:0] usbcnt;
  
  // Now send 512 bytes of PWM data
  Write_USB(16'h7f7f);
  Write_USB(16'h7f00); // Rx_control_0[7:1] = 0
  Write_USB(16'h0000);
  Write_USB(16'h0000); // DFS1, DFS0 are in here somewhere and I want 0,0 = 48kHz
  for (usbcnt = 0; usbcnt < 63; usbcnt = usbcnt + 1)
  begin
    Write_USB(usbcnt);  // Left Data
    Write_USB(usbcnt+16'h0100);  // Right Data
    Write_USB(usbcnt+16'h0200);  // I_PWM
    Write_USB(usbcnt+16'h0300);  // Q_PWM
  end
end
endtask
///////////////////////////////////////
reg [15:0] USB_RDMEM [0:1023];  // USB data to PC
reg [9:0] usb_rdinptr;  // FX data written into USB (SLWR)
reg [9:0] usb_rdoutptr; // USB data to PC

reg [15:0] USB_WRMEM [0:1023];  // PC data to USB
reg [9:0] usb_wrinptr;  // PC to USB data
reg [9:0] usb_wroutptr; // USB to FX2 (SLRD) data

///////////////////////////////////////
reg [9:0] tcnt, iptr;
wire [9:0] tmp;
reg oflg;

task Read_USB;
begin
  wait(tcnt != 0);
  @(posedge IF_clk);
  USB_RD_DATA <= #1 USB_RDMEM[usb_rdoutptr];
  usb_rdoutptr <= #1 usb_rdoutptr + 1;
  oflg = 1'b1;
  @(posedge IF_clk);
  oflg = 1'b0;
end
endtask

always @(posedge OJ.IF_rst)
begin
  tcnt <= 0;
  iptr <= 0;
  tcnt <= 0;
  oflg <= 0;
  usb_rdinptr  <= 0;
  usb_rdoutptr <= 0;
  usb_wrinptr  <= 0;
  usb_wroutptr <= 0;
end

assign tmp = iptr + 1;

//assign FLAGC = ((tcnt == 0) || (tmp != usb_rdoutptr)) ? 1'b1 : 1'b0; // set EP6 ready (RDMEM[] not full)
assign FLAGC = (tcnt <= 1023);

always @(posedge SLWR)
begin
  if (!OJ.IF_rst)
    tcnt <= #1 tcnt + 1;
end
always @(posedge oflg)
begin
  if (!OJ.IF_rst)
    tcnt <= #1 tcnt - 1;
end

always @(posedge SLWR)
begin
  if (!OJ.IF_rst)
  begin
    USB_RDMEM[usb_rdinptr] <= #1 {FX2_FD[7:0], FX2_FD[15:8]};
    usb_rdinptr <= #1 usb_rdinptr + 1'b1;
  end
  else
    usb_rdinptr <= #1 0;
end
///////////////////////////////////////
task Write_USB;
input [15:0] wdata;
begin
  @(posedge IF_clk);
  if (!OJ.IF_rst)
  begin
    USB_WRMEM[usb_wrinptr] <= #1 {wdata[7:0], wdata[15:8]}; // PC data is actually reversed going to USB
    usb_wrinptr <=  #1 usb_wrinptr + 1'b1;
  end
  else
    usb_wrinptr <= #1 0;
  @(posedge IF_clk);
end
endtask

assign FLAGA = (usb_wrinptr != usb_wroutptr);

always @(negedge SLRD)
begin
  #4.5 usb_wroutptr <= usb_wroutptr + 1'b1;
end

assign #10.5 FX2_FD = !SLOE ? USB_WRMEM[usb_wroutptr] : 16'bz;


//////////////////////////////////////////////////////////////////////////////////////////
//
// Now let's take the data out of the USB and see if everything is OK.
//
//////////////////////////////////////////////////////////////////////////////////////////
task GET_USB_DATA;
begin: GUD
  reg [15:0] tlvd;

  Read_USB();  // sync word
  if (USB_RD_DATA !== 16'h7F7F)
  begin
    $display ("GET_USB_DATA ERROR: Invalid Sync Data of %16h. Should be 16'h7F7F", USB_RD_DATA);
    #1000; $stop;
  end
  Read_USB();  // sync word, info
  if (USB_RD_DATA !== {16'h7F,8'b0000_00xx})
  begin
    $display ("GET_USB_DATA ERROR: Invalid Sync Data of %16h. Should be 16'h7F0X", USB_RD_DATA);
    #1000; $stop;
  end
  Read_USB();  //
  if (USB_RD_DATA !== {7'b0,9'bx_xxxx_xxxx})
  begin
    $display ("GET_USB_DATA ERROR: Invalid Control Data of %16h. Should be 16'h0XXX", USB_RD_DATA);
    #1000; $stop;
  end
  Read_USB();  //
  if (USB_RD_DATA !== 16'hxx0C)
  begin
    $display ("GET_USB_DATA ERROR: Invalid Control Data of %16h. Should be 16'hxx0C", USB_RD_DATA); // 16 x 4096 FIFO
    #1000; $stop;
  end
  AK5394A_LD   = AK_LD_INIT;
  AK5394A_RD   = AK_RD_INIT;
  for (j = 0; j < 63; j = j + 1)
  begin
    Read_USB();  // AK5394A LData [23:8]
    ak5394A_ldata[23:8] = USB_RD_DATA;
    Read_USB();  // AK5394A LData [7:0], AK5394A RData [23:16]
    ak5394A_ldata[7:0] = USB_RD_DATA[15:8];
    ak5394A_rdata[23:16] = USB_RD_DATA[7:0];
    Read_USB();  // AK5394A LData [15:0]
    ak5394A_rdata[15:0] = USB_RD_DATA;
    Read_USB();  // TLV320  LData [15:0]
    if (ak5394A_ldata !== AK5394A_LD)
    begin
      $display ("GET_USB_DATA ERROR: %d: Invalid AK5394A LData read of %16h. Should be %16h", j, ak5394A_ldata, AK5394A_LD);
      #1000; $stop;
    end
    if (ak5394A_rdata !== AK5394A_RD)
    begin
      $display ("GET_USB_DATA ERROR: %d: Invalid AK5394A RData read of %16h. Should be %16h", j, ak5394A_rdata, AK5394A_RD);
      #1000; $stop;
    end
    tlvd = j;
    if (USB_RD_DATA !== tlvd)
    begin
      $display ("GET_USB_DATA ERROR: %d: Invalid TLV320 LData read of %16h. Should be %16h", j, USB_RD_DATA, tlvd);
      #1000; $stop;
    end

    AK5394A_LD   = AK5394A_LD + AK_LD_INC;
    AK5394A_RD   = AK5394A_RD + AK_RD_INC;
  end
end
endtask

//////////////////////////////////////////////////////////////////////////////////////////
//
// Simulate sending CDOUT_P data from Penelope to Ozy
//
//////////////////////////////////////////////////////////////////////////////////////////
reg  [31:0] P_XDATA;
reg         P_XREQ;
wire        P_XRDY;
wire        P_XACK;

reg  [15:0] tc;

initial
begin
  P_XREQ  = 1'b0;
  #1000;
  wait (MP_SEND);

  wait (P_IQ_sync_fall);
  wait (P_XRDY);
  P_XDATA = 32'b0; // This first data is needed by receivers to synchronize to
  P_XREQ  = 1'b1;  // This first data never gets saved by receiver, but must be sent
  wait (P_XACK);
  P_XREQ  = 1'b0;

  for (tc = 0; tc < 64; tc = tc + 1)
  begin
    wait (P_IQ_sync_fall);
    wait (P_XRDY);
    P_XDATA = {tc, tc};
    P_XREQ  = 1'b1;
    wait (P_XACK);
    P_XREQ  = 1'b0;
  end
end

NWire_xmit #(.SEND_FREQ(192000), .DATA_BITS(32), .ICLK_FREQ(122880000), .XCLK_FREQ(122880000),
             .LOW_TIME(1'b0), .NUM_DLY_CLKS (50))
     PJDOUT (.irst(M_rst), .iclk(CLK_122MHz), .xrst(M_rst), .xclk(CLK_122MHz),
             .xdata(P_XDATA), .xreq(P_XREQ), .xrdy(P_XRDY), .xack(P_XACK), .dout(CDOUT_P));
assign CDOUT = 1'bx; // Make sure simulation is using CDOUT_P or else we'll get this unknown for data


//////////////////////////////////////////////////////////////////////////////////////////
// simulate sending I & Q data via MDOUT from Mercury to Ozy
//////////////////////////////////////////////////////////////////////////////////////////
reg  [47:0] M_XDATA;
reg         M_XREQ;
wire        M_XRDY;
wire        M_XACK;

reg  [15:0] ac;
reg  [23:0] AK_LD;
reg  [23:0] AK_RD;
reg         ak_mark;

initial
begin
  M_XREQ  = 1'b0;
  #1000;
  AK_LD = AK_LD_INIT;
  AK_RD = AK_RD_INIT;
  wait (MP_SEND);

  wait (M_LR_sync_fall);
  wait (M_XRDY);
  M_XDATA = 48'b0; // This first data is needed by receivers to synchronize to
  M_XREQ  = 1'b1;  // This first data never gets saved by receiver, but must be sent
  wait (M_XACK);
  M_XREQ  = 1'b0;

//ak_mark = 1'b1;
  for (ac = 0; ac < 64; ac = ac + 1)
  begin
    wait (M_LR_sync_fall);
    wait (M_XRDY);
    M_XDATA = {AK_LD, AK_RD};
    M_XREQ  = 1'b1;
    wait (M_XACK);
    M_XREQ  = 1'b0;

    AK_LD   = AK_LD + AK_LD_INC;
    AK_RD   = AK_RD + AK_RD_INC;
  end
end

NWire_xmit #(.SEND_FREQ(192000), .DATA_BITS(48), .ICLK_FREQ(122880000), .XCLK_FREQ(122880000),
             .LOW_TIME(1'b0), .NUM_DLY_CLKS (50))
       MDIQ (.irst(M_rst), .iclk(CLK_122MHz), .xrst(M_rst), .xclk(CLK_122MHz),
             .xdata(M_XDATA), .xreq(M_XREQ), .xrdy(M_XRDY), .xack(M_XACK), .dout(MDOUT));
assign DOUT = 1'bx; // if we dont select the Mercury then we'll get this from Janus

//////////////////////////////////////////////////////////////////////////////////////////
//
// check all the generated TLV320 and AK5394A data going into the TX_fifo
//
//////////////////////////////////////////////////////////////////////////////////////////

integer k;
reg [15:0] pd;
reg [23:0] akld, akrd;
reg [15:0] tlvcheck;

initial
begin
  ak_mark = 1'b0;
  akld = AK_LD_INIT;
  akrd = AK_RD_INIT;
  tlvcheck = 16'bXXXX;
  #1000;
  wait (OJ.Tx_fifo_wreq);
  @(posedge IF_clk);
  if (OJ.Tx_fifo_wdata !== 16'h7F7F)
  begin
    $display("Invalid data %16h at start of stream.  Should be 16'h7F7F", OJ.Tx_fifo_wdata);
    $stop;
  end
  @(posedge IF_clk);
  if (OJ.Tx_fifo_wdata !== {8'h7F,8'b0000_00xx})
  begin
    $display("Invalid data %16h at 2nd word in stream.  Should be 16'h7F0X", OJ.Tx_fifo_wdata);
    $stop;
  end
  @(posedge IF_clk);
  if (OJ.Tx_fifo_wdata !== {7'b0,9'bx_xxxx_xxxx})
  begin
    $display("Invalid data %16h at 3rd word in stream.  Should be 16'h0XXX", OJ.Tx_fifo_wdata);
    $stop;
  end
  @(posedge IF_clk);
  if (OJ.Tx_fifo_wdata !== 16'hxx0C)
  begin
    $display("Invalid data %16h at 4th word in stream.  Should be 16'hxx0C", OJ.Tx_fifo_wdata);
    $stop;
  end
  $display("Initial SYNC bytes saved into TX FIFO are OK");
  $display("MDOUT/CDOUT_P data (akld, akrd) sent to TX FIFO will now be checked");
  
  // now check the generated data
  for (k = 0; k < 63; k = k + 1)    // 512 deep fifo - 8 (initial data) = 504.  504/8 = 63
  begin
    @(posedge IF_clk);
    $display ("akld = %0h, akrd = %0h", akld, akrd);
    @(posedge OJ.Tx_fifo_wreq);
    @(posedge IF_clk);            // AK5394A_LData[23:8]
    if (OJ.Tx_fifo_wdata !== akld[23:8])
    begin
      $display("Invalid AK5394A LDATA[23:8] = %16h at %d in stream.  Should be %16h", OJ.Tx_fifo_wdata, 4+k*4, akld[23:8]);
      ak_mark = 1'b1; #1000;
      $stop;
    end

    @(posedge IF_clk);            // AK5394A_LData[7:0], AK5394A_RData[23:16]
    if (OJ.Tx_fifo_wdata !== {akld[7:0], akrd[23:16]})
    begin
      $display("Invalid AK5394A LDATA[7:0],RDATA[23:16] = %16h at %d word in stream.  Should be %16h", OJ.Tx_fifo_wdata, 4+k*4+1, {akld[7:0], akrd[23:16]});
      ak_mark = 1'b1; #1000;
      $stop;
    end
    
    @(posedge IF_clk);            // AK5394A_RData[15:0]
    if (OJ.Tx_fifo_wdata !== akrd[15:0])
    begin
      $display("Invalid AK5394A RDATA[15:0] = %16h at %d word in stream.  Should be %16h", OJ.Tx_fifo_wdata, 4+k*4+2, akrd[15:0]);
      ak_mark = 1'b1; #1000;
      $stop;
    end
    
    @(posedge IF_clk);            // TLV320 Left Data
    pd = k;
    if (OJ.Tx_fifo_wdata !== pd)
    begin
      $display("Invalid TLV320 Left data = %16h at %d word in stream.  Should be %16h", OJ.Tx_fifo_wdata, 4+k*4+3, pd);
      ak_mark = 1'b1; #1000;
      $stop;
    end

    akld = akld + AK_LD_INC;
    akrd = akrd + AK_RD_INC;
  end
  $display("MDOUT/CDOUT_P data (akld, akrd) sent to TX FIFO are OK!");
end


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
parameter CLK122_SPEED = 4.0;  // 122.88Mhz = 8.138nS, CLK122_SPEED = 8.138nS/2 = 4.07nS

initial
begin
  CLK_122MHz = 1'b0;
  $display ("CLK_122Mhz started - NWire clock used in Mercury & Penelope");
  #7;
  forever
  begin
    #CLK122_SPEED CLK_122MHz = 1'b1;
    #CLK122_SPEED CLK_122MHz = 1'b0;
  end
end

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
parameter CLK12_SPEED = 40.7;  // CLK_MCLK = 12.288Mhz = 81.38nS, CPUSPEED = 81.38nS/2 = 40.7nS

initial
begin
  CLK_12MHZ = 1'b0;
  $display ("CLK_12Mhz started - Janus clock");
  #5;
  forever
  begin
    #CLK12_SPEED CLK_12MHZ = 1'b1;
    #CLK12_SPEED CLK_12MHZ = 1'b0;
  end
end


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
parameter IFCLK_SPEED = 10.4;  // IF_clk = 48Mhz = 20.83nS, CPUSPEED = 20.83nS/2 = 10.4nS

initial
begin
  $display ("IF_clk started = 48Mhz");
  IF_clk = 1'b0;
  #15  //start at a different time than CLK_MCLK
  forever
  begin
    #IFCLK_SPEED IF_clk = 1'b1;
    #IFCLK_SPEED IF_clk = 1'b0;
  end
end

endmodule