/*
 * File:   Interfaces.h
 * Author: John Melton, G0ORX/N6LYT
 *
 * Created on 23 November 2010
 */

/* Copyright (C)
* 2009 - John Melton, G0ORX/N6LYT
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

#ifndef INTERFACES_H
#define INTERFACES_H

#include <sys/types.h>
#include <sys/socket.h>
#include <sys/ioctl.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <net/if_arp.h>
#include <net/if.h>
#include <ifaddrs.h>

#include <pcap.h>

#include <QList>
#include <QtNetwork/QNetworkInterface>
#include <QtNetwork/QNetworkAddressEntry>

class Interfaces
{
public:
    Interfaces();
    int getInterfaces();
    QString getInterfaceNameAt(int index);
    QString getInterfaceHardwareAddress(int index);
    long getInterfaceIPAddress(int index);
private:
    int nInterfaces;
    struct ifconf ifconf;
    struct ifreq ifreqs[20];

    pcap_if_t* alldevs;
    pcap_if_t* dev;

    QList<QNetworkInterface> interfaces;
};

#endif // INTERFACES_H
