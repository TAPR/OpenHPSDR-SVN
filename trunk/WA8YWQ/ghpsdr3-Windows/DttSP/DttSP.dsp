# Microsoft Developer Studio Project File - Name="DttSP" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Dynamic-Link Library" 0x0102

CFG=DttSP - Win32 Debug
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "DttSP.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "DttSP.mak" CFG="DttSP - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "DttSP - Win32 Release" (based on "Win32 (x86) Dynamic-Link Library")
!MESSAGE "DttSP - Win32 Debug" (based on "Win32 (x86) Dynamic-Link Library")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""
# PROP Scc_LocalPath ""
CPP=cl.exe
MTL=midl.exe
RSC=rc.exe

!IF  "$(CFG)" == "DttSP - Win32 Release"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Release"
# PROP BASE Intermediate_Dir "Release"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Release"
# PROP Intermediate_Dir "Release"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /MT /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /D "_USRDLL" /D "DTTSP_EXPORTS" /YX /FD /c
# ADD CPP /nologo /MT /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /D "_USRDLL" /D "DTTSP_EXPORTS" /YX /FD /c
# ADD BASE MTL /nologo /D "NDEBUG" /mktyplib203 /win32
# ADD MTL /nologo /D "NDEBUG" /mktyplib203 /win32
# ADD BASE RSC /l 0x409 /d "NDEBUG"
# ADD RSC /l 0x409 /d "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /dll /machine:I386
# ADD LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /dll /machine:I386

!ELSEIF  "$(CFG)" == "DttSP - Win32 Debug"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 1
# PROP BASE Output_Dir "Debug"
# PROP BASE Intermediate_Dir "Debug"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 1
# PROP Output_Dir "Debug"
# PROP Intermediate_Dir "Debug"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /MTd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /D "_USRDLL" /D "DTTSP_EXPORTS" /YX /FD /GZ /c
# ADD CPP /nologo /MTd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /D "_USRDLL" /D "DTTSP_EXPORTS" /YX /FD /GZ /c
# ADD BASE MTL /nologo /D "_DEBUG" /mktyplib203 /win32
# ADD MTL /nologo /D "_DEBUG" /mktyplib203 /win32
# ADD BASE RSC /l 0x409 /d "_DEBUG"
# ADD RSC /l 0x409 /d "_DEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /dll /debug /machine:I386 /pdbtype:sept
# ADD LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /dll /debug /machine:I386 /pdbtype:sept

!ENDIF 

# Begin Target

# Name "DttSP - Win32 Release"
# Name "DttSP - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Source File

SOURCE=.\am_demod.c
# End Source File
# Begin Source File

SOURCE=.\banal.c
# End Source File
# Begin Source File

SOURCE=.\bufvec.c
# End Source File
# Begin Source File

SOURCE=.\correctIQ.c
# End Source File
# Begin Source File

SOURCE=.\cwtones.c
# End Source File
# Begin Source File

SOURCE=.\cxops.c
# End Source File
# Begin Source File

SOURCE=.\dcblock.c
# End Source File
# Begin Source File

SOURCE=.\dttspagc.c
# End Source File
# Begin Source File

SOURCE=.\fastrig.c
# End Source File
# Begin Source File

SOURCE=.\filter.c
# End Source File
# Begin Source File

SOURCE=.\fm_demod.c
# End Source File
# Begin Source File

SOURCE=.\getopt.c
# End Source File
# Begin Source File

SOURCE=.\getopt_long.c
# End Source File
# Begin Source File

SOURCE=.\graphiceq.c
# End Source File
# Begin Source File

SOURCE=.\hilbert.c
# End Source File
# Begin Source File

SOURCE=.\isoband.c
# End Source File
# Begin Source File

SOURCE=.\keyd.c
# End Source File
# Begin Source File

SOURCE=.\keyer.c
# End Source File
# Begin Source File

SOURCE=.\keyerio.c
# End Source File
# Begin Source File

SOURCE=.\lmadf.c
# End Source File
# Begin Source File

SOURCE=.\meter.c
# End Source File
# Begin Source File

SOURCE=.\metermon.c
# End Source File
# Begin Source File

SOURCE=.\noiseblanker.c
# End Source File
# Begin Source File

SOURCE=.\oscillator.c
# End Source File
# Begin Source File

SOURCE=.\ovsv.c
# End Source File
# Begin Source File

SOURCE=.\resample.c
# End Source File
# Begin Source File

SOURCE=.\resampleF.c
# End Source File
# Begin Source File

