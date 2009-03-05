@ECHO OFF
CLS
:: Load the FX2 code 
loadFW.exe  0xfffe  0x7 ozyfw-sdr1k.hex
ECHO.
msecsleep 3000
loadFPGA.exe 0xfffe 0x7 usb_blaster.rbf
ECHO.
sleep 3000
loadFW.exe  0xfffe  0x7 std.hex
ECHO.
msecsleep 3000
:: prompt the user for the file to use
:LOOP
ECHO.     
ECHO.      
ECHO A. Program using Penelope_v1.1
ECHO Q. Quit
ECHO.      
SET Choice=
SET /P Choice=Type the letter and press Enter: 

IF NOT '%Choice%'=='' SET Choice=%Choice:~0,1%
ECHO.
:: /I makes the IF comparison case-insensitive
IF /I '%Choice%'=='A' GOTO ItemA
IF /I '%Choice%'=='Q' GOTO End
ECHO "%Choice%" is not valid. Please try again.
ECHO.
GOTO Loop
:ItemA
c:\altera\81\qprogrammer\bin\quartus_pgm -c USB-Blaster Penelope_v1.1.cdf
GOTO CONTINUE
:CONTINUE
PAUSE
CLS
:End

