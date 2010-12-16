#include "QDebug"

#include "ReceiveThread.h"

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
    stopped=true;
}

void ReceiveThread::run() {
    int on=1;

    qDebug() << "ReceiveThread::run";

    s=socket(PF_INET,SOCK_DGRAM,IPPROTO_UDP);
    if(s<0) {
        qDebug() << "create socket failed for ReceiveThread";
        exit(1);
    }

    setsockopt(s,SOL_SOCKET,SO_REUSEADDR,&on,sizeof(on));

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

    while(!stopped) {
        bytes_read=recvfrom(s,buffer,sizeof(buffer),0,(struct sockaddr*)&metis_addr,(socklen_t*)&metis_length);
        qDebug()<<"ReceiveThread: read "<<bytes_read<<"bytes";
        if(bytes_read<0) {
            qDebug() << "recvfrom socket failed for ReceiveThread";
            exit(1);
        }
        if(bytes_read>=60) {
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

    close(s);
}
