#include <stdio.h>
#include <stdlib.h>
#include <semaphore.h>

#include "portaudio.h"

#define CHANNELS 2
#define SAMPLE_RATE 48000
#define SAMPLES_PER_BUFFER 2048

static PaStream *stream;

static int insert=0;
static float audio_buffer[SAMPLES_PER_BUFFER*CHANNELS*2];
static sem_t audio_sem;

static int write_port_audio_thread_id;

static void write_port_audio_thread(void* arg) {
    int rc;

    while(1) {
        sem_wait(&audio_sem);
        rc=Pa_WriteStream(stream,audio_buffer,SAMPLES_PER_BUFFER);
        if(rc!=0) {
            fprintf(stderr,"error writing first audio_buffer rc=%d\n",rc);
        }

        sem_wait(&audio_sem);
        rc=Pa_WriteStream(stream,&audio_buffer[SAMPLES_PER_BUFFER],SAMPLES_PER_BUFFER);
        if(rc!=0) {
            fprintf(stderr,"error writing second audio_buffer rc=%d\n",rc);
        }
    }

}

void open_port_audio() {

    fprintf(stderr,"open_port_audio\n");

    int rc=Pa_Initialize();
    if(rc!=paNoError) {
        fprintf(stderr,"Pa_Initialize failed: %s\n",Pa_GetErrorText(rc));
        exit(1);
    }

    rc=Pa_OpenDefaultStream(&stream,0,CHANNELS,paFloat32,SAMPLE_RATE,SAMPLES_PER_BUFFER,
                            NULL, NULL);
    if(rc!=paNoError) {
        fprintf(stderr,"Pa_OpenDefaultStream failed: %s\n",Pa_GetErrorText(rc));
        exit(1);
    }

    sem_init (&audio_sem, 0, 0);
    rc=pthread_create(&write_port_audio_thread_id,NULL,write_port_audio_thread,NULL);
    if(rc!=0) {
        fprintf(stderr,"pthread_create failed on write_port_audio_thread: rc=%d\n",rc);
    }

    rc=Pa_StartStream(stream);
    if(rc!=paNoError) {
        fprintf(stderr,"Pa_StartStream failed: %s\n",Pa_GetErrorText(rc));
        exit(1);
    }

}

void close_port_audio() {
    int rc=Pa_Terminate();
    if(rc!=paNoError) {
        fprintf(stderr,"Pa_Terminate failed: %s\n",Pa_GetErrorText(rc));
        exit(1);
    }
}

int write_port_audio(float* left_samples,float* right_samples,int samples,int increment) {
    int i;

    for(i=0;i<samples;i+=increment) {
        audio_buffer[insert++]=left_samples[i];
        audio_buffer[insert++]=right_samples[i];
        if(insert==SAMPLES_PER_BUFFER*2) {
            sem_post(&audio_sem);
        } else if(insert==(SAMPLES_PER_BUFFER*4)) {
            insert=0;
            sem_post(&audio_sem);
        }
    }

}
