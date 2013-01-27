/*
 * File:   discovery.h
 * Author: John Melton, G0ORX/N6LYT
 *
 * Created on 23 November 2010
 *
 * Created on December 30, 2012
 * Author: Dave Larsen, KV0S
 */

/* Copyright (C)
* 2009 - John Melton, G0ORX/N6LYT
* 2012 - Dave Larsen, KV0S
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


#ifndef DISCOVERY_H
#define DISCOVERY_H

#include <QThread>
#include <QUdpSocket>

#include "board.h"

class Discovery : public QObject {
    Q_OBJECT
public:
    Discovery(QUdpSocket* s,QString myip);
    void discover();
    void stop();
signals:
    void board_found(Board*);
    void reply(unsigned char);
public slots:
    void readyRead();
private:
    bool stopped;
    long ipAddress;
    int s;
    unsigned char buffer[2048];
    int bytes_read;

    QUdpSocket* socket;
    QString ip;
    qint16 port;
};

#endif // DISCOVERY_H

