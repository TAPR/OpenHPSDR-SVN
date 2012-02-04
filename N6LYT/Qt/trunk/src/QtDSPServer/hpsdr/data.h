#ifndef DATA_H
#define DATA_H

#include <QObject>
#include <QUdpSocket>
#include <QMutex>

class Data : public QObject {
    Q_OBJECT

public:
    static Data* getInstance();
    void setConnection(QString h,int r,int l);
    void send();
    void setMox(bool state);
    bool getMox();
    void addMicSample(float sample);
    void setMicRemote(bool state);

protected:
    Data();

signals:

public slots:
    void readPendingDatagrams();

private:
    static Data* instance;

    QMutex mutex;
    QMutex micMutex;

    QString host;
    int remote_port;
    int local_port;
    QUdpSocket* udpSocket;

    bool mox;

    quint64 sequence;
    int offset;
    quint64 tx_sequence;
    float input_buffer[1024*3]; // I, Q, Mic
    float output_buffer[1024*4]; // left audio, right audio, Tx I, Tx Q

    bool mic_remote;

    float mic_buffer[1024];
    int mic_buffer_offset;

};

#endif // DATA_H
