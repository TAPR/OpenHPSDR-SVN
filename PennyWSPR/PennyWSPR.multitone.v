// Copyright 2010 Bill Tracey KD5TFD
// Copyright 2007, 2007, 2008, 2009 Phil Harman VK6APH
// Copyright 2009 Kirk Weedman KD7IRS
//
//
//  HPSDR - High Performance Software Defined Radio
//
//  Penelope WSPR - derived from Original Penelope Exciter code.
//
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


/*
 * This is Penelope WSPR - a WSPR beacon derived from the standard Penny v2.9 exciter code.
 *
 *
 */



/*
  IMPORTANT:  In Analysis and Synthesis Settings make sure Power-Up Don't Care is NOT checked.
  Built with Quartus V9.0
*/


module PennyWSPR (
        _10MHZ,ext_10MHZ,C122_clk,
        // A11,C19,C22,
        LED2,LED3,LED4,LED5,LED6,LED7,
        USEROUT0,USEROUT1,USEROUT2,USEROUT3,USEROUT4,USEROUT5,USEROUT6,DAC,nLVDSRXE,LVDSTXE,
        FPGA_PLL,PTT,PTT_in,nCS,CMODE,
        // TLV320_CDIN,TLV320_CDOUT,TLV320_CBCLK,TLV320_CLRCIN,TLV320_CLRCOUT,TLV320_CMCLK,CC,
        ADCMOSI,ADCCLK,ADCMISO,nADCCS,
        // PWM0,PWM1,PWM2,
        FPGA_PTT, A5
        );

input  wire _10MHZ;
inout  tri  ext_10MHZ;  // 10MHz reference to Atlas pin C16
input  wire C122_clk;
// output wire A11;        // CDOUT_P (Mic) to Atlas bus
// input  wire C19;        // P_IQ_data (NWire I&Q) from Ozy
// output wire C22;        // P_IQ_sync from Penelope to Ozy
output wire LED2;
output wire LED3;
output wire LED4;
output wire LED5;
output wire LED6;
output wire LED7;
output wire USEROUT0;
output wire USEROUT1;
output wire USEROUT2;
output wire USEROUT3;
output wire USEROUT4;
output wire USEROUT5;
output wire USEROUT6;
output wire [13:0] DAC;
output wire nLVDSRXE;
output wire LVDSTXE;
output wire FPGA_PLL;
input  wire PTT;        // PTT from mic connector or DB25 pin 1
output tri  PTT_in;     // PTT on Atlas bus - C15
output wire nCS;
output wire CMODE;
// output wire TLV320_CDIN;
// input  wire TLV320_CDOUT;      // Mic or Line In data from TLV320
// output wire TLV320_CBCLK;
// output wire TLV320_CLRCIN;
// output wire TLV320_CLRCOUT;
// output wire TLV320_CMCLK;      // 12.288MHz
// input  wire CC;                // Command & Control data from Ozy C20
output wire ADCMOSI;
output wire ADCCLK;
input  wire ADCMISO;
output wire nADCCS;
// output wire PWM0;
// output wire PWM1;
// output wire PWM2;
output wire FPGA_PTT;   // controls PTT FET Q3
// output wire A5;         // serial # out to Atlas bus on A5
output wire A5;         // PPS out
wire   source_122MHZ;   // Set when internal 122.88MHz source is used and sent to LVDS

assign CMODE    = 1'b0; // Set to 0 for I2C mode
assign nCS      = 1'b1; // I2C address of TLV320 is 0x1B

localparam SERIAL = 8'd12;  // software version serial number = 1.2

localparam C122_TPD = 1.3;
//////////////////////////////////////////////////////////////
//
//              Reset
//
//////////////////////////////////////////////////////////////

reg C122_rst;
reg [10:0] C122_rst_cnt;

always @(posedge C122_clk)
begin
  if (!C122_rst_cnt[10])
    C122_rst_cnt <= #C122_TPD C122_rst_cnt + 1'b1;

  C122_rst <= #C122_TPD C122_rst_cnt[10] ? 1'b0 : 1'b1;
end

//////////////////////////////////////////////////////////////
//
//    CLOCKS
//
//////////////////////////////////////////////////////////////

