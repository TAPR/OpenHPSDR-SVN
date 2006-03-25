EESchema Schematic File Version 1
LIBS:power,o_device,o_assembly,o_analog,HPSDR,device,conn,linear,regul,74xx,cmos4000,adc-dac,memory,xilinx,special,microcontrollers,microchip,analog_switches,motorola,intel,audio,interface,digital-audio,philips,display,cypress,siliconi,contrib,.\fpga_usb.cache
EELAYER 23  0
EELAYER END
$Descr C 22000 16000
Sheet 1 1
Title "FPGA-USB BOARD - EP2C5_208"
Date "24 mar 2006"
Rev "None"
Comp "OPEN HIGH PERFORMANCE SDR PROJECT"
Comment1 ""
Comment2 ""
Comment3 ""
Comment4 ""
$EndDescr
Wire Wire Line
	13200 8150 13200 8900
Text GLabel 13200 8900 1    60   Input
1.2VPLL
Text GLabel 18200 1500 3    60   Input
1.2VPLL
Wire Wire Line
	18200 2250 18200 1500
$Comp
L +1.2V #PWR?
U 1 1 442449F6
P 19900 2750
F 0 "#PWR?" H 19900 2890 20  0001 C C
F 1 "+1.2V" V 19900 2950 30  0000 C C
	1    19900 2750
	0    1    1    0   
$EndComp
Wire Wire Line
	19150 2750 19900 2750
$Comp
L +1.2V #PWR?
U 1 1 442449CA
P 11600 7650
F 0 "#PWR?" H 11600 7790 20  0001 C C
F 1 "+1.2V" V 11600 7850 30  0000 C C
	1    11600 7650
	0    -1   -1   0   
$EndComp
Wire Wire Line
	12250 7650 11600 7650
Connection ~ 18100 1750
Wire Wire Line
	18100 2250 18100 1750
Connection ~ 11800 7750
Wire Wire Line
	12250 7750 11800 7750
Connection ~ 11800 7550
Wire Wire Line
	12250 7550 11800 7550
Connection ~ 13300 8350
Wire Wire Line
	13300 8150 13300 8350
Connection ~ 19750 2650
Wire Wire Line
	19150 2650 19750 2650
Connection ~ 19750 2850
Wire Wire Line
	19150 2850 19750 2850
Wire Wire Line
	900  4850 1200 4850
Wire Wire Line
	1900 4850 1600 4850
Wire Wire Line
	1900 5000 1900 4450
Wire Wire Line
	1900 5450 1900 5300
Wire Wire Line
	900  5450 900  5300
Wire Wire Line
	900  4350 900  5000
Wire Wire Line
	1700 14250 1700 14400
Wire Wire Line
	1700 13950 1700 13700
Wire Wire Line
	11150 7350 12250 7350
Wire Wire Line
	12250 7250 11150 7250
Wire Wire Line
	11150 7150 12250 7150
$Comp
L CONN_3 K?
U 1 1 442446A8
P 10800 7250
F 0 "K?" V 10750 7250 50  0000 C C
F 1 "CONN_3" V 10850 7250 40  0000 C C
	1    10800 7250
	-1   0    0    -1  
$EndComp
Wire Wire Line
	13600 850  13600 2250
Wire Wire Line
	12900 850  13600 850 
Wire Wire Line
	13400 950  12900 950 
Wire Wire Line
	13400 2250 13400 950 
Wire Wire Line
	13300 1050 13300 2250
Wire Wire Line
	12900 1050 13300 1050
Wire Wire Line
	13200 1150 12900 1150
Wire Wire Line
	13200 2250 13200 1150
Wire Wire Line
	13100 1250 13100 2250
Wire Wire Line
	12900 1250 13100 1250
$Comp
L CONN_5 P?
U 1 1 4424466D
P 12500 1050
F 0 "P?" V 12450 1050 50  0000 C C
F 1 "CONN_5" V 12550 1050 50  0000 C C
	1    12500 1050
	-1   0    0    -1  
$EndComp
$Comp
L DGND #PWR?
U 1 1 44244644
P 18500 11250
F 0 "#PWR?" H 18500 11250 40  0001 C C
F 1 "DGND" H 18500 11180 40  0000 C C
	1    18500 11250
	1    0    0    -1  
$EndComp
Wire Wire Line
	18500 11050 18500 11250
Wire Wire Line
	18800 11050 18500 11050
Wire Wire Line
	18800 10850 15900 10850
Wire Wire Line
	15800 10950 18800 10950
Wire Wire Line
	15800 8150 15800 10950
Wire Wire Line
	15900 10850 15900 8150
Wire Wire Line
	16000 10750 18800 10750
Wire Wire Line
	16000 8150 16000 10750
Wire Wire Line
	16200 10650 18800 10650
Wire Wire Line
	16200 8150 16200 10650
Wire Wire Line
	16400 10550 18800 10550
Wire Wire Line
	16400 8150 16400 10550
Wire Wire Line
	16500 10450 18800 10450
Wire Wire Line
	16500 8150 16500 10450
Wire Wire Line
	16600 10350 18800 10350
Wire Wire Line
	16600 8150 16600 10350
Wire Wire Line
	16700 10250 18800 10250
Wire Wire Line
	16700 8150 16700 10250
Wire Wire Line
	16800 10150 18800 10150
Wire Wire Line
	16800 8150 16800 10150
Wire Wire Line
	17000 10050 18800 10050
Wire Wire Line
	17000 8150 17000 10050
Wire Wire Line
	17200 9950 18800 9950
Wire Wire Line
	17200 8150 17200 9950
Wire Wire Line
	17300 9850 18800 9850
Wire Wire Line
	17300 8150 17300 9850
Wire Wire Line
	17400 9750 18800 9750
Wire Wire Line
	17400 8150 17400 9750
Wire Wire Line
	17500 9650 17500 8150
Wire Wire Line
	17500 9650 18800 9650
Wire Wire Line
	17700 9550 18800 9550
Wire Wire Line
	17700 8150 17700 9550
Wire Wire Line
	17900 9450 18800 9450
Wire Wire Line
	17900 8150 17900 9450
Wire Wire Line
	18000 9350 18800 9350
Wire Wire Line
	18000 8150 18000 9350
Wire Wire Line
	18100 9250 18800 9250
Wire Wire Line
	18100 8150 18100 9250
Wire Wire Line
	18200 9150 18800 9150
Wire Wire Line
	18200 8150 18200 9150
$Comp
L CONN_20 P?
U 1 1 442445C9
P 19150 10100
F 0 "P?" V 19100 10100 60  0000 C C
F 1 "CONN_20" V 19200 10100 60  0000 C C
	1    19150 10100
	1    0    0    -1  
$EndComp
Text GLabel 11600 7050 0    60   BiDi
FX2_PE7
Text GLabel 11600 6950 0    60   BiDi
FX2_PE6
Text GLabel 11600 6850 0    60   BiDi
FX2_PE5/INT6
Text GLabel 11600 6650 0    60   BiDi
FX2_PE4
Text GLabel 11600 6550 0    60   BiDi
FX2_PE3
Text GLabel 11600 6450 0    60   BiDi
FX2_PE2
Text GLabel 11600 6250 0    60   BiDi
FX2_PE1
Text GLabel 11600 6150 0    60   BiDi
FX2_PE0
Text GLabel 11600 6050 0    60   BiDi
RDY5
Text GLabel 11600 5950 0    60   BiDi
RDY4
Text GLabel 11600 5850 0    60   BiDi
RDY3
Text GLabel 11600 5750 0    60   BiDi
RDY2
Text GLabel 11600 5650 0    60   BiDi
RDY1/SLWR
Text GLabel 11600 5550 0    60   BiDi
RDY0/SLRD
Text GLabel 13800 1500 3    60   BiDi
FX2_PC0
Text GLabel 13900 1500 3    60   Output
FX2_INT5
Text GLabel 14000 1500 3    60   Output
FX2_INT4
Wire Wire Line
	13800 2250 13800 1500
Wire Wire Line
	13900 2250 13900 1500
Wire Wire Line
	14000 2250 14000 1500
Wire Wire Line
	19150 7750 19900 7750
Wire Wire Line
	19900 7650 19150 7650
Wire Wire Line
	19150 7550 19900 7550
Text GLabel 19900 7750 2    60   BiDi
FX2_CTL5
Text GLabel 19900 7650 2    60   BiDi
FX2_CTL4
Text GLabel 19900 7550 2    60   BiDi
FX2_CTL3
Wire Wire Line
	19150 7450 19900 7450
