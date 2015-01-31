#include "writeboard.h"


WriteBoard::WriteBoard(QUdpSocket *s, StatusDialog *st)
{
    QString myip;
    socket=s;

    stat = st;

    connect(socket, SIGNAL(readyRead()),this, SLOT(readPending()));

}

WriteBoard::~WriteBoard()
{

}



void WriteBoard::discovery()
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
        qDebug()<<"Error: discovery: writeDatagram failed "<<socket->errorString();
        return;
    }

    //QTimer *timer = new QTimer(this);
    //timer->setSingleShot(true);
    //connect(timer, SIGNAL(timeout()), this, SLOT(update_discovery()));
    //timer->start(BOARD_DISCOVERY_TIMEOUT);

    QTimer::singleShot( BOARD_DISCOVERY_TIMEOUT , this, SLOT(update_discovery()));


    qDebug()<< "discovery packet sent";

}

void WriteBoard::update_discovery()
{
    qDebug() <<" writeboard::discoveryUpdate ";
    state=IDLE;
    emit discoveryBoxUpdate();
}

void WriteBoard::update_command()
{
    qDebug() << "No return from the erase command";
    qDebug() << "Try again";

}

// private load an rbf file
int WriteBoard::loadRBF(QString filename) {
    int length;
    int rc;

    QFile rbfFile(filename);
    rbfFile.open(QIODevice::ReadOnly);
    QDataStream in(&rbfFile);
    length=((rbfFile.size()+255)/256)*256;
    data=(char*)malloc(length);


    qDebug() << "file size=" << rbfFile.size() << "length=" << length;
    qDebug() << filename;
    stat->status("reading file...");
    if(in.readRawData(data,rbfFile.size())!=rbfFile.size()) {
        qDebug("Error: could not read rbf file");
        rbfFile.close();
    //    QApplication::restoreOverrideCursor();
        blocks=0;
        rc=1;
    } else {
        stat->status("file read successfully");

        // pad out to mod 256 with 0xFF
        for(int i=rbfFile.size();i<length;i++) {
            data[i]=0xFF;
        }
        rbfFile.close();

        start=0;
        offset=start;
        end=length;
        blocks=length/256;

        qDebug() <<"start="<<start<<" end="<<end<<" blocks="<<blocks;
        rc=0;
    }
    return rc;
}

void WriteBoard::sendCommand(unsigned char command, Board *bd) {
    unsigned char buffer[64];
    int i;

    qDebug()<<"sendCommand "<<command;

    buffer[0]=0xEF; // protocol
    buffer[1]=0xFE;

    buffer[2]=0x03;
    buffer[3]=command;

    /*fill the frame with 0x00*/
    for(i=0;i<60;i++) {
        buffer[i+4]=(unsigned char)0x00;
    }

    qDebug()<<"before send";
    if(socket->writeDatagram((const char*)buffer,sizeof(buffer),bd->getIpAddress(),1024)<0) {
        qDebug()<<"Error: sendCommand: writeDatagram failed "<<socket->errorString();
        return;
    }

    QTimer *timer = new QTimer(this);
    timer->setSingleShot(true);
    connect(timer, SIGNAL(timeout()), this, SLOT(update_command()));
    if ( boards[currentboard]->getBoardString() == "metis" ){
        timer->start(METIS_MAX_ERASE_TIMEOUTS);
    }else if ( boards[currentboard]->getBoardString() == "hermes" ){
        timer->start(HERMES_MAX_ERASE_TIMEOUTS);
    }else if ( boards[currentboard]->getBoardString() == "angelia" ){
        timer->start(ANGELIA_MAX_ERASE_TIMEOUTS);
    }else {  // Orion
        timer->start(ANGELIA_MAX_ERASE_TIMEOUTS);
    }


}



void WriteBoard::sendData(Board *bd)
{
    QString text;
    unsigned char buffer[264];

    qDebug()<<"sendData offset="<<offset;

    buffer[0]=0xEF;
    buffer[1]=0xFE;
    buffer[2]=0x03;
    buffer[3]=PROGRAM_METIS_FLASH;
    buffer[4]=(blocks>>24)&0xFF;
    buffer[5]=(blocks>>16)&0xFF;
    buffer[6]=(blocks>>8)&0xFF;
    buffer[7]=blocks&0xFF;

    /*fill the frame with some data*/
    for(int i=0;i<256;i++) {
        buffer[i+8]=(unsigned char)data[i+offset];
    }

    if(socket->writeDatagram((const char*)buffer,sizeof(buffer),bd->getIpAddress(),1024)<0) {
        qDebug()<<"Error: sendData: writeDatagram failed "<<socket->errorString();
        return;
    }

    int p=(offset+256)*100/(end-start);
    if(p!=percent) {
        if((p%20)==0) {
            percent=p;
            text = QString("Programming device %0 \% written ...").arg(percent);
            stat->status(text);
        }
    }


}



// private function to send the command to erase
void WriteBoard::eraseData(Board *bd) {
     //qDebug("Erasing device ... (can take upto  seconds)");
    if ( boards[currentboard]->getBoardString() == "metis" ){
        stat->status(QString("Erasing device ... (can take upto %0 seconds)").arg(METIS_MAX_ERASE_TIMEOUTS/1000));
    }else if ( boards[currentboard]->getBoardString() == "hermes" ){
        stat->status(QString("Erasing device ... (can take upto %0 seconds)").arg(HERMES_MAX_ERASE_TIMEOUTS/1000));
    }else {  // Angelia
        stat->status(QString("Erasing device ... (can take upto %0 seconds)").arg(ANGELIA_MAX_ERASE_TIMEOUTS/1000));
    }

    sendCommand(ERASE_METIS_FLASH, bd);

    // wait to allow replys
    if ( boards[currentboard]->getBoardString() == "metis" ){
        QTimer::singleShot(METIS_MAX_ERASE_TIMEOUTS,this,SLOT(erase_timeout()));
    }else if ( boards[currentboard]->getBoardString() == "hermes" ){
        QTimer::singleShot(HERMES_MAX_ERASE_TIMEOUTS,this,SLOT(erase_timeout()));
    }else {  // Angelia
        QTimer::singleShot(ANGELIA_MAX_ERASE_TIMEOUTS,this,SLOT(erase_timeout()));
    }

}

