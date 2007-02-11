@ECHO OFF
load_firmware 0xfffe 0x00ff QS1R_FirmwareV1.hex
Sleep 2000
upload_FPGA 0xfffe 0x00ff fpga_top.rbf
pause
