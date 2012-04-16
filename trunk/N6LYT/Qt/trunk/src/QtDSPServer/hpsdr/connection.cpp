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
    configuration=NULL;
}

void Connection::isConnected() {
    qDebug()<<"Connection::isConnected";
    connected=true;
    emit sendNext();
}

void Connection::isDisconnected() {
    qDebug()<<"Connection::isDisconnected";
    connected=false;
    emit disconnected();
}

void Connection::setConnection(QString host,int port) {
    qDebug()<<"Connection::setConnection: "<<host<<":"<<port;
    connect(socket,SIGNAL(connected()),this,SLOT(isConnected()));
    connect(socket,SIGNAL(readyRead()),this,SLOT(readyRead()));
    connect(socket,SIGNAL(disconnected()),this,SLOT(isDisconnected()));
    socket->connectToHost(QHostAddress(host),port);
}

QString Connection::getConfiguration() {
    return stringConfiguration;
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
    QString buffer(socket->readLine(2048));
    buffer.chop(1); // remove nl
    QStringList args=buffer.split(":");
    QDomNodeList elements;

    if(args.length()<=0) {
        qDebug()<<"Connection::readyRead: no args!";
        return;
    }
    if(pending.isEmpty()) {
        qDebug()<<"Connection::readyRead: none pending!";
        return;
    }

    pending.removeFirst();

    switch(args.length()) {
    case 0:
        qDebug()<<"response had no args";
        break;
    case 1:
        // just OK
        break;
    case 2:
        // OK and configuration
        stringConfiguration=args[1];
        qDebug()<<"readRead: config.length:"<<stringConfiguration.length();
        qDebug()<<"readyRead: config:"<<stringConfiguration;
        configuration=new QDomDocument();
        configuration->setContent(stringConfiguration);
        elements=configuration->elementsByTagName("samplerate");
        if(elements.length()>=1) {
            sampleRate=elements.at(0).toElement().text().toInt();
            qDebug()<<"samplerate:"<<sampleRate;
        } else {
            qDebug()<<"missing samplerate. default to 96000";
            sampleRate=96000;
        }
        emit setSampleRate(sampleRate);
        break;
    default:
        qDebug()<<"Connection::readReady: "<<buffer;
        break;

    }

    emit sendNext();
}

int Connection::getSampleRate() {
    return sampleRate;
}
