/* 
 * File:   Audio.h
 * Author: john
 *
 * Created on 16 August 2010, 11:19
 */

#ifndef AUDIO_H
#define	AUDIO_H

#include <QtCore>
#include <QtMultimedia/QAudioFormat>
#include <QtMultimedia/QAudioOutput>
#include <QtMultimedia/QAudioDeviceInfo>
#include <QtGui/QComboBox>

#define AUDIO_BUFFER_SIZE 480

#define BIGENDIAN

// There are problems running at 8000 samples per second on Mac OS X
// The resolution is to run at 8011 samples persecond.
#define SAMPLE_RATE_FUDGE 11

class Audio : public QObject {
    Q_OBJECT
public:
    Audio();
    Audio(const Audio& orig);
    virtual ~Audio();
    void initialize_audio(int buffer_size);
    void select_audio(QAudioDeviceInfo info,int rate,int channels,QAudioFormat::Endian byteOrder);
    void process_audio(char* header,char* buffer,int length);
    void get_audio_devices(QComboBox* comboBox);

    int get_sample_rate();
    int get_channels();

private:
    void aLawDecode(char* buffer,int length);
    void init_decodetable();
    QAudioFormat     audio_format;
    QAudioOutput*    audio_output;
    QAudioDeviceInfo audio_device;
    QIODevice*       audio_out;
    QByteArray       decoded_buffer;
    short decodetable[256];

    int sampleRate;
    int audio_channels;
    QAudioFormat::Endian audio_byte_order;

};

#endif	/* AUDIO_H */
