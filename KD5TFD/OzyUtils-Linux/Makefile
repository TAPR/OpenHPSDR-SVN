

all: loadFW loadFPGA

loadFW: loadFW.o USB.o
	gcc -o loadFW -lusb loadFW.o USB.o   
	
loadFPGA: loadFPGA.o USB.o OZY.o 
	gcc -o loadFPGA -lusb loadFPGA.o USB.o OZY.o
	
loadFW.o: src/loadFW.c 
	gcc -c src/loadFW.c
	
loadFPGA.o: src/loadFPGA.c 
	gcc -c src/loadFPGA.c
	
OZY.o: src/OZY.c
	gcc -c src/OZY.c 
	
USB.o: src/USB.c 
	gcc -c src/USB.c

clean: 
	rm *.o loadFW loadFPGA