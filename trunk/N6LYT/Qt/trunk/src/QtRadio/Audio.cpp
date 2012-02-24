/* 
 * File:   Audio.cpp
 * Author: John Melton, G0ORX/N6LYT
 * 
 * Created on 16 August 2010, 11:19
 */

/* Copyright (C)
* 2009 - John Melton, G0ORX/N6LYT
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*
*/

#include "Audio.h"

Audio::Audio() {
    remote_port=0;

    sequence=0;

    audio_output=NULL;
    audio_out=NULL;
    sampleRate=8000;
    audio_channels=1;
    audio_byte_order=QAudioFormat::LittleEndian;
    audio_encoding=ENCODING_G711A;

    audio_buffer_size=sampleRate*audio_channels*sizeof(short);
    input_buffer=(char*)malloc(audio_buffer_size);
    encoded_buffer_size=8+((sampleRate*audio_channels)/AUDIO_FRAMES_PER_SECOND); // 20ms + 8 byte sequence
    encoded_buffer=(char*)malloc(encoded_buffer_size);

    output_buffer=(char*)malloc(encoded_buffer_size);
    decoded_buffer=(char*)malloc(audio_buffer_size);

    qDebug() << "Audio: LittleEndian=" << QAudioFormat::LittleEndian << " BigEndian=" << QAudioFormat::BigEndian;

    qDebug() << "Audio: sampleRate:"<<sampleRate<<" channels:"<<audio_channels<<" encoding:"<<audio_encoding<<" audio_buffer_size:"<<audio_buffer_size<<" encoded_buffer_size:"<<encoded_buffer_size;

    audio_format.setSampleType(QAudioFormat::SignedInt);
    audio_format.setFrequency(sampleRate);
    audio_format.setChannels(audio_channels);
    audio_format.setSampleSize(16);
    audio_format.setCodec("audio/pcm");
    audio_format.setByteOrder(audio_byte_order);

    skip=false;
}

Audio::~Audio() {
}

void Audio::init(int p) {
    port=p;

    socket = new QUdpSocket(this);

    qDebug()<<"Audio::Audio bind to port "<<port;
    socket->bind(port, QUdpSocket::ShareAddress);
    connect(socket,SIGNAL(readyRead()),this,SLOT(networkReadyRead()));

}

void Audio::reset() {
    sequence=0;
}

int Audio::get_sample_rate() {
    return sampleRate;
}

int Audio::get_channels() {
    return audio_channels;
}

int Audio::get_encoding() {
    return audio_encoding;
}

void Audio::initialize_audio() {

    qDebug()<<"Audio::initialize_audio: encoding:"<<audio_encoding<<" rate:"<<sampleRate<<" channels:"<<audio_channels;

    audio_buffer_size=sampleRate*audio_channels*sizeof(short);
    encoded_buffer_size=8+((sampleRate*audio_channels)/AUDIO_FRAMES_PER_SECOND); // + 8 byte sequence
    if(audio_encoding==ENCODING_PCM) {
        encoded_buffer_size=encoded_buffer_size*2;
    }

    free(input_buffer);
    free(encoded_buffer);
    input_buffer=(char*)malloc(audio_buffer_size);
    encoded_buffer=(char*)malloc(encoded_buffer_size);

    free(output_buffer);
    free(decoded_buffer);
    output_buffer=(char*)malloc(encoded_buffer_size);
    decoded_buffer=(char*)malloc(audio_buffer_size);

    audio_format.setFrequency(sampleRate);
    audio_format.setChannels(audio_channels);
    audio_format.setSampleSize(16);
    audio_format.setCodec("audio/pcm");
    audio_format.setByteOrder(audio_byte_order);
}

