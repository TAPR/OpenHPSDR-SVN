create_clock -period 122.88MHz   -name LTC2208_122MHz [get_ports LTC2208_122MHz]
create_clock -period 122.88MHz   -name _122MHz _122MHz
create_clock -period 10.000MHz   -name OSC_10MHZ [get_ports OSC_10MHZ]
create_clock -period 125.00MHz   -name PHY_CLK125 [get_ports PHY_CLK125]
create_clock -period  25.00MHz   -name CLK_25MHZ [get_ports CLK_25MHZ]
create_clock -period  25.00MHz   -name PHY_RX_CLOCK [get_ports PHY_RX_CLOCK]
create_clock -period  3.072MHz   -name Hermes_clk_lrclk_gen:clrgen|BCLK Hermes_clk_lrclk_gen:clrgen|BCLK
create_clock -period  12.50MHz   -name Rx_MAC:Rx_MAC_inst|PHY_100T_state Rx_MAC:Rx_MAC_inst|PHY_100T_state
create_clock -period  0.768MHz   -name spc[1] spc[1]
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
										 -group {Hermes_clk_lrclk_gen:clrgen|BCLK} \
										 -group {CLK_25MHZ} \
										 -group {Rx_MAC:Rx_MAC_inst|PHY_100T_state} \
										 -group {spc[1]} \
										 -group {Attenuator:Attenuator_inst|clk_2} \
										 -group {LTC2208_122MHz} \
										 -group {PLL_IF_inst|altpll_component|auto_generated|pll1|clk[0]} \
										 -group {_122MHz}
										 
# Add internally generated clocks
# create_generated_clock -divide_by 4 -source Hermes_clk_lrclk_gen:clrgen|BCLK -name spc[1]

# for an inverted clock creat a clock using the -invert option  in create_generated_clock