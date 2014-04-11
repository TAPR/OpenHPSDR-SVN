/*
 * Metis.c - Support code for OpenHPSDR.org's Metis board 
 * Copyright (C) 2010, 2011 Bill Tracey, KD5TFD (bill@ewjt.com) 
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 *
 */



#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "private.h" 

#include <windows.h> 
#include <winsock2.h>
#include <ws2tcpip.h>
#include <wspiapi.h>
#include <Iphlpapi.h>
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#pragma comment(lib, "ws2_32.lib")
#pragma comment(lib, "IPHLPAPI.lib")

#define OBSOLETE (0) 


/* globals */ 
int MetisKeepRunning; 
int MetisReadThreadRunning = 0; 
sem_t MetisReadThreadInitSem; 
#define NUM_RINGBUF_FRAMES (200)
void *MetisEP6RingBuf = NULL; 
pthread_t MetisReadThreadID; 
int MetisLastRecvSeq = 0; 
char MetisMACAddr[6] = { 0, 0, 0, 0, 0, 0 }; 
char MetisCodeVersion[1] = { 0 };
char MetisBoardID[1] = {0 };

extern void Dump(FILE *ofile,                /* file handle to dump to - assumed to be      */
                                         /* open.                                       */
    unsigned char *buf,            /* pointer to data to dump                     */
    unsigned int count,            /* number of bytes to dump                      */
    unsigned char *comment);      /* comment put out at top of dump, may be NULL */



#if 1 
KD5TFDVK6APHAUDIO_API int getNetworkAddrs(int addrs[], int addr_count) { 
	int addrs_used = 0; 
	IP_ADAPTER_ADDRESSES addrbuf[5]; 
	PIP_ADAPTER_ADDRESSES paddrs = addrbuf; 
	PIP_ADAPTER_ADDRESSES p; 

	ULONG addrbufsize = sizeof(addrbuf); 	
	IP_ADAPTER_UNICAST_ADDRESS *unip; 
	ULONG rc;
	CHAR ipaddrbuf[100]; 
	DWORD ipaddrbuflen; 
	ULONG family = AF_INET;
	ULONG flags = GAA_FLAG_SKIP_MULTICAST;

	printf("addrbufsize is: %d\n", addrbufsize); 
	memset(addrbuf, 0, sizeof(addrbuf)); 

	rc = GetAdaptersAddresses(family, flags, NULL, 
		                            paddrs, &addrbufsize); 
	if ( rc == ERROR_BUFFER_OVERFLOW ) {  /* buf too small, realloc and try again */ 
		printf("initial buf too small - regrouping\n"); 
		printf("needed addrbufsize is: %d\n", addrbufsize); 
		//paddrs = new IP_ADAPTER_ADDRESSES[addrbufsize / sizeof(IP_ADAPTER_ADDRESSES)]; 
		paddrs = (PIP_ADAPTER_ADDRESSES) malloc(addrbufsize); 
		if ( paddrs == NULL ) { 
			printf("malloc failed!\n"); fflush(stdout); 
			return -1;
		}

		memset(paddrs, 0, addrbufsize); 
		rc = GetAdaptersAddresses(family, flags, NULL, 
		                            paddrs, &addrbufsize); 
	}
	
	if ( rc != ERROR_SUCCESS ) { 
		printf("GetAdpatersAddresses returned %d\n", rc); fflush(stdout); 
		if ( paddrs != addrbuf ) { 
			free(paddrs);
		}
		return -1;
	} 

	p = paddrs; 
	while ( p != NULL ) { 
		printf("Name: %s\n", p->AdapterName); 
		printf("Friendly Name: %S\n", p->FriendlyName); 
		
		unip = p->FirstUnicastAddress; 
		if ( unip == NULL ) { 
			printf("FirstUnicastAddress is null!!!\n"); 
		} 
		while ( unip != NULL ) { 
			struct sockaddr_in *saddrp; 
			ipaddrbuflen = sizeof(ipaddrbuf); 
			WSAAddressToString(unip->Address.lpSockaddr, unip->Address.iSockaddrLength, NULL, ipaddrbuf,  &ipaddrbuflen); 			
			printf("    %s\n", ipaddrbuf); 
			Dump(stdout, (unsigned char *)(unip->Address.lpSockaddr), unip->Address.iSockaddrLength, (unsigned char *)"sockaddr"); 
			saddrp = (struct sockaddr_in *)(unip->Address.lpSockaddr); 
			printf("addr: 0x%08x\n", saddrp->sin_addr.S_un.S_addr);
			if ( addrs_used < addr_count ) { 
				addrs[addrs_used] = saddrp->sin_addr.S_un.S_addr;
				++addrs_used;
			}
			unip = unip->Next;				
		} 
		p = p->Next; 
	}
	if ( paddrs != addrbuf ) { 
		free(paddrs);
	}
	printf("getNetworkAddrs returning: %d\n", addrs_used); 
	fflush(stdout); 

	return addrs_used; 
} 
#endif

