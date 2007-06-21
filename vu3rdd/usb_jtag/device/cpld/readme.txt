usb_jtag using FT245BM + CPLD
=============================

The jtag_logic.vhd in this directory describes the logic for a parallel-serial converter 
to be connected to a FT245BM USB chip from FTDI Inc. The newer FT245R, which has the EEPROM
AT93C46 for Vendor/Product ID and 6 MHz clock source integrated, could be used as well. 
Additionally, the FT245R has an option to output 12 MHz from its internal oscillator; that
could be used to clock the CPLD (though it would run at half the speed then).

        _________
       |         |
       | AT93C46 |
       |_________|
        __|__________    _________
       |             |  |         |
  USB__| FTDI 245BM  |__| EPM7064 |__JTAG (B_TDO,B_TDI,B_TMS,B_TCK)
       |_____________|  |_________|
        __|__________    _|___________
       |             |  |             |
       | 6 MHz XTAL  |  | 24 MHz Osc. |
       |_____________|  |_____________|


The Quartus assignments file epm7064.qsf here lists a possible pin assignment,
valid for the PLCC-44 variant of EPM7064. For testing, I just wired a FT245
module (as it is available directly from FTDI and several distributors)
directly to an EPM7064, and added only a few external resistors in series with
the JTAG I/O pins. I didn't draw detailed schematics yet because the
connections can be completely taken from FTDI datasheets. 

To match the logic voltage levels of my specific target, I chose an
EPM7064SLC44-10 with 5V I/O. There are others that use 3.3V etc.  For more
flexibility, you should add level shifter and some protection circuit on the
JTAG I/O. 

Please note that this CPLD code is NOT the same as in an Altera USB-Blaster,
it just behaves very similar. It's very probable that logic of real USB-Blaster
performs better than this design.

Changes since initial release on 2006-04-23:
  - added this readme.txt
  - reorganized my project folder: diff is now created from Subversion repository
  - explicit state machine encoding to avoid illegal states at powerup
  - added AS/PS mode

Thanks to the readers at edaboard.com for their interest, testing and comments!



