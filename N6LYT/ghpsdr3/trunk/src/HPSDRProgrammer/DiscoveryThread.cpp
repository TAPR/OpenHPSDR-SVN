#include "QDebug"

#include "DiscoveryThread.h"


DiscoveryThread::DiscoveryThread() {
}

void DiscoveryThread::stop() {
    stopped=true;
    close(s);
}

void DiscoveryThread::setIPAddress(long ip) {
    ipAddress=ip;
}

void DiscoveryThread::run() {
    int on=1;

    s=socket(PF_INET,SOCK_DGRAM,IPPROTO_UDP);
    if(s<0) {
        qDebug() << "create socket failed for discovery_thread";
        exit(1);
    }

    setsockopt(s,SOL_SOCKET,SO_REUSEADDR,&on,sizeof(on));

    length=sizeof(addr);
    memset(&addr,0,length);
    addr.sin_family=AF_INET;
    addr.sin_port=htons(1025);
    addr.sin_addr.s_addr = htonl(ipAddress);
    if(bind(s,(struct sockaddr*)&addr,length)<0) {
        qDebug() << "bind socket failed for discovery_thread ";
        perror("bind failed");
        exit(1);
    }



    qDebug() << "Discovery thread";

    stopped=false;

    while(!stopped) {
        bytes_read=recvfrom(s,buffer,sizeof(buffer),0,(struct sockaddr*)&addr,(socklen_t*)&length);
        if(bytes_read<0) {
            qDebug() << "recvfrom socket failed for discovery_thread";
            exit(1);
        }
        if(bytes_read>=13) {
            if(buffer[0]==0xEF && buffer[1]==0xFE) {
                switch(buffer[2]) {
                    case 3:  // reply
                        emit reply(buffer[3]);
                        break;
                    case 2:  // response to a discovery packet
                        emit metis_found(&buffer[3],addr.sin_addr.s_addr);
                        break;
                    case 1:  // a data packet
                       // should not happen on this port
                       qDebug() << "unexpected data packet on port 1025!";
                       break;
                }
            } else {
                qDebug() << "received invalid response to discovery";
            }
        } else {
            qDebug() << "discovery_thread expected at least 13 bytes. read" << bytes_read;
        }
    }

    //close(s);

    qDebug()<<"DiscoveryThread stopped";
}
