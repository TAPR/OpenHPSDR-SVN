Updated 24 May 2009 

Now using Eclipse 3.4.2 with 5.0.x CDT plugins 
SDCC Plugin:  http://eclipse-sdcc.sourceforge.net/   Version 1.0.0
SCCC --version
SDCC : mcs51/gbz80/z80/avr/ds390/pic16/pic14/TININative/xa51/ds400/hc08 2.9.0 #5416 (Mar 22 2009) (MINGW32) 


On Vista sh.exe packaged with SDCC eclipse plugin does not work - get zsh for cygwin and rename to sh.exe and replace sh.exe from SDCC eclipse plugin  
Also on Vista - turn off UAC and run as Admin - otherwise ran into all sorts of file permission probs running SDCC build 

Bill Tracey (24 May 2009) 
-------------------
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