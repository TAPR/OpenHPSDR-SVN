/*
 * HPSDR/OZY - High Performance Software Defined Radio, OZY Firmware
 * 
 * SDR 1000 Control Support 
 *
 *
 * Copyright (C) 2007 Bill Tracey, KD5TFD (bill@ewjt.com)
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


#include "../include/spi.h"
#include "../include/hpsdr_commands.h"
#include "../include/hpsdr_rev1_regs.h"
#include "../include/hpsdr_common.h"
#include "../include/delay.h"

#if 0 
#include <string.h>

#include "../include/fpga.h"
#include "../include/timer.h"
#include "../include/i2c.h"
#include "../include/isr.h"
#include "../include/usb_common.h"
#include "../include/fx2utils.h"
#include "../include/hpsdr_globals.h"
#include "../include/hpsdr_i2c_addr.h"

#include "../include/eeprom_io.h"
#include "../include/usb_descriptors.h"
#endif 


#define ADDR_DATA 0x1
#define ADDR_CTRL 0x2
#define CTRL_MASK (0x0f) 
#define ADDR_STATUS 0x3 

    
#define LATCH_EXT 0x1 
#define LATCH_BPF 0x2 
#define LATCH_DAT 0x4 
#define LATCH_ADR 0x8 
    

#define SPI_EN_FPGA 128 

#if 0 
// wjt -- not clue why this does not work 
unsigned char hexits[] = "0123456789abcdef"; 
void byteToHex(unsigned char b, unsigned char *bufp) {
	bufp[2] = 0; 
	bufp[1] = hexits[b & 0xf]; 
	bufp[0] = hexits[(b >> 4) & 0xf]; 
	return; 
}
#endif


void byteToHex(unsigned char b, unsigned char *bufp) {
	unsigned char hi = (b >> 4) & 0xf;
	unsigned char lo = ( b & 0xf );
	if ( hi >= 10 ) {
		bufp[0] = 'a' + hi - 10;    
	}
	else {
		bufp[0] = '0' + hi;  
	}
	if ( lo >= 10 ) {
		bufp[1] = 'a' + lo - 10;    
	}
	else {
		bufp[1] = '0' + lo;  
	}
	bufp[2] = 0;
	 
	return;
} 


// Write the Data reg of the simulated parallel port - returns 0 on failure, !0 for ok  
unsigned char SDR1k_Control_setDataReg(unsigned char val) {
	unsigned char xdata databuf[1];
	unsigned char bbuf[3];  
	databuf[0] = val;
	putstr("setDataReg val=0x"); 
	byteToHex(val, bbuf); 
	putstr(bbuf); 
	putstr("\n");  
	
	// HPSDR_LED_REG ^= bmLED0; // toggle LED so we know we got here 
	if (!spi_write (0, ADDR_DATA, SPI_EN_FPGA, SPI_FMT_HDR_1 | SPI_FMT_MSB , (const xdata unsigned char *)databuf, 1)) {
		putstr("setDataReg bailout\n"); 
		return 0;  
	}	
	/* else */
 	putstr("setDataReg good rc\n");  
	return 1; 	 
}


void SDR1k_Control_delay() {
	return;  
}


#define LATCH_INTEROP_DELAY_MICROS (2)
#define DDS_RESET_INTEROP_DELAY_MICROS (2)
#define DDS_WRITE_INTEROP_DELAY_MICROS (2) 

unsigned char LastBPF_Value = 0; 

unsigned char SDR1k_Control_Latch(unsigned char wIndexH, unsigned char wIndexL)  {
	// wIndexH == latch 
	// wIndexL == data
	
	unsigned char rc;  
	unsigned char xdata databuf[1];
	
	if ( wIndexH == LATCH_BPF ) { // we need to remember last value written to bpf to be able to support writing to the rfe regs
		                          // since rfe only uses bottom 6 bits of this mess 
		LastBPF_Value = wIndexL;  
	} 
	

	// spi_write returns 0 on failure 
	
	do {  /* once */
		databuf[0] = wIndexL; 
		rc = spi_write(0, ADDR_DATA, SPI_EN_FPGA, SPI_FMT_HDR_1 | SPI_FMT_MSB , (const xdata unsigned char *)databuf, 1); 
		if ( rc == 0 ) {
			putstr("latch bail out spi write 1\n");  
			break;
		}
		udelay(LATCH_INTEROP_DELAY_MICROS); 
		// SDR1k_Control_delay(); 
		
		//!!FIXME -- need to read the control register before writing it since it is shared w/ the 9 pin connector 
		// 
		databuf[0] = wIndexH; 
		rc = spi_write(0, ADDR_CTRL, SPI_EN_FPGA, SPI_FMT_HDR_1 | SPI_FMT_MSB , (const xdata unsigned char *)databuf, 1);
		if ( rc == 0 ) { 
			putstr("latch bail out spi write 2\n");
			break;
		}
		udelay(LATCH_INTEROP_DELAY_MICROS);
		// SDR1k_Control_delay();
		 
		databuf[0] = 0; 
		rc = spi_write(0, ADDR_CTRL, SPI_EN_FPGA, SPI_FMT_HDR_1 | SPI_FMT_MSB , (const xdata unsigned char *)databuf, 1);
		if ( rc == 0 ) { 
			putstr("latch bail out spi write 3\n");
			break;
		}	 
		udelay(LATCH_INTEROP_DELAY_MICROS); 
		
	} while ( 0 ); 
	 
	return rc; 	
}



