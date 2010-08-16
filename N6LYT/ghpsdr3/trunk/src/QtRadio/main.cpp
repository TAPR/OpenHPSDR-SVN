/*
 * File:   main.cpp
 * Author: john
 *
 * Created on 13 August 2010, 14:28
 */

#include <QtGui/QApplication>

#include "UI.h"

int main(int argc, char *argv[]) {
    // initialize resources, if needed
    // Q_INIT_RESOURCE(resfile);

    QApplication app(argc, argv);

    // create and show your widgets here
    UI widget;

    widget.show();

    return app.exec();
}
