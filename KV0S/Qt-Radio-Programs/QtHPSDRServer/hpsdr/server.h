#ifndef SERVER_H
#define SERVER_H

#include <QThread>
#include <QString>
#include <QList>
#include <QtNetwork/QUdpSocket>
#include <QtNetwork/QHostAddress>
#include <QSettings>

#include "interfaces.h"
#include "metis.h"
#include "receiver.h"
#include "xvtr.h"
#include "error.h"
#include "../client/client.h"

#define MIN_FREQUENCY 100
#define MAX_FREQUENCY 55000000

#define XVTRS 4

class Server : public QThread {
    Q_OBJECT
public:
    static Server* getInstance();

    void configure(QSettings* mysettings);
    void save();

    int bind();

    QString getConfig();

    Interfaces* getInterfaces();

    void setHermes(bool state);
    bool getHermes();

    void setHPSDR(bool state);
    bool getHPSDR();
    void setOzy(bool state);
    bool getOzy();
    void setMetis(bool state);
    bool getMetis();

    void setMercury(bool state);
    bool getMercury();
    void setPenelope(bool state);
    bool getPenelope();
    void setPennylane(bool state);
    bool getPennylane();
    void setExcalibur(bool state);
    bool getExcalibur();

    void setAlex(bool state);
    bool getAlex();

    void setDevice(QString d);
    void setInterface(QString i);
    void setMetisDetail(QString m);

    QUdpSocket* getMetisSocket();
    void setMetisPort(int port);
    int getMetisPort();

    void setClientPort(int port);
    int getClientPort();

    QString getDevice();
    QString getInterface();
    QString getMetisDetail();

    QString getMetisText(int index);

    int getMetisCount();

    QString getInterfaceIPAddress(QString iface);

    void clearMetis();
    void addMetis(Metis metis);

    void start();
    void stop();

    enum STATES {STOPPED,RUNNING};

    STATES getState();

    Receiver* getReceiver(int r);
    void setReceivers(int r);
    void setSampleRate(int s);
    void set10MHzClock(QString c);
    void set122_88MHzClock(QString c);
    void setPreamp(bool s);
    void setRandom(bool s);
    void setDither(bool s);
    void setDuplex(bool s);
    void setClassE(bool s);
    void setLineIn(bool s);
    void setMicBoost(bool s);
    void setAutoStart(bool s);
    void setAutoStartDsp(bool s);
    void setAlexAttenuation(int v);

    int getSampleRate();
    int getReceivers();
    QString get10MHzClock();
    QString get122_88MHzClock();
    bool getPreamp();
    bool getRandom();
    bool getDither();
    bool getDuplex();
    bool getClassE();
    bool getLineIn();
    bool getMicBoost();
    bool getAutoStart();
    bool getAutoStartDsp();
    int getAlexAttenuation();

    void sendBuffer();

    void send_metis_buffer(int ep,unsigned char* buffer);

    int getOzySoftwareVersion();
    int getMercurySoftwareVersion();
    int getPenelopeSoftwareVersion();

    int getReceiveSequenceError();
    int getReceivedFrames();
    int getTransmittedFrames();

    QStringList getReceiverInfo(int receiver);

    int attachTo(Client* c,int receiver);
    int detachFrom(Client* c,int receiver);
    int startReceiver(Client* c,int rx,QHostAddress host,int port);
    int stopReceiver(Client* c,int rx);
    int setClientType(Client* c,int rx,QString type);
    int setFrequency(Client* c,int rx,long frequency);
    int setFrequencyAndBand(Client* c,int rx,long frequency,int band);
    int setMox(Client* c,int rx,int state);

    void playAudio(float* buffer);

    void clearError();
    void setError(QString e);

    void setMox(int state);
    int getMox();

    float getMicGain();
    void setMicGain(float gain);

    unsigned char getControlOut(int index);

    float getAudioGain();
    void setAudioGain(float gain);

    void setAlexRxAntenna(int a);
    void setAlexTxAntenna(int a);
    void setAlexRxOnlyAntenna(int a);

    void enableAudio(int rx);

    XVTR* getXvtrs();

    void setHardware(QString option,QString value);

protected:
    Server();

signals:
    void clientStateChanged();
    void firmwareVersionChanged();

public slots:
    void readyRead();

private:
    static Server* instance;

    bool hermes;

    bool hpsdr;
    bool ozy;
    bool metis;

    bool mercury;
    bool penelope;
    bool pennylane;
    bool excalibur;

    bool alex;

    int band;

    QString device; // Ozy or Metis
    QString iface;
    QString metisDetail;

    Interfaces interfaces;

    QList<Metis> metisCards;

    QHostAddress* metisAddress;
    QUdpSocket metisSocket;
    int metisPort;

    int clientPort;

    STATES state;

    void startMetis();
    void stopMetis();


    int sampleRate;
    int receivers;
    QString clock10MHz;
    QString clock122_88MHz;
    bool preamp;
    bool random;
    bool dither;
    bool duplex;
    bool classE;
    bool line_in;
    bool mic_boost;

    bool auto_start;
    bool auto_start_dsp;

    int alexAttenuation;
    int alexRxAntenna;
    int alexTxAntenna;
    int alexRxOnlyAntenna;

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

    bool mox;



    float mic_gain;

    int samples;

    unsigned long rx_frame;
    unsigned long tx_frame;

    void process_iq_buffer(unsigned char* buffer);

    int start_frames;

    Receiver* receiver[4];
    int current_receiver;

    long long loFrequency;

    int send_rx_frequency;
    int send_tx_frequency;

    int penny_change;

    Error error;

    float audio_gain;

    QSettings* settings;

    XVTR xvtrs[XVTRS];
};

#endif // SERVER_H