Wire Wire Line
	19900 7250 19150 7250
Text GLabel 19900 7450 2    60   BiDi
FX2_CTL2/FLAGC
Text GLabel 19900 7250 2    60   BiDi
FX2_CTL1/FLAGB
Text GLabel 19900 7050 2    60   BiDi
FX2_CTL0/FLAGA
Text GLabel 19900 6950 2    60   BiDi
X26DC
Text GLabel 19900 6850 2    60   BiDi
X25DC
Text GLabel 19900 6750 2    60   BiDi
X24DC
Text GLabel 19900 6650 2    60   BiDi
X23C7
Wire Wire Line
	19150 7050 19900 7050
Wire Wire Line
	19150 6950 19900 6950
Wire Wire Line
	19150 6850 19900 6850
Wire Wire Line
	19150 6750 19900 6750
Wire Wire Line
	19150 6650 19900 6650
Text GLabel 11600 3650 0    60   BiDi
FX2_PA0
Text GLabel 11600 3550 0    60   BiDi
FX2_PA1
Text GLabel 11600 3150 0    60   BiDi
FX2_PA2/SLOE
Text GLabel 11600 3050 0    60   BiDi
FX2_PA3
Text GLabel 11600 2950 0    60   BiDi
FX2_PA4/FIFOADR0
Text GLabel 11600 2850 0    60   BiDi
FX2_PA5/FIFOADR1
Text GLabel 11600 2750 0    60   BiDi
FX2_PA6/PKTEND
Text GLabel 11600 2650 0    60   BiDi
FX2_PA7/FLAGD/SLCS
Wire Wire Line
	12250 2650 11600 2650
Wire Wire Line
	12250 2750 11600 2750
Wire Wire Line
	12250 2850 11600 2850
Wire Wire Line
	12250 2950 11600 2950
Wire Wire Line
	12250 3050 11600 3050
Wire Wire Line
	12250 3150 11600 3150
Wire Wire Line
	12250 4050 11600 4050
Wire Wire Line
	12250 3950 11600 3950
Wire Wire Line
	12250 3850 11600 3850
Wire Wire Line
	12250 3750 11600 3750
Wire Wire Line
	12250 3650 11600 3650
Wire Wire Line
	12250 3550 11600 3550
Wire Wire Line
	11800 1900 11800 8850
Wire Wire Line
	12250 7050 11600 7050
Wire Wire Line
	12250 6950 11600 6950
Wire Wire Line
	12250 6850 11600 6850
Wire Wire Line
	12250 6650 11600 6650
Wire Wire Line
	12250 6550 11600 6550
Wire Wire Line
	12250 6450 11600 6450
Wire Wire Line
	12250 6250 11600 6250
Wire Wire Line
	12250 6150 11600 6150
Wire Wire Line
	12250 6050 11600 6050
Wire Wire Line
	12250 5950 11600 5950
Wire Wire Line
	12250 5850 11600 5850
Wire Wire Line
	12250 5750 11600 5750
Wire Wire Line
	12250 5650 11600 5650
Wire Wire Line
	12250 5550 11600 5550
Wire Wire Line
	15500 8900 15500 8150
Wire Wire Line
	15400 8150 15400 8900
Wire Wire Line
	15300 8900 15300 8150
Wire Wire Line
	15100 8150 15100 8900
Wire Wire Line
	14900 8900 14900 8150
Wire Wire Line
	14800 8150 14800 8900
Wire Wire Line
	14700 8900 14700 8150
Wire Wire Line
	14600 8900 14600 8150
Text GLabel 15500 8900 1    60   BiDi
FX2_FD15
Text GLabel 15400 8900 1    60   BiDi
FX2_FD14
Text GLabel 15300 8900 1    60   BiDi
FX2_FD13
Text GLabel 15100 8900 1    60   BiDi
FX2_FD12
Text GLabel 14900 8900 1    60   BiDi
FX2_FD11
Text GLabel 14800 8900 1    60   BiDi
FX2_FD10
Text GLabel 14700 8900 1    60   BiDi
FX2_FD9
Text GLabel 14600 8900 1    60   BiDi
FX2_FD8
Text GLabel 14300 8900 1    60   BiDi
FX2_FD7
Text GLabel 14200 8900 1    60   BiDi
FX2_FD6
Text GLabel 14000 8900 1    60   BiDi
FX2_FD5
Text GLabel 13900 8900 1    60   BiDi
FX2_FD4
Text GLabel 13800 8900 1    60   BiDi
FX2_FD3
Text GLabel 13700 8900 1    60   BiDi
FX2_FD2
Text GLabel 13600 8900 1    60   BiDi
FX2_FD1
Text GLabel 13500 8900 1    60   BiDi
FX2_FD0
Connection ~ 17100 8350
Wire Wire Line
	17800 8350 17800 8150
Wire Wire Line
	11800 8350 17800 8350
Connection ~ 16300 8350
Wire Wire Line
	17100 8350 17100 8150
Connection ~ 15600 8350
Wire Wire Line
	16300 8350 16300 8150
Connection ~ 15200 8350
Wire Wire Line
	15600 8350 15600 8150
Connection ~ 14400 8350
Wire Wire Line
	15200 8350 15200 8150
Connection ~ 13400 8350
Wire Wire Line
	13400 8150 13400 8350
Connection ~ 11800 8350
Wire Wire Line
	14400 8150 14400 8350
Wire Wire Line
	14300 8150 14300 8900
Wire Wire Line
	14200 8150 14200 8900
Wire Wire Line
	14000 8150 14000 8900
Wire Wire Line
	13900 8150 13900 8900
Wire Wire Line
	13800 8150 13800 8900
Wire Wire Line
	13700 8150 13700 8900
Wire Wire Line
	13600 8150 13600 8900
Wire Wire Line
	13500 8150 13500 8900
Text Notes 8100 7250 0    60   ~
FX2
Text Notes 18350 7950 0    60   ~
CYCLONE II
Wire Wire Line
	14800 2250 14800 1500
Wire Wire Line
	14700 1500 14700 2250
Wire Wire Line
	14600 2250 14600 1500
Wire Wire Line
	14400 1500 14400 2250
Wire Wire Line
	14200 2250 14200 1500
Wire Wire Line
	14100 1500 14100 2250
Text GLabel 14100 1500 3    60   BiDi
Y26DC
Text GLabel 14200 1500 3    60   BiDi
Y25DC
Text GLabel 14400 1500 3    60   BiDi
Y24DC
Text GLabel 14600 1500 3    60   BiDi
Y23C7
Text GLabel 14700 1500 3    60   BiDi
Y22C6
Text GLabel 14800 1500 3    60   BiDi
Y21C5
Text GLabel 15000 1500 3    60   BiDi
Y20C4
Text GLabel 15100 1500 3    60   BiDi
Y19C3
Text GLabel 15200 1500 3    60   BiDi
Y18C2
Text GLabel 15400 1500 3    60   BiDi
Y17C1
Text GLabel 15700 1500 3    60   BiDi
Y16C0
Text GLabel 15800 1500 3    60   BiDi
Y15B7
Text GLabel 15900 1500 3    60   BiDi
Y14B6
Text GLabel 16000 1500 3    60   BiDi
Y13B5
Wire Wire Line
	15400 2250 15400 1500
Wire Wire Line
	15000 2250 15000 1500
Wire Wire Line
	15100 2250 15100 1500
Wire Wire Line
	15200 2250 15200 1500
Wire Wire Line
	15700 2250 15700 1500
Wire Wire Line
	15800 2250 15800 1500
Wire Wire Line
	15900 2250 15900 1500
Wire Wire Line
	16000 2250 16000 1500
Connection ~ 17200 1900
Wire Wire Line
	18000 1900 18000 2250
Wire Wire Line
	11800 1900 18000 1900
Connection ~ 16500 1900
Wire Wire Line
	17200 1900 17200 2250
Connection ~ 16200 1900
Wire Wire Line
	16500 1900 16500 2250
Connection ~ 15500 1900
Wire Wire Line
	16200 1900 16200 2250
Connection ~ 15300 1900
Wire Wire Line
	15500 1900 15500 2250
Connection ~ 14300 1900
Wire Wire Line
	15300 1900 15300 2250
Connection ~ 13500 1900
Wire Wire Line
	14300 1900 14300 2250
Connection ~ 11800 3450
Wire Wire Line
	13500 1900 13500 2250
