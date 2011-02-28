#ifndef SERVER_H
#define SERVER_H

#include <QThread>
#include <QString>
#include <QList>
#include <QtNetwork>

#include "interfaces.h"
#include "metis.h"
#include "clientserver.h"
#include "receiver.h"
#include "error.h"


class Server : public QThread {
    Q_OBJECT
public:
    Server();

    void setDevice(QString d);
    void setInterface(QString i);
    void setMetis(QString m);

    QString getDevice();
    QString getInterface();
    QString getMetis();

    QString getDevicesHTML();
    QString getInterfacesHTML();
    QString getMetisHTML();

    QString getErrorHTML();

    int getMetisCount();

    QString getInterfaceIPAddress(QString iface);

    void discover();

    void clearMetis();
    void addMetis(Metis metis);

    void start();
    void stop();

    enum STATES {STOPPED,RUNNING};

    STATES getState();

    void setSampleRate(QString s);
    void setReceivers(QString r);
    QString getSampleRate();
    QString getReceivers();

    void sendBuffer();

    void send_metis_buffer(int ep,unsigned char* buffer);

    int getOzySoftwareVersion();
    int getMercurySoftwareVersion();
    int getPenelopeSoftwareVersion();

    int getReceiveSequenceError();
    int getReceivedFrames();

    QString getReceiversHTML();

    QString attach(int rx,Client* c);
    void setFrequency(int rx,long f);

    void playAudio(float* buffer);

    void clearError();
    void setError(QString e);

public slots:
    void readyRead();

private:
    QString device;
    QString iface;
    QString metis;

    Interfaces interfaces;;

    QList<Metis> metisCards;

    QHostAddress* metisAddress;
    QUdpSocket socket;

    STATES state;

    void startMetis();
    void stopMetis();


    QString sampleRate;
    QString receivers;

    unsigned char output_buffer[1032];
    unsigned char metis_buffer[512];
    int metis_buffer_index;

    unsigned long send_sequence;
    int receive_sequence_error;
    int offset;

    unsigned long receive_sequence;

    unsigned char control_out[5];
    unsigned char control_in[5];
    int ptt;
    int dash;
    int dot;

    int lt2208ADCOverflow;
    int IO1;
    int IO2;
    int IO3;
    int mercury_software_version;
    int penelope_software_version;
    int ozy_software_version;
    int forwardPower;
    int alexForwardPower;
    int AIN3;
    int AIN4;
    int AIN6;

    float mic_gain;

    int samples;

    unsigned long rx_frame;

    void process_iq_buffer(unsigned char* buffer);

    int start_frames;

    Receiver* receiver[4];
    int current_receiver;

    ClientServer* clientServer;

    int send_frequency;

    Error error;

};

#endif // SERVER_H
