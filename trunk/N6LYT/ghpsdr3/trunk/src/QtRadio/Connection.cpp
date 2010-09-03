/* 
 * File:   Connection.cpp
 * Author: John Melton, G0ORX/N6LYT
 * 
 * Created on 16 August 2010, 07:40
 */

/* Copyright (C)
* 2009 - John Melton, G0ORX/N6LYT
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

#include "Connection.h"

Connection::Connection() {
    //qDebug() << "Connection::Connection";
    tcpSocket=NULL;
}

Connection::Connection(const Connection& orig) {
}

Connection::~Connection() {
}

void Connection::connect(QString h,int r) {
    host=h;
    receiver=r;
    tcpSocket=new QTcpSocket(this);

    QObject::connect(tcpSocket, SIGNAL(error(QAbstractSocket::SocketError)),
            this, SLOT(socketError(QAbstractSocket::SocketError)));

    QObject::connect(tcpSocket, SIGNAL(connected()),
            this, SLOT(connected()));

    QObject::connect(tcpSocket, SIGNAL(disconnected()),
            this, SLOT(disconnected()));

    QObject::connect(tcpSocket, SIGNAL(readyRead()),
            this, SLOT(socketData()));

    //qDebug() << "Connection::connect: connectToHost";
    tcpSocket->connectToHost(host,receiver+DSPSERVER_BASE_PORT);


}

void Connection::disconnected() {
    emit disconnected("Remote disconnected");
}

void Connection::disconnect() {

    //qDebug() << "Connection::disconnect";
    if(tcpSocket!=NULL) {
        tcpSocket->close();
        tcpSocket=NULL;
    }
}

void Connection::socketError(QAbstractSocket::SocketError socketError) {
    switch (socketError) {
        case QAbstractSocket::RemoteHostClosedError:
            qDebug() << "Remote closed connection";
            break;
        case QAbstractSocket::HostNotFoundError:
            qDebug() << "Host not found";
            break;
        case QAbstractSocket::ConnectionRefusedError:
            qDebug() << "Remote host refused connection";
            break;
        default:
            qDebug() << "Socket Error: " << tcpSocket->errorString();
    }

    emit disconnected(tcpSocket->errorString());
    tcpSocket=NULL;
}

void Connection::connected() {
    //qDebug() << "Connected" << tcpSocket->isValid();
    emit isConnected();
}

void Connection::sendCommand(QString command) {
    char buffer[32];

    if(tcpSocket!=NULL) {
        mutex.lock();
        //qDebug() << "sendCommand:" << command;
        strcpy(buffer,command.toUtf8().constData());
        tcpSocket->write(buffer,32);
        tcpSocket->flush();
        mutex.unlock();
    }
}

void Connection::socketData() {
    char* header;
    char* buffer;
    int bytes;
    int length;

    while(tcpSocket->bytesAvailable()>=HEADER_SIZE) {
        // read the header
        header=(char*)malloc(HEADER_SIZE);
        bytes=0;
        while(bytes<HEADER_SIZE) {
            bytes+=tcpSocket->read(&header[bytes],HEADER_SIZE-bytes);
        }
        // read the data
        length = atoi(&header[26]);
        buffer=(char*)malloc(length);
        bytes=0;
        while(bytes<length) {
            bytes+=tcpSocket->read(&buffer[bytes],length-bytes);
        }
        // emit a signal to show what buffer we have
        if(header[0]==0) {
            emit audioBuffer(header,buffer);
        } else if(header[0]==1) {
            emit spectrumBuffer(header,buffer);
        } else {
            qDebug() << "Connection::socketData: invalid header: " << header[0];
        }
    }
}

void Connection::freeBuffers(char* header,char* buffer) {
    free(header);
    free(buffer);
}

QString Connection::getHost() {
    return host;
}

int Connection::getReceiver() {
    return receiver;
}