#if 0
/* get MAC addr for a given adapter - returns 0 on success, !0 otherwise */ 
int getMACaddr(int addr_wanted, char mac[6]) { 
	int addrs_used = 0; 


	IP_ADAPTER_ADDRESSES addrbuf[5]; 
	PIP_ADAPTER_ADDRESSES paddrs = addrbuf; 
	PIP_ADAPTER_ADDRESSES p; 

	ULONG addrbufsize = sizeof(addrbuf); 	
	IP_ADAPTER_UNICAST_ADDRESS *unip; 

	ULONG rc;
	CHAR ipaddrbuf[100]; 
	DWORD ipaddrbuflen; 


	printf("addr_wanted:  0x%08x\n", addr_wanted); 
	printf("addrbufsize is: %d\n", addrbufsize); 
	memset(addrbuf, 0, sizeof(addrbuf)); 

	rc = GetAdaptersAddresses(AF_INET,  GAA_FLAG_SKIP_MULTICAST, NULL, 
		                            paddrs, &addrbufsize); 
	if ( rc == ERROR_BUFFER_OVERFLOW ) {  /* buf too small, realloc and try again */ 
		printf("initial buf too small - regrouping\n"); 
		printf("needed addrbufsize is: %d\n", addrbufsize); 
		paddrs = malloc(addrbufsize); 
		if ( paddrs == NULL ) { 
			printf("malloc failed!\n"); fflush(stdout); 
			return -1;
		}
		memset(paddrs, 0, addrbufsize); 
		rc = GetAdaptersAddresses(AF_INET,  GAA_FLAG_SKIP_MULTICAST, NULL, 
		                            paddrs, &addrbufsize); 
	}
	
	if ( rc != ERROR_SUCCESS ) { 
		printf("GetAdpatersAddresses returned %d\n", rc); fflush(stdout); 
		if ( paddrs != addrbuf ) { 
			free(paddrs);
		}
		return -1;
	} 
	p = paddrs; 
	while ( p != NULL ) { 
		printf("Name: %s\n", p->AdapterName); 
		printf("Friendly Name: %S\n", p->FriendlyName); 
		
		unip = p->FirstUnicastAddress; 
		if ( unip == NULL ) { 
			printf("FirstUnicastAddress is null!!!\n"); 
		} 
		while ( unip != NULL ) { 
			struct sockaddr_in *saddrp; 			
			ipaddrbuflen = sizeof(ipaddrbuf); 
			WSAAddressToString(unip->Address.lpSockaddr, unip->Address.iSockaddrLength, NULL, ipaddrbuf,  &ipaddrbuflen); 			
			printf("    %s\n", ipaddrbuf); 
			Dump(stdout, (unsigned char *)(unip->Address.lpSockaddr), unip->Address.iSockaddrLength, "sockaddr"); 
			saddrp = (struct sockaddr_in *)(unip->Address.lpSockaddr); 
			printf("addr: 0x%08x\n", saddrp->sin_addr.S_un.S_addr);
			if ( saddrp->sin_addr.S_un.S_addr == addr_wanted ) {  /* found adapter we're looking for, now find appropriate broadcast addr */ 
				printf("found matching addr!\n"); 
				memcpy(mac, p->PhysicalAddress, min(p->PhysicalAddressLength, 6));
				if ( paddrs != addrbuf ) { 
					free(paddrs);
				}
				return 0;
			}
			else { 
				unip = unip->Next;
			} 
		} 
		p = p->Next; 
	}

	/* if we get here we did not find matching addr!! */ 
	if ( paddrs != addrbuf ) { 
		free(paddrs);
	}
	return -1; 
} 
#endif 

