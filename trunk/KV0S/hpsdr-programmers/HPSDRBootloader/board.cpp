/*
 * File:   board.cpp
 * Author: John Melton, G0ORX/N6LYT
 *
 * Created on 23 November 2010
 *
 * Created on December 30, 2012
 * Author: Dave Larsen, KV0S
 */

/* Copyright (C)
* 2009 - John Melton, G0ORX/N6LYT
* 2012 - Dave Larsen, KV0S
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*
*/

#include "board.h"

Board::Board(long ipaddr,unsigned char* macaddr,unsigned char software_version,unsigned char board_type) {
    ipaddress=ipaddr;
    for(int i=0;i<6;i++) {
        macaddress[i]=macaddr[i];
    }
    version=software_version;
    board=board_type;
}


long Board::getIpAddress() {
    return ipaddress;
}

unsigned char* Board::getMACAddress() {
    return macaddress;
}

QString Board::getHostAddress() {
    QString address;
    address.sprintf("%ld.%ld.%ld.%ld",
                 (ipaddress>>24)&0xFF,(ipaddress>>16)&0xFF,(ipaddress>>8)&0xFF,ipaddress&0xFF);
    return address;
}

QString Board::toString() {
    QString text;
    text.sprintf("%02X:%02X:%02X:%02X:%02x:%02X (%ld.%ld.%ld.%ld) Software version: %d.%d (%s)",
                 macaddress[0],macaddress[1],macaddress[2],macaddress[3],macaddress[4],macaddress[5],
                 (ipaddress>>24)&0xFF,(ipaddress>>16)&0xFF,(ipaddress>>8)&0xFF,ipaddress&0xFF,
                 version/10,version%10,board==0?"Metis":"Hermes");
    return text;
}

unsigned char Board::getVersion() {
    return version;
}

unsigned char Board::getBoard() {
    return board;
}
