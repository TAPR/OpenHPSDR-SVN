//==============================================================
//Copyright by FlexRadio Systems, 2004-2007
//This software is licensed under the GNU General Public License
//==============================================================

The source tree includes 8 projects which can all be compiled 
in Visual Studio .NET 2003.  The solution file (PowerSDR.sln) in 
the Source folder will compile all 6 projects and output the 
files to the bin\Debug or bin\Release folder depending on which
version is being compiled.

Note that MDAC 2.8 and the DirectX SDK are both required in
order to compile.  These libraries can be found on the Microsoft
website at www.microsoft.com/downloads.

WARNING!  Before opening the forms in design view in MS Visual
Studio .NET 2003, the PowerSDR project should be compiled.  This
is a workaround to prevent a bug that is documented at the below
link with regards to losing custom control information in the
project source code.  Note that it is also necessary to close all
design views of forms before switching between Debug and Release
versions.

See more at: http://www.kbalertz.com/feedback.aspx?kbnumber=842706

This code will be used to test the SDR-X hardware.  ANY QUESTIONS?
CALL Flex Radio Headquarters.  That is all you will get out of ANY
of us.

