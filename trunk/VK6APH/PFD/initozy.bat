load_firmware 0xfffe 0x0007 ozyv1fw08082006-1.hex
sleep 2000
upload_FPGA 0xfffe 0x0007 PFD_test.rbf
pause
