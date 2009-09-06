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
#include <string.h> 
#define HAVE_REMOTE (1) 

#include <pcap.h>

#include <winsock.h>
// #include <ws2def.h> 
#include <winsock2.h>
#include <iphlpapi.h> 
#pragma comment(lib, "IPHLPAPI.lib")
#include <windows.h>

#if 0 
#include <mstcpip.h>
#endif 


char *wjt_Copyright = "Copyright (C) 2009 Bill Tracey, KD5TFD (bill@ewjt.com).\nThis progam is licensed under the GUN General Public License Version 2.\n";


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


#define PACKET_LEN (512) 
BYTE macbuf[6] = { 0xfa, 0xaf, 0xb0, 0x0b, 0xd0, 0x0d };

char *HelpMsg = "\nusage: beacon [n]\n\nBeacons a packet 1 time/sec. [n] indicates interface to beacon on, omit n to get a list of interfaces.\n\n"; 

void doHelp() { 
	printf(HelpMsg); 
	printf(wjt_Copyright); 
} 

int main(int argc, char *argv[]) {
    pcap_if_t *alldevs;
    pcap_if_t *d;
    int i=0;
    char errbuf[PCAP_ERRBUF_SIZE];
	int devidx;
	char inbuf[200]; 
	char namebuf[PCAP_BUF_SIZE]; 
	char descbuf[PCAP_BUF_SIZE]; 


	pcap_t *fp; 
	int rc;
	int packet_num = 0; 
	int count_is_odd; 
	unsigned char packet[PACKET_LEN]; 
	
	if ( argc >= 2 ) { 
		char *lcasearg;
		lcasearg = _strdup(argv[1]); 
		if ( lcasearg != NULL ) { 
			_strlwr(lcasearg); 
			if (   strstr(lcasearg, "?") != 0 ||  strstr(lcasearg, "help") != 0 ) { 
				doHelp(); 
				exit(99); 
			}
			free(lcasearg); 
		} 
	} 
  

    /* Retrieve the device list from the local machine */
    if (pcap_findalldevs_ex(PCAP_SRC_IF_STRING, NULL /* auth is not needed */, &alldevs, errbuf) == -1)
    {
        fprintf(stderr,"Error in pcap_findalldevs_ex: %s\n", errbuf);
        exit(1);
    }
    
    /* Print the list */
    for(d= alldevs; d != NULL; d= d->next)
    {
		++i;
		if ( argc <= 1 ) {  /* no need to list interfaces if we have idx to use on command line */ 
			printf("%d. %s", i, d->name);
			if (d->description)  printf(" (%s)\n", d->description);
			else printf(" (No description available)\n");
		}
		/* ifprint(d);  */ 
    }
    
    if (i == 0)
    {
        printf("\nNo interfaces found! Make sure WinPcap is installed.\n");
        return;
    }
	
	if ( argc >= 2 ) {  /* devidx given on command line */ 
			strncpy(inbuf, argv[1], sizeof(inbuf));
			devidx = atoi(inbuf); 
	}
	else if ( i != 1 ) { 	
		printf("Enter number of interface to beacon on: "); fflush(stdout); 
		gets(inbuf); 
		devidx = atoi(inbuf); 
	} 
	else { 
		devidx = 1;
	}
	i = 0; 
	namebuf[0] = 0; 
	for ( d = alldevs;  d != NULL; d = d->next ) { 
		++i; 
		if ( devidx == i ) { 
			strncpy(namebuf, d->name, sizeof(namebuf)); 
			strncpy(descbuf, d->description, sizeof(descbuf)); 
			break; 
		} 
	} 

    /* We don't need any more the device list. Free it */
    pcap_freealldevs(alldevs);

	if ( namebuf[0] == 0 ) {  /* did not find device */ 
		printf("did not find device @ >%s<\n", inbuf); 
		return 8;
	} 

	printf("Using: %s (%s)\n", namebuf, descbuf); 


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

	/* target mac addr */ 
	for ( i = 0; i < 6; i++ ) { 
		packet[i] = 0xff; 
	} 
	/* src mac addr */ 
	for ( i = 6; i < 12; i++ ) { 
		packet[i] = macbuf[i-6]; 
	} 
	packet[12] = 0xef; /* set ethertype to 0xeffe */ 
	packet[13] = 0xfe; 

	printf("Beaconing 1 packet/sec ... ctrl-c to stop..."); fflush(stdout); 

	while ( 1 ) { 
		int adder; 
		count_is_odd = (packet_num & 1); 
		if ( count_is_odd ) { 
			adder = 1; 
		} 
		else { 
			adder = 0; 
		} 
		for ( i = 18;  i <  PACKET_LEN; i++ ) { 
			packet[i] = (2*(i-18)) + adder;  
		} 
		memcpy(packet + 14, &packet_num, 4);
		
		rc = pcap_sendpacket(fp, packet, PACKET_LEN); 
		if ( rc != 0 ) { 
			printf("i=%d, pcap_sendpacket failed: %s\n", i, pcap_geterr(fp));
			break;
		} 		
		++packet_num; 
		Sleep(1000); 
	} 
	pcap_close(fp);
}
