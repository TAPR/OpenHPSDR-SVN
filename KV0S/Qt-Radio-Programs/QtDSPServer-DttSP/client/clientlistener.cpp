#include "clientlistener.h"

ClientListener* ClientListener::instance = NULL;

ClientListener* ClientListener::getInstance() {
    if(instance==NULL) {
        instance=new ClientListener();
    }
    return instance;
}
ClientListener::ClientListener(QObject *parent) :
    QObject(parent)
{
}

void ClientListener::setup(int p,int rx) {
    qDebug()<<"ClientListener: listening on port:"<<p;
    port=p;
    receiver=rx;
    connect(&socket,SIGNAL(newConnection()),this,SLOT(newConnection()));
    socket.listen(QHostAddress::Any,port);
}

void ClientListener::newConnection() {
    QTcpSocket* clientSocket=socket.nextPendingConnection();
    Client* client=new Client(clientSocket,receiver,this);
    clients.append(client);
    connect(client,SIGNAL(disconnected(Client*)),this,SLOT(disconnected(Client*)));
}

void ClientListener::disconnected(Client* client) {
    clients.removeOne(client);
}

QList<Client*> ClientListener::getClients() {
    return clients;
}

void ClientListener::sendAudio(float* left,float* right,int length) {
    Client* client;
    for (int i = 0; i < clients.size(); ++i) {
        client=clients.at(i);
        client->sendAudio(left,right,length);
     }
}
