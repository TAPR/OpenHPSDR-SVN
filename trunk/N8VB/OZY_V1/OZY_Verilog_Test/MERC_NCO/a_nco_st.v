//	Copyright (C) 1988-2002 Altera Corporation

//	Any megafunction design, and related net list (encrypted or decrypted),
//	support information, device programming or simulation file, and any other
//	associated documentation or information provided by Altera or a partner
//	under Altera's Megafunction Partnership Program may be used only to
//	program PLD devices (but not masked PLD devices) from Altera.  Any other
//	use of such megafunction design, net list, support information, device
//	programming or simulation file, or any other related documentation or
//	information is prohibited for any other purpose, including, but not
//	limited to modification, reverse engineering, de-compiling, or use with
//	any other silicon devices, unless such use is explicitly licensed under
//	a separate agreement with Altera or a megafunction partner.  Title to
//	the intellectual property, including patents, copyrights, trademarks,
//	trade secrets, or maskworks, embodied in any such megafunction design,
//	net list, support information, device programming or simulation file, or
//	any other related documentation or information provided by Altera or a
//	megafunction partner, remains with Altera, the megafunction partner, or
//	their respective licensors.  No other licenses, including any licenses
//	needed under any third party's intellectual property, are provided herein.


module a_nco_st(clk,reset,clken,phi_inc_i,fsin_o, fcos_o,data_ready);

parameter mpr = 16;
parameter apr = 32;
parameter apri= 16;
parameter aprf= 32;
parameter aprp= 16;
parameter aprid=21;
parameter dpri= 5;
parameter cpr= 4;
parameter mval= 16;
parameter nstages = 33;
parameter X0 = 19897;
parameter Z0 = 16384;
parameter Z1 = 9672;
parameter Z2 = 5110;
parameter Z3 = 2594;
parameter Z4 = 1302;
parameter Z5 = 651;
parameter Z6 = 325;
parameter Z7 = 162;
parameter Z8 = 81;
parameter Z9 = 40;
parameter Z10 = 20;
parameter Z11 = 10;
parameter Z12 = 5;
parameter Z13 = 2;
parameter Z14 = 1;
parameter Z15 = 0;
parameter Z16 = 0;
parameter Z17 = 0;
parameter Z18 = 0;
parameter Z19 = 0;
parameter Z20 = 0;
parameter Z21 = 0;
parameter Z22 = 0;
parameter Z23 = 0;
parameter Z24 = 0;
parameter Z25 = 0;
parameter Z26 = 0;
parameter Z27 = 0;
parameter Z28 = 0;
parameter Z29 = 0;
parameter Z30 = 0;
parameter Z31 = 0;
parameter nc = 1;
parameter log2nc =0;
parameter outselinit = 0;
parameter paci0= 0;
parameter paci1= 0;
parameter paci2= 0;
parameter paci3= 0;
parameter paci4= 0;
parameter paci5= 0;
parameter paci6= 0;
parameter paci7= 0;

input clk; 
input reset; 
input clken; 
input [apr-1:0] phi_inc_i; 

output [mpr-1:0] fsin_o;
output [mpr-1:0] fcos_o;
output data_ready;

