# Bin dir make

bin_PROGRAMS = loadFW loadFPGA write_i2c read_i2c ozyjack


loadFW_OB = ../object/loadFW.o ../object/USB.o
loadFPGA_OB = ../object/loadFPGA.o ../object/USB.o ../object/OZY.o
write_i2c_OB = ../object/write_i2c.o ../object/USB.o ../object/OZY.o
read_i2c_OB = ../object/read_i2c.o ../object/USB.o ../object/OZY.o
ozyjack_OB = ../object/ozyjack.o ../object/callback-thread.o ../object/fifo.o ../object/globals.o \
	../object/io-thread.o ../object/OzyIO.o  ../object/PowerSDR-Interface.o ../object/JackClient.o ../object/resampleFOJ.o

#all: loadFW loadFPGA write_i2c ozyjack
all: $(bin_PROGRAMS)

loadFW: $(loadFW_OB)
	gcc -o loadFW $(LIBS) $(loadFW_OB)   
	
loadFPGA: $(loadFPGA_OB) 
	gcc -o loadFPGA $(LIBS) $(loadFPGA_OB)

write_i2c: $(write_i2c_OB)
	gcc -o write_i2c $(LIBS) $(write_i2c_OB)

read_i2c: $(read_i2c_OB)
	gcc -o read_i2c $(LIBS) $(read_i2c_OB)

ozyjack: $(ozyjack_OB)
	gcc -o ozyjack $(LIBS) $(ozyjack_OB)
	

