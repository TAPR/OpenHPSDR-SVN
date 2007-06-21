This directory is a placeholder for a project which will enable 
accessing JTAG ports of various CPLDs in the HPSDR system. To
begin with, I am taking the code from this website:

 http://www.ixo.de/info/usb_jtag/

The usb_jtag directory is an unzipped version of the file:

 usb_jtag-20070215-1134.zip

from the above URL.

The idea is to write FPGA firmware on Ozy to connect FX2 to
the JTAG pins routed thru the ATLAS backplane. This way,
altera quartus can "see" the CPLDs directly. FX2 with the
required firmware together with the code running on Cyclone
should emulate a USB-Blaster. 
