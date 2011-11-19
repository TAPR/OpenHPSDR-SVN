@echo off
set skinpath=.\..\

Set console=Console.png
Set picDisplay=picDisplay.png
Set display=panDisplayBack.png
set panelband=panelbackBand.png
set panelmode=panelbackMode.png
set paneloptions=panelbackOptions.png
set panelmodespecific=panelbackModeSpecific.png
set powerup=chkPower-0.png
set powerdn=chkPower-1.png
set rx2up=rx2-0.png
set rx2dn=rx2-1.png
set playup=play-0.png
set playdn=play-1.png
set recup=record-0.png
set recdn=record-1.png
set up2=80X23bttn-0.png
set dn2=80X23bttn-1.png
set dn2red=80x23bttn_red-1.png
set up3=40X20bttn-0.png
set dn3=40X20bttn-1.png
set up4=16X20bttn-0.png
set dn4=16X20bttn-1.png
set up5=32X20bttn-0.png
set dn5=32X20bttn-1.png
set up1=longledbttn-0.png
set dn1=longledbttn-1.png
set slideback=slider_back.png
set slidehead=slider_head.png
set slidebackv=slider_back_v.png
set slideheadv=slider_head_v.png




copy %console% %skinpath%Console.png
copy %picDisplay% %skinpath%picDisplay.png
REM copy %display% %skinpath%panDisplayBack.png
copy %panelband% %skinpath%panelBandHF.png
copy %panelband% %skinpath%panelBandVHF.png
copy %panelmode% %skinpath%panelMode.png
copy %paneloptions% %skinpath%panelOptions.png
copy %panelmodespecific% %skinpath%panelModeSpecificPhone.png
copy %panelmodespecific% %skinpath%panelModeSpecificCW.png
copy %panelmodespecific% %skinpath%panelModeSpecificDigital.png
copy %panelmodespecific% %skinpath%panelModeSpecificFM.png


copy %slideback% %skinpath%ptbDisplayPan-back.png
copy %slideback% %skinpath%ptbDisplayZoom-back.png
copy %slideback% %skinpath%ptbAF-back.png
copy %slideback% %skinpath%ptbRF-back.png
copy %slideback% %skinpath%ptbPWR-back.png
copy %slideback% %skinpath%ptbSquelch-back.png
copy %slideback% %skinpath%ptbMic-back.png
copy %slideback% %skinpath%ptbFMMic-back.png
copy %slideback% %skinpath%ptbDX-back.png
copy %slideback% %skinpath%ptbCPDR-back.png
copy %slideback% %skinpath%ptbVOX-back.png
copy %slideback% %skinpath%ptbNoiseGate-back.png
copy %slideback% %skinpath%ptbFilterWidth-back.png
copy %slideback% %skinpath%ptbFilterShift-back.png
copy %slideback% %skinpath%ptbPanMainRX-back.png
copy %slideback% %skinpath%ptbPanSubRX-back.png
copy %slideback% %skinpath%ptbRX2RF-back.png
copy %slideback% %skinpath%ptbRX2Squelch-back.png
copy %slideback% %skinpath%ptbCWSpeed-back.png
copy %slideback% %skinpath%ptbRX2Pan-back.png
copy %slideback% %skinpath%ptbVACRXGain-back.png
copy %slideback% %skinpath%ptbVACTXGain-back.png

copy %slidebackv% %skinpath%ptbRX2Gain-back.png
copy %slidebackv% %skinpath%ptbRX0Gain-back.png
copy %slidebackv% %skinpath%ptbRX1Gain-back.png


