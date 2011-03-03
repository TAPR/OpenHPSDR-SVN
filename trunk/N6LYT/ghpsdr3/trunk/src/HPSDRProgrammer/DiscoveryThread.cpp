#include "QDebug"

#include "DiscoveryThread.h"


DiscoveryThread::DiscoveryThread(int discovery_socket) {
    s=discovery_socket;
}

void DiscoveryThread::stop() {
    struct linger so_linger;

    qDebug()<<"DiscoveryThread::stop discovery_socket:"<<s;
    stopped=true;
    qDebug()<<"close discovery_socket";

    so_linger.l_onoff=1;
    so_linger.l_linger=0;
    qDebug()<<"setsockopt="<<setsockopt(s,SOL_SOCKET,SO_LINGER,(const char*)&so_linger,sizeof(so_linger));
    qDebug()<<"shutdown="<<shutdown(s,2);
#ifdef __WIN32
    closesocket(s);
#else
    qDebug()<<"close="<<close(s);
#endif
}

void DiscoveryThread::run() {
    stopped=false;

#ifdef __WIN32
#define socklen_t int
#endif

    while(!stopped) {
        length=sizeof(addr);
        bytes_read=recvfrom(s,(char*)buffer,sizeof(buffer),0,(struct sockaddr*)&addr,(socklen_t*)&length);
        if(bytes_read<=0) {
            qDebug() << "recvfrom socket failed for discovery_thread";
            break;
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
                       qDebug() << "unexpected data packet on port 1024!";
                       break;
                }
            } else {
                qDebug() << "received invalid response to discovery";
            }
        } else {
            qDebug() << "discovery_thread expected at least 13 bytes. read" << bytes_read;
        }
    }

    qDebug()<<"DiscoveryThread stopped";
}