localparam SPEED_48K = 2'b00;

reg        C122_DFS1, C122_DFS0;
reg        C122_cgen_rst;
reg  [1:0] C122_SPEED;
//
// create a slower system clock = 122.88Mhz / 10 = 12.288Mhz
//clk_div TLVCLK (.reset(C122_rst), .clk_in(C122_clk), .clk_out(TLV320_CMCLK));
//
// make sure CBCLK/CLRCLK and BCLK/LRCLK stay synced when speed changes
always @(posedge C122_clk)
begin
  if (C122_rst)
    C122_SPEED <= 2'b00;
 else if (C122_SPEED != {C122_DFS1,C122_DFS0})
    C122_SPEED <= {C122_DFS1,C122_DFS0};

  if (C122_rst)
    C122_cgen_rst <= 1'b1;
  else if (C122_SPEED != {C122_DFS1,C122_DFS0})
    C122_cgen_rst <= 1'b1;
  else
    C122_cgen_rst <= 1'b0;
 end

wire C122_cbrise, C122_cbfall;
wire C122_CBCLK, CLRCLK;

clk_lrclk_gen clrgen
    (.reset(C122_cgen_rst), .CLK_IN(C122_clk), .BCLK(C122_CBCLK),
     .Brise(C122_cbrise), .Bfall(C122_cbfall), .LRCLK(CLRCLK), .Speed(SPEED_48K));

//assign TLV320_CBCLK   = C122_CBCLK;
//assign TLV320_CLRCIN  = CLRCLK;
//assign TLV320_CLRCOUT = CLRCLK;

//wire LRfall;
//clk_lrclk_gen lrgen (.reset(C122_cgen_rst), .CLK_IN(C122_clk), .BCLK(), .LRCLK(),
//                      .LRfall(LRfall), .Speed({C122_DFS1,C122_DFS0}));

// Select 122.88MHz source. If source_122MHZ set then use Penelope's 122.88MHz clock and send to LVDS
// Otherwise get external clock from LVDS

assign nLVDSRXE = source_122MHZ ? 1'b1 : 1'b0;  // enable LVDS receiver if clock is external
assign LVDSTXE  = source_122MHZ ? 1'b1 : 1'b0;  // enable LVDS transmitter if Penny is the source


// select 10MHz reference source. If ref_ext is set use Penelope's 10MHz ref and send to Atlas C16

wire reference;
wire ref_ext;        // Set when internal 10MHz reference sent to Atlas C16

assign reference = ref_ext ? _10MHZ : ext_10MHZ;
assign ext_10MHZ = ref_ext ? _10MHZ : 1'bz;    // C16 is bidirectional so set high Z if input.

//
// PPS clock from 122.88 MHz clock
//
reg C122_pps; // pps clock
reg [25:0] C122_pps_count;
always @(posedge C122_clk) begin
        if ( C122_pps_count >= 61439999  ) begin
                C122_pps_count <= 0;
                C122_pps <= ~C122_pps;
        end
        else begin
                C122_pps_count <= C122_pps_count + 1;
        end
end
assign A5 = C122_pps;

//////////////////////////////////////////////////////////////
//
//    ADC SPI interface
//
//////////////////////////////////////////////////////////////

wire [11:0] AIN5;  // holds 12 bit ADC value of Forward Power detector.

ADC ADC_SPI(.clock(C122_CBCLK), .SCLK(ADCCLK), .nCS(nADCCS), .MISO(ADCMISO), .MOSI(ADCMOSI), .AIN5(AIN5));


// phase word calculation:
// phase_word = (freq_in_mhz/122.88) * (2^32)

// phase words for 20 meters
//
// 492 725 862    14097000
// 492 725 914    14097001.4648
// 492 725 905    14097002.9296
// 492 726 016    14097004.3945

// 492 729 358 14097100
// 492 729 409 14097101.4648
// 492 729 460 14097102.9297
// 492 729 511 14097104.3945

// phase words for 30 meters
//  354 422 183 10140100
//  354 422 234 10140101.4648
//  354 422 286 10140102.9297
//  354 422 337 10140104.3945