#if 0
void oldlistNetworkAddrs(void) { 
	struct addrinfo hints; 
	struct addrinfo *results = NULL; 
	struct addrinfo *addrp = NULL; 
	int addrcount = 0; 
	int rc; 

	memset(&hints, 0, sizeof(hints));
	hints.ai_family = AF_INET; 
	// hints.ai_socktype = SOCK_DGRAM; 
	// hints.ai_protocol = IPPROTO_UDP;

	rc = getaddrinfo(NULL, NULL, &hints, &results); 
	if (  rc != 0 ) { 
		printf("getaddrinfo failed rc=%d\n", rc); fflush(stdout); 
		return; 
	} 

	addrp = results;

	while ( addrp != NULL ) { 
		++addrcount; 
		printf("   name: >%s<\n", addrp->ai_canonname); 
		addrp = addrp->ai_next;
	} 
	printf("%d adapters found\n", addrcount); fflush(stdout); 
	return;
} 
#endif 


 /* returns 0 on success, non zero on failure */ 
 int initWSA(void) { 
	 int rc;
	 /* int addrs[10];  */ 
	 if ( WSAinitialized ) { 
		 return 0;
	 }
	 WSAinitialized = 1; 
	 rc = WSAStartup(MAKEWORD(2,2), &WSAdata);
	 if ( rc != 0 ) { 
		 printf("WSAStartup failed with rc=%d\n", rc); 
	 }
	 /* getNetworkAddrs(addrs, 10);    */ 
	 return rc; 
 }


SOCKET createSocket(int portnum, u_long addr) { 
	struct sockaddr_in bind_addr; 
	int addrcount = 0; 
	int rc; 
	int btrue = 1; 	
	SOCKET sock; 
	struct sockaddr_in bound_addr; 
	int bound_addr_size; 
	int rcv_timeo; 
	
	sock = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);

	if ( sock == INVALID_SOCKET ) { 
		printf("createSocket Error: socket failed rc=%d\n", WSAGetLastError()); 
		return INVALID_SOCKET;
	} 

	memset(&bind_addr, 0, sizeof(bind_addr)); 
	
	bind_addr.sin_family = AF_INET; 
