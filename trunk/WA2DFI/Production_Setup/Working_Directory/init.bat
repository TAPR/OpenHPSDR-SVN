write_i2c 0xfffe 0x7 0x1a 0x1e 0x00 0x00 2
write_i2c 0xfffe 0x7 0x1a 0x12 0x01 0x00 2
write_i2c 0xfffe 0x7 0x1a 0x08 0x14 0x00 2
rem change 0x15 to 0x14 to drop mic input gain 20dB
write_i2c 0xfffe 0x7 0x1a 0x0c 0x00 0x00 2
write_i2c 0xfffe 0x7 0x1a 0x0e 0x02 0x00 2
write_i2c 0xfffe 0x7 0x1a 0x10 0x00 0x00 2
write_i2c 0xfffe 0x7 0x1a 0x0a 0x00 0x00 2
