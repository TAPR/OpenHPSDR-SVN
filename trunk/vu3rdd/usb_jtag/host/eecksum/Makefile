EEPROM_C=../../device/fx2/eeprom.c.dist

compute: eecksum.c $(EEPROM_C)
	ln -sf $(EEPROM_C) eeprom.c
	gcc -Dxdata= "-DBYTE=unsigned char" -o eecksum eecksum.c eeprom.c
	./eecksum

clean:
	-rm -f eecksum eeprom.c

distclean: clean
	
