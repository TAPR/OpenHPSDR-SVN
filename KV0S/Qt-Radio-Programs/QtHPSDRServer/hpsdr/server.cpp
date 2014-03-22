#include "server.h"
#include "alex.h"

#include <QtNetwork/QUdpSocket>
#include <QDebug>
#include <QEventLoop>
#include <QTimer>

Server* Server::instance = NULL;

Server* Server::getInstance(){
  if(instance == NULL){
    instance = new Server();
  }
  return instance;
}

Server::Server() {
}

void Server::configure(QSettings* mysettings) {
    settings=mysettings;

    setHermes(settings->value("hermes",false).toBool());
    setHPSDR(settings->value("hpsdr",false).toBool());
    setOzy(settings->value("ozy",false).toBool());
    setMetis(settings->value("metis",false).toBool());

    setMetisPort(settings->value("metisport",10500).toInt());

    setClientPort(settings->value("clientport",10600).toInt());

    setMercury(settings->value("mercury",false).toBool());
    setPenelope(settings->value("penelope",false).toBool());
    setPennylane(settings->value("pennylane",false).toBool());
    setExcalibur(settings->value("excalibur",false).toBool());

    setAlex(settings->value("alex",false).toBool());
    band=settings->value("band",3).toInt();

    setDevice(settings->value("device",QString("Metis")).toString());
    setInterface(settings->value("interface",QString("")).toString());
    setMetisDetail(settings->value("metis",QString("")).toString());

    setReceivers(settings->value("receivers",1).toInt());
    setSampleRate(settings->value("samplerate",96000).toInt());
    set10MHzClock(settings->value("clock10MHz",QString("Mercury")).toString());
    set122_88MHzClock(settings->value("clock122_88MHz",QString("Mercury")).toString());
    setPreamp(settings->value("preamp",false).toBool());
    setRandom(settings->value("random",true).toBool());
    setDither(settings->value("dither",true).toBool());
    setDuplex(settings->value("duplex",false).toBool());
    setClassE(settings->value("classE",false).toBool());
    setLineIn(settings->value("linein",false).toBool());
    setMicBoost(settings->value("micboost",false).toBool());
    setMicGain(settings->value("micgain",0.25F).toFloat());
    setAudioGain(settings->value("audiogain",0.25F).toFloat());
    setAlexAttenuation(settings->value("alexattenuation",0).toInt());

    auto_start=settings->value("autostart",false).toBool();
    auto_start_dsp=settings->value("autostartdsp",false).toBool();

    // initialize up to 4 receivers
    // default the first receiver to send audio to Metis
    for(int i=0;i<4;i++) {
        receiver[i]=new Receiver(clientPort+i);
        receiver[i]->init(i);
        receiver[i]->setPlayAudio(i==0);
    }

    qDebug()<<"Server::configure: init xvtrs";
    for(int i=0;i<4;i++) {
        xvtrs[i].setLabel(QString());
        xvtrs[i].setMinFrequency(0);
        xvtrs[i].setMaxFrequency(0);
        xvtrs[i].setLOFrequency(0);
        xvtrs[i].setRx(0);
        xvtrs[i].setTx(0);
    }

    state=Server::STOPPED;

    receive_sequence=0;
    receive_sequence_error=0;

    send_rx_frequency=0;
    send_tx_frequency=0;

    penny_change=1;

    samples=0;

    control_out[0]=(unsigned char)0x00; // MOX_DISABLED
    control_out[1]=(unsigned char)0xF9; // CONFIG_BOTH| MERCURY_122_88MHZ_SOURCE | MERCURY_10MHZ_SOURCE | MIC_SOURCE_PENELOPE | SPEED_96KHZ
    control_out[2]=(unsigned char)0x00; // MODE_OTHERS
    control_out[3]=(unsigned char)0x18; // ALEX_ATTENUATION_0DB | LT2208_GAIN_OFF | LT2208_DITHER_ON | LT2208_RANDOM_ON
    control_out[4]=(unsigned char)0x00; // SIMPLEX

    metis_buffer_index=8;
    mox=false;

    XVTR* xvtr;
    for(int i=0;i<4;i++) {
        xvtr=&xvtrs[i];
        xvtr->setLabel(settings->value("xvtrlabel"+QString::number(i),"").toString());
        xvtr->setMinFrequency(settings->value("xvtrminfrequency"+QString::number(i),0).toLongLong());
        xvtr->setMaxFrequency(settings->value("xvtrmaxfrequency"+QString::number(i),0).toLongLong());
        xvtr->setLOFrequency(settings->value("xvtrlofrequency"+QString::number(i),0).toLongLong());
        xvtr->setRx(settings->value("xvtrrxant"+QString::number(i),0).toInt());
        xvtr->setTx(settings->value("xvtrtxant"+QString::number(i),0).toInt());
    }

    loFrequency=(long long)0;
}

