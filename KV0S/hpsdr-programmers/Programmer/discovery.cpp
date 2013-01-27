/*
 * File:   discovery.cpp
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


#include <QDebug>

#include "discovery.h"
#include "board.h"

/*! \brief Discovery(QUdpSocket* s, QString myip)
 *
 *  \param QUdpSocket* s
 *  \param QString myip
 *
 *  This constructor function for Discovery class.
 */

Discovery::Discovery(QUdpSocket* s,QString myip) {
    qDebug()<<"Discovery: "<<myip;
    ip=myip;
    socket=s;

    connect(socket,SIGNAL(readyRead()),this,SLOT(readyRead()));
}

/*! \brief discover()
 *
 *  This function send the discovery packet.
 */

void Discovery::discover() {
    // send the discovery packet
    unsigned char buffer[63];
    int i;

    buffer[0]=(char)0xEF; //header
    buffer[1]=(char)0XFE;
    buffer[2]=(char)0x02;
    for(i=3;i<63;i++) {
        buffer[i]=(char)0x00;
    }

    if(socket->writeDatagram((const char*)buffer,sizeof(buffer),QHostAddress::Broadcast,1024)<0) {
        qDebug()<<"Error: Discovery: writeDatagram failed "<<socket->errorString();
        return;
    }
}

/*! \brief stop()
 *
 *  This function disconnect the socket.
 */

void Discovery::stop() {
    disconnect(socket,SIGNAL(readyRead()),this,SLOT(readyRead()));
}

/*! \brief readyRead()
 *
 *  This function SLOT to process the socket data.
 */

void Discovery::readyRead() {

    QHostAddress metisAddress;
    quint16 metisPort;
    unsigned char buffer[1024];

    qDebug()<<"Discovery::readyRead";

    if(socket->readDatagram((char*)&buffer,(qint64)sizeof(buffer),&metisAddress,&metisPort)>0) {

        if(buffer[0]==0xEF && buffer[1]==0xFE) {
            switch(buffer[2]) {
            case 3:  // reply
                // should not happen on this port
                qDebug()<<"Discovery::readyRead: reply!!!";
                break;
            case 2:  // response to a discovery packet
                qDebug()<<"Discovery::readyRead: discovery response: "<<metisAddress.toString();
                if(metisAddress.toString()!=ip) {
                    Board* bd=new Board(metisAddress.toIPv4Address(),&buffer[3],buffer[9],buffer[10]);
                    emit board_found(bd);
                } else {
                    qDebug()<<"Discovery::readyRead: from: "<<metisAddress.toString();
                }
                break;
            case 1:  // a data packet
                // should not happen on this port
                qDebug()<<"Discovery::readyRead: data!!!";
                break;
            }
        } else {
            qDebug() << "received invalid response to discovery";
        }
    } else {
        qDebug()<<"Discovery::readyRead: readDatagram failed";
    }
}
