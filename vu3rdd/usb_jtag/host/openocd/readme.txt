OpenOCD driver
==============

The "openocd.patch" in this directory adds support for my adapter and Altera
USB-Blaster to Dominic Rath's OpenOCD ( http://openocd.berlios.de/web ).

The driver yet isn't utilizing the full bandwidth but only bit bang mode.
Because it is soo slow right now, it isn't really useful for anything. But
improving it is not complicated, really... The advantage of OpenOCD over
OpenWinCE is that there is active development and the internal JTAG low level
API is quite well designed with regard to support for USB adapters like
USB-Blaster.

To try it, get the OpenOCD source code, apply the patch, build and install it.

The "openocd.cfg" here is a configuration example, describing a chain with only
one device in it, with an IDCODE register length of 10 bits (e.g. Altera Cyclone):

  telnet_port 4444
  interface usb_blaster
  usb_blaster_vid_pid 0x09fb 0x6001
  jtag_device 10 0x155 0x3FF 6

Given this configuration, the example script "read_idcode" (when executed in a telnet
session to the server running on localhost at port 4444): would yield this:

  > script read_idcode
  scan
  0: idcode: 0x00000000 ir length 10, ir capture 0x155, ir mask 0x3ff, current instruction 0x3ff
  var idcode 32
  endstate rti
  irscan 0 6
  drscan 0 idcode
  var idcode
  idcode (1 fields):
  0x20830dd (/32)
  exit

Changes since initial release on 2006-09-03:
  - added this readme.txt
  - reorganized my project folder: diff is now created from Subversion repository
  - added examples read-idcode and openocd.cfg