void Server::save() {

    settings->setValue("hermes",hermes);
    settings->setValue("hpsdr",hpsdr);
    settings->setValue("ozy",ozy);
    settings->setValue("metis",metis);

    settings->setValue("metisport",metisPort);

    settings->setValue("clientport",clientPort);

    settings->setValue("mercury",mercury);
    settings->setValue("penelope",penelope);
    settings->setValue("pennylane",pennylane);
    settings->setValue("excalibur",excalibur);

    settings->setValue("alex",alex);
    settings->setValue("band",band);

    settings->setValue("device",getDevice());
    settings->setValue("interface",getInterface());
    settings->setValue("metis",getMetisDetail());

    settings->setValue("receivers",receivers);

    settings->setValue("samplerate",sampleRate);

    settings->setValue("clock10MHz",clock10MHz);
    settings->setValue("clock122_88MHz",clock122_88MHz);
    settings->setValue("preamp",preamp);
    settings->setValue("random",random);
    settings->setValue("dither",dither);
    settings->setValue("duplex",duplex);
    settings->setValue("classE",classE);
    settings->setValue("linein",line_in);
    settings->setValue("micboost",mic_boost);
    settings->setValue("micgain",mic_gain);
    settings->setValue("audiogain",audio_gain);
    settings->setValue("alexattenuation",alexAttenuation);

    settings->setValue("autostart",auto_start);
    settings->setValue("autostartdsp",auto_start_dsp);

    XVTR* xvtr;
    for(int i=0;i<4;i++) {
        xvtr=&xvtrs[i];
        settings->setValue("xvtrlabel"+QString::number(i),xvtr->getLabel());
        settings->setValue("xvtrminfrequency"+QString::number(i),xvtr->getMinFrequency());
        settings->setValue("xvtrmaxfrequency"+QString::number(i),xvtr->getMaxFrequency());
        settings->setValue("xvtrlofrequency"+QString::number(i),xvtr->getLOFrequency());
        settings->setValue("xvtrrxant"+QString::number(i),xvtr->getRx());
        settings->setValue("xvtrtxant"+QString::number(i),xvtr->getTx());
    }
}

Interfaces* Server::getInterfaces() {
    return &interfaces;
}

QUdpSocket* Server::getMetisSocket() {
    return &metisSocket;
}

void Server::setHermes(bool state) {
    metis=state;
}

bool Server::getHermes() {
    return metis;
}

void Server::setHPSDR(bool state) {
    hpsdr=state;
}

bool Server::getHPSDR() {
    return hpsdr;
}

void Server::setOzy(bool state) {
    ozy=state;
}

bool Server::getOzy() {
    return ozy;
}

void Server::setMetis(bool state) {
    metis=state;
}

bool Server::getMetis() {
    return metis;
}

void Server::setMetisPort(int p) {
    metisPort=p;
}

int Server::getMetisPort() {
    return metisPort;
}

void Server::setClientPort(int p) {
    clientPort=p;
}

int Server::getClientPort() {
    return clientPort;
}

int Server::bind() {

qDebug()<<"Server::bind: "<<getInterface()<<":"<<getInterfaceIPAddress(getInterface());
    metisSocket.close();

    if(!metisSocket.bind(QHostAddress(getInterfaceIPAddress(getInterface())),metisPort,QUdpSocket::ReuseAddressHint)) {
        error.set("Error: Server::bind: bind failed");
        qDebug()<<"Error: Server::bind bind failed "<<metisSocket.errorString();
        return -1;
    }
    return 0;
}

void Server::setMercury(bool state) {
    mercury=state;
}

bool Server::getMercury() {
    return mercury;
}

bool Server::getPenelope() {
    return penelope;
}

bool Server::getPennylane() {
    return pennylane;
}

bool Server::getExcalibur() {
    return excalibur;
}

bool Server::getAlex() {
    return alex;
}

void Server::setPenelope(bool state) {
    penelope=state;
}

void Server::setPennylane(bool state) {
    pennylane=state;
}

void Server::setExcalibur(bool state) {
    excalibur=state;
}

void Server::setAlex(bool state) {
    alex=state;
}

void Server::setDevice(QString d) {
    device=d;
}

void Server::setInterface(QString i) {
    iface=i;
}

void Server::setMetisDetail(QString m) {
    metisDetail=m;
}

QString Server::getDevice() {
    return device;
}

QString Server::getInterface() {
    return iface;
}

QString Server::getMetisDetail() {
    return metisDetail;
}

QString Server::getInterfaceIPAddress(QString iface) {
    return interfaces.getInterfaceIPAddress(iface);
}

QString Server::getMetisText(int index) {
    Metis m=metisCards.at(index);
    return m.toString();
}

void Server::clearMetis() {
qDebug()<<"Server::clearMetis";
    metisCards.clear();
}

void Server::addMetis(Metis metis) {
qDebug()<<"Server::addMetis";
    metisCards.append(metis);
    metisDetail=metis.toString();
}

int Server::getMetisCount() {
    return metisCards.count();
}

int Server::getOzySoftwareVersion() {
    return ozy_software_version;
}

int Server::getMercurySoftwareVersion() {
    return mercury_software_version;
}

int Server::getPenelopeSoftwareVersion() {
    return penelope_software_version;
}

int Server::getReceiveSequenceError() {
    return receive_sequence_error;
}

int Server::getReceivedFrames() {
    return rx_frame;
}

int Server::getTransmittedFrames() {
    return tx_frame;
}

int Server::attachTo(Client* c,int rx) {
    if(!(receiver[rx]->getClient()==NULL)) {
        return -1;
    } 
    receiver[rx]->attach(c);
    emit clientStateChanged();
    return 0;
}

int Server::detachFrom(Client* c,int rx) {
    if(receiver[rx]->getClient()!=c) {
        return -1;
    } 
    receiver[rx]->detach(c);
    emit clientStateChanged();
    return 0;
}

int Server::startReceiver(Client* c,int rx,QHostAddress host,int port) {
    if(receiver[rx]->getClient()!=c) {
        return -1;
    }
    receiver[rx]->start(c,host,port);
    emit clientStateChanged();
    return 0;
}

int Server::setClientType(Client* c,int rx,QString type) {
    qDebug()<<"Server::setClientType: rx:"<<rx<<" type:"<<type;
    if(receiver[rx]->getClient()!=c) {
        return -1;
    }
    receiver[rx]->setClientType(c,type);
    emit clientStateChanged();
    return 0;
}

