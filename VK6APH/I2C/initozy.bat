rem load_firmware 0xfffe 0x0007 ozyv1fw08282006-1.hex
load_firmware 0xfffe 0x0007 ozyv1fw08082006-1.hex
sleep 2000
upload_FPGA 0xfffe 0x0007 I2C.rbf
pause
set_I2CSPEED 0xfffe 0x7 0x00

