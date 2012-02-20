/**
  @file
  @author Stefan Frings and modified by John Melton
*/

#include "static.h"
#include "startup.h"
#include "dttsp.h"
#include "hpsdr/connection.h"

#include <QDir>
#include <QObject>

/** Name of this application */
#define APPNAME "QtDSPServer"

/** Publisher of this application */
#define ORGANISATION "HPSDR"

/** Short description of this application */
#define DESCRIPTION "Qt DSP Server"


Startup::Startup(int argv, char* argc[]) {

    //QString configFileName=Static::getConfigDir()+"/"+APPNAME+".ini";

    // defaults
    receiver=0;

    host="127.0.0.1";
    serverPort=10600;
    localPort=10700;
    clientPort=10800;

    QRegExp rxArg("--receiver=([0-3])");
    QRegExp clientArg("--clientport=(\\d{1,6}$)");
    QRegExp serverArg("--serverport=(\\d{1,6}$)");
    QRegExp hostArg("--server=(\\S+$)");

    for (int i = 1; i < argv; ++i) {
        if (rxArg.indexIn(argc[i]) != -1 ) {
            receiver=rxArg.cap(1).toInt();
        } else if (clientArg.indexIn(argc[i]) != -1 ) {
            clientPort=clientArg.cap(1).toInt();
        } else if (serverArg.indexIn(argc[i]) != -1 ) {
            serverPort=serverArg.cap(1).toInt();
        } else if (hostArg.indexIn(argc[i]) != -1 ) {
            host=hostArg.cap(1);
        }
    }
    qDebug()<<"receiver:"<<receiver;
    qDebug()<<"clientPort:"<<clientPort;
    qDebug()<<"serverPort:"<<serverPort;
    qDebug()<<"host:"<<host;

    // startup the client listener
    connect(Connection::getInstance(),SIGNAL(setSampleRate(int)),this,SLOT(setSampleRate(int)));
    Connection::getInstance()->setConnection(host,serverPort);
    Data::getInstance();
    ClientListener::getInstance()->setup(clientPort,receiver);



    // Configure DttSP
    Setup_SDR();
    Release_Update();
    SetTRX(0,RX); // thread 0 is for receive
    SetTRX(1,TX);  // thread 1 is for transmit
    SetRingBufferOffset(0,0); // 0 for HPSDR
    SetThreadProcessingMode(0,RUN_PLAY);
    SetThreadProcessingMode(1,RUN_PLAY);
    SetSubRXSt(0,0,1);
    SetRXOutputGain(0,0,0.20);
    SetSampleRate(96000.0);
    reset_for_buflen(0,1024);
    reset_for_buflen(1,1024);

    SetRXOsc(0,0,0.0);
    SetRXOsc(0,1,0.0);
    SetTXOsc(1,0.0);


    QString command;
    command.append(QString("attach "));
    command.append(QString::number(receiver));
    Connection::getInstance()->sendCommand(command);

    command.clear();
    command.append(QString("frequency "));
    command.append(QString::number(receiver));
    command.append(QString(" 7056000"));
    Connection::getInstance()->sendCommand(command);

    // startup to receive the I, Q and microphone samples
    Data::getInstance()->setConnection(host,serverPort+receiver,localPort+receiver);

    // start the IQ and microphone samples
    command.clear();
    command.append(QString("start "));
    command.append(QString::number(receiver));
    command.append(QString(" iq "));
    command.append(QString(host));
    command.append(QString(" "));
    command.append(QString::number(localPort+receiver));
    Connection::getInstance()->sendCommand(command);

}


void Startup::setSampleRate(int rate) {
    SetSampleRate(96000.0);
    reset_for_buflen(0,1024);
    reset_for_buflen(1,1024);
}
