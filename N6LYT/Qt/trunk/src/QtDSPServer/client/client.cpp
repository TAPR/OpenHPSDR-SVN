#include "client.h"
#include "../dttsp.h"
#include "../hpsdr/connection.h"
#include "../hpsdr/data.h"

#include <math.h>


#include <QStringList>

Client::Client(QTcpSocket* s,int rx,QObject *parent) :
    QObject(parent)
{
    qDebug()<<"Client: rx:"<<rx;
    multimeterCalibrationOffset=-41.0f;
    displayCalibrationOffset=-48.0f;
    squelchCalibrationOffset=-21.0f;

    sampleRate=96000;

    audio_port=0;

    audioBufferOffset=0;

    sequence=0;

    receiver=rx;
    socket=s;
    connect(s,SIGNAL(readyRead()),this,SLOT(readyRead()));
    connect(s,SIGNAL(disconnected()),this,SLOT(disconnected()));


}

void Client::disconnected() {
    socket->close();
    socket=NULL;
    audio_port=0;
    emit disconnected(this);

    QString command;
    command.append(QString("client "));
    command.append(QString::number(receiver));
    command.append(QString(" "));
    command.append(QString("Disconnected"));
    //emit sendCommand(command);
    Connection::getInstance()->sendCommand(command);
}

QHostAddress Client::getPeerAddress() {
    QHostAddress a;
    if(socket!=NULL) a=socket->peerAddress();
    return a;
}