copy %slidehead% %skinpath%ptbDisplayPan-head.png
copy %slidehead% %skinpath%ptbDisplayZoom-head.png
copy %slidehead% %skinpath%ptbAF-head.png
copy %slidehead% %skinpath%ptbRF-head.png
copy %slidehead% %skinpath%ptbPWR-head.png
copy %slidehead% %skinpath%ptbSquelch-head.png
copy %slidehead% %skinpath%ptbMic-head.png
copy %slidehead% %skinpath%ptbFMMic-head.png
copy %slidehead% %skinpath%ptbDX-head.png
copy %slidehead% %skinpath%ptbCPDR-head.png
copy %slidehead% %skinpath%ptbVOX-head.png
copy %slidehead% %skinpath%ptbNoiseGate-head.png
copy %slidehead% %skinpath%ptbFilterWidth-head.png
copy %slidehead% %skinpath%ptbFilterShift-head.png
copy %slidehead% %skinpath%ptbPanMainRX-head.png
copy %slidehead% %skinpath%ptbPanSubRX-head.png
copy %slidehead% %skinpath%ptbRX2RF-head.png
copy %slidehead% %skinpath%ptbRX2Squelch-head.png
copy %slidehead% %skinpath%ptbCWSpeed-head.png
copy %slidehead% %skinpath%ptbRX2Pan-head.png
copy %slidehead% %skinpath%ptbVACRXGain-head.png
copy %slidehead% %skinpath%ptbVACTXGain-head.png

copy %slideheadv% %skinpath%ptbRX2Gain-head.png
copy %slideheadv% %skinpath%ptbRX0Gain-head.png
copy %slideheadv% %skinpath%ptbRX1Gain-head.png


copy %powerup% %skinpath%chkPower-0.png
copy %powerdn% %skinpath%chkPower-1.png


copy %rx2up% %skinpath%chkRX2-0.png
copy %rx2dn% %skinpath%chkRX2-1.png


copy %playup% %skinpath%ckQuickPlay-0.png
copy %playdn% %skinpath%ckQuickPlay-1.png


copy %recup% %skinpath%ckQuickRec-0.png
copy %recdn% %skinpath%ckQuickRec-1.png


copy %up2% %skinpath%chkVFOSync-0.png
copy %dn2% %skinpath%chkVFOSync-1.png
copy %up2% %skinpath%chkVFOLock-0.png
copy %dn2% %skinpath%chkVFOLock-1.png
copy %up2% %skinpath%chkBCI-0.png
copy %dn2% %skinpath%chkBCI-1.png
copy %up2% %skinpath%chkVFOATX-0.png
copy %dn2red% %skinpath%chkVFOATX-1.png
copy %up2% %skinpath%chkVFOBTX-0.png
copy %dn2red% %skinpath%chkVFOBTX-1.png
copy %up2% %skinpath%radFMDeviation2kHz-0.png
copy %dn2% %skinpath%radFMDeviation2kHz-1.png
copy %up2% %skinpath%radFMDeviation5kHz-0.png
copy %dn2% %skinpath%radFMDeviation5kHz-1.png
copy %up2% %skinpath%chkFMCTCSS-0.png
copy %dn2% %skinpath%chkFMCTCSS-1.png


copy %up3% %skinpath%radDisplayZoom05-0.png
copy %dn3% %skinpath%radDisplayZoom05-1.png
copy %up3% %skinpath%radDisplayZoom1x-0.png
copy %dn3% %skinpath%radDisplayZoom1x-1.png
copy %up3% %skinpath%radDisplayZoom2x-0.png
copy %dn3% %skinpath%radDisplayZoom2x-1.png
copy %up3% %skinpath%radDisplayZoom4x-0.png
copy %dn3% %skinpath%radDisplayZoom4x-1.png


copy %up4% %skinpath%btnXITReset-0.png
copy %dn4% %skinpath%btnXITReset-1.png
copy %up4% %skinpath%btnRITReset-0.png
copy %dn4% %skinpath%btnRITReset-1.png
copy %up4% %skinpath%btnTuneStepChangeSmaller-0.png
copy %dn4% %skinpath%btnTuneStepChangeSmaller-1.png
copy %up4% %skinpath%btnTuneStepChangeLarger-0.png
copy %dn4% %skinpath%btnTuneStepChangeLarger-1.png


copy %up5% %skinpath%chkXIT-0.png
copy %dn5% %skinpath%chkXIT-1.png
copy %up5% %skinpath%chkRIT-0.png
copy %dn5% %skinpath%chkRIT-1.png


