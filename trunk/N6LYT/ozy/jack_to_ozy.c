/* 
 * File:   jack_to_ozy.c
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 23 December 2008, 15:36
 */

#include <stdio.h>
#include <stdlib.h>
#include <semaphore.h>

#include "global.h"
#include "jack_ringbuffer.h"
#include "ozy_to_jack.h"
#include "jack_buffers.h"
#include "ozy_ringbuffer.h"
#include "ozy_client.h"

/*
 * 
 */

pthread_t jack_to_ozy_thread_id;

int out_index=0;

int state_out;
int data_state_out;

float left_rx_sample_float;
float right_rx_sample_float;
float left_tx_sample_float;
float right_tx_sample_float;
short left_rx_sample;
short right_rx_sample;
short left_tx_sample;
short right_tx_sample;

void process_jack_input_buffer(struct jack_buffer* buffer) {
    int i,b;
    char ozy_samples[8*8]; //
    for(i=0,b=0;i<buffer->nframes;i+=output_sample_increment) {
        left_rx_sample=(int)(buffer->buffer_1[i]*32767.0);
        right_rx_sample=(int)(buffer->buffer_2[i]*32767.0);
        left_tx_sample=(int)(buffer->buffer_3[i]*32767.0);
        right_tx_sample=(int)(buffer->buffer_4[i]*32767.0);
        ozy_samples[b++]=left_rx_sample>>8;
        ozy_samples[b++]=left_rx_sample;
        ozy_samples[b++]=right_rx_sample>>8;
        ozy_samples[b++]=right_rx_sample;
        ozy_samples[b++]=left_tx_sample>>8;
        ozy_samples[b++]=left_tx_sample;
        ozy_samples[b++]=right_tx_sample>>8;
        ozy_samples[b++]=right_tx_sample;
        if(b==64) {
            if(ozy_ringbuffer_put(ozy_output_buffer,ozy_samples,b)!=b) {
                // ozy output buffer overflow
            }
            b=0;
        }
        
    }
    
}

void* jack_to_ozy_thread(void* arg) {
    struct jack_buffer* jack_input_buffer;
    while(1) {
        // wait for an ozy buffer
        sem_wait(&jack_input_buffer_sem);
        if(debug_buffers) fprintf(stderr,"jack_to_ozy_thread: get_jack_input_buffer\n");
        jack_input_buffer=get_jack_input_buffer();
        if(jack_input_buffer==NULL) {
            fprintf(stderr,"jack_to_ozy_thread: get_jack_input_buffer returned NULL!\n");
        } else {
            process_jack_input_buffer(jack_input_buffer);
            free_jack_buffer(jack_input_buffer);
        }

    }
}

int create_jack_to_ozy() {
    int rc;
    if(debug) fprintf(stderr,"create_jack_to_ozy\n");

    rc=pthread_create(&jack_to_ozy_thread_id,NULL,jack_to_ozy_thread,NULL);
    if(rc != 0) {
        fprintf(stderr,"pthread_create failed on jack_to_ozy_thread: rc=%d\n", rc);
    }
}

