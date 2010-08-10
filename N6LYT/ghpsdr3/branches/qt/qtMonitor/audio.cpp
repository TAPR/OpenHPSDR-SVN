/* 
 * File:   audio.cpp
 * Author: john
 * 
 * Created on 06 August 2010, 16:17
 */

#include "audio.h"

audio::audio() {
}

audio::~audio() {
}

void audio::initialize_audio(int buffer_size) {
    qDebug() << "initializ_audio " << buffer_size;

    decoded_buffer.resize(buffer_size*2);

    init_decodetable();

    audio_format.setFrequency(8000);
    audio_format.setChannels(1);
    audio_format.setSampleSize(16);
    audio_format.setCodec("audio/pcm");
    audio_format.setByteOrder(QAudioFormat::BigEndian);
    audio_format.setSampleType(QAudioFormat::SignedInt);

    //QAudioDeviceInfo info(QAudioDeviceInfo::availableDevices(QAudio::AudioOutput));

    foreach(const QAudioDeviceInfo &deviceInfo,QAudioDeviceInfo::availableDevices(QAudio::AudioOutput)) {
        qDebug() << deviceInfo.deviceName();
        if(deviceInfo.deviceName()=="pulse") {
            qDebug() << "found pulse";
            audio_device=deviceInfo;
        }
    }

    //QAudioDeviceInfo info(QAudioDeviceInfo::defaultOutputDevice());
    if (!audio_device.isFormatSupported(audio_format)) {
        qWarning() << "Default format not supported - trying to use nearest";
        audio_format = audio_device.nearestFormat(audio_format);

        qDebug() << "channels: " << audio_format.channels();
        qDebug() << "frequency: " << audio_format.frequency();
        qDebug() << "byteorder: " << audio_format.byteOrder();
        qDebug() << "samplesize: " << audio_format.sampleSize();
    }

    qDebug() << audio_device.deviceName();
    
    qDebug() << "delete";
    //delete audio_output;
    audio_output = 0;
    qDebug() << "new QAudiiOutput";
    audio_output = new QAudioOutput(audio_device, audio_format, this);
    //connect(audio_output, SIGNAL(notify()), SLOT(notified()));
    //connect(audio_output, SIGNAL(stateChanged(QAudio::State)), SLOT(stateChanged(QAudio::State)));
    qDebug() << "start";
    audio_out=audio_output->start();


}

void audio::process_audio(char* buffer) {
    //qDebug() << "process audio";
    aLawDecode(buffer);
    if(audio_out) {
        //qDebug() << "write audio data: " <<  audio_out->bytesToWrite();
        audio_out->write(decoded_buffer.data(),decoded_buffer.length());
    }
}

void audio::aLawDecode(char* buffer) {
    int i;
    short v;

    //qDebug() << "aLawDecode " << decoded_buffer.length();

    unsigned char *ptr = reinterpret_cast<unsigned char *> (decoded_buffer.data());

    for (int inIx=48; inIx < 48+decoded_buffer.length(); inIx++) {
        i=buffer[inIx]&0xFF;
        v=decodetable[i];

        // assumes BIGENDIAN
        *ptr++=(unsigned char)((v>>8)&0xFF);
        *ptr++=(unsigned char)(v&0xFF);
    }

}


void audio::init_decodetable() {
    qDebug() << "init_decodetable";
    for (int i = 0; i < 256; i++) {
        int input = i ^ 85;
        int mantissa = (input & 15) << 4;
        int segment = (input & 112) >> 4;
        int value = mantissa + 8;
        if (segment >= 1) {
            value += 256;
        }
        if (segment > 1) {
            value <<= (segment - 1);
        }
        if ((input & 128) == 0) {
            value = -value;
        }
        decodetable[i] = (short) value;

        qDebug() << "decodetable[" << i <<"]=" << decodetable[i];
    }
}