int Server::stopReceiver(Client* c,int rx) {
    if(receiver[rx]->getClient()!=c) {
        return -1;
    }
    receiver[rx]->stop(c);
    return 0;
}

int Server::setFrequency(Client* c,int rx,long frequency) {
    if(receiver[rx]->getClient()!=c) {
        return -1;
    }

    if(band<XVTR_BAND) {
        // check frequency range within MIN_FREQUENCY and MAX_FREQUENCY
        if(frequency<MIN_FREQUENCY) {
            frequency=MIN_FREQUENCY;
        } else if(frequency>MAX_FREQUENCY) {
            frequency=MAX_FREQUENCY;
        }
    } else {
        if(frequency<xvtrs[band-XVTR_BAND].getMinFrequency()) {
            frequency=xvtrs[band-XVTR_BAND].getMinFrequency();
        } else if(frequency>xvtrs[band-XVTR_BAND].getMaxFrequency()) {
            frequency=xvtrs[band-XVTR_BAND].getMaxFrequency();
        }
    }
    receiver[rx]->setFrequency(c,frequency-loFrequency);
    emit clientStateChanged();
    return 0;
}

int Server::setFrequencyAndBand(Client* c,int rx,long frequency,int b) {
    if(receiver[rx]->getClient()!=c) {
        return -1;
    }

    if(b<XVTR_BAND) {
        loFrequency=0;
        // check frequency range within MIN_FREQUENCY and MAX_FREQUENCY
        if(frequency<MIN_FREQUENCY) {
            frequency=MIN_FREQUENCY;
        } else if(frequency>MAX_FREQUENCY) {
            frequency=MAX_FREQUENCY;
        }
    } else {
        loFrequency=xvtrs[b-XVTR_BAND].getLOFrequency();
        if(frequency<xvtrs[band-XVTR_BAND].getMinFrequency()) {
            frequency=xvtrs[band-XVTR_BAND].getMinFrequency();
        } else if(frequency>xvtrs[band-XVTR_BAND].getMaxFrequency()) {
            frequency=xvtrs[band-XVTR_BAND].getMaxFrequency();
        }
    }

    qDebug()<<"Server::setFrequencyAndBand: band:"<<band<<" lo:"<<loFrequency;

    receiver[rx]->setFrequency(c,frequency-loFrequency,band);

    if(band!=b) {
        band=b;

        if(alex) {
            int a;
            if(band>=XVTR_BAND) {
                // xvtr
                if(mox) {
                    setAlexTxAntenna(xvtrs[band-XVTR_BAND].getTx());
                } else {
                    a=xvtrs[band-XVTR_BAND].getRx();
                    qDebug()<<"Server::setFrequency: Alex Rx Antenna: "<<a;
                    switch(a) {
                    case ANT1:  // 0 - 0
                    case ANT2:  // 1 - 1
                    case ANT3:  // 2 - 2
                        setAlexRxAntenna(a);
                        break;
                    case RX1:  // 3 - 1
                    case RX2:  // 4 - 2
                    case XVRX: // 5 - 3
                        setAlexRxOnlyAntenna(a-2);
                        break;
                    }
                }
            } else {
                if(mox) {
                    setAlexTxAntenna(Alex::getInstance()->getTx(band));
                } else {
                    a=Alex::getInstance()->getRx(band);
                    qDebug()<<"Server::setFrequency: Alex Rx Antenna: "<<a;
                    switch(a) {
                    case ANT1:  // 0 - 0
                    case ANT2:  // 1 - 1
                    case ANT3:  // 2 - 2
                        setAlexRxAntenna(a);
                        break;
                    case RX1:  // 3 - 1
                    case RX2:  // 4 - 2
                    case XVRX: // 5 - 3
                        setAlexRxOnlyAntenna(a-2);
                        break;
                    }
                }
            }
        }
    }
    emit clientStateChanged();
    return 0;
}

int Server::setMox(Client* c,int rx,int state) {
    if(receiver[rx]->getClient()!=c) {
        return -1;
    }
    mox=state;
    control_out[0]=control_out[0]&0xFE;
    control_out[0]=control_out[0]|(mox&0x01);
    return 0;
}

void Server::start() {

    qDebug()<<"Server::start";

    error.clear();

    if(device=="Metis") {
        receive_sequence=0;
        startMetis();

    } else if(device=="Ozy") {
        // nothing to do for Ozy
    } else {
        // no device selected !!!
    }

    // send 2 frames to get it really started
    start_frames=2;
    qDebug()<<"Server::start send frames";
    while(start_frames>0) {
        sendBuffer();
        start_frames--;
    }

    qDebug()<<"Server::start set receiver frequency";
    for(int i=0;i<receivers;i++) {
        receiver[i]->init(i);
        current_receiver=i;
        sendBuffer();
    }

    current_receiver=0;

}

void Server::stop() {
    for(int i=0;i<receivers;i++) {
        receiver[i]->stop(receiver[i]->getClient());
    }
    if(device=="Metis") {
        state=Server::STOPPED;
        stopMetis();
    } else if(device=="Ozy") {
    } else {
    }
}