if (full_discovery) {
	bind_addr.sin_addr.S_un.S_addr = addr;
}
else {
	bind_addr.sin_addr.S_un.S_addr = INADDR_ANY; // inet_addr("10.1.1.15"); 
}
	bind_addr.sin_port = htons(portnum); 
	
	rc = bind(sock, (struct sockaddr *)&bind_addr, (int)sizeof(bind_addr)); 

	if ( rc == SOCKET_ERROR ) { 
		printf("createSocket Error: bind failed rc=%d\n", WSAGetLastError()); 
		return INVALID_SOCKET;		
	} 

	bound_addr_size = sizeof(bound_addr);
	rc = getsockname(sock, (struct sockaddr *)&bound_addr, &bound_addr_size); 
	if ( rc == 0 ) {
		printf("bound_addr: 0x%08x\n", bound_addr.sin_addr.S_un);
	} 
	else { 
		printf("getsockname failed\n");
	} 
	
	rc = setsockopt(sock, SOL_SOCKET, SO_BROADCAST, (const char *)&btrue, sizeof(int)); 
	if ( rc == SOCKET_ERROR ) { 
		printf("CreateSockets Warning: setsockopt SO_BROADCAST failed!\n"); 
	}

	rcv_timeo = 500; 
	rc = setsockopt(sock, SOL_SOCKET, SO_RCVTIMEO, (const char *)&rcv_timeo, sizeof(int)); 
	if ( rc == SOCKET_ERROR ) { 
		printf("CreateSockets Warning: setsockopt SO_RCVTIMEO failed!\n"); 
	}

	fflush(stdout); 
	return sock;
 } 

 int recvfrom_withtimeout(SOCKET s, char *buf, int buflen, int flags, struct sockaddr *fromsockp, int *fromlenp, long secs, long usecs ) { 
	struct timeval timeout; 
	struct fd_set rdsocks; 
	int rc; 
	rdsocks.fd_count = 1; 
	rdsocks.fd_array[0] = s; 
	timeout.tv_sec = secs;
	timeout.tv_usec = usecs; 

#if 0		
	rc = select(0, &rdsocks, NULL, NULL, &timeout); 
	if ( rc == 0 ) { 
		printf("select timed out\n"); 
		return 0; 
	} 
	if ( rc == SOCKET_ERROR ) { 
		printf("select failed.  WSAGetLAstError=%d\n", WSAGetLastError()); 
		return SOCKET_ERROR; 
	}
#endif 
	rc = recvfrom(s, buf, buflen, flags, (SOCKADDR *)fromsockp, fromlenp); 
	return rc; 
 } 

 u_long doDiscovery(char *netaddr, int dostatic) { 
	 int rc; 
	 struct outdgram {
		 unsigned char packetbuf[63];
	 } outpacket;
	 struct indgram {
		 unsigned char discbuf[2048];
	 } inpacket;

	 int i; 
	 struct sockaddr_in dest_addr; 
	 struct sockaddr_in disc_reply_addr; 
	 int disc_reply_addr_len = sizeof(disc_reply_addr);
	 int j;	
	 int btrue = 1; 
	 unsigned char sigbuf[6] = { 0xba, 0xba, 0xba, 
		 0xba, 0xba, 0xba }; 


	 // for ( i = 0; i < 250; i++ ) { 
	 // packetbuf[i] = 0xba; 
	 //} 
	 memset(outpacket.packetbuf, 0, sizeof(outpacket));
	 outpacket.packetbuf[0] = 0xef; 
	 outpacket.packetbuf[1] = 0xfe; 
	 outpacket.packetbuf[2] = 0x02; 

	 Dump(stdout, outpacket.packetbuf, 100, (unsigned char *)"packetbuf"); 
	 dest_addr.sin_family = AF_INET;
	 dest_addr.sin_port = htons(1024); 
	 dest_addr.sin_addr.s_addr = inet_addr("255.255.255.255"); 
	 // outsock = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP); 
	 // rc = setsockopt(outsock, SOL_SOCKET, SO_BROADCAST, (const char *)&btrue, sizeof(int)); 
	 // if ( rc == SOCKET_ERROR ) { 
	 // 	printf("CreateSockets Warning: setsockopt SO_BROADCAST failed!\n"); 
	 // }

	 for ( i = 0; i < 5; i++ ) { 
		 sendto(listenSock, (char *) &outpacket, sizeof(outpacket), 0, (SOCKADDR *)&dest_addr, sizeof(dest_addr)); 
		 printf("discovery packet sent\n");  fflush(stdout); 

		 for ( j = 0; j < 2; j++ ) {
			 rc = recvfrom_withtimeout(listenSock,  (char *) &inpacket, sizeof(inpacket), 0, 
				 (SOCKADDR *)&disc_reply_addr, &disc_reply_addr_len, 3, 0); 
			 printf("recvfrom_withtimeout rc=%d\n", rc); 
			 if ( rc > 0 ) { 
				 if ( inpacket.discbuf[0] == 0xef && inpacket.discbuf[1] == 0xfe && inpacket.discbuf[2] == 02  ) { 
					 if ( memcmp(inpacket.discbuf+3, sigbuf, 6) == 0 ) {
						 printf("ignoring our own broadcast\n"); 
						 continue; 
					 }
					 printf("got good discovery reply\n"); 
					 Dump(stdout, inpacket.discbuf, rc, (unsigned char *)"discovery reply"); 
					 memcpy(MetisMACAddr, inpacket.discbuf+3, 6); /* save off MAC Addr */ 
					 memcpy(MetisCodeVersion, inpacket.discbuf+9, 1); /* save off loaded Code Version */
					 memcpy(MetisBoardID, inpacket.discbuf+10, 1); /* save off Board_ID 0x00 = Metis, 0x01 = Hermes, 0x02 = Griffin */

					 if (dostatic) {
						 if (disc_reply_addr.sin_addr.S_un.S_addr == inet_addr(netaddr)) {
							 return disc_reply_addr.sin_addr.S_un.S_addr;
						 }
					 }
					 else {
						 return disc_reply_addr.sin_addr.S_un.S_addr;
					 }
				 }
			 } 
			 else { 
				 printf("disc reply rc=%d\n", rc); 
			 } 
		 }
	 }					
	 fflush(stdout); 
	 return 0; 
 }

 u_long MetisAddr = 0; 
