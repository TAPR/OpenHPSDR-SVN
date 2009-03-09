rem
rem this loads code into the FX2 to make it look like an Altera USBblaster
rem
rem - Updated 4 March 2009 to use Vista capabile native code utils
loadFW.exe  0xfffe  0x7 ozyfw-sdr1k.hex
msecsleep 3000
loadFPGA.exe 0xfffe 0x7 usb_blaster.rbf
msecsleep 3000
loadFW.exe  0xfffe  0x7 std.hex
pause
