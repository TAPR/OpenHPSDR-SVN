@ECHO OFF
CLS
:: Load the FX2 code 
load_firmwareV1.1.exe  0xfffe  0x7 ozyfw-sdr1k.hex
ECHO.
sleep 3000
upload_fpgaV1.1.exe 0xfffe 0x7 usb_blaster.rbf
ECHO.
sleep 3000
load_firmwareV1.1.exe  0xfffe  0x7 std.hex
ECHO.
sleep 3000
:: prompt the user for the file to use
:LOOP
ECHO.     
ECHO.      
ECHO A. Program using Mercury_v1
ECHO B. Program using Mercury_v2.1
ECHO C. Program using Mercury_v2.2
ECHO D. Program using Mercury_v2.3
ECHO E. Program using Mercury_v2.4
ECHO Q. Quit
ECHO.      
SET Choice=
SET /P Choice=Type the letter and press Enter: 

IF NOT '%Choice%'=='' SET Choice=%Choice:~0,1%
ECHO.
:: /I makes the IF comparison case-insensitive
IF /I '%Choice%'=='A' GOTO ItemA
IF /I '%Choice%'=='B' GOTO ItemB
IF /I '%Choice%'=='C' GOTO ItemC
IF /I '%Choice%'=='D' GOTO ItemD
IF /I '%Choice%'=='E' GOTO ItemE
IF /I '%Choice%'=='Q' GOTO End
ECHO "%Choice%" is not valid. Please try again.
ECHO.
GOTO Loop
:ItemA
c:\altera\81\qprogrammer\bin\quartus_pgm -c USB-Blaster mercury_v1.cdf
GOTO CONTINUE
:ItemB
c:\altera\81\qprogrammer\bin\quartus_pgm -c USB-Blaster mercury_v2.1.cdf
GOTO CONTINUE
:ItemC
c:\altera\81\qprogrammer\bin\quartus_pgm -c USB-Blaster mercury_v2.2.cdf
GOTO CONTINUE
:ItemD
c:\altera\81\qprogrammer\bin\quartus_pgm -c USB-Blaster mercury_v2.3.cdf
GOTO CONTINUE
:ItemE
c:\altera\81\qprogrammer\bin\quartus_pgm -c USB-Blaster mercury_v2.4.cdf
GOTO CONTINUE
:CONTINUE
PAUSE
CLS
:End