struct sockaddr_in MetisSockAddr; 

int WSA_inited = 0; 


KD5TFDVK6APHAUDIO_API void DeInitMetisSockets(void) {
	if ( listenSock != (SOCKET)0 ) { 
		shutdown(listenSock, SD_BOTH); 
		closesocket(listenSock); 
		listenSock = (SOCKET)0; 
	} 
//	if ( discoverySock != (SOCKET)0) { 
//		shutdown(discoverySock, SD_BOTH); 
//		discoverySock = 0; 
//	} 
}


/* returns 0 on success, != 0 otherwise */ 
KD5TFDVK6APHAUDIO_API int nativeInitMetis(char *netaddr, int dostatic) {

	int rc; 
	u_long metis_addr; 
	u_long myaddr; 
	unsigned char macbuf[6]; 
	int i; 
	int j;
	int sndbufsize; 
	int addrs[10]; 
	int addr_count; 

	isMetis = 1; 

	if ( !WSA_inited ) { 
		rc = initWSA(); 
		if ( rc != 0 ) { 
			return rc; 
		} 
		WSA_inited = 1; 
		printf("initWSA ok!"); 
	}

	  /*  if (dostatic) {
			addr_count = 1;
			addrs[0] = inet_addr(netaddr);
		}
		else */
		addr_count = getNetworkAddrs(addrs, 10);   
		printf("addr_count: %d\n", addr_count); 
		for ( j = 0; j < addr_count; j++ ) { 

			if ( listenSock == (SOCKET)0 ) { 
				if (full_discovery) {
					closesocket(listenSock); 
					listenSock = (SOCKET)0; 
				}
			}
			listenSock = createSocket(0, addrs[j]); 

			if ( listenSock == INVALID_SOCKET ) { 
				printf("createSocket on listenSock failed.\n");
				if (full_discovery) {
					continue;
				}
				else {
					return -2; 
				}
			} 

			sndbufsize = 0xffff;
			rc = setsockopt(listenSock, SOL_SOCKET, SO_SNDBUF, (const char *)&sndbufsize, sizeof(int)); 
			if ( rc == SOCKET_ERROR ) { 
				printf("CreateSockets Warning: setsockopt SO_SNDBUF failed!\n"); 
			}

			sndbufsize = 0xffff;
			rc = setsockopt(listenSock, SOL_SOCKET, SO_RCVBUF, (const char *)&sndbufsize, sizeof(int)); 
			if ( rc == SOCKET_ERROR ) { 
				printf("CreateSockets Warning: setsockopt SO_RCVBUF failed!\n"); 
			}

			metis_addr = doDiscovery(netaddr, dostatic); 						

			if ( metis_addr != 0 ) { 
				printf("metis_addr: 0x%08x\n", metis_addr); 
				fflush(stdout);
				MetisAddr = metis_addr; 

				MetisSockAddr.sin_family = AF_INET;
				MetisSockAddr.sin_port = htons(1024); 
				MetisSockAddr.sin_addr.s_addr = metis_addr;
				return 0; 
			}
		}
	//}

	/* if we get here no metis was found */ 
	return -4; 
}



KD5TFDVK6APHAUDIO_API int GetMetisIPAddr(void) { 
	return MetisAddr; 
} 

KD5TFDVK6APHAUDIO_API void GetMetisMACAddr(char addr_bytes[]) { 
	memcpy(addr_bytes, MetisMACAddr, 6); 
} 

KD5TFDVK6APHAUDIO_API void GetMetisCodeVersion(char addr_bytes[]) { 
	memcpy(addr_bytes, MetisCodeVersion, 1); 
} 

KD5TFDVK6APHAUDIO_API void GetMetisBoardID(char addr_bytes[]) { 
	memcpy(addr_bytes, MetisBoardID, 1); 
} 


