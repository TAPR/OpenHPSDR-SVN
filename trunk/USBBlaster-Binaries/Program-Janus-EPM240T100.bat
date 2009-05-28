@ECHO OFF
CLS
:: Load the FX2 code 
loadFW.exe  0xfffe  0x7 ozyfw-sdr1k.hex
ECHO.
msecsleep 3000
loadFPGA.exe 0xfffe 0x7 usb_blaster.rbf
ECHO.
msecsleep 3000
loadFW.exe  0xfffe  0x7 std.hex
ECHO.
msecsleep 3000
:: prompt the user for the version of Quartus to use
:LOOP1
ECHO Which version of the Quartus Programmer are you using?
ECHO.
ECHO.
ECHO  1. Quartus V8.1
ECHO  2. Quartus V9.0
ECHO  3. Quit
ECHO.
SET Choice=
SET /P Choice=Type the number and press Enter:
IF NOT '%Choice%'=='' SET Choice=%Choice:~0,1%
ECHO.
:: /I makes the IF comparison case-insensitive
IF /I '%Choice%'=='1' GOTO Q81
IF /I '%Choice%'=='2' GOTO Q90
IF /I '%Choice%'=='Q' GOTO End
ECHO "%Choice%" is not valid. Please try again.
ECHO.
GOTO Loop1

:Q81
SET DIRECTORY=c:\altera\81\qprogrammer\bin\quartus_pgm
GOTO LOOP

:Q90
SET DIRECTORY=c:\altera\90\qprogrammer\bin\quartus_pgm
GOTO LOOP
:: prompt the user for the file to use
:LOOP
ECHO.     
ECHO.      
ECHO A. Program using Janus_V2
ECHO B. Program using Janus_V3
ECHO Q. Quit
ECHO.      
SET Choice=
SET /P Choice=Type the letter and press Enter: 

IF NOT '%Choice%'=='' SET Choice=%Choice:~0,1%
ECHO.
:: /I makes the IF comparison case-insensitive
IF /I '%Choice%'=='A' GOTO ItemA
IF /I '%Choice%'=='B' GOTO ItemB
IF /I '%Choice%'=='Q' GOTO End
ECHO "%Choice%" is not valid. Please try again.
ECHO.
GOTO Loop
:ItemA
%DIRECTORY% -c USB-Blaster Janus_V2.cdf
GOTO CONTINUE
:ItemB
%DIRECTORY% -c USB-Blaster Janus_V3.cdf
GOTO CONTINUE
:CONTINUE
PAUSE
CLS
:End

