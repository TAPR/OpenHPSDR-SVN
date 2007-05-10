Working_Directory directory: this is a working directory from which to run the Janus and Ozy command-line batch file tests

Batch files:
<ld1.bat> contains the one line: "load_firmware 0x04b4 0x8613 OZYV1FW.hex" and loads a test program into a "virgin" FX2 with a USB VID/PID of 04b4/8613
<ld2.bat> contains the one line: "load_firmware 0xfffe 0x0007 ozyfw-sdr1k.hex" and loads a different test program into an FX2 at the Ozy standard USB VID/PID of fffe/0007
<pg1.bat> contains the one line: "program_OZYEEPROM 0xfffe 0x0007" and uses a previously loaded test program to program the VID/PID into the Ozy EEPROM
<pg2.bat> contains the one line: "upload_FPGA 0xfffe 0x0007 Ozy_Janus.rbf" and uses a previously loaded test program to load the Ozy FPGA with operating code
<jtest.bat> contains the one line: JanusTester  | grep  -F -f jtest-exclude.nocrlf -v and is used to filter the massive output from JanusTester. Requires CYGWIN.
<init.bat> contains the following lines. It initializes the CODEC via I2C over the ther ATLAS bus
	write_i2c 0xfffe 0x7 0x1a 0x1e 0x00 0x00 2
	write_i2c 0xfffe 0x7 0x1a 0x12 0x01 0x00 2
	write_i2c 0xfffe 0x7 0x1a 0x08 0x14 0x00 2
	rem change 0x15 to 0x14 to drop mic input gain 20dB
	write_i2c 0xfffe 0x7 0x1a 0x0c 0x00 0x00 2
	write_i2c 0xfffe 0x7 0x1a 0x0e 0x02 0x00 2
	write_i2c 0xfffe 0x7 0x1a 0x10 0x00 0x00 2
	write_i2c 0xfffe 0x7 0x1a 0x0a 0x00 0x00 2

Executable files:
<jt.exe> and <JanusTester.exe> are identical executable versions of the Janus analog loopback program written by Bill, KD5TFD (try typing "JanusTester" 500 times!)
<load_firmware.exe> loads FX2 firmware vis the USB interface
<program_OZYEEPROM.exe> programs a new value of USB VID/PID into the Ozy EEPROM
<upload_FPGA.exe> loads an RBF file (FPGA program binary image) into the Ozy FPGA
<write_I2C.exe> is used by the init.bat script to initialize the Janus CODEC over the ATLAS bus

FX2 program images:
<OZYV1FW.hex> is an FX2 image that provides VID/PID EEPROM programming and blinks Ozy LED D9
<ozyfw-sdr1k.hex> is the operating FX2 image that provides VID/PID EEPROM programming, truns on LEDs D5-D8 and alternately blinks Ozy LEDs D9 and D10

FPGA images:
<Ozy_Janus.rbf> is an FPGA image of the production code
<WrapTest.rbf> is an FPGA image of the loopback test code (note: this is an older version and NOT the one that is loaded into the FPGA PROM)

Other files:
<jtest-exclude.list> is a list of JanusTester output lines that grep can filter out. Requires CYGWIN.
<jtest-exclude.nocrlf> is a UNIX-formatted version of jtest-exclude.list

Required DLLs:
<KD5TFD-VK6APH-Audio.dll>
<HPSDR_USB_LIB_V1.dll>
<pthreadVC.dll>
<nanotimer.dll>
<libfftw3f-3.dll>
<DttSP.dll>



