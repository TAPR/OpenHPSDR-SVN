@echo off
copy "F:\sdr1000.src\HPSDR-svn701\Source\Console\CAT\CATStructs.xml" "..\..\bin\Release\"; 
copy "F:\sdr1000.src\HPSDR-svn701\Source\Console\..\FFTW\libfftw3f-3.dll" "..\..\bin\Release\"
if errorlevel 1 goto CSharpReportError
goto CSharpEnd
:CSharpReportError
echo Project error: A tool returned an error code from the build event
exit 1
:CSharpEnd