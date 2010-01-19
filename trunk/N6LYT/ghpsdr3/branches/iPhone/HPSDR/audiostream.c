/*
 *  audiostream.c
 *  HPSDR
 *
 *  Created by John Melton on 10/07/2009.
 *  Copyright 2009 __MyCompanyName__. All rights reserved.
 *
 */

#include <stdio.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <netdb.h>
#include <string.h>
#include <unistd.h>
#include <CoreFoundation/CFSocket.h>
#include <AudioToolbox/AudioQueue.h>


#include "audiostream.h"


char audiostream_host[64]="81.146.61.118";
int audiostream_port=8002;

CFSocketRef audiostream_s=NULL;
struct sockaddr_in audiostream_address;
CFRunLoopSourceRef audiostream_runLoop;


unsigned char audiostream_buffer[AUDIO_BUFFER_SIZE];

AQCallbackStruct aqc;
int buffer=0;
int started=0;


void aqInit();


void audiostream_setHost(char * id) {
	fprintf(stderr,"audiostream_setHost: %s\n",id);
	strcpy(audiostream_host,id);
}

void audiostream_setPort(int p) {
	fprintf(stderr,"audiostream_setPort: %d\n",p);
	audiostream_port=p;
}

void audiostream_socketCallback(CFSocketRef s,CFSocketCallBackType callbackType,CFDataRef address,const void *data,void *info) {
	
	CFSocketNativeHandle handle;
	int bytesRead;
	UInt32 err;
	

	
	//fprintf(stderr,"socketCallback\n");
	switch(callbackType) {
		case kCFSocketReadCallBack:
			//fprintf(stderr,"socket read callback\n");
		    handle=CFSocketGetNative(s);			
			if(handle>=0) {
				bytesRead=0;
				while(bytesRead!=AUDIO_BUFFER_SIZE) {
				    bytesRead+=read(handle,&audiostream_buffer[bytesRead],AUDIO_BUFFER_SIZE-bytesRead);
				}
				if(bytesRead==AUDIO_BUFFER_SIZE) {
					
					memcpy(aqc.mBuffers[buffer]->mAudioData,audiostream_buffer,AUDIO_BUFFER_SIZE);
					aqc.mBuffers[buffer]->mAudioDataByteSize=AUDIO_BUFFER_SIZE;
					err=AudioQueueEnqueueBuffer(aqc.queue,aqc.mBuffers[buffer],0,NULL);
					if(err) {
						fprintf(stderr,"AudioQueueEnqueueBuffer error=%d\n",err);
					}
					buffer++;
					if(buffer>=AUDIO_BUFFERS) {
						
						if(started==0) {
							fprintf(stderr,"AudioQueueStart\n");
						    err = AudioQueueStart(aqc.queue, NULL);
							if(err) {
							    fprintf(stderr,"AudioQueueStart: err=%d\n",err);
						    }
						    started=1;
					    }
						buffer=0;
					}

					
				} else {
				}
			}  else {
				fprintf(stderr,"audiostream_socketCallback: native handle=%d\n",handle);
			}
			break;
	    case kCFSocketConnectCallBack:
			fprintf(stderr,"audiostream_connect callback\n");
			break;
	}
}

void audiostream_sendCommand(char *command) {
	
	//fprintf(stderr,"sendCommand: %s\n",command);
	
	if(audiostream_s!=NULL) {
		// send command to get the samples
	    CFSocketNativeHandle handle;
	    handle=CFSocketGetNative(audiostream_s);
		send(handle,command,strlen(command)+1,0);
	}
}