// reset the DDS - returns 1 on success, 0 on failure 
unsigned char SDR1k_Control_DDS_Reset() {
	unsigned char rc; 
	do { /* once */ 
		rc = SDR1k_Control_Latch(LATCH_ADR, 0x40);
		if ( rc == 0 ) {
			putstr("DDS Reset bail out 1\n");  
			break;
		}
		udelay(DDS_RESET_INTEROP_DELAY_MICROS);   
		rc = SDR1k_Control_Latch(LATCH_ADR, 0xc0);
		if ( rc == 0 ) {
			putstr("DDS Reset bail out 2\n"); 
			break; 
		}
		udelay(DDS_RESET_INTEROP_DELAY_MICROS);
		rc = SDR1k_Control_Latch(LATCH_ADR, 0x40);
		if ( rc == 0 ) {
			putstr("DDS Reset bail out 3\n"); 
			break; 
		} 		   	
		udelay(DDS_RESET_INTEROP_DELAY_MICROS);
	} while ( 0 ); 
	return rc; 	
}

 
unsigned char SDR1k_Control_DDS_Write(unsigned char wIndexH, unsigned char wIndexL) {
	unsigned int rc; 
	do { /* once */ 
		rc = SDR1k_Control_Latch(LATCH_DAT, wIndexL);
		if ( rc == 0 ) { 
			putstr("DDS Write bail out 1\n");
			break;
		}
		udelay(DDS_WRITE_INTEROP_DELAY_MICROS);
		rc = SDR1k_Control_Latch(LATCH_ADR, 0x40 | wIndexH);
		if ( rc == 0 ) { 
			putstr("DDS Write bail out 2\n");
			break;
		} 
		udelay(DDS_WRITE_INTEROP_DELAY_MICROS);
		rc = SDR1k_Control_Latch(LATCH_ADR, wIndexH);
		if ( rc == 0 ) { 
			putstr("DDS Write bail out 3\n");
			break;
		}
		udelay(DDS_WRITE_INTEROP_DELAY_MICROS);
		rc = SDR1k_Control_Latch(LATCH_ADR, 0x40 | wIndexH);
		if ( rc == 0 ) {
			putstr("DDS Write bail out 4\n"); 
			break;
		}
		udelay(DDS_WRITE_INTEROP_DELAY_MICROS);
		
	} while ( 0 ); 
	 
	return rc;
}

#define SET_BIT(num, bitnum)   (num) |= ( 1 << (bitnum) )
#define CLEAR_BIT(num, bitnum)   (num) &= (~( 1 << (bitnum)) )
#define IS_BIT_SET(num, bitnum)  ( (num) &  (1<<(bitnum)) )


 
unsigned char SDR1k_Control_SR_Load(unsigned char wIndexH, unsigned char wIndexL) {
	// wIndexH == addr
	// wIndexL = value 
	
	int i; 
	unsigned char rc;  
	unsigned char bpfbits = LastBPF_Value;
	SET_BIT(bpfbits, 5);  // srload=1 
	if ( IS_BIT_SET(wIndexH, 4) ) {
		SET_BIT(bpfbits, 4);  
	} 
	else {
		CLEAR_BIT(bpfbits, 4); 
	}
	if ( IS_BIT_SET(wIndexH, 3) ) {
		SET_BIT(bpfbits, 3);  
	} 
	else {
		CLEAR_BIT(bpfbits, 3); 
	}
	SET_BIT(bpfbits, 2);  // xsclr = 1
	CLEAR_BIT(bpfbits, 1);  // sclk = 0 
	
	 if ( wIndexL != 0 ) {
	 	for ( i = 7; i >= 0; i-- ) { 
	 		if ( IS_BIT_SET(wIndexL, i) ) {
	 			SET_BIT(bpfbits, 0);  
	 		} 
	 		else {
	 			CLEAR_BIT(bpfbits, 0);  
	 		} 
	 		rc = SDR1k_Control_Latch(LATCH_BPF, bpfbits); // load data 
	 		SET_BIT(bpfbits, 1);   // sclk = hi 
	 		rc = SDR1k_Control_Latch(LATCH_BPF, bpfbits); // clock it 
	 		CLEAR_BIT(bpfbits, 1); // sclk = lo 	 		 	 		
	 	}   
	 } 
	 else { // value to write is 0 - short circuit it  
	 	CLEAR_BIT(bpfbits, 2); 
	 	rc = SDR1k_Control_Latch(LATCH_BPF, bpfbits); 
	 }
	 // load it  
	 CLEAR_BIT(bpfbits, 5); // srload=0 
	 rc = SDR1k_Control_Latch(LATCH_BPF, bpfbits); 
	 SET_BIT(bpfbits, 5); // srload=1
	 rc = SDR1k_Control_Latch(LATCH_BPF, bpfbits);  
	return rc; 
	
} 

