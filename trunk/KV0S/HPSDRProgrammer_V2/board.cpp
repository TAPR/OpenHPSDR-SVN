#include "board.h"

Board::Board(long ipaddr,unsigned char* macaddr,unsigned char software_version,unsigned char board_type) {
    ipaddress=ipaddr;
    for(int i=0;i<6;i++) {
        macaddress[i]=macaddr[i];
    }
    version=software_version;
    board=board_type;
}


long Board::getIpAddress() {
    return ipaddress;
}

unsigned char* Board::getMACAddress() {
    return macaddress;
}

QString Board::getHostAddress() {
    QString address;
    address.sprintf("%ld.%ld.%ld.%ld",
                 (ipaddress>>24)&0xFF,(ipaddress>>16)&0xFF,(ipaddress>>8)&0xFF,ipaddress&0xFF);
    return address;
}

QString Board::toString() {
    QString text;
    text.sprintf("%02X:%02X:%02X:%02X:%02x:%02X (%ld.%ld.%ld.%ld) Software version: %d.%d (%s)",
                 macaddress[0],macaddress[1],macaddress[2],macaddress[3],macaddress[4],macaddress[5],
                 (ipaddress>>24)&0xFF,(ipaddress>>16)&0xFF,(ipaddress>>8)&0xFF,ipaddress&0xFF,
                 version/10,version%10,board==0?"Metis":"Hermes");
    return text;
}

unsigned char Board::getVersion() {
    return version;
}

unsigned char Board::getBoard() {
    return board;
}
