#include <errno.h>

#include "QDebug"

#include "ReceiveThread.h"
#include "mainwindow.h"

ReceiveThread::ReceiveThread(long metisIP) {

    qDebug()<<"ReceiveThread";

    QString text;
    text.sprintf("ReceiveThread: metisIP %d.%d.%d.%d)",
                 metisIP&0xFF,(metisIP>>8)&0xFF,(metisIP>>16)&0xFF,(metisIP>>24)&0xFF);
    qDebug()<<text;
    metisIPAddress=metisIP;
    stopped=false;
}

void ReceiveThread::setIPAddress(long ip) {
    myIPAddress=ip;
    QString text;
    text.sprintf("ReceiveThread: myIP %d.%d.%d.%d)",
                 (ip>>24)&0xFF,(ip>>16)&0xFF,(ip>>8)&0xFF,ip&0xFF);
    qDebug()<<text;
}

void ReceiveThread::stop() {
    qDebug()<<"ReceiveThread::stop";
    stopped=true;
}

void ReceiveThread::run() {
    struct timeval t;
    int on=1;

    qDebug() << "ReceiveThread::run";

    s=socket(PF_INET,SOCK_DGRAM,IPPROTO_UDP);
    if(s<0) {
        qDebug() << "create socket failed for ReceiveThread";
        exit(1);
    }

    // set timeout on the socket
    t.tv_sec = TIMEOUT / 1000 ;
    t.tv_usec = ( TIMEOUT % 1000) * 1000  ;
    setsockopt (s, SOL_SOCKET, SO_RCVTIMEO, (char *)&t, sizeof t);

    // set the reuse flag so we do not have to wait for a close to complete
    setsockopt(s,SOL_SOCKET,SO_REUSEADDR,(char*)&on,sizeof(on));


    // bind to the selected interface
    length=sizeof(addr);
    memset(&addr,0,length);
    addr.sin_family=AF_INET;
    addr.sin_port=htons(1025);
    addr.sin_addr.s_addr=htonl(myIPAddress);
    if(bind(s,(struct sockaddr*)&addr,length)<0) {
        qDebug() << "bind socket failed for ReceiveThread ";
        perror("bind failed");
        exit(1);
    }

    qDebug() << "ReceiveThread";

    stopped=false;

    metis_length=sizeof(metis_addr);
    memset(&metis_addr,0,metis_length);

    /*
    metis_addr.sin_family=AF_INET;
    metis_addr.sin_port=htons(1025);
    metis_addr.sin_addr.s_addr=metisIPAddress;
    */

#ifdef WIN32
#define socklen_t int
#endif

    while(!stopped) {
        bytes_read=recvfrom(s,(char*)buffer,sizeof(buffer),0,(struct sockaddr*)&metis_addr,(socklen_t*)&metis_length);
        if(bytes_read<0) {
#ifdef WIN32
            if (errno!=WSAEWOULDBLOCK) {
#else
            if (errno!=EWOULDBLOCK) {
#endif
                qDebug() << "recvfrom socket failed for ReceiveThread";
                exit(1);
            } else {
                emit timeout();
            }
        } else if(bytes_read>=60) {
            if(buffer[0]==0xEF && buffer[1]==0xFE) {
                switch(buffer[2]) {
                    case 3:  // erase completed
                        qDebug()<<"commandCompleted";
                        emit commandCompleted();
                        break;
                    case 4:  // ready for next buffer
                        qDebug()<<"ready for next buffer";
                        emit nextBuffer();
                        break;
                    default:
                        qDebug()<<"reply="<<buffer[3];
                        break;
                }
            } else {
                qDebug() << "received invalid response in ReceiveThread";
            }
        } else {
            qDebug() << "ReceiveThread expected at least 60 bytes. read" << bytes_read;
        }
    }

#ifdef WIN32
    closesocket(s);
#else
    close(s);
#endif
}
