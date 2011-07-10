#
# alextest Makefile (Linux)
#
CC=gcc
LINK=gcc
OPTIONS=-g
INCLUDES=-I. -I/usr/include/gtk-2.0 -I/usr/lib/gtk-2.0/include\
         -I/usr/include/gdk-pixbuf-2.0\
         -I/usr/include/atk-1.0 -I/usr/include/cairo\
         -I/usr/include/pango-1.0 -I/usr/include/glib-2.0\
         -I/usr/lib/glib-2.0/include -I/usr/include/pixman-1\
         -I/usr/include/freetype2 -I/usr/include/libpng12 \
	 -I/usr/local/include/libusb-1.0

LIBS=-L. -lDttSP -lpthread -lusb-1.0\
     -lgtk-x11-2.0 -lgdk-x11-2.0 -latk-1.0 -lpangoft2-1.0 -lgdk_pixbuf-2.0\
     -lm -lpangocairo-1.0 -lgio-2.0 -lcairo -lpango-1.0 -lfreetype -lz\
     -lfontconfig -lgobject-2.0 -lgmodule-2.0 -lglib-2.0 -lfftw3f

COMPILE=$(CC) $(INCLUDES)

SOURCES= agc.c audio.c band.c bandscope.c bandscope_control.c bandscope_update.c command.c\
         display.c filter.c frequency.c libusbio.c main.c meter.c\
         meter_update.c mode.c ozy_buffers.c ozy.c ozy_ringbuffer.c\
         preamp.c property.c receiver.c\
         setup.c display_setup.c hpsdr_setup.c xvtr_setup.c\
         soundcard.c spectrum_buffers.c spectrum.c spectrum_update.c util.c vfo.c volume.c\
         sinewave.c transmit.c\
         audiostream.c\
         subrx.c\
         metis.c\
         cw.c\
         alex_rx_test.c alex_tx_test.c test.c

HEADERS= agc.h alex_rx_test.h alex_tx_test.h audio.h audiostream.h band.h bandscope_control.h\
         bandscope.h bandscope_update.h bandstack.h command.h cw.h display.h display_setup.h\
         dttsp.h filter.h frequency.h hpsdr_setup.h libusbio.h main.h meter.h meter_update.h\
         metis.h mode.h ozy_buffers.h ozy.h ozy_ringbuffer.h preamp.h property.h receiver.h\
         screensize.h setup.h sinewave.h soundcard.h spectrum_buffers.h spectrum.h\
         spectrum_update.h subrx.h test.h transmit.h util.h version.h vfo.h volume.h\
         xvtr.h xvtr_setup.h

OBJS= agc.o audio.o band.o bandscope.o bandscope_control.o bandscope_update.o command.o\
      display.o filter.o frequency.o libusbio.o main.o meter.o\
      meter_update.o mode.o ozy_buffers.o ozy.o ozy_ringbuffer.o\
      preamp.o property.o receiver.o\
      setup.o display_setup.o hpsdr_setup.o xvtr_setup.o\
      soundcard.o spectrum_buffers.o spectrum.o spectrum_update.o util.o vfo.o volume.o\
      sinewave.o transmit.o\
      audiostream.o\
      subrx.o\
      metis.o\
      cw.o\
      alex_rx_test.o alex_tx_test.o test.o

PROGRAM=bin/alextest

all: $(PROGRAM)

bin/alextest: $(OBJS)
	$(LINK) -o bin/alextest $(OBJS) $(DTTSP) $(FFTW) $(LIBS)

.c.o:
	$(COMPILE) $(OPTIONS) -c -o $@ $<

clean:
	-rm -f *.o
	-rm -f bin/alextest

