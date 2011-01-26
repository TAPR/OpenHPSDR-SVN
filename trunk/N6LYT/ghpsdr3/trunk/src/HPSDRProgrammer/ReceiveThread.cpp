#include <errno.h>

#include "QDebug"

#include "ReceiveThread.h"
#include "mainwindow.h"

ReceiveThread::ReceiveThread(long metisIP,int receiveSocket) {

    qDebug()<<"ReceiveThread";

    s=receiveSocket;

    QString text;
    text.sprintf("ReceiveThread: metisIP %ld.%ld.%ld.%ld)",
                 metisIP&0xFF,(metisIP>>8)&0xFF,(metisIP>>16)&0xFF,(metisIP>>24)&0xFF);
    qDebug()<<text;
    metisIPAddress=metisIP;
    stopped=false;
}

void ReceiveThread::setIPAddress(long ip) {
    myIPAddress=ip;
    QString text;
    text.sprintf("ReceiveThread: myIP %ld.%ld.%ld.%ld)",
                 (ip>>24)&0xFF,(ip>>16)&0xFF,(ip>>8)&0xFF,ip&0xFF);
    qDebug()<<text;
}

void ReceiveThread::stop() {
    qDebug()<<"ReceiveThread::stop";
    stopped=true;
#ifdef __WIN32
    closesocket(s);
#else
    close(s);
#endif
}

void ReceiveThread::run() {
    fd_set fds;
    int n;
    struct timeval t;

    qDebug() << "ReceiveThread::run";

    // set timeout on the socket
    t.tv_sec = 20;
    t.tv_usec = 0;

    stopped=false;

    metis_length=sizeof(metis_addr);
    memset(&metis_addr,0,metis_length);

    while(!stopped) {
        FD_ZERO(&fds);
        FD_SET(s,&fds);
        t.tv_sec = 20;
        t.tv_usec = 0;
        n=select(s+1,&fds,NULL,NULL,&t);
        if(n==0) {
            qDebug()<<"select timeout";
            emit timeout();
            break;
        } else if(n<0) {
            qDebug()<<"Error: ReceiveThread: select returned "<<n;
            break;
        } else {
            bytes_read=recvfrom(s,(char*)buffer,sizeof(buffer),0,(struct sockaddr*)&metis_addr,(socklen_t*)&metis_length);
            if(bytes_read<=0) {
                qDebug() << "recvfrom socket failed for ReceiveThread:"<<errno<<"bytes_read:"<<bytes_read;
                break;
            } else if(bytes_read>=60) {
                if(buffer[0]==0xEF && buffer[1]==0xFE) {
                    switch(buffer[2]) {
                    case 3:  // erase completed
                        qDebug()<<"commandCompleted";
                        emit eraseCompleted();
                        break;
                    case 4:  // ready for next buffer
                        qDebug()<<"ready for next buffer";
                        emit nextBuffer();
                        break;
                    default:
                        qDebug()<<"invalid reply="<<buffer[3];
                        break;
                    }
                } else {
                    qDebug() << "received invalid response in ReceiveThread";
                }
            } else {
                qDebug() << "ReceiveThread expected at least 60 bytes. read" << bytes_read;
            }
        }
    }

    qDebug() << "ReceiveThread stopped";
}
