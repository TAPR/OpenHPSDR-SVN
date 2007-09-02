

all: loadFW 

loadFW: loadFW.o USB.o
	gcc -o loadFW -lusb loadFW.o USB.o   
	
loadFW.o: src/loadFW.c 
	gcc -c src/loadFW.c
	
USB.o: src/USB.c 
	gcc -c src/USB.c

clean: 
	rm *.o loadFW 