wire [apr-1:0]  phi_inc_i_w;
wire [apr-1:0]  phi_acc_w;
wire [mpr-1:0] cordic_0x;
wire [mpr-1:0] cordic_0y;
wire [apri-1:0] cordic_0z;
wire [mpr-1:0] cordic_0x_w;
wire [mpr-1:0] cordic_0y_w;
wire [apri-1:0] cordic_0z_w;
wire [mpr-1:0] cordic_1x;
wire [mpr-1:0] cordic_1y;
wire [apri-1:0] cordic_1z;
wire [mpr-1:0] cordic_1x_w;
wire [mpr-1:0] cordic_1y_w;
wire [apri-1:0] cordic_1z_w;
wire [mpr-1:0] cordic_2x;
wire [mpr-1:0] cordic_2y;
wire [apri-1:0] cordic_2z;
wire [mpr-1:0] cordic_2x_w;
wire [mpr-1:0] cordic_2y_w;
wire [apri-1:0] cordic_2z_w;
wire [mpr-1:0] cordic_3x;
wire [mpr-1:0] cordic_3y;
wire [apri-1:0] cordic_3z;
wire [mpr-1:0] cordic_3x_w;
wire [mpr-1:0] cordic_3y_w;
wire [apri-1:0] cordic_3z_w;
wire [mpr-1:0] cordic_4x;
wire [mpr-1:0] cordic_4y;
wire [apri-1:0] cordic_4z;
wire [mpr-1:0] cordic_4x_w;
wire [mpr-1:0] cordic_4y_w;
wire [apri-1:0] cordic_4z_w;
wire [mpr-1:0] cordic_5x;
wire [mpr-1:0] cordic_5y;
wire [apri-1:0] cordic_5z;
wire [mpr-1:0] cordic_5x_w;
wire [mpr-1:0] cordic_5y_w;
wire [apri-1:0] cordic_5z_w;
wire [mpr-1:0] cordic_6x;
wire [mpr-1:0] cordic_6y;
wire [apri-1:0] cordic_6z;
wire [mpr-1:0] cordic_6x_w;
wire [mpr-1:0] cordic_6y_w;
wire [apri-1:0] cordic_6z_w;
wire [mpr-1:0] cordic_7x;
wire [mpr-1:0] cordic_7y;
wire [apri-1:0] cordic_7z;
wire [mpr-1:0] cordic_7x_w;
wire [mpr-1:0] cordic_7y_w;
wire [apri-1:0] cordic_7z_w;
wire [mpr-1:0] cordic_8x;
wire [mpr-1:0] cordic_8y;
wire [apri-1:0] cordic_8z;
wire [mpr-1:0] cordic_8x_w;
wire [mpr-1:0] cordic_8y_w;
wire [apri-1:0] cordic_8z_w;
wire [mpr-1:0] cordic_9x;
wire [mpr-1:0] cordic_9y;
wire [apri-1:0] cordic_9z;
wire [mpr-1:0] cordic_9x_w;
wire [mpr-1:0] cordic_9y_w;
wire [apri-1:0] cordic_9z_w;
wire [mpr-1:0] cordic_10x;
wire [mpr-1:0] cordic_10y;
wire [apri-1:0] cordic_10z;
wire [mpr-1:0] cordic_10x_w;
wire [mpr-1:0] cordic_10y_w;
wire [apri-1:0] cordic_10z_w;
wire [mpr-1:0] cordic_11x;
wire [mpr-1:0] cordic_11y;
wire [apri-1:0] cordic_11z;
wire [mpr-1:0] cordic_11x_w;
wire [mpr-1:0] cordic_11y_w;
wire [apri-1:0] cordic_11z_w;
wire [mpr-1:0] cordic_12x;
wire [mpr-1:0] cordic_12y;
wire [apri-1:0] cordic_12z;
wire [mpr-1:0] cordic_12x_w;
wire [mpr-1:0] cordic_12y_w;
wire [apri-1:0] cordic_12z_w;
wire [mpr-1:0] cordic_13x;
wire [mpr-1:0] cordic_13y;
wire [apri-1:0] cordic_13z;
wire [mpr-1:0] cordic_13x_w;
wire [mpr-1:0] cordic_13y_w;
wire [apri-1:0] cordic_13z_w;
wire [mpr-1:0] cordic_14x;
wire [mpr-1:0] cordic_14y;
wire [apri-1:0] cordic_14z;
wire [mpr-1:0] cordic_14x_w;
wire [mpr-1:0] cordic_14y_w;
wire [apri-1:0] cordic_14z_w;
wire [mpr-1:0] cordic_15x;
wire [mpr-1:0] cordic_15y;
wire [apri-1:0] cordic_15z;
wire [mpr-1:0] cordic_15x_w;
wire [mpr-1:0] cordic_15y_w;
wire [apri-1:0] cordic_15z_w;
wire [mpr-1:0] cordic_16x_w;
wire [mpr-1:0] cordic_16y_w;
wire [mpr-1:0] cordic_x_res;
wire [mpr-1:0] cordic_y_res;
wire [mpr-1:0] cordic_x_res_d;
wire [mpr-1:0] cordic_y_res_d;
wire [mpr-1:0] cordic_x_res_2c;
wire [mpr-1:0] cordic_y_res_2c;
wire [1:0] qd;
wire [1:0] curqd;
wire [mpr-1:0] sin_o_w;
wire [mpr-1:0] cos_o_w;
wire [mpr-1:0] fsin_o_w;	
wire [mpr-1:0] fcos_o_w;	





