usb_jtag using Cypress FX2 chip
===============================

The code in this directory is for Cypress FX2 (e.g. CY7C68013A) and can be compiled with
the SDCC compiler (I tried version 2.6 as shipped with Ubuntu 6.10). Once I had a version
that could be compiled with Keil tools (until early 2007), but switched to SDCC because I 
usually develop on a Linux host.

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

Other assignments are possible; you'll have to adapt the definitions in hardware.h and maybe
in hardware.c, too.

The USB identification data (vendor/product ID, strings, ...) can be modified in dscr.a51. My
firmware emulates the 128 byte EEPROM that usually holds configuration data for the FT245 and
which can be read from the host; its content is computed from the data in dscr.a51 as well.

The WAKEUP pin should be high for the re-numeration to work reliably (thanks Jean/fpga4fun!)

From this release on, there is new code to support running in the "Xilinx
Platform Cable USB". If you select HARDWARE=hw_xpcu_i or hw_xpcu_x at the top
of the Makefile, a firmware for the XPCU will be built. I've tested this only
with unmodified CPLD version 18 (0x12) on a Spartan-3E starter kit, as it was
programmed by my WebPack 8.2i. The code needs optimization; yet it is merely a
proof of concept.

 hw_xpcu_i: Access "internal" chain (the XPCU CPLD, IC3, itself)
 hw_xpcu_x: Access "external" chain (the Spartan 3E, PROM, etc.)

Changes since previous release on 2007-01-28:
  - Initial suppport for running on Xilinx XPCU.
  - New FX2 code, based on USRP2 from the GNU Radio Project;
  - Firmware can now be compiled using SDCC 2.6. No more Keil support.
  - EEPROM content is automatically computed from descriptors, including checksum.

Changes since initial release on 2006-04-23:
  - added this readme.txt
  - reorganized my project folder: diff is now created from Subversion repository
  - stripped *.dist extension from eeprom.c and dscr.a51 
  - added unique proper product and vendor ID (thanks to Antti Lukats!)
  - fixed checksum in eeprom.c
  - added comments about AS/PS mode pins in usbjtag.c




