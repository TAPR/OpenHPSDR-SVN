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

/*! \brief Board(long ipaddr, unsigned char* macaddr, unsigned char software_version, unsigned char board_type)
 *
 *  \param long ipaddr
 *  \param unsigned char* macaddr
 *  \param unsigned char software_version
 *  \param unsigned char board_type
 *
 *  This constructor function for Board class.  These are the network information for
 *  the discovered HPSDR interface boards.
 */

Board::Board(quint32 ipaddr,unsigned char* macaddr,unsigned char software_version,unsigned char board_type) {
    ipaddress=ipaddr;
    for(int i=0;i<6;i++) {
        macaddress[i]=macaddr[i];
    }
    version=software_version;
    board=board_type;

    boardtype[0] = "metis";
    boardtype[1] = "hermes";
    boardtype[2] = "griffin";
    boardtype[4] = "angelia";
    boardtype[5] = "orion";

    jumper[0] = "J1";
    jumper[1] = "J12";
    jumper[2] = "J";
    jumper[4] = "J17";
    jumper[5] = "J17";
}

/*! \brief getIpAddress()
 *
 *
 *  This function returns the IP address of the found HPSDR board.
 */

QHostAddress Board::getIpAddress() {
    return QHostAddress(ipaddress);
}

/*! \brief getMACAddress()
 *
 *  This function returns the MAC address of the found HPSDR board.
 */

unsigned char* Board::getMACAddress() {
    return macaddress;
}

/*! \brief getHostAddress()
 *
 *  This function returns the Host address.
 */

QString Board::getHostAddress() {
    QString address;
    address.sprintf("%d.%d.%d.%d",
                 (ipaddress>>24)&0xFF,(ipaddress>>16)&0xFF,(ipaddress>>8)&0xFF,ipaddress&0xFF);
    return address;
}

/*! \brief toAllString()
 *
 *  This function formats the stored data into a QString.
 */


QString Board::toAllString() {
    QString text;
    text.sprintf("%02X:%02X:%02X:%02X:%02x:%02X (%d.%d.%d.%d) Software version: %d.%d (%s)",
                 macaddress[0],macaddress[1],macaddress[2],macaddress[3],macaddress[4],macaddress[5],
                 (ipaddress>>24)&0xFF,(ipaddress>>16)&0xFF,(ipaddress>>8)&0xFF,ipaddress&0xFF,
                 version/10,version%10,boardtype[board].toStdString().c_str());
    qDebug() << board << boardtype[0];

    return text;
}

/*! \brief toIPString()
 *
 *  This function formats the stored data into a QString.
 */


QString Board::toIPString() {
    QString text;
    text.sprintf("%d.%d.%d.%d",
                 (ipaddress>>24)&0xFF,(ipaddress>>16)&0xFF,(ipaddress>>8)&0xFF,ipaddress&0xFF);
    return text;
}

/*! \brief toIPString()
 *
 *  This function formats the stored data into a QString part of each of the 4 numbers.
 */


QString Board::toIPPartString(int id) {
    QString text;
    if( id == 1){
        text.sprintf("%d",(ipaddress>>24)&0xFF);
    }else if ( id == 2){
        text.sprintf("%d",(ipaddress>>16)&0xFF);
    }else if ( id == 3){
        text.sprintf("%d",(ipaddress>>8)&0xFF);
    }else if ( id == 4){
        text.sprintf("%d",ipaddress&0xFF);
    }
    return text;
}

/*! \brief toMACString()
 *
 *  This function formats the stored data into a QString.
 */


QString Board::toMACString() {
    QString text;
    text.sprintf("%02X:%02X:%02X:%02X:%02x:%02X",
                 macaddress[0],macaddress[1],macaddress[2],macaddress[3],macaddress[4],macaddress[5]);
    return text;
}

/*! \brief getVersion()
 *
 *  This function returns the board version number.
 */

unsigned char Board::getVersion() {
    return version;
}

/*! \brief getVersion()
 *
 *  This function returns the board version number.
 */

//unsigned char Board::getVersionString() {
//    return version;
//}

/*! \brief getBoard()
 *
 *  This function returns the type of board.
 */


unsigned char Board::getBoard() {
    return board;
}

/*! \brief getBoard()
 *
 *  This function returns the type of board.
 */


QString Board::getJumper() {
    return jumper[board];
}
/*! \brief getBoardString()
 *
 *  This function returns the type of board.
 */


QString Board::getBoardString() {
    return boardtype[board];
}
