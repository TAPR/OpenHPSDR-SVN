#include "discovery.h"

#include <QEventLoop>
#include <QTimer>

Discovery::Discovery(Server* s) {

    int i;

    qDebug()<<"Discovery::Discovery";
    server=s;
    server->clearMetis();

    if(!socket.bind(QHostAddress(server->getInterfaceIPAddress(server->getInterface())),1024,QUdpSocket::ReuseAddressHint)) {
        server->setError("Error: Discovery: bind failed");
        qDebug()<<"Error: Discovery: bind failed "<<socket.errorString();
        return;
    }

    connect(&socket,SIGNAL(readyRead()),this,SLOT(readyRead()));

    unsigned char buffer[63];

    buffer[0]=(char)0xEF;
    buffer[1]=(char)0XFE;
    buffer[2]=(char)0x02;
    for(i=3;i<63;i++) {
        buffer[i]=(char)0x00;
    }

    if(socket.writeDatagram((const char*)buffer,sizeof(buffer),QHostAddress::Broadcast,1024)<0) {
        qDebug()<<"Error: Discovery: writeDatagram failed "<<socket.errorString();
        return;
    }

    // wait for 1 second before returning
    QEventLoop loop;
    QTimer *timer = new QTimer(this);
    connect(timer, SIGNAL(timeout()), &loop, SLOT(quit()));
    timer->start(1000);

    qDebug()<<"start 1 second timeout loop";
    // Execute the event loop here, now we will wait here until readyRead() signal is emitted
    // which in turn will trigger event loop quit.
    loop.exec();

    qDebug()<<"Closing discovery socket";
    socket.close();

}

void Discovery::readyRead() {

    QHostAddress metisAddress;
    quint16 metisPort;
    unsigned char buffer[1024];

    if(socket.readDatagram((char*)&buffer,(qint64)sizeof(buffer),&metisAddress,&metisPort)<0) {
        qDebug()<<"Error: Discovery: readDatagram failed "<<socket.errorString();
        return;
    }

    if(buffer[0]==0xEF && buffer[1]==0xFE) {
        switch(buffer[2]) {
            case 3:  // reply
                // should not happen on this port
                break;
            case 2:  // response to a discovery packet
                if(metisAddress.toString()!=server->getInterfaceIPAddress(server->getInterface())) {
                    Metis* metis=new Metis(metisAddress.toIPv4Address(),&buffer[3]);
                    server->addMetis(*metis);
                }
                break;
            case 1:  // a data packet
               // should not happen on this port
               break;
        }
    } else {
        qDebug() << "received invalid response to discovery";
    }

}
