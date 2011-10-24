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
ECHO  7. Quartus V9.1sp2
ECHO  8. Quartus 10.0
ECHO  9. Quartus V10.1sp1
ECHO  A. Quartus V11.0sp1
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
IF /I '%Choice%'=='8' GOTO Q100
IF /I '%Choice%'=='9' GOTO Q101sp1
IF /I '%Choice%'=='A' GOTO Q110sp1
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
#  SET DIRECTORY=c:\altera\91\qprogrammer\bin\quartus_pgm
SET DIRECTORY=C:\altera\91sp1\qprogrammer\bin\quartus_pgm
GOTO LOOP

:Q91sp1
SET DIRECTORY=c:\altera\91sp1\qprogrammer\bin\quartus_pgm
GOTO LOOP

:Q91sp2

SET DIRECTORY=c:\altera\91sp2\qprogrammer\bin\quartus_pgm
GOTO LOOP

:Q100
SET DIRECTORY=c:\altera\10.0\qprogrammer\bin\quartus_pgm
GOTO LOOP


:Q101sp1
SET DIRECTORY=c:\altera\10.1sp1\qprogrammer\bin\quartus_pgm
GOTO LOOP

:Q110sp1
SET DIRECTORY=c:\altera\11.0sp1\qprogrammer\bin\quartus_pgm
GOTO LOOP

:: prompt the user for the file to use
:LOOP
ECHO.
ECHO.
ECHO A. Program using Mercury_v1
ECHO B. Program using Mercury_v2.1
ECHO C. Program using Mercury_v2.2
ECHO D. Program using Mercury_v2.3
ECHO E. Program using Mercury_v2.4
ECHO F. Program using Mercury_v2.5
ECHO G. Program using Mercury_v2.6
ECHO H. Program using Mercury_v2.7
ECHO I. Program using Mercury_v2.8
ECHO J. Program using Mercury_v2.9a
ECHO K. Program using Mercury_Apollo_Test ** not for general use!
ECHO L. Program using Mercury_v3.0
ECHO Leu. Program using Mercury_eu_V3.0
ECHO.
ECHO Z. Mercury Test (carrier at 7.68MHz)
ECHO Q. Quit
ECHO.
SET Choice=
SET /P Choice=Type the letter and press Enter:

REM IF NOT '%Choice%'=='' SET Choice=%Choice:~0,1%
IF NOT '%Choice%'=='' SET Choice=%Choice:~0%

ECHO.
:: /I makes the IF comparison case-insensitive
IF /I '%Choice%'=='A' GOTO ItemA
IF /I '%Choice%'=='B' GOTO ItemB
IF /I '%Choice%'=='C' GOTO ItemC
IF /I '%Choice%'=='D' GOTO ItemD
IF /I '%Choice%'=='E' GOTO ItemE
IF /I '%Choice%'=='F' GOTO ItemF
IF /I '%Choice%'=='G' GOTO ItemG
IF /I '%Choice%'=='H' GOTO ItemH
IF /I '%Choice%'=='I' GOTO ItemI
IF /I '%Choice%'=='J' GOTO ItemJ
IF /I '%Choice%'=='K' GOTO ItemK
IF /I '%Choice%'=='L' GOTO ItemL
IF /I '%Choice%'=='Leu' GOTO ItemLEU
IF /I '%Choice%'=='Z' GOTO ItemZ

IF /I '%Choice%'=='Q' GOTO End
ECHO "%Choice%" is not valid. Please try again.
ECHO.
GOTO Loop
:ItemA
%DIRECTORY% -c USB-Blaster mercury_v1.cdf
GOTO CONTINUE
:ItemB
%DIRECTORY% -c USB-Blaster mercury_v2.1.cdf
GOTO CONTINUE
:ItemC
%DIRECTORY% -c USB-Blaster mercury_v2.2.cdf
GOTO CONTINUE
:ItemD
%DIRECTORY% -c USB-Blaster mercury_v2.3.cdf
GOTO CONTINUE
:ItemE
%DIRECTORY% -c USB-Blaster mercury_v2.4.cdf
GOTO CONTINUE
:ItemF
%DIRECTORY% -c USB-Blaster mercury_v2.5.cdf
GOTO CONTINUE
:ItemG
%DIRECTORY% -c USB-Blaster mercury_v2.6.cdf
GOTO CONTINUE
:ItemH
%DIRECTORY% -c USB-Blaster mercury_v2.7.cdf
GOTO CONTINUE
:ItemI
%DIRECTORY% -c USB-Blaster mercury_v2.8.cdf
GOTO CONTINUE
:ItemJ
%DIRECTORY% -c USB-Blaster mercury_v2.9.cdf
GOTO CONTINUE
:ItemK
%DIRECTORY% -c USB-Blaster mercury_apollo_test.cdf
GOTO CONTINUE
:ItemL
%DIRECTORY% -c USB-Blaster mercury_v3.0.cdf
GOTO CONTINUE
:ItemLEU
%DIRECTORY% -c USB-Blaster mercury_eu_v3.0.cdf
GOTO CONTINUE
:ItemZ
%DIRECTORY% -c USB-Blaster mercury_test.cdf
GOTO CONTINUE

:CONTINUE
PAUSE
CLS
:End

