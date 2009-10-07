all:
	date +'#define VERSION "%Y-%m-%d %H:%M:%S"' > version.h
	gcc -o ../../bin/ghpsdrsvr *.c -I/usr/local/include/libusb-1.0 -lpthread -lusb-1.0