void Client::readyRead() {
    //Server* server=Server::getInstance();
    QByteArray response;
    QString config;
    while(socket->canReadLine()) {
        response.resize(0);
        QString buffer(socket->readLine(1024));
        buffer.chop(1); // remove the new line
        //qDebug()<<"client:"<<buffer;
        QStringList args=buffer.split(" ");
        switch(args.length()){
        case 1:
            // no arguments
            if(args[0]=="disconnect") {
                // drop client
            } else {
                //response.append("Error: Invalid command\n");
            }
            break;
        case 2:
            // 1 argument
            if (args[0]=="frequency") {
                // arg[1] frequency
                QString command;
                command.append(QString("frequency "));
                command.append(QString::number(receiver));
                command.append(QString(" "));
                command.append(args[1]);
                //emit sendCommand(command);
                Connection::getInstance()->sendCommand(command);
                //response.append("OK");
            } else if (args[0]=="subrxfrequency") {
                // arg[1] frequency
                SetRXOsc(0,1,args[1].toDouble());
                //response.append("OK");
            } else if (args[0]=="getspectrum") {
                // arg[1] samples
                int n=args[1].toInt();
                // get the spectrum
                float spectrumBuffer[4096];
                int meter;
                int subrx_meter;
                if(Data::getInstance()->getMox()) {
                    Process_Panadapter(1,spectrumBuffer);
                    // get the MIC level
                    meter=CalculateTXMeter(1,5);
                    subrx_meter=-121;
                } else {
                    Process_Panadapter(0,spectrumBuffer);
                    // get the RX_SIGNAL_STRENGTH
                    meter=CalculateRXMeter(0,0,0)+multimeterCalibrationOffset+getFilterSizeCalibrationOffset();
                    subrx_meter=CalculateRXMeter(0,1,0)+multimeterCalibrationOffset+getFilterSizeCalibrationOffset();
                }

                response.resize(n+HEADER_SIZE); // spectrum data * HEADER

                response.data()[0]=SPECTRUM_BUFFER;
                response.data()[1]=HEADER_VERSION;
                response.data()[2]=HEADER_SUBVERSION;

                response.data()[3]=(n>>8)&0xFF;  // samples length
                response.data()[4]=n&0xFF;
                response.data()[5]=((int)meter>>8)&0xFF; // main rx meter
                response.data()[6]=(int)meter&0xFF;
                response.data()[7]=((int)subrx_meter>>8)&0xFF; // sub rx meter
                response.data()[8]=(int)subrx_meter&0xFF;

                sampleRate=Connection::getInstance()->getSampleRate();
                response.data()[9]=(sampleRate>>24)&0xFF; // sample rate
                response.data()[10]=(sampleRate>>16)&0xFF;
                response.data()[11]=(sampleRate>>8)&0xFF;
                response.data()[12]=sampleRate&0xFF;

                float slope;
                float max;
                int lindex,rindex;

                slope=(float)4096/(float)n;
                for(int i=0;i<n;i++) {
                    max=-10000.0F;
                    lindex=(int)floor((float)i*slope);
                    rindex=(int)floor(((float)i*slope)+slope);
                    if(rindex>4096) rindex=4096;
                    for(int j=lindex;j<rindex;j++) {
                        if(spectrumBuffer[j]>max)
                            max=spectrumBuffer[j];
                    }
                    response.data()[i+HEADER_SIZE]=(unsigned char)-(max+displayCalibrationOffset/*+preampOffset*/);
                }
            } else if (args[0]=="client") {
                client_type=args[1];
                QString command;
                command.append(QString("client "));
                command.append(QString::number(receiver));
                command.append(QString(" "));
                command.append(args[1]);
                //emit sendCommand(command);
                Connection::getInstance()->sendCommand(command);
            } else if (args[0]=="preamp") {
                // arg[1] state
                QString command;
                command.append(QString("preamp "));
                command.append(args[1]);
                //emit sendCommand(command);
                Connection::getInstance()->sendCommand(command);
                //response.append("OK");
            } else if (args[0]=="mode") {
                // args[1] mode
                SetMode(0,0,(SDRMODE)args[1].toInt());  // main receiver
                SetMode(0,1,(SDRMODE)args[1].toInt());  // sub receiver
                SetMode(1,0,(SDRMODE)args[1].toInt());  // transmit
                //response.append("OK");
            } else if (args[0]=="agc") {
                // arg[1] agc
                SetRXAGC(0,0,args[1].toInt());
                SetRXAGC(0,1,args[1].toInt());
                //response.append("OK");
            } else if (args[0]=="nr") {
                // arg[1] state
                SetNR(0,0,args[1].toInt());
                SetNR(0,1,args[1].toInt());
                //response.append("OK");
            } else if (args[0]=="nb") {
                // arg[1] state
                SetNB(0,0,args[1].toInt());
                SetNB(0,1,args[1].toInt());
                //response.append("OK");
            } else if (args[0]=="sdrom") {
                // arg[1] state
                SetSDROM(0,0,args[1].toInt());
                SetSDROM(0,1,args[1].toInt());
                //response.append("OK");
            } else if (args[0]=="anf") {
                // arg[1] state
                SetANF(0,0,args[1].toInt());
                SetANF(0,1,args[1].toInt());
                //response.append("OK");
            } else if (args[0]=="rxoutputgain") {
                // arg[1] gain
                SetRXOutputGain(0,0,args[1].toDouble()/100.0);
                //response.append("OK");
            } else if (args[0]=="subrxoutputgain") {
                // arg[1] gain
                SetRXOutputGain(0,1,args[1].toDouble()/100.0);
                //response.append("OK");
            } else if (args[0]=="subrx") {
                // arg[1] state
                SetSubRXSt(0,1,args[1].toInt());
                //response.append("OK");
            } else if (args[0]=="pan") {
                // arg[1] pan
                SetRXPan(0,0,args[1].toFloat());
                //response.append("OK");
            } else if (args[0]=="subrxpan") {
                // arg[1] pan
                SetRXPan(0,1,args[1].toFloat());
                //response.append("OK");
            } else if (args[0]=="nbvals") {
                // args[1] threshold
                SetNBvals(0,0,args[1].toDouble());
                SetNBvals(0,1,args[1].toDouble());
                //response.append("OK");
            } else if (args[0]=="sdromvals") {
                // args[1] threshold
                SetSDROMvals(0,0,args[1].toDouble());
                SetSDROMvals(0,1,args[1].toDouble());
                //response.append("OK");
            } else if (args[0]=="dcblock") {
                // args[1] state
                SetRXDCBlock(0,0,args[1].toInt());
                SetRXDCBlock(0,1,args[1].toInt());
                //response.append("OK");
            } else if (args[0]=="mox") {
                // args[1] state
                QString command;
                command.append(QString("mox "));
                command.append(QString::number(receiver));
                command.append(QString(" "));
                command.append(args[1]);
                Connection::getInstance()->sendCommand(command);
                Data::getInstance()->setMox(args[1].toInt());
            } else if (args[0]=="squelch") {
                // args[1] value
                SetSquelchVal(0,0,args[1].toFloat()-squelchCalibrationOffset);
            } else if (args[0]=="subrxsquelch") {
                // args[1] value
                SetSquelchVal(0,1,args[1].toFloat()-displayCalibrationOffset);
            } else if (args[0]=="squelchstate") {
                // args[1] state
                SetSquelchState(0,0,args[1].toInt());
            } else if (args[0]=="subrxsquelchstate") {
                // args[1] state
                SetSquelchState(0,1,args[1].toInt());
            } else if (args[0]=="polyphase") {
                // args[1] state
                SetSpectrumPolyphase(0,0,args[1].toInt());
                SetSpectrumPolyphase(0,1,args[1].toInt());
            } else if (args[0]=="ocoutputs") {
                // args[1] oc output
            } else if(args[0]=="remotemic") {
                Data::getInstance()->setMicRemote((bool)args[1].toInt());
            }  else {
                qDebug()<<"Error: InvalidCommand:"<<buffer;
            }
            break;
        case 3:
            if (args[0]=="frequency") {
                // arg[1] frequency
                // arg[2] band
                QString command;
                command.append(QString("frequency "));
                command.append(QString::number(receiver));
                command.append(QString(" "));
                command.append(args[1]);
                command.append(QString(" "));
                command.append(args[2]);
                //emit sendCommand(command);
                Connection::getInstance()->sendCommand(command);
                //response.append("OK");
            } else if (args[0]=="filter") {
                // args[1] low
                // args[2] high
                SetRXFilter(0,0,args[1].toDouble(),args[2].toDouble());
                SetRXFilter(0,1,args[1].toDouble(),args[2].toDouble());
                SetTXFilter(1,args[1].toDouble(),args[2].toDouble());
            } else if(args[0]=="hardware") {
                QString command;
                command.append(args[0]);
                command.append(QString(" "));
                command.append(args[1]);
                command.append(QString(" "));
                command.append(args[2]);
                Connection::getInstance()->sendCommand(command);
            }  else {
                qDebug()<<"Error: InvalidCommand:"<<buffer;
            }
            break;
        case 5:
            if (args[0]=="anfvals") {
                // arg[1] taps
                // arg[2] delay
                // arg[3] gain
                // arg[4] leakage
                SetANFvals(0,0,args[1].toInt(),args[2].toInt(),args[3].toFloat(),args[4].toFloat());
                SetANFvals(0,1,args[1].toInt(),args[2].toInt(),args[3].toFloat(),args[4].toFloat());
            } else if (args[0]=="nrvals") {
                // arg[1] taps
                // arg[2] delay
                // arg[3] gain
                // arg[4] leakage
                SetNRvals(0,0,args[1].toInt(),args[2].toInt(),args[3].toFloat(),args[4].toFloat());
                SetNRvals(0,1,args[1].toInt(),args[2].toInt(),args[3].toFloat(),args[4].toFloat());
            } else {
                //response.append("Error: Invalid command");
                qDebug()<<"Error: InvalidCommand:"<<buffer;
            }
            break;
        case 6:
            if(args[0]=="connect") {
                // arg[1] port
                // arg[2] rate
                // arg[3] channels
                // arg[4] encoding
                // arg[5] client type
                audio_port=args[1].toInt();
                audio_rate=args[2].toInt();
                audio_channels=args[3].toInt();
                qDebug()<<"connect: PCM audio_port:"<<audio_port<<" audio_rate:"<<audio_rate<<" encoding:"<<args[5]<<" channels:"<<audio_channels;
                if(args[4]=="G711a") {
                    audio_encoding=ENCODING_G711A;
                    audioBufferSize=1*audio_channels*(audio_rate/AUDIO_FRAMES_PER_SECOND);   // 8 bits per sample
                } else if(args[4]=="G711u") {
                    audio_encoding=ENCODING_G711U;
                    audioBufferSize=1*audio_channels*(audio_rate/AUDIO_FRAMES_PER_SECOND);   // 8 bits per sample
                } else if(args[4]=="PCM") {
                    audio_encoding=ENCODING_PCM;
                    audioBufferSize=2*audio_channels*(audio_rate/AUDIO_FRAMES_PER_SECOND);   // 16 bits per sample
                } else if(args[4]=="G721") {
                    audio_encoding=ENCODING_G721;
                    audioBufferSize=(audio_channels*(audio_rate/AUDIO_FRAMES_PER_SECOND))/2; // 4 bits per sample
                } else {
                    // default
                    audio_encoding=ENCODING_G711A;
                    audioBufferSize=1*audio_channels*(audio_rate/AUDIO_FRAMES_PER_SECOND);
                }

                client_type=args[5];

                audioBufferSize+=8;  // 8 byte sequence

                qDebug()<<"connect port="<<audio_port;
                audioSocket=new QUdpSocket();
                audioSocket->bind(11000);

                audioBuffer.resize(audioBufferSize);
                audioBuffer.data()[0]=sequence>>56;
                audioBuffer.data()[1]=sequence>>48;
                audioBuffer.data()[2]=sequence>>40;
                audioBuffer.data()[3]=sequence>>32;
                audioBuffer.data()[4]=sequence>>24;
                audioBuffer.data()[5]=sequence>>16;
                audioBuffer.data()[6]=sequence>>8;
                audioBuffer.data()[7]=sequence;

                audioBufferOffset=8;
                audioBufferCount=0;

                connect(audioSocket,SIGNAL(readyRead()),this,SLOT(receiveAudio()));

                qDebug()<<"Client::readyRead:"<<buffer;
                qDebug()<<"Client::readyRead: port:"<<audio_port<<" rate:"<<audio_rate<<" chanels:"<<audio_channels<<" encoding:"<<audio_encoding;

                config=Connection::getInstance()->getConfiguration();
                if(!config.isNull()) {
                    int length=config.length();
                    response.resize(HEADER_SIZE+config.length());  // type, version, subversion,length and config data

                    response.data()[0]=CONFIG_BUFFER;
                    response.data()[1]=HEADER_VERSION;
                    response.data()[2]=HEADER_SUBVERSION;
                    response.data()[3]=(length>>8)&0xFF;  // samples length
                    response.data()[4]=length&0xFF;
                    response.data()[5]=0;                 // unused
                    response.data()[6]=0;
                    response.data()[7]=0;
                    response.data()[8]=0;
                    response.data()[9]=0;
                    response.data()[10]=0;
                    response.data()[11]=0;
                    response.data()[12]=0;
                    for(int i=0;i<length;i++) {
                        response.data()[HEADER_SIZE+i]=config.at(i).toAscii();
                    }
                }

                QString command;
                command.append(QString("client "));
                command.append(QString::number(receiver));
                command.append(QString(" "));
                command.append(client_type);
                Connection::getInstance()->sendCommand(command);

                //response.append("OK ");
                //response.append(QString::number(sampleRate));
            } else {
                //response.append("Error: Invalid command");
                qDebug()<<"Error: InvalidCommand:"<<buffer;
            }
            break;
        default:
            //response.append("Error: Invalid command");
            qDebug()<<"Error: InvalidCommand:"<<buffer;
            break;

        }
        //response.append("\n");
        if(response.length()>0) {
            //qDebug()<<"sending response:"<<response.length()<<" bytes";
            socket->write(response);
        }
    }
}

