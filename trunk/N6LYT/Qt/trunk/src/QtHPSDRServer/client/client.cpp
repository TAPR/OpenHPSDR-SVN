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
                    response.append("OK ");

                    response.append("<radio>");
                    response.append("<type>");
                    if(server->getHPSDR()) {
                        response.append("HPSDR");
                    } else if(server->getHermes()) {
                        response.append("HERMES");
                    }
                    response.append("</type>");
                    response.append("<version>");
                    response.append(QString::number(server->getOzySoftwareVersion()));
                    response.append("</version>");

                    response.append("<samplerate>");
                    response.append(QString::number(server->getSampleRate()));
                    response.append("</samplerate>");

                    response.append("<receiver>");
                    response.append("<type>Mercury</type>");
                    response.append("<version>");
                    response.append(QString::number(server->getMercurySoftwareVersion()));
                    response.append("</version>");
                    response.append("<receivers>");
                    response.append(QString::number(server->getReceivers()));
                    response.append("</receivers>");
                    response.append("</receiver>");

                    response.append("<exciter>");
                    if(server->getPenelope()) {
                        response.append("<type>Penelope</type>");
                    } else if(server->getPennylane()) {
                        response.append("<type>Pennylane</type>");
                    }
                    response.append("<version>");
                    response.append(QString::number(server->getPenelopeSoftwareVersion()));
                    response.append("</version>");
                    response.append("</exciter>");

                    if(server->getAlex()) {
                        Alex* alex=Alex::getInstance();
                        response.append("<alex>");

                        for(int i=0;i<BANDS;i++) {
                            response.append("<"+alex->getBand(i)+">");
                            response.append("<rx>");
                            response.append(alex->getRxAntenna(i));
                            response.append("</rx>");
                            response.append("<tx>");
                            response.append(alex->getTxAntenna(i));
                            response.append("</tx>");
                            response.append("</"+alex->getBand(i)+">");
                        }

                        response.append("</alex>");
                    }
                    XVTR* xvtrs=server->getXvtrs();
                    XVTR* xvtr;
                    for(int i=0;i<4;i++) {
                        xvtr=&xvtrs[i];
                        if(xvtr->getLabel()!="") {
                            response.append("<xvtr>");
                            response.append("<entry>");
                            response.append(QString::number(i+BANDS));
                            response.append("</entry>");
                            response.append("<label>");
                            response.append(xvtr->getLabel());
                            response.append("</label>");
                            response.append("<minfreq>");
                            response.append(QString::number(xvtr->getMinFrequency()));
                            response.append("</minfreq>");
                            response.append("<maxfreq>");
                            response.append(QString::number(xvtr->getMaxFrequency()));
                            response.append("</maxfreq>");
                            response.append("<lofreq>");
                            response.append(QString::number(xvtr->getLOFrequency()));
                            response.append("</lofreq>");
                            response.append("<rxant>");
                            response.append(xvtr->getRxAntenna());
                            response.append("</rxant>");
                            response.append("<txant>");
                            response.append(xvtr->getTxAntenna());
                            response.append("</txant>");
                            response.append("</xvtr>");
                        }
                    }

                    response.append("</radio>");

                    qDebug()<<"response.length:"<<response.length();
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
            } else {
                response.append("Error: Invalid command");
                qDebug()<<"Invalid command:"<<buffer;
            }
            break;
        case 4:
            if(args[0]=="frequency") {
                qDebug()<<buffer;
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
                // args[1] is the receiver
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
