Phil Harman - VK6APH - 13 December 2008


The files in this directory enable an  HPSDR board connected to the Atlas bus to 
have its CPLD/ Flash chip  programmed. Run the relevant batch file to program
the required CPLD or Flash chip.

VERY IMPORTANT:  The board to be programed MUST be next to an Ozy board
and closest to the powerconnector on the bus.The "Last JTAG" jumper
must be in place on the last board i.e. that closest to the power connector.
Only ONE Last JTAG jumper must be in place.



Here are the steps to program the Mercury EPCS16 via FX2/USBblaster under Quartus II.


- Program the FX2 with USBblaster code as previously i.e. run usbblaster.bat.
- Use the FX2/USBblaster to program the Mercury FPGA with Serial_Flash.sof.

OR

- Convert your Mercury.sof file to a Mercury.jic file.  See Altera AN370 page 11
  as to how to do this.   The FPGA is an EP3C25 and the flash an EPCS16.
  Set up Quartus to program the flash chip using a *.jic file. Make sure you set the 
  programming options.
  Save the *.cdf file to the working directory then;
- use quartus_pgm *.cdf to program the flash chip (can take a minute to do this)
- Cycle power to Ozy and Mercury boards and Mercury will load from its flash memory.



Here are the steps to program the Mercury EPCS16 using a batch file

load_firmwareV1.1.exe  0xfffe  0x7 ozyfw-sdr1k.hex
sleep 1000
upload_fpgaV1.1.exe 0xfffe 0x7 usb_blaster.rbf
sleep 1000
load_firmwareV1.1.exe  0xfffe  0x7 std.hex

// this has loaded the FX2 to make it look like a USBblaser and 
// linked the FPGA pins to the flash memeory

then 

sleep 3000
quartus_pgm mercury.cdf

Which will program the flash 


OR

- The above can be run from Program-Mercury-EPCS16.bat in this directory if desired. 

NOTE: YOu need to use quartus_pgm from Quartus II V8.1 since this supports the Cyclone III devices.
  