int SendStartToMetis(void) 	 {

	 int starting_seq; 
	 struct outdgram {
		 unsigned char packetbuf[64];
	 } outpacket;

	 struct indgram {
		 unsigned char fbuf[2000];
	 } inpacket;

	 int i; 

	 memset(outpacket.packetbuf, 0, sizeof(outpacket)); 

	 outpacket.packetbuf[0] = 0xef; 
	 outpacket.packetbuf[1] = 0xfe; 
	 outpacket.packetbuf[2] = 0x04; 
	 outpacket.packetbuf[3] = 0x01;
	 
	 starting_seq = MetisLastRecvSeq;
	 for ( i = 0; i < 5; i++ ) {
		/* printf("start sent\n"); */ 
		ForceCandCFrame(1);
		sendto(listenSock, (char *) &outpacket, sizeof(outpacket), 0, (SOCKADDR *)&MetisSockAddr, sizeof(MetisSockAddr)); 
		MetisReadDirect((char *) &inpacket, sizeof(inpacket)); 
		if ( MetisLastRecvSeq != starting_seq ) { 
			break; 
		} 
		/* printf("c&c forced\n");  */ 
		//ForceCandCFrame(); 
		Sleep(10); 
//		if ( MetisLastRecvSeq != starting_seq ) { 
//			break; 
//		} 
	 }
	 /* fflush(stdout);  */ 
	 if ( MetisLastRecvSeq == starting_seq ) {		 
		 return -1; 
	 } 
	 return 0; 
} 


/* returns 0 if metis appears to have stopped, !0 otherwise */
int SendStopToMetis(void) 	 {
	 
	 int starting_seq; 
	// unsigned char packetbuf[64];
	 struct outdgram {
		 unsigned char packetbuf[64];
	 } outpacket;

	 int i;  

	 memset(outpacket.packetbuf, 0, sizeof(outpacket)); 

	 outpacket.packetbuf[0] = 0xef; 
	 outpacket.packetbuf[1] = 0xfe; 
	 outpacket.packetbuf[2] = 0x04; 
	 outpacket.packetbuf[3] = 0x00;
	 
	 starting_seq = MetisLastRecvSeq;
	 for ( i = 0; i < 5; i++ ) { 
		sendto(listenSock, (char *) &outpacket, sizeof(outpacket), 0, (SOCKADDR *)&MetisSockAddr, sizeof(MetisSockAddr)); 		
		Sleep(10); 
		if ( MetisLastRecvSeq == starting_seq ) { 
			break; 
		} 
	 }
	 if ( MetisLastRecvSeq != starting_seq ) {
		 return -1; 
	 } 
	 return 0; 
} 

int MetisReadDirect(char *bufp, int buflen) { 
	struct indgram {
		unsigned char readbuf[1074];
	} inpacket;

	struct sockaddr_in fromaddr; 
	int fromlen; 
	int rc; 
	unsigned int endpoint; 
	unsigned int seqnum; 
	unsigned char *seqbytep = (unsigned char *)&seqnum; 

	fromlen = sizeof(fromaddr); 
	 
	rc = recvfrom_withtimeout(listenSock, (char *) &inpacket, sizeof(inpacket), 0, (struct sockaddr *)&fromaddr, &fromlen, 0, 500000); 
	/* rc = recvfrom(listenSock, readbuf, sizeof(readbuf), 0, (struct sockaddr *)&fromaddr, &fromlen);  */ 
	if ( rc < 0 ) {  /* failed */ 
		printf("MRD: recvfrom on listSock failed w/ rc=%d!\n", rc);  fflush(stdout); 
		return rc; 	
	}
	/* check frame is from who we expect */ 
	if ( rc == 1032 ) {   /* looks like a data frame */ 
		if ( (inpacket.readbuf[0] == 0xef) && (inpacket.readbuf[1] == 0xfe) && (inpacket.readbuf[2] == 0x01) ) { 
			endpoint = (unsigned int)inpacket.readbuf[3]; 
			seqbytep[3] = inpacket.readbuf[4]; 
			seqbytep[2] = inpacket.readbuf[5]; 
			seqbytep[1] = inpacket.readbuf[6]; 
			seqbytep[0] = inpacket.readbuf[7]; 
			if ( seqnum != (1 + MetisLastRecvSeq) )  { 
				printf("MRD: seq error this: %d last: %d\n", seqnum, MetisLastRecvSeq); 
			} 
			MetisLastRecvSeq = seqnum;

			if ( endpoint == 6 ) { 
				if ( (inpacket.readbuf[8] == 0x7f) && (inpacket.readbuf[9] == 0x7f) && (inpacket.readbuf[10] == 0x7f) ) { 
					HaveSync = 1;
				}
				else { 
					HaveSync = 0;
					printf("MRD: sync error on frame %d\n", seqnum); 
					// return 0;
				} 
				memcpy(bufp, inpacket.readbuf+8, 1024); 
				return 1024; 
			}
			else { 
				printf("MRD: ignoring data for ep %d\n", endpoint); 
			} 
		} 
		else { 
			printf("MRD: ignoring right sized frame bad header!\n", rc); 			
		} 
	} 
	else { 
		printf("MRD: ignoring short frame size=%d\n", rc); 
	} 
	return 0; 
} 


