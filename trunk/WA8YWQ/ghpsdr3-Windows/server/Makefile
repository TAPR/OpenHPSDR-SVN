#
# Makefile for Linux version of ghpsdr3
# modified by Dave Larsen, KV0S
#
CC            = gcc
CFLAGS        = 
LIBS          = -lpthread -lusb-1.0
TARGET 	=	server
SOURCES = 	bandscope.c  \
		listener.c \
		receiver.c \
		util.c \
		client.c \
		ozy.c \
		ozyio.c \
		server.c
OBJECTS =       bandscope.o  \
                listener.o \
                receiver.o \
                util.o \
                client.o \
                ozy.o \
                ozyio.o \
                server.o

.c.o:
	$(CC) -c $(CFLAGS) $(INCPATH) -o "$@" "$<"


all: Makefile $(TARGET)

$(TARGET):  $(OBJECTS)
	$(CC) -o ./bin/$(TARGET) $(LIBS) $(OBJECTS) 

