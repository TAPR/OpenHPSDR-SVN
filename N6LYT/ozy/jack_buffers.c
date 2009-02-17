/* 
 * File:   jack_buffers.c
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 05 January 2009, 16:30
 */

#include <stdio.h>
#include <stdlib.h>

#include "global.h"
#include "jack_buffers.h"

struct jack_buffer* jack_input_buffers_head;
struct jack_buffer* jack_input_buffers_tail;
sem_t jack_input_buffer_sem;
int jack_input_sequence=0;
int jack_input_buffer_underruns=0;

pthread_mutex_t jack_input_buffer_mutex;

struct jack_buffer* jack_free_buffers_head;
struct jack_buffer* jack_free_buffers_tail;
int jack_free_buffer_underruns=0;

pthread_mutex_t jack_free_buffer_mutex;

/*
 *
 */
void put_jack_free_buffer(struct jack_buffer* buffer) {
    if(debug_buffers) fprintf(stderr,"put_jack_free_buffer: %08X\n",(unsigned int)buffer);

    pthread_mutex_lock(&jack_free_buffer_mutex);
    if(jack_free_buffers_tail==NULL) {
        jack_free_buffers_head=jack_free_buffers_tail=buffer;
    } else {
        jack_free_buffers_tail->next=buffer;
        jack_free_buffers_tail=buffer;
    }
    pthread_mutex_unlock(&jack_free_buffer_mutex);
}

struct jack_buffer* get_jack_free_buffer(void) {
    struct jack_buffer* buffer;
    if(jack_free_buffers_head==NULL) {
        jack_free_buffer_underruns++;
        fprintf(stderr,"get_jack_free_buffer: underruns=%d\n",jack_input_buffer_underruns);
        return NULL;
    }

    pthread_mutex_lock(&jack_free_buffer_mutex);
    buffer=jack_free_buffers_head;
    jack_free_buffers_head=buffer->next;
    if(jack_free_buffers_head==NULL) {
        jack_free_buffers_tail=NULL;
    }
    pthread_mutex_unlock(&jack_free_buffer_mutex);

    if(debug_buffers) fprintf(stderr,"get_jack_free_buffer: %08X\n",(unsigned int)buffer);
    return buffer;
}

void put_jack_input_buffer(struct jack_buffer* buffer) {
    if(debug_buffers) fprintf(stderr,"put_jack_input_buffer: %d: %08X\n",jack_input_sequence,(unsigned int)buffer);
    pthread_mutex_lock(&jack_input_buffer_mutex);
    buffer->sequence=jack_input_sequence++;
    if(jack_input_buffers_tail==NULL) {
        jack_input_buffers_head=jack_input_buffers_tail=buffer;
    } else {
        jack_input_buffers_tail->next=buffer;
        jack_input_buffers_tail=buffer;
    }
    pthread_mutex_unlock(&jack_input_buffer_mutex);
}

struct jack_buffer* get_jack_input_buffer(void) {
    struct jack_buffer* buffer;
    if(jack_input_buffers_head==NULL) {
        jack_input_buffer_underruns++;
        fprintf(stderr,"get_jack_input_buffer: underruns=%d\n",jack_input_buffer_underruns);
        return NULL;
    }
    pthread_mutex_lock(&jack_input_buffer_mutex);
    buffer=jack_input_buffers_head;
    jack_input_buffers_head=buffer->next;
    if(jack_input_buffers_head==NULL) {
        jack_input_buffers_tail=NULL;
    }
    pthread_mutex_unlock(&jack_input_buffer_mutex);
    if(debug_buffers) fprintf(stderr,"get_jack_input_buffer: %d: %08X\n",buffer->sequence,(unsigned int)buffer);
    return buffer;
}

struct jack_buffer* new_jack_buffer() {
    struct jack_buffer* buffer;
    buffer=malloc(sizeof(struct jack_buffer));
    buffer->next=NULL;
    if(debug_buffers) fprintf(stderr,"new_jack_buffer: %08X\n",(unsigned int)buffer);
    return buffer;
}

void create_jack_buffers(int n) {
    struct jack_buffer* buffer;
    int i;

    if(debug) fprintf(stderr,"create_jack_buffers: entry: %d\n",n);

    pthread_mutex_init(&jack_input_buffer_mutex, NULL);
    pthread_mutex_init(&jack_free_buffer_mutex, NULL);

    for(i=0;i<n;i++) {
        buffer=new_jack_buffer();
        put_jack_free_buffer(buffer);
    }

    if(debug) fprintf(stderr,"create_jack_buffers: enxit\n");
}

void free_jack_buffer(struct jack_buffer* buffer) {
    if(debug_buffers) fprintf(stderr,"free_jack_buffer: %d: %08X\n",buffer->sequence,(unsigned int)buffer);
    
    put_jack_free_buffer(buffer);
}

