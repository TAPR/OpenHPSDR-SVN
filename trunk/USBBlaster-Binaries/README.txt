Phil Harman - VK6APH - 1 Jan 2009


Here are the steps requried to progam the EPCS16 on the Mercury board.

1. Install the Quartus II Programmer application (80MB) from www.altera.com i.e.

   https://www.altera.com/support/software/download/sof-download_center.html

   Install in the default directory. If you install in a different directory then edit
   Program-Mercury-EPCS16.bat to reflect the change. 

   We need to install this application in order to load the large number of
   DLLs requried to run the simple command line  programer (quartus_pgm.exe)
   - if there is a better way of doing this please share it!!

2. THE FIRST TIME YOU DO THIS DO THE FOLLOWING. SUBSEQUENTLY GOTO STEP 3.
    
   These steps load the Altera USBblaster drivers.    

   Connect just your Ozy board to the Atlas bus, connect the USB cable and power it on.

   Run the file usbblaster.bat. This will load the code in the FX2 to make it appear 
   to be an Altera USBblaster. 

   Windows will detect a new USB device. You need to install the drivers for it.

   See http://www.altera.com/literature/ug/ug_usb_blstr.pdf for driver installation instructions.

   Check that the drivers have installed correctly by looking in 

   Start\Control Panel\System\Hardware\Device Manager\Universal Serial Bus controllers
   
   there should be an entry marked  Altera USB-Blaster


3. Fit a jumper to the last JTAG header on your Mercury board i.e. JP7, bottom right of the board.

4. Power off your Atlas board. Plug just your Mercury and Ozy board into the Atlas bus.
   The Mercury board MUST be closest to the power connector and the Ozy board in the NEXT slot.
   Turn the power on to your Atlas bus.  

5. Run the batch file Program-Mercury-EPCS16.bat, select which version of Mercury code you wish to load
   and look at the output, there should be no errors. It takes approximately 1 minute to load the EPCS16. 

6. Power cycle the supply to the Atlas bus. If the EPCS16 has loaded correctly then the
   right hand LEDs on the Mercury board will be flashing.

7. Remove the PowerSDR database file PowerSDR.mdb and allow PowerSDR to rebuild it when you
   next start it. Make a note of any important settings you want to keep. 

8. Test Mercury with PowerSDR.


This is how the process works.

Firstly the FX2 is loaded with code to make it appear as an Altera USBblaster.
We have previously  converted the usual Mercury.sof file into a Mercury.jic file (see
Altera application note AN370).

We then load code into the FPGA that enables it to write to its associated flash memory.

We then use quartus_pgm to run mercury*.cdf. This loads Mercury*.jic into the FPGA, 
the imbeded mercury.sof file is then loaded into the EPCS16 flash memory.

