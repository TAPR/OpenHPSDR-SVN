/* 
 * File:   audio.cpp
 * Author: john
 * 
 * Created on 06 August 2010, 16:17
 */

#include "audio.h"

audio::audio() {
}

audio::audio(const audio& orig) {
}

audio::~audio() {
}

void audio::initialize_audio() {
    qDebug() << "initializ_audio";

    decoded_buffer.resize(480*2*6);

    init_decodetable();

    audio_format.setFrequency(48000);
    audio_format.setChannels(1);
    audio_format.setSampleSize(16);
    audio_format.setCodec("audio/pcm");
    audio_format.setByteOrder(QAudioFormat::BigEndian);
    audio_format.setSampleType(QAudioFormat::SignedInt);

    QAudioDeviceInfo info(QAudioDeviceInfo::defaultOutputDevice());
    if (!info.isFormatSupported(audio_format)) {
        qWarning() << "Default format not supported - trying to use nearest";
        audio_format = info.nearestFormat(audio_format);
    }

    qDebug() << info.deviceName();
    
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
    aLawDecode(buffer,decoded_buffer,6);
    

    if(audio_out) {
        //qDebug() << "write audio data: " <<  audio_out->bytesToWrite();
        audio_out->write(decoded_buffer.data(),(long long)480*2*6);
    }
}

void audio::aLawDecode(char* buffer,QByteArray decoded_buffer,int replications) {
    int i;
    int j;
    short v;

    //qDebug() << "aLawDecode";

    unsigned char *ptr = reinterpret_cast<unsigned char *> (decoded_buffer.data());

    for (int inIx=48, outIx=0; inIx < 48+480; inIx++) {
        i=buffer[inIx]&0xFF;
        v=decodetable[i];
        // assumes BIGENDIAN
        for(j=0;j<replications;j++) {
            *ptr++=(char)((v>>8)&0xFF);
            *ptr++=(char)(v&0xFF);
        }
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
    }
}
