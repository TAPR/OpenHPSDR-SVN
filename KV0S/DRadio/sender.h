#ifndef SENDER_H
#define SENDER_H

#include <QUdpSocket>

class QUdpSocket;

class Sender : QObject
{
    Q_OBJECT

public:
    Sender();

    void broadcastDatagram( QByteArray meassage );

private:
    QUdpSocket *udpSocket;

};

#endif // SENDER_H
