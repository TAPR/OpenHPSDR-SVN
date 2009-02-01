This is the Ozy firmware to control an SDR 1000 via Janus.  It is derived from Phil Covington's (N8VB)
Ozy Firmware 

This project is setup to build in Eclipse with SDCC.  The versions of things I'm using are: 

Eclipse: 3.2.1

SDCC Plugin:  http://eclipse-sdcc.sourceforge.net/   Version 1.0.0 

F:\sdr1000.src\HPSDR-svn701\bin\Release>sdcc --version
SDCC : mcs51/gbz80/z80/avr/ds390/pic16/pic14/TININative/xa51/ds400/hc08 2.6.0 #4309 (Jul 28 2006) (MINGW32)

I do not use the default builder for the link step because I can't find a way to get it to link 
the object files in the order needed.    See tools/custom-link.zsh for the link step -- it needs to be updated 
if object files are added to the project 

Serial port output

This code is seutp to write to the Ozy FX2 serial port (J6) at 115200 baud.   

Bill Tracey (kd5tfd) 4 Mar 2007 

Change History: 

Initial Release: 4 Mar 2007 