/*
 * Metis.c - Support code for OpenHPSDR.org's Metis board 
 * Copyright (C) 2010, 2011  Bill Tracey, KD5TFD (bill@ewjt.com) 
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
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

void process_ozy_input_buffer(char* buffer);

int isMetis; 
SOCKET listenSock; 
SOCKET discoverySock; 
WSADATA WSAdata; 
int WSAinitialized; 
int MetisReadThreadRunning; 


/* globals */ 
extern int receivers;
extern int current_receiver;

int MetisKeepRunning; 
int MetisReadThreadRunning = 0; 
sem_t MetisReadThreadInitSem; 
#define NUM_RINGBUF_FRAMES (200)
void *MetisEP6RingBuf = NULL; 
pthread_t MetisReadThreadID; 
unsigned int MetisLastRecvSeq = 0; 
char MetisMACAddr[6] = { 0, 0, 0, 0, 0, 0 }; 


extern void Dump(FILE *ofile,                /* file handle to dump to - assumed to be      */
                                         /* open.                                       */
    unsigned char *buf,            /* pointer to data to dump                     */
    unsigned int count,            /* number of bytes to dump                      */
    unsigned char *comment)      /* comment put out at top of dump, may be NULL */
{
	// ########   NULL IMPLEMENTATION   ##########
}


