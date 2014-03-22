/**
* \file about.cpp
* \brief Code files for the About functions as part of the QtRadio program
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
#include "UI.h"

About::About(QWidget *parent)
{
}

void About::aboutMessage()
{
    QString str;
    str = "<h2> QtRadio program </h2> Written by John Melton, G0ORX/N6LYT";
    str.append("john.d.melton@googlemail.com");
    str.append("<br><br>Copyright 2012 <br>Version ");
    str.append( version);
    str.append("<br><br>Design to work with the ghpsdr3-Qt suite of programs");
    str.append("<br>The program is provided AS IS with NO WARRANTY OF ANY KIND, INCLUDING THE WARRANTY");
    str.append(" OF DESIGN, MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. ");
    str.append("<br><br>For more information,<a href=\"http://openhpsdr.org/wiki/index.php?title=Ghpsdr3\">  http://http://openhpsdr.org/wiki/index.php?title=Ghpsdr3  </a>");
    QMessageBox::about(this, "About QtRadio", str );

}

void About::setVersion( QString ver )
{
    version = ver;
}

