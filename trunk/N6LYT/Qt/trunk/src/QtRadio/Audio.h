/*
 * File:   Audio.h
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

#ifndef AUDIO_H
#define	AUDIO_H

#include "g711a.h"
#include "g711u.h"
#include "g721.h"

#include <QtCore>
#include <QtNetwork/QUdpSocket>
#include <QtGui/QComboBox>
#ifdef Q_OS_MAC
#include <QtMultimedia/QAudioFormat>
#include <QtMultimedia/QAudioOutput>
#include <QtMultimedia/QAudioInput>
#include <QtMultimedia/QAudioDeviceInfo>
#else
#include <QtMultimedia/QAudioFormat>
#include <QtMultimedia/QAudioOutput>
#include <QtMultimedia/QAudioInput>
#include <QtMultimedia/QAudioDeviceInfo>
#endif

#define ENCODING_G711A 0
#define ENCODING_G711U 1
#define ENCODING_PCM 2
#define ENCODING_G721 3

// 8000/50 for 20ms
#define AUDIO_FRAMES_PER_SECOND 100

class Audio : public QObject {
    Q_OBJECT
public:
    Audio();
    Audio(const Audio& orig);
    virtual ~Audio();
    void init(int p);
    void initialize_audio();
    void reset();
    void select_audio(QAudioDeviceInfo info,int rate,int channels,QAudioFormat::Endian byteOrder,QString encoding);
    void get_audio_devices(QComboBox* comboBox);

    int get_sample_rate();
    int get_channels();
    int get_encoding();

    void mox(bool state);

public slots:
    void readyRead();
    void networkReadyRead();

private:
    G711a g711a;
    G711u g711u;
    G721 g721;

    QAudioFormat     audio_format;
    QAudioOutput*    audio_output;
    QAudioInput*     audio_input;
    QAudioDeviceInfo audio_device;
    QIODevice*       audio_out;
    QIODevice*       audio_in;

    int audio_encoding;

    int sampleRate;
    int audio_channels;
    QAudioFormat::Endian audio_byte_order;

    QUdpSocket* socket;
    int port;

    QHostAddress remote_host;
    quint16 remote_port;

    int audio_buffer_size;
    int encoded_buffer_size;

    char* input_buffer;
    char* encoded_buffer;
    int encoded_offset;
    int encoded_nibble;

    char* output_buffer;
    char* decoded_buffer;
    int decoded_nibble;

    qint64 sequence;
    bool skip;

};

#endif	/* AUDIO_H */
