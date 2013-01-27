#ifndef DISCOVERY_H
#define DISCOVERY_H

#include <QThread>
#include <QUdpSocket>

#include "board.h"

class Discovery : public QObject {
    Q_OBJECT
public:
    Discovery(QUdpSocket* s,QString myip);
    void discover();
    void stop();
signals:
    void board_found(Board*);
    void reply(unsigned char);
public slots:
    void readyRead();
private:
    bool stopped;
    long ipAddress;
    int s;
    unsigned char buffer[2048];
    int bytes_read;

    QUdpSocket* socket;
    QString ip;
    qint16 port;
};

#endif // DISCOVERY_H