Wire Wire Line
	16300 2250 16300 1500
Wire Wire Line
	16400 1500 16400 2250
Wire Wire Line
	16600 2250 16600 1500
Wire Wire Line
	16800 1500 16800 2250
Wire Wire Line
	16900 2250 16900 1500
Wire Wire Line
	17000 1500 17000 2250
Wire Wire Line
	17100 2250 17100 1500
Wire Wire Line
	17400 2250 17400 1500
Wire Wire Line
	17500 1500 17500 2250
Wire Wire Line
	17600 2250 17600 1500
Wire Wire Line
	17700 1500 17700 2250
Wire Wire Line
	17800 2250 17800 1500
Wire Wire Line
	17900 1500 17900 2250
Text GLabel 16300 1500 3    60   BiDi
Y12B4
Text GLabel 16400 1500 3    60   BiDi
Y11B3
Text GLabel 16600 1500 3    60   BiDi
Y10B2
Text GLabel 16800 1500 3    60   BiDi
Y9B1
Text GLabel 16900 1500 3    60   BiDi
Y8B0
Text GLabel 17000 1500 3    60   BiDi
Y7A7
Text GLabel 17100 1500 3    60   BiDi
Y6A6
Text GLabel 17400 1500 3    60   BiDi
Y5A5
Text GLabel 17500 1500 3    60   BiDi
Y4A4
Text GLabel 17600 1500 3    60   BiDi
Y3A3
Text GLabel 17700 1500 3    60   BiDi
Y2A2
Text GLabel 17800 1500 3    60   BiDi
Y1A1
Text GLabel 17900 1500 3    60   BiDi
Y0A0
Wire Wire Line
	19150 6550 19900 6550
Wire Wire Line
	19900 6450 19150 6450
Wire Wire Line
	19150 6350 19900 6350
Text GLabel 19900 6550 2    60   BiDi
X22C6
Text GLabel 19900 6450 2    60   BiDi
X21C5
Text GLabel 19900 6350 2    60   BiDi
X20C4
Wire Wire Line
	19900 6250 19150 6250
Text GLabel 19900 6250 2    60   BiDi
X19C3
Text GLabel 19900 5550 2    60   BiDi
X18C2
Text GLabel 19900 5450 2    60   BiDi
X17C1
Text GLabel 19900 5350 2    60   Input
FPGA_CLK7IN
Text GLabel 19900 5250 2    60   Input
FPGA_CLK6IN
Text GLabel 19900 5150 2    60   Input
FPGA_CLK5IN
Text GLabel 19900 5050 2    60   Input
FPGA_CLK4IN
Text GLabel 19900 4950 2    60   BiDi
X16C0
Text GLabel 19900 4850 2    60   BiDi
X15B7
Text GLabel 19900 4750 2    60   BiDi
X14B6
Text GLabel 19900 4550 2    60   BiDi
X13B5
Text GLabel 19900 4450 2    60   BiDi
X12B4
Text GLabel 19900 4350 2    60   BiDi
X11B3
Text GLabel 19900 4150 2    60   BiDi
X10B2
Text GLabel 19900 4050 2    60   BiDi
X9B1
Text GLabel 19900 3950 2    60   BiDi
X8B0
Text GLabel 19900 3850 2    60   BiDi
X7A7
Text GLabel 19900 3750 2    60   BiDi
X6A6
Text GLabel 19900 3650 2    60   BiDi
X5A5
Text GLabel 19900 3550 2    60   BiDi
X4A4
Text GLabel 19900 3350 2    60   BiDi
X3A3
Text GLabel 19900 3250 2    60   BiDi
X2A2
Text GLabel 19900 3150 2    60   BiDi
X1A1
Text GLabel 19900 3050 2    60   BiDi
X0A0
Wire Wire Line
	19150 5550 19900 5550
Wire Wire Line
	19150 5450 19900 5450
Wire Wire Line
	19150 5350 19900 5350
Wire Wire Line
	19150 5250 19900 5250
Wire Wire Line
	19150 5150 19900 5150
Wire Wire Line
	19150 5050 19900 5050
Wire Wire Line
	19150 4950 19900 4950
Wire Wire Line
	19150 4850 19900 4850
Wire Wire Line
	19150 4750 19900 4750
Wire Wire Line
	19150 4550 19900 4550
Wire Wire Line
	19150 4450 19900 4450
Wire Wire Line
	19150 4350 19900 4350
Wire Wire Line
	19150 4150 19900 4150
Wire Wire Line
	19150 4050 19900 4050
Wire Wire Line
	19150 3950 19900 3950
Wire Wire Line
	19150 3850 19900 3850
Wire Wire Line
	19150 3750 19900 3750
Wire Wire Line
	19150 3650 19900 3650
Wire Wire Line
	19150 3550 19900 3550
Wire Wire Line
	19150 3350 19900 3350
Wire Wire Line
	19150 3250 19900 3250
Wire Wire Line
	19150 3150 19900 3150
Wire Wire Line
	19150 3050 19900 3050
Wire Wire Line
	12350 12550 12000 12550
Wire Wire Line
	12000 12750 12350 12750
Wire Wire Line
	12350 12950 12000 12950
Wire Wire Line
	13400 12950 13750 12950
Wire Wire Line
	13750 12750 13400 12750
Wire Wire Line
	13400 12550 13750 12550
Text GLabel 13750 12950 2    60   BiDi
Y26DC
Text GLabel 13750 12750 2    60   BiDi
Y25DC
Text GLabel 13750 12550 2    60   BiDi
Y24DC
Text GLabel 12000 12950 0    60   BiDi
X26DC
Text GLabel 12000 12750 0    60   BiDi
X25DC
Text GLabel 12000 12550 0    60   BiDi
X24DC
Wire Wire Line
	13400 12350 13750 12350
Wire Wire Line
	13750 12250 13400 12250
Wire Wire Line
	13400 12150 13750 12150
Wire Wire Line
	13750 12050 13400 12050
Wire Wire Line
	13400 11950 13750 11950
Wire Wire Line
	13400 11850 13750 11850
Wire Wire Line
	13750 11750 13400 11750
Wire Wire Line
	13400 11650 13750 11650
Wire Wire Line
	13750 11550 13400 11550
Wire Wire Line
	13400 11450 13750 11450
Wire Wire Line
	13750 11350 13400 11350
Wire Wire Line
	13400 11250 13750 11250
Wire Wire Line
	13750 11150 13400 11150
Wire Wire Line
	13400 11050 13750 11050
Wire Wire Line
	13750 10950 13400 10950
Wire Wire Line
	13400 10850 13750 10850
Wire Wire Line
	13750 10750 13400 10750
Wire Wire Line
	13400 10650 13750 10650
Wire Wire Line
	13750 10550 13400 10550
Wire Wire Line
	13400 10450 13750 10450
Wire Wire Line
	13750 10350 13400 10350
Wire Wire Line
	13400 10250 13750 10250
Wire Wire Line
	13750 10150 13400 10150
Wire Wire Line
	13400 10050 13750 10050
Text GLabel 13750 12350 2    60   BiDi
Y23C7
Text GLabel 13750 12250 2    60   BiDi
Y22C6
Text GLabel 13750 12150 2    60   BiDi
Y21C5
Text GLabel 13750 12050 2    60   BiDi
Y20C4
Text GLabel 13750 11950 2    60   BiDi
Y19C3
Text GLabel 13750 11850 2    60   BiDi
Y18C2
Text GLabel 13750 11750 2    60   BiDi
Y17C1
Text GLabel 13750 11650 2    60   BiDi
Y16C0
Text GLabel 13750 11550 2    60   BiDi
Y15B7
Text GLabel 13750 11450 2    60   BiDi
Y14B6
Text GLabel 13750 11350 2    60   BiDi
Y13B5
Text GLabel 13750 11250 2    60   BiDi
Y12B4
Text GLabel 13750 11150 2    60   BiDi
Y11B3
Text GLabel 13750 11050 2    60   BiDi
Y10B2
Text GLabel 13750 10950 2    60   BiDi
Y9B1
Text GLabel 13750 10850 2    60   BiDi
Y8B0
Text GLabel 13750 10750 2    60   BiDi
Y7A7
Text GLabel 13750 10650 2    60   BiDi
Y6A6
Text GLabel 13750 10550 2    60   BiDi
Y5A5
Text GLabel 13750 10450 2    60   BiDi
Y4A4
Text GLabel 13750 10350 2    60   BiDi
Y3A3
Text GLabel 13750 10250 2    60   BiDi
Y2A2
Text GLabel 13750 10150 2    60   BiDi
Y1A1
Text GLabel 13750 10050 2    60   BiDi
Y0A0
Wire Wire Line
	12350 12350 12000 12350
