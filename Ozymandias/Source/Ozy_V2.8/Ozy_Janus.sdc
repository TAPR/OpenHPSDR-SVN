# Ozy_Janus.sdc

#**************************************************************
# Time Information
#**************************************************************

set_time_format -unit ns -decimal_places 3


#**************************************************************
# Create Clock
#**************************************************************

create_clock -name IF_clk 	-period 48.000MHz [get_ports {IF_clk}]
create_clock -name C5		-period 12.288MHz [get_ports {C5}]
create_clock -name SPI_SCK	-period 12.288MHz [get_ports {SPI_SCK}]

derive_pll_clocks

derive_clock_uncertainty

#**************************************************************
# Create Generated Clock
#**************************************************************

create_generated_clock -name clk_lrclk_gen:clrgen|BCLK 				-source IF_clk 				-divide_by 40 	clk_lrclk_gen:clrgen|BCLK 
create_generated_clock -name I2C_monitor:I2C_inst|deglitch:deglitch_scl|out 	-source IF_clk 						I2C_monitor:I2C_inst|deglitch:deglitch_scl|out
create_generated_clock -name I2C_monitor:I2C_inst|deglitch:deglitch_sda|out 	-source IF_clk 						I2C_monitor:I2C_inst|deglitch:deglitch_sda|out
create_generated_clock -name sidetone2:sidetone_inst|sidetone_clock		-source IF_clk						sidetone2:sidetone_inst|sidetone_clock
create_generated_clock -name pro_clock						-source cm3|altpll_component|pll|clk[1] -divide_by 200	pro_clock
create_generated_clock -name iambic_clk						-source cm3|altpll_component|pll|clk[1] -divide_by 640	iambic_clk


#**************************************************************
# Set Input Delay
#**************************************************************

set_input_delay -add_delay -clock IF_clk -max 1.000 { A12 A13 A20 A21 C5 C22 C23 CDOUT CDOUT_P DOUT FLAGA FLAGB FLAGC FX2_FD[*] GPIO_IN[*] IF_clk MDOUT_I[*] MDOUT_Q[*] PTT_in SDOBACK SPI_CS SPI_SI }
set_input_delay -add_delay -clock IF_clk -min -1.000 { A12 A13 A20 A21 C5 C22 C23 CDOUT CDOUT_P DOUT FLAGA FLAGB FLAGC FX2_FD[*] GPIO_IN[*] IF_clk MDOUT_I[*] MDOUT_Q[*] PTT_in SDOBACK SPI_CS SPI_SI }


#**************************************************************
# Set Output Delay
#**************************************************************

set_output_delay -add_delay -clock IF_clk -max 1.000 { AK_reset C4 C7 C8 C9 C12 C12 C13 C14 C19 C21 C24 CC DEBUG_LED* FIFO_ADR[*] GPIO_OUT[*] SLOE SLRD SLWR SPI_SO }
set_output_delay -add_delay -clock IF_clk -min -1.000 { AK_reset C4 C7 C8 C9 C12 C12 C13 C14 C19 C21 C24 CC DEBUG_LED* FIFO_ADR[*] GPIO_OUT[*] SLOE SLRD SLWR SPI_SO }


#**************************************************************
# Set Clock Groups
#**************************************************************

set_clock_groups -asynchronous  -group {IF_clk \
				        cm3|altpll_component|pll|clk[0] \
				        cm3|altpll_component|pll|clk[1] \
					I2C_monitor:I2C_inst|deglitch:deglitch_scl|out \
					I2C_monitor:I2C_inst|deglitch:deglitch_sda|out \
					sidetone2:sidetone_inst|sidetone_clock \
					pro_clock \
					iambic_clk \
				       } \
				-group {C5} \
				-group {SPI_SCK}


#**************************************************************
# Set Maximum Delay
#**************************************************************

set_max_delay -from C5 -to C6 10
set_max_delay -from C5 -to clk_lrclk_gen:clrgen|BCLK 7

set_max_delay -from clk_lrclk_gen:clrgen|BCLK -to C5 3

set_max_delay -from cm3|altpll_component|pll|clk[0] -to cm3|altpll_component|pll|clk[0]	11
set_max_delay -from cm3|altpll_component|pll|clk[0] -to IF_clk	11

set_max_delay -from IF_clk -to C48_clk	10
set_max_delay -from IF_clk -to iambic_clk 140
set_max_delay -from IF_clk -to IF_clk 60
set_max_delay -from IF_clk -to cm3|altpll_component|pll|clk[0] 16

set_max_delay -from pro_clock -to cm3|altpll_component|pll|clk[0] 18

set_max_delay -from SDOBACK -to FX2_PE1 11

set_max_delay -to FX2_FD[*] 12


#**************************************************************
# Set Minimum Delay
#**************************************************************

set_min_delay -from C5 -to C6 -2

set_min_delay -from clk_lrclk_gen:clrgen|BCLK -to C5 -3

set_min_delay -from cm3|altpll_component|pll|clk[0] -to IF_clk -4

set_min_delay -from IF_clk -to C48_clk	-2

set_min_delay -from pro_clock -to sidetone2:sidetone_inst|sidetone_clock -2

set_min_delay -from SDOBACK -to FX2_PE1 -2

set_min_delay -to FX2_FD[*] -2
