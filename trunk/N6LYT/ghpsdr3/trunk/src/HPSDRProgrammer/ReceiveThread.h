#ifndef RECEIVETHREAD_H
#define RECEIVETHREAD_H

#include <sys/types.h>
#ifdef __WIN32
#include <winsock2.h>
#define socklen_t int
#else
#include <sys/select.h>
#include <sys/socket.h>
#include <sys/ioctl.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <net/if_arp.h>
#include <net/if.h>
#include <ifaddrs.h>
#endif
#include <QThread>


class ReceiveThread : public QThread {
    Q_OBJECT
public:
    ReceiveThread(long metisIP,int receiveSocket);
    void run();
    void stop();
    void setIPAddress(long ip);
signals:
    void eraseCompleted();
    void nextBuffer();
    void timeout();
private:
    long metisIPAddress;
    bool stopped;
    long myIPAddress;
    int s;
    struct sockaddr_in addr;
    int length;
    unsigned char buffer[2048];
    int bytes_read;

    struct sockaddr_in metis_addr;
    int metis_length;
    fd_set fds;
};

#endif // RECEIVETHREAD_H
