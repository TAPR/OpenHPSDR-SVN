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

#ifndef _HPSDR_INTERFACES_H_
#define _HPSDR_INTERFACES_H_

/*
 * Interface 0 contains only ep0 and is used for command and status.
 * Interface 1 is the Tx path and it uses ep2 OUT BULK.
 * Interface 2 is the Rx path and it uses ep6 IN BULK.
 */
 
#define	HPSDR_CMD_INTERFACE		0
#define	HPSDR_CMD_ALTINTERFACE	0
#define	HPSDR_CMD_ENDPOINT		0

#define	HPSDR_TX_INTERFACE		1
#define	HPSDR_TX_ALTINTERFACE	0
#define	HPSDR_TX_ENDPOINT		2	// streaming data from host to FPGA

#define	HPSDR_RX_INTERFACE		2
#define	HPSDR_RX_ALTINTERFACE	0
#define	HPSDR_RX_ENDPOINT		6	// streaming data from FPGA to host


#endif /* _HPSDR_INTERFACES_H_ */
