/**
* @file ozyio.c
* @brief USB I/O with Ozy
* @author John Melton, G0ORX/N6LYT
* @version 0.1
* @date 2009-10-13
*/


/* Copyright (C)
* 2009 - John Melton, G0ORX/N6LYT
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*
*/

#include <unistd.h>
#include <fcntl.h>
#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/ioctl.h>
#include <portaudio.h>

#include "softrock.h"
#include "softrockio.h"

#define SAMPLE_RATE 48000   /* the sampling rate */
#define CHANNELS 2  /* 1 = mono 2 = stereo */
#define SAMPLES_PER_BUFFER 1024

static PaStream* stream;

int softrock_open(void) {
    int arg;
    int status;
    int rc;
    PaStreamParameters inputParameters;
    PaStreamParameters outputParameters;
    PaStreamInfo *info;
    int devices;
    int i;
    PaDeviceInfo* deviceInfo;

fprintf(stderr,"softrock_open: portaudio\n");


    rc=Pa_Initialize();
    if(rc!=paNoError) {
        fprintf(stderr,"Pa_Initialize failed: %s\n",Pa_GetErrorText(rc));
        exit(1);
    }

    devices=Pa_GetDeviceCount();
    if(devices<0) {
        fprintf(stderr,"Px_GetDeviceCount failed: %s\n",Pa_GetErrorText(devices));
    } else {
        fprintf(stderr,"default input=%d output=%d devices=%d\n",Pa_GetDefaultInputDevice(),Pa_GetDefaultOutputDevice(),devices);

        for(i=0;i<devices;i++) {
            deviceInfo=Pa_GetDeviceInfo(i);
            fprintf(stderr,"%d - %s\n",i,deviceInfo->name);
                fprintf(stderr,"maxInputChannels: %d\n",deviceInfo->maxInputChannels);
                fprintf(stderr,"maxOututChannels: %d\n",deviceInfo->maxOutputChannels);
                fprintf(stderr,"defaultLowInputLatency: %f\n",deviceInfo->defaultLowInputLatency);
                fprintf(stderr,"defaultLowOutputLatency: %f\n",deviceInfo->defaultLowOutputLatency);
                fprintf(stderr,"defaultHighInputLatency: %f\n",deviceInfo->defaultHighInputLatency);
                fprintf(stderr,"defaultHighOutputLatency: %f\n",deviceInfo->defaultHighOutputLatency);
                fprintf(stderr,"defaultSampleRate: %f\n",deviceInfo->defaultSampleRate);
        }
    }

    inputParameters.device=Pa_GetDefaultInputDevice();
    inputParameters.channelCount=2;
    inputParameters.sampleFormat=paFloat32;
    inputParameters.suggestedLatency=Pa_GetDeviceInfo(inputParameters.device)->defaultLowInputLatency;
    inputParameters.hostApiSpecificStreamInfo=NULL;

    outputParameters.device=Pa_GetDefaultOutputDevice();
    outputParameters.channelCount=2;
    outputParameters.sampleFormat=paFloat32;
    outputParameters.suggestedLatency=Pa_GetDeviceInfo(outputParameters.device)->defaultLowOutputLatency;
    outputParameters.hostApiSpecificStreamInfo=NULL;

fprintf(stderr,"input device=%d output device=%d\n",inputParameters.device,outputParameters.device);
    rc=Pa_OpenStream(&stream,&inputParameters,&outputParameters,(double)softrock_get_sample_rate(),(unsigned long)SAMPLES_PER_BUFFER,paNoFlag,NULL,NULL);
/*
    rc=Pa_OpenDefaultStream(&stream,CHANNELS,CHANNELS,paFloat32,SAMPLE_RATE,SAMPLES_PER_BUFFER,NULL, NULL);
*/
    if(rc!=paNoError) {
        fprintf(stderr,"Pa_OpenStream failed: %s\n",Pa_GetErrorText(rc));
        exit(1);
    }

    rc=Pa_StartStream(stream);
    if(rc!=paNoError) {
        fprintf(stderr,"Pa_StartStream failed: %s\n",Pa_GetErrorText(rc));
        exit(1);
    }

    info=Pa_GetStreamInfo(stream);
    if(info!=NULL) {
        fprintf(stderr,"sample rate wanted=%d got=%f\n",SAMPLE_RATE,info->sampleRate);
    } else {
        fprintf(stderr,"Pa_GetStreamInfo returned NULL\n");
    }

    return 0;
}

int softrock_close() {
    int rc=Pa_Terminate();
    if(rc!=paNoError) {
        fprintf(stderr,"Pa_Terminate failed: %s\n",Pa_GetErrorText(rc));
        exit(1);
    }
}

int softrock_write(float* left_samples,float* right_samples) {
    int rc;
    int i;
    float audio_buffer[SAMPLES_PER_BUFFER*2];

    // interleave samples
    for(i=0;i<SAMPLES_PER_BUFFER;i++) {
        audio_buffer[i*2]=right_samples[i];
        audio_buffer[(i*2)+1]=left_samples[i];
    }

    rc=Pa_WriteStream(stream,audio_buffer,SAMPLES_PER_BUFFER);
    if(rc!=0) {
        fprintf(stderr,"error writing audio_buffer %s (rc=%d)\n",Pa_GetErrorText(rc),rc);
    }

    return rc;
}

int softrock_read(float* left_samples,float* right_samples) {
    int rc;
    int i;
    float audio_buffer[SAMPLES_PER_BUFFER*2];

    rc=Pa_ReadStream(stream,audio_buffer,SAMPLES_PER_BUFFER);
    if(rc!=0) {
        fprintf(stderr,"error reading audio_buffer %s (rc=%d)\n",Pa_GetErrorText(rc),rc);
    }

    // de-interleave samples
    for(i=0;i<SAMPLES_PER_BUFFER;i++) {
        right_samples[i]=audio_buffer[i*2];
        left_samples[i]=audio_buffer[(i*2)+1];
//fprintf(stderr,"%d left=%f right=%f\n",i, left_samples[i],right_samples[i]);
    }

    return rc;
}
