#include "receiver.h"

#include "server.h"

#include <QtNetwork/QUdpSocket>
#include <QtNetwork/QHostAddress>
#include <QDebug>

#ifdef Q_WS_WIN
#include <windows.h>
#include <winsock2.h>
#endif

Receiver::Receiver(int local_port) {
    udpSocket = new QUdpSocket(this);
    udpSocket->bind(QHostAddress::LocalHost, local_port);

#ifdef Q_WS_WIN
    int v=4096;
    if(::setsockopt(udpSocket->socketDescriptor(),SOL_SOCKET,SO_RCVBUF,(char*)&v,sizeof(v))==-1) {
        qDebug()<<"Data::setConnection: error using setsockopt";
    }
#endif

    connect(udpSocket, SIGNAL(readyRead()),
                this, SLOT(readPendingDatagrams()));

    client_address=QHostAddress();
    iq_port=-1;
    bandscope_port=-1;
    rx_sequence=0;
    tx_sequence=0;
    tx_offset=0;
    frequency_changed=0;
    play_audio=0;
    client=NULL;
    client_type=QString("Disconnected");

    dspServer=NULL;
}

Receiver::~Receiver() {
}

void Receiver::init(int r) {
    rx=r;
    frequency=7056000;
    frequency_changed=1;
    //audio=new Audio(this);
}

int Receiver::getRx() {
    return rx;
}

int Receiver::attach(Client* c) {
    client=c;
    return 0;
}

int Receiver::detach(Client* c) {
    if(client!=c) {
qDebug()<<"detach: Invalid client!";
        return -1;
    }
    client=NULL;
    return 0;
}

int Receiver::start(Client* c,QHostAddress host,int p) {
    if(client!=c) {
qDebug()<<"start: Invalid client!";
        return -1;
    }
    qDebug()<<"Receiver::start: "<<host.toString()<<":"<<p;
    client_address=host;
    iq_port=p;
    return 0;
}

void Receiver::startDspServer() {
    dspServer=new QProcess();
    connect(dspServer,SIGNAL(started()),this,SLOT(dspServerStarted()));
    connect(dspServer,SIGNAL(error(QProcess::ProcessError)),this,SLOT(dspServerError(QProcess::ProcessError)));

    connect(dspServer,SIGNAL(readyReadStandardError()),this,SLOT(dspServerReadyReadStandardError()));
    connect(dspServer,SIGNAL(readyReadStandardOutput()),this,SLOT(dspServerReadyReadStandardOutput()));
    QString command="./QtDSPServer";
    QStringList args;
    QString receiver;
    receiver.sprintf("--receiver=%d",rx);
    args.append(receiver);
    dspServer->start(command,args);
}

void Receiver::stopDspServer() {
    if(dspServer!=NULL) {
        dspServer->close();
        dspServer=NULL;
        client_address=QHostAddress();
        iq_port=-1;
        client=NULL;
    }
}

void Receiver::dspServerStarted() {
    qDebug()<<"Receiver::dspServerStarted";
}

void Receiver::dspServerError(QProcess::ProcessError error) {
    qDebug()<<"Receiver::dspServerError: "<<error;
}

void Receiver::dspServerReadyReadStandardError() {
    qDebug()<<"QtDspServer-"<<rx<<"[err]:";
    qDebug()<<dspServer->readAllStandardError();
}

void Receiver::dspServerReadyReadStandardOutput() {
    qDebug()<<"QtDspServer-"<<rx<<"[out]:";
    qDebug()<<dspServer->readAllStandardOutput();
}

int Receiver::stop(Client* c) {
    if(client!=c) {
qDebug()<<"stop: Invalid client!";
        return -1;
    }
    stopDspServer();
    client_address=QHostAddress();
    iq_port=-1;
    return 0;
}

int Receiver::setClientType(Client *c, QString type) {
    if(client!=c) {
qDebug()<<"setFrequency: Invalid session!";
        return -1;
    }
    client_type=type;
    return 0;
}

int Receiver::setFrequency(Client* c,long f) {
    if(client!=c) {
qDebug()<<"setFrequency: Invalid session!";
        return -1;
    }

    //qDebug()<<"Receiver::setFrequency:"<<f;
    frequency=f;
    frequency_changed=1;
    return 0;
}

int Receiver::setFrequency(Client* c,long f,int b) {
    if(client!=c) {
qDebug()<<"setFrequency: Invalid session!";
        return -1;
    }

    //qDebug()<<"Receiver::setFrequency:"<<f<<" band:"<<b;

    frequency=f;
    frequency_changed=1;

    // process the band
    band=b;
    return 0;
}

int Receiver::getBand() {
    return band;
}

Client* Receiver::getClient() {
    return client;
}

void Receiver::put_iq_samples(int index,float left,float right) {
    input_buffer[index]=left;
    input_buffer[index+BUFFER_SIZE]=right;
}

void Receiver::put_mic_samples(int index, float mic) {
    input_buffer[index+BUFFER_SIZE+BUFFER_SIZE]=mic;
}