Wire Wire Line
	12000 12250 12350 12250
Wire Wire Line
	12350 12150 12000 12150
Wire Wire Line
	12000 12050 12350 12050
Wire Wire Line
	12350 11950 12000 11950
Wire Wire Line
	12000 11850 12350 11850
Wire Wire Line
	12350 11750 12000 11750
Wire Wire Line
	12000 11650 12350 11650
Text GLabel 12000 12350 0    60   BiDi
X23C7
Text GLabel 12000 12250 0    60   BiDi
X22C6
Text GLabel 12000 12150 0    60   BiDi
X21C5
Text GLabel 12000 12050 0    60   BiDi
X20C4
Text GLabel 12000 11950 0    60   BiDi
X19C3
Text GLabel 12000 11850 0    60   BiDi
X18C2
Text GLabel 12000 11750 0    60   BiDi
X17C1
Text GLabel 12000 11650 0    60   BiDi
X16C0
$Comp
L +12V #PWR?
U 1 1 4422FEA6
P 13750 9950
F 0 "#PWR?" H 13750 9900 20  0001 C C
F 1 "+12V" V 13750 10100 30  0000 C C
	1    13750 9950
	0    1    1    0   
$EndComp
$Comp
L +5V #PWR?
U 1 1 4422FE90
P 13800 13050
F 0 "#PWR?" H 13800 13140 20  0001 C C
F 1 "+5V" V 13800 13200 30  0000 C C
	1    13800 13050
	0    1    1    0   
$EndComp
$Comp
L +3.3V #PWR?
U 1 1 4422FE84
P 13800 12850
F 0 "#PWR?" H 13800 12810 30  0001 C C
F 1 "+3.3V" V 13800 13050 30  0000 C C
	1    13800 12850
	0    1    1    0   
$EndComp
$Comp
L -5V #PWR?
U 1 1 4422FE81
P 13800 12650
F 0 "#PWR?" H 13800 12790 20  0001 C C
F 1 "-5V" V 13800 12800 30  0000 C C
	1    13800 12650
	0    1    1    0   
$EndComp
$Comp
L -12V #PWR?
U 1 1 4422FE7C
P 13800 12450
F 0 "#PWR?" H 13800 12580 20  0001 C C
F 1 "-12V" V 13800 12600 30  0000 C C
	1    13800 12450
	0    1    1    0   
$EndComp
Wire Wire Line
	13400 13050 13800 13050
Wire Wire Line
	13400 12850 13800 12850
Wire Wire Line
	13400 12650 13800 12650
Wire Wire Line
	13400 12450 13800 12450
Wire Wire Line
	13400 9950 13750 9950
Wire Wire Line
	13400 14850 13800 14850
$Comp
L +5V #PWR?
U 1 1 4422FE1E
P 13800 14850
F 0 "#PWR?" H 13800 14940 20  0001 C C
F 1 "+5V" V 13800 15000 30  0000 C C
	1    13800 14850
	0    1    1    0   
$EndComp
$Comp
L +3.3V #PWR?
U 1 1 4422FE19
P 13800 14650
F 0 "#PWR?" H 13800 14610 30  0001 C C
F 1 "+3.3V" V 13800 14850 30  0000 C C
	1    13800 14650
	0    1    1    0   
$EndComp
$Comp
L -5V #PWR?
U 1 1 4422FE16
P 13800 14450
F 0 "#PWR?" H 13800 14590 20  0001 C C
F 1 "-5V" V 13800 14600 30  0000 C C
	1    13800 14450
	0    1    1    0   
$EndComp
$Comp
L -12V #PWR?
U 1 1 4422FE0F
P 13800 14250
F 0 "#PWR?" H 13800 14380 20  0001 C C
F 1 "-12V" V 13800 14400 30  0000 C C
	1    13800 14250
	0    1    1    0   
$EndComp
$Comp
L +12V #PWR?
U 1 1 4422FE02
P 12000 13350
F 0 "#PWR?" H 12000 13300 20  0001 C C
F 1 "+12V" V 12000 13500 30  0000 C C
	1    12000 13350
	0    -1   -1   0   
$EndComp
Wire Wire Line
	13400 14650 13800 14650
Wire Wire Line
	13400 14450 13800 14450
Wire Wire Line
	13400 14250 13800 14250
Wire Wire Line
	12350 13350 12000 13350
Connection ~ 13600 14750
Wire Wire Line
	13400 14750 13600 14750
Connection ~ 13600 14550
Wire Wire Line
	13400 14550 13600 14550
Connection ~ 13600 14350
Wire Wire Line
	13400 14350 13600 14350
Connection ~ 13600 14150
Wire Wire Line
	13600 15100 13600 13350
$Comp
L GND #PWR?
U 1 1 4422FDBC
P 13600 15100
F 0 "#PWR?" H 13600 15100 30  0001 C C
F 1 "GND" H 13600 15030 30  0001 C C
	1    13600 15100
	1    0    0    -1  
$EndComp
Connection ~ 13600 14050
Wire Wire Line
	13400 14050 13600 14050
Connection ~ 13600 13950
Wire Wire Line
	13400 13950 13600 13950
Connection ~ 13600 13850
Wire Wire Line
	13400 13850 13600 13850
Connection ~ 13600 13750
Wire Wire Line
	13400 13750 13600 13750
Connection ~ 13600 13650
Wire Wire Line
	13400 13650 13600 13650
Connection ~ 13600 13550
Wire Wire Line
	13400 13550 13600 13550
Connection ~ 13600 13450
Wire Wire Line
	13400 13450 13600 13450
Wire Wire Line
	13600 14150 13400 14150
Wire Wire Line
	13400 13350 13600 13350
$Comp
L GND #PWR?
U 1 1 4422FD87
P 12150 15100
F 0 "#PWR?" H 12150 15100 30  0001 C C
F 1 "GND" H 12150 15030 30  0001 C C
	1    12150 15100
	1    0    0    -1  
$EndComp
Connection ~ 12150 14850
Wire Wire Line
	12350 14850 12150 14850
Connection ~ 12150 14750
Wire Wire Line
	12350 14750 12150 14750
Connection ~ 12150 14650
Wire Wire Line
	12350 14650 12150 14650
Connection ~ 12150 14550
Wire Wire Line
	12350 14550 12150 14550
Connection ~ 12150 14450
Wire Wire Line
	12350 14450 12150 14450
Connection ~ 12150 14350
Wire Wire Line
	12350 14350 12150 14350
Connection ~ 12150 14250
Wire Wire Line
	12350 14250 12150 14250
Connection ~ 12150 14150
Wire Wire Line
	12350 14150 12150 14150
Connection ~ 12150 14050
Wire Wire Line
	12350 14050 12150 14050
Connection ~ 12150 13950
Wire Wire Line
	12350 13950 12150 13950
Connection ~ 12150 13850
Wire Wire Line
	12350 13850 12150 13850
Connection ~ 12150 13750
Wire Wire Line
	12350 13750 12150 13750
Connection ~ 12150 13650
Wire Wire Line
	12350 13650 12150 13650
Connection ~ 12150 13550
Wire Wire Line
	12350 13550 12150 13550
Wire Wire Line
	12150 13450 12150 15100
Wire Wire Line
	12350 13450 12150 13450
$Comp
L +5V #PWR?
U 1 1 4422FD0C
P 12000 13050
F 0 "#PWR?" H 12000 13140 20  0001 C C
F 1 "+5V" V 12000 13200 30  0000 C C
	1    12000 13050
	0    -1   -1   0   
$EndComp
$Comp
L +3.3V #PWR?
U 1 1 4422FCFE
P 12000 12850
F 0 "#PWR?" H 12000 12810 30  0001 C C
F 1 "+3.3V" V 12000 13050 30  0000 C C
	1    12000 12850
	0    -1   -1   0   
$EndComp
$Comp
L -5V #PWR?
U 1 1 4422FCEF
P 12000 12650
F 0 "#PWR?" H 12000 12790 20  0001 C C
F 1 "-5V" V 12000 12800 30  0000 C C
	1    12000 12650
	0    -1   -1   0   
