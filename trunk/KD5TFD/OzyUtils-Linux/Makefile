

all: loadFW loadFPGA write_i2c

loadFW: loadFW.o USB.o
	gcc -o loadFW -lusb loadFW.o USB.o   
	
loadFPGA: loadFPGA.o USB.o OZY.o 
	gcc -o loadFPGA -lusb loadFPGA.o USB.o OZY.o

write_i2c: write_i2c.o USB.o OZY.o
	gcc -o write_i2c -lusb write_i2c.o USB.o OZY.o
	
loadFW.o: src/loadFW.c 
	gcc -c src/loadFW.c
	
loadFPGA.o: src/loadFPGA.c 
	gcc -c src/loadFPGA.c

write_i2c.o: src/write_i2c.c
	gcc -c src/write_i2c.c
	
OZY.o: src/OZY.c
	gcc -c src/OZY.c 
	
USB.o: src/USB.c 
	gcc -c src/USB.c

clean: 
	rm *.o loadFW loadFPGA write_i2c