copy %up1% %skinpath%radRX2Filter1-0.png
copy %up1% %skinpath%radRX2Filter2-0.png
copy %up1% %skinpath%radRX2Filter3-0.png
copy %up1% %skinpath%radRX2Filter4-0.png
copy %up1% %skinpath%radRX2Filter5-0.png
copy %up1% %skinpath%radRX2Filter6-0.png
copy %up1% %skinpath%radRX2Filter7-0.png
copy %up1% %skinpath%radRX2FilterVar1-0.png
copy %up1% %skinpath%radRX2FilterVar2-0.png
copy %up1% %skinpath%radRX2ModeAM-0.png
copy %up1% %skinpath%radRX2ModeSAM-0.png
copy %up1% %skinpath%radRX2ModeDSB-0.png
copy %up1% %skinpath%radRX2ModeCWU-0.png
copy %up1% %skinpath%radRX2ModeDIGU-0.png
copy %up1% %skinpath%radRX2ModeDIGL-0.png
copy %up1% %skinpath%radRX2ModeLSB-0.png
copy %up1% %skinpath%radRX2ModeSPEC-0.png
copy %up1% %skinpath%radRX2ModeDRM-0.png
copy %up1% %skinpath%radRX2ModeFMN-0.png
copy %up1% %skinpath%radRX2ModeUSB-0.png
copy %up1% %skinpath%radRX2ModeCWL-0.png
copy %up1% %skinpath%radFilter1-0.png
copy %up1% %skinpath%radFilter2-0.png
copy %up1% %skinpath%radFilter3-0.png
copy %up1% %skinpath%radFilter4-0.png
copy %up1% %skinpath%radFilter5-0.png
copy %up1% %skinpath%radFilter6-0.png
copy %up1% %skinpath%radFilter7-0.png
copy %up1% %skinpath%radFilter8-0.png
copy %up1% %skinpath%radFilter9-0.png
copy %up1% %skinpath%radFilter10-0.png
copy %up1% %skinpath%radFilterVar1-0.png
copy %up1% %skinpath%radFilterVar2-0.png
copy %up1% %skinpath%btnMemoryQuickSave-0.png
copy %up1% %skinpath%btnFilterShiftReset-0.png
copy %up1% %skinpath%btnMemoryQuickRestore-0.png

copy %up1% %skinpath%btnBandHF-0.png
copy %up1% %skinpath%radBand2-0.png
copy %up1% %skinpath%radBand6-0.png
copy %up1% %skinpath%radBand10-0.png
copy %up1% %skinpath%radBand12-0.png
copy %up1% %skinpath%radBand15-0.png
copy %up1% %skinpath%radBand17-0.png
copy %up1% %skinpath%radBand20-0.png
copy %up1% %skinpath%radBand30-0.png
copy %up1% %skinpath%radBand40-0.png
copy %up1% %skinpath%radBand60-0.png
copy %up1% %skinpath%radBand80-0.png
copy %up1% %skinpath%radBand160-0.png
copy %up1% %skinpath%radBandWWV-0.png
copy %up1% %skinpath%radBandGEN-0.png

copy %up1% %skinpath%btnBandVHF-0.png
copy %up1% %skinpath%radBandVHF13-0.png
copy %up1% %skinpath%radBandVHF12-0.png
copy %up1% %skinpath%radBandVHF11-0.png
copy %up1% %skinpath%radBandVHF10-0.png
copy %up1% %skinpath%radBandVHF9-0.png
copy %up1% %skinpath%radBandVHF8-0.png
copy %up1% %skinpath%radBandVHF7-0.png
copy %up1% %skinpath%radBandVHF6-0.png
copy %up1% %skinpath%radBandVHF5-0.png
copy %up1% %skinpath%radBandVHF4-0.png
copy %up1% %skinpath%radBandVHF3-0.png
copy %up1% %skinpath%radBandVHF2-0.png
copy %up1% %skinpath%radBandVHF1-0.png
copy %up1% %skinpath%radBandVHF0-0.png

