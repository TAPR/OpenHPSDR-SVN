
#include "data.h"
#include "../dttsp.h"
#include "connection.h"
#include "../client/clientlistener.h"

#ifdef Q_WS_WIN
#include <winsock2.h>
#endif

#include <QDebug>

Data* Data::instance = NULL;

Data::Data() {
}

Data* Data::getInstance() {
    if(instance==NULL) {
        instance=new Data();
    }
    return instance;
}

void Data::setConnection(QString h,int r,int l) {

    qDebug()<<"Data:setConnection: "<<h<<":"<<r<<" local_port:"<<l;

    host=h;
    remote_port=r;
    local_port=l;
    tx_sequence=0;
    mox=false;
    mic_remote=false;
    mic_buffer_offset=0;

    // listen for datagrams on local port
    udpSocket = new QUdpSocket(this);
    udpSocket->bind(QHostAddress::LocalHost, local_port);

#ifdef Q_WS_WIN
    int v=32768;
    if(::setsockopt(udpSocket->socketDescriptor(),SOL_SOCKET,SO_RCVBUF,(char*)&v,sizeof(v))==-1) {
        qDebug()<<"Data::setConnection: error using setsockopt";
    }
#endif
     
    connect(udpSocket, SIGNAL(readyRead()),
                this, SLOT(readPendingDatagrams()));
}
     
void Data::readPendingDatagrams() {
    quint64 this_sequence;
    int this_offset;
    int this_length;
    QByteArray datagram;
    QHostAddress sender;
    quint16 senderPort;
    qint64 bytes;
    qint64 bytes_read;
    datagram.resize(512);


    while (udpSocket->hasPendingDatagrams())  {

        bytes=udpSocket->pendingDatagramSize();
        //qDebug()<<"pedingDatagramSize:"<<udpSocket->pendingDatagramSize();
        bytes_read=udpSocket->readDatagram(datagram.data(), bytes,
                                &sender, &senderPort);
 
        if(bytes_read!=bytes) {
            qDebug()<<"readDatagram: bytes:"<<bytes<<" bytes_read:"<<bytes_read;
        }
        // processTheDatagram(datagram);
        //     8 bytes sequency number
        //     2 byte offset
        //     2 byte length
        //     500 bytes of data (or less)

        this_sequence=(datagram.data()[0]&0xFF)<<56 |
                      (datagram.data()[1]&0xFF)<<48 |
                      (datagram.data()[2]&0xFF)<<40 |
                      (datagram.data()[3]&0xFF)<<32 |
                      (datagram.data()[4]&0xFF)<<24 |
                      (datagram.data()[5]&0xFF)<<16 |
                      (datagram.data()[6]&0xFF)<<8 |
                      (datagram.data()[7]&0xFF);

        this_offset=(datagram.data()[8]&0xFF)<<8 |
                    (datagram.data()[9]&0xFF);

        this_length=(datagram.data()[10]&0xFF)<<8 |
                    (datagram.data()[11]&0xFF);

        //qDebug()<<"sequence:"<<this_sequence<<" offset:"<<this_offset<<" length:"<<this_length;
        if(this_offset==0) {
            offset=0;
            sequence=this_sequence;
            memcpy((char *)&input_buffer[0],(char *)&datagram.data()[12],this_length);
            offset+=this_length;
        } else {
            if(this_sequence==sequence && this_offset==offset) {
                memcpy((char *)&input_buffer[offset/4],(char *)&datagram.data()[12],this_length);
                offset+=this_length;
                if(offset==sizeof(input_buffer)) {
                    offset=0;

                    // process the data
                    //qDebug()<<"Audio_Callback: IQ";

                    Audio_Callback(input_buffer,&input_buffer[1024],output_buffer,&output_buffer[1024],1024,0);

                    /*
                      if(BPSK) {

                      }
                      */
/*
   need to fix to handle full duplex when remote audio
*/

                    if(mox) {
                        if(mic_remote) {
                            // mic data comes from remote client
                        } else {
                            // process the microphone samples
                            //qDebug()<<"Audio_Callback: Mic";
                            Audio_Callback(&input_buffer[2048],&input_buffer[2048],&output_buffer[2048],&output_buffer[3072],1024,1);
                        }
                    } else {
                        // set Tx I and Q samples to 0
                        for(int i=0;i<1024;i++) {
                            output_buffer[2048+i]=output_buffer[3072+i]=0.0F;
                        }
                        // send the audio to the clients
                        ClientListener::getInstance()->sendAudio(output_buffer,&output_buffer[1024],1024);
                    }

                    if(mox && mic_remote) {
                        // tx I/Q samples from the received microphone samples
                        qDebug()<<"remote mox";
                    } else {
                        send();
                    }
                }
            } else {
                qDebug()<<"Data::readPendingDatagrams: missing IQ frames:"
                         <<" expected:"<<QString::number(sequence)<<":"<<QString::number(offset)
                         <<" got:"<<QString::number(this_sequence)<<":"<<QString::number(this_offset);
            }
        }
    }
}

void Data::send() {

    mutex.lock();
    QByteArray datagram;
    datagram.resize(512);

    int offset=0;
    int length;
    while(offset<sizeof(output_buffer)) {

        length=sizeof(output_buffer)-offset;
        if(length>500) length=500;

        datagram.data()[0]=(tx_sequence>>56)&0xFF;
        datagram.data()[1]=(tx_sequence>>48)&0xFF;
        datagram.data()[2]=(tx_sequence>>40)&0xFF;
        datagram.data()[3]=(tx_sequence>>32)&0xFF;
        datagram.data()[4]=(tx_sequence>>24)&0xFF;
        datagram.data()[5]=(tx_sequence>>16)&0xFF;
        datagram.data()[6]=(tx_sequence>>8)&0xFF;
        datagram.data()[7]=tx_sequence&0xFF;

        datagram.data()[8]=(offset>>8)&0xFF;
        datagram.data()[9]=offset&0xFF;

        datagram.data()[10]=(length>>8)&0xFF;
        datagram.data()[11]=length&0xFF;

        memcpy((char*)&datagram.data()[12],(char *)&output_buffer[offset/4],length);
        udpSocket->writeDatagram(datagram,QHostAddress(host),remote_port);
        offset+=length;
    }
    tx_sequence++;
    mutex.unlock();
}

void Data::setMox(bool state) {
    qDebug()<<"Data::setMox:"<<state;
    mox=state;
    mic_buffer_offset=0;
}

bool Data::getMox() {
    return mox;
}

void Data::addMicSample(float sample) {
    micMutex.lock();
    if(mox & mic_remote) {
        mic_buffer[mic_buffer_offset]=sample;
        mic_buffer_offset++;
        if(mic_buffer_offset==1024) {
            Audio_Callback(mic_buffer,mic_buffer,&output_buffer[2048],&output_buffer[3072],1024,1);
            // mute the audio output
            for(int i=0;i<1024;i++) {
                output_buffer[i]=output_buffer[1024+i]=0.0F;
            }
            send();
            mic_buffer_offset=0;
        }
    }
    micMutex.unlock();
}

void Data::setMicRemote(bool state) {
    qDebug()<<"Data::setMicRemote:"<<state;
    mic_remote=state;
}
