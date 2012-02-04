#include <QtGui/QApplication>
#include "mainwindow.h"

int main(int argc, char *argv[])
{
    QApplication app(argc, argv);

    QCoreApplication::setOrganizationName("g0orx");
    QCoreApplication::setOrganizationDomain("g0orx.org");
    QCoreApplication::setApplicationName("QtHPSDRServer");

    MainWindow w;
    w.show();
    
    return app.exec();
}
