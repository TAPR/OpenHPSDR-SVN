/* HPSDR-USB - High Performance Software Defined Radio, USB support 
 *  
 * Copyright (C) 2009 Bill Tracey, KD5TFD  
 * Copyright (C) 2012 George Byrkit, K9TRV: add manifest to get 'administrator' privilege on Vista, Win7 and Win8
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


// sleep for a given number of millisecs 
// 
#include <stdio.h> 
#include <windows.h> 
#include <stdlib.h> 

char *copyright = "msecsleep - High Performance Software Defined Radio, USB support Copyright (C) 2009 Bill Tracey, KD5TFD";

void usageAndExit() { 
	printf("usage: msecsleep N\n    N is number of millisecs to sleep.\n"); 
	exit(1); 
} 
int main(int argc, char *argv[]) { 
	int msecs; 
	if ( argc != 2 ) { 
		usageAndExit(); 
	} 
	msecs = atoi(argv[1]); 
	if  ( msecs <= 0 ) { 
		printf("Invalid msec count: \'%s\'\n", argv[1]); 
		usageAndExit(); 
	} 
	Sleep(msecs); 
	exit(0); 
	
} 