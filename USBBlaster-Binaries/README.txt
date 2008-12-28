Phil Harman - VK6APH - 24 December 2008


Here are the steps requried to progam the EPCS16 on the Mercury board.

1. Install EITHER the Altera Quartus II V8.1 application (1GB download!) OR the 
   Quartus II Programmer application (only 80MB) from www.altera.com i.e.

   https://www.altera.com/support/software/download/sof-download_center.html

   Install in the default directory. We need to install one of these applications
   in order to load the large number of DLLs requried to run the simple command line
   programer (quartus_pgm.exe) - if there is a better way of doing this please share it!

3. Fit a jumper to the last JTAG header on your Mercury board i.e. JP7

4. Plug just your Mercury and Ozy board into the Atlas bus. The Mercury board MUST
   be closest to the power connector and the Ozy board in the next slot. 

5. Run the batch file Program-Mercury-EPCS16.bat and look at the output, there should 
   be no errors. This will take approximately 1 minute. 

6. Power cycle the supply to the Atlas bus. If the EPCS16 has loaded correctly then the
   right hand LEDs on the Mercury board will be flashing.

7. Test Mercury with PowerSDR.


This is how the process works.

Firstly the FX2 is loaded with code to make it appear as an Altera USBblaster.
We have previously  converted the usual Mercury.sof file into a Mercury.jic file (see
Altera application note AN370). 

We then use quartus_pgm to load mercury.jic in the FPGA. This first loads some code into
the Mercury FPGA that connects it to the EPCS16 flash memory. The imbeded mercury.sof file 
is then loaded into the EPCS16 flash memory.

