/* 
 * File:   FreqSender.h
 * Author: Dave Larsen, KV0S
 *
 * Created on 14 August 2011
 */

/* Copyright (C)
* 2011 - Dave Larsen, KV0S - John Melton, G0ORX/N6LYT
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

#ifndef SENDER_H
#define SENDER_H

#include <QUdpSocket>

class QUdpSocket;

class FreqSender : QObject
{
    Q_OBJECT

public:
    FreqSender();

    void broadcastDatagram( QByteArray meassage );

private:
    QUdpSocket *udpSocket;

};

#endif // SENDER_H
