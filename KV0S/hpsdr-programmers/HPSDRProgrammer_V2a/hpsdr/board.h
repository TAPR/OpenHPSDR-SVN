/*
 * File:   board.h
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


#ifndef BOARD_H
#define BOARD_H

#include <QString>
#include <QHash>
#include <QDebug>
#include <QHostAddress>

// command codes
#define PROGRAM_METIS_FLASH 0x01
#define ERASE_METIS_FLASH   0x02
#define READ_METIS_MAC      0x03
#define READ_METIS_IP       0x04
#define WRITE_METIS_IP      0x05
#define GET_JTAG_DEVICE_ID  0x06
#define PROGRAM_MERCURY     0x07
#define PROGRAM_PENELOPE    0x08
#define JTAG_ERASE_FLASH    0x09
#define PROGRAM_FLASH       0x0A

// reply codes
#define INVALID_COMMAND  0x00
#define ERASE_DONE       0x01
#define SEND_MORE        0x02
#define HAVE_MAC_ADDRESS 0x03
#define HAVE_IP_ADDRESS  0x04
#define FPGA_ID          0x05


class Board
{
public:
    Board(quint32 ipaddr,unsigned char* macaddr,unsigned char software_version,unsigned char board_type);
    QHash<int, QString> boardtype;
    QHash<int, QString> jumper;
    QHostAddress getIpAddress();
    unsigned char* getMACAddress();
    QString getHostAddress();
    QString toAllString();
    QString toIPString();
    QString toIPPartString(int id);
    QString toMACString();
    QString getBoardString();
    unsigned char getVersion();
    unsigned char getBoard();
    QString getJumper();

private:
    quint32 ipaddress;
    unsigned char macaddress[6];
    unsigned char version;
    unsigned char board;
};

#endif // BOARD_H
