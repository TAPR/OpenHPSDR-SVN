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

Connection* Connection::instance=NULL;

Connection* Connection::getInstance() {
    if(instance==NULL) {
        instance=new Connection();
    }
    return instance;
}

Connection::Connection() {
    //qDebug() << "Connection::Connection";
    tcpSocket=NULL;
    state=READ_HEADER;
    bytes=0;
    hdr=(char*)malloc(HEADER_SIZE);
}

Connection::~Connection() {
}

void Connection::connect(QString h,int p) {
    host=h;
    port=p;
    tcpSocket=new QTcpSocket(this);

    QObject::connect(tcpSocket, SIGNAL(error(QAbstractSocket::SocketError)),
            this, SLOT(socketError(QAbstractSocket::SocketError)));

    QObject::connect(tcpSocket, SIGNAL(connected()),
            this, SLOT(connected()));

    QObject::connect(tcpSocket, SIGNAL(disconnected()),
            this, SLOT(disconnected()));

    QObject::connect(tcpSocket, SIGNAL(readyRead()),
            this, SLOT(socketData()));

    qDebug() << "Connection::connect: connectToHost: " << host << ":" << port;
    tcpSocket->connectToHost(host,port);


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
    mutex.lock();
    command.append("\n");
    if(tcpSocket!=NULL) {
        tcpSocket->write(command.toLocal8Bit());
        tcpSocket->flush();
    }
    mutex.unlock();
}

void Connection::socketData() {

    int toRead;
    int bytesRead=0;
    int thisRead;

    toRead=tcpSocket->bytesAvailable();

    while(bytesRead<toRead) {
        switch(state) {
        case READ_HEADER:
            thisRead=tcpSocket->read(&hdr[bytes],HEADER_SIZE-bytes);
            bytes+=thisRead;
            if(bytes==HEADER_SIZE) {
                length=((hdr[3]&0xFF)<<8)+(hdr[4]&0xFF);
                buffer=(char*)malloc(length);
                bytes=0;
                state=READ_BUFFER;
            }
            break;
        case READ_BUFFER:
            thisRead=tcpSocket->read(&buffer[bytes],length-bytes);
            bytes+=thisRead;
            if(bytes==length) {
                queue.enqueue(new Buffer(hdr,buffer));
                QTimer::singleShot(0,this,SLOT(processBuffer()));
                hdr=(char*)malloc(HEADER_SIZE);
                bytes=0;
                state=READ_HEADER;
            }
            break;
        }
        bytesRead+=thisRead;
    }
}

void Connection::processBuffer() {
    Buffer* buffer;
    char* nextHeader;
    char* nextBuffer;

    buffer=queue.dequeue();
    nextHeader=buffer->getHeader();
    nextBuffer=buffer->getBuffer();

    // emit a signal to show what buffer we have
    if(nextHeader[0]==SPECTRUM_BUFFER) {
        emit spectrumBuffer(nextHeader,nextBuffer);
    } else if(nextHeader[0]==CONFIG_BUFFER) {
        emit configBuffer(nextHeader,nextBuffer);
    } else {
        qDebug() << "Connection::socketData: invalid header: " << nextHeader[0];
    }
}

void Connection::freeBuffers(char* header,char* buffer) {
    free(header);
    free(buffer);
}
