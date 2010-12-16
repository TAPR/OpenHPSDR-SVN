#ifndef RAWRECEIVETHREAD_H
#define RAWRECEIVETHREAD_H

#include <pcap.h>

#include <QThread>


class RawReceiveThread : public QThread {
    Q_OBJECT
public:
    RawReceiveThread(unsigned char*,pcap_t*);
    void run();
    void stop();
signals:
    void commandCompleted();
    void nextBuffer();
    void macAddress(unsigned char*);
    void ipAddress(unsigned char*);
    void timeout();
private:
    bool stopped;
    unsigned char* hw;
    pcap_t* handle;
};

#endif // RAWRECEIVETHREAD_H
