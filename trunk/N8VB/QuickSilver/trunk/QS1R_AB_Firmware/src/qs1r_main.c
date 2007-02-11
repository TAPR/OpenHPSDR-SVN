/*
 * QuickSilver QS1R Firmware
 *
 * Adapted from USRP firmware 12/07/2006 by Phil Covington N8VB
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

#include <string.h>
#include "../include/qs1r_common.h"
#include "../include/qs1r_commands.h"
#include "../include/fpga.h"
#include "../include/timer.h"
#include "../include/i2c.h"
#include "../include/isr.h"
#include "../include/usb_common.h"
#include "../include/fx2utils.h"
#include "../include/qs1r_globals.h"
#include "../include/qs1r_i2c_addr.h"
#include "../include/spi.h"
#include "../include/eeprom_io.h"
#include "../include/usb_descriptors.h"
#include "../include/qs1r_ddc_regs.h"

/*
 * offsets into boot eeprom for configuration values
 */
#define	HW_REV_OFFSET		  5

#define	bRequestType	SETUPDAT[0]
#define	bRequest		    SETUPDAT[1]
#define	wValueL			SETUPDAT[2]
#define	wValueH			SETUPDAT[3]
#define	wIndexL			SETUPDAT[4]
#define	wIndexH		    SETUPDAT[5]
#define	wLengthL		    SETUPDAT[6]
#define	wLengthH		    SETUPDAT[7]

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
    int i, j;
    unsigned char offset;

	switch (bRequest)
		{

            case VRQ_DEBUG_LED_SET:

                DEBUG_LED_REG = (BYTE)wValueL;
                break;

            case VRQ_LTC2208_CNTRL_SET:

                LTC2208_CNTRL_REG = (BYTE)wValueL;
                break;

            case VRQ_ATTN_SET:

                ATTN = (BYTE)wValueL;
                break;

            case VRQ_RFFE_SET:

                RFFE_CNTRL = (BYTE)wValueL;
                break;

            case VRQ_DDC_CNTRL_SET:

                DDC_CNTRL = (BYTE)wValueL;
                break;

            case VRQ_CPLD_GPIO_SET:

                CPLD_GPIO = (BYTE)wValueL;
                break;

            case VRQ_DDC0_DR0_SET:

                DDC0_DR0 = (BYTE)wValueL;
                break;

            case VRQ_DDC0_DR1_SET:

                DDC0_DR1 = (BYTE)wValueL;
                break;

            case VRQ_DDC0_DR2_SET:

                DDC0_DR2 = (BYTE)wValueL;
                break;

            case VRQ_DDC0_DR3_SET:

                DDC0_DR3 = (BYTE)wValueL;
                break;

            case VRQ_DDC0_DR4_SET:

                DDC0_DR4 = (BYTE)wValueL;
                break;

            case VRQ_DDC0_LAR_SET:

                DDC0_LAR = (BYTE)wValueL;
                break;

            case VRQ_DDC0_AMR_SET:

                DDC0_AMR = (BYTE)wValueL;
                break;

            case VRQ_DDC1_DR0_SET:

                DDC1_DR0 = (BYTE)wValueL;
                break;

            case VRQ_DDC1_DR1_SET:

                DDC1_DR1 = (BYTE)wValueL;
                break;

            case VRQ_DDC1_DR2_SET:

                DDC1_DR2 = (BYTE)wValueL;
                break;

            case VRQ_DDC1_DR3_SET:

                DDC1_DR3 = (BYTE)wValueL;
                break;

            case VRQ_DDC1_DR4_SET:

                DDC1_DR4 = (BYTE)wValueL;
                break;

            case VRQ_DDC1_LAR_SET:

                DDC1_LAR = (BYTE)wValueL;
                break;

            case VRQ_DDC1_AMR_SET:

                DDC1_AMR = (BYTE)wValueL;
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

            case VRQ_I2C_SPEED_SET:
                if (wValueL == 1)
                    I2CTL |= bm400KHZ;
                else
                    I2CTL &= ~bm400KHZ;
                break;

            case VRQ_CPU_SPEED_SET:
                if (wValueL == 0)
                    CPUCS = bmCLKOE; // 12 MHz
                else if (wValueL == 1)
                    CPUCS = bmCLKSPD0 | bmCLKOE; // 24 MHz
                else
                    CPUCS = bmCLKSPD1 | bmCLKOE; // 48 MHz
                break;

            case VRQ_DDC_MCR_SET:
                get_ep0_data ();
		  		if (wIndexL == 0) {
                    DDC0_AMR = (MCR >> 8);
                    DDC0_LAR = (MCR & 0xFF);
                    DDC0_DR0 = EP0BUF[0];
                } else if (wIndexL == 1) {
                    DDC1_AMR = (MCR >> 8);
                    DDC1_LAR = (MCR & 0xFF);
                    DDC1_DR0 = EP0BUF[0];
                } else return 0;
                break;

            case VRQ_DDC_NCR_SET:
                get_ep0_data ();
		  		if (wIndexL == 0) {
                    DDC0_AMR = (NCR >> 8);
                    DDC0_LAR = (NCR & 0xFF);
                    DDC0_DR0 = EP0BUF[0];
                } else if (wIndexL == 1) {
                    DDC1_AMR = (NCR >> 8);
                    DDC1_LAR = (NCR & 0xFF);
                    DDC1_DR0 = EP0BUF[0];
                } else return 0;
                break;

            case VRQ_DDC_NSCR_SET:
                get_ep0_data ();
		  		if (wIndexL == 0) {
                    DDC0_AMR = (NSCR >> 8);
                    DDC0_LAR = (NSCR & 0xFF);
                    DDC0_DR3 = EP0BUF[3];
                    DDC0_DR2 = EP0BUF[2];
                    DDC0_DR1 = EP0BUF[1];
                    DDC0_DR0 = EP0BUF[0];
                } else if (wIndexL == 1) {
                    DDC1_AMR = (NSCR >> 8);
                    DDC1_LAR = (NSCR & 0xFF);
                    DDC1_DR3 = EP0BUF[3];
                    DDC1_DR2 = EP0BUF[2];
                    DDC1_DR1 = EP0BUF[1];
                    DDC1_DR0 = EP0BUF[0];
                } else return 0;
                break;

		  	case VRQ_DDC_NFRE_SET:
                get_ep0_data ();
		  		if (wIndexL == 0) {
                    DDC0_AMR = (NFRE >> 8);
                    DDC0_LAR = (NFRE & 0xFF);
                    DDC0_DR3 = EP0BUF[3];
                    DDC0_DR2 = EP0BUF[2];
                    DDC0_DR1 = EP0BUF[1];
                    DDC0_DR0 = EP0BUF[0];
                } else if (wIndexL == 1) {
                    DDC1_AMR = (NFRE >> 8);
                    DDC1_LAR = (NFRE & 0xFF);
                    DDC1_DR3 = EP0BUF[3];
                    DDC1_DR2 = EP0BUF[2];
                    DDC1_DR1 = EP0BUF[1];
                    DDC1_DR0 = EP0BUF[0];
                } else return 0;
                break;

            case VRQ_DDC_NPO_SET:
                get_ep0_data ();
		  		if (wIndexL == 0) {
                    DDC0_AMR = (NPHA >> 8);
                    DDC0_LAR = (NPHA & 0xFF);
                    DDC0_DR1 = EP0BUF[1];
                    DDC0_DR0 = EP0BUF[0];
                } else if (wIndexL == 1) {
                    DDC1_AMR = (NPHA >> 8);
                    DDC1_LAR = (NPHA & 0xFF);
                    DDC1_DR1 = EP0BUF[1];
                    DDC1_DR0 = EP0BUF[0];
                } else return 0;
                break;

		  	case VRQ_DDC_C2SR_SET:
                get_ep0_data ();
		  		if (wIndexL == 0) {
                    DDC0_AMR = (C2SR >> 8);
                    DDC0_LAR = (C2SR & 0xFF);
                    DDC0_DR0 = EP0BUF[0];
                } else if (wIndexL == 1) {
                    DDC1_AMR = (C2SR >> 8);
                    DDC1_LAR = (C2SR & 0xFF);
                    DDC1_DR0 = EP0BUF[0];
                } else return 0;
                break;

            case VRQ_DDC_C2DEC_SET:
                get_ep0_data ();
		  		if (wIndexL == 0) {
                    DDC0_AMR = (C2DEC >> 8);
                    DDC0_LAR = (C2DEC & 0xFF);
                    DDC0_DR0 = EP0BUF[0];
                } else if (wIndexL == 1) {
                    DDC1_AMR = (C2DEC >> 8);
                    DDC1_LAR = (C2DEC & 0xFF);
                    DDC1_DR0 = EP0BUF[0];
                } else return 0;
                break;

		  	case VRQ_DDC_C5SR_SET:
                get_ep0_data ();
		  		if (wIndexL == 0) {
                    DDC0_AMR = (C5SR >> 8);
                    DDC0_LAR = (C5SR & 0xFF);
                    DDC0_DR0 = EP0BUF[0];
                } else if (wIndexL == 1) {
                    DDC1_AMR = (C5SR >> 8);
                    DDC1_LAR = (C5SR & 0xFF);
                    DDC1_DR0 = EP0BUF[0];
                } else return 0;
                break;

            case VRQ_DDC_C5DEC_SET:
                get_ep0_data ();
		  		if (wIndexL == 0) {
                    DDC0_AMR = (C5DEC >> 8);
                    DDC0_LAR = (C5DEC & 0xFF);
                    DDC0_DR0 = EP0BUF[0];
                } else if (wIndexL == 1) {
                    DDC1_AMR = (C5DEC >> 8);
                    DDC1_LAR = (C5DEC & 0xFF);
                    DDC1_DR0 = EP0BUF[0];
                } else return 0;
                break;

            case VRQ_DDC_RCR_SET:
                get_ep0_data ();
		  		if (wIndexL == 0) {
                    DDC0_AMR = (RCR >> 8);
                    DDC0_LAR = (RCR & 0xFF);
                    DDC0_DR0 = EP0BUF[0];
                } else if (wIndexL == 1) {
                    DDC1_AMR = (RCR >> 8);
                    DDC1_LAR = (RCR & 0xFF);
                    DDC1_DR0 = EP0BUF[0];
                } else return 0;
                break;

            case VRQ_DDC_RDEC_SET:
                get_ep0_data ();
		  		if (wIndexL == 0) {
                    DDC0_AMR = (RDEC >> 8);
                    DDC0_LAR = (RDEC & 0xFF);
                    DDC0_DR0 = EP0BUF[0];
                } else if (wIndexL == 1) {
                    DDC1_AMR = (RDEC >> 8);
                    DDC1_LAR = (RDEC & 0xFF);
                    DDC1_DR0 = EP0BUF[0];
                } else return 0;
                break;

            case VRQ_DDC_RAO_SET:
                get_ep0_data ();
		  		if (wIndexL == 0) {
                    DDC0_AMR = (RAO >> 8);
                    DDC0_LAR = (RAO & 0xFF);
                    DDC0_DR0 = EP0BUF[0];
                } else if (wIndexL == 1) {
                    DDC1_AMR = (RAO >> 8);
                    DDC1_LAR = (RAO & 0xFF);
                    DDC1_DR0 = EP0BUF[0];
                } else return 0;
                break;

            case VRQ_DDC_RTAPS_SET:
                get_ep0_data ();
		  		if (wIndexL == 0) {
                    DDC0_AMR = (RTAP >> 8);
                    DDC0_LAR = (RTAP & 0xFF);
                    DDC0_DR0 = EP0BUF[0];
                } else if (wIndexL == 1) {
                    DDC1_AMR = (RTAP >> 8);
                    DDC1_LAR = (RTAP & 0xFF);
                    DDC1_DR0 = EP0BUF[0];
                } else return 0;
                break;

		  	case VRQ_DDC_RSVR_SET:
                get_ep0_data ();
		  		if (wIndexL == 0) {
                    DDC0_AMR = (RSRV >> 8);
                    DDC0_LAR = (RSRV & 0xFF);
                    DDC0_DR0 = EP0BUF[0];
                } else if (wIndexL == 1) {
                    DDC1_AMR = (RSRV >> 8);
                    DDC1_LAR = (RSRV & 0xFF);
                    DDC1_DR0 = EP0BUF[0];
                } else return 0;
                break;

            case VRQ_DDC_COEF_SET:
                get_ep0_data ();
		  		if (wValueL > 0x0F) return 0;
                offset = wValueL << 4; // advance by 16
                if (wIndexL == 0)
                {
                    for (i=0, j=0; i < 16; i++, j+=3) {
                        DDC0_AMR = 0;
                        DDC0_LAR = ((DDC_COEF + i + offset) & 0xFF);
                        DDC0_DR2 = (EP0BUF[j+2] & 0xFF);
                        DDC0_DR1 = (EP0BUF[j+1] & 0xFF);
                        DDC0_DR0 = (EP0BUF[j] & 0xFF);
                    }
                } else if (wIndexL == 1) {
                    for (i=0, j=0; i < 16; i++, j+=3) {
                        DDC1_AMR = 0;
                        DDC1_LAR = ((DDC_COEF + i + offset) & 0xFF);
                        DDC1_DR2 = (EP0BUF[j+2] & 0xFF);
                        DDC1_DR1 = (EP0BUF[j+1] & 0xFF);
                        DDC1_DR0 = (EP0BUF[j] & 0xFF);
                    }
                } else return 0;
                break;

            case VRQ_DDC_COEF_CLR:
                get_ep0_data ();
                if (wIndexL == 0)  {
                    for (i=0; i < 0x100; i++) {
                        DDC0_AMR = ((DDC_COEF + i) & 0xFF00) >> 8;
                        DDC0_LAR = ((DDC_COEF + i) & 0xFF);
                        DDC0_DR4 = 0;
                        DDC0_DR3 = 0;
                        DDC0_DR2 = 0;
                        DDC0_DR1 = 0;
                        DDC0_DR0 = 0;
                    }
                } else if (wIndexL == 1) {
                     for (i=0; i < 0x100; i++) {
                        DDC1_AMR = ((DDC_COEF + i) & 0xFF00) >> 8;
                        DDC1_LAR = ((DDC_COEF + i) & 0xFF);
                        DDC1_DR4 = 0;
                        DDC1_DR3 = 0;
                        DDC1_DR2 = 0;
                        DDC1_DR1 = 0;
                        DDC1_DR0 = 0;
                    }
                } else return 0;
                break;

            case VRQ_DDC_RAM_CLR:
                get_ep0_data ();
                if (wIndexL == 0)  {
                    for (i=0; i < 0x100; i++) {
                        DDC0_AMR = ((DDC_RAM + i) & 0xFF00) >> 8;
                        DDC0_LAR = ((DDC_RAM + i) & 0xFF);
                        DDC0_DR4 = 0;
                        DDC0_DR3 = 0;
                        DDC0_DR2 = 0;
                        DDC0_DR1 = 0;
                        DDC0_DR0 = 0;
                    }
                } else if (wIndexL == 1) {
                     for (i=0; i < 0x100; i++) {
                        DDC1_AMR = ((DDC_RAM + i) & 0xFF00) >> 8;
                        DDC1_LAR = ((DDC_RAM + i) & 0xFF);
                        DDC1_DR4 = 0;
                        DDC1_DR3 = 0;
                        DDC1_DR2 = 0;
                        DDC1_DR1 = 0;
                        DDC1_DR0 = 0;
                    }
                } else return 0;
                break;

			default:
		  		return 0;
	 }
	return 1;
}

