#!/usr/bin/python
#
# Copyright 2006 Free Software Foundation, Inc.
# 
# Author: Ramakrishnan Muthukrishnan <vu3rdd@gmail.com>
# 
# GNU Radio is free software; you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation; either version 2, or (at your option)
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
# Takes a CSV file (which maps inputs/outputs to actual PINs) as input
# and generates the qsf file. Also scans the project directory and adds
# verilog files 
# 

import csv

# open the CSV file containing pin assignments
reader = csv.reader (open ("blinkity.csv", "rb"))

# print equiv QSF PIN assignment lines
for c1, c2 in reader:
  print "set_location_assignment " + c2 + " -to " + c1

