/**
* \file main.cpp
* \brief Code files for the main function as part of the Griffin ID program
* \author David R. Larsen, KV0S
* \version 1.0.1
* \date August 14, 2011
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


#include <QtGui/QApplication>
#include "mainwindow.h"

/**
 *  \brief the About class for the griffinID program
 *
 */

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);

    QCoreApplication::setOrganizationName("openhpsdr" );
    QCoreApplication::setOrganizationDomain("openhpsdr.org" );
    QCoreApplication::setApplicationName("griffinID");

    MainWindow w;
    w.show();

    return a.exec();
}
