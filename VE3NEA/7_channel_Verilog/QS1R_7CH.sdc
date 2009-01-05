## Generated SDC file "QS1R_7CH.sdc"

## Copyright (C) 1991-2007 Altera Corporation
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
## VERSION "Version 7.2 Build 207 03/18/2008 Service Pack 3 SJ Web Edition"

## DATE    "Thu Jul 31 15:09:04 2008"

##
## DEVICE  "EP3C25Q240C8"
##


#**************************************************************
# Time Information
#**************************************************************

set_time_format -unit ns -decimal_places 3



#**************************************************************
# Create Clock
#**************************************************************

create_clock -name {FX2_clk_Clock} -period 20.833 -waveform { 0.000 10.416 } [get_ports {FX2_clk}] -add
create_clock -name {FX2_ifclk_Clock} -period 20.833 -waveform { 0.000 10.416 } [get_ports {FX2_ifclk}] -add
create_clock -name {FX2_multi_clk_Clock} -period 20.833 -waveform { 0.000 10.416 } [get_ports {FX2_multi_clk}] -add
create_clock -name {ADC_clka_Clock} -period 8.000 -waveform { 0.000 4.000 } [get_ports {ADC_clka}] -add


#**************************************************************
# Create Generated Clock
#**************************************************************

derive_pll_clocks -create_base_clock
derive_clocks -period "1.0"


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

set_clock_groups -exclusive -group [get_clocks {ADC_clka_Clock}]
set_clock_groups -exclusive -group [get_clocks {FX2_clk_Clock}]
set_clock_groups -exclusive -group [get_clocks {FX2_ifclk_Clock}]
set_clock_groups -exclusive -group [get_clocks {FX2_multi_clk_Clock}]
set_clock_groups -exclusive -group [get_clocks {dac_pll_inst|altpll_component|auto_generated|pll1|clk[0]}]


#**************************************************************
# Set False Path
#**************************************************************



#**************************************************************
# Set Multicycle Path
#**************************************************************



#**************************************************************
# Set Maximum Delay
#**************************************************************



#**************************************************************
# Set Minimum Delay
#**************************************************************



#**************************************************************
# Set Input Transition
#**************************************************************



#**************************************************************
# Set Load
#**************************************************************

