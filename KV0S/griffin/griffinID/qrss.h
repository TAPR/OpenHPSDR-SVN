/**
* \file qrss.h
* \brief Header files for the QRSS functions as part of the Griffin ID program
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


#ifndef QRSS_H
#define QRSS_H

#include <QString>
#include <QHash>
#include <QDebug>


/**
 *  \brief the QRSS class for the griffinID program
 *
 *  These functions encode the text string to transmit the QRSS beacon.  This is used to encode your
 *  output beacon using the openhpsdr.org : griffin board
 *
 *  QRSS is vaey slow speed, low bandwidth transmissions.
 *  Thanks to Dave WW2R for his help on the QRSS information.
 *
 */

class QRSS
{
public:
    QRSS();
    QString morsechar( QChar ch );
    QString encodeCW( QString message );
    QString displayCW( QString message );

private:
    QHash <QChar, QString> morse;

private slots:
    QString message;

};

#endif // QRSS_H
