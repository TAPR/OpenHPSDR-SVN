/*
 * File:   main.cpp
 * Author: john
 *
 * Created on 13 August 2010, 14:28
 */

#include <QtGui/QApplication>
#include "UI.h"

#include <stdio.h>
#include <stdlib.h>


int fOutputDisabled = 0;

void myMessageOutput(QtMsgType type, const char *msg)
{
    if (fOutputDisabled) return;
    switch (type) {
    case QtDebugMsg:
         fprintf(stderr, "Debug: %s\n", msg);
         break;
    case QtWarningMsg:
         fprintf(stderr, "Warning: %s\n", msg);
         break;
    case QtCriticalMsg:
         fprintf(stderr, "Critical: %s\n", msg);
         break;
    case QtFatalMsg:
         fprintf(stderr, "Fatal: %s\n", msg);
         abort();
    }
}

int main(int argc, char *argv[]) {

     if (getenv("QT_RADIO_NO_DEBUG")) fOutputDisabled = 1;
     qInstallMsgHandler(myMessageOutput);

    // initialize resources, if needed
    // Q_INIT_RESOURCE(resfile);

    QApplication app(argc, argv);

    // create and show your widgets here
    UI widget;

    widget.show();

    return app.exec();
}