// returns 1 on success, 0 on failure 
//
// wValueL = command 
// wIndexL = value  
unsigned char SDR1k_ControlOut(unsigned char wValueH, unsigned char wValueL,
							  unsigned char wIndexH, unsigned char wIndexL, 
							  const xdata unsigned char *bufp, unsigned char buflen) {
#if 0 							  	
	unsigned char hbuf[3]; 	 
	putstr("SDR1k_ControlOut wValueL=0x");
	byteToHex(wValueL, hbuf); 
	putstr(hbuf); 
	putstr("\n");
#endif 	 
	
	switch ( wValueL ) {			
		case SDR1KCTRL_LATCH:    
			return SDR1k_Control_Latch(wIndexH, wIndexL); 
			break; 
			
	 	case SDR1KCTRL_DDS_RESET:
	 		return SDR1k_Control_DDS_Reset(); 
			break; 
	 	
		case SDR1KCTRL_DDS_WRITE:
			return SDR1k_Control_DDS_Write(wIndexH, wIndexL); 
			break; 
		 
		case SDR1KCTRL_SR_LOAD:     
			return SDR1k_Control_SR_Load(wIndexH, wIndexL); 
			break; 
		
		case SDR1KCTRL_SET_DATA_REG: 
			return SDR1k_Control_setDataReg(wIndexL); 
			break;  
	}
	return 0;	 
}


 

unsigned char SDR1k_Control_Read_Status(const xdata unsigned char *bufp, unsigned char buflen) {
	unsigned char rc; 
	unsigned char bbuf[3]; 
	// 0x40 == read 
	rc = spi_read(0, ADDR_STATUS | 0x40, SPI_EN_FPGA, SPI_FMT_HDR_1 | SPI_FMT_MSB, bufp, buflen);
#if 0 	
	putstr("read_status rc=0x");  
	byteToHex(rc, bbuf);  
	putstr(bbuf); 
	putstr(" data=0x"); 
	byteToHex(bufp[0], bbuf); 
	putstr(bbuf); 
	putstr("\n");
#endif
	return rc; 
}

 
unsigned char SDR1k_Control_Read_Version(const xdata unsigned char *bufp, unsigned char buflen) {
	unsigned char rc; 
	unsigned char bbuf[3]; 
	unsigned char *srcp; 
	xdata unsigned char *destp;
	int i;
	unsigned char ver_string[] = "20070722";   
	
#if 0 
	putstr("read_version\n-->");
	putstr(ver_string); 
	putstr("<--\n"); 
#endif
 
	srcp = ver_string; 
	destp = (xdata unsigned char *)bufp; 
	while ( *srcp != 0 ) {
		*destp = *srcp; 
		++destp; 
		++srcp; 
	} 
 	
	return 1;
	 
	 
	
#if 0 	
	putstr("read_status rc=0x");  
	byteToHex(rc, bbuf);  
	putstr(bbuf); 
	putstr(" data=0x"); 
	byteToHex(bufp[0], bbuf); 
	putstr(bbuf); 
	putstr("\n");
#endif
	 
}


// returns 1 on success, 0 on failure 
//
// wValueL = command 
// wIndexL = value  
unsigned char SDR1k_ControlIn(unsigned char wValueH, unsigned char wValueL,
							   unsigned char wIndexH, unsigned char wIndexL, 
							   const xdata unsigned char *bufp, unsigned char buflen) {
	 							   	
	switch ( wValueL ) {
		
		case SDR1KCTRL_READ_STATUS: 
			return SDR1k_Control_Read_Status(bufp, buflen);
			break;  
			
        case SDR1KCTRL_READ_VERSION: 
			return SDR1k_Control_Read_Version(bufp, buflen);
			break;
				
		default:
			putstr("Sdr1k_ControlIn: not implementd!\n"); 
			break;    
	} 							   	
	return 0;  							   	
	
}							   	