void Audio::get_audio_devices(QComboBox* comboBox) {

    QList<QAudioDeviceInfo> devices=QAudioDeviceInfo::availableDevices(QAudio::AudioOutput);
    QAudioDeviceInfo device_info;

    qDebug() << "Audio::get_audio_devices";
    for(int i=0;i<devices.length();i++) {
        device_info=devices.at(i);
        qDebug() << "Audio::get_audio_devices: " << device_info.deviceName();

        qDebug() << "    Codecs:";
        QStringList codecs=device_info.supportedCodecs();
        for(int j=0;j<codecs.size();j++) {
            qDebug() << "        " << codecs.at(j).toLocal8Bit().constData();
        }

        qDebug() << "    Byte Orders";
        QList<QAudioFormat::Endian> byteOrders=device_info.supportedByteOrders();
        for(int j=0;j<byteOrders.size();j++) {
            qDebug() << "        " << (byteOrders.at(j)==QAudioFormat::BigEndian?"BigEndian":"LittleEndian");
        }

        qDebug() << "    Sample Type";
        QList<QAudioFormat::SampleType> sampleTypes=device_info.supportedSampleTypes();
        for(int j=0;j<sampleTypes.size();j++) {
            if(sampleTypes.at(j)==QAudioFormat::Unknown) {
                qDebug() << "        Unknown";
            } else if(sampleTypes.at(j)==QAudioFormat::SignedInt) {
                qDebug() << "        SignedInt";
            } else if(sampleTypes.at(j)==QAudioFormat::UnSignedInt) {
                qDebug() << "        UnSignedInt";
            } else if(sampleTypes.at(j)==QAudioFormat::Float) {
                qDebug() << "        Float";
            }
        }

        qDebug() << "    Sample Rates";
        QList<int> sampleRates=device_info.supportedFrequencies();
        for(int j=0;j<sampleRates.size();j++) {
            qDebug() << "        " << sampleRates.at(j);
        }

        qDebug() << "    Sample Sizes";
        QList<int> sampleSizes=device_info.supportedSampleSizes();
        for(int j=0;j<sampleSizes.size();j++) {
            qDebug() << "        " << sampleSizes.at(j);
        }

        qDebug() << "    Channels";
        QList<int> channels=device_info.supportedChannels();
        for(int j=0;j<channels.size();j++) {
            qDebug() << "        " << channels.at(j);
        }

        comboBox->addItem(device_info.deviceName(),qVariantFromValue(device_info));
        if(i==0) {
            audio_device=device_info;
        }
        i++;
    }

    qDebug() << "Audio::get_audio_devices: default is " << audio_device.deviceName();

    audio_output = new QAudioOutput(audio_device, audio_format, this);
    audio_input = new QAudioInput(audio_device, audio_format, this);

    qDebug() << "QAudioOutput: error=" << audio_output->error() << " state=" << audio_output->state();
    qDebug() << "QAudioInput: error=" << audio_input->error() << " state=" << audio_input->state();

    audio_out = audio_output->start();

    if(audio_output->error()!=0) {
        qDebug() << "QAudioOutput: after start error=" << audio_output->error() << " state=" << audio_output->state();

        qDebug() << "Format:";
        qDebug() << "    sample rate: " << audio_format.frequency();
        qDebug() << "    codec: " << audio_format.codec();
        qDebug() << "    byte order: " << audio_format.byteOrder();
        qDebug() << "    sample size: " << audio_format.sampleSize();
        qDebug() << "    sample type: " << audio_format.sampleType();
        qDebug() << "    channels: " << audio_format.channels();
        audio_out=NULL;
    }

    if(audio_out==NULL) {
        qDebug() << "Audio::selectAudio: audio_out is NULL!";
    }

}

