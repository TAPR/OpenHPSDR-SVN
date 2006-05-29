@echo off
rem initialize i2c device on the fpga board
rem wolfson init
rem     - 34 00 4B - I2S, master, 96 khz
rem     - 34 02 02 - 24 bit samples
rem TLV320 init
rem     - 36 1E 00 - Reset chip
rem     - 36 12 01 - set digital interface active
rem     - 36 08 15 - D/A on, mic input, mic 20dB boost
rem     - 36 0C 00 - All chip power on
rem     - 36 0E 01 - Slave, 16 bit, Left Justified
rem     - 36 10 5C - Clock/2, 96k, Normal mode
rem     - 36 0A 00 - turn D/A mute off

i2c-cmd 34 00 4b
i2c-cmd 34 02 02
i2c-cmd 36 1e 00
i2c-cmd 36 12 01
i2c-cmd 36 08 15
i2c-cmd 36 0c 00
i2c-cmd 36 0e 01
i2c-cmd 36 10 5c
i2c-cmd 36 0a 00
