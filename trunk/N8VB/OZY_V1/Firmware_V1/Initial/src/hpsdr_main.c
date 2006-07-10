/*
 * HPSDR/OZY - High Performance Software Defined Radio, OZY Firmware
 *
 * Adapted from USRP firmware 07/10/2006 by Phil Covington N8VB
 *
 * Copyright (C) 2003,2004 Free Software Foundation, Inc.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
 
#include "hpsdr_common.h"
#include "hpsdr_commands.h"
#include "fpga.h"
#include "timer.h"
#include "i2c.h"
#include "isr.h"
#include "usb_common.h"
#include "fx2utils.h"
#include "hpsdr_globals.h"
#include "hpsdr_i2c_addr.h"
#include <string.h>
#include "spi.h"
#include "eeprom_io.h"
#include "usb_descriptors.h"

/*
 * offsets into boot eeprom for configuration values
 */
#define	HW_REV_OFFSET		  5

#define	bRequestType	SETUPDAT[0]
#define	bRequest		SETUPDAT[1]
#define	wValueL			SETUPDAT[2]
#define	wValueH			SETUPDAT[3]
#define	wIndexL			SETUPDAT[4]
#define	wIndexH		SETUPDAT[5]
#define	wLengthL		SETUPDAT[6]
#define	wLengthH		SETUPDAT[7]

static void
get_ep0_data (void)
{
	EP0BCL = 0;			// arm EP0 for OUT xfer.  This sets the busy bit
  	while (EP0CS & bmEPBUSY);	// wait for busy to clear    
}

/*
 * Handle our "Vendor Extension" commands on endpoint 0.
 * If we handle this one, return non-zero.
 */

unsigned char app_vendor_OUT_cmd(void)
{
	switch (bRequest)
		{

			case VRQ_SET_LED:
				
		  		switch (wIndexL)
					{
						case 0:
							set_led_0 (wValueL);
							break;

					      	case 1:
							set_led_1 (wValueL);
							break;

					      	default:
							return 0;
		  			}	
		  		break;
		  
			case VRQ_FPGA_LOAD:

				switch (wIndexL)			// sub-command
					{
						case FL_BEGIN:
							return fpga_load_begin ();

					      	case FL_XFER:
							get_ep0_data ();
							return fpga_load_xfer (EP0BUF, EP0BCL);

					      	case FL_END:
							return fpga_load_end ();

  						default:
							return 0;
  					}
  				break;      

			case VRQ_I2C_WRITE:

				get_ep0_data ();
				if (!i2c_write (wValueL, EP0BUF, EP0BCL))
					return 0;
				break;

			case VRQ_SPI_WRITE:

				get_ep0_data ();
		  		if (!spi_write (wValueH, wValueL, wIndexH, wIndexL, EP0BUF, EP0BCL))
					return 0;
		  		break;

			default:
		  		return 0;
	 }
	return 1;
}

unsigned char app_vendor_IN_cmd(void)
{
	switch (bRequest)
		{
			case VRQ_I2C_READ:
      				if (!i2c_read (wValueL, EP0BUF, wLengthL))
					return 0;
      				EP0BCH = 0;
      				EP0BCL = wLengthL;
      				break;
  
			case VRQ_SPI_READ:
  				if (!spi_read (wValueH, wValueL, wIndexH, wIndexL, EP0BUF, wLengthL))
					return 0;
			      EP0BCH = 0;
			      EP0BCL = wLengthL;
			      break;

		    	default:
		      		return 0;
		}
	return 1;
}

unsigned char
app_vendor_cmd (void)
{
	if (bRequestType == VRT_VENDOR_IN)
		return app_vendor_IN_cmd();		
  	else if (bRequestType == VRT_VENDOR_OUT)
		return app_vendor_OUT_cmd();
	else
    		return 0;    // invalid bRequestType  	
}

static void
main_loop (void)
{
  
  while (1)
  	{
    		if (usb_setup_packet_avail ())
      			usb_handle_setup_packet ();        
  	}
}


/*
 * called at 100 Hz from timer2 interrupt
 *
 * Toggle led 0
 */
void
isr_tick (void) interrupt
{
	static unsigned char	count = 1;  
  	if (--count == 0)
  		{
    			count = 50;
    			HPSDR_LED_REG ^= bmLED0;
		}
  clear_timer_irq ();
}

/*
 * Read h/w rev code and serial number out of boot eeprom and
 * patch the usb descriptors with the values.
 */
void
patch_usb_descriptors(void)
{	
	static xdata unsigned char hw_rev;
	static xdata unsigned char serial_no[8];
	unsigned char i;

	eeprom_read(I2C_ADDR_BOOT, HW_REV_OFFSET, &hw_rev, 1);	// LSB of device id
  
	usb_desc_hw_rev_binary_patch_location_0[0] = hw_rev;
	usb_desc_hw_rev_binary_patch_location_1[0] = hw_rev;
	usb_desc_hw_rev_ascii_patch_location_0[0] = hw_rev + '0';     // FIXME if we get > 9 
}

void
main (void)
{
	init_hpsdr();
    
  	set_led_0 (0);
  	set_led_1 (0);
  
  	EA = 0;		// disable all interrupts

  	patch_usb_descriptors();

  	setup_autovectors ();
  	usb_install_handlers ();
  	hook_timer_tick ((unsigned short) isr_tick);

  	EIEX4 = 1;	// disable INT4 FIXME
  	EA = 1;			// global interrupt enable

  	fx2_renumerate ();	// simulates disconnect / reconnect

  	main_loop ();
}
