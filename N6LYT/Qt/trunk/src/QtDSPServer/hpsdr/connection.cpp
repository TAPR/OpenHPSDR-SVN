#include "connection.h"

#include <QHostAddress>
#include <QStringList>

#include <QDebug>

Connection* Connection::instance = NULL;

Connection* Connection::getInstance() {
    if(instance==NULL) {
        instance=new Connection();
    }
    return instance;
}

Connection::Connection() {
qDebug()<<"Connection::Connection";
    socket=new QTcpSocket(this);
    connected=false;
    messageSent=false;
    connect(this,SIGNAL(sendNext()),this,SLOT(sendNextCommand()));
}

void Connection::isConnected() {
    qDebug()<<"Connection::isConnected";
    connected=true;
    emit sendNext();
}

void Connection::isDisconnected() {
    qDebug()<<"Connection::isDisconnected";
    connected=false;
}

void Connection::setConnection(QString host,int port) {
    qDebug()<<"Connection::setConnection: "<<host<<":"<<port;
    connect(socket,SIGNAL(connected()),this,SLOT(isConnected()));
    connect(socket,SIGNAL(readyRead()),this,SLOT(readyRead()));
    connect(socket,SIGNAL(disconnected()),this,SLOT(isDisconnected()));
    socket->connectToHost(QHostAddress(host),port);
}

void Connection::sendNextCommand() {
    if(!waiting.isEmpty()) {
        QString command=waiting.first();
        command.append("\n");
        socket->write(command.toLocal8Bit());
        pending.append(command);
        waiting.removeFirst();
        messageSent=true;
    }
}

void Connection::sendCommand(QString command) {
    mutex.lock();
    if(connected) {
        if(pending.size()==0) {
            command.append("\n");
            socket->write(command.toLocal8Bit());
            pending.append(command);
            messageSent=true;
        } else {
            waiting.append(command);
        }
    } else {
        qDebug()<<"Connection::sendCommand: not connected - appending to waiting!!";
        waiting.append(command);
    }
    mutex.unlock();
}

void Connection::readyRead() {
    QString buffer(socket->readLine(1024));
    buffer.chop(1); // remove nl
    QStringList args=buffer.split(" ");

    if(args.length()<=0) {
        qDebug()<<"Connection::readyRead: no args!";
        return;
    }
    if(pending.isEmpty()) {
        qDebug()<<"Connection::readyRead: none pending!";
        return;
    }

    pending.removeFirst();

    if(args[0]=="OK") {
        if(args.length()==2) {
            // must be sampleRate
            sampleRate=args[1].toInt();
        }
    } else {
        qDebug()<<"Connection::readReady: "<<buffer;
    }

    emit sendNext();
}

int Connection::getSampleRate() {
    return sampleRate;
}
