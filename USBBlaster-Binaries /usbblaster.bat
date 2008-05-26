load_firmwareV1.1.exe  0xfffe  0x7 ozyfw-sdr1k.hex
sleep 3000
upload_fpgaV1.1.exe 0xfffe 0x7 usb_blaster.rbf
sleep 3000
load_firmwareV1.1.exe  0xfffe  0x7 std.hex