void Server::startMetis() {
    int i;

    qDebug()<<"Server::startMetis";

    rx_frame=0;
    tx_frame=0;
    send_sequence=-1;
    offset=8;

    // find the Metis card
    foreach (Metis m, metisCards) {
        if(metisDetail==m.toString()) {
            metisAddress=m.getHostAddress();

            qDebug()<<"startMetis on interface"<<getInterface()<<getInterfaceIPAddress(getInterface());
            qDebug()<<"startMetis:"<<metis;
            //if(!socket->bind(QHostAddress(getInterfaceIPAddress(getInterface())),1024,QUdpSocket::ReuseAddressHint)) {
            //    error.set("Error: Server::startMetis: bind failed");
            //    qDebug()<<"Error: Server::startMetis bind failed "<<socket.errorString();
            //    return;
            //}

            connect(&metisSocket,SIGNAL(readyRead()),this,SLOT(readyRead()));

            unsigned char buffer[64];

            buffer[0]=(char)0xEF;
            buffer[1]=(char)0XFE;
            buffer[2]=(char)0x04;
            buffer[3]=(char)0x01;
            for(i=4;i<64;i++) {
                buffer[i]=(char)0x00;
            }

            qDebug()<<"writeDatagram start command"<<metisAddress->toString();
            if(metisSocket.writeDatagram((const char*)buffer,sizeof(buffer),*metisAddress,1024)<0) {
                qDebug()<<"Error: Server::startMetis: writeDatagram failed "<<metisSocket.errorString();
                return;
            }

            metisSocket.flush();

            state=Server::RUNNING;
            samples=0;

            if(auto_start_dsp) {
                // start QtDSPServer
                for(int i=0;i<receivers;i++) {
                    receiver[i]->startDspServer();
                }
            }
            break;

        }
    }

}

void Server::stopMetis() {
    int i;

    qDebug()<<"Server::stopMetis";
    // find the Metis card
    foreach (Metis m, metisCards) {
        if(metisDetail==m.toString()) {
            metisAddress=m.getHostAddress();

            qDebug()<<"Server::stopMetis metisAddress "<<metisAddress->toString();

            unsigned char buffer[64];

            buffer[0]=(char)0xEF;
            buffer[1]=(char)0XFE;
            buffer[2]=(char)0x04;
            buffer[3]=(char)0x00;
            for(i=4;i<64;i++) {
                buffer[i]=(char)0x00;
            }

            disconnect(&metisSocket,SIGNAL(readyRead()),this,SLOT(readyRead()));

            qDebug()<<"Server::stopMetis writeDatagram";

            if(metisSocket.writeDatagram((const char*)buffer,sizeof(buffer),*metisAddress,1024)<0) {
                qDebug()<<"Error: Server::stopMetis: writeDatagram failed "<<metisSocket.errorString();
                return;
            }

            metisSocket.flush();
            //socket.close();

            if(auto_start_dsp) {
                // stop QtDSPServer
                for(int i=0;i<receivers;i++) {
                    receiver[i]->stopDspServer();
                }
            }

            break;
        }
    }

}


void Server::readyRead() {

    QHostAddress metisAddress;
    quint16 metisPort;
    unsigned char receiveBuffer[1032];
    qint64 length;

    unsigned long sequence;

    while(metisSocket.hasPendingDatagrams()) {

        if((length=metisSocket.readDatagram((char*)&receiveBuffer,(qint64)sizeof(receiveBuffer),&metisAddress,&metisPort))!=1032) {
            qDebug()<<"Error: Server: readDatagram failed "<<metisSocket.errorString();
            return;
        }

        //qDebug()<<"receiver"<<length<<"bytes: "<<receiveBuffer[0]<<","<<receiveBuffer[1]<<","<<receiveBuffer[2]<<","<<receiveBuffer[3];

        if(receiveBuffer[0]==0xEF && receiveBuffer[1]==0xFE) {
            // valid frame
            switch(receiveBuffer[2]) {
            case 1: // IQ data
                switch(receiveBuffer[3]) {
                case 4: // EP4 data
                    break;
                case 6: // EP6 data
                    sequence=((receiveBuffer[4]&0xFF)<<24)+((receiveBuffer[5]&0xFF)<<16)+((receiveBuffer[6]&0xFF)<<8)+(receiveBuffer[7]&0xFF);
                    if(receive_sequence==0) {
                        receive_sequence=sequence;
                    } else {
                        receive_sequence++;
                        if(receive_sequence!=sequence) {
                            //qDebug()<<"Sequence error: expected "<<receive_sequence<<" got "<<sequence;
                            receive_sequence=sequence;
                            receive_sequence_error++;
                        }
                    }
                    process_iq_buffer(&receiveBuffer[8]);
                    process_iq_buffer(&receiveBuffer[520]);
                    break;
                default:
                    qDebug()<<"invalid EP";
                    break;
                }
                break;
            default:
                qDebug()<<"Expected data packet (1) got "<<receiveBuffer[2];
                break;
            }
        } else {
            qDebug()<<"expected EFFE";
        }
    }
}

