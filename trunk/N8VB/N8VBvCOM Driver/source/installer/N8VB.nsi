; Script generated with the Venis Install Wizard

; Define your application name
!define APPNAME "N8VBvCOM Driver"
!define APPNAMEANDVERSION "N8VBvCOM Driver Build 226"

; Main Install settings
Name "${APPNAMEANDVERSION}"
InstallDir "$PROGRAMFILES\N8VBvCOM Driver"
InstallDirRegKey HKLM "Software\${APPNAME}" ""
OutFile "N8VBvCOMSetup.exe"

BrandingText "N8VBvCOM Copyright © 2005 Phil Covington "

; Modern interface settings
!include "MUI.nsh"

!define MUI_ABORTWARNING
!define MUI_FINISHPAGE_RUN "$INSTDIR\devcon"
!define MUI_FINISHPAGE_RUN_PARAMETERS " -install n8vbvcom.inf n8vbvcom"
!define MUI_FINISHPAGE_RUN_TEXT "Install N8VBvCOM driver device into Windows"

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "..\..\license.txt"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

; Set languages (first is default language)
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "Danish"
!insertmacro MUI_LANGUAGE "Dutch"
!insertmacro MUI_LANGUAGE "Finnish"
!insertmacro MUI_LANGUAGE "French"
!insertmacro MUI_LANGUAGE "German"
!insertmacro MUI_LANGUAGE "Greek"
!insertmacro MUI_LANGUAGE "Italian"
!insertmacro MUI_LANGUAGE "Japanese"
!insertmacro MUI_LANGUAGE "Korean"
!insertmacro MUI_LANGUAGE "Norwegian"
!insertmacro MUI_LANGUAGE "Polish"
!insertmacro MUI_LANGUAGE "Russian"
!insertmacro MUI_LANGUAGE "Spanish"
!insertmacro MUI_LANGUAGE "Swedish"
!insertmacro MUI_RESERVEFILE_LANGDLL

Section "N8VBvCOM Driver Files" Section1

	; Set Section properties
	SetOverwrite ifnewer

	; Set Section Files and Shortcuts
	SetOutPath "$INSTDIR\"
	File "..\..\N8VBvCOM.inf"
	File "..\..\devcon.exe"
	File "..\..\installDriver.bat"
	File "..\..\license.txt"
	File "..\..\removeDriver.bat"
	File "..\..\update.bat"
	File "..\..\vCOM.sys"
	File "..\..\VComConfigurator.exe"
	CreateDirectory "$SMPROGRAMS\N8VBvCOM Driver"
	CreateShortCut "$SMPROGRAMS\N8VBvCOM Driver\Uninstall.lnk" "$INSTDIR\uninstall.exe"
	CreateShortCut "$SMPROGRAMS\N8VBvCOM Driver\VComConfigurator.lnk" "$INSTDIR\VComConfigurator.exe"

SectionEnd

