#!/bin/bash
#
# Point a Desktop launcher at this shell script rather than ghpsdr.
# It will set the current diectory correctly.
#
# You can also use icon.png as the icon for the launcher.
#
cd `dirname $0`
pwd > ghpsdr.log 2>&1
./ghpsdr >> ghpsdr.log 2>&1
