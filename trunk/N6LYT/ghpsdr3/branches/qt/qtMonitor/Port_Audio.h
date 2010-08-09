/* 
 * File:   portaudio.h
 * Author: john
 *
 * Created on 08 August 2010, 18:55
 */

#ifndef PORT_AUDIO_H
#define	PORT_AUDIO_H

#include <QtCore>

#include <portaudio.h>


class Port_Audio : public QObject {
    Q_OBJECT
public:
    Port_Audio();
    virtual ~Port_Audio();
    void initialize_audio();
    void process_audio(char* buffer);
    
private:
    void aLawDecode(char* buffer, char*decoded_buffer);
    void init_decodetable();
    short decodetable[256];
    PaStream* stream;

};

#endif	/* PORT_AUDIO_H */