void Client::sendAudio(float* left,float* right,int length) {
    if(audio_port!=0) {


        sampleRate=Connection::getInstance()->getSampleRate();
        mutex.lock();
        //qDebug()<<"sendAudio: audio_rate:"<<audio_rate<<" sampleRate:"<<sampleRate<<" encoding:"<<audio_encoding<<" channels:"<<audio_channels;
        int downsample=(sampleRate/audio_rate);
        for(int i=0;i<length;i+=downsample) {
            float sum_left=0.0F;
            float sum_right=0.0F;

            for(int j=0;j<downsample;j++) {
                sum_left+=left[i+j];
                sum_right+=right[i+j];
            }
            short l=(short)(sum_left/(float)downsample*32767.0F);
            short r=(short)(sum_right/(float)downsample*32767.0F);

            int v;
            int vl;
            int vr;
            switch(audio_encoding) {
            case ENCODING_G711A:
                switch(audio_channels) {
                case 1:
                    audioBuffer.data()[audioBufferOffset++]=g711a.encode((l+r)/2);
                    break;
                case 2:
                    audioBuffer.data()[audioBufferOffset++]=g711a.encode(l);
                    audioBuffer.data()[audioBufferOffset++]=g711a.encode(r);
                    break;
                }
                break;
            case ENCODING_G711U:
                switch(audio_channels) {
                case 1:
                    audioBuffer.data()[audioBufferOffset++]=g711u.encode((l+r)/2);
                    break;
                case 2:
                    audioBuffer.data()[audioBufferOffset++]=g711u.encode(l);
                    audioBuffer.data()[audioBufferOffset++]=g711u.encode(r);
                    break;
                }
                break;
            case ENCODING_PCM:
                switch(audio_channels) {
                case 1:
                    audioBuffer.data()[audioBufferOffset++]=((l+r)/2)>>8;
                    audioBuffer.data()[audioBufferOffset++]=(l+r)/2;
                    break;
                case 2:
                    audioBuffer.data()[audioBufferOffset++]=l>>8;
                    audioBuffer.data()[audioBufferOffset++]=l;
                    audioBuffer.data()[audioBufferOffset++]=r>>8;
                    audioBuffer.data()[audioBufferOffset++]=r;
                    break;
                }
                break;
            case ENCODING_G721:
                switch(audio_channels) {
                case 1:
                    v=g721.encode((l+r)/2);
                    if(audioBufferCount==0) {
                        audioBuffer.data()[audioBufferOffset]=v<<4;
                        audioBufferCount++;
                    } else {
                        audioBuffer.data()[audioBufferOffset++]+=v&0x0F;
                        audioBufferCount=0;
                    }
                    break;
                case 2:
                    vl=g721.encode(l);
                    vr=g721.encode(r);
                    audioBuffer.data()[audioBufferOffset++]=(vl<<4)+(vr&0x0F);
                    break;
                }
            }
            if(audioBufferOffset>=audioBuffer.length()) {
                // send the buffer
                int sent=audioSocket->writeDatagram(audioBuffer.data(),(quint64)audioBuffer.length(),socket->peerAddress(),audio_port);
                if(sent<=0) {
                    qDebug()<<"audio writeDatagram failed:"<<audioSocket->errorString();
                } else if(sent<audioBuffer.length()) {
                    qDebug()<<"audio writeDatagram failed: length:"<<sent<<" expected:"<<audioBuffer.length();
                }

                // setup for next
                sequence++;
                audioBuffer.data()[0]=sequence>>56;
                audioBuffer.data()[1]=sequence>>48;
                audioBuffer.data()[2]=sequence>>40;
                audioBuffer.data()[3]=sequence>>32;
                audioBuffer.data()[4]=sequence>>24;
                audioBuffer.data()[5]=sequence>>16;
                audioBuffer.data()[6]=sequence>>8;
                audioBuffer.data()[7]=sequence;

                audioBufferOffset=8;
            }
        }
        mutex.unlock();
    }
}