void Server::process_iq_buffer(unsigned char* buffer) {
    int b=0;
    int b_max;
    int r;
    int left_sample,right_sample,mic_sample;
    float left_sample_float,right_sample_float,mic_sample_float;

    //qDebug()<<"process_iq_buffer: receivers="<<receivers;
    //if(rx_frame<10) {
    //    dump_ozy_buffer("received from Ozy:",rx_frame,buffer);
    //}

    if(buffer[b++]==0x7F && buffer[b++]==0x7F && buffer[b++]==0x7F) {

        // extract control bytes
        control_in[0]=buffer[b++];
        control_in[1]=buffer[b++];
        control_in[2]=buffer[b++];
        control_in[3]=buffer[b++];
        control_in[4]=buffer[b++];

        // extract PTT, DOT and DASH
        ptt=(control_in[0]&0x01)==0x01;
        dash=(control_in[0]&0x02)==0x02;
        dot=(control_in[0]&0x04)==0x04;

        switch((control_in[0]>>3)&0x1F) {
        case 0:
            lt2208ADCOverflow=control_in[1]&0x01;
            IO1=(control_in[1]&0x02)?0:1;
            IO2=(control_in[1]&0x04)?0:1;
            IO3=(control_in[1]&0x08)?0:1;
            if(mercury_software_version!=control_in[2]) {
                mercury_software_version=control_in[2];
                qDebug()<<"Mercury Software version: "<<mercury_software_version;
                emit firmwareVersionChanged();
            }
            if(penelope_software_version!=control_in[3]) {
                penelope_software_version=control_in[3];
                qDebug()<<"Penelope Software version: "<<penelope_software_version;
                emit firmwareVersionChanged();
            }
            if(ozy_software_version!=control_in[4]) {
                ozy_software_version=control_in[4];
                qDebug()<<"Ozy Software version: "<<ozy_software_version;
                emit firmwareVersionChanged();
            }
            break;
                    case 1:
            forwardPower=(control_in[1]<<8)+control_in[2]; // from Penelope or Hermes

            alexForwardPower=(control_in[3]<<8)+control_in[4]; // from Alex or Apollo
            break;
                    case 2:
            alexForwardPower=(control_in[1]<<8)+control_in[2]; // from Alex or Apollo
            AIN3=(control_in[3]<<8)+control_in[4]; // from Pennelope or Hermes
            break;
                    case 3:
            AIN4=(control_in[1]<<8)+control_in[2]; // from Pennelope or Hermes
            AIN6=(control_in[3]<<8)+control_in[4]; // from Pennelope or Hermes
            break;
        }

        b_max=512;
        switch(receivers) {
        case 1: b_max=512-0; break;
        case 2: b_max=512-0; break;
        case 3: b_max=512-4; break;
        case 4: b_max=512-10; break;
        case 5: b_max=512-24; break;
        case 6: b_max=512-10; break;
        case 7: b_max=512-20; break;
        case 8: b_max=512-4; break;
        }

        // extract the samples
        while(b<b_max) {
            // extract each of the receivers
            for(r=0;r<receivers;r++) {
                left_sample   = (int)((signed char)buffer[b++]) << 16;
                left_sample  += (int)((unsigned char)buffer[b++]) << 8;
                left_sample  += (int)((unsigned char)buffer[b++]);
                right_sample  = (int)((signed char)buffer[b++]) << 16;
                right_sample += (int)((unsigned char)buffer[b++]) << 8;
                right_sample += (int)((unsigned char)buffer[b++]);
                left_sample_float=(float)left_sample/8388607.0; // 24 bit sample
                right_sample_float=(float)right_sample/8388607.0; // 24 bit sample
                receiver[r]->put_iq_samples(samples,left_sample_float,right_sample_float);
            }
            mic_sample    = (int)((signed char) buffer[b++]) << 8;
            mic_sample   += (int)((unsigned char)buffer[b++]);
            mic_sample_float=(float)mic_sample/32767.0*mic_gain; // 16 bit sample
            for(r=0;r<receivers;r++) {
                receiver[r]->put_mic_samples(samples,mic_sample_float);
            }
            samples++;

            // when we have enough samples send them to the clients
            if(samples==1024) {
                // send I/Q and mic data to clients
                for(r=0;r<receivers;r++) {
                    receiver[r]->send_IQ_buffer();
                }
                samples=0;
            }
        }

    } else {
        qDebug()<<"Server::process_iq_buffer SYNC Error";
    }

    rx_frame++;

}

Server::STATES Server::getState() {
    return state;
}

void Server::setSampleRate(int s) {
    unsigned char speed=0x00;
    sampleRate=s;

    control_out[1] &= 0xfc;
    switch(sampleRate) {
    case 48000:
        speed=0x00;
        break;
    case 96000:
        speed=0x01;
        break;
    case 192000:
        speed=0x02;
        break;
    default:
        speed=0x01;
        sampleRate=96000;
qDebug()<<"Invalid sample rate: "<<s<<" - default to 96000";
    }
    control_out[1] |= speed;
}

void Server::setReceivers(int r) {
    receivers=r;
    control_out[4] &= 0xc7;
    control_out[4] |= (r-1)<<3;
}

int Server::getSampleRate() {
    return sampleRate;
}

Receiver* Server::getReceiver(int r) {
    return receiver[r];
}

int Server::getReceivers() {
    return receivers;
}

void Server::set10MHzClock(QString c) {
    qDebug()<<"Server::set10MhzClock:"<<c;
    int source=0;
    clock10MHz=c;
    if(c=="Excalibur") {
        source=0;
    } else if(c=="Mercury") {
        source=2;
    } else if(c=="Penelope") {
        source=1;
    }
    control_out[1]=control_out[1]&0xF3;
    control_out[1]=control_out[1]|(source<<2);
}

QString Server::get10MHzClock() {
    return clock10MHz;
}

void Server::set122_88MHzClock(QString c) {
    qDebug()<<"Server::set122_88MHzClock:"<<c;
    int source=0;
    clock122_88MHz=c;
    if(c=="Mercury") {
        source=1;
    } else if(c=="Penelope") {
        source=0;
    }
    control_out[1]=control_out[1]&0xEF;
    control_out[1]=control_out[1]|(source<<4);
}

QString Server::get122_88MHzClock() {
    return clock122_88MHz;
}

void Server::setAlexAttenuation(int v) {
    qDebug()<<"Server::setAlexAttenuation:"<<v;
    alexAttenuation=v;
    control_out[3]=control_out[3]&0xFC;
    control_out[3]=control_out[3]|v;
}

int Server::getAlexAttenuation() {
    return alexAttenuation;
}