copy %up1% %skinpath%radModeLSB-0.png
copy %up1% %skinpath%radModeCWL-0.png
copy %up1% %skinpath%radModeUSB-0.png
copy %up1% %skinpath%radModeFMN-0.png
copy %up1% %skinpath%radModeDRM-0.png
copy %up1% %skinpath%radModeSPEC-0.png
copy %up1% %skinpath%radModeDIGL-0.png
copy %up1% %skinpath%radModeDIGU-0.png
copy %up1% %skinpath%radModeCWU-0.png
copy %up1% %skinpath%radModeDSB-0.png
copy %up1% %skinpath%radModeSAM-0.png
copy %up1% %skinpath%radModeAM-0.png
copy %up1% %skinpath%chkRX2NB2-0.png
copy %up1% %skinpath%chkRX2NB-0.png
copy %up1% %skinpath%chkRX2ANF-0.png
copy %up1% %skinpath%chkRX2NR-0.png
copy %up1% %skinpath%chkRX2BIN-0.png
copy %up1% %skinpath%chkRX2SR-0.png
copy %up1% %skinpath%chkSR-0.png
copy %up1% %skinpath%chkDSPNB2-0.png
copy %up1% %skinpath%chkNB-0.png
copy %up1% %skinpath%chkANF-0.png
copy %up1% %skinpath%chkNR-0.png
copy %up1% %skinpath%chkBIN-0.png
copy %up1% %skinpath%chkMUT-0.png
copy %up1% %skinpath%chkMON-0.png
copy %up1% %skinpath%chkTUN-0.png
copy %up1% %skinpath%chkMOX-0.png
copy %up1% %skinpath%chkDX-0.png
copy %up1% %skinpath%chkRXEQ-0.png
copy %up1% %skinpath%chkTXEQ-0.png
copy %up1% %skinpath%chkPhoneVAC-0.png
copy %up1% %skinpath%chkFMVAC-0.png
copy %up1% %skinpath%chkCPDR-0.png
copy %up1% %skinpath%chkVOX-0.png
copy %up1% %skinpath%chkNoiseGate-0.png
copy %up1% %skinpath%chkVACEnabled-0.png
copy %up1% %skinpath%chkCWVAC-0.png
copy %up1% %skinpath%chkRX2DisplayPeak-0.png
copy %up1% %skinpath%chkRX2DisplayAVG-0.png
copy %up1% %skinpath%chkEnableMultiRX-0.png
copy %up1% %skinpath%chkPanSwap-0.png
copy %up1% %skinpath%chkDisplayAVG-0.png
copy %up1% %skinpath%chkDisplayPeak-0.png
copy %up1% %skinpath%chkSquelch-0.png
copy %up1% %skinpath%chkRX2Squelch-0.png
copy %up1% %skinpath%chkSplitDisplay-0.png
copy %up1% %skinpath%btnZeroBeat-0.png
copy %up1% %skinpath%btnIFtoVFO-0.png
copy %up1% %skinpath%chkRX1Preamp-0.png
copy %up1% %skinpath%chkRX2Preamp-0.png
copy %up1% %skinpath%chkVFOSplit-0.png
copy %up1% %skinpath%btnVFOBtoA-0.png
copy %up1% %skinpath%btnVFOAtoB-0.png
copy %up1% %skinpath%btnVFOSwap-0.png
copy %up1% %skinpath%chkRX2Mute-0.png
copy %up1% %skinpath%chkFWCATU-0.png
copy %up1% %skinpath%chkX2TR-0.png
copy %up1% %skinpath%chkFWCATUBypass-0.png
copy %up1% %skinpath%btnDisplayPanCenter-0.png
copy %up1% %skinpath%chkFMTXSimplex-0.png
copy %up1% %skinpath%chkFMTXLow-0.png
copy %up1% %skinpath%chkFMTXHigh-0.png
copy %up1% %skinpath%chkFMTXRev-0.png
copy %up1% %skinpath%btnFMMemoryUp-0.png
copy %up1% %skinpath%btnFMMemoryDown-0.png
copy %up1% %skinpath%btnFMMemory-0.png
copy %up1% %skinpath%chkTNF-0.png
copy %up1% %skinpath%btnTNFAdd-0.png

