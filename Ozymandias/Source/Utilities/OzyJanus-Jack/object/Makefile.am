# Object directory Make file

loadFW_SOURCE = ../src/loadFW.c
loadFPGA_SOURCE = ../src/loadFPGA.c ../src/USB.c ../src/OZY.c
write_i2c_SOURCE = ../src/write_i2c.c ../src/USB.c ../src/OZY.c
read_i2c_SOURCE = ../src/read_i2c.c ../src/USB.c ../src/OZY.c
ozyjack_OB = ../object/ozyjack.o ../object/callback-thread.o ../object/fifo.o ../object/globals.o \
	../object/io-thread.o ../object/OzyIO.o  ../object/PowerSDR-Interface.o ../object/JackClient.o ../object/resampleFOJ.o


all: loadFW.o loadFPGA.o write_i2c.o read_i2c.o OZY.o USB.o ozyjack.o callback-thread.o fifo.o globals.o\
	io-thread.o OzyIO.o PowerSDR-Interface.o JackClient.o resampleFOJ.o

loadFW.o: $(loadFW_SOURCE)
	 gcc -c $(loadFW_SOURCE)

loadFPGA.o: $(loadFPGA_SOURCE)
	 gcc -c $(loadFPGA_SOURCE)	

write_i2c.o: $(write_i2c_SOURCE)
	gcc -c $(write_i2c_SOURCE)

read_i2c.o: $(read_i2c_SOURCE)
	gcc -c $(read_i2c_SOURCE)
	
OZY.o: ../src/OZY.c
	gcc -c ../src/OZY.c 
	
USB.o: ../src/USB.c 
	gcc -c ../src/USB.c

ozyjack.o:  ../src/ozyjack.c
	gcc -c ../src/ozyjack.c

callback-thread.o:  ../src/callback-thread.c
	gcc -c -DLINUX ../src/callback-thread.c

fifo.o:  ../src/fifo.c
	gcc -c -DLINUX ../src/fifo.c

globals.o:  ../src/globals.c
	gcc -c -DLINUX ../src/globals.c

io-thread.o:  ../src/io-thread.c
	gcc -c -DLINUX ../src/io-thread.c

OzyIO.o:  ../src/OzyIO.c
	gcc -c -DLINUX ../src/OzyIO.c

PowerSDR-Interface.o:  ../src/PowerSDR-Interface.c
	gcc -c -DLINUX ../src/PowerSDR-Interface.c

JackClient.o:  ../src/JackClient.c
	gcc -c -DLINUX ../src/JackClient.c

resampleFOJ.o:  ../src/resampleFOJ.c
	gcc -c -DLINUX ../src/resampleFOJ.c