void WriteBoard::erase_timeout() {
    //qDebug()<<"WriteBoard::erase_timeout";
    //if(state==ERASING || state==ERASING_ONLY) {
        //qDebug("Error: erase timeout.");
        //qDebug("Try again.");

    //}
    QString text;
    qDebug()<<"MainWindow::timeout state="<<state;
    switch(state) {
    case ERASING:
    case ERASING_ONLY:
        stat->status("Error: erase timeout.");
        stat->status("Try again.");

        if ( boards[currentboard]->getBoardString() == "metis" ){
            //QMessageBox::warning(this, tr("HPSDRProgrammer_V2"),
            qDebug() <<  tr("Erase has timeout at %0 seconds\n").arg(METIS_MAX_ERASE_TIMEOUTS/1000);
            //                     QMessageBox::Retry, QMessageBox::Cancel);
        }else if ( boards[currentboard]->getBoardString() == "hermes" ){
            //QMessageBox::warning(this, tr("HPSDRProgrammer_V2"),
            qDebug() << tr("Erase has timeout at %0 seconds\n").arg(HERMES_MAX_ERASE_TIMEOUTS/1000);
            //                     QMessageBox::Retry, QMessageBox::Cancel);
        }else {  // Angelia
           // QMessageBox::warning(this, tr("HPSDRProgrammer_V2"),
           qDebug() << tr("Erase has timeout at %0 seconds\n").arg(ANGELIA_MAX_ERASE_TIMEOUTS/1000);
           //                      QMessageBox::Retry, QMessageBox::Cancel);
        }

        //QApplication::restoreOverrideCursor();
        break;
    case PROGRAMMING:
        qDebug()<<"timeout";
        break;
    case READ_MAC:
        stat->status("Error: timeout reading MAC address!");
        stat->status("Check that the correct interface is selected.");
        text=QString("Check that there is a jumper");
        stat->status(text);
        //idle();
        break;
    case READ_IP:
        stat->status("Error: timeout reading IP address!");
        stat->status("Check that the correct interface is selected.");
        text=QString("Check that there is a jumper");
        stat->status(text);
        //idle();
        break;
    case WRITE_IP:
        // should not happen as there is no repsonse
        break;
   }
}



void WriteBoard::writeRBF()
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

    if(socket->writeDatagram((const char*)buffer,(qint64)sizeof(buffer),QHostAddress::Broadcast,1024)<0) {
        qDebug()<<"Error: changeIP: writeDatagram failed "<<socket->errorString();
        return;
    }

}

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


 */

void WriteBoard::changeIP(QStringList *saddr,  unsigned char* macaddr)
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
    qDebug() << "buffer" << buffer;

    if(socket->writeDatagram((const char*)buffer,(qint64)sizeof(buffer),QHostAddress::Broadcast,1024)<0) {
        qDebug()<<"Error: changeIP: writeDatagram failed "<<socket->errorString();
        return;
    }

}


void WriteBoard::readPending() {

    while (socket->hasPendingDatagrams()) {
        QHostAddress boardAddress;
        quint16 boardPort;
        unsigned char buffer[1024];
        QList<unsigned char*> MACaddr;

        qDebug()<<"WriteBoard::readyRead";

        if( socket->readDatagram((char*)&buffer,(qint64)sizeof(buffer),&boardAddress,&boardPort)<0) {
            qDebug()<<"Error: WriteBoard: readDatagram failed "<< socket->errorString();
            return;
        }
        qDebug()<< "WriteBoard: readDatagram read " << boardAddress.toString() << boardPort << buffer[2];


        if(buffer[0]==0xEF && buffer[1]==0xFE) {
            switch(buffer[2]) {
                case 4:  // ready for next buffer
                     qDebug()<<"ready for next buffer";
                    emit nextBuffer();
                    break;
                case 3:  // reply
                    // request eraseflash done
                    qDebug() << "Case 3";
                    emit eraseCompleted();
                    state = ERASE_DONE;
                    break;
                case 2:  // response to a discovery packet
                    qDebug() << "Case 2";
                    if( 1  ) {
                        qDebug() << "******* " << &buffer[3] << buffer[9] << buffer[10];
                        Board* bd=new Board(boardAddress.toIPv4Address(), &buffer[3], buffer[9], buffer[10]);
                        //if( !(MACaddr.contains( bd->getMACAddress())) ){
                            boards[bd->toAllString()] = bd;
                            MACaddr.append( bd->getMACAddress() );
                            qDebug() << "board address" << bd->toAllString();
                            return;
                       // }
                    }
                    break;
                case 1:  // a data packet
                    qDebug() << "Case 1";
                    // should not happen on this port
                    break;
           }
        } else {
            qDebug() << "received invalid response to discovery";
        }
    }
    return;
}

void WriteBoard::incOffset()
{
    offset+=256;
    if(offset<end) {
        sendData(boards[currentboard]);
    }else{
        stat->status(tr("Programming device completed successfully. (Will rediscovery board in %0 seconds)").arg(BOARD_DISCOVERY_DELAY/1000));
        emit programmingCompleted();
    }

}
