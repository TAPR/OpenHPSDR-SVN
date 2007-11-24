# Check for libusb support.                             -*- Autoconf -*-

# Copyright 2003 Free Software Foundation, Inc.

# This program is free software; you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation; either version 3, or (at your option)
# any later version.

# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.

# You should have received a copy of the GNU General Public License
# along with this program; if not, write to the Free Software
# Foundation, Inc., 51 Franklin Street, Boston, MA
# 02110-1301, USA.

AC_DEFUN([HPSDR_OZY_CHECKS],
[
	HPSDR_LIBUSB([],[passed=no;AC_MSG_RESULT([Unable to configure USB dependency.])])
	HPSDR_SET_FUSB_TECHNIQUE([],[passed=no;AC_MSG_RESULT([Unable to set fast USB technique.])])
	HPSDR_SDCC([2.4.0],[],[passed=no;AC_MSG_RESULT([Unable to find firmware compiler.])])

	AC_CONFIG_FILES([ \
            ozy/host/lib/std_paths.h \
        ])
])