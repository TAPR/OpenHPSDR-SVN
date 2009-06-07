#
# ghpsdr Makefile (Linux)
#
CC=gcc
LINK=gcc
OPTIONS=-g
INCLUDES=-I. -I/usr/include/gtk-2.0 -I/usr/lib/gtk-2.0/include\
         -I/usr/include/atk-1.0 -I/usr/include/cairo\
         -I/usr/include/pango-1.0 -I/usr/include/glib-2.0\
         -I/usr/lib/glib-2.0/include -I/usr/include/pixman-1\
         -I/usr/include/freetype2 -I/usr/include/libpng12 \
	 -I/usr/local/include/libusb-1.0
LIBS=-lpthread -lusb-1.0\
     -lgtk-x11-2.0 -lgdk-x11-2.0 -latk-1.0 -lpangoft2-1.0 -lgdk_pixbuf-2.0\
     -lm -lpangocairo-1.0 -lgio-2.0 -lcairo -lpango-1.0 -lfreetype -lz\
     -lfontconfig -lgobject-2.0 -lgmodule-2.0 -lglib-2.0 -lusb-1.0

FFTW=/usr/lib/libfftw3f.a
DTTSP=libDttSP.a
COMPILE=$(CC) $(INCLUDES)

SOURCES= agc.c audio.c band.c bandscope.c bandscope_control.c bandscope_update.c command.c\
         clock122_88.c clock10.c\
         display.c filter.c frequency.c libusbio.c main.c hardware.c meter.c\
         meter_update.c mode.c ozy_buffers.c ozy.c ozy_ringbuffer.c\
         property.c receiver.c setup.c soundcard.c spectrum_buffers.c spectrum.c spectrum_update.c util.c vfo.c volume.c

OBJS= agc.o audio.o band.o bandscope.o bandscope_control.o bandscope_update.o command.o\
      clock122_88.o clock10.o\
      display.o filter.o frequency.o libusbio.o main.o hardware.o meter.o\
      meter_update.o mode.o ozy_buffers.o ozy.o ozy_ringbuffer.o\
      property.o receiver.o setup.o soundcard.o spectrum_buffers.o spectrum.o spectrum_update.o util.o vfo.o volume.o

PROGRAM=ghpsdr

all: ghpsdr

ghpsdr: $(OBJS)
	$(LINK) -o ghpsdr $(OBJS) $(DTTSP) $(FFTW) $(LIBS)

.c.o:
	$(COMPILE) $(OPTIONS) -c -o $@ $<

clean:
	-rm -f *.o
	-rm -f ghpsdr

