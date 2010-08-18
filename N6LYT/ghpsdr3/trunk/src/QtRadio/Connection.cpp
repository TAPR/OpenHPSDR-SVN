/* 
 * File:   Connection.cpp
 * Author: john
 * 
 * Created on 16 August 2010, 07:40
 */

#include "Connection.h"

Connection::Connection() {
    qDebug() << "Connection::Connection";
    tcpSocket=NULL;
    sem.release(1);
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

    QObject::connect(tcpSocket, SIGNAL(readyRead()),
            this, SLOT(socketData()));

    qDebug() << "Connection::connect: connectToHost";
    tcpSocket->connectToHost(host,receiver+DSPSERVER_BASE_PORT);
}

void Connection::disconnect() {

    tcpSocket->close();
    tcpSocket=NULL;
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
    qDebug() << "Connected" << tcpSocket->isValid();
    emit isConnected();
}

void Connection::sendCommand(QString command) {
    char buffer[32];

    //qDebug() << "Connection::sendCommand: " << command;
    if(tcpSocket!=NULL) {
        sem.acquire(1);
        //qDebug() << "sendCommand:" << command;
        strcpy(buffer,command.toUtf8().constData());
        //qDebug() << "sendCommand: buffer:" << buffer;
        tcpSocket->write(buffer,32);
        tcpSocket->flush();
        sem.release(1);
    }
}

void Connection::socketData() {
    char buffer[HEADER_SIZE];

    tcpSocket->read(buffer,HEADER_SIZE);

    //qDebug() << "Connection::socketData: " << (int)buffer[0];

    emit header(&buffer[0]);
}

void Connection::read(char* buffer,int length) {
    if(tcpSocket!=NULL) {
        tcpSocket->read(buffer,length);
    }
}

QString Connection::getHost() {
    return host;
}

int Connection::getReceiver() {
    return receiver;
}