Section /o "N8VBvCOM Source Files" Section2

	; Set Section properties
	SetOverwrite ifnewer

	; Set Section Files and Shortcuts
	SetOutPath "$INSTDIR\source\"
	File "..\buildchk_wxp_x86.log"
	File "..\buildfre_wxp_x86.log"
	File "..\makefile"
	File "..\sources"
	File "..\vCOM.h"
	File "..\vCOM.rc"
	File "..\vCOM_csq.c"
	File "..\vCOM_device.c"
	File "..\vCOM_dispatch.c"
	File "..\vCOM_init.c"
	File "..\vCOM_pnp.c"
	File "..\vCOM_readthread.c"
	File "..\vCOM_transferthread.c"
	File "..\vCOM_writethread.c"
	SetOutPath "$INSTDIR\source\installer\"
	File "N8VB.nsi"
	SetOutPath "$INSTDIR\source\configurator\"
	File "..\configurator\Unit1.dfm"
	File "..\configurator\Unit1.pas"
	File "..\configurator\unit1_1.inc"
	File "..\configurator\VComConfigurator.cfg"
	File "..\configurator\VComConfigurator.dof"
	File "..\configurator\VComConfigurator.dpr"
	SetOutPath "$INSTDIR\source\configurator\bin"
	SetOutPath "$INSTDIR\source\configurator\dcu"
	SetOutPath "$INSTDIR\source\objchk_wxp_x86\"
	File "..\objchk_wxp_x86\_objects.mac"
	SetOutPath "$INSTDIR\source\objchk_wxp_x86\i386\"
	File "..\objchk_wxp_x86\i386\vCOM.pdb"
	File "..\objchk_wxp_x86\i386\vcom.res"
	File "..\objchk_wxp_x86\i386\vCOM.sys"
	File "..\objchk_wxp_x86\i386\vcom_csq.obj"
	File "..\objchk_wxp_x86\i386\vcom_device.obj"
	File "..\objchk_wxp_x86\i386\vcom_dispatch.obj"
	File "..\objchk_wxp_x86\i386\vcom_init.obj"
	File "..\objchk_wxp_x86\i386\vcom_pnp.obj"
	File "..\objchk_wxp_x86\i386\vcom_readthread.obj"
	File "..\objchk_wxp_x86\i386\vcom_transferthread.obj"
	File "..\objchk_wxp_x86\i386\vcom_writethread.obj"
	SetOutPath "$INSTDIR\source\objfre_wxp_x86\"
	File "..\objfre_wxp_x86\_objects.mac"
	SetOutPath "$INSTDIR\source\objfre_wxp_x86\i386\"
	File "..\objfre_wxp_x86\i386\vCOM.pdb"
	File "..\objfre_wxp_x86\i386\vcom.res"
	File "..\objfre_wxp_x86\i386\vCOM.sys"
	File "..\objfre_wxp_x86\i386\vcom_csq.obj"
	File "..\objfre_wxp_x86\i386\vcom_device.obj"
	File "..\objfre_wxp_x86\i386\vcom_dispatch.obj"
	File "..\objfre_wxp_x86\i386\vcom_init.obj"
	File "..\objfre_wxp_x86\i386\vcom_pnp.obj"
	File "..\objfre_wxp_x86\i386\vcom_readthread.obj"
	File "..\objfre_wxp_x86\i386\vcom_transferthread.obj"
	File "..\objfre_wxp_x86\i386\vcom_writethread.obj"

SectionEnd

Section -FinishSection

	WriteRegStr HKLM "Software\${APPNAME}" "" "$INSTDIR"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "DisplayName" "${APPNAME}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "UninstallString" "$INSTDIR\uninstall.exe"
	WriteUninstaller "$INSTDIR\uninstall.exe"

SectionEnd

; Modern install component descriptions
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${Section1} "Driver and configuration files."
	!insertmacro MUI_DESCRIPTION_TEXT ${Section2} "GPL licensed source files for N8VBvCom driver, configurator, and install script."
!insertmacro MUI_FUNCTION_DESCRIPTION_END

