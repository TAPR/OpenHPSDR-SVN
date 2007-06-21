usb_jtag using Cypress FX2 chip
===============================

The code in this directory is for Cypress FX2 (e.g. CY7C68013A) and can be compiled with
the Keil compiler. When I developed this, it was possible to compile it with the evaluation
version of the compiler (the resulting code is less than 4 kByte).

No logic beside the FX2 itself and only a few external components are required for a basic
JTAG adapter. I don't have detailed schematics available; my test setup consists of a FX2
on a custom board where it's directly connected to a Cyclone FPGA.

        ____________
       |            |
       | Cypress    |
  USB__| EZ-USB FX2 |__JTAG(TDI,TDO,TCK,TMS)
       | CY7C68013A |
       |____________|
        __|__________
       |             |
       | 24 MHz XTAL |
       |_____________|


Similar eval boards are now available from fpga4fun.com - the boards named
"Saxo-L" and "Xylo-EM" are pre-wired for use with an adapted version of my
code, while "Saxo" and "Xylo" can be used after soldering 4 extra wires:

	http://www.fpga4fun.com/board_Xylo.html


As is, the code assumes the following pin assignment:

 Port C.0: TDI
 Port C.1: TDO
 Port C.2: TCK
 Port C.3: TMS

Other assignments are possible; you'll have to adapt the code in usbjtag.c and shift.a51.

The WAKEUP pin should be high for the re-numeration to work reliably (thanks Jean/fpga4fun!)

NB, the "Xilinx Platform Cable USB" is based on a FX2 chip. In theory, it is possible to
write code for that adapter to make it compatible with my adapter drivers on the fly
(and reversible). In practice however, it's complicated because Xilinx attaches
an additional CPLD to the FX2 for high-speed serial/parallel conversion and you
would have to find out how to communicate with that CPLD first. Not impossible,
but I'm not finished with it yet...

Changes since initial release on 2006-04-23:
  - added this readme.txt
  - reorganized my project folder: diff is now created from Subversion repository
  - stripped *.dist extension from eeprom.c and dscr.a51 
  - added unique proper product and vendor ID (thanks to Antti Lukats!)
  - fixed checksum in eeprom.c
  - added comments about AS/PS mode pins in usbjtag.c