void Audio::select_audio(QAudioDeviceInfo info,int rate,int channels,QAudioFormat::Endian byteOrder,QString encoding) {
    qDebug() << "selected audio " << info.deviceName() <<  " sampleRate:" << rate << " Channels: " << channels << " Endian:" << (byteOrder==QAudioFormat::BigEndian?"BigEndian":"LittleEndian")<<" encoding:"<<encoding;

    sampleRate=rate;
    audio_channels=channels;
    audio_byte_order=byteOrder;

    if(encoding=="G711a") {
        audio_encoding=ENCODING_G711A;
        free(input_buffer);
        free(encoded_buffer);
        audio_buffer_size=sampleRate*audio_channels*sizeof(short)/AUDIO_FRAMES_PER_SECOND; // 20ms
        input_buffer=(char*)malloc(audio_buffer_size);
        encoded_buffer_size=8+((sampleRate*audio_channels)/AUDIO_FRAMES_PER_SECOND); // 20ms
        encoded_buffer=(char*)malloc(encoded_buffer_size);
        free(output_buffer);
        free(decoded_buffer);
        output_buffer=(char*)malloc(encoded_buffer_size);
        decoded_buffer=(char*)malloc(audio_buffer_size);
    } else if (encoding=="G711u") {
        audio_encoding=ENCODING_G711U;
        free(input_buffer);
        free(encoded_buffer);
        audio_buffer_size=sampleRate*audio_channels*sizeof(short)/AUDIO_FRAMES_PER_SECOND;
        input_buffer=(char*)malloc(audio_buffer_size);
        encoded_buffer_size=8+((sampleRate*audio_channels)/AUDIO_FRAMES_PER_SECOND); // 20ms
        encoded_buffer=(char*)malloc(encoded_buffer_size);
        free(output_buffer);
        free(decoded_buffer);
        output_buffer=(char*)malloc(encoded_buffer_size);
        decoded_buffer=(char*)malloc(audio_buffer_size);
    } else if (encoding=="PCM") {
        audio_encoding=ENCODING_PCM;
        free(input_buffer);
        free(encoded_buffer);
        audio_buffer_size=sampleRate*audio_channels*sizeof(short)/AUDIO_FRAMES_PER_SECOND;
        input_buffer=(char*)malloc(audio_buffer_size);
        encoded_buffer_size=8+((sampleRate*audio_channels*sizeof(short))/AUDIO_FRAMES_PER_SECOND); // 20ms
        encoded_buffer=(char*)malloc(encoded_buffer_size);
        free(output_buffer);
        free(decoded_buffer);
        output_buffer=(char*)malloc(encoded_buffer_size);
        decoded_buffer=(char*)malloc(audio_buffer_size);
    } else if (encoding=="G721") {
        audio_encoding=ENCODING_G721;
        free(input_buffer);
        free(encoded_buffer);
        audio_buffer_size=sampleRate*audio_channels*sizeof(short)/AUDIO_FRAMES_PER_SECOND;
        input_buffer=(char*)malloc(audio_buffer_size);
        encoded_buffer_size=8+((sampleRate*audio_channels)/AUDIO_FRAMES_PER_SECOND/2); // 20ms
        encoded_buffer=(char*)malloc(encoded_buffer_size);
        free(output_buffer);
        free(decoded_buffer);
        output_buffer=(char*)malloc(encoded_buffer_size);
        decoded_buffer=(char*)malloc(audio_buffer_size);
    } else {
        qDebug()<<"Audio::select_audio: invalid encoding:"<<encoding;
        audio_encoding=ENCODING_G711A;
        free(input_buffer);
        free(encoded_buffer);
        audio_buffer_size=sampleRate*audio_channels*sizeof(short)/AUDIO_FRAMES_PER_SECOND;
        input_buffer=(char*)malloc(audio_buffer_size);
        encoded_buffer_size=8+((sampleRate*audio_channels)/AUDIO_FRAMES_PER_SECOND); // 20ms
        encoded_buffer=(char*)malloc(encoded_buffer_size);
        free(output_buffer);
        free(decoded_buffer);
        output_buffer=(char*)malloc(encoded_buffer_size);
        decoded_buffer=(char*)malloc(audio_buffer_size);
    }

    if(audio_output!=NULL) {
        audio_output->stop();
        audio_output->disconnect(this);
        audio_output=NULL;
        audio_out=NULL;
    }

    qDebug() << "Audio: sampleRate:"<<sampleRate<<" channels:"<<audio_channels<<" encoding:"<<audio_encoding<<" audio_buffer_size:"<<audio_buffer_size<<" encoded_buffer_size:"<<encoded_buffer_size;

    audio_device=info;

    audio_format.setFrequency(sampleRate);
    audio_format.setChannels(audio_channels);
    audio_format.setSampleSize(16);
    audio_format.setCodec("audio/pcm");
    audio_format.setByteOrder(audio_byte_order);

    if (!audio_device.isFormatSupported(audio_format)) {
        qDebug()<<"Audio format not supported by device.";
    }

    audio_output = new QAudioOutput(audio_device, audio_format, this);
    audio_input = new QAudioInput(audio_device, audio_format, this);

    qDebug() << "QAudioOutput: error=" << audio_output->error() << " state=" << audio_output->state();

    audio_out = audio_output->start();

    if(audio_output->error()!=0) {
        qDebug() << "QAudioOutput: after start error=" << audio_output->error() << " state=" << audio_output->state();

        qDebug() << "Format:";
        qDebug() << "    sample rate: " << audio_format.frequency();
        qDebug() << "    codec: " << audio_format.codec();
        qDebug() << "    byte order: " << audio_format.byteOrder();
        qDebug() << "    sample size: " << audio_format.sampleSize();
        qDebug() << "    sample type: " << audio_format.sampleType();
        qDebug() << "    channels: " << audio_format.channels();
        audio_out=NULL;
    }

    if(audio_out==NULL) {
        qDebug() << "Audio::selectAudio: audio_out is NULL!";
    }

}

