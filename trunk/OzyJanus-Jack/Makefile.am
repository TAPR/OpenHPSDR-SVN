# Top level 
#
#SUBDIRS = . object bin debug
SUBDIRS = . object bin
dist_doc_DATA = README

#CFLAGS=-DLINUX
#CFLAGS=-DLINUX -g

AM_CFLAGS = -O3 -I.
#INSTALL_DIR = ${prefix}/$(PACKAGE_NAME)
INSTALL_DIR = ~/SdrTest/

loadFW:
loadFW_SOURCE = ./src/loadFW.c
 
clean: 
	rm ./object/*.o
	rm ./bin/loadFW
	rm ./bin/loadFPGA
	rm ./bin/write_i2c
	rm ./bin/read_i2c
	rm ./bin/ozyjack
#	rm ./debug/loadFW
#	rm ./debug/loadFPGA
#	rm ./debug/write_i2c
#	rm ./debug/read_i2c
#	rm ./debug/ozyjack


install:
	-@if [ ! -d $(INSTALL_DIR) ]; then mkdir $(INSTALL_DIR); fi
	cp -u ./bin/loadFW $(INSTALL_DIR)
	cp -u ./bin/loadFPGA $(INSTALL_DIR)
	cp -u ./bin/write_i2c $(INSTALL_DIR)
	cp -u ./bin/read_i2c $(INSTALL_DIR)
	cp -u ./bin/ozyjack $(INSTALL_DIR)
	cp -u ./install/initozy $(INSTALL_DIR)
	cp -u ./install/Setspeed $(INSTALL_DIR)
	chmod 4755 $(INSTALL_DIR)*
	cp -u ./install/*.hex $(INSTALL_DIR)
	cp -u ./install/*.rbf $(INSTALL_DIR)

uninstall:
	rm -r $(INSTALL_DIR)

debug:
	./debug/make debug