void MetisReadThreadMainLoop() { 
#if 0
	int rc; 
	int dumpbuf = 1; 
	
	unsigned char readbuf[1600]; 
	struct sockaddr_in fromaddr; 
	int fromlen; 
	unsigned int endpoint; 
	unsigned int seqnum; 
	unsigned char *seqbytep = (unsigned char *)&seqnum; 
//	int start_sending_count = 5; 
#endif

	while ( MetisKeepRunning ) { 		
#if 1
		Sleep(1000); 
#else
		fromlen = sizeof(fromaddr); 
		rc = recvfrom(listenSock, readbuf, sizeof(readbuf), 0, (struct sockaddr *)&fromaddr, &fromlen); 
		if ( rc < 0 ) {  /* failed */ 
			printf("recvfrom on listSock failed!\n");  fflush(stdout); 
			Sleep(10); /* stay out of a tight loop */ 
			continue; /* go try again */ 
		} 
		/* else */ 
        /* check frame is from who we expect */ 
		if ( rc == 1032 ) {   /* looks like a data frame */ 
			if ( (readbuf[0] == 0xef) && (readbuf[1] == 0xfe) && (readbuf[2] == 0x01) ) { 
				endpoint = (unsigned int)readbuf[3]; 
				seqbytep[3] = readbuf[4]; 
				seqbytep[2] = readbuf[5]; 
				seqbytep[1] = readbuf[6]; 
				seqbytep[0] = readbuf[7]; 
				if ( seqnum != (1 + MetisLastRecvSeq) )  { 
					printf("seq error this: %d last: %d\n", seqnum, MetisLastRecvSeq); 
				} 
				MetisLastRecvSeq = seqnum;
				if ( endpoint == 6 ) { 
					putRingBuffer(MetisEP6RingBuf, readbuf+8 , 1024) ; 
				}
				else { 
					printf("ignoring data for ep %d\n", endpoint); 
				} 
			} 
			else { 
				printf("ignoring right sized frame bad header!\n", rc); 
				if ( dumpbuf ) { 
					dumpbuf =  0; 
					Dump(stdout, readbuf, 200, "readbuf"); 
				} 
			} 
		} 
		else { 
			printf("ignoring short frame size=%d\n", rc); 
		} 		
#endif
	}
	return; 
} 


// this is the main thread that reads data from Metis
void *MetisReadThreadMain(void *argp) {	
	MetisKeepRunning = 1;
    MetisReadThreadRunning = 1;
#ifndef LINUX
     // SetThreadPriority(GetCurrentThread(), /* THREAD_PRIORITY_ABOVE_NORMAL */  THREAD_PRIORITY_TIME_CRITICAL /* THREAD_PRIORITY_HIGHEST  */ ); 
#else
#warning message("info - LINUX code missing ... set priority!")
#endif

    sem_post(&MetisReadThreadInitSem); // tell launching thread we're rockin and rollin
	printf("MetisReadThread runs...\n"); fflush(stdout); 
    MetisReadThreadMainLoop();
    MetisReadThreadRunning = 0;
	printf("MetisReadThread dies...\n"); fflush(stdout); 

	return NULL; 
}


/* returns 0 on sucess, !0 on failure */ 
int MetisStartReadThread(void) {
	int myrc = 0; 
	int rc; 

	do { 
		/* allocate ring buffer */ 
		MetisEP6RingBuf = createRingBuffer(512 * NUM_RINGBUF_FRAMES); 
		if ( MetisEP6RingBuf == NULL ) { 
			return -1; 
		} 
		sem_init(&MetisReadThreadInitSem, 0, 0);
		rc = pthread_create(&MetisReadThreadID, NULL, MetisReadThreadMain, NULL);
		if ( rc != 0 ) {  // failed
			fprintf(stderr, "pthread_created failed on MetisReadThread w/ rc=%d\n", rc);
			myrc =  -2;   
			break; 
		}
		sem_wait(&MetisReadThreadInitSem);  // wait for the thread to get going

		rc = SendStartToMetis(); 
		if ( rc != 0 ) { 
			printf("SendStartTo Metis failed ...\n"); fflush(stdout); 
			MetisStopReadThread(); 
			myrc = -3; 
			break; 
		} 


	} while ( 0 ); 

	if ( myrc != 0 ) {  /* something died - cleanup */ 
		if ( MetisEP6RingBuf != NULL ) { 
			destroyRingBuffer(MetisEP6RingBuf); 
			MetisEP6RingBuf = NULL; 
		} 		
	} 		
	return myrc; 
} 

