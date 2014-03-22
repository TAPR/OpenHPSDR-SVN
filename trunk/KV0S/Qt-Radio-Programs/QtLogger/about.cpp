/**
* \file about.cpp
* \brief Code files for the About functions as part of the QtLogger program
* \author David R. Larsen, KV0S
* \version 1.0.2
* \date August 21, 2011
*/



/* Copyright (C) 2011 - David R. Larsen, KV0S
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*
*/



#include "about.h"
#include "mainwindow.h"

About::About(QWidget *parent) :
    QWidget(parent)
{
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

void About::setVersion( QString ver )
{
    version = ver;
}
