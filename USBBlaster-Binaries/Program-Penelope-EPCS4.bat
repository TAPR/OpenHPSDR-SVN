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
ECHO  3. Quartus V9.0sp1
ECHO  4. Quartus V9.0sp2
ECHO  5. Quartus V9.1
ECHO  6. Quartus V9.1sp1
ECHO  Q. Quit
ECHO.
SET Choice=
SET /P Choice=Type the number and press Enter:
IF NOT '%Choice%'=='' SET Choice=%Choice:~0,1%
ECHO.
:: /I makes the IF comparison case-insensitive
IF /I '%Choice%'=='1' GOTO Q81
IF /I '%Choice%'=='2' GOTO Q90
IF /I '%Choice%'=='3' GOTO Q90sp1
IF /I '%Choice%'=='4' GOTO Q90sp2
IF /I '%Choice%'=='5' GOTO Q91
IF /I '%Choice%'=='6' GOTO Q91sp1
IF /I '%Choice%'=='7' GOTO Q91sp2
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

:Q90sp1
SET DIRECTORY=c:\altera\90sp1\qprogrammer\bin\quartus_pgm
GOTO LOOP

:Q90sp2
SET DIRECTORY=c:\altera\90sp2\qprogrammer\bin\quartus_pgm
GOTO LOOP

:Q91
SET DIRECTORY=c:\altera\91\qprogrammer\bin\quartus_pgm
GOTO LOOP

:Q91sp1
SET DIRECTORY=c:\altera\91sp1\qprogrammer\bin\quartus_pgm
GOTO LOOP

:Q91sp2
SET DIRECTORY=c:\altera\91sp2\qprogrammer\bin\quartus_pgm
GOTO LOOP

:: prompt the user for the file to use
:LOOP
ECHO.
ECHO.
ECHO A. Program using Penelope_v1.1
ECHO B. Program using Penelope_v1.2
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
%DIRECTORY% -c USB-Blaster Penelope_v1.1.cdf
GOTO CONTINUE
:ItemB
%DIRECTORY% -c USB-Blaster Penelope_v1.2.cdf
GOTO CONTINUE
:CONTINUE
PAUSE
CLS
:End

