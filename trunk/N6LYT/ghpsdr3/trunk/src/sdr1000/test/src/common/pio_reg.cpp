//=================================================================
// pio_reg.cpp
//=================================================================
// This file is part of a Software Defined Radio.
// Copyright (C) 2006, 2007  FlexRadio Systems
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// You may contact us via email at: sales@flex-radio.com.
// Paper mail may be sent to: 
//    FlexRadio Systems
//    12100 Technology Blvd.
//    Austin, TX 78727
//    USA
//=================================================================

#include <pio_reg.h>

PIOReg::PIOReg(char* name, SDR1000* sdr, uint8 addr)
{
	this->name = name;
	this->sdr = sdr;
	this->addr = addr;
	hw_data = sw_data = 0;
	update_hw = FALSE;
}

PIOReg::PIOReg(char* name, SDR1000* sdr, uint8 addr, uint8 init_val)
{
	this->name = name;
	this->sdr = sdr;
	this->addr = addr;
	hw_data = sw_data = init_val;
	update_hw = FALSE;
}

int32 PIOReg::Write(uint32 new_data)
{
	if(sdr->usb)
	{
		uint8 tmp = 0;
		switch(addr)
		{
			case PIO_IC01: tmp = 1; break;
			case PIO_IC03: tmp = 3; break;
			default: tmp = 0; break;
		}
		sdr->sdr1kusb->Latch(tmp, new_data);
	}
	else
	{
		sdr->Latch(addr, new_data);
	}
	return 0;
}