void Client::receiveAudio() {
    // read microphone data
    // if mox and using remote audio process it
    // always 1 channel for 20ms
    int size;
    switch(audio_encoding) {
    case ENCODING_G711A:
        size=audio_rate/AUDIO_FRAMES_PER_SECOND;
        break;
    case ENCODING_G711U:
        size=audio_rate/AUDIO_FRAMES_PER_SECOND;
        break;
    case ENCODING_PCM:
        size=2*audio_rate/AUDIO_FRAMES_PER_SECOND;
        break;
    }


    sampleRate=Connection::getInstance()->getSampleRate();

    char buffer[size];
    while(audioSocket->bytesAvailable()>=size) {
        int bytes_read=audioSocket->readDatagram(buffer,(qint64)size);
        if(bytes_read==size) {
            Data* data=Data::getInstance();
            float v;
            int upsample=(sampleRate/audio_rate);
            int i=0;
            while(i<size) {
                switch(audio_encoding) {
                case ENCODING_G711A:
                    v=(float)g711a.decode(buffer[i]&0xFF)/32767.0F;
                    i++;
                    break;
                case ENCODING_G711U:
                    v=(float)g711u.decode(buffer[i]&0xFF)/32767.0F;
                    i++;
                    break;
                case ENCODING_PCM:
                    v=(float)(((buffer[i]&0xFF)<<8)+(buffer[i]&0xFF))/31767.0F;
                    break;
                }
                for(int j=0;j<upsample;j++) {
                    data->addMicSample(v);
                }
            }
        } else {
            qDebug()<<"Client::micReadyRead: read:"<<bytes_read;
        }
    }
}

float Client::getFilterSizeCalibrationOffset() {
    int size=1024; // dspBufferSize
    float i=log10((float)size);
    return 3.0f*(11.0f-i);
}
