@echo off

"C:\Program Files\SDCC\bin\sdcc" --no-xinit-opt --xram-loc 0x2000 --xram-size 0x2000 --code-size 0x2000 -Wl -b" USBDESCSEG = 0xE000" -o"Ozy_firmware.hex"  ./obj/vectors.rel ./obj/_startup.rel ./obj/usb_descriptors_X1.rel   ./obj/delay.rel ./obj/fx2utils.rel ./obj/i2c.rel ./obj/isr.rel ./obj/spi.rel ./obj/timer.rel ./obj/usb_common.rel  ./obj/board_specific.rel ./obj/eeprom_io.rel ./obj/fpga_load.rel ./obj/hpsdr_common.rel ./obj/hpsdr_main.rel ./obj/sdr1kctl.rel ./obj/jrun.rel
copy Ozy_firmware.hex C:\Proj\DSP\Mercury\Try\Ozy_firmware.hex
pause
