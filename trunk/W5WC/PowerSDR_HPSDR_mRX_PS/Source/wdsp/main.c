/*  main.c

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

#include "comm.h"

void main (void *pargs)
{
	int channel = (int)pargs;
	switch (ch[channel].type)
	{
	case 0:	// rxa
		while (_InterlockedAnd (&ch[channel].run, 1))
		{
			WaitForSingleObject(ch[channel].iob.pd->Sem_BuffReady,INFINITE);
			EnterCriticalSection (&ch[channel].csDSP);
			dexchange (channel, rxa[channel].outbuff, rxa[channel].inbuff);
			xrxa (channel);
			LeaveCriticalSection (&ch[channel].csDSP);
		}
		break;
	case 1:  // txa
		while (_InterlockedAnd (&ch[channel].run, 1))
		{
			WaitForSingleObject(ch[channel].iob.pd->Sem_BuffReady,INFINITE);
			EnterCriticalSection (&ch[channel].csDSP);
			dexchange (channel, txa[channel].outbuff, txa[channel].inbuff);
			xtxa (channel);
			LeaveCriticalSection (&ch[channel].csDSP);
		}
		break;
	case 31: // 
		while (_InterlockedAnd (&ch[channel].run, 1))
		{
			
		}
		break;
	}
	_endthread();
}

void create_main (int channel)
{
	switch (ch[channel].type)
	{
	case 0:
		create_rxa (channel);
		break;
	case 1:
		create_txa (channel);
		break;
	case 31:  //
		
		break;
	}
}

void destroy_main (int channel)
{
	switch (ch[channel].type)
	{
	case 0:
		destroy_rxa (channel);
		break;
	case 1:
		destroy_txa (channel);
		break;
	case 31:  //
		
		break;
	}
}

void flush_main (int channel)
{
	switch (ch[channel].type)
	{
	case 0:
		flush_rxa (channel);
		break;
	case 1:
		flush_txa (channel);
		break;
	case 31:
		
		break;
	}
}
