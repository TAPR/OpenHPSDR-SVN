/*  cblock.h

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2013 Warren Pratt, NR0V

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

The author can be reached by email at  

warren@wpratt.com

*/

#ifndef _cblock_h
#define _cblock_h

typedef struct _cbl
{
	int buff_size;						//buffer size
	COMPLEX *in_buff;					//pointer to input buffer
	COMPLEX *out_buff;					//pointer to output buffer
	int mode;
	float sample_rate;					//sample rate
	double dcI;							//dc component in I
	double dcQ;							//dc component in Q
	double tau;							//carrier removal time constant
	double mtau;						//carrier removal multiplier
	double onem_mtau;					//1.0 - carrier_removal_multiplier
}cbl, *CBL;

extern CBL newCBL
	(
	int buff_size,
	COMPLEX *in_buff,
	COMPLEX *out_buff,
	int mode,
	float sample_rate,
	double tau,
	char *tag
	);

extern void del_cbl(CBL a);

extern void c_block(CBL a);

#endif