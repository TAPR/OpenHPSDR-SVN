# Hermes.sdc
#
#
#
#**************************************************************
# Time Information
#**************************************************************

set_time_format -unit ns -decimal_places 3


#**************************************************************************************
# Create Clock
#**************************************************************************************
# externally generated clocks (with respect to the FPGA)
#
create_clock -period 122.880MHz	-name LTC2208_122MHz    [get_ports LTC2208_122MHz]
create_clock -period 122.880MHz	-name _122MHz		[get_ports _122MHz]
create_clock -period  10.000MHz	-name OSC_10MHZ		[get_ports OSC_10MHZ]
create_clock -period 125.000MHz	-name PHY_CLK125	[get_ports PHY_CLK125]
create_clock -period  25.000MHz	-name PHY_RX_CLOCK	[get_ports PHY_RX_CLOCK]


derive_pll_clocks

derive_clock_uncertainty


#*************************************************************************************
# Create Generated CloCK
#*************************************************************************************
# internally generated clocks
#
create_generated_clock -name Hermes_clk_lrclk_gen:clrgen|BCLK -source [get_ports LTC2208_122MHz] -divide_by 40 Hermes_clk_lrclk_gen:clrgen|BCLK
create_generated_clock -name spc[2] -source Hermes_clk_lrclk_gen:clrgen|BCLK -divide_by 8 spc[2]
create_generated_clock -name PHY_RX_CLOCK_2 -source PHY_RX_CLOCK -divide_by 2 PHY_RX_CLOCK_2
create_generated_clock -name {Attenuator:Attenuator_inst|clk_2} -source [get_pins {PLL_IF_inst|altpll_component|auto_generated|pll1|clk[0]}] -multiply_by 1 [get_registers {Attenuator:Attenuator_inst|clk_2}]

#*************************************************************************************
# Set Clock Groups
#*************************************************************************************
# Note: output clock c0 (48.034909 MHz) of PLL_IF_inst is asynchronous with input source clock inclk0 (122.88MHz)
#
set_clock_groups -asynchronous -group {PHY_CLK125 \
					PLL_clocks_inst|altpll_component|auto_generated|pll1|clk[0] \
					PLL_clocks_inst|altpll_component|auto_generated|pll1|clk[1] \
					PLL_clocks_inst|altpll_component|auto_generated|pll1|clk[2] \
					PHY_RX_CLOCK \
					PHY_RX_CLOCK_2 \
					} \
				-group {_122MHz \
					LTC2208_122MHz \
					Hermes_clk_lrclk_gen:clrgen|BCLK \
					spc[2] \
					PLL_IF_inst|altpll_component|auto_generated|pll1|clk[1] \
					PLL_IF_inst|altpll_component|auto_generated|pll1|clk[2] \
					} \
				-group {PLL2_OSC_10MHz \
					PLL2_inst|altpll_component|auto_generated|pll1|clk[0]} \
				-group	{PLL_IF_inst|altpll_component|auto_generated|pll1|clk[0] \
					Attenuator:Attenuator_inst|clk_2}
										 

#*************************************************************************************************************
# set input delay
#*************************************************************************************************************

set_input_delay -add_delay -max -clock PHY_CLK125 0.000 {PHY_MDIO PHY_RX[*] RX_DV PHY_INT_N ASMI_interface:ASMI_int_inst|ASMI:ASMI_inst|ASMI_altasmi_parallel_cv82:ASMI_altasmi_parallel_cv82_component|cycloneii_asmiblock2~ALTERA_DATA0}
set_input_delay -add_delay -min -clock PHY_CLK125 0.000 {PHY_MDIO PHY_RX[*] RX_DV PHY_INT_N ASMI_interface:ASMI_int_inst|ASMI:ASMI_inst|ASMI_altasmi_parallel_cv82:ASMI_altasmi_parallel_cv82_component|cycloneii_asmiblock2~ALTERA_DATA0}
#
set_input_delay -add_delay -max -clock LTC2208_122MHz 0.000 {ADCMISO ANT_TUNE CDOUT INA[*] INA_2[*] IO4 IO5 IO6 IO7 IO8 KEY_DASH KEY_DOT OVERFLOW PTT SO SPI_SDI}
set_input_delay -add_delay -min -clock LTC2208_122MHz 0.000 {ADCMISO ANT_TUNE CDOUT INA[*] INA_2[*] IO4 IO5 IO6 IO7 IO8 KEY_DASH KEY_DOT OVERFLOW PTT SO SPI_SDI}


#*************************************************************************************************************
# set output delay
#*************************************************************************************************************

