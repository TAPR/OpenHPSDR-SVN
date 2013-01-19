#ifndef CHANGEIPADDRESS_H
#define CHANGEIPADDRESS_H

#include <QThread>
#include <QUdpSocket>

#include "board.h"

class changeIPAddress : public QObject
{
    Q_OBJECT
public:
    changeIPAddress(QUdpSocket* s);


public slots:
    void readyread();
    void changeIP();

private:
    QUdpSocket* socket;
    QString ip;
    qint16 port;
};

#endif // CHANGEIPADDRESS_H
