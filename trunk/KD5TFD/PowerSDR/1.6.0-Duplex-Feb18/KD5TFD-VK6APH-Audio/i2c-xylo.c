// Copyright Bill Tracey 2006 (bill@ewjt.com) 
// Derived from Code from Phil Harman (VK6APH) 
//
//
// i2c-xylo - send I2C commands to devices on the Xylo's I2C bus 
// 

#include "KD5TFD-VK6APH-Audio.h"
#include "private.h" 
#include <windows.h> 
#include <string.h>
#include <stdio.h> 

char *I2C_rc_strings[] = { 
	"ok", 
	"cmd too long", 
	"Xylo open failed", 
	"Xylo write failed", 
	"Xylo read failed", 
	"i2c bus error", 
	"no i2c ack", 
	"unknown"     // last entry needs to be unknwon 
} ;

// get a string describing an I2C_cmd rc 
// 
KD5TFDVK6APHAUDIO_API char * I2C_RCtoString(int rc) { 
	if ( rc >  sizeof(I2C_rc_strings) - 1   ) { 
		rc = sizeof(I2C_rc_strings) - 1; 
	} 
	return I2C_rc_strings[rc]; 
}


// 
// send an I2C command 
//		addr - address of device 
//		cmdbuf - array of bytes in the command 
//		cmdlen - # of bytes in cmd in cmdbuf 
// 
// returns: 
//  0 - success		
//  1 - cmd buf too long 
//  2 - could not open Xylo 
//  3 - failed writing i2c command to xylo 
//  4 - failed reading i2c response from xylo 
//  5 - i2c bus error 
//  6 - no i2c ack 
KD5TFDVK6APHAUDIO_API int I2C_Cmd(unsigned char addr, unsigned char cmdbuf[], unsigned char cmdlen) { 
	unsigned char buf[65]; 
	int rc; 
	int numwritten; 
	int numread; 
	HANDLE h; 
	if ( cmdlen > 64 ) { 
		return 1; 
	} 
	buf[0] = 1; // number of commands 
	buf[1] = 1 + cmdlen; 
	buf[2] = addr; 
	memcpy((void *)(&buf[3]), cmdbuf, cmdlen); 
	h = XyloOpen(); 
	if ( h == NULL ) { 
		return 2; 
	} 
	do { /* once */ 
		// write the i2c command 
		numwritten = XyloBulkWrite(h, 0, buf, 3+cmdlen); 
		// printf("\nnumwritten: %d cmdlen: %d\n", numwritten, cmdlen); 
		if ( numwritten != 3+cmdlen ) {  // short write 
			rc = 3; 
			break; 
		} 
		// now read the response 
		numread = XyloBulkRead(h, 1, buf, sizeof(buf));
		if ( numread < 1 ) {
			rc = 4; 
			break; 
		} 
		if ( buf[0] & 0x80 ) {  // bus error 
			rc = 5; 
		} 
		else if ( buf[0] & 0x40 ) {  // got ack 
			rc = 0;  
		} 
		else {  // no ack */ 
			rc = 6;
		} 

	} while ( 0 ) ; 
	XyloClose(h); 
	return rc; 
} 




#if 0 
// vk6aph's original code 
//Setup chips via  I2C

// Set PIC MCP23016 I/O expander to GP0 as outputs

	str[0] = 1;	str[1] = 3;	str[2] = 0x40;	str[3] = 0x06;	str[4] = 0x00;
	if(USB_send_I2C(str, 5))
		printf("I2C error - Can't setup GPO  outputs \n");

// Set PIC MCP23016 I/O expander to GP1 as inputs

	str[0] = 1;	str[1] = 3;	str[2] = 0x40;	str[3] = 0x07;	str[4] = 0xff;
	if(USB_send_I2C(str, 5))
		printf("I2C error - Can't setup GP1  inputs \n");

