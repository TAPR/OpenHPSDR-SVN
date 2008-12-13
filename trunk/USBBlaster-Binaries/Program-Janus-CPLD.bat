load_firmwareV1.1.exe  0xfffe  0x7 ozyfw-sdr1k.hex
sleep 1000
upload_fpgaV1.1.exe 0xfffe 0x7 usb_blaster.rbf
sleep 1000
load_firmwareV1.1.exe  0xfffe  0x7 std.hex
sleep 2000
quartus_jli -a program janus.jam -l
pause
