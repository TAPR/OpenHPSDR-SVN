#include "server.h"
#include "alex.h"

#include <QtNetwork/QUdpSocket>
#include <QDebug>
#include <QEventLoop>
#include <QTimer>

Server* Server::instance = NULL;

Server* Server::getInstance(){
  if(instance == NULL){
    instance = new Server();
  }
  return instance;
}

Server::Server() {
}



Interfaces* Server::getInterfaces() {
    return &interfaces;
}

QUdpSocket* Server::getMetisSocket() {
    return &metisSocket;
}

void Server::setDevice(QString d) {
    device=d;
}

void Server::setInterface(QString i) {
    iface=i;
}

void Server::setMetisDetail(QString m) {
    metisDetail=m;
}

QString Server::getDevice() {
    return device;
}

QString Server::getInterface() {
    return iface;
}

QString Server::getMetisDetail() {
    return metisDetail;
}

QString Server::getInterfaceIPAddress(QString iface) {
    return interfaces.getInterfaceIPAddress(iface);
}

QString Server::getMetisText(int index) {
    Board m=boards.at(index);
    return m.toString();
}

void Server::clearMetis() {
qDebug()<<"Server::clearMetis";
    boards.clear();
}

void Server::addMetis(Board bd) {
qDebug()<<"Server::addBoard";
    boards.append(bd);
    metisDetail=bd.toString();
}

int Server::getMetisCount() {
    return boards.count();
}


void Server::readyRead() {

    QHostAddress metisAddress;
    quint16 metisPort;
    unsigned char receiveBuffer[1032];
    qint64 length;

    unsigned long sequence;

    /*
    while(metisSocket.hasPendingDatagrams()) {

        if((length=metisSocket.readDatagram((char*)&receiveBuffer,(qint64)sizeof(receiveBuffer),&metisAddress,&metisPort))!=1032) {
            qDebug()<<"Error: Server: readDatagram failed "<<metisSocket.errorString();
            return;
        }

        //qDebug()<<"receiver"<<length<<"bytes: "<<receiveBuffer[0]<<","<<receiveBuffer[1]<<","<<receiveBuffer[2]<<","<<receiveBuffer[3];

        if(receiveBuffer[0]==0xEF && receiveBuffer[1]==0xFE) {
            // valid frame
            switch(receiveBuffer[2]) {
            case 1: // IQ data
                switch(receiveBuffer[3]) {
                case 4: // EP4 data
                    break;
                case 6: // EP6 data
                    sequence=((receiveBuffer[4]&0xFF)<<24)+((receiveBuffer[5]&0xFF)<<16)+((receiveBuffer[6]&0xFF)<<8)+(receiveBuffer[7]&0xFF);
                    if(receive_sequence==0) {
                        receive_sequence=sequence;
                    } else {
                        receive_sequence++;
                        if(receive_sequence!=sequence) {
                            //qDebug()<<"Sequence error: expected "<<receive_sequence<<" got "<<sequence;
                            receive_sequence=sequence;
                            receive_sequence_error++;
                        }
                    }
                    //process_iq_buffer(&receiveBuffer[8]);
                    //process_iq_buffer(&receiveBuffer[520]);
                    break;
                default:
                    qDebug()<<"invalid EP";
                    break;
                }
                break;
            default:
                qDebug()<<"Expected data packet (1) got "<<receiveBuffer[2];
                break;
            }
        } else {
            qDebug()<<"expected EFFE";
        }
    } */
}


void Server::clearError() {
    error.clear();
}

void Server::setError(QString e) {
    error.set(e);
}



