#ifndef METIS_H
#define METIS_H

#include "QString"

class Metis
{
public:
    Metis(long ipaddr,unsigned char* macaddr);
    long getIpAddress();
    unsigned char* getMACAddress();
    QString toString();

private:
    long ipaddress;
    unsigned char macaddress[6];
};

#endif // METIS_H