SOURCE=.\ringb.c
# End Source File
# Begin Source File

SOURCE=.\sdr.c
# End Source File
# Begin Source File

SOURCE=.\sdrexport.c
# End Source File
# Begin Source File

SOURCE=.\spectrum.c
# End Source File
# Begin Source File

SOURCE=.\speechproc.c
# End Source File
# Begin Source File

SOURCE=.\splitfields.c
# End Source File
# Begin Source File

SOURCE=.\spottone.c
# End Source File
# Begin Source File

SOURCE=.\update.c
# End Source File
# Begin Source File

SOURCE=.\window.c
# End Source File
# Begin Source File

SOURCE=.\winmain.c
# End Source File
# Begin Source File

SOURCE=.\wscompand.c
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# Begin Source File

SOURCE=.\am_demod.h
# End Source File
# Begin Source File

SOURCE=.\banal.h
# End Source File
# Begin Source File

SOURCE=.\bufvec.h
# End Source File
# Begin Source File

SOURCE=.\common.h
# End Source File
# Begin Source File

SOURCE=.\complex.h
# End Source File
# Begin Source File

SOURCE=.\correctIQ.h
# End Source File
# Begin Source File

SOURCE=.\cwtones.h
# End Source File
# Begin Source File

SOURCE=.\cxops.h
# End Source File
# Begin Source File

SOURCE=.\datatypes.h
# End Source File
# Begin Source File

SOURCE=.\dcblock.h
# End Source File
# Begin Source File

SOURCE=.\defs.h
# End Source File
# Begin Source File

SOURCE=.\dttspagc.h
# End Source File
# Begin Source File

SOURCE=.\fastrig.h
# End Source File
# Begin Source File

SOURCE=.\fftw.h
# End Source File
# Begin Source File

SOURCE=.\fftw3.h
# End Source File
# Begin Source File

SOURCE=.\fftw3_fix.h
# End Source File
# Begin Source File

SOURCE=.\filter.h
# End Source File
# Begin Source File

SOURCE=.\fm_demod.h
# End Source File
# Begin Source File

SOURCE=.\fromsys.h
# End Source File
# Begin Source File

SOURCE=.\getopt.h
# End Source File
# Begin Source File

SOURCE=.\graphiceq.h
# End Source File
# Begin Source File

SOURCE=.\hilbert.h
# End Source File
# Begin Source File

SOURCE=.\iosdr.h
# End Source File
# Begin Source File

SOURCE=.\isoband.h
# End Source File
# Begin Source File

SOURCE=.\keyer.h
# End Source File
# Begin Source File

SOURCE=.\lmadf.h
# End Source File
# Begin Source File

SOURCE=.\local.h
# End Source File
# Begin Source File

SOURCE=.\malloc16.h
# End Source File
# Begin Source File

SOURCE=.\meter.h
# End Source File
# Begin Source File

SOURCE=.\noiseblanker.h
# End Source File
# Begin Source File

SOURCE=.\oscillator.h
# End Source File
# Begin Source File

SOURCE=.\ovsv.h
# End Source File
# Begin Source File

SOURCE=.\pthread.h
# End Source File
# Begin Source File

SOURCE=.\resample.h
# End Source File
# Begin Source File

SOURCE=.\resampleF.h
# End Source File
# Begin Source File

SOURCE=.\ringb.h
# End Source File
# Begin Source File

SOURCE=.\sched.h
# End Source File
# Begin Source File

SOURCE=.\sdrexport.h
# End Source File
# Begin Source File

SOURCE=.\semaphore.h
# End Source File
# Begin Source File

SOURCE=.\spectrum.h
# End Source File
# Begin Source File

SOURCE=.\speechproc.h
# End Source File
# Begin Source File

SOURCE=.\splitfields.h
# End Source File
# Begin Source File

SOURCE=.\spottone.h
# End Source File
# Begin Source File

SOURCE=.\update.h
# End Source File
# Begin Source File

SOURCE=.\window.h
# End Source File
# Begin Source File

SOURCE=.\wscompand.h
# End Source File
# End Group
# Begin Group "Resource Files"

# PROP Default_Filter "ico;cur;bmp;dlg;rc2;rct;bin;rgs;gif;jpg;jpeg;jpe"
# End Group
# Begin Source File

SOURCE=".\libfftw3f-3.lib"
# End Source File
# Begin Source File

SOURCE=.\pthreadVC2.lib
# End Source File
# End Target
# End Project
