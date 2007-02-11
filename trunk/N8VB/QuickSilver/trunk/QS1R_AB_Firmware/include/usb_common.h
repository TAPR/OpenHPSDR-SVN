/*
 * QuickSilver QS1R Firmware
 *
 * Adapted from USRP firmware 12/07/2006 by Phil Covington N8VB
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

#ifndef _USB_COMMON_H_
#define _USB_COMMON_H_

extern volatile bit _usb_got_SUDAV;

// Provided by user application to handle VENDOR commands.
// returns non-zero if it handled the command.
unsigned char app_vendor_cmd (void);

void usb_install_handlers (void);
void usb_handle_setup_packet (void);
void usb_handle_set_feature(void);
void usb_handle_clear_feature(void);
void usb_handle_get_status(void);
void usb_handle_get_descriptor(void);
void usb_handle_IN_std_requests(void);
void usb_handle_OUT_std_requests(void);
void usb_handle_std_requests(void);

#define usb_setup_packet_avail()	_usb_got_SUDAV

#endif /* _USB_COMMON_H_ */
