/*
 * HPSDR/OZY - High Performance Software Defined Radio, OZY Firmware
 *
 * Adapted from USRP firmware 07/10/2006 by Phil Covington N8VB
 *
 * Copyright 2003 Free Software Foundation, Inc.
 * 
 * This file is part of GNU Radio
 * 
 * GNU Radio is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 * 
 * GNU Radio is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with GNU Radio; see the file COPYING.  If not, write to
 * the Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */

extern xdata const char high_speed_device_descr[];
extern xdata const char high_speed_devqual_descr[];
extern xdata const char high_speed_config_descr[];

extern xdata const char full_speed_device_descr[];
extern xdata const char full_speed_devqual_descr[];
extern xdata const char full_speed_config_descr[];

extern xdata unsigned char nstring_descriptors;
extern xdata char * xdata string_descriptors[];

/*
 * We patch these locations with info read from the usrp config eeprom
 */
extern xdata char usb_desc_hw_rev_binary_patch_location_0[];
extern xdata char usb_desc_hw_rev_binary_patch_location_1[];
extern xdata char usb_desc_hw_rev_ascii_patch_location_0[];
extern xdata char usb_desc_serial_number_ascii[];
