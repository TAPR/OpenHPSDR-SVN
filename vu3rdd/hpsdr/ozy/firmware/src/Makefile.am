# High Performance Software Defined Radio (hpsdr.org)
# Modified for HPSDR project by Ramakrishnan Muthukrishnan <vu3rdd@gmail.com>
# on Nov 21st 2007
#
# Original source part of GNU Radio USRP project.
#
# Copyright 2003,2006 Free Software Foundation, Inc.
# 
# This file is part of GNU Radio
# 
# GNU Radio is free software; you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation; either version 3, or (at your option)
# any later version.
# 
# GNU Radio is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
# 
# You should have received a copy of the GNU General Public License
# along with GNU Radio; see the file COPYING.  If not, write to
# the Free Software Foundation, Inc., 51 Franklin Street,
# Boston, MA 02110-1301, USA.
# 

firmware1dir = $(prefix)/share/hpsdr/ozy_v1
firmware1_DATA = std.ihx

EXTRA_DIST =			\
	_startup.a51		\
	board_specific.c	\
	eeprom_io.c		\
	fpga_load.c		\
	hpsdr_common.c          \
	hpsdr_main.c            \
	sdr1kctl.c              \
	usb_descriptors.a51	\
	vectors.a51		


DEFINES=-DSDR1K_CONTROL
INCLUDES=-I$(top_srcdir)/include

# with EA = 0, the FX2 implements a portion of the 8051 "external memory"
# on chip.  This memory is mapped like this:
#
# The bottom 8K of memory (0x0000 - 0x1fff) is used for both data and
# code accesses.  There's also 512 bytes for data only from 0xe000 - 0xe1ff.
#
# We tell the linker to start the xdata segment at 0x1800, 6K up from
# the bottom.

#MEMOPTS = --code-loc 0x0000 --code-size 0x1800 --xram-loc 0x1800 --xram-size 0x0800 \
# -Wl '-b USBDESCSEG = 0xE000'
MEMOPTS = --code-loc 0x0000 --code-size 0x2000 --xram-loc 0x2000 --xram-size 0x2000 --no-xinit-opt \
 -Wl '-b USBDESCSEG = 0xE000'

LIBOPTS = -L ../lib libfx2.lib
LIBDEP = ../lib/libfx2.lib

LINKOPTS = $(MEMOPTS) $(LIBOPTS)

EXECUTABLES = 			\
	std.ihx			

STARTUP = _startup.rel

#noinst_SCRIPTS = 		\
#	burn-usrp2-eeprom	\
#	burn-usrp4-eeprom

.c.rel:
	$(XCC) $(INCLUDES) $(DEFINES) \
		-c -o $@ `test -f '$<' || echo '$(srcdir)/'`$<

.a51.rel:
	test -f `basename '$<'` || $(LN_S) '$<' .
	test -f ../common/`basename '$<'` -o \
		\! -f `dirname '$<'`/../common/`basename '$<'` \
		|| $(LN_S) `dirname '$<'`/../common/`basename '$<'` ../common/`basename '$<'`
	$(XAS) `basename '$<'`


#EEPROM_BOOT_OBJS = eeprom_boot.rel eeprom_init.rel $(STARTUP)

#eeprom_boot.ihx: $(EEPROM_BOOT_OBJS) $(LIBDEP)
#	$(XCC) $(LINKOPTS) -o $@ $(EEPROM_BOOT_OBJS)

#burn-usrp2-eeprom: eeprom_boot.ihx
#	$(PYTHON) $(srcdir)/../common/build_eeprom.py -r2 eeprom_boot.ihx > $@
#	chmod +x $@

#burn-usrp4-eeprom: eeprom_boot.ihx
#	$(PYTHON) $(srcdir)/../common/build_eeprom.py -r4 eeprom_boot.ihx > $@
#	chmod +x $@

#BLINK_LEDS_OBJS = blink_leds.rel usrp_common.rel board_specific.rel spi.rel $(STARTUP)

#blink_leds.ihx: $(BLINK_LEDS_OBJS) $(LIBDEP)
#	$(XCC) $(LINKOPTS) -o $@ $(BLINK_LEDS_OBJS)


#CHECK_MDELAY_OBJS = check_mdelay.rel usrp_common.rel board_specific.rel spi.rel $(STARTUP)

#check_mdelay.ihx: $(CHECK_MDELAY_OBJS) $(LIBDEP)
#	$(XCC) $(LINKOPTS) -o $@ $(CHECK_MDELAY_OBJS)


#CHECK_UDELAY_OBJS = check_udelay.rel usrp_common.rel board_specific.rel spi.rel $(STARTUP)

#check_udelay.ihx: $(CHECK_UDELAY_OBJS) $(LIBDEP)
#	$(XCC) $(LINKOPTS) -o $@ $(CHECK_UDELAY_OBJS)


HPSDR_OZY_OBJS = \
	vectors.rel 						\
	hpsdr_main.rel hpsdr_common.rel board_specific.rel	\
	fpga_load.rel sdr1kctl.rel \
	usb_descriptors.rel eeprom_io.rel $(STARTUP)

std.ihx: $(HPSDR_OZY_OBJS) $(LIBDEP)
	$(XCC) $(LINKOPTS) -o $@ $(HPSDR_OZY_OBJS)

CLEANFILES = 		\
	*.ihx *.lnk *.lst *.map *.mem *.rel *.rst *.sym *.asm *.lib

DISTCLEANFILES = 	\
	*.ihx *.lnk *.lst *.map *.mem *.rel *.rst *.sym *.asm *.lib

# build gpif stuff

#all: usrp_gpif.c

#usrp_gpif.c usrp_gpif_inline.h : gpif.c
#	srcdir=$(srcdir) $(PYTHON) $(srcdir)/edit-gpif $(srcdir)/gpif.c usrp_gpif.c usrp_gpif_inline.h


# dependencies

#usrp_main.rel: usrp_gpif_inline.h
#usrp_main.rel: fpga.h usrp_common.h ../../include/usrp_commands.h usrp_gpif_inline.h  ../../include/usrp_config.h usrp_rev2_regs.h ../../include/fx2regs.h
#usrp_common.rel: usrp_common.h ../../include/usrp_commands.h ../../include/usrp_config.h usrp_rev2_regs.h ../../include/fx2regs.h
#fpga.rel: usrp_common.h ../../include/usrp_commands.h fpga.h ../../include/usrp_config.h usrp_rev2_regs.h ../../include/fx2regs.h
#init_gpif.rel: usrp_common.h ../../include/usrp_config.h usrp_rev2_regs.h ../../include/fx2regs.h
