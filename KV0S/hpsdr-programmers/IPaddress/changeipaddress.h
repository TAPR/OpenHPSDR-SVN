#ifndef CHANGEIPADDRESS_H
#define CHANGEIPADDRESS_H

#include <QThread>
#include <QUdpSocket>
#include <QHostInfo>
#include <QStringList>
#include <QMessageBox>

#include "../Programmer/board.h"

class ChangeIPAddress : public QObject
{
    Q_OBJECT
public:
    ChangeIPAddress(QUdpSocket *s, unsigned char *MACaddress);
    ~ChangeIPAddress();


public slots:
    //void readyRead();
    void changeIP(QStringList *saddr );

private:
    QUdpSocket* socket;
    QString ip;
    unsigned char* macaddr;
    unsigned char buffer[1024];
    qint16 port;
};

#endif // CHANGEIPADDRESS_H
