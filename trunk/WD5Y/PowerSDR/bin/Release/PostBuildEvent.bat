@echo off
copy "C:\Documents and Settings\Joe Torrey\Desktop\Copy of PowerSDR - REV. #3353\Source\Console\CAT\CATStructs.xml" "..\..\bin\Debug\"; 
copy "C:\Documents and Settings\Joe Torrey\Desktop\Copy of PowerSDR - REV. #3353\Source\Console\..\FFTW\libfftw3f-3.dll" "..\..\bin\Debug\"
copy "C:\Documents and Settings\Joe Torrey\Desktop\Copy of PowerSDR - REV. #3353\Source\Console\..\libusb\bin\libusb0.dll" "..\..\bin\Debug\"
copy "C:\Documents and Settings\Joe Torrey\Desktop\Copy of PowerSDR - REV. #3353\Source\Console\..\JanusAudio\OzyFirmwareBinaries\*" "..\..\bin\Debug\"

if errorlevel 1 goto CSharpReportError
goto CSharpEnd
:CSharpReportError
echo Project error: A tool returned an error code from the build event
exit 1
:CSharpEnd