void Server::setPreamp(bool s) {
    qDebug()<<"Server::setPreamp:"<<s;
    int p=0;
    preamp=s;
    if(preamp) p=1;
    control_out[3]=control_out[3]&0xFB;
    control_out[3]=control_out[3]|(p<<2);
}

bool Server::getPreamp() {
    return preamp;
}

void Server::setRandom(bool s) {
    int r=0;
    random=s;
    if(s) r=1;
    control_out[3]=control_out[3]&0xEF;
    control_out[3]=control_out[3]|(r<<4);
}

bool Server::getRandom() {
    return random;
}

void Server::setDither(bool s) {
    int d=0;
    dither=s;
    if(d) d=1;
    control_out[3]=control_out[3]&0xF7;
    control_out[3]=control_out[3]|(d<<3);
}

bool Server::getDither() {
    return dither;
}

void Server::setDuplex(bool s) {
    int d=0;
    duplex=s;
    if(s) d=1;
    control_out[4]=control_out[4]&0xFB;
    control_out[4]=control_out[4]|(d<<2);
}

bool Server::getDuplex() {
    return duplex;
}

void Server::setClassE(bool s) {
    int d=0;
    classE=s;
    if(s) d=1;
    control_out[2]=control_out[2]&0xFB;
    control_out[2]=control_out[2]|d;
}

bool Server::getClassE() {
    return classE;
}

void Server::sendBuffer() {
    unsigned char buffer[512];

//qDebug()<<"sendBuffer";
    buffer[0]=(unsigned char)0x7F;
    buffer[1]=(unsigned char)0x7F;
    buffer[2]=(unsigned char)0x7F;

    if(start_frames>0) {
        buffer[3]=control_out[0];
        buffer[4]=control_out[1];
        buffer[5]=control_out[2];
        buffer[6]=control_out[3];
        buffer[7]=control_out[4];
    } else {
        if(send_rx_frequency) {
            long frequency=receiver[current_receiver]->getFrequency();
            buffer[3]=control_out[0]|((current_receiver+2)<<1);
            buffer[4]=frequency>>24;
            buffer[5]=frequency>>16;
            buffer[6]=frequency>>8;
            buffer[7]=frequency;
        } else {
            buffer[3]=control_out[0];
            buffer[4]=control_out[1];
            buffer[5]=control_out[2];
            buffer[6]=control_out[3];
            buffer[7]=control_out[4];

        }
        current_receiver++;
        if(current_receiver>=receivers) {
            current_receiver=0;
            if(send_rx_frequency) {
                send_rx_frequency=0;
            } else {
                send_rx_frequency=1;
            }
        }
    }

    for(int i=8;i<512;i++) {
        buffer[i]=(unsigned char)0x00;
    }

    if(device=="Metis") {
        send_metis_buffer(2,buffer);
    } else if(device=="Ozy") {
    }



}

void Server::send_metis_buffer(int ep,unsigned char* buffer) {
    int i;

    //qDebug()<<"send_metis_buffer offset="<<offset;
    if(offset==8) {
        send_sequence++;
        output_buffer[0]=0xEF;
        output_buffer[1]=0xFE;
        output_buffer[2]=0x01;
        output_buffer[3]=ep;
        output_buffer[4]=(send_sequence>>24)&0xFF;
        output_buffer[5]=(send_sequence>>16)&0xFF;
        output_buffer[6]=(send_sequence>>8)&0xFF;
        output_buffer[7]=(send_sequence)&0xFF;

        // copy the buffer over
        for(i=0;i<512;i++) {
            output_buffer[i+offset]=buffer[i];
        }
        offset=520;
    } else {
        // copy the buffer over
        for(i=0;i<512;i++) {
            output_buffer[i+offset]=buffer[i];
        }
        offset=8;

        // send the buffer
        //qDebug()<<"send_metis_buffer: writeDatagram";
        if(metisSocket.writeDatagram((const char*)output_buffer,sizeof(output_buffer),*metisAddress,1024)<0) {
            error.set("Error: Server::sendMetisBuffer: writeDatagram failed");
            qDebug()<<"Error: Server::sendMetisBuffer writeDatagram failed "<<metisSocket.errorString();
            return;
        }

        metisSocket.flush();

    }

    tx_frame++;
}

QStringList Server::getReceiverInfo(int r) {
    QStringList info;

    info<<QString::number(r);

    Client* client=receiver[r]->getClient();
    if(client!=NULL) {
        QHostAddress addr=client->getPeerAddress();
        if(addr.isNull()) {
            info<<""<<""<<""<<""<<"";
        } else {
            info<<addr.toString()+":"+QString::number(receiver[r]->get_iq_port());
            info<<receiver[r]->getDspServerInfo();
            info<<QString(receiver[r]->getPlayAudio()?"YES":"NO");
            info<<receiver[r]->getClientType();
            info<<QString::number(receiver[r]->getFrequency());
        }
    }

    return info;
}

void Server::enableAudio(int rx) {
    for(int i=0;i<receivers;i++) {
            receiver[i]->setPlayAudio(i==rx);
    }
}

