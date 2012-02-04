#ifndef RECEIVER_H
#define RECEIVER_H

#include <QString>
#include <QtNetwork/QUdpSocket>
#include <QProcess>

#include "../client/client.h"

#define BUFFER_SIZE 1024

class Receiver : public QObject {
    Q_OBJECT
public:
    Receiver(int local_port);
    ~Receiver();

    void init(int rx);
    int getRx();

    int attach(Client* c);
    int detach(Client* c);
    Client* getClient();

    int start(Client* c,QHostAddress host,int p);
    int stop(Client* c);
    void setIQPort(int port);
    void setBandscopePort(int port);

    void put_iq_samples(int index,float left,float right);
    void put_mic_samples(int index,float mic);
    void send_IQ_buffer();

    int setClientType(Client* c,QString type);
    QString getClientType();

    int setFrequency(Client* c,long f);
    int setFrequency(Client* c,long f,int b);
    long getFrequency();
    int getBand();

    int get_iq_port();
    int get_bandscope_port();

    void send_audio_buffer(float* audio_buffer);

    void setPlayAudio(int state);
    int getPlayAudio();

    void setMox(int state);

    void startDspServer();
    void stopDspServer();

    QString getDspServerInfo();

public slots:
    void readPendingDatagrams();
    void dspServerStarted();
    void dspServerError(QProcess::ProcessError);
    void dspServerReadyReadStandardError();
    void dspServerReadyReadStandardOutput();


private:
    quint64 getProcessID(const QProcess* proc);

    QProcess* dspServer;

    Client* client;

    QString client_type;

    QHostAddress client_address;
    int iq_port;
    int bandscope_port;

    int rx;
    int play_audio;

    float input_buffer[3*BUFFER_SIZE];  // I,Q,Mic
    float output_buffer[4*BUFFER_SIZE]; // Audio L/R and Transmit I/Q

    long frequency;
    int frequency_changed;
    int band;

    QUdpSocket* udpSocket;

    quint64 rx_sequence;
    quint64 tx_sequence;
    int tx_offset;

    int mox;

};

#endif // RECEIVER_H
