#!/usr/bin/python
#
# Copyright 2007 Free Software Foundation, Inc.
# 
# Author: Ramakrishnan Muthukrishnan <vu3rdd@gmail.com>
# 
# This program is free software; you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation; either version 2, or (at your option)
# any later version.
# 
# csv2qsf is distributed in the hope that it will be useful,
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
# usage: csv2qsf csv_file qsf_file
#
import csv
import sys

def write_pins (csv_file, qsf_file):
  # open the CSV file containing pin assignments
  reader = csv.reader (csv_file)

  # print equiv QSF PIN assignment lines
  for c1, c2 in reader:
    qsf_file.write ("set_location_assignment " + c2 + " -to " + c1 + "\n")

  return 0


if __name__ == '__main__':
  # open CSV file which describes pin assignments
  csv_file = open (sys.argv[1], "rb")

  # open qsf file in append mode
  qsf_file = open (sys.argv[2], "a") 

  write_pins (csv_file, qsf_file)
