#include "about.h"
#include "mainwindow.h"

About::About(QWidget *parent) :
    QWidget(parent)
{

}

void About::aboutMessage()
{
    QString str;
    str = "<h2> GriffinID program </h2> Written by Dave Larsen, KV0S";
    str.append("<br>Copyright 2011 <br>Version ");
    str.append( version);
    str.append("<br>Design to work with the penny(lane) WSPR/QRSS beacon");
    str.append("<br>For more information,<a href=\"http://openhpsdr.org/wiki/\">  http://http://openhpsdr.org/wiki/  </a>");
    QMessageBox::about(this, "About griffinID", str );

}

void About::setVersion( QString vers )
{
    version = vers;
}