$EndComp
$Comp
L -12V #PWR?
U 1 1 4422FCDE
P 12000 12450
F 0 "#PWR?" H 12000 12580 20  0001 C C
F 1 "-12V" V 12000 12600 30  0000 C C
	1    12000 12450
	0    -1   -1   0   
$EndComp
Wire Wire Line
	12350 13050 12000 13050
Wire Wire Line
	12350 12850 12000 12850
Wire Wire Line
	12350 12650 12000 12650
Wire Wire Line
	12350 12450 12000 12450
$Comp
L +12V #PWR?
U 1 1 4422FC8A
P 12000 9950
F 0 "#PWR?" H 12000 9900 20  0001 C C
F 1 "+12V" V 12000 10100 30  0000 C C
	1    12000 9950
	0    -1   -1   0   
$EndComp
Wire Wire Line
	12350 9950 12000 9950
Wire Wire Line
	12000 11550 12350 11550
Wire Wire Line
	12350 11450 12000 11450
Wire Wire Line
	12000 11350 12350 11350
Wire Wire Line
	12350 11250 12000 11250
Wire Wire Line
	12000 11150 12350 11150
Wire Wire Line
	12350 11050 12000 11050
Wire Wire Line
	12000 10950 12350 10950
Wire Wire Line
	12350 10850 12000 10850
Wire Wire Line
	12000 10750 12350 10750
Wire Wire Line
	12350 10650 12000 10650
Wire Wire Line
	12000 10550 12350 10550
Text GLabel 12000 11550 0    60   BiDi
X15B7
Text GLabel 12000 11450 0    60   BiDi
X14B6
Text GLabel 12000 11350 0    60   BiDi
X13B5
Text GLabel 12000 11250 0    60   BiDi
X12B4
Text GLabel 12000 11150 0    60   BiDi
X11B3
Text GLabel 12000 11050 0    60   BiDi
X10B2
Text GLabel 12000 10950 0    60   BiDi
X9B1
Text GLabel 12000 10850 0    60   BiDi
X8B0
Text GLabel 12000 10750 0    60   BiDi
X7A7
Text GLabel 12000 10650 0    60   BiDi
X6A6
Text GLabel 12000 10550 0    60   BiDi
X5A5
Text GLabel 12000 10450 0    60   BiDi
X4A4
Text GLabel 12000 10350 0    60   BiDi
X3A3
Text GLabel 12000 10250 0    60   BiDi
X2A2
Text GLabel 12000 10150 0    60   BiDi
X1A1
Text GLabel 12000 10050 0    60   BiDi
X0A0
Wire Wire Line
	12350 10450 12000 10450
Wire Wire Line
	12350 10350 12000 10350
Wire Wire Line
	12350 10250 12000 10250
Wire Wire Line
	12350 10150 12000 10150
Wire Wire Line
	12350 10050 12000 10050
$Comp
L DIN41612_96 J?
U 1 1 4422FAEB
P 12650 9850
F 0 "J?" H 12850 9900 60  0000 C C
F 1 "DIN41612_96" H 12900 4650 60  0000 C C
	1    12650 9850
	1    0    0    -1  
$EndComp
Wire Wire Line
	12250 5350 11600 5350
Wire Wire Line
	12250 5250 11600 5250
Text GLabel 11600 4950 0    60   Input
FX2_IFCLK
Text GLabel 11600 4850 0    60   Input
FX2_CLK
Wire Wire Line
	12250 4950 11600 4950
Wire Wire Line
	12250 4850 11600 4850
Wire Wire Line
	1700 13200 1700 12900
$Comp
L +1.2V #PWR?
U 1 1 4422F420
P 14500 9000
F 0 "#PWR?" H 14500 9140 20  0001 C C
F 1 "+1.2V" V 14500 9200 30  0000 C C
	1    14500 9000
	1    0    0    1   
$EndComp
Connection ~ 14500 8800
Wire Wire Line
	14500 8150 14500 9000
Wire Wire Line
	15700 8800 15700 8150
Wire Wire Line
	14500 8800 15700 8800
$Comp
L +1.2V #PWR?
U 1 1 4422F402
P 14900 1400
F 0 "#PWR?" H 14900 1540 20  0001 C C
F 1 "+1.2V" V 14900 1600 30  0000 C C
	1    14900 1400
	1    0    0    -1  
$EndComp
Connection ~ 14900 1600
Wire Wire Line
	14900 2250 14900 1400
Wire Wire Line
	16100 1600 14900 1600
Wire Wire Line
	16100 2250 16100 1600
$Comp
L VDD #PWR?
U 1 1 4422F372
P 11950 1400
F 0 "#PWR?" H 11950 1500 30  0001 C C
F 1 "VDD" H 11950 1510 30  0000 C C
	1    11950 1400
	1    0    0    -1  
$EndComp
Connection ~ 11950 1750
Wire Wire Line
	11950 8650 11950 1400
Connection ~ 16900 8650
Wire Wire Line
	17600 8650 17600 8150
Wire Wire Line
	11950 8650 17600 8650
Connection ~ 16100 8650
Wire Wire Line
	16900 8650 16900 8150
Connection ~ 15000 8650
Wire Wire Line
	16100 8650 16100 8150
Connection ~ 14100 8650
Wire Wire Line
	15000 8650 15000 8150
Connection ~ 11950 6750
Wire Wire Line
	14100 8650 14100 8150
Connection ~ 11950 5450
Wire Wire Line
	11950 6750 12250 6750
Wire Wire Line
	11950 5450 12250 5450
Connection ~ 11950 3250
Connection ~ 13700 1750
Wire Wire Line
	11950 3250 12250 3250
Wire Wire Line
	19750 1750 11950 1750
Connection ~ 14500 1750
Wire Wire Line
	13700 1750 13700 2250
Connection ~ 15600 1750
Wire Wire Line
	14500 1750 14500 2250
Connection ~ 16700 1750
Wire Wire Line
	15600 1750 15600 2250
Connection ~ 17300 1750
Wire Wire Line
	16700 1750 16700 2250
Connection ~ 19750 3450
Wire Wire Line
	17300 1750 17300 2250
Wire Wire Line
	19750 7350 19750 1750
Text GLabel 11600 4450 0    60   BiDi
FPGA_TDI
Text GLabel 11600 4350 0    60   BiDi
FPGA_TCK
Text GLabel 11600 4250 0    60   BiDi
FPGA_TMS
Text GLabel 11600 4150 0    60   BiDi
FPGA_TDO
Wire Wire Line
	12250 4450 11600 4450
Wire Wire Line
	12250 4350 11600 4350
Wire Wire Line
	12250 4250 11600 4250
Wire Wire Line
	12250 4150 11600 4150
Wire Wire Line
	6500 14200 6500 12950
Text GLabel 5750 14700 0    60   BiDi
FPGA_TDI
Text GLabel 5750 14300 0    60   BiDi
FPGA_TMS
Text GLabel 5750 14100 0    60   BiDi
FPGA_TDO
Text GLabel 5750 13900 0    60   BiDi
FPGA_TCK
Connection ~ 6000 14700
Wire Wire Line
	6750 14700 5750 14700
Connection ~ 6150 14300
Wire Wire Line
	6750 14300 5750 14300
Wire Wire Line
	6750 14100 5750 14100
Connection ~ 6300 13900
Wire Wire Line
	6750 13900 5750 13900
Wire Wire Line
	6000 13750 6000 14700
Wire Wire Line
	6150 13750 6150 14300
Wire Wire Line
	6300 13750 6300 13900
Connection ~ 6300 13150
Wire Wire Line
	6300 13250 6300 13150
Connection ~ 6150 13150
Wire Wire Line
	6150 13250 6150 13150
Connection ~ 6500 13150
Wire Wire Line
	6000 13150 6500 13150
Wire Wire Line
	6000 13250 6000 13150
$Comp
L R R?
U 1 1 4422F104
P 6000 13500
F 0 "R?" V 5900 13400 50  0000 C C
F 1 "1K" V 5900 13550 50  0000 C C
	1    6000 13500
	1    0    0    -1  
$EndComp
$Comp
L R R?
U 1 1 4422F100
P 6150 13500
F 0 "R?" V 6100 13400 50  0000 C C
F 1 "1K" V 6100 13550 50  0000 C C
	1    6150 13500
	1    0    0    -1  
$EndComp
$Comp
L R R?
U 1 1 4422F0F8
P 6300 13500
F 0 "R?" V 6250 13400 50  0000 C C
F 1 "1K" V 6250 13550 50  0000 C C
	1    6300 13500
	1    0    0    -1  
