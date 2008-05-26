USB Blaster HowTo 

This directory contains the binaries needed to use an Ozy board as an Altera USB Blaster. 

How To Use this code:  

Download the files in this directory to a directory on your PC. 

Attach Ozy and the board(s) to be programmed in consecutive slots in Atlas, Ozy needs to be 
in the lowest numbered slot used and the board(s) to be programmed in higher slots.  The last 
board to be programmed needs to have the LAST JTAG jumper present. 

Power up the Atlas bus and connect USB cable to computer. 

Run: USBBlaster.bat 

Once the batch file completes, Altera's programming tools should see Ozy as a USB Blaster and should
be able to program the devices on the JTAG chain 