void MetisStopReadThread(void) {
	SendStopToMetis(); 
	MetisKeepRunning = 0; 
	DeInitMetisSockets(); 
	return; 
} 

/* read data from the ringbuffer */ 
int MetisBulkRead(int endpoint, char *bufp, int buflen) { 
	int result; 
	int sleepcount = 10; 
	if ( endpoint != 6 ) { 
		printf("ep != 6!!\n");  fflush(stdout); 
		return 0; 
	} 
	while ( 1 ) { 
		result = getRingBuffer(MetisEP6RingBuf, bufp, buflen, 0); 
		if ( result != 0 ) { 
			break; 
		} 		
		--sleepcount; 
		if ( sleepcount <= 0 ) break; 
		Sleep(13);
	}
#if 0 
	if ( sleepcount <= 8 ) { 
		printf("info: sleepcount: %d\n", sleepcount); fflush(stdout); 
	} 
#endif 
	return result;
} 


/* write data to Metis board */ 
unsigned int MetisOutBoundSeqNum; 


int MetisWriteFrame(int endpoint, char *bufp, int buflen) { 
	int result; 
	//unsigned char framebuf[1032]; 
	struct outdgram {
		unsigned char framebuf[1032];
	} outpacket;

	unsigned char *p = (unsigned char *)&MetisOutBoundSeqNum; 


	if ( buflen > 1024 ) { 
		return -1; 
	} 
	outpacket.framebuf[0] = 0xef;
	outpacket.framebuf[1] = 0xfe; 
	outpacket.framebuf[2] = 01; 
	outpacket.framebuf[3] = endpoint; 
	outpacket.framebuf[4] = p[3]; 
	outpacket.framebuf[5] = p[2]; 
	outpacket.framebuf[6] = p[1]; 
	outpacket.framebuf[7] = p[0]; 
	++MetisOutBoundSeqNum; 
	memcpy(outpacket.framebuf+8, bufp, buflen); 

	result = sendto(listenSock, (char *) &outpacket, 8+buflen, 0, (SOCKADDR *)&MetisSockAddr, sizeof(MetisSockAddr)); 	
	result -= 8; 
	return result;
} 


int MetisBulkWrite(int endpoint, char *bufp, int buflen) { 
	int framecount; 
	int leftover; 
	int i; 
	int result = 0; 
	int rc; 

	framecount = buflen / 1024; 
	leftover = buflen - (framecount * 1024); 
	if ( leftover != 0 ) { 
		printf("mayday  - MetisBulkWrite - buflen not even multiple of 1024!!\n");  fflush(stdout); 
	} 
	for ( i = 0; i < framecount; i++ ) { 
		rc = MetisWriteFrame(endpoint, bufp + (i*1024), 1024); 
		if ( rc != 1024 ) { 
			printf("warning MetisFrameWrite rc: %d\n", rc); fflush(stdout); 
		} 
		result += rc; 
	} 
	return result; 
}

void SendCommandToMetis(unsigned char command) 	 {
	 
	 struct outdgram {
		 unsigned char packetbuf[64];
	 } outpacket;

	 int i;  

	 memset(outpacket.packetbuf, 0, sizeof(outpacket)); // fill the frame with 0x00

	 outpacket.packetbuf[0] = 0xef; 
	 outpacket.packetbuf[1] = 0xfe; 
	 outpacket.packetbuf[2] = 0x03; 
	 outpacket.packetbuf[3] = command;
	 
	 sendto(listenSock, (char *) &outpacket, sizeof(outpacket), 0, (SOCKADDR *)&MetisSockAddr, sizeof(MetisSockAddr)); 
} 