// 354 425 679  10140200
// 354 425 730  10140201.4648
// 354 425 781  10140202.92968
// 354 425 832  10140204.3945

// phase words for 40 meters
// 246 069 330  7040100
// 246 069 381 7040101.4648
// 246 069 432
// 246 069 484


reg [31:0] b30_phase_words [3:0];
reg [31:0] b20_phase_words [3:0];
reg [31:0] b40_phase_words [3:0];
// reg [31:0] b15_phase_words [3:0];

// symbol step size = 12000/8192 ~= 1.46484375 hz

// wspr freqs
//    * Band Dial freq (MHz) Tx freq (MHz)
//    * 160m 1.836600 1.838000 - 1.838200
//    * 80m 3.592600 3.594000 - 3.594200
//    * 60m 5.287200 5.288600 - 5.288800
//    * 40m 7.038600 7.040000 - 7.040200
//    * 30m 10.138700 10.140100 - 10.140300
//    * 20m 14.095600 14.097000 - 14.097200
//    * 17m 18.104600 18.106000 - 18.106200
//    * 15m 21.094600 21.096000 - 21.096200
//    * 12m 24.924600 24.926000 - 24.926200
//    * 10m 28.124600 28.126000 - 28.126200
//    * 6m 50.293000 50.294400 - 50.294600
//    * 2m 144.488500 144.489900 - 144.490100



//
// symbool and message clocks
//
// WSPR message is transmited as 162 symbols, 8192/12000 secs/ symbol - approx 110 secs starting at the top
// of an even minutes.  We assume board is powerd on at top of en even minute.
//
//
reg  [33:0] c122_two_min_counter;
reg  [26:0] c122_symbol_counter;
reg [7:0] c122_current_symbol;
reg two_min_led;
reg [1:0] two_min_count;

