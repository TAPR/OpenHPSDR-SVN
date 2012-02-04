#ifndef CLIENTLISTENER_H
#define CLIENTLISTENER_H

#include <QObject>
#include <QtNetwork/QTcpServer>
#include <QtNetwork/QTcpSocket>
#include <QSettings>

#include "client.h"

class ClientListener : public QObject
{
    Q_OBJECT
public:
    explicit ClientListener(QObject *parent = 0);
    void setPort(int p);
    int getPort();
    void configure(QSettings* mysettings);
    void save();

signals:
    void clientConnected();
    void clientDisconnected();

public slots:
    void newConnection();
    void disconnected(Client*);

private:
    QTcpServer socket;
    int port;
    QSettings* settings;

};

#endif // CLIENT_H
