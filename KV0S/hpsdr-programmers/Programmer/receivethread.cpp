/*
 * File:  receivethread.cpp
 *
 * Author: John Melton, G0ORX/N6LYT
 *
 * Created on 23 November 2010
 *
 * Revised on December 30, 2012
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


#include <errno.h>

#include <QDebug>

#include "receivethread.h"
#include "mainwindow.h"

ReceiveThread::ReceiveThread(QUdpSocket* s,QString myip,QString metis) {
    qDebug()<<"ReceiveThread: "<<myip;
    metisip=metis;
    socket=s;

    connect(socket,SIGNAL(readyRead()),this,SLOT(readyRead()));
}

void ReceiveThread::send(const char* buffer,int length) {
    qDebug() << "in send";
    if(socket->writeDatagram(buffer,length,QHostAddress(metisip),1024)<0) {
        qDebug()<<"Error: Discovery: writeDatagram failed "<<socket->errorString();
        return;
    }
}

void ReceiveThread::readyRead() {

    QHostAddress metisAddress;
    quint16 metisPort;
    unsigned char buffer[1024];

    qDebug()<<"Discovery::readyRead";

    if(socket->readDatagram((char*)&buffer,(qint64)sizeof(buffer),&metisAddress,&metisPort)>0) {

        if(buffer[0]==0xEF && buffer[1]==0xFE) {
            switch(buffer[2]) {
            case 2:
                qDebug() << "call Discovery";
                emit discover();
                break;
            case 3:  // erase completed
                qDebug()<<"commandCompleted";
                emit eraseCompleted();
                break;
            case 4:  // ready for next buffer
                qDebug()<<"ready for next buffer";
                emit nextBuffer();
                break;
            default:
                qDebug()<<"invalid reply="<< buffer[2] << buffer[3];
                break;
            }
        } else {
            qDebug() << "received invalid response in ReceiveThread";
        }
    } else {
        qDebug()<<"ReceiveThread::readyRead: readDatagram failed";
    }
}

void ReceiveThread::stop() {
    disconnect(socket,SIGNAL(readyRead()),this,SLOT(readyRead()));
}
