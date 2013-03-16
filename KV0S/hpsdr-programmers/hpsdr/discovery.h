#ifndef DISCOVERY_H
#define DISCOVERY_H

#include <QtNetwork/QUdpSocket>

#include "server.h"
#include "board.h"

class Discovery :public QObject {
    Q_OBJECT

public:
    Discovery();

public slots:
    void readyRead();
    void discon();

private:
    Server *server;
};

#endif // DISCOVERY_H
