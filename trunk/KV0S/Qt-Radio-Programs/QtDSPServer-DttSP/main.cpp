#include "startup.h"

#include <QtCore/QCoreApplication>

/**
  Entry point of the program.
*/
int main(int argc, char *argv[]) {
    QCoreApplication application(argc,argv);

    Startup* startup=new Startup(argc,argv);

    application.exec();
}
