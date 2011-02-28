#ifndef RECEIVER_H
#define RECEIVER_H

#include <QString>
#include <QUdpSocket>

#define BUFFER_SIZE 1024

class Client;
class Audio;

class Receiver
{
public:
    Receiver();
    ~Receiver();

    void init(int rx);
    int getRx();

    QString attach(Client* c);
    QString detach(Client* c);
    void put_samples(int index,float left,float right);
    void send_IQ_buffer();

    void setFrequency(long f);
    long getFrequency();

    Client* getClient();

    void send_audio_buffer(float* audio_buffer);

    void setPlayAudio(int state);
    int getPlayAudio();

private:
    Client* client;
    Audio* audio;
    int rx;
    int play_audio;

    float input_buffer[2*BUFFER_SIZE];
    float output_buffer[2*BUFFER_SIZE];

    long frequency;
    int frequency_changed;

    QUdpSocket socket;

    quint64 sequence;

};

#endif // RECEIVER_H
