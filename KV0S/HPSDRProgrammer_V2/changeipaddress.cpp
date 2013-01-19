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



changeIPAddress::changeIPAddress(QUdpSocket *s)
{
    QString myip;
    myip = QString("255.255.255.255");

    qDebug()<<"Change IP Address: "<< myip;
    ip=myip;
    socket=s;

    connect(socket,SIGNAL(readyRead()),this,SLOT(readyRead()));

}

void changeIPAddress::changeIP() {
    // send the changeIP packet
    unsigned char buffer[63];
    int i;

    buffer[0]=(char)0xEF; //header
    buffer[1]=(char)0XFE;
    buffer[2]=(char)0x03;
    //buffer[3]=
    //buffer[4]=
    //buffer[5]=

    for(i=13;i<63;i++) {
        buffer[i]=(char)0x00;
    }

    if(socket->writeDatagram((const char*)buffer,sizeof(buffer),QHostAddress::Broadcast,1024)<0) {
        qDebug()<<"Error: changeIP: writeDatagram failed "<<socket->errorString();
        return;
    }
}

void changeIPAddress::readyread()
{


}