$EndComp
$Comp
L VDD #PWR?
U 1 1 4422F0E6
P 6500 12950
F 0 "#PWR?" H 6500 13050 30  0001 C C
F 1 "VDD" H 6500 13060 30  0000 C C
	1    6500 12950
	1    0    0    -1  
$EndComp
Wire Wire Line
	6750 14200 6500 14200
$Comp
L DGND #PWR?
U 1 1 4422F0CD
P 6600 14950
F 0 "#PWR?" H 6600 14950 40  0001 C C
F 1 "DGND" H 6600 14880 40  0000 C C
	1    6600 14950
	1    0    0    -1  
$EndComp
Connection ~ 6600 14800
Wire Wire Line
	6600 14000 6600 14950
Wire Wire Line
	6600 14800 6750 14800
Wire Wire Line
	6750 14000 6600 14000
$Comp
L ALTERA_JTAG J?
U 1 1 4422F09E
P 7050 13800
F 0 "J?" H 7450 13850 60  0000 C C
F 1 "ALTERA_JTAG" H 7450 12650 60  0000 C C
	1    7050 13800
	1    0    0    -1  
$EndComp
Text GLabel 3000 14000 2    60   BiDi
FX2_PC2
Text GLabel 3000 13900 2    60   BiDi
FX2_PC4
Text GLabel 3000 13800 2    60   BiDi
FX2_PC5
Wire Wire Line
	2350 14000 3000 14000
Wire Wire Line
	2350 13700 2350 14000
Wire Wire Line
	2600 13900 3000 13900
Wire Wire Line
	2600 13700 2600 13900
Wire Wire Line
	2850 13800 3000 13800
Wire Wire Line
	2850 13700 2850 13800
Connection ~ 2600 13050
Wire Wire Line
	2600 13200 2600 13050
Connection ~ 2350 13050
Wire Wire Line
	2350 13200 2350 13050
Connection ~ 1700 13050
Wire Wire Line
	2850 13050 1700 13050
Wire Wire Line
	2850 13200 2850 13050
$Comp
L R R?
U 1 1 4421C5AF
P 2850 13450
F 0 "R?" V 2750 13350 50  0000 C C
F 1 "10K" V 2750 13550 50  0000 C C
	1    2850 13450
	1    0    0    -1  
$EndComp
$Comp
L R R?
U 1 1 4421C5A9
P 2600 13450
F 0 "R?" V 2500 13350 50  0000 C C
F 1 "10K" V 2500 13550 50  0000 C C
	1    2600 13450
	1    0    0    -1  
$EndComp
$Comp
L R R?
U 1 1 4421C5A5
P 2350 13450
F 0 "R?" V 2250 13350 50  0000 C C
F 1 "10K" V 2250 13550 50  0000 C C
	1    2350 13450
	1    0    0    -1  
$EndComp
Wire Wire Line
	19900 6150 19150 6150
Text GLabel 19900 6150 2    60   BiDi
FX2_PC5
Text GLabel 19900 5950 2    60   BiDi
FX2_PC4
Wire Wire Line
	19150 5950 19900 5950
Text GLabel 11600 4650 0    60   BiDi
FX2_PC3
Wire Wire Line
	12250 4650 11600 4650
Text GLabel 11600 5150 0    60   BiDi
FX2_PC2
Wire Wire Line
	12250 5150 11600 5150
Connection ~ 11800 4750
Wire Wire Line
	11800 3450 12250 3450
$Comp
L DGND #PWR?
U 1 1 4421C4C5
P 11800 8850
F 0 "#PWR?" H 11800 8850 40  0001 C C
F 1 "DGND" H 11800 8780 40  0000 C C
	1    11800 8850
	1    0    0    -1  
$EndComp
Connection ~ 11800 7450
Connection ~ 11800 6350
Wire Wire Line
	11800 7450 12250 7450
Connection ~ 11800 5050
Wire Wire Line
	11800 6350 12250 6350
Wire Wire Line
	11800 5050 12250 5050
Wire Wire Line
	12250 4750 11800 4750
Text GLabel 11600 4550 0    60   BiDi
FX2_PC5
Wire Wire Line
	12250 4550 11600 4550
Connection ~ 19750 6050
Wire Wire Line
	19150 7350 19750 7350
Connection ~ 19750 4650
Wire Wire Line
	19750 3450 19150 3450
Connection ~ 19650 4250
Wire Wire Line
	19650 2950 19650 8650
Wire Wire Line
	19150 2950 19650 2950
Connection ~ 19650 5750
Wire Wire Line
	19650 4250 19150 4250
Connection ~ 19650 7150
Wire Wire Line
	19150 7150 19650 7150
$Comp
L DGND #PWR?
U 1 1 4421C428
P 19650 8650
F 0 "#PWR?" H 19650 8650 40  0001 C C
F 1 "DGND" H 19650 8580 40  0000 C C
	1    19650 8650
	1    0    0    -1  
$EndComp
Connection ~ 19650 5850
Wire Wire Line
	19650 5850 19150 5850
Connection ~ 19750 5650
Wire Wire Line
	19750 4650 19150 4650
Wire Wire Line
	19750 6050 19150 6050
Wire Wire Line
	19150 5650 19750 5650
Wire Wire Line
	19150 5750 19650 5750
Text GLabel 5900 2300 3    60   Input
FX2_FD15
Text GLabel 6000 2300 3    60   BiDi
FX2_BD14
Text GLabel 6100 2300 3    60   BiDi
FX2_FD13
Text GLabel 6200 2300 3    60   BiDi
FX2_FD12
Text GLabel 6800 2300 3    60   BiDi
FX2_PE7
Text GLabel 6900 2300 3    60   BiDi
FX2_PE6
Text GLabel 7000 2300 3    60   BiDi
FX2_PE5/INT6
Text GLabel 7100 2300 3    60   BiDi
FX2_PE4
Text GLabel 7200 2300 3    60   BiDi
FX2_PE3
Text GLabel 7300 2300 3    60   BiDi
FX2_PE2
Text GLabel 7400 2300 3    60   BiDi
FX2_PE1
Text GLabel 7500 2300 3    60   BiDi
FX2_PE0
Text GLabel 7700 2300 3    60   Input
FX2_INT5
Text GLabel 7800 2300 3    60   BiDi
FX2_FD11
Text GLabel 7900 2300 3    60   BiDi
FX2_FD10
Text GLabel 8000 2300 3    60   BiDi
FX2_FD9
Wire Wire Line
	5900 2950 5900 2300
Wire Wire Line
	6000 2950 6000 2300
Wire Wire Line
	6100 2950 6100 2300
Wire Wire Line
	6200 2950 6200 2300
Wire Wire Line
	6800 2950 6800 2300
Wire Wire Line
	6900 2950 6900 2300
Wire Wire Line
	7000 2950 7000 2300
Wire Wire Line
	7100 2950 7100 2300
Wire Wire Line
	7200 2950 7200 2300
Wire Wire Line
	7300 2950 7300 2300
Wire Wire Line
	7400 2950 7400 2300
Wire Wire Line
	7500 2950 7500 2300
Wire Wire Line
	7700 2950 7700 2300
Wire Wire Line
	7800 2950 7800 2300
Wire Wire Line
	7900 2950 7900 2300
Wire Wire Line
	8000 2950 8000 2300
Text GLabel 9500 3750 2    60   BiDi
FX2_CTL5
Text GLabel 9500 3350 2    60   BiDi
FX2_FD8
Text GLabel 9500 4350 2    60   BiDi
FX2_PA7/FLAGD/SLCS
Text GLabel 9500 4450 2    60   BiDi
FX2_PA6/PKTEND
Text GLabel 9500 4550 2    60   BiDi
FX2_PA5/FIFOADR1
Text GLabel 9500 4650 2    60   BiDi
FX2_PA4/FIFOADR0
Wire Wire Line
	8800 3350 9500 3350
Wire Wire Line
	8800 3750 9500 3750
Wire Wire Line
	8800 4350 9500 4350
Wire Wire Line
	8800 4450 9500 4450
Wire Wire Line
	8800 4550 9500 4550
Wire Wire Line
	8800 4650 9500 4650
Text GLabel 9500 5050 2    60   BiDi
FX2_PA3
Text GLabel 9500 5150 2    60   BiDi
FX2_PA2/SLOE
Text GLabel 9500 5250 2    60   BiDi
FX2_PA1
Text GLabel 9500 5350 2    60   BiDi
FX2_PA0
Wire Wire Line
	8800 5050 9500 5050
