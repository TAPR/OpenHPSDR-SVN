/**
* \file about.cpp
* \brief Code files for the about functions as part of the Griffin ID program
* \author David R. Larsen, KV0S
* \version 1.0.0
* \date August 14, 2011
*/



/* Copyright (C) 2010 - David R. Larsen, KV0S
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

/**
 *  \brief the About class for the griffinID program
 *   constructor
 *
 */

About::About(QWidget *parent) :
    QWidget(parent)
{

}

/**
 *  \brief the aboutMessage function
 */

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

/**
 *  \brief  the setVersion function
 *
 *  \parameter QString vers
 */

void About::setVersion( QString vers )
{
    version = vers;
}
