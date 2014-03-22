#include "clientlistener.h"

ClientListener::ClientListener(QObject *parent) :
    QObject(parent)
{
}

void ClientListener::configure(QSettings* mysettings) {
    settings=mysettings;
    setPort(settings->value("clientport",QString("10600")).toInt());

    connect(&socket,SIGNAL(newConnection()),this,SLOT(newConnection()));
    socket.listen(QHostAddress::Any,port);
}

void ClientListener::save() {
    settings->setValue("clientport",port);
}

void ClientListener::setPort(int p) {
    port=p;
}

int ClientListener::getPort() {
    return port;
}

void ClientListener::newConnection() {
    QTcpSocket* clientSocket=socket.nextPendingConnection();
    Client* client=new Client(clientSocket,this);
    connect(client,SIGNAL(clientDisconnected(Client*)),this,SLOT(disconnected(Client*)));
    emit clientConnected();
}

void ClientListener::disconnected(Client* client) {
    disconnect(client,SIGNAL(clientDisconnected(Client*)));
    emit clientDisconnected();
}