;Uninstall section
Section Uninstall

	;Uninstall driver device
	ExecWait "$INSTDIR\devcon remove n8vbvcom"
	
	;Remove from registry...
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}"
	DeleteRegKey HKLM "SOFTWARE\${APPNAME}"

	; Delete self
	Delete "$INSTDIR\uninstall.exe"

	; Delete Shortcuts
	Delete "$SMPROGRAMS\N8VBvCOM Driver\Uninstall.lnk"
	Delete "$SMPROGRAMS\N8VBvCOM Driver\VComConfigurator.lnk"

	; Clean up N8VBvCOM Driver
	Delete "$INSTDIR\N8VBvCOM.inf"
	Delete "$INSTDIR\devcon.exe"
	Delete "$INSTDIR\installDriver.bat"
	Delete "$INSTDIR\license.txt"
	Delete "$INSTDIR\removeDriver.bat"
	Delete "$INSTDIR\update.bat"
	Delete "$INSTDIR\vCOM.sys"
	Delete "$INSTDIR\VComConfigurator.exe"

	; Clean up N8VBvCOM Source Files
	Delete "$INSTDIR\source\buildchk_wxp_x86.log"
	Delete "$INSTDIR\source\buildfre_wxp_x86.log"
	Delete "$INSTDIR\source\makefile"
	Delete "$INSTDIR\source\sources"
	Delete "$INSTDIR\source\vCOM.h"
	Delete "$INSTDIR\source\vCOM.rc"
	Delete "$INSTDIR\source\vCOM_csq.c"
	Delete "$INSTDIR\source\vCOM_device.c"
	Delete "$INSTDIR\source\vCOM_dispatch.c"
	Delete "$INSTDIR\source\vCOM_init.c"
	Delete "$INSTDIR\source\vCOM_pnp.c"
	Delete "$INSTDIR\source\vCOM_readthread.c"
	Delete "$INSTDIR\source\vCOM_transferthread.c"
	Delete "$INSTDIR\source\vCOM_writethread.c"
	Delete "$INSTDIR\source\installer\N8VB.nsi"
	Delete "$INSTDIR\source\configurator\Unit1.dfm"
	Delete "$INSTDIR\source\configurator\Unit1.pas"
	Delete "$INSTDIR\source\configurator\unit1_1.inc"
	Delete "$INSTDIR\source\configurator\VComConfigurator.cfg"
	Delete "$INSTDIR\source\configurator\VComConfigurator.dof"
	Delete "$INSTDIR\source\configurator\VComConfigurator.dpr"
	Delete "$INSTDIR\source\objchk_wxp_x86\_objects.mac"
	Delete "$INSTDIR\source\objchk_wxp_x86\i386\vCOM.pdb"
	Delete "$INSTDIR\source\objchk_wxp_x86\i386\vcom.res"
	Delete "$INSTDIR\source\objchk_wxp_x86\i386\vCOM.sys"
	Delete "$INSTDIR\source\objchk_wxp_x86\i386\vcom_csq.obj"
	Delete "$INSTDIR\source\objchk_wxp_x86\i386\vcom_device.obj"
	Delete "$INSTDIR\source\objchk_wxp_x86\i386\vcom_dispatch.obj"
	Delete "$INSTDIR\source\objchk_wxp_x86\i386\vcom_init.obj"
	Delete "$INSTDIR\source\objchk_wxp_x86\i386\vcom_pnp.obj"
	Delete "$INSTDIR\source\objchk_wxp_x86\i386\vcom_readthread.obj"
	Delete "$INSTDIR\source\objchk_wxp_x86\i386\vcom_transferthread.obj"
	Delete "$INSTDIR\source\objchk_wxp_x86\i386\vcom_writethread.obj"
	Delete "$INSTDIR\source\objfre_wxp_x86\_objects.mac"
	Delete "$INSTDIR\source\objfre_wxp_x86\i386\vCOM.pdb"
	Delete "$INSTDIR\source\objfre_wxp_x86\i386\vcom.res"
	Delete "$INSTDIR\source\objfre_wxp_x86\i386\vCOM.sys"
	Delete "$INSTDIR\source\objfre_wxp_x86\i386\vcom_csq.obj"
	Delete "$INSTDIR\source\objfre_wxp_x86\i386\vcom_device.obj"
	Delete "$INSTDIR\source\objfre_wxp_x86\i386\vcom_dispatch.obj"
	Delete "$INSTDIR\source\objfre_wxp_x86\i386\vcom_init.obj"
	Delete "$INSTDIR\source\objfre_wxp_x86\i386\vcom_pnp.obj"
	Delete "$INSTDIR\source\objfre_wxp_x86\i386\vcom_readthread.obj"
	Delete "$INSTDIR\source\objfre_wxp_x86\i386\vcom_transferthread.obj"
	Delete "$INSTDIR\source\objfre_wxp_x86\i386\vcom_writethread.obj"

	; Remove remaining directories
	RMDir "$SMPROGRAMS\N8VBvCOM Driver"
	RMDir "$INSTDIR\source\configurator\bin\"
	RMDir "$INSTDIR\source\configurator\dcu\"
	RMDir "$INSTDIR\source\configurator\"
	RMDir "$INSTDIR\source\objfre_wxp_x86\i386\"
	RMDir "$INSTDIR\source\objfre_wxp_x86\"
	RMDir "$INSTDIR\source\objchk_wxp_x86\i386\"
	RMDir "$INSTDIR\source\objchk_wxp_x86\"
	RMDir "$INSTDIR\source\installer\"
	RMDir "$INSTDIR\source\"
	RMDir "$INSTDIR\"

SectionEnd

; On initialization
Function .onInit

	!insertmacro MUI_LANGDLL_DISPLAY

FunctionEnd

; eof