void Audio::mox(bool state) {
    qDebug()<<"Audio::mox"<<state;
    if(state) {
        audio_in=audio_input->start();
        encoded_offset=0;
        encoded_nibble=0;
        connect(audio_in,SIGNAL(readyRead()),this,SLOT(readyRead()));
    } else {
        audio_input->stop();
        disconnect(audio_in,SIGNAL(readyRead()));
    }
}

void Audio::readyRead() {
    qint64 bytes;

    qDebug()<<"readyRead";
    bytes=audio_in->read(input_buffer,(qint64)sizeof(input_buffer));

    short left;
    short right;
    unsigned char s;
    int i=0;
    while(i<bytes) {
        if(audio_byte_order==QAudioFormat::LittleEndian) {
            switch(audio_channels) {
            case 1:
                left=(input_buffer[i]&0xFF);
                i++;
                left=left+((input_buffer[i]&0xFF)<<8);
                i++;
                break;
            case 2:
                right=(input_buffer[i]&0xFF);
                i++;
                right=left+((input_buffer[i]&0xFF)<<8);
                i++;
                right=(input_buffer[i]&0xFF);
                i++;
                right=left+((input_buffer[i]&0xFF)<<8);
                i++;
                break;
            }
        } else { // BigEndian
            switch(audio_channels) {
            case 1:
                left=left+((input_buffer[i]&0xFF)<<8);
                i++;
                left=(input_buffer[i]&0xFF);
                i++;
                break;
            case 2:
                right=left+((input_buffer[i]&0xFF)<<8);
                i++;
                right=(input_buffer[i]&0xFF);
                i++;
                right=left+((input_buffer[i]&0xFF)<<8);
                i++;
                right=(input_buffer[i]&0xFF);
                i++;
                break;
            }
        }
        switch(audio_encoding) {
        case ENCODING_G711A:
            switch(audio_channels) {
            case 1:
                encoded_buffer[encoded_offset]=g711a.encode((left+right)/2);
                encoded_offset++;
                break;
            case 2:
                encoded_buffer[encoded_offset]=g711a.encode(left);
                encoded_offset++;
                encoded_buffer[encoded_offset]=g711a.encode(right);
                encoded_offset++;
                break;
            }
            break;
        case ENCODING_G711U:
            switch(audio_channels) {
            case 1:
                encoded_buffer[encoded_offset]=g711u.encode((left+right)/2);
                encoded_offset++;
                break;
            case 2:
                encoded_buffer[encoded_offset]=g711u.encode(left);
                encoded_offset++;
                encoded_buffer[encoded_offset]=g711u.encode(right);
                encoded_offset++;
                break;
            }
            break;
        case ENCODING_PCM:
            switch(audio_channels) {
            case 1:
                encoded_buffer[encoded_offset]=((left+right)/2)>>8;
                encoded_offset++;
                encoded_buffer[encoded_offset]=(left+right)/2;
                encoded_offset++;
                break;
            case 2:
                encoded_buffer[encoded_offset]=left>>8;
                encoded_offset++;
                encoded_buffer[encoded_offset]=left;
                encoded_offset++;
                encoded_buffer[encoded_offset]=right>>8;
                encoded_offset++;
                encoded_buffer[encoded_offset]=right;
                encoded_offset++;
                break;
            }
            break;
        case ENCODING_G721:
            switch(audio_channels) {
            case 1:
                if(encoded_nibble==0) {
                    encoded_buffer[encoded_offset]=g721.encode((left+right)/2)<<4;
                    encoded_nibble++;
                } else {
                    encoded_buffer[encoded_offset]+=g721.encode((left+right)/2)&0x0F;
                    encoded_nibble=0;
                }
                break;
            case 2:
                encoded_buffer[encoded_offset]=(g721.encode(left)<<4)+(g721.encode(right)&0x0F);
                encoded_offset++;
                break;
            }

            break;
        }

        if(encoded_offset==encoded_buffer_size) {
            //qDebug()<<"Audio::readyRead: writeDatagram:"<<remote_host.toString()<<":"<<remote_port;
            socket->writeDatagram(encoded_buffer,(qint64)encoded_buffer_size,remote_host,remote_port);
            encoded_offset=0;
        }
    }
}

