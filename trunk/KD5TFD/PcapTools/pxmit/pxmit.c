/*
 *
 * Copyright (C) 2009 Bill Tracey, KD5TFD  (bill@ewjt.com) 
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */


#define _CRT_SECURE_NO_WARNINGS  (1) /* I'm an old C programmer, I can handle the danger! */ 

#include <stdio.h>

#define HAVE_REMOTE (1)

#include <pcap.h>

#include <winsock.h>
// #include <ws2def.h>
#include <winsock2.h>
#include <iphlpapi.h>
#pragma comment(lib, "IPHLPAPI.lib")

#if 0
#include <mstcpip.h>
#endif



// Function prototypes
void ifprint(pcap_if_t *d);
char *iptos(u_long in);
char* ip6tos(struct sockaddr *sockaddr, char *address, int addrlen);



/* Print all the available information on the given interface */
void ifprint(pcap_if_t *d)
{
  pcap_addr_t *a;
  char ip6str[128];

  /* Name */
  printf("%s\n",d->name);

  /* Description */
  if (d->description)
    printf("\tDescription: %s\n",d->description);

  /* Loopback Address*/
  printf("\tLoopback: %s\n",(d->flags & PCAP_IF_LOOPBACK)?"yes":"no");

  /* IP addresses */
  for(a=d->addresses;a;a=a->next) {
    printf("\tAddress Family: #%d\n",a->addr->sa_family);

    switch(a->addr->sa_family)
    {
      case AF_INET:
        printf("\tAddress Family Name: AF_INET\n");
        if (a->addr)
          printf("\tAddress: %s\n",iptos(((struct sockaddr_in *)a->addr)->sin_addr.s_addr));
        if (a->netmask)
          printf("\tNetmask: %s\n",iptos(((struct sockaddr_in *)a->netmask)->sin_addr.s_addr));
        if (a->broadaddr)
          printf("\tBroadcast Address: %s\n",iptos(((struct sockaddr_in *)a->broadaddr)->sin_addr.s_addr));
        if (a->dstaddr)
          printf("\tDestination Address: %s\n",iptos(((struct sockaddr_in *)a->dstaddr)->sin_addr.s_addr));
        break;

      case AF_INET6:
        printf("\tAddress Family Name: AF_INET6\n");
        if (a->addr)
          printf("\tAddress: %s\n", ip6tos(a->addr, ip6str, sizeof(ip6str)));
       break;

      default:
        printf("\tAddress Family Name: Unknown\n");
        break;
    }
  }
  printf("\n");
}



/* From tcptraceroute, convert a numeric IP address to a string */
#define IPTOSBUFFERS    12
char *iptos(u_long in)
{
    static char output[IPTOSBUFFERS][3*4+3+1];
    static short which;
    u_char *p;

    p = (u_char *)&in;
    which = (which + 1 == IPTOSBUFFERS ? 0 : which + 1);
    sprintf(output[which], "%d.%d.%d.%d", p[0], p[1], p[2], p[3]);
    return output[which];
}

char* ip6tos(struct sockaddr *sockaddr, char *address, int addrlen)
{
    socklen_t sockaddrlen;

    #ifdef WIN32
    sockaddrlen = sizeof(struct sockaddr_in6);
    #else
    sockaddrlen = sizeof(struct sockaddr_storage);
    #endif


    if(getnameinfo(sockaddr,
        sockaddrlen,
        address,
        addrlen,
        NULL,
        0,
        NI_NUMERICHOST) != 0) address = NULL;

    return address;
}

#define GAA_FLAGS ( GAA_FLAG_SKIP_ANYCAST |   GAA_FLAG_SKIP_DNS_SERVER |  GAA_FLAG_SKIP_FRIENDLY_NAME |   GAA_FLAG_SKIP_MULTICAST )


/*
 * resultbufp assumed to point to a PCAP_BUF_SIZE sized buffer
 * returns 0 on succes, !0 else
 */
int pcapNameFromIPv4(IN_ADDR *in_addrp, char *resultbufp) {

    pcap_if_t *alldevs;
    pcap_if_t *d;
    int i=0;
    char errbuf[PCAP_ERRBUF_SIZE];



    /* Retrieve the device list from the local machine */
        if (pcap_findalldevs_ex(PCAP_SRC_IF_STRING, NULL /* auth is not needed */, &alldevs, errbuf) == -1) {
                return 1;
    }

    /* loop thru devices  looking for ours */
    for(d= alldevs; d != NULL; d= d->next)
    {
                pcap_addr_t *a;
                /* loop thru addrs on the device */
                for  ( a = d->addresses; a != NULL; a = a->next ) {
                        if ( a->addr->sa_family == AF_INET ) {
                                if ( memcmp(&(((struct sockaddr_in *)(a->addr))->sin_addr), in_addrp, sizeof(IN_ADDR)) == 0 ) {  /* found it */
                                        strncpy(resultbufp, d->name, PCAP_BUF_SIZE);
                                        return 0;
                                }
                        }
                }
    }
        return -1;
}



