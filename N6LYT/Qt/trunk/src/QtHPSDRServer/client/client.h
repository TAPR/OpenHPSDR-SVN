#ifndef CLIENT_H
#define CLIENT_H

#include <QObject>
#include <QtNetwork/QTcpSocket>


class Client : public QObject
{
    Q_OBJECT
public:
    explicit Client(QTcpSocket* s,QObject *parent = 0);
    void send();
    QHostAddress getPeerAddress();

signals:
    void clientDisconnected(Client*);

public slots:
    void readyRead();
    void disconnected();

private:
    QTcpSocket* socket;

    int band;
    long long frequency;
};

#endif // CLIENT_H
