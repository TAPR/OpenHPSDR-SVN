/**
  @file
  @author Stefan Frings
*/

#ifndef STARTUP_H
#define STARTUP_H

#include <QtCore/QCoreApplication>

#include "client/clientlistener.h"
#include "hpsdr/connection.h"
#include "hpsdr/data.h"

/**
  Helper class to install and run the application as a windows
  service.
*/
class Startup : public QObject
{
public:

    /** Constructor */
    Startup(int argc, char *argv[]);

public slots:
    void setSampleRate(int);

protected:



private:
    int receiver;
    int clientPort;
    int serverPort;
    int localPort;
    QString host;
    ClientListener* clientListener;
    Connection* connection;
    Data* data;



};

#endif // STARTUP_H