void Server::playAudio(float *buffer) {
    //  buffer contains 1024 Left Audio Samples, Right Audio Samples and Transmit I samples, Transmit Q samples.
    //qDebug()<<"Server::playAudio";

    // send audio to HPSDR always at 48000
    int increment=1;
    if(sampleRate==48000) {
        increment=1;
    } else if(sampleRate==96000) {
        increment=2;
    }else if(sampleRate==192000) {
        increment=4;
    }

    float* left=buffer;
    float* right=&buffer[BUFFER_SIZE];
    float* tx_left=&buffer[BUFFER_SIZE*2];
    float* tx_right=&buffer[BUFFER_SIZE*3];

    short left_audio_sample;
    short right_audio_sample;
    short left_tx_sample;
    short right_tx_sample;
    //qDebug()<<"Server::playAudio: process the buffer: increment:"<<increment<<" mox:"<<mox;
    for(int i=0;i<BUFFER_SIZE;i+=increment) {
        if(mox) {
            left_audio_sample=(short)0;
            right_audio_sample=(short)0;
            left_tx_sample=(short)(tx_left[i]*32767.0);
            right_tx_sample=(short)(tx_right[i]*32767.0);
        } else {
            left_audio_sample=(short)(left[i]*32767.0*audio_gain);
            right_audio_sample=(short)(right[i]*32767.0*audio_gain);
            left_tx_sample=(short)0;
            right_tx_sample=(short)0;
        }

        metis_buffer[metis_buffer_index++]=left_audio_sample>>8;
        metis_buffer[metis_buffer_index++]=left_audio_sample;
        metis_buffer[metis_buffer_index++]=right_audio_sample>>8;
        metis_buffer[metis_buffer_index++]=right_audio_sample;

        metis_buffer[metis_buffer_index++]=left_tx_sample>>8;
        metis_buffer[metis_buffer_index++]=left_tx_sample;
        metis_buffer[metis_buffer_index++]=right_tx_sample>>8;
        metis_buffer[metis_buffer_index++]=right_tx_sample;

        if(metis_buffer_index==512) {
            metis_buffer[0]=(unsigned char)0x7F;
            metis_buffer[1]=(unsigned char)0x7F;
            metis_buffer[2]=(unsigned char)0x7F;

            if(send_tx_frequency) {
                send_tx_frequency=0;
                long frequency=receiver[current_receiver]->getFrequency();
//qDebug() << "send_tx_frequency:"<<frequency;
                metis_buffer[3]=control_out[0]|0x02;
                metis_buffer[4]=(frequency>>24)&0xFF;
                metis_buffer[5]=(frequency>>16)&0xFF;
                metis_buffer[6]=(frequency>>8)&0xFF;
                metis_buffer[7]=frequency&0xFF;
            } else if(penny_change) {
                metis_buffer[3]=control_out[0]|0x12;
                metis_buffer[4]=0; // Hermes/PennyLane drive level
                metis_buffer[5]=0;
                if(mic_boost) metis_buffer[5]|=0x01;
                if(line_in) metis_buffer[5]|=0x02;
                metis_buffer[6]=0;
                metis_buffer[7]=0;
                penny_change=0;
            } else {
                if(send_rx_frequency) {
                    long frequency=receiver[current_receiver]->getFrequency();
//qDebug() << "send_rx_frequency:"<<frequency;
                    metis_buffer[3]=control_out[0]|((current_receiver+2)<<1);
                    metis_buffer[4]=(frequency>>24)&0xFF;
                    metis_buffer[5]=(frequency>>16)&0xFF;
                    metis_buffer[6]=(frequency>>8)&0xFF;
                    metis_buffer[7]=frequency&0xFF;
                } else {
                    metis_buffer[3]=control_out[0];
                    metis_buffer[4]=control_out[1];
                    metis_buffer[5]=control_out[2];
                    metis_buffer[6]=control_out[3];
                    metis_buffer[7]=control_out[4];
                }
                current_receiver++;
                if(current_receiver>=receivers) {
                    current_receiver=0;
                    if(send_rx_frequency) {
                        send_rx_frequency=0;
                        send_tx_frequency=1;
                    } else {
                        send_rx_frequency=1;
                    }
                }
            }

            send_metis_buffer(2,metis_buffer);
            metis_buffer_index=8;
        }

    }
}

void Server::clearError() {
    error.clear();
}

void Server::setError(QString e) {
    error.set(e);
}

void Server::setMox(int state) {
    qDebug()<<"Server::setMox"<<state;
    mox=state;
    control_out[0]=control_out[0]&0xFE;
    control_out[0]=control_out[0]|(mox&0x01);
}

int Server::getMox() {
    return mox;
}

unsigned char Server::getControlOut(int index) {
    return control_out[index];
}

float Server::getMicGain() {
    return mic_gain;
}

void Server::setMicGain(float gain) {
    if(gain<0.0F) {
        mic_gain=0.0F;
    } else if(gain>1.0F) {
        mic_gain=1.0F;
    } else {
        mic_gain=gain;
    }
}

bool Server::getMicBoost() {
    return mic_boost;
}

void Server::setMicBoost(bool s) {
        mic_boost=s;
        penny_change=1;
}

bool Server::getLineIn() {
    return line_in;
}

void Server::setLineIn(bool s) {
    line_in=s;
    penny_change=1;
}

void Server::setAudioGain(float gain) {
    if(gain<0.0F) {
        audio_gain=0.0F;
    } else if(gain>1.0F) {
        audio_gain=1.0F;
    } else {
        audio_gain=gain;
    }
}

float Server::getAudioGain() {
    return audio_gain;
}

void Server::setAutoStart(bool s) {
    auto_start=s;
}

bool Server::getAutoStart() {
    return auto_start;
}

void Server::setAutoStartDsp(bool s) {
    auto_start_dsp=s;
}

bool Server::getAutoStartDsp() {
    return auto_start_dsp;
}

void Server::setAlexRxAntenna(int a) {
    qDebug()<<"Server::setAlexRxAntenna:"<<a;
    alexRxAntenna=a;
    if(!mox) {
        control_out[4]=control_out[4]&0xFC;
        control_out[4]=control_out[4]|a;
        control_out[3]=control_out[3]&0x7F;
    }
}