Wire Wire Line
	8800 5150 9500 5150
Wire Wire Line
	8800 5250 9500 5250
Wire Wire Line
	8800 5350 9500 5350
Text GLabel 9500 5650 2    60   BiDi
FX2_PC7
Text GLabel 9500 5750 2    60   BiDi
FX2_PC6
Text GLabel 9500 5850 2    60   BiDi
FX2_PC5
Text GLabel 9500 5950 2    60   BiDi
FX2_PC4
Text GLabel 9500 6050 2    60   BiDi
FX2_PC3
Text GLabel 9500 6150 2    60   BiDi
FX2_PC2
Text GLabel 9500 6250 2    60   BiDi
FX2_PC1
Text GLabel 9500 6350 2    60   BiDi
FX2_PC0
Text GLabel 9500 6450 2    60   BiDi
FX2_CTL2/FLAGC
Text GLabel 9500 6550 2    60   BiDi
FX2_CTL1/FLAGB
Text GLabel 9500 6650 2    60   BiDi
FX2_CTL0/FLAGA
Text GLabel 9500 6850 2    60   BiDi
FX2_CTL4
Text GLabel 9500 6950 2    60   BiDi
FX2_CTL3
Wire Wire Line
	8800 5650 9500 5650
Wire Wire Line
	8800 5750 9500 5750
Wire Wire Line
	8800 5850 9500 5850
Wire Wire Line
	8800 5950 9500 5950
Wire Wire Line
	8800 6050 9500 6050
Wire Wire Line
	8800 6150 9500 6150
Wire Wire Line
	8800 6250 9500 6250
Wire Wire Line
	8800 6350 9500 6350
Wire Wire Line
	8800 6450 9500 6450
Wire Wire Line
	8800 6550 9500 6550
Wire Wire Line
	8800 6650 9500 6650
Wire Wire Line
	8800 6850 9500 6850
Wire Wire Line
	8800 6950 9500 6950
Text GLabel 7300 8100 1    60   BiDi
FX2_FD7
Text GLabel 7200 8100 1    60   BiDi
FX2_FD6
Text GLabel 7100 8100 1    60   BiDi
FX2_FD5
Text GLabel 7000 8100 1    60   BiDi
FX2_FD4
Text GLabel 6900 8100 1    60   Input
FX2_RXD1
Text GLabel 6800 8100 1    60   Output
FX2_TXD1
Text GLabel 6700 8100 1    60   Input
FX2_RXD0
Text GLabel 6600 8100 1    60   Output
FX2_TXD0
Text GLabel 6300 8100 1    60   BiDi
FX2_FD3
Text GLabel 6200 8100 1    60   BiDi
FX2_FD2
Text GLabel 6100 8100 1    60   BiDi
FX2_FD1
Text GLabel 6000 8100 1    60   BiDi
FX2_FD0
Wire Wire Line
	7300 7450 7300 8100
Wire Wire Line
	7200 7450 7200 8100
Wire Wire Line
	7100 7450 7100 8100
Wire Wire Line
	7000 7450 7000 8100
Wire Wire Line
	6900 7450 6900 8100
Wire Wire Line
	6800 7450 6800 8100
Wire Wire Line
	6700 7450 6700 8100
Wire Wire Line
	6600 7450 6600 8100
Wire Wire Line
	6300 7450 6300 8100
Wire Wire Line
	6200 7450 6200 8100
Wire Wire Line
	6100 7450 6100 8100
Wire Wire Line
	6000 7450 6000 8100
Text GLabel 3950 6350 0    60   Output
T2
Text GLabel 3950 6250 0    60   Output
T1
Text GLabel 3950 6150 0    60   Output
T0
Wire Wire Line
	4700 6350 3950 6350
Wire Wire Line
	4700 6250 3950 6250
Wire Wire Line
	4700 6150 3950 6150
Wire Wire Line
	3950 6450 4700 6450
Wire Wire Line
	3950 6050 4700 6050
Text GLabel 3950 6450 0    60   Output
FX2_IFCLK
Text GLabel 3950 6050 0    60   Output
FX2_INT4
Wire Wire Line
	4700 4150 3500 4150
Wire Wire Line
	3500 4050 4700 4050
Wire Wire Line
	4700 3950 3500 3950
Wire Wire Line
	3500 3850 4700 3850
Wire Wire Line
	4700 3750 3500 3750
Wire Wire Line
	3500 3650 4700 3650
Wire Wire Line
	4700 3350 3500 3350
Wire Wire Line
	4700 5850 3800 5850
Connection ~ 3600 4250
Wire Wire Line
	4700 4250 3600 4250
Wire Wire Line
	4700 4950 3600 4950
Connection ~ 3800 3450
Wire Wire Line
	4700 3450 3800 3450
Connection ~ 3800 2400
Wire Wire Line
	9400 2400 3800 2400
Wire Wire Line
	2000 6950 4700 6950
Wire Wire Line
	4700 6850 2000 6850
Wire Wire Line
	2600 5150 4700 5150
Wire Wire Line
	4700 5050 2700 5050
Wire Wire Line
	1900 4450 4700 4450
Wire Wire Line
	4700 4350 900  4350
Text GLabel 3500 4150 0    60   Input
RDY5
Text GLabel 3500 4050 0    60   Input
RDY4
Text GLabel 3500 3950 0    60   Input
RDY3
Text GLabel 3500 3850 0    60   Input
RDY2
Text GLabel 3500 3750 0    60   Input
RDY1/SLWR
Text GLabel 3500 3650 0    60   Input
RDY0/SLRD
Text GLabel 3500 3350 0    60   Output
FX2_CLK
$Comp
L VDD #PWR?
U 1 1 4421AB5B
P 700 6350
F 0 "#PWR?" H 700 6450 30  0001 C C
F 1 "VDD" H 700 6460 30  0000 C C
	1    700  6350
	1    0    0    -1  
$EndComp
Connection ~ 700  6450
Wire Wire Line
	700  6650 700  6350
Wire Wire Line
	2150 6650 2000 6650
Wire Wire Line
	2150 6450 2150 6650
Wire Wire Line
	700  6450 2150 6450
Wire Wire Line
	850  6650 700  6650
$Comp
L DGND #PWR?
U 1 1 4421AB0B
P 700 7100
F 0 "#PWR?" H 700 7100 40  0001 C C
F 1 "DGND" H 700 7030 40  0000 C C
	1    700  7100
	1    0    0    -1  
$EndComp
Connection ~ 700  6950
Wire Wire Line
	700  6750 700  7100
Connection ~ 700  6850
Wire Wire Line
	850  6850 700  6850
Wire Wire Line
	700  6950 850  6950
Wire Wire Line
	850  6750 700  6750
Connection ~ 2900 6150
Wire Wire Line
	2900 6250 2900 6050
$Comp
L VDD #PWR?
U 1 1 4421AA2F
P 2900 6050
F 0 "#PWR?" H 2900 6150 30  0001 C C
F 1 "VDD" H 2900 6160 30  0000 C C
	1    2900 6050
	1    0    0    -1  
$EndComp
Wire Wire Line
	3100 6150 3100 6250
Wire Wire Line
	2900 6150 3100 6150
Connection ~ 3100 6950
Wire Wire Line
	3100 6750 3100 6950
Connection ~ 2900 6850
Wire Wire Line
	2900 6750 2900 6850
$Comp
L R R?
U 1 1 4421AA1C
P 3100 6500
F 0 "R?" V 3200 6400 50  0000 C C
F 1 "2.2K" V 3200 6600 50  0000 C C
	1    3100 6500
	1    0    0    -1  
$EndComp
$Comp
L R R?
U 1 1 4421AA15
P 2900 6500
F 0 "R?" V 2800 6400 50  0000 C C
F 1 "2.2K" V 2800 6600 50  0000 C C
	1    2900 6500
	1    0    0    -1  
$EndComp
$Comp
L 24LC128 U?
U 1 1 4421A9F3
P 1150 7050
F 0 "U?" H 1200 7600 60  0000 C C
F 1 "24LC128" H 1400 7000 60  0000 C C
	1    1150 7050
	1    0    0    -1  
$EndComp
$Comp
L DGND #PWR?
U 1 1 4421A83B
P 2600 6350
F 0 "#PWR?" H 2600 6350 40  0001 C C
F 1 "DGND" H 2600 6280 40  0000 C C
	1    2600 6350
	1    0    0    -1  
