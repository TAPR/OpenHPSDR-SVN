#include "client.h"
#include "../hpsdr/server.h"
#include "../hpsdr/xvtr.h"
#include "../hpsdr/alex.h"

#include <QStringList>

/*
  attach <rx>
      OK <radio><type>HPSDR|HERMES</type><receivers>1|2|3|4</receivers><exciter>Penelope|Pennylane</exciter><samplerate>48000|96000|192000></samplerate><xvtr><label>LABEL</label><minfreq>Hz</minfreq><maxfreq>Hz</maxfreq></xvtr></radio>
  detach <rx>
      OK
  client <rx> <client type>
      OK
  start <rx> <iq|bandscope> <remote address> <remote port>
      OK
  stop <rx> <iq|bandscope>
      OK
  frequency <rx> <frequency> [<band>]
      OK
  mox <rx> <state>
      OK
*/

Client::Client(QTcpSocket* s,QObject *parent) :
    QObject(parent)
{
    socket=s;
    connect(s,SIGNAL(readyRead()),this,SLOT(readyRead()));
    connect(s,SIGNAL(disconnected()),this,SLOT(disconnected()));
    frequency=7056000;
    band=HAM_BAND_40;
}

void Client::disconnected() {
    socket->close();
    socket=NULL;
    emit clientDisconnected(this);
}

QHostAddress Client::getPeerAddress() {
    QHostAddress a;
    if(socket!=NULL) a=socket->peerAddress();
    return a;
}

void Client::readyRead() {
    Server* server=Server::getInstance();
    QString response;
    while(socket->canReadLine()) {
        QString buffer(socket->readLine(1024));
        buffer.chop(1);  //remove nl
        //qDebug()<<"Client::readyRead:"<<buffer;
        QStringList args=buffer.split(" ");
        switch(args.length()) {
        case 2:
            if(args[0]=="attach") {
                // args[1] receiver
                if(server->attachTo(this,args[1].toInt())!=0) {
                    response.append("Error: Service unavailable");
                } else {
                    response.append("OK:");
                    response.append(server->getConfig());
                }
            } else if (args[0]=="detach") {
                // args[1] receiver
                if(server->detachFrom(this,args[1].toInt())!=0) {
                    response.append("Error: Service unavailable");
                } else {
                    response.append("OK");
                }
            } else {
                response.append("Error: Invalid command");
                qDebug()<<"Invalid command:"<<buffer;
            }
            break;
        case 3:
            if (args[0]=="frequency") {
                qDebug()<<buffer;
                // superceeded by version below in case 4:
                // args[1] receiver
                // args[2] frequency
                server->setFrequency(this,args[1].toInt(),args[2].toLong());
                response.append("OK");
            } else if (args[0]=="mox") {
                // args[1] receiver
                // args[2] state
                server->setMox(this,args[1].toInt(),args[2].toInt());
                response.append("OK");
            } else if (args[0]=="stop") {
                // args[1] receiver
                // args[2] is the data type (iq or bandscope)
                server->stopReceiver(this,args[1].toInt());
                response.append("OK");
            } else if (args[0]=="client") {
                // args[1] receiver
                // args[2] client type
                server->setClientType(this,args[1].toInt(),args[2]);
                response.append("OK");
            } else if (args[0]=="hardware") {
                server->setHardware(args[1],args[2]);
                response.append("OK");
            } else {
                response.append("Error: Invalid command");
                qDebug()<<"Invalid command:"<<buffer;
            }
            break;
        case 4:
            if(args[0]=="frequency") {
                // args[1] receiver
                // args[2] frequency
                // args[3] band (for alex control)
                server->setFrequencyAndBand(this,args[1].toInt(),args[2].toLong(),args[3].toInt());
                response.append("OK");
            } else {
                response.append("Error: Invalid command");
                qDebug()<<"Invalid command:"<<buffer;
            }
            break;
        case 5:
            if (args[0]=="start") {
                // args[1] is the receiver (not used if data type os bandscope)
                // args[2] is the data type (iq or bandscope)
                // args[3] is the remote address to send to
                // args[4] is the remote port to send to
                server->startReceiver(this,args[1].toInt(),QHostAddress(args[3]),args[4].toInt());
                response.append("OK");
            } else {
                response.append("Error: Invalid command");
                qDebug()<<"Invalid command:"<<buffer;
            }
            break;
        default:
            response.append("Error: Invalid command");
            qDebug()<<"Invalid command:"<<buffer;
            break;

        }
        response.append("\n");
        socket->write(response.toLocal8Bit());
    }
}
