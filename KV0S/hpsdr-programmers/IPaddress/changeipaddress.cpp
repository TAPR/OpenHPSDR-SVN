#include "changeipaddress.h"



/*
 * "Metis Set IP  Packet format.

A Metis Set IP  packet is sent from a PC in order to set the IP Address of
a specific Metis card

The Set IP  packet is a UDP/IP frame sent to Ethernet address
255.255.255.255 (i.e. a Broadcast) and port 1024 with the following
payload:

<0xEFFE><0x03><MAC_Address><IP_Address>

where MAC_Address is the MAC address of the Metis card that the IP address
is to the set  - (6 bytes) and IP_Address is the IP address to write to
that card - (4 bytes).

Once the new IP address has been written then the power needs to be cycled
in order for the new address to be read using a Discovery."

 */



ChangeIPAddress::ChangeIPAddress(QUdpSocket *s, unsigned char* MACaddress)
{
    QString myip;
    myip = QString("255.255.255.255");
    macaddr = MACaddress;

    qDebug()<<"Broadcast IP Address: "<< myip;
    ip=myip;
    socket=s;

}

ChangeIPAddress::~ChangeIPAddress()
{

}

void ChangeIPAddress::changeIP(QStringList *saddr )
{
    // send the changeIP packet
    unsigned char buffer[63];
    int i;
    int addr[4];

    addr[0] = saddr->at(0).toInt();
    addr[1] = saddr->at(1).toInt();
    addr[2] = saddr->at(2).toInt();
    addr[3] = saddr->at(3).toInt();

    qDebug() << "new address" << saddr->at(0) <<saddr->at(1) << saddr->at(2) << saddr->at(3);

    QString text;
    text.sprintf("%02X:%02X:%02X:%02X:%02x:%02X",
                 macaddr[0],macaddr[1],macaddr[2],macaddr[3],macaddr[4],macaddr[5]);
    qDebug() << " MAC "  << text;

    buffer[0]=(char)0xEF; //header
    buffer[1]=(char)0XFE;
    buffer[2]=(char)0x03;
    buffer[3]=macaddr[0];
    buffer[4]=macaddr[1];
    buffer[5]=macaddr[2];
    buffer[6]=macaddr[3];
    buffer[7]=macaddr[4];
    buffer[8]=macaddr[5];

    // the IP address from the interface
    buffer[9]=(unsigned char)addr[0];
    buffer[10]=(unsigned char)addr[1];
    buffer[11]=(unsigned char)addr[2];
    buffer[12]=(unsigned char)addr[3];


    for(i=13;i<63;i++) {
        buffer[i]=(char)0x00;
    }

    if(socket->writeDatagram((const char*)buffer,sizeof(buffer),QHostAddress::Broadcast,1024)<0) {
        qDebug()<<"Error: changeIP: writeDatagram failed "<<socket->errorString();
        return;
    }

}




