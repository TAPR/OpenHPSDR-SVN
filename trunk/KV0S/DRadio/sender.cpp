#include "sender.h"

Sender::Sender() : QObject()
{
    udpSocket = new QUdpSocket(this);    
}

void Sender::broadcastDatagram( QByteArray message )
{
    udpSocket->writeDatagram(message.data(), message.size(),QHostAddress::Broadcast, 45454);

}