assign cordic_1x = cordic_1x_w;
assign cordic_1y = cordic_1y_w;
assign cordic_1z = cordic_1z_w;
assign cordic_2x = cordic_2x_w;
assign cordic_2y = cordic_2y_w;
assign cordic_2z = cordic_2z_w;
assign cordic_3x = cordic_3x_w;
assign cordic_3y = cordic_3y_w;
assign cordic_3z = cordic_3z_w;
assign cordic_4x = cordic_4x_w;
assign cordic_4y = cordic_4y_w;
assign cordic_4z = cordic_4z_w;
assign cordic_5x = cordic_5x_w;
assign cordic_5y = cordic_5y_w;
assign cordic_5z = cordic_5z_w;
assign cordic_6x = cordic_6x_w;
assign cordic_6y = cordic_6y_w;
assign cordic_6z = cordic_6z_w;
assign cordic_7x = cordic_7x_w;
assign cordic_7y = cordic_7y_w;
assign cordic_7z = cordic_7z_w;
assign cordic_8x = cordic_8x_w;
assign cordic_8y = cordic_8y_w;
assign cordic_8z = cordic_8z_w;
assign cordic_9x = cordic_9x_w;
assign cordic_9y = cordic_9y_w;
assign cordic_9z = cordic_9z_w;
assign cordic_10x = cordic_10x_w;
assign cordic_10y = cordic_10y_w;
assign cordic_10z = cordic_10z_w;
assign cordic_11x = cordic_11x_w;
assign cordic_11y = cordic_11y_w;
assign cordic_11z = cordic_11z_w;
assign cordic_12x = cordic_12x_w;
assign cordic_12y = cordic_12y_w;
assign cordic_12z = cordic_12z_w;
assign cordic_13x = cordic_13x_w;
assign cordic_13y = cordic_13y_w;
assign cordic_13z = cordic_13z_w;
assign cordic_14x = cordic_14x_w;
assign cordic_14y = cordic_14y_w;
assign cordic_14z = cordic_14z_w;
assign cordic_15x = cordic_15x_w;
assign cordic_15y = cordic_15y_w;
assign cordic_15z = cordic_15z_w;
assign cordic_x_res = cordic_16x_w;
assign cordic_y_res = cordic_16y_w;

cord_init ci(.clk(clk),
                .reset(reset), 
                .clken(clken), 
             .phi_acc_w(phi_acc_w),
             .corx(cordic_0x),
             .cory(cordic_0y),
             .corz(cordic_0z)
);
defparam ci.apr=apr;
defparam ci.apri=apri;
defparam ci.mpr=mpr;
defparam ci.X0=X0;

cord_fs cfs(.clk(clk),
            .reset(reset),
            .clken(clken), 
            .cor0x(cordic_0x),
            .cor0y(cordic_0y),
            .cor0zmsb(cordic_0z[apri-1]),
            .cor1x(cordic_1x_w),
            .cor1y(cordic_1y_w)
           );
defparam cfs.mpr=mpr;



assign curqd = phi_acc_w[apr-1:apr-2];

cord_seg_sel css(.clk(clk),
                 .reset(reset),
                 .clken(clken), 
                 .cur_seg(curqd),
                 .seg_rot(qd)
                 );
defparam css.nstages = nstages;

assign phi_inc_i_w = phi_inc_i;

asj_altqmcpipe ux000 (.clk(clk), 
             .reset(reset), 
             .clken(clken), 
             .phi_inc_int(phi_inc_i_w), 
             .phi_acc_reg(phi_acc_w)
             );

defparam ux000.apr = apr ;
defparam ux000.lat = 1 ;
defparam ux000.nc = nc ;
defparam ux000.paci0 = paci0 ;
defparam ux000.paci1 = paci1 ;
defparam ux000.paci2 = paci2 ;
defparam ux000.paci3 = paci3 ;
defparam ux000.paci4 = paci4 ;
defparam ux000.paci5 = paci5 ;
defparam ux000.paci6 = paci6 ;
defparam ux000.paci7 = paci7 ;


cordic_zxor_1p_lpm u3(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_0z),
                                          .zdata(cordic_0z[apri-1]), .result(cordic_1z_w));
defparam u3.mpr = apri;
defparam u3.Zn = Z0;
defparam u3.pipe = 1;
cordic_sxor_1p_lpm u4(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_1x), .datab(cordic_1y),
                                            .zdata(cordic_1z[apri-1]), .result(cordic_2x_w));
defparam u4.shiftby = 1;
defparam u4.mpr = mpr;
defparam u4.pipe = 1;
cordic_axor_1p_lpm u5(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_1y), .datab(cordic_1x),
                                            .zdata(cordic_1z[apri-1]), .result(cordic_2y_w));
