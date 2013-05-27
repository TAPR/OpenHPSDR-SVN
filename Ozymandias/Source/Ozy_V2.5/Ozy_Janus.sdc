## Generated SDC file "Ozy_Janus.sdc"

## Copyright (C) 1991-2012 Altera Corporation
## Your use of Altera Corporation's design tools, logic functions 
## and other software and tools, and its AMPP partner logic 
## functions, and any output files from any of the foregoing 
## (including device programming or simulation files), and any 
## associated documentation or information are expressly subject 
## to the terms and conditions of the Altera Program License 
## Subscription Agreement, Altera MegaCore Function License 
## Agreement, or other applicable license agreement, including, 
## without limitation, that your use is for the sole purpose of 
## programming logic devices manufactured by Altera and sold by 
## Altera or its authorized distributors.  Please refer to the 
## applicable agreement for further details.


## VENDOR  "Altera"
## PROGRAM "Quartus II"
## VERSION "Version 12.1 Build 177 11/07/2012 SJ Web Edition"

## DATE    "Mon Apr 01 10:23:42 2013"

##
## DEVICE  "EP2C8Q208C8"
##


#**************************************************************
# Time Information
#**************************************************************

set_time_format -unit ns -decimal_places 3



#**************************************************************
# Create Clock
#**************************************************************

create_clock -name {IF_clk} -period 20.833 -waveform { 0.000 10.416 } [get_ports {IF_clk}]
create_clock -name {C5} -period 81.38020 [get_ports {C5}]
create_clock -name {SPI_SCK} -period 81.38020 [get_ports {SPI_SCK}]
create_clock -name I2C_monitor:I2C_inst|deglitch:deglitch_scl|out -period 5000 I2C:I2C_inst|deglitch:deglitch_scl|out
create_clock -name I2C_monitor:I2C_inst|deglitch:deglitch_sda|out -period 5000 I2C:I2C_inst|deglitch:deglitch_sda|out

derive_pll_clocks

#derive_clock_uncertainty

#**************************************************************
# Create Generated Clock
#**************************************************************

create_generated_clock -name {clk_lrclk_gen:clrgen|BCLK} -source [get_ports {IF_clk}] -divide_by 40 -master_clock {IF_clk} [get_registers {clk_lrclk_gen:clrgen|BCLK}] 
#create_generated_clock -name {cm3|altpll_component|pll|clk[0]} -source [get_pins {cm3|altpll_component|pll|inclk[0]}] -duty_cycle 50.000 -multiply_by 3 -master_clock {IF_clk} [get_pins {cm3|altpll_component|pll|clk[0]}] 


#**************************************************************
# Set Clock Latency
#**************************************************************



#**************************************************************
# Set Clock Uncertainty
#**************************************************************



#**************************************************************
# Set Input Delay
#**************************************************************



#**************************************************************
# Set Output Delay
#**************************************************************



#**************************************************************
# Set Clock Groups
#**************************************************************

set_clock_groups -asynchronous  -group  {IF_clk} \
				        {cm3|altpll_component|pll|clk[0]} \
				-group	{C5} \
				-group  {I2C_monitor:I2C_inst|deglitch:deglitch_scl|out} \
					{I2C_monitor:I2C_inst|deglitch:deglitch_sda|out}


#**************************************************************
# Set False Path
#**************************************************************
set_false_path -from C5 -to cm3|altpll_component|pll|clk[0]}
set_false_path -from cm3|altpll_component|pll|clk[0]} -to C5
set_false_path -from IF_clk -to C5
set_false_path -from C5 -to IF_clk
set_false_path -from C5 -to clk_lrclk_gen:clrgen|BCLK
set_false_path -from clk_lrclk_gen:clrgen|BCLK -to C5

#**************************************************************
# Set Multicycle Path
#**************************************************************

set_multicycle_path -from cm3|altpll_component|pll|clk[0] -to cm3|altpll_component|pll|clk[0] -setup 2
set_multicycle_path -from cm3|altpll_component|pll|clk[0] -to cm3|altpll_component|pll|clk[0] -hold 1

#set_multicycle_path -from cm3|altpll_component|pll|clk[0] -to IF_clk -setup 8
#set_multicycle_path -from cm3|altpll_component|pll|clk[0] -to IF_clk -hold 7
#set_multicycle_path -from IF_clk -to C5 -setup 2
#set_multicycle_path -from IF_clk -to C5 -hold 1
#set_multicycle_path -from C5 -to IF_clk -setup 2
#set_multicycle_path -from C5 -to IF_clk -hold 1


#**************************************************************
# Set Maximum Delay
#**************************************************************



#**************************************************************
# Set Minimum Delay
#**************************************************************



#**************************************************************
# Set Input Transition
#**************************************************************