// Select GP1 inputs 

	str[0] = 1;	str[1] = 2;	str[2] = 0x40;	str[3] = 0x01;	
	if(USB_send_I2C(str, 4))
		printf("I2C error - Can't select GP1  inputs \n");

// set Wolfson to 16 bit

	str[0] = 1;	str[1] = 3;	str[2] = 0x34;	str[3] = 0x02;	str[4] = 0x00;
	if(USB_send_I2C(str, 5))
		printf("I2C error - Can't setup Wolfson to 16 bit\n");

/*
// set Wolfson to 24 bit

	str[0] = 1;	str[1] = 3;	str[2] = 0x34;	str[3] = 0x02;	str[4] = 0x02;
	if(USB_send_I2C(str, 5)0
	printf("I2C error - Can't setup Wolfson to 24 bit \n");
*/

// set Wolfson to Left Justified, Master and 48KHz sampling rate

	str[0] = 1;	str[1] = 3;	str[2] = 0x34;	str[3] = 0x00;	str[4] = 0x25;
	if(USB_send_I2C(str, 5))
		printf("I2C error - Can't setup Wolfson as Master etc \n");

// set TLV 
//
// TLV320  		- 36 1E 00 - Reset chip
//			- 36 12 01 - set digital interface active
//			- 36 08 15 - D/A on, mic input, mic 20dB boost
//			- 36 0C 00 - All chip power on
//			- 36 0E 01 - Slave, 16 bit, Left Justified
//			- 36 10 40 - Clock/2, 48k, Normal mode

	str[0] = 1;	str[1] = 3;	str[2] = 0x36;	str[3] = 0x1E;	str[4] = 0x00;
	if(USB_send_I2C(str, 5))
		printf("I2C error - Can't reset TLV320 \n");

	str[0] = 1;	str[1] = 3;	str[2] = 0x36;	str[3] = 0x12;	str[4] = 0x01;
	if(USB_send_I2C(str, 5))
		printf("I2C error - Can't set TLV320 active \n");

	str[0] = 1;	str[1] = 3;	str[2] = 0x36;	str[3] = 0x08;	str[4] = 0x15;
	if(USB_send_I2C(str, 5))
		printf("I2C error - Can't set TLV320 DA on etc \n");

	str[0] = 1;	str[1] = 3;	str[2] = 0x36;	str[3] = 0x0C;	str[4] = 0x00;
	if(USB_send_I2C(str, 5))
		printf("I2C error - Can't power on  TLV320  \n");

	str[0] = 1;	str[1] = 3;	str[2] = 0x36;	str[3] = 0x0E;	str[4] = 0x01;
	if(USB_send_I2C(str, 5))
		printf("I2C error - Can't set TLV320 Slave etc \n");

	str[0] = 1;	str[1] = 3;	str[2] = 0x36;	str[3] = 0x10;	str[4] = 0x40;
	if(USB_send_I2C(str, 5))
		printf("I2C error - Can't set TLV320 Clock etc \n");



// how to read from MCP23016 I/O port expander over I2C

	// read current status of I2C port expander

	input[0] = 1;	input[1] = 1;	input[2] = 0x41;	

	I2C_input = USB_receive_I2C(input,3);

	printf ("I2C data = %d\n", I2C_input);

//I2C functions

int USB_send_I2C(char* buf, int len)
{
	USB_BulkWrite(0, buf, (WORD)len);	// send I2C packet
	USB_BulkRead(1, buf, 64);		// get I2C response
	if (buf[0] & 0x80) 
]		return(1);			// Bus error
	if (buf[0] & 0x40)
		return(0);			// ACK received from USB device
	else 
		return(1);			// no ACK received 

}

char USB_receive_I2C(char* buf, int len)
{
	USB_BulkWrite(0, buf, (WORD)len);	// send I2C packet
	USB_BulkRead(1, buf, 64);		// get I2C response
	return(buf[1]);				// returns status and port value so read buf[1] as input
}

#endif