defparam u5.shiftby = 1;
defparam u5.mpr = mpr;
defparam u5.pipe = 1;
cordic_zxor_1p_lpm u6(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_1z),
                                          .zdata(cordic_1z[apri-1]), .result(cordic_2z_w));
defparam u6.mpr = apri;
defparam u6.Zn = Z1;
defparam u6.pipe = 1;
cordic_sxor_1p_lpm u7(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_2x), .datab(cordic_2y),
                                            .zdata(cordic_2z[apri-1]), .result(cordic_3x_w));
defparam u7.shiftby = 2;
defparam u7.mpr = mpr;
defparam u7.pipe = 1;
cordic_axor_1p_lpm u8(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_2y), .datab(cordic_2x),
                                            .zdata(cordic_2z[apri-1]), .result(cordic_3y_w));
defparam u8.shiftby = 2;
defparam u8.mpr = mpr;
defparam u8.pipe = 1;
cordic_zxor_1p_lpm u9(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_2z),
                                          .zdata(cordic_2z[apri-1]), .result(cordic_3z_w));
defparam u9.mpr = apri;
defparam u9.Zn = Z2;
defparam u9.pipe = 1;
cordic_sxor_1p_lpm u10(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_3x), .datab(cordic_3y),
                                            .zdata(cordic_3z[apri-1]), .result(cordic_4x_w));
defparam u10.shiftby = 3;
defparam u10.mpr = mpr;
defparam u10.pipe = 1;
cordic_axor_1p_lpm u11(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_3y), .datab(cordic_3x),
                                            .zdata(cordic_3z[apri-1]), .result(cordic_4y_w));
defparam u11.shiftby = 3;
defparam u11.mpr = mpr;
defparam u11.pipe = 1;
cordic_zxor_1p_lpm u12(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_3z),
                                          .zdata(cordic_3z[apri-1]), .result(cordic_4z_w));
defparam u12.mpr = apri;
defparam u12.Zn = Z3;
defparam u12.pipe = 1;
cordic_sxor_1p_lpm u13(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_4x), .datab(cordic_4y),
                                            .zdata(cordic_4z[apri-1]), .result(cordic_5x_w));
defparam u13.shiftby = 4;
defparam u13.mpr = mpr;
defparam u13.pipe = 1;
cordic_axor_1p_lpm u14(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_4y), .datab(cordic_4x),
                                            .zdata(cordic_4z[apri-1]), .result(cordic_5y_w));
defparam u14.shiftby = 4;
defparam u14.mpr = mpr;
defparam u14.pipe = 1;
cordic_zxor_1p_lpm u15(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_4z),
                                          .zdata(cordic_4z[apri-1]), .result(cordic_5z_w));
defparam u15.mpr = apri;
defparam u15.Zn = Z4;
defparam u15.pipe = 1;
cordic_sxor_1p_lpm u16(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_5x), .datab(cordic_5y),
                                            .zdata(cordic_5z[apri-1]), .result(cordic_6x_w));
defparam u16.shiftby = 5;
defparam u16.mpr = mpr;
defparam u16.pipe = 1;
cordic_axor_1p_lpm u17(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_5y), .datab(cordic_5x),
                                            .zdata(cordic_5z[apri-1]), .result(cordic_6y_w));
defparam u17.shiftby = 5;
defparam u17.mpr = mpr;
defparam u17.pipe = 1;
cordic_zxor_1p_lpm u18(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_5z),
                                          .zdata(cordic_5z[apri-1]), .result(cordic_6z_w));
defparam u18.mpr = apri;
defparam u18.Zn = Z5;
defparam u18.pipe = 1;
cordic_sxor_1p_lpm u19(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_6x), .datab(cordic_6y),
                                            .zdata(cordic_6z[apri-1]), .result(cordic_7x_w));
defparam u19.shiftby = 6;
defparam u19.mpr = mpr;
defparam u19.pipe = 1;
cordic_axor_1p_lpm u20(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_6y), .datab(cordic_6x),
                                            .zdata(cordic_6z[apri-1]), .result(cordic_7y_w));
defparam u20.shiftby = 6;
defparam u20.mpr = mpr;
defparam u20.pipe = 1;
cordic_zxor_1p_lpm u21(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_6z),
                                          .zdata(cordic_6z[apri-1]), .result(cordic_7z_w));
