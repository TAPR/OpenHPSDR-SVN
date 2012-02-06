#ifndef CLIENTLISTENER_H
#define CLIENTLISTENER_H

#include <QObject>
#include <QTcpServer>
#include <QTcpSocket>

#include "client.h"

class ClientListener : public QObject
{
    Q_OBJECT
public:
    static ClientListener* getInstance();
    void setup(int p,int rx);
    QList<Client*> getClients();
    void sendAudio(float* left,float* right, int length);

protected:
    ClientListener(QObject *parent = 0);

signals:

public slots:
    void newConnection();
    void disconnected(Client*);

private:
    static ClientListener* instance;

    QTcpServer socket;
    int port;
    int receiver;

    QList<Client*> clients;

};

#endif // CLIENT_H
