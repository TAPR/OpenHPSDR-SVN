/* 
 * File:   ozy_buffers.c
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 05 January 2009, 16:28
 */

#include <stdio.h>
#include <stdlib.h>
#include "ozy_buffers.h"

/*
 * 
 */
//#define MAX_BUFFERS 32
//#define MAX_OUTPUT_BUFFERS 34

struct ozy_buffer* ozy_input_buffers_head;
struct ozy_buffer* ozy_input_buffers_tail;
sem_t ozy_input_buffer_sem;
int ozy_input_sequence=0;
int ozy_input_buffers=0;

pthread_mutex_t ozy_input_buffer_mutex;

struct ozy_buffer* ozy_free_buffers_head;
struct ozy_buffer* ozy_free_buffers_tail;

pthread_mutex_t ozy_free_buffer_mutex;

void free_ozy_buffer(struct ozy_buffer* buffer);

void put_ozy_free_buffer(struct ozy_buffer* buffer) {

    pthread_mutex_lock(&ozy_free_buffer_mutex);
    if(ozy_free_buffers_tail==NULL) {
        ozy_free_buffers_head=ozy_free_buffers_tail=buffer;
    } else {
        ozy_free_buffers_tail->next=buffer;
        ozy_free_buffers_tail=buffer;
    }
    pthread_mutex_unlock(&ozy_free_buffer_mutex);
}

struct ozy_buffer* get_ozy_free_buffer(void) {
    struct ozy_buffer* buffer;
    if(ozy_free_buffers_head==NULL) {
        fprintf(stderr,"get_ozy_free_buffer: NULL\n");
        return NULL;
    }
    pthread_mutex_lock(&ozy_free_buffer_mutex);
    buffer=ozy_free_buffers_head;
    ozy_free_buffers_head=buffer->next;
    if(ozy_free_buffers_head==NULL) {
        ozy_free_buffers_tail=NULL;
    }
    buffer->size=OZY_BUFFER_SIZE;
    pthread_mutex_unlock(&ozy_free_buffer_mutex);
    return buffer;
}

void put_ozy_input_buffer(struct ozy_buffer* buffer) {

    pthread_mutex_lock(&ozy_input_buffer_mutex);
    buffer->sequence=ozy_input_sequence++;
    if(ozy_input_buffers_tail==NULL) {
        ozy_input_buffers_head=ozy_input_buffers_tail=buffer;
    } else {
        ozy_input_buffers_tail->next=buffer;
        ozy_input_buffers_tail=buffer;
    }
    pthread_mutex_unlock(&ozy_input_buffer_mutex);

}

struct ozy_buffer* get_ozy_input_buffer(void) {
    struct ozy_buffer* buffer;
    if(ozy_input_buffers_head==NULL) {
        fprintf(stderr,"get_ozy_input_buffer: NULL\n");
        return NULL;
    }
    pthread_mutex_lock(&ozy_input_buffer_mutex);
    buffer=ozy_input_buffers_head;
    ozy_input_buffers_head=buffer->next;
    if(ozy_input_buffers_head==NULL) {
        ozy_input_buffers_tail=NULL;
    }
    pthread_mutex_unlock(&ozy_input_buffer_mutex);
    return buffer;
}

struct ozy_buffer* new_ozy_buffer() {
    struct ozy_buffer* buffer;
    buffer=malloc(sizeof(struct ozy_buffer));
    buffer->next=NULL;
    buffer->size=OZY_BUFFER_SIZE;
    return buffer;
}

void create_ozy_buffers(int n) {
    struct ozy_buffer* buffer;
    int i;

    pthread_mutex_init(&ozy_input_buffer_mutex, NULL);
    pthread_mutex_init(&ozy_free_buffer_mutex, NULL);
    
    for(i=0;i<n;i++) {
        buffer=new_ozy_buffer();
        put_ozy_free_buffer(buffer);
    }

}

void free_ozy_buffer(struct ozy_buffer* buffer) {
    put_ozy_free_buffer(buffer);
}