defparam u21.mpr = apri;
defparam u21.Zn = Z6;
defparam u21.pipe = 1;
cordic_sxor_1p_lpm u22(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_7x), .datab(cordic_7y),
                                            .zdata(cordic_7z[apri-1]), .result(cordic_8x_w));
defparam u22.shiftby = 7;
defparam u22.mpr = mpr;
defparam u22.pipe = 1;
cordic_axor_1p_lpm u23(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_7y), .datab(cordic_7x),
                                            .zdata(cordic_7z[apri-1]), .result(cordic_8y_w));
defparam u23.shiftby = 7;
defparam u23.mpr = mpr;
defparam u23.pipe = 1;
cordic_zxor_1p_lpm u24(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_7z),
                                          .zdata(cordic_7z[apri-1]), .result(cordic_8z_w));
defparam u24.mpr = apri;
defparam u24.Zn = Z7;
defparam u24.pipe = 1;
cordic_sxor_1p_lpm u25(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_8x), .datab(cordic_8y),
                                            .zdata(cordic_8z[apri-1]), .result(cordic_9x_w));
defparam u25.shiftby = 8;
defparam u25.mpr = mpr;
defparam u25.pipe = 1;
cordic_axor_1p_lpm u26(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_8y), .datab(cordic_8x),
                                            .zdata(cordic_8z[apri-1]), .result(cordic_9y_w));
defparam u26.shiftby = 8;
defparam u26.mpr = mpr;
defparam u26.pipe = 1;
cordic_zxor_1p_lpm u27(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_8z),
                                          .zdata(cordic_8z[apri-1]), .result(cordic_9z_w));
defparam u27.mpr = apri;
defparam u27.Zn = Z8;
defparam u27.pipe = 1;
cordic_sxor_1p_lpm u28(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_9x), .datab(cordic_9y),
                                            .zdata(cordic_9z[apri-1]), .result(cordic_10x_w));
defparam u28.shiftby = 9;
defparam u28.mpr = mpr;
defparam u28.pipe = 1;
cordic_axor_1p_lpm u29(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_9y), .datab(cordic_9x),
                                            .zdata(cordic_9z[apri-1]), .result(cordic_10y_w));
defparam u29.shiftby = 9;
defparam u29.mpr = mpr;
defparam u29.pipe = 1;
cordic_zxor_1p_lpm u30(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_9z),
                                          .zdata(cordic_9z[apri-1]), .result(cordic_10z_w));
defparam u30.mpr = apri;
defparam u30.Zn = Z9;
defparam u30.pipe = 1;
cordic_sxor_1p_lpm u31(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_10x), .datab(cordic_10y),
                                            .zdata(cordic_10z[apri-1]), .result(cordic_11x_w));
defparam u31.shiftby = 10;
defparam u31.mpr = mpr;
defparam u31.pipe = 1;
cordic_axor_1p_lpm u32(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_10y), .datab(cordic_10x),
                                            .zdata(cordic_10z[apri-1]), .result(cordic_11y_w));
defparam u32.shiftby = 10;
defparam u32.mpr = mpr;
defparam u32.pipe = 1;
cordic_zxor_1p_lpm u33(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_10z),
                                          .zdata(cordic_10z[apri-1]), .result(cordic_11z_w));
defparam u33.mpr = apri;
defparam u33.Zn = Z10;
defparam u33.pipe = 1;
cordic_sxor_1p_lpm u34(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_11x), .datab(cordic_11y),
                                            .zdata(cordic_11z[apri-1]), .result(cordic_12x_w));
defparam u34.shiftby = 11;
defparam u34.mpr = mpr;
defparam u34.pipe = 1;
cordic_axor_1p_lpm u35(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_11y), .datab(cordic_11x),
                                            .zdata(cordic_11z[apri-1]), .result(cordic_12y_w));
defparam u35.shiftby = 11;
defparam u35.mpr = mpr;
defparam u35.pipe = 1;
cordic_zxor_1p_lpm u36(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_11z),
                                          .zdata(cordic_11z[apri-1]), .result(cordic_12z_w));
defparam u36.mpr = apri;
defparam u36.Zn = Z11;
defparam u36.pipe = 1;
cordic_sxor_1p_lpm u37(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_12x), .datab(cordic_12y),
                                            .zdata(cordic_12z[apri-1]), .result(cordic_13x_w));