void Receiver::send_IQ_buffer() {

    //qDebug()<<"Receiver::send_IQ_buffer";
    if(!client_address.isNull()) {
        if(iq_port!=-1) {

            //qDebug()<<"Receiver::send_IQ_buffer:"<<client_address.toString()<<":"<<iq_port;
            // send the buffer to the client - keep UDP packets to a max of 512 bytes
            //     8 bytes sequency number
            //     2 byte offset
            //     2 byte length
            //     500 bytes of data (or less)


            int offset=0;
            int length=0;
            unsigned char buffer[512];
            while(offset<sizeof(input_buffer)) {

                length=sizeof(input_buffer)-offset;
                if(length>500) length=500;

                // network byte order
                buffer[0]=(rx_sequence>>56)&0xFF;
                buffer[1]=(rx_sequence>>48)&0xFF;
                buffer[2]=(rx_sequence>>40)&0xFF;
                buffer[3]=(rx_sequence>>32)&0xFF;
                buffer[4]=(rx_sequence>>24)&0xFF;
                buffer[5]=(rx_sequence>>16)&0xFF;
                buffer[6]=(rx_sequence>>8)&0xFF;
                buffer[7]=rx_sequence&0xFF;

                buffer[8]=(offset>>8)&0xFF;
                buffer[9]=offset&0xFF;

                buffer[10]=(length>>8)&0xFF;
                buffer[11]=length&0xFF;

                memcpy((char*)&buffer[12],(char*)&input_buffer[offset/4],length);
                if(udpSocket->writeDatagram((const char*)buffer,length+12,client_address,iq_port)!=length+12) {
                    qDebug()<<"Error: Receiver::send_IQ_buffer writeDatagram failed "<<udpSocket->errorString()<<" "<<client_address.toString()<<":"<<iq_port;
                    return;
                }

                offset+=length;
            }
            rx_sequence++;
        } else {
            qDebug()<<"Receiver::send_IQ_buffer iq_port is -1";
        }
    } else {
        //qDebug()<<"Receiver::send_IQ_buffer client is null";
    }
}

QString Receiver::getClientType() {
    return client_type;
}

long Receiver::getFrequency() {
    return frequency;
}

int Receiver::get_iq_port() {
    return iq_port;
}

int Receiver::get_bandscope_port() {
    return bandscope_port;
}

void Receiver::send_audio_buffer(float *audio_buffer) {
    // if this receiver is playing it's audio then send to HPSDR
    if(play_audio) {
        Server::getInstance()->playAudio(audio_buffer);
    }
}

void Receiver::setPlayAudio(int state) {
    play_audio=state;
}

int Receiver::getPlayAudio() {
    return play_audio;
}

void Receiver::readPendingDatagrams() {
    quint64 this_sequence;
    int this_offset;
    int this_length;
    QByteArray datagram;
    QHostAddress sender;
    quint16 senderPort;

    datagram.resize(512);

    while (udpSocket->hasPendingDatagrams())  {

        udpSocket->readDatagram(datagram.data(), 512,
                                &sender, &senderPort);
        // processTheDatagram(datagram);
        //     8 bytes sequency number
        //     2 byte offset
        //     2 byte length
        //     500 bytes of data (or less)

        this_sequence=(quint64)(datagram.data()[0]&0xFF)<<56 |
                      (quint64)(datagram.data()[1]&0xFF)<<48 |
                      (quint64)(datagram.data()[2]&0xFF)<<40 |
                      (quint64)(datagram.data()[3]&0xFF)<<32 |
                      (quint64)(datagram.data()[4]&0xFF)<<24 |
                      (quint64)(datagram.data()[5]&0xFF)<<16 |
                      (quint64)(datagram.data()[6]&0xFF)<<8 |
                      (quint64)(datagram.data()[7]&0xFF);

        this_offset=(datagram.data()[8]&0xFF)<<8 |
                    (datagram.data()[9]&0xFF);

        this_length=(datagram.data()[10]&0xFF)<<8 |
                    (datagram.data()[11]&0xFF);

        if(this_offset==0) {
            tx_offset=0;
            tx_sequence=this_sequence;
            memcpy((char *)&output_buffer[0],(char *)&datagram.data()[12],this_length);
            tx_offset+=this_length;
        } else {
            if(this_sequence==tx_sequence && this_offset==tx_offset) {

                memcpy((char *)&output_buffer[tx_offset/4],(char *)&datagram.data()[12],this_length);
                tx_offset+=this_length;
                if(tx_offset==sizeof(output_buffer)) {
                    tx_offset=0;

                    // process the data
                    send_audio_buffer(output_buffer);

                }
            } else {
                //qDebug()<<"Receiver::readPendingDatagrams: expected "<<tx_sequence<<":"<<tx_offset<<" got "<<this_sequence<<":"<<this_offset;
            }
        }
    }
}

void Receiver::setMox(int state) {
    mox=state;
}

QString Receiver::getDspServerInfo() {
    if(dspServer!=NULL) {
        return QString::number(getProcessID(dspServer));
    }
    return QString("Unknown");
}

quint64 Receiver::getProcessID(const QProcess* proc)
{
#ifdef Q_WS_WIN
    struct _PROCESS_INFORMATION* procinfo = proc->pid();
    return procinfo->dwProcessId;
#else // Linux
    return proc->pid();
#endif // Q_WS_WIN
}
