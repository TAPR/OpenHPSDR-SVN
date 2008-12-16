@echo off

"C:\Program Files\SDCC\bin\sdcc" -c .\src\board_specific.c -o .\obj\
"C:\Program Files\SDCC\bin\sdcc" -c .\src\eeprom_io.c -o .\obj\
"C:\Program Files\SDCC\bin\sdcc" -c .\src\fpga_load.c -o .\obj\
"C:\Program Files\SDCC\bin\sdcc" -c .\src\hpsdr_common.c -o .\obj\

rem "C:\Program Files\SDCC\bin\sdcc" -c .\src\hpsdr_main.c -o .\obj\
"C:\Program Files\SDCC\bin\sdcc" -D SDR1K_CONTROL -c .\src\hpsdr_main.c -o .\obj\
"C:\Program Files\SDCC\bin\sdcc" -D SDR1K_CONTROL -c .\src\sdr1kctl.c -o .\obj\

"C:\Program Files\SDCC\bin\sdcc" -c .\src\\lib\delay.c -o .\obj\
"C:\Program Files\SDCC\bin\sdcc" -c .\src\\lib\fx2utils.c -o .\obj\
"C:\Program Files\SDCC\bin\sdcc" -c .\src\\lib\i2c.c -o .\obj\
"C:\Program Files\SDCC\bin\sdcc" -c .\src\\lib\isr.c -o .\obj\
"C:\Program Files\SDCC\bin\sdcc" -c .\src\\lib\spi.c -o .\obj\
"C:\Program Files\SDCC\bin\sdcc" -c .\src\\lib\timer.c -o .\obj\
"C:\Program Files\SDCC\bin\sdcc" -c .\src\\lib\usb_common.c -o .\obj\

"C:\Program Files\SDCC\bin\asx8051" -olg .\src\asm\vectors.asm
"C:\Program Files\SDCC\bin\asx8051" -olg .\src\asm\_startup.asm
"C:\Program Files\SDCC\bin\asx8051" -olg .\src\asm\usb_descriptors_X1.asm

move ".\src\asm\vectors.rel" ".\Obj\vectors.rel"
move ".\src\asm\_startup.rel" ".\Obj\_startup.rel"
move ".\src\asm\usb_descriptors_X1.rel" ".\Obj\usb_descriptors_X1.rel"

move ".\src\asm\vectors.lst" ".\Obj\vectors.lst"
move ".\src\asm\_startup.lst" ".\Obj\_startup.lst"
move ".\src\asm\usb_descriptors_X1.lst" ".\Obj\usb_descriptors_X1.lst"

"C:\Program Files\SDCC\bin\sdcc" -c .\src\jrun.c -o .\obj\

pause
