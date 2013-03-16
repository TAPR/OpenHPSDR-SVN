#include "writeboard.h"



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



writeBoard::writeBoard(QUdpSocket *s, unsigned char* MACaddress)
{
    QString myip;
    myip = QString("255.255.255.255");
    macaddr = MACaddress;

    qDebug()<<"Broadcast IP Address: "<< myip;
    ip=myip;
    socket=s;

}

writeBoard::~writeBoard()
{

}

void writeBoard::discovery()
{
    // send the discovery packet
    unsigned char buffer[63];
    int i;

    buffer[0]=(char)0xEF; //header
    buffer[1]=(char)0XFE;
    buffer[2]=(char)0x02;

    for(i=3;i<63;i++) {
        buffer[i]=(char)0x00;
    }

    if(socket->writeDatagram((const char*)buffer,sizeof(buffer),QHostAddress::Broadcast,1024)<0) {
        qDebug()<<"Error: changeIP: writeDatagram failed "<<socket->errorString();
        return;
    }


}

// private load an rbf file
int writeBoard::loadRBF(QString filename) {
    int length;
    int i;
    int rc;

    QFile rbfFile(filename);
    rbfFile.open(QIODevice::ReadOnly);
    QDataStream in(&rbfFile);
    length=((rbfFile.size()+255)/256)*256;
    //data=(char*)malloc(length);


    qDebug() << "file size=" << rbfFile.size() << "length=" << length;
    //status(filename);
    //status("reading file...");
    //if(in.readRawData(data,rbfFile.size())!=rbfFile.size()) {
    //    status("Error: could not read rbf file");
    //    rbfFile.close();
    //    QApplication::restoreOverrideCursor();
    //    blocks=0;
    //    rc=1;
    //} else {
        //status("file read successfully");

        // pad out to mod 256 with 0xFF
      //  for(i=rbfFile.size();i<length;i++) {
        //    data[i]=0xFF;
        //}
        //rbfFile.close();

        //start=0;
        //end=length;
        //blocks=length/256;

        //qDebug() <<"start="<<start<<" end="<<end<<" blocks="<<blocks;
        rc=0;
    //}
    return rc;
}

void writeBoard::sendData() {
    unsigned char buffer[264];

    //qDebug()<<"sendData offset="<<offset;

    //buffer[0]=0xEF;
    //buffer[1]=0xFE;
    //buffer[2]=0x03;
    //buffer[3]=PROGRAM_METIS_FLASH;
    //buffer[4]=(blocks>>24)&0xFF;
    //buffer[5]=(blocks>>16)&0xFF;
    //buffer[6]=(blocks>>8)&0xFF;
    //buffer[7]=blocks&0xFF;

    /*fill the frame with some data*/
    for(int i=0;i<256;i++) {
    //    buffer[i+8]=(unsigned char)data[i+offset];
    }

    //receiveThread->send((const char*)buffer,sizeof(buffer));

    //int p=(offset+256)*100/(end-start);
    //if(p!=percent) {
    //    if((p%20)==0) {
    //        percent=p;
    //        text.sprintf("Programming device %d%% written ...",percent);
    //        status(text);
    //    }
    //}

}

void writeBoard::flashProgram() {

    //size=blocks;
    //data_command=PROGRAM_METIS_FLASH;

    // start a thread to listen for replies
   // QString myip=interfaces.getInterfaceIPAddress(interfaceName);
    //receiveThread=new ReceiveThread(&socket,myip,selectedBoardHostAddress);

    //connect(receiveThread,SIGNAL(eraseCompleted()),this,SLOT(eraseCompleted()));
    //connect(receiveThread,SIGNAL(nextBuffer()),this,SLOT(nextBuffer()));
    //connect(receiveThread,SIGNAL(timeout()),this,SLOT(timeout()));

    //state=ERASING;
    //eraseData();

}

// private function to send the command to erase
void writeBoard::eraseData() {
    //eraseTimeouts=0;
    //status("Erasing device ... (takes several seconds)");
    //sendCommand(ERASE_METIS_FLASH);
    // wait 20 seconds to allow replys
    //QTimer::singleShot(20000,this,SLOT(erase_timeout()));
}

void writeBoard::writeRBF()
{
    // send the RBF write packet
    unsigned char buffer[63];
    int i;

    buffer[0]=(char)0xEF; //header
    buffer[1]=(char)0XFE;
    buffer[2]=(char)0x02;

    for(i=3;i<63;i++) {
        buffer[i]=(char)0x00;
    }

    if(socket->writeDatagram((const char*)buffer,sizeof(buffer),QHostAddress::Broadcast,1024)<0) {
        qDebug()<<"Error: changeIP: writeDatagram failed "<<socket->errorString();
        return;
    }

}

void writeBoard::changeIP(QStringList *saddr )
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


void writeBoard::readyRead() {

    QHostAddress boardAddress;
    quint16 boardPort;
    unsigned char buffer[1024];

    qDebug()<<"Discovery::readyRead";

    //if(   ->readDatagram((char*)&buffer,(qint64)sizeof(buffer),&boardAddress,&boardPort)<0) {
        //qDebug()<<"Error: Discovery: readDatagram failed "<<   ->errorString();
    //    return;
    //}

    if(buffer[0]==0xEF && buffer[1]==0xFE) {
        switch(buffer[2]) {
            case 3:  // reply
                // should not happen on this port
                break;
            case 2:  // response to a discovery packet
                //if(boardAddress.toString()!=  ->getInterfaceIPAddress( ->getInterface())) {
                //    Board* bd=new Board(boardAddress.toIPv4Address(),&buffer[3]);
                //}
                break;
            case 1:  // a data packet
               // should not happen on this port
               break;
        }
    } else {
        qDebug() << "received invalid response to discovery";
    }

}