void Server::setAlexTxAntenna(int a) {
    qDebug()<<"Server::setAlexTxAntenna:"<<a;
    alexTxAntenna=a;
    if(mox) {
        control_out[4]=control_out[4]&0xFC;
        control_out[4]=control_out[4]|a;
        control_out[3]=control_out[3]&0x7F;
    }
}

void Server::setAlexRxOnlyAntenna(int a) {
    qDebug()<<"Server::setAlexRxOnlyAntenna:"<<a;
    alexRxOnlyAntenna=a;
    if(!mox) {
        control_out[3]=control_out[3]&0x9F;
        control_out[3]=control_out[3]|(a<<5);
        if(a!=0) {
            control_out[3]=control_out[3]|0x80;
        } else {
            control_out[3]=control_out[3]&0x7F;
        }
    }
}

XVTR* Server::getXvtrs() {
    return xvtrs;
}

QString Server::getConfig() {
    QString response;

    response.append("<radio>");
    response.append("<type>");
    if(getHPSDR()) {
        response.append("HPSDR");
    } else if(getHermes()) {
        response.append("Hermes");
    }
    response.append("</type>");

    response.append("<id>");
    response.append("G0ORX");
    response.append("</id>");

    response.append("<version>");
    response.append(QString::number(getOzySoftwareVersion()));
    response.append("</version>");

    response.append("<samplerate>");
    response.append(QString::number(getSampleRate()));
    response.append("</samplerate>");

    response.append("<minfrequency>");
    response.append(QString::number(MIN_FREQUENCY));
    response.append("</minfrequency>");
    response.append("<maxfrequency>");
    response.append(QString::number(MAX_FREQUENCY));
    response.append("</maxfrequency>");

    response.append("<receiver>");
    response.append("<type>Mercury</type>");
    response.append("<version>");
    response.append(QString::number(getMercurySoftwareVersion()));
    response.append("</version>");
    response.append("<receivers>");
    response.append(QString::number(getReceivers()));
    response.append("</receivers>");
    response.append("</receiver>");

    response.append("<control>");
    response.append("<id>preamp</id>");
    response.append("<state>");
    response.append(QString(getPreamp()?"true":"false"));
    response.append("</state>");
    response.append("</control>");

    response.append("<control>");
    response.append("<id>dither</id>");
    response.append("<state>");
    response.append(QString(getDither()?"true":"false"));
    response.append("</state>");
    response.append("</control>");

    response.append("<control>");
    response.append("<id>random</id>");
    response.append("<state>");
    response.append(QString(getRandom()?"true":"false"));
    response.append("</state>");
    response.append("</control>");

    if(getAlex()) {
        response.append("<control>");
        response.append("<id>attenuation</id>");
        response.append("<choice>");
        response.append("0dB");
        response.append("</choice>");
        response.append("<choice>");
        response.append("10dB");
        response.append("</choice>");
        response.append("<choice>");
        response.append("20dB");
        response.append("</choice>");
        response.append("<choice>");
        response.append("30dB");
        response.append("</choice>");
        response.append("</control>");
    }



    response.append("<exciter>");
    if(getPenelope()) {
        response.append("<type>Penelope</type>");
    } else if(getPennylane()) {
        response.append("<type>Pennylane</type>");
    }
    response.append("<version>");
    response.append(QString::number(getPenelopeSoftwareVersion()));
    response.append("</version>");
    response.append("</exciter>");

    if(getAlex()) {
        Alex* alex=Alex::getInstance();
        response.append("<alex>");

        for(int i=0;i<=HAM_BAND_LAST;i++) {
            response.append("<"+alex->getBand(i)+">");
            response.append("<rx>");
            response.append(alex->getRxAntenna(i));
            response.append("</rx>");
            response.append("<tx>");
            response.append(alex->getTxAntenna(i));
            response.append("</tx>");
            response.append("</"+alex->getBand(i)+">");
        }

        response.append("</alex>");
    }

    XVTR* xvtr;
    for(int i=0;i<XVTRS;i++) {
        xvtr=&xvtrs[i];
        if(xvtr->getLabel()!="") {
            response.append("<xvtr>");
            response.append("<entry>");
            response.append(QString::number(i+XVTR_BAND));
            response.append("</entry>");
            response.append("<label>");
            response.append(xvtr->getLabel());
            response.append("</label>");
            response.append("<minfreq>");
            response.append(QString::number(xvtr->getMinFrequency()));
            response.append("</minfreq>");
            response.append("<maxfreq>");
            response.append(QString::number(xvtr->getMaxFrequency()));
            response.append("</maxfreq>");
            response.append("<lofreq>");
            response.append(QString::number(xvtr->getLOFrequency()));
            response.append("</lofreq>");
            response.append("<rxant>");
            response.append(xvtr->getRxAntenna());
            response.append("</rxant>");
            response.append("<txant>");
            response.append(xvtr->getTxAntenna());
            response.append("</txant>");
            response.append("</xvtr>");
        }
    }

    response.append("</radio>");

    return response;
}

void Server::setHardware(QString option,QString value) {
    qDebug()<<"Server::setHardware:"<<option<<","<<value;
    if(option=="preamp") {
        setPreamp(value=="on");
    } else if(option=="dither") {
        setDither(value=="on");
    } else if(option=="random") {
        setRandom(value=="on");
    } else if(option=="attenuation") {
        int v=0;
        if(value=="0dB") {
            v=0;
        } else if(value=="10dB") {
            v=1;
        } else if(value=="20dB") {
            v=2;
        } else if(value=="30dB") {
            v=3;
        }
        setAlexAttenuation(v);
    }
}