SOCKET createSocket(int portnum) { 
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
	bind_addr.sin_addr.S_un.S_addr = INADDR_ANY; // inet_addr("10.1.1.15"); 
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

 u_long doDiscovery() { 
	 // SOCKET outsock; 
	 int rc; 
	 unsigned char packetbuf[250];
	 unsigned char discbuf[2048]; 
	 int i; 
	 struct sockaddr_in dest_addr; 
	 struct sockaddr_in disc_reply_addr; 
	 int disc_reply_addr_len = sizeof(disc_reply_addr);
	 int j;	
	 int btrue = 1; 
	 unsigned char sigbuf[6] = { 0xba, 0xba, 0xba, 
								 0xba, 0xba, 0xba }; 
	 

	 for ( i = 0; i < 250; i++ ) { 
		 packetbuf[i] = 0xba; 
	 } 
	 packetbuf[0] = 0xef; 
	 packetbuf[1] = 0xfe; 
	 packetbuf[2] = 0x02; 
	 
	
	 Dump(stdout, packetbuf, 100, "packetbuf"); 
	 dest_addr.sin_family = AF_INET;
	 dest_addr.sin_port = htons(1024); 
	 dest_addr.sin_addr.s_addr = inet_addr("255.255.255.255"); 
	 // outsock = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP); 
	 // rc = setsockopt(outsock, SOL_SOCKET, SO_BROADCAST, (const char *)&btrue, sizeof(int)); 
	 // if ( rc == SOCKET_ERROR ) { 
     // 	printf("CreateSockets Warning: setsockopt SO_BROADCAST failed!\n"); 
	 // }

	 for ( i = 0; i < 5; i++ ) { 
		sendto(listenSock, packetbuf, 17, 0, (SOCKADDR *)&dest_addr, sizeof(dest_addr)); 
		printf("discovery packet sent\n");  fflush(stdout); 

		for ( j = 0; j < 2; j++ ) {
			rc = recvfrom_withtimeout(listenSock,  discbuf, sizeof(discbuf), 0, 
		                          (SOCKADDR *)&disc_reply_addr, &disc_reply_addr_len, 3, 0); 
			printf("recvfrom_withtimeout rc=%d\n", rc); 
			if ( rc > 0 ) { 
				if ( discbuf[0] == 0xef && discbuf[1] == 0xfe && discbuf[2] == 02  ) { 
					if ( memcmp(discbuf+3, sigbuf, 6) ==0 ) {
						printf("ignoring our own broadcast\n"); 
						continue; 
					}
					printf("got good discovery reply\n"); 
					Dump(stdout, discbuf, rc, "discovery reply"); 
					memcpy(MetisMACAddr, discbuf+3, 6); /* save off MAC Addr */ 
					return disc_reply_addr.sin_addr.S_un.S_addr;
					// return *((u_long *)(&(discbuf[3])));				
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


void DeInitMetisSockets(void) {
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
int nativeInitMetis(char *netaddr) {

	int rc; 
	u_long metis_addr; 
	u_long myaddr; 
	unsigned char macbuf[6]; 
	int i; 
	int sndbufsize; 
	
	isMetis = 1; 
	if ( listenSock == (SOCKET)0 ) { 

		listenSock = createSocket(0); 
		if ( listenSock == INVALID_SOCKET ) { 
			printf("createSocket on listenSock failed.\n");
			return -2; 
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
	}

//	if ( discoverySock == (SOCKET)0 ) { 
//
//		discoverySock = createSocket(1025); 
//
//		if ( discoverySock == INVALID_SOCKET ) { 
//			printf("createSocket on discoverySock failed.\n");
//			return -2; 
//		} 
//	}



	//myaddr = inet_addr(netaddr);
	//printf("myaddr: 0x%08x\n", myaddr); 
	//rc = getMACaddr(myaddr, macbuf); 
	//if ( rc == 0 ) { 
	//	printf("mac addr: "); 
	//	for ( i = 0;  i < 6; i++ ) { 
	//		printf("%02x", macbuf[i]); 
	//	} 
	//	printf("\n"); 
	//} 
	//else { 
	//	printf("getMACaddr failed!!\n"); 		
	//}

	metis_addr = doDiscovery(); 
	printf("metis_addr: 0x%08x\n", metis_addr); 

	fflush(stdout);
	MetisAddr = metis_addr; 

	MetisSockAddr.sin_family = AF_INET;
	MetisSockAddr.sin_port = htons(1024); 
	MetisSockAddr.sin_addr.s_addr = metis_addr;
	
	if ( metis_addr == 0 ) { 
		return -4; 
	}


	return 0; 
}



int GetMetisIPAddr(void) { 
	return MetisAddr; 
} 

void GetMetisMACAddr(char addr_bytes[]) { 
	memcpy(addr_bytes, MetisMACAddr, 6); 
} 

int SendStartToMetis(void) 	 {

	 unsigned int starting_seq; 
	 unsigned char packetbuf[64];
	 unsigned char fbuf[2000]; 

	 int i; 

	 memset(packetbuf, 0, sizeof(packetbuf)); 

	 packetbuf[0] = 0xef; 
	 packetbuf[1] = 0xfe; 
	 packetbuf[2] = 0x04; 
	 packetbuf[3] = 0x01;
	 
	 starting_seq = MetisLastRecvSeq;
	 for ( i = 0; i < 5; i++ ) {
		/* printf("start sent\n"); */ 
		sendto(listenSock, packetbuf, sizeof(packetbuf), 0, (SOCKADDR *)&MetisSockAddr, sizeof(MetisSockAddr)); 
		MetisReadDirect(fbuf, sizeof(fbuf)); 
		if ( MetisLastRecvSeq != starting_seq ) { 
			break; 
		} 
		/* printf("c&c forced\n");  */ 
		ForceCandCFrame(); 
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
	 
	 unsigned int starting_seq; 
	 unsigned char packetbuf[64];

	 int i;  

	 memset(packetbuf, 0, sizeof(packetbuf)); 

	 packetbuf[0] = 0xef; 
	 packetbuf[1] = 0xfe; 
	 packetbuf[2] = 0x04; 
	 packetbuf[3] = 0x00;
	 
	 starting_seq = MetisLastRecvSeq;
	 for ( i = 0; i < 5; i++ ) { 
		sendto(listenSock, packetbuf, sizeof(packetbuf), 0, (SOCKADDR *)&MetisSockAddr, sizeof(MetisSockAddr)); 		
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
	unsigned char readbuf[1600]; 
	struct sockaddr_in fromaddr; 
	int fromlen; 
	int rc; 
	unsigned int endpoint; 
	unsigned int seqnum; 
	unsigned char *seqbytep = (char *)&seqnum; 

	fromlen = sizeof(fromaddr); 
	 
//	rc = recvfrom_withtimeout(listenSock, readbuf, sizeof(readbuf), 0, (struct sockaddr *)&fromaddr, &fromlen, 0, 500000);	// #### djm changed back to recvfrom() without timeout

	 rc = recvfrom(listenSock, readbuf, sizeof(readbuf), 0, (struct sockaddr *)&fromaddr, &fromlen);  
	if ( rc < 0 ) {  /* failed */ 
		printf("MRD: recvfrom on listSock failed w/ rc=%d!\n", rc);  fflush(stdout); 
		return rc; 	
	}
	/* check frame is from who we expect */ 
	if ( rc == 1032 ) {   /* looks like a data frame */ 
		if ( (readbuf[0] == 0xef) && (readbuf[1] == 0xfe) && (readbuf[2] == 0x01) ) { 
			endpoint = (unsigned int)readbuf[3]; 
			seqbytep[3] = readbuf[4]; 
			seqbytep[2] = readbuf[5]; 
			seqbytep[1] = readbuf[6]; 
			seqbytep[0] = readbuf[7]; 
			if ( seqnum != (1 + MetisLastRecvSeq) )  { 
				printf("MRD: seq error this: %d last: %d\n", seqnum, MetisLastRecvSeq); 
			} 
			MetisLastRecvSeq = seqnum;
			if ( endpoint == 6 ) { 
				memcpy(bufp, readbuf+8, 1024); 
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

#define SYNC 0x7f

void MetisReadThreadMainLoop() { 
	int i;
	int rc; 
	int dumpbuf = 1; 
	
	unsigned char readbuf[1600]; 
	unsigned char processBuf[512];

	struct sockaddr_in fromaddr; 
	int fromlen; 
	unsigned int endpoint; 
	unsigned int seqnum; 
	unsigned char *seqbytep = (char *)&seqnum; 
	int start_sending_count = 5; 

	while ( MetisKeepRunning ) { 		
		fromlen = sizeof(fromaddr); 
		rc = recvfrom(listenSock, readbuf, sizeof(readbuf), 0, (struct sockaddr *)&fromaddr, &fromlen); 
		if ( rc < 0 ) {  /* failed */ 
			printf("recvfrom on listenSock in MetisReadThreadManiLoop() failed, return code = #d\n", rc);  fflush(stdout); 
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
				if ( endpoint == 6 ) 
				{
#if 0
					int syncLoc = -1;
					for (i = 8; i < 1029; ++i) 
					{	if ( (readbuf[i] == SYNC) && (readbuf[i+1] == SYNC) && (readbuf[i+2] == SYNC) ) 
						{	syncLoc = i;
							break;
						}
					}
					if (syncLoc >= 8) putRingBuffer(MetisEP6RingBuf, readbuf + syncLoc /* readbuf+8 */ , 1032 - syncLoc) ;
#endif
					putRingBuffer(MetisEP6RingBuf, readbuf + 8, 1024);

//					process_ozy_input_buffer(readbuf+8);
//                  process_ozy_input_buffer(readbuf+520);
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

		skipToSYNC(MetisEP6RingBuf);
		while (getCountRingBuffer(MetisEP6RingBuf) > 512)
		{	getRingBuffer(MetisEP6RingBuf, processBuf, 512, 1);
			process_ozy_input_buffer(processBuf);
		}
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

#if 0
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
#endif

/* write data to Metis board */ 
unsigned int MetisOutBoundSeqNum; 


int MetisWriteFrame(int endpoint, char *bufp, int buflen) { 
	int result; 
	unsigned char framebuf[1032]; 
	unsigned char *p = (unsigned char *)&MetisOutBoundSeqNum; 


	if ( buflen > 1024 ) { 
		return -1; 
	} 
	framebuf[0] = 0xef;
	framebuf[1] = 0xfe; 
	framebuf[2] = 01; 
	framebuf[3] = endpoint; 
	framebuf[4] = p[3]; 
	framebuf[5] = p[2]; 
	framebuf[6] = p[1]; 
	framebuf[7] = p[0]; 
	++MetisOutBoundSeqNum; 
	memcpy(framebuf+8, bufp, buflen); 

	result = sendto(listenSock, framebuf, 8+buflen, 0, (SOCKADDR *)&MetisSockAddr, sizeof(MetisSockAddr)); 	
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