/* 
 * File:   audio.h
 * Author: john
 *
 * Created on 06 August 2010, 16:17
 */

#ifndef AUDIO_H
#define	AUDIO_H

#include <QtCore>
#include <qt4/QtMultimedia/QAudioFormat>
#include <qt4/QtMultimedia/QAudioOutput>
#include <qt4/QtMultimedia/QAudioDeviceInfo>

class audio : public QObject {
    Q_OBJECT
public:
    audio();
    audio(const audio& orig);
    virtual ~audio();
    void initialize_audio();
    void process_audio(char* buffer);
private:
    void aLawDecode(char* buffer, char*decoded_buffer);
    void init_decodetable();
    QAudioFormat     audio_format;
    QAudioOutput*    audio_output;
    QAudioDeviceInfo audio_device;
    QIODevice*       audio_out;
    short decodetable[256];


};

#endif	/* AUDIO_H */

