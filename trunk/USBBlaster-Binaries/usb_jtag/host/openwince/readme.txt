openwince jtag driver
=====================

The "openwince.patch" in this directory adds support for my adapter and Altera
USB-Blaster to the jtag tools from the openwince project ( http://openwince.sf.net ).

To try it, get the openwince "jtag" and "include" modules from cvs.openwince.sf.net,
apply the patch, build and install it (if gcc 4.x doesn't work, try an older gcc 3.x).

NB: The patch was created as a diff against openwince CVS HEAD as of 2007-01-28.

To access the adapter, a new parallel port driver "ftdi" had to be added, based
on libftdi. Therefore, in addition to openwince source code, you need libftdi
with headers installed (the package "libftdi-dev" as included with Debian Sarge
(3.1) and Ubuntu 6.x works well).

You can connect to the adapter with a jtag command like "cable ftdi * UsbBlaster"
or with a non-standard specific vendor/product id and optional serial number, e.g.
"cable ftdi 09fb:6001 UsbBlaster".

But beware, it is slower slower than ByteBlaster driver: "detect" with a single
EP1C12 in the chain takes 2.3 seconds (as opposed to 0.17 s with ByteBlaster).
This is probably due to the fact that bit banging over USB has a much higher
overhead than with a directly attached parallel port. However, I tried to
circumvent this by adding some buffering and a "transfer()" function that
accumulates several bit changes into a single transfer for the USB Blaster. The
difference shouldn't be so big when doing larger transfers. And well, there may
still be some flaw in the code that puts the brake on the transfers... The
addition of transfer() also affects every other cable driver, but adding it is
as easy as adding "generic_transfer" in your cable_driver_t. I even noticed a
slight speedup with the byteblaster driver after adding it there!

I won't spend too much time on further development of this driver, as I'm not
really sure whether using this adapter in an application designed for direct
parallel port access is the right way to go (and I'm not sure whether
openwince-jtag is still alive anyway?!).

Changes since initial release on 2006-05-01:
  - added this readme.txt
  - reorganized my project folder: diff is now created from Subversion repository
  - swapped Vendor/Product ID in command line (thanks to Arnim Laeuger for the report)

