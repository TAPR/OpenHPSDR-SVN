@ECHO OFF
load_firmware 0xfffe 0x0007 OZYV1FW08282006-1.hex
Sleep 2000
upload_FPGA 0xfffe 0x0007 ozy_eval.rbf
pause
