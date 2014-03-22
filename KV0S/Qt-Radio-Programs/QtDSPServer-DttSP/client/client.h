#ifndef CLIENT_H
#define CLIENT_H

#include <QObject>
#include <QHostAddress>
#include <QTcpSocket>
#include <QUdpSocket>
#include <QMutex>

#include "g711a.h"
#include "g711u.h"
#include "g721.h"

#define HEADER_SIZE 13

#define SPECTRUM_BUFFER 0
#define CONFIG_BUFFER 1

#define HEADER_VERSION 2
#define HEADER_SUBVERSION 0

#define ENCODING_G711A 0
#define ENCODING_G711U 1
#define ENCODING_PCM 2
#define ENCODING_G721 3

#define AUDIO_FRAMES_PER_SECOND 100

class Client : public QObject
{
    Q_OBJECT
public:
    explicit Client(QTcpSocket* s,int rx,QObject *parent = 0);
    void send();
    QHostAddress getPeerAddress();
    void sendAudio(float* left,float* right,int length);
    void setSampleRate(int rate);

signals:
    void sendCommand(QString command);
    void disconnected(Client*);

public slots:
    void readyRead();
    void disconnected();
    void receiveAudio();

private:
    QTcpSocket* socket;
    int receiver;

    float getFilterSizeCalibrationOffset();

    float multimeterCalibrationOffset;
    float displayCalibrationOffset;
    float squelchCalibrationOffset;

    int sampleRate;

    QUdpSocket* audioSocket;
    int audio_size;
    int audio_port;
    int audio_rate;
    int audio_channels;
    int audio_encoding;
    int audioBufferSize;
    QByteArray audioBuffer;
    int audioBufferOffset;
    int audioBufferCount;

    QString client_type;

    G711a g711a;
    G711u g711u;
    G721  g721;

    QMutex mutex;

    qint64 sequence;
};

#endif // CLIENT_H
