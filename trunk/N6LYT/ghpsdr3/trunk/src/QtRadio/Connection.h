/* 
 * File:   Connection.h
 * Author: john
 *
 * Created on 16 August 2010, 07:40
 */

#ifndef CONNECTION_H
#define	CONNECTION_H

#include <QObject>
#include <QDebug>
#include <QTcpSocket>
#include <QSemaphore>

#define DSPSERVER_BASE_PORT 8000

#define HEADER_SIZE 48

class Connection : public QObject {
    Q_OBJECT
public:
    Connection();
    Connection(const Connection& orig);
    virtual ~Connection();
    void connect(QString host,int receiver);
    void disconnect();
    void sendCommand(QString command);
    void read(char* buffer,int length);
public slots:
    void connected();
    void socketError(QAbstractSocket::SocketError socketError);
    void socketData();
signals:
    void isConnected();
    void disconnected(QString message);
    void header(char* header);
private:
    QTcpSocket* tcpSocket;
    QSemaphore sem;
};

#endif	/* CONNECTION_H */

