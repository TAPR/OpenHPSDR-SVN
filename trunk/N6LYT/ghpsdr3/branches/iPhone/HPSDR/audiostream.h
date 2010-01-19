/*
 *  audiostream.h
 *  HPSDR
 *
 *  Created by John Melton on 10/07/2009.
 *  Copyright 2009 __MyCompanyName__. All rights reserved.
 *
 */

#define AUDIO_BUFFER_SIZE 256
#define AUDIO_BUFFERS 3

typedef struct AQCallbackStruct {
    AudioQueueRef queue;
    UInt32 frameCount;
    AudioQueueBufferRef mBuffers[AUDIO_BUFFERS];
    AudioStreamBasicDescription mDataFormat;
    UInt32 playPtr;
    UInt32 sampleLen;
    unsigned char *pcmBuffer;
} AQCallbackStruct;


void audiostream_setHost(char * id);
void audiostream_setPort(int p);
void audiostream_sendCommand(char *command);
void audiostream_makeConnection();
void audiostream_disconnect() ;
void audiostream_newConnection(const char *newHost,const char *newPort);
int audiostream_isConnected();