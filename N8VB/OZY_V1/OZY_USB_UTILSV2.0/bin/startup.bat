@ECHO OFF
load_firmware 0xfffe 0x0007 OZYV1FW08172006-1.hex
Sleep 2000
upload_FPGA 0xfffe 0x0007 gpio_control.rbf
pause