copy %dn1% %skinpath%radRX2Filter1-1.png
copy %dn1% %skinpath%radRX2Filter2-1.png
copy %dn1% %skinpath%radRX2Filter3-1.png
copy %dn1% %skinpath%radRX2Filter4-1.png
copy %dn1% %skinpath%radRX2Filter5-1.png
copy %dn1% %skinpath%radRX2Filter6-1.png
copy %dn1% %skinpath%radRX2Filter7-1.png
copy %dn1% %skinpath%radRX2FilterVar1-1.png
copy %dn1% %skinpath%radRX2FilterVar2-1.png
copy %dn1% %skinpath%radRX2ModeAM-1.png
copy %dn1% %skinpath%radRX2ModeSAM-1.png
copy %dn1% %skinpath%radRX2ModeDSB-1.png
copy %dn1% %skinpath%radRX2ModeCWU-1.png
copy %dn1% %skinpath%radRX2ModeDIGU-1.png
copy %dn1% %skinpath%radRX2ModeDIGL-1.png
copy %dn1% %skinpath%radRX2ModeLSB-1.png
copy %dn1% %skinpath%radRX2ModeSPEC-1.png
copy %dn1% %skinpath%radRX2ModeDRM-1.png
copy %dn1% %skinpath%radRX2ModeFMN-1.png
copy %dn1% %skinpath%radRX2ModeUSB-1.png
copy %dn1% %skinpath%radRX2ModeCWL-1.png
copy %dn1% %skinpath%radFilter1-1.png
copy %dn1% %skinpath%radFilter2-1.png
copy %dn1% %skinpath%radFilter3-1.png
copy %dn1% %skinpath%radFilter4-1.png
copy %dn1% %skinpath%radFilter5-1.png
copy %dn1% %skinpath%radFilter6-1.png
copy %dn1% %skinpath%radFilter7-1.png
copy %dn1% %skinpath%radFilter8-1.png
copy %dn1% %skinpath%radFilter9-1.png
copy %dn1% %skinpath%radFilter10-1.png
copy %dn1% %skinpath%radFilterVar1-1.png
copy %dn1% %skinpath%radFilterVar2-1.png
copy %dn1% %skinpath%btnMemoryQuickSave-1.png
copy %dn1% %skinpath%btnFilterShiftReset-1.png
copy %dn1% %skinpath%btnMemoryQuickRestore-1.png

copy %dn1% %skinpath%btnBandHF-1.png
copy %dn1% %skinpath%radBand2-1.png
copy %dn1% %skinpath%radBand6-1.png
copy %dn1% %skinpath%radBand10-1.png
copy %dn1% %skinpath%radBand12-1.png
copy %dn1% %skinpath%radBand15-1.png
copy %dn1% %skinpath%radBand17-1.png
copy %dn1% %skinpath%radBand20-1.png
copy %dn1% %skinpath%radBand30-1.png
copy %dn1% %skinpath%radBand40-1.png
copy %dn1% %skinpath%radBand60-1.png
copy %dn1% %skinpath%radBand80-1.png
copy %dn1% %skinpath%radBand160-1.png
copy %dn1% %skinpath%radBandGEN-1.png
copy %dn1% %skinpath%radBandWWV-1.png