void Audio::networkReadyRead() {
    QHostAddress host;
    quint16 port;
    int length;
    qint64 this_sequence;
    while(socket->hasPendingDatagrams()) {
        length=socket->readDatagram(output_buffer,encoded_buffer_size,&host,&port);
        if(remote_port==0) {
            remote_host=host;
            remote_port=port;
        }
        if(length==encoded_buffer_size) {
            int i=0;
            int j=0;
            short left;
            short right;
            decoded_nibble=0;

            // first 8 bytes are sequence
            this_sequence=(output_buffer[0]&0xFF)<<56;
            this_sequence+=(output_buffer[1]&0xFF)<<48;
            this_sequence+=(output_buffer[2]&0xFF)<<40;
            this_sequence+=(output_buffer[3]&0xFF)<<32;
            this_sequence+=(output_buffer[4]&0xFF)<<24;
            this_sequence+=(output_buffer[5]&0xFF)<<16;
            this_sequence+=(output_buffer[6]&0xFF)<<8;
            this_sequence+=(output_buffer[7]&0xFF);

            if(this_sequence!=sequence) {
                qDebug()<<"this_sequence:"<<this_sequence<<" expected:"<<sequence;
            }


            //if((sequence%AUDIO_FRAMES_PER_SECOND)==0) {
            //    qDebug()<<sequence;
            //}

            sequence=this_sequence+1;

            i=8;
            while(i<length) {
                switch(audio_encoding) {
                case ENCODING_G711A:
                    switch(audio_channels) {
                    case 1:
                        left=g711a.decode(output_buffer[i]&0xFF);
                        i++;
                        break;
                    case 2:
                        left=g711a.decode(output_buffer[i]&0xFF);
                        i++;
                        right=g711a.decode(output_buffer[i]&0xFF);
                        i++;
                        break;
                    }
                    break;
                case ENCODING_G711U:
                    switch(audio_channels) {
                    case 1:
                        left=g711u.decode(output_buffer[i]&0xFF);
                        i++;
                        break;
                    case 2:
                        left=g711u.decode(output_buffer[i]&0xFF);
                        i++;
                        right=g711u.decode(output_buffer[i]&0xFF);
                        i++;
                        break;
                    }
                    break;
                case ENCODING_PCM:
                    switch(audio_channels) {
                    case 1:
                        left=(output_buffer[i]&0xFF)<<8;
                        i++;
                        left=left+(output_buffer[i]&0xFF);
                        i++;
                        break;
                    case 2:
                        left=(output_buffer[i]&0xFF)<<8;
                        i++;
                        left=left+(output_buffer[i]&0xFF);
                        i++;
                        right=(output_buffer[i]&0xFF)<<8;
                        i++;
                        right=left+(output_buffer[i]&0xFF);
                        i++;
                        break;
                    }
                    break;
                case ENCODING_G721:
                    switch(audio_channels) {
                    case 1:
                        if(decoded_nibble==0) {
                            left=g721.decode((output_buffer[i]>>4)&0x0F);
                            decoded_nibble++;
                        } else {
                            left=g721.decode(output_buffer[i]&0x0F);
                            decoded_nibble=0;
                            i++;
                        }
                        break;
                    case 2:
                        left=g721.decode((output_buffer[i]>>4)&0x0F);
                        right=g721.decode(output_buffer[i]&0x0F);
                        i++;
                        break;
                    }
                    break;
                }
                if(audio_byte_order==QAudioFormat::LittleEndian) {
                    switch(audio_channels) {
                    case 1:
                        decoded_buffer[j]=(char)(left&0xFF);
                        j++;
                        decoded_buffer[j]=(char)((left>>8)&0xFF);
                        j++;
                        break;
                    case 2:
                        decoded_buffer[j]=(char)(left&0xFF);
                        j++;
                        decoded_buffer[j]=(char)((left>>8)&0xFF);
                        j++;
                        decoded_buffer[j]=(char)(right&0xFF);
                        j++;
                        decoded_buffer[j]=(char)((right>>8)&0xFF);
                        j++;
                        break;
                    }
                } else {
                    switch(audio_channels) {
                    case 1:
                        decoded_buffer[j]=(char)((left>>8)&0xFF);
                        j++;
                        decoded_buffer[j]=(char)(left&0xFF);
                        j++;
                        break;
                    case 2:
                        decoded_buffer[j]=(char)((left>>8)&0xFF);
                        j++;
                        decoded_buffer[j]=(char)(left&0xFF);
                        j++;
                        decoded_buffer[j]=(char)((right>>8)&0xFF);
                        j++;
                        decoded_buffer[j]=(char)(right&0xFF);
                        j++;
                        break;
                    }
                }
            }

            audio_out->write(decoded_buffer,(qint64)j);

        } else {
            qDebug()<<"Audio::networkReadyRead: length not encoded_buffer_size: "<<length;
        }

    }
}

