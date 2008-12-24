rem
rem this loads code into the FX2 to make it look like an Altera USBblaster
rem
load_firmwareV1.1.exe  0xfffe  0x7 ozyfw-sdr1k.hex
sleep 1000
upload_fpgaV1.1.exe 0xfffe 0x7 usb_blaster.rbf
sleep 1000
load_firmwareV1.1.exe  0xfffe  0x7 std.hex
pause
