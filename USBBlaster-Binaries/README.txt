Phil Harman - VK6APH - 7 September 2011

 *******  NOTE:  Altera indicates that their software will run on 32 bit versions of XP and Vista. Support or Win7 or 64 bit vesions is included from V10.0  ******


Here are the steps requried to progam the EPCS16 on the Mercury board, EPCS4 on Penelope or PennyLane and the EPM240T100 on Janus.


1. Download all the files from here


	svn://svn.openhpsdr.org/svn/repos_sdr_sdr_hpsdr/trunk/USBBlaster-Binaries

2. Install the Quartus II Programmer application (177MB) from www.altera.com i.e.

   https://www.altera.com/support/software/download/programming/quartus2/dnl-quartus2_programmer.jsp

   The latest version is V10.1sp1.

   Install in the default directory. If you install in a different directory then edit
   Program-Mercury-EPCS16.bat, Program-Penelope-EPCS4.bat and Program-Janus-EPM240T100.bat to reflect the change. 

   We need to install this application in order to load the large number of
   DLLs requried to run the simple command line  programer (quartus_pgm.exe)
   - if there is a better way of doing this please share it!!

3. THE FIRST TIME YOU DO THIS DO THE FOLLOWING. SUBSEQUENTLY GOTO STEP 4.
    
   These steps load the Altera USBblaster drivers.    

   Connect just your Ozy board to the Atlas bus, connect the USB cable and power it on.

   Run the file usbblaster.bat. This will load the code in the FX2 to make it appear 
   to be an Altera USBblaster. 

   Windows will detect a new USB device. You need to install the drivers for it.

   See http://www.altera.com/literature/ug/ug_usb_blstr.pdf for driver installation instructions.


   Most of this installation instruction deals with the 10-pin programming cable.
   What you need can be found in section 1.3.

   After running the batch-file usbblaster.bat, the Found New Hardware Wizard will open.

   Select: Install from a list or specific location and browse to

   C:/altera/[version]/qprogrammer/drivers/usb-blaster  where [version] is the version you have downloaded

   The Wizard will then install altera USB-Blaster


   Check that the drivers have installed correctly by looking in 

   Start\Control Panel\System\Hardware\Device Manager\Universal Serial Bus controllers

   there should be an entry marked  Altera USB-Blaster


4. To update your Mercury board, fit a jumper to the header pins marked 
   LAST JTAG JP7 at bottom left of the board just above  the Atlas connector.
   To update your Penelope or PennyLane board, fit a jumper to the header pins marked
   LAST JP7 located at the edge of the board next to the FPGA.
   To update your Janus board, fit a jumber to the header pins marked 
   LAST JTAG JP12 located at the bottom left of the board just above the Atlas connector. 

5. Power off your Atlas board. To program Mercury, plug just your Mercury and Ozy board into the Atlas bus.
   To program Penelope, plug just your Penelope board and Ozy board in the Atlas bus. 
   To program Janus, plug just your Janus board and Ozy board in the Atlas bus.
   The board to be programmed MUST be closest to the power connector and the Ozy board in the NEXT slot.
   Turn the power on to your Atlas bus.  

6. To program Mercury run the batch file Program-Mercury-EPCS16.bat, select which version of the Altera Quartus 
   programmer code you are using and the Mercury code you wish to load and look at the output, there should be no errors.
   It takes approximately 1 minute to load the EPCS16.

   To program Penelope run the batch file Program-Penelope-EPCS4.bat, select which version of the Altera Quartus 
   programmer code you are using and the Penelope code you wish to load and look at the output, there should be no errors.
   It takes approximately 15 seconds to load the EPCS4.

   To program Janus run the batch file Program-Janus-EPM240T100.bat, select which version of the Altera Quartus 
   programmer code you are using and the Janus code you wish to load and look at the output, there should be no errors.
   It takes approximately 5seconds to load the EPM240T100 on Janus.


7. Power cycle the supply to the Atlas bus. 

8. Remove the PowerSDR database file PowerSDR.mdb and allow PowerSDR to rebuild it when you
   next start it. Make a note of any important settings you want to keep. 

9. Remove Power to the Atlas bus, restart your PC and load PowerSDR.

10. Test Mercury/Penelope/Janus with PowerSDR. If necessary recalibrate Frequency and Level.

NOTE: The jumper inserted at step 3 can be left in place for now. 


This is how the process works.

Firstly the FX2 is loaded with code to make it appear as an Altera USBblaster.
We have previously  converted the usual *.sof file into a *.jic file (see
Altera application note AN370).

We then load code into the FPGA that enables it to write to its associated flash memory.

We then use quartus_pgm to run *.cdf. This loads *.jic into the FPGA, 
the imbeded *.sof file is then loaded into the flash memory.