always @ (posedge C122_clk)  begin
        if ( C122_rst ) begin
                c122_two_min_counter <= 0;
                c122_symbol_counter <= 0;
                c122_current_symbol <= 0;
        end
        else begin
                if ( c122_two_min_counter == 34'd14745599999 ) begin // top of two min xmit cycle
                        two_min_led <= ~two_min_led;
                        c122_symbol_counter <= 0;
                        c122_two_min_counter <= 0;
                        c122_current_symbol <= 0;
                        if ( two_min_count == 2 ) begin
                        two_min_count <= 0;
                    end
                    else begin
                                two_min_count <= two_min_count + 1;
                        end
                end
                else begin
                        c122_two_min_counter <= c122_two_min_counter + 34'd1;
                        if ( c122_symbol_counter == 27'd83886079 ) begin // end of a symbol advance to next sym
                                c122_symbol_counter <= 0;
                                if ( c122_current_symbol != 162 )  begin
                                        c122_current_symbol <= c122_current_symbol + 1;
                                end
                        end
                        else begin
                                c122_symbol_counter <= c122_symbol_counter + 1;
                        end
                end
        end
end


wire xmit_active;

// xmit is active one out of 3 slots, and only if we still have a symbol
assign xmit_active = ( two_min_count == 0 ? ( c122_current_symbol == 162 ? 0 : 1 ) : 0 );




//////////////////////////////////////////////////////////////
//
//    CORDIC NCO
//
//////////////////////////////////////////////////////////////

// Code rotates input at set frequency and produces I & Q /

wire [14:0] C122_i_out;
wire [14:0] C122_q_out;
wire [31:0] C122_phase;

//
// message symbol rom - presents the correct symbol to the wspr xmitter 
//
wire [1:0] current_sym;
symrom  message_syms(
                .address(c122_current_symbol),
                .clock(C122_clk),
                .q(current_sym)
                );


// 
// the individual wspr xmitters - three of 'em 
//

wspr_xmit b30_xmit (
        .clock(C122_clk),
        .phase_words(b30_phase_words),
        .symnum(current_sym),
        .in_level(32500),
        .i_out(C122_i_out)
        );


wire [14:0] C122_i_out_b20;


wspr_xmit b20_xmit (
        .clock(C122_clk),
        .phase_words(b20_phase_words),
        .symnum(current_sym),
        .in_level(32500),
        .i_out(C122_i_out_b20)
        );


wire [14:0] C122_i_out_b40;

wspr_xmit b40_xmit (
        .clock(C122_clk),
        .phase_words(b40_phase_words),
        .symnum(current_sym),
        .in_level(32500),
        .i_out(C122_i_out_b40)
        );

// wire [14:0] C122_i_out_b15;
//
//wspr_xmit b15_xmit (
//        .clock(C122_clk),
//        .phase_words(b15_phase_words),
//        .symnum(current_sym),
//        .in_level(32500),
//        .i_out(C122_i_out_b15)
//        );


reg [17:0] sig_sum;


reg [15:0] sig_a;
reg [15:0] sig_b;
// reg [15:0] sig_c;
reg [15:0] sig_d;
reg [15:0] sig_a2;
reg [15:0] sig_b2;
// reg [15:0] sig_c2;
reg [15:0] sig_d2;

// 
// register the output of each of the transmitters, as well as 1/2 the output of each to a second register
// 
always @ (posedge C122_clk) begin

        sig_a <= { C122_i_out[14], C122_i_out[14:0] } ;
        sig_b <= { C122_i_out_b20[14], C122_i_out_b20[14:0] } ;
        // sig_c <= { C122_i_out_b15[14], C122_i_out_b15[14:0] } ;
        sig_d <= { C122_i_out_b40[14], C122_i_out_b40[14:0] } ;

        // 0.5 of original signal
        sig_a2 <= { C122_i_out[14], C122_i_out[14], C122_i_out[14:1] } ;
        sig_b2 <= { C122_i_out_b20[14], C122_i_out_b20[14], C122_i_out_b20[14:1] } ;
        // sig_c2 <= { C122_i_out_b15[14], C122_i_out_b15[14], C122_i_out_b15[14:1] } ;
        sig_d2 <= { C122_i_out_b40[14], C122_i_out_b40[14], C122_i_out_b40[14:1] } ;


end



// 
// add the signals together -- this gives us 1.5 time the original set of signals. 
// With the xmit's set to a level of 32500 this gives us about 21 dbm (125 mW) output level per 
// tone 
// 
sigadder final_adder (
        .clock(C122_clk),
        .data0x(sig_a),
        .data1x(sig_b),
        // .data2x(sig_c),
        .data2x(0),
        .data3x(sig_d),
        // .data4x(0),
        // .data5x(0),
        // .data6x(0),
        // .data7x(0),
        .data4x(sig_a2),
        .data5x(sig_b2),
        //.data6x(sig_c2),
        .data6x(0),
        .data7x(sig_d2),
        .result(sig_sum)
        );


reg [13:0]  dac_reg;

always @ (posedge C122_clk) begin
  dac_reg <= ( xmit_active ? sig_sum[15:2] : 0 );
end


assign  DAC[13:0] = dac_reg;



// get necessary C & C data

reg         PTT_out;       // PTT to Penelope
// reg   [3:0] C122_clock_select;  // 10MHz and 122.88MHz clock selection
reg   [6:0] OC;            // Open Collector outputs data
parameter PENNY_ADDR = 4'b0; // set C&C address that Penny will respond to




always @ (posedge C122_clk)
begin
  if (C122_rst)
  begin
    C122_DFS1         <= 1'b0;   // I/Q sampling rate selection
    C122_DFS0         <= 1'b0;
    PTT_out           <= 1'b0;

// phase words for 30 meters
//  354 422 183 10140100
//  354 422 234 10140101.4648
//  354 422 286 10140102.9297
//  354 422 337 10140104.3945

// 354 425 679  10140200
// 354 425 730  10140201.4648
// 354 425 781  10140202.92968
// 354 425 832  10140204.3945

    b30_phase_words[0] <= 354425679;
    b30_phase_words[1] <= 354425730;
    b30_phase_words[2] <= 354425781;
    b30_phase_words[3] <= 354425832;

// 20 meter phase words
// 492 729 358 14097100
// 492 729 409 14097101.4648
// 492 729 460 14097102.9297
// 492 729 511 14097104.3945

    b20_phase_words[0] <= 492729358;
    b20_phase_words[1] <= 492729409;
    b20_phase_words[2] <= 492729460;
    b20_phase_words[3] <= 492729511;

// 246 069 330  7040100
// 246 069 381 7040101.4648
// 246 069 432
// 246 069 484

    b40_phase_words[0] <= 246069330;
    b40_phase_words[1] <= 246069381;
    b40_phase_words[2] <= 246069432;
    b40_phase_words[3] <= 246069484;


// phase words for 15 meters
//  737 362 138 21096100
//  737 362 190
//  737 362 241
//  737 362 292

//    b15_phase_words[0] <= 737362138;
//    b15_phase_words[1] <= 737362190;  
//    b15_phase_words[2] <= 737362241;
//    b15_phase_words[3] <= 737362292;




    OC                <= 7'b0;   // Penelope Open Collectors
  end
end

// kd5tfd hard code for now - extenal 10 Mhz and use 122.88 on Penny
// if set use internal 10MHz TCXO and send to C16 else get from C16
assign ref_ext        =  0;
// if set use internally and send to LVDS else get from LVDS
assign source_122MHZ  =  1;


///////////////////////////////////////////////////////////
//
//    PTT
//
///////////////////////////////////////////////////////////

/*
  The Atlas bus has a tri-state PTT line (C15 = PTT_in) that
  when high indicates a board on the bus has requested PTT.
  When inactive set high Z, a pull down resistor on the bus will
  set PTT_in to logic 0.
*/

assign  PTT_in    = PTT ? 1'bz: 1'b1;  // set high if PTT active(low) else high Z
assign  FPGA_PTT  = PTT_out;       // turn PTT FET Q3 on when Txing

///////////////////////////////////////////////////////////
//
//    PLL
//
///////////////////////////////////////////////////////////

/*
  Divide the 10MHz reference and 122.88MHz clock to give 80kHz signals.
  Apply these to an EXOR phase detector. If the 10MHz reference is not
  present the EXOR output will be a 80kHz square wave. When passed through
  the loop filter this will provide a dc level of (3.3/2)v which will
  set the 122.88MHz VCXO to its nominal frequency.
  The selection of the internal or external 10MHz reference for the PLL
  is made using ref_ext.

*/

// div 10 MHz ref clock by 125 to get 80 khz

wire ref_80khz;
reg osc_80khz;

oddClockDivider refClockDivider(reference, ref_80khz);

// Divide  122.88 MHz by 1536 to get 80 khz
reg [9:0] count_12288;

always @ (posedge C122_clk)
begin
  if (count_12288 == 767)
  begin
    count_12288 <= 0;
    osc_80khz   <= ~osc_80khz;
  end
  else
    count_12288 <= count_12288 + 10'b1;
end


// Apply to EXOR phase detector
assign FPGA_PLL = ref_80khz ^ osc_80khz;


/////////////////////////////////////////////////////////////////
//
// General I O pins
//
/////////////////////////////////////////////////////////////////


// LEDs for testing       PCB LED Marking
// NOTE: LED1 is power on

// Bar graph for power output
// assign LED2 = (AIN5 > 250)?  1'b0 : 1'b1;
// assign LED3 = (AIN5 > 500)?  1'b0 : 1'b1;
// assign LED4 = (AIN5 > 1000)? 1'b0 : 1'b1;
// assign LED5 = (AIN5 > 2000)? 1'b0 : 1'b1;
// assign LED6 = (AIN5 > 3000)? 1'b0 : 1'b1;

// assign LED7 = 0;    // LED7 ON so we can see code has loaded OK
assign LED7 = C122_pps;
assign LED6 = two_min_led;
assign LED4 = current_sym[0];
assign LED5 = current_sym[1];
assign LED3 = two_min_count[1]; //xmit_active;
assign LED2 = two_min_count[0];

// User open collector outputs
assign USEROUT0 = OC[0];
assign USEROUT1 = OC[1];
assign USEROUT2 = OC[2];
assign USEROUT3 = OC[3];
assign USEROUT4 = OC[4];
assign USEROUT5 = OC[5];
assign USEROUT6 = OC[6];

// function integer clogb2;
// input [31:0] depth;
// begin
// for(clogb2=0; depth>0; clogb2=clogb2+1)
//  depth = depth >> 1;
//end
// endfunction

endmodule

