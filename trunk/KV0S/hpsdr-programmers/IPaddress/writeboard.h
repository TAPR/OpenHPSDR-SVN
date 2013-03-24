#ifndef CHANGEIPADDRESS_H
#define CHANGEIPADDRESS_H

#include <QThread>
#include <QUdpSocket>
#include <QHostInfo>
#include <QStringList>
#include <QMessageBox>

#include "../Programmer/board.h"

class writeBoard : public QObject
{
    Q_OBJECT
public:
    writeBoard(QUdpSocket *s, unsigned char *MACaddress);
    ~writeBoard();


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
