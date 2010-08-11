PennyWSPR - 10 Aug 2010

This directory contains the PennyWSPR code.  PennyWSPR is Verilig code
that implements a WSPR beacon using a Penelope board.  To use this
code you'll need a Penelope board, Excalibur board, Atlas and a power
supply.  (One can modify the code to use the 10 Mhz ref on Penny).  As
distributed, the code is setup to run a single tone beacon on 40 and
20 meters.  It will beacon as:

    pe0nny AA00 23

If you want to try this code as is, attach a dummy load to Penny and load
PennyWSPR.pof into Penny.  Power on Penny at the top of an even minute.

To use this code on the air, it needs to be built with the desired
call sign and grid square locator to beacon.  To do this you will need
to get GenWSPR.exe from:  http://www.g4jnt.com/GENWSPR.EXE and install
ooRexx from:  http://www.oorexx.org/.  Then from a command line run

    rexx makemif.rexx callsign locator powerlevel_in_dbm

And then regenerate PennyWSPR.pof by building the project in Quartus.

There are two variants of the beacon code distributed.  The first is a
single tone system where Penelope will transmit on one band at a time,
and will cycle through the desired bands.  This is probably the most
practical way to run a beacon as it puts the maximum power on to one
band at a time.  This code is in PennyWSPR.singletone.v

The second variant is a multitone variant, where it becaons on three
bandds (40, 30 and 20) concurrently.  This is interesting as a gee
whiz demonstration of something one can do with a wideband DAC but is
of limited practical value since it will reduce the power transmitted
on each band since all the available power from Penelope is now
distributed across mutiple bands.

To change the version of code you're using, copy PennyWSPR.multitone.v
or PennyWSPR.singletone.v to PennyWSPR.v and rebuild the project.

For information on WSPR see: http://www.physics.princeton.edu/pulsar/K1JT/wspr.html

For a good write up on the WSPR coding process see: http://www.g4jnt.com/Coding/WSPR_Coding_Process.pdf

Licensing

This code is licensed under the GNU GPL v2.

Acknowledgments

This code is derived from the standard Penelope code and a Cordic from
Alex Shovkplyas (VE3NEA) that is based on algorithm by Darrell Hamrmon
and modified by Cathy Moss.  Phil Harman (VK6APH) adn Kirk Wedman
(KD7IRS) are the authors of the original Penelope code.  My thanks to
these folks for releasing their work under the GPL.

Bill Tracey (KD5TFD)
