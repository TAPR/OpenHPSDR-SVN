#
# ghpsdr Makefile (Linux)
#
CC=gcc
LINK=gcc
OPTIONS=-g
GTKINCLUDES=`pkg-config --cflags gtk+-2.0`
GDKINCLUDES=`pkg-config --cflags gdk-2.0`
USBINCLUDES=`pkg-config --cflags libusb-1.0`
INCLUDES=-I. \
	 $(GTKINCLUDES) \
	 $(GDKINCLUDES) \
         $(USBINCLUDES) 
#	 -I/usr/local/include -I/usr/local/include/libusb-1.0

GTKLIBS=`pkg-config --libs gtk+-2.0`
GDKLIBS=`pkg-config --libs gdk-2.0`
USBLIBS=`pkg-config --libs libusb-1.0`

LIBS=-lm -lpthread -lusb-1.0\
     $(GTKLIBS)\
     $(GDKLIBS)\
     $(USBLIBS)\
     -L/usr/local/lib -lwdsp -lfftw3f

COMPILE=$(CC) $(INCLUDES)

PROGRAM=bin/ghpsdr

SOURCES= agc.c audio.c band.c bandscope.c bandscope_control.c bandscope_update.c command.c\
         display.c filter.c frequency.c libusbio.c main.c meter.c\
         meter_update.c mode.c ozy_buffers.c ozy.c ozy_ringbuffer.c\
         preamp.c property.c receiver.c\
         setup.c display_setup.c hpsdr_setup.c xvtr_setup.c alex_setup.c\
         soundcard.c spectrum_buffers.c spectrum.c spectrum_update.c util.c vfo.c volume.c\
         sinewave.c transmit.c\
         audiostream.c\
         subrx.c\
         metis.c\
         cw.c\
         discovery.c\
         controller.c

HEADERS= agc.h audio.h audiostream.h band.h bandscope_control.h\
         bandscope.h bandscope_update.h bandstack.h command.h cw.h display.h display_setup.h\
         filter.h frequency.h hpsdr_setup.h libusbio.h main.h meter.h meter_update.h\
         metis.h mode.h ozy_buffers.h ozy.h ozy_ringbuffer.h preamp.h property.h receiver.h\
         screensize.h setup.h sinewave.h soundcard.h spectrum_buffers.h spectrum.h\
         spectrum_update.h subrx.h transmit.h util.h version.h vfo.h volume.h\
         xvtr.h xvtr_setup.h \
         discovered.h discovery.h\
         controller.h

OBJS= agc.o audio.o band.o bandscope.o bandscope_control.o bandscope_update.o command.o\
      display.o filter.o frequency.o libusbio.o main.o meter.o\
      meter_update.o mode.o ozy_buffers.o ozy.o ozy_ringbuffer.o\
      preamp.o property.o receiver.o\
      setup.o display_setup.o hpsdr_setup.o xvtr_setup.o alex_setup.o\
      soundcard.o spectrum_buffers.o spectrum.o spectrum_update.o util.o vfo.o volume.o\
      sinewave.o transmit.o\
      audiostream.o\
      subrx.o\
      metis.o\
      cw.o\
      discovery.o\
      controller.o


all: $(PROGRAM) $(HEADERS) $(SOURCES)

bin/ghpsdr: $(OBJS)
	$(LINK) -o bin/ghpsdr $(OBJS) $(DTTSP) $(FFTW) $(LIBS)

.c.o:
	$(COMPILE) $(OPTIONS) -c -o $@ $<


clean:
	-rm -f *.o
	-rm -f bin/ghpsdr


