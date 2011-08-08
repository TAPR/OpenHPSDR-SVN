#include "about.h"
#include "mainwindow.h"

About::About(QWidget *parent) :
    QWidget(parent)
{
   version = version;
}

void About::aboutMessage()
{
    QString str;
    str = "<h2> QtLogger program </h2> Written by Dave Larsen, KV0S";
    str.append("<br>Copyright 2011 <br>Version ");
    str.append( version);
    str.append("<br>Design to work with the ghpsdr3 suite of programs");
    str.append("<br>For more information,<a href=\"http://openhpsdr.org/wiki/index.php?title=Ghpsdr3\">  http://http://openhpsdr.org/wiki/index.php?title=Ghpsdr3  </a>");
    QMessageBox::about(this, "About QtLogger", str );

}
