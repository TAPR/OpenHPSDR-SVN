#include "Metis.h"

Metis::Metis(long ipaddr,unsigned char* macaddr) {
    ipaddress=ipaddr;
    for(int i=0;i<6;i++) {
        macaddress[i]=macaddr[i];
    }
}


long Metis::getIpAddress() {
    return ipaddress;
}

unsigned char* Metis::getMACAddress() {
    return macaddress;
}

QString Metis::toString() {
    QString text;
    text.sprintf("%02X:%02X:%02X:%02X:%02x:%02X (%d.%d.%d.%d)",
                 macaddress[0],macaddress[1],macaddress[2],macaddress[3],macaddress[4],macaddress[5],
                 ipaddress&0xFF,(ipaddress>>8)&0xFF,(ipaddress>>16)&0xFF,(ipaddress>>24)&0xFF);
    return text;
}
