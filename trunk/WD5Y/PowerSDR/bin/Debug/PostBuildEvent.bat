@echo off
copy "C:\Documents and Settings\Joe Torrey\Desktop\PowerSDR - HPSDR\Source\Console\CAT\CATStructs.xml" "..\..\bin\Debug\"; 
copy "C:\Documents and Settings\Joe Torrey\Desktop\PowerSDR - HPSDR\Source\Console\..\FFTW\libfftw3f-3.dll" "..\..\bin\Debug\"
copy "C:\Documents and Settings\Joe Torrey\Desktop\PowerSDR - HPSDR\Source\Console\..\libusb\bin\libusb0.dll" "..\..\bin\Debug\"
copy "C:\Documents and Settings\Joe Torrey\Desktop\PowerSDR - HPSDR\Source\Console\..\JanusAudio\OzyFirmwareBinaries\*" "..\..\bin\Debug\"

if errorlevel 1 goto CSharpReportError
goto CSharpEnd
:CSharpReportError
echo Project error: A tool returned an error code from the build event
exit 1
:CSharpEnd