copy %dn1% %skinpath%btnBandVHF-1.png
copy %dn1% %skinpath%radBandVHF13-1.png
copy %dn1% %skinpath%radBandVHF12-1.png
copy %dn1% %skinpath%radBandVHF11-1.png
copy %dn1% %skinpath%radBandVHF10-1.png
copy %dn1% %skinpath%radBandVHF9-1.png
copy %dn1% %skinpath%radBandVHF8-1.png
copy %dn1% %skinpath%radBandVHF7-1.png
copy %dn1% %skinpath%radBandVHF6-1.png
copy %dn1% %skinpath%radBandVHF5-1.png
copy %dn1% %skinpath%radBandVHF4-1.png
copy %dn1% %skinpath%radBandVHF3-1.png
copy %dn1% %skinpath%radBandVHF2-1.png
copy %dn1% %skinpath%radBandVHF1-1.png
copy %dn1% %skinpath%radBandVHF0-1.png
copy %dn1% %skinpath%radModeLSB-1.png
copy %dn1% %skinpath%radModeCWL-1.png
copy %dn1% %skinpath%radModeUSB-1.png
copy %dn1% %skinpath%radModeFMN-1.png
copy %dn1% %skinpath%radModeDRM-1.png
copy %dn1% %skinpath%radModeSPEC-1.png
copy %dn1% %skinpath%radModeDIGL-1.png
copy %dn1% %skinpath%radModeDIGU-1.png
copy %dn1% %skinpath%radModeCWU-1.png
copy %dn1% %skinpath%radModeDSB-1.png
copy %dn1% %skinpath%radModeSAM-1.png
copy %dn1% %skinpath%radModeAM-1.png
copy %dn1% %skinpath%chkRX2NB2-1.png
copy %dn1% %skinpath%chkRX2NB-1.png
copy %dn1% %skinpath%chkRX2ANF-1.png
copy %dn1% %skinpath%chkRX2NR-1.png
copy %dn1% %skinpath%chkRX2BIN-1.png
copy %dn1% %skinpath%chkRX2SR-1.png
copy %dn1% %skinpath%chkSR-1.png
copy %dn1% %skinpath%chkDSPNB2-1.png
copy %dn1% %skinpath%chkNB-1.png
copy %dn1% %skinpath%chkANF-1.png
copy %dn1% %skinpath%chkNR-1.png
copy %dn1% %skinpath%chkBIN-1.png
copy %dn1% %skinpath%chkMUT-1.png
copy %dn1% %skinpath%chkMON-1.png
copy %dn1% %skinpath%chkTUN-1.png
copy %dn1% %skinpath%chkMOX-1.png
copy %dn1% %skinpath%chkDX-1.png
copy %dn1% %skinpath%chkRXEQ-1.png
copy %dn1% %skinpath%chkTXEQ-1.png
copy %dn1% %skinpath%chkPhoneVAC-1.png
copy %dn1% %skinpath%chkFMVAC-1.png
copy %dn1% %skinpath%chkCPDR-1.png
copy %dn1% %skinpath%chkVOX-1.png
copy %dn1% %skinpath%chkNoiseGate-1.png
copy %dn1% %skinpath%chkVACEnabled-1.png
copy %dn1% %skinpath%chkCWVAC-1.png
copy %dn1% %skinpath%chkRX2DisplayPeak-1.png
copy %dn1% %skinpath%chkRX2DisplayAVG-1.png
copy %dn1% %skinpath%chkEnableMultiRX-1.png
copy %dn1% %skinpath%chkPanSwap-1.png
copy %dn1% %skinpath%chkDisplayAVG-1.png
copy %dn1% %skinpath%chkDisplayPeak-1.png
copy %dn1% %skinpath%chkSquelch-1.png
copy %dn1% %skinpath%chkRX2Squelch-1.png
copy %dn1% %skinpath%chkSplitDisplay-1.png
copy %dn1% %skinpath%btnZeroBeat-1.png
copy %dn1% %skinpath%btnIFtoVFO-1.png
copy %dn1% %skinpath%chkRX1Preamp-1.png
copy %dn1% %skinpath%chkRX2Preamp-1.png
copy %dn1% %skinpath%chkVFOSplit-1.png
copy %dn1% %skinpath%btnVFOBtoA-1.png
copy %dn1% %skinpath%btnVFOAtoB-1.png
copy %dn1% %skinpath%btnVFOSwap-1.png
copy %dn1% %skinpath%chkRX2Mute-1.png
copy %dn1% %skinpath%chkFWCATU-1.png
copy %dn1% %skinpath%chkX2TR-1.png
copy %dn1% %skinpath%chkFWCATUBypass-1.png
copy %dn1% %skinpath%btnDisplayPanCenter-1.png
copy %dn1% %skinpath%chkFMTXSimplex-1.png
copy %dn1% %skinpath%chkFMTXLow-1.png
copy %dn1% %skinpath%chkFMTXHigh-1.png
copy %dn1% %skinpath%chkFMTXRev-1.png
copy %dn1% %skinpath%btnFMMemoryUp-1.png
copy %dn1% %skinpath%btnFMMemoryDown-1.png
copy %dn1% %skinpath%btnFMMemory-1.png
copy %dn1% %skinpath%chkTNF-1.png
copy %dn1% %skinpath%btnTNFAdd-1.png