unsigned char app_vendor_IN_cmd(void)
{
    int i, j;
    unsigned char offset;
    unsigned char foop;

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

            case VRQ_EEPROM_TYPE_READ:
                EP0BUF[0] = I2CS & bmID; // 16 = 2 byte, 8 = 1 byte
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_I2C_SPEED_READ:
                EP0BUF[0] = I2CTL & bm400KHZ; // 0 = 100 kHz, 1 = 400 kHz
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_SN_READ:
                break;

            case VRQ_DEBUG_LED_READ:
                EP0BUF[0] = DEBUG_LED_REG;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_LTC2208_CNTRL_READ:
                EP0BUF[0] = LTC2208_CNTRL_REG;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_ATTN_READ:
                EP0BUF[0] = ATTN;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_RFFE_READ:
                EP0BUF[0] = RFFE_CNTRL;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC_CNTRL_READ:
                EP0BUF[0] = DDC_CNTRL;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_CPLD_GPIO_READ:
                EP0BUF[0] = CPLD_GPIO;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC0_DR0_READ:
                EP0BUF[0] = DDC0_DR0;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC0_DR1_READ:
                EP0BUF[0] = DDC0_DR1;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC0_DR2_READ:
                EP0BUF[0] = DDC0_DR2;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC0_DR3_READ:
                EP0BUF[0] = DDC0_DR3;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC0_DR4_READ:
                EP0BUF[0] = DDC0_DR4;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC0_LAR_READ:
                EP0BUF[0] = DDC0_LAR;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC0_AMR_READ:
                EP0BUF[0] = DDC0_AMR;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC1_DR0_READ:
                EP0BUF[0] = DDC1_DR0;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC1_DR1_READ:
                EP0BUF[0] = DDC1_DR1;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC1_DR2_READ:
                EP0BUF[0] = DDC1_DR2;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC1_DR3_READ:
                EP0BUF[0] = DDC1_DR3;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC1_DR4_READ:
                EP0BUF[0] = DDC1_DR4;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC1_LAR_READ:
                EP0BUF[0] = DDC1_LAR;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC1_AMR_READ:
                EP0BUF[0] = DDC1_AMR;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

           case VRQ_DDC_MCR_READ:
                if (wIndexL == 0) {
                    DDC0_AMR = (MCR >> 8);
                    DDC0_LAR = (MCR & 0xFF);
                    EP0BUF[0] = DDC0_DR0;
                } else if (wIndexL == 1) {
                    DDC1_AMR = (MCR >> 8);
                    DDC1_LAR = (MCR & 0xFF);
                    EP0BUF[0] = DDC1_DR0;
                } else return 0;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC_NCR_READ:
                if (wIndexL == 0) {
                    DDC0_AMR = (NCR >> 8);
                    DDC0_LAR = (NCR & 0xFF);
                    EP0BUF[0] = DDC0_DR0;
                } else if (wIndexL == 1) {
                    DDC1_AMR = (NCR >> 8);
                    DDC1_LAR = (NCR & 0xFF);
                    EP0BUF[0] = DDC1_DR0;
                } else return 0;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC_NSCR_READ:
                if (wIndexL == 0) {
                    DDC0_AMR = (NSCR >> 8);
                    DDC0_LAR = (NSCR & 0xFF);
                    EP0BUF[0] = DDC0_DR0;
                    EP0BUF[1] = DDC0_DR1;
                    EP0BUF[2] = DDC0_DR2;
                    EP0BUF[3] = DDC0_DR3;
                } else if (wIndexL == 1) {
                    DDC1_AMR = (NSCR >> 8);
                    DDC1_LAR = (NSCR & 0xFF);
                    EP0BUF[0] = DDC1_DR0;
                    EP0BUF[1] = DDC1_DR1;
                    EP0BUF[2] = DDC1_DR2;
                    EP0BUF[3] = DDC1_DR3;
                } else return 0;
                EP0BCH = 0;
                EP0BCL = 4;
                break;

            case VRQ_DDC_NFRE_READ:
                if (wIndexL == 0) {
                    DDC0_AMR = (NFRE >> 8);
                    DDC0_LAR = (NFRE & 0xFF);
                    EP0BUF[0] = DDC0_DR0;
                    EP0BUF[1] = DDC0_DR1;
                    EP0BUF[2] = DDC0_DR2;
                    EP0BUF[3] = DDC0_DR3;
                } else if (wIndexL == 1) {
                    DDC1_AMR = (NFRE >> 8);
                    DDC1_LAR = (NFRE & 0xFF);
                    EP0BUF[0] = DDC1_DR0;
                    EP0BUF[1] = DDC1_DR1;
                    EP0BUF[2] = DDC1_DR2;
                    EP0BUF[3] = DDC1_DR3;
                } else return 0;
                EP0BCH = 0;
                EP0BCL = 4;
                break;

            case VRQ_DDC_NPO_READ:
                 if (wIndexL == 0) {
                    DDC0_AMR = (NPHA >> 8);
                    DDC0_LAR = (NPHA & 0xFF);
                    EP0BUF[0] = DDC0_DR0;
                    EP0BUF[1] = DDC0_DR1;
                } else if (wIndexL == 1) {
                    DDC1_AMR = (NPHA >> 8);
                    DDC1_LAR = (NPHA & 0xFF);
                    EP0BUF[0] = DDC1_DR0;
                    EP0BUF[1] = DDC1_DR1;
                } else return 0;
                EP0BCH = 0;
                EP0BCL = 2;
                break;

            case VRQ_DDC_C2SR_READ:
                if (wIndexL == 0) {
                    DDC0_AMR = (C2SR >> 8);
                    DDC0_LAR = (C2SR & 0xFF);
                    EP0BUF[0] = DDC0_DR0;
                } else if (wIndexL == 1) {
                    DDC1_AMR = (C2SR >> 8);
                    DDC1_LAR = (C2SR & 0xFF);
                    EP0BUF[0] = DDC1_DR0;
                } else return 0;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC_C2DEC_READ:
               if (wIndexL == 0) {
                    DDC0_AMR = ((C2DEC & 0xFF00) >> 8);
                    DDC0_LAR = (C2DEC & 0xFF);
                    foop = DDC0_DR1;
                    EP0BUF[0] = DDC0_DR0;
                } else if (wIndexL == 1) {
                    DDC1_AMR = ((C2DEC & 0xFF00) >> 8);
                    DDC1_LAR = (C2DEC & 0xFF);
                    foop = DDC1_DR1;
                    EP0BUF[0] = DDC1_DR0;
                } else return 0;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC_C5SR_READ:
                if (wIndexL == 0) {
                    DDC0_AMR = (C5SR >> 8);
                    DDC0_LAR = (C5SR & 0xFF);
                    EP0BUF[0] = DDC0_DR0;
                } else if (wIndexL == 1) {
                    DDC1_AMR = (C5SR >> 8);
                    DDC1_LAR = (C5SR & 0xFF);
                    EP0BUF[0] = DDC1_DR0;
                } else return 0;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC_C5DEC_READ:
                if (wIndexL == 0) {
                    DDC0_AMR = (C5DEC >> 8);
                    DDC0_LAR = (C5DEC & 0xFF);
                    EP0BUF[0] = DDC0_DR0;
                } else if (wIndexL == 1) {
                    DDC1_AMR = (C5DEC >> 8);
                    DDC1_LAR = (C5DEC & 0xFF);
                    EP0BUF[0] = DDC1_DR0;
                } else return 0;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC_RCR_READ:
                if (wIndexL == 0) {
                    DDC0_AMR = (RCR >> 8);
                    DDC0_LAR = (RCR & 0xFF);
                    EP0BUF[0] = DDC0_DR0;
                } else if (wIndexL == 1) {
                    DDC1_AMR = (RCR >> 8);
                    DDC1_LAR = (RCR & 0xFF);
                    EP0BUF[0] = DDC1_DR0;
                } else return 0;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC_RDEC_READ:
                if (wIndexL == 0) {
                    DDC0_AMR = (RDEC >> 8);
                    DDC0_LAR = (RDEC & 0xFF);
                    EP0BUF[0] = DDC0_DR0;
                } else if (wIndexL == 1) {
                    DDC1_AMR = (RDEC >> 8);
                    DDC1_LAR = (RDEC & 0xFF);
                    EP0BUF[0] = DDC1_DR0;
                } else return 0;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC_RAO_READ:
                if (wIndexL == 0) {
                    DDC0_AMR = (RAO >> 8);
                    DDC0_LAR = (RAO & 0xFF);
                    EP0BUF[0] = DDC0_DR0;
                } else if (wIndexL == 1) {
                    DDC1_AMR = (RAO >> 8);
                    DDC1_LAR = (RAO & 0xFF);
                    EP0BUF[0] = DDC1_DR0;
                } else return 0;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC_RTAPS_READ:
                if (wIndexL == 0) {
                    DDC0_AMR = (RTAP >> 8);
                    DDC0_LAR = (RTAP & 0xFF);
                    EP0BUF[0] = DDC0_DR0;
                } else if (wIndexL == 1) {
                    DDC1_AMR = (RTAP >> 8);
                    DDC1_LAR = (RTAP & 0xFF);
                    EP0BUF[0] = DDC1_DR0;
                } else return 0;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC_RSVR_READ:
                if (wIndexL == 0) {
                    DDC0_AMR = (RSRV >> 8);
                    DDC0_LAR = (RSRV & 0xFF);
                    EP0BUF[0] = DDC0_DR0;
                } else if (wIndexL == 1) {
                    DDC1_AMR = (RSRV >> 8);
                    DDC1_LAR = (RSRV & 0xFF);
                    EP0BUF[0] = DDC1_DR0;
                } else return 0;
                EP0BCH = 0;
                EP0BCL = 1;
                break;

            case VRQ_DDC_COEF_READ:
                if (wValueL > 0x0F) return 0;
                offset = wValueL << 4; // advance by 16
                if (wIndexL == 0)
                {
                    for (i=0, j=0; i < 16; i++, j+=3) {
                        DDC0_AMR = 0;
                        DDC0_LAR = ((i + offset) & 0xFF);
                        EP0BUF[j] = DDC0_DR0;
                        EP0BUF[j+1] = DDC0_DR1;
                        EP0BUF[j+2] = (DDC0_DR2 & 0x0F);
                    }
                } else if (wIndexL == 1) {
                    for (i=0, j=0; i < 16; i++, j+=3) {
                        DDC1_AMR = 0;
                        DDC1_LAR = ((i + offset) & 0xFF);
                        EP0BUF[j] = DDC1_DR0;
                        EP0BUF[j+1] = DDC1_DR1;
                        EP0BUF[j+2] = (DDC1_DR2 & 0x0F);
                    }
                } else return 0;
                EP0BCH = 0;
                EP0BCL = 48;
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
            else
                putchar(32);
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
    		// do nothing basically;
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

	eeprom_read(I2C_ADDR_BOOT, HW_REV_OFFSET, &hw_rev, 1);	// LSB of device id

	usb_desc_hw_rev_binary_patch_location_0[0] = hw_rev;
	usb_desc_hw_rev_binary_patch_location_1[0] = hw_rev;
	usb_desc_hw_rev_ascii_patch_location_0[0] = hw_rev + '0';     // FIXME if we get > 9
}

/*
 * 07/12/2006 P.Covington N8VB
 * NOTES: The vectors.rel must be linked in first
 * I'm using
 * Compiler options: --code-loc 0x0000 --code-size 0x2000 --xram-loc 0x2000 --xram-size 0x2000 --no-xinit-opt
 * Linker options: -Wl "-b USBDESCSEG = 0xE000"
 */
void
main (void)
{
    init_qs1r();

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
