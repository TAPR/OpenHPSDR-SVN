/*
 * File:   Interfaces.cpp
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

#include <QDebug>
#include <QString>

#include "Interfaces.h"

// get a list of the interfaces on the system
Interfaces::Interfaces()
{
    char errbuf[PCAP_ERRBUF_SIZE];
    pcap_addr_t* addr;
    struct sockaddr* sa;
    struct sockaddr_in* sa_in;

    interfaces=0;
    if (pcap_findalldevs(&alldevs, errbuf) == -1) {
        qDebug()<<"Error in pcap_findalldevs:" << errbuf;
    } else {
        /* Print the list */
        for(dev=alldevs; dev; dev=dev->next) {
            interfaces++;
            qDebug() << dev->name;
            for(addr=dev->addresses; addr; addr=addr->next) {
                sa=addr->addr;
                if(sa->sa_family==AF_INET) {
                    sa_in=(sockaddr_in*)sa;
                    qDebug() << "AF_INET" << inet_ntoa(sa_in->sin_addr);
                }
            }
        }
    }

    qDebug() << "Interfaces found " << interfaces;
}

int Interfaces::getInterfaces() {
    return interfaces;
}

QString Interfaces::getInterfaceNameAt(int index) {
    char *name;
    int i;
    for(dev=alldevs,i=0; dev; dev=dev->next,i++) {
        if(i==index) {
            name=dev->name;
            break;
        }
    }
    return QString(name);
}

unsigned char* Interfaces::getInterfaceHardwareAddress(QString ifname) {
    static unsigned char* u;
    static struct ifreq iface;
    int rc;
    int sock=socket(AF_INET,SOCK_DGRAM,0);

    iface.ifr_addr.sa_family=AF_INET;
    strncpy(iface.ifr_name,ifname.toAscii(),sizeof(iface.ifr_name));
    if((rc=ioctl(sock,SIOCGIFHWADDR,&iface))<0) {
        u=NULL;
        qDebug()<<"No interface found for"<<ifname<<"rc="<<rc;
    } else {
        u=(unsigned char*)&(iface.ifr_addr.sa_data);
    }
    close(sock);
    return u;
}

long Interfaces::getInterfaceIPAddress(QString ifname) {
    long ipAddress=0;
    int rc;
    int sock=socket(AF_INET,SOCK_DGRAM,0);
    struct ifreq iface;
    iface.ifr_addr.sa_family=AF_INET;
    strncpy(iface.ifr_name,ifname.toAscii(),sizeof(iface.ifr_name));
    if((rc=ioctl(sock,SIOCGIFADDR,&iface))<0) {
        qDebug()<<"No interface found for"<<ifname<<"rc="<<rc;
    } else {
        struct sockaddr_in *sin = (struct sockaddr_in *)&iface.ifr_addr;
        ipAddress=sin->sin_addr.s_addr;
    }
    close(sock);
    return ipAddress;
}