void audiostream_makeConnection() {
	
	struct hostent *hp;
	
	fprintf(stderr,"makeConnection: %s:%d\n",audiostream_host,audiostream_port);
	
	if((strlen(audiostream_host)>0)&&(audiostream_port>0)) {
		
		
		
	    hp=gethostbyname(audiostream_host);
	    memset(&audiostream_address, 0, sizeof(audiostream_address));
	    audiostream_address.sin_family = AF_INET; 
	    audiostream_address.sin_addr.s_addr = ((struct in_addr *)(hp->h_addr))->s_addr;
	    audiostream_address.sin_port = htons(audiostream_port);
		
	    audiostream_s=CFSocketCreate(kCFAllocatorDefault,PF_INET,SOCK_STREAM,IPPROTO_TCP,kCFSocketReadCallBack|kCFSocketConnectCallBack,audiostream_socketCallback,NULL);
		
	    if(audiostream_s==NULL) {
		    fprintf(stderr,"CFSocketCreate failed audiostream\n");
		    return;
	    }
		
	    fprintf(stderr,"audiostream socket created\n");
		
	    CFSocketError err=CFSocketConnectToAddress(audiostream_s,CFDataCreate(kCFAllocatorDefault,(const UInt8*)&audiostream_address,sizeof(audiostream_address)),(CFTimeInterval)10);
	    if(err) {
		    fprintf(stderr,"audiostream CFSocketConnectToAddress failed: %d\n",err);
			//CFSocketInvalidate(s);
			audiostream_s=NULL;
		    return;
	    }
	    fprintf(stderr,"audiostream socket connected to %s:%d\n",audiostream_host,audiostream_port);
		
	    audiostream_runLoop=CFSocketCreateRunLoopSource(kCFAllocatorDefault,audiostream_s,(CFIndex)0);
	    fprintf(stderr,"audiostream run loop source created\n");
		
	    CFRunLoopAddSource(CFRunLoopGetCurrent(),audiostream_runLoop,kCFRunLoopCommonModes);
    	fprintf(stderr,"audiostream run loop source added\n");
		
		aqInit();
    }
}



void audiostream_disconnect() {
	if(audiostream_s!=NULL) {
	    CFSocketDisableCallBacks(audiostream_s,kCFSocketReadCallBack|kCFSocketConnectCallBack);
        CFRunLoopRemoveSource(CFRunLoopGetCurrent(),audiostream_runLoop,kCFRunLoopCommonModes);
		CFSocketInvalidate(audiostream_s);
		audiostream_s=NULL;
	}
}

void audiostream_newConnection(const char *newHost,const char *newPort) {
	audiostream_disconnect();
	strcpy(audiostream_host,newHost);
	audiostream_port=atoi(newPort);
	audiostream_makeConnection();
	
}

int audiostream_isConnected() {
	return audiostream_s!=NULL;
}


//---------------------------------------------
//
// Audio Queue
//
//---------------------------------------------

static void aqOutputCallback(void *aqCustomData,AudioQueueRef aqRef,AudioQueueBufferRef aqBufferRef) {
	
	//fprintf(stderr,"aqOutputCallback %d\n",aqBufferRef->mAudioDataBytesCapacity);
	//memcpy(aqBufferRef->mAudioData,audiostream_buffer,aqBufferRef->mAudioDataBytesCapacity);
	//AudioQueueEnqueueBuffer(aqRef,aqBufferRef,0,NULL);

}


void aqInit() {
	
	int i;

    aqc.mDataFormat.mSampleRate = 8000.0;
	//aqc.mDataFormat.mFormatID = kAudioFormatLinearPCM;
	aqc.mDataFormat.mFormatID = kAudioFormatALaw;
	
	//aqc.mDataFormat.mFormatFlags = kLinearPCMFormatFlagIsSignedInteger|kAudioFormatFlagIsPacked;
	aqc.mDataFormat.mFormatFlags = kAudioFormatFlagIsPacked;
	aqc.mDataFormat.mBytesPerPacket = 1;
    aqc.mDataFormat.mFramesPerPacket = 1;
    aqc.mDataFormat.mBytesPerFrame = 1;
    aqc.mDataFormat.mChannelsPerFrame = 1;
    aqc.mDataFormat.mBitsPerChannel = 8;
    aqc.frameCount = AUDIO_BUFFER_SIZE;
	
	UInt32 err = AudioQueueNewOutput(&aqc.mDataFormat,
							  aqOutputCallback,
							  &aqc,
							  NULL,
							  NULL,
							  0,
							  &aqc.queue);
    if (err) {
		fprintf(stderr,"AudioQueueNewOutput: err=%d\n",err);
	}

	for (i=0; i<AUDIO_BUFFERS; i++) {
		//fprintf(stderr,"AudioQueueAllocateBuffer\n");
        err = AudioQueueAllocateBuffer(aqc.queue, AUDIO_BUFFER_SIZE,
									   &aqc.mBuffers[i]);
        if (err) {
			fprintf(stderr,"AudioQueueAllocateBuffer: err=%d\n",err);
		}
	
    }
	
	buffer=0;
	started=0;
		
}