int dumpIPandMACaddrs(void) {
        unsigned char addrbuf[8192];
        int addrbufsize = sizeof(addrbuf);
        int rc;

        IP_ADAPTER_ADDRESSES *addrp = (IP_ADAPTER_ADDRESSES *)addrbuf;

        rc =  GetAdaptersAddresses(AF_INET, GAA_FLAGS, NULL, addrp,  &addrbufsize);
        if ( rc == ERROR_BUFFER_OVERFLOW ) {
                /* not enough space... alloc buffer needed and retry */
                addrp = malloc(addrbufsize);
                if ( addrp == NULL ) {
                        return -1;
                }
                rc =  GetAdaptersAddresses(AF_INET, GAA_FLAGS, NULL, addrp,  &addrbufsize);
        }

        if ( rc == NO_ERROR ) {
                int j;
                IP_ADAPTER_ADDRESSES *adapterp = addrp;
                while ( adapterp != NULL ) {
                        IP_ADAPTER_UNICAST_ADDRESS *ipaddrp = adapterp->FirstUnicastAddress;
                        printf("name: %s\n", adapterp->AdapterName);
                        while ( ipaddrp != NULL ) {
                                int i;
                                SOCKADDR *sap = (ipaddrp->Address.lpSockaddr);
                                if ( sap->sa_family == AF_INET ) {
                                        SOCKADDR_IN *sainp = (SOCKADDR_IN *)sap;
                                        printf("ip addr: %s\n", inet_ntoa(sainp->sin_addr)); /* return frmo inet_ntoa does not need to be freed */
                                }
                                else {
                                        printf("family: %d addr_data:", sap->sa_family);
                                        for ( i = 0; i < 14; i++ ) {
                                                printf(" %u", (unsigned char)(sap->sa_data[i]));
                                        }
                                        printf("\n");
                                }
                                ipaddrp = ipaddrp->Next;
                        }
                        printf("Phys addr: ");
                        for (  j = 0; j < (int)adapterp->PhysicalAddressLength; j++ ) {
                                if ( j != 0 ) {
                                        printf(":");
                                }
                                printf("%02x", (unsigned char)(adapterp->PhysicalAddress[j]));
                        }
                        printf("\n");
                        adapterp = adapterp->Next;
                }
        }
        else {
                printf("GetAdapterAddresses failed with rc=%d\n", rc);
        }
        if ( (void *)addrp != (void *)addrbuf ) {
                free(addrp);
        }
        return 0;
}


/*
 * returns 0 if an adapter with IP addr in in_addrp can be found, !0 otherwise
 * macbufp MUST point to a 6 byte buffer
 */
int getMacAddrfromIPv4(IN_ADDR *in_addrp, BYTE *macbufp) {
        unsigned char addrbuf[8192];
        int addrbufsize = sizeof(addrbuf);
        int rc;

        IP_ADAPTER_ADDRESSES *addrp = (IP_ADAPTER_ADDRESSES *)addrbuf;

        rc =  GetAdaptersAddresses(AF_INET, GAA_FLAGS, NULL, addrp,  &addrbufsize);
        if ( rc == ERROR_BUFFER_OVERFLOW ) {
                /* not enough space... alloc buffer needed and retry */
                addrp = malloc(addrbufsize);
                if ( addrp == NULL ) {
                        return -1;
                }
                rc =  GetAdaptersAddresses(AF_INET, GAA_FLAGS, NULL, addrp,  &addrbufsize);
        }

        if ( rc == NO_ERROR ) {
                IP_ADAPTER_ADDRESSES *adapterp = addrp;
                while ( adapterp != NULL ) {
                        IP_ADAPTER_UNICAST_ADDRESS *ipaddrp = adapterp->FirstUnicastAddress;
                        /* printf("name: %s\n", adapterp->AdapterName);  */
                        while ( ipaddrp != NULL ) {
                                SOCKADDR *sap = (ipaddrp->Address.lpSockaddr);
                                if ( sap->sa_family == AF_INET ) {
                                        SOCKADDR_IN *sainp = (SOCKADDR_IN *)sap;
                                        if ( memcmp(&(sainp->sin_addr), in_addrp, sizeof(IN_ADDR)) == 0 ) {
                                                memcpy(macbufp, adapterp->PhysicalAddress, min(6, adapterp->PhysicalAddressLength));
                                                if ( (void *)addrp != (void *)addrbuf ) {
                                                        free(addrp);
                                                }
                                                return 0;
                                        }
                                        /* printf("ip addr: %s\n", inet_ntoa(sainp->sin_addr));  */ /* return frmo inet_ntoa does not need to be freed */
                                }
                                ipaddrp = ipaddrp->Next;
                        }
                        adapterp = adapterp->Next;
                }
        }

        if ( (void *)addrp != (void *)addrbuf ) {
                free(addrp);
        }
        return 1;
}


#define PACKET_LEN (1500)


