#ifndef CONNECTION_H
#define CONNECTION_H

#include <QObject>
#include <QPair>
#include <QTcpSocket>
#include <QUrl>
#include <QMutex>
#include <QtXml/QDomDocument>
#include <QtXml/QDomNodeList>


class Connection : public QObject {
    Q_OBJECT
public:
    static Connection* getInstance();
    void setConnection(QString host,int port);
    int getSampleRate();
    QString getConfiguration();
protected:
    Connection();
signals:
    void sendNext();
    void setSampleRate(int); //sampleRate
    void disconnected();

public slots:
    void readyRead();
    void isConnected();
    void isDisconnected();
    void sendNextCommand();
    void sendCommand(QString);

private:
    static Connection* instance;

    QTcpSocket* socket;
    QString prefix;
    QUrl savedUrl;

    bool connected;

    QList <QString> waiting;
    QList <QString> pending;

    bool messageSent;

    int sampleRate;

    QMutex mutex;

    QString stringConfiguration;
    QDomDocument* configuration;
};

#endif // CONNECTION_H