set_output_delay -add_delay -max -clock PHY_CLK125 0.000 {PHY_MDIO PHY_TX[*] PHY_TX_EN PHY_TX_CLOCK PHY_MDC ASMI_interface:ASMI_int_inst|ASMI:ASMI_inst|ASMI_altasmi_parallel_cv82:ASMI_altasmi_parallel_cv82_component|cycloneii_asmiblock2~ALTERA_SCE ASMI_interface:ASMI_int_inst|ASMI:ASMI_inst|ASMI_altasmi_parallel_cv82:ASMI_altasmi_parallel_cv82_component|cycloneii_asmiblock2~ALTERA_SDO ASMI_interface:ASMI_int_inst|ASMI:ASMI_inst|ASMI_altasmi_parallel_cv82:ASMI_altasmi_parallel_cv82_component|cycloneii_asmiblock2~ALTERA_DCLK}
set_output_delay -add_delay -min -clock PHY_CLK125 0.000 {PHY_MDIO PHY_TX[*] PHY_TX_EN PHY_TX_CLOCK PHY_MDC ASMI_interface:ASMI_int_inst|ASMI:ASMI_inst|ASMI_altasmi_parallel_cv82:ASMI_altasmi_parallel_cv82_component|cycloneii_asmiblock2~ALTERA_SCE ASMI_interface:ASMI_int_inst|ASMI:ASMI_inst|ASMI_altasmi_parallel_cv82:ASMI_altasmi_parallel_cv82_component|cycloneii_asmiblock2~ALTERA_SDO ASMI_interface:ASMI_int_inst|ASMI:ASMI_inst|ASMI_altasmi_parallel_cv82:ASMI_altasmi_parallel_cv82_component|cycloneii_asmiblock2~ALTERA_DCLK}
 
set_output_delay -add_delay -max -clock _122MHz 0.000 {ADCCLK ADCMOSI ATTN_CLK ATTN_DATA  ATTN_LE  CBCLK CDIN CLRCIN CLRCOUT CMCLK CS DACD[*] DEBUG_LED* DITH FPGA_PLL FPGA_PTT J15_5 J15_6 MICBIAS_ENABLE MICBIAS_SELECT MIC_SIG_SELECT MOSI NCONFIG PTT_SELECT RAND SCK SI SPI_SCK SPI_SDO SSCK Status_LED USEROUT* nADCCS nCS DAC_ALC}
set_output_delay -add_delay -min -clock _122MHz 0.000 {ADCCLK ADCMOSI ATTN_CLK ATTN_DATA ATTN_LE CBCLK CDIN CLRCIN CLRCOUT CMCLK CS DACD[*] DEBUG_LED* DITH FPGA_PLL FPGA_PTT J15_5 J15_6 MICBIAS_ENABLE MICBIAS_SELECT MIC_SIG_SELECT MOSI NCONFIG PTT_SELECT RAND SCK SI SPI_SCK SPI_SDO SSCK Status_LED USEROUT* nADCCS nCS DAC_ALC}
#

#*************************************************************************************************************
# Set Maximum Delay
#*************************************************************************************************************
#

set_max_delay -from _122MHz -to _122MHz 13

set_max_delay -from Hermes_clk_lrclk_gen:clrgen|BCLK -to _122MHz 13

set_max_delay -from LTC2208_122MHz -to LTC2208_122MHz 14
set_max_delay -from LTC2208_122MHz -to _122MHz 14
set_max_delay -from LTC2208_122MHz -to spc[2] 11

set_max_delay -from PLL_clocks_inst|altpll_component|auto_generated|pll1|clk[0] -to PHY_CLK125 16

set_max_delay -from PLL_clocks_inst|altpll_component|auto_generated|pll1|clk[1] -to PHY_CLK125 14

set_max_delay -from PLL_IF_inst|altpll_component|auto_generated|pll1|clk[2] -to _122MHz 11

set_max_delay -from spc[2] -to _122MHz 35


#*************************************************************************************************************
# Set Minimum Delay
#*************************************************************************************************************
#

set_min_delay -from _122MHz -to _122MHz -2

set_min_delay -from Hermes_clk_lrclk_gen:clrgen|BCLK -to _122MHz 5

set_min_delay -from LTC2208_122MHz -to LTC2208_122MHz -2
set_min_delay -from LTC2208_122MHz -to _122MHz 0
set_min_delay -from LTC2208_122MHz -to spc[2] -1

set_min_delay -from PLL_clocks_inst|altpll_component|auto_generated|pll1|clk[0] -to PHY_CLK125 5

set_min_delay -from PLL_clocks_inst|altpll_component|auto_generated|pll1|clk[1] -to PHY_CLK125 1

set_min_delay -from PLL_IF_inst|altpll_component|auto_generated|pll1|clk[2] -to _122MHz 3

set_min_delay -from spc[2] -to _122MHz 12