void sendBurst(int packet_count, pcap_t *fp, byte *srcmac, byte *targetmac) { 
	char odd_packet[PACKET_LEN]; 
	char even_packet[PACKET_LEN]; 
	char *packetp; 
	int rc; 
	int j; 
	int i; 
	

	printf("bursting %d packets\n", packet_count); 
	for ( i = 0; i < 6; i++ ) { 
		odd_packet[i] = *(targetmac + i);
	} 
	for ( i = 0 ; i < 6; i++ ) { 
		odd_packet[6+i] = *(srcmac + i); 
	} 
	odd_packet[12] = 0xef;
	odd_packet[13] = 0xfe;
	memcpy(even_packet, odd_packet, 14); 
	
	for ( i = 18; i < PACKET_LEN; i++ ) { 
		even_packet[i] = 2 * ( i - 18 );
		odd_packet[i] = 1 + even_packet[i]; 
	} 

	for ( j = 0; j < packet_count; j++ ) { 
		if ( ( j & 1 ) != 0 ) { 
			packetp = odd_packet; 
		} 
		else { 
			packetp = even_packet; 
		} 
		memcpy(packetp + 14, &j, 4); /* insert packet num */ 
		rc = pcap_sendpacket(fp, packetp, PACKET_LEN); 
		if ( rc != 0 ) { 
            printf("i=%d, pcap_sendpacket failed: %s\n", i, pcap_geterr(fp));
			return;
		}
	} 
	return;
} 




int main(int argc, char *argv[]) {
    pcap_if_t *alldevs;
    pcap_if_t *d;
    int i=0;
    char errbuf[PCAP_ERRBUF_SIZE];
    char namebuf[PCAP_BUF_SIZE];
    BYTE macbuf[6];
	BYTE target_mac[6]; 
    IN_ADDR in_addr;
    pcap_t *fp;

    

    /* Retrieve the device list from the local machine */
    if (pcap_findalldevs_ex(PCAP_SRC_IF_STRING, NULL /* auth is not needed */, &alldevs, errbuf) == -1)
    {
        fprintf(stderr,"Error in pcap_findalldevs_ex: %s\n", errbuf);
        exit(1);
    }

    /* Print the list */
    for(d= alldevs; d != NULL; d= d->next)
    {
        printf("%d. %s", ++i, d->name);
                if (d->description)  printf(" (%s)\n", d->description);
        else printf(" (No description available)\n");
                ifprint(d);
    }

    if (i == 0)
    {
        printf("\nNo interfaces found! Make sure WinPcap is installed.\n");
        return;
    }

    /* We don't need any more the device list. Free it */
    pcap_freealldevs(alldevs);



        dumpIPandMACaddrs();
#if 0
        lrc = RtlIpv4StringToAddressA("10.1.1.15", 0, NULL, &in_addr);
        if ( lrc == NO_ERROR ) {
                if (  getMacAddrfromIPv4(&in_addr, macbuf) == 0 ) {
                        int j;
                        printf("mac addr: ");
                        for ( j = 0; j < sizeof(macbuf); j++  ) {
                                if ( j != 0 ) {
                                        printf(":");
                                }
                                printf("%02x", (unsigned char)macbuf[j]);
                        }
                        printf("\n");
                }
                else {
                        printf("getMacAddrfromIPv4 failed.\n");
                }
        }
        else {
                printf("RtlIpv4StringToAddress failed\n");
        }
#else
        if ( inet_pton(AF_INET, "10.1.1.15", &in_addr) == 1 ) {
                if (  getMacAddrfromIPv4(&in_addr, macbuf) == 0 ) {
                        int j;
                        printf("mac addr: ");
                        for ( j = 0; j < sizeof(macbuf); j++  ) {
                                if ( j != 0 ) {
                                        printf(":");
                                }
                                printf("%02x", (unsigned char)macbuf[j]);
                        }
                        printf("\n");
                }
                else {
                        printf("getMacAddrfromIPv4 failed.\n");
                }

                if ( pcapNameFromIPv4(&in_addr, namebuf) == 0 ) {
                        printf("pcap name: >%s<\n", namebuf);
                }
                else {
                        printf("pcapNameFromIPv4 failed\n");
                        exit(99);
                }
        }
        else {
                printf("inet_pton failed\n");
                exit(98);
        }

        /* if we get here namebuf has the name of the pcapbuf we want to play with */
        fp = pcap_open(namebuf,
                           2048, /* snaplen */
                                   0,  /* flags */
                                   1000, /* read timeout */
                                   NULL, /* authentication */
                                   errbuf);
        if ( fp == NULL ) {
                printf("pcap_open of \'%s\' failed.\n", namebuf);
                exit(97);
        }

		target_mac[0] = 0x00; 
		target_mac[1] = 0x1a; 
		target_mac[2] = 0x6b; 
		target_mac[3] = 0xce; 
		target_mac[4] = 0x34; 
		target_mac[5] = 0xeb; 

		sendBurst(100, fp, macbuf, target_mac); 

        pcap_close(fp);


#endif
        /* getMacAddrfromIPv4(0);  */
}



