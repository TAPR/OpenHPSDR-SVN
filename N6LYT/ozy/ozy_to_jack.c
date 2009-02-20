/* 
 * File:   ozy_to_jack.c
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 22 December 2008, 17:09
 */

#include <stdio.h>
#include <stdlib.h>
#include <semaphore.h>

#include "global.h"
#include "jack_ringbuffer.h"
#include "ozy_buffers.h"
#include "ozy_to_jack.h"
#include "ozy_client.h"
#include "spectrum_buffers.h"

/*
 * OZY sends 512 byte frames
 * 
 */

pthread_t ozy_to_jack_thread_id;

struct jack_buffer* jack_buffer;
int jack_buffer_index;
int ozy_samples_in=0;
int ozy_total_samples_in=0;



int left_sample;
int right_sample;
int mic_sample;

float left_sample_float;
float right_sample_float;
float mic_sample_float;


struct spectrum_buffer* spectrum_buffer;
int adc_spectrum_index=0;
int collect_adc_spectrum=0;

int frame=0;

void process_ozy_input_buffer(char* buffer) {
    int i;
    int b=0;
    int f;
    if(debug) fprintf(stderr,"process_ozy_input_buffer frame=%d\n",frame++);

    // allow for multiple frames (i.e. bulk read multiple of 512 bytes)
    for(f=0;f<(OZY_BUFFER_SIZE/512);f++) {
        if(buffer[b++]==SYNC && buffer[b++]==SYNC && buffer[b++]==SYNC) {
            // extract control bytes
            control_in[0]=buffer[b++];
            control_in[1]=buffer[b++];
            control_in[2]=buffer[b++];
            control_in[3]=buffer[b++];
            control_in[4]=buffer[b++];

            // extract the 63 samples
            for(i=0;i<63;i++) {
                left_sample = buffer[b++] << 16;
                left_sample = left_sample + ((buffer[b++] & 0xFF) << 8);
                left_sample = left_sample + (buffer[b++] & 0xFF);
                right_sample = buffer[b++] << 16;
                right_sample = right_sample + ((buffer[b++] & 0xFF) << 8);
                right_sample = right_sample + (buffer[b++] & 0xFF);
                mic_sample=buffer[b++]<<8;
                mic_sample=mic_sample+(buffer[b++]&0xFF);

                left_sample_float=(float)left_sample/8388607.0; // 24 bit sample
                right_sample_float=(float)right_sample/8388607.0; // 24 bit sample
                mic_sample_float=(float)mic_sample/32767.0; // 16 bit sample

                //fprintf(stderr,"ozy_to_jack: %f %f %f\n",left_sample_float,right_sample_float,mic_sample_float);
                if(jack_ringbuffer_put(left_sample_float,right_sample_float,mic_sample_float,mic_sample_float)==0) {
                    fprintf(stderr,"jack_ringbuffer_put: overflow!\n");
                }
            }

        } else {
            fprintf(stderr,"ozy_to_jack: did not find sync f=%d\n",f);
        }
    }
}

void* ozy_to_jack_thread(void* arg) {
    struct ozy_buffer* buffer;
    while(1) {
        // wait for an ozy buffer
        sem_wait(&ozy_input_buffer_sem);
        if(debug_buffers) fprintf(stderr,"ozy_to_jack_thread: get_ozy_input_buffer\n");
        buffer=get_ozy_input_buffer();
        if(buffer==NULL) {
            fprintf(stderr,"ozy_to_jack_thread: get_ozy_buffer returned NULL!\n");
        } else {
            process_ozy_input_buffer(buffer->buffer);
            free_ozy_buffer(buffer);
        }

    }
}

int create_ozy_to_jack() {
    int rc;
    if(debug) fprintf(stderr,"create_ozy_to_jack\n");
    jack_buffer=NULL;
    rc=pthread_create(&ozy_to_jack_thread_id,NULL,ozy_to_jack_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on ozy_to_jack_thread: rc=%d\n", rc);
    }
}

