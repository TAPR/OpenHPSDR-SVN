Phil Harman - VK6APH - 13 December 2008


The files in this directory enable an  HPSDR board connected to the Atlas bus to 
have its CPLD/ Flash chip  programmed. Run the relevant batch file to program
the required CPLD or Flash chip.

VERY IMPORTANT:  The board to be programed MUST be next to an Ozy board
and closest to the powerconnector on the bus.The "Last JTAG" jumper
must be in place on the last board i.e. that closest to the power connector.
Only ONE Last JTAG jumper must be in place.



Here are the steps to program the Mercury EPCS16 via FX2/USBblaste under Quartus II.

- Program the FX2 with USBblaster code as previously i.e. run usbblaster.bat.
- Use the FX2/USBblaster to program the Mercury FPGA with Serial_Flash.sof.
- Convert your Mercury.sof file to a Mercury.jic file.  See Altera AN370 page 11
  as to how to do this.   The FPGA is an EP3C25 and the flash an EPCS16.
- Use the FX2/USBblaster to program Mercury with Mercury.jic
- Power cycle the Ozy and Mercury boards and Mercury will load from its flash memory.

I've attached the source for Serial_Flash.v. It's just the SFL Megafunction with this line in it

SFL SFL_code(.noe_in(1'b0));

Once you are able to correctly load the flash chip then this could be included in your Mercury code since it is tiny.



Here are the steps to program the Mercury EPCS16 using a batch file

- Program the FX2 with USBblaster code as previously i.e. run usbblaster.bat.
- Convert your Mercury.sof file to a Mercury.jic file.  See Altera AN370 page 11
  as to how to do this.   The FPGA is an EP3C25 and the flash an EPCS16.
- Convert the Mercury.jic file into a Mercury.jam file. See Altera AN370 page 19.
- Run the following from the comand line; 
  quartus_jli -a configure Mercury.jam -l 
- Cycle power to Ozy and Mercury boards and Mercury will load from its flash memory.

OR

- The above can be run from Program-Mercury-EPCS16.bat in this directory if desired. 

NOTE: YOu need to use quartus_jli from Quartus II V8.1 since this supports the Cyclone III devices.
  
