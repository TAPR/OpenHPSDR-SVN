#ifndef DISCOVERYTHREAD_H
#define DISCOVERYTHREAD_H

#include "QThread"

#include <sys/types.h>
#ifdef __WIN32
#include <winsock2.h>
#else
#include <sys/socket.h>
#include <sys/ioctl.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <net/if_arp.h>
#include <net/if.h>
#include <ifaddrs.h>
#endif

class DiscoveryThread : public QThread {
    Q_OBJECT
public:
    DiscoveryThread(int discovery_socket);
    void run();
    void stop();
signals:
    void metis_found(unsigned char*,long);
    void reply(unsigned char);
private:
    bool stopped;
    long ipAddress;
    int s;
    struct sockaddr_in addr;
    int length;
    unsigned char buffer[2048];
    int bytes_read;
};

#endif // DISCOVERYTHREAD_H

