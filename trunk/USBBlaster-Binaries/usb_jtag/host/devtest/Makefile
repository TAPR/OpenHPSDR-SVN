CC=gcc
LDFLAGS=-lftdi

default: devtest
	./devtest

devtest: devtest.o
devtest.o: devtest.c

distclean: clean
clean:
	rm -f devtest.o devtest


