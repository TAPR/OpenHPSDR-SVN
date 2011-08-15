/**
* \file about.h
* \brief Header files for the about functions as part of the Griffin ID program
* \author David R. Larsen , KV0S
* \version 1.0.0
* \date August 14, 2011
*/



/* Copyright (C) 2010 - David R. Larsen , KV0S
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


#ifndef ABOUT_H
#define ABOUT_H

#include <QWidget>
#include <QMessageBox>

/*!
 *  \brief the About class for the griffinID program
 *
 */

class About : public QWidget
{
    Q_OBJECT
public:
    explicit About(QWidget *parent = 0);
    QString version;
    void setVersion( QString vers );

signals:

public slots:
    void aboutMessage();

};

#endif // ABOUT_H