$EndComp
Wire Wire Line
	2600 6200 2600 6350
Wire Wire Line
	2450 6200 2600 6200
Wire Wire Line
	2700 6100 2450 6100
Wire Wire Line
	2700 5050 2700 6100
Wire Wire Line
	2600 6000 2600 5150
Wire Wire Line
	2450 6000 2600 6000
$Comp
L USB C?
U 1 1 4421A80D
P 1900 6250
F 0 "C?" H 1950 6700 60  0000 C C
F 1 "USB" H 2000 6200 60  0000 C C
	1    1900 6250
	1    0    0    -1  
$EndComp
NoConn ~ 8800 4750
NoConn ~ 8800 4850
NoConn ~ 8800 4950
NoConn ~ 7900 7450
NoConn ~ 7800 7450
NoConn ~ 7700 7450
NoConn ~ 7600 7450
NoConn ~ 7500 7450
NoConn ~ 5800 7450
NoConn ~ 5500 7450
$Comp
L VAA #PWR?
U 1 1 4421A62B
P 3600 2250
F 0 "#PWR?" H 3600 2450 40  0001 C C
F 1 "VAA" H 3600 2400 40  0000 C C
	1    3600 2250
	1    0    0    -1  
$EndComp
Wire Wire Line
	3600 2250 3600 4950
$Comp
L VDD #PWR?
U 1 1 4421A5DF
P 3800 2250
F 0 "#PWR?" H 3800 2350 30  0001 C C
F 1 "VDD" H 3800 2360 30  0000 C C
	1    3800 2250
	1    0    0    -1  
$EndComp
Connection ~ 6400 7950
Wire Wire Line
	5900 7950 9400 7950
Wire Wire Line
	5900 7450 5900 7950
Connection ~ 8000 7950
Wire Wire Line
	6400 7450 6400 7950
Connection ~ 9400 6750
Wire Wire Line
	9400 7950 9400 2400
Wire Wire Line
	8000 7450 8000 7950
Connection ~ 9400 5450
Wire Wire Line
	9400 6750 8800 6750
Connection ~ 9400 3550
Wire Wire Line
	9400 5450 8800 5450
Connection ~ 7600 2400
Wire Wire Line
	9400 3550 8800 3550
Wire Wire Line
	7600 2950 7600 2400
Wire Wire Line
	3800 5850 3800 2250
NoConn ~ 8800 4150
NoConn ~ 8800 4050
NoConn ~ 8800 3950
NoConn ~ 8800 3850
NoConn ~ 6600 2950
NoConn ~ 6500 2950
NoConn ~ 6400 2950
NoConn ~ 6300 2950
NoConn ~ 5700 2950
NoConn ~ 5600 2950
NoConn ~ 5500 2950
NoConn ~ 4700 5750
NoConn ~ 4700 5650
NoConn ~ 4700 5550
NoConn ~ 4700 5450
NoConn ~ 4700 5350
NoConn ~ 4700 7050
NoConn ~ 4700 6750
Connection ~ 4100 5250
Wire Wire Line
	4700 5250 4100 5250
Wire Wire Line
	4100 4550 4700 4550
Wire Wire Line
	4100 8150 4100 4550
$Comp
L AGND #PWR?
U 1 1 4421A4E8
P 4100 8150
F 0 "#PWR?" H 4100 8150 40  0001 C C
F 1 "AGND" H 4100 8080 50  0000 C C
	1    4100 8150
	1    0    0    -1  
$EndComp
$Comp
L DGND #PWR?
U 1 1 4421A48D
P 1900 5450
F 0 "#PWR?" H 1900 5450 40  0001 C C
F 1 "DGND" H 1900 5380 40  0000 C C
	1    1900 5450
	1    0    0    -1  
$EndComp
$Comp
L DGND #PWR?
U 1 1 4421A489
P 900 5450
F 0 "#PWR?" H 900 5450 40  0001 C C
F 1 "DGND" H 900 5380 40  0000 C C
	1    900  5450
	1    0    0    -1  
$EndComp
Connection ~ 1900 4850
Connection ~ 900  4850
$Comp
L C C?
U 1 1 4421A477
P 1900 5150
F 0 "C?" H 1950 5250 50  0000 L C
F 1 "12p" H 1950 5050 50  0000 L C
	1    1900 5150
	1    0    0    -1  
$EndComp
$Comp
L C C?
U 1 1 4421A473
P 900 5150
F 0 "C?" H 950 5250 50  0000 L C
F 1 "12p" H 950 5050 50  0000 L C
	1    900  5150
	1    0    0    -1  
$EndComp
$Comp
L CRYSTAL X?
U 1 1 4421A3E2
P 1400 4850
F 0 "X?" H 1400 5000 60  0000 C C
F 1 "24 MHz" H 1400 4700 60  0000 C C
	1    1400 4850
	1    0    0    -1  
$EndComp
Connection ~ 5800 2600
Wire Wire Line
	6700 2600 4350 2600
Wire Wire Line
	6700 2950 6700 2600
Connection ~ 4350 3550
Wire Wire Line
	5800 2600 5800 2950
Wire Wire Line
	4350 7950 4350 2600
Connection ~ 4350 5950
Wire Wire Line
	4700 3550 4350 3550
Connection ~ 4350 6550
Wire Wire Line
	4700 5950 4350 5950
Connection ~ 9250 5550
Wire Wire Line
	9250 4250 9250 7800
Wire Wire Line
	8800 4250 9250 4250
Connection ~ 9250 7050
Wire Wire Line
	8800 5550 9250 5550
Connection ~ 7400 7800
Wire Wire Line
	9250 7050 8800 7050
Wire Wire Line
	4350 7800 9250 7800
Connection ~ 6500 7800
Wire Wire Line
	7400 7800 7400 7450
Connection ~ 4350 7800
Wire Wire Line
	6500 7450 6500 7800
NoConn ~ 4700 4850
NoConn ~ 4700 4750
NoConn ~ 4700 4650
$Comp
L DGND #PWR?
U 1 1 4421A2FE
P 4350 7950
F 0 "#PWR?" H 4350 7950 40  0001 C C
F 1 "DGND" H 4350 7880 40  0000 C C
	1    4350 7950
	1    0    0    -1  
$EndComp
Wire Wire Line
	4700 6550 4350 6550
NoConn ~ 5600 7450
NoConn ~ 5700 7450
NoConn ~ 4700 6650
NoConn ~ 8800 3450
Wire Wire Line
	9650 3650 8800 3650
Text GLabel 9650 3650 2    60   Input
/RESET
Connection ~ 1700 13800
Wire Wire Line
	1550 13800 1700 13800
Text GLabel 1550 13800 0    60   Input
/RESET
$Comp
L DGND #PWR?
U 1 1 4421A1DE
P 1700 14400
F 0 "#PWR?" H 1700 14400 40  0001 C C
F 1 "DGND" H 1700 14330 40  0000 C C
	1    1700 14400
	1    0    0    -1  
$EndComp
$Comp
L VDD #PWR?
U 1 1 4421A1CB
P 1700 12900
F 0 "#PWR?" H 1700 13000 30  0001 C C
F 1 "VDD" H 1700 13010 30  0000 C C
	1    1700 12900
	1    0    0    -1  
$EndComp
$Comp
L C C?
U 1 1 4421A164
P 1700 14100
F 0 "C?" H 1750 14200 50  0000 L C
F 1 "0.1u" H 1750 14000 50  0000 L C
	1    1700 14100
	1    0    0    -1  
$EndComp
$Comp
L R R?
U 1 1 4421A152
P 1700 13450
F 0 "R?" V 1600 13350 50  0000 C C
F 1 "100K" V 1600 13550 50  0000 C C
	1    1700 13450
	1    0    0    -1  
$EndComp
$Comp
L CY7C68013A_128 U?
U 1 1 4420647D
P 4700 3350
F 0 "U?" H 6750 1550 60  0000 C C
F 1 "CY7C68013A_128" H 6750 1400 60  0000 C C
	1    4700 3350
	1    0    0    -1  
$EndComp
$Comp
L EP2C5-208 U?
U 1 1 44206463
P 12550 2550
F 0 "U?" H 15650 150 60  0000 C C
F 1 "EP2C5-208" H 15650 50  60  0000 C C
	1    12550 2550
	1    0    0    -1  
$EndComp
$EndSCHEMATC
