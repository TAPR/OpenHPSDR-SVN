/*
 *  connection.c
 *  SDR
 *
 *  Created by John Melton on 02/07/2009.
 *  Copyright 2009 G0ORX. All rights reserved.
 *
 */

#include <stdio.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <netdb.h>
#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <CoreFoundation/CFSocket.h>
#include "connection.h"
#include "samples.h"
#include "band.h"

#define PREFIX 48

char host[64]="81.146.61.118";
int port=8000;

CFSocketRef s=NULL;
struct sockaddr_in address;
CFRunLoopSourceRef runLoop;

int currentBand=-1;
int displayedBand=-1;

void setHost(char * id) {
	fprintf(stderr,"setHost: %s\n",id);
	strcpy(host,id);
}

void setPort(int p) {
	fprintf(stderr,"setPort: %d\n",p);
	port=p;
}

void socketCallback(CFSocketRef s,CFSocketCallBackType callbackType,CFDataRef address,const void *data,void *info) {
	
	CFSocketNativeHandle handle;
	int bytesRead;
	unsigned char byteSamples[SAMPLES_PER_BUFFER+PREFIX];
	int i;
	
	//fprintf(stderr,"socketCallback\n");
	switch(callbackType) {
		case kCFSocketReadCallBack:
			//fprintf(stderr,"read callback\n");
		    handle=CFSocketGetNative(s);
			if(handle>=0) {
				bytesRead=0;
				while(bytesRead!=SAMPLES_PER_BUFFER+PREFIX) {
				    bytesRead+=read(handle,&byteSamples[bytesRead],SAMPLES_PER_BUFFER+PREFIX
									-bytesRead);
				}
				if(bytesRead==SAMPLES_PER_BUFFER+PREFIX) {
				    strcpy(frequency,(char *)byteSamples);
					filterLow=atoi((char *)&byteSamples[14]);
					filterHigh=atoi((char*)&byteSamples[20]);
					strcpy(mode,(char*)&byteSamples[26]);
					sampleRate=atoi((char*)&byteSamples[32]);
					currentBand=atoi((char*)&byteSamples[40]);
					for(i=0;i<SAMPLES_PER_BUFFER;i++) {
						samples[i]=(float)(-byteSamples[i+PREFIX]);
					}			
				} else {
				}
			}  else {
				fprintf(stderr,"socketCallback: native handle=%d\n",handle);
			}
			
			break;
	    case kCFSocketConnectCallBack:
			fprintf(stderr,"connect callback\n");
			break;
	}
}

void sendCommand(char *command) {
	char buffer[32];
	int i;
	
	//fprintf(stderr,"sendCommand: %s\n",command);
	
	for(i=0;i<32;i++) {
		if(i<strlen(command)) {
			buffer[i]=command[i];
		} else {
			buffer[i]='\0';
		}
	}
	
	if(s!=NULL) {
		// send command to get the samples
	    CFSocketNativeHandle handle;
	    handle=CFSocketGetNative(s);
		send(handle,buffer,32,0);
	}
}

void makeConnection() {
	
	struct hostent *hp;
	
	fprintf(stderr,"makeConnection: %s:%d\n",host,port);
	
	if((strlen(host)>0)&&(port>0)) {
		

	
	    hp=gethostbyname(host);
	    memset(&address, 0, sizeof(address));
	    address.sin_family = AF_INET; 
	    address.sin_addr.s_addr = ((struct in_addr *)(hp->h_addr))->s_addr;
	    address.sin_port = htons(port);
	
	    s=CFSocketCreate(kCFAllocatorDefault,PF_INET,SOCK_STREAM,IPPROTO_TCP,kCFSocketReadCallBack|kCFSocketConnectCallBack,socketCallback,NULL);

	    if(s==NULL) {
		    fprintf(stderr,"CFSocketCreate failed\n");
		    return;
	    }
	
	    fprintf(stderr,"socket created\n");
	
	    CFSocketError err=CFSocketConnectToAddress(s,CFDataCreate(kCFAllocatorDefault,(const UInt8*)&address,sizeof(address)),(CFTimeInterval)10);
	    if(err) {
		    fprintf(stderr,"CFSocketConnectToAddress failed: %d\n",err);
			//CFSocketInvalidate(s);
			s=NULL;
		    return;
	    }
	    fprintf(stderr,"socket connected to %s:%d\n",host,port);
	
	    runLoop=CFSocketCreateRunLoopSource(kCFAllocatorDefault,s,(CFIndex)0);
	    fprintf(stderr,"run loop source created\n");
	
	    CFRunLoopAddSource(CFRunLoopGetCurrent(),runLoop,kCFRunLoopCommonModes);
    	fprintf(stderr,"run loop source added\n");
    }
}



void disconnect() {
	if(s!=NULL) {
	    CFSocketDisableCallBacks(s,kCFSocketReadCallBack|kCFSocketConnectCallBack);
        CFRunLoopRemoveSource(CFRunLoopGetCurrent(),runLoop,kCFRunLoopCommonModes);
		CFSocketInvalidate(s);
		//close(CFSocketGetNative(s));
		s=NULL;
	}
}

void newConnection(const char *newHost,const char *newPort) {
	disconnect();
	strcpy(host,newHost);
	port=atoi(newPort);
	makeConnection();
	
}

int isConnected() {
	return s!=NULL;
}