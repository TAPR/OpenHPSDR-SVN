/* 
 * File:   ozy_ringbuffer.c
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 05 January 2009, 09:07
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <pthread.h>

#include "ozy_buffers.h"
#include "ozy_ringbuffer.h"

/*
 * 
 */

struct ozy_ringbuffer* ozy_output_buffer;

pthread_mutex_t ozy_output_buffer_mutex; 

int ozy_put_bytes=0;
int ozy_get_bytes=0;

struct ozy_ringbuffer* new_ozy_ringbuffer(int n) {
    struct ozy_ringbuffer* buffer;

    buffer=malloc(sizeof(struct ozy_ringbuffer));
    if(buffer!=NULL) {
        buffer->size=n;
        buffer->entries=0;
        buffer->buffer=malloc(sizeof(char)*n);
        buffer->insert_index=0;
        buffer->remove_index=0;
    }
    return buffer;
}

int ozy_ringbuffer_space(struct ozy_ringbuffer* buffer) {
    return buffer->size-buffer->entries;
}

int ozy_ringbuffer_entries(struct ozy_ringbuffer* buffer) {
    return buffer->entries;
}

int ozy_ringbuffer_put(struct ozy_ringbuffer* buffer,char* f,int n) {
    int bytes;
    bytes=n;
    if(ozy_ringbuffer_space(buffer)<=n) {
fprintf(stderr,"ozy_ringbuffer_put: space=%d wanted=%d\n",ozy_ringbuffer_space(buffer),n);
        bytes=ozy_ringbuffer_space(buffer)-1;
    }
    ozy_put_bytes+=bytes;

    pthread_mutex_lock(&ozy_output_buffer_mutex);
    
    if(bytes>0) {

        if((buffer->insert_index+bytes)<=buffer->size) {
            // all together
            memcpy(&buffer->buffer[buffer->insert_index],f,bytes);
        } else {
            memcpy(&buffer->buffer[buffer->insert_index],f,buffer->size-buffer->insert_index);
            memcpy(buffer->buffer,&f[buffer->size-buffer->insert_index],bytes-(buffer->size-buffer->insert_index));
        }

        buffer->entries+=bytes;
        buffer->insert_index+=bytes;
        if(buffer->insert_index>=buffer->size) {
            buffer->insert_index=0;
        }
    }
    pthread_mutex_unlock(&ozy_output_buffer_mutex);

    return n;
}

int ozy_ringbuffer_get(struct ozy_ringbuffer* buffer,char* f,int n) {
    int entries;

    pthread_mutex_lock(&ozy_output_buffer_mutex);
    entries=n;
    if(buffer->entries<n) entries=buffer->entries;
    
    ozy_get_bytes+=entries;

    if((buffer->remove_index+entries)<=buffer->size) {
        // all together
        memcpy(f,&buffer->buffer[buffer->remove_index],entries);
    } else {
        memcpy(f,&buffer->buffer[buffer->remove_index],buffer->size-buffer->remove_index);
        memcpy(&f[buffer->size-buffer->remove_index],buffer->buffer,entries-(buffer->size-buffer->remove_index));
    }
    
    buffer->entries-=entries;
    buffer->remove_index+=entries;
    if(buffer->remove_index>=buffer->size) {
        buffer->remove_index-=buffer->size;
    }
    pthread_mutex_unlock(&ozy_output_buffer_mutex);

    return entries;
}

int create_ozy_ringbuffer(int n) {
    pthread_mutex_init(&ozy_output_buffer_mutex, NULL);
    ozy_output_buffer=new_ozy_ringbuffer(n);
}
