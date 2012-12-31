# NOTE: When using -name <value> the value is what you want to call it for use in the sdc file.
# for example, we can refer to the clock [get_ports LTC2208_122MHz]  as just LTC2208_122MHz
# if you don't give a -name then use the full name of the clock.

# [get_ports <name>] assigns the name of a clock connected to an FPGA input PIN

# you can set clock frequency in MHz rather than using nS by placing MHz after the period value.


create_clock -period 122.88MHz   -name LTC2208_122MHz [get_ports LTC2208_122MHz]  
create_clock -period 122.88MHz   -name _122MHz _122MHz
create_clock -period 10.000MHz   -name OSC_10MHZ [get_ports OSC_10MHZ]
create_clock -period 125.00MHz   -name PHY_CLK125 [get_ports PHY_CLK125]
create_clock -period  25.00MHz   -name CLK_25MHZ [get_ports CLK_25MHZ]
create_clock -period  25.00MHz   -name PHY_RX_CLOCK [get_ports PHY_RX_CLOCK]
create_clock -period  12.50MHz   -name Rx_MAC:Rx_MAC_inst|PHY_100T_state Rx_MAC:Rx_MAC_inst|PHY_100T_state
create_clock -period  24.00MHz   -name Attenuator:Attenuator_inst|clk_2 Attenuator:Attenuator_inst|clk_2 

derive_pll_clocks

derive_clock_uncertainty

set_clock_groups -asynchronous -group {OSC_10MHZ PLL2_inst|altpll_component|auto_generated|pll1|clk[0]} \
                               -group {PLL_IF_inst|altpll_component|auto_generated|pll1|clk[1] \
													PLL_IF_inst|altpll_component|auto_generated|pll1|clk[2] }\
										 -group {PHY_CLK125 PLL_clocks_inst|altpll_component|auto_generated|pll1|clk[0]  \
													PLL_clocks_inst|altpll_component|auto_generated|pll1|clk[1]  \
													PLL_clocks_inst|altpll_component|auto_generated|pll1|clk[2] } \
										 -group {PLL_inst|altpll_component|auto_generated|pll1|clk[0]} \
										 -group {CLK_25MHZ} \
										 -group {Rx_MAC:Rx_MAC_inst|PHY_100T_state} \
										 -group {LTC2208_122MHz} \
										 -group {PLL_IF_inst|altpll_component|auto_generated|pll1|clk[0]} \
										 -group {_122MHz} \
										 -group {Attenuator:Attenuator_inst|clk_2} 										 
										 
# Add internally generated clocks - for an inverted clock creat a clock using the -invert option 
create_generated_clock -divide_by 40 -source [get_ports LTC2208_122MHz] -name Hermes_clk_lrclk_gen:clrgen|BCLK Hermes_clk_lrclk_gen:clrgen|BCLK
create_generated_clock -divide_by 4  -source Hermes_clk_lrclk_gen:clrgen|BCLK -name spc[1] spc[1]
create_generated_clock -divide_by 2  -source PHY_RX_CLOCK -name PHY_RX_CLOCK_2 PHY_RX_CLOCK_2


set_false_path -from {cdc_sync:freq*|sigb[*]}  -to {C122_sync_phase_word[*][*]}
set_false_path -from {cdc_sync:freq*|sigb[*]}  -to {C122_sync_phase_word_Tx[*]}
set_false_path -from {Hermes_clk_lrclk_gen:clrgen|BCLK} -to {PLL_IF_inst|altpll_component|auto_generated|pll1|clk[*]}
set_false_path -from {spc[1]} -to {Hermes_clk_lrclk_gen:clrgen|BCLK} 
set_false_path -from PLL_IF_inst|altpll_component|auto_generated|pll1|clk[0] -to spc[1]
set_false_path -from LTC2208_122MHz -to spc[1]
# note multiple targets in the next line
set_false_path -from PHY_RX_CLOCK_2 -to {PLL_IF_inst|altpll_component|auto_generated|pll1|clk[0]  LTC2208_122MHz}

 
set_multicycle_path -from {receiver:MDC[*].receiver_inst|memcic:memcic_inst_*|memcic_ram:memcic_ram_inst|altsyncram:altsyncram_component|altsyncram_1en1:auto_generated|q_b[*]} -to {receiver:MDC[*].receiver_inst|memcic:memcic_inst_*|memcic_ram:memcic_ram_inst|altsyncram:altsyncram_component|altsyncram_1en1:auto_generated|ram_block1a*~porta_datain_reg*} -setup -end 2
set_multicycle_path -from {receiver:MDC[*].receiver_inst|memcic:memcic_inst_*|memcic_ram:memcic_ram_inst|altsyncram:altsyncram_component|altsyncram_1en1:auto_generated|q_b[*]} -to {receiver:MDC[*].receiver_inst|memcic:memcic_inst_*|memcic_ram:memcic_ram_inst|altsyncram:altsyncram_component|altsyncram_1en1:auto_generated|ram_block1a*~porta_datain_reg*} -hold  -end 1

set_multicycle_path -from {receiver:MDC[*].receiver_inst|memcic:memcic_inst_*|work_reg[*]} -to {receiver:MDC[*].receiver_inst|memcic:memcic_inst_*|memcic_ram:memcic_ram_inst|altsyncram:altsyncram_component|altsyncram_1en1:auto_generated|ram_block1a*~porta_datain_reg0} -setup -end 2
set_multicycle_path -from {receiver:MDC[*].receiver_inst|memcic:memcic_inst_*|work_reg[*]} -to {receiver:MDC[*].receiver_inst|memcic:memcic_inst_*|memcic_ram:memcic_ram_inst|altsyncram:altsyncram_component|altsyncram_1en1:auto_generated|ram_block1a*~porta_datain_reg0} -hold  -end 1

set_multicycle_path -from {receiver:MDC[*].receiver_inst|memcic:memcic_inst_*|memcic_ram:memcic_ram_inst|altsyncram:altsyncram_component|altsyncram_1en1:auto_generated|q_b[*]} -to {receiver:MDC[*].receiver_inst|memcic:memcic_inst_*|work_reg[*]} -setup -end 2
set_multicycle_path -from {receiver:MDC[*].receiver_inst|memcic:memcic_inst_*|memcic_ram:memcic_ram_inst|altsyncram:altsyncram_component|altsyncram_1en1:auto_generated|q_b[*]} -to {receiver:MDC[*].receiver_inst|memcic:memcic_inst_*|work_reg[*]} -hold  -end 1

set_multicycle_path -from {cdc_sync:freq*|sigb[*]~_Duplicate_2} -to {C122_freq_*[*]} -setup -end 2
set_multicycle_path -from {cdc_sync:freq*|sigb[*]~_Duplicate_2} -to {C122_freq_*[*]} -hold  -end 1

set_multicycle_path -from {receiver:MDC[*].receiver_inst|memcic:memcic_inst_*|work_reg[*]} -to {receiver:MDC[*].receiver_inst|memcic:memcic_inst_*|work_reg[*]} -setup -end 2
set_multicycle_path -from {receiver:MDC[*].receiver_inst|memcic:memcic_inst_*|work_reg[*]} -to {receiver:MDC[*].receiver_inst|memcic:memcic_inst_*|work_reg[*]} -hold  -end 1

set_multicycle_path -from {cdc_sync:rate|sigb[*]} -to {receiver:MDC[*].receiver_inst|varcic:varcic_inst_*|out_data[*]} -setup -end 2
set_multicycle_path -from {cdc_sync:rate|sigb[*]} -to {receiver:MDC[*].receiver_inst|varcic:varcic_inst_*|out_data[*]} -hold  -end 1


