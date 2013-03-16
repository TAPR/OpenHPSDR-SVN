#ifndef SERVER_H
#define SERVER_H

#include <QThread>
#include <QString>
#include <QList>
#include <QtNetwork/QUdpSocket>
#include <QtNetwork/QHostAddress>
#include <QSettings>

#include "interfaces.h"
#include "board.h"
#include "error.h"




class Server : QObject {
    Q_OBJECT
public:
    static Server* getInstance();
    void clearMetis();
    void addMetis(Board bd);

    QUdpSocket* getMetisSocket();
    void setMetisPort(int port);
    int getMetisPort();

    QString getInterfaceIPAddress(QString iface);
    QString getInterface();
    Interfaces* getInterfaces();

    void setDevice(QString d);
    void setInterface(QString i);
    void setMetisDetail(QString m);

    QString getDevice();
    QString getMetisDetail();
    QString getMetisText(int index);
    int getMetisCount();

    void clearError();
    void setError(QString e);

    QList<Board> boards;

protected:
    Server();

signals:
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



    QHostAddress* metisAddress;
    QUdpSocket metisSocket;
    int metisPort;

    int clientPort;

    Error error;



};

#endif // SERVER_H