defparam u37.shiftby = 12;
defparam u37.mpr = mpr;
defparam u37.pipe = 1;
cordic_axor_1p_lpm u38(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_12y), .datab(cordic_12x),
                                            .zdata(cordic_12z[apri-1]), .result(cordic_13y_w));
defparam u38.shiftby = 12;
defparam u38.mpr = mpr;
defparam u38.pipe = 1;
cordic_zxor_1p_lpm u39(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_12z),
                                          .zdata(cordic_12z[apri-1]), .result(cordic_13z_w));
defparam u39.mpr = apri;
defparam u39.Zn = Z12;
defparam u39.pipe = 1;
cordic_sxor_1p_lpm u40(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_13x), .datab(cordic_13y),
                                            .zdata(cordic_13z[apri-1]), .result(cordic_14x_w));
defparam u40.shiftby = 13;
defparam u40.mpr = mpr;
defparam u40.pipe = 1;
cordic_axor_1p_lpm u41(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_13y), .datab(cordic_13x),
                                            .zdata(cordic_13z[apri-1]), .result(cordic_14y_w));
defparam u41.shiftby = 13;
defparam u41.mpr = mpr;
defparam u41.pipe = 1;
cordic_zxor_1p_lpm u42(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_13z),
                                          .zdata(cordic_13z[apri-1]), .result(cordic_14z_w));
defparam u42.mpr = apri;
defparam u42.Zn = Z13;
defparam u42.pipe = 1;
cordic_sxor_1p_lpm u43(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_14x), .datab(cordic_14y),
                                            .zdata(cordic_14z[apri-1]), .result(cordic_15x_w));
defparam u43.shiftby = 14;
defparam u43.mpr = mpr;
defparam u43.pipe = 1;
cordic_axor_1p_lpm u44(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_14y), .datab(cordic_14x),
                                            .zdata(cordic_14z[apri-1]), .result(cordic_15y_w));
defparam u44.shiftby = 14;
defparam u44.mpr = mpr;
defparam u44.pipe = 1;
cordic_zxor_1p_lpm u45(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_14z),
                                          .zdata(cordic_14z[apri-1]), .result(cordic_15z_w));
defparam u45.mpr = apri;
defparam u45.Zn = Z14;
defparam u45.pipe = 1;
cordic_sxor_1p_lpm u46(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_15x), .datab(cordic_15y),
                                            .zdata(cordic_15z[apri-1]), .result(cordic_16x_w));
defparam u46.shiftby = 15;
defparam u46.mpr = mpr;
defparam u46.pipe = 1;
cordic_axor_1p_lpm u47(.clk(clk), .reset(reset), .clken(clken), .dataa(cordic_15y), .datab(cordic_15x),
                                            .zdata(cordic_15z[apri-1]), .result(cordic_16y_w));
defparam u47.shiftby = 15;
defparam u47.mpr = mpr;
defparam u47.pipe = 1;

cord_2c cordinv(.clk(clk),
                .reset(reset), 
                .clken(clken), 
                .cordic_x_res(cordic_x_res),
                .cordic_y_res(cordic_y_res),
                .cordic_x_res_d(cordic_x_res_d),
                .cordic_y_res_d(cordic_y_res_d),
                .cordic_x_res_2c(cordic_x_res_2c),
                .cordic_y_res_2c(cordic_y_res_2c)
               );
defparam cordinv.mpr = mpr;

asj_crd     ux005(.clk(clk),
                  .reset(reset),
                  .clken(clken),
                  .qd(qd),
                  .cordic_x_res_d(cordic_x_res_d),
                  .cordic_y_res_d(cordic_y_res_d),
                  .cordic_x_res_2c(cordic_x_res_2c),
                  .cordic_y_res_2c(cordic_y_res_2c),
                  .sin_o(sin_o_w),
                  .cos_o(cos_o_w)
                  );
defparam ux005.mpr = mpr;

dop_reg dop(.clk(clk),
            .reset(reset),
            .clken(clken), 
            .sin_i(sin_o_w),
            .cos_i(cos_o_w),
            .sin_o(fsin_o_w),
            .cos_o(fcos_o_w)
           );
defparam dop.mpr=mpr;
assign fsin_o = fsin_o_w;
assign fcos_o = fcos_o_w;


asj_nco_isdr ux710isdr(.clk(clk),                              
                    .reset(reset),                          
                    .clken(clken),                  
                    .data_ready(data_ready)          
                    );                                      
defparam ux710isdr.ctc=36;                                       
defparam ux710isdr.cpr=6;                                   
                                                            

endmodule